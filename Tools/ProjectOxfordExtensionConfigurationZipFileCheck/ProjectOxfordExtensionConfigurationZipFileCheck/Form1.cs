using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Azure;

namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// The API configuration manager
        /// </summary>
        public static ApiConfigurationManager apiConfigurationManager = new ApiConfigurationManager();

        /// <summary>
        /// The API configuration storage container
        /// </summary>
        public static string apiConfigurationStorageContainer;

        private JsonSerializerSettings settingFormat = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button1_Click(object sender, EventArgs e)
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
            this.textErrorMessage.Text = "";
            this.fileName.Text = this.openFileDialog1.FileName;
            FileInfo fileInfo = new FileInfo(this.openFileDialog1.FileName);
            Stream stream = fileInfo.OpenRead();
            apiConfigurationManager.VeriliadteStream(stream, this.openFileDialog1.FileName);

            List<ErrorEntity> listError = apiConfigurationManager.listError;
            if (listError.Count > 0)
            {
                foreach (ErrorEntity errorEntity in listError)
                {
                    this.textErrorMessage.Text += errorEntity.GetErrorInfo() + "\r\n";
                }
            }
            else
            {
                this.textErrorMessage.Text = "This ZIP file is Correct!";
            }
            apiConfigurationManager.listError = new List<ErrorEntity>();
        }

        /// <summary>
        /// Handles the Click event of the button3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnLoadData_Click(object sender, EventArgs e)
        {
            this.btnUpLoadData.Enabled = false;
            apiConfigurationStorageContainer = CloudConfigurationManager.GetSetting("ApiConfigurationContainer");
            var apiConfigurationPublicAccessUrl = CloudConfigurationManager.GetSetting("ApiConfigurationPublicAccessUrl");

            apiConfigurationManager = new ApiConfigurationManager(apiConfigurationPublicAccessUrl, apiConfigurationStorageContainer);

            apiConfigurationManager.LoadDataToCache();
            List<ErrorEntity> listError = apiConfigurationManager.listError;

            if (listError.Count > 0)
            {
                foreach (ErrorEntity errorMessage in listError)
                {
                    this.textFileContent.Text += errorMessage.GetErrorInfo() + "\n";
                }
            }
            else
            {
                this.btnUpLoadData.Enabled = true;
                LoadFileTree(apiConfigurationManager.CacheData);
            }
        }

        private void LoadFileTree(List<ApiConfigurationData> cacheData)
        {
            if (cacheData.Count == 0)
            {
                return;
            }

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

                this.root.Nodes.Add(apiNode);
            }
        }

        private void root_DoubleClick(object sender, EventArgs e)
        {
            if (this.root.SelectedNode != null)
            {
                if (this.root.SelectedNode.Tag != null)
                {
                    string selectedNodeTag = this.root.SelectedNode.Tag.ToString();
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
                                    this.textFileContent.Text = JsonConvert.SerializeObject(tempData.ApiItem, settingFormat);
                                    break;
                                case "spec.json":
                                    this.textFileContent.Text = JsonConvert.SerializeObject(tempData.Spec, settingFormat);
                                    break;
                                case "quickStarts.json":
                                    this.textFileContent.Text = JsonConvert.SerializeObject(tempData.QuickStart, settingFormat);
                                    break;

                            }
                        }
                        if (pathArr.Length == 3)
                        {
                            if (pathArr[1] == "icons")
                            {
                                this.textFileContent.Text = tempData.Icons[pathArr[2]];
                            }
                            if (pathArr[1] == "strings")
                            {
                                this.textFileContent.Text = JsonConvert.SerializeObject(tempData.Resources[pathArr[2]], settingFormat);
                            }
                        }
                    }
                }
            }
        }
    }
}
