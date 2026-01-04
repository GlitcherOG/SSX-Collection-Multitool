using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SSXMultiTool.JsonFiles.SSX3
{
    public class AIPJsonHandler
    {
        public List<AIPath> aiPaths = new List<AIPath>();
        public List<TrackPath> trackPaths = new List<TrackPath>();
        public List<U0Struct> u0Structs = new List<U0Struct>();
        public List<U1Struct> u1Structs = new List<U1Struct>();

        public void CreateJson(string path, bool Inline = false)
        {
            var TempFormating = Formatting.None;
            if (Inline)
            {
                TempFormating = Formatting.Indented;
            }

            var serializer = JsonConvert.SerializeObject(this, TempFormating);
            File.WriteAllText(path, serializer);
        }

        public static Bin0JsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<Bin0JsonHandler>(stream);
                return container;
            }
            else
            {
                return new Bin0JsonHandler();
            }
        }

        public struct AIPath
        {
            public string Name;

            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;

            public float[] PathPos;

            public float[,] PathPoints;
            public List<PathEvent> PathEvents;
        }

        public struct TrackPath
        {
            public string Name;

            public int Type;
            public int U0;
            public int U1;
            public float U2;

            public float[] PathPos;

            public float[,] PathPoints;
            public List<PathEvent> PathEvents;
        }

        public struct PathEvent
        {
            public int EventType;
            public int EventValue;
            public float EventStart;
            public float EventEnd;
        }

        public struct U0Struct
        {
            public int U0;
            public int U1;
        }

        public struct U1Struct
        {
            public int U0;
            public int U1;
            public float[] U2;
            public float[] U3;
            public int U4;
            public int U5;
        }
    }
}
