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
                    Filter = "gltf File (*.gltf)|*.gltf|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    modelHandler.SaveModel(openFileDialog.FileName, MPFList.SelectedIndex);
                }
            }
        }
    }
}
