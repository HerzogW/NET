

namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// The Api configuration manager.
    /// </summary>
    public class ApiConfigurationManager
    {
        /// <summary>
        /// The default language
        /// </summary>
        public const string DefaultLanguage = "en";

        /// <summary>
        /// The API configuration file name
        /// </summary>
        private const string ApiItemFileName = "api.json";

        /// <summary>
        /// The quick starts file name
        /// </summary>
        private const string QuickStartsFileName = "quickStarts.json";

        /// <summary>
        /// The spec file name
        /// </summary>
        private const string SpecFileName = "spec.json";

        /// <summary>
        /// The resource file name
        /// </summary>
        private const string ResourceFileName = "resources.resjson";

        /// <summary>
        /// The icons folder name
        /// </summary>
        private const string IconsFolderName = "icons";

        /// <summary>
        /// The strings folder name
        /// </summary>
        private const string StringsFolderName = "strings";

        /// <summary>
        /// The icon file extension
        /// </summary>
        private const string IconFileExtension = ".svg";

        /// <summary>
        /// The configuration container
        /// </summary>
        private CloudBlobContainer configContainer;

        /// <summary>
        /// The list error
        /// </summary>
        public List<ErrorEntity> listError = new List<ErrorEntity>();

        /// <summary>
        /// The cache data
        /// </summary>
        public List<ApiConfigurationData> CacheData = new List<ApiConfigurationData>();

        public ApiConfigurationManager()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiConfigurationManager" /> class.
        /// </summary>
        /// <param name="connString">The connection string.</param>
        /// <param name="container">The container.</param>
        /// <param name="apiConfigCacheRefreshInterval">The API configuration cache refresh interval.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public ApiConfigurationManager(string connString, string container)
        {
            SetupBlobConnection(connString, container);
        }

        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <exception cref="System.InvalidOperationException"></exception>
        public void LoadDataToCache()
        {
            var blobs = configContainer.ListBlobs();

            foreach (var apiZip in blobs.OfType<CloudBlockBlob>())
            {
                DeserializeApiConfig(apiZip);
            }
        }

        /// <summary>
        /// Setups the BLOB connection.
        /// </summary>
        /// <param name="connString">The connection string.</param>
        /// <param name="container">The container.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        private void SetupBlobConnection(string connString, string container)
        {
            var cloudBlobClient = new CloudBlobClient(new Uri(connString));

            configContainer = cloudBlobClient.GetContainerReference(container);

            if (!configContainer.Exists())
            {
                throw new InvalidOperationException(string.Format("Api configuration blob container '{0}' doesn't exist.", configContainer));
            }
        }

        /// <summary>
        /// Gets the name of the current directory.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>The DIR name</returns>
        private string GetLocalizationDirectoryName(ZipArchiveEntry entry)
        {
            var fullDir = entry.FullName.Replace(entry.Name, "").TrimEnd('/');
            var currentDirName = fullDir.Substring(fullDir.LastIndexOf('/') + 1);
            return currentDirName == StringsFolderName ? DefaultLanguage : currentDirName;
        }

        /// <summary>
        /// Deserializes the API configuration.
        /// </summary>
        /// <param name="apiZipBlob">The API directory.</param>
        /// <returns></returns>
        private void DeserializeApiConfig(CloudBlockBlob apiZipBlob)
        {
            List<ErrorEntity> listError = new List<ErrorEntity>();
            var requestOption = new BlobRequestOptions() { RetryPolicy = new ExponentialRetry() };

            using (var blobStream = apiZipBlob.OpenRead(null, requestOption))
            {
                VeriliadteStream(blobStream, apiZipBlob.Name);
            }
        }

        /// <summary>
        /// Veriliadtes the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="fileName">Name of the file.</param>
        public void VeriliadteStream(Stream stream, string fileName = null)
        {
            ErrorEntity errorEntity = new ErrorEntity();
            errorEntity.zipFileName = fileName;
            using (var zip = new ZipArchive(stream))
            {
                try
                {
                    errorEntity.jsonFileName = ApiItemFileName;
                    errorEntity.errorType = ErrorType.NotFound;
                    var apiItemJson = ReadZipEntryToString(zip.Entries.First(z => z.Name == ApiItemFileName));
                    errorEntity.errorType = ErrorType.CanNotDeserialize;
                    var originalApiConfig = JsonConvert.DeserializeObject<ApiConfigurationData>(apiItemJson);

                    errorEntity.errorType = ErrorType.CanNotConvertToJson;
                    var apiItem = JObject.Parse(apiItemJson);
                    originalApiConfig.ApiItem = apiItem;

                    errorEntity.jsonFileName = SpecFileName;
                    errorEntity.errorType = ErrorType.NotFound;
                    var specItemJson = ReadZipEntryToString(zip.Entries.First(z => z.Name == SpecFileName));
                    errorEntity.errorType = ErrorType.CanNotConvertToJson;
                    originalApiConfig.Spec = JObject.Parse(specItemJson);

                    errorEntity.jsonFileName = QuickStartsFileName;
                    errorEntity.errorType = ErrorType.NotFound;
                    var quickStartItemJson = ReadZipEntryToString(zip.Entries.First(z => z.Name == QuickStartsFileName));
                    errorEntity.errorType = ErrorType.CanNotConvertToJson;
                    originalApiConfig.QuickStart = JObject.Parse(quickStartItemJson);

                    var icons = new Dictionary<string, string>();

                    var localizableStrings = new Dictionary<string, Dictionary<string, string>>();

                    foreach (var zipEntry in zip.Entries)
                    {
                        if (zipEntry.Name.EndsWith(IconFileExtension))
                        {
                            icons.Add(zipEntry.Name, ReadZipEntryToString(zipEntry));
                        }

                        if (zipEntry.Name == ResourceFileName)
                        {
                            localizableStrings.Add(GetLocalizationDirectoryName(zipEntry), JsonConvert.DeserializeObject<Dictionary<string, string>>(ReadZipEntryToString(zipEntry)));
                        }
                    }

                    originalApiConfig.Icons = icons;
                    originalApiConfig.Resources = localizableStrings;

                    var tempApiConfig = new ApiConfigurationData();
                    tempApiConfig.LocaleId = originalApiConfig.LocaleId;
                    tempApiConfig.ApiTypeName = originalApiConfig.ApiTypeName;
                    tempApiConfig.ApiItem = originalApiConfig.ApiItem;
                    tempApiConfig.Spec = originalApiConfig.Spec;
                    tempApiConfig.QuickStart = originalApiConfig.QuickStart;

                    tempApiConfig.Icons = originalApiConfig.Icons;
                    tempApiConfig.Resources = originalApiConfig.Resources;


                    CacheData.Add(tempApiConfig);

                    ApiConfigurationData apiConfigurationData = new ApiConfigurationData();

                    apiConfigurationData.ReplaceIcons(originalApiConfig, icons);

                    var localizedConfigs = new Dictionary<string, ApiConfigurationData>();

                    if (localizableStrings.Count != 0)
                    {
                        foreach (var localeString in localizableStrings)
                        {
                            localizedConfigs.Add(localeString.Key, apiConfigurationData.GetLocalized(originalApiConfig, localeString.Key, localeString.Value, localizableStrings[DefaultLanguage]));
                        }
                    }
                    else
                    {
                        listError.Add(new ErrorEntity()
                        {
                            zipFileName = fileName,
                            errorType = ErrorType.LostResource,
                            resourceName = ResourceFileName
                        });
                    }

                    foreach (ErrorEntity errorInfo in apiConfigurationData.listError)
                    {
                        errorInfo.zipFileName = fileName;
                        listError.Add(errorInfo);
                    }
                }
                catch (Exception ex)
                {
                    errorEntity.errorMessage = ex.Message;
                    listError.Add(errorEntity);
                }
            }
        }

        /// <summary>
        /// Reads the zip entry to string.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns></returns>
        private string ReadZipEntryToString(ZipArchiveEntry entry)
        {
            using (var stream = entry.Open())
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
