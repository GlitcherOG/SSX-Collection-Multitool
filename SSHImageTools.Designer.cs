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
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.ColourAmountLabel = new System.Windows.Forms.Label();
            this.ColourByteSwappedCheckbox = new System.Windows.Forms.CheckBox();
            this.button11 = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.button12 = new System.Windows.Forms.Button();
            this.ColourAlphaFix = new System.Windows.Forms.CheckBox();
            this.MetalAlphaCheckbox = new System.Windows.Forms.CheckBox();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.BlackDisplayCheckBox = new System.Windows.Forms.CheckBox();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
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
            // 
            // XAxisNum
            // 
            this.XAxisNum.Location = new System.Drawing.Point(11, 82);
            this.XAxisNum.Name = "XAxisNum";
            this.XAxisNum.Size = new System.Drawing.Size(112, 23);
            this.XAxisNum.TabIndex = 26;
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
            // 
            // ImageShortNameTextbox
            // 
            this.ImageShortNameTextbox.Location = new System.Drawing.Point(11, 38);
            this.ImageShortNameTextbox.MaxLength = 4;
            this.ImageShortNameTextbox.Name = "ImageShortNameTextbox";
            this.ImageShortNameTextbox.Size = new System.Drawing.Size(112, 23);
            this.ImageShortNameTextbox.TabIndex = 4;
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
            this.groupBox3.Controls.Add(this.button10);
            this.groupBox3.Controls.Add(this.button9);
            this.groupBox3.Controls.Add(this.ColourAmountLabel);
            this.groupBox3.Controls.Add(this.ColourByteSwappedCheckbox);
            this.groupBox3.Controls.Add(this.button11);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.button12);
            this.groupBox3.Controls.Add(this.ColourAlphaFix);
            this.groupBox3.Controls.Add(this.MetalAlphaCheckbox);
            this.groupBox3.Controls.Add(this.button8);
            this.groupBox3.Controls.Add(this.button7);
            this.groupBox3.Controls.Add(this.button5);
            this.groupBox3.Controls.Add(this.button6);
            this.groupBox3.Location = new System.Drawing.Point(192, 252);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(521, 169);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Colour Table Info";
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(118, 138);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(90, 23);
            this.button10.TabIndex = 22;
            this.button10.Text = "Half Alpha";
            this.button10.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(118, 109);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(90, 23);
            this.button9.TabIndex = 23;
            this.button9.Text = "Double Alpha";
            this.button9.UseVisualStyleBackColor = true;
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
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(11, 138);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(101, 23);
            this.button11.TabIndex = 21;
            this.button11.Text = "Half Colours";
            this.button11.UseVisualStyleBackColor = true;
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
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(11, 109);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(101, 23);
            this.button12.TabIndex = 20;
            this.button12.Text = "Double Colours";
            this.button12.UseVisualStyleBackColor = true;
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
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(427, 72);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(89, 23);
            this.button8.TabIndex = 19;
            this.button8.Text = "Both Import";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(331, 72);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(90, 23);
            this.button7.TabIndex = 18;
            this.button7.Text = "Both Extract";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(331, 43);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(90, 23);
            this.button5.TabIndex = 16;
            this.button5.Text = "Metal Extract";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(427, 43);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(89, 23);
            this.button6.TabIndex = 17;
            this.button6.Text = "Metal Import";
            this.button6.UseVisualStyleBackColor = true;
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
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(12, 538);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "/\\";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(101, 538);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(85, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "\\/";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(12, 567);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(83, 23);
            this.button3.TabIndex = 14;
            this.button3.Text = "Add";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.Location = new System.Drawing.Point(101, 567);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(85, 23);
            this.button4.TabIndex = 15;
            this.button4.Text = "Remove";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button14
            // 
            this.button14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button14.Location = new System.Drawing.Point(719, 567);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(93, 23);
            this.button14.TabIndex = 16;
            this.button14.Text = "Replace Image";
            this.button14.UseVisualStyleBackColor = true;
            // 
            // button15
            // 
            this.button15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button15.Location = new System.Drawing.Point(1122, 567);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(97, 23);
            this.button15.TabIndex = 17;
            this.button15.Text = "Export Image";
            this.button15.UseVisualStyleBackColor = true;
            // 
            // SSHImageTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1231, 599);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.PictureBox1);
            this.Controls.Add(this.BlackDisplayCheckBox);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.ImageList);
            this.Name = "SSHImageTools";
            this.Text = "SSHImageTools";
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
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private Button button9;
        private Button button10;
        private Button button11;
        private Button button12;
        private TextBox GimxVersionTextBox;
        private Label ImageSizeLabel;
        private TextBox ImageShortNameTextbox;
        private TextBox ImageLongNameTextbox;
        private ComboBox MatrixTypeDropdown;
        private Label ColourAmountLabel;
        private CheckBox ColourByteSwappedCheckbox;
        private Label label12;
        private Button button14;
        private Button button15;
        private NumericUpDown YAxisNum;
        private NumericUpDown XAxisNum;
    }
}