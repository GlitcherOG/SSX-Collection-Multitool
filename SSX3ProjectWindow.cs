using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2;

namespace SSXMultiTool
{
    public partial class SSX3ProjectWindow : Form
    {
        public SSX3ProjectWindow()
        {
            InitializeComponent();
        }

        SSBHandler ssbHandler = new SSBHandler();
        private void LevelExtract_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "SSB File (*.ssb)|*.ssb|All files (*.*)|*.*",
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
                        ssbHandler.LoadAndExtractSSBFromSBD(openFileDialog.FileName, commonDialog.FileName);
                    }
                    else
                    {
                        MessageBox.Show("Folder Must Be Empty.");
                    }
                }
            }
        }

        private void PackSSB_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog commonDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };
            if (commonDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                SaveFileDialog openFileDialog = new SaveFileDialog
                {
                    Filter = "SSB File (*.ssb)|*.ssb|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //ssbHandler.PackSSB(commonDialog.FileName, openFileDialog.FileName);
                }
            }

        }

        SDBHandler sdbHandler = new SDBHandler();
        private void LoadSDB_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "SDB File (*.SDB)|*.SDB|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                sdbHandler = new SDBHandler();
                sdbHandler.LoadSBD(openFileDialog.FileName);
            }
        }
        PSMHandler psmHandler = new PSMHandler();
        private void PSMLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PSM File (*.PSM)|*.PSM|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                psmHandler = new PSMHandler();
                psmHandler.LoadPSM(openFileDialog.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "SDB File (*.SDB)|*.SDB|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                sdbHandler = new SDBHandler();
                sdbHandler.Save(openFileDialog.FileName);
            }
        }
    }
}
