using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers.Models
{
    public class SSXOnTourMPF
    {
        public byte[] magicWords = new byte[4];
        public int NumModels;
        public int HeaderSize;
        public int DataOffset;
        public List<MPFHeader> ModelList = new List<MPFHeader>();

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                magicWords = StreamUtil.ReadBytes(stream, 4);
                NumModels = StreamUtil.ReadInt16(stream);
                HeaderSize = StreamUtil.ReadInt16(stream);
                DataOffset = StreamUtil.ReadInt32(stream);

                ModelList = new List<MPFHeader>();
                for (int i = 0; i < NumModels; i++)
                {
                    var TempHeader = new MPFHeader();
                    TempHeader.ModelName = StreamUtil.ReadString(stream, 16);
                    TempHeader.DataOffset = StreamUtil.ReadInt32(stream);
                    TempHeader.EntrySize = StreamUtil.ReadInt32(stream);
                    TempHeader.U1 = StreamUtil.ReadInt32(stream);
                    TempHeader.U2 = StreamUtil.ReadInt32(stream);
                    TempHeader.U3 = StreamUtil.ReadInt32(stream);
                    TempHeader.U4 = StreamUtil.ReadInt32(stream);
                    TempHeader.U5 = StreamUtil.ReadInt32(stream);
                    TempHeader.U6 = StreamUtil.ReadInt32(stream);
                    TempHeader.U7 = StreamUtil.ReadInt32(stream);
                    TempHeader.U8 = StreamUtil.ReadInt32(stream);
                    TempHeader.U9 = StreamUtil.ReadInt32(stream);
                    TempHeader.U10 = StreamUtil.ReadInt32(stream);

                    TempHeader.U11 = StreamUtil.ReadInt32(stream);
                    TempHeader.U12 = StreamUtil.ReadInt32(stream);
                    TempHeader.U13 = StreamUtil.ReadInt32(stream);
                    TempHeader.U14 = StreamUtil.ReadInt32(stream);

                    TempHeader.UC1 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC2 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC3 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC4 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC5 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC6 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC7 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC8 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC9 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC10 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC11 = StreamUtil.ReadInt16(stream);

                    TempHeader.UC12 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC13 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC14 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC15 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC16 = StreamUtil.ReadInt16(stream);

                    ModelList.Add(TempHeader);
                }


                //Read Matrix And Decompress
                int StartPos = DataOffset;
                for (int i = 0; i < ModelList.Count; i++)
                {
                    stream.Position = StartPos + ModelList[i].DataOffset;
                    int EntrySize = 0;

                    if(i==ModelList.Count-1)
                    {
                        EntrySize = (int)((stream.Length- DataOffset) - ModelList[i].DataOffset);
                    }
                    else
                    {
                        EntrySize = ModelList[i + 1].DataOffset - ModelList[i].DataOffset;
                    }


                    MPFHeader modelHandler = ModelList[i];
                    modelHandler.Matrix = StreamUtil.ReadBytes(stream, EntrySize);
                    modelHandler.Matrix = RefpackHandler.Decompress(modelHandler.Matrix);
                    ModelList[i] = modelHandler;
                }
            }


        }

        public void SaveDecompress(string Path)
        {
            MemoryStream stream = new MemoryStream();

            StreamUtil.WriteBytes(stream, magicWords);
            StreamUtil.WriteInt16(stream, ModelList.Count);
            StreamUtil.WriteInt16(stream, 12);
            StreamUtil.WriteInt32(stream, DataOffset); //FIX OFFSET NUMBER

            for (int i = 0; i < ModelList.Count; i++)
            {
                var TempModel = ModelList[i];

                StreamUtil.WriteString(stream, TempModel.ModelName, 16);

                StreamUtil.WriteInt32(stream, TempModel.DataOffset);
                StreamUtil.WriteInt32(stream, TempModel.EntrySize);
                StreamUtil.WriteInt32(stream, TempModel.U1);
                StreamUtil.WriteInt32(stream, TempModel.U2);
                StreamUtil.WriteInt32(stream, TempModel.U3);
                StreamUtil.WriteInt32(stream, TempModel.U4);
                StreamUtil.WriteInt32(stream, TempModel.U5);
                StreamUtil.WriteInt32(stream, TempModel.U6);
                StreamUtil.WriteInt32(stream, TempModel.U7);
                StreamUtil.WriteInt32(stream, TempModel.U8);
                StreamUtil.WriteInt32(stream, TempModel.U9);
                StreamUtil.WriteInt32(stream, TempModel.U10);
                StreamUtil.WriteInt32(stream, TempModel.U11);
                StreamUtil.WriteInt32(stream, TempModel.U12);
                StreamUtil.WriteInt32(stream, TempModel.U13);
                StreamUtil.WriteInt32(stream, TempModel.U14);

                StreamUtil.WriteInt16(stream, TempModel.UC1);
                StreamUtil.WriteInt16(stream, TempModel.UC2);
                StreamUtil.WriteInt16(stream, TempModel.UC3);
                StreamUtil.WriteInt16(stream, TempModel.UC4);
                StreamUtil.WriteInt16(stream, TempModel.UC5);
                StreamUtil.WriteInt16(stream, TempModel.UC6);
                StreamUtil.WriteInt16(stream, TempModel.UC7);
                StreamUtil.WriteInt16(stream, TempModel.UC8);
                StreamUtil.WriteInt16(stream, TempModel.UC9);
                StreamUtil.WriteInt16(stream, TempModel.UC10);
                StreamUtil.WriteInt16(stream, TempModel.UC11);
                StreamUtil.WriteInt16(stream, TempModel.UC12);
                StreamUtil.WriteInt16(stream, TempModel.UC13);
                StreamUtil.WriteInt16(stream, TempModel.UC14);
                StreamUtil.WriteInt16(stream, TempModel.UC15);
                StreamUtil.WriteInt16(stream, TempModel.UC16);
            }

            StreamUtil.AlignBy16(stream);

            for (int i = 0; i < ModelList.Count; i++)
            {
                StreamUtil.WriteBytes(stream, ModelList[i].Matrix);
                StreamUtil.AlignBy16(stream);
            }


            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
            var file = File.Create(Path);
            stream.Position = 0;
            stream.CopyTo(file);
            stream.Dispose();
            file.Close();
        }

        public struct MPFHeader
        {
            //Main Header//Offsets
            public string ModelName;
            public int DataOffset;
            public int EntrySize;
            public int U1; //Material?
            public int U2; //Bone
            public int U3;
            public int U4;
            public int U5; //Data Offset Start
            public int U6; //Material?
            public int U7;
            public int U8; //Entry Size?
            public int U9;
            public int U10;

            public int U11;
            public int U12;
            public int U13;
            public int U14;

            //Header Counts
            public int UC1;
            public int UC2;
            public int UC3;
            public int UC4;
            public int UC5;
            public int UC6;
            public int UC7;
            public int UC8;
            public int UC9;
            public int UC10;
            public int UC11;

            public int UC12;
            public int UC13;
            public int UC14;
            public int UC15;
            public int UC16;

            public byte[] Matrix;
        }
    }
}
