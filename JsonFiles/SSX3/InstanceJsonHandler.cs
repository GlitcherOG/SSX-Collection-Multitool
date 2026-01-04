using Newtonsoft.Json;
using System.IO;


namespace SSXMultiTool.JsonFiles.SSX3
{
    public class InstanceJsonHandler
    {
        public List<Instance> Instances = new List<Instance>();

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

        public static InstanceJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<InstanceJsonHandler>(stream);
                return container;
            }
            else
            {
                return new InstanceJsonHandler();
            }
        }


        public struct Instance
        {
            public string Name;

            public int TrackID;
            public int RID;

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
            public float U31;

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
            public int U12;
        }
    }
}
