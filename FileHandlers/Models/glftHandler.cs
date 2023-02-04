using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Schema2;
using SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2;
using SSXMultiTool.FileHandlers.Models;

//https://github.com/vpenades/SharpGLTF/blob/master/src/SharpGLTF.Toolkit/Geometry/readme.md
namespace SSXMultiTool.FileHandlers
{

    public class glftHandler
    {
        public static void SaveOGglTF(string Output, SSXMPFModelHandler.MPFModelHeader modelHeader)
        {
            var mesh = new MeshBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4>(modelHeader.FileName);
            //Make Materials

            List<MaterialBuilder> materialBuilders = new List<MaterialBuilder>();

            for (int i = 0; i < modelHeader.materialDataList.Count; i++)
            {
                var TempVar = modelHeader.materialDataList[i];
                var material1 = new MaterialBuilder(TempVar.Name)
                .WithChannelParam(KnownChannel.BaseColor, KnownProperty.RGBA, new Vector4(0, 0, 0, 1));
                materialBuilders.Add(material1);
            }

            var bindings = new List<SharpGLTF.Scenes.NodeBuilder>();
            SharpGLTF.Scenes.NodeBuilder Binding = new SharpGLTF.Scenes.NodeBuilder();
            for (int i = 0; i < modelHeader.bone.Count; i++)
            {
                if (modelHeader.bone[i].BoneParentID == -1)
                {
                    Binding = new SharpGLTF.Scenes.NodeBuilder();
                }
                else
                {
                    Binding = bindings[modelHeader.bone[i].BoneParentID];
                }
                Binding = Binding.CreateNode(modelHeader.bone[i].boneName);
                float tempX = modelHeader.bone[i].Radians.X;
                float tempY = modelHeader.bone[i].Radians.Y;
                float tempZ = modelHeader.bone[i].Radians.Z;

                Binding.WithLocalRotation(ToQuaternion(new Vector3(tempX, tempY, tempZ)));

                Binding.WithLocalTranslation(modelHeader.bone[i].Position);

                Binding.LocalMatrix = Binding.LocalMatrix;
                bindings.Add(Binding);
            }


            for (int i = 0; i < modelHeader.MaterialGroups.Count; i++)
            {
                for (int a = 0; a < modelHeader.MaterialGroups[i].staticMesh.Count; a++)
                {
                    var Data = modelHeader.MaterialGroups[i].staticMesh[a];
                    int MatId = modelHeader.MaterialGroups[i].MaterialID;

                    for (int b = 0; b < Data.faces.Count; b++)
                    {
                        var Face = Data.faces[b];
                        VertexPositionNormal TempPos1 = new VertexPositionNormal();
                        TempPos1.Position = Face.V1;
                        TempPos1.Normal = Face.Normal1;

                        VertexPositionNormal TempPos2 = new VertexPositionNormal();
                        TempPos2.Position = Face.V2;
                        TempPos2.Normal = Face.Normal2;

                        VertexPositionNormal TempPos3 = new VertexPositionNormal();
                        TempPos3.Position = Face.V3;
                        TempPos3.Normal = Face.Normal3;

                        VertexTexture1 TempTexture1 = new VertexTexture1();
                        TempTexture1.TexCoord = Face.UV1;

                        VertexTexture1 TempTexture2 = new VertexTexture1();
                        TempTexture2.TexCoord = Face.UV2;

                        VertexTexture1 TempTexture3 = new VertexTexture1();
                        TempTexture3.TexCoord = Face.UV3;

                        (int Temp, float TempFloat)[] bindings1 = new (int Temp, float TempFloat)[1];
                        VertexJoints4 TempBinding1 = new VertexJoints4();
                        bindings1 = new (int Temp, float TempFloat)[1];
                        for (int ia = 0; ia < 1; ia++)
                        {
                            bindings1[ia] = (0, 1);
                        }
                        TempBinding1.SetBindings(bindings1);

                        VertexJoints4 TempBinding2 = new VertexJoints4();
                        bindings1 = new (int Temp, float TempFloat)[1];
                        for (int ia = 0; ia < 1; ia++)
                        {
                            bindings1[ia] = (0, 1);
                        }
                        TempBinding2.SetBindings(bindings1);

                        VertexJoints4 TempBinding3 = new VertexJoints4();
                        bindings1 = new (int Temp, float TempFloat)[1];
                        for (int ia = 0; ia < 1; ia++)
                        {
                            bindings1[ia] = (0, 1);
                        }
                        TempBinding3.SetBindings(bindings1);

                        mesh.UsePrimitive(materialBuilders[MatId]).AddTriangle((TempPos1, TempTexture1, TempBinding1), (TempPos2, TempTexture2, TempBinding2), (TempPos3, TempTexture3, TempBinding3));
                    }
                }
            }

            var scene = new SharpGLTF.Scenes.SceneBuilder();

            scene.AddSkinnedMesh(mesh, Matrix4x4.Identity, bindings.ToArray());

            if (modelHeader.IKPoint != null)
            {
                for (int i = 0; i < modelHeader.IKPoint.Count; i++)
                {
                    var Temp = new SharpGLTF.Scenes.NodeBuilder("IKPoint " + i.ToString());
                    Temp.WithLocalTranslation(modelHeader.IKPoint[i]);
                    scene.AddNode(Temp);
                }
            }

            // save the model in different formats

            var model = scene.ToGltf2();
            model.SaveGLB(Output);
        }

        public static void SaveTrickyglTF(string Output, TrickyModelCombiner Handler)
        {
            var scene = new SharpGLTF.Scenes.SceneBuilder();

            List<MaterialBuilder> materialBuilders = new List<MaterialBuilder>();
            for (int i = 0; i < Handler.materials.Count; i++)
            {
                var TempVar = Handler.materials[i];
                var material1 = new MaterialBuilder(TempVar.MainTexture)
                .WithChannelParam(KnownChannel.BaseColor, KnownProperty.RGBA, new Vector4(1,1,1,1));
                materialBuilders.Add(material1);
            }
            var bindings = new List<SharpGLTF.Scenes.NodeBuilder>();
            SharpGLTF.Scenes.NodeBuilder Binding = new SharpGLTF.Scenes.NodeBuilder();
            for (int i = 0; i < Handler.bones.Count; i++)
            {
                if (Handler.bones[i].ParentBone == -1)
                {
                    Binding = new SharpGLTF.Scenes.NodeBuilder();
                }
                else
                {
                    Binding = bindings[Handler.bones[i].ParentBone];
                }
                Binding = Binding.CreateNode(Handler.bones[i].BoneName);
                float tempX = Handler.bones[i].Radians.X;
                float tempY = Handler.bones[i].Radians.Y;
                float tempZ = Handler.bones[i].Radians.Z;

                Binding.WithLocalRotation(ToQuaternion(new Vector3(-tempX, -tempY, -tempZ)));
                Binding.WithLocalTranslation(Handler.bones[i].Position);

                Binding.LocalMatrix = Binding.LocalMatrix;
                bindings.Add(Binding);
            }


            for (int a = 0; a < Handler.reassignedMesh.Count; a++)
            {
                if (!Handler.reassignedMesh[a].ShadowModel)
                {
                    List<PointMorph> pointMorphs = new List<PointMorph>();
                    var mesh = new MeshBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4>(Handler.reassignedMesh[a].MeshName);
                    for (int b = 0; b < Handler.reassignedMesh[a].faces.Count; b++)
                    {
                        var Face = Handler.reassignedMesh[a].faces[b];
                        VertexPositionNormal TempPos1 = new VertexPositionNormal();
                        TempPos1.Position = Face.V1;
                        TempPos1.Normal = Face.Normal1;

                        VertexPositionNormal TempPos2 = new VertexPositionNormal();
                        TempPos2.Position = Face.V2;
                        TempPos2.Normal = Face.Normal2;

                        VertexPositionNormal TempPos3 = new VertexPositionNormal();
                        TempPos3.Position = Face.V3;
                        TempPos3.Normal = Face.Normal3;

                        VertexTexture1 TempTexture1 = new VertexTexture1();
                        TempTexture1.TexCoord.X = (float)Face.UV1.X;
                        TempTexture1.TexCoord.Y = (float)Face.UV1.Y;

                        VertexTexture1 TempTexture2 = new VertexTexture1();
                        TempTexture2.TexCoord.X = (float)Face.UV2.X;
                        TempTexture2.TexCoord.Y = (float)Face.UV2.Y;

                        VertexTexture1 TempTexture3 = new VertexTexture1();
                        TempTexture3.TexCoord.X = (float)Face.UV3.X;
                        TempTexture3.TexCoord.Y = (float)Face.UV3.Y;

                        (int Temp, float TempFloat)[] bindings1 = new (int Temp, float TempFloat)[1];

                        VertexJoints4 TempBinding1 = new VertexJoints4();
                        bindings1 = new (int Temp, float TempFloat)[Face.Weight1.boneWeights.Count];
                        for (int ia = 0; ia < Face.Weight1.boneWeights.Count; ia++)
                        {
                            bindings1[ia] = (Face.Weight1.boneWeights[ia].BoneID, Face.Weight1.boneWeights[ia].Weight);
                        }
                        TempBinding1.SetBindings(bindings1);

                        VertexJoints4 TempBinding2 = new VertexJoints4();
                        bindings1 = new (int Temp, float TempFloat)[Face.Weight2.boneWeights.Count];
                        for (int ia = 0; ia < Face.Weight2.boneWeights.Count; ia++)
                        {
                            bindings1[ia] = (Face.Weight2.boneWeights[ia].BoneID, Face.Weight2.boneWeights[ia].Weight);
                        }
                        TempBinding2.SetBindings(bindings1);

                        VertexJoints4 TempBinding3 = new VertexJoints4();
                        bindings1 = new (int Temp, float TempFloat)[Face.Weight3.boneWeights.Count];
                        for (int ia = 0; ia < Face.Weight3.boneWeights.Count; ia++)
                        {
                            bindings1[ia] = (Face.Weight3.boneWeights[ia].BoneID, Face.Weight3.boneWeights[ia].Weight);
                        }
                        TempBinding3.SetBindings(bindings1);

                        mesh.UsePrimitive(materialBuilders[Face.MaterialID]).AddTriangle((TempPos1, TempTexture1, TempBinding1), (TempPos2, TempTexture2, TempBinding2), (TempPos3, TempTexture3, TempBinding3));

                        if(Handler.reassignedMesh[a].MorphTargetCount!=0)
                        {
                            if(!pointMorphs.Contains(GeneratePointMorph(TempPos1.Position, Face.MorphPoint1)))
                            {
                                pointMorphs.Add(GeneratePointMorph(TempPos1.Position, Face.MorphPoint1));
                            }
                            if (!pointMorphs.Contains(GeneratePointMorph(TempPos2.Position, Face.MorphPoint2)))
                            {
                                pointMorphs.Add(GeneratePointMorph(TempPos2.Position, Face.MorphPoint2));
                            }
                            if (!pointMorphs.Contains(GeneratePointMorph(TempPos3.Position, Face.MorphPoint3)))
                            {
                                pointMorphs.Add(GeneratePointMorph(TempPos3.Position, Face.MorphPoint3));
                            }
                        }
                    }

                    for (int c = 0; c < Handler.reassignedMesh[a].MorphTargetCount; c++)
                    {
                        var morphTargetBuilder = mesh.UseMorphTarget(c);
                        foreach (var vertexPosition in morphTargetBuilder.Vertices)
                        {
                            for (int i = 0; i < pointMorphs.Count; i++)
                            {
                                if (pointMorphs[i].Point== vertexPosition.Position)
                                {
                                    var NewVertexPosition = vertexPosition;
                                    NewVertexPosition.Position += pointMorphs[i].MorphPoints[c]; 
                                    morphTargetBuilder.SetVertex(vertexPosition, NewVertexPosition);
                                }
                            }
                        }
                    }
                    
                    scene.AddSkinnedMesh(mesh, Matrix4x4.CreateTranslation(0, 0, 0), bindings.ToArray());
                }
                else
                {
                    var mesh = new MeshBuilder<VertexPosition, VertexEmpty, VertexJoints4>(Handler.reassignedMesh[a].MeshName);
                    for (int b = 0; b < Handler.reassignedMesh[a].faces.Count; b++)
                    {
                        var Face = Handler.reassignedMesh[a].faces[b];
                        VertexPosition TempPos1 = new VertexPosition();
                        TempPos1.Position = Face.V1;

                        VertexPosition TempPos2 = new VertexPosition();
                        TempPos2.Position = Face.V2;

                        VertexPosition TempPos3 = new VertexPosition();
                        TempPos3.Position = Face.V3;

                        (int Temp, float TempFloat)[] bindings1 = new (int Temp, float TempFloat)[1];

                        VertexJoints4 TempBinding1 = new VertexJoints4();
                        bindings1 = new (int Temp, float TempFloat)[Face.Weight1.boneWeights.Count];
                        for (int ia = 0; ia < Face.Weight1.boneWeights.Count; ia++)
                        {
                            bindings1[ia] = (Face.Weight1.boneWeights[ia].BoneID, Face.Weight1.boneWeights[ia].Weight);
                        }
                        TempBinding1.SetBindings(bindings1);

                        VertexJoints4 TempBinding2 = new VertexJoints4();
                        bindings1 = new (int Temp, float TempFloat)[Face.Weight2.boneWeights.Count];
                        for (int ia = 0; ia < Face.Weight2.boneWeights.Count; ia++)
                        {
                            bindings1[ia] = (Face.Weight2.boneWeights[ia].BoneID, Face.Weight2.boneWeights[ia].Weight);
                        }
                        TempBinding2.SetBindings(bindings1);

                        VertexJoints4 TempBinding3 = new VertexJoints4();
                        bindings1 = new (int Temp, float TempFloat)[Face.Weight3.boneWeights.Count];
                        for (int ia = 0; ia < Face.Weight3.boneWeights.Count; ia++)
                        {
                            bindings1[ia] = (Face.Weight3.boneWeights[ia].BoneID, Face.Weight3.boneWeights[ia].Weight);
                        }
                        TempBinding3.SetBindings(bindings1);

                        mesh.UsePrimitive(materialBuilders[Face.MaterialID]).AddTriangle((TempPos1, TempBinding1), (TempPos2, TempBinding2), (TempPos3, TempBinding3));
                    }
                    scene.AddSkinnedMesh(mesh, Matrix4x4.CreateTranslation(0, 0, 0), bindings.ToArray());
                }
                if (Handler.reassignedMesh[a].IKPoints != null)
                {
                    for (int i = 0; i < Handler.reassignedMesh[a].IKPoints.Count; i++)
                    {
                        var Temp = new SharpGLTF.Scenes.NodeBuilder("IKPoint " + i.ToString());
                        Temp.WithLocalTranslation(Handler.reassignedMesh[a].IKPoints[i]);
                        //Temp.Name = "IKPoint " + i.ToString();
                        scene.AddNode(Temp);
                    }
                }
            }

            // save the model in different formats
            var model = scene.ToGltf2();
            model.SaveGLB(Output);
        }

        public static void SaveSSX3Glft(string Output, SSX3ModelCombiner Handler)
        {
            var scene = new SharpGLTF.Scenes.SceneBuilder();

            List<MaterialBuilder> materialBuilders = new List<MaterialBuilder>();
            for (int i = 0; i < Handler.materials.Count; i++)
            {
                var TempVar = Handler.materials[i];
                var material1 = new MaterialBuilder(TempVar.MainTexture)
                .WithChannelParam(KnownChannel.BaseColor, KnownProperty.RGBA, new Vector4(1, 1, 1, 1));
                materialBuilders.Add(material1);
            }
            var bindings = new List<SharpGLTF.Scenes.NodeBuilder>();
            SharpGLTF.Scenes.NodeBuilder Binding = new SharpGLTF.Scenes.NodeBuilder();
            for (int i = 0; i < Handler.boneDatas.Count; i++)
            {
                if (Handler.boneDatas[i].ParentBone == -1)
                {
                    Binding = new SharpGLTF.Scenes.NodeBuilder();
                }
                else
                {
                    Binding = bindings[Handler.boneDatas[i].ParentBone];
                }
                Binding = Binding.CreateNode(Handler.boneDatas[i].BoneName);

                Binding.WithLocalRotation(Handler.boneDatas[i].Rotation);
                Binding.WithLocalTranslation(new Vector3(Handler.boneDatas[i].Position.X, Handler.boneDatas[i].Position.Y, Handler.boneDatas[i].Position.Z));

                Binding.LocalMatrix = Binding.LocalMatrix;
                bindings.Add(Binding);
            }

            var mesh = new MeshBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4>(Handler.reassignedMesh.MeshName);

            if (true/*!Handler.reassignedMesh[a].ShadowModel*/)
            {
                for (int b = 0; b < Handler.reassignedMesh.faces.Count; b++)
                {
                    var Face = Handler.reassignedMesh.faces[b];
                    VertexPositionNormal TempPos1 = new VertexPositionNormal();
                    TempPos1.Position = Face.V1;
                    TempPos1.Normal = Face.Normal1;

                    VertexPositionNormal TempPos2 = new VertexPositionNormal();
                    TempPos2.Position = Face.V2;
                    TempPos2.Normal = Face.Normal2;

                    VertexPositionNormal TempPos3 = new VertexPositionNormal();
                    TempPos3.Position = Face.V3;
                    TempPos3.Normal = Face.Normal3;

                    VertexTexture1 TempTexture1 = new VertexTexture1();
                    TempTexture1.TexCoord.X = (float)Face.UV1.X;
                    TempTexture1.TexCoord.Y = (float)Face.UV1.Y;

                    VertexTexture1 TempTexture2 = new VertexTexture1();
                    TempTexture2.TexCoord.X = (float)Face.UV2.X;
                    TempTexture2.TexCoord.Y = (float)Face.UV2.Y;

                    VertexTexture1 TempTexture3 = new VertexTexture1();
                    TempTexture3.TexCoord.X = (float)Face.UV3.X;
                    TempTexture3.TexCoord.Y = (float)Face.UV3.Y;

                    (int Temp, float TempFloat)[] bindings1 = new (int Temp, float TempFloat)[1];

                    VertexJoints4 TempBinding1 = new VertexJoints4();
                    bindings1 = new (int Temp, float TempFloat)[Face.Weight1.BoneWeightList.Count];
                    for (int ia = 0; ia < Face.Weight1.BoneWeightList.Count; ia++)
                    {
                        bindings1[ia] = (Face.Weight1.BoneWeightList[ia].BoneID, Face.Weight1.BoneWeightList[ia].Weight);
                    }
                    TempBinding1.SetBindings(bindings1);

                    VertexJoints4 TempBinding2 = new VertexJoints4();
                    bindings1 = new (int Temp, float TempFloat)[Face.Weight2.BoneWeightList.Count];
                    for (int ia = 0; ia < Face.Weight2.BoneWeightList.Count; ia++)
                    {
                        bindings1[ia] = (Face.Weight2.BoneWeightList[ia].BoneID, Face.Weight2.BoneWeightList[ia].Weight);
                    }
                    TempBinding2.SetBindings(bindings1);

                    VertexJoints4 TempBinding3 = new VertexJoints4();
                    bindings1 = new (int Temp, float TempFloat)[Face.Weight3.BoneWeightList.Count];
                    for (int ia = 0; ia < Face.Weight3.BoneWeightList.Count; ia++)
                    {
                        bindings1[ia] = (Face.Weight3.BoneWeightList[ia].BoneID, Face.Weight3.BoneWeightList[ia].Weight);
                    }
                    TempBinding3.SetBindings(bindings1);

                    mesh.UsePrimitive(materialBuilders[Handler.reassignedMesh.faces[b].MaterialID]).AddTriangle((TempPos1, TempTexture1, TempBinding1), (TempPos2, TempTexture2, TempBinding2), (TempPos3, TempTexture3, TempBinding3));
                }

                //scene.AddSkinnedMesh(mesh, Matrix4x4.CreateTranslation(0, 0, 0), bindings.ToArray());
            }
            scene.AddSkinnedMesh(mesh, Matrix4x4.CreateTranslation(0, 0, 0), bindings.ToArray());
            // save the model in different formats
            var model = scene.ToGltf2();
            model.SaveGLB(Output);
        }

        public static TrickyModelCombiner LoadGlft(string Path)
        {
            TrickyModelCombiner trickyModelCombiner = new TrickyModelCombiner();
            trickyModelCombiner.materials = new List<TrickyMPFModelHandler.MaterialData>();
            trickyModelCombiner.reassignedMesh = new List<TrickyModelCombiner.ReassignedMesh>();
            var Scene = SharpGLTF.Scenes.SceneBuilder.LoadDefaultScene(Path);
            var Instances = Scene.Instances.ToArray();

            //Read All Instances Looking for IK Point
            List<Vector3> IKPoints = new List<Vector3>();
            int IkCount = 0;
            for (int i = 0; i < Instances.Length; i++)
            {
                if (Instances[i].Name != null)
                {
                    if (Instances[i].Name.ToLower().Contains("ikpoint"))
                    {
                        IkCount += 1;
                    }
                }
            }

            for (int a = 0; a < IkCount; a++)
            {
                for (int i = 0; i < Instances.Length; i++)
                {
                    if (Instances[i].Name != null)
                    {
                        if (Instances[i].Name.ToLower() == ("ikpoint " + a))
                        {
                            IKPoints.Add(Instances[i].Content.GetPoseWorldMatrix().Translation);
                            break;
                        }
                    }
                }
            }

            //Read Bones Building List
            List<TrickyMPFModelHandler.BoneData> boneDatas = new List<TrickyMPFModelHandler.BoneData>();
            var Ampatures = Scene.FindArmatures();

            for (int a = 0; a < Ampatures[0].VisualChildren.Count; a++)
            {
                if (Ampatures[0].VisualChildren[a].Name.ToLower() == "hips")
                {
                    var StartBone = Ampatures[0].VisualChildren[a];
                    boneDatas = ReturnBoneAndChildren(StartBone, true);
                }
                else if (Ampatures[0].VisualChildren[a].Name.ToLower() == "board")
                {
                    var StartBone = Ampatures[0].VisualChildren[a];
                    boneDatas = ReturnBoneAndChildren(StartBone, true);
                }
            }

            trickyModelCombiner.bones = boneDatas;

            for (int i = 0; i < Instances.Length; i++)
            {
                if (Instances[i].Content.GetType().Name == "SkinnedTransformer")
                {
                    var GLFTMesh = Instances[i].Content.GetGeometryAsset();
                    var SkinnedMesh = (SharpGLTF.Scenes.SkinnedTransformer)Instances[i].Content;
                    TrickyModelCombiner.ReassignedMesh reassignedMesh = new TrickyModelCombiner.ReassignedMesh();
                    reassignedMesh.IKPoints = IKPoints;
                    reassignedMesh.MeshName = GLFTMesh.Name;
                    reassignedMesh.faces = new List<TrickyMPFModelHandler.Face>();

                    var JointBindings = SkinnedMesh.GetJointBindings();
                    var MaterialArray = GLFTMesh.Primitives.ToArray();
                    for (int a = 0; a < MaterialArray.Length; a++)
                    {
                        //Build New Material With Primitive Name
                        trickyModelCombiner.materials = MakeNewMaterial(trickyModelCombiner.materials, MaterialArray[a].Material.Name);

                        int MatID = FindMaterialID(trickyModelCombiner.materials, MaterialArray[a].Material.Name);

                        //Build Vertex List
                        List<VertexData> VertexList = new List<VertexData>();
                        var OldVertexList = MaterialArray[a].Vertices.ToArray();
                        int MorphTargetsCount = MaterialArray[a].MorphTargets.Count;

                        for (int b = 0; b < OldVertexList.Length; b++)
                        {
                            var NewVertex = new VertexData();

                            var TempGeo = OldVertexList[b].GetGeometry();
                            var TempSkinning = OldVertexList[b].GetSkinning();
                            var TempMaterial = OldVertexList[b].GetMaterial();

                            //Get Postion Normal and UV
                            NewVertex.Position = TempGeo.GetPosition();

                            Vector3 TempNormal;
                            if (TempGeo.TryGetNormal(out TempNormal))
                            {
                                NewVertex.Normal = TempNormal;
                            }
                            try
                            {
                                NewVertex.UV = TempMaterial.GetTexCoord(0);
                            }
                            catch
                            {
                                NewVertex.UV = new Vector2();
                            }

                            //Get Weights
                            NewVertex.weightHeader = new TrickyMPFModelHandler.BoneWeightHeader();
                            NewVertex.weightHeader.unknown = 36;
                            NewVertex.weightHeader.boneWeights = new List<TrickyMPFModelHandler.BoneWeight>();

                            for (int d = 0; d < TempSkinning.MaxBindings; d++)
                            {
                                var BindingList = TempSkinning.GetBinding(d);
                                if (BindingList.Weight != 0)
                                {
                                    TrickyMPFModelHandler.BoneWeight TempWeight = new TrickyMPFModelHandler.BoneWeight();
                                    TempWeight.BoneID = BindingList.Index;
                                    TempWeight.Weight = (int)(BindingList.Weight * 100f);
                                    TempWeight.boneName = JointBindings[BindingList.Index].Joint.Name;
                                    NewVertex.weightHeader.boneWeights.Add(TempWeight);
                                }
                            }

                            //Add Morph Data
                            NewVertex.MorphPoints = new List<Vector3>();
                            for (int d = 0; d < MorphTargetsCount; d++)
                            {
                                NewVertex.MorphPoints.Add(MaterialArray[a].MorphTargets[d].GetVertex(b).GetGeometry().GetPosition() - NewVertex.Position);
                            }

                            VertexList.Add(NewVertex);
                        }

                        //Build Faces
                        var TriangleList = MaterialArray[a].Triangles;
                        for (int b = 0; b < TriangleList.Count; b++)
                        {
                            TrickyMPFModelHandler.Face TempFace = new TrickyMPFModelHandler.Face();
                            var FaceTri = TriangleList[b];

                            var TempPoint = VertexList[FaceTri.A];

                            TempFace.V1 = TempPoint.Position;
                            TempFace.Normal1 = TempPoint.Normal;
                            TempFace.UV1 = new Vector4(TempPoint.UV, 0, 0);
                            TempFace.Weight1 = TempPoint.weightHeader;
                            TempFace.MorphPoint1 = TempPoint.MorphPoints;

                            TempPoint = VertexList[FaceTri.B];

                            TempFace.V2 = TempPoint.Position;
                            TempFace.Normal2 = TempPoint.Normal;
                            TempFace.UV2 = new Vector4(TempPoint.UV, 0, 0);
                            TempFace.Weight2 = TempPoint.weightHeader;
                            TempFace.MorphPoint2 = TempPoint.MorphPoints;

                            TempPoint = VertexList[FaceTri.C];

                            TempFace.V3 = TempPoint.Position;
                            TempFace.Normal3 = TempPoint.Normal;
                            TempFace.UV3 = new Vector4(TempPoint.UV, 0, 0);
                            TempFace.Weight3 = TempPoint.weightHeader;
                            TempFace.MorphPoint3 = TempPoint.MorphPoints;

                            TempFace.MaterialID = MatID;

                            reassignedMesh.faces.Add(TempFace);
                        }
                    }

                    trickyModelCombiner.reassignedMesh.Add(reassignedMesh);
                }
            }

            return trickyModelCombiner;
        }

        static List<TrickyMPFModelHandler.MaterialData> MakeNewMaterial(List<TrickyMPFModelHandler.MaterialData> materials, string name)
        {
            bool test = false;
            for (int i = 0; i < materials.Count; i++)
            {
                if (materials[i].MainTexture == name)
                {
                    test = true;
                    break;
                }
            }
            if(!test)
            {
                TrickyMPFModelHandler.MaterialData newmat = new TrickyMPFModelHandler.MaterialData();
                newmat.MainTexture = name;
                materials.Add(newmat);
            }

            return materials;
        }

        static int FindMaterialID(List<TrickyMPFModelHandler.MaterialData> materials, string name)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                if (materials[i].MainTexture == name)
                {
                    return i;
                }
            }
            return -1;
        }

        struct VertexData
        {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector2 UV;

            public TrickyMPFModelHandler.BoneWeightHeader weightHeader;
            public List<Vector3> MorphPoints;
        }

        public static List<TrickyMPFModelHandler.BoneData> ReturnBoneAndChildren(SharpGLTF.Scenes.NodeBuilder nodeBuilder, bool startBone)
        {
            List<TrickyMPFModelHandler.BoneData> boneDatas = new List<TrickyMPFModelHandler.BoneData>();
            var TempBoneData = new TrickyMPFModelHandler.BoneData();

            TempBoneData.BoneName = nodeBuilder.Name;
            TempBoneData.Position = nodeBuilder.LocalTransform.Translation;
            if (nodeBuilder.Parent != null)
            {
                TempBoneData.parentName = nodeBuilder.Parent.Name;
            }
            Quaternion quaternion = nodeBuilder.LocalTransform.GetDecomposed().Rotation;
            Vector3 radians = ToEulerAngles(quaternion);
            TempBoneData.Radians = radians;

            boneDatas.Add(TempBoneData);

            for (int i = 0; i < nodeBuilder.VisualChildren.Count; i++)
            {
                boneDatas.AddRange(ReturnBoneAndChildren(nodeBuilder.VisualChildren[i], false));
            }

            return boneDatas;
        }

        public static PointMorph GeneratePointMorph(Vector3 Point, List<Vector3> MorphPoints)
        {
            PointMorph pointMorph = new PointMorph();
            pointMorph.Point = Point;
            pointMorph.MorphPoints = MorphPoints;
            return pointMorph;
        }

        public struct PointMorph
        {
            public Vector3 Point;
            public List<Vector3> MorphPoints;
        }

        public static Quaternion ToQuaternion(Vector3 v)
        {
            float cy = (float)Math.Cos(v.Z * 0.5);
            float sy = (float)Math.Sin(v.Z * 0.5);
            float cp = (float)Math.Cos(v.Y * 0.5);
            float sp = (float)Math.Sin(v.Y * 0.5);
            float cr = (float)Math.Cos(v.X * 0.5);
            float sr = (float)Math.Sin(v.X * 0.5);

            return new Quaternion
            {
                W = (cr * cp * cy + sr * sp * sy),
                X = (sr * cp * cy - cr * sp * sy),
                Y = (cr * sp * cy + sr * cp * sy),
                Z = (cr * cp * sy - sr * sp * cy)
            };

        }

        public static Vector3 ToEulerAngles(Quaternion q)
        {
            Vector3 angles = new();

            // roll / x
            double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            angles.X = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch / y
            double sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1)
            {
                angles.Y = (float)Math.CopySign(Math.PI / 2, sinp);
            }
            else
            {
                angles.Y = (float)Math.Asin(sinp);
            }

            // yaw / z
            double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            angles.Z = (float)Math.Atan2(siny_cosp, cosy_cosp);

            return angles;
        }
    }
}
