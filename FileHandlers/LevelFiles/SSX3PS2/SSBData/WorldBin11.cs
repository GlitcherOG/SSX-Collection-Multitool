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
    public class WorldBin11
    {
        public float U0;
        public float U1;
        public float U2;
        public float U3;

        public Vector4 Point4;
        public Vector4 Point3;
        public Vector4 Point2;
        public Vector4 ControlPoint;

        public float U4;
        public float U5;
        public float U6;
        public float U7;


        public Vector3 BBoxMin;
        public Vector3 BBoxMax;

        public void LoadData(Stream stream)
        {
            U0 = StreamUtil.ReadFloat(stream);
            U1 = StreamUtil.ReadFloat(stream);
            U2 = StreamUtil.ReadFloat(stream);
            U3 = StreamUtil.ReadFloat(stream);

            Point4 = StreamUtil.ReadVector4(stream);
            Point3 = StreamUtil.ReadVector4(stream);
            Point2 = StreamUtil.ReadVector4(stream);
            ControlPoint = StreamUtil.ReadVector4(stream);

            U4 = StreamUtil.ReadFloat(stream);
            U5 = StreamUtil.ReadFloat(stream);
            U6 = StreamUtil.ReadFloat(stream);
            U7 = StreamUtil.ReadFloat(stream);

            stream.Position += 0x40;

            BBoxMin = StreamUtil.ReadVector3(stream);
            BBoxMax = StreamUtil.ReadVector3(stream);
        }

        public Bin11JsonHandler.Bin11File ToJSON()
        {
            Bin11JsonHandler.Bin11File bin11File = new Bin11JsonHandler.Bin11File();

            bin11File.U0 = U0;
            bin11File.U1 = U1;
            bin11File.U2 = U2;
            bin11File.U3 = U3;

            bin11File.Point4 = JsonUtil.Vector4ToArray(Point4);
            bin11File.Point3 = JsonUtil.Vector4ToArray(Point3);
            bin11File.Point2 = JsonUtil.Vector4ToArray(Point2);
            bin11File.ControlPoint = JsonUtil.Vector4ToArray(ControlPoint);

            bin11File.U4 = U4;
            bin11File.U5 = U5;
            bin11File.U6 = U6;
            bin11File.U7 = U7;

            return bin11File;
        }
    }
}
