using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using SSX_Library;

namespace SSXMultiTool
{
    public partial class BigArchiveTool : Form
    {
        bool Compressed;
        bool SlashMode;
        string FilePath;
        string FolderPath;
        List<MemberFileInfo> MemberFiles = new List<MemberFileInfo>();

        public BigArchiveTool(string OpenPath = "")
        {
            InitializeComponent();
            SetupDataView();
            BigTypeCombobox.SelectedIndex = 0;
            ExtractBigArchive.Enabled = false;
            BuildBigArchive.Enabled = false;

            if (File.Exists(OpenPath))
            {
                LoadBigPath(OpenPath);
            }
        }

        public void SetupDataView()
        {
            BigDataView.ColumnCount = 2;
            BigDataView.Columns[0].Name = "File Name";
            BigDataView.Columns[1].Name = "Current File Size (Bytes)";
            BigDataView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            BigDataView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        public void LoadDataRows()
        {
            BigDataView.Rows.Clear();
            for (int i = 0; i < MemberFiles.Count; i++)
            {
                string[] NewRow = { MemberFiles[i].Path, MemberFiles[i].Size.ToString()};
                BigDataView.Rows.Add(NewRow);
            }
        }

        public void LoadBigPath(string path)
        {
            FilePath = path;
            MemberFiles = BIG.GetMembersInfo(FilePath);
            ExtractBigArchive.Enabled = true;
            BuildBigArchive.Enabled = false;
            BigTypeCombobox.Enabled = false;
            BigTypeCombobox.Text = BIG.GetBigType(FilePath).ToString();
            this.Text = "Big Archive (" + path + ")";
            LoadDataRows();
            GC.Collect();
        }

        public void LoadFolderPath(string path)
        {
            FolderPath = path;

            ExtractBigArchive.Enabled = false;
            BuildBigArchive.Enabled = true;
            BigTypeCombobox.Enabled = true;
            //BigTypeCombobox.SelectedIndex = 0;

            //Replace with Load into Members Info
            //bigHandler.LoadFolder(path);

            this.Text = "Big Archive Folder Mode (" + path + ")";
            LoadDataRows();
        }

        public void ExtractBigPath(string path)
        {
            BIG.Extract(FilePath, path);
        }

        public void CreateBigPath(string path)
        {
            BIG.Create((BigType)Enum.Parse(typeof(BigType), BigTypeCombobox.Text), FolderPath, path, Compressed, SlashMode);
            MessageBox.Show("Big File Created");
        }

        private void LoadBigArchive_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Big Archive (*.big, *.viv, *.ser)|*.big;*.viv;*.ser|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    LoadBigPath(openFileDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("Error Loading .Big Archive");
                }
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
                try
                {
                    ExtractBigPath(commonDialog.FileName);
                    MessageBox.Show("Extracted");
                }
                catch
                {
                    MessageBox.Show("Error Extracting Big Archive");
                }
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
                GC.Collect();
            }
        }

        private void CompressionButton_Click(object sender, EventArgs e)
        {
            if (BigTypeCombobox.Enabled)
            {
                //LoadFolderPath(bigHandler.bigPath, !bigHandler.CompressBuild);
                Compressed = !Compressed;
                CompressionButton.Text = "Compressed Build: " + Compressed.ToString();
            }
        }

        private void BigTypeCombobox_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SlashMode = !SlashMode;

            if(!SlashMode)
            {
                SlashToggle.Text = "Slash: \\";
            }
            else
            {
                SlashToggle.Text = "Slash: /";
            }
        }
    }
}
