using SSXMultiTool.JsonFiles.SSX3;
using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static SSXMultiTool.FileHandlers.Models.SSX2012.GEOMHandler;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2.SSBData
{
    public class WorldBin12
    {
        public int TrackID;
        public int RID;

        public int U0;
        public int U0Count;

        public int ModelsOffset;
        public int U1Offset;
        public int U2Offset;

        public List<U0Struct> Models = new List<U0Struct>();


        public void LoadData(Stream stream, int trackID, int rID)
        {
            TrackID = trackID;
            RID = rID;

            U0 = StreamUtil.ReadInt16(stream);
            U0Count = StreamUtil.ReadInt16(stream);

            ModelsOffset = StreamUtil.ReadInt32(stream);
            U1Offset = StreamUtil.ReadInt32(stream);
            U2Offset = StreamUtil.ReadInt32(stream);

            if(U0!=1)
            {
                Console.WriteLine("Bin12 Type " + U0); 
                return;
            }

            Models = new List<U0Struct>();
            for (int i = 0; i < U0Count; i++)
            {
                long Pos = stream.Position;

                var TempU0Struct = new U0Struct();

                TempU0Struct.IndicesCount = StreamUtil.ReadInt16(stream);
                TempU0Struct.VectorsCount = StreamUtil.ReadInt16(stream);

                TempU0Struct.IndicesOffset = StreamUtil.ReadInt32(stream);
                TempU0Struct.BBoxOffset = StreamUtil.ReadInt32(stream);
                TempU0Struct.VectorsOffset = StreamUtil.ReadInt32(stream);
                TempU0Struct.NormalOffset = StreamUtil.ReadInt32(stream);

                TempU0Struct.Indices = new List<Indexs>();

                stream.Position = Pos + TempU0Struct.IndicesOffset;
                for (int j = 0; j < TempU0Struct.IndicesCount; j++)
                {
                    var TempIndex = new Indexs();

                    TempIndex.Index1 = StreamUtil.ReadInt8(stream);
                    TempIndex.Index2 = StreamUtil.ReadInt8(stream);
                    TempIndex.Index3 = StreamUtil.ReadInt8(stream);

                    TempU0Struct.Indices.Add(TempIndex);
                }

                //MathBbox Info
                stream.Position = Pos + TempU0Struct.BBoxOffset;
                TempU0Struct.Bboxes = new List<BBox>();
                int BBoxCount = (TempU0Struct.VectorsOffset - TempU0Struct.BBoxOffset) / 24;
                for (int j = 0; j < BBoxCount; j++)
                {
                    var BBox = new BBox();

                    BBox.BBoxLow = StreamUtil.ReadVector3(stream);
                    BBox.BBoxHigh = StreamUtil.ReadVector3(stream);

                    TempU0Struct.Bboxes.Add(BBox);
                }

                stream.Position = Pos + TempU0Struct.VectorsOffset;
                TempU0Struct.Vectors = new List<Vector4>();

                for (int j = 0; j < TempU0Struct.VectorsCount; j++)
                {
                    TempU0Struct.Vectors.Add(StreamUtil.ReadVector4(stream));
                }

                stream.Position = Pos + TempU0Struct.NormalOffset;
                TempU0Struct.Normals = new List<Vector4>();

                for (int j = 0; j < TempU0Struct.IndicesCount; j++)
                {
                    TempU0Struct.Normals.Add(StreamUtil.ReadVector4(stream));
                }

                Models.Add(TempU0Struct);
            }
        }

        public void CollisionObjectSave(string Path)
        {
            for (int i = 0; i < Models.Count; i++)
            {
                string output = "# Exported From SSX Using SSX Multitool Modder by GlitcherOG \n";

                output += "o Mesh" + i.ToString() + "\n";

                var Data = Models[i];

                for (int z = 0; z < Data.Vectors.Count; z++)
                {
                    output += "v " + Data.Vectors[z].X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Data.Vectors[z].Y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Data.Vectors[z].Z.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\n";
                }
                for (int z = 0; z < Data.Normals.Count; z++)
                {
                    output += "vn " + Data.Normals[z].X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Data.Normals[z].Y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Data.Normals[z].Z.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\n";
                }

                for (int j = 0; j < Data.Indices.Count; j++)
                {
                    output += "f " + Data.Indices[j].Index1.ToString() + "//" + j.ToString() + " " + Data.Indices[j].Index2.ToString() + "//" + j.ToString() + " " + Data.Indices[j].Index3.ToString() + "//" + j.ToString() + "\n";
                }

                if (Models.Count != 0)
                {
                    File.WriteAllText(Path + "/" + RID.ToString() + "-" + i.ToString() + ".obj", output);
                }

            }
        }

        public struct U0Struct
        { 
            public int IndicesCount;
            public int VectorsCount;

            public int IndicesOffset;
            public int BBoxOffset;
            public int VectorsOffset;
            public int NormalOffset;

            public List<Indexs> Indices;
            public List<BBox> Bboxes;
            public List<Vector4> Vectors;
            public List<Vector4> Normals;
        }

        public struct Indexs
        {
            public int Index1;
            public int Index2;
            public int Index3;
        }

        public struct BBox
        {
            public Vector3 BBoxLow;
            public Vector3 BBoxHigh;
        }
    }
}
