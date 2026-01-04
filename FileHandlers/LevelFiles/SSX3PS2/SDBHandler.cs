using SSXMultiTool.Utilities;
using System.IO;
using System.Numerics;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2
{
    internal class SDBHandler
    {
        public byte[] UnknownBytes = new byte[4];
        public float UnknownFloat;
        public int numLocations; //4
        public int numChunks; //4
        public int numSubChunks; //4
        public byte[] UnknownBytes2 = new byte[60];

        public List<Location> locations = new List<Location>();
        public List<ChunkInfo> chunksInfo = new List<ChunkInfo>();
        public List<SubChunkInfo> subChunksInfo = new List<SubChunkInfo>();

        public void LoadSBD(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                UnknownBytes = StreamUtil.ReadBytes(stream, 4);
                UnknownFloat = StreamUtil.ReadFloat(stream);
                numLocations = StreamUtil.ReadUInt32(stream);
                numChunks = StreamUtil.ReadUInt32(stream);
                numSubChunks = StreamUtil.ReadUInt32(stream);
                UnknownBytes2 = StreamUtil.ReadBytes(stream, 60);

                int TempData = 0;

                locations = new List<Location>();
                for (int i = 0; i < numLocations; i++)
                {
                    var TempLocation = new Location();
                    TempLocation.Name = StreamUtil.ReadString(stream, 16);
                    TempLocation.numSubChunks = StreamUtil.ReadUInt32(stream);
                    TempLocation.numChunks = StreamUtil.ReadUInt32(stream);
                    TempLocation.posChunks = StreamUtil.ReadUInt32(stream);
                    TempLocation.posSubChunks = StreamUtil.ReadUInt32(stream);

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
                    TempLocation.Unknown18 = StreamUtil.ReadInt16(stream); //13
                    TempLocation.Unknown19 = StreamUtil.ReadInt16(stream); //14
                    TempLocation.Unknown20 = StreamUtil.ReadInt16(stream); //15
                    TempLocation.Unknown21 = StreamUtil.ReadInt16(stream); //16
                    TempLocation.Unknown22 = StreamUtil.ReadInt16(stream); //17
                    TempLocation.Unknown23 = StreamUtil.ReadInt16(stream); //18
                    TempLocation.Unknown24 = StreamUtil.ReadInt16(stream); //19
                    TempLocation.Unknown25 = StreamUtil.ReadInt16(stream); //20
                    TempLocation.Unknown26 = StreamUtil.ReadInt16(stream); //21
                    TempLocation.Unknown27 = StreamUtil.ReadInt16(stream); //22
                    TempLocation.Unknown28 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown29 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown30 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown31 = StreamUtil.ReadInt16(stream);
                    TempLocation.Unknown32 = StreamUtil.ReadInt16(stream);

                    locations.Add(TempLocation);
                }

                StreamUtil.AlignBy16(stream);
                chunksInfo = new List<ChunkInfo>();
                for (int i = 0; i < numChunks; i++)
                {
                    var TempUnknown1 = new ChunkInfo();
                    TempUnknown1.BboxLow = StreamUtil.ReadVector4(stream);
                    TempUnknown1.BboxHigh = StreamUtil.ReadVector4(stream);
                    TempUnknown1.UnknownFloat9 = StreamUtil.ReadFloat(stream);
                    TempUnknown1.UnknownFloat10 = StreamUtil.ReadFloat(stream);
                    TempUnknown1.UnknownFloat11 = StreamUtil.ReadFloat(stream);
                    TempUnknown1.UnknownFloat12 = StreamUtil.ReadFloat(stream);
                    TempUnknown1.UnknownFloat13 = StreamUtil.ReadFloat(stream);
                    TempUnknown1.UnknownFloat14 = StreamUtil.ReadFloat(stream);
                    TempUnknown1.UnknownFloat15 = StreamUtil.ReadFloat(stream);
                    TempUnknown1.UnknownFloat16 = StreamUtil.ReadFloat(stream);
                    TempUnknown1.UnknownFloat17 = StreamUtil.ReadFloat(stream);
                    TempUnknown1.UnknownFloat18 = StreamUtil.ReadFloat(stream);
                    TempUnknown1.UnknownFloat19 = StreamUtil.ReadFloat(stream);
                    TempUnknown1.UnknownFloat20 = StreamUtil.ReadFloat(stream);

                    TempUnknown1.UnknownInt1 = StreamUtil.ReadUInt32(stream);
                    TempUnknown1.UnknownInt2 = StreamUtil.ReadUInt32(stream);
                    TempUnknown1.UnknownInt3 = StreamUtil.ReadUInt32(stream);
                    TempUnknown1.UnknownInt4 = StreamUtil.ReadUInt32(stream);

                    chunksInfo.Add(TempUnknown1);
                }

                subChunksInfo = new List<SubChunkInfo>();

                for (int i = 0; i < numSubChunks; i++)
                {
                    var TempUnknown2 = new SubChunkInfo();
                    TempUnknown2.numResources = StreamUtil.ReadUInt16(stream);
                    TempUnknown2.subChunkID = StreamUtil.ReadUInt16(stream);
                    TempUnknown2.UnknownInt3 = StreamUtil.ReadUInt16(stream);
                    TempUnknown2.UnknownInt4 = StreamUtil.ReadUInt16(stream);
                    TempUnknown2.UnknownInt5 = StreamUtil.ReadUInt16(stream);
                    TempUnknown2.UnknownInt6 = StreamUtil.ReadUInt16(stream);
                    TempUnknown2.UnknownInt7 = StreamUtil.ReadUInt16(stream); //0
                    TempUnknown2.UnknownInt8 = StreamUtil.ReadUInt16(stream); //1
                    TempUnknown2.UnknownInt9 = StreamUtil.ReadUInt16(stream); //WorldMDR Count - ID 2
                    TempUnknown2.UnknownInt10 = StreamUtil.ReadUInt16(stream); //3
                    TempUnknown2.UnknownInt11 = StreamUtil.ReadUInt16(stream); //4
                    TempUnknown2.UnknownInt12 = StreamUtil.ReadUInt16(stream); //5
                    TempUnknown2.UnknownInt13 = StreamUtil.ReadUInt16(stream); //6
                    TempUnknown2.UnknownInt14 = StreamUtil.ReadUInt16(stream); //7
                    TempUnknown2.UnknownInt15 = StreamUtil.ReadUInt16(stream); //8
                    TempUnknown2.UnknownInt16 = StreamUtil.ReadUInt16(stream); //Shape Count - ID 9
                    TempUnknown2.UnknownInt17 = StreamUtil.ReadUInt16(stream); //10
                    TempUnknown2.UnknownInt18 = StreamUtil.ReadUInt16(stream); //11
                    TempUnknown2.UnknownInt19 = StreamUtil.ReadUInt16(stream); //12

                    TempUnknown2.UnknownInt20 = StreamUtil.ReadUInt16(stream);
                    TempUnknown2.UnusedUnknownInt11 = StreamUtil.ReadUInt32(stream);
                    TempUnknown2.UnusedUnknownInt12 = StreamUtil.ReadUInt32(stream);
                    TempUnknown2.UnusedUnknownInt13 = StreamUtil.ReadUInt32(stream);
                    TempUnknown2.UnusedUnknownInt14 = StreamUtil.ReadUInt32(stream);
                    TempUnknown2.UnusedUnknownInt15 = StreamUtil.ReadUInt32(stream);
                    TempUnknown2.UnusedUnknownInt16 = StreamUtil.ReadUInt32(stream);
                    TempUnknown2.UnusedUnknownInt17 = StreamUtil.ReadUInt32(stream);
                    subChunksInfo.Add(TempUnknown2);
                }
            }

        }

        public int FindLocationChunk(int ID)
        {
            if(locations.Count==1)
            {
                return 0;
            }


            for (int i = 0; i < locations.Count; i++)
            {
                int EndPos = locations[i].posChunks;

                if(i== locations.Count-1)
                {
                    EndPos = chunksInfo.Count;
                }
                else
                {
                    EndPos += locations[i+1].numChunks;
                }

                if (locations[i].posChunks<=ID&& EndPos>ID)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Save(string path)
        {
            //using (Stream stream = File.Open(path, FileMode.Open))
            //{
            //    UnknownBytes = StreamUtil.ReadBytes(stream, 4);
            //    UnknownFloat = StreamUtil.ReadFloat(stream);
            //    numLocations = StreamUtil.ReadUInt32(stream);
            //    numChunks = StreamUtil.ReadUInt32(stream);
            //    numUnknown2 = StreamUtil.ReadUInt32(stream);
            //    UnknownBytes2 = StreamUtil.ReadBytes(stream, 60);

            //    locations = new List<Location>();
            //    for (int i = 0; i < numLocations; i++)
            //    {
            //        var TempLocation = new Location();
            //        TempLocation.Name = StreamUtil.ReadString(stream, 16);
            //        TempLocation.numUnknown2 = StreamUtil.ReadUInt32(stream);
            //        TempLocation.numChunks = StreamUtil.ReadUInt32(stream);
            //        TempLocation.posChunks = StreamUtil.ReadUInt32(stream);
            //        TempLocation.posUnknown2 = StreamUtil.ReadUInt32(stream);

            //        TempLocation.Unknown5 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown6 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown7 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown8 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown9 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown10 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown11 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown12 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown13 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown14 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown15 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown16 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown17 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown18 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown19 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown20 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown21 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown22 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown23 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown24 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown25 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown26 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown27 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown28 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown29 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown30 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown31 = StreamUtil.ReadInt16(stream);
            //        TempLocation.Unknown32 = StreamUtil.ReadInt16(stream);

            //        locations.Add(TempLocation);
            //    }
            //    StreamUtil.AlignBy16(stream);
            //    chunksInfo = new List<ChunkInfo>();
            //    for (int i = 0; i < numChunks; i++)
            //    {
            //        var TempUnknown1 = new ChunkInfo();
            //        TempUnknown1.BboxLow = StreamUtil.ReadVector4(stream);
            //        TempUnknown1.BboxHigh = StreamUtil.ReadVector4(stream);
            //        TempUnknown1.UnknownFloat9 = StreamUtil.ReadFloat(stream);
            //        TempUnknown1.UnknownFloat10 = StreamUtil.ReadFloat(stream);
            //        TempUnknown1.UnknownFloat11 = StreamUtil.ReadFloat(stream);
            //        TempUnknown1.UnknownFloat12 = StreamUtil.ReadFloat(stream);
            //        TempUnknown1.UnknownFloat13 = StreamUtil.ReadFloat(stream);
            //        TempUnknown1.UnknownFloat14 = StreamUtil.ReadFloat(stream);
            //        TempUnknown1.UnknownFloat15 = StreamUtil.ReadFloat(stream);
            //        TempUnknown1.UnknownFloat16 = StreamUtil.ReadFloat(stream);
            //        TempUnknown1.UnknownFloat17 = StreamUtil.ReadFloat(stream);
            //        TempUnknown1.UnknownFloat18 = StreamUtil.ReadFloat(stream);
            //        TempUnknown1.UnknownFloat19 = StreamUtil.ReadFloat(stream);
            //        TempUnknown1.UnknownFloat20 = StreamUtil.ReadFloat(stream);

            //        TempUnknown1.UnknownInt1 = StreamUtil.ReadUInt32(stream);
            //        TempUnknown1.UnknownInt2 = StreamUtil.ReadUInt32(stream);
            //        TempUnknown1.UnknownInt3 = StreamUtil.ReadUInt32(stream);
            //        TempUnknown1.UnknownInt4 = StreamUtil.ReadUInt32(stream);

            //        chunksInfo.Add(TempUnknown1);
            //    }

            //    unknown2s = new List<Unknown2>();

            //    for (int i = 0; i < numUnknown2; i++)
            //    {
            //        var TempUnknown2 = new Unknown2();
            //        TempUnknown2.UnknownInt1 = StreamUtil.ReadUInt32(stream); //Items in SubChunk
            //        TempUnknown2.UnknownInt2 = StreamUtil.ReadUInt32(stream);
            //        TempUnknown2.UnknownInt3 = StreamUtil.ReadUInt32(stream);
            //        TempUnknown2.UnknownInt4 = StreamUtil.ReadUInt32(stream);
            //        TempUnknown2.UnknownInt5 = StreamUtil.ReadUInt32(stream);
            //        TempUnknown2.UnknownInt6 = StreamUtil.ReadUInt32(stream);
            //        TempUnknown2.UnknownInt7 = StreamUtil.ReadUInt32(stream);
            //        TempUnknown2.UnknownInt8 = StreamUtil.ReadUInt32(stream);
            //        TempUnknown2.UnknownInt9 = StreamUtil.ReadUInt32(stream);
            //        TempUnknown2.UnknownInt10 = StreamUtil.ReadUInt32(stream);



            //        TempUnknown2.UnknownInt11 = StreamUtil.ReadUInt32(stream);
            //        TempUnknown2.UnknownInt12 = StreamUtil.ReadUInt32(stream);
            //        TempUnknown2.UnknownInt13 = StreamUtil.ReadUInt32(stream);
            //        TempUnknown2.UnknownInt14 = StreamUtil.ReadUInt32(stream);
            //        TempUnknown2.UnknownInt15 = StreamUtil.ReadUInt32(stream);
            //        TempUnknown2.UnknownInt16 = StreamUtil.ReadUInt32(stream);
            //        TempUnknown2.UnknownInt17 = StreamUtil.ReadUInt32(stream);
            //        unknown2s.Add(TempUnknown2);
            //    }
            //}
        }

        //88 Bytes
        public struct Location
        {
            public string Name; //16
                                //Int32s
            public int numSubChunks;
            public int numChunks;
            public int posChunks; //Chunks
            public int posSubChunks;
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
        public struct ChunkInfo
        {
            public Vector4 BboxLow;
            public Vector4 BboxHigh;

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
        public struct SubChunkInfo
        {
            //Int32
            public int numResources;
            public int subChunkID;
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
            public int UnknownInt18;
            public int UnknownInt19;
            public int UnknownInt20;

            //Doesnt Seem to Change
            public int UnusedUnknownInt11;
            public int UnusedUnknownInt12;
            public int UnusedUnknownInt13;
            public int UnusedUnknownInt14;
            public int UnusedUnknownInt15;
            public int UnusedUnknownInt16;
            public int UnusedUnknownInt17;
        }
    }
}
