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

        public static float[] Vector2ToArray(Vector2 vector3)
        {
            float[] array = new float[2];
            array[0] = vector3.X;
            array[1] = vector3.Y;
            return array;
        }

        public static Vector2 ArrayToVector2(float[] floats)
        {
            return new Vector2(floats[0], floats[1]);
        }

        public static float[] QuaternionToArray(Quaternion quaternion)
        {
            float[] array = new float[4];
            array[0] = quaternion.X;
            array[1] = quaternion.Y;
            array[2] = quaternion.Z;
            array[3] = quaternion.W;
            return array;
        }

        public static Quaternion ArrayToQuaternion(float[] array)
        {
            return new Quaternion(array[0], array[1], array[2], array[3]);
        }

        public static float GenerateDistance(Vector3 Point1 , Vector3 Point2, Vector3 Point3, Vector3 Point4)
        {
            float Distance = 0;
            Distance += Vector3.Distance(Point1, Point2);
            Distance += Vector3.Distance(Point2, Point3);
            Distance += Vector3.Distance(Point3, Point4);
            return Distance;
        }

        public static bool WithinXY(Vector3 Object, Vector3 LowestXYZ, Vector3 HighestXYZ)
        {
            if (Object.X >= LowestXYZ.X&& Object.X <= HighestXYZ.X && Object.Y >= LowestXYZ.Y && Object.Y <= HighestXYZ.Y)
            {
                return true;
            }
            return false;
        }

        //Square 1 is MainPatch. Square 2 is Light
        public static bool IntersectingSquares(Vector3 Square1Lowest, Vector3 Square1Highest, Vector3 Square2Lowest, Vector3 Square2Highest)
        {
            Vector3 Square1Point1 = Square1Lowest;
            Vector3 Square1Point2 = new Vector3(Square1Highest.X, Square1Lowest.Y, 0);
            Vector3 Square1Point3 = Square1Highest;
            Vector3 Square1Point4 = new Vector3(Square1Lowest.X, Square1Highest.Y, 0);

            Vector3 Square2Point1 = Square2Lowest;
            Vector3 Square2Point2 = new Vector3(Square2Highest.X, Square2Lowest.Y, 0);
            Vector3 Square2Point3 = Square2Highest;
            Vector3 Square2Point4 = new Vector3(Square2Lowest.X, Square2Highest.Y, 0);

            //Check if Node is within Light
            if (Square1Point1.X >= Square2Lowest.X && Square1Point1.X <= Square2Highest.X && Square1Point1.Y >= Square2Lowest.Y && Square1Point1.Y <= Square2Highest.Y)
            {
                return true;
            }

            if (Square1Point3.X >= Square2Lowest.X && Square1Point3.X <= Square2Highest.X && Square1Point3.Y >= Square2Lowest.Y && Square1Point3.Y <= Square2Highest.Y)
            {
                return true;
            }

            if (Square1Point2.X >= Square2Lowest.X && Square1Point2.X <= Square2Highest.X && Square1Point2.Y >= Square2Lowest.Y && Square1Point2.Y <= Square2Highest.Y)
            {
                return true;
            }

            if (Square1Point4.X >= Square2Lowest.X && Square1Point4.X <= Square2Highest.X && Square1Point4.Y >= Square2Lowest.Y && Square1Point4.Y <= Square2Highest.Y)
            {
                return true;
            }

            //Check if Light is Within Node
            if (Square2Point1.X >= Square1Lowest.X && Square2Point1.X <= Square1Highest.X && Square2Point1.Y >= Square1Lowest.Y && Square2Point1.Y <= Square1Highest.Y)
            {
                return true;
            }

            if (Square2Point2.X >= Square1Lowest.X && Square2Point2.X <= Square1Highest.X && Square2Point2.Y >= Square1Lowest.Y && Square2Point2.Y <= Square1Highest.Y)
            {
                return true;
            }

            if (Square2Point3.X >= Square1Lowest.X && Square2Point3.X <= Square1Highest.X && Square2Point3.Y >= Square1Lowest.Y && Square2Point3.Y <= Square1Highest.Y)
            {
                return true;
            }

            if (Square2Point4.X >= Square1Lowest.X && Square2Point4.X <= Square1Highest.X && Square2Point4.Y >= Square1Lowest.Y && Square2Point4.Y <= Square1Highest.Y)
            {
                return true;
            }

            return false;
        }
    }
}
