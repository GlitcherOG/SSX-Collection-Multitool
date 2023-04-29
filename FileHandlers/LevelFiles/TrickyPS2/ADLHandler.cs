using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2
{
    public class ADLHandler
    {
        public byte[] Magic;
        public float U0;
        public int UCount;
        public List<UStruct0> UStruct = new List<UStruct0>();

        //Put into a seperate list just so its easier to find out wtf is going on
        public List<UStruct1> uStruct1s = new List<UStruct1>();

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                Magic = StreamUtil.ReadBytes(stream,4);
                U0 = StreamUtil.ReadFloat(stream);
                UCount = StreamUtil.ReadUInt32(stream);

                UStruct = new List<UStruct0>();

                for (int i = 0; i < UCount; i++)
                {
                    var NewUStruct = new UStruct0();

                    NewUStruct.Hash = StreamUtil.ReadUInt32(stream);
                    NewUStruct.UOffset = StreamUtil.ReadUInt32(stream);

                    NewUStruct.uStruct1 = new UStruct1();
                    int TempPos = (int)stream.Position;
                    stream.Position = NewUStruct.UOffset;

                    //UStruct0
                    NewUStruct.uStruct1.U0 = StreamUtil.ReadUInt32(stream);
                    NewUStruct.uStruct1.UCount = StreamUtil.ReadUInt32(stream);
                    NewUStruct.uStruct1.U2 = new List<UStruct2>();

                    for (int a = 0; a < NewUStruct.uStruct1.UCount; a++)
                    {
                        var NewUStruct2 = new UStruct2();
                        NewUStruct2.U0 = StreamUtil.ReadUInt32(stream);
                        NewUStruct2.U1 = StreamUtil.ReadUInt32(stream);
                        NewUStruct2.U2 = StreamUtil.ReadFloat(stream);
                        NewUStruct2.U3 = StreamUtil.ReadFloat(stream);
                        NewUStruct2.U4 = StreamUtil.ReadFloat(stream);
                        NewUStruct2.U5 = StreamUtil.ReadFloat(stream);
                        NewUStruct2.U6 = StreamUtil.ReadFloat(stream);
                        NewUStruct.uStruct1.U2.Add(NewUStruct2);
                    }
                    stream.Position = TempPos;
                    UStruct.Add(NewUStruct);
                }

            }
        }

        public void Save(string Path)
        {

        }

        public struct UStruct0
        {
            public int Hash;
            public int UOffset;
            public UStruct1 uStruct1;
        }


        public struct UStruct1
        {
            public int U0;
            public int UCount;
            public List<UStruct2> U2;
        }

        public struct UStruct2
        {
            public int U0;
            public int U1;
            public float U2;
            public float U3;
            public float U4;
            public float U5;
            public float U6;
        }
    }
}
