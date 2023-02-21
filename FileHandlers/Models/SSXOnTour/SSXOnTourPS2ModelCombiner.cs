using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models
{
    public class SSXOnTourPS2ModelCombiner
    {
        public SSXOnTourMPF? modelHandlers = null;
        public List<SSXOnTourMPF.BoneData> boneDatasOrg = new List<SSXOnTourMPF.BoneData>();
        public List<SSXOnTourMPF.BoneData> boneDatas = new List<SSXOnTourMPF.BoneData>();
        public List<SSXOnTourMPF.MaterialData> materials = new List<SSXOnTourMPF.MaterialData>();

        public List<int> FileIDs = new List<int>();

        public List<ReassignedMesh> reassignedMesh = new List<ReassignedMesh>();


        public bool NormalAverage = false;
        public bool UpdateBones = false;
        public void AddFile(SSXOnTourMPF modelHandler)
        {
            modelHandlers = modelHandler;
            boneDatasOrg = new List<SSXOnTourMPF.BoneData>();
            for (int i = 0; i < modelHandler.ModelList.Count; i++)
            {
                if (!FileIDs.Contains(modelHandler.ModelList[i].FileID))
                {
                    boneDatasOrg.AddRange(modelHandler.ModelList[i].BoneList);
                    FileIDs.Add(modelHandler.ModelList[i].FileID);
                }
            }
        }

        public void AddBones(SSXOnTourMPF modelHandler)
        {
            for (int i = 0; i < modelHandler.ModelList.Count; i++)
            {
                if (!FileIDs.Contains(modelHandler.ModelList[i].FileID))
                {
                    boneDatasOrg.AddRange(modelHandler.ModelList[i].BoneList);
                    FileIDs.Add(modelHandler.ModelList[i].FileID);
                }
            }
        }

        public void MeshReassigned(int MeshID)
        {
            var TempModel = modelHandlers.ModelList[MeshID];
            materials = TempModel.MaterialList;
            var TempMesh = new ReassignedMesh();
            if (TempModel.MaterialGroupList[0].Type == 17)
            {
                TempMesh.ShadowModel = true;
            }

            boneDatas = new List<SSXOnTourMPF.BoneData>();
            for (int i = 0; i < boneDatasOrg.Count; i++)
            {
                var NewBone = new SSXOnTourMPF.BoneData();

                NewBone.BoneName = boneDatasOrg[i].BoneName;
                NewBone.ParentFileID = boneDatasOrg[i].ParentFileID;
                NewBone.ParentBone = boneDatasOrg[i].ParentBone;
                NewBone.Unknown1 = boneDatasOrg[i].Unknown1;
                NewBone.BoneID = boneDatasOrg[i].BoneID;

                NewBone.Unknown2 = boneDatasOrg[i].Unknown2;
                NewBone.Unknown3 = boneDatasOrg[i].Unknown3;
                NewBone.Unknown4 = boneDatasOrg[i].Unknown4;
                NewBone.Unknown5 = boneDatasOrg[i].Unknown5;
                NewBone.Unknown6 = boneDatasOrg[i].Unknown6;

                NewBone.Position = boneDatasOrg[i].Position;
                NewBone.Rotation = boneDatasOrg[i].Rotation;
                NewBone.Unknown = boneDatasOrg[i].Unknown;

                NewBone.FileID = boneDatasOrg[i].FileID;
                NewBone.BonePos = boneDatasOrg[i].BonePos;
                boneDatas.Add(NewBone);
            }

            TempMesh.MorphTargetCount = TempModel.MorphCount;
            ReshuffleBones();
            TempMesh.faces = ReturnFixedFaces(TempModel, boneDatas);
            TempMesh.MeshName = TempModel.ModelName;
            reassignedMesh = new List<ReassignedMesh>();
            reassignedMesh.Add(TempMesh);
            FixBoneParents();
        }

        public void ReshuffleBones()
        {
            List<SSXOnTourMPF.BoneData> TempBoneDatas = new List<SSXOnTourMPF.BoneData>();
            //Add Bones with no Parents
            List<int> RemoveInts = new List<int>();
            for (int i = 0; i < boneDatas.Count; i++)
            {
                if (boneDatas[i].ParentFileID == -1 && boneDatas[i].ParentBone == -1)
                {
                    TempBoneDatas.Add(boneDatas[i]);
                    RemoveInts.Add(i - RemoveInts.Count);
                }
            }

            for (int i = 0; i < RemoveInts.Count; i++)
            {
                boneDatas.RemoveAt(RemoveInts[i]);
            }

            while (true)
            {
                for (int i = 0; i < boneDatas.Count; i++)
                {
                    bool TestDone = false;
                    for (int a = 0; a < TempBoneDatas.Count; a++)
                    {
                        if (boneDatas[i].ParentFileID == TempBoneDatas[a].FileID && boneDatas[i].ParentBone == TempBoneDatas[a].BonePos)
                        {
                            TempBoneDatas.Add(boneDatas[i]);
                            boneDatas.RemoveAt(i);
                            TestDone = true;
                            break;
                        }

                    }
                    if (TestDone)
                    {
                        break;
                    }
                }

                if (boneDatas.Count == 0)
                {
                    break;
                }
            }

            boneDatas = TempBoneDatas;
        }

        public List<SSXOnTourMPF.Face> ReturnFixedFaces(SSXOnTourMPF.MPFHeader modelHeader, List<SSXOnTourMPF.BoneData> BoneData)
        {
            List<SSXOnTourMPF.Face> NewFaces = new List<SSXOnTourMPF.Face>();

            for (int i = 0; i < modelHeader.BoneWeightHeaderList.Count; i++)
            {
                modelHeader.BoneWeightHeaderList[i] = FixBoneIDs(modelHeader.BoneWeightHeaderList[i], BoneData);
            }

            for (int a = 0; a < modelHeader.MaterialGroupList.Count; a++)
            {
                for (int ab = 0; ab < modelHeader.MaterialGroupList[a].WeightRefList.Count; ab++)
                {
                    for (int ac = 0; ac < modelHeader.MaterialGroupList[a].WeightRefList[ab].MorphMeshGroupList.Count; ac++)
                    {
                        for (int i = 0; i < modelHeader.MaterialGroupList[a].WeightRefList[ab].MorphMeshGroupList[ac].MeshChunkList.Count; i++)
                        {
                            var Data = modelHeader.MaterialGroupList[a].WeightRefList[ab].MorphMeshGroupList[ac].MeshChunkList[i];
                            int MatId = modelHeader.MaterialGroupList[a].Material;
                            int WeightRefListID = modelHeader.MaterialGroupList[a].WeightRefList[ab].MorphMeshGroupList[ac].WeightRefID;

                            for (int b = 0; b < Data.Faces.Count; b++)
                            {
                                var Face = Data.Faces[b];

                                var TempList = modelHeader.WeightRefrenceList[WeightRefListID];

                                int WeightId = TempList.WeightIDs[Face.Weight1Pos];
                                Face.Weight1 = modelHeader.BoneWeightHeaderList[WeightId];

                                WeightId = TempList.WeightIDs[Face.Weight2Pos];
                                Face.Weight2 = modelHeader.BoneWeightHeaderList[WeightId];

                                WeightId = TempList.WeightIDs[Face.Weight3Pos];
                                Face.Weight3 = modelHeader.BoneWeightHeaderList[WeightId];

                                Face.MaterialID = MatId;

                                if ((Face.V1 != Face.V2) && (Face.V2 != Face.V3) && (Face.V3 != Face.V1))
                                {
                                    NewFaces.Add(Face);
                                }
                            }

                        }
                    }
                }
            }

            return NewFaces;
        }

        public SSXOnTourMPF.BoneWeightHeader FixBoneIDs(SSXOnTourMPF.BoneWeightHeader weightHeader, List<SSXOnTourMPF.BoneData> BoneData)
        {
            var NewHeader = weightHeader;
            for (int i = 0; i < NewHeader.BoneWeightList.Count; i++)
            {
                var Temp = NewHeader.BoneWeightList[i];
                for (int a = 0; a < BoneData.Count; a++)
                {
                    if (BoneData[a].FileID == Temp.FileID && BoneData[a].BonePos == Temp.BoneID)
                    {
                        Temp.BoneID = a;
                        break;
                    }
                }
                NewHeader.BoneWeightList[i] = Temp;
            }
            return NewHeader;
        }

        public string CheckBones(int ModelID = -1)
        {
            if (ModelID == -1)
            {
                ModelID = 0;
            }

            var TempModelHandler = modelHandlers.ModelList[ModelID];

            for (int i = 0; i < TempModelHandler.BoneWeightHeaderList.Count; i++)
            {
                var TempWeightHeader = TempModelHandler.BoneWeightHeaderList[i];

                for (int a = 0; a < TempWeightHeader.BoneWeightList.Count; a++)
                {
                    var TempWeightList = TempWeightHeader.BoneWeightList[a];
                    bool test = false;
                    for (int b = 0; b < boneDatasOrg.Count; b++)
                    {
                        if (boneDatasOrg[b].BonePos == TempWeightList.BoneID && boneDatasOrg[b].FileID == TempWeightList.FileID)
                        {
                            test = true;
                            break;
                        }
                    }
                    if (!test)
                    {
                        if (TempWeightList.FileID == 0)
                        {
                            return "Missing Bones From Torso";
                        }
                        else
                        if (TempWeightList.FileID == 4)
                        {
                            return "Missing Bones From Board";
                        }
                        else
                        if (TempWeightList.FileID == 1)
                        {
                            return "Missing Bones From Traffic Hands";
                        }
                        else
                        {
                            return "Missing Bones From File " + TempWeightList.FileID;
                        }
                    }
                }
            }

            for (int i = 0; i < boneDatasOrg.Count; i++)
            {
                bool test = false;
                if (boneDatasOrg[i].ParentBone != -1 && boneDatasOrg[i].ParentFileID != -1)
                {
                    for (int a = 0; a < boneDatasOrg.Count; a++)
                    {
                        if (boneDatasOrg[i].ParentBone == boneDatasOrg[a].BonePos && boneDatasOrg[i].ParentFileID == boneDatasOrg[a].FileID)
                        {
                            test = true;
                            break;
                        }
                    }
                    if (!test)
                    {
                        if (boneDatasOrg[i].ParentFileID == 0)
                        {
                            return "Missing Bones From Torso";
                        }
                        else
                        if(boneDatasOrg[i].ParentFileID == 4)
                        {
                            return "Missing Bones From Board";
                        }
                        else
                        if (boneDatasOrg[i].ParentFileID == 1)
                        {
                            return "Missing Bones From Traffic Hands";
                        }
                        else
                        {
                            return "Missing Bones From File " + boneDatasOrg[i].ParentFileID;
                        }
                    }
                }
            }

            return "";
        }

        public void FixBoneParents()
        {
            for (int i = 0; i < boneDatas.Count; i++)
            {
                if (boneDatas[i].ParentFileID != -1 && boneDatas[i].ParentBone != -1)
                {
                    for (int a = 0; a < boneDatas.Count; a++)
                    {
                        if (boneDatas[i].ParentFileID == boneDatas[a].FileID && boneDatas[i].ParentBone == boneDatas[a].BonePos)
                        {
                            var TempBone = boneDatas[i];
                            TempBone.ParentBone = a;
                            boneDatas[i] = TempBone;
                            break;
                        }
                    }
                }
            }
        }

        public struct ReassignedMesh
        {
            public string MeshName;
            public int MeshId;
            public bool ShadowModel;
            public int MorphTargetCount;
            //public List<Vector3> IKPoints;
            public List<SSXOnTourMPF.Face> faces;
        }

        public struct VectorPoint
        {
            public bool Tristripped;
            public int Material;
            public Vector3 vector;
            public Vector3 normal;
            public Vector4 TextureCord;
            public int Weight;
            public List<Vector3> Normals;
            public List<Vector3> MorphData;
        }


    }
}
