using SSXMultiTool.Utilities;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Windows;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2.SSBData
{
    public class WorldCollision
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

        public void CollisionObjectSave(string Path, string Name)
        {
            string output = "# Exported From SSX Using SSX Multitool Modder by GlitcherOG \n";

            //Combine Lists
            List<Vector4> Vertices = new List<Vector4>();
            List<Vector4> Normals = new List<Vector4>();
            List<Indexs> indices = new List<Indexs>();
            List<int> ModelIndicesCount = new List<int>();
            List<int> VerticesCount = new List<int>();
            int StartPointI = 0;
            int StartPointV = 0;
            for (int i = 0; i < Models.Count; i++)
            {
                ModelIndicesCount.Add(StartPointI);
                VerticesCount.Add(StartPointV);

                Vertices.AddRange(Models[i].Vectors);
                Normals.AddRange(Models[i].Normals);

                for (global::System.Int32 j = 0; j < Models[i].Indices.Count; j++)
                {
                    var TempIndices = new Indexs();

                    TempIndices.Index1 = Models[i].Indices[j].Index1 + StartPointV;
                    TempIndices.Index2 = Models[i].Indices[j].Index2 + StartPointV;
                    TempIndices.Index3 = Models[i].Indices[j].Index3 + StartPointV;

                    indices.Add(TempIndices);   
                }

                StartPointI += Models[i].Indices.Count;
                StartPointV += Models[i].Vectors.Count;
            }


            for (int i = 0; i < Vertices.Count; i++)
            {
                output += "v " + Vertices[i].X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Vertices[i].Y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Vertices[i].Z.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\n";
            }

            for (int i = 0; i < Normals.Count; i++)
            {
                output += "vn " + Normals[i].X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Normals[i].Y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Normals[i].Z.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\n";
            }


            for (int i = 0; i < indices.Count; i++)
            {
                if(ModelIndicesCount.Contains(i))
                {
                    output += "o Mesh" + ModelIndicesCount.IndexOf(i).ToString() + "\n";
                }

                output += "f " + (indices[i].Index1+1).ToString() + "//" + (i + 1).ToString() + " " + (indices[i].Index2 + 1).ToString() + "//" + (i + 1).ToString() + " " + (indices[i].Index3 + 1).ToString() + "//" + (i + 1).ToString() + "\n";
            }

            File.WriteAllText(Path + "/" + RID.ToString() + "-" + Name + ".obj", output);
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
