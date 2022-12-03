using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

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

                pathAOffset = StreamUtil.ReadInt32(stream);
                pathBOffset = StreamUtil.ReadInt32(stream);
                pathACount = StreamUtil.ReadInt32(stream);

                //Skip to path A
                stream.Position = 0x30;
                typeAs = new List<PathTypeA>();

                for (int i = 0; i < pathACount; i++)
                {
                    var TempTypeA = new PathTypeA();
                    TempTypeA.Unknown1 = StreamUtil.ReadInt32(stream);
                    TempTypeA.Unknown2 = StreamUtil.ReadInt32(stream);
                    TempTypeA.Unknown3 = StreamUtil.ReadInt32(stream);
                    TempTypeA.Unknown4 = StreamUtil.ReadInt32(stream);
                    TempTypeA.Unknown5 = StreamUtil.ReadInt32(stream);
                    TempTypeA.Unknown6 = StreamUtil.ReadInt32(stream);
                    TempTypeA.Unknown7 = StreamUtil.ReadInt32(stream);

                    TempTypeA.pathPointsCount = StreamUtil.ReadInt32(stream);
                    TempTypeA.UnknownCount = StreamUtil.ReadInt32(stream);

                    TempTypeA.pathPos = StreamUtil.ReadVector3(stream);
                    TempTypeA.bboxMin = StreamUtil.ReadVector3(stream);
                    TempTypeA.bboxMax = StreamUtil.ReadVector3(stream);

                    TempTypeA.patchPoints = new List<Vector3>();
                    for (int a = 0; a < TempTypeA.pathPointsCount; a++)
                    {
                        Vector3 Direction = StreamUtil.ReadVector3(stream);
                        float Distance = StreamUtil.ReadFloat(stream);
                        TempTypeA.patchPoints.Add(Direction * Distance);
                    }

                    TempTypeA.unkownListTypeAs = new List<UnkownListTypeA>();
                    for (int a = 0; a < TempTypeA.UnknownCount; a++)
                    {
                        var TempUnknownListTypeA = new UnkownListTypeA();
                        TempUnknownListTypeA.Unknown1 = StreamUtil.ReadInt32(stream);
                        TempUnknownListTypeA.Unknown2 = StreamUtil.ReadInt32(stream);
                        TempUnknownListTypeA.Unknown3 = StreamUtil.ReadFloat(stream);
                        TempUnknownListTypeA.Unknown4 = StreamUtil.ReadFloat(stream);
                        TempTypeA.unkownListTypeAs.Add(TempUnknownListTypeA);
                    }
                    typeAs.Add(TempTypeA);
                }

                //Skip to PathB
                stream.Position = pathBOffset + 16;
                Unknown1 = StreamUtil.ReadInt32(stream);
                Unknown2 = StreamUtil.ReadInt32(stream);
                pathBCount = StreamUtil.ReadInt32(stream);
                pathBUnknown = StreamUtil.ReadInt32(stream);

                typeBs = new List<PathTypeB>();
                for (int i = 0; i < pathBCount; i++)
                {
                    var TempTypeB = new PathTypeB();
                    TempTypeB.Unknown1 = StreamUtil.ReadInt32(stream);
                    TempTypeB.Unknown2 = StreamUtil.ReadInt32(stream);
                    TempTypeB.Unknown3 = StreamUtil.ReadInt32(stream);
                    TempTypeB.Unknown4 = StreamUtil.ReadFloat(stream);

                    TempTypeB.pathPointsCount = StreamUtil.ReadInt32(stream);
                    TempTypeB.UnknownCount = StreamUtil.ReadInt32(stream);

                    TempTypeB.pathPos = StreamUtil.ReadVector3(stream);
                    TempTypeB.bboxMin = StreamUtil.ReadVector3(stream);
                    TempTypeB.bboxMax = StreamUtil.ReadVector3(stream);

                    TempTypeB.patchPoints = new List<Vector3>();
                    for (int a = 0; a < TempTypeB.pathPointsCount; a++)
                    {
                        Vector3 Direction = StreamUtil.ReadVector3(stream);
                        float Distance = StreamUtil.ReadFloat(stream);
                        TempTypeB.patchPoints.Add(Direction * Distance);
                    }

                    TempTypeB.unkownListTypeAs = new List<UnkownListTypeA>();
                    for (int a = 0; a < TempTypeB.UnknownCount; a++)
                    {
                        var TempUnknownListTypeA = new UnkownListTypeA();
                        TempUnknownListTypeA.Unknown1 = StreamUtil.ReadInt32(stream);
                        TempUnknownListTypeA.Unknown2 = StreamUtil.ReadInt32(stream);
                        TempUnknownListTypeA.Unknown3 = StreamUtil.ReadFloat(stream);
                        TempUnknownListTypeA.Unknown4 = StreamUtil.ReadFloat(stream);
                        TempTypeB.unkownListTypeAs.Add(TempUnknownListTypeA);
                    }
                    typeBs.Add(TempTypeB);
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

            public List<Vector3> patchPoints;
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

            public List<Vector3> patchPoints;
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
