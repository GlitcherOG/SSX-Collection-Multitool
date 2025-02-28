using SSXMultiTool.FileHandlers.LevelFiles.OGPS2;
using SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2;
using SSXMultiTool.FileHandlers.SSX3;
using SSXMultiTool.FileHandlers.Textures;
using SSXMultiTool.JsonFiles.SSXOG;
using SSXMultiTool.JsonFiles.Tricky;
using SSXMultiTool.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.LevelFiles.Converters
{
    public class SSXOGToTricky
    {
        public static void Convert(string LoadPath, string ExportPath)
        {
            OGPS2.WDXHandler wdxHandler = new WDXHandler();
            wdxHandler.Load(LoadPath + ".wdx");

            OGPS2.WDFHandler wdfHandler = new WDFHandler();
            wdfHandler.Load(LoadPath + ".wdf", wdxHandler.WDFGridGroups);

            OGPS2.MapHandler mapHandler = new OGPS2.MapHandler();
            mapHandler.Load(LoadPath + ".map");

            OGPS2.WDRHandler wdrHandler = new WDRHandler();
            wdrHandler.Load(LoadPath + ".wdr", wdxHandler.ModelOffsets);

            OGPS2.WFXHandler wfxHandler = new WFXHandler();
            wfxHandler.Load(LoadPath + ".wfx");

            OGPS2.AIPHandler aipHandler = new AIPHandler();
            aipHandler.Load(LoadPath + ".aip");

            OldSSHHandler sshTexture = new OldSSHHandler();
            sshTexture.LoadSSH(LoadPath + ".ssh");

            OldSSHHandler sshTextureSky = new OldSSHHandler();
            sshTextureSky.LoadSSH(LoadPath + "_sky.ssh");

            OldSSHHandler sshTextureLight = new OldSSHHandler();
            sshTextureLight.LoadSSH(LoadPath + "l.ssh");

            PBDHandler pbdHandler = new PBDHandler();
            LTGHandler ltgHandler = new LTGHandler();

            //Save Textures
            for (int i = 0; i < sshTexture.sshImages.Count; i++)
            {
                var Texture = sshTexture.sshImages[i];
                Texture.shortname = i.ToString().PadLeft(4, '0');
                sshTexture.sshImages[i] = Texture;
            }
            sshTexture.SaveSSH(ExportPath + ".ssh", true);

            for (int i = 0; i < sshTextureSky.sshImages.Count; i++)
            {
                var Texture = sshTextureSky.sshImages[i];
                Texture.shortname = i.ToString().PadLeft(4, '0');
                sshTextureSky.sshImages[i] = Texture;
            }
            sshTextureSky.SaveSSH(ExportPath + "_sky.ssh", true);

            for (int i = 0; i < sshTextureLight.sshImages.Count; i++)
            {
                var Texture = sshTextureLight.sshImages[i];
                Texture.shortname = i.ToString().PadLeft(4, '0');
                sshTextureLight.sshImages[i] = Texture;
            }
            sshTextureLight.SaveSSH(ExportPath + "_L.ssh", true);

            //Convert Instances and Patches
            pbdHandler.Patches = new List<Patch>();

            for (int y = 0; y < wdfHandler.WDFChunks.GetLength(1); y++)
            {
                for (int x = 0; x < wdfHandler.WDFChunks.GetLength(0); x++)
                {
                    var TempChunk = wdfHandler.WDFChunks[x, y];
                    for (int i = 0; i < TempChunk.Patches.Count; i++)
                    {
                        TrickyPS2.Patch patch = new Patch();

                        patch.LightMapPoint = TempChunk.Patches[i].LightMapPoint;

                        patch.UVPoint1 = TempChunk.Patches[i].UVPoint1;
                        patch.UVPoint2 = TempChunk.Patches[i].UVPoint2;
                        patch.UVPoint3 = TempChunk.Patches[i].UVPoint3;
                        patch.UVPoint4 = TempChunk.Patches[i].UVPoint4;

                        patch.R1C1 = TempChunk.Patches[i].R1C1;
                        patch.R1C2 = TempChunk.Patches[i].R1C2;
                        patch.R1C3 = TempChunk.Patches[i].R1C3;
                        patch.R1C4 = TempChunk.Patches[i].R1C4;

                        patch.R2C1 = TempChunk.Patches[i].R2C1;
                        patch.R2C2 = TempChunk.Patches[i].R2C2;
                        patch.R2C3 = TempChunk.Patches[i].R2C3;
                        patch.R2C4 = TempChunk.Patches[i].R2C4;

                        patch.R3C1 = TempChunk.Patches[i].R3C1;
                        patch.R3C2 = TempChunk.Patches[i].R3C2;
                        patch.R3C3 = TempChunk.Patches[i].R3C3;
                        patch.R3C4 = TempChunk.Patches[i].R3C4;

                        patch.R4C1 = TempChunk.Patches[i].R4C1;
                        patch.R4C2 = TempChunk.Patches[i].R4C2;
                        patch.R4C3 = TempChunk.Patches[i].R4C3;
                        patch.R4C4 = TempChunk.Patches[i].R4C4;

                        patch.Point1 = TempChunk.Patches[i].Point1;
                        patch.Point2 = TempChunk.Patches[i].Point2;
                        patch.Point3 = TempChunk.Patches[i].Point3;
                        patch.Point4 = TempChunk.Patches[i].Point4;

                        patch.LowestXYZ = JsonUtil.Vector4ToVector3(TempChunk.Patches[i].LowestXYZ);
                        patch.HighestXYZ = JsonUtil.Vector4ToVector3(TempChunk.Patches[i].HighestXYZ);

                        patch.Unknown2 = 41;
                        patch.PatchVisablity = 32768;

                        patch.SurfaceType = TempChunk.Patches[i].PatchType;
                        patch.TextureAssigment = wdxHandler.Materials[TempChunk.Patches[i].TextureID].TextureID;
                        patch.LightmapID = TempChunk.Patches[i].LightmapID;

                        pbdHandler.Patches.Add(patch);
                    }

                    for (int i = 0; i < TempChunk.Instances.Count; i++)
                    {
                        //var TempInstanceJson = new InstanceJsonHandler.InstanceJson();

                        //TempInstanceJson.Name = mapHandler.GetInstanceName(TempChunk.Instances[i].WDFGridID, TempChunk.Instances[i].InstanceIndex);

                        //var OldMatrixData = TempChunk.Instances[i].matrix4X4;

                        //Vector3 Scale;
                        //Quaternion Rotation;
                        //Vector3 Location;

                        //Matrix4x4.Decompose(OldMatrixData, out Scale, out Rotation, out Location);
                        //TempInstanceJson.Location = JsonUtil.Vector3ToArray(Location);
                        //TempInstanceJson.Rotation = JsonUtil.QuaternionToArray(Rotation);
                        //TempInstanceJson.Scale = JsonUtil.Vector3ToArray(Scale);

                        //var TempMatrix = TempChunk.Instances[i].lightingMatrix4x4;

                        //TempInstanceJson.LightVector1 = new float[] { TempMatrix.M11, TempMatrix.M21, TempMatrix.M31, TempMatrix.M41 };
                        //TempInstanceJson.LightVector2 = new float[] { TempMatrix.M12, TempMatrix.M22, TempMatrix.M32, TempMatrix.M42 };
                        //TempInstanceJson.LightVector3 = new float[] { TempMatrix.M13, TempMatrix.M23, TempMatrix.M33, TempMatrix.M43 };
                        //TempInstanceJson.AmbentLightVector = new float[] { TempMatrix.M14, TempMatrix.M24, TempMatrix.M34, TempMatrix.M44 };

                        //TempInstanceJson.LightColour1 = JsonUtil.Vector4ToArray(TempChunk.Instances[i].LightColour1);
                        //TempInstanceJson.LightColour2 = JsonUtil.Vector4ToArray(TempChunk.Instances[i].LightColour2);
                        //TempInstanceJson.LightColour3 = JsonUtil.Vector4ToArray(TempChunk.Instances[i].LightColour3);
                        //TempInstanceJson.AmbentLightColour = JsonUtil.Vector4ToArray(TempChunk.Instances[i].AmbentLightColour);

                        //TempInstanceJson.U2 = TempChunk.Instances[i].U2;
                        //TempInstanceJson.U3 = TempChunk.Instances[i].U3;
                        //TempInstanceJson.PrefabID = TempChunk.Instances[i].PrefabID;

                        //TempInstanceJson.U5 = TempChunk.Instances[i].U5;

                        //byte[] TempBytes = BitConverter.GetBytes(TempChunk.Instances[i].BitFlags);

                        //BitArray bitArray = new BitArray(TempBytes);

                        //TempInstanceJson.Visable = bitArray[0];
                        //TempInstanceJson.PlayerCollision = bitArray[5];
                        //TempInstanceJson.PlayerBounce = bitArray[7];

                        //TempInstanceJson.PlayerBounceValue = TempChunk.Instances[i].PlayerBounce;

                        //TempInstanceJson.CollsionMode = TempChunk.Instances[i].CollsionMode;

                        //int CollsionPos = TempChunk.Instances[i].CollisonModelIndex;
                        //if (CollsionPos != -1 /*&& TempChunk.Instances[i].CollsionMode != 3*/)
                        //{
                        //    TempInstanceJson.CollsionModelPaths = new string[wfxHandler.CollisonModelPointers[CollsionPos].Models.Count];

                        //    for (int a = 0; a < TempInstanceJson.CollsionModelPaths.Length; a++)
                        //    {
                        //        TempInstanceJson.CollsionModelPaths[a] = wfxHandler.CollisonModelPointers[CollsionPos].Models[a].MeshPath;
                        //    }
                        //}

                        //TempInstanceJson.PhysicsIndex = TempChunk.Instances[i].PhysicsIndex;
                        //TempInstanceJson.U11 = TempChunk.Instances[i].U11;

                        //TempInstanceJson.U12 = TempChunk.Instances[i].U12;
                        //TempInstanceJson.U13 = TempChunk.Instances[i].U13;

                        //TempInstanceJson.U14 = TempChunk.Instances[i].U14;
                        //TempInstanceJson.U15 = TempChunk.Instances[i].U15;
                        //TempInstanceJson.U16 = TempChunk.Instances[i].U16;
                        //TempInstanceJson.U17 = TempChunk.Instances[i].U17;

                        //instanceJsonHandler.Instances.Add(TempInstanceJson);
                    }
                }
            }


            pbdHandler.MeshData = new byte[0];

            //LTG Regenerate
            pbdHandler.RegenerateLowestAndHighest();
            ltgHandler.RegenerateOriginLTG(pbdHandler);
            ltgHandler.SaveLTGFile(ExportPath + ".ltg");

            pbdHandler.SaveNew(ExportPath + ".pbd");

            //AIP/SOP
            TrickyPS2.AIPSOPHandler aipTrickyHandler = new TrickyPS2.AIPSOPHandler();
            aipTrickyHandler.AIPath = new AIPSOPHandler.TypeA();
            aipTrickyHandler.AIPath.StartPosList = new List<int>();// aipHandler.StartPosList;

            aipTrickyHandler.AIPath.StartPosList.Add(0);
            aipTrickyHandler.AIPath.StartPosList.Add(1);
            aipTrickyHandler.AIPath.StartPosList.Add(2);
            aipTrickyHandler.AIPath.StartPosList.Add(3);
            aipTrickyHandler.AIPath.StartPosList.Add(4);
            aipTrickyHandler.AIPath.StartPosList.Add(5);

            aipTrickyHandler.AIPath.PathAs = new List<TrickyPS2.AIPSOPHandler.PathA>();
            for (int i = 0; i < aipHandler.PathAs.Count; i++)
            {
                var NewAIPATH = new AIPSOPHandler.PathA();

                //NewAIPATH.U0 = aipHandler.PathAs[i].U0;

                //        NewAIPATH.Type = aip.AIPaths[i].Type;
                //        NewAIPATH.U1 = aip.AIPaths[i].U1;
                //        NewAIPATH.U2 = aip.AIPaths[i].U2;
                //        NewAIPATH.U3 = aip.AIPaths[i].U3;
                //        NewAIPATH.U4 = aip.AIPaths[i].U4;
                //        NewAIPATH.U5 = aip.AIPaths[i].U5;
                //        NewAIPATH.Respawnable = aip.AIPaths[i].Respawnable;

                NewAIPATH.Respawnable = 1;

                NewAIPATH.PathPos = aipHandler.PathAs[i].PathPos;
                NewAIPATH.VectorPoints = new List<Vector4>();

                NewAIPATH.VectorPoints = aipHandler.PathAs[i].VectorPoints;

                NewAIPATH.BBoxMax = aipHandler.PathAs[i].BBoxMax;
                NewAIPATH.BBoxMin = aipHandler.PathAs[i].BBoxMin;

                NewAIPATH.PathEvents = new List<AIPSOPHandler.PathEvent>();
                for (int a = 0; a < aipHandler.PathAs[i].PathEvents.Count; a++)
                {
                    var NewStruct = new AIPSOPHandler.PathEvent();
                    NewStruct.EventType = aipHandler.PathAs[i].PathEvents[a].EventType;
                    NewStruct.EventValue = aipHandler.PathAs[i].PathEvents[a].EventValue;
                    NewStruct.EventStart = aipHandler.PathAs[i].PathEvents[a].EventStart;
                    NewStruct.EventEnd = aipHandler.PathAs[i].PathEvents[a].EventEnd;
                    NewAIPATH.PathEvents.Add(NewStruct);
                }

                aipTrickyHandler.AIPath.PathAs.Add(NewAIPATH);
            }

            aipTrickyHandler.RaceLine.PathBs = new List<TrickyPS2.AIPSOPHandler.PathB>();
            for (int i = 0; i < aipHandler.PathBs.Count; i++)
            {
                var NewAIPATH = new AIPSOPHandler.PathB();

                //        NewAIPATH.Type = aip.RaceLines[i].Type;
                //        NewAIPATH.U0 = aip.RaceLines[i].U0;
                //        NewAIPATH.U1 = aip.RaceLines[i].U1;
                //        NewAIPATH.U2 = aip.RaceLines[i].U2;

                NewAIPATH.PathPos = aipHandler.PathBs[i].PathPos;
                NewAIPATH.VectorPoints = new List<Vector4>();

                NewAIPATH.VectorPoints = aipHandler.PathBs[i].VectorPoints;

                NewAIPATH.BBoxMax = aipHandler.PathBs[i].BBoxMax;
                NewAIPATH.BBoxMin = aipHandler.PathBs[i].BBoxMin;

                NewAIPATH.PathEvents = new List<AIPSOPHandler.PathEvent>();
                for (int a = 0; a < aipHandler.PathBs[i].PathEvents.Count; a++)
                {
                    var NewStruct = new AIPSOPHandler.PathEvent();
                    NewStruct.EventType = aipHandler.PathBs[i].PathEvents[a].EventType;
                    NewStruct.EventValue = aipHandler.PathBs[i].PathEvents[a].EventValue;
                    NewStruct.EventStart = aipHandler.PathBs[i].PathEvents[a].EventStart;
                    NewStruct.EventEnd = aipHandler.PathBs[i].PathEvents[a].EventEnd;
                    NewAIPATH.PathEvents.Add(NewStruct);
                }

                aipTrickyHandler.RaceLine.PathBs.Add(NewAIPATH);
            }

            aipTrickyHandler.SaveAIPSOP(ExportPath + ".aip");
        }
    }
}
