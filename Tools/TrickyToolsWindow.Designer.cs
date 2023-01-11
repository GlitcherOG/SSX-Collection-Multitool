namespace SSXMultiTool
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
            this.VerticeCount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TristripCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MPFImport = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.MpfList = new System.Windows.Forms.ListBox();
            this.MPFExtract = new System.Windows.Forms.Button();
            this.MPFLoad = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ELFLdrSetup = new System.Windows.Forms.Button();
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
            this.tabPage1.Controls.Add(this.VerticeCount);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.TristripCount);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.MPFImport);
            this.tabPage1.Controls.Add(this.button2);
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
            // VerticeCount
            // 
            this.VerticeCount.AutoSize = true;
            this.VerticeCount.Location = new System.Drawing.Point(204, 65);
            this.VerticeCount.Name = "VerticeCount";
            this.VerticeCount.Size = new System.Drawing.Size(13, 15);
            this.VerticeCount.TabIndex = 9;
            this.VerticeCount.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(204, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "Vertice Count";
            // 
            // TristripCount
            // 
            this.TristripCount.AutoSize = true;
            this.TristripCount.Location = new System.Drawing.Point(204, 21);
            this.TristripCount.Name = "TristripCount";
            this.TristripCount.Size = new System.Drawing.Size(13, 15);
            this.TristripCount.TabIndex = 7;
            this.TristripCount.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(204, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Tristrip Count";
            // 
            // MPFImport
            // 
            this.MPFImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MPFImport.Location = new System.Drawing.Point(285, 366);
            this.MPFImport.Name = "MPFImport";
            this.MPFImport.Size = new System.Drawing.Size(75, 23);
            this.MPFImport.TabIndex = 5;
            this.MPFImport.Text = "Import Model";
            this.MPFImport.UseVisualStyleBackColor = true;
            this.MPFImport.Click += new System.EventHandler(this.MPFImport_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(687, 366);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Save MPF";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
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
            this.MpfList.SelectedIndexChanged += new System.EventHandler(this.MpfList_SelectedIndexChanged);
            // 
            // MPFExtract
            // 
            this.MPFExtract.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MPFExtract.Location = new System.Drawing.Point(606, 366);
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
            this.MPFLoad.Text = "Load MPF";
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
            this.tabPage1.PerformLayout();
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
        private Button button2;
        private Button MPFImport;
        private Label VerticeCount;
        private Label label4;
        private Label TristripCount;
        private Label label1;
    }
}