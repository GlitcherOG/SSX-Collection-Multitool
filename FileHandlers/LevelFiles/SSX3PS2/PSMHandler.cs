using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2
{
    internal class PSMHandler
    {
        public byte[] Unknown1 = new byte[8];
        public int NumArrays;
        public List<NameList> nameLists = new List<NameList>();

        public void LoadPSM(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                bool test = false;

                Unknown1 = StreamUtil.ReadBytes(stream, 8);
                NumArrays = StreamUtil.ReadUInt32(stream);
                nameLists = new List<NameList>();
                for (int i = 0; i < NumArrays; i++)
                {
                    var TempNameList = new NameList();
                    TempNameList.Unknown2 = StreamUtil.ReadBytes(stream, 4);
                    TempNameList.NumStrings = StreamUtil.ReadUInt32(stream);
                    TempNameList.strings = new List<string>();
                    for (int a = 0; a < TempNameList.NumStrings; a++)
                    {
                        TempNameList.strings.Add(StreamUtil.ReadNullEndString(stream));
                        stream.Position++;
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
            public byte[] Unknown2;
            public int NumStrings;
            public List<string> strings;
        }
    }
}
