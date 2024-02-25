using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SSXMultiTool.JsonFiles.Tricky
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
            public string SplineName;

            public int U0;
            public int U1;
            public int SplineStyle;

            public List<SegmentJson> Segments;

        }

        public struct SegmentJson
        {
            public float[,] Points;
            public float U0;
            public float U1;
            public float U2;
            public float U3;
        }
    }
}
