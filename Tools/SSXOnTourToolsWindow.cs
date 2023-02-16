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

namespace SSXMultiTool.Tools
{
    public partial class SSXOnTourToolsWindow : Form
    {
        public SSXOnTourToolsWindow()
        {
            InitializeComponent();
        }
        SSXOnTourMPF onTourMPF = new SSXOnTourMPF();
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
                onTourMPF = new SSXOnTourMPF();


                onTourMPF.Load(openFileDialog.FileName);
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
                onTourMPF.SaveDecompress(openFileDialog.FileName);
            }
        }
    }
}
