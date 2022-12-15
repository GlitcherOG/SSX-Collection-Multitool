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

        public static void SaveTrickyglTF(string Output, TrickyMPFModelHandler Handler)
        {
            var scene = new SharpGLTF.Scenes.SceneBuilder();

            for (int ax = 0; ax < Handler.ModelList.Count; ax++)
            {
                var modelHeader = Handler.ModelList[ax];
                //VertexJoints4
                var mesh = new MeshBuilder<VertexPositionNormal, VertexTexture1, VertexEmpty>(modelHeader.FileName);
                //var bindings = new List<Node>();
                List<MaterialBuilder> materialBuilders = new List<MaterialBuilder>();
                for (int i = 0; i < modelHeader.materialDatas.Count; i++)
                {
                    var TempVar = modelHeader.materialDatas[i];
                    var material1 = new MaterialBuilder(TempVar.MainTexture)
                    .WithChannelParam(KnownChannel.BaseColor, KnownProperty.RGBA, new Vector4(TempVar.R, TempVar.G, TempVar.B, 1));
                    materialBuilders.Add(material1);
                }
                //SharpGLTF.Scenes.NodeBuilder Binding = new SharpGLTF.Scenes.NodeBuilder();
                //for (int i = 0; i < modelHeader.boneDatas.Count; i++)
                //{
                //    Binding = Binding.CreateNode(modelHeader.boneDatas[i].BoneName);
                //    //Binding.
                //    Binding.LocalTransform = Matrix4x4.CreateTranslation(modelHeader.boneDatas[i].XLocation, modelHeader.boneDatas[i].YLocation, modelHeader.boneDatas[i].ZLocation);
                //    Binding.AddNode(Binding);
                //}
                for (int i = 0; i < modelHeader.staticMesh.Count; i++)
                {
                    var Data = modelHeader.staticMesh[i];
                    int tempInt = Data.ChunkID;
                    int MatId = modelHeader.MeshGroups[tempInt].MaterialID;

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

                        //VertexJoints4 temp = new VertexJoints4();

                        mesh.UsePrimitive(materialBuilders[MatId]).AddTriangle((TempPos1, TempTexture1), (TempPos2, TempTexture2), (TempPos3, TempTexture3));
                    }

                }

                scene.AddRigidMesh(mesh, Matrix4x4.CreateTranslation(0, 0, 0));
            }

            // save the model in different formats

            var model = scene.ToGltf2();
            model.SaveGLB(Output);
        }

        public static void SavePDBModelglTF(string Output, PBDHandler Handler)
        {
            int Pos1 = 0;
            for (int a = 0; a < Handler.PrefabData.Count; a++)
            {
                var scene = new SharpGLTF.Scenes.SceneBuilder();
                var material = new MaterialBuilder("Default").WithChannelParam(KnownChannel.BaseColor, KnownProperty.RGBA, new Vector4(1, 1, 1, 1));
                int StartPos = Pos1;
                for (int ax = Pos1; ax < StartPos + Handler.PrefabData[a].TotalMeshCount; ax++)
                {
                    var mesh = new MeshBuilder<VertexPositionNormal, VertexTexture1, VertexEmpty>(a.ToString() + " " + Handler.models[a].staticMeshes.Count);
                    for (int i = 0; i < Handler.models[ax].staticMeshes.Count; i++)
                    {
                        var Data = Handler.models[ax].staticMeshes[i];
                        for (int b = 0; b < Data.faces.Count; b++)
                        {
                            Vector3 Scale = Handler.PrefabData[a].Scale;
                            if (Scale.X == 0)
                            {
                                Scale.X = 1;
                            }
                            if (Scale.Y == 0)
                            {
                                Scale.Y = 1;
                            }
                            if (Scale.Z == 0)
                            {
                                Scale.Z = 1;
                            }

                            var Face = Data.faces[b];
                            VertexPositionNormal TempPos1 = new VertexPositionNormal();
                            TempPos1.Position.X = Face.V1.X* Scale.X;
                            TempPos1.Position.Y = Face.V1.Y * Scale.Y;
                            TempPos1.Position.Z = Face.V1.Z * Scale.Z;

                            TempPos1.Normal.X = (float)Face.Normal1.X;
                            TempPos1.Normal.Y = (float)Face.Normal1.Y;
                            TempPos1.Normal.Z = (float)Face.Normal1.Z;

                            VertexPositionNormal TempPos2 = new VertexPositionNormal();
                            TempPos2.Position.X = Face.V2.X * Scale.X;
                            TempPos2.Position.Y = Face.V2.Y * Scale.Y;
                            TempPos2.Position.Z = Face.V2.Z  * Scale.Z;

                            TempPos2.Normal.X = (float)Face.Normal2.X;
                            TempPos2.Normal.Y = (float)Face.Normal2.Y ;
                            TempPos2.Normal.Z = (float)Face.Normal2.Z;

                            VertexPositionNormal TempPos3 = new VertexPositionNormal();
                            TempPos3.Position.X = Face.V3.X * Scale.X;
                            TempPos3.Position.Y = Face.V3.Y * Scale.Y;
                            TempPos3.Position.Z = Face.V3.Z* Scale.Z;

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

                            mesh.UsePrimitive(material).AddTriangle((TempPos1, TempTexture1), (TempPos2, TempTexture2), (TempPos3, TempTexture3));
                        }
                    }
                    scene.AddRigidMesh(mesh, Matrix4x4.Identity);
                    Pos1++;
                }
                var model = scene.ToGltf2();
                model.SaveAsWavefront(Output + "/" + a.ToString());
            }
        }

    }
}
