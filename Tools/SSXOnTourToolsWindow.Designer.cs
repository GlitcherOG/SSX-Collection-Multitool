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
            this.MpfModelList = new System.Windows.Forms.ListBox();
            this.MpfWarning = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MpfBoneLoad = new System.Windows.Forms.Button();
            this.MpfExport = new System.Windows.Forms.Button();
            this.MpfSave = new System.Windows.Forms.Button();
            this.MpfLoad = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
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
            this.tabPage1.Controls.Add(this.MpfModelList);
            this.tabPage1.Controls.Add(this.MpfWarning);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.MpfBoneLoad);
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
            // MpfModelList
            // 
            this.MpfModelList.FormattingEnabled = true;
            this.MpfModelList.ItemHeight = 15;
            this.MpfModelList.Location = new System.Drawing.Point(6, 6);
            this.MpfModelList.Name = "MpfModelList";
            this.MpfModelList.Size = new System.Drawing.Size(154, 349);
            this.MpfModelList.TabIndex = 6;
            // 
            // MpfWarning
            // 
            this.MpfWarning.AutoSize = true;
            this.MpfWarning.Location = new System.Drawing.Point(166, 18);
            this.MpfWarning.Name = "MpfWarning";
            this.MpfWarning.Size = new System.Drawing.Size(36, 15);
            this.MpfWarning.TabIndex = 5;
            this.MpfWarning.Text = "None";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(166, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Warning";
            // 
            // MpfBoneLoad
            // 
            this.MpfBoneLoad.Location = new System.Drawing.Point(87, 369);
            this.MpfBoneLoad.Name = "MpfBoneLoad";
            this.MpfBoneLoad.Size = new System.Drawing.Size(75, 23);
            this.MpfBoneLoad.TabIndex = 3;
            this.MpfBoneLoad.Text = "Bone Load";
            this.MpfBoneLoad.UseVisualStyleBackColor = true;
            this.MpfBoneLoad.Click += new System.EventHandler(this.MpfBoneLoad_Click);
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
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button MpfLoad;
        private TabPage tabPage2;
        private Button MpfSave;
        private Button MpfExport;
        private Button MpfBoneLoad;
        private Label MpfWarning;
        private Label label1;
        private ListBox MpfModelList;
    }
}