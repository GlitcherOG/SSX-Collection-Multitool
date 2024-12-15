using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace SSXMultiTool.FileHandlers.Models.SSX3
{
    public class SSX3GCMNF
    {
        public byte[] Version;
        public int NumModels;
        public int OffsetModelHeader;
        public int OffsetModelData;
        public List<ModelHeader> modelHeaders = new List<ModelHeader>();

        public void load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                Version = StreamUtil.ReadBytes(stream, 4);
                NumModels = StreamUtil.ReadInt16(stream, true);
                OffsetModelHeader = StreamUtil.ReadInt16(stream, true);
                OffsetModelData = StreamUtil.ReadUInt32(stream, true);

                modelHeaders = new List<ModelHeader>();
                for (int i = 0; i < NumModels; i++)
                {
                    var NewModelHeader = new ModelHeader();

                    NewModelHeader.ModelName = StreamUtil.ReadString(stream, 16);
                    NewModelHeader.ModelOffset = StreamUtil.ReadUInt32(stream, true);
                    NewModelHeader.ModelSize = StreamUtil.ReadUInt32(stream, true);
                    NewModelHeader.OffsetBoneData = StreamUtil.ReadUInt32(stream, true);

                    NewModelHeader.OffsetSkinningSection = StreamUtil.ReadUInt32(stream, true);
                    NewModelHeader.U1 = StreamUtil.ReadUInt32(stream, true);
                    NewModelHeader.U2 = StreamUtil.ReadUInt32(stream, true);
                    NewModelHeader.U3 = StreamUtil.ReadUInt32(stream, true);

                    NewModelHeader.OffsetTristripSection = StreamUtil.ReadUInt32(stream, true);
                    NewModelHeader.OffsetVertexSection = StreamUtil.ReadUInt32(stream, true);
                    NewModelHeader.OffsetMateralList = StreamUtil.ReadUInt32(stream, true);

                    stream.Position += 24;

                    NewModelHeader.NumTristrip = StreamUtil.ReadUInt16(stream, true);
                    NewModelHeader.NumVertices = StreamUtil.ReadUInt16(stream, true);
                    NewModelHeader.NumBones = StreamUtil.ReadUInt16(stream, true);
                    NewModelHeader.NumMorphs = StreamUtil.ReadUInt16(stream, true);
                    NewModelHeader.NumWeight = StreamUtil.ReadUInt16(stream, true);
                    NewModelHeader.U4 = StreamUtil.ReadUInt16(stream, true);
                    NewModelHeader.NumMaterials = StreamUtil.ReadUInt16(stream, true);
                    NewModelHeader.FileID = StreamUtil.ReadUInt16(stream, false);

                    modelHeaders.Add(NewModelHeader);
                }

                //Read Data Into Matrix
                int StartPos = OffsetModelData;
                for (int i = 0; i < modelHeaders.Count; i++)
                {
                    stream.Position = StartPos + modelHeaders[i].ModelOffset;
                    ModelHeader modelHandler = modelHeaders[i];
                    modelHandler.Matrix = StreamUtil.ReadBytes(stream, modelHeaders[i].ModelSize);
                    modelHeaders[i] = modelHandler;
                }
            }
        }

        public void SaveDecompressedData(string path)
        {
            MemoryStream stream = new MemoryStream();

            for (int i = 0; i < modelHeaders.Count; i++)
            {
                var TempModel = modelHeaders[i];

                StreamUtil.WriteBytes(stream, TempModel.Matrix);
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            var file = File.Create(path);
            stream.Position = 0;
            stream.CopyTo(file);
            stream.Dispose();
            file.Close();
        }

        public struct ModelHeader
        {
            public string ModelName;
            public int ModelOffset;
            public int ModelSize;
            public int OffsetBoneData;
            public int OffsetSkinningSection;

            public int U1;
            public int U2;
            public int U3;

            public int OffsetTristripSection;
            public int OffsetVertexSection;
            public int OffsetMateralList;

            public int NumTristrip;
            public int NumVertices;
            public int NumBones;
            public int NumMorphs;
            public int NumWeight;
            public int U4;
            public int NumMaterials;
            public int FileID;

            public byte[] Matrix;

            public List<MaterialData> materialDatas;
            public List<BoneData> boneDatas;
            public List<Vector3> iKPoints;
            public List<MorphHeader> morphHeader;
            public List<BoneWeightHeader> boneWeightHeaders;
            public List<VertexData> Vertex;
            public List<MeshHeader> meshHeaders;
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

            public Vector3 Radians2;

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
            public int VertexIndex;
            public Vector3 Morph;
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

        public struct MeshHeader
        {
            public int NumWeightIndices;
            public int NumIndexGroups;
            public int OffsetSkinIndexList;
            public int OffsetIndexGroupList;
            public int unk0;
            public int unk1;
            public int unk2;

            public List<int> WeightIndex;
            public List<IndexGroupHeader> indexGroupHeaders;
        }

        public struct IndexGroupHeader
        {
            public int Offset;
            public int ByteLength;
            public int MatIndex0;
            public int MatIndex1;
            public int MatIndex2;
            public int MatIndex3;
            public int MatIndex4;

            public List<Face> faces;

            public IndexGroup indexGroup;
        }

        public struct IndexGroup
        {
            //public int OffsetStrip;
            public int IndexUnk0;
            public int NumIndex;
            public int IndexUnk1;

            public List<ShadowIndex> shadowIndices;
            public List<Index> indices;
        }

        public struct Index
        {
            public int WeightIndex;
            public int Index0;
            public int Index1;
            public int Index2;
            public int Unk0;
        }

        public struct ShadowIndex
        {
            public int Index;
            public int WeightIndex;
        }

        public struct VertexData
        {
            public Vector3 Vertex;
            public Vector3 VertexNormal;
            public Vector2 UV;

            public int MaterialID;
            public int WeightIndex;
            public List<Vector3> MorphDatas;
        }

        public struct Face
        {
            public Vector3 V1;
            public Vector3 V2;
            public Vector3 V3;

            public int V1Pos;
            public int V2Pos;
            public int V3Pos;

            public Vector2 UV1;
            public Vector2 UV2;
            public Vector2 UV3;

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
    }
}
