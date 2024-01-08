using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.SSX2012
{
    public class VaultHandler
    {
        public VersChunk versChunk = new VersChunk();
        public StrNChunk strNChunk = new StrNChunk();
        public DepNChunk depNChunk = new DepNChunk();
        public DatNChunk datNChunk = new DatNChunk();
        public ExpNChunk expNChunk = new ExpNChunk();
        public PtrNChunk ptrNChunk = new PtrNChunk();
        public EndCChunk endCChunk = new EndCChunk();

        public List<string> strings = new List<string>(); 

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                versChunk = new VersChunk();
                versChunk.MagicHeader = StreamUtil.ReadString(stream, 4);
                versChunk.Size = StreamUtil.ReadUInt32(stream, true);
                versChunk.Data = StreamUtil.ReadBytes(stream, 8);

                strNChunk = new StrNChunk();
                strNChunk.MagicHeader = StreamUtil.ReadString(stream, 4);
                strNChunk.Size = StreamUtil.ReadUInt32(stream, true);
                strNChunk.Data = StreamUtil.ReadBytes(stream, 8);

                depNChunk = new DepNChunk();
                depNChunk.MagicHeader = StreamUtil.ReadString(stream, 4);
                depNChunk.Size = StreamUtil.ReadUInt32(stream, true);
                depNChunk.FileCount = StreamUtil.ReadUInt32(stream, true);
                depNChunk.FileHash = new List<int>();
                for (int i = 0; i < depNChunk.FileCount; i++)
                {
                    depNChunk.FileHash.Add(StreamUtil.ReadUInt32(stream, true));
                }

                depNChunk.U0 = StreamUtil.ReadUInt32(stream, true);
                depNChunk.StringSize = StreamUtil.ReadUInt32(stream, true);
                depNChunk.FileName = new List<string>();
                for (int i = 0; i < depNChunk.FileCount; i++)
                {
                    depNChunk.FileName.Add(StreamUtil.ReadString(stream, depNChunk.StringSize));
                }

                StreamUtil.AlignBy16(stream);

                datNChunk = new DatNChunk();
                datNChunk.ChunkOffset = (int)stream.Position;
                datNChunk.MagicHeader = StreamUtil.ReadString(stream, 4);
                datNChunk.Size = StreamUtil.ReadUInt32(stream, true);

                stream.Position += datNChunk.Size - 8;


                //DatN magic 4 bytes
                //4 bytes int size
                //8 bytes

                //DatN Struct 128-375, 1616-1863, 1864-1983
                //16 bytes header?

                expNChunk = new ExpNChunk();

                expNChunk.MagicHeader = StreamUtil.ReadString(stream, 4);
                expNChunk.Size = StreamUtil.ReadUInt32(stream, true);
                expNChunk.Count = StreamUtil.ReadUInt32(stream, true);
                expNChunk.ExpNStructs = new List<ExpNStruct>();

                for (int i = 0; i < expNChunk.Count; i++)
                {
                    var NewStruct = new ExpNStruct();

                    NewStruct.ExportID = StreamUtil.ReadUInt32(stream, true);
                    NewStruct.Type = StreamUtil.ReadUInt32(stream, true);
                    NewStruct.Size = StreamUtil.ReadUInt32(stream, true);
                    NewStruct.Offset = StreamUtil.ReadUInt32(stream, true);

                    long TempPos = stream.Position;

                    NewStruct.DatNStructs = new DatNStruct();

                    stream.Position = NewStruct.Offset;

                    NewStruct.DatNStructs.Hash0 = StreamUtil.ReadUInt32(stream, true);
                    if(NewStruct.DatNStructs.Hash1 == -1083517349)
                    {
                        var Temp = BitConverter.GetBytes(NewStruct.DatNStructs.Hash0);
                        Array.Reverse(Temp);
                        strings.Add("0x" + Convert.ToHexString(Temp));

                    }

                    NewStruct.DatNStructs.Hash1 = StreamUtil.ReadUInt32(stream, true);

                    NewStruct.DatNStructs.U0 = StreamUtil.ReadUInt32(stream, true);
                    NewStruct.DatNStructs.UCount1 = StreamUtil.ReadUInt32(stream, true);
                    NewStruct.DatNStructs.U2 = StreamUtil.ReadUInt32(stream, true);
                    NewStruct.DatNStructs.UCount3 = StreamUtil.ReadUInt32(stream, true);
                    NewStruct.DatNStructs.UCount4 = StreamUtil.ReadUInt16(stream, true);
                    NewStruct.DatNStructs.UCount5 = StreamUtil.ReadUInt16(stream, true);
                    NewStruct.DatNStructs.U6 = StreamUtil.ReadUInt32(stream, true);
                    //NewStruct.DatNStructs.U7 = StreamUtil.ReadUInt32(stream, true);

                    NewStruct.DatNStructs.UList4 = new List<int>();

                    for (int a = 0; a < NewStruct.DatNStructs.UCount4; a++)
                    {
                        NewStruct.DatNStructs.UList4.Add(StreamUtil.ReadUInt32(stream, true));
                    }

                    stream.Position += (NewStruct.DatNStructs.UCount4 % 2) * 4;
                    NewStruct.DatNStructs.DatN0Structs = new List<DatN0Struct>();

                    for (int a = 0; a < NewStruct.DatNStructs.UCount1; a++)
                    {
                        var NewStruct1 = new DatN0Struct();

                        NewStruct1.U0 = StreamUtil.ReadUInt32(stream, true);
                        NewStruct1.U1 = StreamUtil.ReadUInt32(stream, true);
                        NewStruct1.U2 = StreamUtil.ReadUInt32(stream, true);

                        NewStruct.DatNStructs.DatN0Structs.Add(NewStruct1);
                    }

                    if (NewStruct.Offset+ NewStruct.Size !=stream.Position)
                    {
                        Console.WriteLine("");
                    }


                    stream.Position = TempPos;

                    expNChunk.ExpNStructs.Add(NewStruct);
                }

                StreamUtil.AlignBy16(stream);

                ptrNChunk = new PtrNChunk();
                ptrNChunk.MagicHeader = StreamUtil.ReadString(stream, 4);
                ptrNChunk.Size = StreamUtil.ReadUInt32(stream, true);
                ptrNChunk.PtrNStructs = new List<PtrNStruct>();
                while (true)
                {
                    PtrNStruct ptrNStruct = new PtrNStruct();

                    ptrNStruct.U0 = StreamUtil.ReadUInt32(stream, true);
                    ptrNStruct.PtrType = StreamUtil.ReadUInt16(stream, true);
                    ptrNStruct.FileIndex = StreamUtil.ReadUInt16(stream, true);
                    ptrNStruct.U3 = StreamUtil.ReadUInt32(stream, true);

                    ptrNChunk.PtrNStructs.Add(ptrNStruct);

                    if (ptrNStruct.PtrType == 0)
                    {
                        break;
                    }
                }

                StreamUtil.AlignBy16(stream);

                endCChunk = new EndCChunk();
                endCChunk.MagicHeader = StreamUtil.ReadString(stream, 4);
                ptrNChunk.Size = StreamUtil.ReadUInt32(stream, true);

                //EndC Magic 4 Bytes
                //4 bytes int size
                //Allign by 16
            }
        }

        public int FNV1(string pData, int nInitialValue)
        {
            byte[] pData8 = Encoding.ASCII.GetBytes(pData);

            for (int i = 0; i < pData8.Length; i++)
            {
                nInitialValue = (nInitialValue * 16777619) ^ pData8[i];
            }

            return nInitialValue;
        }

        public struct VersChunk
        {
            public string MagicHeader;
            public int Size;
            public byte[] Data; //8 bytes
        }

        public struct StrNChunk
        {
            public string MagicHeader;
            public int Size;
            public byte[] Data; //8 bytes (Blank)
        }

        public struct DepNChunk
        {
            public string MagicHeader;
            public int Size;
            public int FileCount;
            public List<int> FileHash;
            public int U0;
            public int StringSize;
            public List<string> FileName;
        }

        public struct DatNChunk
        {
            public int ChunkOffset;

            public string MagicHeader;
            public int Size;
        }

        public struct DatNStruct
        {
            public int Hash0; //might be in the main Header
            public int Hash1; //Hash

            public int U0;
            public int UCount1;
            public int U2;
            public int UCount3;  //Matches U1
            public int UCount4; //16 bytes
            public int UCount5; //16 bytes
            public int U6;
            public int U7; //Probably a value or allign by 8

            public List<int> UList4; //U4 Count 

            public List<DatN0Struct> DatN0Structs; //U1 or U3

            //DatN Struct 128-375, 1616-1863, 1864-1983
            //16 bytes header?
        }

        public struct DatN0Struct
        {
            public int U0;
            public int U1;
            public int U2;
        }


        public struct ExpNChunk
        {
            public string MagicHeader;
            public int Size;
            public int Count;
            public List<ExpNStruct> ExpNStructs;
        }

        public struct ExpNStruct
        {
            public int ExportID; //Export ID
            public int Type; //Type
            public int Size;
            public int Offset;

            public DatNStruct DatNStructs;
        }

        public struct PtrNChunk
        {
            public string MagicHeader;
            public int Size;

            public List<PtrNStruct> PtrNStructs;
        }

        public struct PtrNStruct
        {
            public int U0;
            public int PtrType; //0 - End Pointer, 1 - Null, 2 - PtrSetup Target, 3 - PtrDepRelative, 4 - Export
            public int FileIndex; //0 - Vault, 1 - Bin
            public int U3;
        }

        public struct EndCChunk
        {
            public string MagicHeader;
            public int Size;
            public byte[] Data; //8 bytes (Blank)
        }
    }
}
