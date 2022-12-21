using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models
{
    public class objHandler
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
                        vector3.X = float.Parse(splitLine[1]);
                        vector3.Y = float.Parse(splitLine[2]);
                        vector3.Z = float.Parse(splitLine[3]);
                        vertices.Add(vector3);
                    }

                    if (Lines[a].StartsWith("vt "))
                    {
                        string[] splitLine = Lines[a].Split(' ');
                        Vector2 vector2 = new Vector2();
                        vector2.X = float.Parse(splitLine[1]);
                        vector2.Y = float.Parse(splitLine[2]);
                        TextureCords.Add(vector2);
                    }

                    if (Lines[a].StartsWith("vn "))
                    {
                        string[] splitLine = Lines[a].Split(' ');
                        Vector3 vector3 = new Vector3();
                        vector3.X = float.Parse(splitLine[1]);
                        vector3.Y = float.Parse(splitLine[2]);
                        vector3.Z = float.Parse(splitLine[3]);
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

                if (Lines[a].StartsWith("v "))
                {
                    string[] splitLine = Lines[a].Split(' ');
                    Vector3 vector3 = new Vector3();
                    vector3.X = float.Parse(splitLine[1]);
                    vector3.Y = float.Parse(splitLine[2]);
                    vector3.Z = float.Parse(splitLine[3]);
                    vertices.Add(vector3);
                }

                if (Lines[a].StartsWith("vt "))
                {
                    string[] splitLine = Lines[a].Split(' ');
                    Vector2 vector2 = new Vector2();
                    vector2.X = float.Parse(splitLine[1]);
                    vector2.Y = -float.Parse(splitLine[2]);
                    TextureCords.Add(vector2);
                }

                if (Lines[a].StartsWith("vn "))
                {
                    string[] splitLine = Lines[a].Split(' ');
                    Vector3 vector3 = new Vector3();
                    vector3.X = float.Parse(splitLine[1]);
                    vector3.Y = float.Parse(splitLine[2]);
                    vector3.Z = float.Parse(splitLine[3]);
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

        public void GenerateTristripData()
        {
            for (int x = 0; x < modelObjects.Count; x++)
            {
                for (int a = 0; a < modelObjects[x].Meshs.Count; a++)
                {
                    var TempMesh = modelObjects[x].Meshs[a];
                    List<int> Tristrip = new List<int>();
                    List<Vector3> vertices = new List<Vector3>();
                    List<Vector3> normals = new List<Vector3>();
                    List<Vector2> TextureCords = new List<Vector2>();
                    List<MeshChunk> meshChunks = new List<MeshChunk>();
                    MeshChunk meshChunk = new MeshChunk();

                    int tristripcount = 0;
                    bool rotation = false;
                    bool RunWhile = true;
                    while (RunWhile)
                    {
                        if (tristripcount != 0)
                        {
                            for (int b = 0; b < TempMesh.meshFaces.Count; b++)
                            {
                                var TempFace = TempMesh.meshFaces[b];
                                if (vertices.Count != 72)
                                {
                                    if (!TempFace.tripstriped)
                                    {
                                        int Index = vertices.Count;
                                        int Index2 = 0;
                                        int Index3 = 0;
                                        //Fixes the Rotation For Exporting
                                        //Swap When Exporting to other formats
                                        //1-Clockwise
                                        //0-Counter Clocwise
                                        if (rotation)
                                        {
                                            Index2 = Index - 1;
                                            Index3 = Index - 2;
                                        }
                                        else
                                        {
                                            Index2 = Index - 2;
                                            Index3 = Index - 1;
                                        }
                                        if (TempFace.V2 == vertices[Index3] && TempFace.V3 == vertices[Index2])
                                        {
                                            if (TempFace.Normal2 == normals[Index3] && TempFace.Normal3 == normals[Index2])
                                            {
                                                if (TempFace.UV2 == TextureCords[Index3] && TempFace.UV3 == TextureCords[Index2])
                                                {
                                                    TempFace.tripstriped = true;
                                                    rotation = !rotation;
                                                    vertices.Add(TempFace.V1);
                                                    normals.Add(TempFace.Normal1);
                                                    TextureCords.Add(TempFace.UV1);
                                                    TempMesh.meshFaces[b] = TempFace;
                                                    tristripcount++;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (TempMesh.meshFaces.Count - 1 == b || tristripcount == 32)
                                    {
                                        Tristrip.Add(tristripcount);
                                        tristripcount = 0;
                                        break;
                                    }
                                }
                                else
                                {
                                    Tristrip.Add(tristripcount);
                                    tristripcount = 0;
                                    break;
                                }

                            }
                        }
                        else
                        {
                            if (Tristrip.Count == 16)
                            {
                                meshChunk.TextureCords = TextureCords;
                                meshChunk.vertices = vertices;
                                meshChunk.normals = normals;
                                meshChunk.Tristrip = Tristrip;
                                meshChunks.Add(meshChunk);
                                meshChunk = new MeshChunk();
                                Tristrip = new List<int>();
                                vertices = new List<Vector3>();
                                normals = new List<Vector3>();
                                TextureCords = new List<Vector2>();
                            }
                            for (int b = 0; b < TempMesh.meshFaces.Count; b++)
                            {
                                var TempFace = TempMesh.meshFaces[b];
                                if (!TempFace.tripstriped)
                                {
                                    TempFace.tripstriped = true;
                                    rotation = false;
                                    tristripcount = 3;
                                    vertices.Add(TempFace.V2);
                                    vertices.Add(TempFace.V3);
                                    vertices.Add(TempFace.V1);
                                    normals.Add(TempFace.Normal2);
                                    normals.Add(TempFace.Normal3);
                                    normals.Add(TempFace.Normal1);
                                    TextureCords.Add(TempFace.UV2);
                                    TextureCords.Add(TempFace.UV3);
                                    TextureCords.Add(TempFace.UV1);
                                    TempMesh.meshFaces[b] = TempFace;
                                    break;
                                }

                                if (b == TempMesh.meshFaces.Count - 1)
                                {
                                    meshChunk.TextureCords = TextureCords;
                                    meshChunk.vertices = vertices;
                                    meshChunk.normals = normals;
                                    meshChunk.Tristrip = Tristrip;
                                    meshChunks.Add(meshChunk);
                                    meshChunk = new MeshChunk();
                                    Tristrip = new List<int>();
                                    vertices = new List<Vector3>();
                                    normals = new List<Vector3>();
                                    TextureCords = new List<Vector2>();
                                    RunWhile = false;
                                }
                            }
                        }
                    }
                    TempMesh.meshChunk = meshChunks;
                    modelObjects[x].Meshs[a] = TempMesh;
                }
            }
        }

        public static Mesh GenerateTristripDataOneNew(Mesh modelObject)
        {
            var TempMesh = modelObject;
            List<TristripStruct> tristripStructs = new List<TristripStruct>();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> TextureCords = new List<Vector2>();
            int tristripcount = 0;
            bool rotation = false;
            bool RunWhile = true;

            TristripStruct tri = new TristripStruct();
            if (TempMesh.meshFaces.Count != 0)
            {
                //Tristrip Model
                while (RunWhile)
                {
                    if (tristripcount != 0)
                    {
                        for (int b = 0; b < TempMesh.meshFaces.Count; b++)
                        {
                            var TempFace = TempMesh.meshFaces[b];
                            if (!TempFace.tripstriped)
                            {
                                int Index = vertices.Count;
                                int Index2 = 0;
                                int Index3 = 0;
                                //Fixes the Rotation For Exporting
                                //Swap When Exporting to other formats
                                //1-Clockwise
                                //0-Counter Clocwise
                                if (rotation)
                                {
                                    Index2 = Index - 1;
                                    Index3 = Index - 2;
                                }
                                else
                                {
                                    Index2 = Index - 2;
                                    Index3 = Index - 1;
                                }
                                if (TempFace.V2 == vertices[Index3] && TempFace.V3 == vertices[Index2])
                                {
                                    if (TempFace.Normal2 == normals[Index3] && TempFace.Normal3 == normals[Index2])
                                    {
                                        if (TempFace.UV2 == TextureCords[Index3] && TempFace.UV3 == TextureCords[Index2])
                                        {
                                            TempFace.tripstriped = true;
                                            rotation = !rotation;
                                            vertices.Add(TempFace.V1);
                                            normals.Add(TempFace.Normal1);
                                            TextureCords.Add(TempFace.UV1);
                                            TempMesh.meshFaces[b] = TempFace;
                                            tristripcount++;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (TempMesh.meshFaces.Count - 1 == b)
                            {
                                if (vertices.Count != 0)
                                {
                                    tri.vertices = vertices;
                                    tri.TextureCords = TextureCords;
                                    tri.normals = normals;
                                    vertices = new List<Vector3>();
                                    TextureCords = new List<Vector2>();
                                    normals = new List<Vector3>();
                                    tristripStructs.Add(tri);
                                    tri = new TristripStruct();
                                    tristripcount = 0;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int b = 0; b < TempMesh.meshFaces.Count; b++)
                        {
                            var TempFace = TempMesh.meshFaces[b];
                            if (!TempFace.tripstriped)
                            {
                                TempFace.tripstriped = true;
                                rotation = false;
                                tristripcount = 3;
                                vertices.Add(TempFace.V2);
                                vertices.Add(TempFace.V3);
                                vertices.Add(TempFace.V1);
                                normals.Add(TempFace.Normal2);
                                normals.Add(TempFace.Normal3);
                                normals.Add(TempFace.Normal1);
                                TextureCords.Add(TempFace.UV2);
                                TextureCords.Add(TempFace.UV3);
                                TextureCords.Add(TempFace.UV1);
                                TempMesh.meshFaces[b] = TempFace;
                                break;
                            }

                            if (b == TempMesh.meshFaces.Count - 1)
                            {
                                if (vertices.Count != 0)
                                {
                                    tri.vertices = vertices;
                                    tri.TextureCords = TextureCords;
                                    tri.normals = normals;
                                    vertices = new List<Vector3>();
                                    TextureCords = new List<Vector2>();
                                    normals = new List<Vector3>();
                                    tristripStructs.Add(tri);
                                    tri = new TristripStruct();
                                    tristripcount = 0;
                                }
                                RunWhile = false;
                            }
                        }
                    }
                }

                TempMesh.meshChunk = new List<MeshChunk>();

                //Chunk Tristrip
                MeshChunk meshChunk = new MeshChunk();
                meshChunk.vertices = new List<Vector3>();
                meshChunk.normals = new List<Vector3>();
                meshChunk.Tristrip = new List<int>();
                meshChunk.TextureCords = new List<Vector2>();
                for (int i = 0; i < tristripStructs.Count; i++)
                {
                    if(meshChunk.vertices.Count<=60)
                    {
                        meshChunk.Tristrip.Add(tristripStructs[i].vertices.Count);
                        meshChunk.vertices.AddRange(tristripStructs[i].vertices);
                        meshChunk.TextureCords.AddRange(tristripStructs[i].TextureCords);
                        meshChunk.normals.AddRange(tristripStructs[i].normals);
                    }
                    else if (i > tristripStructs.Count - 3)
                    {
                        meshChunk.Tristrip.Add(tristripStructs[i].vertices.Count);
                        meshChunk.vertices.AddRange(tristripStructs[i].vertices);
                        meshChunk.TextureCords.AddRange(tristripStructs[i].TextureCords);
                        meshChunk.normals.AddRange(tristripStructs[i].normals);
                    }
                    else
                    {
                        TempMesh.meshChunk.Add(meshChunk);
                        meshChunk = new MeshChunk();
                        meshChunk.vertices = new List<Vector3>();
                        meshChunk.normals = new List<Vector3>();
                        meshChunk.Tristrip = new List<int>();
                        meshChunk.TextureCords = new List<Vector2>();
                    }
                }

                if(!meshChunk.Equals(new MeshChunk()))
                {
                    TempMesh.meshChunk.Add(meshChunk);
                }
            }
            return TempMesh;
        }

        public static Mesh GenerateTristripDataOne(Mesh modelObject)
        {
            var TempMesh = modelObject;
            List<int> Tristrip = new List<int>();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> TextureCords = new List<Vector2>();
            List<MeshChunk> meshChunks = new List<MeshChunk>();
            MeshChunk meshChunk = new MeshChunk();

            int tristripcount = 0;
            bool rotation = false;
            bool RunWhile = true;
            if(TempMesh.meshFaces.Count!=0)
            {
                while (RunWhile)
                {
                    if (vertices.Count > 60)
                    {
                        Tristrip.Add(tristripcount);
                        tristripcount = 0;
                        meshChunk.TextureCords = TextureCords;
                        meshChunk.vertices = vertices;
                        meshChunk.normals = normals;
                        meshChunk.Tristrip = Tristrip;
                        meshChunks.Add(meshChunk);
                        meshChunk = new MeshChunk();
                        Tristrip = new List<int>();
                        vertices = new List<Vector3>();
                        normals = new List<Vector3>();
                        TextureCords = new List<Vector2>();
                    }
                    if (tristripcount != 0)
                    {
                        for (int b = 0; b < TempMesh.meshFaces.Count; b++)
                        {
                            var TempFace = TempMesh.meshFaces[b];
                            if (!TempFace.tripstriped)
                            {
                                int Index = vertices.Count;
                                int Index2 = 0;
                                int Index3 = 0;
                                //Fixes the Rotation For Exporting
                                //Swap When Exporting to other formats
                                //1-Clockwise
                                //0-Counter Clocwise
                                if (rotation)
                                {
                                    Index2 = Index - 1;
                                    Index3 = Index - 2;
                                }
                                else
                                {
                                    Index2 = Index - 2;
                                    Index3 = Index - 1;
                                }
                                if (TempFace.V2 == vertices[Index3] && TempFace.V3 == vertices[Index2])
                                {
                                    if (TempFace.Normal2 == normals[Index3] && TempFace.Normal3 == normals[Index2])
                                    {
                                        if (TempFace.UV2 == TextureCords[Index3] && TempFace.UV3 == TextureCords[Index2])
                                        {
                                            TempFace.tripstriped = true;
                                            rotation = !rotation;
                                            vertices.Add(TempFace.V1);
                                            normals.Add(TempFace.Normal1);
                                            TextureCords.Add(TempFace.UV1);
                                            TempMesh.meshFaces[b] = TempFace;
                                            tristripcount++;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (TempMesh.meshFaces.Count - 1 == b)
                            {
                                Tristrip.Add(tristripcount);
                                tristripcount = 0;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (vertices.Count > 60)
                        {
                            meshChunk.TextureCords = TextureCords;
                            meshChunk.vertices = vertices;
                            meshChunk.normals = normals;
                            meshChunk.Tristrip = Tristrip;
                            meshChunks.Add(meshChunk);
                            meshChunk = new MeshChunk();
                            Tristrip = new List<int>();
                            vertices = new List<Vector3>();
                            normals = new List<Vector3>();
                            TextureCords = new List<Vector2>();
                        }
                        for (int b = 0; b < TempMesh.meshFaces.Count; b++)
                        {
                            var TempFace = TempMesh.meshFaces[b];
                            if (!TempFace.tripstriped)
                            {
                                TempFace.tripstriped = true;
                                rotation = false;
                                tristripcount = 3;
                                vertices.Add(TempFace.V2);
                                vertices.Add(TempFace.V3);
                                vertices.Add(TempFace.V1);
                                normals.Add(TempFace.Normal2);
                                normals.Add(TempFace.Normal3);
                                normals.Add(TempFace.Normal1);
                                TextureCords.Add(TempFace.UV2);
                                TextureCords.Add(TempFace.UV3);
                                TextureCords.Add(TempFace.UV1);
                                TempMesh.meshFaces[b] = TempFace;
                                break;
                            }

                            if (b == TempMesh.meshFaces.Count - 1)
                            {
                                meshChunk.TextureCords = TextureCords;
                                meshChunk.vertices = vertices;
                                meshChunk.normals = normals;
                                meshChunk.Tristrip = Tristrip;
                                meshChunks.Add(meshChunk);
                                meshChunk = new MeshChunk();
                                Tristrip = new List<int>();
                                vertices = new List<Vector3>();
                                normals = new List<Vector3>();
                                TextureCords = new List<Vector2>();
                                RunWhile = false;
                            }
                        }
                    }
                }
            }

            TempMesh.meshChunk = meshChunks;
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
