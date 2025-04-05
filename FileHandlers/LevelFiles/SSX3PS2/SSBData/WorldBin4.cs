using SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2;
using SSXMultiTool.JsonFiles.SSX3;
using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2.SSBData
{
    public class WorldBin4
    {
        public int U0;
        public int U1;
        public int U2;
        public int U3;

        public Vector4 U4;
        public Vector4 U5;
        public Vector4 U6;
        public Vector4 U7;

        public Vector4 U8;

        public int U9;
        public int U10;
        public Vector3 U11;
        public Vector3 U12;

        public int U13;
        public int U14;
        public int U15;
        public int U16;

        public void LoadData(Stream stream)
        {
            U0 = StreamUtil.ReadInt32(stream);
            U1 = StreamUtil.ReadInt32(stream);
            U2 = StreamUtil.ReadInt32(stream);
            U3 = StreamUtil.ReadInt32(stream);

            U4 = StreamUtil.ReadVector4(stream);
            U5 = StreamUtil.ReadVector4(stream);
            U6 = StreamUtil.ReadVector4(stream);
            U7 = StreamUtil.ReadVector4(stream);

            U8 = StreamUtil.ReadVector4(stream);

            U9 = StreamUtil.ReadInt32(stream);
            U10 = StreamUtil.ReadInt32(stream);
            
            U11 = StreamUtil.ReadVector3(stream);
            U12 = StreamUtil.ReadVector3(stream);

            U13 = StreamUtil.ReadInt32(stream);
            U14 = StreamUtil.ReadInt32(stream);
            U15 = StreamUtil.ReadInt32(stream);
            U16 = StreamUtil.ReadInt32(stream);
        }

        public Bin4JsonHandler.Bin4File ToJSON()
        {
            Bin4JsonHandler.Bin4File bin3File = new Bin4JsonHandler.Bin4File();

            bin3File.U0 = U0;
            bin3File.U1 = U1;
            bin3File.U2 = U2;
            bin3File.U3 = U3;

            bin3File.U4 = JsonUtil.Vector4ToArray(U4);
            bin3File.U5 = JsonUtil.Vector4ToArray(U5);
            bin3File.U6 = JsonUtil.Vector4ToArray(U6);
            bin3File.U7 = JsonUtil.Vector4ToArray(U7);

            bin3File.U8 = JsonUtil.Vector4ToArray(U8);

            bin3File.U9 = U9;
            bin3File.U10 = U10;

            bin3File.U11 = JsonUtil.Vector3ToArray(U11);
            bin3File.U12 = JsonUtil.Vector3ToArray(U12);

            bin3File.U13 = U13;
            bin3File.U14 = U14;
            bin3File.U15 = U15;
            bin3File.U16 = U16;

            return bin3File;
        }
    }
}
