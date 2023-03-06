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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            label5 = new Label();
            label2 = new Label();
            groupBox4 = new GroupBox();
            label25 = new Label();
            MorphGroupCount = new Label();
            label23 = new Label();
            WeightGroupCount = new Label();
            label21 = new Label();
            MaterialGroupCount = new Label();
            label1 = new Label();
            TristripCountLabel = new Label();
            label4 = new Label();
            VerticeCount = new Label();
            MeshChunks = new Label();
            label3 = new Label();
            groupBox3 = new GroupBox();
            label9 = new Label();
            MatUnknown2 = new NumericUpDown();
            label7 = new Label();
            MatUnknown1 = new NumericUpDown();
            label20 = new Label();
            MatFlagFactor = new NumericUpDown();
            MatTextureFlag4 = new TextBox();
            label19 = new Label();
            MatTextureFlag3 = new TextBox();
            label18 = new Label();
            MatTextureFlag2 = new TextBox();
            label17 = new Label();
            MatTextureFlag1 = new TextBox();
            label16 = new Label();
            MatMainTexture = new TextBox();
            label15 = new Label();
            MaterialList = new ListBox();
            groupBox2 = new GroupBox();
            label13 = new Label();
            MpfWeights = new Label();
            label14 = new Label();
            FileID = new Label();
            label6 = new Label();
            BoneCount = new Label();
            label8 = new Label();
            ShapeKeyCount = new Label();
            MaterialCount = new Label();
            label11 = new Label();
            label10 = new Label();
            IkCount = new Label();
            groupBox1 = new GroupBox();
            label12 = new Label();
            TristripMethodList = new ComboBox();
            BoneUpdateCheck = new CheckBox();
            ImportAverageNormal = new CheckBox();
            CharacterParts = new ListBox();
            MPFImport = new Button();
            button2 = new Button();
            MpfList = new ListBox();
            MPFExtract = new Button();
            MPFLoad = new Button();
            tabPage3 = new TabPage();
            MXFLoad = new Button();
            tabPage4 = new TabPage();
            groupBox6 = new GroupBox();
            groupBox8 = new GroupBox();
            groupBox7 = new GroupBox();
            listBox2 = new ListBox();
            hdrSave = new Button();
            HdrLoad = new Button();
            groupBox5 = new GroupBox();
            listBox1 = new ListBox();
            bnkPlay = new Button();
            BnkTotalSamples = new Label();
            label27 = new Label();
            hdrBuildDAT = new Button();
            button1 = new Button();
            hdrExtract = new Button();
            BnkSample = new Label();
            button3 = new Button();
            label26 = new Label();
            button4 = new Button();
            BnkFileSize = new Label();
            button5 = new Button();
            label22 = new Label();
            button6 = new Button();
            label24 = new Label();
            BnkTime = new Label();
            tabPage2 = new TabPage();
            ELFLdrSetup = new Button();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MatUnknown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MatUnknown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MatFlagFactor).BeginInit();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox5.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(776, 426);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(groupBox4);
            tabPage1.Controls.Add(groupBox3);
            tabPage1.Controls.Add(groupBox2);
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Controls.Add(CharacterParts);
            tabPage1.Controls.Add(MPFImport);
            tabPage1.Controls.Add(button2);
            tabPage1.Controls.Add(MpfList);
            tabPage1.Controls.Add(MPFExtract);
            tabPage1.Controls.Add(MPFLoad);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(768, 398);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Tricky MPF (Models)";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 197);
            label5.Name = "label5";
            label5.Size = new Size(65, 15);
            label5.TabIndex = 28;
            label5.Text = "Mesh Parts";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 7);
            label2.Name = "label2";
            label2.Size = new Size(46, 15);
            label2.TabIndex = 27;
            label2.Text = "Models";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(label25);
            groupBox4.Controls.Add(MorphGroupCount);
            groupBox4.Controls.Add(label23);
            groupBox4.Controls.Add(WeightGroupCount);
            groupBox4.Controls.Add(label21);
            groupBox4.Controls.Add(MaterialGroupCount);
            groupBox4.Controls.Add(label1);
            groupBox4.Controls.Add(TristripCountLabel);
            groupBox4.Controls.Add(label4);
            groupBox4.Controls.Add(VerticeCount);
            groupBox4.Controls.Add(MeshChunks);
            groupBox4.Controls.Add(label3);
            groupBox4.Location = new Point(210, 100);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(292, 109);
            groupBox4.TabIndex = 26;
            groupBox4.TabStop = false;
            groupBox4.Text = "Model Data";
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(180, 49);
            label25.Name = "label25";
            label25.Size = new Size(84, 15);
            label25.TabIndex = 17;
            label25.Text = "Morph Groups";
            // 
            // MorphGroupCount
            // 
            MorphGroupCount.AutoSize = true;
            MorphGroupCount.Location = new Point(180, 64);
            MorphGroupCount.Name = "MorphGroupCount";
            MorphGroupCount.Size = new Size(13, 15);
            MorphGroupCount.TabIndex = 18;
            MorphGroupCount.Text = "0";
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(96, 49);
            label23.Name = "label23";
            label23.Size = new Size(86, 15);
            label23.TabIndex = 15;
            label23.Text = "Weight Groups";
            // 
            // WeightGroupCount
            // 
            WeightGroupCount.AutoSize = true;
            WeightGroupCount.Location = new Point(96, 64);
            WeightGroupCount.Name = "WeightGroupCount";
            WeightGroupCount.Size = new Size(13, 15);
            WeightGroupCount.TabIndex = 16;
            WeightGroupCount.Text = "0";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(6, 49);
            label21.Name = "label21";
            label21.Size = new Size(91, 15);
            label21.TabIndex = 13;
            label21.Text = "Material Groups";
            // 
            // MaterialGroupCount
            // 
            MaterialGroupCount.AutoSize = true;
            MaterialGroupCount.Location = new Point(6, 64);
            MaterialGroupCount.Name = "MaterialGroupCount";
            MaterialGroupCount.Size = new Size(13, 15);
            MaterialGroupCount.TabIndex = 14;
            MaterialGroupCount.Text = "0";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 19);
            label1.Name = "label1";
            label1.Size = new Size(78, 15);
            label1.TabIndex = 6;
            label1.Text = "Tristrip Count";
            // 
            // TristripCountLabel
            // 
            TristripCountLabel.AutoSize = true;
            TristripCountLabel.Location = new Point(6, 34);
            TristripCountLabel.Name = "TristripCountLabel";
            TristripCountLabel.Size = new Size(13, 15);
            TristripCountLabel.TabIndex = 7;
            TristripCountLabel.Text = "0";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(96, 19);
            label4.Name = "label4";
            label4.Size = new Size(78, 15);
            label4.TabIndex = 8;
            label4.Text = "Vertice Count";
            // 
            // VerticeCount
            // 
            VerticeCount.AutoSize = true;
            VerticeCount.Location = new Point(96, 34);
            VerticeCount.Name = "VerticeCount";
            VerticeCount.Size = new Size(13, 15);
            VerticeCount.TabIndex = 9;
            VerticeCount.Text = "0";
            // 
            // MeshChunks
            // 
            MeshChunks.AutoSize = true;
            MeshChunks.Location = new Point(180, 34);
            MeshChunks.Name = "MeshChunks";
            MeshChunks.Size = new Size(13, 15);
            MeshChunks.TabIndex = 12;
            MeshChunks.Text = "0";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(180, 19);
            label3.Name = "label3";
            label3.Size = new Size(79, 15);
            label3.TabIndex = 11;
            label3.Text = "Mesh Chunks";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(label9);
            groupBox3.Controls.Add(MatUnknown2);
            groupBox3.Controls.Add(label7);
            groupBox3.Controls.Add(MatUnknown1);
            groupBox3.Controls.Add(label20);
            groupBox3.Controls.Add(MatFlagFactor);
            groupBox3.Controls.Add(MatTextureFlag4);
            groupBox3.Controls.Add(label19);
            groupBox3.Controls.Add(MatTextureFlag3);
            groupBox3.Controls.Add(label18);
            groupBox3.Controls.Add(MatTextureFlag2);
            groupBox3.Controls.Add(label17);
            groupBox3.Controls.Add(MatTextureFlag1);
            groupBox3.Controls.Add(label16);
            groupBox3.Controls.Add(MatMainTexture);
            groupBox3.Controls.Add(label15);
            groupBox3.Controls.Add(MaterialList);
            groupBox3.Location = new Point(508, 6);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(254, 218);
            groupBox3.TabIndex = 25;
            groupBox3.TabStop = false;
            groupBox3.Text = "Material Settings";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(167, 164);
            label9.Name = "label9";
            label9.Size = new Size(67, 15);
            label9.TabIndex = 43;
            label9.Text = "Unknown 2";
            // 
            // MatUnknown2
            // 
            MatUnknown2.DecimalPlaces = 6;
            MatUnknown2.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            MatUnknown2.Location = new Point(167, 183);
            MatUnknown2.Name = "MatUnknown2";
            MatUnknown2.Size = new Size(81, 23);
            MatUnknown2.TabIndex = 42;
            MatUnknown2.ValueChanged += MPFUpdateMat;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(82, 164);
            label7.Name = "label7";
            label7.Size = new Size(67, 15);
            label7.TabIndex = 41;
            label7.Text = "Unknown 1";
            // 
            // MatUnknown1
            // 
            MatUnknown1.DecimalPlaces = 6;
            MatUnknown1.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            MatUnknown1.Location = new Point(82, 183);
            MatUnknown1.Name = "MatUnknown1";
            MatUnknown1.Size = new Size(81, 23);
            MatUnknown1.TabIndex = 40;
            MatUnknown1.ValueChanged += MPFUpdateMat;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(167, 117);
            label20.Name = "label20";
            label20.Size = new Size(65, 15);
            label20.TabIndex = 39;
            label20.Text = "Flag Factor";
            // 
            // MatFlagFactor
            // 
            MatFlagFactor.DecimalPlaces = 6;
            MatFlagFactor.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            MatFlagFactor.Location = new Point(167, 136);
            MatFlagFactor.Name = "MatFlagFactor";
            MatFlagFactor.Size = new Size(81, 23);
            MatFlagFactor.TabIndex = 38;
            MatFlagFactor.ValueChanged += MPFUpdateMat;
            // 
            // MatTextureFlag4
            // 
            MatTextureFlag4.Location = new Point(82, 135);
            MatTextureFlag4.Name = "MatTextureFlag4";
            MatTextureFlag4.Size = new Size(79, 23);
            MatTextureFlag4.TabIndex = 37;
            MatTextureFlag4.TextChanged += MPFUpdateMat;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(82, 117);
            label19.Name = "label19";
            label19.Size = new Size(79, 15);
            label19.TabIndex = 36;
            label19.Text = "Texture Flag 4";
            // 
            // MatTextureFlag3
            // 
            MatTextureFlag3.Location = new Point(169, 86);
            MatTextureFlag3.Name = "MatTextureFlag3";
            MatTextureFlag3.Size = new Size(79, 23);
            MatTextureFlag3.TabIndex = 35;
            MatTextureFlag3.TextChanged += MPFUpdateMat;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(169, 68);
            label18.Name = "label18";
            label18.Size = new Size(79, 15);
            label18.TabIndex = 34;
            label18.Text = "Texture Flag 3";
            // 
            // MatTextureFlag2
            // 
            MatTextureFlag2.Location = new Point(82, 86);
            MatTextureFlag2.Name = "MatTextureFlag2";
            MatTextureFlag2.Size = new Size(79, 23);
            MatTextureFlag2.TabIndex = 33;
            MatTextureFlag2.TextChanged += MPFUpdateMat;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(82, 68);
            label17.Name = "label17";
            label17.Size = new Size(79, 15);
            label17.TabIndex = 32;
            label17.Text = "Texture Flag 2";
            // 
            // MatTextureFlag1
            // 
            MatTextureFlag1.Location = new Point(169, 40);
            MatTextureFlag1.Name = "MatTextureFlag1";
            MatTextureFlag1.Size = new Size(79, 23);
            MatTextureFlag1.TabIndex = 31;
            MatTextureFlag1.TextChanged += MPFUpdateMat;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(169, 22);
            label16.Name = "label16";
            label16.Size = new Size(79, 15);
            label16.TabIndex = 30;
            label16.Text = "Texture Flag 1";
            // 
            // MatMainTexture
            // 
            MatMainTexture.Location = new Point(82, 40);
            MatMainTexture.Name = "MatMainTexture";
            MatMainTexture.Size = new Size(79, 23);
            MatMainTexture.TabIndex = 29;
            MatMainTexture.TextChanged += MPFUpdateMat;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(82, 22);
            label15.Name = "label15";
            label15.Size = new Size(75, 15);
            label15.TabIndex = 28;
            label15.Text = "Main Texture";
            // 
            // MaterialList
            // 
            MaterialList.FormattingEnabled = true;
            MaterialList.ItemHeight = 15;
            MaterialList.Location = new Point(6, 22);
            MaterialList.Name = "MaterialList";
            MaterialList.Size = new Size(70, 184);
            MaterialList.TabIndex = 27;
            MaterialList.SelectedIndexChanged += MaterialList_SelectedIndexChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label13);
            groupBox2.Controls.Add(MpfWeights);
            groupBox2.Controls.Add(label14);
            groupBox2.Controls.Add(FileID);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(BoneCount);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(ShapeKeyCount);
            groupBox2.Controls.Add(MaterialCount);
            groupBox2.Controls.Add(label11);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(IkCount);
            groupBox2.Location = new Point(210, 6);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(292, 88);
            groupBox2.TabIndex = 24;
            groupBox2.TabStop = false;
            groupBox2.Text = "Model Header Info";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(6, 53);
            label13.Name = "label13";
            label13.Size = new Size(50, 15);
            label13.TabIndex = 24;
            label13.Text = "Weights";
            // 
            // MpfWeights
            // 
            MpfWeights.AutoSize = true;
            MpfWeights.Location = new Point(6, 68);
            MpfWeights.Name = "MpfWeights";
            MpfWeights.Size = new Size(13, 15);
            MpfWeights.TabIndex = 25;
            MpfWeights.Text = "0";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(6, 19);
            label14.Name = "label14";
            label14.Size = new Size(39, 15);
            label14.TabIndex = 22;
            label14.Text = "File ID";
            // 
            // FileID
            // 
            FileID.AutoSize = true;
            FileID.Location = new Point(6, 34);
            FileID.Name = "FileID";
            FileID.Size = new Size(13, 15);
            FileID.TabIndex = 23;
            FileID.Text = "0";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(51, 19);
            label6.Name = "label6";
            label6.Size = new Size(39, 15);
            label6.TabIndex = 13;
            label6.Text = "Bones";
            // 
            // BoneCount
            // 
            BoneCount.AutoSize = true;
            BoneCount.Location = new Point(51, 34);
            BoneCount.Name = "BoneCount";
            BoneCount.Size = new Size(13, 15);
            BoneCount.TabIndex = 14;
            BoneCount.Text = "0";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(96, 19);
            label8.Name = "label8";
            label8.Size = new Size(55, 15);
            label8.TabIndex = 15;
            label8.Text = "Materials";
            // 
            // ShapeKeyCount
            // 
            ShapeKeyCount.AutoSize = true;
            ShapeKeyCount.Location = new Point(216, 34);
            ShapeKeyCount.Name = "ShapeKeyCount";
            ShapeKeyCount.Size = new Size(13, 15);
            ShapeKeyCount.TabIndex = 20;
            ShapeKeyCount.Text = "0";
            // 
            // MaterialCount
            // 
            MaterialCount.AutoSize = true;
            MaterialCount.Location = new Point(96, 34);
            MaterialCount.Name = "MaterialCount";
            MaterialCount.Size = new Size(13, 15);
            MaterialCount.TabIndex = 16;
            MaterialCount.Text = "0";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(216, 19);
            label11.Name = "label11";
            label11.Size = new Size(66, 15);
            label11.TabIndex = 19;
            label11.Text = "Shape Keys";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(157, 19);
            label10.Name = "label10";
            label10.Size = new Size(53, 15);
            label10.TabIndex = 17;
            label10.Text = "IK Points";
            // 
            // IkCount
            // 
            IkCount.AutoSize = true;
            IkCount.Location = new Point(157, 34);
            IkCount.Name = "IkCount";
            IkCount.Size = new Size(13, 15);
            IkCount.TabIndex = 18;
            IkCount.Text = "0";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label12);
            groupBox1.Controls.Add(TristripMethodList);
            groupBox1.Controls.Add(BoneUpdateCheck);
            groupBox1.Controls.Add(ImportAverageNormal);
            groupBox1.Location = new Point(204, 282);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(292, 78);
            groupBox1.TabIndex = 21;
            groupBox1.TabStop = false;
            groupBox1.Text = "Import Settings";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(149, 23);
            label12.Name = "label12";
            label12.Size = new Size(87, 15);
            label12.TabIndex = 3;
            label12.Text = "Tristrip Method";
            // 
            // TristripMethodList
            // 
            TristripMethodList.FormattingEnabled = true;
            TristripMethodList.Items.AddRange(new object[] { "Nvida Tristrip" });
            TristripMethodList.Location = new Point(149, 41);
            TristripMethodList.Name = "TristripMethodList";
            TristripMethodList.Size = new Size(121, 23);
            TristripMethodList.TabIndex = 2;
            // 
            // BoneUpdateCheck
            // 
            BoneUpdateCheck.AutoSize = true;
            BoneUpdateCheck.Location = new Point(6, 47);
            BoneUpdateCheck.Name = "BoneUpdateCheck";
            BoneUpdateCheck.Size = new Size(131, 19);
            BoneUpdateCheck.TabIndex = 1;
            BoneUpdateCheck.Text = "Update Bones (WIP)";
            BoneUpdateCheck.UseVisualStyleBackColor = true;
            // 
            // ImportAverageNormal
            // 
            ImportAverageNormal.AutoSize = true;
            ImportAverageNormal.Location = new Point(6, 22);
            ImportAverageNormal.Name = "ImportAverageNormal";
            ImportAverageNormal.Size = new Size(117, 19);
            ImportAverageNormal.TabIndex = 0;
            ImportAverageNormal.Text = "Average Normals";
            ImportAverageNormal.UseVisualStyleBackColor = true;
            // 
            // CharacterParts
            // 
            CharacterParts.FormattingEnabled = true;
            CharacterParts.ItemHeight = 15;
            CharacterParts.Location = new Point(6, 215);
            CharacterParts.Name = "CharacterParts";
            CharacterParts.Size = new Size(192, 169);
            CharacterParts.TabIndex = 10;
            CharacterParts.SelectedIndexChanged += CharacterParts_SelectedIndexChanged;
            // 
            // MPFImport
            // 
            MPFImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            MPFImport.Location = new Point(285, 366);
            MPFImport.Name = "MPFImport";
            MPFImport.Size = new Size(75, 23);
            MPFImport.TabIndex = 5;
            MPFImport.Text = "Import Model";
            MPFImport.UseVisualStyleBackColor = true;
            MPFImport.Click += MPFImport_Click;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button2.Location = new Point(687, 366);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 4;
            button2.Text = "Save MPF";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // MpfList
            // 
            MpfList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            MpfList.FormattingEnabled = true;
            MpfList.ItemHeight = 15;
            MpfList.Location = new Point(6, 25);
            MpfList.Name = "MpfList";
            MpfList.Size = new Size(192, 169);
            MpfList.TabIndex = 2;
            MpfList.SelectedIndexChanged += MpfList_SelectedIndexChanged;
            // 
            // MPFExtract
            // 
            MPFExtract.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            MPFExtract.Location = new Point(606, 366);
            MPFExtract.Name = "MPFExtract";
            MPFExtract.Size = new Size(75, 23);
            MPFExtract.TabIndex = 1;
            MPFExtract.Text = "Extract";
            MPFExtract.UseVisualStyleBackColor = true;
            MPFExtract.Click += MPFExtract_Click;
            // 
            // MPFLoad
            // 
            MPFLoad.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            MPFLoad.Location = new Point(204, 366);
            MPFLoad.Name = "MPFLoad";
            MPFLoad.Size = new Size(75, 23);
            MPFLoad.TabIndex = 0;
            MPFLoad.Text = "Load MPF";
            MPFLoad.UseVisualStyleBackColor = true;
            MPFLoad.Click += MPFLoad_Click;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(MXFLoad);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(768, 398);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Tricky MXF (Models)";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // MXFLoad
            // 
            MXFLoad.Location = new Point(3, 372);
            MXFLoad.Name = "MXFLoad";
            MXFLoad.Size = new Size(75, 23);
            MXFLoad.TabIndex = 0;
            MXFLoad.Text = "Load";
            MXFLoad.UseVisualStyleBackColor = true;
            MXFLoad.Click += MXFLoad_Click;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(groupBox6);
            tabPage4.Controls.Add(groupBox5);
            tabPage4.Location = new Point(4, 24);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(768, 398);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Tricky Sound Files (.HDR/.DAT)";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(groupBox8);
            groupBox6.Controls.Add(groupBox7);
            groupBox6.Controls.Add(listBox2);
            groupBox6.Controls.Add(hdrSave);
            groupBox6.Controls.Add(HdrLoad);
            groupBox6.Location = new Point(376, 9);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(389, 386);
            groupBox6.TabIndex = 39;
            groupBox6.TabStop = false;
            groupBox6.Text = ".HDR";
            // 
            // groupBox8
            // 
            groupBox8.Location = new Point(182, 22);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(200, 154);
            groupBox8.TabIndex = 40;
            groupBox8.TabStop = false;
            groupBox8.Text = "HDR Entry";
            // 
            // groupBox7
            // 
            groupBox7.Location = new Point(182, 182);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(200, 174);
            groupBox7.TabIndex = 39;
            groupBox7.TabStop = false;
            groupBox7.Text = "Header Info";
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.ItemHeight = 15;
            listBox2.Location = new Point(6, 22);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(170, 334);
            listBox2.TabIndex = 38;
            // 
            // hdrSave
            // 
            hdrSave.Location = new Point(308, 357);
            hdrSave.Name = "hdrSave";
            hdrSave.Size = new Size(75, 23);
            hdrSave.TabIndex = 28;
            hdrSave.Text = "Save .HDR";
            hdrSave.UseVisualStyleBackColor = true;
            // 
            // HdrLoad
            // 
            HdrLoad.Location = new Point(6, 357);
            HdrLoad.Name = "HdrLoad";
            HdrLoad.Size = new Size(80, 23);
            HdrLoad.TabIndex = 19;
            HdrLoad.Text = "Load .HDR";
            HdrLoad.UseVisualStyleBackColor = true;
            HdrLoad.Click += HdrLoad_Click;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(listBox1);
            groupBox5.Controls.Add(bnkPlay);
            groupBox5.Controls.Add(BnkTotalSamples);
            groupBox5.Controls.Add(label27);
            groupBox5.Controls.Add(hdrBuildDAT);
            groupBox5.Controls.Add(button1);
            groupBox5.Controls.Add(hdrExtract);
            groupBox5.Controls.Add(BnkSample);
            groupBox5.Controls.Add(button3);
            groupBox5.Controls.Add(label26);
            groupBox5.Controls.Add(button4);
            groupBox5.Controls.Add(BnkFileSize);
            groupBox5.Controls.Add(button5);
            groupBox5.Controls.Add(label22);
            groupBox5.Controls.Add(button6);
            groupBox5.Controls.Add(label24);
            groupBox5.Controls.Add(BnkTime);
            groupBox5.Location = new Point(10, 9);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(360, 386);
            groupBox5.TabIndex = 38;
            groupBox5.TabStop = false;
            groupBox5.Text = ".DAT";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(6, 23);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(170, 304);
            listBox1.TabIndex = 24;
            // 
            // bnkPlay
            // 
            bnkPlay.Location = new Point(182, 328);
            bnkPlay.Name = "bnkPlay";
            bnkPlay.Size = new Size(84, 23);
            bnkPlay.TabIndex = 33;
            bnkPlay.Text = "Play Audio";
            bnkPlay.UseVisualStyleBackColor = true;
            // 
            // BnkTotalSamples
            // 
            BnkTotalSamples.AutoSize = true;
            BnkTotalSamples.Location = new Point(272, 68);
            BnkTotalSamples.Name = "BnkTotalSamples";
            BnkTotalSamples.Size = new Size(13, 15);
            BnkTotalSamples.TabIndex = 37;
            BnkTotalSamples.Text = "0";
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(182, 23);
            label27.Name = "label27";
            label27.Size = new Size(73, 15);
            label27.TabIndex = 29;
            label27.Text = "Time Length";
            // 
            // hdrBuildDAT
            // 
            hdrBuildDAT.Enabled = false;
            hdrBuildDAT.Location = new Point(272, 357);
            hdrBuildDAT.Name = "hdrBuildDAT";
            hdrBuildDAT.Size = new Size(82, 23);
            hdrBuildDAT.TabIndex = 21;
            hdrBuildDAT.Text = "Build .DAT";
            hdrBuildDAT.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(6, 328);
            button1.Name = "button1";
            button1.Size = new Size(38, 23);
            button1.TabIndex = 27;
            button1.Text = "-";
            button1.UseVisualStyleBackColor = true;
            // 
            // hdrExtract
            // 
            hdrExtract.Location = new Point(182, 357);
            hdrExtract.Name = "hdrExtract";
            hdrExtract.Size = new Size(84, 23);
            hdrExtract.TabIndex = 20;
            hdrExtract.Text = "Extract .DAT";
            hdrExtract.UseVisualStyleBackColor = true;
            // 
            // BnkSample
            // 
            BnkSample.AutoSize = true;
            BnkSample.Location = new Point(182, 68);
            BnkSample.Name = "BnkSample";
            BnkSample.Size = new Size(13, 15);
            BnkSample.TabIndex = 36;
            BnkSample.Text = "0";
            // 
            // button3
            // 
            button3.Location = new Point(96, 328);
            button3.Name = "button3";
            button3.Size = new Size(32, 23);
            button3.TabIndex = 26;
            button3.Text = "\\/";
            button3.UseVisualStyleBackColor = true;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(272, 23);
            label26.Name = "label26";
            label26.Size = new Size(48, 15);
            label26.TabIndex = 30;
            label26.Text = "File Size";
            // 
            // button4
            // 
            button4.Location = new Point(48, 328);
            button4.Name = "button4";
            button4.Size = new Size(33, 23);
            button4.TabIndex = 25;
            button4.Text = "/\\";
            button4.UseVisualStyleBackColor = true;
            // 
            // BnkFileSize
            // 
            BnkFileSize.AutoSize = true;
            BnkFileSize.Location = new Point(272, 38);
            BnkFileSize.Name = "BnkFileSize";
            BnkFileSize.Size = new Size(13, 15);
            BnkFileSize.TabIndex = 35;
            BnkFileSize.Text = "0";
            // 
            // button5
            // 
            button5.Location = new Point(6, 357);
            button5.Name = "button5";
            button5.Size = new Size(170, 23);
            button5.TabIndex = 23;
            button5.Text = "Load Folder";
            button5.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(272, 53);
            label22.Name = "label22";
            label22.Size = new Size(79, 15);
            label22.TabIndex = 32;
            label22.Text = "Total Samples";
            // 
            // button6
            // 
            button6.Location = new Point(134, 328);
            button6.Name = "button6";
            button6.Size = new Size(42, 23);
            button6.TabIndex = 22;
            button6.Text = "+";
            button6.UseVisualStyleBackColor = true;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(182, 53);
            label24.Name = "label24";
            label24.Size = new Size(72, 15);
            label24.TabIndex = 31;
            label24.Text = "Sample Rate";
            // 
            // BnkTime
            // 
            BnkTime.AutoSize = true;
            BnkTime.Location = new Point(182, 38);
            BnkTime.Name = "BnkTime";
            BnkTime.Size = new Size(13, 15);
            BnkTime.TabIndex = 34;
            BnkTime.Text = "0";
            // 
            // tabPage2
            // 
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
            ELFLdrSetup.TabIndex = 0;
            ELFLdrSetup.Text = "Setup For ELFLdr";
            ELFLdrSetup.UseVisualStyleBackColor = true;
            ELFLdrSetup.Click += ELFLdrSetup_Click;
            // 
            // TrickyToolsWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Name = "TrickyToolsWindow";
            Text = "TrickyToolsWindow";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)MatUnknown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)MatUnknown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)MatFlagFactor).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);
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
        private Label TristripCountLabel;
        private Label label1;
        private Label IkCount;
        private Label label10;
        private Label MaterialCount;
        private Label label8;
        private Label BoneCount;
        private Label label6;
        private Label MeshChunks;
        private Label label3;
        private ListBox CharacterParts;
        private GroupBox groupBox1;
        private Label label12;
        private ComboBox TristripMethodList;
        private CheckBox BoneUpdateCheck;
        private CheckBox ImportAverageNormal;
        private Label ShapeKeyCount;
        private Label label11;
        private Label FileID;
        private Label label14;
        private GroupBox groupBox4;
        private GroupBox groupBox3;
        private NumericUpDown MatFlagFactor;
        private TextBox MatTextureFlag4;
        private Label label19;
        private TextBox MatTextureFlag3;
        private Label label18;
        private TextBox MatTextureFlag2;
        private Label label17;
        private TextBox MatTextureFlag1;
        private Label label16;
        private TextBox MatMainTexture;
        private Label label15;
        private ListBox MaterialList;
        private GroupBox groupBox2;
        private Label label25;
        private Label MorphGroupCount;
        private Label label23;
        private Label WeightGroupCount;
        private Label label21;
        private Label MaterialGroupCount;
        private Label label20;
        private Label label5;
        private Label label2;
        private Label label9;
        private NumericUpDown MatUnknown2;
        private Label label7;
        private NumericUpDown MatUnknown1;
        private Label label13;
        private Label MpfWeights;
        private TabPage tabPage3;
        private Button MXFLoad;
        private TabPage tabPage4;
        private Button hdrSave;
        private Button button1;
        private Button button3;
        private Button button4;
        private ListBox listBox1;
        private Button button5;
        private Button button6;
        private Button hdrBuildDAT;
        private Button hdrExtract;
        private Button HdrLoad;
        private Label BnkTotalSamples;
        private Label BnkSample;
        private Label BnkFileSize;
        private Label BnkTime;
        private Button bnkPlay;
        private Label label22;
        private Label label24;
        private Label label26;
        private Label label27;
        private GroupBox groupBox6;
        private ListBox listBox2;
        private GroupBox groupBox5;
        private GroupBox groupBox8;
        private GroupBox groupBox7;
    }
}