using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

                stream.Position = ObjectTableOffset;
                ObjectTableCount = StreamUtil.ReadUInt32(stream);

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


        struct FontTable
        {
            public int U0;
            public int U1;
            public string Name;
        }

    }
}
