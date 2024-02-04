using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
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
                vfmtStruct.TextString = StreamUtil.ReadString(stream, vfmtStruct.CharSize+1);
                vfmtStruct.U0 = StreamUtil.ReadUInt32(stream, true);

                strmStruct = new STRMStruct();
                strmStruct.STRMMagic = StreamUtil.ReadString(stream, 4);
                strmStruct.U0 = StreamUtil.ReadInt8(stream);
                strmStruct.U1 = StreamUtil.ReadUInt32(stream, true);
                strmStruct.NumVertices = StreamUtil.ReadUInt32(stream, true);
                strmStruct.VerticesMode = StreamUtil.ReadUInt32(stream, true);
                strmStruct.U3 = StreamUtil.ReadUInt32(stream, true);

                strmStruct.vectors = new List<Vertices>();
                for (int i = 0; i < strmStruct.NumVertices; i++)
                {
                    var Vertice = new Vertices();

                    Vertice.Position = StreamUtil.ReadVector3(stream, true);
                    Vertice.U1 = StreamUtil.ReadUInt32(stream, true);
                    Vertice.Normal.X = StreamUtil.ReadHalfFloat(stream, true);
                    Vertice.Normal.Y = StreamUtil.ReadHalfFloat(stream, true);
                    Vertice.Normal.Z = StreamUtil.ReadHalfFloat(stream, true);
                    Vertice.U2 = StreamUtil.ReadUInt16(stream, true);
                    Vertice.UV.X = StreamUtil.ReadHalfFloat(stream, true);
                    Vertice.UV.Y = StreamUtil.ReadHalfFloat(stream, true);
                    Vertice.U3 = StreamUtil.ReadUInt32(stream, true);

                    strmStruct.vectors.Add(Vertice);
                }

            }
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
        }
        public struct Vertices
        {
            public Vector3 Position;
            public int U1;
            public Vector3 Normal;
            public int U2;
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
