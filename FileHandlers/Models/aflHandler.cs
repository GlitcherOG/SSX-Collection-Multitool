using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;

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

                    //Header Type - Pointer Count
                    //0 - 60
                    //1 - 6
                    //2 - 0
                    //4 - 12
                    //5 - 6
                    //6 - 6
                    //7 - 6
                    //8 - 12
                    //9 - 12
                    //10 - 23
                    //11 - 6
                    //12 - 3
                    //13 - 12
                    //14 - 6
                    //15 - 3

                    //45 - 0
                    //49 - 0
                    //205 - 0
                    //255 - 0

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
                    else if (TempHeader.HeaderType == 13 || TempHeader.HeaderType == 8 || TempHeader.HeaderType == 9 || TempHeader.HeaderType == 4)
                    {
                        for (int a = 0; a < 12; a++)
                        {
                            TempHeader.animationPointerDataOffsets.Add(StreamUtil.ReadUInt32(stream));
                        }
                    }
                    else if (TempHeader.HeaderType == 12 || TempHeader.HeaderType == 15)
                    {
                        for (int a = 0; a < 3; a++)
                        {
                            TempHeader.animationPointerDataOffsets.Add(StreamUtil.ReadUInt32(stream));
                        }
                    }
                    else if ((TempHeader.HeaderType == 255 || TempHeader.HeaderType == 205 || TempHeader.HeaderType == 49 || TempHeader.HeaderType == 45 || TempHeader.HeaderType ==2) && TempHeader.PointerIndexStart == 0)
                    {

                    }
                    else
                    {
                        MessageBox.Show("Error Unknown Header Type " + TempHeader.HeaderType);
                    }
                    animationHeaders[i] = TempHeader;
                }

            }
        }



        /// <summary>
        /// Temp Function Made to Replacate Linkz reversed enginred function
        /// </summary>
        int bytes_to_skip= 0;
        float kfc_float = 0;
        float afl_read_first(byte[] data)
        {
            float final_float = 0;
            //Reads int 16 i think need to double check
            int TempUShort = data[0];

            if (TempUShort % 16 == 5)
            {
                var second = (int)data[1];


                int Test = (data[0] << 16) >> 20;
                if (Test > 0)
                {
                    for (int i = 0; i < Test; i++)
                    {
                        int second_again = second;

                        var yuh = (second << 16) >> 20;

                        if(kfc_float <= ByteUtil.UintByteToFloat(yuh + -1))
                        {
                            final_float = afl_read_second(data);
                            return final_float;
                        }


                        int wut = 0;

                        switch (second % 16)
                        {
                            case 0:
                                wut = 5;
                                break;

                            case 1:
                                wut = 8;
                                break;

                            case 2:
                                wut = 11;
                                break;

                            case 3:
                                wut = 14;
                                break;

                            case 4:
                                wut = yuh * 4 + 2;
                                break;

                            case 6:
                                wut = yuh + 8;
                                break;

                            case 7:
                                wut = yuh * 2 + 8;
                                break;

                            default:
                                MessageBox.Show("Error Running Function" + (second % 16).ToString());
                                break;
                        }

                        second = second + wut;

                        kfc_float = kfc_float - ByteUtil.UintByteToFloat((second_again << 16) >> 20) + -1;
                    }
                }
                final_float = 0;
            }
            else if (TempUShort % 16 == 0)
            {
                final_float = afl_read_second(data);
            }

            if(bytes_to_skip==0)
            {
                bytes_to_skip = 2;
            }

            return final_float;
        }

        float afl_read_second(byte[] data)
        {
            float some_float = 0;
            var third = data[2];

            //The Original switch i was given im sure was broken ill need to check
            switch (data[0])
            {
                case 0:
                    var shifted = data[2] << 8 | 128 | data[3] << 16 | data[4] << 24;
                    float floated = ByteUtil.UintByteToFloat(shifted);
                    bytes_to_skip = 5;
                    return floated;
                case 1:
                    var shifted1 = data[5] << 8 | 128 | data[6] << 16 | data[7] << 24;
                    some_float = kfc_float * (third << 8 | 128 | data[3] << 16 | data[4] << 24);
                    break;

                case 2:
                    shifted1 = data[8] << 8 | 128 | data[9] << 16 | data[10] << 24;
                    some_float = kfc_float * (kfc_float * ByteUtil.UintByteToFloat(third << 8 | 128 | data[3] << 16 | data[4] << 24)
									           +ByteUtil.UintByteToFloat(data[5] << 8 | 128 | data[6] << 16 | data[7] << 24));
                    break;
            }

            return some_float;
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
