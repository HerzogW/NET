﻿namespace ProjectOxfordExtensionConfigurationZipFileCheck
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
            this.textMessage1 = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSave1 = new System.Windows.Forms.Button();
            this.btnUploadData1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.btnSelectZip1 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl22 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textMessage2 = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.textFileContent2 = new System.Windows.Forms.TextBox();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.tabControl21 = new System.Windows.Forms.TabControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.root2 = new System.Windows.Forms.TreeView();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.checkedListBox2 = new System.Windows.Forms.CheckedListBox();
            this.btnLoadData2 = new System.Windows.Forms.Button();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.textAccount2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textAccountKey2 = new System.Windows.Forms.TextBox();
            this.btnUpLoadZip2 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl22.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox22.SuspendLayout();
            this.tabControl21.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.groupBox21.SuspendLayout();
            this.SuspendLayout();
            // 
            // textFilePath1
            // 
            this.textFilePath1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textFilePath1.Location = new System.Drawing.Point(78, 6);
            this.textFilePath1.Name = "textFilePath1";
            this.textFilePath1.Size = new System.Drawing.Size(614, 26);
            this.textFilePath1.TabIndex = 0;
            // 
            // textMessage1
            // 
            this.textMessage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(960, 515);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSave1);
            this.tabPage1.Controls.Add(this.btnUploadData1);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.groupBox11);
            this.tabPage1.Controls.Add(this.textFilePath1);
            this.tabPage1.Controls.Add(this.btnSelectZip1);
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(952, 486);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "GenerateApiConfiguration";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSave1
            // 
            this.btnSave1.Enabled = false;
            this.btnSave1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave1.Location = new System.Drawing.Point(790, 6);
            this.btnSave1.Name = "btnSave1";
            this.btnSave1.Size = new System.Drawing.Size(75, 28);
            this.btnSave1.TabIndex = 5;
            this.btnSave1.Text = "Save";
            this.btnSave1.UseVisualStyleBackColor = true;
            this.btnSave1.Click += new System.EventHandler(this.btnSave1_Click);
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
            // btnSelectZip1
            // 
            this.btnSelectZip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectZip1.Location = new System.Drawing.Point(698, 6);
            this.btnSelectZip1.Name = "btnSelectZip1";
            this.btnSelectZip1.Size = new System.Drawing.Size(86, 28);
            this.btnSelectZip1.TabIndex = 1;
            this.btnSelectZip1.Text = "Select";
            this.btnSelectZip1.UseVisualStyleBackColor = true;
            this.btnSelectZip1.Click += new System.EventHandler(this.btnSelectZip_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tabControl22);
            this.tabPage2.Controls.Add(this.groupBox22);
            this.tabPage2.Controls.Add(this.groupBox21);
            this.tabPage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage2.ForeColor = System.Drawing.Color.Black;
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(952, 486);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "  Upload  ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabControl22
            // 
            this.tabControl22.Controls.Add(this.tabPage3);
            this.tabControl22.Controls.Add(this.tabPage4);
            this.tabControl22.Location = new System.Drawing.Point(191, 112);
            this.tabControl22.Name = "tabControl22";
            this.tabControl22.SelectedIndex = 0;
            this.tabControl22.Size = new System.Drawing.Size(755, 369);
            this.tabControl22.TabIndex = 3;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textMessage2);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(747, 336);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Message";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textMessage2
            // 
            this.textMessage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textMessage2.Location = new System.Drawing.Point(6, 6);
            this.textMessage2.Multiline = true;
            this.textMessage2.Name = "textMessage2";
            this.textMessage2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textMessage2.Size = new System.Drawing.Size(735, 323);
            this.textMessage2.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.textFileContent2);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(747, 336);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "FileContent";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // textFileContent2
            // 
            this.textFileContent2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textFileContent2.Location = new System.Drawing.Point(6, 6);
            this.textFileContent2.Multiline = true;
            this.textFileContent2.Name = "textFileContent2";
            this.textFileContent2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textFileContent2.Size = new System.Drawing.Size(735, 323);
            this.textFileContent2.TabIndex = 2;
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.tabControl21);
            this.groupBox22.Controls.Add(this.btnLoadData2);
            this.groupBox22.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox22.Location = new System.Drawing.Point(6, 112);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Size = new System.Drawing.Size(179, 369);
            this.groupBox22.TabIndex = 7;
            this.groupBox22.TabStop = false;
            this.groupBox22.Text = "Check and Review";
            // 
            // tabControl21
            // 
            this.tabControl21.Controls.Add(this.tabPage6);
            this.tabControl21.Controls.Add(this.tabPage7);
            this.tabControl21.Location = new System.Drawing.Point(9, 61);
            this.tabControl21.Name = "tabControl21";
            this.tabControl21.SelectedIndex = 0;
            this.tabControl21.Size = new System.Drawing.Size(164, 301);
            this.tabControl21.TabIndex = 3;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.root2);
            this.tabPage6.Location = new System.Drawing.Point(4, 29);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(156, 268);
            this.tabPage6.TabIndex = 0;
            this.tabPage6.Text = "FileTree";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // root2
            // 
            this.root2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.root2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.root2.ForeColor = System.Drawing.Color.Coral;
            this.root2.ItemHeight = 21;
            this.root2.Location = new System.Drawing.Point(6, 6);
            this.root2.Name = "root2";
            this.root2.Size = new System.Drawing.Size(144, 252);
            this.root2.TabIndex = 1;
            this.root2.DoubleClick += new System.EventHandler(this.root_DoubleClick);
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.checkedListBox2);
            this.tabPage7.Location = new System.Drawing.Point(4, 29);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(156, 268);
            this.tabPage7.TabIndex = 1;
            this.tabPage7.Text = "Items";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // checkedListBox2
            // 
            this.checkedListBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox2.FormattingEnabled = true;
            this.checkedListBox2.HorizontalScrollbar = true;
            this.checkedListBox2.Location = new System.Drawing.Point(6, 6);
            this.checkedListBox2.Name = "checkedListBox2";
            this.checkedListBox2.Size = new System.Drawing.Size(144, 252);
            this.checkedListBox2.TabIndex = 8;
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(984, 534);
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
            this.tabControl22.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.groupBox22.ResumeLayout(false);
            this.tabControl21.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.groupBox21.ResumeLayout(false);
            this.groupBox21.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textFilePath1;
        private System.Windows.Forms.TextBox textMessage1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnUpLoadZip2;
        private System.Windows.Forms.TreeView root2;
        private System.Windows.Forms.TextBox textAccountKey2;
        private System.Windows.Forms.Button btnLoadData2;
        private System.Windows.Forms.GroupBox groupBox22;
        private System.Windows.Forms.GroupBox groupBox21;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.TextBox textAccount2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnUploadData1;
        private System.Windows.Forms.Button btnSelectZip1;
        private System.Windows.Forms.TabControl tabControl22;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox textFileContent2;
        private System.Windows.Forms.TabControl tabControl21;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.CheckedListBox checkedListBox2;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox textMessage2;
        private System.Windows.Forms.Button btnSave1;
    }
}

