using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models.SSX2012
{
    public class CRSFHandler
    {
        public string Magic;
        public int U1;
        public int U2;
        public int U3;

        public WEITStruct weitStruct = new WEITStruct();
        public SkelStruct skelStruct = new SkelStruct();
        public FPOFStruct fpofStruct = new FPOFStruct();
        public SGRFStruct sgrfStruct = new SGRFStruct();

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                Magic = StreamUtil.ReadString(stream, 4);
                U1 = StreamUtil.ReadUInt32(stream);
                U2 = StreamUtil.ReadUInt32(stream);
                U3 = StreamUtil.ReadUInt32(stream);

                weitStruct = new WEITStruct();
                weitStruct.WEITMagic = StreamUtil.ReadString(stream, 4);
                weitStruct.U0 = StreamUtil.ReadUInt32(stream, true);
                weitStruct.U1 = StreamUtil.ReadUInt32(stream, true);
                weitStruct.U2 = StreamUtil.ReadUInt32(stream, true);
                weitStruct.U3 = StreamUtil.ReadUInt32(stream, true);
                weitStruct.U4 = StreamUtil.ReadUInt32(stream, true);
                weitStruct.WeightCount = StreamUtil.ReadUInt32(stream, true);
                weitStruct.VertexCount = StreamUtil.ReadUInt32(stream, true);

                weitStruct.weightStructs = new List<WeightStruct>();

                for (int i = 0; i < weitStruct.VertexCount; i++)
                {
                    WeightStruct NewWeightStruct = new WeightStruct();
                    NewWeightStruct.WeightValue = new List<float>();
                    for (int a = 0; a < weitStruct.WeightCount; a++)
                    {
                        NewWeightStruct.WeightValue.Add(StreamUtil.ReadFloat(stream, true));
                    }
                    weitStruct.weightStructs.Add(NewWeightStruct);
                }

                for (int i = 0; i < weitStruct.weightStructs.Count; i++)
                {
                    WeightStruct NewWeightStruct = weitStruct.weightStructs[i];
                    NewWeightStruct.BoneIndex = new List<int>();
                    for (int a = 0; a < weitStruct.WeightCount; a++)
                    {
                        NewWeightStruct.BoneIndex.Add(StreamUtil.ReadInt8(stream));
                    }
                    weitStruct.weightStructs[i] = NewWeightStruct;
                }

                skelStruct = new SkelStruct();
                skelStruct.SkelMagic = StreamUtil.ReadString(stream, 4);
                skelStruct.U0 = StreamUtil.ReadUInt32(stream, true);
                skelStruct.U1 = StreamUtil.ReadUInt32(stream, true);
                skelStruct.U2 = StreamUtil.ReadUInt32(stream, true);
                skelStruct.BoneCount = StreamUtil.ReadUInt32(stream, true);

                skelStruct.boneDatas = new List<BoneData>();
                for (int i = 0; i < skelStruct.BoneCount; i++)
                {
                    BoneData boneData = new BoneData();

                    boneData.BoneNameSize = StreamUtil.ReadUInt32(stream, true);
                    boneData.BoneName = StreamUtil.ReadString(stream, boneData.BoneNameSize + 1, true);

                    skelStruct.boneDatas.Add(boneData);
                }

                for (int i = 0; i < skelStruct.boneDatas.Count; i++)
                {
                    BoneData boneData = skelStruct.boneDatas[i];

                    boneData.matrix = StreamUtil.ReadMatrix4x4(stream);

                    skelStruct.boneDatas[i] = boneData;
                }

                StreamUtil.AlignBy16(stream);

                string Test = StreamUtil.ReadString(stream, 4);
                if(Test=="FPOF")
                {
                    stream.Position -= 4;
                    fpofStruct = new FPOFStruct();

                    fpofStruct.FPOFMagic = StreamUtil.ReadString(stream, 4);

                    fpofStruct.U0 = StreamUtil.ReadUInt32(stream, true);
                    fpofStruct.U1 = StreamUtil.ReadUInt32(stream, true);
                    fpofStruct.U2 = StreamUtil.ReadUInt32(stream, true);
                    fpofStruct.FacePoseCount = StreamUtil.ReadUInt32(stream, true);

                    fpofStruct.faceBones = new List<FaceBone>();

                    for (int i = 0; i < fpofStruct.FacePoseCount; i++)
                    {
                        var NewFaceBone = new FaceBone();

                        NewFaceBone.NameCount = StreamUtil.ReadUInt32(stream, true);
                        NewFaceBone.Name = StreamUtil.ReadString(stream, NewFaceBone.NameCount+1);
                        NewFaceBone.U0 = StreamUtil.ReadFloat(stream, true);
                        NewFaceBone.U1 = StreamUtil.ReadFloat(stream, true);
                        NewFaceBone.U2 = StreamUtil.ReadFloat(stream, true);
                        NewFaceBone.U3 = StreamUtil.ReadFloat(stream, true);

                        fpofStruct.faceBones.Add(NewFaceBone);
                    }
                }

                if(Test == "SGRF")
                {
                    stream.Position -= 4;
                    sgrfStruct = new SGRFStruct();

                    sgrfStruct.Magic = StreamUtil.ReadString(stream, 4);
                    sgrfStruct.U0 = StreamUtil.ReadUInt32(stream, true);
                    sgrfStruct.Size = StreamUtil.ReadUInt32(stream, true);

                    sgrfStruct.SGNodeGroup = new List<SGNode>();
                    for (int i = 0; i < 1; i++)
                    {
                        var TempSGNode = new SGNode();

                        TempSGNode.U0 = StreamUtil.ReadUInt32(stream, true);
                        TempSGNode.NodeSize = StreamUtil.ReadUInt32(stream, true);
                        TempSGNode.NodeType = StreamUtil.ReadString(stream, TempSGNode.NodeSize +1);
                        TempSGNode.U1 = StreamUtil.ReadUInt32(stream, true);
                        TempSGNode.NameSize = StreamUtil.ReadUInt32(stream, true);
                        TempSGNode.Name = StreamUtil.ReadString(stream, TempSGNode.NameSize + 1);
                        TempSGNode.U11 = StreamUtil.ReadUInt16(stream, true);
                        TempSGNode.U2 = StreamUtil.ReadUInt32(stream, true);
                        TempSGNode.U3 = StreamUtil.ReadUInt32(stream, true);

                        sgrfStruct.SGNodeGroup.Add(TempSGNode);
                    }

                    sgrfStruct.SGNodeTransform = new List<SGNode>();
                    for (int i = 0; i < 2; i++)
                    {
                        var TempSGNode = new SGNode();

                        TempSGNode.U0 = StreamUtil.ReadUInt32(stream, true);
                        TempSGNode.NodeSize = StreamUtil.ReadUInt32(stream, true);
                        TempSGNode.NodeType = StreamUtil.ReadString(stream, TempSGNode.NodeSize + 1);
                        TempSGNode.U1 = StreamUtil.ReadUInt32(stream, true);
                        TempSGNode.NameSize = StreamUtil.ReadUInt32(stream, true);
                        TempSGNode.Name = StreamUtil.ReadString(stream, TempSGNode.NameSize + 1);
                        TempSGNode.U11 = StreamUtil.ReadInt8(stream);
                        TempSGNode.U2 = StreamUtil.ReadUInt32(stream, true);
                        TempSGNode.U3 = StreamUtil.ReadUInt32(stream, true);

                        TempSGNode.U4 = StreamUtil.ReadUInt32(stream, true);

                        TempSGNode.transformDatas = new List<TransformData>();

                        for (int a = 0; a < TempSGNode.U4; a++)
                        {
                            var TempTransformData = new TransformData();

                            TempTransformData.Type = StreamUtil.ReadUInt32(stream, true);
                            TempTransformData.U0 = StreamUtil.ReadUInt32(stream, true);
                            TempTransformData.TransformNameSize = StreamUtil.ReadUInt32(stream, true);
                            TempTransformData.TransformName = StreamUtil.ReadString(stream, TempTransformData.TransformNameSize + 1);
                            TempTransformData.X = StreamUtil.ReadFloat(stream, true);
                            TempTransformData.Y = StreamUtil.ReadFloat(stream, true);
                            TempTransformData.Z = StreamUtil.ReadFloat(stream, true);

                            TempSGNode.transformDatas.Add(TempTransformData);
                        }

                        TempSGNode.U5 = StreamUtil.ReadInt8(stream);
                        TempSGNode.U6 = StreamUtil.ReadUInt32(stream, true);
                        TempSGNode.U7 = StreamUtil.ReadUInt32(stream, true);

                        TempSGNode.matrix = StreamUtil.ReadMatrix4x4(stream, true);

                        sgrfStruct.SGNodeTransform.Add(TempSGNode);
                    }

                    sgrfStruct.SGNodeJoint = new List<SGNode>();
                    for (int i = 0; i < 279; i++)
                    {
                        var TempSGNode = new SGNode();

                        TempSGNode.U0 = StreamUtil.ReadUInt32(stream, true);
                        TempSGNode.NodeSize = StreamUtil.ReadUInt32(stream, true);
                        TempSGNode.NodeType = StreamUtil.ReadString(stream, TempSGNode.NodeSize + 1);
                        TempSGNode.U1 = StreamUtil.ReadUInt32(stream, true);
                        TempSGNode.NameSize = StreamUtil.ReadUInt32(stream, true);
                        TempSGNode.Name = StreamUtil.ReadString(stream, TempSGNode.NameSize + 1);
                        TempSGNode.U11 = StreamUtil.ReadInt8(stream);
                        TempSGNode.U2 = StreamUtil.ReadUInt32(stream, true);
                        TempSGNode.U3 = StreamUtil.ReadUInt32(stream, true);

                        TempSGNode.U4 = StreamUtil.ReadUInt32(stream, true);

                        TempSGNode.transformDatas = new List<TransformData>();

                        for (int a = 0; a < TempSGNode.U4; a++)
                        {
                            var TempTransformData = new TransformData();

                            TempTransformData.Type = StreamUtil.ReadUInt32(stream, true);
                            TempTransformData.U0 = StreamUtil.ReadUInt32(stream, true);
                            TempTransformData.TransformNameSize = StreamUtil.ReadUInt32(stream, true);
                            TempTransformData.TransformName = StreamUtil.ReadString(stream, TempTransformData.TransformNameSize + 1);
                            TempTransformData.X = StreamUtil.ReadFloat(stream, true);
                            TempTransformData.Y = StreamUtil.ReadFloat(stream, true);
                            TempTransformData.Z = StreamUtil.ReadFloat(stream, true);

                            TempSGNode.transformDatas.Add(TempTransformData);
                        }

                        TempSGNode.U5 = StreamUtil.ReadInt8(stream);
                        TempSGNode.U6 = StreamUtil.ReadUInt32(stream, true);
                        TempSGNode.U7 = StreamUtil.ReadUInt32(stream, true);

                        TempSGNode.matrix = StreamUtil.ReadMatrix4x4(stream, true);

                        TempSGNode.U8 = StreamUtil.ReadUInt32(stream, true);

                        TempSGNode.matrix1 = StreamUtil.ReadMatrix4x4(stream, true);
                        TempSGNode.matrix2 = StreamUtil.ReadMatrix4x4(stream, true);
                        TempSGNode.matrix3 = StreamUtil.ReadMatrix4x4(stream, true);

                        TempSGNode.U9 = StreamUtil.ReadUInt32(stream, true);

                        sgrfStruct.SGNodeJoint.Add(TempSGNode);
                    }
                }
            }
        }

        public struct WEITStruct
        {
            public string WEITMagic;
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int WeightCount;
            public int VertexCount;

            public List<WeightStruct> weightStructs;
        }

        public struct WeightStruct
        {
            public List<float> WeightValue;
            public List<int> BoneIndex;
        }

        public struct SkelStruct
        {
            public string SkelMagic;
            public int U0;
            public int U1;
            public int U2;
            public int BoneCount;

            public List<BoneData> boneDatas;
        }

        public struct BoneData
        {
            public int BoneNameSize;
            public string BoneName;
            public Matrix4x4 matrix;
        }

        public struct FPOFStruct
        {
            public string FPOFMagic;
            public int U0;
            public int U1;
            public int U2;
            public int FacePoseCount;
            public List<FaceBone> faceBones;
        }

        public struct FaceBone
        {
            public int NameCount;
            public string Name;
            public float U0;
            public float U1;
            public float U2;
            public float U3;
        }

        public struct SGRFStruct
        {
            public string Magic;
            public int U0;
            public int Size;

            public List<SGNode> SGNodeGroup;
            public List<SGNode> SGNodeTransform;
            public List<SGNode> SGNodeJoint;
        } 

        public struct TransformData
        {
            public int Type;
            public int U0;
            public int TransformNameSize;
            public string TransformName;
            public float X;
            public float Y;
            public float Z;
        }

        public struct SGNode
        {
            //SGNodeGroup
            public int U0;
            public int NodeSize;
            public string NodeType;
            public int U1;
            public int NameSize;
            public string Name; 
            public int U11; //2 for base, 1 for rest
            public int U2; 
            public int U3;

            //SGNodeTransform
            public int U4;
            public List<TransformData> transformDatas;
            public int U5; //8
            public int U6;
            public int U7;
            public Matrix4x4 matrix;

            //SGNodeJoint
            public int U8;
            public Matrix4x4 matrix1;
            public Matrix4x4 matrix2;
            public Matrix4x4 matrix3;
            public int U9;
        }
    }
}
