using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models
{
    public class SSXOnTourMPF
    {
        public byte[] magicWords = new byte[4];
        public int NumModels;
        public int HeaderSize;
        public int DataOffset;
        public void Load(string path)
        {



        }

        public struct MPFHeader
        {
            //Main Header//Offsets
            public string ModelName;
            public int DataOffset;
            public int EntrySize;
            public int U1;
            public int U2;
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

            //Header Counts
            public int UC1;
            public int UC2;
            public int UC3;
            public int UC4;
            public int UC5;
            public int UC6;
            public int UC7;
            public int UC8;
            public int UC9;
            public int UC10;
            public int UC11;
            public int UC12;
            public int UC13;
            public int UC14;
            public int UC15;
            public int UC16;
        }
    }
}
