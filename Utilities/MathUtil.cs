using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.Utilities
{
    public static class MathUtil
    {
        public static Quaternion ToQuaternion(Vector3 v)
        {
            float cy = (float)Math.Cos(v.Z * 0.5);
            float sy = (float)Math.Sin(v.Z * 0.5);
            float cp = (float)Math.Cos(v.Y * 0.5);
            float sp = (float)Math.Sin(v.Y * 0.5);
            float cr = (float)Math.Cos(v.X * 0.5);
            float sr = (float)Math.Sin(v.X * 0.5);

            return new Quaternion
            {
                W = (cr * cp * cy + sr * sp * sy),
                X = (sr * cp * cy - cr * sp * sy),
                Y = (cr * sp * cy + sr * cp * sy),
                Z = (cr * cp * sy - sr * sp * cy)
            };

        }

        public static Vector3 ToEulerAngles(Quaternion q)
        {
            Vector3 angles = new();

            // roll / x
            double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            angles.X = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch / y
            double sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1)
            {
                angles.Y = (float)Math.CopySign(Math.PI / 2, sinp);
            }
            else
            {
                angles.Y = (float)Math.Asin(sinp);
            }

            // yaw / z
            double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            angles.Z = (float)Math.Atan2(siny_cosp, cosy_cosp);

            return angles;
        }


        public static Vector3 Highest(Vector3 current, Vector3 vector3)
        {
            Vector3 vertex = vector3;
            if (vertex.X > current.X)
            {
                current.X = vertex.X;
            }
            if (vertex.Y > current.Y)
            {
                current.Y = vertex.Y;
            }
            if (vertex.Z > current.Z)
            {
                current.Z = vertex.Z;
            }
            return current;
        }

        public static Vector3 Lowest(Vector3 current, Vector3 vector3)
        {
            Vector3 vertex = vector3;
            if (vertex.X < current.X)
            {
                current.X = vertex.X;
            }
            if (vertex.Y < current.Y)
            {
                current.Y = vertex.Y;
            }
            if (vertex.Z < current.Z)
            {
                current.Z = vertex.Z;
            }
            return current;
        }

    }
}
