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
            this.fileName.Text = this.openFileDialog1.FileName;
            FileInfo fileInfo = new FileInfo(this.openFileDialog1.FileName);
            Stream stream = fileInfo.OpenRead();
            List<string> listError = apiConfigurationManager.VeriliadteStream(stream, this.openFileDialog1.FileName);
            if (listError.Count > 0)
            {
                foreach (string errorMessage in listError)
                {
                    this.textErrorMessage.Text += errorMessage + "\r\n";
                }
            }
            else
            {
                this.textErrorMessage.Text = "This ZIP file is Correct!";
            }
        }

        /// <summary>
        /// Handles the Click event of the button3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button3_Click(object sender, EventArgs e)
        {
            apiConfigurationStorageContainer = CloudConfigurationManager.GetSetting("ApiConfigurationContainer");
            var apiConfigurationPublicAccessUrl = CloudConfigurationManager.GetSetting("ApiConfigurationPublicAccessUrl");

            apiConfigurationManager = new ApiConfigurationManager(apiConfigurationPublicAccessUrl, apiConfigurationStorageContainer);

            List<string> listError = apiConfigurationManager.LoadDataToCache();

            foreach (string errorMessage in listError)
            {
                this.textFileContent.Text += errorMessage + "\n";
            }

        }
    }
}
