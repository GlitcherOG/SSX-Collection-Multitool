﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.JsonFiles.SSXOG
{
    public class InstanceJsonHandler
    {
        public List<InstanceJson> Instances = new List<InstanceJson>();

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

        public struct InstanceJson
        {
            public string Name;

            public float[] Location;
            public float[] Rotation;
            public float[] Scale;

            //Colour Data ?
            //public Matrix4x4 matrix4X41;
            //public Vector4 vector40;
            //public Vector4 vector41;
            //public Vector4 vector42;
            //public Vector4 vector43;

            //public int WDFGridID; //16
            //public int InstanceIndex; //16

            public int U2;
            public int U3;
            public int PrefabID;

            public int U5; //16

            public bool Visable;
            public bool PlayerCollision;
            public bool PlayerBounce;

            public float PlayerBounceValue;

            public int CollsionMode; //16
            public string[] CollsionModelPaths;
            public int PhysicsIndex; //16
            public int U11; //16

            public float U12;
            public int U13;

            public int U14;
            public int U15;
            public int U16;
            public int U17;
        }
    }
}
