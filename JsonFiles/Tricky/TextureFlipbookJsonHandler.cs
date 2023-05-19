using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SSXMultiTool.JsonFiles.Tricky
{
    public class TextureFlipbookJsonHandler
    {
        public List<FlipbookJson> Flipbooks = new List<FlipbookJson>();

        public void CreateJson(string path)
        {
            var serializer = JsonConvert.SerializeObject(this, Formatting.None);
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
            public List<int> Images;
        }
    }
}
