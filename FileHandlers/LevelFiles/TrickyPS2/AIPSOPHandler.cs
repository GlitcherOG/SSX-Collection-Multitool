using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using System.Windows.Automation;

namespace SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2
{
    public class AIPSOPHandler
    {
        public byte[] MagicBytes = new byte[4] { 0x0A, 0x0A, 0x0A, 0x0A };
        public int PathTypeCount = 2;
        public int TypeAOffset;
        public int TypeBOffset;

        public TypeA AIPath;
        public TypeB RaceLine;

        public void LoadAIPSOP(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                MagicBytes = StreamUtil.ReadBytes(stream, 4);
                PathTypeCount = StreamUtil.ReadUInt32(stream);
                TypeAOffset = StreamUtil.ReadUInt32(stream);
                TypeBOffset = StreamUtil.ReadUInt32(stream);

                AIPath = new TypeA();
                AIPath.PathACount = StreamUtil.ReadUInt32(stream);
                AIPath.StartPosCount = StreamUtil.ReadUInt32(stream);

                AIPath.StartPosList = new List<int>();
                for (int i = 0; i < AIPath.StartPosCount; i++)
                {
                    AIPath.StartPosList.Add(StreamUtil.ReadUInt32(stream));
                }

                AIPath.PathAs = new List<PathA>();
                for (int i = 0; i < AIPath.PathACount; i++)
                {
                    var NewPath = new PathA();
                    NewPath.Type = StreamUtil.ReadUInt32(stream);
                    NewPath.U1 = StreamUtil.ReadUInt32(stream);
                    NewPath.U2 = StreamUtil.ReadUInt32(stream);
                    NewPath.U3 = StreamUtil.ReadUInt32(stream);
                    NewPath.U4 = StreamUtil.ReadUInt32(stream);
                    NewPath.U5 = StreamUtil.ReadUInt32(stream);
                    NewPath.Respawnable = StreamUtil.ReadUInt32(stream);

                    NewPath.NumPoints = StreamUtil.ReadUInt32(stream);
                    NewPath.NumEvents = StreamUtil.ReadUInt32(stream);

                    NewPath.PathPos = StreamUtil.ReadVector3(stream);
                    NewPath.BBoxMin = StreamUtil.ReadVector3(stream);
                    NewPath.BBoxMax = StreamUtil.ReadVector3(stream);

                    NewPath.VectorPoints = new List<Vector4>();
                    for (int a = 0; a < NewPath.NumPoints; a++)
                    {
                        NewPath.VectorPoints.Add(StreamUtil.ReadVector4(stream));
                    }

                    NewPath.PathEvents = new List<PathEvent>();
                    for (int a = 0; a < NewPath.NumEvents; a++)
                    {
                        var NewUStrcut = new PathEvent();
                        NewUStrcut.EventType = StreamUtil.ReadUInt32(stream);
                        NewUStrcut.EventValue = StreamUtil.ReadUInt32(stream);
                        NewUStrcut.EventStart = StreamUtil.ReadFloat(stream);
                        NewUStrcut.EventEnd = StreamUtil.ReadFloat(stream);
                        NewPath.PathEvents.Add(NewUStrcut);
                    }

                    AIPath.PathAs.Add(NewPath);
                }

                stream.Position = TypeBOffset + 16;
                RaceLine = new TypeB();
                RaceLine.U0 = StreamUtil.ReadUInt32(stream);
                RaceLine.ByteSize = StreamUtil.ReadUInt32(stream);
                RaceLine.PathBCount = StreamUtil.ReadUInt32(stream);
                RaceLine.PathBUnknown = StreamUtil.ReadUInt32(stream);

                RaceLine.PathBs = new List<PathB>();
                for (int i = 0; i < RaceLine.PathBCount; i++)
                {
                    var NewPath = new PathB();
                    NewPath.Type = StreamUtil.ReadUInt32(stream);
                    NewPath.U0 = StreamUtil.ReadUInt32(stream);
                    NewPath.U1 = StreamUtil.ReadUInt32(stream);
                    NewPath.U2 = StreamUtil.ReadFloat(stream);

                    NewPath.NumPoints = StreamUtil.ReadUInt32(stream);
                    NewPath.NumEvents = StreamUtil.ReadUInt32(stream);

                    NewPath.PathPos = StreamUtil.ReadVector3(stream);
                    NewPath.BBoxMin = StreamUtil.ReadVector3(stream);
                    NewPath.BBoxMax = StreamUtil.ReadVector3(stream);

                    NewPath.VectorPoints = new List<Vector4>();
                    for (int a = 0; a < NewPath.NumPoints; a++)
                    {
                        NewPath.VectorPoints.Add(StreamUtil.ReadVector4(stream));
                    }

                    NewPath.PathEvents = new List<PathEvent>();
                    for (int a = 0; a < NewPath.NumEvents; a++)
                    {
                        var NewUStrcut = new PathEvent();
                        NewUStrcut.EventType = StreamUtil.ReadUInt32(stream);
                        NewUStrcut.EventValue = StreamUtil.ReadUInt32(stream);
                        NewUStrcut.EventStart = StreamUtil.ReadFloat(stream);
                        NewUStrcut.EventEnd = StreamUtil.ReadFloat(stream);
                        NewPath.PathEvents.Add(NewUStrcut);
                    }

                    RaceLine.PathBs.Add(NewPath);
                }
            }
        }
        public void SaveAIPSOP(string path)
        {
            MemoryStream stream = new MemoryStream();

            stream.Position = 16;
            TypeAOffset = 0;
            StreamUtil.WriteInt32(stream, AIPath.PathAs.Count);
            StreamUtil.WriteInt32(stream, AIPath.StartPosList.Count);
            for (int i = 0; i < AIPath.StartPosList.Count; i++)
            {
                StreamUtil.WriteInt32(stream, AIPath.StartPosList[i]);
            }

            for (int i = 0; i < AIPath.PathAs.Count; i++)
            {
                StreamUtil.WriteInt32(stream, AIPath.PathAs[i].Type);
                StreamUtil.WriteInt32(stream, AIPath.PathAs[i].U1);
                StreamUtil.WriteInt32(stream, AIPath.PathAs[i].U2);
                StreamUtil.WriteInt32(stream, AIPath.PathAs[i].U3);
                StreamUtil.WriteInt32(stream, AIPath.PathAs[i].U4);
                StreamUtil.WriteInt32(stream, AIPath.PathAs[i].U5);
                StreamUtil.WriteInt32(stream, AIPath.PathAs[i].Respawnable);

                StreamUtil.WriteInt32(stream, AIPath.PathAs[i].VectorPoints.Count);
                StreamUtil.WriteInt32(stream, AIPath.PathAs[i].PathEvents.Count);

                StreamUtil.WriteVector3(stream, AIPath.PathAs[i].PathPos);
                StreamUtil.WriteVector3(stream, AIPath.PathAs[i].BBoxMin);
                StreamUtil.WriteVector3(stream, AIPath.PathAs[i].BBoxMax);

                for (int a = 0; a < AIPath.PathAs[i].VectorPoints.Count; a++)
                {
                    StreamUtil.WriteVector4(stream, AIPath.PathAs[i].VectorPoints[a]);
                }

                for (int a = 0; a < AIPath.PathAs[i].PathEvents.Count; a++)
                {
                    StreamUtil.WriteInt32(stream, AIPath.PathAs[i].PathEvents[a].EventType);
                    StreamUtil.WriteInt32(stream, AIPath.PathAs[i].PathEvents[a].EventValue);
                    StreamUtil.WriteFloat32(stream, AIPath.PathAs[i].PathEvents[a].EventStart);
                    StreamUtil.WriteFloat32(stream, AIPath.PathAs[i].PathEvents[a].EventEnd);
                }
            }

            TypeBOffset = (int)(stream.Position - 16);
            StreamUtil.WriteInt32(stream, RaceLine.U0);
            long Pos= stream.Position;
            stream.Position += 4;
            StreamUtil.WriteInt32(stream, RaceLine.PathBs.Count);
            StreamUtil.WriteInt32(stream, RaceLine.PathBUnknown);
            for (int i = 0; i < RaceLine.PathBs.Count; i++)
            {
                StreamUtil.WriteInt32(stream, RaceLine.PathBs[i].Type);
                StreamUtil.WriteInt32(stream, RaceLine.PathBs[i].U0);
                StreamUtil.WriteInt32(stream, RaceLine.PathBs[i].U1);
                StreamUtil.WriteFloat32(stream, RaceLine.PathBs[i].U2);

                StreamUtil.WriteInt32(stream, RaceLine.PathBs[i].VectorPoints.Count);
                StreamUtil.WriteInt32(stream, RaceLine.PathBs[i].PathEvents.Count);

                StreamUtil.WriteVector3(stream, RaceLine.PathBs[i].PathPos);
                StreamUtil.WriteVector3(stream, RaceLine.PathBs[i].BBoxMin);
                StreamUtil.WriteVector3(stream, RaceLine.PathBs[i].BBoxMax);

                for (int a = 0; a < RaceLine.PathBs[i].VectorPoints.Count; a++)
                {
                    StreamUtil.WriteVector4(stream, RaceLine.PathBs[i].VectorPoints[a]);
                }

                for (int a = 0; a < RaceLine.PathBs[i].PathEvents.Count; a++)
                {
                    StreamUtil.WriteInt32(stream, RaceLine.PathBs[i].PathEvents[a].EventType);
                    StreamUtil.WriteInt32(stream, RaceLine.PathBs[i].PathEvents[a].EventValue);
                    StreamUtil.WriteFloat32(stream, RaceLine.PathBs[i].PathEvents[a].EventStart);
                    StreamUtil.WriteFloat32(stream, RaceLine.PathBs[i].PathEvents[a].EventEnd);
                }
            }
            long TempPos = stream.Position;
            stream.Position = Pos;
            StreamUtil.WriteInt32(stream, (int)(TempPos - Pos - 4));

            stream.Position = 0;
            StreamUtil.WriteBytes(stream, MagicBytes);
            StreamUtil.WriteInt32(stream, PathTypeCount);
            StreamUtil.WriteInt32(stream, TypeAOffset);
            StreamUtil.WriteInt32(stream, TypeBOffset);

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

        public static List<Vector4> GenerateNewVectors(List<Vector3> PathPoints)
        {
            //Turn Points Into Vectors
            List<Vector4> Vectors = new List<Vector4>();
            for (int a = 1; a < PathPoints.Count; a++)
            {
                Vector3 Vector = PathPoints[a] /*- PathPoints[a - 1]*/;
                float Distance = length2D(Vector.X, Vector.Y);
                Vector4 NewPoint = new Vector4(Vector.X / Distance, Vector.Y / Distance, Vector.Z / Distance, Distance);
                Vectors.Add(NewPoint);
            }

            //Correct Vectors
            //List<Vector4> VectorPoints = new List<Vector4>();
            //for (int a = 0; a < Vectors.Count; a++)
            //{
            //    Vector4 vector4 = Vectors[a];

            //    //float Testfloat = Vectors[a].X + Vectors[a].Y /*+ Vectors[a].Z*/;

            //    //if (Testfloat >= 0)
            //    //{
            //    //    if (Vectors[a].X >= Vectors[a].Y /*&& Vectors[a].X >= Vectors[a].Z*/)
            //    //    {
            //    //        Vector4 NewVector = new Vector4();
            //    //        //Find X and W

            //    //        NewVector.X = 1f;
            //    //        NewVector.Y = (Vectors[a].Y / Vectors[a].X) * NewVector.X;
            //    //        NewVector.Z = (Vectors[a].Z / Vectors[a].X) * NewVector.X;

            //    //        NewVector.W = (Vectors[a].W * Vectors[a].X) * NewVector.X;

            //    //        VectorPoints.Add(NewVector);
            //    //    }
            //    //    else if (Vectors[a].Y >= Vectors[a].X/* && Vectors[a].Y >= Vectors[a].Z*/)
            //    //    {
            //    //        Vector4 NewVector = new Vector4();
            //    //        NewVector.Y = 1f;
            //    //        NewVector.X = (Vectors[a].X / Vectors[a].Y) * NewVector.Y;
            //    //        NewVector.Z = (Vectors[a].Z / Vectors[a].Y) * NewVector.Y;

            //    //        NewVector.W = (Vectors[a].W * Vectors[a].Y) * NewVector.Y;

            //    //        VectorPoints.Add(NewVector);
            //    //    }
            //    //    //else if (Vectors[a].Z >= Vectors[a].Y && Vectors[a].Z >= Vectors[a].X)
            //    //    //{
            //    //    //    Vector4 NewVector = new Vector4();
            //    //    //    NewVector.Z = 1f;
            //    //    //    NewVector.W = NewVector.Z * (Vectors[a].W/Vectors[a].Z);
            //    //    //    NewVector.X = (Vectors[a].X / Vectors[a].Z) * NewVector.Z;
            //    //    //    NewVector.Y = (Vectors[a].Y / Vectors[a].Z) * NewVector.Z;
            //    //    //    TempPath.VectorPoints.Add(NewVector);
            //    //    //}
            //    //}
            //    //else
            //    //{
            //    //    if (Vectors[a].X <= Vectors[a].Y /*&& Vectors[a].X <= Vectors[a].Z*/)
            //    //    {
            //    //        Vector4 NewVector = new Vector4();
            //    //        //Find X and W
            //    //        NewVector.X = -1f;
            //    //        NewVector.Y = (Vectors[a].Y / Vectors[a].X) * NewVector.X;
            //    //        NewVector.Z = (Vectors[a].Z / Vectors[a].X) * NewVector.X;

            //    //        NewVector.W = (Vectors[a].W * Vectors[a].X) * NewVector.X;

            //    //        VectorPoints.Add(NewVector);
            //    //    }
            //    //    else if (Vectors[a].Y <= Vectors[a].X /*&& Vectors[a].Y <= Vectors[a].Z*/)
            //    //    {
            //    //        Vector4 NewVector = new Vector4();
            //    //        NewVector.Y = -1f;
            //    //        NewVector.X = (Vectors[a].X / Vectors[a].Y) * NewVector.Y;
            //    //        NewVector.Z = (Vectors[a].Z / Vectors[a].Y) * NewVector.Y;

            //    //        NewVector.W = (Vectors[a].W * Vectors[a].Y) * NewVector.Y;

            //    //        VectorPoints.Add(NewVector);
            //    //    }
            //    //    //else if (Vectors[a].Z <= Vectors[a].Y && Vectors[a].Z <= Vectors[a].X)
            //    //    //{
            //    //    //    Vector4 NewVector = new Vector4();
            //    //    //    NewVector.Z = -1f;
            //    //    //    NewVector.W = NewVector.Z * (Vectors[a].W/Vectors[a].Z);
            //    //    //    NewVector.X = (Vectors[a].X / Vectors[a].Z) * NewVector.Z;
            //    //    //    NewVector.Y = (Vectors[a].Y / Vectors[a].Z) * NewVector.Z;
            //    //    //    TempPath.VectorPoints.Add(NewVector);
            //    //    //}
            //    //}
            //}
            return Vectors;
        }

        public static float length2D(float x, float y)
        {
            return MathF.Sqrt((float)(Math.Pow(x,2) + Math.Pow(y,2)));
        }

        public struct TypeA
        {
            public int PathACount;

            public int StartPosCount;
            public List<int> StartPosList;

            public List<PathA> PathAs;
        }

        public struct PathA
        {
            public int Type;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int Respawnable;

            public int NumPoints;
            public int NumEvents;

            public Vector3 PathPos;
            public Vector3 BBoxMin;
            public Vector3 BBoxMax;

            public List<Vector4> VectorPoints;
            public List<PathEvent> PathEvents;
        }

        public struct PathEvent
        {
            public int EventType;
            public int EventValue;
            public float EventStart;
            public float EventEnd;
        }

        public struct TypeB
        {
            public int U0;
            public int ByteSize;

            public int PathBCount;
            public int PathBUnknown;

            public List<PathB> PathBs;
        }

        public struct PathB
        {
            public int Type;
            public int U0;
            public int U1;
            public float U2;

            public int NumPoints;
            public int NumEvents;

            public Vector3 PathPos;
            public Vector3 BBoxMin;
            public Vector3 BBoxMax;

            public List<Vector4> VectorPoints;
            public List<PathEvent> PathEvents;
        }
    }
}
