using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers.Models
{
    public class SSXOnTourMPF
    {
        public byte[] magicWords = new byte[4];
        public int NumModels;
        public int HeaderSize;
        public int DataOffset;
        public List<MPFHeader> ModelList = new List<MPFHeader>();

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                magicWords = StreamUtil.ReadBytes(stream, 4);
                NumModels = StreamUtil.ReadInt16(stream);
                HeaderSize = StreamUtil.ReadInt16(stream);
                DataOffset = StreamUtil.ReadInt32(stream);

                ModelList = new List<MPFHeader>();
                for (int i = 0; i < NumModels; i++)
                {
                    var TempHeader = new MPFHeader();
                    TempHeader.ModelName = StreamUtil.ReadString(stream, 16);
                    TempHeader.DataOffset = StreamUtil.ReadInt32(stream);
                    TempHeader.EntrySize = StreamUtil.ReadInt32(stream);
                    TempHeader.U1 = StreamUtil.ReadInt32(stream);
                    TempHeader.BoneOffset = StreamUtil.ReadInt32(stream);
                    TempHeader.IKPointOffset = StreamUtil.ReadInt32(stream);
                    TempHeader.U4 = StreamUtil.ReadInt32(stream);
                    TempHeader.U5 = StreamUtil.ReadInt32(stream);
                    TempHeader.MaterialOffset = StreamUtil.ReadInt32(stream);
                    TempHeader.MorphOffset = StreamUtil.ReadInt32(stream);
                    TempHeader.U8 = StreamUtil.ReadInt32(stream);
                    TempHeader.WeightRefrenceOffset = StreamUtil.ReadInt32(stream);
                    TempHeader.BoneWeightOffset = StreamUtil.ReadInt32(stream);

                    TempHeader.U11 = StreamUtil.ReadInt32(stream);
                    TempHeader.U12 = StreamUtil.ReadInt32(stream);
                    TempHeader.U13 = StreamUtil.ReadInt32(stream);
                    TempHeader.U14 = StreamUtil.ReadInt32(stream);

                    TempHeader.WeightCount = StreamUtil.ReadInt16(stream);
                    TempHeader.WeightRefrenceCount = StreamUtil.ReadInt16(stream);
                    TempHeader.UC3 = StreamUtil.ReadInt16(stream);
                    TempHeader.BoneCount = StreamUtil.ReadInt16(stream);
                    TempHeader.MaterialCount = StreamUtil.ReadInt16(stream);
                    TempHeader.UC6 = StreamUtil.ReadInt16(stream);
                    TempHeader.MorphCount = StreamUtil.ReadInt16(stream);
                    TempHeader.UC8 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC9 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC10 = StreamUtil.ReadInt16(stream);
                    TempHeader.FileID = StreamUtil.ReadInt16(stream);

                    TempHeader.UC12 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC13 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC14 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC15 = StreamUtil.ReadInt16(stream);
                    TempHeader.UC16 = StreamUtil.ReadInt16(stream);


                    ModelList.Add(TempHeader);
                }


                //Read Matrix And Decompress
                int StartPos = DataOffset;
                for (int i = 0; i < ModelList.Count; i++)
                {
                    stream.Position = StartPos + ModelList[i].DataOffset;
                    int EntrySize = 0;

                    if(i==ModelList.Count-1)
                    {
                        EntrySize = (int)((stream.Length- DataOffset) - ModelList[i].DataOffset);
                    }
                    else
                    {
                        EntrySize = ModelList[i + 1].DataOffset - ModelList[i].DataOffset;
                    }


                    MPFHeader modelHandler = ModelList[i];
                    modelHandler.Matrix = StreamUtil.ReadBytes(stream, EntrySize);
                    modelHandler.Matrix = RefpackHandler.Decompress(modelHandler.Matrix);
                    ModelList[i] = modelHandler;
                }



                for (int i = 0; i < ModelList.Count; i++)
                {
                    var TempModel = ModelList[i];
                    Stream streamMatrix = new MemoryStream();
                    streamMatrix.Write(TempModel.Matrix, 0, TempModel.Matrix.Length);
                    streamMatrix.Position = 0;

                    //Material
                    streamMatrix.Position = TempModel.MaterialOffset;
                    TempModel.MaterialList = new List<MaterialData>();
                    for (int a = 0; a < TempModel.MaterialCount; a++)
                    {
                        var TempMat = new MaterialData();
                        TempMat.MainTexture = StreamUtil.ReadString(streamMatrix, 4);
                        if (streamMatrix.ReadByte() != 0x00)
                        {
                            streamMatrix.Position -= 1;
                            TempMat.Texture1 = StreamUtil.ReadString(streamMatrix, 4);
                        }
                        else
                        {
                            streamMatrix.Position += 3;
                            TempMat.Texture1 = "";
                        }

                        if (streamMatrix.ReadByte() != 0x00)
                        {
                            streamMatrix.Position -= 1;
                            TempMat.Texture2 = StreamUtil.ReadString(streamMatrix, 4);
                        }
                        else
                        {
                            streamMatrix.Position += 3;
                            TempMat.Texture2 = "";
                        }

                        if (streamMatrix.ReadByte() != 0x00)
                        {
                            streamMatrix.Position -= 1;
                            TempMat.Texture3 = StreamUtil.ReadString(streamMatrix, 4);
                        }
                        else
                        {
                            streamMatrix.Position += 3;
                            TempMat.Texture3 = "";
                        }

                        if (streamMatrix.ReadByte() != 0x00)
                        {
                            streamMatrix.Position -= 1;
                            TempMat.Texture4 = StreamUtil.ReadString(streamMatrix, 4);
                        }
                        else
                        {
                            streamMatrix.Position += 3;
                            TempMat.Texture4 = "";
                        }

                        TempMat.FactorFloat = StreamUtil.ReadFloat(streamMatrix);
                        TempMat.Unused1Float = StreamUtil.ReadFloat(streamMatrix);
                        TempMat.Unused2Float = StreamUtil.ReadFloat(streamMatrix);
                        TempModel.MaterialList.Add(TempMat);
                    }

                    //Bone Data
                    streamMatrix.Position = TempModel.BoneOffset;
                    TempModel.BoneList = new List<BoneData>();
                    for (int a = 0; a < TempModel.BoneCount; a++)
                    {
                        var TempBoneData = new BoneData();
                        TempBoneData.BoneName = StreamUtil.ReadString(streamMatrix, 16);
                        TempBoneData.ParentFileID = StreamUtil.ReadInt16(streamMatrix);
                        TempBoneData.ParentBone = StreamUtil.ReadInt16(streamMatrix);
                        TempBoneData.Unknown1 = StreamUtil.ReadInt16(streamMatrix);
                        TempBoneData.BoneID = StreamUtil.ReadInt16(streamMatrix);

                        TempBoneData.Unknown2 = StreamUtil.ReadInt8(streamMatrix);
                        TempBoneData.Unknown3 = StreamUtil.ReadInt8(streamMatrix);
                        TempBoneData.Unknown4 = StreamUtil.ReadInt8(streamMatrix);
                        TempBoneData.Unknown5 = StreamUtil.ReadInt8(streamMatrix);

                        TempBoneData.Unknown6 = StreamUtil.ReadInt32(streamMatrix);

                        TempBoneData.Position = StreamUtil.ReadVector4(streamMatrix);
                        TempBoneData.Rotation = StreamUtil.ReadQuaternion(streamMatrix);
                        TempBoneData.Unknown = StreamUtil.ReadVector4(streamMatrix);

                        TempBoneData.FileID = TempModel.FileID;
                        TempBoneData.BonePos = a;

                        TempModel.BoneList.Add(TempBoneData);
                    }

                    //Standard Morph Header
                    streamMatrix.Position = TempModel.MorphOffset;
                    TempModel.MorphHeaderList = new List<MorphHeader>();
                    for (int a = 0; a < TempModel.MorphCount; a++)
                    {
                        var TempMorph = new MorphHeader();
                        TempMorph.MorphName = StreamUtil.ReadString(streamMatrix, 28);
                        TempMorph.MorphID = StreamUtil.ReadInt32(streamMatrix);
                        TempModel.MorphHeaderList.Add(TempMorph);
                    }

                    //Bone Weight Info 
                    streamMatrix.Position = TempModel.BoneWeightOffset;
                    TempModel.BoneWeightHeaderList = new List<BoneWeightHeader>();
                    for (int b = 0; b < TempModel.WeightCount; b++)
                    {
                        var BoneWeight = new BoneWeightHeader();

                        BoneWeight.WeightCount = StreamUtil.ReadInt32(streamMatrix);
                        BoneWeight.WeightOffset = StreamUtil.ReadInt32(streamMatrix);
                        BoneWeight.Unknown = StreamUtil.ReadInt32(streamMatrix);
                        BoneWeight.BoneWeightList = new List<BoneWeight>();
                        int TempPos = (int)streamMatrix.Position;
                        streamMatrix.Position = BoneWeight.WeightOffset;
                        for (int a = 0; a < BoneWeight.WeightCount; a++)
                        {
                            var boneWeight = new BoneWeight();
                            boneWeight.Weight = StreamUtil.ReadInt16(streamMatrix);
                            boneWeight.BoneID = StreamUtil.ReadUInt8(streamMatrix);
                            boneWeight.FileID = StreamUtil.ReadUInt8(streamMatrix);
                            BoneWeight.BoneWeightList.Add(boneWeight);
                        }
                        streamMatrix.Position = TempPos;
                        TempModel.BoneWeightHeaderList.Add(BoneWeight);
                    }

                    //Weight Refrence List
                    streamMatrix.Position = TempModel.WeightRefrenceOffset;
                    TempModel.WeightRefrenceLists = new List<WeightRefList>();
                    for (int b = 0; b < TempModel.WeightRefrenceCount; b++)
                    {
                        var NumberListRef = new WeightRefList();
                        NumberListRef.ListCount = StreamUtil.ReadInt32(streamMatrix);
                        NumberListRef.Offset = StreamUtil.ReadInt32(streamMatrix);
                        NumberListRef.WeightIDs = new List<int>();

                        int TempPos = (int)streamMatrix.Position;
                        streamMatrix.Position = NumberListRef.Offset;
                        for (int c = 0; c < NumberListRef.ListCount; c++)
                        {
                            NumberListRef.WeightIDs.Add(StreamUtil.ReadInt32(streamMatrix));
                        }
                        streamMatrix.Position = TempPos;
                        TempModel.WeightRefrenceLists.Add(NumberListRef);
                    }


                    streamMatrix.Close();
                    streamMatrix.Dispose();
                    ModelList[i] = TempModel;
                }
            }


        }

        public void SaveDecompress(string Path)
        {
            MemoryStream stream = new MemoryStream();

            StreamUtil.WriteBytes(stream, magicWords);
            StreamUtil.WriteInt16(stream, ModelList.Count);
            StreamUtil.WriteInt16(stream, 12);
            StreamUtil.WriteInt32(stream, DataOffset); //FIX OFFSET NUMBER

            for (int i = 0; i < ModelList.Count; i++)
            {
                var TempModel = ModelList[i];

                StreamUtil.WriteString(stream, TempModel.ModelName, 16);

                StreamUtil.WriteInt32(stream, TempModel.DataOffset);
                StreamUtil.WriteInt32(stream, TempModel.EntrySize);
                StreamUtil.WriteInt32(stream, TempModel.U1);
                StreamUtil.WriteInt32(stream, TempModel.BoneOffset);
                StreamUtil.WriteInt32(stream, TempModel.IKPointOffset);
                StreamUtil.WriteInt32(stream, TempModel.U4);
                StreamUtil.WriteInt32(stream, TempModel.U5);
                StreamUtil.WriteInt32(stream, TempModel.MaterialOffset);
                StreamUtil.WriteInt32(stream, TempModel.MorphOffset);
                StreamUtil.WriteInt32(stream, TempModel.U8);
                StreamUtil.WriteInt32(stream, TempModel.WeightRefrenceOffset);
                StreamUtil.WriteInt32(stream, TempModel.BoneWeightOffset);
                StreamUtil.WriteInt32(stream, TempModel.U11);
                StreamUtil.WriteInt32(stream, TempModel.U12);
                StreamUtil.WriteInt32(stream, TempModel.U13);
                StreamUtil.WriteInt32(stream, TempModel.U14);

                StreamUtil.WriteInt16(stream, TempModel.WeightCount);
                StreamUtil.WriteInt16(stream, TempModel.WeightRefrenceCount);
                StreamUtil.WriteInt16(stream, TempModel.UC3);
                StreamUtil.WriteInt16(stream, TempModel.BoneCount);
                StreamUtil.WriteInt16(stream, TempModel.MaterialCount);
                StreamUtil.WriteInt16(stream, TempModel.UC6);
                StreamUtil.WriteInt16(stream, TempModel.MorphCount);
                StreamUtil.WriteInt16(stream, TempModel.UC8);
                StreamUtil.WriteInt16(stream, TempModel.UC9);
                StreamUtil.WriteInt16(stream, TempModel.UC10);
                StreamUtil.WriteInt16(stream, TempModel.FileID);
                StreamUtil.WriteInt16(stream, TempModel.UC12);
                StreamUtil.WriteInt16(stream, TempModel.UC13);
                StreamUtil.WriteInt16(stream, TempModel.UC14);
                StreamUtil.WriteInt16(stream, TempModel.UC15);
                StreamUtil.WriteInt16(stream, TempModel.UC16);
            }

            StreamUtil.AlignBy16(stream);

            for (int i = 0; i < ModelList.Count; i++)
            {
                StreamUtil.WriteBytes(stream, ModelList[i].Matrix);
                StreamUtil.AlignBy16(stream);
            }


            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
            var file = File.Create(Path);
            stream.Position = 0;
            stream.CopyTo(file);
            stream.Dispose();
            file.Close();
        }

        public struct MPFHeader
        {
            //Main Header Offsets
            public string ModelName;
            public int DataOffset;
            public int EntrySize;
            public int U1; //Alternative MorphData?
            public int BoneOffset;
            public int IKPointOffset; //IK Points (Unused)
            public int U4; //Material Groups
            public int U5; //Model Data Start
            public int MaterialOffset;
            public int MorphOffset;
            public int U8; //Alternative MorphData Size?
            public int WeightRefrenceOffset;  //Weight Refrence
            public int BoneWeightOffset; //Weights

            //Unused
            public int U11;
            public int U12;
            public int U13;
            public int U14;

            //Header Counts
            public int WeightCount;
            public int WeightRefrenceCount; //
            public int UC3; //
            public int BoneCount; // Bone Count
            public int MaterialCount; // Material Count
            public int UC6; //
            public int MorphCount; // Morph Count
            public int UC8; //
            public int UC9; // Face Count? (Possibly Int32 not Int16)
            public int UC10; // 
            public int FileID; // File ID

            //Unused
            public int UC12;
            public int UC13;
            public int UC14;
            public int UC15;
            public int UC16;

            public byte[] Matrix;

            public List<MaterialData> MaterialList;
            public List<BoneData> BoneList;
            public List<MorphHeader> MorphHeaderList;
            public List<BoneWeightHeader> BoneWeightHeaderList;
            public List<WeightRefList> WeightRefrenceLists;

        }

        public struct MaterialData
        {
            public string MainTexture;
            public string Texture1;
            public string Texture2;
            public string Texture3;
            public string Texture4;

            public float FactorFloat;
            public float Unused1Float;
            public float Unused2Float;
        }

        public struct MorphHeader
        {
            public string MorphName;
            public int MorphID;
        }

        public struct BoneData
        {
            public string BoneName;
            public int ParentFileID;
            public int ParentBone;
            public int Unknown1;
            public int BoneID;

            public int Unknown2;
            public int Unknown3;
            public int Unknown4;
            public int Unknown5;

            public int Unknown6; //Padding

            public Vector4 Position;
            public Quaternion Rotation;
            public Vector4 Unknown;

            public int FileID;
            public int BonePos;
        }

        public struct BoneWeightHeader
        {
            public int WeightCount;
            public int WeightOffset;
            public int Unknown;

            public List<BoneWeight> BoneWeightList;
        }

        public struct BoneWeight
        {
            public int Weight;
            public int BoneID;
            public int FileID;

            public string boneName;
        }

        public struct WeightRefList
        {
            public int ListCount;
            public int Offset;

            public List<int> WeightIDs;
        }
    }
}
