﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Schema2;
using SSXMultiTool.FileHandlers.Models.Tricky;
using SSXMultiTool.FileHandlers.Models.OnTour;
using SSXMultiTool.FileHandlers.Models.SSX3;
using SSXMultiTool.Utilities;
using SSXMultiTool.FileHandlers.Models.SSXOG;

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

                Binding.WithLocalRotation(MathUtil.ToQuaternion(new Vector3(tempX, tempY, tempZ)));

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
                        Matrix4x4 matrix4X4 = bindings[Data.faces[b].BoneAssigment].WorldMatrix;

                        var Face = Data.faces[b];
                        VertexPositionNormal TempPos1 = new VertexPositionNormal();
                        TempPos1.Position = Vector3.Transform(Face.V1, matrix4X4);
                        TempPos1.Normal = Face.Normal1;

                        VertexPositionNormal TempPos2 = new VertexPositionNormal();
                        TempPos2.Position = Vector3.Transform(Face.V2, matrix4X4);
                        TempPos2.Normal = Face.Normal2;

                        VertexPositionNormal TempPos3 = new VertexPositionNormal();
                        TempPos3.Position = Vector3.Transform(Face.V3, matrix4X4);
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
                            bindings1[ia] = (Data.BoneAssigment, 1);
                        }
                        TempBinding1.SetBindings(bindings1);

                        VertexJoints4 TempBinding2 = new VertexJoints4();
                        bindings1 = new (int Temp, float TempFloat)[1];
                        for (int ia = 0; ia < 1; ia++)
                        {
                            bindings1[ia] = (Data.BoneAssigment, 1);
                        }
                        TempBinding2.SetBindings(bindings1);

                        VertexJoints4 TempBinding3 = new VertexJoints4();
                        bindings1 = new (int Temp, float TempFloat)[1];
                        for (int ia = 0; ia < 1; ia++)
                        {
                            bindings1[ia] = (Data.BoneAssigment, 1);
                        }
                        TempBinding3.SetBindings(bindings1);

                        mesh.UsePrimitive(materialBuilders[MatId]).AddTriangle((TempPos1, TempTexture1, TempBinding1), (TempPos2, TempTexture2, TempBinding2), (TempPos3, TempTexture3, TempBinding3));
                    }
                }

                for (int a = 0; a < modelHeader.MaterialGroups[i].flexableMesh.Count; a++)
                {
                    var Data = modelHeader.MaterialGroups[i].flexableMesh[a];
                    int MatId = modelHeader.MaterialGroups[i].MaterialID;

                    for (int b = 0; b < Data.faces.Count; b++)
                    {
                        var Face = Data.faces[b];
                        Matrix4x4 matrix4X4 = Matrix4x4.CreateScale(1.0f, 1.0f, 1.0f);

                        VertexPositionNormal TempPos1 = new VertexPositionNormal();
                        TempPos1.Position = Vector3.Transform(Face.V1, matrix4X4);
                        TempPos1.Normal = new Vector3(0,0,0);

                        VertexPositionNormal TempPos2 = new VertexPositionNormal();
                        TempPos2.Position = Vector3.Transform(Face.V2, matrix4X4);
                        TempPos2.Normal = new Vector3(0, 0, 0);

                        VertexPositionNormal TempPos3 = new VertexPositionNormal();
                        TempPos3.Position = Vector3.Transform(Face.V3, matrix4X4);
                        TempPos3.Normal = new Vector3(0, 0, 0);

                        VertexTexture1 TempTexture1 = new VertexTexture1();
                        TempTexture1.TexCoord = new Vector2(0,0);

                        VertexTexture1 TempTexture2 = new VertexTexture1();
                        TempTexture2.TexCoord = new Vector2(0, 0);

                        VertexTexture1 TempTexture3 = new VertexTexture1();
                        TempTexture3.TexCoord = new Vector2(0, 0);

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

        public static Vector3 SwapYZ(Vector3 vector3)
        {
            return new Vector3(vector3.X, vector3.Z, vector3.Z);
        }

        public static void SaveTrickyPS2glTF(string Output, TrickyPS2ModelCombiner Handler)
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

                Binding.WithLocalRotation(MathUtil.ToQuaternion(new Vector3(-tempX, -tempY, -tempZ)));
                Binding.WithLocalTranslation(Handler.bones[i].Position);

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
                    
                    scene.AddSkinnedMesh(mesh, Matrix4x4.CreateFromYawPitchRoll(0, 0/*-1.5708f*/, 0), bindings.ToArray());
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
                    scene.AddSkinnedMesh(mesh, Matrix4x4.CreateFromYawPitchRoll(0, 0, 0), bindings.ToArray());
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


            //Matrix4x4 matrix4X4 = Matrix4x4.CreateRotationX(-1.5708f);

            //scene.ApplyBasisTransform(matrix4X4);

            // save the model in different formats
            var model = scene.ToGltf2();
            //model.ApplyBasisTransform(matrix4X4);
            model.SaveGLB(Output);
        }

        public static void SaveTrickyXboxglTF(string Output, TrickyXboxModelCombiner Handler)
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

                Binding.WithLocalRotation(MathUtil.ToQuaternion(new Vector3(-tempX, -tempY, -tempZ)));
                Binding.WithLocalTranslation(Handler.bones[i].Position);

                bindings.Add(Binding);
            }

            for (int i = 0; i < Handler.reassignedMesh.Count; i++)
            {
                List<PointMorph> pointMorphs = new List<PointMorph>();
                var mesh = new MeshBuilder<VertexPositionNormalTangent, VertexTexture1, VertexJoints4>(Handler.reassignedMesh[i].MeshName);

                    var TristripTemp = Handler.reassignedMesh[i];
                    for (int b = 0; b < TristripTemp.faces.Count; b++)
                    {
                        var Face = TristripTemp.faces[b];

                        VertexPositionNormalTangent TempPos1 = new VertexPositionNormalTangent();
                        TempPos1.Position = Face.V1;
                        TempPos1.Normal = Face.Normal1;

                        if(Face.TangentNormal1 == Vector3.Zero)
                        {
                            Face.TangentNormal1 = new Vector3(1f / 3f, 1f / 3f, 1f / 3f);
                        }
                        TempPos1.Tangent = new Vector4(Face.TangentNormal1.X, Face.TangentNormal1.Y, Face.TangentNormal1.Z, 1);

                        VertexPositionNormalTangent TempPos2 = new VertexPositionNormalTangent();
                        TempPos2.Position = Face.V2;
                        TempPos2.Normal = Face.Normal2;

                        if (Face.TangentNormal2 == Vector3.Zero)
                        {
                            Face.TangentNormal2 = new Vector3(1f / 3f, 1f / 3f, 1f / 3f);
                        }
                        TempPos2.Tangent = new Vector4(Face.TangentNormal2.X, Face.TangentNormal2.Y, Face.TangentNormal2.Z, 1);

                        VertexPositionNormalTangent TempPos3 = new VertexPositionNormalTangent();
                        TempPos3.Position = Face.V3;
                        TempPos3.Normal = Face.Normal3;
                        if (Face.TangentNormal3 == Vector3.Zero)
                        {
                            Face.TangentNormal3 = new Vector3(1f / 3f, 1f / 3f, 1f / 3f);
                        }
                        TempPos3.Tangent = new Vector4(Face.TangentNormal3.X, Face.TangentNormal3.Y, Face.TangentNormal3.Z, 1);

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

                        if (Handler.reassignedMesh[i].MorphTargetCount != 0)
                        {
                            if (!pointMorphs.Contains(GeneratePointMorph(TempPos1.Position, Face.MorphPoint1)))
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
                

                for (int c = 0; c < Handler.reassignedMesh[i].MorphTargetCount; c++)
                {
                    var morphTargetBuilder = mesh.UseMorphTarget(c);
                    foreach (var vertexPosition in morphTargetBuilder.Vertices)
                    {
                        for (int d = 0; d < pointMorphs.Count; d++)
                        {
                            if (pointMorphs[d].Point == vertexPosition.Position)
                            {
                                var NewVertexPosition = vertexPosition;
                                NewVertexPosition.Position += pointMorphs[d].MorphPoints[c];
                                morphTargetBuilder.SetVertex(vertexPosition, NewVertexPosition);
                            }
                        }
                    }
                }

                scene.AddSkinnedMesh(mesh, Matrix4x4.CreateFromYawPitchRoll(0, 0/*-1.5708f*/, 0), bindings.ToArray());
            }
            if (Handler.reassignedMesh[0].IKPoints != null)
            {
                for (int i = 0; i < Handler.reassignedMesh[0].IKPoints.Count; i++)
                {
                    var Temp = new SharpGLTF.Scenes.NodeBuilder("IKPoint " + i.ToString());
                    Temp.WithLocalTranslation(Handler.reassignedMesh[0].IKPoints[i]);
                    scene.AddNode(Temp);
                }
            }
            // save the model in different formats
            var model = scene.ToGltf2();
            model.SaveGLB(Output);
        }

        public static void SaveTrickyGCglTF(string Output, TrickyGCModelCombiner Handler)
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

                Binding.WithLocalRotation(MathUtil.ToQuaternion(new Vector3(-tempX, -tempY, -tempZ)));
                Binding.WithLocalTranslation(Handler.bones[i].Position);

                bindings.Add(Binding);
            }

            for (int i = 0; i < Handler.reassignedMesh.Count; i++)
            {
                List<PointMorph> pointMorphs = new List<PointMorph>();
                var mesh = new MeshBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4>(Handler.reassignedMesh[i].MeshName);

                var TristripTemp = Handler.reassignedMesh[i];
                for (int b = 0; b < TristripTemp.faces.Count; b++)
                {
                    var Face = TristripTemp.faces[b];

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

                    if (Handler.reassignedMesh[i].MorphTargetCount != 0)
                    {
                        if (!pointMorphs.Contains(GeneratePointMorph(TempPos1.Position, Face.MorphPoint1)))
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

                for (int c = 0; c < Handler.reassignedMesh[i].MorphTargetCount; c++)
                {
                    var morphTargetBuilder = mesh.UseMorphTarget(c);
                    foreach (var vertexPosition in morphTargetBuilder.Vertices)
                    {
                        for (int d = 0; d < pointMorphs.Count; d++)
                        {
                            if (pointMorphs[d].Point == vertexPosition.Position)
                            {
                                var NewVertexPosition = vertexPosition;
                                NewVertexPosition.Position += pointMorphs[d].MorphPoints[c];
                                morphTargetBuilder.SetVertex(vertexPosition, NewVertexPosition);
                            }
                        }
                    }
                }

                scene.AddSkinnedMesh(mesh, Matrix4x4.CreateFromYawPitchRoll(0, 0/*-1.5708f*/, 0), bindings.ToArray());
            }
            if (Handler.reassignedMesh[0].IKPoints != null)
            {
                for (int i = 0; i < Handler.reassignedMesh[0].IKPoints.Count; i++)
                {
                    var Temp = new SharpGLTF.Scenes.NodeBuilder("IKPoint " + i.ToString());
                    Temp.WithLocalTranslation(Handler.reassignedMesh[0].IKPoints[i]);
                    scene.AddNode(Temp);
                }
            }
            // save the model in different formats
            var model = scene.ToGltf2();
            model.SaveGLB(Output);
        }

        public static void SaveSSX3Glft(string Output, SSX3PS2ModelCombiner Handler)
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
            for (int Z = 0; Z < Handler.reassignedMesh.Count; Z++)
            {
                if (!Handler.reassignedMesh[Z].ShadowModel)
                {
                    List<PointMorph> pointMorphs = new List<PointMorph>();
                    var mesh = new MeshBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4>(Handler.reassignedMesh[Z].MeshName);
                    for (int b = 0; b < Handler.reassignedMesh[Z].faces.Count; b++)
                    {
                        var Face = Handler.reassignedMesh[Z].faces[b];
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

                        mesh.UsePrimitive(materialBuilders[Handler.reassignedMesh[Z].faces[b].MaterialID]).AddTriangle((TempPos1, TempTexture1, TempBinding1), (TempPos2, TempTexture2, TempBinding2), (TempPos3, TempTexture3, TempBinding3));

                        if (Handler.reassignedMesh[Z].MorphTargetCount != 0)
                        {
                            if (!pointMorphs.Contains(GeneratePointMorph(TempPos1.Position, Face.MorphPoint1)))
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

                    for (int c = 0; c < Handler.reassignedMesh[Z].MorphTargetCount; c++)
                    {
                        var morphTargetBuilder = mesh.UseMorphTarget(c);
                        foreach (var vertexPosition in morphTargetBuilder.Vertices)
                        {
                            for (int i = 0; i < pointMorphs.Count; i++)
                            {
                                if (pointMorphs[i].Point == vertexPosition.Position)
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
                    var mesh = new MeshBuilder<VertexPosition, VertexEmpty, VertexJoints4>(Handler.reassignedMesh[Z].MeshName);
                    for (int b = 0; b < Handler.reassignedMesh[Z].faces.Count; b++)
                    {
                        var Face = Handler.reassignedMesh[Z].faces[b];
                        VertexPosition TempPos1 = new VertexPosition();
                        TempPos1.Position = Face.V1;

                        VertexPosition TempPos2 = new VertexPosition();
                        TempPos2.Position = Face.V2;

                        VertexPosition TempPos3 = new VertexPosition();
                        TempPos3.Position = Face.V3;

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

                        mesh.UsePrimitive(materialBuilders[Face.MaterialID]).AddTriangle((TempPos1, TempBinding1), (TempPos2, TempBinding2), (TempPos3, TempBinding3));
                    }
                    scene.AddSkinnedMesh(mesh, Matrix4x4.CreateTranslation(0, 0, 0), bindings.ToArray());
                }
            }

                //scene.AddSkinnedMesh(mesh, Matrix4x4.CreateTranslation(0, 0, 0), bindings.ToArray());
            // save the model in different formats
            var model = scene.ToGltf2();
            model.SaveGLB(Output);
        }

        public static void SaveSSX3GCGlft(string Output, SSX3GCModelCombiner Handler)
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
            for (int Z = 0; Z < Handler.reassignedMesh.Count; Z++)
            {
                if (!Handler.reassignedMesh[Z].ShadowModel)
                {
                    List<PointMorph> pointMorphs = new List<PointMorph>();
                    var mesh = new MeshBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4>(Handler.reassignedMesh[Z].MeshName);
                    for (int b = 0; b < Handler.reassignedMesh[Z].faces.Count; b++)
                    {
                        var Face = Handler.reassignedMesh[Z].faces[b];
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

                        mesh.UsePrimitive(materialBuilders[Handler.reassignedMesh[Z].faces[b].MaterialID]).AddTriangle((TempPos1, TempTexture1, TempBinding1), (TempPos2, TempTexture2, TempBinding2), (TempPos3, TempTexture3, TempBinding3));

                        if (Handler.reassignedMesh[Z].MorphTargetCount != 0)
                        {
                            if (!pointMorphs.Contains(GeneratePointMorph(TempPos1.Position, Face.MorphPoint1)))
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

                    for (int c = 0; c < Handler.reassignedMesh[Z].MorphTargetCount; c++)
                    {
                        var morphTargetBuilder = mesh.UseMorphTarget(c);
                        foreach (var vertexPosition in morphTargetBuilder.Vertices)
                        {
                            for (int i = 0; i < pointMorphs.Count; i++)
                            {
                                if (pointMorphs[i].Point == vertexPosition.Position)
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
                    var mesh = new MeshBuilder<VertexPosition, VertexEmpty, VertexJoints4>(Handler.reassignedMesh[Z].MeshName);
                    for (int b = 0; b < Handler.reassignedMesh[Z].faces.Count; b++)
                    {
                        var Face = Handler.reassignedMesh[Z].faces[b];
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
            }

            //scene.AddSkinnedMesh(mesh, Matrix4x4.CreateTranslation(0, 0, 0), bindings.ToArray());
            // save the model in different formats
            var model = scene.ToGltf2();
            model.SaveGLB(Output);
        }

        public static void SaveSSXOnTourGlft(string Output, SSXOnTourPS2ModelCombiner Handler)
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
            for (int Z = 0; Z < Handler.reassignedMesh.Count; Z++)
            {
                if (!Handler.reassignedMesh[Z].ShadowModel)
                {
                    List<PointMorph> pointMorphs = new List<PointMorph>();
                    var mesh = new MeshBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4>(Handler.reassignedMesh[Z].MeshName);
                    for (int b = 0; b < Handler.reassignedMesh[Z].faces.Count; b++)
                    {
                        var Face = Handler.reassignedMesh[Z].faces[b];
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

                        mesh.UsePrimitive(materialBuilders[Handler.reassignedMesh[Z].faces[b].MaterialID]).AddTriangle((TempPos1, TempTexture1, TempBinding1), (TempPos2, TempTexture2, TempBinding2), (TempPos3, TempTexture3, TempBinding3));

                        if (Handler.reassignedMesh[Z].MorphTargetCount != 0 || Handler.reassignedMesh[Z].AltMorphTargetCount !=0)
                        {
                            if (!pointMorphs.Contains(GeneratePointMorph(TempPos1.Position, Face.MorphPoint1, Face.AltMorphPoint1, Face.AltMorphNormal1)))
                            {
                                pointMorphs.Add(GeneratePointMorph(TempPos1.Position, Face.MorphPoint1, Face.AltMorphPoint1, Face.AltMorphNormal1));
                            }
                            if (!pointMorphs.Contains(GeneratePointMorph(TempPos2.Position, Face.MorphPoint2, Face.AltMorphPoint1, Face.AltMorphNormal1)))
                            {
                                pointMorphs.Add(GeneratePointMorph(TempPos2.Position, Face.MorphPoint2, Face.AltMorphPoint1, Face.AltMorphNormal1));
                            }
                            if (!pointMorphs.Contains(GeneratePointMorph(TempPos3.Position, Face.MorphPoint3, Face.AltMorphPoint1, Face.AltMorphNormal1)))
                            {
                                pointMorphs.Add(GeneratePointMorph(TempPos3.Position, Face.MorphPoint3, Face.AltMorphPoint1, Face.AltMorphNormal1));
                            }
                        }
                    }


                    for (int c = 0; c < Handler.reassignedMesh[Z].MorphTargetCount; c++)
                    {
                        var morphTargetBuilder = mesh.UseMorphTarget(c);
                        foreach (var vertexPosition in morphTargetBuilder.Vertices)
                        {
                            for (int i = 0; i < pointMorphs.Count; i++)
                            {
                                if (pointMorphs[i].Point == vertexPosition.Position)
                                {
                                    var NewVertexPosition = vertexPosition;
                                    NewVertexPosition.Position += pointMorphs[i].MorphPoints[c];
                                    morphTargetBuilder.SetVertex(vertexPosition, NewVertexPosition);
                                }
                            }
                        }
                    }

                    for (int c = 0; c < Handler.reassignedMesh[Z].AltMorphTargetCount; c++)
                    {
                        var morphTargetBuilder = mesh.UseMorphTarget(Handler.reassignedMesh[Z].MorphTargetCount + c);
                        foreach (var vertexPosition in morphTargetBuilder.Vertices)
                        {
                            for (int i = 0; i < pointMorphs.Count; i++)
                            {
                                if (pointMorphs[i].Point == vertexPosition.Position)
                                {
                                    var NewVertexPosition = vertexPosition;
                                    NewVertexPosition.Position += pointMorphs[i].AltMorphPoints[c];
                                    NewVertexPosition.Normal += pointMorphs[i].AltMorphNormal[c];
                                    morphTargetBuilder.SetVertex(vertexPosition, NewVertexPosition);
                                }
                            }
                        }
                    }

                    scene.AddSkinnedMesh(mesh, Matrix4x4.CreateTranslation(0, 0, 0), bindings.ToArray());
                }
                else
                {
                    var mesh = new MeshBuilder<VertexPosition, VertexEmpty, VertexJoints4>(Handler.reassignedMesh[Z].MeshName);
                    for (int b = 0; b < Handler.reassignedMesh[Z].faces.Count; b++)
                    {
                        var Face = Handler.reassignedMesh[Z].faces[b];
                        VertexPosition TempPos1 = new VertexPosition();
                        TempPos1.Position = Face.V1;

                        VertexPosition TempPos2 = new VertexPosition();
                        TempPos2.Position = Face.V2;

                        VertexPosition TempPos3 = new VertexPosition();
                        TempPos3.Position = Face.V3;

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

                        mesh.UsePrimitive(materialBuilders[Face.MaterialID]).AddTriangle((TempPos1, TempBinding1), (TempPos2, TempBinding2), (TempPos3, TempBinding3));
                    }
                    scene.AddSkinnedMesh(mesh, Matrix4x4.CreateTranslation(0, 0, 0), bindings.ToArray());
                }
            }

            //scene.AddSkinnedMesh(mesh, Matrix4x4.CreateTranslation(0, 0, 0), bindings.ToArray());
            // save the model in different formats
            var model = scene.ToGltf2();
            model.SaveGLB(Output);
        }

        public static TrickyPS2ModelCombiner LoadTrickyPS2Glft(string Path)
        {
            TrickyPS2ModelCombiner trickyModelCombiner = new TrickyPS2ModelCombiner();
            trickyModelCombiner.materials = new List<TrickyPS2MPF.MaterialData>();
            trickyModelCombiner.reassignedMesh = new List<TrickyPS2ModelCombiner.ReassignedMesh>();
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
            List<TrickyPS2MPF.BoneData> boneDatas = new List<TrickyPS2MPF.BoneData>();
            var Ampatures = Scene.FindArmatures();

            for (int a = 0; a < Ampatures[0].VisualChildren.Count; a++)
            {
                if (Ampatures[0].VisualChildren[a].Name.ToLower() == "hips")
                {
                    var StartBone = Ampatures[0].VisualChildren[a];
                    boneDatas = ReturnBoneAndChildrenTrickyPS2(StartBone, true);
                }
                else if (Ampatures[0].VisualChildren[a].Name.ToLower() == "board")
                {
                    var StartBone = Ampatures[0].VisualChildren[a];
                    boneDatas = ReturnBoneAndChildrenTrickyPS2(StartBone, true);
                }
            }

            trickyModelCombiner.bones = boneDatas;

            for (int i = 0; i < Instances.Length; i++)
            {
                if (Instances[i].Content.GetType().Name == "SkinnedTransformer")
                {
                    var GLFTMesh = Instances[i].Content.GetGeometryAsset();
                    var SkinnedMesh = (SharpGLTF.Scenes.SkinnedTransformer)Instances[i].Content;
                    TrickyPS2ModelCombiner.ReassignedMesh reassignedMesh = new TrickyPS2ModelCombiner.ReassignedMesh();
                    reassignedMesh.IKPoints = IKPoints;
                    reassignedMesh.MeshName = GLFTMesh.Name;
                    reassignedMesh.faces = new List<TrickyPS2MPF.Face>();

                    var JointBindings = SkinnedMesh.GetJointBindings();
                    var MaterialArray = GLFTMesh.Primitives.ToArray();
                    for (int a = 0; a < MaterialArray.Length; a++)
                    {
                        //Build New Material With Primitive Name
                        trickyModelCombiner.materials = MakeNewMaterialTrickyPS2(trickyModelCombiner.materials, MaterialArray[a].Material.Name);

                        int MatID = FindMaterialIDTrickyPS2(trickyModelCombiner.materials, MaterialArray[a].Material.Name);

                        //Build Vertex List
                        List<VertexDataTrickyPS2> VertexList = new List<VertexDataTrickyPS2>();
                        var OldVertexList = MaterialArray[a].Vertices.ToArray();
                        int MorphTargetsCount = MaterialArray[a].MorphTargets.Count;

                        for (int b = 0; b < OldVertexList.Length; b++)
                        {
                            var NewVertex = new VertexDataTrickyPS2();

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
                            NewVertex.weightHeader = new TrickyPS2MPF.BoneWeightHeader();
                            NewVertex.weightHeader.Unknown = 36;
                            NewVertex.weightHeader.boneWeights = new List<TrickyPS2MPF.BoneWeight>();

                            for (int d = 0; d < TempSkinning.MaxBindings; d++)
                            {
                                var BindingList = TempSkinning.GetBinding(d);
                                if (BindingList.Weight != 0)
                                {
                                    TrickyPS2MPF.BoneWeight TempWeight = new TrickyPS2MPF.BoneWeight();
                                    TempWeight.BoneID = BindingList.Index;
                                    TempWeight.Weight = (int)(BindingList.Weight * 100f);
                                    TempWeight.boneName = JointBindings[BindingList.Index].Joint.Name;
                                    NewVertex.weightHeader.boneWeights.Add(TempWeight);
                                }
                            }

                            //Correct Weights
                            int Count = 0;
                            for (int d = 0; d < NewVertex.weightHeader.boneWeights.Count; d++)
                            {
                                Count += NewVertex.weightHeader.boneWeights[d].Weight;
                            }

                            if(Count!=100)
                            {
                                var TempWeight = NewVertex.weightHeader.boneWeights[0];
                                if (Count>100)
                                {
                                    TempWeight.Weight -= Count - 100;
                                }
                                else if (Count < 100)
                                {
                                    TempWeight.Weight += 100 - Count;
                                }
                                NewVertex.weightHeader.boneWeights[0] = TempWeight;
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
                            TrickyPS2MPF.Face TempFace = new TrickyPS2MPF.Face();
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

        public static TrickyXboxModelCombiner LoadTrickyXboxGlft(string Path)
        {
            TrickyXboxModelCombiner trickyModelCombiner = new TrickyXboxModelCombiner();
            trickyModelCombiner.materials = new List<TrickyXboxMXF.MaterialData>();
            trickyModelCombiner.reassignedMesh = new List<TrickyXboxModelCombiner.ReassignedMesh>();
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
            List<TrickyXboxMXF.BoneData> boneDatas = new List<TrickyXboxMXF.BoneData>();
            var Ampatures = Scene.FindArmatures();

            for (int a = 0; a < Ampatures[0].VisualChildren.Count; a++)
            {
                if (Ampatures[0].VisualChildren[a].Name.ToLower() == "hips")
                {
                    var StartBone = Ampatures[0].VisualChildren[a];
                    boneDatas = ReturnBoneAndChildrenTrickyXbox(StartBone, true);
                }
                else if (Ampatures[0].VisualChildren[a].Name.ToLower() == "board")
                {
                    var StartBone = Ampatures[0].VisualChildren[a];
                    boneDatas = ReturnBoneAndChildrenTrickyXbox(StartBone, true);
                }
            }

            trickyModelCombiner.bones = boneDatas;

            for (int i = 0; i < Instances.Length; i++)
            {
                if (Instances[i].Content.GetType().Name == "SkinnedTransformer")
                {
                    var GLFTMesh = Instances[i].Content.GetGeometryAsset();
                    var SkinnedMesh = (SharpGLTF.Scenes.SkinnedTransformer)Instances[i].Content;
                    TrickyXboxModelCombiner.ReassignedMesh reassignedMesh = new TrickyXboxModelCombiner.ReassignedMesh();
                    reassignedMesh.IKPoints = IKPoints;
                    reassignedMesh.MeshName = GLFTMesh.Name;
                    reassignedMesh.faces = new List<TrickyXboxMXF.Face>();

                    var JointBindings = SkinnedMesh.GetJointBindings();
                    var MaterialArray = GLFTMesh.Primitives.ToArray();
                    for (int a = 0; a < MaterialArray.Length; a++)
                    {
                        //Build New Material With Primitive Name
                        trickyModelCombiner.materials = MakeNewMaterialTrickyXbox(trickyModelCombiner.materials, MaterialArray[a].Material.Name);

                        int MatID = FindMaterialIDTrickyXbox(trickyModelCombiner.materials, MaterialArray[a].Material.Name);

                        //Build Vertex List
                        List<VertexDataTrickyXbox> VertexList = new List<VertexDataTrickyXbox>();
                        var OldVertexList = MaterialArray[a].Vertices.ToArray();
                        int MorphTargetsCount = MaterialArray[a].MorphTargets.Count;

                        for (int b = 0; b < OldVertexList.Length; b++)
                        {
                            var NewVertex = new VertexDataTrickyXbox();

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
                            else
                            {
                                MessageBox.Show("Must Contain Normals");
                                return null;
                            }

                            Vector4 TempTangent;
                            if (TempGeo.TryGetTangent(out TempTangent))
                            {
                                NewVertex.Tangent = TempTangent;
                            }
                            else
                            {
                                MessageBox.Show("Must Contain Tangents");
                                return null;
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
                            NewVertex.weightHeader = new TrickyXboxMXF.BoneWeightHeader();
                            NewVertex.weightHeader.Unknown2 = 19;
                            NewVertex.weightHeader.boneWeights = new List<TrickyXboxMXF.BoneWeight>();

                            for (int d = 0; d < TempSkinning.MaxBindings; d++)
                            {
                                var BindingList = TempSkinning.GetBinding(d);
                                if (BindingList.Weight != 0)
                                {
                                    TrickyXboxMXF.BoneWeight TempWeight = new TrickyXboxMXF.BoneWeight();
                                    TempWeight.BoneID = BindingList.Index;
                                    TempWeight.Weight = (int)(BindingList.Weight * 100f);
                                    TempWeight.BoneName = JointBindings[BindingList.Index].Joint.Name;
                                    NewVertex.weightHeader.boneWeights.Add(TempWeight);
                                }
                            }

                            //Correct Weights
                            int Count = 0;
                            for (int d = 0; d < NewVertex.weightHeader.boneWeights.Count; d++)
                            {
                                Count += NewVertex.weightHeader.boneWeights[d].Weight;
                            }

                            if (Count != 100)
                            {
                                var TempWeight = NewVertex.weightHeader.boneWeights[0];
                                if (Count > 100)
                                {
                                    TempWeight.Weight -= Count - 100;
                                }
                                else if (Count < 100)
                                {
                                    TempWeight.Weight += 100 - Count;
                                }
                                NewVertex.weightHeader.boneWeights[0] = TempWeight;
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
                            TrickyXboxMXF.Face TempFace = new TrickyXboxMXF.Face();
                            var FaceTri = TriangleList[b];

                            var TempPoint = VertexList[FaceTri.A];

                            TempFace.V1 = TempPoint.Position;
                            TempFace.Normal1 = TempPoint.Normal;
                            TempFace.UV1 = TempPoint.UV;
                            TempFace.Weight1 = TempPoint.weightHeader;
                            TempFace.MorphPoint1 = TempPoint.MorphPoints;
                            TempFace.TangentNormal1 = new Vector3(TempPoint.Tangent.X, TempPoint.Tangent.Y, TempPoint.Tangent.Z);

                            TempPoint = VertexList[FaceTri.B];

                            TempFace.V2 = TempPoint.Position;
                            TempFace.Normal2 = TempPoint.Normal;
                            TempFace.UV2 = TempPoint.UV;
                            TempFace.Weight2 = TempPoint.weightHeader;
                            TempFace.MorphPoint2 = TempPoint.MorphPoints;
                            TempFace.TangentNormal2 = new Vector3(TempPoint.Tangent.X, TempPoint.Tangent.Y, TempPoint.Tangent.Z);

                            TempPoint = VertexList[FaceTri.C];

                            TempFace.V3 = TempPoint.Position;
                            TempFace.Normal3 = TempPoint.Normal;
                            TempFace.UV3 = TempPoint.UV;
                            TempFace.Weight3 = TempPoint.weightHeader;
                            TempFace.MorphPoint3 = TempPoint.MorphPoints;
                            TempFace.TangentNormal3 = new Vector3(TempPoint.Tangent.X, TempPoint.Tangent.Y, TempPoint.Tangent.Z);

                            TempFace.MaterialID = MatID;

                            reassignedMesh.faces.Add(TempFace);
                        }
                    }

                    trickyModelCombiner.reassignedMesh.Add(reassignedMesh);
                }
            }

            return trickyModelCombiner;
        }

        public static TrickyGCModelCombiner LoadTrickyGCGlft(string Path)
        {
            TrickyGCModelCombiner trickyModelCombiner = new TrickyGCModelCombiner();
            trickyModelCombiner.materials = new List<TrickyGCMNF.MaterialData>();
            trickyModelCombiner.reassignedMesh = new List<TrickyGCModelCombiner.ReassignedMesh>();
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
            List<TrickyGCMNF.BoneData> boneDatas = new List<TrickyGCMNF.BoneData>();
            var Ampatures = Scene.FindArmatures();

            for (int a = 0; a < Ampatures[0].VisualChildren.Count; a++)
            {
                if (Ampatures[0].VisualChildren[a].Name.ToLower() == "hips")
                {
                    var StartBone = Ampatures[0].VisualChildren[a];
                    boneDatas = ReturnBoneAndChildrenTrickyGC(StartBone, true);
                }
                else if (Ampatures[0].VisualChildren[a].Name.ToLower() == "board")
                {
                    var StartBone = Ampatures[0].VisualChildren[a];
                    boneDatas = ReturnBoneAndChildrenTrickyGC(StartBone, true);
                }
            }

            trickyModelCombiner.bones = boneDatas;

            for (int i = 0; i < Instances.Length; i++)
            {
                if (Instances[i].Content.GetType().Name == "SkinnedTransformer")
                {
                    var GLFTMesh = Instances[i].Content.GetGeometryAsset();
                    var SkinnedMesh = (SharpGLTF.Scenes.SkinnedTransformer)Instances[i].Content;
                    TrickyGCModelCombiner.ReassignedMesh reassignedMesh = new TrickyGCModelCombiner.ReassignedMesh();
                    reassignedMesh.IKPoints = IKPoints;
                    reassignedMesh.MeshName = GLFTMesh.Name;
                    reassignedMesh.faces = new List<TrickyGCMNF.Face>();

                    var JointBindings = SkinnedMesh.GetJointBindings();
                    var MaterialArray = GLFTMesh.Primitives.ToArray();
                    for (int a = 0; a < MaterialArray.Length; a++)
                    {
                        //Build New Material With Primitive Name
                        trickyModelCombiner.materials = MakeNewMaterialTrickyGC(trickyModelCombiner.materials, MaterialArray[a].Material.Name);

                        int MatID = FindMaterialIDTrickyGC(trickyModelCombiner.materials, MaterialArray[a].Material.Name);

                        //Build Vertex List
                        List<VertexDataTrickyGC> VertexList = new List<VertexDataTrickyGC>();
                        var OldVertexList = MaterialArray[a].Vertices.ToArray();
                        int MorphTargetsCount = MaterialArray[a].MorphTargets.Count;

                        for (int b = 0; b < OldVertexList.Length; b++)
                        {
                            var NewVertex = new VertexDataTrickyGC();

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
                            else
                            {
                                MessageBox.Show("Must Contain Normals");
                                return null;
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
                            NewVertex.weightHeader = new TrickyGCMNF.BoneWeightHeader();
                            NewVertex.weightHeader.Unknown2 = 19;
                            NewVertex.weightHeader.boneWeights = new List<TrickyGCMNF.BoneWeight>();

                            for (int d = 0; d < TempSkinning.MaxBindings; d++)
                            {
                                var BindingList = TempSkinning.GetBinding(d);
                                if (BindingList.Weight != 0)
                                {
                                    TrickyGCMNF.BoneWeight TempWeight = new TrickyGCMNF.BoneWeight();
                                    TempWeight.BoneID = BindingList.Index;
                                    TempWeight.Weight = (int)(BindingList.Weight * 100f);
                                    TempWeight.BoneName = JointBindings[BindingList.Index].Joint.Name;
                                    NewVertex.weightHeader.boneWeights.Add(TempWeight);
                                }
                            }

                            //Correct Weights
                            int Count = 0;
                            for (int d = 0; d < NewVertex.weightHeader.boneWeights.Count; d++)
                            {
                                Count += NewVertex.weightHeader.boneWeights[d].Weight;
                            }

                            if (Count != 100)
                            {
                                var TempWeight = NewVertex.weightHeader.boneWeights[0];
                                if (Count > 100)
                                {
                                    TempWeight.Weight -= Count - 100;
                                }
                                else if (Count < 100)
                                {
                                    TempWeight.Weight += 100 - Count;
                                }
                                NewVertex.weightHeader.boneWeights[0] = TempWeight;
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
                            TrickyGCMNF.Face TempFace = new TrickyGCMNF.Face();
                            var FaceTri = TriangleList[b];

                            var TempPoint = VertexList[FaceTri.A];

                            TempFace.V1 = TempPoint.Position;
                            TempFace.Normal1 = TempPoint.Normal;
                            TempFace.UV1 = TempPoint.UV;
                            TempFace.Weight1 = TempPoint.weightHeader;
                            TempFace.MorphPoint1 = TempPoint.MorphPoints;

                            TempPoint = VertexList[FaceTri.B];

                            TempFace.V2 = TempPoint.Position;
                            TempFace.Normal2 = TempPoint.Normal;
                            TempFace.UV2 = TempPoint.UV;
                            TempFace.Weight2 = TempPoint.weightHeader;
                            TempFace.MorphPoint2 = TempPoint.MorphPoints;

                            TempPoint = VertexList[FaceTri.C];

                            TempFace.V3 = TempPoint.Position;
                            TempFace.Normal3 = TempPoint.Normal;
                            TempFace.UV3 = TempPoint.UV;
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

        public static SSX3PS2ModelCombiner LoadSSX3Glft(string Path)
        {
            SSX3PS2ModelCombiner trickyModelCombiner = new SSX3PS2ModelCombiner();
            trickyModelCombiner.materials = new List<SSX3PS2MPF.MaterialData>();
            trickyModelCombiner.reassignedMesh = new List<SSX3PS2ModelCombiner.ReassignedMesh>();
            var Scene = SharpGLTF.Scenes.SceneBuilder.LoadDefaultScene(Path);
            var Instances = Scene.Instances.ToArray();

            //Read Bones Building List
            List<SSX3PS2MPF.BoneData> boneDatas = new List<SSX3PS2MPF.BoneData>();
            var Ampatures = Scene.FindArmatures();

            bool FoundBone = false;
            for (int z = 0; z < Ampatures.Count; z++)
            {
                for (int a = 0; a < Ampatures[z].VisualChildren.Count; a++)
                {
                    if (Ampatures[z].VisualChildren[a].Name.ToLower() == "hips" || Ampatures[z].VisualChildren[a].Name.ToLower() == "board_rootg")
                    {
                        var StartBone = Ampatures[z].VisualChildren[a];
                        boneDatas = ReturnBoneAndChildrenSSX3(StartBone, true);
                        FoundBone = true;
                        break;
                    }
                }

                if(FoundBone)
                {
                    break;
                }
            }

            trickyModelCombiner.boneDatas = boneDatas;

            for (int i = 0; i < Instances.Length; i++)
            {
                if (Instances[i].Content.GetType().Name == "SkinnedTransformer")
                {
                    var GLFTMesh = Instances[i].Content.GetGeometryAsset();
                    var SkinnedMesh = (SharpGLTF.Scenes.SkinnedTransformer)Instances[i].Content;
                    SSX3PS2ModelCombiner.ReassignedMesh reassignedMesh = new SSX3PS2ModelCombiner.ReassignedMesh();
                    reassignedMesh.MeshName = GLFTMesh.Name;
                    reassignedMesh.faces = new List<SSX3PS2MPF.Face>();

                    var JointBindings = SkinnedMesh.GetJointBindings();
                    var MaterialArray = GLFTMesh.Primitives.ToArray();
                    for (int a = 0; a < MaterialArray.Length; a++)
                    {
                        //Build New Material With Primitive Name
                        trickyModelCombiner.materials = MakeNewMaterialSSX3(trickyModelCombiner.materials, MaterialArray[a].Material.Name);

                        int MatID = FindMaterialIDSSX3(trickyModelCombiner.materials, MaterialArray[a].Material.Name);

                        //Build Vertex List
                        List<VertexDataSSX3> VertexList = new List<VertexDataSSX3>();
                        var OldVertexList = MaterialArray[a].Vertices.ToArray();
                        int MorphTargetsCount = MaterialArray[a].MorphTargets.Count;

                        for (int b = 0; b < OldVertexList.Length; b++)
                        {
                            var NewVertex = new VertexDataSSX3();

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
                            NewVertex.weightHeader = new SSX3PS2MPF.BoneWeightHeader();
                            NewVertex.weightHeader.Unknown = 0;
                            NewVertex.weightHeader.BoneWeightList = new List<SSX3PS2MPF.BoneWeight>();

                            for (int d = 0; d < TempSkinning.MaxBindings; d++)
                            {
                                var BindingList = TempSkinning.GetBinding(d);
                                if (BindingList.Weight != 0)
                                {
                                    SSX3PS2MPF.BoneWeight TempWeight = new SSX3PS2MPF.BoneWeight();
                                    TempWeight.BoneID = BindingList.Index;
                                    TempWeight.Weight = (int)(Math.Round((BindingList.Weight * 100f) / 5f) * 5f);
                                    TempWeight.boneName = JointBindings[BindingList.Index].Joint.Name;
                                    NewVertex.weightHeader.BoneWeightList.Add(TempWeight);
                                }
                            }

                            //Correct Weights
                            int Count = 0;
                            for (int d = 0; d < NewVertex.weightHeader.BoneWeightList.Count; d++)
                            {
                                Count += NewVertex.weightHeader.BoneWeightList[d].Weight;
                            }

                            if (Count != 100)
                            {
                                var TempWeight = NewVertex.weightHeader.BoneWeightList[0];
                                if (Count > 100)
                                {
                                    TempWeight.Weight -= Count - 100;
                                }
                                else if (Count < 100)
                                {
                                    TempWeight.Weight += 100 - Count;
                                }
                                NewVertex.weightHeader.BoneWeightList[0] = TempWeight;
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
                            SSX3PS2MPF.Face TempFace = new SSX3PS2MPF.Face();
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

        public static SSXOnTourPS2ModelCombiner LoadSSXOnTourGlft(string Path)
        {
            SSXOnTourPS2ModelCombiner trickyModelCombiner = new SSXOnTourPS2ModelCombiner();
            trickyModelCombiner.materials = new List<SSXOnTourMPF.MaterialData>();
            trickyModelCombiner.reassignedMesh = new List<SSXOnTourPS2ModelCombiner.ReassignedMesh>();
            var Scene = SharpGLTF.Scenes.SceneBuilder.LoadDefaultScene(Path);
            var Instances = Scene.Instances.ToArray();

            //Read Bones Building List
            List<SSXOnTourMPF.BoneData> boneDatas = new List<SSXOnTourMPF.BoneData>();
            var Ampatures = Scene.FindArmatures();

            bool FoundBone = false;
            for (int z = 0; z < Ampatures.Count; z++)
            {
                for (int a = 0; a < Ampatures[z].VisualChildren.Count; a++)
                {
                    if (Ampatures[z].VisualChildren[a].Name.ToLower() == "hips" || Ampatures[z].VisualChildren[a].Name.ToLower() == "hipsl" || Ampatures[z].VisualChildren[a].Name.ToLower() == "board_childg")
                    {
                        var StartBone = Ampatures[z].VisualChildren[a];
                        boneDatas = ReturnBoneAndChildrenSSXOnTour(StartBone, true);
                        FoundBone = true;
                        break;
                    }
                }

                if (FoundBone)
                {
                    break;
                }
            }

            trickyModelCombiner.boneDatas = boneDatas;

            for (int i = 0; i < Instances.Length; i++)
            {
                if (Instances[i].Content.GetType().Name == "SkinnedTransformer")
                {
                    var GLFTMesh = Instances[i].Content.GetGeometryAsset();
                    var SkinnedMesh = (SharpGLTF.Scenes.SkinnedTransformer)Instances[i].Content;
                    SSXOnTourPS2ModelCombiner.ReassignedMesh reassignedMesh = new SSXOnTourPS2ModelCombiner.ReassignedMesh();
                    reassignedMesh.MeshName = GLFTMesh.Name;
                    reassignedMesh.faces = new List<SSXOnTourMPF.Face>();

                    var JointBindings = SkinnedMesh.GetJointBindings();
                    var MaterialArray = GLFTMesh.Primitives.ToArray();
                    for (int a = 0; a < MaterialArray.Length; a++)
                    {
                        //Build New Material With Primitive Name
                        trickyModelCombiner.materials = MakeNewMaterialSSXOnTour(trickyModelCombiner.materials, MaterialArray[a].Material.Name);

                        int MatID = FindMaterialIDSSXOnTour(trickyModelCombiner.materials, MaterialArray[a].Material.Name);

                        //Build Vertex List
                        List<VertexDataSSXOnTour> VertexList = new List<VertexDataSSXOnTour>();
                        var OldVertexList = MaterialArray[a].Vertices.ToArray();
                        int MorphTargetsCount = MaterialArray[a].MorphTargets.Count;

                        for (int b = 0; b < OldVertexList.Length; b++)
                        {
                            var NewVertex = new VertexDataSSXOnTour();

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
                            NewVertex.weightHeader = new SSXOnTourMPF.BoneWeightHeader();
                            NewVertex.weightHeader.Unknown = 0;
                            NewVertex.weightHeader.BoneWeightList = new List<SSXOnTourMPF.BoneWeight>();

                            for (int d = 0; d < TempSkinning.MaxBindings; d++)
                            {
                                var BindingList = TempSkinning.GetBinding(d);
                                if (BindingList.Weight != 0)
                                {
                                    SSXOnTourMPF.BoneWeight TempWeight = new SSXOnTourMPF.BoneWeight();
                                    TempWeight.BoneID = BindingList.Index;
                                    TempWeight.Weight = (int)(BindingList.Weight * 100f);
                                    TempWeight.boneName = JointBindings[BindingList.Index].Joint.Name;
                                    NewVertex.weightHeader.BoneWeightList.Add(TempWeight);
                                }
                            }

                            //Correct Weights
                            int Count = 0;
                            for (int d = 0; d < NewVertex.weightHeader.BoneWeightList.Count; d++)
                            {
                                Count += NewVertex.weightHeader.BoneWeightList[d].Weight;
                            }

                            if (Count != 100)
                            {
                                var TempWeight = NewVertex.weightHeader.BoneWeightList[0];
                                if (Count > 100)
                                {
                                    TempWeight.Weight -= Count - 100;
                                }
                                else if (Count < 100)
                                {
                                    TempWeight.Weight += 100 - Count;
                                }
                                NewVertex.weightHeader.BoneWeightList[0] = TempWeight;
                            }

                            //Add Morph Data
                            NewVertex.MorphPoints = new List<Vector3>();
                            NewVertex.NormalPoints = new List<Vector3>();
                            for (int d = 0; d < MorphTargetsCount; d++)
                            {
                                NewVertex.MorphPoints.Add(MaterialArray[a].MorphTargets[d].GetVertex(b).GetGeometry().GetPosition() - NewVertex.Position);
                                Vector3 Normal;
                                if (MaterialArray[a].MorphTargets[d].GetVertex(b).GetGeometry().TryGetNormal(out Normal))
                                {
                                    NewVertex.NormalPoints.Add(Normal - NewVertex.Normal);
                                }
                            }

                            VertexList.Add(NewVertex);
                        }

                        //Build Faces
                        var TriangleList = MaterialArray[a].Triangles;
                        for (int b = 0; b < TriangleList.Count; b++)
                        {
                            SSXOnTourMPF.Face TempFace = new SSXOnTourMPF.Face();
                            var FaceTri = TriangleList[b];

                            var TempPoint = VertexList[FaceTri.A];

                            TempFace.V1 = TempPoint.Position;
                            TempFace.Normal1 = TempPoint.Normal;
                            TempFace.UV1 = new Vector4(TempPoint.UV, 0, 0);
                            TempFace.Weight1 = TempPoint.weightHeader;
                            TempFace.MorphPoint1 = TempPoint.MorphPoints;
                            TempFace.AltMorphNormal1 = TempPoint.NormalPoints;

                            TempPoint = VertexList[FaceTri.B];

                            TempFace.V2 = TempPoint.Position;
                            TempFace.Normal2 = TempPoint.Normal;
                            TempFace.UV2 = new Vector4(TempPoint.UV, 0, 0);
                            TempFace.Weight2 = TempPoint.weightHeader;
                            TempFace.MorphPoint2 = TempPoint.MorphPoints;
                            TempFace.AltMorphNormal2 = TempPoint.NormalPoints;

                            TempPoint = VertexList[FaceTri.C];

                            TempFace.V3 = TempPoint.Position;
                            TempFace.Normal3 = TempPoint.Normal;
                            TempFace.UV3 = new Vector4(TempPoint.UV, 0, 0);
                            TempFace.Weight3 = TempPoint.weightHeader;
                            TempFace.MorphPoint3 = TempPoint.MorphPoints;
                            TempFace.AltMorphNormal3 = TempPoint.NormalPoints;

                            TempFace.MaterialID = MatID;

                            reassignedMesh.faces.Add(TempFace);
                        }
                    }

                    trickyModelCombiner.reassignedMesh.Add(reassignedMesh);
                }
            }

            return trickyModelCombiner;
        }

        static List<TrickyPS2MPF.MaterialData> MakeNewMaterialTrickyPS2(List<TrickyPS2MPF.MaterialData> materials, string name)
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
                TrickyPS2MPF.MaterialData newmat = new TrickyPS2MPF.MaterialData();
                newmat.MainTexture = name;
                materials.Add(newmat);
            }

            return materials;
        }

        static List<TrickyXboxMXF.MaterialData> MakeNewMaterialTrickyXbox(List<TrickyXboxMXF.MaterialData> materials, string name)
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
            if (!test)
            {
                TrickyXboxMXF.MaterialData newmat = new TrickyXboxMXF.MaterialData();
                newmat.MainTexture = name;
                materials.Add(newmat);
            }

            return materials;
        }

        static List<TrickyGCMNF.MaterialData> MakeNewMaterialTrickyGC(List<TrickyGCMNF.MaterialData> materials, string name)
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
            if (!test)
            {
                TrickyGCMNF.MaterialData newmat = new TrickyGCMNF.MaterialData();
                newmat.MainTexture = name;
                materials.Add(newmat);
            }

            return materials;
        }

        static List<SSX3PS2MPF.MaterialData> MakeNewMaterialSSX3(List<SSX3PS2MPF.MaterialData> materials, string name)
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
            if (!test)
            {
                SSX3PS2MPF.MaterialData newmat = new SSX3PS2MPF.MaterialData();
                newmat.MainTexture = name;
                materials.Add(newmat);
            }

            return materials;
        }

        static List<SSXOnTourMPF.MaterialData> MakeNewMaterialSSXOnTour(List<SSXOnTourMPF.MaterialData> materials, string name)
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
            if (!test)
            {
                SSXOnTourMPF.MaterialData newmat = new SSXOnTourMPF.MaterialData();
                newmat.MainTexture = name;
                materials.Add(newmat);
            }

            return materials;
        }

        static int FindMaterialIDTrickyPS2(List<TrickyPS2MPF.MaterialData> materials, string name)
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

        static int FindMaterialIDTrickyXbox(List<TrickyXboxMXF.MaterialData> materials, string name)
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

        static int FindMaterialIDTrickyGC(List<TrickyGCMNF.MaterialData> materials, string name)
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

        static int FindMaterialIDSSX3(List<SSX3PS2MPF.MaterialData> materials, string name)
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

        static int FindMaterialIDSSXOnTour(List<SSXOnTourMPF.MaterialData> materials, string name)
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

        struct VertexDataTrickyPS2
        {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector2 UV;

            public TrickyPS2MPF.BoneWeightHeader weightHeader;
            public List<Vector3> MorphPoints;
        }

        struct VertexDataTrickyXbox
        {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector2 UV;
            public Vector4 Tangent;

            public TrickyXboxMXF.BoneWeightHeader weightHeader;
            public List<Vector3> MorphPoints;
        }

        struct VertexDataTrickyGC
        {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector2 UV;

            public TrickyGCMNF.BoneWeightHeader weightHeader;
            public List<Vector3> MorphPoints;
        }

        struct VertexDataSSX3
        {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector2 UV;

            public SSX3PS2MPF.BoneWeightHeader weightHeader;
            public List<Vector3> MorphPoints;
        }

        struct VertexDataSSXOnTour
        {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector2 UV;

            public SSXOnTourMPF.BoneWeightHeader weightHeader;
            public List<Vector3> MorphPoints;
            public List<Vector3> NormalPoints;
        }

        public static List<TrickyPS2MPF.BoneData> ReturnBoneAndChildrenTrickyPS2(SharpGLTF.Scenes.NodeBuilder nodeBuilder, bool startBone)
        {
            List<TrickyPS2MPF.BoneData> boneDatas = new List<TrickyPS2MPF.BoneData>();
            var TempBoneData = new TrickyPS2MPF.BoneData();

            TempBoneData.BoneName = nodeBuilder.Name;
            TempBoneData.Position = nodeBuilder.LocalTransform.Translation;
            if (nodeBuilder.Parent != null)
            {
                TempBoneData.parentName = nodeBuilder.Parent.Name;
            }
            Quaternion quaternion = nodeBuilder.LocalTransform.GetDecomposed().Rotation;
            Vector3 radians = MathUtil.ToEulerAngles(quaternion);
            TempBoneData.Radians = radians;

            boneDatas.Add(TempBoneData);

            for (int i = 0; i < nodeBuilder.VisualChildren.Count; i++)
            {
                boneDatas.AddRange(ReturnBoneAndChildrenTrickyPS2(nodeBuilder.VisualChildren[i], false));
            }

            return boneDatas;
        }

        public static List<TrickyXboxMXF.BoneData> ReturnBoneAndChildrenTrickyXbox(SharpGLTF.Scenes.NodeBuilder nodeBuilder, bool startBone)
        {
            List<TrickyXboxMXF.BoneData> boneDatas = new List<TrickyXboxMXF.BoneData>();
            var TempBoneData = new TrickyXboxMXF.BoneData();

            TempBoneData.BoneName = nodeBuilder.Name;
            TempBoneData.Position = nodeBuilder.LocalTransform.Translation;
            if (nodeBuilder.Parent != null)
            {
                TempBoneData.parentName = nodeBuilder.Parent.Name;
            }
            Quaternion quaternion = nodeBuilder.LocalTransform.GetDecomposed().Rotation;
            Vector3 radians = MathUtil.ToEulerAngles(quaternion);
            TempBoneData.Radians = radians;

            boneDatas.Add(TempBoneData);

            for (int i = 0; i < nodeBuilder.VisualChildren.Count; i++)
            {
                boneDatas.AddRange(ReturnBoneAndChildrenTrickyXbox(nodeBuilder.VisualChildren[i], false));
            }

            return boneDatas;
        }

        public static List<TrickyGCMNF.BoneData> ReturnBoneAndChildrenTrickyGC(SharpGLTF.Scenes.NodeBuilder nodeBuilder, bool startBone)
        {
            List<TrickyGCMNF.BoneData> boneDatas = new List<TrickyGCMNF.BoneData>();
            var TempBoneData = new TrickyGCMNF.BoneData();

            TempBoneData.BoneName = nodeBuilder.Name;
            TempBoneData.Position = nodeBuilder.LocalTransform.Translation;
            if (nodeBuilder.Parent != null)
            {
                TempBoneData.parentName = nodeBuilder.Parent.Name;
            }
            Quaternion quaternion = nodeBuilder.LocalTransform.GetDecomposed().Rotation;
            Vector3 radians = MathUtil.ToEulerAngles(quaternion);
            TempBoneData.Radians = radians;

            boneDatas.Add(TempBoneData);

            for (int i = 0; i < nodeBuilder.VisualChildren.Count; i++)
            {
                boneDatas.AddRange(ReturnBoneAndChildrenTrickyGC(nodeBuilder.VisualChildren[i], false));
            }

            return boneDatas;
        }

        public static List<SSX3PS2MPF.BoneData> ReturnBoneAndChildrenSSX3(SharpGLTF.Scenes.NodeBuilder nodeBuilder, bool startBone)
        {
            List<SSX3PS2MPF.BoneData> boneDatas = new List<SSX3PS2MPF.BoneData>();
            var TempBoneData = new SSX3PS2MPF.BoneData();

            TempBoneData.BoneName = nodeBuilder.Name;
            Vector4 TempVector = new Vector4(nodeBuilder.LocalTransform.Translation.X, nodeBuilder.LocalTransform.Translation.Y, nodeBuilder.LocalTransform.Translation.Z, 1);
            TempBoneData.Position = TempVector;
            if (nodeBuilder.Parent != null)
            {
                TempBoneData.parentName = nodeBuilder.Parent.Name;
            }
            TempBoneData.Rotation = nodeBuilder.LocalTransform.GetDecomposed().Rotation;

            TempBoneData.WorldMatrix = nodeBuilder.WorldMatrix;

            boneDatas.Add(TempBoneData);

            for (int i = 0; i < nodeBuilder.VisualChildren.Count; i++)
            {
                boneDatas.AddRange(ReturnBoneAndChildrenSSX3(nodeBuilder.VisualChildren[i], false));
            }

            return boneDatas;
        }

        public static List<SSXOnTourMPF.BoneData> ReturnBoneAndChildrenSSXOnTour(SharpGLTF.Scenes.NodeBuilder nodeBuilder, bool startBone)
        {
            List<SSXOnTourMPF.BoneData> boneDatas = new List<SSXOnTourMPF.BoneData>();
            var TempBoneData = new SSXOnTourMPF.BoneData();

            TempBoneData.BoneName = nodeBuilder.Name;
            Vector4 TempVector = new Vector4(nodeBuilder.LocalTransform.Translation.X, nodeBuilder.LocalTransform.Translation.Y, nodeBuilder.LocalTransform.Translation.Z, 1);
            TempBoneData.Position = TempVector;
            if (nodeBuilder.Parent != null)
            {
                TempBoneData.parentName = nodeBuilder.Parent.Name;
            }
            TempBoneData.Rotation = nodeBuilder.LocalTransform.GetDecomposed().Rotation;

            TempBoneData.WorldMatrix = nodeBuilder.WorldMatrix;

            boneDatas.Add(TempBoneData);

            for (int i = 0; i < nodeBuilder.VisualChildren.Count; i++)
            {
                boneDatas.AddRange(ReturnBoneAndChildrenSSXOnTour(nodeBuilder.VisualChildren[i], false));
            }

            return boneDatas;
        }

        public static PointMorph GeneratePointMorph(Vector3 Point, List<Vector3> MorphPoints, List<Vector3> AltMorphPoint = null, List<Vector3> AltMorphNormal = null)
        {
            PointMorph pointMorph = new PointMorph();
            pointMorph.Point = Point;
            pointMorph.MorphPoints = MorphPoints;

            pointMorph.AltMorphPoints = AltMorphPoint;
            pointMorph.AltMorphNormal = AltMorphNormal;

            return pointMorph;
        }

        public struct PointMorph
        {
            public Vector3 Point;
            public List<Vector3> MorphPoints;

            public List<Vector3> AltMorphPoints;
            public List<Vector3> AltMorphNormal;
        }
    }
}
