using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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

        public void ExtractLoad(string path)
        {
            Load(path);
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                for (int i = 0; i < Files.Count; i++)
                {
                    stream.Position = Files[i].Offset * Aligment;

                    
                }
            }
        }

        public struct FileIndex
        {
            public int Offset;
            public int zSize; //Unused?
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



        int hash(char[] str)
        {
            int hash = 5381;

            for (int i = 0; i < str.Length; i++)
            {
                hash = ((hash << 5) + hash) + str[i]; /* hash * 33 + c */
            }
            return hash;
        }
    }
}
