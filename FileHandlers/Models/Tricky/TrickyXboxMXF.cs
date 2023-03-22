using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers.Models.Tricky
{
    public class TrickyXboxMXF
    {
        public byte[] Version;
        public int NumModels;
        public int OffsetModelHeader;
        public int OffsetModelData;

        public List<ModelHeader> modelHeaders = new List<ModelHeader>();

        public void Load(string Path)
        {
            using (Stream stream = File.Open(Path, FileMode.Open))
            {
                Version = StreamUtil.ReadBytes(stream, 4);
                NumModels = StreamUtil.ReadInt16(stream);
                OffsetModelHeader = StreamUtil.ReadInt16(stream);
                OffsetModelData = StreamUtil.ReadUInt32(stream);

                modelHeaders = new List<ModelHeader>();
                for (int i = 0; i < NumModels; i++)
                {
                    var TempModelHeader = new ModelHeader();
                    TempModelHeader.ModelName = StreamUtil.ReadString(stream, 16);
                    TempModelHeader.ModelOffset = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.ModelSize = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.OffsetBoneData = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.OffsetBoneData2 = StreamUtil.ReadUInt32(stream);

                    TempModelHeader.OffsetMateralList = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.OffsetBoneData3 = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.OffsetIKPointList = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.OffsetMorphList = StreamUtil.ReadUInt32(stream);

                    TempModelHeader.OffsetSkinningSection = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.OffsetTristripSection = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.Unknown0 = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.OffsetVertexSection = StreamUtil.ReadUInt32(stream);

                    TempModelHeader.OffsetLastData = StreamUtil.ReadUInt32(stream);
                    TempModelHeader.OffsetUnknownData = StreamUtil.ReadUInt32(stream);

                    stream.Position += 298;

                    TempModelHeader.Unknown1 = StreamUtil.ReadInt16(stream);
                    TempModelHeader.Unknown2 = StreamUtil.ReadInt16(stream);
                    TempModelHeader.NumBones = StreamUtil.ReadInt16(stream);
                    TempModelHeader.NumMorphs = StreamUtil.ReadInt16(stream);
                    TempModelHeader.NumMaterials = StreamUtil.ReadInt16(stream);
                    TempModelHeader.NumIKPoints = StreamUtil.ReadInt16(stream);
                    TempModelHeader.NumSkinningHeaders = StreamUtil.ReadInt16(stream);
                    TempModelHeader.NumTristripGroups = StreamUtil.ReadInt16(stream);
                    TempModelHeader.Unknown3 = StreamUtil.ReadInt16(stream);
                    TempModelHeader.NumVertices = StreamUtil.ReadInt16(stream);
                    TempModelHeader.FileID = StreamUtil.ReadInt16(stream);
                    TempModelHeader.NumLastData = StreamUtil.ReadInt16(stream);
                    TempModelHeader.NumUnknownData = StreamUtil.ReadInt16(stream);

                    modelHeaders.Add(TempModelHeader);
                }

                //Read Data Into Matrix
                int StartPos = OffsetModelData;
                for (int i = 0; i < modelHeaders.Count; i++)
                {
                    stream.Position = StartPos + modelHeaders[i].ModelOffset;
                    ModelHeader modelHandler = modelHeaders[i];
                    modelHandler.Matrix = StreamUtil.ReadBytes(stream, modelHeaders[i].ModelOffset);
                    modelHeaders[i] = modelHandler;
                }

                //Read Matrix
                for (int i = 0; i < modelHeaders.Count; i++)
                {
                    Stream streamMatrix = new MemoryStream();
                    var Model = modelHeaders[i];
                    streamMatrix.Write(modelHeaders[i].Matrix, 0, modelHeaders[i].Matrix.Length);
                    streamMatrix.Position = 0;

                    Model.materialDatas = new List<MaterialData>();
                    for (int a = 0; a < Model.NumMaterials; a++)
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
                        Model.materialDatas.Add(TempMat);
                    }

                    streamMatrix.Position = Model.OffsetBoneData;
                    Model.boneDatas = new List<BoneData>();
                    for (int a = 0; a < Model.NumBones; a++)
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
                        TempBoneData.UnknownFloat6 = StreamUtil.ReadFloat(streamMatrix);

                        TempBoneData.FileID = Model.FileID;
                        TempBoneData.BonePos = a;

                        Model.boneDatas.Add(TempBoneData);
                    }

                    streamMatrix.Position = Model.OffsetIKPointList;
                    Model.iKPoints = new List<Vector3>();
                    for (int a = 0; a < Model.NumIKPoints; a++)
                    {
                        var TempIKPoint = new Vector3();
                        TempIKPoint = StreamUtil.ReadVector3(streamMatrix);
                        streamMatrix.Position += 4;
                        Model.iKPoints.Add(TempIKPoint);
                    }



                }

            }
        }


    }


    public struct ModelHeader
    {
        public string ModelName;
        public int ModelOffset;
        public int ModelSize;
        public int OffsetBoneData;
        public int OffsetBoneData2;

        public int OffsetMateralList;
        public int OffsetBoneData3;
        public int OffsetIKPointList;
        public int OffsetMorphList;

        public int OffsetSkinningSection;
        public int OffsetTristripSection;
        public int Unknown0;
        public int OffsetVertexSection;

        public int OffsetLastData;
        public int OffsetUnknownData;

        public int Unknown1;
        public int Unknown2;
        public int NumBones;
        public int NumMorphs;
        public int NumMaterials;
        public int NumIKPoints;
        public int NumSkinningHeaders;
        public int NumTristripGroups;
        public int Unknown3;
        public int NumVertices;

        public int FileID;
        public int NumLastData;
        public int NumUnknownData;

        public byte[] Matrix;

        public List<MaterialData> materialDatas;
        public List<BoneData> boneDatas;
        public List<Vector3> iKPoints;
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
        public float UnknownFloat6;

        public int FileID;
        public int BonePos;

        public string parentName;
    }

}
