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
                for (int i = 0; i < lOCEditor.textList.Count; i++)
                {
                    ListText.Items.Add(lOCEditor.textList[i]);
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
                MainTextbox.Text = lOCEditor.textList[ListText.SelectedIndex];
            }
        }

        private void MainTextbox_TextChanged(object sender, EventArgs e)
        {
            if (ListText.SelectedIndex != -1)
            {
                int pos = MainTextbox.SelectionStart;
                lOCEditor.textList[ListText.SelectedIndex] = MainTextbox.Text;
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
                if (lOCEditor.textList[i].ToLower().Contains(SearchTextBar.Text.ToLower()))
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
            for (int i = a; i < lOCEditor.textList.Count; i++)
            {
                if (lOCEditor.textList[i].ToLower().Contains(SearchTextBar.Text.ToLower()))
                {
                    ListText.SelectedIndex = i;
                    break;
                }
            }
        }
    }
}
