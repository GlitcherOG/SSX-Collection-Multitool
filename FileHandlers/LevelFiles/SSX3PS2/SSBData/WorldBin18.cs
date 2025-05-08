using SSXMultiTool.JsonFiles.SSX3;
using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2.SSBData;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2.SSBData
{
    public class WorldBin18
    {
        public ObjectID U0;
        public ObjectID U1;
        public ObjectID U2;
        public ObjectID U3;
        public ObjectID U4;
        public ObjectID U5;
        public ObjectID U6;
        public ObjectID U7;
        public ObjectID U8;
        public ObjectID U9;
        public ObjectID U10;
        public ObjectID U11;
        public ObjectID U12;
        public ObjectID U13;

        public void LoadData(Stream stream, int TrackID, int RID)
        {
            U0 = WorldCommon.ObjectIDLoad(stream);
            U1 = WorldCommon.ObjectIDLoad(stream);
            U2 = WorldCommon.ObjectIDLoad(stream);
            U3 = WorldCommon.ObjectIDLoad(stream);
            U4 = WorldCommon.ObjectIDLoad(stream);
            U5 = WorldCommon.ObjectIDLoad(stream);
            U6 = WorldCommon.ObjectIDLoad(stream);
            U7 = WorldCommon.ObjectIDLoad(stream);
            U8 = WorldCommon.ObjectIDLoad(stream);
            U9 = WorldCommon.ObjectIDLoad(stream);
            U10 = WorldCommon.ObjectIDLoad(stream);
            U11 = WorldCommon.ObjectIDLoad(stream);
            U12 = WorldCommon.ObjectIDLoad(stream);
            U13 = WorldCommon.ObjectIDLoad(stream);
        }
    }
}
