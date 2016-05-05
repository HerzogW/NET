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
            this.textFilePath1 = new System.Windows.Forms.TextBox();
            this.btnSelectZip1 = new System.Windows.Forms.Button();
            this.textMessage1 = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnUploadData1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.textMessage2 = new System.Windows.Forms.TextBox();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.btnLoadData2 = new System.Windows.Forms.Button();
            this.root2 = new System.Windows.Forms.TreeView();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.textAccount2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textAccountKey2 = new System.Windows.Forms.TextBox();
            this.btnUpLoadZip2 = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnSelectZip3 = new System.Windows.Forms.Button();
            this.btnSave3 = new System.Windows.Forms.Button();
            this.btnUploadData3 = new System.Windows.Forms.Button();
            this.textFilePath3 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox31 = new System.Windows.Forms.GroupBox();
            this.textMessage3 = new System.Windows.Forms.TextBox();
            this.openFileDialog3 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox23.SuspendLayout();
            this.groupBox22.SuspendLayout();
            this.groupBox21.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox31.SuspendLayout();
            this.SuspendLayout();
            // 
            // textFilePath1
            // 
            this.textFilePath1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textFilePath1.Location = new System.Drawing.Point(78, 6);
            this.textFilePath1.Name = "textFilePath1";
            this.textFilePath1.Size = new System.Drawing.Size(695, 26);
            this.textFilePath1.TabIndex = 0;
            // 
            // btnSelectZip1
            // 
            this.btnSelectZip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectZip1.Location = new System.Drawing.Point(779, 6);
            this.btnSelectZip1.Name = "btnSelectZip1";
            this.btnSelectZip1.Size = new System.Drawing.Size(86, 28);
            this.btnSelectZip1.TabIndex = 1;
            this.btnSelectZip1.Text = "Select";
            this.btnSelectZip1.UseVisualStyleBackColor = true;
            this.btnSelectZip1.Click += new System.EventHandler(this.btnSelectZip_Click);
            // 
            // textMessage1
            // 
            this.textMessage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textMessage1.Location = new System.Drawing.Point(6, 25);
            this.textMessage1.Multiline = true;
            this.textMessage1.Name = "textMessage1";
            this.textMessage1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textMessage1.Size = new System.Drawing.Size(928, 396);
            this.textMessage1.TabIndex = 2;
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
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(960, 500);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnUploadData1);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.groupBox11);
            this.tabPage1.Controls.Add(this.textFilePath1);
            this.tabPage1.Controls.Add(this.btnSelectZip1);
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(952, 471);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "  Validation  ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnUploadData1
            // 
            this.btnUploadData1.Enabled = false;
            this.btnUploadData1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadData1.Location = new System.Drawing.Point(871, 6);
            this.btnUploadData1.Name = "btnUploadData1";
            this.btnUploadData1.Size = new System.Drawing.Size(75, 28);
            this.btnUploadData1.TabIndex = 4;
            this.btnUploadData1.Text = "Upload";
            this.btnUploadData1.UseVisualStyleBackColor = true;
            this.btnUploadData1.Click += new System.EventHandler(this.btnUploadSingleData_Click);
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
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.textMessage1);
            this.groupBox11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox11.Location = new System.Drawing.Point(6, 38);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(940, 427);
            this.groupBox11.TabIndex = 3;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Message";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox23);
            this.tabPage2.Controls.Add(this.groupBox22);
            this.tabPage2.Controls.Add(this.groupBox21);
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
            // groupBox23
            // 
            this.groupBox23.Controls.Add(this.textMessage2);
            this.groupBox23.Location = new System.Drawing.Point(191, 112);
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.Size = new System.Drawing.Size(755, 353);
            this.groupBox23.TabIndex = 8;
            this.groupBox23.TabStop = false;
            this.groupBox23.Text = "Message/Content";
            // 
            // textMessage2
            // 
            this.textMessage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textMessage2.Location = new System.Drawing.Point(6, 25);
            this.textMessage2.Multiline = true;
            this.textMessage2.Name = "textMessage2";
            this.textMessage2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textMessage2.Size = new System.Drawing.Size(743, 318);
            this.textMessage2.TabIndex = 2;
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.btnLoadData2);
            this.groupBox22.Controls.Add(this.root2);
            this.groupBox22.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox22.Location = new System.Drawing.Point(6, 112);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Size = new System.Drawing.Size(179, 353);
            this.groupBox22.TabIndex = 7;
            this.groupBox22.TabStop = false;
            this.groupBox22.Text = "Check and Review";
            // 
            // btnLoadData2
            // 
            this.btnLoadData2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadData2.Location = new System.Drawing.Point(6, 25);
            this.btnLoadData2.Name = "btnLoadData2";
            this.btnLoadData2.Size = new System.Drawing.Size(167, 30);
            this.btnLoadData2.TabIndex = 4;
            this.btnLoadData2.Text = "Load From Blob";
            this.btnLoadData2.UseVisualStyleBackColor = true;
            this.btnLoadData2.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // root2
            // 
            this.root2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.root2.ForeColor = System.Drawing.Color.Coral;
            this.root2.Location = new System.Drawing.Point(6, 61);
            this.root2.Name = "root2";
            this.root2.Size = new System.Drawing.Size(167, 282);
            this.root2.TabIndex = 1;
            this.root2.DoubleClick += new System.EventHandler(this.root_DoubleClick);
            // 
            // groupBox21
            // 
            this.groupBox21.Controls.Add(this.textAccount2);
            this.groupBox21.Controls.Add(this.label4);
            this.groupBox21.Controls.Add(this.label1);
            this.groupBox21.Controls.Add(this.textAccountKey2);
            this.groupBox21.Controls.Add(this.btnUpLoadZip2);
            this.groupBox21.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox21.Location = new System.Drawing.Point(6, 6);
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.Size = new System.Drawing.Size(940, 89);
            this.groupBox21.TabIndex = 6;
            this.groupBox21.TabStop = false;
            this.groupBox21.Text = "Azure Blob Storage";
            // 
            // textAccount2
            // 
            this.textAccount2.Location = new System.Drawing.Point(149, 20);
            this.textAccount2.Name = "textAccount2";
            this.textAccount2.Size = new System.Drawing.Size(693, 26);
            this.textAccount2.TabIndex = 9;
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
            // textAccountKey2
            // 
            this.textAccountKey2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textAccountKey2.Location = new System.Drawing.Point(149, 52);
            this.textAccountKey2.Name = "textAccountKey2";
            this.textAccountKey2.Size = new System.Drawing.Size(785, 26);
            this.textAccountKey2.TabIndex = 0;
            // 
            // btnUpLoadZip2
            // 
            this.btnUpLoadZip2.Enabled = false;
            this.btnUpLoadZip2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpLoadZip2.ForeColor = System.Drawing.Color.Black;
            this.btnUpLoadZip2.Location = new System.Drawing.Point(848, 20);
            this.btnUpLoadZip2.Name = "btnUpLoadZip2";
            this.btnUpLoadZip2.Size = new System.Drawing.Size(86, 28);
            this.btnUpLoadZip2.TabIndex = 3;
            this.btnUpLoadZip2.Text = "Upload";
            this.btnUpLoadZip2.UseVisualStyleBackColor = true;
            this.btnUpLoadZip2.Click += new System.EventHandler(this.btnUpLoadMultiZip_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox31);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.textFilePath3);
            this.tabPage3.Controls.Add(this.btnUploadData3);
            this.tabPage3.Controls.Add(this.btnSave3);
            this.tabPage3.Controls.Add(this.btnSelectZip3);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(952, 471);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "SeparateKeyValue";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnSelectZip3
            // 
            this.btnSelectZip3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectZip3.Location = new System.Drawing.Point(687, 6);
            this.btnSelectZip3.Name = "btnSelectZip3";
            this.btnSelectZip3.Size = new System.Drawing.Size(86, 28);
            this.btnSelectZip3.TabIndex = 0;
            this.btnSelectZip3.Text = "Select";
            this.btnSelectZip3.UseVisualStyleBackColor = true;
            this.btnSelectZip3.Click += new System.EventHandler(this.btnSelectOriginalZip_Click);
            // 
            // btnSave3
            // 
            this.btnSave3.Enabled = false;
            this.btnSave3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave3.Location = new System.Drawing.Point(779, 6);
            this.btnSave3.Name = "btnSave3";
            this.btnSave3.Size = new System.Drawing.Size(86, 28);
            this.btnSave3.TabIndex = 1;
            this.btnSave3.Text = "Save";
            this.btnSave3.UseVisualStyleBackColor = true;
            // 
            // btnUploadData3
            // 
            this.btnUploadData3.Enabled = false;
            this.btnUploadData3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadData3.Location = new System.Drawing.Point(871, 6);
            this.btnUploadData3.Name = "btnUploadData3";
            this.btnUploadData3.Size = new System.Drawing.Size(75, 28);
            this.btnUploadData3.TabIndex = 2;
            this.btnUploadData3.Text = "Upload";
            this.btnUploadData3.UseVisualStyleBackColor = true;
            // 
            // textFilePath3
            // 
            this.textFilePath3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textFilePath3.Location = new System.Drawing.Point(78, 6);
            this.textFilePath3.Name = "textFilePath3";
            this.textFilePath3.Size = new System.Drawing.Size(603, 26);
            this.textFilePath3.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "FilePath:";
            // 
            // groupBox31
            // 
            this.groupBox31.Controls.Add(this.textMessage3);
            this.groupBox31.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox31.Location = new System.Drawing.Point(6, 38);
            this.groupBox31.Name = "groupBox31";
            this.groupBox31.Size = new System.Drawing.Size(940, 427);
            this.groupBox31.TabIndex = 5;
            this.groupBox31.TabStop = false;
            this.groupBox31.Text = "Message";
            // 
            // textMessage3
            // 
            this.textMessage3.Location = new System.Drawing.Point(6, 25);
            this.textMessage3.Multiline = true;
            this.textMessage3.Name = "textMessage3";
            this.textMessage3.Size = new System.Drawing.Size(928, 396);
            this.textMessage3.TabIndex = 0;
            // 
            // openFileDialog3
            // 
            this.openFileDialog3.FileName = "openFileDialog3";
            this.openFileDialog3.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog3_FileOk);
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
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox23.ResumeLayout(false);
            this.groupBox23.PerformLayout();
            this.groupBox22.ResumeLayout(false);
            this.groupBox21.ResumeLayout(false);
            this.groupBox21.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox31.ResumeLayout(false);
            this.groupBox31.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textFilePath1;
        private System.Windows.Forms.Button btnSelectZip1;
        private System.Windows.Forms.TextBox textMessage1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnUpLoadZip2;
        private System.Windows.Forms.TextBox textMessage2;
        private System.Windows.Forms.TreeView root2;
        private System.Windows.Forms.TextBox textAccountKey2;
        private System.Windows.Forms.Button btnLoadData2;
        private System.Windows.Forms.GroupBox groupBox22;
        private System.Windows.Forms.GroupBox groupBox21;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.GroupBox groupBox23;
        private System.Windows.Forms.TextBox textAccount2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnUploadData1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textFilePath3;
        private System.Windows.Forms.Button btnUploadData3;
        private System.Windows.Forms.Button btnSave3;
        private System.Windows.Forms.Button btnSelectZip3;
        private System.Windows.Forms.GroupBox groupBox31;
        private System.Windows.Forms.TextBox textMessage3;
        private System.Windows.Forms.OpenFileDialog openFileDialog3;
    }
}

