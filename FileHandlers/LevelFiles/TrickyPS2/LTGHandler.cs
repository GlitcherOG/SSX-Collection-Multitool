using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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

    }
}
