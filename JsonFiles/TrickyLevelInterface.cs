using SSXMultiTool.FileHandlers;
using SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2;
using SSXMultiTool.JsonFiles.Tricky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;

namespace SSXMultiTool
{
    internal class TrickyLevelInterface
    {

        public void ExtractTrickyLevelFiles(string LoadPath, string ExportPath)
        {
            //Load Map
            MapHandler mapHandler = new MapHandler();
            mapHandler.Load(LoadPath + ".map");

            //Load PBD
            PBDHandler pbdHandler = new PBDHandler();
            pbdHandler.LoadPBD(LoadPath + ".pbd");

            //Create Patches JSON
            PatchesJsonHandler patchPoints = new PatchesJsonHandler();
            for (int i = 0; i < pbdHandler.Patches.Count; i++)
            {
                PatchesJsonHandler.PatchJson patch = new PatchesJsonHandler.PatchJson();
                patch.PatchName = mapHandler.Patchs[i].Name;

                patch.LightMapPoint = pbdHandler.Patches[i].LightMapPoint;

                patch.UVPoint1 = pbdHandler.Patches[i].UVPoint1;
                patch.UVPoint2 = pbdHandler.Patches[i].UVPoint2;
                patch.UVPoint3 = pbdHandler.Patches[i].UVPoint3;
                patch.UVPoint4 = pbdHandler.Patches[i].UVPoint4;

                patch.R4C4 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].R4C4);
                patch.R4C3 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].R4C3);
                patch.R4C2 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].R4C2);
                patch.R4C1 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].R4C1);
                patch.R3C4 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].R3C4);
                patch.R3C3 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].R3C3);
                patch.R3C2 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].R3C2);
                patch.R3C1 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].R3C1);
                patch.R2C4 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].R2C4);
                patch.R2C3 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].R2C3);
                patch.R2C2 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].R2C2);
                patch.R2C1 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].R2C1);
                patch.R1C4 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].R1C4);
                patch.R1C3 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].R1C3);
                patch.R1C2 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].R1C2);
                patch.R1C1 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].R1C1);

                patch.PatchStyle = pbdHandler.Patches[i].PatchStyle;
                patch.Unknown2 = pbdHandler.Patches[i].Unknown2;
                patch.TextureAssigment = pbdHandler.Patches[i].TextureAssigment;
                patch.LightmapID = pbdHandler.Patches[i].LightmapID;
                patch.Unknown4 = pbdHandler.Patches[i].Unknown4;
                patch.Unknown5 = pbdHandler.Patches[i].Unknown5;
                patch.Unknown6 = pbdHandler.Patches[i].Unknown6;
                patchPoints.patches.Add(patch);
            }
            patchPoints.CreateJson(ExportPath + "/Patches.json");

            //Create Instance JSON
            InstanceJsonHandler instancesJson = new InstanceJsonHandler();
            for (int i = 0; i < pbdHandler.Instances.Count; i++)
            {
                InstanceJsonHandler.InstanceJson instanceJson = new InstanceJsonHandler.InstanceJson();
                instanceJson.InstanceName = mapHandler.InternalInstances[i].Name;
                instanceJson.MatrixCol1 = pbdHandler.Instances[i].MatrixCol1;
                instanceJson.MatrixCol2 = pbdHandler.Instances[i].MatrixCol2;
                instanceJson.MatrixCol3 = pbdHandler.Instances[i].MatrixCol3;
                instanceJson.InstancePosition = pbdHandler.Instances[i].InstancePosition;
                instanceJson.Unknown5 = pbdHandler.Instances[i].Unknown5;
                instanceJson.Unknown6 = pbdHandler.Instances[i].Unknown6;
                instanceJson.Unknown7 = pbdHandler.Instances[i].Unknown7;
                instanceJson.Unknown8 = pbdHandler.Instances[i].Unknown8;
                instanceJson.Unknown9 = pbdHandler.Instances[i].Unknown9;
                instanceJson.Unknown10 = pbdHandler.Instances[i].Unknown10;
                instanceJson.Unknown11 = pbdHandler.Instances[i].Unknown11;
                instanceJson.RGBA = pbdHandler.Instances[i].RGBA;
                instanceJson.ModelID = pbdHandler.Instances[i].ModelID;
                instanceJson.PrevInstance = pbdHandler.Instances[i].NextInstance;

                instanceJson.LowestXYZ = pbdHandler.Instances[i].LowestXYZ;
                instanceJson.HighestXYZ = pbdHandler.Instances[i].HighestXYZ;

                instanceJson.UnknownInt26 = pbdHandler.Instances[i].UnknownInt26;
                instanceJson.UnknownInt27 = pbdHandler.Instances[i].UnknownInt27;
                instanceJson.UnknownInt28 = pbdHandler.Instances[i].UnknownInt28;
                instanceJson.ModelID2 = pbdHandler.Instances[i].ModelID2;
                instanceJson.UnknownInt30 = pbdHandler.Instances[i].UnknownInt30;
                instanceJson.UnknownInt31 = pbdHandler.Instances[i].UnknownInt31;
                instanceJson.UnknownInt32 = pbdHandler.Instances[i].UnknownInt32;
                instancesJson.instances.Add(instanceJson);
            }
            instancesJson.CreateJson(ExportPath + "/Instances.json");

            //Create Particle Instances JSON
            ParticleInstanceJsonHandler particleInstanceJson = new ParticleInstanceJsonHandler();
            for (int i = 0; i < pbdHandler.particleInstances.Count; i++)
            {
                ParticleInstanceJsonHandler.ParticleJson TempParticle = new ParticleInstanceJsonHandler.ParticleJson();
                TempParticle.ParticleName = mapHandler.ParticleInstances[i].Name;
                TempParticle.MatrixCol1 = pbdHandler.particleInstances[i].MatrixCol1;
                TempParticle.MatrixCol2 = pbdHandler.particleInstances[i].MatrixCol2;
                TempParticle.MatrixCol3 = pbdHandler.particleInstances[i].MatrixCol3;
                TempParticle.ParticleInstancePosition = pbdHandler.particleInstances[i].ParticleInstancePosition;
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
            MaterialJsonHandler materialJson = new MaterialJsonHandler();
            for (int i = 0; i < pbdHandler.materials.Count; i++)
            {
                MaterialJsonHandler.MaterialsJson TempMaterial = new MaterialJsonHandler.MaterialsJson();
                TempMaterial.MaterialName = mapHandler.Materials[i].Name;

                TempMaterial.UnknownInt1 = pbdHandler.materials[i].UnknownInt1;
                TempMaterial.UnknownInt2 = pbdHandler.materials[i].UnknownInt2;
                TempMaterial.UnknownInt3 = pbdHandler.materials[i].UnknownInt3;
                TempMaterial.UnknownInt4 = pbdHandler.materials[i].UnknownInt4;
                TempMaterial.UnknownInt5 = pbdHandler.materials[i].UnknownInt5;
                TempMaterial.UnknownInt6 = pbdHandler.materials[i].UnknownInt6;
                TempMaterial.UnknownInt7 = pbdHandler.materials[i].UnknownInt7;
                TempMaterial.UnknownInt8 = pbdHandler.materials[i].UnknownInt8;
                TempMaterial.UnknownInt9 = pbdHandler.materials[i].UnknownInt9;
                TempMaterial.UnknownInt10 = pbdHandler.materials[i].UnknownInt10;
                TempMaterial.UnknownInt11 = pbdHandler.materials[i].UnknownInt11;
                TempMaterial.UnknownInt12 = pbdHandler.materials[i].UnknownInt12;
                TempMaterial.UnknownInt13 = pbdHandler.materials[i].UnknownInt13;
                TempMaterial.UnknownInt14 = pbdHandler.materials[i].UnknownInt14;
                TempMaterial.UnknownInt15 = pbdHandler.materials[i].UnknownInt15;
                TempMaterial.UnknownInt16 = pbdHandler.materials[i].UnknownInt16;
                TempMaterial.UnknownInt17 = pbdHandler.materials[i].UnknownInt17;
                TempMaterial.UnknownInt18 = pbdHandler.materials[i].UnknownInt18;
                materialJson.MaterialsJsons.Add(TempMaterial);
            }
            materialJson.CreateJson(ExportPath + "/Material.json");

            //Create Material Block Json
            MaterialBlockJsonHandler materialBlockJson = new MaterialBlockJsonHandler();
            for (int i = 0; i < pbdHandler.materialBlocks.Count; i++)
            {
                MaterialBlockJsonHandler.MaterialBlock TempBlock = new MaterialBlockJsonHandler.MaterialBlock();

                TempBlock.BlockCount = pbdHandler.materialBlocks[i].ints.Count;
                TempBlock.ints = pbdHandler.materialBlocks[i].ints;
                materialBlockJson.MaterialBlockJsons.Add(TempBlock);
            }
            materialBlockJson.CreateJson(ExportPath + "/MaterialBlocks.json");

            //Create Lights Json
            LightJsonHandler lightJsonHandler = new LightJsonHandler();
            for (int i = 0; i < pbdHandler.lights.Count; i++)
            {
                LightJsonHandler.LightJson TempLight = new LightJsonHandler.LightJson();
                TempLight.LightName = mapHandler.Lights[i].Name;

                TempLight.UnknownInt1 = pbdHandler.lights[i].UnknownInt1;
                TempLight.UnknownInt2 = pbdHandler.lights[i].UnknownInt2;
                TempLight.UnknownInt3 = pbdHandler.lights[i].UnknownInt3;
                TempLight.UnknownInt4 = pbdHandler.lights[i].UnknownInt4;
                TempLight.UnknownInt5 = pbdHandler.lights[i].UnknownInt5;
                TempLight.UnknownInt6 = pbdHandler.lights[i].UnknownInt6;
                TempLight.UnknownInt7 = pbdHandler.lights[i].UnknownInt7;
                TempLight.UnknownInt8 = pbdHandler.lights[i].UnknownInt8;
                TempLight.UnknownInt9 = pbdHandler.lights[i].UnknownInt9;
                TempLight.UnknownInt10 = pbdHandler.lights[i].UnknownInt10;
                TempLight.UnknownInt11 = pbdHandler.lights[i].UnknownInt11;
                TempLight.UnknownInt12 = pbdHandler.lights[i].UnknownInt12;
                TempLight.UnknownInt13 = pbdHandler.lights[i].UnknownInt13;
                TempLight.UnknownInt14 = pbdHandler.lights[i].UnknownInt14;
                TempLight.UnknownInt15 = pbdHandler.lights[i].UnknownInt15;
                TempLight.UnknownInt16 = pbdHandler.lights[i].UnknownInt16;
                TempLight.UnknownInt17 = pbdHandler.lights[i].UnknownInt17;
                TempLight.UnknownInt18 = pbdHandler.lights[i].UnknownInt18;
                TempLight.UnknownInt19 = pbdHandler.lights[i].UnknownInt19;
                TempLight.UnknownInt20 = pbdHandler.lights[i].UnknownInt20;
                TempLight.UnknownInt21 = pbdHandler.lights[i].UnknownInt21;
                TempLight.UnknownInt22 = pbdHandler.lights[i].UnknownInt22;
                TempLight.UnknownInt23 = pbdHandler.lights[i].UnknownInt23;

                lightJsonHandler.LightJsons.Add(TempLight);

            }
            lightJsonHandler.CreateJson(ExportPath + "/Lights.json");

            //Create Spline Json
            SplineJsonHandler splineJsonHandler = new SplineJsonHandler();
            for (int i = 0; i < pbdHandler.splines.Count; i++)
            {
                SplineJsonHandler.SplineJson TempSpline = new SplineJsonHandler.SplineJson();
                TempSpline.SplineName = mapHandler.Splines[i].Name;
                TempSpline.Unknown1 = pbdHandler.splines[i].Unknown1;
                TempSpline.Unknown2 = pbdHandler.splines[i].Unknown2;
                TempSpline.SegmentCount = pbdHandler.splines[i].SplineSegmentCount;
                TempSpline.Segments = new List<SplineJsonHandler.SegmentJson>();

                for (int a = pbdHandler.splines[i].SplineSegmentPosition; a < pbdHandler.splines[i].SplineSegmentPosition+TempSpline.SegmentCount; a++)
                {
                    SplineJsonHandler.SegmentJson segmentJson = new SplineJsonHandler.SegmentJson();
                    segmentJson.Point4 = JsonUtil.Vector4ToArray(pbdHandler.splinesSegments[a].Point4);
                    segmentJson.Point3 = JsonUtil.Vector4ToArray(pbdHandler.splinesSegments[a].Point3);
                    segmentJson.Point2 = JsonUtil.Vector4ToArray(pbdHandler.splinesSegments[a].Point2);
                    segmentJson.Point1 = JsonUtil.Vector4ToArray(pbdHandler.splinesSegments[a].ControlPoint);

                    segmentJson.Unknown = JsonUtil.Vector4ToArray(pbdHandler.splinesSegments[a].ScalingPoint);
                    segmentJson.Unknown32 = pbdHandler.splinesSegments[a].Unknown32;
                    TempSpline.Segments.Add(segmentJson);
                }
                splineJsonHandler.SplineJsons.Add(TempSpline);
            }
            splineJsonHandler.CreateJson(ExportPath + "/Splines.json");

            //Create Texture FLipbook Json
            TextureFlipbookJsonHandler textureFlipbookJsonHandler = new TextureFlipbookJsonHandler();
            for (int i = 0; i < pbdHandler.textureFlipbooks.Count; i++)
            {
                TextureFlipbookJsonHandler.FlipbookJson TempFlipbook = new TextureFlipbookJsonHandler.FlipbookJson();
                TempFlipbook.ImageCount = pbdHandler.textureFlipbooks[i].ImagePositions.Count;
                TempFlipbook.Images = pbdHandler.textureFlipbooks[i].ImagePositions;
                textureFlipbookJsonHandler.FlipbookJsons.Add(TempFlipbook);
            }
            textureFlipbookJsonHandler.CreateJson(ExportPath + "/TextureFlipbook.json");

            //Create Model Json
            ModelJsonHandler modelJsonHandler = new ModelJsonHandler();
            for (int i = 0; i < pbdHandler.modelHeaders.Count; i++)
            {
                ModelJsonHandler.ModelJson TempModel = new ModelJsonHandler.ModelJson();
                TempModel.ModelName = mapHandler.Models[i].Name;
                TempModel.TotalLength = pbdHandler.modelHeaders[i].TotalLength;
                TempModel.Unknown0 = pbdHandler.modelHeaders[i].Unknown0;
                TempModel.Unknown1 = pbdHandler.modelHeaders[i].Unknown1;
                TempModel.Unknown2 = pbdHandler.modelHeaders[i].Unknown2;
                TempModel.Unknown3 = pbdHandler.modelHeaders[i].Unknown3;
                TempModel.Unknown4 = pbdHandler.modelHeaders[i].Unknown4;
                TempModel.ScaleX = pbdHandler.modelHeaders[i].ScaleX;
                TempModel.ScaleZ = pbdHandler.modelHeaders[i].ScaleZ;
                TempModel.ScaleY = pbdHandler.modelHeaders[i].ScaleY;
                TempModel.Unknown8 = pbdHandler.modelHeaders[i].Unknown8;
                TempModel.Unknown9 = pbdHandler.modelHeaders[i].Unknown9;
                TempModel.TriStripCount = pbdHandler.modelHeaders[i].TriStripCount;
                TempModel.VertexCount = pbdHandler.modelHeaders[i].VertexCount;
                TempModel.Unknown12 = pbdHandler.modelHeaders[i].Unknown12;
                TempModel.Unknown13 = pbdHandler.modelHeaders[i].Unknown13;
                TempModel.Unknown14 = pbdHandler.modelHeaders[i].Unknown14;
                TempModel.Unknown15 = pbdHandler.modelHeaders[i].Unknown15;
                TempModel.Unknown16 = pbdHandler.modelHeaders[i].Unknown16;
                TempModel.Unknown17 = pbdHandler.modelHeaders[i].Unknown17;
                TempModel.Unknown18 = pbdHandler.modelHeaders[i].Unknown18;
                TempModel.UnknownLength = pbdHandler.modelHeaders[i].UnknownLength;

                TempModel.LowestXYZ = pbdHandler.modelHeaders[i].LowestXYZ;
                TempModel.HighestXYZ = pbdHandler.modelHeaders[i].HighestXYZ;

                TempModel.bytes = pbdHandler.modelHeaders[i].bytes;
                modelJsonHandler.ModelJsons.Add(TempModel);
            }
            modelJsonHandler.CreateJson(ExportPath + "/ModelHeaders.json");

            //Create Particle Model Json
            ParticleModelJsonHandler particleModelJsonHandler = new ParticleModelJsonHandler();
            for (int i = 0; i < pbdHandler.particleModels.Count; i++)
            {
                ParticleModelJsonHandler.ParticleModelJson TempParticleModel = new ParticleModelJsonHandler.ParticleModelJson();
                TempParticleModel.ParticleModelName = mapHandler.particelModels[i].Name;
                TempParticleModel.Unknown0 = pbdHandler.particleModels[i].Unknown0;
                TempParticleModel.Unknown1 = pbdHandler.particleModels[i].Unknown0;
                TempParticleModel.Unknown2 = pbdHandler.particleModels[i].Unknown0;
                TempParticleModel.Unknown3 = pbdHandler.particleModels[i].Unknown0;
                TempParticleModel.Unknown4 = pbdHandler.particleModels[i].Unknown0;
                TempParticleModel.Unknown5 = pbdHandler.particleModels[i].Unknown0;
                TempParticleModel.Unknown6 = pbdHandler.particleModels[i].Unknown0;
                TempParticleModel.Unknown7 = pbdHandler.particleModels[i].Unknown0;
                TempParticleModel.Unknown8 = pbdHandler.particleModels[i].Unknown0;
                TempParticleModel.Unknown9 = pbdHandler.particleModels[i].Unknown0;
                TempParticleModel.Unknown10 = pbdHandler.particleModels[i].Unknown0;
                TempParticleModel.UnknownLenght = pbdHandler.particleModels[i].UnknownLenght;
                TempParticleModel.bytes = pbdHandler.particleModels[i].bytes;
                particleModelJsonHandler.ParticleModelJsons.Add(TempParticleModel);
            }
            particleModelJsonHandler.CreateJson(ExportPath + "/ParticleModelHeaders.json");

            //Create Camera Json

            //Create Mesh data

            //Load and Export Textures
            SSHHandler TextureHandler = new SSHHandler();
            SSHHandler SkyboxHandler = new SSHHandler();
            SSHHandler LightmapHandler = new SSHHandler();
            Directory.CreateDirectory(ExportPath + "/Textures");
            Directory.CreateDirectory(ExportPath + "/Skybox");
            Directory.CreateDirectory(ExportPath + "/Lightmaps");

            TextureHandler.LoadSSH(LoadPath + ".ssh");
            for (int i = 0; i < TextureHandler.sshImages.Count; i++)
            {
                TextureHandler.BrightenBitmap(i);
                TextureHandler.BMPOneExtract(ExportPath + "\\Textures\\" + TextureHandler.sshImages[i].shortname + ".png", i);
            }

            SkyboxHandler.LoadSSH(LoadPath + "_sky.ssh");
            for (int i = 0; i < SkyboxHandler.sshImages.Count; i++)
            {
                SkyboxHandler.BrightenBitmap(i);
                SkyboxHandler.BMPOneExtract(ExportPath + "\\Skybox\\" + SkyboxHandler.sshImages[i].shortname + ".png", i);
            }

            LightmapHandler.LoadSSH(LoadPath + "_L.ssh");
            for (int i = 0; i < LightmapHandler.sshImages.Count; i++)
            {
                LightmapHandler.BrightenBitmap(i);
                LightmapHandler.BMPOneExtract(ExportPath + "\\Lightmaps\\" + LightmapHandler.sshImages[i].shortname + ".png", i);
            }
        }
    }
}
