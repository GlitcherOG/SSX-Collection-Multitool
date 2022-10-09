using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers
{
    public class GSMTModelFormatHandler
    {
        public string ModelName = "";
        public List<Mesh> meshes = new List<Mesh>();

        public void SaveFile(string path)
        {

        }
    }

    public struct Mesh
    {
        public List<Face> faces;
    }


    public struct Face
    {
        public Vector3 V1;
        public Vector3 V2;
        public Vector3 V3;

        public int V1Pos;
        public int V2Pos;
        public int V3Pos;

        public Vector2 UV1;
        public Vector2 UV2;
        public Vector2 UV3;

        public Vector3 Normal1;
        public Vector3 Normal2;
        public Vector3 Normal3;
    }
}
