using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models.Tricky
{
    public class TrickyGCMNF
    {
        public byte[] Version;
        public int NumModels;
        public int OffsetModelHeader;
        public int OffsetModelData;

        


        public struct ModelHeader
        {
            public string ModelName;
            public int ModelOffset;
            public int ModelSize;
            public int Unused0;
            public int Unused1;

            public int OffsetMateralList;
            public int OffsetBoneData;
            public int OffsetIKPointList;
            public int OffsetMorphList;

            public int OffsetSkinningSection;
            public int OffsetTristripSection;
            public int Unused2;
            public int OffsetVertexSection;

            public int NumBones;
            public int NumMorphs;
            public int NumMaterials;
            public int NumIKPoints;
            public int NumSkinningHeaders;
            public int NumMeshPerSkin;
            public int Unknown3;
            public int NumVertices;
            public int FileID;

            public byte[] Matrix;

            public List<MaterialData> materialDatas;
            public List<BoneData> boneDatas;
            public List<Vector3> iKPoints;
            public List<MorphHeader> morphHeader;
            public List<BoneWeightHeader> boneWeightHeaders;
        }

        public struct MaterialData
        {
            public string MainTexture;
            public string Texture1;
            public string Texture2;
            public string Texture3;
            public string Texture4;

            public float FactorFloat;
            public float Unused1Float;
            public float Unused2Float;
        }

        public struct BoneData
        {
            public string BoneName;
            public int ParentFileID;
            public int ParentBone;
            public int Unknown2;
            public int BoneID;

            public Vector3 Position;
            public Vector3 Radians;

            public float XRadian2;
            public float YRadian2;
            public float ZRadian2;

            public float UnknownFloat1;
            public float UnknownFloat2;
            public float UnknownFloat3;
            public float UnknownFloat4;
            public float UnknownFloat5;
            public float UnknownFloat6;

            public int FileID;
            public int BonePos;

            public string parentName;
        }

        public struct MorphHeader
        {
            public int NumMorphData;
            public int OffsetMorphDataList;

            public List<MorphData> MorphDataList;
        }

        public struct MorphData
        {
            public Vector3 Morph;
            public int VertexIndex;
            public int U1;
            public int U2;
        }

        public struct BoneWeightHeader
        {
            public int WeightCount;
            public int WeightListOffset;
            public int Unknown1; //0
            public int Unknown2; //19

            public List<BoneWeight> boneWeights;
        }

        public struct BoneWeight
        {
            public int Weight;
            public int BoneID;
            public int FileID;

            public string BoneName;
        }
    }
}
