using SSXMultiTool.FileHandlers.SSX2012;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSXMultiTool.Tools
{
    public partial class SSX2012ToolsWindow : Form
    {
        public SSX2012ToolsWindow()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Vault|*.vlt|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            //openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                VaultHandler vaultHandler = new VaultHandler();

                vaultHandler.Load(openFileDialog.FileName);
            }
        }
    }
}
