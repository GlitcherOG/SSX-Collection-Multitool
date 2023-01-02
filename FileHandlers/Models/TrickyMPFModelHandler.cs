using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;
using System.Numerics;

namespace SSXMultiTool.FileHandlers
{
    public class TrickyMPFModelHandler
    {
        public int U1;
        public int HeaderCount;
        public int HeaderSize;
        public int FileStart;
        public List<MPFModelHeader> ModelList = new List<MPFModelHeader>();

        public void load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                U1 = StreamUtil.ReadInt32(stream);
                HeaderCount = StreamUtil.ReadInt16(stream);
                HeaderSize = StreamUtil.ReadInt16(stream);
                FileStart = StreamUtil.ReadInt32(stream);
                //Load Headers
                for (int i = 0; i < HeaderCount; i++)
                {
                    MPFModelHeader modelHeader = new MPFModelHeader();

                    modelHeader.FileName = StreamUtil.ReadString(stream, 16);
                    modelHeader.DataOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.EntrySize = StreamUtil.ReadInt32(stream);
                    modelHeader.BoneDataOffset = StreamUtil.ReadInt32(stream); //2
                    modelHeader.IKPointOffset = StreamUtil.ReadInt32(stream); //Done
                    modelHeader.MeshGroupOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.MeshDataOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.MaterialOffset = StreamUtil.ReadInt32(stream); //1
                    modelHeader.NumberListOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.BoneWeightOffet = StreamUtil.ReadInt32(stream);

                    modelHeader.Unused1 = StreamUtil.ReadInt32(stream);
                    modelHeader.Unused2 = StreamUtil.ReadInt32(stream);

                    modelHeader.BoneWeightCount = StreamUtil.ReadInt16(stream);
                    modelHeader.NumberListCount = StreamUtil.ReadInt16(stream);
                    modelHeader.MeshGroupCount = StreamUtil.ReadInt16(stream);
                    modelHeader.BoneDataCount = StreamUtil.ReadInt16(stream);
                    modelHeader.MaterialCount = StreamUtil.ReadInt16(stream);
                    modelHeader.IKCount = StreamUtil.ReadInt16(stream);
                    modelHeader.UnknownCount7 = StreamUtil.ReadInt16(stream);
                    modelHeader.UnknownCount8 = StreamUtil.ReadInt16(stream);
                    stream.Position += 4;

                    ModelList.Add(modelHeader);
                }

                //Read Matrix
                int StartPos = FileStart;
                for (int i = 0; i < ModelList.Count; i++)
                {
                    stream.Position = StartPos + ModelList[i].DataOffset;
                    MPFModelHeader modelHandler = ModelList[i];
                    modelHandler.Matrix = StreamUtil.ReadBytes(stream, ModelList[i].EntrySize);
                    ModelList[i] = modelHandler;
                }
            }

            for (int i = 0; i < ModelList.Count; i++)
            {
                Stream streamMatrix = new MemoryStream();
                var Model = ModelList[i];
                streamMatrix.Write(ModelList[i].Matrix, 0, ModelList[i].Matrix.Length);
                streamMatrix.Position = 0;

                Model.materialDatas = new List<MaterialData>();
                for (int a = 0; a < Model.MaterialCount; a++)
                {
                    var TempMat = new MaterialData();
                    TempMat.MainTexture = StreamUtil.ReadString(streamMatrix, 4);
                    TempMat.Texture1 = StreamUtil.ReadString(streamMatrix, 4);
                    TempMat.Texture2 = StreamUtil.ReadString(streamMatrix, 4);
                    TempMat.Texture3 = StreamUtil.ReadString(streamMatrix, 4);
                    TempMat.Texture4 = StreamUtil.ReadString(streamMatrix, 4);

                    TempMat.R = StreamUtil.ReadFloat(streamMatrix);
                    TempMat.G = StreamUtil.ReadFloat(streamMatrix);
                    TempMat.B = StreamUtil.ReadFloat(streamMatrix);
                    Model.materialDatas.Add(TempMat);
                }

                streamMatrix.Position = Model.BoneDataOffset;
                Model.boneDatas = new List<BoneData>();
                for (int a = 0; a < Model.BoneDataCount; a++)
                {
                    var TempBoneData = new BoneData();
                    TempBoneData.BoneName = StreamUtil.ReadString(streamMatrix, 16);
                    TempBoneData.Unknown = StreamUtil.ReadInt16(streamMatrix);
                    TempBoneData.ParentBone = StreamUtil.ReadInt16(streamMatrix);
                    TempBoneData.Unknown2 = StreamUtil.ReadInt16(streamMatrix);
                    TempBoneData.BoneID = StreamUtil.ReadInt16(streamMatrix);
                    TempBoneData.XLocation = StreamUtil.ReadFloat(streamMatrix);
                    TempBoneData.YLocation = StreamUtil.ReadFloat(streamMatrix);
                    TempBoneData.ZLocation = StreamUtil.ReadFloat(streamMatrix);
                    TempBoneData.XRadian = StreamUtil.ReadFloat(streamMatrix);
                    TempBoneData.YRadian = StreamUtil.ReadFloat(streamMatrix);
                    TempBoneData.ZRadian = StreamUtil.ReadFloat(streamMatrix);
                    TempBoneData.XRadian2 = StreamUtil.ReadFloat(streamMatrix);
                    TempBoneData.YRadian2 = StreamUtil.ReadFloat(streamMatrix);
                    TempBoneData.ZRadian2 = StreamUtil.ReadFloat(streamMatrix);

                    TempBoneData.UnknownFloat1 = StreamUtil.ReadFloat(streamMatrix);
                    TempBoneData.UnknownFloat2 = StreamUtil.ReadFloat(streamMatrix);
                    TempBoneData.UnknownFloat3 = StreamUtil.ReadFloat(streamMatrix);
                    TempBoneData.UnknownFloat4 = StreamUtil.ReadFloat(streamMatrix);
                    TempBoneData.UnknownFloat5 = StreamUtil.ReadFloat(streamMatrix);
                    TempBoneData.UnknownFloat6 = StreamUtil.ReadFloat(streamMatrix);
                    Model.boneDatas.Add(TempBoneData);
                }

                streamMatrix.Position = Model.IKPointOffset;
                Model.iKPoints = new List<Vector3>();
                for (int a = 0; a < Model.IKCount; a++)
                {
                    var TempIKPoint = new Vector3();
                    TempIKPoint = StreamUtil.ReadVector3(streamMatrix);
                    streamMatrix.Position += 4;
                    Model.iKPoints.Add(TempIKPoint);
                }

                //Mesh Group Data
                streamMatrix.Position = Model.MeshGroupOffset;
                Model.MeshGroups = new List<GroupMainHeader>();
                int NumberWeightRef = 0;
                for (int a = 0; a < Model.MeshGroupCount; a++)
                {
                    var TempChunkData = new GroupMainHeader();
                    TempChunkData.ID = StreamUtil.ReadInt32(streamMatrix);
                    TempChunkData.MaterialID = StreamUtil.ReadInt32(streamMatrix);
                    TempChunkData.Unknown = StreamUtil.ReadInt32(streamMatrix);
                    TempChunkData.LinkCount = StreamUtil.ReadInt32(streamMatrix);
                    TempChunkData.LinkOffset = StreamUtil.ReadInt32(streamMatrix);

                    int TempPos = (int)streamMatrix.Position;
                    streamMatrix.Position = TempChunkData.LinkOffset;
                    TempChunkData.meshGroupSubs = new List<GroupSubHeader>();
                    for (int b = 0; b < TempChunkData.LinkCount; b++)
                    {
                        var TempSubHeader = new GroupSubHeader();
                        TempSubHeader.LinkOffset = StreamUtil.ReadInt32(streamMatrix);
                        TempSubHeader.LinkCount = StreamUtil.ReadInt32(streamMatrix);
                        int TempPos1 = (int)streamMatrix.Position;
                        TempSubHeader.MeshGroupHeaders = new List<MeshGroupHeader>();
                        streamMatrix.Position = TempSubHeader.LinkOffset;
                        for (int c = 0; c < TempSubHeader.LinkCount; c++)
                        {
                            var TempMeshGroupHeader = new MeshGroupHeader();
                            TempMeshGroupHeader.ModelOffset = StreamUtil.ReadInt32(streamMatrix);
                            TempMeshGroupHeader.Unknown2 = StreamUtil.ReadInt32(streamMatrix);
                            TempMeshGroupHeader.Unknown3 = StreamUtil.ReadInt32(streamMatrix);
                            TempMeshGroupHeader.WeightRefGroup = NumberWeightRef;
                            NumberWeightRef++;
                            TempSubHeader.MeshGroupHeaders.Add(TempMeshGroupHeader);
                        }
                        streamMatrix.Position = TempPos1;
                        TempChunkData.meshGroupSubs.Add(TempSubHeader);
                    }

                    streamMatrix.Position = TempPos;

                    Model.MeshGroups.Add(TempChunkData);
                }

                //Bone Weight
                streamMatrix.Position = Model.BoneWeightOffet;
                Model.boneWeightHeader = new List<BoneWeightHeader>();

                for (int b = 0; b < Model.BoneWeightCount; b++)
                {
                    var BoneWeight = new BoneWeightHeader();

                    BoneWeight.length = StreamUtil.ReadInt32(streamMatrix);
                    BoneWeight.WeightListOffset = StreamUtil.ReadInt32(streamMatrix);
                    BoneWeight.unknown = StreamUtil.ReadInt32(streamMatrix);
                    BoneWeight.boneWeights = new List<BoneWeight>();
                    int TempPos = (int)streamMatrix.Position;
                    streamMatrix.Position = BoneWeight.WeightListOffset;
                    for (int a = 0; a < BoneWeight.length; a++)
                    {
                        var boneWeight = new BoneWeight();
                        boneWeight.Weight = StreamUtil.ReadInt16(streamMatrix);
                        boneWeight.BoneID = StreamUtil.ReadByte(streamMatrix);
                        boneWeight.Flag = StreamUtil.ReadByte(streamMatrix);
                        BoneWeight.boneWeights.Add(boneWeight);
                    }
                    streamMatrix.Position = TempPos;
                    Model.boneWeightHeader.Add(BoneWeight);
                }

                //Number List Ref
                streamMatrix.Position = Model.NumberListOffset;
                Model.numberListRefs = new List<NumberListRef>();
                for (int b = 0; b < Model.NumberListCount; b++)
                {
                    var NumberListRef = new NumberListRef();
                    NumberListRef.SubCount = StreamUtil.ReadInt32(streamMatrix);
                    NumberListRef.Offset = StreamUtil.ReadInt32(streamMatrix);
                    NumberListRef.WeightIDs = new List<int>();

                    int TempPos = (int)streamMatrix.Position;
                    streamMatrix.Position = NumberListRef.Offset;
                    for (int c = 0; c < NumberListRef.SubCount; c++)
                    {
                        NumberListRef.WeightIDs.Add(StreamUtil.ReadInt32(streamMatrix));
                    }
                    streamMatrix.Position = TempPos;
                    Model.numberListRefs.Add(NumberListRef);
                }

                for (int ax = 0; ax < Model.MeshGroupCount; ax++)
                {
                    var GroupHeader = Model.MeshGroups[ax];
                    for (int bx = 0; bx < GroupHeader.meshGroupSubs.Count; bx++)
                    {
                        var SubGroupHeader = GroupHeader.meshGroupSubs[bx];
                        for (int cx = 0; cx < SubGroupHeader.MeshGroupHeaders.Count; cx++)
                        {
                            var SubSubGroupHeader = SubGroupHeader.MeshGroupHeaders[cx];
                            SubSubGroupHeader.staticMesh = new();
                            streamMatrix.Position = SubSubGroupHeader.ModelOffset;
                            while (true)
                            {
                                streamMatrix.Position += 31;
                                byte Temp = StreamUtil.ReadByte(streamMatrix);
                                if (Temp != 0x6C)
                                {
                                    break;
                                }
                                streamMatrix.Position += 16;
                                var ModelData = new StaticMesh();

                                ModelData.StripCount = StreamUtil.ReadInt32(streamMatrix);
                                ModelData.EdgeCount = StreamUtil.ReadInt32(streamMatrix);
                                ModelData.NormalCount = StreamUtil.ReadInt32(streamMatrix);
                                ModelData.VertexCount = StreamUtil.ReadInt32(streamMatrix);

                                //Load Strip Count
                                List<int> TempStrips = new List<int>();
                                for (int a = 0; a < ModelData.StripCount; a++)
                                {
                                    TempStrips.Add(StreamUtil.ReadInt32(streamMatrix));
                                    streamMatrix.Position += 12;
                                }
                                streamMatrix.Position += 16;
                                ModelData.Strips = TempStrips;

                                List<Vector4> UVs = new List<Vector4>();
                                //Read UV Texture Points
                                if (ModelData.NormalCount != 0)
                                {
                                    streamMatrix.Position += 48;
                                    for (int a = 0; a < ModelData.VertexCount; a++)
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
                                ModelData.uv = UVs;

                                List<Vector3> Normals = new List<Vector3>();
                                //Read Normals
                                if (ModelData.NormalCount != 0)
                                {
                                    streamMatrix.Position += 48;
                                    for (int a = 0; a < ModelData.VertexCount; a++)
                                    {
                                        Vector3 normal = new Vector3();
                                        normal.X = StreamUtil.ReadInt16(streamMatrix)/4096f;
                                        normal.Y = StreamUtil.ReadInt16(streamMatrix) / 4096f;
                                        normal.Z = StreamUtil.ReadInt16(streamMatrix) / 4096f;
                                        Normals.Add(normal);
                                    }
                                    StreamUtil.AlignBy16(streamMatrix);
                                }
                                ModelData.uvNormals = Normals;

                                List<Vector3> vertices = new List<Vector3>();
                                //Load Vertex
                                if (ModelData.VertexCount != 0)
                                {
                                    streamMatrix.Position += 47;
                                    //Can also make it use normal but this seems safer
                                    if (StreamUtil.ReadByte(streamMatrix) != 0x6C)
                                    {
                                        for (int a = 0; a < ModelData.VertexCount; a++)
                                        {
                                            Vector3 vertex = new Vector3();
                                            vertex.X = StreamUtil.ReadFloat(streamMatrix);
                                            vertex.Y = StreamUtil.ReadFloat(streamMatrix);
                                            vertex.Z = StreamUtil.ReadFloat(streamMatrix);
                                            vertices.Add(vertex);
                                        }
                                        StreamUtil.AlignBy16(streamMatrix);
                                    }
                                    else
                                    {
                                        ModelData.Weights = new List<int>();
                                        for (int a = 0; a < ModelData.VertexCount; a++)
                                        {
                                            Vector3 vertex = new Vector3();
                                            vertex.X = StreamUtil.ReadFloat(streamMatrix);
                                            vertex.Y = StreamUtil.ReadFloat(streamMatrix);
                                            vertex.Z = StreamUtil.ReadFloat(streamMatrix);
                                            ModelData.Weights.Add(StreamUtil.ReadInt32(streamMatrix));
                                            vertices.Add(vertex);
                                        }
                                        StreamUtil.AlignBy16(streamMatrix);
                                    }
                                }
                                ModelData.vertices = vertices;

                                streamMatrix.Position += 16 * 2;
                                SubSubGroupHeader.staticMesh.Add(ModelData);
                            }
                            for (int b = 0; b < SubSubGroupHeader.staticMesh.Count; b++)
                            {
                                SubSubGroupHeader.staticMesh[b] = GenerateFaces(SubSubGroupHeader.staticMesh[b]);
                            }
                            SubGroupHeader.MeshGroupHeaders[cx] = SubSubGroupHeader;
                        }
                        GroupHeader.meshGroupSubs[bx] = SubGroupHeader;
                    }
                    Model.MeshGroups[ax] = GroupHeader;
                }


                ModelList[i] = Model;
            }

        }

        public StaticMesh GenerateFaces(StaticMesh models)
        {
            var ModelData = models;
            //Increment Strips
            List<int> strip2 = new List<int>();
            strip2.Add(0);
            foreach (var item in ModelData.Strips)
            {
                strip2.Add(strip2[strip2.Count - 1] + item);
            }
            ModelData.Strips = strip2;

            //Make Faces
            ModelData.faces = new List<Face>();
            int localIndex = 0;
            int Rotation = 0;
            for (int b = 0; b < ModelData.vertices.Count; b++)
            {
                if (InsideSplits(b, ModelData.Strips))
                {
                    Rotation = 0;
                    localIndex = 1;
                    continue;
                }
                if (localIndex < 2)
                {
                    localIndex++;
                    continue;
                }

                ModelData.faces.Add(CreateFaces(b, ModelData, Rotation));
                Rotation++;
                if (Rotation == 2)
                {
                    Rotation = 0;
                }
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
        public Face CreateFaces(int Index, StaticMesh ModelData, int roatation)
        {
            Face face = new Face();
            int Index1 = 0;
            int Index2 = 0;
            int Index3 = 0;
            //Fixes the Rotation For Exporting
            //Swap When Exporting to other formats
            //1-Clockwise
            //0-Counter Clocwise
            if (roatation == 1)
            {
                Index1 = Index;
                Index2 = Index - 1;
                Index3 = Index - 2;
            }
            if (roatation == 0)
            {
                Index1 = Index;
                Index2 = Index - 2;
                Index3 = Index - 1;
            }
            face.V1 = ModelData.vertices[Index1];
            face.V2 = ModelData.vertices[Index2];
            face.V3 = ModelData.vertices[Index3];

            face.V1Pos = Index1;
            face.V2Pos = Index2;
            face.V3Pos = Index3;

            if (ModelData.uv.Count != 0)
            {
                face.UV1 = ModelData.uv[Index1];
                face.UV2 = ModelData.uv[Index2];
                face.UV3 = ModelData.uv[Index3];

                face.UV1Pos = Index1;
                face.UV2Pos = Index2;
                face.UV3Pos = Index3;

                face.Normal1 = ModelData.uvNormals[Index1];
                face.Normal2 = ModelData.uvNormals[Index2];
                face.Normal3 = ModelData.uvNormals[Index3];

                face.Normal1Pos = Index1;
                face.Normal2Pos = Index2;
                face.Normal3Pos = Index3;

                face.Weight1Pos = (int)((face.UV1.Z - 14) / 4);
                face.Weight2Pos = (int)((face.UV2.Z - 14) / 4);
                face.Weight3Pos = (int)((face.UV3.Z - 14) / 4);
            }
            else
            {
                face.Weight1Pos = ModelData.Weights[Index1];
                face.Weight2Pos = ModelData.Weights[Index2];
                face.Weight3Pos = ModelData.Weights[Index3];

            }

            return face;
        }

        public void Save(string path)
        {
            MemoryStream stream = new MemoryStream();
            StreamUtil.WriteInt32(stream, 8);
            StreamUtil.WriteInt16(stream, ModelList.Count);
            StreamUtil.WriteInt16(stream, 12);
            StreamUtil.WriteInt32(stream, 12+ 80 * ModelList.Count + 4);

            stream.Position += 80 * ModelList.Count + 4;

            for (int i = 0; i < ModelList.Count; i++)
            {
                var Model = ModelList[i];
                MemoryStream ModelStream = new MemoryStream();
                Model.MaterialOffset = (int)ModelStream.Position;
                for (int a = 0; a < Model.materialDatas.Count; a++)
                {
                    StreamUtil.WriteString(ModelStream, Model.materialDatas[a].MainTexture, 4);
                    StreamUtil.WriteString(ModelStream, Model.materialDatas[a].Texture1, 4);
                    StreamUtil.WriteString(ModelStream, Model.materialDatas[a].Texture2, 4);
                    StreamUtil.WriteString(ModelStream, Model.materialDatas[a].Texture3, 4);
                    StreamUtil.WriteString(ModelStream, Model.materialDatas[a].Texture4, 4);

                    StreamUtil.WriteFloat32(ModelStream, Model.materialDatas[a].R);
                    StreamUtil.WriteFloat32(ModelStream, Model.materialDatas[a].G);
                    StreamUtil.WriteFloat32(ModelStream, Model.materialDatas[a].B);
                }

                Model.BoneDataOffset = (int)ModelStream.Position;
                for (int a = 0; a < Model.boneDatas.Count; a++)
                {
                    StreamUtil.WriteString(ModelStream, Model.boneDatas[a].BoneName, 16);
                    StreamUtil.WriteInt16(ModelStream, Model.boneDatas[a].Unknown);
                    StreamUtil.WriteInt16(ModelStream, Model.boneDatas[a].ParentBone);
                    StreamUtil.WriteInt16(ModelStream, Model.boneDatas[a].Unknown2);
                    StreamUtil.WriteInt16(ModelStream, Model.boneDatas[a].BoneID);

                    StreamUtil.WriteFloat32(ModelStream, Model.boneDatas[a].XLocation);
                    StreamUtil.WriteFloat32(ModelStream, Model.boneDatas[a].YLocation);
                    StreamUtil.WriteFloat32(ModelStream, Model.boneDatas[a].ZLocation);
                    StreamUtil.WriteFloat32(ModelStream, Model.boneDatas[a].XRadian);
                    StreamUtil.WriteFloat32(ModelStream, Model.boneDatas[a].YRadian);
                    StreamUtil.WriteFloat32(ModelStream, Model.boneDatas[a].ZRadian);
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

                Model.IKPointOffset = (int)ModelStream.Position;
                for (int a = 0; a < Model.iKPoints.Count; a++)
                {
                    StreamUtil.WriteVector3(ModelStream, Model.iKPoints[a]);
                    ModelStream.Position += 4;
                }

                //

                //Bone Weigth
                Model.BoneWeightOffet = (int)ModelStream.Position;
                ModelStream.Position += 12 * Model.boneWeightHeader.Count;
                for (int a = 0; a < Model.boneWeightHeader.Count; a++)
                {
                    var BoneWeightHeader = Model.boneWeightHeader[a];
                    BoneWeightHeader.WeightListOffset = (int)ModelStream.Position;
                    for (int b = 0; b < BoneWeightHeader.boneWeights.Count; b++)
                    {
                        StreamUtil.WriteInt16(ModelStream, BoneWeightHeader.boneWeights[b].Weight);
                        StreamUtil.WriteInt8(ModelStream, BoneWeightHeader.boneWeights[b].BoneID);
                        StreamUtil.WriteInt8(ModelStream, BoneWeightHeader.boneWeights[b].Flag);
                    }
                    Model.boneWeightHeader[a] = BoneWeightHeader;
                }

                Model.NumberListOffset = (int)ModelStream.Position;
                ModelStream.Position = Model.BoneWeightOffet;
                for (int a = 0; a < Model.boneWeightHeader.Count; a++)
                {
                    StreamUtil.WriteInt32(ModelStream, Model.boneWeightHeader[a].boneWeights.Count);
                    StreamUtil.WriteInt32(ModelStream, Model.boneWeightHeader[a].WeightListOffset);
                    StreamUtil.WriteInt32(ModelStream, Model.boneWeightHeader[a].unknown);
                }

                ModelStream.Position = Model.NumberListOffset;
                //Number Ref List
                ModelStream.Position += Model.numberListRefs.Count * 8;
                for (int a = 0; a < Model.numberListRefs.Count; a++)
                {
                    var TempNumberRef = Model.numberListRefs[a];
                    TempNumberRef.Offset = (int)ModelStream.Position;
                    Model.numberListRefs[a] = TempNumberRef;

                    for (int c = 0; c < TempNumberRef.WeightIDs.Count; c++)
                    {
                        StreamUtil.WriteInt32(ModelStream,TempNumberRef.WeightIDs[c]);
                    }
                }

                Model.MeshGroupOffset = (int)ModelStream.Position;
                ModelStream.Position = Model.NumberListOffset;

                for (int a = 0; a < Model.numberListRefs.Count; a++)
                {
                    var TempNumberRef = Model.numberListRefs[a];
                    StreamUtil.WriteInt32(ModelStream, TempNumberRef.WeightIDs.Count);
                    StreamUtil.WriteInt32(ModelStream, TempNumberRef.Offset);
                }


                ModelStream.Position = Model.MeshGroupOffset;

                int MathOffset = 0;
                //Mesh Group
                for (int a = 0; a < Model.MeshGroups.Count; a++)
                {
                    MathOffset += 4 * 5;
                    for (int b = 0; b < Model.MeshGroups[a].meshGroupSubs.Count; b++)
                    {
                        MathOffset += 8 + Model.MeshGroups[a].meshGroupSubs[b].MeshGroupHeaders.Count *4* 3;
                    }
                }

                ModelStream.Position += MathOffset;
                StreamUtil.AlignBy16(ModelStream);
                ModelStream.Position -= 1;
                StreamUtil.WriteInt8(ModelStream, 0);



                //Mesh Data Offset
                Model.MeshDataOffset = (int)ModelStream.Position;


                StreamUtil.WriteStreamIntoStream(stream, ModelStream);
                ModelStream.Dispose();
                ModelStream.Close();
                ModelList[i] = Model;
            }


            stream.Position = 12;
            for (int i = 0; i < ModelList.Count; i++)
            {
                StreamUtil.WriteString(stream, ModelList[i].FileName, 16);
                StreamUtil.WriteInt32(stream, ModelList[i].DataOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].EntrySize);
                StreamUtil.WriteInt32(stream, ModelList[i].BoneDataOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].IKPointOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].MeshGroupOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].MeshDataOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].MaterialOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].NumberListOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].BoneWeightOffet);

                StreamUtil.WriteInt32(stream, ModelList[i].Unused1);
                StreamUtil.WriteInt32(stream, ModelList[i].Unused2);

                StreamUtil.WriteInt16(stream, ModelList[i].boneWeightHeader.Count);
                StreamUtil.WriteInt16(stream, ModelList[i].numberListRefs.Count);
                StreamUtil.WriteInt16(stream, ModelList[i].MeshGroups.Count);
                StreamUtil.WriteInt16(stream, ModelList[i].boneDatas.Count);
                StreamUtil.WriteInt16(stream, ModelList[i].materialDatas.Count);
                StreamUtil.WriteInt16(stream, ModelList[i].iKPoints.Count);
                StreamUtil.WriteInt16(stream, ModelList[i].UnknownCount7);
                StreamUtil.WriteInt16(stream, ModelList[i].UnknownCount8);

                stream.Position += 4;
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
            public string FileName;
            public int DataOffset;
            public int EntrySize;
            public int BoneDataOffset;
            public int IKPointOffset;
            public int MeshGroupOffset;
            public int MeshDataOffset;
            public int MaterialOffset;
            public int NumberListOffset;
            public int BoneWeightOffet;
            public int Unused1;
            public int Unused2;

            //Counts
            public int BoneWeightCount;
            public int NumberListCount;
            public int MeshGroupCount;
            public int BoneDataCount;
            public int MaterialCount;
            public int IKCount;
            public int UnknownCount7;
            public int UnknownCount8;

            public byte[] Matrix;

            public List<MaterialData> materialDatas;
            public List<BoneData> boneDatas;
            public List<Vector3> iKPoints;
            public List<GroupMainHeader> MeshGroups;
            public List<BoneWeightHeader> boneWeightHeader;
            public List<NumberListRef> numberListRefs;
        }

        public struct NumberListRef
        {
            public int SubCount;
            public int Offset;

            public List<int> WeightIDs;
        }

        public struct MaterialData
        {
            public string MainTexture;
            public string Texture1;
            public string Texture2;
            public string Texture3;
            public string Texture4;

            public float R;
            public float G;
            public float B;
        }

        public struct BoneData
        {
            public string BoneName;
            public int Unknown;
            public int ParentBone;
            public int Unknown2;
            public int BoneID;
            public float XLocation;
            public float YLocation;
            public float ZLocation;

            public float XRadian;
            public float YRadian;
            public float ZRadian;
            public float XRadian2;
            public float YRadian2;
            public float ZRadian2;

            public float UnknownFloat1;
            public float UnknownFloat2;
            public float UnknownFloat3;
            public float UnknownFloat4;
            public float UnknownFloat5;
            public float UnknownFloat6;

        }


        public struct GroupMainHeader
        {
            public int ID;
            public int MaterialID;
            public int Unknown;
            public int LinkCount;
            public int LinkOffset;

            public List<GroupSubHeader> meshGroupSubs;
        }

        public struct GroupSubHeader
        {
            public int LinkOffset;
            public int LinkCount;

            public List<MeshGroupHeader> MeshGroupHeaders;
        }

        public struct MeshGroupHeader
        {
            public int ModelOffset;
            public int Unknown2;
            public int Unknown3;
            public int WeightRefGroup;

            public List<StaticMesh> staticMesh;
        }

        public struct BoneWeightHeader
        {
            public int length;
            public int WeightListOffset;
            public int unknown;

            public List<BoneWeight> boneWeights;
        }

        public struct BoneWeight
        {
            public int Weight;
            public int BoneID;
            public int Flag;
        }

        public struct StaticMesh
        {
            public int StripCount;
            public int EdgeCount;
            public int NormalCount;
            public int VertexCount;
            public List<int> Strips;

            public List<Vector4> uv;
            public List<Vector3> vertices;
            public List<int> Weights;
            public List<Face> faces;
            public List<Vector3> uvNormals;
        }


        public struct Face
        {
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

            public int MaterialID;
        }
    }
}
