
namespace ApiOnBoardingConfigurationTool
{
    using Microsoft.Azure;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SkuConfigurationEntity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// The MainForm
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class MainForm : Form
    {
        /// <summary>
        /// The API on boarding configuration tool temporary folder
        /// </summary>
        private const string ApiOnBoardingConfigurationToolFolder = @"C:\ApiOnBoardingConfigurationToolTemp\";

        /// <summary>
        /// The API on boarding configuration tool save to local folder
        /// </summary>
        private const string ApiOnBoardingConfigurationToolSaveToLocalFolder = @"C:\ApiOnBoardingConfigurationToolTemp\SaveToLocal\";

        /// <summary>
        /// The API on boarding configuration tool inner temporary folder
        /// </summary>
        private const string ApiOnBoardingConfigurationToolInnerTempFolder = @"C:\ApiOnBoardingConfigurationToolTemp\Temp\";

        /// <summary>
        /// The loading file message
        /// </summary>
        private const string LoadingFileMessage = "Loading files...";

        /// <summary>
        /// The finish loading message
        /// </summary>
        private const string FinishLoadingMessage = "Finished!";

        /// <summary>
        /// The occur error message
        /// </summary>
        private const string OccurErrorMessage = "Occured some errors!";

        /// <summary>
        /// The loaded successfully message
        /// </summary>
        private const string LoadedSuccessfullyMessage = "Loaded successfully!";

        /// <summary>
        /// The resource token
        /// </summary>
        private const string ResourceToken = "ms-resource:";

        /// <summary>
        /// The icons token
        /// </summary>
        private const string IconToken = "ms-icon:";

        /// <summary>
        /// The API configuration storage container
        /// </summary>
        public string ApiConfigurationStorageContainerName = "apiconfiguration";

        /// <summary>
        /// The alert title
        /// </summary>
        private const string AlertTitle = "Alert";

        /// <summary>
        /// The exception title
        /// </summary>
        private const string ExceptionTitle = "Exception";

        /// <summary>
        /// The common alert text1
        /// </summary>
        private const string CommonAlertText1 = "Occured some errors!\r\nPlease check the last ErrorLog!";

        /// <summary>
        /// The common saved successfully text
        /// </summary>
        private const string CommonSavedSuccessfullyText = "'{0}' saved successfully!";

        /// <summary>
        /// The template should not be empty text
        /// </summary>
        private const string TemplateShouldNotBeEmptyText = "'{0}' should not be empty!\r\n";

        /// <summary>
        /// The template should not all be empty text
        /// </summary>
        private const string TemplateShouldNotAllBeEmptyText = "'{0}' and '{1}' should not all be empty!\r\n";

        /// <summary>
        /// The template cannot be deserialized text
        /// </summary>
        private const string TemplateCannotBeDeserializedText = "'{0}' can not be converted!";

        private const string TemplateShouldBeANumberText = "'{0}' should be number!\r\n";

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
        /// The API configuration icons
        /// </summary>
        private Dictionary<string, string> ApiConfigIcons = new Dictionary<string, string>();

        /// <summary>
        /// The manual configuration data
        /// config对象，用于存放配置信息
        /// </summary>
        private ApiConfigurationData ManualConfigurationData = new ApiConfigurationData();

        /// <summary>
        /// The list cached API configuration data
        /// </summary>
        private List<ApiConfigurationData> ListCachedApiConfigurationData;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            string containerName = CloudConfigurationManager.GetSetting("ApiConfigurationStorageContainerName");
            if (!string.IsNullOrWhiteSpace(containerName))
            {
                this.ApiConfigurationStorageContainerName = containerName;
            }
        }

        #region Extension Configuration

        #region Action And Review

        /// <summary>
        /// Handles the Click event of the ECBtnCreate control.
        /// Create a new Extension Configuration Folder or Zip file.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnCreate_Click(object sender, EventArgs e)
        {
            ManualConfigurationData = new ApiConfigurationData();
            this.ApiConfigIcons = new Dictionary<string, string>();
            ResetAllExtensionConfigurationControls();
        }

        /// <summary>
        /// Handles the Click event of the ECBtnLoadFromLocal control.
        /// Load Configuration Folder from Local.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnLoadFromLocal_Click(object sender, EventArgs e)
        {
            ApiConfigurationData cacheConfigurationData;

            if (this.ECFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                cacheConfigurationData = ApiConfigurationManager.LoadSingelApiConfigurationFormLoadFolderPath(this.ECFolderBrowserDialog.SelectedPath);

                if (cacheConfigurationData != null)
                {
                    LoadApiConfigInfoToPage(cacheConfigurationData);
                }

                if (cacheConfigurationData.listError.Count > 0)
                {
                    int i = 1;
                    string tempErrorMessage = DateTime.UtcNow.ToString() + "\r\n";
                    foreach (ErrorEntity errorEntity in cacheConfigurationData.listError)
                    {
                        tempErrorMessage += (i++) + "、" + errorEntity.GetErrorInfo() + "\r\n";
                    }

                    LogError(tempErrorMessage);
                    MessageBox.Show(CommonAlertText1, AlertTitle);
                    this.ECTextActionMessage.Text = OccurErrorMessage;
                }
                else
                {
                    this.ECTextActionMessage.Text = FinishLoadingMessage;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the ECBtnLoadFromPPEBlob control.
        /// Load Configuration Zip files from PPE Blob.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnLoadFromPPEBlob_Click(object sender, EventArgs e)
        {
            this.ECBtnCreate.Enabled = false;
            this.ECBtnLoadFromPPEBlob.Enabled = false;
            this.ECBtnLoadFromLocal.Enabled = false;
            this.ECTextActionMessage.Text = LoadingFileMessage;

            var apiConfigurationPublicAccessUrl = CloudConfigurationManager.GetSetting("ApiConfigurationPublicAccessUrl");

            ApiConfigurationManager manager = new ApiConfigurationManager(apiConfigurationPublicAccessUrl, this.ApiConfigurationStorageContainerName);
            ListCachedApiConfigurationData = manager.LoadDataToCache();

            List<ErrorEntity> listError = manager.listError;

            if (manager.listError.Count > 0)
            {
                this.ECTextActionMessage.Text = OccurErrorMessage;
                int i = 1;
                string tempErrorMessage = DateTime.UtcNow.ToString() + "\r\n";
                foreach (ErrorEntity errorMessage in manager.listError)
                {
                    tempErrorMessage += (i++) + "、" + errorMessage.GetErrorInfo() + "\r\n";
                }

                LogError(tempErrorMessage);
                MessageBox.Show(CommonAlertText1, AlertTitle);
                this.ECTextActionMessage.Text = OccurErrorMessage;
            }
            else
            {
                this.ECTextActionMessage.Text = LoadedSuccessfullyMessage;
            }

            LoadApiItemList();

            this.ECBtnCreate.Enabled = true;
            this.ECBtnLoadFromPPEBlob.Enabled = true;
            this.ECBtnLoadFromLocal.Enabled = true;
        }

        /// <summary>
        /// Handles the DoubleClick event of the ECListItems control.
        /// Api configuration packages list.(Double click to load on the pages.)
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECListItems_DoubleClick(object sender, EventArgs e)
        {
            if (this.ECListItems.Items.Count > 0)
            {
                string selectedApiItemName = this.ECListItems.SelectedItem.ToString();
                foreach (var apiConfigInfo in ListCachedApiConfigurationData)
                {
                    if (apiConfigInfo.ApiFolderName.Equals(selectedApiItemName))
                    {
                        LoadApiConfigInfoToPage(apiConfigInfo);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the ECBtnUploadToProBlob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnUploadToProBlob_Click(object sender, EventArgs e)
        {
            string storageAccount = this.ECTextStorageAccount.Text;
            string storageAccountKey = this.ECTextStorageAccountKey.Text;

            if (string.IsNullOrWhiteSpace(storageAccount))
            {
                this.ECTextActionMessage.Text = "Please input the Storage Account.";
                return;
            }

            if (string.IsNullOrWhiteSpace(storageAccountKey))
            {
                this.ECTextActionMessage.Text = "Please input the Storage Account Key.";
                return;
            }

            if (this.ECListItems.CheckedItems.Count == 0)
            {
                this.ECTextActionMessage.Text = "Please select the api items to upload to production blob.";
                return;
            }

            List<string> selectApiItems = new List<string>();

            for (int i = 0; i < this.ECListItems.CheckedItems.Count; i++)
            {
                selectApiItems.Add(this.ECListItems.CheckedItems[i].ToString());
            }

            try
            {
                List<ApiConfigurationData> selectedApiConfigurationDataList = new List<ApiConfigurationData>();
                foreach (var data in ListCachedApiConfigurationData)
                {
                    if (selectApiItems.Contains(data.ApiTypeName))
                    {
                        selectedApiConfigurationDataList.Add(data);
                    }
                }

                StorageCredentials credentials = new StorageCredentials(storageAccount, storageAccountKey, "AccountKey");
                BlobHelper.UploadApiConfigurationListToBlobContainer(credentials, ApiConfigurationStorageContainerName, selectedApiConfigurationDataList, ApiOnBoardingConfigurationToolInnerTempFolder);

                this.ECTextActionMessage.Text = "Upload files from Test blob to productive blob successfully .";
            }
            catch
            {
                this.ECTextActionMessage.Text = OccurErrorMessage;
            }
        }

        #endregion

        #region Flags Operation

        /// <summary>
        /// Handles the Click event of the ECBtnIconsFlag control.
        /// The Flag for Icons.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void ECBtnIconsFlag_Click(object sender, EventArgs e)
        {
            this.TabApiItemPage.SelectedTab = this.TabIcon;
        }

        /// <summary>
        /// Handles the Click event of the ECBtnApiFlag control.
        /// The flag for Api.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void ECBtnApiFlag_Click(object sender, EventArgs e)
        {
            this.TabApiItemPage.SelectedTab = this.TabApi;
        }

        /// <summary>
        /// Handles the Click event of the ECBtnQuickStartsFlag control.
        /// The falg for QuickStarts
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void ECBtnQuickStartsFlag_Click(object sender, EventArgs e)
        {
            this.TabApiItemPage.SelectedTab = this.TabQuickStarts;
        }

        /// <summary>
        /// Handles the Click event of the ECBtnSpecFlag control.
        /// The flag for Spec.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void ECBtnSpecFlag_Click(object sender, EventArgs e)
        {
            this.TabApiItemPage.SelectedTab = this.TabSpec;
        }

        #endregion

        #region Reset Pages

        /// <summary>
        /// Resets all flag control.
        /// </summary>
        private void ResetAllFlagControl()
        {
            ///Flags
            this.ECBtnIconsFlag.BackColor = System.Drawing.Color.Orange;
            this.ECBtnApiFlag.BackColor = System.Drawing.Color.Orange;
            this.ECBtnQuickStartsFlag.BackColor = System.Drawing.Color.Orange;
            this.ECBtnSpecFlag.BackColor = System.Drawing.Color.Orange;
        }

        /// <summary>
        /// Resets the icons page.
        /// </summary>
        private void ResetIconsPage()
        {
            ///Svg Files
            this.ECTextIconFolderPath.Text = string.Empty;
            this.ECTextSvgFiles.Text = string.Empty;
            this.ECTextSvgFiles.BackColor = System.Drawing.Color.White;
        }

        /// <summary>
        /// Resets the API page.
        /// </summary>
        private void ResetApiPage()
        {
            ///Api.Json
            this.ECTextApiItem.Text = string.Empty;
            this.ECTextApiItem.BackColor = System.Drawing.Color.White;
            this.ECTextApiTitle.Text = string.Empty;
            this.ECTextApiTitle.BackColor = System.Drawing.Color.White;
            this.ECTextApiSubTitle.Text = string.Empty;
            this.ECTextApiSubTitle.BackColor = System.Drawing.Color.White;
            this.ECCBApiIconData.Items.Clear();
            this.ECCBApiIconData.BackColor = System.Drawing.Color.White;
            this.ECTextApiCategory.Text = string.Empty;
            //this.ECTextApiCategory.BackColor = System.Drawing.Color.White;
            this.ECTextApiCategory.Text = "CognitiveServices";
            this.ECTextApiCategory.BackColor = System.Drawing.Color.White;
            this.ECTextApiSkuQuotaCode.Text = string.Empty;
            this.ECTextApiSkuQuotaName.Text = string.Empty;
            this.ECTextApiSkuQuotaQuota.Text = string.Empty;
            this.ECTextApiSkuQuotaData.Text = string.Empty;
            this.ECRDBApiShowLegalTermTrue.Checked = true;
            this.ECRDBApiShowLegalTermFalse.Checked = false;
            this.ECTextApiDefaultLegalTerm.Text = string.Empty;
        }

        /// <summary>
        /// Resets the quick starts page.
        /// </summary>
        private void ResetQuickStartsPage()
        {
            ///QuickStarts.Json
            this.ECTextQuickStartItemTitle.Text = string.Empty;
            this.ECCBQuickStartItemIcon.SelectedItem = string.Empty;
            this.ECTextQuickStartItemDescription.Text = string.Empty;
            this.ECTextQuickStartItemLinkText.Text = string.Empty;
            this.ECTextQuickStartItemLinkUri.Text = string.Empty;
            this.ECTextQuickStartItemLinks.Text = string.Empty;
            this.ECTextQuickStartItems.Text = string.Empty;
            this.ECTextQuickStartItems.BackColor = System.Drawing.Color.White;
        }

        /// <summary>
        /// Resets the spec pages.
        /// </summary>
        private void ResetSpecPages()
        {
            ResetSpecFeaturesPage();
            ResetSpecItemsPage();
            ResetSpecCostItemPage();
            ResetSpecAllowZeroCostPage();
        }

        /// <summary>
        /// Resets the spec features page.
        /// </summary>
        private void ResetSpecFeaturesPage()
        {
            ///Spec.Json Features
            this.ECTextSpecFeatureItemDisplayName.Text = string.Empty;
            this.ECCBSpecFeatureItemIconSvgData.Items.Clear();
            this.ECTextSpecFeatureItemIconName.Text = string.Empty;
            this.ECTextSpecFeatureItems.Text = string.Empty;
            this.ECTextSpecFeatureItems.BackColor = System.Drawing.Color.White;
        }

        /// <summary>
        /// Resets the spec items page.
        /// </summary>
        private void ResetSpecItemsPage()
        {
            ///Spec.Json SpecItems
            this.ECTextSpecItemSpecCode.Text = string.Empty;
            this.ECCBSpecItemColorScheme.SelectedItem = string.Empty;
            this.ECTextSpecItemTitle.Text = string.Empty;
            this.ECTextSpecItemPromotedFeatureItemValue.Text = string.Empty;
            this.ECTextSpecItemPromotedFeatureItemUnitDescription.Text = string.Empty;
            this.ECTextSpecItemPromotedFeatureItems.Text = string.Empty;
            this.ECCBSpecItemFeatureID.Items.Clear();
            this.ECTextSpecItemFeatureIDs.Text = string.Empty;
            this.ECTextSpecItemCostAmount.Text = string.Empty;
            this.ECTextSpecItemCostCurrencyCode.Text = string.Empty;
            this.ECTextSpecItemCostCaption.Text = string.Empty;
            this.ECTextSpecItems.Text = string.Empty;
            this.ECTextSpecItems.BackColor = System.Drawing.Color.White;
        }

        /// <summary>
        /// Resets the spec cost item page.
        /// </summary>
        private void ResetSpecCostItemPage()
        {
            ///Spec.Json Cost Item
            this.ECCBSpecDefaultItemSpecCode.Items.Clear();
            this.ECTextSpecDefaultItemFirstPartyResourceID.Text = string.Empty;
            this.ECTextSpecDefaultItemFirstPartyQuantity.Text = string.Empty;
            this.ECTextSpecDefaultItemFirstPartyItems.Text = string.Empty;
            this.ECTextSpecDefaultItems.Text = string.Empty;
            this.ECTextSpecDefaultItems.BackColor = System.Drawing.Color.White;
        }

        /// <summary>
        /// Resets the spec allow zero cost page.
        /// </summary>
        private void ResetSpecAllowZeroCostPage()
        {
            ///Spec.Json AllowZeroCost
            this.ECCBSpecAllowZeroCostSpecCode.Items.Clear();
            this.ECTextSpecAllowZeroCostSpecCodeItems.Text = string.Empty;
            this.ECTextSpecAllowZeroCostSpecCodeItems.BackColor = System.Drawing.Color.White;
        }

        /// <summary>
        /// Resets the over all page.
        /// </summary>
        public void ResetOverAllPage()
        {
            ///OverAll
            this.ECTextOverAllApiJson.Text = string.Empty;
            this.ECTextOverAllQuickStartsJson.Text = string.Empty;
            this.ECTextOverAllSpecJson.Text = string.Empty;
        }

        /// <summary>
        /// Resets all extension configuration controls.
        /// </summary>
        private void ResetAllExtensionConfigurationControls()
        {
            /////ApiItemList
            //this.ECListItems.Items.Clear();

            this.TabApiItemPage.SelectedTab = this.TabIcon;

            ResetAllFlagControl();
            ResetIconsPage();
            ResetApiPage();
            ResetQuickStartsPage();
            ResetSpecPages();

            this.ECTextActionMessage.Text = "Reseted All!";
        }

        #endregion

        /// <summary>
        /// Handles the MouseCaptureChanged event of the TabApiItemPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void TabApiItemPage_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (this.TabApiItemPage.SelectedTab == this.TabApi)
            {
                string tempSelectedValue = this.ECCBApiIconData.Text;

                this.ECCBApiIconData.Items.Clear();

                if (this.ApiConfigIcons.Count > 0)
                {
                    this.ECCBApiIconData.Items.Add(string.Empty);
                    foreach (var icon in this.ApiConfigIcons)
                    {
                        this.ECCBApiIconData.Items.Add(icon.Key);
                    }

                    this.ECCBApiIconData.SelectedItem = tempSelectedValue;
                }
            }
            else if (this.TabApiItemPage.SelectedTab == this.TabSpec)
            {
                string tempSelectedValue = this.ECCBSpecFeatureItemIconSvgData.Text;

                this.ECCBSpecFeatureItemIconSvgData.Items.Clear();

                if (this.ApiConfigIcons.Count > 0)
                {
                    this.ECCBSpecFeatureItemIconSvgData.Items.Add(string.Empty);
                    foreach (var icon in this.ApiConfigIcons)
                    {
                        this.ECCBSpecFeatureItemIconSvgData.Items.Add(icon.Key);
                    }

                    this.ECCBSpecFeatureItemIconSvgData.SelectedItem = tempSelectedValue;
                }
            }
            else if (this.TabApiItemPage.SelectedTab == this.TabOverAll)
            {
                try
                {
                    this.ECTextOverAllApiJson.Text = string.Empty;
                    this.ECTextOverAllQuickStartsJson.Text = string.Empty;
                    this.ECTextOverAllSpecJson.Text = string.Empty;

                    ManualConfigurationData.Icons = this.ApiConfigIcons;
                    if (ManualConfigurationData.ApiItem != null)
                    {
                        this.ECTextOverAllApiJson.Text = JsonConvert.SerializeObject(ManualConfigurationData.ApiItem, specialSettingFormat);
                    }

                    if (ManualConfigurationData.QuickStart != null)
                    {
                        this.ECTextOverAllQuickStartsJson.Text = JsonConvert.SerializeObject(ManualConfigurationData.QuickStart, settingFormat);
                    }

                    if (ManualConfigurationData.Spec != null)
                    {
                        this.ECTextOverAllSpecJson.Text = JsonConvert.SerializeObject(ManualConfigurationData.Spec, settingFormat);
                    }
                }
                catch (Exception ex)
                {
                    this.ECTextActionMessage.Text = OccurErrorMessage;
                    MessageBox.Show(ex.Message, ExceptionTitle);
                }
            }
        }

        #region Icon Operation

        /// <summary>
        /// Handles the Click event of the ECBtnSelectSvgFolder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnSelectSvgFolder_Click(object sender, EventArgs e)
        {
            if (this.ECFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.ECTextIconFolderPath.Text = this.ECFolderBrowserDialog.SelectedPath;
                DirectoryInfo iconDirectory = new DirectoryInfo(this.ECFolderBrowserDialog.SelectedPath);

                FileInfo[] files = iconDirectory.GetFiles();
                this.ApiConfigIcons.Clear();

                foreach (FileInfo file in files)
                {
                    if (file.Extension.ToLower().Equals(".svg"))
                    {
                        this.ECTextSvgFiles.Text += file.Name + "\r\n";

                        using (StreamReader reader = file.OpenText())
                        {
                            this.ApiConfigIcons.Add(file.Name, reader.ReadToEnd());
                        }
                    }
                }

                if (this.ApiConfigIcons.Count > 0)
                {
                    this.ECBtnIconsFlag.BackColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.ECBtnIconsFlag.BackColor = System.Drawing.Color.Red;
                }
            }
        }

        #endregion

        #region Api Operation

        /// <summary>
        /// Handles the Click event of the BtnApiSkuQuotaAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnApiSkuQuotaAdd_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.ECTextApiSkuQuotaCode.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "SkuQuota - Code");
            }

            if (string.IsNullOrWhiteSpace(this.ECTextApiSkuQuotaName.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "SkuQuota - Name");
            }

            if (string.IsNullOrWhiteSpace(this.ECTextApiSkuQuotaQuota.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "SkuQuota - Quoda");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            int tempQuota;
            try
            {
                tempQuota = int.Parse(this.ECTextApiSkuQuotaQuota.Text);
            }
            catch
            {
                string exceptionMessage = string.Format(TemplateShouldBeANumberText, "SkuQuota - Quota");
                MessageBox.Show(exceptionMessage, ExceptionTitle);
                return;
            }

            List<ApiSkuQuotaEntity> apiSkuQuotaEntityList = new List<ApiSkuQuotaEntity>();
            ApiSkuQuotaEntity apiSkuQuotaEntity = new ApiSkuQuotaEntity { code = this.ECTextApiSkuQuotaCode.Text, name = this.ECTextApiSkuQuotaName.Text, quota = tempQuota };

            if (!string.IsNullOrWhiteSpace(this.ECTextApiSkuQuotaData.Text))
            {
                try
                {
                    apiSkuQuotaEntityList = JsonConvert.DeserializeObject<List<ApiSkuQuotaEntity>>(this.ECTextApiSkuQuotaData.Text, settingFormat);

                }
                catch
                {
                    string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "SkuQuota");
                    MessageBox.Show(exceptionMessage, ExceptionTitle);
                    return;
                }

                apiSkuQuotaEntityList.Add(apiSkuQuotaEntity);
            }
            else
            {
                apiSkuQuotaEntityList.Add(apiSkuQuotaEntity);
            }

            this.ECTextApiSkuQuotaData.Text = JsonConvert.SerializeObject(apiSkuQuotaEntityList, settingFormat);
            this.ECTextApiSkuQuotaCode.Text = string.Empty;
            this.ECTextApiSkuQuotaName.Text = string.Empty;
            this.ECTextApiSkuQuotaQuota.Text = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the ECBtnSaveApiInfo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnSaveApiInfo_Click(object sender, EventArgs e)
        {
            try
            {
                string ErrorMessage = string.Empty;
                ApiEntity entity = new ApiEntity();
                entity.categories = new List<string>();
                entity.skuQuota = new List<ApiSkuQuotaEntity>();
                List<ApiSkuQuotaEntity> apiSkuQuota = new List<ApiSkuQuotaEntity>();

                if (string.IsNullOrWhiteSpace(this.ECTextApiItem.Text))
                {
                    ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "ApiTypeName");
                }

                if (string.IsNullOrWhiteSpace(this.ECTextApiTitle.Text))
                {
                    ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Title");
                }

                if (string.IsNullOrWhiteSpace(this.ECTextApiSubTitle.Text))
                {
                    ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Description");
                }

                if (string.IsNullOrWhiteSpace(this.ECCBApiIconData.Text))
                {
                    ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Api Icon");
                }

                if (string.IsNullOrWhiteSpace(this.ECTextApiCategory.Text))
                {
                    ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Category");
                }

                if (!string.IsNullOrWhiteSpace(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, AlertTitle);
                    return;
                }
                else
                {
                    entity.item = this.ECTextApiItem.Text;
                    entity.title = this.ECTextApiItem.Text;
                    entity.subtitle = this.ECTextApiSubTitle.Text;
                    entity.iconData = this.ECCBApiIconData.Text;

                    entity.categories = new List<string>();
                    entity.categories.Add(this.ECTextApiCategory.Text);

                    if (!string.IsNullOrWhiteSpace(this.ECTextApiSkuQuotaData.Text))
                    {
                        try
                        {
                            string apiSkuQuotaData = this.ECTextApiSkuQuotaData.Text;
                            entity.skuQuota = JsonConvert.DeserializeObject<List<ApiSkuQuotaEntity>>(apiSkuQuotaData, settingFormat);
                        }
                        catch (Exception ex)
                        {
                            ErrorMessage = string.Format(TemplateCannotBeDeserializedText, "SkuQuota", ex.Message);
                            MessageBox.Show(ErrorMessage, ExceptionTitle);
                            return;
                        }
                    }

                    entity.showLegalTerm = this.ECRDBApiShowLegalTermTrue.Checked;
                    if (!string.IsNullOrWhiteSpace(this.ECTextApiDefaultLegalTerm.Text))
                    {
                        entity.defaultLegalTerm = this.ECTextApiDefaultLegalTerm.Text;
                    }

                    ManualConfigurationData.ApiTypeName = entity.item;
                    ManualConfigurationData.ApiItem = entity;

                    this.ECBtnApiFlag.BackColor = System.Drawing.Color.Green;
                    this.ECTextActionMessage.Text = string.Format(CommonSavedSuccessfullyText, "Api");

                    ResetApiPage();
                }
            }
            catch (Exception ex)
            {
                this.ECTextActionMessage.Text = "Occured some errors!";
                MessageBox.Show(ex.Message, ExceptionTitle);
            }
        }

        #endregion

        #region QuickStarts Operation

        /// <summary>
        /// Handles the Click event of the ECBtnAddLinkItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnAddLinkItem_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.ECTextQuickStartItemLinkText.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Links - Text");
            }

            if (string.IsNullOrWhiteSpace(this.ECTextQuickStartItemLinkUri.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Links - Uri");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            QuickStartLink tempLink = new QuickStartLink { text = this.ECTextQuickStartItemLinkText.Text, uri = this.ECTextQuickStartItemLinkUri.Text };
            List<QuickStartLink> tempLinks = new List<QuickStartLink>();
            if (!string.IsNullOrWhiteSpace(this.ECTextQuickStartItemLinks.Text))
            {
                try
                {
                    tempLinks = JsonConvert.DeserializeObject<List<QuickStartLink>>(this.ECTextQuickStartItemLinks.Text, settingFormat);
                }
                catch
                {
                    string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "Links");
                    MessageBox.Show(exceptionMessage, ExceptionTitle);
                    return;
                }

                tempLinks.Add(tempLink);
            }
            else
            {
                tempLinks.Add(tempLink);
            }

            this.ECTextQuickStartItemLinks.Text = JsonConvert.SerializeObject(tempLinks, settingFormat);

            this.ECTextQuickStartItemLinkText.Text = string.Empty;
            this.ECTextQuickStartItemLinkUri.Text = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the ECBtnAddQuickStartItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnAddQuickStartItem_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.ECTextQuickStartItemTitle.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Title");
            }

            if (string.IsNullOrWhiteSpace(this.ECCBQuickStartItemIcon.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Icon");
            }

            if (string.IsNullOrWhiteSpace(this.ECTextQuickStartItemDescription.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Description");
            }

            if (string.IsNullOrWhiteSpace(this.ECTextQuickStartItemLinks.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Links");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            QuickStartUnit qucikStartUnit = new QuickStartUnit();
            try
            {
                qucikStartUnit.links = JsonConvert.DeserializeObject<List<QuickStartLink>>(this.ECTextQuickStartItemLinks.Text, settingFormat);
            }
            catch
            {
                string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "Links");
                MessageBox.Show(exceptionMessage, ExceptionTitle);
                return;
            }

            qucikStartUnit.title = this.ECTextQuickStartItemTitle.Text;
            qucikStartUnit.icon = this.ECCBQuickStartItemIcon.Text;
            qucikStartUnit.description = this.ECTextQuickStartItemDescription.Text;

            List<QuickStartUnit> quickStarts = new List<QuickStartUnit>();
            if (!string.IsNullOrWhiteSpace(this.ECTextQuickStartItems.Text))
            {
                try
                {
                    quickStarts = JsonConvert.DeserializeObject<List<QuickStartUnit>>(this.ECTextQuickStartItems.Text, settingFormat);
                }
                catch
                {
                    string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "QuickStartItems");
                    MessageBox.Show(exceptionMessage, ExceptionTitle);
                    return;
                }
            }

            quickStarts.Add(qucikStartUnit);
            this.ECTextQuickStartItems.Text = JsonConvert.SerializeObject(quickStarts, settingFormat);

            this.ECTextQuickStartItemTitle.Text = string.Empty;
            this.ECCBQuickStartItemIcon.Text = string.Empty;
            this.ECTextQuickStartItemDescription.Text = string.Empty;
            this.ECTextQuickStartItemLinks.Text = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the ECBtnSaveQuickStartsInfo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnSaveQuickStartsInfo_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.ECTextQuickStartItems.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "QuickStartItems");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            List<QuickStartUnit> quickStarts = new List<QuickStartUnit>();
            try
            {
                quickStarts = JsonConvert.DeserializeObject<List<QuickStartUnit>>(this.ECTextQuickStartItems.Text, settingFormat);
            }
            catch
            {
                string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "QuickStartItems");
                MessageBox.Show(exceptionMessage, ExceptionTitle);
                return;
            }

            ManualConfigurationData.QuickStart = new QuickStartsEntity();
            ManualConfigurationData.QuickStart.quickStarts = quickStarts;

            this.ECTextActionMessage.Text = string.Format(CommonSavedSuccessfullyText, "QuickStarts");

            this.ECBtnQuickStartsFlag.BackColor = System.Drawing.Color.Green;
            ResetQuickStartsPage();
        }

        #endregion

        #region Spec Operation

        /// <summary>
        /// Handles the MouseCaptureChanged event of the TabSpecContent control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void TabSpecContent_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (this.TabSpecContent.SelectedTab == this.TabSpecFeatures)
            {
                this.ECCBSpecFeatureItemIconSvgData.Items.Clear();
                this.ECCBSpecFeatureItemIconSvgData.Items.Add(string.Empty);
                foreach (var icon in this.ApiConfigIcons)
                {
                    this.ECCBSpecFeatureItemIconSvgData.Items.Add(icon.Key);
                }
            }
            else if (this.TabSpecContent.SelectedTab == this.TabSpecItems)
            {
                this.ECCBSpecItemFeatureID.Items.Clear();
                if (!string.IsNullOrWhiteSpace(this.ECTextSpecFeatureItems.Text))
                {
                    List<SpecFeatureEntity> features;
                    try
                    {
                        features = JsonConvert.DeserializeObject<List<SpecFeatureEntity>>(this.ECTextSpecFeatureItems.Text, settingFormat);
                    }
                    catch
                    {
                        string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "FeatureItems");
                        MessageBox.Show(exceptionMessage, ExceptionTitle);
                        return;
                    }

                    this.ECCBSpecItemFeatureID.Items.Add(string.Empty);
                    foreach (var feature in features)
                    {
                        this.ECCBSpecItemFeatureID.Items.Add(feature.id);
                    }
                }
            }
            else if (this.TabSpecContent.SelectedTab == this.TabSpecCostItems)
            {
                this.ECCBSpecDefaultItemSpecCode.Items.Clear();
                if (!string.IsNullOrWhiteSpace(this.ECTextSpecItems.Text))
                {
                    List<SpecUnitEntity> specs;
                    try
                    {
                        specs = JsonConvert.DeserializeObject<List<SpecUnitEntity>>(this.ECTextSpecItems.Text, settingFormat);
                    }
                    catch
                    {
                        string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "SpecItems");
                        MessageBox.Show(exceptionMessage, ExceptionTitle);
                        return;
                    }

                    this.ECCBSpecDefaultItemSpecCode.Items.Add(string.Empty);
                    foreach (var spec in specs)
                    {
                        this.ECCBSpecDefaultItemSpecCode.Items.Add(spec.specCode);
                    }
                }
            }
            else if (this.TabSpecContent.SelectedTab == this.TabSpecAllowZeroCost)
            {
                this.ECCBSpecAllowZeroCostSpecCode.Items.Clear();
                if (!string.IsNullOrWhiteSpace(this.ECTextSpecItems.Text))
                {
                    List<SpecUnitEntity> specs;
                    try
                    {
                        specs = JsonConvert.DeserializeObject<List<SpecUnitEntity>>(this.ECTextSpecItems.Text, settingFormat);
                    }
                    catch
                    {
                        string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "SpecItems");
                        MessageBox.Show(exceptionMessage, ExceptionTitle);
                        return;
                    }

                    this.ECCBSpecAllowZeroCostSpecCode.Items.Add(string.Empty);
                    foreach (var spec in specs)
                    {
                        this.ECCBSpecAllowZeroCostSpecCode.Items.Add(spec.specCode);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the ECBtnAddSpecFeatureItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnAddSpecFeatureItem_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.ECTextSpecFeatureItemDisplayName.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Display Name");
            }

            if (string.IsNullOrWhiteSpace(this.ECCBSpecFeatureItemIconSvgData.Text) && string.IsNullOrWhiteSpace(this.ECTextSpecFeatureItemIconName.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotAllBeEmptyText, "Feature Icon", "Icon Name");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            SpecFeatureEntity specFeatureEntity = new SpecFeatureEntity();
            specFeatureEntity.displayName = this.ECTextSpecFeatureItemDisplayName.Text;
            specFeatureEntity.id = specFeatureEntity.displayName.Trim(' ');
            if (!string.IsNullOrWhiteSpace(this.ECCBSpecFeatureItemIconSvgData.Text))
            {
                specFeatureEntity.iconSvgData = this.ECCBSpecFeatureItemIconSvgData.Text;
            }
            else
            {
                specFeatureEntity.displayName = this.ECTextSpecFeatureItemIconName.Text;
            }

            List<SpecFeatureEntity> features = new List<SpecFeatureEntity>();
            if (!string.IsNullOrWhiteSpace(this.ECTextSpecFeatureItems.Text))
            {
                try
                {
                    features = JsonConvert.DeserializeObject<List<SpecFeatureEntity>>(this.ECTextSpecFeatureItems.Text, settingFormat);
                }
                catch
                {
                    string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "Links");
                    MessageBox.Show(exceptionMessage, ExceptionTitle);
                    return;
                }
            }

            features.Add(specFeatureEntity);

            this.ECTextSpecFeatureItems.Text = JsonConvert.SerializeObject(features, specialSettingFormat);
            this.ECTextSpecFeatureItemDisplayName.Text = string.Empty;
            this.ECCBSpecFeatureItemIconSvgData.Text = string.Empty;
            this.ECTextSpecFeatureItemIconName.Text = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the ECBtnAddPromotedFeature control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnAddPromotedFeature_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.ECTextSpecItemPromotedFeatureItemValue.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "PromotedFeatures - Value");
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecItemPromotedFeatureItemUnitDescription.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "PromotedFeatures - unitDescription");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            SpecPromotedFeature specPromotedFeature = new SpecPromotedFeature();
            specPromotedFeature.value = this.ECTextSpecItemPromotedFeatureItemValue.Text;
            specPromotedFeature.unitDescription = this.ECTextSpecItemPromotedFeatureItemUnitDescription.Text;

            List<SpecPromotedFeature> promotedFeatures = new List<SpecPromotedFeature>();
            if (!string.IsNullOrWhiteSpace(this.ECTextSpecItemPromotedFeatureItems.Text))
            {
                try
                {
                    promotedFeatures = JsonConvert.DeserializeObject<List<SpecPromotedFeature>>(this.ECTextSpecItemPromotedFeatureItems.Text, settingFormat);
                }
                catch
                {
                    string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "PromotedFeatures");
                    MessageBox.Show(exceptionMessage, ExceptionTitle);
                    return;
                }
            }

            promotedFeatures.Add(specPromotedFeature);
            this.ECTextSpecItemPromotedFeatureItems.Text = JsonConvert.SerializeObject(promotedFeatures, settingFormat);
            this.ECTextSpecItemPromotedFeatureItemValue.Text = string.Empty;
            this.ECTextSpecItemPromotedFeatureItemUnitDescription.Text = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the ECBtnAddFeatureID control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnAddFeatureID_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.ECCBSpecItemFeatureID.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Features - FeatureID");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            SpecFeatureUnit specFeatureUnit = new SpecFeatureUnit();
            specFeatureUnit.id = this.ECCBSpecItemFeatureID.Text;

            List<SpecFeatureUnit> features = new List<SpecFeatureUnit>();
            if (!string.IsNullOrWhiteSpace(this.ECTextSpecItemFeatureIDs.Text))
            {
                try
                {
                    features = JsonConvert.DeserializeObject<List<SpecFeatureUnit>>(this.ECTextSpecItemFeatureIDs.Text, settingFormat);
                }
                catch
                {
                    string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "Features");
                    MessageBox.Show(exceptionMessage, ExceptionTitle);
                    return;
                }
            }

            features.Add(specFeatureUnit);
            this.ECTextSpecItemFeatureIDs.Text = JsonConvert.SerializeObject(features, settingFormat);
            this.ECCBSpecItemFeatureID.SelectedItem = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the ECBtnAddSpecItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnAddSpecItem_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.ECTextSpecItemSpecCode.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "SpecCode");
            }

            if (string.IsNullOrWhiteSpace(this.ECCBSpecItemColorScheme.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "ColorScheme");
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecItemTitle.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Title");
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecItemPromotedFeatureItems.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "PromotedFeatures");
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecItemFeatureIDs.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Features");
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecItemCostCaption.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Cost - Caption");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            SpecUnitEntity specUnitEntity = new SpecUnitEntity();
            specUnitEntity.specCode = this.ECTextSpecItemSpecCode.Text;
            specUnitEntity.id = specUnitEntity.specCode;
            specUnitEntity.colorScheme = this.ECCBSpecItemColorScheme.Text;
            specUnitEntity.title = this.ECTextSpecItemTitle.Text;

            SpecCost specCost = new SpecCost();
            specCost.caption = this.ECTextSpecItemCostCaption.Text;

            if (!string.IsNullOrWhiteSpace(this.ECTextSpecItemCostAmount.Text))
            {
                specCost.amount = float.Parse(this.ECTextSpecItemCostAmount.Text);
            }

            if (!string.IsNullOrWhiteSpace(this.ECTextSpecItemCostCurrencyCode.Text))
            {
                specCost.currencyCode = this.ECTextSpecItemCostCurrencyCode.Text;
            }

            specUnitEntity.cost = specCost;

            specUnitEntity.promotedFeatures = new List<SpecPromotedFeature>();
            try
            {
                specUnitEntity.promotedFeatures = JsonConvert.DeserializeObject<List<SpecPromotedFeature>>(this.ECTextSpecItemPromotedFeatureItems.Text);
            }
            catch
            {
                string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "Features");
                MessageBox.Show(exceptionMessage, ExceptionTitle);
                return;
            }

            specUnitEntity.features = new List<SpecFeatureUnit>();
            try
            {
                specUnitEntity.features = JsonConvert.DeserializeObject<List<SpecFeatureUnit>>(this.ECTextSpecItemFeatureIDs.Text);
            }
            catch
            {
                string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "Features");
                MessageBox.Show(exceptionMessage, ExceptionTitle);
                return;
            }

            List<SpecUnitEntity> specs = new List<SpecUnitEntity>();
            if (!string.IsNullOrWhiteSpace(this.ECTextSpecItems.Text))
            {
                try
                {
                    specs = JsonConvert.DeserializeObject<List<SpecUnitEntity>>(this.ECTextSpecItems.Text, settingFormat);
                }
                catch
                {
                    string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "SpecItems");
                    MessageBox.Show(exceptionMessage, ExceptionTitle);
                    return;
                }
            }

            specs.Add(specUnitEntity);
            this.ECTextSpecItems.Text = JsonConvert.SerializeObject(specs, settingFormat);
            this.ECTextSpecItemSpecCode.Text = string.Empty;
            this.ECCBSpecItemColorScheme.SelectedItem = string.Empty;
            this.ECTextSpecItemTitle.Text = string.Empty;
            this.ECTextSpecItemPromotedFeatureItems.Text = string.Empty;
            this.ECTextSpecItemFeatureIDs.Text = string.Empty;
            this.ECTextSpecItemCostAmount.Text = string.Empty;
            this.ECTextSpecItemCostCurrencyCode.Text = string.Empty;
            this.ECTextSpecItemCostCaption.Text = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the ECBtnAddSpecDefaultItemFirstPartyItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnAddSpecDefaultItemFirstPartyItem_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.ECTextSpecDefaultItemFirstPartyResourceID.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "FistParty - ResourceID");
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecDefaultItemFirstPartyQuantity.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "FistParty - Quantity");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            SpecFirstParty specFirstParty = new SpecFirstParty();
            specFirstParty.resourceId = this.ECTextSpecDefaultItemFirstPartyResourceID.Text;
            try
            {
                specFirstParty.quantity = float.Parse(this.ECTextSpecDefaultItemFirstPartyQuantity.Text);
            }
            catch
            {
                string exceptionMessage = string.Format(TemplateShouldBeANumberText, "FirstParty - Quantity");
                MessageBox.Show(exceptionMessage, ExceptionTitle);
                return;
            }

            List<SpecFirstParty> firstParty = new List<SpecFirstParty>();
            if (!string.IsNullOrWhiteSpace(this.ECTextSpecDefaultItemFirstPartyItems.Text))
            {
                try
                {
                    firstParty = JsonConvert.DeserializeObject<List<SpecFirstParty>>(this.ECTextSpecDefaultItemFirstPartyItems.Text, settingFormat);
                }
                catch
                {
                    string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "FirstParty");
                    MessageBox.Show(exceptionMessage, ExceptionTitle);
                    return;
                }
            }

            firstParty.Add(specFirstParty);
            this.ECTextSpecDefaultItemFirstPartyItems.Text = JsonConvert.SerializeObject(firstParty, settingFormat);
            this.ECTextSpecDefaultItemFirstPartyResourceID.Text = string.Empty;
            this.ECTextSpecDefaultItemFirstPartyQuantity.Text = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the ECBtnAddSpecDefaultItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnAddSpecDefaultItem_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.ECCBSpecDefaultItemSpecCode.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "SpecCode");
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecDefaultItemFirstPartyItems.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "FistParty");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            SpecResourceMapDefault specResourceMapDefault = new SpecResourceMapDefault();
            specResourceMapDefault.id = this.ECCBSpecDefaultItemSpecCode.Text;

            try
            {
                specResourceMapDefault.firstParty = JsonConvert.DeserializeObject<List<SpecFirstParty>>(this.ECTextSpecDefaultItemFirstPartyItems.Text, settingFormat);
            }
            catch
            {
                string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "FirstParty");
                MessageBox.Show(exceptionMessage, ExceptionTitle);
                return;
            }

            List<SpecResourceMapDefault> specResourceMapDefaultItems = new List<SpecResourceMapDefault>();
            if (!string.IsNullOrWhiteSpace(this.ECTextSpecDefaultItems.Text))
            {
                try
                {
                    specResourceMapDefaultItems = JsonConvert.DeserializeObject<List<SpecResourceMapDefault>>(this.ECTextSpecDefaultItems.Text, settingFormat);
                }
                catch
                {
                    string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "Default Items");
                    MessageBox.Show(exceptionMessage, ExceptionTitle);
                    return;
                }
            }

            specResourceMapDefaultItems.Add(specResourceMapDefault);
            this.ECTextSpecDefaultItems.Text = JsonConvert.SerializeObject(specResourceMapDefaultItems, settingFormat);
            this.ECCBSpecDefaultItemSpecCode.SelectedItem = string.Empty;
            this.ECTextSpecDefaultItemFirstPartyItems.Text = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the ECBtnAddSpecAllowZeroCostSpecCodeItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnAddSpecAllowZeroCostSpecCodeItem_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.ECCBSpecAllowZeroCostSpecCode.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "SpecCode");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            List<string> specsToAllowZeroCost = new List<string>();
            if (!string.IsNullOrWhiteSpace(this.ECTextSpecAllowZeroCostSpecCodeItems.Text))
            {
                try
                {
                    specsToAllowZeroCost = JsonConvert.DeserializeObject<List<string>>(this.ECTextSpecAllowZeroCostSpecCodeItems.Text, settingFormat);
                }
                catch
                {
                    string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "SpecsToAllowZeroCost");
                    MessageBox.Show(exceptionMessage, ExceptionTitle);
                    return;
                }
            }

            specsToAllowZeroCost.Add(this.ECCBSpecAllowZeroCostSpecCode.Text);
            this.ECTextSpecAllowZeroCostSpecCodeItems.Text = JsonConvert.SerializeObject(specsToAllowZeroCost, settingFormat);
            this.ECCBSpecAllowZeroCostSpecCode.SelectedItem = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the ECBtnSaveSpecInfo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnSaveSpecInfo_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.ECTextSpecFeatureItems.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "FeatureItems");
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecItems.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "SpecItems");
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecDefaultItems.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "DefaultItems");
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecAllowZeroCostSpecCodeItems.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "SpecsToAllowZeroCost");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            SpecsEntity specsEntity = new SpecsEntity();

            specsEntity.specType = string.Format("Microsoft.ProjectOxford/{0}", ManualConfigurationData.ApiTypeName);

            try
            {
                specsEntity.features = JsonConvert.DeserializeObject<List<SpecFeatureEntity>>(this.ECTextSpecFeatureItems.Text, settingFormat);
            }
            catch
            {
                string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "FeatureItems");
                MessageBox.Show(exceptionMessage, ExceptionTitle);
                return;
            }

            try
            {
                specsEntity.specs = JsonConvert.DeserializeObject<List<SpecUnitEntity>>(this.ECTextSpecItems.Text, settingFormat);
            }
            catch
            {
                string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "SpecItems");
                MessageBox.Show(exceptionMessage, ExceptionTitle);
                return;
            }

            try
            {
                List<SpecResourceMapDefault> specResourceMapDefaults = JsonConvert.DeserializeObject<List<SpecResourceMapDefault>>(this.ECTextSpecDefaultItems.Text, settingFormat);
                specsEntity.resourceMap = new SpecResourceMapEntity { specResourceMapDefault = specResourceMapDefaults };
            }
            catch
            {
                string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "DefaultItems");
                MessageBox.Show(exceptionMessage, ExceptionTitle);
                return;
            }

            try
            {
                specsEntity.specsToAllowZeroCost = JsonConvert.DeserializeObject<List<string>>(this.ECTextSpecAllowZeroCostSpecCodeItems.Text, settingFormat);
            }
            catch
            {
                string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "SpecsToAllowZeroCost");
                MessageBox.Show(exceptionMessage, ExceptionTitle);
                return;
            }

            ManualConfigurationData.Spec = specsEntity;

            this.ECBtnSpecFlag.BackColor = System.Drawing.Color.Green;
            this.ECTextActionMessage.Text = string.Format(CommonSavedSuccessfullyText, "Spec");
            ResetSpecPages();


        }

        #endregion

        #region OverAll Operation

        /// <summary>
        /// Handles the Click event of the ECBtnSaveToLocal control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnSaveToLocal_Click(object sender, EventArgs e)
        {
            SeperateApiConfigurationInfoIntoResource();

            if (this.ECRDBSaveAsFolder.Checked)
            {
                ApiConfigurationManager.SaveConfigurationDataToLocalFolder(ManualConfigurationData, ApiOnBoardingConfigurationToolSaveToLocalFolder);
            }
            else
            {
                ApiConfigurationManager.SaveConfigurationDataToLocalZip(ManualConfigurationData, ApiOnBoardingConfigurationToolSaveToLocalFolder, ManualConfigurationData.ApiFolderName);
            }
        }

        /// <summary>
        /// Handles the Click event of the ECBtnUploadToPPEBlob control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ECBtnUploadToPPEBlob_Click(object sender, EventArgs e)
        {
            try
            {
                SeperateApiConfigurationInfoIntoResource();

                string ApiCongigurationTestBlobAccountName = CloudConfigurationManager.GetSetting("ApiCongigurationTestBlobAccountName");
                string ApiConfigurationTestBlobAccountKey = CloudConfigurationManager.GetSetting("ApiConfigurationTestBlobAccountKey");

                if (string.IsNullOrWhiteSpace(ApiCongigurationTestBlobAccountName))
                {
                    string ErrorMessage = "Not find the key 'ApiCongigurationTestBlobAccountName' in App.config or its value is empty.";
                    MessageBox.Show(ErrorMessage, AlertTitle);
                    return;
                }
                if (string.IsNullOrWhiteSpace(ApiConfigurationTestBlobAccountKey))
                {
                    string ErrorMessage = "Not find the key 'ApiConfigurationTestBlobAccountKey' in App.config or its value is empty.";
                    MessageBox.Show(ErrorMessage, AlertTitle);
                    return;
                }

                StorageCredentials credentials = new StorageCredentials(ApiCongigurationTestBlobAccountName, ApiConfigurationTestBlobAccountKey, "AccountKey");
                BlobHelper.UploadApiConfigurationToBlobContainer(credentials, ApiConfigurationStorageContainerName, ManualConfigurationData, ApiOnBoardingConfigurationToolInnerTempFolder);

                this.ECTextActionMessage.Text = "Upload file successfully.";
            }
            catch (Exception ex)
            {
                string ErrorMessage = string.Format("Original Error Message: {0}", ex.Message);
                MessageBox.Show(ErrorMessage, AlertTitle);
            }
        }

        #endregion

        /// <summary>
        /// Loads the API item list.
        /// </summary>
        private void LoadApiItemList()
        {
            this.ECListItems.Items.Clear();

            foreach (var item in ListCachedApiConfigurationData)
            {
                if (item != null)
                {
                    this.ECListItems.Items.Add(item.ApiFolderName);
                }
            }
        }

        /// <summary>
        /// Loads the API configuration information to page.
        /// </summary>
        /// <param name="apiConfigInfo">The API configuration information.</param>
        private void LoadApiConfigInfoToPage(ApiConfigurationData apiConfigInfo)
        {
            ResetAllExtensionConfigurationControls();
            ManualConfigurationData = RestoreApiConfigurationInfoFromResource(apiConfigInfo);
            if (ManualConfigurationData.Icons.Count > 0)
            {
                this.ApiConfigIcons = ManualConfigurationData.Icons;

                foreach (var icon in this.ApiConfigIcons)
                {
                    this.ECTextSvgFiles.Text += icon.Key + "\r\n";
                    this.ECCBApiIconData.Items.Add(icon.Key);
                    this.ECCBSpecFeatureItemIconSvgData.Items.Add(icon.Key);
                }
            }
            else
            {
                this.ECTextSvgFiles.BackColor = System.Drawing.Color.Red;
                this.ECBtnIconsFlag.BackColor = System.Drawing.Color.Red;
            }

            try
            {
                if (ManualConfigurationData.ApiItem != null)
                {
                    SetControlContentText(this.ECTextApiItem, ManualConfigurationData.ApiItem.item, this.ECBtnApiFlag);
                    SetControlContentText(this.ECTextApiTitle, ManualConfigurationData.ApiItem.title, this.ECBtnApiFlag);
                    SetControlContentText(this.ECTextApiSubTitle, ManualConfigurationData.ApiItem.subtitle, this.ECBtnApiFlag);
                    if (this.ApiConfigIcons.ContainsKey(ManualConfigurationData.ApiItem.iconData))
                    {
                        SetControlContentText(this.ECCBApiIconData, ManualConfigurationData.ApiItem.iconData, this.ECBtnApiFlag);
                    }
                    else
                    {
                        SetControlContentText(this.ECCBApiIconData, null, this.ECBtnApiFlag);
                    }

                    if (ManualConfigurationData.ApiItem.categories.Count > 0)
                    {
                        SetControlContentText(this.ECTextApiCategory, ManualConfigurationData.ApiItem.categories[0], this.ECBtnApiFlag);
                    }
                    else
                    {
                        SetControlContentText(this.ECTextApiCategory, null, this.ECBtnApiFlag);
                    }

                    if (ManualConfigurationData.ApiItem.skuQuota.Count > 0)
                    {
                        SetControlContentText(this.ECTextApiSkuQuotaData, JsonConvert.SerializeObject(ManualConfigurationData.ApiItem.skuQuota, settingFormat), this.ECBtnApiFlag, false);
                    }

                    this.ECRDBApiShowLegalTermTrue.Checked = ManualConfigurationData.ApiItem.showLegalTerm;
                    this.ECRDBApiShowLegalTermFalse.Checked = !ManualConfigurationData.ApiItem.showLegalTerm;

                    SetControlContentText(this.ECTextApiDefaultLegalTerm, ManualConfigurationData.ApiItem.defaultLegalTerm, this.ECBtnApiFlag, false);
                }
                else
                {
                    this.ECBtnApiFlag.BackColor = System.Drawing.Color.DarkRed;
                }
            }
            catch
            {
                this.ECBtnApiFlag.BackColor = System.Drawing.Color.DarkRed;
            }

            try
            {
                if (ManualConfigurationData.QuickStart != null)
                {
                    SetControlContentText(this.ECTextQuickStartItems, JsonConvert.SerializeObject(ManualConfigurationData.QuickStart.quickStarts, settingFormat), this.ECBtnApiFlag);
                }
                else
                {
                    this.ECBtnQuickStartsFlag.BackColor = System.Drawing.Color.DarkRed;
                }
            }
            catch
            {
                this.ECBtnQuickStartsFlag.BackColor = System.Drawing.Color.DarkRed;
            }

            try
            {
                if (ManualConfigurationData.Spec != null)
                {
                    this.ECCBSpecItemFeatureID.Items.Add(string.Empty);
                    foreach (SpecFeatureEntity feature in ManualConfigurationData.Spec.features)
                    {
                        this.ECCBSpecItemFeatureID.Items.Add(feature.id);
                    }

                    this.ECCBSpecDefaultItemSpecCode.Items.Add(string.Empty);
                    this.ECCBSpecAllowZeroCostSpecCode.Items.Add(string.Empty);
                    foreach (SpecUnitEntity specItem in ManualConfigurationData.Spec.specs)
                    {
                        this.ECCBSpecDefaultItemSpecCode.Items.Add(specItem.specCode);
                        this.ECCBSpecAllowZeroCostSpecCode.Items.Add(specItem.specCode);
                    }

                    SetControlContentText(this.ECTextSpecItems, JsonConvert.SerializeObject(ManualConfigurationData.Spec.specs, settingFormat), this.ECBtnSpecFlag);
                    SetControlContentText(this.ECTextSpecFeatureItems, JsonConvert.SerializeObject(ManualConfigurationData.Spec.features, settingFormat), this.ECBtnSpecFlag);
                    if (ManualConfigurationData.Spec.resourceMap.specResourceMapDefault != null)
                    {
                        SetControlContentText(this.ECTextSpecDefaultItems, JsonConvert.SerializeObject(ManualConfigurationData.Spec.resourceMap.specResourceMapDefault, settingFormat), this.ECBtnSpecFlag);
                    }

                    SetControlContentText(this.ECTextSpecAllowZeroCostSpecCodeItems, JsonConvert.SerializeObject(ManualConfigurationData.Spec.specsToAllowZeroCost, settingFormat), this.ECBtnSpecFlag);
                }
                else
                {
                    this.ECBtnSpecFlag.BackColor = System.Drawing.Color.DarkRed;
                }
            }
            catch
            {
                this.ECBtnSpecFlag.BackColor = System.Drawing.Color.DarkRed;
            }


            if (this.ECBtnIconsFlag.BackColor == System.Drawing.Color.Orange)
            {
                this.ECBtnIconsFlag.BackColor = System.Drawing.Color.Green;
            }

            if (this.ECBtnApiFlag.BackColor == System.Drawing.Color.Orange)
            {
                this.ECBtnApiFlag.BackColor = System.Drawing.Color.Green;
            }

            if (this.ECBtnQuickStartsFlag.BackColor == System.Drawing.Color.Orange)
            {
                this.ECBtnQuickStartsFlag.BackColor = System.Drawing.Color.Green;
            }

            if (this.ECBtnSpecFlag.BackColor == System.Drawing.Color.Orange)
            {
                this.ECBtnSpecFlag.BackColor = System.Drawing.Color.Green;
            }
        }

        /// <summary>
        /// Seperates the API configuration information into resource.
        /// </summary>
        private void SeperateApiConfigurationInfoIntoResource()
        {
            string ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.ECTextOverAllApiJson.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Api.Json Content");
            }

            if (string.IsNullOrWhiteSpace(this.ECTextOverAllQuickStartsJson.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "QuickStarts.Json Content");
            }

            if (string.IsNullOrWhiteSpace(this.ECTextOverAllSpecJson.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Spec.Json Content");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            try
            {
                ManualConfigurationData.ApiItem = JsonConvert.DeserializeObject<ApiEntity>(this.ECTextOverAllApiJson.Text, settingFormat);
            }
            catch
            {
                string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "Api.Json Content");
                MessageBox.Show(exceptionMessage, ExceptionTitle);
                return;
            }

            try
            {
                ManualConfigurationData.QuickStart = JsonConvert.DeserializeObject<QuickStartsEntity>(this.ECTextOverAllQuickStartsJson.Text, settingFormat);
            }
            catch
            {
                string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "QuickStarts.Json Content");
                MessageBox.Show(exceptionMessage, ExceptionTitle);
                return;
            }

            try
            {
                ManualConfigurationData.Spec = JsonConvert.DeserializeObject<SpecsEntity>(this.ECTextOverAllSpecJson.Text, settingFormat);
            }
            catch
            {
                string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "Spec.Json Content");
                MessageBox.Show(exceptionMessage, ExceptionTitle);
                return;
            }

            ManualConfigurationData.Icons = this.ApiConfigIcons;

            Dictionary<string, string> resource = new Dictionary<string, string>();

            resource.Add("title", ManualConfigurationData.ApiItem.title);
            ManualConfigurationData.ApiItem.title = string.Format("{0}{1}", ResourceToken, "title");

            resource.Add("subtitle", ManualConfigurationData.ApiItem.subtitle);
            ManualConfigurationData.ApiItem.subtitle = string.Format("{0}{1}", ResourceToken, "subtitle");

            ManualConfigurationData.ApiItem.iconData = string.Format("{0}{1}", IconToken, ManualConfigurationData.ApiItem.iconData);

            resource.Add("defaultLegalTerm", ManualConfigurationData.ApiItem.defaultLegalTerm);
            ManualConfigurationData.ApiItem.defaultLegalTerm = string.Format("{0}{1}", ResourceToken, "defaultLegalTerm");


            for (int i = 0; i < ManualConfigurationData.QuickStart.quickStarts.Count; i++)
            {
                string tempQuickStartItemTitle = string.Format("quickStart{0}.title", i);
                resource.Add(tempQuickStartItemTitle, ManualConfigurationData.QuickStart.quickStarts[i].title);
                ManualConfigurationData.QuickStart.quickStarts[i].title = string.Format("{0}{1}", ResourceToken, tempQuickStartItemTitle);

                string tempQuickStartItemDescription = string.Format("quickStart{0}.des", i);
                resource.Add(tempQuickStartItemDescription, ManualConfigurationData.QuickStart.quickStarts[i].description);
                ManualConfigurationData.QuickStart.quickStarts[i].description = string.Format("{0}{1}", ResourceToken, tempQuickStartItemDescription);

                for (int j = 0; j < ManualConfigurationData.QuickStart.quickStarts[i].links.Count; j++)
                {
                    string tempQuickStartItemLinkItemText = string.Format("quickStart{0}.link{1}.text", i, j);
                    resource.Add(tempQuickStartItemLinkItemText, ManualConfigurationData.QuickStart.quickStarts[i].links[j].text);
                    ManualConfigurationData.QuickStart.quickStarts[i].links[j].text = string.Format("{0}{1}", ResourceToken, tempQuickStartItemLinkItemText);
                }
            }

            for (int i = 0; i < ManualConfigurationData.Spec.features.Count; i++)
            {
                string tempSpecFeatureDisplayName = string.Format("feature.{0}.displayName", ManualConfigurationData.Spec.features[i].id);
                resource.Add(tempSpecFeatureDisplayName, ManualConfigurationData.Spec.features[i].displayName);
                ManualConfigurationData.Spec.features[i].displayName = string.Format("{0}{1}", ResourceToken, tempSpecFeatureDisplayName);

                ManualConfigurationData.Spec.features[i].iconSvgData = string.Format("{0}{1}", IconToken, ManualConfigurationData.Spec.features[i].iconSvgData);
            }

            for (int i = 0; i < ManualConfigurationData.Spec.specs.Count; i++)
            {
                string tempSpecItemTitle = string.Format("spec.{0}.title", ManualConfigurationData.Spec.specs[i].id);
                resource.Add(tempSpecItemTitle, ManualConfigurationData.Spec.specs[i].title);
                ManualConfigurationData.Spec.specs[i].title = string.Format("{0}{1}", ResourceToken, tempSpecItemTitle);

                for (int j = 0; j < ManualConfigurationData.Spec.specs[i].promotedFeatures.Count; j++)
                {
                    SpecPromotedFeature promotedFeature = ManualConfigurationData.Spec.specs[i].promotedFeatures[j];
                    string tempUnitDescription = string.Format("spec.promotedFeature.unitDescription{0}", j);
                    if (resource.ContainsValue(promotedFeature.unitDescription))
                    {
                        tempUnitDescription = resource.FirstOrDefault(d => d.Value == promotedFeature.unitDescription).Key;
                        ManualConfigurationData.Spec.specs[i].promotedFeatures[j].unitDescription = string.Format("{0}{1}", ResourceToken, tempUnitDescription);
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
                                ManualConfigurationData.Spec.specs[i].promotedFeatures[j].unitDescription = string.Format("{0}{1}", ResourceToken, tempUnitDescription);
                                break;
                            }

                            tempIndex++;
                        }
                    }
                }

                string tempSpecItemCostCaption = string.Format("spec.cost.{0}.caption.format", ManualConfigurationData.Spec.specs[i].id);
                resource.Add(tempSpecItemCostCaption, ManualConfigurationData.Spec.specs[i].cost.caption);
                ManualConfigurationData.Spec.specs[i].cost.caption = string.Format("{0}{1}", ResourceToken, tempSpecItemCostCaption);
            }

            ManualConfigurationData.Resources.Clear();
            ManualConfigurationData.Resources.Add("en", resource);
        }

        /// <summary>
        /// Restores the API configuration information from resource.
        /// </summary>
        /// <param name="apiConfigInfo">The API configuration information.</param>
        /// <returns></returns>
        private ApiConfigurationData RestoreApiConfigurationInfoFromResource(ApiConfigurationData apiConfigInfo)
        {
            ApiConfigurationData configInfo = new ApiConfigurationData();

            configInfo.ApiTypeName = apiConfigInfo.ApiTypeName;
            configInfo.ApiFolderName = apiConfigInfo.ApiFolderName;
            configInfo.Icons = apiConfigInfo.Icons;

            Dictionary<string, string> resourceEN;
            if (apiConfigInfo.Resources.ContainsKey("en"))
            {
                resourceEN = apiConfigInfo.Resources["en"];
            }
            else
            {
                resourceEN = new Dictionary<string, string>();
            }

            try
            {
                configInfo.ApiItem = new ApiEntity();
                configInfo.ApiItem.item = apiConfigInfo.ApiItem.item;
                configInfo.ApiItem.title = resourceEN[apiConfigInfo.ApiItem.title.Replace(ResourceToken, string.Empty)];
                configInfo.ApiItem.subtitle = resourceEN[apiConfigInfo.ApiItem.subtitle.Replace(ResourceToken, string.Empty)];
                configInfo.ApiItem.iconData = apiConfigInfo.ApiItem.iconData.Replace(IconToken, string.Empty);

                configInfo.ApiItem.categories = new List<string>();
                foreach (var category in apiConfigInfo.ApiItem.categories)
                {
                    configInfo.ApiItem.categories.Add(category);
                }

                configInfo.ApiItem.skuQuota = new List<ApiSkuQuotaEntity>();
                foreach (var skuQuota in apiConfigInfo.ApiItem.skuQuota)
                {
                    ApiSkuQuotaEntity skuQuotaEntity = new ApiSkuQuotaEntity { code = skuQuota.code, name = skuQuota.name, quota = skuQuota.quota };
                    configInfo.ApiItem.skuQuota.Add(skuQuotaEntity);
                }

                configInfo.ApiItem.showLegalTerm = apiConfigInfo.ApiItem.showLegalTerm;
                if (!string.IsNullOrWhiteSpace(apiConfigInfo.ApiItem.defaultLegalTerm))
                {
                    configInfo.ApiItem.defaultLegalTerm = resourceEN[apiConfigInfo.ApiItem.defaultLegalTerm.Replace(ResourceToken, string.Empty)];
                }
            }
            catch
            {
                this.ECBtnApiFlag.BackColor = System.Drawing.Color.DarkRed;
            }

            try
            {
                configInfo.QuickStart = new QuickStartsEntity();
                if (apiConfigInfo.QuickStart != null)
                {
                    configInfo.QuickStart.quickStarts = new List<QuickStartUnit>();
                    for (int i = 0; i < apiConfigInfo.QuickStart.quickStarts.Count; i++)
                    {
                        QuickStartUnit quickStartUnit = new QuickStartUnit();

                        quickStartUnit.title = resourceEN[apiConfigInfo.QuickStart.quickStarts[i].title.Replace(ResourceToken, "")];
                        quickStartUnit.description = resourceEN[apiConfigInfo.QuickStart.quickStarts[i].description.Replace(ResourceToken, "")];
                        quickStartUnit.icon = apiConfigInfo.QuickStart.quickStarts[i].icon;

                        quickStartUnit.links = new List<QuickStartLink>();

                        for (int j = 0; j < apiConfigInfo.QuickStart.quickStarts[i].links.Count; j++)
                        {
                            QuickStartLink link = new QuickStartLink();
                            link.text = resourceEN[apiConfigInfo.QuickStart.quickStarts[i].links[j].text.Replace(ResourceToken, "")];
                            link.uri = apiConfigInfo.QuickStart.quickStarts[i].links[j].uri;
                            quickStartUnit.links.Add(link);
                        }

                        configInfo.QuickStart.quickStarts.Add(quickStartUnit);
                    }
                }
                else
                {
                    this.ECBtnQuickStartsFlag.BackColor = System.Drawing.Color.DarkRed;
                }
            }
            catch
            {
                this.ECBtnQuickStartsFlag.BackColor = System.Drawing.Color.DarkRed;
            }

            try
            {
                configInfo.Spec = new SpecsEntity();
                configInfo.Spec.specType = apiConfigInfo.Spec.specType;

                if (apiConfigInfo.Spec != null)
                {
                    configInfo.Spec.features = new List<SpecFeatureEntity>();
                    for (int i = 0; i < apiConfigInfo.Spec.features.Count; i++)
                    {
                        SpecFeatureEntity feature = new SpecFeatureEntity();
                        feature.id = apiConfigInfo.Spec.features[i].id;
                        feature.displayName = resourceEN[apiConfigInfo.Spec.features[i].displayName.Replace(ResourceToken, "")];

                        if (!string.IsNullOrWhiteSpace(apiConfigInfo.Spec.features[i].iconSvgData))
                        {
                            feature.iconSvgData = apiConfigInfo.Spec.features[i].iconSvgData.Replace(ResourceToken, "");
                        }

                        if (!string.IsNullOrWhiteSpace(apiConfigInfo.Spec.features[i].iconName))
                        {
                            feature.iconName = apiConfigInfo.Spec.features[i].iconName;
                        }

                        configInfo.Spec.features.Add(feature);
                    }

                    configInfo.Spec.specs = new List<SpecUnitEntity>();
                    for (int i = 0; i < apiConfigInfo.Spec.specs.Count; i++)
                    {
                        SpecUnitEntity specUnitEntity = new SpecUnitEntity();
                        specUnitEntity.id = apiConfigInfo.Spec.specs[i].id;
                        specUnitEntity.colorScheme = apiConfigInfo.Spec.specs[i].colorScheme;
                        specUnitEntity.title = resourceEN[apiConfigInfo.Spec.specs[i].title.Replace(ResourceToken, "")];
                        specUnitEntity.specCode = apiConfigInfo.Spec.specs[i].specCode;

                        specUnitEntity.promotedFeatures = new List<SpecPromotedFeature>();
                        for (int j = 0; j < apiConfigInfo.Spec.specs[i].promotedFeatures.Count; j++)
                        {
                            SpecPromotedFeature specPromotedFeature = new SpecPromotedFeature();
                            specPromotedFeature.value = apiConfigInfo.Spec.specs[i].promotedFeatures[j].value;
                            specPromotedFeature.unitDescription = resourceEN[apiConfigInfo.Spec.specs[i].promotedFeatures[j].unitDescription.Replace(ResourceToken, "")];

                            specUnitEntity.promotedFeatures.Add(specPromotedFeature);
                        }

                        configInfo.Spec.specs.Add(specUnitEntity);

                        specUnitEntity.features = new List<SpecFeatureUnit>();
                        for (int j = 0; j < apiConfigInfo.Spec.specs[i].features.Count; j++)
                        {
                            SpecFeatureUnit specFeatureUnit = new SpecFeatureUnit();
                            specFeatureUnit.id = apiConfigInfo.Spec.specs[i].features[j].id;

                            specUnitEntity.features.Add(specFeatureUnit);
                        }

                        SpecCost specCost = new SpecCost();
                        if (apiConfigInfo.Spec.specs[i].cost.amount != null)
                        {
                            specCost.amount = apiConfigInfo.Spec.specs[i].cost.amount;
                        }

                        if (!string.IsNullOrWhiteSpace(apiConfigInfo.Spec.specs[i].cost.currencyCode))
                        {
                            specCost.currencyCode = apiConfigInfo.Spec.specs[i].cost.currencyCode;
                        }

                        specCost.caption = resourceEN[apiConfigInfo.Spec.specs[i].cost.caption.Replace(ResourceToken, "")];

                        configInfo.Spec.specs[i].cost = specCost;
                    }

                    configInfo.Spec.resourceMap = apiConfigInfo.Spec.resourceMap;
                    configInfo.Spec.specsToAllowZeroCost = apiConfigInfo.Spec.specsToAllowZeroCost;
                }
                else
                {
                    this.ECBtnSpecFlag.BackColor = System.Drawing.Color.DarkRed;
                }
            }
            catch
            {
                this.ECBtnSpecFlag.BackColor = System.Drawing.Color.DarkRed;
            }

            return configInfo;
        }

        #endregion

        #region Sku Configuration

        #region PPE

        /// <summary>
        /// Handles the Click event of the SCBtnPPEAddLocationItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SCBtnPPEAddLocationItem_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(this.SCTextPPESkuItemLocationItemLocation.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Location");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            LocationEntity locationEntity = new LocationEntity();
            locationEntity.location = this.SCTextPPESkuItemLocationItemLocation.Text;
            if (!string.IsNullOrWhiteSpace(this.SCTextPPESkuItemLocationItemApimProductID.Text))
            {
                locationEntity.apimProductId = this.SCTextPPESkuItemLocationItemApimProductID.Text;
            }

            List<LocationEntity> locations = new List<LocationEntity>();
            if (!string.IsNullOrWhiteSpace(this.SCTextPPESkuItemLocations.Text))
            {
                try
                {
                    locations = JsonConvert.DeserializeObject<List<LocationEntity>>(this.SCTextPPESkuItemLocations.Text, specialSettingFormat);
                }
                catch
                {
                    MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "Locations"), AlertTitle);
                    return;
                }
            }

            locations.Add(locationEntity);
            this.SCTextPPESkuItemLocations.Text = JsonConvert.SerializeObject(locations, specialSettingFormat);
            this.SCTextPPESkuItemLocationItemLocation.Text = string.Empty;
            this.SCTextPPESkuItemLocationItemApimProductID.Text = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the SCBtnAddPPESkuItemMeterID control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SCBtnAddPPESkuItemMeterID_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(this.SCTextPPESkuItemMeterID.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "MeterID");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            List<string> MeterIDs = new List<string>();
            if (!string.IsNullOrWhiteSpace(this.SCTextPPESkuItemMeterIDs.Text))
            {
                try
                {
                    MeterIDs = JsonConvert.DeserializeObject<List<string>>(this.SCTextPPESkuItemMeterIDs.Text, settingFormat);
                }
                catch
                {
                    MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "MeterIDS"), AlertTitle);
                    return;
                }
            }

            MeterIDs.Add(this.SCTextPPESkuItemMeterID.Text);
            this.SCTextPPESkuItemMeterIDs.Text = JsonConvert.SerializeObject(MeterIDs, settingFormat);
            this.SCTextPPESkuItemMeterID.Text = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the SCBtnPPEAddSkuItemRequiredFeature control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SCBtnPPEAddSkuItemRequiredFeature_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(this.SCTextPPESkuItemRequiredFeature.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Feature");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            List<string> requiredFeatures = new List<string>();
            if (!string.IsNullOrWhiteSpace(this.SCTextPPESkuItemRequiredFeatures.Text))
            {
                try
                {
                    requiredFeatures = JsonConvert.DeserializeObject<List<string>>(this.SCTextPPESkuItemRequiredFeatures.Text, settingFormat);
                }
                catch
                {
                    MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "RequiredFeatures"), AlertTitle);
                    return;
                }
            }

            requiredFeatures.Add(this.SCTextPPESkuItemRequiredFeature.Text);
            this.SCTextPPESkuItemRequiredFeatures.Text = JsonConvert.SerializeObject(requiredFeatures, settingFormat);
            this.SCTextPPESkuItemRequiredFeature.Text = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the SCBtnPPEAddSkuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SCBtnPPEAddSkuItem_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(this.SCTextPPESkuItemName.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Name");
            }

            if (string.IsNullOrWhiteSpace(this.SCTextPPESkuItemTier.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Tier");
            }

            if (string.IsNullOrWhiteSpace(this.SCTextPPESkuItemLocations.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Locations");
            }

            if (string.IsNullOrWhiteSpace(this.SCTextPPESkuItemMeterIDs.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "MeterIDs");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            SkuEntity skuEntity = new SkuEntity();
            skuEntity.name = this.SCTextPPESkuItemName.Text;
            skuEntity.tier = this.SCTextPPESkuItemTier.Text;

            if (!string.IsNullOrWhiteSpace(this.SCTextPPESkuItemSkuType.Text))
            {
                skuEntity.skutype = this.SCTextPPESkuItemSkuType.Text;
            }

            if (!string.IsNullOrWhiteSpace(this.SCTextPPESkuItemSkuQuota.Text))
            {
                skuEntity.skuquota = this.SCTextPPESkuItemSkuQuota.Text;
            }

            if (!string.IsNullOrWhiteSpace(this.SCTextPPESkuItemApimProductID.Text))
            {
                skuEntity.apimProductId = this.SCTextPPESkuItemApimProductID.Text;
            }

            try
            {
                skuEntity.locations = JsonConvert.DeserializeObject<List<LocationEntity>>(this.SCTextPPESkuItemLocations.Text, specialSettingFormat);
            }
            catch
            {
                MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "Locations"));
                return;
            }

            try
            {
                skuEntity.meterIds = JsonConvert.DeserializeObject<List<string>>(this.SCTextPPESkuItemMeterIDs.Text, settingFormat);
            }
            catch
            {
                MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "MeterIDs"));
                return;
            }

            if (!string.IsNullOrWhiteSpace(this.SCTextPPESkuItemRequiredFeatures.Text))
            {
                try
                {
                    skuEntity.requiredFeatures = JsonConvert.DeserializeObject<List<string>>(this.SCTextPPESkuItemRequiredFeatures.Text, settingFormat);
                }
                catch
                {
                    MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "RequiredFeatures"));
                    return;
                }
            }

            List<SkuEntity> skuEntities = new List<SkuEntity>();
            if (!string.IsNullOrWhiteSpace(this.SCTextPPESkuItems.Text))
            {
                try
                {
                    skuEntities = JsonConvert.DeserializeObject<List<SkuEntity>>(this.SCTextPPESkuItems.Text, settingFormat);
                }
                catch
                {
                    MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "SkuItems"));
                    return;
                }
            }

            skuEntities.Add(skuEntity);
            this.SCTextPPESkuItems.Text = JsonConvert.SerializeObject(skuEntities, specialSettingFormat);
            this.SCTextPPESkuItemName.Text = string.Empty;
            this.SCTextPPESkuItemTier.Text = string.Empty;
            this.SCTextPPESkuItemSkuType.Text = string.Empty;
            this.SCTextPPESkuItemSkuQuota.Text = string.Empty;
            this.SCTextPPESkuItemApimProductID.Text = string.Empty;
            this.SCTextPPESkuItemLocations.Text = string.Empty;
            this.SCTextPPESkuItemMeterIDs.Text = string.Empty;
            this.SCTextPPESkuItemRequiredFeatures.Text = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the SCBtnUploadToPPE control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SCBtnUploadToPPE_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.SCTextPPEName.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Name");
            }

            if (string.IsNullOrWhiteSpace(this.SCTextPPEDisplayName.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "DisplayName");
            }

            if (string.IsNullOrWhiteSpace(this.SCTextPPEApimInstance.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "ApimInstance");
            }

            if (string.IsNullOrWhiteSpace(this.SCTextPPEApiPath.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "ApiPath");
            }

            if (string.IsNullOrWhiteSpace(this.SCTextPPESkuItems.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "SkuItems");
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            SkuConfigEntity skuConfigEntity = new SkuConfigEntity();
            skuConfigEntity.name = this.SCTextPPEName.Text;
            skuConfigEntity.displayName = this.SCTextPPEDisplayName.Text;
            skuConfigEntity.apimInstance = this.SCTextPPEApimInstance.Text;
            skuConfigEntity.apiPath = this.SCTextPPEApiPath.Text;

            try
            {
                skuConfigEntity.skus = JsonConvert.DeserializeObject<List<SkuEntity>>(this.SCTextPPESkuItems.Text, settingFormat);
            }
            catch
            {
                MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "SkuItems"));
                return;
            }

            string tempFileName = string.Format("{0}.json", skuConfigEntity.name.Replace(".", ""));
            ApiConfigurationManager.SaveContentStringToLocalFile(ApiOnBoardingConfigurationToolInnerTempFolder, tempFileName,JsonConvert.SerializeObject(skuConfigEntity,specialSettingFormat));
        }

        #endregion

        #region Production
        private void SCBtnLoadFromPPE_Click(object sender, EventArgs e)
        {

        }

        private void SCListItems_DoubleClick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the SCBtnProAddLocationItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SCBtnProAddLocationItem_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(this.SCTextProSkuItemLocationItemLocation.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Location"); ;
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            LocationEntity locationEntity = new LocationEntity();
            locationEntity.location = this.SCTextProSkuItemLocationItemLocation.Text;
            if (!string.IsNullOrWhiteSpace(this.SCTextProSkuItemLocationItemApimProductID.Text))
            {
                locationEntity.apimProductId = this.SCTextProSkuItemLocationItemApimProductID.Text;
            }

            List<LocationEntity> locations = new List<LocationEntity>();
            if (!string.IsNullOrWhiteSpace(this.SCTextProSkuItemLocations.Text))
            {
                try
                {
                    locations = JsonConvert.DeserializeObject<List<LocationEntity>>(this.SCTextProSkuItemLocations.Text, specialSettingFormat);
                }
                catch
                {
                    MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "Locations"), AlertTitle);
                    return;
                }
            }

            locations.Add(locationEntity);
            this.SCTextProSkuItemLocations.Text = JsonConvert.SerializeObject(locations, specialSettingFormat);
            this.SCTextProSkuItemLocationItemLocation.Text = string.Empty;
            this.SCTextProSkuItemLocationItemApimProductID.Text = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the SCBtnAddProSkuItemMeterID control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SCBtnAddProSkuItemMeterID_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(this.SCTextProSkuItemMeterID.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "MeterID"); ;
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            List<string> MeterIDs = new List<string>();
            if (!string.IsNullOrWhiteSpace(this.SCTextProSkuItemMeterIDs.Text))
            {
                try
                {
                    MeterIDs = JsonConvert.DeserializeObject<List<string>>(this.SCTextProSkuItemMeterIDs.Text, settingFormat);
                }
                catch
                {
                    MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "MeterIDS"), AlertTitle);
                    return;
                }
            }

            MeterIDs.Add(this.SCTextProSkuItemMeterID.Text);
            this.SCTextProSkuItemMeterIDs.Text = JsonConvert.SerializeObject(MeterIDs, settingFormat);
            this.SCTextProSkuItemMeterID.Text = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the SCBtnProAddSkuItemRequiredFeature control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SCBtnProAddSkuItemRequiredFeature_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(this.SCTextProSkuItemRequiredFeature.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Feature"); ;
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            List<string> requiredFeatures = new List<string>();
            if (!string.IsNullOrWhiteSpace(this.SCTextProSkuItemRequiredFeatures.Text))
            {
                try
                {
                    requiredFeatures = JsonConvert.DeserializeObject<List<string>>(this.SCTextProSkuItemRequiredFeatures.Text, settingFormat);
                }
                catch
                {
                    MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "RequiredFeatures"), AlertTitle);
                    return;
                }
            }

            requiredFeatures.Add(this.SCTextProSkuItemRequiredFeature.Text);
            this.SCTextProSkuItemRequiredFeatures.Text = JsonConvert.SerializeObject(requiredFeatures, settingFormat);
            this.SCTextProSkuItemRequiredFeature.Text = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the SCBtnProAddSkuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SCBtnProAddSkuItem_Click(object sender, EventArgs e)
        {
            string ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(this.SCTextProSkuItemName.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Name"); ;
            }

            if (string.IsNullOrWhiteSpace(this.SCTextProSkuItemTier.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Tier"); ;
            }

            if (string.IsNullOrWhiteSpace(this.SCTextProSkuItemLocations.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "Locations"); ;
            }

            if (string.IsNullOrWhiteSpace(this.SCTextProSkuItemMeterIDs.Text))
            {
                ErrorMessage += string.Format(TemplateShouldNotBeEmptyText, "MeterIDs"); ;
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MessageBox.Show(ErrorMessage, AlertTitle);
                return;
            }

            SkuEntity skuEntity = new SkuEntity();
            skuEntity.name = this.SCTextProSkuItemName.Text;
            skuEntity.tier = this.SCTextProSkuItemTier.Text;

            if (!string.IsNullOrWhiteSpace(this.SCTextProSkuItemSkuType.Text))
            {
                skuEntity.skutype = this.SCTextProSkuItemSkuType.Text;
            }

            if (!string.IsNullOrWhiteSpace(this.SCTextProSkuItemSkuQuota.Text))
            {
                skuEntity.skuquota = this.SCTextProSkuItemSkuQuota.Text;
            }

            if (!string.IsNullOrWhiteSpace(this.SCTextProSkuItemApimProductID.Text))
            {
                skuEntity.apimProductId = this.SCTextProSkuItemApimProductID.Text;
            }

            try
            {
                skuEntity.locations = JsonConvert.DeserializeObject<List<LocationEntity>>(this.SCTextProSkuItemLocations.Text, specialSettingFormat);
            }
            catch
            {
                MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "Locations"));
                return;
            }

            try
            {
                skuEntity.meterIds = JsonConvert.DeserializeObject<List<string>>(this.SCTextProSkuItemMeterIDs.Text, settingFormat);
            }
            catch
            {
                MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "MeterIDs"));
                return;
            }

            if (!string.IsNullOrWhiteSpace(this.SCTextProSkuItemRequiredFeatures.Text))
            {
                try
                {
                    skuEntity.requiredFeatures = JsonConvert.DeserializeObject<List<string>>(this.SCTextProSkuItemRequiredFeatures.Text, settingFormat);
                }
                catch
                {
                    MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "RequiredFeatures"));
                    return;
                }
            }

            List<SkuEntity> skuEntities = new List<SkuEntity>();
            if (!string.IsNullOrWhiteSpace(this.SCTextProSkuItems.Text))
            {
                try
                {
                    skuEntities = JsonConvert.DeserializeObject<List<SkuEntity>>(this.SCTextProSkuItems.Text, settingFormat);
                }
                catch
                {
                    MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "SkuItems"));
                    return;
                }
            }

            skuEntities.Add(skuEntity);
            this.SCTextProSkuItems.Text = JsonConvert.SerializeObject(skuEntities, specialSettingFormat);
            this.SCTextProSkuItemName.Text = string.Empty;
            this.SCTextProSkuItemTier.Text = string.Empty;
            this.SCTextProSkuItemSkuType.Text = string.Empty;
            this.SCTextProSkuItemSkuQuota.Text = string.Empty;
            this.SCTextProSkuItemApimProductID.Text = string.Empty;
            this.SCTextProSkuItemLocations.Text = string.Empty;
            this.SCTextProSkuItemMeterIDs.Text = string.Empty;
            this.SCTextProSkuItemRequiredFeatures.Text = string.Empty;
        }

        #endregion

        #endregion

        #region Meter Configuration



        #endregion

        /// <summary>
        /// Sets the control content text.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="contentText">The content text.</param>
        /// <param name="flagControl">The flag control.</param>
        /// <param name="isRequired">if set to <c>true</c> [is required].</param>
        private void SetControlContentText(Control control, string contentText, Control flagControl, bool isRequired = true)
        {
            if (isRequired)
            {
                if (string.IsNullOrWhiteSpace(contentText))
                {
                    control.Text = string.Empty;
                    control.BackColor = System.Drawing.Color.Red;
                    flagControl.BackColor = System.Drawing.Color.Red;
                }
                else
                {
                    control.BackColor = System.Drawing.Color.White;
                    control.Text = contentText;
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(contentText))
                {
                    control.Text = contentText;
                }
            }
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="content">The content.</param>
        private void LogError(string content)
        {
            try
            {
                string logFileName = string.Format("ErrorLog--{0}.txt", DateTime.Now.ToString("yyyy-MM-dd"));
                string logFilePath = string.Format("{0}{1}", ApiOnBoardingConfigurationToolFolder, logFileName);
                using (FileStream fs = File.Open(logFilePath, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(content);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ExceptionTitle);
            }
        }

    }
}