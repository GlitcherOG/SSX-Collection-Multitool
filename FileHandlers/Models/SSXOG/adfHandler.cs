using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models.SSXOG
{
    public class adfHandler
    {
        public int Magic;
        public int AnimHeaderOffset;
        public int AnimDataOffset;
        public int AnimCount;
        public List<AnimHeader> AnimHeaders = new List<AnimHeader>();

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                Magic = StreamUtil.ReadUInt32(stream);
                AnimHeaderOffset = StreamUtil.ReadUInt32(stream);
                AnimDataOffset = StreamUtil.ReadUInt32(stream);
                AnimCount = StreamUtil.ReadUInt32(stream);

                AnimHeaders = new List<AnimHeader>();
                for (int i = 0; i < AnimCount; i++)
                {
                    var NewHeader = new AnimHeader();

                    NewHeader.AnimName = StreamUtil.ReadString(stream, 4);
                    NewHeader.AnimOffset = StreamUtil.ReadUInt32(stream);
                    NewHeader.ByteSize = StreamUtil.ReadUInt32(stream);

                    NewHeader.UCount0 = StreamUtil.ReadUInt8(stream);
                    NewHeader.UCount1 = StreamUtil.ReadUInt8(stream);
                    NewHeader.U5 = StreamUtil.ReadUInt8(stream);
                    NewHeader.U6 = StreamUtil.ReadUInt8(stream);

                    NewHeader.U7 = StreamUtil.ReadUInt8(stream);
                    NewHeader.U8 = StreamUtil.ReadUInt8(stream);
                    NewHeader.U9 = StreamUtil.ReadUInt8(stream);
                    NewHeader.U10 = StreamUtil.ReadUInt8(stream);

                    NewHeader.U11 = StreamUtil.ReadUInt16(stream);
                    NewHeader.U11b = StreamUtil.ReadUInt16(stream);
                    NewHeader.U12 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U13 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U14 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U15 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U16 = StreamUtil.ReadUInt32(stream);
                    AnimHeaders.Add(NewHeader);
                }



                for (int i = 0; i < AnimHeaders.Count; i++)
                {
                    stream.Position = AnimDataOffset + AnimHeaders[i].AnimOffset;



                }
            }
        }




        public struct AnimHeader
        {
            public string AnimName;
            public int AnimOffset;
            public int ByteSize;

            public int UCount0;
            public int UCount1;
            public int U5;
            public int U6;

            public int U7;
            public int U8;
            public int U9;
            public int U10;

            public int U11;
            public int U11b;
            public int U12;
            public int U13;
            public int U14;
            public int U15;
            public int U16;
        }


    }
}
