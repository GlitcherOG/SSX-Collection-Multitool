﻿using SSXMultiTool.FileHandlers.LevelFiles.OGPS2;
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

namespace SSXMultiTool
{
    public partial class SSXOGProjectWindow : Form
    {
        public SSXOGProjectWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Map File (*.wdr)|*.wdr|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                WDRHandler handler = new WDRHandler();
                handler.LoadGuess(openFileDialog.FileName);

                Directory.CreateDirectory(Path.GetDirectoryName(openFileDialog.FileName) + "\\Models");
                handler.ExportModels(Path.GetDirectoryName(openFileDialog.FileName) + "\\Models");
            }
        }
    }
}
