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
            LTGMode.SelectedIndex = 1;
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
                if (commonDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    if (Directory.GetFiles(commonDialog.FileName).Count() != 1)
                    {
                        if (openFileDialog.FileName.ToLower().Contains(".big"))
                        {
                            BigHandler bigHandler = new BigHandler();
                            bigHandler.LoadBig(openFileDialog.FileName);
                            bigHandler.ExtractBig(Application.StartupPath + "\\TempExtract");
                            string[] strings = Directory.GetFiles(Application.StartupPath + "\\TempExtract", "*.map", SearchOption.AllDirectories);
                            if (strings.Length != 0)
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
            trickyLevelInterface.InlineExporting = JSONInlineCheck.Checked;
            trickyLevelInterface.ExtractTrickyLevelFiles(LoadPath, ExportPath);
            ProjectPath = ExportPath;
            SaveConfig.Enabled = true;
            RebuildButton.Enabled = true;
            trickyConfig = new SSXTrickyConfig();
            trickyConfig.CreateJson(ExportPath + "/Config.ssx2");
            trickyLevelInterface.LoadAndVerifyFiles(ProjectPath);
            UpdateText();
        }

        void UpdateText()
        {
            PatchesLabel.Text = trickyLevelInterface.patchPoints.Patches.Count.ToString();
            InstancesLabel.Text = trickyLevelInterface.instancesJson.Instances.Count.ToString();
            ParticleInstancesLabel.Text = trickyLevelInterface.particleInstanceJson.Particles.Count.ToString();
            MaterialLabel.Text = trickyLevelInterface.materialJson.Materials.Count.ToString();
            SplinesLabel.Text = trickyLevelInterface.splineJsonHandler.Splines.Count.ToString();
            LightLabel.Text = trickyLevelInterface.lightJsonHandler.Lights.Count.ToString();
            ModelsLabel.Text = trickyLevelInterface.prefabJsonHandler.Prefabs.Count.ToString();
            ParticleModelLabels.Text = trickyLevelInterface.particleModelJsonHandler.ParticlePrefabs.Count.ToString();

            SkyMat.Text = trickyLevelInterface.SkyMaterialJson.Materials.Count.ToString();
            SkyModel.Text = trickyLevelInterface.SkyPrefabJsonHandler.Prefabs.Count.ToString();

            if (ProjectPath != null && ProjectPath != "")
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
                    trickyLevelInterface.Unilightmap = UnlitCheckbox.Checked;
                    trickyLevelInterface.PBDGenerate = GenPBD.Checked;
                    trickyLevelInterface.SSHGenerate = GenSSH.Checked;
                    trickyLevelInterface.LSSHGenerate = GenLSSH.Checked;
                    trickyLevelInterface.LTGGenerate = GenLTG.Checked;
                    trickyLevelInterface.MAPGenerate = GenMAP.Checked;
                    trickyLevelInterface.SkyPBDGenerate = GenSkyPBD.Checked;
                    trickyLevelInterface.SkySSHGenerate = GenSkySSH.Checked;
                    trickyLevelInterface.ADLGenerate = GenADL.Checked;
                    trickyLevelInterface.SSFGenerate = GenSSF.Checked;
                    trickyLevelInterface.AIPGenerate = GenAIP.Checked;
                    trickyLevelInterface.SOPGenerate = GenSOP.Checked;
                    trickyLevelInterface.LTGGenerateMode = LTGMode.SelectedIndex;


                    if (openFileDialog.FileName.ToLower().Contains(".big"))
                    {
                        Directory.CreateDirectory(Application.StartupPath + "\\TempExtract");
                        Directory.CreateDirectory(Application.StartupPath + "\\TempExtract\\Data");
                        Directory.CreateDirectory(Application.StartupPath + "\\TempExtract\\data\\models");
                        string InputPath = Application.StartupPath + "\\TempExtract\\data\\models\\" + Path.GetFileName(openFileDialog.FileName.ToLower()).Substring(0, Path.GetFileName(openFileDialog.FileName).Length - 3) + "map";
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
                if (trickyConfig.Version == 2)
                {
                    trickyLevelInterface = new TrickyLevelInterface();
                    SaveConfig.Enabled = true;
                    RebuildButton.Enabled = true;
                    ProjectPath = Path.GetDirectoryName(openFileDialog.FileName);

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

        private void PatchesTitleLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
