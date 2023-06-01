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
using System.Windows.Media;
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
        public List<PhysicsHeader> PhysicsHeaders = new List<PhysicsHeader>();
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

                    TempUstruct1.Slot1 = StreamUtil.ReadUInt32(stream); //Constant
                    TempUstruct1.Slot2 = StreamUtil.ReadUInt32(stream); //Collsion
                    TempUstruct1.Slot3 = StreamUtil.ReadUInt32(stream);
                    TempUstruct1.Slot4 = StreamUtil.ReadUInt32(stream);
                    TempUstruct1.Slot5 = StreamUtil.ReadUInt32(stream); //Effect Triggers (Effects and cause these to happen)
                    TempUstruct1.Slot6 = StreamUtil.ReadUInt32(stream);
                    TempUstruct1.Slot7 = StreamUtil.ReadUInt32(stream);
                    EffectSlots.Add(TempUstruct1);
                }
                //Phyisics Objects
                PhysicsHeaders = new List<PhysicsHeader>();
                stream.Position = PhysicsOffset;
                for (int i = 0; i < PhysicsCount; i++)
                {
                    var TempUstruct2 = new PhysicsHeader();

                    TempUstruct2.Offset = StreamUtil.ReadUInt32(stream);
                    TempUstruct2.ByteSize = StreamUtil.ReadUInt32(stream);
                    TempUstruct2.Count = StreamUtil.ReadUInt32(stream); //Probably?

                    var TempPos = stream.Position;
                    stream.Position = TempUstruct2.Offset;
                    TempUstruct2.PhysicsDatas = new List<PhysicsData>();
                    for (int a = 0; a < TempUstruct2.Count; a++)
                    {
                        var NewPhysicsData = new PhysicsData();

                        NewPhysicsData.U0 = StreamUtil.ReadUInt32(stream);
                        NewPhysicsData.U1 = StreamUtil.ReadUInt32(stream);
                        NewPhysicsData.U2 = StreamUtil.ReadUInt32(stream);
                        NewPhysicsData.U3 = StreamUtil.ReadUInt32(stream);

                        if(NewPhysicsData.U2!=0&& NewPhysicsData.U2 != 1)
                        {
                            Console.WriteLine("");
                        }

                        NewPhysicsData.UFloat0 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat1 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat2 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat3 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat4 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat5 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat6 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat7 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat8 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat9 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat10 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat11 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat12 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat13 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat14 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat15 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat16 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat17 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat18 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat19 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat20 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat21 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat22 = StreamUtil.ReadFloat(stream);
                        NewPhysicsData.UFloat23 = StreamUtil.ReadFloat(stream);

                        NewPhysicsData.uPhysicsStruct0 = new List<UPhysicsStruct>();

                        for (int b = 0; b < NewPhysicsData.U3 + 1; b++)
                        {
                            var NewPhysicsStruct = new UPhysicsStruct();

                            NewPhysicsStruct.U0 = StreamUtil.ReadFloat(stream);
                            NewPhysicsStruct.U1 = StreamUtil.ReadFloat(stream);
                            NewPhysicsStruct.U2 = StreamUtil.ReadUInt32(stream);

                            NewPhysicsData.uPhysicsStruct0.Add(NewPhysicsStruct);
                        }

                        NewPhysicsData.UByteData = StreamUtil.ReadBytes(stream, NewPhysicsData.U1);
                        NewPhysicsData.UByteEndData = StreamUtil.ReadBytes(stream, NewPhysicsData.U0);

                        TempUstruct2.PhysicsDatas.Add(NewPhysicsData);
                    }

                    if(stream.Position-TempUstruct2.Offset!= TempUstruct2.ByteSize)
                    {
                        Debug.WriteLine("Error " + TempUstruct2.Offset);
                    }

                    stream.Position = TempPos;

                    PhysicsHeaders.Add(TempUstruct2);
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

                Functions = new List<Function>();
                stream.Position = FunctionOffset;
                for (int i = 0; i < FunctionCount; i++)
                {
                    var TempFunction = new Function();
                    TempFunction.Count = StreamUtil.ReadUInt32(stream);
                    TempFunction.Offset = StreamUtil.ReadUInt32(stream); //Offset
                    TempFunction.FunctionName = StreamUtil.ReadString(stream, 16);
                    Functions.Add(TempFunction);
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
                        var NewEffect = LoadEffectsData(stream);

                        if(NewEffect==null)
                        {
                            break;
                        }

                        TempEffect.Effects.Add(NewEffect.Value);
                    }

                    EffectHeaders[i] = TempEffect;
                }

                for (int i = 0; i < Functions.Count; i++)
                {
                    var Function = Functions[i];
                    stream.Position = Function.Offset + EffectStartPos;
                    Function.Effects = new List<Effect>();

                    for (int a = 0; a < Function.Count; a++)
                    {
                        var NewEffect = LoadEffectsData(stream);

                        if (NewEffect == null)
                        {
                            break;
                        }

                        Function.Effects.Add(NewEffect.Value);
                    }

                    Functions[i] = Function;
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

                    TempUstruct5.U2 = StreamUtil.ReadUInt8(stream);
                    TempUstruct5.U22 = StreamUtil.ReadUInt8(stream);
                    TempUstruct5.U23 = StreamUtil.ReadUInt8(stream);
                    TempUstruct5.U24 = StreamUtil.ReadUInt8(stream);

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

        public Effect? LoadEffectsData(Stream stream)
        {
            var NewEffect = new Effect();

            //Debug Info
            NewEffect.Offset = (int)stream.Position;

            NewEffect.MainType = StreamUtil.ReadUInt32(stream);
            NewEffect.ByteSize = StreamUtil.ReadUInt32(stream);

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
                    NewMainType.DeadNodeMode = StreamUtil.ReadUInt32(stream);
                }
                else
                if (NewMainType.SubType == 6)
                {
                    var NewSubType = new CounterEffect(); //Counter
                    NewSubType.Count = StreamUtil.ReadUInt32(stream); //Count
                    NewSubType.U1 = StreamUtil.ReadFloat(stream); 
                    NewMainType.Counter = NewSubType;
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
                    var NewSubType = new UVScrolling();
                    NewSubType.U0 = StreamUtil.ReadUInt32(stream); //Scroll Mode 
                    NewSubType.U1 = StreamUtil.ReadFloat(stream); //Horozontal Scroll
                    NewSubType.U2 = StreamUtil.ReadFloat(stream); //Vertical Scroll
                    NewSubType.U3 = StreamUtil.ReadFloat(stream); //Horozontal Scroll Length
                    NewSubType.U4 = StreamUtil.ReadFloat(stream); //Vertical Scroll Length
                    NewSubType.U5 = StreamUtil.ReadUInt32(stream);
                    NewMainType.UVScroll = NewSubType;
                }
                else
                if (NewMainType.SubType == 11)
                {
                    var NewSubType = new TextureFlipEffect();
                    NewSubType.U0 = StreamUtil.ReadUInt32(stream); //Nothing
                    NewSubType.Direction = StreamUtil.ReadUInt32(stream); //Direction 0-Forwards, Anything else-backwards
                    NewSubType.Speed = StreamUtil.ReadFloat(stream);
                    NewSubType.Length = StreamUtil.ReadFloat(stream);
                    NewSubType.U4 = StreamUtil.ReadUInt32(stream); //Pauses on frame? Display pattern?
                    NewMainType.TextureFlip = NewSubType;
                }
                else
                if (NewMainType.SubType == 12)
                {
                    var NewSubType = new FenceFlex();
                    NewSubType.U0 = StreamUtil.ReadUInt32(stream); //Flex Mode?
                    NewSubType.FlexAmmount = StreamUtil.ReadFloat(stream);
                    NewMainType.Fence = NewSubType;
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
                    var NewSubType = new CrowdBox();
                    NewSubType.U0 = StreamUtil.ReadUInt32(stream);
                    //Both Effect How Many Rows Of People
                    NewSubType.U1 = StreamUtil.ReadUInt32(stream); //Row
                    NewSubType.U2 = StreamUtil.ReadUInt32(stream); //Column
                    NewMainType.CrowdEffect = NewSubType;
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
                    //Movie, Contains no actual data
                } //Done
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
                    return null;
                }
                NewEffect.type0 = NewMainType;
            }
            else if (NewEffect.MainType == 2)
            {
                var NewMainType = new Type2();
                NewMainType.SubType = StreamUtil.ReadUInt32(stream);

                if (NewMainType.SubType == 0)
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

                    NewMainType.type2Sub0 = NewSubType;
                }
                else if (NewMainType.SubType == 1)
                {
                    var NewSubType = new Type2Sub1();

                    NewSubType.U0 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U1 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U2 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U3 = StreamUtil.ReadUInt32(stream);

                    NewSubType.U4 = StreamUtil.ReadFloat(stream);
                    NewSubType.U5 = StreamUtil.ReadFloat(stream);
                    NewSubType.U6 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U7 = StreamUtil.ReadUInt32(stream);

                    NewSubType.U8 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U9 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U10 = StreamUtil.ReadUInt32(stream);

                    NewMainType.type2Sub1 = NewSubType;
                }
                else if (NewMainType.SubType == 2)
                {
                    var NewSubType = new Type2Sub2();

                    NewSubType.U0 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U1 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U2 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U3 = StreamUtil.ReadUInt32(stream);

                    NewSubType.U4 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U5 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U6 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U7 = StreamUtil.ReadUInt32(stream);

                    NewSubType.U8 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U9 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U10 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U11 = StreamUtil.ReadUInt32(stream);

                    NewSubType.U12 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U13 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U14 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U15 = StreamUtil.ReadUInt32(stream);

                    NewSubType.U16 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U17 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U18 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U19 = StreamUtil.ReadUInt32(stream);

                    NewSubType.U20 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U21 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U22 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U23 = StreamUtil.ReadUInt32(stream);

                    NewSubType.U24 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U25 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U26 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U27 = StreamUtil.ReadUInt32(stream);

                    NewSubType.U28 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U29 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U30 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U31 = StreamUtil.ReadUInt32(stream);

                    NewSubType.U32 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U33 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U34 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U35 = StreamUtil.ReadUInt32(stream);

                    NewSubType.U36 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U37 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U38 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U39 = StreamUtil.ReadUInt32(stream);

                    NewSubType.U40 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U41 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U42 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U43 = StreamUtil.ReadUInt32(stream);

                    NewSubType.U44 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U45 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U46 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U47 = StreamUtil.ReadUInt32(stream);

                    NewSubType.U48 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U49 = StreamUtil.ReadUInt32(stream);
                    NewSubType.U50 = StreamUtil.ReadUInt32(stream);

                    NewMainType.type2Sub2 = NewSubType;
                }
                else
                {
                    Debug.WriteLine("Missing Type " + NewEffect.MainType.ToString() + "," + NewMainType.SubType.ToString());
                    return null;
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
                NewEffect.WaitTime = StreamUtil.ReadFloat(stream);
            } //Done
            else if (NewEffect.MainType == 5)
            {
                var NewMainType = new Type5();

                NewMainType.U0 = StreamUtil.ReadUInt32(stream);
                NewMainType.U1 = StreamUtil.ReadFloat(stream); //Not Actually Sure
                NewMainType.U2 = StreamUtil.ReadUInt32(stream);

                NewEffect.type5 = NewMainType;
            }
            else if (NewEffect.MainType == 7) //Done
            {
                var NewMainType = new InstanceEffect();

                NewMainType.InstanceIndex = StreamUtil.ReadUInt32(stream);
                NewMainType.EffectIndex = StreamUtil.ReadUInt32(stream);

                NewEffect.Instance = NewMainType;
            } //Done
            else if (NewEffect.MainType == 8) //Done
            {
                NewEffect.SoundPlay = StreamUtil.ReadUInt32(stream);
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
                NewEffect.MultiplierScore = StreamUtil.ReadFloat(stream);
            } //Done
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
                NewEffect.FunctionRunIndex = StreamUtil.ReadUInt32(stream);
            } //Done
            else if (NewEffect.MainType == 24)
            {
                NewEffect.TeleportInstanceIndex = StreamUtil.ReadUInt32(stream);
            } //Done
            else if (NewEffect.MainType == 25)
            {
                var NewMainType = new SplineEffect();

                NewMainType.SplineIndex = StreamUtil.ReadUInt32(stream);
                NewMainType.Effect = StreamUtil.ReadUInt32(stream);

                NewEffect.Spline = NewMainType;
            } //Done
            else
            {
                Debug.WriteLine("Missing Type " + NewEffect.MainType.ToString());
                return null;
            }

            return NewEffect;
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
            stream.Position += (PhysicsHeaders.Count * 4 * 3) + (CollisonModelPointers.Count * 4 * 3) + (Functions.Count*16 + Functions.Count*2*4) + (EffectHeaders.Count*4*2);



            stream.Position = PhysicsOffset;
            for (int i = 0; i < PhysicsHeaders.Count; i++)
            {
                StreamUtil.WriteInt32(stream, PhysicsHeaders[i].Offset);
                StreamUtil.WriteInt32(stream, PhysicsHeaders[i].ByteSize);
                StreamUtil.WriteInt32(stream, PhysicsHeaders[i].Count);
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
                StreamUtil.WriteInt32(stream, Functions[i].Count);
                StreamUtil.WriteInt32(stream, Functions[i].Offset);
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

                StreamUtil.WriteUInt8(stream, TempUstruct5.U2);
                StreamUtil.WriteUInt8(stream, TempUstruct5.U22);
                StreamUtil.WriteUInt8(stream, TempUstruct5.U23);
                StreamUtil.WriteUInt8(stream, TempUstruct5.U24);

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
            StreamUtil.WriteInt32(stream, PhysicsHeaders.Count);
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

        public struct PhysicsHeader
        {
            public int Offset;
            public int ByteSize;
            public int Count;

            public List<PhysicsData> PhysicsDatas;
        }

        public struct PhysicsData
        {
            public int U0; //End Bytes
            public int U1; //End Data
            public int U2; //Struct 1
            public int U3; //Struct 2

            public float UFloat0;
            public float UFloat1;
            public float UFloat2;
            public float UFloat3;
            public float UFloat4;
            public float UFloat5;
            public float UFloat6;
            public float UFloat7;
            public float UFloat8;
            public float UFloat9;
            public float UFloat10;
            public float UFloat11;
            public float UFloat12;
            public float UFloat13;
            public float UFloat14;
            public float UFloat15;
            public float UFloat16;
            public float UFloat17;
            public float UFloat18;
            public float UFloat19;
            public float UFloat20;
            public float UFloat21;
            public float UFloat22;
            public float UFloat23;

            public List<UPhysicsStruct> uPhysicsStruct0;

            public byte[] UByteData;
            public byte[] UByteEndData;
        }

        public struct UPhysicsStruct
        {
            public float U0;
            public float U1;
            public int U2;
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
            public int Count;
            public int Offset;
            public string FunctionName;

            public List<Effect> Effects;
        }

        public struct UStruct5
        {
            public int U1;
            public int U12;
            public int U13;
            public int U14;

            public float PlayerBounce;

            public int U2;
            public int U22;
            public int U23;
            public int U24;

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
        }

        #region Effects
        public struct Effect
        {
            //Debug Info
            public int Offset;

            public int MainType;
            public int ByteSize;


            //4 - Wait
            //13 - Reset
            //14 - Multiplyer

            //16 - Sound (ZBxsfx_31)
            //17 - Boost
            //18 - Trick Boost

            //8 - PlaySound
            //7 - Instance Effects
            //21 - Function Run

            //24 - Teleport
            //25 - Spline Effects

            public Type0? type0;
            public Type2? type2;
            public Type3? type3;
            public float WaitTime;
            public Type5? type5;
            public InstanceEffect? Instance;
            public int SoundPlay;
            public Type9? type9;

            public float type13; //Reset Value
            public float MultiplierScore;
            public float type17;
            public float type18;
            public int FunctionRunIndex; //Script Used By Screenlogo
            public int TeleportInstanceIndex;
            public SplineEffect? Spline;
        }

        #region Type0
        public struct Type0
        {
            public int SubType;

            //5 - DeadNode (Add, Destroy, Remove, Enable)
            //6 - Counter
            //10 - UV Scroll
            //11 - Texture Flipping
            //12 - FenceFlex
            //17 - Crowd
            //23 - Movie?

            public Type0Sub0? type0Sub0; ///Roller?
            public int type0Sub2;  //Debounce?
            public int DeadNodeMode; //5
            public CounterEffect? Counter; //6
            public Type0Sub7? type0Sub7; //Boost
            public UVScrolling? UVScroll; //10
            public TextureFlipEffect? TextureFlip; //11
            public FenceFlex? Fence; //12
            public Type0Sub13? type0Sub13; //Flag
            public Type0Sub14? type0Sub14; //Cracked
            public Type0Sub15? type0Sub15; //LapBoost
            public CrowdBox? CrowdEffect; //17
            public Type0Sub18? type0Sub18; //ZBoost
            public Type0Sub20? type0Sub20; //cMeshAnim
            public Type0Sub24? type0Sub24; //TubeEndBoost
            public Type0Sub256? type0Sub256; //AnimObject
            public Type0Sub257? type0Sub257; //AnimDelta
            public Type0Sub258? type0Sub258; //AnimCombo
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

        public struct CounterEffect
        {
            public int Count;
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

        public struct UVScrolling
        {
            public int U0;
            public float U1;
            public float U2;
            public float U3;
            public float U4;
            public int U5;
        }

        public struct TextureFlipEffect
        {
            public int U0;
            public int Direction;
            public float Speed;
            public float Length;
            public int U4;
        }

        public struct FenceFlex
        {
            public int U0;
            public float FlexAmmount;
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

        public struct CrowdBox
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

        #region Type2
        public struct Type2
        {
            public int SubType;

            public Type2Sub0? type2Sub0; //Emitter
            public Type2Sub1? type2Sub1; //SplinePath
            public Type2Sub2? type2Sub2; //CollideEmitter
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

        public struct Type2Sub1
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public float U4;
            public float U5;
            public int U6;
            public int U7;
            public int U8;
            public int U9;
            public int U10;
        }

        public struct Type2Sub2
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

        public struct InstanceEffect
        {
            public int InstanceIndex;
            public int EffectIndex;
        }

        public struct Type9
        {
            public int U0;
            public float U1;
        }

        public struct SplineEffect
        {
            public int SplineIndex;
            public int Effect;

            //0 - Toggle Off
            //1 - Toggle On
        }

        #endregion
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