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
            U0 = ObjectIDLoad(stream);
            U1 = ObjectIDLoad(stream);
            U2 = ObjectIDLoad(stream);
            U3 = ObjectIDLoad(stream);
            U4 = ObjectIDLoad(stream);
            U5 = ObjectIDLoad(stream);
            U6 = ObjectIDLoad(stream);
            U7 = ObjectIDLoad(stream);
            U8 = ObjectIDLoad(stream);
            U9 = ObjectIDLoad(stream);
            U10 = ObjectIDLoad(stream);
            U11 = ObjectIDLoad(stream);
            U12 = ObjectIDLoad(stream);
            U13 = ObjectIDLoad(stream);
        }

        public ObjectID ObjectIDLoad(Stream stream)
        {
            ObjectID U = new ObjectID();

            U.TrackID = StreamUtil.ReadUInt8(stream);
            U.RID = StreamUtil.ReadInt24(stream);
            
            return U;
        }

        public struct ObjectID
        {
            public int TrackID;
            public int RID;
        }
    }
}
