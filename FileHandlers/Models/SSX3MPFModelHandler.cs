using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SSXMultiTool.FileHandlers
{
    internal class SSX3MPFModelHandler
    {
        public byte[] magicWords = new byte[4];
        public int NumModels;
        public int HeaderSize;
        public int DataOffset;
        public List<MPFModelHeader> ModelList = new List<MPFModelHeader>();

        public void load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                magicWords = StreamUtil.ReadBytes(stream, 4);
                NumModels = StreamUtil.ReadInt16(stream);
                HeaderSize = StreamUtil.ReadInt16(stream);
                DataOffset = StreamUtil.ReadInt32(stream);
                for (int i = 0; i < NumModels; i++)
                {
                    MPFModelHeader modelHeader = new MPFModelHeader();

                    modelHeader.ModelName = StreamUtil.ReadString(stream, 16).Replace("\0", "");
                    modelHeader.DataOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.EntrySize = StreamUtil.ReadInt32(stream);
                    modelHeader.BoneOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.U7 = StreamUtil.ReadInt32(stream);
                    modelHeader.U8 = StreamUtil.ReadInt32(stream);
                    modelHeader.MeshDataOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.MaterialOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.MorphIDOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.WeightRefrenceOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.BoneWeightOffset = StreamUtil.ReadInt32(stream);
                    modelHeader.U14 = StreamUtil.ReadInt32(stream);
                    modelHeader.U15 = StreamUtil.ReadInt32(stream);
                    modelHeader.U16 = StreamUtil.ReadInt32(stream);

                    modelHeader.WeightCount = StreamUtil.ReadInt16(stream);
                    modelHeader.WeightRefrenceCount = StreamUtil.ReadInt16(stream);
                    modelHeader.U19 = StreamUtil.ReadInt16(stream);
                    modelHeader.BoneCount = StreamUtil.ReadInt16(stream);
                    modelHeader.MaterialCount = StreamUtil.ReadInt16(stream);
                    modelHeader.U22 = StreamUtil.ReadInt16(stream);
                    modelHeader.MorphKeyCount = StreamUtil.ReadInt16(stream);
                    modelHeader.FileID = StreamUtil.ReadInt16(stream);
                    modelHeader.U25 = StreamUtil.ReadInt16(stream);
                    modelHeader.U26 = StreamUtil.ReadInt16(stream);
                    modelHeader.U27 = StreamUtil.ReadInt16(stream);
                    modelHeader.U28 = StreamUtil.ReadInt16(stream);
                    modelHeader.U29 = StreamUtil.ReadInt16(stream);
                    modelHeader.U30 = StreamUtil.ReadInt16(stream);

                    ModelList.Add(modelHeader);
                }

                //Read Matrix And Decompress
                int StartPos = DataOffset;
                for (int i = 0; i < ModelList.Count; i++)
                {
                    stream.Position = StartPos + ModelList[i].DataOffset;

                    MPFModelHeader modelHandler = ModelList[i];
                    modelHandler.Matrix = StreamUtil.ReadBytes(stream, ModelList[i].EntrySize);
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

                    //Bone
                    streamMatrix.Position = TempModel.BoneOffset;
                    TempModel.BoneList = new List<BoneData>();
                    for (int a = 0; a < TempModel.BoneCount; a++)
                    {
                        var TempBoneData = new BoneData();
                        TempBoneData.BoneName = StreamUtil.ReadString(streamMatrix, 16);
                        TempBoneData.ParentFileID = StreamUtil.ReadInt16(streamMatrix);
                        TempBoneData.ParentBone = StreamUtil.ReadInt16(streamMatrix);
                        TempBoneData.Unknown2 = StreamUtil.ReadInt16(streamMatrix);
                        TempBoneData.BoneID = StreamUtil.ReadInt16(streamMatrix);
                        TempBoneData.Position = StreamUtil.ReadVector3(streamMatrix);
                        TempBoneData.Radians = StreamUtil.ReadVector3(streamMatrix);
                        TempBoneData.XRadian2 = StreamUtil.ReadFloat(streamMatrix);
                        TempBoneData.YRadian2 = StreamUtil.ReadFloat(streamMatrix);
                        TempBoneData.ZRadian2 = StreamUtil.ReadFloat(streamMatrix);

                        TempBoneData.UnknownFloat1 = StreamUtil.ReadFloat(streamMatrix);
                        TempBoneData.UnknownFloat2 = StreamUtil.ReadFloat(streamMatrix);
                        TempBoneData.UnknownFloat3 = StreamUtil.ReadFloat(streamMatrix);
                        TempBoneData.UnknownFloat4 = StreamUtil.ReadFloat(streamMatrix);
                        TempBoneData.UnknownFloat5 = StreamUtil.ReadFloat(streamMatrix);

                        TempBoneData.FileID = TempModel.FileID;
                        TempBoneData.BonePos = a;

                        TempModel.BoneList.Add(TempBoneData);
                    }

                    //Morph Data
                    streamMatrix.Position = TempModel.MorphIDOffset;
                    TempModel.MorphKeyIDList = new List<int>();
                    for (int a = 0; a < TempModel.MorphKeyCount; a++)
                    {
                        TempModel.MorphKeyIDList.Add(StreamUtil.ReadInt32(streamMatrix));
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


                    ModelList[i] = TempModel;
                }
            }
        }


        public void Save(string path)
        {
            Stream stream = new MemoryStream();
            StreamUtil.WriteBytes(stream, magicWords);
            StreamUtil.WriteInt16(stream, NumModels);
            StreamUtil.WriteInt16(stream, HeaderSize);
            StreamUtil.WriteInt32(stream, DataOffset);

            for (int i = 0; i < 1; i++)
            {
                StreamUtil.WriteString(stream, ModelList[i].ModelName, 16);

                StreamUtil.WriteInt32(stream, ModelList[i].DataOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].EntrySize);
                StreamUtil.WriteInt32(stream, ModelList[i].BoneOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].U7);
                StreamUtil.WriteInt32(stream, ModelList[i].U8);
                StreamUtil.WriteInt32(stream, ModelList[i].MeshDataOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].MaterialOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].MorphIDOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].WeightRefrenceOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].BoneWeightOffset);
                StreamUtil.WriteInt32(stream, ModelList[i].U14);
                StreamUtil.WriteInt32(stream, ModelList[i].U15);
                StreamUtil.WriteInt32(stream, ModelList[i].U16);

                StreamUtil.WriteInt16(stream, ModelList[i].WeightCount);
                StreamUtil.WriteInt16(stream, ModelList[i].WeightRefrenceCount);
                StreamUtil.WriteInt16(stream, ModelList[i].U19);
                StreamUtil.WriteInt16(stream, ModelList[i].BoneCount);
                StreamUtil.WriteInt16(stream, ModelList[i].MaterialCount);
                StreamUtil.WriteInt16(stream, ModelList[i].U22);
                StreamUtil.WriteInt16(stream, ModelList[i].MorphKeyCount);
                StreamUtil.WriteInt16(stream, ModelList[i].FileID);
                StreamUtil.WriteInt16(stream, ModelList[i].U25);
                StreamUtil.WriteInt16(stream, ModelList[i].U26);
                StreamUtil.WriteInt16(stream, ModelList[i].U27);
                StreamUtil.WriteInt16(stream, ModelList[i].U28);
                StreamUtil.WriteInt16(stream, ModelList[i].U29);
                StreamUtil.WriteInt16(stream, ModelList[i].U30);
            }
            StreamUtil.AlignBy16(stream);


            for (int i = 0; i < 1; i++)
            {
                //Save current pos go back and set start pos
                stream.Position = DataOffset + ModelList[i].DataOffset;
                //Write Matrix
                StreamUtil.WriteBytes(stream, ModelList[i].Matrix);
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


        public struct MPFModelHeader
        {
            //Main Header
            public string ModelName;
            public int DataOffset;
            public int EntrySize;
            public int BoneOffset; 
            public int U7; //Weight Info (IK Points?)
            public int U8; //Material Groups
            public int MeshDataOffset;
            public int MaterialOffset;
            public int MorphIDOffset;
            public int WeightRefrenceOffset; //Weight Refrence List
            public int BoneWeightOffset; //Weight Info

            //Unused ??
            public int U14;
            public int U15; 
            public int U16;

            //Counts
            public int WeightCount; //Weight
            public int WeightRefrenceCount; //Weight Ref??
            public int U19; //Material Group??
            public int BoneCount;
            public int MaterialCount;
            public int U22; //IK Point Count??
            public int MorphKeyCount;
            public int FileID;
            public int U25; //Possibly Some Kind of Face Ammount

            //Unused?
            public int U26;
            public int U27;
            public int U28;
            public int U29;
            public int U30;

            public List<MaterialData> MaterialList;
            public List<BoneData> BoneList;
            public List<int> MorphKeyIDList;
            public List<BoneWeightHeader> BoneWeightHeaderList;
            public List<WeightRefList> WeightRefrenceLists;

            public byte[] Matrix;
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

        public struct BoneData
        {
            public string BoneName;
            public int ParentFileID;
            public int ParentBone;
            public int Unknown2;
            public int BoneID;

            public Vector3 Position;
            public Vector3 Radians;

            public float XRadian2;
            public float YRadian2;
            public float ZRadian2;

            public float UnknownFloat1;
            public float UnknownFloat2;
            public float UnknownFloat3;
            public float UnknownFloat4;
            public float UnknownFloat5;

            public int FileID;
            public int BonePos;

            public string parentName;
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
