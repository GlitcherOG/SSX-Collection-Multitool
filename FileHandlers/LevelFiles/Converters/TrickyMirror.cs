using SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2;
using SSXMultiTool.JsonFiles.Tricky;
using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace SSXMultiTool.FileHandlers.LevelFiles.Converters
{
    public class TrickyMirror
    {
        public static void ProjectMirror(string LoadPath, string BuildPath)
        {
            //Patches - Done
            //Instances - Done
            //Models/Prefabs
            //Splines - Done
            //Lights - Done Somewhat
            //Particles - Done Somewhat
            //Collision
            //Path General AI
            //Path General Raceline
            //Path Showoff AI
            //Path Showoff Raceline

            ConsoleWindow.GenerateConsole();

            string TempPath = Application.StartupPath + "\\Temp";

            Directory.CreateDirectory(TempPath);

            Console.WriteLine("Begining Level Extraction...");
            var trickyLevelInterface = new TrickyLevelInterface();

            trickyLevelInterface.ExtractTrickyLevelFiles(LoadPath, TempPath);

            Console.WriteLine("Scaling Level Files");
            //Patches
            PatchesJsonHandler patchesJsonHandler = new PatchesJsonHandler();
            patchesJsonHandler = PatchesJsonHandler.Load(TempPath + "\\Patches.json");

            for (int i = 0; i < patchesJsonHandler.Patches.Count; i++)
            {
                var Patch = patchesJsonHandler.Patches[i];

                for (global::System.Int32 j = 0; j < 16; j++)
                {
                    //Patch.Points[j, 0] = Patch.Points[j, 0] * -1f;
                    Patch.Points[j, 1] = Patch.Points[j, 1] * -1f;
                    //Patch.Points[j, 2] = Patch.Points[j, 2] * Scale;
                }

                patchesJsonHandler.Patches[i] = Patch;
            }

            patchesJsonHandler.CreateJson(TempPath + "\\Patches.json");
            //Instances

            InstanceJsonHandler instanceJsonHandler = new InstanceJsonHandler();
            instanceJsonHandler = InstanceJsonHandler.Load(TempPath + "\\Instances.json");

            for (int i = 0; i < instanceJsonHandler.Instances.Count; i++)
            {
                var Instance = instanceJsonHandler.Instances[i];

                //Instance.Location[0] = Instance.Location[0] * -1f;
                Instance.Location[1] = Instance.Location[1] * -1f;
                //Instance.Location[2] = Instance.Location[2] * Scale;

                instanceJsonHandler.Instances[i] = Instance;
            }
            instanceJsonHandler.CreateJson(TempPath + "\\Instances.json");

            //Models
            PrefabJsonHandler prefabsJsonHandler = new PrefabJsonHandler();
            prefabsJsonHandler = PrefabJsonHandler.Load(TempPath + "\\Prefabs.json");

            for (int i = 0; i < prefabsJsonHandler.Prefabs.Count; i++)
            {
                var Prefab = prefabsJsonHandler.Prefabs[i];

                for (global::System.Int32 j = 0; j < Prefab.PrefabObjects.Count; j++)
                {
                    var PrefabObject = Prefab.PrefabObjects[j];

                    if (PrefabObject.Position != null)
                    {
                        //PrefabObject.Position[0] = PrefabObject.Position[0] * -1f;
                        PrefabObject.Position[1] = PrefabObject.Position[1] * -1f;
                        //PrefabObject.Position[2] = PrefabObject.Position[2] * Scale;
                    }

                    Prefab.PrefabObjects[j] = PrefabObject;
                }

                prefabsJsonHandler.Prefabs[i] = Prefab;
            }
            prefabsJsonHandler.CreateJson(TempPath + "\\Prefabs.json");

            //Models
            //Get Models
            //Read through each line of models and scale V points
            string[] paths = Directory.GetFiles(TempPath + "\\Models\\", "*.obj");
            for (int i = 0; i < paths.Length; i++)
            {
                string[] Lines = File.ReadAllLines(paths[i]);

                for (global::System.Int32 j = 0; j < Lines.Length; j++)
                {
                    string[] splitLine = Lines[j].Split(' ');
                    if (Lines[j].StartsWith("v "))
                    {
                        Vector3 vector3 = new Vector3();
                        vector3.X = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                        vector3.Y = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat) * -1f;
                        vector3.Z = float.Parse(splitLine[3], CultureInfo.InvariantCulture.NumberFormat);

                        Lines[j] = "v " + vector3.X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + vector3.Y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + vector3.Z.ToString(CultureInfo.InvariantCulture.NumberFormat);
                    }
                }
                File.WriteAllLines(paths[i], Lines);
            }

            //Splines

            SplineJsonHandler splineJsonHandler = new SplineJsonHandler();
            splineJsonHandler = SplineJsonHandler.Load(TempPath + "\\Splines.json");

            for (int i = 0; i < splineJsonHandler.Splines.Count; i++)
            {
                var Spline = splineJsonHandler.Splines[i];

                for (int j = 0; j < Spline.Segments.Count; j++)
                {
                    var Segment = Spline.Segments[j];

                    for (int a = 0; a < 4; a++)
                    {
                        //Segment.Points[a, 0] = Segment.Points[a, 0] * -1f;
                        Segment.Points[a, 1] = Segment.Points[a, 1] * -1f;
                        //Segment.Points[a, 2] = Segment.Points[a, 2] * Scale;
                    }

                    Spline.Segments[j] = Segment;
                }

                splineJsonHandler.Splines[i] = Spline;
            }

            splineJsonHandler.CreateJson(TempPath + "\\Splines.json");

            //Lights

            LightJsonHandler lightJsonHandler = new LightJsonHandler();
            lightJsonHandler = LightJsonHandler.Load(TempPath + "\\lights.json");

            for (int i = 0; i < lightJsonHandler.Lights.Count; i++)
            {
                var Light = lightJsonHandler.Lights[i];

                //Light.Postion[0] = Light.Postion[0] * -1f;
                Light.Postion[1] = Light.Postion[1] * -1f;
                //Light.Postion[2] = Light.Postion[2] * Scale;

                lightJsonHandler.Lights[i] = Light;
            }

            lightJsonHandler.CreateJson(TempPath + "\\lights.json");

            //Particles

            ParticleInstanceJsonHandler particleInstanceJsonHandler = new ParticleInstanceJsonHandler();
            particleInstanceJsonHandler = ParticleInstanceJsonHandler.Load(TempPath + "\\Particles.json");

            for (int i = 0; i < particleInstanceJsonHandler.Particles.Count; i++)
            {
                var Particle = particleInstanceJsonHandler.Particles[i];

                //Particle.Location[0] = Particle.Location[0] * -1f;
                Particle.Location[1] = Particle.Location[1] * -1f;
                //Particle.Location[2] = Particle.Location[2] * Scale;

                particleInstanceJsonHandler.Particles[i] = Particle;
            }

            particleInstanceJsonHandler.CreateJson(TempPath + "\\Particles.json");

            //Collision

            //Read through each line of models and scale V points
            paths = Directory.GetFiles(TempPath + "\\Collision\\", "*.obj");
            for (int i = 0; i < paths.Length; i++)
            {
                string[] Lines = File.ReadAllLines(paths[i]);

                for (global::System.Int32 j = 0; j < Lines.Length; j++)
                {
                    string[] splitLine = Lines[j].Split(' ');
                    if (Lines[j].StartsWith("v "))
                    {
                        Vector3 vector3 = new Vector3();
                        vector3.X = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                        vector3.Y = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat) * -1f;
                        vector3.Z = float.Parse(splitLine[3], CultureInfo.InvariantCulture.NumberFormat);

                        Lines[j] = "v " + vector3.X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + vector3.Y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + vector3.Z.ToString(CultureInfo.InvariantCulture.NumberFormat);
                    }
                }
                File.WriteAllLines(paths[i], Lines);
            }


            //Path General AI
            AIPSOPJsonHandler aIPSOPJsonHandler = AIPSOPJsonHandler.Load(TempPath + "//AIP.json");

            //Convert points from array to vectors
            //Scale
            //Convert back
            for (int i = 0; i < aIPSOPJsonHandler.AIPaths.Count; i++)
            {
                //aIPSOPJsonHandler.AIPaths[i].PathPos[0] = aIPSOPJsonHandler.AIPaths[i].PathPos[0] * -1f;
                aIPSOPJsonHandler.AIPaths[i].PathPos[1] = aIPSOPJsonHandler.AIPaths[i].PathPos[1] * -1f;
                //aIPSOPJsonHandler.AIPaths[i].PathPos[2] = aIPSOPJsonHandler.AIPaths[i].PathPos[2];

                for (global::System.Int32 j = 0; j < aIPSOPJsonHandler.AIPaths[i].PathPoints.GetLength(0); j++)
                {
                    //aIPSOPJsonHandler.AIPaths[i].PathPoints[j, 0] = aIPSOPJsonHandler.AIPaths[i].PathPoints[j, 0] * -1f;
                    aIPSOPJsonHandler.AIPaths[i].PathPoints[j, 1] = aIPSOPJsonHandler.AIPaths[i].PathPoints[j, 1] * -1f;
                    //aIPSOPJsonHandler.AIPaths[i].PathPoints[j, 2] = aIPSOPJsonHandler.AIPaths[i].PathPoints[j, 2];
                }
            }

            //Convert points from array to vectors
            //Scale
            //Convert back

            //Path General Raceline
            for (int i = 0; i < aIPSOPJsonHandler.RaceLines.Count; i++)
            {
                var RaceLine = aIPSOPJsonHandler.RaceLines[i];

                //RaceLine.PathPos[0] = RaceLine.PathPos[0] * -1f;
                RaceLine.PathPos[1] = RaceLine.PathPos[1] * -1f;
                //RaceLine.PathPos[2] = RaceLine.PathPos[2] * Scale;

                for (global::System.Int32 j = 0; j < RaceLine.PathPoints.GetLength(0); j++)
                {
                    //RaceLine.PathPoints[j, 0] = RaceLine.PathPoints[j, 0] * -1f;
                    RaceLine.PathPoints[j, 1] = RaceLine.PathPoints[j, 1] * -1f;
                    //RaceLine.PathPoints[j, 2] = RaceLine.PathPoints[j, 2] * Scale;
                }

                aIPSOPJsonHandler.RaceLines[i] = RaceLine;
            }

            aIPSOPJsonHandler.CreateJson(TempPath + "//AIP.json");

            //SOP General AI
            aIPSOPJsonHandler = AIPSOPJsonHandler.Load(TempPath + "//SOP.json");

            //Convert points from array to vectors
            //Scale
            //Convert back
            for (int i = 0; i < aIPSOPJsonHandler.AIPaths.Count; i++)
            {
                //aIPSOPJsonHandler.AIPaths[i].PathPos[0] = aIPSOPJsonHandler.AIPaths[i].PathPos[0] * -1f;
                aIPSOPJsonHandler.AIPaths[i].PathPos[1] = aIPSOPJsonHandler.AIPaths[i].PathPos[1] * -1f;
                //aIPSOPJsonHandler.AIPaths[i].PathPos[2] = aIPSOPJsonHandler.AIPaths[i].PathPos[2] * Scale;

                for (global::System.Int32 j = 0; j < aIPSOPJsonHandler.AIPaths[i].PathPoints.GetLength(0); j++)
                {
                    //aIPSOPJsonHandler.AIPaths[i].PathPoints[j, 0] = aIPSOPJsonHandler.AIPaths[i].PathPoints[j, 0] * -1f;
                    aIPSOPJsonHandler.AIPaths[i].PathPoints[j, 1] = aIPSOPJsonHandler.AIPaths[i].PathPoints[j, 1] * -1f;
                    //aIPSOPJsonHandler.AIPaths[i].PathPoints[j, 2] = aIPSOPJsonHandler.AIPaths[i].PathPoints[j, 2] * Scale;
                }
            }

            //Convert points from array to vectors
            //Scale
            //Convert back

            //SOP General Raceline
            for (int i = 0; i < aIPSOPJsonHandler.RaceLines.Count; i++)
            {
                var RaceLine = aIPSOPJsonHandler.RaceLines[i];

                //RaceLine.PathPos[0] = RaceLine.PathPos[0] * -1f;
                RaceLine.PathPos[1] = RaceLine.PathPos[1] * -1f;
                //RaceLine.PathPos[2] = RaceLine.PathPos[2] * Scale;

                for (global::System.Int32 j = 0; j < RaceLine.PathPoints.GetLength(0); j++)
                {
                    //RaceLine.PathPoints[j, 0] = RaceLine.PathPoints[j, 0] * -1f;
                    RaceLine.PathPoints[j, 1] = RaceLine.PathPoints[j, 1] * -1f;
                    //RaceLine.PathPoints[j, 2] = RaceLine.PathPoints[j, 2] * Scale;
                }

                aIPSOPJsonHandler.RaceLines[i] = RaceLine;
            }

            aIPSOPJsonHandler.CreateJson(TempPath + "//SOP.json");

            Console.WriteLine("Begining Level Rebuild...");

            trickyLevelInterface = new TrickyLevelInterface();

            trickyLevelInterface.BuildTrickyLevelFiles(TempPath, BuildPath + ".map");

            Directory.Delete(TempPath, true);

            ConsoleWindow.CloseConsole();
        }
    }
}
