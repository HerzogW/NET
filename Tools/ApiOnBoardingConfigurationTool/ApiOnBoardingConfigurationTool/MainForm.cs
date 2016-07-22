
namespace ApiOnBoardingConfigurationTool
{
    using ExtensionConfigurationEntity;
    using MeterConfigurationEntity;
    using Microsoft.Azure;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Newtonsoft.Json;
    using SkuConfigurationEntity;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        #region folder path
        private const string ApiOnBoardingConfigurationToolRootFolder = @"C:\ApiOnBoardingConfigurationTool";

        private const string ApiOnBoardingConfigurationToolECSaveToLocalFolder = @"C:\ApiOnBoardingConfigurationTool\Extension\SaveToLocal";

        private const string ApiOnBoardingConfigurationToolECTempFolder = @"C:\ApiOnBoardingConfigurationTool\Extension\Temp";

        private const string ApiOnBoardingConfigurationToolECTempConfigurationFolder = @"C:\ApiOnBoardingConfigurationTool\Extension\TempConfiguration";

        #endregion

        #region

        private const string ResourceToken = "ms-resource:";

        private const string IconToken = "ms-icon:";

        public string ECConfigurationStorageContainerName = "apiconfiguration";

        public string SCConfigurationStorageContainerName = "cs-sku";

        public string MCConfigurationStorageContainerName = "meters";

        #endregion

        #region Common Text

        private const string AlertTitle = "Alert";

        private const string ExceptionTitle = "Exception";

        private const string LoadingFileMessage = "Loading files...";

        private const string LoadedMessage = "Loaded!\r\nCreate or Edit?";

        private const string CommonLoadedSuccessfullyText = "Loaded successfully!";

        private const string CommonUploadSuccessfullyText = "Uploaded successfully!";

        private const string CommonOccuredErrorText1 = "Occured some errors!";

        private const string CommonOccuredErrorText2 = "Occured some errors!\r\nPlease check the ErrorLog!";

        private const string CommonDeleteConfirmText = "Confirm to delete these items?";

        private const string CommonDeleteFromPPESuccessfullyText = "Delete these items from PPE blob successfully!";

        private const string CommonInputStorageAccountAlertText = "Please input the Storage Account.\r\n";

        private const string CommonInputStorageAccountKeyAlertText = "Please input the Storage Account Key.\r\n";

        private const string TemplateSavedSuccessfullyText = "'{0}' saved successfully!";

        private const string TemplateShouldNotBeEmptyText = "'{0}' should not be empty!\r\n";

        private const string TemplateShouldNotAllBeEmptyText = "'{0}' and '{1}' should not all be empty!\r\n";

        private const string TemplateCannotBeDeserializedText = "'{0}' can not be converted!";

        private const string TemplateShouldBeANumberText = "'{0}' should be number!\r\n";

        private const string TemplateConnotFindConfigKeyValue = "Not find the key '{0}' in App.config or its value is empty.";

        #endregion

        private JsonSerializerSettings settingFormat = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Include,
            MissingMemberHandling = MissingMemberHandling.Error
        };

        private JsonSerializerSettings specialSettingFormat = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        private string originalFileOrFolderName = string.Empty;

        private ApiConfigurationData loadedConfigurationData;

        private Dictionary<string, string> ApiConfigIcons = new Dictionary<string, string>();

        private List<ApiConfigurationData> ListCachedExtensionConfigurationData;

        private Dictionary<string, string> ListCachedSkuConfigurationData = new Dictionary<string, string>();

        private Dictionary<string, string> ListCachedMeterConfigurationData = new Dictionary<string, string>();


        public MainForm()
        {
            InitializeComponent();
            string containerName = CloudConfigurationManager.GetSetting("ApiConfigurationStorageContainerName");
            if (!string.IsNullOrWhiteSpace(containerName))
            {
                this.ECConfigurationStorageContainerName = containerName;
            }
        }

        #region Extension Configuration

        #region Action And Review

        private void ECBtnLoadFromLocal_Click(object sender, EventArgs e)
        {
            if (this.ECFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                loadedConfigurationData = ApiConfigurationManager.LoadSingelApiConfigurationFormLoadFolderPath(this.ECFolderBrowserDialog.SelectedPath);

                if (loadedConfigurationData.listError.Count > 0)
                {
                    int i = 1;
                    string tempErrorMessage = string.Empty;
                    foreach (ErrorEntity errorEntity in loadedConfigurationData.listError)
                    {
                        tempErrorMessage += (i++) + "、" + errorEntity.GetErrorInfo() + "\r\n";
                    }

                    LogError(tempErrorMessage);
                    MessageBox.Show(CommonOccuredErrorText2, AlertTitle);
                    this.ECTextActionMessage.Text = CommonOccuredErrorText1;
                }
                else
                {
                    this.ECTextActionMessage.Text = LoadedMessage;
                }
            }
        }

        private void ECBtnLoadFromPPEBlob_Click(object sender, EventArgs e)
        {
            this.ECBtnLoadFromPPEBlob.Enabled = false;
            this.ECBtnLoadFromLocal.Enabled = false;
            this.ECTextActionMessage.Text = LoadingFileMessage;

            var apiConfigurationPublicAccessUrl = CloudConfigurationManager.GetSetting("ApiConfigurationPublicAccessUrl");
            if (string.IsNullOrWhiteSpace(apiConfigurationPublicAccessUrl))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiConfigurationPublicAccessUrl"), AlertTitle);
                return;
            }

            ApiConfigurationManager manager = new ApiConfigurationManager(apiConfigurationPublicAccessUrl, this.ECConfigurationStorageContainerName);
            ListCachedExtensionConfigurationData = manager.LoadDataToCache();

            List<ErrorEntity> listError = manager.listError;

            if (manager.listError.Count > 0)
            {
                this.ECTextActionMessage.Text = CommonOccuredErrorText1;
                int i = 1;
                string tempErrorMessage = string.Empty; ;
                foreach (ErrorEntity errorMessage in manager.listError)
                {
                    tempErrorMessage += (i++) + "、" + errorMessage.GetErrorInfo() + "\r\n";
                }

                LogError(tempErrorMessage);
                MessageBox.Show(CommonOccuredErrorText2, AlertTitle);
                this.ECTextActionMessage.Text = CommonOccuredErrorText1;
            }
            else
            {
                this.ECTextActionMessage.Text = CommonLoadedSuccessfullyText;
            }

            LoadApiItemList();

            this.ECBtnLoadFromPPEBlob.Enabled = true;
            this.ECBtnLoadFromLocal.Enabled = true;
        }

        private void ECListItems_DoubleClick(object sender, EventArgs e)
        {
            if (this.ECListItems.Items.Count > 0)
            {
                string selectedApiItemName = this.ECListItems.SelectedItem.ToString();
                foreach (var apiConfigInfo in ListCachedExtensionConfigurationData)
                {
                    if (apiConfigInfo.ApiFolderName.Equals(selectedApiItemName))
                    {
                        //LoadApiConfigInfoToPage(apiConfigInfo);
                        loadedConfigurationData = apiConfigInfo;
                        this.ECTextActionMessage.Text = LoadedMessage;
                        break;
                    }
                }
            }
        }

        private void ECBtnCreate_Click(object sender, EventArgs e)
        {
            if (loadedConfigurationData != null)
            {
                originalFileOrFolderName = string.Empty;
                LoadApiConfigInfoToPage(loadedConfigurationData);
                ResetJsonFileTextBox();
                this.ECTextActionMessage.Text = string.Format("Load '{0}' as a template to create a new ApiConfiguration.\r\nTarget Folder or Zip file will be named as the 'ApiTypeName' field.", loadedConfigurationData.ApiFolderName);
                loadedConfigurationData = null;
            }
            else
            {
                if (MessageBox.Show("Confirm to clear all fields?", AlertTitle, MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    originalFileOrFolderName = string.Empty;
                    this.ApiConfigIcons = new Dictionary<string, string>();
                    ResetAllExtensionConfigurationControls();
                }
                else
                {
                    this.ECTextActionMessage.Text = string.Empty; ;
                }
            }
        }

        private void ECBtnEdit_Click(object sender, EventArgs e)
        {
            if (loadedConfigurationData != null)
            {
                originalFileOrFolderName = loadedConfigurationData.ApiFolderName;
                LoadApiConfigInfoToPage(loadedConfigurationData);
                loadedConfigurationData = null;
                ResetJsonFileTextBox();
                this.ECTextActionMessage.Text = string.Format("Edit the configuration '{0}'.\r\nTarget Folder or Zip file will be named as original.", originalFileOrFolderName);
            }
            else
            {
                this.ECTextActionMessage.Text = "Please load an ApiConfiguration!";
            }
        }

        private void ECBtnDeleteFromPPEBlob_Click(object sender, EventArgs e)
        {
            if (this.ECListItems.CheckedItems.Count == 0)
            {
                return;
            }

            if (MessageBox.Show(CommonDeleteConfirmText, AlertTitle, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }

            List<string> selectApiItems = new List<string>();

            for (int i = 0; i < this.ECListItems.CheckedItems.Count; i++)
            {
                selectApiItems.Add(this.ECListItems.CheckedItems[i].ToString());
            }

            for (int i = 0; i < selectApiItems.Count; i++)
            {
                this.ECListItems.Items.Remove(selectApiItems[i]);
            }

            string ApiCongigurationPPEBlobAccountName = CloudConfigurationManager.GetSetting("ApiCongigurationTestBlobAccountName");
            string ApiConfigurationPPEBlobAccountKey = CloudConfigurationManager.GetSetting("ApiConfigurationTestBlobAccountKey");

            if (string.IsNullOrWhiteSpace(ApiCongigurationPPEBlobAccountName))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiCongigurationTestBlobAccountName"), AlertTitle);
                return;
            }
            if (string.IsNullOrWhiteSpace(ApiConfigurationPPEBlobAccountKey))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiConfigurationTestBlobAccountKey"), AlertTitle);
                return;
            }

            try
            {
                StorageCredentials credentials = new StorageCredentials(ApiCongigurationPPEBlobAccountName, ApiConfigurationPPEBlobAccountKey, "AccountKey");
                BlobHelper.DeleteApiConfigurationListFromBlobContainer(credentials, ECConfigurationStorageContainerName, selectApiItems, "zip");

                this.ECTextActionMessage.Text = CommonDeleteFromPPESuccessfullyText;
            }
            catch
            {
                this.ECTextActionMessage.Text = CommonOccuredErrorText1;
            }
        }

        private void ECBtnUploadToProBlob_Click(object sender, EventArgs e)
        {
            string storageAccount = this.ECTextStorageAccount.Text;
            string storageAccountKey = this.ECTextStorageAccountKey.Text;

            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(storageAccount))
            {
                ErrorMessage.Append(CommonInputStorageAccountAlertText);
            }

            if (string.IsNullOrWhiteSpace(storageAccountKey))
            {
                ErrorMessage.Append(CommonInputStorageAccountKeyAlertText);
            }

            if (this.ECListItems.CheckedItems.Count == 0)
            {
                ErrorMessage.Append("Please select the api items to upload to production blob.\r\n");
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return;
            }

            if (MessageBox.Show("Confirm to upload to Production Blob?", AlertTitle, MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            this.ECBtnUploadToProBlob.Enabled = false;
            this.ECTextActionMessage.Text = "Uploading...";
            List<string> selectApiItems = new List<string>();

            for (int i = 0; i < this.ECListItems.CheckedItems.Count; i++)
            {
                selectApiItems.Add(this.ECListItems.CheckedItems[i].ToString());
            }

            try
            {
                List<ApiConfigurationData> selectedApiConfigurationDataList = new List<ApiConfigurationData>();
                foreach (var data in ListCachedExtensionConfigurationData)
                {
                    if (selectApiItems.Contains(data.ApiFolderName))
                    {
                        selectedApiConfigurationDataList.Add(data);
                    }
                }

                StorageCredentials credentials = new StorageCredentials(storageAccount, storageAccountKey, "AccountKey");
                BlobHelper.UploadApiConfigurationListToBlobContainer(credentials, ECConfigurationStorageContainerName, selectedApiConfigurationDataList, ApiOnBoardingConfigurationToolECTempFolder);

                this.ECTextActionMessage.Text = "Upload selected items from PPE Blob to Productive Blob successfully!";
            }
            catch
            {
                this.ECTextActionMessage.Text = CommonOccuredErrorText1;
            }

            this.ECBtnUploadToProBlob.Enabled = true;
        }

        #endregion

        #region Flags Operation

        private void ECBtnIconsFlag_Click(object sender, EventArgs e)
        {
            this.TabApiItemPage.SelectedTab = this.TabIcon;
        }

        private void ECBtnApiFlag_Click(object sender, EventArgs e)
        {
            this.TabApiItemPage.SelectedTab = this.TabApi;
        }

        private void ECBtnQuickStartsFlag_Click(object sender, EventArgs e)
        {
            this.TabApiItemPage.SelectedTab = this.TabQuickStarts;
        }

        private void ECBtnSpecFlag_Click(object sender, EventArgs e)
        {
            this.TabApiItemPage.SelectedTab = this.TabSpec;
        }

        #endregion

        #region Reset Pages

        private void ResetAllFlagControl()
        {
            ///Flags
            this.ECBtnIconsFlag.BackColor = System.Drawing.Color.Orange;
            this.ECBtnApiFlag.BackColor = System.Drawing.Color.Orange;
            this.ECBtnQuickStartsFlag.BackColor = System.Drawing.Color.Orange;
            this.ECBtnSpecFlag.BackColor = System.Drawing.Color.Orange;
        }

        private void ResetIconsPage()
        {
            ///Svg Files
            this.ECTextIconFolderPath.Text = string.Empty;
            this.ECTextSvgFiles.Text = string.Empty;
            this.ECTextSvgFiles.BackColor = System.Drawing.Color.White;
        }

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
            this.ECTextApiSkuQuotaCode.Text = "F0";
            this.ECTextApiSkuQuotaName.Text = "Free";
            this.ECTextApiSkuQuotaQuota.Text = "1";
            this.ECTextApiSkuQuotaData.Text = string.Empty;
            this.ECRDBApiShowLegalTermTrue.Checked = true;
            this.ECRDBApiShowLegalTermFalse.Checked = false;
            this.ECTextApiDefaultLegalTerm.Text = string.Empty;
        }

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

        private void ResetSpecPages()
        {
            ResetSpecFeaturesPage();
            ResetSpecItemsPage();
            ResetSpecCostItemPage();
            ResetSpecAllowZeroCostPage();
        }

        private void ResetSpecFeaturesPage()
        {
            ///Spec.Json Features
            this.ECTextSpecFeatureItemDisplayName.Text = string.Empty;
            this.ECCBSpecFeatureItemIconSvgData.Items.Clear();
            this.ECTextSpecFeatureItemIconName.Text = string.Empty;
            this.ECTextSpecFeatureItems.Text = string.Empty;
            this.ECTextSpecFeatureItems.BackColor = System.Drawing.Color.White;
        }

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

        private void ResetSpecAllowZeroCostPage()
        {
            ///Spec.Json AllowZeroCost
            this.ECCBSpecAllowZeroCostSpecCode.Items.Clear();
            this.ECTextSpecAllowZeroCostSpecCodeItems.Text = string.Empty;
            this.ECTextSpecAllowZeroCostSpecCodeItems.BackColor = System.Drawing.Color.White;
        }

        public void ResetJsonFileTextBox()
        {
            this.ECTextApiJsonContent.Text = string.Empty;
            this.ECTextQuickStartsJsonContent.Text = string.Empty;
            this.ECTextSpecJsonContent.Text = string.Empty;
        }

        private void ResetAllExtensionConfigurationControls()
        {
            this.TabApiItemPage.SelectedTab = this.TabIcon;

            ResetAllFlagControl();
            ResetIconsPage();
            ResetApiPage();
            ResetQuickStartsPage();
            ResetSpecPages();
            ResetJsonFileTextBox();
            this.ECTextActionMessage.Text = "Cleared All Fields!";
        }

        #endregion

        private void TabApiItemPage_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (this.TabApiItemPage.SelectedTab == this.TabApi)
            {
                this.TabControlApi.SelectedTab = this.TabApiFields;
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
            else if (this.TabApiItemPage.SelectedTab == this.TabQuickStarts)
            {
                this.TabControlQuickStarts.SelectedTab = this.TabQuickStartFileds;
            }
            else if (this.TabApiItemPage.SelectedTab == this.TabSpec)
            {
                this.TabControlSpec.SelectedTab = this.TabSpecFields;
                string tempSelectedValue = this.ECCBSpecFeatureItemIconSvgData.Text;
                this.ECCBSpecFeatureItemIconSvgData.Items.Clear();

                if (this.ECCBSpecItemColorScheme.Items.Count > 0)
                {
                    this.ECCBSpecItemColorScheme.SelectedIndex = 0;
                }

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
        }

        #region Icon Operation

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
                    this.ECTextSvgFiles.BackColor = System.Drawing.Color.White;
                }
                else
                {
                    this.ECBtnIconsFlag.BackColor = System.Drawing.Color.Red;
                    this.ECTextSvgFiles.BackColor = System.Drawing.Color.Red;
                }
            }
        }

        #endregion

        #region Api Operation

        private void BtnApiSkuQuotaAdd_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.ECTextApiSkuQuotaCode.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "SkuQuota - Code"));
            }

            if (string.IsNullOrWhiteSpace(this.ECTextApiSkuQuotaName.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "SkuQuota - Name"));
            }

            if (string.IsNullOrWhiteSpace(this.ECTextApiSkuQuotaQuota.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "SkuQuota - Quoda"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
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

        private void ECBtnSaveApiInfo_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder ErrorMessage = new StringBuilder();
                ApiItemEntity entity = new ApiItemEntity();
                entity.categories = new List<string>();
                entity.skuQuota = new List<ApiSkuQuotaEntity>();
                List<ApiSkuQuotaEntity> apiSkuQuota = new List<ApiSkuQuotaEntity>();

                if (string.IsNullOrWhiteSpace(this.ECTextApiItem.Text))
                {
                    ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "ApiTypeName"));
                }

                if (string.IsNullOrWhiteSpace(this.ECTextApiTitle.Text))
                {
                    ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Title"));
                }

                if (string.IsNullOrWhiteSpace(this.ECTextApiSubTitle.Text))
                {
                    ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Description"));
                }

                //if (string.IsNullOrWhiteSpace(this.ECCBApiIconData.Text))
                //{
                //    ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Api Icon"));
                //}

                if (ErrorMessage.Length > 0)
                {
                    MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                    return;
                }
                else
                {
                    entity.item = this.ECTextApiItem.Text;
                    entity.title = this.ECTextApiTitle.Text;
                    entity.subtitle = this.ECTextApiSubTitle.Text;
                    entity.iconData = this.ECCBApiIconData.Text;

                    entity.categories = new List<string>();
                    entity.categories.Add("CognitiveServices");

                    if (!string.IsNullOrWhiteSpace(this.ECTextApiSkuQuotaData.Text))
                    {
                        try
                        {
                            string apiSkuQuotaData = this.ECTextApiSkuQuotaData.Text;
                            entity.skuQuota = JsonConvert.DeserializeObject<List<ApiSkuQuotaEntity>>(apiSkuQuotaData, settingFormat);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "SkuQuota", ex.Message), ExceptionTitle);
                            return;
                        }
                    }

                    entity.showLegalTerm = this.ECRDBApiShowLegalTermTrue.Checked;
                    if (!string.IsNullOrWhiteSpace(this.ECTextApiDefaultLegalTerm.Text))
                    {
                        entity.defaultLegalTerm = this.ECTextApiDefaultLegalTerm.Text;
                    }

                    string apiContent = JsonConvert.SerializeObject(entity, specialSettingFormat);
                    //ApiConfigurationManager.SaveContentStringToLocalFile(ApiOnBoardingConfigurationToolECTempConfigurationFolder, "api.json", apiContent);
                    this.ECTextApiJsonContent.Text = apiContent;

                    this.ECBtnApiFlag.BackColor = System.Drawing.Color.Green;
                    this.ECTextActionMessage.Text = string.Format(TemplateSavedSuccessfullyText, "Api");
                    this.TabControlApi.SelectedTab = this.TabApiJsonContent;
                }
            }
            catch (Exception ex)
            {
                this.ECTextActionMessage.Text = CommonOccuredErrorText1;
                MessageBox.Show(ex.Message, ExceptionTitle);
            }
        }

        #endregion

        #region QuickStarts Operation

        private void ECBtnAddLinkItem_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.ECTextQuickStartItemLinkText.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Links - Text"));
            }

            if (string.IsNullOrWhiteSpace(this.ECTextQuickStartItemLinkUri.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Links - Uri"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
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

        private void ECBtnAddQuickStartItem_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.ECTextQuickStartItemTitle.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Title"));
            }

            if (string.IsNullOrWhiteSpace(this.ECCBQuickStartItemIcon.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Icon"));
            }

            if (string.IsNullOrWhiteSpace(this.ECTextQuickStartItemDescription.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Description"));
            }

            if (string.IsNullOrWhiteSpace(this.ECTextQuickStartItemLinks.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Links"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
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

        private void ECBtnSaveQuickStartsInfo_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.ECTextQuickStartItems.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "QuickStartItems"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return;
            }

            QuickStartsEntity quickStartsEntity = new QuickStartsEntity();
            try
            {
                quickStartsEntity.quickStarts = JsonConvert.DeserializeObject<List<QuickStartUnit>>(this.ECTextQuickStartItems.Text, settingFormat);
            }
            catch
            {
                MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "QuickStartItems"), ExceptionTitle);
                return;
            }

            string quickStartsContent = JsonConvert.SerializeObject(quickStartsEntity, specialSettingFormat);
            //ApiConfigurationManager.SaveContentStringToLocalFile(ApiOnBoardingConfigurationToolECTempConfigurationFolder, "quickStarts.json", quickStartsContent);
            this.ECTextQuickStartsJsonContent.Text = quickStartsContent;

            this.ECTextActionMessage.Text = string.Format(TemplateSavedSuccessfullyText, "QuickStarts");
            this.ECBtnQuickStartsFlag.BackColor = System.Drawing.Color.Green;
            this.TabControlQuickStarts.SelectedTab = this.TabQuickStartJsonContent;
        }

        #endregion

        #region Spec Operation

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

        private void ECBtnAddSpecFeatureItem_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.ECTextSpecFeatureItemDisplayName.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Display Name"));
            }

            //if (string.IsNullOrWhiteSpace(this.ECCBSpecFeatureItemIconSvgData.Text) && string.IsNullOrWhiteSpace(this.ECTextSpecFeatureItemIconName.Text))
            //{
            //    ErrorMessage.Append(string.Format(TemplateShouldNotAllBeEmptyText, "Feature Icon", "Icon Name"));
            //}

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return;
            }

            SpecFeatureEntity specFeatureEntity = new SpecFeatureEntity();
            specFeatureEntity.displayName = this.ECTextSpecFeatureItemDisplayName.Text;
            specFeatureEntity.id = specFeatureEntity.displayName.Replace(" ", "");
            if (!string.IsNullOrWhiteSpace(this.ECCBSpecFeatureItemIconSvgData.Text))
            {
                specFeatureEntity.iconSvgData = this.ECCBSpecFeatureItemIconSvgData.Text;
            }
            else if (!string.IsNullOrWhiteSpace(this.ECTextSpecFeatureItemIconName.Text))
            {
                specFeatureEntity.iconName = this.ECTextSpecFeatureItemIconName.Text;
            }
            else
            {
                specFeatureEntity.iconSvgData = string.Empty;
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

        private void ECBtnAddPromotedFeature_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.ECTextSpecItemPromotedFeatureItemValue.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "PromotedFeatures - Value"));
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecItemPromotedFeatureItemUnitDescription.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "PromotedFeatures - unitDescription"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
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

        private void ECBtnAddFeatureID_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.ECCBSpecItemFeatureID.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Features - FeatureID"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
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

        private void ECBtnAddSpecItem_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.ECTextSpecItemSpecCode.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "SpecCode"));
            }

            if (string.IsNullOrWhiteSpace(this.ECCBSpecItemColorScheme.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "ColorScheme"));
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecItemTitle.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Title"));
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecItemPromotedFeatureItems.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "PromotedFeatures"));
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecItemFeatureIDs.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Features"));
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecItemCostCaption.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Cost - Caption"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
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

        private void ECBtnAddSpecDefaultItemFirstPartyItem_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.ECTextSpecDefaultItemFirstPartyResourceID.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "FistParty - ResourceID"));
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecDefaultItemFirstPartyQuantity.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "FistParty - Quantity"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
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

        private void ECBtnAddSpecDefaultItem_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.ECCBSpecDefaultItemSpecCode.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "SpecCode"));
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecDefaultItemFirstPartyItems.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "FistParty"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
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

        private void ECBtnAddSpecAllowZeroCostSpecCodeItem_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.ECCBSpecAllowZeroCostSpecCode.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "SpecCode"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
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

        private void ECBtnSaveSpecInfo_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.ECTextSpecFeatureItems.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "FeatureItems"));
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecItems.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "SpecItems"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return;
            }

            if (string.IsNullOrWhiteSpace(this.ECTextApiItem.Text))
            {
                MessageBox.Show("Please input the 'ApiTypeName'", AlertTitle);
                this.TabApiItemPage.SelectedTab = this.TabApi;
                this.TabControlApi.SelectedTab = this.TabApiFields;
                this.ECTextApiItem.Focus();
                return;
            }

            SpecsEntity specsEntity = new SpecsEntity();

            specsEntity.specType = string.Format("Microsoft.ProjectOxford/{0}", this.ECTextApiItem.Text);

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

            if (!string.IsNullOrWhiteSpace(this.ECTextSpecDefaultItems.Text))
            {
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
            }
            else
            {
                specsEntity.resourceMap = new SpecResourceMapEntity();
            }

            if (!string.IsNullOrWhiteSpace(this.ECTextSpecAllowZeroCostSpecCodeItems.Text))
            {
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
            }
            else
            {
                specsEntity.specsToAllowZeroCost = new List<string>();
            }

            string specContent = JsonConvert.SerializeObject(specsEntity, specialSettingFormat);
            //ApiConfigurationManager.SaveContentStringToLocalFile(ApiOnBoardingConfigurationToolECTempConfigurationFolder, "spec.json", specContent);
            this.ECTextSpecJsonContent.Text = specContent;

            this.ECBtnSpecFlag.BackColor = System.Drawing.Color.Green;
            this.ECTextActionMessage.Text = string.Format(TemplateSavedSuccessfullyText, "Spec");
            this.TabControlSpec.SelectedTab = this.TabSpecJsonContent;
        }

        #endregion

        #region Operation

        private void ECBtnSaveToLocal_Click(object sender, EventArgs e)
        {
            ApiConfigurationData configData = SeperateApiConfigurationInfoIntoResource();

            if (configData == null)
            {
                return;
            }

            configData.ApiFolderName = originalFileOrFolderName;

            if (this.ECRDBSaveAsFolder.Checked)
            {
                ApiConfigurationManager.SaveConfigurationDataToLocalFolder(configData, ApiOnBoardingConfigurationToolECSaveToLocalFolder);
            }
            else
            {
                ApiConfigurationManager.SaveConfigurationDataToLocalZip(configData, ApiOnBoardingConfigurationToolECSaveToLocalFolder);
            }

            this.ECTextActionMessage.Text = string.Format("Saved successfully!\r\nPath: {0}", ApiOnBoardingConfigurationToolECSaveToLocalFolder);
        }

        private void ECBtnUploadToPPEBlob_Click(object sender, EventArgs e)
        {
            try
            {
                ApiConfigurationData configData = SeperateApiConfigurationInfoIntoResource();
                if (configData == null)
                {
                    return;
                }

                configData.ApiFolderName = originalFileOrFolderName;

                string ApiCongigurationTestBlobAccountName = CloudConfigurationManager.GetSetting("ApiCongigurationTestBlobAccountName");
                string ApiConfigurationTestBlobAccountKey = CloudConfigurationManager.GetSetting("ApiConfigurationTestBlobAccountKey");

                if (string.IsNullOrWhiteSpace(ApiCongigurationTestBlobAccountName))
                {
                    MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiCongigurationTestBlobAccountName"), AlertTitle);
                    return;
                }
                if (string.IsNullOrWhiteSpace(ApiConfigurationTestBlobAccountKey))
                {
                    MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiConfigurationTestBlobAccountKey"), AlertTitle);
                    return;
                }

                StorageCredentials credentials = new StorageCredentials(ApiCongigurationTestBlobAccountName, ApiConfigurationTestBlobAccountKey, "AccountKey");
                BlobHelper.UploadApiConfigurationToBlobContainer(credentials, ECConfigurationStorageContainerName, configData, ApiOnBoardingConfigurationToolECTempFolder);

                this.ECTextActionMessage.Text = CommonUploadSuccessfullyText;
            }
            catch (Exception ex)
            {
                string ErrorMessage = string.Format("Original Error Message: {0}", ex.Message);
                MessageBox.Show(ErrorMessage, AlertTitle);
            }
        }

        #endregion


        private void LoadApiItemList()
        {
            this.ECListItems.Items.Clear();

            foreach (var item in ListCachedExtensionConfigurationData)
            {
                if (item != null)
                {
                    this.ECListItems.Items.Add(item.ApiFolderName);
                }
            }
        }

        private void LoadApiConfigInfoToPage(ApiConfigurationData apiConfigInfo)
        {
            ResetAllExtensionConfigurationControls();
            ApiConfigurationData configData = RestoreApiConfigurationInfoFromResource(apiConfigInfo);
            if (configData.Icons.Count > 0)
            {
                this.ApiConfigIcons = configData.Icons;

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
                if (configData.ApiItem != null)
                {
                    SetControlContentText(this.ECTextApiItem, configData.ApiItem.item, this.ECBtnApiFlag);
                    SetControlContentText(this.ECTextApiTitle, configData.ApiItem.title, this.ECBtnApiFlag);
                    SetControlContentText(this.ECTextApiSubTitle, configData.ApiItem.subtitle, this.ECBtnApiFlag);
                    if (this.ApiConfigIcons.ContainsKey(configData.ApiItem.iconData))
                    {
                        SetControlContentText(this.ECCBApiIconData, configData.ApiItem.iconData, this.ECBtnApiFlag);
                    }
                    else
                    {
                        SetControlContentText(this.ECCBApiIconData, null, this.ECBtnApiFlag);
                    }

                    if (configData.ApiItem.skuQuota.Count > 0)
                    {
                        SetControlContentText(this.ECTextApiSkuQuotaData, JsonConvert.SerializeObject(configData.ApiItem.skuQuota, settingFormat), this.ECBtnApiFlag, false);
                    }

                    this.ECRDBApiShowLegalTermTrue.Checked = configData.ApiItem.showLegalTerm;
                    this.ECRDBApiShowLegalTermFalse.Checked = !configData.ApiItem.showLegalTerm;

                    SetControlContentText(this.ECTextApiDefaultLegalTerm, configData.ApiItem.defaultLegalTerm, this.ECBtnApiFlag, false);
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
                if (configData.QuickStart != null)
                {
                    SetControlContentText(this.ECTextQuickStartItems, JsonConvert.SerializeObject(configData.QuickStart.quickStarts, settingFormat), this.ECBtnApiFlag);
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
                if (configData.Spec != null)
                {
                    this.ECCBSpecItemFeatureID.Items.Add(string.Empty);
                    foreach (SpecFeatureEntity feature in configData.Spec.features)
                    {
                        this.ECCBSpecItemFeatureID.Items.Add(feature.id);
                    }

                    this.ECCBSpecDefaultItemSpecCode.Items.Add(string.Empty);
                    this.ECCBSpecAllowZeroCostSpecCode.Items.Add(string.Empty);
                    foreach (SpecUnitEntity specItem in configData.Spec.specs)
                    {
                        this.ECCBSpecDefaultItemSpecCode.Items.Add(specItem.specCode);
                        this.ECCBSpecAllowZeroCostSpecCode.Items.Add(specItem.specCode);
                    }

                    SetControlContentText(this.ECTextSpecItems, JsonConvert.SerializeObject(configData.Spec.specs, settingFormat), this.ECBtnSpecFlag);
                    SetControlContentText(this.ECTextSpecFeatureItems, JsonConvert.SerializeObject(configData.Spec.features, settingFormat), this.ECBtnSpecFlag);
                    if (configData.Spec.resourceMap.specResourceMapDefault != null)
                    {
                        SetControlContentText(this.ECTextSpecDefaultItems, JsonConvert.SerializeObject(configData.Spec.resourceMap.specResourceMapDefault, settingFormat), this.ECBtnSpecFlag);
                    }

                    SetControlContentText(this.ECTextSpecAllowZeroCostSpecCodeItems, JsonConvert.SerializeObject(configData.Spec.specsToAllowZeroCost, settingFormat), this.ECBtnSpecFlag);
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

        private ApiConfigurationData SeperateApiConfigurationInfoIntoResource()
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.ECTextApiJsonContent.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Api.Json Content"));
            }

            if (string.IsNullOrWhiteSpace(this.ECTextQuickStartsJsonContent.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "QuickStarts.Json Content"));
            }

            if (string.IsNullOrWhiteSpace(this.ECTextSpecJsonContent.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Spec.Json Content"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return null;
            }

            ApiConfigurationData configData = new ApiConfigurationData();

            try
            {
                configData.ApiItem = JsonConvert.DeserializeObject<ApiItemEntity>(this.ECTextApiJsonContent.Text, settingFormat);
                configData.ApiTypeName = configData.ApiItem.item;
            }
            catch
            {
                string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "Api.Json Content");
                MessageBox.Show(exceptionMessage, ExceptionTitle);
                return null;
            }

            try
            {
                configData.QuickStart = JsonConvert.DeserializeObject<QuickStartsEntity>(this.ECTextQuickStartsJsonContent.Text, settingFormat);
            }
            catch
            {
                string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "QuickStarts.Json Content");
                MessageBox.Show(exceptionMessage, ExceptionTitle);
                return null;
            }

            try
            {
                configData.Spec = JsonConvert.DeserializeObject<SpecsEntity>(this.ECTextSpecJsonContent.Text, settingFormat);
            }
            catch
            {
                string exceptionMessage = string.Format(TemplateCannotBeDeserializedText, "Spec.Json Content");
                MessageBox.Show(exceptionMessage, ExceptionTitle);
                return null;
            }

            configData.Icons = this.ApiConfigIcons;

            Dictionary<string, string> resource = new Dictionary<string, string>();

            resource.Add("title", configData.ApiItem.title);
            configData.ApiItem.title = string.Format("{0}{1}", ResourceToken, "title");

            resource.Add("subtitle", configData.ApiItem.subtitle);
            configData.ApiItem.subtitle = string.Format("{0}{1}", ResourceToken, "subtitle");

            if (!string.IsNullOrWhiteSpace(configData.ApiItem.iconData))
            {
                configData.ApiItem.iconData = string.Format("{0}{1}", IconToken, configData.ApiItem.iconData);
            }

            if (!string.IsNullOrWhiteSpace(configData.ApiItem.defaultLegalTerm))
            {
                resource.Add("defaultLegalTerm", configData.ApiItem.defaultLegalTerm);
                configData.ApiItem.defaultLegalTerm = string.Format("{0}{1}", ResourceToken, "defaultLegalTerm");
            }


            for (int i = 0; i < configData.QuickStart.quickStarts.Count; i++)
            {
                string tempQuickStartItemTitle = string.Format("quickStart{0}.title", i);
                resource.Add(tempQuickStartItemTitle, configData.QuickStart.quickStarts[i].title);
                configData.QuickStart.quickStarts[i].title = string.Format("{0}{1}", ResourceToken, tempQuickStartItemTitle);

                string tempQuickStartItemDescription = string.Format("quickStart{0}.des", i);
                resource.Add(tempQuickStartItemDescription, configData.QuickStart.quickStarts[i].description);
                configData.QuickStart.quickStarts[i].description = string.Format("{0}{1}", ResourceToken, tempQuickStartItemDescription);

                for (int j = 0; j < configData.QuickStart.quickStarts[i].links.Count; j++)
                {
                    string tempQuickStartItemLinkItemText = string.Format("quickStart{0}.link{1}.text", i, j);
                    resource.Add(tempQuickStartItemLinkItemText, configData.QuickStart.quickStarts[i].links[j].text);
                    configData.QuickStart.quickStarts[i].links[j].text = string.Format("{0}{1}", ResourceToken, tempQuickStartItemLinkItemText);
                }
            }

            for (int i = 0; i < configData.Spec.features.Count; i++)
            {
                string tempSpecFeatureDisplayName = string.Format("feature.{0}.displayName", configData.Spec.features[i].id);
                resource.Add(tempSpecFeatureDisplayName, configData.Spec.features[i].displayName);
                configData.Spec.features[i].displayName = string.Format("{0}{1}", ResourceToken, tempSpecFeatureDisplayName);

                if (!string.IsNullOrWhiteSpace(configData.Spec.features[i].iconSvgData))
                {
                    configData.Spec.features[i].iconSvgData = string.Format("{0}{1}", IconToken, configData.Spec.features[i].iconSvgData);
                }
            }

            for (int i = 0; i < configData.Spec.specs.Count; i++)
            {
                string tempSpecItemTitle = string.Format("spec.{0}.title", configData.Spec.specs[i].id);
                resource.Add(tempSpecItemTitle, configData.Spec.specs[i].title);
                configData.Spec.specs[i].title = string.Format("{0}{1}", ResourceToken, tempSpecItemTitle);

                for (int j = 0; j < configData.Spec.specs[i].promotedFeatures.Count; j++)
                {
                    SpecPromotedFeature promotedFeature = configData.Spec.specs[i].promotedFeatures[j];
                    string tempUnitDescription = string.Format("spec.promotedFeature.unitDescription{0}", j);
                    if (resource.ContainsValue(promotedFeature.unitDescription))
                    {
                        tempUnitDescription = resource.FirstOrDefault(d => d.Value == promotedFeature.unitDescription).Key;
                        configData.Spec.specs[i].promotedFeatures[j].unitDescription = string.Format("{0}{1}", ResourceToken, tempUnitDescription);
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
                                configData.Spec.specs[i].promotedFeatures[j].unitDescription = string.Format("{0}{1}", ResourceToken, tempUnitDescription);
                                break;
                            }

                            tempIndex++;
                        }
                    }
                }

                string tempSpecItemCostCaption = string.Format("spec.cost.{0}.caption.format", configData.Spec.specs[i].id);
                resource.Add(tempSpecItemCostCaption, configData.Spec.specs[i].cost.caption);
                configData.Spec.specs[i].cost.caption = string.Format("{0}{1}", ResourceToken, tempSpecItemCostCaption);
            }

            configData.Resources.Clear();
            configData.Resources.Add("en", resource);
            return configData;
        }

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
                configInfo.ApiItem = new ApiItemEntity();
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
                            feature.iconSvgData = apiConfigInfo.Spec.features[i].iconSvgData.Replace(IconToken, "");
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

        private void SCPPEBtnLoadFromPPE_Click(object sender, EventArgs e)
        {
            var apiConfigurationPublicAccessUrl = CloudConfigurationManager.GetSetting("ApiConfigurationPublicAccessUrl");
            if (string.IsNullOrWhiteSpace(apiConfigurationPublicAccessUrl))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiConfigurationPublicAccessUrl"), AlertTitle);
                return;
            }

            this.SCPPEBtnLoadFromPPE.Enabled = false;

            Uri uri = new Uri(apiConfigurationPublicAccessUrl);
            CloudBlobClient blobClient = new CloudBlobClient(uri);
            CloudBlobContainer container = blobClient.GetContainerReference(SCConfigurationStorageContainerName);
            var files = container.ListBlobs();
            var requestOption = new BlobRequestOptions { RetryPolicy = new ExponentialRetry() };

            ListCachedSkuConfigurationData.Clear();
            this.SCPPEListItems.Items.Clear();
            foreach (var file in files.OfType<CloudBlockBlob>())
            {
                using (var blobStream = file.OpenRead(null, requestOption))
                {
                    using (StreamReader reader = new StreamReader(blobStream))
                    {
                        ListCachedSkuConfigurationData.Add(file.Name.Replace(".json", ""), reader.ReadToEnd());
                    }
                }

                this.SCPPEListItems.Items.Add(file.Name.Replace(".json", ""));
            }

            this.SCPPETextActionMessage.Text = "Loaded!";
            this.SCPPEBtnLoadFromPPE.Enabled = true;
        }

        private void SCPPEBtnLoadFromLocal_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(this.OpenFileDialog.FileName);
                if (!file.Extension.ToLower().Equals(".json"))
                {
                    MessageBox.Show("Only support Json file!", AlertTitle);
                    return;
                }
                else
                {
                    SkuConfigEntity skuConfigEntity = null;
                    string content = string.Empty;
                    using (Stream stream = file.OpenRead())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            content = reader.ReadToEnd();
                        }
                    }

                    try
                    {
                        skuConfigEntity = JsonConvert.DeserializeObject<SkuConfigEntity>(content, specialSettingFormat);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("The selected item can not be Deserialized!\r\nException:\r\n    {0}", ex.Message), AlertTitle);
                        return;
                    }

                    this.SCTextPPEName.Text = skuConfigEntity.name;
                    this.SCTextPPEDisplayName.Text = skuConfigEntity.displayName;
                    this.SCTextPPEApimInstance.Text = skuConfigEntity.apimInstance;
                    this.SCTextPPEApiPath.Text = skuConfigEntity.apiPath;
                    this.SCTextPPESkuItems.Text = JsonConvert.SerializeObject(skuConfigEntity.skus, specialSettingFormat);
                    this.SCTextPPEJsonContent.Text = content;
                    this.TabControlSkuPPE.SelectedTab = this.TabSkuPPEJsonContent;
                    this.SCPPETextActionMessage.Text = "Loaded!";
                }
            }
        }

        private void SCPPEListItems_DoubleClick(object sender, EventArgs e)
        {
            foreach (var skuConfigData in ListCachedSkuConfigurationData)
            {
                if (this.SCPPEListItems.SelectedItem.ToString() == skuConfigData.Key)
                {
                    SkuConfigEntity skuConfigEntity = null;
                    try
                    {
                        skuConfigEntity = JsonConvert.DeserializeObject<SkuConfigEntity>(skuConfigData.Value, specialSettingFormat);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("The selected item can not be Deserialized!\r\nException:\r\n    {0}", ex.Message), AlertTitle);
                        return;
                    }

                    this.SCTextPPEName.Text = skuConfigEntity.name;
                    this.SCTextPPEDisplayName.Text = skuConfigEntity.displayName;
                    this.SCTextPPEApimInstance.Text = skuConfigEntity.apimInstance;
                    this.SCTextPPEApiPath.Text = skuConfigEntity.apiPath;
                    this.SCTextPPESkuItems.Text = JsonConvert.SerializeObject(skuConfigEntity.skus, specialSettingFormat);
                    this.SCTextPPEJsonContent.Text = skuConfigData.Value;
                    this.TabControlSkuPPE.SelectedTab = this.TabSkuPPEJsonContent;
                    this.SCPPETextActionMessage.Text = "Loaded!";
                    break;
                }
            }
        }

        private void SCPPEBtnDeleteFromPPEBlob_Click(object sender, EventArgs e)
        {
            if (this.SCPPEListItems.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select item to delete!", AlertTitle);
                return;
            }

            string ApiCongigurationPPEBlobAccountName = CloudConfigurationManager.GetSetting("ApiCongigurationTestBlobAccountName");
            string ApiConfigurationPPEBlobAccountKey = CloudConfigurationManager.GetSetting("ApiConfigurationTestBlobAccountKey");

            if (string.IsNullOrWhiteSpace(ApiCongigurationPPEBlobAccountName))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiCongigurationTestBlobAccountName"), AlertTitle);
                return;
            }
            if (string.IsNullOrWhiteSpace(ApiConfigurationPPEBlobAccountKey))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiConfigurationTestBlobAccountKey"), AlertTitle);
                return;
            }

            if (MessageBox.Show("Confirm to delete these items from PPE Blob?", AlertTitle, MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            StorageCredentials credentials = new StorageCredentials(ApiCongigurationPPEBlobAccountName, ApiConfigurationPPEBlobAccountKey, "AccountKey");

            List<string> selectedItems = new List<string>();
            foreach (var item in this.SCPPEListItems.CheckedItems)
            {
                selectedItems.Add(item.ToString());
                this.SCPPEListItems.Items.Remove(item);
            }

            BlobHelper.DeleteApiConfigurationListFromBlobContainer(credentials, SCConfigurationStorageContainerName, selectedItems, "json");
            this.SCPPETextActionMessage.Text = "Deleted these items!";
        }

        private void SCBtnPPEAddLocationItem_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();

            if (string.IsNullOrWhiteSpace(this.SCTextPPESkuItemLocationItemLocation.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Location"));
            }

            if (string.IsNullOrWhiteSpace(this.SCTextPPESkuItemApimProductID.Text) && string.IsNullOrWhiteSpace(this.SCTextPPESkuItemLocationItemApimProductID.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotAllBeEmptyText, "ApimProductID", "Locations.ApimProductID"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
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

        private void SCBtnAddPPESkuItemMeterID_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();

            if (string.IsNullOrWhiteSpace(this.SCTextPPESkuItemMeterID.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "MeterID"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
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

        private void SCBtnPPEAddSkuItemRequiredFeature_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();

            if (string.IsNullOrWhiteSpace(this.SCTextPPESkuItemRequiredFeature.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Feature"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
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

        private void SCBtnPPEAddSkuItem_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();

            if (string.IsNullOrWhiteSpace(this.SCTextPPESkuItemName.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Name"));
            }

            if (string.IsNullOrWhiteSpace(this.SCTextPPESkuItemTier.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Tier"));
            }

            if (string.IsNullOrWhiteSpace(this.SCTextPPESkuItemLocations.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Locations"));
            }

            if (string.IsNullOrWhiteSpace(this.SCTextPPESkuItemMeterIDs.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "MeterIDs"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
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

        private void SCPPEBtnSave_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.SCTextPPEName.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Name"));
            }

            if (string.IsNullOrWhiteSpace(this.SCTextPPEDisplayName.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "DisplayName"));
            }

            if (string.IsNullOrWhiteSpace(this.SCTextPPEApimInstance.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "ApimInstance"));
            }

            if (string.IsNullOrWhiteSpace(this.SCTextPPEApiPath.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "ApiPath"));
            }

            if (string.IsNullOrWhiteSpace(this.SCTextPPESkuItems.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "SkuItems"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return;
            }

            SkuConfigEntity skuConfigEntity = new SkuConfigEntity();
            skuConfigEntity.name = this.SCTextPPEName.Text;
            skuConfigEntity.displayName = this.SCTextPPEDisplayName.Text;
            skuConfigEntity.apimInstance = this.SCTextPPEApimInstance.Text;
            skuConfigEntity.apiPath = this.SCTextPPEApiPath.Text;

            try
            {
                skuConfigEntity.skus = JsonConvert.DeserializeObject<List<SkuEntity>>(this.SCTextPPESkuItems.Text, specialSettingFormat);
            }
            catch
            {
                MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "SkuItems"));
                return;
            }

            this.SCTextPPEJsonContent.Text = JsonConvert.SerializeObject(skuConfigEntity, specialSettingFormat);
            this.TabControlSkuPPE.SelectedTab = this.TabSkuPPEJsonContent;
            this.SCPPETextActionMessage.Text = "Saved successfully!";
        }

        private void SCBtnUploadToPPE_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm to upload to PPE Blob?", AlertTitle, MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.SCTextPPEJsonContent.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "JsonContent"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return;
            }

            SkuConfigEntity skuConfigEntity = new SkuConfigEntity();
            try
            {
                skuConfigEntity = JsonConvert.DeserializeObject<SkuConfigEntity>(this.SCTextPPEJsonContent.Text, specialSettingFormat);
            }
            catch
            {
                MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "JsonContent"));
                return;
            }

            string ApiCongigurationPPEBlobAccountName = CloudConfigurationManager.GetSetting("ApiCongigurationTestBlobAccountName");
            string ApiConfigurationPPEBlobAccountKey = CloudConfigurationManager.GetSetting("ApiConfigurationTestBlobAccountKey");

            if (string.IsNullOrWhiteSpace(ApiCongigurationPPEBlobAccountName))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiCongigurationTestBlobAccountName"), AlertTitle);
                return;
            }
            if (string.IsNullOrWhiteSpace(ApiConfigurationPPEBlobAccountKey))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiConfigurationTestBlobAccountKey"), AlertTitle);
                return;
            }

            StorageCredentials credentials = new StorageCredentials(ApiCongigurationPPEBlobAccountName, ApiConfigurationPPEBlobAccountKey, "AccountKey");
            string tempFileName = string.Format("{0}.json", skuConfigEntity.name.Replace(".", ""));
            string jsonContent = JsonConvert.SerializeObject(skuConfigEntity, specialSettingFormat);
            byte[] array = Encoding.ASCII.GetBytes(jsonContent);

            using (MemoryStream stream = new MemoryStream(array))
            {
                BlobHelper.UploadConfifuratuinToBlobContainer(credentials, SCConfigurationStorageContainerName, tempFileName, stream);
            }

            this.SCPPETextActionMessage.Text = "Upload successfully!";
        }

        #endregion

        #region Production

        private void SCProBtnLoadFromPPE_Click(object sender, EventArgs e)
        {
            var apiConfigurationPublicAccessUrl = CloudConfigurationManager.GetSetting("ApiConfigurationPublicAccessUrl");
            if (string.IsNullOrWhiteSpace(apiConfigurationPublicAccessUrl))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiConfigurationPublicAccessUrl"), AlertTitle);
                return;
            }

            this.SCProBtnLoadFromPPE.Enabled = false;

            Uri uri = new Uri(apiConfigurationPublicAccessUrl);
            CloudBlobClient blobClient = new CloudBlobClient(uri);
            CloudBlobContainer container = blobClient.GetContainerReference(SCConfigurationStorageContainerName);
            var files = container.ListBlobs();
            var requestOption = new BlobRequestOptions { RetryPolicy = new ExponentialRetry() };

            ListCachedSkuConfigurationData.Clear();
            this.SCProListItems.Items.Clear();
            foreach (var file in files.OfType<CloudBlockBlob>())
            {
                using (var blobStream = file.OpenRead(null, requestOption))
                {
                    using (StreamReader reader = new StreamReader(blobStream))
                    {
                        ListCachedSkuConfigurationData.Add(file.Name.Replace(".json", ""), reader.ReadToEnd());
                    }
                }

                this.SCProListItems.Items.Add(file.Name.Replace(".json", ""));
            }

            this.SCPPETextActionMessage.Text = "Loaded!";
            this.SCProBtnLoadFromPPE.Enabled = true;
        }

        private void SCProBtnLoadFromLocal_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(this.OpenFileDialog.FileName);
                if (!file.Extension.ToLower().Equals(".json"))
                {
                    MessageBox.Show("Only support Json file!", AlertTitle);
                    return;
                }
                else
                {
                    SkuConfigEntity skuConfigEntity = null;
                    string content = string.Empty;
                    using (Stream stream = file.OpenRead())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            content = reader.ReadToEnd();
                        }
                    }

                    try
                    {
                        skuConfigEntity = JsonConvert.DeserializeObject<SkuConfigEntity>(content, specialSettingFormat);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("The selected item can not be Deserialized!\r\nException:\r\n    {0}", ex.Message), AlertTitle);
                        return;
                    }

                    this.SCTextProName.Text = skuConfigEntity.name;
                    this.SCTextProDisplayName.Text = skuConfigEntity.displayName;
                    this.SCTextProApimInstance.Text = skuConfigEntity.apimInstance;
                    this.SCTextProApiPath.Text = skuConfigEntity.apiPath;
                    this.SCTextProSkuItems.Text = JsonConvert.SerializeObject(skuConfigEntity.skus, specialSettingFormat);
                    this.SCTextProJsonContent.Text = content;
                    this.TabControlSkuPro.SelectedTab = this.TabSkuProJsonContent;
                    this.SCProTextActionMessage.Text = "Loaded!";
                }
            }
        }

        private void SCProListItems_DoubleClick(object sender, EventArgs e)
        {
            foreach (var skuConfigData in ListCachedSkuConfigurationData)
            {
                if (this.SCProListItems.SelectedItem.ToString() == skuConfigData.Key)
                {
                    SkuConfigEntity skuConfigEntity = null;
                    try
                    {
                        skuConfigEntity = JsonConvert.DeserializeObject<SkuConfigEntity>(skuConfigData.Value, specialSettingFormat);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("The selected item can not be Deserialized!\r\nException:\r\n    {0}", ex.Message), AlertTitle);
                        return;
                    }

                    this.SCTextProName.Text = skuConfigEntity.name;
                    this.SCTextProDisplayName.Text = skuConfigEntity.displayName;
                    this.SCTextProApimInstance.Text = skuConfigEntity.apimInstance;
                    this.SCTextProApiPath.Text = skuConfigEntity.apiPath;
                    this.SCTextProSkuItems.Text = JsonConvert.SerializeObject(skuConfigEntity.skus, specialSettingFormat);
                    this.SCTextProJsonContent.Text = skuConfigData.Value;
                    this.TabControlSkuPro.SelectedTab = this.TabSkuProJsonContent;
                    this.SCPPETextActionMessage.Text = "Loaded!";
                    break;
                }
            }
        }

        private void SCProBtnDeleteFromPPEBlob_Click(object sender, EventArgs e)
        {
            if (this.SCProListItems.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select item to delete!", AlertTitle);
                return;
            }

            string ApiCongigurationPPEBlobAccountName = CloudConfigurationManager.GetSetting("ApiCongigurationTestBlobAccountName");
            string ApiConfigurationPPEBlobAccountKey = CloudConfigurationManager.GetSetting("ApiConfigurationTestBlobAccountKey");

            if (string.IsNullOrWhiteSpace(ApiCongigurationPPEBlobAccountName))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiCongigurationTestBlobAccountName"), AlertTitle);
                return;
            }
            if (string.IsNullOrWhiteSpace(ApiConfigurationPPEBlobAccountKey))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiConfigurationTestBlobAccountKey"), AlertTitle);
                return;
            }

            if (MessageBox.Show("Confirm to delete these items from PPE Blob?", AlertTitle, MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            StorageCredentials credentials = new StorageCredentials(ApiCongigurationPPEBlobAccountName, ApiConfigurationPPEBlobAccountKey, "AccountKey");

            List<string> selectedItems = new List<string>();
            foreach (var item in this.SCProListItems.CheckedItems)
            {
                selectedItems.Add(item.ToString());
                this.SCProListItems.Items.Remove(item);
            }

            BlobHelper.DeleteApiConfigurationListFromBlobContainer(credentials, SCConfigurationStorageContainerName, selectedItems, "json");
            this.SCPPETextActionMessage.Text = "Deleted these items!";
        }

        private void SCBtnProAddLocationItem_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();

            if (string.IsNullOrWhiteSpace(this.SCTextProSkuItemLocationItemLocation.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Location")); ;
            }

            if (string.IsNullOrWhiteSpace(this.SCTextProSkuItemApimProductID.Text) && string.IsNullOrWhiteSpace(this.SCTextProSkuItemLocationItemApimProductID.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotAllBeEmptyText, "ApimProductID", "Locations.ApimProductID"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
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

        private void SCBtnAddProSkuItemMeterID_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();

            if (string.IsNullOrWhiteSpace(this.SCTextProSkuItemMeterID.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "MeterID")); ;
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
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

        private void SCBtnProAddSkuItemRequiredFeature_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();

            if (string.IsNullOrWhiteSpace(this.SCTextProSkuItemRequiredFeature.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Feature")); ;
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
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

        private void SCBtnProAddSkuItem_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();

            if (string.IsNullOrWhiteSpace(this.SCTextProSkuItemName.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Name")); ;
            }

            if (string.IsNullOrWhiteSpace(this.SCTextProSkuItemTier.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Tier")); ;
            }

            if (string.IsNullOrWhiteSpace(this.SCTextProSkuItemLocations.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Locations")); ;
            }

            if (string.IsNullOrWhiteSpace(this.SCTextProSkuItemMeterIDs.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "MeterIDs")); ;
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
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

        private void SCProBtnSave_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.SCTextProName.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Name"));
            }

            if (string.IsNullOrWhiteSpace(this.SCTextProDisplayName.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "DisplayName"));
            }

            if (string.IsNullOrWhiteSpace(this.SCTextProApimInstance.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "ApimInstance"));
            }

            if (string.IsNullOrWhiteSpace(this.SCTextProApiPath.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "ApiPath"));
            }

            if (string.IsNullOrWhiteSpace(this.SCTextProSkuItems.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "SkuItems"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return;
            }

            SkuConfigEntity skuConfigEntity = new SkuConfigEntity();
            skuConfigEntity.name = this.SCTextProName.Text;
            skuConfigEntity.displayName = this.SCTextProDisplayName.Text;
            skuConfigEntity.apimInstance = this.SCTextProApimInstance.Text;
            skuConfigEntity.apiPath = this.SCTextProApiPath.Text;

            try
            {
                skuConfigEntity.skus = JsonConvert.DeserializeObject<List<SkuEntity>>(this.SCTextProSkuItems.Text, specialSettingFormat);
            }
            catch
            {
                MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "SkuItems"));
                return;
            }

            this.SCTextProJsonContent.Text = JsonConvert.SerializeObject(skuConfigEntity, specialSettingFormat);
            this.TabControlSkuPro.SelectedTab = this.TabSkuProJsonContent;
            this.SCProTextActionMessage.Text = "Saved successfully!";
        }

        private void SCProBtnUploadToProductionBlob_Click(object sender, EventArgs e)
        {
            string ApiCongigurationProBlobAccountName = this.SCProTextStorageAccount.Text;
            string ApiConfigurationProBlobAccountKey = this.SCProTextStorageAccountKey.Text;
            StringBuilder ErrorMessage = new StringBuilder();

            if (string.IsNullOrWhiteSpace(ApiCongigurationProBlobAccountName))
            {
                ErrorMessage.Append(CommonInputStorageAccountAlertText);
            }

            if (string.IsNullOrWhiteSpace(ApiConfigurationProBlobAccountKey))
            {
                ErrorMessage.Append(CommonInputStorageAccountKeyAlertText);
            }

            if (string.IsNullOrWhiteSpace(this.SCTextProJsonContent.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "JsonContent"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return;
            }

            SkuConfigEntity skuConfigEntity = new SkuConfigEntity();
            try
            {
                skuConfigEntity = JsonConvert.DeserializeObject<SkuConfigEntity>(this.SCTextProJsonContent.Text, specialSettingFormat);
            }
            catch
            {
                MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "JsonContent"));
                return;
            }

            if (MessageBox.Show("Confirm to upload to PPE Blob?", AlertTitle, MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            StorageCredentials credentials = new StorageCredentials(ApiCongigurationProBlobAccountName, ApiConfigurationProBlobAccountKey, "AccountKey");
            string tempFileName = string.Format("{0}.json", skuConfigEntity.name.Replace(".", ""));
            string jsonContent = JsonConvert.SerializeObject(skuConfigEntity, specialSettingFormat);
            byte[] array = Encoding.ASCII.GetBytes(jsonContent);

            using (MemoryStream stream = new MemoryStream(array))
            {
                BlobHelper.UploadConfifuratuinToBlobContainer(credentials, SCConfigurationStorageContainerName, tempFileName, stream);
            }

            this.SCProTextActionMessage.Text = "Upload successfully!";
        }

        #endregion

        #endregion

        #region Meter Configuration

        #region PPE

        private void MCPPEBtnLoadFromPPE_Click(object sender, EventArgs e)
        {
            var apiConfigurationPublicAccessUrl = CloudConfigurationManager.GetSetting("ApiConfigurationPublicAccessUrl");
            if (string.IsNullOrWhiteSpace(apiConfigurationPublicAccessUrl))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiConfigurationPublicAccessUrl"), AlertTitle);
                return;
            }

            this.MCPPEBtnLoadFromPPE.Enabled = false;

            Uri uri = new Uri(apiConfigurationPublicAccessUrl);
            CloudBlobClient blobClient = new CloudBlobClient(uri);
            CloudBlobContainer container = blobClient.GetContainerReference(MCConfigurationStorageContainerName);
            var files = container.ListBlobs();
            var requestOption = new BlobRequestOptions { RetryPolicy = new ExponentialRetry() };

            ListCachedMeterConfigurationData.Clear();
            this.MCPPEListItems.Items.Clear();
            foreach (var file in files.OfType<CloudBlockBlob>())
            {
                using (var blobStream = file.OpenRead(null, requestOption))
                {
                    using (StreamReader reader = new StreamReader(blobStream))
                    {
                        ListCachedMeterConfigurationData.Add(file.Name.Replace(".json", ""), reader.ReadToEnd());
                    }
                }

                this.MCPPEListItems.Items.Add(file.Name.Replace(".json", ""));
            }

            this.MCPPETextActionMessage.Text = "Loaded!";
            this.MCPPEBtnLoadFromPPE.Enabled = true;
        }

        private void MCPPEBtnLoadFromLocal_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(this.OpenFileDialog.FileName);
                if (!file.Extension.ToLower().Equals(".json"))
                {
                    MessageBox.Show("Only support Json file!", AlertTitle);
                    return;
                }
                else
                {
                    List<MeterConfigEntity> meterConfigEntities = null;
                    string content = string.Empty;
                    using (Stream stream = file.OpenRead())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            content = reader.ReadToEnd();
                        }
                    }

                    try
                    {
                        meterConfigEntities = JsonConvert.DeserializeObject<List<MeterConfigEntity>>(content, specialSettingFormat);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("The selected item can not be Deserialized!\r\nException:\r\n    {0}", ex.Message), AlertTitle);
                        return;
                    }

                    this.MCPPETextFileName.Text = file.Name.Replace(file.Extension, "");
                    this.MCTextPPEJsonContent.Text = content;
                    this.TabControlMeterPPE.SelectedTab = this.TabMeterPPEJsonContent;
                    this.MCPPETextActionMessage.Text = "Loaded!";
                }
            }
        }

        private void MCPPEListItems_DoubleClick(object sender, EventArgs e)
        {
            foreach (var meterConfigData in ListCachedMeterConfigurationData)
            {
                if (this.MCPPEListItems.SelectedItem.ToString() == meterConfigData.Key)
                {
                    List<MeterConfigEntity> meterConfigEntities = null;
                    try
                    {
                        meterConfigEntities = JsonConvert.DeserializeObject<List<MeterConfigEntity>>(meterConfigData.Value, specialSettingFormat);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("The selected item can not be Deserialized!\r\nException:\r\n    {0}", ex.Message), AlertTitle);
                        return;
                    }

                    this.MCPPETextFileName.Text = this.MCPPEListItems.SelectedItem.ToString();
                    this.MCTextPPEJsonContent.Text = meterConfigData.Value;
                    this.TabControlMeterPPE.SelectedTab = this.TabMeterPPEJsonContent;
                    this.MCPPETextActionMessage.Text = "Loaded!";
                    break;
                }
            }
        }

        private void MCPPEBtnDeleteFromPPEBlob_Click(object sender, EventArgs e)
        {
            if (this.MCPPEListItems.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select item to delete!", AlertTitle);
                return;
            }

            string ApiCongigurationPPEBlobAccountName = CloudConfigurationManager.GetSetting("ApiCongigurationTestBlobAccountName");
            string ApiConfigurationPPEBlobAccountKey = CloudConfigurationManager.GetSetting("ApiConfigurationTestBlobAccountKey");

            if (string.IsNullOrWhiteSpace(ApiCongigurationPPEBlobAccountName))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiCongigurationTestBlobAccountName"), AlertTitle);
                return;
            }
            if (string.IsNullOrWhiteSpace(ApiConfigurationPPEBlobAccountKey))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiConfigurationTestBlobAccountKey"), AlertTitle);
                return;
            }

            if (MessageBox.Show("Confirm to delete these items from PPE Blob?", AlertTitle, MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            StorageCredentials credentials = new StorageCredentials(ApiCongigurationPPEBlobAccountName, ApiConfigurationPPEBlobAccountKey, "AccountKey");

            List<string> selectedItems = new List<string>();
            foreach (var item in this.MCPPEListItems.CheckedItems)
            {
                selectedItems.Add(item.ToString());
                this.MCPPEListItems.Items.Remove(item);
            }

            BlobHelper.DeleteApiConfigurationListFromBlobContainer(credentials, MCConfigurationStorageContainerName, selectedItems, "json");
            this.MCPPETextActionMessage.Text = "Deleted these items!";
        }

        private void MCBtnPPEAddOperation_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.MCTextPPEApiItemOperationItem.Text))
            {
                ErrorMessage.AppendLine(string.Format(TemplateShouldNotBeEmptyText, "Operation"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return;
            }

            List<string> operations = new List<string>();
            if (!string.IsNullOrWhiteSpace(this.MCTextPPEApiItemOperations.Text))
            {
                try
                {
                    operations = JsonConvert.DeserializeObject<List<string>>(this.MCTextPPEApiItemOperations.Text, settingFormat);
                }
                catch
                {
                    MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "Operations"), AlertTitle);
                    return;
                }
            }

            operations.Add(this.MCTextPPEApiItemOperationItem.Text);
            this.MCTextPPEApiItemOperations.Text = JsonConvert.SerializeObject(operations);
            this.MCTextPPEApiItemOperationItem.Text = string.Empty;
        }

        private void MCBtnPPEAddApiItem_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.MCTextPPEApiItemApiID.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "ApiID"));
            }

            if (string.IsNullOrWhiteSpace(this.MCTextPPEApiItemOperations.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Operations"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return;
            }

            ApiEntity apiEntity = new ApiEntity();
            apiEntity.apiId = this.MCTextPPEApiItemApiID.Text;
            apiEntity.operations = new List<string>();
            try
            {
                apiEntity.operations = JsonConvert.DeserializeObject<List<string>>(this.MCTextPPEApiItemOperations.Text, settingFormat);
            }
            catch
            {
                MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "Operations"), AlertTitle);
                return;
            }

            List<ApiEntity> apis = new List<ApiEntity>();
            if (!string.IsNullOrWhiteSpace(this.MCTextPPEApiItems.Text))
            {
                try
                {
                    apis = JsonConvert.DeserializeObject<List<ApiEntity>>(this.MCTextPPEApiItems.Text, specialSettingFormat);
                }
                catch
                {
                    MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "ApimMeterDefinition.Apis"), AlertTitle);
                    return;
                }
            }

            apis.Add(apiEntity);
            this.MCTextPPEApiItems.Text = JsonConvert.SerializeObject(apis, specialSettingFormat);
            this.MCTextPPEApiItemApiID.Text = string.Empty;
            this.MCTextPPEApiItemOperations.Text = string.Empty;
        }

        private void MCBtnPPEAddMeterItem_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.MCTextPPEID.Text))
            {
                ErrorMessage.AppendLine(string.Format(TemplateShouldNotBeEmptyText, "ID"));
            }

            if (string.IsNullOrWhiteSpace(this.MCTextPPECommerceMeterID.Text))
            {
                ErrorMessage.AppendLine(string.Format(TemplateShouldNotBeEmptyText, "CommerceMeterID"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return;
            }

            MeterConfigEntity meterConfigEntity = new MeterConfigEntity();
            meterConfigEntity.id = this.MCTextPPEID.Text;
            meterConfigEntity.commerceMeterId = this.MCTextPPECommerceMeterID.Text;

            if (!string.IsNullOrWhiteSpace(this.MCTextPPECallCountUnit.Text))
            {
                meterConfigEntity.callcountPerUnit = this.MCTextPPECallCountUnit.Text;
            }

            if (!string.IsNullOrWhiteSpace(this.MCTextPPEType.Text))
            {
                meterConfigEntity.Type = this.MCTextPPEType.Text;
            }

            if (!string.IsNullOrWhiteSpace(this.MCTextPPEUOM.Text))
            {
                meterConfigEntity.UOM = this.MCTextPPEUOM.Text;
            }

            if (!string.IsNullOrWhiteSpace(this.MCTextPPECadence.Text))
            {
                meterConfigEntity.Cadence = this.MCTextPPECadence.Text;
            }

            meterConfigEntity.apimMeterDefinition = new ApimMeterDefinitionEntity();

            if (!string.IsNullOrWhiteSpace(this.MCTextPPEApiItems.Text))
            {
                try
                {
                    meterConfigEntity.apimMeterDefinition.apis = JsonConvert.DeserializeObject<List<ApiEntity>>(this.MCTextPPEApiItems.Text, specialSettingFormat);
                }
                catch
                {
                    MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "ApimMeterDefinition.Apis"), AlertTitle);
                    return;
                }
            }

            List<MeterConfigEntity> meterConfigEntities = new List<MeterConfigEntity>();
            if (!string.IsNullOrWhiteSpace(this.MCTextPPEMeterItems.Text))
            {
                try
                {
                    meterConfigEntities = JsonConvert.DeserializeObject<List<MeterConfigEntity>>(this.MCTextPPEMeterItems.Text, specialSettingFormat);
                }
                catch
                {
                    MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "MeterItems"), AlertTitle);
                    return;
                }
            }

            meterConfigEntities.Add(meterConfigEntity);
            this.MCTextPPEMeterItems.Text = JsonConvert.SerializeObject(meterConfigEntities, specialSettingFormat);
            this.MCTextPPEID.Text = string.Empty;
            this.MCTextPPECommerceMeterID.Text = string.Empty;
            this.MCTextPPEType.Text = string.Empty;
            this.MCTextPPECallCountUnit.Text = string.Empty;
            this.MCTextPPEUOM.Text = string.Empty;
            this.MCTextPPECadence.Text = string.Empty;
            this.MCTextPPEApiItems.Text = string.Empty;
        }

        private void MCPPEBtnSave_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.MCTextPPEMeterItems.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "MeterItems"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return;
            }


            List<MeterConfigEntity> meterConfigEntities = new List<MeterConfigEntity>();
            try
            {
                meterConfigEntities = JsonConvert.DeserializeObject<List<MeterConfigEntity>>(this.MCTextPPEMeterItems.Text, specialSettingFormat);
            }
            catch
            {
                MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "MeterItems"), AlertTitle);
                return;
            }

            this.MCTextPPEJsonContent.Text = JsonConvert.SerializeObject(meterConfigEntities, specialSettingFormat);
            this.TabControlMeterPPE.SelectedTab = this.TabMeterPPEJsonContent;
            this.MCPPETextActionMessage.Text = "Saved successfully!";
        }

        private void MCBtnUploadToPPE_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.MCPPETextFileName.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "FileName"));
            }

            if (string.IsNullOrWhiteSpace(this.MCTextPPEJsonContent.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "JsonContent"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return;
            }

            List<MeterConfigEntity> meterConfigEntities = new List<MeterConfigEntity>();
            try
            {
                meterConfigEntities = JsonConvert.DeserializeObject<List<MeterConfigEntity>>(this.MCTextPPEJsonContent.Text, specialSettingFormat);
            }
            catch
            {
                MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "JsonContent"), AlertTitle);
                return;
            }

            string ApiCongigurationPPEBlobAccountName = CloudConfigurationManager.GetSetting("ApiCongigurationTestBlobAccountName");
            string ApiConfigurationPPEBlobAccountKey = CloudConfigurationManager.GetSetting("ApiConfigurationTestBlobAccountKey");

            if (string.IsNullOrWhiteSpace(ApiCongigurationPPEBlobAccountName))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiCongigurationTestBlobAccountName"), AlertTitle);
                return;
            }
            if (string.IsNullOrWhiteSpace(ApiConfigurationPPEBlobAccountKey))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiConfigurationTestBlobAccountKey"), AlertTitle);
                return;
            }

            if (MessageBox.Show("Confirm to upload to PPE Blob?", AlertTitle, MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            StorageCredentials credentials = new StorageCredentials(ApiCongigurationPPEBlobAccountName, ApiConfigurationPPEBlobAccountKey, "AccountKey");
            string tempFileName = string.Format("{0}.json", this.MCPPETextFileName.Text);
            string jsonContent = JsonConvert.SerializeObject(meterConfigEntities, specialSettingFormat);
            byte[] array = Encoding.ASCII.GetBytes(jsonContent);

            using (MemoryStream stream = new MemoryStream(array))
            {
                BlobHelper.UploadConfifuratuinToBlobContainer(credentials, MCConfigurationStorageContainerName, tempFileName, stream);
            }

            this.MCPPETextActionMessage.Text = "Upload successfully!";
        }

        #endregion

        #region Production

        private void MCProBtnLoadFromPPE_Click(object sender, EventArgs e)
        {
            var apiConfigurationPublicAccessUrl = CloudConfigurationManager.GetSetting("ApiConfigurationPublicAccessUrl");
            if (string.IsNullOrWhiteSpace(apiConfigurationPublicAccessUrl))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiConfigurationPublicAccessUrl"), AlertTitle);
                return;
            }

            this.MCProBtnLoadFromPPE.Enabled = false;

            Uri uri = new Uri(apiConfigurationPublicAccessUrl);
            CloudBlobClient blobClient = new CloudBlobClient(uri);
            CloudBlobContainer container = blobClient.GetContainerReference(MCConfigurationStorageContainerName);
            var files = container.ListBlobs();
            var requestOption = new BlobRequestOptions { RetryPolicy = new ExponentialRetry() };

            ListCachedMeterConfigurationData.Clear();
            this.MCProListItems.Items.Clear();
            foreach (var file in files.OfType<CloudBlockBlob>())
            {
                using (var blobStream = file.OpenRead(null, requestOption))
                {
                    using (StreamReader reader = new StreamReader(blobStream))
                    {
                        ListCachedMeterConfigurationData.Add(file.Name.Replace(".json", ""), reader.ReadToEnd());
                    }
                }

                this.MCProListItems.Items.Add(file.Name.Replace(".json", ""));
            }

            this.MCProTextActionMessage.Text = "Loaded!";
            this.MCProBtnLoadFromPPE.Enabled = true;
        }

        private void MCProBtnLoadFromLocal_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(this.OpenFileDialog.FileName);
                if (!file.Extension.ToLower().Equals(".json"))
                {
                    MessageBox.Show("Only support Json file!", AlertTitle);
                    return;
                }
                else
                {
                    List<MeterConfigEntity> meterConfigEntities = null;
                    string content = string.Empty;
                    using (Stream stream = file.OpenRead())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            content = reader.ReadToEnd();
                        }
                    }

                    try
                    {
                        meterConfigEntities = JsonConvert.DeserializeObject<List<MeterConfigEntity>>(content, specialSettingFormat);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("The selected item can not be Deserialized!\r\nException:\r\n    {0}", ex.Message), AlertTitle);
                        return;
                    }

                    this.MCProTextFileName.Text = file.Name.Replace(file.Extension, "");
                    this.MCTextProJsonContent.Text = content;
                    this.TabControlMeterPro.SelectedTab = this.TabMeterProJsonContent;
                    this.MCProTextActionMessage.Text = "Loaded!";
                }
            }
        }

        private void MCProListItems_DoubleClick(object sender, EventArgs e)
        {
            foreach (var meterConfigData in ListCachedMeterConfigurationData)
            {
                if (this.MCProListItems.SelectedItem.ToString() == meterConfigData.Key)
                {
                    List<MeterConfigEntity> meterConfigEntities = null;
                    try
                    {
                        meterConfigEntities = JsonConvert.DeserializeObject<List<MeterConfigEntity>>(meterConfigData.Value, specialSettingFormat);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("The selected item can not be Deserialized!\r\nException:\r\n    {0}", ex.Message), AlertTitle);
                        return;
                    }

                    this.MCProTextFileName.Text = this.MCProListItems.SelectedItem.ToString();
                    this.MCTextProJsonContent.Text = meterConfigData.Value;
                    this.TabControlMeterPro.SelectedTab = this.TabMeterProJsonContent;
                    this.MCProTextActionMessage.Text = "Loaded!";
                    break;
                }
            }
        }

        private void MCProBtnDeleteFromPPEBlob_Click(object sender, EventArgs e)
        {
            if (this.MCProListItems.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select item to delete!", AlertTitle);
                return;
            }

            string ApiCongigurationPPEBlobAccountName = CloudConfigurationManager.GetSetting("ApiCongigurationTestBlobAccountName");
            string ApiConfigurationPPEBlobAccountKey = CloudConfigurationManager.GetSetting("ApiConfigurationTestBlobAccountKey");

            if (string.IsNullOrWhiteSpace(ApiCongigurationPPEBlobAccountName))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiCongigurationTestBlobAccountName"), AlertTitle);
                return;
            }
            if (string.IsNullOrWhiteSpace(ApiConfigurationPPEBlobAccountKey))
            {
                MessageBox.Show(string.Format(TemplateConnotFindConfigKeyValue, "ApiConfigurationTestBlobAccountKey"), AlertTitle);
                return;
            }

            if (MessageBox.Show("Confirm to delete these items from PPE Blob?", AlertTitle, MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            StorageCredentials credentials = new StorageCredentials(ApiCongigurationPPEBlobAccountName, ApiConfigurationPPEBlobAccountKey, "AccountKey");

            List<string> selectedItems = new List<string>();
            foreach (var item in this.MCProListItems.CheckedItems)
            {
                selectedItems.Add(item.ToString());
                this.MCProListItems.Items.Remove(item);
            }

            BlobHelper.DeleteApiConfigurationListFromBlobContainer(credentials, MCConfigurationStorageContainerName, selectedItems, "json");
            this.MCProTextActionMessage.Text = "Deleted these items!";
        }

        private void MCBtnProAddOperation_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.MCTextProApiItemOperationItem.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Operation"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return;
            }

            List<string> operations = new List<string>();
            if (!string.IsNullOrWhiteSpace(this.MCTextProApiItemOperations.Text))
            {
                try
                {
                    operations = JsonConvert.DeserializeObject<List<string>>(this.MCTextProApiItemOperations.Text, settingFormat);
                }
                catch
                {
                    MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "Operations"), AlertTitle);
                    return;
                }
            }

            operations.Add(this.MCTextProApiItemOperationItem.Text);
            this.MCTextProApiItemOperations.Text = JsonConvert.SerializeObject(operations);
            this.MCTextProApiItemOperationItem.Text = string.Empty;
        }

        private void MCBtnProAddApiItem_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.MCTextProApiItemApiID.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "ApiID"));
            }

            if (string.IsNullOrWhiteSpace(this.MCTextProApiItemOperations.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "Operations"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return;
            }

            ApiEntity apiEntity = new ApiEntity();
            apiEntity.apiId = this.MCTextProApiItemApiID.Text;
            apiEntity.operations = new List<string>();
            try
            {
                apiEntity.operations = JsonConvert.DeserializeObject<List<string>>(this.MCTextProApiItemOperations.Text, settingFormat);
            }
            catch
            {
                MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "Operations"), AlertTitle);
                return;
            }

            List<ApiEntity> apis = new List<ApiEntity>();
            if (!string.IsNullOrWhiteSpace(this.MCTextProApiItems.Text))
            {
                try
                {
                    apis = JsonConvert.DeserializeObject<List<ApiEntity>>(this.MCTextProApiItems.Text, specialSettingFormat);
                }
                catch
                {
                    MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "ApimMeterDefinition.Apis"), AlertTitle);
                    return;
                }
            }

            apis.Add(apiEntity);
            this.MCTextProApiItems.Text = JsonConvert.SerializeObject(apis, specialSettingFormat);
            this.MCTextProApiItemApiID.Text = string.Empty;
            this.MCTextProApiItemOperations.Text = string.Empty;
        }

        private void MCBtnProAddMeterItem_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.MCTextProID.Text))
            {
                ErrorMessage.AppendLine(string.Format(TemplateShouldNotBeEmptyText, "ID"));
            }

            if (string.IsNullOrWhiteSpace(this.MCTextProCommerceMeterID.Text))
            {
                ErrorMessage.AppendLine(string.Format(TemplateShouldNotBeEmptyText, "CommerceMeterID"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return;
            }

            MeterConfigEntity meterConfigEntity = new MeterConfigEntity();
            meterConfigEntity.id = this.MCTextProID.Text;
            meterConfigEntity.commerceMeterId = this.MCTextProCommerceMeterID.Text;

            if (!string.IsNullOrWhiteSpace(this.MCTextProCallCountUnit.Text))
            {
                meterConfigEntity.callcountPerUnit = this.MCTextProCallCountUnit.Text;
            }

            if (!string.IsNullOrWhiteSpace(this.MCTextProType.Text))
            {
                meterConfigEntity.Type = this.MCTextProType.Text;
            }

            if (!string.IsNullOrWhiteSpace(this.MCTextProUOM.Text))
            {
                meterConfigEntity.UOM = this.MCTextProUOM.Text;
            }

            if (!string.IsNullOrWhiteSpace(this.MCTextProCadence.Text))
            {
                meterConfigEntity.Cadence = this.MCTextProCadence.Text;
            }

            meterConfigEntity.apimMeterDefinition = new ApimMeterDefinitionEntity();

            if (!string.IsNullOrWhiteSpace(this.MCTextProApiItems.Text))
            {
                try
                {
                    meterConfigEntity.apimMeterDefinition.apis = JsonConvert.DeserializeObject<List<ApiEntity>>(this.MCTextProApiItems.Text, specialSettingFormat);
                }
                catch
                {
                    MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "ApimMeterDefinition.Apis"), AlertTitle);
                    return;
                }
            }

            List<MeterConfigEntity> meterConfigEntities = new List<MeterConfigEntity>();
            if (!string.IsNullOrWhiteSpace(this.MCTextProMeterItems.Text))
            {
                try
                {
                    meterConfigEntities = JsonConvert.DeserializeObject<List<MeterConfigEntity>>(this.MCTextProMeterItems.Text, specialSettingFormat);
                }
                catch
                {
                    MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "MeterItems"), AlertTitle);
                    return;
                }
            }

            meterConfigEntities.Add(meterConfigEntity);
            this.MCTextProMeterItems.Text = JsonConvert.SerializeObject(meterConfigEntities, specialSettingFormat);
            this.MCTextProID.Text = string.Empty;
            this.MCTextProCommerceMeterID.Text = string.Empty;
            this.MCTextProType.Text = string.Empty;
            this.MCTextProCallCountUnit.Text = string.Empty;
            this.MCTextProUOM.Text = string.Empty;
            this.MCTextProCadence.Text = string.Empty;
            this.MCTextProApiItems.Text = string.Empty;
        }

        private void MCProBtnSave_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.MCTextProMeterItems.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "MeterItems"));
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return;
            }


            List<MeterConfigEntity> meterConfigEntities = new List<MeterConfigEntity>();
            try
            {
                meterConfigEntities = JsonConvert.DeserializeObject<List<MeterConfigEntity>>(this.MCTextProMeterItems.Text, specialSettingFormat);
            }
            catch
            {
                MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "MeterItems"), AlertTitle);
                return;
            }

            this.MCTextProJsonContent.Text = JsonConvert.SerializeObject(meterConfigEntities, specialSettingFormat);
            this.TabControlMeterPro.SelectedTab = this.TabMeterProJsonContent;
            this.MCProTextActionMessage.Text = "Saved successfully!";
        }

        private void MCProBtnUploadToProductionBlob_Click(object sender, EventArgs e)
        {
            string ApiCongigurationProBlobAccountName = this.MCProTextStorageAccount.Text;
            string ApiConfigurationProBlobAccountKey = this.MCProTextStorageAccountKey.Text;
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(this.MCTextProJsonContent.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "JsonContent"));
            }

            if (string.IsNullOrWhiteSpace(this.MCProTextFileName.Text))
            {
                ErrorMessage.Append(string.Format(TemplateShouldNotBeEmptyText, "FileName"));
            }

            if (string.IsNullOrWhiteSpace(ApiCongigurationProBlobAccountName))
            {
                ErrorMessage.Append(CommonInputStorageAccountAlertText);
            }

            if (string.IsNullOrWhiteSpace(ApiConfigurationProBlobAccountKey))
            {
                ErrorMessage.Append(CommonInputStorageAccountKeyAlertText);
            }

            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString(), AlertTitle);
                return;
            }

            List<MeterConfigEntity> meterConfigEntities = new List<MeterConfigEntity>();
            try
            {
                meterConfigEntities = JsonConvert.DeserializeObject<List<MeterConfigEntity>>(this.MCTextProJsonContent.Text, specialSettingFormat);
            }
            catch
            {
                MessageBox.Show(string.Format(TemplateCannotBeDeserializedText, "JsonContent"));
                return;
            }

            if (MessageBox.Show("Confirm to upload to PPE Blob?", AlertTitle, MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            StorageCredentials credentials = new StorageCredentials(ApiCongigurationProBlobAccountName, ApiConfigurationProBlobAccountKey, "AccountKey");
            string tempFileName = string.Format("{0}.json", this.MCProTextFileName.Text);
            string jsonContent = JsonConvert.SerializeObject(meterConfigEntities, specialSettingFormat);
            byte[] array = Encoding.ASCII.GetBytes(jsonContent);

            using (MemoryStream stream = new MemoryStream(array))
            {
                BlobHelper.UploadConfifuratuinToBlobContainer(credentials, MCConfigurationStorageContainerName, tempFileName, stream);
            }

            this.MCProTextActionMessage.Text = "Upload successfully!";
        }

        #endregion

        #endregion

        private void TextBoxSelectAll_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                ((TextBox)sender).SelectAll();
            }
        }

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

        private void LogError(string content)
        {
            try
            {
                if (!Directory.Exists(ApiOnBoardingConfigurationToolRootFolder))
                {
                    Directory.CreateDirectory(ApiOnBoardingConfigurationToolRootFolder);
                }

                string logFileName = string.Format("ErrorLog  {0}.txt", DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss"));
                string logFilePath = string.Format(@"{0}\{1}", ApiOnBoardingConfigurationToolRootFolder, logFileName);
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