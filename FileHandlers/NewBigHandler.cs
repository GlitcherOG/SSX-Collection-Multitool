using System;
using System.Collections.Generic;
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
        public long FileSize;

        struct FileIndex
        {
            public int Offset;
            public int zSize; //Unused?
            public int Size;
            public int Hash;

            public int PathIndex;
            public string FileName;
            public string Path;
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





        //4 - header
        //4 - count
        //4 - unknown
        //4 - path offset
        //4 - data start from path offset
        //2 - unknown
        //2 - folder path count
        //4 - unknown
        //4 - size
        //16 - unknown

        //4 - unknown
        //4 - unknown
        //4 - size?
        //4 - unknown
    }
}
