using SSXMultiTool.FileHandlers.Models.SSXBlur;
using SSXMultiTool.FileHandlers.Models.Tricky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models.SSXBlur
{
    public class SSXBlurModelCombiner
    {
        public SSXBlurGCMNF? modelHandlers = null;
        public List<SSXBlurGCMNF.BoneData> boneDatasOrg = new List<SSXBlurGCMNF.BoneData>();
        public List<SSXBlurGCMNF.BoneData> boneDatas = new List<SSXBlurGCMNF.BoneData>();
        public List<SSXBlurGCMNF.MaterialData> materials = new List<SSXBlurGCMNF.MaterialData>();

        public List<int> FileIDs = new List<int>();

        public List<ReassignedMesh> reassignedMesh = new List<ReassignedMesh>();


        public bool NormalAverage = false;
        public bool UpdateBones = false;

        public void AddFile(SSXBlurGCMNF modelHandler)
        {
            modelHandlers = modelHandler;
            boneDatasOrg = new List<SSXBlurGCMNF.BoneData>();
            for (int i = 0; i < modelHandler.modelHeaders.Count; i++)
            {
                if (!FileIDs.Contains(modelHandler.modelHeaders[i].FileID))
                {
                    boneDatasOrg.AddRange(modelHandler.modelHeaders[i].boneDatas);
                    FileIDs.Add(modelHandler.modelHeaders[i].FileID);
                }
            }
        }

        public void AddBones(SSXBlurGCMNF modelHandler)
        {
            for (int i = 0; i < modelHandler.modelHeaders.Count; i++)
            {
                if (!FileIDs.Contains(modelHandler.modelHeaders[i].FileID))
                {
                    boneDatasOrg.AddRange(modelHandler.modelHeaders[i].boneDatas);
                    FileIDs.Add(modelHandler.modelHeaders[i].FileID);
                }
            }
        }

        public void MeshReassigned(int MeshID)
        {
            var TempModel = modelHandlers.modelHeaders[MeshID];
            materials = TempModel.materialDatas;
            reassignedMesh = new List<ReassignedMesh>();
            var TempMesh = new ReassignedMesh();
            if (TempModel.ModelName.ToLower().Contains("shdw"))
            {
                TempMesh.ShadowModel = true;
            }

            boneDatas = new List<SSXBlurGCMNF.BoneData>();
            boneDatas.AddRange(boneDatasOrg);

            TempMesh.MorphTargetCount = TempModel.NumMorphs;
            ReshuffleBones();
            TempMesh.faces = ReturnFixedFaces(TempModel, boneDatas);
            TempMesh.MeshName = TempModel.ModelName;
            reassignedMesh.Add(TempMesh);
        }

        public void ReshuffleBones()
        {
            List<SSXBlurGCMNF.BoneData> TempBoneDatas = new List<SSXBlurGCMNF.BoneData>();
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

            for (int i = 0; i < TempBoneDatas.Count; i++)
            {
                if (TempBoneDatas[i].ParentFileID != -1 && TempBoneDatas[i].ParentBone != -1)
                {
                    for (int a = 0; a < TempBoneDatas.Count; a++)
                    {
                        if (TempBoneDatas[i].ParentFileID == TempBoneDatas[a].FileID && TempBoneDatas[i].ParentBone == TempBoneDatas[a].BonePos)
                        {
                            var TempBone = TempBoneDatas[i];
                            TempBone.ParentBone = a;
                            TempBoneDatas[i] = TempBone;
                            break;
                        }
                    }
                }
            }

            boneDatas = TempBoneDatas;
        }

        public List<SSXBlurGCMNF.Face> ReturnFixedFaces(SSXBlurGCMNF.ModelHeader modelHeader, List<SSXBlurGCMNF.BoneData> BoneData)
        {
            List<SSXBlurGCMNF.Face> NewFaces = new List<SSXBlurGCMNF.Face>();

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
                        //if (TempMat.MainTexture != "bord")
                        //{
                        //    if (TempMat.MainTexture == "exta")
                        //    {
                        //        matID = 0;
                        //    }
                        //    else if (TempMat.MainTexture == "legs")
                        //    {
                        //        matID = 1;
                        //    }
                        //    else if (TempMat.MainTexture == "head")
                        //    {
                        //        matID = 2;
                        //    }
                        //    else if (TempMat.MainTexture == "suit")
                        //    {
                        //        matID = 3;
                        //    }

                        //    if (TempMat.Texture3.EndsWith("_g"))
                        //    {
                        //        focusID = 1;
                        //    }
                        //    else if (TempMat.Texture4 == "envr")
                        //    {
                        //        focusID = 2;
                        //    }
                        //}

                        //matID = (3 * matID) + focusID;
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

        public SSXBlurGCMNF.BoneWeightHeader FixBoneIDs(SSXBlurGCMNF.BoneWeightHeader weightHeader, List<SSXBlurGCMNF.BoneData> BoneData)
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

        public List<SSXBlurGCMNF.Face> ReturnFaces(SSXBlurGCMNF.ModelHeader modelHeader)
        {
            List<SSXBlurGCMNF.Face> NewFaceList = new List<SSXBlurGCMNF.Face>();

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

        public string CheckBones(int ModelID)
        {
            if (ModelID == -1)
            {
                ModelID = 0;
            }

            var TempModelHandler = modelHandlers.modelHeaders[ModelID];

            for (int i = 0; i < TempModelHandler.boneWeightHeaders.Count; i++)
            {
                var TempWeightHeader = TempModelHandler.boneWeightHeaders[i];

                for (int a = 0; a < TempWeightHeader.boneWeights.Count; a++)
                {
                    var TempWeightList = TempWeightHeader.boneWeights[a];
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
                        if (TempWeightList.FileID == 1)
                        {
                            return "Missing Bones From Board Bindings";
                        }
                        if (TempWeightList.FileID == 6)
                        {
                            return "Missing Bones From Eyes";
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
                            return "Missing Bones From Top";
                        }
                        else
                        if (boneDatasOrg[i].ParentFileID == 1)
                        {
                            return "Missing Bones From Board Bindings";
                        }
                        if (boneDatasOrg[i].ParentFileID == 6)
                        {
                            return "Missing Bones From Eyes";
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

        public struct ReassignedMesh
        {
            public string MeshName;
            public int MeshId;
            public bool ShadowModel;
            public int MorphTargetCount;
            //public List<Vector3> IKPoints;
            public List<SSXBlurGCMNF.Face> faces;
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
