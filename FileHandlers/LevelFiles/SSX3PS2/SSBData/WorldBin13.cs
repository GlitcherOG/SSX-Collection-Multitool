using SSXMultiTool.Utilities;
using System.IO;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2.SSBData
{
    public class WorldBin13
    {
        public int U0;
        public int U1;
        public float U2;
        public int U3;

        public List <U0Struct> u0Structs;

        public void LoadData(Stream stream)
        {
            U0 = StreamUtil.ReadUInt32(stream);
            U1 = StreamUtil.ReadUInt32(stream);
            U2 = StreamUtil.ReadFloat(stream);
            U3 = StreamUtil.ReadUInt32(stream);

            u0Structs = new List<U0Struct>();
            for (int i = 0; i < U3; i++)
            {
                var TempU0Struct = new U0Struct();

                TempU0Struct.U0 = StreamUtil.ReadFloat(stream);
                TempU0Struct.U1 = StreamUtil.ReadFloat(stream);
                TempU0Struct.U2 = StreamUtil.ReadFloat(stream);

                TempU0Struct.U3 = StreamUtil.ReadUInt32(stream);
                TempU0Struct.U4 = StreamUtil.ReadUInt32(stream);
                TempU0Struct.U5 = StreamUtil.ReadUInt32(stream);

                long OldPos= stream.Position;
                stream.Position = TempU0Struct.U4;

                {
                    var TempU1Struct = new U1Struct();
                    TempU1Struct.U0 = StreamUtil.ReadInt32(stream);
                    TempU1Struct.U1 = StreamUtil.ReadInt32(stream);

                    TempU1Struct.U2 = new List<int>();

                    for (int j = 0; j < TempU1Struct.U0; j++)
                    {
                        TempU1Struct.U2.Add(StreamUtil.ReadInt32(stream));
                    }

                    TempU1Struct.u2Struct = new List<U2Struct>();

                    for (int j = 0; j < TempU1Struct.U1; j++)
                    {
                        var TempU2Struct = new U2Struct();

                        TempU2Struct.U0 = StreamUtil.ReadInt32(stream);
                        TempU2Struct.U1 = StreamUtil.ReadInt32(stream);
                        TempU2Struct.U2 = StreamUtil.ReadFloat(stream);
                        TempU2Struct.U3 = StreamUtil.ReadFloat(stream);
                        TempU2Struct.U4 = StreamUtil.ReadFloat(stream);
                        TempU2Struct.U5 = StreamUtil.ReadFloat(stream);
                        TempU2Struct.U6 = StreamUtil.ReadFloat(stream);

                        TempU1Struct.u2Struct.Add(TempU2Struct);
                    }

                    TempU0Struct.u1Struct = TempU1Struct;
                }

                stream.Position = OldPos;

                u0Structs.Add(TempU0Struct);
            }

        }

        public struct U0Struct
        {
            public float U0;
            public float U1;
            public float U2;
            public int U3;
            public int U4;
            public int U5;

            public U1Struct u1Struct;
        }

        public struct U1Struct
        {
            public int U0;
            public int U1;
            public List<int> U2;

            public List<U2Struct> u2Struct;
        }

        public struct U2Struct
        {
            public int U0;
            public int U1;
            public float U2;
            public float U3;
            public float U4;
            public float U5;
            public float U6;
        }

    }
}
