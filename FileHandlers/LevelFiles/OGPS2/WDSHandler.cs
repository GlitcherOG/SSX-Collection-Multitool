using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public struct UStruct1
        {
            public int ID;
            public int Count;
            public int StartPos;

            public List<int> ints;
        }
    }
}
