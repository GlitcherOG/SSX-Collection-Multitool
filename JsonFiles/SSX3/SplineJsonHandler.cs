using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static SSXMultiTool.JsonFiles.SSX3.Bin0JsonHandler;

namespace SSXMultiTool.JsonFiles.SSX3
{
    public class SplineJsonHandler
    {
        public List<SplineJson> Splines = new List<SplineJson>();

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

        public static SplineJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<SplineJsonHandler>(stream);
                return container;
            }
            else
            {
                return new SplineJsonHandler();
            }
        }


        [Serializable]
        public struct SplineJson
        {
            //public string SplineName;

            public int U0;
            public int U1;
            public float U2;
            public int U3;
            public int U4;

            public List<SegmentJson> Segments;

        }

        public struct SegmentJson
        {
            public float[,] Points;
            public float E0;
            public float E1;
            public float E2;
            public float E3;
        }

    }
}
