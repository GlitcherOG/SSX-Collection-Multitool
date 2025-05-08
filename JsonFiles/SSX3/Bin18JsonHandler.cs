using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2.SSBData;

namespace SSXMultiTool.JsonFiles.SSX3
{
    public class Bin18JsonHandler
    {
        public ObjectID U0;
        public ObjectID U1;
        public ObjectID U2;
        public ObjectID U3;
        public ObjectID U4;
        public ObjectID U5;
        public ObjectID U6;
        public ObjectID U7;
        public ObjectID U8;
        public ObjectID U9;
        public ObjectID U10;
        public ObjectID U11;
        public ObjectID U12;
        public ObjectID U13;

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

        public static Bin18JsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<Bin18JsonHandler>(stream);
                return container;
            }
            else
            {
                return new Bin18JsonHandler();
            }
        }
    }
}
