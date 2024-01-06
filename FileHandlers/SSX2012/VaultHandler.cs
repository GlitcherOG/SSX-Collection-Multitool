using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.SSX2012
{
    public class VaultHandler
    {
        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                //Version magic 4 bytes
                //4 bytes int size
                //8 bytes

                //StrN Magic 4 bytes
                //4 bytes int size
                //8 bytes blank

                //DepN Magic 4 Bytes
                //4 bytes int size
                //4 bytes int
                //8 bytes
                //4 bytes int
                //4 bytes int string size
                //vault string
                //bin string
                //allign byte 16

                //DatN magic 4 bytes
                //4 bytes int size
                //8 bytes

                //DatN Struct 128-375, 1616-1863, 1864-1983
                //16 bytes header?








                //ExpN magic 4 bytes
                //4 bytes int size
                //4 bytes int count

                //ExpN Struct
                //8 bytes
                //4 bytes int
                //4 bytes int

                //Allign by 16

                //PtrN Magic 4 Bytes
                //4 bytes int size
                //4 bytes

                //PrtN Struct
                //2 bytes int
                //2 bytes int
                //4 bytes int bin data offset?
                //4 bytes int

                //Allign by 16

                //EndC Magic 4 Bytes
                //4 bytes int size
                //Allign by 16
            }
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
            public int U0;
            public int U1;
            public int Size;
            public int Offset;
        }

        public struct PtrNChunk
        {
            public string MagicHeader;
            public int Size;
            public int U0;

            public List<PtrNStruct> PtrNStructs;
        }

        public struct PtrNStruct
        {
            public int U0;
            public int U1;
            public int U2;
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
