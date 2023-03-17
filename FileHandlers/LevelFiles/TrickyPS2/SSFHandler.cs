using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2
{
    public class SSFHandler
    {
        public int U1;
        public int U2;
        public float U3;
        public int UStruct1Count;
        public int UStruct1Offset;
        public int UStruct2Count;
        public int UStruct2Offset; 
        public int CollisonModelCount;
        public int CollisonModelOffset;
        public int UStruct4Count;
        public int UStruct4Offset;
        public int FunctionCount;
        public int FunctionOffset;
        public int UStruct5Count;
        public int UStruct5Offset;
        public int InstanceCount;
        public int InstanceOffset;
        public int SplineCount;
        public int SplineOffset;

        public List<UStruct1> uStruct1s = new List<UStruct1>();
        public List<UStruct2> uStruct2s = new List<UStruct2>();
        public List<CollisonModelPointer> CollisonModelPointers = new List<CollisonModelPointer>();
        public List<UStruct4> uStruct4s = new List<UStruct4>();
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
                UStruct1Count = StreamUtil.ReadInt32(stream);
                UStruct1Offset = StreamUtil.ReadInt32(stream);
                UStruct2Count = StreamUtil.ReadInt32(stream);
                UStruct2Offset = StreamUtil.ReadInt32(stream);
                CollisonModelCount = StreamUtil.ReadInt32(stream);
                CollisonModelOffset = StreamUtil.ReadInt32(stream);
                UStruct4Count = StreamUtil.ReadInt32(stream);
                UStruct4Offset = StreamUtil.ReadInt32(stream);
                FunctionCount = StreamUtil.ReadInt32(stream);
                FunctionOffset = StreamUtil.ReadInt32(stream);
                UStruct5Count = StreamUtil.ReadInt32(stream);
                UStruct5Offset = StreamUtil.ReadInt32(stream);
                InstanceCount = StreamUtil.ReadInt32(stream);
                InstanceOffset = StreamUtil.ReadInt32(stream);
                SplineCount = StreamUtil.ReadInt32(stream);
                SplineOffset = StreamUtil.ReadInt32(stream);

                //Nothing?
                uStruct1s = new List<UStruct1>();
                stream.Position = UStruct1Offset;
                for (int i = 0; i < UStruct1Count; i++)
                {
                    var TempUstruct1 = new UStruct1();

                    TempUstruct1.U1 = StreamUtil.ReadInt32(stream);
                    TempUstruct1.U2 = StreamUtil.ReadInt32(stream);
                    TempUstruct1.U3 = StreamUtil.ReadInt32(stream);
                    TempUstruct1.U4 = StreamUtil.ReadInt32(stream);
                    TempUstruct1.U5 = StreamUtil.ReadInt32(stream);
                    TempUstruct1.U6 = StreamUtil.ReadInt32(stream);
                    TempUstruct1.U7 = StreamUtil.ReadInt32(stream);
                    uStruct1s.Add(TempUstruct1);
                }
                //Phyisics Objects
                uStruct2s = new List<UStruct2>();
                stream.Position = UStruct2Offset;
                for (int i = 0; i < UStruct2Count; i++)
                {
                    var TempUstruct2 = new UStruct2();

                    TempUstruct2.Offset = StreamUtil.ReadInt32(stream);
                    TempUstruct2.ByteSize = StreamUtil.ReadInt32(stream);
                    TempUstruct2.Count = StreamUtil.ReadInt32(stream);

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
                    TempUstruct3.Count = StreamUtil.ReadInt32(stream);

                    var TempPos = stream.Position;
                    stream.Position = TempUstruct3.Offset;

                    //Inset Stuff Here
                    TempUstruct3.Models = new List<CollisonModel>();
                    for (int a = 0; a < TempUstruct3.Count; a++)
                    {
                        var TempModel = new CollisonModel();
                        TempModel.FaceCount = StreamUtil.ReadInt32(stream);
                        TempModel.VerticeCount = StreamUtil.ReadInt32(stream);
                        TempModel.U1 = StreamUtil.ReadInt32(stream);


                        TempModel.Index = new List<int>();
                        TempModel.Vertices = new List<Vector4>();
                        TempModel.Unkowns = new List<Vector4>();

                        for (int b = 0; b < TempModel.FaceCount*3; b++)
                        {
                            TempModel.Index.Add(StreamUtil.ReadInt32(stream));
                        }
                        StreamUtil.AlignBy16(stream);

                        for (int b = 0; b < TempModel.VerticeCount; b++)
                        {
                            TempModel.Vertices.Add(StreamUtil.ReadVector4(stream));
                        }

                        for (int b = 0; b < TempModel.FaceCount; b++)
                        {
                            TempModel.Unkowns.Add(StreamUtil.ReadVector4(stream));
                        }


                        TempUstruct3.Models.Add(TempModel);
                    }

                    stream.Position = TempPos;

                    CollisonModelPointers.Add(TempUstruct3);
                }
                //Animation Stuff in someway
                uStruct4s = new List<UStruct4>();
                stream.Position = UStruct4Offset; 
                for (int i = 0; i < UStruct4Count; i++)
                {
                    var TempUstruct4 = new UStruct4();
                    TempUstruct4.U1 = StreamUtil.ReadInt32(stream); //Count
                    TempUstruct4.U2 = StreamUtil.ReadInt32(stream); //Offset
                    uStruct4s.Add(TempUstruct4);
                }

                Functions = new List<Function>();
                stream.Position = FunctionOffset;
                for (int i = 0; i < FunctionCount; i++)
                {
                    var TempFunction = new Function();
                    TempFunction.U1 = StreamUtil.ReadInt32(stream);
                    TempFunction.U2 = StreamUtil.ReadInt32(stream);
                    TempFunction.FunctionName = StreamUtil.ReadString(stream, 16);
                    Functions.Add(TempFunction);
                }

                //Appears to do nothing
                uStruct5s = new List<UStruct5>();
                stream.Position = UStruct5Offset;
                for (int i = 0; i < UStruct5Count; i++)
                {
                    var TempUstruct5 = new UStruct5();
                    TempUstruct5.U1 = StreamUtil.ReadInt32(stream);
                    TempUstruct5.U2 = StreamUtil.ReadInt32(stream);
                    TempUstruct5.U3 = StreamUtil.ReadInt32(stream);
                    TempUstruct5.U4 = StreamUtil.ReadInt32(stream);
                    TempUstruct5.U5 = StreamUtil.ReadInt32(stream);
                    TempUstruct5.U6 = StreamUtil.ReadInt32(stream);
                    uStruct5s.Add(TempUstruct5);
                }

                //Breaks Collison, Animation, Material Visablity
                InstanceState = new List<int>();
                stream.Position = InstanceOffset;
                for (int i = 0; i < InstanceCount; i++)
                {
                    InstanceState.Add(StreamUtil.ReadInt32(stream));
                }

                //0 - Nothing
                //12 - Smoking
                //36 - Invisible no Collision
                //56 - Nothing


                Splines = new List<Spline>();
                stream.Position = SplineOffset;
                for (int i = 0; i < SplineCount; i++)
                {
                    var TempUstruct6 = new Spline();
                    TempUstruct6.U1 = StreamUtil.ReadInt16(stream);
                    TempUstruct6.U2 = StreamUtil.ReadInt16(stream);
                    TempUstruct6.SplineStyle = StreamUtil.ReadInt32(stream); //Spline Style
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
                    StreamUtil.WriteInt32(stream, 128);
                }

            }
        }

        public struct UStruct1
        {
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
            public int U7;
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
            public int Count;

            public List<CollisonModel> Models;
        }

        public struct CollisonModel
        {
            public int FaceCount;
            public int VerticeCount;
            public int U1;

            public List<int> Index;
            public List<Vector4> Vertices;
            public List<Vector4> Unkowns;
        }

        public struct UStruct4
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
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
        }

        public struct Spline
        {
            public int U1;
            public int U2;
            public int SplineStyle;
        }
    }
}
