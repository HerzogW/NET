
namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using ICSharpCode.SharpZipLib.Checksums;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

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
        public List<ApiConfigurationData> LoadDataToCache()
        {
            var blobs = configContainer.ListBlobs();
            List<ApiConfigurationData> CacheData = new List<ApiConfigurationData>();
            var requestOption = new BlobRequestOptions() { RetryPolicy = new ExponentialRetry() };

            foreach (var apiZip in blobs.OfType<CloudBlockBlob>())
            {
                using (var blobStream = apiZip.OpenRead(null, requestOption))
                {
                    CacheData.Add(VeriliadteStream(blobStream, apiZip.Name));
                }
            }

            return CacheData;
        }

        /// <summary>
        /// Veriliadtes the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="apiFolderName">Name of the api folder.</param>
        public ApiConfigurationData VeriliadteStream(Stream stream, string apiFolderName)
        {
            ApiConfigurationData originalApiConfig;
            ErrorEntity errorEntity = new ErrorEntity();
            errorEntity.apiFolderName = apiFolderName;
            using (var zip = new ZipArchive(stream))
            {
                try
                {
                    errorEntity.jsonFileName = ApiItemFileName;
                    errorEntity.errorType = ErrorType.NotFound;
                    var apiItemJson = ReadZipEntryToString(zip.Entries.First(z => z.Name == ApiItemFileName));
                    errorEntity.errorType = ErrorType.CanNotDeserialize;

                    originalApiConfig = JsonConvert.DeserializeObject<ApiConfigurationData>(apiItemJson);
                    if (string.IsNullOrWhiteSpace(originalApiConfig.ApiTypeName))
                    {
                        ErrorEntity errorEntity2 = new ErrorEntity(ErrorType.NullValue, apiFolderName, ApiItemFileName, "item");
                        listError.Add(errorEntity2);
                    }

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
                            if (localizationDirectoryName == DefaultLanguage)
                            {
                                errorEntity.jsonFileName = string.Format(@"strings\{0}", ResourceFileName);
                            }
                            else
                            {
                                errorEntity.jsonFileName = string.Format(@"strings\{0}\{1}", localizationDirectoryName, ResourceFileName);
                            }
                            errorEntity.errorType = ErrorType.CanNotConvertToJson;

                            localizableStrings.Add(localizationDirectoryName, JsonConvert.DeserializeObject<Dictionary<string, string>>(ReadZipEntryToString(zipEntry)));
                        }
                    }

                    //若无Resource本地化配置信息，则默认添加英文en版本地化配置信息
                    if (localizableStrings.Count == 0)
                    {
                        ErrorEntity errorEntity2 = new ErrorEntity(ErrorType.NotFound, apiFolderName, ResourceFileName);
                        errorEntity2.errorStatus = ErrorStatus.Fixed;
                        listError.Add(errorEntity2);

                        localizableStrings.Add(DefaultLanguage, new Dictionary<string, string>());
                    }

                    originalApiConfig.Icons = icons;
                    originalApiConfig.Resources = localizableStrings;

                    originalApiConfig.HandleItems();

                    foreach (ErrorEntity errorInfo in originalApiConfig.listError)
                    {
                        errorInfo.apiFolderName = apiFolderName;
                        listError.Add(errorInfo);
                    }

                    return originalApiConfig;
                }
                catch (Exception ex)
                {
                    if (errorEntity.errorType != ErrorType.None)
                    {
                        errorEntity.exceptionMessage = ex.Message;
                        listError.Add(errorEntity);
                    }
                    return null;
                }
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
            var fullDir = entry.FullName.Replace(entry.Name, "").TrimEnd('\\');
            var currentDirName = fullDir.Substring(fullDir.LastIndexOf('\\') + 1);
            return currentDirName == StringsFolderName ? DefaultLanguage : currentDirName;
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

        public string ReadFileInfoToString(FileInfo file)
        {
            using (Stream stream = file.OpenRead())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Gets the configuration data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <param name="apiName">Name of the API.</param>
        /// <returns></returns>
        public static void SaveConfigurationDataToLocal(ApiConfigurationData configurationData, string tempFolderPath, string apiName)
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

                foreach (var resource in configurationData.Resources)
                {
                    string filePath = string.Format("{0}/{1}/{2}", apiName, "strings", ResourceFileName);
                    if (resource.Key != "en")
                    {
                        filePath = string.Format("{0}/{1}/{2}/{3}", apiName, "strings", resource.Key, ResourceFileName);
                    }

                    Compress(zipStream, JsonConvert.SerializeObject(resource.Value, new JsonSerializerSettings()
                    {
                        Formatting = Formatting.Indented,
                        NullValueHandling = NullValueHandling.Ignore
                    }), filePath);
                }

                foreach (var entity in configurationData.Icons)
                {
                    Compress(zipStream, entity.Value, string.Format("{0}/{1}/{2}", apiName, "icons", entity.Key));
                }
            }
        }

        /// <summary>
        /// Compresses the specified zip stream.
        /// </summary>
        /// <param name="zipStream">The zip stream.</param>
        /// <param name="data">The data.</param>
        /// <param name="filePath">The file path.</param>
        private static void Compress(ICSharpCode.SharpZipLib.Zip.ZipOutputStream zipStream, string data, string filePath)
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


        public static string CompressFolderContentIntoStream(string sourceFolder, string tempFolderPath)
        {
            string folderName = sourceFolder.Substring(sourceFolder.LastIndexOf(@"\") + 1);
            string tempZipFileName = string.Format("{0}\\{1}.zip", tempFolderPath, folderName);
            if (Directory.Exists(tempFolderPath))
            {
                Directory.Delete(tempFolderPath, true);
            }

            Directory.CreateDirectory(tempFolderPath);

            using (ICSharpCode.SharpZipLib.Zip.ZipOutputStream zipStream = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(File.Create(tempZipFileName)))
            {
                zipStream.SetLevel(6);
                Zip(sourceFolder, zipStream, sourceFolder);
            }

            return tempZipFileName;
        }

        private static void Zip(string parentPath, ICSharpCode.SharpZipLib.Zip.ZipOutputStream zipStream, string rootPath)
        {
            Crc32 crc = new Crc32();

            string[] files = Directory.GetFileSystemEntries(parentPath);
            foreach (string file in files)
            {
                if (Directory.Exists(file))
                {
                    Zip(file, zipStream, rootPath);
                }
                else
                {
                    using (StreamReader reader = new StreamReader(file))
                    {
                        byte[] buffer = System.Text.Encoding.Default.GetBytes(reader.ReadToEnd());
                        string tempFileName = file.Substring(rootPath.LastIndexOf("\\") + 1);
                        ICSharpCode.SharpZipLib.Zip.ZipEntry entry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(tempFileName);
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
        }
    }
}
