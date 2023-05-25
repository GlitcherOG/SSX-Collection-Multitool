using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using System.Windows.Automation;

namespace SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2
{
    public class AIPSOPHandler
    {
        public byte[] MagicBytes;
        public int PathTypeCount;
        public int TypeAOffset;
        public int TypeBOffset;

        public TypeA TypeAStruct;
        public TypeB TypeBStruct;

        public void LoadAIPSOP(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                

            }

        }
        public void SaveAIPSOP(string path)
        {

        }

        public struct TypeA
        {
            public int PathACount;

            public int UCount;
            public List<int> UList;

            List<PathA> PathAs;
        }

        public struct PathA
        {
            public int Type;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;

            public int PointCount;
            public int UCount;

            public Vector3 PathPos;
            public Vector3 BBoxMin;
            public Vector3 BBoxMax;

            public List<Vector4> VectorPoints;
            public List<UnknownStruct> UnknownStructs;
        }

        public struct UnknownStruct
        {
            public int U0;
            public int U1;
            public float U2;
            public float U3;
        }

        public struct TypeB
        {
            public int U0;
            public int ByteSize;

            public int PathBCount;
            public int PathBUnknown;

            public List<PathB> PathBs;
        }

        public struct PathB
        {
            public int Type;
            public int U0;
            public int U1;
            public float U2;

            public int PointCounts;
            public int UCount;

            public Vector3 PathPos;
            public Vector3 BBoxMin;
            public Vector3 BBoxMax;

            public List<Vector4> VectorPoints;
            public List<UnknownStruct> UnknownStructs;
        }
    }
}
