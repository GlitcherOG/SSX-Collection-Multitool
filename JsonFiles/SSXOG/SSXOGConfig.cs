using Newtonsoft.Json;
using SSXMultiTool.JsonFiles.Tricky;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.JsonFiles.SSXOG
{
    public class SSXOGConfig
    {
        public int Game = 1;
        public int Version = 1;
        public string LevelName = "";
        public string Author = "";
        public string LevelVersion = "";
        public string Difficulty = "";
        public string Location = "";
        public string Vertical = "";
        public string Length = "";
        public string Description = "";
        public string BuildPath = "";

        //If BBox Is all Zeros it will generate a new one
        public float[,] BBox = new float[2, 3];

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

        public static SSXOGConfig Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<SSXOGConfig>(stream);
                return container;
            }
            else
            {
                return new SSXOGConfig();
            }
        }

    }
}
