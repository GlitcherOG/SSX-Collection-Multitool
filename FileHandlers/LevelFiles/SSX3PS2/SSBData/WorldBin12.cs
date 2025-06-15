using SSXMultiTool.JsonFiles.SSX3;
using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2.SSBData
{
    public class WorldBin12
    {
        public int U0;
        public int U0Count;

        public int U0Offset;
        public int U1Offset;
        public int U2Offset;

        public List<U0Struct> u0Structs = new List<U0Struct>();


        public void LoadData(Stream stream, int TrackID, int RID)
        {
            U0 = StreamUtil.ReadInt16(stream);
            U0Count = StreamUtil.ReadInt16(stream);

            U0Offset = StreamUtil.ReadInt32(stream);
            U1Offset = StreamUtil.ReadInt32(stream);
            U2Offset = StreamUtil.ReadInt32(stream);

            if(U0!=0)
            {
                Console.WriteLine("Bin12 Type " + U0); 
                return;
            }

            u0Structs = new List<U0Struct>();
            for (int i = 0; i < U0Count; i++)
            {
                long Pos = stream.Position;

                var TempU0Struct = new U0Struct();

                TempU0Struct.U0Count = StreamUtil.ReadInt16(stream);
                TempU0Struct.U1Count = StreamUtil.ReadInt16(stream);

                TempU0Struct.U0Offset = StreamUtil.ReadInt32(stream);
                TempU0Struct.U1Offset = StreamUtil.ReadInt32(stream);
                TempU0Struct.U2Offset = StreamUtil.ReadInt32(stream);
                TempU0Struct.U3Offset = StreamUtil.ReadInt32(stream);

                TempU0Struct.indices = new List<Indexs>();

                stream.Position = Pos + TempU0Struct.U0Offset;
                for (int j = 0; j < TempU0Struct.U0Count; j++)
                {
                    var TempIndex = new Indexs();

                    TempIndex.Index1 = StreamUtil.ReadInt8(stream);
                    TempIndex.Index2 = StreamUtil.ReadInt8(stream);
                    TempIndex.Index3 = StreamUtil.ReadInt8(stream);

                    TempU0Struct.indices.Add(TempIndex);
                }

                //MathBbox Info






                stream.Position = Pos + TempU0Struct.U2Offset;
                TempU0Struct.vectors2 = new List<Vector4>();

                for (int j = 0; j < TempU0Struct.U1Count; j++)
                {
                    TempU0Struct.vectors2.Add(StreamUtil.ReadVector4(stream));
                }


                stream.Position = Pos + TempU0Struct.U3Offset;
                TempU0Struct.vectors3 = new List<Vector4>();

                for (int j = 0; j < TempU0Struct.U0Count; j++)
                {
                    TempU0Struct.vectors3.Add(StreamUtil.ReadVector4(stream));
                }

                u0Structs.Add(TempU0Struct);
            }
        }

        public struct U0Struct
        { 
            public int U0Count;
            public int U1Count;

            public int U0Offset;
            public int U1Offset;
            public int U2Offset;
            public int U3Offset;

            public List<Indexs> indices;
            public List<BBox> vectors1;
            public List<Vector4> vectors2;
            public List<Vector4> vectors3;
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
