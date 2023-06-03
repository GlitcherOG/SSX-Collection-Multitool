using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Newtonsoft.Json;
using System.IO;

namespace SSXMultiTool.JsonFiles.Tricky
{
    [Serializable]
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


        [Serializable]
        public struct InstanceJson
        {
            public string InstanceName;

            public float[] Location;
            public float[] Rotation;
            public float[] Scale; 

            public float[] Unknown5;
            public float[] Unknown6;
            public float[] Unknown7;
            public float[] Unknown8;
            public float[] Unknown9;
            public float[] Unknown10;
            public float[] Unknown11;
            public float[] RGBA;
            public int ModelID;
            public int PrevInstance;
            public int NextInstance;

            public int UnknownInt26;
            public int UnknownInt27;
            public int UnknownInt28;
            public int ModelID2;
            public int UnknownInt30;
            public int UnknownInt31;
            public int UnknownInt32;

            public int LTGState;

            public int Hash;
            public bool IncludeSound;
            public SoundData? Sounds;

            //Object Properties

            public int U1;
            public int U12;
            public int U13;
            public int U14;
            public float PlayerBounce;
            public int U2;
            public int U22;
            public int U23;
            public int U24;

            public int U4;
            public int CollsionMode;
            public string[] CollsionModelPaths;
            public int EffectSlotIndex;
            public int PhysicsIndex;
            public int U8;
        }
        [Serializable]
        public struct SoundData
        {
            public int CollisonSound;
            public List<ExternalSound> ExternalSounds;
        }
        [Serializable]
        public struct ExternalSound
        {
            public int U0;
            public int SoundIndex;
            public float U2;
            public float U3;
            public float U4;
            public float U5; //Radius?
            public float U6;
        }
    }
}
