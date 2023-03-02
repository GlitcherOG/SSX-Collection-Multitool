using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;

namespace SSXMultiTool.Tools
{
    public partial class AudioTools : Form
    {
        public AudioTools()
        {
            InitializeComponent();
            CheckSX();
        }

        public string[] Files = new string[1]; 

        private void BnkWavExtract_Click(object sender, EventArgs e)
        {
            if(CheckSX())
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "PS2 Audio File (*.bnk)|*.bnk|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    CommonOpenFileDialog openFileDialog1 = new CommonOpenFileDialog
                    {
                        IsFolderPicker = true,
                        Title = "Select Extract Folder",
                    };
                    if (openFileDialog1.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        Directory.CreateDirectory(Application.StartupPath + "/TempAudio");
                        File.Copy(openFileDialog.FileName, Application.StartupPath + "/TempAudio/Audio.bnk");
                        File.Copy(Application.StartupPath + "/sx.exe", Application.StartupPath + "/TempAudio/sx.exe");

                        Process cmd = new Process();
                        cmd.StartInfo.FileName = "cmd.exe";
                        cmd.StartInfo.RedirectStandardInput = true;
                        cmd.StartInfo.RedirectStandardOutput = true;
                        cmd.StartInfo.CreateNoWindow = true;
                        cmd.StartInfo.UseShellExecute = false;
                        cmd.Start();

                        FileInfo f = new FileInfo(Application.StartupPath);
                        string drive = Path.GetPathRoot(f.FullName);

                        cmd.StandardInput.WriteLine(drive);
                        cmd.StandardInput.WriteLine("cd " + Application.StartupPath + "/TempAudio");
                        cmd.StandardInput.WriteLine("sx.exe -wave -s16l_int -playlocmaincpu  Audio.bnk -onetomany -=*.wav");
                        cmd.StandardInput.Flush();
                        cmd.StandardInput.Close();
                        cmd.WaitForExit();

                        //Run Command

                        File.Delete(Application.StartupPath + "/TempAudio/sx.exe");
                        File.Delete(Application.StartupPath + "/TempAudio/Audio.bnk");
                        string[] Files = Directory.GetFiles(Application.StartupPath + "/TempAudio");
                        for (int i = 0; i < Files.Length; i++)
                        {
                            File.Copy(Files[i], openFileDialog1.FileName + "/" + i + ".wav", true);
                            File.Delete(Files[i]);
                        }

                        while(Directory.GetFiles(Application.StartupPath + "/TempAudio").Length!=0)
                        {

                        }

                        Directory.Delete(Application.StartupPath + "/TempAudio", true);
                    }
                }

            }
        }

        private bool CheckSX()
        {
            string StringPath = Application.StartupPath;
            if (Directory.GetFiles(StringPath, "sx.exe", SearchOption.TopDirectoryOnly).Length==1)
            {
                return true;
            }
            MessageBox.Show("Missing sx.exe");
            return false;
        }
    }
}
