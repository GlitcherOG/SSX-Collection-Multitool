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
        public List<SplineJson> SplineJsons = new List<SplineJson>();

        public void CreateJson(string path)
        {
            var serializer = JsonConvert.SerializeObject(this, Formatting.None);
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
            public int Unknown1;
            public int Unknown2;

            public int SegmentCount;

            public List<SegmentJson> Segments;

        }

        public struct SegmentJson
        {
            public float[] Point4;
            public float[] Point3;
            public float[] Point2;
            public float[] Point1;
            public float[] Unknown;

            public int Unknown32;
        }
    }
}
