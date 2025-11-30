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
            BigDataView = new DataGridView();
            toolStrip1 = new ToolStrip();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            LoadBigArchive = new ToolStripMenuItem();
            LoadFolder = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            ExtractBigArchive = new ToolStripMenuItem();
            BuildBigArchive = new ToolStripMenuItem();
            BigTypeCombobox = new ToolStripComboBox();
            toolStripLabel1 = new ToolStripLabel();
            toolStripSeparator3 = new ToolStripSeparator();
            CompressionButton = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            toolStripButton1 = new ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)BigDataView).BeginInit();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // BigDataView
            // 
            BigDataView.AllowUserToAddRows = false;
            BigDataView.AllowUserToDeleteRows = false;
            BigDataView.AllowUserToResizeRows = false;
            BigDataView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            BigDataView.BackgroundColor = SystemColors.Window;
            BigDataView.EditMode = DataGridViewEditMode.EditProgrammatically;
            BigDataView.GridColor = SystemColors.ActiveCaptionText;
            BigDataView.Location = new Point(0, 28);
            BigDataView.MultiSelect = false;
            BigDataView.Name = "BigDataView";
            BigDataView.Size = new Size(800, 417);
            BigDataView.TabIndex = 0;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1, BigTypeCombobox, toolStripLabel1, toolStripSeparator3, CompressionButton, toolStripSeparator2, toolStripButton1 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(800, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { LoadBigArchive, LoadFolder, toolStripSeparator1, ExtractBigArchive, BuildBigArchive });
            toolStripDropDownButton1.Image = (Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new Size(38, 22);
            toolStripDropDownButton1.Text = "File";
            // 
            // LoadBigArchive
            // 
            LoadBigArchive.Name = "LoadBigArchive";
            LoadBigArchive.Size = new Size(175, 22);
            LoadBigArchive.Text = "Load .Big Archive";
            LoadBigArchive.Click += LoadBigArchive_Click;
            // 
            // LoadFolder
            // 
            LoadFolder.Name = "LoadFolder";
            LoadFolder.Size = new Size(175, 22);
            LoadFolder.Text = "Load Folder";
            LoadFolder.Click += LoadFolder_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(172, 6);
            // 
            // ExtractBigArchive
            // 
            ExtractBigArchive.Name = "ExtractBigArchive";
            ExtractBigArchive.Size = new Size(175, 22);
            ExtractBigArchive.Text = "Extract .Big Archive";
            ExtractBigArchive.Click += ExtractBigArchive_Click;
            // 
            // BuildBigArchive
            // 
            BuildBigArchive.Name = "BuildBigArchive";
            BuildBigArchive.Size = new Size(175, 22);
            BuildBigArchive.Text = "Build .Big Archive";
            BuildBigArchive.Click += BuildBigArchive_Click;
            // 
            // BigTypeCombobox
            // 
            BigTypeCombobox.Alignment = ToolStripItemAlignment.Right;
            BigTypeCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
            BigTypeCombobox.Enabled = false;
            BigTypeCombobox.Items.AddRange(new object[] { "BIGF", "C0FB", "BIG4", "SSX 2012" });
            BigTypeCombobox.Name = "BigTypeCombobox";
            BigTypeCombobox.Size = new Size(121, 25);
            BigTypeCombobox.Click += BigTypeCombobox_Click;
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Alignment = ToolStripItemAlignment.Right;
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(55, 22);
            toolStripLabel1.Text = "Big Type:";
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Alignment = ToolStripItemAlignment.Right;
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 25);
            // 
            // CompressionButton
            // 
            CompressionButton.Alignment = ToolStripItemAlignment.Right;
            CompressionButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            CompressionButton.Image = (Image)resources.GetObject("CompressionButton.Image");
            CompressionButton.ImageTransparentColor = Color.Magenta;
            CompressionButton.Name = "CompressionButton";
            CompressionButton.Size = new Size(139, 22);
            CompressionButton.Text = "Compressed Build: False";
            CompressionButton.Click += CompressionButton_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Alignment = ToolStripItemAlignment.Right;
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 25);
            // 
            // toolStripButton1
            // 
            toolStripButton1.Alignment = ToolStripItemAlignment.Right;
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(98, 22);
            toolStripButton1.Text = "toolStripButton1";
            toolStripButton1.Visible = false;
            toolStripButton1.Click += toolStripButton1_Click;
            // 
            // BigArchiveTool
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 444);
            Controls.Add(toolStrip1);
            Controls.Add(BigDataView);
            Name = "BigArchiveTool";
            Text = "Big Archive";
            ((System.ComponentModel.ISupportInitialize)BigDataView).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private ToolStripButton CompressionButton;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton toolStripButton1;
    }
}