using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SSXMultiTool.FileHandlers.Models
{
    public class OpenSSXMFHandler
    {
        public string MagicName = "OSMF";
        public int FormatVersion;
        public int ModelHeaderOffset;
        public int ModelHeaderCount;
        public int MaterialOffset;
        public int MaterialCount;
        public int BoneOffset;
        public int BoneCount;
        public int ModelDataOffset;

        public List<ModelHeader> modelHeaders = new List<ModelHeader>();
        public List<MaterialData> materialDatas = new List<MaterialData>();
        public List<BoneData> boneDatas = new List<BoneData>();

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                MagicName = StreamUtil.ReadString(stream, 4);
                FormatVersion = StreamUtil.ReadInt16(stream);
                ModelHeaderOffset = StreamUtil.ReadUInt32(stream);
                ModelHeaderCount = StreamUtil.ReadUInt32(stream);
                MaterialOffset = StreamUtil.ReadUInt32(stream);
                MaterialCount = StreamUtil.ReadUInt32(stream);
                BoneOffset = StreamUtil.ReadUInt32(stream);
                BoneCount = StreamUtil.ReadUInt32(stream);
                ModelDataOffset = StreamUtil.ReadUInt32(stream);

                stream.Position = ModelHeaderOffset;
                modelHeaders = new List<ModelHeader>();
                for (int i = 0; i < ModelHeaderCount; i++)
                {
                    var NewModelHeader = new ModelHeader();

                    NewModelHeader.ModelName = StreamUtil.ReadString(stream, 16);
                    NewModelHeader.ModelSize  = StreamUtil.ReadUInt32(stream);
                    NewModelHeader.IKOffset = StreamUtil.ReadUInt32(stream);
                    NewModelHeader.WeightOffset = StreamUtil.ReadUInt32(stream);
                    NewModelHeader.VertexOffset = StreamUtil.ReadUInt32(stream);
                    NewModelHeader.MorphOffset = StreamUtil.ReadUInt32(stream);
                    NewModelHeader.FaceOffset = StreamUtil.ReadUInt32(stream);

                    NewModelHeader.IKCount = StreamUtil.ReadUInt32(stream);
                    NewModelHeader.WeightCount = StreamUtil.ReadUInt32(stream);
                    NewModelHeader.VertexCount = StreamUtil.ReadUInt32(stream);
                    NewModelHeader.MorphCount = StreamUtil.ReadUInt32(stream);
                    NewModelHeader.FaceCount = StreamUtil.ReadUInt32(stream);
                    NewModelHeader.FileID = StreamUtil.ReadUInt32(stream);

                    modelHeaders.Add(NewModelHeader);
                }

                stream.Position = MaterialOffset;
                materialDatas = new List<MaterialData>();
                for (int i = 0; i < MaterialCount; i++)
                {
                    var TempMat = new MaterialData();
                    TempMat.MainTexture = StreamUtil.ReadString(stream, 4);
                    TempMat.Texture1 = StreamUtil.ReadString(stream, 4);
                    TempMat.Texture2 = StreamUtil.ReadString(stream, 4);
                    TempMat.Texture3 = StreamUtil.ReadString(stream, 4);
                    TempMat.Texture4 = StreamUtil.ReadString(stream, 4);

                    TempMat.FactorFloat = StreamUtil.ReadFloat(stream);
                    TempMat.Unused1Float = StreamUtil.ReadFloat(stream);
                    TempMat.Unused2Float = StreamUtil.ReadFloat(stream);
                    materialDatas.Add(TempMat);
                }

                stream.Position = BoneOffset;
                boneDatas = new List<BoneData>();
                for (int i = 0; i < BoneCount; i++)
                {
                    var TempBone = new BoneData();
                    TempBone.BoneName = StreamUtil.ReadString(stream, 16);
                    TempBone.ParentFileID = StreamUtil.ReadInt16(stream);
                    TempBone.ParentBone = StreamUtil.ReadInt16(stream);
                    TempBone.FileID = StreamUtil.ReadInt16(stream);
                    TempBone.BoneID = StreamUtil.ReadInt16(stream);

                    TempBone.Position = StreamUtil.ReadVector3(stream);
                    TempBone.Rotation = StreamUtil.ReadQuaternion(stream);

                }

                for (int i = 0; i < modelHeaders.Count; i++)
                {
                    stream.Position = ModelDataOffset + modelHeaders[i].ModelOffset;
                    ModelHeader modelHandler = modelHeaders[i];
                    modelHandler.Matrix = StreamUtil.ReadBytes(stream, modelHeaders[i].ModelSize);
                    modelHeaders[i] = modelHandler;
                }

                for (int i = 0; i < modelHeaders.Count; i++)
                {
                    Stream streamMatrix = new MemoryStream();
                    streamMatrix.Write(modelHeaders[i].Matrix, 0, modelHeaders[i].Matrix.Length);
                    streamMatrix.Position = 0;

                    var TempModel = modelHeaders[i];
                    streamMatrix.Position = TempModel.ModelOffset;
                    TempModel.ikPoints = new List<Vector3>();
                    for (int a = 0; a < TempModel.IKCount; a++)
                    {
                        TempModel.ikPoints.Add(StreamUtil.ReadVector3(stream));
                    }

                    TempModel.boneWeightHeaders = new List<BoneWeightHeader>();
                    for (int b = 0; b < TempModel.WeightCount; b++)
                    {
                        var BoneWeight = new BoneWeightHeader();

                        BoneWeight.WeightCount = StreamUtil.ReadUInt32(streamMatrix);
                        BoneWeight.WeightListOffset = StreamUtil.ReadUInt32(streamMatrix);
                        BoneWeight.boneWeights = new List<BoneWeight>();
                        int TempPos = (int)streamMatrix.Position;
                        streamMatrix.Position = BoneWeight.WeightListOffset;
                        for (int a = 0; a < BoneWeight.WeightCount; a++)
                        {
                            var boneWeight = new BoneWeight();
                            boneWeight.Weight = StreamUtil.ReadInt16(streamMatrix);
                            boneWeight.BoneID = StreamUtil.ReadUInt8(streamMatrix);
                            boneWeight.FileID = StreamUtil.ReadUInt8(streamMatrix);
                            BoneWeight.boneWeights.Add(boneWeight);
                        }
                        streamMatrix.Position = TempPos;
                        TempModel.boneWeightHeaders.Add(BoneWeight);
                    }

                    streamMatrix.Position = TempModel.VertexOffset;
                    TempModel.vertexDatas = new List<VertexData>();
                    for (int a = 0; a < TempModel.VertexCount; a++)
                    {
                        var TempVertexData = new VertexData();

                        TempVertexData.VertexPosition = StreamUtil.ReadVector3(streamMatrix);
                        TempVertexData.VertexNormal = StreamUtil.ReadVector3(streamMatrix);
                        TempVertexData.VertexUV = StreamUtil.ReadVector2(streamMatrix);
                        TempVertexData.WeightIndex = StreamUtil.ReadUInt32(streamMatrix);

                        TempVertexData.MorphData = new List<Vector3>();

                        TempModel.vertexDatas.Add(TempVertexData);
                    }

                    streamMatrix.Position = TempModel.MorphOffset;
                    TempModel.morphDatas = new List<MorphHeader>();
                    for (int a = 0; a < TempModel.MorphCount; a++)
                    {
                        var TempMorph = new MorphHeader();

                        TempMorph.MorphName = StreamUtil.ReadString(streamMatrix, 16);

                        TempMorph.NumMorphData = StreamUtil.ReadUInt32(streamMatrix);
                        TempMorph.OffsetMorphDataList = StreamUtil.ReadUInt32(streamMatrix);

                        TempMorph.MorphDataList = new List<MorphData>();
                        int TempPos = (int)streamMatrix.Position;
                        streamMatrix.Position = TempMorph.OffsetMorphDataList;
                        for (int b = 0; b < TempMorph.NumMorphData; b++)
                        {
                            var TempMorphData = new MorphData();

                            TempMorphData.Morph = StreamUtil.ReadVector3(streamMatrix);
                            TempMorphData.VertexIndex = StreamUtil.ReadUInt16(streamMatrix);
                            TempMorph.MorphDataList.Add(TempMorphData);
                        }
                        streamMatrix.Position = TempPos;
                        TempModel.morphDatas.Add(TempMorph);

                    }

                    streamMatrix.Position = TempModel.FaceOffset;
                    TempModel.faces = new List<Face>();
                    for (int a = 0; a < TempModel.FaceCount; a++)
                    {
                        var TempNewFace = new Face();
                        TempNewFace.V1Pos = StreamUtil.ReadUInt32(streamMatrix);
                        TempNewFace.V2Pos = StreamUtil.ReadUInt32(streamMatrix);
                        TempNewFace.V3Pos = StreamUtil.ReadUInt32(streamMatrix);
                        TempNewFace.MaterialID = StreamUtil.ReadUInt32(streamMatrix);
                        TempModel.faces.Add(TempNewFace);
                    }

                    modelHeaders[i] = TempModel;
                }


            }
        }




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
            public byte[] Matrix;

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
            public int FileID;
            public int BoneID;

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
