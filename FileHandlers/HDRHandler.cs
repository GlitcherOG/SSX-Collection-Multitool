using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers
{
    internal class HDRHandler
    {
        public int U1;
        int U2;
        int U3;
        int FileCount;
        int PaddingCount;
        int U5;
        int U6;

        List<FileHeader> fileHeaders = new List<FileHeader>();
        List<int> Padding = new List<int>();
        public void Load(string Path)
        {
            using (Stream stream = File.Open(Path, FileMode.Open))
            {
                U1 = StreamUtil.ReadInt16(stream);
                U2 = StreamUtil.ReadInt16(stream);
                U3 = StreamUtil.ReadInt8(stream);
                FileCount = StreamUtil.ReadInt8(stream);
                PaddingCount = StreamUtil.ReadInt16(stream);
                U5 = StreamUtil.ReadInt24(stream);


                fileHeaders = new List<FileHeader>();
                for (int i = 0; i < FileCount; i++)
                {
                    var TempHeader = new FileHeader();
                    if(U3==3)
                    {
                        TempHeader.Unknown1 = StreamUtil.ReadInt8(stream);
                        TempHeader.Offset = StreamUtil.ReadInt16Big(stream);
                        TempHeader.Unknown2 = StreamUtil.ReadInt16Big(stream); //Sample Rate?
                    }
                    fileHeaders.Add(TempHeader);
                }
                U6 = StreamUtil.ReadInt24(stream);

                Padding = new List<int>();
                for (int i = 0; i < PaddingCount; i++)
                {
                    Padding.Add(StreamUtil.ReadInt8(stream));
                }

            }
        }



        struct FileHeader
        {
            public int Unknown1;
            public int Offset;
            public int Unknown2;
            public int Unknown3;
        }
    }
}
