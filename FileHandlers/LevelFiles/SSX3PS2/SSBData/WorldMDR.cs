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
    public class WorldMDR
    {
        public int U0;
        public int U1Count;
        public int U2;
        public int U3;
        public int U4;
        public int U5;

        public float U6;
        public float U7;
        public float U8;
        public float U9;

        public int U10;
        public int U11Count;

        public List<int> U12 = new List<int>();

        public List<UnknownS1> UnknownS1s = new List<UnknownS1>();

        public int U13;

        public void LoadData(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream();
            StreamUtil.WriteBytes(stream, bytes);
            stream.Position = 0;

            U0 = StreamUtil.ReadUInt32(stream);
            U1Count = StreamUtil.ReadUInt32(stream);
            U2 = StreamUtil.ReadUInt32(stream);
            U3 = StreamUtil.ReadUInt32(stream);
            U4 = StreamUtil.ReadUInt32(stream);
            U5 = StreamUtil.ReadUInt32(stream);

            U6 = StreamUtil.ReadFloat(stream);
            U7 = StreamUtil.ReadFloat(stream);
            U8 = StreamUtil.ReadFloat(stream);
            U9 = StreamUtil.ReadFloat(stream);

            U10 = StreamUtil.ReadUInt32(stream);
            U1Count = StreamUtil.ReadUInt32(stream);

            U12 = new List<int>();

            for (int i = 0; i < U11Count; i++)
            {
                U12.Add(StreamUtil.ReadUInt32(stream));
            }

            U13 = StreamUtil.ReadUInt32(stream);

            UnknownS1s = new List<UnknownS1>();

            for (int i = 0; i < U1Count; i++)
            {
                UnknownS1 TempS1 = new UnknownS1();

                TempS1.U1Offset = StreamUtil.ReadUInt32(stream);
                TempS1.U2Offset = StreamUtil.ReadUInt32(stream);
                TempS1.MatrixOffset = StreamUtil.ReadUInt32(stream);
                TempS1.U4 = StreamUtil.ReadUInt32(stream);

                long TempPos = stream.Position;

                if(TempS1.U1Offset >0)
                {
                    stream.Position = TempS1.U1Offset;

                    var TempS2 = new UnknownS2();
                    TempS2.BboxLow = StreamUtil.ReadVector3(stream);
                    TempS2.BboxHigh = StreamUtil.ReadVector3(stream);
                    TempS2.U0 = StreamUtil.ReadUInt32(stream);
                    TempS2.ArrayCount = StreamUtil.ReadUInt32(stream);
                    TempS2.ArrayOffset = StreamUtil.ReadUInt32(stream);

                    TempS1.unknownS2 = TempS2;
                }

                if (TempS1.U2Offset > 0)
                {
                    stream.Position = TempS1.U2Offset;
                }

                if (TempS1.MatrixOffset > 0)
                {
                    stream.Position = TempS1.MatrixOffset;

                    TempS1.matrix4X4 = StreamUtil.ReadMatrix4x4(stream);
                }

                stream.Position = TempPos;

                UnknownS1s.Add(TempS1);
            }

            stream.Close();
        }

        public struct UnknownS1
        {
            public int U1Offset;
            public int U2Offset;
            public int MatrixOffset;
            public int U4;

            public Matrix4x4 matrix4X4;
            public UnknownS2 unknownS2;
            public UnknownS4 unknownS4;
        }

        public struct UnknownS2
        {
            public Vector3 BboxLow;
            public Vector3 BboxHigh;
            public int U0;
            public int ArrayCount;
            public int ArrayOffset;

            public UnknownS5 unknownS5;
        }

        public struct UnknownS3
        {
            public Vector3 U0;
            public Vector3 U2; //?

            public int U3;
            public int U4;
            public int U5;
            public int U6;

            public int U7;
            public int U8;
            public int U9;
            public int U10;

            public int U11;
            public int U12;
            public int U13;
            public int U14;
        }

        public struct UnknownS4
        {
            public int U0;
            public int U1;
        }

        public struct UnknownS5
        {
            public int Offset;
            public UnknownS6 unknownS6;
        }

        public struct UnknownS6
        {
            public int U0;
            public int U1;
            public int U3Offset;
        }
    }
}
