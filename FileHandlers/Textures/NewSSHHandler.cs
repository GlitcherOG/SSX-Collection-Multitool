using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.DataFormats;

namespace SSXMultiTool.FileHandlers.Textures
{
    public class NewSSHHandler
    {
        public string MagicWord; //4
        public int Size;
        public int ImageCount; //Big 4
        public int U0;
        public List<NewSSHImage> sshImages = new List<NewSSHImage>();

        public void LoadSSH(string path)
        {
            sshImages = new List<NewSSHImage>();
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                MagicWord = StreamUtil.ReadString(stream, 4);

                if (MagicWord == "ShpS")
                {
                    Size = StreamUtil.ReadUInt32(stream);

                    ImageCount = StreamUtil.ReadUInt32(stream, true);

                    U0 = StreamUtil.ReadUInt32(stream, true);

                    for (int i = 0; i < ImageCount; i++)
                    {
                        NewSSHImage tempImage = new NewSSHImage();

                        tempImage.offset = StreamUtil.ReadUInt32(stream, true);

                        tempImage.size = StreamUtil.ReadUInt32(stream, true);

                        tempImage.name = StreamUtil.ReadNullEndString(stream);

                        sshImages.Add(tempImage);
                    }

                    //group = StreamUtil.ReadString(stream, 4);

                    //endingstring = StreamUtil.ReadString(stream, 4);

                    ////try
                    ////{
                    //StandardToBitmap(stream);
                    //}
                    //catch
                    //{
                    //    sshImages = new List<SSHImage>();
                    //    MessageBox.Show("Error reading File " + MagicWord + " " + format);
                    //}
                }
                else
                {
                    MessageBox.Show(MagicWord + " Unsupported format");
                }
                stream.Dispose();
                stream.Close();
            }
        }

        public void StandardToBitmap(Stream stream)
        {
            for (int i = 0; i < sshImages.Count; i++)
            {
                NewSSHImage tempImage = sshImages[i];
            }
        }

        public struct NewSSHImage
        {
            public int offset;
            public int size;
            public string name;

        }
        /* 
u32 Magic @ $;
u32 Size @ $;
be u32 imageCount @ $;
be u32 U0 @ $;

struct imageColour
{

};

struct imageData
{
  u8 Type; 
  u24 uimage0; 
  u32 Size; 
  u32 u2;
  u32 u3;
  u32 u4; 
  u32 u5;
  u32 u6;
  u32 u7;
  u8 Index[Size-32];
};

struct imageHeader
{
    be u32 Offset;
    be u32 Size;
    u8 Name[12];
    imageData ImageData @ Offset;
};

imageHeader Header[imageCount] @$;
        
        */
    }
}
