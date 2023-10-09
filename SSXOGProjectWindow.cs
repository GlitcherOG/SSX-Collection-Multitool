using Microsoft.WindowsAPICodePack.Dialogs;
using SSXMultiTool.FileHandlers.LevelFiles.OGPS2;
using SSXMultiTool.JsonFiles;
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
    public partial class SSXOGProjectWindow : Form
    {
        public SSXOGProjectWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Map File (*.map)|*.map|All files (*.*)|*.*",
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
                        ExtractFiles(openFileDialog.FileName, commonDialog.FileName);
                    }
                }
            }
        }

        public void ExtractFiles(string Laod, string Extract)
        {
            string LoadPath = Laod.Substring(0, Laod.Length - 4);
            SSXOGLevelInterface OGLevelInterface = new SSXOGLevelInterface();
            OGLevelInterface.ExtractOGLevelFiles(LoadPath, Extract);
        }
    }
}
