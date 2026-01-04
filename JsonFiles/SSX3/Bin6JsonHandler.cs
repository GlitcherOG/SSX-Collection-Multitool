using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static SSXMultiTool.JsonFiles.SSX3.Bin0JsonHandler;

namespace SSXMultiTool.JsonFiles.SSX3
{
    public class Bin6JsonHandler
    {
        public List<Bin6File> bin6Files = new List<Bin6File>();

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

        public static Bin6JsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<Bin6JsonHandler>(stream);
                return container;
            }
            else
            {
                return new Bin6JsonHandler();
            }
        }


        public struct Bin6File
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;

            public int U4;
            public float U5;
            public float U6;
            public float U7;

            public float U8;
            public float U9;
            public float U10;
            public float U11;

            public int U12;
            public int U13;
            public int U14;
            public int U15;

            public int U16;
            public int U17;
            public int U18;
            public int U19;

            public int U20;
            public int U21;
            public int U22;
            public int U23;

            public int U24;
            public int U25;
            public int U26;
            public int U27;
        }
    }
}
