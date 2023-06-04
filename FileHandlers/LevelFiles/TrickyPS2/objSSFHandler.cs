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
                if (Lines[a].StartsWith("v "))
                {
                    string[] splitLine = Lines[a].Split(' ');
                    Vector4 vector3 = new Vector4();
                    vector3.X = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                    vector3.Y = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat);
                    vector3.Z = float.Parse(splitLine[3], CultureInfo.InvariantCulture.NumberFormat);
                    vector3.Z = 1f;
                    Model.Vertices.Add(vector3);
                }

                if (Lines[a].StartsWith("vn "))
                {
                    string[] splitLine = Lines[a].Split(' ');
                    Vector4 vector3 = new Vector4();
                    vector3.X = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                    vector3.Y = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat);
                    vector3.Z = float.Parse(splitLine[3], CultureInfo.InvariantCulture.NumberFormat);
                    Normals.Add(vector3);
                }

                if (Lines[a].StartsWith("f "))
                {
                    string[] splitLine = Lines[a].Split(' ');
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
                Vector4 NewNormal = new Vector4();

                NewNormal.X = (Normals[TempFace.Normal1Pos].X + Normals[TempFace.Normal2Pos].X + Normals[TempFace.Normal3Pos].X) / 3;
                NewNormal.Y = (Normals[TempFace.Normal1Pos].Y + Normals[TempFace.Normal2Pos].Y + Normals[TempFace.Normal3Pos].Y) / 3;
                NewNormal.X = (Normals[TempFace.Normal1Pos].Z + Normals[TempFace.Normal2Pos].Z + Normals[TempFace.Normal3Pos].Z) / 3;

                Model.FaceNormals.Add(NewNormal);

                Model.Index.Add(TempFace.V1Pos);
                Model.Index.Add(TempFace.V2Pos);
                Model.Index.Add(TempFace.V3Pos);
            }



            return Model;
        }

    }
}
