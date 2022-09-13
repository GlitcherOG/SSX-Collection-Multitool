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
    public partial class BigArchiveTool : Form
    {
        BigHandler bigHandler = new BigHandler();
        public BigArchiveTool()
        {
            InitializeComponent();
            SetupDataView();
            //LoadBigPath();
            LoadFolderPath();
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
            for (int i = 0; i < bigHandler.bigFiles.Count; i++)
            {
                string[] NewRow = { bigHandler.bigFiles[i].path, bigHandler.bigFiles[i].offset.ToString(), bigHandler.bigFiles[i].size.ToString(), bigHandler.bigFiles[i].Compressed.ToString(), bigHandler.bigFiles[i].UncompressedSize.ToString() };
                BigDataView.Rows.Add(NewRow);
            }
        }

        public void LoadBigPath()
        {
            string path = @"H:\Visual Studio Projects\SSX Modder\bin\Debug\disk\SSX Tricky\DATA\AUDIO\ALASKA.BIG";
            bigHandler.LoadBig(path);
            BigTypeCombobox.Text = bigHandler.bigType.ToString();
            this.Text = "Big Archive (" + path + ")";
            LoadDataRows();
        }

        public void LoadFolderPath()
        {
            string path = @"H:\Visual Studio Projects\SSX Modder\bin\Debug\disk\SSX Tricky\DATA\AUDIO";
            bigHandler.LoadFolder(path);
            BigTypeCombobox.SelectedIndex = 0;
            this.Text = "Big Archive Folder Mode (" + path + ")";
            LoadDataRows();
        }
    }
}
