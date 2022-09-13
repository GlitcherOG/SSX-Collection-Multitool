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
            new Thread(() => new BigArchiveTool().ShowDialog()).Start();
        }
    }
}
