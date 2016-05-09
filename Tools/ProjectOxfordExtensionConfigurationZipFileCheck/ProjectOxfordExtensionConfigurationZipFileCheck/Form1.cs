using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Linq;

namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class Form1 : Form
    {
        /// <summary>
        /// The temporary zip file bytes
        /// </summary>
        private byte[] tempZipFileBytes;

        /// <summary>
        /// The zip file name
        /// </summary>
        private string zipFileName1 = string.Empty;

        /// <summary>
        /// The zip file name
        /// </summary>
        private string zipFileName3 = string.Empty;

        /// <summary>
        /// The setting format
        /// </summary>
        private JsonSerializerSettings settingFormat = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// The configuration data3(For Tab "SeparateKeyValue")
        /// </summary>
        private ApiConfigurationData configurationData3 = new ApiConfigurationData();


        /// <summary>
        /// The API configuration manager
        /// </summary>
        public static ApiConfigurationManager apiConfigurationManager = new ApiConfigurationManager();

        /// <summary>
        /// The API configuration storage container
        /// </summary>
        public static string apiConfigurationStorageContainer = "apiconfiguration";

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        #region 1 Tab "Validation"

        #region Check single configuration zip file.
        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnSelectZip_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.ShowDialog();
        }

        /// <summary>
        /// Handles the FileOk event of the openFileDialog1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            this.textMessage1.Text = "";
            this.btnUploadData1.Enabled = false;
            this.textFilePath1.Text = this.openFileDialog1.FileName;
            zipFileName1 = this.openFileDialog1.SafeFileName;
            FileInfo fileInfo = new FileInfo(this.openFileDialog1.FileName);
            Stream stream = fileInfo.OpenRead();

            tempZipFileBytes = new byte[stream.Length];
            stream.Read(tempZipFileBytes, 0, tempZipFileBytes.Length);

            apiConfigurationManager.VeriliadteStream(stream, this.openFileDialog1.FileName);

            List<ErrorEntity> listError = apiConfigurationManager.listError;
            if (listError.Count > 0)
            {
                int i = 1;
                foreach (ErrorEntity errorEntity in listError)
                {
                    this.textMessage1.Text += (i++) + "、" + errorEntity.GetErrorInfo() + "\r\n";
                }
                this.btnUploadData1.Enabled = false;
            }
            else
            {
                this.textMessage1.Text = "This zip file is correct!";
                this.btnUploadData1.Enabled = true;
            }
            apiConfigurationManager.listError = new List<ErrorEntity>();
        }
        #endregion

        #region Upload single local zip file to test blob.
        /// <summary>
        /// Handles the Click event of the btnUploadSingleData control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnUploadSingleData_Click(object sender, EventArgs e)
        {
            try
            {
                string ApiCongigurationTestBlobAccountName = CloudConfigurationManager.GetSetting("ApiCongigurationTestBlobAccountName");
                string ApiConfigurationTestBlobAccountKey = CloudConfigurationManager.GetSetting("ApiConfigurationTestBlobAccountKey");

                if (string.IsNullOrWhiteSpace(ApiCongigurationTestBlobAccountName))
                {
                    this.textMessage1.Text = "Not find the key 'ApiCongigurationTestBlobAccountName' in App.config or its value is empty.";
                    return;
                }
                if (string.IsNullOrWhiteSpace(ApiConfigurationTestBlobAccountKey))
                {
                    this.textMessage1.Text = "Not find the key 'ApiConfigurationTestBlobAccountKey' in App.config or its value is empty.";
                    return;
                }

                StorageCredentials credentials = new StorageCredentials(ApiCongigurationTestBlobAccountName, ApiConfigurationTestBlobAccountKey, "AccountKey");
                Uri urlPath = new Uri(string.Format("https://{0}.blob.core.windows.net", ApiCongigurationTestBlobAccountName));

                CloudBlobClient cloudBlobClient = new CloudBlobClient(urlPath, credentials);

                CloudBlobContainer container = cloudBlobClient.GetContainerReference(apiConfigurationStorageContainer);
                var blockBlob = container.GetBlockBlobReference(zipFileName1);

                Stream stream = new MemoryStream(tempZipFileBytes);
                blockBlob.UploadFromStream(stream);

                this.textMessage1.Text = "Upload file successfully.";
            }
            catch (Exception ex)
            {
                this.textMessage1.Text = string.Format("Original Error Message: {0}", ex.Message);
            }
        }
        #endregion

        #endregion

        #region 2 Tab "Upload"

        #region File tree operation
        /// <summary>
        /// Handles the Click event of the button3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnLoadData_Click(object sender, EventArgs e)
        {
            this.btnUpLoadZip2.Enabled = false;
            this.textMessage2.Text = "Loading files...";
            var apiConfigurationPublicAccessUrl = CloudConfigurationManager.GetSetting("ApiConfigurationPublicAccessUrl");

            apiConfigurationManager = new ApiConfigurationManager(apiConfigurationPublicAccessUrl, apiConfigurationStorageContainer);

            apiConfigurationManager.LoadDataToCache();
            List<ErrorEntity> listError = apiConfigurationManager.listError;

            if (listError.Count > 0)
            {
                int i = 1;
                foreach (ErrorEntity errorMessage in listError)
                {
                    this.textMessage2.Text += (i++) + "、" + errorMessage.GetErrorInfo() + "\n";
                }
            }
            else
            {
                this.btnUpLoadZip2.Enabled = true;
                LoadFileTree(apiConfigurationManager.CacheData);
                this.textMessage2.Text = "Loaded successfully!";
            }
        }

        /// <summary>
        /// Loads the file tree.
        /// </summary>
        /// <param name="cacheData">The cache data.</param>
        private void LoadFileTree(List<ApiConfigurationData> cacheData)
        {
            if (cacheData.Count == 0)
            {
                return;
            }

            this.root2.Nodes.Clear();

            foreach (var apiCongigurationData in cacheData)
            {
                TreeNode apiNode = new TreeNode();
                apiNode.Text = apiCongigurationData.ApiTypeName;

                if (apiCongigurationData.ApiItem != null)
                {
                    TreeNode apiItemNode = new TreeNode();
                    apiItemNode.Text = "api.json";
                    apiItemNode.Tag = string.Format("{0}/{1}", apiNode.Text, apiItemNode.Text);

                    apiNode.Nodes.Add(apiItemNode);
                }
                if (apiCongigurationData.Spec != null)
                {
                    TreeNode specNode = new TreeNode();
                    specNode.Text = "spec.json";
                    specNode.Tag = string.Format("{0}/{1}", apiNode.Text, specNode.Text);


                    apiNode.Nodes.Add(specNode);
                }
                if (apiCongigurationData.QuickStart != null)
                {
                    TreeNode quickStartNode = new TreeNode();
                    quickStartNode.Text = "quickStarts.json";
                    quickStartNode.Tag = string.Format("{0}/{1}", apiNode.Text, quickStartNode.Text);


                    apiNode.Nodes.Add(quickStartNode);
                }
                if (apiCongigurationData.Icons.Count > 0)
                {
                    TreeNode iconsNode = new TreeNode();
                    iconsNode.Text = "icons";
                    foreach (var icon in apiCongigurationData.Icons)
                    {
                        TreeNode iconNode = new TreeNode();
                        iconNode.Text = icon.Key;
                        iconNode.Tag = string.Format("{0}/icons/{1}", apiNode.Text, iconNode.Text);

                        iconsNode.Nodes.Add(iconNode);
                    }
                    apiNode.Nodes.Add(iconsNode);
                }
                if (apiCongigurationData.Resources.Count > 0)
                {
                    TreeNode stringsNode = new TreeNode();
                    stringsNode.Text = "strings";

                    foreach (var resource in apiCongigurationData.Resources)
                    {
                        TreeNode resourceNode = new TreeNode();
                        resourceNode.Text = resource.Key;
                        resourceNode.Tag = string.Format("{0}/strings/{1}", apiNode.Text, resourceNode.Text);

                        stringsNode.Nodes.Add(resourceNode);
                    }
                    apiNode.Nodes.Add(stringsNode);
                }

                this.root2.Nodes.Add(apiNode);
            }
        }

        /// <summary>
        /// Handles the DoubleClick event of the root control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void root_DoubleClick(object sender, EventArgs e)
        {
            if (this.root2.SelectedNode != null)
            {
                if (this.root2.SelectedNode.Tag != null)
                {
                    string selectedNodeTag = this.root2.SelectedNode.Tag.ToString();
                    string[] pathArr = selectedNodeTag.Split('/');
                    ApiConfigurationData tempData = null;

                    foreach (var data in apiConfigurationManager.CacheData)
                    {
                        if (data.ApiTypeName == pathArr[0])
                        {
                            tempData = data;
                            break;
                        }
                    }

                    if (tempData != null)
                    {
                        if (pathArr.Length == 2)
                        {
                            switch (pathArr[1])
                            {
                                case "api.json":
                                    this.textMessage2.Text = JsonConvert.SerializeObject(tempData.ApiItem, settingFormat);
                                    break;
                                case "spec.json":
                                    this.textMessage2.Text = JsonConvert.SerializeObject(tempData.Spec, settingFormat);
                                    break;
                                case "quickStarts.json":
                                    this.textMessage2.Text = JsonConvert.SerializeObject(tempData.QuickStart, settingFormat);
                                    break;

                            }
                        }
                        if (pathArr.Length == 3)
                        {
                            if (pathArr[1] == "icons")
                            {
                                this.textMessage2.Text = tempData.Icons[pathArr[2]];
                            }
                            if (pathArr[1] == "strings")
                            {
                                this.textMessage2.Text = JsonConvert.SerializeObject(tempData.Resources[pathArr[2]], settingFormat);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Update multi zip files from test blob to production blob.
        /// <summary>
        /// Handles the Click event of the btnUpLoadData control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnUpLoadMultiZip_Click(object sender, EventArgs e)
        {
            this.textMessage2.Text = string.Empty;

            string storageAccount = this.textAccount2.Text;
            string storageAccountKey = this.textAccountKey2.Text;

            if (string.IsNullOrWhiteSpace(storageAccount))
            {
                this.textMessage2.Text = "Please input the Storage Account .";
                return;
            }
            if (string.IsNullOrWhiteSpace(storageAccountKey))
            {
                this.textMessage2.Text = "Please input the Storage Account Key .";
                return;
            }

            try
            {
                string errorMessageStr = UpdateData();
                if (!string.IsNullOrWhiteSpace(errorMessageStr))
                {
                    this.textMessage2.Text = errorMessageStr;
                    return;
                }

                StorageCredentials credentials = new StorageCredentials(storageAccount, storageAccountKey, "AccountKey");
                Uri urlPath = new Uri(string.Format("https://{0}.blob.core.windows.net", storageAccount));
                CloudBlobClient cloudBlobClient = new CloudBlobClient(urlPath, credentials);

                CloudBlobContainer container = cloudBlobClient.GetContainerReference(apiConfigurationStorageContainer);

                var blobs = apiConfigurationManager.GetCloudBlockBlob();
                var requestOption = new BlobRequestOptions() { RetryPolicy = new ExponentialRetry() };

                foreach (var apiZip in blobs.OfType<CloudBlockBlob>())
                {
                    using (var blobStream = apiZip.OpenRead(null, requestOption))
                    {
                        var blockBlob = container.GetBlockBlobReference(apiZip.Name);
                        blockBlob.UploadFromStream(blobStream);
                    }
                }

                this.textMessage2.Text = "Successfully Upload files from Test blob to Production blob.";
            }
            catch (Exception ex)
            {
                this.textMessage2.Text = string.Format("Original Error Message: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <returns></returns>
        private string UpdateData()
        {
            var apiConfigurationPublicAccessUrl = CloudConfigurationManager.GetSetting("ApiConfigurationPublicAccessUrl");

            apiConfigurationManager = new ApiConfigurationManager(apiConfigurationPublicAccessUrl, apiConfigurationStorageContainer);

            apiConfigurationManager.LoadDataToCache();
            List<ErrorEntity> listError = apiConfigurationManager.listError;

            if (listError.Count > 0)
            {
                string errorMessageStr = string.Empty;
                int i = 1;
                foreach (ErrorEntity errorMessage in listError)
                {
                    errorMessageStr += (i++) + "、" + errorMessage.GetErrorInfo() + "\n";
                }
                return errorMessageStr;
            }
            return null;
        }

        #endregion

        #endregion

        #region  3 Tab "SeparateKeyValue"

        /// <summary>
        /// Handles the Click event of the btnSelectOriginalZip control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnSelectOriginalZip_Click(object sender, EventArgs e)
        {
            this.openFileDialog3.ShowDialog();
        }

        /// <summary>
        /// Handles the FileOk event of the openFileDialog2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void openFileDialog3_FileOk(object sender, CancelEventArgs e)
        {
            this.textMessage3.Text = "";
            this.btnUploadData3.Enabled = false;
            this.textFilePath3.Text = this.openFileDialog3.FileName;
            this.zipFileName3 = this.openFileDialog3.SafeFileName;

            FileInfo fileInfo = new FileInfo(this.openFileDialog3.FileName);
            Stream stream = fileInfo.OpenRead();

            var apiConfigurationPublicAccessUrl = CloudConfigurationManager.GetSetting("ApiConfigurationPublicAccessUrl");
            ApiConfigurationManager manager = new ApiConfigurationManager(apiConfigurationPublicAccessUrl, apiConfigurationStorageContainer);

            configurationData3 = manager.HandleOriginalData(stream);
            if (configurationData3.listError.Count > 0)
            {
                foreach (var entity in configurationData3.listError)
                {
                    this.textMessage3.Text += entity.GetErrorInfo();
                }
            }
            else
            {
                this.textMessage3.Text = "The original data has been handled successfully.\r\nYou can save the handled data to local or upload the it to the test blob container.";
                this.btnUploadData3.Enabled = true;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnUploadData3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnUploadData3_Click(object sender, EventArgs e)
        {
            try
            {
                string ApiCongigurationTestBlobAccountName = CloudConfigurationManager.GetSetting("ApiCongigurationTestBlobAccountName");
                string ApiConfigurationTestBlobAccountKey = CloudConfigurationManager.GetSetting("ApiConfigurationTestBlobAccountKey");

                if (string.IsNullOrWhiteSpace(ApiCongigurationTestBlobAccountName))
                {
                    this.textMessage1.Text = "Not find the key 'ApiCongigurationTestBlobAccountName' in App.config or its value is empty.";
                    return;
                }
                if (string.IsNullOrWhiteSpace(ApiConfigurationTestBlobAccountKey))
                {
                    this.textMessage1.Text = "Not find the key 'ApiConfigurationTestBlobAccountKey' in App.config or its value is empty.";
                    return;
                }

                StorageCredentials credentials = new StorageCredentials(ApiCongigurationTestBlobAccountName, ApiConfigurationTestBlobAccountKey, "AccountKey");
                Uri urlPath = new Uri(string.Format("https://{0}.blob.core.windows.net", ApiCongigurationTestBlobAccountName));

                ApiConfigurationManager manager = new ApiConfigurationManager();
                Stream stream = manager.GetConfigurationData(configurationData3, zipFileName3.Substring(0, zipFileName3.LastIndexOf('.')));

                CloudBlobClient cloudBlobClient = new CloudBlobClient(urlPath, credentials);
                CloudBlobContainer container = cloudBlobClient.GetContainerReference(apiConfigurationStorageContainer);
                var blockBlob = container.GetBlockBlobReference(zipFileName3);
                //blockBlob.UploadFromStream(stream);

                this.textMessage3.Text = "Upload file successfully.";
            }
            catch (Exception ex)
            {
                this.textMessage3.Text = string.Format("Original Error Message: {0}", ex.Message);
            }
        }

        #endregion

    }
}
