using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.JsonFiles.SSX3
{
    public class Bin11JsonHandler
    {
        public List<Bin11File> bin11Files = new List<Bin11File>();

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

        public static Bin11JsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<Bin11JsonHandler>(stream);
                return container;
            }
            else
            {
                return new Bin11JsonHandler();
            }
        }


        public struct Bin11File
        {
            public float U0;
            public float U1;
            public float U2;
            public float U3;

            public float[] Point4;
            public float[] Point3;
            public float[] Point2;
            public float[] ControlPoint;

            public float U4;
            public float U5;
            public float U6;
            public float U7;

            public float[] BBoxMin;
            public float[] BBoxMax;
        }
    }
}
