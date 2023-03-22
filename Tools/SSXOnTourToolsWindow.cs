using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SSXMultiTool.FileHandlers.Models.OnTour;
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
                UpdateData();

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
                    if (MpfHeaderChecker.DetectFileType(openFileDialog.FileName) != 3)
                    {
                        MessageBox.Show(MpfHeaderChecker.TypeErrorMessage(MpfHeaderChecker.DetectFileType(openFileDialog.FileName)));
                        return;
                    }


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
                        modelCombiner.NormalAverage = ImportAverageNormal.Checked;
                        modelCombiner.UpdateBones = BoneUpdateCheck.Checked;
                        modelCombiner.StartRegenMesh(TempCombiner, MpfModelList.SelectedIndex);
                        UpdateData();
                    }
                    catch
                    {
                        MessageBox.Show("Failed to Convert File");
                    }
                }
            }
        }

        void UpdateData()
        {
            if (MpfModelList.SelectedIndex != -1)
            {
                var TempModel = modelCombiner.modelHandlers.ModelList[MpfModelList.SelectedIndex];

                FileID.Text = TempModel.FileID.ToString();
                BoneCount.Text = TempModel.BoneCount.ToString();
                MaterialCount.Text = TempModel.MaterialCount.ToString();
                IkCount.Text = "0";
                ShapeKeyCount.Text = TempModel.MorphCount.ToString();
                MpfWeights.Text = TempModel.WeightCount.ToString();

                TristripCountLabel.Text = modelCombiner.TristripCount(TempModel).ToString();
                VerticeCount.Text = modelCombiner.VerticeCount(TempModel).ToString();
                MeshChunks.Text = modelCombiner.ChunkCount(TempModel).ToString();
                MaterialGroupCount.Text = TempModel.MaterialGroupList.Count.ToString();
                WeightGroupCount.Text = modelCombiner.WeigthRefCount(TempModel).ToString();
                MorphGroupCount.Text = modelCombiner.MorphGroupCount(TempModel).ToString();

                MaterialList.Items.Clear();
                for (int i = 0; i < TempModel.MaterialList.Count; i++)
                {
                    MaterialList.Items.Add(TempModel.MaterialList[i].MainTexture);
                }
            }
            else
            {
                FileID.Text = "0";
                BoneCount.Text = "0";
                MaterialCount.Text = "0";
                IkCount.Text = "0";
                ShapeKeyCount.Text = "0";
                MpfWeights.Text = "0";

                TristripCountLabel.Text = "0";
                VerticeCount.Text = "0";
                MeshChunks.Text = "0";
                MaterialGroupCount.Text = "0";
                WeightGroupCount.Text = "0";
                MorphGroupCount.Text = "0";
                MaterialList.Items.Clear();
            }
        }

        private void MpfModelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(MpfModelList.SelectedIndex != -1)
            {
                UpdateData();
            }
        }
        bool DisableUpdate = false;
        private void MaterialList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MaterialList.SelectedIndex != -1 && !DisableUpdate)
            {
                DisableUpdate = true;
                var TempModel = modelCombiner.modelHandlers.ModelList[MpfModelList.SelectedIndex].MaterialList[MaterialList.SelectedIndex];
                MatMainTexture.Text = TempModel.MainTexture;
                MatTextureFlag1.Text = TempModel.Texture1;
                MatTextureFlag2.Text = TempModel.Texture2;
                MatTextureFlag3.Text = TempModel.Texture3;
                MatTextureFlag4.Text = TempModel.Texture4;

                MatFlagFactor.Value = (decimal)TempModel.FactorFloat;
                MatUnknown1.Value = (decimal)TempModel.Unused1Float;
                MatUnknown2.Value = (decimal)TempModel.Unused2Float;

                DisableUpdate = false;
            }
        }

        private void MatUpdate(object sender, EventArgs e)
        {
            if (!DisableUpdate && MaterialList.SelectedIndex != -1)
            {
                DisableUpdate = true;
                var TempModel = modelCombiner.modelHandlers.ModelList[MpfModelList.SelectedIndex];
                var TempMaterial = TempModel.MaterialList[MaterialList.SelectedIndex];

                TempMaterial.MainTexture = MatMainTexture.Text;
                TempMaterial.Texture1 = MatTextureFlag1.Text;
                TempMaterial.Texture2 = MatTextureFlag2.Text;
                TempMaterial.Texture3 = MatTextureFlag3.Text;
                TempMaterial.Texture4 = MatTextureFlag4.Text;

                TempMaterial.FactorFloat = (float)MatFlagFactor.Value;
                TempMaterial.Unused1Float = (float)MatUnknown1.Value;
                TempMaterial.Unused2Float = (float)MatUnknown2.Value;

                MaterialList.Items[MaterialList.SelectedIndex] = TempMaterial.MainTexture;

                TempModel.MaterialList[MaterialList.SelectedIndex] = TempMaterial;
                modelCombiner.modelHandlers.ModelList[MpfModelList.SelectedIndex] = TempModel;

                DisableUpdate = false;
            }
        }
    }
}
