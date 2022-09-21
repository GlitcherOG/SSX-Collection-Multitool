using SSXMultiTool.FileHandlers;
using SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2;
using SSXMultiTool.JsonFiles.Tricky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;
using System.Numerics;

namespace SSXMultiTool
{
    public class TrickyLevelInterface
    {
        public PatchesJsonHandler patchPoints = new PatchesJsonHandler();
        public InstanceJsonHandler instancesJson = new InstanceJsonHandler();
        public ParticleInstanceJsonHandler particleInstanceJson = new ParticleInstanceJsonHandler();
        public MaterialJsonHandler materialJson = new MaterialJsonHandler();
        public MaterialBlockJsonHandler materialBlockJson = new MaterialBlockJsonHandler();
        public LightJsonHandler lightJsonHandler = new LightJsonHandler();
        public SplineJsonHandler splineJsonHandler = new SplineJsonHandler();
        public TextureFlipbookJsonHandler textureFlipbookJsonHandler = new TextureFlipbookJsonHandler();
        public ModelJsonHandler modelJsonHandler = new ModelJsonHandler();
        public ParticleModelJsonHandler particleModelJsonHandler = new ParticleModelJsonHandler();

        public void ExtractTrickyLevelFiles(string LoadPath, string ExportPath)
        {
            File.Create(ExportPath + "/Config.ssx");

            //Load Map
            MapHandler mapHandler = new MapHandler();
            mapHandler.Load(LoadPath + ".map");

            //Load PBD
            PBDHandler pbdHandler = new PBDHandler();
            pbdHandler.LoadPBD(LoadPath + ".pbd");

            //Create Patches JSON
            patchPoints = new PatchesJsonHandler();
            for (int i = 0; i < pbdHandler.Patches.Count; i++)
            {
                PatchesJsonHandler.PatchJson patch = new PatchesJsonHandler.PatchJson();
                patch.PatchName = mapHandler.Patchs[i].Name;

                patch.LightMapPoint = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].LightMapPoint);

                patch.UVPoint1 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].UVPoint1);
                patch.UVPoint2 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].UVPoint2);
                patch.UVPoint3 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].UVPoint3);
                patch.UVPoint4 = JsonUtil.Vector4ToArray(pbdHandler.Patches[i].UVPoint4);

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

                patch.R1C1 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[0]);
                patch.R1C2 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[1]);
                patch.R1C3 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[2]);
                patch.R1C4 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[3]);
                patch.R2C1 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[4]);
                patch.R2C2 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[5]);
                patch.R2C3 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[6]);
                patch.R2C4 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[7]);
                patch.R3C1 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[8]);
                patch.R3C2 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[9]);
                patch.R3C3 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[10]);
                patch.R3C4 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[11]);
                patch.R4C1 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[12]);
                patch.R4C2 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[13]);
                patch.R4C3 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[14]);
                patch.R4C4 = JsonUtil.Vector3ToArray(bezierUtil.RawPoints[15]);

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
            instancesJson = new InstanceJsonHandler();
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
            particleInstanceJson = new ParticleInstanceJsonHandler();
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
            materialJson = new MaterialJsonHandler();
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
            materialBlockJson = new MaterialBlockJsonHandler();
            for (int i = 0; i < pbdHandler.materialBlocks.Count; i++)
            {
                MaterialBlockJsonHandler.MaterialBlock TempBlock = new MaterialBlockJsonHandler.MaterialBlock();

                TempBlock.BlockCount = pbdHandler.materialBlocks[i].ints.Count;
                TempBlock.ints = pbdHandler.materialBlocks[i].ints;
                materialBlockJson.MaterialBlockJsons.Add(TempBlock);
            }
            materialBlockJson.CreateJson(ExportPath + "/MaterialBlocks.json");

            //Create Lights Json
            lightJsonHandler = new LightJsonHandler();
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
            splineJsonHandler = new SplineJsonHandler();
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
                TempFlipbook.ImageCount = pbdHandler.textureFlipbooks[i].ImagePositions.Count;
                TempFlipbook.Images = pbdHandler.textureFlipbooks[i].ImagePositions;
                textureFlipbookJsonHandler.FlipbookJsons.Add(TempFlipbook);
            }
            textureFlipbookJsonHandler.CreateJson(ExportPath + "/TextureFlipbook.json");

            //Create Model Json
            modelJsonHandler = new ModelJsonHandler();
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
            particleModelJsonHandler = new ParticleModelJsonHandler();
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
            Directory.CreateDirectory(ExportPath + "/Models");
            pbdHandler.SaveModel(ExportPath + "/Models/Test.glb");

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
            if (File.Exists(LoadPath + "_sky.ssh"))
            {
                SkyboxHandler.LoadSSH(LoadPath + "_sky.ssh");
                for (int i = 0; i < SkyboxHandler.sshImages.Count; i++)
                {
                    SkyboxHandler.BrightenBitmap(i);
                    SkyboxHandler.BMPOneExtract(ExportPath + "\\Skybox\\" + SkyboxHandler.sshImages[i].shortname + ".png", i);
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
                LightmapHandler.LoadSSH(LoadPath.Substring(0,LoadPath.Length-1) + "_L.ssh");
                for (int i = 0; i < LightmapHandler.sshImages.Count; i++)
                {
                    LightmapHandler.BrightenBitmap(i);
                    LightmapHandler.BMPOneExtract(ExportPath + "\\Lightmaps\\" + LightmapHandler.sshImages[i].shortname + ".png", i);
                }
            }

            Directory.CreateDirectory(ExportPath + "/Original");
            File.Copy(LoadPath + ".adl", ExportPath + "/Original" + "/ald.ald");
            File.Copy(LoadPath + ".aip", ExportPath + "/Original" + "/aip.aip");
            File.Copy(LoadPath + ".ltg", ExportPath + "/Original" + "/ltg.ltg");
            File.Copy(LoadPath + ".sop", ExportPath + "/Original" + "/sop.sop");
            File.Copy(LoadPath + ".ssf", ExportPath + "/Original" + "/ssf.ssf");
            File.Copy(LoadPath + "_sky.pbd", ExportPath + "/Original" + "/sky.pbd");
            File.Copy(LoadPath + ".pbd", ExportPath + "/Original" + "/level.pbd");
            File.Copy(LoadPath + ".map", ExportPath + "/Original" + "/level.map");
        }

        public void BuildTrickyLevelFiles(string LoadPath, string ExportPath)
        {
            ExportPath = ExportPath.Substring(0, ExportPath.Length - 4);





            //Build Textures
            SSHHandler TextureHandler = new SSHHandler();
            TextureHandler.format = "G278";

            string[] ImageFiles = Directory.GetFiles(LoadPath + "/Textures", "*.png");
            for (int i = 0; i < ImageFiles.Length; i++)
            {
                TextureHandler.AddImage();
                TextureHandler.LoadSingle(ImageFiles[i], i);
                TextureHandler.DarkenImage(i);
                var temp = TextureHandler.sshImages[i];
                temp.shortname = i.ToString().PadLeft(4, '0');
                temp.AlphaFix = true;
                TextureHandler.sshImages[i] = temp;
            }

            TextureHandler.SaveSSH(ExportPath+".ssh", true);


            string[] SkyboxFiles = Directory.GetFiles(LoadPath + "/Skybox", "*.png");
            if (SkyboxFiles.Length != 0)
            {
                //Build Skybox
                SSHHandler SkyboxHandler = new SSHHandler();
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
        }

        public void LoadAndVerifyFiles(string LoadPath)
        {
            //Create Patches JSON
            patchPoints = PatchesJsonHandler.Load(LoadPath + "/Patches.json");

            //Create Instance JSON
            instancesJson=InstanceJsonHandler.Load(LoadPath + "/Instances.json");

            //Create Particle Instances JSON
            particleInstanceJson = ParticleInstanceJsonHandler.Load(LoadPath + "/ParticleInstances.json");

            //Create Material Json
            materialJson = MaterialJsonHandler.Load(LoadPath + "/Material.json");

            //Create Material Block Json
            materialBlockJson = MaterialBlockJsonHandler.Load(LoadPath + "/MaterialBlocks.json");

            //Create Lights Json
            lightJsonHandler = LightJsonHandler.Load(LoadPath + "/Lights.json");

            //Create Spline Json
            splineJsonHandler = SplineJsonHandler.Load(LoadPath + "/Splines.json");

            //Create Texture FLipbook Json
            textureFlipbookJsonHandler = TextureFlipbookJsonHandler.Load(LoadPath + "/TextureFlipbook.json");

            //Create Model Json
            modelJsonHandler = ModelJsonHandler.Load(LoadPath + "/ModelHeaders.json");

            //Create Particle Model Json
            particleModelJsonHandler = ParticleModelJsonHandler.Load(LoadPath + "/ParticleModelHeaders.json");
        }
    }
}
