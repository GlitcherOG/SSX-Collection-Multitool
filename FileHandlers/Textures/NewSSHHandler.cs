using SSXMultiTool.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;


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

                        stream.Position++;

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

                tempImage.sshShapeHeader = new List<SSHShapeHeader>();

                while (stream.Position < tempImage.offset + tempImage.size)
                {
                    var shape = new SSHShapeHeader();

                    shape.MatrixFormat = StreamUtil.ReadUInt8(stream);
                    shape.U0 = StreamUtil.ReadUInt8(stream);
                    shape.U1 = StreamUtil.ReadUInt8(stream);
                    shape.U11 = StreamUtil.ReadUInt8(stream);
                    shape.Size = StreamUtil.ReadUInt32(stream);
                    shape.U2 = StreamUtil.ReadUInt32(stream);
                    shape.U3 = StreamUtil.ReadUInt32(stream);
                    shape.U4 = StreamUtil.ReadUInt32(stream);
                    shape.U5 = StreamUtil.ReadUInt32(stream);
                    shape.XSize = StreamUtil.ReadUInt32(stream);
                    shape.YSize = StreamUtil.ReadUInt32(stream);

                    if (shape.Size == 0)
                    {
                        shape.Matrix = StreamUtil.ReadBytes(stream, shape.U3);
                    }
                    else
                    {
                        shape.Matrix = StreamUtil.ReadBytes(stream, shape.Size-32);
                    }

                    tempImage.sshShapeHeader.Add(shape);
                }

                var MatrixType = GetShapeMatrixType(tempImage);

                if(MatrixType==2)
                {
                    //Process Colors
                    tempImage.colorsTable = GetColorTable(tempImage);

                    //Process Matrix into Image
                    var imageMatrix = GetMatrixType(tempImage, 2);

                    if (imageMatrix.U1 == 64)
                    {
                        imageMatrix.Matrix = Unswizzle8(imageMatrix.Matrix, imageMatrix.XSize, imageMatrix.YSize);
                    }

                    //Process Image
                    tempImage.bitmap = new Bitmap(imageMatrix.XSize, imageMatrix.YSize, PixelFormat.Format32bppArgb);

                    for (int y = 0; y < imageMatrix.YSize; y++)
                    {
                        for (int x = 0; x < imageMatrix.XSize; x++)
                        {
                            int colorPos = imageMatrix.Matrix[x + imageMatrix.XSize * y];
                            tempImage.bitmap.SetPixel(x, y, tempImage.colorsTable[colorPos]);
                        }
                    }
                }
                if (MatrixType == 5)
                {
                    //Process Matrix into Image
                    var imageMatrix = GetMatrixType(tempImage, 5);

                    //Process Image
                    tempImage.bitmap = new Bitmap(imageMatrix.XSize, imageMatrix.YSize, PixelFormat.Format32bppArgb);

                    int pos = 0;
                    for (int y = 0; y < imageMatrix.YSize; y++)
                    {
                        for (int x = 0; x < imageMatrix.XSize; x++)
                        {
                            tempImage.bitmap.SetPixel(x, y, Color.FromArgb(imageMatrix.Matrix[pos * 4 + 3], imageMatrix.Matrix[pos * 4], imageMatrix.Matrix[pos * 4 + 1], imageMatrix.Matrix[pos * 4 + 2]));
                            pos++;
                        }
                    }
                }
                tempImage.bitmap.Save("I:\\PS2\\SSX\\SSX On Tour\\SLED 53625\\data\\textures\\"+i+".png");
                sshImages[i] = tempImage;
            }
        }
        public List<Color> GetColorTable(NewSSHImage newSSHImage)
        {
            var colorShape = GetMatrixType(newSSHImage, 33);
            List<Color> colors = new List<Color>();

            if (colorShape.U2 != 0)
            {
                colorShape.Matrix = ByteUtil.UnswizzlePalette(colorShape.Matrix, colorShape.XSize);
            }

            for (int i = 0; i < colorShape.XSize * colorShape.YSize; i++)
            {
                colors.Add(Color.FromArgb(colorShape.Matrix[i * 4 + 3], colorShape.Matrix[i * 4], colorShape.Matrix[i * 4 + 1], colorShape.Matrix[i * 4 + 2]));
            }

            return colors;
        }

        public SSHShapeHeader GetMatrixType(NewSSHImage newSSHImage,int Type)
        {
            for (int i = 0; i < newSSHImage.sshShapeHeader.Count; i++)
            {
                if (newSSHImage.sshShapeHeader[i].MatrixFormat==Type)
                {
                    return newSSHImage.sshShapeHeader[i];
                }
            }
            return new SSHShapeHeader();
        }

        public int GetShapeMatrixType(int ImageID)
        {
            var tempImage = sshImages[ImageID];

            for (int i = 0; i < tempImage.sshShapeHeader.Count; i++)
            {
                if (tempImage.sshShapeHeader[i].MatrixFormat == 2 || tempImage.sshShapeHeader[i].MatrixFormat == 1 || tempImage.sshShapeHeader[i].MatrixFormat == 5)
                {
                    return tempImage.sshShapeHeader[i].MatrixFormat;
                }
            }

            return -1;
        }

        public int GetShapeMatrixType(NewSSHImage tempImage)
        {
            for (int i = 0; i < tempImage.sshShapeHeader.Count; i++)
            {
                if (tempImage.sshShapeHeader[i].MatrixFormat == 2 || tempImage.sshShapeHeader[i].MatrixFormat == 1 || tempImage.sshShapeHeader[i].MatrixFormat == 5)
                {
                    return tempImage.sshShapeHeader[i].MatrixFormat;
                }
            }

            return -1;
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

            public List<SSHShapeHeader> sshShapeHeader;

            public List<Color> colorsTable;
            public Bitmap bitmap;
        }

        public struct SSHShapeHeader
        {
            public byte MatrixFormat;
            public int U0;
            public int U1;
            public int U11;
            public int Size;
            public int U2;
            public int U3;

            public int U4;
            public int U5;
            public int XSize;
            public int YSize;

            public byte[] Matrix;
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
