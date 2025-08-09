using SSXMultiTool.FileHandlers.LevelFiles.OGPS2;
using SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2;
using SSXMultiTool.FileHandlers.SSX3;
using SSXMultiTool.FileHandlers.Textures;
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
        public static void Convert(string LoadPath, string ExportPath)
        {
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
            Directory.Move(TempPathOG + "/Models", TempPathTricky + "/Models");
            Directory.Move(TempPathOG + "/Collision", TempPathTricky + "/Collision");
            Directory.Move(TempPathOG + "/Skybox/Textures", TempPathTricky + "/Skybox/Textures");
            Directory.Move(TempPathOG + "/Skybox/Models", TempPathTricky + "/Skybox/Models");

            //Convert Patches
            SSXMultiTool.JsonFiles.Tricky.PatchesJsonHandler TrickyPatchPoints = new SSXMultiTool.JsonFiles.Tricky.PatchesJsonHandler();

            TrickyPatchPoints.CreateJson(ExportPath + "/Patches.json", false);

            //Convert Splines
            SSXMultiTool.JsonFiles.Tricky.SplineJsonHandler TrickySpline = new SSXMultiTool.JsonFiles.Tricky.SplineJsonHandler();

            TrickySpline.CreateJson(ExportPath + "/Splines.json", false);

            //Convert Materials
            SSXMultiTool.JsonFiles.Tricky.MaterialJsonHandler TrickyMaterialJsonHandler = new SSXMultiTool.JsonFiles.Tricky.MaterialJsonHandler();

            TrickyMaterialJsonHandler.CreateJson(ExportPath + "/Materials.json", false);

            //Convert Prefabs
            SSXMultiTool.JsonFiles.Tricky.PrefabJsonHandler TrickyPrefabJsonHandler = new SSXMultiTool.JsonFiles.Tricky.PrefabJsonHandler();

            TrickyPrefabJsonHandler.CreateJson(ExportPath + "/Prefabs.json", false);

            //Convert AIP & SOP
            SSXMultiTool.JsonFiles.Tricky.AIPSOPJsonHandler TrickyAIPJson = new SSXMultiTool.JsonFiles.Tricky.AIPSOPJsonHandler();

            TrickyAIPJson.CreateJson(ExportPath + "/AIP.json", false);
            TrickyAIPJson.CreateJson(ExportPath + "/SOP.json", false);
            //Convert Lights
            SSXMultiTool.JsonFiles.Tricky.LightJsonHandler TrickyLightJsonHandler = new SSXMultiTool.JsonFiles.Tricky.LightJsonHandler();

            TrickyLightJsonHandler.CreateJson(ExportPath + "/Lights.json", false);
            //Convert SSF Logic

            //Convert Skybox Materials
            SSXMultiTool.JsonFiles.Tricky.MaterialJsonHandler TrickySkyboxMaterialJsonHandler = new SSXMultiTool.JsonFiles.Tricky.MaterialJsonHandler();

            TrickySkyboxMaterialJsonHandler.CreateJson(ExportPath + "/Skybox/Materials.json", false);

            //Convert Skybox Prefabs
            SSXMultiTool.JsonFiles.Tricky.PrefabJsonHandler TrickySkyboxPrefabJsonHandler = new SSXMultiTool.JsonFiles.Tricky.PrefabJsonHandler();

            TrickySkyboxPrefabJsonHandler.CreateJson(ExportPath + "/Skybox/Prefabs.json", false);
        }
    }
}
