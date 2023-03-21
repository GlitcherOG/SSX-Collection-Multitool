using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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
        public int UStruct2Count;
        public int UStruct2Offset; 
        public int CollisonModelCount;
        public int CollisonModelOffset;
        public int EffectsCount;
        public int EffectsOffset;
        public int FunctionCount;
        public int FunctionOffset;
        public int UStruct5Count;
        public int UStruct5Offset;
        public int InstanceCount;
        public int InstanceOffset;
        public int SplineCount;
        public int SplineOffset;

        public List<EffectSlot> EffectSlots = new List<EffectSlot>();
        public List<UStruct2> uStruct2s = new List<UStruct2>();
        public List<CollisonModelPointer> CollisonModelPointers = new List<CollisonModelPointer>();
        public List<EffectStruct> Effects = new List<EffectStruct>();
        public List<Function> Functions = new List<Function>();
        public List<UStruct5> uStruct5s = new List<UStruct5>();
        public List<Spline> Splines = new List<Spline>();
        public List<int> InstanceState = new List<int>();

        public void Load(string Path)
        {
            using (Stream stream = File.Open(Path, FileMode.Open))
            {
                U1 = StreamUtil.ReadInt32(stream);
                U2 = StreamUtil.ReadInt32(stream);
                U3 = StreamUtil.ReadFloat(stream);
                EffectSlotsCount = StreamUtil.ReadInt32(stream);
                EffectSlotsOffset = StreamUtil.ReadInt32(stream);
                UStruct2Count = StreamUtil.ReadInt32(stream);
                UStruct2Offset = StreamUtil.ReadInt32(stream);
                CollisonModelCount = StreamUtil.ReadInt32(stream);
                CollisonModelOffset = StreamUtil.ReadInt32(stream);
                EffectsCount = StreamUtil.ReadInt32(stream);
                EffectsOffset = StreamUtil.ReadInt32(stream);
                FunctionCount = StreamUtil.ReadInt32(stream);
                FunctionOffset = StreamUtil.ReadInt32(stream);
                UStruct5Count = StreamUtil.ReadInt32(stream);
                UStruct5Offset = StreamUtil.ReadInt32(stream);
                InstanceCount = StreamUtil.ReadInt32(stream);
                InstanceOffset = StreamUtil.ReadInt32(stream);
                SplineCount = StreamUtil.ReadInt32(stream);
                SplineOffset = StreamUtil.ReadInt32(stream);

                EffectSlots = new List<EffectSlot>();
                stream.Position = EffectSlotsOffset;
                for (int i = 0; i < EffectSlotsCount; i++)
                {
                    var TempUstruct1 = new EffectSlot();

                    TempUstruct1.Slot1 = StreamUtil.ReadInt32(stream);
                    TempUstruct1.Slot2 = StreamUtil.ReadInt32(stream);
                    TempUstruct1.Slot3 = StreamUtil.ReadInt32(stream);
                    TempUstruct1.Slot4 = StreamUtil.ReadInt32(stream);
                    TempUstruct1.Slot5 = StreamUtil.ReadInt32(stream);
                    TempUstruct1.Slot6 = StreamUtil.ReadInt32(stream);
                    TempUstruct1.Slot7 = StreamUtil.ReadInt32(stream);
                    EffectSlots.Add(TempUstruct1);
                }
                //Phyisics Objects
                uStruct2s = new List<UStruct2>();
                stream.Position = UStruct2Offset;
                for (int i = 0; i < UStruct2Count; i++)
                {
                    var TempUstruct2 = new UStruct2();

                    TempUstruct2.Offset = StreamUtil.ReadInt32(stream);
                    TempUstruct2.ByteSize = StreamUtil.ReadInt32(stream);
                    TempUstruct2.Count = StreamUtil.ReadInt32(stream); //Probably?

                    var TempPos = stream.Position;
                    stream.Position = TempUstruct2.Offset;

                    //Inset Stuff Here


                    stream.Position = TempPos;

                    uStruct2s.Add(TempUstruct2);
                }
                //Collision Models
                CollisonModelPointers = new List<CollisonModelPointer>();
                stream.Position = CollisonModelOffset;
                for (int i = 0; i < CollisonModelCount; i++)
                {
                    var TempUstruct3 = new CollisonModelPointer();

                    TempUstruct3.Offset = StreamUtil.ReadInt32(stream);
                    TempUstruct3.ByteSize = StreamUtil.ReadInt32(stream);
                    TempUstruct3.Count = StreamUtil.ReadInt32(stream); //Probably?

                    var TempPos = stream.Position;
                    stream.Position = TempUstruct3.Offset;

                    //Inset Stuff Here
                    TempUstruct3.Models = new List<CollisonModel>();
                    for (int a = 0; a < TempUstruct3.Count; a++)
                    {
                        var TempModel = new CollisonModel();
                        TempModel.FaceCount = StreamUtil.ReadInt32(stream);
                        TempModel.VerticeCount = StreamUtil.ReadInt32(stream);
                        TempModel.VerticeOffsetAlign = StreamUtil.ReadInt32(stream);


                        TempModel.Index = new List<int>();
                        TempModel.Vertices = new List<Vector4>();
                        TempModel.FaceNormals = new List<Vector4>();

                        for (int b = 0; b < TempModel.FaceCount*3; b++)
                        {
                            TempModel.Index.Add(StreamUtil.ReadInt32(stream));
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
                
                Effects = new List<EffectStruct>();
                stream.Position = EffectsOffset; 
                for (int i = 0; i < EffectsCount; i++)
                {
                    var TempUstruct4 = new EffectStruct();
                    TempUstruct4.U1 = StreamUtil.ReadInt32(stream); //Type
                    TempUstruct4.U2 = StreamUtil.ReadInt32(stream); //Offset
                    Effects.Add(TempUstruct4);
                }

                Functions = new List<Function>();
                stream.Position = FunctionOffset;
                for (int i = 0; i < FunctionCount; i++)
                {
                    var TempFunction = new Function();
                    TempFunction.U1 = StreamUtil.ReadInt32(stream);
                    TempFunction.U2 = StreamUtil.ReadInt32(stream); //Offset
                    TempFunction.FunctionName = StreamUtil.ReadString(stream, 16);
                    Functions.Add(TempFunction);
                }

                //Appears to do nothing
                uStruct5s = new List<UStruct5>();
                stream.Position = UStruct5Offset;
                for (int i = 0; i < UStruct5Count; i++)
                {
                    var TempUstruct5 = new UStruct5();
                    TempUstruct5.U1 = StreamUtil.ReadUInt8(stream);
                    TempUstruct5.U12 = StreamUtil.ReadUInt8(stream);
                    TempUstruct5.U13 = StreamUtil.ReadUInt8(stream);
                    TempUstruct5.U14 = StreamUtil.ReadUInt8(stream);


                    TempUstruct5.PlayerBounce = StreamUtil.ReadFloat(stream);
                    TempUstruct5.U3 = StreamUtil.ReadFloat(stream);

                    TempUstruct5.U4 = StreamUtil.ReadInt32(stream); //-1

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
                    uStruct5s.Add(TempUstruct5);
                }

                //Breaks Collison, Animation, Material Visablity
                InstanceState = new List<int>();
                stream.Position = InstanceOffset;
                for (int i = 0; i < InstanceCount; i++)
                {
                    InstanceState.Add(StreamUtil.ReadInt32(stream));
                }

                Splines = new List<Spline>();
                stream.Position = SplineOffset;
                for (int i = 0; i < SplineCount; i++)
                {
                    var TempUstruct6 = new Spline();
                    TempUstruct6.U1 = StreamUtil.ReadInt16(stream);
                    TempUstruct6.U2 = StreamUtil.ReadInt16(stream);
                    TempUstruct6.SplineStyle = StreamUtil.ReadInt32(stream); 
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

            
            UStruct2Offset = (int)stream.Position;
            //Skip passed it all and write shit
            stream.Position += (uStruct2s.Count * 4 * 3) + (CollisonModelPointers.Count * 4 * 3) + (Functions.Count*16 + Functions.Count*2*4) + (Effects.Count*4*2);



            stream.Position = UStruct2Offset;
            for (int i = 0; i < uStruct2s.Count; i++)
            {
                StreamUtil.WriteInt32(stream, uStruct2s[i].Offset);
                StreamUtil.WriteInt32(stream, uStruct2s[i].ByteSize);
                StreamUtil.WriteInt32(stream, uStruct2s[i].Count);
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
            for (int i = 0; i < Effects.Count; i++)
            {
                StreamUtil.WriteInt32(stream, Effects[i].U1);
                StreamUtil.WriteInt32(stream, Effects[i].U2);
            }






            stream.Position = 0;

            StreamUtil.WriteInt32(stream, U1);
            StreamUtil.WriteInt32(stream, U2);
            StreamUtil.WriteFloat32(stream, U3);
            StreamUtil.WriteInt32(stream, EffectSlots.Count);
            StreamUtil.WriteInt32(stream, EffectSlotsOffset);
            StreamUtil.WriteInt32(stream, uStruct2s.Count);
            StreamUtil.WriteInt32(stream, UStruct2Offset);
            StreamUtil.WriteInt32(stream, CollisonModelPointers.Count);
            StreamUtil.WriteInt32(stream, CollisonModelOffset);
            StreamUtil.WriteInt32(stream, Effects.Count);
            StreamUtil.WriteInt32(stream, EffectsOffset);
            StreamUtil.WriteInt32(stream, Functions.Count);
            StreamUtil.WriteInt32(stream, FunctionOffset);
            StreamUtil.WriteInt32(stream, uStruct5s.Count);
            StreamUtil.WriteInt32(stream, UStruct5Offset);
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
                        output += "v " + Data.Vertices[z].X + " " + Data.Vertices[z].Y + " " + Data.Vertices[z].Z + "\n";
                    }
                    for (int z = 0; z < Data.FaceNormals.Count; z++)
                    {
                        output += "vn " + Data.FaceNormals[z].X + " " + Data.FaceNormals[z].Y + " " + Data.FaceNormals[z].Z + "\n";
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

        public struct EffectStruct
        {
            public int U1; //Count
            public int U2; //Offset
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