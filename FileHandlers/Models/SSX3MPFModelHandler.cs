using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SSXMultiTool.FileHandlers
{
    public class SSX3MPFModelHandler
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
                magicWords = StreamUtil.ReadBytes(stream, 4);
                NumModels = StreamUtil.ReadInt16(stream);
                HeaderSize = StreamUtil.ReadInt16(stream);
                DataOffset = StreamUtil.ReadInt32(stream);
                for (int i = 0; i < NumModels; i++)
                {
                    MPFModelHeader modelHeader = new MPFModelHeader();

                    modelHeader.ModelName = StreamUtil.ReadString(stream, 16);
                    modelHeader.DataOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.EntrySize = StreamUtil.ReadInt32(stream);
                    modelHeader.BoneOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.U7 = StreamUtil.ReadInt32(stream);
                    modelHeader.MaterialGroupOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.MeshDataOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.MaterialOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.MorphIDOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.WeightRefrenceOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.BoneWeightOffset = StreamUtil.ReadInt32(stream);

                    modelHeader.U14 = StreamUtil.ReadInt32(stream);
                    modelHeader.U15 = StreamUtil.ReadInt32(stream);
                    modelHeader.U16 = StreamUtil.ReadInt32(stream);

                    modelHeader.WeightCount = StreamUtil.ReadInt16(stream);
                    modelHeader.WeightRefrenceCount = StreamUtil.ReadInt16(stream);
                    modelHeader.MaterialGroupCount = StreamUtil.ReadInt16(stream);
                    modelHeader.BoneCount = StreamUtil.ReadInt16(stream);
                    modelHeader.MaterialCount = StreamUtil.ReadInt16(stream);
                    modelHeader.U22 = StreamUtil.ReadInt16(stream);
                    modelHeader.MorphKeyCount = StreamUtil.ReadInt16(stream);
                    modelHeader.FileID = StreamUtil.ReadInt16(stream);
                    modelHeader.TriangleCount = StreamUtil.ReadInt16(stream);

                    modelHeader.U26 = StreamUtil.ReadInt16(stream);
                    modelHeader.U27 = StreamUtil.ReadInt16(stream);
                    modelHeader.U28 = StreamUtil.ReadInt16(stream);
                    modelHeader.U29 = StreamUtil.ReadInt16(stream);
                    modelHeader.U30 = StreamUtil.ReadInt16(stream);

                    ModelList.Add(modelHeader);
                }

                //Read Matrix And Decompress
                int StartPos = DataOffset;
                for (int i = 0; i < ModelList.Count; i++)
                {
                    stream.Position = StartPos + ModelList[i].DataOffset;

                    MPFModelHeader modelHandler = ModelList[i];
                    modelHandler.Matrix = StreamUtil.ReadBytes(stream, ModelList[i].EntrySize);
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

                        TempBoneData.Unknown6 = StreamUtil.ReadInt32(streamMatrix);

                        TempBoneData.Position = StreamUtil.ReadVector4(streamMatrix);
                        TempBoneData.Rotation = StreamUtil.ReadQuaternion(streamMatrix);
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
                        TempModel.MorphKeyIDList.Add(StreamUtil.ReadInt32(streamMatrix));
                    }

                    //Bone Weight Info 
                    streamMatrix.Position = TempModel.BoneWeightOffset;
                    TempModel.BoneWeightHeaderList = new List<BoneWeightHeader>();
                    for (int b = 0; b < TempModel.WeightCount; b++)
                    {
                        var BoneWeight = new BoneWeightHeader();

                        BoneWeight.WeightCount = StreamUtil.ReadInt32(streamMatrix);
                        BoneWeight.WeightOffset = StreamUtil.ReadInt32(streamMatrix);
                        BoneWeight.Unknown = StreamUtil.ReadInt32(streamMatrix);
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
                        NumberListRef.ListCount = StreamUtil.ReadInt32(streamMatrix);
                        NumberListRef.Offset = StreamUtil.ReadInt32(streamMatrix);
                        NumberListRef.WeightIDs = new List<int>();

                        int TempPos = (int)streamMatrix.Position;
                        streamMatrix.Position = NumberListRef.Offset;
                        for (int c = 0; c < NumberListRef.ListCount; c++)
                        {
                            NumberListRef.WeightIDs.Add(StreamUtil.ReadInt32(streamMatrix));
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
                        TempChunkData.Type = StreamUtil.ReadInt32(streamMatrix);
                        TempChunkData.Material = StreamUtil.ReadInt32(streamMatrix);
                        TempChunkData.Unknown = StreamUtil.ReadInt32(streamMatrix);
                        TempChunkData.WeightRefGroupCount = StreamUtil.ReadInt32(streamMatrix);
                        TempChunkData.WeightRefGroupOffset = StreamUtil.ReadInt32(streamMatrix);

                        int TempPos = (int)streamMatrix.Position;
                        streamMatrix.Position = TempChunkData.WeightRefGroupOffset;
                        TempChunkData.WeightRefList = new List<WeightRefGroup>();
                        for (int b = 0; b < TempChunkData.WeightRefGroupCount; b++)
                        {
                            var TempSubHeader = new WeightRefGroup();
                            TempSubHeader.MorphMeshOffset = StreamUtil.ReadInt32(streamMatrix);
                            TempSubHeader.MorphMeshCount = StreamUtil.ReadInt32(streamMatrix);
                            int TempPos1 = (int)streamMatrix.Position;
                            TempSubHeader.MorphMeshGroupList = new List<MorphMeshGroup>();
                            streamMatrix.Position = TempSubHeader.MorphMeshOffset;
                            for (int c = 0; c < TempSubHeader.MorphMeshCount; c++)
                            {
                                var TempMeshGroupHeader = new MorphMeshGroup();
                                TempMeshGroupHeader.MeshOffset = StreamUtil.ReadInt32(streamMatrix);
                                TempMeshGroupHeader.MorphOffset = StreamUtil.ReadInt32(streamMatrix);
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

                                        ModelData.StripCount = StreamUtil.ReadInt32(streamMatrix);
                                        ModelData.Unknown1 = StreamUtil.ReadInt32(streamMatrix);
                                        ModelData.Unknown2 = StreamUtil.ReadInt32(streamMatrix);
                                        ModelData.VertexCount = StreamUtil.ReadInt32(streamMatrix);

                                        //Load Strip Count
                                        List<int> TempStrips = new List<int>();
                                        for (int d = 0; d < ModelData.StripCount; d++)
                                        {
                                            TempStrips.Add(StreamUtil.ReadInt32(streamMatrix));
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
                                                    ModelData.Weights.Add(StreamUtil.ReadInt32(streamMatrix));
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
                                            TempMorphKey.ListAmmount = StreamUtil.ReadInt32(streamMatrix);
                                            for (int dcb = 0; dcb < TempMorphKey.ListAmmount; dcb++)
                                            {
                                                var TempMorphData = new MorphData();
                                                TempMorphData.vector3.X = (float)StreamUtil.ReadInt8(streamMatrix) / 3f;
                                                TempMorphData.vector3.Y = (float)StreamUtil.ReadInt8(streamMatrix) / 3f;
                                                TempMorphData.vector3.Z = (float)StreamUtil.ReadInt8(streamMatrix) / 3f;
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

            stream.Position = 96 * ModelList.Count + 4+12;

            //Generate Matrix
            for (int i = 0; i < 0; i++)
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
                    StreamUtil.WriteQuaternion(ModelStream, Model.BoneList[a].Rotation);
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


                Model.MeshDataOffset = (int)ModelStream.Position;
                bool FirstChunk = false;
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


                            }
                        }
                    }
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
                Model.EntrySize = ((int)stream.Position - (96 * ModelList.Count + 4 + 12)) - Model.DataOffset;

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
                StreamUtil.WriteInt32(stream, ModelList[i].U7);
                StreamUtil.WriteInt32(stream, ModelList[i].MaterialGroupOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].MeshDataOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].MaterialOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].MorphIDOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].WeightRefrenceOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].BoneWeightOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].U14);
                StreamUtil.WriteInt32(stream, ModelList[i].U15);
                StreamUtil.WriteInt32(stream, ModelList[i].U16);

                StreamUtil.WriteInt16(stream, ModelList[i].BoneWeightHeaderList.Count);
                StreamUtil.WriteInt16(stream, ModelList[i].WeightRefrenceLists.Count);
                StreamUtil.WriteInt16(stream, ModelList[i].MaterialGroupList.Count);
                StreamUtil.WriteInt16(stream, ModelList[i].BoneList.Count);
                StreamUtil.WriteInt16(stream, ModelList[i].MaterialList.Count);
                StreamUtil.WriteInt16(stream, ModelList[i].U22);
                StreamUtil.WriteInt16(stream, ModelList[i].MorphKeyCount);
                StreamUtil.WriteInt16(stream, ModelList[i].FileID);
                StreamUtil.WriteInt16(stream, ModelList[i].TriangleCount);

                StreamUtil.WriteInt16(stream, ModelList[i].U26);
                StreamUtil.WriteInt16(stream, ModelList[i].U27);
                StreamUtil.WriteInt16(stream, ModelList[i].U28);
                StreamUtil.WriteInt16(stream, ModelList[i].U29);
                StreamUtil.WriteInt16(stream, ModelList[i].U30);
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
            public int U7; //Weight Info (IK Points?)
            public int MaterialGroupOffset;
            public int MeshDataOffset;
            public int MaterialOffset;
            public int MorphIDOffset;
            public int WeightRefrenceOffset;
            public int BoneWeightOffset;

            //Unused?
            public int U14;
            public int U15; 
            public int U16;

            //Counts
            public int WeightCount;
            public int WeightRefrenceCount;
            public int MaterialGroupCount;
            public int BoneCount;
            public int MaterialCount;
            public int U22; //IK Point Count??
            public int MorphKeyCount;
            public int FileID;
            public int TriangleCount; //Possibly Some Kind of Face Ammount Used in Store as Well

            //Unused?
            public int U26;
            public int U27;
            public int U28;
            public int U29;
            public int U30;

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
            public int Type;
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
