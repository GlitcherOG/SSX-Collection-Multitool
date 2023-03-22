﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2
{
    internal class SSBHandler
    {
        /*
            
        All IDS
        0 - Unknown
        2 - WorldMDR (Def Contains Model Info)
        3 - Unknown
        4 - Unknown
        5 - 
        6 -
        7 -
        8 -
        9 - Shape
        10 - 
        11 - 
        12 - 
        13 - 
        14 - 
        15 - 
        16 - 
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
                while(true)
                {
                    if (stream.Position >= stream.Length - 1)
                    {
                        break;
                    }
                    string MagicWords = StreamUtil.ReadString(stream, 4);

                    int Size = StreamUtil.ReadUInt32(stream);
                    byte[] Data = new byte[Size-8];
                    byte[] DecompressedData = new byte[1];
                    Data = StreamUtil.ReadBytes(stream, Size-8);

                    DecompressedData= RefpackHandler.Decompress(Data);
                    StreamUtil.WriteBytes(memoryStream, DecompressedData);


                    if (MagicWords.ToUpper() == "CEND")
                    {
                        int FilePos = 0;
                        memoryStream.Position = 0;
                        Directory.CreateDirectory(extractPath + "//" + a);
                        while (memoryStream.Position< memoryStream.Length)
                        {
                            MemoryStream memoryStream1 = new MemoryStream();
                            int ID = StreamUtil.ReadUInt8(memoryStream);
                            int ChunkSize = StreamUtil.ReadInt24(memoryStream);
                            int RID = StreamUtil.ReadUInt32(memoryStream);

                            byte[] NewData= StreamUtil.ReadBytes(memoryStream, ChunkSize);
                            StreamUtil.WriteBytes(memoryStream1, NewData);

                            var file = File.Create(extractPath + "//" + a + "//" + FilePos + "." + ID + "bin");
                            memoryStream1.Position = 0;
                            memoryStream1.CopyTo(file);
                            memoryStream1 = new MemoryStream();
                            memoryStream1.Dispose();
                            file.Close();
                            FilePos++;
                        }
                        memoryStream.Dispose();
                        memoryStream = new MemoryStream();
                        a++;
                    }
                }
            }
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
