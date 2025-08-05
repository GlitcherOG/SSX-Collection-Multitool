using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.FileHandlers.Textures;

namespace SSXMultiTool.Utilities
{
    public class ByteUtil
    {
        public static int ByteToBitConvert(byte Byte, int Start = 0, int End = 3)
        {
            byte[] array = new byte[1] { Byte };
            var bits = new BitArray(array);
            int Point = 1;
            int Number = 0;
            for (int i = Start; i < End + 1; i++)
            {
                Number += (bits[i] ? 1 : 0) * Point;
                Point = Point * 2;
            }
            return Number;
        }
        
        public static int BytesToBitConvert(byte[] Byte, int Start, int End)
        {
            byte[] array =  Byte;
            Array.Reverse(Byte);
            var bits = new BitArray(array);
            int Point = 1;
            int Number = 0;
            for (int i = Start; i < End + 1; i++)
            {
                Number += (bits[i] ? 1 : 0) * Point;
                Point = Point * 2;
            }
            return Number;
        }

        public static byte[] FlipBytes(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                byte[] tempbyte = new byte[1];

                tempbyte[0] = bytes[i];

                var bits = new BitArray(tempbyte);

                int length = bits.Length;
                int mid = (length / 2);

                for (int a = 0; a < mid; a++)
                {
                    bool bit = bits[a];
                    bits[i] = bits[length - a - 1];
                    bits[length - a - 1] = bit;
                }

                bits.CopyTo(tempbyte, 0);

                bytes[i] = tempbyte[0];
            }

            return bytes;
        }

        public static int BitConbineConvert(byte OneByte, byte TwoByte, int StartPoint = 0, int Length = 4, int Inset=4)
        {
            byte[] arrayOne = new byte[1] { OneByte };
            var bitsOne = new BitArray(arrayOne);
            arrayOne = new byte[1] { TwoByte };
            var bitsTwo = new BitArray(arrayOne);

            for (int i = StartPoint; i < Length; i++)
            {
                bitsOne[Inset + i] = bitsTwo[i];
            }


            int Point = 1;
            int Number = 0;

            for (int i = 0; i < 8; i++)
            {
                Number += (bitsOne[i] ? 1 : 0) * Point;
                Point = Point * 2;
            }

            return Number;
        }

        public static long FindPosition(Stream stream, byte[] byteSequence, long Start = -1, long End = -1)
        {
            int b;
            long i = 0;
            if (Start != -1)
            {
                stream.Position = Start;
            }
            while ((b = stream.ReadByte()) != -1)
            {
                if (End != -1)
                {
                    if (End <= stream.Position)
                    {
                        return -1;
                    }
                }
                if (b == byteSequence[i++])
                {
                    if (i == byteSequence.Length)
                    {
                        return stream.Position - byteSequence.Length;
                    }
                }
                else
                {
                    i = b == byteSequence[0] ? 1 : 0;
                }
            }
            return -1;
        }

        public static int simulateSwitching4th5thBit(int nr)
        {
            bool bit4 = (nr % 16) / 8 >= 1;
            bool bit5 = (nr % 32) / 16 >= 1;
            if (bit4 && !bit5)
            {
                return nr + 8;
            }
            if (!bit4 && bit5)
            {
                return nr - 8;
            }
            else
            {
                return nr;
            }
        }

        public static int ByteBitSwitch(int Byte, int Bit1 = 3, int Bit2 = 4)
        {
            byte[] array = new byte[1] { (byte)Byte };
            var bits = new BitArray(array);
            bool temp1 = bits[Bit2];
            bits[Bit2] = bits[Bit1];
            bits[Bit1] = temp1;
            int Number = 0;
            int Point = 1;
            for (int i = 0; i < 8; i++)
            {
                Number += (bits[i] ? 1 : 0) * Point;
                Point = Point * 2;
            }
            return Number;
        }

        public static int BytesBitSwitch(byte[] Bytes, int Bit1 = 3, int Bit2 = 4)
        {
            byte[] array = Bytes;
            var bits = new BitArray(array);
            bool temp1 = bits[Bit2];
            bits[Bit2] = bits[Bit1];
            bits[Bit1] = temp1;
            int Number = 0;
            int Point = 1;
            for (int i = 0; i < bits.Length; i++)
            {
                Number += (bits[i] ? 1 : 0) * Point;
                Point = Point * 2;
            }
            return Number;
        }

        public static byte[] ByteArraySwap(byte[] Bytes, OldSSHHandler.SSHImageHeader tempImageHeader)
        {
            int x = 0;
            int oldx = 0;
            int y = 0;
            int MatrixPoint = 0;
            bool flip = false;
            int post = 0;

            //Unscramble lines
            byte[,] MatrixRedo = new byte[tempImageHeader.Height, tempImageHeader.Width];

            for (int a = 0; a < tempImageHeader.Height * (tempImageHeader.Width / 16); a++)
            {
                byte[] matrixNew = new byte[16];
                for (int b = 0; b < 16; b++)
                {
                    matrixNew[b] = Bytes[MatrixPoint];
                    MatrixPoint++;
                }
                for (int b = 0; b < 16; b++)
                {
                    MatrixRedo[y, x] = matrixNew[b];
                    post++;
                    x++;
                }
                if (!flip)
                {
                    flip = true;
                    oldx = x;
                    x -= 16;
                    y++;
                }
                else
                {
                    flip = false;
                    x = oldx;
                    y--;
                }
                if (x >= tempImageHeader.Width)
                {
                    x = 0;
                    y += 2;
                }
                if (y >= tempImageHeader.Height)
                {
                    break;
                }
            }

            //Every second byte swap position
            x = 0;
            oldx = 0;
            y = 0;
            flip = true;
            bool flipTemp = true;
            for (int a = 0; a < tempImageHeader.Height / 2; a++)
            {
                for (int b = 0; b < tempImageHeader.Width; b++)
                {
                    if (oldx != 0 && flip == flipTemp)
                    {
                        byte holder = MatrixRedo[y, x];
                        MatrixRedo[y, x] = MatrixRedo[y + 1, x - 1];
                        MatrixRedo[y + 1, x - 1] = holder;
                    }
                    if (oldx != 0)
                    {
                        flip = !flip;
                    }
                    x++;
                    oldx++;
                    if (oldx == 16)
                    {
                        flipTemp = !flipTemp;
                        oldx = 0;
                    }
                }
                x = 0;
                oldx = 0;
                y += 2;
            }

            // Swap every few lines
            int LevelCheck = 1;
            byte Holder;
            for (int i = 0; i < tempImageHeader.Height; i++)
            {
                if(LevelCheck==2)
                {
                    LevelCheck = 0;
                    for (int a = 0; a < tempImageHeader.Width; a++)
                    {
                        Holder = MatrixRedo[i+1,a];
                        MatrixRedo[i + 1, a] = MatrixRedo[i, a];
                        MatrixRedo[i, a] = Holder;
                    }
                    i += 2;
                }
                LevelCheck++;
            }

            LevelCheck = 3;
            flip = true;
            // Swap 16 bytes v1 and v2
            for (int a = 0; a < tempImageHeader.Height; a++)
            {
                for (int i = 0; i < tempImageHeader.Width / 16; i++)
                {
                    int b = i * 16;
                    byte[] bytes = new byte[16];
                    if (flip)
                    {
                        bytes[1 - 1] = MatrixRedo[a, b + 1 - 1];
                        bytes[2 - 1] = MatrixRedo[a, b + 5 - 1];
                        bytes[3 - 1] = MatrixRedo[a, b + 9 - 1];
                        bytes[4 - 1] = MatrixRedo[a, b + 13 - 1];
                        bytes[5 - 1] = MatrixRedo[a, b + 2 - 1];
                        bytes[6 - 1] = MatrixRedo[a, b + 6 - 1];
                        bytes[7 - 1] = MatrixRedo[a, b + 10 - 1];
                        bytes[8 - 1] = MatrixRedo[a, b + 14 - 1];
                        bytes[9 - 1] = MatrixRedo[a, b + 3 - 1];
                        bytes[10 - 1] = MatrixRedo[a, b + 7 - 1];
                        bytes[11 - 1] = MatrixRedo[a, b + 11 - 1];
                        bytes[12 - 1] = MatrixRedo[a, b + 15 - 1];
                        bytes[13 - 1] = MatrixRedo[a, b + 4 - 1];
                        bytes[14 - 1] = MatrixRedo[a, b + 8 - 1];
                        bytes[15 - 1] = MatrixRedo[a, b + 12 - 1];
                        bytes[16 - 1] = MatrixRedo[a, b + 16 - 1];
                    }
                    else
                    {
                        bytes[1 - 1] = MatrixRedo[a, b + 2 - 1];
                        bytes[2 - 1] = MatrixRedo[a, b + 6 - 1];
                        bytes[3 - 1] = MatrixRedo[a, b + 10 - 1];
                        bytes[4 - 1] = MatrixRedo[a, b + 14 - 1];
                        bytes[5 - 1] = MatrixRedo[a, b + 1 - 1];
                        bytes[6 - 1] = MatrixRedo[a, b + 5 - 1];
                        bytes[7 - 1] = MatrixRedo[a, b + 9 - 1];
                        bytes[8 - 1] = MatrixRedo[a, b + 13 - 1];
                        bytes[9 - 1] = MatrixRedo[a, b + 4 - 1];
                        bytes[10 - 1] = MatrixRedo[a, b + 8 - 1];
                        bytes[11 - 1] = MatrixRedo[a, b + 12 - 1];
                        bytes[12 - 1] = MatrixRedo[a, b + 16 - 1];
                        bytes[13 - 1] = MatrixRedo[a, b + 3 - 1];
                        bytes[14 - 1] = MatrixRedo[a, b + 7 - 1];
                        bytes[15 - 1] = MatrixRedo[a, b + 11 - 1];
                        bytes[16 - 1] = MatrixRedo[a, b + 15 - 1];
                    }

                    for (int c = 0; c < bytes.Length; c++)
                    {
                        MatrixRedo[a, b + c] = bytes[c];
                    }
                }
                LevelCheck++;
                if (LevelCheck == 5)
                {
                    LevelCheck = 1;
                    flip = !flip;
                }
            }
            byte[] Buffer = new byte[MatrixRedo.Length];
            int pos = 0;
            for (int a = 0; a < tempImageHeader.Height; a++)
            {
                for (int b = 0; b < tempImageHeader.Width; b++)
                {
                    Buffer[pos] = MatrixRedo[a, b];
                    pos++;
                }
            }


            return Buffer;
        }

        public static byte[] ByteArrayReswap(byte[] Bytes, OldSSHHandler.SSHImageHeader tempImageHeader)
        {
            byte[] Matrix = new byte[tempImageHeader.Height * tempImageHeader.Width];
            byte[,] MatrixRedo = new byte[tempImageHeader.Height, tempImageHeader.Width];
            int pos = 0;
            for (int a = 0; a < tempImageHeader.Height; a++)
            {
                for (int b = 0; b < tempImageHeader.Width; b++)
                {
                    MatrixRedo[a, b] = Bytes[pos];
                    pos++;
                }
            }

            bool flip = true;
            int LevelCheck = 3;

            // Swap 16 bytes v1 and v2
            for (int a = 0; a < tempImageHeader.Height; a++)
            {
                for (int i = 0; i < tempImageHeader.Width / 16; i++)
                {
                    int b = i * 16;
                    byte[] bytes = new byte[16];
                    if (flip)
                    {
                        bytes[1 - 1] = MatrixRedo[a, b + 1 - 1];
                        bytes[5 - 1] = MatrixRedo[a, b + 2 - 1];
                        bytes[9 - 1] = MatrixRedo[a, b + 3 - 1];
                        bytes[13 - 1] = MatrixRedo[a, b + 4 - 1];
                        bytes[2 - 1] = MatrixRedo[a, b + 5 - 1];
                        bytes[6 - 1] = MatrixRedo[a, b + 6 - 1];
                        bytes[10 - 1] = MatrixRedo[a, b + 7 - 1];
                        bytes[14 - 1] = MatrixRedo[a, b + 8 - 1];
                        bytes[3 - 1] = MatrixRedo[a, b + 9 - 1];
                        bytes[7 - 1] = MatrixRedo[a, b + 10 - 1];
                        bytes[11 - 1] = MatrixRedo[a, b + 11 - 1];
                        bytes[15 - 1] = MatrixRedo[a, b + 12 - 1];
                        bytes[4 - 1] = MatrixRedo[a, b + 13 - 1];
                        bytes[8 - 1] = MatrixRedo[a, b + 14 - 1];
                        bytes[12 - 1] = MatrixRedo[a, b + 15 - 1];
                        bytes[16 - 1] = MatrixRedo[a, b + 16 - 1];
                    }
                    else
                    {
                        bytes[2 - 1] = MatrixRedo[a, b + 1 - 1];
                        bytes[6 - 1] = MatrixRedo[a, b + 2 - 1];
                        bytes[10 - 1] = MatrixRedo[a, b + 3 - 1];
                        bytes[14 - 1] = MatrixRedo[a, b + 4 - 1];
                        bytes[1 - 1] = MatrixRedo[a, b + 5 - 1];
                        bytes[5 - 1] = MatrixRedo[a, b + 6 - 1];
                        bytes[9 - 1] = MatrixRedo[a, b + 7 - 1];
                        bytes[13 - 1] = MatrixRedo[a, b + 8 - 1];
                        bytes[4 - 1] = MatrixRedo[a, b + 9 - 1];
                        bytes[8 - 1] = MatrixRedo[a, b + 10 - 1];
                        bytes[12 - 1] = MatrixRedo[a, b + 11 - 1];
                        bytes[16 - 1] = MatrixRedo[a, b + 12 - 1];
                        bytes[3 - 1] = MatrixRedo[a, b + 13 - 1];
                        bytes[7 - 1] = MatrixRedo[a, b + 14 - 1];
                        bytes[11 - 1] = MatrixRedo[a, b + 15 - 1];
                        bytes[15 - 1] = MatrixRedo[a, b + 16 - 1];
                    }

                    for (int c = 0; c < bytes.Length; c++)
                    {
                        MatrixRedo[a, b + c] = bytes[c];
                    }
                }
                LevelCheck++;
                if (LevelCheck == 5)
                {
                    LevelCheck = 1;
                    flip = !flip;
                }
            }

            // Swap every few lines
            LevelCheck = 1;
            byte Holder;
            for (int i = 0; i < tempImageHeader.Height; i++)
            {
                if (LevelCheck == 2)
                {
                    LevelCheck = 0;
                    for (int a = 0; a < tempImageHeader.Width; a++)
                    {
                        Holder = MatrixRedo[i + 1, a];
                        MatrixRedo[i + 1, a] = MatrixRedo[i, a];
                        MatrixRedo[i, a] = Holder;
                    }
                    i += 2;
                }
                LevelCheck++;
            }

            //Every second byte swap position
            int x = 0;
            int oldx = 0;
            int y = 0;
            flip = true;
            bool flipTemp = true;
            for (int a = 0; a < tempImageHeader.Height / 2; a++)
            {
                for (int b = 0; b < tempImageHeader.Width; b++)
                {
                    if (oldx != 0 && flip == flipTemp)
                    {
                        byte holder = MatrixRedo[y, x];
                        MatrixRedo[y, x] = MatrixRedo[y + 1, x - 1];
                        MatrixRedo[y + 1, x - 1] = holder;
                    }
                    if (oldx != 0)
                    {
                        flip = !flip;
                    }
                    x++;
                    oldx++;
                    if (oldx == 16)
                    {
                        flipTemp = !flipTemp;
                        oldx = 0;
                    }
                }
                x = 0;
                oldx = 0;
                y += 2;
            }

            x = 0;
            oldx = 0;
            y = 0;
            int x1 = 0;
            int y1 = 0;
            flip = false;

            byte[,] MatrixRedo1 = new byte[tempImageHeader.Height, tempImageHeader.Width];

            for (int a = 0; a < tempImageHeader.Height * (tempImageHeader.Width / 16); a++)
            {
                byte[] matrixNew = new byte[16];
                for (int b = 0; b < 16; b++)
                {
                    matrixNew[b] = MatrixRedo[y,x];
                    x++;
                }

                if (!flip)
                {
                    flip = true;
                    y++;
                    oldx = x;
                    x -= 16;
                }
                else
                {
                    flip = false;
                    y--;
                    x= oldx;
                }
                if (x >= tempImageHeader.Width)
                {
                    x = 0;
                    y += 2;
                }

                for (int b = 0; b < 16; b++)
                {
                    MatrixRedo1[y1, x1] = matrixNew[b];
                    x1++;
                    if (x1 == tempImageHeader.Width)
                    {
                        y1++;
                        x1 = 0;
                    }
                }
                if (y1 >= tempImageHeader.Height)
                {
                    break;
                }
            }

            pos = 0;
            for (int a = 0; a < tempImageHeader.Height; a++)
            {
                for (int b = 0; b < tempImageHeader.Width; b++)
                {
                    Matrix[pos] = MatrixRedo1[a,b];
                    pos++;
                }
            }

            return Matrix;
        }

        public static byte[] Swizzle8(byte[] buf, int width, int height)
        {
            byte[] output = new byte[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Swizzle mapping math (same as in Unswizzle)
                    int blockLocation = (y & ~0xf) * width + (x & ~0xf) * 2;
                    int swapSelector = (((y + 2) >> 2) & 0x1) * 4;
                    int posY = (((y & ~3) >> 1) + (y & 1)) & 0x7;
                    int columnLocation = posY * width * 2 + ((x + swapSelector) & 0x7) * 4;
                    int byteNum = ((y >> 1) & 1) + ((x >> 2) & 2);
                    int swizzleId = blockLocation + columnLocation + byteNum;

                    // Now swizzle: copy from linear buf into swizzled output
                    if (swizzleId < output.Length && y * width + x < buf.Length)
                    {
                        output[swizzleId] = buf[y * width + x];
                    }
                }
            }

            return output;
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

        public static byte[] Unswizzle4bpp(byte[] pInTexels, int width, int height)
        {
            byte[] pSwizTexels = new byte[width * height / 2];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;

                    // Unswizzling calculations
                    int pageX = x & (~0x7f);
                    int pageY = y & (~0x7f);

                    int pages_horz = (width + 127) / 128;
                    int pages_vert = (height + 127) / 128;

                    int page_number = (pageY / 128) * pages_horz + (pageX / 128);

                    int page32Y = (page_number / pages_vert) * 32;
                    int page32X = (page_number % pages_vert) * 64;

                    int page_location = page32Y * height * 2 + page32X * 4;

                    int locX = x & 0x7f;
                    int locY = y & 0x7f;

                    int block_location = ((locX & (~0x1f)) >> 1) * height + (locY & (~0xf)) * 2;
                    int swap_selector = (((y + 2) >> 2) & 0x1) * 4;
                    int posY = (((y & (~3)) >> 1) + (y & 1)) & 0x7;

                    int column_location = posY * height * 2 + ((x + swap_selector) & 0x7) * 4;

                    int byte_num = (x >> 3) & 3;
                    int bits_set = (y >> 1) & 1;

                    int pos = page_location + block_location + column_location + byte_num;

                    if (pos < pInTexels.Length)
                    {
                        int uPen;
                        if ((bits_set & 1) != 0)
                        {
                            uPen = (pInTexels[pos] >> 4) & 0xf;
                        }
                        else
                        {
                            uPen = pInTexels[pos] & 0xf;
                        }

                        int byteIndex = index >> 1;
                        byte pix = pSwizTexels[byteIndex];

                        if ((index & 1) != 0)
                        {
                            pSwizTexels[byteIndex] = (byte)(((uPen << 4) & 0xf0) | (pix & 0x0f));
                        }
                        else
                        {
                            pSwizTexels[byteIndex] = (byte)((pix & 0xf0) | (uPen & 0x0f));
                        }
                    }
                }
            }

            return pSwizTexels;
        }

        public static byte[] SwizzlePalette(byte[] palBuffer, int width)
        {
            byte[] swizzledPal = new byte[1024];

            for (int p = 0; p < width; p++)
            {
                int pos = (p & 231) + ((p & 8) << 1) + ((p & 16) >> 1); // same swizzle index
                int destIndex = p * 4;
                int srcIndex = pos * 4;

                Array.Copy(palBuffer, srcIndex, swizzledPal, destIndex, 4);
            }

            return swizzledPal;
        }


        public static byte[] UnswizzlePalette(byte[] palBuffer, int width)
        {
            byte[] newPal = new byte[1024];
            for (int p = 0; p < width; p++)
            {
                int pos = (p & 231) + ((p & 8) << 1) + ((p & 16) >> 1);
                int srcIndex = p * 4;
                int destIndex = pos * 4;

                Array.Copy(palBuffer, srcIndex, newPal, destIndex, 4);
            }
            return newPal;
        }
        public static float UintByteToFloat(int Int)
        {
            byte[] bytes = BitConverter.GetBytes(Int);
            return BitConverter.ToSingle(bytes, 0);
        }
    }
}
//public struct Point
//{
//    public int x;
//    public int y;
//}
/*Point decodePixelLocation(Dimension imageDimensions, Dimension imageBlockDimensions, Point pxlLocation)
{
    final int pixelLocation = pxlLocation.x * imageDimensions.width + pxlLocation.y;

    int result = pixelLocation;
    if (!rowBit1EqualsRowBit2(imageDimensions, result))
    {
        result = toggleBit(result, 2);  // info += " -neq r1,r2 ? ^c2-> " + asBin(result); // 4,0 -> 4,16 (^b2 <<2)
    }
    result = swapRowBit0Bit1(imageDimensions, result); // info += " -Sw Ro 0,1-> " + asBin(result); // 1,0 -> 2,0

    result = rotateColumnBlockBitsLeft(imageBlockDimensions, result);  // info += " -RL1 c0_c3-> " + asBin(result); // 0,32 -> 32,0

    result = rotateColumnWithOneBitOfRowLeft(imageDimensions, result);  // info += " -RL1 c0_r1-> " + asBin(result); // 0,32 -> 32,0
    return new Point(result / imageDimensions.width, result % imageDimensions.width);
}
*/