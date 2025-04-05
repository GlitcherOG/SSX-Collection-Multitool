using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;
using SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2.SSBData;
using System.Diagnostics;
using SSXMultiTool.JsonFiles.SSX3;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2
{
    internal class SSBHandler
    {
        /*
            
        All IDS
        0 - Unknown
        1 - Patches
        2 - WorldMDR
        3 - Instance?
        4 - Physics?
        5 - Spline Segment?
        6 - Spline?
        7 -
        8 -
        9 - Shape
        10 - Old Shape Lightmaps
        11 - Spline?
        12 - Collision?
        13 - SSF
        14 - AIP
        15 - 
        16 - Effects?
        17 -
        18 - 
        19 - 
        20 - AudioBank
        21 - 
        22 - End
         */
        public void LoadAndExtractSSB(string path, string extractPath)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                MemoryStream memoryStream = new MemoryStream();
                List<int> ints = new List<int>();
                int a = 0;
                int CEND = 0;
                int CBXS = 0;
                while (true)
                {
                    if (stream.Position >= stream.Length - 1)
                    {
                        break;
                    }
                    string MagicWords = StreamUtil.ReadString(stream, 4);

                    int Size = StreamUtil.ReadUInt32(stream);
                    byte[] Data = new byte[Size - 8];
                    byte[] DecompressedData = new byte[1];
                    Data = StreamUtil.ReadBytes(stream, Size - 8);

                    DecompressedData = RefpackHandler.Decompress(Data);
                    StreamUtil.WriteBytes(memoryStream, DecompressedData);

                    if (MagicWords.ToUpper() == "CBXS")
                    {
                        CBXS += 1;
                    }

                    if (MagicWords.ToUpper() == "CEND")
                    {
                        CEND += 1;
                        int FilePos = 0;
                        memoryStream.Position = 0;
                        Directory.CreateDirectory(extractPath + "//" + a);
                        while (memoryStream.Position < memoryStream.Length)
                        {
                            int ID = StreamUtil.ReadUInt8(memoryStream);
                            int ChunkSize = StreamUtil.ReadInt24(memoryStream);
                            int TrackID = StreamUtil.ReadInt8(memoryStream);
                            int RID = StreamUtil.ReadInt24(memoryStream);

                            byte[] NewData = StreamUtil.ReadBytes(memoryStream, ChunkSize);

                            if (ID == 2)
                            {
                                WorldMDR worldMDR = new WorldMDR();
                                worldMDR.LoadData(NewData);
                            }

                            //var file = File.Create(extractPath + "//" + a + "//" + FilePos + "." + ID + "bin");
                            FilePos++;
                        }
                        memoryStream.Dispose();
                        memoryStream = new MemoryStream();
                        a++;
                    }
                }
            }
        }
        public void LoadAndExtractSSBFromSBD(string path, string extractPath, SDBHandler sdbHandler)
        {
            ConsoleWindow.GenerateConsole();
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                PatchesJsonHandler patchesJsonHandler = new PatchesJsonHandler();
                Bin0JsonHandler bin0JsonHandler = new Bin0JsonHandler();
                Bin3JsonHandler bin3JsonHandler = new Bin3JsonHandler();
                Bin4JsonHandler bin4JsonHandler = new Bin4JsonHandler();
                Bin6JsonHandler bin6JsonHandler = new Bin6JsonHandler();

                MemoryStream memoryStream = new MemoryStream();
                List<int> ints = new List<int>();
                int a = 0;
                int splitCount = 1;
                int FilePos = 0;
                int ChunkID = -1;
                Directory.CreateDirectory(extractPath + "//Textures");
                Directory.CreateDirectory(extractPath + "//Lightmaps");
                while (true)
                {
                    if (stream.Position >= stream.Length - 1)
                    {
                        break;
                    }
                    string MagicWords = StreamUtil.ReadString(stream, 4);

                    int Size = StreamUtil.ReadUInt32(stream);
                    byte[] Data = new byte[Size - 8];
                    byte[] DecompressedData = new byte[1];
                    Data = StreamUtil.ReadBytes(stream, Size - 8);

                    DecompressedData = RefpackHandler.Decompress(Data);
                    StreamUtil.WriteBytes(memoryStream, DecompressedData);
                    if (MagicWords.ToUpper() == "CEND")
                    {
                        ChunkID = sdbHandler.FindLocationChunk(a);
                        Directory.CreateDirectory(extractPath + "//" + sdbHandler.locations[ChunkID].Name);
                        Directory.CreateDirectory(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Textures");
                        Directory.CreateDirectory(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Models");
                        Directory.CreateDirectory(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Sounds");
                        Directory.CreateDirectory(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Lightmaps");
                        memoryStream.Position = 0;

                        while (memoryStream.Position < memoryStream.Length)
                        {
                            MemoryStream memoryStream1 = new MemoryStream();
                            int ID = StreamUtil.ReadUInt8(memoryStream);
                            int ChunkSize = StreamUtil.ReadInt24(memoryStream);
                            int TrackID = StreamUtil.ReadUInt8(memoryStream);
                            int RID = StreamUtil.ReadInt24(memoryStream);

                            byte[] NewData = StreamUtil.ReadBytes(memoryStream, ChunkSize);
                            StreamUtil.WriteBytes(memoryStream1, NewData);

                            string ExtractPath = extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//";
                            string Path = RID + "-" + TrackID + "-" + FilePos;

                            if (ID == 0)
                            {
                                WorldBin0 worldbin0 = new WorldBin0();
                                memoryStream1.Position = 0;
                                worldbin0.LoadData(memoryStream1, TrackID, RID);

                                bin0JsonHandler.bin0Files.Add(worldbin0.ToJSON());
                            }
                            else if (ID == 1)
                            {
                                WorldPatch worldPatch = new WorldPatch();
                                memoryStream1.Position = 0;
                                worldPatch.LoadPatch(memoryStream1);

                                patchesJsonHandler.Patches.Add(worldPatch.ToJSON());
                            }
                            else if (ID == 2)
                            {
                                Console.WriteLine(ExtractPath + "//Models//" + Path + ".glb");
                                WorldMDR worldMDR = new WorldMDR();

                                worldMDR.LoadData(NewData);
                                worldMDR.SaveModel(ExtractPath + "//Models//" + Path + ".glb");
                            }
                            else if (ID == 3)
                            {
                                WorldBin3 worldBin3 = new WorldBin3();
                                memoryStream1.Position = 0;
                                worldBin3.LoadData(memoryStream1);

                                bin3JsonHandler.bin3Files.Add(worldBin3.ToJSON());
                            }
                            else if (ID == 4)
                            {
                                WorldBin4 worldBin4 = new WorldBin4();
                                memoryStream1.Position = 0;
                                worldBin4.LoadData(memoryStream1);

                                bin4JsonHandler.bin3Files.Add(worldBin4.ToJSON());
                            }
                            else if (ID == 6)
                            {
                                WorldBin6 worldBin6 = new WorldBin6();
                                memoryStream1.Position = 0;
                                worldBin6.LoadData(memoryStream1);

                                bin6JsonHandler.bin6Files.Add(worldBin6.ToJSON());
                            }
                            else if (ID == 9)
                            {
                                Console.WriteLine(ExtractPath + "//Textures//" + Path + ".png");
                                WorldSSH worldOldSSH = new WorldSSH();

                                worldOldSSH.Load(NewData);
                                worldOldSSH.SaveImage(ExtractPath + "//Textures//" + Path + ".png");
                                worldOldSSH.SaveImage(extractPath + "//Textures//" + RID + ".png");

                            }
                            else if (ID == 10)
                            {
                                Console.WriteLine(ExtractPath + "//Lightmaps//" + Path + ".png");
                                WorldOldSSH worldOldSSH = new WorldOldSSH();

                                worldOldSSH.Load(NewData);
                                worldOldSSH.SaveImage(ExtractPath + "//Lightmaps//" + Path + ".png");
                                worldOldSSH.SaveImage(extractPath + "//Lightmaps//" + RID.ToString().PadLeft(4, '0') + ".png");
                            }
                            else if (ID == 14)
                            {
                                Console.WriteLine(ExtractPath + ID + "-AIP.json");
                                WorldAIP worldAIP = new WorldAIP();
                                worldAIP.LoadData(NewData);
                                worldAIP.ToJson(ExtractPath + Path + "-AIP.json");
                            }
                            else if (ID == 20)
                            {
                                Console.WriteLine(ExtractPath + "//Sounds//" + Path + ".bnk");
                                var file = File.Create(ExtractPath + "//Sounds//" + Path + ".bnk");
                                memoryStream1.Position = 0;
                                memoryStream1.CopyTo(file);
                                memoryStream1.Dispose();
                                memoryStream1 = new MemoryStream();
                                file.Close();
                            }
                            else
                            {
                                Console.WriteLine(ExtractPath + Path + ".bin" + ID);
                                var file = File.Create(ExtractPath + Path + ".bin" + ID);
                                memoryStream1.Position = 0;
                                memoryStream1.CopyTo(file);
                                memoryStream1.Dispose();
                                memoryStream1 = new MemoryStream();
                                file.Close();
                            }

                            FilePos++;
                        }
                        int TempChunkID = sdbHandler.FindLocationChunk(a+1);
                        if (TempChunkID != ChunkID || stream.Position >= stream.Length - 1)
                        {
                            Console.WriteLine(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Patches.json");
                            patchesJsonHandler.CreateJson(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Patches.json");
                            Console.WriteLine(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Bin0.json");
                            bin0JsonHandler.CreateJson(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Bin0.json");
                            Console.WriteLine(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Bin3.json");
                            bin3JsonHandler.CreateJson(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Bin3.json");
                            Console.WriteLine(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Bin6.json");
                            bin6JsonHandler.CreateJson(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Bin6.json");

                            patchesJsonHandler = new PatchesJsonHandler();
                            bin0JsonHandler = new Bin0JsonHandler();
                            bin3JsonHandler = new Bin3JsonHandler();
                            bin6JsonHandler = new Bin6JsonHandler();
                        }
                        a++;
                        memoryStream = new MemoryStream();
                    }
                }
            }
            ConsoleWindow.CloseConsole();
        }

        public void PackSSB(string Folder, string BuildPath)
        {
            MemoryStream memoryStream = new MemoryStream();
            string[] AllFiles = Directory.GetFiles(Folder, "*.BSX");
            for (int i = 0; i < AllFiles.Length; i++)
            {
                using (Stream stream = File.Open(Folder +"//"+ i.ToString()+".BSX", FileMode.Open))
                {
                    byte[] bytes = new byte[1];
                    while (true)
                    {
                        byte[] output = new byte[32768];
                        bool End = false;
                        int ReadLength = 40000;
                        if (ReadLength+stream.Position>stream.Length)
                        {
                            ReadLength = (int)(stream.Length - stream.Position);
                            End = true;
                        }
                        long StartPos = stream.Position;
                        bool Start = true;
                        while(output.Length> 32768-8)
                        {
                            if (!Start)
                            {
                                stream.Position = StartPos;
                                ReadLength -= 32768 / 4;
                                End = false;
                            }
                            bytes = StreamUtil.ReadBytes(stream, ReadLength);
                            RefpackHandler.Compress(bytes, out output, CompressionLevel.Max);
                            Start = false;
                        }
                        
                        
                        if(!End)
                        {
                            StreamUtil.WriteString(memoryStream,"CBSX");
                        }
                        else
                        {
                            StreamUtil.WriteString(memoryStream, "CEND");
                        }

                        StreamUtil.WriteInt32(memoryStream, 32768);

                        StreamUtil.WriteBytes(memoryStream, output);

                        StreamUtil.AlignBy(memoryStream, 32768);

                        if(End)
                        {
                            break;
                        }

                    }
                }
            }
            if (File.Exists(BuildPath))
            {
                File.Delete(BuildPath);
            }
            var file = File.Create(BuildPath);
            memoryStream.Position = 0;
            memoryStream.CopyTo(file);
            memoryStream.Dispose();
            file.Close();
            GC.Collect();
        }
    }
}
