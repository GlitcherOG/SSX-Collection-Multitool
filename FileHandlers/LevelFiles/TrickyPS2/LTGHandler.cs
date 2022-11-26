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
        public byte Unknown = 0x00;
        public byte ColdFusionVersion = 0x15;
        public byte ColdFusionRevision = 0x1B;
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
                            tempbBox.Modified = true;
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
                                    tempNode.Modified = true;
                                    tempNode.WorldBounds1 = StreamUtil.ReadVector3(stream);
                                    tempNode.WorldBounds2 = StreamUtil.ReadVector3(stream);
                                    tempNode.WorldBounds3 = StreamUtil.ReadVector3(stream);

                                    tempNode.patchCount = StreamUtil.ReadInt16(stream);
                                    tempNode.instanceCount = StreamUtil.ReadInt16(stream);
                                    tempNode.instAndGemCount = StreamUtil.ReadInt16(stream);
                                    tempNode.splineCount = StreamUtil.ReadInt16(stream);
                                    tempNode.lightCount = StreamUtil.ReadInt16(stream);
                                    tempNode.lightsCrossingCount = StreamUtil.ReadInt16(stream);
                                    tempNode.particleCount = StreamUtil.ReadInt16(stream);
                                    tempNode.Unknown1 = StreamUtil.ReadInt16(stream);

                                    tempNode.patchesOffset = StreamUtil.ReadInt32(stream);
                                    tempNode.instancesOffset = StreamUtil.ReadInt32(stream);
                                    tempNode.splinesOffset = StreamUtil.ReadInt32(stream);
                                    tempNode.lightsOffset = StreamUtil.ReadInt32(stream);
                                    tempNode.lightsCrossingOffset = StreamUtil.ReadInt32(stream);
                                    tempNode.particleModelsOffset = StreamUtil.ReadInt32(stream);

                                    tempNode.PatchIndex = ReadOffsetData(stream, offsetList[x, y] + tempNode.patchesOffset, tempNode.patchCount);
                                    if (tempNode.instAndGemCount >= tempNode.instanceCount)
                                    {
                                        tempNode.InstanceIndex = ReadOffsetData(stream, offsetList[x, y] + tempNode.instancesOffset, tempNode.instanceCount);
                                        tempNode.RaceInstanceIndex = new List<int>();
                                        tempNode.GemIndex = ReadOffsetData(stream, offsetList[x, y] + tempNode.instancesOffset + tempNode.instanceCount * 4, tempNode.instAndGemCount - tempNode.instanceCount);
                                    }
                                    else
                                    {
                                        tempNode.InstanceIndex = ReadOffsetData(stream, offsetList[x, y] + tempNode.instancesOffset, tempNode.instAndGemCount);
                                        tempNode.RaceInstanceIndex = ReadOffsetData(stream, offsetList[x, y] + tempNode.instancesOffset + tempNode.instAndGemCount * 4, tempNode.instanceCount - tempNode.instAndGemCount);
                                        tempNode.GemIndex = new List<int>();
                                    }
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
            int UsedBoxesCount = 0;

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

            bool[] Patchbools = new bool[pbdHandler.Patches.Count];
            bool[] InstanceBools = new bool[pbdHandler.Instances.Count];
            //SetBoxes
            for (int y = 0; y < pointerListCount; y++)
            {
                for (int x = 0; x < pointerCount; x++)
                {
                    //Find Out Whats In Box Patches
                    var TempMainBox = mainBboxes[x, y];
                    TempMainBox.patchIndex = new List<int>();
                    for (int i = 0; i < pbdHandler.Patches.Count; i++)
                    {
                        Vector3 MidPoint = Vector3.Lerp(pbdHandler.Patches[i].LowestXYZ, pbdHandler.Patches[i].HighestXYZ, 0.5f);

                        if (JsonUtil.WithinXY(MidPoint, TempMainBox.WorldBounds1, TempMainBox.WorldBounds2) && !Patchbools[i])
                        {
                            TempMainBox.Modified = true;
                            Patchbools[i] = true;
                            TempMainBox.patchIndex.Add(i);
                        }
                    }
                    //Find Out Whats In Box Instances
                    TempMainBox.instanceIndex = new List<int>();
                    for (int i = 0; i < pbdHandler.Instances.Count; i++)
                    {
                        Vector3 MidPoint = Vector3.Lerp(pbdHandler.Instances[i].LowestXYZ, pbdHandler.Instances[i].HighestXYZ, 0.5f);

                        if (JsonUtil.WithinXY(MidPoint, TempMainBox.WorldBounds1, TempMainBox.WorldBounds2) && !InstanceBools[i])
                        {
                            TempMainBox.Modified = true;
                            InstanceBools[i] = true;
                            TempMainBox.instanceIndex.Add(i);
                        }
                    }

                    //Add To Nodes
                    bool[] NodeInstanceBools = new bool[TempMainBox.instanceIndex.Count];
                    bool[] NodePatchBools = new bool[TempMainBox.patchIndex.Count];
                    for (int y1 = 0; y1 < nodeBoxWidth; y1++)
                    {
                        for (int x1 = 0; x1 < nodeBoxWidth; x1++)
                        {
                            var TempNodeBox = TempMainBox.nodeBBoxes[x1, y1];
                            //Generate Patches List
                            TempNodeBox.PatchIndex = new List<int>();
                            for (int i = 0; i < TempMainBox.patchIndex.Count; i++)
                            {
                                Vector3 MidPoint = Vector3.Lerp(pbdHandler.Patches[TempMainBox.patchIndex[i]].LowestXYZ, pbdHandler.Patches[TempMainBox.patchIndex[i]].HighestXYZ, 0.5f);
                                if (JsonUtil.WithinXY(MidPoint, TempNodeBox.WorldBounds1, TempNodeBox.WorldBounds2) && !NodePatchBools[i])
                                {
                                    TempNodeBox.Modified = true;
                                    NodePatchBools[i] = true;
                                    TempNodeBox.PatchIndex.Add(TempMainBox.patchIndex[i]);
                                }
                            }
                            //Generate Instance List
                            TempNodeBox.InstanceIndex = new List<int>();
                            TempNodeBox.GemIndex = new List<int>();
                            TempNodeBox.RaceInstanceIndex = new List<int>();
                            for (int i = 0; i < TempMainBox.instanceIndex.Count; i++)
                            {
                                Vector3 MidPoint = Vector3.Lerp(pbdHandler.Instances[TempMainBox.instanceIndex[i]].LowestXYZ, pbdHandler.Instances[TempMainBox.instanceIndex[i]].HighestXYZ, 0.5f);
                                if (JsonUtil.WithinXY(MidPoint, TempNodeBox.WorldBounds1, TempNodeBox.WorldBounds2) && !NodeInstanceBools[i])
                                {
                                    TempNodeBox.Modified = true;
                                    if (pbdHandler.Instances[TempMainBox.instanceIndex[i]].LTGState == 1 && TempNodeBox.GemIndex.Count == 0)
                                    {
                                        NodeInstanceBools[i] = true;
                                        TempNodeBox.RaceInstanceIndex.Add(TempMainBox.instanceIndex[i]);
                                    }
                                    else if (pbdHandler.Instances[TempMainBox.instanceIndex[i]].LTGState == 2 && TempNodeBox.RaceInstanceIndex.Count == 0)
                                    {
                                        NodeInstanceBools[i] = true;
                                        TempNodeBox.GemIndex.Add(TempMainBox.instanceIndex[i]);
                                    }
                                    else if (pbdHandler.Instances[TempMainBox.instanceIndex[i]].LTGState == 0)
                                    {
                                        NodeInstanceBools[i] = true;
                                        TempNodeBox.InstanceIndex.Add(TempMainBox.instanceIndex[i]);
                                    }
                                    else if (pbdHandler.Instances[TempMainBox.instanceIndex[i]].LTGState == -1)
                                    {
                                        NodeInstanceBools[i] = true;
                                    }
                                }
                            }
                            TempMainBox.nodeBBoxes[x1, y1] = TempNodeBox;
                        }
                    }

                    //Check All Instances Were Added To Nodes
                    if(NodeInstanceBools.Contains(false))
                    {
                        for (int i = 0; i < NodeInstanceBools.Length; i++)
                        {
                            if (NodeInstanceBools[i]==false)
                            {
                                for (int y1 = 0; y1 < nodeBoxWidth; y1++)
                                {
                                    for (int x1 = 0; x1 < nodeBoxWidth; x1++)
                                    {
                                        if (NodeInstanceBools[i] == true)
                                        {
                                            break;
                                        }
                                        var TempNodeBox = TempMainBox.nodeBBoxes[x1, y1];
                                        if (pbdHandler.Instances[TempMainBox.instanceIndex[i]].LTGState == 1 && TempNodeBox.GemIndex.Count == 0)
                                        {
                                            NodeInstanceBools[i] = true;
                                            TempNodeBox.RaceInstanceIndex.Add(TempMainBox.instanceIndex[i]);
                                        }
                                        else if (pbdHandler.Instances[TempMainBox.instanceIndex[i]].LTGState == 2 && TempNodeBox.RaceInstanceIndex.Count == 0)
                                        {
                                            NodeInstanceBools[i] = true;
                                            TempNodeBox.GemIndex.Add(TempMainBox.instanceIndex[i]);
                                        }
                                        TempMainBox.nodeBBoxes[x1, y1] = TempNodeBox;
                                    }
                                }
                            }
                        }
                        if (NodeInstanceBools.Contains(false))
                        {
                            for (int i = 0; i < NodeInstanceBools.Length; i++)
                            {
                                if (NodeInstanceBools[i])
                                {
                                    MessageBox.Show("Instance Not Added To LTG " + TempMainBox.instanceIndex[i]);
                                }
                            }
                        }
                    }

                    for (int y1 = 0; y1 < nodeBoxWidth; y1++)
                    {
                        for (int x1 = 0; x1 < nodeBoxWidth; x1++)
                        {
                            var TempNodeBox = TempMainBox.nodeBBoxes[x1, y1];
                            bool ModifiedSize = false;
                            //Resize For Patches
                            if (TempNodeBox.PatchIndex.Count != 0)
                            {
                                TempNodeBox.patchCount = TempNodeBox.PatchIndex.Count;
                                if (ModifiedSize == false)
                                {
                                    ModifiedSize = true;
                                    TempNodeBox.WorldBounds1 = pbdHandler.Patches[TempNodeBox.PatchIndex[0]].LowestXYZ;
                                    TempNodeBox.WorldBounds2 = pbdHandler.Patches[TempNodeBox.PatchIndex[0]].HighestXYZ;
                                }
                                for (int i = 0; i < TempNodeBox.PatchIndex.Count; i++)
                                {
                                    TempNodeBox.WorldBounds1 = JsonUtil.Lowest(TempNodeBox.WorldBounds1, pbdHandler.Patches[TempNodeBox.PatchIndex[i]].LowestXYZ);
                                    TempNodeBox.WorldBounds2 = JsonUtil.Highest(TempNodeBox.WorldBounds2, pbdHandler.Patches[TempNodeBox.PatchIndex[i]].HighestXYZ);
                                }
                            }
                            //Resize For Instances
                            if (TempNodeBox.InstanceIndex.Count != 0)
                            {
                                TempNodeBox.instAndGemCount = TempNodeBox.InstanceIndex.Count+TempNodeBox.GemIndex.Count;
                                TempNodeBox.instanceCount = TempNodeBox.InstanceIndex.Count + TempNodeBox.RaceInstanceIndex.Count;
                                if (ModifiedSize == false)
                                {
                                    ModifiedSize = true;
                                    TempNodeBox.WorldBounds1 = pbdHandler.Instances[TempNodeBox.InstanceIndex[0]].LowestXYZ;
                                    TempNodeBox.WorldBounds2 = pbdHandler.Instances[TempNodeBox.InstanceIndex[0]].HighestXYZ;
                                }
                                for (int i = 0; i < TempNodeBox.InstanceIndex.Count; i++)
                                {
                                    TempNodeBox.WorldBounds1 = JsonUtil.Lowest(TempNodeBox.WorldBounds1, pbdHandler.Instances[TempNodeBox.InstanceIndex[i]].LowestXYZ);
                                    TempNodeBox.WorldBounds2 = JsonUtil.Highest(TempNodeBox.WorldBounds2, pbdHandler.Instances[TempNodeBox.InstanceIndex[i]].HighestXYZ);
                                }
                            }
                            //Gem Instances
                            if (TempNodeBox.GemIndex.Count != 0)
                            {
                                TempNodeBox.instAndGemCount = TempNodeBox.InstanceIndex.Count + TempNodeBox.GemIndex.Count;
                                TempNodeBox.instanceCount = TempNodeBox.InstanceIndex.Count + TempNodeBox.RaceInstanceIndex.Count;
                                if (ModifiedSize == false)
                                {
                                    ModifiedSize = true;
                                    TempNodeBox.WorldBounds1 = pbdHandler.Instances[TempNodeBox.GemIndex[0]].LowestXYZ;
                                    TempNodeBox.WorldBounds2 = pbdHandler.Instances[TempNodeBox.GemIndex[0]].HighestXYZ;
                                }
                                for (int i = 0; i < TempNodeBox.GemIndex.Count; i++)
                                {
                                    TempNodeBox.WorldBounds1 = JsonUtil.Lowest(TempNodeBox.WorldBounds1, pbdHandler.Instances[TempNodeBox.GemIndex[i]].LowestXYZ);
                                    TempNodeBox.WorldBounds2 = JsonUtil.Highest(TempNodeBox.WorldBounds2, pbdHandler.Instances[TempNodeBox.GemIndex[i]].HighestXYZ);
                                }
                            }
                            //Race Instances
                            if (TempNodeBox.RaceInstanceIndex.Count != 0)
                            {
                                TempNodeBox.instAndGemCount = TempNodeBox.InstanceIndex.Count + TempNodeBox.GemIndex.Count;
                                TempNodeBox.instanceCount = TempNodeBox.InstanceIndex.Count + TempNodeBox.RaceInstanceIndex.Count;
                                if (ModifiedSize == false)
                                {
                                    ModifiedSize = true;
                                    TempNodeBox.WorldBounds1 = pbdHandler.Instances[TempNodeBox.RaceInstanceIndex[0]].LowestXYZ;
                                    TempNodeBox.WorldBounds2 = pbdHandler.Instances[TempNodeBox.RaceInstanceIndex[0]].HighestXYZ;
                                }
                                for (int i = 0; i < TempNodeBox.RaceInstanceIndex.Count; i++)
                                {
                                    TempNodeBox.WorldBounds1 = JsonUtil.Lowest(TempNodeBox.WorldBounds1, pbdHandler.Instances[TempNodeBox.RaceInstanceIndex[i]].LowestXYZ);
                                    TempNodeBox.WorldBounds2 = JsonUtil.Highest(TempNodeBox.WorldBounds2, pbdHandler.Instances[TempNodeBox.RaceInstanceIndex[i]].HighestXYZ);
                                }
                            }

                            TempNodeBox.WorldBounds3 = Vector3.Lerp(TempNodeBox.WorldBounds1, TempNodeBox.WorldBounds2, 0.5f);
                            TempMainBox.nodeBBoxes[x1, y1] = TempNodeBox;
                        }
                    }


                    //Update Total NodeData
                    for (int y1 = 0; y1 < nodeBoxWidth; y1++)
                    {
                        for (int x1 = 0; x1 < nodeBoxWidth; x1++)
                        {
                            var TempNodeBox = TempMainBox.nodeBBoxes[x1, y1];
                            TempMainBox.totalPatchCount += TempNodeBox.patchCount;
                            TempMainBox.totalInstanceCount += TempNodeBox.instAndGemCount;
                            TempMainBox.totalSplineCount += TempNodeBox.splineCount;
                            TempMainBox.totalLightCount += TempNodeBox.lightCount;
                            TempMainBox.totalLightsCrossingCount += TempNodeBox.lightsCrossingCount;
                            TempMainBox.totalParticleInstanceCount += TempNodeBox.particleCount;
                        }
                    }


                    bool defaultset = false;
                    //Reset MainBox Bounds
                    for (int y1 = 0; y1 < nodeBoxWidth; y1++)
                    {
                        for (int x1 = 0; x1 < nodeBoxWidth; x1++)
                        {
                            if (TempMainBox.nodeBBoxes[x1, y1].Modified)
                            {
                                if (defaultset)
                                {
                                    TempMainBox.WorldBounds1 = JsonUtil.Lowest(TempMainBox.WorldBounds1, TempMainBox.nodeBBoxes[x1, y1].WorldBounds1);
                                    TempMainBox.WorldBounds2 = JsonUtil.Highest(TempMainBox.WorldBounds2, TempMainBox.nodeBBoxes[x1, y1].WorldBounds2);
                                }
                                else
                                {
                                    defaultset = true;
                                    TempMainBox.WorldBounds1 = TempMainBox.nodeBBoxes[x1, y1].WorldBounds1;
                                    TempMainBox.WorldBounds2 = TempMainBox.nodeBBoxes[x1, y1].WorldBounds2;
                                }
                            }
                        }
                    }
                    TempMainBox.WorldBounds3 = Vector3.Lerp(TempMainBox.WorldBounds1, TempMainBox.WorldBounds2, 0.5f);

                    if (TempMainBox.Modified == true)
                    {
                        UsedBoxesCount++;
                    }

                    mainBboxes[x, y] = TempMainBox;
                }
            }

            mainBboxCount = UsedBoxesCount;
            mainBboxEmptyCount = totalGridCount - UsedBoxesCount;
        }

        public int FindIfInstaneState(int Index)
        {
            for (int y = 0; y < pointerListCount; y++)
            {
                for (int x = 0; x < pointerCount; x++)
                {
                    //Find Out Whats In Box Patches
                    var TempMainBox = mainBboxes[x, y];


                    if (TempMainBox.Modified)
                    {
                        for (int y1 = 0; y1 < nodeBoxWidth; y1++)
                        {
                            for (int x1 = 0; x1 < nodeBoxWidth; x1++)
                            {
                                var TempNodeBox = TempMainBox.nodeBBoxes[x1, y1];
                                if (TempNodeBox.InstanceIndex.Contains(Index))
                                {
                                    return 0;
                                }
                                if (TempNodeBox.GemIndex.Contains(Index))
                                {
                                    return 2;
                                }
                                if (TempNodeBox.RaceInstanceIndex.Contains(Index))
                                {
                                    return 1;
                                }
                            }
                        }
                    }
                }
            }

            return -1;
        }

        public void SaveLTGFile(string path)
        {
            MemoryStream MainFileStream = new MemoryStream();

            //Header Write
            StreamUtil.WriteInt8(MainFileStream, Unknown);
            StreamUtil.WriteInt8(MainFileStream, ColdFusionVersion);
            StreamUtil.WriteInt8(MainFileStream, ColdFusionRevision);
            StreamUtil.WriteInt8(MainFileStream, endianess);

            StreamUtil.WriteVector3(MainFileStream, WorldBounds1);
            StreamUtil.WriteVector3(MainFileStream, WorldBounds2);
            StreamUtil.WriteVector3(MainFileStream, WorldBounds3);

            StreamUtil.WriteFloat32(MainFileStream, mainBboxSize);
            StreamUtil.WriteInt32(MainFileStream, pointerCount);
            StreamUtil.WriteInt32(MainFileStream, pointerListCount);
            StreamUtil.WriteInt32(MainFileStream, totalGridCount);
            StreamUtil.WriteInt32(MainFileStream, mainBboxCount);
            StreamUtil.WriteInt32(MainFileStream, mainBboxEmptyCount);

            StreamUtil.WriteFloat32(MainFileStream, nodeBoxSize);
            StreamUtil.WriteInt32(MainFileStream, nodeBoxWidth);
            StreamUtil.WriteInt32(MainFileStream, nodeBoxCount);

            StreamUtil.WriteInt32(MainFileStream, (int)MainFileStream.Position+8);
            StreamUtil.WriteInt32(MainFileStream, ((int)MainFileStream.Position + 4) + totalGridCount*4);
            long PositionOffsetList = MainFileStream.Position;
            List<int> OffsetList = new List<int>();

            MainFileStream.Position = (int)MainFileStream.Position + totalGridCount * 4;
            for (int y = 0; y < pointerListCount; y++)
            {
                for (int x = 0; x < pointerCount; x++)
                {
                    if (mainBboxes[x,y].Modified)
                    {
                        OffsetList.Add((int)MainFileStream.Position);
                        MemoryStream MainBoxData = new MemoryStream();
                        var TempMainBox = mainBboxes[x, y];
                        StreamUtil.WriteVector3(MainBoxData, TempMainBox.WorldBounds1);
                        StreamUtil.WriteVector3(MainBoxData, TempMainBox.WorldBounds2);
                        StreamUtil.WriteVector3(MainBoxData, TempMainBox.WorldBounds3);

                        StreamUtil.WriteInt16(MainBoxData, TempMainBox.totalPatchCount);
                        StreamUtil.WriteInt16(MainBoxData, TempMainBox.totalInstanceCount);
                        StreamUtil.WriteInt16(MainBoxData, TempMainBox.totalSplineCount);
                        StreamUtil.WriteInt16(MainBoxData, TempMainBox.totalLightCount);
                        StreamUtil.WriteInt16(MainBoxData, TempMainBox.totalLightsCrossingCount);
                        StreamUtil.WriteInt16(MainBoxData, TempMainBox.totalParticleInstanceCount);

                        StreamUtil.WriteInt32(MainBoxData, TempMainBox.Unknown1);
                        StreamUtil.WriteInt32(MainBoxData, TempMainBox.totalElements);
                        StreamUtil.WriteInt32(MainBoxData, 84); //Header Size/Node Data Offset
                        long MainBoxDataOffsets = MainBoxData.Position;
                        long PatchOffset=0;
                        long InstanceOffset=0;
                        long SplineOffset=0;
                        long LightOffset=0;
                        long LightCrossingOffset=0;
                        long ParticleOffset=0;
                        MainBoxData.Position += 6 * 4;

                        MainBoxData.Position += 76*nodeBoxCount;

                        #region Write All Index Lists
                        if (TempMainBox.totalPatchCount != 0)
                        {
                            PatchOffset = MainBoxData.Position;
                            for (int y1 = 0; y1 < nodeBoxWidth; y1++)
                            {
                                for (int x1 = 0; x1 < nodeBoxWidth; x1++)
                                {
                                    if (TempMainBox.nodeBBoxes[x1, y1].patchCount != 0)
                                    {
                                        TempMainBox.nodeBBoxes[x1, y1].patchesOffset = (int)MainBoxData.Position;
                                        for (int i = 0; i < TempMainBox.nodeBBoxes[x1, y1].PatchIndex.Count; i++)
                                        {
                                            StreamUtil.WriteInt32(MainBoxData, TempMainBox.nodeBBoxes[x1, y1].PatchIndex[i]);
                                        }
                                    }
                                }
                            }
                        }

                        if (TempMainBox.totalInstanceCount != 0)
                        {
                            InstanceOffset = MainBoxData.Position;
                            for (int y1 = 0; y1 < nodeBoxWidth; y1++)
                            {
                                for (int x1 = 0; x1 < nodeBoxWidth; x1++)
                                {
                                    if (TempMainBox.nodeBBoxes[x1, y1].instAndGemCount != 0)
                                    {
                                        TempMainBox.nodeBBoxes[x1, y1].instancesOffset = (int)MainBoxData.Position;
                                        for (int i = 0; i < TempMainBox.nodeBBoxes[x1, y1].InstanceIndex.Count; i++)
                                        {
                                            StreamUtil.WriteInt32(MainBoxData, TempMainBox.nodeBBoxes[x1, y1].InstanceIndex[i]);
                                        }
                                        for (int i = 0; i < TempMainBox.nodeBBoxes[x1, y1].GemIndex.Count; i++)
                                        {
                                            StreamUtil.WriteInt32(MainBoxData, TempMainBox.nodeBBoxes[x1, y1].GemIndex[i]);
                                        }
                                        for (int i = 0; i < TempMainBox.nodeBBoxes[x1, y1].RaceInstanceIndex.Count; i++)
                                        {
                                            StreamUtil.WriteInt32(MainBoxData, TempMainBox.nodeBBoxes[x1, y1].RaceInstanceIndex[i]);
                                        }
                                    }
                                }
                            }
                        }

                        if (TempMainBox.totalSplineCount != 0)
                        {
                            SplineOffset = MainBoxData.Position;
                            for (int y1 = 0; y1 < nodeBoxWidth; y1++)
                            {
                                for (int x1 = 0; x1 < nodeBoxWidth; x1++)
                                {
                                    if (TempMainBox.nodeBBoxes[x1, y1].splineCount != 0)
                                    {
                                        TempMainBox.nodeBBoxes[x1, y1].splinesOffset = (int)MainBoxData.Position;
                                        for (int i = 0; i < TempMainBox.nodeBBoxes[x1, y1].SplineIndex.Count; i++)
                                        {
                                            StreamUtil.WriteInt32(MainBoxData, TempMainBox.nodeBBoxes[x1, y1].SplineIndex[i]);
                                        }
                                    }
                                }
                            }
                        }

                        if (TempMainBox.totalLightCount != 0)
                        {
                            LightOffset = MainBoxData.Position;
                            for (int y1 = 0; y1 < nodeBoxWidth; y1++)
                            {
                                for (int x1 = 0; x1 < nodeBoxWidth; x1++)
                                {
                                    if (TempMainBox.nodeBBoxes[x1, y1].lightCount != 0)
                                    {
                                        TempMainBox.nodeBBoxes[x1, y1].lightsOffset = (int)MainBoxData.Position;
                                        for (int i = 0; i < TempMainBox.nodeBBoxes[x1, y1].LightIndex.Count; i++)
                                        {
                                            StreamUtil.WriteInt32(MainBoxData, TempMainBox.nodeBBoxes[x1, y1].LightIndex[i]);
                                        }
                                    }
                                }
                            }
                        }

                        if (TempMainBox.totalLightsCrossingCount != 0)
                        {
                            LightCrossingOffset = MainBoxData.Position;
                            for (int y1 = 0; y1 < nodeBoxWidth; y1++)
                            {
                                for (int x1 = 0; x1 < nodeBoxWidth; x1++)
                                {
                                    if (TempMainBox.nodeBBoxes[x1, y1].lightsCrossingCount != 0)
                                    {
                                        TempMainBox.nodeBBoxes[x1, y1].lightsCrossingOffset = (int)MainBoxData.Position;
                                        for (int i = 0; i < TempMainBox.nodeBBoxes[x1, y1].LightCrossingIndex.Count; i++)
                                        {
                                            StreamUtil.WriteInt32(MainBoxData, TempMainBox.nodeBBoxes[x1, y1].LightCrossingIndex[i]);
                                        }
                                    }
                                }
                            }
                        }

                        if (TempMainBox.totalParticleInstanceCount != 0)
                        {
                            ParticleOffset = MainBoxData.Position;
                            for (int y1 = 0; y1 < nodeBoxWidth; y1++)
                            {
                                for (int x1 = 0; x1 < nodeBoxWidth; x1++)
                                {
                                    if (TempMainBox.nodeBBoxes[x1, y1].particleCount != 0)
                                    {
                                        TempMainBox.nodeBBoxes[x1, y1].particleModelsOffset = (int)MainBoxData.Position;
                                        for (int i = 0; i < TempMainBox.nodeBBoxes[x1, y1].ParticleIndex.Count; i++)
                                        {
                                            StreamUtil.WriteInt32(MainBoxData, TempMainBox.nodeBBoxes[x1, y1].ParticleIndex[i]);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        MainBoxData.Position = MainBoxDataOffsets;
                        StreamUtil.WriteInt32(MainBoxData, (int)PatchOffset);
                        StreamUtil.WriteInt32(MainBoxData, (int)InstanceOffset);
                        StreamUtil.WriteInt32(MainBoxData, (int)SplineOffset);
                        StreamUtil.WriteInt32(MainBoxData, (int)LightOffset);
                        StreamUtil.WriteInt32(MainBoxData, (int)LightCrossingOffset);
                        StreamUtil.WriteInt32(MainBoxData, (int)ParticleOffset);

                        for (int y1 = 0; y1 < nodeBoxWidth; y1++)
                        {
                            for (int x1 = 0; x1 < nodeBoxWidth; x1++)
                            {
                                var TempNode = TempMainBox.nodeBBoxes[x1, y1];

                                StreamUtil.WriteVector3(MainBoxData, TempNode.WorldBounds1);
                                StreamUtil.WriteVector3(MainBoxData, TempNode.WorldBounds2);
                                StreamUtil.WriteVector3(MainBoxData, TempNode.WorldBounds3);

                                StreamUtil.WriteInt16(MainBoxData, TempNode.patchCount);
                                StreamUtil.WriteInt16(MainBoxData, TempNode.instanceCount);
                                StreamUtil.WriteInt16(MainBoxData, TempNode.instAndGemCount);
                                StreamUtil.WriteInt16(MainBoxData, TempNode.splineCount);
                                StreamUtil.WriteInt16(MainBoxData, TempNode.lightCount);
                                StreamUtil.WriteInt16(MainBoxData, TempNode.lightsCrossingCount);
                                StreamUtil.WriteInt16(MainBoxData, TempNode.particleCount);
                                StreamUtil.WriteInt16(MainBoxData, TempNode.Unknown1);

                                StreamUtil.WriteInt32(MainBoxData, TempNode.patchesOffset);
                                StreamUtil.WriteInt32(MainBoxData, TempNode.instancesOffset);
                                StreamUtil.WriteInt32(MainBoxData, TempNode.splinesOffset);
                                StreamUtil.WriteInt32(MainBoxData, TempNode.lightsOffset);
                                StreamUtil.WriteInt32(MainBoxData, TempNode.lightsCrossingOffset);
                                StreamUtil.WriteInt32(MainBoxData, TempNode.particleModelsOffset);

                            }
                        }
                        StreamUtil.WriteStreamIntoStream(MainFileStream, MainBoxData);
                        MainBoxData.Position = 0;
                        MainBoxData.Dispose();
                    }
                    else
                    {
                        OffsetList.Add(0);
                    }
                }
            }

            //Update PointerList
            MainFileStream.Position = PositionOffsetList;
            for (int i = 0; i < OffsetList.Count; i++)
            {
                StreamUtil.WriteInt32(MainFileStream, OffsetList[i]);
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            var file = File.Create(path);
            MainFileStream.Position = 0;
            MainFileStream.CopyTo(file);
            MainFileStream.Dispose();
            file.Close();
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

            public List<int> patchIndex;
            public List<int> instanceIndex;
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
            public int Unknown1;

            public int patchesOffset;        // offset leads to it's own index list
            public int instancesOffset;      // or models
            public int splinesOffset;
            public int lightsOffset;
            public int lightsCrossingOffset; // offset of it's own extraThing list, usually hex 00000000 01000000
            public int particleModelsOffset;

            public List<int> PatchIndex;
            public List<int> InstanceIndex;
            public List<int> RaceInstanceIndex;
            public List<int> GemIndex;
            public List<int> SplineIndex;
            public List<int> LightIndex;
            public List<int> LightCrossingIndex;
            public List<int> ParticleIndex;
        }

    }
}
