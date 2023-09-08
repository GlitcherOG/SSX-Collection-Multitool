using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2.SSBData
{
    public class WorldMDR
    {
        public int U0;
        public int U1Count;
        public int U2;
        public int U3;
        public int U4;

        public float U6;
        public float U7;
        public float U8;
        public float U9;

        public int U10Offset;
        public int U11Count;

        public List<int> U12 = new List<int>();

        public List<UnknownS1> UnknownS1s = new List<UnknownS1>();

        public int U13;

        public void LoadData(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream();
            StreamUtil.WriteBytes(stream, bytes);
            stream.Position = 0;

            U0 = StreamUtil.ReadUInt32(stream);
            U1Count = StreamUtil.ReadUInt32(stream);
            U2 = StreamUtil.ReadUInt32(stream);
            U3 = StreamUtil.ReadUInt32(stream);
            U4 = StreamUtil.ReadUInt32(stream);

            U6 = StreamUtil.ReadFloat(stream);
            U7 = StreamUtil.ReadFloat(stream);
            U8 = StreamUtil.ReadFloat(stream);
            U9 = StreamUtil.ReadFloat(stream);

            U10Offset = StreamUtil.ReadUInt32(stream);
            U11Count = StreamUtil.ReadUInt32(stream);

            U12 = new List<int>();

            for (int i = 0; i < U11Count; i++)
            {
                U12.Add(StreamUtil.ReadUInt32(stream));
            }

            U13 = StreamUtil.ReadUInt32(stream);

            UnknownS1s = new List<UnknownS1>();

            for (int i = 0; i < U1Count; i++)
            {
                UnknownS1 TempS1 = new UnknownS1();

                TempS1.U1Offset = StreamUtil.ReadUInt32(stream);
                TempS1.U2Offset = StreamUtil.ReadUInt32(stream);
                TempS1.MatrixOffset = StreamUtil.ReadUInt32(stream);
                TempS1.U4 = StreamUtil.ReadUInt32(stream);

                long TempPos = stream.Position;

                if(TempS1.U1Offset >0)
                {
                    stream.Position = TempS1.U1Offset;

                    var TempS2 = new UnknownS2();
                    TempS2.BboxLow = StreamUtil.ReadVector3(stream);
                    TempS2.BboxHigh = StreamUtil.ReadVector3(stream);
                    TempS2.U0 = StreamUtil.ReadUInt32(stream);
                    TempS2.ArrayCount = StreamUtil.ReadUInt32(stream);
                    TempS2.ArrayOffset = StreamUtil.ReadUInt32(stream);

                    TempS2.unknownS5 = new List<UnknownS5>();
                    stream.Position = TempS2.ArrayOffset;
                    for (int a = 0; a < TempS2.ArrayCount; a++)
                    {
                        UnknownS5 TempS5 = new UnknownS5();

                        TempS5.Offset = StreamUtil.ReadUInt32(stream);

                        long TempPos1 = stream.Position;

                        stream.Position = TempS5.Offset;

                        TempS5.unknownS6 = new UnknownS6();

                        TempS5.unknownS6.U0 = StreamUtil.ReadInt16(stream);
                        TempS5.unknownS6.U1 = StreamUtil.ReadInt16(stream);
                        TempS5.unknownS6.U3Offset = StreamUtil.ReadUInt32(stream);

                        TempS5.unknownS6.ModelOffsetHeaders = new List<ModelOffsetHeader>();

                        stream.Position = TempS5.unknownS6.U3Offset + U10Offset;

                        while (true)
                        {
                            ModelOffsetHeader TempS7 = new ModelOffsetHeader();

                            TempS7.LineCount = StreamUtil.ReadInt24(stream);
                            TempS7.U1 = StreamUtil.ReadUInt8(stream);
                            TempS7.ModelOffset = StreamUtil.ReadInt24(stream);
                            TempS5.unknownS6.ModelOffsetHeaders.Add(TempS7);

                            StreamUtil.AlignBy16(stream);

                            if(TempS7.U1==96)
                            {
                                break;
                            }
                        }
                        stream.Position = TempPos1;
                        TempS2.unknownS5.Add(TempS5);
                    }

                    TempS1.unknownS2 = TempS2;
                }

                if (TempS1.U2Offset > 0)
                {
                    stream.Position = TempS1.U2Offset;
                    UnknownS3 TempS3 = new UnknownS3();
                    TempS3.U0 = StreamUtil.ReadVector3(stream);
                    TempS3.U2 = StreamUtil.ReadVector3(stream);

                    TempS3.U3 = StreamUtil.ReadUInt32(stream);
                    TempS3.U4 = StreamUtil.ReadUInt32(stream);
                    TempS3.U5 = StreamUtil.ReadUInt32(stream);
                    TempS3.U6 = StreamUtil.ReadUInt32(stream);

                    TempS3.U7 = StreamUtil.ReadUInt32(stream);
                    TempS3.U8 = StreamUtil.ReadUInt32(stream);
                    TempS3.U9 = StreamUtil.ReadUInt32(stream);
                    TempS3.U10 = StreamUtil.ReadUInt32(stream);

                    TempS3.U11 = StreamUtil.ReadUInt32(stream);
                    TempS3.U12 = StreamUtil.ReadUInt32(stream);
                    TempS3.U13 = StreamUtil.ReadUInt32(stream);
                    TempS3.U14 = StreamUtil.ReadUInt32(stream);

                    TempS1.unknownS3 = TempS3;
                }

                if (TempS1.MatrixOffset > 0)
                {
                    stream.Position = TempS1.MatrixOffset;

                    TempS1.matrix4X4 = StreamUtil.ReadMatrix4x4(stream);
                }

                stream.Position = TempPos;

                UnknownS1s.Add(TempS1);
            }


            for (int i = 0; i < UnknownS1s.Count; i++)
            {
                var S1 = UnknownS1s[i];

                if(S1.U1Offset>0)
                {
                    for (int a = 0; a < S1.unknownS2.unknownS5.Count; a++)
                    {
                        var S5 = S1.unknownS2.unknownS5[a];
                        bool Normal = false;
                        for (int b = 0; b < S5.unknownS6.ModelOffsetHeaders.Count; b++)
                        {
                            var ModelOffsetHeader = S5.unknownS6.ModelOffsetHeaders[b];

                            stream.Position = ModelOffsetHeader.ModelOffset + U10Offset;
                            if (b==0)
                            {
                                //Unknown if used
                                stream.Position += 32;
                            }

                            if(!Normal)
                            {
                                stream.Position += 32;
                                ModelVandUVData TempModelData = new ModelVandUVData();

                                TempModelData.TristripCount = StreamUtil.ReadUInt32(stream);

                                stream.Position += 4;

                                TempModelData.VerticesCount = StreamUtil.ReadUInt32(stream);

                                stream.Position += 4 + 16;

                                TempModelData.Tristrip = new List<int>();

                                for (int c = 0; c < TempModelData.TristripCount; c++)
                                {
                                    TempModelData.Tristrip.Add(StreamUtil.ReadUInt32(stream));
                                    StreamUtil.AlignBy16(stream);
                                }

                                stream.Position += 16;

                                TempModelData.Vertices = new List<Vector3>();

                                for (int c = 0; c < TempModelData.VerticesCount; c++)
                                {
                                    Vector3 vector3 = new Vector3();

                                    vector3.X = StreamUtil.ReadInt16(stream) / 1024f;
                                    vector3.Y = StreamUtil.ReadInt16(stream) / 1024f;
                                    vector3.Z = StreamUtil.ReadInt16(stream) / 1024f;

                                    TempModelData.Vertices.Add(vector3);
                                }
                                StreamUtil.AlignBy16(stream);

                                stream.Position += 16;
                                TempModelData.UV = new List<Vector2>();

                                for (int c = 0; c < TempModelData.VerticesCount; c++)
                                {
                                    Vector2 vector3 = new Vector2();

                                    vector3.X = StreamUtil.ReadInt16(stream) / 1024f;
                                    vector3.Y = StreamUtil.ReadInt16(stream) / 1024f;

                                    TempModelData.UV.Add(vector3);
                                }
                                StreamUtil.AlignBy16(stream);

                                ModelOffsetHeader.modelVandUVData = TempModelData;
                            }
                            else
                            {
                                ModelNormalData TempModelData = new ModelNormalData();

                                stream.Position += 14;

                                TempModelData.NormalCount = StreamUtil.ReadUInt8(stream);

                                stream.Position += 1;

                                TempModelData.Normals = new List<Vector3>();

                                for (int c = 0; c < TempModelData.NormalCount; c++)
                                {
                                    Vector3 vector3 = new Vector3();

                                    vector3.X = StreamUtil.ReadInt16(stream) / 1024f;
                                    vector3.Y = StreamUtil.ReadInt16(stream) / 1024f;
                                    vector3.Z = StreamUtil.ReadInt16(stream) / 1024f;

                                    TempModelData.Normals.Add(vector3);
                                }
                                StreamUtil.AlignBy16(stream);

                                ModelOffsetHeader.modelNormalData = TempModelData;
                            }

                            Normal = !Normal;

                            S5.unknownS6.ModelOffsetHeaders[b] = ModelOffsetHeader;
                        }

                        S1.unknownS2.unknownS5[a] = S5;
                    }
                }

                UnknownS1s[i] = S1;
            }



            stream.Close();
        }

        public struct UnknownS1
        {
            public int U1Offset;
            public int U2Offset;
            public int MatrixOffset;
            public int U4;

            public Matrix4x4 matrix4X4;
            public UnknownS2 unknownS2;
            public UnknownS3 unknownS3;
        }

        public struct UnknownS2
        {
            public Vector3 BboxLow;
            public Vector3 BboxHigh;
            public int U0;
            public int ArrayCount;
            public int ArrayOffset;

            public List<UnknownS5> unknownS5;
        }

        public struct UnknownS3
        {
            public Vector3 U0;
            public Vector3 U2; //?

            public int U3;
            public int U4;
            public int U5;
            public int U6;

            public int U7;
            public int U8;
            public int U9;
            public int U10;

            public int U11;
            public int U12;
            public int U13;
            public int U14;
        }

        public struct UnknownS5
        {
            public int Offset;
            public UnknownS6 unknownS6;
        }

        public struct UnknownS6
        {
            public int U0;
            public int U1;
            public int U3Offset;

            public List <ModelOffsetHeader> ModelOffsetHeaders;
        }

        public struct ModelOffsetHeader
        {
            public int LineCount;
            public int U1;
            public int ModelOffset;

            public ModelVandUVData modelVandUVData;
            public ModelNormalData modelNormalData;
        }

        public struct ModelVandUVData
        {
            public int TristripCount;
            public int VerticesCount;

            public List<int> Tristrip;
            public List<Vector3> Vertices;
            public List<Vector2> UV;
        }

        public struct ModelNormalData
        {
            public int NormalCount;

            public List<Vector3> Normals;
        }
    }
}
