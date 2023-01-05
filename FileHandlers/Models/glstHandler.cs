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

    public class glstHandler
    {
        public static void SaveOGglTF(string Output, SSXMPFModelHandler.MPFModelHeader modelHeader)
        {
            var mesh = new MeshBuilder<VertexPositionNormal, VertexTexture1, VertexEmpty>(modelHeader.FileName);
            //Make Materials

            List<MaterialBuilder> materialBuilders = new List<MaterialBuilder>();

            for (int i = 0; i < modelHeader.materialDataList.Count; i++)
            {
                var TempVar = modelHeader.materialDataList[i];
                var material1 = new MaterialBuilder(TempVar.Name)
                .WithChannelParam(KnownChannel.BaseColor, KnownProperty.RGBA, new Vector4(TempVar.X, TempVar.Y, TempVar.Z, 1));
                materialBuilders.Add(material1);
            }


            for (int i = 0; i < modelHeader.staticMesh.Count; i++)
            {
                var Data = modelHeader.staticMesh[i];
                int tempInt = Data.ChunkID;
                int MatId = modelHeader.chunks[tempInt].MaterialID;

                for (int b = 0; b < Data.faces.Count; b++)
                {
                    var Face = Data.faces[b];
                    VertexPositionNormal TempPos1 = new VertexPositionNormal();
                    TempPos1.Position.X = Face.V1.X;
                    TempPos1.Position.Y = Face.V1.Y;
                    TempPos1.Position.Z = Face.V1.Z;

                    TempPos1.Normal.X = (float)Face.Normal1.X / 4096f;
                    TempPos1.Normal.Y = (float)Face.Normal1.Y / 4096f;
                    TempPos1.Normal.Z = (float)Face.Normal1.Z / 4096f;

                    VertexPositionNormal TempPos2 = new VertexPositionNormal();
                    TempPos2.Position.X = Face.V2.X;
                    TempPos2.Position.Y = Face.V2.Y;
                    TempPos2.Position.Z = Face.V2.Z;

                    TempPos2.Normal.X = (float)Face.Normal2.X / 4096f;
                    TempPos2.Normal.Y = (float)Face.Normal2.Y / 4096f;
                    TempPos2.Normal.Z = (float)Face.Normal2.Z / 4096f;

                    VertexPositionNormal TempPos3 = new VertexPositionNormal();
                    TempPos3.Position.X = Face.V3.X;
                    TempPos3.Position.Y = Face.V3.Y;
                    TempPos3.Position.Z = Face.V3.Z;

                    TempPos3.Normal.X = (float)Face.Normal3.X / 4096f;
                    TempPos3.Normal.Y = (float)Face.Normal3.Y / 4096f;
                    TempPos3.Normal.Z = (float)Face.Normal3.Z / 4096f;

                    VertexTexture1 TempTexture1 = new VertexTexture1();
                    TempTexture1.TexCoord.X = (float)Face.UV1.X / 4096f;
                    TempTexture1.TexCoord.Y = (float)Face.UV1.Y / 4096f;

                    VertexTexture1 TempTexture2 = new VertexTexture1();
                    TempTexture2.TexCoord.X = (float)Face.UV2.X / 4096f;
                    TempTexture2.TexCoord.Y = (float)Face.UV2.Y / 4096f;

                    VertexTexture1 TempTexture3 = new VertexTexture1();
                    TempTexture3.TexCoord.X = (float)Face.UV3.X / 4096f;
                    TempTexture3.TexCoord.Y = (float)Face.UV3.Y / 4096f;

                    mesh.UsePrimitive(materialBuilders[MatId]).AddTriangle((TempPos1, TempTexture1), (TempPos2, TempTexture2), (TempPos3, TempTexture3));
                }

            }

            var scene = new SharpGLTF.Scenes.SceneBuilder();

            scene.AddRigidMesh(mesh, Matrix4x4.Identity);

            // save the model in different formats

            var model = scene.ToGltf2();
            model.SaveGLTF(Output);
        }

        public static void SaveTrickyglTF(string Output, TrickyModelCombiner Handler)
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
                float tempX = Handler.bones[i].XRadian;
                float tempY = Handler.bones[i].YRadian;
                float tempZ = Handler.bones[i].ZRadian;

                Binding.WithLocalRotation(ToQuaternion(new Vector3(-tempX, -tempY, -tempZ)));
                Binding.WithLocalTranslation(new Vector3(Handler.bones[i].XLocation, Handler.bones[i].YLocation, Handler.bones[i].ZLocation));

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
                        TempPos1.Position.X = Face.V1.X;
                        TempPos1.Position.Y = Face.V1.Y;
                        TempPos1.Position.Z = Face.V1.Z;

                        TempPos1.Normal.X = (float)Face.Normal1.X;
                        TempPos1.Normal.Y = (float)Face.Normal1.Y;
                        TempPos1.Normal.Z = (float)Face.Normal1.Z;

                        VertexPositionNormal TempPos2 = new VertexPositionNormal();
                        TempPos2.Position.X = Face.V2.X;
                        TempPos2.Position.Y = Face.V2.Y;
                        TempPos2.Position.Z = Face.V2.Z;

                        TempPos2.Normal.X = (float)Face.Normal2.X;
                        TempPos2.Normal.Y = (float)Face.Normal2.Y;
                        TempPos2.Normal.Z = (float)Face.Normal2.Z;

                        VertexPositionNormal TempPos3 = new VertexPositionNormal();
                        TempPos3.Position.X = Face.V3.X;
                        TempPos3.Position.Y = Face.V3.Y;
                        TempPos3.Position.Z = Face.V3.Z;

                        TempPos3.Normal.X = (float)Face.Normal3.X;
                        TempPos3.Normal.Y = (float)Face.Normal3.Y;
                        TempPos3.Normal.Z = (float)Face.Normal3.Z;

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
                        TempPos1.Position.X = Face.V1.X;
                        TempPos1.Position.Y = Face.V1.Y;
                        TempPos1.Position.Z = Face.V1.Z;

                        VertexPosition TempPos2 = new VertexPosition();
                        TempPos2.Position.X = Face.V2.X;
                        TempPos2.Position.Y = Face.V2.Y;
                        TempPos2.Position.Z = Face.V2.Z;

                        VertexPosition TempPos3 = new VertexPosition();
                        TempPos3.Position.X = Face.V3.X;
                        TempPos3.Position.Y = Face.V3.Y;
                        TempPos3.Position.Z = Face.V3.Z;

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
                        scene.AddNode(Temp);
                    }
                }
            }

            // save the model in different formats

            var model = scene.ToGltf2();
            model.SaveGLB(Output);
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

        public static void LoadGlft(string Path)
        {
            var model = ModelRoot.Load(Path);
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
