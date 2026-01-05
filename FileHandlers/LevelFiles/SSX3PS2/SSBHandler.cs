using System.IO;
using SSXMultiTool.Utilities;
using SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2.SSBData;
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
        public void LoadAndExtractSSBFromSBD(string path, string extractPath)
        {
            ConsoleWindow.GenerateConsole();

            SDBHandler sdbHandler = new SDBHandler();
            sdbHandler.LoadSBD(path.Replace(".ssb", ".sdb"));

            PHMHandler phmHandler = new PHMHandler();
            phmHandler.LoadPHM(path.Replace(".ssb", ".phm"));

            PSMHandler psmHandler = new PSMHandler();
            psmHandler.LoadPSM(path.Replace(".ssb", ".psm"));

            using (Stream stream = File.Open(path, FileMode.Open))
            {
                PatchesJsonHandler patchesJsonHandler = new PatchesJsonHandler();
                Bin0JsonHandler bin0JsonHandler = new Bin0JsonHandler();
                InstanceJsonHandler bin3JsonHandler = new InstanceJsonHandler();
                Bin5JsonHandler bin5JsonHandler = new Bin5JsonHandler();
                Bin6JsonHandler bin6JsonHandler = new Bin6JsonHandler();
                SplineJsonHandler splineJsonHandler = new SplineJsonHandler();
                VisCurtainJsonHandler visCurtainJsonHandler = new VisCurtainJsonHandler();
                MDRJsonHandler mdrJsonHandler = new MDRJsonHandler();

                MemoryStream memoryStream = new MemoryStream();
                List<int> ints = new List<int>();
                int a = 0;
                int splitCount = 1;
                int FilePos = 0;
                int ChunkID = -1;
                Directory.CreateDirectory(extractPath + "//Textures");
                Directory.CreateDirectory(extractPath + "//Lightmaps");
                Directory.CreateDirectory(extractPath + "//Levels");
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
                        Directory.CreateDirectory(extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name);
                        Directory.CreateDirectory(extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name + "//Models");
                        Directory.CreateDirectory(extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name + "//Sounds");
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
                            memoryStream1.Position = 0;

                            string LevelExtractPath = extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name + "//";
                            string Path = RID + "-" + TrackID + "-" + FilePos;

                            if (ID == 0)
                            {
                                WorldBin0 worldbin0 = new WorldBin0();
                                worldbin0.LoadData(memoryStream1, TrackID, RID);

                                bin0JsonHandler.bin0Files.Add(worldbin0.ToJSON());
                            }
                            else if (ID == 1)
                            {
                                WorldPatch worldPatch = new WorldPatch();
                                worldPatch.LoadPatch(memoryStream1);

                                worldPatch.Name = phmHandler.GetName(TrackID, RID, 0, psmHandler);

                                patchesJsonHandler.Patches.Add(worldPatch.ToJSON());
                            }
                            else if (ID == 2)
                            {
                                WorldMDR worldMDR = new WorldMDR();

                                worldMDR.LoadData(memoryStream1);

                                worldMDR.Name = phmHandler.GetName(TrackID, RID, 2, psmHandler);

                                Console.WriteLine(LevelExtractPath + "//Models//" + RID + ".obj");
                                mdrJsonHandler.mainModelHeaders.Add(worldMDR.SaveModel(LevelExtractPath + "//Models//"));
                                //worldMDR.SaveModelGLB(ExtractPath + "//Models//" + RID + ".glb");
                            }
                            else if (ID == 3)
                            {
                                WorldInstance worldBin3 = new WorldInstance();
                                worldBin3.LoadData(memoryStream1);

                                worldBin3.Name = phmHandler.GetName(TrackID, RID, 1, psmHandler);

                                bin3JsonHandler.Instances.Add(worldBin3.ToJSON());
                            }
                            else if (ID == 5)
                            {
                                WorldBin5 worldBin5 = new WorldBin5();
                                worldBin5.LoadData(memoryStream1);

                                bin5JsonHandler.bin5Files.Add(worldBin5.ToJSON());
                            }
                            else if (ID == 6)
                            {
                                WorldBin6 worldBin6 = new WorldBin6();
                                worldBin6.LoadData(memoryStream1);

                                bin6JsonHandler.bin6Files.Add(worldBin6.ToJSON());
                            }
                            else if (ID == 8)
                            {
                                WorldSpline worldSpline = new WorldSpline();
                                worldSpline.LoadData(memoryStream1);

                                worldSpline.Name = phmHandler.GetName(TrackID, RID, 3, psmHandler);

                                splineJsonHandler.Splines.Add(worldSpline.ToJSON());
                            }
                            else if (ID == 9)
                            {
                                if (!File.Exists(extractPath + "//Textures//" + RID + ".png"))
                                {
                                    Console.WriteLine(extractPath + "//Textures//" + Path + ".png");
                                    WorldSSH worldOldSSH = new WorldSSH();

                                    worldOldSSH.Load(NewData);

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
                                worldBin11.LoadData(memoryStream1);

                                visCurtainJsonHandler.VisCurtains.Add(worldBin11.ToJSON());
                            }
                            else if (ID == 14)
                            {
                                WorldAIP worldAIP = new WorldAIP();
                                worldAIP.LoadData(NewData);

                                string FileName = RID + "AIP";

                                if(RID==0)
                                {
                                    FileName = "AIP";
                                }
                                else if(RID==1)
                                {
                                    FileName = "PeakRaceAIP";
                                }
                                else if (RID==2)
                                {
                                    FileName = "PeakShowOffAIP";
                                }

                                Console.WriteLine(LevelExtractPath + FileName + ".json");
                                worldAIP.ToJson(LevelExtractPath + FileName + ".json");
                            }
                            else if (ID == 18)
                            {
                                Console.WriteLine(LevelExtractPath + "Bin18.json");
                                WorldBin18 worldBin18 = new WorldBin18();
                                worldBin18.LoadData(memoryStream1);

                                worldBin18.ToJson(LevelExtractPath + "Bin18.json");
                            }
                            else if (ID == 20)
                            {
                                Console.WriteLine(LevelExtractPath + "//Sounds//" + Path + ".bnk");
                                var file = File.Create(LevelExtractPath + "//Sounds//" + Path + ".bnk");
                                memoryStream1.CopyTo(file);
                                memoryStream1.Dispose();
                                memoryStream1 = new MemoryStream();
                                file.Close();
                            }
                            //else
                            //{
                            //    Console.WriteLine(LevelExtractPath + Path + ".bin" + ID);
                            //    var file = File.Create(LevelExtractPath + Path + ".bin" + ID);
                            //    memoryStream1.CopyTo(file);
                            //    memoryStream1.Dispose();
                            //    memoryStream1 = new MemoryStream();
                            //    file.Close();
                            //}

                            FilePos++;
                        }
                        int TempChunkID = sdbHandler.FindLocationChunk(a+1);
                        if (TempChunkID != ChunkID || stream.Position >= stream.Length - 1)
                        {
                            Console.WriteLine(extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name + "//Patches.json");
                            patchesJsonHandler.CreateJson(extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name + "//Patches.json");

                            Console.WriteLine(extractPath + "//Levels///" + sdbHandler.locations[ChunkID].Name + "//Bin0.json");
                            bin0JsonHandler.CreateJson(extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name + "//Bin0.json");

                            Console.WriteLine(extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name + "//Instances.json");
                            bin3JsonHandler.CreateJson(extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name + "//Instances.json");

                            Console.WriteLine(extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name + "//Bin5.json");
                            bin5JsonHandler.CreateJson(extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name + "//Bin5.json");

                            Console.WriteLine(extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name + "//Bin6.json");
                            bin6JsonHandler.CreateJson(extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name + "//Bin6.json");

                            Console.WriteLine(extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name + "//Bin11.json");
                            visCurtainJsonHandler.CreateJson(extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name + "//VisCurtain.json");

                            Console.WriteLine(extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name + "//Splines.json");
                            splineJsonHandler.CreateJson(extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name + "//Splines.json");

                            Console.WriteLine(extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name + "//Prefabs.json");
                            mdrJsonHandler.CreateJson(extractPath + "//Levels//" + sdbHandler.locations[ChunkID].Name + "//Prefabs.json");

                            patchesJsonHandler = new PatchesJsonHandler();
                            bin0JsonHandler = new Bin0JsonHandler();
                            bin3JsonHandler = new InstanceJsonHandler();
                            bin5JsonHandler = new Bin5JsonHandler();
                            bin6JsonHandler = new Bin6JsonHandler();
                            visCurtainJsonHandler = new VisCurtainJsonHandler();
                            splineJsonHandler = new SplineJsonHandler();
                            mdrJsonHandler = new MDRJsonHandler();
                        }
                        a++;
                        memoryStream = new MemoryStream();
                    }
                }
            }

            //Save Info Config File
            LevelJsonHandler levelJsonHandler = new LevelJsonHandler();
            levelJsonHandler.LevelInfoList = new List<LevelJsonHandler.LevelInfo>();
            for (int i = 0; i < sdbHandler.locations.Count; i++)
            {
                var NewLevelInfo = new LevelJsonHandler.LevelInfo();

                NewLevelInfo.TrackID = i;
                NewLevelInfo.LevelName = sdbHandler.locations[i].Name;

                levelJsonHandler.LevelInfoList.Add(NewLevelInfo);
            }
            levelJsonHandler.CreateJson(extractPath + "//Levels.json");

            SSX3Config ssx3Config = new SSX3Config();
            ssx3Config.CreateJson(extractPath + "//ConfigSSX3.ssx");
            ConsoleWindow.CloseConsole();
        }

        //public void PackSSB(string Folder, string BuildPath)
        //{
        //    MemoryStream memoryStream = new MemoryStream();
        //    string[] AllFiles = Directory.GetFiles(Folder, "*.BSX");
        //    for (int i = 0; i < AllFiles.Length; i++)
        //    {
        //        using (Stream stream = File.Open(Folder +"//"+ i.ToString()+".BSX", FileMode.Open))
        //        {
        //            byte[] bytes = new byte[1];
        //            while (true)
        //            {
        //                byte[] output = new byte[32768];
        //                bool End = false;
        //                int ReadLength = 40000;
        //                if (ReadLength+stream.Position>stream.Length)
        //                {
        //                    ReadLength = (int)(stream.Length - stream.Position);
        //                    End = true;
        //                }
        //                long StartPos = stream.Position;
        //                bool Start = true;
        //                while(output.Length> 32768-8)
        //                {
        //                    if (!Start)
        //                    {
        //                        stream.Position = StartPos;
        //                        ReadLength -= 32768 / 4;
        //                        End = false;
        //                    }
        //                    bytes = StreamUtil.ReadBytes(stream, ReadLength);
        //                    RefpackHandler.Compress(bytes, out output, CompressionLevel.Max);
        //                    Start = false;
        //                }
                        
                        
        //                if(!End)
        //                {
        //                    StreamUtil.WriteString(memoryStream,"CBSX");
        //                }
        //                else
        //                {
        //                    StreamUtil.WriteString(memoryStream, "CEND");
        //                }

        //                StreamUtil.WriteInt32(memoryStream, 32768);

        //                StreamUtil.WriteBytes(memoryStream, output);

        //                StreamUtil.AlignBy(memoryStream, 32768);

        //                if(End)
        //                {
        //                    break;
        //                }

        //            }
        //        }
        //    }
        //    if (File.Exists(BuildPath))
        //    {
        //        File.Delete(BuildPath);
        //    }
        //    var file = File.Create(BuildPath);
        //    memoryStream.Position = 0;
        //    memoryStream.CopyTo(file);
        //    memoryStream.Dispose();
        //    file.Close();
        //    GC.Collect();
        //}
    }
}
