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
            this.button1 = new System.Windows.Forms.Button();
            this.textErrorMessage = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnLoadData = new System.Windows.Forms.Button();
            this.btnUpLoadData = new System.Windows.Forms.Button();
            this.textFileContent = new System.Windows.Forms.TextBox();
            this.root = new System.Windows.Forms.TreeView();
            this.textConnectStr = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileName
            // 
            this.fileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileName.Location = new System.Drawing.Point(6, 6);
            this.fileName.Name = "fileName";
            this.fileName.Size = new System.Drawing.Size(848, 26);
            this.fileName.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(860, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 28);
            this.button1.TabIndex = 1;
            this.button1.Text = "Select";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textErrorMessage
            // 
            this.textErrorMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textErrorMessage.Location = new System.Drawing.Point(6, 40);
            this.textErrorMessage.Multiline = true;
            this.textErrorMessage.Name = "textErrorMessage";
            this.textErrorMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textErrorMessage.Size = new System.Drawing.Size(940, 425);
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
            this.tabPage1.Controls.Add(this.fileName);
            this.tabPage1.Controls.Add(this.textErrorMessage);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(952, 471);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "  Validation  ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnLoadData);
            this.tabPage2.Controls.Add(this.btnUpLoadData);
            this.tabPage2.Controls.Add(this.textFileContent);
            this.tabPage2.Controls.Add(this.root);
            this.tabPage2.Controls.Add(this.textConnectStr);
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
            // btnLoadData
            // 
            this.btnLoadData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadData.Location = new System.Drawing.Point(6, 40);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(163, 30);
            this.btnLoadData.TabIndex = 4;
            this.btnLoadData.Text = "Load From Blob";
            this.btnLoadData.UseVisualStyleBackColor = true;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // btnUpLoadData
            // 
            this.btnUpLoadData.Enabled = false;
            this.btnUpLoadData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpLoadData.ForeColor = System.Drawing.Color.Black;
            this.btnUpLoadData.Location = new System.Drawing.Point(860, 6);
            this.btnUpLoadData.Name = "btnUpLoadData";
            this.btnUpLoadData.Size = new System.Drawing.Size(86, 28);
            this.btnUpLoadData.TabIndex = 3;
            this.btnUpLoadData.Text = "Upload";
            this.btnUpLoadData.UseVisualStyleBackColor = true;
            // 
            // textFileContent
            // 
            this.textFileContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textFileContent.Location = new System.Drawing.Point(175, 40);
            this.textFileContent.Multiline = true;
            this.textFileContent.Name = "textFileContent";
            this.textFileContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textFileContent.Size = new System.Drawing.Size(771, 425);
            this.textFileContent.TabIndex = 2;
            // 
            // root
            // 
            this.root.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.root.ForeColor = System.Drawing.Color.Coral;
            this.root.Location = new System.Drawing.Point(6, 76);
            this.root.Name = "root";
            this.root.Size = new System.Drawing.Size(163, 389);
            this.root.TabIndex = 1;
            this.root.DoubleClick += new System.EventHandler(this.root_DoubleClick);
            // 
            // textConnectStr
            // 
            this.textConnectStr.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textConnectStr.Location = new System.Drawing.Point(6, 6);
            this.textConnectStr.Name = "textConnectStr";
            this.textConnectStr.Size = new System.Drawing.Size(848, 26);
            this.textConnectStr.TabIndex = 0;
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
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox fileName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textErrorMessage;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnUpLoadData;
        private System.Windows.Forms.TextBox textFileContent;
        private System.Windows.Forms.TreeView root;
        private System.Windows.Forms.TextBox textConnectStr;
        private System.Windows.Forms.Button btnLoadData;
    }
}

