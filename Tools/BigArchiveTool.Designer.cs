namespace SSXMultiTool
{
    partial class BigArchiveTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BigArchiveTool));
            this.BigDataView = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.LoadBigArchive = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExtractBigArchive = new System.Windows.Forms.ToolStripMenuItem();
            this.BuildBigArchive = new System.Windows.Forms.ToolStripMenuItem();
            this.BigTypeCombobox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.BigDataView)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BigDataView
            // 
            this.BigDataView.AllowUserToAddRows = false;
            this.BigDataView.AllowUserToDeleteRows = false;
            this.BigDataView.AllowUserToResizeRows = false;
            this.BigDataView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BigDataView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.BigDataView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.BigDataView.GridColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BigDataView.Location = new System.Drawing.Point(0, 28);
            this.BigDataView.MultiSelect = false;
            this.BigDataView.Name = "BigDataView";
            this.BigDataView.RowTemplate.Height = 25;
            this.BigDataView.Size = new System.Drawing.Size(800, 417);
            this.BigDataView.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.BigTypeCombobox,
            this.toolStripLabel1,
            this.toolStripSeparator2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadBigArchive,
            this.LoadFolder,
            this.toolStripSeparator1,
            this.ExtractBigArchive,
            this.BuildBigArchive});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(38, 22);
            this.toolStripDropDownButton1.Text = "File";
            // 
            // LoadBigArchive
            // 
            this.LoadBigArchive.Name = "LoadBigArchive";
            this.LoadBigArchive.Size = new System.Drawing.Size(176, 22);
            this.LoadBigArchive.Text = "Load .Big Archive";
            this.LoadBigArchive.Click += new System.EventHandler(this.LoadBigArchive_Click);
            // 
            // LoadFolder
            // 
            this.LoadFolder.Name = "LoadFolder";
            this.LoadFolder.Size = new System.Drawing.Size(176, 22);
            this.LoadFolder.Text = "Load Folder";
            this.LoadFolder.Click += new System.EventHandler(this.LoadFolder_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(173, 6);
            // 
            // ExtractBigArchive
            // 
            this.ExtractBigArchive.Name = "ExtractBigArchive";
            this.ExtractBigArchive.Size = new System.Drawing.Size(176, 22);
            this.ExtractBigArchive.Text = "Extract .Big Archive";
            this.ExtractBigArchive.Click += new System.EventHandler(this.ExtractBigArchive_Click);
            // 
            // BuildBigArchive
            // 
            this.BuildBigArchive.Name = "BuildBigArchive";
            this.BuildBigArchive.Size = new System.Drawing.Size(176, 22);
            this.BuildBigArchive.Text = "Build .Big Archive";
            this.BuildBigArchive.Click += new System.EventHandler(this.BuildBigArchive_Click);
            // 
            // BigTypeCombobox
            // 
            this.BigTypeCombobox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.BigTypeCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BigTypeCombobox.Enabled = false;
            this.BigTypeCombobox.Items.AddRange(new object[] {
            "BIGF",
            "C0FB",
            "BIG4"});
            this.BigTypeCombobox.Name = "BigTypeCombobox";
            this.BigTypeCombobox.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(54, 22);
            this.toolStripLabel1.Text = "Big Type:";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // BigArchiveTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 444);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.BigDataView);
            this.Name = "BigArchiveTool";
            this.Text = "Big Archive";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BigArchiveTool_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.BigDataView)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView BigDataView;
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem LoadBigArchive;
        private ToolStripMenuItem LoadFolder;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem ExtractBigArchive;
        private ToolStripMenuItem BuildBigArchive;
        private ToolStripComboBox BigTypeCombobox;
        private ToolStripLabel toolStripLabel1;
        private ToolStripSeparator toolStripSeparator2;
    }
}