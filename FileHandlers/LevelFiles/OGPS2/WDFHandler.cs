﻿using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace SSXMultiTool.FileHandlers.LevelFiles.OGPS2
{
    public class WDFHandler
    {
        public List<WDFChunk> WDFChunks = new List<WDFChunk>();

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                WDFChunks = new List<WDFChunk>();

                while(stream.Position<stream.Length)
                {
                    WDFChunk wdfChunk = new WDFChunk();

                    StreamUtil.AlignBy16(stream);

                    wdfChunk.U0 = StreamUtil.ReadVector3(stream);
                    wdfChunk.U1 = StreamUtil.ReadVector3(stream);
                    wdfChunk.U2 = StreamUtil.ReadVector3(stream);

                    wdfChunk.UstructCount2 = StreamUtil.ReadInt16(stream);
                    wdfChunk.PatchesCount = StreamUtil.ReadInt16(stream);
                    wdfChunk.UstructCount3 = StreamUtil.ReadInt16(stream);
                    wdfChunk.UstructCount4 = StreamUtil.ReadInt16(stream);
                    wdfChunk.UstructCount1 = StreamUtil.ReadInt16(stream);
                    wdfChunk.U7 = StreamUtil.ReadInt16(stream);

                    if(wdfChunk.UstructCount4 != 0)
                    {
                        Console.WriteLine("");
                    }

                    wdfChunk.unknownStruct0s = new List<UnknownStruct0>();
                    for (int i = 0; i < 16; i++)
                    {
                        var TempUnknown = new UnknownStruct0();

                        TempUnknown.U0 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U1 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U2 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U3 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U4 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U5 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U6 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U7 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U8 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U9 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U10 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U11 = StreamUtil.ReadInt16(stream);

                        TempUnknown.U12 = StreamUtil.ReadVector3(stream);
                        TempUnknown.U13 = StreamUtil.ReadVector3(stream);

                        wdfChunk.unknownStruct0s.Add(TempUnknown);
                    }

                    wdfChunk.unknownStruct1s = new List<UnknownStruct1>();

                    for (int i = 0; i < wdfChunk.UstructCount1; i++)
                    {
                        var TempUnknown = new UnknownStruct1();

                        TempUnknown.U0 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U1 = StreamUtil.ReadInt16(stream);

                        wdfChunk.unknownStruct1s.Add(TempUnknown);
                    }

                    StreamUtil.AlignBy16(stream);

                    wdfChunk.unknownStruct2s = new List<UnknownStruct2>();

                    for (int i = 0; i < wdfChunk.UstructCount2; i++)
                    {
                        var TempUnknown = new UnknownStruct2();

                        TempUnknown.matrix4X4 = StreamUtil.ReadMatrix4x4(stream);

                        TempUnknown.matrix4X41 = StreamUtil.ReadMatrix4x4(stream);
                        TempUnknown.vector40 = StreamUtil.ReadVector4(stream);
                        TempUnknown.vector41 = StreamUtil.ReadVector4(stream);
                        TempUnknown.vector42 = StreamUtil.ReadVector4(stream);
                        TempUnknown.vector43 = StreamUtil.ReadVector4(stream);

                        TempUnknown.U0 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U1 = StreamUtil.ReadInt16(stream);

                        TempUnknown.U2 = StreamUtil.ReadUInt32(stream);
                        TempUnknown.U3 = StreamUtil.ReadUInt32(stream);
                        TempUnknown.U4 = StreamUtil.ReadUInt32(stream);

                        TempUnknown.vector31 = StreamUtil.ReadVector3(stream);
                        TempUnknown.vector32 = StreamUtil.ReadVector3(stream);

                        TempUnknown.U5 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U6 = StreamUtil.ReadInt16(stream);

                        TempUnknown.U7 = StreamUtil.ReadFloat(stream);

                        TempUnknown.U8 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U9 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U10 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U11 = StreamUtil.ReadInt16(stream);

                        TempUnknown.U12 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U13 = StreamUtil.ReadUInt32(stream);

                        TempUnknown.U14 = StreamUtil.ReadUInt32(stream);
                        TempUnknown.U15 = StreamUtil.ReadUInt32(stream);
                        TempUnknown.U16 = StreamUtil.ReadUInt32(stream);
                        TempUnknown.U17 = StreamUtil.ReadUInt32(stream);

                        wdfChunk.unknownStruct2s.Add(TempUnknown);
                    }

                    wdfChunk.Patches = new List<Patch>();
                    for (int i = 0; i < wdfChunk.PatchesCount; i++)
                    {
                        var TempPatch = new Patch();

                        TempPatch.LightMapPoint = StreamUtil.ReadVector4(stream);

                        TempPatch.UVPoint1 = StreamUtil.ReadVector4(stream);
                        TempPatch.UVPoint2 = StreamUtil.ReadVector4(stream);
                        TempPatch.UVPoint3 = StreamUtil.ReadVector4(stream);
                        TempPatch.UVPoint4 = StreamUtil.ReadVector4(stream);

                        TempPatch.R4C4 = StreamUtil.ReadVector4(stream);
                        TempPatch.R4C3 = StreamUtil.ReadVector4(stream);
                        TempPatch.R4C2 = StreamUtil.ReadVector4(stream);
                        TempPatch.R4C1 = StreamUtil.ReadVector4(stream);

                        TempPatch.R3C4 = StreamUtil.ReadVector4(stream);
                        TempPatch.R3C3 = StreamUtil.ReadVector4(stream);
                        TempPatch.R3C2 = StreamUtil.ReadVector4(stream);
                        TempPatch.R3C1 = StreamUtil.ReadVector4(stream);

                        TempPatch.R2C4 = StreamUtil.ReadVector4(stream);
                        TempPatch.R2C3 = StreamUtil.ReadVector4(stream);
                        TempPatch.R2C2 = StreamUtil.ReadVector4(stream);
                        TempPatch.R2C1 = StreamUtil.ReadVector4(stream);

                        TempPatch.R1C4 = StreamUtil.ReadVector4(stream);
                        TempPatch.R1C3 = StreamUtil.ReadVector4(stream);
                        TempPatch.R1C2 = StreamUtil.ReadVector4(stream);
                        TempPatch.R1C1 = StreamUtil.ReadVector4(stream);

                        TempPatch.LowestXYZ = StreamUtil.ReadVector4(stream);
                        TempPatch.HighestXYZ = StreamUtil.ReadVector4(stream);
                        TempPatch.Point1 = StreamUtil.ReadVector4(stream);
                        TempPatch.Point2 = StreamUtil.ReadVector4(stream);
                        TempPatch.Point3 = StreamUtil.ReadVector4(stream);
                        TempPatch.Point4 = StreamUtil.ReadVector4(stream);

                        TempPatch.TextureID = StreamUtil.ReadUInt32(stream);
                        TempPatch.PatchType = StreamUtil.ReadUInt32(stream);
                        TempPatch.LightmapID = StreamUtil.ReadUInt32(stream);
                        TempPatch.U0 = StreamUtil.ReadUInt32(stream);

                        wdfChunk.Patches.Add(TempPatch);
                    }

                    wdfChunk.unknownStruct4s = new List<UnknownStruct4>();
                    for (int i = 0; i < wdfChunk.UstructCount4; i++)
                    {
                        var TempUnknown = new UnknownStruct4();

                        TempUnknown.matrix4X4 = StreamUtil.ReadMatrix4x4(stream);
                        TempUnknown.vector4 = StreamUtil.ReadVector4(stream);

                        TempUnknown.U0 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U1 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U2 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U3 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U4 = StreamUtil.ReadInt16(stream);
                        TempUnknown.U5 = StreamUtil.ReadInt16(stream);

                        TempUnknown.U6 = StreamUtil.ReadVector3(stream);
                        TempUnknown.U7 = StreamUtil.ReadVector3(stream);

                        TempUnknown.U71 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U8 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U9 = StreamUtil.ReadUInt32(stream);

                        wdfChunk.unknownStruct4s.Add(TempUnknown);
                    }


                    wdfChunk.unknownStruct3s = new List<UnknownStruct3>();
                    for (int i = 0; i < wdfChunk.UstructCount3; i++)
                    {
                        var TempUnknown = new UnknownStruct3();

                        TempUnknown.U0 = StreamUtil.ReadUInt32(stream);

                        TempUnknown.U1 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U2 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U3 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U4 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U5 = StreamUtil.ReadFloat(stream);

                        TempUnknown.U6 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U7 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U8 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U9 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U10 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U11 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U12 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U13 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U14 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U15 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U16 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U17 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U18 = StreamUtil.ReadFloat(stream);

                        TempUnknown.U19 = StreamUtil.ReadUInt32(stream);
                        TempUnknown.U20 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U21 = StreamUtil.ReadUInt32(stream);

                        wdfChunk.unknownStruct3s.Add(TempUnknown);
                    }

                    WDFChunks.Add(wdfChunk);
                }

            }
        }

        public struct WDFChunk
        {
            public Vector3 U0;
            public Vector3 U1;
            public Vector3 U2;
            public int UstructCount2;
            public int PatchesCount;
            public int UstructCount3;
            public int UstructCount4;
            public int UstructCount1;
            public int U7;

            public List<UnknownStruct0> unknownStruct0s; //16 on all levels
            public List<UnknownStruct1> unknownStruct1s;
            //Align by 16

            public List<UnknownStruct2> unknownStruct2s;
            public List<Patch> Patches;
            public List<UnknownStruct4> unknownStruct4s;
            public List<UnknownStruct3> unknownStruct3s;
        }

        public struct UnknownStruct0
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
            public int U7;
            public int U8;
            public int U9;
            public int U10;
            public int U11;

            public Vector3 U12;
            public Vector3 U13;
        }

        public struct UnknownStruct1
        {
            public int U0; //16
            public int U1; //16
        }

        //272 bytes
        public struct UnknownStruct2
        {
            public Matrix4x4 matrix4X4;

            //Colour Data ?
            public Matrix4x4 matrix4X41;
            public Vector4 vector40;
            public Vector4 vector41;
            public Vector4 vector42;
            public Vector4 vector43;

            public int U0; //16
            public int U1; //16

            public int U2;
            public int U3;
            public int U4;

            public Vector3 vector31;
            public Vector3 vector32;

            public int U5; //16
            public int U6; //16

            public float U7;

            public int U8; //16
            public int U9; //16
            public int U10; //16
            public int U11; //16

            public float U12;
            public int U13;

            public int U14;
            public int U15;
            public int U16;
            public int U17;
        }

        //448 bytes
        public struct Patch
        {
            public Vector4 LightMapPoint;

            public Vector4 UVPoint1;
            public Vector4 UVPoint2;
            public Vector4 UVPoint3;
            public Vector4 UVPoint4;

            public Vector4 R4C4;
            public Vector4 R4C3;
            public Vector4 R4C2;
            public Vector4 R4C1;

            public Vector4 R3C4;
            public Vector4 R3C3;
            public Vector4 R3C2;
            public Vector4 R3C1;

            public Vector4 R2C4;
            public Vector4 R2C3;
            public Vector4 R2C2;
            public Vector4 R2C1;

            public Vector4 R1C4;
            public Vector4 R1C3;
            public Vector4 R1C2;
            public Vector4 R1C1;

            public Vector4 LowestXYZ;
            public Vector4 HighestXYZ;

            public Vector4 Point1;
            public Vector4 Point2;
            public Vector4 Point3;
            public Vector4 Point4;

            public int TextureID;
            public int PatchType;
            public int LightmapID;
            public int U0;
        }

        //88
        public struct UnknownStruct3
        {
            public int U0;
            public float U1;
            public float U2;
            public float U3;
            public float U4;
            public float U5;

            public float U6;
            public float U7;
            public float U8;
            public float U9;
            public float U10;
            public float U11;
            public float U12;
            public float U13;
            public float U14;
            public float U15;
            public float U16;
            public float U17;
            public float U18;

            public int U19;
            public float U20;
            public int U21;
        }

        public struct UnknownStruct4
        {
            public Matrix4x4 matrix4X4;
            public Vector4 vector4;

            //16
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;

            public Vector3 U6;
            public Vector3 U7;
            public float U71;

            public float U8;
            public int U9;
        }
    }
}