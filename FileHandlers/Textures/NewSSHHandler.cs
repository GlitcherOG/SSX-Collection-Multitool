using NAudio.Gui;
using SSXMultiTool.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
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

                    StandardToBitmap(stream);
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
                stream.Position = tempImage.offset;

                tempImage.sshImageHeader = new SSHShapeHeader();

                tempImage.sshImageHeader.MatrixFormat = StreamUtil.ReadUInt8(stream);
                tempImage.sshImageHeader.U0 = StreamUtil.ReadInt24(stream);
                tempImage.sshImageHeader.Size = StreamUtil.ReadUInt32(stream);
                tempImage.sshImageHeader.U2 = StreamUtil.ReadUInt32(stream);
                tempImage.sshImageHeader.U3 = StreamUtil.ReadUInt32(stream);
                tempImage.sshImageHeader.U4 = StreamUtil.ReadUInt32(stream);
                tempImage.sshImageHeader.U5 = StreamUtil.ReadUInt32(stream);
                tempImage.sshImageHeader.XSize = StreamUtil.ReadUInt32(stream);
                tempImage.sshImageHeader.YSize = StreamUtil.ReadUInt32(stream);

                tempImage.Matrix = StreamUtil.ReadBytes(stream, tempImage.sshImageHeader.Size - 32);

                //tempImage.Matrix = Unswizzle8(tempImage.Matrix, tempImage.sshImageHeader.XSize, tempImage.sshImageHeader.YSize);

                tempImage.sshColourHeader = new SSHShapeHeader();

                tempImage.sshColourHeader.MatrixFormat = StreamUtil.ReadUInt8(stream);
                tempImage.sshColourHeader.U0 = StreamUtil.ReadInt24(stream);
                tempImage.sshColourHeader.Size = StreamUtil.ReadUInt32(stream);
                tempImage.sshColourHeader.U2 = StreamUtil.ReadUInt32(stream);
                tempImage.sshColourHeader.U3 = StreamUtil.ReadUInt32(stream);
                tempImage.sshColourHeader.U4 = StreamUtil.ReadUInt32(stream);
                tempImage.sshColourHeader.U5 = StreamUtil.ReadUInt32(stream);
                tempImage.sshColourHeader.XSize = StreamUtil.ReadUInt32(stream);
                tempImage.sshColourHeader.YSize = StreamUtil.ReadUInt32(stream);

                tempImage.colorsTable = new List<Color>();

                for (global::System.Int32 j = 0; j < tempImage.sshColourHeader.XSize; j++)
                {
                    tempImage.colorsTable.Add(StreamUtil.ReadColour(stream));
                }

                //tempImage.ColourMatrix = StreamUtil.ReadBytes(stream, tempImage.sshColourHeader.Size - 32);

                //Process Image
                tempImage.bitmap = new Bitmap(tempImage.sshImageHeader.XSize, tempImage.sshImageHeader.YSize, PixelFormat.Format32bppArgb);
                int post = 0;
                for (int y = 0; y < tempImage.sshImageHeader.YSize; y++)
                {
                    for (int x = 0; x < tempImage.sshImageHeader.XSize; x++)
                    {
                        int colorPos = tempImage.Matrix[post];
                        tempImage.bitmap.SetPixel(x, y, tempImage.colorsTable[colorPos]);

                        post++;
                    }
                }

                tempImage.bitmap.Save("I:\\PS2\\SSX\\SSX On Tour\\SLED 53625\\data\\textures\\"+i+".png");
                sshImages[i] = tempImage;
            }
        }

        public static byte[] Unswizzle8(byte[] buf, int width, int height)
        {
            byte[] output = new byte[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int blockLocation = (y & ~0xf) * width + (x & ~0xf) * 2;
                    int swapSelector = (((y + 2) >> 2) & 0x1) * 4;
                    int posY = (((y & ~3) >> 1) + (y & 1)) & 0x7;
                    int columnLocation = posY * width * 2 + ((x + swapSelector) & 0x7) * 4;
                    int byteNum = ((y >> 1) & 1) + ((x >> 2) & 2);
                    int swizzleId = blockLocation + columnLocation + byteNum;

                    if (swizzleId < buf.Length && y * width + x < output.Length)
                    {
                        output[y * width + x] = buf[swizzleId];
                    }
                }
            }

            return output;
        }

        //public static List<Color> UnswizzleColor(List<Color> buf, int width, int height)
        //{
        //    List<Color> output = (new Color[width * height]).ToList();

        //    for (int y = 0; y < height; y++)
        //    {
        //        for (int x = 0; x < width; x++)
        //        {
        //            int blockLocation = (y & ~0xf) * width + (x & ~0xf) * 2;
        //            int swapSelector = (((y + 2) >> 2) & 0x1) * 4;
        //            int posY = (((y & ~3) >> 1) + (y & 1)) & 0x7;
        //            int columnLocation = posY * width * 2 + ((x + swapSelector) & 0x7) * 4;
        //            int byteNum = ((y >> 1) & 1) + ((x >> 2) & 2);
        //            int swizzleId = blockLocation + columnLocation + byteNum;

        //            if (swizzleId < buf.Count && y * width + x < output.Count)
        //            {
        //                output[y * width + x] = buf[swizzleId];
        //            }
        //        }
        //    }

        //    return output;
        //}

        public struct NewSSHImage
        {
            public int offset;
            public int size;
            public string name;

            public SSHShapeHeader sshImageHeader;
            public byte[] Matrix;
            public SSHShapeHeader sshColourHeader;
            public byte[] ColourMatrix;
            public List<Color> colorsTable;
            public Bitmap bitmap;
        }

        public struct SSHShapeHeader
        {
            public byte MatrixFormat;
            public int U0;
            public int Size;
            public int U2;
            public int U3;

            public int U4;
            public int U5;
            public int XSize;
            public int YSize;
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
