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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            BnkTotalSamples = new Label();
            BnkSample = new Label();
            BnkFileSize = new Label();
            BnkTime = new Label();
            bnkPlay = new Button();
            BnkRemoveFile = new Button();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            BnkDown = new Button();
            BnkUp = new Button();
            BnkFileList = new ListBox();
            BnkLoadFolder = new Button();
            BnkBuild = new Button();
            BnkWavExtract = new Button();
            BnkLoadFile = new Button();
            NewLoadTest = new Button();
            button1 = new Button();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(776, 426);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(button1);
            tabPage1.Controls.Add(BnkTotalSamples);
            tabPage1.Controls.Add(BnkSample);
            tabPage1.Controls.Add(BnkFileSize);
            tabPage1.Controls.Add(BnkTime);
            tabPage1.Controls.Add(bnkPlay);
            tabPage1.Controls.Add(BnkRemoveFile);
            tabPage1.Controls.Add(label4);
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(BnkDown);
            tabPage1.Controls.Add(BnkUp);
            tabPage1.Controls.Add(BnkFileList);
            tabPage1.Controls.Add(BnkLoadFolder);
            tabPage1.Controls.Add(BnkBuild);
            tabPage1.Controls.Add(BnkWavExtract);
            tabPage1.Controls.Add(BnkLoadFile);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(768, 398);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "PS2 Bank Files (.bnk)";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // BnkTotalSamples
            // 
            BnkTotalSamples.AutoSize = true;
            BnkTotalSamples.Location = new Point(464, 23);
            BnkTotalSamples.Name = "BnkTotalSamples";
            BnkTotalSamples.Size = new Size(13, 15);
            BnkTotalSamples.TabIndex = 16;
            BnkTotalSamples.Text = "0";
            // 
            // BnkSample
            // 
            BnkSample.AutoSize = true;
            BnkSample.Location = new Point(371, 23);
            BnkSample.Name = "BnkSample";
            BnkSample.Size = new Size(13, 15);
            BnkSample.TabIndex = 15;
            BnkSample.Text = "0";
            // 
            // BnkFileSize
            // 
            BnkFileSize.AutoSize = true;
            BnkFileSize.Location = new Point(292, 23);
            BnkFileSize.Name = "BnkFileSize";
            BnkFileSize.Size = new Size(13, 15);
            BnkFileSize.TabIndex = 14;
            BnkFileSize.Text = "0";
            // 
            // BnkTime
            // 
            BnkTime.AutoSize = true;
            BnkTime.Location = new Point(188, 23);
            BnkTime.Name = "BnkTime";
            BnkTime.Size = new Size(13, 15);
            BnkTime.TabIndex = 13;
            BnkTime.Text = "0";
            // 
            // bnkPlay
            // 
            bnkPlay.Location = new Point(182, 369);
            bnkPlay.Name = "bnkPlay";
            bnkPlay.Size = new Size(75, 23);
            bnkPlay.TabIndex = 12;
            bnkPlay.Text = "Play Audio";
            bnkPlay.UseVisualStyleBackColor = true;
            bnkPlay.Click += bnkPlay_Click;
            // 
            // BnkRemoveFile
            // 
            BnkRemoveFile.Location = new Point(6, 340);
            BnkRemoveFile.Name = "BnkRemoveFile";
            BnkRemoveFile.Size = new Size(38, 23);
            BnkRemoveFile.TabIndex = 11;
            BnkRemoveFile.Text = "-";
            BnkRemoveFile.UseVisualStyleBackColor = true;
            BnkRemoveFile.Click += BnkRemoveFile_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(464, 8);
            label4.Name = "label4";
            label4.Size = new Size(79, 15);
            label4.TabIndex = 10;
            label4.Text = "Total Samples";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(371, 8);
            label3.Name = "label3";
            label3.Size = new Size(72, 15);
            label3.TabIndex = 9;
            label3.Text = "Sample Rate";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(292, 8);
            label2.Name = "label2";
            label2.Size = new Size(48, 15);
            label2.TabIndex = 8;
            label2.Text = "File Size";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(188, 8);
            label1.Name = "label1";
            label1.Size = new Size(73, 15);
            label1.TabIndex = 7;
            label1.Text = "Time Length";
            // 
            // BnkDown
            // 
            BnkDown.Location = new Point(96, 340);
            BnkDown.Name = "BnkDown";
            BnkDown.Size = new Size(32, 23);
            BnkDown.TabIndex = 6;
            BnkDown.Text = "\\/";
            BnkDown.UseVisualStyleBackColor = true;
            BnkDown.Click += BnkDown_Click;
            // 
            // BnkUp
            // 
            BnkUp.Location = new Point(48, 340);
            BnkUp.Name = "BnkUp";
            BnkUp.Size = new Size(33, 23);
            BnkUp.TabIndex = 5;
            BnkUp.Text = "/\\";
            BnkUp.UseVisualStyleBackColor = true;
            BnkUp.Click += BnkUp_Click;
            // 
            // BnkFileList
            // 
            BnkFileList.FormattingEnabled = true;
            BnkFileList.ItemHeight = 15;
            BnkFileList.Location = new Point(6, 6);
            BnkFileList.Name = "BnkFileList";
            BnkFileList.Size = new Size(170, 334);
            BnkFileList.TabIndex = 4;
            BnkFileList.SelectedIndexChanged += BnkFileList_SelectedIndexChanged;
            // 
            // BnkLoadFolder
            // 
            BnkLoadFolder.Location = new Point(6, 369);
            BnkLoadFolder.Name = "BnkLoadFolder";
            BnkLoadFolder.Size = new Size(170, 23);
            BnkLoadFolder.TabIndex = 3;
            BnkLoadFolder.Text = "Load Folder";
            BnkLoadFolder.UseVisualStyleBackColor = true;
            BnkLoadFolder.Click += BnkLoadFolder_Click;
            // 
            // BnkBuild
            // 
            BnkBuild.Location = new Point(687, 369);
            BnkBuild.Name = "BnkBuild";
            BnkBuild.Size = new Size(75, 23);
            BnkBuild.TabIndex = 2;
            BnkBuild.Text = "Build .bnk";
            BnkBuild.UseVisualStyleBackColor = true;
            BnkBuild.Click += BnkBuild_Click;
            // 
            // BnkWavExtract
            // 
            BnkWavExtract.Location = new Point(591, 369);
            BnkWavExtract.Name = "BnkWavExtract";
            BnkWavExtract.Size = new Size(90, 23);
            BnkWavExtract.TabIndex = 1;
            BnkWavExtract.Text = "Extract Wav";
            BnkWavExtract.UseVisualStyleBackColor = true;
            BnkWavExtract.Click += BnkWavExtract_Click;
            // 
            // BnkLoadFile
            // 
            BnkLoadFile.Location = new Point(134, 340);
            BnkLoadFile.Name = "BnkLoadFile";
            BnkLoadFile.Size = new Size(42, 23);
            BnkLoadFile.TabIndex = 0;
            BnkLoadFile.Text = "+";
            BnkLoadFile.UseVisualStyleBackColor = true;
            BnkLoadFile.Click += BnkLoadFile_Click;
            // 
            // NewLoadTest
            // 
            NewLoadTest.Location = new Point(359, 7);
            NewLoadTest.Name = "NewLoadTest";
            NewLoadTest.Size = new Size(75, 23);
            NewLoadTest.TabIndex = 0;
            NewLoadTest.Text = "Load Audio";
            NewLoadTest.UseVisualStyleBackColor = true;
            NewLoadTest.Visible = false;
            NewLoadTest.Click += NewLoadTest_Click;
            // 
            // button1
            // 
            button1.Location = new Point(482, 369);
            button1.Name = "button1";
            button1.Size = new Size(90, 23);
            button1.TabIndex = 17;
            button1.Text = "Extract Wav";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // AudioTools
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(NewLoadTest);
            Controls.Add(tabControl1);
            Name = "AudioTools";
            Text = "AudioTools";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ResumeLayout(false);
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
        private Button bnkPlay;
        private Label BnkTotalSamples;
        private Label BnkSample;
        private Label BnkFileSize;
        private Label BnkTime;
        private Button NewLoadTest;
        private Button button1;
    }
}