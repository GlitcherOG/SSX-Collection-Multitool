using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SSXMultiTool.Tools
{
    public partial class AudioTools : Form
    {
        public AudioTools()
        {
            InitializeComponent();
            CheckSX();
        }

        private void BnkWavExtract_Click(object sender, EventArgs e)
        {

        }

        private bool CheckSX()
        {
            string StringPath = AppContext.BaseDirectory;
            if (Directory.GetFiles(StringPath, "sx.exe", SearchOption.TopDirectoryOnly).Length==1)
            {
                return true;
            }
            MessageBox.Show("Missing sx.exe");
            return false;


        }
    }
}
