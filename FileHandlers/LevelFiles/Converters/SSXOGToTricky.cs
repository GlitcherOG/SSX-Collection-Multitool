using SSXMultiTool.JsonFiles;
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
        //Patches - Done
        //Prefabs -
        //Instances - 
        //Materials - Done
        //Splines - Done
        //AIP & SOP - Done but theres issues
        //Lights - Unknown in OG
        //SSF Logic - Unknown in OG
        //Skybox Materials - Done
        //Skybox Prefabs - Done
        //Skybox Models - Done
        //Skybox Textures - Done
        //Collision - Done
        //Lightmaps - Done
        //Textures - Done
        //Particle Instance - Done
        //Particle Prefabs - Done
        //Camera - Done


        public static void Convert(string LoadPath, string ExportPath)
        {
            ConsoleWindow.GenerateConsole();

            //Create Temp Locations
            string TempPathOG = Application.StartupPath + "/TempOG";
            string TempPathTricky = Application.StartupPath + "/TempTricky";
            Directory.CreateDirectory(TempPathOG);

            Directory.CreateDirectory(TempPathTricky);
            Directory.CreateDirectory(TempPathTricky + "/Skybox");

            SSXOGLevelInterface sSXOGLevelInterface = new SSXOGLevelInterface();
            sSXOGLevelInterface.ExtractOGLevelFiles(LoadPath, TempPathOG);

            //Copy Over Textures, Models, Collison and Skyboxs
            Directory.Move(TempPathOG + "/Textures", TempPathTricky + "/Textures");
            Directory.Move(TempPathOG + "/Lightmaps", TempPathTricky + "/Lightmaps");
            Directory.Move(TempPathOG + "/Models", TempPathTricky + "/Meshes");
            Directory.Move(TempPathOG + "/Collision", TempPathTricky + "/Collision");
            Directory.Move(TempPathOG + "/Skybox/Textures", TempPathTricky + "/Skybox/Textures");
            Directory.Move(TempPathOG + "/Skybox/Models", TempPathTricky + "/Skybox/Meshes");

            //Create Camera, Particles, 
            SSXMultiTool.JsonFiles.Tricky.CameraJSONHandler TrickyCamera = new SSXMultiTool.JsonFiles.Tricky.CameraJSONHandler();
            TrickyCamera.CreateJson(TempPathTricky + "/Cameras.json", false);

            SSXMultiTool.JsonFiles.Tricky.ParticleInstanceJsonHandler TrickyParticles = new SSXMultiTool.JsonFiles.Tricky.ParticleInstanceJsonHandler();
            TrickyParticles.CreateJson(TempPathTricky + "/ParticleInstances.json", false);
            SSXMultiTool.JsonFiles.Tricky.ParticleModelJsonHandler TrickyParticles1 = new SSXMultiTool.JsonFiles.Tricky.ParticleModelJsonHandler();
            TrickyParticles1.CreateJson(TempPathTricky + "/ParticlePrefabs.json", false);

            //Convert Patches
            SSXMultiTool.JsonFiles.Tricky.PatchesJsonHandler TrickyPatchPoints = new SSXMultiTool.JsonFiles.Tricky.PatchesJsonHandler();
            SSXMultiTool.JsonFiles.SSXOG.PatchesJsonHandler OGPatchPoints = SSXMultiTool.JsonFiles.SSXOG.PatchesJsonHandler.Load(TempPathOG + "/Patches.json");

            TrickyPatchPoints.Patches = new List<JsonFiles.Tricky.PatchesJsonHandler.PatchJson>();
            for (int i = 0; i < OGPatchPoints.Patches.Count; i++)
            {
                var Patch = new JsonFiles.Tricky.PatchesJsonHandler.PatchJson();

                Patch.PatchName = OGPatchPoints.Patches[i].PatchName;

                Patch.LightMapPoint = OGPatchPoints.Patches[i].LightMapPoint;
                Patch.UVPoints = OGPatchPoints.Patches[i].UVPoints;
                Patch.Points = OGPatchPoints.Patches[i].Points;

                Patch.SurfaceType = OGPatchPoints.Patches[i].PatchStyle;
                Patch.TexturePath = OGPatchPoints.Patches[i].TexturePath;
                Patch.LightmapID = OGPatchPoints.Patches[i].LightmapID;

                Patch.TrickOnlyPatch = false;

                TrickyPatchPoints.Patches.Add(Patch);
            }

            TrickyPatchPoints.CreateJson(TempPathTricky + "/Patches.json", false);

            //Convert Splines
            SSXMultiTool.JsonFiles.Tricky.SplineJsonHandler TrickySpline = new SSXMultiTool.JsonFiles.Tricky.SplineJsonHandler();
            SSXMultiTool.JsonFiles.SSXOG.SplinesJsonHandler OGSpline = SSXMultiTool.JsonFiles.SSXOG.SplinesJsonHandler.Load(TempPathOG + "/Splines.json");

            TrickySpline.Splines = new List<SplineJsonHandler.SplineJson>();
            for (int i = 0; i < OGSpline.Splines.Count; i++)
            {
                var TempSpline = new SplineJsonHandler.SplineJson();

                TempSpline.SplineName = OGSpline.Splines[i].SplineName;
                TempSpline.U0 = 1;
                TempSpline.U1 = 1;
                TempSpline.SplineStyle = 13;

                TempSpline.Segments = new List<SplineJsonHandler.SegmentJson>();

                for (global::System.Int32 j = 0; j < OGSpline.Splines[i].Segments.Count; j++)
                {
                    var TempSegment  = new SplineJsonHandler.SegmentJson();

                    TempSegment.Points = OGSpline.Splines[i].Segments[j].Points;

                    //TempSegment.U0 = OGSpline.Splines[i].Segments[j].U0;
                    //TempSegment.U1 = OGSpline.Splines[i].Segments[j].U1;
                    //TempSegment.U2 = OGSpline.Splines[i].Segments[j].U2;
                    //TempSegment.U3 = OGSpline.Splines[i].Segments[j].U3;

                    TempSpline.Segments.Add(TempSegment);
                }

                TrickySpline.Splines.Add(TempSpline);
            }

            TrickySpline.CreateJson(TempPathTricky + "/Splines.json", false);

            //Convert Materials
            SSXMultiTool.JsonFiles.Tricky.MaterialJsonHandler TrickyMaterialJsonHandler = new SSXMultiTool.JsonFiles.Tricky.MaterialJsonHandler();
            SSXMultiTool.JsonFiles.SSXOG.MaterialsJsonHandler OGMaterialJsonHandler = SSXMultiTool.JsonFiles.SSXOG.MaterialsJsonHandler.Load(TempPathOG + "/Materials.json");
            TrickyMaterialJsonHandler.Materials = new List<MaterialJsonHandler.MaterialsJson>();
            for (int i = 0; i < OGMaterialJsonHandler.Materials.Count; i++)
            {
                var material = new MaterialJsonHandler.MaterialsJson();

                material.MaterialName = OGMaterialJsonHandler.Materials[i].MaterialName;
                material.TexturePath = OGMaterialJsonHandler.Materials[i].TexturePath;
                material.UnknownInt2 = -1;
                material.UnknownInt3 = -1;
                material.UnknownFloat1 = 1;
                material.UnknownFloat2 = 1;
                material.UnknownFloat3 = 1;
                material.UnknownFloat4 = 6.998028E-39f;
                material.UnknownInt8 = 0;
                material.UnknownFloat5 = 6.948417E-39f;
                material.UnknownFloat6 = 0.5f;
                material.UnknownFloat7 = 0.5f;
                material.UnknownFloat8 = 0.5f;
                material.UnknownInt13 = 0;
                material.UnknownInt14 = 0;
                material.UnknownInt15 = 0;
                material.UnknownInt16 = 0;
                material.UnknownInt17 = 0;
                material.UnknownInt18 = 86024;
                material.UnknownInt20 = -1;

                TrickyMaterialJsonHandler.Materials.Add(material);
            }

            TrickyMaterialJsonHandler.CreateJson(TempPathTricky + "/Materials.json", false);

            //Convert Prefabs
            SSXMultiTool.JsonFiles.Tricky.ModelJsonHandler TrickyPrefabJsonHandler = new SSXMultiTool.JsonFiles.Tricky.ModelJsonHandler();
            SSXMultiTool.JsonFiles.SSXOG.PrefabJsonHandler prefabJsonHandler = SSXMultiTool.JsonFiles.SSXOG.PrefabJsonHandler.Load(TempPathOG + "/prefabs.json");
            TrickyPrefabJsonHandler.Models = new List<ModelJsonHandler.ModelJson>();
            for (int i = 0; i < prefabJsonHandler.Prefabs.Count; i++)
            {
                var TempPrefab = prefabJsonHandler.Prefabs[i];
                ModelJsonHandler.ModelJson model = new ModelJsonHandler.ModelJson();

                model.ModelName = "Model " + i.ToString();
                model.ModelObjects = new List<ModelJsonHandler.ObjectHeader>();

                for (global::System.Int32 j = 0; j < TempPrefab.models.Count; j++)
                {
                    var NewModelObjects = new ModelJsonHandler.ObjectHeader();

                    NewModelObjects.ObjectName = "Object " + j.ToString();
                    NewModelObjects.ParentID = -1;
                    NewModelObjects.IncludeMatrix = false;
                    NewModelObjects.IncludeAnimation = false;

                    NewModelObjects.MeshData = new List<ModelJsonHandler.MeshHeader>();

                    ModelJsonHandler.MeshHeader meshHeader = new ModelJsonHandler.MeshHeader();

                    meshHeader.MeshName = TempPrefab.models[j].MeshPath;
                    meshHeader.MeshPath = TempPrefab.models[j].MeshPath;
                    meshHeader.MaterialID = TempPrefab.models[j].MaterialID;

                    NewModelObjects.MeshData.Add(meshHeader);

                    model.ModelObjects.Add(NewModelObjects);
                }

                TrickyPrefabJsonHandler.Models.Add(model);
            }

            TrickyPrefabJsonHandler.CreateJson(TempPathTricky + "/Models.json", false);

            //Convert Instances
            SSXMultiTool.JsonFiles.Tricky.InstanceJsonHandler TrickyInstanceJsonHandler = new SSXMultiTool.JsonFiles.Tricky.InstanceJsonHandler();
            SSXMultiTool.JsonFiles.SSXOG.InstanceJsonHandler instanceJsonHandler = SSXMultiTool.JsonFiles.SSXOG.InstanceJsonHandler.Load(TempPathOG + "/Instances.json");
            TrickyInstanceJsonHandler.Instances = new List<JsonFiles.Tricky.InstanceJsonHandler.InstanceJson>();

            for (int i = 0; i < instanceJsonHandler.Instances.Count; i++)
            {
                var TempInstance = instanceJsonHandler.Instances[i];
                var NewInstance = new JsonFiles.Tricky.InstanceJsonHandler.InstanceJson();

                NewInstance.InstanceName = TempInstance.Name;

                NewInstance.Location = TempInstance.Location;
                NewInstance.Rotation = TempInstance.Rotation;
                NewInstance.Scale = TempInstance.Scale;

                NewInstance.LightColour1 = TempInstance.LightColour1;
                NewInstance.LightColour2 = TempInstance.LightColour2;
                NewInstance.LightColour3 = TempInstance.LightColour3;
                NewInstance.AmbentLightColour = TempInstance.AmbentLightColour;

                NewInstance.LightVector1 = TempInstance.LightVector1;
                NewInstance.LightVector2 = TempInstance.LightVector2;
                NewInstance.LightVector3 = TempInstance.LightVector3;
                NewInstance.AmbentLightVector = TempInstance.AmbentLightVector;

                NewInstance.ModelID = TempInstance.PrefabID;
                NewInstance.PrevInstance = -1;
                NewInstance.NextInstance = -1;

                NewInstance.LTGState = 0;

                NewInstance.Hash = i;

                NewInstance.IncludeSound = false;

                NewInstance.U0 = 0f;
                NewInstance.PlayerBounceAmmount = TempInstance.PlayerBounceValue;
                NewInstance.U2 = 0;
                NewInstance.Visable = TempInstance.Visable;
                NewInstance.PlayerCollision = TempInstance.PlayerCollision;
                NewInstance.PlayerBounce = TempInstance.PlayerBounce;
                NewInstance.Unknown241 = false;
                NewInstance.UVScroll = false;

                NewInstance.SurfaceType = 1;
                NewInstance.CollsionMode = TempInstance.CollsionMode;
                NewInstance.CollsionModelPaths = TempInstance.CollsionModelPaths;
                NewInstance.EffectSlotIndex = -1; //Swap later
                NewInstance.PhysicsIndex = -1;// TempInstance.PhysicsIndex;
                NewInstance.U8 = -1;

                TrickyInstanceJsonHandler.Instances.Add(NewInstance);
            }

            TrickyInstanceJsonHandler.CreateJson(TempPathTricky + "/Instances.json", false);

            //Convert AIP & SOP
            SSXMultiTool.JsonFiles.Tricky.AIPSOPJsonHandler TrickyAIPJson = new SSXMultiTool.JsonFiles.Tricky.AIPSOPJsonHandler();
            SSXMultiTool.JsonFiles.SSXOG.AIPJsonHandler OGAIPJson = SSXMultiTool.JsonFiles.SSXOG.AIPJsonHandler.Load(TempPathOG + "/AIP.json");

            TrickyAIPJson.StartPosList = OGAIPJson.StartPosList;

            TrickyAIPJson.AIPaths = new List<AIPSOPJsonHandler.PathA>();
            TrickyAIPJson.RaceLines = new List<AIPSOPJsonHandler.PathB>();

            for (int i = 0; i < OGAIPJson.AIPath.Count; i++)
            {
                var TempAI = new AIPSOPJsonHandler.PathA();

                //TempAI.Type = 2;
                //TempAI.U1 = 100;
                //TempAI.U2 = 4;
                TempAI.U3 = 50;
                //TempAI.U4 = 101;
                //TempAI.U5 = 4;
                TempAI.Respawnable = true;

                TempAI.PathPos = OGAIPJson.AIPath[i].PathPos;

                TempAI.PathPoints = OGAIPJson.AIPath[i].PathPoints;

                TempAI.PathEvents = new List<AIPSOPJsonHandler.PathEvent>();

                for (global::System.Int32 j = 0; j < OGAIPJson.AIPath[i].PathEvents.Count; j++)
                {
                    var TempEvent = new AIPSOPJsonHandler.PathEvent();

                    TempEvent.EventType = OGAIPJson.AIPath[i].PathEvents[j].EventType;
                    TempEvent.EventValue = OGAIPJson.AIPath[i].PathEvents[j].EventValue;
                    TempEvent.EventStart = OGAIPJson.AIPath[i].PathEvents[j].EventStart;
                    TempEvent.EventEnd = OGAIPJson.AIPath[i].PathEvents[j].EventEnd;

                    TempAI.PathEvents.Add(TempEvent);
                }

                TrickyAIPJson.AIPaths.Add(TempAI);
            }

            for (int i = 0; i < OGAIPJson.RaceLine.Count; i++)
            {
                var TempRace = new AIPSOPJsonHandler.PathB();

                //TempRace.Type = 1;
                //TempRace.U0 = 0;
                //TempRace.U1 = 4;
                TempRace.DistanceToFinish = OGAIPJson.RaceLine[i].U0;

                TempRace.PathPos = OGAIPJson.RaceLine[i].PathPos;

                TempRace.PathPoints = OGAIPJson.RaceLine[i].PathPoints;

                TempRace.PathEvents = new List<AIPSOPJsonHandler.PathEvent>();

                for (global::System.Int32 j = 0; j < OGAIPJson.RaceLine[i].PathEvents.Count; j++)
                {
                    var TempEvent = new AIPSOPJsonHandler.PathEvent();

                    TempEvent.EventType = OGAIPJson.RaceLine[i].PathEvents[j].EventType;
                    TempEvent.EventValue = OGAIPJson.RaceLine[i].PathEvents[j].EventValue;
                    TempEvent.EventStart = OGAIPJson.RaceLine[i].PathEvents[j].EventStart;
                    TempEvent.EventEnd = OGAIPJson.RaceLine[i].PathEvents[j].EventEnd;

                    TempRace.PathEvents.Add(TempEvent);
                }

                TrickyAIPJson.RaceLines.Add(TempRace);
            }

            TrickyAIPJson.CreateJson(TempPathTricky + "/AIP.json", false);
            TrickyAIPJson.CreateJson(TempPathTricky + "/SOP.json", false);
            //Convert Lights
            SSXMultiTool.JsonFiles.Tricky.LightJsonHandler TrickyLightJsonHandler = new SSXMultiTool.JsonFiles.Tricky.LightJsonHandler();

            TrickyLightJsonHandler.CreateJson(TempPathTricky + "/Lights.json", false);
            //Convert SSF Logic

            #region Skybox
            //Convert Skybox Materials
            SSXMultiTool.JsonFiles.Tricky.MaterialJsonHandler TrickySkyboxMaterialJsonHandler = new SSXMultiTool.JsonFiles.Tricky.MaterialJsonHandler();
            TrickySkyboxMaterialJsonHandler.Materials = new List<MaterialJsonHandler.MaterialsJson>();

            for (int i = 0; i < 25; i++)
            {
                var material = new MaterialJsonHandler.MaterialsJson();

                material.MaterialName = "Materials " + i.ToString();
                material.TexturePath = i.ToString().PadLeft(4, '0') + ".png";
                material.UnknownInt2 = -1;
                material.UnknownInt3 = -1;
                material.UnknownFloat1 = 1;
                material.UnknownFloat2 = 1;
                material.UnknownFloat3 = 1;
                material.UnknownFloat4 = 6.998028E-39f;
                material.UnknownInt8 = 0;
                material.UnknownFloat5 = 6.948417E-39f;
                material.UnknownFloat6 = 0.5f;
                material.UnknownFloat7 = 0.5f;
                material.UnknownFloat8 = 0.5f;
                material.UnknownInt13 = 0;
                material.UnknownInt14 = 0;
                material.UnknownInt15 = 0;
                material.UnknownInt16 = 0;
                material.UnknownInt17 = 0;
                material.UnknownInt18 = 86024;
                material.UnknownInt20 = -1;

                TrickySkyboxMaterialJsonHandler.Materials.Add(material);
            }

            TrickySkyboxMaterialJsonHandler.CreateJson(TempPathTricky + "/Skybox/Materials.json", false);

            //Convert Skybox Prefabs
            SSXMultiTool.JsonFiles.Tricky.ModelJsonHandler TrickySkyboxPrefabJsonHandler = new SSXMultiTool.JsonFiles.Tricky.ModelJsonHandler();
            SSXMultiTool.JsonFiles.SSXOG.PrefabJsonHandler OGSkyboxPrefabJsonHandler = SSXMultiTool.JsonFiles.SSXOG.PrefabJsonHandler.Load(TempPathOG + "/Skybox/Prefabs.json");

            TrickySkyboxPrefabJsonHandler.Models = new List<JsonFiles.Tricky.ModelJsonHandler.ModelJson>();

            JsonFiles.Tricky.ModelJsonHandler.ModelJson Prefab = new JsonFiles.Tricky.ModelJsonHandler.ModelJson();

            Prefab.ModelName = "Skybox Model 0";
            Prefab.Unknown3 = 3;
            Prefab.ModelObjects = new List<JsonFiles.Tricky.ModelJsonHandler.ObjectHeader>();

            var PrefabObject = new JsonFiles.Tricky.ModelJsonHandler.ObjectHeader();

            PrefabObject.ParentID = -1;

            PrefabObject.MeshData = new List<JsonFiles.Tricky.ModelJsonHandler.MeshHeader>();

            for (int i = 0; i < OGSkyboxPrefabJsonHandler.Prefabs[0].models.Count; i++)
            {
                var MeshData = new JsonFiles.Tricky.ModelJsonHandler.MeshHeader();

                MeshData.MeshPath = OGSkyboxPrefabJsonHandler.Prefabs[0].models[i].MeshPath;
                MeshData.MaterialID = OGSkyboxPrefabJsonHandler.Prefabs[0].models[i].MaterialID;

                PrefabObject.MeshData.Add(MeshData);
            }

            Prefab.ModelObjects.Add(PrefabObject);
            TrickySkyboxPrefabJsonHandler.Models.Add(Prefab);

            TrickySkyboxPrefabJsonHandler.CreateJson(TempPathTricky + "/Skybox/Models.json", false);

            #endregion

            SSXTrickyConfig trickyConfig = new SSXTrickyConfig();
            trickyConfig.CreateJson(TempPathTricky + "/ConfigTricky.ssx");

            TrickyLevelInterface trickyLevelInterface = new TrickyLevelInterface();
            trickyLevelInterface.BuildTrickyLevelFiles(TempPathTricky, ExportPath);
            ConsoleWindow.CloseConsole();
        }
    }
}
