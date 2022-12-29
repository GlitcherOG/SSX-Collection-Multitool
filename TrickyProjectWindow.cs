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
                Filter = "Map File (*.map,*.big)|*.map;*.big|All files (*.*)|*.*",
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
                        if (openFileDialog.FileName.ToLower().Contains(".big"))
                        {
                            BigHandler bigHandler = new BigHandler();
                            bigHandler.LoadBig(openFileDialog.FileName);
                            bigHandler.ExtractBig(Application.StartupPath + "\\TempExtract");
                            string[] strings = Directory.GetFiles(Application.StartupPath + "\\TempExtract", "*.map", SearchOption.AllDirectories);
                            if(strings.Length!=0)
                            {
                                ExtractFiles(strings[0], commonDialog.FileName);
                                MessageBox.Show("Level Extracted");
                            }
                            else
                            {
                                MessageBox.Show("Missing Level Files");
                            }
                            Directory.Delete(Application.StartupPath + "\\TempExtract", true);
                        }
                        else
                        {
                            ExtractFiles(openFileDialog.FileName, commonDialog.FileName);
                            MessageBox.Show("Level Extracted");
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
            ModelsLabel.Text = trickyLevelInterface.prefabJsonHandler.PrefabJsons.Count.ToString();
            ParticleModelLabels.Text = trickyLevelInterface.particleModelJsonHandler.ParticleModelJsons.Count.ToString();
            if(ProjectPath!=null && ProjectPath != "")
            {
                TextureLabel.Text = Directory.GetFiles(ProjectPath + "/Textures", "*.png").Length.ToString();
                SykboxLabel.Text = Directory.GetFiles(ProjectPath + "/Skybox/Textures", "*.png").Length.ToString();
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

        private void RebuildButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(ProjectPath + "/config.ssx"))
            {
                SaveFileDialog openFileDialog = new SaveFileDialog
                {
                    Filter = "Map File (*.map,*.big)|*.map;*.big|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    trickyLevelInterface = new TrickyLevelInterface();
                    trickyLevelInterface.AttemptLightingFix = EmulatorLigthFix.Checked;
                    trickyLevelInterface.LTGRegenerate = RegenLTG.Checked;
                    if (openFileDialog.FileName.ToLower().Contains(".big"))
                    {
                        Directory.CreateDirectory(Application.StartupPath + "\\TempExtract");
                        Directory.CreateDirectory(Application.StartupPath + "\\TempExtract\\Data");
                        Directory.CreateDirectory(Application.StartupPath + "\\TempExtract\\data\\models");
                        string InputPath = Application.StartupPath + "\\TempExtract\\data\\models\\" + Path.GetFileName(openFileDialog.FileName.ToLower()).Substring(0, Path.GetFileName(openFileDialog.FileName).Length-3) + "map";
                        trickyLevelInterface.BuildTrickyLevelFiles(ProjectPath, InputPath);
                        BigHandler bigHandler = new BigHandler();
                        bigHandler.LoadFolderC0FB(Application.StartupPath + "\\TempExtract");
                        bigHandler.CompressBuild = true;
                        bigHandler.BuildBig(openFileDialog.FileName);
                        Directory.Delete(Application.StartupPath + "\\TempExtract", true);
                    }
                    else
                    {
                        trickyLevelInterface.BuildTrickyLevelFiles(ProjectPath, openFileDialog.FileName);
                    }
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
                if (trickyConfig.Version == 4)
                {
                    trickyLevelInterface = new TrickyLevelInterface();
                    SaveConfig.Enabled = true;
                    RebuildButton.Enabled = true;
                    ProjectPath = Path.GetDirectoryName(openFileDialog.FileName);

                    PBDHandler pBDHandler = new PBDHandler();
                    pBDHandler.LoadPBD(ProjectPath + "/original/level.pbd");

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