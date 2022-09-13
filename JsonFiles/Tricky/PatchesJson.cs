using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Newtonsoft.Json;

namespace SSXMultiTool.JsonFiles.Tricky
{
    [Serializable]
    public class PatchesJson
    {
        public List<PatchJson> patches = new List<PatchJson>();

        public void CreateJson(string path)
        {
            var serializer = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(path, serializer);
        }

        public static PatchesJson Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<PatchesJson>(stream);
                return container;
            }
            else
            {
                return new PatchesJson();
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

            public Vector4 R4C4;
            public Vector4 R4C3;
            public Vector4 R4C2;
            public Vector4 R4C1;
            public Vector4 R3C4;
            public Vector4 R3C3;
            public Vector4 R3C2;
            public Vector4 R3C1;
            public Vector4 R2C4;
            public Vector4 R2C3;
            public Vector4 R2C2;
            public Vector4 R2C1;
            public Vector4 R1C4;
            public Vector4 R1C3;
            public Vector4 R1C2;
            public Vector4 R1C1;

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
