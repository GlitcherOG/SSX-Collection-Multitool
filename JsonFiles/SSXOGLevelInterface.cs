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
using System.Numerics;
using System.Collections;

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

            WFXHandler wfxHandler = new WFXHandler();
            wfxHandler.Load(LoadPath + ".wfx");

            AIPHandler aipHandler = new AIPHandler();
            aipHandler.Load(LoadPath + ".aip");

            Directory.CreateDirectory(ExtractPath + "\\Textures");
            Directory.CreateDirectory(ExtractPath + "\\Models");
            Directory.CreateDirectory(ExtractPath + "\\Lightmaps");
            Directory.CreateDirectory(ExtractPath + "\\Skybox");
            Directory.CreateDirectory(ExtractPath + "\\Collision");
            Directory.CreateDirectory(ExtractPath + "\\Skybox\\Textures");
            Directory.CreateDirectory(ExtractPath + "\\Skybox\\Models");

            wdrHandler.ExportModels(ExtractPath + "\\Models");
            wfxHandler.SaveModels(ExtractPath + "\\Collision");

            MaterialsJsonHandler materialsJsonHandler = new MaterialsJsonHandler();
            materialsJsonHandler.Materials = new List<MaterialsJsonHandler.MaterialJson>();
            for (int i = 0; i < wdxHandler.Materials.Count; i++)
            {
                MaterialsJsonHandler.MaterialJson TempMaterialJson = new MaterialsJsonHandler.MaterialJson();

                TempMaterialJson.MaterialName = "Material " + i.ToString();

                TempMaterialJson.U0 = wdxHandler.Materials[i].U0;
                TempMaterialJson.TexturePath =  wdxHandler.Materials[i].TextureID.ToString("0000") + ".png";
                TempMaterialJson.U2 = wdxHandler.Materials[i].U2;
                TempMaterialJson.U3 = wdxHandler.Materials[i].U3;

                materialsJsonHandler.Materials.Add(TempMaterialJson);
            }

            materialsJsonHandler.CreateJson(ExtractPath + "\\Materials.json", true);

            PrefabJsonHandler prefabJsonHandler = new PrefabJsonHandler();
            prefabJsonHandler.Prefabs = new List<PrefabJsonHandler.PrefabJson>();
            for (int i = 0; i < wdrHandler.modelHeaders.Count; i++)
            {
                var TempPrefab = new PrefabJsonHandler.PrefabJson();

                TempPrefab.U1 = wdrHandler.modelHeaders[i].U1;
                TempPrefab.U2 = wdrHandler.modelHeaders[i].U2;
                TempPrefab.U3 = wdrHandler.modelHeaders[i].U3;
                TempPrefab.U4 = wdrHandler.modelHeaders[i].U4;

                TempPrefab.models = new List<PrefabJsonHandler.ObjectHeader>();

                for (int a = 0; a < wdrHandler.modelHeaders[i].models.Count; a++)
                {
                    var TempObjectHeader = new PrefabJsonHandler.ObjectHeader();

                    TempObjectHeader.MeshPath = wdrHandler.modelHeaders[i].models[a].MeshPath;

                    TempObjectHeader.U10 = wdrHandler.modelHeaders[i].models[a].U10;

                    TempObjectHeader.U12 = wdrHandler.modelHeaders[i].models[a].U12;
                    TempObjectHeader.MaterialID = wdrHandler.modelHeaders[i].models[a].MaterialID;
                    TempObjectHeader.U14 = wdrHandler.modelHeaders[i].models[a].U14;

                    TempObjectHeader.U16 = wdrHandler.modelHeaders[i].models[a].U16;

                    if(wdrHandler.modelHeaders[i].models[a].matrixData != null)
                    {
                        var TempMatrixData = new PrefabJsonHandler.MatrixData();

                        var OldMatrixData = wdrHandler.modelHeaders[i].models[a].matrixData.Value;

                        Vector3 Scale;
                        Quaternion Rotation;
                        Vector3 Location;

                        Matrix4x4.Decompose(OldMatrixData.matrix4, out Scale, out Rotation, out Location);
                        TempMatrixData.Location = JsonUtil.Vector3ToArray(Location);
                        TempMatrixData.Rotation = JsonUtil.QuaternionToArray(Rotation);
                        TempMatrixData.Scale = JsonUtil.Vector3ToArray(Scale);

                        TempMatrixData.U0 = OldMatrixData.U0;
                        TempMatrixData.U2 = OldMatrixData.U2;
                        TempMatrixData.U3 = OldMatrixData.U3;
                        TempMatrixData.U4 = OldMatrixData.U4;
                        TempMatrixData.U5 = OldMatrixData.U5;
                        TempMatrixData.U6 = OldMatrixData.U6;
                        TempMatrixData.U7 = OldMatrixData.U7;

                        TempMatrixData.uStruct0s = new List<PrefabJsonHandler.UStruct0>();

                        for (int b = 0; b < OldMatrixData.uStruct0s.Count; b++)
                        {
                            var UStruct0 = new PrefabJsonHandler.UStruct0();

                            UStruct0.uStruct1s = new List<PrefabJsonHandler.UStruct1>();

                            for (int c = 0; c < OldMatrixData.uStruct0s[b].uStruct1s.Count; c++)
                            {
                                var Ustruct1 = new PrefabJsonHandler.UStruct1();

                                Ustruct1.vector30 = JsonUtil.Vector3ToArray(OldMatrixData.uStruct0s[b].uStruct1s[c].vector30);
                                Ustruct1.vector31 = JsonUtil.Vector3ToArray(OldMatrixData.uStruct0s[b].uStruct1s[c].vector31);
                                Ustruct1.U0 = OldMatrixData.uStruct0s[b].uStruct1s[c].U0;
                                Ustruct1.U1 = OldMatrixData.uStruct0s[b].uStruct1s[c].U1;

                                UStruct0.uStruct1s.Add(Ustruct1);
                            }

                            TempMatrixData.uStruct0s.Add(UStruct0);
                        }

                        TempObjectHeader.matrixData = TempMatrixData;
                    }

                    TempPrefab.models.Add(TempObjectHeader);
                }

                prefabJsonHandler.Prefabs.Add(TempPrefab);
            }

            prefabJsonHandler.CreateJson(ExtractPath + "\\Prefabs.json", true);

            PatchesJsonHandler patchesJsonHandler = new PatchesJsonHandler();
            patchesJsonHandler.Patches = new List<PatchesJsonHandler.PatchJson>();

            InstanceJsonHandler instanceJsonHandler = new InstanceJsonHandler();
            instanceJsonHandler.Instances = new List<InstanceJsonHandler.InstanceJson>();

            int PatchCount = 0;

            for (int y = 0; y < wdfHandler.WDFChunks.GetLength(1); y++)
            {
                for (int x = 0; x < wdfHandler.WDFChunks.GetLength(0); x++)
                {
                    var TempChunk = wdfHandler.WDFChunks[x, y];
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

                    for (int i = 0; i < TempChunk.Instances.Count; i++)
                    {
                        var TempInstanceJson = new InstanceJsonHandler.InstanceJson();

                        TempInstanceJson.Name = mapHandler.GetInstanceName(TempChunk.Instances[i].WDFGridID, TempChunk.Instances[i].InstanceIndex);

                        var OldMatrixData = TempChunk.Instances[i].matrix4X4;

                        Vector3 Scale;
                        Quaternion Rotation;
                        Vector3 Location;

                        Matrix4x4.Decompose(OldMatrixData, out Scale, out Rotation, out Location);
                        TempInstanceJson.Location = JsonUtil.Vector3ToArray(Location);
                        TempInstanceJson.Rotation = JsonUtil.QuaternionToArray(Rotation);
                        TempInstanceJson.Scale = JsonUtil.Vector3ToArray(Scale);

                        TempInstanceJson.U2 = TempChunk.Instances[i].U2;
                        TempInstanceJson.U3 = TempChunk.Instances[i].U3;
                        TempInstanceJson.PrefabID = TempChunk.Instances[i].PrefabID;

                        TempInstanceJson.U5 = TempChunk.Instances[i].U5;

                        byte[] TempBytes = BitConverter.GetBytes(TempChunk.Instances[i].BitFlags);

                        BitArray bitArray = new BitArray(TempBytes);

                        TempInstanceJson.Visable = bitArray[0];
                        TempInstanceJson.PlayerCollision = bitArray[5];
                        TempInstanceJson.PlayerBounce = bitArray[7];

                        TempInstanceJson.PlayerBounceValue = TempChunk.Instances[i].PlayerBounce;

                        TempInstanceJson.CollsionMode = TempChunk.Instances[i].CollsionMode;
                        TempInstanceJson.CollisonModelIndex = TempChunk.Instances[i].CollisonModelIndex;
                        TempInstanceJson.PhysicsIndex = TempChunk.Instances[i].PhysicsIndex;
                        TempInstanceJson.U11 = TempChunk.Instances[i].U11;

                        TempInstanceJson.U12 = TempChunk.Instances[i].U12;
                        TempInstanceJson.U13 = TempChunk.Instances[i].U13;

                        TempInstanceJson.U14 = TempChunk.Instances[i].U14;
                        TempInstanceJson.U15 = TempChunk.Instances[i].U15;
                        TempInstanceJson.U16 = TempChunk.Instances[i].U16;
                        TempInstanceJson.U17 = TempChunk.Instances[i].U17;

                        instanceJsonHandler.Instances.Add(TempInstanceJson);
                    }
                }
            }
            instanceJsonHandler.CreateJson(ExtractPath + "\\Instances.json", true);
            patchesJsonHandler.CreateJson(ExtractPath + "\\Patches.json", true);

            SplinesJsonHandler splinesJsonHandler = new SplinesJsonHandler();
            splinesJsonHandler.Splines = new List<SplinesJsonHandler.SplineJson>();

            for (int i = 0; i < wdxHandler.Splines.Count; i++)
            {
                var NewSpline = new SplinesJsonHandler.SplineJson();

                NewSpline.vector3 = JsonUtil.Vector3ToArray(wdxHandler.Splines[i].vector3);
                NewSpline.vector31 = JsonUtil.Vector3ToArray(wdxHandler.Splines[i].vector31);

                NewSpline.U0 = wdxHandler.Splines[i].U0;
                NewSpline.U1 = wdxHandler.Splines[i].U1;
                NewSpline.U2 = wdxHandler.Splines[i].U2;

                NewSpline.U3 = wdxHandler.Splines[i].U3;
                NewSpline.U4 = wdxHandler.Splines[i].U4;
                NewSpline.U5 = wdxHandler.Splines[i].U5;
                NewSpline.U6 = wdxHandler.Splines[i].U6;
                NewSpline.U7 = wdxHandler.Splines[i].U7;
                NewSpline.U8 = wdxHandler.Splines[i].U8;
                NewSpline.U9 = wdxHandler.Splines[i].U9;
                NewSpline.U10 = wdxHandler.Splines[i].U10;

                splinesJsonHandler.Splines.Add(NewSpline);
            }
            splinesJsonHandler.CreateJson(ExtractPath + "\\Splines.json", true);

            #region AIP
            AIPJsonHandler aipJsonHandler = new AIPJsonHandler();
            aipJsonHandler.U2 = aipHandler.U2;
            aipJsonHandler.U3 = aipHandler.U3;
            aipJsonHandler.U4 = aipHandler.U4;
            aipJsonHandler.U5 = aipHandler.U5;
            aipJsonHandler.U6 = aipHandler.U6;
            aipJsonHandler.U7 = aipHandler.U7;
            aipJsonHandler.U8 = aipHandler.U8;
            aipJsonHandler.U9 = aipHandler.U9;
            aipJsonHandler.U10 = aipHandler.U10;

            aipJsonHandler.PathAs = new List<AIPJsonHandler.PathData>();
            aipJsonHandler.PathBs = new List<AIPJsonHandler.PathData>();

            for (int i = 0; i < aipHandler.PathAs.Count; i++)
            {
                var TempPath = aipHandler.PathAs[i];
                AIPJsonHandler.PathData NewPath = new AIPJsonHandler.PathData();

                NewPath.U0 = TempPath.U0;
                NewPath.PathPos = JsonUtil.Vector3ToArray(TempPath.PathPos);

                NewPath.PathPoints = new float[aipHandler.PathAs[i].VectorPoints.Count, 3];
                for (int a = 0; a < aipHandler.PathAs[i].VectorPoints.Count; a++)
                {
                    NewPath.PathPoints[a, 0] = aipHandler.PathAs[i].VectorPoints[a].X * aipHandler.PathAs[i].VectorPoints[a].W;
                    NewPath.PathPoints[a, 1] = aipHandler.PathAs[i].VectorPoints[a].Y * aipHandler.PathAs[i].VectorPoints[a].W;
                    NewPath.PathPoints[a, 2] = aipHandler.PathAs[i].VectorPoints[a].Z * aipHandler.PathAs[i].VectorPoints[a].W;
                    if (a != 0)
                    {
                        NewPath.PathPoints[a, 0] += NewPath.PathPoints[a - 1, 0];
                        NewPath.PathPoints[a, 1] += NewPath.PathPoints[a - 1, 1];
                        NewPath.PathPoints[a, 2] += NewPath.PathPoints[a - 1, 2];
                    }
                }

                NewPath.PathEvents = new List<AIPJsonHandler.PathEvent>();
                for (int a = 0; a < aipHandler.PathAs[i].PathEvents.Count; a++)
                {
                    var NewStruct = new AIPJsonHandler.PathEvent();
                    NewStruct.U0 = aipHandler.PathAs[i].PathEvents[a].U0;
                    NewStruct.U1 = aipHandler.PathAs[i].PathEvents[a].U1;
                    NewStruct.U2 = aipHandler.PathAs[i].PathEvents[a].U2;
                    NewStruct.U3 = aipHandler.PathAs[i].PathEvents[a].U3;
                    NewPath.PathEvents.Add(NewStruct);
                }
                aipJsonHandler.PathAs.Add(NewPath);
            }

            for (int i = 0; i < aipHandler.PathBs.Count; i++)
            {
                var TempPath = aipHandler.PathBs[i];
                AIPJsonHandler.PathData NewPath = new AIPJsonHandler.PathData();

                NewPath.U0 = TempPath.U0;
                NewPath.PathPos = JsonUtil.Vector3ToArray(TempPath.PathPos);

                NewPath.PathPoints = new float[aipHandler.PathBs[i].VectorPoints.Count, 3];
                for (int a = 0; a < aipHandler.PathBs[i].VectorPoints.Count; a++)
                {
                    NewPath.PathPoints[a, 0] = aipHandler.PathBs[i].VectorPoints[a].X * aipHandler.PathBs[i].VectorPoints[a].W;
                    NewPath.PathPoints[a, 1] = aipHandler.PathBs[i].VectorPoints[a].Y * aipHandler.PathBs[i].VectorPoints[a].W;
                    NewPath.PathPoints[a, 2] = aipHandler.PathBs[i].VectorPoints[a].Z * aipHandler.PathBs[i].VectorPoints[a].W;
                    if (a != 0)
                    {
                        NewPath.PathPoints[a, 0] += NewPath.PathPoints[a - 1, 0];
                        NewPath.PathPoints[a, 1] += NewPath.PathPoints[a - 1, 1];
                        NewPath.PathPoints[a, 2] += NewPath.PathPoints[a - 1, 2];
                    }
                }

                NewPath.PathEvents = new List<AIPJsonHandler.PathEvent>();
                for (int a = 0; a < aipHandler.PathBs[i].PathEvents.Count; a++)
                {
                    var NewStruct = new AIPJsonHandler.PathEvent();
                    NewStruct.U0 = aipHandler.PathBs[i].PathEvents[a].U0;
                    NewStruct.U1 = aipHandler.PathBs[i].PathEvents[a].U1;
                    NewStruct.U2 = aipHandler.PathBs[i].PathEvents[a].U2;
                    NewStruct.U3 = aipHandler.PathBs[i].PathEvents[a].U3;
                    NewPath.PathEvents.Add(NewStruct);
                }
                aipJsonHandler.PathBs.Add(NewPath);
            }

            aipJsonHandler.CreateJson(ExtractPath + "\\AIP.json", true);
            #endregion

            #region Textures
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
            #endregion

            SSXOGConfig ssxOGConfig = new SSXOGConfig();
            ssxOGConfig.CreateJson(ExtractPath + "\\OGConfig.ssx");
        }

    }
}
