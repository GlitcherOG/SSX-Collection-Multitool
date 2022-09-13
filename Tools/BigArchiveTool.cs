using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using SSXMultiTool.FileHandlers;

namespace SSXMultiTool
{
    public partial class BigArchiveTool : Form
    {
        BigHandler bigHandler = new BigHandler();
        public BigArchiveTool()
        {
            InitializeComponent();
            SetupDataView();
            ExtractBigArchive.Enabled = false;
            BuildBigArchive.Enabled=false;
        }

        public void SetupDataView()
        {
            BigDataView.ColumnCount = 5;
            BigDataView.Columns[0].Name = "File Name";
            BigDataView.Columns[1].Name = "Data Offset (Bytes)";
            BigDataView.Columns[2].Name = "Current File Size (Bytes)";
            BigDataView.Columns[3].Name = "Compressed";
            BigDataView.Columns[4].Name = "Uncompressed File Size (Bytes)";
            BigDataView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            BigDataView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            BigDataView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            BigDataView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            BigDataView.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }

        public void LoadDataRows()
        {
            BigDataView.Rows.Clear();
            for (int i = 0; i < bigHandler.bigFiles.Count; i++)
            {
                string[] NewRow = { bigHandler.bigFiles[i].path, bigHandler.bigFiles[i].offset.ToString(), bigHandler.bigFiles[i].size.ToString(), bigHandler.bigFiles[i].Compressed.ToString(), bigHandler.bigFiles[i].UncompressedSize.ToString() };
                BigDataView.Rows.Add(NewRow);
            }
        }

        public void LoadBigPath(string path)
        {
            bigHandler = new BigHandler();
            ExtractBigArchive.Enabled = true;
            BuildBigArchive.Enabled = false;
            bigHandler.LoadBig(path);
            BigTypeCombobox.Enabled = false;
            BigTypeCombobox.Text = bigHandler.bigType.ToString();
            this.Text = "Big Archive (" + path + ")";
            LoadDataRows();
        }

        public void LoadFolderPath(string path)
        {
            bigHandler = new BigHandler();
            ExtractBigArchive.Enabled = false;
            BuildBigArchive.Enabled = true;
            bigHandler.LoadFolder(path);
            BigTypeCombobox.Enabled = true;
            BigTypeCombobox.SelectedIndex = 0;
            this.Text = "Big Archive Folder Mode (" + path + ")";
            LoadDataRows();
        }

        public void ExtractBigPath(string path)
        {
            bigHandler.ExtractBig(path);
        }

        public void CreateBigPath(string path)
        {
            bigHandler.LoadFolder(path);
            bigHandler.bigType = (BigHandler.BigType)Enum.Parse(typeof(BigHandler.BigType), BigTypeCombobox.Text);
            bigHandler.BuildBig(path);
        }

        private void LoadBigArchive_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Big Archive (*.big)|*.big|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if(openFileDialog.ShowDialog()==DialogResult.OK)
            {
                LoadBigPath(openFileDialog.FileName);
            }
        }

        private void LoadFolder_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog commonDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };
            if (commonDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                LoadFolderPath(commonDialog.FileName);
            }
        }

        private void ExtractBigArchive_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog commonDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };
            if (commonDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ExtractBigPath(commonDialog.FileName);
            }
        }

        private void BuildBigArchive_Click(object sender, EventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog
            {
                Filter = "Big Archive (*.big)|*.big|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                CreateBigPath(openFileDialog.FileName);
            }
        }
    }
}
