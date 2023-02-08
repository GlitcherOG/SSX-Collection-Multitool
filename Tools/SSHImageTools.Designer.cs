namespace SSXMultiTool
{
    partial class SSHImageTools
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SSHImageTools));
            this.ImageList = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.LoadSSHButton = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadFolderButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExportAllButton = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveSSHButton = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.GimxVersionTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.FileNameLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.YAxisNum = new System.Windows.Forms.NumericUpDown();
            this.XAxisNum = new System.Windows.Forms.NumericUpDown();
            this.ImageSizeLabel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ImageByteSwappedCheckbox = new System.Windows.Forms.CheckBox();
            this.MatrixTypeDropdown = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ImageLongNameTextbox = new System.Windows.Forms.TextBox();
            this.ImageShortNameTextbox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.HalfAlphaButton = new System.Windows.Forms.Button();
            this.DoubleAlphaButton = new System.Windows.Forms.Button();
            this.ColourAmountLabel = new System.Windows.Forms.Label();
            this.ColourByteSwappedCheckbox = new System.Windows.Forms.CheckBox();
            this.HalfColourButton = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.DoubleColourButton = new System.Windows.Forms.Button();
            this.ColourAlphaFix = new System.Windows.Forms.CheckBox();
            this.MetalAlphaCheckbox = new System.Windows.Forms.CheckBox();
            this.SSHBothImport = new System.Windows.Forms.Button();
            this.SSHBothExtract = new System.Windows.Forms.Button();
            this.SSHMetalExtract = new System.Windows.Forms.Button();
            this.SSHMetalImport = new System.Windows.Forms.Button();
            this.BlackDisplayCheckBox = new System.Windows.Forms.CheckBox();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.ImageMoveUpButton = new System.Windows.Forms.Button();
            this.ImageMoveDownButton = new System.Windows.Forms.Button();
            this.IamgeAddButton = new System.Windows.Forms.Button();
            this.ImageRemoveButton = new System.Windows.Forms.Button();
            this.ReplaceImageButton = new System.Windows.Forms.Button();
            this.ExportImageButton = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.YAxisNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XAxisNum)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ImageList
            // 
            this.ImageList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ImageList.FormattingEnabled = true;
            this.ImageList.ItemHeight = 15;
            this.ImageList.Location = new System.Drawing.Point(12, 28);
            this.ImageList.Name = "ImageList";
            this.ImageList.Size = new System.Drawing.Size(174, 499);
            this.ImageList.TabIndex = 0;
            this.ImageList.SelectedIndexChanged += new System.EventHandler(this.ImageList_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1231, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadSSHButton,
            this.LoadFolderButton,
            this.toolStripSeparator1,
            this.ExportAllButton,
            this.SaveSSHButton});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(38, 22);
            this.toolStripDropDownButton1.Text = "File";
            // 
            // LoadSSHButton
            // 
            this.LoadSSHButton.Name = "LoadSSHButton";
            this.LoadSSHButton.Size = new System.Drawing.Size(167, 22);
            this.LoadSSHButton.Text = "Load .SSH";
            this.LoadSSHButton.Click += new System.EventHandler(this.LoadSSHButton_Click);
            // 
            // LoadFolderButton
            // 
            this.LoadFolderButton.Name = "LoadFolderButton";
            this.LoadFolderButton.Size = new System.Drawing.Size(167, 22);
            this.LoadFolderButton.Text = "Load Folder";
            this.LoadFolderButton.Click += new System.EventHandler(this.LoadFolderButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(164, 6);
            // 
            // ExportAllButton
            // 
            this.ExportAllButton.Name = "ExportAllButton";
            this.ExportAllButton.Size = new System.Drawing.Size(167, 22);
            this.ExportAllButton.Text = "Export All To PNG";
            this.ExportAllButton.Click += new System.EventHandler(this.ExportAllButton_Click);
            // 
            // SaveSSHButton
            // 
            this.SaveSSHButton.Name = "SaveSSHButton";
            this.SaveSSHButton.Size = new System.Drawing.Size(167, 22);
            this.SaveSSHButton.Text = "Save .SSH";
            this.SaveSSHButton.Click += new System.EventHandler(this.SaveSSHButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.GimxVersionTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.FileNameLabel);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(192, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(521, 69);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File Details";
            // 
            // GimxVersionTextBox
            // 
            this.GimxVersionTextBox.Location = new System.Drawing.Point(93, 35);
            this.GimxVersionTextBox.MaxLength = 4;
            this.GimxVersionTextBox.Name = "GimxVersionTextBox";
            this.GimxVersionTextBox.Size = new System.Drawing.Size(125, 23);
            this.GimxVersionTextBox.TabIndex = 3;
            this.GimxVersionTextBox.TextChanged += new System.EventHandler(this.GimxVersionTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(93, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Gimex Version";
            // 
            // FileNameLabel
            // 
            this.FileNameLabel.AutoSize = true;
            this.FileNameLabel.Location = new System.Drawing.Point(11, 35);
            this.FileNameLabel.Name = "FileNameLabel";
            this.FileNameLabel.Size = new System.Drawing.Size(36, 15);
            this.FileNameLabel.TabIndex = 1;
            this.FileNameLabel.Text = "None";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "File Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.YAxisNum);
            this.groupBox2.Controls.Add(this.XAxisNum);
            this.groupBox2.Controls.Add(this.ImageSizeLabel);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.ImageByteSwappedCheckbox);
            this.groupBox2.Controls.Add(this.MatrixTypeDropdown);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.ImageLongNameTextbox);
            this.groupBox2.Controls.Add(this.ImageShortNameTextbox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(192, 103);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(521, 143);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Image Details";
            // 
            // YAxisNum
            // 
            this.YAxisNum.Location = new System.Drawing.Point(129, 82);
            this.YAxisNum.Name = "YAxisNum";
            this.YAxisNum.Size = new System.Drawing.Size(112, 23);
            this.YAxisNum.TabIndex = 27;
            this.YAxisNum.ValueChanged += new System.EventHandler(this.UpdateImageDetails);
            // 
            // XAxisNum
            // 
            this.XAxisNum.Location = new System.Drawing.Point(11, 82);
            this.XAxisNum.Name = "XAxisNum";
            this.XAxisNum.Size = new System.Drawing.Size(112, 23);
            this.XAxisNum.TabIndex = 26;
            this.XAxisNum.ValueChanged += new System.EventHandler(this.UpdateImageDetails);
            // 
            // ImageSizeLabel
            // 
            this.ImageSizeLabel.AutoSize = true;
            this.ImageSizeLabel.Location = new System.Drawing.Point(331, 82);
            this.ImageSizeLabel.Name = "ImageSizeLabel";
            this.ImageSizeLabel.Size = new System.Drawing.Size(31, 15);
            this.ImageSizeLabel.TabIndex = 7;
            this.ImageSizeLabel.Text = "0 x 0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(331, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 15);
            this.label9.TabIndex = 5;
            this.label9.Text = "Image Size";
            // 
            // ImageByteSwappedCheckbox
            // 
            this.ImageByteSwappedCheckbox.AutoSize = true;
            this.ImageByteSwappedCheckbox.Location = new System.Drawing.Point(415, 118);
            this.ImageByteSwappedCheckbox.Name = "ImageByteSwappedCheckbox";
            this.ImageByteSwappedCheckbox.Size = new System.Drawing.Size(100, 19);
            this.ImageByteSwappedCheckbox.TabIndex = 6;
            this.ImageByteSwappedCheckbox.Text = "Byte Swapped";
            this.ImageByteSwappedCheckbox.UseVisualStyleBackColor = true;
            this.ImageByteSwappedCheckbox.CheckStateChanged += new System.EventHandler(this.UpdateImageDetails);
            // 
            // MatrixTypeDropdown
            // 
            this.MatrixTypeDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MatrixTypeDropdown.FormattingEnabled = true;
            this.MatrixTypeDropdown.Items.AddRange(new object[] {
            "1 (4 Bit, 16 Colour Index)",
            "2 (8 Bit, 256 Colour Index)",
            "5 (Full Colour)",
            "130 (8 bit, 256 Colour Index Compressed)"});
            this.MatrixTypeDropdown.Location = new System.Drawing.Point(331, 38);
            this.MatrixTypeDropdown.Name = "MatrixTypeDropdown";
            this.MatrixTypeDropdown.Size = new System.Drawing.Size(184, 23);
            this.MatrixTypeDropdown.TabIndex = 25;
            this.MatrixTypeDropdown.SelectedIndexChanged += new System.EventHandler(this.UpdateImageDetails);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 15);
            this.label8.TabIndex = 4;
            this.label8.Text = "X-Axis Centre";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(129, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 15);
            this.label7.TabIndex = 3;
            this.label7.Text = "Y-Axis Centre";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Image Short Name";
            // 
            // ImageLongNameTextbox
            // 
            this.ImageLongNameTextbox.Location = new System.Drawing.Point(129, 38);
            this.ImageLongNameTextbox.Name = "ImageLongNameTextbox";
            this.ImageLongNameTextbox.Size = new System.Drawing.Size(196, 23);
            this.ImageLongNameTextbox.TabIndex = 24;
            this.ImageLongNameTextbox.TextChanged += new System.EventHandler(this.UpdateImageDetails);
            // 
            // ImageShortNameTextbox
            // 
            this.ImageShortNameTextbox.Location = new System.Drawing.Point(11, 38);
            this.ImageShortNameTextbox.MaxLength = 4;
            this.ImageShortNameTextbox.Name = "ImageShortNameTextbox";
            this.ImageShortNameTextbox.Size = new System.Drawing.Size(112, 23);
            this.ImageShortNameTextbox.TabIndex = 4;
            this.ImageShortNameTextbox.TextChanged += new System.EventHandler(this.UpdateImageDetails);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(129, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "Image Long Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(331, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "Matrix Type";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.HalfAlphaButton);
            this.groupBox3.Controls.Add(this.DoubleAlphaButton);
            this.groupBox3.Controls.Add(this.ColourAmountLabel);
            this.groupBox3.Controls.Add(this.ColourByteSwappedCheckbox);
            this.groupBox3.Controls.Add(this.HalfColourButton);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.DoubleColourButton);
            this.groupBox3.Controls.Add(this.ColourAlphaFix);
            this.groupBox3.Controls.Add(this.MetalAlphaCheckbox);
            this.groupBox3.Controls.Add(this.SSHBothImport);
            this.groupBox3.Controls.Add(this.SSHBothExtract);
            this.groupBox3.Controls.Add(this.SSHMetalExtract);
            this.groupBox3.Controls.Add(this.SSHMetalImport);
            this.groupBox3.Location = new System.Drawing.Point(192, 252);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(521, 169);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Colour Table Info";
            // 
            // HalfAlphaButton
            // 
            this.HalfAlphaButton.Location = new System.Drawing.Point(118, 138);
            this.HalfAlphaButton.Name = "HalfAlphaButton";
            this.HalfAlphaButton.Size = new System.Drawing.Size(90, 23);
            this.HalfAlphaButton.TabIndex = 22;
            this.HalfAlphaButton.Text = "Half Alpha";
            this.HalfAlphaButton.UseVisualStyleBackColor = true;
            this.HalfAlphaButton.Click += new System.EventHandler(this.HalfAlphaButton_Click);
            // 
            // DoubleAlphaButton
            // 
            this.DoubleAlphaButton.Location = new System.Drawing.Point(118, 109);
            this.DoubleAlphaButton.Name = "DoubleAlphaButton";
            this.DoubleAlphaButton.Size = new System.Drawing.Size(90, 23);
            this.DoubleAlphaButton.TabIndex = 23;
            this.DoubleAlphaButton.Text = "Double Alpha";
            this.DoubleAlphaButton.UseVisualStyleBackColor = true;
            this.DoubleAlphaButton.Click += new System.EventHandler(this.DoubleAlphaButton_Click);
            // 
            // ColourAmountLabel
            // 
            this.ColourAmountLabel.AutoSize = true;
            this.ColourAmountLabel.Location = new System.Drawing.Point(11, 37);
            this.ColourAmountLabel.Name = "ColourAmountLabel";
            this.ColourAmountLabel.Size = new System.Drawing.Size(13, 15);
            this.ColourAmountLabel.TabIndex = 29;
            this.ColourAmountLabel.Text = "0";
            // 
            // ColourByteSwappedCheckbox
            // 
            this.ColourByteSwappedCheckbox.AutoSize = true;
            this.ColourByteSwappedCheckbox.Location = new System.Drawing.Point(415, 144);
            this.ColourByteSwappedCheckbox.Name = "ColourByteSwappedCheckbox";
            this.ColourByteSwappedCheckbox.Size = new System.Drawing.Size(100, 19);
            this.ColourByteSwappedCheckbox.TabIndex = 28;
            this.ColourByteSwappedCheckbox.Text = "Byte Swapped";
            this.ColourByteSwappedCheckbox.UseVisualStyleBackColor = true;
            this.ColourByteSwappedCheckbox.CheckedChanged += new System.EventHandler(this.UpdateImageDetails);
            this.ColourByteSwappedCheckbox.CheckStateChanged += new System.EventHandler(this.UpdateImageDetails);
            // 
            // HalfColourButton
            // 
            this.HalfColourButton.Location = new System.Drawing.Point(11, 138);
            this.HalfColourButton.Name = "HalfColourButton";
            this.HalfColourButton.Size = new System.Drawing.Size(101, 23);
            this.HalfColourButton.TabIndex = 21;
            this.HalfColourButton.Text = "Half Colours";
            this.HalfColourButton.UseVisualStyleBackColor = true;
            this.HalfColourButton.Click += new System.EventHandler(this.HalfColourButton_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(11, 19);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(101, 15);
            this.label12.TabIndex = 28;
            this.label12.Text = "Colour Ammount";
            // 
            // DoubleColourButton
            // 
            this.DoubleColourButton.Location = new System.Drawing.Point(11, 109);
            this.DoubleColourButton.Name = "DoubleColourButton";
            this.DoubleColourButton.Size = new System.Drawing.Size(101, 23);
            this.DoubleColourButton.TabIndex = 20;
            this.DoubleColourButton.Text = "Double Colours";
            this.DoubleColourButton.UseVisualStyleBackColor = true;
            this.DoubleColourButton.Click += new System.EventHandler(this.DoubleColourButton_Click);
            // 
            // ColourAlphaFix
            // 
            this.ColourAlphaFix.AutoSize = true;
            this.ColourAlphaFix.Location = new System.Drawing.Point(11, 84);
            this.ColourAlphaFix.Name = "ColourAlphaFix";
            this.ColourAlphaFix.Size = new System.Drawing.Size(75, 19);
            this.ColourAlphaFix.TabIndex = 7;
            this.ColourAlphaFix.Text = "Alpha Fix";
            this.ColourAlphaFix.UseVisualStyleBackColor = true;
            this.ColourAlphaFix.CheckStateChanged += new System.EventHandler(this.UpdateImageDetails);
            // 
            // MetalAlphaCheckbox
            // 
            this.MetalAlphaCheckbox.AutoSize = true;
            this.MetalAlphaCheckbox.Location = new System.Drawing.Point(331, 18);
            this.MetalAlphaCheckbox.Name = "MetalAlphaCheckbox";
            this.MetalAlphaCheckbox.Size = new System.Drawing.Size(90, 19);
            this.MetalAlphaCheckbox.TabIndex = 8;
            this.MetalAlphaCheckbox.Text = "Metal Alpha";
            this.MetalAlphaCheckbox.UseVisualStyleBackColor = true;
            this.MetalAlphaCheckbox.CheckStateChanged += new System.EventHandler(this.UpdateImageDetails);
            // 
            // SSHBothImport
            // 
            this.SSHBothImport.Location = new System.Drawing.Point(427, 66);
            this.SSHBothImport.Name = "SSHBothImport";
            this.SSHBothImport.Size = new System.Drawing.Size(89, 23);
            this.SSHBothImport.TabIndex = 19;
            this.SSHBothImport.Text = "Both Import";
            this.SSHBothImport.UseVisualStyleBackColor = true;
            this.SSHBothImport.Click += new System.EventHandler(this.SSHBothImport_Click);
            // 
            // SSHBothExtract
            // 
            this.SSHBothExtract.Location = new System.Drawing.Point(331, 66);
            this.SSHBothExtract.Name = "SSHBothExtract";
            this.SSHBothExtract.Size = new System.Drawing.Size(90, 23);
            this.SSHBothExtract.TabIndex = 18;
            this.SSHBothExtract.Text = "Both Extract";
            this.SSHBothExtract.UseVisualStyleBackColor = true;
            this.SSHBothExtract.Click += new System.EventHandler(this.SSHBothExtract_Click);
            // 
            // SSHMetalExtract
            // 
            this.SSHMetalExtract.Location = new System.Drawing.Point(331, 37);
            this.SSHMetalExtract.Name = "SSHMetalExtract";
            this.SSHMetalExtract.Size = new System.Drawing.Size(90, 23);
            this.SSHMetalExtract.TabIndex = 16;
            this.SSHMetalExtract.Text = "Metal Extract";
            this.SSHMetalExtract.UseVisualStyleBackColor = true;
            this.SSHMetalExtract.Click += new System.EventHandler(this.SSHMetalExtract_Click);
            // 
            // SSHMetalImport
            // 
            this.SSHMetalImport.Location = new System.Drawing.Point(427, 37);
            this.SSHMetalImport.Name = "SSHMetalImport";
            this.SSHMetalImport.Size = new System.Drawing.Size(89, 23);
            this.SSHMetalImport.TabIndex = 17;
            this.SSHMetalImport.Text = "Metal Import";
            this.SSHMetalImport.UseVisualStyleBackColor = true;
            this.SSHMetalImport.Click += new System.EventHandler(this.SSHMetalImport_Click);
            // 
            // BlackDisplayCheckBox
            // 
            this.BlackDisplayCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BlackDisplayCheckBox.AutoSize = true;
            this.BlackDisplayCheckBox.Checked = true;
            this.BlackDisplayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BlackDisplayCheckBox.Location = new System.Drawing.Point(719, 533);
            this.BlackDisplayCheckBox.Name = "BlackDisplayCheckBox";
            this.BlackDisplayCheckBox.Size = new System.Drawing.Size(198, 19);
            this.BlackDisplayCheckBox.TabIndex = 10;
            this.BlackDisplayCheckBox.Text = "Black Background (Display Only)";
            this.BlackDisplayCheckBox.UseVisualStyleBackColor = true;
            this.BlackDisplayCheckBox.CheckedChanged += new System.EventHandler(this.BlackDisplayCheckBox_CheckedChanged);
            // 
            // PictureBox1
            // 
            this.PictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PictureBox1.Location = new System.Drawing.Point(719, 27);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(500, 500);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox1.TabIndex = 11;
            this.PictureBox1.TabStop = false;
            // 
            // ImageMoveUpButton
            // 
            this.ImageMoveUpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ImageMoveUpButton.Location = new System.Drawing.Point(12, 538);
            this.ImageMoveUpButton.Name = "ImageMoveUpButton";
            this.ImageMoveUpButton.Size = new System.Drawing.Size(83, 23);
            this.ImageMoveUpButton.TabIndex = 12;
            this.ImageMoveUpButton.Text = "/\\";
            this.ImageMoveUpButton.UseVisualStyleBackColor = true;
            this.ImageMoveUpButton.Click += new System.EventHandler(this.ImageMoveUpButton_Click);
            // 
            // ImageMoveDownButton
            // 
            this.ImageMoveDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ImageMoveDownButton.Location = new System.Drawing.Point(101, 538);
            this.ImageMoveDownButton.Name = "ImageMoveDownButton";
            this.ImageMoveDownButton.Size = new System.Drawing.Size(85, 23);
            this.ImageMoveDownButton.TabIndex = 13;
            this.ImageMoveDownButton.Text = "\\/";
            this.ImageMoveDownButton.UseVisualStyleBackColor = true;
            this.ImageMoveDownButton.Click += new System.EventHandler(this.ImageMoveDownButton_Click);
            // 
            // IamgeAddButton
            // 
            this.IamgeAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.IamgeAddButton.Location = new System.Drawing.Point(12, 567);
            this.IamgeAddButton.Name = "IamgeAddButton";
            this.IamgeAddButton.Size = new System.Drawing.Size(83, 23);
            this.IamgeAddButton.TabIndex = 14;
            this.IamgeAddButton.Text = "Add";
            this.IamgeAddButton.UseVisualStyleBackColor = true;
            this.IamgeAddButton.Click += new System.EventHandler(this.IamgeAddButton_Click);
            // 
            // ImageRemoveButton
            // 
            this.ImageRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ImageRemoveButton.Location = new System.Drawing.Point(101, 567);
            this.ImageRemoveButton.Name = "ImageRemoveButton";
            this.ImageRemoveButton.Size = new System.Drawing.Size(85, 23);
            this.ImageRemoveButton.TabIndex = 15;
            this.ImageRemoveButton.Text = "Remove";
            this.ImageRemoveButton.UseVisualStyleBackColor = true;
            this.ImageRemoveButton.Click += new System.EventHandler(this.ImageRemoveButton_Click);
            // 
            // ReplaceImageButton
            // 
            this.ReplaceImageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ReplaceImageButton.Location = new System.Drawing.Point(719, 567);
            this.ReplaceImageButton.Name = "ReplaceImageButton";
            this.ReplaceImageButton.Size = new System.Drawing.Size(93, 23);
            this.ReplaceImageButton.TabIndex = 16;
            this.ReplaceImageButton.Text = "Replace Image";
            this.ReplaceImageButton.UseVisualStyleBackColor = true;
            this.ReplaceImageButton.Click += new System.EventHandler(this.ReplaceImageButton_Click);
            // 
            // ExportImageButton
            // 
            this.ExportImageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportImageButton.Location = new System.Drawing.Point(1122, 567);
            this.ExportImageButton.Name = "ExportImageButton";
            this.ExportImageButton.Size = new System.Drawing.Size(97, 23);
            this.ExportImageButton.TabIndex = 17;
            this.ExportImageButton.Text = "Export Image";
            this.ExportImageButton.UseVisualStyleBackColor = true;
            this.ExportImageButton.Click += new System.EventHandler(this.ExportImageButton_Click);
            // 
            // SSHImageTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1231, 599);
            this.Controls.Add(this.ExportImageButton);
            this.Controls.Add(this.ReplaceImageButton);
            this.Controls.Add(this.ImageRemoveButton);
            this.Controls.Add(this.IamgeAddButton);
            this.Controls.Add(this.ImageMoveDownButton);
            this.Controls.Add(this.ImageMoveUpButton);
            this.Controls.Add(this.PictureBox1);
            this.Controls.Add(this.BlackDisplayCheckBox);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.ImageList);
            this.Name = "SSHImageTools";
            this.Text = "SSH Image Editor";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.YAxisNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XAxisNum)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox ImageList;
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem LoadSSHButton;
        private ToolStripMenuItem LoadFolderButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem ExportAllButton;
        private ToolStripMenuItem SaveSSHButton;
        private GroupBox groupBox1;
        private Label label3;
        private Label FileNameLabel;
        private Label label1;
        private GroupBox groupBox2;
        private CheckBox ImageByteSwappedCheckbox;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private GroupBox groupBox3;
        private CheckBox MetalAlphaCheckbox;
        private CheckBox ColourAlphaFix;
        private CheckBox BlackDisplayCheckBox;
        private PictureBox PictureBox1;
        private Button ImageMoveUpButton;
        private Button ImageMoveDownButton;
        private Button IamgeAddButton;
        private Button ImageRemoveButton;
        private Button SSHMetalExtract;
        private Button SSHMetalImport;
        private Button SSHBothExtract;
        private Button SSHBothImport;
        private Button DoubleAlphaButton;
        private Button HalfAlphaButton;
        private Button HalfColourButton;
        private Button DoubleColourButton;
        private TextBox GimxVersionTextBox;
        private Label ImageSizeLabel;
        private TextBox ImageShortNameTextbox;
        private TextBox ImageLongNameTextbox;
        private ComboBox MatrixTypeDropdown;
        private Label ColourAmountLabel;
        private CheckBox ColourByteSwappedCheckbox;
        private Label label12;
        private Button ReplaceImageButton;
        private Button ExportImageButton;
        private NumericUpDown YAxisNum;
        private NumericUpDown XAxisNum;
    }
}