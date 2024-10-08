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
    public class WDFHandler
    {
        public WDFChunk[,] WDFChunks = new WDFChunk[1,1];
        public void LoadGuess(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                //WDFChunks = new List<WDFChunk>();

                while(stream.Position<stream.Length)
                {
                    WDFChunk wdfChunk = new WDFChunk();

                    StreamUtil.AlignBy16(stream);

                    wdfChunk.U0 = StreamUtil.ReadVector3(stream);
                    wdfChunk.U1 = StreamUtil.ReadVector3(stream);
                    wdfChunk.U2 = StreamUtil.ReadVector3(stream);

                    wdfChunk.InstanceCount = StreamUtil.ReadInt16(stream);
                    wdfChunk.PatchesCount = StreamUtil.ReadInt16(stream);
                    wdfChunk.LightCount = StreamUtil.ReadInt16(stream);
                    wdfChunk.SplineSegmentCount = StreamUtil.ReadInt16(stream);
                    wdfChunk.UstructCount1 = StreamUtil.ReadInt16(stream);
                    wdfChunk.U7 = StreamUtil.ReadInt16(stream);

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

                    wdfChunk.Instances = new List<Instance>();

                    for (int i = 0; i < wdfChunk.InstanceCount; i++)
                    {
                        var TempUnknown = new Instance();

                        TempUnknown.matrix4X4 = StreamUtil.ReadMatrix4x4(stream);

                        TempUnknown.lightingMatrix4x4 = StreamUtil.ReadMatrix4x4(stream);

                        TempUnknown.LightColour1 = StreamUtil.ReadVector4(stream);
                        TempUnknown.LightColour2 = StreamUtil.ReadVector4(stream);
                        TempUnknown.LightColour3 = StreamUtil.ReadVector4(stream);
                        TempUnknown.AmbentLightColour = StreamUtil.ReadVector4(stream);

                        TempUnknown.WDFGridID = StreamUtil.ReadInt16(stream);
                        TempUnknown.InstanceIndex = StreamUtil.ReadInt16(stream);

                        TempUnknown.U2 = StreamUtil.ReadUInt32(stream);
                        TempUnknown.U3 = StreamUtil.ReadUInt32(stream);
                        TempUnknown.PrefabID = StreamUtil.ReadUInt32(stream);

                        TempUnknown.LowestXYZ = StreamUtil.ReadVector3(stream);
                        TempUnknown.HighestXYZ = StreamUtil.ReadVector3(stream);

                        TempUnknown.U5 = StreamUtil.ReadInt16(stream);
                        TempUnknown.BitFlags = StreamUtil.ReadInt16(stream);

                        TempUnknown.PlayerBounce = StreamUtil.ReadFloat(stream);

                        TempUnknown.CollsionMode = StreamUtil.ReadInt16(stream);
                        TempUnknown.CollisonModelIndex = StreamUtil.ReadInt16(stream);
                        TempUnknown.PhysicsIndex = StreamUtil.ReadInt16(stream);
                        TempUnknown.U11 = StreamUtil.ReadInt16(stream);

                        TempUnknown.U12 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U13 = StreamUtil.ReadUInt32(stream);

                        TempUnknown.U14 = StreamUtil.ReadUInt32(stream);
                        TempUnknown.U15 = StreamUtil.ReadUInt32(stream);
                        TempUnknown.U16 = StreamUtil.ReadUInt32(stream);
                        TempUnknown.U17 = StreamUtil.ReadUInt32(stream);

                        wdfChunk.Instances.Add(TempUnknown);
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
                        TempPatch.LightmapID = StreamUtil.ReadUInt32(stream);
                        TempPatch.PatchType = StreamUtil.ReadUInt32(stream);
                        TempPatch.U0 = StreamUtil.ReadUInt32(stream);

                        wdfChunk.Patches.Add(TempPatch);
                    }

                    wdfChunk.SplineSegments = new List<SplineSegment>();
                    for (int i = 0; i < wdfChunk.SplineSegmentCount; i++)
                    {
                        var TempUnknown = new SplineSegment();

                        TempUnknown.Point4 = StreamUtil.ReadVector4(stream);
                        TempUnknown.Point3 = StreamUtil.ReadVector4(stream);
                        TempUnknown.Point2 = StreamUtil.ReadVector4(stream);
                        TempUnknown.ControlPoint = StreamUtil.ReadVector4(stream);

                        TempUnknown.U0 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U1 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U2 = StreamUtil.ReadFloat(stream);
                        TempUnknown.U3 = StreamUtil.ReadFloat(stream);

                        TempUnknown.PreviousWDFChunkID = StreamUtil.ReadInt16(stream);
                        TempUnknown.PreviousSegmentID = StreamUtil.ReadInt16(stream);
                        TempUnknown.NextWDFChunkID = StreamUtil.ReadInt16(stream);
                        TempUnknown.NextSegmentID = StreamUtil.ReadInt16(stream);
                        TempUnknown.SplineID = StreamUtil.ReadInt16(stream);
                        TempUnknown.U4 = StreamUtil.ReadInt16(stream);

                        TempUnknown.LowestXYZ = StreamUtil.ReadVector3(stream);
                        TempUnknown.HighestXYZ = StreamUtil.ReadVector3(stream);

                        TempUnknown.SegmentDisatnce = StreamUtil.ReadFloat(stream);
                        TempUnknown.PreviousSegmentsDistance = StreamUtil.ReadFloat(stream);
                        TempUnknown.U9 = StreamUtil.ReadUInt32(stream);

                        wdfChunk.SplineSegments.Add(TempUnknown);
                    }


                    wdfChunk.Lights = new List<Light>();
                    for (int i = 0; i < wdfChunk.LightCount; i++)
                    {
                        var TempUnknown = new Light();

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

                        wdfChunk.Lights.Add(TempUnknown);
                    }

                    //WDFChunks.Add(wdfChunk);
                }

            }
        }

        public void Load(string path, WDXHandler.WDFGridGroup[,] wdfGridGroup)
        {
            int segments = 0;
            int Temp = -1;
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                WDFChunks = new WDFChunk[wdfGridGroup.GetLength(0), wdfGridGroup.GetLength(1)];

                for (int y = 0; y < wdfGridGroup.GetLength(1); y++)
                {
                    for (int x = 0; x < wdfGridGroup.GetLength(0); x++)
                    {
                        Temp++;
                        stream.Position = wdfGridGroup[x,y].Offset;
                        WDFChunk wdfChunk = new WDFChunk();
                        wdfChunk.ChunkID = Temp;

                        StreamUtil.AlignBy16(stream);

                        wdfChunk.U0 = StreamUtil.ReadVector3(stream);
                        wdfChunk.U1 = StreamUtil.ReadVector3(stream);
                        wdfChunk.U2 = StreamUtil.ReadVector3(stream);

                        wdfChunk.InstanceCount = StreamUtil.ReadInt16(stream);
                        wdfChunk.PatchesCount = StreamUtil.ReadInt16(stream);
                        wdfChunk.LightCount = StreamUtil.ReadInt16(stream);
                        wdfChunk.SplineSegmentCount = StreamUtil.ReadInt16(stream);
                        wdfChunk.UstructCount1 = StreamUtil.ReadInt16(stream);
                        wdfChunk.U7 = StreamUtil.ReadInt16(stream);

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

                        wdfChunk.Instances = new List<Instance>();

                        for (int i = 0; i < wdfChunk.InstanceCount; i++)
                        {
                            var TempInstance = new Instance();

                            TempInstance.matrix4X4 = StreamUtil.ReadMatrix4x4(stream);

                            TempInstance.lightingMatrix4x4 = StreamUtil.ReadMatrix4x4(stream);

                            TempInstance.LightColour1 = StreamUtil.ReadVector4(stream);
                            TempInstance.LightColour2 = StreamUtil.ReadVector4(stream);
                            TempInstance.LightColour3 = StreamUtil.ReadVector4(stream);
                            TempInstance.AmbentLightColour = StreamUtil.ReadVector4(stream);

                            TempInstance.WDFGridID = StreamUtil.ReadInt16(stream);
                            TempInstance.InstanceIndex = StreamUtil.ReadInt16(stream);

                            TempInstance.U2 = StreamUtil.ReadUInt32(stream);
                            TempInstance.U3 = StreamUtil.ReadUInt32(stream);
                            TempInstance.PrefabID = StreamUtil.ReadUInt32(stream);

                            TempInstance.LowestXYZ = StreamUtil.ReadVector3(stream);
                            TempInstance.HighestXYZ = StreamUtil.ReadVector3(stream);

                            TempInstance.U5 = StreamUtil.ReadInt16(stream);
                            TempInstance.BitFlags = StreamUtil.ReadInt16(stream);

                            TempInstance.PlayerBounce = StreamUtil.ReadFloat(stream);

                            TempInstance.CollsionMode = StreamUtil.ReadInt16(stream);

                            TempInstance.CollisonModelIndex = StreamUtil.ReadInt16(stream);
                            TempInstance.PhysicsIndex = StreamUtil.ReadInt16(stream);
                            TempInstance.U11 = StreamUtil.ReadInt16(stream);

                            TempInstance.U12 = StreamUtil.ReadFloat(stream);
                            TempInstance.U13 = StreamUtil.ReadUInt32(stream);

                            TempInstance.U14 = StreamUtil.ReadUInt32(stream);
                            TempInstance.U15 = StreamUtil.ReadUInt32(stream);
                            TempInstance.U16 = StreamUtil.ReadUInt32(stream);
                            TempInstance.U17 = StreamUtil.ReadUInt32(stream);

                            wdfChunk.Instances.Add(TempInstance);
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
                            TempPatch.LightmapID = StreamUtil.ReadUInt32(stream);
                            TempPatch.PatchType = StreamUtil.ReadUInt32(stream);

                            TempPatch.U0 = StreamUtil.ReadUInt32(stream);

                            wdfChunk.Patches.Add(TempPatch);
                        }

                        wdfChunk.SplineSegments = new List<SplineSegment>();
                        for (int i = 0; i < wdfChunk.SplineSegmentCount; i++)
                        {
                            segments++;
                            var TempUnknown = new SplineSegment();

                            TempUnknown.Point4 = StreamUtil.ReadVector4(stream);
                            TempUnknown.Point3 = StreamUtil.ReadVector4(stream);
                            TempUnknown.Point2 = StreamUtil.ReadVector4(stream);
                            TempUnknown.ControlPoint = StreamUtil.ReadVector4(stream);

                            TempUnknown.U0 = StreamUtil.ReadFloat(stream);
                            TempUnknown.U1 = StreamUtil.ReadFloat(stream);
                            TempUnknown.U2 = StreamUtil.ReadFloat(stream);
                            TempUnknown.U3 = StreamUtil.ReadFloat(stream);

                            TempUnknown.PreviousWDFChunkID = StreamUtil.ReadInt16(stream);
                            TempUnknown.PreviousSegmentID = StreamUtil.ReadInt16(stream);
                            TempUnknown.NextWDFChunkID = StreamUtil.ReadInt16(stream);
                            TempUnknown.NextSegmentID = StreamUtil.ReadInt16(stream);
                            TempUnknown.SplineID = StreamUtil.ReadInt16(stream);
                            TempUnknown.U4 = StreamUtil.ReadInt16(stream);

                            TempUnknown.LowestXYZ = StreamUtil.ReadVector3(stream);
                            TempUnknown.HighestXYZ = StreamUtil.ReadVector3(stream);

                            TempUnknown.SegmentDisatnce = StreamUtil.ReadFloat(stream);
                            TempUnknown.PreviousSegmentsDistance = StreamUtil.ReadFloat(stream);
                            TempUnknown.U9 = StreamUtil.ReadUInt32(stream);

                            wdfChunk.SplineSegments.Add(TempUnknown);
                        }

                        wdfChunk.Lights = new List<Light>();
                        for (int i = 0; i < wdfChunk.LightCount; i++)
                        {
                            var TempUnknown = new Light();

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
                            TempUnknown.U21 = StreamUtil.ReadFloat(stream);

                            wdfChunk.Lights.Add(TempUnknown);
                        }

                        WDFChunks[x,y] = wdfChunk;
                    }

                }
            }
        }

        //public WDXHandler.WDFGridGroup[,] Save(string path, int row, int collum)
        //{
        //    WDXHandler.WDFGridGroup[,] wdfGridGroup = new WDXHandler.WDFGridGroup[row, collum];

        //    MemoryStream MainStream = new MemoryStream();

        //    for (int y = 0; y < WDFChunks.GetLength(1); y++)
        //    {
        //        for (int x = 0; x < WDFChunks.GetLength(0); x++)
        //        {
        //            MemoryStream Stream = new MemoryStream();

        //            var TempChunk = WDFChunks[x,y];

        //            StreamUtil.WriteVector3(Stream, TempChunk.U0);
        //            StreamUtil.WriteVector3(Stream, TempChunk.U1);
        //            StreamUtil.WriteVector3(Stream, TempChunk.U2);

        //            StreamUtil.WriteInt16(Stream, TempChunk.Instances.Count);
        //            StreamUtil.WriteInt16(Stream, TempChunk.Patches.Count);
        //            StreamUtil.WriteInt16(Stream, TempChunk.Lights.Count);
        //            StreamUtil.WriteInt16(Stream, TempChunk.SplineSegments.Count);
        //            StreamUtil.WriteInt16(Stream, TempChunk.unknownStruct1s.Count);
        //            StreamUtil.WriteInt16(Stream, TempChunk.U7);

        //            for (int i = 0; i < TempChunk.unknownStruct0s.Count; i++)
        //            {
        //                StreamUtil.WriteInt16(Stream, TempChunk.unknownStruct0s[i].U0);
        //                StreamUtil.WriteInt16(Stream, TempChunk.unknownStruct0s[i].U1);
        //                StreamUtil.WriteInt16(Stream, TempChunk.unknownStruct0s[i].U2);
        //                StreamUtil.WriteInt16(Stream, TempChunk.unknownStruct0s[i].U3);
        //                StreamUtil.WriteInt16(Stream, TempChunk.unknownStruct0s[i].U4);
        //                StreamUtil.WriteInt16(Stream, TempChunk.unknownStruct0s[i].U5);
        //                StreamUtil.WriteInt16(Stream, TempChunk.unknownStruct0s[i].U6);
        //                StreamUtil.WriteInt16(Stream, TempChunk.unknownStruct0s[i].U7);
        //                StreamUtil.WriteInt16(Stream, TempChunk.unknownStruct0s[i].U8);
        //                StreamUtil.WriteInt16(Stream, TempChunk.unknownStruct0s[i].U9);
        //                StreamUtil.WriteInt16(Stream, TempChunk.unknownStruct0s[i].U10);
        //                StreamUtil.WriteInt16(Stream, TempChunk.unknownStruct0s[i].U11);

        //                StreamUtil.WriteVector3(Stream, TempChunk.unknownStruct0s[i].U12);
        //                StreamUtil.WriteVector3(Stream, TempChunk.unknownStruct0s[i].U13);
        //            }

        //            for (int i = 0; i < TempChunk.unknownStruct1s.Count; i++)
        //            {
        //                StreamUtil.WriteInt16(Stream, TempChunk.unknownStruct1s[i].U0);
        //                StreamUtil.WriteInt16(Stream, TempChunk.unknownStruct1s[i].U1);
        //            }

        //            StreamUtil.AlignBy16(Stream);

        //            for (int i = 0; i < TempChunk.Instances.Count; i++)
        //            {
        //                StreamUtil.WriteMatrix4x4(Stream, TempChunk.Instances[i].matrix4X4);

        //                StreamUtil.WriteMatrix4x4(Stream, TempChunk.Instances[i].matrix4X41);
        //                StreamUtil.WriteVector4(Stream, TempChunk.Instances[i].vector40);
        //                StreamUtil.WriteVector4(Stream, TempChunk.Instances[i].vector41);
        //                StreamUtil.WriteVector4(Stream, TempChunk.Instances[i].vector42);
        //                StreamUtil.WriteVector4(Stream, TempChunk.Instances[i].vector43);

        //                StreamUtil.WriteInt16(Stream, TempChunk.Instances[i].WDFGridID);
        //                StreamUtil.WriteInt16(Stream, TempChunk.Instances[i].InstanceIndex);

        //                StreamUtil.WriteInt32(Stream, TempChunk.Instances[i].U2);
        //                StreamUtil.WriteInt32(Stream, TempChunk.Instances[i].U3);
        //                StreamUtil.WriteInt32(Stream, TempChunk.Instances[i].PrefabID);

        //                StreamUtil.WriteVector3(Stream, TempChunk.Instances[i].LowestXYZ);
        //                StreamUtil.WriteVector3(Stream, TempChunk.Instances[i].HighestXYZ);

        //                StreamUtil.WriteInt16(Stream, TempChunk.Instances[i].U5);
        //                StreamUtil.WriteInt16(Stream, TempChunk.Instances[i].BitFlags);

        //                StreamUtil.WriteFloat32(Stream, TempChunk.Instances[i].PlayerBounce);

        //                StreamUtil.WriteInt16(Stream, TempChunk.Instances[i].CollsionMode);

        //                StreamUtil.WriteInt16(Stream, TempChunk.Instances[i].CollisonModelIndex);
        //                StreamUtil.WriteInt16(Stream, TempChunk.Instances[i].PhysicsIndex);
        //                StreamUtil.WriteInt16(Stream, TempChunk.Instances[i].U11);

        //                StreamUtil.WriteFloat32(Stream, TempChunk.Instances[i].U12);
        //                StreamUtil.WriteInt32(Stream, TempChunk.Instances[i].U13);

        //                StreamUtil.WriteInt32(Stream, TempChunk.Instances[i].U14);
        //                StreamUtil.WriteInt32(Stream, TempChunk.Instances[i].U15);
        //                StreamUtil.WriteInt32(Stream, TempChunk.Instances[i].U16);
        //                StreamUtil.WriteInt32(Stream, TempChunk.Instances[i].U17);
        //            }

        //            for (int i = 0; i < TempChunk.Patches.Count; i++)
        //            {
        //                var TempPatch = TempChunk.Patches[i];
        //                StreamUtil.WriteVector4(Stream, TempPatch.LightMapPoint);

        //                StreamUtil.WriteVector4(Stream, TempPatch.UVPoint1);
        //                StreamUtil.WriteVector4(Stream, TempPatch.UVPoint2);
        //                StreamUtil.WriteVector4(Stream, TempPatch.UVPoint3);
        //                StreamUtil.WriteVector4(Stream, TempPatch.UVPoint4);

        //                StreamUtil.WriteVector4(Stream, TempPatch.R4C4);
        //                StreamUtil.WriteVector4(Stream, TempPatch.R4C3);
        //                StreamUtil.WriteVector4(Stream, TempPatch.R4C2);
        //                StreamUtil.WriteVector4(Stream, TempPatch.R4C1);
        //                StreamUtil.WriteVector4(Stream, TempPatch.R3C4);
        //                StreamUtil.WriteVector4(Stream, TempPatch.R3C3);
        //                StreamUtil.WriteVector4(Stream, TempPatch.R3C2);
        //                StreamUtil.WriteVector4(Stream, TempPatch.R3C1);
        //                StreamUtil.WriteVector4(Stream, TempPatch.R2C4);
        //                StreamUtil.WriteVector4(Stream, TempPatch.R2C3);
        //                StreamUtil.WriteVector4(Stream, TempPatch.R2C2);
        //                StreamUtil.WriteVector4(Stream, TempPatch.R2C1);
        //                StreamUtil.WriteVector4(Stream, TempPatch.R1C4);
        //                StreamUtil.WriteVector4(Stream, TempPatch.R1C3);
        //                StreamUtil.WriteVector4(Stream, TempPatch.R1C2);
        //                StreamUtil.WriteVector4(Stream, TempPatch.R1C1);

        //                StreamUtil.WriteVector4(Stream, TempPatch.LowestXYZ);
        //                StreamUtil.WriteVector4(Stream, TempPatch.HighestXYZ);

        //                StreamUtil.WriteVector4(Stream, TempPatch.Point1);
        //                StreamUtil.WriteVector4(Stream, TempPatch.Point2);
        //                StreamUtil.WriteVector4(Stream, TempPatch.Point3);
        //                StreamUtil.WriteVector4(Stream, TempPatch.Point4);

        //                StreamUtil.WriteInt32(Stream, TempPatch.TextureID);
        //                StreamUtil.WriteInt16(Stream, TempPatch.LightmapID); //41
        //                StreamUtil.WriteInt16(Stream, TempPatch.PatchType);
        //                StreamUtil.WriteInt16(Stream, TempPatch.U0);
        //            }

        //            for (int i = 0; i < TempChunk.SplineSegments.Count; i++)
        //            {
        //                StreamUtil.WriteMatrix4x4(Stream, TempChunk.SplineSegments[i].matrix4X4);
        //                StreamUtil.WriteVector4(Stream, TempChunk.SplineSegments[i].vector4);

        //                StreamUtil.WriteInt16(Stream, TempChunk.SplineSegments[i].U0);
        //                StreamUtil.WriteInt16(Stream, TempChunk.SplineSegments[i].U1);
        //                StreamUtil.WriteInt16(Stream, TempChunk.SplineSegments[i].U2);
        //                StreamUtil.WriteInt16(Stream, TempChunk.SplineSegments[i].U3);
        //                StreamUtil.WriteInt16(Stream, TempChunk.SplineSegments[i].U4);
        //                StreamUtil.WriteInt16(Stream, TempChunk.SplineSegments[i].U5);

        //                StreamUtil.WriteVector3(Stream, TempChunk.SplineSegments[i].U6);
        //                StreamUtil.WriteVector3(Stream, TempChunk.SplineSegments[i].U7);

        //                StreamUtil.WriteFloat32(Stream, TempChunk.SplineSegments[i].U71);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.SplineSegments[i].U8);
        //                StreamUtil.WriteInt32(Stream, TempChunk.SplineSegments[i].U9);
        //            }

        //            for (int i = 0; i < TempChunk.Lights.Count; i++)
        //            {
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U0);

        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U1);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U2);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U3);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U4);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U5);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U6);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U7);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U8);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U9);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U10);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U11);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U12);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U13);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U14);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U15);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U16);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U17);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U18);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U19);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U20);
        //                StreamUtil.WriteFloat32(Stream, TempChunk.Lights[i].U21);
        //            }

        //            var GridGroup = new WDXHandler.WDFGridGroup();
        //            GridGroup.Offset = (int)MainStream.Position;
        //            StreamUtil.WriteStreamIntoStream(MainStream, Stream);
        //            GridGroup.Size = (int)MainStream.Position - GridGroup.Offset;
        //            wdfGridGroup[x, y] = GridGroup;
        //        }
        //    }


        //    if (File.Exists(path))
        //    {
        //        File.Delete(path);
        //    }
        //    var file = File.Create(path);
        //    MainStream.Position = 0;
        //    MainStream.CopyTo(file);
        //    MainStream.Dispose();
        //    file.Close();

        //    return wdfGridGroup;
        //}

        public struct WDFChunk
        {
            public int ChunkID;

            public Vector3 U0;
            public Vector3 U1;
            public Vector3 U2;
            public int InstanceCount;
            public int PatchesCount;
            public int LightCount;
            public int SplineSegmentCount;
            public int UstructCount1;
            public int U7;

            public List<UnknownStruct0> unknownStruct0s; //16 on all levels
            public List<UnknownStruct1> unknownStruct1s;
            //Align by 16

            public List<Instance> Instances;
            public List<Patch> Patches;
            public List<SplineSegment> SplineSegments;
            public List<Light> Lights;
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
        public struct Instance
        {
            public Matrix4x4 matrix4X4;

            //Colour Data ?
            public Matrix4x4 lightingMatrix4x4;

            public Vector4 LightColour1;
            public Vector4 LightColour2;
            public Vector4 LightColour3;
            public Vector4 AmbentLightColour;

            public int WDFGridID; //16
            public int InstanceIndex; //16

            public int U2;
            public int U3;
            public int PrefabID;

            public Vector3 LowestXYZ;
            public Vector3 HighestXYZ;

            public int U5; //16
            public int BitFlags; //Bit Flags

            public float PlayerBounce;

            public int CollsionMode; //16
            public int CollisonModelIndex; //16
            public int PhysicsIndex; //16
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
            public int LightmapID;
            public int PatchType;
            public int U0;
        }

        //88
        public struct Light
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
            public float U21;
        }

        public struct SplineSegment
        {
            public bool Loaded;

            public Vector4 Point4;
            public Vector4 Point3;
            public Vector4 Point2;
            public Vector4 ControlPoint;

            public float U0;
            public float U1;
            public float U2;
            public float U3;

            //16
            public int PreviousWDFChunkID;
            public int PreviousSegmentID;
            public int NextWDFChunkID;
            public int NextSegmentID;
            public int SplineID;
            public int U4;

            public Vector3 LowestXYZ;
            public Vector3 HighestXYZ;
            public float SegmentDisatnce;
            public float PreviousSegmentsDistance;
            public int U9;
        }
    }
}
