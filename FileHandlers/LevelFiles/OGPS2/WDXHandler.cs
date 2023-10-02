using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.LevelFiles.OGPS2
{
    internal class WDXHandler
    {
        //Splines
        public float U0;
        public float U1;
        public int U2;
        public Vector3 U3;
        public Vector3 U4;

        public int GroupSize;
        public int GridRowCount;
        public int GridColumnCount;

        public int ModelCount;
        public int UStruct2Count;
        public int UStruct1Count;
        public int SplineCount;

        //16
        public int U11;
        public int U12;
        public int U13;
        public int U14;

        //Something Here
        public List<ModelOffset> ModelOffsets = new List<ModelOffset>();
        public List<WDFGridGroup> WDFGridGroups = new List<WDFGridGroup>();
        public List<UStruct2> uStruct2s = new List<UStruct2>();
        public List<Spline> Splines = new List<Spline>();

        public struct UStruct1
        {
            public int UsedCount;
            public List<int> U0; //Always 8 Slots
        }

        public struct UStruct2
        {
            public int U0;
            //16
            public int U1;
            public int U2;
            //32
            public int U3;
        }

        public struct Spline
        {
            public Vector3 vector3;
            public Vector3 vector31;

            public int U0;
            public int U1;
            public int U2;

            //16
            public int U3;
            public int U4;
            public int U5;
            public int U6;
            public int U7;
            public int U8;
            public int U9;
            public int U10;
        }

        public struct ModelOffset
        {
            public int Offset;
            public int Size;
        }

        public struct WDFGridGroup
        {
            public int Offset;
            public int Size;
        }
    }
}
