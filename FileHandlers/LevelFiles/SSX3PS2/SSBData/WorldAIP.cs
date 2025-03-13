using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2.SSBData
{
    internal class WorldAIP
    {
        public int MagicWords;
        public int NumAIPaths;
        public List<AIPath> aiPaths = new List<AIPath>();

        public int Unk0;
        

        public void LoadData(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream();
            StreamUtil.WriteBytes(stream, bytes);
            stream.Position = 0;


        }

        public struct AIPath
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;

            public int NumPoints;
            public int NumEvents;

            public Vector3 PathPos;
            public Vector3 BBoxMin;
            public Vector3 BBoxMax;

            public List<Vector4> PathPoints;
            public List<PathEvent> PathEvents;
        }
        public struct PathEvent
        {
            public int EventType;
            public int EventValue;
            public float EventStart;
            public float EventEnd;
        }

        public struct TrackPath
        {
            public int Type;
            public int U0;
            public int U1;
            public float U2;

            public int PointCount;
            public int UCount;

            public Vector3 PathPos;
            public Vector3 BBoxMin;
            public Vector3 BBoxMax;

            public List<Vector4> VectorPoints;
            public List<PathEvent> PathEvents;
        }
    }
}
