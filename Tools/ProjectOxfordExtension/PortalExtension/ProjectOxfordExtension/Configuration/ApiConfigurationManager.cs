/********************************************************
*                                                        *
*   Copyright (c) Microsoft. All rights reserved.        *
*                                                        *
*********************************************************/

namespace Microsoft.Portal.Extensions.ProjectOxfordExtension.Configuration
{
    using Microsoft.Portal.Extensions.ProjectOxfordExtension.DataModels;
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
    /// <seealso cref="Microsoft.Portal.Extensions.ProjectOxfordExtension.Configuration.IApiConfigurationManager" />
    public class ApiConfigurationManager //: IApiConfigurationManager
    {
        /// <summary>
        /// The default language
        /// </summary>
        public const string DefaultLanguage = "en";

        /// <summary>
        /// The error message
        /// </summary>
        public string ErrorMessage = null;

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

        /// <summary>
        /// The timer
        /// </summary>
        private Timer timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiConfigurationManager" /> class.
        /// </summary>
        /// <param name="connString">The connection string.</param>
        /// <param name="container">The container.</param>
        /// <param name="apiConfigCacheRefreshInterval">The API configuration cache refresh interval.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public ApiConfigurationManager(string connString, string container, TimeSpan apiConfigCacheRefreshInterval)
        {
            SetupBlobConnection(connString, container);

            SetupTimer(apiConfigCacheRefreshInterval);
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
        public void LoadDataToCache()
        {
            ErrorMessage = null;
            // Dictionary[apiName, Dictionary[localeId, ApiConfigurationData]]
            var configs = new Dictionary<string, Dictionary<string, ApiConfigurationData>>();
            var blobs = configContainer.ListBlobs();

            // List on api folders
            // Example: face/api.json
            //          face/spec.json
            //          face/quickstarts.json
            //          face/icons/face.svg
            //          face/strings/resources.resjson
            //          face/strings/zh-cn/resources.resjson
            foreach (var apiZip in blobs.OfType<CloudBlockBlob>())
            {
                try
                {
                    var apiConfigLocaleMap = DeserializeApiConfig(apiZip);
                    if (apiConfigLocaleMap != null && apiConfigLocaleMap.Count > 0)
                    {
                        configs.Add(apiConfigLocaleMap.First().Value.ApiTypeName, apiConfigLocaleMap);
                    }
                }
                catch (Exception e)
                {
                    var message = string.Format("load or parse file {0} failed with exception: \r\n {1}", apiZip.Name, e.Message);
                    Trace.TraceError(message);
                    ErrorMessage = message;
                }
            }

            // exchange to current one.
            apiConfigurations = configs;
        }

        /// <summary>
        /// Setups the timer.
        /// </summary>
        /// <param name="apiConfigCacheRefreashInterval">The API configuration cache refreash interval.</param>
        private void SetupTimer(TimeSpan apiConfigCacheRefreashInterval)
        {
            timer = new Timer(s =>
            {
                try
                {
                    LoadDataToCache();
                }
                catch (Exception e)
                {
                    var message = string.Format("load data to cache failed with exception: \r\n {0}", e.Message);
                    Trace.TraceError(message);
                    ErrorMessage = message;
                }
            });

            timer.Change(apiConfigCacheRefreashInterval, apiConfigCacheRefreashInterval);
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
        private Dictionary<string, ApiConfigurationData> DeserializeApiConfig(CloudBlockBlob apiZipBlob)
        {
            var requestOption = new BlobRequestOptions() { RetryPolicy = new ExponentialRetry() };

            using (var blobStream = apiZipBlob.OpenRead(null, requestOption))
            {
                using (var zip = new ZipArchive(blobStream))
                {
                    var apiItemJson = ReadZipEntryToString(zip.Entries.First(z => z.Name == ApiItemFileName));
                    var apiItem = JObject.Parse(apiItemJson);
                    var originalApiConfig = JsonConvert.DeserializeObject<ApiConfigurationData>(apiItemJson);
                    originalApiConfig.Spec = JObject.Parse(ReadZipEntryToString(zip.Entries.First(z => z.Name == SpecFileName)));
                    originalApiConfig.QuickStart = JObject.Parse(ReadZipEntryToString(zip.Entries.First(z => z.Name == QuickStartsFileName)));
                    originalApiConfig.ApiItem = apiItem;

                    // Dictionary<fileName, svgContent>
                    var icons = new Dictionary<string, string>();

                    // Dictionary<localeId, Dictionary<localizableItemKey, localizableItemValue>>
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

                    // repair icons
                    ApiConfigurationData.ReplaceIcons(originalApiConfig, icons);

                    var localizedConfigs = new Dictionary<string, ApiConfigurationData>();

                    // catch up all locales resources.
                    foreach (var localeString in localizableStrings)
                    {
                        localizedConfigs.Add(localeString.Key, ApiConfigurationData.GetLocalized(originalApiConfig, localeString.Key, localeString.Value, localizableStrings[DefaultLanguage]));
                    }

                    return localizedConfigs;
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