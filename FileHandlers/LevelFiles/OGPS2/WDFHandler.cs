using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace SSXMultiTool.FileHandlers.LevelFiles.OGPS2
{
    public class WDFHandler
    {
        public Vector3 U0;
        public Vector3 U1;
        public int UstructCount2;
        public int PatchesCount;
        public int UstructCount4;
        public int U5;
        public int UstructCount1;
        public int U7;

        public List<UnknownStruct0> unknownStruct0s; //16 on all levels
        public List<UnknownStruct1> unknownStruct1s;
        //Align by 16

        public List<UnknownStruct2> unknownStruct2s;
        public List<Patch> Patches;
        public List<UnknownStruct3> unknownStruct3s;

        public struct UnknownStruct0
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;

            public Vector3 U6;
            public Vector3 U7;
        }

        public struct UnknownStruct1
        {
            public int U0; //16
            public int U1; //16
        }

        //272 bytes
        public struct UnknownStruct2
        {

        }

        //448 bytes
        public struct Patch
        {
            public Vector4 LightMapPoint;

            public Vector4 UVPoint1;
            public Vector4 UVPoint2;
            public Vector4 UVPoint3;
            public Vector4 UVPoint4;

            //public List<Vector4> Points;

            public Vector4 R4C4;
            public Vector4 R4C3;
            public Vector4 R4C2;
            public Vector4 R4C1;
            public Vector4 R3C4;
            public Vector4 R3C3;
            public Vector4 R3C2;
            public Vector4 R3C1;
            public Vector4 R2C4;
            public Vector4 R2C3;
            public Vector4 R2C2;
            public Vector4 R2C1;
            public Vector4 R1C4;
            public Vector4 R1C3;
            public Vector4 R1C2;
            public Vector4 R1C1;

            public int TextureID;
            public int PatchType;
            public int LightmapID;
            public int U0;
        }

        //88
        public struct UnknownStruct3
        {
            public Matrix4x4 matrix4X4;

            //Colour Data ?
            public Matrix4x4 matrix4X41;
            public Vector4 vector40;
            public Vector4 vector41;
            public Vector4 vector42;

            public int U0; //16
            public int U1; //16

            public int U2;
            public int U3;
            public int U4; 

            public Vector3 vector31;
            public Vector3 vector32;

            public int U5; //16
            public int U6; //16

            public float U7;

            public int U8; //16
            public int U9; //16
            public int U10; //16
            public int U11; //16

            public float U12;
            public int U13;

            public int U14;
            public int U15;
            public int U16;
            public int U17;
        }
    }
}
