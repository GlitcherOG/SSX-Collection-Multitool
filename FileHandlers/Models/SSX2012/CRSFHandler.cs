using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models.SSX2012
{
    public class CRSFHandler
    {
        public string Magic;
        public int U1;
        public int U2;
        public int U3;



        public struct WEITStruct
        {
            public string WEITMagic;
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;

            public List<WeightStruct> weightStructs;

        }

        public struct WeightStruct
        {
            public List<float> WeightValue;
            public List<int> BoneIndex;
        }

        public struct SkelStructure
        {
            public string SkelMagic;
            public int U0;
            public int U1;
            public int U2;
            public int U3;

            public List<BoneData> boneDatas;
        }

        public struct BoneData
        {
            public int BoneNameSize;
            public string BoneName;
            public Matrix4x4 matrix;
        }
    }
}
