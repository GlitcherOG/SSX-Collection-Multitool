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
    public class WorldPatch
    {
        public int U0;
        public int U1;
        public int U2;
        public int U3;
        public int U4;
        public int U5;

        public Vector4 Lightmap;
        public Vector2 UV1;
        public Vector2 UV2;
        public Vector2 UV3;
        public Vector2 UV4;

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

        public Vector4 U7;
        public ObjectID objectID;
        public int U10;
        public int U11;

        public Vector3 Point1;
        public Vector3 Point2;
        public Vector3 Point3;
        public Vector3 Point4;

        public Vector3 BBoxMin;
        public Vector3 BBoxMax;

        public int TextureRID;
        public int LightmapRID;
        public int U14;
        public int U15;
        public int U16;
        public int U17;
        public int U18;
        public int U19;

        public void LoadPatch(Stream stream)
        {
            U0 = StreamUtil.ReadInt32(stream);
            U1 = StreamUtil.ReadInt32(stream);
            U2 = StreamUtil.ReadInt16(stream);
            U3 = StreamUtil.ReadInt16(stream);
            U4 = StreamUtil.ReadInt16(stream);
            U5 = StreamUtil.ReadInt16(stream);

            Lightmap = StreamUtil.ReadVector4(stream);
            UV1 = StreamUtil.ReadVector2(stream);
            UV2 = StreamUtil.ReadVector2(stream);
            UV3 = StreamUtil.ReadVector2(stream);
            UV4 = StreamUtil.ReadVector2(stream);

            R4C4 = StreamUtil.ReadVector4(stream);
            R4C3 = StreamUtil.ReadVector4(stream);
            R4C2 = StreamUtil.ReadVector4(stream);
            R4C1 = StreamUtil.ReadVector4(stream);
            R3C4 = StreamUtil.ReadVector4(stream);
            R3C3 = StreamUtil.ReadVector4(stream);
            R3C2 = StreamUtil.ReadVector4(stream);
            R3C1 = StreamUtil.ReadVector4(stream);
            R2C4 = StreamUtil.ReadVector4(stream);
            R2C3 = StreamUtil.ReadVector4(stream);
            R2C2 = StreamUtil.ReadVector4(stream);
            R2C1 = StreamUtil.ReadVector4(stream);
            R1C4 = StreamUtil.ReadVector4(stream);
            R1C3 = StreamUtil.ReadVector4(stream);
            R1C2 = StreamUtil.ReadVector4(stream);
            R1C1 = StreamUtil.ReadVector4(stream);

            U7 = StreamUtil.ReadVector4(stream);
            objectID = WorldCommon.ObjectIDLoad(stream);
            U10 = StreamUtil.ReadInt16(stream);
            U11 = StreamUtil.ReadInt16(stream);

            Point1 = StreamUtil.ReadVector3(stream);
            Point2 = StreamUtil.ReadVector3(stream);
            Point3 = StreamUtil.ReadVector3(stream);
            Point4 = StreamUtil.ReadVector3(stream);

            BBoxMin = StreamUtil.ReadVector3(stream);
            BBoxMax = StreamUtil.ReadVector3(stream);

            TextureRID = StreamUtil.ReadInt16(stream);
            LightmapRID = StreamUtil.ReadInt16(stream);
            U14 = StreamUtil.ReadInt16(stream);
            U15 = StreamUtil.ReadInt16(stream);
            U16 = StreamUtil.ReadInt16(stream);
            U17 = StreamUtil.ReadInt16(stream);
            U18 = StreamUtil.ReadInt16(stream);
            U19 = StreamUtil.ReadInt16(stream);
        }

        public PatchesJsonHandler.PatchJson ToJSON()
        {
            PatchesJsonHandler.PatchJson patchJson = new PatchesJsonHandler.PatchJson();

            patchJson.U1 = U1;
            patchJson.U2 = U2;
            patchJson.U3 = U3;
            patchJson.U4 = U4;
            patchJson.U5 = U5;

            //patchJson.TexturePath = "0000.png";

            patchJson.LightMapPoint = JsonUtil.Vector4ToArray(Lightmap);
            patchJson.UVPoints = new float[4, 2];

            patchJson.UVPoints[0, 0] = UV1.X;
            patchJson.UVPoints[0, 1] = UV1.Y;

            patchJson.UVPoints[1, 0] = UV2.X;
            patchJson.UVPoints[1, 1] = UV2.Y;

            patchJson.UVPoints[2, 0] = UV3.X;
            patchJson.UVPoints[2, 1] = UV3.Y;

            patchJson.UVPoints[3, 0] = UV4.X;
            patchJson.UVPoints[3, 1] = UV4.Y;

            BezierUtil bezierUtil = new BezierUtil();
            bezierUtil.ProcessedPoints[0] = JsonUtil.Vector4ToVector3(R1C1);
            bezierUtil.ProcessedPoints[1] = JsonUtil.Vector4ToVector3(R1C2);
            bezierUtil.ProcessedPoints[2] = JsonUtil.Vector4ToVector3(R1C3);
            bezierUtil.ProcessedPoints[3] = JsonUtil.Vector4ToVector3(R1C4);
            bezierUtil.ProcessedPoints[4] = JsonUtil.Vector4ToVector3(R2C1);
            bezierUtil.ProcessedPoints[5] = JsonUtil.Vector4ToVector3(R2C2);
            bezierUtil.ProcessedPoints[6] = JsonUtil.Vector4ToVector3(R2C3);
            bezierUtil.ProcessedPoints[7] = JsonUtil.Vector4ToVector3(R2C4);
            bezierUtil.ProcessedPoints[8] = JsonUtil.Vector4ToVector3(R3C1);
            bezierUtil.ProcessedPoints[9] = JsonUtil.Vector4ToVector3(R3C2);
            bezierUtil.ProcessedPoints[10] = JsonUtil.Vector4ToVector3(R3C3);
            bezierUtil.ProcessedPoints[11] = JsonUtil.Vector4ToVector3(R3C4);
            bezierUtil.ProcessedPoints[12] = JsonUtil.Vector4ToVector3(R4C1);
            bezierUtil.ProcessedPoints[13] = JsonUtil.Vector4ToVector3(R4C2);
            bezierUtil.ProcessedPoints[14] = JsonUtil.Vector4ToVector3(R4C3);
            bezierUtil.ProcessedPoints[15] = JsonUtil.Vector4ToVector3(R4C4);

            bezierUtil.GenerateRawPoints();

            patchJson.Points = new float[16, 3];

            for (int a = 0; a < 16; a++)
            {
                patchJson.Points = JsonUtil.Vector3ToArray2D(patchJson.Points, bezierUtil.RawPoints[a], a);
            }

            patchJson.U7 = JsonUtil.Vector4ToArray(U7);

            patchJson.TrackID = objectID.TrackID;
            patchJson.RID = objectID.RID;

            patchJson.U10 = U10;
            patchJson.U11 = U11;
            patchJson.TextureRID = TextureRID;
            patchJson.LightmapRID = LightmapRID;
            patchJson.U14 = U14;
            patchJson.U15 = U15;
            patchJson.U16 = U16;
            patchJson.U17 = U17;
            patchJson.U18 = U18;
            patchJson.U19 = U19;

            return patchJson;
        }
    }
}
