using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2
{
    internal class SDBHandler
    {
        byte[] UnknownBytes = new byte[8];
        int numLocations; //4
        int numUnknown1; //4
        int numUnknown2; //4
        byte[] UnknownBytes2 = new byte[60];

        public List<Location> locations = new List<Location>();
        public List<Unknown1> Unknown1s = new List<Unknown1>();
        public List<Unknown2> unknown2s = new List<Unknown2>();

        public void LoadSBD(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                UnknownBytes = StreamUtil.ReadBytes(stream, 8);
                numLocations = StreamUtil.ReadInt32(stream);
                numUnknown1 = StreamUtil.ReadInt32(stream);
                numUnknown2 = StreamUtil.ReadInt32(stream);
                UnknownBytes2 = StreamUtil.ReadBytes(stream, 60);

                locations = new List<Location>();
                for (int i = 0; i < numLocations; i++)
                {
                    var TempLocation = new Location();
                    TempLocation.Name = StreamUtil.ReadString(stream, 16);
                    TempLocation.Unknown1 = StreamUtil.ReadInt32(stream);
                    TempLocation.Unknown2 = StreamUtil.ReadInt32(stream);
                    TempLocation.Unknown3 = StreamUtil.ReadInt32(stream);
                    TempLocation.Unknown4 = StreamUtil.ReadInt32(stream);

                    TempLocation.Unknown5 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown6 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown7 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown8 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown9 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown10 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown11 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown12 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown13 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown14 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown15 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown16 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown17 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown18 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown19 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown20 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown21 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown22 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown23 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown24 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown25 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown26 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown27 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown28 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown29 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown30 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown31 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown32 = StreamUtil.ReadInt16(stream);

                    locations.Add(TempLocation);
                }
            }

        }

        //88 Bytes
        public struct Location
        {
            public string Name; //16
                                //Int32s
            public int Unknown1;
            public int Unknown2;
            public int Unknown3; //Chunks
            public int Unknown4;
            //Int16s
            public int Unknown5;
            public int Unknown6;
            public int Unknown7;
            public int Unknown8;
            public int Unknown9;
            public int Unknown10;
            public int Unknown11;
            public int Unknown12;
            public int Unknown13;
            public int Unknown14;
            public int Unknown15;
            public int Unknown16;
            public int Unknown17;
            public int Unknown18;
            public int Unknown19;
            public int Unknown20;
            public int Unknown21;
            public int Unknown22;
            public int Unknown23;
            public int Unknown24;
            public int Unknown25;
            public int Unknown26;
            public int Unknown27;
            public int Unknown28;
            public int Unknown29;
            public int Unknown30;
            public int Unknown31;
            public int Unknown32;
        }

        //96 bytes
        public struct Unknown1
        {
            public float UnknownFloat1;
            public float UnknownFloat2;
            public float UnknownFloat3;
            public float UnknownFloat4;

            public float UnknownFloat5;
            public float UnknownFloat6;
            public float UnknownFloat7;
            public float UnknownFloat8;

            public float UnknownFloat9;
            public float UnknownFloat10;
            public float UnknownFloat11;
            public float UnknownFloat12;

            public float UnknownFloat13;
            public float UnknownFloat14;
            public float UnknownFloat15;
            public float UnknownFloat16;

            public float UnknownFloat17;
            public float UnknownFloat18;
            public float UnknownFloat19;
            public float UnknownFloat20;

            public int UnknownInt1;
            public int UnknownInt2;
            public int UnknownInt3;
            public int UnknownInt4;
        }

        //68 Bytes
        public struct Unknown2
        {
            //Int32
            public int UnknownInt1;
            public int UnknownInt2;
            public int UnknownInt3;
            public int UnknownInt4;
            public int UnknownInt5;
            public int UnknownInt6;
            public int UnknownInt7;
            public int UnknownInt8;
            public int UnknownInt9;
            public int UnknownInt10;
            public int UnknownInt11;
            public int UnknownInt12;
            public int UnknownInt13;
            public int UnknownInt14;
            public int UnknownInt15;
            public int UnknownInt16;
            public int UnknownInt17;
        }
    }
}
