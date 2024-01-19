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
            public float[] vector3;
            public float[] vector31;

            public int U0;
            public int U1;
            public int U2;

            //16
            public int U3;
            public int U4;
            public int U5;
            public int U6;
            public int WDFChunkID;
            public int SegmentIndex;
            public int U9;
            public int U10;
        }
    }
}
