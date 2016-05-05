namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fileName = new System.Windows.Forms.TextBox();
            this.btnSelectZip = new System.Windows.Forms.Button();
            this.textErrorMessage = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnUploadSingleData = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textFileContent = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnLoadData = new System.Windows.Forms.Button();
            this.root = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textAccount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textAccountKey = new System.Windows.Forms.TextBox();
            this.btnUpLoadMultiZip = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileName
            // 
            this.fileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileName.Location = new System.Drawing.Point(77, 6);
            this.fileName.Name = "fileName";
            this.fileName.Size = new System.Drawing.Size(696, 26);
            this.fileName.TabIndex = 0;
            // 
            // btnSelectZip
            // 
            this.btnSelectZip.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectZip.Location = new System.Drawing.Point(779, 6);
            this.btnSelectZip.Name = "btnSelectZip";
            this.btnSelectZip.Size = new System.Drawing.Size(86, 28);
            this.btnSelectZip.TabIndex = 1;
            this.btnSelectZip.Text = "Select";
            this.btnSelectZip.UseVisualStyleBackColor = true;
            this.btnSelectZip.Click += new System.EventHandler(this.btnSelectZip_Click);
            // 
            // textErrorMessage
            // 
            this.textErrorMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textErrorMessage.Location = new System.Drawing.Point(6, 25);
            this.textErrorMessage.Multiline = true;
            this.textErrorMessage.Name = "textErrorMessage";
            this.textErrorMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textErrorMessage.Size = new System.Drawing.Size(928, 396);
            this.textErrorMessage.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(960, 500);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnUploadSingleData);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.fileName);
            this.tabPage1.Controls.Add(this.btnSelectZip);
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(952, 471);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "  Validation  ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnUploadSingleData
            // 
            this.btnUploadSingleData.Enabled = false;
            this.btnUploadSingleData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadSingleData.Location = new System.Drawing.Point(871, 6);
            this.btnUploadSingleData.Name = "btnUploadSingleData";
            this.btnUploadSingleData.Size = new System.Drawing.Size(75, 28);
            this.btnUploadSingleData.TabIndex = 4;
            this.btnUploadSingleData.Text = "Upload";
            this.btnUploadSingleData.UseVisualStyleBackColor = true;
            this.btnUploadSingleData.Click += new System.EventHandler(this.btnUploadSingleData_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "FilePath:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textErrorMessage);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(6, 38);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(940, 427);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Message";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage2.ForeColor = System.Drawing.Color.Black;
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(952, 471);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "  Upload  ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textFileContent);
            this.groupBox4.Location = new System.Drawing.Point(191, 112);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(755, 353);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Message/Content";
            // 
            // textFileContent
            // 
            this.textFileContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textFileContent.Location = new System.Drawing.Point(6, 25);
            this.textFileContent.Multiline = true;
            this.textFileContent.Name = "textFileContent";
            this.textFileContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textFileContent.Size = new System.Drawing.Size(743, 318);
            this.textFileContent.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnLoadData);
            this.groupBox2.Controls.Add(this.root);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(6, 112);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(179, 353);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Check and Review";
            // 
            // btnLoadData
            // 
            this.btnLoadData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadData.Location = new System.Drawing.Point(6, 25);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(167, 30);
            this.btnLoadData.TabIndex = 4;
            this.btnLoadData.Text = "Load From Blob";
            this.btnLoadData.UseVisualStyleBackColor = true;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // root
            // 
            this.root.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.root.ForeColor = System.Drawing.Color.Coral;
            this.root.Location = new System.Drawing.Point(6, 61);
            this.root.Name = "root";
            this.root.Size = new System.Drawing.Size(167, 282);
            this.root.TabIndex = 1;
            this.root.DoubleClick += new System.EventHandler(this.root_DoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textAccount);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textAccountKey);
            this.groupBox1.Controls.Add(this.btnUpLoadMultiZip);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(940, 89);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Azure Blob Storage";
            // 
            // textAccount
            // 
            this.textAccount.Location = new System.Drawing.Point(149, 20);
            this.textAccount.Name = "textAccount";
            this.textAccount.Size = new System.Drawing.Size(693, 26);
            this.textAccount.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "StorageAccount:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "StorageAccountKey:";
            // 
            // textAccountKey
            // 
            this.textAccountKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textAccountKey.Location = new System.Drawing.Point(149, 52);
            this.textAccountKey.Name = "textAccountKey";
            this.textAccountKey.Size = new System.Drawing.Size(785, 26);
            this.textAccountKey.TabIndex = 0;
            // 
            // btnUpLoadMultiZip
            // 
            this.btnUpLoadMultiZip.Enabled = false;
            this.btnUpLoadMultiZip.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpLoadMultiZip.ForeColor = System.Drawing.Color.Black;
            this.btnUpLoadMultiZip.Location = new System.Drawing.Point(848, 20);
            this.btnUpLoadMultiZip.Name = "btnUpLoadMultiZip";
            this.btnUpLoadMultiZip.Size = new System.Drawing.Size(86, 28);
            this.btnUpLoadMultiZip.TabIndex = 3;
            this.btnUpLoadMultiZip.Text = "Upload";
            this.btnUpLoadMultiZip.UseVisualStyleBackColor = true;
            this.btnUpLoadMultiZip.Click += new System.EventHandler(this.btnUpLoadMultiZip_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(984, 524);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "ProjectOxfordExtensionConfigurationCheckTool";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox fileName;
        private System.Windows.Forms.Button btnSelectZip;
        private System.Windows.Forms.TextBox textErrorMessage;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnUpLoadMultiZip;
        private System.Windows.Forms.TextBox textFileContent;
        private System.Windows.Forms.TreeView root;
        private System.Windows.Forms.TextBox textAccountKey;
        private System.Windows.Forms.Button btnLoadData;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textAccount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnUploadSingleData;
    }
}

