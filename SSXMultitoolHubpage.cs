using Microsoft.WindowsAPICodePack.Dialogs;
using SSXMultiTool.Tools;
using SSXMultiTool.Utilities;
using SSXMultiTool.FileHandlers;
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
    public partial class SSXMultitoolHubpage : Form
    {
        public SSXMultitoolHubpage()
        {
            InitializeComponent();
        }

        private void BigArchiveButton_Click(object sender, EventArgs e)
        {
            new BigArchiveTool().ShowDialog();

            //Thread NewThread = new Thread(() => new BigArchiveTool().ShowDialog());
            //NewThread.SetApartmentState(ApartmentState.STA);
            //NewThread.Start();
        }

        private void SSHImageButton_Click(object sender, EventArgs e)
        {
            new SSHImageTools().ShowDialog();
            //Thread NewThread = new Thread(() => new SSHImageTools().ShowDialog());
            //NewThread.SetApartmentState(ApartmentState.STA);
            //NewThread.Start();
        }

        private void TrickyLevelButton_Click(object sender, EventArgs e)
        {
            new TrickyProjectWindow().ShowDialog();
            //Thread NewThread = new Thread(() => new TrickyProjectWindow().ShowDialog());
            //NewThread.SetApartmentState(ApartmentState.STA);
            //NewThread.Start();
        }

        private void LocFileButton_Click(object sender, EventArgs e)
        {
            new LOCEditorTools().ShowDialog();
            //Thread NewThread = new Thread(() => new LOCEditorTools().ShowDialog());
            //NewThread.SetApartmentState(ApartmentState.STA);
            //NewThread.Start();
        }

        private void SSX3ToolsButton_Click(object sender, EventArgs e)
        {
            new SSX3ToolsWindow().ShowDialog();
        }

        private void SSX3LevelButton_Click(object sender, EventArgs e)
        {
            new SSX3ProjectWindow().ShowDialog();
        }

        private void TrickyToolsButton_Click(object sender, EventArgs e)
        {
            new TrickyToolsWindow().ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new OGToolsWindow().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new SSXOnTourToolsWindow().ShowDialog();
        }

        private void AudioWindowButton_Click(object sender, EventArgs e)
        {
            new AudioTools().ShowDialog();
        }
    }
}
