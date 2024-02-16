using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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

        public WEITStruct weitStruct = new WEITStruct();
        public SkelStruct skelStruct = new SkelStruct();

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                Magic = StreamUtil.ReadString(stream, 4);
                U1 = StreamUtil.ReadUInt32(stream);
                U2 = StreamUtil.ReadUInt32(stream);
                U3 = StreamUtil.ReadUInt32(stream);

                weitStruct = new WEITStruct();
                weitStruct.WEITMagic = StreamUtil.ReadString(stream, 4);
                weitStruct.U0 = StreamUtil.ReadUInt32(stream, true);
                weitStruct.U1 = StreamUtil.ReadUInt32(stream, true);
                weitStruct.U2 = StreamUtil.ReadUInt32(stream, true);
                weitStruct.U3 = StreamUtil.ReadUInt32(stream, true);
                weitStruct.U4 = StreamUtil.ReadUInt32(stream, true);
                weitStruct.WeightCount = StreamUtil.ReadUInt32(stream, true);
                weitStruct.VertexCount = StreamUtil.ReadUInt32(stream, true);

                weitStruct.weightStructs = new List<WeightStruct>();

                for (int i = 0; i < weitStruct.VertexCount; i++)
                {
                    WeightStruct NewWeightStruct = new WeightStruct();
                    NewWeightStruct.WeightValue = new List<float>();
                    for (int a = 0; a < weitStruct.WeightCount; a++)
                    {
                        NewWeightStruct.WeightValue.Add(StreamUtil.ReadFloat(stream, true));
                    }
                    weitStruct.weightStructs.Add(NewWeightStruct);
                }

                for (int i = 0; i < weitStruct.weightStructs.Count; i++)
                {
                    WeightStruct NewWeightStruct = weitStruct.weightStructs[i];
                    NewWeightStruct.BoneIndex = new List<int>();
                    for (int a = 0; a < weitStruct.WeightCount; a++)
                    {
                        NewWeightStruct.BoneIndex.Add(StreamUtil.ReadInt8(stream));
                    }
                    weitStruct.weightStructs[i] = NewWeightStruct;
                }

                skelStruct = new SkelStruct();
                skelStruct.SkelMagic = StreamUtil.ReadString(stream, 4);
                skelStruct.U0 = StreamUtil.ReadUInt32(stream, true);
                skelStruct.U1 = StreamUtil.ReadUInt32(stream, true);
                skelStruct.U2 = StreamUtil.ReadUInt32(stream, true);
                skelStruct.BoneCount = StreamUtil.ReadUInt32(stream, true);

                skelStruct.boneDatas = new List<BoneData>();
                for (int i = 0; i < skelStruct.BoneCount; i++)
                {
                    BoneData boneData = new BoneData();

                    boneData.BoneNameSize = StreamUtil.ReadUInt32(stream, true);
                    boneData.BoneName = StreamUtil.ReadString(stream, boneData.BoneNameSize + 1, true);

                    skelStruct.boneDatas.Add(boneData);
                }

                for (int i = 0; i < skelStruct.boneDatas.Count; i++)
                {
                    BoneData boneData = skelStruct.boneDatas[i];

                    boneData.matrix = StreamUtil.ReadMatrix4x4(stream);

                    skelStruct.boneDatas[i] = boneData;
                }

            }
        }

        public struct WEITStruct
        {
            public string WEITMagic;
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int WeightCount;
            public int VertexCount;

            public List<WeightStruct> weightStructs;
        }

        public struct WeightStruct
        {
            public List<float> WeightValue;
            public List<int> BoneIndex;
        }

        public struct SkelStruct
        {
            public string SkelMagic;
            public int U0;
            public int U1;
            public int U2;
            public int BoneCount;

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
