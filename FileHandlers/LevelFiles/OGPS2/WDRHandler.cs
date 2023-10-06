﻿using SSXMultiTool.Utilities;
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
                    var NewHeader = new ModelHeader();
                    NewHeader.U0 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U1 = StreamUtil.ReadUInt32(stream);
                    NewHeader.ModelByteSize = StreamUtil.ReadUInt32(stream);
                    NewHeader.U2 = StreamUtil.ReadUInt32(stream);

                    NewHeader.vector3 = StreamUtil.ReadVector3(stream);
                    NewHeader.U3 = StreamUtil.ReadUInt32(stream);

                    NewHeader.U4 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U5 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U6 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U7 = StreamUtil.ReadUInt32(stream);

                    NewHeader.vector31 = StreamUtil.ReadVector3(stream);
                    NewHeader.vector32 = StreamUtil.ReadVector3(stream);
                    NewHeader.U10 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U11 = StreamUtil.ReadUInt32(stream);

                    NewHeader.U12 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U13 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U14 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U15 = StreamUtil.ReadUInt32(stream);

                    NewHeader.U16 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U17 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U18 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U19 = StreamUtil.ReadUInt32(stream);

                    NewHeader.U20 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U21 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U22 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U23 = StreamUtil.ReadUInt32(stream);

                    NewHeader.modelDatas = new List<ModelData>();

                    while(true)
                    {
                        stream.Position += 48;

                        var TempModelData = new ModelData();

                        TempModelData.TristripCount = StreamUtil.ReadUInt32(stream);
                        TempModelData.U0 = StreamUtil.ReadUInt32(stream);
                        TempModelData.VertexCount = StreamUtil.ReadUInt32(stream);
                        TempModelData.U1 = StreamUtil.ReadUInt32(stream);

                        stream.Position += 16;

                        TempModelData.Tristrip = new List<int>();

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

                        stream.Position += 64;
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

                        if(-559038737 == Temp)
                        {
                            stream.Position += 4 + 16;

                            StreamUtil.AlignBy(stream, 128);

                            break;
                        }
                        else
                        {
                            stream.Position -= 60;
                        }

                        NewHeader.modelDatas.Add(TempModelData);
                    }

                    modelHeaders.Add(NewHeader);
                }
            }
        }

        public struct ModelHeader
        {
            //This is all models

            //Whole thing is chunked
            //Header
            public int U0;
            public int U1;
            public int ModelByteSize;
            public int U2;

            public Vector3 vector3;
            public int U3;

            public int U4;
            public int U5;
            public int U6;
            public int U7;

            public Vector3 vector31;
            public Vector3 vector32;
            public int U10;
            public int U11;

            public int U12;
            public int U13;
            public int U14;
            public int U15;

            public int U16;
            public int U17;
            public int U18;
            public int U19;

            public int U20;
            public int U21;
            public int U22;
            public int U23;

            public List<ModelData> modelDatas;
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
    }
}
