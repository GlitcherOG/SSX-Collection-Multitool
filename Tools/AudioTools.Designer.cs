﻿namespace SSXMultiTool.Tools
{
    partial class AudioTools
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.BnkRemoveFile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.BnkDown = new System.Windows.Forms.Button();
            this.BnkUp = new System.Windows.Forms.Button();
            this.BnkFileList = new System.Windows.Forms.ListBox();
            this.BnkLoadFolder = new System.Windows.Forms.Button();
            this.BnkBuild = new System.Windows.Forms.Button();
            this.BnkWavExtract = new System.Windows.Forms.Button();
            this.BnkLoadFile = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(776, 426);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.BnkRemoveFile);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.BnkDown);
            this.tabPage1.Controls.Add(this.BnkUp);
            this.tabPage1.Controls.Add(this.BnkFileList);
            this.tabPage1.Controls.Add(this.BnkLoadFolder);
            this.tabPage1.Controls.Add(this.BnkBuild);
            this.tabPage1.Controls.Add(this.BnkWavExtract);
            this.tabPage1.Controls.Add(this.BnkLoadFile);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(768, 398);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "PS2 Bank Files (.bnk)";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // BnkRemoveFile
            // 
            this.BnkRemoveFile.Location = new System.Drawing.Point(6, 340);
            this.BnkRemoveFile.Name = "BnkRemoveFile";
            this.BnkRemoveFile.Size = new System.Drawing.Size(38, 23);
            this.BnkRemoveFile.TabIndex = 11;
            this.BnkRemoveFile.Text = "-";
            this.BnkRemoveFile.UseVisualStyleBackColor = true;
            this.BnkRemoveFile.Click += new System.EventHandler(this.BnkRemoveFile_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(449, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Total Samples";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(359, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "Sample Rate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(292, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "File Size";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(188, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Time Length";
            // 
            // BnkDown
            // 
            this.BnkDown.Location = new System.Drawing.Point(96, 340);
            this.BnkDown.Name = "BnkDown";
            this.BnkDown.Size = new System.Drawing.Size(32, 23);
            this.BnkDown.TabIndex = 6;
            this.BnkDown.Text = "\\/";
            this.BnkDown.UseVisualStyleBackColor = true;
            this.BnkDown.Click += new System.EventHandler(this.BnkDown_Click);
            // 
            // BnkUp
            // 
            this.BnkUp.Location = new System.Drawing.Point(48, 340);
            this.BnkUp.Name = "BnkUp";
            this.BnkUp.Size = new System.Drawing.Size(33, 23);
            this.BnkUp.TabIndex = 5;
            this.BnkUp.Text = "/\\";
            this.BnkUp.UseVisualStyleBackColor = true;
            this.BnkUp.Click += new System.EventHandler(this.BnkUp_Click);
            // 
            // BnkFileList
            // 
            this.BnkFileList.FormattingEnabled = true;
            this.BnkFileList.ItemHeight = 15;
            this.BnkFileList.Location = new System.Drawing.Point(6, 6);
            this.BnkFileList.Name = "BnkFileList";
            this.BnkFileList.Size = new System.Drawing.Size(170, 334);
            this.BnkFileList.TabIndex = 4;
            // 
            // BnkLoadFolder
            // 
            this.BnkLoadFolder.Location = new System.Drawing.Point(6, 369);
            this.BnkLoadFolder.Name = "BnkLoadFolder";
            this.BnkLoadFolder.Size = new System.Drawing.Size(170, 23);
            this.BnkLoadFolder.TabIndex = 3;
            this.BnkLoadFolder.Text = "Load Folder";
            this.BnkLoadFolder.UseVisualStyleBackColor = true;
            this.BnkLoadFolder.Click += new System.EventHandler(this.BnkLoadFolder_Click);
            // 
            // BnkBuild
            // 
            this.BnkBuild.Location = new System.Drawing.Point(687, 369);
            this.BnkBuild.Name = "BnkBuild";
            this.BnkBuild.Size = new System.Drawing.Size(75, 23);
            this.BnkBuild.TabIndex = 2;
            this.BnkBuild.Text = "Build .bnk";
            this.BnkBuild.UseVisualStyleBackColor = true;
            this.BnkBuild.Click += new System.EventHandler(this.BnkBuild_Click);
            // 
            // BnkWavExtract
            // 
            this.BnkWavExtract.Location = new System.Drawing.Point(591, 369);
            this.BnkWavExtract.Name = "BnkWavExtract";
            this.BnkWavExtract.Size = new System.Drawing.Size(90, 23);
            this.BnkWavExtract.TabIndex = 1;
            this.BnkWavExtract.Text = "Extract Wav";
            this.BnkWavExtract.UseVisualStyleBackColor = true;
            this.BnkWavExtract.Click += new System.EventHandler(this.BnkWavExtract_Click);
            // 
            // BnkLoadFile
            // 
            this.BnkLoadFile.Location = new System.Drawing.Point(134, 340);
            this.BnkLoadFile.Name = "BnkLoadFile";
            this.BnkLoadFile.Size = new System.Drawing.Size(42, 23);
            this.BnkLoadFile.TabIndex = 0;
            this.BnkLoadFile.Text = "+";
            this.BnkLoadFile.UseVisualStyleBackColor = true;
            this.BnkLoadFile.Click += new System.EventHandler(this.BnkLoadFile_Click);
            // 
            // AudioTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "AudioTools";
            this.Text = "AudioTools";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button BnkDown;
        private Button BnkUp;
        private ListBox BnkFileList;
        private Button BnkLoadFolder;
        private Button BnkBuild;
        private Button BnkWavExtract;
        private Button BnkLoadFile;
        private Label label1;
        private Label label4;
        private Label label3;
        private Label label2;
        private Button BnkRemoveFile;
    }
}