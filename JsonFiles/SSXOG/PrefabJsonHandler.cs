using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.JsonFiles.SSXOG
{
    public class PrefabJsonHandler
    {
        public List<PrefabJson> Prefabs;
        public void CreateJson(string path, bool Inline = false)
        {
            var TempFormating = Formatting.None;
            if (Inline)
            {
                TempFormating = Formatting.Indented;
            }

            var serializer = JsonConvert.SerializeObject(this, TempFormating,new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(path, serializer);
        }

        public static PrefabJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<PrefabJsonHandler>(stream);
                return container;
            }
            else
            {
                return new PrefabJsonHandler();
            }
        }

        [Serializable]
        public struct PrefabJson
        {
            public int U1;
            public int U2;

            public int U3;

            public int U4;

            public List<ObjectHeader> models;
        }
        [Serializable]
        public struct ObjectHeader
        {
            public string MeshPath;

            public int U10;

            public int U12;
            public int MaterialID;
            public int U14;

            public int U16;

            public MatrixData? matrixData;
        }
        [Serializable]
        public struct MatrixData
        {
            public float[] Location;
            public float[] Rotation;
            public float[] Scale;

            //16
            public int U0;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
            public int U7;

            public List<UStruct0> uStruct0s;
        }
        [Serializable]
        public struct UStruct0
        {
            public List<UStruct1> uStruct1s;
        }
        [Serializable]
        public struct UStruct1
        {
            public float[] vector30; //vector 3
            public float[] vector31; //vector 3
            public int U0;
            public int U1;
        }
    }
}
