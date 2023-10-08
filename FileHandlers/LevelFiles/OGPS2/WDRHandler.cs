using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.LevelFiles.OGPS2
{
    internal class WDRHandler
    {
        public List<ModelHeader> modelHeaders = new List<ModelHeader>();

        //Model Data
        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                while (stream.Position <= stream.Length)
                {
                    long StartPos = stream.Position;

                    var NewHeader = new ModelHeader();
                    NewHeader.ModelCount = StreamUtil.ReadUInt32(stream);

                    if (NewHeader.ModelCount > 4)
                    {
                        int Tempdata = 1;
                    }

                    NewHeader.U1 = StreamUtil.ReadUInt32(stream);
                    NewHeader.ModelByteSize = StreamUtil.ReadUInt32(stream);
                    NewHeader.U2 = StreamUtil.ReadUInt32(stream);

                    NewHeader.vector3 = StreamUtil.ReadVector3(stream);
                    NewHeader.U3 = StreamUtil.ReadUInt32(stream);

                    NewHeader.U4 = StreamUtil.ReadUInt32(stream);

                    NewHeader.ModelOffsets = new List<int>();

                    for (int i = 0; i < NewHeader.ModelCount; i++)
                    {
                        NewHeader.ModelOffsets.Add(StreamUtil.ReadUInt32(stream));
                    }

                    StreamUtil.AlignBy16(stream);

                    NewHeader.models = new List<Model>();

                    for (int z = 0; z < NewHeader.ModelCount; z++)
                    {
                        var TempModel = new Model();

                        TempModel.vector31 = StreamUtil.ReadVector3(stream);
                        TempModel.vector32 = StreamUtil.ReadVector3(stream);
                        TempModel.U10 = StreamUtil.ReadUInt32(stream);
                        TempModel.U11 = StreamUtil.ReadUInt32(stream);

                        TempModel.U12 = StreamUtil.ReadUInt32(stream);
                        TempModel.U13 = StreamUtil.ReadUInt32(stream);
                        TempModel.U14 = StreamUtil.ReadUInt32(stream);
                        TempModel.U15 = StreamUtil.ReadUInt32(stream);

                        TempModel.U16 = StreamUtil.ReadUInt32(stream);
                        TempModel.U17 = StreamUtil.ReadUInt32(stream);
                        TempModel.U18 = StreamUtil.ReadUInt32(stream);
                        TempModel.U19 = StreamUtil.ReadUInt32(stream);

                        TempModel.modelDatas = new List<ModelData>();
                        StreamUtil.AlignBy(stream, 128, StartPos);
                        while (true)
                        {
                            stream.Position += 48;

                            var TempModelData = new ModelData();

                            TempModelData.TristripCount = StreamUtil.ReadUInt32(stream);
                            TempModelData.U0 = StreamUtil.ReadUInt32(stream);
                            TempModelData.VertexCount = StreamUtil.ReadUInt32(stream);
                            TempModelData.U1 = StreamUtil.ReadUInt32(stream);

                            stream.Position += 16;

                            TempModelData.Tristrip = new List<int>();

                            if (TempModelData.TristripCount > 20 || TempModelData.VertexCount > 70)
                            {
                                int TempPos = 0;
                            }

                            for (int i = 0; i < TempModelData.TristripCount; i++)
                            {
                                TempModelData.Tristrip.Add(StreamUtil.ReadUInt16(stream));
                            }

                            StreamUtil.AlignBy16(stream);

                            stream.Position += 64;

                            TempModelData.UV = new List<Vector2>();
                            for (int a = 0; a < TempModelData.VertexCount; a++)
                            {
                                Vector2 uv = new Vector2();
                                uv.X = StreamUtil.ReadInt16(stream) / 4096f;
                                uv.Y = StreamUtil.ReadInt16(stream) / 4096f;
                                TempModelData.UV.Add(uv);
                            }
                            StreamUtil.AlignBy16(stream);

                            stream.Position += 48;
                            TempModelData.Normals = new List<Vector3>();

                            for (int a = 0; a < TempModelData.VertexCount; a++)
                            {
                                Vector3 normal = new Vector3();
                                normal.X = StreamUtil.ReadInt16(stream) / 32768f;
                                normal.Y = StreamUtil.ReadInt16(stream) / 32768f;
                                normal.Z = StreamUtil.ReadInt16(stream) / 32768f;
                                TempModelData.Normals.Add(normal);
                            }
                            StreamUtil.AlignBy16(stream);

                            stream.Position += 16;
                            TempModelData.Vertex = new List<Vector3>();
                            for (int a = 0; a < TempModelData.VertexCount; a++)
                            {
                                Vector3 vertex = new Vector3();
                                vertex.X = ((float)StreamUtil.ReadInt16(stream) / 32768f);
                                vertex.Y = ((float)StreamUtil.ReadInt16(stream) / 32768f);
                                vertex.Z = ((float)StreamUtil.ReadInt16(stream) / 32768f);
                                TempModelData.Vertex.Add(vertex);
                            }

                            StreamUtil.AlignBy16(stream);

                            stream.Position += 16;

                            stream.Position += 60;

                            int Temp = StreamUtil.ReadUInt32(stream);

                            if (-559038737 == Temp)
                            {
                                stream.Position += 16;

                                if (TempModel.U11 != -559038737)
                                {
                                    MatrixData matrixData = new MatrixData();

                                    matrixData.matrix4 = StreamUtil.ReadMatrix4x4(stream);
                                    matrixData.U0 = StreamUtil.ReadInt16(stream);
                                    matrixData.U1 = StreamUtil.ReadInt16(stream);
                                    matrixData.U2 = StreamUtil.ReadInt16(stream);
                                    matrixData.U3 = StreamUtil.ReadInt16(stream);
                                    matrixData.U4 = StreamUtil.ReadInt16(stream);
                                    matrixData.U5 = StreamUtil.ReadInt16(stream);
                                    matrixData.U6 = StreamUtil.ReadInt16(stream);
                                    matrixData.U7 = StreamUtil.ReadInt16(stream);

                                    matrixData.UStruct11Count = StreamUtil.ReadUInt32(stream);
                                    matrixData.UStruct11Offset = StreamUtil.ReadUInt32(stream);
                                    matrixData.UStruct12Count = StreamUtil.ReadUInt32(stream);
                                    matrixData.UStruct12Offset = StreamUtil.ReadUInt32(stream);

                                    matrixData.uStruct11s = new List<UStruct1>();

                                    for (int c = 0; c < matrixData.UStruct11Count; c++)
                                    {
                                        var TempUStruct1 = new UStruct1();

                                        TempUStruct1.vector30 = StreamUtil.ReadVector3(stream);
                                        TempUStruct1.vector31 = StreamUtil.ReadVector3(stream);
                                        TempUStruct1.U0 = StreamUtil.ReadUInt32(stream);
                                        TempUStruct1.U1 = StreamUtil.ReadUInt32(stream);

                                        matrixData.uStruct11s.Add(TempUStruct1);
                                    }
                                    matrixData.uStruct12s = new List<UStruct1>();
                                    for (int c = 0; c < matrixData.UStruct12Count; c++)
                                    {
                                        var TempUStruct1 = new UStruct1();

                                        TempUStruct1.vector30 = StreamUtil.ReadVector3(stream);
                                        TempUStruct1.vector31 = StreamUtil.ReadVector3(stream);
                                        TempUStruct1.U0 = StreamUtil.ReadUInt32(stream);
                                        TempUStruct1.U1 = StreamUtil.ReadUInt32(stream);

                                        matrixData.uStruct12s.Add(TempUStruct1);
                                    }
                                }


                                TempModel.modelDatas.Add(TempModelData);
                                break;
                            }
                            else
                            {
                                stream.Position -= 64;
                            }

                            TempModel.modelDatas.Add(TempModelData);
                        }

                        NewHeader.models.Add(TempModel);
                    }
                    StreamUtil.AlignBy(stream, 128);
                    modelHeaders.Add(NewHeader);
                }
            }
        }

        public struct ModelHeader
        {
            //This is all models

            //Whole thing is chunked
            //Header
            public int ModelCount;
            public int U1;
            public int ModelByteSize;
            public int U2;

            public Vector3 vector3;
            public int U3;

            public int U4;
            public List<int> ModelOffsets;

            public List<Model> models;
        }

        public struct Model
        {
            public Vector3 vector31;
            public Vector3 vector32;
            public int U10;
            public int U11; //Matrix Offset

            public int U12;
            public int U13;
            public int U14;
            public int U15; //Mesh Size

            public int U16;
            public int U17; //Mesh Size 1
            public int U18; //Mesh Size 2
            public int U19; //Mesh Size 3

            public List<ModelData> modelDatas;
            public MatrixData matrixData;
        }

        public struct ModelData
        {
            public int TristripCount;
            public int U0;
            public int VertexCount;
            public int U1;

            public List<int> Tristrip;
            public List<Vector2> UV;
            public List<Vector3> Vertex;
            public List<Vector3> Normals;
        }

        public struct MatrixData
        {
            public Matrix4x4 matrix4;

            //16
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
            public int U7;

            public int UStruct11Count;
            public int UStruct11Offset;
            public int UStruct12Count;
            public int UStruct12Offset;

            public List<UStruct1> uStruct11s;
            public List<UStruct1> uStruct12s;
        }

        public struct UStruct1
        {
            public Vector3 vector30;
            public Vector3 vector31;
            public int U0;
            public int U1;
        }
    }
}
