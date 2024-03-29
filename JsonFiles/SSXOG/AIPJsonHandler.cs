﻿using Newtonsoft.Json;
using System.IO;

namespace SSXMultiTool.JsonFiles.SSXOG
{
    public class AIPJsonHandler
    {
        public int U2;
        public int U3;

        public int U4;
        public int U5;
        public int U6;
        public int U7;

        public int U8;
        public int U9;
        public int U10;

        public List<PathData> PathAs = new List<PathData>();
        public List<PathData> PathBs = new List<PathData>();
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

        public static AIPJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<AIPJsonHandler>(stream);
                return container;
            }
            else
            {
                return new AIPJsonHandler();
            }
        }

        public struct PathData
        {
            public float U0;

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
    }
}
