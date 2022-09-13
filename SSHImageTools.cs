using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SSXMultiTool.FileHandlers;

namespace SSXMultiTool
{
    public partial class SSHImageTools : Form
    {
        SSHHandler sshHandler = new SSHHandler();
        string path;
        bool DisableUpdate;
        public SSHImageTools()
        {
            InitializeComponent();
        }
        [STAThread]
        private void LoadSSHButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "SSH Image Archive (*.ssh)|*.ssh|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog.FileName;
                sshHandler.LoadSSH(openFileDialog.FileName);
                UpdateFileText();
            }
        }

        private void LoadFolderButton_Click(object sender, EventArgs e)
        {

        }

        private void ExportAllButton_Click(object sender, EventArgs e)
        {

        }

        private void SaveSSHButton_Click(object sender, EventArgs e)
        {

        }

        void UpdateFileText()
        {
            DisableUpdate = true;
            ImageList.Items.Clear();
            for (int i = 0; i < sshHandler.sshImages.Count; i++)
            {
                ImageList.Items.Add(sshHandler.sshImages[i].shortname);
            }
            FileNameLabel.Text = "NULL";
            GimxVersionTextBox.Text = sshHandler.format;
            DisableUpdate = false;
        }

        private void ImageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!DisableUpdate && ImageList.SelectedIndex != -1)
            {
                UpdateImageText(ImageList.SelectedIndex);
            }
        }

        void UpdateImageText(int a)
        {
            DisableUpdate = true;
            var SSHImage = sshHandler.sshImages[a];
            PictureBox1.Image = SSHImage.bitmap;
            ImageShortNameTextbox.Text = SSHImage.shortname;
            ImageLongNameTextbox.Text = SSHImage.longname;
            for (int i = 0; i < MatrixTypeDropdown.Items.Count; i++)
            {
                if (MatrixTypeDropdown.Items[i].ToString().Contains(sshHandler.sshImages[a].sshHeader.MatrixFormat.ToString() + " "))
                {
                    MatrixTypeDropdown.SelectedIndex = i;
                    break;
                }
                else
                {
                    MatrixTypeDropdown.SelectedIndex = MatrixTypeDropdown.Items.Count - 1;
                }
            }

            YAxisNum.Value = SSHImage.sshHeader.Yaxis;
            XAxisNum.Value = SSHImage.sshHeader.Xaxis;

            ImageSizeLabel.Text = SSHImage.bitmap.Width + " x " + SSHImage.bitmap.Height;
            if (SSHImage.sshHeader.LXPos == 0)
            {
                ImageByteSwappedCheckbox.Checked = false;
            }
            else
            {
                ImageByteSwappedCheckbox.Checked = true;
            }

            ColourAmountLabel.Text = SSHImage.sshTable.colorTable.Count.ToString();
            MetalAlphaCheckbox.Checked = SSHImage.MetalBin;
            ColourAlphaFix.Checked = SSHImage.AlphaFix;

            if (SSHImage.sshTable.Format == 0)
            {
                ColourByteSwappedCheckbox.Checked = false;
            }
            else
            {
                ColourByteSwappedCheckbox.Checked = true;
            }


            DisableUpdate = false;
        }

        private void BlackDisplayCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (BlackDisplayCheckBox.Checked)
            {
                PictureBox1.BackColor = Color.Black;
            }
            else
            {
                PictureBox1.BackColor = Color.White;
            }
        }
    }
}
