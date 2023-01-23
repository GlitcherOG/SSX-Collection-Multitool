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
        public int HeaderOffset;
        public int FileStart;
        public List<MPFModelHeader> ModelList = new List<MPFModelHeader>();

        public void load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                U1 = StreamUtil.ReadInt32(stream);
                HeaderCount = StreamUtil.ReadInt16(stream);
                HeaderOffset = StreamUtil.ReadInt16(stream);
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
                    modelHeader.MorphKeyCount = StreamUtil.ReadInt16(stream);
                    modelHeader.FileID = StreamUtil.ReadInt16(stream);
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
                    if(streamMatrix.ReadByte()!=0x00)
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

                streamMatrix.Position = Model.BoneDataOffset;
                Model.boneDatas = new List<BoneData>();
                for (int a = 0; a < Model.BoneDataCount; a++)
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
                    TempChunkData.GroupType = StreamUtil.ReadInt32(streamMatrix);
                    TempChunkData.MaterialID = StreamUtil.ReadInt32(streamMatrix);
                    TempChunkData.Unknown = StreamUtil.ReadInt32(streamMatrix);
                    TempChunkData.LinkCount = StreamUtil.ReadInt32(streamMatrix);
                    TempChunkData.LinkOffset = StreamUtil.ReadInt32(streamMatrix);

                    int TempPos = (int)streamMatrix.Position;
                    streamMatrix.Position = TempChunkData.LinkOffset;
                    TempChunkData.meshGroupSubs = new List<WeightRefGroup>();
                    for (int b = 0; b < TempChunkData.LinkCount; b++)
                    {
                        var TempSubHeader = new WeightRefGroup();
                        TempSubHeader.LinkOffset = StreamUtil.ReadInt32(streamMatrix);
                        TempSubHeader.LinkCount = StreamUtil.ReadInt32(streamMatrix);
                        int TempPos1 = (int)streamMatrix.Position;
                        TempSubHeader.MeshGroupHeaders = new List<MeshMorphHeader>();
                        streamMatrix.Position = TempSubHeader.LinkOffset;
                        for (int c = 0; c < TempSubHeader.LinkCount; c++)
                        {
                            var TempMeshGroupHeader = new MeshMorphHeader();
                            TempMeshGroupHeader.ModelOffset = StreamUtil.ReadInt32(streamMatrix);
                            TempMeshGroupHeader.MorphKeyOffset = StreamUtil.ReadInt32(streamMatrix);
                            TempMeshGroupHeader.MorphKeyEntrySize = StreamUtil.ReadInt32(streamMatrix);
                            TempMeshGroupHeader.WeightRefGroup = NumberWeightRef;
                            TempSubHeader.MeshGroupHeaders.Add(TempMeshGroupHeader);
                        }
                        streamMatrix.Position = TempPos1;
                        NumberWeightRef++;
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
                        boneWeight.BoneID = StreamUtil.ReadUInt8(streamMatrix);
                        boneWeight.FileID = StreamUtil.ReadUInt8(streamMatrix);
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
                                byte Temp = StreamUtil.ReadUInt8(streamMatrix);
                                if (Temp != 0x6C)
                                {
                                    break;
                                }
                                streamMatrix.Position += 16;
                                var ModelData = new StaticMesh();

                                ModelData.StripCount = StreamUtil.ReadInt32(streamMatrix);
                                ModelData.Unknown1 = StreamUtil.ReadInt32(streamMatrix);
                                ModelData.Unknown2 = StreamUtil.ReadInt32(streamMatrix); 
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
                                if (ModelData.Unknown2 != 0)
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
                                if (ModelData.Unknown2 != 0)
                                {
                                    streamMatrix.Position += 48;
                                    for (int a = 0; a < ModelData.VertexCount; a++)
                                    {
                                        Vector3 normal = new Vector3();
                                        normal.X = StreamUtil.ReadInt16(streamMatrix) / 32768f;
                                        normal.Y = StreamUtil.ReadInt16(streamMatrix) / 32768f;
                                        normal.Z = StreamUtil.ReadInt16(streamMatrix) / 32768f;
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
                                    if (GroupHeader.GroupType!=17)
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

                            if (SubSubGroupHeader.MorphKeyOffset != -1)
                            {
                                streamMatrix.Position = SubSubGroupHeader.ModelOffset + SubSubGroupHeader.MorphKeyOffset;
                                SubSubGroupHeader.MorphKeyList = new List<MorphKey>();
                                for (int dci = 0; dci < Model.MorphKeyCount; dci++)
                                {
                                    var TempMorphKey = new MorphKey();
                                    TempMorphKey.morphData = new List<Vector3>();
                                    streamMatrix.Position += 30;
                                    TempMorphKey.MorphPointDataCount = StreamUtil.ReadUInt8(streamMatrix);
                                    streamMatrix.Position += 1;
                                    for (int dcb = 0; dcb < TempMorphKey.MorphPointDataCount; dcb++)
                                    {
                                        var TempPoint = new Vector3();
                                        TempPoint.X = StreamUtil.ReadInt8(streamMatrix)/12f;
                                        TempPoint.Y = StreamUtil.ReadInt8(streamMatrix) / 12f;
                                        TempPoint.Z = StreamUtil.ReadInt8(streamMatrix) / 12f;
                                        TempMorphKey.morphData.Add(TempPoint);
                                    }
                                    StreamUtil.AlignBy16(streamMatrix);

                                    streamMatrix.Position += 16;

                                    SubSubGroupHeader.MorphKeyList.Add(TempMorphKey);
                                }
                            }

                            for (int b = 0; b < SubSubGroupHeader.staticMesh.Count; b++)
                            {
                                SubSubGroupHeader.staticMesh[b] = GenerateFaces(SubSubGroupHeader.staticMesh[b], SubSubGroupHeader.MorphKeyList);
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

        public StaticMesh GenerateFaces(StaticMesh models, List<MorphKey> morphPointData)
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
            ModelData.faces = new List<Face>();
            int localIndex = 0;
            bool Rotation = false;
            for (int b = 0; b < ModelData.vertices.Count; b++)
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

                ModelData.faces.Add(CreateFaces(b, ModelData, Rotation, morphPointData));
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
        public Face CreateFaces(int Index, StaticMesh ModelData, bool roatation, List<MorphKey> morphPointData)
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
                face.Weight1Pos = (ModelData.Weights[Index1] - 14) /4;
                face.Weight2Pos = (ModelData.Weights[Index2] - 14) / 4;
                face.Weight3Pos = (ModelData.Weights[Index3] - 14) / 4;
            }

            if(morphPointData!=null)
            {
                face.MorphPoint1 = new List<Vector3>();
                face.MorphPoint2 = new List<Vector3>();
                face.MorphPoint3 = new List<Vector3>();

                for (int i = 0; i < morphPointData.Count; i++)
                {
                    face.MorphPoint1.Add(morphPointData[i].morphData[Index1]);
                    face.MorphPoint2.Add(morphPointData[i].morphData[Index2]);
                    face.MorphPoint3.Add(morphPointData[i].morphData[Index3]);
                }
            }

            return face;
        }

        //Within the entire saving there are only 2 issues one byte that seems to change what it is when ever it feels like it
        //and within the head models from 1500 onwards having extra bytes at the end
        //As far as i can tell these pose no issues in game
        public void Save(string path)
        {
            MemoryStream stream = new MemoryStream();
            StreamUtil.WriteInt32(stream, 8);
            StreamUtil.WriteInt16(stream, ModelList.Count);
            StreamUtil.WriteInt16(stream, 12);
            FileStart = 12 + 80 * ModelList.Count + 4;
            StreamUtil.WriteInt32(stream, FileStart);

            stream.Position += 80 * ModelList.Count + 4;

            for (int i = 0; i < ModelList.Count; i++)
            {
                var Model = ModelList[i];
                MemoryStream ModelStream = new MemoryStream();
                Model.MaterialOffset = (int)ModelStream.Position;
                for (int a = 0; a < Model.materialDatas.Count; a++)
                {
                    StreamUtil.WriteString(ModelStream, Model.materialDatas[a].MainTexture, 4);

                    if(Model.materialDatas[a].Texture1 !="")
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

                Model.BoneDataOffset = (int)ModelStream.Position;
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

                Model.IKPointOffset = (int)ModelStream.Position;
                for (int a = 0; a < Model.iKPoints.Count; a++)
                {
                    StreamUtil.WriteVector3(ModelStream, Model.iKPoints[a]);
                    ModelStream.Position += 4;
                }

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
                        StreamUtil.WriteUInt8(ModelStream, BoneWeightHeader.boneWeights[b].BoneID);
                        StreamUtil.WriteUInt8(ModelStream, BoneWeightHeader.boneWeights[b].FileID);
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
                StreamUtil.WriteUInt8(ModelStream, 0);



                //Mesh Data Offset
                Model.MeshDataOffset = (int)ModelStream.Position;
                bool FirstChunk = false;
                for (int a = 0; a < Model.MeshGroups.Count; a++)
                {
                    var TempMeshGroup = Model.MeshGroups[a];
                    for (int b = 0; b < TempMeshGroup.meshGroupSubs.Count; b++)
                    {
                        var TempSubGroup = TempMeshGroup.meshGroupSubs[b];
                        for (int c = 0; c < TempSubGroup.MeshGroupHeaders.Count; c++)
                        {
                            var TempGroupHeader = TempSubGroup.MeshGroupHeaders[c];
                            TempGroupHeader.ModelOffset = (int)ModelStream.Position;
                            bool MeshTest = false;
                            //Write Mesh Chunk
                            for (int d = 0; d < TempGroupHeader.staticMesh.Count; d++)
                            {
                                var TempStaticMesh = TempGroupHeader.staticMesh[d];
                                int RowCountPos = (int)ModelStream.Position;
                                ModelStream.Position += 3;
                                StreamUtil.WriteInt32(ModelStream, 16);
                                StreamUtil.AlignBy16(ModelStream);

                                StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 });
                                StreamUtil.WriteUInt8(ModelStream, TempStaticMesh.Strips.Count + 2);
                                StreamUtil.WriteUInt8(ModelStream, 0x6C);

                                //Tristrip Header InfoCrap
                                StreamUtil.WriteUInt8(ModelStream, TempStaticMesh.vertices.Count);
                                if (TempMeshGroup.GroupType == 1 || TempMeshGroup.GroupType == 256)
                                {
                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x80, 0x00, 0x00, 0x00, 0x40, 0x2E, 0x30, 0x12, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                                }
                                else if (TempMeshGroup.GroupType == 17)
                                {
                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x80, 0x00, 0x00, 0x00, 0x40, 0x22, 0x10, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                                }

                                StreamUtil.WriteInt32(ModelStream, TempStaticMesh.Strips.Count);
                                StreamUtil.WriteInt32(ModelStream, TempStaticMesh.Unknown1);
                                StreamUtil.WriteInt32(ModelStream, TempStaticMesh.Unknown2);
                                StreamUtil.WriteInt32(ModelStream, TempStaticMesh.vertices.Count);

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

                                if (TempMeshGroup.GroupType != 17)
                                {
                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x10, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x50, 0x50, 0x50, 0x50 });

                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x03, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 });
                                    StreamUtil.WriteUInt8(ModelStream, TempStaticMesh.Strips.Count + 2);
                                    StreamUtil.WriteUInt8(ModelStream, 0x80);
                                    StreamUtil.WriteUInt8(ModelStream, TempStaticMesh.uv.Count);
                                    StreamUtil.WriteUInt8(ModelStream, 0x6D);

                                    for (int e = 0; e < TempStaticMesh.uv.Count; e++)
                                    {
                                        StreamUtil.WriteInt16(ModelStream, (int)(TempStaticMesh.uv[e].X * 4096f));
                                        StreamUtil.WriteInt16(ModelStream, (int)(TempStaticMesh.uv[e].Y * 4096f));
                                        StreamUtil.WriteInt16(ModelStream, (int)(TempStaticMesh.uv[e].Z));
                                        StreamUtil.WriteInt16(ModelStream, (int)(TempStaticMesh.uv[e].W));
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
                                    StreamUtil.WriteUInt8(ModelStream, TempStaticMesh.uvNormals.Count);
                                    StreamUtil.WriteUInt8(ModelStream, 0x79);

                                    for (int e = 0; e < TempStaticMesh.uvNormals.Count; e++)
                                    {
                                        StreamUtil.WriteInt16(ModelStream, (int)(TempStaticMesh.uvNormals[e].X * 32768f));
                                        StreamUtil.WriteInt16(ModelStream, (int)(TempStaticMesh.uvNormals[e].Y * 32768f));
                                        StreamUtil.WriteInt16(ModelStream, (int)(TempStaticMesh.uvNormals[e].Z * 32768f));
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
                                    StreamUtil.WriteUInt8(ModelStream, TempStaticMesh.vertices.Count);
                                    StreamUtil.WriteUInt8(ModelStream, 0x78);

                                    for (int e = 0; e < TempStaticMesh.vertices.Count; e++)
                                    {
                                        StreamUtil.WriteFloat32(ModelStream, (TempStaticMesh.vertices[e].X));
                                        StreamUtil.WriteFloat32(ModelStream, (TempStaticMesh.vertices[e].Y));
                                        StreamUtil.WriteFloat32(ModelStream, (TempStaticMesh.vertices[e].Z));
                                    }
                                    StreamUtil.AlignBy16(ModelStream);

                                    //Go back and write row count
                                    TempPos = (int)ModelStream.Position;
                                    ModelStream.Position = RowCountPos;
                                    StreamUtil.WriteInt24(ModelStream, (TempPos - RowCountPos) / 16 - 1);
                                    ModelStream.Position = TempPos;

                                    //Write New RowCount that neve changes
                                    StreamUtil.WriteInt24(ModelStream, 1);
                                    if (TempMeshGroup.GroupType != 256)
                                    {
                                        StreamUtil.WriteInt32(ModelStream, 16);
                                    }
                                    else
                                    {
                                        StreamUtil.WriteInt32(ModelStream, 0x60);
                                    }
                                    StreamUtil.AlignBy16(ModelStream);

                                    if (TempMeshGroup.GroupType != 256)
                                    {
                                        StreamUtil.WriteBytes(ModelStream, new byte[] { 0x01, 0x01, 0x00, 0x01 });

                                        StreamUtil.WriteUInt8(ModelStream, 0x00); // Can sometimes be 0x0A

                                        StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x14 });

                                        if (!MeshTest)
                                        {
                                            if (TempGroupHeader.staticMesh.Count - 1 != d)
                                            {
                                                StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                                            }
                                            else
                                            {
                                                StreamUtil.WriteBytes(ModelStream, new byte[] { 0x08, 0x00, 0x00, 0x14, 0x06, 0x00, 0x00, 0x14 });
                                            }
                                        }
                                        else
                                        {
                                            StreamUtil.WriteBytes(ModelStream, new byte[] { 0x06, 0x00, 0x00, 0x14, 0x06, 0x00, 0x00, 0x14 });
                                        }
                                        MeshTest = !MeshTest;
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
                                    StreamUtil.WriteUInt8(ModelStream, TempStaticMesh.vertices.Count);
                                    StreamUtil.WriteUInt8(ModelStream, 0x6C);

                                    for (int e = 0; e < TempStaticMesh.vertices.Count; e++)
                                    {
                                        StreamUtil.WriteFloat32(ModelStream, (TempStaticMesh.vertices[e].X));
                                        StreamUtil.WriteFloat32(ModelStream, (TempStaticMesh.vertices[e].Y));
                                        StreamUtil.WriteFloat32(ModelStream, (TempStaticMesh.vertices[e].Z));
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

                            if (TempMeshGroup.GroupType == 256)
                            {
                                TempGroupHeader.MorphKeyOffset = (int)ModelStream.Position - TempGroupHeader.ModelOffset;
                                for (int e = 0; e < Model.MorphKeyCount; e++)
                                {
                                    var TempMorphList = TempGroupHeader.MorphKeyList[e];

                                    int RowCountPos = (int)ModelStream.Position;
                                    ModelStream.Position += 3;
                                    StreamUtil.WriteInt32(ModelStream, 96);
                                    StreamUtil.AlignBy16(ModelStream);

                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x04, 0x04, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0xB6, 0x80 });
                                    StreamUtil.WriteUInt8(ModelStream, TempMorphList.morphData.Count);
                                    StreamUtil.WriteUInt8(ModelStream, 0x7A);

                                    for (int d = 0; d < TempMorphList.morphData.Count; d++)
                                    {
                                        StreamUtil.WriteUInt8(ModelStream, (int)(TempMorphList.morphData[d].X * 12f));
                                        StreamUtil.WriteUInt8(ModelStream, (int)(TempMorphList.morphData[d].Y * 12f));
                                        StreamUtil.WriteUInt8(ModelStream, (int)(TempMorphList.morphData[d].Z * 12f));
                                    }
                                    StreamUtil.AlignBy16(ModelStream);

                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x14 });

                                    int TempPos = (int)ModelStream.Position;
                                    ModelStream.Position = RowCountPos;
                                    StreamUtil.WriteInt24(ModelStream, (TempPos - RowCountPos) / 16 - 1);
                                    ModelStream.Position = TempPos;

                                    TempGroupHeader.MorphKeyEntrySize = TempPos - RowCountPos;
                                }

                                if (a == Model.MeshGroups.Count - 1 && b == TempMeshGroup.meshGroupSubs.Count - 1 && c == TempSubGroup.MeshGroupHeaders.Count - 1)
                                {
                                    StreamUtil.WriteInt24(ModelStream, 1);
                                    StreamUtil.WriteInt32(ModelStream, 96);
                                    StreamUtil.AlignBy16(ModelStream);

                                    ModelStream.Position += 4;
                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x01, 0x01, 0x00, 0x01 });
                                    StreamUtil.AlignBy16(ModelStream);


                                    StreamUtil.WriteInt24(ModelStream, 1);
                                    StreamUtil.WriteInt32(ModelStream, 96);
                                    StreamUtil.AlignBy16(ModelStream);

                                    ModelStream.Position += 12;
                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x01, 0x01, 0x00, 0x01 });

                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x14 });

                                    FirstChunk = true;
                                }
                            }
                            else
                            {
                                TempGroupHeader.MorphKeyOffset = -1;
                                TempGroupHeader.MorphKeyEntrySize = -1;
                            }


                            if (TempMeshGroup.GroupType != 256)
                            {
                                //Write End of Meshdata
                                StreamUtil.WriteInt24(ModelStream, 1);
                                StreamUtil.WriteInt32(ModelStream, 96);
                                StreamUtil.AlignBy16(ModelStream);

                                StreamUtil.WriteBytes(ModelStream, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x01 });
                                StreamUtil.AlignBy16(ModelStream);
                                ModelStream.Position -= 1;
                                StreamUtil.WriteUInt8(ModelStream, 0);

                                if (!FirstChunk || (a == Model.MeshGroups.Count - 1 && b == TempMeshGroup.meshGroupSubs.Count - 1 && c == TempSubGroup.MeshGroupHeaders.Count - 1))
                                {
                                    StreamUtil.WriteInt24(ModelStream, 1);
                                    StreamUtil.WriteInt32(ModelStream, 96);
                                    StreamUtil.AlignBy16(ModelStream);

                                    ModelStream.Position += 12;
                                    StreamUtil.WriteBytes(ModelStream, new byte[] { 0x01, 0x01, 0x00, 0x01 });

                                    ModelStream.Position += 7;
                                    StreamUtil.WriteUInt8(ModelStream, 16);
                                    ModelStream.Position += 7;
                                    StreamUtil.WriteUInt8(ModelStream, 20);
                                    FirstChunk = true;
                                }
                            }


                            TempSubGroup.MeshGroupHeaders[c] = TempGroupHeader;
                        }
                        TempMeshGroup.meshGroupSubs[b] = TempSubGroup;
                    }

                    Model.MeshGroups[a] = TempMeshGroup;
                }

                ModelStream.Position = Model.MeshGroupOffset;
                //Go to end of structure
                ModelStream.Position += 4 * 5 * Model.MeshGroups.Count;
                for (int a = 0; a < Model.MeshGroups.Count; a++)
                {
                    ModelStream.Position += Model.MeshGroups[a].meshGroupSubs.Count*8;
                }
                //Write End Of structure
                for (int a = 0; a < Model.MeshGroups.Count; a++)
                {
                    var TempMeshGroup = Model.MeshGroups[a];
                    for (int b = 0; b < TempMeshGroup.meshGroupSubs.Count; b++)
                    {
                        var TempSubGroup = TempMeshGroup.meshGroupSubs[b];
                        TempSubGroup.LinkOffset = (int)ModelStream.Position;
                        for (int c = 0; c < TempSubGroup.MeshGroupHeaders.Count; c++)
                        {
                            StreamUtil.WriteInt32(ModelStream, TempSubGroup.MeshGroupHeaders[c].ModelOffset);
                            StreamUtil.WriteInt32(ModelStream, TempSubGroup.MeshGroupHeaders[c].MorphKeyOffset);
                            StreamUtil.WriteInt32(ModelStream, TempSubGroup.MeshGroupHeaders[c].MorphKeyEntrySize);
                        }
                        TempMeshGroup.meshGroupSubs[b] = TempSubGroup;
                    }
                    Model.MeshGroups[a] = TempMeshGroup;
                }


                //Goto 2nd part of structure
                ModelStream.Position = Model.MeshGroupOffset;
                ModelStream.Position += 4 * 5 * Model.MeshGroups.Count;

                //Write 2nd part of structure
                //Write End Of structure
                for (int a = 0; a < Model.MeshGroups.Count; a++)
                {
                    var TempMeshGroup = Model.MeshGroups[a];
                    TempMeshGroup.LinkOffset = (int)ModelStream.Position;
                    for (int b = 0; b < TempMeshGroup.meshGroupSubs.Count; b++)
                    {
                        var TempSubGroup = TempMeshGroup.meshGroupSubs[b];
                        StreamUtil.WriteInt32(ModelStream, TempSubGroup.LinkOffset);
                        StreamUtil.WriteInt32(ModelStream, TempSubGroup.MeshGroupHeaders.Count);
                    }
                    Model.MeshGroups[a] = TempMeshGroup;
                }


                //Goto start and writestart of structure
                ModelStream.Position = Model.MeshGroupOffset;
                for (int a = 0; a < Model.MeshGroups.Count; a++)
                {
                    StreamUtil.WriteInt32(ModelStream, Model.MeshGroups[a].GroupType);
                    StreamUtil.WriteInt32(ModelStream, Model.MeshGroups[a].MaterialID);
                    StreamUtil.WriteInt32(ModelStream, Model.MeshGroups[a].Unknown);
                    StreamUtil.WriteInt32(ModelStream, Model.MeshGroups[a].meshGroupSubs.Count);
                    StreamUtil.WriteInt32(ModelStream, Model.MeshGroups[a].LinkOffset);
                }
                ModelStream.Position = 0;
                Model.EntrySize = (int)ModelStream.Length;
                Model.DataOffset = (int)(stream.Position - FileStart);
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
                StreamUtil.WriteInt16(stream, ModelList[i].MorphKeyCount);
                StreamUtil.WriteInt16(stream, ModelList[i].FileID);

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
            public int MorphKeyCount;
            public int FileID;

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


        public struct GroupMainHeader
        {
            public int GroupType; //1 Standard, 17 Shadow, 256 Morph
            public int MaterialID;
            public int Unknown;
            public int LinkCount;
            public int LinkOffset;

            public List<WeightRefGroup> meshGroupSubs;
        }

        public struct WeightRefGroup
        {
            public List<int> weights;

            public int LinkOffset;
            public int LinkCount;

            public List<MeshMorphHeader> MeshGroupHeaders;
        }

        public struct MeshMorphHeader
        {
            public int ModelOffset;
            public int MorphKeyOffset; //Morph Target Offset
            public int MorphKeyEntrySize; //Morph Target Entry Size
            public int WeightRefGroup;

            public List<StaticMesh> staticMesh;
            public List<MorphKey> MorphKeyList;
        }

        public struct MorphKey
        {
            public int MorphPointDataCount;

            public List<Vector3> morphData;
        }

        public struct BoneWeightHeader
        {
            public int length;
            public int WeightListOffset;
            public int unknown; //Always 36

            public List<BoneWeight> boneWeights;
        }

        public struct BoneWeight
        {
            public int Weight;
            public int BoneID;
            public int FileID;

            public string boneName;
        }

        public struct StaticMesh
        {
            public int MatieralID;
            public bool Grouped;

            public List<int> weightsInts;

            public int StripCount;
            public int Unknown1;
            public int Unknown2;
            public int VertexCount;
            public List<int> Strips;

            public List<Vector4> uv;
            public List<Vector3> vertices;
            public List<int> Weights;
            public List<Face> faces;
            public List<Vector3> uvNormals;
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
    }
}
