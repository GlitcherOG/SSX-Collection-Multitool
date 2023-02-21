using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers.Models
{
    public class SSXOnTourMPF
    {
        public byte[] magicWords = new byte[4];
        public int NumModels;
        public int HeaderSize;
        public int DataOffset;
        public List<MPFHeader> ModelList = new List<MPFHeader>();

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                magicWords = StreamUtil.ReadBytes(stream, 4);
                NumModels = StreamUtil.ReadInt16(stream);
                HeaderSize = StreamUtil.ReadInt16(stream);
                DataOffset = StreamUtil.ReadInt32(stream);

                ModelList = new List<MPFHeader>();
                for (int i = 0; i < NumModels; i++)
                {
                    var TempHeader = new MPFHeader();
                    TempHeader.ModelName = StreamUtil.ReadString(stream, 16);
                    TempHeader.DataOffset = StreamUtil.ReadInt32(stream);
                    TempHeader.EntrySize = StreamUtil.ReadInt32(stream);
                    TempHeader.AltMorphOffset = StreamUtil.ReadInt32(stream);
                    TempHeader.BoneOffset = StreamUtil.ReadInt32(stream);
                    TempHeader.IKPointOffset = StreamUtil.ReadInt32(stream);
                    TempHeader.MaterialGroupOffset = StreamUtil.ReadInt32(stream);
                    TempHeader.ModelDataOffset = StreamUtil.ReadInt32(stream);
                    TempHeader.MaterialOffset = StreamUtil.ReadInt32(stream);
                    TempHeader.MorphOffset = StreamUtil.ReadInt32(stream);
                    TempHeader.AltMorphSize = StreamUtil.ReadInt32(stream);
                    TempHeader.WeightRefrenceOffset = StreamUtil.ReadInt32(stream);
                    TempHeader.BoneWeightOffset = StreamUtil.ReadInt32(stream);

                    stream.Position += 4 * 4;

                    TempHeader.WeightCount = StreamUtil.ReadInt16(stream);
                    TempHeader.WeightRefrenceCount = StreamUtil.ReadInt16(stream);
                    TempHeader.MaterialGroupCount = StreamUtil.ReadInt16(stream);
                    TempHeader.BoneCount = StreamUtil.ReadInt16(stream);
                    TempHeader.MaterialCount = StreamUtil.ReadInt16(stream);
                    TempHeader.IKCount = StreamUtil.ReadInt16(stream);
                    TempHeader.MorphCount = StreamUtil.ReadInt16(stream);
                    TempHeader.AltMorphCount = StreamUtil.ReadInt16(stream);
                    TempHeader.TriangleCount = StreamUtil.ReadInt32(stream);
                    TempHeader.FileID = StreamUtil.ReadInt16(stream);

                    stream.Position += 2 * 5;


                    ModelList.Add(TempHeader);
                }


                //Read Matrix And Decompress
                int StartPos = DataOffset;
                for (int i = 0; i < ModelList.Count; i++)
                {
                    stream.Position = StartPos + ModelList[i].DataOffset;
                    int EntrySize = 0;

                    if (i == ModelList.Count - 1)
                    {
                        EntrySize = (int)((stream.Length - DataOffset) - ModelList[i].DataOffset);
                    }
                    else
                    {
                        EntrySize = ModelList[i + 1].DataOffset - ModelList[i].DataOffset;
                    }


                    MPFHeader modelHandler = ModelList[i];
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

                    //Bone Data
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

                    //Standard Morph Header
                    streamMatrix.Position = TempModel.MorphOffset;
                    TempModel.MorphHeaderList = new List<MorphHeader>();
                    for (int a = 0; a < TempModel.MorphCount; a++)
                    {
                        var TempMorph = new MorphHeader();
                        TempMorph.MorphName = StreamUtil.ReadString(streamMatrix, 28);
                        TempMorph.MorphID = StreamUtil.ReadInt32(streamMatrix);
                        TempModel.MorphHeaderList.Add(TempMorph);
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
                    TempModel.WeightRefrenceList = new List<WeightRefList>();
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
                        TempModel.WeightRefrenceList.Add(NumberListRef);
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
                    int Vertices = 0;
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
                                if (TempMeshMorphChunk.MeshOffset != -1)
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
                                        Vertices += vertices.Count;
                                        streamMatrix.Position += 16 * 3;
                                        TempMeshMorphChunk.MeshChunkList.Add(ModelData);
                                    }

                                }

                                //Load Morph Chunk
                                if (TempMeshMorphChunk.MorphOffset != -1)
                                {
                                    streamMatrix.Position = TempMeshMorphChunk.MeshOffset + TempMeshMorphChunk.MorphOffset;

                                    TempMeshMorphChunk.MorphDataList = new List<MorphKey>();
                                    for (int dci = 0; dci < TempModel.MorphCount; dci++)
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
                                                TempMorphData.vector3.X = (float)StreamUtil.ReadInt8(streamMatrix) / 2.5f;
                                                TempMorphData.vector3.Y = (float)StreamUtil.ReadInt8(streamMatrix) / 2.5f;
                                                TempMorphData.vector3.Z = (float)StreamUtil.ReadInt8(streamMatrix) / 2.5f;
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

                    if(TempModel.AltMorphOffset!=0)
                    {
                        streamMatrix.Position = TempModel.AltMorphOffset;
                        TempModel.AltMorphList = new List<AltMorphHeader>();
                        for (int a = 0; a < TempModel.AltMorphCount; a++)
                        {
                            var TempAltMorph = new AltMorphHeader();
                            TempAltMorph.MorphName = StreamUtil.ReadString(streamMatrix, 28);
                            TempAltMorph.MorphSize = StreamUtil.ReadInt32(streamMatrix);
                            TempAltMorph.MorphOffset = StreamUtil.ReadInt32(streamMatrix);
                            TempModel.AltMorphList.Add(TempAltMorph);
                        }
                    }


                    streamMatrix.Close();
                    streamMatrix.Dispose();
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

                    if (!Point1Test)
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

        public void SaveDecompress(string Path)
        {
            MemoryStream stream = new MemoryStream();

            StreamUtil.WriteBytes(stream, magicWords);
            StreamUtil.WriteInt16(stream, ModelList.Count);
            StreamUtil.WriteInt16(stream, 12);
            StreamUtil.WriteInt32(stream, DataOffset); //FIX OFFSET NUMBER

            for (int i = 0; i < ModelList.Count; i++)
            {
                var TempModel = ModelList[i];

                StreamUtil.WriteString(stream, TempModel.ModelName, 16);

                StreamUtil.WriteInt32(stream, TempModel.DataOffset);
                StreamUtil.WriteInt32(stream, TempModel.EntrySize);
                StreamUtil.WriteInt32(stream, TempModel.AltMorphOffset);
                StreamUtil.WriteInt32(stream, TempModel.BoneOffset);
                StreamUtil.WriteInt32(stream, TempModel.IKPointOffset);
                StreamUtil.WriteInt32(stream, TempModel.MaterialGroupOffset);
                StreamUtil.WriteInt32(stream, TempModel.ModelDataOffset);
                StreamUtil.WriteInt32(stream, TempModel.MaterialOffset);
                StreamUtil.WriteInt32(stream, TempModel.MorphOffset);
                StreamUtil.WriteInt32(stream, TempModel.AltMorphSize);
                StreamUtil.WriteInt32(stream, TempModel.WeightRefrenceOffset);
                StreamUtil.WriteInt32(stream, TempModel.BoneWeightOffset);

                stream.Position += 4 * 4;

                StreamUtil.WriteInt16(stream, TempModel.WeightCount);
                StreamUtil.WriteInt16(stream, TempModel.WeightRefrenceCount);
                StreamUtil.WriteInt16(stream, TempModel.MaterialGroupCount);
                StreamUtil.WriteInt16(stream, TempModel.BoneCount);
                StreamUtil.WriteInt16(stream, TempModel.MaterialCount);
                StreamUtil.WriteInt16(stream, TempModel.IKCount);
                StreamUtil.WriteInt16(stream, TempModel.MorphCount);
                StreamUtil.WriteInt16(stream, TempModel.AltMorphCount);
                StreamUtil.WriteInt32(stream, TempModel.TriangleCount);
                StreamUtil.WriteInt16(stream, TempModel.FileID);

                stream.Position += 2 * 5;
            }

            StreamUtil.AlignBy16(stream);

            for (int i = 0; i < ModelList.Count; i++)
            {
                StreamUtil.WriteBytes(stream, ModelList[i].Matrix);
                StreamUtil.AlignBy16(stream);
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

        public struct MPFHeader
        {
            //Main Header Offsets
            public string ModelName;
            public int DataOffset;
            public int EntrySize;
            public int AltMorphOffset;
            public int BoneOffset;
            public int IKPointOffset;
            public int MaterialGroupOffset;
            public int ModelDataOffset;
            public int MaterialOffset;
            public int MorphOffset;
            public int AltMorphSize;
            public int WeightRefrenceOffset;
            public int BoneWeightOffset;

            //Header Counts
            public int WeightCount;
            public int WeightRefrenceCount;
            public int MaterialGroupCount;
            public int BoneCount;
            public int MaterialCount;
            public int IKCount; 
            public int MorphCount;
            public int AltMorphCount; 
            public int TriangleCount; // Face Count? (Possibly Int32 not Int16)
            public int FileID;


            public byte[] Matrix;

            public List<MaterialData> MaterialList;
            public List<BoneData> BoneList;
            public List<MorphHeader> MorphHeaderList;
            public List<BoneWeightHeader> BoneWeightHeaderList;
            public List<WeightRefList> WeightRefrenceList;
            public List<MaterialGroup> MaterialGroupList;
            public List<AltMorphHeader> AltMorphList;
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

        public struct MorphHeader
        {
            public string MorphName;
            public int MorphID;
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
            public int Unknown; //65535
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

        public struct AltMorphHeader
        {
            public string MorphName;
            public int MorphSize;
            public int MorphOffset;
        }
    }
}
