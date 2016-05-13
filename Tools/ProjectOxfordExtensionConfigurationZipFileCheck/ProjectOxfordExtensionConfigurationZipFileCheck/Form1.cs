﻿
namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
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

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class Form1 : Form
    {
        /// <summary>
        /// The temporary folder path
        /// </summary>
        private string tempFolderPath = "C:\\ApiConfigurationDataTemp";

        /// <summary>
        /// The setting format
        /// </summary>
        private JsonSerializerSettings settingFormat = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Include,
            MissingMemberHandling = MissingMemberHandling.Error
        };

        /// <summary>
        /// The configuration data3(For Tab "SeparateKeyValue")
        /// </summary>
        private ApiConfigurationData cacheConfigurationData = new ApiConfigurationData();

        /// <summary>
        /// The cache data
        /// </summary>
        private List<ApiConfigurationData> cacheData;

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
            ApiConfigurationManager manager = new ApiConfigurationManager();
            this.textMessage1.Text = "";
            this.btnUploadData1.Enabled = false;
            this.btnSave1.Enabled = false;
            this.textFilePath1.Text = this.openFileDialog1.FileName;
            FileInfo fileInfo = new FileInfo(this.openFileDialog1.FileName);
            using (Stream stream = fileInfo.OpenRead())
            {
                cacheConfigurationData = manager.VeriliadteStream(stream, this.openFileDialog1.FileName);
            }

            if (manager.listError.Count > 0)
            {
                int i = 1;
                bool existActiveError = false;
                foreach (ErrorEntity errorEntity in manager.listError)
                {
                    if (errorEntity.errorStatus == ErrorStatus.NotFixed)
                    {
                        existActiveError = true;
                    }
                    this.textMessage1.Text += (i++) + "、" + errorEntity.GetErrorInfo() + "\r\n";
                }
                if (!existActiveError)
                {
                    this.btnUploadData1.Enabled = true;
                    this.btnSave1.Enabled = true;
                }
            }
            else
            {
                this.textMessage1.Text = "This zip file is correct!";
                this.btnUploadData1.Enabled = true;
                this.btnSave1.Enabled = true;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSave1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnSave1_Click(object sender, EventArgs e)
        {
            string tempZipFilePath = string.Format("{0}\\{1}.zip", tempFolderPath, cacheConfigurationData.ApiTypeName);
            ApiConfigurationManager.GetConfigurationData(cacheConfigurationData, tempFolderPath, cacheConfigurationData.ApiTypeName);

            this.textMessage1.Text = string.Format("Saved as : {0}", tempZipFilePath);
        }

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

                string tempZipFilePath = string.Format("{0}\\{1}.zip", tempFolderPath, cacheConfigurationData.ApiTypeName);
                ApiConfigurationManager.GetConfigurationData(cacheConfigurationData, tempFolderPath, cacheConfigurationData.ApiTypeName);

                FileInfo fileInfo = new FileInfo(tempZipFilePath);
                using (Stream stream = fileInfo.OpenRead())
                {
                    StorageCredentials credentials = new StorageCredentials(ApiCongigurationTestBlobAccountName, ApiConfigurationTestBlobAccountKey, "AccountKey");
                    Uri urlPath = new Uri(string.Format("https://{0}.blob.core.windows.net", ApiCongigurationTestBlobAccountName));

                    CloudBlobClient cloudBlobClient = new CloudBlobClient(urlPath, credentials);
                    CloudBlobContainer container = cloudBlobClient.GetContainerReference(apiConfigurationStorageContainer);
                    var blockBlob = container.GetBlockBlobReference(string.Format("{0}.zip", cacheConfigurationData.ApiTypeName));
                    blockBlob.UploadFromStream(stream);
                }
                this.textMessage1.Text = "Upload file successfully.";
            }
            catch (Exception ex)
            {
                this.textMessage1.Text = string.Format("Original Error Message: {0}", ex.Message);
            }
        }
        #endregion


        #region 2 Tab "Upload"

        /// <summary>
        /// Handles the Click event of the button3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnLoadData_Click(object sender, EventArgs e)
        {
            this.btnUpLoadZip2.Enabled = false;
            this.tabControl21.SelectedTab = this.tabControl21.TabPages[0];
            this.tabControl22.SelectedTab = this.tabControl22.TabPages[0];
            this.textMessage2.Text = "Loading files...";

            var apiConfigurationPublicAccessUrl = CloudConfigurationManager.GetSetting("ApiConfigurationPublicAccessUrl");

            ApiConfigurationManager manager = new ApiConfigurationManager(apiConfigurationPublicAccessUrl, apiConfigurationStorageContainer);
            cacheData = manager.LoadDataToCache();

            List<ErrorEntity> listError = manager.listError;
            bool existActiveError = false;

            if (manager.listError.Count > 0)
            {
                this.textMessage2.Text = "";
                int i = 1;
                foreach (ErrorEntity errorMessage in manager.listError)
                {
                    if (errorMessage.errorStatus == ErrorStatus.NotFixed)
                    {
                        existActiveError = true;
                    }
                    this.textMessage2.Text += (i++) + "、" + errorMessage.GetErrorInfo() + "\n";
                }
            }
            if (!existActiveError)
            {
                LoadFileTree(cacheData);
                this.btnUpLoadZip2.Enabled = true;
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
                this.checkedListBox2.Items.Add(apiCongigurationData.ApiTypeName);

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

                    foreach (var data in cacheData)
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
                            this.tabControl22.SelectedTab = this.tabControl22.TabPages[1];

                            switch (pathArr[1])
                            {
                                case "api.json":
                                    this.textFileContent2.Text = JsonConvert.SerializeObject(tempData.ApiItem, settingFormat);
                                    break;
                                case "spec.json":
                                    this.textFileContent2.Text = JsonConvert.SerializeObject(tempData.Spec, settingFormat);
                                    break;
                                case "quickStarts.json":
                                    this.textFileContent2.Text = JsonConvert.SerializeObject(tempData.QuickStart, settingFormat);
                                    break;
                            }
                        }
                        if (pathArr.Length == 3)
                        {
                            if (pathArr[1] == "icons")
                            {
                                this.textFileContent2.Text = tempData.Icons[pathArr[2]];
                            }
                            if (pathArr[1] == "strings")
                            {
                                this.tabControl22.SelectedTab = this.tabControl22.TabPages[1];
                                this.textFileContent2.Text = JsonConvert.SerializeObject(tempData.Resources[pathArr[2]], settingFormat);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnUpLoadData control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnUpLoadMultiZip_Click(object sender, EventArgs e)
        {
            this.textMessage2.Text = string.Empty;
            this.textFileContent2.Text = string.Empty;

            string storageAccount = this.textAccount2.Text;
            string storageAccountKey = this.textAccountKey2.Text;

            if (string.IsNullOrWhiteSpace(storageAccount))
            {
                this.textMessage2.Text = "Please input the Storage Account.";
                return;
            }
            if (string.IsNullOrWhiteSpace(storageAccountKey))
            {
                this.textMessage2.Text = "Please input the Storage Account Key.";
                return;
            }
            if (this.checkedListBox2.CheckedItems.Count == 0)
            {
                this.textMessage2.Text = "Please select the api items to upload to productive blob.";
                return;
            }
            List<string> selectApiItems = new List<string>();
            for (int i = 0; i < this.checkedListBox2.CheckedItems.Count; i++)
            {
                selectApiItems.Add(this.checkedListBox2.CheckedItems[i].ToString());
            }

            try
            {
                StorageCredentials credentials = new StorageCredentials(storageAccount, storageAccountKey, "AccountKey");
                Uri urlPath = new Uri(string.Format("https://{0}.blob.core.windows.net", storageAccount));
                CloudBlobClient cloudBlobClient = new CloudBlobClient(urlPath, credentials);

                CloudBlobContainer container = cloudBlobClient.GetContainerReference(apiConfigurationStorageContainer);

                var requestOption = new BlobRequestOptions() { RetryPolicy = new ExponentialRetry() };

                foreach (var confifurationData in cacheData)
                {
                    if (!selectApiItems.Contains(confifurationData.ApiTypeName))
                    {
                        continue;
                    }
                    string tempZipFilePath = string.Format("{0}\\{1}.zip", tempFolderPath, confifurationData.ApiTypeName);
                    ApiConfigurationManager.GetConfigurationData(cacheConfigurationData, tempFolderPath, confifurationData.ApiTypeName);

                    FileInfo fileInfo = new FileInfo(tempZipFilePath);
                    using (Stream stream = fileInfo.OpenRead())
                    {
                        var blockBlob = container.GetBlockBlobReference(string.Format("{0}.zip", confifurationData.ApiTypeName));
                        blockBlob.UploadFromStream(stream);
                    }
                }

                this.textMessage2.Text = "Upload files from Test blob to productive blob successfully .";
            }
            catch (Exception ex)
            {
                this.textMessage2.Text = string.Format("Original Error Message: {0}", ex.Message);
            }
        }

        #endregion

    }
}
