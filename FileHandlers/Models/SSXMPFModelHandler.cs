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
                    modelHeader.IKPointOffset = StreamUtil.ReadInt32(stream);
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
                    boneData.Position = StreamUtil.ReadVector3(streamMatrix);
                    bones.Add(boneData);
                }
                Model.bone = bones;

                //Read IK Point?
                streamMatrix.Position = Model.IKPointOffset;
                var Verties = new List<Vector3>();
                for (int b = 0; b < Model.IKCount; b++)
                {
                    Vector3 v = StreamUtil.ReadVector3(streamMatrix);
                    streamMatrix.Position += 4;
                    Verties.Add(v);
                }
                Model.IKPoint = Verties;

                //Read Chunk Offsets
                streamMatrix.Position = Model.ChunkOffsets;
                Model.MaterialGroups = new List<MaterialGroup>();
                for (int b = 0; b < Model.ChunksCount; b++)
                {
                    MaterialGroup materialGroup = new MaterialGroup();
                    materialGroup.ID = StreamUtil.ReadInt32(streamMatrix);
                    materialGroup.MaterialID = StreamUtil.ReadInt32(streamMatrix);
                    materialGroup.Unknown = StreamUtil.ReadInt32(streamMatrix);
                    materialGroup.MeshOffset = StreamUtil.ReadInt32(streamMatrix);
                    materialGroup.MeshOffsetEnd = StreamUtil.ReadInt32(streamMatrix);
                    materialGroup.MorphMeshOffset = StreamUtil.ReadInt32(streamMatrix);
                    materialGroup.MorphMeshOffsetEnd = StreamUtil.ReadInt32(streamMatrix);
                    Model.MaterialGroups.Add(materialGroup);
                }

                streamMatrix.Position = Model.DataStart;
                //Read Static Model Data
                for (int n = 0; n < Model.MaterialGroups.Count; n++)
                {
                    var TempMaterialGroup = Model.MaterialGroups[n];
                    //Loads All Model Entries
                    TempMaterialGroup.staticMesh = new List<StaticMesh>();
                    if (TempMaterialGroup.MeshOffset != -1)
                    {
                        streamMatrix.Position = Model.MaterialGroups[n].MeshOffset;
                        while (true)
                        {
                            var ModelData = new StaticMesh();
                            //Load Main Model Data Header
                            streamMatrix.Position += 48;

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

                            List<Vector2> UVs = new List<Vector2>();
                            //Read UV Texture Points
                            if (ModelData.NormalCount != 0)
                            {
                                streamMatrix.Position += 48;
                                for (int a = 0; a < ModelData.VertexCount; a++)
                                {
                                    Vector2 uv = new Vector2();
                                    uv.X = StreamUtil.ReadInt16(streamMatrix) / 4096f;
                                    uv.Y = StreamUtil.ReadInt16(streamMatrix) / 4096f;
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
                                streamMatrix.Position += 48;
                                for (int a = 0; a < ModelData.VertexCount; a++)
                                {
                                    Vector3 vertex = new Vector3();
                                    vertex = StreamUtil.ReadVector3(streamMatrix);
                                    vertices.Add(vertex);
                                }
                                StreamUtil.AlignBy16(streamMatrix);
                            }
                            ModelData.vertices = vertices;

                            streamMatrix.Position += 16;
                            ModelData=GenerateFaces(ModelData);
                            TempMaterialGroup.staticMesh.Add(ModelData);
                        }
                    }

                    TempMaterialGroup.flexableMesh = new List<FlexableMesh>();
                    //Load Flex Mesh
                    if (TempMaterialGroup.MorphMeshOffset != -1)
                    {
                        streamMatrix.Position = Model.MaterialGroups[n].MorphMeshOffset;
                        while (true)
                        {
                            var modelSplitData = new FlexableMesh();
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
                            List<Vector3> vertices = new List<Vector3>();
                            for (int a = 0; a < TempCount; a++)
                            {
                                Vector3 vertex = new Vector3();
                                vertex = StreamUtil.ReadVector3(streamMatrix);
                                vertices.Add(vertex);
                            }
                            modelSplitData.vertices = vertices;
                            StreamUtil.AlignBy16(streamMatrix);

                            //Possible Faces/Normal Data
                            streamMatrix.Position += 46;
                            TempCount = StreamUtil.ReadUInt8(streamMatrix);
                            streamMatrix.Position += 1;
                            List<Vector3> Normals = new List<Vector3>();
                            for (int a = 0; a < TempCount; a++)
                            {
                                Vector3 normal = new Vector3();
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
                            List<Vector2> uvs = new List<Vector2>();
                            for (int a = 0; a < TempCount; a++)
                            {
                                Vector2 uv = new Vector2();
                                uv.X = StreamUtil.ReadInt16(streamMatrix);
                                uv.Y = StreamUtil.ReadInt16(streamMatrix);
                                uvs.Add(uv);
                            }
                            modelSplitData.uv = uvs;
                            StreamUtil.AlignBy16(streamMatrix);
                            streamMatrix.Position += 16;

                            TempMaterialGroup.flexableMesh.Add(modelSplitData);
                        }
                    }

                    Model.MaterialGroups[n] = TempMaterialGroup;
                }

                ModelList[i] = Model;
                streamMatrix.Dispose();
                streamMatrix.Close();
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


        public struct MPFModelHeader
        {
            //Main Header
            public string FileName;
            public int DataOffset;
            public int EntrySize;
            public int BoneOffset; 
            public int IKPointOffset; 
            public int ChunkOffsets;
            public int DataStart;
            //Counts
            public int ChunksCount;
            public int BoneCount; 
            public int U22;
            public int MaterialCount;
            public int IKCount;

            public byte[] Matrix;

            public List<Vector3> IKPoint;
            public List<MaterialGroup> MaterialGroups;
            public List<MaterialData> materialDataList;
            public List<Bone> bone;
        }

        public struct FlexableMesh
        {
            public int StripCount;
            public int Unkown1;
            public int Unkown2;
            public int NormalCount;

            public List<Vector3> vertices;
            public List<NewSplit> newSplits;
            public List<Vector3> uvNormals;
            public List<int> UnknownInts;
            public List<Vector2> uv;
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
            public int StripCount;
            public int EdgeCount;
            public int NormalCount;
            public int VertexCount;

            public List<int> Strips;
            public List<Vector2> uv;
            public List<Vector3> vertices;
            public List<Vector3> uvNormals;
            public List<Face> faces;
        }
        public struct Bone
        {
            public string boneName;
            public int Unknown;
            public int BoneParentID;
            public Vector3 Position;
        }

        public struct MaterialGroup
        {
            public int ID;
            public int MaterialID;
            public int Unknown;
            public int MeshOffset;
            public int MeshOffsetEnd;
            public int MorphMeshOffset;
            public int MorphMeshOffsetEnd;

            public List<StaticMesh> staticMesh;
            public List<FlexableMesh> flexableMesh;
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
        }

        public struct FlexableFace
        {
            public Vector3 V1;
            public Vector3 V2;
            public Vector3 V3;

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
