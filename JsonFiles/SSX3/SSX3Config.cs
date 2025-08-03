using Newtonsoft.Json;
using SSXMultiTool.JsonFiles.Tricky;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.JsonFiles.SSX3
{
    public class SSX3Config
    {
        public int Game = 3;
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

        public static SSX3Config Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<SSX3Config>(stream);
                return container;
            }
            else
            {
                return new SSX3Config();
            }
        }

    }
}
