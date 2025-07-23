using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Textures
{
    public class NewSSHHandler
    {
        public byte[] Magic; //4
        public int Size;
        public int ImageCount; //Big 4
        public int U0;
    }
}
