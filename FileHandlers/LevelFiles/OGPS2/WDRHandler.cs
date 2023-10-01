using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.LevelFiles.OGPS2
{
    internal class WDRHandler
    {
        //Whole thing is chunked
        //Header
        public int U0;
        public int U1;
        public int ModelByteSize;
        public int U2;

        public Vector3 vector3;

        public int U3;
        public int U4;
        public int U5;
        public int U6;
        public int U7;
        public int U8;
        public int U9;
        public Vector3 vector31;
        public Vector3 vector32;

        public int U10;
        public int U11;
        public int U12;
        public int U13;
        public int U14;
        public int U15;
        public int U16;
        public int U17;

        public int U18;
        public int U19;
        public int U20;
        public int U21;

        //Model Data
    }
}
