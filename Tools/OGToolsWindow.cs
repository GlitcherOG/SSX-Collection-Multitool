using Microsoft.WindowsAPICodePack.Dialogs;
using SSXMultiTool.FileHandlers;
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
using SSXMultiTool.FileHandlers.Models;
using SSXMultiTool.FileHandlers.Models.SSXOG;

namespace SSXMultiTool
{
    public partial class OGToolsWindow : Form
    {
        public OGToolsWindow()
        {
            InitializeComponent();
        }
        SSXMPFModelHandler modelHandler = new SSXMPFModelHandler();
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
                for (int i = 0; i < modelHandler.ModelList.Count; i++)
                {
                    MPFList.Items.Add(modelHandler.ModelList[i].FileName);
                }
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
                    modelHandler.SaveModel(openFileDialog.FileName, MPFList.SelectedIndex);
                    MessageBox.Show("File Extracted");
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
                if (File.Exists(commonDialog.FileName + "\\DATA\\MODELS\\ALOHA.BIG"))
                {
                    BigHandler bigHandler = new BigHandler();
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\ALOHA.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\ELYSIUM.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\MEGAPLEX.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\MERQUERY.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\MESA.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\PIPE.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\SNOW.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\UNTRACK.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
                    bigHandler.LoadBig(commonDialog.FileName + "\\DATA\\MODELS\\WARMUP.BIG");
                    bigHandler.ExtractBig(commonDialog.FileName);
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
    }
}
