﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2
{
    public class SSFHandler
    {
        public int U1;
        public int U2;
        public float U3;
        public int UStruct1Count;
        public int UStruct1Offset;
        public int UStruct2Count;
        public int UStruct2Offset; 
        public int UStruct3Count;
        public int UStruct3Offset;
        public int UStruct4Count;
        public int UStruct4Offset;
        public int FunctionCount;
        public int FunctionOffset;
        public int UStruct5Count;
        public int UStruct5Offset;
        public int UIntsCount;
        public int UIntsOffset;
        public int UStruct6Count;
        public int UStruct6Offset;

        public List<UStruct1> uStruct1s = new List<UStruct1>();
        public List<UStruct2> uStruct2s = new List<UStruct2>();
        public List<UStruct3> uStruct3s = new List<UStruct3>();
        public List<UStruct4> uStruct4s = new List<UStruct4>();
        public List<Function> Functions = new List<Function>();
        public List<UStruct5> uStruct5s = new List<UStruct5>();
        public List<UStruct6> uStruct6s = new List<UStruct6>();
        public List<int> UInts = new List<int>();

        public void Load(string Path)
        {
            using (Stream stream = File.Open(Path, FileMode.Open))
            {
                U1 = StreamUtil.ReadInt32(stream);
                U2 = StreamUtil.ReadInt32(stream);
                U3 = StreamUtil.ReadFloat(stream);
                UStruct1Count = StreamUtil.ReadInt32(stream);
                UStruct1Offset = StreamUtil.ReadInt32(stream);
                UStruct2Count = StreamUtil.ReadInt32(stream);
                UStruct2Offset = StreamUtil.ReadInt32(stream);
                UStruct3Count = StreamUtil.ReadInt32(stream);
                UStruct3Offset = StreamUtil.ReadInt32(stream);
                UStruct4Count = StreamUtil.ReadInt32(stream);
                UStruct4Offset = StreamUtil.ReadInt32(stream);
                FunctionCount = StreamUtil.ReadInt32(stream);
                FunctionOffset = StreamUtil.ReadInt32(stream);
                UStruct5Count = StreamUtil.ReadInt32(stream);
                UStruct5Offset = StreamUtil.ReadInt32(stream);
                UIntsCount = StreamUtil.ReadInt32(stream);
                UIntsOffset = StreamUtil.ReadInt32(stream);
                UStruct6Count = StreamUtil.ReadInt32(stream);
                UStruct6Offset = StreamUtil.ReadInt32(stream);

                uStruct1s = new List<UStruct1>();
                stream.Position = UStruct1Offset;
                for (int i = 0; i < UStruct1Count; i++)
                {
                    var TempUstruct1 = new UStruct1();

                    TempUstruct1.U1 = StreamUtil.ReadInt32(stream);
                    TempUstruct1.U2 = StreamUtil.ReadInt32(stream);
                    TempUstruct1.U3 = StreamUtil.ReadInt32(stream);
                    TempUstruct1.U4 = StreamUtil.ReadInt32(stream);
                    TempUstruct1.U5 = StreamUtil.ReadInt32(stream);
                    TempUstruct1.U6 = StreamUtil.ReadInt32(stream);
                    TempUstruct1.U7 = StreamUtil.ReadInt32(stream);
                    uStruct1s.Add(TempUstruct1);
                }

                uStruct2s = new List<UStruct2>();
                stream.Position = UStruct2Offset;
                for (int i = 0; i < UStruct2Count; i++)
                {
                    var TempUstruct2 = new UStruct2();

                    TempUstruct2.U1 = StreamUtil.ReadInt32(stream);
                    TempUstruct2.U2 = StreamUtil.ReadInt32(stream);
                    TempUstruct2.U3 = StreamUtil.ReadInt32(stream);
                    uStruct2s.Add(TempUstruct2);
                }

                uStruct3s = new List<UStruct3>();
                stream.Position = UStruct3Offset;
                for (int i = 0; i < UStruct3Count; i++)
                {
                    var TempUstruct3 = new UStruct3();

                    TempUstruct3.U1 = StreamUtil.ReadInt32(stream);
                    TempUstruct3.U2 = StreamUtil.ReadInt32(stream);
                    TempUstruct3.U3 = StreamUtil.ReadInt32(stream);
                    uStruct3s.Add(TempUstruct3);
                }

                uStruct4s = new List<UStruct4>();
                stream.Position = UStruct4Offset;
                for (int i = 0; i < UStruct4Count; i++)
                {
                    var TempUstruct4 = new UStruct4();
                    TempUstruct4.U1 = StreamUtil.ReadInt32(stream);
                    TempUstruct4.U2 = StreamUtil.ReadInt32(stream);
                    uStruct4s.Add(TempUstruct4);
                }

                Functions = new List<Function>();
                stream.Position = FunctionOffset;
                for (int i = 0; i < FunctionCount; i++)
                {
                    var TempFunction = new Function();
                    TempFunction.U1 = StreamUtil.ReadInt32(stream);
                    TempFunction.U2 = StreamUtil.ReadInt32(stream);
                    TempFunction.FunctionName = StreamUtil.ReadString(stream, 16);
                    Functions.Add(TempFunction);
                }

                uStruct5s = new List<UStruct5>();
                stream.Position = UStruct5Offset;
                for (int i = 0; i < UStruct5Count; i++)
                {
                    var TempUstruct5 = new UStruct5();
                    TempUstruct5.U1 = StreamUtil.ReadInt32(stream);
                    TempUstruct5.U2 = StreamUtil.ReadInt32(stream);
                    TempUstruct5.U3 = StreamUtil.ReadInt32(stream);
                    TempUstruct5.U4 = StreamUtil.ReadInt32(stream);
                    TempUstruct5.U5 = StreamUtil.ReadInt32(stream);
                    TempUstruct5.U6 = StreamUtil.ReadInt32(stream);
                    uStruct5s.Add(TempUstruct5);
                }

                UInts = new List<int>();
                for (int i = 0; i < UIntsCount; i++)
                {
                    UInts.Add(StreamUtil.ReadInt32(stream));
                }

                uStruct6s = new List<UStruct6>();
                stream.Position = UStruct6Offset;
                for (int i = 0; i < UStruct6Count; i++)
                {
                    var TempUstruct6 = new UStruct6();
                    TempUstruct6.U1 = StreamUtil.ReadInt32(stream);
                    TempUstruct6.U2 = StreamUtil.ReadInt32(stream);
                    uStruct6s.Add(TempUstruct6);
                }

            }
        }

        public struct UStruct1
        {
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
            public int U7;
        }

        public struct UStruct2
        {
            public int U1;
            public int U2;
            public int U3;
        }

        public struct UStruct3
        {
            public int U1;
            public int U2;
            public int U3;
        }

        public struct UStruct4
        {
            public int U1;
            public int U2;
        }

        public struct Function
        {
            public int U1;
            public int U2;
            public string FunctionName;
        }

        public struct UStruct5
        {
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
        }

        public struct UStruct6
        {
            public int U1;
            public int U2;
        }
    }
}
