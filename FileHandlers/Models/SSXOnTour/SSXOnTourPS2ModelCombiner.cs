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

        public void SaveMPF(string Path, bool Compression)
        {
            modelHandlers.Save(Path, Compression);
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
            TempMesh.AltMorphTargetCount = TempModel.AltMorphCount;
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

        //Fix Shadow Model and Fix Alt Morph
        public void StartRegenMesh(SSXOnTourPS2ModelCombiner ssx3ModelCombiner, int MeshID)
        {
            //Mesh Count
            if (ssx3ModelCombiner.reassignedMesh.Count > 1)
            {
                MessageBox.Show("More than one mesh detected");
                return;
            }

            //Check Bones
            if (ssx3ModelCombiner.boneDatas.Count != boneDatasOrg.Count)
            {
                MessageBox.Show("Incorrect Ammount of Bones " + ssx3ModelCombiner.boneDatas.Count + "/" + boneDatasOrg.Count);
                return;
            }

            //Check Morph
            if (modelHandlers.ModelList[MeshID].MorphCount+ modelHandlers.ModelList[MeshID].AltMorphCount != ssx3ModelCombiner.reassignedMesh[0].faces[0].MorphPoint1.Count)
            {
                MessageBox.Show("Incorrect Morph Ammount " + ssx3ModelCombiner.reassignedMesh[0].faces[0].MorphPoint1.Count + "/" + (modelHandlers.ModelList[MeshID].MorphCount + modelHandlers.ModelList[MeshID].AltMorphCount).ToString());
                return;
            }


            bool Shadow = false;

            if (modelHandlers.ModelList[MeshID].MaterialGroupList[0].Type==17)
            {
                Shadow = true;
            }

            bool AltMorph = false;

            //Update Alt Morph
            if (modelHandlers.ModelList[MeshID].AltMorphList.Count != 0)
            {
                AltMorph = true;
                for (int i = 0; i < ssx3ModelCombiner.reassignedMesh.Count; i++)
                {
                    var TempMesh = ssx3ModelCombiner.reassignedMesh[i];
                    for (int a = 0; a < ssx3ModelCombiner.reassignedMesh[i].faces.Count; a++)
                    {
                        var TempFace = ssx3ModelCombiner.reassignedMesh[i].faces[a];

                        List<Vector3> MorphList1 = new List<Vector3>();
                        List<Vector3> MorphList2 = new List<Vector3>();
                        List<Vector3> MorphList3 = new List<Vector3>();

                        List<Vector3> AltMorphList1 = new List<Vector3>();
                        List<Vector3> AltMorphList2 = new List<Vector3>();
                        List<Vector3> AltMorphList3 = new List<Vector3>();

                        List<Vector3> AltNormalList1 = new List<Vector3>();
                        List<Vector3> AltNormalList2 = new List<Vector3>();
                        List<Vector3> AltNormalList3 = new List<Vector3>();


                        for (int b = 0; b < modelHandlers.ModelList[MeshID].MorphHeaderList.Count; b++)
                        {
                            MorphList1.Add(TempFace.MorphPoint1[b]);
                            MorphList2.Add(TempFace.MorphPoint2[b]);
                            MorphList3.Add(TempFace.MorphPoint3[b]);
                        }

                        for (int b = modelHandlers.ModelList[MeshID].MorphHeaderList.Count; b < modelHandlers.ModelList[MeshID].MorphHeaderList.Count + modelHandlers.ModelList[MeshID].AltMorphList.Count; b++)
                        {
                            AltMorphList1.Add(TempFace.AltMorphPoint1[b]);
                            AltMorphList2.Add(TempFace.AltMorphPoint2[b]);
                            AltMorphList3.Add(TempFace.AltMorphPoint3[b]);

                            AltNormalList1.Add(TempFace.AltMorphNormal1[b]);
                            AltNormalList2.Add(TempFace.AltMorphNormal2[b]);
                            AltNormalList3.Add(TempFace.AltMorphNormal3[b]);
                        }

                        TempFace.MorphPoint1 = MorphList1;
                        TempFace.MorphPoint2 = MorphList2;
                        TempFace.MorphPoint3 = MorphList3;

                        TempFace.AltMorphPoint1 = AltMorphList1;
                        TempFace.AltMorphPoint2 = AltMorphList2;
                        TempFace.AltMorphPoint3 = AltMorphList3;

                        TempFace.AltMorphNormal1 = AltNormalList1;
                        TempFace.AltMorphNormal2 = AltNormalList2;
                        TempFace.AltMorphNormal3 = AltNormalList3;

                        ssx3ModelCombiner.reassignedMesh[i].faces[a] = TempFace;
                    }
                    ssx3ModelCombiner.reassignedMesh[i] = TempMesh;
                }
            }
            else if (modelHandlers.ModelList[MeshID].MorphHeaderList.Count !=0)
            {
                for (int i = 0; i < ssx3ModelCombiner.reassignedMesh.Count; i++)
                {
                    var TempMesh = ssx3ModelCombiner.reassignedMesh[i];
                    for (int a = 0; a < ssx3ModelCombiner.reassignedMesh[i].faces.Count; a++)
                    {
                        var TempFace = ssx3ModelCombiner.reassignedMesh[i].faces[a];

                        TempFace.AltMorphNormal1 = new List<Vector3>();
                        TempFace.AltMorphNormal1 = new List<Vector3>();
                        TempFace.AltMorphNormal1 = new List<Vector3>();

                        ssx3ModelCombiner.reassignedMesh[i].faces[a] = TempFace;
                    }
                    ssx3ModelCombiner.reassignedMesh[i] = TempMesh;
                }
            }


            List<Vector3> BonePositions = new List<Vector3>();
            if (UpdateBones)
            {
                //DeepCloneBones
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


                for (int a = 0; a < modelHandlers.ModelList[MeshID].BoneList.Count; a++)
                {
                    var TempBone = modelHandlers.ModelList[MeshID].BoneList[a];
                    for (int b = 0; b < ssx3ModelCombiner.boneDatas.Count; b++)
                    {
                        var TempNewBone = ssx3ModelCombiner.boneDatas[b];
                        if (TempBone.BoneName == TempNewBone.BoneName)
                        {
                            //TempBone.Position = TempNewBone.Position;
                            //TempBone.Rotation = TempNewBone.Rotation;

                            //BonePositions.Add(TempWorldPos);
                            ////Parent Point to Original 
                            ReshuffleBones();
                            FixBoneParents();

                            var bindings = new List<SharpGLTF.Scenes.NodeBuilder>();
                            SharpGLTF.Scenes.NodeBuilder Binding = new SharpGLTF.Scenes.NodeBuilder();
                            for (int i = 0; i < boneDatas.Count; i++)
                            {
                                if (boneDatas[i].ParentBone == -1)
                                {
                                    Binding = new SharpGLTF.Scenes.NodeBuilder();
                                }
                                else
                                {
                                    Binding = bindings[boneDatas[i].ParentBone];
                                }
                                Binding = Binding.CreateNode(boneDatas[i].BoneName);

                                Binding.WithLocalRotation(boneDatas[i].Rotation);
                                Binding.WithLocalTranslation(new Vector3(boneDatas[i].Position.X, boneDatas[i].Position.Y, boneDatas[i].Position.Z));

                                Binding.LocalMatrix = Binding.LocalMatrix;
                                bindings.Add(Binding);
                            }


                            Matrix4x4 matrix4X4 = new Matrix4x4();
                            if (TempBone.ParentBone == -1)
                            {
                                matrix4X4 = Matrix4x4.CreateTranslation(new Vector3());
                            }
                            else
                            {
                                for (int i = 0; i < bindings.Count; i++)
                                {
                                    if (TempNewBone.parentName == bindings[i].Name)
                                    {
                                        matrix4X4 = bindings[i].WorldMatrix;
                                    }
                                }
                            }

                            Matrix4x4 Inverted = new Matrix4x4();
                            Matrix4x4.Invert(matrix4X4, out Inverted);
                            //Inverted = Matrix4x4.Transpose(Inverted);


                            //Vector3 NewPos = Vector3.Transform(TempWorldPos, Inverted);


                            //TempBone.Position = new Vector4(NewPos.X, NewPos.Y, NewPos.Z, 1);
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


                    modelHandlers.ModelList[MeshID].BoneList[a] = TempBone;
                }

            }

            for (int i = 0; i < ssx3ModelCombiner.reassignedMesh.Count; i++)
            {
                var TempReMesh = ssx3ModelCombiner.reassignedMesh[i];
                TempReMesh.MorphTargetCount = modelHandlers.ModelList[MeshID].MorphCount;
                SSXOnTourMPF.MPFHeader TempTrickyMesh = modelHandlers.ModelList[MeshID];

                TempTrickyMesh.TriangleCount = ssx3ModelCombiner.reassignedMesh[i].faces.Count;

                //fix to be a proper regeneration
                TempTrickyMesh.MaterialList = ssx3ModelCombiner.materials;

                //Generate Weight List
                //Redo Data In Correct Formats IE make Weight List and make faces use the positions.
                TempTrickyMesh.BoneWeightHeaderList = new List<SSXOnTourMPF.BoneWeightHeader>();
                for (int a = 0; a < TempReMesh.faces.Count; a++)
                {
                    var TempFace = TempReMesh.faces[a];
                    int WeightID = ContainsWeight(TempFace.Weight1, TempTrickyMesh.BoneWeightHeaderList);
                    if (WeightID == -1)
                    {
                        TempTrickyMesh.BoneWeightHeaderList.Add(TempFace.Weight1);
                        WeightID = TempTrickyMesh.BoneWeightHeaderList.Count - 1;
                    }
                    TempFace.Weight1Pos = WeightID;

                    WeightID = ContainsWeight(TempFace.Weight2, TempTrickyMesh.BoneWeightHeaderList);
                    if (WeightID == -1)
                    {
                        TempTrickyMesh.BoneWeightHeaderList.Add(TempFace.Weight2);
                        WeightID = TempTrickyMesh.BoneWeightHeaderList.Count - 1;
                    }
                    TempFace.Weight2Pos = WeightID;

                    WeightID = ContainsWeight(TempFace.Weight3, TempTrickyMesh.BoneWeightHeaderList);
                    if (WeightID == -1)
                    {
                        TempTrickyMesh.BoneWeightHeaderList.Add(TempFace.Weight3);
                        WeightID = TempTrickyMesh.BoneWeightHeaderList.Count - 1;
                    }
                    TempFace.Weight3Pos = WeightID;

                    TempReMesh.faces[a] = TempFace;
                }
                //Fix Bone ID/FileIDs
                for (int a = 0; a < TempTrickyMesh.BoneWeightHeaderList.Count; a++)
                {
                    var TempBoneHeader = TempTrickyMesh.BoneWeightHeaderList[a];
                    for (int b = 0; b < TempBoneHeader.BoneWeightList.Count; b++)
                    {
                        var TempBoneWeight = TempBoneHeader.BoneWeightList[b];

                        var TempBone = FindBone(boneDatasOrg, TempBoneWeight.boneName);

                        TempBoneWeight.BoneID = TempBone.BonePos;
                        TempBoneWeight.FileID = TempBone.FileID;

                        TempBoneHeader.BoneWeightList[b] = TempBoneWeight;
                    }
                    TempTrickyMesh.BoneWeightHeaderList[a] = TempBoneHeader;
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
                                    else if (AltMorph)
                                    {
                                        if (!AltMorphPointsEqual(TempReMesh.faces[a].MorphPoint1, TempReMesh.faces[a].AltMorphNormal1, VectorPoint[TempID].AltMorph, VectorPoint[TempID].AltNormals))
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
                                else if (AltMorph)
                                {
                                    if (!AltMorphPointsEqual(TempReMesh.faces[a].MorphPoint1, TempReMesh.faces[a].AltMorphNormal1, VectorPoint[TempID].AltMorph, VectorPoint[TempID].AltNormals))
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
                                    else if (AltMorph)
                                    {
                                        if (!AltMorphPointsEqual(TempReMesh.faces[a].MorphPoint2, TempReMesh.faces[a].AltMorphNormal2, VectorPoint[TempID].AltMorph, VectorPoint[TempID].AltNormals))
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
                                else if (AltMorph)
                                {
                                    if (!AltMorphPointsEqual(TempReMesh.faces[a].MorphPoint2, TempReMesh.faces[a].AltMorphNormal2, VectorPoint[TempID].AltMorph, VectorPoint[TempID].AltNormals))
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
                                    else if (AltMorph)
                                    {
                                        if (!AltMorphPointsEqual(TempReMesh.faces[a].MorphPoint3, TempReMesh.faces[a].AltMorphNormal3, VectorPoint[TempID].AltMorph, VectorPoint[TempID].AltNormals))
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
                                else if (AltMorph)
                                {
                                    if (!AltMorphPointsEqual(TempReMesh.faces[a].MorphPoint3, TempReMesh.faces[a].AltMorphNormal3, VectorPoint[TempID].AltMorph, VectorPoint[TempID].AltNormals))
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

                //Send to Tristrip Generator
                indiceFaces = TristripGenerator.NeighbourPriority(indiceFaces);
                List<TristripGenerator.IndiceTristrip> indiceTristrips = TristripGenerator.GenerateTristripNivda(indiceFaces);

                if (indiceTristrips == null)
                {
                    MessageBox.Show("Tristrip Failed to Generate");
                    return;
                }

                //Static mesh that shit
                TempTrickyMesh.MaterialGroupList = new List<SSXOnTourMPF.MaterialGroup>();
                List<SSXOnTourMPF.MeshChunk> meshList = new List<SSXOnTourMPF.MeshChunk>();

                for (int a = 0; a < TempTrickyMesh.MaterialList.Count; a++)
                {
                    while (true)
                    {
                        SSXOnTourMPF.MeshChunk staticMesh = new SSXOnTourMPF.MeshChunk();
                        staticMesh.weightsInts = new List<int>();

                        staticMesh.Unknown1 = 17;
                        if (!Shadow)
                        {
                            staticMesh.Unknown2 = 117;
                        }
                        staticMesh.MatieralID = a;
                        staticMesh.Vertices = new List<Vector3>();
                        staticMesh.UVNormals = new List<Vector3>();
                        staticMesh.Strips = new List<int>();
                        staticMesh.UV = new List<Vector4>();
                        staticMesh.Weights = new List<int>();
                        staticMesh.MorphKeys = new List<SSXOnTourMPF.MorphKey>();
                        staticMesh.AltMorphChunks = new List<SSXOnTourMPF.AltMorphChunk>();

                        if (TempReMesh.MorphTargetCount != 0)
                        {
                            for (int b = 0; b < TempReMesh.MorphTargetCount; b++)
                            {
                                var NewKey = new SSXOnTourMPF.MorphKey();
                                NewKey.MorphDataList = new List<SSXOnTourMPF.MorphData>();
                                staticMesh.MorphKeys.Add(NewKey);
                            }
                        }

                        //Generate Alt Morph Chunk
                        if(AltMorph)
                        {
                            for (int ei = 0; ei < TempReMesh.AltMorphTargetCount; ei++)
                            {
                                var NewAltMorph = new SSXOnTourMPF.AltMorphChunk();
                                NewAltMorph.UV = new List<Vector4>();
                                NewAltMorph.Position = new List<Vector3>();
                                NewAltMorph.Normal = new List<Vector4>();
                                staticMesh.AltMorphChunks.Add(NewAltMorph);
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

                                if (staticMesh.Vertices.Count + indiceTristrips[b].Indices.Count <= 55 && MeshGroupSubTest)
                                {
                                    var TempIndice = indiceTristrips[b];
                                    TempIndice.Used = true;
                                    indiceTristrips[b] = TempIndice;

                                    staticMesh.Strips.Add(indiceTristrips[b].Indices.Count);
                                    for (int d = 0; d < indiceTristrips[b].Indices.Count; d++)
                                    {
                                        staticMesh.Vertices.Add(VectorPoint[indiceTristrips[b].Indices[d]].vector);
                                        staticMesh.UV.Add(VectorPoint[indiceTristrips[b].Indices[d]].TextureCord);
                                        staticMesh.UVNormals.Add(VectorPoint[indiceTristrips[b].Indices[d]].normal);
                                        staticMesh.Weights.Add(VectorPoint[indiceTristrips[b].Indices[d]].Weight);

                                        if (TempReMesh.MorphTargetCount != 0)
                                        {
                                            var TempMorphTargets = VectorPoint[indiceTristrips[b].Indices[d]].MorphData;

                                            for (int ei = 0; ei < TempReMesh.MorphTargetCount; ei++)
                                            {
                                                SSXOnTourMPF.MorphData TempMorph = new SSXOnTourMPF.MorphData();
                                                TempMorph.vector3 = TempMorphTargets[ei];
                                                TempMorph.ID = staticMesh.Vertices.Count - 1;
                                                if (TempMorph.vector3 != Vector3.Zero)
                                                {
                                                    staticMesh.MorphKeys[ei].MorphDataList.Add(TempMorph);
                                                }
                                            }
                                        }
                                        //Add Points To Alt Morph Chunk
                                        if(AltMorph)
                                        {
                                            for (int ei = 0; ei < TempReMesh.AltMorphTargetCount; ei++)
                                            {
                                                var TempAltMorph = staticMesh.AltMorphChunks[ei];

                                                TempAltMorph.UV.Add(new Vector4());
                                                TempAltMorph.Normal.Add(new Vector4(VectorPoint[indiceTristrips[b].Indices[d]].AltNormals[ei].X, VectorPoint[indiceTristrips[b].Indices[d]].AltNormals[ei].Y, VectorPoint[indiceTristrips[b].Indices[d]].AltNormals[ei].Z, 0));
                                                TempAltMorph.Position.Add(VectorPoint[indiceTristrips[b].Indices[d]].AltMorph[ei]);
                                                staticMesh.AltMorphChunks[ei] = TempAltMorph;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (staticMesh.Vertices.Count != 0)
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

                //Reset AltMorph
                for (int ei = 0; ei < TempTrickyMesh.AltMorphList.Count; ei++)
                {
                    var TempAltMorph = TempTrickyMesh.AltMorphList[ei];

                    TempAltMorph.MorphChunkList = new List<SSXOnTourMPF.AltMorphChunk>();

                    TempTrickyMesh.AltMorphList[ei] = TempAltMorph;
                }


                //Group that shit MK2
                while (true)
                {
                    bool FirstAdd = true;
                    for (int b = 0; b < meshList.Count; b++)
                    {
                        bool Added = false;

                        bool MaterialGroupTest = false;

                        for (int a = 0; a < TempTrickyMesh.MaterialGroupList.Count; a++)
                        {
                            if (meshList[b].MatieralID == TempTrickyMesh.MaterialGroupList[a].Material)
                            {
                                MaterialGroupTest = true;
                                if (!meshList[b].Grouped)
                                {
                                    var TempMaterialGroup = TempTrickyMesh.MaterialGroupList[a];
                                    bool WeightRefGroupTest = false;

                                    for (int c = 0; c < TempMaterialGroup.WeightRefList.Count; c++)
                                    {
                                        var TempWeightRefGroup = TempMaterialGroup.WeightRefList[c];

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
                                            if (TempTrickyMesh.MorphCount != 0)
                                            {
                                                //Add New
                                                SSXOnTourMPF.MorphMeshGroup NewMorphHeader = new SSXOnTourMPF.MorphMeshGroup();
                                                NewMorphHeader.MeshChunkList = new List<SSXOnTourMPF.MeshChunk>();
                                                NewMorphHeader.MorphDataList = new List<SSXOnTourMPF.MorphKey>();
                                                NewMorphHeader.MeshChunkList.Add(meshList[b]);

                                                //Alt Morph
                                                for (int ei = 0; ei < TempTrickyMesh.AltMorphList.Count; ei++)
                                                {
                                                    var TempAltMorph = TempTrickyMesh.AltMorphList[ei];

                                                    TempAltMorph.MorphChunkList.Add(meshList[b].AltMorphChunks[ei]);

                                                    TempTrickyMesh.AltMorphList[ei] = TempAltMorph;
                                                }

                                                NewMorphHeader.MorphDataList = meshList[b].MorphKeys;
                                                TempWeightRefGroup.MorphMeshGroupList.Add(NewMorphHeader);

                                            }
                                            else
                                            {
                                                //Add to zero
                                                if (TempWeightRefGroup.MorphMeshGroupList.Count == 0)
                                                {
                                                    SSXOnTourMPF.MorphMeshGroup TempMorphHeader = new SSXOnTourMPF.MorphMeshGroup();
                                                    TempMorphHeader.MeshChunkList = new List<SSXOnTourMPF.MeshChunk>();
                                                    TempMorphHeader.MorphDataList = new List<SSXOnTourMPF.MorphKey>();
                                                    TempWeightRefGroup.MorphMeshGroupList.Add(TempMorphHeader);
                                                }
                                                SSXOnTourMPF.MorphMeshGroup NewMorphHeader = TempWeightRefGroup.MorphMeshGroupList[0];

                                                NewMorphHeader.MeshChunkList.Add(meshList[b]);

                                                //Alt Morph Add
                                                for (int ei = 0; ei < TempTrickyMesh.AltMorphList.Count; ei++)
                                                {
                                                    var TempAltMorph = TempTrickyMesh.AltMorphList[ei];

                                                    TempAltMorph.MorphChunkList.Add(meshList[b].AltMorphChunks[ei]);

                                                    TempTrickyMesh.AltMorphList[ei] = TempAltMorph;
                                                }

                                                TempWeightRefGroup.MorphMeshGroupList[0] = NewMorphHeader;

                                            }
                                            FirstAdd = false;
                                            TempMaterialGroup.WeightRefList[c] = TempWeightRefGroup;
                                            break;
                                        }

                                        TempMaterialGroup.WeightRefList[c] = TempWeightRefGroup;
                                    }
                                    //If false wait till start of list reading and make new one

                                    if (FirstAdd && !WeightRefGroupTest)
                                    {
                                        SSXOnTourMPF.WeightRefGroup weightRefGroup = new SSXOnTourMPF.WeightRefGroup();
                                        weightRefGroup.MorphMeshGroupList = new List<SSXOnTourMPF.MorphMeshGroup>();
                                        weightRefGroup.weights = new List<int>();
                                        TempMaterialGroup.WeightRefList.Add(weightRefGroup);
                                        TempTrickyMesh.MaterialGroupList[a] = TempMaterialGroup;
                                        b--;
                                        break;
                                    }
                                    TempTrickyMesh.MaterialGroupList[a] = TempMaterialGroup;
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
                            var NewMaterialGroup = new SSXOnTourMPF.MaterialGroup();
                            NewMaterialGroup.Material = meshList[b].MatieralID;
                            if (Shadow)
                            {
                                NewMaterialGroup.Type = 17;
                            }
                            else if (TempReMesh.MorphTargetCount != 0)
                            {
                                NewMaterialGroup.Type = 256;
                            }
                            else
                            {
                                NewMaterialGroup.Type = 1;
                            }
                            NewMaterialGroup.Unknown = -1;
                            NewMaterialGroup.WeightRefList = new List<SSXOnTourMPF.WeightRefGroup>();
                            TempTrickyMesh.MaterialGroupList.Add(NewMaterialGroup);
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
                TempTrickyMesh.WeightRefrenceList = new List<SSXOnTourMPF.WeightRefList>();
                for (int ei = 0; ei < TempTrickyMesh.MaterialGroupList.Count; ei++)
                {
                    var TempMeshGroup = TempTrickyMesh.MaterialGroupList[ei];
                    for (int a = 0; a < TempMeshGroup.WeightRefList.Count; a++)
                    {
                        var TempSubGroup = TempMeshGroup.WeightRefList[a];
                        SSXOnTourMPF.WeightRefList NumberRef = new SSXOnTourMPF.WeightRefList();
                        NumberRef.WeightIDs = new List<int>();
                        for (int b = 0; b < TempSubGroup.MorphMeshGroupList.Count; b++)
                        {
                            var TempMeshGroupHeader = TempSubGroup.MorphMeshGroupList[b];

                            for (int c = 0; c < TempMeshGroupHeader.MeshChunkList.Count; c++)
                            {
                                var TempMesh = TempMeshGroupHeader.MeshChunkList[c];
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
                                        var TempUV = TempMesh.UV[d];
                                        TempUV.Z = TempMesh.Weights[d] * 4 + 17;
                                        TempUV.W = TempMesh.Weights[d] * 3 + 117;
                                        TempMesh.UV[d] = TempUV;
                                    }
                                }
                                else
                                {
                                    for (int d = 0; d < TempMesh.Weights.Count; d++)
                                    {
                                        TempMesh.Weights[d] = TempMesh.Weights[d] * 4 + 17;
                                    }
                                }
                                TempMeshGroupHeader.MeshChunkList[c] = TempMesh;
                            }


                            TempSubGroup.MorphMeshGroupList[b] = TempMeshGroupHeader;
                        }
                        TempTrickyMesh.WeightRefrenceList.Add(NumberRef);
                        TempMeshGroup.WeightRefList[a] = TempSubGroup;
                    }
                    TempTrickyMesh.MaterialGroupList[ei] = TempMeshGroup;
                }

                modelHandlers.ModelList[MeshID] = TempTrickyMesh;
            }

        }

        static VectorPoint GenerateVectorPoint(SSXOnTourMPF.Face face, int Vertice)
        {
            VectorPoint vectorPoint = new VectorPoint();
            vectorPoint.Material = face.MaterialID;
            if (Vertice == 1)
            {
                vectorPoint.vector = face.V1;
                vectorPoint.normal = face.Normal1;
                vectorPoint.TextureCord = face.UV1;
                vectorPoint.Weight = face.Weight1Pos;
                vectorPoint.MorphData = face.MorphPoint1;
                vectorPoint.AltMorph = face.AltMorphPoint1;
                vectorPoint.AltNormals = face.AltMorphNormal1;
            }

            if (Vertice == 2)
            {
                vectorPoint.vector = face.V2;
                vectorPoint.normal = face.Normal2;
                vectorPoint.TextureCord = face.UV2;
                vectorPoint.Weight = face.Weight2Pos;
                vectorPoint.MorphData = face.MorphPoint2;
                vectorPoint.AltMorph = face.AltMorphPoint2;
                vectorPoint.AltNormals = face.AltMorphNormal2;
            }

            if (Vertice == 3)
            {
                vectorPoint.vector = face.V3;
                vectorPoint.normal = face.Normal3;
                vectorPoint.TextureCord = face.UV3;
                vectorPoint.Weight = face.Weight3Pos;
                vectorPoint.MorphData = face.MorphPoint3;
                vectorPoint.AltMorph = face.AltMorphPoint3;
                vectorPoint.AltNormals = face.AltMorphNormal3;
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

            if (TestMain == Vertex.Count)
            {
                return true;
            }

            return false;
        }

        static bool AltMorphPointsEqual(List<Vector3> Vertex, List<Vector3> Normal, List<Vector3> ListVertex, List<Vector3> ListNormal)
        {
            int TestMain = 0;
            for (int i = 0; i < Vertex.Count; i++)
            {
                if (ListVertex[i] == Vertex[i])
                {
                    TestMain++;
                }
            }

            for (int i = 0; i < Normal.Count; i++)
            {
                if (ListNormal[i] == Normal[i])
                {
                    TestMain++;
                }
            }

            if (TestMain == Vertex.Count * 2)
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

        static int ContainsWeight(SSXOnTourMPF.BoneWeightHeader boneWeight, List<SSXOnTourMPF.BoneWeightHeader> boneWeightList)
        {
            for (int i = 0; i < boneWeightList.Count; i++)
            {
                if (boneWeightList[i].BoneWeightList.Count == boneWeight.BoneWeightList.Count)
                {
                    bool Test = false;
                    for (int a = 0; a < boneWeightList[i].BoneWeightList.Count; a++)
                    {
                        if (boneWeightList[i].BoneWeightList[a].Weight == boneWeight.BoneWeightList[a].Weight && boneWeightList[i].BoneWeightList[a].BoneID == boneWeight.BoneWeightList[a].BoneID)
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

        static SSXOnTourMPF.BoneData FindBone(List<SSXOnTourMPF.BoneData> boneData, string BoneName)
        {
            for (int i = 0; i < boneData.Count; i++)
            {
                if (boneData[i].BoneName.ToLower() == BoneName.ToLower())
                {
                    return boneData[i];
                }
            }

            return new SSXOnTourMPF.BoneData();
        }

        public int TristripCount(SSXOnTourMPF.MPFHeader modelHeader)
        {
            int Count = 0;

            var TempMesh = modelHeader.MaterialGroupList;

            for (int i = 0; i < TempMesh.Count; i++)
            {
                for (int a = 0; a < TempMesh[i].WeightRefList.Count; a++)
                {
                    for (int b = 0; b < TempMesh[i].WeightRefList[a].MorphMeshGroupList.Count; b++)
                    {
                        for (int c = 0; c < TempMesh[i].WeightRefList[a].MorphMeshGroupList[b].MeshChunkList.Count; c++)
                        {
                            Count += TempMesh[i].WeightRefList[a].MorphMeshGroupList[b].MeshChunkList[c].Strips.Count;
                        }
                    }
                }
            }

            return Count;
        }

        public int VerticeCount(SSXOnTourMPF.MPFHeader modelHeader)
        {
            int Count = 0;
            var TempMesh = modelHeader.MaterialGroupList;

            for (int i = 0; i < TempMesh.Count; i++)
            {
                for (int a = 0; a < TempMesh[i].WeightRefList.Count; a++)
                {
                    for (int b = 0; b < TempMesh[i].WeightRefList[a].MorphMeshGroupList.Count; b++)
                    {
                        for (int c = 0; c < TempMesh[i].WeightRefList[a].MorphMeshGroupList[b].MeshChunkList.Count; c++)
                        {
                            Count += TempMesh[i].WeightRefList[a].MorphMeshGroupList[b].MeshChunkList[c].Vertices.Count;
                        }
                    }
                }
            }

            return Count;
        }

        public int ChunkCount(SSXOnTourMPF.MPFHeader modelHeader)
        {
            int Count = 0;
            var TempMesh = modelHeader.MaterialGroupList;

            for (int i = 0; i < TempMesh.Count; i++)
            {
                for (int a = 0; a < TempMesh[i].WeightRefList.Count; a++)
                {
                    for (int b = 0; b < TempMesh[i].WeightRefList[a].MorphMeshGroupList.Count; b++)
                    {
                        Count += TempMesh[i].WeightRefList[a].MorphMeshGroupList[b].MeshChunkList.Count;
                    }
                }
            }

            return Count;
        }

        public int WeigthRefCount(SSXOnTourMPF.MPFHeader modelHeader)
        {
            int Count = 0;
            var TempMesh = modelHeader.MaterialGroupList;

            for (int i = 0; i < TempMesh.Count; i++)
            {
                Count += TempMesh[i].WeightRefList.Count;
            }

            return Count;
        }

        public int MorphGroupCount(SSXOnTourMPF.MPFHeader modelHeader)
        {
            int Count = 0;
            var TempMesh = modelHeader.MaterialGroupList;

            for (int i = 0; i < TempMesh.Count; i++)
            {
                for (int a = 0; a < TempMesh[i].WeightRefList.Count; a++)
                {
                    Count += TempMesh[i].WeightRefList[a].MorphMeshGroupList.Count;
                }
            }

            return Count;
        }

        public struct ReassignedMesh
        {
            public string MeshName;
            public int MeshId;
            public bool ShadowModel;
            public int MorphTargetCount;
            public int AltMorphTargetCount;
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
            public List<Vector3> MorphData;
            public List<Vector3> AltMorph;
            public List<Vector3> AltNormals;
        }


    }
}
