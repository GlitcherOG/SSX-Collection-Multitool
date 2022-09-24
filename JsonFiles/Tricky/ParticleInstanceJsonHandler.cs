using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SSXMultiTool.JsonFiles.Tricky
{
    [Serializable]
    public class ParticleInstanceJsonHandler
    {
        public List<ParticleJson> particleJsons = new List<ParticleJson>();

        public void CreateJson(string path)
        {
            var serializer = JsonConvert.SerializeObject(this, Formatting.None);
            File.WriteAllText(path, serializer);
        }

        public static ParticleInstanceJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<ParticleInstanceJsonHandler>(stream);
                return container;
            }
            else
            {
                return new ParticleInstanceJsonHandler();
            }
        }

        [Serializable]
        public struct ParticleJson
        {
            public string ParticleName;

            public Vector4 MatrixCol1;
            public Vector4 MatrixCol2;
            public Vector4 MatrixCol3;
            public Vector4 ParticleInstancePosition;
            public int UnknownInt1;
            public Vector3 LowestXYZ;
            public Vector3 HighestXYZ;
            public int UnknownInt8;
            public int UnknownInt9;
            public int UnknownInt10;
            public int UnknownInt11;
            public int UnknownInt12;
        }
    }
}
