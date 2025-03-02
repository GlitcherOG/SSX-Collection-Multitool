using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Newtonsoft.Json;
using System.IO;

namespace SSXMultiTool.JsonFiles.SSX3
{
    [Serializable]
    public class PatchesJsonHandler
    {
        public List<PatchJson> Patches = new List<PatchJson>();

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

            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;

            public float[] LightMapPoint;
            public float[,] UVPoints;
            public float[,] Points;

            public float[] U7;
            public int U8;
            public int U9;
            public int U10;
            public int U11;

            public int U12;
            public int U13;
            public int U14;
            public int U15;
            public int U16;
            public int U17;
            public int U18;
            public int U19;
        }
    }
}
