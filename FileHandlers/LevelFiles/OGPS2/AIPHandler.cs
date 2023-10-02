using System;
using System.Collections.Generic;
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

        List<PathData> pathDatas = new List<PathData>();
        List<PathData> pathDatas1 = new List<PathData>();

        public struct PathData
        {
            public float U0;
            public int U1;
            public float U2;

            public Vector3 Pos1;
            public Vector3 Pos2;
            public Vector3 Pos3;

            public List<Vector4> vector4s;
            public List<PathEvent> PathEvents;
        }

        public struct PathEvent
        {
            public int U0;
            public float U1;
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
