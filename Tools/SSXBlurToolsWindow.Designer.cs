namespace SSXMultiTool.Tools
{
    partial class SSXBlurToolsWindow
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
            groupBox1 = new GroupBox();
            label57 = new Label();
            TristripMethodList = new ComboBox();
            BoneUpdateCheck = new CheckBox();
            ImportAverageNormal = new CheckBox();
            label2 = new Label();
            groupBox3 = new GroupBox();
            label66 = new Label();
            MatUnknown2 = new NumericUpDown();
            label67 = new Label();
            MatUnknown1 = new NumericUpDown();
            label68 = new Label();
            MatFlagFactor = new NumericUpDown();
            MatTextureFlag4 = new TextBox();
            label69 = new Label();
            MatTextureFlag3 = new TextBox();
            label70 = new Label();
            MatTextureFlag2 = new TextBox();
            label71 = new Label();
            MatTextureFlag1 = new TextBox();
            label72 = new Label();
            MatMainTexture = new TextBox();
            label73 = new Label();
            MaterialList = new ListBox();
            groupBox4 = new GroupBox();
            label74 = new Label();
            MorphGroupCount = new Label();
            label75 = new Label();
            WeightGroupCount = new Label();
            label76 = new Label();
            MaterialGroupCount = new Label();
            label77 = new Label();
            TristripCountLabel = new Label();
            label78 = new Label();
            VerticeCount = new Label();
            MeshChunks = new Label();
            label79 = new Label();
            groupBox2 = new GroupBox();
            AltShapeKeyCount = new Label();
            label3 = new Label();
            label60 = new Label();
            MpfWeights = new Label();
            label61 = new Label();
            FileID = new Label();
            label62 = new Label();
            BoneCount = new Label();
            label63 = new Label();
            ShapeKeyCount = new Label();
            MaterialCount = new Label();
            label64 = new Label();
            label65 = new Label();
            IkCount = new Label();
            MpfImport = new Button();
            MPFSaveDecompressed = new Button();
            MpfModelList = new ListBox();
            MpfWarning = new Label();
            label1 = new Label();
            MpfBoneLoad = new Button();
            MpfExport = new Button();
            MpfSave = new Button();
            MpfLoad = new Button();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MatUnknown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MatUnknown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MatFlagFactor).BeginInit();
            groupBox4.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(906, 490);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(groupBox3);
            tabPage1.Controls.Add(groupBox4);
            tabPage1.Controls.Add(groupBox2);
            tabPage1.Controls.Add(MpfImport);
            tabPage1.Controls.Add(MPFSaveDecompressed);
            tabPage1.Controls.Add(MpfModelList);
            tabPage1.Controls.Add(MpfWarning);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(MpfBoneLoad);
            tabPage1.Controls.Add(MpfExport);
            tabPage1.Controls.Add(MpfSave);
            tabPage1.Controls.Add(MpfLoad);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(898, 462);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "SSX Blur Models (MNF)";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label57);
            groupBox1.Controls.Add(TristripMethodList);
            groupBox1.Controls.Add(BoneUpdateCheck);
            groupBox1.Controls.Add(ImportAverageNormal);
            groupBox1.Location = new Point(166, 352);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(292, 78);
            groupBox1.TabIndex = 29;
            groupBox1.TabStop = false;
            groupBox1.Text = "Import Settings";
            // 
            // label57
            // 
            label57.AutoSize = true;
            label57.Location = new Point(149, 23);
            label57.Name = "label57";
            label57.Size = new Size(88, 15);
            label57.TabIndex = 3;
            label57.Text = "Tristrip Method";
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
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 6);
            label2.Name = "label2";
            label2.Size = new Size(62, 15);
            label2.TabIndex = 28;
            label2.Text = "Model List";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(label66);
            groupBox3.Controls.Add(MatUnknown2);
            groupBox3.Controls.Add(label67);
            groupBox3.Controls.Add(MatUnknown1);
            groupBox3.Controls.Add(label68);
            groupBox3.Controls.Add(MatFlagFactor);
            groupBox3.Controls.Add(MatTextureFlag4);
            groupBox3.Controls.Add(label69);
            groupBox3.Controls.Add(MatTextureFlag3);
            groupBox3.Controls.Add(label70);
            groupBox3.Controls.Add(MatTextureFlag2);
            groupBox3.Controls.Add(label71);
            groupBox3.Controls.Add(MatTextureFlag1);
            groupBox3.Controls.Add(label72);
            groupBox3.Controls.Add(MatMainTexture);
            groupBox3.Controls.Add(label73);
            groupBox3.Controls.Add(MaterialList);
            groupBox3.Location = new Point(634, 6);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(254, 218);
            groupBox3.TabIndex = 29;
            groupBox3.TabStop = false;
            groupBox3.Text = "Material Settings";
            // 
            // label66
            // 
            label66.AutoSize = true;
            label66.Location = new Point(167, 164);
            label66.Name = "label66";
            label66.Size = new Size(67, 15);
            label66.TabIndex = 43;
            label66.Text = "Unknown 2";
            // 
            // MatUnknown2
            // 
            MatUnknown2.DecimalPlaces = 6;
            MatUnknown2.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            MatUnknown2.Location = new Point(167, 183);
            MatUnknown2.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            MatUnknown2.Name = "MatUnknown2";
            MatUnknown2.Size = new Size(81, 23);
            MatUnknown2.TabIndex = 42;
            MatUnknown2.ValueChanged += MatUpdate;
            // 
            // label67
            // 
            label67.AutoSize = true;
            label67.Location = new Point(82, 164);
            label67.Name = "label67";
            label67.Size = new Size(67, 15);
            label67.TabIndex = 41;
            label67.Text = "Unknown 1";
            // 
            // MatUnknown1
            // 
            MatUnknown1.DecimalPlaces = 6;
            MatUnknown1.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            MatUnknown1.Location = new Point(82, 183);
            MatUnknown1.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            MatUnknown1.Name = "MatUnknown1";
            MatUnknown1.Size = new Size(81, 23);
            MatUnknown1.TabIndex = 40;
            MatUnknown1.ValueChanged += MatUpdate;
            // 
            // label68
            // 
            label68.AutoSize = true;
            label68.Location = new Point(167, 117);
            label68.Name = "label68";
            label68.Size = new Size(65, 15);
            label68.TabIndex = 39;
            label68.Text = "Flag Factor";
            // 
            // MatFlagFactor
            // 
            MatFlagFactor.DecimalPlaces = 6;
            MatFlagFactor.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            MatFlagFactor.Location = new Point(167, 136);
            MatFlagFactor.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
            MatFlagFactor.Name = "MatFlagFactor";
            MatFlagFactor.Size = new Size(81, 23);
            MatFlagFactor.TabIndex = 38;
            MatFlagFactor.ValueChanged += MatUpdate;
            // 
            // MatTextureFlag4
            // 
            MatTextureFlag4.Location = new Point(82, 135);
            MatTextureFlag4.Name = "MatTextureFlag4";
            MatTextureFlag4.Size = new Size(79, 23);
            MatTextureFlag4.TabIndex = 37;
            MatTextureFlag4.TextChanged += MatUpdate;
            // 
            // label69
            // 
            label69.AutoSize = true;
            label69.Location = new Point(82, 117);
            label69.Name = "label69";
            label69.Size = new Size(79, 15);
            label69.TabIndex = 36;
            label69.Text = "Texture Flag 4";
            // 
            // MatTextureFlag3
            // 
            MatTextureFlag3.Location = new Point(169, 86);
            MatTextureFlag3.Name = "MatTextureFlag3";
            MatTextureFlag3.Size = new Size(79, 23);
            MatTextureFlag3.TabIndex = 35;
            MatTextureFlag3.TextChanged += MatUpdate;
            // 
            // label70
            // 
            label70.AutoSize = true;
            label70.Location = new Point(169, 68);
            label70.Name = "label70";
            label70.Size = new Size(79, 15);
            label70.TabIndex = 34;
            label70.Text = "Texture Flag 3";
            // 
            // MatTextureFlag2
            // 
            MatTextureFlag2.Location = new Point(82, 86);
            MatTextureFlag2.Name = "MatTextureFlag2";
            MatTextureFlag2.Size = new Size(79, 23);
            MatTextureFlag2.TabIndex = 33;
            MatTextureFlag2.TextChanged += MatUpdate;
            // 
            // label71
            // 
            label71.AutoSize = true;
            label71.Location = new Point(82, 68);
            label71.Name = "label71";
            label71.Size = new Size(79, 15);
            label71.TabIndex = 32;
            label71.Text = "Texture Flag 2";
            // 
            // MatTextureFlag1
            // 
            MatTextureFlag1.Location = new Point(169, 40);
            MatTextureFlag1.Name = "MatTextureFlag1";
            MatTextureFlag1.Size = new Size(79, 23);
            MatTextureFlag1.TabIndex = 31;
            MatTextureFlag1.TextChanged += MatUpdate;
            // 
            // label72
            // 
            label72.AutoSize = true;
            label72.Location = new Point(169, 22);
            label72.Name = "label72";
            label72.Size = new Size(79, 15);
            label72.TabIndex = 30;
            label72.Text = "Texture Flag 1";
            // 
            // MatMainTexture
            // 
            MatMainTexture.Location = new Point(82, 40);
            MatMainTexture.Name = "MatMainTexture";
            MatMainTexture.Size = new Size(79, 23);
            MatMainTexture.TabIndex = 29;
            MatMainTexture.TextChanged += MatUpdate;
            // 
            // label73
            // 
            label73.AutoSize = true;
            label73.Location = new Point(82, 22);
            label73.Name = "label73";
            label73.Size = new Size(75, 15);
            label73.TabIndex = 28;
            label73.Text = "Main Texture";
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
            // groupBox4
            // 
            groupBox4.Controls.Add(label74);
            groupBox4.Controls.Add(MorphGroupCount);
            groupBox4.Controls.Add(label75);
            groupBox4.Controls.Add(WeightGroupCount);
            groupBox4.Controls.Add(label76);
            groupBox4.Controls.Add(MaterialGroupCount);
            groupBox4.Controls.Add(label77);
            groupBox4.Controls.Add(TristripCountLabel);
            groupBox4.Controls.Add(label78);
            groupBox4.Controls.Add(VerticeCount);
            groupBox4.Controls.Add(MeshChunks);
            groupBox4.Controls.Add(label79);
            groupBox4.Location = new Point(166, 75);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(462, 93);
            groupBox4.TabIndex = 28;
            groupBox4.TabStop = false;
            groupBox4.Text = "Model Data";
            // 
            // label74
            // 
            label74.AutoSize = true;
            label74.Location = new Point(6, 50);
            label74.Name = "label74";
            label74.Size = new Size(84, 15);
            label74.TabIndex = 17;
            label74.Text = "Morph Groups";
            // 
            // MorphGroupCount
            // 
            MorphGroupCount.AutoSize = true;
            MorphGroupCount.Location = new Point(6, 65);
            MorphGroupCount.Name = "MorphGroupCount";
            MorphGroupCount.Size = new Size(13, 15);
            MorphGroupCount.TabIndex = 18;
            MorphGroupCount.Text = "0";
            // 
            // label75
            // 
            label75.AutoSize = true;
            label75.Location = new Point(355, 19);
            label75.Name = "label75";
            label75.Size = new Size(86, 15);
            label75.TabIndex = 15;
            label75.Text = "Weight Groups";
            // 
            // WeightGroupCount
            // 
            WeightGroupCount.AutoSize = true;
            WeightGroupCount.Location = new Point(355, 34);
            WeightGroupCount.Name = "WeightGroupCount";
            WeightGroupCount.Size = new Size(13, 15);
            WeightGroupCount.TabIndex = 16;
            WeightGroupCount.Text = "0";
            // 
            // label76
            // 
            label76.AutoSize = true;
            label76.Location = new Point(265, 19);
            label76.Name = "label76";
            label76.Size = new Size(91, 15);
            label76.TabIndex = 13;
            label76.Text = "Material Groups";
            // 
            // MaterialGroupCount
            // 
            MaterialGroupCount.AutoSize = true;
            MaterialGroupCount.Location = new Point(265, 34);
            MaterialGroupCount.Name = "MaterialGroupCount";
            MaterialGroupCount.Size = new Size(13, 15);
            MaterialGroupCount.TabIndex = 14;
            MaterialGroupCount.Text = "0";
            // 
            // label77
            // 
            label77.AutoSize = true;
            label77.Location = new Point(6, 19);
            label77.Name = "label77";
            label77.Size = new Size(79, 15);
            label77.TabIndex = 6;
            label77.Text = "Tristrip Count";
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
            // label78
            // 
            label78.AutoSize = true;
            label78.Location = new Point(96, 19);
            label78.Name = "label78";
            label78.Size = new Size(78, 15);
            label78.TabIndex = 8;
            label78.Text = "Vertice Count";
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
            // label79
            // 
            label79.AutoSize = true;
            label79.Location = new Point(180, 19);
            label79.Name = "label79";
            label79.Size = new Size(79, 15);
            label79.TabIndex = 11;
            label79.Text = "Mesh Chunks";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(AltShapeKeyCount);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label60);
            groupBox2.Controls.Add(MpfWeights);
            groupBox2.Controls.Add(label61);
            groupBox2.Controls.Add(FileID);
            groupBox2.Controls.Add(label62);
            groupBox2.Controls.Add(BoneCount);
            groupBox2.Controls.Add(label63);
            groupBox2.Controls.Add(ShapeKeyCount);
            groupBox2.Controls.Add(MaterialCount);
            groupBox2.Controls.Add(label64);
            groupBox2.Controls.Add(label65);
            groupBox2.Controls.Add(IkCount);
            groupBox2.Location = new Point(166, 6);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(462, 63);
            groupBox2.TabIndex = 26;
            groupBox2.TabStop = false;
            groupBox2.Text = "Model Header Info";
            // 
            // AltShapeKeyCount
            // 
            AltShapeKeyCount.AutoSize = true;
            AltShapeKeyCount.Location = new Point(288, 34);
            AltShapeKeyCount.Name = "AltShapeKeyCount";
            AltShapeKeyCount.Size = new Size(13, 15);
            AltShapeKeyCount.TabIndex = 27;
            AltShapeKeyCount.Text = "0";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(288, 19);
            label3.Name = "label3";
            label3.Size = new Size(84, 15);
            label3.TabIndex = 26;
            label3.Text = "Alt Shape Keys";
            // 
            // label60
            // 
            label60.AutoSize = true;
            label60.Location = new Point(378, 19);
            label60.Name = "label60";
            label60.Size = new Size(50, 15);
            label60.TabIndex = 24;
            label60.Text = "Weights";
            // 
            // MpfWeights
            // 
            MpfWeights.AutoSize = true;
            MpfWeights.Location = new Point(378, 34);
            MpfWeights.Name = "MpfWeights";
            MpfWeights.Size = new Size(13, 15);
            MpfWeights.TabIndex = 25;
            MpfWeights.Text = "0";
            // 
            // label61
            // 
            label61.AutoSize = true;
            label61.Location = new Point(6, 19);
            label61.Name = "label61";
            label61.Size = new Size(39, 15);
            label61.TabIndex = 22;
            label61.Text = "File ID";
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
            // label62
            // 
            label62.AutoSize = true;
            label62.Location = new Point(51, 19);
            label62.Name = "label62";
            label62.Size = new Size(39, 15);
            label62.TabIndex = 13;
            label62.Text = "Bones";
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
            // label63
            // 
            label63.AutoSize = true;
            label63.Location = new Point(96, 19);
            label63.Name = "label63";
            label63.Size = new Size(55, 15);
            label63.TabIndex = 15;
            label63.Text = "Materials";
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
            // label64
            // 
            label64.AutoSize = true;
            label64.Location = new Point(216, 19);
            label64.Name = "label64";
            label64.Size = new Size(66, 15);
            label64.TabIndex = 19;
            label64.Text = "Shape Keys";
            // 
            // label65
            // 
            label65.AutoSize = true;
            label65.Location = new Point(157, 19);
            label65.Name = "label65";
            label65.Size = new Size(53, 15);
            label65.TabIndex = 17;
            label65.Text = "IK Points";
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
            // MpfImport
            // 
            MpfImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            MpfImport.Location = new Point(168, 433);
            MpfImport.Name = "MpfImport";
            MpfImport.Size = new Size(75, 23);
            MpfImport.TabIndex = 8;
            MpfImport.Text = "Import";
            MpfImport.UseVisualStyleBackColor = true;
            MpfImport.Click += MpfImport_Click;
            // 
            // MPFSaveDecompressed
            // 
            MPFSaveDecompressed.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            MPFSaveDecompressed.Location = new Point(736, 404);
            MPFSaveDecompressed.Name = "MPFSaveDecompressed";
            MPFSaveDecompressed.Size = new Size(156, 23);
            MPFSaveDecompressed.TabIndex = 7;
            MPFSaveDecompressed.Text = "Save Decompressed";
            MPFSaveDecompressed.UseVisualStyleBackColor = true;
            MPFSaveDecompressed.Click += MPFSaveDecompressed_Click;
            // 
            // MpfModelList
            // 
            MpfModelList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            MpfModelList.FormattingEnabled = true;
            MpfModelList.ItemHeight = 15;
            MpfModelList.Location = new Point(6, 21);
            MpfModelList.Name = "MpfModelList";
            MpfModelList.Size = new Size(154, 409);
            MpfModelList.TabIndex = 6;
            MpfModelList.SelectedIndexChanged += MpfModelList_SelectedIndexChanged;
            // 
            // MpfWarning
            // 
            MpfWarning.AutoSize = true;
            MpfWarning.Location = new Point(166, 186);
            MpfWarning.Name = "MpfWarning";
            MpfWarning.Size = new Size(36, 15);
            MpfWarning.TabIndex = 5;
            MpfWarning.Text = "None";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(166, 171);
            label1.Name = "label1";
            label1.Size = new Size(52, 15);
            label1.TabIndex = 4;
            label1.Text = "Warning";
            // 
            // MpfBoneLoad
            // 
            MpfBoneLoad.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            MpfBoneLoad.Location = new Point(87, 433);
            MpfBoneLoad.Name = "MpfBoneLoad";
            MpfBoneLoad.Size = new Size(75, 23);
            MpfBoneLoad.TabIndex = 3;
            MpfBoneLoad.Text = "Bone Load";
            MpfBoneLoad.UseVisualStyleBackColor = true;
            MpfBoneLoad.Click += MpfBoneLoad_Click;
            // 
            // MpfExport
            // 
            MpfExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            MpfExport.Location = new Point(736, 433);
            MpfExport.Name = "MpfExport";
            MpfExport.Size = new Size(75, 23);
            MpfExport.TabIndex = 2;
            MpfExport.Text = "Export";
            MpfExport.UseVisualStyleBackColor = true;
            MpfExport.Click += MpfExport_Click;
            // 
            // MpfSave
            // 
            MpfSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            MpfSave.Location = new Point(817, 433);
            MpfSave.Name = "MpfSave";
            MpfSave.Size = new Size(75, 23);
            MpfSave.TabIndex = 1;
            MpfSave.Text = "Save";
            MpfSave.UseVisualStyleBackColor = true;
            MpfSave.Click += MpfSave_Click;
            // 
            // MpfLoad
            // 
            MpfLoad.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            MpfLoad.Location = new Point(6, 433);
            MpfLoad.Name = "MpfLoad";
            MpfLoad.Size = new Size(75, 23);
            MpfLoad.TabIndex = 0;
            MpfLoad.Text = "Load";
            MpfLoad.UseVisualStyleBackColor = true;
            MpfLoad.Click += MpfLoad_Click;
            // 
            // SSXBlurToolsWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(930, 514);
            Controls.Add(tabControl1);
            Name = "SSXBlurToolsWindow";
            Text = "SSXOnTourTools";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)MatUnknown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)MatUnknown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)MatFlagFactor).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button MpfLoad;
        private Button MpfSave;
        private Button MpfExport;
        private Button MpfBoneLoad;
        private Label MpfWarning;
        private Label label1;
        private ListBox MpfModelList;
        private Button MPFSaveDecompressed;
        private Button MpfImport;
        private GroupBox groupBox2;
        private Label label60;
        private Label MpfWeights;
        private Label label61;
        private Label FileID;
        private Label label62;
        private Label BoneCount;
        private Label label63;
        private Label ShapeKeyCount;
        private Label MaterialCount;
        private Label label64;
        private Label label65;
        private Label IkCount;
        private Label AltShapeKeyCount;
        private Label label3;
        private GroupBox groupBox4;
        private Label label74;
        private Label MorphGroupCount;
        private Label label75;
        private Label WeightGroupCount;
        private Label label76;
        private Label MaterialGroupCount;
        private Label label77;
        private Label TristripCountLabel;
        private Label label78;
        private Label VerticeCount;
        private Label MeshChunks;
        private Label label79;
        private GroupBox groupBox3;
        private Label label66;
        private NumericUpDown MatUnknown2;
        private Label label67;
        private NumericUpDown MatUnknown1;
        private Label label68;
        private NumericUpDown MatFlagFactor;
        private TextBox MatTextureFlag4;
        private Label label69;
        private TextBox MatTextureFlag3;
        private Label label70;
        private TextBox MatTextureFlag2;
        private Label label71;
        private TextBox MatTextureFlag1;
        private Label label72;
        private TextBox MatMainTexture;
        private Label label73;
        private ListBox MaterialList;
        private Label label2;
        private GroupBox groupBox1;
        private Label label57;
        private ComboBox TristripMethodList;
        private CheckBox BoneUpdateCheck;
        private CheckBox ImportAverageNormal;
    }
}