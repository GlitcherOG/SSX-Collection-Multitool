using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2
{
    internal class LTGHandler
    {
        byte Unknown;
        byte ColdFusionVersion;
        byte ColdFusionRevision;
        byte endianess;

        Vector3 WorldBounds1;
        Vector3 WorldBounds2;
        Vector3 WorldBounds3;

        float mainBboxSize;
        int pointerCount;
        int pointerListCount;
        int totalGridCount;
        int mainBboxCount;
        int mainBboxEmptyCount;

        float nodeBoxSize;
        int nodeBoxWidth;
        int nodeBoxCount;

        int pointerListOffset;
        int bboxDataListOffset;

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
                            tempbBox.Unknown1 = StreamUtil.ReadInt32(stream);
                            tempbBox.Unknown2 = StreamUtil.ReadInt32(stream);
                            tempbBox.Unknown3 = StreamUtil.ReadInt32(stream);
                            tempbBox.Unknown4 = StreamUtil.ReadInt32(stream);
                            tempbBox.Unknown5 = StreamUtil.ReadInt32(stream);
                            tempbBox.Unknown6 = StreamUtil.ReadInt32(stream);
                            tempbBox.Unknown7 = StreamUtil.ReadInt32(stream);
                            tempbBox.Unknown8 = StreamUtil.ReadInt32(stream);

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

                                    tempbBox.nodeBBoxes[x1, y1] = tempNode;
                                }
                            }

                            mainBboxes[x, y] = tempbBox;
                        }
                    }
                }
            }
        }




        public struct mainBbox
        {
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

            public nodeBBox[,] nodeBBoxes;
        }

        public struct nodeBBox
        {
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
        }

    }
}
