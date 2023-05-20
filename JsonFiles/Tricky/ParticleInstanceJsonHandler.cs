﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;

namespace SSXMultiTool.JsonFiles.Tricky
{
    [Serializable]
    public class ParticleInstanceJsonHandler
    {
        public List<ParticleJson> Particles = new List<ParticleJson>();

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

            public float[] Location;
            public float[] Rotation;
            public float[] Scale;

            public int UnknownInt1;
            public float[] LowestXYZ;
            public float[] HighestXYZ;
            public int UnknownInt8;
            public int UnknownInt9;
            public int UnknownInt10;
            public int UnknownInt11;
            public int UnknownInt12;
        }
    }
}
