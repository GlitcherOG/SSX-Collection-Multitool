using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models
{
    public class OpenSSXMFHandler
    {
        public string MagicName = "OSMF";
        public int FormatVersion;
        public int ModelHeaderOffset;
        public int ModelHeaderCount;
        public int BoneOffset;
        public int BoneCount;
        public int MaterialOffset;
        public int MaterialCount;
        public int ModelDataOffset;

        public List<ModelHeader> modelHeaders = new List<ModelHeader>();
        public List<MaterialData> materialDatas = new List<MaterialData>();
        public List<BoneData> boneDatas = new List<BoneData>();

        public struct ModelHeader
        {
            public string ModelName;

            public int ModelOffset;
            public int ModelSize;
            //public int MaterialOffset;
            //public int BoneOffset;
            public int IKOffset;
            public int WeightOffset;
            public int VertexOffset;
            public int MorphOffset;
            public int FaceOffset;

            //public int MaterialCount;
            //public int BoneCount;
            public int IKCount;
            public int WeightCount;
            public int VertexCount;
            public int MorphCount;
            public int FaceCount;

            public int FileID;

            //public List<MaterialData> materialDatas;
            //public List<BoneData> boneDatas;
            public List<Vector3> ikPoints;
            public List<MorphHeader> morphDatas;
            public List<BoneWeightHeader> boneWeightHeaders;
            public List<VertexData> vertexDatas;
            public List<Face> faces;
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
            public int BoneID;
            public int FileID;

            public Vector3 Position;
            public Quaternion Rotation;

            //Non Saving Data
            public string parentName;
        }

        public struct MorphHeader
        {
            public string MorphName; //Unknown if will keep this

            public int NumMorphData;
            public int OffsetMorphDataList;

            public List<MorphData> MorphDataList;
        }

        public struct MorphData
        {
            public Vector3 Morph;
            public int VertexIndex;
        }

        public struct BoneWeightHeader
        {
            public int WeightCount;
            public int WeightListOffset;

            public List<BoneWeight> boneWeights;
        }

        public struct BoneWeight
        {
            public int Weight;
            public int BoneID;
            public int FileID;

            //Non Saving Data
            public string BoneName;
        }

        public struct VertexData
        {
            public Vector3 VertexPosition;
            public Vector3 VertexNormal;
            public Vector2 VertexUV;
            public int WeightIndex;

            //Added Here From Morph
            public List<Vector3> MorphData;
        }

        public struct Face
        {
            public int V1Pos;
            public int V2Pos;
            public int V3Pos;
            public int MaterialID;
        }
    }



}
