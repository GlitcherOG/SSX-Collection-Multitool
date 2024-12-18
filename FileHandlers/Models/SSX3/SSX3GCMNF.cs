﻿using SSXMultiTool.Utilities;
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

                    NewModelHeader.OffsetMorphSection = StreamUtil.ReadUInt32(stream, true);
                    NewModelHeader.OffsetSkinningSection = StreamUtil.ReadUInt32(stream, true);
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
                    NewModelHeader.NumMeshPerSkin = StreamUtil.ReadUInt16(stream, true);
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
                    modelHandler.Matrix =  RefpackHandler.Decompress(StreamUtil.ReadBytes(stream, modelHeaders[i].ModelSize));
                    modelHeaders[i] = modelHandler;
                }

                for (int i = 0; i < modelHeaders.Count; i++)
                {
                    bool Shadow = false;

                    if (modelHeaders[i].ModelName.ToLower().Contains("shdw"))
                    {
                        Shadow = true;
                    }

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

                        TempMat.FactorFloat = StreamUtil.ReadFloat(streamMatrix, true);
                        TempMat.Unused1Float = StreamUtil.ReadFloat(streamMatrix, true);
                        TempMat.Unused2Float = StreamUtil.ReadFloat(streamMatrix, true);
                        Model.materialDatas.Add(TempMat);
                    }

                    streamMatrix.Position = Model.OffsetBoneData;
                    Model.boneDatas = new List<BoneData>();
                    for (int a = 0; a < Model.NumBones; a++)
                    {
                        var TempBoneData = new BoneData();
                        TempBoneData.BoneName = StreamUtil.ReadString(streamMatrix, 16); //16
                        TempBoneData.ParentFileID = StreamUtil.ReadInt16(streamMatrix, true); //18
                        TempBoneData.ParentBone = StreamUtil.ReadInt16(streamMatrix, true); //20
                        TempBoneData.Unknown2 = StreamUtil.ReadInt16(streamMatrix, true); //22
                        TempBoneData.BoneID = StreamUtil.ReadInt16(streamMatrix, true); //24
                        TempBoneData.Position = StreamUtil.ReadVector3(streamMatrix, true); //36
                        TempBoneData.Radians = StreamUtil.ReadVector3(streamMatrix, true); //48
                        TempBoneData.Radians2 = StreamUtil.ReadVector3(streamMatrix, true); //60

                        TempBoneData.UnknownFloat1 = StreamUtil.ReadFloat(streamMatrix, true); //64
                        TempBoneData.UnknownFloat2 = StreamUtil.ReadFloat(streamMatrix, true); //68
                        TempBoneData.UnknownFloat3 = StreamUtil.ReadFloat(streamMatrix, true); //72
                        TempBoneData.UnknownFloat4 = StreamUtil.ReadFloat(streamMatrix, true); //76
                        TempBoneData.UnknownFloat5 = StreamUtil.ReadFloat(streamMatrix, true); //80
                        //TempBoneData.UnknownFloat6 = StreamUtil.ReadFloat(streamMatrix, true);

                        TempBoneData.FileID = Model.FileID;
                        TempBoneData.BonePos = a;

                        Model.boneDatas.Add(TempBoneData);
                    }

                    //streamMatrix.Position = Model.OffsetMorphList;
                    Model.morphHeader = new List<MorphHeader>();
                    //for (int a = 0; a < Model.NumMorphs; a++)
                    //{
                    //    var TempMorph = new MorphHeader();

                    //    TempMorph.NumMorphData = StreamUtil.ReadUInt32(streamMatrix, true);
                    //    TempMorph.OffsetMorphDataList = StreamUtil.ReadUInt32(streamMatrix, true);

                    //    TempMorph.MorphDataList = new List<MorphData>();
                    //    int TempPos = (int)streamMatrix.Position;
                    //    streamMatrix.Position = TempMorph.OffsetMorphDataList;
                    //    for (int b = 0; b < TempMorph.NumMorphData; b++)
                    //    {
                    //        var TempMorphData = new MorphData();

                    //        TempMorphData.VertexIndex = StreamUtil.ReadUInt32(streamMatrix, true);
                    //        TempMorphData.Morph = StreamUtil.ReadVector3(streamMatrix, true) * 100f;
                    //        TempMorph.MorphDataList.Add(TempMorphData);
                    //    }
                    //    streamMatrix.Position = TempPos;
                    //    Model.morphHeader.Add(TempMorph);

                    //}

                    streamMatrix.Position = Model.OffsetSkinningSection;
                    Model.boneWeightHeaders = new List<BoneWeightHeader>();
                    for (int b = 0; b < Model.NumWeight; b++)
                    {
                        var BoneWeight = new BoneWeightHeader();

                        BoneWeight.WeightCount = StreamUtil.ReadUInt32(streamMatrix, true);
                        BoneWeight.WeightListOffset = StreamUtil.ReadUInt32(streamMatrix, true);
                        BoneWeight.Unknown1 = StreamUtil.ReadUInt16(streamMatrix, true); //19
                        BoneWeight.Unknown2 = StreamUtil.ReadUInt16(streamMatrix, true); //0
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
                    Model.meshHeaders = new List<MeshHeader>();
                    for (int b = 0; b < Model.NumMeshPerSkin; b++)
                    {
                        var TempTriData = new MeshHeader();
                        TempTriData.NumWeightIndices = StreamUtil.ReadUInt32(streamMatrix, true);
                        TempTriData.NumIndexGroups = StreamUtil.ReadUInt32(streamMatrix, true);
                        TempTriData.OffsetSkinIndexList = StreamUtil.ReadUInt32(streamMatrix, true);
                        TempTriData.OffsetIndexGroupList = StreamUtil.ReadUInt32(streamMatrix, true);
                        //TempTriData.unk0 = StreamUtil.ReadInt16(streamMatrix, true);
                        //TempTriData.unk1 = StreamUtil.ReadUInt8(streamMatrix); //Shadow or not?
                        //TempTriData.unk2 = StreamUtil.ReadUInt8(streamMatrix);

                        long TempPos = streamMatrix.Position;
                        streamMatrix.Position = TempTriData.OffsetSkinIndexList;
                        TempTriData.WeightIndex = new List<int>();
                        for (int c = 0; c < TempTriData.NumWeightIndices; c++)
                        {
                            TempTriData.WeightIndex.Add(StreamUtil.ReadUInt32(streamMatrix, true));
                        }

                        streamMatrix.Position = TempTriData.OffsetIndexGroupList;
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

                            streamMatrix.Position = TempIndexGroup.Offset;

                            TempIndexGroup.indexGroup = new IndexGroup();

                            TempIndexGroup.indexGroup.IndexUnk0 = StreamUtil.ReadUInt8(streamMatrix);
                            TempIndexGroup.indexGroup.NumIndex = StreamUtil.ReadUInt16(streamMatrix, true);
                            TempIndexGroup.indexGroup.IndexUnk1 = StreamUtil.ReadUInt8(streamMatrix);

                            TempIndexGroup.indexGroup.shadowIndices = new List<ShadowIndex>();
                            TempIndexGroup.indexGroup.indices = new List<Index>();

                            for (int d = 0; d < TempIndexGroup.indexGroup.NumIndex; d++)
                            {
                                if (!Shadow)
                                {
                                    Index shadowIndex = new Index();

                                    shadowIndex.WeightIndex = (StreamUtil.ReadUInt8(streamMatrix) - 30) / 3;
                                    shadowIndex.Index0 = StreamUtil.ReadUInt16(streamMatrix, true);
                                    shadowIndex.Index1 = StreamUtil.ReadUInt16(streamMatrix, true);
                                    shadowIndex.Index2 = StreamUtil.ReadUInt16(streamMatrix, true);
                                    shadowIndex.Unk0 = StreamUtil.ReadUInt8(streamMatrix);
                                    TempIndexGroup.indexGroup.indices.Add(shadowIndex);
                                }
                                else
                                {
                                    ShadowIndex shadowIndex = new ShadowIndex();

                                    shadowIndex.Index = StreamUtil.ReadUInt16(streamMatrix, true);
                                    shadowIndex.WeightIndex = StreamUtil.ReadUInt16(streamMatrix, true) / 3;

                                    TempIndexGroup.indexGroup.shadowIndices.Add(shadowIndex);
                                }
                            }


                            streamMatrix.Position = TempPos1;

                            TempTriData.indexGroupHeaders.Add(TempIndexGroup);
                        }

                        streamMatrix.Position = TempPos;
                        Model.meshHeaders.Add(TempTriData);
                    }


                    streamMatrix.Position = Model.OffsetVertexSection;
                    Model.Vertex = new List<VertexData>();
                    for (int b = 0; b < Model.NumVertices; b++)
                    {
                        VertexData vertexData = new VertexData();

                        vertexData.MorphDatas = new List<Vector3>();

                        //127.5 Probably but ill keep this at here
                        vertexData.Vertex.X = StreamUtil.ReadInt16(streamMatrix, true) / 127f;
                        vertexData.Vertex.Y = StreamUtil.ReadInt16(streamMatrix, true) / 127f;
                        vertexData.Vertex.Z = StreamUtil.ReadInt16(streamMatrix, true) / 127f;

                        //16383.5 Probably but ill keep this at here
                        vertexData.VertexNormal.X = StreamUtil.ReadInt16(streamMatrix, true) / 16384f;
                        vertexData.VertexNormal.Y = StreamUtil.ReadInt16(streamMatrix, true) / 16384f;
                        vertexData.VertexNormal.Z = StreamUtil.ReadInt16(streamMatrix, true) / 16384f;

                        vertexData.UV.X = StreamUtil.ReadUInt16(streamMatrix, true) / 65535f;
                        vertexData.UV.Y = StreamUtil.ReadUInt16(streamMatrix, true) / 65535f;

                        Model.Vertex.Add(vertexData);
                    }

                    //Add Morph Lists To VertexData
                    for (int a = 0; a < Model.Vertex.Count; a++)
                    {
                        var VertexData = Model.Vertex[a];
                        for (int b = 0; b < Model.NumMorphs; b++)
                        {
                            VertexData.MorphDatas.Add(new Vector3());
                        }
                        Model.Vertex[a] = VertexData;
                    }

                    //Add Morph to VertexData
                    for (int a = 0; a < Model.morphHeader.Count; a++)
                    {
                        var MorphHeader = Model.morphHeader[a];

                        for (int b = 0; b < MorphHeader.MorphDataList.Count; b++)
                        {
                            var MorphData = MorphHeader.MorphDataList[b];
                            var VertexData = Model.Vertex[MorphData.VertexIndex];
                            VertexData.MorphDatas[a] = MorphData.Morph;
                            Model.Vertex[MorphData.VertexIndex] = VertexData;
                        }
                    }

                    for (int a = 0; a < Model.meshHeaders.Count; a++)
                    {
                        var TempHeader = Model.meshHeaders[a];

                        for (int b = 0; b < TempHeader.indexGroupHeaders.Count; b++)
                        {
                            var TempIndexGroup = TempHeader.indexGroupHeaders[b];

                            bool roatation = false;
                            int Index = 2;

                            TempIndexGroup.faces = new List<Face>();
                            while (true)
                            {
                                var NewFace = new Face();
                                int Index1 = 0;
                                int Index2 = 0;
                                int Index3 = 0;
                                //Fixes the Rotation For Exporting
                                //Swap When Exporting to other formats
                                //1-Clockwise
                                //0-Counter Clocwise
                                if (roatation)
                                {
                                    Index1 = Index;
                                    Index2 = Index - 1;
                                    Index3 = Index - 2;
                                }
                                if (!roatation)
                                {
                                    Index1 = Index - 2;
                                    Index2 = Index - 1;
                                    Index3 = Index;
                                }


                                if (!Shadow)
                                {
                                    NewFace.V1 = Model.Vertex[TempIndexGroup.indexGroup.indices[Index1].Index0].Vertex;
                                    NewFace.UV1 = Model.Vertex[TempIndexGroup.indexGroup.indices[Index1].Index0].UV;
                                    NewFace.Normal1 = Model.Vertex[TempIndexGroup.indexGroup.indices[Index1].Index0].VertexNormal;
                                    NewFace.Weight1 = Model.boneWeightHeaders[TempHeader.WeightIndex[TempIndexGroup.indexGroup.indices[Index1].WeightIndex]];
                                    NewFace.Weight1Pos = TempHeader.WeightIndex[TempIndexGroup.indexGroup.indices[Index1].WeightIndex];
                                    NewFace.MorphPoint1 = Model.Vertex[TempIndexGroup.indexGroup.indices[Index1].Index0].MorphDatas;

                                    NewFace.V2 = Model.Vertex[TempIndexGroup.indexGroup.indices[Index2].Index0].Vertex;
                                    NewFace.UV2 = Model.Vertex[TempIndexGroup.indexGroup.indices[Index2].Index0].UV;
                                    NewFace.Normal2 = Model.Vertex[TempIndexGroup.indexGroup.indices[Index2].Index0].VertexNormal;
                                    NewFace.Weight2 = Model.boneWeightHeaders[TempHeader.WeightIndex[TempIndexGroup.indexGroup.indices[Index2].WeightIndex]];
                                    NewFace.Weight2Pos = TempHeader.WeightIndex[TempIndexGroup.indexGroup.indices[Index2].WeightIndex];
                                    NewFace.MorphPoint2 = Model.Vertex[TempIndexGroup.indexGroup.indices[Index2].Index0].MorphDatas;

                                    NewFace.V3 = Model.Vertex[TempIndexGroup.indexGroup.indices[Index3].Index0].Vertex;
                                    NewFace.UV3 = Model.Vertex[TempIndexGroup.indexGroup.indices[Index3].Index0].UV;
                                    NewFace.Normal3 = Model.Vertex[TempIndexGroup.indexGroup.indices[Index3].Index0].VertexNormal;
                                    NewFace.Weight3 = Model.boneWeightHeaders[TempHeader.WeightIndex[TempIndexGroup.indexGroup.indices[Index3].WeightIndex]];
                                    NewFace.Weight3Pos = TempHeader.WeightIndex[TempIndexGroup.indexGroup.indices[Index3].WeightIndex];
                                    NewFace.MorphPoint3 = Model.Vertex[TempIndexGroup.indexGroup.indices[Index3].Index0].MorphDatas;

                                    TempIndexGroup.faces.Add(NewFace);
                                    roatation = !roatation;
                                    Index++;
                                    if (Index >= TempIndexGroup.indexGroup.indices.Count)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    NewFace.V1 = Model.Vertex[TempIndexGroup.indexGroup.shadowIndices[Index1].Index].Vertex;
                                    NewFace.UV1 = Model.Vertex[TempIndexGroup.indexGroup.shadowIndices[Index1].Index].UV;
                                    NewFace.Normal1 = Model.Vertex[TempIndexGroup.indexGroup.shadowIndices[Index1].Index].VertexNormal;
                                    NewFace.Weight1 = Model.boneWeightHeaders[TempHeader.WeightIndex[TempIndexGroup.indexGroup.shadowIndices[Index1].WeightIndex]];
                                    NewFace.Weight1Pos = TempHeader.WeightIndex[TempIndexGroup.indexGroup.shadowIndices[Index1].WeightIndex];
                                    //NewFace.MorphPoint1 = Model.Vertex[TempHeader.IndexList[Index1]].MorphData;

                                    NewFace.V2 = Model.Vertex[TempIndexGroup.indexGroup.shadowIndices[Index2].Index].Vertex;
                                    NewFace.UV2 = Model.Vertex[TempIndexGroup.indexGroup.shadowIndices[Index2].Index].UV;
                                    NewFace.Normal2 = Model.Vertex[TempIndexGroup.indexGroup.shadowIndices[Index2].Index].VertexNormal;
                                    NewFace.Weight2 = Model.boneWeightHeaders[TempHeader.WeightIndex[TempIndexGroup.indexGroup.shadowIndices[Index2].WeightIndex]];
                                    NewFace.Weight2Pos = TempHeader.WeightIndex[TempIndexGroup.indexGroup.shadowIndices[Index2].WeightIndex];
                                    //NewFace.MorphPoint2 = Model.Vertex[TempHeader.IndexList[Index2]].MorphData;

                                    NewFace.V3 = Model.Vertex[TempIndexGroup.indexGroup.shadowIndices[Index3].Index].Vertex;
                                    NewFace.UV3 = Model.Vertex[TempIndexGroup.indexGroup.shadowIndices[Index3].Index].UV;
                                    NewFace.Normal3 = Model.Vertex[TempIndexGroup.indexGroup.shadowIndices[Index3].Index].VertexNormal;
                                    NewFace.Weight3 = Model.boneWeightHeaders[TempHeader.WeightIndex[TempIndexGroup.indexGroup.shadowIndices[Index3].WeightIndex]];
                                    NewFace.Weight3Pos = TempHeader.WeightIndex[TempIndexGroup.indexGroup.shadowIndices[Index3].WeightIndex];
                                    //NewFace.MorphPoint3 = Model.Vertex[TempHeader.IndexList[Index3]].MorphData;

                                    TempIndexGroup.faces.Add(NewFace);
                                    roatation = !roatation;
                                    Index++;
                                    if (Index >= TempIndexGroup.indexGroup.shadowIndices.Count)
                                    {
                                        break;
                                    }
                                }
                            }

                            TempHeader.indexGroupHeaders[b] = TempIndexGroup;
                        }

                        Model.meshHeaders[a] = TempHeader;
                    }

                    modelHeaders[i] = Model;
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
            public int OffsetMorphSection;

            public int OffsetSkinningSection;
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
            public int NumMeshPerSkin;
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
