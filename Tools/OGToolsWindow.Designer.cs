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
            tabPage2 = new TabPage();
            LoadADF = new Button();
            ELFLdrSetup = new Button();
            label5 = new Label();
            label2 = new Label();
            groupBox4 = new GroupBox();
            label25 = new Label();
            MorphGroupCountPS2 = new Label();
            label23 = new Label();
            WeightGroupCountPS2 = new Label();
            label21 = new Label();
            MaterialGroupCountPS2 = new Label();
            label1 = new Label();
            TristripCountLabelPS2 = new Label();
            label4 = new Label();
            VerticeCountPS2 = new Label();
            MeshChunksPS2 = new Label();
            label3 = new Label();
            groupBox3 = new GroupBox();
            label9 = new Label();
            MatUnknown2MPF = new NumericUpDown();
            label7 = new Label();
            MatUnknown1MPF = new NumericUpDown();
            label20 = new Label();
            MatFlagFactorMPF = new NumericUpDown();
            MatTextureFlag4MPF = new TextBox();
            label19 = new Label();
            MatTextureFlag3MPF = new TextBox();
            label18 = new Label();
            MatTextureFlag2MPF = new TextBox();
            label17 = new Label();
            MatTextureFlag1MPF = new TextBox();
            label16 = new Label();
            MatMainTextureMPF = new TextBox();
            label15 = new Label();
            MaterialListMPF = new ListBox();
            groupBox2 = new GroupBox();
            label13 = new Label();
            WeightsPS2 = new Label();
            label14 = new Label();
            FileIDPS2 = new Label();
            label6 = new Label();
            BoneCountPS2 = new Label();
            label8 = new Label();
            ShapeKeyCountPS2 = new Label();
            MaterialCountPS2 = new Label();
            label11 = new Label();
            label10 = new Label();
            IkCountPS2 = new Label();
            groupBox1 = new GroupBox();
            BoneUpdateCheckMPF = new CheckBox();
            ImportAverageNormalMPF = new CheckBox();
            CharacterPartsMpf = new ListBox();
            MPFImport = new Button();
            MPFSave = new Button();
            MPFList = new ListBox();
            MpfExtract = new Button();
            MpfLoad = new Button();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MatUnknown2MPF).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MatUnknown1MPF).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MatFlagFactorMPF).BeginInit();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
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
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(groupBox4);
            tabPage1.Controls.Add(groupBox3);
            tabPage1.Controls.Add(groupBox2);
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Controls.Add(CharacterPartsMpf);
            tabPage1.Controls.Add(MPFImport);
            tabPage1.Controls.Add(MPFSave);
            tabPage1.Controls.Add(MPFList);
            tabPage1.Controls.Add(MpfExtract);
            tabPage1.Controls.Add(MpfLoad);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(768, 398);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "SSX (2000) MPF(Models)";
            tabPage1.UseVisualStyleBackColor = true;
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
            // LoadADF
            // 
            LoadADF.Location = new Point(112, 157);
            LoadADF.Name = "LoadADF";
            LoadADF.Size = new Size(75, 23);
            LoadADF.TabIndex = 2;
            LoadADF.Text = "Load Test";
            LoadADF.UseVisualStyleBackColor = true;
            LoadADF.Visible = false;
            LoadADF.Click += LoadADF_Click;
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
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 193);
            label5.Name = "label5";
            label5.Size = new Size(65, 15);
            label5.TabIndex = 40;
            label5.Text = "Mesh Parts";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 5);
            label2.Name = "label2";
            label2.Size = new Size(46, 15);
            label2.TabIndex = 39;
            label2.Text = "Models";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(label25);
            groupBox4.Controls.Add(MorphGroupCountPS2);
            groupBox4.Controls.Add(label23);
            groupBox4.Controls.Add(WeightGroupCountPS2);
            groupBox4.Controls.Add(label21);
            groupBox4.Controls.Add(MaterialGroupCountPS2);
            groupBox4.Controls.Add(label1);
            groupBox4.Controls.Add(TristripCountLabelPS2);
            groupBox4.Controls.Add(label4);
            groupBox4.Controls.Add(VerticeCountPS2);
            groupBox4.Controls.Add(MeshChunksPS2);
            groupBox4.Controls.Add(label3);
            groupBox4.Location = new Point(210, 102);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(292, 109);
            groupBox4.TabIndex = 38;
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
            // MorphGroupCountPS2
            // 
            MorphGroupCountPS2.AutoSize = true;
            MorphGroupCountPS2.Location = new Point(180, 64);
            MorphGroupCountPS2.Name = "MorphGroupCountPS2";
            MorphGroupCountPS2.Size = new Size(13, 15);
            MorphGroupCountPS2.TabIndex = 18;
            MorphGroupCountPS2.Text = "0";
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
            // WeightGroupCountPS2
            // 
            WeightGroupCountPS2.AutoSize = true;
            WeightGroupCountPS2.Location = new Point(96, 64);
            WeightGroupCountPS2.Name = "WeightGroupCountPS2";
            WeightGroupCountPS2.Size = new Size(13, 15);
            WeightGroupCountPS2.TabIndex = 16;
            WeightGroupCountPS2.Text = "0";
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
            // MaterialGroupCountPS2
            // 
            MaterialGroupCountPS2.AutoSize = true;
            MaterialGroupCountPS2.Location = new Point(6, 64);
            MaterialGroupCountPS2.Name = "MaterialGroupCountPS2";
            MaterialGroupCountPS2.Size = new Size(13, 15);
            MaterialGroupCountPS2.TabIndex = 14;
            MaterialGroupCountPS2.Text = "0";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 19);
            label1.Name = "label1";
            label1.Size = new Size(79, 15);
            label1.TabIndex = 6;
            label1.Text = "Tristrip Count";
            // 
            // TristripCountLabelPS2
            // 
            TristripCountLabelPS2.AutoSize = true;
            TristripCountLabelPS2.Location = new Point(6, 34);
            TristripCountLabelPS2.Name = "TristripCountLabelPS2";
            TristripCountLabelPS2.Size = new Size(13, 15);
            TristripCountLabelPS2.TabIndex = 7;
            TristripCountLabelPS2.Text = "0";
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
            // VerticeCountPS2
            // 
            VerticeCountPS2.AutoSize = true;
            VerticeCountPS2.Location = new Point(96, 34);
            VerticeCountPS2.Name = "VerticeCountPS2";
            VerticeCountPS2.Size = new Size(13, 15);
            VerticeCountPS2.TabIndex = 9;
            VerticeCountPS2.Text = "0";
            // 
            // MeshChunksPS2
            // 
            MeshChunksPS2.AutoSize = true;
            MeshChunksPS2.Location = new Point(180, 34);
            MeshChunksPS2.Name = "MeshChunksPS2";
            MeshChunksPS2.Size = new Size(13, 15);
            MeshChunksPS2.TabIndex = 12;
            MeshChunksPS2.Text = "0";
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
            groupBox3.Controls.Add(MatUnknown2MPF);
            groupBox3.Controls.Add(label7);
            groupBox3.Controls.Add(MatUnknown1MPF);
            groupBox3.Controls.Add(label20);
            groupBox3.Controls.Add(MatFlagFactorMPF);
            groupBox3.Controls.Add(MatTextureFlag4MPF);
            groupBox3.Controls.Add(label19);
            groupBox3.Controls.Add(MatTextureFlag3MPF);
            groupBox3.Controls.Add(label18);
            groupBox3.Controls.Add(MatTextureFlag2MPF);
            groupBox3.Controls.Add(label17);
            groupBox3.Controls.Add(MatTextureFlag1MPF);
            groupBox3.Controls.Add(label16);
            groupBox3.Controls.Add(MatMainTextureMPF);
            groupBox3.Controls.Add(label15);
            groupBox3.Controls.Add(MaterialListMPF);
            groupBox3.Location = new Point(508, 8);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(254, 218);
            groupBox3.TabIndex = 37;
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
            // MatUnknown2MPF
            // 
            MatUnknown2MPF.DecimalPlaces = 6;
            MatUnknown2MPF.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            MatUnknown2MPF.Location = new Point(167, 183);
            MatUnknown2MPF.Name = "MatUnknown2MPF";
            MatUnknown2MPF.Size = new Size(81, 23);
            MatUnknown2MPF.TabIndex = 42;
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
            // MatUnknown1MPF
            // 
            MatUnknown1MPF.DecimalPlaces = 6;
            MatUnknown1MPF.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            MatUnknown1MPF.Location = new Point(82, 183);
            MatUnknown1MPF.Name = "MatUnknown1MPF";
            MatUnknown1MPF.Size = new Size(81, 23);
            MatUnknown1MPF.TabIndex = 40;
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
            // MatFlagFactorMPF
            // 
            MatFlagFactorMPF.DecimalPlaces = 6;
            MatFlagFactorMPF.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            MatFlagFactorMPF.Location = new Point(167, 136);
            MatFlagFactorMPF.Name = "MatFlagFactorMPF";
            MatFlagFactorMPF.Size = new Size(81, 23);
            MatFlagFactorMPF.TabIndex = 38;
            // 
            // MatTextureFlag4MPF
            // 
            MatTextureFlag4MPF.Location = new Point(82, 135);
            MatTextureFlag4MPF.Name = "MatTextureFlag4MPF";
            MatTextureFlag4MPF.Size = new Size(79, 23);
            MatTextureFlag4MPF.TabIndex = 37;
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
            // MatTextureFlag3MPF
            // 
            MatTextureFlag3MPF.Location = new Point(169, 86);
            MatTextureFlag3MPF.Name = "MatTextureFlag3MPF";
            MatTextureFlag3MPF.Size = new Size(79, 23);
            MatTextureFlag3MPF.TabIndex = 35;
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
            // MatTextureFlag2MPF
            // 
            MatTextureFlag2MPF.Location = new Point(82, 86);
            MatTextureFlag2MPF.Name = "MatTextureFlag2MPF";
            MatTextureFlag2MPF.Size = new Size(79, 23);
            MatTextureFlag2MPF.TabIndex = 33;
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
            // MatTextureFlag1MPF
            // 
            MatTextureFlag1MPF.Location = new Point(169, 40);
            MatTextureFlag1MPF.Name = "MatTextureFlag1MPF";
            MatTextureFlag1MPF.Size = new Size(79, 23);
            MatTextureFlag1MPF.TabIndex = 31;
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
            // MatMainTextureMPF
            // 
            MatMainTextureMPF.Location = new Point(82, 40);
            MatMainTextureMPF.Name = "MatMainTextureMPF";
            MatMainTextureMPF.Size = new Size(79, 23);
            MatMainTextureMPF.TabIndex = 29;
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
            // MaterialListMPF
            // 
            MaterialListMPF.FormattingEnabled = true;
            MaterialListMPF.Location = new Point(6, 22);
            MaterialListMPF.Name = "MaterialListMPF";
            MaterialListMPF.Size = new Size(70, 184);
            MaterialListMPF.TabIndex = 27;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label13);
            groupBox2.Controls.Add(WeightsPS2);
            groupBox2.Controls.Add(label14);
            groupBox2.Controls.Add(FileIDPS2);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(BoneCountPS2);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(ShapeKeyCountPS2);
            groupBox2.Controls.Add(MaterialCountPS2);
            groupBox2.Controls.Add(label11);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(IkCountPS2);
            groupBox2.Location = new Point(210, 8);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(292, 88);
            groupBox2.TabIndex = 36;
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
            // WeightsPS2
            // 
            WeightsPS2.AutoSize = true;
            WeightsPS2.Location = new Point(6, 68);
            WeightsPS2.Name = "WeightsPS2";
            WeightsPS2.Size = new Size(13, 15);
            WeightsPS2.TabIndex = 25;
            WeightsPS2.Text = "0";
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
            // FileIDPS2
            // 
            FileIDPS2.AutoSize = true;
            FileIDPS2.Location = new Point(6, 34);
            FileIDPS2.Name = "FileIDPS2";
            FileIDPS2.Size = new Size(13, 15);
            FileIDPS2.TabIndex = 23;
            FileIDPS2.Text = "0";
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
            // BoneCountPS2
            // 
            BoneCountPS2.AutoSize = true;
            BoneCountPS2.Location = new Point(51, 34);
            BoneCountPS2.Name = "BoneCountPS2";
            BoneCountPS2.Size = new Size(13, 15);
            BoneCountPS2.TabIndex = 14;
            BoneCountPS2.Text = "0";
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
            // ShapeKeyCountPS2
            // 
            ShapeKeyCountPS2.AutoSize = true;
            ShapeKeyCountPS2.Location = new Point(216, 34);
            ShapeKeyCountPS2.Name = "ShapeKeyCountPS2";
            ShapeKeyCountPS2.Size = new Size(13, 15);
            ShapeKeyCountPS2.TabIndex = 20;
            ShapeKeyCountPS2.Text = "0";
            // 
            // MaterialCountPS2
            // 
            MaterialCountPS2.AutoSize = true;
            MaterialCountPS2.Location = new Point(96, 34);
            MaterialCountPS2.Name = "MaterialCountPS2";
            MaterialCountPS2.Size = new Size(13, 15);
            MaterialCountPS2.TabIndex = 16;
            MaterialCountPS2.Text = "0";
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
            // IkCountPS2
            // 
            IkCountPS2.AutoSize = true;
            IkCountPS2.Location = new Point(157, 34);
            IkCountPS2.Name = "IkCountPS2";
            IkCountPS2.Size = new Size(13, 15);
            IkCountPS2.TabIndex = 18;
            IkCountPS2.Text = "0";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(BoneUpdateCheckMPF);
            groupBox1.Controls.Add(ImportAverageNormalMPF);
            groupBox1.Location = new Point(204, 284);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(156, 78);
            groupBox1.TabIndex = 35;
            groupBox1.TabStop = false;
            groupBox1.Text = "Import Settings";
            // 
            // BoneUpdateCheckMPF
            // 
            BoneUpdateCheckMPF.AutoSize = true;
            BoneUpdateCheckMPF.Location = new Point(6, 47);
            BoneUpdateCheckMPF.Name = "BoneUpdateCheckMPF";
            BoneUpdateCheckMPF.Size = new Size(131, 19);
            BoneUpdateCheckMPF.TabIndex = 1;
            BoneUpdateCheckMPF.Text = "Update Bones (WIP)";
            BoneUpdateCheckMPF.UseVisualStyleBackColor = true;
            // 
            // ImportAverageNormalMPF
            // 
            ImportAverageNormalMPF.AutoSize = true;
            ImportAverageNormalMPF.Location = new Point(6, 22);
            ImportAverageNormalMPF.Name = "ImportAverageNormalMPF";
            ImportAverageNormalMPF.Size = new Size(117, 19);
            ImportAverageNormalMPF.TabIndex = 0;
            ImportAverageNormalMPF.Text = "Average Normals";
            ImportAverageNormalMPF.UseVisualStyleBackColor = true;
            // 
            // CharacterPartsMpf
            // 
            CharacterPartsMpf.FormattingEnabled = true;
            CharacterPartsMpf.Location = new Point(6, 210);
            CharacterPartsMpf.Name = "CharacterPartsMpf";
            CharacterPartsMpf.Size = new Size(192, 184);
            CharacterPartsMpf.TabIndex = 34;
            // 
            // MPFImport
            // 
            MPFImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            MPFImport.Location = new Point(285, 368);
            MPFImport.Name = "MPFImport";
            MPFImport.Size = new Size(75, 23);
            MPFImport.TabIndex = 33;
            MPFImport.Text = "Import Model";
            MPFImport.UseVisualStyleBackColor = true;
            // 
            // MPFSave
            // 
            MPFSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            MPFSave.Location = new Point(687, 368);
            MPFSave.Name = "MPFSave";
            MPFSave.Size = new Size(75, 23);
            MPFSave.TabIndex = 32;
            MPFSave.Text = "Save MPF";
            MPFSave.UseVisualStyleBackColor = true;
            // 
            // MPFList
            // 
            MPFList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            MPFList.FormattingEnabled = true;
            MPFList.Location = new Point(6, 21);
            MPFList.Name = "MPFList";
            MPFList.Size = new Size(192, 169);
            MPFList.TabIndex = 31;
            // 
            // MpfExtract
            // 
            MpfExtract.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            MpfExtract.Location = new Point(606, 369);
            MpfExtract.Name = "MpfExtract";
            MpfExtract.Size = new Size(75, 23);
            MpfExtract.TabIndex = 30;
            MpfExtract.Text = "Extract";
            MpfExtract.UseVisualStyleBackColor = true;
            MpfExtract.Click += MpfExtract_Click;
            // 
            // MpfLoad
            // 
            MpfLoad.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            MpfLoad.Location = new Point(204, 368);
            MpfLoad.Name = "MpfLoad";
            MpfLoad.Size = new Size(75, 23);
            MpfLoad.TabIndex = 29;
            MpfLoad.Text = "Load MPF";
            MpfLoad.UseVisualStyleBackColor = true;
            MpfLoad.Click += MpfLoad_Click;
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
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)MatUnknown2MPF).EndInit();
            ((System.ComponentModel.ISupportInitialize)MatUnknown1MPF).EndInit();
            ((System.ComponentModel.ISupportInitialize)MatFlagFactorMPF).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button ELFLdrSetup;
        private Button LoadADF;
        private Label label5;
        private Label label2;
        private GroupBox groupBox4;
        private Label label25;
        private Label MorphGroupCountPS2;
        private Label label23;
        private Label WeightGroupCountPS2;
        private Label label21;
        private Label MaterialGroupCountPS2;
        private Label label1;
        private Label TristripCountLabelPS2;
        private Label label4;
        private Label VerticeCountPS2;
        private Label MeshChunksPS2;
        private Label label3;
        private GroupBox groupBox3;
        private Label label9;
        private NumericUpDown MatUnknown2MPF;
        private Label label7;
        private NumericUpDown MatUnknown1MPF;
        private Label label20;
        private NumericUpDown MatFlagFactorMPF;
        private TextBox MatTextureFlag4MPF;
        private Label label19;
        private TextBox MatTextureFlag3MPF;
        private Label label18;
        private TextBox MatTextureFlag2MPF;
        private Label label17;
        private TextBox MatTextureFlag1MPF;
        private Label label16;
        private TextBox MatMainTextureMPF;
        private Label label15;
        private ListBox MaterialListMPF;
        private GroupBox groupBox2;
        private Label label13;
        private Label WeightsPS2;
        private Label label14;
        private Label FileIDPS2;
        private Label label6;
        private Label BoneCountPS2;
        private Label label8;
        private Label ShapeKeyCountPS2;
        private Label MaterialCountPS2;
        private Label label11;
        private Label label10;
        private Label IkCountPS2;
        private GroupBox groupBox1;
        private CheckBox BoneUpdateCheckMPF;
        private CheckBox ImportAverageNormalMPF;
        private ListBox CharacterPartsMpf;
        private Button MPFImport;
        private Button MPFSave;
        private ListBox MPFList;
        private Button MpfExtract;
        private Button MpfLoad;
    }
}