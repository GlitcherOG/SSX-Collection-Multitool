﻿using Microsoft.WindowsAPICodePack.Dialogs;
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
using SSXMultiTool.FileHandlers.Models.Tricky;
using NAudio.Wave;

namespace SSXMultiTool
{
    public partial class TrickyToolsWindow : Form
    {
        public TrickyToolsWindow()
        {
            InitializeComponent();

            //Fix Min
            hdrU1.Minimum = -100000000;
            hdrU2.Minimum = -100000000;
            hdrEntryU1.Minimum = -100000000;
            hdrEntryU2.Minimum = -100000000;
            hdrEntryU3.Minimum = -100000000;
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

        #region Tricky PS2 Models
        TrickyPS2ModelCombiner trickyPS2Model = new TrickyPS2ModelCombiner();
        TrickyPS2MPF trickyMPF = new TrickyPS2MPF();

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
                if (MpfHeaderChecker.DetectFileType(openFileDialog.FileName) != 1)
                {
                    MessageBox.Show(MpfHeaderChecker.TypeErrorMessage(MpfHeaderChecker.DetectFileType(openFileDialog.FileName)));
                    return;
                }


                trickyMPF = new TrickyPS2MPF();
                trickyMPF.load(openFileDialog.FileName);

                int Type = trickyPS2Model.DectectModelType(trickyMPF);

                if ((Type == 0 && trickyPS2Model.Head != null) || (Type == 1 && trickyPS2Model.Body != null))
                {
                    MpfList.Items.Clear();
                    MpfList.Items.Add("Character 3000");
                    MpfList.Items.Add("Character 1500");
                    MpfList.Items.Add("Character 750");
                    MpfList.Items.Add("Character Shadow 750");
                }
                else
                {
                    if (trickyPS2Model.Body == null)
                    {
                        MpfList.Items.Clear();
                        MpfList.Items.Add("Please Load Matching Body File");
                    }
                    else if (trickyPS2Model.Head == null)
                    {
                        MpfList.Items.Clear();
                        MpfList.Items.Add("Please Load Matching Head File");
                    }
                    UpdateDataPS2();
                }

                if (Type == 2)
                {
                    MpfList.Items.Clear();
                    for (int i = 0; i < trickyMPF.ModelList.Count; i++)
                    {
                        MpfList.Items.Add(trickyMPF.ModelList[i].FileName);
                    }
                    UpdateDataPS2();
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
                    trickyPS2Model.StartReassignMesh(MpfList.SelectedIndex);

                    glftHandler.SaveTrickyPS2glTF(openFileDialog.FileName, trickyPS2Model);
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
                if (trickyPS2Model.Board != null)
                {
                    trickyMPF = trickyPS2Model.Board;

                    trickyMPF.Save(openFileDialog.FileName);
                }
                else
                {
                    string Path = openFileDialog.FileName.Remove(openFileDialog.FileName.Length - 8, 8);

                    trickyMPF = trickyPS2Model.Body;

                    trickyMPF.Save(Path + "body.mpf");

                    trickyMPF = trickyPS2Model.Head;

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
                    if ((trickyPS2Model.Board != null) || (trickyPS2Model.Head != null && trickyPS2Model.Body != null))
                    {
                        TrickyPS2ModelCombiner TempCombiner = null;

                        try
                        {
                            TempCombiner = glftHandler.LoadTrickyGlft(openFileDialog.FileName);
                        }
                        catch
                        {
                            MessageBox.Show("Failed to Load File");
                        }
                        if (TempCombiner != null)
                        {
                            try
                            {
                                trickyPS2Model.NormalAverage = ImportAverageNormal.Checked;
                                trickyPS2Model.BoneUpdate = BoneUpdateCheck.Checked;
                                trickyPS2Model.StartRegenMesh(TempCombiner, MpfList.SelectedIndex);

                                UpdateDataPS2();
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

        void UpdateDataPS2(TrickyPS2MPF.MPFModelHeader? modelHeader = null)
        {
            if (modelHeader != null)
            {
                FileIDPS2.Text = modelHeader.Value.FileID.ToString();
                BoneCountPS2.Text = modelHeader.Value.boneDatas.Count.ToString();
                MaterialCountPS2.Text = modelHeader.Value.materialDatas.Count.ToString();
                IkCountPS2.Text = modelHeader.Value.iKPoints.Count.ToString();
                ShapeKeyCountPS2.Text = modelHeader.Value.MorphKeyCount.ToString();
                WeightsPS2.Text = modelHeader.Value.boneWeightHeader.Count.ToString();

                TristripCountLabelPS2.Text = trickyPS2Model.TristripCount(modelHeader.Value).ToString();
                VerticeCountPS2.Text = trickyPS2Model.VerticeCount(modelHeader.Value).ToString();
                MeshChunksPS2.Text = trickyPS2Model.ChunkCount(modelHeader.Value).ToString();
                MaterialGroupCountPS2.Text = modelHeader.Value.MeshGroups.Count.ToString();
                WeightGroupCountPS2.Text = trickyPS2Model.WeigthRefCount(modelHeader.Value).ToString();
                MorphGroupCountPS2.Text = trickyPS2Model.MorphGroupCount(modelHeader.Value).ToString();

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
                FileIDPS2.Text = "0";
                BoneCountPS2.Text = "0";
                MaterialCountPS2.Text = "0";
                IkCountPS2.Text = "0";
                ShapeKeyCountPS2.Text = "0";
                WeightsPS2.Text = "0";

                //Model Data
                TristripCountLabelPS2.Text = "0";
                VerticeCountPS2.Text = "0";
                MeshChunksPS2.Text = "0";
                MaterialGroupCountPS2.Text = "0";
                WeightGroupCountPS2.Text = "0";
                MorphGroupCountPS2.Text = "0";

                //Material Items
                MaterialList.Items.Clear();
                MpfUpdateMaterial();
            }
        }
        bool MatDisableUpdate;
        void MpfUpdateMaterial(TrickyPS2MPF.MPFModelHeader? modelHeader = null)
        {
            if (MaterialList.SelectedIndex != -1 && modelHeader != null)
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
                if (trickyPS2Model.Board != null)
                {
                    UpdateDataPS2(trickyPS2Model.Board.ModelList[MpfList.SelectedIndex]);
                }
                else
                {
                    UpdateDataPS2();
                }
                RegeneratePartList();
            }
        }

        void RegeneratePartList()
        {
            CharacterParts.Items.Clear();
            int MeshID = MpfList.SelectedIndex;
            if (trickyPS2Model.Body != null && trickyPS2Model.Head != null)
            {
                for (int i = 0; i < trickyPS2Model.Body.ModelList.Count; i++)
                {
                    if ((MeshID == 0 && trickyPS2Model.Body.ModelList[i].FileName.Contains("3000")) ||
                        (MeshID == 1 && trickyPS2Model.Body.ModelList[i].FileName.Contains("1500")) ||
                        (MeshID == 2 && trickyPS2Model.Body.ModelList[i].FileName.Contains("750") && !trickyPS2Model.Body.ModelList[i].FileName.ToLower().Contains("shdw")) ||
                        (MeshID == 3 && trickyPS2Model.Body.ModelList[i].FileName.ToLower().Contains("shdw")))
                    {
                        CharacterParts.Items.Add(trickyPS2Model.Body.ModelList[i].FileName);
                    }
                }

                //Head
                for (int i = 0; i < trickyPS2Model.Head.ModelList.Count; i++)
                {
                    if ((MeshID == 0 && trickyPS2Model.Head.ModelList[i].FileName.Contains("3000")) ||
                        (MeshID == 1 && trickyPS2Model.Head.ModelList[i].FileName.Contains("1500")) ||
                        (MeshID == 2 && trickyPS2Model.Head.ModelList[i].FileName.Contains("750") && !trickyPS2Model.Head.ModelList[i].FileName.ToLower().Contains("shdw")) ||
                        (MeshID == 3 && trickyPS2Model.Head.ModelList[i].FileName.ToLower().Contains("shdw")))
                    {

                        CharacterParts.Items.Add(trickyPS2Model.Head.ModelList[i].FileName);
                    }
                }
            }
        }

        private void CharacterParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CharacterParts.SelectedIndex != -1)
            {
                string LookingFor = CharacterParts.Items[CharacterParts.SelectedIndex].ToString();
                int MeshID = -1;

                for (int i = 0; i < trickyPS2Model.Body.ModelList.Count; i++)
                {
                    if (LookingFor == trickyPS2Model.Body.ModelList[i].FileName)
                    {
                        MeshID = i;
                        UpdateDataPS2(trickyPS2Model.Body.ModelList[i]);
                        break;
                    }
                }

                if (MeshID == -1)
                {
                    //Head
                    for (int i = 0; i < trickyPS2Model.Head.ModelList.Count; i++)
                    {
                        if (LookingFor == trickyPS2Model.Head.ModelList[i].FileName)
                        {
                            MeshID = i;
                            UpdateDataPS2(trickyPS2Model.Head.ModelList[i]);
                        }
                    }
                }

            }
        }

        private void MaterialList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MaterialList.SelectedIndex != -1)
            {
                if (trickyPS2Model.Board != null)
                {
                    MpfUpdateMaterial(trickyPS2Model.Board.ModelList[MpfList.SelectedIndex]);
                }
                else if (trickyPS2Model.Body != null && trickyPS2Model.Head != null)
                {
                    string LookingFor = CharacterParts.Items[CharacterParts.SelectedIndex].ToString();
                    int MeshID = -1;
                    for (int i = 0; i < trickyPS2Model.Body.ModelList.Count; i++)
                    {
                        if (LookingFor == trickyPS2Model.Body.ModelList[i].FileName)
                        {
                            MeshID = i;
                            MpfUpdateMaterial(trickyPS2Model.Body.ModelList[i]);
                            break;
                        }
                    }

                    if (MeshID == -1)
                    {
                        //Head
                        for (int i = 0; i < trickyPS2Model.Head.ModelList.Count; i++)
                        {
                            if (LookingFor == trickyPS2Model.Head.ModelList[i].FileName)
                            {
                                MeshID = i;
                                MpfUpdateMaterial(trickyPS2Model.Head.ModelList[i]);
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
            if (MaterialList.SelectedIndex != -1 && !MatDisableUpdate)
            {
                MatDisableUpdate = true;

                TrickyPS2MPF.MaterialData TempMat = new TrickyPS2MPF.MaterialData();
                //Load Material
                if (trickyPS2Model.Board != null)
                {
                    TempMat = trickyPS2Model.Board.ModelList[MpfList.SelectedIndex].materialDatas[MaterialList.SelectedIndex];
                }
                else if (trickyPS2Model.Head != null && trickyPS2Model.Body != null)
                {
                    string LookingFor = CharacterParts.Items[CharacterParts.SelectedIndex].ToString();
                    int MeshID = -1;
                    for (int i = 0; i < trickyPS2Model.Body.ModelList.Count; i++)
                    {
                        if (LookingFor == trickyPS2Model.Body.ModelList[i].FileName)
                        {
                            MeshID = i;
                            TempMat = trickyPS2Model.Body.ModelList[i].materialDatas[MaterialList.SelectedIndex];
                            break;
                        }
                    }

                    if (MeshID == -1)
                    {
                        //Head
                        for (int i = 0; i < trickyPS2Model.Head.ModelList.Count; i++)
                        {
                            if (LookingFor == trickyPS2Model.Head.ModelList[i].FileName)
                            {
                                MeshID = i;
                                TempMat = trickyPS2Model.Head.ModelList[i].materialDatas[MaterialList.SelectedIndex];
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

                if (trickyPS2Model.Board != null)
                {
                    trickyPS2Model.Board.ModelList[MpfList.SelectedIndex].materialDatas[MaterialList.SelectedIndex] = TempMat;
                }
                else if (trickyPS2Model.Head != null && trickyPS2Model.Body != null)
                {
                    string LookingFor = CharacterParts.Items[CharacterParts.SelectedIndex].ToString();
                    int MeshID = -1;
                    for (int i = 0; i < trickyPS2Model.Body.ModelList.Count; i++)
                    {
                        if (LookingFor == trickyPS2Model.Body.ModelList[i].FileName)
                        {
                            MeshID = i;
                            trickyPS2Model.Body.ModelList[i].materialDatas[MaterialList.SelectedIndex] = TempMat;
                            break;
                        }
                    }

                    if (MeshID == -1)
                    {
                        //Head
                        for (int i = 0; i < trickyPS2Model.Head.ModelList.Count; i++)
                        {
                            if (LookingFor == trickyPS2Model.Head.ModelList[i].FileName)
                            {
                                trickyPS2Model.Head.ModelList[i].materialDatas[MaterialList.SelectedIndex] = TempMat;
                                break;
                            }
                        }
                    }
                }


                //Save Material

                MatDisableUpdate = false;
            }
        }
        #endregion


        #region Tricky Xbox Models
        TrickyXboxMXF xboxMXF = new TrickyXboxMXF();
        TrickyXboxModelCombiner trickyXboxModel = new TrickyXboxModelCombiner();
        private void MXFLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "MXF Model File (*.mxf)|*.mxf|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                xboxMXF = new TrickyXboxMXF();

                xboxMXF.Load(openFileDialog.FileName);

                int Type = trickyXboxModel.DectectModelType(xboxMXF);

                if (Type == 2)
                {
                    MXFList.Items.Clear();
                    for (int i = 0; i < xboxMXF.modelHeaders.Count; i++)
                    {
                        if (xboxMXF.modelHeaders[i].ModelSize != 0)
                        {
                            MXFList.Items.Add(xboxMXF.modelHeaders[i].ModelName);
                        }
                    }
                    UpdateDataXbox();
                }
            }
        }

        void UpdateDataXbox(TrickyXboxMXF.ModelHeader? modelHeader = null)
        {
            if (modelHeader != null)
            {
                FileIDXbox.Text = modelHeader.Value.FileID.ToString();
                BoneCountXbox.Text = modelHeader.Value.boneDatas.Count.ToString();
                MaterialCountXbox.Text = modelHeader.Value.materialDatas.Count.ToString();
                IKPointsXbox.Text = modelHeader.Value.iKPoints.Count.ToString();
                ShapeKeysXbox.Text = modelHeader.Value.NumMorphs.ToString();
                WeightsXbox.Text = modelHeader.Value.boneWeightHeaders.Count.ToString();


                MaterialList.Items.Clear();
                //MpfUpdateMaterial();
                for (int i = 0; i < modelHeader.Value.materialDatas.Count; i++)
                {
                    MaterialList.Items.Add(modelHeader.Value.materialDatas[i].MainTexture);
                }
            }
            else
            {
                //Modle Header Info
                FileIDXbox.Text = "0";
                BoneCountXbox.Text = "0";
                MaterialCountXbox.Text = "0";
                IKPointsXbox.Text = "0";
                ShapeKeysXbox.Text = "0";
                WeightsXbox.Text = "0";

                ////Material Items
                MaterialList.Items.Clear();
                //MpfUpdateMaterial();
            }
        }

        private void MXFExport_Click(object sender, EventArgs e)
        {
            if (MXFList.SelectedIndex != -1)
            {
                SaveFileDialog openFileDialog = new SaveFileDialog
                {
                    Filter = "GLFT Model File (*.glb)|*.glb|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    glftHandler.SaveTrickyXboxglTF(openFileDialog.FileName, xboxMXF.modelHeaders[MXFList.SelectedIndex]);
                }
            }
        }

        private void MXFList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MXFList.SelectedIndex != -1)
            {
                if (trickyXboxModel.Board != null)
                {
                    UpdateDataXbox(trickyXboxModel.Board.modelHeaders[MXFList.SelectedIndex]);
                }
                else
                {
                    UpdateDataXbox();
                }
            }
        }

        private void MXFSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog
            {
                Filter = "Model File (*.mxf)|*.mxf|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (trickyXboxModel.Board != null)
                {
                    xboxMXF = trickyXboxModel.Board;

                    xboxMXF.Save(openFileDialog.FileName);
                }
                else if (trickyXboxModel.Body != null && trickyXboxModel.Head != null)
                {
                    string Path = openFileDialog.FileName.Remove(openFileDialog.FileName.Length - 8, 8);

                    xboxMXF = trickyXboxModel.Body;

                    xboxMXF.Save(Path + "body.mpf");

                    xboxMXF = trickyXboxModel.Head;

                    xboxMXF.Save(Path + "head.mpf");
                }
            }
        }
        #endregion

        private bool CheckSX()
        {
            string StringPath = Application.StartupPath;
            if (Directory.GetFiles(StringPath, "sx_2002.exe", SearchOption.TopDirectoryOnly).Length == 1)
            {
                return true;
            }
            MessageBox.Show("Missing sx_2002.exe");
            return false;
        }

        DATAudio datAudio = new DATAudio();
        HDRHandler hdrHandler = new HDRHandler();
        public List<string> Files = new List<string>();
        bool HDRLoaded = false;
        bool DisableUpdate = false;
        private void HdrLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PS2 Audio Header File (*.hdr)|*.hdr|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                hdrHandler.Load(openFileDialog.FileName);
                hdrBuildDAT.Enabled = true;
                HDRLoaded = true;

                hdrList2.Items.Clear();

                for (int i = 0; i < hdrHandler.fileHeaders.Count; i++)
                {
                    hdrList2.Items.Add("Entry " + i + " - Offset " + hdrHandler.fileHeaders[i].OffsetInt);
                }

                DisableUpdate = true;
                hdrU1.Value = hdrHandler.U1;
                hdrU2.Value = hdrHandler.U2;
                DisableUpdate = false;
            }
        }

        private void hdrExtract_Click(object sender, EventArgs e)
        {
            if (CheckSX())
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "PS2 Audio File (*.dat)|*.dat|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    CommonOpenFileDialog openFileDialog1 = new CommonOpenFileDialog
                    {
                        IsFolderPicker = true,
                        Title = "Select Extract Folder",
                    };
                    if (openFileDialog1.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        datAudio = new DATAudio();

                        datAudio.ExtractGuess(openFileDialog.FileName, openFileDialog1.FileName);

                        MessageBox.Show("Audio Extracted");
                    }
                }

            }
        }

        private void hdrBuildDAT_Click(object sender, EventArgs e)
        {
            if (CheckSX() && HDRLoaded)
            {
                SaveFileDialog openFileDialog1 = new SaveFileDialog
                {
                    Filter = "PS2 Audio File (*.dat)|*.dat|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (hdrHandler.fileHeaders.Count != Files.ToArray().Length)
                    {
                        MessageBox.Show("Incorrect Wav Count " + hdrHandler.fileHeaders.Count + "/" + Files.ToArray().Length);
                        return;
                    }
                    hdrHandler = datAudio.GenerateDATAndHDR(Files.ToArray(), openFileDialog1.FileName, hdrHandler);
                    MessageBox.Show("File Generated");

                    hdrList2.Items.Clear();
                    for (int i = 0; i < hdrHandler.fileHeaders.Count; i++)
                    {
                        hdrList2.Items.Add("Entry " + i + " - Offset " + hdrHandler.fileHeaders[i].OffsetInt);
                    }
                }
            }
        }

        private void hdrSave_Click(object sender, EventArgs e)
        {
            if (HDRLoaded)
            {
                SaveFileDialog openFileDialog1 = new SaveFileDialog
                {
                    Filter = "PS2 Audio File (*.hdr)|*.hdr|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    hdrHandler.Save(openFileDialog1.FileName);
                }
            }
        }

        private void hdrAddFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Wav Audio File (*.wav)|*.wav|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Files.Add(openFileDialog.FileName);

                hdrFileList.Items.Clear();
                for (int i = 0; i < Files.Count; i++)
                {
                    hdrFileList.Items.Add(Path.GetFileName(Files[i]));
                }
            }
        }

        private void hdrRemoveFile_Click(object sender, EventArgs e)
        {
            if (hdrFileList.SelectedIndex != -1)
            {
                Files.RemoveAt(hdrFileList.SelectedIndex);

                hdrFileList.Items.Clear();
                for (int i = 0; i < Files.Count; i++)
                {
                    hdrFileList.Items.Add(Path.GetFileName(Files[i]));
                }
            }
        }

        private void hdrUp_Click(object sender, EventArgs e)
        {
            if (hdrFileList.SelectedIndex != -1 && hdrFileList.SelectedIndex != 0)
            {
                int Pos = hdrFileList.SelectedIndex;
                var Tempstring = Files[Pos];
                Files.RemoveAt(Pos);
                Files.Insert(Pos - 1, Tempstring);
                hdrFileList.Items.Clear();

                for (int i = 0; i < Files.Count; i++)
                {
                    hdrFileList.Items.Add(Path.GetFileName(Files[i]));
                }

                hdrFileList.SelectedIndex = Pos - 1;
            }
        }

        private void hdrDown_Click(object sender, EventArgs e)
        {
            if (hdrFileList.SelectedIndex != -1 && hdrFileList.SelectedIndex != Files.Count - 1)
            {
                int Pos = hdrFileList.SelectedIndex;
                var Tempstring = Files[Pos];
                Files.RemoveAt(Pos);
                Files.Insert(Pos + 1, Tempstring);
                hdrFileList.Items.Clear();

                for (int i = 0; i < Files.Count; i++)
                {
                    hdrFileList.Items.Add(Path.GetFileName(Files[i]));
                }

                hdrFileList.SelectedIndex = Pos + 1;
            }
        }

        private void hdrLoadFolder_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog openFileDialog1 = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "Select Wav Folder",
            };
            if (openFileDialog1.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string[] NewFiles = Directory.GetFiles(openFileDialog1.FileName, "*.wav", SearchOption.TopDirectoryOnly);

                Files = NewFiles.ToList();

                hdrFileList.Items.Clear();
                for (int i = 0; i < Files.Count; i++)
                {
                    hdrFileList.Items.Add(Path.GetFileName(Files[i]));
                }
            }
        }
        WaveFileReader waveFile = null;
        WaveOut waveOut = null;
        private void hdrPlay_Click(object sender, EventArgs e)
        {
            if (hdrFileList.SelectedIndex != -1)
            {
                if (waveOut != null)
                {
                    waveOut.Stop();
                    waveFile.Close();
                }
                waveFile = new WaveFileReader(Files[hdrFileList.SelectedIndex]);
                waveOut = new WaveOut();
                waveOut.Init(waveFile);
                waveOut.Play();
            }
        }

        private void hdrFileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hdrFileList.SelectedIndex != -1)
            {
                using (var reader = new WaveFileReader(Files[hdrFileList.SelectedIndex]))
                {
                    HDRTime.Text = reader.TotalTime.ToString(@"hh\:mm\:ss\.ff");
                    HDRSample.Text = reader.WaveFormat.SampleRate.ToString();
                    HDRFileSize.Text = reader.Length.ToString();
                    HDRTotalSamples.Text = (reader.TotalTime.TotalSeconds * reader.WaveFormat.SampleRate).ToString();

                }
            }
        }

        private void hdrList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hdrList2.SelectedIndex != -1 && !DisableUpdate)
            {
                DisableUpdate = true;
                hdrEntryU1.Value = hdrHandler.fileHeaders[hdrList2.SelectedIndex].Unknown;
                hdrEntryU2.Value = hdrHandler.fileHeaders[hdrList2.SelectedIndex].Unknown2;
                hdrEntryU3.Value = hdrHandler.fileHeaders[hdrList2.SelectedIndex].EventID;
                DisableUpdate = false;
            }
        }

        private void hdrEntryU1_ValueChanged(object sender, EventArgs e)
        {
            if (hdrList2.SelectedIndex != -1 && !DisableUpdate)
            {
                DisableUpdate = true;
                var Temp = hdrHandler.fileHeaders[hdrList2.SelectedIndex];
                Temp.Unknown = (int)hdrEntryU1.Value;
                Temp.Unknown2 = (int)hdrEntryU2.Value;
                Temp.EventID = (int)hdrEntryU3.Value;
                hdrHandler.fileHeaders[hdrList2.SelectedIndex] = Temp;
                DisableUpdate = false;
            }
        }

        private void TrickyToolsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveFile.Close();
            }
        }

        private void hdrU1_ValueChanged(object sender, EventArgs e)
        {
            if (hdrList2.SelectedIndex != -1 && !DisableUpdate)
            {
                DisableUpdate = true;
                hdrHandler.U1 = (int)hdrU1.Value;
                hdrHandler.U2 = (int)hdrU2.Value;
                DisableUpdate = false;
            }
        }
    }
}
