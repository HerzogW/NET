
namespace ApiOnBoardingConfigurationTool
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
    using ExtensionConfigurationEntity;

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
        public const string ApiItemFileName = "api.json";

        /// <summary>
        /// The quick starts file name
        /// </summary>
        public const string QuickStartsFileName = "quickStarts.json";

        /// <summary>
        /// The spec file name
        /// </summary>
        public const string SpecFileName = "spec.json";

        /// <summary>
        /// The resource file name
        /// </summary>
        public const string ResourceFileName = "resources.resjson";

        /// <summary>
        /// The icons folder name
        /// </summary>
        public const string IconsFolderName = "icons";

        /// <summary>
        /// The strings folder name
        /// </summary>
        public const string StringsFolderName = "strings";

        /// <summary>
        /// The icon file extension
        /// </summary>
        public const string IconFileExtension = ".svg";

        /// <summary>
        /// The special setting format
        /// </summary>
        private static JsonSerializerSettings specialSettingFormat = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

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
                    CacheData.Add(VeriliadteStream(blobStream, apiZip.Name.Replace(".zip", "")));
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

                    originalApiConfig.ApiFolderName = apiFolderName;
                    errorEntity.errorType = ErrorType.CanNotConvertToJson;
                    originalApiConfig.ApiItem = JsonConvert.DeserializeObject<ApiItemEntity>(apiItemJson);

                    errorEntity.jsonFileName = SpecFileName;
                    errorEntity.errorType = ErrorType.NotFound;
                    var specItemJson = ReadZipEntryToString(zip.Entries.First(z => z.Name == SpecFileName));
                    errorEntity.errorType = ErrorType.CanNotConvertToJson;
                    originalApiConfig.Spec = JsonConvert.DeserializeObject<SpecsEntity>(specItemJson);

                    errorEntity.jsonFileName = QuickStartsFileName;
                    errorEntity.errorType = ErrorType.NotFound;
                    var quickStartItemJson = ReadZipEntryToString(zip.Entries.First(z => z.Name == QuickStartsFileName));
                    errorEntity.errorType = ErrorType.CanNotConvertToJson;
                    originalApiConfig.QuickStart = JsonConvert.DeserializeObject<QuickStartsEntity>(quickStartItemJson);

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
            var fullDir = entry.FullName.Replace("\\", "/").Replace(entry.Name, "").TrimEnd('/');
            var currentDirName = fullDir.Substring(fullDir.LastIndexOf('/') + 1);
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

        /// <summary>
        /// Gets the configuration data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <param name="apiFolderName">Name of the API Folder.</param>
        /// <returns></returns>
        public static void SaveConfigurationDataToLocalZip(ApiConfigurationData configurationData, string tempFolderPath, string apiFolderName = null)
        {
            string targetFileOrFolderName = string.Empty;
            if (!string.IsNullOrWhiteSpace(apiFolderName))
            {
                targetFileOrFolderName = apiFolderName;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(configurationData.ApiFolderName))
                {
                    targetFileOrFolderName = configurationData.ApiFolderName;
                }
                else
                {
                    targetFileOrFolderName = configurationData.ApiTypeName;
                }
            }

            string tempZipFileName = string.Format("{0}\\{1}.zip", tempFolderPath, targetFileOrFolderName);

            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }

            if (File.Exists(tempZipFileName))
            {
                File.Delete(tempZipFileName);
            }

            using (ICSharpCode.SharpZipLib.Zip.ZipOutputStream zipStream = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(File.Create(tempZipFileName)))
            {
                zipStream.SetLevel(6);

                Compress(zipStream, JsonConvert.SerializeObject(configurationData.ApiItem, specialSettingFormat), string.Format("{0}/{1}", targetFileOrFolderName, ApiItemFileName));
                Compress(zipStream, JsonConvert.SerializeObject(configurationData.Spec, specialSettingFormat), string.Format("{0}/{1}", targetFileOrFolderName, SpecFileName));
                Compress(zipStream, JsonConvert.SerializeObject(configurationData.QuickStart, specialSettingFormat), string.Format("{0}/{1}", targetFileOrFolderName, QuickStartsFileName));

                foreach (var resource in configurationData.Resources)
                {
                    string filePath = string.Format("{0}/{1}/{2}", targetFileOrFolderName, "strings", ResourceFileName);
                    if (resource.Key != "en")
                    {
                        filePath = string.Format("{0}/{1}/{2}/{3}", targetFileOrFolderName, "strings", resource.Key, ResourceFileName);
                    }

                    Compress(zipStream, JsonConvert.SerializeObject(resource.Value, new JsonSerializerSettings()
                    {
                        Formatting = Formatting.Indented,
                        NullValueHandling = NullValueHandling.Ignore
                    }), filePath);
                }

                foreach (var entity in configurationData.Icons)
                {
                    Compress(zipStream, entity.Value, string.Format("{0}/{1}/{2}", targetFileOrFolderName, "icons", entity.Key));
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

        /// <summary>
        /// Saves the configuration data to local folder.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <param name="tempFolderPath">The temporary folder path.</param>
        public static void SaveConfigurationDataToLocalFolder(ApiConfigurationData configurationData, string tempFolderPath)
        {
            try
            {
                string targetFileOrFolderName = string.Empty;
                if (!string.IsNullOrWhiteSpace(configurationData.ApiFolderName))
                {
                    targetFileOrFolderName = configurationData.ApiFolderName;
                }
                else
                {
                    targetFileOrFolderName = configurationData.ApiTypeName;
                }

                string targetApiFolderPath = string.Format(@"{0}\{1}", tempFolderPath, targetFileOrFolderName);
                string targetApiIconsFolderPath = string.Format(@"{0}\{1}", targetApiFolderPath, "icons");
                string targetApiStringsFolderPath = string.Format(@"{0}\{1}", targetApiFolderPath, "strings");
                if (Directory.Exists(targetApiFolderPath))
                {
                    Directory.Delete(targetApiFolderPath, true);
                }

                Directory.CreateDirectory(targetApiFolderPath);
                Directory.CreateDirectory(targetApiIconsFolderPath);
                Directory.CreateDirectory(targetApiStringsFolderPath);

                if (configurationData != null)
                {
                    SaveContentStringToLocalFile(targetApiFolderPath, "api.json", JsonConvert.SerializeObject(configurationData.ApiItem, specialSettingFormat));
                    SaveContentStringToLocalFile(targetApiFolderPath, "quickStarts.json", JsonConvert.SerializeObject(configurationData.QuickStart, specialSettingFormat));
                    SaveContentStringToLocalFile(targetApiFolderPath, "spec.json", JsonConvert.SerializeObject(configurationData.Spec, specialSettingFormat));

                    foreach (var icon in configurationData.Icons)
                    {
                        SaveContentStringToLocalFile(targetApiIconsFolderPath, icon.Key, icon.Value);
                    }

                    foreach (var resource in configurationData.Resources)
                    {
                        if (resource.Key.Equals("en"))
                        {
                            SaveContentStringToLocalFile(targetApiStringsFolderPath, "resources.resjson", JsonConvert.SerializeObject(resource.Value));
                        }
                        else
                        {
                            string targetotherLanguageResourceFolderPath = string.Format(@"{0}\{1}", targetApiStringsFolderPath, resource.Key);
                            Directory.CreateDirectory(targetotherLanguageResourceFolderPath);
                            SaveContentStringToLocalFile(targetotherLanguageResourceFolderPath, "resources.resjson", JsonConvert.SerializeObject(resource.Value));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Saves the content string to local file.
        /// </summary>
        /// <param name="targetFolderPath">The target folder path.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="content">The content.</param>
        public static void SaveContentStringToLocalFile(string targetFolderPath, string fileName, string content)
        {
            if (!Directory.Exists(targetFolderPath))
            {
                Directory.CreateDirectory(targetFolderPath);
            }

            string filePath = string.Format(@"{0}\{1}", targetFolderPath, fileName);

            using (FileStream fs = File.Open(filePath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(content);
                }
            }
        }

        /// <summary>
        /// Compresses the folder content into stream.
        /// </summary>
        /// <param name="sourceFolder">The source folder.</param>
        /// <param name="tempFolderPath">The temporary folder path.</param>
        /// <returns></returns>
        public static string CompressFolderContentIntoStream(string sourceFolder, string tempFolderPath)
        {
            string folderName = sourceFolder.Substring(sourceFolder.LastIndexOf(@"\") + 1);
            string tempZipFileName = string.Format("{0}\\{1}.zip", tempFolderPath, folderName);
            if (!Directory.Exists(tempFolderPath))
            {

                Directory.CreateDirectory(tempFolderPath);
            }

            if (File.Exists(tempZipFileName))
            {
                File.Delete(tempZipFileName);
            }

            using (ICSharpCode.SharpZipLib.Zip.ZipOutputStream zipStream = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(File.Create(tempZipFileName)))
            {
                zipStream.SetLevel(6);
                Zip(sourceFolder, zipStream, sourceFolder);
            }

            return tempZipFileName;
        }

        /// <summary>
        /// Zips the specified parent path.
        /// </summary>
        /// <param name="parentPath">The parent path.</param>
        /// <param name="zipStream">The zip stream.</param>
        /// <param name="rootPath">The root path.</param>
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

        /// <summary>
        /// Loads the singel API configuration form load folder path.
        /// </summary>
        /// <param name="localFolderPath">The local folder path.</param>
        /// <returns></returns>
        public static ApiConfigurationData LoadSingelApiConfigurationFormLoadFolderPath(string localFolderPath)
        {
            string apiFolderName = localFolderPath.Substring(localFolderPath.LastIndexOf(@"\") + 1);

            ApiConfigurationData apiconfigData = new ApiConfigurationData();
            apiconfigData.ApiFolderName = apiFolderName;

            string apiJsonPath = string.Format(@"{0}\{1}", localFolderPath, ApiItemFileName);
            string quickStartsJsonPath = string.Format(@"{0}\{1}", localFolderPath, QuickStartsFileName);
            string specJsonPath = string.Format(@"{0}\{1}", localFolderPath, SpecFileName);

            string iconFolderPath = string.Format(@"{0}\{1}", localFolderPath, IconsFolderName);
            string stringsFolderPath = string.Format(@"{0}\{1}", localFolderPath, StringsFolderName);

            string apiJsonContent = ReadLocalFileToString(apiJsonPath);
            if (!string.IsNullOrWhiteSpace(apiJsonContent))
            {
                try
                {
                    apiconfigData.ApiItem = JsonConvert.DeserializeObject<ApiItemEntity>(apiJsonContent, specialSettingFormat);
                    apiconfigData.ApiTypeName = apiconfigData.ApiItem.item;
                }
                catch (Exception ex)
                {
                    ErrorEntity entity = new ErrorEntity(ErrorType.CanNotDeserialize, apiFolderName, ApiItemFileName);
                    entity.exceptionMessage = ex.Message;
                    apiconfigData.listError.Add(entity);
                }
            }
            else
            {
                ErrorEntity entity = new ErrorEntity(ErrorType.NullValue, apiFolderName, ApiItemFileName);
                apiconfigData.listError.Add(entity);
            }

            string quickStartsJsonContent = ReadLocalFileToString(quickStartsJsonPath);
            if (!string.IsNullOrWhiteSpace(quickStartsJsonContent))
            {
                try
                {
                    apiconfigData.QuickStart = JsonConvert.DeserializeObject<QuickStartsEntity>(quickStartsJsonContent, specialSettingFormat);
                }
                catch (Exception ex)
                {
                    ErrorEntity entity = new ErrorEntity(ErrorType.CanNotDeserialize, apiFolderName, QuickStartsFileName);
                    entity.exceptionMessage = ex.Message;
                    apiconfigData.listError.Add(entity);
                }
            }
            else
            {
                ErrorEntity entity = new ErrorEntity(ErrorType.NullValue, apiFolderName, QuickStartsFileName);
                apiconfigData.listError.Add(entity);
            }

            string specJsonContent = ReadLocalFileToString(specJsonPath);
            if (!string.IsNullOrWhiteSpace(specJsonContent))
            {
                try
                {
                    apiconfigData.Spec = JsonConvert.DeserializeObject<SpecsEntity>(specJsonContent, specialSettingFormat);
                }
                catch (Exception ex)
                {
                    ErrorEntity entity = new ErrorEntity(ErrorType.CanNotDeserialize, apiFolderName, SpecFileName);
                    entity.exceptionMessage = ex.Message;
                    apiconfigData.listError.Add(entity);
                }
            }
            else
            {
                ErrorEntity entity = new ErrorEntity(ErrorType.NullValue, apiFolderName, SpecFileName);
                apiconfigData.listError.Add(entity);
            }

            if (!Directory.Exists(iconFolderPath))
            {
                ErrorEntity entity = new ErrorEntity(ErrorType.NotFound, apiFolderName, IconsFolderName);
                apiconfigData.listError.Add(entity);
            }
            else
            {
                DirectoryInfo iconDirectoryInfo = new DirectoryInfo(iconFolderPath);
                FileInfo[] files = iconDirectoryInfo.GetFiles();
                foreach (var file in files)
                {
                    string svgFileInfo = ReadFileInfoToString(file);
                    if (!string.IsNullOrWhiteSpace(svgFileInfo))
                    {
                        apiconfigData.Icons.Add(file.Name, svgFileInfo);
                    }
                    else
                    {
                        ErrorEntity entity = new ErrorEntity(ErrorType.NullValue, apiFolderName, file.Name);
                        apiconfigData.listError.Add(entity);
                    }
                }
            }

            if (!Directory.Exists(stringsFolderPath))
            {
                ErrorEntity entity = new ErrorEntity(ErrorType.NotFound, apiFolderName, StringsFolderName);
                apiconfigData.listError.Add(entity);
            }
            else
            {
                DirectoryInfo stringDirectoryInfo = new DirectoryInfo(stringsFolderPath);
                DirectoryInfo[] otherDirectories = stringDirectoryInfo.GetDirectories();
                FileInfo[] files = stringDirectoryInfo.GetFiles();
                if (files.Length > 0)
                {
                    string enResourceContent = ReadFileInfoToString(files[0]);
                    if (!string.IsNullOrWhiteSpace(enResourceContent))
                    {
                        try
                        {
                            Dictionary<string, string> resource = JsonConvert.DeserializeObject<Dictionary<string, string>>(enResourceContent);
                            apiconfigData.Resources.Add("en", resource);
                        }
                        catch (Exception ex)
                        {
                            ErrorEntity entity = new ErrorEntity(ErrorType.CanNotDeserialize, apiFolderName, files[0].Name);
                            entity.exceptionMessage = ex.Message;
                            apiconfigData.listError.Add(entity);
                        }
                    }
                    else
                    {
                        ErrorEntity entity = new ErrorEntity(ErrorType.NullValue, apiFolderName, files[0].Name);
                        apiconfigData.listError.Add(entity);
                    }
                }

                foreach (DirectoryInfo otherDirectory in otherDirectories)
                {
                    FileInfo[] otherfiles = otherDirectory.GetFiles();
                    if (otherfiles.Length > 0)
                    {
                        string enResourceContent = ReadFileInfoToString(otherfiles[0]);
                        if (!string.IsNullOrWhiteSpace(enResourceContent))
                        {
                            try
                            {
                                Dictionary<string, string> resource = JsonConvert.DeserializeObject<Dictionary<string, string>>(enResourceContent);
                                apiconfigData.Resources.Add(otherDirectory.Name, resource);
                            }
                            catch (Exception ex)
                            {
                                ErrorEntity entity = new ErrorEntity(ErrorType.CanNotDeserialize, apiFolderName, otherfiles[0].Name);
                                entity.exceptionMessage = ex.Message;
                                apiconfigData.listError.Add(entity);
                            }
                        }
                        else
                        {
                            ErrorEntity entity = new ErrorEntity(ErrorType.NullValue, apiFolderName, otherfiles[0].Name);
                            apiconfigData.listError.Add(entity);
                        }
                    }
                }
            }

            return apiconfigData;
        }

        /// <summary>
        /// Reads the local file to string.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        private static string ReadLocalFileToString(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (FileStream fs = File.Open(filePath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Reads the file information to string.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        private static string ReadFileInfoToString(FileInfo file)
        {
            using (Stream stream = file.Open(FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
