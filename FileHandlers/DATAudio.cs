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
            File.Copy(Application.StartupPath + "/sx_2002.exe", Application.StartupPath + "/TempAudio/sx.exe", true);
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

                    while(File.Exists(Application.StartupPath + "/TempAudio/Temp.mus"))
                    {

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

                    File.Copy(Application.StartupPath + "/TempAudio/Temp.wav", ExtractFolder + "/" + $"{i:000}" + ".wav", true);
                    //File.Delete(ExtractFolder + "/" + i + ".wav");
                }
            }

            Directory.Delete(Application.StartupPath + "/TempAudio", true);
        }

        public HDRHandler GenerateDATAndHDR(string[] FileOpen, string FileSave, HDRHandler hdrHandler)
        {
            Directory.CreateDirectory(Application.StartupPath + "/TempAudio");
            Directory.CreateDirectory(Application.StartupPath + "/TempAudio/Holder");
            File.Copy(Application.StartupPath + "/sx_2002.exe", Application.StartupPath + "/TempAudio/sx.exe", true);
            if (File.Exists(FileSave))
            {
                File.Delete(FileSave);
            }
            while (File.Exists(FileSave))
            {

            }
            var file = File.Create(FileSave);
            while (!File.Exists(FileSave))
            {

            }
            file.Close();

            List<string> HolderPaths = new List<string>();
            //Create File and memory stream
            using (Stream stream = File.Open(FileSave, FileMode.Open))
            {
                for (int i = 0; i < FileOpen.Length; i++)
                {
                    //Copy File
                    if (File.Exists(Application.StartupPath + "/TempAudio/Temp.mus"))
                    {
                        File.Delete(Application.StartupPath + "/TempAudio/Temp.mus");
                    }

                    while (File.Exists(Application.StartupPath + "/TempAudio/Temp.mus"))
                    {

                    }

                    File.Copy(FileOpen[i], Application.StartupPath + "/TempAudio/Temp.wav", true);


                    Process cmd = new Process();
                    cmd.StartInfo.FileName = "cmd.exe";
                    cmd.StartInfo.RedirectStandardInput = true;
                    cmd.StartInfo.RedirectStandardOutput = false;
                    cmd.StartInfo.CreateNoWindow = false;
                    cmd.StartInfo.UseShellExecute = false;
                    cmd.Start();

                    FileInfo f = new FileInfo(Application.StartupPath);
                    string drive = System.IO.Path.GetPathRoot(f.FullName.Substring(0, 2));

                    cmd.StandardInput.WriteLine(drive);
                    cmd.StandardInput.WriteLine("cd " + Application.StartupPath + "/TempAudio");
                    cmd.StandardInput.WriteLine("sx.exe -ps2stream -mt_blk -playlocmaincpu -removeuserall Temp.wav -=Temp.mus -v3");
                    cmd.StandardInput.Flush();
                    cmd.StandardInput.Close();
                    cmd.WaitForExit();


                    HolderPaths.Add(Application.StartupPath + "/TempAudio/Holder/" + $"{i:000}" + ".Mus");
                    File.Copy(Application.StartupPath + "/TempAudio/Temp.mus", Application.StartupPath + "/TempAudio/Holder/" + $"{i:000}" + ".Mus", true);
                }
                long CurrentOffset = 0;
                //Recalculate Offsets
                for (int i = 0; i < HolderPaths.Count; i++)
                {
                    var TempHolder = File.Open(HolderPaths[i], FileMode.Open);
                    var TempHdrHeader = hdrHandler.fileHeaders[i];
                    TempHolder.Position = TempHolder.Length;
                    StreamUtil.AlignBy(TempHolder, 0x100 * (hdrHandler.AligmentSize + 1));
                    long FixedLength = TempHolder.Position;
                    TempHolder.Close();
                    TempHdrHeader.OffsetInt = (int)(CurrentOffset / (0x100 * (hdrHandler.AligmentSize + 1)));
                    CurrentOffset += FixedLength;

                    hdrHandler.fileHeaders[i] = TempHdrHeader;

                }



                for (int i = 0; i < HolderPaths.Count; i++)
                {
                    using (Stream stream1 = File.Open(HolderPaths[i], FileMode.Open))
                    {
                        stream.Position = (hdrHandler.fileHeaders[i].OffsetInt * 0x100) / (hdrHandler.AligmentSize + 1);

                        StreamUtil.WriteStreamIntoStream(stream, stream1);
                    }

                    File.Delete(HolderPaths[i]);
                }

            }



            Directory.Delete(Application.StartupPath + "/TempAudio", true);

            return hdrHandler;
        }

        public HDRHandler GenerateDATAndHDR3(string[] FileOpen, string FileSave, HDRHandler hdrHandler)
        {
            Directory.CreateDirectory(Application.StartupPath + "/TempAudio");
            Directory.CreateDirectory(Application.StartupPath + "/TempAudio/Holder");
            File.Copy(Application.StartupPath + "/sx_2002.exe", Application.StartupPath + "/TempAudio/sx.exe", true);
            if (File.Exists(FileSave))
            {
                File.Delete(FileSave);
            }
            while (File.Exists(FileSave))
            {

            }
            var file = File.Create(FileSave);
            while (!File.Exists(FileSave))
            {

            }
            file.Close();

            List<string> HolderPaths = new List<string>();
            //Create File and memory stream
            using (Stream stream = File.Open(FileSave, FileMode.Open))
            {
                for (int i = 0; i < FileOpen.Length; i++)
                {
                    //Copy File
                    if (File.Exists(Application.StartupPath + "/TempAudio/Temp.mus"))
                    {
                        File.Delete(Application.StartupPath + "/TempAudio/Temp.mus");
                    }

                    while (File.Exists(Application.StartupPath + "/TempAudio/Temp.mus"))
                    {

                    }

                    File.Copy(FileOpen[i], Application.StartupPath + "/TempAudio/Temp.wav", true);


                    Process cmd = new Process();
                    cmd.StartInfo.FileName = "cmd.exe";
                    cmd.StartInfo.RedirectStandardInput = true;
                    cmd.StartInfo.RedirectStandardOutput = false;
                    cmd.StartInfo.CreateNoWindow = false;
                    cmd.StartInfo.UseShellExecute = false;
                    cmd.Start();

                    FileInfo f = new FileInfo(Application.StartupPath);
                    string drive = System.IO.Path.GetPathRoot(f.FullName.Substring(0, 2));

                    cmd.StandardInput.WriteLine(drive);
                    cmd.StandardInput.WriteLine("cd " + Application.StartupPath + "/TempAudio");
                    cmd.StandardInput.WriteLine("sx.exe -ps2stream -eaxa_blk -playlocmaincpu -removeuserall Temp.wav -=Temp.mus -v3");
                    cmd.StandardInput.Flush();
                    cmd.StandardInput.Close();
                    cmd.WaitForExit();


                    HolderPaths.Add(Application.StartupPath + "/TempAudio/Holder/" + $"{i:000}" + ".Mus");
                    File.Copy(Application.StartupPath + "/TempAudio/Temp.mus", Application.StartupPath + "/TempAudio/Holder/" + $"{i:000}" + ".Mus", true);
                }
                long CurrentOffset = 0;
                //Recalculate Offsets
                for (int i = 0; i < HolderPaths.Count; i++)
                {
                    var TempHolder = File.Open(HolderPaths[i], FileMode.Open);
                    var TempHdrHeader = hdrHandler.fileHeaders[i];
                    TempHolder.Position = TempHolder.Length;
                    StreamUtil.AlignBy(TempHolder, 0x100 * (hdrHandler.AligmentSize + 1));
                    long FixedLength = TempHolder.Position;
                    TempHolder.Close();
                    TempHdrHeader.OffsetInt = (int)(CurrentOffset / (0x100 * (hdrHandler.AligmentSize + 1)));
                    CurrentOffset += FixedLength;

                    hdrHandler.fileHeaders[i] = TempHdrHeader;

                }



                for (int i = 0; i < HolderPaths.Count; i++)
                {
                    using (Stream stream1 = File.Open(HolderPaths[i], FileMode.Open))
                    {
                        stream.Position = (hdrHandler.fileHeaders[i].OffsetInt * 0x100) / (hdrHandler.AligmentSize + 1);

                        StreamUtil.WriteStreamIntoStream(stream, stream1);
                    }

                    File.Delete(HolderPaths[i]);
                }

            }



            Directory.Delete(Application.StartupPath + "/TempAudio", true);

            return hdrHandler;
        }
    }
}
