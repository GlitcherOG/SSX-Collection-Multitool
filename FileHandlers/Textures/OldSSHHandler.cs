using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using SSXMultiTool.Utilities;
using System.Reflection;

namespace SSXMultiTool.FileHandlers.Textures
{
    public class OldSSHHandler
    {
        public string MagicWord;
        public int fileSize;
        public int imageCount;
        public string format;
        public string group;
        public string endingstring;
        public List<SSHImage> sshImages = new List<SSHImage>();
        public void LoadSSH(string path)
        {
            sshImages = new List<SSHImage>();
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                MagicWord = StreamUtil.ReadString(stream, 4);

                if (MagicWord == "SHPS")
                {
                    fileSize = StreamUtil.ReadUInt32(stream);

                    imageCount = StreamUtil.ReadUInt32(stream);

                    format = StreamUtil.ReadString(stream, 4);

                    for (int i = 0; i < imageCount; i++)
                    {
                        SSHImage tempImage = new SSHImage();

                        tempImage.shortname = StreamUtil.ReadString(stream, 4);

                        tempImage.offset = StreamUtil.ReadUInt32(stream);

                        sshImages.Add(tempImage);
                    }

                    group = StreamUtil.ReadString(stream, 4);

                    endingstring = StreamUtil.ReadString(stream, 4);

                    try
                    {
                        StandardToBitmap(stream);
                    }
                    catch
                    {
                        sshImages = new List<SSHImage>();
                        MessageBox.Show("Error reading File " + MagicWord + " " + format);
                    }
                }
                else if (MagicWord == "ShpS")
                {
                    sshImages = new List<SSHImage>();
                    MessageBox.Show("Error Reading File new SSH Detected");
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
            byte[] tempByte;

            for (int i = 0; i < sshImages.Count; i++)
            {
                SSHImage tempImage = sshImages[i];
                SSHImageHeader tempImageHeader = new SSHImageHeader();

                stream.Position = tempImage.offset;

                tempImageHeader.MatrixFormat = StreamUtil.ReadUInt8(stream);

                tempImageHeader.Size = StreamUtil.ReadInt24(stream);

                tempImageHeader.Width = StreamUtil.ReadInt16(stream);

                tempImageHeader.Height = StreamUtil.ReadInt16(stream);

                tempImageHeader.Xaxis = StreamUtil.ReadInt16(stream);

                tempImageHeader.Yaxis = StreamUtil.ReadInt16(stream);

                //Add Other Flags Later
                tempImageHeader.LXPos = StreamUtil.ReadInt12(stream);

                tempImageHeader.TYPos = StreamUtil.ReadInt12(stream);

                int RealSize;

                if (tempImageHeader.Size != 0)
                {
                    RealSize = tempImageHeader.Size - 16;
                }
                else
                {
                    RealSize = tempImageHeader.Width * tempImageHeader.Height;
                    if (tempImageHeader.MatrixFormat == 5)
                    {
                        RealSize = RealSize * 4;
                    }
                }

                //Read Matrix
                tempByte = new byte[RealSize];
                stream.Read(tempByte, 0, tempByte.Length);
                tempImage.Matrix = tempByte;

                //Decompress
                if (tempImageHeader.MatrixFormat == 130)
                {
                    tempImage.Matrix = RefpackHandler.Decompress(tempImage.Matrix);
                }

                //Split Image Into Proper Bytes
                if (tempImageHeader.MatrixFormat == 1)
                {
                    tempByte = new byte[RealSize * 2];
                    int posPoint = 0;
                    for (int a = 0; a < tempImage.Matrix.Length; a++)
                    {
                        tempByte[posPoint] = (byte)ByteUtil.ByteToBitConvert(tempImage.Matrix[a], 0, 3);
                        posPoint++;
                        tempByte[posPoint] = (byte)ByteUtil.ByteToBitConvert(tempImage.Matrix[a], 4, 7);
                        posPoint++;
                    }
                    tempImage.Matrix = tempByte;
                }

                //INDEXED COLOUR
                if (tempImageHeader.MatrixFormat == 2 || tempImageHeader.MatrixFormat == 1 || tempImageHeader.MatrixFormat == 130)
                {
                    int Spos = (int)stream.Position;
                    bool find = false;
                    while (!find)
                    {
                        if (stream.ReadByte() == 0x21)
                        {
                            Spos = (int)stream.Position;
                            find = true;
                        }
                    }
                    SSHColourTable sshTable = new SSHColourTable();

                    sshTable.Size = StreamUtil.ReadInt24(stream);

                    sshTable.Width = StreamUtil.ReadInt16(stream);

                    sshTable.Height = StreamUtil.ReadInt16(stream);

                    sshTable.Total = StreamUtil.ReadInt16(stream);

                    sshTable.Format = StreamUtil.ReadUInt32(stream);

                    sshTable.colorTable = new List<Color>();

                    int tempSize = sshTable.Size / 4 - 4;
                    if (sshTable.Size == 0)
                    {
                        tempSize = sshTable.Total;
                    }

                    stream.Position = Spos + 15;

                    for (int a = 0; a < tempSize; a++)
                    {
                        sshTable.colorTable.Add(StreamUtil.ReadColour(stream));
                    }

                    int Max = 0;
                    //Alpha Fix
                    for (int a = 0; a < sshTable.colorTable.Count; a++)
                    {
                        if (Max < sshTable.colorTable[a].A)
                        {
                            Max = sshTable.colorTable[a].A;
                        }
                    }
                    if (Max <= 128)
                    {
                        tempImage.AlphaFix = true;

                        for (int a = 0; a < sshTable.colorTable.Count; a++)
                        {
                            var TempColour = sshTable.colorTable[a];
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
                            sshTable.colorTable[a] = TempColour;
                        }
                    }

                    tempImage.sshTable = sshTable;
                }

                //Find End Of Image
                long endPos = -1;

                if (i + 1 < sshImages.Count)
                {
                    endPos = sshImages[i + 1].offset;
                }
                else
                {
                    endPos = stream.Length;
                }
                List<Color> MetalColours = new List<Color>();
                //Colour Correction
                int tempRead = stream.ReadByte();
                if (tempRead == 105)
                {
                    //tempImage.MetalBin = true;
                    SSHColourTable sshTable = tempImage.sshTable;
                    for (int c = 0; c < sshTable.colorTable.Count; c++)
                    {
                        Color tempColor = sshTable.colorTable[c];
                        MetalColours.Add(Color.FromArgb(255, tempColor.A, tempColor.A, tempColor.A));
                        int A = 255;
                        int R = tempColor.R;
                        int G = tempColor.G;
                        int B = tempColor.B;
                        sshTable.colorTable[c] = Color.FromArgb(A, R, G, B);
                    }
                    tempImage.sshTable = sshTable;
                }
                else
                {
                    stream.Position -= 1;
                }

                //Get LongName
                endPos = ByteUtil.FindPosition(stream, new byte[1] { 0x70 }, stream.Position - 1, endPos);
                if (endPos != -1)
                {
                    stream.Position = endPos;

                    tempImage.unknownEnd = StreamUtil.ReadUInt32(stream);

                    tempImage.longname = StreamUtil.ReadNullEndString(stream);
                }

                //Create Bitmap Image
                tempImage.bitmap = new Bitmap(tempImageHeader.Width, tempImageHeader.Height, PixelFormat.Format32bppArgb);
                tempImage.metalBitmap = new Bitmap(tempImageHeader.Width, tempImageHeader.Height, PixelFormat.Format32bppArgb);
                int post = 0;
                if (tempImageHeader.MatrixFormat == 1)
                {
                    for (int y = 0; y < tempImageHeader.Height; y++)
                    {
                        for (int x = 0; x < tempImageHeader.Width; x++)
                        {
                            int colorPos = tempImage.Matrix[post];
                            tempImage.bitmap.SetPixel(x, y, tempImage.sshTable.colorTable[colorPos]);

                            if (tempImage.MetalBin)
                            {
                                tempImage.metalBitmap.SetPixel(x, y, MetalColours[colorPos]);
                            }
                            post++;
                        }
                    }
                }
                else
                if (tempImageHeader.MatrixFormat == 2 || tempImageHeader.MatrixFormat == 130)
                {
                    if (tempImageHeader.LXPos == 2)
                    {
                        tempImage.Matrix = ByteUtil.ByteArraySwap(tempImage.Matrix, tempImageHeader);
                    }
                    for (int y = 0; y < tempImageHeader.Height; y++)
                    {
                        for (int x = 0; x < tempImageHeader.Width; x++)
                        {
                            int colorPos = tempImage.Matrix[post];
                            if (tempImage.sshTable.Format != 0)
                            {
                                colorPos = ByteUtil.ByteBitSwitch(colorPos);
                            }

                            if (tempImage.MetalBin)
                            {
                                tempImage.metalBitmap.SetPixel(x, y, MetalColours[colorPos]);
                            }

                            tempImage.bitmap.SetPixel(x, y, tempImage.sshTable.colorTable[colorPos]);
                            post++;
                        }
                    }
                }
                else
                if (tempImageHeader.MatrixFormat == 5)
                {
                    SSHColourTable colourTable = new SSHColourTable();
                    colourTable.colorTable = new List<Color>();
                    for (int y = 0; y < tempImageHeader.Height; y++)
                    {
                        for (int x = 0; x < tempImageHeader.Width; x++)
                        {
                            int R = tempImage.Matrix[post];
                            post++;
                            int G = tempImage.Matrix[post];
                            post++;
                            int B = tempImage.Matrix[post];
                            post++;
                            int A = tempImage.Matrix[post];
                            post++;
                            tempImage.bitmap.SetPixel(x, y, Color.FromArgb(A, R, G, B));
                            if (!colourTable.colorTable.Contains(Color.FromArgb(A, R, G, B)))
                            {
                                colourTable.colorTable.Add(Color.FromArgb(A, R, G, B));
                            }
                        }
                    }
                    tempImage.sshTable = colourTable;
                }
                else
                {
                    MessageBox.Show("Error reading File " + MagicWord + " " + format + "- Matrix " + tempImageHeader.MatrixFormat.ToString());
                    break;
                }
                tempImage.sshHeader = tempImageHeader;
                sshImages[i] = tempImage;
                //SSHColorCalculate(i);
            }
            stream.Dispose();
        }

        public void SSHColorCalculate(int i)
        {
            SSHImage temp = sshImages[i];
            SSHColourTable colourTable = new SSHColourTable();
            colourTable.colorTable = new List<Color>();
            if (temp.MetalBin)
            {
                if (sshImages[i].bitmap.Width != sshImages[i].metalBitmap.Width || sshImages[i].bitmap.Height != sshImages[i].metalBitmap.Height)
                {
                    Bitmap metalBitmap = new Bitmap(sshImages[i].bitmap.Width, sshImages[i].bitmap.Height, PixelFormat.Format32bppArgb);
                    temp.metalBitmap = metalBitmap;
                }
            }
            for (int y = 0; y < temp.bitmap.Height; y++)
            {
                for (int x = 0; x < temp.bitmap.Width; x++)
                {
                    Color color = temp.bitmap.GetPixel(x, y);
                    if (temp.MetalBin)
                    {
                        color = Color.FromArgb(temp.metalBitmap.GetPixel(x, y).R, color.R, color.G, color.B);
                    }

                    if (!colourTable.colorTable.Contains(color))
                    {
                        colourTable.colorTable.Add(color);
                    }
                }
            }
            colourTable.Format = temp.sshTable.Format;
            temp.sshTable = colourTable;
            sshImages[i] = temp;
        }

        public void BMPExtract(string path)
        {
            for (int i = 0; i < sshImages.Count; i++)
            {
                sshImages[i].bitmap.Save(path + "\\" + sshImages[i].shortname + ".png", ImageFormat.Png);
            }
        }

        public void BMPOneExtract(string path, int i)
        {
            sshImages[i].bitmap.Save(path, ImageFormat.Png);
        }

        public void BMPOneExtractMetal(string path, int i)
        {
            sshImages[i].metalBitmap.Save(path, ImageFormat.Png);
        }

        public void BMPOneBothExtract(string path, int i)
        {
            Bitmap newBitmap = new Bitmap(sshImages[i].bitmap.Width, sshImages[i].bitmap.Height, PixelFormat.Format32bppArgb);
            for (int y = 0; y < sshImages[i].bitmap.Height; y++)
            {
                for (int x = 0; x < sshImages[i].bitmap.Width; x++)
                {
                    Color tempColor1 = sshImages[i].metalBitmap.GetPixel(x, y);
                    Color tempColor2 = sshImages[i].bitmap.GetPixel(x, y);
                    Color tempColor = Color.FromArgb(tempColor1.R, tempColor2.R, tempColor2.G, tempColor2.B);
                    newBitmap.SetPixel(x, y, tempColor);
                }
            }
            newBitmap.Save(path, ImageFormat.Png);
        }

        public void LoadSingleBoth(string path, int i)
        {
            Stream stream = File.Open(path, FileMode.Open);
            var ImageTemp = Image.FromStream(stream);
            stream.Close();
            stream.Dispose();
            SSHImage temp = sshImages[i];
            temp.bitmap = (Bitmap)ImageTemp;
            SSHColourTable colourTable = new SSHColourTable();
            colourTable.colorTable = new List<Color>();
            for (int y = 0; y < temp.bitmap.Height; y++)
            {
                for (int x = 0; x < temp.bitmap.Width; x++)
                {
                    Color color = temp.bitmap.GetPixel(x, y);
                    Color AColor = Color.FromArgb(255, color.A, color.A, color.A);
                    Color ImageColor = Color.FromArgb(255, color.R, color.G, color.B);
                    temp.metalBitmap.SetPixel(x, y, AColor);
                    temp.bitmap.SetPixel(x, y, ImageColor);

                    if (!colourTable.colorTable.Contains(ImageColor))
                    {
                        colourTable.colorTable.Add(ImageColor);
                    }
                }
            }
            temp.sshTable = colourTable;
            sshImages[i] = temp;
            SSHColorCalculate(i);
        }

        public void LoadSingle(string path, int i)
        {
            Stream stream = File.Open(path, FileMode.Open);

            var ImageTemp = Image.FromStream(stream);
            stream.Close();
            stream.Dispose();
            SSHImage temp = sshImages[i];
            temp.bitmap = (Bitmap)ImageTemp;
            if (temp.metalBitmap == null)
            {
                temp.metalBitmap = new Bitmap(temp.bitmap.Width, temp.bitmap.Height, PixelFormat.Format32bppArgb);
            }
            SSHColourTable colourTable = new SSHColourTable();
            colourTable.colorTable = new List<Color>();
            for (int y = 0; y < temp.bitmap.Height; y++)
            {
                for (int x = 0; x < temp.bitmap.Width; x++)
                {
                    Color color = temp.bitmap.GetPixel(x, y);
                    if (!colourTable.colorTable.Contains(color))
                    {
                        colourTable.colorTable.Add(color);
                    }
                }
            }
            colourTable.Format = sshImages[i].sshTable.Format;
            temp.sshTable = colourTable;
            sshImages[i] = temp;
            SSHColorCalculate(i);
        }

        public void LoadSingleMetal(string path, int i)
        {
            Stream stream = File.Open(path, FileMode.Open);
            var ImageTemp = Image.FromStream(stream);
            stream.Close();
            stream.Dispose();

            SSHImage temp = sshImages[i];
            temp.metalBitmap = (Bitmap)ImageTemp;
            sshImages[i] = temp;
            SSHColorCalculate(i);
        }

        public void LoadFolder(string path)
        {
            MagicWord = "";
            fileSize = 0;
            group = "";
            endingstring = "";
            sshImages = new List<SSHImage>();
            string[] paths = Directory.GetFiles(path, "*.png");

            for (int i = 1; i < paths.Length; i++)
            {
                Stream stream = File.Open(paths[i], FileMode.Open);
                SSHImage tempImage = new SSHImage();
                SSHImageHeader imageHeader = new SSHImageHeader();
                var ImageTemp = Image.FromStream(stream);
                stream.Close();
                stream.Dispose();
                tempImage.bitmap = (Bitmap)ImageTemp;
                imageHeader.MatrixFormat = 2;

                string name = Path.GetFileName(paths[i].Replace(".png", ""));
                string[] NameList = name.Split('.');

                if(NameList.Length>1)
                {
                    tempImage.longname = NameList[1];
                }
                else
                {
                    tempImage.longname = NameList[0];
                }
                tempImage.shortname = NameList[0];
                imageHeader.Width = tempImage.bitmap.Width;
                imageHeader.Height = tempImage.bitmap.Height;
                SSHColourTable colourTable = new SSHColourTable();
                colourTable.colorTable = new List<Color>();

                for (int y = 0; y < tempImage.bitmap.Height; y++)
                {
                    for (int x = 0; x < tempImage.bitmap.Width; x++)
                    {
                        Color color = tempImage.bitmap.GetPixel(x, y);
                        if (!colourTable.colorTable.Contains(color))
                        {
                            colourTable.colorTable.Add(color);
                        }
                    }
                }
                tempImage.metalBitmap = new Bitmap(tempImage.bitmap.Width, tempImage.bitmap.Height);
                tempImage.sshTable = colourTable;
                tempImage.sshHeader = imageHeader;
                sshImages.Add(tempImage);
            }
        }

        public void BrightenBitmap(int i)
        {
            Bitmap bitmap = new Bitmap(sshImages[i].bitmap.Width, sshImages[i].bitmap.Height, PixelFormat.Format32bppArgb);
            for (int y = 0; y < sshImages[i].bitmap.Height; y++)
            {
                for (int x = 0; x < sshImages[i].bitmap.Width; x++)
                {
                    Color color = sshImages[i].bitmap.GetPixel(x, y);
                    int A = color.A;
                    int R = color.R * 2 - 1;
                    if (R < 0)
                    {
                        R = 0;
                    }
                    else if (R > 255)
                    {
                        R = 255;
                    }
                    int G = color.G * 2 - 1;
                    if (G < 0)
                    {
                        G = 0;
                    }
                    else if (G > 255)
                    {
                        G = 255;
                    }
                    int B = color.B * 2 - 1;
                    if (B < 0)
                    {
                        B = 0;
                    }
                    else if (B > 255)
                    {
                        B = 255;
                    }

                    color = Color.FromArgb(A, R, G, B);
                    bitmap.SetPixel(x, y, color);
                }
            }
            var tempimage = sshImages[i];
            tempimage.bitmap = bitmap;
            sshImages[i] = tempimage;
        }

        public void DarkenImage(int i)
        {
            Bitmap bitmap = new Bitmap(sshImages[i].bitmap.Width, sshImages[i].bitmap.Height, PixelFormat.Format32bppArgb);
            for (int y = 0; y < sshImages[i].bitmap.Height; y++)
            {
                for (int x = 0; x < sshImages[i].bitmap.Width; x++)
                {
                    Color color = sshImages[i].bitmap.GetPixel(x, y);
                    int A = color.A;
                    int R = color.R;
                    int G = color.G;
                    int B = color.B;

                    R = (R + 1) / 2;
                    G = (G + 1) / 2;
                    B = (B + 1) / 2;

                    color = Color.FromArgb(A, R, G, B);
                    bitmap.SetPixel(x, y, color);
                }
            }
            var tempimage = sshImages[i];
            tempimage.bitmap = bitmap;
            sshImages[i] = tempimage;
        }

        public void HalfAlphaImage(int i)
        {
            Bitmap bitmap = new Bitmap(sshImages[i].bitmap.Width, sshImages[i].bitmap.Height, PixelFormat.Format32bppArgb);
            for (int y = 0; y < sshImages[i].bitmap.Height; y++)
            {
                for (int x = 0; x < sshImages[i].bitmap.Width; x++)
                {
                    Color color = sshImages[i].bitmap.GetPixel(x, y);
                    int A = color.A;
                    int R = color.R;
                    int G = color.G;
                    int B = color.B;

                    A = (A + 1) / 2;

                    color = Color.FromArgb(A, R, G, B);
                    bitmap.SetPixel(x, y, color);
                }
            }
            var tempimage = sshImages[i];
            tempimage.bitmap = bitmap;
            sshImages[i] = tempimage;
        }

        public void DoubleAlphaImage(int i)
        {
            Bitmap bitmap = new Bitmap(sshImages[i].bitmap.Width, sshImages[i].bitmap.Height, PixelFormat.Format32bppArgb);
            for (int y = 0; y < sshImages[i].bitmap.Height; y++)
            {
                for (int x = 0; x < sshImages[i].bitmap.Width; x++)
                {
                    Color color = sshImages[i].bitmap.GetPixel(x, y);
                    int A = color.A * 2 - 1;
                    if (A < 0)
                    {
                        A = 0;
                    }
                    else if (A > 255)
                    {
                        A = 255;
                    }
                    int R = color.R;
                    int G = color.G;
                    int B = color.B;

                    color = Color.FromArgb(A, R, G, B);
                    bitmap.SetPixel(x, y, color);
                }
            }
            var tempimage = sshImages[i];
            tempimage.bitmap = bitmap;
            sshImages[i] = tempimage;
        }

        public void RemoveImage(int i)
        {
            sshImages.RemoveAt(i);
        }

        public void AddImage(int imageSize = 64, int MatrixType = 2)
        {
            var NewSSHImage = new SSHImage();
            NewSSHImage.sshHeader.MatrixFormat = (byte)MatrixType;
            NewSSHImage.shortname = "????";
            NewSSHImage.bitmap = new Bitmap(imageSize, imageSize, PixelFormat.Format32bppArgb);
            NewSSHImage.sshTable.colorTable = new List<Color>();
            sshImages.Add(NewSSHImage);
        }

        public void SaveSSH(string path, bool Test)
        {
            bool check = false;
            for (int i = 0; i < sshImages.Count; i++)
            {
                if (Test)
                {
                    SSHColorCalculate(i);
                    if (sshImages[i].sshTable.Total > 256 && (sshImages[i].sshHeader.MatrixFormat == 2 || sshImages[i].sshHeader.MatrixFormat == 130))
                    {
                        MessageBox.Show(sshImages[i].shortname + " " + i.ToString() + " Exceeds 256 Colours");
                        check = true;
                    }
                    if (sshImages[i].sshTable.Total > 16 && sshImages[i].sshHeader.MatrixFormat == 1)
                    {
                        MessageBox.Show(sshImages[i].shortname + " " + i.ToString() + " Exceeds 16 Colours");
                        check = true;
                    }
                }
                if (sshImages[i].sshHeader.MatrixFormat == 130)
                {
                    MessageBox.Show("Error Unable to write to matrix 130");
                    check = true;
                }
            }
            if (check == true)
            {
                return;
            }
            byte[] tempByte = new byte[4];
            Stream stream = new MemoryStream();

            StreamUtil.WriteString(stream, "SHPS");

            long SizePos = stream.Position;
            tempByte = new byte[4];
            stream.Write(tempByte, 0, tempByte.Length);

            StreamUtil.WriteInt32(stream, sshImages.Count);

            StreamUtil.WriteString(stream, format);

            List<int> intPos = new List<int>();

            for (int i = 0; i < sshImages.Count; i++)
            {
                StreamUtil.WriteString(stream, sshImages[i].shortname);
                intPos.Add((int)stream.Position);
                tempByte = new byte[4];
                stream.Write(tempByte, 0, tempByte.Length);
            }

            StreamUtil.WriteString(stream, "Buy ERTS", 16);

            for (int i = 0; i < sshImages.Count; i++)
            {
                int temp = (int)stream.Position;
                stream.Position = intPos[i];

                //Set Start Offset

                StreamUtil.WriteInt32(stream, temp);

                stream.Position = temp;

                StreamUtil.WriteUInt8(stream, sshImages[i].sshHeader.MatrixFormat);

                //Set SSH Header Info
                if (sshImages[i].sshHeader.MatrixFormat == 1)
                {
                    StreamUtil.WriteInt24(stream, sshImages[i].bitmap.Width * sshImages[i].bitmap.Height / 2 + 16);
                }
                else
                if (sshImages[i].sshHeader.MatrixFormat == 2 || sshImages[i].sshHeader.MatrixFormat == 130)
                {
                    StreamUtil.WriteInt24(stream, sshImages[i].bitmap.Width * sshImages[i].bitmap.Height + 16);
                }
                else if (sshImages[i].sshHeader.MatrixFormat == 5)
                {
                    StreamUtil.WriteInt24(stream, sshImages[i].bitmap.Width * sshImages[i].bitmap.Height * 4 + 16);
                }

                StreamUtil.WriteInt16(stream, sshImages[i].bitmap.Width);

                StreamUtil.WriteInt16(stream, sshImages[i].bitmap.Height);

                StreamUtil.WriteInt16(stream, sshImages[i].sshHeader.Xaxis);

                StreamUtil.WriteInt16(stream, sshImages[i].sshHeader.Yaxis);

                if (sshImages[i].sshHeader.LXPos == 2)
                {
                    tempByte = new byte[4] { 0x00, 0x20, 0x00, 0x00 };
                }
                else
                {
                    tempByte = new byte[4];
                }
                stream.Write(tempByte, 0, tempByte.Length);

                if (sshImages[i].sshHeader.MatrixFormat == 1)
                {
                    Maxtrix1Write(stream, i, stream.Position);
                }
                if (sshImages[i].sshHeader.MatrixFormat == 2)
                {
                    Maxtrix2Write(stream, i, stream.Position);
                }
                else if (sshImages[i].sshHeader.MatrixFormat == 5)
                {
                    Maxtrix5Write(stream, i);
                }
                else if (sshImages[i].sshHeader.MatrixFormat == 130)
                {
                    MessageBox.Show("Error Can't Compress file (Compresson method doesn't exist)");
                }

                if (sshImages[i].MetalBin)
                {
                    tempByte = new byte[16] { 0x69, 0x10, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    stream.Write(tempByte, 0, tempByte.Length);
                }

                if (sshImages[i].longname != "" && sshImages[i].longname != null)
                {
                    //ending
                    tempByte = new byte[4] { 0x70, 0x00, 0x00, 0x00 };
                    stream.Write(tempByte, 0, tempByte.Length);

                    StreamUtil.WriteNullString(stream, sshImages[i].longname);

                    StreamUtil.WriteString(stream, "Buy ERTS", 9);
                }
            }

            stream.Position = SizePos;

            StreamUtil.WriteInt32(stream, (int)stream.Length);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            var file = File.Create(path);
            stream.Position = 0;
            stream.CopyTo(file);
            stream.Dispose();
            file.Close();
        }

        public void Maxtrix1Write(Stream stream, int i, long pos)
        {
            stream.Position = pos;
            byte[] tempByte;
            SSHColourTable colourTable = new SSHColourTable();
            colourTable.colorTable = new List<Color>();
            byte[] ByteCombine = new byte[2];
            int bytepos = 0;

            for (int y = 0; y < sshImages[i].bitmap.Height; y++)
            {
                for (int x = 0; x < sshImages[i].bitmap.Width; x++)
                {
                    Color color = sshImages[i].bitmap.GetPixel(x, y);
                    //Alpha Fix
                    if (sshImages[i].AlphaFix)
                    {
                        int A = (color.A + 1) / 2;
                        color = Color.FromArgb(A, color.R, color.G, color.B);
                    }


                    if (colourTable.colorTable.Contains(color))
                    {
                        int index = colourTable.colorTable.IndexOf(color);
                        tempByte = new byte[4];
                        BitConverter.GetBytes(index).CopyTo(tempByte, 0);
                        ByteCombine[bytepos] = tempByte[0];
                    }
                    else
                    {
                        colourTable.colorTable.Add(color);
                        int index = colourTable.colorTable.Count - 1;
                        tempByte = new byte[4];
                        BitConverter.GetBytes(index).CopyTo(tempByte, 0);
                        ByteCombine[bytepos] = tempByte[0];
                    }
                    bytepos++;

                    if (bytepos == 2)
                    {
                        bytepos = 0;
                        tempByte = new byte[4];
                        BitConverter.GetBytes(ByteUtil.BitConbineConvert(ByteCombine[0], ByteCombine[1], 0, 4, 4)).CopyTo(tempByte, 0);
                        ByteCombine = new byte[2];
                        stream.Write(tempByte, 0, 1);
                    }
                }
            }

            //Colour Table
            tempByte = new byte[4];
            BitConverter.GetBytes(0x21).CopyTo(tempByte, 0);
            stream.Write(tempByte, 0, 1);

            tempByte = new byte[4];
            BitConverter.GetBytes(colourTable.colorTable.Count * 4 + 16).CopyTo(tempByte, 0);
            stream.Write(tempByte, 0, 3);

            tempByte = new byte[4];
            BitConverter.GetBytes(colourTable.colorTable.Count).CopyTo(tempByte, 0);
            stream.Write(tempByte, 0, 2);

            tempByte = new byte[4];
            BitConverter.GetBytes(1).CopyTo(tempByte, 0);
            stream.Write(tempByte, 0, 2);

            tempByte = new byte[4];
            BitConverter.GetBytes(colourTable.colorTable.Count).CopyTo(tempByte, 0);
            stream.Write(tempByte, 0, 2);

            tempByte = new byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            //BitConverter.GetBytes(colourTable.colorTable.Count).CopyTo(tempByte, 0);
            stream.Write(tempByte, 0, tempByte.Length);

            for (int a = 0; a < colourTable.colorTable.Count; a++)
            {
                tempByte = new byte[4];
                int R = colourTable.colorTable[a].R;
                int G = colourTable.colorTable[a].G;
                int B = colourTable.colorTable[a].B;
                int A = colourTable.colorTable[a].A;
                BitConverter.GetBytes(R).CopyTo(tempByte, 0);
                stream.Write(tempByte, 0, 1);
                tempByte = new byte[4];
                BitConverter.GetBytes(G).CopyTo(tempByte, 0);
                stream.Write(tempByte, 0, 1);
                tempByte = new byte[4];
                BitConverter.GetBytes(B).CopyTo(tempByte, 0);
                stream.Write(tempByte, 0, 1);
                tempByte = new byte[4];
                BitConverter.GetBytes(A).CopyTo(tempByte, 0);
                stream.Write(tempByte, 0, 1);
            }
        }

        public void Maxtrix2Write(Stream stream, int i, long pos)
        {
            stream.Position = pos;
            byte[] tempByte;
            SSHColourTable colourTable = new SSHColourTable();
            colourTable.colorTable = new List<Color>();

            byte[] Matrix = new byte[sshImages[i].bitmap.Height * sshImages[i].bitmap.Width];
            int pos1 = 0;
            for (int y = 0; y < sshImages[i].bitmap.Height; y++)
            {
                for (int x = 0; x < sshImages[i].bitmap.Width; x++)
                {
                    Color color = sshImages[i].bitmap.GetPixel(x, y);

                    if (sshImages[i].MetalBin)
                    {
                        color = Color.FromArgb(sshImages[i].metalBitmap.GetPixel(x, y).R, color.R, color.G, color.B);
                    }

                    if (sshImages[i].AlphaFix)
                    {
                        int A = (color.A + 1) / 2;
                        color = Color.FromArgb(A, color.R, color.G, color.B);
                    }

                    if (colourTable.colorTable.Contains(color))
                    {
                        int index = colourTable.colorTable.IndexOf(color);
                        if (sshImages[i].sshTable.Format != 0)
                        {
                            index = ByteUtil.ByteBitSwitch(index);
                        }
                        tempByte = new byte[4];
                        BitConverter.GetBytes(index).CopyTo(tempByte, 0);
                        Matrix[pos1] = tempByte[0];
                    }
                    else
                    {
                        colourTable.colorTable.Add(color);
                        int index = colourTable.colorTable.Count - 1;
                        if (sshImages[i].sshTable.Format != 0)
                        {
                            index = ByteUtil.ByteBitSwitch(index);
                        }
                        tempByte = new byte[4];
                        BitConverter.GetBytes(index).CopyTo(tempByte, 0);
                        Matrix[pos1] = tempByte[0];
                    }
                    pos1++;
                }
            }

            if (sshImages[i].sshHeader.LXPos == 2)
            {
                SSHImageHeader imageHeader = new SSHImageHeader();
                imageHeader.Width = sshImages[i].bitmap.Width;
                imageHeader.Height = sshImages[i].bitmap.Height;
                Matrix = ByteUtil.ByteArrayReswap(Matrix, imageHeader);
            }

            stream.Write(Matrix, 0, Matrix.Length);

            //Colour Table
            tempByte = new byte[4];
            BitConverter.GetBytes(0x21).CopyTo(tempByte, 0);
            stream.Write(tempByte, 0, 1);

            tempByte = new byte[4];
            BitConverter.GetBytes(colourTable.colorTable.Count * 4 + 16).CopyTo(tempByte, 0);
            stream.Write(tempByte, 0, 3);

            tempByte = new byte[4];
            BitConverter.GetBytes(colourTable.colorTable.Count).CopyTo(tempByte, 0);
            stream.Write(tempByte, 0, 2);

            tempByte = new byte[4];
            BitConverter.GetBytes(1).CopyTo(tempByte, 0);
            stream.Write(tempByte, 0, 2);

            tempByte = new byte[4];
            BitConverter.GetBytes(colourTable.colorTable.Count).CopyTo(tempByte, 0);
            stream.Write(tempByte, 0, 2);

            if (sshImages[i].sshTable.Format != 0)
            {
                tempByte = new byte[6] { 0x00, 0x00, 0x00, 0x20, 0x00, 0x00 };
                //BitConverter.GetBytes(colourTable.colorTable.Count).CopyTo(tempByte, 0);
            }
            else
            {
                tempByte = new byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            }
            stream.Write(tempByte, 0, tempByte.Length);

            for (int a = 0; a < colourTable.colorTable.Count; a++)
            {
                tempByte = new byte[4];
                int R = colourTable.colorTable[a].R;
                int G = colourTable.colorTable[a].G;
                int B = colourTable.colorTable[a].B;
                int A = colourTable.colorTable[a].A;

                BitConverter.GetBytes(R).CopyTo(tempByte, 0);
                stream.Write(tempByte, 0, 1);
                tempByte = new byte[4];
                BitConverter.GetBytes(G).CopyTo(tempByte, 0);
                stream.Write(tempByte, 0, 1);
                tempByte = new byte[4];
                BitConverter.GetBytes(B).CopyTo(tempByte, 0);
                stream.Write(tempByte, 0, 1);
                tempByte = new byte[4];
                BitConverter.GetBytes(A).CopyTo(tempByte, 0);
                stream.Write(tempByte, 0, 1);
            }
        }

        public void Maxtrix5Write(Stream stream, int i)
        {
            byte[] tempByte;
            SSHColourTable colourTable = new SSHColourTable();
            colourTable.colorTable = new List<Color>();
            //colourTable.colorTable.Add(Color.FromArgb(0, 0, 0, 0));
            for (int y = 0; y < sshImages[i].bitmap.Height; y++)
            {
                for (int x = 0; x < sshImages[i].bitmap.Width; x++)
                {
                    Color color = sshImages[i].bitmap.GetPixel(x, y);
                    tempByte = new byte[4];
                    int R = color.R;
                    int G = color.G;
                    int B = color.B;
                    int A = color.A;
                    if (sshImages[i].AlphaFix)
                    {
                        A = (A + 1) / 2;
                    }
                    BitConverter.GetBytes(R).CopyTo(tempByte, 0);
                    stream.Write(tempByte, 0, 1);
                    tempByte = new byte[4];
                    BitConverter.GetBytes(G).CopyTo(tempByte, 0);
                    stream.Write(tempByte, 0, 1);
                    tempByte = new byte[4];
                    BitConverter.GetBytes(B).CopyTo(tempByte, 0);
                    stream.Write(tempByte, 0, 1);
                    tempByte = new byte[4];
                    BitConverter.GetBytes(A).CopyTo(tempByte, 0);
                    stream.Write(tempByte, 0, 1);
                }
            }
        }
    }


    public struct SSHImage
    {
        public string shortname;
        public string longname;
        public int unknownEnd;
        public int offset;
        public SSHImageHeader sshHeader;
        public byte[] Matrix;
        public SSHColourTable sshTable;
        public bool AlphaFix;
        public bool MetalBin;
        public Bitmap metalBitmap;
        public Bitmap bitmap;
    }
    public struct SSHImageHeader
    {
        public byte MatrixFormat;
        public int Size;
        public int Width;
        public int Height;
        public int Xaxis;
        public int Yaxis;
        public int LXPos;
        public bool flag1;
        public bool flag2;
        public bool flag3;
        public bool flag4;
        public int TYPos;
        public int Mipmaps; //Unit4
    }

    public struct SSHColourTable
    {
        public int Size;
        public int Width;
        public int Height;
        public int Total;
        public int Format;
        public List<Color> colorTable;
    }
}
