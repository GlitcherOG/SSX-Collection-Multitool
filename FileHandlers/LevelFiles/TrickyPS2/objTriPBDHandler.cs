using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.FileHandlers.Models;

namespace SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2
{
    public class objTriPBDHandler
    {
        public List<ModelObject> modelObjects = new List<ModelObject>();
        public void LoadFiles(string path)
        {
            string[] Files = Directory.GetFiles(path, "*.obj");
            for (int i = 0; i < Files.Length; i++)
            {
                string[] Lines = File.ReadAllLines(path + "/" + i.ToString() + ".obj");
                ModelObject modelObject = new ModelObject();
                List<Mesh> MeshList = new List<Mesh>();
                List<Vector3> vertices = new List<Vector3>();
                List<Vector3> normals = new List<Vector3>();
                List<Vector2> TextureCords = new List<Vector2>();
                Mesh mesh = new Mesh();
                mesh.meshFaces = new List<Faces>();

                //Load File
                for (int a = 0; a < Lines.Length; a++)
                {
                    if (Lines[a].StartsWith("o "))
                    {
                        if (mesh.meshFaces.Count != 0)
                        {
                            MeshList.Add(mesh);
                            mesh = new Mesh();
                            mesh.meshFaces = new List<Faces>();
                        }
                    }

                    if (Lines[a].StartsWith("v "))
                    {
                        string[] splitLine = Lines[a].Split(' ');
                        Vector3 vector3 = new Vector3();
                        vector3.X = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                        vector3.Y = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat);
                        vector3.Z = float.Parse(splitLine[3], CultureInfo.InvariantCulture.NumberFormat);
                        vertices.Add(vector3);
                    }

                    if (Lines[a].StartsWith("vt "))
                    {
                        string[] splitLine = Lines[a].Split(' ');
                        Vector2 vector2 = new Vector2();
                        vector2.X = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                        vector2.Y = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat);
                        TextureCords.Add(vector2);
                    }

                    if (Lines[a].StartsWith("vn "))
                    {
                        string[] splitLine = Lines[a].Split(' ');
                        Vector3 vector3 = new Vector3();
                        vector3.X = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                        vector3.Y = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat);
                        vector3.Z = float.Parse(splitLine[3], CultureInfo.InvariantCulture.NumberFormat);
                        normals.Add(vector3);
                    }

                    if (Lines[a].StartsWith("f "))
                    {
                        string[] splitLine = Lines[a].Split(' ');
                        Faces faces = new Faces();

                        string[] SplitPoint = splitLine[1].Split('/');
                        faces.V1Pos = int.Parse(SplitPoint[0]) - 1;
                        faces.UV1Pos = int.Parse(SplitPoint[1]) - 1;
                        faces.Normal1Pos = int.Parse(SplitPoint[2]) - 1;

                        SplitPoint = splitLine[2].Split('/');
                        faces.V2Pos = int.Parse(SplitPoint[0]) - 1;
                        faces.UV2Pos = int.Parse(SplitPoint[1]) - 1;
                        faces.Normal2Pos = int.Parse(SplitPoint[2]) - 1;

                        SplitPoint = splitLine[3].Split('/');
                        faces.V3Pos = int.Parse(SplitPoint[0]) - 1;
                        faces.UV3Pos = int.Parse(SplitPoint[1]) - 1;
                        faces.Normal3Pos = int.Parse(SplitPoint[2]) - 1;

                        mesh.meshFaces.Add(faces);
                    }
                }
                MeshList.Add(mesh);

                for (int a = 0; a < MeshList.Count; a++)
                {
                    for (int b = 0; b < MeshList[a].meshFaces.Count; b++)
                    {
                        var Face = MeshList[a].meshFaces[b];

                        Face.V1 = vertices[Face.V1Pos];
                        Face.V2 = vertices[Face.V2Pos];
                        Face.V3 = vertices[Face.V3Pos];

                        Face.Normal1 = normals[Face.Normal1Pos];
                        Face.Normal2 = normals[Face.Normal2Pos];
                        Face.Normal3 = normals[Face.Normal3Pos];

                        Face.UV1 = TextureCords[Face.UV1Pos];
                        Face.UV2 = TextureCords[Face.UV2Pos];
                        Face.UV3 = TextureCords[Face.UV3Pos];

                        MeshList[a].meshFaces[b] = Face;
                    }
                }

                modelObject.Meshs = MeshList;
                modelObjects.Add(modelObject);
            }
        }

        public static ModelObject LoadFile(string path)
        {
            string[] Lines = File.ReadAllLines(path);
            ModelObject modelObject = new ModelObject();
            List<Mesh> MeshList = new List<Mesh>();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> TextureCords = new List<Vector2>();
            Mesh mesh = new Mesh();
            mesh.meshFaces = new List<Faces>();
            //Load File
            for (int a = 0; a < Lines.Length; a++)
            {
                //if (Lines[a].StartsWith("o "))
                //{
                //    if (mesh.meshFaces.Count != 0)
                //    {
                //        MeshList.Add(mesh);
                //        mesh = new Mesh();
                //        mesh.meshFaces = new List<Faces>();
                //    }
                //}

                string[] splitLine = Lines[a].Split(' ');
                var Check = splitLine.ToList();
                Check.Remove("");
                splitLine = Check.ToArray();

                if (Lines[a].StartsWith("v "))
                {
                    Vector3 vector3 = new Vector3();
                    vector3.X = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                    vector3.Y = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat);
                    vector3.Z = float.Parse(splitLine[3], CultureInfo.InvariantCulture.NumberFormat);
                    vertices.Add(vector3);
                }

                if (Lines[a].StartsWith("vt "))
                {
                    Vector2 vector2 = new Vector2();
                    vector2.X = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                    vector2.Y = 1-float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat);
                    TextureCords.Add(vector2);
                }

                if (Lines[a].StartsWith("vn "))
                {
                    Vector3 vector3 = new Vector3();
                    vector3.X = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                    vector3.Y = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat);
                    vector3.Z = float.Parse(splitLine[3], CultureInfo.InvariantCulture.NumberFormat);
                    normals.Add(vector3);
                }

                if (Lines[a].StartsWith("f "))
                {
                    Faces faces = new Faces();

                    string[] SplitPoint = splitLine[1].Split('/');
                    faces.V1Pos = int.Parse(SplitPoint[0]) - 1;
                    faces.UV1Pos = int.Parse(SplitPoint[1]) - 1;
                    faces.Normal1Pos = int.Parse(SplitPoint[2]) - 1;

                    SplitPoint = splitLine[2].Split('/');
                    faces.V2Pos = int.Parse(SplitPoint[0]) - 1;
                    faces.UV2Pos = int.Parse(SplitPoint[1]) - 1;
                    faces.Normal2Pos = int.Parse(SplitPoint[2]) - 1;

                    SplitPoint = splitLine[3].Split('/');
                    faces.V3Pos = int.Parse(SplitPoint[0]) - 1;
                    faces.UV3Pos = int.Parse(SplitPoint[1]) - 1;
                    faces.Normal3Pos = int.Parse(SplitPoint[2]) - 1;

                    if (faces.V1Pos != faces.V3Pos && faces.V1Pos != faces.V2Pos && faces.V3Pos != faces.V2Pos)
                    {
                        mesh.meshFaces.Add(faces);
                    }
                }
            }
            MeshList.Add(mesh);

            for (int a = 0; a < MeshList.Count; a++)
            {
                for (int b = 0; b < MeshList[a].meshFaces.Count; b++)
                {
                    var Face = MeshList[a].meshFaces[b];

                    Face.V1 = vertices[Face.V1Pos];
                    Face.V2 = vertices[Face.V2Pos];
                    Face.V3 = vertices[Face.V3Pos];

                    Face.Normal1 = normals[Face.Normal1Pos];
                    Face.Normal2 = normals[Face.Normal2Pos];
                    Face.Normal3 = normals[Face.Normal3Pos];

                    Face.UV1 = TextureCords[Face.UV1Pos];
                    Face.UV2 = TextureCords[Face.UV2Pos];
                    Face.UV3 = TextureCords[Face.UV3Pos];

                    MeshList[a].meshFaces[b] = Face;
                }
            }

            modelObject.Meshs = MeshList;
            return modelObject;

        }

        public struct VectorPoint
        {
            public bool Tristripped;
            public Vector3 vector;
            public Vector3 normal;
            public Vector2 TextureCord;
        }

        public static Mesh GenerateTristripDataOneNew(Mesh modelObject)
        {
            var TempMesh = modelObject;

            if (TempMesh.meshFaces.Count != 0)
            {
                List<VectorPoint> point = new List<VectorPoint>();
                List<TristripGenerator.IndiceFace> faces = new List<TristripGenerator.IndiceFace>();
                for (int i = 0; i < TempMesh.meshFaces.Count; i++)
                {
                    TristripGenerator.IndiceFace TempFace = new TristripGenerator.IndiceFace();
                    bool Test = false;
                    for (int a = 0; a < point.Count; a++)
                    {
                        if (point[a].vector == TempMesh.meshFaces[i].V1)
                        {
                            if (point[a].normal == TempMesh.meshFaces[i].Normal1)
                            {
                                if (point[a].TextureCord == TempMesh.meshFaces[i].UV1)
                                {
                                    Test = true;
                                    TempFace.Id1 = a;
                                    break;
                                }
                            }
                        }
                    }

                    if (!Test)
                    {
                        TempFace.Id1 = point.Count;
                        VectorPoint vectorPoint = new VectorPoint();
                        vectorPoint.vector = TempMesh.meshFaces[i].V1;
                        vectorPoint.normal = TempMesh.meshFaces[i].Normal1;
                        vectorPoint.TextureCord = TempMesh.meshFaces[i].UV1;
                        point.Add(vectorPoint);
                    }

                    Test = false;
                    for (int a = 0; a < point.Count; a++)
                    {
                        if (point[a].vector == TempMesh.meshFaces[i].V2)
                        {
                            if (point[a].normal == TempMesh.meshFaces[i].Normal2)
                            {
                                if (point[a].TextureCord == TempMesh.meshFaces[i].UV2)
                                {
                                    Test = true;
                                    TempFace.Id2 = a;
                                    break;
                                }
                            }
                        }
                    }

                    if (!Test)
                    {
                        TempFace.Id2 = point.Count;
                        VectorPoint vectorPoint = new VectorPoint();
                        vectorPoint.vector = TempMesh.meshFaces[i].V2;
                        vectorPoint.normal = TempMesh.meshFaces[i].Normal2;
                        vectorPoint.TextureCord = TempMesh.meshFaces[i].UV2;
                        point.Add(vectorPoint);
                    }

                    Test = false;
                    for (int a = 0; a < point.Count; a++)
                    {
                        if (point[a].vector == TempMesh.meshFaces[i].V3)
                        {
                            if (point[a].normal == TempMesh.meshFaces[i].Normal3)
                            {
                                if (point[a].TextureCord == TempMesh.meshFaces[i].UV3)
                                {
                                    Test = true;
                                    TempFace.Id3 = a;
                                    break;
                                }
                            }
                        }
                    }

                    if (!Test)
                    {
                        TempFace.Id3 = point.Count;
                        VectorPoint vectorPoint = new VectorPoint();
                        vectorPoint.vector = TempMesh.meshFaces[i].V3;
                        vectorPoint.normal = TempMesh.meshFaces[i].Normal3;
                        vectorPoint.TextureCord = TempMesh.meshFaces[i].UV3;
                        point.Add(vectorPoint);
                    }

                    faces.Add(TempFace);
                }

                List<TristripGenerator.IndiceTristrip> TempStrips = TristripGenerator.GenerateTristripNivda(faces);

                TempMesh.meshChunk = new List<MeshChunk>();

                //Chunk Tristrip
                MeshChunk meshChunk = new MeshChunk();
                meshChunk.vertices = new List<Vector3>();
                meshChunk.normals = new List<Vector3>();
                meshChunk.Tristrip = new List<int>();
                meshChunk.TextureCords = new List<Vector2>();
                for (int i = 0; i < TempStrips.Count; i++)
                {
                    if (meshChunk.vertices.Count + TempStrips[i].Indices.Count <= 50)
                    {
                        meshChunk.Tristrip.Add(TempStrips[i].Indices.Count);
                        for (int a = 0; a < TempStrips[i].Indices.Count; a++)
                        {
                            meshChunk.vertices.Add(point[TempStrips[i].Indices[a]].vector);
                            meshChunk.TextureCords.Add(point[TempStrips[i].Indices[a]].TextureCord);
                            meshChunk.normals.Add(point[TempStrips[i].Indices[a]].normal);
                        }
                    }
                    else
                    {
                        TempMesh.meshChunk.Add(meshChunk);
                        meshChunk = new MeshChunk();
                        meshChunk.vertices = new List<Vector3>();
                        meshChunk.normals = new List<Vector3>();
                        meshChunk.Tristrip = new List<int>();
                        meshChunk.TextureCords = new List<Vector2>();
                        meshChunk.Tristrip.Add(TempStrips[i].Indices.Count);
                        for (int a = 0; a < TempStrips[i].Indices.Count; a++)
                        {
                            meshChunk.vertices.Add(point[TempStrips[i].Indices[a]].vector);
                            meshChunk.TextureCords.Add(point[TempStrips[i].Indices[a]].TextureCord);
                            meshChunk.normals.Add(point[TempStrips[i].Indices[a]].normal);
                        }
                    }
                }

                if (!meshChunk.Equals(new MeshChunk()))
                {
                    TempMesh.meshChunk.Add(meshChunk);
                }
            }

            return TempMesh;
        }

        public static Mesh GenerateFaces(Mesh models)
        {
            models.meshFaces = new List<Faces>();
            for (int i = 0; i < models.meshChunk.Count; i++)
            {
                var ModelData = models.meshChunk[i];
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
                for (int b = 0; b < ModelData.vertices.Count; b++)
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

                    models.meshFaces.Add(CreateFaces(b, ModelData, Rotation));
                    Rotation++;
                    if (Rotation == 2)
                    {
                        Rotation = 0;
                    }
                    localIndex++;
                }
            }
            return models;
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
        public static Faces CreateFaces(int Index, MeshChunk ModelData, int roatation)
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
            face.V1 = ModelData.vertices[Index1];
            face.V2 = ModelData.vertices[Index2];
            face.V3 = ModelData.vertices[Index3];

            face.V1Pos = Index1;
            face.V2Pos = Index2;
            face.V3Pos = Index3;

            if (ModelData.TextureCords.Count != 0)
            {
                face.UV1 = ModelData.TextureCords[Index1];
                face.UV2 = ModelData.TextureCords[Index2];
                face.UV3 = ModelData.TextureCords[Index3];

                face.UV1Pos = Index1;
                face.UV2Pos = Index2;
                face.UV3Pos = Index3;

                face.Normal1 = ModelData.normals[Index1];
                face.Normal2 = ModelData.normals[Index2];
                face.Normal3 = ModelData.normals[Index3];

                face.Normal1Pos = Index1;
                face.Normal2Pos = Index2;
                face.Normal3Pos = Index3;
            }

            return face;
        }
    }

    public struct TristripStruct
    {
        public bool DebugUsed;
        public List<Vector3> vertices;
        public List<Vector3> normals;
        public List<Vector2> TextureCords;
    }

    public struct ModelObject
    {
        public List<Mesh> Meshs;
    }

    public struct Mesh
    {
        public List<Faces> meshFaces;
        public List<MeshChunk> meshChunk;

    }

    public struct MeshChunk
    {
        public int StripCount;
        public int VertexCount;

        public List<int> Tristrip;
        public List<Vector3> vertices;
        public List<Vector3> normals;
        public List<Vector2> TextureCords;
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
