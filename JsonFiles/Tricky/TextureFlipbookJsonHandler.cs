using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.JsonFiles.Tricky
{
    public class TextureFlipbookJsonHandler
    {
        public List<FlipbookJson> FlipbookJsons = new List<FlipbookJson>();

        public void CreateJson(string path)
        {
            var serializer = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(path, serializer);
        }

        public static TextureFlipbookJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<TextureFlipbookJsonHandler>(stream);
                return container;
            }
            else
            {
                return new TextureFlipbookJsonHandler();
            }
        }

        [Serializable]
        public struct FlipbookJson
        {
            public int ImageCount;
            public List<int> Images;
        }
    }
}
