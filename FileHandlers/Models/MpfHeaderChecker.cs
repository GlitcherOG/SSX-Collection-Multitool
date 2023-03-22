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

        public static int DetectFileType(string path)
        {
            //3 SSX (2000) PS2
            //8 SSX Tricky PS2
            //13 SSX 3 PS2
            //14 SSX On Tour PS2

            //xDFP SSX 3 Music

            using (Stream stream = File.Open(path, FileMode.Open))
            {
                int Temp = StreamUtil.ReadUInt32(stream);

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


        public static string TypeErrorMessage(int type)
        {

            if(type==0)
            {
                return "Failed To Load SSX (2000) Model File Detected";
            }

            if (type == 1)
            {
                return "Failed To Load SSX Tricky Model File Detected";
            }

            if (type == 2)
            {
                return "Failed To Load SSX 3 Model File Detected";
            }

            if (type == 3)
            {
                return "Failed To Load SSX On Tour Model File Detected";
            }

            if (type == 4)
            {
                return "Failed To Load SSX Music File Detected";
            }

            return "Unknown MPF File Type";
        }
    }

}
