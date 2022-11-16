using Microsoft.WindowsAPICodePack.Dialogs;
using SSXMultiTool.FileHandlers.LevelFiles;
using System.IO;
using SSXMultiTool.FileHandlers;
using System.Windows.Forms;
using SSXMultiTool.JsonFiles;
using SSXMultiTool.JsonFiles.Tricky;
using SSXMultiTool.FileHandlers.Models;
using SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2;

namespace SSXMultiTool
{
    public partial class TrickyProjectWindow : Form
    {
        public TrickyProjectWindow()
        {
            InitializeComponent();
        }

        private void ExtractLevel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Map File (*.map)|*.map|Big File (*.big)|*.big|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                CommonOpenFileDialog commonDialog = new CommonOpenFileDialog
                {
                    IsFolderPicker = true
                };
                if(commonDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    if (Directory.GetFiles(commonDialog.FileName).Count() != 1)
                    {
                        if (openFileDialog.FileName.Contains(".big"))
                        {

                        }
                        else
                        {
                            ExtractFiles(openFileDialog.FileName, commonDialog.FileName);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Folder Must Be Empty.");
                    }
                }
            }
        }
        TrickyLevelInterface trickyLevelInterface = new TrickyLevelInterface();
        SSXTrickyConfig trickyConfig = new SSXTrickyConfig();
        public string ProjectPath;
        void ExtractFiles(string StartPath, string ExportPath)
        {
            string LoadPath = StartPath.Substring(0, StartPath.Length - 4);
            trickyLevelInterface = new TrickyLevelInterface();
            trickyLevelInterface.ExtractTrickyLevelFiles(LoadPath, ExportPath);
            ProjectPath = ExportPath;
            SaveConfig.Enabled = true;
            RebuildButton.Enabled = true;
            trickyConfig = new SSXTrickyConfig();
            trickyConfig.CreateJson(ExportPath + "/Config.ssx");
            MessageBox.Show("Level Extracted");
            UpdateText();
        }

        void UpdateText()
        {
            PatchesLabel.Text = trickyLevelInterface.patchPoints.patches.Count.ToString();
            InstancesLabel.Text = trickyLevelInterface.instancesJson.instances.Count.ToString();
            ParticleInstancesLabel.Text = trickyLevelInterface.particleInstanceJson.particleJsons.Count.ToString();
            MaterialLabel.Text = trickyLevelInterface.materialJson.MaterialsJsons.Count.ToString();
            MaterialBlockLabel.Text = trickyLevelInterface.materialBlockJson.MaterialBlockJsons.Count.ToString();
            SplinesLabel.Text = trickyLevelInterface.splineJsonHandler.SplineJsons.Count.ToString();
            LightLabel.Text = trickyLevelInterface.lightJsonHandler.LightJsons.Count.ToString();
            TextureFlipLabel.Text = trickyLevelInterface.textureFlipbookJsonHandler.FlipbookJsons.Count.ToString();
            ModelsLabel.Text = trickyLevelInterface.modelJsonHandler.ModelJsons.Count.ToString();
            ParticleModelLabels.Text = trickyLevelInterface.particleModelJsonHandler.ParticleModelJsons.Count.ToString();
            if(ProjectPath!=null && ProjectPath != "")
            {
                TextureLabel.Text = Directory.GetFiles(ProjectPath + "/Textures", "*.png").Length.ToString();
                SykboxLabel.Text = Directory.GetFiles(ProjectPath + "/Skybox", "*.png").Length.ToString();
                LightmapLabel.Text = Directory.GetFiles(ProjectPath + "/Lightmaps", "*.png").Length.ToString();
            }
            LevelNameTextbox.Text = trickyConfig.LevelName;
            AuthorTextbox.Text = trickyConfig.Author;
            VersionTextbox.Text = trickyConfig.LevelVersion;
            DifficultyTextbox.Text = trickyConfig.Difficulty;
            LocationTextbox.Text = trickyConfig.Location;
            VerticalDropTextbox.Text = trickyConfig.Vertical;
            CourseLengthTextbox.Text = trickyConfig.Length;
            DescriptionTextbox.Text = trickyConfig.Description;
        }

        void ExtractBig(string BigPath, string ExportPath)
        {

        }

        private void RebuildButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(ProjectPath + "/config.ssx"))
            {
                SaveFileDialog openFileDialog = new SaveFileDialog
                {
                    Filter = "Map File (*.map)|*.map|Big File (*.big)|*.big|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    trickyLevelInterface = new TrickyLevelInterface();
                    trickyLevelInterface.AttemptLightingFix = EmulatorLigthFix.Checked;
                    trickyLevelInterface.BuildTrickyLevelFiles(ProjectPath, openFileDialog.FileName);
                    MessageBox.Show("Level Built");
                }
            }
            else
            {
                MessageBox.Show("Not a valid project");
            }
        }

        private void LoadProject_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Config File (*.ssx)|*.ssx|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                trickyConfig = SSXTrickyConfig.Load(openFileDialog.FileName);
                if (trickyConfig.Version == 2)
                {
                    trickyLevelInterface = new TrickyLevelInterface();
                    SaveConfig.Enabled = true;
                    RebuildButton.Enabled = true;
                    ProjectPath = Path.GetDirectoryName(openFileDialog.FileName);

                    objHandler handler = new objHandler();
                    handler.LoadFiles(ProjectPath+"/Models");
                    handler.GenerateTristripData();
                    PBDHandler pBDHandler = new PBDHandler();
                    pBDHandler.LoadPBD(ProjectPath + "/original/level.pbd");
                    pBDHandler.ImportModels(ProjectPath + "/test.mpf", handler);

                    trickyLevelInterface.LoadAndVerifyFiles(ProjectPath);
                    UpdateText();
                }
                else
                {
                    MessageBox.Show("Cannont Open Due to Incorrect Version");
                }
            }
        }

        private void SaveConfig_Click(object sender, EventArgs e)
        {
            trickyConfig.LevelName = LevelNameTextbox.Text;
            trickyConfig.Author = AuthorTextbox.Text;
            trickyConfig.LevelVersion = VersionTextbox.Text;
            trickyConfig.Difficulty = DifficultyTextbox.Text;
            trickyConfig.Location = LocationTextbox.Text;
            trickyConfig.Vertical = VerticalDropTextbox.Text;
            trickyConfig.Length = CourseLengthTextbox.Text;
            trickyConfig.Description = DescriptionTextbox.Text;
            trickyConfig.CreateJson(ProjectPath + "/Config.ssx");
        }
    }
}