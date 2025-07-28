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
        public string group;
        public string endingstring;

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

                        tempImage.shortname = StreamUtil.ReadNullEndString(stream);

                        stream.Position++;

                        sshImages.Add(tempImage);
                    }

                    group = StreamUtil.ReadString(stream, 8);

                    endingstring = StreamUtil.ReadString(stream, 8);

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
                    shape.Flags1 = StreamUtil.ReadUInt8(stream); //Bit Flags? +1 - Image?, +2 - Compressed,  
                    shape.Flags2 = StreamUtil.ReadUInt8(stream); //Flags? +64 - Swizzled,
                    shape.Flags3 = StreamUtil.ReadUInt8(stream);
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

                tempImage.MatrixType  = GetShapeMatrixType(tempImage);

                if (tempImage.MatrixType == 1)
                {
                    //Process Colors
                    var colorShape = GetMatrixType(tempImage, 33);
                    tempImage.colorsTable = GetColorTable(tempImage);
                    tempImage = AlphaFix(tempImage);

                    //Process Matrix into Image
                    var imageMatrix = GetMatrixType(tempImage, 1);

                    tempImage.Compressed = (imageMatrix.Flags1 & 2) == 2;
                    tempImage.SwizzledImage = (imageMatrix.Flags2 & 64) == 64;
                    tempImage.SwizzledColours = (colorShape.Flags2 & 64) == 64;

                    if (imageMatrix.Flags2 == 64)
                    {
                        imageMatrix.Matrix = ByteUtil.Unswizzle4bpp(imageMatrix.Matrix, imageMatrix.XSize, imageMatrix.YSize);
                    }

                    byte[] tempByte = new byte[imageMatrix.Size * 2];
                    int posPoint = 0;
                    for (int a = 0; a < imageMatrix.Matrix.Length; a++)
                    {
                        tempByte[posPoint] = (byte)ByteUtil.ByteToBitConvert(imageMatrix.Matrix[a], 0, 3);
                        posPoint++;
                        tempByte[posPoint] = (byte)ByteUtil.ByteToBitConvert(imageMatrix.Matrix[a], 4, 7);
                        posPoint++;
                    }
                    imageMatrix.Matrix = tempByte;

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
                else if(tempImage.MatrixType == 2)
                {
                    //Process Colors
                    var colorShape = GetMatrixType(tempImage, 33);
                    tempImage.colorsTable = GetColorTable(tempImage);
                    tempImage = AlphaFix(tempImage);

                    //Process Matrix into Image
                    var imageMatrix = GetMatrixType(tempImage, 2);

                    tempImage.Compressed = (imageMatrix.Flags1 & 2) == 2;
                    tempImage.SwizzledImage = (imageMatrix.Flags2 & 64) == 64;
                    tempImage.SwizzledColours = (colorShape.Flags2 & 64) == 64;

                    if (imageMatrix.Flags1 == 3)
                    {
                        imageMatrix.Matrix = RefpackHandler.Decompress(imageMatrix.Matrix);
                    }

                    if (imageMatrix.Flags2 == 64)
                    {
                        imageMatrix.Matrix = ByteUtil.Unswizzle8(imageMatrix.Matrix, imageMatrix.XSize, imageMatrix.YSize);
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
                else if(tempImage.MatrixType == 5)
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

                tempImage.colorsTable = ImageUtil.GetBitmapColorsFast(tempImage.bitmap).ToList();
                sshImages[i] = tempImage;
            }
        }

        public List<Color> GetColorTable(NewSSHImage newSSHImage)
        {
            var colorShape = GetMatrixType(newSSHImage, 33);
            List<Color> colors = new List<Color>();

            if (colorShape.Flags2 == 64)
            {
                colorShape.Matrix = ByteUtil.UnswizzlePalette(colorShape.Matrix, colorShape.XSize);
            }

            for (int i = 0; i < colorShape.XSize * colorShape.YSize; i++)
            {
                colors.Add(Color.FromArgb(colorShape.Matrix[i * 4 + 3], colorShape.Matrix[i * 4], colorShape.Matrix[i * 4 + 1], colorShape.Matrix[i * 4 + 2]));
            }

            return colors;
        }


        public NewSSHImage AlphaFix(NewSSHImage newSSHImage)
        {
            bool TestAlpha = true;

            for (int i = 0; i < newSSHImage.colorsTable.Count; i++)
            {
                if (newSSHImage.colorsTable[i].A>0x80)
                {
                    TestAlpha = false;
                    break;
                }
            }
            newSSHImage.AlphaFix = true;

            if (TestAlpha)
            {
                for (int i = 0; i < newSSHImage.colorsTable.Count; i++)
                {
                    var TempColour = newSSHImage.colorsTable[i];
                    int A = TempColour.A * 2 - 1;
                    if (A > 255)
                    {
                        A = 255;
                    }
                    else if (A < 0)
                    {
                        A = 0;
                    }
                    TempColour = Color.FromArgb(A, TempColour.R, TempColour.G, TempColour.B);
                    newSSHImage.colorsTable[i] = TempColour;
                }
            }


            return newSSHImage;
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

        public void BMPExtract(string path)
        {
            for (int i = 0; i < sshImages.Count; i++)
            {
                sshImages[i].bitmap.Save(path + "\\" + sshImages[i].shortname + /*"." +*/ i + ".png", ImageFormat.Png);
            }
        }

        public void BMPOneExtract(string path, int i)
        {
            sshImages[i].bitmap.Save(path, ImageFormat.Png);
        }

        public void LoadSingle(string path, int i)
        {
            Stream stream = File.Open(path, FileMode.Open);

            var ImageTemp = Image.FromStream(stream);
            stream.Close();
            stream.Dispose();
            var temp = sshImages[i];
            temp.bitmap = (Bitmap)ImageTemp;
            temp.colorsTable = ImageUtil.GetBitmapColorsFast(temp.bitmap).ToList();
            temp.MatrixType = sshImages[i].MatrixType;
            sshImages[i] = temp;
        }

        public void SaveSSH(string path, bool TestImages)
        {
            for (int i = 0; i < sshImages.Count; i++)
            {
                if (TestImages)
                {
                    var sshImage = sshImages[i];

                    sshImage.colorsTable = ImageUtil.GetBitmapColorsFast(sshImage.bitmap).ToList();

                    //if metal bin combine images and then reduce

                    if (sshImage.colorsTable.Count > 256 && sshImage.MatrixType == 2)
                    {
                        sshImage.bitmap = ImageUtil.ReduceBitmapColorsSlow(sshImage.bitmap, 256);
                        //MessageBox.Show(sshImages[i].shortname + " " + i.ToString() + " Exceeds 256 Colours");
                        //check = true;
                    }
                    if (sshImage.colorsTable.Count > 16 && sshImage.MatrixType == 1)
                    {
                        sshImage.bitmap = ImageUtil.ReduceBitmapColorsSlow(sshImage.bitmap, 16);
                        //MessageBox.Show(sshImage.shortname + " " + i.ToString() + " Exceeds 16 Colours");
                        //check = true;
                    }
                    sshImages[i] = sshImage;
                }
            }

            //Write Header

            byte[] tempByte = new byte[4];
            Stream stream = new MemoryStream();

            StreamUtil.WriteString(stream, "ShpS");

            long SizePos = stream.Position;
            tempByte = new byte[4];
            stream.Write(tempByte, 0, tempByte.Length);

            StreamUtil.WriteInt32(stream, sshImages.Count);

            StreamUtil.WriteInt32(stream, U0);

            List<int> intPos = new List<int>();

            for (int i = 0; i < sshImages.Count; i++)
            {
                intPos.Add((int)stream.Position);
                tempByte = new byte[8];
                stream.Write(tempByte, 0, tempByte.Length);
                StreamUtil.WriteNullString(stream, sshImages[i].shortname);
            }

            StreamUtil.WriteString(stream, "Buy ERTS", 8);

            StreamUtil.AlignBy16(stream);

            ////Process Image to SSHShapeHeader
            //for (int i = 0; i < length; i++)
            //{
                
            //}

        }

        public struct NewSSHImage
        {
            public int offset;
            public int size;
            public string shortname;
            public string longname;
            public List<SSHShapeHeader> sshShapeHeader;

            //Converted
            public List<Color> colorsTable;
            public Bitmap bitmap;
            public int MatrixType;
            public bool Compressed;
            public bool SwizzledImage;
            public bool SwizzledColours;
            public bool AlphaFix;
        }

        public struct SSHShapeHeader
        {
            public byte MatrixFormat;
            public int Flags1;
            public int Flags2;
            public int Flags3;
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
