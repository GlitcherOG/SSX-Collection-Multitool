﻿using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SSXMultiTool.FileHandlers.LevelFiles.OGPS2
{
    internal class WFXHandler
    {
        public float U0;
        public float U1;
        public int EffectSlotsCount;
        public int EffectSlotsOffset;
        public int UStruct1Count;
        public int UStruct1Offset;
        public int CollisonModelCount;
        public int CollisonModelOffset;
        public int EffectHeaderCount;
        public int EffectHeaderOffset;

        public List<EffectSlot> EffectSlots = new List<EffectSlot>();
        public List<UStruct1> uStruct1s = new List<UStruct1>();
        public List<CollisonModelPointer> CollisonModelPointers = new List<CollisonModelPointer>();
        public List<EffectHeader> EffectHeaders = new List<EffectHeader>();

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                U0 = StreamUtil.ReadFloat(stream);
                U1 = StreamUtil.ReadFloat(stream);
                EffectSlotsCount = StreamUtil.ReadUInt32(stream);
                EffectSlotsOffset = StreamUtil.ReadUInt32(stream);
                UStruct1Count = StreamUtil.ReadUInt32(stream);
                UStruct1Offset = StreamUtil.ReadUInt32(stream);
                CollisonModelCount = StreamUtil.ReadUInt32(stream);
                CollisonModelOffset = StreamUtil.ReadUInt32(stream);
                EffectHeaderCount = StreamUtil.ReadUInt32(stream);
                EffectHeaderOffset = StreamUtil.ReadUInt32(stream);

                EffectSlots = new List<EffectSlot>();
                stream.Position = EffectSlotsOffset;
                for (int i = 0; i < EffectSlotsCount; i++)
                {
                    var TempUstruct0 = new EffectSlot();

                    TempUstruct0.Slot1 = StreamUtil.ReadUInt32(stream);
                    TempUstruct0.Slot2 = StreamUtil.ReadUInt32(stream);
                    TempUstruct0.Slot3 = StreamUtil.ReadUInt32(stream);
                    TempUstruct0.Slot4 = StreamUtil.ReadUInt32(stream);
                    TempUstruct0.Slot5 = StreamUtil.ReadUInt32(stream);
                    TempUstruct0.Slot6 = StreamUtil.ReadUInt32(stream);
                    TempUstruct0.Slot7 = StreamUtil.ReadUInt32(stream);

                    EffectSlots.Add(TempUstruct0);
                }

                uStruct1s = new List<UStruct1>();
                stream.Position = UStruct1Offset;
                for (int i = 0; i < UStruct1Count; i++)
                {
                    var TempUStruct1 = new UStruct1();

                    TempUStruct1.Offset = StreamUtil.ReadUInt32(stream);
                    TempUStruct1.ByteSize = StreamUtil.ReadUInt32(stream);
                    TempUStruct1.UStruct3Count = StreamUtil.ReadUInt32(stream);

                    TempUStruct1.uStruct3s = new List<UStruct3>();

                    long TempPos = stream.Position;
                    stream.Position = TempUStruct1.Offset;
                    for (int a = 0; a < TempUStruct1.UStruct3Count; a++)
                    {
                        var NewTempStruct = new UStruct3();

                        NewTempStruct.EndAlligment = StreamUtil.ReadUInt32(stream);
                        NewTempStruct.U1 = StreamUtil.ReadUInt32(stream);
                        NewTempStruct.U2 = StreamUtil.ReadUInt32(stream);
                        NewTempStruct.UStruct4Count = StreamUtil.ReadUInt32(stream);

                        NewTempStruct.U4 = StreamUtil.ReadUInt32(stream);
                        NewTempStruct.U5 = StreamUtil.ReadFloat(stream);
                        NewTempStruct.U6 = StreamUtil.ReadFloat(stream);
                        NewTempStruct.U7 = StreamUtil.ReadFloat(stream);

                        NewTempStruct.U8 = StreamUtil.ReadFloat(stream);
                        NewTempStruct.U9 = StreamUtil.ReadFloat(stream);
                        NewTempStruct.U10 = StreamUtil.ReadFloat(stream);
                        NewTempStruct.U11 = StreamUtil.ReadFloat(stream);

                        NewTempStruct.U12 = StreamUtil.ReadFloat(stream);
                        NewTempStruct.U13 = StreamUtil.ReadFloat(stream);
                        NewTempStruct.U14 = StreamUtil.ReadFloat(stream);
                        NewTempStruct.U15 = StreamUtil.ReadFloat(stream);

                        NewTempStruct.U16 = StreamUtil.ReadFloat(stream);
                        NewTempStruct.U17 = StreamUtil.ReadFloat(stream);
                        NewTempStruct.U18 = StreamUtil.ReadFloat(stream);
                        NewTempStruct.U19 = StreamUtil.ReadFloat(stream);

                        NewTempStruct.U20 = StreamUtil.ReadFloat(stream);
                        NewTempStruct.U21 = StreamUtil.ReadFloat(stream);
                        NewTempStruct.U22 = StreamUtil.ReadFloat(stream);
                        NewTempStruct.U23 = StreamUtil.ReadFloat(stream);

                        NewTempStruct.U24 = StreamUtil.ReadFloat(stream);
                        NewTempStruct.U25 = StreamUtil.ReadFloat(stream);
                        NewTempStruct.U26 = StreamUtil.ReadFloat(stream);
                        NewTempStruct.U27 = StreamUtil.ReadFloat(stream);

                        NewTempStruct.struct4s = new List<UStruct4>();

                        for (int b = 0; b < NewTempStruct.UStruct4Count + 1; b++)
                        {
                            UStruct4 TempStruct4 = new UStruct4();

                            TempStruct4.U0 = StreamUtil.ReadFloat(stream);
                            TempStruct4.U1 = StreamUtil.ReadFloat(stream);
                            TempStruct4.U2 = StreamUtil.ReadUInt32(stream);

                            NewTempStruct.struct4s.Add(TempStruct4);
                        }

                        if (NewTempStruct.U1 != -1)
                        {
                            NewTempStruct.UByteData = StreamUtil.ReadBytes(stream, NewTempStruct.U1);
                        }
                        if (NewTempStruct.EndAlligment != -1)
                        {
                            NewTempStruct.UEndData = StreamUtil.ReadBytes(stream, NewTempStruct.EndAlligment);
                        }

                        TempUStruct1.uStruct3s.Add(NewTempStruct);
                    }

                    stream.Position = TempPos;

                    uStruct1s.Add(TempUStruct1);
                }

                CollisonModelPointers = new List<CollisonModelPointer>();
                stream.Position = CollisonModelOffset;
                int MeshID = 0;
                for (int i = 0; i < CollisonModelCount; i++)
                {
                    var TempUStruct2 = new CollisonModelPointer();

                    TempUStruct2.Offset = StreamUtil.ReadUInt32(stream);
                    TempUStruct2.ByteSize = StreamUtil.ReadUInt32(stream);
                    TempUStruct2.Count = StreamUtil.ReadUInt32(stream);

                    var TempPos = stream.Position;
                    stream.Position = TempUStruct2.Offset;

                    //Inset Stuff Here
                    TempUStruct2.Models = new List<CollisonModel>();
                    for (int a = 0; a < TempUStruct2.Count; a++)
                    {
                        var TempModel = new CollisonModel();
                        TempModel.FaceCount = StreamUtil.ReadUInt32(stream);
                        TempModel.VerticeCount = StreamUtil.ReadUInt32(stream);
                        TempModel.VerticeOffsetAlign = StreamUtil.ReadUInt32(stream);

                        TempModel.MeshPath = MeshID.ToString() + ".obj";

                        TempModel.Index = new List<int>();
                        TempModel.Vertices = new List<Vector4>();
                        TempModel.FaceNormals = new List<Vector4>();

                        for (int b = 0; b < TempModel.FaceCount*3; b++)
                        {
                            TempModel.Index.Add(StreamUtil.ReadUInt32(stream));
                        }
                        stream.Position += TempModel.VerticeOffsetAlign;

                        //StreamUtil.AlignBy16(stream);

                        for (int b = 0; b < TempModel.VerticeCount; b++)
                        {
                            TempModel.Vertices.Add(StreamUtil.ReadVector4(stream));
                        }

                        for (int b = 0; b < TempModel.FaceCount; b++)
                        {
                            TempModel.FaceNormals.Add(StreamUtil.ReadVector4(stream));
                        }


                        TempUStruct2.Models.Add(TempModel);
                        MeshID++;
                    }

                    stream.Position = TempPos;

                    CollisonModelPointers.Add(TempUStruct2);
                }

                EffectHeaders = new List<EffectHeader>();
                stream.Position = EffectHeaderOffset;
                for (int i = 0; i < EffectHeaderCount; i++)
                {
                    EffectHeader TempEffectHeader = new EffectHeader();

                    TempEffectHeader.Count = StreamUtil.ReadUInt32(stream);
                    TempEffectHeader.Offset = StreamUtil.ReadUInt32(stream);

                    TempEffectHeader.Effects = new List<Effect>();
                    long TempPos = stream.Position;
                    stream.Position = EffectHeaderOffset + (EffectHeaderCount*8) + TempEffectHeader.Offset;
                    for (int a = 0; a < TempEffectHeader.Count; a++)
                    {
                        TempEffectHeader.Effects.Add(LoadEffect(stream));
                    }

                    stream.Position = TempPos;

                    EffectHeaders.Add(TempEffectHeader);
                }


            }

        }

        public void Save(string path)
        {
            MemoryStream stream = new MemoryStream();

            stream.Position += 4 * 10;

            EffectSlotsOffset = (int)stream.Position;
            for (int i = 0; i < EffectSlots.Count; i++)
            {
                StreamUtil.WriteInt32(stream, EffectSlots[i].Slot1);
                StreamUtil.WriteInt32(stream, EffectSlots[i].Slot2);
                StreamUtil.WriteInt32(stream, EffectSlots[i].Slot3);
                StreamUtil.WriteInt32(stream, EffectSlots[i].Slot4);
                StreamUtil.WriteInt32(stream, EffectSlots[i].Slot5);
                StreamUtil.WriteInt32(stream, EffectSlots[i].Slot6);
                StreamUtil.WriteInt32(stream, EffectSlots[i].Slot7);
            }

            UStruct1Offset = (int)stream.Position;
            stream.Position += 4 * 3 * uStruct1s.Count;
            for (int i = 0; i < uStruct1s.Count; i++)
            {
                var TempUstruct1 = uStruct1s[i];
                TempUstruct1.Offset = (int)stream.Position;
                for (int a = 0; a < TempUstruct1.uStruct3s.Count; a++)
                {
                    StreamUtil.WriteInt32(stream, TempUstruct1.uStruct3s[a].EndAlligment);
                    StreamUtil.WriteInt32(stream, TempUstruct1.uStruct3s[a].U1);
                    StreamUtil.WriteInt32(stream, TempUstruct1.uStruct3s[a].U2);
                    StreamUtil.WriteInt32(stream, TempUstruct1.uStruct3s[a].struct4s.Count-1);

                    StreamUtil.WriteInt32(stream, TempUstruct1.uStruct3s[a].U4);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U5);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U6);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U7);

                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U8);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U9);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U10);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U11);

                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U12);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U13);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U14);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U15);

                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U16);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U17);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U18);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U19);

                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U20);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U21);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U22);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U23);

                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U24);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U25);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U26);
                    StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].U27);

                    for (int b = 0; b < TempUstruct1.uStruct3s[a].struct4s.Count; b++)
                    {
                        StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].struct4s[b].U0);
                        StreamUtil.WriteFloat32(stream, TempUstruct1.uStruct3s[a].struct4s[b].U1);
                        StreamUtil.WriteInt32(stream, TempUstruct1.uStruct3s[a].struct4s[b].U2);
                    }


                    StreamUtil.WriteBytes(stream, TempUstruct1.uStruct3s[a].UByteData);

                    StreamUtil.WriteBytes(stream, TempUstruct1.uStruct3s[a].UEndData);

                }

                uStruct1s[i] = TempUstruct1;
            }

            CollisonModelOffset = (int)stream.Position;
            stream.Position = UStruct1Offset;
            for (int i = 0; i < uStruct1s.Count; i++)
            {
                StreamUtil.WriteInt32(stream, uStruct1s[i].Offset);
                StreamUtil.WriteInt32(stream, uStruct1s[i].ByteSize);
                StreamUtil.WriteInt32(stream, uStruct1s[i].uStruct3s.Count);
            }
            stream.Position = CollisonModelOffset;

            stream.Position += 4 * 3 * CollisonModelPointers.Count;
            for (int i = 0; i < CollisonModelPointers.Count; i++)
            {

            }

            long TempPos = stream.Position;
            stream.Position = CollisonModelOffset;
            for (int i = 0; i < CollisonModelPointers.Count; i++)
            {
                StreamUtil.WriteInt32(stream, CollisonModelPointers[i].Offset);
                StreamUtil.WriteInt32(stream, CollisonModelPointers[i].ByteSize);
                StreamUtil.WriteInt32(stream, CollisonModelPointers[i].Count);
            }
            stream.Position = TempPos;

            stream.Position = 0;

            StreamUtil.WriteFloat32(stream, U0);
            StreamUtil.WriteFloat32(stream, U1);

            StreamUtil.WriteInt32(stream, EffectSlots.Count);
            StreamUtil.WriteInt32(stream, EffectSlotsOffset);
            StreamUtil.WriteInt32(stream, uStruct1s.Count);
            StreamUtil.WriteInt32(stream, UStruct1Offset);
            StreamUtil.WriteInt32(stream, CollisonModelPointers.Count);
            StreamUtil.WriteInt32(stream, CollisonModelOffset);
            StreamUtil.WriteInt32(stream, EffectHeaders.Count);
            StreamUtil.WriteInt32(stream, EffectHeaderOffset);

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

        public void SaveModels(string Path)
        {
            int c = 0;
            for (int a = 0; a < CollisonModelPointers.Count; a++)
            {
                var TempCollision = CollisonModelPointers[a];

                for (int ax = 0; ax < TempCollision.Models.Count; ax++)
                {
                    var Data = TempCollision.Models[ax];

                    string outputString = "";
                    string output = "# Exported From SSX Using SSX Multitool Modder by GlitcherOG \n";

                    outputString += "o Mesh" + c + "\n";
                    for (int b = 0; b < Data.FaceCount; b++)
                    {
                        outputString += "f " + (Data.Index[3 * b + 2] + 1) + "//" + (b + 1).ToString() + " " + (Data.Index[3 * b + 1] + 1) + "//" + (b + 1).ToString() + " " + (Data.Index[3 * b] + 1) + "//" + (b + 1).ToString() + "\n";
                    }

                    for (int z = 0; z < Data.Vertices.Count; z++)
                    {
                        output += "v " + Data.Vertices[z].X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Data.Vertices[z].Y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Data.Vertices[z].Z.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\n";
                    }
                    for (int z = 0; z < Data.FaceNormals.Count; z++)
                    {
                        output += "vn " + Data.FaceNormals[z].X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Data.FaceNormals[z].Y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Data.FaceNormals[z].Z.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\n";
                    }
                    output += outputString;
                    File.WriteAllText(Path + "/" + Data.MeshPath, output);
                    c++;
                }

            }

        }

        public Effect LoadEffect(Stream stream)
        {
            var TempEffect = new Effect();

            TempEffect.Type = StreamUtil.ReadUInt16(stream);
            TempEffect.Size = StreamUtil.ReadUInt16(stream);

            if (TempEffect.Type == 0)
            {
                Type0 TempType0 = new Type0();

                TempType0.SubType = StreamUtil.ReadUInt32(stream);

                if (TempType0.SubType == 0)
                {
                    Type0Sub0 type0Sub0 = new Type0Sub0();

                    type0Sub0.U1 = StreamUtil.ReadFloat(stream);
                    type0Sub0.U2 = StreamUtil.ReadFloat(stream);
                    type0Sub0.U3 = StreamUtil.ReadFloat(stream);
                    type0Sub0.U4 = StreamUtil.ReadFloat(stream);
                    type0Sub0.U5 = StreamUtil.ReadUInt32(stream);
                    type0Sub0.U6 = StreamUtil.ReadUInt32(stream);

                    TempType0.type0Sub0 = type0Sub0;
                }
                else if (TempType0.SubType == 2)
                {
                    TempType0.type0Sub2 = StreamUtil.ReadFloat(stream);
                }
                else if (TempType0.SubType == 5)
                {
                    TempType0.type0Sub5 = StreamUtil.ReadUInt32(stream);
                }
                else if (TempType0.SubType==6)
                {
                    Type0Sub6 type0Sub6 = new Type0Sub6();

                    type0Sub6.U0 = StreamUtil.ReadUInt32(stream);
                    type0Sub6.U1 = StreamUtil.ReadFloat(stream);

                    TempType0.type0Sub6 = type0Sub6;
                }
                else if (TempType0.SubType == 7)
                {
                    Type0Sub7 type0Sub7 = new Type0Sub7();

                    type0Sub7.U0 = StreamUtil.ReadUInt32(stream);
                    type0Sub7.U1 = StreamUtil.ReadUInt32(stream);
                    type0Sub7.U2 = StreamUtil.ReadUInt32(stream);
                    type0Sub7.U3 = StreamUtil.ReadUInt32(stream);
                    type0Sub7.U4 = StreamUtil.ReadUInt32(stream);
                    type0Sub7.U5 = StreamUtil.ReadUInt32(stream);
                    type0Sub7.U6 = StreamUtil.ReadUInt32(stream);

                    TempType0.type0Sub7 = type0Sub7;
                }
                else if (TempType0.SubType == 10)
                {
                    Type0Sub10 type0Sub10 = new Type0Sub10();

                    type0Sub10.U1 = StreamUtil.ReadFloat(stream);
                    type0Sub10.U2 = StreamUtil.ReadFloat(stream);
                    type0Sub10.U3 = StreamUtil.ReadFloat(stream);
                    type0Sub10.U4 = StreamUtil.ReadFloat(stream);
                    type0Sub10.U5 = StreamUtil.ReadUInt32(stream);
                    type0Sub10.U6 = StreamUtil.ReadUInt32(stream);

                    TempType0.type0Sub10 = type0Sub10;
                }
                else if (TempType0.SubType == 11)
                {
                    Type0Sub11 type0Sub11 = new Type0Sub11();

                    type0Sub11.U0 = StreamUtil.ReadUInt32(stream);
                    type0Sub11.U1 = StreamUtil.ReadUInt32(stream);
                    type0Sub11.U2 = StreamUtil.ReadUInt32(stream);
                    type0Sub11.U3 = StreamUtil.ReadUInt32(stream);

                    TempType0.type0Sub11 = type0Sub11;
                }
                else if (TempType0.SubType == 12)
                {
                    Type0Sub12 type0Sub12 = new Type0Sub12();

                    type0Sub12.U0 = StreamUtil.ReadUInt32(stream);
                    type0Sub12.U1 = StreamUtil.ReadFloat(stream);

                    TempType0.type0Sub12 = type0Sub12;
                }
                else if (TempType0.SubType == 13)
                {
                    Type0Sub13 type0Sub13 = new Type0Sub13();

                    type0Sub13.U0 = StreamUtil.ReadUInt32(stream);
                    type0Sub13.U1 = StreamUtil.ReadUInt32(stream);
                    type0Sub13.U2 = StreamUtil.ReadUInt32(stream);
                    type0Sub13.U3 = StreamUtil.ReadUInt32(stream);

                    TempType0.type0Sub13 = type0Sub13;
                }
                else if (TempType0.SubType == 14)
                {
                    Type0Sub14 type0Sub14 = new Type0Sub14();

                    type0Sub14.U0 = StreamUtil.ReadUInt32(stream);
                    type0Sub14.U1 = StreamUtil.ReadFloat(stream);

                    TempType0.type0Sub14 = type0Sub14;
                }
                else if (TempType0.SubType == 15)
                {
                    Type0Sub15 type0Sub15 = new Type0Sub15();

                    type0Sub15.U0 = StreamUtil.ReadFloat(stream);
                    type0Sub15.U1 = StreamUtil.ReadFloat(stream);
                    type0Sub15.U2 = StreamUtil.ReadFloat(stream);
                    type0Sub15.U3 = StreamUtil.ReadFloat(stream);
                    type0Sub15.U4 = StreamUtil.ReadFloat(stream);

                    TempType0.type0Sub15 = type0Sub15;
                }
                else if (TempType0.SubType == 16)
                {
                    Type0Sub16 type0Sub16 = new Type0Sub16();

                    type0Sub16.U0 = StreamUtil.ReadUInt32(stream);
                    type0Sub16.U1 = StreamUtil.ReadUInt32(stream);
                    type0Sub16.U2 = StreamUtil.ReadUInt32(stream);
                    type0Sub16.U3 = StreamUtil.ReadUInt32(stream);
                    type0Sub16.U4 = StreamUtil.ReadUInt32(stream);
                    type0Sub16.U5 = StreamUtil.ReadUInt32(stream);
                    type0Sub16.U6 = StreamUtil.ReadUInt32(stream);
                    type0Sub16.U7 = StreamUtil.ReadUInt32(stream);
                    type0Sub16.U8 = StreamUtil.ReadUInt32(stream);
                    type0Sub16.U9 = StreamUtil.ReadUInt32(stream);

                    TempType0.type0Sub16 = type0Sub16;
                }
                else if (TempType0.SubType == 17)
                {
                    TempType0.type0Sub17 = StreamUtil.ReadFloat(stream);
                }
                else if (TempType0.SubType == 18)
                {
                    Type0Sub18 type0Sub18 = new Type0Sub18();

                    type0Sub18.U0 = StreamUtil.ReadUInt32(stream);
                    type0Sub18.U1 = StreamUtil.ReadUInt32(stream);
                    type0Sub18.U2 = StreamUtil.ReadUInt32(stream);
                    type0Sub18.U3 = StreamUtil.ReadUInt32(stream);
                    type0Sub18.U4 = StreamUtil.ReadUInt32(stream);
                    type0Sub18.U5 = StreamUtil.ReadUInt32(stream);

                    TempType0.type0Sub18 = type0Sub18;
                }
                else if (TempType0.SubType == 19)
                {
                    Type0Sub19 type0Sub19 = new Type0Sub19();

                    type0Sub19.U0 = StreamUtil.ReadUInt32(stream);
                    type0Sub19.U1 = StreamUtil.ReadUInt32(stream);
                    type0Sub19.U2 = StreamUtil.ReadUInt32(stream);
                    type0Sub19.U3 = StreamUtil.ReadUInt32(stream);
                    type0Sub19.U4 = StreamUtil.ReadUInt32(stream);
                    type0Sub19.U5 = StreamUtil.ReadUInt32(stream);
                    type0Sub19.U6 = StreamUtil.ReadUInt32(stream);
                    type0Sub19.U7 = StreamUtil.ReadUInt32(stream);

                    TempType0.type0Sub19 = type0Sub19;
                }
                else if (TempType0.SubType == 20)
                {
                    Type0Sub20 type0Sub20 = new Type0Sub20();

                    type0Sub20.U0 = StreamUtil.ReadUInt32(stream);
                    type0Sub20.U1 = StreamUtil.ReadUInt32(stream);
                    type0Sub20.U2 = StreamUtil.ReadUInt32(stream);
                    type0Sub20.U3 = StreamUtil.ReadUInt32(stream);
                    type0Sub20.U4 = StreamUtil.ReadUInt32(stream);
                    type0Sub20.U5 = StreamUtil.ReadUInt32(stream);
                    type0Sub20.U6 = StreamUtil.ReadUInt32(stream);
                    type0Sub20.U7 = StreamUtil.ReadUInt32(stream);
                    type0Sub20.U8 = StreamUtil.ReadUInt32(stream);

                    TempType0.type0Sub20 = type0Sub20;
                }
                else if (TempType0.SubType == 21)
                {
                    Type0Sub21 type0Sub21 = new Type0Sub21();

                    type0Sub21.U0 = StreamUtil.ReadUInt32(stream);
                    type0Sub21.U1 = StreamUtil.ReadUInt32(stream);
                    type0Sub21.U2 = StreamUtil.ReadFloat(stream);

                    TempType0.type0Sub21 = type0Sub21;
                }
                else if (TempType0.SubType == 22)
                {
                    Type0Sub22 type0Sub22 = new Type0Sub22();
                    type0Sub22.U1 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U2 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U3 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U4 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U5 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U6 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U7 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U8 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U9 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U10 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U11 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U12 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U13 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U14 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U15 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U16 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U17 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U18 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U19 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U20 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U21 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U22 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U23 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U24 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U25 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U26 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U27 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U28 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U29 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U30 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U31 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U32 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U33 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U34 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U35 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U36 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U37 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U38 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U39 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U40 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U41 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U42 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U43 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U44 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U45 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U46 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U47 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U48 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U49 = StreamUtil.ReadUInt32(stream);
                    type0Sub22.U50 = StreamUtil.ReadUInt32(stream);

                    TempType0.type0Sub22 = type0Sub22;
                }
                else if (TempType0.SubType == 256)
                {
                    Type0Sub256 type0Sub256 = new Type0Sub256();

                    type0Sub256.U0 = StreamUtil.ReadUInt32(stream);
                    type0Sub256.U1 = StreamUtil.ReadUInt32(stream);
                    type0Sub256.U2 = StreamUtil.ReadUInt32(stream);
                    type0Sub256.U3 = StreamUtil.ReadUInt32(stream);
                    type0Sub256.U4 = StreamUtil.ReadUInt32(stream);
                    type0Sub256.U5 = StreamUtil.ReadUInt32(stream);
                    type0Sub256.U6 = StreamUtil.ReadUInt32(stream);

                    TempType0.type0Sub256 = type0Sub256;
                }
                else if (TempType0.SubType == 257)
                {
                    Type0Sub257 type0Sub257 = new Type0Sub257();

                    type0Sub257.U0 = StreamUtil.ReadUInt32(stream);
                    type0Sub257.U1 = StreamUtil.ReadUInt32(stream);
                    type0Sub257.U2 = StreamUtil.ReadUInt32(stream);
                    type0Sub257.U3 = StreamUtil.ReadUInt32(stream);
                    type0Sub257.U4 = StreamUtil.ReadUInt32(stream);
                    type0Sub257.U5 = StreamUtil.ReadUInt32(stream);
                    type0Sub257.U6 = StreamUtil.ReadUInt32(stream);

                    TempType0.type0Sub257 = type0Sub257;
                }
                else if (TempType0.SubType == 258)
                {
                    Type0Sub258 type0Sub258 = new Type0Sub258();

                    type0Sub258.U0 = StreamUtil.ReadUInt32(stream);
                    type0Sub258.U1 = StreamUtil.ReadUInt32(stream);
                    type0Sub258.U2 = StreamUtil.ReadUInt32(stream);
                    type0Sub258.U3 = StreamUtil.ReadUInt32(stream);
                    type0Sub258.U4 = StreamUtil.ReadUInt32(stream);
                    type0Sub258.U5 = StreamUtil.ReadUInt32(stream);
                    type0Sub258.U6 = StreamUtil.ReadUInt32(stream);
                    type0Sub258.U7 = StreamUtil.ReadUInt32(stream);
                    type0Sub258.U8 = StreamUtil.ReadUInt32(stream);
                    type0Sub258.U9 = StreamUtil.ReadUInt32(stream);
                    type0Sub258.U10 = StreamUtil.ReadUInt32(stream);

                    TempType0.type0Sub258 = type0Sub258;
                }
                else if (TempType0.SubType == 259)
                {
                    Type0Sub259 type0Sub259 = new Type0Sub259();

                    type0Sub259.U0 = StreamUtil.ReadUInt32(stream);
                    type0Sub259.U1 = StreamUtil.ReadUInt32(stream);
                    type0Sub259.U2 = StreamUtil.ReadUInt32(stream);
                    type0Sub259.U3 = StreamUtil.ReadUInt32(stream);
                    type0Sub259.U4 = StreamUtil.ReadUInt32(stream);
                    type0Sub259.U5 = StreamUtil.ReadUInt32(stream);
                    type0Sub259.U6 = StreamUtil.ReadUInt32(stream);
                    type0Sub259.U7 = StreamUtil.ReadUInt32(stream);
                    type0Sub259.U8 = StreamUtil.ReadUInt32(stream);

                    TempType0.type0Sub259 = type0Sub259;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Missing Type " + TempEffect.Type.ToString() + "," + TempType0.SubType.ToString());
                }

                TempEffect.type0 = TempType0;
            }
            else if (TempEffect.Type == 1)
            {
                Type1 TempType1 = new Type1();

                TempType1.SubType = StreamUtil.ReadUInt32(stream);

                if (TempType1.SubType == 0)
                {
                    Type1Sub0 type1Sub0 = new Type1Sub0();
                    type1Sub0.U1 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U2 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U3 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U4 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U5 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U6 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U7 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U8 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U9 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U10 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U11 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U12 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U13 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U14 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U15 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U16 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U17 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U18 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U19 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U20 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U21 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U22 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U23 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U24 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U25 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U26 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U27 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U28 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U29 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U30 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U31 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U32 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U33 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U34 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U35 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U36 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U37 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U38 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U39 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U40 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U41 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U42 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U43 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U44 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U45 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U46 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U47 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U48 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U49 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U50 = StreamUtil.ReadUInt32(stream);
                    type1Sub0.U51 = StreamUtil.ReadUInt32(stream);

                    TempType1.type1Sub0 = type1Sub0;
                }
                else if (TempType1.SubType == 1)
                {
                    Type1Sub1 type1Sub1 = new Type1Sub1();

                    type1Sub1.U0 = StreamUtil.ReadUInt32(stream);
                    type1Sub1.U1 = StreamUtil.ReadUInt32(stream);
                    type1Sub1.U2 = StreamUtil.ReadUInt32(stream);
                    type1Sub1.U3 = StreamUtil.ReadUInt32(stream);
                    type1Sub1.U4 = StreamUtil.ReadUInt32(stream);
                    type1Sub1.U5 = StreamUtil.ReadUInt32(stream);
                    type1Sub1.U6 = StreamUtil.ReadUInt32(stream);
                    type1Sub1.U7 = StreamUtil.ReadUInt32(stream);
                    type1Sub1.U8 = StreamUtil.ReadUInt32(stream);
                    type1Sub1.U9 = StreamUtil.ReadUInt32(stream);
                    type1Sub1.U10 = StreamUtil.ReadUInt32(stream);

                    TempType1.type1Sub1 = type1Sub1;
                }
                else if (TempType1.SubType == 2)
                {
                    Type1Sub2 type1Sub2 = new Type1Sub2();

                    type1Sub2.U0 = StreamUtil.ReadUInt32(stream);
                    type1Sub2.U1 = StreamUtil.ReadUInt32(stream);
                    type1Sub2.U2 = StreamUtil.ReadUInt32(stream);
                    type1Sub2.U3 = StreamUtil.ReadUInt32(stream);
                    type1Sub2.U4 = StreamUtil.ReadUInt32(stream);
                    type1Sub2.U5 = StreamUtil.ReadUInt32(stream);
                    type1Sub2.U6 = StreamUtil.ReadUInt32(stream);
                    type1Sub2.U7 = StreamUtil.ReadUInt32(stream);
                    type1Sub2.U8 = StreamUtil.ReadUInt32(stream);
                    type1Sub2.U9 = StreamUtil.ReadUInt32(stream);
                    type1Sub2.U10 = StreamUtil.ReadUInt32(stream);
                    type1Sub2.U11 = StreamUtil.ReadUInt32(stream);

                    TempType1.type1Sub2 = type1Sub2;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Missing Type " + TempEffect.Type.ToString() + "," + TempType1.SubType.ToString());
                }

                TempEffect.type1 = TempType1;
            }
            else if (TempEffect.Type == 2)
            {
                Type2 type2 = new Type2();

                type2.U0 = StreamUtil.ReadUInt32(stream);
                type2.U1 = StreamUtil.ReadFloat(stream);

                TempEffect.type2 = type2;
            }
            else if (TempEffect.Type == 3)
            {
                TempEffect.type3 = StreamUtil.ReadFloat(stream);
            }
            else if (TempEffect.Type == 4)
            {
                Type4 type4 = new Type4();

                type4.U0 = StreamUtil.ReadUInt32(stream);
                type4.U1 = StreamUtil.ReadUInt32(stream);
                type4.U2 = StreamUtil.ReadUInt32(stream);

                TempEffect.type4 = type4;
            }
            else if (TempEffect.Type == 6)
            {
                Type6 TempType6 = new Type6();

                TempType6.U0 = StreamUtil.ReadUInt32(stream);
                TempType6.U1 = StreamUtil.ReadUInt32(stream);
                TempType6.U2 = StreamUtil.ReadUInt32(stream);

                TempEffect.type6 = TempType6;
            }
            else if (TempEffect.Type == 7)
            {
                TempEffect.type7 = StreamUtil.ReadUInt32(stream);
            }
            else if (TempEffect.Type == 8)
            {
                Type8 TempType8 = new Type8();

                TempType8.U0 = StreamUtil.ReadUInt32(stream);
                TempType8.U1 = StreamUtil.ReadFloat(stream);

                TempEffect.type8 = TempType8;
            }
            else if (TempEffect.Type == 9)
            {
                TempEffect.type9 = StreamUtil.ReadUInt32(stream);
            }
            else if (TempEffect.Type == 12)
            {
                TempEffect.type12 = StreamUtil.ReadUInt32(stream);
            }
            else if (TempEffect.Type == 13)
            {
                TempEffect.type13 = StreamUtil.ReadFloat(stream);
            }
            else if (TempEffect.Type == 16)
            {
                TempEffect.type16 = StreamUtil.ReadFloat(stream);
            }
            else if (TempEffect.Type == 17)
            {
                TempEffect.type17 = StreamUtil.ReadFloat(stream);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Missing Type " + TempEffect.Type.ToString());
            }

            return TempEffect;
        }

        public struct EffectSlot
        {
            public int Slot1;
            public int Slot2;
            public int Slot3;
            public int Slot4;
            public int Slot5;
            public int Slot6;
            public int Slot7;
        }

        public struct UStruct1
        {
            public int Offset;
            public int ByteSize;
            public int UStruct3Count;

            public List<UStruct3> uStruct3s;
        }
        public struct CollisonModelPointer
        {
            public int Offset;
            public int ByteSize;
            public int Count;

            public List<CollisonModel> Models;
        }

        public struct CollisonModel
        {
            public string MeshPath;

            public int FaceCount;
            public int VerticeCount;
            public int VerticeOffsetAlign;

            public List<int> Index;
            public List<Vector4> Vertices;
            public List<Vector4> FaceNormals; //Face Count
        }

        public struct UStruct3
        {
            public int EndAlligment; //Ending Allignment
            public int U1;
            public int U2;
            public int UStruct4Count; //+1

            public int U4;
            public float U5;
            public float U6;
            public float U7;

            public float U8;
            public float U9;
            public float U10;
            public float U11;

            public float U12;
            public float U13;
            public float U14;
            public float U15;

            public float U16;
            public float U17;
            public float U18;
            public float U19;

            public float U20;
            public float U21;
            public float U22;
            public float U23;

            public float U24;
            public float U25;
            public float U26;
            public float U27;

            public List<UStruct4> struct4s;

            public byte[] UByteData;
            public byte[] UEndData;
        }

        public struct UStruct4
        {
            public float U0;
            public float U1;
            public int U2;
        }

        #region Effects
        //Effect
        public struct EffectHeader
        {
            public int Count;
            public int Offset;

            public List<Effect> Effects;
        }

        public struct Effect
        {
            //16
            public int Type;
            public int Size;

            public Type0? type0;
            public Type1? type1;
            public Type2? type2;
            public float type3;
            public Type4? type4;
            public Type6? type6;
            public int type7;
            public Type8? type8;
            public int type9;
            public int type12;
            public float type13;
            public float type16;
            public float type17;
        }

        public struct Type0
        {
            public int SubType;

            public Type0Sub0? type0Sub0;
            public float type0Sub2;
            public int type0Sub5;
            public Type0Sub6? type0Sub6;
            public Type0Sub7? type0Sub7;
            public Type0Sub10? type0Sub10;
            public Type0Sub11? type0Sub11;
            public Type0Sub12? type0Sub12;
            public Type0Sub13? type0Sub13;
            public Type0Sub14? type0Sub14;
            public Type0Sub15? type0Sub15;
            public Type0Sub16? type0Sub16;
            public float type0Sub17;
            public Type0Sub18? type0Sub18;
            public Type0Sub19? type0Sub19;
            public Type0Sub20? type0Sub20;
            public Type0Sub21? type0Sub21;
            public Type0Sub22? type0Sub22;
            public Type0Sub256? type0Sub256;
            public Type0Sub257? type0Sub257;
            public Type0Sub258? type0Sub258;
            public Type0Sub259? type0Sub259;
        }

        public struct Type0Sub0
        {
            public float U1;
            public float U2;
            public float U3;
            public float U4;
            public int U5;
            public int U6;
        }

        public struct Type0Sub6
        {
            public int U0;
            public float U1;

        }

        public struct Type0Sub7
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
        }

        public struct Type0Sub10
        {
            public float U1;
            public float U2;
            public float U3;
            public float U4;
            public int U5;
            public int U6;
        }

        public struct Type0Sub11
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
        }

        public struct Type0Sub12
        {
            public int U0;
            public float U1;
        }

        public struct Type0Sub13
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
        }

        public struct Type0Sub14
        {
            public int U0;
            public float U1;
        }

        public struct Type0Sub15
        {
            public float U0;
            public float U1;
            public float U2;
            public float U3;
            public float U4;
        }

        public struct Type0Sub16
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
        }

        public struct Type0Sub18
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
        }

        public struct Type0Sub19
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
            public int U7;
        }

        public struct Type0Sub20
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
        }

        public struct Type0Sub21
        {
            public int U0;
            public int U1;
            public float U2;
        }

        public struct Type0Sub22
        {
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
            public int U11;
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
            public int U28;
            public int U29;
            public int U30;
            public int U31;
            public int U32;
            public int U33;
            public int U34;
            public int U35;
            public int U36;
            public int U37;
            public int U38;
            public int U39;
            public int U40;
            public int U41;
            public int U42;
            public int U43;
            public int U44;
            public int U45;
            public int U46;
            public int U47;
            public int U48;
            public int U49;
            public int U50;
        }

        public struct Type0Sub256
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
        }

        public struct Type0Sub257
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
        }

        public struct Type0Sub258
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
        }

        public struct Type0Sub259
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
        }

        public struct Type1
        {
            public int SubType;
            public Type1Sub0? type1Sub0;
            public Type1Sub1? type1Sub1;
            public Type1Sub2? type1Sub2;
        }

        public struct Type1Sub0
        {
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
            public int U11;
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
            public int U28;
            public int U29;
            public int U30;
            public int U31;
            public int U32;
            public int U33;
            public int U34;
            public int U35;
            public int U36;
            public int U37;
            public int U38;
            public int U39;
            public int U40;
            public int U41;
            public int U42;
            public int U43;
            public int U44;
            public int U45;
            public int U46;
            public int U47;
            public int U48;
            public int U49;
            public int U50;
            public int U51;
        }

        public struct Type1Sub1
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
        }

        public struct Type1Sub2
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
            public int U11;
        }

        public struct Type2
        {
            public int U0;
            public float U1;
        }

        public struct Type4
        {
            public int U0;
            public int U1;
            public int U2;
        }

        public struct Type6
        {
            public int U0;
            public int U1;
            public int U2;
        }

        public struct Type8
        {
            public int U0;
            public float U1;
        }

        #endregion
    }
}
