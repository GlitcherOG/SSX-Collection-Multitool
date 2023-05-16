using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models.SSXOG
{
    public class adfHandler
    {
        public int Magic;
        public int AnimHeaderOffset;
        public int AnimDataOffset;
        public int AnimCount;
        public List<AnimHeader> AnimHeaders = new List<AnimHeader>();

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                Magic = StreamUtil.ReadUInt32(stream);
                AnimHeaderOffset = StreamUtil.ReadUInt32(stream);
                AnimDataOffset = StreamUtil.ReadUInt32(stream);
                AnimCount = StreamUtil.ReadUInt32(stream);

                AnimHeaders = new List<AnimHeader>();
                for (int i = 0; i < AnimCount; i++)
                {
                    var NewHeader = new AnimHeader();

                    NewHeader.AnimName = StreamUtil.ReadString(stream, 4);
                    NewHeader.AnimOffset = StreamUtil.ReadUInt32(stream);
                    NewHeader.FrameByteSize = StreamUtil.ReadUInt32(stream);

                    //StreamUtil.WriteUInt8(stream, 0);
                    NewHeader.UCount0 = StreamUtil.ReadUInt8(stream);

                    NewHeader.FrameCount = StreamUtil.ReadUInt8(stream)*2;

                    byte Temp = StreamUtil.ReadUInt8(stream);
                    NewHeader.HeaderType = ((byte)(Temp << 5)) >> 5;
                    NewHeader.BoneCount = Temp >> 3;

                    if (NewHeader.HeaderType == 0)
                    {
                        NewHeader.ModelID = StreamUtil.ReadUInt8(stream);
                    }
                    else
                    {
                        NewHeader.U6 = StreamUtil.ReadUInt8(stream);
                    }

                    NewHeader.U7 = StreamUtil.ReadUInt8(stream);
                    NewHeader.U8 = StreamUtil.ReadUInt8(stream);
                    NewHeader.U9 = StreamUtil.ReadUInt8(stream);
                    NewHeader.U10 = StreamUtil.ReadUInt8(stream);

                    if(NewHeader.U7!=0)
                    {
                        Console.WriteLine();
                    }

                    NewHeader.U11 = StreamUtil.ReadUInt16(stream);
                    NewHeader.U11b = StreamUtil.ReadUInt16(stream);
                    NewHeader.U12 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U13 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U14 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U15 = StreamUtil.ReadUInt32(stream);
                    NewHeader.U16 = StreamUtil.ReadUInt32(stream);
                    AnimHeaders.Add(NewHeader);
                }



                for (int i = 0; i < AnimHeaders.Count; i++)
                {
                    stream.Position = AnimDataOffset + AnimHeaders[i].AnimOffset;
                    var TempHeader = AnimHeaders[i];
                    TempHeader.FrameData = new List<FrameData>();


                    for (int a = 0; a < TempHeader.FrameCount; a++)
                    {
                        int TempPos = (int)stream.Position;

                        var NewHeader = new FrameData();
                        NewHeader.BoneRotations = new List<Vector3>();

                        NewHeader.Position = StreamUtil.ReadVector3(stream);

                        for (int b = 0; b < TempHeader.BoneCount; b++)
                        {
                            NewHeader.BoneRotations.Add(ReadInt16Rotations(stream));
                        }
                        ////Probably an allign byte 4
                        //stream.Position += 2;
                        stream.Position = TempPos + TempHeader.FrameByteSize;

                        TempHeader.FrameData.Add(NewHeader);
                    }

                    AnimHeaders[i] = TempHeader;
                }
            }
        }

        public Vector3 ReadInt16Rotations(Stream stream)
        {
            Vector3 vector3;
            vector3.X = StreamUtil.ReadInt16(stream) / 16384f * 6.283185307179586f;
            vector3.Y = StreamUtil.ReadInt16(stream) / 16384f * 6.283185307179586f;
            vector3.Z = StreamUtil.ReadInt16(stream) / 16384f * 6.283185307179586f;
            return vector3;
        }


        public struct AnimHeader
        {
            public string AnimName;
            public int AnimOffset;
            public int FrameByteSize;

            public int UCount0;
            public int FrameCount;
            public int HeaderType;
            public int BoneCount;
            public int ModelID;

            public int U6;
            public int U7;
            public int U8;
            public int U9;
            public int U10;

            public int U11;
            public int U11b;
            public int U12;
            public int U13;
            public int U14;
            public int U15;
            public int U16;

            public List<FrameData> FrameData;
        }


        public struct FrameData
        {
            public Vector3 Position;
            public List<Vector3> BoneRotations;
        }


    }
}
