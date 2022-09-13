using Microsoft.WindowsAPICodePack.Dialogs;
using SSXMultiTool.FileHandlers.LevelFiles;
using System.IO;
using SSXMultiTool.FileHandlers;
using System.Windows.Forms;
using SSXMultiTool.JsonFiles;

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
                Filter = "Map File (*.map)|*.map|Big File (*.big)|*.big|All files (*.*)|*.*",
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
                        if (openFileDialog.FileName.Contains(".big"))
                        {

                        }
                        else
                        {
                            ExtractFiles(openFileDialog.FileName, commonDialog.FileName);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Folder Must Be Empty.");
                    }
                }
            }
        }

        void ExtractFiles(string StartPath, string ExportPath)
        {
            string LoadPath = StartPath.Substring(0, StartPath.Length - 4);
            TrickyLevelInterface trickyLevelInterface = new TrickyLevelInterface();
            trickyLevelInterface.ExtractTrickyLevelFiles(LoadPath, ExportPath);
            MessageBox.Show("Level Extracted");
        }

        void ExtractBig(string BigPath, string ExportPath)
        {

        }
    }
}