using System;
using System.Collections.Generic;
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

        public struct UStruct1
        {
            public int ID;
            public int Count;
            public int StartPos;

            public List<int> ints;
        }

        struct struct2
        {

            Vector3 U0;
            Vector3 U1;
            Vector3 U2;
            float U3;
            float U4;
            int U5;
            int U6;
            int U7;
            int U8; //16
            int U9; //16
            int U10; // -1 or FFFFFFFF
            byte[] blank10; //16 bytes
            float U11;
            byte[] U12; //4 bytes
            float U13; // or 2 * UInt16
            byte[] U14; //4 bytes
            int U15; // -1 or FFFFFFFF
            float U16;
            byte[] U17; //16 bytes
            float U18;
        };
    }
}
