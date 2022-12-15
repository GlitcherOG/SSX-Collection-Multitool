using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    modelHeader.NameOffset = StreamUtil.ReadInt32(stream);
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
                    modelHeader.BodyObjectsCount = StreamUtil.ReadInt16(stream);
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
                for (int i = 0; i < ModelList.Count - 1; i++)
                {
                    stream.Position = StartPos + ModelList[i].DataOffset;
                    int EndPos = 0;
                    if (i == ModelList.Count - 2)
                    {
                        EndPos = ((int)stream.Length - StartPos) - ModelList[i].DataOffset;
                    }
                    else
                    {
                        EndPos = ModelList[i + 1].DataOffset - ModelList[i].DataOffset;
                    }

                    MPFModelHeader modelHandler = ModelList[i];
                    modelHandler.Matrix = StreamUtil.ReadBytes(stream, EndPos);
                    RefpackHandler refpackHandler = new RefpackHandler();
                    modelHandler.Matrix = refpackHandler.Decompress(modelHandler.Matrix);
                    ModelList[i] = modelHandler;
                }
            }

            ////Read Matrix Data
            //for (int i = 0; i < ModelList.Count - 1; i++)
            //{
            //    Stream streamMatrix = new MemoryStream();
            //    var Model = ModelList[i];
            //    streamMatrix.Write(ModelList[i].Matrix, 0, ModelList[i].Matrix.Length);

            //    //U7 IDK
            //    streamMatrix.Position = Model.U7;
            //    List<MPFUnkownArray1> TempArrayListU7 = new List<MPFUnkownArray1>();
            //    for (int a = 0; a < Model.U17; a++)
            //    {
            //        MPFUnkownArray1 TempArray = new MPFUnkownArray1();
            //        TempArray.Count = StreamUtil.ReadInt32(streamMatrix);
            //        TempArray.StartOffset = StreamUtil.ReadInt32(streamMatrix);
            //        TempArray.EndOffset = StreamUtil.ReadInt32(streamMatrix);
            //        long Position = streamMatrix.Position;

            //        //Read Ints
            //        TempArray.IntList = new List<int>();
            //        streamMatrix.Position = TempArray.StartOffset;
            //        for (int b = 0; b < TempArray.Count; b++)
            //        {
            //            TempArray.IntList.Add(StreamUtil.ReadInt32(streamMatrix));
            //        }
            //        streamMatrix.Position = Position;
            //        TempArrayListU7.Add(TempArray);
            //    }
            //    Model.U7UnkownArray1 = TempArrayListU7;

            //    //U12
            //    streamMatrix.Position = Model.U12;
            //    List<MPFUnkownArray1> TempArrayListU12 = new List<MPFUnkownArray1>();
            //    for (int a = 0; a < 3; a++)
            //    {
            //        MPFUnkownArray1 TempArray = new MPFUnkownArray1();
            //        TempArray.Count = StreamUtil.ReadInt32(streamMatrix);
            //        TempArray.StartOffset = StreamUtil.ReadInt32(streamMatrix);
            //        long Position = streamMatrix.Position;

            //        TempArray.IntList = new List<int>();
            //        streamMatrix.Position = TempArray.StartOffset;
            //        for (int b = 0; b < TempArray.Count; b++)
            //        {
            //            TempArray.IntList.Add(StreamUtil.ReadInt32(streamMatrix));
            //        }
            //        streamMatrix.Position = Position;
            //        TempArrayListU12.Add(TempArray);
            //    }
            //    Model.U12UnkownArray2 = TempArrayListU12;




            //    //Just a translation of whats going on in someone elses program

            //    //Read StripCounter
            //    byte[] tempByte = new byte[] { 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, };
            //    streamMatrix.Position = ByteUtil.FindPosition(streamMatrix, tempByte, 0);
            //    streamMatrix.Position += 47;
            //    int StripCount = StreamUtil.ReadInt32(streamMatrix);
            //    streamMatrix.Position += 12;

            //    List<int> TempStrips = new List<int>();
            //    for (int a = 0; a < StripCount; a++)
            //    {
            //        TempStrips.Add(StreamUtil.ReadInt32(streamMatrix));
            //        streamMatrix.Position += 12;
            //    }
            //    // Incrament the stripcount
            //    List<int> stripCounts2 = new List<int>();
            //    stripCounts2.Add(0);
            //    foreach (var item in TempStrips)
            //    {
            //        stripCounts2.Add(stripCounts2[stripCounts2.Count - 1] + item);
            //    }

            //    Model.Strips = stripCounts2;

            //    //Read Vertexes
            //    tempByte = new byte[] { 0x80, 0x3F, 0x00, 0x00, 0x00, 0x20, 0x40, 0x40, 0x40, 0x40, };
            //    streamMatrix.Position = ByteUtil.FindPosition(streamMatrix, tempByte, 0);
            //    streamMatrix.Position += 24;
            //    int VertexCount = StreamUtil.ReadByte(streamMatrix);
            //    streamMatrix.Position++;
            //    List<Vertex3> vertices = new List<Vertex3>();
            //    for (int a = 0; a < VertexCount; a++)
            //    {
            //        Vertex3 vertex = new Vertex3();
            //        vertex.X = StreamUtil.ReadFloat(streamMatrix);
            //        vertex.Y = StreamUtil.ReadFloat(streamMatrix);
            //        vertex.Z = StreamUtil.ReadFloat(streamMatrix);
            //        vertices.Add(vertex);
            //    }
            //    Model.vertices = vertices;

            //    Model.faces = new List<Face>();
            //    int localIndex = 0;
            //    //Make Faces
            //    for (int a = 0; a < Model.vertices.Count; a++)
            //    {
            //        if (InsideSplits(a, Model.Strips))
            //        {
            //            localIndex = 1;
            //            continue;
            //        }
            //        if (localIndex < 2)
            //        {
            //            localIndex++;
            //            continue;
            //        }

            //        Model.faces.Add(CreateFaces(a));
            //        localIndex++;
            //    }
            //    ModelList[i] = Model;

            //    streamMatrix.Dispose();
            //    streamMatrix.Close();
            //}
        }

        public bool InsideSplits(int Number, List<int> splits)
        {
            foreach (var item in splits)
            {
                if (item == Number)
                {
                    return true;
                }
            }
            return false;
        }

        public Face CreateFaces(int Index)
        {
            Face face = new Face();
            face.V1 = Index;
            face.V2 = Index - 1;
            face.V3 = Index - 2;
            return face;
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
                StreamUtil.WriteInt32(stream, ModelList[i].NameOffset);
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
                StreamUtil.WriteInt16(stream, ModelList[i].BodyObjectsCount);
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

        public void SaveModel(string path)
        {
            string output = "";
            for (int b = 0; b < ModelList.Count - 1; b++)
            {
                output += "o mesh" + b.ToString() + "\n";
                var Model = ModelList[b];
                for (int i = 0; i < Model.vertices.Count; i++)
                {
                    output += "v " + Model.vertices[i].X + " " + Model.vertices[i].Y + " " + Model.vertices[i].Z + "\n";
                }

                for (int i = 0; i < Model.faces.Count; i++)
                {
                    output += "f " + (Model.faces[i].V1 + 1).ToString() + " " + (Model.faces[i].V2 + 1).ToString() + " " + (Model.faces[i].V3 + 1).ToString() + "\n";
                }
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, output);
        }


        public struct MPFModelHeader
        {
            //Main Header
            public string ModelName;
            public int DataOffset;
            public int EntrySize;
            public int NameOffset; //Offset Start Of something (Rotation Info?)
            public int U7; //Offset Start Of something (After Roation Info?)
            public int U8; //Offset Start Of something (Right After U7)
            public int U9; //Offset Start Of something 
            public int U10; //Blank Guessing Also Offset Start
            public int U11; //Same as U7 (After Rotation Rotation Info)
            public int U12; //After U7 (After Roation Info)
            public int U13;
            public int U14;
            public int U15;
            public int U16;

            //Counts
            public int U17; //Faces Count? (U7)
            public int U18; //!8-20 might be counts related to the bottom
            public int U19;
            public int U20; //Rotation Info Objects?
            public int BodyObjectsCount; //BodyObjects?
            public int U22;
            public int U23;
            public int U24; //VertexCount?
            public int U25;
            public int U26;
            public int U27;
            public int U28;
            public int U29;
            public int U30;

            public byte[] Matrix;

            public List<BodyObjects> bodyObjectsList;
            public List<MPFUnkownArray1> U7UnkownArray1; //Uses U17 As Count
            public List<MPFUnkownArray1> U12UnkownArray2;


            public List<Vertex3> vertices;
            public List<Face> faces;
            public List<int> Strips;
            //
        }

        public struct Vertex3
        {
            public float X;
            public float Y;
            public float Z;
        }

        public struct Face
        {
            public int V1;
            public int V2;
            public int V3;
        }

        public struct BodyObjects
        {
            public string Name;
            public string Unknown;
            public string Unknown2;
            public string Unknown3;
            public string Unknown4;
            public float Float1;
            public float Float2;
            public float Float3;
        }

        public struct MPFUnkownArray1
        {
            //Header
            public int Count;
            public int StartOffset;
            public int EndOffset; //Sometimes Used Sometimes Not
            public List<int> IntList;
        }
    }
}
