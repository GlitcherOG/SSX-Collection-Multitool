using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.SSX2012
{
    public class InfoDLCHandler
    {
        public string Magic;
        public int ID;
        public List<TextEntries> entries = new List<TextEntries>();
        public int ID2;
        public List<TextEntries> entries2;
        public int ID3;
        public List<TextEntries> entries3;

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                Magic = StreamUtil.ReadString(stream, 8);
                ID = StreamUtil.ReadInt8(stream);

                entries = new List<TextEntries>();
                for (int i = 0; i < length; i++)
                {

                }
            }
        }

        public struct TextEntries
        {
            public int Length;
            public string String;
        }
    }
}
