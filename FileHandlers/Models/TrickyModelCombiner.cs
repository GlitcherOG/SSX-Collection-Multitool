using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models
{
    public class TrickyModelCombiner
    {
        public TrickyMPFModelHandler? Body;
        public TrickyMPFModelHandler? Head;
        public TrickyMPFModelHandler? Boards;

        public List<ReassignedMesh> reassignedMesh = new List<ReassignedMesh>();


        public void StartReassignMesh()
        {
            reassignedMesh = new List<ReassignedMesh>();
            var TempMesh = new ReassignedMesh();
            TempMesh.faces = new List<TrickyMPFModelHandler.Face>();
            TempMesh.materials = new List<TrickyMPFModelHandler.MaterialData>();
            TempMesh.bones = new List<TrickyMPFModelHandler.BoneData>();

            int HeadBonesStart = 0;
            int HeadBonesStart2 = 0;
            int HeadBonesStart3 = 0;
            int HeadBonesStart4 = 0;
            int HeadBonesStart5 = 0;

            //Character 3000
            TempMesh.materials.AddRange(Body.ModelList[0].materialDatas);
            TempMesh.bones.AddRange(Body.ModelList[0].boneDatas);
            HeadBonesStart = TempMesh.bones.Count;
            TempMesh.bones.AddRange(Head.ModelList[0].boneDatas);
            HeadBonesStart2 = TempMesh.bones.Count;
            TempMesh.bones.AddRange(Head.ModelList[1].boneDatas);
            HeadBonesStart3 = TempMesh.bones.Count;
            TempMesh.bones.AddRange(Head.ModelList[2].boneDatas);
            HeadBonesStart4 = TempMesh.bones.Count;
            TempMesh.bones.AddRange(Head.ModelList[3].boneDatas);
            HeadBonesStart5 = TempMesh.bones.Count;
            TempMesh.bones.AddRange(Head.ModelList[4].boneDatas);
            TempMesh.faces.AddRange(ReturnFixedFaces(Body.ModelList[0]));
            TempMesh.faces.AddRange(ReturnFixedFaces(Head.ModelList[0], HeadBonesStart));
            TempMesh.faces.AddRange(ReturnFixedFaces(Head.ModelList[1], HeadBonesStart2));
            TempMesh.faces.AddRange(ReturnFixedFaces(Head.ModelList[2], HeadBonesStart3));
            TempMesh.faces.AddRange(ReturnFixedFaces(Head.ModelList[3], HeadBonesStart4));
            //TempMesh.faces.AddRange(ReturnFixedFaces(Head.ModelList[4], HeadBonesStart5));
            reassignedMesh.Add(TempMesh);

        }

        public List<TrickyMPFModelHandler.Face> ReturnFixedFaces(TrickyMPFModelHandler.MPFModelHeader modelHeader, int StartBoneID = 0)
        {
            List<TrickyMPFModelHandler.Face> NewFaces = new List<TrickyMPFModelHandler.Face>();

            for (int i = 0; i < modelHeader.boneWeightHeader.Count; i++)
            {
                modelHeader.boneWeightHeader[i] = FixBoneIDs(modelHeader.boneWeightHeader[i], StartBoneID);
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

        public TrickyMPFModelHandler.BoneWeightHeader FixBoneIDs(TrickyMPFModelHandler.BoneWeightHeader weightHeader, int BoneStartID)
        {
            var NewHeader = weightHeader;
            for (int i = 0; i < NewHeader.boneWeights.Count; i++)
            {
                var Temp = NewHeader.boneWeights[i];
                if (Temp.Flag != 0)
                {
                    Temp.BoneID += BoneStartID;
                }
                NewHeader.boneWeights[i] = Temp;
            }
            return NewHeader;
        }

        public struct ReassignedMesh
        {
            public List<TrickyMPFModelHandler.Face> faces;
            public List<TrickyMPFModelHandler.MaterialData> materials;
            public List<TrickyMPFModelHandler.BoneData> bones;
        }
    }
}
