using SSXMultiTool.Utilities;
using System.IO;
using System.Numerics;

namespace SSXMultiTool.FileHandlers.LevelFiles.OGPS2
{
    public class AIPHandler
    {
        public int RaceLineCount;
        public int AIPathCount;
        public int U2;


        public List<int> StartPosList;
        public List<PathData> RaceLine = new List<PathData>();
        public List<PathData> AIPath = new List<PathData>();

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                RaceLineCount = StreamUtil.ReadUInt32(stream);
                AIPathCount = StreamUtil.ReadUInt32(stream);
                U2 = StreamUtil.ReadUInt32(stream);

                StartPosList = new List<int>();
                for (int i = 0; i < 8; i++)
                {
                    StartPosList.Add(StreamUtil.ReadUInt32(stream));
                }

                RaceLine = new List<PathData>(); //RaceLine Path

                for (int i = 0; i < RaceLineCount; i++)
                {
                    var TempPathData = new PathData();

                    TempPathData.U0 = StreamUtil.ReadFloat(stream);
                    TempPathData.PointCount = StreamUtil.ReadUInt32(stream);
                    TempPathData.PathEventCount = StreamUtil.ReadUInt32(stream);

                    TempPathData.PathPos = StreamUtil.ReadVector3(stream); //Path Pos
                    TempPathData.BBoxMin = StreamUtil.ReadVector3(stream); //BBoxMin
                    TempPathData.BBoxMax = StreamUtil.ReadVector3(stream); //BBoxMax

                    TempPathData.VectorPoints = new List<Vector4>();
                    for (int a = 0; a < TempPathData.PointCount; a++)
                    {
                        TempPathData.VectorPoints.Add(StreamUtil.ReadVector4(stream));
                    }

                    TempPathData.PathEvents = new List<PathEvent>();
                    for (int a = 0; a < TempPathData.PathEventCount; a++)
                    {
                        var TempEvent = new PathEvent();

                        TempEvent.EventType = StreamUtil.ReadUInt32(stream);
                        TempEvent.EventValue = StreamUtil.ReadUInt32(stream);
                        TempEvent.EventStart = StreamUtil.ReadFloat(stream);
                        TempEvent.EventEnd = StreamUtil.ReadFloat(stream);

                        TempPathData.PathEvents.Add(TempEvent);
                    }

                    RaceLine.Add(TempPathData);
                }

                AIPath = new List<PathData>(); //AI Path

                for (int i = 0; i < AIPathCount; i++)
                {
                    var TempPathData = new PathData();

                    TempPathData.U0 = StreamUtil.ReadFloat(stream);
                    TempPathData.PointCount = StreamUtil.ReadUInt32(stream);
                    TempPathData.PathEventCount = StreamUtil.ReadUInt32(stream);

                    TempPathData.PathPos = StreamUtil.ReadVector3(stream);
                    TempPathData.BBoxMin = StreamUtil.ReadVector3(stream);
                    TempPathData.BBoxMax = StreamUtil.ReadVector3(stream);

                    TempPathData.VectorPoints = new List<Vector4>();
                    for (int a = 0; a < TempPathData.PointCount; a++)
                    {
                        TempPathData.VectorPoints.Add(StreamUtil.ReadVector4(stream));
                    }

                    TempPathData.PathEvents = new List<PathEvent>();
                    for (int a = 0; a < TempPathData.PathEventCount; a++)
                    {
                        var TempEvent = new PathEvent();

                        TempEvent.EventType = StreamUtil.ReadUInt32(stream);
                        TempEvent.EventValue = StreamUtil.ReadUInt32(stream);
                        TempEvent.EventStart = StreamUtil.ReadFloat(stream);
                        TempEvent.EventEnd = StreamUtil.ReadFloat(stream);

                        TempPathData.PathEvents.Add(TempEvent);
                    }

                    AIPath.Add(TempPathData);
                }

            }
        }

        public struct PathData
        {
            public float U0;
            public int PointCount;
            public int PathEventCount;

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
    }
}


//struct aip_Header
//{

//    UInt32 unk1 Count;
//        UInt32 unk2 Count;
//        UInt32 unk[4];
//    UInt32 unk[4];
//    UInt32 unk[4];
//    UInt32 unk[4];
//    UInt32 unk[4];
//    UInt32 unk[4];
//    UInt32 unk[4];
//    UInt32 unk[4];
//    UInt32 unk[4];
//}
//// start of unk1

//// followed by unk2

//struct unk1_unk2
//{ // unk1 and unk2 are the same I think

//    Float32 unk;
//    UInt32 unkA_Count;
//    UInt32 unkB_Count;

//    Vector3 pos1; // main  point
//    Vector3 pos2; // extra point
//    Vector3 pos3; // extra point

//    unkA[
//        Float32 4 unkA_1;
//        ]
//        unkB[
//            UInt32  2 unkB_1;
//            Float32 2 unkB_2;
//        ]
//    };
