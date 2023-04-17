using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models.Tricky
{
    public class TrickyXboxModelCombiner
    {
        public TrickyXboxMXF? Body;
        public TrickyXboxMXF? Head;
        public TrickyXboxMXF? Board;
        public List<TrickyXboxMXF.MaterialData> materials = new List<TrickyXboxMXF.MaterialData>();
        public List<TrickyXboxMXF.BoneData> bones = new List<TrickyXboxMXF.BoneData>();

        public List<ReassignedMesh> reassignedMesh = new List<ReassignedMesh>();

        public bool BodyBool;

        public bool BoneUpdate;
        public int TristripMode;
        public bool NormalAverage;

        public int DectectModelType(TrickyXboxMXF modelHandler)
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
                    if (Board != null)
                    {
                        StartReassignMeshBoard(MeshID);
                    }
                }
            }
        }

        public void StartReassignMeshBoard(int MeshID)
        {
            reassignedMesh = new List<ReassignedMesh>();
            var TempMesh = new ReassignedMesh();
            TempMesh.faces = new List<TrickyXboxMXF.Face>();
            materials = new List<TrickyXboxMXF.MaterialData>();
            bones = new List<TrickyXboxMXF.BoneData>();


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

        public List<TrickyXboxMXF.Face> ReturnFaces(TrickyXboxMXF.ModelHeader modelHeader)
        {
            List<TrickyXboxMXF.Face> NewFaceList = new List<TrickyXboxMXF.Face>(); 

            return NewFaceList;
        }

        public void StartReassignMeshCharacter(int MeshID)
        {
            reassignedMesh = new List<ReassignedMesh>();
            materials = new List<TrickyXboxMXF.MaterialData>();
            bones = new List<TrickyXboxMXF.BoneData>();

            var TempMaterial = new TrickyXboxMXF.MaterialData();
            TempMaterial.MainTexture = "helm";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyXboxMXF.MaterialData();
            TempMaterial.MainTexture = "helm gloss";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyXboxMXF.MaterialData();
            TempMaterial.MainTexture = "helm envr";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyXboxMXF.MaterialData();
            TempMaterial.MainTexture = "boot";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyXboxMXF.MaterialData();
            TempMaterial.MainTexture = "boot gloss";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyXboxMXF.MaterialData();
            TempMaterial.MainTexture = "boot envr";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyXboxMXF.MaterialData();
            TempMaterial.MainTexture = "head";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyXboxMXF.MaterialData();
            TempMaterial.MainTexture = "head gloss";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyXboxMXF.MaterialData();
            TempMaterial.MainTexture = "head envr";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyXboxMXF.MaterialData();
            TempMaterial.MainTexture = "suit";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyXboxMXF.MaterialData();
            TempMaterial.MainTexture = "suit gloss";
            materials.Add(TempMaterial);

            TempMaterial = new TrickyXboxMXF.MaterialData();
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

        public List<TrickyXboxMXF.Face> ReturnFixedFaces(TrickyXboxMXF.ModelHeader modelHeader, List<TrickyXboxMXF.BoneData> BoneData)
        {
            List<TrickyXboxMXF.Face> NewFaces = new List<TrickyXboxMXF.Face>();

            for (int i = 0; i < modelHeader.boneWeightHeaders.Count; i++)
            {
                modelHeader.boneWeightHeaders[i] = FixBoneIDs(modelHeader.boneWeightHeaders[i], BoneData);
            }

            for (int a = 0; a < modelHeader.tristripHeaders.Count; a++)
            {
                var Data = modelHeader.tristripHeaders[a];
                for (int b = 0; b < Data.faces.Count; b++)
                {
                    var Face = Data.faces[b];

                    Face.Weight1 = modelHeader.boneWeightHeaders[Face.Weight1Pos];
                    Face.Weight2 = modelHeader.boneWeightHeaders[Face.Weight2Pos];
                    Face.Weight3 = modelHeader.boneWeightHeaders[Face.Weight3Pos];

                    var TempMat = modelHeader.materialDatas[modelHeader.tristripHeaders[a].MaterialIndex0];
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

                    Data.faces[b] = Face;
                }
                modelHeader.tristripHeaders[a] = Data;
            }

            return NewFaces;
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

        public TrickyXboxMXF.BoneWeightHeader FixBoneIDs(TrickyXboxMXF.BoneWeightHeader weightHeader, List<TrickyXboxMXF.BoneData> BoneData)
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
    }

    public struct ReassignedMesh
    {
        public string MeshName;
        public int MeshId;
        public bool BodyHead;
        public bool ShadowModel;
        public int MorphTargetCount;
        public List<Vector3> IKPoints;
        public List<TrickyXboxMXF.Face> faces;
    }
}
