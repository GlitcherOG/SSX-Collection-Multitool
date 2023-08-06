using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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
        public List<ModelHeader> modelHeaders = new List<ModelHeader>();
        public void Load(string Path)
        {
            using (Stream stream = File.Open(Path, FileMode.Open))
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
                    NewModelHeader.Unused0 = StreamUtil.ReadUInt32(stream, true);
                    NewModelHeader.Unused1 = StreamUtil.ReadUInt32(stream, true);

                    NewModelHeader.OffsetMateralList = StreamUtil.ReadUInt32(stream, true);
                    NewModelHeader.OffsetBoneData = StreamUtil.ReadUInt32(stream, true);
                    NewModelHeader.OffsetIKPointList = StreamUtil.ReadUInt32(stream, true);
                    NewModelHeader.OffsetMorphList = StreamUtil.ReadUInt32(stream, true);

                    NewModelHeader.OffsetSkinningSection = StreamUtil.ReadUInt32(stream, true);
                    NewModelHeader.OffsetTristripSection = StreamUtil.ReadUInt32(stream, true);
                    NewModelHeader.Unused2 = StreamUtil.ReadUInt32(stream, true);
                    NewModelHeader.OffsetVertexSection = StreamUtil.ReadUInt32(stream, true);

                    stream.Position += 290;

                    NewModelHeader.NumBones = StreamUtil.ReadUInt16(stream, true);
                    NewModelHeader.NumMorphs = StreamUtil.ReadUInt16(stream, true);
                    NewModelHeader.NumMaterials = StreamUtil.ReadUInt16(stream, true);
                    NewModelHeader.NumIKPoints = StreamUtil.ReadUInt16(stream, true);

                    NewModelHeader.NumSkinningHeaders = StreamUtil.ReadUInt16(stream, true);
                    NewModelHeader.NumMeshPerSkin = StreamUtil.ReadUInt16(stream, true);
                    NewModelHeader.Unknown3 = StreamUtil.ReadUInt16(stream, true);
                    NewModelHeader.NumVertices = StreamUtil.ReadUInt16(stream, true);
                    NewModelHeader.FileID = StreamUtil.ReadUInt16(stream, true);

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

                //Read Matrix
                for (int i = 0; i < modelHeaders.Count; i++)
                {
                    Stream streamMatrix = new MemoryStream();
                    var Model = modelHeaders[i];
                    streamMatrix.Write(modelHeaders[i].Matrix, 0, modelHeaders[i].Matrix.Length);
                    streamMatrix.Position = 0;

                    Model.materialDatas = new List<MaterialData>();
                    for (int a = 0; a < Model.NumMaterials; a++)
                    {
                        var TempMat = new MaterialData();
                        TempMat.MainTexture = StreamUtil.ReadString(streamMatrix, 4);
                        if (streamMatrix.ReadByte() != 0x00)
                        {
                            streamMatrix.Position -= 1;
                            TempMat.Texture1 = StreamUtil.ReadString(streamMatrix, 4);
                        }
                        else
                        {
                            streamMatrix.Position += 3;
                            TempMat.Texture1 = "";
                        }

                        if (streamMatrix.ReadByte() != 0x00)
                        {
                            streamMatrix.Position -= 1;
                            TempMat.Texture2 = StreamUtil.ReadString(streamMatrix, 4);
                        }
                        else
                        {
                            streamMatrix.Position += 3;
                            TempMat.Texture2 = "";
                        }

                        if (streamMatrix.ReadByte() != 0x00)
                        {
                            streamMatrix.Position -= 1;
                            TempMat.Texture3 = StreamUtil.ReadString(streamMatrix, 4);
                        }
                        else
                        {
                            streamMatrix.Position += 3;
                            TempMat.Texture3 = "";
                        }

                        if (streamMatrix.ReadByte() != 0x00)
                        {
                            streamMatrix.Position -= 1;
                            TempMat.Texture4 = StreamUtil.ReadString(streamMatrix, 4);
                        }
                        else
                        {
                            streamMatrix.Position += 3;
                            TempMat.Texture4 = "";
                        }

                        TempMat.FactorFloat = StreamUtil.ReadFloat(streamMatrix);
                        TempMat.Unused1Float = StreamUtil.ReadFloat(streamMatrix);
                        TempMat.Unused2Float = StreamUtil.ReadFloat(streamMatrix);
                        Model.materialDatas.Add(TempMat);
                    }

                    streamMatrix.Position = Model.OffsetBoneData;
                    Model.boneDatas = new List<BoneData>();
                    for (int a = 0; a < Model.NumBones; a++)
                    {
                        var TempBoneData = new BoneData();
                        TempBoneData.BoneName = StreamUtil.ReadString(streamMatrix, 16);
                        TempBoneData.ParentFileID = StreamUtil.ReadInt16(streamMatrix,true);
                        TempBoneData.ParentBone = StreamUtil.ReadInt16(streamMatrix, true);
                        TempBoneData.Unknown2 = StreamUtil.ReadInt16(streamMatrix, true);
                        TempBoneData.BoneID = StreamUtil.ReadInt16(streamMatrix, true);
                        TempBoneData.Position = StreamUtil.ReadVector3(streamMatrix, true);
                        TempBoneData.Radians = StreamUtil.ReadVector3(streamMatrix, true);
                        TempBoneData.XRadian2 = StreamUtil.ReadFloat(streamMatrix, true);
                        TempBoneData.YRadian2 = StreamUtil.ReadFloat(streamMatrix, true);
                        TempBoneData.ZRadian2 = StreamUtil.ReadFloat(streamMatrix, true);

                        TempBoneData.UnknownFloat1 = StreamUtil.ReadFloat(streamMatrix, true);
                        TempBoneData.UnknownFloat2 = StreamUtil.ReadFloat(streamMatrix, true);
                        TempBoneData.UnknownFloat3 = StreamUtil.ReadFloat(streamMatrix, true);
                        TempBoneData.UnknownFloat4 = StreamUtil.ReadFloat(streamMatrix, true);
                        TempBoneData.UnknownFloat5 = StreamUtil.ReadFloat(streamMatrix, true);
                        TempBoneData.UnknownFloat6 = StreamUtil.ReadFloat(streamMatrix, true);

                        TempBoneData.FileID = Model.FileID;
                        TempBoneData.BonePos = a;

                        Model.boneDatas.Add(TempBoneData);
                    }

                    streamMatrix.Position = Model.OffsetIKPointList;
                    Model.iKPoints = new List<Vector3>();
                    for (int a = 0; a < Model.NumIKPoints; a++)
                    {
                        var TempIKPoint = new Vector3();
                        TempIKPoint = StreamUtil.ReadVector3(streamMatrix, true);
                        streamMatrix.Position += 4;
                        Model.iKPoints.Add(TempIKPoint);
                    }

                    streamMatrix.Position = Model.OffsetMorphList;
                    Model.morphHeader = new List<MorphHeader>();
                    for (int a = 0; a < Model.NumMorphs; a++)
                    {
                        var TempMorph = new MorphHeader();

                        TempMorph.NumMorphData = StreamUtil.ReadUInt32(streamMatrix, true);
                        TempMorph.OffsetMorphDataList = StreamUtil.ReadUInt32(streamMatrix, true);

                        TempMorph.MorphDataList = new List<MorphData>();
                        int TempPos = (int)streamMatrix.Position;
                        streamMatrix.Position = TempMorph.OffsetMorphDataList;
                        for (int b = 0; b < TempMorph.NumMorphData; b++)
                        {
                            var TempMorphData = new MorphData();

                            TempMorphData.Morph = StreamUtil.ReadVector3(streamMatrix, true);
                            TempMorphData.VertexIndex = StreamUtil.ReadUInt16(streamMatrix, true);
                            TempMorphData.U1 = StreamUtil.ReadUInt8(streamMatrix);
                            TempMorphData.U2 = StreamUtil.ReadUInt8(streamMatrix);
                            TempMorph.MorphDataList.Add(TempMorphData);
                        }
                        streamMatrix.Position = TempPos;
                        Model.morphHeader.Add(TempMorph);

                    }

                    streamMatrix.Position = Model.OffsetSkinningSection;
                    Model.boneWeightHeaders = new List<BoneWeightHeader>();
                    for (int b = 0; b < Model.NumSkinningHeaders; b++)
                    {
                        var BoneWeight = new BoneWeightHeader();

                        BoneWeight.WeightCount = StreamUtil.ReadUInt32(streamMatrix, true);
                        BoneWeight.WeightListOffset = StreamUtil.ReadUInt32(streamMatrix, true);
                        BoneWeight.Unknown1 = StreamUtil.ReadUInt16(streamMatrix, true);
                        BoneWeight.Unknown2 = StreamUtil.ReadUInt16(streamMatrix, true);
                        BoneWeight.boneWeights = new List<BoneWeight>();
                        int TempPos = (int)streamMatrix.Position;
                        streamMatrix.Position = BoneWeight.WeightListOffset;
                        for (int a = 0; a < BoneWeight.WeightCount; a++)
                        {
                            var boneWeight = new BoneWeight();
                            boneWeight.Weight = StreamUtil.ReadInt16(streamMatrix, true);
                            boneWeight.BoneID = StreamUtil.ReadUInt8(streamMatrix);
                            boneWeight.FileID = StreamUtil.ReadUInt8(streamMatrix);
                            BoneWeight.boneWeights.Add(boneWeight);
                        }
                        streamMatrix.Position = TempPos;
                        Model.boneWeightHeaders.Add(BoneWeight);
                    }

                    streamMatrix.Position = Model.OffsetTristripSection;
                    for (int b = 0; b < Model.NumMeshPerSkin; b++)
                    {
                        var TempTriData = new MeshHeader();
                        TempTriData.NumWeightIndices = StreamUtil.ReadUInt32(streamMatrix, true);
                        TempTriData.NumIndexGroups = StreamUtil.ReadUInt32(streamMatrix, true);
                        TempTriData.OffsetSkinIndexList = StreamUtil.ReadUInt32(streamMatrix, true);
                        TempTriData.OffsetIndexGroupList = StreamUtil.ReadUInt32(streamMatrix, true);
                        TempTriData.unk0 = StreamUtil.ReadUInt32(streamMatrix, true);
                        TempTriData.unk1 = StreamUtil.ReadUInt32(streamMatrix, true);
                        TempTriData.unk2 = StreamUtil.ReadUInt32(streamMatrix, true);

                        long TempPos = streamMatrix.Position;
                        stream.Position = TempTriData.OffsetSkinIndexList;
                        TempTriData.SkinIndex = new List<int>();
                        for (int c = 0; c < TempTriData.NumWeightIndices; c++)
                        {
                            TempTriData.SkinIndex.Add(StreamUtil.ReadUInt32(streamMatrix, true));
                        }

                        stream.Position = TempTriData.OffsetIndexGroupList;
                        TempTriData.indexGroupHeaders = new List<IndexGroupHeader>();
                        for (int c = 0; c < TempTriData.NumIndexGroups; c++)
                        {
                            var TempIndexGroup = new IndexGroupHeader();

                            TempIndexGroup.Offset = StreamUtil.ReadUInt32(streamMatrix, true);
                            TempIndexGroup.ByteLength = StreamUtil.ReadUInt16(streamMatrix, true);
                            TempIndexGroup.MatIndex0 = StreamUtil.ReadUInt16(streamMatrix, true);
                            TempIndexGroup.MatIndex1 = StreamUtil.ReadUInt16(streamMatrix, true);
                            TempIndexGroup.MatIndex2 = StreamUtil.ReadUInt16(streamMatrix, true);
                            TempIndexGroup.MatIndex3 = StreamUtil.ReadUInt16(streamMatrix, true);
                            TempIndexGroup.MatIndex4 = StreamUtil.ReadUInt16(streamMatrix, true);

                            long TempPos1 = streamMatrix.Position;


                            streamMatrix.Position = TempPos1;

                            TempTriData.indexGroupHeaders.Add(TempIndexGroup);
                        }

                        streamMatrix.Position = TempPos;
                    }

                    modelHeaders[i] = Model;
                }
            }
        }

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

        public struct MeshHeader
        {
            public int NumWeightIndices;
            public int NumIndexGroups;
            public int OffsetSkinIndexList;
            public int OffsetIndexGroupList;
            public int unk0;
            public int unk1;
            public int unk2;

            public List<int> SkinIndex;
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

            public IndexGroup indexGroup;
        }

        public struct IndexGroup
        {
            public int OffsetStrip;
            public int IndexUnk0;
            public int NumIndex;
            public int IndexUnk1;
        }
    }
}
