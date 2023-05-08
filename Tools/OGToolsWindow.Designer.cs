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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            MpfExtract = new Button();
            MpfLoad = new Button();
            MPFList = new ListBox();
            tabPage2 = new TabPage();
            ELFLdrSetup = new Button();
            LoadADF = new Button();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(776, 426);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(MpfExtract);
            tabPage1.Controls.Add(MpfLoad);
            tabPage1.Controls.Add(MPFList);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(768, 398);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "SSX (2000) MPF(Models)";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // MpfExtract
            // 
            MpfExtract.Location = new Point(687, 369);
            MpfExtract.Name = "MpfExtract";
            MpfExtract.Size = new Size(75, 23);
            MpfExtract.TabIndex = 2;
            MpfExtract.Text = "Extract";
            MpfExtract.UseVisualStyleBackColor = true;
            MpfExtract.Click += MpfExtract_Click;
            // 
            // MpfLoad
            // 
            MpfLoad.Location = new Point(257, 369);
            MpfLoad.Name = "MpfLoad";
            MpfLoad.Size = new Size(75, 23);
            MpfLoad.TabIndex = 1;
            MpfLoad.Text = "Load";
            MpfLoad.UseVisualStyleBackColor = true;
            MpfLoad.Click += MpfLoad_Click;
            // 
            // MPFList
            // 
            MPFList.FormattingEnabled = true;
            MPFList.ItemHeight = 15;
            MPFList.Location = new Point(6, 6);
            MPFList.Name = "MPFList";
            MPFList.Size = new Size(245, 379);
            MPFList.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(LoadADF);
            tabPage2.Controls.Add(ELFLdrSetup);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new Size(768, 398);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Tools";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // ELFLdrSetup
            // 
            ELFLdrSetup.Location = new Point(3, 3);
            ELFLdrSetup.Name = "ELFLdrSetup";
            ELFLdrSetup.Size = new Size(132, 72);
            ELFLdrSetup.TabIndex = 1;
            ELFLdrSetup.Text = "Setup For ELFLdr";
            ELFLdrSetup.UseVisualStyleBackColor = true;
            ELFLdrSetup.Click += ELFLdrSetup_Click;
            // 
            // LoadADF
            // 
            LoadADF.Location = new Point(112, 157);
            LoadADF.Name = "LoadADF";
            LoadADF.Size = new Size(75, 23);
            LoadADF.TabIndex = 2;
            LoadADF.Text = "Load Test";
            LoadADF.UseVisualStyleBackColor = true;
            LoadADF.Click += LoadADF_Click;
            // 
            // OGToolsWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Name = "OGToolsWindow";
            Text = "OGToolsWindow";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button MpfExtract;
        private Button MpfLoad;
        private ListBox MPFList;
        private TabPage tabPage2;
        private Button ELFLdrSetup;
        private Button LoadADF;
    }
}