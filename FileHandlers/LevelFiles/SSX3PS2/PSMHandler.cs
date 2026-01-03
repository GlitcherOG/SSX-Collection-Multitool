using SSXMultiTool.Utilities;
using System.IO;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2
{
    public class PSMHandler
    {
        public float U0;
        public float U1;
        public int NumArrays;
        public List<NameList> nameLists = new List<NameList>();

        public void LoadPSM(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                bool test = false;

                U0  = StreamUtil.ReadFloat(stream);
                U1 = StreamUtil.ReadFloat(stream);
                NumArrays = StreamUtil.ReadUInt32(stream);
                nameLists = new List<NameList>();
                for (int i = 0; i < NumArrays; i++)
                {
                    var TempNameList = new NameList();
                    TempNameList.Unknown2 = StreamUtil.ReadUInt32(stream);
                    TempNameList.NumStrings = StreamUtil.ReadUInt32(stream);
                    TempNameList.strings = new List<string>();
                    for (int a = 0; a < TempNameList.NumStrings; a++)
                    {
                        TempNameList.strings.Add(StreamUtil.ReadNullEndString(stream));
                        stream.Position++; //Put here to fix bug with Read Null End String
                    }
                    stream.Position++;
                    if(test)
                    {
                        stream.Position++;
                    }
                    test = !test;
                    nameLists.Add(TempNameList);
                }
            }
        }

        public struct NameList
        {
            public int Unknown2;
            public int NumStrings;
            public List<string> strings;
        }
    }
}
