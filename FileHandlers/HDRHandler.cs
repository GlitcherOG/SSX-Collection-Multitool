using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
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

        public List<FileHeader> fileHeaders = new List<FileHeader>();
        public List<int> Padding = new List<int>();
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

        public void Save(string Path)
        {
            MemoryStream stream = new MemoryStream();

            StreamUtil.WriteInt16(stream, U1);
            StreamUtil.WriteInt16(stream, U2);
            StreamUtil.WriteUInt8(stream, OffsetBytes);
            StreamUtil.WriteUInt8(stream, fileHeaders.Count);
            StreamUtil.WriteUInt8(stream, Padding.Count);
            StreamUtil.WriteUInt8(stream, U4);
            StreamUtil.WriteInt16(stream, U5);

            stream.Position += 4 - OffsetBytes;

            for (int i = 0; i < fileHeaders.Count; i++)
            {
                var TempHeader = fileHeaders[i];
                if (OffsetBytes == 1)
                {
                    StreamUtil.WriteUInt8(stream, TempHeader.OffsetInt);
                }
                if (OffsetBytes == 2)
                {
                    StreamUtil.WriteInt16Big(stream, TempHeader.OffsetInt);
                }
                if (OffsetBytes == 3)
                {
                    StreamUtil.WriteInt24Big(stream, TempHeader.OffsetInt);
                }
                if (OffsetBytes == 4)
                {
                    StreamUtil.WriteUInt8(stream, TempHeader.Speaker);
                    StreamUtil.WriteInt24Big(stream, TempHeader.OffsetInt);
                }

                StreamUtil.WriteInt16(stream, TempHeader.Unknown2);
            }

            if (OffsetBytes != 1)
            {
                stream.Position += OffsetBytes - 1;
            }

            for (int i = 0; i < Padding.Count; i++)
            {
                StreamUtil.WriteUInt8(stream, Padding[i]);
            }

            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
            var file = File.Create(Path);
            stream.Position = 0;
            stream.CopyTo(file);
            stream.Dispose();
            file.Close();
        }



        public struct FileHeader
        {
            public int Speaker;
            public byte[] Offset;
            public int Unknown2;

            public int OffsetInt;
        }
    }
}
