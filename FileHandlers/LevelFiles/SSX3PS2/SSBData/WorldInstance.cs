using SSXMultiTool.JsonFiles.SSX3;
using SSXMultiTool.Utilities;
using System.IO;
using System.Numerics;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2.SSBData
{
    public class WorldInstance
    {
        public string Name;

        public ObjectID objectID;

        public int U0;
        public int U1;
        public int U2;
        public int U3;

        public Matrix4x4 matrix4X4 = new Matrix4x4();
        public Vector4 V0;
        public Vector3 V1;
        public Vector3 V2;

        public int U4;

        public ObjectID UObjectID;

        public float U5;
        public int U6;
        public int U7;

        public int U8;
        public int U9;
        public int U10;
        public int U11;
        public int U12;

        public void LoadData(Stream stream)
        {
            U0 = StreamUtil.ReadInt32(stream);
            U1 = StreamUtil.ReadInt32(stream);
            U2 = StreamUtil.ReadInt32(stream);
            U3 = StreamUtil.ReadInt32(stream);

            matrix4X4 = StreamUtil.ReadMatrix4x4(stream);
            V0 = StreamUtil.ReadVector4(stream);
            V1 = StreamUtil.ReadVector3(stream);
            V2 = StreamUtil.ReadVector3(stream);

            objectID = WorldCommon.ObjectIDLoad(stream); 
            U4 = StreamUtil.ReadInt32(stream);

            UObjectID = WorldCommon.ObjectIDLoad(stream);

            U5 = StreamUtil.ReadFloat(stream);
            U6 = StreamUtil.ReadInt32(stream);
            U7 = StreamUtil.ReadInt32(stream);

            U8 = StreamUtil.ReadInt16(stream);
            U9 = StreamUtil.ReadInt16(stream);
            U10 = StreamUtil.ReadInt32(stream);
            U11 = StreamUtil.ReadInt32(stream);
            U12 = StreamUtil.ReadInt32(stream);

            //READ MODEL DATA
        }

        public InstanceJsonHandler.Instance ToJSON()
        {
            InstanceJsonHandler.Instance bin3File = new InstanceJsonHandler.Instance();

            bin3File.Name = Name;

            bin3File.U0 = U0;
            bin3File.U1 = U1;
            bin3File.U2 = U2;
            bin3File.U3 = U3;

            Vector3 Scale;
            Quaternion Rotation;
            Vector3 Location;

            Matrix4x4.Decompose(matrix4X4, out Scale, out Rotation, out Location);
            bin3File.Position = JsonUtil.Vector3ToArray(Location);
            bin3File.Rotation = JsonUtil.QuaternionToArray(Rotation);
            bin3File.Scale = JsonUtil.Vector3ToArray(Scale);

            bin3File.V0 = JsonUtil.Vector4ToArray(V0);
            bin3File.V1 = JsonUtil.Vector3ToArray(V1);
            bin3File.V2 = JsonUtil.Vector3ToArray(V2);

            bin3File.TrackID = objectID.TrackID;
            bin3File.RID = objectID.RID;
            bin3File.U4 = U4;

            bin3File.UTrackID = UObjectID.TrackID;
            bin3File.URID = UObjectID.RID;
            bin3File.U5 = U5;
            bin3File.U6 = U6;
            bin3File.U7 = U7;

            bin3File.U8 = U8;
            bin3File.U9 = U9;
            bin3File.U10 = U10;
            bin3File.U11 = U11;
            bin3File.U12 = U12;

            return bin3File;
        }
    }
}
