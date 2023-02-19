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
        public bool NormalAverage;

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

        public int TristripCount(TrickyMPFModelHandler.MPFModelHeader modelHeader)
        {
            int Count = 0;

            var TempMesh = modelHeader.MeshGroups;

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

            return Count;
        }

        public int VerticeCount(TrickyMPFModelHandler.MPFModelHeader modelHeader)
        {
            int Count = 0;
            var TempMesh = modelHeader.MeshGroups;

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

            return Count;
        }

        public int ChunkCount(TrickyMPFModelHandler.MPFModelHeader modelHeader)
        {
            int Count = 0;
            var TempMesh = modelHeader.MeshGroups;

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

            return Count;
        }

        public int WeigthRefCount(TrickyMPFModelHandler.MPFModelHeader modelHeader)
        {
            int Count = 0;
            var TempMesh = modelHeader.MeshGroups;

            for (int i = 0; i < TempMesh.Count; i++)
            {
                Count += TempMesh[i].meshGroupSubs.Count;
            }

            return Count;
        }

        public int MorphGroupCount(TrickyMPFModelHandler.MPFModelHeader modelHeader)
        {
            int Count = 0;
            var TempMesh = modelHeader.MeshGroups;

            for (int i = 0; i < TempMesh.Count; i++)
            {
                for (int a = 0; a < TempMesh[i].meshGroupSubs.Count; a++)
                {
                    Count += TempMesh[i].meshGroupSubs[a].MeshGroupHeaders.Count;
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
            FixBoneParents();
        }

        public void FixBoneParents()
        {
            for (int i = 0; i < bones.Count; i++)
            {
                if (bones[i].ParentFileID != -1 && bones[i].ParentBone != -1)
                {
                    for (int a = 0; a < bones.Count; a++)
                    {
                        if (bones[i].ParentFileID == bones[a].FileID && bones[i].ParentBone == bones[a].BonePos)
                        {
                            var TempBone = bones[i];
                            TempBone.ParentBone = a;
                            bones[i] = TempBone;
                        }
                    }
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
                                var TempList = new TrickyMPFModelHandler.WeightRefList();
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

            //Check Materials Valid
            for (int i = 0; i < trickyModelCombiner.materials.Count; i++)
            {
                if (!trickyModelCombiner.materials[i].MainTexture.ToLower().Contains("bord"))
                {
                    MessageBox.Show("Invalid material " + trickyModelCombiner.materials[i].MainTexture);
                    return;
                }
            }

            //Regenerate Materials
            List<int> MaterialsID = new List<int>();
            List<int> RedoneMaterial = new List<int>();
            TempTrickyMesh.materialDatas = new List<TrickyMPFModelHandler.MaterialData>();
            for (int a = 0; a < ReassignedMesh.faces.Count; a++)
            {
                if (!MaterialsID.Contains(ReassignedMesh.faces[a].MaterialID))
                {
                    MaterialsID.Add(ReassignedMesh.faces[a].MaterialID);
                }
            }

            for (int a = 0; a < MaterialsID.Count; a++)
            {
                TrickyMPFModelHandler.MaterialData MaterialData = new TrickyMPFModelHandler.MaterialData();
                RedoneMaterial.Add(a);
                MaterialData.MainTexture = trickyModelCombiner.materials[MaterialsID[a]].MainTexture.Substring(0, 4).ToLower();
                MaterialData.Texture1 = "";
                MaterialData.Texture2 = "";
                MaterialData.Texture3 = "";
                MaterialData.Texture4 = "";

                string[] Split = trickyModelCombiner.materials[MaterialsID[a]].MainTexture.Split(' ');

                if (Split.Length > 1)
                {
                    if (Split[1].ToLower() == "gloss")
                    {
                        if (MaterialData.MainTexture == "bord")
                        {
                            MaterialData.Texture3 = "bd_g";
                        }
                    }
                    else if (Split[1].ToLower() == "envr")
                    {
                        MaterialData.Texture4 = "envr";
                    }

                }

                TempTrickyMesh.materialDatas.Add(MaterialData);
            }

            for (int a = 0; a < ReassignedMesh.faces.Count; a++)
            {
                int Index = MaterialsID.IndexOf(ReassignedMesh.faces[a].MaterialID);
                var TempFace = ReassignedMesh.faces[a];
                TempFace.MaterialID = Index;
                ReassignedMesh.faces[a] = TempFace;
            }

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

            for (int a = 0; a < ReassignedMesh.faces.Count; a++)
            {
                TristripGenerator.IndiceFace TempFace = new TristripGenerator.IndiceFace();
                bool Test = false;
                int TempID = 0;
                int TestID = 0;
                #region Point 1
                while (!Test)
                {
                    TempID = ContainsVertice(ReassignedMesh.faces[a].V1, VectorPoint, TestID);
                    if (TempID == -1)
                    {
                        TempFace.Id1 = VectorPoint.Count;

                        VectorPoint.Add(GenerateVectorPoint(ReassignedMesh.faces[a], 1));

                        Test = true;
                    }
                    else if (ReassignedMesh.faces[a].Weight1Pos != VectorPoint[TempID].Weight)
                    {
                        TestID++;
                    }
                    else if (!Shadow)
                    {
                        TestID++;
                        if (ReassignedMesh.faces[a].MaterialID != VectorPoint[TempID].Material)
                        {

                        }
                        else if (!NormalsEqual(ReassignedMesh.faces[a].Normal1, VectorPoint[TempID].normal) && !NormalAverage)
                        {

                        }
                        else if (!UVEqual(ReassignedMesh.faces[a].UV1, VectorPoint[TempID].TextureCord))
                        {

                        }
                        else
                        {
                            if (ReassignedMesh.MorphTargetCount != 0)
                            {
                                if (!MorphPointsEqual(ReassignedMesh.faces[a].MorphPoint1, VectorPoint[TempID].MorphData))
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
                        TempFace.Id1 = TempID;
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
                    TempID = ContainsVertice(ReassignedMesh.faces[a].V2, VectorPoint, TestID);
                    if (TempID == -1)
                    {
                        TempFace.Id2 = VectorPoint.Count;

                        VectorPoint.Add(GenerateVectorPoint(ReassignedMesh.faces[a], 2));

                        Test = true;
                    }
                    else if (ReassignedMesh.faces[a].Weight2Pos != VectorPoint[TempID].Weight)
                    {
                        TestID++;
                    }
                    else if (!Shadow)
                    {
                        TestID++;
                        if (ReassignedMesh.faces[a].MaterialID != VectorPoint[TempID].Material)
                        {

                        }
                        else if (!NormalsEqual(ReassignedMesh.faces[a].Normal2, VectorPoint[TempID].normal) && !NormalAverage)
                        {

                        }
                        else if (!UVEqual(ReassignedMesh.faces[a].UV2, VectorPoint[TempID].TextureCord))
                        {

                        }
                        else
                        {
                            if (ReassignedMesh.MorphTargetCount != 0)
                            {
                                if (!MorphPointsEqual(ReassignedMesh.faces[a].MorphPoint2, VectorPoint[TempID].MorphData))
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
                        TempFace.Id2 = TempID;
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
                    TempID = ContainsVertice(ReassignedMesh.faces[a].V3, VectorPoint, TestID);
                    if (TempID == -1)
                    {
                        TempFace.Id3 = VectorPoint.Count;

                        VectorPoint.Add(GenerateVectorPoint(ReassignedMesh.faces[a], 3));

                        Test = true;
                    }
                    else if (ReassignedMesh.faces[a].Weight3Pos != VectorPoint[TempID].Weight)
                    {
                        TestID++;
                    }
                    else if (!Shadow)
                    {
                        TestID++;
                        if (ReassignedMesh.faces[a].MaterialID != VectorPoint[TempID].Material)
                        {

                        }
                        else if (!NormalsEqual(ReassignedMesh.faces[a].Normal3, VectorPoint[TempID].normal) && !NormalAverage)
                        {

                        }
                        else if (!UVEqual(ReassignedMesh.faces[a].UV3, VectorPoint[TempID].TextureCord))
                        {

                        }
                        else
                        {
                            if (ReassignedMesh.MorphTargetCount != 0)
                            {
                                if (!MorphPointsEqual(ReassignedMesh.faces[a].MorphPoint3, VectorPoint[TempID].MorphData))
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
                        TempFace.Id3 = TempID;
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
            List<TrickyMPFModelHandler.MeshChunk> meshList = new List<TrickyMPFModelHandler.MeshChunk>();

            for (int a = 0; a < TempTrickyMesh.materialDatas.Count; a++)
            {
                while (true)
                {
                    TrickyMPFModelHandler.MeshChunk staticMesh = new TrickyMPFModelHandler.MeshChunk();
                    staticMesh.weightsInts = new List<int>();
                    staticMesh.Unknown1 = 14;
                    if (!Shadow)
                    {
                        staticMesh.Unknown2 = 114;
                    }
                    staticMesh.MatieralID = a;
                    staticMesh.vertices = new List<Vector3>();
                    staticMesh.uvNormals = new List<Vector3>();
                    staticMesh.Strips = new List<int>();
                    staticMesh.uv = new List<Vector4>();
                    staticMesh.Weights = new List<int>();
                    staticMesh.MorphKeys = new List<TrickyMPFModelHandler.MorphKey>();

                    if (TempTrickyMesh.MorphKeyCount != 0)
                    {
                        for (int b = 0; b < TempTrickyMesh.MorphKeyCount; b++)
                        {
                            var NewKey = new TrickyMPFModelHandler.MorphKey();
                            NewKey.morphData = new List<Vector3>();
                            staticMesh.MorphKeys.Add(NewKey);
                        }
                    }

                    for (int b = 0; b < indiceTristrips.Count; b++)
                    {
                        if (VectorPoint[indiceTristrips[b].Indices[0]].Material == staticMesh.MatieralID && !indiceTristrips[b].Used)
                        {
                            bool MeshGroupSubTest = false;
                            List<int> NewInts = new List<int>();
                            for (int c = 0; c < indiceTristrips[b].Indices.Count; c++)
                            {
                                if (!staticMesh.weightsInts.Contains(VectorPoint[indiceTristrips[b].Indices[c]].Weight) && !NewInts.Contains(VectorPoint[indiceTristrips[b].Indices[c]].Weight))
                                {
                                    NewInts.Add(VectorPoint[indiceTristrips[b].Indices[c]].Weight);
                                }
                            }

                            if (NewInts.Count + staticMesh.weightsInts.Count <= 25)
                            {
                                staticMesh.weightsInts.AddRange(NewInts);
                                MeshGroupSubTest = true;
                            }

                            if (staticMesh.vertices.Count + indiceTristrips[b].Indices.Count <= 55 && MeshGroupSubTest)
                            {
                                var TempIndice = indiceTristrips[b];
                                TempIndice.Used = true;
                                indiceTristrips[b] = TempIndice;

                                staticMesh.Strips.Add(indiceTristrips[b].Indices.Count);
                                for (int d = 0; d < indiceTristrips[b].Indices.Count; d++)
                                {
                                    staticMesh.vertices.Add(VectorPoint[indiceTristrips[b].Indices[d]].vector);
                                    staticMesh.uv.Add(VectorPoint[indiceTristrips[b].Indices[d]].TextureCord);
                                    staticMesh.uvNormals.Add(VectorPoint[indiceTristrips[b].Indices[d]].normal);
                                    staticMesh.Weights.Add(VectorPoint[indiceTristrips[b].Indices[d]].Weight);

                                    if (TempTrickyMesh.MorphKeyCount != 0)
                                    {
                                        var TempMorphTargets = VectorPoint[indiceTristrips[b].Indices[d]].MorphData;

                                        for (int ei = 0; ei < TempTrickyMesh.MorphKeyCount; ei++)
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
                    bool BreakoutTest = true;
                    for (int b = 0; b < indiceTristrips.Count; b++)
                    {
                        if (!indiceTristrips[b].Used && VectorPoint[indiceTristrips[b].Indices[0]].Material == staticMesh.MatieralID)
                        {
                            BreakoutTest = false;
                            break;
                        }
                    }
                    if (BreakoutTest)
                    {
                        break;
                    }
                }
            }

            //Group that shit MK2
            while (true)
            {
                bool FirstAdd = true;
                for (int b = 0; b < meshList.Count; b++)
                {
                    bool Added = false;

                    bool MaterialGroupTest = false;

                    for (int a = 0; a < TempTrickyMesh.MeshGroups.Count; a++)
                    {
                        if (meshList[b].MatieralID == TempTrickyMesh.MeshGroups[a].MaterialID)
                        {
                            MaterialGroupTest = true;
                            if (!meshList[b].Grouped)
                            {
                                var TempMaterialGroup = TempTrickyMesh.MeshGroups[a];
                                bool WeightRefGroupTest = false;

                                for (int c = 0; c < TempMaterialGroup.meshGroupSubs.Count; c++)
                                {
                                    var TempWeightRefGroup = TempMaterialGroup.meshGroupSubs[c];

                                    //Test the WeigthRef
                                    bool WeightRefTest = false;
                                    List<int> NewInts = new List<int>();
                                    for (int ce = 0; ce < meshList[b].Weights.Count; ce++)
                                    {
                                        if (!TempWeightRefGroup.weights.Contains(meshList[b].Weights[ce]) && !NewInts.Contains(meshList[b].Weights[ce]))
                                        {
                                            NewInts.Add(meshList[b].Weights[ce]);
                                        }
                                    }

                                    if (NewInts.Count + TempWeightRefGroup.weights.Count <= 25)
                                    {
                                        TempWeightRefGroup.weights.AddRange(NewInts);
                                        WeightRefTest = true;
                                    }

                                    if (WeightRefTest)
                                    {
                                        WeightRefGroupTest = true;
                                        var TempMesh = meshList[b];
                                        TempMesh.Grouped = true;
                                        Added = true;
                                        meshList[b] = TempMesh;
                                        //If Morph is true
                                        if (TempTrickyMesh.MorphKeyCount != 0)
                                        {
                                            //Add New
                                            TrickyMPFModelHandler.MeshMorphHeader NewMorphHeader = new TrickyMPFModelHandler.MeshMorphHeader();
                                            NewMorphHeader.staticMesh = new List<TrickyMPFModelHandler.MeshChunk>();
                                            NewMorphHeader.MorphKeyList = new List<TrickyMPFModelHandler.MorphKey>();
                                            NewMorphHeader.staticMesh.Add(meshList[b]);
                                            NewMorphHeader.MorphKeyList = meshList[b].MorphKeys;
                                            TempWeightRefGroup.MeshGroupHeaders.Add(NewMorphHeader);

                                        }
                                        else
                                        {
                                            //Add to zero
                                            if (TempWeightRefGroup.MeshGroupHeaders.Count == 0)
                                            {
                                                TrickyMPFModelHandler.MeshMorphHeader TempMorphHeader = new TrickyMPFModelHandler.MeshMorphHeader();
                                                TempMorphHeader.staticMesh = new List<TrickyMPFModelHandler.MeshChunk>();
                                                TempMorphHeader.MorphKeyList = new List<TrickyMPFModelHandler.MorphKey>();
                                                TempWeightRefGroup.MeshGroupHeaders.Add(TempMorphHeader);
                                            }
                                            TrickyMPFModelHandler.MeshMorphHeader NewMorphHeader = TempWeightRefGroup.MeshGroupHeaders[0];
                                            NewMorphHeader.staticMesh.Add(meshList[b]);
                                            TempWeightRefGroup.MeshGroupHeaders[0] = NewMorphHeader;

                                        }
                                        FirstAdd = false;
                                        TempMaterialGroup.meshGroupSubs[c] = TempWeightRefGroup;
                                        break;
                                    }

                                    TempMaterialGroup.meshGroupSubs[c] = TempWeightRefGroup;
                                }
                                //If false wait till start of list reading and make new one

                                if (FirstAdd && !WeightRefGroupTest)
                                {
                                    TrickyMPFModelHandler.WeightRefGroup weightRefGroup = new TrickyMPFModelHandler.WeightRefGroup();
                                    weightRefGroup.MeshGroupHeaders = new List<TrickyMPFModelHandler.MeshMorphHeader>();
                                    weightRefGroup.weights = new List<int>();
                                    TempMaterialGroup.meshGroupSubs.Add(weightRefGroup);
                                    TempTrickyMesh.MeshGroups[a] = TempMaterialGroup;
                                    b--;
                                    break;
                                }
                                TempTrickyMesh.MeshGroups[a] = TempMaterialGroup;
                                if (Added)
                                {
                                    break;
                                }
                            }
                        }
                    }

                    //Make Material and minus 1 from b
                    if (!MaterialGroupTest)
                    {
                        var NewMaterialGroup = new TrickyMPFModelHandler.GroupMainHeader();
                        NewMaterialGroup.MaterialID = meshList[b].MatieralID;
                        if (Shadow)
                        {
                            NewMaterialGroup.GroupType = 17;
                        }
                        else if (trickyModelCombiner.reassignedMesh[0].MorphTargetCount != 0)
                        {
                            NewMaterialGroup.GroupType = 256;
                        }
                        else
                        {
                            NewMaterialGroup.GroupType = 1;
                        }
                        NewMaterialGroup.Unknown = -1;
                        NewMaterialGroup.meshGroupSubs = new List<TrickyMPFModelHandler.WeightRefGroup>();
                        TempTrickyMesh.MeshGroups.Add(NewMaterialGroup);
                        b--;
                    }
                }


                bool BreakOutTest = true;
                for (int ai = 0; ai < meshList.Count; ai++)
                {
                    if (!meshList[ai].Grouped)
                    {
                        BreakOutTest = false;
                        break;
                    }
                }
                if (BreakOutTest)
                {
                    break;
                }
            }


            //Generate Number Ref and correct UV
            //Prephaps Move into static meshing
            TempTrickyMesh.numberListRefs = new List<TrickyMPFModelHandler.WeightRefList>();
            for (int i = 0; i < TempTrickyMesh.MeshGroups.Count; i++)
            {
                var TempMeshGroup = TempTrickyMesh.MeshGroups[i];
                for (int a = 0; a < TempMeshGroup.meshGroupSubs.Count; a++)
                {
                    var TempSubGroup = TempMeshGroup.meshGroupSubs[a];
                    for (int b = 0; b < TempSubGroup.MeshGroupHeaders.Count; b++)
                    {
                        var TempMeshGroupHeader = TempSubGroup.MeshGroupHeaders[b];
                        TrickyMPFModelHandler.WeightRefList NumberRef = new TrickyMPFModelHandler.WeightRefList();
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
            MessageBox.Show("Import Sucessful");
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
                MessageBox.Show("Incorrect Ammount of Bones " + trickyModelCombiner.bones.Count + "/" + bones.Count);
                return;
            }

            //Test that all names are vaild for selected mesh type
            for (int i = 0; i < trickyModelCombiner.reassignedMesh.Count; i++)
            {
                if ((MeshID != 0 && trickyModelCombiner.reassignedMesh[i].MeshName.Contains("3000")) ||
                (MeshID != 1 && trickyModelCombiner.reassignedMesh[i].MeshName.Contains("1500")) ||
                (MeshID != 2 && trickyModelCombiner.reassignedMesh[i].MeshName.Contains("750") && !trickyModelCombiner.reassignedMesh[i].MeshName.ToLower().Contains("shdw")) ||
                (MeshID != 3 && trickyModelCombiner.reassignedMesh[i].MeshName.ToLower().Contains("shdw")))
                {
                    MessageBox.Show(trickyModelCombiner.reassignedMesh[i].MeshName + " Non-Matching Mesh Part");
                    return;
                }
            }

            //Check That Mesh Ammount and Names Are Correct Attaching if they are body and there mesh id
            //Set if mesh contains morph or is shadow making sure that morph contains morph points
            for (int i = 0; i < trickyModelCombiner.reassignedMesh.Count; i++)
            {
                bool Reassinged = false;
                var TempReMesh = trickyModelCombiner.reassignedMesh[i];

                if (TempReMesh.faces[0].MorphPoint1.Count != 0)
                {
                    TempReMesh.MorphTargetCount = TempReMesh.faces[0].MorphPoint1.Count;
                }

                for (int a = 0; a < Body.ModelList.Count; a++)
                {
                    if (Body.ModelList[a].FileName.Trim().ToLower() == trickyModelCombiner.reassignedMesh[i].MeshName.Trim().ToLower())
                    {
                        TempReMesh.BodyHead = false;
                        TempReMesh.MeshId = a;
                        Reassinged = true;
                        break;
                    }
                }

                for (int a = 0; a < Head.ModelList.Count; a++)
                {
                    if (Head.ModelList[a].FileName.Trim().ToLower() == trickyModelCombiner.reassignedMesh[i].MeshName.Trim().ToLower())
                    {
                        TempReMesh.BodyHead = true;
                        TempReMesh.MeshId = a;
                        Reassinged = true;
                        if (TempReMesh.MorphTargetCount!= Head.ModelList[a].MorphKeyCount)
                        {
                            MessageBox.Show("Incorrect ammount of Shapekeys " + Head.ModelList[a].MorphKeyCount + "/" +TempReMesh.MorphTargetCount);
                            return;
                        }

                        break;
                    }
                }
                if (!Reassinged)
                {
                    MessageBox.Show("Incorrectly Named Or Unneeded mesh part detected \n" + TempReMesh.MeshName);
                }

                trickyModelCombiner.reassignedMesh[i] = TempReMesh;
            }

            //Check Materials are valid
            for (int i = 0; i < trickyModelCombiner.materials.Count; i++)
            {
                if (!trickyModelCombiner.materials[i].MainTexture.ToLower().Contains("boot") &&
                    !trickyModelCombiner.materials[i].MainTexture.ToLower().Contains("suit") &&
                    !trickyModelCombiner.materials[i].MainTexture.ToLower().Contains("head") &&
                    !trickyModelCombiner.materials[i].MainTexture.ToLower().Contains("helm"))
                {
                    MessageBox.Show("Invalid material " + trickyModelCombiner.materials[i].MainTexture);
                    return;
                }
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

                //Update Bones
                if (BoneUpdate)
                {
                    for (int a = 0; a < TempTrickyMesh.boneDatas.Count; a++)
                    {
                        var TempBone = TempTrickyMesh.boneDatas[a];
                        for (int b = 0; b < trickyModelCombiner.bones.Count; b++)
                        {
                            var TempNewBone = trickyModelCombiner.bones[b];
                            if (TempBone.BoneName == TempNewBone.BoneName)
                            {
                                TempBone.Position = TempNewBone.Position;
                                //TempBone.Radians = TempNewBone.Radians;

                                //for (int c = 0; c < bones.Count; c++)
                                //{
                                //    if (bones[c].BoneName == TempBone.parentName)
                                //    {
                                //        TempBone.ParentBone = bones[c].BonePos;
                                //        TempBone.ParentFileID = bones[c].FileID;
                                //    }
                                //}
                            }
                        }


                        TempTrickyMesh.boneDatas[a] = TempBone;
                    }

                }

                List<int> MaterialsID = new List<int>();
                List<int> RedoneMaterial = new List<int>();
                //Regenerate Materials
                TempTrickyMesh.materialDatas = new List<TrickyMPFModelHandler.MaterialData>();
                for (int a = 0; a < TempReMesh.faces.Count; a++)
                {
                    if (!MaterialsID.Contains(TempReMesh.faces[a].MaterialID))
                    {
                        MaterialsID.Add(TempReMesh.faces[a].MaterialID);
                    }
                }

                for (int a = 0; a < MaterialsID.Count; a++)
                {
                    TrickyMPFModelHandler.MaterialData MaterialData = new TrickyMPFModelHandler.MaterialData();
                    RedoneMaterial.Add(a);
                    MaterialData.MainTexture = trickyModelCombiner.materials[MaterialsID[a]].MainTexture.Substring(0, 4).ToLower();
                    MaterialData.Texture1 = "";
                    MaterialData.Texture2 = "";
                    MaterialData.Texture3 = "";
                    MaterialData.Texture4 = "";

                    string[] Split = trickyModelCombiner.materials[MaterialsID[a]].MainTexture.Split(' ');

                    if(Split.Length>1)
                    {
                        if (Split[1].ToLower()=="gloss")
                        {
                            if(MaterialData.MainTexture=="suit")
                            {
                                MaterialData.Texture3 = "st_g";
                            }
                            else if (MaterialData.MainTexture == "helm")
                            {
                                MaterialData.Texture3 = "hm_g";
                            }
                            else if (MaterialData.MainTexture == "boot")
                            {
                                MaterialData.Texture3 = "bt_g";
                            }
                            else if (MaterialData.MainTexture == "head")
                            {
                                MaterialData.Texture3 = "hd_g";
                            }
                        }
                        else if (Split[1].ToLower() == "envr")
                        {
                            MaterialData.Texture4 = "envr";
                        }

                    }

                    TempTrickyMesh.materialDatas.Add(MaterialData);
                }

                for (int a = 0; a < TempReMesh.faces.Count; a++)
                {
                    int Index = MaterialsID.IndexOf(TempReMesh.faces[a].MaterialID);
                    var TempFace = TempReMesh.faces[a];
                    TempFace.MaterialID = Index;
                    TempReMesh.faces[a] = TempFace;
                }

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
                        else if (TempReMesh.faces[a].Weight1Pos != VectorPoint[TempID].Weight)
                        {
                            TestID++;
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
                            TempFace.Id1 = TempID;
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
                        else if (TempReMesh.faces[a].Weight2Pos != VectorPoint[TempID].Weight)
                        {
                            TestID++;
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
                            TempFace.Id2 = TempID;
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
                        else if (TempReMesh.faces[a].Weight3Pos != VectorPoint[TempID].Weight)
                        {
                            TestID++;
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
                            TempFace.Id3 = TempID;
                            Test = true;
                        }
                    }
                    #endregion

                    indiceFaces.Add(TempFace);
                }


                indiceFaces = TristripGenerator.NeighbourPriority(indiceFaces);

                //Send to Tristrip Generator
                List<TristripGenerator.IndiceTristrip> indiceTristrips = TristripGenerator.GenerateTristripNivda(indiceFaces);

                if(indiceTristrips==null)
                {
                    MessageBox.Show("Tristrip Failed to Generate");
                    return;
                }

                //Static mesh that shit
                TempTrickyMesh.MeshGroups = new List<TrickyMPFModelHandler.GroupMainHeader>();
                List<TrickyMPFModelHandler.MeshChunk> meshList = new List<TrickyMPFModelHandler.MeshChunk>();

                for (int a = 0; a < TempTrickyMesh.materialDatas.Count; a++)
                {
                    while (true)
                    {
                        TrickyMPFModelHandler.MeshChunk staticMesh = new TrickyMPFModelHandler.MeshChunk();
                        staticMesh.weightsInts = new List<int>();
                        staticMesh.Unknown1 = 14;
                        if (!Shadow)
                        {
                            staticMesh.Unknown2 = 114;
                        }
                        staticMesh.MatieralID = a;
                        staticMesh.vertices = new List<Vector3>();
                        staticMesh.uvNormals = new List<Vector3>();
                        staticMesh.Strips = new List<int>();
                        staticMesh.uv = new List<Vector4>();
                        staticMesh.Weights = new List<int>();
                        staticMesh.MorphKeys = new List<TrickyMPFModelHandler.MorphKey>();

                        if (TempReMesh.MorphTargetCount != 0)
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
                            if (VectorPoint[indiceTristrips[b].Indices[0]].Material == staticMesh.MatieralID && !indiceTristrips[b].Used)
                            {
                                bool MeshGroupSubTest = false;
                                List<int> NewInts = new List<int>();
                                for (int c = 0; c < indiceTristrips[b].Indices.Count; c++)
                                {
                                    if (!staticMesh.weightsInts.Contains(VectorPoint[indiceTristrips[b].Indices[c]].Weight) && !NewInts.Contains(VectorPoint[indiceTristrips[b].Indices[c]].Weight))
                                    {
                                        NewInts.Add(VectorPoint[indiceTristrips[b].Indices[c]].Weight);
                                    }
                                }

                                if (NewInts.Count + staticMesh.weightsInts.Count <= 25)
                                {
                                    staticMesh.weightsInts.AddRange(NewInts);
                                    MeshGroupSubTest = true;
                                }

                                if (staticMesh.vertices.Count + indiceTristrips[b].Indices.Count <= 55 && MeshGroupSubTest)
                                {
                                    var TempIndice = indiceTristrips[b];
                                    TempIndice.Used = true;
                                    indiceTristrips[b] = TempIndice;

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
                            }
                        }

                        if (staticMesh.vertices.Count != 0)
                        {
                            meshList.Add(staticMesh);
                        }
                        bool BreakoutTest = true;
                        for (int b = 0; b < indiceTristrips.Count; b++)
                        {
                            if (!indiceTristrips[b].Used && VectorPoint[indiceTristrips[b].Indices[0]].Material == staticMesh.MatieralID)
                            {
                                BreakoutTest = false;
                                break;
                            }
                        }
                        if(BreakoutTest)
                        {
                            break;
                        }
                    }
                }

                //Group that shit MK2
                while (true)
                {
                    bool FirstAdd = true;
                    for (int b = 0; b < meshList.Count; b++)
                    {
                        bool Added = false;

                        bool MaterialGroupTest = false;

                        for (int a = 0; a < TempTrickyMesh.MeshGroups.Count; a++)
                        {
                            if (meshList[b].MatieralID == TempTrickyMesh.MeshGroups[a].MaterialID )
                            {
                                MaterialGroupTest = true;
                                if (!meshList[b].Grouped)
                                {
                                    var TempMaterialGroup = TempTrickyMesh.MeshGroups[a];
                                    bool WeightRefGroupTest = false;

                                    for (int c = 0; c < TempMaterialGroup.meshGroupSubs.Count; c++)
                                    {
                                        var TempWeightRefGroup = TempMaterialGroup.meshGroupSubs[c];

                                        //Test the WeigthRef
                                        bool WeightRefTest = false;
                                        List<int> NewInts = new List<int>();
                                        for (int ce = 0; ce < meshList[b].Weights.Count; ce++)
                                        {
                                            if (!TempWeightRefGroup.weights.Contains(meshList[b].Weights[ce]) && !NewInts.Contains(meshList[b].Weights[ce]))
                                            {
                                                NewInts.Add(meshList[b].Weights[ce]);
                                            }
                                        }

                                        if (NewInts.Count + TempWeightRefGroup.weights.Count <= 25)
                                        {
                                            TempWeightRefGroup.weights.AddRange(NewInts);
                                            WeightRefTest = true;
                                        }

                                        if (WeightRefTest)
                                        {
                                            WeightRefGroupTest = true;
                                            var TempMesh = meshList[b];
                                            TempMesh.Grouped = true;
                                            Added = true;
                                            meshList[b] = TempMesh;
                                            //If Morph is true
                                            if (TempTrickyMesh.MorphKeyCount != 0)
                                            {
                                                //Add New
                                                TrickyMPFModelHandler.MeshMorphHeader NewMorphHeader = new TrickyMPFModelHandler.MeshMorphHeader();
                                                NewMorphHeader.staticMesh = new List<TrickyMPFModelHandler.MeshChunk>();
                                                NewMorphHeader.MorphKeyList = new List<TrickyMPFModelHandler.MorphKey>();
                                                NewMorphHeader.staticMesh.Add(meshList[b]);
                                                NewMorphHeader.MorphKeyList = meshList[b].MorphKeys;
                                                TempWeightRefGroup.MeshGroupHeaders.Add(NewMorphHeader);

                                            }
                                            else
                                            {
                                                //Add to zero
                                                if (TempWeightRefGroup.MeshGroupHeaders.Count == 0)
                                                {
                                                    TrickyMPFModelHandler.MeshMorphHeader TempMorphHeader = new TrickyMPFModelHandler.MeshMorphHeader();
                                                    TempMorphHeader.staticMesh = new List<TrickyMPFModelHandler.MeshChunk>();
                                                    TempMorphHeader.MorphKeyList = new List<TrickyMPFModelHandler.MorphKey>();
                                                    TempWeightRefGroup.MeshGroupHeaders.Add(TempMorphHeader);
                                                }
                                                TrickyMPFModelHandler.MeshMorphHeader NewMorphHeader = TempWeightRefGroup.MeshGroupHeaders[0];
                                                NewMorphHeader.staticMesh.Add(meshList[b]);
                                                TempWeightRefGroup.MeshGroupHeaders[0] = NewMorphHeader;

                                            }
                                            FirstAdd = false;
                                            TempMaterialGroup.meshGroupSubs[c] = TempWeightRefGroup;
                                            break;
                                        }

                                        TempMaterialGroup.meshGroupSubs[c] = TempWeightRefGroup;
                                    }
                                    //If false wait till start of list reading and make new one

                                    if (FirstAdd && !WeightRefGroupTest)
                                    {
                                        TrickyMPFModelHandler.WeightRefGroup weightRefGroup = new TrickyMPFModelHandler.WeightRefGroup();
                                        weightRefGroup.MeshGroupHeaders = new List<TrickyMPFModelHandler.MeshMorphHeader>();
                                        weightRefGroup.weights = new List<int>();
                                        TempMaterialGroup.meshGroupSubs.Add(weightRefGroup);
                                        TempTrickyMesh.MeshGroups[a] = TempMaterialGroup;
                                        b--;
                                        break;
                                    }
                                    TempTrickyMesh.MeshGroups[a] = TempMaterialGroup;
                                    if (Added)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        //Make Material and minus 1 from b
                        if (!MaterialGroupTest)
                        {
                            var NewMaterialGroup = new TrickyMPFModelHandler.GroupMainHeader();
                            NewMaterialGroup.MaterialID = meshList[b].MatieralID;
                            if (Shadow)
                            {
                                NewMaterialGroup.GroupType = 17;
                            }
                            else if (TempReMesh.MorphTargetCount != 0)
                            {
                                NewMaterialGroup.GroupType = 256;
                            }
                            else
                            {
                                NewMaterialGroup.GroupType = 1;
                            }
                            NewMaterialGroup.Unknown = -1;
                            NewMaterialGroup.meshGroupSubs = new List<TrickyMPFModelHandler.WeightRefGroup>();
                            TempTrickyMesh.MeshGroups.Add(NewMaterialGroup);
                            b--;
                        }
                    }


                    bool BreakOutTest = true;
                    for (int ai = 0; ai < meshList.Count; ai++)
                    {
                        if (!meshList[ai].Grouped)
                        {
                            BreakOutTest = false;
                            break;
                        }
                    }
                    if (BreakOutTest)
                    {
                        break;
                    }
                }

                //Generate Number Ref and correct UV
                TempTrickyMesh.numberListRefs = new List<TrickyMPFModelHandler.WeightRefList>();
                for (int ei = 0; ei < TempTrickyMesh.MeshGroups.Count; ei++)
                {
                    var TempMeshGroup = TempTrickyMesh.MeshGroups[ei];
                    for (int a = 0; a < TempMeshGroup.meshGroupSubs.Count; a++)
                    {
                        var TempSubGroup = TempMeshGroup.meshGroupSubs[a];
                        TrickyMPFModelHandler.WeightRefList NumberRef = new TrickyMPFModelHandler.WeightRefList();
                        NumberRef.WeightIDs = new List<int>();
                        for (int b = 0; b < TempSubGroup.MeshGroupHeaders.Count; b++)
                        {
                            var TempMeshGroupHeader = TempSubGroup.MeshGroupHeaders[b];

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
                                        TempUV.W = TempMesh.Weights[d] * 3 + 114;
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


                            TempSubGroup.MeshGroupHeaders[b] = TempMeshGroupHeader;
                        }
                        TempTrickyMesh.numberListRefs.Add(NumberRef);
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
            vectorPoint.Normals = new List<Vector3>();
            if(Vertice==1)
            {
                vectorPoint.vector = face.V1;
                vectorPoint.normal = face.Normal1;
                vectorPoint.TextureCord = face.UV1;
                vectorPoint.Weight = face.Weight1Pos;
                vectorPoint.MorphData = face.MorphPoint1;
                vectorPoint.Normals.Add(face.Normal1);
            }

            if (Vertice == 2)
            {
                vectorPoint.vector = face.V2;
                vectorPoint.normal = face.Normal2;
                vectorPoint.TextureCord = face.UV2;
                vectorPoint.Weight = face.Weight2Pos;
                vectorPoint.MorphData = face.MorphPoint2;
                vectorPoint.Normals.Add(face.Normal2);
            }

            if (Vertice == 3)
            {
                vectorPoint.vector = face.V3;
                vectorPoint.normal = face.Normal3;
                vectorPoint.TextureCord = face.UV3;
                vectorPoint.Weight = face.Weight3Pos;
                vectorPoint.MorphData = face.MorphPoint3;
                vectorPoint.Normals.Add(face.Normal3);
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
            if ((int)(Uv1.X * 4096f) == (int)(Uv2.X * 4096f) && (int)(Uv1.Y * 4096f) == (int)(Uv2.Y * 4096f))
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
                        if (boneWeightList[i].boneWeights[a].Weight == boneWeight.boneWeights[a].Weight && boneWeightList[i].boneWeights[a].BoneID == boneWeight.boneWeights[a].BoneID)
                        {
                            Test = true;
                        }
                        else
                        {
                            Test = false;
                            break;
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
            public List<Vector3> Normals;
            public List<Vector3> MorphData;
        }

    }
}
