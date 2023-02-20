namespace SSXMultiTool.Tools
{
    partial class SSXOnTourToolsWindow
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
            this.MpfSave = new System.Windows.Forms.Button();
            this.MpfLoad = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.MpfExport = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
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
            this.tabPage1.Controls.Add(this.MpfExport);
            this.tabPage1.Controls.Add(this.MpfSave);
            this.tabPage1.Controls.Add(this.MpfLoad);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(768, 398);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // MpfSave
            // 
            this.MpfSave.Location = new System.Drawing.Point(687, 369);
            this.MpfSave.Name = "MpfSave";
            this.MpfSave.Size = new System.Drawing.Size(75, 23);
            this.MpfSave.TabIndex = 1;
            this.MpfSave.Text = "Save";
            this.MpfSave.UseVisualStyleBackColor = true;
            this.MpfSave.Click += new System.EventHandler(this.MpfSave_Click);
            // 
            // MpfLoad
            // 
            this.MpfLoad.Location = new System.Drawing.Point(6, 369);
            this.MpfLoad.Name = "MpfLoad";
            this.MpfLoad.Size = new System.Drawing.Size(75, 23);
            this.MpfLoad.TabIndex = 0;
            this.MpfLoad.Text = "Load";
            this.MpfLoad.UseVisualStyleBackColor = true;
            this.MpfLoad.Click += new System.EventHandler(this.MpfLoad_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(768, 398);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // MpfExport
            // 
            this.MpfExport.Location = new System.Drawing.Point(606, 369);
            this.MpfExport.Name = "MpfExport";
            this.MpfExport.Size = new System.Drawing.Size(75, 23);
            this.MpfExport.TabIndex = 2;
            this.MpfExport.Text = "Export";
            this.MpfExport.UseVisualStyleBackColor = true;
            this.MpfExport.Click += new System.EventHandler(this.MpfExport_Click);
            // 
            // SSXOnTourToolsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "SSXOnTourToolsWindow";
            this.Text = "SSXOnTourTools";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button MpfLoad;
        private TabPage tabPage2;
        private Button MpfSave;
        private Button MpfExport;
    }
}