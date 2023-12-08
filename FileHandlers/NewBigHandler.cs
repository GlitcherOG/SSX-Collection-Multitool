using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers
{
    public class NewBigHandler
    {
        public int Signature;
        public int HeaderVersion;
        public int FileCount;
        public int Flags;
        public int Aligment;
        public int Reserved;
        public int BaseHeaderSize;
        public int NameHeaderSize;
        public int NameLength;
        public int PathLength;
        public int NumPath;
        public ulong FileSize;

        public List<FileIndex> Files = new List<FileIndex>();
        public List<string> Paths = new List<string>();
        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                Signature = StreamUtil.ReadInt16(stream, true);
                HeaderVersion = StreamUtil.ReadInt16(stream, true);
                FileCount = StreamUtil.ReadUInt32(stream, true);
                Flags = StreamUtil.ReadInt16(stream, true);
                Aligment = StreamUtil.ReadUInt8(stream);
                Reserved = StreamUtil.ReadUInt8(stream);
                BaseHeaderSize = StreamUtil.ReadUInt32(stream, true);
                NameHeaderSize = StreamUtil.ReadUInt32(stream, true);
                NameLength = StreamUtil.ReadUInt8(stream);
                PathLength = StreamUtil.ReadUInt8(stream);
                NumPath = StreamUtil.ReadInt16(stream, true);
                FileSize = StreamUtil.ReadUInt64(stream, true);

                stream.Position += 16;

                Files = new List<FileIndex>();

                for (int i = 0; i < FileCount; i++)
                {
                    FileIndex fileIndex = new FileIndex();

                    fileIndex.Offset = StreamUtil.ReadUInt32(stream, true);
                    fileIndex.zSize = StreamUtil.ReadUInt32(stream, true);
                    fileIndex.Size = StreamUtil.ReadUInt32(stream, true);
                    fileIndex.Hash = StreamUtil.ReadUInt32(stream, true);

                    Files.Add(fileIndex);
                }

                stream.Position = BaseHeaderSize;

                for (int i = 0; i < Files.Count; i++)
                {
                    FileIndex fileIndex = Files[i];

                    fileIndex.PathIndex = StreamUtil.ReadUInt16(stream, true);
                    fileIndex.FileName = StreamUtil.ReadString(stream, NameLength-2);

                    Files[i] = fileIndex;
                }

                StreamUtil.AlignBy16(stream);

                Paths = new List<string>();
                for (int i = 0; i < NumPath; i++)
                {
                    Paths.Add(StreamUtil.ReadString(stream, PathLength));
                }
            }
        }

        public void ExtractLoad(string path, string extractpath)
        {
            Load(path);
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                for (int i = 0; i < Files.Count; i++)
                {
                    string FilePath = extractpath + "\\" + Paths[Files[i].PathIndex] + "\\" + Files[i].FileName;

                    if (!Directory.Exists(Path.GetDirectoryName(extractpath + "\\" + Paths[Files[i].PathIndex] + "\\")))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(extractpath + "\\" + Paths[Files[i].PathIndex] + "\\"));
                    }

                    if (File.Exists(FilePath))
                    {
                        File.Delete(FilePath);
                    }
                    var file = File.Create(FilePath);
                    file.Close();
                    using (Stream OutputStream = File.Open(FilePath, FileMode.Open))
                    {
                        stream.Position = Files[i].Offset * 16;

                        byte[] RefpackTest = StreamUtil.ReadBytes(stream,2);
                        stream.Position -= 2;
                        if (RefpackTest[0] == 0x10 && RefpackTest[1] == 0xFB)
                        {
                            //RefpackHandler.DecompressStream(stream, OutputStream);
                            StreamUtil.WriteBytes(OutputStream, RefpackHandler.Decompress(StreamUtil.ReadBytes(stream, Files[i].zSize)));

                        }
                        else
                        {
                            string ChunkedTest = StreamUtil.ReadString(stream, 8);

                            stream.Position -= 8;

                            if (ChunkedTest == "chunkzip")
                            {
                                ChunkZipHeader chunkZipHeader = new ChunkZipHeader();

                                chunkZipHeader.Header = StreamUtil.ReadString(stream, 8);
                                chunkZipHeader.VersionNumber = StreamUtil.ReadUInt32(stream, true);
                                chunkZipHeader.FullSize = StreamUtil.ReadUInt32(stream, true);

                                chunkZipHeader.BlockSize = StreamUtil.ReadUInt32(stream, true);
                                chunkZipHeader.NumSegments = StreamUtil.ReadUInt32(stream, true);
                                chunkZipHeader.Alignment = StreamUtil.ReadUInt32(stream, true);

                                for (int a = 0; a < chunkZipHeader.NumSegments; a++)
                                {
                                    MemoryStream InputStream = new MemoryStream();
                                    long OldPos = stream.Position;
                                    StreamUtil.AlignBy16(stream);
                                    long CurPos = stream.Position;

                                    if (CurPos - OldPos < 8)
                                    {
                                        stream.Position += 8;
                                    }
                                    else
                                    {
                                        stream.Position -= 8;
                                    }

                                    Chunk chunk = new Chunk();

                                    chunk.ChunkSize = StreamUtil.ReadUInt32(stream, true);
                                    chunk.CompressionType = StreamUtil.ReadUInt32(stream, true);

                                    StreamUtil.WriteStreamIntoStream(InputStream, stream, stream.Position, chunk.ChunkSize);
                                    var decompressor = new DeflateStream(InputStream, CompressionMode.Decompress);
                                    InputStream.Position = 0;
                                    decompressor.CopyTo(OutputStream);
                                    decompressor.Close();
                                    InputStream.Close();
                                }
                            }
                            else
                            {
                                StreamUtil.WriteStreamIntoStream(OutputStream, stream, stream.Position, Files[i].Size);
                            }
                        }
                    }
                    GC.Collect();
                }
            }
        }

        public void SaveFile(string SavePath, string LoadPath, bool Compressed)
        {
            Files = new List<FileIndex>();
            Paths = new List<string>();

            string FilePath = SavePath;

            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            var file = File.Create(FilePath);
            file.Close();
            using (Stream OutputStream = File.Open(FilePath, FileMode.Open))
            {
                string[] paths = Directory.GetFiles(LoadPath, "*.*", SearchOption.AllDirectories);

                //Make Space For Header
                OutputStream.Position = 16 * 3;

                OutputStream.Position += 16 * paths.Length;

                NameLength = 0;
                PathLength = 0;

                for (int i = 0; i < paths.Length; i++)
                {
                    //Write File or If Compressed

                    string DataPath = Path.GetFileName(paths[i]);
                    string DataFolder = Path.GetDirectoryName(paths[i]).Remove(0, LoadPath.Length + 1).Replace("//", @"\").Replace("\\", @"\");

                    if(NameLength+2< DataPath.Length+2)
                    {
                        NameLength = DataPath.Length+2;
                    }

                    if (PathLength+1 < DataFolder.Length+1)
                    {
                        PathLength = DataFolder.Length+1;
                    }

                    if (!Paths.Contains(DataFolder))
                    {
                        Paths.Add(DataFolder);
                    }

                    FileIndex fileIndex = new FileIndex();

                    fileIndex.FileName = DataPath;
                    fileIndex.PathIndex = Paths.IndexOf(DataFolder);
                    fileIndex.Hash = hash(paths[i].Remove(0, LoadPath.Length + 1).Replace("//", @"\").Replace("\\", @"\").ToArray());

                    Files.Add(fileIndex);
                }
                //Gap is file count alligned by 16
                OutputStream.Position += paths.Length;
                StreamUtil.AlignBy16(OutputStream);

                BaseHeaderSize = (int)OutputStream.Position;

                //Make Space for paths
                long TempNameLenght = OutputStream.Position;

                OutputStream.Position += (NameLength+2)*Files.Count + PathLength * Paths.Count;

                StreamUtil.AlignBy16(OutputStream);
                NameHeaderSize = (int)(OutputStream.Position - TempNameLenght);

                for (int i = 0; i < Files.Count; i++)
                {
                    var fileIndex = Files[i];

                    fileIndex.Offset = (int)(OutputStream.Position / 16);

                    if (Compressed)
                    {
                        OutputStream.Position += 16 * 3;
                        fileIndex.Compressed = true;
                        //Log Header Position
                        long FileLenght = 0;
                        int Chunks = 0;

                        using (Stream InputStream = File.Open(paths[i], FileMode.Open))
                        {
                            FileLenght = InputStream.Length;
                            while (InputStream.Position < InputStream.Length)
                            {
                                long OldPos = OutputStream.Position;
                                StreamUtil.AlignBy16(OutputStream);
                                long CurPos = OutputStream.Position;

                                if (CurPos - OldPos < 8)
                                {
                                    OutputStream.Position += 8;
                                }
                                else
                                {
                                    OutputStream.Position -= 8;
                                }
                                long ChunkHeaderPos = OutputStream.Position;
                                int ReadLenght = 131072;
                                if (InputStream.Position + ReadLenght >= InputStream.Length)
                                {
                                    ReadLenght = (int)(InputStream.Length - InputStream.Position);
                                }

                                using (MemoryStream TempStream = new MemoryStream())
                                {
                                    byte[] TempBytes = StreamUtil.ReadBytes(InputStream, ReadLenght);
                                    StreamUtil.WriteBytes(TempStream, TempBytes);
                                    var decompressor = new DeflateStream(OutputStream, CompressionMode.Compress);
                                    TempStream.Position = 0;
                                    TempStream.CopyTo(decompressor);
                                }
                                long TempPos1 = OutputStream.Position;
                                OutputStream.Position = ChunkHeaderPos;

                                StreamUtil.WriteInt32(OutputStream, (int)(TempPos1 - ChunkHeaderPos - 8), true);
                                StreamUtil.WriteInt32(OutputStream, 1, true);
                                OutputStream.Position = TempPos1;

                                Chunks++;
                            }
                            StreamUtil.AlignBy16(OutputStream);
                        }
                        long TempPos = OutputStream.Position;
                        OutputStream.Position = fileIndex.Offset * 16;

                        StreamUtil.WriteString(OutputStream, "chunkzip");
                        StreamUtil.WriteInt32(OutputStream, 3, true);
                        StreamUtil.WriteInt32(OutputStream, (int)FileLenght, true);
                        StreamUtil.WriteInt32(OutputStream, 131072, true);
                        StreamUtil.WriteInt32(OutputStream, Chunks, true);
                        StreamUtil.WriteInt32(OutputStream, 16, true);

                        OutputStream.Position = TempPos;

                    }
                    else
                    {
                        using (Stream InputStream = File.Open(paths[i], FileMode.Open))
                        {
                            fileIndex.Size = (int)InputStream.Length;
                            InputStream.CopyTo(OutputStream);
                        }
                        fileIndex.Size = (int)(OutputStream.Position - (fileIndex.Offset * 16));
                    }
                    StreamUtil.AlignBy16(OutputStream);

                    Files[i] = fileIndex;
                }



                OutputStream.Position = 0;
                StreamUtil.WriteInt16(OutputStream, 17730, true);
                StreamUtil.WriteInt16(OutputStream, 3, true);
                StreamUtil.WriteInt32(OutputStream, Files.Count, true);
                StreamUtil.WriteInt16(OutputStream, 16, true);
                StreamUtil.WriteUInt8(OutputStream, 4);
                StreamUtil.WriteUInt8(OutputStream, 0);
                StreamUtil.WriteInt32(OutputStream, BaseHeaderSize, true);
                StreamUtil.WriteInt32(OutputStream, NameHeaderSize, true);
                StreamUtil.WriteUInt8(OutputStream, NameLength+2);
                StreamUtil.WriteUInt8(OutputStream, PathLength);
                StreamUtil.WriteInt16(OutputStream, Paths.Count, true);
                StreamUtil.WriteInt64(OutputStream, OutputStream.Length, true);

                OutputStream.Position +=16;

                Files.Sort((s1, s2) => ((uint)s1.Hash).CompareTo((uint)s2.Hash));
                for (int i = 0; i < Files.Count; i++)
                {
                    StreamUtil.WriteInt32(OutputStream, Files[i].Offset, true);
                    StreamUtil.WriteInt32(OutputStream, Files[i].zSize, true);
                    StreamUtil.WriteInt32(OutputStream, Files[i].Size, true);
                    StreamUtil.WriteInt32(OutputStream, Files[i].Hash, true);
                }

                //Gap is file count alligned by 16
                OutputStream.Position += paths.Length;
                StreamUtil.AlignBy16(OutputStream);

                for (int i = 0; i < Files.Count; i++)
                {
                    StreamUtil.WriteInt16(OutputStream, Files[i].PathIndex, true);
                    StreamUtil.WriteString(OutputStream, Files[i].FileName, NameLength);
                }

                for (int i = 0; i < Paths.Count; i++)
                {
                    StreamUtil.WriteString(OutputStream, Paths[i], PathLength);
                }

                //FIX Size bytes
                //FIX HASH GENERATION
            }
        }

        public struct BIGHeader
        {
            public string MagicWords;
            public int FileSize;
            public int FileCount;
            public int BaseHeaderSize;
            public byte[] Footer;
        }

        public enum BigType
        {
            BIGF,
            C0FB,
            BIG4
        }

        public struct NewBigHeader
        {
            public int Signature;
            public int HeaderVersion;
            public int FileCount;
            public int Flags;
            public int Aligment;
            public int Reserved;
            public int BaseHeaderSize;
            public int NameHeaderSize;
            public int NameLength;
            public int PathLength;
            public int NumPath;
            public ulong FileSize;

            public List<FileIndex> Files;
            public List<string> Paths;
        }

        public struct FileIndex
        {
            public int Offset;
            public int zSize; //Uncompressed only used for refpack
            public int Size;
            public int Hash;

            public bool Compressed;

            public int PathIndex;
            public string FileName;
        }

        //struct BigFileFormatHeaderV2
        //{
        //    uint16_t signature;         // "EB" (Electronic Arts Bigfile)
        //    uint16_t headerVersion;     // version number
        //    uint32_t fileCount;         // number of files in this archive
        //    uint16_t flags;             // bitflags
        //    uint8_t alignment;          // power of two on which the files are aligned (default = 4, 16 bytes)
        //    uint8_t _reserved0;         // not used
        //    uint32_t baseHeaderSize;    // file header + hash index
        //    uint32_t nameHeaderSize;    // size of the names & paths, in bytes
        //    uint8_t nameLength;         // length of each name entry
        //    uint8_t pathLength;         // length of each path entry
        //    uint16_t numPaths;          // total number of unique paths
        //    uint64_t fileSize;          // total size of the bigfile
        //};

        public struct ChunkZipHeader
        {
            public string Header;
            public int VersionNumber;
            public int FullSize;

            public int BlockSize;
            public int NumSegments; 
            public int Alignment;
        }

        public struct Chunk
        {
            public int ChunkSize;
            public int CompressionType;
        }


        int hash(string str) 
        {
            char[] Array = str.ToCharArray();

            int hash = 5381;

            for (int i = 0; i < Array.Length; i++)
            {
                hash = ((hash << 5) + hash) + (int)Array[i]; /* hash * 33 + Array[i] */
            }
            return hash;
        }

        public static UInt32 bxStringHash(string stringInput)
        {
            char[] input = stringInput.ToCharArray();

            UInt32 hash = 0;
            UInt32 high = 0;

            foreach (var item in input)
            {
                hash = (hash << 4) + item;
                high = hash & 0xf0000000;
                if (high > 0)
                    hash ^= high >> 23;

                hash &= ~high;
            }

            return hash;
        }
    }
}
