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

        public int[,] offsetList;

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

                for (int y = 0; y < pointerListCount; y++)
                {
                    for (int x = 0; x < pointerCount; x++)
                    {
                        offsetList[y, x] = StreamUtil.ReadInt32(stream);
                    }
                }
            }
        }




        struct mainBbox
        {
            Vector3 WorldBounds1;
            Vector3 WorldBounds2;
            Vector3 WorldBounds3;

            int totalPatchCount;            // Total Patch count
            int totalInstanceCount;         // Total Instance count
            int unknown;
            int totalLightCount;            // Total Light count
            int totallightsCrossingCount;   // Total Lights crossing count. Whatever that means
            int totalParticleInstanceCount;
            int Unknown1; // number of elements?
            int Unknown2; // offset to first nodeBbox? or mainBbox byte size
            int Unknown3; // index list offset
            int Unknown4;
            int Unknown5;
            int Unknown6;
            int Unknown7; // offset leads to list of extraThing lists
            int Unknown8;
        }

        struct nodeBBox
        {
            Vector3 WorldBounds1;
            Vector3 WorldBounds2;
            Vector3 WorldBounds3;

            int patchCount;          // Patch count
            int instanceCount;       // Instance count
            int instAndGemCount;     // Models/Instances & Gems apparently
            int splineCount;         // Spline count
            int lightCount;          // Light count
            int lightsCrossingCount; // Lights crossing count
            int particleCount;       // Particle model count

            int patchesOffset;        // offset leads to it's own index list
            int instancesOffset;      // or models
            int splinesOffset;
            int lightsOffset;
            int lightsCrossingOffset; // offset of it's own extraThing list, usually hex 00000000 01000000
            int particleModelsOffset;
        }

    }
}
