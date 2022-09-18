using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SSXMultiTool.Utilities
{
    public class JsonUtil
    {
        public static Vector3 Vector4ToVector3(Vector4 vector4)
        {
            return new Vector3(vector4.X, vector4.Y, vector4.Z);
        }

        public static Vector4 Vector3ToVector4(Vector3 vector3, float W = 1)
        {
            return new Vector4(vector3.X, vector3.Y, vector3.Z, W);
        }

        public static float[] Vector4ToArray(Vector4 vector4)
        {
            float[] array = new float[4];
            array[0] = vector4.X;
            array[1] = vector4.Y;
            array[2] = vector4.Z;
            array[3] = vector4.W;
            return array;
        }

        public static Vector4 ArrayToVector4(float[] floats)
        {
            return new Vector4(floats[0], floats[1], floats[2], floats[3]);
        }

        public static float[] Vector3ToArray(Vector3 vector3)
        {
            float[] array = new float[3];
            array[0] = vector3.X;
            array[1] = vector3.Y;
            array[2] = vector3.Z;
            return array;
        }

        public static Vector3 ArrayToVector3(float[] floats)
        {
            return new Vector3(floats[0], floats[1], floats[2]);
        }

    }
}
