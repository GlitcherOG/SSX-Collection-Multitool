﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using SSXMultiTool.FileHandlers.Textures;
using SSXMultiTool.Utilities;

namespace SSXMultiTool
{
    public partial class NewSSHImageTools : Form
    {
        NewSSHHandler sshHandler = new NewSSHHandler();
        bool DisableUpdate;
        public NewSSHImageTools(string OpenFile = "")
        {
            InitializeComponent();
            if (File.Exists(OpenFile))
            {
                sshHandler = new NewSSHHandler();
                sshHandler.LoadSSH(OpenFile);
                UpdateFileText();
            }
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
                sshHandler = new NewSSHHandler();
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
                sshHandler = new NewSSHHandler();
                //sshHandler.LoadFolder(openFileDialog.FileName);
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
                //sshHandler.SaveSSH(openFileDialog.FileName, true);
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
            GimxVersionTextBox.Text = sshHandler.group;
            DisableUpdate = false;
        }

        private void ImageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!DisableUpdate && ImageList.SelectedIndex != -1)
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

                for (int i = 0; i < MatrixTypeDropdown.Items.Count; i++)
                {
                    if (MatrixTypeDropdown.Items[i].ToString().Contains(sshHandler.sshImages[a].MatrixType.ToString() + " "))
                    {
                        MatrixTypeDropdown.SelectedIndex = i;
                        break;
                    }
                    else
                    {
                        MatrixTypeDropdown.SelectedIndex = MatrixTypeDropdown.Items.Count - 1;
                    }
                }

                SSHCompressed.Checked = SSHImage.Compressed;
                ImageByteSwappedCheckbox.Checked= SSHImage.SwizzledImage;
                ColourByteSwappedCheckbox.Checked = SSHImage.SwizzledColours;

                ImageSizeLabel.Text = SSHImage.bitmap.Width + " x " + SSHImage.bitmap.Height;
                //if (SSHImage.sshHeader.LXPos == 0)
                //{
                //    ImageByteSwappedCheckbox.Checked = false;
                //}
                //else
                //{
                //    ImageByteSwappedCheckbox.Checked = true;
                //}

                ColourAmountLabel.Text = SSHImage.colorsTable.Count.ToString();
                ColourAlphaFix.Checked = SSHImage.AlphaFix;

                //if (SSHImage.sshTable.Format == 0)
                //{
                //    ColourByteSwappedCheckbox.Checked = false;
                //}
                //else
                //{
                //    ColourByteSwappedCheckbox.Checked = true;
                //}


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
                sshHandler.group = GimxVersionTextBox.Text;
            }
        }

        private void UpdateImageDetails(object sender, EventArgs e)
        {
            if (!DisableUpdate && ImageList.SelectedIndex != -1)
            {
                DisableUpdate = true;
                var ImageDetails = sshHandler.sshImages[ImageList.SelectedIndex];
                ImageDetails.shortname = ImageShortNameTextbox.Text;
                string tempString = MatrixTypeDropdown.Text;
                string[] tempAString = tempString.Split(' ');
                tempString = tempAString[0];
                int indexInt = Int32.Parse(tempString);
                ImageDetails.MatrixType = (byte)indexInt;
                ImageDetails.AlphaFix = ColourAlphaFix.Checked;
                ImageDetails.Compressed = SSHCompressed.Checked;
                ImageDetails.SwizzledImage = ImageByteSwappedCheckbox.Checked;
                ImageDetails.SwizzledColours = ColourByteSwappedCheckbox.Checked;

                sshHandler.sshImages[ImageList.SelectedIndex] = ImageDetails;
                DisableUpdate = false;
            }
        }

        private void DoubleColourButton_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                //sshHandler.BrightenBitmap(ImageList.SelectedIndex);
                UpdateImageText(ImageList.SelectedIndex);
            }
        }

        private void DoubleAlphaButton_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                //sshHandler.DoubleAlphaImage(ImageList.SelectedIndex);
                UpdateImageText(ImageList.SelectedIndex);
            }
        }

        private void HalfColourButton_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                //sshHandler.DarkenImage(ImageList.SelectedIndex);
                UpdateImageText(ImageList.SelectedIndex);
            }
        }

        private void HalfAlphaButton_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                //sshHandler.HalfAlphaImage(ImageList.SelectedIndex);
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
                sshHandler.sshImages.Insert(ImageList.SelectedIndex - 1, Temp);
                UpdateFileText();
                ImageList.SelectedIndex = a - 1;
            }
        }

        private void ImageMoveDownButton_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1 && ImageList.Items.Count - 1 != ImageList.SelectedIndex)
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
            //sshHandler.AddImage();
            UpdateFileText();
            ImageList.SelectedIndex = a;
        }

        private void ImageRemoveButton_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                //sshHandler.RemoveImage(ImageList.SelectedIndex);
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
                    //SSHImage.sshTable.Format = 1;
                }
                else
                {
                    //SSHImage.sshTable.Format = 0;
                }
                sshHandler.sshImages[ImageList.SelectedIndex] = SSHImage;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
            };
            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string[] AllSSHFiles = Directory.GetFiles(openFileDialog.FileName, "*.ssh");
                ConsoleWindow.GenerateConsole();
                for (int i = 0; i < AllSSHFiles.Length; i++)
                {
                    Console.WriteLine("Extracting " + Path.GetFileName(AllSSHFiles[i]));

                    string FileName = Path.GetFileName(AllSSHFiles[i]).Replace(".ssh", "");

                    sshHandler = new NewSSHHandler();
                    sshHandler.LoadSSH(AllSSHFiles[i]);

                    Directory.CreateDirectory(openFileDialog.FileName + "\\" + FileName);

                    sshHandler.BMPExtract(openFileDialog.FileName + "\\" + FileName);

                    GC.Collect();
                }
                ConsoleWindow.CloseConsole();
            }
        }
    }
}
