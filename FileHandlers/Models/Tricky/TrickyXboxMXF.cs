using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers.Models.Tricky
{
    public class TrickyXboxMXF
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
                NumModels = StreamUtil.ReadInt16(stream);
                OffsetModelHeader = StreamUtil.ReadInt16(stream);
                OffsetModelData = StreamUtil.ReadUInt32(stream);

                modelHeaders = new List<ModelHeader>();
                for (int i = 0; i < NumModels; i++)
                {
                    var TempModelHeader = new ModelHeader();
                    TempModelHeader.ModelName = StreamUtil.ReadString(stream, 16);
                    TempModelHeader.ModelOffset = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.ModelSize = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.OffsetBoneData = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.OffsetBoneData2 = StreamUtil.ReadUInt32(stream);

                    TempModelHeader.OffsetMateralList = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.OffsetBoneData3 = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.OffsetIKPointList = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.OffsetMorphList = StreamUtil.ReadUInt32(stream);

                    TempModelHeader.OffsetWeight = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.OffsetTristripSection = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.Unknown0 = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.OffsetVertexSection = StreamUtil.ReadUInt32(stream);

                    TempModelHeader.OffsetLastData = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.OffsetUnknownData = StreamUtil.ReadUInt32(stream);

                    stream.Position += 0x12A/*298*/;

                    TempModelHeader.Unknown1 = StreamUtil.ReadInt16(stream);
                    TempModelHeader.Unknown2 = StreamUtil.ReadInt16(stream);
                    TempModelHeader.NumBones = StreamUtil.ReadInt16(stream);
                    TempModelHeader.NumMorphs = StreamUtil.ReadInt16(stream);
                    TempModelHeader.NumMaterials = StreamUtil.ReadInt16(stream);
                    TempModelHeader.NumIKPoints = StreamUtil.ReadInt16(stream);
                    TempModelHeader.NumWeights = StreamUtil.ReadInt16(stream);
                    TempModelHeader.NumTristripGroups = StreamUtil.ReadInt16(stream);
                    TempModelHeader.Unknown3 = StreamUtil.ReadInt16(stream);
                    TempModelHeader.NumVertices = StreamUtil.ReadInt16(stream);
                    TempModelHeader.FileID = StreamUtil.ReadInt16(stream);
                    TempModelHeader.NumLastData = StreamUtil.ReadInt16(stream);
                    TempModelHeader.NumUnknownData = StreamUtil.ReadInt16(stream);

                    modelHeaders.Add(TempModelHeader);
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
                        TempBoneData.ParentFileID = StreamUtil.ReadInt16(streamMatrix);
                        TempBoneData.ParentBone = StreamUtil.ReadInt16(streamMatrix);
                        TempBoneData.Unknown2 = StreamUtil.ReadInt16(streamMatrix);
                        TempBoneData.BoneID = StreamUtil.ReadInt16(streamMatrix);
                        TempBoneData.Position = StreamUtil.ReadVector3(streamMatrix);
                        TempBoneData.Radians = StreamUtil.ReadVector3(streamMatrix);
                        TempBoneData.XRadian2 = StreamUtil.ReadFloat(streamMatrix);
                        TempBoneData.YRadian2 = StreamUtil.ReadFloat(streamMatrix);
                        TempBoneData.ZRadian2 = StreamUtil.ReadFloat(streamMatrix);

                        TempBoneData.UnknownFloat1 = StreamUtil.ReadFloat(streamMatrix);
                        TempBoneData.UnknownFloat2 = StreamUtil.ReadFloat(streamMatrix);
                        TempBoneData.UnknownFloat3 = StreamUtil.ReadFloat(streamMatrix);
                        TempBoneData.UnknownFloat4 = StreamUtil.ReadFloat(streamMatrix);
                        TempBoneData.UnknownFloat5 = StreamUtil.ReadFloat(streamMatrix);
                        TempBoneData.UnknownFloat6 = StreamUtil.ReadFloat(streamMatrix);

                        TempBoneData.FileID = Model.FileID;
                        TempBoneData.BonePos = a;

                        Model.boneDatas.Add(TempBoneData);
                    }

                    streamMatrix.Position = Model.OffsetIKPointList;
                    Model.iKPoints = new List<Vector3>();
                    for (int a = 0; a < Model.NumIKPoints; a++)
                    {
                        var TempIKPoint = new Vector3();
                        TempIKPoint = StreamUtil.ReadVector3(streamMatrix);
                        streamMatrix.Position += 4;
                        Model.iKPoints.Add(TempIKPoint);
                    }

                    streamMatrix.Position = Model.OffsetMorphList;
                    Model.morphHeader = new List<MorphHeader>();
                    for (int a = 0; a < Model.NumMorphs; a++)
                    {
                        var TempMorph = new MorphHeader();

                        TempMorph.NumMorphData = StreamUtil.ReadUInt32(streamMatrix);
                        TempMorph.OffsetMorphDataList = StreamUtil.ReadUInt32(streamMatrix);

                        TempMorph.MorphDataList = new List<MorphData>();
                        int TempPos = (int)streamMatrix.Position;
                        streamMatrix.Position = TempMorph.OffsetMorphDataList;
                        for (int b = 0; b < TempMorph.NumMorphData; b++)
                        {
                            var TempMorphData= new MorphData();

                            TempMorphData.Morph = StreamUtil.ReadVector3(streamMatrix);
                            TempMorphData.VertexIndex = StreamUtil.ReadUInt16(streamMatrix);
                            TempMorphData.U1 = StreamUtil.ReadUInt8(streamMatrix);
                            TempMorphData.U2 = StreamUtil.ReadUInt8(streamMatrix);
                            TempMorph.MorphDataList.Add(TempMorphData);
                        }
                        streamMatrix.Position = TempPos;
                        Model.morphHeader.Add(TempMorph);

                    }

                    streamMatrix.Position = Model.OffsetWeight;
                    Model.boneWeightHeaders = new List<BoneWeightHeader>();
                    for (int b = 0; b < Model.NumWeights; b++)
                    {
                        var BoneWeight = new BoneWeightHeader();

                        BoneWeight.WeightCount = StreamUtil.ReadUInt32(streamMatrix);
                        BoneWeight.WeightListOffset = StreamUtil.ReadUInt32(streamMatrix);
                        BoneWeight.Unknown1 = StreamUtil.ReadUInt16(streamMatrix);
                        BoneWeight.Unknown2 = StreamUtil.ReadUInt16(streamMatrix);
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
                        Model.boneWeightHeaders.Add(BoneWeight);
                    }

                    streamMatrix.Position = Model.OffsetTristripSection;
                    Model.tristripHeaders = new List<TristripHeader>();

                    for (int a = 0; a < Model.NumTristripGroups; a++)
                    {
                        var TempTristrip = new TristripHeader();
                        TempTristrip.IndexListOffset = StreamUtil.ReadUInt32(streamMatrix);
                        TempTristrip.NumIndices = StreamUtil.ReadUInt16(streamMatrix);
                        TempTristrip.MaterialIndex0 = StreamUtil.ReadUInt16(streamMatrix);
                        TempTristrip.MaterialIndex1 = StreamUtil.ReadUInt16(streamMatrix);
                        TempTristrip.MaterialIndex2 = StreamUtil.ReadUInt16(streamMatrix);
                        TempTristrip.MaterialIndex3 = StreamUtil.ReadUInt16(streamMatrix);
                        TempTristrip.MaterialIndex4 = StreamUtil.ReadUInt16(streamMatrix);

                        TempTristrip.IndexList = new List<int>();
                        int TempPos = (int)streamMatrix.Position;
                        streamMatrix.Position = TempTristrip.IndexListOffset;
                        for (int b = 0; b < TempTristrip.NumIndices; b++)
                        {
                            TempTristrip.IndexList.Add(StreamUtil.ReadUInt16(streamMatrix));
                        }

                        streamMatrix.Position = TempPos;
                        Model.tristripHeaders.Add(TempTristrip);
                    }

                    streamMatrix.Position = Model.OffsetVertexSection;
                    Model.vertexDatas = new List<VertexData>();
                    for (int a = 0; a < Model.NumVertices; a++)
                    {
                        var TempVertexData = new VertexData();

                        TempVertexData.VertexPosition = StreamUtil.ReadVector3(streamMatrix);
                        TempVertexData.Unknown1 = StreamUtil.ReadFloat(streamMatrix);
                        TempVertexData.VertexNormal = StreamUtil.ReadVector3(streamMatrix);
                        TempVertexData.Unknown2 = StreamUtil.ReadFloat(streamMatrix);
                        TempVertexData.VertexTangentNormal = StreamUtil.ReadVector3(streamMatrix);
                        TempVertexData.Unknown3 = StreamUtil.ReadFloat(streamMatrix);
                        TempVertexData.VertexUV = StreamUtil.ReadVector2(streamMatrix);
                        TempVertexData.Unknown4 = StreamUtil.ReadUInt32(streamMatrix);
                        TempVertexData.WeightIndex = StreamUtil.ReadUInt32(streamMatrix);

                        TempVertexData.MorphData = new List<Vector3>();

                        Model.vertexDatas.Add(TempVertexData);
                    }

                    Model.unknownVertexData = new List<VertexData>();
                    for (int a = 0; a < Model.NumVertices; a++)
                    {
                        var TempVertexData = new VertexData();

                        TempVertexData.VertexPosition = StreamUtil.ReadVector3(streamMatrix);
                        TempVertexData.Unknown1 = StreamUtil.ReadFloat(streamMatrix);
                        TempVertexData.VertexNormal = StreamUtil.ReadVector3(streamMatrix);
                        TempVertexData.Unknown2 = StreamUtil.ReadFloat(streamMatrix);
                        TempVertexData.VertexTangentNormal = StreamUtil.ReadVector3(streamMatrix);
                        TempVertexData.Unknown3 = StreamUtil.ReadFloat(streamMatrix);
                        TempVertexData.VertexUV = StreamUtil.ReadVector2(streamMatrix);
                        TempVertexData.Unknown4 = StreamUtil.ReadUInt32(streamMatrix);
                        TempVertexData.WeightIndex = StreamUtil.ReadUInt32(streamMatrix);

                        TempVertexData.MorphData = new List<Vector3>();

                        Model.unknownVertexData.Add(TempVertexData);
                    }

                    stream.Position = Model.OffsetUnknownData;
                    Model.unknownDatas = new List<UnknownData>();
                    for (int a = 0; a < Model.NumUnknownData; a++)
                    {
                        var TempUnknownData= new UnknownData();

                        TempUnknownData.U1 = StreamUtil.ReadUInt16(streamMatrix);
                        TempUnknownData.U2 = StreamUtil.ReadUInt16(streamMatrix);
                        TempUnknownData.U3 = StreamUtil.ReadUInt16(streamMatrix);
                        TempUnknownData.U4 = StreamUtil.ReadUInt16(streamMatrix);

                        Model.unknownDatas.Add(TempUnknownData);
                    }

                    stream.Position = Model.OffsetLastData;
                    Model.lastDatas = new List<LastData>();
                    for (int a = 0; a < Model.NumLastData; a++)
                    {
                        var TempLastData = new LastData();
                        TempLastData.U1 = StreamUtil.ReadUInt16(streamMatrix);
                        TempLastData.U2 = StreamUtil.ReadUInt16(streamMatrix);
                        TempLastData.U3 = StreamUtil.ReadUInt16(streamMatrix);
                        TempLastData.U4 = StreamUtil.ReadUInt16(streamMatrix);
                        TempLastData.U5 = StreamUtil.ReadUInt16(streamMatrix);
                        TempLastData.U6 = StreamUtil.ReadUInt16(streamMatrix);
                        TempLastData.U7 = StreamUtil.ReadUInt16(streamMatrix);
                        TempLastData.U8 = StreamUtil.ReadUInt16(streamMatrix);
                        TempLastData.U9 = StreamUtil.ReadUInt16(streamMatrix);
                        TempLastData.U10 = StreamUtil.ReadUInt16(streamMatrix);
                        Model.lastDatas.Add(TempLastData);
                    }

                    //Add Morph Lists To VertexData
                    for (int a = 0; a < Model.vertexDatas.Count; a++)
                    {
                        var VertexData = Model.vertexDatas[a];
                        for (int b = 0; b < Model.NumMorphs; b++)
                        {
                            VertexData.MorphData.Add(new Vector3());
                        }
                        Model.vertexDatas[a] = VertexData;
                    }

                    //Add Morph to VertexData
                    for (int a = 0; a < Model.morphHeader.Count; a++)
                    {
                        var MorphHeader = Model.morphHeader[a];

                        for (int b = 0; b < MorphHeader.MorphDataList.Count; b++)
                        {
                            var MorphData = MorphHeader.MorphDataList[b];
                            var VertexData = Model.vertexDatas[MorphData.VertexIndex];
                            VertexData.MorphData[a] = MorphData.Morph;
                            Model.vertexDatas[MorphData.VertexIndex] = VertexData;
                        }
                    }


                    //Generate the Faces
                    for (int a = 0; a < Model.tristripHeaders.Count; a++)
                    {
                        var TempHeader = Model.tristripHeaders[a];
                        bool roatation = false;
                        int Index = 2;
                        TempHeader.faces = new List<Face>();
                        while(true)
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

                            NewFace.V1 = Model.vertexDatas[TempHeader.IndexList[Index1]].VertexPosition;
                            NewFace.UV1 = Model.vertexDatas[TempHeader.IndexList[Index1]].VertexUV;
                            NewFace.Normal1 = Model.vertexDatas[TempHeader.IndexList[Index1]].VertexNormal;
                            NewFace.TangentNormal1 = Model.vertexDatas[TempHeader.IndexList[Index1]].VertexTangentNormal;
                            NewFace.Weight1 = Model.boneWeightHeaders[Model.vertexDatas[TempHeader.IndexList[Index1]].WeightIndex];
                            NewFace.Weight1Pos = Model.vertexDatas[TempHeader.IndexList[Index1]].WeightIndex;
                            NewFace.MorphPoint1 = Model.vertexDatas[TempHeader.IndexList[Index1]].MorphData;

                            NewFace.V2 = Model.vertexDatas[TempHeader.IndexList[Index2]].VertexPosition;
                            NewFace.UV2 = Model.vertexDatas[TempHeader.IndexList[Index2]].VertexUV;
                            NewFace.Normal2 = Model.vertexDatas[TempHeader.IndexList[Index2]].VertexNormal;
                            NewFace.TangentNormal2 = Model.vertexDatas[TempHeader.IndexList[Index2]].VertexTangentNormal;
                            NewFace.Weight2 = Model.boneWeightHeaders[Model.vertexDatas[TempHeader.IndexList[Index2]].WeightIndex];
                            NewFace.Weight2Pos = Model.vertexDatas[TempHeader.IndexList[Index2]].WeightIndex;
                            NewFace.MorphPoint2 = Model.vertexDatas[TempHeader.IndexList[Index2]].MorphData;

                            NewFace.V3 = Model.vertexDatas[TempHeader.IndexList[Index3]].VertexPosition;
                            NewFace.UV3 = Model.vertexDatas[TempHeader.IndexList[Index3]].VertexUV;
                            NewFace.Normal3 = Model.vertexDatas[TempHeader.IndexList[Index3]].VertexNormal;
                            NewFace.TangentNormal3 = Model.vertexDatas[TempHeader.IndexList[Index3]].VertexTangentNormal;
                            NewFace.Weight3 = Model.boneWeightHeaders[Model.vertexDatas[TempHeader.IndexList[Index3]].WeightIndex];
                            NewFace.Weight3Pos = Model.vertexDatas[TempHeader.IndexList[Index3]].WeightIndex;
                            NewFace.MorphPoint3 = Model.vertexDatas[TempHeader.IndexList[Index3]].MorphData;


                            TempHeader.faces.Add(NewFace);
                            roatation = !roatation;
                            Index++;
                            if(Index>= TempHeader.IndexList.Count)
                            {
                                break;
                            }
                        }

                        Model.tristripHeaders[a] = TempHeader;
                    }

                    modelHeaders[i] = Model;
                }

            }
        }

        public void Save(string Path)
        {
            MemoryStream stream = new MemoryStream();

            StreamUtil.WriteBytes(stream, Version);
            StreamUtil.WriteInt16(stream, modelHeaders.Count);
            StreamUtil.WriteInt16(stream, 12);
            OffsetModelData = 12 + 396* modelHeaders.Count;
            StreamUtil.WriteInt32(stream, OffsetModelData);
            stream.Position = OffsetModelData;

            for (int i = 0; i < modelHeaders.Count; i++)
            {
                var Model = modelHeaders[i];
                if (Model.vertexDatas != null)
                {
                    if (Model.vertexDatas.Count != 0)
                    {
                        MemoryStream ModelStream = new MemoryStream();
                        Model.OffsetMateralList = (int)ModelStream.Position;
                        for (int a = 0; a < Model.materialDatas.Count; a++)
                        {
                            StreamUtil.WriteString(ModelStream, Model.materialDatas[a].MainTexture, 4);

                            if (Model.materialDatas[a].Texture1 != "")
                            {
                                StreamUtil.WriteString(ModelStream, Model.materialDatas[a].Texture1, 4);
                            }
                            else
                            {
                                StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x20, 0x20, 0x20 });
                            }

                            if (Model.materialDatas[a].Texture2 != "")
                            {
                                StreamUtil.WriteString(ModelStream, Model.materialDatas[a].Texture2, 4);
                            }
                            else
                            {
                                StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x20, 0x20, 0x20 });
                            }

                            if (Model.materialDatas[a].Texture3 != "")
                            {
                                StreamUtil.WriteString(ModelStream, Model.materialDatas[a].Texture3, 4);
                            }
                            else
                            {
                                StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x20, 0x20, 0x20 });
                            }

                            if (Model.materialDatas[a].Texture4 != "")
                            {
                                StreamUtil.WriteString(ModelStream, Model.materialDatas[a].Texture4, 4);
                            }
                            else
                            {
                                StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x20, 0x20, 0x20 });
                            }


                            StreamUtil.WriteFloat32(ModelStream, Model.materialDatas[a].FactorFloat);
                            StreamUtil.WriteFloat32(ModelStream, Model.materialDatas[a].Unused1Float);
                            StreamUtil.WriteFloat32(ModelStream, Model.materialDatas[a].Unused2Float);
                        }

                        Model.OffsetBoneData = (int)ModelStream.Position;
                        Model.OffsetBoneData2 = (int)ModelStream.Position;
                        Model.OffsetBoneData3 = (int)ModelStream.Position;
                        for (int a = 0; a < Model.boneDatas.Count; a++)
                        {
                            StreamUtil.WriteString(ModelStream, Model.boneDatas[a].BoneName, 16);
                            StreamUtil.WriteInt16(ModelStream, Model.boneDatas[a].ParentFileID);
                            StreamUtil.WriteInt16(ModelStream, Model.boneDatas[a].ParentBone);
                            StreamUtil.WriteInt16(ModelStream, Model.boneDatas[a].Unknown2);
                            StreamUtil.WriteInt16(ModelStream, Model.boneDatas[a].BoneID);

                            StreamUtil.WriteVector3(ModelStream, Model.boneDatas[a].Position);

                            StreamUtil.WriteVector3(ModelStream, Model.boneDatas[a].Radians);
                            StreamUtil.WriteFloat32(ModelStream, Model.boneDatas[a].XRadian2);
                            StreamUtil.WriteFloat32(ModelStream, Model.boneDatas[a].YRadian2);
                            StreamUtil.WriteFloat32(ModelStream, Model.boneDatas[a].ZRadian2);

                            StreamUtil.WriteFloat32(ModelStream, Model.boneDatas[a].UnknownFloat1);
                            StreamUtil.WriteFloat32(ModelStream, Model.boneDatas[a].UnknownFloat2);
                            StreamUtil.WriteFloat32(ModelStream, Model.boneDatas[a].UnknownFloat3);
                            StreamUtil.WriteFloat32(ModelStream, Model.boneDatas[a].UnknownFloat4);
                            StreamUtil.WriteFloat32(ModelStream, Model.boneDatas[a].UnknownFloat5);
                            StreamUtil.WriteFloat32(ModelStream, Model.boneDatas[a].UnknownFloat6);
                        }

                        Model.OffsetIKPointList = (int)ModelStream.Position;
                        for (int a = 0; a < Model.iKPoints.Count; a++)
                        {
                            StreamUtil.WriteVector3(ModelStream, Model.iKPoints[a]);
                            ModelStream.Position += 4;
                        }

                        WritePadding(ModelStream, 2);

                        //Morph

                        Model.OffsetMorphList = (int)ModelStream.Position;
                        ModelStream.Position += 8 * Model.morphHeader.Count;
                        
                        for (int a = 0; a < Model.morphHeader.Count; a++)
                        {
                            var TempMorph = Model.morphHeader[a];
                            TempMorph.OffsetMorphDataList = (int)ModelStream.Position;
                            for (int b = 0; b < TempMorph.MorphDataList.Count; b++)
                            {
                                StreamUtil.WriteVector3(ModelStream, TempMorph.MorphDataList[b].Morph);
                                StreamUtil.WriteInt16(ModelStream, TempMorph.MorphDataList[b].VertexIndex);
                                StreamUtil.WriteUInt8(ModelStream, TempMorph.MorphDataList[b].U1);
                                StreamUtil.WriteUInt8(ModelStream, TempMorph.MorphDataList[b].U2);
                            }
                            Model.morphHeader[a] = TempMorph;
                        }

                        Model.OffsetWeight = (int)ModelStream.Position;
                        ModelStream.Position = Model.OffsetMorphList;

                        for (int a = 0; a < Model.morphHeader.Count; a++)
                        {
                            StreamUtil.WriteInt32(ModelStream, Model.morphHeader[a].MorphDataList.Count);
                            StreamUtil.WriteInt32(ModelStream, Model.morphHeader[a].OffsetMorphDataList);
                        }


                        //Weight
                        ModelStream.Position = Model.OffsetWeight;
                        ModelStream.Position += 12 * Model.boneWeightHeaders.Count;
                        for (int a = 0; a < Model.boneWeightHeaders.Count; a++)
                        {
                            var TempMorph = Model.boneWeightHeaders[a];
                            TempMorph.WeightListOffset = (int)ModelStream.Position;
                            for (int b = 0; b < TempMorph.boneWeights.Count; b++)
                            {
                                StreamUtil.WriteInt16(ModelStream, TempMorph.boneWeights[b].Weight);
                                StreamUtil.WriteUInt8(ModelStream, TempMorph.boneWeights[b].BoneID);
                                StreamUtil.WriteUInt8(ModelStream, TempMorph.boneWeights[b].FileID);
                            }
                            Model.boneWeightHeaders[a] = TempMorph;
                        }

                        Model.OffsetTristripSection = (int)ModelStream.Position;
                        ModelStream.Position = Model.OffsetWeight;

                        for (int a = 0; a < Model.boneWeightHeaders.Count; a++)
                        {
                            StreamUtil.WriteInt32(ModelStream, Model.boneWeightHeaders[a].boneWeights.Count);
                            StreamUtil.WriteInt32(ModelStream, Model.boneWeightHeaders[a].WeightListOffset);
                            StreamUtil.WriteInt16(ModelStream, Model.boneWeightHeaders[a].Unknown1);
                            StreamUtil.WriteInt16(ModelStream, Model.boneWeightHeaders[a].Unknown2);
                        }

                        //Tristrip
                        ModelStream.Position = Model.OffsetTristripSection;
                        ModelStream.Position += 16 * Model.tristripHeaders.Count;

                        for (int a = 0; a < Model.tristripHeaders.Count; a++)
                        {
                            var TempTristrip = Model.tristripHeaders[a];
                            TempTristrip.IndexListOffset = (int)ModelStream.Position;
                            for (int b = 0; b < TempTristrip.IndexList.Count; b++)
                            {
                                StreamUtil.WriteInt16(ModelStream, TempTristrip.IndexList[b]);
                            }
                            Model.tristripHeaders[a] = TempTristrip;
                        }

                        WritePadding(ModelStream, 1);
                        Model.OffsetVertexSection = (int)ModelStream.Position;
                        ModelStream.Position = Model.OffsetTristripSection;

                        for (int a = 0; a < Model.tristripHeaders.Count; a++)
                        {
                            StreamUtil.WriteInt32(ModelStream, Model.tristripHeaders[a].IndexListOffset);
                            StreamUtil.WriteInt16(ModelStream, Model.tristripHeaders[a].IndexList.Count);
                            StreamUtil.WriteInt16(ModelStream, Model.tristripHeaders[a].MaterialIndex0);
                            StreamUtil.WriteInt16(ModelStream, Model.tristripHeaders[a].MaterialIndex1);
                            StreamUtil.WriteInt16(ModelStream, Model.tristripHeaders[a].MaterialIndex2);
                            StreamUtil.WriteInt16(ModelStream, Model.tristripHeaders[a].MaterialIndex3);
                            StreamUtil.WriteInt16(ModelStream, Model.tristripHeaders[a].MaterialIndex4);
                        }

                        //VertexData
                        ModelStream.Position = Model.OffsetVertexSection;
                        for (int a = 0; a < Model.vertexDatas.Count; a++)
                        {
                            StreamUtil.WriteVector3(ModelStream, Model.vertexDatas[a].VertexPosition);
                            StreamUtil.WriteFloat32(ModelStream, Model.vertexDatas[a].Unknown1);
                            StreamUtil.WriteVector3(ModelStream, Model.vertexDatas[a].VertexNormal);
                            StreamUtil.WriteFloat32(ModelStream, Model.vertexDatas[a].Unknown2);
                            StreamUtil.WriteVector3(ModelStream, Model.vertexDatas[a].VertexTangentNormal);
                            StreamUtil.WriteFloat32(ModelStream, Model.vertexDatas[a].Unknown3);
                            StreamUtil.WriteVector2(ModelStream, Model.vertexDatas[a].VertexUV);
                            StreamUtil.WriteInt32(ModelStream, Model.vertexDatas[a].Unknown4);
                            StreamUtil.WriteInt32(ModelStream, Model.vertexDatas[a].WeightIndex);
                        }

                        for (int a = 0; a < Model.unknownVertexData.Count; a++)
                        {
                            StreamUtil.WriteVector3(ModelStream, Model.unknownVertexData[a].VertexPosition);
                            StreamUtil.WriteFloat32(ModelStream, Model.unknownVertexData[a].Unknown1);
                            StreamUtil.WriteVector3(ModelStream, Model.unknownVertexData[a].VertexNormal);
                            StreamUtil.WriteFloat32(ModelStream, Model.unknownVertexData[a].Unknown2);
                            StreamUtil.WriteVector3(ModelStream, Model.unknownVertexData[a].VertexTangentNormal);
                            StreamUtil.WriteFloat32(ModelStream, Model.unknownVertexData[a].Unknown3);
                            StreamUtil.WriteVector2(ModelStream, Model.unknownVertexData[a].VertexUV);
                            StreamUtil.WriteInt32(ModelStream, Model.unknownVertexData[a].Unknown4);
                            StreamUtil.WriteInt32(ModelStream, Model.unknownVertexData[a].WeightIndex);
                        }

                        Model.OffsetUnknownData = (int)ModelStream.Position;
                        for (int a = 0; a < Model.unknownDatas.Count; a++)
                        {
                            StreamUtil.WriteInt16(ModelStream, Model.unknownDatas[a].U1);
                            StreamUtil.WriteInt16(ModelStream, Model.unknownDatas[a].U2);
                            StreamUtil.WriteInt16(ModelStream, Model.unknownDatas[a].U3);
                            StreamUtil.WriteInt16(ModelStream, Model.unknownDatas[a].U4);
                        }

                        Model.OffsetLastData = (int)ModelStream.Position;
                        for (int a = 0; a < Model.lastDatas.Count; a++)
                        {
                            StreamUtil.WriteInt16(ModelStream, Model.lastDatas[a].U1);
                            StreamUtil.WriteInt16(ModelStream, Model.lastDatas[a].U2);
                            StreamUtil.WriteInt16(ModelStream, Model.lastDatas[a].U3);
                            StreamUtil.WriteInt16(ModelStream, Model.lastDatas[a].U4);
                            StreamUtil.WriteInt16(ModelStream, Model.lastDatas[a].U5);
                            StreamUtil.WriteInt16(ModelStream, Model.lastDatas[a].U6);
                            StreamUtil.WriteInt16(ModelStream, Model.lastDatas[a].U7);
                            StreamUtil.WriteInt16(ModelStream, Model.lastDatas[a].U8);
                            StreamUtil.WriteInt16(ModelStream, Model.lastDatas[a].U9);
                            StreamUtil.WriteInt16(ModelStream, Model.lastDatas[a].U10);
                        }


                        ModelStream.Position = 0;
                        Model.ModelSize = (int)ModelStream.Length;
                        Model.ModelOffset = (int)(stream.Position - OffsetModelData);
                        StreamUtil.WriteStreamIntoStream(stream, ModelStream);
                        ModelStream.Dispose();
                        ModelStream.Close();
                        modelHeaders[i] = Model;
                    }
                }
            }

            //Write Headers
            stream.Position = 12;
            for (int i = 0; i < modelHeaders.Count; i++)
            {
                var Model = modelHeaders[i];

                StreamUtil.WriteString(stream, Model.ModelName, 16);
                StreamUtil.WriteInt32(stream, Model.ModelOffset); //done
                StreamUtil.WriteInt32(stream, Model.ModelSize); //done
                StreamUtil.WriteInt32(stream, Model.OffsetBoneData); //done
                StreamUtil.WriteInt32(stream, Model.OffsetBoneData2); //done

                StreamUtil.WriteInt32(stream, Model.OffsetMateralList); //done
                StreamUtil.WriteInt32(stream, Model.OffsetBoneData3); //done
                StreamUtil.WriteInt32(stream, Model.OffsetIKPointList); //done
                StreamUtil.WriteInt32(stream, Model.OffsetMorphList);

                StreamUtil.WriteInt32(stream, Model.OffsetWeight);
                StreamUtil.WriteInt32(stream, Model.OffsetTristripSection);
                StreamUtil.WriteInt32(stream, Model.Unknown0); //done
                StreamUtil.WriteInt32(stream, Model.OffsetVertexSection);

                StreamUtil.WriteInt32(stream, Model.OffsetLastData);
                StreamUtil.WriteInt32(stream, Model.OffsetUnknownData);

                stream.Position += 28;
                StreamUtil.WriteInt32(stream, -1);
                stream.Position += 266;

                StreamUtil.WriteInt16(stream, Model.Unknown1);
                StreamUtil.WriteInt16(stream, Model.Unknown2);
                StreamUtil.WriteInt16(stream, Model.boneDatas.Count);
                StreamUtil.WriteInt16(stream, Model.morphHeader.Count);
                StreamUtil.WriteInt16(stream, Model.materialDatas.Count);
                StreamUtil.WriteInt16(stream, Model.iKPoints.Count);
                StreamUtil.WriteInt16(stream, Model.boneWeightHeaders.Count);
                StreamUtil.WriteInt16(stream, Model.tristripHeaders.Count);
                StreamUtil.WriteInt16(stream, Model.Unknown3);
                StreamUtil.WriteInt16(stream, Model.vertexDatas.Count);
                StreamUtil.WriteInt16(stream, Model.FileID);
                StreamUtil.WriteInt16(stream, Model.lastDatas.Count);
                StreamUtil.WriteInt16(stream, Model.unknownDatas.Count);

                modelHeaders[i] = Model;
            }


            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
            var file = File.Create(Path);
            stream.Position = 0;
            stream.CopyTo(file);
            stream.Dispose();
            file.Close();
        }


        public void WritePadding(Stream stream, int Rows)
        {
            int Num = 16 - ((int)stream.Position % 16);
            if (Num != 16)
            {
                for (int i = 0; i < Num; i++)
                {
                    StreamUtil.WriteUInt8(stream, 0xFF);
                }
            }

            for (int i = 0; i < Rows * 16; i++)
            {
                StreamUtil.WriteUInt8(stream, 0xFF);
            }

        }
        public struct ModelHeader
        {
            public string ModelName;
            public int ModelOffset;
            public int ModelSize;
            public int OffsetBoneData;
            public int OffsetBoneData2;

            public int OffsetMateralList;
            public int OffsetBoneData3;
            public int OffsetIKPointList;
            public int OffsetMorphList;

            public int OffsetWeight;
            public int OffsetTristripSection;
            public int Unknown0;
            public int OffsetVertexSection;

            public int OffsetLastData;
            public int OffsetUnknownData;

            public int Unknown1;
            public int Unknown2;
            public int NumBones;
            public int NumMorphs;
            public int NumMaterials;
            public int NumIKPoints;
            public int NumWeights;
            public int NumTristripGroups;
            public int Unknown3;
            public int NumVertices;

            public int FileID;
            public int NumLastData;
            public int NumUnknownData;

            public byte[] Matrix;

            public List<MaterialData> materialDatas;
            public List<BoneData> boneDatas;
            public List<Vector3> iKPoints;
            public List<MorphHeader> morphHeader;
            public List<BoneWeightHeader> boneWeightHeaders;
            public List<TristripHeader> tristripHeaders;
            public List<VertexData> vertexDatas;
            public List<VertexData> unknownVertexData;
            public List<UnknownData> unknownDatas;
            public List<LastData> lastDatas;
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
            public int Unknown1;
            public int Unknown2;

            public List<BoneWeight> boneWeights;
        }

        public struct BoneWeight
        {
            public int Weight;
            public int BoneID;
            public int FileID;
        }

        public struct TristripHeader
        {
            public int IndexListOffset;
            public int NumIndices;
            public int MaterialIndex0;
            public int MaterialIndex1;
            public int MaterialIndex2;
            public int MaterialIndex3;
            public int MaterialIndex4;

            public List<int> IndexList;

            public List<Face> faces;
        }

        public struct VertexData
        {
            public Vector3 VertexPosition;
            public float Unknown1;
            public Vector3 VertexNormal;
            public float Unknown2;
            public Vector3 VertexTangentNormal;
            public float Unknown3;
            public Vector2 VertexUV;
            public int Unknown4;
            public int WeightIndex;

            public List<Vector3> MorphData;
        }

        public struct UnknownData
        {
            public int U1; //Weight
            public int U2; //Index
            public int U3; //Index
            public int U4; //Index
        }

        public struct LastData
        {
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

            public Vector3 TangentNormal1;
            public Vector3 TangentNormal2;
            public Vector3 TangentNormal3;

            public int TangentNormal1Pos;
            public int TangentNormal2Pos;
            public int TangentNormal3Pos;

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
