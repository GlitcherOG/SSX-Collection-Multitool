using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2
{
    public class LightmapGenerator
    {
        public void GenerateNewLightmap()
        {

        }

        public void GenerateNewLightmapPoints(PBDHandler handler, int LightmapPatchRes = 8)
        {
            int GridSize = 128 / LightmapPatchRes;
            int X = 0;
            int Y = 0;
            int LightmapID = 0;
            for (int i = 0; i < handler.Patches.Count; i++)
            {
                var patch = handler.Patches[i];
                patch.LightMapPoint.X = 1 / GridSize * X; //X position
                patch.LightMapPoint.Y = 1 / GridSize * Y; //Y position
                patch.LightMapPoint.Z = 1 / GridSize; //Width
                patch.LightMapPoint.W = 1 / GridSize; //Hight
                patch.LightmapID = LightmapID;
                X++;
                if(X== GridSize)
                {
                    Y++;
                    X = 0;
                }
                if(Y==GridSize)
                {
                    Y = 0;
                    LightmapID++;
                }
                handler.Patches[i] = patch;
            }
        }

        public static SSHHandler GenerateUnlitLightmap(PBDHandler pbdHandler)
        {
            SSHHandler LightmapHandler = new SSHHandler();
            LightmapHandler.format = "G278";
            int maxLightmap = 0;
            for (int i = 0; i < pbdHandler.Patches.Count; i++)
            {
                if (maxLightmap < pbdHandler.Patches[i].LightmapID)
                {
                    maxLightmap = pbdHandler.Patches[i].LightmapID;
                }
            }

            for (int i = 0; i < maxLightmap+1; i++)
            {
                LightmapHandler.AddImage();
                var temp = LightmapHandler.sshImages[i];
                temp.bitmap = new Bitmap(128, 128, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                temp.sshHeader.MatrixFormat = 5;
                temp.shortname = i.ToString().PadLeft(4, '0');
                for (int y = 0; y < temp.bitmap.Height; y++)
                {
                    for (int x = 0; x < temp.bitmap.Width; x++)
                    {
                        Color color = Color.FromArgb(1, 0, 0, 0);
                        temp.bitmap.SetPixel(x, y, color);
                    }
                }
                LightmapHandler.sshImages[i] = temp;
            }
            return LightmapHandler;
        }
    }
}
