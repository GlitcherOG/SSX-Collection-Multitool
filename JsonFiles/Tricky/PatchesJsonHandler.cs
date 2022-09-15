﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Newtonsoft.Json;

namespace SSXMultiTool.JsonFiles.Tricky
{
    [Serializable]
    public class PatchesJsonHandler
    {
        public List<PatchJson> patches = new List<PatchJson>();

        public void CreateJson(string path)
        {
            var serializer = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(path, serializer);
        }

        public static PatchesJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<PatchesJsonHandler>(stream);
                return container;
            }
            else
            {
                return new PatchesJsonHandler();
            }
        }


        [Serializable]
        public struct PatchJson
        {
            public string PatchName;

            public Vector4 LightMapPoint;

            public Vector4 UVPoint1;
            public Vector4 UVPoint2;
            public Vector4 UVPoint3;
            public Vector4 UVPoint4;

            public float[] R4C4;
            public float[] R4C3;
            public float[] R4C2;
            public float[] R4C1;
            public float[] R3C4;
            public float[] R3C3;
            public float[] R3C2;
            public float[] R3C1;
            public float[] R2C4;
            public float[] R2C3;
            public float[] R2C2;
            public float[] R2C1;
            public float[] R1C4;
            public float[] R1C3;
            public float[] R1C2;
            public float[] R1C1;

            public int PatchStyle;
            public int Unknown2;
            public int TextureAssigment;
            public int LightmapID;
            public int Unknown4;
            public int Unknown5;
            public int Unknown6;
        }
    }
}