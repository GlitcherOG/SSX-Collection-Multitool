using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using SSXMultiTool.Utilities;
using static SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2.SSFHandler;

namespace SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2
{
    public class SSFHandler
    {
        public int U1;
        public int U2;
        public float U3;
        public int EffectSlotsCount;
        public int EffectSlotsOffset;
        public int PhysicsCount;
        public int PhysicsOffset; 
        public int CollisonModelCount;
        public int CollisonModelOffset;
        public int EffectsCount;
        public int EffectsOffset;
        public int FunctionCount;
        public int FunctionOffset;
        public int ObjectPropertiesCount;
        public int ObjectPropertiesOffset;
        public int InstanceCount;
        public int InstanceOffset;
        public int SplineCount;
        public int SplineOffset;

        public List<EffectSlot> EffectSlots = new List<EffectSlot>();
        public List<UStruct2> PhysicsObjects = new List<UStruct2>();
        public List<CollisonModelPointer> CollisonModelPointers = new List<CollisonModelPointer>();
        public List<EffectHeaderStruct> EffectHeaders = new List<EffectHeaderStruct>();
        public List<Function> Functions = new List<Function>();
        public List<UStruct5> ObjectProperties = new List<UStruct5>();
        public List<Spline> Splines = new List<Spline>();
        public List<int> InstanceState = new List<int>();

        public void Load(string Path)
        {
            using (Stream stream = File.Open(Path, FileMode.Open))
            {
                U1 = StreamUtil.ReadUInt32(stream);
                U2 = StreamUtil.ReadUInt32(stream);
                U3 = StreamUtil.ReadFloat(stream);
                EffectSlotsCount = StreamUtil.ReadUInt32(stream);
                EffectSlotsOffset = StreamUtil.ReadUInt32(stream);
                PhysicsCount = StreamUtil.ReadUInt32(stream);
                PhysicsOffset = StreamUtil.ReadUInt32(stream);
                CollisonModelCount = StreamUtil.ReadUInt32(stream);
                CollisonModelOffset = StreamUtil.ReadUInt32(stream);
                EffectsCount = StreamUtil.ReadUInt32(stream);
                EffectsOffset = StreamUtil.ReadUInt32(stream);
                FunctionCount = StreamUtil.ReadUInt32(stream);
                FunctionOffset = StreamUtil.ReadUInt32(stream);
                ObjectPropertiesCount = StreamUtil.ReadUInt32(stream);
                ObjectPropertiesOffset = StreamUtil.ReadUInt32(stream);
                InstanceCount = StreamUtil.ReadUInt32(stream);
                InstanceOffset = StreamUtil.ReadUInt32(stream);
                SplineCount = StreamUtil.ReadUInt32(stream);
                SplineOffset = StreamUtil.ReadUInt32(stream);

                EffectSlots = new List<EffectSlot>();
                stream.Position = EffectSlotsOffset;
                for (int i = 0; i < EffectSlotsCount; i++)
                {
                    var TempUstruct1 = new EffectSlot();

                    TempUstruct1.Slot1 = StreamUtil.ReadUInt32(stream);
                    TempUstruct1.Slot2 = StreamUtil.ReadUInt32(stream);
                    TempUstruct1.Slot3 = StreamUtil.ReadUInt32(stream);
                    TempUstruct1.Slot4 = StreamUtil.ReadUInt32(stream);
                    TempUstruct1.Slot5 = StreamUtil.ReadUInt32(stream);
                    TempUstruct1.Slot6 = StreamUtil.ReadUInt32(stream);
                    TempUstruct1.Slot7 = StreamUtil.ReadUInt32(stream);
                    EffectSlots.Add(TempUstruct1);
                }
                //Phyisics Objects
                PhysicsObjects = new List<UStruct2>();
                stream.Position = PhysicsOffset;
                for (int i = 0; i < PhysicsCount; i++)
                {
                    var TempUstruct2 = new UStruct2();

                    TempUstruct2.Offset = StreamUtil.ReadUInt32(stream);
                    TempUstruct2.ByteSize = StreamUtil.ReadUInt32(stream);
                    TempUstruct2.Count = StreamUtil.ReadUInt32(stream); //Probably?

                    var TempPos = stream.Position;
                    stream.Position = TempUstruct2.Offset;

                    //Inset Stuff Here

                    stream.Position = TempPos;

                    PhysicsObjects.Add(TempUstruct2);
                }
                //Collision Models
                CollisonModelPointers = new List<CollisonModelPointer>();
                stream.Position = CollisonModelOffset;
                for (int i = 0; i < CollisonModelCount; i++)
                {
                    var TempUstruct3 = new CollisonModelPointer();

                    TempUstruct3.Offset = StreamUtil.ReadUInt32(stream);
                    TempUstruct3.ByteSize = StreamUtil.ReadUInt32(stream);
                    TempUstruct3.Count = StreamUtil.ReadUInt32(stream); //Probably?

                    var TempPos = stream.Position;
                    stream.Position = TempUstruct3.Offset;

                    //Inset Stuff Here
                    TempUstruct3.Models = new List<CollisonModel>();
                    for (int a = 0; a < TempUstruct3.Count; a++)
                    {
                        var TempModel = new CollisonModel();
                        TempModel.FaceCount = StreamUtil.ReadUInt32(stream);
                        TempModel.VerticeCount = StreamUtil.ReadUInt32(stream);
                        TempModel.VerticeOffsetAlign = StreamUtil.ReadUInt32(stream);


                        TempModel.Index = new List<int>();
                        TempModel.Vertices = new List<Vector4>();
                        TempModel.FaceNormals = new List<Vector4>();

                        for (int b = 0; b < TempModel.FaceCount*3; b++)
                        {
                            TempModel.Index.Add(StreamUtil.ReadUInt32(stream));
                        }
                        stream.Position += TempModel.VerticeOffsetAlign;

                        //StreamUtil.AlignBy16(stream);

                        for (int b = 0; b < TempModel.VerticeCount; b++)
                        {
                            TempModel.Vertices.Add(StreamUtil.ReadVector4(stream));
                        }

                        for (int b = 0; b < TempModel.FaceCount; b++)
                        {
                            TempModel.FaceNormals.Add(StreamUtil.ReadVector4(stream));
                        }


                        TempUstruct3.Models.Add(TempModel);
                    }

                    stream.Position = TempPos;

                    CollisonModelPointers.Add(TempUstruct3);
                }

                EffectHeaders = new List<EffectHeaderStruct>();
                stream.Position = EffectsOffset; 
                for (int i = 0; i < EffectsCount; i++)
                {
                    var TempUstruct4 = new EffectHeaderStruct();
                    TempUstruct4.EffectCount = StreamUtil.ReadUInt32(stream); //Type
                    TempUstruct4.EffectOffset = StreamUtil.ReadUInt32(stream); //Offset
                    EffectHeaders.Add(TempUstruct4);
                }

                //Effects Start Here
                int EffectStartPos = (int)stream.Position;

                for (int i = 0; i < EffectHeaders.Count; i++)
                {
                    stream.Position = EffectHeaders[i].EffectOffset + EffectStartPos;
                    var TempEffect = EffectHeaders[i];
                    TempEffect.Effects = new List<Effect>();

                    for (int a = 0; a < TempEffect.EffectCount; a++)
                    {
                        var NewEffect = new Effect();
                        NewEffect.MainType = StreamUtil.ReadUInt32(stream);
                        NewEffect.ByteSize = StreamUtil.ReadUInt32(stream);
                        
                        //Debug Data
                        TempEffect.MainType = NewEffect.MainType;
                        TempEffect.ByteSize = NewEffect.ByteSize;
                        TempEffect.SubType = StreamUtil.ReadUInt32(stream);
                        stream.Position -= 4;
                        if (NewEffect.MainType == 0)
                        {
                            var NewMainType = new Type0();
                            NewMainType.SubType = StreamUtil.ReadUInt32(stream);

                            if (NewMainType.SubType == 0)
                            {
                                var NewSubType = new Type0Sub0();
                                NewSubType.U0 = StreamUtil.ReadFloat(stream);
                                NewSubType.U1 = StreamUtil.ReadFloat(stream);
                                NewSubType.U2 = StreamUtil.ReadFloat(stream);
                                NewSubType.U3 = StreamUtil.ReadFloat(stream);
                                NewSubType.U4 = StreamUtil.ReadFloat(stream);
                                NewSubType.U5 = StreamUtil.ReadFloat(stream);
                                NewMainType.type0Sub0 = NewSubType;
                            }
                            else 
                            if (NewMainType.SubType == 2)
                            {
                                NewMainType.type0Sub2 = StreamUtil.ReadUInt32(stream);
                            }
                            else
                            if (NewMainType.SubType == 5)
                            {
                                NewMainType.type0Sub5 = StreamUtil.ReadUInt32(stream);
                            }
                            else
                            if (NewMainType.SubType == 6)
                            {
                                var NewSubType = new Type0Sub6();
                                NewSubType.U0 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U1 = StreamUtil.ReadFloat(stream);
                                NewMainType.type0Sub6 = NewSubType;
                            }
                            else
                            if (NewMainType.SubType == 7)
                            {
                                var NewSubType = new Type0Sub7();
                                NewSubType.U0 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U1 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U2 = StreamUtil.ReadFloat(stream);
                                NewSubType.U3 = StreamUtil.ReadFloat(stream);
                                NewSubType.U4 = StreamUtil.ReadFloat(stream);
                                NewSubType.U5 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U6 = StreamUtil.ReadFloat(stream);
                                NewMainType.type0Sub7 = NewSubType;
                            }
                            else
                            if (NewMainType.SubType == 10)
                            {
                                var NewSubType = new Type0Sub10();
                                NewSubType.U0 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U1 = StreamUtil.ReadFloat(stream);
                                NewSubType.U2 = StreamUtil.ReadFloat(stream);
                                NewSubType.U3 = StreamUtil.ReadFloat(stream);
                                NewSubType.U4 = StreamUtil.ReadFloat(stream);
                                NewSubType.U5 = StreamUtil.ReadUInt32(stream);
                                NewMainType.type0Sub10 = NewSubType;
                            }
                            else
                            if (NewMainType.SubType == 11)
                            {
                                var NewSubType = new Type0Sub11();
                                NewSubType.U0 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U1 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U2 = StreamUtil.ReadFloat(stream);
                                NewSubType.U3 = StreamUtil.ReadFloat(stream);
                                NewSubType.U4 = StreamUtil.ReadUInt32(stream);
                                NewMainType.type0Sub11 = NewSubType;
                            }
                            else
                            if (NewMainType.SubType == 12)
                            {
                                var NewSubType = new Type0Sub12();
                                NewSubType.U0 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U1 = StreamUtil.ReadFloat(stream);
                                NewMainType.type0Sub12 = NewSubType;
                            }
                            else
                            if (NewMainType.SubType == 13)
                            {
                                var NewSubType = new Type0Sub13();
                                NewSubType.U0 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U1 = StreamUtil.ReadFloat(stream);
                                NewSubType.U2 = StreamUtil.ReadFloat(stream);
                                NewSubType.U3 = StreamUtil.ReadUInt32(stream);
                                NewMainType.type0Sub13 = NewSubType;
                            }
                            else
                            if (NewMainType.SubType == 14)
                            {
                                var NewSubType = new Type0Sub14();
                                NewSubType.U0 = StreamUtil.ReadFloat(stream);
                                NewSubType.U1 = StreamUtil.ReadFloat(stream);
                                NewMainType.type0Sub14 = NewSubType;
                            }
                            else
                            if (NewMainType.SubType == 15)
                            {
                                var NewSubType = new Type0Sub15();
                                NewSubType.U0 = StreamUtil.ReadFloat(stream);
                                NewSubType.U1 = StreamUtil.ReadFloat(stream);
                                NewSubType.U2 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U3 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U4 = StreamUtil.ReadFloat(stream);
                                NewMainType.type0Sub15 = NewSubType;
                            }
                            else
                            if (NewMainType.SubType == 17)
                            {
                                var NewSubType = new Type0Sub17();
                                NewSubType.U0 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U1 = StreamUtil.ReadUInt32(stream); 
                                NewSubType.U2 = StreamUtil.ReadUInt32(stream);
                                NewMainType.type0Sub17 = NewSubType;
                            }
                            else
                            if (NewMainType.SubType == 18)
                            {
                                var NewSubType = new Type0Sub18();
                                NewSubType.U0 = StreamUtil.ReadFloat(stream);
                                NewSubType.U1 = StreamUtil.ReadFloat(stream);
                                NewSubType.U2 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U3 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U4 = StreamUtil.ReadFloat(stream);
                                NewSubType.U5 = StreamUtil.ReadFloat(stream);
                                NewSubType.U6 = StreamUtil.ReadFloat(stream);
                                NewMainType.type0Sub18 = NewSubType;
                            }
                            else
                            if (NewMainType.SubType == 20)
                            {
                                var NewSubType = new Type0Sub20();
                                NewSubType.U0 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U1 = StreamUtil.ReadFloat(stream);
                                NewSubType.U2 = StreamUtil.ReadFloat(stream);
                                NewSubType.U3 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U4 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U5 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U6 = StreamUtil.ReadFloat(stream);
                                NewSubType.U7 = StreamUtil.ReadFloat(stream);
                                NewSubType.U8 = StreamUtil.ReadFloat(stream);
                                NewSubType.U9 = StreamUtil.ReadFloat(stream);

                                NewMainType.type0Sub20 = NewSubType;
                            }
                            else
                            if (NewMainType.SubType == 23)
                            {

                            }
                            else
                            if (NewMainType.SubType == 24)
                            {
                                var NewSubType = new Type0Sub24();
                                NewSubType.U0 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U1 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U2 = StreamUtil.ReadFloat(stream);
                                NewSubType.U3 = StreamUtil.ReadFloat(stream);
                                NewSubType.U4 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U5 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U6 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U7 = StreamUtil.ReadFloat(stream);
                                NewSubType.U8 = StreamUtil.ReadFloat(stream);
                                NewSubType.U9 = StreamUtil.ReadFloat(stream);
                                NewSubType.U10 = StreamUtil.ReadFloat(stream);
                                NewSubType.U11 = StreamUtil.ReadFloat(stream);
                                NewSubType.U12 = StreamUtil.ReadFloat(stream);
                                NewSubType.U13 = StreamUtil.ReadFloat(stream);
                                NewSubType.U14 = StreamUtil.ReadFloat(stream);
                                NewSubType.U15 = StreamUtil.ReadFloat(stream);
                                NewSubType.U16 = StreamUtil.ReadFloat(stream);
                                NewSubType.U17 = StreamUtil.ReadFloat(stream);
                                NewSubType.U18 = StreamUtil.ReadFloat(stream);

                                NewMainType.type0Sub24 = NewSubType;
                            }
                            else if (NewMainType.SubType == 256)
                            {
                                var NewSubType = new Type0Sub256();
                                NewSubType.U0 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U1 = StreamUtil.ReadFloat(stream);
                                NewSubType.U2 = StreamUtil.ReadFloat(stream);
                                NewSubType.U3 = StreamUtil.ReadFloat(stream);
                                NewSubType.U4 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U5 = StreamUtil.ReadFloat(stream);
                                NewSubType.U6 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U7 = StreamUtil.ReadUInt32(stream);

                                NewMainType.type0Sub256 = NewSubType;
                            }
                            else if (NewMainType.SubType == 257)
                            {
                                var NewSubType = new Type0Sub257();
                                NewSubType.U0 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U1 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U2 = StreamUtil.ReadFloat(stream);
                                NewSubType.U3 = StreamUtil.ReadFloat(stream);
                                NewSubType.U4 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U5 = StreamUtil.ReadFloat(stream);
                                NewSubType.U6 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U7 = StreamUtil.ReadUInt32(stream);

                                NewMainType.type0Sub257 = NewSubType;
                            }
                            else if (NewMainType.SubType == 258)
                            {
                                var NewSubType = new Type0Sub258();
                                NewSubType.U0 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U1 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U2 = StreamUtil.ReadFloat(stream);
                                NewSubType.U3 = StreamUtil.ReadFloat(stream);
                                NewSubType.U4 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U5 = StreamUtil.ReadFloat(stream);
                                NewSubType.U6 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U7 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U8 = StreamUtil.ReadFloat(stream);
                                NewSubType.U9 = StreamUtil.ReadFloat(stream);
                                NewSubType.U10 = StreamUtil.ReadFloat(stream);
                                NewSubType.U11 = StreamUtil.ReadUInt32(stream);

                                NewMainType.type0Sub258 = NewSubType;
                            }
                            else
                            {
                                Debug.WriteLine("Missing Type " + NewEffect.MainType.ToString() + "," + NewMainType.SubType.ToString());
                                break;
                            }
                            NewEffect.type0 = NewMainType;
                        }
                        else if (NewEffect.MainType == 2)
                        {
                            var NewMainType = new Type2();
                            NewMainType.SubType = StreamUtil.ReadUInt32(stream);

                            if(NewMainType.SubType == 0)
                            {
                                var NewSubType = new Type2Sub0();

                                NewSubType.U0 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U1 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U2 = StreamUtil.ReadFloat(stream);
                                NewSubType.U3 = StreamUtil.ReadFloat(stream);

                                NewSubType.U4 = StreamUtil.ReadFloat(stream);
                                NewSubType.U5 = StreamUtil.ReadFloat(stream);
                                NewSubType.U6 = StreamUtil.ReadFloat(stream);
                                NewSubType.U7 = StreamUtil.ReadFloat(stream);

                                NewSubType.U8 = StreamUtil.ReadFloat(stream);
                                NewSubType.U9 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U10 = StreamUtil.ReadFloat(stream);
                                NewSubType.U11 = StreamUtil.ReadFloat(stream);

                                NewSubType.U12 = StreamUtil.ReadFloat(stream);
                                NewSubType.U13 = StreamUtil.ReadFloat(stream);
                                NewSubType.U14 = StreamUtil.ReadFloat(stream);
                                NewSubType.U15 = StreamUtil.ReadFloat(stream);

                                NewSubType.U16 = StreamUtil.ReadFloat(stream);
                                NewSubType.U17 = StreamUtil.ReadFloat(stream);
                                NewSubType.U18 = StreamUtil.ReadFloat(stream);
                                NewSubType.U19 = StreamUtil.ReadFloat(stream);

                                NewSubType.U20 = StreamUtil.ReadFloat(stream);
                                NewSubType.U21 = StreamUtil.ReadFloat(stream);
                                NewSubType.U22 = StreamUtil.ReadFloat(stream);
                                NewSubType.U23 = StreamUtil.ReadFloat(stream);

                                NewSubType.U24 = StreamUtil.ReadFloat(stream);
                                NewSubType.U25 = StreamUtil.ReadFloat(stream);
                                NewSubType.U26 = StreamUtil.ReadFloat(stream);
                                NewSubType.U27 = StreamUtil.ReadFloat(stream);

                                NewSubType.U28 = StreamUtil.ReadFloat(stream);
                                NewSubType.U29 = StreamUtil.ReadFloat(stream);
                                NewSubType.U30 = StreamUtil.ReadFloat(stream);
                                NewSubType.U31 = StreamUtil.ReadFloat(stream);

                                NewSubType.U32 = StreamUtil.ReadFloat(stream);
                                NewSubType.U33 = StreamUtil.ReadFloat(stream);
                                NewSubType.U34 = StreamUtil.ReadFloat(stream);
                                NewSubType.U35 = StreamUtil.ReadFloat(stream);

                                NewSubType.U36 = StreamUtil.ReadFloat(stream);
                                NewSubType.U37 = StreamUtil.ReadFloat(stream);
                                NewSubType.U38 = StreamUtil.ReadFloat(stream); 
                                NewSubType.U39 = StreamUtil.ReadFloat(stream);

                                NewSubType.U40 = StreamUtil.ReadFloat(stream);
                                NewSubType.U41 = StreamUtil.ReadFloat(stream);
                                NewSubType.U42 = StreamUtil.ReadFloat(stream);
                                NewSubType.U43 = StreamUtil.ReadFloat(stream);

                                NewSubType.U44 = StreamUtil.ReadFloat(stream);
                                NewSubType.U45 = StreamUtil.ReadFloat(stream);
                                NewSubType.U46 = StreamUtil.ReadFloat(stream);
                                NewSubType.U47 = StreamUtil.ReadFloat(stream); 

                                NewSubType.U48 = StreamUtil.ReadFloat(stream);
                                NewSubType.U49 = StreamUtil.ReadUInt32(stream);
                                NewSubType.U50 = StreamUtil.ReadUInt32(stream); 

                                if(NewSubType.U9 != 0)
                                {
                                    Console.WriteLine("");
                                }

                                NewMainType.type2Sub0 = NewSubType;
                            }
                            else
                            {
                                Debug.WriteLine("Missing Type " + NewEffect.MainType.ToString() + "," + NewMainType.SubType.ToString());
                                break;
                            }

                            NewEffect.type2 = NewMainType;
                        }
                        else if (NewEffect.MainType == 3)
                        {
                            var NewMainType = new Type3();

                            NewMainType.U0 = StreamUtil.ReadUInt32(stream);
                            NewMainType.U1 = StreamUtil.ReadUInt32(stream);

                            NewEffect.type3 = NewMainType;
                        }
                        else if (NewEffect.MainType == 4)
                        {
                            NewEffect.type4 = StreamUtil.ReadFloat(stream);
                        }
                        else if (NewEffect.MainType == 5)
                        {
                            var NewMainType = new Type5();

                            NewMainType.U0 = StreamUtil.ReadUInt32(stream);
                            NewMainType.U1 = StreamUtil.ReadFloat(stream); //Not Actually Sure
                            NewMainType.U2 = StreamUtil.ReadUInt32(stream);

                            NewEffect.type5 = NewMainType;
                        }
                        else if (NewEffect.MainType == 7)
                        {
                            var NewMainType = new Type7();

                            NewMainType.U0 = StreamUtil.ReadUInt32(stream);
                            NewMainType.U1 = StreamUtil.ReadUInt32(stream);

                            NewEffect.type7 = NewMainType;
                        }
                        else if (NewEffect.MainType == 8)
                        {
                            NewEffect.type8 = StreamUtil.ReadUInt32(stream);
                        }
                        else if (NewEffect.MainType == 9)
                        {
                            var NewMainType = new Type9();

                            NewMainType.U0 = StreamUtil.ReadUInt32(stream);
                            NewMainType.U1 = StreamUtil.ReadFloat(stream);

                            NewEffect.type9 = NewMainType;
                        }
                        else if (NewEffect.MainType == 13)
                        {
                            NewEffect.type13 = StreamUtil.ReadFloat(stream);
                        }
                        else if (NewEffect.MainType == 14)
                        {
                            NewEffect.type14 = StreamUtil.ReadFloat(stream);
                        }
                        else if (NewEffect.MainType == 17)
                        {
                            NewEffect.type17 = StreamUtil.ReadFloat(stream);
                        }
                        else if (NewEffect.MainType == 18)
                        {
                            NewEffect.type18 = StreamUtil.ReadFloat(stream);
                        }
                        else if (NewEffect.MainType == 21)
                        {
                            NewEffect.type21 = StreamUtil.ReadUInt32(stream);
                        }
                        else if (NewEffect.MainType == 24)
                        {
                            NewEffect.type24 = StreamUtil.ReadUInt32(stream);
                        }
                        else
                        {
                            Debug.WriteLine("Missing Type " + NewEffect.MainType.ToString());
                            break;
                        }
                    }

                    EffectHeaders[i] = TempEffect;
                }



                Functions = new List<Function>();
                stream.Position = FunctionOffset;
                for (int i = 0; i < FunctionCount; i++)
                {
                    var TempFunction = new Function();
                    TempFunction.U1 = StreamUtil.ReadUInt32(stream);
                    TempFunction.U2 = StreamUtil.ReadUInt32(stream); //Offset
                    TempFunction.FunctionName = StreamUtil.ReadString(stream, 16);
                    Functions.Add(TempFunction);
                }

                //Appears to do nothing
                ObjectProperties = new List<UStruct5>();
                stream.Position = ObjectPropertiesOffset;
                for (int i = 0; i < ObjectPropertiesCount; i++)
                {
                    var TempUstruct5 = new UStruct5();
                    TempUstruct5.U1 = StreamUtil.ReadUInt8(stream);
                    TempUstruct5.U12 = StreamUtil.ReadUInt8(stream);
                    TempUstruct5.U13 = StreamUtil.ReadUInt8(stream);
                    TempUstruct5.U14 = StreamUtil.ReadUInt8(stream);


                    TempUstruct5.PlayerBounce = StreamUtil.ReadFloat(stream);
                    TempUstruct5.U3 = StreamUtil.ReadFloat(stream);

                    TempUstruct5.U4 = StreamUtil.ReadUInt32(stream); //-1

                    TempUstruct5.CollsionMode = StreamUtil.ReadInt16(stream);

                    if (TempUstruct5.CollsionMode != 3)
                    {
                        TempUstruct5.CollisonModelIndex = StreamUtil.ReadInt16(stream);
                        TempUstruct5.EffectSlotIndex = StreamUtil.ReadInt16(stream);
                    }
                    else
                    {
                        TempUstruct5.PhysicsIndex = StreamUtil.ReadInt16(stream);
                        TempUstruct5.EffectSlotIndex = StreamUtil.ReadInt16(stream);
                    }
                    TempUstruct5.U8 = StreamUtil.ReadInt16(stream);
                    ObjectProperties.Add(TempUstruct5);
                }

                //Breaks Collison, Animation, Material Visablity
                InstanceState = new List<int>();
                stream.Position = InstanceOffset;
                for (int i = 0; i < InstanceCount; i++)
                {
                    InstanceState.Add(StreamUtil.ReadUInt32(stream));
                }

                Splines = new List<Spline>();
                stream.Position = SplineOffset;
                for (int i = 0; i < SplineCount; i++)
                {
                    var TempUstruct6 = new Spline();
                    TempUstruct6.U1 = StreamUtil.ReadInt16(stream);
                    TempUstruct6.U2 = StreamUtil.ReadInt16(stream);
                    TempUstruct6.SplineStyle = StreamUtil.ReadUInt32(stream); 
                    Splines.Add(TempUstruct6);
                }

            }
        }

        public void SaveTest(string Path)
        {
            using (Stream stream = File.Open(Path, FileMode.Open))
            {
                stream.Position = InstanceOffset;
                for (int i = 0; i < InstanceCount; i++)
                {
                    StreamUtil.WriteInt32(stream, 84);
                }

            }
        }

        public void Save(string path)
        {
            MemoryStream stream = new MemoryStream();

            stream.Position = 76;

            EffectsOffset = (int)stream.Position;
            for (int i = 0; i < EffectSlots.Count; i++)
            {
                StreamUtil.WriteInt32(stream, EffectSlots[i].Slot1);
                StreamUtil.WriteInt32(stream, EffectSlots[i].Slot2);
                StreamUtil.WriteInt32(stream, EffectSlots[i].Slot3);
                StreamUtil.WriteInt32(stream, EffectSlots[i].Slot4);
                StreamUtil.WriteInt32(stream, EffectSlots[i].Slot5);
                StreamUtil.WriteInt32(stream, EffectSlots[i].Slot6);
                StreamUtil.WriteInt32(stream, EffectSlots[i].Slot7);
            }


            PhysicsOffset = (int)stream.Position;
            //Skip passed it all and write shit
            stream.Position += (PhysicsObjects.Count * 4 * 3) + (CollisonModelPointers.Count * 4 * 3) + (Functions.Count*16 + Functions.Count*2*4) + (EffectHeaders.Count*4*2);



            stream.Position = PhysicsOffset;
            for (int i = 0; i < PhysicsObjects.Count; i++)
            {
                StreamUtil.WriteInt32(stream, PhysicsObjects[i].Offset);
                StreamUtil.WriteInt32(stream, PhysicsObjects[i].ByteSize);
                StreamUtil.WriteInt32(stream, PhysicsObjects[i].Count);
            }

            CollisonModelOffset = (int)stream.Position;
            for (int i = 0; i < CollisonModelPointers.Count; i++)
            {
                StreamUtil.WriteInt32(stream, CollisonModelPointers[i].Offset);
                StreamUtil.WriteInt32(stream, CollisonModelPointers[i].ByteSize);
                StreamUtil.WriteInt32(stream, CollisonModelPointers[i].Count);
            }

            FunctionOffset = (int)stream.Position;
            for (int i = 0; i < Functions.Count; i++)
            {
                StreamUtil.WriteInt32(stream, Functions[i].U1);
                StreamUtil.WriteInt32(stream, Functions[i].U2);
                StreamUtil.WriteString(stream, Functions[i].FunctionName, 16);
            }

            EffectsOffset = (int)stream.Position;
            for (int i = 0; i < EffectHeaders.Count; i++)
            {
                StreamUtil.WriteInt32(stream, EffectHeaders[i].EffectCount);
                StreamUtil.WriteInt32(stream, EffectHeaders[i].EffectOffset);
            }

            ObjectPropertiesOffset = (int)stream.Position;
            for (int i = 0; i < ObjectProperties.Count; i++)
            {
                var TempUstruct5 = ObjectProperties[i];

                StreamUtil.WriteUInt8(stream, TempUstruct5.U1);
                StreamUtil.WriteUInt8(stream, TempUstruct5.U12);
                StreamUtil.WriteUInt8(stream, TempUstruct5.U13);
                StreamUtil.WriteUInt8(stream, TempUstruct5.U14);

                StreamUtil.WriteFloat32(stream, TempUstruct5.PlayerBounce);
                StreamUtil.WriteFloat32(stream, TempUstruct5.U3);

                StreamUtil.WriteInt32(stream, TempUstruct5.U4);

                StreamUtil.WriteInt16(stream, TempUstruct5.CollsionMode);

                if (TempUstruct5.CollsionMode != 3)
                {
                    StreamUtil.WriteInt16(stream, TempUstruct5.CollisonModelIndex);
                    StreamUtil.WriteInt16(stream, TempUstruct5.EffectSlotIndex);
                }
                else
                {
                    StreamUtil.WriteInt16(stream, TempUstruct5.PhysicsIndex);
                    StreamUtil.WriteInt16(stream, TempUstruct5.EffectSlotIndex);
                }
                StreamUtil.WriteInt16(stream, TempUstruct5.U8);
            }

            InstanceOffset = (int)stream.Position;
            for (int i = 0; i < InstanceState.Count; i++)
            {
                StreamUtil.WriteInt32(stream, InstanceState[i]);
            }

            SplineOffset = (int)stream.Position;
            for (int i = 0; i < Splines.Count; i++)
            {
                StreamUtil.WriteInt16(stream, Splines[i].U1);
                StreamUtil.WriteInt16(stream, Splines[i].U2);
                StreamUtil.WriteInt16(stream, Splines[i].SplineStyle);
            }


            stream.Position = 0;

            StreamUtil.WriteInt32(stream, U1);
            StreamUtil.WriteInt32(stream, U2);
            StreamUtil.WriteFloat32(stream, U3);
            StreamUtil.WriteInt32(stream, EffectSlots.Count);
            StreamUtil.WriteInt32(stream, EffectSlotsOffset);
            StreamUtil.WriteInt32(stream, PhysicsObjects.Count);
            StreamUtil.WriteInt32(stream, PhysicsOffset);
            StreamUtil.WriteInt32(stream, CollisonModelPointers.Count);
            StreamUtil.WriteInt32(stream, CollisonModelOffset);
            StreamUtil.WriteInt32(stream, EffectHeaders.Count);
            StreamUtil.WriteInt32(stream, EffectsOffset);
            StreamUtil.WriteInt32(stream, Functions.Count);
            StreamUtil.WriteInt32(stream, FunctionOffset);
            StreamUtil.WriteInt32(stream, ObjectProperties.Count);
            StreamUtil.WriteInt32(stream, ObjectPropertiesOffset);
            StreamUtil.WriteInt32(stream, InstanceState.Count);
            StreamUtil.WriteInt32(stream, InstanceOffset);
            StreamUtil.WriteInt32(stream, Splines.Count);
            StreamUtil.WriteInt32(stream, SplineOffset);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            var file = File.Create(path);
            stream.Position = 0;
            stream.CopyTo(file);
            stream.Dispose();
            file.Close();
        }

        public void SaveModels(string Path)
        {
            int c = 0;
            for (int a = 0; a < CollisonModelPointers.Count; a++)
            {
                var TempCollision = CollisonModelPointers[a];

                for (int ax = 0; ax < TempCollision.Models.Count; ax++)
                {
                    var Data = TempCollision.Models[ax];

                    string outputString = "";
                    string output = "# Exported From SSX Using SSX Multitool Modder by GlitcherOG \n";

                    outputString += "o Mesh" + c + "\n";
                    for (int b = 0; b < Data.FaceCount; b++)
                    {
                        outputString += "f " + (Data.Index[3*b]+1) + "//" + (b+1).ToString() + " " + (Data.Index[3*b+1]+1) + "//" + (b+1).ToString() + " " + (Data.Index[3*b+2]+1) + "//" + (b+1).ToString() + "\n";
                    }

                    for (int z = 0; z < Data.Vertices.Count; z++)
                    {
                        output += "v " + Data.Vertices[z].X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Data.Vertices[z].Y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Data.Vertices[z].Z.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\n";
                    }
                    for (int z = 0; z < Data.FaceNormals.Count; z++)
                    {
                        output += "vn " + Data.FaceNormals[z].X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Data.FaceNormals[z].Y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Data.FaceNormals[z].Z.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\n";
                    }
                    output += outputString;
                    File.WriteAllText(Path + "/" + c + ".obj", output);
                    c++;
                }

            }

        }


        public struct EffectSlot
        {
            public int Slot1;
            public int Slot2;
            public int Slot3;
            public int Slot4;
            public int Slot5;
            public int Slot6;
            public int Slot7;
        }

        public struct UStruct2
        {
            public int Offset;
            public int ByteSize;
            public int Count;
        }

        public struct CollisonModelPointer
        {
            public int Offset;
            public int ByteSize;
            public int Count; //Maybe?

            public List<CollisonModel> Models;
        }

        public struct CollisonModel
        {
            public int FaceCount;
            public int VerticeCount;
            public int VerticeOffsetAlign;

            public List<int> Index;
            public List<Vector4> Vertices;
            public List<Vector4> FaceNormals;
        }

        public struct Function
        {
            public int U1;
            public int U2;
            public string FunctionName;
        }

        public struct UStruct5
        {
            public int U1;
            public int U12;
            public int U13;
            public int U14;

            public float PlayerBounce;
            public float U3;

            public int U4;
            public int CollsionMode;
            public int CollisonModelIndex;
            public int EffectSlotIndex;

            public int PhysicsIndex;

            public int U8;
        }

        public struct Spline
        {
            public int U1;
            public int U2;
            public int SplineStyle;
        }

        public struct EffectHeaderStruct
        {
            public int EffectCount; //Count
            public int EffectOffset; //Offset

            public List<Effect> Effects;

            //Debug Info
            public int MainType;
            public int SubType;
            public int ByteSize;
        }

        public struct Effect
        {
            public int MainType;
            public int ByteSize;

            public Type0? type0;
            public Type2? type2;
            public Type3? type3;
            public float type4;
            public Type5? type5;
            public Type7? type7;
            public int type8;
            public Type9? type9;

            public float type13;
            public float type14;
            public float type17;
            public float type18;
            public int type21;
            public int type24;
        }

        #region Type0
        public struct Type0
        {
            public int SubType;

            public Type0Sub0? type0Sub0;
            public int type0Sub2; 
            public int type0Sub5;
            public Type0Sub6? type0Sub6;
            public Type0Sub7? type0Sub7;
            public Type0Sub10? type0Sub10;
            public Type0Sub11? type0Sub11;
            public Type0Sub12? type0Sub12;
            public Type0Sub13? type0Sub13;
            public Type0Sub14? type0Sub14;
            public Type0Sub15? type0Sub15;
            public Type0Sub17? type0Sub17;
            public Type0Sub18? type0Sub18;
            public Type0Sub20? type0Sub20;
            public Type0Sub24? type0Sub24;
            public Type0Sub256? type0Sub256;
            public Type0Sub257? type0Sub257;
            public Type0Sub258? type0Sub258;
        }

        public struct Type0Sub0
        {
            public float U0;
            public float U1;
            public float U2;
            public float U3;
            public float U4;
            public float U5;
        }

        public struct Type0Sub6
        {
            public int U0;
            public float U1;
        }

        public struct Type0Sub7
        {
            public int U0;
            public int U1;
            public float U2;
            public float U3;
            public float U4;
            public int U5;
            public float U6;
        }

        public struct Type0Sub10
        {
            public int U0;
            public float U1;
            public float U2;
            public float U3;
            public float U4;
            public int U5;
        }

        public struct Type0Sub11
        {
            public int U0;
            public int U1;
            public float U2;
            public float U3;
            public int U4;
        }

        public struct Type0Sub12
        {
            public int U0;
            public float U1;
        }

        public struct Type0Sub13
        {
            public int U0;
            public float U1;
            public float U2;
            public int U3;
        }

        public struct Type0Sub14
        {
            public float U0;
            public float U1;
        }

        public struct Type0Sub15
        {
            public float U0;
            public float U1;
            public int U2;
            public int U3;
            public float U4;
        }

        public struct Type0Sub17
        {
            public int U0;
            public int U1;
            public int U2;
        }

        public struct Type0Sub18
        {
            public float U0;
            public float U1;
            public int U2;
            public int U3;
            public float U4;
            public float U5;
            public float U6;
        }

        public struct Type0Sub20
        {
            public int U0;
            public float U1;
            public float U2;
            public int U3;
            public int U4;
            public int U5;
            public float U6;
            public float U7;
            public float U8;
            public float U9;
        }

        public struct Type0Sub24
        {
            public int U0;
            public int U1;
            public float U2;
            public float U3;
            public int U4;
            public int U5;
            public int U6;
            public float U7;
            public float U8;
            public float U9;
            public float U10;
            public float U11;
            public float U12;
            public float U13;
            public float U14;
            public float U15;
            public float U16;
            public float U17;
            public float U18;
        }

        public struct Type0Sub256
        {
            public int U0;
            public float U1;
            public float U2;
            public float U3;
            public int U4;
            public float U5;
            public int U6;
            public int U7;
        }

        public struct Type0Sub257
        {
            public int U0;
            public int U1;
            public float U2;
            public float U3;
            public int U4;
            public float U5;
            public int U6;
            public int U7;
        }

        public struct Type0Sub258
        {
            public int U0;
            public int U1;
            public float U2;
            public float U3;
            public int U4;
            public float U5;
            public int U6;
            public int U7;
            public float U8;
            public float U9;
            public float U10;
            public int U11;
        }
        #endregion

        public struct Type3
        {
            public int U0;
            public int U1;
        }

        public struct Type5
        {
            public int U0;
            public float U1;
            public int U2;
        }

        #region Type2
        public struct Type2
        {
            public int SubType;

            public Type2Sub0? type2Sub0;
        }

        public struct Type2Sub0
        {
            public int U0;
            public int U1;
            public float U2;
            public float U3;
            public float U4;
            public float U5;
            public float U6;
            public float U7;
            public float U8;
            public int U9;
            public float U10;
            public float U11;
            public float U12;
            public float U13;
            public float U14;
            public float U15;
            public float U16;
            public float U17;
            public float U18;
            public float U19;
            public float U20;
            public float U21;
            public float U22;
            public float U23;
            public float U24;
            public float U25;
            public float U26;
            public float U27;
            public float U28;
            public float U29;
            public float U30;
            public float U31;
            public float U32;
            public float U33;
            public float U34;
            public float U35;
            public float U36;
            public float U37;
            public float U38;
            public float U39;
            public float U40;
            public float U41;
            public float U42;
            public float U43;
            public float U44;
            public float U45;
            public float U46;
            public float U47;
            public float U48;
            public int U49;
            public int U50;
        }
        #endregion

        public struct Type7
        {
            public int U0;
            public int U1;
        }

        public struct Type9
        {
            public int U0;
            public float U1;
        }
    }
}


//sub_type:
//0: # "Roller"?
//    'fff'
//    'iii'
//2: # "Debounce"?
//5: # "DeadNode"? b = 2, 4
//6: # "Counter"
//7: # "Boost" 
//8: # "Timer"
//9: # "Rail"
//10: # "UVScroll" b = 0
//    'ii'
//    'f'
//11: # "TexFlip" #print(f.tell())
//12: # "Fence"
//13: # "Flag"
//14: # "Cracked"
//15: # "LapBoost"
//16: # "RandomBoost"
//17: # "CrowdBox"
//18: # "ZBoost"
//19: # "UVScrollTexFlip"
//20: # "cMeshAnim"
//21: # "TrickTrigger"
//22: # "Particle"
//23: # "Movie"
//24: # "TubeEndBoost"

//256: # "AnimObject"
//257: # "AnimDelta"
//258: # "AnimCombo"
//259: # "AnimTexFlip"