using SSXMultiTool.FileHandlers.LevelFiles.OGPS2;
using SSXMultiTool.JsonFiles.SSXOG;
using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.FileHandlers.Textures;
using System.IO;

namespace SSXMultiTool.JsonFiles
{
    public class SSXOGLevelInterface
    {

        public void ExtractOGLevelFiles(string LoadPath, string ExtractPath)
        {
            WDXHandler wdxHandler = new WDXHandler();
            wdxHandler.Load(LoadPath + ".wdx");

            WDFHandler wdfHandler = new WDFHandler();
            wdfHandler.Load(LoadPath + ".wdf",wdxHandler.WDFGridGroups);

            MapHandler mapHandler = new MapHandler();
            mapHandler.Load(LoadPath + ".map");

            WDRHandler wdrHandler = new WDRHandler();
            wdrHandler.Load(LoadPath + ".wdr", wdxHandler.ModelOffsets);

            int Test = 0;
            for (int y = 0; y < wdrHandler.modelHeaders.Count; y++)
            {
                Test += wdrHandler.modelHeaders[y].models.Count;
            }

            Directory.CreateDirectory(ExtractPath + "\\Textures");
            Directory.CreateDirectory(ExtractPath + "\\Models");
            Directory.CreateDirectory(ExtractPath + "\\Lightmaps");
            Directory.CreateDirectory(ExtractPath + "\\Skybox");
            Directory.CreateDirectory(ExtractPath + "\\Skybox\\Textures");
            Directory.CreateDirectory(ExtractPath + "\\Skybox\\Models");

            wdrHandler.ExportModels(ExtractPath + "\\Models");

            MaterialsJsonHandler materialsJsonHandler = new MaterialsJsonHandler();
            materialsJsonHandler.Materials = new List<MaterialsJsonHandler.MaterialJson>();
            for (int i = 0; i < wdxHandler.Materials.Count; i++)
            {
                MaterialsJsonHandler.MaterialJson TempMaterialJson = new MaterialsJsonHandler.MaterialJson();

                TempMaterialJson.MaterialName = "Material " + i.ToString();

                TempMaterialJson.U0 = wdxHandler.Materials[i].U0;
                TempMaterialJson.TexturePath =  wdxHandler.Materials[wdxHandler.Materials[i].TextureID].TextureID.ToString("0000") + ".png";
                TempMaterialJson.U2 = wdxHandler.Materials[i].U2;
                TempMaterialJson.U3 = wdxHandler.Materials[i].U3;

                materialsJsonHandler.Materials.Add(TempMaterialJson);
            }
            materialsJsonHandler.CreateJson(ExtractPath + "\\Materials.json", true);

            PatchesJsonHandler patchesJsonHandler = new PatchesJsonHandler();
            patchesJsonHandler.Patches = new List<PatchesJsonHandler.PatchJson>();

            int PatchCount = 0;

            for (int y = 0; y < wdfHandler.WDFChunks.Count; y++)
            {
                var TempChunk = wdfHandler.WDFChunks[y];
                //for (int i = 0; i < length; i++)
                //{
                for (int i = 0; i < TempChunk.Patches.Count; i++)
                {
                    PatchesJsonHandler.PatchJson patch = new PatchesJsonHandler.PatchJson();
                    patch.PatchName = "Patch " + PatchCount;
                    PatchCount++;
                    patch.LightMapPoint = JsonUtil.Vector4ToArray(TempChunk.Patches[i].LightMapPoint);

                    patch.UVPoints = new float[4, 2];

                    patch.UVPoints[0, 0] = TempChunk.Patches[i].UVPoint1.X;
                    patch.UVPoints[0, 1] = TempChunk.Patches[i].UVPoint1.Y;

                    patch.UVPoints[1, 0] = TempChunk.Patches[i].UVPoint2.X;
                    patch.UVPoints[1, 1] = TempChunk.Patches[i].UVPoint2.Y;

                    patch.UVPoints[2, 0] = TempChunk.Patches[i].UVPoint3.X;
                    patch.UVPoints[2, 1] = TempChunk.Patches[i].UVPoint3.Y;

                    patch.UVPoints[3, 0] = TempChunk.Patches[i].UVPoint4.X;
                    patch.UVPoints[3, 1] = TempChunk.Patches[i].UVPoint4.Y;

                    BezierUtil bezierUtil = new BezierUtil();
                    bezierUtil.ProcessedPoints[0] = JsonUtil.Vector4ToVector3(TempChunk.Patches[i].R1C1);
                    bezierUtil.ProcessedPoints[1] = JsonUtil.Vector4ToVector3(TempChunk.Patches[i].R1C2);
                    bezierUtil.ProcessedPoints[2] = JsonUtil.Vector4ToVector3(TempChunk.Patches[i].R1C3);
                    bezierUtil.ProcessedPoints[3] = JsonUtil.Vector4ToVector3(TempChunk.Patches[i].R1C4);
                    bezierUtil.ProcessedPoints[4] = JsonUtil.Vector4ToVector3(TempChunk.Patches[i].R2C1);
                    bezierUtil.ProcessedPoints[5] = JsonUtil.Vector4ToVector3(TempChunk.Patches[i].R2C2);
                    bezierUtil.ProcessedPoints[6] = JsonUtil.Vector4ToVector3(TempChunk.Patches[i].R2C3);
                    bezierUtil.ProcessedPoints[7] = JsonUtil.Vector4ToVector3(TempChunk.Patches[i].R2C4);
                    bezierUtil.ProcessedPoints[8] = JsonUtil.Vector4ToVector3(TempChunk.Patches[i].R3C1);
                    bezierUtil.ProcessedPoints[9] = JsonUtil.Vector4ToVector3(TempChunk.Patches[i].R3C2);
                    bezierUtil.ProcessedPoints[10] = JsonUtil.Vector4ToVector3(TempChunk.Patches[i].R3C3);
                    bezierUtil.ProcessedPoints[11] = JsonUtil.Vector4ToVector3(TempChunk.Patches[i].R3C4);
                    bezierUtil.ProcessedPoints[12] = JsonUtil.Vector4ToVector3(TempChunk.Patches[i].R4C1);
                    bezierUtil.ProcessedPoints[13] = JsonUtil.Vector4ToVector3(TempChunk.Patches[i].R4C2);
                    bezierUtil.ProcessedPoints[14] = JsonUtil.Vector4ToVector3(TempChunk.Patches[i].R4C3);
                    bezierUtil.ProcessedPoints[15] = JsonUtil.Vector4ToVector3(TempChunk.Patches[i].R4C4);

                    bezierUtil.GenerateRawPoints();

                    patch.Points = new float[16, 3];

                    for (int a = 0; a < 16; a++)
                    {
                        patch.Points[a, 0] = bezierUtil.RawPoints[a].X;
                        patch.Points[a, 1] = bezierUtil.RawPoints[a].Y;
                        patch.Points[a, 2] = bezierUtil.RawPoints[a].Z;
                    }

                    patch.PatchStyle = TempChunk.Patches[i].PatchType;

                    patch.TexturePath = wdxHandler.Materials[TempChunk.Patches[i].TextureID].TextureID.ToString("0000") + ".png";
                    patch.LightmapID = TempChunk.Patches[i].LightmapID;
                    patchesJsonHandler.Patches.Add(patch);
                }
                //}
            }

            patchesJsonHandler.CreateJson(ExtractPath + "\\Patches.json", true);

            OldSSHHandler sshTexture = new OldSSHHandler();
            sshTexture.LoadSSH(LoadPath + ".ssh");

            for (int i = 0; i < sshTexture.sshImages.Count; i++)
            {
                sshTexture.BrightenBitmap(i);
                sshTexture.BMPOneExtract(ExtractPath + "\\Textures\\" + i.ToString("0000") + ".png", i);
            }

            OldSSHHandler sshTextureSky = new OldSSHHandler();
            sshTextureSky.LoadSSH(LoadPath + "_sky.ssh");

            for (int i = 0; i < sshTextureSky.sshImages.Count; i++)
            {
                sshTextureSky.BrightenBitmap(i);
                sshTextureSky.BMPOneExtract(ExtractPath + "\\Skybox\\Textures\\" + i.ToString("0000") + ".png", i);
            }

            OldSSHHandler sshTextureLight = new OldSSHHandler();
            sshTextureLight.LoadSSH(LoadPath + "l.ssh");

            for (int i = 0; i < sshTextureLight.sshImages.Count; i++)
            {
                //sshTextureLight.BrightenBitmap(i);
                sshTextureLight.BMPOneExtract(ExtractPath + "\\Lightmaps\\" + i.ToString("0000") + ".png", i);
            }

        }

    }
}
