using SSXMultiTool.Utilities;
using System.IO;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2
{
    public class PHMHandler
    {
        public float U0;
        public float U1;
        public int NumArrays;

        public List<ResourceLink> resourceLinks = new List<ResourceLink>();
        
        public void LoadPHM(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                bool test = false;

                U0 = StreamUtil.ReadFloat(stream);
                U1 = StreamUtil.ReadFloat(stream);
                NumArrays = StreamUtil.ReadUInt32(stream);
                resourceLinks = new List<ResourceLink>();
                for (int i = 0; i < NumArrays; i++)
                {
                    var TempEntryList = new ResourceLink();
                    TempEntryList.U0 = StreamUtil.ReadUInt32(stream);
                    TempEntryList.NumEntries = StreamUtil.ReadUInt32(stream);
                    TempEntryList.Entries = new List<Entry>();
                    for (int a = 0; a < TempEntryList.NumEntries; a++)
                    {
                        var TempEntry = new Entry();

                        TempEntry.U0 = StreamUtil.ReadUInt32(stream);
                        TempEntry.U1 = StreamUtil.ReadUInt32(stream);
                        TempEntry.TrackID = StreamUtil.ReadInt8(stream);
                        TempEntry.RID = StreamUtil.ReadUInt24(stream);
                        TempEntry.U3 = StreamUtil.ReadUInt32(stream);

                        TempEntryList.Entries.Add(TempEntry);
                    }
                    resourceLinks.Add(TempEntryList);
                }
            }
        }

        //0 - Patches
        //1 - Models?
        public string GetName(int TrackID, int RID, int ArrayID, PSMHandler psmHandler)
        {
            var Array = resourceLinks[ArrayID];

            for (int i = 0; i < Array.Entries.Count; i++)
            {
                if (Array.Entries[i].RID == RID && Array.Entries[i].TrackID == TrackID)
                {
                    return psmHandler.nameLists[ArrayID].strings[i];
                }
            }

            return "Null";
        }

        public struct ResourceLink
        {
            public int U0;
            public int NumEntries;
            public List<Entry> Entries;
        }

        public struct Entry
        {
            public int U0;
            public int U1;
            public int TrackID;
            public int RID;
            public int U3;
        }
    }
}
