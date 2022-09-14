using SSXMultiTool.FileHandlers;
using SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2;
using SSXMultiTool.JsonFiles.Tricky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            PatchesJson patchPoints = new PatchesJson();
            for (int i = 0; i < pbdHandler.Patches.Count; i++)
            {
                PatchesJson.PatchJson patch = new PatchesJson.PatchJson();
                patch.PatchName = mapHandler.Patchs[i].Name;

                patch.LightMapPoint = pbdHandler.Patches[i].LightMapPoint;

                patch.UVPoint1 = pbdHandler.Patches[i].UVPoint1;
                patch.UVPoint2 = pbdHandler.Patches[i].UVPoint2;
                patch.UVPoint3 = pbdHandler.Patches[i].UVPoint3;
                patch.UVPoint4 = pbdHandler.Patches[i].UVPoint4;

                patch.R4C4 = pbdHandler.Patches[i].R4C4;
                patch.R4C3 = pbdHandler.Patches[i].R4C3;
                patch.R4C2 = pbdHandler.Patches[i].R4C2;
                patch.R4C1 = pbdHandler.Patches[i].R4C1;
                patch.R3C4 = pbdHandler.Patches[i].R3C4;
                patch.R3C3 = pbdHandler.Patches[i].R3C3;
                patch.R3C2 = pbdHandler.Patches[i].R3C2;
                patch.R3C1 = pbdHandler.Patches[i].R3C1;
                patch.R2C3 = pbdHandler.Patches[i].R2C3;
                patch.R2C2 = pbdHandler.Patches[i].R2C2;
                patch.R2C1 = pbdHandler.Patches[i].R2C1;
                patch.R1C4 = pbdHandler.Patches[i].R1C4;
                patch.R1C3 = pbdHandler.Patches[i].R1C3;
                patch.R1C2 = pbdHandler.Patches[i].R1C2;
                patch.R1C1 = pbdHandler.Patches[i].R1C1;

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
            InstancesJson instancesJson = new InstancesJson();
            for (int i = 0; i < pbdHandler.Instances.Count; i++)
            {
                InstancesJson.InstanceJson instanceJson = new InstancesJson.InstanceJson();
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
            ParticleInstanceJson particleInstanceJson = new ParticleInstanceJson();
            for (int i = 0; i < pbdHandler.particleInstances.Count; i++)
            {
                ParticleInstanceJson.ParticleJson TempParticle = new ParticleInstanceJson.ParticleJson();
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
            MaterialJson materialJson = new MaterialJson();
            for (int i = 0; i < pbdHandler.materials.Count; i++)
            {
                MaterialJson.MaterialsJson TempMaterial = new MaterialJson.MaterialsJson();
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

            //Create Lights Json

            //Create Spline Json

            //Create Texture FLipbook Json

            //Create Model Json

            //Create Particle Model Json

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
