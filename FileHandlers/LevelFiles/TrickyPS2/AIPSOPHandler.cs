using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;

namespace SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2
{
    public class AIPSOPHandler
    {
        int pathAOffset;
        int pathBOffset;
        int pathACount;

        //PathBStuff
        int Unknown1;
        int Unknown2;
        int pathBCount;
        public int pathBUnknown;

        public List<PathTypeA> typeAs = new List<PathTypeA>();
        public List<PathTypeB> typeBs = new List<PathTypeB>();


        public void LoadAIPSOP(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                stream.Position += 0x08;

                pathAOffset = StreamUtil.ReadUInt32(stream);
                pathBOffset = StreamUtil.ReadUInt32(stream);
                pathACount = StreamUtil.ReadUInt32(stream);

                //Skip to path A
                stream.Position = 0x30;
                typeAs = new List<PathTypeA>();

                for (int i = 0; i < pathACount; i++)
                {
                    var TempTypeA = new PathTypeA();
                    TempTypeA.Unknown1 = StreamUtil.ReadUInt32(stream);
                    TempTypeA.Unknown2 = StreamUtil.ReadUInt32(stream);
                    TempTypeA.Unknown3 = StreamUtil.ReadUInt32(stream);
                    TempTypeA.Unknown4 = StreamUtil.ReadUInt32(stream);
                    TempTypeA.Unknown5 = StreamUtil.ReadUInt32(stream);
                    TempTypeA.Unknown6 = StreamUtil.ReadUInt32(stream);
                    TempTypeA.Unknown7 = StreamUtil.ReadUInt32(stream);

                    TempTypeA.pathPointsCount = StreamUtil.ReadUInt32(stream);
                    TempTypeA.UnknownCount = StreamUtil.ReadUInt32(stream);

                    TempTypeA.pathPos = StreamUtil.ReadVector3(stream);
                    TempTypeA.bboxMin = StreamUtil.ReadVector3(stream);
                    TempTypeA.bboxMax = StreamUtil.ReadVector3(stream);

                    //Original Points
                    TempTypeA.vectorPoints = new List<Vector4>();
                    for (int a = 0; a < TempTypeA.pathPointsCount; a++)
                    {
                        Vector4 Direction = StreamUtil.ReadVector4(stream);
                        TempTypeA.vectorPoints.Add(Direction);
                    }

                    ////Converted to proper Vectors
                    //List<Vector3> Points = new List<Vector3>();
                    //for (int a = 0; a < TempTypeA.vectorPoints.Count; a++)
                    //{
                    //    Points.Add(new Vector3(TempTypeA.vectorPoints[a].X * TempTypeA.vectorPoints[a].W, TempTypeA.vectorPoints[a].Y * TempTypeA.vectorPoints[a].W, TempTypeA.vectorPoints[a].Z * TempTypeA.vectorPoints[a].W));
                    //}

                    ////Generate other two values based on which value is higher
                    //List<Vector4> NewVectorPoints = new List<Vector4>();
                    //for (int a = 0; a < Points.Count; a++)
                    //{
                    //    if (TempTypeA.vectorPoints[a].X > TempTypeA.vectorPoints[a].Y && TempTypeA.vectorPoints[a].X > TempTypeA.vectorPoints[a].Z)
                    //    {
                    //        Vector4 NewVector = new Vector4(TempTypeA.vectorPoints[a].X, 0,0, TempTypeA.vectorPoints[a].W);
                    //        NewVector.Y = (Points[a].Y / Points[a].X) * NewVector.X;
                    //        NewVector.Z = (Points[a].Z / Points[a].X) * NewVector.X;
                    //        NewVectorPoints.Add(NewVector);
                    //    }

                    //    if (TempTypeA.vectorPoints[a].Y > TempTypeA.vectorPoints[a].Z && TempTypeA.vectorPoints[a].Y > TempTypeA.vectorPoints[a].X)
                    //    {
                    //        Vector4 NewVector = new Vector4(0, TempTypeA.vectorPoints[a].Y, 0, TempTypeA.vectorPoints[a].W);
                    //        NewVector.X = (Points[a].X / Points[a].Y) * NewVector.Y;
                    //        NewVector.Z = (Points[a].Z / Points[a].Y) * NewVector.Y;
                    //        NewVectorPoints.Add(NewVector);
                    //    }

                    //    if (TempTypeA.vectorPoints[a].Z > TempTypeA.vectorPoints[a].Y && TempTypeA.vectorPoints[a].Z > TempTypeA.vectorPoints[a].X)
                    //    {
                    //        Vector4 NewVector = new Vector4(0, 0, TempTypeA.vectorPoints[a].Z, TempTypeA.vectorPoints[a].W);
                    //        NewVector.X = (Points[a].X / Points[a].Z) * NewVector.Z;
                    //        NewVector.Y = (Points[a].Y / Points[a].Z) * NewVector.Z;
                    //        NewVectorPoints.Add(NewVector);
                    //    }
                    //}


                    TempTypeA.unkownListTypeAs = new List<UnkownListTypeA>();
                    for (int a = 0; a < TempTypeA.UnknownCount; a++)
                    {
                        var TempUnknownListTypeA = new UnkownListTypeA();
                        TempUnknownListTypeA.Unknown1 = StreamUtil.ReadUInt32(stream);
                        TempUnknownListTypeA.Unknown2 = StreamUtil.ReadUInt32(stream);
                        TempUnknownListTypeA.Unknown3 = StreamUtil.ReadFloat(stream);
                        TempUnknownListTypeA.Unknown4 = StreamUtil.ReadFloat(stream);
                        TempTypeA.unkownListTypeAs.Add(TempUnknownListTypeA);
                    }
                    typeAs.Add(TempTypeA);
                }

                //Skip to PathB
                stream.Position = pathBOffset + 16;
                Unknown1 = StreamUtil.ReadUInt32(stream);
                Unknown2 = StreamUtil.ReadUInt32(stream);
                pathBCount = StreamUtil.ReadUInt32(stream);
                pathBUnknown = StreamUtil.ReadUInt32(stream);

                typeBs = new List<PathTypeB>();
                for (int i = 0; i < pathBCount; i++)
                {
                    var TempTypeB = new PathTypeB();
                    TempTypeB.Unknown1 = StreamUtil.ReadUInt32(stream);
                    TempTypeB.Unknown2 = StreamUtil.ReadUInt32(stream);
                    TempTypeB.Unknown3 = StreamUtil.ReadUInt32(stream);
                    TempTypeB.Unknown4 = StreamUtil.ReadFloat(stream);

                    TempTypeB.pathPointsCount = StreamUtil.ReadUInt32(stream);
                    TempTypeB.UnknownCount = StreamUtil.ReadUInt32(stream);

                    TempTypeB.pathPos = StreamUtil.ReadVector3(stream);
                    TempTypeB.bboxMin = StreamUtil.ReadVector3(stream);
                    TempTypeB.bboxMax = StreamUtil.ReadVector3(stream);

                    TempTypeB.vectorPoints = new List<Vector4>();
                    for (int a = 0; a < TempTypeB.pathPointsCount; a++)
                    {
                        Vector4 Direction = StreamUtil.ReadVector4(stream);
                        TempTypeB.vectorPoints.Add(Direction);
                    }

                    TempTypeB.unkownListTypeAs = new List<UnkownListTypeA>();
                    for (int a = 0; a < TempTypeB.UnknownCount; a++)
                    {
                        var TempUnknownListTypeA = new UnkownListTypeA();
                        TempUnknownListTypeA.Unknown1 = StreamUtil.ReadUInt32(stream);
                        TempUnknownListTypeA.Unknown2 = StreamUtil.ReadUInt32(stream);
                        TempUnknownListTypeA.Unknown3 = StreamUtil.ReadFloat(stream);
                        TempUnknownListTypeA.Unknown4 = StreamUtil.ReadFloat(stream);
                        TempTypeB.unkownListTypeAs.Add(TempUnknownListTypeA);
                    }
                    typeBs.Add(TempTypeB);
                }

            }

        }

        public void SaveAIPSOP(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                stream.Position += 0x08;

                pathAOffset = StreamUtil.ReadUInt32(stream);
                pathBOffset = StreamUtil.ReadUInt32(stream);
                pathACount = StreamUtil.ReadUInt32(stream);

                //Skip to path A
                stream.Position = 0x30;

                for (int i = 0; i < typeAs.Count; i++)
                {
                    var TempTypeA = typeAs[i];
                    StreamUtil.WriteInt32(stream, TempTypeA.Unknown1);
                    StreamUtil.WriteInt32(stream, TempTypeA.Unknown2);
                    StreamUtil.WriteInt32(stream, TempTypeA.Unknown3);
                    StreamUtil.WriteInt32(stream, TempTypeA.Unknown4);
                    StreamUtil.WriteInt32(stream, TempTypeA.Unknown5);
                    StreamUtil.WriteInt32(stream, TempTypeA.Unknown6);
                    StreamUtil.WriteInt32(stream, TempTypeA.Unknown7);

                    StreamUtil.WriteInt32(stream, TempTypeA.vectorPoints.Count);
                    StreamUtil.WriteInt32(stream, TempTypeA.unkownListTypeAs.Count);

                    StreamUtil.WriteVector3(stream, TempTypeA.pathPos);
                    StreamUtil.WriteVector3(stream, TempTypeA.bboxMin);
                    StreamUtil.WriteVector3(stream, TempTypeA.bboxMax);

                    for (int a = 0; a < TempTypeA.vectorPoints.Count; a++)
                    {
                        StreamUtil.WriteVector4(stream, TempTypeA.vectorPoints[a]);
                    }

                    for (int a = 0; a < TempTypeA.unkownListTypeAs.Count; a++)
                    {
                        var TempUnknownListTypeA = TempTypeA.unkownListTypeAs[a];
                        StreamUtil.WriteInt32(stream, TempUnknownListTypeA.Unknown1);
                        StreamUtil.WriteInt32(stream, TempUnknownListTypeA.Unknown2);
                        StreamUtil.WriteFloat32(stream, TempUnknownListTypeA.Unknown3);
                        StreamUtil.WriteFloat32(stream, TempUnknownListTypeA.Unknown4);
                    }
                }

                //Skip to PathB
                stream.Position = pathBOffset + 16;
                Unknown1 = StreamUtil.ReadUInt32(stream);
                Unknown2 = StreamUtil.ReadUInt32(stream);
                pathBCount = StreamUtil.ReadUInt32(stream);
                pathBUnknown = StreamUtil.ReadUInt32(stream);

                for (int i = 0; i < typeBs.Count; i++)
                {
                    var TempTypeB = typeBs[i];
                    StreamUtil.WriteInt32(stream, TempTypeB.Unknown1);
                    StreamUtil.WriteInt32(stream, TempTypeB.Unknown2);
                    StreamUtil.WriteInt32(stream, TempTypeB.Unknown3);
                    StreamUtil.WriteFloat32(stream, TempTypeB.Unknown4);

                    StreamUtil.WriteInt32(stream, TempTypeB.vectorPoints.Count);
                    StreamUtil.WriteInt32(stream, TempTypeB.unkownListTypeAs.Count);

                    StreamUtil.WriteVector3(stream, TempTypeB.pathPos);
                    StreamUtil.WriteVector3(stream, TempTypeB.bboxMin);
                    StreamUtil.WriteVector3(stream, TempTypeB.bboxMax);

                    for (int a = 0; a < TempTypeB.vectorPoints.Count; a++)
                    {
                        StreamUtil.WriteVector4(stream, TempTypeB.vectorPoints[a]);
                    }

                    for (int a = 0; a < TempTypeB.unkownListTypeAs.Count; a++)
                    {
                        var TempUnknownListTypeA = TempTypeB.unkownListTypeAs[a];
                        StreamUtil.WriteInt32(stream, TempUnknownListTypeA.Unknown1);
                        StreamUtil.WriteInt32(stream, TempUnknownListTypeA.Unknown2);
                        StreamUtil.WriteFloat32(stream, TempUnknownListTypeA.Unknown3);
                        StreamUtil.WriteFloat32(stream, TempUnknownListTypeA.Unknown4);
                    }
                }
            }

        }


        public struct PathTypeA
        {
            public int Unknown1;
            public int Unknown2;
            public int Unknown3;
            public int Unknown4;
            public int Unknown5;
            public int Unknown6;
            public int Unknown7;

            public int pathPointsCount;
            public int UnknownCount;

            public Vector3 pathPos;
            public Vector3 bboxMin;
            public Vector3 bboxMax;

            public List<Vector4> vectorPoints;
            public List<UnkownListTypeA> unkownListTypeAs;

        }

        public struct PathTypeB
        {
            public int Unknown1;
            public int Unknown2;
            public int Unknown3;
            public float Unknown4;

            public int pathPointsCount;
            public int UnknownCount;

            public Vector3 pathPos;
            public Vector3 bboxMin;
            public Vector3 bboxMax;

            public List<Vector4> vectorPoints;
            public List<UnkownListTypeA> unkownListTypeAs;
        }

        public struct UnkownListTypeA
        {
            public int Unknown1;
            public int Unknown2;
            public float Unknown3;
            public float Unknown4;
        }
    }
}
