using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SSXMultiTool.FileHandlers.SSX2012.InfoDLCHandler;

namespace SSXMultiTool.FileHandlers.SSX2012
{
    public class VaultBinHandler
    {
        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                //Version magic 4 bytes
                //4 bytes int size
                //8 bytes

                //StrN Magic 4 bytes
                //4 bytes int size
                //8 bytes blank

                //DepN Magic 4 Bytes
                //4 bytes int size
                //4 bytes int
                //8 bytes
                //4 bytes int
                //4 bytes int string size
                //vault string
                //bin string
                //allign byte 16

                //DatN magic 4 bytes
                //4 bytes int size
                //8 bytes
                
                //DatN Struct 128-375, 1616-1863, 1864-1983
                //16 bytes header?








                //ExpN magic 4 bytes
                //4 bytes int size
                //4 bytes int count

                //ExpN Struct
                //8 bytes
                //4 bytes int
                //4 bytes int

                //Allign by 16

                //PtrN Magic 4 Bytes
                //4 bytes int size
                //4 bytes

                //PrtN Struct
                //2 bytes int
                //2 bytes int
                //4 bytes int bin data offset?
                //4 bytes int

                //Allign by 16

                //EndC Magic 4 Bytes
                //4 bytes int size
                //Allign by 16
            }
        }
    }
}
