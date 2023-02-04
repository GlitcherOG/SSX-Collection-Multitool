using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models
{
    public class SSX3ModelCombiner
    {
        public SSX3MPFModelHandler? modelHandlers = null;
        public List<SSX3MPFModelHandler.BoneData> boneDatas = new List<SSX3MPFModelHandler.BoneData>();
        public List<SSX3MPFModelHandler.MaterialData> materials = new List<SSX3MPFModelHandler.MaterialData>();

        public ReassignedMesh reassignedMesh = new ReassignedMesh();

        public void AddFile(SSX3MPFModelHandler modelHandler)
        {
            modelHandlers = modelHandler;
            boneDatas = new List<SSX3MPFModelHandler.BoneData>();
            boneDatas.AddRange(modelHandler.ModelList[0].BoneList);
        }

        public void AddBones(SSX3MPFModelHandler modelHandler)
        {
            var TempBoneRange = boneDatas;
            boneDatas = new List<SSX3MPFModelHandler.BoneData>();
            boneDatas.AddRange(modelHandler.ModelList[0].BoneList);
            boneDatas.AddRange(TempBoneRange);
        }

        public void MeshReassigned(int MeshID)
        {
            if(CheckBones(MeshID)!="")
            {
                MessageBox.Show(CheckBones(MeshID));
                return;
            }
            var TempModel = modelHandlers.ModelList[MeshID];
            materials = TempModel.MaterialList;
            var TempMesh = new ReassignedMesh();
            TempMesh.faces = ReturnFixedFaces(TempModel, boneDatas);
            TempMesh.MeshName = TempModel.ModelName;
            reassignedMesh = TempMesh;
            FixBoneParents();
        }

        public List<SSX3MPFModelHandler.Face> ReturnFixedFaces(SSX3MPFModelHandler.MPFModelHeader modelHeader, List<SSX3MPFModelHandler.BoneData> BoneData)
        {
            List<SSX3MPFModelHandler.Face> NewFaces = new List<SSX3MPFModelHandler.Face>();

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
                                var TempList = new SSX3MPFModelHandler.WeightRefList();
                                try
                                {
                                    TempList = modelHeader.WeightRefrenceLists[WeightRefListID];
                                }
                                catch
                                {
                                    TempList = modelHeader.WeightRefrenceLists[0];
                                }
                                int WeightId = 0;

                                WeightId = TempList.WeightIDs[Face.Weight1Pos];
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

        public SSX3MPFModelHandler.BoneWeightHeader FixBoneIDs(SSX3MPFModelHandler.BoneWeightHeader weightHeader, List<SSX3MPFModelHandler.BoneData> BoneData)
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

        public string CheckBones(int ModelID)
        {
            if(ModelID==-1)
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
                    for (int b = 0; b < boneDatas.Count; b++)
                    {
                        if (boneDatas[b].BonePos == TempWeightList.BoneID && boneDatas[b].FileID == TempWeightList.FileID)
                        {
                            test = true;
                            break;
                        }
                    }
                    if (!test)
                    {
                        if (TempWeightList.FileID == 0)
                        {
                            return "Missing Bones From Top";
                        }
                        else
                        if (boneDatas[i].ParentFileID == 1)
                        {
                            return "Missing Bones From Board Bindings";
                        }
                        else
                        {
                            return "Missing Bones Form " + TempWeightList.FileID;
                        }
                    }
                }
            }

            for (int i = 0; i < boneDatas.Count; i++)
            {
                bool test = false;
                if (boneDatas[i].ParentBone != -1 && boneDatas[i].ParentFileID != -1)
                {
                    for (int a = 0; a < boneDatas.Count; a++)
                    {
                        if (boneDatas[i].ParentBone == boneDatas[a].BonePos && boneDatas[i].ParentFileID == boneDatas[a].FileID)
                        {
                            test = true;
                            break;
                        }
                    }
                    if (!test)
                    {
                        if (boneDatas[i].ParentFileID == 0)
                        {
                            return "Missing Bones From Top";
                        }
                        else
                        if (boneDatas[i].ParentFileID == 1)
                        {
                            return "Missing Bones From Board Bindings";
                        }
                        else
                        {
                            return "Missing Bones From " + boneDatas[i].ParentFileID;
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
            public List<Vector3> IKPoints;
            public List<SSX3MPFModelHandler.Face> faces;
        }
    }
}
