﻿using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SSXMultiTool.FileHandlers.Models.SSX3
{
    public class SSX3PS2MPF
    {
        public byte[] magicWords = new byte[4];
        public int NumModels;
        public int HeaderSize;
        public int DataOffset;
        public List<MPFModelHeader> ModelList = new List<MPFModelHeader>();

        public static float MorphScale = 4.5f;
        public static bool AlphaModel = false;

        public void load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                magicWords = StreamUtil.ReadBytes(stream, 4);
                NumModels = StreamUtil.ReadInt16(stream);
                HeaderSize = StreamUtil.ReadInt16(stream);
                DataOffset = StreamUtil.ReadUInt32(stream);
                for (int i = 0; i < NumModels; i++)
                {
                    MPFModelHeader modelHeader = new MPFModelHeader();

                    modelHeader.ModelName = StreamUtil.ReadString(stream, 16);
                    modelHeader.DataOffset = StreamUtil.ReadUInt32(stream);
                    modelHeader.EntrySize = StreamUtil.ReadUInt32(stream);
                    modelHeader.BoneOffset = StreamUtil.ReadUInt32(stream);
                    modelHeader.UnusedIKPointOffset = StreamUtil.ReadUInt32(stream);
                    modelHeader.MaterialGroupOffset = StreamUtil.ReadUInt32(stream);
                    modelHeader.MeshDataOffset = StreamUtil.ReadUInt32(stream);
                    modelHeader.MaterialOffset = StreamUtil.ReadUInt32(stream);
                    modelHeader.MorphIDOffset = StreamUtil.ReadUInt32(stream);
                    modelHeader.WeightRefrenceOffset = StreamUtil.ReadUInt32(stream);
                    modelHeader.BoneWeightOffset = StreamUtil.ReadUInt32(stream);

                    stream.Position += 12;

                    modelHeader.WeightCount = StreamUtil.ReadInt16(stream);
                    modelHeader.WeightRefrenceCount = StreamUtil.ReadInt16(stream);
                    modelHeader.MaterialGroupCount = StreamUtil.ReadInt16(stream);
                    modelHeader.BoneCount = StreamUtil.ReadInt16(stream);
                    modelHeader.MaterialCount = StreamUtil.ReadInt16(stream);
                    modelHeader.UnusedIKPointCount = StreamUtil.ReadInt16(stream);
                    modelHeader.MorphKeyCount = StreamUtil.ReadInt16(stream);
                    modelHeader.FileID = StreamUtil.ReadInt16(stream);
                    modelHeader.TriangleCount = StreamUtil.ReadInt16(stream);

                    stream.Position += 10;

                    ModelList.Add(modelHeader);
                }

                //Read Matrix And Decompress
                int StartPos = DataOffset;
                for (int i = 0; i < ModelList.Count; i++)
                {
                    stream.Position = StartPos + ModelList[i].DataOffset;

                    MPFModelHeader modelHandler = ModelList[i];

                    int EntrySize = 0;

                    if (i == ModelList.Count - 1)
                    {
                        EntrySize = (int)((stream.Length - DataOffset) - ModelList[i].DataOffset);
                    }
                    else
                    {
                        EntrySize = ModelList[i + 1].DataOffset - ModelList[i].DataOffset;
                    }

                    modelHandler.Matrix = StreamUtil.ReadBytes(stream, EntrySize);
                    modelHandler.Matrix = RefpackHandler.Decompress(modelHandler.Matrix);
                    ModelList[i] = modelHandler;
                }

                for (int i = 0; i < ModelList.Count; i++)
                {
                    var TempModel = ModelList[i];
                    Stream streamMatrix = new MemoryStream();
                    streamMatrix.Write(TempModel.Matrix, 0, TempModel.Matrix.Length);
                    streamMatrix.Position = 0;

                    //Material
                    streamMatrix.Position = TempModel.MaterialOffset;
                    TempModel.MaterialList = new List<MaterialData>();
                    for (int a = 0; a < TempModel.MaterialCount; a++)
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
                        TempModel.MaterialList.Add(TempMat);
                    }

                    //Bone
                    streamMatrix.Position = TempModel.BoneOffset;
                    TempModel.BoneList = new List<BoneData>();
                    for (int a = 0; a < TempModel.BoneCount; a++)
                    {
                        var TempBoneData = new BoneData();
                        TempBoneData.BoneName = StreamUtil.ReadString(streamMatrix, 16);
                        TempBoneData.ParentFileID = StreamUtil.ReadInt16(streamMatrix);
                        TempBoneData.ParentBone = StreamUtil.ReadInt16(streamMatrix);
                        TempBoneData.Unknown1 = StreamUtil.ReadInt16(streamMatrix);
                        TempBoneData.BoneID = StreamUtil.ReadInt16(streamMatrix);

                        TempBoneData.Unknown2 = StreamUtil.ReadInt8(streamMatrix);
                        TempBoneData.Unknown3 = StreamUtil.ReadInt8(streamMatrix);
                        TempBoneData.Unknown4 = StreamUtil.ReadInt8(streamMatrix);
                        TempBoneData.Unknown5 = StreamUtil.ReadInt8(streamMatrix);

                        TempBoneData.Unknown6 = StreamUtil.ReadUInt32(streamMatrix);

                        TempBoneData.Position = StreamUtil.ReadVector4(streamMatrix);

                        if (!AlphaModel)
                        {
                            TempBoneData.Rotation = StreamUtil.ReadQuaternion(streamMatrix);
                        }
                        else
                        {
                            TempBoneData.Rotation.W = StreamUtil.ReadFloat(streamMatrix);
                            TempBoneData.Rotation.X = StreamUtil.ReadFloat(streamMatrix);
                            TempBoneData.Rotation.Y = StreamUtil.ReadFloat(streamMatrix);
                            TempBoneData.Rotation.Z = StreamUtil.ReadFloat(streamMatrix);
                        }
                        TempBoneData.Unknown = StreamUtil.ReadVector4(streamMatrix);

                        TempBoneData.FileID = TempModel.FileID;
                        TempBoneData.BonePos = a;

                        TempModel.BoneList.Add(TempBoneData);
                    }

                    //Morph Data
                    streamMatrix.Position = TempModel.MorphIDOffset;
                    TempModel.MorphKeyIDList = new List<int>();
                    for (int a = 0; a < TempModel.MorphKeyCount; a++)
                    {
                        TempModel.MorphKeyIDList.Add(StreamUtil.ReadUInt32(streamMatrix));
                    }

                    //Bone Weight Info 
                    streamMatrix.Position = TempModel.BoneWeightOffset;
                    TempModel.BoneWeightHeaderList = new List<BoneWeightHeader>();
                    for (int b = 0; b < TempModel.WeightCount; b++)
                    {
                        var BoneWeight = new BoneWeightHeader();

                        BoneWeight.WeightCount = StreamUtil.ReadUInt32(streamMatrix);
                        BoneWeight.WeightOffset = StreamUtil.ReadUInt32(streamMatrix);
                        BoneWeight.Unknown = StreamUtil.ReadUInt32(streamMatrix);
                        BoneWeight.BoneWeightList = new List<BoneWeight>();
                        int TempPos = (int)streamMatrix.Position;
                        streamMatrix.Position = BoneWeight.WeightOffset;
                        for (int a = 0; a < BoneWeight.WeightCount; a++)
                        {
                            var boneWeight = new BoneWeight();
                            boneWeight.Weight = StreamUtil.ReadInt16(streamMatrix);
                            boneWeight.BoneID = StreamUtil.ReadUInt8(streamMatrix);
                            boneWeight.FileID = StreamUtil.ReadUInt8(streamMatrix);
                            BoneWeight.BoneWeightList.Add(boneWeight);
                        }
                        streamMatrix.Position = TempPos;
                        TempModel.BoneWeightHeaderList.Add(BoneWeight);
                    }

                    //Weight Refrence List
                    streamMatrix.Position = TempModel.WeightRefrenceOffset;
                    TempModel.WeightRefrenceLists = new List<WeightRefList>();
                    for (int b = 0; b < TempModel.WeightRefrenceCount; b++)
                    {
                        var NumberListRef = new WeightRefList();
                        NumberListRef.ListCount = StreamUtil.ReadUInt32(streamMatrix);
                        NumberListRef.Offset = StreamUtil.ReadUInt32(streamMatrix);
                        NumberListRef.WeightIDs = new List<int>();

                        int TempPos = (int)streamMatrix.Position;
                        streamMatrix.Position = NumberListRef.Offset;
                        for (int c = 0; c < NumberListRef.ListCount; c++)
                        {
                            NumberListRef.WeightIDs.Add(StreamUtil.ReadUInt32(streamMatrix));
                        }
                        streamMatrix.Position = TempPos;
                        TempModel.WeightRefrenceLists.Add(NumberListRef);
                    }

                    //Mesh Group Data
                    streamMatrix.Position = TempModel.MaterialGroupOffset;
                    TempModel.MaterialGroupList = new List<MaterialGroup>();
                    int NumberWeightRef = 0;
                    for (int a = 0; a < TempModel.MaterialGroupCount; a++)
                    {
                        var TempChunkData = new MaterialGroup();
                        TempChunkData.Type = StreamUtil.ReadUInt32(streamMatrix);
                        TempChunkData.Material = StreamUtil.ReadUInt32(streamMatrix);
                        TempChunkData.Unknown = StreamUtil.ReadUInt32(streamMatrix);
                        TempChunkData.WeightRefGroupCount = StreamUtil.ReadUInt32(streamMatrix);
                        TempChunkData.WeightRefGroupOffset = StreamUtil.ReadUInt32(streamMatrix);

                        int TempPos = (int)streamMatrix.Position;
                        streamMatrix.Position = TempChunkData.WeightRefGroupOffset;
                        TempChunkData.WeightRefList = new List<WeightRefGroup>();
                        for (int b = 0; b < TempChunkData.WeightRefGroupCount; b++)
                        {
                            var TempSubHeader = new WeightRefGroup();
                            TempSubHeader.MorphMeshOffset = StreamUtil.ReadUInt32(streamMatrix);
                            TempSubHeader.MorphMeshCount = StreamUtil.ReadUInt32(streamMatrix);
                            int TempPos1 = (int)streamMatrix.Position;
                            TempSubHeader.MorphMeshGroupList = new List<MorphMeshGroup>();
                            streamMatrix.Position = TempSubHeader.MorphMeshOffset;
                            for (int c = 0; c < TempSubHeader.MorphMeshCount; c++)
                            {
                                var TempMeshGroupHeader = new MorphMeshGroup();
                                TempMeshGroupHeader.MeshOffset = StreamUtil.ReadUInt32(streamMatrix);
                                TempMeshGroupHeader.MorphOffset = StreamUtil.ReadUInt32(streamMatrix);
                                TempMeshGroupHeader.WeightRefID = NumberWeightRef;
                                TempSubHeader.MorphMeshGroupList.Add(TempMeshGroupHeader);
                            }
                            streamMatrix.Position = TempPos1;
                            NumberWeightRef++;
                            TempChunkData.WeightRefList.Add(TempSubHeader);
                        }

                        streamMatrix.Position = TempPos;

                        TempModel.MaterialGroupList.Add(TempChunkData);
                    }


                    //Load Mesh Data
                    for (int a = 0; a < TempModel.MaterialGroupList.Count; a++)
                    {
                        var TempMaterialGroup = TempModel.MaterialGroupList[a];

                        for (int b = 0; b < TempMaterialGroup.WeightRefList.Count; b++)
                        {
                            var TempWeightRefGroup = TempMaterialGroup.WeightRefList[b];

                            for (int c = 0; c < TempWeightRefGroup.MorphMeshGroupList.Count; c++)
                            {
                                var TempMeshMorphChunk = TempWeightRefGroup.MorphMeshGroupList[c];

                                TempMeshMorphChunk.MeshChunkList = new List<MeshChunk>();
                                //Load Mesh Chunk
                                if(TempMeshMorphChunk.MeshOffset!=-1)
                                {
                                    streamMatrix.Position = TempMeshMorphChunk.MeshOffset;

                                    while (true)
                                    {
                                        streamMatrix.Position += 31;
                                        byte Temp = StreamUtil.ReadUInt8(streamMatrix);
                                        if (Temp != 0x6C)
                                        {
                                            break;
                                        }
                                        streamMatrix.Position += 16;
                                        var ModelData = new MeshChunk();

                                        ModelData.StripCount = StreamUtil.ReadUInt32(streamMatrix);
                                        ModelData.Unknown1 = StreamUtil.ReadUInt32(streamMatrix);
                                        ModelData.Unknown2 = StreamUtil.ReadUInt32(streamMatrix);
                                        ModelData.VertexCount = StreamUtil.ReadUInt32(streamMatrix);

                                        //Load Strip Count
                                        List<int> TempStrips = new List<int>();
                                        for (int d = 0; d < ModelData.StripCount; d++)
                                        {
                                            TempStrips.Add(StreamUtil.ReadUInt32(streamMatrix));
                                            streamMatrix.Position += 12;
                                        }
                                        streamMatrix.Position += 16;
                                        ModelData.Strips = TempStrips;

                                        List<Vector4> UVs = new List<Vector4>();
                                        //Read UV Texture Points
                                        if (ModelData.Unknown2 != 0)
                                        {
                                            streamMatrix.Position += 48;
                                            for (int d = 0; d < ModelData.VertexCount; d++)
                                            {
                                                Vector4 uv = new Vector4();
                                                uv.X = StreamUtil.ReadInt16(streamMatrix) / 4096f;
                                                uv.Y = StreamUtil.ReadInt16(streamMatrix) / 4096f;
                                                uv.Z = StreamUtil.ReadInt16(streamMatrix); //Weight Assigment
                                                uv.W = StreamUtil.ReadInt16(streamMatrix); //Weight Assigment
                                                UVs.Add(uv);
                                            }
                                            StreamUtil.AlignBy16(streamMatrix);
                                        }
                                        ModelData.UV = UVs;

                                        List<Vector3> Normals = new List<Vector3>();
                                        //Read Normals
                                        if (ModelData.Unknown2 != 0)
                                        {
                                            streamMatrix.Position += 48;
                                            for (int d = 0; d < ModelData.VertexCount; d++)
                                            {
                                                Vector3 normal = new Vector3();
                                                normal.X = StreamUtil.ReadInt16(streamMatrix) / 32768f;
                                                normal.Y = StreamUtil.ReadInt16(streamMatrix) / 32768f;
                                                normal.Z = StreamUtil.ReadInt16(streamMatrix) / 32768f;
                                                Normals.Add(normal);
                                            }
                                            StreamUtil.AlignBy16(streamMatrix);
                                        }
                                        ModelData.UVNormals = Normals;

                                        List<Vector3> vertices = new List<Vector3>();
                                        ModelData.Weights = new List<int>();
                                        //Load Vertex
                                        if (ModelData.VertexCount != 0)
                                        {
                                            streamMatrix.Position += 48;
                                            if (TempMaterialGroup.Type != 17)
                                            {
                                                for (int d = 0; d < ModelData.VertexCount; d++)
                                                {
                                                    Vector3 vertex = new Vector3();
                                                    vertex.X = StreamUtil.ReadFloat(streamMatrix);
                                                    vertex.Y = StreamUtil.ReadFloat(streamMatrix);
                                                    vertex.Z = StreamUtil.ReadFloat(streamMatrix);
                                                    vertices.Add(vertex);
                                                }
                                            }
                                            else
                                            {
                                                for (int d = 0; d < ModelData.VertexCount; d++)
                                                {
                                                    Vector3 vertex = new Vector3();
                                                    vertex.X = StreamUtil.ReadFloat(streamMatrix);
                                                    vertex.Y = StreamUtil.ReadFloat(streamMatrix);
                                                    vertex.Z = StreamUtil.ReadFloat(streamMatrix);
                                                    ModelData.Weights.Add(StreamUtil.ReadUInt32(streamMatrix));
                                                    vertices.Add(vertex);
                                                }
                                            }
                                            StreamUtil.AlignBy16(streamMatrix);

                                        }
                                        ModelData.Vertices = vertices;

                                        streamMatrix.Position += 16 * 2;
                                        TempMeshMorphChunk.MeshChunkList.Add(ModelData);
                                    }

                                }

                                //Load Morph Chunk
                                if (TempMeshMorphChunk.MorphOffset != -1)
                                {
                                    streamMatrix.Position = TempMeshMorphChunk.MeshOffset + TempMeshMorphChunk.MorphOffset;

                                    TempMeshMorphChunk.MorphDataList = new List<MorphKey>();
                                    for (int dci = 0; dci < TempModel.MorphKeyCount; dci++)
                                    {
                                        var TempMorphKey = new MorphKey();
                                        TempMorphKey.MorphDataList = new List<MorphData>();
                                        streamMatrix.Position += 30;
                                        TempMorphKey.MorphPointCount = StreamUtil.ReadUInt8(streamMatrix);
                                        streamMatrix.Position += 1;
                                        if (TempMorphKey.MorphPointCount > 0)
                                        {
                                            TempMorphKey.ListAmmount = StreamUtil.ReadUInt32(streamMatrix);
                                            for (int dcb = 0; dcb < TempMorphKey.ListAmmount; dcb++)
                                            {
                                                var TempMorphData = new MorphData();
                                                TempMorphData.vector3.X = (float)StreamUtil.ReadInt8(streamMatrix) / MorphScale;
                                                TempMorphData.vector3.Y = (float)StreamUtil.ReadInt8(streamMatrix) / MorphScale;
                                                TempMorphData.vector3.Z = (float)StreamUtil.ReadInt8(streamMatrix) / MorphScale;
                                                TempMorphData.ID = StreamUtil.ReadUInt8(streamMatrix) / 3;
                                                TempMorphKey.MorphDataList.Add(TempMorphData);
                                            }
                                            StreamUtil.AlignBy16(streamMatrix);
                                        }
                                        else
                                        {
                                            streamMatrix.Position -= 16;
                                        }
                                        streamMatrix.Position += 16;

                                        TempMeshMorphChunk.MorphDataList.Add(TempMorphKey);
                                    }
                                }

                                //Generate Faces
                                for (int d = 0; d < TempMeshMorphChunk.MeshChunkList.Count; d++)
                                {
                                    var TempChunk = TempMeshMorphChunk.MeshChunkList[d];

                                    TempChunk = GenerateFaces(TempChunk, TempMeshMorphChunk.MorphDataList);

                                    TempMeshMorphChunk.MeshChunkList[d] = TempChunk;
                                }

                                TempWeightRefGroup.MorphMeshGroupList[c] = TempMeshMorphChunk;
                            }

                            TempMaterialGroup.WeightRefList[b] = TempWeightRefGroup;
                        }

                        TempModel.MaterialGroupList[a] = TempMaterialGroup;
                    }


                    ModelList[i] = TempModel;
                }
            }
        }

        public MeshChunk GenerateFaces(MeshChunk models, List<MorphKey> morphPointData)
        {
            var ModelData = models;
            //Increment Strips
            List<int> strip2 = new List<int>();
            strip2.Add(0);
            foreach (var item in ModelData.Strips)
            {
                strip2.Add(strip2[strip2.Count - 1] + item);
            }

            //Make Faces
            ModelData.Faces = new List<Face>();
            int localIndex = 0;
            bool Rotation = false;
            for (int b = 0; b < ModelData.Vertices.Count; b++)
            {
                if (InsideSplits(b, strip2))
                {
                    Rotation = false;
                    localIndex = 1;
                    continue;
                }
                if (localIndex < 2)
                {
                    localIndex++;
                    continue;
                }

                ModelData.Faces.Add(CreateFaces(b, ModelData, Rotation, morphPointData));
                Rotation = !Rotation;
                localIndex++;
            }

            return ModelData;
        }
        public bool InsideSplits(int Number, List<int> splits)
        {
            foreach (var item in splits)
            {
                if (item == Number)
                {
                    return true;
                }
            }
            return false;
        }
        public Face CreateFaces(int Index, MeshChunk ModelData, bool roatation, List<MorphKey> morphPointData)
        {
            Face face = new Face();
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
            face.V1 = ModelData.Vertices[Index1];
            face.V2 = ModelData.Vertices[Index2];
            face.V3 = ModelData.Vertices[Index3];

            face.V1Pos = Index1;
            face.V2Pos = Index2;
            face.V3Pos = Index3;

            if (ModelData.UV.Count != 0)
            {
                face.UV1 = ModelData.UV[Index1];
                face.UV2 = ModelData.UV[Index2];
                face.UV3 = ModelData.UV[Index3];

                face.UV1Pos = Index1;
                face.UV2Pos = Index2;
                face.UV3Pos = Index3;

                face.Normal1 = ModelData.UVNormals[Index1];
                face.Normal2 = ModelData.UVNormals[Index2];
                face.Normal3 = ModelData.UVNormals[Index3];

                face.Normal1Pos = Index1;
                face.Normal2Pos = Index2;
                face.Normal3Pos = Index3;

                face.Weight1Pos = (int)((face.UV1.Z - 17) / 4);
                face.Weight2Pos = (int)((face.UV2.Z - 17) / 4);
                face.Weight3Pos = (int)((face.UV3.Z - 17) / 4);
            }
            else
            {
                face.Weight1Pos = (ModelData.Weights[Index1] - 17) / 4;
                face.Weight2Pos = (ModelData.Weights[Index2] - 17) / 4;
                face.Weight3Pos = (ModelData.Weights[Index3] - 17) / 4;
            }

            if (morphPointData != null)
            {
                face.MorphPoint1 = new List<Vector3>();
                face.MorphPoint2 = new List<Vector3>();
                face.MorphPoint3 = new List<Vector3>();

                for (int i = 0; i < morphPointData.Count; i++)
                {
                    bool Point1Test = false;
                    bool Point2Test = false;
                    bool Point3Test = false;
                    for (int a = 0; a < morphPointData[i].MorphDataList.Count; a++)
                    {
                        if (Index1 == morphPointData[i].MorphDataList[a].ID)
                        {
                            face.MorphPoint1.Add(morphPointData[i].MorphDataList[a].vector3);
                            Point1Test = true;
                        }

                        if (Index2 == morphPointData[i].MorphDataList[a].ID)
                        {
                            face.MorphPoint2.Add(morphPointData[i].MorphDataList[a].vector3);
                            Point2Test = true;
                        }

                        if (Index3 == morphPointData[i].MorphDataList[a].ID)
                        {
                            face.MorphPoint3.Add(morphPointData[i].MorphDataList[a].vector3);
                            Point3Test = true;
                        }
                    }

                    if(!Point1Test)
                    {
                        face.MorphPoint1.Add(new Vector3());
                    }

                    if (!Point2Test)
                    {
                        face.MorphPoint2.Add(new Vector3());
                    }

                    if (!Point3Test)
                    {
                        face.MorphPoint3.Add(new Vector3());
                    }
                }
            }

            return face;
        }

        public void Save(string path, bool Compression)
        {
            Stream stream = new MemoryStream();
            StreamUtil.WriteBytes(stream, magicWords);
            StreamUtil.WriteInt16(stream, ModelList.Count);
            StreamUtil.WriteInt16(stream, 12);
            DataOffset = 96 * ModelList.Count + 4 + 12;
            StreamUtil.WriteInt32(stream, DataOffset);

            stream.Position = 96 * ModelList.Count + 4 + 12;

            //Generate Matrix
            for (int i = 0; i < ModelList.Count; i++)
            {
                var Model = ModelList[i];
                MemoryStream ModelStream = new MemoryStream();

                Model.MaterialOffset = (int)ModelStream.Position;
                for (int a = 0; a < Model.MaterialList.Count; a++)
                {
                    StreamUtil.WriteString(ModelStream, Model.MaterialList[a].MainTexture, 4);

                    if (Model.MaterialList[a].Texture1 != "")
                    {
                        StreamUtil.WriteString(ModelStream, Model.MaterialList[a].Texture1, 4);
                    }
                    else
                    {
                        StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x20, 0x20, 0x20 });
                    }

                    if (Model.MaterialList[a].Texture2 != "")
                    {
                        StreamUtil.WriteString(ModelStream, Model.MaterialList[a].Texture2, 4);
                    }
                    else
                    {
                        StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x20, 0x20, 0x20 });
                    }

                    if (Model.MaterialList[a].Texture3 != "")
                    {
                        StreamUtil.WriteString(ModelStream, Model.MaterialList[a].Texture3, 4);
                    }
                    else
                    {
                        StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x20, 0x20, 0x20 });
                    }

                    if (Model.MaterialList[a].Texture4 != "")
                    {
                        StreamUtil.WriteString(ModelStream, Model.MaterialList[a].Texture4, 4);
                    }
                    else
                    {
                        StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x20, 0x20, 0x20 });
                    }


                    StreamUtil.WriteFloat32(ModelStream, Model.MaterialList[a].FactorFloat);
                    StreamUtil.WriteFloat32(ModelStream, Model.MaterialList[a].Unused1Float);
                    StreamUtil.WriteFloat32(ModelStream, Model.MaterialList[a].Unused2Float);
                }

                Model.BoneOffset = (int)ModelStream.Position;
                for (int a = 0; a < Model.BoneList.Count; a++)
                {
                    StreamUtil.WriteString(ModelStream, Model.BoneList[a].BoneName, 16);
                    StreamUtil.WriteInt16(ModelStream, Model.BoneList[a].ParentFileID);
                    StreamUtil.WriteInt16(ModelStream, Model.BoneList[a].ParentBone);
                    StreamUtil.WriteInt16(ModelStream, Model.BoneList[a].Unknown1);
                    StreamUtil.WriteInt16(ModelStream, Model.BoneList[a].BoneID);

                    StreamUtil.WriteUInt8(ModelStream, Model.BoneList[a].Unknown2);
                    StreamUtil.WriteUInt8(ModelStream, Model.BoneList[a].Unknown3);
                    StreamUtil.WriteUInt8(ModelStream, Model.BoneList[a].Unknown4);
                    StreamUtil.WriteUInt8(ModelStream, Model.BoneList[a].Unknown5);

                    StreamUtil.WriteInt32(ModelStream, Model.BoneList[a].Unknown6);

                    StreamUtil.WriteVector4(ModelStream, Model.BoneList[a].Position);
                    if (!AlphaModel)
                    {
                        StreamUtil.WriteQuaternion(ModelStream, Model.BoneList[a].Rotation);
                    }
                    else
                    {
                        StreamUtil.WriteFloat32(ModelStream, Model.BoneList[a].Rotation.W);
                        StreamUtil.WriteFloat32(ModelStream, Model.BoneList[a].Rotation.X);
                        StreamUtil.WriteFloat32(ModelStream, Model.BoneList[a].Rotation.Y);
                        StreamUtil.WriteFloat32(ModelStream, Model.BoneList[a].Rotation.Z);
                    }
                    StreamUtil.WriteVector4(ModelStream, Model.BoneList[a].Unknown);
                }

                Model.MorphIDOffset = (int)ModelStream.Position;
                for (int a = 0; a < Model.MorphKeyIDList.Count; a++)
                {
                    StreamUtil.WriteInt32(ModelStream, Model.MorphKeyIDList[a]);
                }

                //Bone Weigth
                Model.BoneWeightOffset = (int)ModelStream.Position;
                ModelStream.Position += 12 * Model.BoneWeightHeaderList.Count;
                for (int a = 0; a < Model.BoneWeightHeaderList.Count; a++)
                {
                    var BoneWeightHeader = Model.BoneWeightHeaderList[a];
                    BoneWeightHeader.WeightOffset = (int)ModelStream.Position;
                    for (int b = 0; b < BoneWeightHeader.BoneWeightList.Count; b++)
                    {
                        StreamUtil.WriteInt16(ModelStream, BoneWeightHeader.BoneWeightList[b].Weight);
                        StreamUtil.WriteUInt8(ModelStream, BoneWeightHeader.BoneWeightList[b].BoneID);
                        StreamUtil.WriteUInt8(ModelStream, BoneWeightHeader.BoneWeightList[b].FileID);
                    }
                    Model.BoneWeightHeaderList[a] = BoneWeightHeader;
                }

                Model.WeightRefrenceOffset = (int)ModelStream.Position;
                ModelStream.Position = Model.BoneWeightOffset;
                for (int a = 0; a < Model.BoneWeightHeaderList.Count; a++)
                {
                    StreamUtil.WriteInt32(ModelStream, Model.BoneWeightHeaderList[a].BoneWeightList.Count);
                    StreamUtil.WriteInt32(ModelStream, Model.BoneWeightHeaderList[a].WeightOffset);
                    StreamUtil.WriteInt32(ModelStream, Model.BoneWeightHeaderList[a].Unknown);
                }

                ModelStream.Position = Model.WeightRefrenceOffset;

                //Number Ref List
                ModelStream.Position += Model.WeightRefrenceLists.Count * 8;
                for (int a = 0; a < Model.WeightRefrenceLists.Count; a++)
                {
                    var TempNumberRef = Model.WeightRefrenceLists[a];
                    TempNumberRef.Offset = (int)ModelStream.Position;
                    Model.WeightRefrenceLists[a] = TempNumberRef;

                    for (int c = 0; c < TempNumberRef.WeightIDs.Count; c++)
                    {
                        StreamUtil.WriteInt32(ModelStream, TempNumberRef.WeightIDs[c]);
                    }
                }

                Model.MaterialGroupOffset = (int)ModelStream.Position;
                ModelStream.Position = Model.WeightRefrenceOffset;

                for (int a = 0; a < Model.WeightRefrenceLists.Count; a++)
                {
                    var TempNumberRef = Model.WeightRefrenceLists[a];
                    StreamUtil.WriteInt32(ModelStream, TempNumberRef.WeightIDs.Count);
                    StreamUtil.WriteInt32(ModelStream, TempNumberRef.Offset);
                }

                //Mesh Group
                ModelStream.Position = Model.MaterialGroupOffset;
                int MathOffset = 0;
                for (int a = 0; a < Model.MaterialGroupList.Count; a++)
                {
                    MathOffset += 4 * 5;
                    for (int b = 0; b < Model.MaterialGroupList[a].WeightRefList.Count; b++)
                    {
                        MathOffset += 8;
                        MathOffset += Model.MaterialGroupList[a].WeightRefList[b].MorphMeshGroupList.Count * 4 * 2;
                    }
                }
                ModelStream.Position += MathOffset;
                StreamUtil.AlignBy16(ModelStream);


                Model.MeshDataOffset = (int)ModelStream.Position;
                for (int a = 0; a < Model.MaterialGroupList.Count; a++)
                {
                    var TempMaterialGroup = Model.MaterialGroupList[a];
                    for (int b = 0; b < TempMaterialGroup.WeightRefList.Count; b++)
                    {
                        var TempWeightRefGroup = TempMaterialGroup.WeightRefList[b];
                        for (int c = 0; c < TempWeightRefGroup.MorphMeshGroupList.Count; c++)
                        {
                            var TempGroupHeader = TempWeightRefGroup.MorphMeshGroupList[c];
                            TempGroupHeader.MeshOffset = (int)ModelStream.Position;
                            bool MeshTest = false;
                            //Write Mesh Chunk
                            for (int d = 0; d < TempGroupHeader.MeshChunkList.Count; d++)
                            {
                                var TempStaticMesh = TempGroupHeader.MeshChunkList[d];
                                int RowCountPos = (int)ModelStream.Position;
                                ModelStream.Position += 3;
                                StreamUtil.WriteInt32(ModelStream, 16);
                                StreamUtil.AlignBy16(ModelStream);

                                StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 });
                                StreamUtil.WriteUInt8(ModelStream, TempStaticMesh.Strips.Count + 2);
                                StreamUtil.WriteUInt8(ModelStream, 0x6C);

                                //Tristrip Header InfoCrap
                                StreamUtil.WriteUInt8(ModelStream, TempStaticMesh.Vertices.Count);
                                if (TempMaterialGroup.Type == 1 || TempMaterialGroup.Type == 256)
                                {
                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x80, 0x00, 0x00, 0x00, 0x40, 0x2E, 0x30, 0x12, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                                }
                                else if (TempMaterialGroup.Type == 17)
                                {
                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x80, 0x00, 0x00, 0x00, 0x40, 0x22, 0x10, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                                }

                                StreamUtil.WriteInt32(ModelStream, TempStaticMesh.Strips.Count);
                                StreamUtil.WriteInt32(ModelStream, TempStaticMesh.Unknown1);
                                StreamUtil.WriteInt32(ModelStream, TempStaticMesh.Unknown2);
                                StreamUtil.WriteInt32(ModelStream, TempStaticMesh.Vertices.Count);

                                //Write Tristrips
                                for (int e = 0; e < TempStaticMesh.Strips.Count; e++)
                                {
                                    StreamUtil.WriteInt32(ModelStream, TempStaticMesh.Strips[e]);
                                    StreamUtil.AlignBy16(ModelStream);
                                }

                                //Go back and write rowcount
                                int TempPos = (int)ModelStream.Position;
                                ModelStream.Position = RowCountPos;
                                StreamUtil.WriteInt24(ModelStream, (TempPos - RowCountPos) / 16 - 1);
                                ModelStream.Position = TempPos;

                                //Set New Rowcount
                                RowCountPos = (int)ModelStream.Position;
                                ModelStream.Position += 3;
                                StreamUtil.WriteInt32(ModelStream, 16);
                                StreamUtil.AlignBy16(ModelStream);

                                //Write UV
                                ModelStream.Position += 7;
                                StreamUtil.WriteUInt8(ModelStream, 0x30);
                                ModelStream.Position += 8;

                                if (TempMaterialGroup.Type != 17)
                                {
                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x10, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x50, 0x50, 0x50, 0x50 });

                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x03, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 });
                                    StreamUtil.WriteUInt8(ModelStream, TempStaticMesh.Strips.Count + 2);
                                    StreamUtil.WriteUInt8(ModelStream, 0x80);
                                    StreamUtil.WriteUInt8(ModelStream, TempStaticMesh.UV.Count);
                                    StreamUtil.WriteUInt8(ModelStream, 0x6D);

                                    for (int e = 0; e < TempStaticMesh.UV.Count; e++)
                                    {
                                        StreamUtil.WriteInt16(ModelStream, (int)(TempStaticMesh.UV[e].X * 4096f));
                                        StreamUtil.WriteInt16(ModelStream, (int)(TempStaticMesh.UV[e].Y * 4096f));
                                        StreamUtil.WriteInt16(ModelStream, (int)(TempStaticMesh.UV[e].Z));
                                        StreamUtil.WriteInt16(ModelStream, (int)(TempStaticMesh.UV[e].W));
                                    }
                                    StreamUtil.AlignBy16(ModelStream);

                                    //Write Normals
                                    ModelStream.Position += 7;
                                    StreamUtil.WriteUInt8(ModelStream, 0x30);
                                    ModelStream.Position += 8;

                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x40, 0x40, 0x40, 0x40 });

                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x03, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 });
                                    StreamUtil.WriteUInt8(ModelStream, TempStaticMesh.Strips.Count + 3);
                                    StreamUtil.WriteUInt8(ModelStream, 0x80);
                                    StreamUtil.WriteUInt8(ModelStream, TempStaticMesh.UVNormals.Count);
                                    StreamUtil.WriteUInt8(ModelStream, 0x79);

                                    for (int e = 0; e < TempStaticMesh.UVNormals.Count; e++)
                                    {
                                        StreamUtil.WriteInt16(ModelStream, (int)(TempStaticMesh.UVNormals[e].X * 32768f));
                                        StreamUtil.WriteInt16(ModelStream, (int)(TempStaticMesh.UVNormals[e].Y * 32768f));
                                        StreamUtil.WriteInt16(ModelStream, (int)(TempStaticMesh.UVNormals[e].Z * 32768f));
                                    }
                                    StreamUtil.AlignBy16(ModelStream);

                                    //Write Vertex Count
                                    ModelStream.Position += 7;
                                    StreamUtil.WriteUInt8(ModelStream, 0x30);
                                    ModelStream.Position += 8;

                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x20, 0x40, 0x40, 0x40, 0x40 });

                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x03, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 });
                                    StreamUtil.WriteUInt8(ModelStream, TempStaticMesh.Strips.Count + 4);
                                    StreamUtil.WriteUInt8(ModelStream, 0x80);
                                    StreamUtil.WriteUInt8(ModelStream, TempStaticMesh.Vertices.Count);
                                    StreamUtil.WriteUInt8(ModelStream, 0x78);

                                    for (int e = 0; e < TempStaticMesh.Vertices.Count; e++)
                                    {
                                        StreamUtil.WriteFloat32(ModelStream, (TempStaticMesh.Vertices[e].X));
                                        StreamUtil.WriteFloat32(ModelStream, (TempStaticMesh.Vertices[e].Y));
                                        StreamUtil.WriteFloat32(ModelStream, (TempStaticMesh.Vertices[e].Z));
                                    }
                                    StreamUtil.AlignBy16(ModelStream);

                                    //Go back and write row count
                                    TempPos = (int)ModelStream.Position;
                                    ModelStream.Position = RowCountPos;
                                    StreamUtil.WriteInt24(ModelStream, (TempPos - RowCountPos) / 16 - 1);
                                    ModelStream.Position = TempPos;

                                    //Write New RowCount that neve changes
                                    StreamUtil.WriteInt24(ModelStream, 1);
                                    if (TempMaterialGroup.Type != 256)
                                    {
                                        StreamUtil.WriteInt32(ModelStream, 16);
                                    }
                                    else
                                    {
                                        StreamUtil.WriteInt32(ModelStream, 0x60);
                                    }
                                    StreamUtil.AlignBy16(ModelStream);

                                    if (TempMaterialGroup.Type != 256)
                                    {
                                        StreamUtil.WriteBytes(ModelStream, new byte[] { 0x01, 0x01, 0x00, 0x01 });

                                        StreamUtil.WriteUInt8(ModelStream, 0x00); // Can sometimes be 0x0A

                                        StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x14 });
                                        StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });

                                    }
                                    else
                                    {
                                        StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                                    }

                                }
                                else
                                {
                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x20, 0x40, 0x40, 0x40, 0x40 });

                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 });
                                    StreamUtil.WriteUInt8(ModelStream, TempStaticMesh.Strips.Count + 2);
                                    StreamUtil.WriteUInt8(ModelStream, 0x80);
                                    StreamUtil.WriteUInt8(ModelStream, TempStaticMesh.Vertices.Count);
                                    StreamUtil.WriteUInt8(ModelStream, 0x6C);

                                    for (int e = 0; e < TempStaticMesh.Vertices.Count; e++)
                                    {
                                        StreamUtil.WriteFloat32(ModelStream, (TempStaticMesh.Vertices[e].X));
                                        StreamUtil.WriteFloat32(ModelStream, (TempStaticMesh.Vertices[e].Y));
                                        StreamUtil.WriteFloat32(ModelStream, (TempStaticMesh.Vertices[e].Z));
                                        StreamUtil.WriteInt32(ModelStream, (TempStaticMesh.Weights[e]));
                                    }
                                    StreamUtil.AlignBy16(ModelStream);

                                    //Go back and write row count
                                    TempPos = (int)ModelStream.Position;
                                    ModelStream.Position = RowCountPos;
                                    StreamUtil.WriteInt24(ModelStream, (TempPos - RowCountPos) / 16 - 1);
                                    ModelStream.Position = TempPos;

                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x01, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x14 });
                                }
                            }

                            //Do Morph Stuff

                            if (TempMaterialGroup.Type == 256)
                            {
                                TempGroupHeader.MorphOffset = (int)ModelStream.Position - TempGroupHeader.MeshOffset;
                                for (int e = 0; e < Model.MorphKeyCount; e++)
                                {
                                    var TempMorphList = TempGroupHeader.MorphDataList[e];

                                    int RowCountPos = (int)ModelStream.Position;
                                    ModelStream.Position += 3;
                                    StreamUtil.WriteInt32(ModelStream, 96);
                                    StreamUtil.AlignBy16(ModelStream);

                                    if (TempMorphList.MorphDataList.Count != 0)
                                    {
                                        StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0xB5, 0x80 });
                                        StreamUtil.WriteUInt8(ModelStream, TempMorphList.MorphDataList.Count+1);
                                        StreamUtil.WriteUInt8(ModelStream, 0x6E);
                                        StreamUtil.WriteInt32(ModelStream, TempMorphList.MorphDataList.Count);
                                        for (int d = 0; d < TempMorphList.MorphDataList.Count; d++)
                                        {
                                            StreamUtil.WriteUInt8(ModelStream, (int)(TempMorphList.MorphDataList[d].vector3.X * MorphScale));
                                            StreamUtil.WriteUInt8(ModelStream, (int)(TempMorphList.MorphDataList[d].vector3.Y * MorphScale));
                                            StreamUtil.WriteUInt8(ModelStream, (int)(TempMorphList.MorphDataList[d].vector3.Z * MorphScale));
                                            StreamUtil.WriteUInt8(ModelStream, TempMorphList.MorphDataList[d].ID*3);
                                        }
                                        StreamUtil.AlignBy16(ModelStream);
                                        StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x14 });

                                        int TempPos = (int)ModelStream.Position;
                                        ModelStream.Position = RowCountPos;
                                        StreamUtil.WriteInt24(ModelStream, (TempPos - RowCountPos) / 16 - 1);
                                        ModelStream.Position = TempPos;
                                    }
                                    else
                                    {
                                        StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x14 });

                                        int TempPos = (int)ModelStream.Position;
                                        ModelStream.Position = RowCountPos;
                                        StreamUtil.WriteInt24(ModelStream, 1);
                                        ModelStream.Position = TempPos;
                                    }
                                    TempGroupHeader.MorphDataList[e] = TempMorphList;
                                }
                                if ((a == Model.MaterialGroupList.Count - 1 && b == TempMaterialGroup.WeightRefList.Count - 1 && c == TempWeightRefGroup.MorphMeshGroupList.Count - 1))
                                {
                                    //Write End of Meshdata
                                    StreamUtil.WriteInt24(ModelStream, 1);
                                    StreamUtil.WriteInt32(ModelStream, 96);
                                    StreamUtil.AlignBy16(ModelStream);

                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x01 });
                                    StreamUtil.AlignBy16(ModelStream);
                                    ModelStream.Position -= 1;
                                    StreamUtil.WriteUInt8(ModelStream, 0);

                                    StreamUtil.WriteInt24(ModelStream, 1);
                                    StreamUtil.WriteInt32(ModelStream, 96);
                                    StreamUtil.AlignBy16(ModelStream);

                                    ModelStream.Position += 4;
                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x01, 0x01, 0x00, 0x01 });

                                    ModelStream.Position += 7;
                                    StreamUtil.WriteUInt8(ModelStream, 17);
                                }
                            }
                            else
                            {
                                TempGroupHeader.MorphOffset = -1;
                            }

                            if (TempMaterialGroup.Type != 256)
                            {
                                //Write End of Meshdata
                                StreamUtil.WriteInt24(ModelStream, 1);
                                StreamUtil.WriteInt32(ModelStream, 96);
                                StreamUtil.AlignBy16(ModelStream);

                                StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x01 });
                                StreamUtil.AlignBy16(ModelStream);
                                ModelStream.Position -= 1;
                                StreamUtil.WriteUInt8(ModelStream, 0);

                                if ((a == Model.MaterialGroupList.Count - 1 && b == TempMaterialGroup.WeightRefList.Count - 1 && c == TempWeightRefGroup.MorphMeshGroupList.Count - 1))
                                {
                                    StreamUtil.WriteInt24(ModelStream, 1);
                                    StreamUtil.WriteInt32(ModelStream, 96);
                                    StreamUtil.AlignBy16(ModelStream);

                                    ModelStream.Position += 4;
                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x01, 0x01, 0x00, 0x01 });

                                    ModelStream.Position += 7;
                                    StreamUtil.WriteUInt8(ModelStream, 17);
                                }
                            }
                            TempWeightRefGroup.MorphMeshGroupList[c] = TempGroupHeader;
                        }
                        TempMaterialGroup.WeightRefList[b] = TempWeightRefGroup;
                    }
                    Model.MaterialGroupList[a] = TempMaterialGroup;
                }


                //Regenerate Mesh Group
                ModelStream.Position = Model.MaterialGroupOffset;
                //Go to end of structure
                ModelStream.Position += 4 * 5 * Model.MaterialGroupList.Count;
                for (int a = 0; a < Model.MaterialGroupList.Count; a++)
                {
                    ModelStream.Position += Model.MaterialGroupList[a].WeightRefList.Count * 8;
                }
                //Write End Of structure
                for (int a = 0; a < Model.MaterialGroupList.Count; a++)
                {
                    var TempMeshGroup = Model.MaterialGroupList[a];
                    for (int b = 0; b < TempMeshGroup.WeightRefList.Count; b++)
                    {
                        var TempSubGroup = TempMeshGroup.WeightRefList[b];
                        TempSubGroup.MorphMeshOffset = (int)ModelStream.Position;
                        for (int c = 0; c < TempSubGroup.MorphMeshGroupList.Count; c++)
                        {
                            StreamUtil.WriteInt32(ModelStream, TempSubGroup.MorphMeshGroupList[c].MeshOffset);
                            StreamUtil.WriteInt32(ModelStream, TempSubGroup.MorphMeshGroupList[c].MorphOffset);
                        }
                        TempMeshGroup.WeightRefList[b] = TempSubGroup;
                    }
                    Model.MaterialGroupList[a] = TempMeshGroup;
                }

                //Goto 2nd part of structure
                ModelStream.Position = Model.MaterialGroupOffset;
                ModelStream.Position += 4 * 5 * Model.MaterialGroupList.Count;

                //Write 2nd part of structure
                //Write End Of structure
                for (int a = 0; a < Model.MaterialGroupList.Count; a++)
                {
                    var TempMeshGroup = Model.MaterialGroupList[a];
                    TempMeshGroup.WeightRefGroupOffset = (int)ModelStream.Position;
                    for (int b = 0; b < TempMeshGroup.WeightRefList.Count; b++)
                    {
                        var TempSubGroup = TempMeshGroup.WeightRefList[b];
                        StreamUtil.WriteInt32(ModelStream, TempSubGroup.MorphMeshOffset);
                        StreamUtil.WriteInt32(ModelStream, TempSubGroup.MorphMeshGroupList.Count);
                    }
                    Model.MaterialGroupList[a] = TempMeshGroup;
                }

                //Goto start and writestart of structure
                ModelStream.Position = Model.MaterialGroupOffset;
                for (int a = 0; a < Model.MaterialGroupList.Count; a++)
                {
                    StreamUtil.WriteInt32(ModelStream, Model.MaterialGroupList[a].Type);
                    StreamUtil.WriteInt32(ModelStream, Model.MaterialGroupList[a].Material);
                    StreamUtil.WriteInt32(ModelStream, Model.MaterialGroupList[a].Unknown);
                    StreamUtil.WriteInt32(ModelStream, Model.MaterialGroupList[a].WeightRefList.Count);
                    StreamUtil.WriteInt32(ModelStream, Model.MaterialGroupList[a].WeightRefGroupOffset);
                }



                ModelStream.Position = 0;
                Model.Matrix = StreamUtil.ReadBytes(ModelStream, (int)(ModelStream.Length));
                ModelStream.Dispose();
                ModelStream.Close();
                ModelList[i] = Model;
            }


            //Write Matrix and Update Position Offsets
            for (int i = 0; i < ModelList.Count; i++)
            {
                var Model = ModelList[i];
                Model.DataOffset = (int)stream.Position - (96 * ModelList.Count + 4 + 12);
                var TempMatrix = new byte[1];
                if (Compression)
                {
                    RefpackHandler.Compress(Model.Matrix, out TempMatrix);
                }
                else
                {
                    TempMatrix = Model.Matrix;
                }
                StreamUtil.WriteBytes(stream, TempMatrix);
                StreamUtil.AlignBy16(stream);
                Model.EntrySize = Model.Matrix.Length;

                ModelList[i] = Model;
            }


            //Go back and write Header
            stream.Position = 12;
            for (int i = 0; i < ModelList.Count; i++)
            {
                StreamUtil.WriteString(stream, ModelList[i].ModelName, 16);

                StreamUtil.WriteInt32(stream, ModelList[i].DataOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].EntrySize);
                StreamUtil.WriteInt32(stream, ModelList[i].BoneOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].UnusedIKPointOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].MaterialGroupOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].MeshDataOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].MaterialOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].MorphIDOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].WeightRefrenceOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].BoneWeightOffset);
                stream.Position += 12;

                StreamUtil.WriteInt16(stream, ModelList[i].BoneWeightHeaderList.Count);
                StreamUtil.WriteInt16(stream, ModelList[i].WeightRefrenceLists.Count);
                StreamUtil.WriteInt16(stream, ModelList[i].MaterialGroupList.Count);
                StreamUtil.WriteInt16(stream, ModelList[i].BoneList.Count);
                StreamUtil.WriteInt16(stream, ModelList[i].MaterialList.Count);
                StreamUtil.WriteInt16(stream, ModelList[i].UnusedIKPointCount);
                StreamUtil.WriteInt16(stream, ModelList[i].MorphKeyCount);
                StreamUtil.WriteInt16(stream, ModelList[i].FileID);
                StreamUtil.WriteInt16(stream, ModelList[i].TriangleCount);

                stream.Position += 10;
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
