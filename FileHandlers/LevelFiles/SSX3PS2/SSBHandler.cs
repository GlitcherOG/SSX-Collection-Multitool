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
        0 - Materials?
        1 - Patches
        2 - WorldMDR
        3 - Instance
        4 - Particle Model
        5 - Particles Instance
        6 - Lights
        7 - Halo
        8 - Splines
        9 - Shape
        10 - Old Shape Lightmaps
        11 - Vis Curtains
        12 - Collision?
        13 - Sound Triggers?
        14 - AIP
        15 - World Painter?
        16 - Scripts?
        17 - CameraTriggers?
        18 - NIS Table
        19 - Missions?
        20 - AudioBank
        21 - Radar?
        22 - Avalanche Animation
         */
        //public void LoadAndExtractSSB(string path, string extractPath)
        //{
        //    using (Stream stream = File.Open(path, FileMode.Open))
        //    {
        //        MemoryStream memoryStream = new MemoryStream();
        //        List<int> ints = new List<int>();
        //        int a = 0;
        //        int CEND = 0;
        //        int CBXS = 0;
        //        while (true)
        //        {
        //            if (stream.Position >= stream.Length - 1)
        //            {
        //                break;
        //            }
        //            string MagicWords = StreamUtil.ReadString(stream, 4);

        //            int Size = StreamUtil.ReadUInt32(stream);
        //            byte[] Data = new byte[Size - 8];
        //            byte[] DecompressedData = new byte[1];
        //            Data = StreamUtil.ReadBytes(stream, Size - 8);

        //            DecompressedData = RefpackHandler.Decompress(Data);
        //            StreamUtil.WriteBytes(memoryStream, DecompressedData);

        //            if (MagicWords.ToUpper() == "CBXS")
        //            {
        //                CBXS += 1;
        //            }

        //            if (MagicWords.ToUpper() == "CEND")
        //            {
        //                CEND += 1;
        //                int FilePos = 0;
        //                memoryStream.Position = 0;
        //                Directory.CreateDirectory(extractPath + "//" + a);
        //                while (memoryStream.Position < memoryStream.Length)
        //                {
        //                    int ID = StreamUtil.ReadUInt8(memoryStream);
        //                    int ChunkSize = StreamUtil.ReadInt24(memoryStream);
        //                    int TrackID = StreamUtil.ReadInt8(memoryStream);
        //                    int RID = StreamUtil.ReadInt24(memoryStream);

        //                    byte[] NewData = StreamUtil.ReadBytes(memoryStream, ChunkSize);

        //                    if (ID == 2)
        //                    {
        //                        WorldMDR worldMDR = new WorldMDR();
        //                        worldMDR.LoadData(NewData);
        //                    }

        //                    //var file = File.Create(extractPath + "//" + a + "//" + FilePos + "." + ID + "bin");
        //                    FilePos++;
        //                }
        //                memoryStream.Dispose();
        //                memoryStream = new MemoryStream();
        //                a++;
        //            }
        //        }
        //    }
        //}
        public void LoadAndExtractSSBFromSBD(string path, string extractPath, SDBHandler sdbHandler)
        {
            ConsoleWindow.GenerateConsole();
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                PatchesJsonHandler patchesJsonHandler = new PatchesJsonHandler();
                Bin0JsonHandler bin0JsonHandler = new Bin0JsonHandler();
                Bin3JsonHandler bin3JsonHandler = new Bin3JsonHandler();
                Bin5JsonHandler bin5JsonHandler = new Bin5JsonHandler();
                Bin6JsonHandler bin6JsonHandler = new Bin6JsonHandler();
                SplineJsonHandler splineJsonHandler = new SplineJsonHandler();
                VisCurtainJsonHandler visCurtainJsonHandler = new VisCurtainJsonHandler();

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
                        //Directory.CreateDirectory(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Textures");
                        Directory.CreateDirectory(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Models");
                        Directory.CreateDirectory(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Sounds");
                        //Directory.CreateDirectory(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Lightmaps");
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
                                Console.WriteLine(ExtractPath + "//Models//" + RID + ".glb");
                                WorldMDR worldMDR = new WorldMDR();

                                worldMDR.LoadData(memoryStream1);

 
                                //worldMDR.SaveModelGLB(ExtractPath + "//Models//" + RID + ".glb");
                            }
                            else if (ID == 3)
                            {
                                WorldBin3 worldBin3 = new WorldBin3();
                                memoryStream1.Position = 0;
                                worldBin3.LoadData(memoryStream1);

                                bin3JsonHandler.bin3Files.Add(worldBin3.ToJSON());
                            }
                            else if (ID == 5)
                            {
                                WorldBin5 worldBin5 = new WorldBin5();
                                memoryStream1.Position = 0;
                                worldBin5.LoadData(memoryStream1);

                                bin5JsonHandler.bin5Files.Add(worldBin5.ToJSON());
                            }
                            else if (ID == 6)
                            {
                                WorldBin6 worldBin6 = new WorldBin6();
                                memoryStream1.Position = 0;
                                worldBin6.LoadData(memoryStream1);

                                bin6JsonHandler.bin6Files.Add(worldBin6.ToJSON());
                            }
                            else if (ID == 8)
                            {
                                WorldSpline worldSpline = new WorldSpline();
                                memoryStream1.Position = 0;
                                worldSpline.LoadData(memoryStream1);

                                splineJsonHandler.Splines.Add(worldSpline.ToJSON());
                            }
                            else if (ID == 9)
                            {
                                Console.WriteLine(extractPath + "//Textures//" + Path + ".png");
                                WorldSSH worldOldSSH = new WorldSSH();

                                worldOldSSH.Load(NewData);
                                //worldOldSSH.SaveImage(ExtractPath + "//Textures//" + Path + ".png");
                                if (!File.Exists(extractPath + "//Textures//" + RID + ".png"))
                                {
                                    worldOldSSH.SaveImage(extractPath + "//Textures//" + RID + ".png");
                                }

                            }
                            else if (ID == 10)
                            {
                                Console.WriteLine(extractPath + "//Lightmaps//" + Path + ".png");
                                WorldOldSSH worldOldSSH = new WorldOldSSH();

                                worldOldSSH.Load(NewData);
                                //worldOldSSH.SaveImage(ExtractPath + "//Lightmaps//" + Path + ".png");

                                if (!File.Exists(extractPath + "//Lightmaps//" + RID.ToString().PadLeft(4, '0') + ".png"))
                                {
                                    worldOldSSH.SaveImage(extractPath + "//Lightmaps//" + RID.ToString().PadLeft(4, '0') + ".png");
                                }
                            }
                            else if (ID == 11)
                            {
                                WorldVisCurtain worldBin11 = new WorldVisCurtain();
                                memoryStream1.Position = 0;
                                worldBin11.LoadData(memoryStream1);

                                visCurtainJsonHandler.VisCurtains.Add(worldBin11.ToJSON());
                            }
                            else if (ID == 14)
                            {
                                Console.WriteLine(ExtractPath + "AIP.json");
                                WorldAIP worldAIP = new WorldAIP();
                                worldAIP.LoadData(NewData);
                                worldAIP.ToJson(ExtractPath + "AIP.json");
                            }
                            else if (ID == 14)
                            {
                                Console.WriteLine(ExtractPath + "Bin18.json");
                                WorldBin18 worldBin18 = new WorldBin18();
                                worldBin18.LoadData(stream);

                                worldBin18.ToJson(ExtractPath + "Bin18.json");
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

                            Console.WriteLine(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Bin5.json");
                            bin5JsonHandler.CreateJson(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Bin5.json");

                            Console.WriteLine(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Bin6.json");
                            bin6JsonHandler.CreateJson(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Bin6.json");

                            Console.WriteLine(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Bin11.json");
                            visCurtainJsonHandler.CreateJson(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//VisCurtain.json");

                            Console.WriteLine(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Splines.json");
                            splineJsonHandler.CreateJson(extractPath + "//" + sdbHandler.locations[ChunkID].Name + "//Splines.json");

                            patchesJsonHandler = new PatchesJsonHandler();
                            bin0JsonHandler = new Bin0JsonHandler();
                            bin3JsonHandler = new Bin3JsonHandler();
                            bin5JsonHandler = new Bin5JsonHandler();
                            bin6JsonHandler = new Bin6JsonHandler();
                            visCurtainJsonHandler = new VisCurtainJsonHandler();
                            splineJsonHandler = new SplineJsonHandler();
                        }
                        a++;
                        memoryStream = new MemoryStream();
                    }
                }
            }
            SSX3Config ssx3Config = new SSX3Config();
            ssx3Config.CreateJson(extractPath + "//ConfigSSX3.ssx");
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
