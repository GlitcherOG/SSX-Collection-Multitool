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
        //Exporting Options
        public bool InlineExporting;


        //Importing Options
        public bool PBDGenerate = true;
        public bool SSHGenerate = true;
        public bool LSSHGenerate = true;
        public bool LTGGenerate = true;
        public bool MAPGenerate = true;
        public bool SkyPBDGenerate = true;
        public bool SkySSHGenerate = true;
        public bool ADLGenerate = true;
        public bool SSFGenerate = true;
        public bool AIPGenerate = true;
        public bool SOPGenerate = true;

        public bool Unilightmap;

        //PBD JSONS
        public PatchesJsonHandler patchPoints = new PatchesJsonHandler();
        public InstanceJsonHandler instancesJson = new InstanceJsonHandler();
        public ParticleInstanceJsonHandler particleInstanceJson = new ParticleInstanceJsonHandler();
        public MaterialJsonHandler materialJson = new MaterialJsonHandler();
        public LightJsonHandler lightJsonHandler = new LightJsonHandler();
        public SplineJsonHandler splineJsonHandler = new SplineJsonHandler();
        public PrefabJsonHandler prefabJsonHandler = new PrefabJsonHandler();
        public ParticleModelJsonHandler particleModelJsonHandler = new ParticleModelJsonHandler();
        public CameraJSONHandler cameraJSONHandler  = new CameraJSONHandler();
        public SSFJsonHandler ssfJsonHandler = new SSFJsonHandler();

        //Skybox JSON

        public MaterialJsonHandler SkyMaterialJson = new MaterialJsonHandler();
        public PrefabJsonHandler SkyPrefabJsonHandler = new PrefabJsonHandler();



        public void ExtractTrickyLevelFiles(string LoadPath, string ExportPath)
        {
            ADLHandler adlHandler = new ADLHandler();
            if (File.Exists(LoadPath + ".adl"))
            {
                adlHandler.Load(LoadPath + ".adl");
            }

            SSFHandler ssfHandler = new SSFHandler();
            ssfHandler.Load(LoadPath + ".ssf");
            ssfHandler.Save(LoadPath + ".ssf1");

            //Load Map
            MapHandler mapHandler = new MapHandler();
            mapHandler.Load(LoadPath + ".map");

            //Load PBD
            PBDHandler pbdHandler = new PBDHandler();
            pbdHandler.LoadPBD(LoadPath + ".pbd");

            OldSSHHandler TextureHandler = new OldSSHHandler();
            TextureHandler.LoadSSH(LoadPath + ".ssh");

            LTGHandler ltgHandler = new LTGHandler();
            ltgHandler.LoadLTG(LoadPath + ".ltg");

            #region PBD JSON DATA
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
                patch.TexturePath = TextureHandler.sshImages[pbdHandler.Patches[i].TextureAssigment].shortname + ".png";
                patch.LightmapID = pbdHandler.Patches[i].LightmapID;
                patchPoints.Patches.Add(patch);
            }
            patchPoints.CreateJson(ExportPath + "/Patches.json", InlineExporting);

            bool[] ADLTest = new bool[adlHandler.HashSounds.Count];

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

                //LTG
                instanceJson.LTGState = ltgHandler.FindIfInstaneState(i);

                //Find Hash
                int FindHash = -1;
                for (int a = 0; a < pbdHandler.hashData.InstanceHash.Count; a++)
                {
                    if(i== pbdHandler.hashData.InstanceHash[a].ObjectUID)
                    {
                        FindHash = pbdHandler.hashData.InstanceHash[a].Hash;
                        instanceJson.Hash = FindHash;
                        break;
                    }
                }

                //ADL
                if (FindHash != -1)
                {
                    for (int a = 0; a < adlHandler.HashSounds.Count; a++)
                    {
                        if (FindHash == adlHandler.HashSounds[a].Hash)
                        {
                            ADLTest[a] = true;

                            instanceJson.IncludeSound = true;
                            var NewSound = new SoundData();

                            NewSound.CollisonSound = adlHandler.HashSounds[a].Sound.CollisonSound;
                            NewSound.ExternalSounds = new List<ExternalSound>();

                            for (int b = 0; b < adlHandler.HashSounds[a].Sound.ExternalSounds.Count; b++)
                            {
                                var NewExternalSound = new ExternalSound();
                                NewExternalSound.U0 = adlHandler.HashSounds[a].Sound.ExternalSounds[b].U0;
                                NewExternalSound.SoundIndex = adlHandler.HashSounds[a].Sound.ExternalSounds[b].SoundIndex;
                                NewExternalSound.U2 = adlHandler.HashSounds[a].Sound.ExternalSounds[b].U2;
                                NewExternalSound.U3 = adlHandler.HashSounds[a].Sound.ExternalSounds[b].U3;
                                NewExternalSound.U4 = adlHandler.HashSounds[a].Sound.ExternalSounds[b].U4;
                                NewExternalSound.U5 = adlHandler.HashSounds[a].Sound.ExternalSounds[b].U5;
                                NewExternalSound.U6 = adlHandler.HashSounds[a].Sound.ExternalSounds[b].U6;
                                NewSound.ExternalSounds.Add(NewExternalSound);
                            }
                            instanceJson.Sounds = NewSound;
                            break;
                        }
                    }
                }

                //SSF
                instanceJson.U0 = ssfHandler.ObjectProperties[ssfHandler.InstanceState[i]].U0;
                instanceJson.PlayerBounce = ssfHandler.ObjectProperties[ssfHandler.InstanceState[i]].PlayerBounce;
                instanceJson.U2 = ssfHandler.ObjectProperties[ssfHandler.InstanceState[i]].U2;
                instanceJson.U22 = ssfHandler.ObjectProperties[ssfHandler.InstanceState[i]].U22;
                instanceJson.U23 = ssfHandler.ObjectProperties[ssfHandler.InstanceState[i]].U23;
                instanceJson.U24 = ssfHandler.ObjectProperties[ssfHandler.InstanceState[i]].U24;
                instanceJson.U4 = ssfHandler.ObjectProperties[ssfHandler.InstanceState[i]].U4;
                instanceJson.CollsionMode = ssfHandler.ObjectProperties[ssfHandler.InstanceState[i]].CollsionMode;
                instanceJson.EffectSlotIndex = ssfHandler.ObjectProperties[ssfHandler.InstanceState[i]].EffectSlotIndex;
                instanceJson.PhysicsIndex = ssfHandler.ObjectProperties[ssfHandler.InstanceState[i]].PhysicsIndex;
                instanceJson.U8 = ssfHandler.ObjectProperties[ssfHandler.InstanceState[i]].U8;

                int CollsionPos = ssfHandler.ObjectProperties[ssfHandler.InstanceState[i]].CollisonModelIndex;
                if (CollsionPos!=-1 && instanceJson.CollsionMode!=3)
                {
                    instanceJson.CollsionModelPaths = new string[ssfHandler.CollisonModelPointers[CollsionPos].Models.Count];

                    for (int a = 0; a < instanceJson.CollsionModelPaths.Length; a++)
                    {
                        instanceJson.CollsionModelPaths[a] = ssfHandler.CollisonModelPointers[CollsionPos].Models[a].MeshPath;
                    }
                }

                instancesJson.Instances.Add(instanceJson);
            }
            instancesJson.CreateJson(ExportPath + "/Instances.json", InlineExporting);

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
                TempParticle.LowestXYZ = JsonUtil.Vector3ToArray(pbdHandler.particleInstances[i].LowestXYZ);
                TempParticle.HighestXYZ = JsonUtil.Vector3ToArray(pbdHandler.particleInstances[i].HighestXYZ);
                TempParticle.UnknownInt8 = pbdHandler.particleInstances[i].UnknownInt8;
                TempParticle.UnknownInt9 = pbdHandler.particleInstances[i].UnknownInt9;
                TempParticle.UnknownInt10 = pbdHandler.particleInstances[i].UnknownInt10;
                TempParticle.UnknownInt11 = pbdHandler.particleInstances[i].UnknownInt11;
                TempParticle.UnknownInt12 = pbdHandler.particleInstances[i].UnknownInt12;
                particleInstanceJson.Particles.Add(TempParticle);
            }
            particleInstanceJson.CreateJson(ExportPath + "/ParticleInstances.json", InlineExporting);

            //Create Material Json
            materialJson = new MaterialJsonHandler();
            for (int i = 0; i < pbdHandler.materials.Count; i++)
            {
                MaterialJsonHandler.MaterialsJson TempMaterial = new MaterialJsonHandler.MaterialsJson();
                TempMaterial.MaterialName = mapHandler.Materials[i].Name;

                if (pbdHandler.materials[i].TextureID != -1)
                {
                    TempMaterial.TexturePath = TextureHandler.sshImages[pbdHandler.materials[i].TextureID].shortname + ".png";
                }
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

                TempMaterial.TextureFlipbook = new List<string>();
                if (pbdHandler.materials[i].TextureFlipbookID != -1)
                {
                    for (int a = 0; a < pbdHandler.textureFlipbooks[pbdHandler.materials[i].TextureFlipbookID].ImagePositions.Count; a++)
                    {
                        TempMaterial.TextureFlipbook.Add(TextureHandler.sshImages[pbdHandler.textureFlipbooks[pbdHandler.materials[i].TextureFlipbookID].ImagePositions[a]].shortname + ".png");
                    }
                }
                TempMaterial.UnknownInt20 = pbdHandler.materials[i].UnknownInt20;
                materialJson.Materials.Add(TempMaterial);
            }
            materialJson.CreateJson(ExportPath + "/Materials.json", InlineExporting);

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

                int FindHash = -1;
                for (int a = 0; a < pbdHandler.hashData.LightsHash.Count; a++)
                {
                    if (i == pbdHandler.hashData.LightsHash[a].ObjectUID)
                    {
                        FindHash = pbdHandler.hashData.LightsHash[a].Hash;
                        TempLight.Hash = FindHash;
                        break;
                    }
                }

                //if (FindHash != -1)
                //{
                //    for (int a = 0; a < adlHandler.HashSounds.Count; a++)
                //    {
                //        if (FindHash == adlHandler.HashSounds[a].Hash)
                //        {
                //            ADLTest[a] = true;

                //            TempLight.IncludeSound = true;
                //            var NewSound = new LightJsonHandler.SoundData();

                //            NewSound.CollisonSound = adlHandler.HashSounds[a].Sound.CollisonSound;
                //            NewSound.ExternalSounds = new List<LightJsonHandler.ExternalSound>();

                //            for (int b = 0; b < adlHandler.HashSounds[a].Sound.ExternalSounds.Count; b++)
                //            {
                //                var NewExternalSound = new LightJsonHandler.ExternalSound();
                //                NewExternalSound.U0 = adlHandler.HashSounds[a].Sound.ExternalSounds[b].U0;
                //                NewExternalSound.SoundIndex = adlHandler.HashSounds[a].Sound.ExternalSounds[b].SoundIndex;
                //                NewExternalSound.U2 = adlHandler.HashSounds[a].Sound.ExternalSounds[b].U2;
                //                NewExternalSound.U3 = adlHandler.HashSounds[a].Sound.ExternalSounds[b].U3;
                //                NewExternalSound.U4 = adlHandler.HashSounds[a].Sound.ExternalSounds[b].U4;
                //                NewExternalSound.U5 = adlHandler.HashSounds[a].Sound.ExternalSounds[b].U5;
                //                NewExternalSound.U6 = adlHandler.HashSounds[a].Sound.ExternalSounds[b].U6;
                //                NewSound.ExternalSounds.Add(NewExternalSound);
                //            }
                //            TempLight.Sounds = NewSound;
                //            break;
                //        }
                //    }
                //}

                lightJsonHandler.Lights.Add(TempLight);

            }
            lightJsonHandler.CreateJson(ExportPath + "/Lights.json", InlineExporting);


            //Create Spline Json
            splineJsonHandler = new SplineJsonHandler();
            for (int i = 0; i < pbdHandler.splines.Count; i++)
            {
                SplineJsonHandler.SplineJson TempSpline = new SplineJsonHandler.SplineJson();
                TempSpline.SplineName = mapHandler.Splines[i].Name;

                TempSpline.U0 = ssfHandler.Splines[i].U1;
                TempSpline.U1 = ssfHandler.Splines[i].U2;
                TempSpline.SplineStyle = ssfHandler.Splines[i].SplineStyle;

                TempSpline.Segments = new List<SplineJsonHandler.SegmentJson>();

                for (int a = pbdHandler.splines[i].SplineSegmentPosition; a < pbdHandler.splines[i].SplineSegmentPosition + pbdHandler.splines[i].SplineSegmentCount; a++)
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

                    segmentJson.U0 = pbdHandler.splinesSegments[a].U0;
                    segmentJson.U1 = pbdHandler.splinesSegments[a].U1;
                    segmentJson.U2 = pbdHandler.splinesSegments[a].U2;
                    segmentJson.U3 = pbdHandler.splinesSegments[a].U3;

                    TempSpline.Segments.Add(segmentJson);
                }

                splineJsonHandler.Splines.Add(TempSpline);
            }
            splineJsonHandler.CreateJson(ExportPath + "/Splines.json", InlineExporting);

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

                prefabJsonHandler.Prefabs.Add(TempModel);
            }
            prefabJsonHandler.CreateJson(ExportPath + "/Prefabs.json", InlineExporting);

            //Create Particle Model Json
            particleModelJsonHandler = new ParticleModelJsonHandler();
            for (int i = 0; i < pbdHandler.particleModels.Count; i++)
            {
                ParticleModelJsonHandler.ParticleModelJson TempParticleModel = new ParticleModelJsonHandler.ParticleModelJson();
                TempParticleModel.ParticleModelName = mapHandler.particelModels[i].Name;
                TempParticleModel.ParticleObjectHeaders = new List<ParticleModelJsonHandler.ParticleObjectHeader>();

                for (int a = 0; a < pbdHandler.particleModels[i].ParticleObjectHeaders.Count; a++)
                {
                    var NewObjectHeader = new ParticleModelJsonHandler.ParticleObjectHeader();
                    NewObjectHeader.ParticleObject = new ParticleModelJsonHandler.ParticleObject();

                    NewObjectHeader.ParticleObject.LowestXYZ = JsonUtil.Vector3ToArray(pbdHandler.particleModels[i].ParticleObjectHeaders[a].ParticleObject.LowestXYZ);
                    NewObjectHeader.ParticleObject.HighestXYZ = JsonUtil.Vector3ToArray(pbdHandler.particleModels[i].ParticleObjectHeaders[a].ParticleObject.HighestXYZ);
                    NewObjectHeader.ParticleObject.U1 = pbdHandler.particleModels[i].ParticleObjectHeaders[a].ParticleObject.U1;

                    NewObjectHeader.ParticleObject.AnimationFrames = new List<ParticleModelJsonHandler.AnimationFrames>();

                    for (int b = 0; b < pbdHandler.particleModels[i].ParticleObjectHeaders[a].ParticleObject.animationFrames.Count; b++)
                    {
                        var NewAnimationFrame = new ParticleModelJsonHandler.AnimationFrames();

                        NewAnimationFrame.Position = JsonUtil.Vector3ToArray(pbdHandler.particleModels[i].ParticleObjectHeaders[a].ParticleObject.animationFrames[b].Position);
                        NewAnimationFrame.Rotation = JsonUtil.Vector3ToArray(pbdHandler.particleModels[i].ParticleObjectHeaders[a].ParticleObject.animationFrames[b].Rotation);
                        NewAnimationFrame.Unknown = pbdHandler.particleModels[i].ParticleObjectHeaders[a].ParticleObject.animationFrames[b].Unknown;

                        NewObjectHeader.ParticleObject.AnimationFrames.Add(NewAnimationFrame);
                    }

                    TempParticleModel.ParticleObjectHeaders.Add(NewObjectHeader);
                }

                particleModelJsonHandler.ParticleModels.Add(TempParticleModel);
            }
            particleModelJsonHandler.CreateJson(ExportPath + "/ParticleModelHeaders.json", InlineExporting);

            //Create Camera Json
            cameraJSONHandler = new CameraJSONHandler();
            for (int i = 0; i < pbdHandler.Cameras.Count; i++)
            {
                var TempCamera = pbdHandler.Cameras[i];
                var NewCamera = new CameraJSONHandler.CameraInstance();

                NewCamera.CameraName = mapHandler.Cameras[i].Name;

                NewCamera.Translation = JsonUtil.Vector3ToArray(TempCamera.Translation);
                NewCamera.Rotation = JsonUtil.Vector3ToArray(TempCamera.Rotation);
                NewCamera.Type = TempCamera.Type;
                NewCamera.FocalLength = TempCamera.FocalLength;
                NewCamera.AspectRatio = TempCamera.AspectRatio;
                NewCamera.Aperture = JsonUtil.Vector2ToArray(TempCamera.Aperture);
                NewCamera.ClipPlane = JsonUtil.Vector2ToArray(TempCamera.ClipPlane);
                NewCamera.IntrestPoint = JsonUtil.Vector3ToArray(TempCamera.IntrestPoint);
                NewCamera.UpVector = JsonUtil.Vector3ToArray(TempCamera.UpVector);
                NewCamera.AnimTime = TempCamera.AnimTime;
                NewCamera.InitialPosition = JsonUtil.Vector3ToArray(TempCamera.AnimationInitial.InitialPosition);
                NewCamera.InitalRotation = JsonUtil.Vector3ToArray(TempCamera.AnimationInitial.InitalRotation);
                NewCamera.U0 = TempCamera.AnimationInitial.U0;

                NewCamera.AnimationHeaders = new List<CameraJSONHandler.CameraAnimationHeader>();

                for (int a = 0; a < TempCamera.AnimationInitial.AnimationHeaders.Count; a++)
                {
                    var NewAnimationHeader  = new CameraJSONHandler.CameraAnimationHeader();
                    NewAnimationHeader.Action = TempCamera.AnimationInitial.AnimationHeaders[a].Action;
                    NewAnimationHeader.AnimationDatas = new List<CameraJSONHandler.CameraAnimationData>();

                    for (int b = 0; b < TempCamera.AnimationInitial.AnimationHeaders[a].AnimationDatas.Count; b++)
                    {
                        var NewAnimationData = new CameraJSONHandler.CameraAnimationData();
                        NewAnimationData.Translation = JsonUtil.Vector3ToArray(TempCamera.AnimationInitial.AnimationHeaders[a].AnimationDatas[b].Translation);
                        NewAnimationData.Rotation = JsonUtil.Vector3ToArray(TempCamera.AnimationInitial.AnimationHeaders[a].AnimationDatas[b].Rotation);

                        NewAnimationHeader.AnimationDatas.Add(NewAnimationData);
                    }

                    NewCamera.AnimationHeaders.Add(NewAnimationHeader);
                }

                int FindHash = -1;
                for (int a = 0; a < pbdHandler.hashData.CameraHash.Count; a++)
                {
                    if (i == pbdHandler.hashData.CameraHash[a].ObjectUID)
                    {
                        FindHash = pbdHandler.hashData.CameraHash[a].Hash;
                        NewCamera.Hash = FindHash;
                        break;
                    }
                }

                //if (FindHash != -1)
                //{
                //    for (int a = 0; a < adlHandler.HashSounds.Count; a++)
                //    {
                //        if (FindHash == adlHandler.HashSounds[a].Hash)
                //        {
                //            ADLTest[a] = true;

                //            NewCamera.IncludeSound = true;
                //            var NewSound = new CameraJSONHandler.SoundData();

                //            NewSound.CollisonSound = adlHandler.HashSounds[a].Sound.CollisonSound;
                //            NewSound.ExternalSounds = new List<CameraJSONHandler.ExternalSound>();

                //            for (int b = 0; b < adlHandler.HashSounds[a].Sound.ExternalSounds.Count; b++)
                //            {
                //                var NewExternalSound = new CameraJSONHandler.ExternalSound();
                //                NewExternalSound.U0 = adlHandler.HashSounds[a].Sound.ExternalSounds[b].U0;
                //                NewExternalSound.SoundIndex = adlHandler.HashSounds[a].Sound.ExternalSounds[b].SoundIndex;
                //                NewExternalSound.U2 = adlHandler.HashSounds[a].Sound.ExternalSounds[b].U2;
                //                NewExternalSound.U3 = adlHandler.HashSounds[a].Sound.ExternalSounds[b].U3;
                //                NewExternalSound.U4 = adlHandler.HashSounds[a].Sound.ExternalSounds[b].U4;
                //                NewExternalSound.U5 = adlHandler.HashSounds[a].Sound.ExternalSounds[b].U5;
                //                NewExternalSound.U6 = adlHandler.HashSounds[a].Sound.ExternalSounds[b].U6;
                //                NewSound.ExternalSounds.Add(NewExternalSound);
                //            }
                //            NewCamera.Sounds = NewSound;
                //            break;
                //        }
                //    }
                //}

                cameraJSONHandler.Cameras.Add(NewCamera);
            }
            cameraJSONHandler.CreateJson(ExportPath + "/Cameras.json", InlineExporting);

            //if (ADLTest.Contains(false))
            //{
            //    MessageBox.Show("Report To Archy/Glitcher. Error 26");
            //}


            #endregion

            #region AIP & SOP
            if (File.Exists(LoadPath + ".aip"))
            {
                AIPSOPHandler aip = new AIPSOPHandler();
                aip.LoadAIPSOP(LoadPath + ".aip");

                AIPSOPJsonHandler aipJson = new AIPSOPJsonHandler();
                aipJson.StartPosList = aip.AIPath.StartPosList;

                aipJson.AIPaths = new List<AIPSOPJsonHandler.PathA>();
                for (int i = 0; i < aip.AIPath.PathAs.Count; i++)
                {
                    var NewAIPath = new AIPSOPJsonHandler.PathA();
                    NewAIPath.Type = aip.AIPath.PathAs[i].Type;
                    NewAIPath.U1 = aip.AIPath.PathAs[i].U1;
                    NewAIPath.U2 = aip.AIPath.PathAs[i].U2;
                    NewAIPath.U3 = aip.AIPath.PathAs[i].U3;
                    NewAIPath.U4 = aip.AIPath.PathAs[i].U4;
                    NewAIPath.U5 = aip.AIPath.PathAs[i].U5;
                    NewAIPath.U6 = aip.AIPath.PathAs[i].U6;

                    NewAIPath.PathPoints = new float[aip.AIPath.PathAs[i].VectorPoints.Count, 3];

                    NewAIPath.PathPos = JsonUtil.Vector3ToArray(aip.AIPath.PathAs[i].PathPos);

                    for (int a = 0; a < aip.AIPath.PathAs[i].VectorPoints.Count; a++)
                    {
                        NewAIPath.PathPoints[a, 0] = aip.AIPath.PathAs[i].VectorPoints[a].X * aip.AIPath.PathAs[i].VectorPoints[a].W;
                        NewAIPath.PathPoints[a, 1] = aip.AIPath.PathAs[i].VectorPoints[a].Y * aip.AIPath.PathAs[i].VectorPoints[a].W;
                        NewAIPath.PathPoints[a, 2] = aip.AIPath.PathAs[i].VectorPoints[a].Z * aip.AIPath.PathAs[i].VectorPoints[a].W;
                        if (a != 0)
                        {
                            NewAIPath.PathPoints[a, 0] += NewAIPath.PathPoints[a-1, 0];
                            NewAIPath.PathPoints[a, 1] += NewAIPath.PathPoints[a-1, 1];
                            NewAIPath.PathPoints[a, 2] += NewAIPath.PathPoints[a-1, 2];
                        }
                    }

                    NewAIPath.UnknownStructs = new List<AIPSOPJsonHandler.UnknownStruct>();
                    for (int a = 0; a < aip.AIPath.PathAs[i].UnknownStructs.Count; a++)
                    {
                        var NewStruct = new AIPSOPJsonHandler.UnknownStruct();
                        NewStruct.U0 = aip.AIPath.PathAs[i].UnknownStructs[a].U0;
                        NewStruct.U1 = aip.AIPath.PathAs[i].UnknownStructs[a].U1;
                        NewStruct.U2 = aip.AIPath.PathAs[i].UnknownStructs[a].U2;
                        NewStruct.U3 = aip.AIPath.PathAs[i].UnknownStructs[a].U3;
                        NewAIPath.UnknownStructs.Add(NewStruct);
                    }
                    aipJson.AIPaths.Add(NewAIPath);
                }

                aipJson.RaceLines = new List<AIPSOPJsonHandler.PathB>();
                for (int i = 0; i < aip.RaceLine.PathBs.Count; i++)
                {
                    var NewAIPath = new AIPSOPJsonHandler.PathB();
                    NewAIPath.Type = aip.RaceLine.PathBs[i].Type;
                    NewAIPath.U0 = aip.RaceLine.PathBs[i].U0;
                    NewAIPath.U1 = aip.RaceLine.PathBs[i].U1;
                    NewAIPath.U2 = aip.RaceLine.PathBs[i].U2;

                    NewAIPath.PathPoints = new float[aip.RaceLine.PathBs[i].VectorPoints.Count, 3];


                    NewAIPath.PathPos = JsonUtil.Vector3ToArray(aip.RaceLine.PathBs[i].PathPos);

                    for (int a = 0; a < aip.RaceLine.PathBs[i].VectorPoints.Count; a++)
                    {
                        NewAIPath.PathPoints[a, 0] = aip.RaceLine.PathBs[i].VectorPoints[a].X * aip.RaceLine.PathBs[i].VectorPoints[a].W;
                        NewAIPath.PathPoints[a, 1] = aip.RaceLine.PathBs[i].VectorPoints[a].Y * aip.RaceLine.PathBs[i].VectorPoints[a].W;
                        NewAIPath.PathPoints[a, 2] = aip.RaceLine.PathBs[i].VectorPoints[a].Z * aip.RaceLine.PathBs[i].VectorPoints[a].W;
                        if (a != 0)
                        {
                            NewAIPath.PathPoints[a, 0] += NewAIPath.PathPoints[a - 1, 0];
                            NewAIPath.PathPoints[a, 1] += NewAIPath.PathPoints[a - 1, 1];
                            NewAIPath.PathPoints[a, 2] += NewAIPath.PathPoints[a - 1, 2];
                        }
                    }

                    NewAIPath.UnknownStructs = new List<AIPSOPJsonHandler.UnknownStruct>();
                    for (int a = 0; a < aip.RaceLine.PathBs[i].UnknownStructs.Count; a++)
                    {
                        var NewStruct = new AIPSOPJsonHandler.UnknownStruct();
                        NewStruct.U0 = aip.RaceLine.PathBs[i].UnknownStructs[a].U0;
                        NewStruct.U1 = aip.RaceLine.PathBs[i].UnknownStructs[a].U1;
                        NewStruct.U2 = aip.RaceLine.PathBs[i].UnknownStructs[a].U2;
                        NewStruct.U3 = aip.RaceLine.PathBs[i].UnknownStructs[a].U3;
                        NewAIPath.UnknownStructs.Add(NewStruct);
                    }
                    aipJson.RaceLines.Add(NewAIPath);
                }

                aipJson.CreateJson(ExportPath + "/AIP.json", InlineExporting);

                AIPSOPHandler sop = new AIPSOPHandler();
                sop.LoadAIPSOP(LoadPath + ".sop");

                AIPSOPJsonHandler sopJson = new AIPSOPJsonHandler();
                sopJson.StartPosList = sop.AIPath.StartPosList;

                sopJson.AIPaths = new List<AIPSOPJsonHandler.PathA>();
                for (int i = 0; i < sop.AIPath.PathAs.Count; i++)
                {
                    var NewAIPath = new AIPSOPJsonHandler.PathA();
                    NewAIPath.Type = sop.AIPath.PathAs[i].Type;
                    NewAIPath.U1 = sop.AIPath.PathAs[i].U1;
                    NewAIPath.U2 = sop.AIPath.PathAs[i].U2;
                    NewAIPath.U3 = sop.AIPath.PathAs[i].U3;
                    NewAIPath.U4 = sop.AIPath.PathAs[i].U4;
                    NewAIPath.U5 = sop.AIPath.PathAs[i].U5;
                    NewAIPath.U6 = sop.AIPath.PathAs[i].U6;

                    NewAIPath.PathPoints = new float[sop.AIPath.PathAs[i].VectorPoints.Count, 3];

                    NewAIPath.PathPos = JsonUtil.Vector3ToArray(sop.AIPath.PathAs[i].PathPos);

                    for (int a = 0; a < sop.AIPath.PathAs[i].VectorPoints.Count; a++)
                    {
                        NewAIPath.PathPoints[a, 0] = sop.AIPath.PathAs[i].VectorPoints[a].X * sop.AIPath.PathAs[i].VectorPoints[a].W;
                        NewAIPath.PathPoints[a, 1] = sop.AIPath.PathAs[i].VectorPoints[a].Y * sop.AIPath.PathAs[i].VectorPoints[a].W;
                        NewAIPath.PathPoints[a, 2] = sop.AIPath.PathAs[i].VectorPoints[a].Z * sop.AIPath.PathAs[i].VectorPoints[a].W;
                        if (a != 0)
                        {
                            NewAIPath.PathPoints[a, 0] += NewAIPath.PathPoints[a - 1, 0];
                            NewAIPath.PathPoints[a, 1] += NewAIPath.PathPoints[a - 1, 1];
                            NewAIPath.PathPoints[a, 2] += NewAIPath.PathPoints[a - 1, 2];
                        }
                    }

                    NewAIPath.UnknownStructs = new List<AIPSOPJsonHandler.UnknownStruct>();
                    for (int a = 0; a < sop.AIPath.PathAs[i].UnknownStructs.Count; a++)
                    {
                        var NewStruct = new AIPSOPJsonHandler.UnknownStruct();
                        NewStruct.U0 = sop.AIPath.PathAs[i].UnknownStructs[a].U0;
                        NewStruct.U1 = sop.AIPath.PathAs[i].UnknownStructs[a].U1;
                        NewStruct.U2 = sop.AIPath.PathAs[i].UnknownStructs[a].U2;
                        NewStruct.U3 = sop.AIPath.PathAs[i].UnknownStructs[a].U3;
                        NewAIPath.UnknownStructs.Add(NewStruct);
                    }
                    sopJson.AIPaths.Add(NewAIPath);
                }

                sopJson.RaceLines = new List<AIPSOPJsonHandler.PathB>();
                for (int i = 0; i < sop.RaceLine.PathBs.Count; i++)
                {
                    var NewAIPath = new AIPSOPJsonHandler.PathB();
                    NewAIPath.Type = sop.RaceLine.PathBs[i].Type;
                    NewAIPath.U0 = sop.RaceLine.PathBs[i].U0;
                    NewAIPath.U1 = sop.RaceLine.PathBs[i].U1;
                    NewAIPath.U2 = sop.RaceLine.PathBs[i].U2;

                    NewAIPath.PathPoints = new float[sop.RaceLine.PathBs[i].VectorPoints.Count, 3];
                    NewAIPath.PathPos = JsonUtil.Vector3ToArray(sop.RaceLine.PathBs[i].PathPos);
                    for (int a = 0; a < sop.RaceLine.PathBs[i].VectorPoints.Count; a++)
                    {
                        NewAIPath.PathPoints[a, 0] = sop.RaceLine.PathBs[i].VectorPoints[a].X * sop.RaceLine.PathBs[i].VectorPoints[a].W;
                        NewAIPath.PathPoints[a, 1] = sop.RaceLine.PathBs[i].VectorPoints[a].Y * sop.RaceLine.PathBs[i].VectorPoints[a].W;
                        NewAIPath.PathPoints[a, 2] = sop.RaceLine.PathBs[i].VectorPoints[a].Z * sop.RaceLine.PathBs[i].VectorPoints[a].W;
                        if (a != 0)
                        {
                            NewAIPath.PathPoints[a, 0] += NewAIPath.PathPoints[a - 1, 0];
                            NewAIPath.PathPoints[a, 1] += NewAIPath.PathPoints[a - 1, 1];
                            NewAIPath.PathPoints[a, 2] += NewAIPath.PathPoints[a - 1, 2];
                        }
                    }

                    NewAIPath.UnknownStructs = new List<AIPSOPJsonHandler.UnknownStruct>();
                    for (int a = 0; a < sop.RaceLine.PathBs[i].UnknownStructs.Count; a++)
                    {
                        var NewStruct = new AIPSOPJsonHandler.UnknownStruct();
                        NewStruct.U0 = sop.RaceLine.PathBs[i].UnknownStructs[a].U0;
                        NewStruct.U1 = sop.RaceLine.PathBs[i].UnknownStructs[a].U1;
                        NewStruct.U2 = sop.RaceLine.PathBs[i].UnknownStructs[a].U2;
                        NewStruct.U3 = sop.RaceLine.PathBs[i].UnknownStructs[a].U3;
                        NewAIPath.UnknownStructs.Add(NewStruct);
                    }
                    sopJson.RaceLines.Add(NewAIPath);
                }

                sopJson.CreateJson(ExportPath + "/SOP.json", InlineExporting);
            }
            #endregion

            #region SSF JSON Data
            ssfJsonHandler = new SSFJsonHandler();
            ssfJsonHandler.EffectSlots = new List<SSFJsonHandler.EffectSlotJson>();

            for (int i = 0; i < ssfHandler.EffectSlots.Count; i++)
            {
                var TempEffectSlot = new SSFJsonHandler.EffectSlotJson();

                TempEffectSlot.EffectSlotName = "EffectSlot " + i;
                TempEffectSlot.PersistantEffectSlot = ssfHandler.EffectSlots[i].Slot1;
                TempEffectSlot.CollisionEffectSlot = ssfHandler.EffectSlots[i].Slot2;
                TempEffectSlot.Slot3 = ssfHandler.EffectSlots[i].Slot3;
                TempEffectSlot.Slot4 = ssfHandler.EffectSlots[i].Slot4;
                TempEffectSlot.EffectTriggerSlot = ssfHandler.EffectSlots[i].Slot5;
                TempEffectSlot.Slot6 = ssfHandler.EffectSlots[i].Slot6;
                TempEffectSlot.Slot7 = ssfHandler.EffectSlots[i].Slot7;
                ssfJsonHandler.EffectSlots.Add(TempEffectSlot);
            }

            ssfJsonHandler.EffectHeaders = new List<SSFJsonHandler.EffectHeaderStruct>();
            for (int i = 0; i < ssfHandler.EffectHeaders.Count; i++)
            {
                var NewEffectHeader = new SSFJsonHandler.EffectHeaderStruct();
                NewEffectHeader.EffectName = "Effect " + i.ToString();
                NewEffectHeader.Effects = new List<SSFJsonHandler.Effect>();

                for (int a = 0; a < ssfHandler.EffectHeaders[i].Effects.Count; a++)
                {
                    var TempEffect = ssfHandler.EffectHeaders[i].Effects[a];
                    NewEffectHeader.Effects.Add(SSFJsonHandler.EffectToJSON(TempEffect));
                }
                

                ssfJsonHandler.EffectHeaders.Add(NewEffectHeader);
            }

            ssfJsonHandler.Functions = new List<SSFJsonHandler.Function>();
            for (int i = 0; i < ssfHandler.Functions.Count; i++)
            {
                var NewFunction = new SSFJsonHandler.Function();
                NewFunction.FunctionName = ssfHandler.Functions[i].FunctionName;

                NewFunction.Effects = new List<SSFJsonHandler.Effect>();

                for (int a = 0; a < ssfHandler.Functions[i].Effects.Count; a++)
                {
                    var TempEffect = ssfHandler.Functions[i].Effects[a];
                    NewFunction.Effects.Add(SSFJsonHandler.EffectToJSON(TempEffect));
                }

                ssfJsonHandler.Functions.Add(NewFunction);
            }

            ssfJsonHandler.PhysicsHeaders = new List<SSFJsonHandler.PhysicsHeader>();
            for (int i = 0; i < ssfHandler.PhysicsHeaders.Count; i++)
            {
                var NewPhysicsHeader = new SSFJsonHandler.PhysicsHeader();
                NewPhysicsHeader.PhysicsName = "Physics Header " + i;
                NewPhysicsHeader.PhysicsDatas = new List<SSFJsonHandler.PhysicsData>();

                for (int a = 0; a < ssfHandler.PhysicsHeaders[i].PhysicsDatas.Count; a++)
                {
                    var NewPhysicsData = new SSFJsonHandler.PhysicsData();

                    NewPhysicsData.U2 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].U2;

                    NewPhysicsData.UFloat0 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat0;
                    NewPhysicsData.UFloat1 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat1;
                    NewPhysicsData.UFloat2 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat2;
                    NewPhysicsData.UFloat3 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat3;
                    NewPhysicsData.UFloat4 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat4;
                    NewPhysicsData.UFloat5 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat5;
                    NewPhysicsData.UFloat6 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat6;
                    NewPhysicsData.UFloat7 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat7;
                    NewPhysicsData.UFloat8 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat8;
                    NewPhysicsData.UFloat9 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat9;
                    NewPhysicsData.UFloat10 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat10;
                    NewPhysicsData.UFloat11 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat11;
                    NewPhysicsData.UFloat12 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat12;
                    NewPhysicsData.UFloat13 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat13;
                    NewPhysicsData.UFloat14 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat14;
                    NewPhysicsData.UFloat15 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat15;
                    NewPhysicsData.UFloat16 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat16;
                    NewPhysicsData.UFloat17 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat17;
                    NewPhysicsData.UFloat18 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat18;
                    NewPhysicsData.UFloat19 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat19;
                    NewPhysicsData.UFloat20 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat20;
                    NewPhysicsData.UFloat21 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat21;
                    NewPhysicsData.UFloat22 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat22;
                    NewPhysicsData.UFloat23 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat23;

                    NewPhysicsData.uPhysicsStruct0 = new List<SSFJsonHandler.UPhysicsStruct>();

                    for (int b = 0; b < ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].uPhysicsStruct0.Count; b++)
                    {
                        var NewPhysicsStruct = new SSFJsonHandler.UPhysicsStruct();
                        NewPhysicsStruct.U0 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].uPhysicsStruct0[b].U0;
                        NewPhysicsStruct.U1 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].uPhysicsStruct0[b].U1;
                        NewPhysicsStruct.U2 = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].uPhysicsStruct0[b].U2;
                        NewPhysicsData.uPhysicsStruct0.Add(NewPhysicsStruct);
                    }

                    NewPhysicsData.UByteData = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UByteData;
                    NewPhysicsData.UByteEndData = ssfHandler.PhysicsHeaders[i].PhysicsDatas[a].UByteEndData;
                    NewPhysicsHeader.PhysicsDatas.Add(NewPhysicsData);
                }



                ssfJsonHandler.PhysicsHeaders.Add(NewPhysicsHeader);
            }


            ssfJsonHandler.CreateJson(ExportPath + "/SSFLogic.json", InlineExporting);
            #endregion

            //Load and Export Textures
            OldSSHHandler LightmapHandler = new OldSSHHandler();
            Directory.CreateDirectory(ExportPath + "/Models");
            Directory.CreateDirectory(ExportPath + "/Textures");
            Directory.CreateDirectory(ExportPath + "/Skybox");
            Directory.CreateDirectory(ExportPath + "/Skybox/Textures");
            Directory.CreateDirectory(ExportPath + "/Skybox/Models");
            Directory.CreateDirectory(ExportPath + "/Lightmaps");
            Directory.CreateDirectory(ExportPath + "/Collision");
            ssfHandler.SaveModels(ExportPath + "/Collision/");
            pbdHandler.ExportModels(ExportPath + "/Models/");

            for (int i = 0; i < TextureHandler.sshImages.Count; i++)
            {
                TextureHandler.BrightenBitmap(i);
                TextureHandler.BMPOneExtract(ExportPath + "\\Textures\\" + TextureHandler.sshImages[i].shortname + ".png", i);
            }


            //NOTE CHANGE TO CHECK EACH PATH BEFORE RUNNING

            if (File.Exists(LoadPath + "_sky.ssh"))
            {
                OldSSHHandler SkyboxHandler = new OldSSHHandler();
                SkyboxHandler.LoadSSH(LoadPath + "_sky.ssh");

                //Load PBD Sky
                PBDHandler skypbdHandler = new PBDHandler();
                skypbdHandler.LoadPBD(LoadPath + "_sky.pbd");

                //Create Material Json
                SkyMaterialJson = new MaterialJsonHandler();
                for (int i = 0; i < skypbdHandler.materials.Count; i++)
                {
                    MaterialJsonHandler.MaterialsJson TempMaterial = new MaterialJsonHandler.MaterialsJson();

                    TempMaterial.TexturePath = SkyboxHandler.sshImages[skypbdHandler.materials[i].TextureID].shortname + ".png";
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
                    //TempMaterial.TextureFlipbookID = skypbdHandler.materials[i].TextureFlipbookID;
                    TempMaterial.UnknownInt20 = skypbdHandler.materials[i].UnknownInt20;
                    SkyMaterialJson.Materials.Add(TempMaterial);
                }
                SkyMaterialJson.CreateJson(ExportPath + "/Skybox/Materials.json", InlineExporting);

                //Create Model Json
                SkyPrefabJsonHandler = new PrefabJsonHandler();
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

                    SkyPrefabJsonHandler.Prefabs.Add(TempModel);
                }
                SkyPrefabJsonHandler.CreateJson(ExportPath + "/Skybox/Prefabs.json", InlineExporting);

                skypbdHandler.ExportModels(ExportPath + "/Skybox/Models/");

                for (int i = 0; i < SkyboxHandler.sshImages.Count; i++)
                {
                    SkyboxHandler.BrightenBitmap(i);
                    SkyboxHandler.BMPOneExtract(ExportPath + "\\Skybox\\Textures\\" + SkyboxHandler.sshImages[i].shortname + ".png", i);
                }
            }

            if (File.Exists(LoadPath + "_L.ssh"))
            {
                LightmapHandler.LoadSSH(LoadPath + "_L.ssh");

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
        }

        public void BuildTrickyLevelFiles(string LoadPath, string ExportPath)
        {
            List<string> ImageFiles = new List<string>();

            ExportPath = ExportPath.Substring(0, ExportPath.Length - 4);

            #region Rebuild PBD
            MapHandler mapHandler = new MapHandler();
            //mapHandler.Load(LoadPath + "/Original/level.map");

            //Load PBDHandler
            PBDHandler pbdHandler = new PBDHandler();
            //pbdHandler.LoadPBD(ExportPath + ".pbd");

            SSFHandler ssfHandler = new SSFHandler();

            ADLHandler adlHandler = new ADLHandler();
            ssfHandler.CollisonModelPointers = new List<SSFHandler.CollisonModelPointer>();
            ssfHandler.ObjectProperties = new List<SSFHandler.ObjectPropertiesStruct>();


            if (MAPGenerate || PBDGenerate || LTGGenerate || ADLGenerate || SSFGenerate)
            {
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

                    if (ImageFiles.Contains(ImportPatch.TexturePath))
                    {
                        patch.TextureAssigment = ImageFiles.IndexOf(ImportPatch.TexturePath);
                    }
                    else
                    {
                        ImageFiles.Add(ImportPatch.TexturePath);
                        patch.TextureAssigment = ImageFiles.Count - 1;
                    }

                    //if (lightingFixObjects.Count - 1 >= patch.TextureAssigment)
                    //{
                    //    lightingFixObjects[patch.TextureAssigment].Patch.Add(i);
                    //}
                    patch.LightmapID = ImportPatch.LightmapID;

                    pbdHandler.Patches.Add(patch);

                    LinkerItem linkerItem = new LinkerItem();
                    linkerItem.Name = ImportPatch.PatchName;
                    linkerItem.Ref = 1;
                    linkerItem.UID = i;
                    linkerItem.Hashvalue = MapHandler.GenerateHash(linkerItem.Name);
                    mapHandler.Patchs.Add(linkerItem);
                }

                ////Rebuild Splines
                splineJsonHandler = new SplineJsonHandler();
                splineJsonHandler = SplineJsonHandler.Load(LoadPath + "/Splines.json");
                pbdHandler.splines = new List<Spline>();
                pbdHandler.splinesSegments = new List<SplinesSegments>();
                mapHandler.Splines = new List<LinkerItem>();
                ssfHandler.Splines = new List<SSFHandler.Spline>();
                int SegmentPos = 0;
                for (int i = 0; i < splineJsonHandler.Splines.Count; i++)
                {
                    var TempSpline = splineJsonHandler.Splines[i];
                    Spline spline = new Spline();
                    spline.SplineSegmentPosition = SegmentPos;
                    spline.SplineSegmentCount = TempSpline.Segments.Count;
                    spline.Unknown1 = 0;
                    spline.Unknown2 = -1;

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

                        segments.U0 = TempSegment.U0;
                        segments.U1 = TempSegment.U1;
                        segments.U2 = TempSegment.U2;
                        segments.U3 = TempSegment.U3;

                        if (a == 0)
                        {
                            segments.PreviousSegment = -1;
                        }
                        else
                        {
                            segments.PreviousSegment = SegmentPos - 1;
                        }
                        if (a == TempSpline.Segments.Count - 1)
                        {
                            segments.NextSegment = -1;
                        }
                        else
                        {
                            segments.NextSegment = SegmentPos + 1;
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
                        LowestXYZSpline = MathUtil.Lowest(LowestXYZSpline, LowestXYZSegment);

                        segments.SegmentDisatnce = JsonUtil.GenerateDistance(bezierUtil.RawPoints[0], bezierUtil.RawPoints[1], bezierUtil.RawPoints[2], bezierUtil.RawPoints[3]);
                        segments.PreviousSegmentsDistance = PreviousSegmentDiffrence;
                        PreviousSegmentDiffrence += segments.SegmentDisatnce;
                        segments.Unknown32 = 4311823;
                        pbdHandler.splinesSegments.Add(segments);
                        SegmentPos++;
                    }

                    spline.LowestXYZ = LowestXYZSpline;
                    spline.HighestXYZ = HighestXYZSpline;

                    if(SSFGenerate)
                    {
                        var NewSpline = new SSFHandler.Spline();

                        NewSpline.U1 = TempSpline.U0;
                        NewSpline.U2 = TempSpline.U1;
                        NewSpline.SplineStyle = TempSpline.SplineStyle;

                        ssfHandler.Splines.Add(NewSpline);
                    }


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
                pbdHandler.hashData.InstanceHash = new List<HashDataUnknown>();
                ssfHandler.InstanceState = new List<int>();
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

                    var TempUnknownHash = new HashDataUnknown();
                    TempUnknownHash.Hash = Oldinstance.Hash;
                    TempUnknownHash.ObjectUID = i;
                    pbdHandler.hashData.InstanceHash.Add(TempUnknownHash);

                    if(Oldinstance.IncludeSound && Oldinstance.Sounds!=null && ADLGenerate)
                    {
                        var NewSound = new ADLHandler.HashSound();
                        NewSound.Hash = Oldinstance.Hash;
                        NewSound.Sound = new ADLHandler.SoundData();
                        NewSound.Sound.CollisonSound = Oldinstance.Sounds.Value.CollisonSound;
                        NewSound.Sound.ExternalSounds = new List<ADLHandler.ExternalSound>();

                        for (int a = 0; a < Oldinstance.Sounds.Value.ExternalSounds.Count; a++)
                        {
                            var NewExternalSound = new ADLHandler.ExternalSound();

                            NewExternalSound.U0= Oldinstance.Sounds.Value.ExternalSounds[a].U0;
                            NewExternalSound.SoundIndex = Oldinstance.Sounds.Value.ExternalSounds[a].SoundIndex;
                            NewExternalSound.U2 = Oldinstance.Sounds.Value.ExternalSounds[a].U2;
                            NewExternalSound.U3 = Oldinstance.Sounds.Value.ExternalSounds[a].U3;
                            NewExternalSound.U4 = Oldinstance.Sounds.Value.ExternalSounds[a].U4;
                            NewExternalSound.U5 = Oldinstance.Sounds.Value.ExternalSounds[a].U5;
                            NewExternalSound.U6 = Oldinstance.Sounds.Value.ExternalSounds[a].U6;

                            NewSound.Sound.ExternalSounds.Add(NewExternalSound);
                             
                        }

                        adlHandler.HashSounds.Add(NewSound);
                    }

                    if(SSFGenerate)
                    {
                        int CollsionIndex = -1;

                        if (Oldinstance.CollsionModelPaths != null)
                        {
                            if (Oldinstance.CollsionModelPaths.Length != 0)
                            {
                                //Make Collision and test if exists
                                var NewCollision = new SSFHandler.CollisonModelPointer();
                                NewCollision.Models = new List<SSFHandler.CollisonModel>();

                                for (int a = 0; a < Oldinstance.CollsionModelPaths.Length; a++)
                                {
                                    var NewCollisionModel = new SSFHandler.CollisonModel();

                                    NewCollisionModel.MeshPath = Oldinstance.CollsionModelPaths[a];

                                    NewCollision.Models.Add(NewCollisionModel);
                                }

                                bool Test = false;
                                for (int a = 0; a < ssfHandler.CollisonModelPointers.Count; a++)
                                {
                                    if (ssfHandler.CollisonModelPointers[a].Models.Count == NewCollision.Models.Count)
                                    {
                                        for (int b = 0; b < ssfHandler.CollisonModelPointers[a].Models.Count; b++)
                                        {
                                            if (ssfHandler.CollisonModelPointers[a].Models[b].MeshPath == NewCollision.Models[b].MeshPath)
                                            {
                                                Test = true;
                                            }
                                            else
                                            {
                                                Test = false;
                                                break;
                                            }
                                        }
                                    }
                                    if (Test)
                                    {
                                        CollsionIndex = a;
                                        break;
                                    }
                                }

                                if (!Test)
                                {
                                    ssfHandler.CollisonModelPointers.Add(NewCollision);
                                    CollsionIndex = ssfHandler.CollisonModelPointers.Count - 1;
                                }
                            }
                        }
                        //Make Object Properties and Test if Exists
                        var NewObjectProperties = new SSFHandler.ObjectPropertiesStruct();

                        NewObjectProperties.U0 = Oldinstance.U0;
                        NewObjectProperties.PlayerBounce = Oldinstance.PlayerBounce;
                        NewObjectProperties.U2 = Oldinstance.U2;
                        NewObjectProperties.U22 = Oldinstance.U22;
                        NewObjectProperties.U23 = Oldinstance.U23;
                        NewObjectProperties.U24 = Oldinstance.U24;
                        NewObjectProperties.U4 = Oldinstance.U4;
                        NewObjectProperties.PhysicsIndex = Oldinstance.PhysicsIndex;
                        NewObjectProperties.EffectSlotIndex = Oldinstance.EffectSlotIndex;
                        NewObjectProperties.U8 = Oldinstance.U8;
                        NewObjectProperties.CollsionMode = Oldinstance.CollsionMode;
                        NewObjectProperties.CollisonModelIndex = CollsionIndex;

                        bool Test1 = false;
                        for (int a = 0; a < ssfHandler.ObjectProperties.Count; a++)
                        {
                            var TempProperties = ssfHandler.ObjectProperties[a];

                            if (NewObjectProperties.U0 == TempProperties.U0 &&
                                NewObjectProperties.PlayerBounce == TempProperties.PlayerBounce &&
                                NewObjectProperties.U2 == TempProperties.U2 &&
                                NewObjectProperties.U22 == TempProperties.U22 &&
                                NewObjectProperties.U23 == TempProperties.U23 &&
                                NewObjectProperties.U24 == TempProperties.U24 &&
                                NewObjectProperties.U4 == TempProperties.U4 &&
                                NewObjectProperties.PhysicsIndex == TempProperties.PhysicsIndex &&
                                NewObjectProperties.EffectSlotIndex == TempProperties.EffectSlotIndex &&
                                NewObjectProperties.U8 == TempProperties.U8 &&
                                NewObjectProperties.CollsionMode == Oldinstance.CollsionMode &&
                                NewObjectProperties.CollisonModelIndex == TempProperties.CollisonModelIndex
                                )
                            {
                                Test1 = true;
                                ssfHandler.InstanceState.Add(a);
                                break;
                            }

                        }


                        if(!Test1)
                        {
                            ssfHandler.ObjectProperties.Add(NewObjectProperties);
                            ssfHandler.InstanceState.Add(ssfHandler.ObjectProperties.Count - 1);
                        }

                    }

                    LinkerItem linkerItem = new LinkerItem();
                    linkerItem.Ref = 1;
                    linkerItem.UID = i;
                    linkerItem.Name = Oldinstance.InstanceName;
                    linkerItem.Hashvalue = MapHandler.GenerateHash(linkerItem.Name);
                    mapHandler.InternalInstances.Add(linkerItem);
                }
                pbdHandler.hashData.InstanceHash.Sort((s1, s2) => s1.Hash.CompareTo(s2.Hash));

                if (ADLGenerate && adlHandler.HashSounds.Count !=0)
                {
                    adlHandler.HashSounds.Sort((s1, s2) => s1.Hash.CompareTo(s2.Hash));

                    adlHandler.Save(ExportPath + ".adl");
                }

                //Rebuild PraticleInstances
                particleInstanceJson = new ParticleInstanceJsonHandler();
                particleInstanceJson = ParticleInstanceJsonHandler.Load(LoadPath + "/ParticleInstances.json");
                mapHandler.particelModels = new List<LinkerItem>();
                pbdHandler.particleInstances = new List<ParticleInstance>();
                for (int i = 0; i < particleInstanceJson.Particles.Count; i++)
                {
                    ParticleInstance TempParticle = new ParticleInstance();

                    Matrix4x4 scale = Matrix4x4.CreateScale(JsonUtil.ArrayToVector3(particleInstanceJson.Particles[i].Scale));
                    Matrix4x4 Rotation = Matrix4x4.CreateFromQuaternion(JsonUtil.ArrayToQuaternion(particleInstanceJson.Particles[i].Rotation));
                    Matrix4x4 matrix4X4 = Matrix4x4.Multiply(scale, Rotation);
                    matrix4X4.Translation = JsonUtil.ArrayToVector3(particleInstanceJson.Particles[i].Location);

                    TempParticle.matrix4X4 = matrix4X4;

                    TempParticle.UnknownInt1 = particleInstanceJson.Particles[i].UnknownInt1;
                    TempParticle.LowestXYZ = JsonUtil.ArrayToVector3(particleInstanceJson.Particles[i].LowestXYZ);
                    TempParticle.HighestXYZ = JsonUtil.ArrayToVector3(particleInstanceJson.Particles[i].HighestXYZ);
                    TempParticle.UnknownInt8 = particleInstanceJson.Particles[i].UnknownInt8;
                    TempParticle.UnknownInt9 = particleInstanceJson.Particles[i].UnknownInt9;
                    TempParticle.UnknownInt10 = particleInstanceJson.Particles[i].UnknownInt10;
                    TempParticle.UnknownInt11 = particleInstanceJson.Particles[i].UnknownInt11;
                    TempParticle.UnknownInt12 = particleInstanceJson.Particles[i].UnknownInt12;
                    pbdHandler.particleInstances.Add(TempParticle);

                    LinkerItem linkerItem = new LinkerItem();
                    linkerItem.Ref = 1;
                    linkerItem.UID = i;
                    linkerItem.Name = particleInstanceJson.Particles[i].ParticleName;
                    linkerItem.Hashvalue = MapHandler.GenerateHash(linkerItem.Name);
                    mapHandler.ParticleInstances.Add(linkerItem);
                }

                //Rebuild Material Blocks
                prefabJsonHandler = new PrefabJsonHandler();
                prefabJsonHandler = PrefabJsonHandler.Load(LoadPath + "/Prefabs.json");
                pbdHandler.materialBlocks = new List<MaterialBlock>();
                for (int i = 0; i < prefabJsonHandler.Prefabs.Count; i++)
                {
                    var TempPrefab = prefabJsonHandler.Prefabs[i];
                    var NewMaterialBlock = new MaterialBlock();

                    NewMaterialBlock.ints = new List<int>();

                    for (int a = 0; a < prefabJsonHandler.Prefabs[i].PrefabObjects.Count; a++)
                    {
                        var TempObject = TempPrefab.PrefabObjects[a];
                        for (int b = 0; b < prefabJsonHandler.Prefabs[i].PrefabObjects[a].MeshData.Count; b++)
                        {
                            var TempMesh = TempObject.MeshData[b];
                            NewMaterialBlock.ints.Add(prefabJsonHandler.Prefabs[i].PrefabObjects[a].MeshData[b].MaterialID);

                            TempMesh.MaterialID = NewMaterialBlock.ints.Count - 1;

                            TempObject.MeshData[b] = TempMesh;
                        }
                        TempPrefab.PrefabObjects[a] = TempObject;
                    }
                    prefabJsonHandler.Prefabs[i] = TempPrefab;
                    pbdHandler.materialBlocks.Add(NewMaterialBlock);
                }

                //Rebuild Prefabs
                pbdHandler.PrefabData = new List<Prefabs>();
                mapHandler.Models = new List<LinkerItem>();
                for (int i = 0; i < prefabJsonHandler.Prefabs.Count; i++)
                {
                    var NewPrefab = new Prefabs();
                    var TempPrefab = prefabJsonHandler.Prefabs[i];
                    NewPrefab.MaterialBlockID = i;
                    NewPrefab.Unknown3 = prefabJsonHandler.Prefabs[i].Unknown3;
                    NewPrefab.AnimTime = prefabJsonHandler.Prefabs[i].AnimTime;

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

                        if (TempObject.IncludeAnimation && TempObject.Animation !=null)
                        {
                            NewObject.IncludeAnimation = true;
                            NewObject.objectAnimation = new ObjectAnimation();
                            NewObject.objectAnimation.U1 = TempObject.Animation.Value.U1;
                            NewObject.objectAnimation.U2 = TempObject.Animation.Value.U2;
                            NewObject.objectAnimation.U3 = TempObject.Animation.Value.U3;
                            NewObject.objectAnimation.U4 = TempObject.Animation.Value.U4;
                            NewObject.objectAnimation.U5 = TempObject.Animation.Value.U5;
                            NewObject.objectAnimation.U6 = TempObject.Animation.Value.U6;

                            NewObject.objectAnimation.AnimationAction = TempObject.Animation.Value.AnimationAction;
                            NewObject.objectAnimation.animationEntries = new List<AnimationEntry>();
                            if (TempObject.Animation.Value.AnimationEntries != null)
                            {
                                for (int b = 0; b < TempObject.Animation.Value.AnimationEntries.Count; b++)
                                {
                                    var TempAnimationEntry = TempObject.Animation.Value.AnimationEntries[b];
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

                    LinkerItem linkerItem = new LinkerItem();
                    linkerItem.Ref = 1;
                    linkerItem.UID = i;
                    linkerItem.Name = prefabJsonHandler.Prefabs[i].PrefabName;
                    linkerItem.Hashvalue = MapHandler.GenerateHash(linkerItem.Name);
                    mapHandler.Models.Add(linkerItem);

                }

                //Rebuild Materials
                materialJson = new MaterialJsonHandler();
                materialJson = MaterialJsonHandler.Load(LoadPath + "/Materials.json");
                pbdHandler.materials = new List<TrickyMaterial>();
                pbdHandler.textureFlipbooks = new List<TextureFlipbook>();
                mapHandler.Materials = new List<LinkerItem>();
                for (int i = 0; i < materialJson.Materials.Count; i++)
                {
                    var NewMaterial = new TrickyMaterial();

                    if (materialJson.Materials[i].TexturePath != "" && materialJson.Materials[i].TexturePath != null)
                    {
                        if (ImageFiles.Contains(materialJson.Materials[i].TexturePath))
                        {
                            NewMaterial.TextureID = ImageFiles.IndexOf(materialJson.Materials[i].TexturePath);
                        }
                        else
                        {
                            ImageFiles.Add(materialJson.Materials[i].TexturePath);
                            NewMaterial.TextureID = ImageFiles.Count - 1;
                        }
                    }
                    else
                    {
                        NewMaterial.TextureID = -1;
                    }

                    NewMaterial.UnknownInt2 = materialJson.Materials[i].UnknownInt2;
                    NewMaterial.UnknownInt3 = materialJson.Materials[i].UnknownInt3;
                    NewMaterial.UnknownFloat1 = materialJson.Materials[i].UnknownFloat1;
                    NewMaterial.UnknownFloat2 = materialJson.Materials[i].UnknownFloat2;
                    NewMaterial.UnknownFloat3 = materialJson.Materials[i].UnknownFloat3;
                    NewMaterial.UnknownFloat4 = materialJson.Materials[i].UnknownFloat4;
                    NewMaterial.UnknownInt8 = materialJson.Materials[i].UnknownInt8;
                    NewMaterial.UnknownFloat5 = materialJson.Materials[i].UnknownFloat5;

                    NewMaterial.UnknownFloat6 = materialJson.Materials[i].UnknownFloat6;
                    NewMaterial.UnknownFloat7 = materialJson.Materials[i].UnknownFloat7;
                    NewMaterial.UnknownFloat8 = materialJson.Materials[i].UnknownFloat8;

                    NewMaterial.UnknownInt13 = materialJson.Materials[i].UnknownInt13;
                    NewMaterial.UnknownInt14 = materialJson.Materials[i].UnknownInt14;
                    NewMaterial.UnknownInt15 = materialJson.Materials[i].UnknownInt15;
                    NewMaterial.UnknownInt16 = materialJson.Materials[i].UnknownInt16;
                    NewMaterial.UnknownInt17 = materialJson.Materials[i].UnknownInt17;
                    NewMaterial.UnknownInt18 = materialJson.Materials[i].UnknownInt18;

                    if (materialJson.Materials[i].TextureFlipbook.Count != 0)
                    {
                        TextureFlipbook textureFlipbook = new TextureFlipbook();
                        textureFlipbook.ImagePositions = new List<int>();

                        for (int a = 0; a < materialJson.Materials[i].TextureFlipbook.Count; a++)
                        {
                            if (ImageFiles.Contains(materialJson.Materials[i].TextureFlipbook[a]))
                            {
                                textureFlipbook.ImagePositions.Add(ImageFiles.IndexOf(materialJson.Materials[i].TextureFlipbook[a]));
                            }
                            else
                            {
                                ImageFiles.Add(materialJson.Materials[i].TextureFlipbook[a]);
                                textureFlipbook.ImagePositions.Add(ImageFiles.Count - 1);
                            }
                        }
                        pbdHandler.textureFlipbooks.Add(textureFlipbook);
                        NewMaterial.TextureFlipbookID = pbdHandler.textureFlipbooks.Count-1;
                    }
                    else
                    {
                        NewMaterial.TextureFlipbookID = -1;
                    }

                    NewMaterial.UnknownInt20 = materialJson.Materials[i].UnknownInt20;

                    pbdHandler.materials.Add(NewMaterial);

                    LinkerItem linkerItem = new LinkerItem();
                    linkerItem.Name = materialJson.Materials[i].MaterialName;
                    linkerItem.Ref = -1; //FIX Later
                    linkerItem.UID = i;
                    linkerItem.Hashvalue = MapHandler.GenerateHash(linkerItem.Name);
                    mapHandler.Materials.Add(linkerItem);
                }

                //Rebuild Lights
                lightJsonHandler = new LightJsonHandler();
                lightJsonHandler = LightJsonHandler.Load(LoadPath + "/Lights.json");
                pbdHandler.lights = new List<Light>();
                mapHandler.Lights = new List<LinkerItem>();
                pbdHandler.hashData.LightsHash = new List<HashDataUnknown>();
                for (int i = 0; i < lightJsonHandler.Lights.Count; i++)
                {
                    Light TempLight = new Light();
                    TempLight.Type = lightJsonHandler.Lights[i].Type;
                    TempLight.spriteRes = lightJsonHandler.Lights[i].SpriteRes;
                    TempLight.UnknownFloat1 = lightJsonHandler.Lights[i].UnknownFloat1;
                    TempLight.UnknownInt1 = lightJsonHandler.Lights[i].UnknownInt1;
                    TempLight.Colour = JsonUtil.ArrayToVector3(lightJsonHandler.Lights[i].Colour);
                    TempLight.Direction = JsonUtil.ArrayToVector3(lightJsonHandler.Lights[i].Direction);
                    TempLight.Postion = JsonUtil.ArrayToVector3(lightJsonHandler.Lights[i].Postion);
                    TempLight.LowestXYZ = JsonUtil.ArrayToVector3(lightJsonHandler.Lights[i].LowestXYZ);
                    TempLight.HighestXYZ = JsonUtil.ArrayToVector3(lightJsonHandler.Lights[i].HighestXYZ);
                    TempLight.UnknownFloat2 = lightJsonHandler.Lights[i].UnknownFloat2;
                    TempLight.UnknownInt2 = lightJsonHandler.Lights[i].UnknownInt2;
                    TempLight.UnknownFloat3 = lightJsonHandler.Lights[i].UnknownFloat3;
                    TempLight.UnknownInt3 = lightJsonHandler.Lights[i].UnknownInt3;

                    pbdHandler.lights.Add(TempLight);

                    var TempUnknownHash = new HashDataUnknown();
                    TempUnknownHash.Hash = lightJsonHandler.Lights[i].Hash;
                    TempUnknownHash.ObjectUID = i;
                    pbdHandler.hashData.LightsHash.Add(TempUnknownHash);

                    LinkerItem linkerItem = new LinkerItem();
                    linkerItem.Ref = 1;
                    linkerItem.UID = i;
                    linkerItem.Name = lightJsonHandler.Lights[i].LightName;
                    linkerItem.Hashvalue = MapHandler.GenerateHash(linkerItem.Name);
                    mapHandler.Lights.Add(linkerItem);

                }
                pbdHandler.hashData.LightsHash.Sort((s1, s2) => s1.Hash.CompareTo(s2.Hash));

                //Rebuild Particle Model
                particleModelJsonHandler = new ParticleModelJsonHandler();
                particleModelJsonHandler = ParticleModelJsonHandler.Load(LoadPath + "/ParticleModelHeaders.json");
                pbdHandler.particleModels = new List<ParticlePrefab>();
                mapHandler.particelModels = new List<LinkerItem>();
                for (int i = 0; i < particleModelJsonHandler.ParticleModels.Count; i++)
                {
                    var ParticleModel = new ParticlePrefab();

                    ParticleModel.ParticleObjectHeaders = new List<ParticleObjectHeader>();

                    for (int a = 0; a < particleModelJsonHandler.ParticleModels[i].ParticleObjectHeaders.Count; a++)
                    {
                        var NewParticleHeader = new ParticleObjectHeader();

                        NewParticleHeader.U1 = -1;
                        NewParticleHeader.U4 = -1;

                        NewParticleHeader.ParticleObject = new ParticleObject();
                        NewParticleHeader.ParticleObject.LowestXYZ = JsonUtil.ArrayToVector3(particleModelJsonHandler.ParticleModels[i].ParticleObjectHeaders[a].ParticleObject.LowestXYZ);
                        NewParticleHeader.ParticleObject.HighestXYZ = JsonUtil.ArrayToVector3(particleModelJsonHandler.ParticleModels[i].ParticleObjectHeaders[a].ParticleObject.HighestXYZ);
                        NewParticleHeader.ParticleObject.U1 = particleModelJsonHandler.ParticleModels[i].ParticleObjectHeaders[a].ParticleObject.U1;
                        NewParticleHeader.ParticleObject.animationFrames = new List<AnimationFrames>();

                        for (int b = 0; b < particleModelJsonHandler.ParticleModels[i].ParticleObjectHeaders[a].ParticleObject.AnimationFrames.Count; b++)
                        {
                            var NewAnimation = new AnimationFrames();
                            NewAnimation.Position = JsonUtil.ArrayToVector3(particleModelJsonHandler.ParticleModels[i].ParticleObjectHeaders[a].ParticleObject.AnimationFrames[b].Position);
                            NewAnimation.Rotation = JsonUtil.ArrayToVector3(particleModelJsonHandler.ParticleModels[i].ParticleObjectHeaders[a].ParticleObject.AnimationFrames[b].Rotation);
                            NewAnimation.Unknown = particleModelJsonHandler.ParticleModels[i].ParticleObjectHeaders[a].ParticleObject.AnimationFrames[b].Unknown;
                            NewParticleHeader.ParticleObject.animationFrames.Add(NewAnimation);
                        }

                        ParticleModel.ParticleObjectHeaders.Add(NewParticleHeader);
                    }


                    pbdHandler.particleModels.Add(ParticleModel);

                    LinkerItem linkerItem = new LinkerItem();
                    linkerItem.Ref = 1;
                    linkerItem.UID = i;
                    linkerItem.Name = particleModelJsonHandler.ParticleModels[i].ParticleModelName;
                    linkerItem.Hashvalue = MapHandler.GenerateHash(linkerItem.Name);
                    mapHandler.particelModels.Add(linkerItem);
                }

                //Rebuild Camera
                cameraJSONHandler = new CameraJSONHandler();
                cameraJSONHandler = CameraJSONHandler.Load(LoadPath + "/Cameras.json");
                pbdHandler.Cameras = new List<CameraInstance>();
                pbdHandler.hashData.CameraHash = new List<HashDataUnknown>();
                mapHandler.Cameras = new List<LinkerItem>();
                for (int i = 0; i < cameraJSONHandler.Cameras.Count; i++)
                {
                    var TempCamera = cameraJSONHandler.Cameras[i];
                    var NewCameraInstance = new CameraInstance();

                    NewCameraInstance.Translation = JsonUtil.ArrayToVector3(TempCamera.Translation);
                    NewCameraInstance.Rotation = JsonUtil.ArrayToVector3(TempCamera.Rotation);
                    NewCameraInstance.Type = TempCamera.Type;
                    NewCameraInstance.FocalLength = TempCamera.FocalLength;
                    NewCameraInstance.AspectRatio = TempCamera.AspectRatio;
                    NewCameraInstance.Aperture = JsonUtil.ArrayToVector2(TempCamera.Aperture);
                    NewCameraInstance.ClipPlane = JsonUtil.ArrayToVector2(TempCamera.ClipPlane);
                    NewCameraInstance.IntrestPoint = JsonUtil.ArrayToVector3(TempCamera.IntrestPoint);
                    NewCameraInstance.UpVector = JsonUtil.ArrayToVector3(TempCamera.UpVector);
                    NewCameraInstance.AnimTime = TempCamera.AnimTime;

                    NewCameraInstance.AnimationInitial = new CameraAnimationInitial();
                    NewCameraInstance.AnimationInitial.InitialPosition = JsonUtil.ArrayToVector3(TempCamera.InitialPosition);
                    NewCameraInstance.AnimationInitial.InitalRotation = JsonUtil.ArrayToVector3(TempCamera.InitalRotation);
                    NewCameraInstance.AnimationInitial.U0 = TempCamera.U0;

                    NewCameraInstance.AnimationInitial.AnimationHeaders = new List<CameraAnimationHeader>();

                    for (int a = 0; a < TempCamera.AnimationHeaders.Count; a++)
                    {
                        var NewAnimHeader = new CameraAnimationHeader();
                        NewAnimHeader.Action = TempCamera.AnimationHeaders[a].Action;
                        NewAnimHeader.AnimationDatas = new List<CameraAnimationData>();

                        for (int b = 0; b < TempCamera.AnimationHeaders[a].AnimationDatas.Count; b++)
                        {
                            var TempAnimationData = TempCamera.AnimationHeaders[a].AnimationDatas[b];
                            var NewAnimationData = new CameraAnimationData();

                            NewAnimationData.Translation = JsonUtil.ArrayToVector3(TempAnimationData.Translation);
                            NewAnimationData.Rotation = JsonUtil.ArrayToVector3(TempAnimationData.Rotation);

                            NewAnimHeader.AnimationDatas.Add(NewAnimationData);
                        }
                        NewCameraInstance.AnimationInitial.AnimationHeaders.Add(NewAnimHeader);
                    }

                    pbdHandler.Cameras.Add(NewCameraInstance);

                    LinkerItem linkerItem = new LinkerItem();
                    linkerItem.Ref = 1;
                    linkerItem.UID = i;
                    linkerItem.Name = cameraJSONHandler.Cameras[i].CameraName;
                    linkerItem.Hashvalue = MapHandler.GenerateHash(linkerItem.Name);

                    var TempUnknownHash = new HashDataUnknown();
                    TempUnknownHash.Hash = TempCamera.Hash;
                    TempUnknownHash.ObjectUID = i;
                    pbdHandler.hashData.CameraHash.Add(TempUnknownHash);


                    mapHandler.Cameras.Add(linkerItem);
                }
                pbdHandler.hashData.CameraHash.Sort((s1, s2) => s1.Hash.CompareTo(s2.Hash));
            }
            #endregion

            if (MAPGenerate)
            {
                mapHandler.Save(ExportPath + ".map");
            }

            if (PBDGenerate || LTGGenerate)
            {
                pbdHandler.ImportMeshes(LoadPath + "\\Models");
                pbdHandler.RegenerateLowestAndHighest();
            }

            if (PBDGenerate)
            {
                pbdHandler.SaveNew(ExportPath + ".pbd");
            }
            if (LTGGenerate)
            {
                LTGHandler ltgHandler = new LTGHandler();
                ltgHandler.RegenerateLTG(pbdHandler);
                ltgHandler.SaveLTGFile(ExportPath + ".ltg");
            }

            if (AIPGenerate)
            {
                //Edit AIP
                //AIPSOPJsonHandler aipJsonHandler = AIPSOPJsonHandler.Load(LoadPath + "/AIP.json");
                AIPSOPHandler aipHandler = new AIPSOPHandler();
                AIPSOPJsonHandler aip = new AIPSOPJsonHandler();
                aip = AIPSOPJsonHandler.Load(LoadPath + "/AIP.json");

                aipHandler.AIPath = new AIPSOPHandler.TypeA();
                aipHandler.AIPath.StartPosList = aip.StartPosList;
                aipHandler.AIPath.PathAs = new List<AIPSOPHandler.PathA>();
                for (int i = 0; i < aip.AIPaths.Count; i++)
                {
                    var NewAIPATH = new AIPSOPHandler.PathA();

                    NewAIPATH.Type = aip.AIPaths[i].Type;
                    NewAIPATH.U1 = aip.AIPaths[i].U1;
                    NewAIPATH.U2 = aip.AIPaths[i].U2;
                    NewAIPATH.U3 = aip.AIPaths[i].U3;
                    NewAIPATH.U4 = aip.AIPaths[i].U4;
                    NewAIPATH.U5 = aip.AIPaths[i].U5;
                    NewAIPATH.U6 = aip.AIPaths[i].U6;

                    NewAIPATH.PathPos = JsonUtil.ArrayToVector3(aip.AIPaths[i].PathPos);

                    List<Vector3> Points = new List<Vector3>();
                    Points.Add(new Vector3(0, 0, 0));
                    for (int a = 0; a < aip.AIPaths[i].PathPoints.GetLength(0); a++)
                    {
                        Points.Add(new Vector3(aip.AIPaths[i].PathPoints[a, 0], aip.AIPaths[i].PathPoints[a, 1], aip.AIPaths[i].PathPoints[a, 2]));
                    }

                    NewAIPATH.BBoxMax = NewAIPATH.PathPos;
                    NewAIPATH.BBoxMin = NewAIPATH.PathPos;
                    for (int a = 0; a < Points.Count; a++)
                    {
                        NewAIPATH.BBoxMax = MathUtil.Highest(NewAIPATH.BBoxMax, Points[a] + NewAIPATH.PathPos);
                        NewAIPATH.BBoxMin = MathUtil.Lowest(NewAIPATH.BBoxMin, Points[a] + NewAIPATH.PathPos);
                    }

                    NewAIPATH.VectorPoints = AIPSOPHandler.GenerateNewVectors(Points);

                    NewAIPATH.UnknownStructs = new List<AIPSOPHandler.UnknownStruct>();
                    for (int a = 0; a < aip.AIPaths[i].UnknownStructs.Count; a++)
                    {
                        var NewStruct = new AIPSOPHandler.UnknownStruct();
                        NewStruct.U0 = aip.AIPaths[i].UnknownStructs[a].U0;
                        NewStruct.U1 = aip.AIPaths[i].UnknownStructs[a].U1;
                        NewStruct.U2 = aip.AIPaths[i].UnknownStructs[a].U2;
                        NewStruct.U3 = aip.AIPaths[i].UnknownStructs[a].U3;
                        NewAIPATH.UnknownStructs.Add(NewStruct);
                    }

                    aipHandler.AIPath.PathAs.Add(NewAIPATH);
                }

                aipHandler.RaceLine = new AIPSOPHandler.TypeB();
                aipHandler.RaceLine.PathBs = new List<AIPSOPHandler.PathB>();
                aipHandler.RaceLine.U0 = 1;
                for (int i = 0; i < aip.RaceLines.Count; i++)
                {
                    var NewAIPATH = new AIPSOPHandler.PathB();

                    NewAIPATH.Type = aip.RaceLines[i].Type;
                    NewAIPATH.U0 = aip.RaceLines[i].U0;
                    NewAIPATH.U1 = aip.RaceLines[i].U1;
                    NewAIPATH.U2 = aip.RaceLines[i].U2;

                    NewAIPATH.PathPos = JsonUtil.ArrayToVector3(aip.RaceLines[i].PathPos);

                    List<Vector3> Points = new List<Vector3>();
                    Points.Add(new Vector3(0, 0, 0));
                    for (int a = 0; a < aip.RaceLines[i].PathPoints.GetLength(0); a++)
                    {
                        Points.Add(new Vector3(aip.RaceLines[i].PathPoints[a, 0], aip.RaceLines[i].PathPoints[a, 1], aip.RaceLines[i].PathPoints[a, 2]));
                    }

                    NewAIPATH.BBoxMax = NewAIPATH.PathPos;
                    NewAIPATH.BBoxMin = NewAIPATH.PathPos;
                    for (int a = 0; a < Points.Count; a++)
                    {
                        NewAIPATH.BBoxMax = MathUtil.Highest(NewAIPATH.BBoxMax, Points[a] + NewAIPATH.PathPos);
                        NewAIPATH.BBoxMin = MathUtil.Lowest(NewAIPATH.BBoxMin, Points[a] + NewAIPATH.PathPos);
                    }

                    NewAIPATH.VectorPoints = AIPSOPHandler.GenerateNewVectors(Points);

                    NewAIPATH.UnknownStructs = new List<AIPSOPHandler.UnknownStruct>();
                    for (int a = 0; a < aip.RaceLines[i].UnknownStructs.Count; a++)
                    {
                        var NewStruct = new AIPSOPHandler.UnknownStruct();
                        NewStruct.U0 = aip.RaceLines[i].UnknownStructs[a].U0;
                        NewStruct.U1 = aip.RaceLines[i].UnknownStructs[a].U1;
                        NewStruct.U2 = aip.RaceLines[i].UnknownStructs[a].U2;
                        NewStruct.U3 = aip.RaceLines[i].UnknownStructs[a].U3;
                        NewAIPATH.UnknownStructs.Add(NewStruct);
                    }

                    aipHandler.RaceLine.PathBs.Add(NewAIPATH);
                }

                aipHandler.SaveAIPSOP(ExportPath + ".aip");
            }

            if (SOPGenerate)
            {
                //Edit AIP
                //AIPSOPJsonHandler aipJsonHandler = AIPSOPJsonHandler.Load(LoadPath + "/AIP.json");
                AIPSOPHandler sopHandler = new AIPSOPHandler();
                AIPSOPJsonHandler sop = new AIPSOPJsonHandler();
                sop = AIPSOPJsonHandler.Load(LoadPath + "/SOP.json");

                sopHandler.AIPath = new AIPSOPHandler.TypeA();
                sopHandler.AIPath.StartPosList = sop.StartPosList;
                sopHandler.AIPath.PathAs = new List<AIPSOPHandler.PathA>();
                for (int i = 0; i < sop.AIPaths.Count; i++)
                {
                    var NewAIPATH = new AIPSOPHandler.PathA();

                    NewAIPATH.Type = sop.AIPaths[i].Type;
                    NewAIPATH.U1 = sop.AIPaths[i].U1;
                    NewAIPATH.U2 = sop.AIPaths[i].U2;
                    NewAIPATH.U3 = sop.AIPaths[i].U3;
                    NewAIPATH.U4 = sop.AIPaths[i].U4;
                    NewAIPATH.U5 = sop.AIPaths[i].U5;
                    NewAIPATH.U6 = sop.AIPaths[i].U6;

                    NewAIPATH.PathPos = JsonUtil.ArrayToVector3(sop.AIPaths[i].PathPos);

                    List<Vector3> Points = new List<Vector3>();
                    Points.Add(new Vector3(0, 0, 0));
                    for (int a = 0; a < sop.AIPaths[i].PathPoints.GetLength(0); a++)
                    {
                        Points.Add(new Vector3(sop.AIPaths[i].PathPoints[a, 0], sop.AIPaths[i].PathPoints[a, 1], sop.AIPaths[i].PathPoints[a, 2]));
                    }

                    NewAIPATH.BBoxMax = NewAIPATH.PathPos;
                    NewAIPATH.BBoxMin = NewAIPATH.PathPos;
                    for (int a = 0; a < Points.Count; a++)
                    {
                        NewAIPATH.BBoxMax = MathUtil.Highest(NewAIPATH.BBoxMax, Points[a] + NewAIPATH.PathPos);
                        NewAIPATH.BBoxMin = MathUtil.Lowest(NewAIPATH.BBoxMin, Points[a] + NewAIPATH.PathPos);
                    }

                    NewAIPATH.VectorPoints = AIPSOPHandler.GenerateNewVectors(Points);

                    NewAIPATH.UnknownStructs = new List<AIPSOPHandler.UnknownStruct>();
                    for (int a = 0; a < sop.AIPaths[i].UnknownStructs.Count; a++)
                    {
                        var NewStruct = new AIPSOPHandler.UnknownStruct();
                        NewStruct.U0 = sop.AIPaths[i].UnknownStructs[a].U0;
                        NewStruct.U1 = sop.AIPaths[i].UnknownStructs[a].U1;
                        NewStruct.U2 = sop.AIPaths[i].UnknownStructs[a].U2;
                        NewStruct.U3 = sop.AIPaths[i].UnknownStructs[a].U3;
                        NewAIPATH.UnknownStructs.Add(NewStruct);
                    }

                    sopHandler.AIPath.PathAs.Add(NewAIPATH);
                }

                sopHandler.RaceLine = new AIPSOPHandler.TypeB();
                sopHandler.RaceLine.PathBs = new List<AIPSOPHandler.PathB>();
                sopHandler.RaceLine.U0 = 1;
                for (int i = 0; i < sop.RaceLines.Count; i++)
                {
                    var NewAIPATH = new AIPSOPHandler.PathB();

                    NewAIPATH.Type = sop.RaceLines[i].Type;
                    NewAIPATH.U0 = sop.RaceLines[i].U0;
                    NewAIPATH.U1 = sop.RaceLines[i].U1;
                    NewAIPATH.U2 = sop.RaceLines[i].U2;

                    NewAIPATH.PathPos = JsonUtil.ArrayToVector3(sop.RaceLines[i].PathPos);

                    List<Vector3> Points = new List<Vector3>();
                    Points.Add(new Vector3(0, 0, 0));
                    for (int a = 0; a < sop.RaceLines[i].PathPoints.GetLength(0); a++)
                    {
                        Points.Add(new Vector3(sop.RaceLines[i].PathPoints[a, 0], sop.RaceLines[i].PathPoints[a, 1], sop.RaceLines[i].PathPoints[a, 2]));
                    }

                    NewAIPATH.BBoxMax = NewAIPATH.PathPos;
                    NewAIPATH.BBoxMin = NewAIPATH.PathPos;
                    for (int a = 0; a < Points.Count; a++)
                    {
                        NewAIPATH.BBoxMax = MathUtil.Highest(NewAIPATH.BBoxMax, Points[a] + NewAIPATH.PathPos);
                        NewAIPATH.BBoxMin = MathUtil.Lowest(NewAIPATH.BBoxMin, Points[a] + NewAIPATH.PathPos);
                    }

                    NewAIPATH.VectorPoints = AIPSOPHandler.GenerateNewVectors(Points);

                    NewAIPATH.UnknownStructs = new List<AIPSOPHandler.UnknownStruct>();
                    for (int a = 0; a < sop.RaceLines[i].UnknownStructs.Count; a++)
                    {
                        var NewStruct = new AIPSOPHandler.UnknownStruct();
                        NewStruct.U0 = sop.RaceLines[i].UnknownStructs[a].U0;
                        NewStruct.U1 = sop.RaceLines[i].UnknownStructs[a].U1;
                        NewStruct.U2 = sop.RaceLines[i].UnknownStructs[a].U2;
                        NewStruct.U3 = sop.RaceLines[i].UnknownStructs[a].U3;
                        NewAIPATH.UnknownStructs.Add(NewStruct);
                    }

                    sopHandler.RaceLine.PathBs.Add(NewAIPATH);
                }

                sopHandler.SaveAIPSOP(ExportPath + ".sop");
            }

            if (SSHGenerate)
            {
                //Build Textures
                OldSSHHandler TextureHandler = new OldSSHHandler();
                TextureHandler.format = "G278";

                for (int i = 0; i < ImageFiles.Count; i++)
                {
                    TextureHandler.AddImage();
                    TextureHandler.LoadSingle(LoadPath + "/Textures/" + ImageFiles[i], i);
                    TextureHandler.DarkenImage(i);
                    var temp = TextureHandler.sshImages[i];
                    temp.shortname = i.ToString().PadLeft(4, '0');
                    temp.AlphaFix = true;
                    TextureHandler.sshImages[i] = temp;
                }
                TextureHandler.SaveSSH(ExportPath + ".ssh", true);
            }

            if(SSFGenerate)
            {
                ssfJsonHandler = new SSFJsonHandler();
                ssfJsonHandler = SSFJsonHandler.Load(LoadPath + "/SSFLogic.json");
                ssfHandler.EffectSlots = new List<SSFHandler.EffectSlot>();
                //Effect Slot
                for (int i = 0; i < ssfJsonHandler.EffectSlots.Count; i++)
                {
                    var NewEffectSlot = new SSFHandler.EffectSlot();
                    NewEffectSlot.Slot1 = ssfJsonHandler.EffectSlots[i].PersistantEffectSlot;
                    NewEffectSlot.Slot2 = ssfJsonHandler.EffectSlots[i].CollisionEffectSlot;
                    NewEffectSlot.Slot3 = ssfJsonHandler.EffectSlots[i].Slot3;
                    NewEffectSlot.Slot4 = ssfJsonHandler.EffectSlots[i].Slot4;
                    NewEffectSlot.Slot5 = ssfJsonHandler.EffectSlots[i].EffectTriggerSlot;
                    NewEffectSlot.Slot6 = ssfJsonHandler.EffectSlots[i].Slot6;
                    NewEffectSlot.Slot7 = ssfJsonHandler.EffectSlots[i].Slot7;

                    ssfHandler.EffectSlots.Add(NewEffectSlot);
                }

                //Physics
                ssfHandler.PhysicsHeaders = new List<SSFHandler.PhysicsHeader>();
                for (int i = 0; i < ssfJsonHandler.PhysicsHeaders.Count; i++)
                {
                    var NewPhysicsHeader = new SSFHandler.PhysicsHeader();
                    NewPhysicsHeader.PhysicsDatas = new List<SSFHandler.PhysicsData>();

                    for (int a = 0; a < ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas.Count; a++)
                    {
                        var NewPhysicsData = new SSFHandler.PhysicsData();

                        NewPhysicsData.U2 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].U2;

                        NewPhysicsData.UFloat0 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat0;
                        NewPhysicsData.UFloat1 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat1;
                        NewPhysicsData.UFloat2 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat2;
                        NewPhysicsData.UFloat3 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat3;
                        NewPhysicsData.UFloat4 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat4;
                        NewPhysicsData.UFloat5 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat5;
                        NewPhysicsData.UFloat6 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat6;
                        NewPhysicsData.UFloat7 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat7;
                        NewPhysicsData.UFloat8 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat8;
                        NewPhysicsData.UFloat9 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat9;
                        NewPhysicsData.UFloat10 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat10;
                        NewPhysicsData.UFloat11 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat11;
                        NewPhysicsData.UFloat12 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat12;
                        NewPhysicsData.UFloat13 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat13;
                        NewPhysicsData.UFloat14 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat14;
                        NewPhysicsData.UFloat15 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat15;
                        NewPhysicsData.UFloat16 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat16;
                        NewPhysicsData.UFloat17 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat17;
                        NewPhysicsData.UFloat18 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat18;
                        NewPhysicsData.UFloat19 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat19;
                        NewPhysicsData.UFloat20 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat20;
                        NewPhysicsData.UFloat21 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat21;
                        NewPhysicsData.UFloat22 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat22;
                        NewPhysicsData.UFloat23 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UFloat23;

                        NewPhysicsData.uPhysicsStruct0 = new List<SSFHandler.UPhysicsStruct>();

                        for (int b = 0; b < ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].uPhysicsStruct0.Count; b++)
                        {
                            var NewPhysicsUStruct = new SSFHandler.UPhysicsStruct();

                            NewPhysicsUStruct.U0 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].uPhysicsStruct0[b].U0;
                            NewPhysicsUStruct.U1 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].uPhysicsStruct0[b].U1;
                            NewPhysicsUStruct.U2 = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].uPhysicsStruct0[b].U2;

                            NewPhysicsData.uPhysicsStruct0.Add(NewPhysicsUStruct);
                        }

                        NewPhysicsData.UByteData = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UByteData;
                        NewPhysicsData.UByteEndData = ssfJsonHandler.PhysicsHeaders[i].PhysicsDatas[a].UByteEndData;

                        NewPhysicsHeader.PhysicsDatas.Add(NewPhysicsData);
                    }

                    ssfHandler.PhysicsHeaders.Add(NewPhysicsHeader);
                }


                //Effects
                ssfHandler.EffectHeaders = new List<SSFHandler.EffectHeaderStruct>();
                for (int i = 0; i < ssfJsonHandler.EffectHeaders.Count; i++)
                {
                    var NewEffectHeader = new SSFHandler.EffectHeaderStruct();

                    NewEffectHeader.Effects = new List<SSFHandler.Effect>();

                    for (int a = 0; a < ssfJsonHandler.EffectHeaders[i].Effects.Count; a++)
                    {
                        NewEffectHeader.Effects.Add(SSFJsonHandler.JSONToEffect(ssfJsonHandler.EffectHeaders[i].Effects[a]));
                    }

                    ssfHandler.EffectHeaders.Add(NewEffectHeader);
                }


                //Function
                ssfHandler.Functions = new List<SSFHandler.Function>();
                for (int i = 0; i < ssfJsonHandler.Functions.Count; i++)
                {
                    var NewEffectHeader = new SSFHandler.Function();
                    NewEffectHeader.FunctionName = ssfJsonHandler.Functions[i].FunctionName;
                    NewEffectHeader.Effects = new List<SSFHandler.Effect>();

                    for (int a = 0; a < ssfJsonHandler.Functions[i].Effects.Count; a++)
                    {
                        NewEffectHeader.Effects.Add(SSFJsonHandler.JSONToEffect(ssfJsonHandler.Functions[i].Effects[a]));
                    }

                    ssfHandler.Functions.Add(NewEffectHeader);
                }

                //Load Model
                ssfHandler.LoadModels(LoadPath + "/Collision/");

                ssfHandler.Save(ExportPath + ".ssf");
            }


            #region Skybox Rebuild
            PBDHandler skyboxpbdHander = new PBDHandler();
            if (File.Exists(LoadPath + "/Skybox/Prefabs.json"))
            {
                //Rebuild Materials
                SkyMaterialJson = new MaterialJsonHandler();
                SkyMaterialJson = MaterialJsonHandler.Load(LoadPath + "/Skybox/Materials.json");
                skyboxpbdHander.materials = new List<TrickyMaterial>();

                List<string> SkyboxImageFiles = new List<string>();

                for (int i = 0; i < SkyMaterialJson.Materials.Count; i++)
                {
                    var NewMaterial = new TrickyMaterial();

                    if (SkyboxImageFiles.Contains(SkyMaterialJson.Materials[i].TexturePath))
                    {
                        NewMaterial.TextureID = SkyboxImageFiles.IndexOf(SkyMaterialJson.Materials[i].TexturePath);
                    }
                    else
                    {
                        SkyboxImageFiles.Add(SkyMaterialJson.Materials[i].TexturePath);
                        NewMaterial.TextureID = SkyboxImageFiles.Count - 1;
                    }

                    NewMaterial.UnknownInt2 = SkyMaterialJson.Materials[i].UnknownInt2;
                    NewMaterial.UnknownInt3 = SkyMaterialJson.Materials[i].UnknownInt3;
                    NewMaterial.UnknownFloat1 = SkyMaterialJson.Materials[i].UnknownFloat1;
                    NewMaterial.UnknownFloat2 = SkyMaterialJson.Materials[i].UnknownFloat2;
                    NewMaterial.UnknownFloat3 = SkyMaterialJson.Materials[i].UnknownFloat3;
                    NewMaterial.UnknownFloat4 = SkyMaterialJson.Materials[i].UnknownFloat4;
                    NewMaterial.UnknownInt8 = SkyMaterialJson.Materials[i].UnknownInt8;
                    NewMaterial.UnknownFloat5 = SkyMaterialJson.Materials[i].UnknownFloat5;

                    NewMaterial.UnknownFloat6 = SkyMaterialJson.Materials[i].UnknownFloat6;
                    NewMaterial.UnknownFloat7 = SkyMaterialJson.Materials[i].UnknownFloat7;
                    NewMaterial.UnknownFloat8 = SkyMaterialJson.Materials[i].UnknownFloat8;

                    NewMaterial.UnknownInt13 = SkyMaterialJson.Materials[i].UnknownInt13;
                    NewMaterial.UnknownInt14 = SkyMaterialJson.Materials[i].UnknownInt14;
                    NewMaterial.UnknownInt15 = SkyMaterialJson.Materials[i].UnknownInt15;
                    NewMaterial.UnknownInt16 = SkyMaterialJson.Materials[i].UnknownInt16;
                    NewMaterial.UnknownInt17 = SkyMaterialJson.Materials[i].UnknownInt17;
                    NewMaterial.UnknownInt18 = SkyMaterialJson.Materials[i].UnknownInt18;
                    //NewMaterial.TextureFlipbookID = SkyMaterialJson.Materials[i].TextureFlipbookID;
                    NewMaterial.UnknownInt20 = SkyMaterialJson.Materials[i].UnknownInt20;

                    skyboxpbdHander.materials.Add(NewMaterial);
                }

                //Rebuild Material Blocks
                SkyPrefabJsonHandler = new PrefabJsonHandler();
                SkyPrefabJsonHandler = PrefabJsonHandler.Load(LoadPath + "/Skybox/Prefabs.json");
                skyboxpbdHander.materialBlocks = new List<MaterialBlock>();
                for (int i = 0; i < SkyPrefabJsonHandler.Prefabs.Count; i++)
                {
                    var TempPrefab = SkyPrefabJsonHandler.Prefabs[i];
                    var NewMaterialBlock = new MaterialBlock();

                    NewMaterialBlock.ints = new List<int>();

                    for (int a = 0; a < SkyPrefabJsonHandler.Prefabs[i].PrefabObjects.Count; a++)
                    {
                        var TempObject = TempPrefab.PrefabObjects[a];
                        for (int b = 0; b < SkyPrefabJsonHandler.Prefabs[i].PrefabObjects[a].MeshData.Count; b++)
                        {
                            var TempMesh = TempObject.MeshData[b];
                            NewMaterialBlock.ints.Add(SkyPrefabJsonHandler.Prefabs[i].PrefabObjects[a].MeshData[b].MaterialID);

                            TempMesh.MaterialID = NewMaterialBlock.ints.Count - 1;

                            TempObject.MeshData[b] = TempMesh;
                        }
                        TempPrefab.PrefabObjects[a] = TempObject;
                    }
                    SkyPrefabJsonHandler.Prefabs[i] = TempPrefab;
                    skyboxpbdHander.materialBlocks.Add(NewMaterialBlock);
                }

                //Rebuild Prefabs
                skyboxpbdHander.PrefabData = new List<Prefabs>();
                for (int i = 0; i < SkyPrefabJsonHandler.Prefabs.Count; i++)
                {
                    var NewPrefab = new Prefabs();
                    var TempPrefab = SkyPrefabJsonHandler.Prefabs[i];
                    NewPrefab.MaterialBlockID = i;
                    NewPrefab.Unknown3 = SkyPrefabJsonHandler.Prefabs[i].Unknown3;
                    NewPrefab.AnimTime = SkyPrefabJsonHandler.Prefabs[i].AnimTime;

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

                        if (TempObject.IncludeAnimation && TempObject.Animation!=null)
                        {
                            NewObject.IncludeAnimation = true;
                            NewObject.objectAnimation = new ObjectAnimation();
                            NewObject.objectAnimation.U1 = TempObject.Animation.Value.U1;
                            NewObject.objectAnimation.U2 = TempObject.Animation.Value.U2;
                            NewObject.objectAnimation.U3 = TempObject.Animation.Value.U3;
                            NewObject.objectAnimation.U4 = TempObject.Animation.Value.U4;
                            NewObject.objectAnimation.U5 = TempObject.Animation.Value.U5;
                            NewObject.objectAnimation.U6 = TempObject.Animation.Value.U6;

                            NewObject.objectAnimation.AnimationAction = TempObject.Animation.Value.AnimationAction;
                            NewObject.objectAnimation.animationEntries = new List<AnimationEntry>();
                            if (TempObject.Animation.Value.AnimationEntries != null)
                            {
                                for (int b = 0; b < TempObject.Animation.Value.AnimationEntries.Count; b++)
                                {
                                    var TempAnimationEntry = TempObject.Animation.Value.AnimationEntries[b];
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
                    skyboxpbdHander.PrefabData.Add(NewPrefab);

                }

                if (SkyPBDGenerate)
                {
                    skyboxpbdHander.ImportMeshes(LoadPath + "/Skybox/Models");
                    skyboxpbdHander.SaveNew(ExportPath + "_sky.pbd");
                }

                if (SkySSHGenerate)
                {
                    //Build Skybox
                    OldSSHHandler SkyboxHandler = new OldSSHHandler();
                    SkyboxHandler.format = "G278";

                    for (int i = 0; i < SkyboxImageFiles.Count; i++)
                    {
                        SkyboxHandler.AddImage();
                        SkyboxHandler.LoadSingle(LoadPath + "/Skybox/Textures/" + SkyboxImageFiles[i], i);
                        SkyboxHandler.DarkenImage(i);
                        var temp = SkyboxHandler.sshImages[i];
                        temp.shortname = i.ToString().PadLeft(4, '0');
                        temp.AlphaFix = true;
                        SkyboxHandler.sshImages[i] = temp;
                    }

                    SkyboxHandler.SaveSSH(ExportPath + "_sky.ssh", true);
                }
            }
            #endregion

            if (LSSHGenerate)
            {
                OldSSHHandler LightmapHandler = new OldSSHHandler();
                //Build Lightmap
                if (!Unilightmap)
                {
                    LightmapHandler.format = "G278";

                    string[] LightmapFiles = Directory.GetFiles(LoadPath + "/Lightmaps", "*.png");
                    for (int i = 0; i < LightmapFiles.Length; i++)
                    {
                        LightmapHandler.AddImage(64, 5);
                        LightmapHandler.LoadSingle(LightmapFiles[i], i);
                        LightmapHandler.DarkenImage(i);
                        var temp = LightmapHandler.sshImages[i];
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
            }
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
            materialJson = MaterialJsonHandler.Load(LoadPath + "/Materials.json");

            //Create Lights Json
            lightJsonHandler = LightJsonHandler.Load(LoadPath + "/Lights.json");

            //Create Spline Json
            splineJsonHandler = SplineJsonHandler.Load(LoadPath + "/Splines.json");

            //Create Model Json
            prefabJsonHandler = PrefabJsonHandler.Load(LoadPath + "/Prefabs.json");

            //Create Particle Model Json
            particleModelJsonHandler = ParticleModelJsonHandler.Load(LoadPath + "/ParticleModelHeaders.json");

            SkyMaterialJson = MaterialJsonHandler.Load(LoadPath + "/Skybox/Materials.json");

            SkyPrefabJsonHandler = PrefabJsonHandler.Load(LoadPath + "/Skybox/Prefabs.json");


        }
    }
}
