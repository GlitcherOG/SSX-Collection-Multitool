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
    public class InstanceJsonHandler
    {
        public List<InstanceJson> instances = new List<InstanceJson>();

        public void CreateJson(string path)
        {
            var serializer = JsonConvert.SerializeObject(this, Formatting.Indented);
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


        [Serializable]
        public struct InstanceJson
        {
            public string InstanceName;

            public Vector4 MatrixCol1;
            public Vector4 MatrixCol2;
            public Vector4 MatrixCol3;
            public Vector4 InstancePosition;
            public Vector4 Unknown5;
            public Vector4 Unknown6;
            public Vector4 Unknown7;
            public Vector4 Unknown8;
            public Vector4 Unknown9;
            public Vector4 Unknown10;
            public Vector4 Unknown11;
            public Vector4 RGBA;
            public int ModelID;
            public int PrevInstance;
            public int NextInstance;

            public Vector3 LowestXYZ;
            public Vector3 HighestXYZ;

            public int UnknownInt26;
            public int UnknownInt27;
            public int UnknownInt28;
            public int ModelID2;
            public int UnknownInt30;
            public int UnknownInt31;
            public int UnknownInt32;
        }
    }
}
