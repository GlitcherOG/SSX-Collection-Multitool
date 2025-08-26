using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.JsonFiles.SSX3
{
    public class LevelJsonHandler
    {
        public List<LevelInfo> LevelInfoList = new List<LevelInfo>();
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

        public static LevelJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<LevelJsonHandler>(stream);
                return container;
            }
            else
            {
                return new LevelJsonHandler();
            }
        }

        public struct LevelInfo
        {
            public int TrackID;
            public string LevelName;

            public bool Skybox;
            public bool Transport;
        }
    }
}
