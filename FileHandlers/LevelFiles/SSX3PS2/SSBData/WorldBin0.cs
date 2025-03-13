using SSXMultiTool.JsonFiles.SSX3;
using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2.SSBData
{
    public class WorldBin0
    {
        public int U0;
        public int U1;
        public int U2;
        public int U3;
        public int U4;
        public int U5;
        public int U6;
        public int U7;
        public int U8;
        public int U9;
        public int U10;


        public void LoadData(Stream stream)
        {
            U0 = StreamUtil.ReadInt16(stream);
            U1 = StreamUtil.ReadInt16(stream);
            U2 = StreamUtil.ReadInt16(stream);
            U3 = StreamUtil.ReadInt16(stream);
            U4 = StreamUtil.ReadInt16(stream);
            U5 = StreamUtil.ReadInt16(stream);
            U6 = StreamUtil.ReadInt16(stream);
            U7 = StreamUtil.ReadInt16(stream);
            U8 = StreamUtil.ReadInt16(stream);
            U9 = StreamUtil.ReadInt16(stream);
            U10 = StreamUtil.ReadInt16(stream);
        }

        public Bin0JsonHandler.Bin0File ToJSON()
        {
            Bin0JsonHandler.Bin0File bin0File = new Bin0JsonHandler.Bin0File();

            bin0File.U0 = U0;
            bin0File.U1 = U1;
            bin0File.U2 = U2;
            bin0File.U3 = U3;
            bin0File.U4 = U4;
            bin0File.U5 = U5;
            bin0File.U6 = U6;
            bin0File.U7 = U7;
            bin0File.U8 = U8;
            bin0File.U9 = U9;
            bin0File.U10 = U10;

            return bin0File;
        }

    }
}
