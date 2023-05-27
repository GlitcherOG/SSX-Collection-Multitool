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

        public TypeA TypeAStruct;
        public TypeB TypeBStruct;

        public void LoadAIPSOP(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                MagicBytes = StreamUtil.ReadBytes(stream, 4);
                PathTypeCount = StreamUtil.ReadUInt32(stream);
                TypeAOffset = StreamUtil.ReadUInt32(stream);
                TypeBOffset = StreamUtil.ReadUInt32(stream);

                TypeAStruct = new TypeA();
                TypeAStruct.PathACount = StreamUtil.ReadUInt32(stream);
                TypeAStruct.StartPosCount = StreamUtil.ReadUInt32(stream);

                TypeAStruct.StartPosList = new List<int>();
                for (int i = 0; i < TypeAStruct.StartPosCount; i++)
                {
                    TypeAStruct.StartPosList.Add(StreamUtil.ReadUInt32(stream));
                }

                TypeAStruct.PathAs = new List<PathA>();
                for (int i = 0; i < TypeAStruct.PathACount; i++)
                {
                    var NewPath = new PathA();
                    NewPath.Type = StreamUtil.ReadUInt32(stream);
                    NewPath.U1 = StreamUtil.ReadUInt32(stream);
                    NewPath.U2 = StreamUtil.ReadUInt32(stream);
                    NewPath.U3 = StreamUtil.ReadUInt32(stream);
                    NewPath.U4 = StreamUtil.ReadUInt32(stream);
                    NewPath.U5 = StreamUtil.ReadUInt32(stream);
                    NewPath.U6 = StreamUtil.ReadUInt32(stream);

                    NewPath.PointCount = StreamUtil.ReadUInt32(stream);
                    NewPath.UCount = StreamUtil.ReadUInt32(stream);

                    NewPath.PathPos = StreamUtil.ReadVector3(stream);
                    NewPath.BBoxMin = StreamUtil.ReadVector3(stream);
                    NewPath.BBoxMax = StreamUtil.ReadVector3(stream);

                    NewPath.VectorPoints = new List<Vector4>();
                    for (int a = 0; a < NewPath.PointCount; a++)
                    {
                        NewPath.VectorPoints.Add(StreamUtil.ReadVector4(stream));
                    }

                    NewPath.UnknownStructs = new List<UnknownStruct>();
                    for (int a = 0; a < NewPath.UCount; a++)
                    {
                        var NewUStrcut = new UnknownStruct();
                        NewUStrcut.U0 = StreamUtil.ReadUInt32(stream);
                        NewUStrcut.U1 = StreamUtil.ReadUInt32(stream);
                        NewUStrcut.U2 = StreamUtil.ReadFloat(stream);
                        NewUStrcut.U3 = StreamUtil.ReadFloat(stream);
                        NewPath.UnknownStructs.Add(NewUStrcut);
                    }

                    TypeAStruct.PathAs.Add(NewPath);
                }

                stream.Position = TypeBOffset + 16;
                TypeBStruct = new TypeB();
                TypeBStruct.U0 = StreamUtil.ReadUInt32(stream);
                TypeBStruct.ByteSize = StreamUtil.ReadUInt32(stream);
                TypeBStruct.PathBCount = StreamUtil.ReadUInt32(stream);
                TypeBStruct.PathBUnknown = StreamUtil.ReadUInt32(stream);

                TypeBStruct.PathBs = new List<PathB>();
                for (int i = 0; i < TypeBStruct.PathBCount; i++)
                {
                    var NewPath = new PathB();
                    NewPath.Type = StreamUtil.ReadUInt32(stream);
                    NewPath.U0 = StreamUtil.ReadUInt32(stream);
                    NewPath.U1 = StreamUtil.ReadUInt32(stream);
                    NewPath.U2 = StreamUtil.ReadFloat(stream);

                    NewPath.PointCount = StreamUtil.ReadUInt32(stream);
                    NewPath.UCount = StreamUtil.ReadUInt32(stream);

                    NewPath.PathPos = StreamUtil.ReadVector3(stream);
                    NewPath.BBoxMin = StreamUtil.ReadVector3(stream);
                    NewPath.BBoxMax = StreamUtil.ReadVector3(stream);

                    NewPath.VectorPoints = new List<Vector4>();
                    for (int a = 0; a < NewPath.PointCount; a++)
                    {
                        NewPath.VectorPoints.Add(StreamUtil.ReadVector4(stream));
                    }

                    NewPath.UnknownStructs = new List<UnknownStruct>();
                    for (int a = 0; a < NewPath.UCount; a++)
                    {
                        var NewUStrcut = new UnknownStruct();
                        NewUStrcut.U0 = StreamUtil.ReadUInt32(stream);
                        NewUStrcut.U1 = StreamUtil.ReadUInt32(stream);
                        NewUStrcut.U2 = StreamUtil.ReadFloat(stream);
                        NewUStrcut.U3 = StreamUtil.ReadFloat(stream);
                        NewPath.UnknownStructs.Add(NewUStrcut);
                    }

                    TypeBStruct.PathBs.Add(NewPath);
                }
            }

        }
        public void SaveAIPSOP(string path)
        {

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
            public int U6;

            public int PointCount;
            public int UCount;

            public Vector3 PathPos;
            public Vector3 BBoxMin;
            public Vector3 BBoxMax;

            public List<Vector4> VectorPoints;
            public List<UnknownStruct> UnknownStructs;
        }

        public struct UnknownStruct
        {
            public int U0;
            public int U1;
            public float U2;
            public float U3;
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

            public int PointCount;
            public int UCount;

            public Vector3 PathPos;
            public Vector3 BBoxMin;
            public Vector3 BBoxMax;

            public List<Vector4> VectorPoints;
            public List<UnknownStruct> UnknownStructs;
        }
    }
}
