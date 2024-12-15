using SSXMultiTool.FileHandlers.Models.Tricky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models.SSX3
{
    public class SSX3GCModelCombiner
    {
        public SSX3GCMNF? modelHandlers = null;
        public List<SSX3GCMNF.BoneData> boneDatasOrg = new List<SSX3GCMNF.BoneData>();
        public List<SSX3GCMNF.BoneData> boneDatas = new List<SSX3GCMNF.BoneData>();
        public List<SSX3GCMNF.MaterialData> materials = new List<SSX3GCMNF.MaterialData>();

        public List<int> FileIDs = new List<int>();

        public List<ReassignedMesh> reassignedMesh = new List<ReassignedMesh>();


        public bool NormalAverage = false;
        public bool UpdateBones = false;

        public void AddFile(SSX3GCMNF modelHandler)
        {
            modelHandlers = modelHandler;
            boneDatasOrg = new List<SSX3GCMNF.BoneData>();
            for (int i = 0; i < modelHandler.modelHeaders.Count; i++)
            {
                if (!FileIDs.Contains(modelHandler.modelHeaders[i].FileID))
                {
                    boneDatasOrg.AddRange(modelHandler.modelHeaders[i].boneDatas);
                    FileIDs.Add(modelHandler.modelHeaders[i].FileID);
                }
            }
        }

        public void AddBones(SSX3GCMNF modelHandler)
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
            var TempMesh = new ReassignedMesh();
            if (TempModel.ModelName.ToLower().Contains("shdw"))
            {
                TempMesh.ShadowModel = true;
            }

            boneDatas = new List<SSX3GCMNF.BoneData>();
            boneDatas.AddRange(boneDatasOrg);
            //for (int i = 0; i < boneDatasOrg.Count; i++)
            //{
            //    var NewBone = new SSX3GCMNF.BoneData();

            //    NewBone.BoneName = boneDatasOrg[i].BoneName;
            //    NewBone.ParentFileID = boneDatasOrg[i].ParentFileID;
            //    NewBone.ParentBone = boneDatasOrg[i].ParentBone;
            //    NewBone.Unknown1 = boneDatasOrg[i].Unknown1;
            //    NewBone.BoneID = boneDatasOrg[i].BoneID;

            //    NewBone.Unknown2 = boneDatasOrg[i].Unknown2;
            //    NewBone.Unknown3 = boneDatasOrg[i].Unknown3;
            //    NewBone.Unknown4 = boneDatasOrg[i].Unknown4;
            //    NewBone.Unknown5 = boneDatasOrg[i].Unknown5;
            //    NewBone.Unknown6 = boneDatasOrg[i].Unknown6;

            //    NewBone.Position = boneDatasOrg[i].Position;
            //    NewBone.Rotation = boneDatasOrg[i].Rotation;
            //    NewBone.Unknown = boneDatasOrg[i].Unknown;

            //    NewBone.FileID = boneDatasOrg[i].FileID;
            //    NewBone.BonePos = boneDatasOrg[i].BonePos;
            //    boneDatas.Add(NewBone);
            //}

            TempMesh.MorphTargetCount = TempModel.NumMorphs;
            ReshuffleBones();
            TempMesh.faces = ReturnFixedFaces(TempModel, boneDatas);
            TempMesh.MeshName = TempModel.ModelName;
            reassignedMesh = new List<ReassignedMesh>();
            reassignedMesh.Add(TempMesh);
            FixBoneParents();
        }

        public void ReshuffleBones()
        {
            List<SSX3GCMNF.BoneData> TempBoneDatas = new List<SSX3GCMNF.BoneData>();
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
                        }
                    }
                }
            }
        }

        public List<SSX3GCMNF.Face> ReturnFixedFaces(SSX3GCMNF.ModelHeader modelHeader, List<SSX3GCMNF.BoneData> BoneData)
        {
            List<SSX3GCMNF.Face> NewFaces = new List<SSX3GCMNF.Face>();

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

        public SSX3GCMNF.BoneWeightHeader FixBoneIDs(SSX3GCMNF.BoneWeightHeader weightHeader, List<SSX3GCMNF.BoneData> BoneData)
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

        public List<SSX3GCMNF.Face> ReturnFaces(SSX3GCMNF.ModelHeader modelHeader)
        {
            List<SSX3GCMNF.Face> NewFaceList = new List<SSX3GCMNF.Face>();

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
                            return "Missing Bones From Top";
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
            public List<SSX3GCMNF.Face> faces;
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
