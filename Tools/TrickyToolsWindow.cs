using Microsoft.WindowsAPICodePack.Dialogs;
using SSXMultiTool.FileHandlers;
using SSXMultiTool.FileHandlers.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SSXMultiTool
{
    public partial class TrickyToolsWindow : Form
    {
        public TrickyToolsWindow()
        {
            InitializeComponent();
        }
        TrickyModelCombiner trickyModel = new TrickyModelCombiner();
        TrickyMPFModelHandler trickyMPF = new TrickyMPFModelHandler();

        private void MPFLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Model File (*.mpf)|*.mpf|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                trickyMPF = new TrickyMPFModelHandler();
                trickyMPF.load(openFileDialog.FileName);
                TristripMethodList.SelectedIndex = 0;

                int Type = trickyModel.DectectModelType(trickyMPF);

                if((Type==0 && trickyModel.Head!=null)|| (Type == 1 && trickyModel.Body != null))
                {
                    MpfList.Items.Clear();
                    MpfList.Items.Add("Character 3000");
                    MpfList.Items.Add("Character 1500");
                    MpfList.Items.Add("Character 750");
                    MpfList.Items.Add("Character Shadow 750");
                }
                else
                {
                    if(trickyModel.Body == null)
                    {
                        MpfList.Items.Clear();
                        MpfList.Items.Add("Please Load Matching Body File");
                    }
                    else if (trickyModel.Head == null)
                    {
                        MpfList.Items.Clear();
                        MpfList.Items.Add("Please Load Matching Head File");
                    }
                    UpdateData();
                }

                if (Type == 2)
                {
                    MpfList.Items.Clear();
                    for (int i = 0; i < trickyMPF.ModelList.Count; i++)
                    {
                        MpfList.Items.Add(trickyMPF.ModelList[i].FileName);
                    }
                    UpdateData();
                }
            }

        }


        private void MPFExtract_Click(object sender, EventArgs e)
        {
            if (MpfList.SelectedIndex != -1)
            {
                SaveFileDialog openFileDialog = new SaveFileDialog
                {
                    Filter = "gltf File (*.glb)|*.glb|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    trickyModel.StartReassignMesh(MpfList.SelectedIndex);

                    glftHandler.SaveTrickyglTF(openFileDialog.FileName, trickyModel);
                }
            }
        }

        private void ELFLdrSetup_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog commonDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };
            if (commonDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (File.Exists(commonDialog.FileName + "\\DATA\\MODELS\\ALASKA.BIG"))
                {
                    BigHandler bigHandler = new BigHandler();
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\ALASKA.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\ALOHA.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\ELYSIUM.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\GARI.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\MEGAPLE.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\MERQUER.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\MESA.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\PIPE.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\SNOW.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\SSXFE.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\TRICK.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\UNTRACK.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\ALASKA.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\ALOHA.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\ELYSIUM.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\GARI.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\MEGAPLE.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\MERQUER.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\MESA.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\PIPE.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\SNOW.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\SSXFE.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\TRICK.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\UNTRACK.BIG");

                    MessageBox.Show("Finished Setting up Path.");
                }
                else
                {
                    MessageBox.Show("Unknown Game Path or Game Already Extracted");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog
            {
                Filter = "Model File (*.mpf)|*.mpf|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (trickyModel.Board != null)
                {
                    trickyMPF = trickyModel.Board;

                    trickyMPF.Save(openFileDialog.FileName);
                }
                else
                {
                    string Path = openFileDialog.FileName.Remove(openFileDialog.FileName.Length-8,8);

                    trickyMPF = trickyModel.Body;

                    trickyMPF.Save(Path + "body.mpf");

                    trickyMPF = trickyModel.Head;

                    trickyMPF.Save(Path + "head.mpf");
                }
            }
        }

        private void MPFImport_Click(object sender, EventArgs e)
        {
            if (MpfList.SelectedIndex != -1)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "gltf File (*.glb)|*.glb|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if ((trickyModel.Board != null) || (trickyModel.Head != null && trickyModel.Body != null))
                    {
                        TrickyModelCombiner TempCombiner = null;

                        try
                        {
                            TempCombiner = glftHandler.LoadGlft(openFileDialog.FileName);
                        }
                        catch
                        {
                            MessageBox.Show("Failed to Load File");
                        }
                        if (TempCombiner != null)
                        {
                            try
                            {
                                trickyModel.NormalAverage = ImportAverageNormal.Checked;
                                trickyModel.BoneUpdate = BoneUpdateCheck.Checked;
                                trickyModel.TristripMode = TristripMethodList.SelectedIndex;
                                trickyModel.StartRegenMesh(TempCombiner, MpfList.SelectedIndex);

                                UpdateData();
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

        void UpdateData(TrickyMPFModelHandler.MPFModelHeader? modelHeader = null)
        {
            if(modelHeader!=null)
            {
                FileID.Text = modelHeader.Value.FileID.ToString();
                BoneCount.Text = modelHeader.Value.boneDatas.Count.ToString();
                MaterialCount.Text = modelHeader.Value.materialDatas.Count.ToString();
                IkCount.Text = modelHeader.Value.iKPoints.Count.ToString();
                ShapeKeyCount.Text = modelHeader.Value.MorphKeyCount.ToString();
                MpfWeights.Text = modelHeader.Value.boneWeightHeader.Count.ToString();

                TristripCountLabel.Text = trickyModel.TristripCount(modelHeader.Value).ToString();
                VerticeCount.Text = trickyModel.VerticeCount(modelHeader.Value).ToString();
                MeshChunks.Text = trickyModel.ChunkCount(modelHeader.Value).ToString();
                MaterialGroupCount.Text = modelHeader.Value.MeshGroups.Count.ToString();
                WeightGroupCount.Text = trickyModel.WeigthRefCount(modelHeader.Value).ToString();
                MorphGroupCount.Text = trickyModel.MorphGroupCount(modelHeader.Value).ToString();

                MaterialList.Items.Clear();
                MpfUpdateMaterial();
                for (int i = 0; i < modelHeader.Value.materialDatas.Count; i++)
                {
                    MaterialList.Items.Add(modelHeader.Value.materialDatas[i].MainTexture);
                }
            }
            else
            {
                //Modle Header Info
                FileID.Text = "0";
                BoneCount.Text = "0";
                MaterialCount.Text = "0";
                IkCount.Text = "0";
                ShapeKeyCount.Text = "0";
                MpfWeights.Text = "0";

                //Model Data
                TristripCountLabel.Text = "0";
                VerticeCount.Text = "0";
                MeshChunks.Text = "0";
                MaterialGroupCount.Text = "0";
                WeightGroupCount.Text = "0";
                MorphGroupCount.Text = "0";

                //Material Items
                MaterialList.Items.Clear();
                MpfUpdateMaterial();
            }
        }
        bool MatDisableUpdate;
        void MpfUpdateMaterial(TrickyMPFModelHandler.MPFModelHeader? modelHeader = null)
        {
            if(MaterialList.SelectedIndex!=-1 && modelHeader!=null)
            {
                MatDisableUpdate = true;
                MatMainTexture.Text = modelHeader.Value.materialDatas[MaterialList.SelectedIndex].MainTexture;
                MatTextureFlag1.Text = modelHeader.Value.materialDatas[MaterialList.SelectedIndex].Texture1;
                MatTextureFlag2.Text = modelHeader.Value.materialDatas[MaterialList.SelectedIndex].Texture2;
                MatTextureFlag3.Text = modelHeader.Value.materialDatas[MaterialList.SelectedIndex].Texture3;
                MatTextureFlag4.Text = modelHeader.Value.materialDatas[MaterialList.SelectedIndex].Texture4;
                MatFlagFactor.Value = (decimal)modelHeader.Value.materialDatas[MaterialList.SelectedIndex].FactorFloat;
                MatUnknown1.Value = (decimal)modelHeader.Value.materialDatas[MaterialList.SelectedIndex].Unused1Float;
                MatUnknown2.Value = (decimal)modelHeader.Value.materialDatas[MaterialList.SelectedIndex].Unused2Float;
                MatDisableUpdate = false;
            }
            else
            {
                MatDisableUpdate = true;
                MatMainTexture.Text = "";
                MatTextureFlag1.Text = "";
                MatTextureFlag2.Text = "";
                MatTextureFlag3.Text = "";
                MatTextureFlag4.Text = "";
                MatFlagFactor.Value = 0;
                MatUnknown1.Value = 0;
                MatUnknown2.Value = 0;
                MatDisableUpdate = false;
            }
        }

        private void MpfList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MpfList.SelectedIndex != -1)
            {
                if (trickyModel.Board != null)
                {
                    UpdateData(trickyModel.Board.ModelList[MpfList.SelectedIndex]);
                }
                else
                {
                    UpdateData();
                }
                RegeneratePartList();
            }
        }

        void RegeneratePartList()
        {
            CharacterParts.Items.Clear();
            int MeshID = MpfList.SelectedIndex;
            if (trickyModel.Body!=null && trickyModel.Head != null)
            {
                for (int i = 0; i < trickyModel.Body.ModelList.Count; i++)
                {
                    if ((MeshID == 0 && trickyModel.Body.ModelList[i].FileName.Contains("3000")) ||
                        (MeshID == 1 && trickyModel.Body.ModelList[i].FileName.Contains("1500")) ||
                        (MeshID == 2 && trickyModel.Body.ModelList[i].FileName.Contains("750") && !trickyModel.Body.ModelList[i].FileName.ToLower().Contains("shdw")) ||
                        (MeshID == 3 && trickyModel.Body.ModelList[i].FileName.ToLower().Contains("shdw")))
                    {
                        CharacterParts.Items.Add(trickyModel.Body.ModelList[i].FileName);
                    }
                }

                //Head
                for (int i = 0; i < trickyModel.Head.ModelList.Count; i++)
                {
                    if ((MeshID == 0 && trickyModel.Head.ModelList[i].FileName.Contains("3000")) ||
                        (MeshID == 1 && trickyModel.Head.ModelList[i].FileName.Contains("1500")) ||
                        (MeshID == 2 && trickyModel.Head.ModelList[i].FileName.Contains("750") && !trickyModel.Head.ModelList[i].FileName.ToLower().Contains("shdw")) ||
                        (MeshID == 3 && trickyModel.Head.ModelList[i].FileName.ToLower().Contains("shdw")))
                    {

                        CharacterParts.Items.Add(trickyModel.Head.ModelList[i].FileName);
                    }
                }
            }
        }

        private void CharacterParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(CharacterParts.SelectedIndex!=-1)
            {
                string LookingFor = CharacterParts.Items[CharacterParts.SelectedIndex].ToString();
                int MeshID = -1;

                for (int i = 0; i < trickyModel.Body.ModelList.Count; i++)
                {
                    if(LookingFor== trickyModel.Body.ModelList[i].FileName)
                    {
                        MeshID = i;
                        UpdateData(trickyModel.Body.ModelList[i]);
                        break;
                    }
                }

                if (MeshID == -1)
                {
                    //Head
                    for (int i = 0; i < trickyModel.Head.ModelList.Count; i++)
                    {
                        if (LookingFor == trickyModel.Head.ModelList[i].FileName)
                        {
                            MeshID = i;
                            UpdateData(trickyModel.Head.ModelList[i]);
                        }
                    }
                }

            }
        }

        private void MaterialList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(MaterialList.SelectedIndex != -1)
            {
                if (trickyModel.Board != null)
                {
                    MpfUpdateMaterial(trickyModel.Board.ModelList[MpfList.SelectedIndex]);
                }
                else if (trickyModel.Body != null && trickyModel.Head != null)
                {
                    string LookingFor = CharacterParts.Items[CharacterParts.SelectedIndex].ToString();
                    int MeshID = -1;
                    for (int i = 0; i < trickyModel.Body.ModelList.Count; i++)
                    {
                        if (LookingFor == trickyModel.Body.ModelList[i].FileName)
                        {
                            MeshID = i;
                            MpfUpdateMaterial(trickyModel.Body.ModelList[i]);
                            break;
                        }
                    }

                    if (MeshID == -1)
                    {
                        //Head
                        for (int i = 0; i < trickyModel.Head.ModelList.Count; i++)
                        {
                            if (LookingFor == trickyModel.Head.ModelList[i].FileName)
                            {
                                MeshID = i;
                                MpfUpdateMaterial(trickyModel.Head.ModelList[i]);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    MpfUpdateMaterial();
                }
            }
        }

        private void MPFUpdateMat(object sender, EventArgs e)
        {
            if(MaterialList.SelectedIndex!=-1&&!MatDisableUpdate)
            {
                MatDisableUpdate = true;

                TrickyMPFModelHandler.MaterialData TempMat = new TrickyMPFModelHandler.MaterialData();
                //Load Material
                if (trickyModel.Board!=null)
                {
                    TempMat = trickyModel.Board.ModelList[MpfList.SelectedIndex].materialDatas[MaterialList.SelectedIndex];
                }
                else if (trickyModel.Head!=null && trickyModel.Body!=null)
                {
                    string LookingFor = CharacterParts.Items[CharacterParts.SelectedIndex].ToString();
                    int MeshID = -1;
                    for (int i = 0; i < trickyModel.Body.ModelList.Count; i++)
                    {
                        if (LookingFor == trickyModel.Body.ModelList[i].FileName)
                        {
                            MeshID = i;
                            TempMat = trickyModel.Body.ModelList[i].materialDatas[MaterialList.SelectedIndex];
                            break;
                        }
                    }

                    if (MeshID == -1)
                    {
                        //Head
                        for (int i = 0; i < trickyModel.Head.ModelList.Count; i++)
                        {
                            if (LookingFor == trickyModel.Head.ModelList[i].FileName)
                            {
                                MeshID = i;
                                TempMat = trickyModel.Head.ModelList[i].materialDatas[MaterialList.SelectedIndex];
                                break;
                            }
                        }
                    }
                }

                TempMat.MainTexture = MatMainTexture.Text;
                TempMat.Texture1 = MatTextureFlag1.Text;
                TempMat.Texture2 = MatTextureFlag2.Text;
                TempMat.Texture3 = MatTextureFlag3.Text;
                TempMat.Texture4 = MatTextureFlag4.Text;
                TempMat.FactorFloat = (float)MatFlagFactor.Value;
                TempMat.Unused1Float = (float)MatUnknown1.Value;
                TempMat.Unused2Float = (float)MatUnknown2.Value;

                if (trickyModel.Board != null)
                {
                    trickyModel.Board.ModelList[MpfList.SelectedIndex].materialDatas[MaterialList.SelectedIndex] = TempMat;
                }
                else if (trickyModel.Head != null && trickyModel.Body != null)
                {
                    string LookingFor = CharacterParts.Items[CharacterParts.SelectedIndex].ToString();
                    int MeshID = -1;
                    for (int i = 0; i < trickyModel.Body.ModelList.Count; i++)
                    {
                        if (LookingFor == trickyModel.Body.ModelList[i].FileName)
                        {
                            MeshID = i;
                            trickyModel.Body.ModelList[i].materialDatas[MaterialList.SelectedIndex] = TempMat;
                            break;
                        }
                    }

                    if (MeshID == -1)
                    {
                        //Head
                        for (int i = 0; i < trickyModel.Head.ModelList.Count; i++)
                        {
                            if (LookingFor == trickyModel.Head.ModelList[i].FileName)
                            {
                                trickyModel.Head.ModelList[i].materialDatas[MaterialList.SelectedIndex] = TempMat;
                                break;
                            }
                        }
                    }
                }


                //Save Material

                MatDisableUpdate = false;
            }
        }

    }
}
