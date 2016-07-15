
namespace ApiOnBoardingConfigurationTool
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// The BlobHelper
    /// </summary>
    public class BlobHelper
    {
        /// <summary>
        /// Uploads one API configuration to BLOB container.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="apiConfigurationData">The API configuration data.</param>
        /// <param name="localFolderPath">The local folder path.</param>
        public static void UploadApiConfigurationToBlobContainer(StorageCredentials credentials, string containerName, ApiConfigurationData apiConfigurationData, string localFolderPath)
        {
            Uri urlPath = new Uri(string.Format("https://{0}.blob.core.windows.net", credentials.AccountName));
            CloudBlobClient cloudBlobClient = new CloudBlobClient(urlPath, credentials);
            CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);

            string apiFolderName = string.Empty;
            if (!string.IsNullOrWhiteSpace(apiConfigurationData.ApiFolderName))
            {
                apiFolderName = apiConfigurationData.ApiFolderName;
            }
            else
            {
                apiFolderName = apiConfigurationData.ApiTypeName;
            }

            string tempZipFilePath = string.Format("{0}\\{1}.zip", localFolderPath, apiFolderName);
            ApiConfigurationManager.SaveConfigurationDataToLocalZip(apiConfigurationData, localFolderPath, apiFolderName);

            FileInfo fileInfo = new FileInfo(tempZipFilePath);
            using (Stream stream = fileInfo.OpenRead())
            {
                var blockBlob = container.GetBlockBlobReference(string.Format("{0}.zip", apiFolderName));
                blockBlob.UploadFromStream(stream);
            }
        }

        /// <summary>
        /// Uploads the API configuration list to BLOB container.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="apiConfigurationDataList">The API configuration data list.</param>
        /// <param name="localFolderPath">The local folder path.</param>
        public static void UploadApiConfigurationListToBlobContainer(StorageCredentials credentials, string containerName, List<ApiConfigurationData> apiConfigurationDataList, string localFolderPath)
        {
            Uri urlPath = new Uri(string.Format("https://{0}.blob.core.windows.net", credentials.AccountName));
            CloudBlobClient cloudBlobClient = new CloudBlobClient(urlPath, credentials);
            CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);

            foreach (var configurationData in apiConfigurationDataList)
            {
                string apiFolderName = string.Empty;
                if (!string.IsNullOrWhiteSpace(configurationData.ApiFolderName))
                {
                    apiFolderName = configurationData.ApiFolderName;
                }
                else
                {
                    apiFolderName = configurationData.ApiTypeName;
                }

                string tempZipFilePath = string.Format("{0}\\{1}.zip", localFolderPath, apiFolderName);
                ApiConfigurationManager.SaveConfigurationDataToLocalZip(configurationData, localFolderPath, apiFolderName);

                FileInfo fileInfo = new FileInfo(tempZipFilePath);
                using (Stream stream = fileInfo.OpenRead())
                {
                    var blockBlob = container.GetBlockBlobReference(string.Format("{0}.zip", apiFolderName));
                    blockBlob.UploadFromStream(stream);
                }
            }
        }

        /// <summary>
        /// Deletes the API configuration list from BLOB container.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="apiItemList">The API item list.</param>
        public static void DeleteApiConfigurationListFromBlobContainer(StorageCredentials credentials, string containerName, List<string> apiItemList, string extension)
        {
            Uri urlPath = new Uri(string.Format("https://{0}.blob.core.windows.net", credentials.AccountName));
            CloudBlobClient cloudBlobClient = new CloudBlobClient(urlPath, credentials);
            CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);

            foreach (var apiItemName in apiItemList)
            {
                var blockBlob = container.GetBlobReference(string.Format("{0}.{1}", apiItemName, extension));
                bool result = blockBlob.DeleteIfExists();
            }
        }

        /// <summary>
        /// Uploads the confifuratuin to BLOB container.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="stream">The stream.</param>
        public static void UploadConfifuratuinToBlobContainer(StorageCredentials credentials, string containerName, string fileName, Stream stream)
        {
            Uri urlPath = new Uri(string.Format("https://{0}.blob.core.windows.net", credentials.AccountName));
            CloudBlobClient cloudBlobClient = new CloudBlobClient(urlPath, credentials);
            CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);

            var blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.UploadFromStream(stream);
        }
    }
}
