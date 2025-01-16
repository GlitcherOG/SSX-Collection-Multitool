using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2
{
    public class objSSFHandler
    {

        public static SSFHandler.CollisonModel LoadModel(SSFHandler.CollisonModel Model, string FolderPath)
        {
            Model.Vertices = new List<Vector4>();
            Model.FaceNormals = new List<Vector4>();
            Model.Index = new List<int>();
            string[] Lines = File.ReadAllLines(FolderPath + Model.MeshPath);

            List<Vector4> Normals = new List<Vector4>();

            List<Faces> Faces = new List<Faces>();

            //Load Model
            for (int a = 0; a < Lines.Length; a++)
            {
                string[] splitLine = Lines[a].Split(' ');
                var Check = splitLine.ToList();
                Check.Remove("");
                splitLine = Check.ToArray();

                if (Lines[a].StartsWith("v "))
                {
                    Vector4 vector3 = new Vector4();
                    vector3.X = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                    vector3.Y = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat);
                    vector3.Z = float.Parse(splitLine[3], CultureInfo.InvariantCulture.NumberFormat);
                    vector3.W = 1f;
                    Model.Vertices.Add(vector3);
                }

                if (Lines[a].StartsWith("vn "))
                {
                    Vector4 vector3 = new Vector4();
                    vector3.X = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                    vector3.Y = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat);
                    vector3.Z = float.Parse(splitLine[3], CultureInfo.InvariantCulture.NumberFormat);
                    Normals.Add(vector3);
                }

                if (Lines[a].StartsWith("f "))
                {
                    Faces faces = new Faces();

                    string[] SplitPoint = splitLine[1].Split('/');
                    faces.V1Pos = int.Parse(SplitPoint[0]) - 1;
                    if (SplitPoint.Length == 3)
                    {
                        //faces.UV1Pos = int.Parse(SplitPoint[1]) - 1;
                        faces.Normal1Pos = int.Parse(SplitPoint[2]) - 1;
                    }
                    else
                    {
                        faces.Normal1Pos = int.Parse(SplitPoint[1]) - 1;
                    }

                    SplitPoint = splitLine[2].Split('/');
                    faces.V2Pos = int.Parse(SplitPoint[0]) - 1;
                    if (SplitPoint.Length == 3)
                    {
                        //faces.UV2Pos = int.Parse(SplitPoint[1]) - 1;
                        faces.Normal2Pos = int.Parse(SplitPoint[2]) - 1;
                    }
                    else
                    {
                        faces.Normal2Pos = int.Parse(SplitPoint[1]) - 1;
                    }

                    SplitPoint = splitLine[3].Split('/');
                    faces.V3Pos = int.Parse(SplitPoint[0]) - 1;
                    if (SplitPoint.Length == 3)
                    {
                        //faces.UV3Pos = int.Parse(SplitPoint[1]) - 1;
                        faces.Normal3Pos = int.Parse(SplitPoint[2]) - 1;
                    }
                    else
                    {
                        faces.Normal3Pos = int.Parse(SplitPoint[1]) - 1;
                    }

                    Faces.Add(faces);
                }
            }

            for (int i = 0; i < Faces.Count; i++)
            {
                var TempFace = Faces[i];

                Model.FaceNormals.Add(Normals[TempFace.Normal1Pos]  /*CalculateFaceNormal(TempFace.V1, TempFace.V2, TempFace.V3)*/);

                Model.Index.Add(TempFace.V1Pos);
                Model.Index.Add(TempFace.V2Pos);
                Model.Index.Add(TempFace.V3Pos);
            }



            return Model;
        }


        public static Vector4 CalculateFaceNormal(Vector3 P1, Vector3 P2, Vector3 P3)
        {
            Vector3 U = P2 - P1;
            Vector3 V = P3 - P1;

            Vector4 Normal = new Vector4();

            var Temp = Vector3.Cross(U, V);

            Temp = Vector3.Normalize(Temp);

            Normal.X = Temp.X;
            Normal.Y = Temp.Y;
            Normal.Z = Temp.Z;

            return Normal;
        }

    }
}
