using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers
{
    class LOCHandler
    {
        //Whole thing be kinda shit replace later
        public string filePath;
        public byte[] headerBytes;
        public byte[] LOCLHeader;
        public int TextCount;
        public List<int> StringOffsets = new List<int>();
        public List<string> StringList = new List<string>();

        public void ReadLocFile(string path)
        {
            filePath = path;
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                //Find start of file
                long pos = ByteUtil.FindPosition(stream, new byte[] { 0x4C, 0x4F, 0x43, 0x4C });

                //Save Header of file
                stream.Position = 0;
                headerBytes = new byte[pos];
                stream.Read(headerBytes, 0, (int)pos);

                //Save LOCL Header
                stream.Position = pos;
                LOCLHeader = new byte[12];
                stream.Read(LOCLHeader, 0, 12);

                //Save Ammount of Entires
                TextCount = StreamUtil.ReadUInt32(stream);

                //Grab List of Offsets
                for (int i = 0; i < TextCount; i++)
                {
                    int posLoc = StreamUtil.ReadUInt32(stream);
                    StringOffsets.Add(posLoc);
                }

                //Using Offsets Grab Text
                for (int i = 0; i < TextCount; i++)
                {
                    int Length;
                    if (i + 1 >= TextCount)
                    {
                        Length = (int)(stream.Length - stream.Position);
                    }
                    else
                    {
                        Length = StringOffsets[i + 1] - StringOffsets[i];
                    }
                    byte[] byteString = new byte[Length];
                    stream.Read(byteString, 0, Length);
                    byte[] modString;
                    //Check If Blank Entry
                    if (byteString.Length > 5)
                    {
                        modString = new byte[byteString.Length - 4];
                    }
                    else
                    {
                        modString = new byte[byteString.Length];
                    }
                    for (int a = 0; a < byteString.Length - 5; a++)
                    {
                        modString[a] = byteString[a];
                    }
                    string Text = System.Text.Encoding.Unicode.GetString(modString);
                    StringList.Add(Text);
                }
                stream.Dispose();
            }
        }

        public void SaveLocFile(string path = null)
        {
            if(path==null)
            {
                path = filePath;
            }
            Stream stream = new MemoryStream();
            stream.Write(headerBytes, 0, headerBytes.Length);
            stream.Write(LOCLHeader, 0, LOCLHeader.Length);
            StreamUtil.WriteInt32(stream, TextCount);
            //Write Intial Offset
            stream.Write(BitConverter.GetBytes(StringOffsets[0]), 0, 4);

            //Set New Offsets
            for (int i = 0; i < StringList.Count; i++)
            {
                string temp = StringList[i];
                Stream stream1 = new MemoryStream();
                byte[] temp1 = Encoding.Unicode.GetBytes(temp);
                stream1.Write(temp1, 0, temp1.Length);
                int Diff = (int)(stream1.Length) + StringOffsets[i] + 4;
                if (i < StringOffsets.Count - 1)
                {
                    StringOffsets[i + 1] = Diff;
                    stream.Write(BitConverter.GetBytes(Diff), 0, 4);
                }
            }

            //Set strings
            for (int i = 0; i < StringList.Count; i++)
            {
                byte[] temp;
                temp = Encoding.Unicode.GetBytes(StringList[i]);
                stream.Write(temp, 0, temp.Length);
                for (int ai = 0; ai < 4; ai++)
                {
                    stream.WriteByte(0x00);
                }
            }

            //Save File
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            var file = File.Create(path);
            stream.Position = 0;
            stream.CopyTo(file);
            file.Close();
        }

    }

    // LOCH
    // 0-3 Magic Words
    // 4-7 LOCT Offset (Or Size)
    // 8-11 Unknown (Flag? Always 0)
    // 12-15 Unknown 2 (Flag? Always 1)
    // 16-19 LOCL Offset

    // LOCT

    // LOCL
    // 0-3 Magic words
    // 4-7 File Size
    // 8-11 Unknown
    // 12-15 Ammount
    // 16-19 Offset Start
}
