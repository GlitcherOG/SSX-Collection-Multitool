using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SSXMultiTool.FileHandlers.Models
{
    public class TrickyModelCombiner
    {
        public TrickyMPFModelHandler? Body;
        public TrickyMPFModelHandler? Head;
        public TrickyMPFModelHandler? Board;
        public List<TrickyMPFModelHandler.MaterialData> materials;
        public List<TrickyMPFModelHandler.BoneData> bones;
        bool BodyBool;


        public List<ReassignedMesh> reassignedMesh = new List<ReassignedMesh>();

        public int DectectModelType(TrickyMPFModelHandler modelHandler)
        {
            for (int i = 0; i < modelHandler.ModelList.Count; i++)
            {
                if (modelHandler.ModelList[i].FileName.ToLower().Contains("body"))
                {
                    Board = null;
                    Body = modelHandler;
                    BodyBool = true;
                    return 0;
                }
                if (modelHandler.ModelList[i].FileName.ToLower().Contains("head"))
                {
                    Board = null;
                    Head = modelHandler;
                    BodyBool = true;
                    return 1;
                }
                if (modelHandler.ModelList[i].FileName.ToLower().Contains("algoofy"))
                {
                    Head= null;
                    Body = null;
                    Board = modelHandler;
                    BodyBool = false;
                    return 2;
                }
            }
            return -1;
        }

        public void StartReassignMesh(int MeshID)
        {
            if (MeshID != -1)
            {
                if (BodyBool)
                {
                    StartReassignMeshCharacter(MeshID);
                }
                else
                {
                    StartReassignMeshBoard(MeshID);
                }
            }
        }

        public void StartReassignMeshBoard(int MeshID)
        {
            reassignedMesh = new List<ReassignedMesh>();
            var TempMesh = new ReassignedMesh();
            TempMesh.faces = new List<TrickyMPFModelHandler.Face>();
            materials = new List<TrickyMPFModelHandler.MaterialData>();
            bones = new List<TrickyMPFModelHandler.BoneData>();


            materials.AddRange(Board.ModelList[MeshID].materialDatas);
            bones.AddRange(Board.ModelList[MeshID].boneDatas);
            TempMesh.faces.AddRange(ReturnFixedFaces(Board.ModelList[MeshID], bones));
            TempMesh.MeshName = Board.ModelList[MeshID].FileName;
            if (TempMesh.MeshName.ToLower().Contains("shdw"))
            {
                TempMesh.ShadowModel = true;
            }
            TempMesh.IKPoints = Board.ModelList[MeshID].iKPoints;


            reassignedMesh.Add(TempMesh);
        }




        public void StartReassignMeshCharacter(int MeshID)
        {
            reassignedMesh = new List<ReassignedMesh>();
            List<int> headboneStart = new List<int>();
            headboneStart.Add(0);
            materials = new List<TrickyMPFModelHandler.MaterialData>();
            bones = new List<TrickyMPFModelHandler.BoneData>();
            bool Materials = false;
            int ListSize = 0;

            //Body
            for (int i = 0; i < Body.ModelList.Count; i++)
            {
                if ((MeshID == 0 && Body.ModelList[i].FileName.Contains("3000")) ||
                    (MeshID == 1 && Body.ModelList[i].FileName.Contains("1500")) ||
                    (MeshID == 2 && Body.ModelList[i].FileName.Contains("750") && !Body.ModelList[i].FileName.ToLower().Contains("shdw")) ||
                    (MeshID == 3 && Body.ModelList[i].FileName.ToLower().Contains("shdw")))
                {
                    var TempMesh = new ReassignedMesh();
                    if (!Materials)
                    {
                        materials.AddRange(Body.ModelList[i].materialDatas);
                        Materials = true;
                    }
                    bones.AddRange(Body.ModelList[i].boneDatas);
                    headboneStart.Add(bones.Count);
                    TempMesh.faces = ReturnFixedFaces(Body.ModelList[i], bones);
                    ListSize++;
                    TempMesh.MeshName = Body.ModelList[i].FileName;
                    if (MeshID == 3)
                    {
                        TempMesh.ShadowModel = true;
                    }

                    reassignedMesh.Add(TempMesh);
                }
            }

            //Head
            for (int i = 0; i < Head.ModelList.Count; i++)
            {
                if ((MeshID == 0 && Head.ModelList[i].FileName.Contains("3000")) ||
                    (MeshID == 1 && Head.ModelList[i].FileName.Contains("1500")) ||
                    (MeshID == 2 && Head.ModelList[i].FileName.Contains("750") && !Head.ModelList[i].FileName.ToLower().Contains("shdw")) ||
                    (MeshID == 3 && Head.ModelList[i].FileName.ToLower().Contains("shdw")))
                {
                    var TempMesh = new ReassignedMesh();
                    bones.AddRange(Head.ModelList[i].boneDatas);
                    headboneStart.Add(bones.Count);
                    TempMesh.MeshName = Head.ModelList[i].FileName;
                    TempMesh.faces = ReturnFixedFaces(Head.ModelList[i], bones);
                    ListSize++;
                    if (MeshID == 3)
                    {
                        TempMesh.ShadowModel = true;
                    }
                    if (Head.ModelList[i].MorphKeyCount!=0)
                    {
                        TempMesh.MorphTargets = true;
                    }
                    reassignedMesh.Add(TempMesh);
                }
            }

        }

        public List<TrickyMPFModelHandler.Face> ReturnFixedFaces(TrickyMPFModelHandler.MPFModelHeader modelHeader, List<TrickyMPFModelHandler.BoneData> BoneData)
        {
            List<TrickyMPFModelHandler.Face> NewFaces = new List<TrickyMPFModelHandler.Face>();

            for (int i = 0; i < modelHeader.boneWeightHeader.Count; i++)
            {
                modelHeader.boneWeightHeader[i] = FixBoneIDs(modelHeader.boneWeightHeader[i], BoneData);
            }


            for (int a = 0; a < modelHeader.MeshGroups.Count; a++)
            {
                for (int ab = 0; ab < modelHeader.MeshGroups[a].meshGroupSubs.Count; ab++)
                {
                    for (int ac = 0; ac < modelHeader.MeshGroups[a].meshGroupSubs[ab].MeshGroupHeaders.Count; ac++)
                    {
                        for (int i = 0; i < modelHeader.MeshGroups[a].meshGroupSubs[ab].MeshGroupHeaders[ac].staticMesh.Count; i++)
                        {
                            var Data = modelHeader.MeshGroups[a].meshGroupSubs[ab].MeshGroupHeaders[ac].staticMesh[i];
                            int MatId = modelHeader.MeshGroups[a].MaterialID;
                            int WeightRefListID = modelHeader.MeshGroups[a].meshGroupSubs[ab].MeshGroupHeaders[ac].WeightRefGroup;

                            for (int b = 0; b < Data.faces.Count; b++)
                            {
                                var Face = Data.faces[b];
                                var TempList = new TrickyMPFModelHandler.NumberListRef();
                                try
                                {
                                    TempList = modelHeader.numberListRefs[WeightRefListID];
                                }
                                catch
                                {
                                    TempList = modelHeader.numberListRefs[0];
                                }
                                int WeightId = 0;

                                WeightId = TempList.WeightIDs[Face.Weight1Pos];
                                Face.Weight1 = modelHeader.boneWeightHeader[WeightId];

                                WeightId = TempList.WeightIDs[Face.Weight2Pos];
                                Face.Weight2 = modelHeader.boneWeightHeader[WeightId];

                                WeightId = TempList.WeightIDs[Face.Weight3Pos];
                                Face.Weight3 = modelHeader.boneWeightHeader[WeightId];

                                Face.MaterialID = modelHeader.MeshGroups[a].MaterialID;

                                NewFaces.Add(Face);  
                            }

                        }
                    }
                }
            }

            return NewFaces;
        }

        public TrickyMPFModelHandler.BoneWeightHeader FixBoneIDs(TrickyMPFModelHandler.BoneWeightHeader weightHeader, List<TrickyMPFModelHandler.BoneData> BoneData)
        {
            var NewHeader = weightHeader;
            for (int i = 0; i < NewHeader.boneWeights.Count; i++)
            {
                var Temp = NewHeader.boneWeights[i];
                for (int a = 0; a < BoneData.Count; a++)
                {
                    if (BoneData[a].FileID== Temp.FileID)
                    {
                        if (BoneData[a].BonePos==Temp.BoneID)
                        {
                            Temp.BoneID = a;
                            break;
                        }
                    }
                }
                NewHeader.boneWeights[i] = Temp;
            }
            return NewHeader;
        }

        public struct ReassignedMesh
        {
            public string MeshName;
            public bool ShadowModel;
            public bool MorphTargets;
            public List<Vector3> IKPoints;
            public List<TrickyMPFModelHandler.Face> faces;
        }
    }
}
