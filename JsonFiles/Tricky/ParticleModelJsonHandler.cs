using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.JsonFiles.Tricky
{
    public class ParticleModelJsonHandler
    {
        public List<ParticleModelJson> ParticleModelJsons = new List<ParticleModelJson>();

        public void CreateJson(string path)
        {
            var serializer = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(path, serializer);
        }

        public static ParticleModelJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<ParticleModelJsonHandler>(stream);
                return container;
            }
            else
            {
                return new ParticleModelJsonHandler();
            }
        }

        [Serializable]
        public struct ParticleModelJson
        {
            public string ParticleModelName;

            public int TotalLength;
            public int Unknown0;
            public int Unknown1;
            public int Unknown2;
            public int Unknown3;
            public int Unknown4;
            public int Unknown5;
            public int Unknown6;
            public int Unknown7;
            public int Unknown8;
            public int Unknown9;
            public int Unknown10;
            public int UnknownLenght;
            public byte[] bytes;
        }
    }
}
