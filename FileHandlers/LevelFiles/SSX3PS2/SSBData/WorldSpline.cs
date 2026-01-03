using SSXMultiTool.JsonFiles.SSX3;
using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2.SSBData
{
    public class WorldSpline
    {
        public string Name;

        public int U0;
        public Vector3 BBoxMin;
        public Vector3 BBoxMax;
        public int U1; // Padding?
        public int NumSplineSegments;
        public float U2;
        public int U3;
        public int U4; // Padding?

        public List<SplineSegment> splineSegments = new List<SplineSegment>();

        public void LoadData(Stream stream)
        {
            U0 = StreamUtil.ReadInt32(stream);
            BBoxMin = StreamUtil.ReadVector3(stream);
            BBoxMax = StreamUtil.ReadVector3(stream);
            U1 = StreamUtil.ReadInt32(stream);
            NumSplineSegments = StreamUtil.ReadInt32(stream);
            U2 = StreamUtil.ReadFloat(stream);
            U3 = StreamUtil.ReadInt32(stream);
            U4 = StreamUtil.ReadInt32(stream);

            splineSegments = new List<SplineSegment>();

            for (int i = 0; i < NumSplineSegments; i++)
            {
                var TempSegment = new SplineSegment();
                TempSegment.U0 = StreamUtil.ReadBytes(stream, 12);
                TempSegment.U1 = StreamUtil.ReadFloat(stream);
                TempSegment.Point4 = StreamUtil.ReadVector4(stream);
                TempSegment.Point3 = StreamUtil.ReadVector4(stream);
                TempSegment.Point2 = StreamUtil.ReadVector4(stream);
                TempSegment.Point1 = StreamUtil.ReadVector4(stream);
                TempSegment.E0 = StreamUtil.ReadFloat(stream);
                TempSegment.E1 = StreamUtil.ReadFloat(stream);
                TempSegment.E2 = StreamUtil.ReadFloat(stream);
                TempSegment.E3 = StreamUtil.ReadFloat(stream);
                TempSegment.U2 = StreamUtil.ReadInt32(stream);
                TempSegment.U3 = StreamUtil.ReadInt32(stream);
                TempSegment.U4 = StreamUtil.ReadInt32(stream);
                TempSegment.BBoxMin = StreamUtil.ReadVector3(stream);
                TempSegment.BBoxMax = StreamUtil.ReadVector3(stream);
                TempSegment.U5 = StreamUtil.ReadFloat(stream);
                TempSegment.U6 = StreamUtil.ReadInt32(stream);
                TempSegment.U7 = StreamUtil.ReadInt32(stream);

                splineSegments.Add(TempSegment);
            }
        }


        public SplineJsonHandler.SplineJson ToJSON()
        {
            SplineJsonHandler.SplineJson splineJson = new SplineJsonHandler.SplineJson();

            splineJson.Name = Name;

            splineJson.U0 = U0;
            splineJson.U1 = U1;
            splineJson.U2 = U2;
            splineJson.U3 = U3;
            splineJson.U4 = U4;

            splineJson.Segments = new List<SplineJsonHandler.SegmentJson>();
            for (int i = 0; i < NumSplineSegments; i++)
            {
                var TempJsonSegment = new SplineJsonHandler.SegmentJson();

                BezierUtil bezierUtil = new BezierUtil();
                bezierUtil.ProcessedPoints[0] = JsonUtil.Vector4ToVector3(splineSegments[i].Point1);
                bezierUtil.ProcessedPoints[1] = JsonUtil.Vector4ToVector3(splineSegments[i].Point2);
                bezierUtil.ProcessedPoints[2] = JsonUtil.Vector4ToVector3(splineSegments[i].Point3);
                bezierUtil.ProcessedPoints[3] = JsonUtil.Vector4ToVector3(splineSegments[i].Point4);
                bezierUtil.GenerateRawPoints();

                TempJsonSegment.Points = new float[4, 3];

                for (int j = 0; j < 4; j++)
                {
                    TempJsonSegment.Points = JsonUtil.Vector3ToArray2D(TempJsonSegment.Points, bezierUtil.RawPoints[j], j);
                }

                TempJsonSegment.E0 = splineSegments[i].E0;
                TempJsonSegment.E1 = splineSegments[i].E1;
                TempJsonSegment.E2 = splineSegments[i].E2;
                TempJsonSegment.E3 = splineSegments[i].E3;

                splineJson.Segments.Add(TempJsonSegment);
            }

            return splineJson;
        }


        public struct SplineSegment
        {
            public byte[] U0;
            public float U1;
            public Vector4 Point4;
            public Vector4 Point3;
            public Vector4 Point2;
            public Vector4 Point1;
            public float E0;
            public float E1;
            public float E2;
            public float E3;
            public int U2;
            public int U3;
            public int U4;
            public Vector3 BBoxMin;
            public Vector3 BBoxMax;
            public float U5; // SplineDistance/SplineLength?
            public int U6;
            public int U7; // Always 15?
        }

    }
}
