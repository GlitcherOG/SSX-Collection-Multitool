using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SSXMultiTool.JsonFiles.Tricky
{
    [Serializable]
    public class MaterialBlockJsonHandler
    {
        public List<MaterialBlock> MaterialBlockJsons = new List<MaterialBlock>();

        public void CreateJson(string path)
        {
            var serializer = JsonConvert.SerializeObject(this, Formatting.None);
            File.WriteAllText(path, serializer);
        }

        public static MaterialBlockJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<MaterialBlockJsonHandler>(stream);
                return container;
            }
            else
            {
                return new MaterialBlockJsonHandler();
            }
        }

        [Serializable]
        public struct MaterialBlock
        {
            public int BlockCount;
            public List<int> ints; 
        }
    }
}
