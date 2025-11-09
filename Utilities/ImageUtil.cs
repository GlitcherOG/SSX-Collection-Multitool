using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.Utilities
{
    public class ImageUtil
    {
        public static HashSet<Color> GetBitmapColorsFast(Bitmap bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;
            HashSet<Color> result = new HashSet<Color>();

            BitmapData data = bmp.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb); // Assumes 32bppArgb

            unsafe
            {
                byte* ptr = (byte*)data.Scan0;

                for (int y = 0; y < height; y++)
                {
                    byte* row = ptr + (y * data.Stride);
                    for (int x = 0; x < width; x++)
                    {
                        int index = y * width + x;
                        byte b = row[x * 4];
                        byte g = row[x * 4 + 1];
                        byte r = row[x * 4 + 2];
                        byte a = row[x * 4 + 3];

                        result.Add(Color.FromArgb(a, r, g, b));
                    }
                }
            }

            bmp.UnlockBits(data);
            return result;
        }
        public static Bitmap ReduceBitmapColorsFast(Bitmap bmp, int maxColors)
        {
            int width = bmp.Width;
            int height = bmp.Height;

            // Step 1: Lock bitmap for fast access
            var rect = new Rectangle(0, 0, width, height);
            var bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int bytesPerPixel = 3;
            int stride = bmpData.Stride;
            int byteCount = stride * height;
            byte[] pixels = new byte[byteCount];
            Marshal.Copy(bmpData.Scan0, pixels, 0, byteCount);

            // Step 2: Extract pixels (sample for speed)
            var pixelColors = new List<Color>(width * height / 4); // sample 25% pixels
            for (int y = 0; y < height; y += 2)
            {
                int row = y * stride;
                for (int x = 0; x < width * bytesPerPixel; x += 6)
                {
                    byte b = pixels[row + x + 0];
                    byte g = pixels[row + x + 1];
                    byte r = pixels[row + x + 2];
                    pixelColors.Add(Color.FromArgb(r, g, b));
                }
            }

            // Step 3: Compute reduced palette
            var reducedPalette = ReduceColors(pixelColors, maxColors);

            // Step 4: Recolor bitmap using nearest palette color (parallel)
            Parallel.For(0, height, y =>
            {
                int row = y * stride;
                for (int x = 0; x < width * bytesPerPixel; x += 3)
                {
                    byte b = pixels[row + x + 0];
                    byte g = pixels[row + x + 1];
                    byte r = pixels[row + x + 2];

                    var nearest = FindNearestColor(Color.FromArgb(r, g, b), reducedPalette);

                    pixels[row + x + 0] = nearest.B;
                    pixels[row + x + 1] = nearest.G;
                    pixels[row + x + 2] = nearest.R;
                }
            });

            Marshal.Copy(pixels, 0, bmpData.Scan0, byteCount);
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        // --- Helper methods below ---

        public static List<Color> ReduceColors(List<Color> inputColors, int maxColors)
        {
            if (inputColors.Count <= maxColors)
                return inputColors.Distinct().ToList();

            var vectors = inputColors.Select(c => new float[] { c.R, c.G, c.B }).ToList();
            var clusters = KMeans(vectors, maxColors);
            return clusters.Select(v => Color.FromArgb(
                (int)Math.Round(v[0]),
                (int)Math.Round(v[1]),
                (int)Math.Round(v[2])
            )).ToList();
        }

        private static List<float[]> KMeans(List<float[]> points, int k, int maxIterations = 20)
        {
            var rnd = new Random();
            var centroids = points.OrderBy(_ => rnd.Next()).Take(k).ToList();
            var assignments = new int[points.Count];

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                Parallel.For(0, points.Count, i =>
                {
                    float minDist = float.MaxValue;
                    int closest = 0;
                    for (int j = 0; j < k; j++)
                    {
                        float dist = Distance(points[i], centroids[j]);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            closest = j;
                        }
                    }
                    assignments[i] = closest;
                });

                var newCentroids = new List<float[]>(k);
                for (int j = 0; j < k; j++)
                    newCentroids.Add(new float[3]);

                int[] counts = new int[k];
                for (int i = 0; i < points.Count; i++)
                {
                    int cluster = assignments[i];
                    counts[cluster]++;
                    newCentroids[cluster][0] += points[i][0];
                    newCentroids[cluster][1] += points[i][1];
                    newCentroids[cluster][2] += points[i][2];
                }

                for (int j = 0; j < k; j++)
                {
                    if (counts[j] > 0)
                    {
                        newCentroids[j][0] /= counts[j];
                        newCentroids[j][1] /= counts[j];
                        newCentroids[j][2] /= counts[j];
                    }
                    else
                    {
                        newCentroids[j] = points[rnd.Next(points.Count)];
                    }
                }

                centroids = newCentroids;
            }

            return centroids;
        }

        private static float Distance(float[] a, float[] b)
        {
            float dr = a[0] - b[0];
            float dg = a[1] - b[1];
            float db = a[2] - b[2];
            return dr * dr + dg * dg + db * db;
        }

        private static Color FindNearestColor(Color color, List<Color> palette)
        {
            int minDist = int.MaxValue;
            Color nearest = palette[0];

            foreach (var p in palette)
            {
                int dr = color.R - p.R;
                int dg = color.G - p.G;
                int db = color.B - p.B;
                int dist = dr * dr + dg * dg + db * db;

                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = p;
                }
            }

            return nearest;
        }
    }
}
