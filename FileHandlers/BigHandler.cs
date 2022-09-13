using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers
{
    class BigHandler
    {
        public BigType bigType;
        public BIGFHeader bigHeader;
        public List<BIGFFiles> bigFiles;
        string bigPath;
        //bool BuildMode;
        public void LoadBig(string path)
        {
            //BuildMode = false;
            bigPath = path;
            bigHeader = new BIGFHeader();
            bigFiles = new List<BIGFFiles>();
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                bigHeader.MagicWords = StreamUtil.ReadString(stream, 4);
                //Figure out what any of these mean
                if (bigHeader.MagicWords == "BIGF")
                {
                    bigType = BigType.BIGF;
                    ReadBigF(stream);
                }
                else if(bigHeader.MagicWords == "BIG4")
                {
                    bigType = BigType.BIG4;
                    ReadBigF(stream);
                }
                else
                {
                    stream.Position = 0;
                    byte[] bytes = new byte[2];
                    stream.Read(bytes, 0, 2);
                    if (bytes[1] == 0xFB)
                    {
                        ReadBigC0FB(stream);
                    }
                    else
                    {
                        MessageBox.Show(bigHeader.MagicWords + " Unknown Big Format");
                    }
                }
                stream.Dispose();
            }
        }

        public void ReadBigF(Stream stream)
        {

            bigHeader.fileSize = StreamUtil.ReadInt32(stream);

            bigHeader.ammount = StreamUtil.ReadInt32Big(stream);

            bigHeader.startOffset = StreamUtil.ReadInt32Big(stream);

            for (int i = 0; i < bigHeader.ammount; i++)
            {
                BIGFFiles temp = new BIGFFiles();

                temp.offset = StreamUtil.ReadInt32Big(stream);

                temp.size = StreamUtil.ReadInt32Big(stream);

                temp.path = StreamUtil.ReadNullEndString(stream);
                bigFiles.Add(temp);
                stream.Position += 1;
            }

            bigHeader.footer = new byte[8];
            stream.Read(bigHeader.footer, 0, bigHeader.footer.Length);

            for (int i = 0; i < bigHeader.ammount; i++)
            {
                stream.Position = bigFiles[i].offset;
                BIGFFiles tempFile = bigFiles[i];
                byte[] bytes = new byte[2];
                stream.Read(bytes, 0, bytes.Length);
                if (bytes[1] == 0xFB)
                {
                    tempFile.Compressed = true;
                    tempFile.UncompressedSize = StreamUtil.ReadInt24Big(stream);
                    bigFiles[i] = tempFile;
                }
            }

        }

        public void ReadBigC0FB(Stream stream)
        {
            bigType = BigType.C0FB;

            bigHeader.startOffset = StreamUtil.ReadInt16Big(stream);

            bigHeader.ammount = StreamUtil.ReadInt16Big(stream);

            for (int i = 0; i < bigHeader.ammount; i++)
            {
                BIGFFiles temp = new BIGFFiles();

                temp.offset = StreamUtil.ReadInt24Big(stream);

                temp.size = StreamUtil.ReadInt24Big(stream);

                temp.path = StreamUtil.ReadNullEndString(stream);
                bigFiles.Add(temp);
                stream.Position += 1;
            }

            for (int i = 0; i < bigHeader.ammount; i++)
            {
                stream.Position = bigFiles[i].offset;
                BIGFFiles tempFile = bigFiles[i];
                byte[] bytes = new byte[2];
                stream.Read(bytes, 0, bytes.Length);
                if (bytes[1] == 0xFB)
                {
                    tempFile.Compressed = true;
                    tempFile.UncompressedSize = StreamUtil.ReadInt24Big(stream);
                    bigFiles[i] = tempFile;
                }
            }
        }

        public void ExtractBig(string path = null)
        {
            using (Stream stream = File.Open(bigPath, FileMode.Open))
            {
                for (int i = 0; i < bigFiles.Count; i++)
                {
                    if (!bigFiles[i].path.Contains("*"))
                    {
                        Stream stream1 = new MemoryStream();
                        byte[] temp = new byte[bigFiles[i].size];
                        stream.Position = bigFiles[i].offset;
                        stream.Read(temp, 0, temp.Length);
                        if (bigFiles[i].Compressed)
                        {
                            RefpackHandler refpackHandler = new RefpackHandler();
                            temp = refpackHandler.Decompress(temp);
                        }
                        stream1.Write(temp, 0, temp.Length);

                        if (!Directory.Exists(Path.GetDirectoryName(path + "//" + bigFiles[i].path)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(path + "//" + bigFiles[i].path));
                        }
                        var file = File.Create(path + "//" + bigFiles[i].path);
                        stream1.Position = 0;
                        stream1.CopyTo(file);
                        file.Close();
                        stream1.Dispose();
                    }
                }
                stream.Dispose();
            }
        }

        public void LoadFolder(string path)
        {
            //BuildMode = true;
            bigType = BigType.BIGF;
            bigPath = path;
            bigHeader = new BIGFHeader();
            bigFiles = new List<BIGFFiles>();
            string[] paths = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            bigHeader.ammount = paths.Length;
            int FileOffset = 16;
            for (int i = 0; i < paths.Length; i++)
            {
                BIGFFiles tempFile = new BIGFFiles();
                tempFile.path = paths[i].Remove(0, path.Length+1).Replace("//",@"\");
                FileOffset += tempFile.path.Length + 9;
                Stream stream = File.OpenRead(paths[i]);
                tempFile.size = (int)stream.Length;
                stream.Dispose();
                bigFiles.Add(tempFile);
            }
            FileOffset += 8;
            for (int i = 0; i < bigFiles.Count; i++)
            {
                BIGFFiles tempFile1 = bigFiles[i];
                tempFile1.offset = FileOffset;
                FileOffset += bigFiles[i].size;
                bigFiles[i] = tempFile1;
            }
        }

        public void LoadFolderC0FB(string path)
        {
            bigHeader = new BIGFHeader();
            bigFiles = new List<BIGFFiles>();
            string[] paths = Directory.GetFiles(bigPath, "*.*", SearchOption.AllDirectories);
            bigHeader.ammount = paths.Length;
            int FileOffset = 6;
            for (int i = 0; i < paths.Length; i++)
            {
                BIGFFiles tempFile = new BIGFFiles();
                tempFile.path = paths[i].Remove(0, bigPath.Length + 1).Replace("//", @"\");
                FileOffset += tempFile.path.Length + 7;
                Stream stream1 = File.OpenRead(paths[i]);
                tempFile.size = (int)stream1.Length;
                stream1.Dispose();
                stream1.Close();
                bigFiles.Add(tempFile);
            }
            //FileOffset += 6;
            for (int i = 0; i < bigFiles.Count; i++)
            {
                BIGFFiles tempFile1 = bigFiles[i];
                tempFile1.offset = FileOffset;
                FileOffset += bigFiles[i].size;
                bigFiles[i] = tempFile1;
            }
        }

        public void BuildBig(string path)
        {
            Stream stream = new MemoryStream();

            if (bigType == BigType.BIGF || bigType == BigType.BIG4)
            {
                var temp = bigType;
                LoadFolder(bigPath);
                if(temp == BigType.BIG4)
                {
                    bigType = BigType.BIG4;
                }
                BuildBigF(stream);
            }
            else if (bigType == BigType.C0FB)
            {
                LoadFolderC0FB(bigPath);
                BuildBigC0FB(stream);
            }
            else
            {
                MessageBox.Show("Unkown format");
            }

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

        public void BuildBigF(Stream stream)
        {
            bigHeader.MagicWords = bigType.ToString();
            byte[] tempByte = new byte[4];
            StreamUtil.WriteString(stream, bigHeader.MagicWords);

            // Set File Size Later
            tempByte = new byte[4];
            stream.Write(tempByte, 0, tempByte.Length);

            //Set Ammount
            StreamUtil.WriteInt32Big(stream, bigHeader.ammount);

            //Set Blank Start of file offset
            tempByte = new byte[4];
            stream.Write(tempByte, 0, tempByte.Length);

            for (int i = 0; i < bigFiles.Count; i++)
            {
                //Write offset
                StreamUtil.WriteInt32Big(stream, bigFiles[i].offset);

                //Write size
                StreamUtil.WriteInt32Big(stream, bigFiles[i].size);

                //Write Path
                StreamUtil.WriteNullString(stream, bigFiles[i].path);
            }
            //Write Footer
            tempByte = new byte[8];
            Encoding.ASCII.GetBytes("L222").CopyTo(tempByte, 0);
            stream.Write(tempByte, 0, tempByte.Length);

            //Set File start offset
            long pos = stream.Position;
            stream.Position = 12;

            StreamUtil.WriteInt32Big(stream, (int)pos);

            stream.Position = stream.Length;

            //Write Files
            for (int i = 0; i < bigFiles.Count; i++)
            {
                using (Stream stream1 = File.Open(bigPath + "\\" + bigFiles[i].path, FileMode.Open))
                {
                    tempByte = new byte[stream1.Length];
                    stream1.Read(tempByte, 0, tempByte.Length);
                    stream.Write(tempByte, 0, tempByte.Length);
                }
            }

            //Set filesize
            stream.Position = 4;

            StreamUtil.WriteInt32Big(stream, (int)stream.Length);
        }

        public void BuildBigC0FB(Stream stream)
        {
            byte[] tempByte = new byte[2] {0xC0,0xFB};
            stream.Write(tempByte, 0, tempByte.Length);

            //Set Blank Start of file offset
            tempByte = new byte[2];
            stream.Write(tempByte, 0, tempByte.Length);

            //Set Ammount
            StreamUtil.WriteInt16Big(stream, bigHeader.ammount);

            for (int i = 0; i < bigFiles.Count; i++)
            {
                //Write offset
                StreamUtil.WriteInt24Big(stream, bigFiles[i].offset);

                //Write size
                StreamUtil.WriteInt24Big(stream, bigFiles[i].size);

                //Write Path
                StreamUtil.WriteNullString(stream, bigFiles[i].path.Replace(@"\",@"/"));
            }

            //Set File start offset
            long pos = stream.Position;
            stream.Position = 2;

            StreamUtil.WriteInt16Big(stream, (int)pos);

            stream.Position = stream.Length;

            //Write Files
            for (int i = 0; i < bigFiles.Count; i++)
            {
                using (Stream stream1 = File.Open(bigPath + "\\" + bigFiles[i].path, FileMode.Open))
                {
                    tempByte = new byte[stream1.Length];
                    stream1.Read(tempByte, 0, tempByte.Length);
                    stream.Write(tempByte, 0, tempByte.Length);
                }
            }
        }
    }

//00-03 - Magic Word
//04-07 - file size (little endian)
//08-0b - Ammount
//0c-0f - Start of file Offset
//Start of file listings
//10-13 - offset
//14-17 - Size
//17 - file path
//80 ish blank bytes after each file (Done to make the file an even number?)

    struct BIGFHeader
    {
        public string MagicWords;
        public int fileSize;
        public int ammount;
        public int startOffset;
        public bool compression;
        public byte[] footer;
    }

    struct BIGFFiles
    {
        public string path;
        public bool Compressed;
        public int size;
        public int offset;
        public int UncompressedSize;
    }

    enum BigType
    {
        BIGF,
        C0FB,
        BIG4
    }
}
