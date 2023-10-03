using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.LevelFiles.OGPS2
{
    internal class WDXHandler
    {
        //Splines
        public float FormatVersion;
        public float PS2Version;
        public int Revision;
        public Vector3 U3; //Bbox stuff?
        public Vector3 U4; //Bbox stuff?

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

        public List<ModelOffset> ModelOffsets = new List<ModelOffset>();
        public List<WDFGridGroup> WDFGridGroups = new List<WDFGridGroup>();
        public List<UStruct1> uStruct1s = new List<UStruct1>(); //Convert to have 8 slots, appears to be effect slots
        public List<UStruct2> uStruct2s = new List<UStruct2>();
        public List<Spline> Splines = new List<Spline>();

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                FormatVersion = StreamUtil.ReadFloat(stream);
                PS2Version = StreamUtil.ReadFloat(stream);
                Revision = StreamUtil.ReadUInt32(stream);

                U3 = StreamUtil.ReadVector3(stream);
                U4 = StreamUtil.ReadVector3(stream);

                GroupSize = StreamUtil.ReadUInt32(stream);
                GridRowCount = StreamUtil.ReadUInt32(stream);
                GridColumnCount = StreamUtil.ReadUInt32(stream);

                ModelCount = StreamUtil.ReadUInt32(stream);
                UStruct2Count = StreamUtil.ReadUInt32(stream);
                UStruct1Count = StreamUtil.ReadUInt32(stream);
                SplineCount = StreamUtil.ReadUInt32(stream);

                U11 = StreamUtil.ReadUInt16(stream);
                U12 = StreamUtil.ReadUInt16(stream);
                U13 = StreamUtil.ReadUInt16(stream);
                U14 = StreamUtil.ReadUInt16(stream);

                ModelOffsets = new List<ModelOffset>();
                for (int i = 0; i < ModelCount; i++)
                {
                    var TempOffset = new ModelOffset();
                    TempOffset.Offset = StreamUtil.ReadUInt32(stream);
                    TempOffset.Size = StreamUtil.ReadUInt32(stream);
                    ModelOffsets.Add(TempOffset);
                }

                WDFGridGroups = new List<WDFGridGroup>();
                for (int i = 0; i < GridRowCount* GridColumnCount; i++)
                {
                    var TempOffset = new WDFGridGroup();
                    TempOffset.Offset = StreamUtil.ReadUInt32(stream);
                    TempOffset.Size = StreamUtil.ReadUInt32(stream);
                    WDFGridGroups.Add(TempOffset);
                }

                uStruct1s = new List<UStruct1>();
                for (int i = 0; i < UStruct1Count; i++)
                {
                    var TempUStruct1 = new UStruct1();

                    TempUStruct1.UsedCount = StreamUtil.ReadUInt32(stream);

                    TempUStruct1.U0 = StreamUtil.ReadUInt16(stream);
                    TempUStruct1.U1 = StreamUtil.ReadUInt16(stream);
                    TempUStruct1.U2 = StreamUtil.ReadUInt16(stream);
                    TempUStruct1.U3 = StreamUtil.ReadUInt16(stream);
                    TempUStruct1.U4 = StreamUtil.ReadUInt16(stream);
                    TempUStruct1.U5 = StreamUtil.ReadUInt16(stream);
                    TempUStruct1.U6 = StreamUtil.ReadUInt16(stream);
                    TempUStruct1.U7 = StreamUtil.ReadUInt16(stream);

                    uStruct1s.Add(TempUStruct1);
                }

                uStruct2s = new List<UStruct2>();
                for (int i = 0; i < UStruct2Count; i++)
                {
                    var TempUStruct0 = new UStruct2();

                    TempUStruct0.U0 = StreamUtil.ReadUInt32(stream);

                    TempUStruct0.U1 = StreamUtil.ReadUInt16(stream);
                    TempUStruct0.U2 = StreamUtil.ReadUInt16(stream);

                    TempUStruct0.U3 = StreamUtil.ReadUInt32(stream);

                    uStruct2s.Add(TempUStruct0);
                }

                Splines = new List<Spline>();
                for (int i = 0; i < SplineCount; i++)
                {
                    var TempSpline = new Spline();

                    TempSpline.vector3 = StreamUtil.ReadVector3(stream);
                    TempSpline.vector31 = StreamUtil.ReadVector3(stream);

                    TempSpline.U0 = StreamUtil.ReadUInt32(stream);
                    TempSpline.U1 = StreamUtil.ReadUInt32(stream);
                    TempSpline.U2 = StreamUtil.ReadUInt32(stream);

                    TempSpline.U3 = StreamUtil.ReadUInt16(stream);
                    TempSpline.U4 = StreamUtil.ReadUInt16(stream);
                    TempSpline.U5 = StreamUtil.ReadUInt16(stream);
                    TempSpline.U6 = StreamUtil.ReadUInt16(stream);
                    TempSpline.U7 = StreamUtil.ReadUInt16(stream);
                    TempSpline.U8 = StreamUtil.ReadUInt16(stream);
                    TempSpline.U9 = StreamUtil.ReadUInt16(stream);
                    TempSpline.U10 = StreamUtil.ReadUInt16(stream);

                    Splines.Add(TempSpline);
                }
            }
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
        public struct UStruct1
        {
            public int UsedCount; //?

            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
            public int U7;
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
    }
}
