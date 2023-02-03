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
    internal class SSX3MPFModelHandler
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

                                        TempMorphKey.ListAmmount = StreamUtil.ReadInt32(streamMatrix);
                                        for (int dcb = 0; dcb < TempMorphKey.MorphPointCount; dcb++)
                                        {
                                            var TempMorphData = new MorphData();
                                            TempMorphData.X = StreamUtil.ReadInt8(streamMatrix);
                                            TempMorphData.Y = StreamUtil.ReadInt8(streamMatrix);
                                            TempMorphData.Z = StreamUtil.ReadInt8(streamMatrix);
                                            TempMorphData.ID = StreamUtil.ReadUInt8(streamMatrix);
                                            TempMorphKey.MorphDataList.Add(TempMorphData);
                                        }
                                        StreamUtil.AlignBy16(streamMatrix);

                                        streamMatrix.Position += 16;

                                        TempMeshMorphChunk.MorphDataList.Add(TempMorphKey);
                                    }
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


        public void SaveDecompressed(string path)
        {
            Stream stream = new MemoryStream();
            StreamUtil.WriteBytes(stream, magicWords);
            StreamUtil.WriteInt16(stream, NumModels);
            StreamUtil.WriteInt16(stream, HeaderSize);
            StreamUtil.WriteInt32(stream, DataOffset);

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

                StreamUtil.WriteInt16(stream, ModelList[i].WeightCount);
                StreamUtil.WriteInt16(stream, ModelList[i].WeightRefrenceCount);
                StreamUtil.WriteInt16(stream, ModelList[i].MaterialGroupCount);
                StreamUtil.WriteInt16(stream, ModelList[i].BoneCount);
                StreamUtil.WriteInt16(stream, ModelList[i].MaterialCount);
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
            StreamUtil.AlignBy16(stream);


            for (int i = 0; i < ModelList.Count; i++)
            {
                //Write Matrix
                StreamUtil.WriteBytes(stream, ModelList[i].Matrix);
                StreamUtil.AlignBy16(stream);
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


            public List<int> Strips;
            public List<Vector4> UV;
            public List<Vector3> UVNormals;
            public List<Vector3> Vertices;
            public List<int> Weights;
        }

        public struct MorphKey
        {
            public int MorphPointCount;
            public int ListAmmount;
            public List<MorphData> MorphDataList;
        }

        public struct MorphData
        {
            public int X;
            public int Y;
            public int Z;
            public int ID;
        }
    }
}
