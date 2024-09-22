using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models.Tricky
{
    public class TrickyGCModelCombiner
    {
        public TrickyGCMNF? Body;
        public TrickyGCMNF? Head;
        public TrickyGCMNF? Board;

        public List<TrickyGCMNF.MaterialData> materials;
        public List<TrickyGCMNF.BoneData> bones;
        public bool BodyBool;

        public bool BoneUpdate;
        public int TristripMode;
        public bool NormalAverage;

        public List<ReassignedMesh> reassignedMesh = new List<ReassignedMesh>();

        public int DectectModelType(TrickyGCMNF modelHandler)
        {
            for (int i = 0; i < modelHandler.modelHeaders.Count; i++)
            {
                if (modelHandler.modelHeaders[i].ModelName.ToLower().Contains("body"))
                {
                    Board = null;
                    Body = modelHandler;
                    BodyBool = true;
                    return 0;
                }
                if (modelHandler.modelHeaders[i].ModelName.ToLower().Contains("head"))
                {
                    Board = null;
                    Head = modelHandler;
                    BodyBool = true;
                    return 1;
                }
                if (modelHandler.modelHeaders[i].ModelName.ToLower().Contains("algoofy"))
                {
                    Head = null;
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
            TempMesh.faces = new List<TrickyGCMNF.Face>();
            materials = new List<TrickyGCMNF.MaterialData>();
            bones = new List<TrickyGCMNF.BoneData>();


            materials.AddRange(Board.modelHeaders[MeshID].materialDatas);
            bones.AddRange(Board.modelHeaders[MeshID].boneDatas);
            TempMesh.faces.AddRange(ReturnFaces(Board.modelHeaders[MeshID]));
            TempMesh.MeshName = Board.modelHeaders[MeshID].ModelName;
            if (TempMesh.MeshName.ToLower().Contains("shdw"))
            {
                TempMesh.ShadowModel = true;
            }
            TempMesh.IKPoints = Board.modelHeaders[MeshID].iKPoints;


            reassignedMesh.Add(TempMesh);
        }

        public void StartReassignMeshCharacter(int MeshID)
        {
            reassignedMesh = new List<ReassignedMesh>();
            materials = new List<TrickyGCMNF.MaterialData>();
            bones = new List<TrickyGCMNF.BoneData>();
            int ListSize = 0;

            var TempMaterial = new TrickyGCMNF.MaterialData();
            TempMaterial.MainTexture = "helm";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyGCMNF.MaterialData();
            TempMaterial.MainTexture = "helm gloss";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyGCMNF.MaterialData();
            TempMaterial.MainTexture = "helm envr";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyGCMNF.MaterialData();
            TempMaterial.MainTexture = "boot";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyGCMNF.MaterialData();
            TempMaterial.MainTexture = "boot gloss";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyGCMNF.MaterialData();
            TempMaterial.MainTexture = "boot envr";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyGCMNF.MaterialData();
            TempMaterial.MainTexture = "head";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyGCMNF.MaterialData();
            TempMaterial.MainTexture = "head gloss";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyGCMNF.MaterialData();
            TempMaterial.MainTexture = "head envr";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyGCMNF.MaterialData();
            TempMaterial.MainTexture = "suit";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyGCMNF.MaterialData();
            TempMaterial.MainTexture = "suit gloss";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyGCMNF.MaterialData();
            TempMaterial.MainTexture = "suit envr";
            materials.Add(TempMaterial);


            //Body
            for (int i = 0; i < Body.modelHeaders.Count; i++)
            {
                if ((MeshID == 0 && Body.modelHeaders[i].ModelName.Contains("3000")) ||
                    (MeshID == 1 && Body.modelHeaders[i].ModelName.Contains("1500")) ||
                    (MeshID == 2 && Body.modelHeaders[i].ModelName.Contains("750") && !Body.modelHeaders[i].ModelName.ToLower().Contains("shdw")) ||
                    (MeshID == 3 && Body.modelHeaders[i].ModelName.ToLower().Contains("shdw")))
                {
                    var TempMesh = new ReassignedMesh();
                    bones.AddRange(Body.modelHeaders[i].boneDatas);
                    TempMesh.faces = ReturnFixedFaces(Body.modelHeaders[i], bones);
                    ListSize++;
                    TempMesh.MeshName = Body.modelHeaders[i].ModelName;
                    if (MeshID == 3)
                    {
                        TempMesh.ShadowModel = true;
                    }

                    reassignedMesh.Add(TempMesh);
                }
            }

            //Head
            for (int i = 0; i < Head.modelHeaders.Count; i++)
            {
                if ((MeshID == 0 && Head.modelHeaders[i].ModelName.Contains("3000")) ||
                    (MeshID == 1 && Head.modelHeaders[i].ModelName.Contains("1500")) ||
                    (MeshID == 2 && Head.modelHeaders[i].ModelName.Contains("750") && !Head.modelHeaders[i].ModelName.ToLower().Contains("shdw")) ||
                    (MeshID == 3 && Head.modelHeaders[i].ModelName.ToLower().Contains("shdw")))
                {
                    var TempMesh = new ReassignedMesh();
                    bones.AddRange(Head.modelHeaders[i].boneDatas);
                    TempMesh.MeshName = Head.modelHeaders[i].ModelName;
                    TempMesh.faces = ReturnFixedFaces(Head.modelHeaders[i], bones);
                    ListSize++;
                    if (MeshID == 3)
                    {
                        TempMesh.ShadowModel = true;
                    }

                    TempMesh.MorphTargetCount = Head.modelHeaders[i].NumMorphs;
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

        public List<TrickyGCMNF.Face> ReturnFixedFaces(TrickyGCMNF.ModelHeader modelHeader, List<TrickyGCMNF.BoneData> BoneData)
        {
            List<TrickyGCMNF.Face> NewFaces = new List<TrickyGCMNF.Face>();

            for (int i = 0; i < modelHeader.boneWeightHeaders.Count; i++)
            {
                modelHeader.boneWeightHeaders[i] = FixBoneIDs(modelHeader.boneWeightHeaders[i], BoneData);
            }

            for (int a = 0; a < modelHeader.meshHeaders.Count; a++)
            {
                var MeshHeaders = modelHeader.meshHeaders[a];
                for (int b = 0; b < MeshHeaders.indexGroupHeaders.Count; b++)
                {
                    var IndexHeaders = MeshHeaders.indexGroupHeaders[b];
                    for (int c = 0; c < IndexHeaders.faces.Count; c++)
                    {
                        var Face = IndexHeaders.faces[c];

                        Face.Weight1 = modelHeader.boneWeightHeaders[Face.Weight1Pos];
                        Face.Weight2 = modelHeader.boneWeightHeaders[Face.Weight2Pos];
                        Face.Weight3 = modelHeader.boneWeightHeaders[Face.Weight3Pos];

                        var TempMat = modelHeader.materialDatas[IndexHeaders.MatIndex0];
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

                        IndexHeaders.faces[c] = Face;
                    }
                    MeshHeaders.indexGroupHeaders[b] = IndexHeaders;
                }
                modelHeader.meshHeaders[a] = MeshHeaders;
            }

            return NewFaces;
        }

        public TrickyGCMNF.BoneWeightHeader FixBoneIDs(TrickyGCMNF.BoneWeightHeader weightHeader, List<TrickyGCMNF.BoneData> BoneData)
        {
            var NewHeader = weightHeader;
            for (int i = 0; i < NewHeader.boneWeights.Count; i++)
            {
                var Temp = NewHeader.boneWeights[i];
                for (int a = 0; a < BoneData.Count; a++)
                {
                    if (BoneData[a].FileID == Temp.FileID)
                    {
                        if (BoneData[a].BonePos == Temp.BoneID)
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

        public List<TrickyGCMNF.Face> ReturnFaces(TrickyGCMNF.ModelHeader modelHeader)
        {
            List<TrickyGCMNF.Face> NewFaceList = new List<TrickyGCMNF.Face>();

            for (int i = 0; i < modelHeader.meshHeaders.Count; i++)
            {
                var TempMesh = modelHeader.meshHeaders[i];
                for (int a = 0; a < TempMesh.indexGroupHeaders.Count; a++)
                {
                    var TempIndex = TempMesh.indexGroupHeaders[a];

                    NewFaceList.AddRange(TempIndex.faces);
                }
            }

            return NewFaceList;
        }

        public void StartRegenMesh(TrickyGCModelCombiner trickyModelCombiner, int Selected)
        {
            if (Board != null && !BodyBool)
            {
                StartRegenMeshBoard(trickyModelCombiner, Selected);
            }
            else if (Head != null && Body != null)
            {
                MessageBox.Show("Characters Currently Unsupported");
                //StartRegenMeshCharacter(trickyModelCombiner, Selected);
            }
            else
            {
                MessageBox.Show("Error Missing Files");
            }

        }

        public void StartRegenMeshBoard(TrickyGCModelCombiner trickyModelCombiner, int Selected)
        {
            if (trickyModelCombiner.bones == null)
            {
                MessageBox.Show("No Bones Detected");
                return;
            }
            if (trickyModelCombiner.bones.Count != Board.modelHeaders[Selected].boneDatas.Count)
            {
                MessageBox.Show("Incorrect Ammount of Bones");
                return;
            }

            bool Shadow = false;

            if (Board.modelHeaders[Selected].ModelName.ToLower().Contains("shdw"))
            {
                Shadow = true;
            }

            var TempTrickyMesh = Board.modelHeaders[Selected];

            ReassignedMesh ReassignedMesh = new ReassignedMesh();
            bool MeshTest = false;
            for (int i = 0; i < trickyModelCombiner.reassignedMesh.Count; i++)
            {
                if (trickyModelCombiner.reassignedMesh[i].MeshName == Board.modelHeaders[Selected].ModelName)
                {
                    ReassignedMesh = trickyModelCombiner.reassignedMesh[i];
                    MeshTest = true;
                }
            }

            if (!MeshTest)
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
            TempTrickyMesh.materialDatas = new List<TrickyGCMNF.MaterialData>();
            for (int a = 0; a < ReassignedMesh.faces.Count; a++)
            {
                if (!MaterialsID.Contains(ReassignedMesh.faces[a].MaterialID))
                {
                    MaterialsID.Add(ReassignedMesh.faces[a].MaterialID);
                }
            }

            for (int a = 0; a < MaterialsID.Count; a++)
            {
                TrickyGCMNF.MaterialData MaterialData = new TrickyGCMNF.MaterialData();
                RedoneMaterial.Add(a);
                MaterialData.MainTexture = trickyModelCombiner.materials[MaterialsID[a]].MainTexture.Substring(0, 4).ToLower();
                MaterialData.Texture1 = "";
                MaterialData.Texture2 = "";
                MaterialData.Texture3 = "";
                MaterialData.Texture4 = "";

                MaterialData.FactorFloat = 0.00392156885936856f;
                MaterialData.Unused1Float = 0.00392156885936856f;
                MaterialData.Unused2Float = 0.00392156885936856f;

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

            //Redo Data In Correct Formats IE make Weight List and make faces use the positions.
            TempTrickyMesh.boneWeightHeaders = new List<TrickyGCMNF.BoneWeightHeader>();

            //Load Headers into file
            for (int i = 0; i < ReassignedMesh.faces.Count; i++)
            {
                var TempFace = ReassignedMesh.faces[i];
                int WeightID = ContainsWeight(TempFace.Weight1, TempTrickyMesh.boneWeightHeaders);
                if (WeightID == -1)
                {
                    TempTrickyMesh.boneWeightHeaders.Add(TempFace.Weight1);
                    WeightID = TempTrickyMesh.boneWeightHeaders.Count - 1;
                }
                TempFace.Weight1Pos = WeightID;

                WeightID = ContainsWeight(TempFace.Weight2, TempTrickyMesh.boneWeightHeaders);
                if (WeightID == -1)
                {
                    TempTrickyMesh.boneWeightHeaders.Add(TempFace.Weight2);
                    WeightID = TempTrickyMesh.boneWeightHeaders.Count - 1;
                }
                TempFace.Weight2Pos = WeightID;

                WeightID = ContainsWeight(TempFace.Weight3, TempTrickyMesh.boneWeightHeaders);
                if (WeightID == -1)
                {
                    TempTrickyMesh.boneWeightHeaders.Add(TempFace.Weight3);
                    WeightID = TempTrickyMesh.boneWeightHeaders.Count - 1;
                }
                TempFace.Weight3Pos = WeightID;

                ReassignedMesh.faces[i] = TempFace;
            }
            //Correct Bone File ID
            for (int i = 0; i < TempTrickyMesh.boneWeightHeaders.Count; i++)
            {
                var TempHeader = TempTrickyMesh.boneWeightHeaders[i];

                for (int a = 0; a < TempHeader.boneWeights.Count; a++)
                {
                    var TempWeight = TempHeader.boneWeights[a];

                    TempWeight.FileID = TempTrickyMesh.FileID;

                    TempHeader.boneWeights[a] = TempWeight;
                }
                TempTrickyMesh.boneWeightHeaders[i] = TempHeader;
            }

            //Take faces and Generate Indce faces and giant vertex list
            List<TrickyGCMNF.VertexData> VectorPoint = new List<TrickyGCMNF.VertexData>();
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
                    else if (ReassignedMesh.faces[a].Weight1Pos != VectorPoint[TempID].WeightIndex)
                    {
                        TestID++;
                    }
                    else
                    {
                        TestID++;
                        if (ReassignedMesh.faces[a].MaterialID != VectorPoint[TempID].MaterialID)
                        {

                        }
                        else if (!NormalsEqual(ReassignedMesh.faces[a].Normal1, VectorPoint[TempID].VertexNormal) && !NormalAverage)
                        {

                        }
                        else if (!UVEqual(ReassignedMesh.faces[a].UV1, VectorPoint[TempID].UV))
                        {

                        }
                        else
                        {
                            if (ReassignedMesh.MorphTargetCount != 0)
                            {
                                if (!MorphPointsEqual(ReassignedMesh.faces[a].MorphPoint1, VectorPoint[TempID].MorphDatas))
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
                    else if (ReassignedMesh.faces[a].Weight2Pos != VectorPoint[TempID].WeightIndex)
                    {
                        TestID++;
                    }
                    else
                    {
                        TestID++;
                        if (ReassignedMesh.faces[a].MaterialID != VectorPoint[TempID].MaterialID)
                        {

                        }
                        else if (!NormalsEqual(ReassignedMesh.faces[a].Normal2, VectorPoint[TempID].VertexNormal) && !NormalAverage)
                        {

                        }
                        else if (!UVEqual(ReassignedMesh.faces[a].UV2, VectorPoint[TempID].UV))
                        {

                        }
                        else
                        {
                            if (ReassignedMesh.MorphTargetCount != 0)
                            {
                                if (!MorphPointsEqual(ReassignedMesh.faces[a].MorphPoint2, VectorPoint[TempID].MorphDatas))
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
                    else if (ReassignedMesh.faces[a].Weight3Pos != VectorPoint[TempID].WeightIndex)
                    {
                        TestID++;
                    }
                    else
                    {
                        TestID++;
                        if (ReassignedMesh.faces[a].MaterialID != VectorPoint[TempID].MaterialID)
                        {

                        }
                        else if (!NormalsEqual(ReassignedMesh.faces[a].Normal3, VectorPoint[TempID].VertexNormal) && !NormalAverage)
                        {

                        }
                        else if (!UVEqual(ReassignedMesh.faces[a].UV3, VectorPoint[TempID].UV))
                        {

                        }
                        else
                        {
                            if (ReassignedMesh.MorphTargetCount != 0)
                            {
                                if (!MorphPointsEqual(ReassignedMesh.faces[a].MorphPoint3, VectorPoint[TempID].MorphDatas))
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
                }
                #endregion

                indiceFaces.Add(TempFace);
            }

            //Once in Vertex List Send Morph Data To MorphList
            List<TrickyGCMNF.MorphHeader> NewMorphHeader = new List<TrickyGCMNF.MorphHeader>();
            for (int i = 0; i < ReassignedMesh.MorphTargetCount; i++)
            {
                var TempMorphHeader = new TrickyGCMNF.MorphHeader();
                TempMorphHeader.MorphDataList = new List<TrickyGCMNF.MorphData>();
                NewMorphHeader.Add(TempMorphHeader);
            }

            for (int i = 0; i < NewMorphHeader.Count; i++)
            {
                for (int a = 0; a < VectorPoint.Count; a++)
                {
                    if (VectorPoint[a].MorphDatas[i] != Vector3.Zero)
                    {
                        TrickyGCMNF.MorphData TempMorphData = new TrickyGCMNF.MorphData();
                        TempMorphData.Morph = VectorPoint[a].MorphDatas[i];
                        TempMorphData.VertexIndex = a;
                        NewMorphHeader[i].MorphDataList.Add(TempMorphData);
                    }
                }
            }
            TempTrickyMesh.morphHeader = NewMorphHeader;

            indiceFaces = TristripGenerator.NeighbourPriority(indiceFaces);

            //Send to Tristrip Generator
            List<TristripGenerator.IndiceTristrip> indiceTristrips = TristripGenerator.GenerateTristripNivda(indiceFaces, 10000, true);
            if (indiceTristrips == null)
            {
                MessageBox.Show("Tristrip Failed to Generate");
                return;
            }

            //Generate Morph Header Data

            //For each tristrip check material number
            //if material number has data in the mesh header list go on to the next
            //If none found generate a new header
            TempTrickyMesh.meshHeaders = new List<TrickyGCMNF.MeshHeader>();

            for (int i = 0; i < indiceTristrips.Count; i++)
            {
                int MaterialID = indiceTristrips[i].MaterialID;

                int HeaderID = -1;

                for (int a = 0; a < TempTrickyMesh.meshHeaders.Count; a++)
                {
                    bool Added = false;
                    for (int b = 0; b < TempTrickyMesh.meshHeaders[a].indexGroupHeaders.Count; b++)
                    {
                        if (TempTrickyMesh.meshHeaders[a].indexGroupHeaders[b].MatIndex1 == MaterialID)
                        {
                            Added = true;
                            break;
                        }
                    }
                    if(!Added)
                    {
                        HeaderID = a;
                        break;
                    }
                }

                if (HeaderID == -1)
                {
                    HeaderID = TempTrickyMesh.meshHeaders.Count;
                    TrickyGCMNF.MeshHeader NewHeader = new TrickyGCMNF.MeshHeader();
                    NewHeader.indexGroupHeaders = new List<TrickyGCMNF.IndexGroupHeader>();
                    NewHeader.WeightIndex = new List<int>();
                    TempTrickyMesh.meshHeaders.Add(NewHeader);
                }

                var TempHeader = TempTrickyMesh.meshHeaders[HeaderID];
                TrickyGCMNF.IndexGroupHeader indexGroupHeader = new TrickyGCMNF.IndexGroupHeader();
                indexGroupHeader.MatIndex0 = MaterialID;
                indexGroupHeader.MatIndex1 = MaterialID;
                indexGroupHeader.MatIndex2 = MaterialID;
                indexGroupHeader.MatIndex3 = MaterialID;
                indexGroupHeader.MatIndex4 = MaterialID;
                TrickyGCMNF.IndexGroup indexGroup = new TrickyGCMNF.IndexGroup();

                indexGroup.indices = new List<TrickyGCMNF.Index>();
                indexGroup.shadowIndices = new List<TrickyGCMNF.ShadowIndex>();
                
                //Generate New Index List with Weight List
                //Add Index List
                for (int a = 0; a < indiceTristrips[i].Indices.Count; a++)
                {
                    if (!Shadow)
                    {
                        TrickyGCMNF.Index TrickyIndex = new TrickyGCMNF.Index();
                        if (!TempHeader.WeightIndex.Contains(VectorPoint[indiceTristrips[i].Indices[a]].WeightIndex))
                        {
                            TempHeader.WeightIndex.Add(VectorPoint[indiceTristrips[i].Indices[a]].WeightIndex);
                        }
                        TrickyIndex.WeightIndex = TempHeader.WeightIndex.IndexOf(VectorPoint[indiceTristrips[i].Indices[a]].WeightIndex);

                        TrickyIndex.Index0 = indiceTristrips[i].Indices[a];
                        TrickyIndex.Index1 = indiceTristrips[i].Indices[a];
                        TrickyIndex.Index2 = indiceTristrips[i].Indices[a];
                        indexGroup.indices.Add(TrickyIndex);
                    }
                    else
                    {
                        TrickyGCMNF.ShadowIndex TrickyIndex = new TrickyGCMNF.ShadowIndex();
                        if (!TempHeader.WeightIndex.Contains(VectorPoint[indiceTristrips[i].Indices[a]].WeightIndex))
                        {
                            TempHeader.WeightIndex.Add(VectorPoint[indiceTristrips[i].Indices[a]].WeightIndex);
                        }
                        TrickyIndex.WeightIndex = TempHeader.WeightIndex.IndexOf(VectorPoint[indiceTristrips[i].Indices[a]].WeightIndex);

                        TrickyIndex.Index = indiceTristrips[i].Indices[a];
                        indexGroup.shadowIndices.Add(TrickyIndex);
                    }
                }
                indexGroupHeader.indexGroup = indexGroup;

                TempHeader.indexGroupHeaders.Add(indexGroupHeader);
                TempTrickyMesh.meshHeaders[HeaderID] = TempHeader;
            }


            //set data to mxf
            TempTrickyMesh.Vertex = VectorPoint;

            Board.modelHeaders[Selected] = TempTrickyMesh;
        }

        public void StartRegenMeshCharacter(TrickyGCModelCombiner trickyModelCombiner, int MeshID)
        {
            if (trickyModelCombiner.bones == null)
            {
                MessageBox.Show("No Bones Detected");
                return;
            }
            bones = new List<TrickyGCMNF.BoneData>();
            for (int i = 0; i < Body.modelHeaders.Count; i++)
            {
                if ((MeshID == 0 && Body.modelHeaders[i].ModelName.Contains("3000")) ||
                    (MeshID == 1 && Body.modelHeaders[i].ModelName.Contains("1500")) ||
                    (MeshID == 2 && Body.modelHeaders[i].ModelName.Contains("750") && !Body.modelHeaders[i].ModelName.ToLower().Contains("shdw")) ||
                    (MeshID == 3 && Body.modelHeaders[i].ModelName.ToLower().Contains("shdw")))
                {
                    bones.AddRange(Body.modelHeaders[i].boneDatas);
                }
            }

            //Head
            for (int i = 0; i < Head.modelHeaders.Count; i++)
            {
                if ((MeshID == 0 && Head.modelHeaders[i].ModelName.Contains("3000")) ||
                    (MeshID == 1 && Head.modelHeaders[i].ModelName.Contains("1500")) ||
                    (MeshID == 2 && Head.modelHeaders[i].ModelName.Contains("750") && !Head.modelHeaders[i].ModelName.ToLower().Contains("shdw")) ||
                    (MeshID == 3 && Head.modelHeaders[i].ModelName.ToLower().Contains("shdw")))
                {
                    bones.AddRange(Head.modelHeaders[i].boneDatas);
                }
            }

            bool Shadow = false;

            if (MeshID == 3)
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

                for (int a = 0; a < Body.modelHeaders.Count; a++)
                {
                    if (Body.modelHeaders[a].ModelName.Trim().ToLower() == trickyModelCombiner.reassignedMesh[i].MeshName.Trim().ToLower())
                    {
                        TempReMesh.BodyHead = false;
                        TempReMesh.MeshId = a;
                        Reassinged = true;
                        break;
                    }
                }

                for (int a = 0; a < Head.modelHeaders.Count; a++)
                {
                    if (Head.modelHeaders[a].ModelName.Trim().ToLower() == trickyModelCombiner.reassignedMesh[i].MeshName.Trim().ToLower())
                    {
                        TempReMesh.BodyHead = true;
                        TempReMesh.MeshId = a;
                        Reassinged = true;
                        if (TempReMesh.MorphTargetCount != Head.modelHeaders[a].NumMorphs)
                        {
                            MessageBox.Show("Incorrect ammount of Shapekeys " + Head.modelHeaders[a].NumMorphs + "/" + TempReMesh.MorphTargetCount);
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

                TrickyGCMNF.ModelHeader TempTrickyMesh = new TrickyGCMNF.ModelHeader();

                if (!TempReMesh.BodyHead)
                {
                    TempTrickyMesh = Body.modelHeaders[TempReMesh.MeshId];
                }
                else
                {
                    TempTrickyMesh = Head.modelHeaders[TempReMesh.MeshId];
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
                TempTrickyMesh.materialDatas = new List<TrickyGCMNF.MaterialData>();
                for (int a = 0; a < TempReMesh.faces.Count; a++)
                {
                    if (!MaterialsID.Contains(TempReMesh.faces[a].MaterialID))
                    {
                        MaterialsID.Add(TempReMesh.faces[a].MaterialID);
                    }
                }

                for (int a = 0; a < MaterialsID.Count; a++)
                {
                    TrickyGCMNF.MaterialData MaterialData = new TrickyGCMNF.MaterialData();
                    RedoneMaterial.Add(a);
                    MaterialData.MainTexture = trickyModelCombiner.materials[MaterialsID[a]].MainTexture.Substring(0, 4).ToLower();
                    MaterialData.Texture1 = "";
                    MaterialData.Texture2 = "";
                    MaterialData.Texture3 = "";
                    MaterialData.Texture4 = "";

                    MaterialData.FactorFloat = 0.00392156885936856f;
                    MaterialData.Unused1Float = 0.00392156885936856f;
                    MaterialData.Unused2Float = 0.00392156885936856f;

                    string[] Split = trickyModelCombiner.materials[MaterialsID[a]].MainTexture.Split(' ');

                    if (Split.Length > 1)
                    {
                        if (Split[1].ToLower() == "gloss")
                        {
                            if (MaterialData.MainTexture == "suit")
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

                TrickyGCMNF.MaterialData TempMaterialData = new TrickyGCMNF.MaterialData();
                TempMaterialData.MainTexture = "head";
                TempTrickyMesh.materialDatas.Add(TempMaterialData);

                TempMaterialData = new TrickyGCMNF.MaterialData();
                TempMaterialData.MainTexture = "helm";
                TempTrickyMesh.materialDatas.Add(TempMaterialData);

                TempMaterialData = new TrickyGCMNF.MaterialData();
                TempMaterialData.MainTexture = "suit";
                TempTrickyMesh.materialDatas.Add(TempMaterialData);

                TempMaterialData = new TrickyGCMNF.MaterialData();
                TempMaterialData.MainTexture = "boot";
                TempTrickyMesh.materialDatas.Add(TempMaterialData);

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
                TempTrickyMesh.boneWeightHeaders = new List<TrickyGCMNF.BoneWeightHeader>();

                //Load Headers into file
                for (int a = 0; a < TempReMesh.faces.Count; a++)
                {
                    var TempFace = TempReMesh.faces[a];
                    int WeightID = ContainsWeight(TempFace.Weight1, TempTrickyMesh.boneWeightHeaders);
                    if (WeightID == -1)
                    {
                        TempTrickyMesh.boneWeightHeaders.Add(TempFace.Weight1);
                        WeightID = TempTrickyMesh.boneWeightHeaders.Count - 1;
                    }
                    TempFace.Weight1Pos = WeightID;

                    WeightID = ContainsWeight(TempFace.Weight2, TempTrickyMesh.boneWeightHeaders);
                    if (WeightID == -1)
                    {
                        TempTrickyMesh.boneWeightHeaders.Add(TempFace.Weight2);
                        WeightID = TempTrickyMesh.boneWeightHeaders.Count - 1;
                    }
                    TempFace.Weight2Pos = WeightID;

                    WeightID = ContainsWeight(TempFace.Weight3, TempTrickyMesh.boneWeightHeaders);
                    if (WeightID == -1)
                    {
                        TempTrickyMesh.boneWeightHeaders.Add(TempFace.Weight3);
                        WeightID = TempTrickyMesh.boneWeightHeaders.Count - 1;
                    }
                    TempFace.Weight3Pos = WeightID;

                    TempReMesh.faces[a] = TempFace;
                }

                //Fix Bone ID/FileIDs
                for (int a = 0; a < TempTrickyMesh.boneWeightHeaders.Count; a++)
                {
                    var TempBoneHeader = TempTrickyMesh.boneWeightHeaders[a];
                    for (int b = 0; b < TempBoneHeader.boneWeights.Count; b++)
                    {
                        var TempBoneWeight = TempBoneHeader.boneWeights[b];

                        var TempBone = FindBone(bones, TempBoneWeight.BoneName);

                        TempBoneWeight.BoneID = TempBone.BonePos;
                        TempBoneWeight.FileID = TempBone.FileID;

                        TempBoneHeader.boneWeights[b] = TempBoneWeight;
                    }
                    TempTrickyMesh.boneWeightHeaders[a] = TempBoneHeader;
                }



                //Take faces and Generate Indce faces and giant vertex list
                List<TrickyGCMNF.VertexData> VectorPoint = new List<TrickyGCMNF.VertexData>();
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
                        else if (TempReMesh.faces[a].Weight1Pos != VectorPoint[TempID].WeightIndex)
                        {
                            TestID++;
                        }
                        else
                        {
                            TestID++;
                            if (TempReMesh.faces[a].MaterialID != VectorPoint[TempID].MaterialID)
                            {

                            }
                            else if (!NormalsEqual(TempReMesh.faces[a].Normal1, VectorPoint[TempID].VertexNormal) && !NormalAverage)
                            {

                            }
                            else if (!UVEqual(TempReMesh.faces[a].UV1, VectorPoint[TempID].UV))
                            {

                            }
                            else
                            {
                                if (TempReMesh.MorphTargetCount != 0)
                                {
                                    if (!MorphPointsEqual(TempReMesh.faces[a].MorphPoint1, VectorPoint[TempID].MorphDatas))
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
                        else if (TempReMesh.faces[a].Weight2Pos != VectorPoint[TempID].WeightIndex)
                        {
                            TestID++;
                        }
                        else
                        {
                            TestID++;
                            if (TempReMesh.faces[a].MaterialID != VectorPoint[TempID].MaterialID)
                            {

                            }
                            else if (!NormalsEqual(TempReMesh.faces[a].Normal2, VectorPoint[TempID].VertexNormal) && !NormalAverage)
                            {

                            }
                            else if (!UVEqual(TempReMesh.faces[a].UV2, VectorPoint[TempID].UV))
                            {

                            }
                            else
                            {
                                if (TempReMesh.MorphTargetCount != 0)
                                {
                                    if (!MorphPointsEqual(TempReMesh.faces[a].MorphPoint2, VectorPoint[TempID].MorphDatas))
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
                        else if (TempReMesh.faces[a].Weight3Pos != VectorPoint[TempID].WeightIndex)
                        {
                            TestID++;
                        }
                        else
                        {
                            TestID++;
                            if (TempReMesh.faces[a].MaterialID != VectorPoint[TempID].MaterialID)
                            {

                            }
                            else if (!NormalsEqual(TempReMesh.faces[a].Normal3, VectorPoint[TempID].VertexNormal) && !NormalAverage)
                            {

                            }
                            else if (!UVEqual(TempReMesh.faces[a].UV3, VectorPoint[TempID].UV))
                            {

                            }
                            else
                            {
                                if (TempReMesh.MorphTargetCount != 0)
                                {
                                    if (!MorphPointsEqual(TempReMesh.faces[a].MorphPoint3, VectorPoint[TempID].MorphDatas))
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
                    }
                    #endregion

                    indiceFaces.Add(TempFace);
                }

                //Once in Vertex List Send Morph Data To MorphList
                List<TrickyGCMNF.MorphHeader> NewMorphHeader = new List<TrickyGCMNF.MorphHeader>();
                for (int z = 0; z < TempReMesh.MorphTargetCount; z++)
                {
                    var TempMorphHeader = new TrickyGCMNF.MorphHeader();
                    TempMorphHeader.MorphDataList = new List<TrickyGCMNF.MorphData>();
                    NewMorphHeader.Add(TempMorphHeader);
                }

                for (int z = 0; z < NewMorphHeader.Count; z++)
                {
                    for (int a = 0; a < VectorPoint.Count; a++)
                    {
                        if (VectorPoint[a].MorphDatas[z] != Vector3.Zero)
                        {
                            TrickyGCMNF.MorphData TempMorphData = new TrickyGCMNF.MorphData();
                            TempMorphData.Morph = VectorPoint[a].MorphDatas[z];
                            TempMorphData.VertexIndex = a;
                            NewMorphHeader[z].MorphDataList.Add(TempMorphData);
                        }
                    }
                }
                TempTrickyMesh.morphHeader = NewMorphHeader;

                indiceFaces = TristripGenerator.NeighbourPriority(indiceFaces);

                //Send to Tristrip Generator
                List<TristripGenerator.IndiceTristrip> indiceTristrips = TristripGenerator.GenerateTristripNivda(indiceFaces, 10000, true);
                if (indiceTristrips == null)
                {
                    MessageBox.Show("Tristrip Failed to Generate");
                    return;
                }

                //Generate Morph Header Data

                //For each tristrip check material number
                //if material number has data in the mesh header list go on to the next
                //If none found generate a new header
                TempTrickyMesh.meshHeaders = new List<TrickyGCMNF.MeshHeader>();

                for (int z = 0; z < indiceTristrips.Count; z++)
                {
                    int MaterialID = indiceTristrips[z].MaterialID;

                    int HeaderID = -1;

                    for (int a = 0; a < TempTrickyMesh.meshHeaders.Count; a++)
                    {
                        bool Added = false;
                        for (int b = 0; b < TempTrickyMesh.meshHeaders[a].indexGroupHeaders.Count; b++)
                        {
                            if (TempTrickyMesh.meshHeaders[a].indexGroupHeaders[b].MatIndex1 == MaterialID)
                            {
                                Added = true;
                                break;
                            }
                        }
                        if (!Added)
                        {
                            HeaderID = a;
                            break;
                        }
                    }

                    if (HeaderID == -1)
                    {
                        HeaderID = TempTrickyMesh.meshHeaders.Count;
                        TrickyGCMNF.MeshHeader NewHeader = new TrickyGCMNF.MeshHeader();
                        NewHeader.indexGroupHeaders = new List<TrickyGCMNF.IndexGroupHeader>();
                        NewHeader.WeightIndex = new List<int>();
                        TempTrickyMesh.meshHeaders.Add(NewHeader);
                    }

                    var TempHeader = TempTrickyMesh.meshHeaders[HeaderID];
                    TrickyGCMNF.IndexGroupHeader indexGroupHeader = new TrickyGCMNF.IndexGroupHeader();
                    indexGroupHeader.MatIndex0 = MaterialID;
                    indexGroupHeader.MatIndex1 = MaterialID;
                    indexGroupHeader.MatIndex2 = MaterialID;
                    indexGroupHeader.MatIndex3 = MaterialID;
                    indexGroupHeader.MatIndex4 = MaterialID;
                    TrickyGCMNF.IndexGroup indexGroup = new TrickyGCMNF.IndexGroup();

                    indexGroup.indices = new List<TrickyGCMNF.Index>();
                    indexGroup.shadowIndices = new List<TrickyGCMNF.ShadowIndex>();

                    //Generate New Index List with Weight List
                    //Add Index List
                    for (int a = 0; a < indiceTristrips[z].Indices.Count; a++)
                    {
                        if (!Shadow)
                        {
                            TrickyGCMNF.Index TrickyIndex = new TrickyGCMNF.Index();
                            if (!TempHeader.WeightIndex.Contains(VectorPoint[indiceTristrips[z].Indices[a]].WeightIndex))
                            {
                                TempHeader.WeightIndex.Add(VectorPoint[indiceTristrips[z].Indices[a]].WeightIndex);
                            }
                            TrickyIndex.WeightIndex = TempHeader.WeightIndex.IndexOf(VectorPoint[indiceTristrips[z].Indices[a]].WeightIndex);

                            TrickyIndex.Index0 = indiceTristrips[z].Indices[a];
                            TrickyIndex.Index1 = indiceTristrips[z].Indices[a];
                            TrickyIndex.Index2 = indiceTristrips[z].Indices[a];
                            indexGroup.indices.Add(TrickyIndex);
                        }
                        else
                        {
                            TrickyGCMNF.ShadowIndex TrickyIndex = new TrickyGCMNF.ShadowIndex();
                            if (!TempHeader.WeightIndex.Contains(VectorPoint[indiceTristrips[z].Indices[a]].WeightIndex))
                            {
                                TempHeader.WeightIndex.Add(VectorPoint[indiceTristrips[z].Indices[a]].WeightIndex);
                            }
                            TrickyIndex.WeightIndex = TempHeader.WeightIndex.IndexOf(VectorPoint[indiceTristrips[z].Indices[a]].WeightIndex);

                            TrickyIndex.Index = indiceTristrips[z].Indices[a];
                            indexGroup.shadowIndices.Add(TrickyIndex);
                        }
                    }
                    indexGroupHeader.indexGroup = indexGroup;

                    TempHeader.indexGroupHeaders.Add(indexGroupHeader);
                    TempTrickyMesh.meshHeaders[HeaderID] = TempHeader;
                }


                //set data to mxf
                TempTrickyMesh.Vertex = VectorPoint;


                if (!TempReMesh.BodyHead)
                {
                    Body.modelHeaders[TempReMesh.MeshId] = TempTrickyMesh;
                }
                else
                {
                    Head.modelHeaders[TempReMesh.MeshId] = TempTrickyMesh;
                }
                trickyModelCombiner.reassignedMesh[i] = TempReMesh;
            }
            MessageBox.Show("Import Sucessful");
        }


        static TrickyGCMNF.BoneData FindBone(List<TrickyGCMNF.BoneData> boneData, string BoneName)
        {
            for (int i = 0; i < boneData.Count; i++)
            {
                if (boneData[i].BoneName.ToLower() == BoneName.ToLower())
                {
                    return boneData[i];
                }
            }

            return new TrickyGCMNF.BoneData();
        }

        static TrickyGCMNF.VertexData GenerateVectorPoint(TrickyGCMNF.Face face, int Vertice)
        {
            TrickyGCMNF.VertexData vectorPoint = new TrickyGCMNF.VertexData();
            vectorPoint.MaterialID = face.MaterialID;
            if (Vertice == 1)
            {
                vectorPoint.Vertex = face.V1;
                vectorPoint.VertexNormal = face.Normal1;
                vectorPoint.UV = face.UV1;
                vectorPoint.WeightIndex = face.Weight1Pos;
                vectorPoint.MorphDatas = face.MorphPoint1;
            }

            if (Vertice == 2)
            {
                vectorPoint.Vertex = face.V2;
                vectorPoint.VertexNormal = face.Normal2;
                vectorPoint.UV = face.UV2;
                vectorPoint.WeightIndex = face.Weight2Pos;
                vectorPoint.MorphDatas = face.MorphPoint2;
            }

            if (Vertice == 3)
            {
                vectorPoint.Vertex = face.V3;
                vectorPoint.VertexNormal = face.Normal3;
                vectorPoint.UV = face.UV3;
                vectorPoint.WeightIndex = face.Weight3Pos;
                vectorPoint.MorphDatas = face.MorphPoint3;
            }

            //vectorPoint.Unknown1 = 1;
            //vectorPoint.Unknown2 = 0;
            //vectorPoint.Unknown3 = 1;
            //vectorPoint.Unknown4 = -1;

            return vectorPoint;
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

            if (TestMain == Vertex.Count)
            {
                return true;
            }

            return false;
        }

        static bool NormalsEqual(Vector3 normal1, Vector3 normal2)
        {
            if (normal1 == normal2)
            {
                return true;
            }
            return false;
        }

        static bool UVEqual(Vector2 Uv1, Vector2 Uv2)
        {
            if (Uv1 == Uv2)
            {
                return true;
            }
            return false;
        }

        static int ContainsVertice(Vector3 vector1, List<TrickyGCMNF.VertexData> Point, int TestInt = 0)
        {
            int TestID = 0;
            for (int i = 0; i < Point.Count; i++)
            {
                if (vector1.X <= Point[i].Vertex.X + 0.01f && vector1.X >= Point[i].Vertex.X - 0.01f && vector1.Y <= Point[i].Vertex.Y + 0.01f && vector1.Y >= Point[i].Vertex.Y - 0.01f && vector1.Z <= Point[i].Vertex.Z + 0.01f && vector1.Z >= Point[i].Vertex.Z - 0.01f)
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

        static int ContainsWeight(TrickyGCMNF.BoneWeightHeader boneWeight, List<TrickyGCMNF.BoneWeightHeader> boneWeightList)
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


        public struct ReassignedMesh
        {
            public string MeshName;
            public int MeshId;
            public bool BodyHead;
            public bool ShadowModel;
            public int MorphTargetCount;
            public List<Vector3> IKPoints;
            public List<TrickyGCMNF.Face> faces;
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
