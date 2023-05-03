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
using System.Media;
using NAudio;
using NAudio.Wave;
using SSXMultiTool.FileHandlers;
using SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2;
using SSXMultiTool.FileHandlers.Audio;

namespace SSXMultiTool.Tools
{
    public partial class AudioTools : Form
    {
        public AudioTools()
        {
            InitializeComponent();
            CheckSX();
        }

        private void BnkWavExtract_Click(object sender, EventArgs e)
        {
            if (CheckSX())
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
                        string drive = Path.GetPathRoot(f.FullName.Substring(0, 2));

                        cmd.StandardInput.WriteLine(drive);
                        cmd.StandardInput.WriteLine("cd " + Application.StartupPath + "/TempAudio");
                        cmd.StandardInput.WriteLine("sx.exe -wave -s16l_int -playlocmaincpu Audio.bnk -onetomany -=*");
                        cmd.StandardInput.Flush();
                        cmd.StandardInput.Close();
                        cmd.WaitForExit();

                        //Run Command

                        File.Delete(Application.StartupPath + "/TempAudio/sx.exe");
                        File.Delete(Application.StartupPath + "/TempAudio/Audio.bnk");
                        string[] Files = Directory.GetFiles(Application.StartupPath + "/TempAudio");

                        int[] FileNumbers = new int[Files.Length];

                        for (int i = 0; i < FileNumbers.Length; i++)
                        {
                            var temp = Files[i].Split(".");
                            FileNumbers[i] = int.Parse(temp[temp.Length - 1]);
                        }
                        Array.Sort(FileNumbers);


                        for (int i = 0; i < FileNumbers.Length; i++)
                        {
                            File.Copy(Application.StartupPath + "/TempAudio/." + FileNumbers[i], openFileDialog1.FileName + "/" + $"{FileNumbers[i]:000}" + ".wav", true);
                            File.Delete(Application.StartupPath + "/TempAudio/." + FileNumbers[i]);
                        }

                        while (Directory.GetFiles(Application.StartupPath + "/TempAudio").Length != 0)
                        {

                        }

                        Directory.Delete(Application.StartupPath + "/TempAudio", true);

                        MessageBox.Show("Audio Extracted");
                    }
                }

            }
        }

        private bool CheckSX()
        {
            string StringPath = Application.StartupPath;
            if (Directory.GetFiles(StringPath, "sx.exe", SearchOption.TopDirectoryOnly).Length == 1)
            {
                return true;
            }
            MessageBox.Show("Missing sx.exe");
            return false;
        }

        public List<string> Files = new List<string>();

        private void BnkLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Wav Audio File (*.wav)|*.wav|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Files.Add(openFileDialog.FileName);

                BnkFileList.Items.Clear();
                for (int i = 0; i < Files.Count; i++)
                {
                    BnkFileList.Items.Add(Path.GetFileName(Files[i]));
                }
            }
        }

        private void BnkRemoveFile_Click(object sender, EventArgs e)
        {
            if (BnkFileList.SelectedIndex != -1)
            {
                Files.RemoveAt(BnkFileList.SelectedIndex);

                BnkFileList.Items.Clear();
                for (int i = 0; i < Files.Count; i++)
                {
                    BnkFileList.Items.Add(Path.GetFileName(Files[i]));
                }
            }
        }

        private void BnkUp_Click(object sender, EventArgs e)
        {
            if (BnkFileList.SelectedIndex != -1 && BnkFileList.SelectedIndex != 0)
            {
                int Pos = BnkFileList.SelectedIndex;
                var Tempstring = Files[Pos];
                Files.RemoveAt(Pos);
                Files.Insert(Pos - 1, Tempstring);
                BnkFileList.Items.Clear();

                for (int i = 0; i < Files.Count; i++)
                {
                    BnkFileList.Items.Add(Path.GetFileName(Files[i]));
                }

                BnkFileList.SelectedIndex = Pos - 1;
            }
        }

        private void BnkDown_Click(object sender, EventArgs e)
        {
            if (BnkFileList.SelectedIndex != -1 && BnkFileList.SelectedIndex != Files.Count - 1)
            {
                int Pos = BnkFileList.SelectedIndex;
                var Tempstring = Files[Pos];
                Files.RemoveAt(Pos);
                Files.Insert(Pos + 1, Tempstring);
                BnkFileList.Items.Clear();

                for (int i = 0; i < Files.Count; i++)
                {
                    BnkFileList.Items.Add(Path.GetFileName(Files[i]));
                }

                BnkFileList.SelectedIndex = Pos + 1;
            }
        }

        private void BnkLoadFolder_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog openFileDialog1 = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "Select Wav Folder",
            };
            if (openFileDialog1.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string[] NewFiles = Directory.GetFiles(openFileDialog1.FileName, "*.wav", SearchOption.TopDirectoryOnly);

                Files = NewFiles.ToList();

                BnkFileList.Items.Clear();
                for (int i = 0; i < Files.Count; i++)
                {
                    BnkFileList.Items.Add(Path.GetFileName(Files[i]));
                }
            }
        }

        private void BnkBuild_Click(object sender, EventArgs e)
        {
            if (Files.Count != 0)
            {
                SaveFileDialog openFileDialog = new SaveFileDialog
                {
                    Filter = "PS2 Audio File (*.bnk)|*.bnk|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Directory.CreateDirectory(Application.StartupPath + "/TempAudio");
                    File.Copy(Application.StartupPath + "/sx.exe", Application.StartupPath + "/TempAudio/sx.exe");
                    for (int i = 0; i < Files.Count; i++)
                    {
                        File.Copy(Files[i], Application.StartupPath + "/TempAudio/" + $"{i:000}" + ".wav");
                    }

                    Process cmd = new Process();
                    cmd.StartInfo.FileName = "cmd.exe";
                    cmd.StartInfo.RedirectStandardInput = true;
                    cmd.StartInfo.RedirectStandardOutput = false;
                    cmd.StartInfo.CreateNoWindow = false;
                    cmd.StartInfo.UseShellExecute = false;
                    cmd.Start();

                    FileInfo f = new FileInfo(Application.StartupPath);
                    string drive = Path.GetPathRoot(f.FullName);

                    cmd.StandardInput.WriteLine(drive.Substring(0, 2));
                    cmd.StandardInput.WriteLine("cd " + Application.StartupPath + "/TempAudio");
                    cmd.StandardInput.WriteLine("sx.exe -ps2bank -mt_blk -playlocmaincpu  *.wav -=Audio.bnk");
                    cmd.StandardInput.Flush();
                    cmd.StandardInput.Close();
                    cmd.WaitForExit();

                    File.Copy(Application.StartupPath + "/TempAudio/Audio.bnk", openFileDialog.FileName);

                    Directory.Delete(Application.StartupPath + "/TempAudio", true);

                    MessageBox.Show("Audio BNK Built");
                }
            }
        }

        private void BnkFileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BnkFileList.SelectedIndex != -1)
            {
                using (var reader = new WaveFileReader(Files[BnkFileList.SelectedIndex]))
                {
                    BnkTime.Text = reader.TotalTime.ToString(@"hh\:mm\:ss\.ff");
                    BnkSample.Text = reader.WaveFormat.SampleRate.ToString();
                    BnkFileSize.Text = reader.Length.ToString();
                    BnkTotalSamples.Text = (reader.TotalTime.TotalSeconds * reader.WaveFormat.SampleRate).ToString();

                }
            }
        }
        WaveFileReader waveFile = null;
        WaveOut waveOut = null;
        private void bnkPlay_Click(object sender, EventArgs e)
        {
            if (BnkFileList.SelectedIndex != -1)
            {
                if (waveOut != null)
                {
                    waveOut.Stop();
                    waveFile.Close();
                }
                waveFile = new WaveFileReader(Files[BnkFileList.SelectedIndex]);
                waveOut = new WaveOut();
                waveOut.Init(waveFile);
                waveOut.Play();
            }
        }

        private void NewLoadTest_Click(object sender, EventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog
            {
                Filter = "PS2 Audio File (*.mus)|*.mus|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                EAAudioHandler audioHandler = new EAAudioHandler();

                audioHandler.Load(openFileDialog.FileName);
            }
         }
    }
}
