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
    public partial class SSXMultitoolHubpage : Form
    {
        public SSXMultitoolHubpage()
        {
            InitializeComponent();
        }

        private void BigArchiveButton_Click(object sender, EventArgs e)
        {
            new BigArchiveTool().ShowDialog();
            //new Thread(() => new BigArchiveTool().ShowDialog()).Start();
        }

        private void SSHImageButton_Click(object sender, EventArgs e)
        {
            new SSHImageTools().ShowDialog();
            //new Thread(() => new SSHImageTools().ShowDialog()).Start();
        }
    }
}
