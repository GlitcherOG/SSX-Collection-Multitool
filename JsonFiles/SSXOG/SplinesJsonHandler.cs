using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.JsonFiles.SSXOG
{
    public class SplinesJsonHandler
    {
        public List<SplineJson> Splines = new List<SplineJson>();

        public List<SplineSegment> SegmentsData = new List<SplineSegment>();
        public void CreateJson(string path, bool Inline = false)
        {
            var TempFormating = Formatting.None;
            if (Inline)
            {
                TempFormating = Formatting.Indented;
            }

            var serializer = JsonConvert.SerializeObject(this, TempFormating, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(path, serializer);
        }

        public static SplinesJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<SplinesJsonHandler>(stream);
                return container;
            }
            else
            {
                return new SplinesJsonHandler();
            }
        }

        public struct SplineJson
        {
            public string SplineName;

            public int U0;
            public int U1;
            public int U2;

            //16
            public int U3;
            public int U4;
            public int U6;
            public int U9;
            public int U10;

            public List<SplineSegment> Segments;
        }

        public struct SplineSegment
        {
            public float[,] Points;

            public float U0;
            public float U1;
            public float U2;
            public float U3;


            public int U4;

        }
    }
}
