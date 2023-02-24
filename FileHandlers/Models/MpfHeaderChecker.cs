using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers.Models
{
    public class MpfHeaderChecker
    {

        public int DetectFileType(string path)
        {
            //3 SSX (2000) PS2
            //8 SSX Tricky PS2
            //13 SSX 3 PS2
            //xDFP SSX 3 Music
            //14 SSX On Tour PS2

            using (Stream stream = File.Open(path, FileMode.Open))
            {
                int Temp = StreamUtil.ReadInt32(stream);

                if(Temp==3)
                {
                    return 0;
                }
                if (Temp == 8)
                {
                    return 1;
                }
                if (Temp == 13)
                {
                    return 2;
                }
                if (Temp == 14)
                {
                    return 3;
                }

                stream.Position = 0;

                string Name = StreamUtil.ReadString(stream, 4);

                if(Name.ToLower() == "xDFP".ToLower())
                {
                    return 4;
                }
            }




                return -1;
        }
    }

}
