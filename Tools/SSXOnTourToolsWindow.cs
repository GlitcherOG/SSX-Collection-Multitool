using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SSXMultiTool.FileHandlers.Models;
using System.IO;
using SSXMultiTool.FileHandlers;

namespace SSXMultiTool.Tools
{
    public partial class SSXOnTourToolsWindow : Form
    {
        public SSXOnTourToolsWindow()
        {
            InitializeComponent();
        }
        SSXOnTourMPF onTourMPF = new SSXOnTourMPF();
        SSXOnTourPS2ModelCombiner modelCombiner = new SSXOnTourPS2ModelCombiner();
        private void MpfLoad_Click(object sender, EventArgs e)
        {

            //string[] directory = Directory.GetFiles(@"H:\Visual Studio Projects\SSX Modder\bin\Debug\disk\Mods\SSX ON TOUR", "*.mpf", SearchOption.AllDirectories);
            //for (int i = 0; i < directory.Length; i++)
            //{
            //    onTourMPF = new SSXOnTourMPF();
            //    onTourMPF.Load(directory[i]);
            //}
            //MessageBox.Show("Done");
            //return;

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Model File (*.mpf)|*.mpf|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (MpfHeaderChecker.DetectFileType(openFileDialog.FileName) != 3)
                {
                    MessageBox.Show(MpfHeaderChecker.TypeErrorMessage(MpfHeaderChecker.DetectFileType(openFileDialog.FileName)));
                    return;
                }
                onTourMPF = new SSXOnTourMPF();
                onTourMPF.Load(openFileDialog.FileName);
                modelCombiner = new SSXOnTourPS2ModelCombiner();
                modelCombiner.AddFile(onTourMPF);

                MpfWarning.Text = modelCombiner.CheckBones(0);


                MpfModelList.Items.Clear();
                for (int i = 0; i < onTourMPF.ModelList.Count; i++)
                {
                    MpfModelList.Items.Add(onTourMPF.ModelList[i].ModelName);
                }

            }
        }

        private void MpfSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog
            {
                Filter = "Model File (*.mpf)|*.mpf|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                modelCombiner.SaveMPF(openFileDialog.FileName, true);
            }
        }

        private void MpfExport_Click(object sender, EventArgs e)
        {
            if (MpfModelList.SelectedIndex != -1)
            {
                SaveFileDialog openFileDialog = new SaveFileDialog
                {
                    Filter = "Model File (*.glb)|*.glb|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    modelCombiner.MeshReassigned(MpfModelList.SelectedIndex);
                    glftHandler.SaveSSXOnTourGlft(openFileDialog.FileName, modelCombiner);
                }
            }
        }

        private void MpfBoneLoad_Click(object sender, EventArgs e)
        {
            if (modelCombiner.modelHandlers != null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Model File (*.mpf)|*.mpf|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    onTourMPF = new SSXOnTourMPF();
                    onTourMPF.Load(openFileDialog.FileName);
                    modelCombiner.AddBones(onTourMPF);

                    MpfWarning.Text = modelCombiner.CheckBones(0);
                }
            }
        }

        private void MPFSaveDecompressed_Click(object sender, EventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog
            {
                Filter = "Model File (*.mpf)|*.mpf|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                modelCombiner.SaveMPF(openFileDialog.FileName, false);
            }
        }

        private void MpfImport_Click(object sender, EventArgs e)
        {
            if (MpfModelList.SelectedIndex != -1)
            {
                if (modelCombiner.CheckBones(MpfModelList.SelectedIndex) != "")
                {
                    MessageBox.Show(modelCombiner.CheckBones(MpfModelList.SelectedIndex));
                    return;
                }
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "gltf File (*.glb)|*.glb|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SSXOnTourPS2ModelCombiner TempCombiner = null;

                    try
                    {
                        TempCombiner = glftHandler.LoadSSXOnTourGlft(openFileDialog.FileName);
                    }
                    catch
                    {
                        MessageBox.Show("Failed to Load File");
                    }

                    try
                    {
                        //modelCombiner.NormalAverage = ImportAverageNormal.Checked;
                        //modelCombiner.UpdateBones = BoneUpdateCheck.Checked;
                        modelCombiner.StartRegenMesh(TempCombiner, MpfModelList.SelectedIndex);
                        //UpdateData();
                    }
                    catch
                    {
                        MessageBox.Show("Failed to Convert File");
                    }
                }
            }
        }
    }
}
