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

        public static void SaveTrickyglTF(string Output, TrickyModelCombiner.ReassignedMesh Handler)
        {
            var scene = new SharpGLTF.Scenes.SceneBuilder();
            //VertexJoints4
            var mesh = new MeshBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4>("Character3000");
            //var bindings = new List<Node>();
            List<MaterialBuilder> materialBuilders = new List<MaterialBuilder>();
            for (int i = 0; i < Handler.materials.Count; i++)
            {
                var TempVar = Handler.materials[i];
                var material1 = new MaterialBuilder(TempVar.MainTexture)
                .WithChannelParam(KnownChannel.BaseColor, KnownProperty.RGBA, new Vector4(TempVar.R, TempVar.G, TempVar.B, 1));
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

            for (int b = 0; b < Handler.faces.Count; b++)
            {
                var Face = Handler.faces[b];
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
            }
            scene.AddSkinnedMesh(mesh, Matrix4x4.CreateTranslation(0, 0, 0), bindings.ToArray());

            // save the model in different formats

            var model = scene.ToGltf2();
            model.SaveGLB(Output);
        }


        //public static void SaveTrickyglTF(string Output, TrickyMPFModelHandler Handler)
        //{
        //    var scene = new SharpGLTF.Scenes.SceneBuilder();
        //    for (int ax = 0; ax < 1; ax++)
        //    {
        //        var modelHeader = Handler.ModelList[ax];
        //        //VertexJoints4
        //        var mesh = new MeshBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4>(modelHeader.FileName);
        //        //var bindings = new List<Node>();
        //        List<MaterialBuilder> materialBuilders = new List<MaterialBuilder>();
        //        for (int i = 0; i < modelHeader.materialDatas.Count; i++)
        //        {
        //            var TempVar = modelHeader.materialDatas[i];
        //            var material1 = new MaterialBuilder(TempVar.MainTexture)
        //            .WithChannelParam(KnownChannel.BaseColor, KnownProperty.RGBA, new Vector4(TempVar.R, TempVar.G, TempVar.B, 1));
        //            materialBuilders.Add(material1);
        //        }
        //        var bindings = new List<SharpGLTF.Scenes.NodeBuilder>();
        //        SharpGLTF.Scenes.NodeBuilder Binding = new SharpGLTF.Scenes.NodeBuilder();
        //        for (int i = 0; i < modelHeader.boneDatas.Count; i++)
        //        {
        //            if(modelHeader.boneDatas[i].ParentBone==-1)
        //            {
        //                Binding = new SharpGLTF.Scenes.NodeBuilder();
        //            }
        //            else
        //            {
        //                Binding = bindings[modelHeader.boneDatas[i].ParentBone];
        //            }
        //            Binding = Binding.CreateNode(modelHeader.boneDatas[i].BoneName);
        //            float tempX = modelHeader.boneDatas[i].XRadian;
        //            float tempY = modelHeader.boneDatas[i].YRadian;
        //            float tempZ = modelHeader.boneDatas[i].ZRadian;

        //            Binding.WithLocalRotation(ToQuaternion(new Vector3(-tempX, -tempY, -tempZ)));
        //            Binding.WithLocalTranslation(new Vector3(modelHeader.boneDatas[i].XLocation, modelHeader.boneDatas[i].YLocation, modelHeader.boneDatas[i].ZLocation));

        //            Binding.LocalMatrix = Binding.LocalMatrix;


        //            bindings.Add(Binding);
        //        }
        //        for (int a = 0; a < modelHeader.MeshGroups.Count; a++)
        //        {
        //            for (int ab = 0; ab < modelHeader.MeshGroups[a].meshGroupSubs.Count; ab++)
        //            {
        //                for (int ac = 0; ac < modelHeader.MeshGroups[a].meshGroupSubs[ab].MeshGroupHeaders.Count; ac++)
        //                {
        //                    for (int i = 0; i < modelHeader.MeshGroups[a].meshGroupSubs[ab].MeshGroupHeaders[ac].staticMesh.Count; i++)
        //                    {
        //                        var Data = modelHeader.MeshGroups[a].meshGroupSubs[ab].MeshGroupHeaders[ac].staticMesh[i];
        //                        int MatId = modelHeader.MeshGroups[a].MaterialID;
        //                        int WeightRefListID = modelHeader.MeshGroups[a].meshGroupSubs[ab].MeshGroupHeaders[ac].WeightRefGroup;

        //                        for (int b = 0; b < Data.faces.Count; b++)
        //                        {
        //                            var Face = Data.faces[b];
        //                            VertexPositionNormal TempPos1 = new VertexPositionNormal();
        //                            TempPos1.Position.X = Face.V1.X;
        //                            TempPos1.Position.Y = Face.V1.Y;
        //                            TempPos1.Position.Z = Face.V1.Z;

        //                            TempPos1.Normal.X = (float)Face.Normal1.X;
        //                            TempPos1.Normal.Y = (float)Face.Normal1.Y;
        //                            TempPos1.Normal.Z = (float)Face.Normal1.Z;

        //                            VertexPositionNormal TempPos2 = new VertexPositionNormal();
        //                            TempPos2.Position.X = Face.V2.X;
        //                            TempPos2.Position.Y = Face.V2.Y;
        //                            TempPos2.Position.Z = Face.V2.Z;

        //                            TempPos2.Normal.X = (float)Face.Normal2.X;
        //                            TempPos2.Normal.Y = (float)Face.Normal2.Y;
        //                            TempPos2.Normal.Z = (float)Face.Normal2.Z;

        //                            VertexPositionNormal TempPos3 = new VertexPositionNormal();
        //                            TempPos3.Position.X = Face.V3.X;
        //                            TempPos3.Position.Y = Face.V3.Y;
        //                            TempPos3.Position.Z = Face.V3.Z;

        //                            TempPos3.Normal.X = (float)Face.Normal3.X;
        //                            TempPos3.Normal.Y = (float)Face.Normal3.Y;
        //                            TempPos3.Normal.Z = (float)Face.Normal3.Z;

        //                            VertexTexture1 TempTexture1 = new VertexTexture1();
        //                            TempTexture1.TexCoord.X = (float)Face.UV1.X;
        //                            TempTexture1.TexCoord.Y = (float)Face.UV1.Y;

        //                            VertexTexture1 TempTexture2 = new VertexTexture1();
        //                            TempTexture2.TexCoord.X = (float)Face.UV2.X;
        //                            TempTexture2.TexCoord.Y = (float)Face.UV2.Y;

        //                            VertexTexture1 TempTexture3 = new VertexTexture1();
        //                            TempTexture3.TexCoord.X = (float)Face.UV3.X;
        //                            TempTexture3.TexCoord.Y = (float)Face.UV3.Y;

        //                            var TempList = modelHeader.numberListRefs[WeightRefListID];
        //                            int FindInt = 0;
        //                            int WeightId = 0;
        //                            var TempWeight = new TrickyMPFModelHandler.BoneWeightHeader();
        //                            (int Temp, float TempFloat)[] bindings1 = new (int Temp, float TempFloat)[1];

        //                            VertexJoints4 TempBinding1 = new VertexJoints4();
        //                            FindInt = (int)((Face.UV1.Z - 14) / 4);
        //                            WeightId = TempList.subNumberRefs[FindInt].Unknown;
        //                            TempWeight = modelHeader.boneWeightHeader[WeightId];
        //                            bindings1 = new (int Temp, float TempFloat)[TempWeight.boneWeights.Count];
        //                            for (int ia = 0; ia < TempWeight.boneWeights.Count; ia++)
        //                            {
        //                                bindings1[ia] = (TempWeight.boneWeights[ia].ID, TempWeight.boneWeights[ia].weight);
        //                            }
        //                            TempBinding1.SetBindings(bindings1);

        //                            VertexJoints4 TempBinding2 = new VertexJoints4();
        //                            FindInt = (int)((Face.UV2.Z - 14) / 4);
        //                            WeightId = TempList.subNumberRefs[FindInt].Unknown;
        //                            TempWeight = modelHeader.boneWeightHeader[WeightId];
        //                            bindings1 = new (int Temp, float TempFloat)[TempWeight.boneWeights.Count];
        //                            for (int ia = 0; ia < TempWeight.boneWeights.Count; ia++)
        //                            {
        //                                bindings1[ia] = (TempWeight.boneWeights[ia].ID, TempWeight.boneWeights[ia].weight);
        //                            }
        //                            TempBinding2.SetBindings(bindings1);

        //                            VertexJoints4 TempBinding3 = new VertexJoints4();
        //                            FindInt = (int)((Face.UV3.Z - 14) / 4);
        //                            WeightId = TempList.subNumberRefs[FindInt].Unknown;
        //                            TempWeight = modelHeader.boneWeightHeader[WeightId];
        //                            bindings1 = new (int Temp, float TempFloat)[TempWeight.boneWeights.Count];
        //                            for (int ia = 0; ia < TempWeight.boneWeights.Count; ia++)
        //                            {
        //                                bindings1[ia] = (TempWeight.boneWeights[ia].ID, TempWeight.boneWeights[ia].weight);
        //                            }
        //                            TempBinding3.SetBindings(bindings1);

        //                            mesh.UsePrimitive(materialBuilders[MatId]).AddTriangle((TempPos1, TempTexture1, TempBinding1), (TempPos2, TempTexture2, TempBinding2), (TempPos3, TempTexture3, TempBinding3));
        //                        }

        //                    }
        //                }
        //            }
        //        }
        //        //scene.
        //        scene.AddSkinnedMesh(mesh, Matrix4x4.CreateTranslation(0, 0, 0), bindings.ToArray());
        //        scene.AddSkinnedMesh(mesh, Matrix4x4.CreateTranslation(0, 0, 0), bindings.ToArray());
        //    }

        //    // save the model in different formats

        //    var model = scene.ToGltf2();
        //    model.SaveGLB(Output);
        //}

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
