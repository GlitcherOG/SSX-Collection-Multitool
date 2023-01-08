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
                }

                if (Type == 2)
                {
                    MpfList.Items.Clear();
                    for (int i = 0; i < trickyMPF.ModelList.Count; i++)
                    {
                        MpfList.Items.Add(trickyMPF.ModelList[i].FileName);
                    }
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

                    glstHandler.SaveTrickyglTF(openFileDialog.FileName, trickyModel);
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
                trickyMPF = trickyModel.Board;

                trickyMPF.Save(openFileDialog.FileName);
            }
        }

        private void MPFImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "gltf File (*.glb)|*.glb|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (trickyModel.Board != null)
                {
                    var TempCombiner = glstHandler.LoadGlft(openFileDialog.FileName);

                    trickyModel.StartRegenMesh(TempCombiner, MpfList.SelectedIndex);

                }
            }
        }
    }
}
