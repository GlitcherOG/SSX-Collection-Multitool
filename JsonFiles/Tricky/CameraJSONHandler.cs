using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.JsonFiles.Tricky
{
    public class CameraJSONHandler
    {
        public List<CameraInstance> Cameras = new List<CameraInstance>();
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

        public static CameraJSONHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<CameraJSONHandler>(stream);
                return container;
            }
            else
            {
                return new CameraJSONHandler();
            }
        }


        [Serializable]
        public struct CameraInstance
        {
            public string CameraName;

            public float[] Translation;
            public float[] Rotation;
            public int Type;
            public float FocalLength;
            public float AspectRatio;
            public float[] Aperture;
            public float[] ClipPlane;
            public float[] IntrestPoint;
            public float[] UpVector;
            public float AnimTime;

            public float[] InitialPosition;
            public float[] InitalRotation;
            public float U0; //Big ?
            public List<CameraAnimationHeader> AnimationHeaders;

            public int Hash;
            //public bool IncludeSound;
            //public SoundData? Sounds;
        }

        public struct CameraAnimationHeader
        {
            public int Action; //Could also be offset

            public List<CameraAnimationData> AnimationDatas;
        }

        public struct CameraAnimationData
        {
            //Probably Wrong I'll figure it out
            public float[] Translation;
            public float[] Rotation;
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
