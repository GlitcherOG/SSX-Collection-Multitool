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
        public bool BodyBool;

        public bool BoneUpdate;
        public int TristripMode;
        public bool NormalAverage = true;


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

        public int TristripCount(int MeshID)
        {
            int Count = 0;
            if (BodyBool)
            {
                if (Head != null && Body != null)
                {
                    return 0;
                }
            }
            else if (Board != null)
            {
                var TempMesh = Board.ModelList[MeshID].MeshGroups;

                for (int i = 0; i < TempMesh.Count; i++)
                {
                    for (int a = 0; a < TempMesh[i].meshGroupSubs.Count; a++)
                    {
                        for (int b = 0; b < TempMesh[i].meshGroupSubs[a].MeshGroupHeaders.Count; b++)
                        {
                            for (int c = 0; c < TempMesh[i].meshGroupSubs[a].MeshGroupHeaders[b].staticMesh.Count; c++)
                            {
                                Count += TempMesh[i].meshGroupSubs[a].MeshGroupHeaders[b].staticMesh[c].Strips.Count;
                            }
                        }
                    }
                }
            }

            return Count;
        }

        public int VerticeCount(int MeshID)
        {
            int Count = 0;
            if (BodyBool)
            {
                if (Head != null && Body != null)
                {
                    return 0;
                }
            }
            else if (Board != null)
            {
                var TempMesh = Board.ModelList[MeshID].MeshGroups;

                for (int i = 0; i < TempMesh.Count; i++)
                {
                    for (int a = 0; a < TempMesh[i].meshGroupSubs.Count; a++)
                    {
                        for (int b = 0; b < TempMesh[i].meshGroupSubs[a].MeshGroupHeaders.Count; b++)
                        {
                            for (int c = 0; c < TempMesh[i].meshGroupSubs[a].MeshGroupHeaders[b].staticMesh.Count; c++)
                            {
                                Count += TempMesh[i].meshGroupSubs[a].MeshGroupHeaders[b].staticMesh[c].vertices.Count;
                            }
                        }
                    }
                }
            }

            return Count;
        }

        public int ChunkCount(int MeshID)
        {
            int Count = 0;
            if (BodyBool)
            {
                if (Head != null && Body != null)
                {
                    return 0;
                }
            }
            else if (Board != null)
            {
                var TempMesh = Board.ModelList[MeshID].MeshGroups;

                for (int i = 0; i < TempMesh.Count; i++)
                {
                    for (int a = 0; a < TempMesh[i].meshGroupSubs.Count; a++)
                    {
                        for (int b = 0; b < TempMesh[i].meshGroupSubs[a].MeshGroupHeaders.Count; b++)
                        {
                            Count += TempMesh[i].meshGroupSubs[a].MeshGroupHeaders[b].staticMesh.Count;
                        }
                    }
                }
            }

            return Count;
        }

        public void StartReassignMesh(int MeshID)
        {
            if (MeshID != -1)
            {
                if (BodyBool)
                {
                    if (Head != null && Body != null)
                    {
                        StartReassignMeshCharacter(MeshID);
                    }
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
            materials = new List<TrickyMPFModelHandler.MaterialData>();
            bones = new List<TrickyMPFModelHandler.BoneData>();
            int ListSize = 0;

            var TempMaterial = new TrickyMPFModelHandler.MaterialData();
            TempMaterial.MainTexture = "helm";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyMPFModelHandler.MaterialData();
            TempMaterial.MainTexture = "helm gloss";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyMPFModelHandler.MaterialData();
            TempMaterial.MainTexture = "helm envr";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyMPFModelHandler.MaterialData();
            TempMaterial.MainTexture = "boot";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyMPFModelHandler.MaterialData();
            TempMaterial.MainTexture = "boot gloss";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyMPFModelHandler.MaterialData();
            TempMaterial.MainTexture = "boot envr";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyMPFModelHandler.MaterialData();
            TempMaterial.MainTexture = "head";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyMPFModelHandler.MaterialData();
            TempMaterial.MainTexture = "head gloss";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyMPFModelHandler.MaterialData();
            TempMaterial.MainTexture = "head envr";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyMPFModelHandler.MaterialData();
            TempMaterial.MainTexture = "suit";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyMPFModelHandler.MaterialData();
            TempMaterial.MainTexture = "suit gloss";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyMPFModelHandler.MaterialData();
            TempMaterial.MainTexture = "suit envr";
            materials.Add(TempMaterial);


            //Body
            for (int i = 0; i < Body.ModelList.Count; i++)
            {
                if ((MeshID == 0 && Body.ModelList[i].FileName.Contains("3000")) ||
                    (MeshID == 1 && Body.ModelList[i].FileName.Contains("1500")) ||
                    (MeshID == 2 && Body.ModelList[i].FileName.Contains("750") && !Body.ModelList[i].FileName.ToLower().Contains("shdw")) ||
                    (MeshID == 3 && Body.ModelList[i].FileName.ToLower().Contains("shdw")))
                {
                    var TempMesh = new ReassignedMesh();
                    bones.AddRange(Body.ModelList[i].boneDatas);
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
                    TempMesh.MeshName = Head.ModelList[i].FileName;
                    TempMesh.faces = ReturnFixedFaces(Head.ModelList[i], bones);
                    ListSize++;
                    if (MeshID == 3)
                    {
                        TempMesh.ShadowModel = true;
                    }

                    TempMesh.MorphTargetCount = Head.ModelList[i].MorphKeyCount;
                    reassignedMesh.Add(TempMesh);
                }
            }
            //CorrectBonesandFaces();
        }

        void CorrectBonesandFaces()
        {
            var OldBones = bones;
            bones = new List<TrickyMPFModelHandler.BoneData>();
            if (OldBones[0].ParentBone==-1)
            {
                bones.Add(OldBones[0]);
                FindBoneChildren(0, 0, OldBones);
            }
            else
            {
                MessageBox.Show("Unexpected Error");
            }

            for (int i = 0; i < reassignedMesh.Count; i++)
            {
                var TempMesh = reassignedMesh[i];
                for (int a = 0; a < TempMesh.faces.Count; a++)
                {
                    var TempFace = TempMesh.faces[a];

                    for (int b = 0; b < TempFace.Weight1.boneWeights.Count; b++)
                    {
                        var TempBoneWeight = TempFace.Weight1.boneWeights[b];

                        for (int c = 0; c < bones.Count; c++)
                        {
                            if (bones[c].BonePos == TempBoneWeight.BoneID)
                            {
                                TempBoneWeight.BoneID = c;
                                break;
                            }
                        }

                        TempFace.Weight1.boneWeights[b] = TempBoneWeight;
                    }

                    for (int b = 0; b < TempFace.Weight2.boneWeights.Count; b++)
                    {
                        var TempBoneWeight = TempFace.Weight2.boneWeights[b];

                        for (int c = 0; c < bones.Count; c++)
                        {
                            if (bones[c].BonePos == TempBoneWeight.BoneID)
                            {
                                TempBoneWeight.BoneID = c;
                                break;
                            }
                        }

                        TempFace.Weight2.boneWeights[b] = TempBoneWeight;
                    }

                    for (int b = 0; b < TempFace.Weight3.boneWeights.Count; b++)
                    {
                        var TempBoneWeight = TempFace.Weight3.boneWeights[b];

                        for (int c = 0; c < bones.Count; c++)
                        {
                            if (bones[c].BonePos == TempBoneWeight.BoneID)
                            {
                                TempBoneWeight.BoneID = c;
                                break;
                            }
                        }

                        TempFace.Weight3.boneWeights[b] = TempBoneWeight;
                    }


                    TempMesh.faces[a] = TempFace;
                }
                reassignedMesh[i] = TempMesh;
            }
        }

        void FindBoneChildren(int OldID, int NewID, List<TrickyMPFModelHandler.BoneData> oldBoneList)
        {
            for (int i = 0; i < oldBoneList.Count; i++)
            {
                if (oldBoneList[i].ParentBone==OldID)
                {
                    var TempBone = oldBoneList[i];
                    TempBone.ParentBone = NewID;
                    TempBone.BonePos = i;
                    bones.Add(TempBone);
                    FindBoneChildren(i, bones.Count - 1, oldBoneList);
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

                                var TempMat = modelHeader.materialDatas[modelHeader.MeshGroups[a].MaterialID];
                                int matID = 0;
                                int focusID = 0;
                                if (TempMat.MainTexture != "bord")
                                {
                                    if (TempMat.MainTexture == "helm")
                                    {
                                        matID = 0;
                                    }
                                    else if (TempMat.MainTexture == "boot")
                                    {
                                        matID = 1;
                                    }
                                    else if (TempMat.MainTexture == "head")
                                    {
                                        matID = 2;
                                    }
                                    else if (TempMat.MainTexture == "suit")
                                    {
                                        matID = 3;
                                    }

                                    if (TempMat.Texture3.EndsWith("_g"))
                                    {
                                        focusID = 1;
                                    }
                                    else if (TempMat.Texture4 == "envr")
                                    {
                                        focusID = 2;
                                    }
                                }

                                matID = (3 * matID) + focusID;
                                Face.MaterialID = matID;

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

        public void StartRegenMesh(TrickyModelCombiner trickyModelCombiner, int Selected)
        {
            if(Board!=null&&!BodyBool)
            {
                StartRegenMeshBoard(trickyModelCombiner, Selected);
            }
            else if (Head!=null&&Body!=null)
            {
                StartRegenMeshCharacter(trickyModelCombiner, Selected);
            }
            else
            {
                MessageBox.Show("Error Missing Files");
            }

        }

        public void StartRegenMeshBoard(TrickyModelCombiner trickyModelCombiner, int Selected)
        {
            if(trickyModelCombiner.bones==null)
            {
                MessageBox.Show("No Bones Detected");
                return;
            }
            if (trickyModelCombiner.bones.Count != Board.ModelList[Selected].boneDatas.Count)
            {
                MessageBox.Show("Incorrect Ammount of Bones");
                return;
            }
            
            bool Shadow = false;

            if (Board.ModelList[Selected].FileName.ToLower().Contains("shdw"))
            {
                Shadow = true;
            }

            var TempTrickyMesh = Board.ModelList[Selected];

            ReassignedMesh ReassignedMesh = new ReassignedMesh();
            bool MeshTest = false;
            for (int i = 0; i < trickyModelCombiner.reassignedMesh.Count; i++)
            {
                if (trickyModelCombiner.reassignedMesh[i].MeshName == Board.ModelList[Selected].FileName)
                {
                    ReassignedMesh = trickyModelCombiner.reassignedMesh[i];
                    MeshTest = true;
                }
            }

            if(!MeshTest)
            {
                MessageBox.Show("Error Model Not Found. Ensure Name is Correct");
                return;
            }

            //Regenerate Materials

            //Update Bones

            //Redo Data In Correct Formats IE make Weight List and make faces use the positions.
            TempTrickyMesh.boneWeightHeader = new List<TrickyMPFModelHandler.BoneWeightHeader>();
            
            //Load Headers into file
            for (int i = 0; i < ReassignedMesh.faces.Count; i++)
            {
                var TempFace = ReassignedMesh.faces[i];
                int WeightID = ContainsWeight(TempFace.Weight1, TempTrickyMesh.boneWeightHeader);
                if (WeightID==-1)
                {
                    TempTrickyMesh.boneWeightHeader.Add(TempFace.Weight1);
                    WeightID = TempTrickyMesh.boneWeightHeader.Count - 1;
                }
                TempFace.Weight1Pos = WeightID;

                WeightID = ContainsWeight(TempFace.Weight2, TempTrickyMesh.boneWeightHeader);
                if (WeightID == -1)
                {
                    TempTrickyMesh.boneWeightHeader.Add(TempFace.Weight2);
                    WeightID = TempTrickyMesh.boneWeightHeader.Count - 1;
                }
                TempFace.Weight2Pos = WeightID;

                WeightID = ContainsWeight(TempFace.Weight3, TempTrickyMesh.boneWeightHeader);
                if (WeightID == -1)
                {
                    TempTrickyMesh.boneWeightHeader.Add(TempFace.Weight3);
                    WeightID = TempTrickyMesh.boneWeightHeader.Count - 1;
                }
                TempFace.Weight3Pos = WeightID;

                ReassignedMesh.faces[i] = TempFace;
            }
            //Correct Bone File ID
            for (int i = 0; i < TempTrickyMesh.boneWeightHeader.Count; i++)
            {
                var TempHeader = TempTrickyMesh.boneWeightHeader[i];

                for (int a = 0; a < TempHeader.boneWeights.Count; a++)
                {
                    var TempWeight = TempHeader.boneWeights[a];

                    TempWeight.FileID = TempTrickyMesh.FileID;

                    TempHeader.boneWeights[a] = TempWeight;
                }
                TempTrickyMesh.boneWeightHeader[i] = TempHeader;
            }
            //Take faces and Generate Indce faces and giant vertex list
            List<VectorPoint> VectorPoint = new List<VectorPoint>();
            List<TristripGenerator.IndiceFace> indiceFaces = new List<TristripGenerator.IndiceFace>();

            for (int i = 0; i < ReassignedMesh.faces.Count; i++)
            {
                TristripGenerator.IndiceFace TempFace = new TristripGenerator.IndiceFace();
                bool Test = false;
                int TempID = 0;
                int TestID = 0;
                #region Point 1
                while (!Test)
                {
                    TempID = ContainsVertice(ReassignedMesh.faces[i].V1, VectorPoint, TestID);
                    if (TempID == -1)
                    {
                        TempFace.Id1 = VectorPoint.Count;

                        VectorPoint.Add(GenerateVectorPoint(ReassignedMesh.faces[i], 1));

                        Test = true;
                    }
                    else if (!Shadow)
                    {
                        TestID++;
                        if (ReassignedMesh.faces[i].MaterialID != VectorPoint[TempID].Material)
                        {

                        }
                        else if (!NormalsEqual(ReassignedMesh.faces[i].Normal1, VectorPoint[TempID].normal) && !NormalAverage)
                        {

                        }
                        else if (!UVEqual(ReassignedMesh.faces[i].UV1, VectorPoint[TempID].TextureCord))
                        {

                        }
                        else if (ReassignedMesh.faces[i].Weight1Pos != VectorPoint[TempID].Weight)
                        {

                        }
                        else
                        {
                            TempFace.Id1 = TempID;
                            Test = true;
                        }
                    }
                    else
                    {
                        TempFace.Id1 = VectorPoint.Count;

                        VectorPoint.Add(GenerateVectorPoint(ReassignedMesh.faces[i], 1));
                        Test = true;
                    }
                }
                #endregion

                Test = false;
                TempID = 0;
                TestID = 0;
                #region Point 2
                while (!Test)
                {
                    TempID = ContainsVertice(ReassignedMesh.faces[i].V2, VectorPoint, TestID);
                    if (TempID == -1)
                    {
                        TempFace.Id2 = VectorPoint.Count;

                        VectorPoint.Add(GenerateVectorPoint(ReassignedMesh.faces[i], 2));

                        Test = true;
                    }
                    else if (!Shadow)
                    {
                        TestID++;
                        if (ReassignedMesh.faces[i].MaterialID != VectorPoint[TempID].Material)
                        {

                        }
                        else if (!NormalsEqual(ReassignedMesh.faces[i].Normal2, VectorPoint[TempID].normal) && !NormalAverage)
                        {

                        }
                        else if (!UVEqual(ReassignedMesh.faces[i].UV2, VectorPoint[TempID].TextureCord))
                        {

                        }
                        else if (ReassignedMesh.faces[i].Weight2Pos != VectorPoint[TempID].Weight)
                        {

                        }
                        else
                        {
                            TempFace.Id2 = TempID;
                            Test = true;
                        }
                    }
                    else
                    {
                        TempFace.Id2 = VectorPoint.Count;

                        VectorPoint.Add(GenerateVectorPoint(ReassignedMesh.faces[i], 2));
                        Test = true;
                    }
                }
                #endregion

                Test = false;
                TempID = 0;
                TestID = 0;
                #region Point 3
                while (!Test)
                {
                    TempID = ContainsVertice(ReassignedMesh.faces[i].V3, VectorPoint, TestID);
                    if (TempID == -1)
                    {
                        TempFace.Id3 = VectorPoint.Count;

                        VectorPoint.Add(GenerateVectorPoint(ReassignedMesh.faces[i], 3));

                        Test = true;
                    }
                    else if (!Shadow)
                    {
                        TestID++;
                        if (ReassignedMesh.faces[i].MaterialID != VectorPoint[TempID].Material)
                        {

                        }
                        else if (!NormalsEqual(ReassignedMesh.faces[i].Normal3, VectorPoint[TempID].normal) && !NormalAverage)
                        {

                        }
                        else if (!UVEqual(ReassignedMesh.faces[i].UV3, VectorPoint[TempID].TextureCord))
                        {

                        }
                        else if (ReassignedMesh.faces[i].Weight3Pos != VectorPoint[TempID].Weight)
                        {

                        }
                        else
                        {
                            TempFace.Id3 = TempID;
                            Test = true;
                        }
                    }
                    else
                    {
                        TempFace.Id3 = VectorPoint.Count;

                        VectorPoint.Add(GenerateVectorPoint(ReassignedMesh.faces[i], 3));
                        Test = true;
                    }
                }
                #endregion

                indiceFaces.Add(TempFace);
            }

            indiceFaces = TristripGenerator.NeighbourPriority(indiceFaces);

            //Send to Tristrip Generator
            List<TristripGenerator.IndiceTristrip> indiceTristrips = TristripGenerator.GenerateTristripNivda(indiceFaces);

            //Static mesh that shit
            TempTrickyMesh.MeshGroups = new List<TrickyMPFModelHandler.GroupMainHeader>();
            List<TrickyMPFModelHandler.StaticMesh> meshList = new List<TrickyMPFModelHandler.StaticMesh>();

            for (int a = 0; a < TempTrickyMesh.materialDatas.Count; a++)
            {
                TrickyMPFModelHandler.StaticMesh staticMesh = new TrickyMPFModelHandler.StaticMesh();
                staticMesh.Unknown1 = 14;
                staticMesh.Unknown2 = 114;
                staticMesh.MatieralID = a;
                staticMesh.vertices = new List<Vector3>();
                staticMesh.uvNormals = new List<Vector3>();
                staticMesh.Strips = new List<int>();
                staticMesh.uv = new List<Vector4>();
                staticMesh.Weights = new List<int>();
                for (int i = 0; i < indiceTristrips.Count; i++)
                {
                    if (indiceTristrips[i].MaterialID == a)
                    {
                        if (staticMesh.vertices.Count + indiceTristrips[i].Indices.Count <= 55 /*&& staticMesh.Strips.Count<10*/)
                        {
                            staticMesh.Strips.Add(indiceTristrips[i].Indices.Count);
                            for (int d = 0; d < indiceTristrips[i].Indices.Count; d++)
                            {
                                staticMesh.vertices.Add(VectorPoint[indiceTristrips[i].Indices[d]].vector);
                                staticMesh.uv.Add(VectorPoint[indiceTristrips[i].Indices[d]].TextureCord);
                                staticMesh.uvNormals.Add(VectorPoint[indiceTristrips[i].Indices[d]].normal);
                                staticMesh.Weights.Add(VectorPoint[indiceTristrips[i].Indices[d]].Weight);
                            }
                        }
                        else
                        {
                            meshList.Add(staticMesh);
                            staticMesh = new TrickyMPFModelHandler.StaticMesh();
                            staticMesh.Unknown1 = 14;
                            staticMesh.Unknown2 = 114;
                            staticMesh.vertices = new List<Vector3>();
                            staticMesh.uvNormals = new List<Vector3>();
                            staticMesh.Strips = new List<int>();
                            staticMesh.uv = new List<Vector4>();
                            staticMesh.Weights = new List<int>();
                            staticMesh.Strips.Add(indiceTristrips[i].Indices.Count);
                            for (int d = 0; d < indiceTristrips[i].Indices.Count; d++)
                            {
                                staticMesh.vertices.Add(VectorPoint[indiceTristrips[i].Indices[d]].vector);
                                staticMesh.uv.Add(VectorPoint[indiceTristrips[i].Indices[d]].TextureCord);
                                staticMesh.uvNormals.Add(VectorPoint[indiceTristrips[i].Indices[d]].normal);
                                staticMesh.Weights.Add(VectorPoint[indiceTristrips[i].Indices[d]].Weight);
                            }
                        }
                    }
                }
                if (staticMesh.vertices.Count!=0)
                {
                    meshList.Add(staticMesh);
                }
            }

            //Group That Shit
            for (int i = 0; i < meshList.Count; i++)
            {
                bool TestIfExists = false;

                for (int a = 0; a < TempTrickyMesh.MeshGroups.Count; a++)
                {
                    var TempGroup = TempTrickyMesh.MeshGroups[a];
                    if (TempGroup.MaterialID == meshList[i].MatieralID)
                    {
                        TestIfExists = true;
                        var TempSubGroup = TempGroup.meshGroupSubs[0];

                        var TempMorphMeshGroup = TempSubGroup.MeshGroupHeaders[0];

                        TempMorphMeshGroup.staticMesh.Add(meshList[i]);

                        TempSubGroup.MeshGroupHeaders[0] = TempMorphMeshGroup;

                        TempGroup.meshGroupSubs[0] = TempSubGroup;
                    }
                    TempTrickyMesh.MeshGroups[a] = TempGroup;
                }

                if(!TestIfExists)
                {
                    TrickyMPFModelHandler.GroupMainHeader TempHeader = new TrickyMPFModelHandler.GroupMainHeader();
                    if(Shadow)
                    {
                        TempHeader.GroupType = 17;
                    }
                    else
                    {
                        TempHeader.GroupType = 1;
                    }
                    TempHeader.MaterialID = meshList[i].MatieralID;
                    TempHeader.Unknown = -1;
                    TempHeader.meshGroupSubs = new List<TrickyMPFModelHandler.GroupSubHeader>();
                    TrickyMPFModelHandler.GroupSubHeader TempGroupSub = new TrickyMPFModelHandler.GroupSubHeader();
                    TempGroupSub.MeshGroupHeaders = new List<TrickyMPFModelHandler.MeshMorphHeader>();
                    TrickyMPFModelHandler.MeshMorphHeader TempMeshGroupHeaders = new TrickyMPFModelHandler.MeshMorphHeader();
                    TempMeshGroupHeaders.staticMesh = new List<TrickyMPFModelHandler.StaticMesh>();
                    TempMeshGroupHeaders.staticMesh.Add(meshList[i]);

                    TempGroupSub.MeshGroupHeaders.Add(TempMeshGroupHeaders);
                    TempHeader.meshGroupSubs.Add(TempGroupSub);
                    TempTrickyMesh.MeshGroups.Add(TempHeader);
                }
            }


            //Generate Number Ref and correct UV
            //Prephaps Move into static meshing
            TempTrickyMesh.numberListRefs = new List<TrickyMPFModelHandler.NumberListRef>();
            for (int i = 0; i < TempTrickyMesh.MeshGroups.Count; i++)
            {
                var TempMeshGroup = TempTrickyMesh.MeshGroups[i];
                for (int a = 0; a < TempMeshGroup.meshGroupSubs.Count; a++)
                {
                    var TempSubGroup = TempMeshGroup.meshGroupSubs[a];
                    for (int b = 0; b < TempSubGroup.MeshGroupHeaders.Count; b++)
                    {
                        var TempMeshGroupHeader = TempSubGroup.MeshGroupHeaders[b];
                        TrickyMPFModelHandler.NumberListRef NumberRef = new TrickyMPFModelHandler.NumberListRef();
                        NumberRef.WeightIDs = new List<int>();

                        for (int c = 0; c < TempMeshGroupHeader.staticMesh.Count; c++)
                        {
                            var TempMesh = TempMeshGroupHeader.staticMesh[c];
                            for (int d = 0; d < TempMesh.Weights.Count; d++)
                            {
                                if (!NumberRef.WeightIDs.Contains(TempMesh.Weights[d]))
                                {
                                    NumberRef.WeightIDs.Add(TempMesh.Weights[d]);
                                }
                                TempMesh.Weights[d] = NumberRef.WeightIDs.IndexOf(TempMesh.Weights[d]);
                            }

                            if(!Shadow)
                            {
                                for (int d = 0; d < TempMesh.Weights.Count; d++)
                                {
                                    var TempUV = TempMesh.uv[d];
                                    TempUV.Z = TempMesh.Weights[d]*4 + 14;
                                    TempUV.W = TempMesh.Weights[d]*3 + 114; //Figure Out
                                    TempMesh.uv[d] = TempUV;
                                }
                            }
                            else
                            {
                                for (int d = 0; d < TempMesh.Weights.Count; d++)
                                {
                                    TempMesh.Weights[d] = TempMesh.Weights[d] * 4 + 14;
                                }
                            }
                            TempMeshGroupHeader.staticMesh[c] = TempMesh;
                        }

                        TempTrickyMesh.numberListRefs.Add(NumberRef);

                        TempSubGroup.MeshGroupHeaders[b] = TempMeshGroupHeader;
                    }
                    TempMeshGroup.meshGroupSubs[a]=TempSubGroup;
                }
                TempTrickyMesh.MeshGroups[i] = TempMeshGroup;
            }

            //Update IK Points
            TempTrickyMesh.iKPoints = ReassignedMesh.IKPoints;



            Board.ModelList[Selected] = TempTrickyMesh;
        }

        public void StartRegenMeshCharacter(TrickyModelCombiner trickyModelCombiner, int MeshID)
        {
            if (trickyModelCombiner.bones == null)
            {
                MessageBox.Show("No Bones Detected");
                return;
            }
            bones = new List<TrickyMPFModelHandler.BoneData>();
            for (int i = 0; i < Body.ModelList.Count; i++)
            {
                if ((MeshID == 0 && Body.ModelList[i].FileName.Contains("3000")) ||
                    (MeshID == 1 && Body.ModelList[i].FileName.Contains("1500")) ||
                    (MeshID == 2 && Body.ModelList[i].FileName.Contains("750") && !Body.ModelList[i].FileName.ToLower().Contains("shdw")) ||
                    (MeshID == 3 && Body.ModelList[i].FileName.ToLower().Contains("shdw")))
                {
                    bones.AddRange(Body.ModelList[i].boneDatas);
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
                    bones.AddRange(Head.ModelList[i].boneDatas);
                }
            }

            bool Shadow = false;

            if(MeshID==3)
            {
                Shadow = true;
            }

            if (trickyModelCombiner.bones.Count != bones.Count)
            {
                MessageBox.Show("Incorrect Ammount of Bones");
                return;
            }

            int TestNum = 0;
            //Check That Mesh Ammount and Names Are Correct Attaching if they are body and there mesh id
            //Set if mesh contains morph or is shadow making sure that morph contains morph points
            for (int i = 0; i < trickyModelCombiner.reassignedMesh.Count; i++)
            {
                var TempReMesh = trickyModelCombiner.reassignedMesh[i];

                if (TempReMesh.faces[0].MorphPoint1.Count != 0)
                {
                    TempReMesh.MorphTargetCount = TempReMesh.faces[0].MorphPoint1.Count;
                }

                for (int a = 0; a < Body.ModelList.Count; a++)
                {
                    if (Body.ModelList[a].FileName.ToLower() == trickyModelCombiner.reassignedMesh[i].MeshName.ToLower())
                    {
                        TestNum++;
                        TempReMesh.BodyHead = false;
                        TempReMesh.MeshId = a;
                        break;
                    }
                }

                for (int a = 0; a < Head.ModelList.Count; a++)
                {
                    if (Head.ModelList[a].FileName.ToLower() == trickyModelCombiner.reassignedMesh[i].MeshName.ToLower())
                    {
                        TestNum++;
                        TempReMesh.BodyHead = true;
                        TempReMesh.MeshId = a;

                        if (TempReMesh.MorphTargetCount!= Head.ModelList[a].MorphKeyCount)
                        {
                            MessageBox.Show("Incorrect ammount of Shapekeys");
                            return;
                        }

                        break;
                    }
                }

                trickyModelCombiner.reassignedMesh[i] = TempReMesh;
            }

            if(TestNum!=trickyModelCombiner.reassignedMesh.Count)
            {
                if (TestNum > trickyModelCombiner.reassignedMesh.Count)
                {
                    MessageBox.Show("Idk how you even got this error");
                }
                else
                {
                    MessageBox.Show("Incorrectly Named Or Unneeded mesh parts detected.");
                }
                return;
            }


            //For Each Mesh
            for (int i = 0; i < trickyModelCombiner.reassignedMesh.Count; i++)
            {
                var TempReMesh = trickyModelCombiner.reassignedMesh[i];

                TrickyMPFModelHandler.MPFModelHeader TempTrickyMesh = new TrickyMPFModelHandler.MPFModelHeader();

                if(!TempReMesh.BodyHead)
                {
                    TempTrickyMesh = Body.ModelList[TempReMesh.MeshId];
                }
                else
                {
                    TempTrickyMesh = Head.ModelList[TempReMesh.MeshId];
                }


                //Regenerate Materials

                //Update Bones

                //Generate Weight List
                //Redo Data In Correct Formats IE make Weight List and make faces use the positions.
                TempTrickyMesh.boneWeightHeader = new List<TrickyMPFModelHandler.BoneWeightHeader>();

                //Load Headers into file
                for (int a = 0; a < TempReMesh.faces.Count; a++)
                {
                    var TempFace = TempReMesh.faces[a];
                    int WeightID = ContainsWeight(TempFace.Weight1, TempTrickyMesh.boneWeightHeader);
                    if (WeightID == -1)
                    {
                        TempTrickyMesh.boneWeightHeader.Add(TempFace.Weight1);
                        WeightID = TempTrickyMesh.boneWeightHeader.Count - 1;
                    }
                    TempFace.Weight1Pos = WeightID;

                    WeightID = ContainsWeight(TempFace.Weight2, TempTrickyMesh.boneWeightHeader);
                    if (WeightID == -1)
                    {
                        TempTrickyMesh.boneWeightHeader.Add(TempFace.Weight2);
                        WeightID = TempTrickyMesh.boneWeightHeader.Count - 1;
                    }
                    TempFace.Weight2Pos = WeightID;

                    WeightID = ContainsWeight(TempFace.Weight3, TempTrickyMesh.boneWeightHeader);
                    if (WeightID == -1)
                    {
                        TempTrickyMesh.boneWeightHeader.Add(TempFace.Weight3);
                        WeightID = TempTrickyMesh.boneWeightHeader.Count - 1;
                    }
                    TempFace.Weight3Pos = WeightID;

                    TempReMesh.faces[a] = TempFace;
                }

                //Fix Bone ID/FileIDs
                for (int a = 0; a < TempTrickyMesh.boneWeightHeader.Count; a++)
                {
                    var TempBoneHeader = TempTrickyMesh.boneWeightHeader[a];
                    for (int b = 0; b < TempBoneHeader.boneWeights.Count; b++)
                    {
                        var TempBoneWeight = TempBoneHeader.boneWeights[b];

                        var TempBone = FindBone(bones, TempBoneWeight.boneName);

                        TempBoneWeight.BoneID = TempBone.BonePos;
                        TempBoneWeight.FileID = TempBone.FileID;

                        TempBoneHeader.boneWeights[b] = TempBoneWeight;
                    }
                    TempTrickyMesh.boneWeightHeader[a] = TempBoneHeader;
                }

                //Take faces and Generate Indce faces and giant vertex list for each material
                List<VectorPoint> VectorPoint = new List<VectorPoint>();
                List<TristripGenerator.IndiceFace> indiceFaces = new List<TristripGenerator.IndiceFace>();

                for (int a = 0; a < TempReMesh.faces.Count; a++)
                {
                    TristripGenerator.IndiceFace TempFace = new TristripGenerator.IndiceFace();
                    bool Test = false;
                    int TempID = 0;
                    int TestID = 0;
                    #region Point 1
                    while (!Test)
                    {
                        TempID = ContainsVertice(TempReMesh.faces[a].V1, VectorPoint, TestID);
                        if (TempID == -1)
                        {
                            TempFace.Id1 = VectorPoint.Count;

                            VectorPoint.Add(GenerateVectorPoint(TempReMesh.faces[a], 1));

                            Test = true;
                        }
                        else if (!Shadow)
                        {
                            TestID++;
                            if (TempReMesh.faces[a].MaterialID != VectorPoint[TempID].Material)
                            {

                            }
                            else if (!NormalsEqual(TempReMesh.faces[a].Normal1, VectorPoint[TempID].normal) && !NormalAverage)
                            {

                            }
                            else if (!UVEqual(TempReMesh.faces[a].UV1, VectorPoint[TempID].TextureCord))
                            {

                            }
                            else if (TempReMesh.faces[a].Weight1Pos != VectorPoint[TempID].Weight)
                            {

                            }
                            else
                            {
                                if (TempReMesh.MorphTargetCount != 0)
                                {
                                    if (!MorphPointsEqual(TempReMesh.faces[a].MorphPoint1, VectorPoint[TempID].MorphData))
                                    {

                                    }
                                    else
                                    {
                                        TempFace.Id1 = TempID;
                                        Test = true;
                                    }
                                }
                                else
                                {
                                    TempFace.Id1 = TempID;
                                    Test = true;
                                }
                            }
                        }
                        else
                        {
                            TempFace.Id1 = VectorPoint.Count;

                            VectorPoint.Add(GenerateVectorPoint(TempReMesh.faces[a], 1));
                            Test = true;
                        }
                    }
                    #endregion

                    Test = false;
                    TempID = 0;
                    TestID = 0;
                    #region Point 2
                    while (!Test)
                    {
                        TempID = ContainsVertice(TempReMesh.faces[a].V2, VectorPoint, TestID);
                        if (TempID == -1)
                        {
                            TempFace.Id2 = VectorPoint.Count;

                            VectorPoint.Add(GenerateVectorPoint(TempReMesh.faces[a], 2));

                            Test = true;
                        }
                        else if (!Shadow)
                        {
                            TestID++;
                            if (TempReMesh.faces[a].MaterialID != VectorPoint[TempID].Material)
                            {

                            }
                            else if (!NormalsEqual(TempReMesh.faces[a].Normal2, VectorPoint[TempID].normal) && !NormalAverage)
                            {

                            }
                            else if (!UVEqual(TempReMesh.faces[a].UV2, VectorPoint[TempID].TextureCord))
                            {

                            }
                            else if (TempReMesh.faces[a].Weight2Pos != VectorPoint[TempID].Weight)
                            {

                            }
                            else
                            {
                                if (TempReMesh.MorphTargetCount != 0)
                                {
                                    if (!MorphPointsEqual(TempReMesh.faces[a].MorphPoint2, VectorPoint[TempID].MorphData))
                                    {

                                    }
                                    else
                                    {
                                        TempFace.Id2 = TempID;
                                        Test = true;
                                    }
                                }
                                else
                                {
                                    TempFace.Id2 = TempID;
                                    Test = true;
                                }
                            }
                        }
                        else
                        {
                            TempFace.Id2 = VectorPoint.Count;

                            VectorPoint.Add(GenerateVectorPoint(TempReMesh.faces[a], 2));
                            Test = true;
                        }
                    }
                    #endregion

                    Test = false;
                    TempID = 0;
                    TestID = 0;
                    #region Point 3
                    while (!Test)
                    {
                        TempID = ContainsVertice(TempReMesh.faces[a].V3, VectorPoint, TestID);
                        if (TempID == -1)
                        {
                            TempFace.Id3 = VectorPoint.Count;

                            VectorPoint.Add(GenerateVectorPoint(TempReMesh.faces[a], 3));

                            Test = true;
                        }
                        else if (!Shadow)
                        {
                            TestID++;
                            if (TempReMesh.faces[a].MaterialID != VectorPoint[TempID].Material)
                            {

                            }
                            else if (!NormalsEqual(TempReMesh.faces[a].Normal3, VectorPoint[TempID].normal) && !NormalAverage)
                            {

                            }
                            else if (!UVEqual(TempReMesh.faces[a].UV3, VectorPoint[TempID].TextureCord))
                            {

                            }
                            else if (TempReMesh.faces[a].Weight3Pos != VectorPoint[TempID].Weight)
                            {

                            }
                            else
                            {
                                if (TempReMesh.MorphTargetCount != 0)
                                {
                                    if (!MorphPointsEqual(TempReMesh.faces[a].MorphPoint3, VectorPoint[TempID].MorphData))
                                    {

                                    }
                                    else
                                    {
                                        TempFace.Id3 = TempID;
                                        Test = true;
                                    }
                                }
                                else
                                {
                                    TempFace.Id3 = TempID;
                                    Test = true;
                                }
                            }
                        }
                        else
                        {
                            TempFace.Id3 = VectorPoint.Count;

                            VectorPoint.Add(GenerateVectorPoint(TempReMesh.faces[a], 3));
                            Test = true;
                        }
                    }
                    #endregion

                    indiceFaces.Add(TempFace);
                }

                indiceFaces = TristripGenerator.NeighbourPriority(indiceFaces);

                //Send to Tristrip Generator
                List<TristripGenerator.IndiceTristrip> indiceTristrips = TristripGenerator.GenerateTristripNivda(indiceFaces);

                //Static mesh that shit
                TempTrickyMesh.MeshGroups = new List<TrickyMPFModelHandler.GroupMainHeader>();
                List<TrickyMPFModelHandler.StaticMesh> meshList = new List<TrickyMPFModelHandler.StaticMesh>();

                for (int a = 0; a < TempTrickyMesh.materialDatas.Count; a++)
                {
                    TrickyMPFModelHandler.StaticMesh staticMesh = new TrickyMPFModelHandler.StaticMesh();
                    staticMesh.Unknown1 = 14;
                    staticMesh.Unknown2 = 114;
                    staticMesh.MatieralID = a;
                    staticMesh.vertices = new List<Vector3>();
                    staticMesh.uvNormals = new List<Vector3>();
                    staticMesh.Strips = new List<int>();
                    staticMesh.uv = new List<Vector4>();
                    staticMesh.Weights = new List<int>();
                    staticMesh.MorphKeys = new List<TrickyMPFModelHandler.MorphKey>();

                    if(TempReMesh.MorphTargetCount!=0)
                    {
                        for (int b = 0; b < TempReMesh.MorphTargetCount; b++)
                        {
                            var NewKey = new TrickyMPFModelHandler.MorphKey();
                            NewKey.morphData = new List<Vector3>();
                            staticMesh.MorphKeys.Add(NewKey);
                        }
                    }

                    for (int b = 0; b < indiceTristrips.Count; b++)
                    {
                        if (VectorPoint[indiceTristrips[b].Indices[0]].Material== a)
                        {
                            if (staticMesh.vertices.Count + indiceTristrips[b].Indices.Count <= 55 /*&& staticMesh.Strips.Count<10*/)
                            {
                                staticMesh.Strips.Add(indiceTristrips[b].Indices.Count);
                                for (int d = 0; d < indiceTristrips[b].Indices.Count; d++)
                                {
                                    staticMesh.vertices.Add(VectorPoint[indiceTristrips[b].Indices[d]].vector);
                                    staticMesh.uv.Add(VectorPoint[indiceTristrips[b].Indices[d]].TextureCord);
                                    staticMesh.uvNormals.Add(VectorPoint[indiceTristrips[b].Indices[d]].normal);
                                    staticMesh.Weights.Add(VectorPoint[indiceTristrips[b].Indices[d]].Weight);

                                    if (TempReMesh.MorphTargetCount != 0)
                                    {
                                        var TempMorphTargets = VectorPoint[indiceTristrips[b].Indices[d]].MorphData;

                                        for (int ei = 0; ei < TempReMesh.MorphTargetCount; ei++)
                                        {
                                            staticMesh.MorphKeys[ei].morphData.Add(TempMorphTargets[ei]);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                meshList.Add(staticMesh);
                                staticMesh = new TrickyMPFModelHandler.StaticMesh();
                                staticMesh.Unknown1 = 14;
                                staticMesh.Unknown2 = 114;
                                staticMesh.vertices = new List<Vector3>();
                                staticMesh.uvNormals = new List<Vector3>();
                                staticMesh.Strips = new List<int>();
                                staticMesh.uv = new List<Vector4>();
                                staticMesh.Weights = new List<int>();
                                staticMesh.MorphKeys = new List<TrickyMPFModelHandler.MorphKey>();

                                if (TempReMesh.MorphTargetCount != 0)
                                {
                                    for (int c = 0; c < TempReMesh.MorphTargetCount; c++)
                                    {
                                        var NewKey = new TrickyMPFModelHandler.MorphKey();
                                        NewKey.morphData = new List<Vector3>();
                                        staticMesh.MorphKeys.Add(NewKey);
                                    }
                                }

                                staticMesh.Strips.Add(indiceTristrips[b].Indices.Count);
                                for (int d = 0; d < indiceTristrips[b].Indices.Count; d++)
                                {
                                    staticMesh.vertices.Add(VectorPoint[indiceTristrips[b].Indices[d]].vector);
                                    staticMesh.uv.Add(VectorPoint[indiceTristrips[b].Indices[d]].TextureCord);
                                    staticMesh.uvNormals.Add(VectorPoint[indiceTristrips[b].Indices[d]].normal);
                                    staticMesh.Weights.Add(VectorPoint[indiceTristrips[b].Indices[d]].Weight);

                                    if(TempReMesh.MorphTargetCount!=0)
                                    {
                                        var TempMorphTargets = VectorPoint[indiceTristrips[b].Indices[d]].MorphData;

                                        for (int ei = 0; ei < TempReMesh.MorphTargetCount; ei++)
                                        {
                                            staticMesh.MorphKeys[ei].morphData.Add(TempMorphTargets[ei]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (staticMesh.vertices.Count != 0)
                    {
                        meshList.Add(staticMesh);
                    }
                }

                //Group That Shit
                for (int b = 0; b < meshList.Count; b++)
                {
                    bool TestIfExists = false;

                    for (int a = 0; a < TempTrickyMesh.MeshGroups.Count; a++)
                    {
                        var TempGroup = TempTrickyMesh.MeshGroups[a];
                        if (TempGroup.MaterialID == meshList[b].MatieralID)
                        {
                            TestIfExists = true;
                            var TempSubGroup = TempGroup.meshGroupSubs[0];

                            if (TempReMesh.MorphTargetCount == 0)
                            {
                                var TempMorphMeshGroup = TempSubGroup.MeshGroupHeaders[0];

                                TempMorphMeshGroup.staticMesh.Add(meshList[b]);

                                TempSubGroup.MeshGroupHeaders[0] = TempMorphMeshGroup;

                                TempGroup.meshGroupSubs[0] = TempSubGroup;
                            }
                            else
                            {
                                var TempMorphMeshGroup = new TrickyMPFModelHandler.MeshMorphHeader();
                                TempMorphMeshGroup.staticMesh = new List<TrickyMPFModelHandler.StaticMesh>();
                                TempMorphMeshGroup.MorphKeyList = meshList[b].MorphKeys;
                                TempMorphMeshGroup.staticMesh.Add(meshList[b]);
                                TempSubGroup.MeshGroupHeaders.Add(TempMorphMeshGroup);
                                TempGroup.meshGroupSubs[0] = TempSubGroup;
                            }
                        }
                        TempTrickyMesh.MeshGroups[a] = TempGroup;
                    }

                    if (!TestIfExists)
                    {
                        TrickyMPFModelHandler.GroupMainHeader TempHeader = new TrickyMPFModelHandler.GroupMainHeader();
                        if (Shadow)
                        {
                            TempHeader.GroupType = 17;
                        }
                        else if (TempReMesh.MorphTargetCount!=0)
                        {
                            TempHeader.GroupType = 256;
                        }
                        else
                        {
                            TempHeader.GroupType = 1;
                        }
                        TempHeader.MaterialID = meshList[b].MatieralID;
                        TempHeader.Unknown = -1;
                        TempHeader.meshGroupSubs = new List<TrickyMPFModelHandler.GroupSubHeader>();
                        TrickyMPFModelHandler.GroupSubHeader TempGroupSub = new TrickyMPFModelHandler.GroupSubHeader();
                        TempGroupSub.MeshGroupHeaders = new List<TrickyMPFModelHandler.MeshMorphHeader>();
                        TrickyMPFModelHandler.MeshMorphHeader TempMeshGroupHeaders = new TrickyMPFModelHandler.MeshMorphHeader();
                        TempMeshGroupHeaders.staticMesh = new List<TrickyMPFModelHandler.StaticMesh>();
                        TempMeshGroupHeaders.MorphKeyList = meshList[b].MorphKeys;
                        TempMeshGroupHeaders.staticMesh.Add(meshList[b]);

                        TempGroupSub.MeshGroupHeaders.Add(TempMeshGroupHeaders);
                        TempHeader.meshGroupSubs.Add(TempGroupSub);
                        TempTrickyMesh.MeshGroups.Add(TempHeader);
                    }
                }

                //Generate Number Ref and correct UV
                TempTrickyMesh.numberListRefs = new List<TrickyMPFModelHandler.NumberListRef>();
                for (int ei = 0; ei < TempTrickyMesh.MeshGroups.Count; ei++)
                {
                    var TempMeshGroup = TempTrickyMesh.MeshGroups[ei];
                    for (int a = 0; a < TempMeshGroup.meshGroupSubs.Count; a++)
                    {
                        var TempSubGroup = TempMeshGroup.meshGroupSubs[a];
                        for (int b = 0; b < TempSubGroup.MeshGroupHeaders.Count; b++)
                        {
                            var TempMeshGroupHeader = TempSubGroup.MeshGroupHeaders[b];
                            TrickyMPFModelHandler.NumberListRef NumberRef = new TrickyMPFModelHandler.NumberListRef();
                            NumberRef.WeightIDs = new List<int>();

                            for (int c = 0; c < TempMeshGroupHeader.staticMesh.Count; c++)
                            {
                                var TempMesh = TempMeshGroupHeader.staticMesh[c];
                                for (int d = 0; d < TempMesh.Weights.Count; d++)
                                {
                                    if (!NumberRef.WeightIDs.Contains(TempMesh.Weights[d]))
                                    {
                                        NumberRef.WeightIDs.Add(TempMesh.Weights[d]);
                                    }
                                    TempMesh.Weights[d] = NumberRef.WeightIDs.IndexOf(TempMesh.Weights[d]);
                                }

                                if (!Shadow)
                                {
                                    for (int d = 0; d < TempMesh.Weights.Count; d++)
                                    {
                                        var TempUV = TempMesh.uv[d];
                                        TempUV.Z = TempMesh.Weights[d] * 4 + 14;
                                        TempUV.W = TempMesh.Weights[d] * 3 + 114; //Figure Out
                                        TempMesh.uv[d] = TempUV;
                                    }
                                }
                                else
                                {
                                    for (int d = 0; d < TempMesh.Weights.Count; d++)
                                    {
                                        TempMesh.Weights[d] = TempMesh.Weights[d] * 4 + 14;
                                    }
                                }
                                TempMeshGroupHeader.staticMesh[c] = TempMesh;
                            }

                            TempTrickyMesh.numberListRefs.Add(NumberRef);

                            TempSubGroup.MeshGroupHeaders[b] = TempMeshGroupHeader;
                        }
                        TempMeshGroup.meshGroupSubs[a] = TempSubGroup;
                    }
                    TempTrickyMesh.MeshGroups[ei] = TempMeshGroup;
                }



                if (!TempReMesh.BodyHead)
                {
                    Body.ModelList[TempReMesh.MeshId] = TempTrickyMesh;
                }
                else
                {
                    Head.ModelList[TempReMesh.MeshId] = TempTrickyMesh;
                }
                trickyModelCombiner.reassignedMesh[i] = TempReMesh;
            }
            MessageBox.Show("Import Sucessful");
        }



        static VectorPoint GenerateVectorPoint(TrickyMPFModelHandler.Face face, int Vertice)
        {
            VectorPoint vectorPoint = new VectorPoint();
            vectorPoint.Material = face.MaterialID;

            if(Vertice==1)
            {
                vectorPoint.vector = face.V1;
                vectorPoint.normal = face.Normal1;
                vectorPoint.TextureCord = face.UV1;
                vectorPoint.Weight = face.Weight1Pos;
                vectorPoint.MorphData = face.MorphPoint1;
            }

            if (Vertice == 2)
            {
                vectorPoint.vector = face.V2;
                vectorPoint.normal = face.Normal2;
                vectorPoint.TextureCord = face.UV2;
                vectorPoint.Weight = face.Weight2Pos;
                vectorPoint.MorphData = face.MorphPoint2;
            }

            if (Vertice == 3)
            {
                vectorPoint.vector = face.V3;
                vectorPoint.normal = face.Normal3;
                vectorPoint.TextureCord = face.UV3;
                vectorPoint.Weight = face.Weight3Pos;
                vectorPoint.MorphData = face.MorphPoint3;
            }

            return vectorPoint;
        }

        static int ContainsVertice(Vector3 vector1, List<VectorPoint> Point, int TestInt = 0)
        {
            int TestID = 0;
            for (int i = 0; i < Point.Count; i++)
            {
                if (vector1.X <= Point[i].vector.X + 0.01f && vector1.X >= Point[i].vector.X - 0.01f && vector1.Y <= Point[i].vector.Y + 0.01f && vector1.Y >= Point[i].vector.Y - 0.01f && vector1.Z <= Point[i].vector.Z + 0.01f && vector1.Z >= Point[i].vector.Z - 0.01f)
                {
                    if (TestID == TestInt)
                    {
                        return i;
                    }
                    TestID++;
                }
            }
            return -1;
        }

        static bool MorphPointsEqual(List<Vector3> Vertex, List<Vector3> ListVertex)
        {
            int TestMain = 0;
            for (int i = 0; i < Vertex.Count; i++)
            {
                if (ListVertex[i] == Vertex[i])
                {
                    TestMain++;
                }
            }

            if(TestMain==Vertex.Count)
            {
                return true;
            }

            return false;
        }

        static bool NormalsEqual(Vector3 normal1, Vector3 normal2)
        {
            if ((int)(normal1.X * 32768f) == (int)(normal2.X * 32768f) && (int)(normal1.Y * 32768f) == (int)(normal2.Y * 32768f) && (int)(normal1.Z * 32768f) == (int)(normal2.Z * 32768f))
            {
                return true;
            }
            return false;
        }

        static bool UVEqual(Vector4 Uv1, Vector4 Uv2)
        {
            if ((int)(Uv1.X * 32768f) == (int)(Uv2.X * 32768f) && (int)(Uv1.Y * 32768f) == (int)(Uv2.Y * 32768f))
            {
                return true;
            }
            return false;
        }

        static int ContainsWeight(TrickyMPFModelHandler.BoneWeightHeader boneWeight, List<TrickyMPFModelHandler.BoneWeightHeader> boneWeightList)
        {
            for (int i = 0; i < boneWeightList.Count; i++)
            {
                if (boneWeightList[i].boneWeights.Count == boneWeight.boneWeights.Count)
                {
                    bool Test = false;
                    for (int a = 0; a < boneWeightList[i].boneWeights.Count; a++)
                    {
                        if (boneWeightList[i].boneWeights[a].Weight == boneWeight.boneWeights[a].Weight && boneWeightList[i].boneWeights[a].BoneID == boneWeight.boneWeights[a].BoneID && boneWeightList[i].boneWeights[a].FileID == boneWeight.boneWeights[a].FileID)
                        {
                            Test = true;
                        }
                    }
                    if (Test)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        static TrickyMPFModelHandler.BoneData FindBone(List<TrickyMPFModelHandler.BoneData> boneData, string BoneName)
        {
            for (int i = 0; i < boneData.Count; i++)
            {
                if (boneData[i].BoneName.ToLower()==BoneName.ToLower())
                {
                    return boneData[i];
                }
            }

            return new TrickyMPFModelHandler.BoneData();
        }

        public struct ReassignedMesh
        {
            public string MeshName;
            public int MeshId;
            public bool BodyHead;
            public bool ShadowModel;
            public int MorphTargetCount;
            public List<Vector3> IKPoints;
            public List<TrickyMPFModelHandler.Face> faces;
        }

        public struct VectorPoint
        {
            public bool Tristripped;
            public int Material;
            public Vector3 vector;
            public Vector3 normal;
            public Vector4 TextureCord;
            public int Weight;
            public List<Vector3> MorphData;
        }

    }
}
