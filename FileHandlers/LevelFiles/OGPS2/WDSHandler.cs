using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SSXMultiTool.FileHandlers.LevelFiles.OGPS2
{
    public class WDSHandler
    {
        public float U0;
        public float U1;
        public float U2;
        public float U3;

        public float U4;
        public int U5;
        public int UStruct2Count;
        public int RowCount;

        public int CollumCount;
        public int U9;
        public int U10;
        public int U11;

        public string FilePath;
        public byte[] Unused; //64 - file path lenght appears to be unused

        public List<UStruct1> uStruct1s;
        public List<UStruct2> uStruct2s;

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                U0 = StreamUtil.ReadFloat(stream);
                U1 = StreamUtil.ReadFloat(stream);
                U2 = StreamUtil.ReadFloat(stream);
                U3 = StreamUtil.ReadFloat(stream);

                U4 = StreamUtil.ReadFloat(stream);
                U5 = StreamUtil.ReadUInt32(stream);
                UStruct2Count = StreamUtil.ReadUInt32(stream);
                RowCount = StreamUtil.ReadUInt32(stream);

                CollumCount = StreamUtil.ReadUInt32(stream);
                U9 = StreamUtil.ReadUInt32(stream);
                U10 = StreamUtil.ReadUInt32(stream);
                U11 = StreamUtil.ReadUInt32(stream);

                long Pos = stream.Position;
                FilePath = StreamUtil.ReadNullEndString(stream);

                long Length = 64 - (stream.Position - Pos);
                Unused = StreamUtil.ReadBytes(stream, (int)Length);

                uStruct1s = new List<UStruct1>();
                for (int i = 0; i < RowCount * CollumCount; i++)
                {
                    UStruct1 TempUstruct1 = new UStruct1();

                    TempUstruct1.ID = StreamUtil.ReadUInt32(stream);
                    TempUstruct1.Count = StreamUtil.ReadUInt32(stream);
                    TempUstruct1.StartPos = StreamUtil.ReadUInt32(stream);

                    TempUstruct1.ints = new List<int>();

                    uStruct1s.Add(TempUstruct1);
                }

                Pos = stream.Position;

                for (int i = 0; i < uStruct1s.Count; i++)
                {
                    var TempUStruct = uStruct1s[i];
                    stream.Position = Pos + (4 * TempUStruct.StartPos);

                    for (int a = 0; a < TempUStruct.Count; a++)
                    {
                        TempUStruct.ints.Add(StreamUtil.ReadUInt32(stream));
                    }

                    uStruct1s[i] = TempUStruct;
                }

                uStruct2s = new List<UStruct2>();

                for (int i = 0; i < UStruct2Count; i++)
                {
                    UStruct2 TempStruct = new UStruct2();

                    TempStruct.U0 = StreamUtil.ReadVector3(stream);
                    TempStruct.U1 = StreamUtil.ReadVector3(stream);
                    TempStruct.U2 = StreamUtil.ReadVector3(stream);
                    TempStruct.U3 = StreamUtil.ReadFloat(stream);
                    TempStruct.U4 = StreamUtil.ReadFloat(stream);
                    TempStruct.U5 = StreamUtil.ReadUInt32(stream);

                    TempStruct.U6 = StreamUtil.ReadUInt32(stream);
                    TempStruct.U7 = StreamUtil.ReadUInt32(stream);

                    TempStruct.U8 = StreamUtil.ReadUInt16(stream);
                    TempStruct.U9 = StreamUtil.ReadUInt16(stream);

                    TempStruct.U10 = StreamUtil.ReadUInt32(stream);

                    TempStruct.U11 = StreamUtil.ReadUInt32(stream);
                    TempStruct.U12 = StreamUtil.ReadUInt32(stream);
                    TempStruct.U13 = StreamUtil.ReadUInt32(stream);
                    TempStruct.U14 = StreamUtil.ReadUInt32(stream);

                    TempStruct.U15 = StreamUtil.ReadFloat(stream);
                    TempStruct.U16 = StreamUtil.ReadUInt32(stream);
                    TempStruct.U17 = StreamUtil.ReadUInt16(stream);
                    TempStruct.U18 = StreamUtil.ReadUInt16(stream);
                    TempStruct.U19 = StreamUtil.ReadUInt32(stream);

                    TempStruct.U20 = StreamUtil.ReadUInt32(stream);
                    TempStruct.U21 = StreamUtil.ReadFloat(stream);
                    TempStruct.U22 = StreamUtil.ReadUInt32(stream);
                    TempStruct.U23 = StreamUtil.ReadUInt32(stream);

                    TempStruct.U24 = StreamUtil.ReadUInt32(stream);
                    TempStruct.U25 = StreamUtil.ReadUInt32(stream);
                    TempStruct.U26 = StreamUtil.ReadFloat(stream);

                    uStruct2s.Add(TempStruct);
                }

            }
        }


        public struct UStruct1
        {
            public int ID;
            public int Count;
            public int StartPos;

            public List<int> ints;
        }

        public struct UStruct2
        {
            public Vector3 U0;
            public Vector3 U1;
            public Vector3 U2;
            public float U3;
            public float U4;
            public int U5;

            public int U6;
            public int U7;
            //16
            public int U8;
            public int U9;
            //32
            public int U10;

            public int U11;
            public int U12;
            public int U13;
            public int U14;

            public float U15;
            public int U16;
            //16?
            public int U17;
            public int U18;
            //32
            public int U19;

            public int U20;
            public float U21;
            public int U22;
            public int U23;

            public int U24;
            public int U25;
            public float U26;
        };
    }
}
