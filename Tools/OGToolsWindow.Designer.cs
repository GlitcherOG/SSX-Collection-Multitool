namespace SSXMultiTool
{
    partial class OGToolsWindow
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
            this.MpfExtract = new System.Windows.Forms.Button();
            this.MpfLoad = new System.Windows.Forms.Button();
            this.MPFList = new System.Windows.Forms.ListBox();
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
            this.tabPage1.Controls.Add(this.MpfExtract);
            this.tabPage1.Controls.Add(this.MpfLoad);
            this.tabPage1.Controls.Add(this.MPFList);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(768, 398);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "SSX (2000) MPF(Models)";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // MpfExtract
            // 
            this.MpfExtract.Location = new System.Drawing.Point(687, 369);
            this.MpfExtract.Name = "MpfExtract";
            this.MpfExtract.Size = new System.Drawing.Size(75, 23);
            this.MpfExtract.TabIndex = 2;
            this.MpfExtract.Text = "button2";
            this.MpfExtract.UseVisualStyleBackColor = true;
            this.MpfExtract.Click += new System.EventHandler(this.MpfExtract_Click);
            // 
            // MpfLoad
            // 
            this.MpfLoad.Location = new System.Drawing.Point(257, 369);
            this.MpfLoad.Name = "MpfLoad";
            this.MpfLoad.Size = new System.Drawing.Size(75, 23);
            this.MpfLoad.TabIndex = 1;
            this.MpfLoad.Text = "button1";
            this.MpfLoad.UseVisualStyleBackColor = true;
            this.MpfLoad.Click += new System.EventHandler(this.MpfLoad_Click);
            // 
            // MPFList
            // 
            this.MPFList.FormattingEnabled = true;
            this.MPFList.ItemHeight = 15;
            this.MPFList.Location = new System.Drawing.Point(6, 6);
            this.MPFList.Name = "MPFList";
            this.MPFList.Size = new System.Drawing.Size(245, 379);
            this.MPFList.TabIndex = 0;
            // 
            // OGToolsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "OGToolsWindow";
            this.Text = "OGToolsWindow";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button MpfExtract;
        private Button MpfLoad;
        private ListBox MPFList;
    }
}