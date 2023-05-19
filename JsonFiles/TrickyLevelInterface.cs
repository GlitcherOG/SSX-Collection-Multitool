using SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2;
using SSXMultiTool.JsonFiles.Tricky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;
using System.Numerics;
using System.IO;
using SSXMultiTool.FileHandlers.Models;
using static SSXMultiTool.JsonFiles.Tricky.InstanceJsonHandler;
using System.Windows.Documents;
using SSXMultiTool.FileHandlers.Textures;
using Microsoft.Toolkit.HighPerformance;

namespace SSXMultiTool
{
    public class TrickyLevelInterface
    {
        public bool AttemptLightingFix;
        public bool Unilightmap;

        public PatchesJsonHandler patchPoints = new PatchesJsonHandler();
        public InstanceJsonHandler instancesJson = new InstanceJsonHandler();
        public ParticleInstanceJsonHandler particleInstanceJson = new ParticleInstanceJsonHandler();
        public MaterialJsonHandler materialJson = new MaterialJsonHandler();
        public LightJsonHandler lightJsonHandler = new LightJsonHandler();
        public SplineJsonHandler splineJsonHandler = new SplineJsonHandler();
        public TextureFlipbookJsonHandler textureFlipbookJsonHandler = new TextureFlipbookJsonHandler();
        public PrefabJsonHandler prefabJsonHandler = new PrefabJsonHandler();
        public ParticleModelJsonHandler particleModelJsonHandler = new ParticleModelJsonHandler();

        public struct LightingFixObject
        {
            public List<int> Object;
            public List<int> Patch;
        }

        struct InstanceExport
        {
            public int InstanceIndex;
            public string InstanceName;
            public string TextureID;
        }

        public void ExtractTrickyLevelFiles(string LoadPath, string ExportPath)
        {
            ADLHandler adlHandler = new ADLHandler();
            adlHandler.Load(LoadPath + ".adl");
            //adlHandler.Save(LoadPath + ".adl");

            SSFHandler ssfHandler = new SSFHandler();
            ssfHandler.Load(LoadPath + ".ssf");
            ////ssfHandler.SaveTest(LoadPath + ".ssf");

            Directory.CreateDirectory(ExportPath + "\\Collision");
            ssfHandler.SaveModels(ExportPath + "\\Collision");

            //Load Map
            MapHandler mapHandler = new MapHandler();
            mapHandler.Load(LoadPath + ".map");

            //Load PBD
            PBDHandler pbdHandler = new PBDHandler();
            pbdHandler.LoadPBD(LoadPath + ".pbd");

            LTGHandler ltgHandler = new LTGHandler();
            ltgHandler.LoadLTG(LoadPath + ".ltg");

            //Create Patches JSON
            patchPoints = new PatchesJsonHandler();
            for (int i = 0; i < pbdHandler.Patches.Count; i++)
            {
                PatchesJsonHandler.PatchJson patch = new PatchesJsonHandler.PatchJson();
                patch.PatchName = mapHandler.Patchs[i].Name;

                patch.LightMapPoint = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].LightMapPoint);

                patch.UVPoints = new float[4, 2];

                patch.UVPoints[0, 0] = pbdHandler.Patches[i].UVPoint1.X;
                patch.UVPoints[0, 1] = pbdHandler.Patches[i].UVPoint1.Y;

                patch.UVPoints[1, 0] = pbdHandler.Patches[i].UVPoint2.X;
                patch.UVPoints[1, 1] = pbdHandler.Patches[i].UVPoint2.Y;

                patch.UVPoints[2, 0] = pbdHandler.Patches[i].UVPoint3.X;
                patch.UVPoints[2, 1] = pbdHandler.Patches[i].UVPoint3.Y;

                patch.UVPoints[3, 0] = pbdHandler.Patches[i].UVPoint4.X;
                patch.UVPoints[3, 1] = pbdHandler.Patches[i].UVPoint4.Y;

                BezierUtil bezierUtil = new BezierUtil();
                bezierUtil.ProcessedPoints[0] = JsonUtil.Vector4ToVector3(pbdHandler.Patches[i].R1C1);
                bezierUtil.ProcessedPoints[1] = JsonUtil.Vector4ToVector3(pbdHandler.Patches[i].R1C2);
                bezierUtil.ProcessedPoints[2] = JsonUtil.Vector4ToVector3(pbdHandler.Patches[i].R1C3);
                bezierUtil.ProcessedPoints[3] = JsonUtil.Vector4ToVector3(pbdHandler.Patches[i].R1C4);
                bezierUtil.ProcessedPoints[4] = JsonUtil.Vector4ToVector3(pbdHandler.Patches[i].R2C1);
                bezierUtil.ProcessedPoints[5] = JsonUtil.Vector4ToVector3(pbdHandler.Patches[i].R2C2);
                bezierUtil.ProcessedPoints[6] = JsonUtil.Vector4ToVector3(pbdHandler.Patches[i].R2C3);
                bezierUtil.ProcessedPoints[7] = JsonUtil.Vector4ToVector3(pbdHandler.Patches[i].R2C4);
                bezierUtil.ProcessedPoints[8] = JsonUtil.Vector4ToVector3(pbdHandler.Patches[i].R3C1);
                bezierUtil.ProcessedPoints[9] = JsonUtil.Vector4ToVector3(pbdHandler.Patches[i].R3C2);
                bezierUtil.ProcessedPoints[10] = JsonUtil.Vector4ToVector3(pbdHandler.Patches[i].R3C3);
                bezierUtil.ProcessedPoints[11] = JsonUtil.Vector4ToVector3(pbdHandler.Patches[i].R3C4);
                bezierUtil.ProcessedPoints[12] = JsonUtil.Vector4ToVector3(pbdHandler.Patches[i].R4C1);
                bezierUtil.ProcessedPoints[13] = JsonUtil.Vector4ToVector3(pbdHandler.Patches[i].R4C2);
                bezierUtil.ProcessedPoints[14] = JsonUtil.Vector4ToVector3(pbdHandler.Patches[i].R4C3);
                bezierUtil.ProcessedPoints[15] = JsonUtil.Vector4ToVector3(pbdHandler.Patches[i].R4C4);

                bezierUtil.GenerateRawPoints();

                patch.Points = new float[16, 3];

                for (int a= 0; a < 16; a++)
                {
                    patch.Points[a, 0] = bezierUtil.RawPoints[a].X;
                    patch.Points[a, 1] = bezierUtil.RawPoints[a].Y;
                    patch.Points[a, 2] = bezierUtil.RawPoints[a].Z;
                }

                patch.PatchStyle = pbdHandler.Patches[i].PatchStyle;

                if (pbdHandler.Patches[i].PatchVisablity<0)
                {
                    patch.TrickOnlyPatch =true;
                }
                patch.TextureAssigment = pbdHandler.Patches[i].TextureAssigment;
                patch.LightmapID = pbdHandler.Patches[i].LightmapID;
                patchPoints.Patches.Add(patch);
            }
            patchPoints.CreateJson(ExportPath + "/Patches.json");

            //Create Instance JSON
            instancesJson = new InstanceJsonHandler();
            for (int i = 0; i < pbdHandler.Instances.Count; i++)
            {
                InstanceJsonHandler.InstanceJson instanceJson = new InstanceJsonHandler.InstanceJson();
                instanceJson.InstanceName = mapHandler.InternalInstances[i].Name;

                Vector3 Scale;
                Quaternion Rotation;
                Vector3 Location;

                Matrix4x4.Decompose(pbdHandler.Instances[i].matrix4X4, out Scale, out Rotation, out Location);
                instanceJson.Location = JsonUtil.Vector3ToArray(Location);
                instanceJson.Rotation = JsonUtil.QuaternionToArray(Rotation);
                instanceJson.Scale = JsonUtil.Vector3ToArray(Scale);

                instanceJson.Unknown5 = JsonUtil.Vector4ToArray(pbdHandler.Instances[i].Unknown5);
                instanceJson.Unknown6 = JsonUtil.Vector4ToArray(pbdHandler.Instances[i].Unknown6);
                instanceJson.Unknown7 = JsonUtil.Vector4ToArray(pbdHandler.Instances[i].Unknown7);
                instanceJson.Unknown8 = JsonUtil.Vector4ToArray(pbdHandler.Instances[i].Unknown8);
                instanceJson.Unknown9 = JsonUtil.Vector4ToArray(pbdHandler.Instances[i].Unknown9);
                instanceJson.Unknown10 = JsonUtil.Vector4ToArray(pbdHandler.Instances[i].Unknown10);
                instanceJson.Unknown11 = JsonUtil.Vector4ToArray(pbdHandler.Instances[i].Unknown11);
                instanceJson.RGBA = JsonUtil.Vector4ToArray(pbdHandler.Instances[i].RGBA);
                instanceJson.ModelID = pbdHandler.Instances[i].ModelID;
                instanceJson.PrevInstance = pbdHandler.Instances[i].PrevInstance;
                instanceJson.NextInstance = pbdHandler.Instances[i].NextInstance;

                instanceJson.UnknownInt26 = pbdHandler.Instances[i].UnknownInt26;
                instanceJson.UnknownInt27 = pbdHandler.Instances[i].UnknownInt27;
                instanceJson.UnknownInt28 = pbdHandler.Instances[i].UnknownInt28;
                instanceJson.ModelID2 = pbdHandler.Instances[i].ModelID2;
                instanceJson.UnknownInt30 = pbdHandler.Instances[i].UnknownInt30;
                instanceJson.UnknownInt31 = pbdHandler.Instances[i].UnknownInt31;
                instanceJson.UnknownInt32 = pbdHandler.Instances[i].UnknownInt32;

                instanceJson.LTGState = ltgHandler.FindIfInstaneState(i);
                instanceJson.SSFState = ssfHandler.InstanceState[i];

                instancesJson.Instances.Add(instanceJson);
            }
            instancesJson.CreateJson(ExportPath + "/Instances.json");

            //Create Particle Instances JSON
            particleInstanceJson = new ParticleInstanceJsonHandler();
            for (int i = 0; i < pbdHandler.particleInstances.Count; i++)
            {
                ParticleInstanceJsonHandler.ParticleJson TempParticle = new ParticleInstanceJsonHandler.ParticleJson();
                TempParticle.ParticleName = mapHandler.ParticleInstances[i].Name;

                Vector3 Scale;
                Quaternion Rotation;
                Vector3 Location;

                Matrix4x4.Decompose(pbdHandler.particleInstances[i].matrix4X4, out Scale, out Rotation, out Location);
                TempParticle.Location = JsonUtil.Vector3ToArray(Location);
                TempParticle.Rotation = JsonUtil.QuaternionToArray(Rotation);
                TempParticle.Scale = JsonUtil.Vector3ToArray(Scale);

                TempParticle.UnknownInt1 = pbdHandler.particleInstances[i].UnknownInt1;
                TempParticle.LowestXYZ = pbdHandler.particleInstances[i].LowestXYZ;
                TempParticle.HighestXYZ = pbdHandler.particleInstances[i].HighestXYZ;
                TempParticle.UnknownInt8 = pbdHandler.particleInstances[i].UnknownInt8;
                TempParticle.UnknownInt9 = pbdHandler.particleInstances[i].UnknownInt9;
                TempParticle.UnknownInt10 = pbdHandler.particleInstances[i].UnknownInt10;
                TempParticle.UnknownInt11 = pbdHandler.particleInstances[i].UnknownInt11;
                TempParticle.UnknownInt12 = pbdHandler.particleInstances[i].UnknownInt12;
                particleInstanceJson.particleJsons.Add(TempParticle);
            }
            particleInstanceJson.CreateJson(ExportPath + "/ParticleInstances.json");

            //Create Material Json
            materialJson = new MaterialJsonHandler();
            for (int i = 0; i < pbdHandler.materials.Count; i++)
            {
                MaterialJsonHandler.MaterialsJson TempMaterial = new MaterialJsonHandler.MaterialsJson();
                TempMaterial.MaterialName = mapHandler.Materials[i].Name;

                TempMaterial.TextureID = pbdHandler.materials[i].TextureID;
                TempMaterial.UnknownInt2 = pbdHandler.materials[i].UnknownInt2;
                TempMaterial.UnknownInt3 = pbdHandler.materials[i].UnknownInt3;
                TempMaterial.UnknownFloat1 = pbdHandler.materials[i].UnknownFloat1;
                TempMaterial.UnknownFloat2 = pbdHandler.materials[i].UnknownFloat2;
                TempMaterial.UnknownFloat3 = pbdHandler.materials[i].UnknownFloat3;
                TempMaterial.UnknownFloat4 = pbdHandler.materials[i].UnknownFloat4;
                TempMaterial.UnknownInt8 = pbdHandler.materials[i].UnknownInt8;
                TempMaterial.UnknownFloat5 = pbdHandler.materials[i].UnknownFloat5;
                TempMaterial.UnknownFloat6 = pbdHandler.materials[i].UnknownFloat6;
                TempMaterial.UnknownFloat7 = pbdHandler.materials[i].UnknownFloat7;
                TempMaterial.UnknownFloat8 = pbdHandler.materials[i].UnknownFloat8;
                TempMaterial.UnknownInt13 = pbdHandler.materials[i].UnknownInt13;
                TempMaterial.UnknownInt14 = pbdHandler.materials[i].UnknownInt14;
                TempMaterial.UnknownInt15 = pbdHandler.materials[i].UnknownInt15;
                TempMaterial.UnknownInt16 = pbdHandler.materials[i].UnknownInt16;
                TempMaterial.UnknownInt17 = pbdHandler.materials[i].UnknownInt17;
                TempMaterial.UnknownInt18 = pbdHandler.materials[i].UnknownInt18;
                TempMaterial.TextureFlipbookID = pbdHandler.materials[i].TextureFlipbookID;
                TempMaterial.UnknownInt20 = pbdHandler.materials[i].UnknownInt20;
                materialJson.MaterialsJsons.Add(TempMaterial);
            }
            materialJson.CreateJson(ExportPath + "/Material.json");

            //Create Lights Json
            lightJsonHandler = new LightJsonHandler();
            for (int i = 0; i < pbdHandler.lights.Count; i++)
            {
                LightJsonHandler.LightJson TempLight = new LightJsonHandler.LightJson();
                TempLight.LightName = mapHandler.Lights[i].Name;

                TempLight.Type = pbdHandler.lights[i].Type;
                TempLight.SpriteRes = pbdHandler.lights[i].spriteRes;
                TempLight.UnknownFloat1 = pbdHandler.lights[i].UnknownFloat1;
                TempLight.UnknownInt1 = pbdHandler.lights[i].UnknownInt1;
                TempLight.Colour = JsonUtil.Vector3ToArray(pbdHandler.lights[i].Colour);
                TempLight.Direction = JsonUtil.Vector3ToArray(pbdHandler.lights[i].Direction);
                TempLight.Postion = JsonUtil.Vector3ToArray(pbdHandler.lights[i].Postion);
                TempLight.LowestXYZ = JsonUtil.Vector3ToArray(pbdHandler.lights[i].LowestXYZ);
                TempLight.HighestXYZ = JsonUtil.Vector3ToArray(pbdHandler.lights[i].HighestXYZ);
                TempLight.UnknownFloat2 = pbdHandler.lights[i].UnknownFloat2;
                TempLight.UnknownInt2 = pbdHandler.lights[i].UnknownInt2;
                TempLight.UnknownFloat3 = pbdHandler.lights[i].UnknownFloat3;
                TempLight.UnknownInt3 = pbdHandler.lights[i].UnknownInt3;

                lightJsonHandler.LightJsons.Add(TempLight);

            }
            lightJsonHandler.CreateJson(ExportPath + "/Lights.json");

            //Create Spline Json
            splineJsonHandler = new SplineJsonHandler();
            for (int i = 0; i < pbdHandler.splines.Count; i++)
            {
                SplineJsonHandler.SplineJson TempSpline = new SplineJsonHandler.SplineJson();
                TempSpline.SplineName = mapHandler.Splines[i].Name;
                TempSpline.Unknown1 = pbdHandler.splines[i].Unknown1;
                TempSpline.Unknown2 = pbdHandler.splines[i].Unknown2;
                TempSpline.SegmentCount = pbdHandler.splines[i].SplineSegmentCount;
                TempSpline.Segments = new List<SplineJsonHandler.SegmentJson>();

                for (int a = pbdHandler.splines[i].SplineSegmentPosition; a < pbdHandler.splines[i].SplineSegmentPosition + TempSpline.SegmentCount; a++)
                {
                    SplineJsonHandler.SegmentJson segmentJson = new SplineJsonHandler.SegmentJson();

                    BezierUtil bezierUtil = new BezierUtil();
                    bezierUtil.ProcessedPoints[0] = JsonUtil.Vector4ToVector3(pbdHandler.splinesSegments[a].ControlPoint);
                    bezierUtil.ProcessedPoints[1] = JsonUtil.Vector4ToVector3(pbdHandler.splinesSegments[a].Point2);
                    bezierUtil.ProcessedPoints[2] = JsonUtil.Vector4ToVector3(pbdHandler.splinesSegments[a].Point3);
                    bezierUtil.ProcessedPoints[3] = JsonUtil.Vector4ToVector3(pbdHandler.splinesSegments[a].Point4);

                    bezierUtil.GenerateRawPoints();

                    segmentJson.Point4 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[0]);
                    segmentJson.Point3 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[1]);
                    segmentJson.Point2 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[2]);
                    segmentJson.Point1 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[3]);

                    segmentJson.Unknown = JsonUtil.Vector4ToArray(pbdHandler.splinesSegments[a].ScalingPoint);

                    segmentJson.Unknown32 = pbdHandler.splinesSegments[a].Unknown32;
                    TempSpline.Segments.Add(segmentJson);
                }
                splineJsonHandler.SplineJsons.Add(TempSpline);
            }
            splineJsonHandler.CreateJson(ExportPath + "/Splines.json");

            //Create Texture FLipbook Json
            textureFlipbookJsonHandler = new TextureFlipbookJsonHandler();
            for (int i = 0; i < pbdHandler.textureFlipbooks.Count; i++)
            {
                TextureFlipbookJsonHandler.FlipbookJson TempFlipbook = new TextureFlipbookJsonHandler.FlipbookJson();
                TempFlipbook.Images = pbdHandler.textureFlipbooks[i].ImagePositions;
                textureFlipbookJsonHandler.FlipbookJsons.Add(TempFlipbook);
            }
            textureFlipbookJsonHandler.CreateJson(ExportPath + "/TextureFlipbook.json");

            //Create Model Json
            prefabJsonHandler = new PrefabJsonHandler();
            for (int i = 0; i < pbdHandler.PrefabData.Count; i++)
            {
                PrefabJsonHandler.PrefabJson TempModel = new PrefabJsonHandler.PrefabJson();
                TempModel.PrefabName = mapHandler.Models[i].Name;
                TempModel.Unknown3 = pbdHandler.PrefabData[i].Unknown3;
                TempModel.AnimTime = pbdHandler.PrefabData[i].AnimTime;
                TempModel.PrefabObjects = new();

                for (int a = 0; a < pbdHandler.PrefabData[i].PrefabObjects.Count; a++)
                {
                    var TempPrefabObject = new PrefabJsonHandler.ObjectHeader();
                    TempPrefabObject.ParentID = pbdHandler.PrefabData[i].PrefabObjects[a].ParentID;
                    TempPrefabObject.Flags = pbdHandler.PrefabData[i].PrefabObjects[a].objectData.Flags;

                    TempPrefabObject.MeshData = new List<PrefabJsonHandler.MeshHeader>();
                    if (pbdHandler.PrefabData[i].PrefabObjects[a].objectData.MeshOffsets != null)
                    {
                        for (int b = 0; b < pbdHandler.PrefabData[i].PrefabObjects[a].objectData.MeshOffsets.Count; b++)
                        {
                            var TempMeshHeader = new PrefabJsonHandler.MeshHeader();
                            TempMeshHeader.MeshID = pbdHandler.PrefabData[i].PrefabObjects[a].objectData.MeshOffsets[b].MeshID;
                            TempMeshHeader.MeshPath = pbdHandler.PrefabData[i].PrefabObjects[a].objectData.MeshOffsets[b].MeshID + ".obj";
                            TempMeshHeader.MaterialID = pbdHandler.materialBlocks[pbdHandler.PrefabData[i].MaterialBlockID].ints[pbdHandler.PrefabData[i].PrefabObjects[a].objectData.MeshOffsets[b].MaterialBlockPos];

                            TempPrefabObject.MeshData.Add(TempMeshHeader);
                        }
                    }

                    if (pbdHandler.PrefabData[i].PrefabObjects[a].IncludeMatrix)
                    {
                        TempPrefabObject.IncludeMatrix = true;
                        Vector3 Scale;
                        Quaternion Rotation;
                        Vector3 Location;

                        Matrix4x4.Decompose(pbdHandler.PrefabData[i].PrefabObjects[a].matrix4X4, out Scale, out Rotation, out Location);

                        TempPrefabObject.Position = JsonUtil.Vector3ToArray(Location);
                        TempPrefabObject.Rotation = JsonUtil.QuaternionToArray(Rotation);
                        TempPrefabObject.Scale = JsonUtil.Vector3ToArray(Scale);
                    }

                    if(pbdHandler.PrefabData[i].PrefabObjects[a].IncludeAnimation)
                    {
                        var TempAnimation = new PrefabJsonHandler.ObjectAnimation();
                        TempPrefabObject.IncludeAnimation = true;
                        TempAnimation.U1 = pbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.U1;
                        TempAnimation.U2 = pbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.U2;
                        TempAnimation.U3 = pbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.U3;
                        TempAnimation.U4 = pbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.U4;
                        TempAnimation.U5 = pbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.U5;
                        TempAnimation.U6 = pbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.U6;

                        TempAnimation.AnimationAction = pbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.AnimationAction;

                        TempAnimation.AnimationEntries = new List<PrefabJsonHandler.AnimationEntry>();

                        for (int b = 0; b < pbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.animationEntries.Count; b++)
                        {
                            var TempEntry = new PrefabJsonHandler.AnimationEntry();
                            TempEntry.AnimationMaths = new List<PrefabJsonHandler.AnimationMath>();
                            for (int c = 0; c < pbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.animationEntries[b].animationMaths.Count; c++)
                            {
                                var TempMaths = new PrefabJsonHandler.AnimationMath();

                                TempMaths.Value1 = pbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.animationEntries[b].animationMaths[c].Value1;
                                TempMaths.Value2 = pbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.animationEntries[b].animationMaths[c].Value2;
                                TempMaths.Value3 = pbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.animationEntries[b].animationMaths[c].Value3;
                                TempMaths.Value4 = pbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.animationEntries[b].animationMaths[c].Value4;
                                TempMaths.Value5 = pbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.animationEntries[b].animationMaths[c].Value5;
                                TempMaths.Value6 = pbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.animationEntries[b].animationMaths[c].Value6;

                                TempEntry.AnimationMaths.Add(TempMaths);
                            }


                            TempAnimation.AnimationEntries.Add(TempEntry);
                        }


                        TempPrefabObject.Animation = TempAnimation;
                    }

                    TempModel.PrefabObjects.Add(TempPrefabObject);
                }

                prefabJsonHandler.PrefabJsons.Add(TempModel);
            }
            prefabJsonHandler.CreateJson(ExportPath + "/Prefabs.json");

            //Create Particle Model Json
            //particleModelJsonHandler = new ParticleModelJsonHandler();
            //for (int i = 0; i < pbdHandler.particleModels.Count; i++)
            //{
            //    ParticleModelJsonHandler.ParticleModelJson TempParticleModel = new ParticleModelJsonHandler.ParticleModelJson();
            //    TempParticleModel.ParticleModelName = mapHandler.particelModels[i].Name;
            //    TempParticleModel.Unknown0 = pbdHandler.particleModels[i].Unknown0;
            //    TempParticleModel.Unknown1 = pbdHandler.particleModels[i].Unknown0;
            //    TempParticleModel.Unknown2 = pbdHandler.particleModels[i].Unknown0;
            //    TempParticleModel.Unknown3 = pbdHandler.particleModels[i].Unknown0;
            //    TempParticleModel.Unknown4 = pbdHandler.particleModels[i].Unknown0;
            //    TempParticleModel.Unknown5 = pbdHandler.particleModels[i].Unknown0;
            //    TempParticleModel.Unknown6 = pbdHandler.particleModels[i].Unknown0;
            //    TempParticleModel.Unknown7 = pbdHandler.particleModels[i].Unknown0;
            //    TempParticleModel.Unknown8 = pbdHandler.particleModels[i].Unknown0;
            //    TempParticleModel.Unknown9 = pbdHandler.particleModels[i].Unknown0;
            //    TempParticleModel.Unknown10 = pbdHandler.particleModels[i].Unknown0;
            //    TempParticleModel.UnknownLenght = pbdHandler.particleModels[i].UnknownLenght;
            //    TempParticleModel.bytes = pbdHandler.particleModels[i].bytes;
            //    particleModelJsonHandler.ParticleModelJsons.Add(TempParticleModel);
            //}
            //particleModelJsonHandler.CreateJson(ExportPath + "/ParticleModelHeaders.json");

            //Create Camera Json

            //Create Mesh data
            Directory.CreateDirectory(ExportPath + "/Models");
            pbdHandler.ExportModels(ExportPath + "/Models/");

            //Load and Export Textures
            OldSSHHandler TextureHandler = new OldSSHHandler();
            OldSSHHandler SkyboxHandler = new OldSSHHandler();
            OldSSHHandler LightmapHandler = new OldSSHHandler();
            Directory.CreateDirectory(ExportPath + "/Textures");
            Directory.CreateDirectory(ExportPath + "/Skybox");
            Directory.CreateDirectory(ExportPath + "/Skybox/Textures");
            Directory.CreateDirectory(ExportPath + "/Skybox/Models");
            Directory.CreateDirectory(ExportPath + "/Lightmaps");

            TextureHandler.LoadSSH(LoadPath + ".ssh");
            for (int i = 0; i < TextureHandler.sshImages.Count; i++)
            {
                TextureHandler.BrightenBitmap(i);
                TextureHandler.BMPOneExtract(ExportPath + "\\Textures\\" + TextureHandler.sshImages[i].shortname + ".png", i);
            }
            if (File.Exists(LoadPath + "_sky.ssh"))
            {
                //Load PBD Sky
                PBDHandler skypbdHandler = new PBDHandler();
                skypbdHandler.LoadPBD(LoadPath + "_sky.pbd");

                //Create Material Json
                materialJson = new MaterialJsonHandler();
                for (int i = 0; i < skypbdHandler.materials.Count; i++)
                {
                    MaterialJsonHandler.MaterialsJson TempMaterial = new MaterialJsonHandler.MaterialsJson();

                    TempMaterial.TextureID = skypbdHandler.materials[i].TextureID;
                    TempMaterial.UnknownInt2 = skypbdHandler.materials[i].UnknownInt2;
                    TempMaterial.UnknownInt3 = skypbdHandler.materials[i].UnknownInt3;
                    TempMaterial.UnknownFloat1 = skypbdHandler.materials[i].UnknownFloat1;
                    TempMaterial.UnknownFloat2 = skypbdHandler.materials[i].UnknownFloat2;
                    TempMaterial.UnknownFloat3 = skypbdHandler.materials[i].UnknownFloat3;
                    TempMaterial.UnknownFloat4 = skypbdHandler.materials[i].UnknownFloat4;
                    TempMaterial.UnknownInt8 = skypbdHandler.materials[i].UnknownInt8;
                    TempMaterial.UnknownFloat5 = skypbdHandler.materials[i].UnknownFloat5;
                    TempMaterial.UnknownFloat6 = skypbdHandler.materials[i].UnknownFloat6;
                    TempMaterial.UnknownFloat7 = skypbdHandler.materials[i].UnknownFloat7;
                    TempMaterial.UnknownFloat8 = skypbdHandler.materials[i].UnknownFloat8;
                    TempMaterial.UnknownInt13 = skypbdHandler.materials[i].UnknownInt13;
                    TempMaterial.UnknownInt14 = skypbdHandler.materials[i].UnknownInt14;
                    TempMaterial.UnknownInt15 = skypbdHandler.materials[i].UnknownInt15;
                    TempMaterial.UnknownInt16 = skypbdHandler.materials[i].UnknownInt16;
                    TempMaterial.UnknownInt17 = skypbdHandler.materials[i].UnknownInt17;
                    TempMaterial.UnknownInt18 = skypbdHandler.materials[i].UnknownInt18;
                    TempMaterial.TextureFlipbookID = skypbdHandler.materials[i].TextureFlipbookID;
                    TempMaterial.UnknownInt20 = skypbdHandler.materials[i].UnknownInt20;
                    materialJson.MaterialsJsons.Add(TempMaterial);
                }
                materialJson.CreateJson(ExportPath + "/Skybox/Material.json");

                ////Create Model Json
                prefabJsonHandler = new PrefabJsonHandler();
                for (int i = 0; i < skypbdHandler.PrefabData.Count; i++)
                {
                    PrefabJsonHandler.PrefabJson TempModel = new PrefabJsonHandler.PrefabJson();
 
                    TempModel.Unknown3 = skypbdHandler.PrefabData[i].Unknown3;
                    TempModel.AnimTime = skypbdHandler.PrefabData[i].AnimTime;
                    TempModel.PrefabObjects = new();

                    for (int a = 0; a < skypbdHandler.PrefabData[i].PrefabObjects.Count; a++)
                    {
                        var TempPrefabObject = new PrefabJsonHandler.ObjectHeader();
                        TempPrefabObject.ParentID = skypbdHandler.PrefabData[i].PrefabObjects[a].ParentID;
                        TempPrefabObject.Flags = skypbdHandler.PrefabData[i].PrefabObjects[a].objectData.Flags;

                        TempPrefabObject.MeshData = new List<PrefabJsonHandler.MeshHeader>();
                        if (skypbdHandler.PrefabData[i].PrefabObjects[a].objectData.MeshOffsets != null)
                        {
                            for (int b = 0; b < skypbdHandler.PrefabData[i].PrefabObjects[a].objectData.MeshOffsets.Count; b++)
                            {
                                var TempMeshHeader = new PrefabJsonHandler.MeshHeader();
                                TempMeshHeader.MeshID = skypbdHandler.PrefabData[i].PrefabObjects[a].objectData.MeshOffsets[b].MeshID;
                                TempMeshHeader.MeshPath = skypbdHandler.PrefabData[i].PrefabObjects[a].objectData.MeshOffsets[b].MeshID + ".obj";
                                TempMeshHeader.MaterialID = skypbdHandler.materialBlocks[skypbdHandler.PrefabData[i].MaterialBlockID].ints[skypbdHandler.PrefabData[i].PrefabObjects[a].objectData.MeshOffsets[b].MaterialBlockPos];

                                TempPrefabObject.MeshData.Add(TempMeshHeader);
                            }
                        }

                        if (skypbdHandler.PrefabData[i].PrefabObjects[a].IncludeMatrix)
                        {
                            TempPrefabObject.IncludeMatrix = true;
                            Vector3 Scale;
                            Quaternion Rotation;
                            Vector3 Location;

                            Matrix4x4.Decompose(skypbdHandler.PrefabData[i].PrefabObjects[a].matrix4X4, out Scale, out Rotation, out Location);

                            TempPrefabObject.Position = JsonUtil.Vector3ToArray(Location);
                            TempPrefabObject.Rotation = JsonUtil.QuaternionToArray(Rotation);
                            TempPrefabObject.Scale = JsonUtil.Vector3ToArray(Scale);
                        }

                        if (skypbdHandler.PrefabData[i].PrefabObjects[a].IncludeAnimation)
                        {
                            var TempAnimation = new PrefabJsonHandler.ObjectAnimation();
                            TempPrefabObject.IncludeAnimation = true;
                            TempAnimation.U1 = skypbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.U1;
                            TempAnimation.U2 = skypbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.U2;
                            TempAnimation.U3 = skypbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.U3;
                            TempAnimation.U4 = skypbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.U4;
                            TempAnimation.U5 = skypbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.U5;
                            TempAnimation.U6 = skypbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.U6;

                            TempAnimation.AnimationAction = skypbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.AnimationAction;

                            TempAnimation.AnimationEntries = new List<PrefabJsonHandler.AnimationEntry>();

                            for (int b = 0; b < skypbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.animationEntries.Count; b++)
                            {
                                var TempEntry = new PrefabJsonHandler.AnimationEntry();
                                TempEntry.AnimationMaths = new List<PrefabJsonHandler.AnimationMath>();
                                for (int c = 0; c < skypbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.animationEntries[b].animationMaths.Count; c++)
                                {
                                    var TempMaths = new PrefabJsonHandler.AnimationMath();

                                    TempMaths.Value1 = skypbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.animationEntries[b].animationMaths[c].Value1;
                                    TempMaths.Value2 = skypbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.animationEntries[b].animationMaths[c].Value2;
                                    TempMaths.Value3 = skypbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.animationEntries[b].animationMaths[c].Value3;
                                    TempMaths.Value4 = skypbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.animationEntries[b].animationMaths[c].Value4;
                                    TempMaths.Value5 = skypbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.animationEntries[b].animationMaths[c].Value5;
                                    TempMaths.Value6 = skypbdHandler.PrefabData[i].PrefabObjects[a].objectAnimation.animationEntries[b].animationMaths[c].Value6;

                                    TempEntry.AnimationMaths.Add(TempMaths);
                                }


                                TempAnimation.AnimationEntries.Add(TempEntry);
                            }


                            TempPrefabObject.Animation = TempAnimation;
                        }

                        TempModel.PrefabObjects.Add(TempPrefabObject);
                    }

                    prefabJsonHandler.PrefabJsons.Add(TempModel);
                }
                prefabJsonHandler.CreateJson(ExportPath + "/Skybox/Prefabs.json");


                skypbdHandler.ExportModels(ExportPath + "/Skybox/Models/");

                SkyboxHandler.LoadSSH(LoadPath + "_sky.ssh");
                for (int i = 0; i < SkyboxHandler.sshImages.Count; i++)
                {
                    SkyboxHandler.BrightenBitmap(i);
                    SkyboxHandler.BMPOneExtract(ExportPath + "\\Skybox\\Textures\\" + SkyboxHandler.sshImages[i].shortname + ".png", i);
                }


            }

            if (File.Exists(LoadPath + "_L.ssh"))
            {
                LightmapHandler.LoadSSH(LoadPath + "_L.ssh");
                //LightmapHandler = LightmapGenerator.ConvertToStandardLightmap(pbdHandler, TextureHandler, LightmapHandler);

                for (int i = 0; i < LightmapHandler.sshImages.Count; i++)
                {
                    LightmapHandler.BrightenBitmap(i);
                    LightmapHandler.BMPOneExtract(ExportPath + "\\Lightmaps\\" + LightmapHandler.sshImages[i].shortname + ".png", i);
                }
            }
            else
            {
                LightmapHandler.LoadSSH(LoadPath.Substring(0, LoadPath.Length - 1) + "_L.ssh");
                for (int i = 0; i < LightmapHandler.sshImages.Count; i++)
                {
                    LightmapHandler.BrightenBitmap(i);
                    LightmapHandler.BMPOneExtract(ExportPath + "\\Lightmaps\\" + LightmapHandler.sshImages[i].shortname + ".png", i);
                }
            }

            Directory.CreateDirectory(ExportPath + "/Original");
            File.Copy(LoadPath + ".ltg", ExportPath + "/Original" + "/ltg.ltg");
            File.Copy(LoadPath + ".ssf", ExportPath + "/Original" + "/ssf.ssf");
            File.Copy(LoadPath + ".pbd", ExportPath + "/Original" + "/level.pbd");
            File.Copy(LoadPath + ".map", ExportPath + "/Original" + "/level.map");
            if (File.Exists(LoadPath + "_sky.pbd"))
            {
                File.Copy(LoadPath + ".adl", ExportPath + "/Original" + "/ald.ald"); //Not in Menu
                File.Copy(LoadPath + "_sky.pbd", ExportPath + "/Original/sky.pbd");
                File.Copy(LoadPath + ".aip", ExportPath + "/Original" + "/aip.aip"); //Not in Menu
                File.Copy(LoadPath + ".sop", ExportPath + "/Original" + "/sop.sop"); //Not in menu

                //Load and Convert AIP to JSON
                AIPSOPHandler aipHandler = new AIPSOPHandler();
                aipHandler.LoadAIPSOP(LoadPath + ".aip");
                AIPSOPJsonHandler aipJsonHandler = new AIPSOPJsonHandler();
                aipJsonHandler.PathTypeA = new List<AIPSOPJsonHandler.PathTypeAJson>();
                for (int i = 0; i < aipHandler.typeAs.Count; i++)
                {
                    AIPSOPJsonHandler.PathTypeAJson pathTypeAJson = new AIPSOPJsonHandler.PathTypeAJson();
                    var TempPath = aipHandler.typeAs[i];
                    pathTypeAJson.Unknown1 = TempPath.Unknown1;
                    pathTypeAJson.Unknown2 = TempPath.Unknown2;
                    pathTypeAJson.Unknown3 = TempPath.Unknown3;
                    pathTypeAJson.Unknown4 = TempPath.Unknown4;
                    pathTypeAJson.Unknown5 = TempPath.Unknown5;
                    pathTypeAJson.Unknown6 = TempPath.Unknown6;
                    pathTypeAJson.Unknown7 = TempPath.Unknown7;

                    pathTypeAJson.pathPos = JsonUtil.Vector3ToArray(TempPath.pathPos);

                    Vector3 CurrentPoint = TempPath.pathPos;
                    pathTypeAJson.vectorPoints = new List<float[]>();
                    for (int a = 0; a < TempPath.vectorPoints.Count; a++)
                    {
                        Vector3 Direction = JsonUtil.Vector4ToVector3(TempPath.vectorPoints[a]);
                        float Distance = TempPath.vectorPoints[a].W;
                        CurrentPoint = (Direction * Distance) + CurrentPoint;
                        pathTypeAJson.vectorPoints.Add(JsonUtil.Vector3ToArray(CurrentPoint));
                    }

                    pathTypeAJson.unkownListTypeAs = new List<AIPSOPJsonHandler.UnkownListTypeAJson>();
                    for (int a = 0; a < TempPath.unkownListTypeAs.Count; a++)
                    {
                        var unknownTypeA = new AIPSOPJsonHandler.UnkownListTypeAJson();
                        var TempTypeAList = TempPath.unkownListTypeAs[a];

                        unknownTypeA.Unknown1 = TempTypeAList.Unknown1;
                        unknownTypeA.Unknown2 = TempTypeAList.Unknown2;
                        unknownTypeA.Unknown3 = TempTypeAList.Unknown3;
                        unknownTypeA.Unknown4 = TempTypeAList.Unknown4;

                        pathTypeAJson.unkownListTypeAs.Add(unknownTypeA);
                    }

                    aipJsonHandler.PathTypeA.Add(pathTypeAJson);
                }

                aipJsonHandler.PathTypeB = new List<AIPSOPJsonHandler.PathTypeBJson>();
                for (int i = 0; i < aipHandler.typeBs.Count; i++)
                {
                    AIPSOPJsonHandler.PathTypeBJson pathTypeBJson = new AIPSOPJsonHandler.PathTypeBJson();
                    var TempPath = aipHandler.typeBs[i];
                    pathTypeBJson.Unknown1 = TempPath.Unknown1;
                    pathTypeBJson.Unknown2 = TempPath.Unknown2;
                    pathTypeBJson.Unknown3 = TempPath.Unknown3;
                    pathTypeBJson.Unknown4 = TempPath.Unknown4;

                    pathTypeBJson.pathPos = JsonUtil.Vector3ToArray(TempPath.pathPos);

                    Vector3 CurrentPoint = TempPath.pathPos;
                    pathTypeBJson.vectorPoints = new List<float[]>();
                    for (int a = 0; a < TempPath.vectorPoints.Count; a++)
                    {
                        Vector3 Direction = JsonUtil.Vector4ToVector3(TempPath.vectorPoints[a]);
                        float Distance = TempPath.vectorPoints[a].W;
                        CurrentPoint = (Direction * Distance) + CurrentPoint;
                        pathTypeBJson.vectorPoints.Add(JsonUtil.Vector3ToArray(CurrentPoint));
                    }

                    pathTypeBJson.unkownListTypeAs = new List<AIPSOPJsonHandler.UnkownListTypeAJson>();
                    for (int a = 0; a < TempPath.unkownListTypeAs.Count; a++)
                    {
                        var unknownTypeA = new AIPSOPJsonHandler.UnkownListTypeAJson();
                        var TempTypeAList = TempPath.unkownListTypeAs[a];

                        unknownTypeA.Unknown1 = TempTypeAList.Unknown1;
                        unknownTypeA.Unknown2 = TempTypeAList.Unknown2;
                        unknownTypeA.Unknown3 = TempTypeAList.Unknown3;
                        unknownTypeA.Unknown4 = TempTypeAList.Unknown4;

                        pathTypeBJson.unkownListTypeAs.Add(unknownTypeA);
                    }

                    aipJsonHandler.PathTypeB.Add(pathTypeBJson);
                }
                aipJsonHandler.CreateJson(ExportPath + "/AIP.json");

                AIPSOPHandler sopHandler = new AIPSOPHandler();
                sopHandler.LoadAIPSOP(LoadPath + ".sop");
                AIPSOPJsonHandler sopJsonHandler = new AIPSOPJsonHandler();
                sopJsonHandler.PathTypeA = new List<AIPSOPJsonHandler.PathTypeAJson>();
                for (int i = 0; i < aipHandler.typeAs.Count; i++)
                {
                    AIPSOPJsonHandler.PathTypeAJson pathTypeAJson = new AIPSOPJsonHandler.PathTypeAJson();
                    var TempPath = aipHandler.typeAs[i];
                    pathTypeAJson.Unknown1 = TempPath.Unknown1;
                    pathTypeAJson.Unknown2 = TempPath.Unknown2;
                    pathTypeAJson.Unknown3 = TempPath.Unknown3;
                    pathTypeAJson.Unknown4 = TempPath.Unknown4;
                    pathTypeAJson.Unknown5 = TempPath.Unknown5;
                    pathTypeAJson.Unknown6 = TempPath.Unknown6;
                    pathTypeAJson.Unknown7 = TempPath.Unknown7;

                    pathTypeAJson.pathPos = JsonUtil.Vector3ToArray(TempPath.pathPos);

                    Vector3 CurrentPoint = TempPath.pathPos;
                    pathTypeAJson.vectorPoints = new List<float[]>();
                    for (int a = 0; a < TempPath.vectorPoints.Count; a++)
                    {
                        Vector3 Direction = JsonUtil.Vector4ToVector3(TempPath.vectorPoints[a]);
                        float Distance = TempPath.vectorPoints[a].W;
                        CurrentPoint = (Direction * Distance) + CurrentPoint;
                        pathTypeAJson.vectorPoints.Add(JsonUtil.Vector3ToArray(CurrentPoint));
                    }

                    pathTypeAJson.unkownListTypeAs = new List<AIPSOPJsonHandler.UnkownListTypeAJson>();
                    for (int a = 0; a < TempPath.unkownListTypeAs.Count; a++)
                    {
                        var unknownTypeA = new AIPSOPJsonHandler.UnkownListTypeAJson();
                        var TempTypeAList = TempPath.unkownListTypeAs[a];

                        unknownTypeA.Unknown1 = TempTypeAList.Unknown1;
                        unknownTypeA.Unknown2 = TempTypeAList.Unknown2;
                        unknownTypeA.Unknown3 = TempTypeAList.Unknown3;
                        unknownTypeA.Unknown4 = TempTypeAList.Unknown4;

                        pathTypeAJson.unkownListTypeAs.Add(unknownTypeA);
                    }

                    sopJsonHandler.PathTypeA.Add(pathTypeAJson);
                }

                sopJsonHandler.PathTypeB = new List<AIPSOPJsonHandler.PathTypeBJson>();
                for (int i = 0; i < aipHandler.typeBs.Count; i++)
                {
                    AIPSOPJsonHandler.PathTypeBJson pathTypeBJson = new AIPSOPJsonHandler.PathTypeBJson();
                    var TempPath = aipHandler.typeBs[i];
                    pathTypeBJson.Unknown1 = TempPath.Unknown1;
                    pathTypeBJson.Unknown2 = TempPath.Unknown2;
                    pathTypeBJson.Unknown3 = TempPath.Unknown3;
                    pathTypeBJson.Unknown4 = TempPath.Unknown4;

                    pathTypeBJson.pathPos = JsonUtil.Vector3ToArray(TempPath.pathPos);

                    Vector3 CurrentPoint = TempPath.pathPos;
                    pathTypeBJson.vectorPoints = new List<float[]>();
                    for (int a = 0; a < TempPath.vectorPoints.Count; a++)
                    {
                        Vector3 Direction = JsonUtil.Vector4ToVector3(TempPath.vectorPoints[a]);
                        float Distance = TempPath.vectorPoints[a].W;
                        CurrentPoint = (Direction * Distance) + CurrentPoint;
                        pathTypeBJson.vectorPoints.Add(JsonUtil.Vector3ToArray(CurrentPoint));
                    }

                    pathTypeBJson.unkownListTypeAs = new List<AIPSOPJsonHandler.UnkownListTypeAJson>();
                    for (int a = 0; a < TempPath.unkownListTypeAs.Count; a++)
                    {
                        var unknownTypeA = new AIPSOPJsonHandler.UnkownListTypeAJson();
                        var TempTypeAList = TempPath.unkownListTypeAs[a];

                        unknownTypeA.Unknown1 = TempTypeAList.Unknown1;
                        unknownTypeA.Unknown2 = TempTypeAList.Unknown2;
                        unknownTypeA.Unknown3 = TempTypeAList.Unknown3;
                        unknownTypeA.Unknown4 = TempTypeAList.Unknown4;

                        pathTypeBJson.unkownListTypeAs.Add(unknownTypeA);
                    }

                    sopJsonHandler.PathTypeB.Add(pathTypeBJson);
                }
                sopJsonHandler.CreateJson(ExportPath + "/SOP.json");
            }
        }

        public void BuildTrickyLevelFiles(string LoadPath, string ExportPath)
        {
            List<LightingFixObject> lightingFixObjects = new List<LightingFixObject>();
            int Length = Directory.GetFiles(LoadPath + "/Textures", "*.png").Length;
            for (int i = 0; i < Length; i++)
            {
                LightingFixObject temp = new LightingFixObject();
                temp.Object = new List<int>();
                temp.Patch = new List<int>();
                lightingFixObjects.Add(temp);
            }

            ExportPath = ExportPath.Substring(0, ExportPath.Length - 4);

            File.Copy(LoadPath + "/Original/ssf.ssf", ExportPath + ".ssf", true);
            File.Copy(LoadPath + "/Original/level.pbd", ExportPath + ".pbd", true);
            if (File.Exists(LoadPath + "/Original/sky.pbd"))
            {
                File.Copy(LoadPath + "/Original/ald.ald", ExportPath + ".adl", true);
                File.Copy(LoadPath + "/Original/aip.aip", ExportPath + ".aip", true);
                File.Copy(LoadPath + "/Original/sky.pbd", ExportPath + "_sky.pbd", true);
                File.Copy(LoadPath + "/Original/sop.sop", ExportPath + ".sop", true);
            }

            //Reset mapHandler
            MapHandler mapHandler = new MapHandler();
            mapHandler.Load(LoadPath + "/Original/level.map");

            //Load PBDHandler
            PBDHandler pbdHandler = new PBDHandler();
            pbdHandler.LoadPBD(ExportPath + ".pbd");

            //Rebuild Patches
            patchPoints = new PatchesJsonHandler();
            patchPoints = PatchesJsonHandler.Load(LoadPath + "/Patches.json");
            pbdHandler.Patches = new List<Patch>();
            mapHandler.Patchs = new List<LinkerItem>();
            for (int i = 0; i < patchPoints.Patches.Count; i++)
            {
                Patch patch = new Patch();
                var ImportPatch = patchPoints.Patches[i];
                patch.LightMapPoint = JsonUtil.ArrayToVector4(ImportPatch.LightMapPoint);

                patch.UVPoint1 = new Vector4(ImportPatch.UVPoints[0, 0], ImportPatch.UVPoints[0, 1], 1, 1);
                patch.UVPoint2 = new Vector4(ImportPatch.UVPoints[1, 0], ImportPatch.UVPoints[1, 1], 1, 1);
                patch.UVPoint3 = new Vector4(ImportPatch.UVPoints[2, 0], ImportPatch.UVPoints[2, 1], 1, 1);
                patch.UVPoint4 = new Vector4(ImportPatch.UVPoints[3, 0], ImportPatch.UVPoints[3, 1], 1, 1);

                BezierUtil bezierUtil = new BezierUtil();

                for (int a = 0; a < 16; a++)
                {
                    bezierUtil.RawPoints[a].X = ImportPatch.Points[a, 0];
                    bezierUtil.RawPoints[a].Y = ImportPatch.Points[a, 1];
                    bezierUtil.RawPoints[a].Z = ImportPatch.Points[a, 2];
                }

                bezierUtil.GenerateProcessedPoints();

                patch.R1C1 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[0]);
                patch.R1C2 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[1]);
                patch.R1C3 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[2]);
                patch.R1C4 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[3]);
                patch.R2C1 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[4]);
                patch.R2C2 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[5]);
                patch.R2C3 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[6]);
                patch.R2C4 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[7]);
                patch.R3C1 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[8]);
                patch.R3C2 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[9]);
                patch.R3C3 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[10]);
                patch.R3C4 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[11]);
                patch.R4C1 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[12]);
                patch.R4C2 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[13]);
                patch.R4C3 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[14]);
                patch.R4C4 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[15]);

                Vector3 HighestXYZ = bezierUtil.RawPoints[0];
                HighestXYZ = MathUtil.Highest(HighestXYZ, bezierUtil.RawPoints[1]);
                HighestXYZ = MathUtil.Highest(HighestXYZ, bezierUtil.RawPoints[2]);
                HighestXYZ = MathUtil.Highest(HighestXYZ, bezierUtil.RawPoints[3]);
                HighestXYZ = MathUtil.Highest(HighestXYZ, bezierUtil.RawPoints[4]);
                HighestXYZ = MathUtil.Highest(HighestXYZ, bezierUtil.RawPoints[5]);
                HighestXYZ = MathUtil.Highest(HighestXYZ, bezierUtil.RawPoints[6]);
                HighestXYZ = MathUtil.Highest(HighestXYZ, bezierUtil.RawPoints[7]);
                HighestXYZ = MathUtil.Highest(HighestXYZ, bezierUtil.RawPoints[8]);
                HighestXYZ = MathUtil.Highest(HighestXYZ, bezierUtil.RawPoints[9]);
                HighestXYZ = MathUtil.Highest(HighestXYZ, bezierUtil.RawPoints[10]);
                HighestXYZ = MathUtil.Highest(HighestXYZ, bezierUtil.RawPoints[11]);
                HighestXYZ = MathUtil.Highest(HighestXYZ, bezierUtil.RawPoints[12]);
                HighestXYZ = MathUtil.Highest(HighestXYZ, bezierUtil.RawPoints[13]);
                HighestXYZ = MathUtil.Highest(HighestXYZ, bezierUtil.RawPoints[14]);
                HighestXYZ = MathUtil.Highest(HighestXYZ, bezierUtil.RawPoints[15]);

                Vector3 LowestXYZ = bezierUtil.RawPoints[0];
                LowestXYZ = MathUtil.Lowest(LowestXYZ, bezierUtil.RawPoints[1]);
                LowestXYZ = MathUtil.Lowest(LowestXYZ, bezierUtil.RawPoints[2]);
                LowestXYZ = MathUtil.Lowest(LowestXYZ, bezierUtil.RawPoints[3]);
                LowestXYZ = MathUtil.Lowest(LowestXYZ, bezierUtil.RawPoints[4]);
                LowestXYZ = MathUtil.Lowest(LowestXYZ, bezierUtil.RawPoints[5]);
                LowestXYZ = MathUtil.Lowest(LowestXYZ, bezierUtil.RawPoints[6]);
                LowestXYZ = MathUtil.Lowest(LowestXYZ, bezierUtil.RawPoints[7]);
                LowestXYZ = MathUtil.Lowest(LowestXYZ, bezierUtil.RawPoints[8]);
                LowestXYZ = MathUtil.Lowest(LowestXYZ, bezierUtil.RawPoints[9]);
                LowestXYZ = MathUtil.Lowest(LowestXYZ, bezierUtil.RawPoints[10]);
                LowestXYZ = MathUtil.Lowest(LowestXYZ, bezierUtil.RawPoints[11]);
                LowestXYZ = MathUtil.Lowest(LowestXYZ, bezierUtil.RawPoints[12]);
                LowestXYZ = MathUtil.Lowest(LowestXYZ, bezierUtil.RawPoints[13]);
                LowestXYZ = MathUtil.Lowest(LowestXYZ, bezierUtil.RawPoints[14]);
                LowestXYZ = MathUtil.Lowest(LowestXYZ, bezierUtil.RawPoints[15]);

                patch.HighestXYZ = HighestXYZ;
                patch.LowestXYZ = LowestXYZ;

                patch.Point1 = JsonUtil.Vector3ToVector4(bezierUtil.RawPoints[0]);
                patch.Point2 = JsonUtil.Vector3ToVector4(bezierUtil.RawPoints[12]);
                patch.Point3 = JsonUtil.Vector3ToVector4(bezierUtil.RawPoints[3]);
                patch.Point4 = JsonUtil.Vector3ToVector4(bezierUtil.RawPoints[15]);

                patch.PatchStyle = ImportPatch.PatchStyle;
                patch.Unknown2 = 41;
                if (ImportPatch.TrickOnlyPatch)
                {
                    patch.PatchVisablity = 32768;
                }
                patch.TextureAssigment = ImportPatch.TextureAssigment;

                if (lightingFixObjects.Count - 1 >= patch.TextureAssigment)
                {
                    lightingFixObjects[patch.TextureAssigment].Patch.Add(i);
                }
                patch.LightmapID = ImportPatch.LightmapID;

                pbdHandler.Patches.Add(patch);

                LinkerItem linkerItem = new LinkerItem();
                linkerItem.Name = ImportPatch.PatchName;
                linkerItem.Ref = 1;
                linkerItem.UID = i;
                linkerItem.Hashvalue = MapHandler.GenerateHash(linkerItem.Name);
                mapHandler.Patchs.Add(linkerItem);
            }

            //Rebuild Splines
            splineJsonHandler = new SplineJsonHandler();
            splineJsonHandler = SplineJsonHandler.Load(LoadPath + "/Splines.json");
            pbdHandler.splines = new List<Spline>();
            pbdHandler.splinesSegments = new List<SplinesSegments>();
            mapHandler.Splines = new List<LinkerItem>();
            int SegmentPos = 0;
            for (int i = 0; i < splineJsonHandler.SplineJsons.Count; i++)
            {
                var TempSpline = splineJsonHandler.SplineJsons[i];
                Spline spline = new Spline();
                spline.SplineSegmentPosition = SegmentPos;
                spline.SplineSegmentCount = TempSpline.Segments.Count;
                spline.Unknown1 = TempSpline.Unknown1;
                spline.Unknown2 = TempSpline.Unknown2;

                Vector3 HighestXYZSpline = JsonUtil.ArrayToVector3(TempSpline.Segments[0].Point1);
                Vector3 LowestXYZSpline = JsonUtil.ArrayToVector3(TempSpline.Segments[0].Point1);
                float PreviousSegmentDiffrence = 0f;
                for (int a = 0; a < TempSpline.Segments.Count; a++)
                {
                    SplinesSegments segments = new SplinesSegments();
                    var TempSegment = TempSpline.Segments[a];
                    BezierUtil bezierUtil = new BezierUtil();

                    bezierUtil.RawPoints[0] = JsonUtil.ArrayToVector3(TempSegment.Point1);
                    bezierUtil.RawPoints[1] = JsonUtil.ArrayToVector3(TempSegment.Point2);
                    bezierUtil.RawPoints[2] = JsonUtil.ArrayToVector3(TempSegment.Point3);
                    bezierUtil.RawPoints[3] = JsonUtil.ArrayToVector3(TempSegment.Point4);

                    bezierUtil.GenerateProcessedPoints();

                    segments.ControlPoint = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[0]);
                    segments.Point2 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[1], 0);
                    segments.Point3 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[2], 0);
                    segments.Point4 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[3], 0);
                    segments.ScalingPoint = JsonUtil.ArrayToVector4(TempSegment.Unknown);


                    if (a == 0)
                    {
                        segments.PreviousSegment = -1;
                    }
                    else
                    {
                        segments.PreviousSegment = a - 1;
                    }
                    if (a == TempSpline.Segments.Count - 1)
                    {
                        segments.NextSegment = -1;
                    }
                    else
                    {
                        segments.NextSegment = a + 1;
                    }
                    segments.SplineParent = i;

                    Vector3 HighestXYZSegment = bezierUtil.RawPoints[0];
                    HighestXYZSegment = MathUtil.Highest(HighestXYZSegment, bezierUtil.RawPoints[1]);
                    HighestXYZSegment = MathUtil.Highest(HighestXYZSegment, bezierUtil.RawPoints[2]);
                    HighestXYZSegment = MathUtil.Highest(HighestXYZSegment, bezierUtil.RawPoints[3]);

                    segments.HighestXYZ = HighestXYZSegment;
                    HighestXYZSpline = MathUtil.Highest(HighestXYZSpline, HighestXYZSegment);

                    Vector3 LowestXYZSegment = bezierUtil.RawPoints[0];
                    LowestXYZSegment = MathUtil.Lowest(LowestXYZSegment, bezierUtil.RawPoints[1]);
                    LowestXYZSegment = MathUtil.Lowest(LowestXYZSegment, bezierUtil.RawPoints[2]);
                    LowestXYZSegment = MathUtil.Lowest(LowestXYZSegment, bezierUtil.RawPoints[3]);

                    segments.LowestXYZ = LowestXYZSegment;
                    LowestXYZSpline = MathUtil.Highest(LowestXYZSpline, LowestXYZSegment);

                    segments.SegmentDisatnce = JsonUtil.GenerateDistance(bezierUtil.RawPoints[0], bezierUtil.RawPoints[1], bezierUtil.RawPoints[2], bezierUtil.RawPoints[3]);
                    segments.PreviousSegmentsDistance = PreviousSegmentDiffrence;
                    PreviousSegmentDiffrence += segments.SegmentDisatnce;
                    segments.Unknown32 = TempSegment.Unknown32;
                    pbdHandler.splinesSegments.Add(segments);
                    SegmentPos++;
                }

                spline.LowestXYZ = LowestXYZSpline;
                spline.HighestXYZ = HighestXYZSpline;


                LinkerItem linkerItem = new LinkerItem();
                linkerItem.Name = TempSpline.SplineName;
                linkerItem.Ref = 1;
                linkerItem.UID = i;
                linkerItem.Hashvalue = MapHandler.GenerateHash(linkerItem.Name);
                mapHandler.Splines.Add(linkerItem);
                pbdHandler.splines.Add(spline);
            }

            //Rebuild Instances
            instancesJson = new InstanceJsonHandler();
            instancesJson = InstanceJsonHandler.Load(LoadPath + "/Instances.json");
            pbdHandler.Instances = new List<Instance>();
            mapHandler.InternalInstances = new List<LinkerItem>();
            for (int i = 0; i < instancesJson.Instances.Count; i++)
            {
                var Oldinstance = instancesJson.Instances[i];
                Instance NewInstance = new Instance();

                Matrix4x4 scale = Matrix4x4.CreateScale(JsonUtil.ArrayToVector3(Oldinstance.Scale));
                Matrix4x4 Rotation = Matrix4x4.CreateFromQuaternion(JsonUtil.ArrayToQuaternion(Oldinstance.Rotation));
                Matrix4x4 matrix4X4 = Matrix4x4.Multiply(scale, Rotation);
                matrix4X4.Translation = JsonUtil.ArrayToVector3(Oldinstance.Location);

                NewInstance.matrix4X4 = matrix4X4;

                NewInstance.Unknown5 = JsonUtil.ArrayToVector4(Oldinstance.Unknown5);
                NewInstance.Unknown6 = JsonUtil.ArrayToVector4(Oldinstance.Unknown6);
                NewInstance.Unknown7 = JsonUtil.ArrayToVector4(Oldinstance.Unknown7);
                NewInstance.Unknown8 = JsonUtil.ArrayToVector4(Oldinstance.Unknown8);
                NewInstance.Unknown9 = JsonUtil.ArrayToVector4(Oldinstance.Unknown9);
                NewInstance.Unknown10 = JsonUtil.ArrayToVector4(Oldinstance.Unknown10);
                NewInstance.Unknown11 = JsonUtil.ArrayToVector4(Oldinstance.Unknown11);
                NewInstance.RGBA = JsonUtil.ArrayToVector4(Oldinstance.RGBA);

                NewInstance.ModelID = Oldinstance.ModelID;
                NewInstance.PrevInstance = Oldinstance.PrevInstance;
                NewInstance.NextInstance = Oldinstance.NextInstance;

                NewInstance.UnknownInt26 = Oldinstance.UnknownInt26;
                NewInstance.UnknownInt27 = Oldinstance.UnknownInt27;
                NewInstance.UnknownInt28 = Oldinstance.UnknownInt28;
                NewInstance.ModelID2 = Oldinstance.ModelID2;
                NewInstance.UnknownInt30 = Oldinstance.UnknownInt30;
                NewInstance.UnknownInt31 = Oldinstance.UnknownInt31;
                NewInstance.UnknownInt32 = Oldinstance.UnknownInt32;

                NewInstance.LTGState = Oldinstance.LTGState;
                pbdHandler.Instances.Add(NewInstance);

                LinkerItem linkerItem = new LinkerItem();
                linkerItem.Ref = 1;
                linkerItem.UID = i;
                linkerItem.Name = Oldinstance.InstanceName;
                linkerItem.Hashvalue = MapHandler.GenerateHash(linkerItem.Name);
                mapHandler.InternalInstances.Add(linkerItem);
            }

            //Rebuild Material Blocks
            prefabJsonHandler = new PrefabJsonHandler();
            prefabJsonHandler = PrefabJsonHandler.Load(LoadPath + "/Prefabs.json");
            pbdHandler.materialBlocks = new List<MaterialBlock>();
            for (int i = 0; i < prefabJsonHandler.PrefabJsons.Count; i++)
            {
                var TempPrefab = prefabJsonHandler.PrefabJsons[i];
                var NewMaterialBlock = new MaterialBlock();

                NewMaterialBlock.ints = new List<int>();

                for (int a = 0; a < prefabJsonHandler.PrefabJsons[i].PrefabObjects.Count; a++)
                {
                    var TempObject = TempPrefab.PrefabObjects[a];
                    for (int b = 0; b < prefabJsonHandler.PrefabJsons[i].PrefabObjects[a].MeshData.Count; b++)
                    {
                        var TempMesh = TempObject.MeshData[b];
                        NewMaterialBlock.ints.Add(prefabJsonHandler.PrefabJsons[i].PrefabObjects[a].MeshData[b].MaterialID);

                        TempMesh.MaterialID = NewMaterialBlock.ints.Count - 1;

                        TempObject.MeshData[b] = TempMesh;
                    }
                    TempPrefab.PrefabObjects[a] = TempObject;
                }
                prefabJsonHandler.PrefabJsons[i] = TempPrefab;
                pbdHandler.materialBlocks.Add(NewMaterialBlock);
            }

            //Rebuild Prefabs
            pbdHandler.PrefabData = new List<Prefabs>();
            for (int i = 0; i < prefabJsonHandler.PrefabJsons.Count; i++)
            {
                var NewPrefab = new Prefabs();
                var TempPrefab = prefabJsonHandler.PrefabJsons[i];
                NewPrefab.MaterialBlockID = i;

                NewPrefab.Unknown3 = prefabJsonHandler.PrefabJsons[i].Unknown3;
                NewPrefab.AnimTime = prefabJsonHandler.PrefabJsons[i].AnimTime;

                //MeshCount
                //VertexCount
                //TristirpCount
                //Unknown4
                //NonTriCunt

                NewPrefab.PrefabObjects = new List<ObjectHeader>();
                for (int a = 0; a < TempPrefab.PrefabObjects.Count; a++)
                {
                    var TempObject = TempPrefab.PrefabObjects[a];
                    var NewObject = new ObjectHeader();
                    NewObject.ParentID = TempObject.ParentID;

                    if (TempObject.IncludeMatrix)
                    {
                        NewObject.IncludeMatrix = true;
                        Matrix4x4 scale = Matrix4x4.CreateScale(JsonUtil.ArrayToVector3(TempObject.Scale));
                        Matrix4x4 Rotation = Matrix4x4.CreateFromQuaternion(JsonUtil.ArrayToQuaternion(TempObject.Rotation));
                        Matrix4x4 matrix4X4 = Matrix4x4.Multiply(scale, Rotation);

                        matrix4X4.Translation = JsonUtil.ArrayToVector3(TempObject.Position);
                        NewObject.matrix4X4 = matrix4X4;
                    }

                    ObjectData NewObjectData = new ObjectData();

                    NewObjectData.Flags = TempObject.Flags;
                    //MeshCount
                    //FaceCount

                    if (TempObject.MeshData.Count != 0)
                    {
                        NewObjectData.MeshOffsets = new List<MeshOffsets>();

                        for (int b = 0; b < TempObject.MeshData.Count; b++)
                        {
                            var NewMeshOffset = new MeshOffsets();
                            NewMeshOffset.MeshID = TempObject.MeshData[b].MeshID;
                            NewMeshOffset.MaterialBlockPos = TempObject.MeshData[b].MaterialID;
                            NewMeshOffset.MeshPath = TempObject.MeshData[b].MeshPath;
                            NewObjectData.MeshOffsets.Add(NewMeshOffset);
                        }
                    }

                    if (TempObject.IncludeAnimation)
                    {
                        NewObject.IncludeAnimation = true;
                        NewObject.objectAnimation = new ObjectAnimation();
                        NewObject.objectAnimation.U1 = TempObject.Animation.U1;
                        NewObject.objectAnimation.U2 = TempObject.Animation.U2;
                        NewObject.objectAnimation.U3 = TempObject.Animation.U3;
                        NewObject.objectAnimation.U4 = TempObject.Animation.U4;
                        NewObject.objectAnimation.U5 = TempObject.Animation.U5;
                        NewObject.objectAnimation.U6 = TempObject.Animation.U6;

                        NewObject.objectAnimation.AnimationAction = TempObject.Animation.AnimationAction;
                        NewObject.objectAnimation.animationEntries = new List<AnimationEntry>();
                        if (TempObject.Animation.AnimationEntries != null)
                        {
                            for (int b = 0; b < TempObject.Animation.AnimationEntries.Count; b++)
                            {
                                var TempAnimationEntry = TempObject.Animation.AnimationEntries[b];
                                var NewAnimationEntry = new AnimationEntry();
                                NewAnimationEntry.animationMaths = new List<AnimationMath>();
                                for (int c = 0; c < TempAnimationEntry.AnimationMaths.Count; c++)
                                {
                                    var TempAnimationMaths = TempAnimationEntry.AnimationMaths[c];
                                    var NewAnimationMath = new AnimationMath();

                                    NewAnimationMath.Value1 = TempAnimationMaths.Value1;
                                    NewAnimationMath.Value2 = TempAnimationMaths.Value2;
                                    NewAnimationMath.Value3 = TempAnimationMaths.Value3;
                                    NewAnimationMath.Value4 = TempAnimationMaths.Value4;
                                    NewAnimationMath.Value5 = TempAnimationMaths.Value5;
                                    NewAnimationMath.Value6 = TempAnimationMaths.Value6;


                                    NewAnimationEntry.animationMaths.Add(NewAnimationMath);
                                }

                                NewObject.objectAnimation.animationEntries.Add(NewAnimationEntry);
                            }
                        }
                    }

                    NewObject.objectData = NewObjectData;

                    NewPrefab.PrefabObjects.Add(NewObject);
                }
                pbdHandler.PrefabData.Add(NewPrefab);
            }

            //Rebuild Materials
            materialJson = new MaterialJsonHandler();
            materialJson = MaterialJsonHandler.Load(LoadPath + "/Material.json");
            pbdHandler.materials = new List<TrickyMaterial>();
            mapHandler.Materials = new List<LinkerItem>();
            for (int i = 0; i < materialJson.MaterialsJsons.Count; i++)
            {
                var NewMaterial = new TrickyMaterial();

                NewMaterial.TextureID = materialJson.MaterialsJsons[i].TextureID;
                if (AttemptLightingFix && NewMaterial.TextureID != -1)
                {
                    if (lightingFixObjects.Count - 1 >= NewMaterial.TextureID)
                    {
                        lightingFixObjects[NewMaterial.TextureID].Object.Add(i);
                    }
                }
                NewMaterial.UnknownInt2 = materialJson.MaterialsJsons[i].UnknownInt2;
                NewMaterial.UnknownInt3 = materialJson.MaterialsJsons[i].UnknownInt3;
                NewMaterial.UnknownFloat1 = materialJson.MaterialsJsons[i].UnknownFloat1;
                NewMaterial.UnknownFloat2 = materialJson.MaterialsJsons[i].UnknownFloat2;
                NewMaterial.UnknownFloat3 = materialJson.MaterialsJsons[i].UnknownFloat3;
                NewMaterial.UnknownFloat4 = materialJson.MaterialsJsons[i].UnknownFloat4;
                NewMaterial.UnknownInt8 = materialJson.MaterialsJsons[i].UnknownInt8;
                NewMaterial.UnknownFloat5 = materialJson.MaterialsJsons[i].UnknownFloat5;

                NewMaterial.UnknownFloat6 = materialJson.MaterialsJsons[i].UnknownFloat6;
                NewMaterial.UnknownFloat7 = materialJson.MaterialsJsons[i].UnknownFloat7;
                NewMaterial.UnknownFloat8 = materialJson.MaterialsJsons[i].UnknownFloat8;

                NewMaterial.UnknownInt13 = materialJson.MaterialsJsons[i].UnknownInt13;
                NewMaterial.UnknownInt14 = materialJson.MaterialsJsons[i].UnknownInt14;
                NewMaterial.UnknownInt15 = materialJson.MaterialsJsons[i].UnknownInt15;
                NewMaterial.UnknownInt16 = materialJson.MaterialsJsons[i].UnknownInt16;
                NewMaterial.UnknownInt17 = materialJson.MaterialsJsons[i].UnknownInt17;
                NewMaterial.UnknownInt18 = materialJson.MaterialsJsons[i].UnknownInt18;
                NewMaterial.TextureFlipbookID = materialJson.MaterialsJsons[i].TextureFlipbookID;
                NewMaterial.UnknownInt20 = materialJson.MaterialsJsons[i].UnknownInt20;

                pbdHandler.materials.Add(NewMaterial);

                LinkerItem linkerItem = new LinkerItem();
                linkerItem.Name = materialJson.MaterialsJsons[i].MaterialName;
                linkerItem.Ref = -1; //FIX Later
                linkerItem.UID = i;
                linkerItem.Hashvalue = MapHandler.GenerateHash(linkerItem.Name);
                mapHandler.Materials.Add(linkerItem);
            }

            //Rebuild TextureFlipbook
            textureFlipbookJsonHandler = new TextureFlipbookJsonHandler();
            textureFlipbookJsonHandler = TextureFlipbookJsonHandler.Load(LoadPath + "/TextureFlipbook.json");
            pbdHandler.textureFlipbooks = new List<TextureFlipbook>();
            for (int i = 0; i < textureFlipbookJsonHandler.FlipbookJsons.Count; i++)
            {
                TextureFlipbook textureFlipbook = new TextureFlipbook();

                textureFlipbook.ImagePositions = pbdHandler.textureFlipbooks[i].ImagePositions;

                pbdHandler.textureFlipbooks.Add(textureFlipbook);
            }

            //Rebuild Lights
            lightJsonHandler = new LightJsonHandler();
            lightJsonHandler = LightJsonHandler.Load(LoadPath + "/Lights.json");
            pbdHandler.lights = new List<Light>();
            for (int i = 0; i < lightJsonHandler.LightJsons.Count; i++)
            {
                Light TempLight = new Light();
                TempLight.Type = lightJsonHandler.LightJsons[i].Type;
                TempLight.spriteRes = lightJsonHandler.LightJsons[i].SpriteRes;
                TempLight.UnknownFloat1 = lightJsonHandler.LightJsons[i].UnknownFloat1;
                TempLight.UnknownInt1 = lightJsonHandler.LightJsons[i].UnknownInt1;
                TempLight.Colour = JsonUtil.ArrayToVector3(lightJsonHandler.LightJsons[i].Colour);
                TempLight.Direction = JsonUtil.ArrayToVector3(lightJsonHandler.LightJsons[i].Direction);
                TempLight.Postion = JsonUtil.ArrayToVector3(lightJsonHandler.LightJsons[i].Postion);
                TempLight.LowestXYZ = JsonUtil.ArrayToVector3(lightJsonHandler.LightJsons[i].LowestXYZ);
                TempLight.HighestXYZ = JsonUtil.ArrayToVector3(lightJsonHandler.LightJsons[i].HighestXYZ);
                TempLight.UnknownFloat2 = lightJsonHandler.LightJsons[i].UnknownFloat2;
                TempLight.UnknownInt2 = lightJsonHandler.LightJsons[i].UnknownInt2;
                TempLight.UnknownFloat3 = lightJsonHandler.LightJsons[i].UnknownFloat3;
                TempLight.UnknownInt3 = lightJsonHandler.LightJsons[i].UnknownInt3;

                pbdHandler.lights.Add(TempLight);
            }

            mapHandler.Save(ExportPath + ".map");

            pbdHandler.ImportMeshes(LoadPath + "\\Models");
            //Regenerate Instance Lowest Highest
            pbdHandler.RegenerateLowestAndHighest();

            LTGHandler ltgHandler = new LTGHandler();
            ltgHandler.LoadLTG(LoadPath + "/Original/ltg.ltg");
            ltgHandler.RegenerateLTG(pbdHandler);
            ltgHandler.SaveLTGFile(ExportPath + ".ltg");


            //Build Textures
            OldSSHHandler TextureHandler = new OldSSHHandler();
            TextureHandler.format = "G278";

            string[] ImageFiles = Directory.GetFiles(LoadPath + "/Textures", "*.png");
            for (int i = 0; i < ImageFiles.Length; i++)
            {
                TextureHandler.AddImage();
                TextureHandler.LoadSingle(ImageFiles[i], i);
                if (!AttemptLightingFix)
                {
                    TextureHandler.DarkenImage(i);
                }
                var temp = TextureHandler.sshImages[i];
                temp.shortname = i.ToString().PadLeft(4, '0');
                temp.AlphaFix = true;
                TextureHandler.sshImages[i] = temp;
            }

            if (AttemptLightingFix)
            {
                int OldCount = TextureHandler.sshImages.Count;
                for (int i = 0; i < OldCount; i++)
                {
                    if (lightingFixObjects[i].Object.Count != 0 && lightingFixObjects[i].Patch.Count != 0)
                    {
                        var TempImage = TextureHandler.sshImages[i];
                        TempImage.shortname = (TextureHandler.sshImages.Count).ToString().PadLeft(4, '0');
                        for (int a = 0; a < lightingFixObjects[i].Object.Count; a++)
                        {
                            var TempMatieral = pbdHandler.materials[lightingFixObjects[i].Object[a]];
                            TempMatieral.TextureID = TextureHandler.sshImages.Count;
                            pbdHandler.materials[lightingFixObjects[i].Object[a]] = TempMatieral;
                        }
                        TextureHandler.sshImages.Add(TempImage);
                    }
                    else if (lightingFixObjects[i].Patch.Count == 0)
                    {
                        TextureHandler.DarkenImage(i);
                    }
                }

                for (int i = OldCount; i < TextureHandler.sshImages.Count; i++)
                {
                    TextureHandler.DarkenImage(i);
                }
            }

            pbdHandler.SaveNew(ExportPath + ".pbd");
            TextureHandler.SaveSSH(ExportPath + ".ssh", true);


            string[] SkyboxFiles = Directory.GetFiles(LoadPath + "\\Skybox\\Textures", "*.png");
            if (SkyboxFiles.Length != 0)
            {
                //Build Skybox
                OldSSHHandler SkyboxHandler = new OldSSHHandler();
                SkyboxHandler.format = "G278";

                for (int i = 0; i < SkyboxFiles.Length; i++)
                {
                    SkyboxHandler.AddImage();
                    SkyboxHandler.LoadSingle(SkyboxFiles[i], i);
                    SkyboxHandler.DarkenImage(i);
                    var temp = SkyboxHandler.sshImages[i];
                    temp.shortname = i.ToString().PadLeft(4, '0');
                    temp.AlphaFix = true;
                    SkyboxHandler.sshImages[i] = temp;
                }

                SkyboxHandler.SaveSSH(ExportPath + "_sky.ssh", true);
            }

            OldSSHHandler LightmapHandler = new OldSSHHandler();
            //Build Lightmap
            if (!Unilightmap)
            {
                LightmapHandler.format = "G278";

                string[] LightmapFiles = Directory.GetFiles(LoadPath + "/Lightmaps", "*.png");
                for (int i = 0; i < LightmapFiles.Length; i++)
                {
                    LightmapHandler.AddImage();
                    var temp = LightmapHandler.sshImages[i];
                    temp.sshHeader.MatrixFormat = 5;
                    LightmapHandler.sshImages[i] = temp;
                    LightmapHandler.LoadSingle(LightmapFiles[i], i);
                    if (!AttemptLightingFix)
                    {
                        LightmapHandler.DarkenImage(i);
                    }
                    temp = LightmapHandler.sshImages[i];
                    temp.shortname = i.ToString().PadLeft(4, '0');
                    //temp.AlphaFix = true;
                    LightmapHandler.sshImages[i] = temp;
                }
            }
            else
            {
                pbdHandler = LightmapGenerator.GenerateNewLightmapPoints(pbdHandler);
                LightmapHandler = LightmapGenerator.GenerateUnlitLightmap(pbdHandler);
            }

            LightmapHandler.SaveSSH(ExportPath + "_L.ssh", true);

            //Edit AIP
            AIPSOPJsonHandler aipJsonHandler = AIPSOPJsonHandler.Load(LoadPath + "/AIP.json");
            AIPSOPHandler aipHandler = new AIPSOPHandler();
            aipHandler.typeAs = new List<AIPSOPHandler.PathTypeA>();
            for (int i = 0; i < aipJsonHandler.PathTypeA.Count; i++)
            {
                var TempPath = aipJsonHandler.PathTypeA[i];
                var NewPath = new AIPSOPHandler.PathTypeA();

                NewPath.Unknown1 = TempPath.Unknown1;
                NewPath.Unknown2 = TempPath.Unknown2;
                NewPath.Unknown3 = TempPath.Unknown3;
                NewPath.Unknown4 = TempPath.Unknown4;
                NewPath.Unknown5 = TempPath.Unknown5;
                NewPath.Unknown6 = TempPath.Unknown6;
                NewPath.Unknown7 = TempPath.Unknown7;

                NewPath.pathPos = JsonUtil.ArrayToVector3(TempPath.pathPos);

                Vector3 bboxMin = NewPath.pathPos;
                Vector3 bboxMax = NewPath.pathPos;
                for (int a = 0; a < TempPath.vectorPoints.Count; a++)
                {
                    bboxMin = MathUtil.Lowest(bboxMin, JsonUtil.ArrayToVector3(TempPath.vectorPoints[a]));
                    bboxMax = MathUtil.Highest(bboxMax, JsonUtil.ArrayToVector3(TempPath.vectorPoints[a]));
                }
                NewPath.bboxMin = bboxMin;
                NewPath.bboxMax = bboxMax;

                Vector3 CurrentPosition = NewPath.pathPos;
                NewPath.vectorPoints = new List<Vector4>();
                for (int a = 0; a < TempPath.vectorPoints.Count; a++)
                {
                    Vector3 NewPosition = JsonUtil.ArrayToVector3(TempPath.vectorPoints[a]);

                    Vector3 Direction = Vector3.Normalize(NewPosition - CurrentPosition);
                    float Distance = Vector3.Distance(NewPosition, CurrentPosition);

                    Vector4 NewPointVector = JsonUtil.Vector3ToVector4(Direction, Distance);
                    NewPath.vectorPoints.Add(NewPointVector);
                    CurrentPosition = NewPosition;
                }

                NewPath.unkownListTypeAs = new List<AIPSOPHandler.UnkownListTypeA>();
                for (int a = 0; a < TempPath.unkownListTypeAs.Count; a++)
                {
                    var NewListTypeA = new AIPSOPHandler.UnkownListTypeA();

                    NewListTypeA.Unknown1 = TempPath.unkownListTypeAs[a].Unknown1;
                    NewListTypeA.Unknown2 = TempPath.unkownListTypeAs[a].Unknown2;
                    NewListTypeA.Unknown3 = TempPath.unkownListTypeAs[a].Unknown3;
                    NewListTypeA.Unknown4 = TempPath.unkownListTypeAs[a].Unknown4;

                    NewPath.unkownListTypeAs.Add(NewListTypeA);
                }

                aipHandler.typeAs.Add(NewPath);
            }

            aipHandler.typeBs = new List<AIPSOPHandler.PathTypeB>();
            for (int i = 0; i < aipJsonHandler.PathTypeB.Count; i++)
            {
                var TempPath = aipJsonHandler.PathTypeB[i];
                var NewPath = new AIPSOPHandler.PathTypeB();

                NewPath.Unknown1 = TempPath.Unknown1;
                NewPath.Unknown2 = TempPath.Unknown2;
                NewPath.Unknown3 = TempPath.Unknown3;
                NewPath.Unknown4 = TempPath.Unknown4;

                NewPath.pathPos = JsonUtil.ArrayToVector3(TempPath.pathPos);

                Vector3 bboxMin = NewPath.pathPos;
                Vector3 bboxMax = NewPath.pathPos;
                for (int a = 0; a < TempPath.vectorPoints.Count; a++)
                {
                    bboxMin = MathUtil.Lowest(bboxMin, JsonUtil.ArrayToVector3(TempPath.vectorPoints[a]));
                    bboxMax = MathUtil.Highest(bboxMax, JsonUtil.ArrayToVector3(TempPath.vectorPoints[a]));
                }
                NewPath.bboxMin = bboxMin;
                NewPath.bboxMax = bboxMax;

                Vector3 CurrentPosition = NewPath.pathPos;
                NewPath.vectorPoints = new List<Vector4>();
                for (int a = 0; a < TempPath.vectorPoints.Count; a++)
                {
                    Vector3 NewPosition = JsonUtil.ArrayToVector3(TempPath.vectorPoints[a]);

                    Vector3 Direction = Vector3.Normalize(NewPosition - CurrentPosition);
                    float Distance = Vector3.Distance(NewPosition, CurrentPosition);

                    Vector4 NewPointVector = JsonUtil.Vector3ToVector4(Direction, Distance);
                    NewPath.vectorPoints.Add(NewPointVector);
                    CurrentPosition = NewPosition;
                }

                NewPath.unkownListTypeAs = new List<AIPSOPHandler.UnkownListTypeA>();
                for (int a = 0; a < TempPath.unkownListTypeAs.Count; a++)
                {
                    var NewListTypeA = new AIPSOPHandler.UnkownListTypeA();

                    NewListTypeA.Unknown1 = TempPath.unkownListTypeAs[a].Unknown1;
                    NewListTypeA.Unknown2 = TempPath.unkownListTypeAs[a].Unknown2;
                    NewListTypeA.Unknown3 = TempPath.unkownListTypeAs[a].Unknown3;
                    NewListTypeA.Unknown4 = TempPath.unkownListTypeAs[a].Unknown4;

                    NewPath.unkownListTypeAs.Add(NewListTypeA);
                }

                aipHandler.typeBs.Add(NewPath);
            }

            //aipHandler.SaveAIPSOP(ExportPath + ".aip");

        }

        public void LoadAndVerifyFiles(string LoadPath)
        {
            //Create Patches JSON
            patchPoints = PatchesJsonHandler.Load(LoadPath + "/Patches.json");

            //Create Instance JSON
            instancesJson = InstanceJsonHandler.Load(LoadPath + "/Instances.json");

            //Create Particle Instances JSON
            particleInstanceJson = ParticleInstanceJsonHandler.Load(LoadPath + "/ParticleInstances.json");

            //Create Material Json
            materialJson = MaterialJsonHandler.Load(LoadPath + "/Material.json");

            //Create Lights Json
            lightJsonHandler = LightJsonHandler.Load(LoadPath + "/Lights.json");

            //Create Spline Json
            splineJsonHandler = SplineJsonHandler.Load(LoadPath + "/Splines.json");

            //Create Texture FLipbook Json
            textureFlipbookJsonHandler = TextureFlipbookJsonHandler.Load(LoadPath + "/TextureFlipbook.json");

            //Create Model Json
            prefabJsonHandler = PrefabJsonHandler.Load(LoadPath + "/Prefabs.json");

            //Create Particle Model Json
            particleModelJsonHandler = ParticleModelJsonHandler.Load(LoadPath + "/ParticleModelHeaders.json");
        }
    }
}
