using SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2;
using SSXMultiTool.JsonFiles.Tricky;
using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static SSXMultiTool.FileHandlers.LevelFiles.OGPS2.WDFHandler;

namespace SSXMultiTool.FileHandlers.LevelFiles.Converters
{
    public class TrickyScaler
    {
        public void Scaler(string LoadPath, float Scale)
        {
            Console.WriteLine("Loading PBD File");
            //Load PBD

            //Patches - Done
            //Instances - Done
            //Models/Prefabs - Done
            //Splines
            //Lights
            //Particles
            //Collision
            //LTG - Done
            //Path General AI
            //Path General Raceline
            //Path Showoff AI
            //Path Showoff Raceline


            PBDHandler pbdHandler = new PBDHandler();
            pbdHandler.LoadPBD(LoadPath + ".pbd");

            //Patches
            for (int i = 0; i < pbdHandler.Patches.Count; i++)
            {
                var Patch = pbdHandler.Patches[i];

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

                for (int a = 0; a < bezierUtil.RawPoints.Length; a++)
                {
                    bezierUtil.RawPoints[a] = bezierUtil.RawPoints[a] * Scale;
                }

                Patch.R1C1 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[0]);
                Patch.R1C2 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[1]);
                Patch.R1C3 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[2]);
                Patch.R1C4 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[3]);
                Patch.R2C1 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[4]);
                Patch.R2C2 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[5]);
                Patch.R2C3 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[6]);
                Patch.R2C4 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[7]);
                Patch.R3C1 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[8]);
                Patch.R3C2 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[9]);
                Patch.R3C3 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[10]);
                Patch.R3C4 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[11]);
                Patch.R4C1 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[12]);
                Patch.R4C2 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[13]);
                Patch.R4C3 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[14]);
                Patch.R4C4 = JsonUtil.Vector3ToVector4(bezierUtil.ProcessedPoints[15]);

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

                Patch.HighestXYZ = HighestXYZ;
                Patch.LowestXYZ = LowestXYZ;

                Patch.Point1 = JsonUtil.Vector3ToVector4(bezierUtil.RawPoints[0]);
                Patch.Point2 = JsonUtil.Vector3ToVector4(bezierUtil.RawPoints[12]);
                Patch.Point3 = JsonUtil.Vector3ToVector4(bezierUtil.RawPoints[3]);
                Patch.Point4 = JsonUtil.Vector3ToVector4(bezierUtil.RawPoints[15]);

                pbdHandler.Patches[i] = Patch;
            }

            //Instances
            for (int i = 0; i < pbdHandler.Instances.Count; i++)
            {
                var Instance = pbdHandler.Instances[i];

                Vector3 InstanceScale;
                Quaternion Rotation;
                Vector3 Location;

                Matrix4x4.Decompose(pbdHandler.Instances[i].matrix4X4, out InstanceScale, out Rotation, out Location);

                Matrix4x4 ScaleMatrix = Matrix4x4.CreateScale(InstanceScale);
                Matrix4x4 RotationMatrix = Matrix4x4.CreateFromQuaternion(Rotation);
                Matrix4x4 matrix4X4 = Matrix4x4.Multiply(ScaleMatrix, RotationMatrix);
                matrix4X4.Translation = Location*Scale;

                Instance.matrix4X4 = matrix4X4;

                pbdHandler.Instances[i] = Instance;
            }

            //Prefabs
            var Prefabs = pbdHandler.PrefabData;
            for (int a = 0; a < Prefabs.Count; a++)
            {
                var Prefab= Prefabs[a];
                for (int ax = 0; ax < Prefab.PrefabObjects.Count; ax++)
                {
                    var PrefabObject = Prefab.PrefabObjects[ax];
                    if (PrefabObject.objectData.MeshOffsets != null)
                    {
                        for (int i = 0; i < PrefabObject.objectData.MeshOffsets.Count; i++)
                        {
                            var MeshOffset = PrefabObject.objectData.MeshOffsets[i];

                            for (int b = 0; b < MeshOffset.FullMesh.meshChunk.Count; b++)
                            {
                                var Chunk = MeshOffset.FullMesh.meshChunk[b];

                                for (int j = 0; j < Chunk.vertices.Count; j++)
                                {
                                    Chunk.vertices[j] = Chunk.vertices[j] * Scale;
                                }

                                MeshOffset.FullMesh.meshChunk[b] = Chunk;
                            }

                            PrefabObject.objectData.MeshOffsets[i] = MeshOffset;
                        }
                    }

                    if(PrefabObject.IncludeMatrix)
                    {
                        var Matrix = PrefabObject.matrix4X4;

                        Vector3 InstanceScale;
                        Quaternion Rotation;
                        Vector3 Location;

                        Matrix4x4.Decompose(Matrix, out InstanceScale, out Rotation, out Location);

                        Matrix4x4 ScaleMatrix = Matrix4x4.CreateScale(InstanceScale);
                        Matrix4x4 RotationMatrix = Matrix4x4.CreateFromQuaternion(Rotation);
                        Matrix4x4 matrix4X4 = Matrix4x4.Multiply(ScaleMatrix, RotationMatrix);
                        matrix4X4.Translation = Location * Scale;

                        PrefabObject.matrix4X4 = matrix4X4;
                    }

                    Prefab.PrefabObjects[ax] = PrefabObject;
                }

                Prefabs[a] = Prefab;
            }
            pbdHandler.PrefabData = Prefabs;

            //Splines
            for (int i = 0; i < pbdHandler.splines.Count; i++)
            {
                var Spline = pbdHandler.splines[i];
                Vector3 HighestXYZSpline = JsonUtil.Vector4ToVector3(pbdHandler.splinesSegments[Spline.SplineSegmentPosition].ControlPoint);
                Vector3 LowestXYZSpline = JsonUtil.Vector4ToVector3(pbdHandler.splinesSegments[Spline.SplineSegmentPosition].ControlPoint);
                for (int j = Spline.SplineSegmentPosition; j < Spline.SplineSegmentPosition + Spline.SplineSegmentCount; j++)
                {
                    var Segment = pbdHandler.splinesSegments[j];




                    pbdHandler.splinesSegments[j] = Segment;
                }


                pbdHandler.splines[i] = Spline;
            }


            //Lights
            //Particles
            //Collision

            pbdHandler.RegenerateLowestAndHighest();

            LTGHandler ltgHandler = new LTGHandler();
            Console.WriteLine("Generating LTG File");

            ltgHandler.RegenerateOriginLTG(pbdHandler);

            Console.WriteLine("Saving LTG File");
            ltgHandler.SaveLTGFile(LoadPath + ".ltg");

            pbdHandler.SaveNew(LoadPath + ".pbd");
        }

        public void ProjectScaler(string LoadPath, float Scale)
        {
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
                    for (global::System.Int32 k = 0; k < 3; k++)
                    {
                        Patch.Points[j, k] = Patch.Points[j, k] * Scale;
                    }
                }

                patchesJsonHandler.Patches[i] = Patch;
            }

            patchesJsonHandler.CreateJson(TempPath + "\\Patches.json");
            //Instances

            //Convert points from array to vectors
            //Scale
            //Convert back

            //Models/Prefabs

            //Convert points from array to vectors
            //Scale
            //Convert back
            //Read through each line of models and scale V points

            //Splines

            //Convert points from array to vectors
            //Scale
            //Convert back

            //Lights

            //Convert points from array to vectors
            //Scale
            //Convert back

            //Particles

            //Convert points from array to vectors
            //Scale
            //Convert back

            //Collision

            //Read through each line of models and scale V points

            //Path General AI

            //Convert points from array to vectors
            //Scale
            //Convert back

            //Path General Raceline

            //Convert points from array to vectors
            //Scale
            //Convert back

            //Path Showoff AI

            //Convert points from array to vectors
            //Scale
            //Convert back

            //Path Showoff Raceline

            //Convert points from array to vectors
            //Scale
            //Convert back

            Console.WriteLine("Begining Level Rebuild...");

            trickyLevelInterface = new TrickyLevelInterface();

            trickyLevelInterface.BuildTrickyLevelFiles(TempPath, LoadPath);

            ConsoleWindow.CloseConsole();
        }
    }
}
