using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SSXMultiTool.JsonFiles.Tricky
{
    [Serializable]
    public class MaterialJsonHandler
    {
        public List<MaterialsJson> MaterialsJsons = new List<MaterialsJson>();

        public void CreateJson(string path)
        {
            var serializer = JsonConvert.SerializeObject(this, Formatting.None);
            File.WriteAllText(path, serializer);
        }

        public static MaterialJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<MaterialJsonHandler>(stream);
                return container;
            }
            else
            {
                return new MaterialJsonHandler();
            }
        }

        [Serializable]
        public struct MaterialsJson
        {
            public string MaterialName;

            public int TextureID;
            public int UnknownInt2;
            public int UnknownInt3;

            public float UnknownFloat1;
            public float UnknownFloat2;
            public float UnknownFloat3;
            public float UnknownFloat4;

            public int UnknownInt8;

            public float UnknownFloat5;
            public float UnknownFloat6;
            public float UnknownFloat7;
            public float UnknownFloat8;

            public int UnknownInt13;
            public int UnknownInt14;
            public int UnknownInt15;
            public int UnknownInt16;
            public int UnknownInt17;
            public int UnknownInt18;

            public int TextureFlipbookID;
            public int UnknownInt20;
        }
    }
}
