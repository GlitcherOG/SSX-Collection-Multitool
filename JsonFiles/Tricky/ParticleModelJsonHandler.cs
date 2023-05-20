using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

namespace SSXMultiTool.JsonFiles.Tricky
{
    public class ParticleModelJsonHandler
    {
        public List<ParticleModelJson> ParticleModels = new List<ParticleModelJson>();

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
            public List<ParticleObjectHeader> ParticleObjectHeaders;
        }
        [Serializable]
        public struct ParticleObjectHeader
        {
            public ParticleObject ParticleObject;
        }
        [Serializable]
        public struct ParticleObject
        {
            public float[] LowestXYZ;
            public float[] HighestXYZ;
            public int U1;

            public List<AnimationFrames> AnimationFrames;
        }
        [Serializable]
        public struct AnimationFrames
        {
            public float[] Position;
            public float[] Rotation;
            public float Unknown;
        }
    }
}
