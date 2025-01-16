using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SSXMultiTool.JsonFiles.Tricky
{
    public class SSXTrickyConfig
    {
        public int Game = 2;
        public int Version = 3;
        public string LevelName = "";
        public string Author = "";
        public string LevelVersion = "";
        public string Difficulty = "";
        public string Location = "";
        public string Vertical = "";
        public string Length = "";
        public string Description = "";

        //BuildSettings
        public bool BuildLighting;
        public bool BuildUniLightmap;
        public bool BuildUniInstance;
        public bool BuildPBDGenerate;
        public bool BuildSSHGenerate;
        public bool BuildLSSHGenerate;
        public bool BuildLTGGenerate;
        public bool BuildMAPGenerate;
        public bool BuildSkyPBDGenerate;
        public bool BuildSkySSHGenerate;
        public bool BuildADLGenerate;
        public bool BuildSSFGenerate;
        public bool BuildAIPGenerate;
        public bool BuildSOPGenerate;
        public int BuildLTGGenerateMode;
        public string BuildPath = "";

        //If BBox Is all Zeros it will generate a new one
        public float[,] BBox = new float[2,3];

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

        public static SSXTrickyConfig Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<SSXTrickyConfig>(stream);
                return container;
            }
            else
            {
                return new SSXTrickyConfig();
            }
        }

    }
}
