using BCnEncoder.Decoder;
using BCnEncoder.Encoder;
using SSXMultiTool.Utilities;
using System.IO;
using System.Text;

namespace SSXMultiTool.FileHandlers.Textures
{
    internal class OldXSHHandler
    {
        public string MagicWord; //4
        public int Size;
        public int ImageCount; //Big 4
        public string GVersion;
        public List<XSHImage> xshImage = new List<XSHImage>();
        public string group;
        public string endingstring;

        public void LoadXSH(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                MagicWord = StreamUtil.ReadString(stream, 4);

                if (MagicWord == "SHPX")
                {
                    Size = StreamUtil.ReadUInt32(stream);

                    ImageCount = StreamUtil.ReadUInt32(stream);

                    GVersion = StreamUtil.ReadString(stream, 4);

                    for (int i = 0; i < ImageCount; i++)
                    {
                        XSHImage tempImage = new XSHImage();

                        tempImage.shortname = StreamUtil.ReadString(stream, 4);

                        tempImage.offset = StreamUtil.ReadUInt32(stream);

                        xshImage.Add(tempImage);
                    }

                    group = StreamUtil.ReadString(stream, 8);

                    endingstring = StreamUtil.ReadString(stream, 8);

                    StandardToBitmap(stream);
                }
            }
        }

        public void StandardToBitmap(Stream stream)
        {
            byte[] tempByte;

            for (int i = 0; i < xshImage.Count; i++)
            {
                XSHImage tempImage = xshImage[i];
                XSHImageHeader tempImageHeader = new XSHImageHeader();

                stream.Position = tempImage.offset;

                tempImageHeader.MatrixFormat = StreamUtil.ReadUInt8(stream);

                tempImageHeader.Size = StreamUtil.ReadInt24(stream);

                tempImageHeader.Width = StreamUtil.ReadInt16(stream);

                tempImageHeader.Height = StreamUtil.ReadInt16(stream);

                tempImageHeader.Xaxis = StreamUtil.ReadInt16(stream);

                tempImageHeader.Yaxis = StreamUtil.ReadInt16(stream);

                //Add Other Flags Later
                tempImageHeader.LXPos = StreamUtil.ReadInt16(stream);

                tempImageHeader.TYPos = StreamUtil.ReadInt16(stream);

                tempImageHeader.Matrix = StreamUtil.ReadBytes(stream, tempImageHeader.Size-16);

                BcDecoder bcDecoder = new BcDecoder();

                //96 - BCnEncoder.Shared.CompressionFormat.Bc1
                //97 - BCnEncoder.Shared.CompressionFormat.Bc2
                //109 - Bgra 4444 https://github.com/bartlomiejduda/EA-Graphics-Manager/blob/c9aec00c005437ddbc2752001913e1e2f46840e7/src/EA_Image/ea_image_decoder.py#L289
                //123 - https://github.com/bartlomiejduda/EA-Graphics-Manager/blob/c9aec00c005437ddbc2752001913e1e2f46840e7/src/EA_Image/ea_image_decoder.py#L334
                //125 - BCnEncoder.Shared.CompressionFormat.Bgra

                var Temp = bcDecoder.DecodeRaw(tempImageHeader.Matrix, tempImageHeader.Width, tempImageHeader.Height, BCnEncoder.Shared.CompressionFormat.);

                tempImage.bitmap = new Bitmap(tempImageHeader.Width, tempImageHeader.Height);

                int post = 0;

                for (global::System.Int32 y = 0; y < tempImageHeader.Height; y++)
                {
                    for (global::System.Int32 x = 0; x < tempImageHeader.Width; x++)
                    {
                        tempImage.bitmap.SetPixel(x, y, Color.FromArgb(Temp[post].a,Temp[post].r, Temp[post].g, Temp[post].b));
                        post++;
                    }
                }

                tempImage.bitmap.Save("G:\\SSX\\TRicky Stuff\\SSX Tricky\\data\\textures\\crowd.png");
            }

        }

        public struct XSHImage
        {
            public int offset;
            public int size;
            public string shortname;
            public string longname;
            public List<XSHImageHeader> xshShapeHeader;

            //Converted
            public Bitmap bitmap;
        }

        public struct XSHImageHeader
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

            public byte[] Matrix;
        }
    }
}
