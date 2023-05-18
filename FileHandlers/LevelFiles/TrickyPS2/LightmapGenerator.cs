using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Numerics;
using Color = System.Drawing.Color;
using Color2 = System.Windows.Media.Color;
using SSXMultiTool.FileHandlers.Textures;
using SharpGLTF.Schema2;

namespace SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2
{
    public class LightmapGenerator
    {
        public static OldSSHHandler GenerateNewLightmap(PBDHandler handler, OldSSHHandler textures)
        {
            //Reset LightingData
            handler = GenerateNewLightmapPoints(handler);
            OldSSHHandler LightmapHandler = GenerateUnlitLightmap(handler);

            for (int i = 0; i < handler.Patches.Count; i++)
            {
                PatchObject patchObject = new PatchObject();

                //Go generate per patch data
                patchObject.patchData = handler.Patches[i];
                patchObject.lightingImage = new Bitmap(8, 8, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                patchObject.PS2Corrected = new Bitmap(8, 8, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                var TempTexture = textures.sshImages[handler.Patches[i].TextureAssigment].bitmap;

                //Build UV Points
                NURBS.Surface surface;
                NURBS.ControlPoint[,] cps = new NURBS.ControlPoint[2, 2];

                int degreeU = 3;
                int degreeV = 3;

                int resolutionU = TempTexture.Width;
                int resolutionV = TempTexture.Height;

                surface = new NURBS.Surface(cps, degreeU, degreeV);

                cps[0, 0] = new NURBS.ControlPoint(patchObject.patchData.UVPoint1.X, -patchObject.patchData.UVPoint1.Y, 0, 1);
                cps[0, 1] = new NURBS.ControlPoint(patchObject.patchData.UVPoint2.X, -patchObject.patchData.UVPoint2.Y, 0, 1);
                cps[1, 0] = new NURBS.ControlPoint(patchObject.patchData.UVPoint3.X, -patchObject.patchData.UVPoint3.Y, 0, 1);
                cps[1, 1] = new NURBS.ControlPoint(patchObject.patchData.UVPoint4.X, -patchObject.patchData.UVPoint4.Y, 0, 1);

                surface = new NURBS.Surface(cps, 1, 1);

                Vector3[] UV = surface.ReturnVertices(resolutionU-1, resolutionV-1);
                Vector2[] NewUVPoints = new Vector2[UV.Length];

                for (int a = 0; a < UV.Length; a++)
                {
                    NewUVPoints[a] = new Vector2(UV[a].X, UV[a].Y);
                }

                //Convert to 2D Array
                Vector2[,] UVPoints = new Vector2[resolutionU, resolutionV];
                int f = resolutionV;
                for (int a = 0; a < resolutionU; a++)
                {
                    for (int b = 0; b < resolutionV; b++)
                    {
                        int h = a * f + b;
                        //int e = a * f + b + 1; //SharePoint 2
                        //int j = (a + 1) * f + b + 1;
                        //int k = (a + 1) * f + b; //Share Point 1

                        //E, H
                        //J, K

                        UVPoints[a, b] = NewUVPoints[h];
                        //UVPoints[a+1, b] = NewUVPoints[e];
                        //UVPoints[a, b+1] = NewUVPoints[k];
                        //UVPoints[a+1, b+1] = NewUVPoints[j];

                    }
                }

                //Set Texture with UV
                Bitmap bitTexture = new Bitmap(TempTexture.Width, TempTexture.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                for (int y = 0; y < bitTexture.Height; y++)
                {
                    for (int x = 0; x < bitTexture.Width; x++)
                    {
                        int NewXPos = (int)(UVPoints[y, x].X * TempTexture.Width);
                        int NewYPos = (int)(UVPoints[y, x].Y * TempTexture.Height);

                        if (NewXPos < 0)
                        {
                            NewXPos = TempTexture.Width - 1 - (NewXPos % TempTexture.Width);
                        }
                        else
                        {
                            NewXPos = (NewXPos % TempTexture.Width);
                        }

                        if(NewYPos < 0)
                        {
                            NewYPos = TempTexture.Height - 1 - (NewYPos % TempTexture.Height);
                        }
                        else
                        {
                            NewYPos = (NewYPos % TempTexture.Height);
                        }

                        Color NewColor = TempTexture.GetPixel(NewXPos, NewYPos);
                        bitTexture.SetPixel(x, y, NewColor);
                    }
                }

                patchObject.patchTexture = new Bitmap(bitTexture, new Size(8, 8));

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
                        patchObject.PS2Corrected.SetPixel(x, y, Color.FromArgb(NewColor.A, NewColor.R, NewColor.G, NewColor.B));
                    }
                }

                //Combine data to ssh
                var TempLightmap = LightmapHandler.sshImages[patchObject.patchData.LightmapID];

                int PosX = (int)(patchObject.patchData.LightMapPoint.X * TempLightmap.bitmap.Width);
                int PosY = (int)(patchObject.patchData.LightMapPoint.Y * TempLightmap.bitmap.Height);

                int X1 = 0;
                int Y1 = 0;
                for (int x = PosX; x < PosX + patchObject.PS2Corrected.Width; x++)
                {
                    for (int y = PosY; y < PosX + patchObject.PS2Corrected.Height; y++)
                    {
                        TempLightmap.bitmap.SetPixel(x, y, patchObject.PS2Corrected.GetPixel(X1, Y1));
                        Y1++;
                    }
                    X1++;
                }

                LightmapHandler.sshImages[patchObject.patchData.LightmapID] = TempLightmap;
            }

            //Return Data
            return LightmapHandler;
        }

        public static OldSSHHandler ConvertToStandardLightmap(PBDHandler handler, OldSSHHandler textures, OldSSHHandler LightmapHandler)
        {
            for (int i = 0; i < handler.Patches.Count; i++)
            {
                PatchObject patchObject = new PatchObject();

                //Go generate per patch data
                patchObject.patchData = handler.Patches[i];

                var TempTexture = textures.sshImages[handler.Patches[i].TextureAssigment].bitmap;

                //Build UV Points
                NURBS.Surface surface;
                NURBS.ControlPoint[,] cps = new NURBS.ControlPoint[2, 2];

                int degreeU = 3;
                int degreeV = 3;

                int resolutionU = TempTexture.Width;
                int resolutionV = TempTexture.Height;

                surface = new NURBS.Surface(cps, degreeU, degreeV);

                cps[0, 0] = new NURBS.ControlPoint(patchObject.patchData.UVPoint1.X, -patchObject.patchData.UVPoint1.Y, 0, 1);
                cps[0, 1] = new NURBS.ControlPoint(patchObject.patchData.UVPoint2.X, -patchObject.patchData.UVPoint2.Y, 0, 1);
                cps[1, 0] = new NURBS.ControlPoint(patchObject.patchData.UVPoint3.X, -patchObject.patchData.UVPoint3.Y, 0, 1);
                cps[1, 1] = new NURBS.ControlPoint(patchObject.patchData.UVPoint4.X, -patchObject.patchData.UVPoint4.Y, 0, 1);

                surface = new NURBS.Surface(cps, 1, 1);

                Vector3[] UV = surface.ReturnVertices(resolutionU - 1, resolutionV - 1);
                Vector2[] NewUVPoints = new Vector2[UV.Length];

                for (int a = 0; a < UV.Length; a++)
                {
                    NewUVPoints[a] = new Vector2(UV[a].X, UV[a].Y);
                }

                //Convert to 2D Array
                Vector2[,] UVPoints = new Vector2[resolutionU, resolutionV];
                int f = resolutionV;
                for (int a = 0; a < resolutionU; a++)
                {
                    for (int b = 0; b < resolutionV; b++)
                    {
                        int h = a * f + b;
                        //int e = a * f + b + 1; //SharePoint 2
                        //int j = (a + 1) * f + b + 1;
                        //int k = (a + 1) * f + b; //Share Point 1

                        //E, H
                        //J, K

                        UVPoints[a, b] = NewUVPoints[h];
                        //UVPoints[a+1, b] = NewUVPoints[e];
                        //UVPoints[a, b+1] = NewUVPoints[k];
                        //UVPoints[a+1, b+1] = NewUVPoints[j];

                    }
                }

                //Set Texture with UV
                Bitmap bitTexture = new Bitmap(TempTexture.Width, TempTexture.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                for (int y = 0; y < bitTexture.Height; y++)
                {
                    for (int x = 0; x < bitTexture.Width; x++)
                    {
                        int NewXPos = (int)(UVPoints[y, x].X * TempTexture.Width);
                        int NewYPos = (int)(UVPoints[y, x].Y * TempTexture.Height);

                        if (NewXPos < 0)
                        {
                            NewXPos = TempTexture.Width - 1 - (NewXPos % TempTexture.Width);
                        }
                        else
                        {
                            NewXPos = (NewXPos % TempTexture.Width);
                        }

                        if (NewYPos < 0)
                        {
                            NewYPos = TempTexture.Height - 1 - (NewYPos % TempTexture.Height);
                        }
                        else
                        {
                            NewYPos = (NewYPos % TempTexture.Height);
                        }

                        Color NewColor = TempTexture.GetPixel(NewXPos, NewYPos);
                        bitTexture.SetPixel(x, y, NewColor);
                    }
                }

                patchObject.patchTexture = new Bitmap(bitTexture, new Size(8, 8));

                //Load Tricky Lightmap + Texture and convert to standard

                int XCord = (int)(patchObject.patchData.LightMapPoint.X * LightmapHandler.sshImages[patchObject.patchData.LightmapID].bitmap.Width);
                int YCord = (int)(patchObject.patchData.LightMapPoint.Y * LightmapHandler.sshImages[patchObject.patchData.LightmapID].bitmap.Height);
                int Width = (int)(patchObject.patchData.LightMapPoint.Z * LightmapHandler.sshImages[patchObject.patchData.LightmapID].bitmap.Width);
                int Height = (int)(patchObject.patchData.LightMapPoint.W * LightmapHandler.sshImages[patchObject.patchData.LightmapID].bitmap.Height);
                patchObject.PS2Corrected = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                patchObject.lightingImage = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        var Colour = LightmapHandler.sshImages[patchObject.patchData.LightmapID].bitmap.GetPixel(XCord + x, YCord + y);
                        patchObject.PS2Corrected.SetPixel(x, y, Colour);
                    }
                }


                for (int x = 0; x < patchObject.PS2Corrected.Width; x++)
                {
                    for (int y = 0; y < patchObject.PS2Corrected.Height; y++)
                    {
                        Color OldColor1 = patchObject.patchTexture.GetPixel(x, y);
                        Color LightingColor1 = patchObject.PS2Corrected.GetPixel(x, y);
                        Color2 OldColor = Color2.FromRgb(OldColor1.R, OldColor1.G, OldColor1.B);
                        Color2 LightingColor = Color2.FromArgb(LightingColor1.A, LightingColor1.R, LightingColor1.G, LightingColor1.B);

                        Color2 MixedColour = Color2.FromScRgb(1,(OldColor.ScR-LightingColor.ScR) * LightingColor.ScA, (OldColor.ScG - LightingColor.ScG) * LightingColor.ScA, (OldColor.ScB - LightingColor.ScB) * LightingColor.ScA);

                        Color2 NewColor = Color2.FromScRgb(1, MixedColour.ScR / OldColor.ScR, MixedColour.ScG / OldColor.ScG, MixedColour.ScB / OldColor.ScB);

                        patchObject.lightingImage.SetPixel(x, y, Color.FromArgb(NewColor.A, NewColor.R, NewColor.G, NewColor.B));
                    }
                }


                //Combine data to ssh
                var TempLightmap = LightmapHandler.sshImages[patchObject.patchData.LightmapID];

                int PosX = (int)(patchObject.patchData.LightMapPoint.X * TempLightmap.bitmap.Width);
                int PosY = (int)(patchObject.patchData.LightMapPoint.Y * TempLightmap.bitmap.Height);

                int X1 = 0;
                for (int x = PosX; x < PosX + patchObject.lightingImage.Width; x++)
                {
                    int Y1 = 0;
                    for (int y = PosY; y < PosY + patchObject.lightingImage.Height; y++)
                    {
                        TempLightmap.bitmap.SetPixel(x, y, patchObject.lightingImage.GetPixel(X1, Y1));
                        Y1++;
                    }
                    X1++;
                }

                LightmapHandler.sshImages[patchObject.patchData.LightmapID] = TempLightmap;
                patchObject = new PatchObject();
                GC.Collect();
            }

            return LightmapHandler;
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

        public static OldSSHHandler GenerateUnlitLightmap(PBDHandler pbdHandler)
        {
            OldSSHHandler LightmapHandler = new OldSSHHandler();
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

        public Bitmap PS2Corrected;
    }
}
