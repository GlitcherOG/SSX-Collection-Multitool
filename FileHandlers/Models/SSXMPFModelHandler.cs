using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers
{
    public class SSXMPFModelHandler
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
                U1= StreamUtil.ReadInt32(stream);
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
                    modelHeader.BoneOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.IKPoint = StreamUtil.ReadInt32(stream);
                    modelHeader.ChunkOffsets = StreamUtil.ReadInt32(stream);
                    modelHeader.DataStart = StreamUtil.ReadInt32(stream);

                    modelHeader.ChunksCount = StreamUtil.ReadInt16(stream);
                    modelHeader.BoneCount = StreamUtil.ReadInt16(stream);
                    modelHeader.U22 = StreamUtil.ReadInt16(stream);
                    modelHeader.MaterialCount = StreamUtil.ReadUInt8(stream);
                    modelHeader.IKCount = StreamUtil.ReadUInt8(stream);
                    stream.Position += 16;
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

            //Read Matrix Data
            for (int i = 0; i < ModelList.Count; i++)
            {
                Stream streamMatrix = new MemoryStream();
                var Model = ModelList[i];
                streamMatrix.Write(ModelList[i].Matrix, 0, ModelList[i].Matrix.Length);

                //Read BodyObjects
                Model.materialDataList = new List<MaterialData>();
                streamMatrix.Position = 0;
                for (int b = 0; b < Model.MaterialCount; b++)
                {
                    MaterialData body = new MaterialData();
                    body.Name = StreamUtil.ReadString(streamMatrix, 4);
                    body.X = StreamUtil.ReadFloat(streamMatrix);
                    body.Y = StreamUtil.ReadFloat(streamMatrix);
                    body.Z = StreamUtil.ReadFloat(streamMatrix);
                    Model.materialDataList.Add(body);
                }

                //Read Bone Data
                streamMatrix.Position = Model.BoneOffset;
                var bones = new List<Bone>();
                for (int b = 0; b < Model.BoneCount; b++)
                {
                    Bone boneData = new Bone();
                    boneData.boneName = StreamUtil.ReadString(streamMatrix, 16);
                    boneData.Unknown = StreamUtil.ReadInt16(streamMatrix);
                    boneData.BoneParentID = StreamUtil.ReadInt16(streamMatrix);
                    boneData.X = StreamUtil.ReadFloat(streamMatrix);
                    boneData.Y = StreamUtil.ReadFloat(streamMatrix);
                    boneData.Z = StreamUtil.ReadFloat(streamMatrix);
                    bones.Add(boneData);
                }
                Model.bone = bones;

                //Read IK Point?
                streamMatrix.Position = Model.IKPoint;
                var Verties = new List<Vertex3>();
                for (int b = 0; b < Model.IKCount; b++)
                {
                    Vertex3 v = new Vertex3();
                    v.X = StreamUtil.ReadFloat(streamMatrix);
                    v.Y = StreamUtil.ReadFloat(streamMatrix);
                    v.Z = StreamUtil.ReadFloat(streamMatrix);
                    streamMatrix.Position += 4;
                    Verties.Add(v);
                }
                Model.Unkown = Verties;

                //Read Chunk Offsets
                streamMatrix.Position = Model.ChunkOffsets;
                Model.MaterialGroups = new List<MaterialGroup>();
                for (int b = 0; b < Model.ChunksCount; b++)
                {
                    MaterialGroup chunk = new MaterialGroup();
                    chunk.ID = StreamUtil.ReadInt32(streamMatrix);
                    chunk.MaterialID = StreamUtil.ReadInt32(streamMatrix);
                    streamMatrix.Position += 4;
                    chunk.MeshOffset = StreamUtil.ReadInt32(streamMatrix);
                    chunk.MeshOffsetEnd = StreamUtil.ReadInt32(streamMatrix);
                    chunk.MorphMeshOffset = StreamUtil.ReadInt32(streamMatrix);
                    chunk.MorphMeshOffsetEnd = StreamUtil.ReadInt32(streamMatrix);
                    Model.MaterialGroups.Add(chunk);
                }
                Model.staticMesh = new List<StaticMesh>();

                Model.flexableMesh = new List<FlexableMesh>();

                streamMatrix.Position = Model.DataStart;
                //Read Static Model Data
                for (int n = 0; n < Model.ChunksCount; n++)
                {
                    //Loads All Model Entries
                    if (Model.MaterialGroups[n].MeshOffset != -1)
                    {
                        streamMatrix.Position = Model.MaterialGroups[n].MeshOffset;
                        while (true)
                        {
                            Model.MeshCount++;
                            var ModelData = new StaticMesh();
                            ModelData.ChunkID = n;
                            //Load Main Model Data Header

                            if (streamMatrix.Position >= Model.MaterialGroups[n].MeshOffsetEnd)
                            {
                                break;
                            }
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

                            List<UV> UVs = new List<UV>();
                            //Read UV Texture Points
                            if (ModelData.NormalCount != 0)
                            {
                                streamMatrix.Position += 48;
                                for (int a = 0; a < ModelData.VertexCount; a++)
                                {
                                    UV uv = new UV();
                                    uv.X = StreamUtil.ReadInt16(streamMatrix);
                                    uv.Y = StreamUtil.ReadInt16(streamMatrix);
                                    UVs.Add(uv);
                                }
                                StreamUtil.AlignBy16(streamMatrix);
                            }
                            ModelData.uv = UVs;

                            List<UVNormal> Normals = new List<UVNormal>();
                            //Read Normals
                            if (ModelData.NormalCount != 0)
                            {
                                streamMatrix.Position += 48;
                                for (int a = 0; a < ModelData.VertexCount; a++)
                                {
                                    UVNormal normal = new UVNormal();
                                    normal.X = StreamUtil.ReadInt16(streamMatrix);
                                    normal.Y = StreamUtil.ReadInt16(streamMatrix);
                                    normal.Z = StreamUtil.ReadInt16(streamMatrix);
                                    Normals.Add(normal);
                                }
                                StreamUtil.AlignBy16(streamMatrix);
                            }
                            ModelData.uvNormals = Normals;

                            List<Vertex3> vertices = new List<Vertex3>();
                            //Load Vertex
                            if (ModelData.VertexCount != 0)
                            {
                                streamMatrix.Position += 48;
                                for (int a = 0; a < ModelData.VertexCount; a++)
                                {
                                    Vertex3 vertex = new Vertex3();
                                    vertex.X = StreamUtil.ReadFloat(streamMatrix);
                                    vertex.Y = StreamUtil.ReadFloat(streamMatrix);
                                    vertex.Z = StreamUtil.ReadFloat(streamMatrix);
                                    vertices.Add(vertex);
                                }
                                StreamUtil.AlignBy16(streamMatrix);
                            }
                            ModelData.vertices = vertices;

                            streamMatrix.Position += 16;
                            Model.staticMesh.Add(ModelData);
                        }
                    }

                    //Load Flex Mesh
                    if(Model.MaterialGroups[n].MorphMeshOffset != -1)
                    {
                        streamMatrix.Position = Model.MaterialGroups[n].MorphMeshOffset;
                        while (true)
                        {
                            Model.FlexMeshCount++;
                            var modelSplitData = new FlexableMesh();
                            modelSplitData.ChunkID = n;
                            streamMatrix.Position += 48;
                            if (streamMatrix.Position >= Model.MaterialGroups[n].MorphMeshOffsetEnd)
                            {
                                break;
                            }

                            modelSplitData.StripCount = StreamUtil.ReadInt32(streamMatrix);
                            modelSplitData.Unkown1 = StreamUtil.ReadInt32(streamMatrix);
                            modelSplitData.Unkown2 = StreamUtil.ReadInt32(streamMatrix);
                            modelSplitData.NormalCount = StreamUtil.ReadInt32(streamMatrix);

                            //Load Strip Count
                            var TempStrips = new List<NewSplit>();
                            for (int a = 0; a < modelSplitData.StripCount; a++)
                            {
                                NewSplit newSplit = new NewSplit();
                                newSplit.Unknown = StreamUtil.ReadInt32(streamMatrix);
                                newSplit.Split = StreamUtil.ReadInt32(streamMatrix);
                                newSplit.Unknown2 = StreamUtil.ReadInt32(streamMatrix); //To do with UV
                                newSplit.Unknown3 = StreamUtil.ReadInt32(streamMatrix);
                                TempStrips.Add(newSplit);
                            }
                            modelSplitData.newSplits = TempStrips;

                            //Load Vertices
                            streamMatrix.Position += 46;
                            int TempCount = StreamUtil.ReadUInt8(streamMatrix);
                            streamMatrix.Position += 1;
                            List<Vertex3> vertices = new List<Vertex3>();
                            for (int a = 0; a < TempCount; a++)
                            {
                                Vertex3 vertex = new Vertex3();
                                vertex.X = StreamUtil.ReadFloat(streamMatrix);
                                vertex.Y = StreamUtil.ReadFloat(streamMatrix);
                                vertex.Z = StreamUtil.ReadFloat(streamMatrix);
                                vertices.Add(vertex);
                            }
                            modelSplitData.vertices = vertices;
                            StreamUtil.AlignBy16(streamMatrix);

                            //Possible Faces/Normal Data
                            streamMatrix.Position += 46;
                            TempCount = StreamUtil.ReadUInt8(streamMatrix);
                            streamMatrix.Position += 1;
                            List<UVNormal> Normals = new List<UVNormal>();
                            for (int a = 0; a < TempCount; a++)
                            {
                                UVNormal normal = new UVNormal();
                                normal.X = StreamUtil.ReadInt16(streamMatrix);
                                normal.Y = StreamUtil.ReadInt16(streamMatrix);
                                normal.Z = StreamUtil.ReadInt16(streamMatrix);
                                Normals.Add(normal);
                            }
                            modelSplitData.uvNormals = Normals;
                            StreamUtil.AlignBy16(streamMatrix);

                            //Unknown
                            streamMatrix.Position += 14;
                            TempCount = StreamUtil.ReadUInt8(streamMatrix); 
                            streamMatrix.Position += 1;
                            List<int> ints = new List<int>();
                            for (int a = 0; a < TempCount; a++)
                            {
                                ints.Add(StreamUtil.ReadInt16(streamMatrix));
                            }
                            modelSplitData.UnknownInts = ints;
                            StreamUtil.AlignBy16(streamMatrix);

                            //Unknown
                            streamMatrix.Position += 46;
                            TempCount = StreamUtil.ReadUInt8(streamMatrix);
                            streamMatrix.Position += 1;
                            List<UV> uvs = new List<UV>();
                            for (int a = 0; a < TempCount; a++)
                            {
                                UV uv = new UV();
                                uv.X = StreamUtil.ReadInt16(streamMatrix);
                                uv.Y = StreamUtil.ReadInt16(streamMatrix);
                                uvs.Add(uv);
                            }
                            modelSplitData.uv = uvs;
                            StreamUtil.AlignBy16(streamMatrix);
                            streamMatrix.Position += 16;

                            Model.flexableMesh.Add(modelSplitData);
                        }
                    }
                }


                for (int b = 0; b < Model.staticMesh.Count; b++)
                {
                    Model.staticMesh[b] = GenerateFaces(Model.staticMesh[b]);
                }

                ModelList[i] = Model;
                streamMatrix.Dispose();
                streamMatrix.Close();
            }
        }

        public List<Vertex3> ReadVertex(int count, Stream stream, List<Vertex3> vertices)
        {
            for (int a = 0; a < count; a++)
            {
                Vertex3 vertex = new Vertex3();
                vertex.X = StreamUtil.ReadFloat(stream);
                vertex.Y = StreamUtil.ReadFloat(stream);
                vertex.Z = StreamUtil.ReadFloat(stream);
                vertices.Add(vertex);
            }
            return vertices;
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
                if(Rotation==2)
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
                if(item==Number)
                {
                    return true;
                }
            }
            return false;
        }
        public Face CreateFaces(int Index, StaticMesh ModelData,int roatation)
        {
            Face face = new Face();
            int Index1 = 0;
            int Index2 = 0;
            int Index3 = 0;
            //Fixes the Rotation For Exporting
            //Swap When Exporting to other formats
            //1-Clockwise
            //0-Counter Clocwise
            if(roatation==1)
            {
                Index1 = Index;
                Index2 = Index - 1;
                Index3 = Index - 2;
            }
            if(roatation==0)
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
            }

            return face;
        }


        public FlexableFace CreateFlexableFace(int Index, FlexableMesh ModelData, int roatation)
        {
            FlexableFace face = new FlexableFace();
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
                Index2 = Index-1;
                Index3 = Index-2;
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

            return face;
        }

        public FlexableMesh GenerateFaces1(FlexableMesh models)
        {
            var ModelData = models;
            //Increment Strips
            List<int> strip2 = new List<int>();
            strip2.Add(0);
            foreach (var item in ModelData.newSplits)
            {
                strip2.Add(strip2[strip2.Count - 1] + item.Split);
                strip2.Add(strip2[strip2.Count - 1] + item.Unknown2);
            }

            //Make Faces
            ModelData.faces = new List<FlexableFace>();
            int localIndex = 0;
            int Rotation = 0;

            for (int b = 0; b < ModelData.vertices.Count; b++)
            {
                if (InsideSplits(b, strip2))
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

                ModelData.faces.Add(CreateFlexableFace(b, ModelData, Rotation));
                Rotation++;
                if (Rotation == 2)
                {
                    Rotation = 0;
                }
                localIndex++;
            }

            return ModelData;
        }


        public struct MPFModelHeader
        {
            //Main Header
            public string FileName;
            public int DataOffset;
            public int EntrySize;
            public int BoneOffset; 
            public int IKPoint; 
            public int ChunkOffsets;
            public int DataStart;
            //Counts
            public int ChunksCount;
            public int BoneCount; 
            public int U22;
            public int MaterialCount;
            public int IKCount; //Possible Rotation Ammount 

            public byte[] Matrix;

            public List<Vertex3> Unkown;
            //Matrix Data
            public List<MaterialGroup> MaterialGroups;
            public List<MaterialData> materialDataList;
            public List<StaticMesh> staticMesh;
            public List<FlexableMesh> flexableMesh;
            public List<Bone> bone;
            //

            public int MeshCount;
            public int FlexMeshCount;
        }

        public struct FlexableMesh
        {
            public int ChunkID;

            public int StripCount;
            public int Unkown1;
            public int Unkown2;
            public int NormalCount;

            public List<Vertex3> vertices;
            public List<NewSplit> newSplits;
            public List<UVNormal> uvNormals;
            public List<int> UnknownInts;
            public List<UV> uv;
            public List<FlexableFace> faces;

        }

        public struct NewSplit
        {
            public int Unknown;
            public int Split;
            public int Unknown2;
            public int Unknown3;
        }
        public struct StaticMesh
        {
            public int ChunkID;

            public int StripCount;
            public int EdgeCount;
            public int NormalCount;
            public int VertexCount;

            public List<int> Strips;
            public List<UV> uv;
            public List<Vertex3> vertices;
            public List<UVNormal> uvNormals;

            public List<Face> faces;
        }
        public struct Bone
        {
            public string boneName;
            public int Unknown;
            public int BoneParentID;
            public float X;
            public float Y;
            public float Z;
        }

        public struct MaterialGroup
        {
            public int ID;
            public int MaterialID;
            public int MeshOffset;
            public int MeshOffsetEnd;
            public int MorphMeshOffset;
            public int MorphMeshOffsetEnd;
        }

        public struct Vertex3
        {
            public float X;
            public float Y;
            public float Z;
        }

        //Since there both int 16's They need to be divided by 4096
        public struct UV
        {
            public int X;
            public int Y;
        }

        public struct UVNormal
        {
            public int X;
            public int Y;
            public int Z;
        }

        public struct Face
        {
            public Vertex3 V1;
            public Vertex3 V2;
            public Vertex3 V3;

            public int V1Pos;
            public int V2Pos;
            public int V3Pos;

            public UV UV1;
            public UV UV2;
            public UV UV3;

            public int UV1Pos;
            public int UV2Pos;
            public int UV3Pos;

            public UVNormal Normal1;
            public UVNormal Normal2;
            public UVNormal Normal3;

            public int Normal1Pos;
            public int Normal2Pos;
            public int Normal3Pos;
        }

        public struct FlexableFace
        {
            public Vertex3 V1;
            public Vertex3 V2;
            public Vertex3 V3;

            public int V1Pos;
            public int V2Pos;
            public int V3Pos;
        }

        public struct MaterialData
        {
            public string Name;
            public float X;
            public float Y;
            public float Z;
        }
    }
}
