using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Color = System.Drawing.Color;
using Color2 = System.Windows.Media.Color;
namespace SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2
{
    public class LightmapGenerator
    {
        public static SSHHandler GenerateNewLightmap(PBDHandler handler, SSHHandler textures)
        {
            //Reset LightingData
            handler = GenerateNewLightmapPoints(handler);
            SSHHandler LightmapHandler = GenerateUnlitLightmap(handler);

            for (int i = 0; i < handler.Patches.Count; i++)
            {
                PatchObject patchObject = new PatchObject();

                //Go generate per patch data
                patchObject.patchData = handler.Patches[i];
                patchObject.patchTexture = new Bitmap(textures.sshImages[handler.Patches[i].TextureAssigment].bitmap, new Size(8, 8));
                patchObject.lightingImage = new Bitmap(8, 8, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                patchObject.trickyCorreted = new Bitmap(8, 8, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                for (int a = 0; a < handler.lights.Count; a++)
                {
                    // light type (0 = Directional (Sun), 1 = Spot/Area, 2 = Point, 3 = Ambient)
                    if (handler.lights[a].Type==3)
                    {
                        for (int x= 0; x < patchObject.lightingImage.Width; x++)
                        {
                            for (int y = 0; y < patchObject.lightingImage.Height; y++)
                            {
                                patchObject.lightingImage = GreaterThanPixel(patchObject.lightingImage, x, y, handler.lights[a]);
                            }
                        }
                    }
                }

                //Convert to ssx tricky lightmap format
                for (int x = 0; x < patchObject.lightingImage.Width; x++)
                {
                    for (int y = 0; y < patchObject.lightingImage.Height; y++)
                    {
                        Color OldColor1 = patchObject.patchTexture.GetPixel(x, y);
                        Color LightingColor1 = patchObject.lightingImage.GetPixel(x, y);
                        Color2 OldColor = Color2.FromRgb(OldColor1.R, OldColor1.G, OldColor1.B);
                        Color2 LightingColor = Color2.FromRgb(LightingColor1.R, LightingColor1.G, LightingColor1.B);

                        Color2 NewColor = Color2.FromScRgb(1, OldColor.ScR * LightingColor.ScR, OldColor.ScG * LightingColor.ScG, OldColor.ScB * LightingColor.ScB);
                        float[] colors = new float[3] { LightingColor.ScR, LightingColor.ScG, LightingColor.ScB };
                        float Alpha = colors.Max();

                        NewColor = Color2.FromScRgb(Alpha, OldColor.ScR - (NewColor.ScR / Alpha), OldColor.ScG - (NewColor.ScG / Alpha), OldColor.ScB - (NewColor.ScB / Alpha));
                        patchObject.trickyCorreted.SetPixel(x, y, Color.FromArgb(NewColor.A, NewColor.R, NewColor.G, NewColor.B));
                    }
                }

                //Combine data to ssh
                var TempLightmap = LightmapHandler.sshImages[patchObject.patchData.LightmapID];

                int PosX = (int)(patchObject.patchData.LightMapPoint.X * TempLightmap.bitmap.Width);
                int PosY = (int)(patchObject.patchData.LightMapPoint.Y * TempLightmap.bitmap.Height);

                int X1 = 0;
                int Y1 = 0;
                for (int x = PosX; x < PosX + patchObject.trickyCorreted.Width; x++)
                {
                    for (int y = PosY; y < PosX + patchObject.trickyCorreted.Height; y++)
                    {
                        TempLightmap.bitmap.SetPixel(x, y, patchObject.trickyCorreted.GetPixel(X1, Y1));
                        Y1++;
                    }
                    X1++;
                }

                LightmapHandler.sshImages[patchObject.patchData.LightmapID] = TempLightmap;
            }

            //Return Data
            return GenerateUnlitLightmap(handler);
        }


        public static Bitmap GreaterThanPixel(Bitmap bitmap , int X, int Y, Light light)
        {
            Color OldColor = bitmap.GetPixel(X, Y);
            Color2 NewColor = Color2.FromScRgb(1f,light.Colour.X, light.Colour.Y, light.Colour.Z);

            if(OldColor.R < NewColor.R)
            {
                OldColor = Color.FromArgb(255, NewColor.R, OldColor.G, OldColor.B); 
            }
            if (OldColor.G < NewColor.G)
            {
                OldColor = Color.FromArgb(255, OldColor.R, NewColor.G, OldColor.B);
            }
            if (OldColor.B < NewColor.B)
            {
                OldColor = Color.FromArgb(255, OldColor.R, OldColor.G, NewColor.B);
            }

            bitmap.SetPixel(X, Y, OldColor);
            return bitmap;
        }

        public static PBDHandler GenerateNewLightmapPoints(PBDHandler handler, int LightmapPatchRes = 8)
        {
            int GridSize = 128 / LightmapPatchRes;
            int X = 0;
            int Y = 0;
            int LightmapID = 0;
            for (int i = 0; i < handler.Patches.Count; i++)
            {
                var patch = handler.Patches[i];
                patch.LightMapPoint.X = (1 / GridSize) * X; //X position
                patch.LightMapPoint.Y = (1 / GridSize) * Y; //Y position
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
            return handler;
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
                LightmapHandler.AddImage(128);
                var temp = LightmapHandler.sshImages[i];
                temp.sshHeader.MatrixFormat = 5;
                temp.shortname = i.ToString().PadLeft(4, '0');
                for (int y = 0; y < temp.bitmap.Height; y++)
                {
                    for (int x = 0; x < temp.bitmap.Width; x++)
                    {
                        Color color = Color.FromArgb(255, 0, 0, 0);
                        temp.bitmap.SetPixel(x, y, color);
                    }
                }
                LightmapHandler.sshImages[i] = temp;
            }
            return LightmapHandler;
        }
    }


    struct PatchObject
    {
        public Patch patchData;

        public Bitmap lightingImage;
        public Bitmap patchTexture;

        public Bitmap trickyCorreted;
    }
}
