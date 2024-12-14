using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models.SSX3
{
    public class SSX3GCMNF
    {
        public byte[] magicWords = new byte[4];
        public int NumModels;
        public int HeaderSize;
        public int DataOffset;
        public List<MPFModelHeader> ModelList = new List<MPFModelHeader>();

        public void load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {

            }
        }

        public struct MPFModelHeader
        {
            //Main Header
            public string ModelName;
            public int DataOffset;
            public int EntrySize;
            public int BoneOffset;
            public int UnusedIKPointOffset; //Weight Info (IK Points?)
            public int MaterialGroupOffset;
            public int MeshDataOffset;
            public int MaterialOffset;
            public int MorphIDOffset;
            public int WeightRefrenceOffset;
            public int BoneWeightOffset;

            //Counts
            public int WeightCount;
            public int WeightRefrenceCount;
            public int MaterialGroupCount;
            public int BoneCount;
            public int MaterialCount;
            public int UnusedIKPointCount; //IK Point Count??
            public int MorphKeyCount;
            public int FileID;
            public int TriangleCount; //Possibly Some Kind of Face Ammount Used in Store as Well


            public List<MaterialData> MaterialList;
            public List<BoneData> BoneList;
            public List<int> MorphKeyIDList;
            public List<BoneWeightHeader> BoneWeightHeaderList;
            public List<WeightRefList> WeightRefrenceLists;
            public List<MaterialGroup> MaterialGroupList;

            public byte[] Matrix;
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
            public int Unknown1;
            public int BoneID;

            public int Unknown2;
            public int Unknown3;
            public int Unknown4;
            public int Unknown5;

            public int Unknown6; //Padding

            public Vector4 Position;
            public Quaternion Rotation;
            public Vector4 Unknown;

            public int FileID;
            public int BonePos;

            public string parentName;
            public Matrix4x4 WorldMatrix;
        }

        public struct BoneWeightHeader
        {
            public int WeightCount;
            public int WeightOffset;
            public int Unknown;

            public List<BoneWeight> BoneWeightList;
        }

        public struct BoneWeight
        {
            public int Weight;
            public int BoneID;
            public int FileID;

            public string boneName;
        }

        public struct WeightRefList
        {
            public int ListCount;
            public int Offset;

            public List<int> WeightIDs;
        }

        public struct MaterialGroup
        {
            public int Type; //1 - Standard, 17 - Shadow, 256 - Morph
            public int Material;
            public int Unknown;
            public int WeightRefGroupCount;
            public int WeightRefGroupOffset;

            public List<WeightRefGroup> WeightRefList;
        }

        public struct WeightRefGroup
        {
            public int MorphMeshOffset;
            public int MorphMeshCount;

            public List<MorphMeshGroup> MorphMeshGroupList;

            public List<int> weights;
        }

        public struct MorphMeshGroup
        {
            public int MeshOffset;
            public int MorphOffset;

            public int WeightRefID;
            public List<MeshChunk> MeshChunkList;
            public List<MorphKey> MorphDataList;
        }

        public struct MeshChunk
        {
            public int StripCount;
            public int Unknown1;
            public int Unknown2;
            public int VertexCount;

            public int WeightRefID;

            public List<int> Strips;
            public List<Vector4> UV;
            public List<Vector3> UVNormals;
            public List<Vector3> Vertices;
            public List<int> Weights;

            public List<Face> Faces;


            public int MatieralID;
            public bool Grouped;

            public List<int> weightsInts;
            public List<MorphKey> MorphKeys;
        }

        public struct Face
        {
            public bool tristripped;

            public Vector3 V1;
            public Vector3 V2;
            public Vector3 V3;

            public int V1Pos;
            public int V2Pos;
            public int V3Pos;

            public Vector4 UV1;
            public Vector4 UV2;
            public Vector4 UV3;

            public int UV1Pos;
            public int UV2Pos;
            public int UV3Pos;

            public Vector3 Normal1;
            public Vector3 Normal2;
            public Vector3 Normal3;

            public int Normal1Pos;
            public int Normal2Pos;
            public int Normal3Pos;

            public BoneWeightHeader Weight1;
            public BoneWeightHeader Weight2;
            public BoneWeightHeader Weight3;

            public int Weight1Pos;
            public int Weight2Pos;
            public int Weight3Pos;

            public List<Vector3> MorphPoint1;
            public List<Vector3> MorphPoint2;
            public List<Vector3> MorphPoint3;

            public int MaterialID;
        }

        public struct MorphKey
        {
            public int MorphPointCount;
            public int ListAmmount;
            public List<MorphData> MorphDataList;
        }

        public struct MorphData
        {
            public Vector3 vector3;
            public int ID;
        }
    }
}
