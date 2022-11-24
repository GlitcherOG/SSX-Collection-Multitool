using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2
{
    internal class LTGHandler
    {
        public byte Unknown;
        public byte ColdFusionVersion;
        public byte ColdFusionRevision;
        public byte endianess;

        public Vector3 WorldBounds1;
        public Vector3 WorldBounds2;
        public Vector3 WorldBounds3;

        public float mainBboxSize;
        public int pointerCount;
        public int pointerListCount;
        public int totalGridCount;
        public int mainBboxCount;
        public int mainBboxEmptyCount;

        public float nodeBoxSize;
        public int nodeBoxWidth;
        public int nodeBoxCount;

        public int pointerListOffset;
        public int bboxDataListOffset;

        public int[,]? offsetList;

        public mainBbox[,]? mainBboxes;

        public void LoadLTG(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                Unknown = StreamUtil.ReadByte(stream);
                ColdFusionVersion = StreamUtil.ReadByte(stream);
                ColdFusionRevision = StreamUtil.ReadByte(stream);
                endianess = StreamUtil.ReadByte(stream);

                WorldBounds1 = StreamUtil.ReadVector3(stream);
                WorldBounds2 = StreamUtil.ReadVector3(stream);
                WorldBounds3 = StreamUtil.ReadVector3(stream);

                mainBboxSize = StreamUtil.ReadFloat(stream);
                pointerCount = StreamUtil.ReadInt32(stream);
                pointerListCount = StreamUtil.ReadInt32(stream);
                totalGridCount = StreamUtil.ReadInt32(stream);
                mainBboxCount = StreamUtil.ReadInt32(stream);
                mainBboxEmptyCount = StreamUtil.ReadInt32(stream);

                nodeBoxSize = StreamUtil.ReadFloat(stream);
                nodeBoxWidth = StreamUtil.ReadInt32(stream);
                nodeBoxCount = StreamUtil.ReadInt32(stream);

                pointerListOffset = StreamUtil.ReadInt32(stream);
                bboxDataListOffset = StreamUtil.ReadInt32(stream);

                offsetList = new int[pointerCount, pointerListCount];
                stream.Position = pointerListOffset;
                for (int y = 0; y < pointerListCount; y++)
                {
                    for (int x = 0; x < pointerCount; x++)
                    {
                        offsetList[x, y] = StreamUtil.ReadInt32(stream);
                    }
                }

                mainBboxes = new mainBbox[pointerCount, pointerListCount];
                for (int y = 0; y < pointerListCount; y++)
                {
                    for (int x = 0; x < pointerCount; x++)
                    {
                        if (offsetList[x, y] != 0)
                        {
                            stream.Position = offsetList[x, y];
                            mainBbox tempbBox = new mainBbox();
                            tempbBox.WorldBounds1 = StreamUtil.ReadVector3(stream);
                            tempbBox.WorldBounds2 = StreamUtil.ReadVector3(stream);
                            tempbBox.WorldBounds3 = StreamUtil.ReadVector3(stream);

                            tempbBox.totalPatchCount = StreamUtil.ReadInt16(stream);
                            tempbBox.totalInstanceCount = StreamUtil.ReadInt16(stream);
                            tempbBox.unknown = StreamUtil.ReadInt16(stream);
                            tempbBox.totalLightCount = StreamUtil.ReadInt16(stream);
                            tempbBox.totallightsCrossingCount = StreamUtil.ReadInt16(stream);
                            tempbBox.totalParticleInstanceCount = StreamUtil.ReadInt16(stream);
                            tempbBox.Unknown1 = StreamUtil.ReadInt32(stream); // number of elements?
                            tempbBox.Unknown2 = StreamUtil.ReadInt32(stream); // offset to first nodeBbox? or mainBbox byte size
                            tempbBox.Unknown3 = StreamUtil.ReadInt32(stream); // index list offset
                            tempbBox.Unknown4 = StreamUtil.ReadInt32(stream);
                            tempbBox.Unknown5 = StreamUtil.ReadInt32(stream);
                            tempbBox.Unknown6 = StreamUtil.ReadInt32(stream);
                            tempbBox.Unknown7 = StreamUtil.ReadInt32(stream); // offset leads to list of extraThing lists
                            tempbBox.Unknown8 = StreamUtil.ReadInt32(stream);
                            tempbBox.Unknown9 = StreamUtil.ReadInt32(stream);

                            tempbBox.nodeBBoxes = new nodeBBox[nodeBoxWidth, nodeBoxWidth];

                            for (int y1 = 0; y1 < nodeBoxWidth; y1++)
                            {
                                for (int x1 = 0; x1 < nodeBoxWidth; x1++)
                                {
                                    nodeBBox tempNode = new nodeBBox();
                                    tempNode.WorldBounds1 = StreamUtil.ReadVector3(stream);
                                    tempNode.WorldBounds2 = StreamUtil.ReadVector3(stream);
                                    tempNode.WorldBounds3 = StreamUtil.ReadVector3(stream);

                                    tempNode.patchCount = StreamUtil.ReadInt16(stream);
                                    tempNode.instanceCount = StreamUtil.ReadInt16(stream);
                                    tempNode.instAndGemCount = StreamUtil.ReadInt16(stream);
                                    tempNode.splineCount = StreamUtil.ReadInt16(stream);
                                    tempNode.lightCount = StreamUtil.ReadInt16(stream);
                                    tempNode.lightsCrossingCount = StreamUtil.ReadInt16(stream);
                                    tempNode.particleCount = StreamUtil.ReadInt32(stream);

                                    tempNode.patchesOffset = StreamUtil.ReadInt32(stream);
                                    tempNode.instancesOffset = StreamUtil.ReadInt32(stream);
                                    tempNode.splinesOffset = StreamUtil.ReadInt32(stream);
                                    tempNode.lightsOffset = StreamUtil.ReadInt32(stream);
                                    tempNode.lightsCrossingOffset = StreamUtil.ReadInt32(stream);
                                    tempNode.particleModelsOffset = StreamUtil.ReadInt32(stream);

                                    tempNode.PatchIndex = ReadOffsetData(stream, offsetList[x, y] + tempNode.patchesOffset, tempNode.patchCount);
                                    tempNode.InstanceIndex = ReadOffsetData(stream, offsetList[x, y] + tempNode.instancesOffset, tempNode.instanceCount);
                                    tempNode.InstAndGemIndex = ReadOffsetData(stream, offsetList[x, y] + tempNode.instancesOffset, tempNode.instAndGemCount);
                                    tempNode.SplineIndex = ReadOffsetData(stream, offsetList[x, y] + tempNode.splinesOffset, tempNode.splineCount);
                                    tempNode.LightIndex = ReadOffsetData(stream, offsetList[x, y] + tempNode.lightsOffset, tempNode.lightCount);
                                    tempNode.LightCrossingIndex = ReadOffsetDataCorssing(stream, offsetList[x, y] + tempNode.lightsCrossingOffset, tempNode.lightsCrossingCount);
                                    tempNode.ParticleIndex = ReadOffsetData(stream, offsetList[x, y] + tempNode.particleModelsOffset, tempNode.particleCount);

                                    tempbBox.nodeBBoxes[x1, y1] = tempNode;
                                }
                            }

                            mainBboxes[x, y] = tempbBox;
                        }
                    }
                }
            }
        }

        public void RegenerateAndSaveLTG(PBDHandler pbdHandler, string path)
        {
            MemoryStream MainFileStream = new MemoryStream();

            //Generate Array Bounds
            Vector3 TopLeft = new Vector3(WorldBounds1.X, WorldBounds2.Y, 0);
            Vector3 BottomRight = new Vector3(WorldBounds2.X, WorldBounds1.Y, 0);
            Vector3 StartTopLeft;
            Vector3 EndBottomRight;

            Vector3 Size;

            float TempXPosition = MathF.Round(TopLeft.X / (nodeBoxSize * 4), MidpointRounding.ToZero);
            if (TopLeft.X % (nodeBoxSize * 4)!=0)
            {
                TempXPosition += -1;
            }
            StartTopLeft.X = TempXPosition;
            float TempYPosition = MathF.Round(TopLeft.Y / (nodeBoxSize * 4), MidpointRounding.ToZero);
            if (TopLeft.Y % (nodeBoxSize * 4) != 0)
            {
                TempYPosition += 1;
            }
            StartTopLeft.Y = TempYPosition;
            TempXPosition = MathF.Round(BottomRight.X / (nodeBoxSize * 4), MidpointRounding.ToZero);
            if (BottomRight.X % (nodeBoxSize * 4) != 0)
            {
                TempXPosition += 1;
            }
            EndBottomRight.X = TempXPosition;
            TempYPosition = MathF.Round(BottomRight.Y / (nodeBoxSize * 4), MidpointRounding.ToZero);
            if (BottomRight.Y % (nodeBoxSize * 4) != 0)
            {
                TempYPosition += -1;
            }
            EndBottomRight.Y = TempYPosition;

            Size.X = EndBottomRight.X - StartTopLeft.X;
            Size.Y = StartTopLeft.Y - EndBottomRight.Y;

            mainBboxes = new mainBbox[(int)Size.X, (int)Size.Y];


            //if (File.Exists(path))
            //{
            //    File.Delete(path);
            //}
            //var file = File.Create(path);
            //MainFileStream.Position = 0;
            //MainFileStream.CopyTo(file);
            //MainFileStream.Dispose();
            //file.Close();
        }


        public List<int> ReadOffsetData(Stream stream, int offset, int count)
        {
            int OldPos = (int)stream.Position;
            List<int> ints = new List<int>();
            stream.Position = offset;
            for (int i = 0; i < count; i++)
            {
                ints.Add(StreamUtil.ReadInt32(stream));
            }
            stream.Position = OldPos;
            return ints;
        }

        public List<lightCrossing> ReadOffsetDataCorssing(Stream stream, int offset, int count)
        {
            int OldPos = (int)stream.Position;
            List<lightCrossing> ints = new List<lightCrossing>();
            stream.Position = offset;
            for (int i = 0; i < count; i++)
            {
                var Temp = new lightCrossing();
                Temp.Int1 = StreamUtil.ReadInt32(stream);
                Temp.Int2 = StreamUtil.ReadInt32(stream);
                ints.Add(Temp);
            }
            stream.Position = OldPos;
            return ints;
        }


        public struct mainBbox
        {
            public bool Modified;

            public Vector3 WorldBounds1;
            public Vector3 WorldBounds2;
            public Vector3 WorldBounds3;

            public int totalPatchCount;            // Total Patch count
            public int totalInstanceCount;         // Total Instance count
            public int unknown;                    
            public int totalLightCount;            // Total Light count
            public int totallightsCrossingCount;   // Total Lights crossing count. Whatever that means
            public int totalParticleInstanceCount;
            public int Unknown1; // number of elements?
            
            public int Unknown2; // offset to first nodeBbox? or mainBbox byte size
            public int Unknown3; // index list offset
            public int Unknown4;
            public int Unknown5;
            public int Unknown6;
            public int Unknown7; // offset leads to list of extraThing lists
            public int Unknown8;
            public int Unknown9;

            //Num Num Patches
            //Num Instance
            //Num Particle Instance
            //Num Splines
            //Num Lights Inside
            //Num Lights Crossing
            //Num Elements

            public nodeBBox[,] nodeBBoxes;
        }

        public struct nodeBBox
        {
            public bool Modified;

            public Vector3 WorldBounds1;
            public Vector3 WorldBounds2;
            public Vector3 WorldBounds3;

            public int patchCount;          // Patch count
            public int instanceCount;       // Instance count
            public int instAndGemCount;     // Models/Instances & Gems apparently
            public int splineCount;         // Spline count
            public int lightCount;          // Light count
            public int lightsCrossingCount; // Lights crossing count
            public int particleCount;       // Particle model count

            public int patchesOffset;        // offset leads to it's own index list
            public int instancesOffset;      // or models
            public int splinesOffset;
            public int lightsOffset;
            public int lightsCrossingOffset; // offset of it's own extraThing list, usually hex 00000000 01000000
            public int particleModelsOffset;

            public List<int> PatchIndex;
            public List<int> InstanceIndex;
            public List<int> InstAndGemIndex;
            public List<int> SplineIndex;
            public List<int> LightIndex;
            public List<lightCrossing> LightCrossingIndex;
            public List<int> ParticleIndex;
        }

        public struct lightCrossing
        {
            public int Int1;
            public int Int2;
        }

    }
}
