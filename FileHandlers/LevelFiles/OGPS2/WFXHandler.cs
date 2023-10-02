using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.LevelFiles.OGPS2
{
    internal class WFXHandler
    {
        public float U0;
        public float U1;
        public int UStruct0Count;
        public int UStruct0Offset;
        public int UStruct1Count;
        public int UStruct1Offset;
        public int UStruct2Count;
        public int UStruct2Offset;
        public int UStruct3Count;
        public int UStruct3Offset;

        public List<UStruct0> uStruct0s = new List<UStruct0>();
        public List<UStruct1> uStruct1s = new List<UStruct1>();
        public List<UStruct2> uStruct2s = new List<UStruct2>();

        public struct UStruct0
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
        }

        public struct UStruct1
        {
            public int U0;
            public int U1;
            public int U2;
        }
        public struct UStruct2
        {
            public int U0;
            public int U1;
            public int U2;
        }
    }
}
