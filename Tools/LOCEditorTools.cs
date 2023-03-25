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
using SSXMultiTool.FileHandlers;

namespace SSXMultiTool
{
    public partial class LOCEditorTools : Form
    {
        LOCHandler lOCEditor = new LOCHandler();
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
                lOCEditor = new LOCHandler();
                ListText.Items.Clear();
                lOCEditor.ReadLocFile(openFileDialog.FileName);
                for (int i = 0; i < lOCEditor.StringList.Count; i++)
                {
                    ListText.Items.Add(lOCEditor.StringList[i]);
                }
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
                lOCEditor.SaveLodFile(openFileDialog.FileName);
            }
        }

        private void ListText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListText.SelectedIndex != -1)
            {
                MainTextbox.Text = lOCEditor.StringList[ListText.SelectedIndex];
            }
        }

        private void MainTextbox_TextChanged(object sender, EventArgs e)
        {
            if (ListText.SelectedIndex != -1)
            {
                int pos = MainTextbox.SelectionStart;
                lOCEditor.StringList[ListText.SelectedIndex] = MainTextbox.Text;
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
                if (lOCEditor.StringList[i].ToLower().Contains(SearchTextBar.Text.ToLower()))
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
            for (int i = a; i < lOCEditor.StringList.Count; i++)
            {
                if (lOCEditor.StringList[i].ToLower().Contains(SearchTextBar.Text.ToLower()))
                {
                    ListText.SelectedIndex = i;
                    break;
                }
            }
        }

        private void ExportTXT_Click(object sender, EventArgs e)
        {
            if (lOCEditor.StringList.Count != 0)
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

                    for (int i = 0; i < lOCEditor.StringList.Count; i++)
                    {
                        NewFile.WriteLine(lOCEditor.StringList[i].Replace("\n", "/n").Replace("\r","/r"));
                    }

                    NewFile.Close();
                }
            }
        }

        private void ImportTXT_Click(object sender, EventArgs e)
        {
            if (lOCEditor.StringList.Count != 0)
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

                    while (true)
                    {
                        string? TempString = OldFile.ReadLine();

                        if (TempString == null)
                        {
                            break;
                        }

                        strings.Add(TempString.Replace("/n", "\n").Replace("/r", "\r"));
                    }

                    if (strings.Count < lOCEditor.StringList.Count)
                    {
                        MessageBox.Show("Error Incorrect Ammount Of Strings " + strings.Count + "/" + lOCEditor.StringList.Count);
                    }

                    for (int i = 0; i < lOCEditor.StringList.Count; i++)
                    {
                        lOCEditor.StringList[i] = strings[i];
                    }

                    OldFile.Close();

                    ListText.Items.Clear();

                    for (int i = 0; i < lOCEditor.StringList.Count; i++)
                    {
                        ListText.Items.Add(lOCEditor.StringList[i]);
                    }

                }
            }
        }
    }
}
