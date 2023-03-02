namespace SSXMultiTool.Tools
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
            // BnkDown
            // 
            this.BnkDown.Location = new System.Drawing.Point(101, 340);
            this.BnkDown.Name = "BnkDown";
            this.BnkDown.Size = new System.Drawing.Size(75, 23);
            this.BnkDown.TabIndex = 6;
            this.BnkDown.Text = "\\/";
            this.BnkDown.UseVisualStyleBackColor = true;
            // 
            // BnkUp
            // 
            this.BnkUp.Location = new System.Drawing.Point(6, 340);
            this.BnkUp.Name = "BnkUp";
            this.BnkUp.Size = new System.Drawing.Size(75, 23);
            this.BnkUp.TabIndex = 5;
            this.BnkUp.Text = "/\\";
            this.BnkUp.UseVisualStyleBackColor = true;
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
            this.BnkLoadFolder.Location = new System.Drawing.Point(87, 369);
            this.BnkLoadFolder.Name = "BnkLoadFolder";
            this.BnkLoadFolder.Size = new System.Drawing.Size(89, 23);
            this.BnkLoadFolder.TabIndex = 3;
            this.BnkLoadFolder.Text = "Load Folder";
            this.BnkLoadFolder.UseVisualStyleBackColor = true;
            // 
            // BnkBuild
            // 
            this.BnkBuild.Location = new System.Drawing.Point(687, 369);
            this.BnkBuild.Name = "BnkBuild";
            this.BnkBuild.Size = new System.Drawing.Size(75, 23);
            this.BnkBuild.TabIndex = 2;
            this.BnkBuild.Text = "Build .bnk";
            this.BnkBuild.UseVisualStyleBackColor = true;
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
            this.BnkLoadFile.Location = new System.Drawing.Point(6, 369);
            this.BnkLoadFile.Name = "BnkLoadFile";
            this.BnkLoadFile.Size = new System.Drawing.Size(75, 23);
            this.BnkLoadFile.TabIndex = 0;
            this.BnkLoadFile.Text = "Load File";
            this.BnkLoadFile.UseVisualStyleBackColor = true;
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
    }
}