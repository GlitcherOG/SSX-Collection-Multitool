using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

namespace SSXMultiTool.JsonFiles.Tricky
{
    [System.Serializable]
    public class AIPSOPJsonHandler
    {
        public List<int> StartPosList = new List<int>();
        public List<PathA> AIPaths = new List<PathA>();
        public List<PathB> RaceLines = new List<PathB>();
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

        public static AIPSOPJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<AIPSOPJsonHandler>(stream);
                return container;
            }
            else
            {
                return new AIPSOPJsonHandler();
            }
        }

        [System.Serializable]
        public struct PathA
        {
            public string Name;

            public int Type;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int Respawnable;

            public float[] PathPos;

            public float[,] PathPoints;
            public List<PathEvent> PathEvents;
        }
        [System.Serializable]
        public struct PathEvent
        {
            public int EventType;
            public int EventValue;
            public float EventStart;
            public float EventEnd;
        }

        [System.Serializable]
        public struct PathB
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
    }
}
