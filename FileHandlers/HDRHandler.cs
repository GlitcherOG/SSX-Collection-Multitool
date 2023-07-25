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
        public int EntryTypes;
        public int FileCount;
        public int PaddingCount;
        public int AligmentSize;
        public int U5;

        public int GapSize;

        public List<FileHeader> fileHeaders = new List<FileHeader>();
        public List<int> Padding = new List<int>();
        public void Load(string Path)
        {
            using (Stream stream = File.Open(Path, FileMode.Open))
            {
                U1 = StreamUtil.ReadInt16(stream);
                U2 = StreamUtil.ReadInt16(stream); //Always -1
                EntryTypes = StreamUtil.ReadUInt8(stream);
                FileCount = StreamUtil.ReadUInt8(stream);
                PaddingCount = StreamUtil.ReadUInt8(stream);
                AligmentSize = StreamUtil.ReadUInt8(stream); //Multi 0 == 1
                U5 = StreamUtil.ReadInt16(stream);

                if(EntryTypes == 0 || EntryTypes == 2)
                {
                    stream.Position += 2;
                }
                else if (EntryTypes == 3 || EntryTypes == 1)
                {
                    stream.Position += 1;
                }


                fileHeaders = new List<FileHeader>();
                for (int i = 0; i < FileCount; i++)
                {
                    var TempHeader = new FileHeader();

                    if (EntryTypes == 0)
                    {
                        TempHeader.OffsetInt = StreamUtil.ReadInt16(stream, true);
                    }
                    if (EntryTypes == 1)
                    {
                        TempHeader.Unknown = StreamUtil.ReadInt8(stream);
                        TempHeader.OffsetInt = StreamUtil.ReadInt16(stream, true);
                    }
                    if (EntryTypes == 2)
                    {
                        TempHeader.OffsetInt = StreamUtil.ReadInt16(stream, true);
                        TempHeader.Unknown2 = StreamUtil.ReadInt8(stream);
                        TempHeader.EventID = StreamUtil.ReadInt8(stream);
                    }
                    if (EntryTypes == 3)
                    {
                        TempHeader.OffsetInt = StreamUtil.ReadInt24(stream, true);
                        TempHeader.Unknown2 = StreamUtil.ReadInt8(stream);
                        TempHeader.EventID = StreamUtil.ReadInt8(stream);
                    }
                    if (EntryTypes == 4)
                    {
                        TempHeader.Unknown = StreamUtil.ReadUInt8(stream);
                        TempHeader.OffsetInt = StreamUtil.ReadInt24(stream, true);
                        TempHeader.Unknown2 = StreamUtil.ReadInt8(stream);
                        TempHeader.EventID = StreamUtil.ReadInt8(stream);
                    }

                    fileHeaders.Add(TempHeader);
                }

                //Garbage stuff here
                if(PaddingCount>0)
                {
                    long Pos = stream.Position;

                    long NewPos = ByteUtil.FindPosition(stream, new byte[1] { 0xff });

                    if (NewPos != -1)
                    {
                        GapSize = (int)(NewPos - Pos);
                        stream.Position -= 1;
                    }
                }

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
            StreamUtil.WriteUInt8(stream, EntryTypes);
            StreamUtil.WriteUInt8(stream, fileHeaders.Count);
            StreamUtil.WriteUInt8(stream, Padding.Count);
            StreamUtil.WriteUInt8(stream, AligmentSize);
            StreamUtil.WriteInt16(stream, U5);

            if (EntryTypes == 0 || EntryTypes == 2)
            {
                stream.Position += 2;
            }
            else if (EntryTypes == 3 || EntryTypes == 1)
            {
                stream.Position += 1;
            }

            for (int i = 0; i < fileHeaders.Count; i++)
            {
                var TempHeader = fileHeaders[i];
                if (EntryTypes == 0)
                {
                    StreamUtil.WriteInt16(stream, TempHeader.OffsetInt, true);
                }
                if (EntryTypes == 1)
                {
                    StreamUtil.WriteUInt8(stream, TempHeader.Unknown);
                    StreamUtil.WriteInt16(stream, TempHeader.OffsetInt, true);
                }
                if (EntryTypes == 2)
                {
                    StreamUtil.WriteUInt8(stream, TempHeader.Unknown);
                    StreamUtil.WriteInt16(stream, TempHeader.OffsetInt, true);
                }
                if (EntryTypes == 3)
                {
                    StreamUtil.WriteInt24(stream, TempHeader.OffsetInt, true);
                    StreamUtil.WriteUInt8(stream, TempHeader.Unknown2);
                    StreamUtil.WriteUInt8(stream, TempHeader.EventID);
                }
                if (EntryTypes == 4)
                {
                    StreamUtil.WriteUInt8(stream, TempHeader.Unknown);
                    StreamUtil.WriteInt24(stream, TempHeader.OffsetInt, true);
                    StreamUtil.WriteUInt8(stream, TempHeader.Unknown2);
                    StreamUtil.WriteUInt8(stream, TempHeader.EventID);
                }
            }

            //Garbage stuff here
            stream.Position += GapSize;


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
            public int Unknown;
            public byte[] Offset;
            public int Unknown2;
            public int EventID;

            public int OffsetInt;
        }
    }
}
