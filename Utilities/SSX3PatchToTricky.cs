using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SSXMultiTool.Utilities
{
    public class SSX3PatchToTricky
    {

        public static void CovertAllPatchFiles(string Input, string Output)
        {
            string[] AllPatches = Directory.GetFiles(Input, "*Patches.json", SearchOption.AllDirectories);

            var TrickyPatch = new SSXLibrary.JsonFiles.Tricky.PatchesJsonHandler();
            TrickyPatch.Patches = new List<SSXLibrary.JsonFiles.Tricky.PatchesJsonHandler.PatchJson>();
            for (int i = 0; i < AllPatches.Length; i++)
            {
                string[] Slash = AllPatches[i].Split("\\");

                string WorldFileName = Slash[11];

                var SSX3Patch = SSXLibrary.JsonFiles.SSX3.PatchesJsonHandler.Load(AllPatches[i]);

                for (int j = 0; j < SSX3Patch.Patches.Count; j++)
                {
                    var LocalTrickyPatch = new SSXLibrary.JsonFiles.Tricky.PatchesJsonHandler.PatchJson();

                    var LocalSSX3Patch = SSX3Patch.Patches[j];

                    LocalTrickyPatch.PatchName = LocalSSX3Patch.Name;
                    LocalTrickyPatch.LightMapPoint = LocalSSX3Patch.LightMapPoint;
                    LocalTrickyPatch.UVPoints = LocalSSX3Patch.UVPoints;
                    LocalTrickyPatch.Points = LocalSSX3Patch.Points;

                    LocalTrickyPatch.SurfaceType = 1;
                    LocalTrickyPatch.TrickOnlyPatch = false;
                    LocalTrickyPatch.TexturePath = "0000.png";
                    LocalTrickyPatch.LightmapID = 0;

                    TrickyPatch.Patches.Add(LocalTrickyPatch);
                }
            }

            TrickyPatch.CreateJson(Output + "\\" + "Patches.json");
        }

    }
}
