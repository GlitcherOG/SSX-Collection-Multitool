using Newtonsoft.Json;
using SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.JsonFiles.Tricky
{
    public class HashJsonHandler
    {
        public List<HashDataUnknown> LightsHash = new List<HashDataUnknown>();
        public List<HashDataUnknown> CameraHash = new List<HashDataUnknown>();

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

        public static HashJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<HashJsonHandler>(stream);
                return container;
            }
            else
            {
                return new HashJsonHandler();
            }
        }

        [Serializable]
        public struct HashDataUnknown
        {
            public int Hash;
            public int ObjectUID;
        }
    }
}
