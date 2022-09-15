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
        public static float[] Vector4ToArray(Vector4 vector4)
        {
            float[] array = new float[4];
            array[0] = vector4.X;
            array[1] = vector4.Y;
            array[2] = vector4.Z;
            array[3] = vector4.W;
            return array;
        }
    }
}
