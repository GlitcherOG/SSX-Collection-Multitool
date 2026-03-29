using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using SSX_Library.EATextureLibrary;
using SSXMultiTool.Utilities;

namespace SSXMultiTool
{
    public partial class NewSSHImageTools : Form
    {
        NewShapeHandler sshHandler = new NewShapeHandler();
        bool DisableUpdate;
        public NewSSHImageTools(string OpenFile = "")
        {
            InitializeComponent();
            if (File.Exists(OpenFile))
            {
                sshHandler = new NewShapeHandler();
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
                sshHandler = new NewShapeHandler();
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
                sshHandler = new NewShapeHandler();
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
            GimxVersionTextBox.Text = sshHandler.Group;
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

                SSHCompressed.Checked = SSHImage.Compressed;
                ImageByteSwappedCheckbox.Checked= SSHImage.SwizzledImage;
                ColourByteSwappedCheckbox.Checked = SSHImage.SwizzledColours;

                ImageSizeLabel.Text = SSHImage.Image.Width + " x " + SSHImage.Image.Height;
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
                sshHandler.Group = GimxVersionTextBox.Text;
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
                ImageDetails.MatrixType = (NewShapeHandler.MatrixType)indexInt;
                ImageDetails.AlphaFix = ColourAlphaFix.Checked;
                ImageDetails.Compressed = SSHCompressed.Checked;
                ImageDetails.SwizzledImage = ImageByteSwappedCheckbox.Checked;
                ImageDetails.SwizzledColours = ColourByteSwappedCheckbox.Checked;

                sshHandler.ShapeImages[ImageList.SelectedIndex] = ImageDetails;
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
                    sshHandler.LoadShape(openFileDialog.FileName);
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

                    sshHandler = new NewShapeHandler();
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
