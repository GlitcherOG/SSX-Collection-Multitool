using SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2;
using SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2;
using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace SSXMultiTool.FileHandlers
{
    public class DATAudio
    {
        public void Extract(string Path, HDRHandler hdrHandler)
        {

        }

        public void ExtractGuess(string OpenPath, string ExtractFolder)
        {
            Directory.CreateDirectory(Application.StartupPath + "/TempAudio");
            File.Copy(Application.StartupPath + "/sx.exe", Application.StartupPath + "/TempAudio/sx.exe", true);
            List<long> Offsets = new List<long>();
            using (Stream stream = File.Open(OpenPath, FileMode.Open))
            {
                while (true)
                {
                    long Offset = ByteUtil.FindPosition(stream, new byte[4] { 0x53, 0x43, 0x48, 0x6C}, -1, -1);

                    if(Offset!=-1)
                    {
                        Offsets.Add(Offset);

                    }
                    else
                    {
                        break;
                    }
                }

                for (int i = 0; i < Offsets.Count; i++)
                {
                    stream.Position = Offsets[i];

                    long ByteSize = 0;

                    if(i==Offsets.Count-1)
                    {
                        ByteSize = stream.Length - Offsets[i];
                    }
                    else
                    {
                        ByteSize = Offsets[i + 1] - Offsets[i];
                    }

                    MemoryStream StreamMemory = new MemoryStream();

                    byte[] Data = StreamUtil.ReadBytes(stream, (int)ByteSize);

                    StreamUtil.WriteBytes(StreamMemory, Data);

                    if (File.Exists(Application.StartupPath + "/TempAudio/Temp.mus"))
                    {
                        File.Delete(Application.StartupPath + "/TempAudio/Temp.mus");
                    }
                    var file = File.Create(Application.StartupPath + "/TempAudio/Temp.mus");
                    StreamMemory.Position = 0;
                    StreamMemory.CopyTo(file);
                    StreamMemory.Dispose();
                    file.Close();

                    Process cmd = new Process();
                    cmd.StartInfo.FileName = "cmd.exe";
                    cmd.StartInfo.RedirectStandardInput = true;
                    cmd.StartInfo.RedirectStandardOutput = false;
                    cmd.StartInfo.CreateNoWindow = true;
                    cmd.StartInfo.UseShellExecute = false;
                    cmd.Start();

                    FileInfo f = new FileInfo(Application.StartupPath);
                    string drive = System.IO.Path.GetPathRoot(f.FullName.Substring(0, 2));

                    cmd.StandardInput.WriteLine(drive);
                    cmd.StandardInput.WriteLine("cd " + Application.StartupPath + "/TempAudio");
                    cmd.StandardInput.WriteLine("sx.exe -wave -s16l_int -playlocmaincpu  Temp.mus -=Temp.wav");
                    cmd.StandardInput.Flush();
                    cmd.StandardInput.Close();
                    cmd.WaitForExit();

                    File.Copy(Application.StartupPath + "/TempAudio/Temp.wav", ExtractFolder + "/" + i + ".wav", true);
                    //File.Delete(ExtractFolder + "/" + i + ".wav");
                }
            }

            Directory.Delete(Application.StartupPath + "/TempAudio", true);
        }
    }
}
