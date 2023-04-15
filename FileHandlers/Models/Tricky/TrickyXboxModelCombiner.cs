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
                        //StartReassignMeshCharacter(MeshID);
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

        //public void StartReassignMeshCharacter(int MeshID)
        //{
        //    reassignedMesh = new List<ReassignedMesh>();
        //    materials = new List<TrickyPS2MPF.MaterialData>();
        //    bones = new List<TrickyPS2MPF.BoneData>();
        //    int ListSize = 0;

        //    var TempMaterial = new TrickyPS2MPF.MaterialData();
        //    TempMaterial.MainTexture = "helm";
        //    materials.Add(TempMaterial);

        //    TempMaterial = new TrickyPS2MPF.MaterialData();
        //    TempMaterial.MainTexture = "helm gloss";
        //    materials.Add(TempMaterial);

        //    TempMaterial = new TrickyPS2MPF.MaterialData();
        //    TempMaterial.MainTexture = "helm envr";
        //    materials.Add(TempMaterial);

        //    TempMaterial = new TrickyPS2MPF.MaterialData();
        //    TempMaterial.MainTexture = "boot";
        //    materials.Add(TempMaterial);

        //    TempMaterial = new TrickyPS2MPF.MaterialData();
        //    TempMaterial.MainTexture = "boot gloss";
        //    materials.Add(TempMaterial);

        //    TempMaterial = new TrickyPS2MPF.MaterialData();
        //    TempMaterial.MainTexture = "boot envr";
        //    materials.Add(TempMaterial);

        //    TempMaterial = new TrickyPS2MPF.MaterialData();
        //    TempMaterial.MainTexture = "head";
        //    materials.Add(TempMaterial);

        //    TempMaterial = new TrickyPS2MPF.MaterialData();
        //    TempMaterial.MainTexture = "head gloss";
        //    materials.Add(TempMaterial);

        //    TempMaterial = new TrickyPS2MPF.MaterialData();
        //    TempMaterial.MainTexture = "head envr";
        //    materials.Add(TempMaterial);

        //    TempMaterial = new TrickyPS2MPF.MaterialData();
        //    TempMaterial.MainTexture = "suit";
        //    materials.Add(TempMaterial);

        //    TempMaterial = new TrickyPS2MPF.MaterialData();
        //    TempMaterial.MainTexture = "suit gloss";
        //    materials.Add(TempMaterial);

        //    TempMaterial = new TrickyPS2MPF.MaterialData();
        //    TempMaterial.MainTexture = "suit envr";
        //    materials.Add(TempMaterial);


        //    //Body
        //    for (int i = 0; i < Body.ModelList.Count; i++)
        //    {
        //        if ((MeshID == 0 && Body.ModelList[i].FileName.Contains("3000")) ||
        //            (MeshID == 1 && Body.ModelList[i].FileName.Contains("1500")) ||
        //            (MeshID == 2 && Body.ModelList[i].FileName.Contains("750") && !Body.ModelList[i].FileName.ToLower().Contains("shdw")) ||
        //            (MeshID == 3 && Body.ModelList[i].FileName.ToLower().Contains("shdw")))
        //        {
        //            var TempMesh = new ReassignedMesh();
        //            bones.AddRange(Body.ModelList[i].boneDatas);
        //            TempMesh.faces = ReturnFixedFaces(Body.ModelList[i], bones);
        //            ListSize++;
        //            TempMesh.MeshName = Body.ModelList[i].FileName;
        //            if (MeshID == 3)
        //            {
        //                TempMesh.ShadowModel = true;
        //            }

        //            reassignedMesh.Add(TempMesh);
        //        }
        //    }

        //    //Head
        //    for (int i = 0; i < Head.ModelList.Count; i++)
        //    {
        //        if ((MeshID == 0 && Head.ModelList[i].FileName.Contains("3000")) ||
        //            (MeshID == 1 && Head.ModelList[i].FileName.Contains("1500")) ||
        //            (MeshID == 2 && Head.ModelList[i].FileName.Contains("750") && !Head.ModelList[i].FileName.ToLower().Contains("shdw")) ||
        //            (MeshID == 3 && Head.ModelList[i].FileName.ToLower().Contains("shdw")))
        //        {
        //            var TempMesh = new ReassignedMesh();
        //            bones.AddRange(Head.ModelList[i].boneDatas);
        //            TempMesh.MeshName = Head.ModelList[i].FileName;
        //            TempMesh.faces = ReturnFixedFaces(Head.ModelList[i], bones);
        //            ListSize++;
        //            if (MeshID == 3)
        //            {
        //                TempMesh.ShadowModel = true;
        //            }

        //            TempMesh.MorphTargetCount = Head.ModelList[i].MorphKeyCount;
        //            reassignedMesh.Add(TempMesh);
        //        }
        //    }
        //    FixBoneParents();
        //}
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
