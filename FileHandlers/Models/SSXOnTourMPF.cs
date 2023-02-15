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
            //Main Header
            public string ModelName;
            public int DataOffset;
            public int EntrySize;

        }
    }
}
