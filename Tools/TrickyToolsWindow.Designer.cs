﻿namespace SSXMultiTool
{
    partial class TrickyToolsWindow
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
            this.MpfList = new System.Windows.Forms.ListBox();
            this.MPFExtract = new System.Windows.Forms.Button();
            this.MPFLoad = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ELFLdrSetup = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(776, 426);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.MpfList);
            this.tabPage1.Controls.Add(this.MPFExtract);
            this.tabPage1.Controls.Add(this.MPFLoad);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(768, 398);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Tricky MPF (Models)";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // MpfList
            // 
            this.MpfList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MpfList.FormattingEnabled = true;
            this.MpfList.ItemHeight = 15;
            this.MpfList.Location = new System.Drawing.Point(6, 6);
            this.MpfList.Name = "MpfList";
            this.MpfList.Size = new System.Drawing.Size(192, 379);
            this.MpfList.TabIndex = 2;
            // 
            // MPFExtract
            // 
            this.MPFExtract.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MPFExtract.Location = new System.Drawing.Point(687, 366);
            this.MPFExtract.Name = "MPFExtract";
            this.MPFExtract.Size = new System.Drawing.Size(75, 23);
            this.MPFExtract.TabIndex = 1;
            this.MPFExtract.Text = "Extract";
            this.MPFExtract.UseVisualStyleBackColor = true;
            this.MPFExtract.Click += new System.EventHandler(this.MPFExtract_Click);
            // 
            // MPFLoad
            // 
            this.MPFLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MPFLoad.Location = new System.Drawing.Point(204, 366);
            this.MPFLoad.Name = "MPFLoad";
            this.MPFLoad.Size = new System.Drawing.Size(75, 23);
            this.MPFLoad.TabIndex = 0;
            this.MPFLoad.Text = "Load";
            this.MPFLoad.UseVisualStyleBackColor = true;
            this.MPFLoad.Click += new System.EventHandler(this.MPFLoad_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ELFLdrSetup);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(768, 398);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tools";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ELFLdrSetup
            // 
            this.ELFLdrSetup.Location = new System.Drawing.Point(3, 3);
            this.ELFLdrSetup.Name = "ELFLdrSetup";
            this.ELFLdrSetup.Size = new System.Drawing.Size(132, 72);
            this.ELFLdrSetup.TabIndex = 0;
            this.ELFLdrSetup.Text = "Setup For ELFLdr";
            this.ELFLdrSetup.UseVisualStyleBackColor = true;
            this.ELFLdrSetup.Click += new System.EventHandler(this.ELFLdrSetup_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(285, 366);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Load 1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TrickyToolsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "TrickyToolsWindow";
            this.Text = "TrickyToolsWindow";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button MPFExtract;
        private Button MPFLoad;
        private ListBox MpfList;
        private TabPage tabPage2;
        private Button ELFLdrSetup;
        private Button button1;
    }
}