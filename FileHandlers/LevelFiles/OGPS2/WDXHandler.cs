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
    public class WDXHandler
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
        public WDFGridGroup[,] WDFGridGroups = new WDFGridGroup[1,1];
        public List<UStruct1> uStruct1s = new List<UStruct1>();
        public List<Material> Materials = new List<Material>();
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

                WDFGridGroups = new WDFGridGroup[GridRowCount, GridColumnCount];
                for (int y = 0; y < GridColumnCount; y++)
                {
                    for (int x = 0; x < GridRowCount; x++)
                    {
                        var TempOffset = new WDFGridGroup();
                        TempOffset.Offset = StreamUtil.ReadUInt32(stream);
                        TempOffset.Size = StreamUtil.ReadUInt32(stream);
                        WDFGridGroups[x, y] = TempOffset;
                    }
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

                Materials = new List<Material>();
                for (int i = 0; i < UStruct2Count; i++)
                {
                    var TempMaterial = new Material();

                    TempMaterial.U0 = StreamUtil.ReadUInt32(stream);

                    TempMaterial.TextureID = StreamUtil.ReadUInt16(stream);
                    TempMaterial.U2 = StreamUtil.ReadUInt16(stream);

                    TempMaterial.U3 = StreamUtil.ReadUInt32(stream);

                    Materials.Add(TempMaterial);
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

        public struct Material
        {
            public int U0;
            //16
            public int TextureID;
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
