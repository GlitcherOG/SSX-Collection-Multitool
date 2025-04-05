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
    public class WorldBin6
    {
        public int U0;
        public int U1;
        public int U2;
        public int U3;

        public int U4;
        public float U5;
        public float U6;
        public float U7;

        public float U8;
        public float U9;
        public float U10;
        public float U11;

        public int U12;
        public int U13;
        public int U14;
        public int U15;

        public int U16;
        public int U17;
        public int U18;
        public int U19;

        public int U20;
        public int U21;
        public int U22;
        public int U23;

        public int U24;
        public int U25;
        public int U26;
        public int U27;


        public void LoadData(Stream stream)
        {
            U0 = StreamUtil.ReadInt32(stream);
            U1 = StreamUtil.ReadInt32(stream);
            U2 = StreamUtil.ReadInt32(stream);
            U3 = StreamUtil.ReadInt32(stream);

            U4 = StreamUtil.ReadInt32(stream);
            U5 = StreamUtil.ReadFloat(stream);
            U6 = StreamUtil.ReadFloat(stream);
            U7 = StreamUtil.ReadFloat(stream);

            U8 = StreamUtil.ReadFloat(stream);
            U8 = StreamUtil.ReadFloat(stream);
            U10 = StreamUtil.ReadFloat(stream);
            U11 = StreamUtil.ReadFloat(stream);

            U12 = StreamUtil.ReadInt32(stream);
            U13 = StreamUtil.ReadInt32(stream);
            U14 = StreamUtil.ReadInt32(stream);
            U15 = StreamUtil.ReadInt32(stream);

            U16 = StreamUtil.ReadInt32(stream);
            U17 = StreamUtil.ReadInt32(stream);
            U18 = StreamUtil.ReadInt32(stream);
            U19 = StreamUtil.ReadInt32(stream);

            U20 = StreamUtil.ReadInt32(stream);
            U21 = StreamUtil.ReadInt32(stream);
            U22 = StreamUtil.ReadInt32(stream);
            U23 = StreamUtil.ReadInt32(stream);

            U24 = StreamUtil.ReadInt32(stream);
            U25 = StreamUtil.ReadInt32(stream);
            U26 = StreamUtil.ReadInt32(stream);
            U27 = StreamUtil.ReadInt32(stream);
        }

        public Bin6JsonHandler.Bin6File ToJSON()
        {
            Bin6JsonHandler.Bin6File bin3File = new Bin6JsonHandler.Bin6File();

            bin3File.U0 = U0;
            bin3File.U1 = U1;
            bin3File.U2 = U2;
            bin3File.U3 = U3;

            bin3File.U4 = U4;
            bin3File.U5 = U5;
            bin3File.U6 = U6;
            bin3File.U7 = U7;

            bin3File.U8 = U8;
            bin3File.U9 = U9;
            bin3File.U10 = U10;
            bin3File.U11 = U11;

            bin3File.U12 = U12;
            bin3File.U13 = U13;
            bin3File.U14 = U14;
            bin3File.U15 = U15;

            bin3File.U16 = U16;
            bin3File.U17 = U17;
            bin3File.U18 = U18;
            bin3File.U19 = U19;

            bin3File.U20 = U20;
            bin3File.U21 = U21;
            bin3File.U22 = U22;
            bin3File.U23 = U23;

            bin3File.U24 = U24;
            bin3File.U25 = U25;
            bin3File.U26 = U26;
            bin3File.U27 = U27;

            return bin3File;
        }
    }
}
