﻿using System;
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
            CorrectBonesandFaces();
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
            else
            {
                MessageBox.Show("Only Boards Supported");
            }


        }

        //Redo Tristrip model
        //Manually add Unknown
        //Fix Index Find and Bool Find Of Weights

        public void StartRegenMeshBoard(TrickyModelCombiner trickyModelCombiner, int Selected)
        {
            if(trickyModelCombiner.bones==null)
            {
                MessageBox.Show("No Bones Detected");
                return;
            }
            if(trickyModelCombiner.bones.Count!=1)
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

            var ReassignedMesh = trickyModelCombiner.reassignedMesh[0];

            //Regenerate Materials

            //Redo Data In Correct Formats IE make Weight List and make faces use the positions. NOTE ENSURE THE FILE ID IS CORRECT
            TempTrickyMesh.boneWeightHeader = new List<TrickyMPFModelHandler.BoneWeightHeader>();
            
            //Load Headers into file
            for (int i = 0; i < ReassignedMesh.faces.Count; i++)
            {
                var TempFace = ReassignedMesh.faces[i];
                if (!ContainsWeight(TempFace.Weight1, TempTrickyMesh.boneWeightHeader))
                {
                    TempTrickyMesh.boneWeightHeader.Add(TempFace.Weight1);
                }
                TempFace.Weight1Pos = WeightIndex(TempFace.Weight1, TempTrickyMesh.boneWeightHeader);

                if (!ContainsWeight(TempFace.Weight2, TempTrickyMesh.boneWeightHeader))
                {
                    TempTrickyMesh.boneWeightHeader.Add(TempFace.Weight2);
                }
                TempFace.Weight2Pos = WeightIndex(TempFace.Weight2, TempTrickyMesh.boneWeightHeader);

                if (!ContainsWeight(TempFace.Weight3, TempTrickyMesh.boneWeightHeader))
                {
                    TempTrickyMesh.boneWeightHeader.Add(TempFace.Weight3);
                }
                TempFace.Weight3Pos = WeightIndex(TempFace.Weight3, TempTrickyMesh.boneWeightHeader);

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


            bool rotation = false;
            bool RunWhile = true;
            bool TestFailed = false;
            List<TristripStruct> tristripStructs = new List<TristripStruct>();
            TristripStruct tristrip = new TristripStruct();
            tristrip.normals = new List<Vector3>();
            tristrip.vertices = new List<Vector3>();
            tristrip.Weights = new List<int>();
            tristrip.TextureCords = new List<Vector4>();

            List<int> NewMaterials = new List<int>();
            while (RunWhile)
            {
                if (tristrip.vertices.Count != 0)
                {
                    //Tristrip data ensuring that all the material id is taken into account
                    bool EndedEarly = false;
                    for (int i = 0; i < ReassignedMesh.faces.Count; i++)
                    {
                        var TempFace = ReassignedMesh.faces[i];
                        if (TempFace.MaterialID == tristrip.Material && !TempFace.tristripped)
                        {
                            int Edge = SharesEdge(rotation, TempFace, tristrip, Shadow);
                            if (Edge == 1)
                            {
                                TempFace.tristripped = true;
                                rotation = !rotation;
                                tristrip.vertices.Add(TempFace.V1);
                                tristrip.normals.Add(TempFace.Normal1);
                                tristrip.TextureCords.Add(TempFace.UV1);
                                tristrip.Weights.Add(TempFace.Weight1Pos);
                                ReassignedMesh.faces[i] = TempFace;
                                EndedEarly = true;
                                break;
                            }
                            else if (Edge == 2)
                            {
                                TempFace.tristripped = true;
                                rotation = !rotation;
                                tristrip.vertices.Add(TempFace.V2);
                                tristrip.normals.Add(TempFace.Normal2);
                                tristrip.TextureCords.Add(TempFace.UV2);
                                tristrip.Weights.Add(TempFace.Weight2Pos);
                                ReassignedMesh.faces[i] = TempFace;
                                EndedEarly = true;
                                break;
                            }
                            else if (Edge == 3)
                            {
                                TempFace.tristripped = true;
                                rotation = !rotation;
                                tristrip.vertices.Add(TempFace.V3);
                                tristrip.normals.Add(TempFace.Normal3);
                                tristrip.TextureCords.Add(TempFace.UV3);
                                tristrip.Weights.Add(TempFace.Weight3Pos);
                                ReassignedMesh.faces[i] = TempFace;
                                EndedEarly = true;
                                break;
                            }
                        }
                        if(i >= ReassignedMesh.faces.Count-1)
                        {
                            EndedEarly = false;
                        }
                    }

                    if(!EndedEarly || tristrip.vertices.Count >= 20)
                    {
                        if (tristrip.vertices.Count < 6 && !TestFailed )
                        {
                            rotation = !rotation;
                            for (int i = 0; i < ReassignedMesh.faces.Count; i++)
                            {
                                var TempFace = ReassignedMesh.faces[i];
                                if (TempFace.MaterialID == tristrip.Material && !TempFace.tristripped)
                                {
                                    int Edge = SharesEdge2(rotation, TempFace, tristrip, Shadow);

                                    if (Edge != -1)
                                    {
                                        tristrip.vertices.Add(new Vector3(0, 0, 0));
                                        tristrip.normals.Add(new Vector3(0, 0, 0));
                                        tristrip.TextureCords.Add(new Vector4(0, 0, 0, 0));
                                        tristrip.Weights.Add(0);

                                        tristrip.vertices.Add(tristrip.vertices[tristrip.vertices.Count - 2]);
                                        tristrip.normals.Add(tristrip.normals[tristrip.normals.Count - 2]);
                                        tristrip.TextureCords.Add(tristrip.TextureCords[tristrip.TextureCords.Count - 2]);
                                        tristrip.Weights.Add(tristrip.Weights[tristrip.Weights.Count - 2]);

                                        if (Edge == 1)
                                        {
                                            TempFace.tristripped = true;
                                            int Index = tristrip.vertices.Count - 1;
                                            if (rotation)
                                            {
                                                tristrip.vertices[Index - 1] = TempFace.V2;
                                                tristrip.normals[Index - 1] = TempFace.Normal2;
                                                tristrip.TextureCords[Index - 1] = TempFace.UV2;
                                                tristrip.Weights[Index - 1] = TempFace.Weight2Pos;
                                            }
                                            else
                                            {
                                                tristrip.vertices[Index - 1] = TempFace.V3;
                                                tristrip.normals[Index - 1] = TempFace.Normal3;
                                                tristrip.TextureCords[Index - 1] = TempFace.UV3;
                                                tristrip.Weights[Index - 1] = TempFace.Weight3Pos;
                                            }
                                            rotation = !rotation;
                                            tristrip.vertices.Add(TempFace.V1);
                                            tristrip.normals.Add(TempFace.Normal1);
                                            tristrip.TextureCords.Add(TempFace.UV1);
                                            tristrip.Weights.Add(TempFace.Weight1Pos);
                                            ReassignedMesh.faces[i] = TempFace;
                                            break;
                                        }
                                        else if (Edge == 2)
                                        {
                                            TempFace.tristripped = true;
                                            int Index = tristrip.vertices.Count - 1;
                                            if (rotation)
                                            {
                                                tristrip.vertices[Index - 1] = TempFace.V3;
                                                tristrip.normals[Index - 1] = TempFace.Normal3;
                                                tristrip.TextureCords[Index - 1] = TempFace.UV3;
                                                tristrip.Weights[Index - 1] = TempFace.Weight3Pos;
                                            }
                                            else
                                            {
                                                tristrip.vertices[Index - 1] = TempFace.V1;
                                                tristrip.normals[Index - 1] = TempFace.Normal1;
                                                tristrip.TextureCords[Index - 1] = TempFace.UV1;
                                                tristrip.Weights[Index - 1] = TempFace.Weight1Pos;
                                            }

                                            rotation = !rotation;
                                            tristrip.vertices.Add(TempFace.V2);
                                            tristrip.normals.Add(TempFace.Normal2);
                                            tristrip.TextureCords.Add(TempFace.UV2);
                                            tristrip.Weights.Add(TempFace.Weight2Pos);
                                            ReassignedMesh.faces[i] = TempFace;
                                            break;
                                        }
                                        else if (Edge == 3)
                                        {
                                            TempFace.tristripped = true;

                                            int Index = tristrip.vertices.Count - 1;
                                            if (rotation)
                                            {
                                                tristrip.vertices[Index - 1] = TempFace.V1;
                                                tristrip.normals[Index - 1] = TempFace.Normal1;
                                                tristrip.TextureCords[Index - 1] = TempFace.UV1;
                                                tristrip.Weights[Index - 1] = TempFace.Weight1Pos;
                                            }
                                            else
                                            {
                                                tristrip.vertices[Index - 1] = TempFace.V2;
                                                tristrip.normals[Index - 1] = TempFace.Normal2;
                                                tristrip.TextureCords[Index - 1] = TempFace.UV2;
                                                tristrip.Weights[Index - 1] = TempFace.Weight2Pos;
                                            }
                                            rotation = !rotation;
                                            tristrip.vertices.Add(TempFace.V3);
                                            tristrip.normals.Add(TempFace.Normal3);
                                            tristrip.TextureCords.Add(TempFace.UV3);
                                            tristrip.Weights.Add(TempFace.Weight3Pos);
                                            ReassignedMesh.faces[i] = TempFace;
                                            break;
                                        }
                                    }
                                }
                                if (ReassignedMesh.faces.Count - 1 == i)
                                {
                                    TestFailed = true;
                                }
                            }
                        }
                        else
                        {
                            TestFailed = false;
                            tristripStructs.Add(tristrip);
                            tristrip = new TristripStruct();
                            tristrip.normals = new List<Vector3>();
                            tristrip.vertices = new List<Vector3>();
                            tristrip.Weights = new List<int>();
                            tristrip.TextureCords = new List<Vector4>();
                        }
                    }
                }
                else
                {
                    bool FullRunTest = false;
                    for (int i = 0; i < ReassignedMesh.faces.Count; i++)
                    {
                        var TempFace = ReassignedMesh.faces[i];
                        if(!TempFace.tristripped)
                        {
                            FullRunTest = true;
                            rotation = false;
                            TempFace.tristripped = true;
                            tristrip.vertices.Add(TempFace.V1);
                            tristrip.vertices.Add(TempFace.V2);
                            tristrip.vertices.Add(TempFace.V3);

                            tristrip.TextureCords.Add(TempFace.UV1);
                            tristrip.TextureCords.Add(TempFace.UV2);
                            tristrip.TextureCords.Add(TempFace.UV3);

                            tristrip.normals.Add(TempFace.Normal1);
                            tristrip.normals.Add(TempFace.Normal2);
                            tristrip.normals.Add(TempFace.Normal3);

                            tristrip.Weights.Add(TempFace.Weight1Pos);
                            tristrip.Weights.Add(TempFace.Weight2Pos);
                            tristrip.Weights.Add(TempFace.Weight3Pos);

                            tristrip.Material = TempFace.MaterialID;
                            if(!NewMaterials.Contains(TempFace.MaterialID))
                            {
                                NewMaterials.Add(TempFace.MaterialID);
                            }
                            ReassignedMesh.faces[i] = TempFace;
                            break;
                        }
                    }

                    if(!FullRunTest)
                    {
                        RunWhile = false;
                        break;
                    }
                }
            }
            
            //Static mesh that shit
            TempTrickyMesh.MeshGroups = new List<TrickyMPFModelHandler.GroupMainHeader>();
            List<TrickyMPFModelHandler.StaticMesh> meshList = new List<TrickyMPFModelHandler.StaticMesh>();

            for (int a = 0; a < NewMaterials.Count; a++)
            {
                TrickyMPFModelHandler.StaticMesh staticMesh = new TrickyMPFModelHandler.StaticMesh();
                staticMesh.Unknown1 = 14;
                staticMesh.Unknown2 = 114;
                staticMesh.MatieralID = NewMaterials[a];
                staticMesh.vertices = new List<Vector3>();
                staticMesh.uvNormals = new List<Vector3>();
                staticMesh.Strips = new List<int>();
                staticMesh.uv = new List<Vector4>();
                staticMesh.Weights = new List<int>();
                for (int i = 0; i < tristripStructs.Count; i++)
                {
                    if (tristripStructs[i].Material == NewMaterials[a])
                    {
                        if (staticMesh.vertices.Count + tristripStructs[i].vertices.Count <= 55)
                        {
                            staticMesh.Strips.Add(tristripStructs[i].vertices.Count);
                            staticMesh.vertices.AddRange(tristripStructs[i].vertices);
                            staticMesh.uv.AddRange(tristripStructs[i].TextureCords);
                            staticMesh.uvNormals.AddRange(tristripStructs[i].normals);
                            staticMesh.Weights.AddRange(tristripStructs[i].Weights);
                        }
                        else if (i > tristripStructs.Count - 3)
                        {
                            staticMesh.Strips.Add(tristripStructs[i].vertices.Count);
                            staticMesh.vertices.AddRange(tristripStructs[i].vertices);
                            staticMesh.uv.AddRange(tristripStructs[i].TextureCords);
                            staticMesh.uvNormals.AddRange(tristripStructs[i].normals);
                            staticMesh.Weights.AddRange(tristripStructs[i].Weights);
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
                            staticMesh.Strips.Add(tristripStructs[i].vertices.Count);
                            staticMesh.vertices.AddRange(tristripStructs[i].vertices);
                            staticMesh.uv.AddRange(tristripStructs[i].TextureCords);
                            staticMesh.uvNormals.AddRange(tristripStructs[i].normals);
                            staticMesh.Weights.AddRange(tristripStructs[i].Weights);
                        }
                    }
                }
                if (!staticMesh.Equals(new TrickyMPFModelHandler.StaticMesh()))
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
                    if (TempGroup.MaterialID == meshList[a].MatieralID)
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
            //TempTrickyMesh.iKPoints = ReassignedMesh.IKPoints;



            Board.ModelList[Selected] = TempTrickyMesh;
        }

        static bool VerticeEqual(Vector3 vector1,  Vector3 vector2)
        {
            if(vector1.X <= vector2.X + 0.01f && vector1.X >= vector2.X - 0.01f && vector1.Y <= vector2.Y + 0.01f && vector1.Y >= vector2.Y - 0.01f && vector1.Z <= vector2.Z + 0.01f && vector1.Z >= vector2.Z - 0.01f)
            {
                return true;
            }
            return false;
        }

        static bool NormalsEqual(Vector3 normal1, Vector3 normal2)
        {
            if((int)(normal1.X*32768f) == (int)(normal2.X * 32768f) && (int)(normal1.Y * 32768f) == (int)(normal2.Y * 32768f) && (int)(normal1.Z * 32768f) == (int)(normal2.Z * 32768f))
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

        static int SharesEdge(bool CounterClockwise, TrickyMPFModelHandler.Face TempFace, TristripStruct tristrip, bool LooseCheck)
        {
            int Index = tristrip.vertices.Count - 1;
            int Index2 = 0;
            int Index3 = 0;
            if (CounterClockwise)
            {
                Index2 = Index;
                Index3 = Index - 1;
            }
            else
            {
                Index2 = Index - 1;
                Index3 = Index;
            }

            if (VerticeEqual(TempFace.V1, tristrip.vertices[Index3]) && VerticeEqual(TempFace.V2, tristrip.vertices[Index2]))
            {
                if (TempFace.Weight1Pos == tristrip.Weights[Index3] && TempFace.Weight2Pos == tristrip.Weights[Index2])
                {
                    if (!LooseCheck)
                    {
                        if (NormalsEqual(TempFace.Normal1, tristrip.normals[Index3]) && NormalsEqual(TempFace.Normal2, tristrip.normals[Index2]))
                        {
                            if (UVEqual(TempFace.UV1, tristrip.TextureCords[Index3]) && UVEqual(TempFace.UV2, tristrip.TextureCords[Index2]))
                            {
                                return 3;
                            }
                        }
                    }
                    else
                    {
                        return 3;
                    }
                }
            }

            if (VerticeEqual(TempFace.V2, tristrip.vertices[Index3]) && VerticeEqual(TempFace.V3, tristrip.vertices[Index2]))
            {
                if (TempFace.Weight2Pos == tristrip.Weights[Index3] && TempFace.Weight3Pos == tristrip.Weights[Index2])
                {
                    if (!LooseCheck)
                    {
                        if (NormalsEqual(TempFace.Normal2, tristrip.normals[Index3]) && NormalsEqual(TempFace.Normal3, tristrip.normals[Index2]))
                        {
                            if (UVEqual(TempFace.UV2, tristrip.TextureCords[Index3]) && UVEqual(TempFace.UV3, tristrip.TextureCords[Index2]))
                            {
                                return 1;
                            }
                        }
                    }
                    else
                    {

                        return 1;
                    }
                }
            }

            if (VerticeEqual(TempFace.V3, tristrip.vertices[Index2]) && VerticeEqual(TempFace.V1, tristrip.vertices[Index3]))
            {
                if (TempFace.Weight3Pos == tristrip.Weights[Index2] && TempFace.Weight1Pos == tristrip.Weights[Index3])
                {
                    if (!LooseCheck)
                    {
                        if (NormalsEqual(TempFace.Normal3, tristrip.normals[Index2]) && NormalsEqual(TempFace.Normal1, tristrip.normals[Index3]))
                        {
                            if (UVEqual(TempFace.UV3, tristrip.TextureCords[Index2]) && UVEqual(TempFace.UV1, tristrip.TextureCords[Index3]))
                            {
                                return 2;
                            }
                        }
                    }
                    else
                    {
                        return 2;
                    }
                }
            }
            return -1;
        }

        static int SharesEdge2(bool CounterClockwise, TrickyMPFModelHandler.Face TempFace, TristripStruct tristrip, bool LooseCheck)
        {
            int Index = tristrip.vertices.Count - 1;
            int Index2 = 0;
            int Index3 = 0;

            tristrip.vertices.Add(new Vector3(0, 0, 0));
            tristrip.normals.Add(new Vector3(0, 0, 0));
            tristrip.TextureCords.Add(new Vector4(0, 0, 0, 0));
            tristrip.Weights.Add(0);

            tristrip.vertices.Add(tristrip.vertices[tristrip.vertices.Count - 2]);
            tristrip.normals.Add(tristrip.normals[tristrip.normals.Count - 2]);
            tristrip.TextureCords.Add(tristrip.TextureCords[tristrip.TextureCords.Count - 2]);
            tristrip.Weights.Add(tristrip.Weights[tristrip.Weights.Count - 2]);

            if (CounterClockwise)
            {
                Index2 = Index;
                Index3 = Index - 1;
            }
            else
            {
                Index2 = Index - 1;
                Index3 = Index;
            }

            if(CounterClockwise)
            {
                tristrip.vertices[Index - 1] = TempFace.V1;
                tristrip.normals[Index - 1] = TempFace.Normal1;
                tristrip.TextureCords[Index - 1] = TempFace.UV1;
                tristrip.Weights[Index - 1] = TempFace.Weight1Pos;
            }
            else
            {
                tristrip.vertices[Index - 1] = TempFace.V2;
                tristrip.normals[Index - 1] = TempFace.Normal2;
                tristrip.TextureCords[Index - 1] = TempFace.UV2;
                tristrip.Weights[Index - 1] = TempFace.Weight2Pos;
            }

            if (VerticeEqual(TempFace.V1, tristrip.vertices[Index3]) && VerticeEqual(TempFace.V2, tristrip.vertices[Index2]))
            {
                if (TempFace.Weight1Pos == tristrip.Weights[Index3] && TempFace.Weight2Pos == tristrip.Weights[Index2])
                {
                    if (!LooseCheck)
                    {
                        if (NormalsEqual(TempFace.Normal1, tristrip.normals[Index3]) && NormalsEqual(TempFace.Normal2, tristrip.normals[Index2]))
                        {
                            if (UVEqual(TempFace.UV1, tristrip.TextureCords[Index3]) && UVEqual(TempFace.UV2, tristrip.TextureCords[Index2]))
                            {
                                tristrip.vertices.RemoveRange(tristrip.vertices.Count - 3, 2);
                                tristrip.normals.RemoveRange(tristrip.vertices.Count - 3, 2);
                                tristrip.TextureCords.RemoveRange(tristrip.vertices.Count - 3, 2);
                                tristrip.Weights.RemoveRange(tristrip.vertices.Count - 3, 2);
                                return 3;
                            }
                        }
                    }
                    else
                    {
                        tristrip.vertices.RemoveRange(tristrip.vertices.Count - 3, 2);
                        tristrip.normals.RemoveRange(tristrip.vertices.Count - 3, 2);
                        tristrip.TextureCords.RemoveRange(tristrip.vertices.Count - 3, 2);
                        tristrip.Weights.RemoveRange(tristrip.vertices.Count - 3, 2);
                        return 3;
                    }
                }
            }

            if (CounterClockwise)
            {
                tristrip.vertices[Index - 1] = TempFace.V2;
                tristrip.normals[Index - 1] = TempFace.Normal2;
                tristrip.TextureCords[Index - 1] = TempFace.UV2;
                tristrip.Weights[Index - 1] = TempFace.Weight2Pos;
            }
            else
            {
                tristrip.vertices[Index - 1] = TempFace.V3;
                tristrip.normals[Index - 1] = TempFace.Normal3;
                tristrip.TextureCords[Index - 1] = TempFace.UV3;
                tristrip.Weights[Index - 1] = TempFace.Weight3Pos;
            }

            if (VerticeEqual(TempFace.V2, tristrip.vertices[Index3]) && VerticeEqual(TempFace.V3, tristrip.vertices[Index2]))
            {
                if (TempFace.Weight2Pos == tristrip.Weights[Index3] && TempFace.Weight3Pos == tristrip.Weights[Index2])
                {
                    if (!LooseCheck)
                    {
                        if (NormalsEqual(TempFace.Normal2, tristrip.normals[Index3]) && NormalsEqual(TempFace.Normal3, tristrip.normals[Index2]))
                        {
                            if (UVEqual(TempFace.UV2, tristrip.TextureCords[Index3]) && UVEqual(TempFace.UV3, tristrip.TextureCords[Index2]))
                            {
                                tristrip.vertices.RemoveRange(tristrip.vertices.Count - 3, 2);
                                tristrip.normals.RemoveRange(tristrip.vertices.Count - 3, 2);
                                tristrip.TextureCords.RemoveRange(tristrip.vertices.Count - 3, 2);
                                tristrip.Weights.RemoveRange(tristrip.vertices.Count - 3, 2);
                                return 1;
                            }
                        }
                    }
                    else
                    {
                        tristrip.vertices.RemoveRange(tristrip.vertices.Count - 3, 2);
                        tristrip.normals.RemoveRange(tristrip.vertices.Count - 3, 2);
                        tristrip.TextureCords.RemoveRange(tristrip.vertices.Count - 3, 2);
                        tristrip.Weights.RemoveRange(tristrip.vertices.Count - 3, 2);
                        return 1;
                    }
                }
            }

            if (CounterClockwise)
            {
                tristrip.vertices[Index - 1] = TempFace.V3;
                tristrip.normals[Index - 1] = TempFace.Normal3;
                tristrip.TextureCords[Index - 1] = TempFace.UV3;
                tristrip.Weights[Index - 1] = TempFace.Weight3Pos;
            }
            else
            {
                tristrip.vertices[Index - 1] = TempFace.V1;
                tristrip.normals[Index - 1] = TempFace.Normal1;
                tristrip.TextureCords[Index - 1] = TempFace.UV1;
                tristrip.Weights[Index - 1] = TempFace.Weight1Pos;
            }

            if (VerticeEqual(TempFace.V3, tristrip.vertices[Index3]) && VerticeEqual(TempFace.V1, tristrip.vertices[Index2]))
            {
                if (TempFace.Weight3Pos == tristrip.Weights[Index3] && TempFace.Weight1Pos == tristrip.Weights[Index2])
                {
                    if (!LooseCheck)
                    {
                        if (NormalsEqual(TempFace.Normal3, tristrip.normals[Index3]) && NormalsEqual(TempFace.Normal1, tristrip.normals[Index2]))
                        {
                            if (UVEqual(TempFace.UV3, tristrip.TextureCords[Index3]) && UVEqual(TempFace.UV1, tristrip.TextureCords[Index2]))
                            {
                                tristrip.vertices.RemoveRange(tristrip.vertices.Count - 3, 2);
                                tristrip.normals.RemoveRange(tristrip.vertices.Count - 3, 2);
                                tristrip.TextureCords.RemoveRange(tristrip.vertices.Count - 3, 2);
                                tristrip.Weights.RemoveRange(tristrip.vertices.Count - 3, 2);
                                return 2;
                            }
                        }
                    }
                    else
                    {
                        tristrip.vertices.RemoveRange(tristrip.vertices.Count - 3, 2);
                        tristrip.normals.RemoveRange(tristrip.vertices.Count - 3, 2);
                        tristrip.TextureCords.RemoveRange(tristrip.vertices.Count - 3, 2);
                        tristrip.Weights.RemoveRange(tristrip.vertices.Count - 3, 2);
                        return 2;
                    }
                }
            }

            tristrip.vertices.RemoveRange(tristrip.vertices.Count-3,2);
            tristrip.normals.RemoveRange(tristrip.vertices.Count - 3, 2);
            tristrip.TextureCords.RemoveRange(tristrip.vertices.Count - 3, 2);
            tristrip.Weights.RemoveRange(tristrip.vertices.Count - 3, 2);

            return -1;
        }


        static bool ContainsWeight(TrickyMPFModelHandler.BoneWeightHeader boneWeight, List<TrickyMPFModelHandler.BoneWeightHeader> boneWeightList)
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
                        return true;
                    }
                }
            }

            return false;
        }

        static int WeightIndex(TrickyMPFModelHandler.BoneWeightHeader boneWeight, List<TrickyMPFModelHandler.BoneWeightHeader> boneWeightList)
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




        public struct ReassignedMesh
        {
            public string MeshName;
            public bool ShadowModel;
            public int MorphTargetCount;
            public List<Vector3> IKPoints;
            public List<TrickyMPFModelHandler.Face> faces;
        }

        public struct TristripStruct
        {
            public bool Tristripped;
            public int Material;
            public List<Vector3> vertices;
            public List<Vector3> normals;
            public List<Vector4> TextureCords;
            public List<int> Weights;
            public List<TrickyMPFModelHandler.MorphKey> MorphKeyData;
        }

    }
}
