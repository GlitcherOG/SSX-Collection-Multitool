using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.JsonFiles.SSX3
{
    internal class MDRJsonHandler
    {
        public List<MainModelHeader> mainModelHeaders;

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

        public static MDRJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<MDRJsonHandler>(stream);
                return container;
            }
            else
            {
                return new MDRJsonHandler();
            }
        }

        public struct MainModelHeader
        {
            public int TrackID;
            public int RID;

            public int U3;
            public int U4;

            public float U6;
            public float U7;
            public float U8;
            public float U9;

            public List<int> U12;

            public List<ModelObject> ModelObjects;
        }

        public struct ModelObject
        {
            public int ParentID;

            public float[] Position;
            public float[] Rotation;
            public float[] Scale;

            public UnknownS2 unknownS2;
            public UnknownS3 unknownS3;
        }

        public struct UnknownS2
        {
            public Vector3 BboxLow;
            public Vector3 BboxHigh;
            public int U0;

            public List<ModelDataHeaderStruct> ModelHeaderOffset;
        }

        public struct UnknownS3
        {
            public Vector3 U0;
            public Vector3 U1; //?

            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
        }

        public struct ModelDataHeaderStruct
        {
            public int U0;
            public int U1;
            public int U4;

            //Main Header Object
            public int U00;
            public int U01;
            public int U02;
            public int U03;

            public Vector4 U04;

            public string ModelPath; 
        }

    }
}
