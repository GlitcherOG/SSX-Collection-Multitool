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

namespace SSXMultiTool
{
    public partial class TrickyToolsWindow : Form
    {
        public TrickyToolsWindow()
        {
            InitializeComponent();
        }
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
                MpfList.Items.Clear();
                for (int i = 0; i < trickyMPF.ModelList.Count; i++)
                {
                    MpfList.Items.Add(trickyMPF.ModelList[i].FileName);
                }
            }

        }

        private void MPFExtract_Click(object sender, EventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog
            {
                Filter = "gltf File (*.gltf)|*.gltf|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                trickyMPF.SaveModel(openFileDialog.FileName);
            }
        }
    }
}
