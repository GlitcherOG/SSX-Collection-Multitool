using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SSXMultiTool.FileHandlers
{
    internal class SSX3MPFModelHandler
    {
        public byte[] magicWords = new byte[4];
        public int NumModels;
        public int HeaderSize;
        public int DataOffset;
        public List<MPFModelHeader> ModelList = new List<MPFModelHeader>();

        public void load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                magicWords = StreamUtil.ReadBytes(stream, 4);
                NumModels = StreamUtil.ReadInt16(stream);
                HeaderSize = StreamUtil.ReadInt16(stream);
                DataOffset = StreamUtil.ReadInt32(stream);
                for (int i = 0; i < NumModels; i++)
                {
                    MPFModelHeader modelHeader = new MPFModelHeader();

                    int Test = StreamUtil.ReadInt32(stream);
                    if (Test == 0)
                    {
                        ModelList.Add(modelHeader);
                        break;
                    }
                    else
                    {
                        stream.Position -= 4;
                    }

                    modelHeader.ModelName = StreamUtil.ReadString(stream, 16).Replace("\0", "");
                    modelHeader.DataOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.EntrySize = StreamUtil.ReadInt32(stream);
                    modelHeader.BoneOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.U7 = StreamUtil.ReadInt32(stream);
                    modelHeader.U8 = StreamUtil.ReadInt32(stream);
                    modelHeader.U9 = StreamUtil.ReadInt32(stream);
                    modelHeader.U10 = StreamUtil.ReadInt32(stream);
                    modelHeader.U11 = StreamUtil.ReadInt32(stream);
                    modelHeader.U12 = StreamUtil.ReadInt32(stream);
                    modelHeader.U13 = StreamUtil.ReadInt32(stream);
                    modelHeader.U14 = StreamUtil.ReadInt32(stream);
                    modelHeader.U15 = StreamUtil.ReadInt32(stream);
                    modelHeader.U16 = StreamUtil.ReadInt32(stream);

                    modelHeader.U17 = StreamUtil.ReadInt16(stream);
                    modelHeader.U18 = StreamUtil.ReadInt16(stream);
                    modelHeader.U19 = StreamUtil.ReadInt16(stream);
                    modelHeader.U20 = StreamUtil.ReadInt16(stream);
                    modelHeader.U21 = StreamUtil.ReadInt16(stream);
                    modelHeader.U22 = StreamUtil.ReadInt16(stream);
                    modelHeader.U23 = StreamUtil.ReadInt16(stream);
                    modelHeader.U24 = StreamUtil.ReadInt16(stream);
                    modelHeader.U25 = StreamUtil.ReadInt16(stream);
                    modelHeader.U26 = StreamUtil.ReadInt16(stream);
                    modelHeader.U27 = StreamUtil.ReadInt16(stream);
                    modelHeader.U28 = StreamUtil.ReadInt16(stream);
                    modelHeader.U29 = StreamUtil.ReadInt16(stream);
                    modelHeader.U30 = StreamUtil.ReadInt16(stream);

                    ModelList.Add(modelHeader);
                }

                //Read Matrix And Decompress
                int StartPos = DataOffset;
                for (int i = 0; i < ModelList.Count; i++)
                {
                    stream.Position = StartPos + ModelList[i].DataOffset;

                    MPFModelHeader modelHandler = ModelList[i];
                    modelHandler.Matrix = StreamUtil.ReadBytes(stream, ModelList[i].EntrySize);
                    modelHandler.Matrix = RefpackHandler.Decompress(modelHandler.Matrix);
                    ModelList[i] = modelHandler;
                }
            }
        }


        public void Save(string path)
        {
            Stream stream = new MemoryStream();
            StreamUtil.WriteBytes(stream, magicWords);
            StreamUtil.WriteInt16(stream, NumModels);
            StreamUtil.WriteInt16(stream, HeaderSize);
            StreamUtil.WriteInt32(stream, DataOffset);

            for (int i = 0; i < 1; i++)
            {
                StreamUtil.WriteString(stream, ModelList[i].ModelName, 16);

                StreamUtil.WriteInt32(stream, ModelList[i].DataOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].EntrySize);
                StreamUtil.WriteInt32(stream, ModelList[i].BoneOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].U7);
                StreamUtil.WriteInt32(stream, ModelList[i].U8);
                StreamUtil.WriteInt32(stream, ModelList[i].U9);
                StreamUtil.WriteInt32(stream, ModelList[i].U10);
                StreamUtil.WriteInt32(stream, ModelList[i].U11);
                StreamUtil.WriteInt32(stream, ModelList[i].U12);
                StreamUtil.WriteInt32(stream, ModelList[i].U13);
                StreamUtil.WriteInt32(stream, ModelList[i].U14);
                StreamUtil.WriteInt32(stream, ModelList[i].U15);
                StreamUtil.WriteInt32(stream, ModelList[i].U16);

                StreamUtil.WriteInt16(stream, ModelList[i].U17);
                StreamUtil.WriteInt16(stream, ModelList[i].U18);
                StreamUtil.WriteInt16(stream, ModelList[i].U19);
                StreamUtil.WriteInt16(stream, ModelList[i].U20);
                StreamUtil.WriteInt16(stream, ModelList[i].U21);
                StreamUtil.WriteInt16(stream, ModelList[i].U22);
                StreamUtil.WriteInt16(stream, ModelList[i].U23);
                StreamUtil.WriteInt16(stream, ModelList[i].U24);
                StreamUtil.WriteInt16(stream, ModelList[i].U25);
                StreamUtil.WriteInt16(stream, ModelList[i].U26);
                StreamUtil.WriteInt16(stream, ModelList[i].U27);
                StreamUtil.WriteInt16(stream, ModelList[i].U28);
                StreamUtil.WriteInt16(stream, ModelList[i].U29);
                StreamUtil.WriteInt16(stream, ModelList[i].U30);
            }
            StreamUtil.AlignBy16(stream);


            for (int i = 0; i < 1; i++)
            {
                //Save current pos go back and set start pos
                stream.Position = DataOffset + ModelList[i].DataOffset;
                //Write Matrix
                StreamUtil.WriteBytes(stream, ModelList[i].Matrix);
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


        public struct MPFModelHeader
        {
            //Main Header
            public string ModelName;
            public int DataOffset;
            public int EntrySize;
            public int BoneOffset; 
            public int U7; //Weight Info
            public int U8; //Material Groups
            public int U9; //Mesh Data Start
            public int U10; //Material Offset
            public int U11; //Weight Info 2
            public int U12; //Weight Refrence List
            public int U13; //Weight Info 3

            //Unused ??
            public int U14;
            public int U15; 
            public int U16;

            //Counts
            public int U17;
            public int U18;
            public int U19;
            public int U20; //Bone Count
            public int U21;
            public int U22; //Morph Key
            public int U23;
            public int U24;
            public int U25;

            //Unused?
            public int U26;
            public int U27;
            public int U28;
            public int U29;
            public int U30;

            public byte[] Matrix;
        }
    }
}
