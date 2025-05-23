﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers
{
    class BigHandler
    {
        public BigType bigType;
        public bool CompressBuild;
        public BIGFHeader bigHeader;
        public List<BIGFFiles> bigFiles;
        public string bigPath;
        //bool BuildMode;
        public void LoadBig(string path)
        {
            //BuildMode = false;
            bigPath = path;
            bigHeader = new BIGFHeader();
            bigFiles = new List<BIGFFiles>();
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                bigHeader.MagicWords = StreamUtil.ReadString(stream, 4);
                //Figure out what any of these mean
                if (bigHeader.MagicWords == "BIGF")
                {
                    bigType = BigType.BIGF;
                    ReadBigF(stream);
                }
                else if(bigHeader.MagicWords == "BIG4")
                {
                    bigType = BigType.BIG4;
                    ReadBigF(stream);
                }
                else
                {
                    stream.Position = 0;
                    byte[] bytes = new byte[2];
                    stream.Read(bytes, 0, 2);
                    if (bytes[1] == 0xFB)
                    {
                        ReadBigC0FB(stream);
                    }
                    else
                    {
                        MessageBox.Show(bigHeader.MagicWords + " Unknown Big Format");
                    }
                }
                stream.Dispose();
            }
        }

        public void ReadBigF(Stream stream)
        {

            bigHeader.fileSize = StreamUtil.ReadUInt32(stream);

            bigHeader.fileCount = StreamUtil.ReadUInt32(stream, true);

            bigHeader.startOffset = StreamUtil.ReadUInt32(stream, true);

            for (int i = 0; i < bigHeader.fileCount; i++)
            {
                BIGFFiles temp = new BIGFFiles();

                temp.offset = StreamUtil.ReadUInt32(stream, true);

                temp.size = StreamUtil.ReadUInt32(stream, true);

                temp.path = StreamUtil.ReadNullEndString(stream);
                bigFiles.Add(temp);
                stream.Position += 1;
            }

            bigHeader.footer = new byte[8];
            stream.Read(bigHeader.footer, 0, bigHeader.footer.Length);

            for (int i = 0; i < bigHeader.fileCount; i++)
            {
                stream.Position = bigFiles[i].offset;
                BIGFFiles tempFile = bigFiles[i];
                byte[] bytes = new byte[2];
                stream.Read(bytes, 0, bytes.Length);
                if (bytes[1] == 0xFB && bytes[0] == 0x10)
                {
                    tempFile.Compressed = true;
                    tempFile.UncompressedSize = StreamUtil.ReadInt24(stream, true);
                    bigFiles[i] = tempFile;
                }
            }

        }

        public void ReadBigC0FB(Stream stream)
        {
            bigType = BigType.C0FB;

            bigHeader.startOffset = StreamUtil.ReadInt16(stream, true);

            bigHeader.fileCount = StreamUtil.ReadInt16(stream, true);

            for (int i = 0; i < bigHeader.fileCount; i++)
            {
                BIGFFiles temp = new BIGFFiles();

                temp.offset = StreamUtil.ReadInt24(stream, true);

                temp.size = StreamUtil.ReadInt24(stream, true);

                temp.path = StreamUtil.ReadNullEndString(stream);
                bigFiles.Add(temp);
                stream.Position += 1;
            }

            for (int i = 0; i < bigHeader.fileCount; i++)
            {
                stream.Position = bigFiles[i].offset;
                BIGFFiles tempFile = bigFiles[i];
                byte[] bytes = new byte[2];
                stream.Read(bytes, 0, bytes.Length);
                if (bytes[1] == 0xFB)
                {
                    tempFile.Compressed = true;
                    tempFile.UncompressedSize = StreamUtil.ReadInt24(stream, true);
                    bigFiles[i] = tempFile;
                }
            }
        }

        public void ExtractBig(string path)
        {
            using (Stream stream = File.Open(bigPath, FileMode.Open))
            {
                for (int i = 0; i < bigFiles.Count; i++)
                {
                    if (!bigFiles[i].path.Contains("*"))
                    {
                        Stream stream1 = new MemoryStream();
                        byte[] temp = new byte[bigFiles[i].size];
                        stream.Position = bigFiles[i].offset;
                        stream.Read(temp, 0, temp.Length);
                        if (bigFiles[i].Compressed)
                        {
                            temp = RefpackHandler.Decompress(temp);
                        }
                        stream1.Write(temp, 0, temp.Length);

                        string WritePath = path + "//" + bigFiles[i].path;

                        WritePath = WritePath.Replace("../", "..");

                        if (!Directory.Exists(Path.GetDirectoryName(WritePath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(WritePath));
                        }
                        var file = File.Create(WritePath);
                        stream1.Position = 0;
                        stream1.CopyTo(file);
                        file.Close();
                        stream1.Dispose();
                    }
                }
                stream.Dispose();
            }
        }

        public void LoadFolder(string path)
        {
            //BuildMode = true;
            bigType = BigType.BIGF;
            bigPath = path;
            bigHeader = new BIGFHeader();
            bigFiles = new List<BIGFFiles>();
            string[] paths = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            bigHeader.fileCount = paths.Length;
            int FileOffset = 16;
            for (int i = 0; i < paths.Length; i++)
            {
                BIGFFiles tempFile = new BIGFFiles();
                tempFile.path = paths[i].Remove(0, path.Length+1).Replace("//",@"/").Replace("\\", @"/");
                FileOffset += tempFile.path.Length + 9;
                Stream stream = File.OpenRead(paths[i]);
                if (CompressBuild)
                {
                    tempFile.Compressed = true;
                    byte[] CompressedInput = new byte[stream.Length];
                    byte[] CompressedOutput = new byte[1];
                    stream.Read(CompressedInput, 0, CompressedInput.Length);
                    RefpackHandler.Compress(CompressedInput, out CompressedOutput, CompressionLevel.Max);
                    tempFile.size = CompressedOutput.Length;
                    tempFile.UncompressedSize = (int)stream.Length;
                }
                else
                {
                    tempFile.size = (int)stream.Length;
                }
                stream.Dispose();
                bigFiles.Add(tempFile);
            }
            FileOffset += 8;
            for (int i = 0; i < bigFiles.Count; i++)
            {
                BIGFFiles tempFile1 = bigFiles[i];
                tempFile1.offset = FileOffset;
                FileOffset += bigFiles[i].size;
                bigFiles[i] = tempFile1;
            }
        }

        public void LoadFolderC0FB(string path)
        {
            bigHeader = new BIGFHeader();
            bigType = BigType.C0FB;
            bigPath = path;
            bigFiles = new List<BIGFFiles>();
            string[] paths = Directory.GetFiles(bigPath, "*.*", SearchOption.AllDirectories);
            bigHeader.fileCount = paths.Length;
            int FileOffset = 6;
            for (int i = 0; i < paths.Length; i++)
            {
                BIGFFiles tempFile = new BIGFFiles();
                tempFile.path = paths[i].Remove(0, bigPath.Length + 1).Replace("//", @"/").Replace("\\", @"/");
                FileOffset += tempFile.path.Length + 7;
                Stream stream1 = File.OpenRead(paths[i]);
                if (CompressBuild)
                {
                    tempFile.Compressed = true;
                    byte[] CompressedInput = new byte[stream1.Length];
                    byte[] CompressedOutput = new byte[1];
                    stream1.Read(CompressedInput, 0, CompressedInput.Length);
                    RefpackHandler.Compress(CompressedInput, out CompressedOutput, CompressionLevel.Max);
                    tempFile.size = CompressedOutput.Length;
                    tempFile.UncompressedSize = (int)stream1.Length;
                }
                else
                {
                    tempFile.size = (int)stream1.Length;
                }
                stream1.Dispose();
                stream1.Close();
                bigFiles.Add(tempFile);
            }
            //FileOffset += 6;
            for (int i = 0; i < bigFiles.Count; i++)
            {
                BIGFFiles tempFile1 = bigFiles[i];
                tempFile1.offset = FileOffset;
                FileOffset += bigFiles[i].size;
                bigFiles[i] = tempFile1;
            }
        }

        public void BuildBig(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                var file = File.Create(path);
                Stream stream = file;

                if (bigType == BigType.BIGF || bigType == BigType.BIG4)
                {
                    var temp = bigType;
                    LoadFolder(bigPath);
                    if (temp == BigType.BIG4)
                    {
                        bigType = BigType.BIG4;
                    }
                    BuildBigF(stream);
                }
                else if (bigType == BigType.C0FB)
                {
                    LoadFolderC0FB(bigPath);
                    BuildBigC0FB(stream);
                }
                else
                {
                    MessageBox.Show("Unkown format");
                }

                stream.Dispose();
                stream.Close();
                file.Close();
                GC.Collect();
            }
            catch
            {
                MessageBox.Show("Unable to Build Big File");
            }
        }

        public void BuildBigF(Stream stream)
        {
            bigHeader.MagicWords = bigType.ToString();
            byte[] tempByte = new byte[4];
            StreamUtil.WriteString(stream, bigHeader.MagicWords);

            // Set File Size Later
            tempByte = new byte[4];
            stream.Write(tempByte, 0, tempByte.Length);

            //Set Ammount
            StreamUtil.WriteInt32(stream, bigHeader.fileCount, true);

            //Set Blank Start of file offset
            tempByte = new byte[4];
            stream.Write(tempByte, 0, tempByte.Length);

            for (int i = 0; i < bigFiles.Count; i++)
            {
                //Write offset
                StreamUtil.WriteInt32(stream, bigFiles[i].offset, true);

                //Write size
                StreamUtil.WriteInt32(stream, bigFiles[i].size, true);


                //Write Path
                StreamUtil.WriteNullString(stream, bigFiles[i].path.Replace("..", "../"));
            }

            //Write Footer
            tempByte = new byte[8];
            Encoding.ASCII.GetBytes("L222").CopyTo(tempByte, 0);
            stream.Write(tempByte, 0, tempByte.Length);

            //Set File start offset
            long pos = stream.Position;
            stream.Position = 12;

            StreamUtil.WriteInt32(stream, (int)pos, true);

            stream.Position = stream.Length;

            //Write Files
            for (int i = 0; i < bigFiles.Count; i++)
            {
                using (Stream stream1 = File.Open(bigPath + "\\" + bigFiles[i].path, FileMode.Open))
                {
                    if (!CompressBuild)
                    {
                        tempByte = new byte[stream1.Length];
                        stream1.Read(tempByte, 0, tempByte.Length);
                        stream.Write(tempByte, 0, tempByte.Length);
                    }
                    else
                    {
                        tempByte = new byte[stream1.Length];
                        stream1.Read(tempByte, 0, tempByte.Length);
                        byte[] Output = new byte[stream1.Length];
                        RefpackHandler.Compress(tempByte, out Output, CompressionLevel.Max);
                        stream.Write(Output, 0, Output.Length);
                    }
                }
                GC.Collect();
            }

            //Set filesize
            stream.Position = 4;

            StreamUtil.WriteInt32(stream, (int)stream.Length, true);
        }

        public void BuildBigC0FB(Stream stream)
        {
            byte[] tempByte = new byte[2] {0xC0,0xFB};
            stream.Write(tempByte, 0, tempByte.Length);

            //Set Blank Start of file offset
            tempByte = new byte[2];
            stream.Write(tempByte, 0, tempByte.Length);

            //Set Ammount
            StreamUtil.WriteInt16(stream, bigHeader.fileCount, true);

            for (int i = 0; i < bigFiles.Count; i++)
            {
                //Write offset
                StreamUtil.WriteInt24(stream, bigFiles[i].offset, true);

                //Write size
                StreamUtil.WriteInt24(stream, bigFiles[i].size, true);

                //Write Path
                StreamUtil.WriteNullString(stream, bigFiles[i].path.Replace("..", "../").Replace("\\","/"));
            }

            //Set File start offset
            long pos = stream.Position;
            stream.Position = 2;

            StreamUtil.WriteInt16(stream, (int)pos , true);

            stream.Position = stream.Length;

            //Write Files
            for (int i = 0; i < bigFiles.Count; i++)
            {
                using (Stream stream1 = File.Open(bigPath + "\\" + bigFiles[i].path, FileMode.Open))
                {
                    if (!CompressBuild)
                    {
                        tempByte = new byte[stream1.Length];
                        stream1.Read(tempByte, 0, tempByte.Length);
                        stream.Write(tempByte, 0, tempByte.Length);
                    }
                    else
                    {
                        tempByte = new byte[stream1.Length];
                        stream1.Read(tempByte, 0, tempByte.Length);
                        byte[] Output = new byte[stream1.Length];
                        RefpackHandler.Compress(tempByte, out Output, CompressionLevel.Max);
                        stream.Write(Output, 0, Output.Length);
                    }
                    GC.Collect();
                }
            }

        }

        public struct BIGFHeader
        {
            public string MagicWords;
            public int fileSize;
            public int fileCount;
            public int startOffset;
            public byte[] footer;
        }

        public struct BIGFFiles
        {
            public string path;
            public bool Compressed;
            public int size;
            public int offset;
            public int UncompressedSize;
        }

        public enum BigType
        {
            BIGF,
            C0FB,
            BIG4
        }
    }

//00-03 - Magic Word
//04-07 - file size (little endian)
//08-0b - Ammount
//0c-0f - Start of file Offset
//Start of file listings
//10-13 - offset
//14-17 - Size
//17 - file path
//80 ish blank bytes after each file (Done to make the file an even number?)
}
