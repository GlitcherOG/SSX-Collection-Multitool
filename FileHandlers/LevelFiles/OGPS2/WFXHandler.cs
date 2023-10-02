using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SSXMultiTool.FileHandlers.LevelFiles.OGPS2
{
    internal class WFXHandler
    {
        public float U0;
        public float U1;
        public int UStruct0Count;
        public int UStruct0Offset;
        public int UStruct1Count;
        public int UStruct1Offset;
        public int UStruct2Count;
        public int UStruct2Offset;
        public int EffectHeaderCount;
        public int EffectHeaderOffset;

        public List<UStruct0> uStruct0s = new List<UStruct0>();
        public List<UStruct1> uStruct1s = new List<UStruct1>();
        public List<UStruct2> uStruct2s = new List<UStruct2>();
        public List<EffectHeader> EffectHeaders = new List<EffectHeader>();

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                U0 = StreamUtil.ReadFloat(stream);
                U1 = StreamUtil.ReadFloat(stream);
                UStruct0Count = StreamUtil.ReadUInt32(stream);
                UStruct0Offset = StreamUtil.ReadUInt32(stream);
                UStruct1Count = StreamUtil.ReadUInt32(stream);
                UStruct1Offset = StreamUtil.ReadUInt32(stream);
                UStruct2Count = StreamUtil.ReadUInt32(stream);
                UStruct2Offset = StreamUtil.ReadUInt32(stream);
                EffectHeaderCount = StreamUtil.ReadUInt32(stream);
                EffectHeaderOffset = StreamUtil.ReadUInt32(stream);

                uStruct0s = new List<UStruct0>();
                stream.Position = UStruct0Offset;
                for (int i = 0; i < UStruct0Count; i++)
                {
                    var TempUstruct0 = new UStruct0();

                    TempUstruct0.U0 = StreamUtil.ReadUInt32(stream);
                    TempUstruct0.U1 = StreamUtil.ReadUInt32(stream);
                    TempUstruct0.U2 = StreamUtil.ReadUInt32(stream);
                    TempUstruct0.U3 = StreamUtil.ReadUInt32(stream);
                    TempUstruct0.U4 = StreamUtil.ReadUInt32(stream);
                    TempUstruct0.U5 = StreamUtil.ReadUInt32(stream);
                    TempUstruct0.U6 = StreamUtil.ReadUInt32(stream);

                    uStruct0s.Add(TempUstruct0);
                }

                uStruct1s = new List<UStruct1>();
                stream.Position = UStruct1Offset;
                for (int i = 0; i < UStruct1Count; i++)
                {
                    var TempUStruct1 = new UStruct1();

                    TempUStruct1.U0 = StreamUtil.ReadUInt32(stream);
                    TempUStruct1.U1 = StreamUtil.ReadUInt32(stream);
                    TempUStruct1.U2 = StreamUtil.ReadUInt32(stream);

                    uStruct1s.Add(TempUStruct1);
                }

                uStruct2s = new List<UStruct2>();
                stream.Position = UStruct2Offset;
                for (int i = 0; i < UStruct2Count; i++)
                {
                    var TempUStruct2 = new UStruct2();

                    TempUStruct2.U0 = StreamUtil.ReadUInt32(stream);
                    TempUStruct2.U1 = StreamUtil.ReadUInt32(stream);
                    TempUStruct2.U2 = StreamUtil.ReadUInt32(stream);

                    uStruct2s.Add(TempUStruct2);
                }

                EffectHeaders = new List<EffectHeader>();
                stream.Position = EffectHeaderOffset;
                for (int i = 0; i < EffectHeaderCount; i++)
                {
                    EffectHeader TempEffectHeader = new EffectHeader();

                    TempEffectHeader.Count = StreamUtil.ReadUInt32(stream);
                    TempEffectHeader.Offset = StreamUtil.ReadUInt32(stream);

                    TempEffectHeader.Effects = new List<Effect>();
                    long TempPos = stream.Position;
                    stream.Position = EffectHeaderOffset + (EffectHeaderCount*8) + TempEffectHeader.Offset;
                    for (int a = 0; a < TempEffectHeader.Count; a++)
                    {
                        var TempEffect = new Effect();

                        TempEffect.Type = StreamUtil.ReadUInt16(stream);
                        TempEffect.Size = StreamUtil.ReadUInt16(stream);

                        if(TempEffect.Type==0)
                        {
                            Type0 TempType0 = new Type0();

                            TempType0.SubType = StreamUtil.ReadUInt32(stream);

                            if(TempType0.SubType == 0)
                            {
                                Type0Sub0 type0Sub0 = new Type0Sub0();

                                type0Sub0.U1 = StreamUtil.ReadFloat(stream);
                                type0Sub0.U2 = StreamUtil.ReadFloat(stream);
                                type0Sub0.U3 = StreamUtil.ReadFloat(stream);
                                type0Sub0.U4 = StreamUtil.ReadFloat(stream);
                                type0Sub0.U5 = StreamUtil.ReadUInt32(stream);
                                type0Sub0.U6 = StreamUtil.ReadUInt32(stream);

                                TempType0.type0Sub0 = type0Sub0;
                            }
                            else if (TempType0.SubType == 2)
                            {
                                TempType0.type0Sub2 = StreamUtil.ReadFloat(stream);
                            }
                            else if (TempType0.SubType == 5)
                            {
                                TempType0.type0Sub5 = StreamUtil.ReadUInt32(stream);
                            }
                            else if (TempType0.SubType == 7)
                            {
                                Type0Sub7 type0Sub7 = new Type0Sub7();

                                type0Sub7.U0 = StreamUtil.ReadUInt32(stream);
                                type0Sub7.U1 = StreamUtil.ReadUInt32(stream);
                                type0Sub7.U2 = StreamUtil.ReadUInt32(stream);
                                type0Sub7.U3 = StreamUtil.ReadUInt32(stream);
                                type0Sub7.U4 = StreamUtil.ReadUInt32(stream);
                                type0Sub7.U5 = StreamUtil.ReadUInt32(stream);
                                type0Sub7.U6 = StreamUtil.ReadUInt32(stream);

                                TempType0.type0Sub7 = type0Sub7;
                            }
                            else if (TempType0.SubType == 10)
                            {
                                Type0Sub10 type0Sub10 = new Type0Sub10();

                                type0Sub10.U1 = StreamUtil.ReadFloat(stream);
                                type0Sub10.U2 = StreamUtil.ReadFloat(stream);
                                type0Sub10.U3 = StreamUtil.ReadFloat(stream);
                                type0Sub10.U4 = StreamUtil.ReadFloat(stream);
                                type0Sub10.U5 = StreamUtil.ReadUInt32(stream);
                                type0Sub10.U6 = StreamUtil.ReadUInt32(stream);

                                TempType0.type0Sub10 = type0Sub10;
                            }
                            else if (TempType0.SubType == 11)
                            {
                                Type0Sub11 type0Sub11 = new Type0Sub11();

                                type0Sub11.U0 = StreamUtil.ReadUInt32(stream);
                                type0Sub11.U1 = StreamUtil.ReadUInt32(stream);
                                type0Sub11.U2 = StreamUtil.ReadUInt32(stream);
                                type0Sub11.U3 = StreamUtil.ReadUInt32(stream);

                                TempType0.type0Sub11 = type0Sub11;
                            }
                            else if (TempType0.SubType == 12)
                            {
                                Type0Sub12 type0Sub12 = new Type0Sub12();

                                type0Sub12.U0 = StreamUtil.ReadUInt32(stream);
                                type0Sub12.U1 = StreamUtil.ReadFloat(stream);

                                TempType0.type0Sub12 = type0Sub12;
                            }
                            else if (TempType0.SubType == 13)
                            {
                                Type0Sub13 type0Sub13 = new Type0Sub13();

                                type0Sub13.U0 = StreamUtil.ReadUInt32(stream);
                                type0Sub13.U1 = StreamUtil.ReadUInt32(stream);
                                type0Sub13.U2 = StreamUtil.ReadUInt32(stream);
                                type0Sub13.U3 = StreamUtil.ReadUInt32(stream);

                                TempType0.type0Sub13 = type0Sub13;
                            }
                            else if (TempType0.SubType == 14)
                            {
                                Type0Sub14 type0Sub14 = new Type0Sub14();

                                type0Sub14.U0 = StreamUtil.ReadUInt32(stream);
                                type0Sub14.U1 = StreamUtil.ReadFloat(stream);

                                TempType0.type0Sub14 = type0Sub14;
                            }
                            else if (TempType0.SubType == 17)
                            {
                                TempType0.type0Sub17 = StreamUtil.ReadFloat(stream);
                            }
                            else if (TempType0.SubType == 19)
                            {
                                Type0Sub19 type0Sub19 = new Type0Sub19();

                                type0Sub19.U0 = StreamUtil.ReadUInt32(stream);
                                type0Sub19.U1 = StreamUtil.ReadUInt32(stream);
                                type0Sub19.U2 = StreamUtil.ReadUInt32(stream);
                                type0Sub19.U3 = StreamUtil.ReadUInt32(stream);
                                type0Sub19.U4 = StreamUtil.ReadUInt32(stream);
                                type0Sub19.U5 = StreamUtil.ReadUInt32(stream);
                                type0Sub19.U6 = StreamUtil.ReadUInt32(stream);
                                type0Sub19.U7 = StreamUtil.ReadUInt32(stream);

                                TempType0.type0Sub19 = type0Sub19;
                            }
                            else if (TempType0.SubType == 20)
                            {
                                Type0Sub20 type0Sub20 = new Type0Sub20();

                                type0Sub20.U0 = StreamUtil.ReadUInt32(stream);
                                type0Sub20.U1 = StreamUtil.ReadUInt32(stream);
                                type0Sub20.U2 = StreamUtil.ReadUInt32(stream);
                                type0Sub20.U3 = StreamUtil.ReadUInt32(stream);
                                type0Sub20.U4 = StreamUtil.ReadUInt32(stream);
                                type0Sub20.U5 = StreamUtil.ReadUInt32(stream);
                                type0Sub20.U6 = StreamUtil.ReadUInt32(stream);
                                type0Sub20.U7 = StreamUtil.ReadUInt32(stream);
                                type0Sub20.U8 = StreamUtil.ReadUInt32(stream);

                                TempType0.type0Sub20 = type0Sub20;
                            }
                            else if (TempType0.SubType == 21)
                            {
                                Type0Sub21 type0Sub21 = new Type0Sub21();

                                type0Sub21.U0 = StreamUtil.ReadUInt32(stream);
                                type0Sub21.U1 = StreamUtil.ReadUInt32(stream);
                                type0Sub21.U2 = StreamUtil.ReadFloat(stream);

                                TempType0.type0Sub21 = type0Sub21;
                            }
                            else if (TempType0.SubType == 256)
                            {
                                Type0Sub256 type0Sub256 = new Type0Sub256();

                                type0Sub256.U0 = StreamUtil.ReadUInt32(stream);
                                type0Sub256.U1 = StreamUtil.ReadUInt32(stream);
                                type0Sub256.U2 = StreamUtil.ReadUInt32(stream);
                                type0Sub256.U3 = StreamUtil.ReadUInt32(stream);
                                type0Sub256.U4 = StreamUtil.ReadUInt32(stream);
                                type0Sub256.U5 = StreamUtil.ReadUInt32(stream);
                                type0Sub256.U6 = StreamUtil.ReadUInt32(stream);

                                TempType0.type0Sub256 = type0Sub256;
                            }
                            else if (TempType0.SubType ==257)
                            {
                                Type0Sub257 type0Sub257 = new Type0Sub257();

                                type0Sub257.U0 = StreamUtil.ReadUInt32(stream);
                                type0Sub257.U1 = StreamUtil.ReadUInt32(stream);
                                type0Sub257.U2 = StreamUtil.ReadUInt32(stream);
                                type0Sub257.U3 = StreamUtil.ReadUInt32(stream);
                                type0Sub257.U4 = StreamUtil.ReadUInt32(stream);
                                type0Sub257.U5 = StreamUtil.ReadUInt32(stream);
                                type0Sub257.U6 = StreamUtil.ReadUInt32(stream);

                                TempType0.type0Sub257 = type0Sub257;
                            }
                            else if (TempType0.SubType == 258)
                            {
                                Type0Sub258 type0Sub258 = new Type0Sub258();

                                type0Sub258.U0 = StreamUtil.ReadUInt32(stream);
                                type0Sub258.U1 = StreamUtil.ReadUInt32(stream);
                                type0Sub258.U2 = StreamUtil.ReadUInt32(stream);
                                type0Sub258.U3 = StreamUtil.ReadUInt32(stream);
                                type0Sub258.U4 = StreamUtil.ReadUInt32(stream);
                                type0Sub258.U5 = StreamUtil.ReadUInt32(stream);
                                type0Sub258.U6 = StreamUtil.ReadUInt32(stream);
                                type0Sub258.U7 = StreamUtil.ReadUInt32(stream);
                                type0Sub258.U8 = StreamUtil.ReadUInt32(stream);
                                type0Sub258.U9 = StreamUtil.ReadUInt32(stream);
                                type0Sub258.U10 = StreamUtil.ReadUInt32(stream);

                                TempType0.type0Sub258 = type0Sub258;
                            }
                            else
                            {
                                Console.WriteLine("Unknown Type and Size");
                            }

                            TempEffect.type0 = TempType0;
                        }
                        else if (TempEffect.Type == 1)
                        {
                            Type1 TempType1= new Type1();

                            TempType1.SubType = StreamUtil.ReadUInt32(stream);

                            if(TempType1.SubType==0)
                            {
                                Type1Sub0 type1Sub0 = new Type1Sub0();
                                type1Sub0.U1 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U2 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U3 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U4 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U5 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U6 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U7 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U8 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U9 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U10 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U11 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U12 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U13 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U14 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U15 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U16 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U17 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U18 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U19 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U20 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U21 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U22 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U23 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U24 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U25 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U26 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U27 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U28 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U29 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U30 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U31 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U32 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U33 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U34 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U35 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U36 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U37 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U38 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U39 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U40 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U41 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U42 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U43 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U44 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U45 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U46 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U47 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U48 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U49 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U50 = StreamUtil.ReadUInt32(stream);
                                type1Sub0.U51 = StreamUtil.ReadUInt32(stream);
                            }
                            else if (TempType1.SubType == 1)
                            {
                                Type1Sub1 type1Sub1 = new Type1Sub1();

                                type1Sub1.U0 = StreamUtil.ReadUInt32(stream);
                                type1Sub1.U1 = StreamUtil.ReadUInt32(stream);
                                type1Sub1.U2 = StreamUtil.ReadUInt32(stream);
                                type1Sub1.U3 = StreamUtil.ReadUInt32(stream);
                                type1Sub1.U4 = StreamUtil.ReadUInt32(stream);
                                type1Sub1.U5 = StreamUtil.ReadUInt32(stream);
                                type1Sub1.U6 = StreamUtil.ReadUInt32(stream);
                                type1Sub1.U7 = StreamUtil.ReadUInt32(stream);
                                type1Sub1.U8 = StreamUtil.ReadUInt32(stream);
                                type1Sub1.U9 = StreamUtil.ReadUInt32(stream);
                                type1Sub1.U10 = StreamUtil.ReadUInt32(stream);

                                TempType1.type1Sub1 = type1Sub1;
                            }
                            else if (TempType1.SubType == 2)
                            {
                                Type1Sub2 type1Sub2 = new Type1Sub2();

                                type1Sub2.U0 = StreamUtil.ReadUInt32(stream);
                                type1Sub2.U1 = StreamUtil.ReadUInt32(stream);
                                type1Sub2.U2 = StreamUtil.ReadUInt32(stream);
                                type1Sub2.U3 = StreamUtil.ReadUInt32(stream);
                                type1Sub2.U4 = StreamUtil.ReadUInt32(stream);
                                type1Sub2.U5 = StreamUtil.ReadUInt32(stream);
                                type1Sub2.U6 = StreamUtil.ReadUInt32(stream);
                                type1Sub2.U7 = StreamUtil.ReadUInt32(stream);
                                type1Sub2.U8 = StreamUtil.ReadUInt32(stream);
                                type1Sub2.U9 = StreamUtil.ReadUInt32(stream);
                                type1Sub2.U10 = StreamUtil.ReadUInt32(stream);
                                type1Sub2.U11 = StreamUtil.ReadUInt32(stream);

                                TempType1.type1Sub2 = type1Sub2;
                            }
                            else
                            {
                                Console.WriteLine("Unknown SubType and Size");
                            }

                            TempEffect.type1 = TempType1;
                        }
                        else if (TempEffect.Type == 2)
                        {
                            Type2 type2 = new Type2();

                            type2.U0 = StreamUtil.ReadUInt32(stream);
                            type2.U1 = StreamUtil.ReadFloat(stream);

                            TempEffect.type2 = type2;
                        }
                        else if (TempEffect.Type == 3)
                        {
                            TempEffect.type3 = StreamUtil.ReadFloat(stream);
                        }
                        else if (TempEffect.Type == 4)
                        {
                            Type4 type4 = new Type4();

                            type4.U0 = StreamUtil.ReadUInt32(stream);
                            type4.U1 = StreamUtil.ReadUInt32(stream);
                            type4.U2 = StreamUtil.ReadUInt32(stream);

                            TempEffect.type4 = type4;
                        }
                        else if (TempEffect.Type == 6)
                        {
                            Type6 TempType6 = new Type6();

                            TempType6.U0 = StreamUtil.ReadUInt32(stream);
                            TempType6.U1 = StreamUtil.ReadUInt32(stream);
                            TempType6.U2 = StreamUtil.ReadUInt32(stream);

                            TempEffect.type6 = TempType6;
                        }
                        else if (TempEffect.Type == 7)
                        {
                            TempEffect.type7 = StreamUtil.ReadUInt32(stream);
                        }
                        else if (TempEffect.Type == 8)
                        {
                            Type8 TempType8 = new Type8();

                            TempType8.U0 = StreamUtil.ReadUInt32(stream);
                            TempType8.U1 = StreamUtil.ReadFloat(stream);

                            TempEffect.type8 = TempType8;
                        }
                        else if (TempEffect.Type == 12)
                        {
                            TempEffect.type12 = StreamUtil.ReadUInt32(stream);
                        }
                        else if (TempEffect.Type == 13)
                        {
                            TempEffect.type13 = StreamUtil.ReadFloat(stream);
                        }
                        else if (TempEffect.Type == 16)
                        {
                            TempEffect.type16 = StreamUtil.ReadFloat(stream);
                        }
                        else if (TempEffect.Type == 17)
                        {
                            TempEffect.type17 = StreamUtil.ReadFloat(stream);
                        }
                        else
                        {
                            Console.WriteLine("Unknown Type and Size");
                        }

                        TempEffectHeader.Effects.Add(TempEffect);
                    }

                    stream.Position = TempPos;

                    EffectHeaders.Add(TempEffectHeader);
                }


            }
        }

        public struct UStruct0
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
        }

        public struct UStruct1
        {
            public int U0;
            public int U1;
            public int U2;
        }
        public struct UStruct2
        {
            public int U0;
            public int U1;
            public int U2;
        }

        //Effect
        public struct EffectHeader
        {
            public int Count;
            public int Offset;

            public List<Effect> Effects;
        }

        public struct Effect
        {
            //16
            public int Type;
            public int Size;

            public Type0? type0;
            public Type1? type1;
            public Type2? type2;
            public float type3;
            public Type4? type4;
            public Type6? type6;
            public int type7;
            public Type8? type8;
            public int type12;
            public float type13;
            public float type16;
            public float type17;
        }

        public struct Type0
        {
            public int SubType;

            public Type0Sub0? type0Sub0;
            public float type0Sub2;
            public int type0Sub5;
            public Type0Sub7? type0Sub7;
            public Type0Sub10? type0Sub10;
            public Type0Sub11? type0Sub11;
            public Type0Sub12? type0Sub12;
            public Type0Sub13? type0Sub13;
            public Type0Sub14? type0Sub14;
            public float type0Sub17;
            public Type0Sub19? type0Sub19;
            public Type0Sub20? type0Sub20;
            public Type0Sub21? type0Sub21;
            public Type0Sub256? type0Sub256;
            public Type0Sub257? type0Sub257;
            public Type0Sub258? type0Sub258;
        }

        public struct Type0Sub0
        {
            public float U1;
            public float U2;
            public float U3;
            public float U4;
            public int U5;
            public int U6;
        }

        public struct Type0Sub7
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
        }

        public struct Type0Sub10
        {
            public float U1;
            public float U2;
            public float U3;
            public float U4;
            public int U5;
            public int U6;
        }

        public struct Type0Sub11
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
        }

        public struct Type0Sub12
        {
            public int U0;
            public float U1;
        }

        public struct Type0Sub13
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
        }

        public struct Type0Sub14
        {
            public int U0;
            public float U1;
        }

        public struct Type0Sub19
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
            public int U7;
        }

        public struct Type0Sub20
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
            public int U7;
            public int U8;
        }

        public struct Type0Sub21
        {
            public int U0;
            public int U1;
            public float U2;

        }

        public struct Type0Sub256
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
        }

        public struct Type0Sub257
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
        }

        public struct Type0Sub258
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
            public int U7;
            public int U8;
            public int U9;
            public int U10;
        }

        public struct Type1
        {
            public int SubType;
            public Type1Sub0? type1Sub0;
            public Type1Sub1? type1Sub1;
            public Type1Sub2? type1Sub2;
        }

        public struct Type1Sub0
        {
            public int U1;
            public int U2;
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
            public int U15;
            public int U16;
            public int U17;
            public int U18;
            public int U19;
            public int U20;
            public int U21;
            public int U22;
            public int U23;
            public int U24;
            public int U25;
            public int U26;
            public int U27;
            public int U28;
            public int U29;
            public int U30;
            public int U31;
            public int U32;
            public int U33;
            public int U34;
            public int U35;
            public int U36;
            public int U37;
            public int U38;
            public int U39;
            public int U40;
            public int U41;
            public int U42;
            public int U43;
            public int U44;
            public int U45;
            public int U46;
            public int U47;
            public int U48;
            public int U49;
            public int U50;
            public int U51;
        }

        public struct Type1Sub1
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
            public int U7;
            public int U8;
            public int U9;
            public int U10;
        }

        public struct Type1Sub2
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
            public int U7;
            public int U8;
            public int U9;
            public int U10;
            public int U11;
        }

        public struct Type2
        {
            public int U0;
            public float U1;
        }

        public struct Type4
        {
            public int U0;
            public int U1;
            public int U2;
        }

        public struct Type6
        {
            public int U0;
            public int U1;
            public int U2;
        }

        public struct Type8
        {
            public int U0;
            public float U1;
        }
    }
}
