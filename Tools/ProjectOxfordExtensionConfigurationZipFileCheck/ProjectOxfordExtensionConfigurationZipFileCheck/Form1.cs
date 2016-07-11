
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
    using Newtonsoft.Json.Linq;
    using System.Linq;

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
        /// The special setting format
        /// </summary>
        private JsonSerializerSettings specialSettingFormat = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
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
        /// The manual configuration data(For Tab Visual Configuration)
        /// </summary>
        private ApiConfigurationData manualConfigurationData = new ApiConfigurationData();

        /// <summary>
        /// The alert title
        /// </summary>
        private const string AlertTitle = "Alert";

        /// <summary>
        /// The exception title
        /// </summary>
        private const string ExceptionTitle = "Exception";

        /// <summary>
        /// The error message(For Tab Visual Configuration)
        /// </summary>
        private string ErrorMessage = string.Empty;

        /// <summary>
        /// The resource token
        /// </summary>
        private const string ResourceToken = "ms-resource:";

        /// <summary>
        /// The icons token
        /// </summary>
        private const string IconToken = "ms-icon:";

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1" /> class.
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
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnSelectZip_Click(object sender, EventArgs e)
        {
            ApiConfigurationManager manager = new ApiConfigurationManager();

            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textFilePath1.Text = this.folderBrowserDialog1.SelectedPath;
                string folderName = this.folderBrowserDialog1.SelectedPath.Substring(this.folderBrowserDialog1.SelectedPath.LastIndexOf(@"\") + 1);

                string tempZipFilepath = ApiConfigurationManager.CompressFolderContentIntoStream(this.folderBrowserDialog1.SelectedPath, tempFolderPath);
                FileInfo fileInfo = new FileInfo(tempZipFilepath);
                using (Stream stream = fileInfo.OpenRead())
                {
                    cacheConfigurationData = manager.VeriliadteStream(stream, folderName);
                    cacheConfigurationData.ApiFolderName = folderName;
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
                        this.BtnUploadData1.Enabled = true;
                        this.BtnSaveAsZip.Enabled = true;
                    }
                }
                else
                {
                    this.textMessage1.Text = "This api configuration is correct!";
                    this.BtnUploadData1.Enabled = true;
                    this.BtnSaveAsZip.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSave1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void BtnSaveAsZip_Click(object sender, EventArgs e)
        {
            string tempZipFilePath = string.Format("{0}\\{1}.zip", tempFolderPath, cacheConfigurationData.ApiTypeName);
            string zipFileName = string.Empty;
            if (!string.IsNullOrWhiteSpace(cacheConfigurationData.ApiFolderName))
            {
                zipFileName = cacheConfigurationData.ApiFolderName;
            }
            else
            {
                zipFileName = cacheConfigurationData.ApiTypeName;
            }

            ApiConfigurationManager.SaveConfigurationDataToLocal(cacheConfigurationData, tempFolderPath, zipFileName);

            this.textMessage1.Text = string.Format("Saved as : {0}", tempZipFilePath);
        }

        /// <summary>
        /// Handles the Click event of the btnUploadSingleData control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void BtnUploadData1_Click(object sender, EventArgs e)
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
                BlobHelper.UploadApiConfigurationToBlobContainer(credentials, apiConfigurationStorageContainer, cacheConfigurationData, tempFolderPath);

                //Uri urlPath = new Uri(string.Format("https://{0}.blob.core.windows.net", ApiCongigurationTestBlobAccountName));
                //CloudBlobClient cloudBlobClient = new CloudBlobClient(urlPath, credentials);
                //CloudBlobContainer container = cloudBlobClient.GetContainerReference(apiConfigurationStorageContainer);

                //string tempZipFilePath = string.Format("{0}\\{1}.zip", tempFolderPath, cacheConfigurationData.ApiTypeName);
                //ApiConfigurationManager.SaveConfigurationDataToLocal(cacheConfigurationData, tempFolderPath, cacheConfigurationData.ApiTypeName);

                //FileInfo fileInfo = new FileInfo(tempZipFilePath);
                //using (Stream stream = fileInfo.OpenRead())
                //{
                //    var blockBlob = container.GetBlockBlobReference(string.Format("{0}.zip", cacheConfigurationData.ApiTypeName));
                //    blockBlob.UploadFromStream(stream);
                //}

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
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnLoadData_Click(object sender, EventArgs e)
        {
            this.BtnLoadData2.Enabled = false;
            this.BtnUpLoadZip2.Enabled = false;
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
                this.BtnUpLoadZip2.Enabled = true;
                this.textMessage2.Text = "Loaded successfully!";
            }

            this.BtnLoadData2.Enabled = true;
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
            this.checkedListBox2.Items.Clear();

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
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
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
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
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
                List<ApiConfigurationData> selectedApiConfigurationDataList = new List<ApiConfigurationData>();
                foreach (var data in cacheData)
                {
                    if (selectApiItems.Contains(data.ApiTypeName))
                    {
                        selectedApiConfigurationDataList.Add(data);
                    }
                }

                StorageCredentials credentials = new StorageCredentials(storageAccount, storageAccountKey, "AccountKey");
                BlobHelper.UploadApiConfigurationListToBlobContainer(credentials, apiConfigurationStorageContainer, selectedApiConfigurationDataList, tempFolderPath);

                this.textMessage2.Text = "Upload files from Test blob to productive blob successfully .";
            }
            catch (Exception ex)
            {
                this.labelTotalResult.Text = "Occured some errors!";
                this.textMessage2.Text = string.Format("Original Error Message: {0}", ex.Message);
            }
        }

        #endregion

        #region 3 Tab "Visual configuration"

        /// <summary>
        /// Handles the MouseCaptureChanged event of the tabControl2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void tabControl2_MouseCaptureChanged(object sender, EventArgs e)
        {
            this.labelCurrentTab.Text = this.tabControl2.SelectedTab.Text;

            if (this.tabControl2.SelectedTab == this.TabApi)
            {
                this.CBApiIconData.Items.Clear();
                if (manualConfigurationData.Icons.Count > 0)
                {
                    this.CBApiIconData.Items.Add("");
                    foreach (var icon in manualConfigurationData.Icons)
                    {
                        this.CBApiIconData.Items.Add(icon.Key);
                    }
                }
            }
            else if (this.tabControl2.SelectedTab == this.TabOverAll)
            {
                try
                {
                    this.textOverallApiJsonContent.Text = "null";
                    this.textOverallQuickStartJsonContent.Text = "null";
                    this.textOverallSpecJsonContent.Text = "null";
                    this.textOverallResourceJsonContent.Text = "null";
                    this.textOverallSvgIconsList.Text = "null";

                    this.textOverallApiJsonContent.Text = JsonConvert.SerializeObject(manualConfigurationData.ApiItem, settingFormat);
                    this.textOverallQuickStartJsonContent.Text = JsonConvert.SerializeObject(manualConfigurationData.QuickStart, settingFormat);
                    this.textOverallSpecJsonContent.Text = JsonConvert.SerializeObject(manualConfigurationData.Spec, settingFormat);

                    if (manualConfigurationData.Resources.ContainsKey("en"))
                    {
                        this.textOverallResourceJsonContent.Text = JsonConvert.SerializeObject(manualConfigurationData.Resources["en"], settingFormat);
                    }

                    if (manualConfigurationData.Icons.Count > 0)
                    {
                        foreach (var icon in manualConfigurationData.Icons)
                        {
                            this.textOverallSvgIconsList.Text += icon.Key + "\r\n";
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.labelTotalResult.Text = "Occured some errors!";
                    MessageBox.Show(ex.Message, ExceptionTitle);
                }
            }
        }

        #region 3-1 For Icons

        /// <summary>
        /// Handles the Click event of the BtnSelectSvgFiles control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void BtnSelectSvgFiles_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.textIconFolderPath.Text = this.folderBrowserDialog1.SelectedPath;
                    DirectoryInfo svgFolderPath = new DirectoryInfo(this.folderBrowserDialog1.SelectedPath);

                    FileInfo[] fileNames = svgFolderPath.GetFiles();
                    this.textSvgFiles.Text = string.Empty;
                    this.CBApiIconData.Items.Clear();
                    manualConfigurationData.Icons.Clear();

                    foreach (var file in fileNames)
                    {
                        if (file.Extension.ToLower().Equals(".svg"))
                        {
                            this.textSvgFiles.Text += file.Name + "\r\n";

                            using (StreamReader reader = file.OpenText())
                            {
                                manualConfigurationData.Icons.Add(file.Name, reader.ReadToEnd());
                            }
                        }
                    }

                    if (manualConfigurationData.Icons.Count == 0)
                    {
                        MessageBox.Show("There is no SVG file in this folder!", AlertTitle);
                        return;
                    }

                    this.FlagIcon.BackColor = System.Drawing.Color.Green;
                    this.labelTotalResult.Text = "'Svg' files loaded successfully!";
                }
            }
            catch (Exception ex)
            {
                this.labelTotalResult.Text = "Occured some errors!";
                MessageBox.Show(ex.Message, ExceptionTitle);
            }
        }

        #endregion

        #region 3-2 For api

        /// <summary>
        /// Handles the Click event of the BtnApiCategoryAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnApiCategoryAdd_Click(object sender, EventArgs e)
        {
            this.ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.textApiCategory.Text))
            {
                this.ErrorMessage = "'Catogory' should not be empty!";
            }

            if (!string.IsNullOrWhiteSpace(this.ErrorMessage))
            {
                MessageBox.Show(this.ErrorMessage, AlertTitle);
                return;
            }

            if (string.IsNullOrWhiteSpace(this.textApiCategories.Text))
            {
                this.textApiCategories.Text = string.Format("\"{0}\"", this.textApiCategory.Text);
            }
            else
            {
                this.textApiCategories.Text += string.Format(",\r\n\"{0}\"", this.textApiCategory.Text);
            }

            this.textApiCategory.Text = string.Empty;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the CKAPISkuQuotaEmpty control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void CKAPISkuQuotaEmpty_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CKApiSkuQuotaEmpty.Checked)
            {
                this.BtnApiSkuQuotaAdd.Enabled = false;
                this.textApiSkuQuotaCode.Enabled = false;
                this.textApiSkuQuotaName.Enabled = false;
                this.textApiSkuQuotaQuota.Enabled = false;

                this.textApiSkuQuotaCode.Text = string.Empty;
                this.textApiSkuQuotaName.Text = string.Empty;
                this.textApiSkuQuotaQuota.Text = string.Empty;
                this.textApiSkuQuotaData.Text = string.Empty;
            }
            else
            {
                this.BtnApiSkuQuotaAdd.Enabled = true;
                this.textApiSkuQuotaCode.Enabled = true;
                this.textApiSkuQuotaName.Enabled = true;
                this.textApiSkuQuotaQuota.Enabled = true;

                this.textApiSkuQuotaCode.Text = "F0";
                this.textApiSkuQuotaName.Text = "Free";
                this.textApiSkuQuotaQuota.Text = "1";
            }
        }

        /// <summary>
        /// Handles the Click event of the BtnApiSkuQuotaAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void BtnApiSkuQuotaAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.ErrorMessage = string.Empty;
                if (string.IsNullOrWhiteSpace(this.textApiSkuQuotaCode.Text))
                {
                    this.ErrorMessage += "'SkuQuota - Code' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textApiSkuQuotaName.Text))
                {
                    this.ErrorMessage += "'SkuQuota - Name' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textApiSkuQuotaQuota.Text))
                {
                    this.ErrorMessage += "'SkuQuota - Quota' should not be empty!\r\n";
                }

                foreach (char ch in this.textApiSkuQuotaQuota.Text)
                {
                    if (!Char.IsNumber(ch))
                    {
                        this.ErrorMessage += "'SkuQuota - Quota' should be number!\r\n";
                        break;
                    }
                }

                if (!string.IsNullOrWhiteSpace(this.ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, AlertTitle);
                    return;
                }

                string itemData = "{\"code\": \"" + this.textApiSkuQuotaCode.Text + "\",\"name\": \"" + this.textApiSkuQuotaName.Text + "\",\"quota\": " + this.textApiSkuQuotaQuota.Text + "}";
                if (string.IsNullOrWhiteSpace(this.textApiSkuQuotaData.Text))
                {
                    this.textApiSkuQuotaData.Text = itemData;
                }
                else
                {
                    this.textApiSkuQuotaData.Text += ",\r\n" + itemData;
                }
            }
            catch (Exception ex)
            {
                this.labelTotalResult.Text = "Occured some errors!";
                MessageBox.Show(ex.Message, ExceptionTitle);
            }
        }

        /// <summary>
        /// Handles the Click event of the BtnSaveApiInfo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void BtnSaveApiInfo_Click(object sender, EventArgs e)
        {
            try
            {
                this.ErrorMessage = string.Empty;
                ApiEntity entity = new ApiEntity();
                entity.categories = new List<string>();
                entity.skuQuota = new List<ApiSkuQuotaEntity>();
                List<ApiSkuQuotaEntity> apiSkuQuota = new List<ApiSkuQuotaEntity>();

                Dictionary<string, string> resource;
                if (manualConfigurationData.Resources.ContainsKey("en"))
                {
                    resource = manualConfigurationData.Resources["en"];
                }
                else
                {
                    resource = new Dictionary<string, string>();
                }

                if (string.IsNullOrWhiteSpace(this.textApiItem.Text))
                {
                    this.ErrorMessage += "Item should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textApiTitle.Text))
                {
                    this.ErrorMessage += "Title should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textApiSubTitle.Text))
                {
                    this.ErrorMessage += "SubTitle should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.CBApiIconData.Text))
                {
                    this.ErrorMessage += "IconData should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textApiCategories.Text))
                {
                    this.ErrorMessage += "'Categories' should not be empty!\r\n";
                }

                if (!this.CKApiSkuQuotaEmpty.Checked)
                {
                    if (string.IsNullOrWhiteSpace(this.textApiSkuQuotaData.Text))
                    {
                        this.ErrorMessage += "'SkuQuota' should not be empty!\r\n";
                    }

                    try
                    {
                        string apiSkuQuotaData = this.textApiSkuQuotaData.Text;
                        apiSkuQuota = JsonConvert.DeserializeObject<List<ApiSkuQuotaEntity>>(string.Format("[{0}]", apiSkuQuotaData));
                    }
                    catch (Exception ex)
                    {
                        this.ErrorMessage = string.Format("'SkuQuota' can not be converted!\r\n{0}", ex.Message);
                        MessageBox.Show(this.ErrorMessage, "Json Convert Exception");
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, AlertTitle);
                    return;
                }
                else
                {
                    entity.item = this.textApiItem.Text;
                    entity.title = string.Format("{0}title", ResourceToken);
                    entity.subtitle = string.Format("{0}subtitle", ResourceToken);
                    entity.iconData = string.Format("ms-icon:{0}", this.CBApiIconData.Text);

                    if (resource.ContainsKey("title"))
                    {
                        resource["title"] = this.textApiTitle.Text;
                    }
                    else
                    {
                        resource.Add("title", this.textApiTitle.Text);
                    }

                    if (resource.ContainsKey("subtitle"))
                    {
                        resource["subtitle"] = this.textApiSubTitle.Text;
                    }
                    else
                    {
                        resource.Add("subtitle", this.textApiSubTitle.Text);
                    }

                    string tempCategories = string.Format("[{0}]", this.textApiCategories.Text);
                    List<string> categories = JsonConvert.DeserializeObject<List<string>>(tempCategories, settingFormat);
                    foreach (string category in categories)
                    {
                        entity.categories.Add(category);
                    }

                    entity.skuQuota = apiSkuQuota;
                    entity.showLegalTerm = this.RBApiShowlegalItemTrue.Checked;

                    manualConfigurationData.ApiTypeName = entity.item;
                    manualConfigurationData.ApiItem = JObject.Parse(JsonConvert.SerializeObject(entity, settingFormat));

                    if (manualConfigurationData.Resources.ContainsKey("en"))
                    {
                        manualConfigurationData.Resources["en"] = resource;
                    }
                    else
                    {
                        manualConfigurationData.Resources.Add("en", resource);
                    }

                    this.FlagApi.BackColor = System.Drawing.Color.Green;
                    this.labelTotalResult.Text = "'Api' saved successfully!";

                    this.textApiItem.Text = string.Empty;
                    this.textApiTitle.Text = string.Empty;
                    this.textApiSubTitle.Text = string.Empty;
                    this.CBApiIconData.SelectedItem = string.Empty;
                    this.textApiCategories.Text = "\"CognitiveServices\"";
                    this.CKApiSkuQuotaEmpty.Checked = true;
                    this.textApiSkuQuotaCode.Text = string.Empty;
                    this.textApiSkuQuotaName.Text = string.Empty;
                    this.textApiSkuQuotaQuota.Text = string.Empty;
                    this.textApiSkuQuotaData.Text = string.Empty;
                    this.RBApiShowlegalItemTrue.Checked = true;
                    this.RBApiShowlegalItemFalse.Checked = false;
                }
            }
            catch (Exception ex)
            {
                this.labelTotalResult.Text = "Occured some errors!";
                MessageBox.Show(ex.Message, ExceptionTitle);
            }
        }

        #endregion

        #region 3-3 For QuickStarts

        /// <summary>
        /// Handles the Click event of the BtnQuickStartAddLink control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void BtnQuickStartAddLink_Click(object sender, EventArgs e)
        {
            try
            {
                this.ErrorMessage = string.Empty;
                QuickStartLink quickStartLink = new QuickStartLink();

                if (string.IsNullOrWhiteSpace(this.textQuickStartLinkText.Text))
                {
                    this.ErrorMessage += "'Text' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textQuickStartLinkUri.Text))
                {
                    this.ErrorMessage += "'Uri' should not be empty!\r\n";
                }

                if (!string.IsNullOrWhiteSpace(this.ErrorMessage))
                {
                    MessageBox.Show(this.ErrorMessage, AlertTitle);
                    return;
                }
                else
                {
                    quickStartLink.text = this.textQuickStartLinkText.Text;
                    quickStartLink.uri = this.textQuickStartLinkUri.Text;

                    if (string.IsNullOrWhiteSpace(this.textQuickStartLinks.Text))
                    {
                        this.textQuickStartLinks.Text = JsonConvert.SerializeObject(quickStartLink, settingFormat);
                    }
                    else
                    {
                        this.textQuickStartLinks.Text += ",\r\n" + JsonConvert.SerializeObject(quickStartLink, settingFormat);
                    }

                    //string linkItemData = "{\r\n    \"text\": \"" + this.textQuickStartLinkText.Text + "\",\r\n    \"uri\": \"" + this.textQuickStartLinkUri.Text + "\"\r\n}";
                    //if (string.IsNullOrWhiteSpace(this.textQuickStartLinks.Text))
                    //{
                    //    this.textQuickStartLinks.Text = linkItemData;
                    //}
                    //else
                    //{
                    //    this.textQuickStartLinks.Text += ",\r\n" + linkItemData;
                    //}

                    this.textQuickStartLinkText.Text = string.Empty;
                    this.textQuickStartLinkUri.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                this.labelTotalResult.Text = "Occured some errors!";
                MessageBox.Show(ex.Message, ExceptionTitle);
            }
        }

        /// <summary>
        /// Handles the Click event of the BtnQuickStartAddItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void BtnQuickStartAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.ErrorMessage = string.Empty;
                QucikStartUnit qucikStartUnit = new QucikStartUnit();
                List<QuickStartLink> linkList = new List<QuickStartLink>();

                if (string.IsNullOrWhiteSpace(this.textQuickStartTitle.Text))
                {
                    this.ErrorMessage += "'Title' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textQuickStartIcon.Text))
                {
                    this.ErrorMessage += "'Icon' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textQuickStartDescription.Text))
                {
                    this.ErrorMessage += "'Description' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textQuickStartLinks.Text))
                {
                    this.ErrorMessage += "'Links' should not be empty!\r\n";
                }

                try
                {
                    string tempLinkItemsData = string.Format("[{0}]", this.textQuickStartLinks.Text);
                    linkList = JsonConvert.DeserializeObject<List<QuickStartLink>>(tempLinkItemsData, settingFormat);
                }
                catch (Exception ex)
                {
                    this.ErrorMessage = string.Format("'Links' can not be converted!\r\n{0}", ex.Message);
                    MessageBox.Show(this.ErrorMessage, "Json Convert Exception");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(this.ErrorMessage))
                {
                    MessageBox.Show(this.ErrorMessage, AlertTitle);
                    return;
                }
                else
                {
                    qucikStartUnit.title = this.textQuickStartTitle.Text;
                    qucikStartUnit.icon = this.textQuickStartIcon.Text;
                    qucikStartUnit.description = this.textQuickStartDescription.Text;
                    qucikStartUnit.links = linkList;

                    if (string.IsNullOrWhiteSpace(this.textQuickStartItems.Text))
                    {
                        this.textQuickStartItems.Text = JsonConvert.SerializeObject(qucikStartUnit, settingFormat);
                    }
                    else
                    {
                        this.textQuickStartItems.Text += ",\r\n" + JsonConvert.SerializeObject(qucikStartUnit, settingFormat);
                    }

                    this.textQuickStartTitle.Text = string.Empty;
                    this.textQuickStartDescription.Text = string.Empty;
                    this.textQuickStartIcon.Text = string.Empty;
                    this.textQuickStartLinks.Text = string.Empty;
                    this.textQuickStartLinkText.Text = string.Empty;
                    this.textQuickStartLinkUri.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                this.labelTotalResult.Text = "Occured some errors!";
                MessageBox.Show(ex.Message, ExceptionTitle);
            }
        }

        /// <summary>
        /// Handles the Click event of the BtnSaveQuickStartInfo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void BtnSaveQuickStartInfo_Click(object sender, EventArgs e)
        {
            try
            {
                this.ErrorMessage = string.Empty;
                Dictionary<string, string> resource;
                if (manualConfigurationData.Resources.ContainsKey("en"))
                {
                    resource = manualConfigurationData.Resources["en"];
                }
                else
                {
                    resource = new Dictionary<string, string>();
                }

                if (string.IsNullOrWhiteSpace(this.textQuickStartItems.Text))
                {
                    this.ErrorMessage += "'Items' should not be empty!\r\n";
                }

                if (!string.IsNullOrWhiteSpace(this.ErrorMessage))
                {
                    MessageBox.Show(this.ErrorMessage, AlertTitle);
                    return;
                }

                string tempQuickStartItemsData = "{\"quickStarts\":" + string.Format("[{0}]", this.textQuickStartItems.Text) + "}";

                QuickStartsEntity quickStartsEntity = null;
                try
                {
                    quickStartsEntity = JsonConvert.DeserializeObject<QuickStartsEntity>(tempQuickStartItemsData, settingFormat);
                }
                catch (Exception ex)
                {
                    this.ErrorMessage = string.Format("'Items' can not be converted!\r\n{0}", ex.Message);
                    MessageBox.Show(this.ErrorMessage, "Json Convert Exception");
                    return;
                }

                for (int i = 0; i < quickStartsEntity.quickStarts.Count; i++)
                {
                    QucikStartUnit unit = quickStartsEntity.quickStarts[i];
                    string tempTitle = string.Format("quickStart{0}.title", i);
                    string tempDescription = string.Format("quickStart{0}.des", i);

                    if (resource.ContainsKey(tempTitle))
                    {
                        resource[tempTitle] = unit.title;
                    }
                    else
                    {
                        resource.Add(tempTitle, unit.title);
                    }

                    quickStartsEntity.quickStarts[i].title = string.Format("{0}{1}", ResourceToken, tempTitle);

                    if (resource.ContainsKey(tempDescription))
                    {
                        resource[tempDescription] = unit.description;
                    }
                    else
                    {
                        resource.Add(tempDescription, unit.description);
                    }

                    quickStartsEntity.quickStarts[i].description = string.Format("{0}{1}", ResourceToken, tempDescription);

                    for (int j = 0; j < unit.links.Count; j++)
                    {
                        QuickStartLink link = quickStartsEntity.quickStarts[i].links[j];
                        string tempLinkText = string.Format("quickStart{0}.link{1}.text", i, j);

                        if (resource.ContainsKey(tempLinkText))
                        {
                            resource[tempLinkText] = link.text;
                        }
                        else
                        {
                            resource.Add(tempLinkText, link.text);
                        }

                        quickStartsEntity.quickStarts[i].links[j].text = string.Format("{0}{1}", ResourceToken, tempLinkText);
                    }
                }

                manualConfigurationData.QuickStart = JObject.Parse(JsonConvert.SerializeObject(quickStartsEntity, settingFormat));

                if (manualConfigurationData.Resources.ContainsKey("en"))
                {
                    manualConfigurationData.Resources["en"] = resource;
                }
                else
                {
                    manualConfigurationData.Resources.Add("en", resource);
                }

                this.FlagQuickStart.BackColor = System.Drawing.Color.Green;
                this.labelTotalResult.Text = "'QuickStarts' saved successfully!";
            }
            catch (Exception ex)
            {
                this.labelTotalResult.Text = "Occured some errors!";
                MessageBox.Show(ex.Message, ExceptionTitle);
            }
        }

        #endregion

        #region 3-4 For Spec

        /// <summary>
        /// Handles the MouseCaptureChanged event of the tabControl3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void tabControl3_MouseCaptureChanged(object sender, EventArgs e)
        {
            try
            {
                this.labelCurrentSubTab.Text = this.tabControl3.SelectedTab.Text;
                List<string> specIds = new List<string>();
                List<SpecUnitEntity> specItems = null;

                if (!string.IsNullOrWhiteSpace(this.textSpecItems.Text))
                {
                    try
                    {
                        string tempSpecItems = string.Format("[{0}]", this.textSpecItems.Text);
                        specItems = JsonConvert.DeserializeObject<List<SpecUnitEntity>>(tempSpecItems, settingFormat);
                    }
                    catch
                    {
                        MessageBox.Show("'Spec Items' can not be converted!", AlertTitle);
                        return;
                    }
                }

                if (this.tabControl3.SelectedTab == this.TabSpecFeatures)
                {
                    this.CBSpecFeaturesID.Items.Clear();
                    if (specItems != null)
                    {
                        this.CBSpecFeaturesID.Items.Add("");
                        foreach (SpecUnitEntity entity in specItems)
                        {
                            foreach (SpecFeatureUnit feature in entity.features)
                            {
                                if (!this.CBSpecFeaturesID.Items.Contains(feature.id))
                                {
                                    this.CBSpecFeaturesID.Items.Add(feature.id);
                                }
                            }
                        }
                    }

                    this.CBSpecFeaturesIconData.Items.Clear();
                    if (manualConfigurationData.Icons.Count > 0)
                    {
                        this.CBSpecFeaturesIconData.Items.Add("");
                        foreach (var icon in manualConfigurationData.Icons)
                        {
                            this.CBSpecFeaturesIconData.Items.Add(icon.Key);
                        }
                    }
                }
                else if (this.tabControl3.SelectedTab == this.TabSpecResourceMap)
                {
                    this.CBSpecResourceMapDefaultItemId.Items.Clear();
                    if (specItems != null)
                    {
                        this.CBSpecResourceMapDefaultItemId.Items.Add("");
                        foreach (SpecUnitEntity entity in specItems)
                        {
                            this.CBSpecResourceMapDefaultItemId.Items.Add(entity.id);
                        }
                    }
                }
                else if (this.tabControl3.SelectedTab == this.TabSpecAllowZero)
                {
                    this.CBSpecAllowZerpCostID.Items.Clear();
                    if (specItems != null)
                    {
                        this.CBSpecAllowZerpCostID.Items.Add("");
                        foreach (SpecUnitEntity entity in specItems)
                        {
                            this.CBSpecAllowZerpCostID.Items.Add(entity.id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.labelTotalResult.Text = "Occured some errors!";
                MessageBox.Show(ex.Message, ExceptionTitle);
                return;
            }
        }

        #region 3-4-1 Specs

        /// <summary>
        /// Handles the Click event of the BtnSpecAddPromotedFeatures control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnSpecAddPromotedFeatures_Click(object sender, EventArgs e)
        {
            try
            {
                this.ErrorMessage = string.Empty;
                SpecPromotedFeature specPromotedFeature = new SpecPromotedFeature();

                if (string.IsNullOrWhiteSpace(this.textSpecPromotedFeatureValue.Text))
                {
                    this.ErrorMessage += "'Value' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textSpecPromotedFeatureUnitDescription.Text))
                {
                    this.ErrorMessage += "'UnitDescription' should not be empty!\r\n";
                }

                if (!string.IsNullOrWhiteSpace(this.ErrorMessage))
                {
                    MessageBox.Show(this.ErrorMessage, AlertTitle);
                    return;
                }
                else
                {
                    specPromotedFeature.value = this.textSpecPromotedFeatureValue.Text;
                    specPromotedFeature.unitDescription = this.textSpecPromotedFeatureUnitDescription.Text;

                    if (string.IsNullOrWhiteSpace(this.textSpecPromotedFeatureItems.Text))
                    {
                        this.textSpecPromotedFeatureItems.Text = JsonConvert.SerializeObject(specPromotedFeature, settingFormat);
                    }
                    else
                    {
                        this.textSpecPromotedFeatureItems.Text += ",\r\n" + JsonConvert.SerializeObject(specPromotedFeature, settingFormat);
                    }

                    this.textSpecPromotedFeatureValue.Text = string.Empty;
                    this.textSpecPromotedFeatureUnitDescription.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                this.labelTotalResult.Text = "Occured some errors!";
                MessageBox.Show(ex.Message, ExceptionTitle);
            }
        }

        /// <summary>
        /// Handles the Click event of the BtnSpecAddInnerFeature control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnSpecAddInnerFeature_Click(object sender, EventArgs e)
        {
            try
            {
                this.ErrorMessage = string.Empty;
                SpecFeatureUnit specFeatureUnit = new SpecFeatureUnit();

                if (string.IsNullOrWhiteSpace(this.textSpecInnerFeatureId.Text))
                {
                    this.ErrorMessage += "'ID' should not be empty!\r\n";
                }

                if (!string.IsNullOrWhiteSpace(this.ErrorMessage))
                {
                    MessageBox.Show(this.ErrorMessage, AlertTitle);
                    return;
                }
                else
                {
                    specFeatureUnit.id = this.textSpecInnerFeatureId.Text;
                    if (string.IsNullOrWhiteSpace(this.textSpecInnerFeatureItems.Text))
                    {
                        this.textSpecInnerFeatureItems.Text = JsonConvert.SerializeObject(specFeatureUnit, settingFormat);
                    }
                    else
                    {
                        this.textSpecInnerFeatureItems.Text += ",\r\n" + JsonConvert.SerializeObject(specFeatureUnit, settingFormat);
                    }

                    this.textSpecInnerFeatureId.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                this.labelTotalResult.Text = "Occured some errors!";
                MessageBox.Show(ex.Message, ExceptionTitle);
            }
        }

        /// <summary>
        /// Handles the Click event of the BtnSpecAddSpecItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnSpecAddSpecItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.ErrorMessage = string.Empty;

                SpecUnitEntity specUnitEntity = new SpecUnitEntity();
                SpecCost specCost = new SpecCost();

                if (string.IsNullOrWhiteSpace(this.textSpecItemID.Text))
                {
                    this.ErrorMessage += "'ID' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textSpecItemColorScheme.Text))
                {
                    this.ErrorMessage += "'ColorScheme' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textSpecItemTitle.Text))
                {
                    this.ErrorMessage += "'Title' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textSpecItemSpecCode.Text))
                {
                    this.ErrorMessage += "'SpecCode' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textSpecPromotedFeatureItems.Text))
                {
                    this.ErrorMessage += "'PromotedFeatures' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textSpecInnerFeatureItems.Text))
                {
                    this.ErrorMessage += "'Features' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textSpecCostAccount.Text) && !string.IsNullOrWhiteSpace(this.textSpecCostCurrencyCode.Text))
                {
                    this.ErrorMessage += "'Account' should not be empty!\r\n";
                }

                if (!string.IsNullOrWhiteSpace(this.textSpecCostAccount.Text) && string.IsNullOrWhiteSpace(this.textSpecCostCurrencyCode.Text))
                {
                    this.ErrorMessage += "'CurrencyCode' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textSpecCostCaption.Text))
                {
                    this.ErrorMessage += "'Caption' should not be empty!\r\n";
                }

                if (!string.IsNullOrWhiteSpace(this.ErrorMessage))
                {
                    MessageBox.Show(this.ErrorMessage, AlertTitle);
                    return;
                }

                List<SpecPromotedFeature> tempSpecPromotedFeatures = null;
                try
                {
                    string tempPromotedFeatureItems = string.Format("[{0}]", this.textSpecPromotedFeatureItems.Text);
                    tempSpecPromotedFeatures = JsonConvert.DeserializeObject<List<SpecPromotedFeature>>(tempPromotedFeatureItems, settingFormat);
                }
                catch (Exception ex)
                {
                    this.labelTotalResult.Text = "Occured some errors!";
                    MessageBox.Show(ex.Message, ExceptionTitle);
                    return;
                }

                List<SpecFeatureUnit> tempSpecInnerFeatures = null;
                try
                {
                    string tempFeatureItems = string.Format("[{0}]", this.textSpecInnerFeatureItems.Text);
                    tempSpecInnerFeatures = JsonConvert.DeserializeObject<List<SpecFeatureUnit>>(tempFeatureItems, settingFormat);
                }
                catch (Exception ex)
                {
                    this.labelTotalResult.Text = "Occured some errors!";
                    MessageBox.Show(ex.Message, ExceptionTitle);
                    return;
                }

                try
                {
                    if (!string.IsNullOrWhiteSpace(this.textSpecCostAccount.Text))
                    {
                        specCost.amount = float.Parse(this.textSpecCostAccount.Text);
                    }
                }
                catch
                {
                    MessageBox.Show("'Account' should be a float number!", ExceptionTitle);
                    return;
                }

                specUnitEntity.id = this.textSpecItemID.Text;
                specUnitEntity.colorScheme = this.textSpecItemColorScheme.Text;
                specUnitEntity.title = this.textSpecItemTitle.Text;
                specUnitEntity.specCode = this.textSpecItemSpecCode.Text;

                specUnitEntity.promotedFeatures = tempSpecPromotedFeatures;
                specUnitEntity.features = tempSpecInnerFeatures;
                if (!string.IsNullOrWhiteSpace(this.textSpecCostCurrencyCode.Text))
                {
                    specCost.currencyCode = this.textSpecCostCurrencyCode.Text;
                }

                specCost.caption = this.textSpecCostCaption.Text;
                specUnitEntity.cost = specCost;

                if (string.IsNullOrWhiteSpace(this.textSpecItems.Text))
                {
                    this.textSpecItems.Text = JsonConvert.SerializeObject(specUnitEntity, specialSettingFormat);
                }
                else
                {
                    this.textSpecItems.Text += ",\r\n" + JsonConvert.SerializeObject(specUnitEntity, specialSettingFormat);
                }

                this.textSpecItemID.Text = string.Empty;
                //this.textSpecItemColorScheme.Text = string.Empty;
                this.textSpecItemTitle.Text = string.Empty;
                this.textSpecItemSpecCode.Text = string.Empty;
                this.textSpecPromotedFeatureItems.Text = string.Empty;
                //this.textSpecInnerFeatureItems.Text = string.Empty;
                this.textSpecCostAccount.Text = string.Empty;
                this.textSpecCostCurrencyCode.Text = string.Empty;
                this.textSpecCostCaption.Text = string.Empty;
            }
            catch (Exception ex)
            {
                this.labelTotalResult.Text = "Occured some errors!";
                MessageBox.Show(ex.Message, ExceptionTitle);
            }
        }

        #endregion

        #region 3-4-2 Features

        /// <summary>
        /// Handles the Click event of the BtnSpecAddFeatureItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnSpecAddFeatureItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.ErrorMessage = string.Empty;
                SpecFeatureEntity specFeatureEntity = new SpecFeatureEntity();

                if (string.IsNullOrWhiteSpace(this.CBSpecFeaturesID.Text))
                {
                    this.ErrorMessage += "'ID' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textSpecFeatureDisplayName.Text))
                {
                    this.ErrorMessage += "'DisplayName' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.CBSpecFeaturesIconData.Text) && string.IsNullOrWhiteSpace(this.textSpecFeatureIconName.Text))
                {
                    this.ErrorMessage += "'IconSvgData' and 'IconName' should not all be empty!\r\n";
                }

                if (!string.IsNullOrWhiteSpace(this.ErrorMessage))
                {
                    MessageBox.Show(this.ErrorMessage, AlertTitle);
                    return;
                }
                else
                {
                    specFeatureEntity.id = this.CBSpecFeaturesID.Text;
                    specFeatureEntity.displayName = this.textSpecFeatureDisplayName.Text;
                    if (!string.IsNullOrWhiteSpace(this.CBSpecFeaturesIconData.Text))
                    {
                        specFeatureEntity.iconSvgData = this.CBSpecFeaturesIconData.Text;
                    }

                    if (!string.IsNullOrWhiteSpace(this.textSpecFeatureIconName.Text))
                    {
                        specFeatureEntity.iconName = this.textSpecFeatureIconName.Text;
                    }

                    if (string.IsNullOrWhiteSpace(this.textSpecFeatureItems.Text))
                    {
                        this.textSpecFeatureItems.Text = JsonConvert.SerializeObject(specFeatureEntity, settingFormat);
                    }
                    else
                    {
                        this.textSpecFeatureItems.Text += ",\r\n" + JsonConvert.SerializeObject(specFeatureEntity, settingFormat);
                    }

                    this.CBSpecFeaturesID.SelectedItem = string.Empty;
                    this.textSpecFeatureDisplayName.Text = string.Empty;
                    this.CBSpecFeaturesIconData.SelectedItem = string.Empty;
                    this.textSpecFeatureIconName.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                this.labelTotalResult.Text = "Occured some errors!";
                MessageBox.Show(ex.Message, ExceptionTitle);
            }
        }

        #endregion

        #region 3-4-3 ResourceMap

        /// <summary>
        /// Handles the Click event of the BtnSpecAddFirstParty control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnSpecAddFirstParty_Click(object sender, EventArgs e)
        {
            try
            {
                this.ErrorMessage = string.Empty;
                SpecFirstParty firstParty = new SpecFirstParty();

                if (string.IsNullOrWhiteSpace(this.textSpecResourceMapItemResourceId.Text))
                {
                    this.ErrorMessage += "'ResourceId' should not be empty!\r\n";
                }
                if (string.IsNullOrWhiteSpace(this.textSpecResourceMapItemQuantity.Text))
                {
                    this.ErrorMessage += "'Quantity' should not be empty!\r\n";
                }

                if (!string.IsNullOrWhiteSpace(this.ErrorMessage))
                {
                    MessageBox.Show(this.ErrorMessage, AlertTitle);
                    return;
                }

                try
                {
                    firstParty.quantity = float.Parse(this.textSpecResourceMapItemQuantity.Text);
                }
                catch
                {
                    MessageBox.Show("'Quantity' should be float!", AlertTitle);
                    return;
                }
                firstParty.resourceId = this.textSpecResourceMapItemResourceId.Text;

                if (string.IsNullOrWhiteSpace(this.textSpecResourceMapFirstPartyItems.Text))
                {
                    this.textSpecResourceMapFirstPartyItems.Text = JsonConvert.SerializeObject(firstParty, settingFormat);
                }
                else
                {
                    this.textSpecResourceMapFirstPartyItems.Text += ",\r\n" + JsonConvert.SerializeObject(firstParty, settingFormat);
                }

                this.textSpecResourceMapItemResourceId.Text = string.Empty;
                this.textSpecResourceMapItemQuantity.Text = string.Empty;
            }
            catch (Exception ex)
            {
                this.labelTotalResult.Text = "Occured some errors!";
                MessageBox.Show(ex.Message, ExceptionTitle);
                return;
            }
        }

        /// <summary>
        /// Handles the Click event of the BtnSpecAddResourceMapDefaultItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnSpecAddResourceMapDefaultItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.ErrorMessage = string.Empty;
                SpecResourceMapDefault defaultEntity = new SpecResourceMapDefault();

                if (string.IsNullOrWhiteSpace(this.CBSpecResourceMapDefaultItemId.Text))
                {
                    this.ErrorMessage += "'ID' should not be empty!\r\n";
                }
                if (string.IsNullOrWhiteSpace(this.textSpecResourceMapFirstPartyItems.Text))
                {
                    this.ErrorMessage += "'FirstParty' should not be empty!\r\n";
                }

                if (!string.IsNullOrWhiteSpace(this.ErrorMessage))
                {
                    MessageBox.Show(this.ErrorMessage, AlertTitle);
                    return;
                }

                try
                {
                    string tempSpecResourceMapFirstPartyItems = string.Format("[{0}]", this.textSpecResourceMapFirstPartyItems.Text);
                    defaultEntity.firstParty = JsonConvert.DeserializeObject<List<SpecFirstParty>>(tempSpecResourceMapFirstPartyItems, settingFormat);
                }
                catch
                {
                    MessageBox.Show("'FirstParty' can not be convert!", AlertTitle);
                    return;
                }

                defaultEntity.id = this.CBSpecResourceMapDefaultItemId.Text;

                if (string.IsNullOrWhiteSpace(this.textSpecResourceMapDefaultItems.Text))
                {
                    this.textSpecResourceMapDefaultItems.Text = JsonConvert.SerializeObject(defaultEntity, settingFormat);
                }
                else
                {
                    this.textSpecResourceMapDefaultItems.Text += ",\r\n" + JsonConvert.SerializeObject(defaultEntity, settingFormat);
                }

                this.CBSpecResourceMapDefaultItemId.SelectedItem = string.Empty;
                this.textSpecResourceMapItemResourceId.Text = string.Empty;
                this.textSpecResourceMapItemQuantity.Text = string.Empty;
                this.textSpecResourceMapFirstPartyItems.Text = string.Empty;
            }
            catch (Exception ex)
            {
                this.labelTotalResult.Text = "Occured some errors!";
                MessageBox.Show(ex.Message, ExceptionTitle);
                return;
            }
        }

        #endregion

        #region 3-4-4 SpecsToAllowZeroCost

        /// <summary>
        /// Handles the Click event of the BtnSpecAllowZeroCost control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnSpecAllowZeroCost_Click(object sender, EventArgs e)
        {
            this.ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.CBSpecAllowZerpCostID.Text))
            {
                this.ErrorMessage += "'ID' should not be empty!";
            }

            if (!string.IsNullOrWhiteSpace(this.ErrorMessage))
            {
                MessageBox.Show(this.ErrorMessage, AlertTitle);
                return;
            }

            if (string.IsNullOrWhiteSpace(this.textSpecAllowZeroCostIDs.Text))
            {
                this.textSpecAllowZeroCostIDs.Text = string.Format("\"{0}\"", this.CBSpecAllowZerpCostID.Text);
            }
            else
            {
                this.textSpecAllowZeroCostIDs.Text += string.Format(",\r\n\"{0}\"", this.CBSpecAllowZerpCostID.Text);
            }

            this.CBSpecAllowZerpCostID.SelectedItem = string.Empty;
        }

        #endregion

        /// <summary>
        /// Handles the Click event of the BtnSaveSpecInfo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnSaveSpecInfo_Click(object sender, EventArgs e)
        {
            try
            {
                this.ErrorMessage = string.Empty;
                Dictionary<string, string> resource;
                if (manualConfigurationData.Resources.ContainsKey("en"))
                {
                    resource = manualConfigurationData.Resources["en"];
                }
                else
                {
                    resource = new Dictionary<string, string>();
                }

                SpecsEntity specEntity = new SpecsEntity();

                if (string.IsNullOrWhiteSpace(this.textSpecType.Text))
                {
                    this.ErrorMessage += "'SpecType' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textSpecItems.Text))
                {
                    this.ErrorMessage += "'Spec Items' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textSpecFeatureItems.Text))
                {
                    this.ErrorMessage += "'Feature Items' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textSpecResourceMapDefaultItems.Text))
                {
                    this.ErrorMessage += "'Default Items' should not be empty!\r\n";
                }

                if (string.IsNullOrWhiteSpace(this.textSpecAllowZeroCostIDs.Text))
                {
                    this.ErrorMessage += "'SpecsToAllowZeroCost' should not be empty!\r\n";
                }

                if (!string.IsNullOrWhiteSpace(this.ErrorMessage))
                {
                    MessageBox.Show(this.ErrorMessage, AlertTitle);
                    return;
                }

                specEntity.specType = this.textSpecType.Text;

                try
                {
                    string temoSpecItems = string.Format("[{0}]", this.textSpecItems.Text);
                    specEntity.specs = JsonConvert.DeserializeObject<List<SpecUnitEntity>>(temoSpecItems, settingFormat);
                }
                catch
                {
                    MessageBox.Show("'Spec Items' can not be converted!", AlertTitle);
                    return;
                }

                try
                {
                    string tempFeatureItems = string.Format("[{0}]", this.textSpecFeatureItems.Text);
                    specEntity.features = JsonConvert.DeserializeObject<List<SpecFeatureEntity>>(tempFeatureItems, settingFormat);
                }
                catch
                {
                    MessageBox.Show("'Feature Items' can not be converted!", AlertTitle);
                    return;
                }

                try
                {
                    string tempDefaultItems = string.Format("[{0}]", this.textSpecResourceMapDefaultItems.Text);
                    SpecResourceMapEntity resourceMapEntity = new SpecResourceMapEntity();
                    resourceMapEntity.specResourceMapDefault = JsonConvert.DeserializeObject<List<SpecResourceMapDefault>>(tempDefaultItems, settingFormat);
                    specEntity.resourceMap = resourceMapEntity;
                }
                catch
                {
                    MessageBox.Show("'Default Items' can not be converted!", AlertTitle);
                    return;
                }

                try
                {
                    string tempAllowZeroCostItems = string.Format("[{0}]", this.textSpecAllowZeroCostIDs.Text);
                    specEntity.specsToAllowZeroCost = JsonConvert.DeserializeObject<List<string>>(tempAllowZeroCostItems, settingFormat);
                }
                catch
                {
                    MessageBox.Show("'SpecsToAllowZeroCost' can not be converted!", AlertTitle);
                    return;
                }

                for (int i = 0; i < specEntity.specs.Count; i++)
                {
                    SpecUnitEntity specItem = specEntity.specs[i];
                    string tempSpecItemTitle = string.Format("spec.{0}.title", specItem.id);
                    if (resource.ContainsKey(tempSpecItemTitle))
                    {
                        resource[tempSpecItemTitle] = specItem.title;
                    }
                    else
                    {
                        resource.Add(tempSpecItemTitle, specItem.title);
                    }

                    specEntity.specs[i].title = string.Format("{0}{1}", ResourceToken, tempSpecItemTitle);

                    for (int j = 0; j < specEntity.specs[i].promotedFeatures.Count; j++)
                    {
                        SpecPromotedFeature promotedFeature = specEntity.specs[i].promotedFeatures[j];
                        string tempUnitDescription = string.Format("spec.promotedFeature.unitDescription{0}", j);
                        if (resource.ContainsValue(promotedFeature.unitDescription))
                        {
                            tempUnitDescription = resource.FirstOrDefault(d => d.Value == promotedFeature.unitDescription).Key;
                            specEntity.specs[i].promotedFeatures[j].unitDescription = string.Format("{0}{1}", ResourceToken, tempUnitDescription);
                        }
                        else
                        {
                            int tempIndex = 0;
                            while (true)
                            {
                                tempUnitDescription = string.Format("spec.promotedFeature.unitDescription{0}", tempIndex);
                                if (!resource.ContainsKey(tempUnitDescription))
                                {
                                    resource.Add(tempUnitDescription, promotedFeature.unitDescription);
                                    specEntity.specs[i].promotedFeatures[j].unitDescription = string.Format("{0}{1}", ResourceToken, tempUnitDescription);
                                    break;
                                }

                                tempIndex++;
                            }
                        }
                    }

                    string tempSpecItemCostCaption = string.Format("spec.cost.{0}.caption.format", specItem.id);
                    if (resource.ContainsKey(tempSpecItemCostCaption))
                    {
                        resource[tempSpecItemCostCaption] = specItem.cost.caption;
                    }
                    else
                    {
                        resource.Add(tempSpecItemCostCaption, specItem.cost.caption);
                    }

                    specEntity.specs[i].cost.caption = string.Format("{0}{1}", ResourceToken, tempSpecItemCostCaption);
                }

                for (int i = 0; i < specEntity.features.Count; i++)
                {
                    SpecFeatureEntity specFeature = specEntity.features[i];

                    string tempDisplayName = string.Format("feature.{0}.displayName", specFeature.id);
                    if (resource.ContainsKey(tempDisplayName))
                    {
                        resource[tempDisplayName] = specFeature.displayName;
                    }
                    else
                    {
                        resource.Add(tempDisplayName, specFeature.displayName);
                    }

                    specEntity.features[i].displayName = string.Format("{0}{1}", ResourceToken, tempDisplayName);

                    if (!string.IsNullOrWhiteSpace(specEntity.features[i].iconSvgData))
                    {
                        specEntity.features[i].iconSvgData = string.Format("{0}{1}", IconToken, specEntity.features[i].iconSvgData);
                    }
                }

                manualConfigurationData.Spec = JObject.Parse(JsonConvert.SerializeObject(specEntity, settingFormat));
                if (manualConfigurationData.Resources.ContainsKey("en"))
                {
                    manualConfigurationData.Resources["en"] = resource;
                }
                else
                {
                    manualConfigurationData.Resources.Add("en", resource);
                }

                this.FlagSpec.BackColor = System.Drawing.Color.Green;
                this.labelTotalResult.Text = "'Spec' saved successfully!";
            }
            catch (Exception ex)
            {
                this.labelTotalResult.Text = "Occured some errors!";
                MessageBox.Show(ex.Message, ExceptionTitle);
                return;
            }
        }

        #endregion

        #region 3-5 For Overall

        /// <summary>
        /// Handles the Click event of the BtnOverallRefresh control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void BtnOverallReload_Click(object sender, EventArgs e)
        {
            try
            {
                this.textOverallApiJsonContent.Text = string.Empty;
                this.textOverallQuickStartJsonContent.Text = string.Empty;
                this.textOverallSpecJsonContent.Text = string.Empty;
                this.textOverallResourceJsonContent.Text = string.Empty;
                this.textOverallSvgIconsList.Text = string.Empty;

                this.textOverallApiJsonContent.Text = JsonConvert.SerializeObject(manualConfigurationData.ApiItem, settingFormat);
                this.textOverallQuickStartJsonContent.Text = JsonConvert.SerializeObject(manualConfigurationData.QuickStart, settingFormat);
                this.textOverallSpecJsonContent.Text = JsonConvert.SerializeObject(manualConfigurationData.Spec, settingFormat);

                if (manualConfigurationData.Resources.ContainsKey("en"))
                {
                    this.textOverallResourceJsonContent.Text = JsonConvert.SerializeObject(manualConfigurationData.Resources["en"], settingFormat);
                }
                else
                {
                    this.textOverallResourceJsonContent.Text = "null";
                }

                if (manualConfigurationData.Icons.Count != 0)
                {
                    foreach (var icon in manualConfigurationData.Icons)
                    {
                        this.textOverallSvgIconsList.Text += icon.Key + "\r\n";
                    }
                }
                else
                {
                    this.textOverallSvgIconsList.Text = "null";
                }
            }
            catch (Exception ex)
            {
                this.labelTotalResult.Text = "Occured some errors!";
                MessageBox.Show(ex.Message, ExceptionTitle);
            }
        }

        /// <summary>
        /// Handles the Click event of the BtnGenerateZipFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnGenerateZipFile_Click(object sender, EventArgs e)
        {
            try
            {
                this.ErrorMessage = string.Empty;
                ApiConfigurationData tempConfigurationData = new ApiConfigurationData();

                if (this.textOverallApiJsonContent.Text == "null")
                {
                    this.ErrorMessage += "'Api Json Content' should not be null!\r\n";
                }

                if (this.textOverallQuickStartJsonContent.Text == "null")
                {
                    this.ErrorMessage += "'QuickStart Json Content' should not be null!\r\n";
                }

                if (this.textOverallSpecJsonContent.Text == "null")
                {
                    this.ErrorMessage += "'Spec Json Content' should not be null!\r\n";
                }

                if (this.textOverallResourceJsonContent.Text == "null")
                {
                    this.ErrorMessage += "Resource (en)' should not be null!\r\n";
                }

                if (this.textOverallSvgIconsList.Text == "null")
                {
                    this.ErrorMessage += "There is no svg file";
                }

                if (!string.IsNullOrWhiteSpace(this.ErrorMessage))
                {
                    MessageBox.Show(this.ErrorMessage, AlertTitle);
                    return;
                }

                try
                {
                    ApiEntity apiEntity = JsonConvert.DeserializeObject<ApiEntity>(this.textOverallApiJsonContent.Text, settingFormat);
                    tempConfigurationData.ApiItem = JObject.Parse(this.textOverallApiJsonContent.Text);
                }
                catch
                {
                    MessageBox.Show("'Api Json Content' can not be converted!", AlertTitle);
                    return;
                }

                try
                {
                    QuickStartsEntity quickStartsEntity = JsonConvert.DeserializeObject<QuickStartsEntity>(this.textOverallQuickStartJsonContent.Text, settingFormat);
                    tempConfigurationData.QuickStart = JObject.Parse(this.textOverallQuickStartJsonContent.Text);
                }
                catch
                {
                    MessageBox.Show("'QuickStart Json Content' can not be converted!", AlertTitle);
                    return;
                }

                try
                {
                    SpecsEntity specsEntity = JsonConvert.DeserializeObject<SpecsEntity>(this.textOverallSpecJsonContent.Text, settingFormat);
                    tempConfigurationData.Spec = JObject.Parse(this.textOverallSpecJsonContent.Text);
                }
                catch
                {
                    MessageBox.Show("'Spec Json Content' can not be converted!", AlertTitle);
                    return;
                }

                try
                {
                    Dictionary<string, string> resource = JsonConvert.DeserializeObject<Dictionary<string, string>>(this.textOverallResourceJsonContent.Text, settingFormat);
                    tempConfigurationData.Resources.Add("en", resource);
                }
                catch
                {
                    MessageBox.Show("'Resource (en)' can not be converted!", AlertTitle);
                    return;
                }

                var tempIcons = manualConfigurationData.Icons;
                tempConfigurationData.Icons = tempIcons;
                tempConfigurationData.ApiTypeName = manualConfigurationData.ApiTypeName;

                try
                {
                    string ApiCongigurationTestBlobAccountName = CloudConfigurationManager.GetSetting("ApiCongigurationTestBlobAccountName");
                    string ApiConfigurationTestBlobAccountKey = CloudConfigurationManager.GetSetting("ApiConfigurationTestBlobAccountKey");

                    if (string.IsNullOrWhiteSpace(ApiCongigurationTestBlobAccountName))
                    {
                        MessageBox.Show("Not find the key 'ApiCongigurationTestBlobAccountName' in App.config or its value is empty.", AlertTitle);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(ApiConfigurationTestBlobAccountKey))
                    {
                        MessageBox.Show("Not find the key 'ApiConfigurationTestBlobAccountKey' in App.config or its value is empty.", AlertTitle);
                        return;
                    }

                    StorageCredentials credentials = new StorageCredentials(ApiCongigurationTestBlobAccountName, ApiConfigurationTestBlobAccountKey, "AccountKey");
                    BlobHelper.UploadApiConfigurationToBlobContainer(credentials, apiConfigurationStorageContainer, tempConfigurationData, tempFolderPath);

                    InitVisualConfiguration();
                    this.labelTotalResult.Text = "Genarate and upload successfully!";
                }
                catch (Exception ex)
                {
                    this.textMessage1.Text = string.Format("Original Error Message: {0}", ex.Message);
                }

            }
            catch (Exception ex)
            {
                this.labelTotalResult.Text = "Occured some errors!";
                MessageBox.Show(ex.Message, ExceptionTitle);
            }
        }

        #endregion

        /// <summary>
        /// Handles the CheckedChanged event of the CBEnableAllGeneratedItems control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CBEnableAllGeneratedItems_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CBEnableAllGeneratedItems.Checked)
            {
                this.textApiCategories.ReadOnly = false;
                this.textApiSkuQuotaData.ReadOnly = false;

                this.textQuickStartLinks.ReadOnly = false;
                this.textQuickStartItems.ReadOnly = false;

                this.textSpecPromotedFeatureItems.ReadOnly = false;
                this.textSpecInnerFeatureItems.ReadOnly = false;
                this.textSpecItems.ReadOnly = false;

                this.textSpecFeatureItems.ReadOnly = false;

                this.textSpecResourceMapFirstPartyItems.ReadOnly = false;
                this.textSpecResourceMapDefaultItems.ReadOnly = false;

                this.textSpecAllowZeroCostIDs.ReadOnly = false;

                this.textOverallApiJsonContent.ReadOnly = false;
                this.textOverallQuickStartJsonContent.ReadOnly = false;
                this.textOverallSpecJsonContent.ReadOnly = false;
                this.textOverallResourceJsonContent.ReadOnly = false;
            }
            else
            {
                this.textApiCategories.ReadOnly = true;
                this.textApiSkuQuotaData.ReadOnly = true;

                this.textQuickStartLinks.ReadOnly = true;
                this.textQuickStartItems.ReadOnly = true;

                this.textSpecPromotedFeatureItems.ReadOnly = true;
                this.textSpecInnerFeatureItems.ReadOnly = true;
                this.textSpecItems.ReadOnly = true;

                this.textSpecFeatureItems.ReadOnly = true;

                this.textSpecResourceMapFirstPartyItems.ReadOnly = true;
                this.textSpecResourceMapDefaultItems.ReadOnly = true;

                this.textSpecAllowZeroCostIDs.ReadOnly = true;

                this.textOverallApiJsonContent.ReadOnly = true;
                this.textOverallQuickStartJsonContent.ReadOnly = true;
                this.textOverallSpecJsonContent.ReadOnly = true;
                this.textOverallResourceJsonContent.ReadOnly = true;
            }
        }

        /// <summary>
        /// Initializes the visual configuration.
        /// </summary>
        private void InitVisualConfiguration()
        {
            manualConfigurationData = new ApiConfigurationData();

            this.FlagIcon.BackColor = System.Drawing.Color.Red;
            this.FlagApi.BackColor = System.Drawing.Color.Red;
            this.FlagQuickStart.BackColor = System.Drawing.Color.Red;
            this.FlagSpec.BackColor = System.Drawing.Color.Red;

            this.textIconFolderPath.Text = string.Empty;
            this.textSvgFiles.Text = string.Empty;

            this.CBApiIconData.Items.Clear();

            this.textQuickStartItems.Text = string.Empty;

            this.textSpecType.Text = string.Empty;
            this.textSpecItemColorScheme.Text = string.Empty;
            this.textSpecInnerFeatureItems.Text = string.Empty;
            this.textSpecItems.Text = string.Empty;

            this.CBSpecFeaturesID.Items.Clear();
            this.CBSpecFeaturesIconData.Items.Clear();
            this.textSpecFeatureItems.Text = string.Empty;

            this.CBSpecResourceMapDefaultItemId.Items.Clear();
            this.textSpecResourceMapDefaultItems.Text = string.Empty;

            this.CBSpecAllowZerpCostID.Items.Clear();
            this.textSpecAllowZeroCostIDs.Text = string.Empty;

            this.textOverallApiJsonContent.Text = "null";
            this.textOverallQuickStartJsonContent.Text = "null";
            this.textOverallSpecJsonContent.Text = "null";
            this.textOverallResourceJsonContent.Text = "null";
            this.textOverallSvgIconsList.Text = "null";
        }

        #endregion

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textFilePath1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textMessage1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox11_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabControl22_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void textMessage2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void textFileContent2_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox22_Enter(object sender, EventArgs e)
        {

        }

        private void tabControl21_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void root2_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void tabPage7_Click(object sender, EventArgs e)
        {

        }

        private void groupBox21_Enter(object sender, EventArgs e)
        {

        }

        private void textAccount2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textAccountKey2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void FlagSpec_Click(object sender, EventArgs e)
        {

        }

        private void FlagQuickStart_Click(object sender, EventArgs e)
        {

        }

        private void FlagIcon_Click(object sender, EventArgs e)
        {

        }

        private void FlagApi_Click(object sender, EventArgs e)
        {

        }

        private void labelTotalResult_Click(object sender, EventArgs e)
        {

        }

        private void labelCurrentTab_Click(object sender, EventArgs e)
        {

        }

        private void TabIcon_Click(object sender, EventArgs e)
        {

        }

        private void label31_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textSvgFiles_TextChanged(object sender, EventArgs e)
        {

        }

        private void textIconFolderPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void TabApi_Click(object sender, EventArgs e)
        {

        }

        private void groupBox25_Enter(object sender, EventArgs e)
        {

        }

        private void textApiCategory_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textApiCategories_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void RBApiShowlegalItemTrue_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RBApiShowlegalItemFalse_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textApiSkuQuotaData_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textApiSkuQuotaCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void textApiSkuQuotaQuota_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textApiSkuQuotaName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void CBApiIconData_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textApiSubTitle_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textApiTitle_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textApiItem_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void TabQuickStarts_Click(object sender, EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void textQuickStartItems_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void textQuickStartDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void textQuickStartTitle_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void textQuickStartIcon_TextChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void textQuickStartLinks_TextChanged(object sender, EventArgs e)
        {

        }

        private void textQuickStartLinkUri_TextChanged(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void textQuickStartLinkText_TextChanged(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void TabSpec_Click(object sender, EventArgs e)
        {

        }

        private void labelCurrentSubTab_Click(object sender, EventArgs e)
        {

        }

        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void TabSpecs_Click(object sender, EventArgs e)
        {

        }

        private void textSpecType_TextChanged(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void groupBox17_Enter(object sender, EventArgs e)
        {

        }

        private void textSpecItems_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox13_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox15_Enter(object sender, EventArgs e)
        {

        }

        private void textSpecInnerFeatureId_TextChanged(object sender, EventArgs e)
        {

        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void textSpecInnerFeatureItems_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox14_Enter(object sender, EventArgs e)
        {

        }

        private void textSpecCostCaption_TextChanged(object sender, EventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void textSpecCostCurrencyCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void textSpecCostAccount_TextChanged(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void groupBox16_Enter(object sender, EventArgs e)
        {

        }

        private void textSpecPromotedFeatureItems_TextChanged(object sender, EventArgs e)
        {

        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void textSpecPromotedFeatureUnitDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void textSpecPromotedFeatureValue_TextChanged(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void textSpecItemSpecCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void textSpecItemTitle_TextChanged(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void textSpecItemColorScheme_TextChanged(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void textSpecItemID_TextChanged(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void TabSpecFeatures_Click(object sender, EventArgs e)
        {

        }

        private void groupBox24_Enter(object sender, EventArgs e)
        {

        }

        private void textSpecFeatureItems_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox18_Enter(object sender, EventArgs e)
        {

        }

        private void CBSpecFeaturesID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label32_Click(object sender, EventArgs e)
        {

        }

        private void textSpecFeatureIconName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label35_Click(object sender, EventArgs e)
        {

        }

        private void textSpecFeatureDisplayName_TextChanged(object sender, EventArgs e)
        {

        }

        private void CBSpecFeaturesIconData_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label34_Click(object sender, EventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void TabSpecResourceMap_Click(object sender, EventArgs e)
        {

        }

        private void groupBox20_Enter(object sender, EventArgs e)
        {

        }

        private void textSpecResourceMapDefaultItems_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox19_Enter(object sender, EventArgs e)
        {

        }

        private void CBSpecResourceMapDefaultItemId_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox23_Enter(object sender, EventArgs e)
        {

        }

        private void textSpecResourceMapFirstPartyItems_TextChanged(object sender, EventArgs e)
        {

        }

        private void textSpecResourceMapItemQuantity_TextChanged(object sender, EventArgs e)
        {

        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void textSpecResourceMapItemResourceId_TextChanged(object sender, EventArgs e)
        {

        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void label36_Click(object sender, EventArgs e)
        {

        }

        private void TabSpecAllowZero_Click(object sender, EventArgs e)
        {

        }

        private void groupBox26_Enter(object sender, EventArgs e)
        {

        }

        private void CBSpecAllowZerpCostID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textSpecAllowZeroCostIDs_TextChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void TabOverAll_Click(object sender, EventArgs e)
        {

        }

        private void groupBox12_Enter(object sender, EventArgs e)
        {

        }

        private void textOverallResourceJsonContent_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox10_Enter(object sender, EventArgs e)
        {

        }

        private void textOverallSvgIconsList_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox9_Enter(object sender, EventArgs e)
        {

        }

        private void textOverallSpecJsonContent_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }

        private void textOverallQuickStartJsonContent_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void textOverallApiJsonContent_TextChanged(object sender, EventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}