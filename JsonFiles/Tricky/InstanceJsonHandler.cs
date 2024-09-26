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

            public float[] LightVector1;
            public float[] LightVector2;
            public float[] LightVector3;
            public float[] AmbentLightVector;

            public float[] LightColour1;
            public float[] LightColour2;
            public float[] LightColour3;
            public float[] AmbentLightColour;

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

            public float U0;
            public float PlayerBounceAmmount;
            public int U2;
            public bool Visable;
            public bool PlayerCollision;
            public bool PlayerBounce;
            public bool Unknown241;
            public bool UVScroll;

            public int SurfaceType;
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
