namespace SSXMultiTool
{
    partial class NewSSHImageTools
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewSSHImageTools));
            ImageList = new ListBox();
            toolStrip1 = new ToolStrip();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            LoadSSHButton = new ToolStripMenuItem();
            LoadPNGFolderButton = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            ExportAllButton = new ToolStripMenuItem();
            SaveSSHButton = new ToolStripMenuItem();
            groupBox1 = new GroupBox();
            GimxVersionTextBox = new TextBox();
            label3 = new Label();
            FileNameLabel = new Label();
            label1 = new Label();
            groupBox2 = new GroupBox();
            YAxisNum = new NumericUpDown();
            XAxisNum = new NumericUpDown();
            ImageSizeLabel = new Label();
            label9 = new Label();
            ImageByteSwappedCheckbox = new CheckBox();
            MatrixTypeDropdown = new ComboBox();
            label8 = new Label();
            label7 = new Label();
            label4 = new Label();
            ImageShortNameTextbox = new TextBox();
            label6 = new Label();
            groupBox3 = new GroupBox();
            checkBox1 = new CheckBox();
            HalfAlphaButton = new Button();
            DoubleAlphaButton = new Button();
            ColourAmountLabel = new Label();
            ColourByteSwappedCheckbox = new CheckBox();
            HalfColourButton = new Button();
            label12 = new Label();
            DoubleColourButton = new Button();
            ColourAlphaFix = new CheckBox();
            MetalAlphaCheckbox = new CheckBox();
            SSHBothImport = new Button();
            SSHBothExtract = new Button();
            SSHMetalExtract = new Button();
            SSHMetalImport = new Button();
            BlackDisplayCheckBox = new CheckBox();
            PictureBox1 = new PictureBox();
            ImageMoveUpButton = new Button();
            ImageMoveDownButton = new Button();
            IamgeAddButton = new Button();
            ImageRemoveButton = new Button();
            ReplaceImageButton = new Button();
            ExportImageButton = new Button();
            toolStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)YAxisNum).BeginInit();
            ((System.ComponentModel.ISupportInitialize)XAxisNum).BeginInit();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBox1).BeginInit();
            SuspendLayout();
            // 
            // ImageList
            // 
            ImageList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            ImageList.FormattingEnabled = true;
            ImageList.Location = new Point(12, 28);
            ImageList.Name = "ImageList";
            ImageList.Size = new Size(174, 499);
            ImageList.TabIndex = 0;
            ImageList.SelectedIndexChanged += ImageList_SelectedIndexChanged;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1231, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { LoadSSHButton, LoadPNGFolderButton, toolStripMenuItem1, toolStripSeparator1, ExportAllButton, SaveSSHButton });
            toolStripDropDownButton1.Image = (Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new Size(38, 22);
            toolStripDropDownButton1.Text = "File";
            // 
            // LoadSSHButton
            // 
            LoadSSHButton.Name = "LoadSSHButton";
            LoadSSHButton.Size = new Size(172, 22);
            LoadSSHButton.Text = "Load .SSH";
            LoadSSHButton.Click += LoadSSHButton_Click;
            // 
            // LoadPNGFolderButton
            // 
            LoadPNGFolderButton.Name = "LoadPNGFolderButton";
            LoadPNGFolderButton.Size = new Size(172, 22);
            LoadPNGFolderButton.Text = "Load PNG Folder";
            LoadPNGFolderButton.Click += LoadFolderButton_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(172, 22);
            toolStripMenuItem1.Text = "Load Export Folder";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(169, 6);
            // 
            // ExportAllButton
            // 
            ExportAllButton.Name = "ExportAllButton";
            ExportAllButton.Size = new Size(172, 22);
            ExportAllButton.Text = "Export All To PNG";
            ExportAllButton.Click += ExportAllButton_Click;
            // 
            // SaveSSHButton
            // 
            SaveSSHButton.Name = "SaveSSHButton";
            SaveSSHButton.Size = new Size(172, 22);
            SaveSSHButton.Text = "Save .SSH";
            SaveSSHButton.Click += SaveSSHButton_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(GimxVersionTextBox);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(FileNameLabel);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(192, 28);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(521, 69);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "File Details";
            // 
            // GimxVersionTextBox
            // 
            GimxVersionTextBox.Location = new Point(93, 35);
            GimxVersionTextBox.MaxLength = 4;
            GimxVersionTextBox.Name = "GimxVersionTextBox";
            GimxVersionTextBox.Size = new Size(125, 23);
            GimxVersionTextBox.TabIndex = 3;
            GimxVersionTextBox.TextChanged += GimxVersionTextBox_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(93, 19);
            label3.Name = "label3";
            label3.Size = new Size(81, 15);
            label3.TabIndex = 2;
            label3.Text = "Gimex Version";
            // 
            // FileNameLabel
            // 
            FileNameLabel.AutoSize = true;
            FileNameLabel.Location = new Point(11, 35);
            FileNameLabel.Name = "FileNameLabel";
            FileNameLabel.Size = new Size(36, 15);
            FileNameLabel.TabIndex = 1;
            FileNameLabel.Text = "None";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(11, 19);
            label1.Name = "label1";
            label1.Size = new Size(60, 15);
            label1.TabIndex = 0;
            label1.Text = "File Name";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(YAxisNum);
            groupBox2.Controls.Add(XAxisNum);
            groupBox2.Controls.Add(ImageSizeLabel);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(ImageByteSwappedCheckbox);
            groupBox2.Controls.Add(MatrixTypeDropdown);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(ImageShortNameTextbox);
            groupBox2.Controls.Add(label6);
            groupBox2.Location = new Point(192, 103);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(521, 143);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "Image Details";
            // 
            // YAxisNum
            // 
            YAxisNum.Location = new Point(129, 82);
            YAxisNum.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            YAxisNum.Name = "YAxisNum";
            YAxisNum.Size = new Size(112, 23);
            YAxisNum.TabIndex = 27;
            YAxisNum.ValueChanged += UpdateImageDetails;
            // 
            // XAxisNum
            // 
            XAxisNum.Location = new Point(11, 82);
            XAxisNum.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            XAxisNum.Name = "XAxisNum";
            XAxisNum.Size = new Size(112, 23);
            XAxisNum.TabIndex = 26;
            XAxisNum.ValueChanged += UpdateImageDetails;
            // 
            // ImageSizeLabel
            // 
            ImageSizeLabel.AutoSize = true;
            ImageSizeLabel.Location = new Point(331, 82);
            ImageSizeLabel.Name = "ImageSizeLabel";
            ImageSizeLabel.Size = new Size(30, 15);
            ImageSizeLabel.TabIndex = 7;
            ImageSizeLabel.Text = "0 x 0";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(331, 64);
            label9.Name = "label9";
            label9.Size = new Size(63, 15);
            label9.TabIndex = 5;
            label9.Text = "Image Size";
            // 
            // ImageByteSwappedCheckbox
            // 
            ImageByteSwappedCheckbox.AutoSize = true;
            ImageByteSwappedCheckbox.Location = new Point(415, 118);
            ImageByteSwappedCheckbox.Name = "ImageByteSwappedCheckbox";
            ImageByteSwappedCheckbox.Size = new Size(100, 19);
            ImageByteSwappedCheckbox.TabIndex = 6;
            ImageByteSwappedCheckbox.Text = "Byte Swapped";
            ImageByteSwappedCheckbox.UseVisualStyleBackColor = true;
            ImageByteSwappedCheckbox.CheckStateChanged += UpdateImageDetails;
            // 
            // MatrixTypeDropdown
            // 
            MatrixTypeDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            MatrixTypeDropdown.FormattingEnabled = true;
            MatrixTypeDropdown.Items.AddRange(new object[] { "1 (4 Bit, 16 Colour Index)", "2 (8 Bit, 256 Colour Index)", "5 (Full Colour)" });
            MatrixTypeDropdown.Location = new Point(331, 38);
            MatrixTypeDropdown.Name = "MatrixTypeDropdown";
            MatrixTypeDropdown.Size = new Size(184, 23);
            MatrixTypeDropdown.TabIndex = 25;
            MatrixTypeDropdown.SelectedIndexChanged += UpdateImageDetails;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(11, 64);
            label8.Name = "label8";
            label8.Size = new Size(78, 15);
            label8.TabIndex = 4;
            label8.Text = "X-Axis Centre";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(129, 64);
            label7.Name = "label7";
            label7.Size = new Size(78, 15);
            label7.TabIndex = 3;
            label7.Text = "Y-Axis Centre";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(8, 20);
            label4.Name = "label4";
            label4.Size = new Size(106, 15);
            label4.TabIndex = 0;
            label4.Text = "Image Short Name";
            // 
            // ImageShortNameTextbox
            // 
            ImageShortNameTextbox.Location = new Point(11, 38);
            ImageShortNameTextbox.MaxLength = 4;
            ImageShortNameTextbox.Name = "ImageShortNameTextbox";
            ImageShortNameTextbox.Size = new Size(112, 23);
            ImageShortNameTextbox.TabIndex = 4;
            ImageShortNameTextbox.TextChanged += UpdateImageDetails;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(331, 19);
            label6.Name = "label6";
            label6.Size = new Size(68, 15);
            label6.TabIndex = 2;
            label6.Text = "Matrix Type";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(checkBox1);
            groupBox3.Controls.Add(HalfAlphaButton);
            groupBox3.Controls.Add(DoubleAlphaButton);
            groupBox3.Controls.Add(ColourAmountLabel);
            groupBox3.Controls.Add(ColourByteSwappedCheckbox);
            groupBox3.Controls.Add(HalfColourButton);
            groupBox3.Controls.Add(label12);
            groupBox3.Controls.Add(DoubleColourButton);
            groupBox3.Controls.Add(ColourAlphaFix);
            groupBox3.Controls.Add(MetalAlphaCheckbox);
            groupBox3.Controls.Add(SSHBothImport);
            groupBox3.Controls.Add(SSHBothExtract);
            groupBox3.Controls.Add(SSHMetalExtract);
            groupBox3.Controls.Add(SSHMetalImport);
            groupBox3.Location = new Point(192, 252);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(521, 169);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "Colour Table Info";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(214, 141);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(97, 19);
            checkBox1.TabIndex = 30;
            checkBox1.Text = "Limit Colours";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // HalfAlphaButton
            // 
            HalfAlphaButton.Location = new Point(118, 138);
            HalfAlphaButton.Name = "HalfAlphaButton";
            HalfAlphaButton.Size = new Size(90, 23);
            HalfAlphaButton.TabIndex = 22;
            HalfAlphaButton.Text = "Half Alpha";
            HalfAlphaButton.UseVisualStyleBackColor = true;
            HalfAlphaButton.Click += HalfAlphaButton_Click;
            // 
            // DoubleAlphaButton
            // 
            DoubleAlphaButton.Location = new Point(118, 109);
            DoubleAlphaButton.Name = "DoubleAlphaButton";
            DoubleAlphaButton.Size = new Size(90, 23);
            DoubleAlphaButton.TabIndex = 23;
            DoubleAlphaButton.Text = "Double Alpha";
            DoubleAlphaButton.UseVisualStyleBackColor = true;
            DoubleAlphaButton.Click += DoubleAlphaButton_Click;
            // 
            // ColourAmountLabel
            // 
            ColourAmountLabel.AutoSize = true;
            ColourAmountLabel.Location = new Point(11, 37);
            ColourAmountLabel.Name = "ColourAmountLabel";
            ColourAmountLabel.Size = new Size(13, 15);
            ColourAmountLabel.TabIndex = 29;
            ColourAmountLabel.Text = "0";
            // 
            // ColourByteSwappedCheckbox
            // 
            ColourByteSwappedCheckbox.AutoSize = true;
            ColourByteSwappedCheckbox.Location = new Point(415, 144);
            ColourByteSwappedCheckbox.Name = "ColourByteSwappedCheckbox";
            ColourByteSwappedCheckbox.Size = new Size(100, 19);
            ColourByteSwappedCheckbox.TabIndex = 28;
            ColourByteSwappedCheckbox.Text = "Byte Swapped";
            ColourByteSwappedCheckbox.UseVisualStyleBackColor = true;
            ColourByteSwappedCheckbox.CheckedChanged += UpdateImageDetails;
            ColourByteSwappedCheckbox.CheckStateChanged += UpdateImageDetails;
            // 
            // HalfColourButton
            // 
            HalfColourButton.Location = new Point(11, 138);
            HalfColourButton.Name = "HalfColourButton";
            HalfColourButton.Size = new Size(101, 23);
            HalfColourButton.TabIndex = 21;
            HalfColourButton.Text = "Half Colours";
            HalfColourButton.UseVisualStyleBackColor = true;
            HalfColourButton.Click += HalfColourButton_Click;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(11, 19);
            label12.Name = "label12";
            label12.Size = new Size(79, 15);
            label12.TabIndex = 28;
            label12.Text = "Colour Count";
            // 
            // DoubleColourButton
            // 
            DoubleColourButton.Location = new Point(11, 109);
            DoubleColourButton.Name = "DoubleColourButton";
            DoubleColourButton.Size = new Size(101, 23);
            DoubleColourButton.TabIndex = 20;
            DoubleColourButton.Text = "Double Colours";
            DoubleColourButton.UseVisualStyleBackColor = true;
            DoubleColourButton.Click += DoubleColourButton_Click;
            // 
            // ColourAlphaFix
            // 
            ColourAlphaFix.AutoSize = true;
            ColourAlphaFix.Location = new Point(11, 84);
            ColourAlphaFix.Name = "ColourAlphaFix";
            ColourAlphaFix.Size = new Size(74, 19);
            ColourAlphaFix.TabIndex = 7;
            ColourAlphaFix.Text = "Alpha Fix";
            ColourAlphaFix.UseVisualStyleBackColor = true;
            ColourAlphaFix.CheckStateChanged += UpdateImageDetails;
            // 
            // MetalAlphaCheckbox
            // 
            MetalAlphaCheckbox.AutoSize = true;
            MetalAlphaCheckbox.Location = new Point(331, 18);
            MetalAlphaCheckbox.Name = "MetalAlphaCheckbox";
            MetalAlphaCheckbox.Size = new Size(90, 19);
            MetalAlphaCheckbox.TabIndex = 8;
            MetalAlphaCheckbox.Text = "Metal Alpha";
            MetalAlphaCheckbox.UseVisualStyleBackColor = true;
            MetalAlphaCheckbox.CheckStateChanged += UpdateImageDetails;
            // 
            // SSHBothImport
            // 
            SSHBothImport.Location = new Point(427, 66);
            SSHBothImport.Name = "SSHBothImport";
            SSHBothImport.Size = new Size(89, 23);
            SSHBothImport.TabIndex = 19;
            SSHBothImport.Text = "Both Import";
            SSHBothImport.UseVisualStyleBackColor = true;
            SSHBothImport.Click += SSHBothImport_Click;
            // 
            // SSHBothExtract
            // 
            SSHBothExtract.Location = new Point(331, 66);
            SSHBothExtract.Name = "SSHBothExtract";
            SSHBothExtract.Size = new Size(90, 23);
            SSHBothExtract.TabIndex = 18;
            SSHBothExtract.Text = "Both Extract";
            SSHBothExtract.UseVisualStyleBackColor = true;
            SSHBothExtract.Click += SSHBothExtract_Click;
            // 
            // SSHMetalExtract
            // 
            SSHMetalExtract.Location = new Point(331, 37);
            SSHMetalExtract.Name = "SSHMetalExtract";
            SSHMetalExtract.Size = new Size(90, 23);
            SSHMetalExtract.TabIndex = 16;
            SSHMetalExtract.Text = "Metal Extract";
            SSHMetalExtract.UseVisualStyleBackColor = true;
            SSHMetalExtract.Click += SSHMetalExtract_Click;
            // 
            // SSHMetalImport
            // 
            SSHMetalImport.Location = new Point(427, 37);
            SSHMetalImport.Name = "SSHMetalImport";
            SSHMetalImport.Size = new Size(89, 23);
            SSHMetalImport.TabIndex = 17;
            SSHMetalImport.Text = "Metal Import";
            SSHMetalImport.UseVisualStyleBackColor = true;
            SSHMetalImport.Click += SSHMetalImport_Click;
            // 
            // BlackDisplayCheckBox
            // 
            BlackDisplayCheckBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BlackDisplayCheckBox.AutoSize = true;
            BlackDisplayCheckBox.Checked = true;
            BlackDisplayCheckBox.CheckState = CheckState.Checked;
            BlackDisplayCheckBox.Location = new Point(719, 533);
            BlackDisplayCheckBox.Name = "BlackDisplayCheckBox";
            BlackDisplayCheckBox.Size = new Size(198, 19);
            BlackDisplayCheckBox.TabIndex = 10;
            BlackDisplayCheckBox.Text = "Black Background (Display Only)";
            BlackDisplayCheckBox.UseVisualStyleBackColor = true;
            BlackDisplayCheckBox.CheckedChanged += BlackDisplayCheckBox_CheckedChanged;
            // 
            // PictureBox1
            // 
            PictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PictureBox1.BackColor = SystemColors.ActiveCaptionText;
            PictureBox1.BorderStyle = BorderStyle.FixedSingle;
            PictureBox1.Location = new Point(719, 27);
            PictureBox1.Name = "PictureBox1";
            PictureBox1.Size = new Size(500, 500);
            PictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            PictureBox1.TabIndex = 11;
            PictureBox1.TabStop = false;
            // 
            // ImageMoveUpButton
            // 
            ImageMoveUpButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ImageMoveUpButton.Location = new Point(12, 538);
            ImageMoveUpButton.Name = "ImageMoveUpButton";
            ImageMoveUpButton.Size = new Size(83, 23);
            ImageMoveUpButton.TabIndex = 12;
            ImageMoveUpButton.Text = "/\\";
            ImageMoveUpButton.UseVisualStyleBackColor = true;
            ImageMoveUpButton.Click += ImageMoveUpButton_Click;
            // 
            // ImageMoveDownButton
            // 
            ImageMoveDownButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ImageMoveDownButton.Location = new Point(101, 538);
            ImageMoveDownButton.Name = "ImageMoveDownButton";
            ImageMoveDownButton.Size = new Size(85, 23);
            ImageMoveDownButton.TabIndex = 13;
            ImageMoveDownButton.Text = "\\/";
            ImageMoveDownButton.UseVisualStyleBackColor = true;
            ImageMoveDownButton.Click += ImageMoveDownButton_Click;
            // 
            // IamgeAddButton
            // 
            IamgeAddButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            IamgeAddButton.Location = new Point(12, 567);
            IamgeAddButton.Name = "IamgeAddButton";
            IamgeAddButton.Size = new Size(83, 23);
            IamgeAddButton.TabIndex = 14;
            IamgeAddButton.Text = "Add";
            IamgeAddButton.UseVisualStyleBackColor = true;
            IamgeAddButton.Click += IamgeAddButton_Click;
            // 
            // ImageRemoveButton
            // 
            ImageRemoveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ImageRemoveButton.Location = new Point(101, 567);
            ImageRemoveButton.Name = "ImageRemoveButton";
            ImageRemoveButton.Size = new Size(85, 23);
            ImageRemoveButton.TabIndex = 15;
            ImageRemoveButton.Text = "Remove";
            ImageRemoveButton.UseVisualStyleBackColor = true;
            ImageRemoveButton.Click += ImageRemoveButton_Click;
            // 
            // ReplaceImageButton
            // 
            ReplaceImageButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ReplaceImageButton.Location = new Point(719, 567);
            ReplaceImageButton.Name = "ReplaceImageButton";
            ReplaceImageButton.Size = new Size(93, 23);
            ReplaceImageButton.TabIndex = 16;
            ReplaceImageButton.Text = "Replace Image";
            ReplaceImageButton.UseVisualStyleBackColor = true;
            ReplaceImageButton.Click += ReplaceImageButton_Click;
            // 
            // ExportImageButton
            // 
            ExportImageButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ExportImageButton.Location = new Point(1122, 567);
            ExportImageButton.Name = "ExportImageButton";
            ExportImageButton.Size = new Size(97, 23);
            ExportImageButton.TabIndex = 17;
            ExportImageButton.Text = "Export Image";
            ExportImageButton.UseVisualStyleBackColor = true;
            ExportImageButton.Click += ExportImageButton_Click;
            // 
            // NewSSHImageTools
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1231, 599);
            Controls.Add(ExportImageButton);
            Controls.Add(ReplaceImageButton);
            Controls.Add(ImageRemoveButton);
            Controls.Add(IamgeAddButton);
            Controls.Add(ImageMoveDownButton);
            Controls.Add(ImageMoveUpButton);
            Controls.Add(PictureBox1);
            Controls.Add(BlackDisplayCheckBox);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(toolStrip1);
            Controls.Add(ImageList);
            Name = "NewSSHImageTools";
            Text = "New SSH Image Editor";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)YAxisNum).EndInit();
            ((System.ComponentModel.ISupportInitialize)XAxisNum).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox ImageList;
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem LoadSSHButton;
        private ToolStripMenuItem LoadPNGFolderButton;
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
        private ComboBox MatrixTypeDropdown;
        private Label ColourAmountLabel;
        private CheckBox ColourByteSwappedCheckbox;
        private Label label12;
        private Button ReplaceImageButton;
        private Button ExportImageButton;
        private NumericUpDown YAxisNum;
        private NumericUpDown XAxisNum;
        private ToolStripMenuItem toolStripMenuItem1;
        private CheckBox checkBox1;
    }
}