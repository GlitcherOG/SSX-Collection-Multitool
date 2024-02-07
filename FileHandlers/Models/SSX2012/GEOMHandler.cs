using SSXMultiTool.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models.SSX2012
{
    public class GEOMHandler
    {
        public string Magic;
        public int U0;
        public int U1;
        public int U2;

        public GEOMStruct geomStruct = new GEOMStruct();
        public VFMTStruct vfmtStruct = new VFMTStruct();
        public STRMStruct strmStruct = new STRMStruct();
        public INDXStruct indxStruct = new INDXStruct();

        public List<Faces> faces = new List<Faces>();

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                Magic = StreamUtil.ReadString(stream, 4);
                U0 = StreamUtil.ReadUInt32(stream);
                U1 = StreamUtil.ReadUInt32(stream);
                U2 = StreamUtil.ReadUInt32(stream);

                geomStruct = new GEOMStruct();
                geomStruct.Magic = StreamUtil.ReadString(stream, 4);
                geomStruct.U0 = StreamUtil.ReadUInt32(stream, true);
                geomStruct.GeomSize = StreamUtil.ReadUInt32(stream, true);
                geomStruct.U2 = StreamUtil.ReadUInt32(stream, true);
                geomStruct.U3 = StreamUtil.ReadUInt32(stream, true);

                vfmtStruct = new VFMTStruct();

                vfmtStruct.VFMTMagic = StreamUtil.ReadString(stream, 4);
                vfmtStruct.CharSize = StreamUtil.ReadUInt32(stream, true);
                vfmtStruct.TextString = StreamUtil.ReadString(stream, vfmtStruct.CharSize + 1);
                vfmtStruct.U0 = StreamUtil.ReadUInt32(stream, true);

                strmStruct = new STRMStruct();
                strmStruct.STRMMagic = StreamUtil.ReadString(stream, 4);
                strmStruct.U0 = StreamUtil.ReadInt8(stream);
                strmStruct.U1 = StreamUtil.ReadUInt32(stream, true);
                strmStruct.NumVertices = StreamUtil.ReadUInt32(stream, true);
                strmStruct.VerticesMode = StreamUtil.ReadUInt32(stream, true);
                strmStruct.U3 = StreamUtil.ReadUInt32(stream, true);

                strmStruct.vectors = new List<Vertices>();

                bool[] Testbools = new bool[32];

                for (int i = 0; i < strmStruct.NumVertices; i++)
                {
                    var Vertice = new Vertices();

                    Vertice.Position = StreamUtil.ReadVector3(stream, true);
                    Vertice.U1 = StreamUtil.ReadUInt32(stream, true);

                    //var TempBytes = StreamUtil.ReadBytes(stream, 4);

                    //Array.Reverse(TempBytes);
                    //var bits = new BitArray(TempBytes);

                    //for (int a = 0; a < bits.Count; a++)
                    //{
                    //    if (bits[a]==true)
                    //    {
                    //        Testbools[a] = true;
                    //    }
                    //}

                    Vertice.Normal.X = StreamUtil.ReadIntCustom(stream, 4, 10, 0, true)/512f;
                    stream.Position -= 4;
                    Vertice.Normal.Y = StreamUtil.ReadIntCustom(stream, 4, 10, 10, true) / 512f;
                    stream.Position -= 4;
                    Vertice.Normal.Z = StreamUtil.ReadIntCustom(stream, 4, 10, 20, true) / 512f;
                    Vertice.TangentNormal.X = StreamUtil.ReadIntCustom(stream, 4, 10, 0, true) / 512f;
                    stream.Position -= 4;
                    Vertice.TangentNormal.Y = StreamUtil.ReadIntCustom(stream, 4, 10, 10, true) / 512f;
                    stream.Position -= 4;
                    Vertice.TangentNormal.Z = StreamUtil.ReadIntCustom(stream, 4, 10, 20, true) / 512f;
                    Vertice.UV.X = StreamUtil.ReadHalfFloat(stream, true);
                    Vertice.UV.Y = 1 - StreamUtil.ReadHalfFloat(stream, true);
                    Vertice.U3 = StreamUtil.ReadUInt32(stream, true);

                    strmStruct.vectors.Add(Vertice);
                }

                strmStruct.U4 = StreamUtil.ReadUInt32(stream, true);

                indxStruct = new INDXStruct();
                indxStruct.INDXMagic = StreamUtil.ReadString(stream, 4);
                indxStruct.U0 = StreamUtil.ReadUInt8(stream);
                indxStruct.U1 = StreamUtil.ReadUInt32(stream, true);
                indxStruct.IndexCount = StreamUtil.ReadUInt32(stream, true);
                indxStruct.IndexMode = StreamUtil.ReadString(stream, 4);

                indxStruct.Indexs = new List<int>();
                if (indxStruct.IndexMode == "ID16")
                {
                    for (int i = 0; i < indxStruct.IndexCount; i++)
                    {
                        indxStruct.Indexs.Add(StreamUtil.ReadUInt16(stream, true));
                    }
                }
                else
                {
                    MessageBox.Show("Unknown Index Mode");
                    return;
                }
                indxStruct.U2 = StreamUtil.ReadUInt32(stream, true);
                indxStruct.U3 = StreamUtil.ReadUInt32(stream, true);
            }
            GenerateFaces();
        }

        public void GenerateFaces()
        {
            faces = new List<Faces>();

            for (int i = 0; i < indxStruct.Indexs.Count/3; i++)
            {
                Faces NewFace = new Faces();

                NewFace.V1 = strmStruct.vectors[indxStruct.Indexs[i * 3]];
                NewFace.V2 = strmStruct.vectors[indxStruct.Indexs[i * 3 + 1]];
                NewFace.V3 = strmStruct.vectors[indxStruct.Indexs[i * 3 + 2]];

                faces.Add(NewFace);
            }
        }

        public void ExportModels(string path)
        {
            string outputString = "";
            string output = "# Exported From SSX Using SSX Multitool Modder by GlitcherOG \n";

            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> Normals = new List<Vector3>();
            List<Vector2> UV = new List<Vector2>();
            outputString += "o Mesh 0\n";
            for (int b = 0; b < faces.Count; b++)
            {
                var Face = faces[b];

                //Vertices
                if (!vertices.Contains(Face.V1.Position))
                {
                    vertices.Add(Face.V1.Position);
                }
                int VPos1 = vertices.IndexOf(Face.V1.Position) + 1;

                if (!vertices.Contains(Face.V2.Position))
                {
                    vertices.Add(Face.V2.Position);
                }
                int VPos2 = vertices.IndexOf(Face.V2.Position) + 1;

                if (!vertices.Contains(Face.V3.Position))
                {
                    vertices.Add(Face.V3.Position);
                }
                int VPos3 = vertices.IndexOf(Face.V3.Position) + 1;

                //UVs
                if (!UV.Contains(Face.V1.UV))
                {
                    UV.Add(Face.V1.UV);
                }
                int UPos1 = UV.IndexOf(Face.V1.UV) + 1;

                if (!UV.Contains(Face.V2.UV))
                {
                    UV.Add(Face.V2.UV);
                }
                int UPos2 = UV.IndexOf(Face.V2.UV) + 1;

                if (!UV.Contains(Face.V3.UV))
                {
                    UV.Add(Face.V3.UV);
                }
                int UPos3 = UV.IndexOf(Face.V3.UV) + 1;

                //Normals
                if (!Normals.Contains(Face.V1.Normal))
                {
                    Normals.Add(Face.V1.Normal);
                }
                int NPos1 = Normals.IndexOf(Face.V1.Normal) + 1;

                if (!Normals.Contains(Face.V2.Normal))
                {
                    Normals.Add(Face.V2.Normal);
                }
                int NPos2 = Normals.IndexOf(Face.V2.Normal) + 1;

                if (!Normals.Contains(Face.V3.Normal))
                {
                    Normals.Add(Face.V3.Normal);
                }
                int NPos3 = Normals.IndexOf(Face.V3.Normal) + 1;

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
            //for (int z = 0; z < Normals.Count; z++)
            //{
            //    output += "vn " + Normals[z].X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Normals[z].Y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Normals[z].Z.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\n";
            //}
            output += outputString;

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.AppendAllText(path, output);
        }

        public struct GEOMStruct
        {
            public string Magic;
            public int U0;
            public int GeomSize;
            public int U2;
            public int U3;
        }

        public struct VFMTStruct
        {
            public string VFMTMagic;
            public int CharSize;
            public string TextString;
            public int U0;
        }

        public struct STRMStruct
        {
            public string STRMMagic;
            public int U0; //8
            public int U1;
            public int NumVertices;
            public int VerticesMode;
            public int U3;

            public List<Vertices> vectors;

            public int U4;
        }
        public struct Vertices
        {
            public Vector3 Position;
            public int U1;
            public Vector3 Normal;
            public Vector3 TangentNormal;
            public Vector2 UV;
            public int U3;
        }

        public struct INDXStruct
        {
            public string INDXMagic;
            public int U0;
            public int U1;
            public int IndexCount;
            public string IndexMode;
            public List<int> Indexs;
            public int U2;
            public int U3;
        }

        public struct MESHStruct
        {
            public string MeshMagic;
            public int U0;
            public string TLST;
            public int U1;
            public int U2;
            public int U3;
            public int U4;

            public int U5;
            public int U6;
            public int U7;
            public int U8;
            public int U9;
            public int U10;

            public string GEOMGEOMagic;
            public int TextSize;
            public string Text;
            public Vector3 vector3;
            public Vector3 vector31;
            public int U11;
            public int U12;
            public int U13;
        }

        public struct Faces
        {
            public Vertices V1;
            public Vertices V2;
            public Vertices V3;
        }
    }
}

/*struct Vector
{
float X;
float Y;
float Z;

u32 U0;

u64 Normal;

u16 XUV;
u16 YUV;
u32 U1;

};


#pragma endian big
le u32 Magic @ $;
le u32 U1 @ $;
le u32 U2 @ $;
le u32 U3 @ $;

le u32 GeomMagic @ $;
le u32 U5 @ $;
u32 GeomSize @ $;
u32 U6 @ $;
u32 U7 @ $;

u32 VFMTMagic @ $;
u32 CharSize @ $;
u8 Text[CharSize+1] @ $;
u32 U8 @ $;

u32 STRMMagic @ $;
u8 U9 @ $;
u32 U10 @ $;
u32 NumVertices @ $;
u32 U11 @ $;
u32 U12 @ $;

Vector Vectors[NumVertices] @ $;
u32 U13 @ $;

u32 INDXMagic @ $;
u8 U14 @ $;
u32 U15 @$;
u32 IndexCount @ $;
u32 IndexMode @ $;
u16 Indexs[IndexCount] @ $;
u32 U16 @$;
u32 U17 @$;

u32 MeshMagic @ $;
u32 U18 @ $;
u32 TLST @ $;
u32 U19 @ $;
u32 U20 @ $;
u32 U21 @ $;
u32 U22 @ $;
u32 U23 @ $;
u32 U24 @ $;
u32 U25 @ $;
u32 U26 @ $;
u32 U27 @ $;
u32 U28 @ $;

u64 GEOMGEOMagic @ $;
u32 CharSize2 @ $;
u8 Text2[CharSize2+1] @ $;

float X @ $;
float Y @ $;
float Z @ $;
float X1 @ $;
float Y2 @ $;
float Z3 @ $;
u32 U29 @ $;
u32 U30 @ $;
le u32 U31 @ $;*/
