using System;
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
using SSX_Library.EATextureLibrary;
using SSXLibrary.FileHandlers.Textures;
using SSXMultiTool.Utilities;

namespace SSXMultiTool
{
    public partial class SSHImageTools : Form
    {
        OldShapeHandler sshHandler = new OldShapeHandler();
        bool DisableUpdate;
        public SSHImageTools(string OpenFile = "")
        {
            InitializeComponent();
            if (File.Exists(OpenFile))
            {
                sshHandler = new OldShapeHandler();
                sshHandler.LoadShape(OpenFile);
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
                sshHandler = new OldShapeHandler();
                sshHandler.LoadShape(openFileDialog.FileName);
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
                sshHandler = new OldShapeHandler();
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
                sshHandler.ExtractImage(openFileDialog.FileName);
                GC.Collect();
                //Process.Start(openFileDialog.FileName);
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
                sshHandler.SaveShape(openFileDialog.FileName);
            }
        }

        void UpdateFileText()
        {
            DisableUpdate = true;
            ImageList.Items.Clear();
            for (int i = 0; i < sshHandler.ShapeImages.Count; i++)
            {
                ImageList.Items.Add(sshHandler.ShapeImages[i].Shortname);
            }
            FileNameLabel.Text = "NULL";
            GimxVersionTextBox.Text = sshHandler.Format;
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
                var SSHImage = sshHandler.ShapeImages[a];
                //PictureBox1.Image = SSHImage.Image;
                ImageShortNameTextbox.Text = SSHImage.Shortname;
                ImageLongNameTextbox.Text = SSHImage.Longname;

                bool tempBool = false;//SSHImage.Metal;
                SSHMetalExtract.Enabled = tempBool;
                SSHMetalImport.Enabled = tempBool;
                SSHBothExtract.Enabled = tempBool;
                SSHBothImport.Enabled = tempBool;

                for (int i = 0; i < MatrixTypeDropdown.Items.Count; i++)
                {
                    if (MatrixTypeDropdown.Items[i].ToString().Contains(sshHandler.ShapeImages[a].MatrixType.ToString() + " "))
                    {
                        MatrixTypeDropdown.SelectedIndex = i;
                        break;
                    }
                    else
                    {
                        MatrixTypeDropdown.SelectedIndex = MatrixTypeDropdown.Items.Count - 1;
                    }
                }

                YAxisNum.Value = SSHImage.Yaxis;
                XAxisNum.Value = SSHImage.Xaxis;

                ImageSizeLabel.Text = SSHImage.Image.Width + " x " + SSHImage.Image.Height;
                ImageByteSwappedCheckbox.Checked = SSHImage.SwizzledImage;

                ColourAmountLabel.Text = SSHImage.colorsTable.Count.ToString();
                //MetalAlphaCheckbox.Checked = SSHImage.MetalBin;
                ColourAlphaFix.Checked = SSHImage.AlphaFix;
                ColourByteSwappedCheckbox.Checked = SSHImage.SwizzledColours;

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
                sshHandler.Format = GimxVersionTextBox.Text;
            }
        }

        private void UpdateImageDetails(object sender, EventArgs e)
        {
            if (!DisableUpdate && ImageList.SelectedIndex != -1)
            {
                DisableUpdate = true;
                var ImageDetails = sshHandler.ShapeImages[ImageList.SelectedIndex];
                ImageDetails.Shortname = ImageShortNameTextbox.Text;
                ImageDetails.Longname = ImageLongNameTextbox.Text;
                string tempString = MatrixTypeDropdown.Text;
                string[] tempAString = tempString.Split(' ');
                tempString = tempAString[0];
                int indexInt = Int32.Parse(tempString);
                //ImageDetails.MatrixType = (OldShapeHandler.MatrixType)indexInt;
                ImageDetails.AlphaFix = ColourAlphaFix.Checked;

                ImageDetails.SwizzledColours = ColourByteSwappedCheckbox.Checked;
                ImageDetails.SwizzledImage = ImageByteSwappedCheckbox.Checked;

                //ImageDetails.MetalBin = MetalAlphaCheckbox.Checked;
                bool tempBool = false;//ImageDetails.MetalBin;
                SSHMetalExtract.Enabled = tempBool;
                SSHMetalImport.Enabled = tempBool;
                SSHBothExtract.Enabled = tempBool;
                SSHBothImport.Enabled = tempBool;

                sshHandler.ShapeImages[ImageList.SelectedIndex] = ImageDetails;
                DisableUpdate = false;
            }
        }

        private void SSHMetalExtract_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                //if (sshHandler.ShapeImages[ImageList.SelectedIndex].MetalBin)
                //{
                //    SaveFileDialog openFileDialog = new SaveFileDialog
                //    {
                //        Filter = "Png File (*.png)|*.png|All files (*.*)|*.*",
                //        FilterIndex = 1,
                //        RestoreDirectory = false
                //    };
                //    if (openFileDialog.ShowDialog() == DialogResult.OK)
                //    {
                //        //sshHandler.BMPOneExtractMetal(openFileDialog.FileName, ImageList.SelectedIndex);
                //        GC.Collect();
                //    }
                //}
            }
        }

        private void SSHMetalImport_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                //if (sshHandler.ShapeImages[ImageList.SelectedIndex].MetalBin)
                //{
                //    OpenFileDialog openFileDialog = new OpenFileDialog
                //    {
                //        Filter = "Png File (*.png)|*.png|All files (*.*)|*.*",
                //        FilterIndex = 1,
                //        RestoreDirectory = false
                //    };
                //    if (openFileDialog.ShowDialog() == DialogResult.OK)
                //    {
                //        //sshHandler.LoadSingleMetal(openFileDialog.FileName, ImageList.SelectedIndex);
                //        GC.Collect();
                //    }
                //}
            }
        }

        private void SSHBothExtract_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                //if (sshHandler.ShapeImages[ImageList.SelectedIndex].MetalBin)
                //{
                //    SaveFileDialog openFileDialog = new SaveFileDialog
                //    {
                //        Filter = "Png File (*.png)|*.png|All files (*.*)|*.*",
                //        FilterIndex = 1,
                //        RestoreDirectory = false
                //    };
                //    if (openFileDialog.ShowDialog() == DialogResult.OK)
                //    {
                //        sshHandler.ExtractSingleImage(openFileDialog.FileName, ImageList.SelectedIndex);
                //        GC.Collect();
                //    }
                //}
            }
        }

        private void SSHBothImport_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                //if (sshHandler.ShapeImages[ImageList.SelectedIndex].MetalBin)
                //{
                //    OpenFileDialog openFileDialog = new OpenFileDialog
                //    {
                //        Filter = "Png File (*.png)|*.png|All files (*.*)|*.*",
                //        FilterIndex = 1,
                //        RestoreDirectory = false
                //    };
                //    if (openFileDialog.ShowDialog() == DialogResult.OK)
                //    {
                //        //sshHandler.LoadSingleBoth(openFileDialog.FileName, ImageList.SelectedIndex);
                //        UpdateImageText(ImageList.SelectedIndex);
                //        GC.Collect();
                //    }
                //}
            }
        }

        private void DoubleColourButton_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                sshHandler.BrightenImage(ImageList.SelectedIndex);
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
                    sshHandler.LoadSingleImage(openFileDialog.FileName, ImageList.SelectedIndex);
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
                    sshHandler.ExtractSingleImage(openFileDialog.FileName, ImageList.SelectedIndex);
                    GC.Collect();
                }
            }
        }

        private void ImageMoveUpButton_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1 && ImageList.SelectedIndex != 0)
            {
                int a = ImageList.SelectedIndex;
                var Temp = sshHandler.ShapeImages[ImageList.SelectedIndex];
                sshHandler.ShapeImages.RemoveAt(ImageList.SelectedIndex);
                sshHandler.ShapeImages.Insert(ImageList.SelectedIndex - 1, Temp);
                UpdateFileText();
                ImageList.SelectedIndex = a - 1;
            }
        }

        private void ImageMoveDownButton_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1 && ImageList.Items.Count - 1 != ImageList.SelectedIndex)
            {
                int a = ImageList.SelectedIndex;
                var Temp = sshHandler.ShapeImages[ImageList.SelectedIndex];
                sshHandler.ShapeImages.RemoveAt(ImageList.SelectedIndex);
                sshHandler.ShapeImages.Insert(ImageList.SelectedIndex + 1, Temp);
                UpdateFileText();
                ImageList.SelectedIndex = a + 1;
            }
        }

        private void IamgeAddButton_Click(object sender, EventArgs e)
        {
            int a = ImageList.SelectedIndex;
            sshHandler.AddImage(OldShapeHandler.MatrixType.EightBit);
            UpdateFileText();
            ImageList.SelectedIndex = a;
        }

        private void ImageRemoveButton_Click(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                sshHandler.ShapeImages.RemoveAt(ImageList.SelectedIndex);
                ImageList.SelectedIndex = -1;
                UpdateFileText();
            }
        }

        private void ColourByteSwappedCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ImageList.SelectedIndex != -1)
            {
                var SSHImage = sshHandler.ShapeImages[ImageList.SelectedIndex];
                SSHImage.SwizzledColours = ColourByteSwappedCheckbox.Checked;
                sshHandler.ShapeImages[ImageList.SelectedIndex] = SSHImage;
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

                    sshHandler = new OldShapeHandler();
                    sshHandler.LoadShape(AllSSHFiles[i]);

                    Directory.CreateDirectory(openFileDialog.FileName + "\\" + FileName);

                    sshHandler.ExtractImage(openFileDialog.FileName + "\\" + FileName);

                    GC.Collect();
                }
                ConsoleWindow.CloseConsole();
            }
        }
    }
}
