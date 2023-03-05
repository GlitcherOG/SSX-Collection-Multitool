using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers
{
    public class HDRHandler
    {
        public int U1;
        public int U2;
        public int OffsetBytes;
        public int FileCount;
        public int PaddingCount;
        public int U4;
        public int U5;
        public int U6;

        List<FileHeader> fileHeaders = new List<FileHeader>();
        List<int> Padding = new List<int>();
        public void Load(string Path)
        {
            using (Stream stream = File.Open(Path, FileMode.Open))
            {
                U1 = StreamUtil.ReadInt16(stream);
                U2 = StreamUtil.ReadInt16(stream); //Always -1
                OffsetBytes = StreamUtil.ReadUInt8(stream);
                FileCount = StreamUtil.ReadUInt8(stream);
                PaddingCount = StreamUtil.ReadUInt8(stream);
                U4 = StreamUtil.ReadUInt8(stream); //Multi 0 == 1
                U5 = StreamUtil.ReadInt16(stream);

                //stream
                //StreamUtil.AlignBy16(stream);

                stream.Position += 4-OffsetBytes;
                int Temp = 0;
                fileHeaders = new List<FileHeader>();
                for (int i = 0; i < FileCount; i++)
                {
                    var TempHeader = new FileHeader();
                    TempHeader.Offset = StreamUtil.ReadBytes(stream, OffsetBytes);

                    if(TempHeader.Offset.Length==1)
                    {
                        stream.Position -= 1;
                        TempHeader.OffsetInt = (byte)TempHeader.Offset[0];
                    }
                    if (TempHeader.Offset.Length == 2)
                    {
                        stream.Position -= 2;
                        TempHeader.OffsetInt = StreamUtil.ReadInt16Big(stream);
                    }
                    if (TempHeader.Offset.Length == 3)
                    {
                        stream.Position -= 3;
                        TempHeader.OffsetInt = StreamUtil.ReadInt24Big(stream);
                    }
                    if (TempHeader.Offset.Length == 4)
                    {
                        stream.Position -= 4;
                        TempHeader.Speaker = StreamUtil.ReadUInt8(stream);
                        TempHeader.OffsetInt = StreamUtil.ReadInt24Big(stream);
                    }

                    TempHeader.Unknown2 = StreamUtil.ReadInt16(stream);
                    Temp += TempHeader.Unknown2;
                    fileHeaders.Add(TempHeader);
                }
                U6 = StreamUtil.ReadInt16(stream);

                Padding = new List<int>();
                for (int i = 0; i < PaddingCount; i++)
                {
                    Padding.Add(StreamUtil.ReadInt8(stream));
                }

            }
        }



        struct FileHeader
        {
            public int Speaker;
            public byte[] Offset;
            public int Unknown2;

            public int OffsetInt;
        }
    }
}
