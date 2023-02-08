using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using SSXMultiTool.FileHandlers;

namespace SSXMultiTool
{
    public partial class SSHImageTools : Form
    {
        SSHHandler sshHandler = new SSHHandler();
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
                sshHandler = new SSHHandler();
                sshHandler.LoadSSH(openFileDialog.FileName);
                UpdateFileText();
            }
        }

        private void LoadFolderButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
            };
            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                sshHandler = new SSHHandler();
                sshHandler.LoadFolder(openFileDialog.FileName);
            }
        }

        private void ExportAllButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
            };
            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                sshHandler.BMPExtract(openFileDialog.FileName);
                GC.Collect();
                Process.Start(openFileDialog.FileName);
            }
        }

        private void SaveSSHButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog
            {
                Filter = "SSH File (*.ssh)|*.ssh|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                sshHandler.SaveSSH(openFileDialog.FileName, true);
            }
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
            if (!DisableUpdate)
            {
                DisableUpdate = true;
                var SSHImage = sshHandler.sshImages[a];
                PictureBox1.Image = SSHImage.bitmap;
                ImageShortNameTextbox.Text = SSHImage.shortname;
                ImageLongNameTextbox.Text = SSHImage.longname;

                bool tempBool = SSHImage.MetalBin;
                SSHMetalExtract.Enabled = tempBool;
                SSHMetalImport.Enabled = tempBool;
                SSHBothExtract.Enabled = tempBool;
                SSHBothImport.Enabled = tempBool;

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

        private void GimxVersionTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!DisableUpdate)
            {
                sshHandler.format = GimxVersionTextBox.Text;
            }
        }

        private void UpdateImageDetails(object sender, EventArgs e)
        {
            if(!DisableUpdate && ImageList.SelectedIndex!=-1)
            {
                DisableUpdate = true;
                var ImageDetails = sshHandler.sshImages[ImageList.SelectedIndex];
                ImageDetails.shortname = ImageShortNameTextbox.Text;
                ImageDetails.longname = ImageLongNameTextbox.Text;
                string tempString = MatrixTypeDropdown.Text;
                string[] tempAString = tempString.Split(' ');
                tempString = tempAString[0];
                int indexInt = Int32.Parse(tempString);
                ImageDetails.sshHeader.MatrixFormat = (byte)indexInt;
                ImageDetails.AlphaFix = ColourAlphaFix.Checked;

                if (ColourByteSwappedCheckbox.Checked)
                {
                    ImageDetails.sshTable.Format = 2;
                }
                else
                {
                    ImageDetails.sshTable.Format = 0;
                }

                if (ImageByteSwappedCheckbox.Checked)
                {
                    ImageDetails.sshHeader.LXPos = 2;
                }
                else
                {
                    ImageDetails.sshHeader.LXPos = 0;
                }
                ImageDetails.MetalBin = MetalAlphaCheckbox.Checked;
                bool tempBool = ImageDetails.MetalBin;
                SSHMetalExtract.Enabled = tempBool;
                SSHMetalImport.Enabled = tempBool;
                SSHBothExtract.Enabled = tempBool;
                SSHBothImport.Enabled = tempBool;

                sshHandler.sshImages[ImageList.SelectedIndex] = ImageDetails;
                DisableUpdate = false;
            }
        }

        private void SSHMetalExtract_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                if (sshHandler.sshImages[ImageList.SelectedIndex].MetalBin)
                {
                    SaveFileDialog openFileDialog = new SaveFileDialog
                    {
                        Filter = "Png File (*.png)|*.png|All files (*.*)|*.*",
                        FilterIndex = 1,
                        RestoreDirectory = false
                    };
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        sshHandler.BMPOneExtractMetal(openFileDialog.FileName, ImageList.SelectedIndex);
                        GC.Collect();
                    }
                }
            }
        }

        private void SSHMetalImport_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                if (sshHandler.sshImages[ImageList.SelectedIndex].MetalBin)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog
                    {
                        Filter = "Png File (*.png)|*.png|All files (*.*)|*.*",
                        FilterIndex = 1,
                        RestoreDirectory = false
                    };
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        sshHandler.LoadSingleMetal(openFileDialog.FileName, ImageList.SelectedIndex);
                        GC.Collect();
                    }
                }
            }
        }

        private void SSHBothExtract_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                if (sshHandler.sshImages[ImageList.SelectedIndex].MetalBin)
                {
                    SaveFileDialog openFileDialog = new SaveFileDialog
                    {
                        Filter = "Png File (*.png)|*.png|All files (*.*)|*.*",
                        FilterIndex = 1,
                        RestoreDirectory = false
                    };
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        sshHandler.BMPOneBothExtract(openFileDialog.FileName, ImageList.SelectedIndex);
                        GC.Collect();
                    }
                }
            }
        }

        private void SSHBothImport_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                if (sshHandler.sshImages[ImageList.SelectedIndex].MetalBin)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog
                    {
                        Filter = "Png File (*.png)|*.png|All files (*.*)|*.*",
                        FilterIndex = 1,
                        RestoreDirectory = false
                    };
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        sshHandler.LoadSingleBoth(openFileDialog.FileName, ImageList.SelectedIndex);
                        UpdateImageText(ImageList.SelectedIndex);
                        GC.Collect();
                    }
                }
            }
        }

        private void DoubleColourButton_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                sshHandler.BrightenBitmap(ImageList.SelectedIndex);
                UpdateImageText(ImageList.SelectedIndex);
            }
        }

        private void DoubleAlphaButton_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                sshHandler.DoubleAlphaImage(ImageList.SelectedIndex);
                UpdateImageText(ImageList.SelectedIndex);
            }
        }

        private void HalfColourButton_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                sshHandler.DarkenImage(ImageList.SelectedIndex);
                UpdateImageText(ImageList.SelectedIndex);
            }
        }

        private void HalfAlphaButton_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                sshHandler.HalfAlphaImage(ImageList.SelectedIndex);
                UpdateImageText(ImageList.SelectedIndex);
            }
        }

        private void ReplaceImageButton_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Png File (*.png)|*.png|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    sshHandler.LoadSingle(openFileDialog.FileName, ImageList.SelectedIndex);
                    UpdateImageText(ImageList.SelectedIndex);
                }
            }
        }

        private void ExportImageButton_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                SaveFileDialog openFileDialog = new SaveFileDialog
                {
                    Filter = "Png File (*.png)|*.png|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    sshHandler.BMPOneExtract(openFileDialog.FileName, ImageList.SelectedIndex);
                    GC.Collect();
                }
            }
        }

        private void ImageMoveUpButton_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1 && ImageList.SelectedIndex != 0)
            {
                int a = ImageList.SelectedIndex;
                var Temp = sshHandler.sshImages[ImageList.SelectedIndex];
                sshHandler.sshImages.RemoveAt(ImageList.SelectedIndex);
                sshHandler.sshImages.Insert(ImageList.SelectedIndex-1, Temp);
                UpdateFileText();
                ImageList.SelectedIndex = a - 1;
            }
        }

        private void ImageMoveDownButton_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1 && ImageList.Items.Count-1 != ImageList.SelectedIndex)
            {
                int a = ImageList.SelectedIndex;
                var Temp = sshHandler.sshImages[ImageList.SelectedIndex];
                sshHandler.sshImages.RemoveAt(ImageList.SelectedIndex);
                sshHandler.sshImages.Insert(ImageList.SelectedIndex + 1, Temp);
                UpdateFileText();
                ImageList.SelectedIndex = a + 1;
            }
        }

        private void IamgeAddButton_Click(object sender, EventArgs e)
        {
            int a = ImageList.SelectedIndex;
            sshHandler.AddImage();
            UpdateFileText();
            ImageList.SelectedIndex = a;
        }

        private void ImageRemoveButton_Click(object sender, EventArgs e)
        {
            if(ImageList.SelectedIndex!=-1)
            {
                sshHandler.RemoveImage(ImageList.SelectedIndex);
                ImageList.SelectedIndex = -1;
                UpdateFileText();
            }
        }

        private void ColourByteSwappedCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                var SSHImage = sshHandler.sshImages[ImageList.SelectedIndex];
                if (ColourByteSwappedCheckbox.Checked)
                {
                    SSHImage.sshTable.Format = 1;
                }
                else
                {
                    SSHImage.sshTable.Format = 0;
                }
                sshHandler.sshImages[ImageList.SelectedIndex] = SSHImage;
            }
        }
    }
}
