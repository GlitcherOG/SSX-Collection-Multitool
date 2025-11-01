using Microsoft.WindowsAPICodePack.Dialogs;
using SSXMultiTool.FileHandlers.LevelFiles;
using System.IO;
using SSXMultiTool.FileHandlers;
using System.Windows.Forms;
using SSXMultiTool.JsonFiles;
using SSXMultiTool.JsonFiles.Tricky;
using SSXMultiTool.FileHandlers.Models;
using SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2;
using SSXMultiTool.Utilities;

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
                        if (ConsoleLogCheck.Checked)
                        {
                            ConsoleWindow.GenerateConsole();

                        }
                        this.Text = "Tricky Project Window (Extracting...)";
                        if (openFileDialog.FileName.ToLower().Contains(".big"))
                        {
                            BigHandler bigHandler = new BigHandler();
                            bigHandler.LoadBig(openFileDialog.FileName);
                            bigHandler.ExtractBig(Application.StartupPath + "\\TempExtract");
                            string[] strings = Directory.GetFiles(Application.StartupPath + "\\TempExtract", "*.map", SearchOption.AllDirectories);
                            if (strings.Length != 0)
                            {
                                ExtractFiles(strings[0], commonDialog.FileName);
                                this.Text = "Tricky Project Window (Extracting Done)";
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
                            //MessageBox.Show("Level Extracted");
                        }
                        this.Text = "Tricky Project Window (Extracting Done)";
                        //FlashWindow.Flash(this, 5);
                        if (ConsoleLogCheck.Checked)
                        {
                            ConsoleWindow.CloseConsole();
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
        public string ProjectPath = "";
        public string BuildPath = "";
        void ExtractFiles(string StartPath, string ExportPath)
        {
            Console.WriteLine("Begining Level Extraction...");
            string LoadPath = StartPath.Substring(0, StartPath.Length - 4);
            trickyLevelInterface = new TrickyLevelInterface();
            trickyLevelInterface.InlineExporting = JSONInlineCheck.Checked;
            trickyLevelInterface.ExtractTrickyLevelFiles(LoadPath, ExportPath);
            ProjectPath = ExportPath;
            SaveConfig.Enabled = true;
            RebuildButton.Enabled = true;
            RebuildNoPathButton.Enabled = true;
            trickyConfig = new SSXTrickyConfig();

            trickyConfig.BuildUniLightmap = UnlitLightmapCheckbox.Checked;
            trickyConfig.BuildPBDGenerate = GenPBD.Checked;
            trickyConfig.BuildSSHGenerate = GenSSH.Checked;
            trickyConfig.BuildLSSHGenerate = GenLSSH.Checked;
            trickyConfig.BuildLTGGenerate = GenLTG.Checked;
            trickyConfig.BuildMAPGenerate = GenMAP.Checked;
            trickyConfig.BuildSkyPBDGenerate = GenSkyPBD.Checked;
            trickyConfig.BuildSkySSHGenerate = GenSkySSH.Checked;
            trickyConfig.BuildADLGenerate = GenADL.Checked;
            trickyConfig.BuildSSFGenerate = GenSSF.Checked;
            trickyConfig.BuildAIPGenerate = GenAIP.Checked;
            trickyConfig.BuildSOPGenerate = GenSOP.Checked;
            trickyConfig.BuildLTGGenerateMode = LTGMode.SelectedIndex;

            trickyConfig.CreateJson(ExportPath + "/ConfigTricky.ssx");
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
            ModelsLabel.Text = trickyLevelInterface.prefabJsonHandler.Models.Count.ToString();
            ParticleModelLabels.Text = trickyLevelInterface.particleModelJsonHandler.ParticlePrefabs.Count.ToString();

            SkyMat.Text = trickyLevelInterface.SkyMaterialJson.Materials.Count.ToString();
            SkyModel.Text = trickyLevelInterface.SkyPrefabJsonHandler.Models.Count.ToString();

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
            if (File.Exists(ProjectPath + "/ConfigTricky.ssx"))
            {
                SaveFileDialog openFileDialog = new SaveFileDialog
                {
                    Filter = "Map File (*.map,*.big)|*.map;*.big|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (ConsoleLogCheck.Checked)
                    {
                        ConsoleWindow.GenerateConsole();
                    }
                    SaveConfig_Click(sender, e);

                    this.Text = "Tricky Project Window (Building...)";
                    trickyLevelInterface = new TrickyLevelInterface();
                    trickyLevelInterface.Unilightmap = UnlitLightmapCheckbox.Checked;
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
                    BuildPath = openFileDialog.FileName;

                    try
                    {
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
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ErrorUtil.Error);
                    }
                    this.Text = "Tricky Project Window (Building Done)";
                    //FlashWindow.Flash(this);
                    //MessageBox.Show("Level Built");
                    if (ConsoleLogCheck.Checked)
                    {
                        ConsoleWindow.CloseConsole();
                    }
                }
            }
            else
            {
                MessageBox.Show("Not a valid project");
            }
        }

        private void RebuildNoPathButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(ProjectPath + "/ConfigTricky.ssx"))
            {
                if (BuildPath == "")
                {
                    RebuildButton_Click(sender, e);
                    return;
                }

                if (ConsoleLogCheck.Checked)
                {
                    ConsoleWindow.GenerateConsole();
                }

                SaveConfig_Click(sender, e);

                this.Text = "Tricky Project Window (Building...)";
                trickyLevelInterface = new TrickyLevelInterface();
                trickyLevelInterface.Unilightmap = UnlitLightmapCheckbox.Checked;
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

                try
                {
                    if (BuildPath.Contains(".big"))
                    {
                        Directory.CreateDirectory(Application.StartupPath + "\\TempExtract");
                        Directory.CreateDirectory(Application.StartupPath + "\\TempExtract\\Data");
                        Directory.CreateDirectory(Application.StartupPath + "\\TempExtract\\data\\models");
                        string InputPath = Application.StartupPath + "\\TempExtract\\data\\models\\" + Path.GetFileName(BuildPath.ToLower()).Substring(0, Path.GetFileName(BuildPath).Length - 3) + "map";
                        trickyLevelInterface.BuildTrickyLevelFiles(ProjectPath, InputPath);
                        BigHandler bigHandler = new BigHandler();
                        bigHandler.LoadFolderC0FB(Application.StartupPath + "\\TempExtract");
                        bigHandler.CompressBuild = true;
                        bigHandler.BuildBig(BuildPath);
                        Directory.Delete(Application.StartupPath + "\\TempExtract", true);
                    }
                    else
                    {
                        trickyLevelInterface.BuildTrickyLevelFiles(ProjectPath, BuildPath);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ErrorUtil.Error);
                }
                this.Text = "Tricky Project Window (Building Done)";
                //FlashWindow.Flash(this);
                //MessageBox.Show("Level Built");
                if (ConsoleLogCheck.Checked)
                {
                    ConsoleWindow.CloseConsole();
                }
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

                if (trickyConfig.Version == 3)
                {
                    UnlitLightmapCheckbox.Checked = trickyConfig.BuildUniLightmap;
                    GenPBD.Checked = trickyConfig.BuildPBDGenerate;
                    GenSSH.Checked = trickyConfig.BuildSSHGenerate;
                    GenLSSH.Checked = trickyConfig.BuildLSSHGenerate;
                    GenLTG.Checked = trickyConfig.BuildLTGGenerate;
                    GenMAP.Checked = trickyConfig.BuildMAPGenerate;
                    GenSkyPBD.Checked = trickyConfig.BuildSkyPBDGenerate;
                    GenSkySSH.Checked = trickyConfig.BuildSkySSHGenerate;
                    GenADL.Checked = trickyConfig.BuildADLGenerate;
                    GenSSF.Checked = trickyConfig.BuildSSFGenerate;
                    GenAIP.Checked = trickyConfig.BuildAIPGenerate;
                    GenSOP.Checked = trickyConfig.BuildSOPGenerate;
                    LTGMode.SelectedIndex = trickyConfig.BuildLTGGenerateMode;
                    BuildPath = trickyConfig.BuildPath;

                    trickyLevelInterface = new TrickyLevelInterface();
                    SaveConfig.Enabled = true;
                    RebuildButton.Enabled = true;
                    RebuildNoPathButton.Enabled = true;
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

            trickyConfig.BuildUniLightmap = UnlitLightmapCheckbox.Checked;
            trickyConfig.BuildPBDGenerate = GenPBD.Checked;
            trickyConfig.BuildSSHGenerate = GenSSH.Checked;
            trickyConfig.BuildLSSHGenerate = GenLSSH.Checked;
            trickyConfig.BuildLTGGenerate = GenLTG.Checked;
            trickyConfig.BuildMAPGenerate = GenMAP.Checked;
            trickyConfig.BuildSkyPBDGenerate = GenSkyPBD.Checked;
            trickyConfig.BuildSkySSHGenerate = GenSkySSH.Checked;
            trickyConfig.BuildADLGenerate = GenADL.Checked;
            trickyConfig.BuildSSFGenerate = GenSSF.Checked;
            trickyConfig.BuildAIPGenerate = GenAIP.Checked;
            trickyConfig.BuildSOPGenerate = GenSOP.Checked;
            trickyConfig.BuildLTGGenerateMode = LTGMode.SelectedIndex;
            trickyConfig.BuildPath = BuildPath;

            trickyConfig.CreateJson(ProjectPath + "/ConfigTricky.ssx");
        }

        private void PatchesTitleLabel_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void GenLightingCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if(GenLightingCheckbox.Checked)
            {
                UnlitLightmapCheckbox.Enabled = false;
                UnlitInstancesCheckbox.Enabled = false;
            }
            else
            {
                UnlitLightmapCheckbox.Enabled = true;
                UnlitInstancesCheckbox.Enabled = true;
            }
        }
    }
}
