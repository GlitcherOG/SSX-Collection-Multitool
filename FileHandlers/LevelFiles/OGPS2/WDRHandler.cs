using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.LevelFiles.OGPS2
{
    internal class WDRHandler
    {
        public List<ModelHeader> modelHeaders = new List<ModelHeader>();

        //Model Data
        public void LoadGuess(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                int MeshID = 0;

                while (stream.Position < stream.Length)
                {
                    long StartPos = stream.Position;

                    var NewHeader = new ModelHeader();
                    NewHeader.ModelCount = StreamUtil.ReadUInt32(stream);

                    NewHeader.U1 = StreamUtil.ReadUInt32(stream);
                    NewHeader.ModelByteSize = StreamUtil.ReadUInt32(stream);
                    NewHeader.U2 = StreamUtil.ReadUInt32(stream);

                    NewHeader.Scale = StreamUtil.ReadVector3(stream);
                    NewHeader.U3 = StreamUtil.ReadUInt32(stream);

                    NewHeader.U4 = StreamUtil.ReadUInt32(stream);

                    NewHeader.ModelOffsets = new List<int>();

                    for (int i = 0; i < NewHeader.ModelCount; i++)
                    {
                        NewHeader.ModelOffsets.Add(StreamUtil.ReadUInt32(stream));
                    }

                    StreamUtil.AlignBy16(stream);

                    NewHeader.models = new List<Model>();

                    for (int z = 0; z < NewHeader.ModelCount; z++)
                    {
                        var TempModel = new Model();
                        TempModel.MeshPath = MeshID.ToString() + ".obj";

                        TempModel.vector31 = StreamUtil.ReadVector3(stream);
                        TempModel.vector32 = StreamUtil.ReadVector3(stream);
                        TempModel.U10 = StreamUtil.ReadUInt32(stream);
                        TempModel.U11 = StreamUtil.ReadUInt32(stream);

                        TempModel.U12 = StreamUtil.ReadUInt32(stream);
                        TempModel.U13 = StreamUtil.ReadUInt32(stream);
                        TempModel.U14 = StreamUtil.ReadUInt32(stream);
                        TempModel.U15 = StreamUtil.ReadUInt32(stream);

                        TempModel.U16 = StreamUtil.ReadUInt32(stream);
                        TempModel.U17 = StreamUtil.ReadUInt32(stream);
                        TempModel.U18 = StreamUtil.ReadUInt32(stream);
                        TempModel.U19 = StreamUtil.ReadUInt32(stream);

                        TempModel.modelDatas = new List<ModelData>();
                        StreamUtil.AlignBy(stream, 128, StartPos);
                        while (true)
                        {
                            stream.Position += 48;

                            var TempModelData = new ModelData();

                            TempModelData.TristripCount = StreamUtil.ReadUInt32(stream);
                            TempModelData.U0 = StreamUtil.ReadUInt32(stream);
                            TempModelData.VertexCount = StreamUtil.ReadUInt32(stream);
                            TempModelData.U1 = StreamUtil.ReadUInt32(stream);

                            stream.Position += 16;

                            TempModelData.Tristrip = new List<int>();

                            for (int i = 0; i < TempModelData.TristripCount; i++)
                            {
                                TempModelData.Tristrip.Add(StreamUtil.ReadUInt16(stream)/3);
                            }

                            StreamUtil.AlignBy16(stream);

                            stream.Position += 64;

                            TempModelData.UV = new List<Vector2>();
                            for (int a = 0; a < TempModelData.VertexCount; a++)
                            {
                                Vector2 uv = new Vector2();
                                uv.X = StreamUtil.ReadInt16(stream) / 4096f;
                                uv.Y = StreamUtil.ReadInt16(stream) / 4096f;
                                TempModelData.UV.Add(uv);
                            }
                            StreamUtil.AlignBy16(stream);

                            stream.Position += 48;
                            TempModelData.Normals = new List<Vector3>();

                            for (int a = 0; a < TempModelData.VertexCount; a++)
                            {
                                Vector3 normal = new Vector3();
                                normal.X = StreamUtil.ReadInt16(stream) / 32768f;
                                normal.Y = StreamUtil.ReadInt16(stream) / 32768f;
                                normal.Z = StreamUtil.ReadInt16(stream) / 32768f;
                                TempModelData.Normals.Add(normal);
                            }
                            StreamUtil.AlignBy16(stream);

                            stream.Position += 16;
                            TempModelData.Vertex = new List<Vector3>();
                            for (int a = 0; a < TempModelData.VertexCount; a++)
                            {
                                Vector3 vertex = new Vector3();
                                vertex.X = ((float)StreamUtil.ReadInt16(stream) / 32768f)* NewHeader.Scale.X;
                                vertex.Y = ((float)StreamUtil.ReadInt16(stream) / 32768f) * NewHeader.Scale.Y;
                                vertex.Z = ((float)StreamUtil.ReadInt16(stream) / 32768f) * NewHeader.Scale.Z;
                                TempModelData.Vertex.Add(vertex);
                            }

                            StreamUtil.AlignBy16(stream);

                            stream.Position += 16;

                            stream.Position += 60;

                            int Temp = StreamUtil.ReadUInt32(stream);

                            if (-559038737 == Temp)
                            {
                                stream.Position += 16;

                                if (TempModel.U11 != -559038737)
                                {
                                    MatrixData matrixData = new MatrixData();

                                    matrixData.matrix4 = StreamUtil.ReadMatrix4x4(stream);
                                    matrixData.U0 = StreamUtil.ReadInt16(stream);
                                    matrixData.uStruct0Count = StreamUtil.ReadInt16(stream);
                                    matrixData.U2 = StreamUtil.ReadInt16(stream);
                                    matrixData.U3 = StreamUtil.ReadInt16(stream);
                                    matrixData.U4 = StreamUtil.ReadInt16(stream);
                                    matrixData.U5 = StreamUtil.ReadInt16(stream);
                                    matrixData.U6 = StreamUtil.ReadInt16(stream);
                                    matrixData.U7 = StreamUtil.ReadInt16(stream);

                                    matrixData.uStruct0s = new List<UStruct0>();

                                    for (int c = 0; c < matrixData.uStruct0Count; c++)
                                    {
                                        var UStruct0 = new UStruct0();

                                        UStruct0.UStruct1Count = StreamUtil.ReadUInt32(stream);
                                        UStruct0.UStruct1Offset = StreamUtil.ReadUInt32(stream);

                                        matrixData.uStruct0s.Add(UStruct0);
                                    }

                                    StreamUtil.AlignBy16(stream);

                                    for (int c = 0; c < matrixData.uStruct0s.Count; c++)
                                    {
                                        var TempData = matrixData.uStruct0s[c];

                                        TempData.uStruct1s = new List<UStruct1>();

                                        for (int d = 0; d < TempData.UStruct1Count; d++)
                                        {
                                            var TempUStruct1 = new UStruct1();

                                            TempUStruct1.vector30 = StreamUtil.ReadVector3(stream);
                                            TempUStruct1.vector31 = StreamUtil.ReadVector3(stream);
                                            TempUStruct1.U0 = StreamUtil.ReadUInt32(stream);
                                            TempUStruct1.U1 = StreamUtil.ReadUInt32(stream);

                                            TempData.uStruct1s.Add(TempUStruct1);
                                        }

                                        matrixData.uStruct0s[c] = TempData;
                                    }
                                }


                                TempModel.modelDatas.Add(TempModelData);
                                break;
                            }
                            else
                            {
                                stream.Position -= 64;
                            }

                            TempModel.modelDatas.Add(TempModelData);
                        }
                        MeshID++;

                        TempModel.FullMesh = GenerateFaces(TempModel);

                        NewHeader.models.Add(TempModel);
                    }
                    StreamUtil.AlignBy(stream, 128);
                    modelHeaders.Add(NewHeader);
                }
            }


        }

        public static Mesh GenerateFaces(Model models)
        {
            Mesh mesh = new Mesh();
            mesh.meshFaces = new List<Faces>();
            for (int i = 0; i < models.modelDatas.Count; i++)
            {
                var ModelData = models.modelDatas[i];
                //Increment Strips
                List<int> strip2 = new List<int>();
                strip2.Add(0);
                foreach (var item in ModelData.Tristrip)
                {
                    strip2.Add(strip2[strip2.Count - 1] + item);
                }
                ModelData.Tristrip = strip2;

                //Make Faces
                int localIndex = 0;
                int Rotation = 0;
                for (int b = 0; b < ModelData.Vertex.Count; b++)
                {
                    if (InsideSplits(b, ModelData.Tristrip))
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

                    mesh.meshFaces.Add(CreateFaces(b, ModelData, Rotation));
                    Rotation++;
                    if (Rotation == 2)
                    {
                        Rotation = 0;
                    }
                    localIndex++;
                }
            }
            return mesh;
        }
        public static bool InsideSplits(int Number, List<int> splits)
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
        public static Faces CreateFaces(int Index, ModelData ModelData, int roatation)
        {
            Faces face = new Faces();
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
                Index1 = Index - 2;
                Index2 = Index - 1;
                Index3 = Index;
            }
            face.V1 = ModelData.Vertex[Index1];
            face.V2 = ModelData.Vertex[Index2];
            face.V3 = ModelData.Vertex[Index3];

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

                face.Normal1 = ModelData.Normals[Index1];
                face.Normal2 = ModelData.Normals[Index2];
                face.Normal3 = ModelData.Normals[Index3];

                face.Normal1Pos = Index1;
                face.Normal2Pos = Index2;
                face.Normal3Pos = Index3;
            }

            return face;
        }

        public void Load(string path, WDXHandler.ModelOffset ModelOffsets)
        {

        }

        public void ExportModels(string path)
        {
            //glstHandler.SavePDBModelglTF(path, this);
            for (int a = 0; a < modelHeaders.Count; a++)
            {
                for (int ax = 0; ax < modelHeaders[a].models.Count; ax++)
                {
                    string outputString = "";
                    string output = "# Exported From SSX Using SSX Multitool Modder by GlitcherOG \n";

                    List<Vector3> vertices = new List<Vector3>();
                    List<Vector3> Normals = new List<Vector3>();
                    List<Vector2> UV = new List<Vector2>();
                    outputString += "o Mesh" + ax.ToString() + "\n";
                    var Data = modelHeaders[a].models[ax].FullMesh;
                    for (int b = 0; b < Data.meshFaces.Count; b++)
                    {
                        var Face = Data.meshFaces[b];

                        //Vertices
                        if (!vertices.Contains(Face.V1))
                        {
                            vertices.Add(Face.V1);
                        }
                        int VPos1 = vertices.IndexOf(Face.V1) + 1;

                        if (!vertices.Contains(Face.V2))
                        {
                            vertices.Add(Face.V2);
                        }
                        int VPos2 = vertices.IndexOf(Face.V2) + 1;

                        if (!vertices.Contains(Face.V3))
                        {
                            vertices.Add(Face.V3);
                        }
                        int VPos3 = vertices.IndexOf(Face.V3) + 1;

                        //UVs
                        if (!UV.Contains(Face.UV1))
                        {
                            UV.Add(Face.UV1);
                        }
                        int UPos1 = UV.IndexOf(Face.UV1) + 1;

                        if (!UV.Contains(Face.UV2))
                        {
                            UV.Add(Face.UV2);
                        }
                        int UPos2 = UV.IndexOf(Face.UV2) + 1;

                        if (!UV.Contains(Face.UV3))
                        {
                            UV.Add(Face.UV3);
                        }
                        int UPos3 = UV.IndexOf(Face.UV3) + 1;

                        //Normals
                        if (!Normals.Contains(Face.Normal1))
                        {
                            Normals.Add(Face.Normal1);
                        }
                        int NPos1 = Normals.IndexOf(Face.Normal1) + 1;

                        if (!Normals.Contains(Face.Normal2))
                        {
                            Normals.Add(Face.Normal2);
                        }
                        int NPos2 = Normals.IndexOf(Face.Normal2) + 1;

                        if (!Normals.Contains(Face.Normal3))
                        {
                            Normals.Add(Face.Normal3);
                        }
                        int NPos3 = Normals.IndexOf(Face.Normal3) + 1;

                        outputString += "f " + VPos1.ToString() + "/" + UPos1.ToString() + "/" + NPos1.ToString() + " " + VPos2.ToString() + "/" + UPos2.ToString() + "/" + NPos2.ToString() + " " + VPos3.ToString() + "/" + UPos3.ToString() + "/" + NPos3.ToString() + "\n";
                    }

                    for (int z = 0; z < vertices.Count; z++)
                    {
                        output += "v " + vertices[z].X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + vertices[z].Y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + vertices[z].Z.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\n";
                    }
                    for (int z = 0; z < UV.Count; z++)
                    {
                        output += "vt " + UV[z].X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + (-UV[z].Y).ToString(CultureInfo.InvariantCulture.NumberFormat) + "\n";
                    }
                    for (int z = 0; z < Normals.Count; z++)
                    {
                        output += "vn " + Normals[z].X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Normals[z].Y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Normals[z].Z.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\n";
                    }
                    output += outputString;
                    File.AppendAllText(path + "/" + modelHeaders[a].models[ax].MeshPath, output);

                }

            }
        }

        public struct ModelHeader
        {
            public int Offset;

            public int ModelCount;
            public int U1;
            public int ModelByteSize;
            public int U2;

            public Vector3 Scale;
            public int U3;

            public int U4;
            public List<int> ModelOffsets;

            public byte[] ModelBytes;

            public List<Model> models;
        }

        public struct Model
        {
            public string MeshPath;

            public Vector3 vector31;
            public Vector3 vector32;
            public int U10;
            public int U11; //Matrix Offset

            public int U12;
            public int U13;
            public int U14;
            public int U15; //Mesh Size

            public int U16;
            public int U17; //Mesh Size 1
            public int U18; //Mesh Size 2
            public int U19; //Mesh Size 3

            public List<ModelData> modelDatas;
            public MatrixData matrixData;

            public Mesh FullMesh;
        }

        public struct ModelData
        {
            public int TristripCount;
            public int U0;
            public int VertexCount;
            public int U1;

            public List<int> Tristrip;
            public List<Vector2> UV;
            public List<Vector3> Vertex;
            public List<Vector3> Normals;
        }

        public struct MatrixData
        {
            public Matrix4x4 matrix4;

            //16
            public int U0;
            public int uStruct0Count;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
            public int U7;

            public List<UStruct0> uStruct0s;
        }

        public struct UStruct0
        {
            public int UStruct1Count;
            public int UStruct1Offset;
            public List<UStruct1> uStruct1s;
        }

        public struct UStruct1
        {
            public Vector3 vector30;
            public Vector3 vector31;
            public int U0;
            public int U1;
        }

        public struct Mesh
        {
            public List<Faces> meshFaces;
            //public List<MeshChunk> meshChunk;

        }

        public struct Faces
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

            public bool tripstriped;
        }
    }
}
