using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.SSX3
{
    public class LUIHandler
    {
        public int Magic;
        public int U0;
        public int U1Offset;
        public int ScreenTableOffset;
        public int ObjectTableOffset;
        public int FontTableOffset;

        public int U1Count;
        public int ScreenTableCount;
        public int ObjectTableCount;
        public int FontTableCount;

        public List<ScreenTable> ScreenTables = new List<ScreenTable>();
        public List<ObjectTable> ObjectTables = new List<ObjectTable>();
        public List<FontTable> FontTables = new List<FontTable>();

        public void LoadLUIFile(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                Magic = StreamUtil.ReadUInt32(stream);
                U0 = StreamUtil.ReadUInt32(stream);
                U1Offset = StreamUtil.ReadUInt32(stream);
                ScreenTableOffset = StreamUtil.ReadUInt32(stream);
                ObjectTableOffset = StreamUtil.ReadUInt32(stream);
                FontTableOffset = StreamUtil.ReadUInt32(stream);

                stream.Position = U1Offset;
                U1Count = StreamUtil.ReadUInt32(stream);

                stream.Position = ScreenTableOffset;
                ScreenTableCount = StreamUtil.ReadUInt32(stream);
                ScreenTables = new List<ScreenTable>();
                for (int i = 0; i < ScreenTableCount; i++)
                {
                    var NewScreenTable = new ScreenTable();

                    NewScreenTable.U0 = StreamUtil.ReadUInt32(stream);
                    NewScreenTable.Offset = StreamUtil.ReadUInt32(stream);

                    ScreenTables.Add(NewScreenTable);
                }

                for (int i = 0; i < ScreenTables.Count; i++)
                {
                    stream.Position = ScreenTableOffset + ScreenTables[i].Offset;

                    int ByteSize = 0;

                    if(ScreenTables.Count-1!=i)
                    {
                        ByteSize = (int)(ScreenTables[i + 1].Offset - ScreenTables[i].Offset);
                    }
                    else
                    {
                        ByteSize = (int)(ObjectTableOffset - (stream.Position + ScreenTables[i].Offset));
                    }
                    var TempScreenTable = ScreenTables[i];
                    TempScreenTable.RefpackData = StreamUtil.ReadBytes(stream, ByteSize);

                    TempScreenTable.RefpackData = RefpackHandler.Decompress(TempScreenTable.RefpackData);

                    ScreenTables[i] = TempScreenTable;
                }

                stream.Position = ObjectTableOffset;
                ObjectTableCount = StreamUtil.ReadUInt32(stream);
                ObjectTables = new List<ObjectTable>();
                for (int i = 0; i < ObjectTableCount; i++)
                {
                    var NewObjectTable = new ObjectTable();
                    NewObjectTable.Name = StreamUtil.ReadUInt32(stream);
                    NewObjectTable.Flags = StreamUtil.ReadUInt32(stream);

                    NewObjectTable.Position = StreamUtil.ReadVector2(stream);
                    NewObjectTable.Size = StreamUtil.ReadVector2(stream);

                    ObjectTables.Add(NewObjectTable);
                }

                stream.Position = FontTableOffset;
                FontTableCount = StreamUtil.ReadUInt32(stream);
                FontTables = new List<FontTable>();
                for (int i = 0; i < FontTableCount; i++)
                {
                    var NewFontTable = new FontTable();
                    NewFontTable.U0 = StreamUtil.ReadUInt32(stream);
                    NewFontTable.U1  =StreamUtil.ReadUInt32(stream);
                    NewFontTable.Name = StreamUtil.ReadString(stream, 8);
                    FontTables.Add(NewFontTable);
                }

            }
        }

        public struct U1Struct
        {
            public int LineID;
            public int Type;
            public int LineCount;
            public int NameHash;
            public int ByteSize;



        }

        public struct Type51Lines
        {

        }

        public struct ScreenTable
        {
            public int U0;
            public int Offset;

            public byte[] RefpackData;
        }

        public struct ObjectTable
        {
            public int Name;
            public int Flags;

            public Vector2 Position;
            public Vector2 Size;
        }

        public struct FontTable
        {
            public int U0;
            public int U1;
            public string Name;
        }

    }
}
