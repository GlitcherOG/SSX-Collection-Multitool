using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
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
        public static Bitmap ReduceBitmapColorsSlow(Bitmap bmp, int MaxColors)
        {
            // Step 1: Extract pixel colors
            var pixelColors = new List<Color>();
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    pixelColors.Add(bmp.GetPixel(x, y));
                }
            }

            // Step 2: Reduce colors
            var reducedPalette = ReduceColors(pixelColors, MaxColors);

            // Step 3: Recolor bitmap using nearest palette color
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    var original = bmp.GetPixel(x, y);
                    var nearest = FindNearestColor(original, reducedPalette);
                    bmp.SetPixel(x, y, nearest);
                }
            }

            return bmp;
        }

        public static List<Color> ReduceColors(List<Color> inputColors, int MaxColors)
        {
            if (inputColors.Count <= MaxColors)
                return inputColors.Distinct().ToList();

            // Convert to vectors
            var vectors = inputColors.Select(c => new float[] { c.R, c.G, c.B }).ToList();

            // K-Means clustering
            var clusters = KMeans(vectors, MaxColors);

            // Convert cluster centroids back to Colors
            return clusters.Select(v => Color.FromArgb((int)v[0], (int)v[1], (int)v[2])).ToList();
        }

        private static List<float[]> KMeans(List<float[]> points, int k, int maxIterations = 100)
        {
            var rnd = new Random();
            var centroids = points.OrderBy(_ => rnd.Next()).Take(k).ToList();
            var assignments = new int[points.Count];

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                // Assign points to the nearest centroid
                for (int i = 0; i < points.Count; i++)
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
                }

                // Update centroids
                var newCentroids = new List<float[]>();
                for (int i = 0; i < k; i++)
                {
                    var clusterPoints = points
                        .Where((_, index) => assignments[index] == i)
                        .ToList();

                    if (clusterPoints.Count == 0)
                    {
                        newCentroids.Add(points[rnd.Next(points.Count)]);
                        continue;
                    }

                    float[] mean = new float[3];
                    foreach (var p in clusterPoints)
                    {
                        mean[0] += p[0];
                        mean[1] += p[1];
                        mean[2] += p[2];
                    }

                    mean[0] /= clusterPoints.Count;
                    mean[1] /= clusterPoints.Count;
                    mean[2] /= clusterPoints.Count;

                    newCentroids.Add(mean);
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
            Color nearest = palette[0];
            int minDistance = int.MaxValue;

            foreach (var c in palette)
            {
                int dr = color.R - c.R;
                int dg = color.G - c.G;
                int db = color.B - c.B;
                int dist = dr * dr + dg * dg + db * db;

                if (dist < minDistance)
                {
                    minDistance = dist;
                    nearest = c;
                }
            }

            return nearest;
        }
    }
}
