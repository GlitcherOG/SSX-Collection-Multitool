using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.JsonFiles.Tricky
{
    [Serializable]
    public class LightJsonHandler
    {
        public List<LightJson> LightJsons = new List<LightJson>();

        public void CreateJson(string path)
        {
            var serializer = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(path, serializer);
        }

        public static LightJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<LightJsonHandler>(stream);
                return container;
            }
            else
            {
                return new LightJsonHandler();
            }
        }

        [Serializable]
        public struct LightJson
        {
            public string LightName;

            public int UnknownInt1;
            public int UnknownInt2;
            public int UnknownInt3;
            public int UnknownInt4;
            public int UnknownInt5;
            public int UnknownInt6;
            public int UnknownInt7;
            public int UnknownInt8;
            public int UnknownInt9;
            public int UnknownInt10;
            public int UnknownInt11;
            public int UnknownInt12;
            public int UnknownInt13;
            public int UnknownInt14;
            public int UnknownInt15;
            public int UnknownInt16;
            public int UnknownInt17;
            public int UnknownInt18;
            public int UnknownInt19;
            public int UnknownInt20;
            public int UnknownInt21;
            public int UnknownInt22;
            public int UnknownInt23;
        }
    }
}
