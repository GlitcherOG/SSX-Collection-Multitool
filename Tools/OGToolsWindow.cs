using Microsoft.WindowsAPICodePack.Dialogs;
using SSXLibrary;
using SSXLibrary.FileHandlers;
using SSXLibrary.FileHandlers.Models;
using SSXLibrary.FileHandlers.Models.SSXOG;
using SSXLibrary.FileHandlers.Models.Tricky;
using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSXMultiTool
{
    public partial class OGToolsWindow : Form
    {
        public OGToolsWindow()
        {
            InitializeComponent();
        }

        SSXOGModelCombiner ogPS2Model = new SSXOGModelCombiner();
        SSXMPFModelHandler modelHandler = new SSXMPFModelHandler();
        bool Highmodel = false;
        private void MpfLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Model File (*.mpf)|*.mpf|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (MpfHeaderChecker.DetectFileType(openFileDialog.FileName) != 0)
                {
                    MessageBox.Show(MpfHeaderChecker.TypeErrorMessage(MpfHeaderChecker.DetectFileType(openFileDialog.FileName)));
                    return;
                }

                modelHandler = new SSXMPFModelHandler();
                modelHandler.load(openFileDialog.FileName);

                MPFList.Items.Clear();

                int Type = ogPS2Model.DectectModelType(modelHandler);

                if ((Type == 0 && ogPS2Model.Head != null) || (Type == 1 && ogPS2Model.Body != null))
                {
                    MPFList.Items.Clear();
                    if (ogPS2Model.Head.ModelList[2].FileName.Contains("5000"))
                    {
                        MPFList.Items.Add("Character 5000");
                        MPFList.Items.Add("Character Shadow 750");
                        Highmodel = true;
                    }
                    else
                    {
                        Highmodel = false;
                        MPFList.Items.Add("Character 2500");
                        MPFList.Items.Add("Character 1200");
                        MPFList.Items.Add("Character 750");
                        MPFList.Items.Add("Character Shadow 750");
                    }
                }
                else
                {
                    if (ogPS2Model.Body == null)
                    {
                        MPFList.Items.Clear();
                        MPFList.Items.Add("Please Load Matching Body File");
                    }
                    else if (ogPS2Model.Head == null)
                    {
                        MPFList.Items.Clear();
                        MPFList.Items.Add("Please Load Matching Head File");
                    }
                    UpdateDataPS2();
                }

                if (Type == 2)
                {
                    MPFList.Items.Clear();
                    for (int i = 0; i < modelHandler.ModelList.Count; i++)
                    {
                        MPFList.Items.Add(modelHandler.ModelList[i].FileName);
                    }
                    UpdateDataPS2();
                }
            }
        }

        void UpdateDataPS2(SSXMPFModelHandler.MPFModelHeader? modelHeader = null)
        {
            if (modelHeader != null)
            {
                //FileIDPS2.Text = modelHeader.Value.FileID.ToString();
                //BoneCountPS2.Text = modelHeader.Value.boneDatas.Count.ToString();
                //MaterialCountPS2.Text = modelHeader.Value.materialDatas.Count.ToString();
                //IkCountPS2.Text = modelHeader.Value.iKPoints.Count.ToString();
                //ShapeKeyCountPS2.Text = modelHeader.Value.MorphKeyCount.ToString();
                //WeightsPS2.Text = modelHeader.Value.boneWeightHeader.Count.ToString();

                //TristripCountLabelPS2.Text = modelHandler.TristripCount(modelHeader.Value).ToString();
                //VerticeCountPS2.Text = modelHandler.VerticeCount(modelHeader.Value).ToString();
                //MeshChunksPS2.Text = modelHandler.ChunkCount(modelHeader.Value).ToString();
                //MaterialGroupCountPS2.Text = modelHeader.Value.MeshGroups.Count.ToString();
                //WeightGroupCountPS2.Text = modelHandler.WeigthRefCount(modelHeader.Value).ToString();
                //MorphGroupCountPS2.Text = modelHandler.MorphGroupCount(modelHeader.Value).ToString();

                MaterialListMPF.Items.Clear();
                MpfUpdateMaterialPS2();
                for (int i = 0; i < modelHeader.Value.materialDataList.Count; i++)
                {
                    MaterialListMPF.Items.Add(modelHeader.Value.materialDataList[i].Name);
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
                MaterialListMPF.Items.Clear();
                MpfUpdateMaterialPS2();
            }
        }
        bool MatDisableUpdate;
        void MpfUpdateMaterialPS2(SSXMPFModelHandler.MPFModelHeader? modelHeader = null)
        {
            if (MaterialListMPF.SelectedIndex != -1 && modelHeader != null)
            {
                MatDisableUpdate = true;
                MatMainTextureMPF.Text = modelHeader.Value.materialDataList[MaterialListMPF.SelectedIndex].Name;
                MatFlagFactorMPF.Value = (decimal)modelHeader.Value.materialDataList[MaterialListMPF.SelectedIndex].X;
                MatUnknown1MPF.Value = (decimal)modelHeader.Value.materialDataList[MaterialListMPF.SelectedIndex].Y;
                MatUnknown2MPF.Value = (decimal)modelHeader.Value.materialDataList[MaterialListMPF.SelectedIndex].Z;
                MatDisableUpdate = false;
            }
            else
            {
                MatDisableUpdate = true;
                MatMainTextureMPF.Text = "";
                MatFlagFactorMPF.Value = 0;
                MatUnknown1MPF.Value = 0;
                MatUnknown2MPF.Value = 0;
                MatDisableUpdate = false;
            }
        }

        private void MpfExtract_Click(object sender, EventArgs e)
        {
            if (MPFList.SelectedIndex != -1)
            {
                SaveFileDialog openFileDialog = new SaveFileDialog
                {
                    Filter = "gltf File (*.glb)|*.glb|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ogPS2Model.StartReassignMesh(MPFList.SelectedIndex, Highmodel);

                    glftHandler.SaveOGCombinedglTF(openFileDialog.FileName, ogPS2Model);

                    MessageBox.Show("File Extracted");
                }
            }
        }

        private void MPFUpdateMat(object sender, EventArgs e)
        {
            if (MaterialListMPF.SelectedIndex != -1 && !MatDisableUpdate)
            {
                MatDisableUpdate = true;

                SSXMPFModelHandler.MaterialData TempMat = new SSXMPFModelHandler.MaterialData();
                //Load Material
                if (ogPS2Model.Board != null)
                {
                    TempMat = ogPS2Model.Board.ModelList[MPFList.SelectedIndex].materialDataList[MaterialListMPF.SelectedIndex];
                }
                else if (ogPS2Model.Head != null && ogPS2Model.Body != null)
                {
                    string LookingFor = CharacterPartsMpf.Items[CharacterPartsMpf.SelectedIndex].ToString();
                    int MeshID = -1;
                    for (int i = 0; i < ogPS2Model.Body.ModelList.Count; i++)
                    {
                        if (LookingFor == ogPS2Model.Body.ModelList[i].FileName)
                        {
                            MeshID = i;
                            TempMat = ogPS2Model.Body.ModelList[i].materialDataList[MaterialListMPF.SelectedIndex];
                            break;
                        }
                    }

                    if (MeshID == -1)
                    {
                        //Head
                        for (int i = 0; i < ogPS2Model.Head.ModelList.Count; i++)
                        {
                            if (LookingFor == ogPS2Model.Head.ModelList[i].FileName)
                            {
                                MeshID = i;
                                TempMat = ogPS2Model.Head.ModelList[i].materialDataList[MaterialListMPF.SelectedIndex];
                                break;
                            }
                        }
                    }
                }

                TempMat.Name = MatMainTextureMPF.Text;
                TempMat.X = (float)MatFlagFactorMPF.Value;
                TempMat.Y = (float)MatUnknown1MPF.Value;
                TempMat.Z = (float)MatUnknown2MPF.Value;

                if (ogPS2Model.Board != null)
                {
                    ogPS2Model.Board.ModelList[MPFList.SelectedIndex].materialDataList[MaterialListMPF.SelectedIndex] = TempMat;
                }
                else if (ogPS2Model.Head != null && ogPS2Model.Body != null)
                {
                    string LookingFor = CharacterPartsMpf.Items[CharacterPartsMpf.SelectedIndex].ToString();
                    int MeshID = -1;
                    for (int i = 0; i < ogPS2Model.Body.ModelList.Count; i++)
                    {
                        if (LookingFor == ogPS2Model.Body.ModelList[i].FileName)
                        {
                            MeshID = i;
                            ogPS2Model.Body.ModelList[i].materialDataList[MaterialListMPF.SelectedIndex] = TempMat;
                            break;
                        }
                    }

                    if (MeshID == -1)
                    {
                        //Head
                        for (int i = 0; i < ogPS2Model.Head.ModelList.Count; i++)
                        {
                            if (LookingFor == ogPS2Model.Head.ModelList[i].FileName)
                            {
                                ogPS2Model.Head.ModelList[i].materialDataList[MaterialListMPF.SelectedIndex] = TempMat;
                                break;
                            }
                        }
                    }
                }

                //Save Material

                MatDisableUpdate = false;
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
                if (File.Exists(commonDialog.FileName + "\\DATA\\MODELS\\ALOHA.BIG"))
                {
                    BIG.Extract(commonDialog.FileName + "\\DATA\\MODELS\\ALOHA.BIG", commonDialog.FileName);
                    BIG.Extract(commonDialog.FileName + "\\DATA\\MODELS\\ELYSIUM.BIG", commonDialog.FileName);
                    BIG.Extract(commonDialog.FileName + "\\DATA\\MODELS\\MEGAPLEX.BIG", commonDialog.FileName);
                    BIG.Extract(commonDialog.FileName + "\\DATA\\MODELS\\MERQUERY.BIG", commonDialog.FileName);
                    BIG.Extract(commonDialog.FileName + "\\DATA\\MODELS\\MESA.BIG", commonDialog.FileName);
                    BIG.Extract(commonDialog.FileName + "\\DATA\\MODELS\\PIPE.BIG", commonDialog.FileName);
                    BIG.Extract(commonDialog.FileName + "\\DATA\\MODELS\\SNOW.BIG", commonDialog.FileName);
                    BIG.Extract(commonDialog.FileName + "\\DATA\\MODELS\\UNTRACK.BIG", commonDialog.FileName);
                    BIG.Extract(commonDialog.FileName + "\\DATA\\MODELS\\WARMUP.BIG", commonDialog.FileName);

                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\ALOHA.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\ELYSIUM.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\MEGAPLEX.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\MERQUERY.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\MESA.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\PIPE.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\SNOW.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\UNTRACK.BIG");
                    File.Delete(commonDialog.FileName + "\\DATA\\MODELS\\WARMUP.BIG");

                    MessageBox.Show("Finished Setting up Path.");
                }
                else
                {
                    MessageBox.Show("Unknown Game Path or Game Already Extracted");
                }
            }
        }

        private void LoadADF_Click(object sender, EventArgs e)
        {
            //CommonOpenFileDialog openFileDialog1 = new CommonOpenFileDialog
            //{
            //    IsFolderPicker = true,
            //    Title = "Select Alf Folder",
            //};
            //if (openFileDialog1.ShowDialog() == CommonFileDialogResult.Ok)
            //{
            //    string[] NewFiles = Directory.GetFiles(openFileDialog1.FileName, "*.adf", SearchOption.AllDirectories);

            //    for (int i = 0; i < NewFiles.Length; i++)
            //    {
            //        adfHandler aflHandler = new adfHandler();

            //        aflHandler.Load(NewFiles[i]);
            //    }
            //}

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Model File (*.adf)|*.adf|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                adfHandler aflHandler = new adfHandler();

                aflHandler.Load(openFileDialog.FileName);
            }
        }

        private void MPFImport_Click(object sender, EventArgs e)
        {
            if (MPFList.SelectedIndex != -1)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "gltf File (*.glb)|*.glb|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if ((ogPS2Model.Board != null) || (ogPS2Model.Head != null && ogPS2Model.Body != null))
                    {
                        SSXOGModelCombiner TempCombiner = null;

                        try
                        {
                            TempCombiner = glftHandler.LoadOGGlft(openFileDialog.FileName);
                        }
                        catch
                        {
                            MessageBox.Show(ErrorUtil.Error);
                        }
                        if (TempCombiner != null)
                        {
                            try
                            {
                                //ogPS2Model.NormalAverage = ImportAverageNormalMPF.Checked;
                                //ogPS2Model.BoneUpdate = BoneUpdateCheckMPF.Checked;
                                ogPS2Model.StartRegenMesh(TempCombiner, MPFList.SelectedIndex, Highmodel);

                                UpdateDataPS2();
                            }
                            catch
                            {
                                MessageBox.Show(ErrorUtil.Error);
                            }
                        }
                    }
                }
            }
        }

        private void MPFSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog
            {
                Filter = "Model File (*.mpf)|*.mpf|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (ogPS2Model.Board != null)
                {
                    modelHandler = ogPS2Model.Board;

                    modelHandler.Save(openFileDialog.FileName);
                }
                else
                {
                    string Path = openFileDialog.FileName.Remove(openFileDialog.FileName.Length - 8, 8);

                    if (ogPS2Model.Body != null)
                    {
                        modelHandler = ogPS2Model.Body;

                        modelHandler.Save(Path + "body.mpf");
                    }

                    if (ogPS2Model.Head != null)
                    {
                        modelHandler = ogPS2Model.Head;

                        modelHandler.Save(Path + "head.mpf");
                    }
                }
            }
        }

        private void MaterialListMPF_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MaterialListMPF.SelectedIndex != -1)
            {
                if (ogPS2Model.Board != null)
                {
                    MpfUpdateMaterialPS2(ogPS2Model.Board.ModelList[MPFList.SelectedIndex]);
                }
                else if (ogPS2Model.Body != null && ogPS2Model.Head != null)
                {
                    string LookingFor = CharacterPartsMpf.Items[CharacterPartsMpf.SelectedIndex].ToString();
                    int MeshID = -1;
                    for (int i = 0; i < ogPS2Model.Body.ModelList.Count; i++)
                    {
                        if (LookingFor == ogPS2Model.Body.ModelList[i].FileName)
                        {
                            MeshID = i;
                            MpfUpdateMaterialPS2(ogPS2Model.Body.ModelList[i]);
                            break;
                        }
                    }

                    if (MeshID == -1)
                    {
                        //Head
                        for (int i = 0; i < ogPS2Model.Head.ModelList.Count; i++)
                        {
                            if (LookingFor == ogPS2Model.Head.ModelList[i].FileName)
                            {
                                MeshID = i;
                                MpfUpdateMaterialPS2(ogPS2Model.Head.ModelList[i]);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    MpfUpdateMaterialPS2();
                }
            }
        }

        private void MPFList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MPFList.SelectedIndex != -1)
            {
                if (ogPS2Model.Board != null)
                {
                    UpdateDataPS2(ogPS2Model.Board.ModelList[MPFList.SelectedIndex]);
                }
                else
                {
                    UpdateDataPS2();
                }
                RegeneratePartListPS2();
            }
        }

        void RegeneratePartListPS2()
        {
            CharacterPartsMpf.Items.Clear();
            int MeshID = MPFList.SelectedIndex;

            if (ogPS2Model.Body != null && ogPS2Model.Head != null)
            {
                for (int i = 0; i < ogPS2Model.Body.ModelList.Count; i++)
                {
                    if ((MeshID == 0 && ogPS2Model.Body.ModelList[i].FileName.Contains("5000")) ||
                        (MeshID == 0 && ogPS2Model.Body.ModelList[i].FileName.Contains("2500")) ||
                        (MeshID == 1 && ogPS2Model.Body.ModelList[i].FileName.Contains("1200")) ||
                        (MeshID == 2 && ogPS2Model.Body.ModelList[i].FileName.Contains("750") && !ogPS2Model.Body.ModelList[i].FileName.ToLower().Contains("shdw")) ||
                        (!Highmodel && MeshID == 3 && ogPS2Model.Body.ModelList[i].FileName.ToLower().Contains("shdw")) ||
                        (Highmodel && MeshID == 1 && ogPS2Model.Body.ModelList[i].FileName.ToLower().Contains("shdw")))
                    {
                        CharacterPartsMpf.Items.Add(ogPS2Model.Body.ModelList[i].FileName);
                    }
                }

                //Head
                for (int i = 0; i < ogPS2Model.Head.ModelList.Count; i++)
                {
                    if ((MeshID == 0 && ogPS2Model.Body.ModelList[i].FileName.Contains("5000")) ||
                        (MeshID == 0 && ogPS2Model.Body.ModelList[i].FileName.Contains("2500")) ||
                        (MeshID == 1 && ogPS2Model.Body.ModelList[i].FileName.Contains("1200")) ||
                        (MeshID == 2 && ogPS2Model.Body.ModelList[i].FileName.Contains("750") && !ogPS2Model.Body.ModelList[i].FileName.ToLower().Contains("shdw")) ||
                        (!Highmodel && MeshID == 3 && ogPS2Model.Body.ModelList[i].FileName.ToLower().Contains("shdw")) ||
                        (Highmodel && MeshID == 1 && ogPS2Model.Body.ModelList[i].FileName.ToLower().Contains("shdw")))
                    {

                        CharacterPartsMpf.Items.Add(ogPS2Model.Head.ModelList[i].FileName);
                    }
                }
            }
        }

        private void CharacterParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CharacterPartsMpf.SelectedIndex != -1)
            {
                string LookingFor = CharacterPartsMpf.Items[CharacterPartsMpf.SelectedIndex].ToString();
                int MeshID = -1;

                for (int i = 0; i < ogPS2Model.Body.ModelList.Count; i++)
                {
                    if (LookingFor == ogPS2Model.Body.ModelList[i].FileName)
                    {
                        MeshID = i;
                        UpdateDataPS2(ogPS2Model.Body.ModelList[i]);
                        break;
                    }
                }

                if (MeshID == -1)
                {
                    //Head
                    for (int i = 0; i < ogPS2Model.Head.ModelList.Count; i++)
                    {
                        if (LookingFor == ogPS2Model.Head.ModelList[i].FileName)
                        {
                            MeshID = i;
                            UpdateDataPS2(ogPS2Model.Head.ModelList[i]);
                        }
                    }
                }

            }
        }
    }
}
