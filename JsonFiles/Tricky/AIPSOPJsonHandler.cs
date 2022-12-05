using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.JsonFiles.Tricky
{
    [System.Serializable]
    public class AIPSOPJsonHandler
    {
        public List<PathTypeAJson> PathTypeA = new List<PathTypeAJson>();
        public List<PathTypeBJson> PathTypeB = new List<PathTypeBJson>();
        public void CreateJson(string path)
        {
            var serializer = JsonConvert.SerializeObject(this, Formatting.None);
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
        public struct PathTypeAJson
        {
            public int Unknown1;
            public int Unknown2;
            public int Unknown3;
            public int Unknown4;
            public int Unknown5;
            public int Unknown6;
            public int Unknown7;

            public float[] pathPos;

            public List<float[]> vectorPoints;
            public List<UnkownListTypeAJson> unkownListTypeAs;

        }
        [System.Serializable]
        public struct PathTypeBJson
        {
            public int Unknown1;
            public int Unknown2;
            public int Unknown3;
            public float Unknown4;

            public float[] pathPos;

            public List<float[]> vectorPoints;
            public List<UnkownListTypeAJson> unkownListTypeAs;
        }
        [System.Serializable]
        public struct UnkownListTypeAJson
        {
            public int Unknown1;
            public int Unknown2;
            public float Unknown3;
            public float Unknown4;
        }
    }
}
