using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models
{
    public class aflHandler
    {
        public int Console;
        public int U1;
        public int AnimationHeaderCount;

        public int AnimationPointerDataOffset;
        public int AnimationDataOffset;

        public List<AnimationHeader> animationHeaders = new List<AnimationHeader>();
        public List<int> animationPointerDataOffsets = new List<int>();

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                Console = StreamUtil.ReadInt8(stream);
                U1 = StreamUtil.ReadInt8(stream);
                AnimationHeaderCount = StreamUtil.ReadInt16(stream);

                AnimationPointerDataOffset = StreamUtil.ReadUInt32(stream);
                AnimationDataOffset = StreamUtil.ReadUInt32(stream);

                animationHeaders = new List<AnimationHeader>();

                for (int i = 0; i < AnimationHeaderCount; i++)
                {
                    var AnimHeader = new AnimationHeader();
                    AnimHeader.U0 = StreamUtil.ReadUInt32(stream);
                    AnimHeader.PointerIndexStart = StreamUtil.ReadUInt32(stream);
                    AnimHeader.HeaderType = StreamUtil.ReadUInt8(stream);
                    AnimHeader.RelatedHeaders = StreamUtil.ReadUInt8(stream);
                    AnimHeader.U3 = StreamUtil.ReadUInt16(stream);
                    AnimHeader.U4 = StreamUtil.ReadUInt16(stream);
                    AnimHeader.U5 = StreamUtil.ReadUInt16(stream);
                    AnimHeader.U6 = StreamUtil.ReadUInt16(stream);
                    AnimHeader.U7 = StreamUtil.ReadUInt16(stream);
                    AnimHeader.U8 = StreamUtil.ReadUInt16(stream);
                    AnimHeader.U9 = StreamUtil.ReadUInt16(stream);
                    AnimHeader.U10 = StreamUtil.ReadUInt16(stream);
                    AnimHeader.U11 = StreamUtil.ReadUInt16(stream);
                    AnimHeader.U12 = StreamUtil.ReadUInt16(stream);
                    AnimHeader.U13 = StreamUtil.ReadUInt16(stream);
                    AnimHeader.U14 = StreamUtil.ReadUInt32(stream);
                    animationHeaders.Add(AnimHeader);
                }

                int TempAnimDataOffset = (int)stream.Position;

                for (int i = 0; i < animationHeaders.Count; i++)
                {
                    var TempHeader = animationHeaders[i];

                    TempHeader.animationPointerDataOffsets = new List<int>();

                    stream.Position = TempAnimDataOffset + 4 * TempHeader.PointerIndexStart;

                    //0
                    //1
                    //5
                    //6
                    //7
                    //8
                    //10
                    //11
                    //12
                    //13
                    //14

                    if (TempHeader.HeaderType==0)
                    {
                        for (int a = 0; a < 60; a++)
                        {
                            TempHeader.animationPointerDataOffsets.Add(StreamUtil.ReadUInt32(stream));
                        }
                    }
                    else if (TempHeader.HeaderType == 1 || TempHeader.HeaderType == 5 || TempHeader.HeaderType == 6 || TempHeader.HeaderType == 7|| TempHeader.HeaderType == 11 || TempHeader.HeaderType == 14)
                    {
                        for (int a = 0; a < 6; a++)
                        {
                            TempHeader.animationPointerDataOffsets.Add(StreamUtil.ReadUInt32(stream));
                        }
                    }
                    else if (TempHeader.HeaderType == 10)
                    {
                        for (int a = 0; a < 23; a++)
                        {
                            TempHeader.animationPointerDataOffsets.Add(StreamUtil.ReadUInt32(stream));
                        }
                    }
                    else if (TempHeader.HeaderType == 13 || TempHeader.HeaderType == 8)
                    {
                        for (int a = 0; a < 12; a++)
                        {
                            TempHeader.animationPointerDataOffsets.Add(StreamUtil.ReadUInt32(stream));
                        }
                    }
                    else if (TempHeader.HeaderType == 12)
                    {
                        for (int a = 0; a < 3; a++)
                        {
                            TempHeader.animationPointerDataOffsets.Add(StreamUtil.ReadUInt32(stream));
                        }
                    }





                    animationHeaders[i] = TempHeader;
                }

            }
        }



        public struct AnimationHeader
        {
            public int U0;
            public int PointerIndexStart;
            public int HeaderType; //int8
            //0 = 60 Anim
            //1 - 6

            public int RelatedHeaders; //int8
            public int U3;
            public int U4;
            public int U5;
            public int U6;
            public int U7;
            public int U8;
            public int U9;
            public int U10;
            public int U11;
            public int U12;
            public int U13;
            public int U14;

            public List<int> animationPointerDataOffsets;
        }

    }
}
