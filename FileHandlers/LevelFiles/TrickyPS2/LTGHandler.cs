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
    public class LTGHandler
    {
        public byte Unknown;
        public byte ColdFusionVersion;
        public byte ColdFusionRevision;
        public byte endianess = 0x00;

        public Vector3 WorldBounds1;
        public Vector3 WorldBounds2;
        public Vector3 WorldBounds3;

        public float mainBboxSize = 10000f; //Done
        public int pointerCount; //Generating
        public int pointerListCount; //Generating
        public int totalGridCount; //Generating
        public int mainBboxCount; 
        public int mainBboxEmptyCount;

        public float nodeBoxSize = 2500f; //Generating
        public int nodeBoxWidth = 4; //Generating
        public int nodeBoxCount = 16; //Generating

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
                            tempbBox.totalSplineCount = StreamUtil.ReadInt16(stream);
                            tempbBox.totalLightCount = StreamUtil.ReadInt16(stream);
                            tempbBox.totalLightsCrossingCount = StreamUtil.ReadInt16(stream);
                            tempbBox.totalParticleInstanceCount = StreamUtil.ReadInt16(stream);
                            tempbBox.Unknown1 = StreamUtil.ReadInt32(stream); // Unknown
                            tempbBox.totalElements = StreamUtil.ReadInt32(stream); // Total Elements
                            tempbBox.mainBoxHeaderSize = StreamUtil.ReadInt32(stream); // Mainbox Header Size?
                            tempbBox.patchIndexOffset = StreamUtil.ReadInt32(stream); // Patch Index Offset
                            tempbBox.instanceIndexOffset = StreamUtil.ReadInt32(stream); // Instance Index Offset
                            tempbBox.splineIndexOffset = StreamUtil.ReadInt32(stream); // Spline Index Offset
                            tempbBox.lightIndexOffset = StreamUtil.ReadInt32(stream); // Light Index Offset
                            tempbBox.lightCrossingIndexOffset = StreamUtil.ReadInt32(stream); // Light Corssing Index Offset
                            tempbBox.particleIndexOffset = StreamUtil.ReadInt32(stream); // Particle Index Offset

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
                                    tempNode.InstAndGemIndex = ReadOffsetData(stream, offsetList[x, y] + tempNode.instancesOffset, tempNode.instAndGemCount);
                                    tempNode.SplineIndex = ReadOffsetData(stream, offsetList[x, y] + tempNode.splinesOffset, tempNode.splineCount);
                                    tempNode.LightIndex = ReadOffsetData(stream, offsetList[x, y] + tempNode.lightsOffset, tempNode.lightCount);
                                    tempNode.LightCrossingIndex = ReadOffsetData(stream, offsetList[x, y] + tempNode.lightsCrossingOffset, tempNode.lightsCrossingCount);
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

        public void RegenerateLTG(PBDHandler pbdHandler, float NewMainBoxSize = 10000f, int NewNodeWidth = 4)
        {
            mainBboxSize = NewMainBoxSize;
            nodeBoxWidth = NewNodeWidth;
            nodeBoxSize = mainBboxSize / (float)nodeBoxWidth;
            nodeBoxCount = nodeBoxWidth * nodeBoxWidth;

            //Generate Array Bounds
            Vector3 BottomLeft = new Vector3(WorldBounds1.X, WorldBounds1.Y, 0);
            Vector3 TopRight = new Vector3(WorldBounds2.X, WorldBounds2.Y, 0);
            Vector3 StartBottomLeft;
            Vector3 EndTopRight;

            Vector3 Size;

            float TempXPosition = (int)(BottomLeft.X / mainBboxSize);
            if (BottomLeft.X % mainBboxSize != 0)
            {
                TempXPosition += -1;
            }
            StartBottomLeft.X = TempXPosition;
            float TempYPosition = (int)(BottomLeft.Y / mainBboxSize);
            if (BottomLeft.Y % mainBboxSize != 0)
            {
                TempYPosition += -1;
            }
            StartBottomLeft.Y = TempYPosition;
            TempXPosition = (int)(TopRight.X / mainBboxSize);
            if (TopRight.X % mainBboxSize != 0)
            {
                TempXPosition += 1;
            }
            EndTopRight.X = TempXPosition;
            TempYPosition = (int)(TopRight.Y / mainBboxSize);
            if (TopRight.Y % mainBboxSize != 0)
            {
                TempYPosition += 1;
            }
            EndTopRight.Y = TempYPosition;

            Size.X = EndTopRight.X - StartBottomLeft.X;
            Size.Y = EndTopRight.Y - StartBottomLeft.Y;

            //Generate MainBoxes
            mainBboxes = new mainBbox[(int)Size.X, (int)Size.Y];

            pointerCount = (int)Size.X;
            pointerListCount = (int)Size.Y;
            totalGridCount = pointerCount * pointerListCount;

            for (int y = 0; y < (int)Size.Y; y++)
            {
                for (int x = 0; x < (int)Size.X; x++)
                {
                    var TempMainBox = mainBboxes[x, y];

                    TempMainBox.WorldBounds1 = new Vector3((StartBottomLeft.X+x)* mainBboxSize, (StartBottomLeft.Y+y)* mainBboxSize, 0);
                    TempMainBox.WorldBounds2 = new Vector3((StartBottomLeft.X + x) * mainBboxSize, (StartBottomLeft.Y + y) * mainBboxSize, 0);
                    TempMainBox.WorldBounds2 += new Vector3(mainBboxSize, mainBboxSize, 0);
                    TempMainBox.WorldBounds3 = Vector3.Lerp(TempMainBox.WorldBounds1, TempMainBox.WorldBounds2, 0.5f);

                    TempMainBox.nodeBBoxes = new nodeBBox[nodeBoxWidth, nodeBoxWidth];
                    for (int y1 = 0; y1 < nodeBoxWidth; y1++)
                    {
                        for (int x1 = 0; x1 < nodeBoxWidth; x1++)
                        {
                            var TempNode = TempMainBox.nodeBBoxes[x1, y1];

                            TempNode.WorldBounds1 = new Vector3(TempMainBox.WorldBounds1.X + (nodeBoxSize)*x1, TempMainBox.WorldBounds1.Y + (nodeBoxSize) * y1, 0);
                            TempNode.WorldBounds2 = new Vector3(TempMainBox.WorldBounds1.X + (nodeBoxSize) * (x1+1), TempMainBox.WorldBounds1.Y + (nodeBoxSize) * (y1+1), 0);
                            TempNode.WorldBounds3 = Vector3.Lerp(TempNode.WorldBounds1, TempNode.WorldBounds2, 0.5f);

                            TempMainBox.nodeBBoxes[x1, y1] = TempNode;
                        }
                    }

                    mainBboxes[x, y] = TempMainBox;
                }
            }


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

        [System.Serializable]
        public struct mainBbox
        {
            public bool Modified;

            public Vector3 WorldBounds1;
            public Vector3 WorldBounds2;
            public Vector3 WorldBounds3;

            public int totalPatchCount;            // Total Patch count
            public int totalInstanceCount;         // Total Instance and Gem count
            public int totalSplineCount;                    
            public int totalLightCount;            // Total Light count
            public int totalLightsCrossingCount;   // Total Lights crossing count. Whatever that means
            public int totalParticleInstanceCount;

            public int Unknown1;
            
            public int totalElements; 
            public int mainBoxHeaderSize; 
            public int patchIndexOffset;
            public int instanceIndexOffset;
            public int splineIndexOffset;
            public int lightIndexOffset; 
            public int lightCrossingIndexOffset;
            public int particleIndexOffset;

            //Num Num Patches
            //Num Instance
            //Num Particle Instance
            //Num Splines
            //Num Lights Inside
            //Num Lights Crossing
            //Num Elements

            public nodeBBox[,] nodeBBoxes;
        }
        [System.Serializable]
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
            public List<int> InstAndGemIndex;
            public List<int> SplineIndex;
            public List<int> LightIndex;
            public List<int> LightCrossingIndex;
            public List<int> ParticleIndex;
        }

    }
}
