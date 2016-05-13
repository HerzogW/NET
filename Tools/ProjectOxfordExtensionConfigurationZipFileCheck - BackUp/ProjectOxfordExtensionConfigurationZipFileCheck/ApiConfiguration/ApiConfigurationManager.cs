
namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using ICSharpCode.SharpZipLib;
    using ICSharpCode.SharpZipLib.Checksums;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiConfigurationManager"/> class.
        /// </summary>
        public ApiConfigurationManager()
        {

        }

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
        /// Gets the cloud block BLOB.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IListBlobItem> GetCloudBlockBlob()
        {
            return configContainer.ListBlobs();
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
                VeriliadteStream(blobStream, apiZipBlob.Name, true);
            }
        }

        /// <summary>
        /// Veriliadtes the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="zipFileName">Name of the zip file.</param>
        /// <param name="cacheData">if set to <c>true</c> [cache data].</param>
        public void VeriliadteStream(Stream stream, string zipFileName = null, bool cacheData = false)
        {
            ErrorEntity errorEntity = new ErrorEntity();
            errorEntity.zipFileName = zipFileName;
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

                    errorEntity.errorType = ErrorType.None;

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
                            string localizationDirectoryName = GetLocalizationDirectoryName(zipEntry);
                            errorEntity.jsonFileName = string.Format("{0}{1}", localizationDirectoryName, ResourceFileName);
                            errorEntity.errorType = ErrorType.CanNotConvertToJson;

                            localizableStrings.Add(localizationDirectoryName, JsonConvert.DeserializeObject<Dictionary<string, string>>(ReadZipEntryToString(zipEntry)));
                        }
                    }

                    originalApiConfig.Icons = icons;
                    originalApiConfig.Resources = localizableStrings;

                    if (cacheData)
                    {
                        var tempApiConfig = new ApiConfigurationData();
                        tempApiConfig.LocaleId = originalApiConfig.LocaleId;
                        tempApiConfig.ApiTypeName = originalApiConfig.ApiTypeName;
                        tempApiConfig.ApiItem = originalApiConfig.ApiItem;
                        tempApiConfig.Spec = originalApiConfig.Spec;
                        tempApiConfig.QuickStart = originalApiConfig.QuickStart;

                        tempApiConfig.Icons = originalApiConfig.Icons;
                        tempApiConfig.Resources = originalApiConfig.Resources;

                        CacheData.Add(tempApiConfig);
                    }

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
                            zipFileName = zipFileName,
                            errorType = ErrorType.LostResource,
                            resourceName = ResourceFileName
                        });
                    }

                    foreach (ErrorEntity errorInfo in apiConfigurationData.listError)
                    {
                        errorInfo.zipFileName = zipFileName;
                        listError.Add(errorInfo);
                    }
                }
                catch (Exception ex)
                {
                    if (errorEntity.errorType != ErrorType.None)
                    {
                        errorEntity.errorMessage = ex.Message;
                        listError.Add(errorEntity);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the original data.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public ApiConfigurationData HandleOriginalData(Stream stream)
        {
            ApiConfigurationData configurationData = new ApiConfigurationData();

            ErrorEntity entity = new ErrorEntity();
            entity.errorType = ErrorType.CanNotConvertToJson;
            try
            {
                using (var zip = new ZipArchive(stream))
                {
                    entity.jsonFileName = ApiItemFileName;
                    var apiItemJson = ReadZipEntryToString(zip.Entries.First(z => z.Name == ApiItemFileName));
                    configurationData.ApiItem = JObject.Parse(apiItemJson);

                    entity.jsonFileName = SpecFileName;
                    var specJson = ReadZipEntryToString(zip.Entries.First(z => z.Name == SpecFileName));
                    configurationData.Spec = JObject.Parse(specJson);

                    entity.jsonFileName = QuickStartsFileName;
                    var quickStartJson = ReadZipEntryToString(zip.Entries.First(z => z.Name == QuickStartsFileName));
                    configurationData.QuickStart = JObject.Parse(quickStartJson);

                    configurationData.HandleItems();
                }
            }
            catch (Exception ex)
            {
                entity.errorMessage = ex.Message;
                configurationData.listError.Add(entity);
            }

            return configurationData;
        }

        /// <summary>
        /// Gets the configuration data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <param name="apiName">Name of the API.</param>
        /// <returns></returns>
        public void GetConfigurationData(ApiConfigurationData configurationData, string tempFolderPath, string apiName)
        {
            string tempZipFileName = string.Format("{0}\\{1}.zip", tempFolderPath, apiName);
            if (Directory.Exists(tempFolderPath))
            {
                Directory.Delete(tempFolderPath, true);
            }
            Directory.CreateDirectory(tempFolderPath);

            using (ICSharpCode.SharpZipLib.Zip.ZipOutputStream zipStream = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(File.Create(tempZipFileName)))
            {
                zipStream.SetLevel(6);

                Compress(zipStream, configurationData.ApiItem.ToString(), string.Format("{0}/{1}", apiName, ApiItemFileName));
                Compress(zipStream, configurationData.Spec.ToString(), string.Format("{0}/{1}", apiName, SpecFileName));
                Compress(zipStream, configurationData.QuickStart.ToString(), string.Format("{0}/{1}", apiName, QuickStartsFileName));
                Compress(zipStream, JsonConvert.SerializeObject(configurationData.Resources["en"], new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                }), string.Format("{0}/{1}/{2}", apiName, "strings", ResourceFileName));

                foreach (var entity in configurationData.Icons)
                {
                    Compress(zipStream, entity.Value, string.Format("{0}/{1}/{2}", apiName, "icons", entity.Key));
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

        /// <summary>
        /// Writes the data to local file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="data">The data.</param>
        private void WriteDataToLocalFile(string filePath, string data)
        {
            using (FileStream fs = File.Create(filePath))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine(data);
                }
            }
        }

        /// <summary>
        /// Compresses the specified zip stream.
        /// </summary>
        /// <param name="zipStream">The zip stream.</param>
        /// <param name="data">The data.</param>
        /// <param name="filePath">The file path.</param>
        private void Compress(ICSharpCode.SharpZipLib.Zip.ZipOutputStream zipStream, string data, string filePath)
        {
            Crc32 crc = new Crc32();
            byte[] buffer = System.Text.Encoding.Default.GetBytes(data);

            ICSharpCode.SharpZipLib.Zip.ZipEntry entry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(filePath);
            entry.DateTime = DateTime.Now;
            entry.Size = buffer.Length;
            crc.Reset();
            crc.Update(buffer);
            entry.Crc = crc.Value;
            zipStream.PutNextEntry(entry);
            zipStream.Write(buffer, 0, buffer.Length);
        }
    }
}
