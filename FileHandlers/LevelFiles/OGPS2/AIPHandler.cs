using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SSXMultiTool.FileHandlers.LevelFiles.OGPS2
{
    public class AIPHandler
    {
        public int Count0;
        public int Count1;
        public int U2;
        public int U3;

        public int U4;
        public int U5;
        public int U6;
        public int U7;

        public int U8;
        public int U9;
        public int U10;

        List<PathData> pathDatas = new List<PathData>();
        List<PathData> pathDatas1 = new List<PathData>();

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                Count0 = StreamUtil.ReadUInt32(stream);
                Count1 = StreamUtil.ReadUInt32(stream);
                U2 = StreamUtil.ReadUInt32(stream);
                U3 = StreamUtil.ReadUInt32(stream);

                U4 = StreamUtil.ReadUInt32(stream);
                U5 = StreamUtil.ReadUInt32(stream);
                U6 = StreamUtil.ReadUInt32(stream);
                U7 = StreamUtil.ReadUInt32(stream);

                U8 = StreamUtil.ReadUInt32(stream);
                U9 = StreamUtil.ReadUInt32(stream);
                U10 = StreamUtil.ReadUInt32(stream);

                pathDatas = new List<PathData>();

                for (int i = 0; i < Count0; i++)
                {
                    var TempPathData = new PathData();

                    TempPathData.U0 = StreamUtil.ReadFloat(stream);
                    TempPathData.Vector4Count = StreamUtil.ReadUInt32(stream);
                    TempPathData.PathEventCount = StreamUtil.ReadUInt32(stream);

                    TempPathData.Pos1 = StreamUtil.ReadVector3(stream);
                    TempPathData.Pos2 = StreamUtil.ReadVector3(stream);
                    TempPathData.Pos3 = StreamUtil.ReadVector3(stream);

                    TempPathData.vector4s = new List<Vector4>();
                    for (int a = 0; a < TempPathData.Vector4Count; a++)
                    {
                        TempPathData.vector4s.Add(StreamUtil.ReadVector4(stream));
                    }

                    TempPathData.PathEvents = new List<PathEvent>();
                    for (int a = 0; a < TempPathData.PathEventCount; a++)
                    {
                        var TempEvent = new PathEvent();

                        TempEvent.U0 = StreamUtil.ReadUInt32(stream);
                        TempEvent.U1 = StreamUtil.ReadUInt32(stream);
                        TempEvent.U2 = StreamUtil.ReadFloat(stream);
                        TempEvent.U3 = StreamUtil.ReadFloat(stream);

                        TempPathData.PathEvents.Add(TempEvent);
                    }

                    pathDatas.Add(TempPathData);
                }

                pathDatas1 = new List<PathData>();

                for (int i = 0; i < Count1; i++)
                {
                    var TempPathData = new PathData();

                    TempPathData.U0 = StreamUtil.ReadFloat(stream);
                    TempPathData.Vector4Count = StreamUtil.ReadUInt32(stream);
                    TempPathData.PathEventCount = StreamUtil.ReadUInt32(stream);

                    TempPathData.Pos1 = StreamUtil.ReadVector3(stream);
                    TempPathData.Pos2 = StreamUtil.ReadVector3(stream);
                    TempPathData.Pos3 = StreamUtil.ReadVector3(stream);

                    TempPathData.vector4s = new List<Vector4>();
                    for (int a = 0; a < TempPathData.Vector4Count; a++)
                    {
                        TempPathData.vector4s.Add(StreamUtil.ReadVector4(stream));
                    }

                    TempPathData.PathEvents = new List<PathEvent>();
                    for (int a = 0; a < TempPathData.PathEventCount; a++)
                    {
                        var TempEvent = new PathEvent();

                        TempEvent.U0 = StreamUtil.ReadUInt32(stream);
                        TempEvent.U1 = StreamUtil.ReadUInt32(stream);
                        TempEvent.U2 = StreamUtil.ReadFloat(stream);
                        TempEvent.U3 = StreamUtil.ReadFloat(stream);

                        TempPathData.PathEvents.Add(TempEvent);
                    }

                    pathDatas1.Add(TempPathData);
                }

                stream.Position = 0;
            }
        }

        public struct PathData
        {
            public float U0;
            public int Vector4Count;
            public int PathEventCount;

            public Vector3 Pos1;
            public Vector3 Pos2;
            public Vector3 Pos3;

            public List<Vector4> vector4s;
            public List<PathEvent> PathEvents;
        }

        public struct PathEvent
        {
            public int U0;
            public int U1;
            public float U2;
            public float U3;
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
