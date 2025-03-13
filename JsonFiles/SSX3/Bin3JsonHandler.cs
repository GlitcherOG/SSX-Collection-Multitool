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
    public class Bin3JsonHandler
    {
        public List<Bin3File> bin3Files = new List<Bin3File>();

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


        public struct Bin3File
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;

            public float[] Position;
            public float[] Rotation;
            public float[] Scale;


            public float[] V0;
            public float[] V1;
            public float[] V2;

            public int TrackID;
            public int RID;
            public int U4;

            public int UTrackID;
            public int URID;

            public float U5;
            public int U6;
            public int U7;

            public int U8;
            public int U9;
            public int U10;
            public int U11;
        }
    }
}
