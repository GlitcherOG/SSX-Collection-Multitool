using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2
{
    internal class SSBHandler
    {
        public void LoadAndExtractSSB(string path, string extractPath)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                MemoryStream memoryStream = new MemoryStream();
                int a = 0;
                while(true)
                {
                    if (stream.Position >= stream.Length - 1)
                    {
                        break;
                    }
                    string MagicWords = StreamUtil.ReadString(stream, 4);

                    int Size = StreamUtil.ReadInt32(stream);
                    byte[] Data = new byte[Size-8];
                    byte[] DecompressedData = new byte[1];
                    Data = StreamUtil.ReadBytes(stream, Size-8);

                    RefpackHandler refpackHandler = new RefpackHandler();
                    DecompressedData=refpackHandler.Decompress(Data);
                    StreamUtil.WriteBytes(memoryStream, DecompressedData);

                    if (MagicWords.ToUpper() == "CEND")
                    {
                        var file = File.Create(extractPath + "//" + a + ".BSX");
                        memoryStream.Position = 0;
                        memoryStream.CopyTo(file);
                        memoryStream.Dispose();
                        memoryStream = new MemoryStream();
                        file.Close();
                        a++;
                    }
                }
            }
        }
    }
}
