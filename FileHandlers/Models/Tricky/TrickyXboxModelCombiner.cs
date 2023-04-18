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

        public int TristripCount(TrickyXboxMXF.ModelHeader modelHeader)
        {
            int Count = 0;

            var TempMesh = modelHeader.tristripHeaders;

            for (int i = 0; i < TempMesh.Count; i++)
            {
                Count += TempMesh[i].IndexList.Count;
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

        public void StartRegenMesh(TrickyXboxModelCombiner trickyModelCombiner, int Selected)
        {
            if (Board != null && !BodyBool)
            {
                StartRegenMeshBoard(trickyModelCombiner, Selected);
            }
            else if (Head != null && Body != null)
            {
                //StartRegenMeshCharacter(trickyModelCombiner, Selected);
            }
            else
            {
                MessageBox.Show("Error Missing Files");
            }

        }

        public void StartRegenMeshBoard(TrickyXboxModelCombiner trickyModelCombiner, int Selected)
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
            TempTrickyMesh.materialDatas = new List<TrickyXboxMXF.MaterialData>();
            for (int a = 0; a < ReassignedMesh.faces.Count; a++)
            {
                if (!MaterialsID.Contains(ReassignedMesh.faces[a].MaterialID))
                {
                    MaterialsID.Add(ReassignedMesh.faces[a].MaterialID);
                }
            }

            for (int a = 0; a < MaterialsID.Count; a++)
            {
                TrickyXboxMXF.MaterialData MaterialData = new TrickyXboxMXF.MaterialData();
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

            //Redo Data In Correct Formats IE make Weight List and make faces use the positions.
            TempTrickyMesh.boneWeightHeaders = new List<TrickyXboxMXF.BoneWeightHeader>();

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
                    else
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
                }
            #endregion

            Test = false;
                TempID = 0;
                TestID = 0;
                //#region Point 2
                //while (!Test)
                //{
                //    TempID = ContainsVertice(TempReMesh.faces[a].V2, VectorPoint, TestID);
                //    if (TempID == -1)
                //    {
                //        TempFace.Id2 = VectorPoint.Count;

                //        VectorPoint.Add(GenerateVectorPoint(TempReMesh.faces[a], 2));

                //        Test = true;
                //    }
                //    else if (TempReMesh.faces[a].Weight2Pos != VectorPoint[TempID].Weight)
                //    {
                //        TestID++;
                //    }
                //    else if (!Shadow)
                //    {
                //        TestID++;
                //        if (TempReMesh.faces[a].MaterialID != VectorPoint[TempID].Material)
                //        {

                //        }
                //        else if (!NormalsEqual(TempReMesh.faces[a].Normal2, VectorPoint[TempID].normal) && !NormalAverage)
                //        {

                //        }
                //        else if (!UVEqual(TempReMesh.faces[a].UV2, VectorPoint[TempID].TextureCord))
                //        {

                //        }
                //        else
                //        {
                //            if (TempReMesh.MorphTargetCount != 0)
                //            {
                //                if (!MorphPointsEqual(TempReMesh.faces[a].MorphPoint2, VectorPoint[TempID].MorphData))
                //                {

                //                }
                //                else
                //                {
                //                    TempFace.Id2 = TempID;
                //                    Test = true;
                //                }
                //            }
                //            else
                //            {
                //                TempFace.Id2 = TempID;
                //                Test = true;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        TempFace.Id2 = TempID;
                //        Test = true;
                //    }
                //}
                //#endregion

                Test = false;
                TempID = 0;
                TestID = 0;
                //#region Point 3
                //while (!Test)
                //{
                //    TempID = ContainsVertice(TempReMesh.faces[a].V3, VectorPoint, TestID);
                //    if (TempID == -1)
                //    {
                //        TempFace.Id3 = VectorPoint.Count;

                //        VectorPoint.Add(GenerateVectorPoint(TempReMesh.faces[a], 3));

                //        Test = true;
                //    }
                //    else if (TempReMesh.faces[a].Weight3Pos != VectorPoint[TempID].Weight)
                //    {
                //        TestID++;
                //    }
                //    else if (!Shadow)
                //    {
                //        TestID++;
                //        if (TempReMesh.faces[a].MaterialID != VectorPoint[TempID].Material)
                //        {

                //        }
                //        else if (!NormalsEqual(TempReMesh.faces[a].Normal3, VectorPoint[TempID].normal) && !NormalAverage)
                //        {

                //        }
                //        else if (!UVEqual(TempReMesh.faces[a].UV3, VectorPoint[TempID].TextureCord))
                //        {

                //        }
                //        else
                //        {
                //            if (TempReMesh.MorphTargetCount != 0)
                //            {
                //                if (!MorphPointsEqual(TempReMesh.faces[a].MorphPoint3, VectorPoint[TempID].MorphData))
                //                {

                //                }
                //                else
                //                {
                //                    TempFace.Id3 = TempID;
                //                    Test = true;
                //                }
                //            }
                //            else
                //            {
                //                TempFace.Id3 = TempID;
                //                Test = true;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        TempFace.Id3 = TempID;
                //        Test = true;
                //    }
                //}
                //#endregion

                indiceFaces.Add(TempFace);
            }

            //Once in Vertex List Send Morph Data To MorphList

            indiceFaces = TristripGenerator.NeighbourPriority(indiceFaces);

            //Send to Tristrip Generator
            List<TristripGenerator.IndiceTristrip> indiceTristrips = TristripGenerator.GenerateTristripNivda(indiceFaces);
            if (indiceTristrips == null)
            {
                MessageBox.Show("Tristrip Failed to Generate");
                return;
            }

            //Correct to one gaint tristrip for each material

            //set data to mxf
        }

        static VectorPoint GenerateVectorPoint(TrickyXboxMXF.Face face, int Vertice)
        {
            VectorPoint vectorPoint = new VectorPoint();
            vectorPoint.Material = face.MaterialID;
            vectorPoint.Normals = new List<Vector3>();
            if (Vertice == 1)
            {
                vectorPoint.vector = face.V1;
                vectorPoint.normal = face.Normal1;
                vectorPoint.TextureCord = face.UV1;
                vectorPoint.Weight = face.Weight1Pos;
                vectorPoint.MorphData = face.MorphPoint1;
                vectorPoint.tangent = face.TangentNormal1;
                vectorPoint.Normals.Add(face.Normal1);
            }

            if (Vertice == 2)
            {
                vectorPoint.vector = face.V2;
                vectorPoint.normal = face.Normal2;
                vectorPoint.TextureCord = face.UV2;
                vectorPoint.Weight = face.Weight2Pos;
                vectorPoint.MorphData = face.MorphPoint2;
                vectorPoint.tangent = face.TangentNormal1;
                vectorPoint.Normals.Add(face.Normal2);
            }

            if (Vertice == 3)
            {
                vectorPoint.vector = face.V3;
                vectorPoint.normal = face.Normal3;
                vectorPoint.TextureCord = face.UV3;
                vectorPoint.Weight = face.Weight3Pos;
                vectorPoint.MorphData = face.MorphPoint3;
                vectorPoint.tangent = face.TangentNormal1;
                vectorPoint.Normals.Add(face.Normal3);
            }

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
            if ((int)(normal1.X * 32768f) == (int)(normal2.X * 32768f) && (int)(normal1.Y * 32768f) == (int)(normal2.Y * 32768f) && (int)(normal1.Z * 32768f) == (int)(normal2.Z * 32768f))
            {
                return true;
            }
            return false;
        }

        static bool UVEqual(Vector2 Uv1, Vector2 Uv2)
        {
            if ((int)(Uv1.X * 4096f) == (int)(Uv2.X * 4096f) && (int)(Uv1.Y * 4096f) == (int)(Uv2.Y * 4096f))
            {
                return true;
            }
            return false;
        }

        static int ContainsWeight(TrickyPS2MPF.BoneWeightHeader boneWeight, List<TrickyPS2MPF.BoneWeightHeader> boneWeightList)
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

        static int ContainsWeight(TrickyXboxMXF.BoneWeightHeader boneWeight, List<TrickyXboxMXF.BoneWeightHeader> boneWeightList)
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
            public List<TrickyXboxMXF.Face> faces;
        }

        public struct VectorPoint
        {
            public bool Tristripped;
            public int Material;
            public Vector3 vector;
            public Vector3 normal;
            public Vector2 TextureCord;
            public Vector3 tangent;
            public int Weight;
            public List<Vector3> Normals;
            public List<Vector3> MorphData;
        }
    }
}
