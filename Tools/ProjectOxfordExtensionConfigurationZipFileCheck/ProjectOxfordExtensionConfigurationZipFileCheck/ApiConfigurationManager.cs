

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
        /// The Api configurations: Dictionary[apiName, Dictionary[localeId, ApiConfigurationData]]
        /// </summary>
        private volatile Dictionary<string, Dictionary<string, ApiConfigurationData>> apiConfigurations;

        /// <summary>
        /// The configuration container
        /// </summary>
        private CloudBlobContainer configContainer;

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
        /// Gets the apis configuration.
        /// </summary>
        /// <value>
        /// The Api configuration.
        /// </value>
        public Dictionary<string, Dictionary<string, ApiConfigurationData>> DataCache
        {
            get
            {
                return apiConfigurations;
            }
        }

        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <exception cref="System.InvalidOperationException"></exception>
        public List<string> LoadDataToCache()
        {
            List<string> listError = new List<string>();

            var blobs = configContainer.ListBlobs();

            foreach (var apiZip in blobs.OfType<CloudBlockBlob>())
            {
                List<string> errors = DeserializeApiConfig(apiZip);

                foreach (string errorMessage in errors)
                {
                    listError.Add(errorMessage);
                }
            }
            return listError;
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
        private List<String> DeserializeApiConfig(CloudBlockBlob apiZipBlob)
        {
            List<string> listError = new List<string>();
            var requestOption = new BlobRequestOptions() { RetryPolicy = new ExponentialRetry() };

            using (var blobStream = apiZipBlob.OpenRead(null, requestOption))
            {
                listError = VeriliadteStream(blobStream, apiZipBlob.Name);
            }

            return listError;
        }

        /// <summary>
        /// Veriliadtes the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="fileName">Name of the file.</param>
        public List<string> VeriliadteStream(Stream stream, string fileName = null)
        {
            List<String> listError = new List<String>();
            string errorMessage = "";
            using (var zip = new ZipArchive(stream))
            {
                try
                {
                    errorMessage = string.Format("【File】{0} not found", ApiItemFileName);
                    var apiItemJson = ReadZipEntryToString(zip.Entries.First(z => z.Name == ApiItemFileName));
                    var apiItem = JObject.Parse(apiItemJson);
                    var originalApiConfig = JsonConvert.DeserializeObject<ApiConfigurationData>(apiItemJson);
                    errorMessage = string.Format("【File】{0} not found", SpecFileName);
                    originalApiConfig.Spec = JObject.Parse(ReadZipEntryToString(zip.Entries.First(z => z.Name == SpecFileName)));
                    errorMessage = string.Format("【File】{0} not found", QuickStartsFileName);
                    originalApiConfig.QuickStart = JObject.Parse(ReadZipEntryToString(zip.Entries.First(z => z.Name == QuickStartsFileName)));
                    originalApiConfig.ApiItem = apiItem;
                    errorMessage = "";

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



                    ApiConfigurationData apiConfigurationData = new ApiConfigurationData();
                    // repair icons
                    apiConfigurationData.ReplaceIcons(originalApiConfig, icons);

                    var localizedConfigs = new Dictionary<string, ApiConfigurationData>();

                    if (localizableStrings.Count != 0)
                    {
                        // catch up all locales resources.
                        foreach (var localeString in localizableStrings)
                        {
                            localizedConfigs.Add(localeString.Key, apiConfigurationData.GetLocalized(originalApiConfig, localeString.Key, localeString.Value, localizableStrings[DefaultLanguage]));
                        }
                    }
                    else
                    {
                        listError.Add(string.Format("Zip file {0}:    【File】{1} is not found.", fileName, ResourceFileName));
                    }

                    foreach (string errorMessage2 in apiConfigurationData.listError)
                    {
                        listError.Add(string.Format("Zip file {0}:    【Resource】{1} is not found.", fileName, errorMessage2));
                    }

                    return listError;
                }
                catch (Exception ex)
                {
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        listError.Add(string.Format("Zip file {0}:    {1}", fileName, errorMessage));
                    }
                    else
                    {
                        listError.Add(string.Format("Zip file {0}:    {1}", fileName, ex.Message));
                    }
                    return listError;
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
