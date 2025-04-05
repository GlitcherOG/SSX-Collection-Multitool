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
    public class Bin4JsonHandler
    {
        public List<Bin4File> bin3Files = new List<Bin4File>();

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

        public static Bin3JsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<Bin3JsonHandler>(stream);
                return container;
            }
            else
            {
                return new Bin3JsonHandler();
            }
        }


        public struct Bin4File
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;

            public float[] U4;
            public float[] U5;
            public float[] U6;
            public float[] U7;

            public float[] U8;

            public int U9;
            public int U10;
            public float[] U11;
            public float[] U12;

            public int U13;
            public int U14;
            public int U15;
            public int U16;
        }
    }
}
