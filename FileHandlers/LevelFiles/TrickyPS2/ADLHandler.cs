using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2
{
    public class ADLHandler
    {
        public byte[] Magic;
        public float U0; //Anything Other than 1 breaks sound
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

        public void Save(string path)
        {
            //Generate UStruct1 List
            uStruct1s = new List<UStruct1>();
            for (int i = 0; i < UStruct.Count; i++)
            {
                var TempUStruct = UStruct[i];
                bool Exists = false;

                for (int a = 0; a < uStruct1s.Count; a++)
                {
                    var first = uStruct1s[a];
                    var second = TempUStruct.uStruct1;

                    if (first.U0 == second.U0 && first.U2.Count == second.U2.Count)
                    {
                        if(first.U2.Count==0)
                        {
                            Exists = true;
                            TempUStruct.UStruct1Index = a;
                            break;
                        }
                        bool TestFail = false;
                        for (int b = 0; b < first.U2.Count; b++)
                        {
                            if (first.U2[b].U0 != second.U2[b].U0
                                || first.U2[b].U1 != second.U2[b].U1
                                || first.U2[b].U2 != second.U2[b].U2
                                || first.U2[b].U3 != second.U2[b].U3
                                || first.U2[b].U4 != second.U2[b].U4
                                || first.U2[b].U5 != second.U2[b].U5
                                || first.U2[b].U6 != second.U2[b].U6)
                            {
                                TestFail = true;
                                break;
                            }
                        }
                        if (!TestFail)
                        {
                            Exists = true;
                            TempUStruct.UStruct1Index = a;
                        }
                        else
                        {
                            Exists = false;
                        }
                        break;
                    }
                }

                if(!Exists)
                {
                    uStruct1s.Add(TempUStruct.uStruct1);
                    TempUStruct.UStruct1Index = uStruct1s.Count - 1;
                }

                UStruct[i] = TempUStruct;
            }


            MemoryStream stream = new MemoryStream();

            //Write Header
            StreamUtil.WriteBytes(stream, Magic);
            StreamUtil.WriteFloat32(stream, U0);
            StreamUtil.WriteInt32(stream, UStruct.Count);

            //Skip UStruct 0 and write UStruct 1 Data Making sure to have offset in u1
            stream.Position += UStruct.Count * 4 * 2;

            for (int i = 0; i < uStruct1s.Count; i++)
            {
                var TempUstruct1 = uStruct1s[i];
                TempUstruct1.Offset = (int)stream.Position;

                StreamUtil.WriteInt32(stream, TempUstruct1.U0);
                StreamUtil.WriteInt32(stream, TempUstruct1.U2.Count);

                for (int a = 0; a < TempUstruct1.U2.Count; a++)
                {
                    StreamUtil.WriteInt32(stream, TempUstruct1.U2[a].U0);
                    StreamUtil.WriteInt32(stream, TempUstruct1.U2[a].U1);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.U2[a].U2);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.U2[a].U3);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.U2[a].U4);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.U2[a].U5); //Radius?
                    StreamUtil.WriteFloat32(stream, TempUstruct1.U2[a].U6);
                }

                uStruct1s[i] = TempUstruct1;
            }

            StreamUtil.WriteUInt8(stream, 0xff);

            //Go back and write 0
            stream.Position = 4 * 3;
            for (int i = 0; i < UStruct.Count; i++)
            {
                StreamUtil.WriteInt32(stream, UStruct[i].Hash);
                StreamUtil.WriteInt32(stream, uStruct1s[UStruct[i].UStruct1Index].Offset);
            }


            if (File.Exists(path))
            {
                File.Delete(path);
            }
            var file = File.Create(path);
            stream.Position = 0;
            stream.CopyTo(file);
            stream.Dispose();
            file.Close();
        }

        public struct UStruct0
        {
            public int Hash;
            public int UOffset;
            public UStruct1 uStruct1;

            public int UStruct1Index;
        }


        public struct UStruct1
        {
            public int U0;
            public int UCount;
            public List<UStruct2> U2;

            public int Offset;
        }

        public struct UStruct2
        {
            public int U0;
            public int U1; //Channel ID? So that sounds dont overlap in someway
            public float U2;
            public float U3;
            public float U4;
            public float U5; //Radius?
            public float U6;
        }
    }
}
