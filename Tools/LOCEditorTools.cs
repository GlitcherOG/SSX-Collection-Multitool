using System.IO;
using System.Windows;
using SSX_Library;
using SSXLibrary.FileHandlers;

namespace SSXMultiTool
{
    public partial class LOCEditorTools : Form
    {
        LOC lOCEditor = new LOC();
        public LOCEditorTools()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Loc File (*.loc)|*.loc|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false,
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                lOCEditor = new LOC();
                ListText.Items.Clear();
                lOCEditor.Load(openFileDialog.FileName);
                for (int i = 0; i < lOCEditor.TextCount; i++)
                {
                    ListText.Items.Add(lOCEditor.GetTextByID(i));
                }
                this.Text = "LOC Editor - " + Path.GetFileName(openFileDialog.FileName); 
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog
            {
                Filter = "Loc File (*.loc)|*.loc|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                lOCEditor.Save(openFileDialog.FileName);
            }
        }

        private void ListText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListText.SelectedIndex != -1)
            {
                MainTextbox.Text = lOCEditor.GetTextByID(ListText.SelectedIndex);
            }
        }

        private void MainTextbox_TextChanged(object sender, EventArgs e)
        {
            if (ListText.SelectedIndex != -1)
            {
                int pos = MainTextbox.SelectionStart;
                lOCEditor.SetTextByID(ListText.SelectedIndex, MainTextbox.Text);
                ListText.Items[ListText.SelectedIndex] = MainTextbox.Text;
                MainTextbox.SelectionStart = pos;
            }
        }

        private void FindBackButton_Click(object sender, EventArgs e)
        {
            int a = ListText.SelectedIndex;
            if (a == -1)
            {
                a = ListText.Items.Count - 1;
            }
            else if (a != 0)
            {
                a--;
            }
            for (int i = a; i > -1; i--)
            {
                if (lOCEditor.GetTextByID(i).ToLower().Contains(SearchTextBar.Text.ToLower()))
                {
                    ListText.SelectedIndex = i;
                    break;
                }
            }
        }

        private void FindNextButton_Click(object sender, EventArgs e)
        {
            int a = ListText.SelectedIndex;
            if (ListText.SelectedIndex == -1)
            {
                a = 0;
            }
            else if (a != ListText.Items.Count - 1)
            {
                a++;
            }
            for (int i = a; i < lOCEditor.TextCount; i++)
            {
                if (lOCEditor.GetTextByID(i).ToLower().Contains(SearchTextBar.Text.ToLower()))
                {
                    ListText.SelectedIndex = i;
                    break;
                }
            }
        }

        private void ExportTXT_Click(object sender, EventArgs e)
        {
            if (lOCEditor.TextCount != 0)
            {
                SaveFileDialog openFileDialog = new SaveFileDialog
                {
                    Filter = "Txt File (*.txt)|*.txt|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(openFileDialog.FileName))
                    {
                        File.Delete(openFileDialog.FileName);
                    }

                    var NewFile = File.CreateText(openFileDialog.FileName);

                    for (int i = 0; i < lOCEditor.TextCount; i++)
                    {
                        NewFile.WriteLine(lOCEditor.GetTextByID(i)/*.Replace("\n", "/n").Replace("\r", "/r")*/);
                        NewFile.WriteLine("\n###New String###\n");
                    }

                    NewFile.Close();
                }
            }
        }

        private void ImportTXT_Click(object sender, EventArgs e)
        {
            if (lOCEditor.TextCount != 0)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Txt File (*.txt)|*.txt|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var OldFile = File.OpenText(openFileDialog.FileName);

                    List<string> strings = new List<string>();

                    string ReadTextLines = OldFile.ReadToEnd();
                    string[] SplitLines = ReadTextLines.Split("\n###New String###\n");

                    for (int i = 0; i < SplitLines.Length; i++)
                    {
                        strings.Add(SplitLines[i]);
                    }

                    //while (true)
                    //{
                    //    string? TempString = OldFile.ReadLine();

                    //    if (TempString == null)
                    //    {
                    //        break;
                    //    }

                    //    strings.Add(TempString.Replace("/n", "\n").Replace("/r", "\r"));
                    //}

                    if (strings.Count < lOCEditor.TextCount)
                    {
                        System.Windows.Forms.MessageBox.Show("Error Incorrect Amount Of Strings " + strings.Count + "/" + lOCEditor.TextCount);
                    }

                    for (int i = 0; i < lOCEditor.TextCount; i++)
                    {
                        lOCEditor.SetTextByID(i,strings[i]);
                    }

                    OldFile.Close();

                    ListText.Items.Clear();

                    for (int i = 0; i < lOCEditor.TextCount; i++)
                    {
                        ListText.Items.Add(lOCEditor.GetTextByID(i));
                    }

                }
            }
        }
    }
}
