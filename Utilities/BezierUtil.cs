using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SSXMultiTool.Utilities
{
    public class BezierUtil
    {
        public Vector3[] ProcessedPoints = new Vector3[16];
        public Vector3[] MidTablePoints = new Vector3[16];
        public Vector3[] RawPoints = new Vector3[16];

        #region Raw
        public void GenerateRawPoints()
        {
            RawPoints = new Vector3[16];
            MidTablePoints = new Vector3[16];

            MidTablePoints[0] = ProcessedPoints[0];
            MidTablePoints[1] = ProcessedToMidEq1(1, 0);
            MidTablePoints[2] = ProcessedToMidEq2(2, 1, 1);
            MidTablePoints[3] = ProcessedToMidEq3(3, 2, 1, 0);

            MidTablePoints[4] = ProcessedPoints[4];
            MidTablePoints[5] = ProcessedToMidEq1(5, 4);
            MidTablePoints[6] = ProcessedToMidEq2(6, 5, 5);
            MidTablePoints[7] = ProcessedToMidEq3(7, 6, 5, 4);

            MidTablePoints[8] = ProcessedPoints[8];
            MidTablePoints[9] = ProcessedToMidEq1(9, 8);
            MidTablePoints[10] = ProcessedToMidEq2(10, 9, 9);
            MidTablePoints[11] = ProcessedToMidEq3(11, 10, 9, 8);

            MidTablePoints[12] = ProcessedPoints[12];
            MidTablePoints[13] = ProcessedToMidEq1(13, 12);
            MidTablePoints[14] = ProcessedToMidEq2(14, 13, 13);
            MidTablePoints[15] = ProcessedToMidEq3(15, 14, 13, 12);

            RawPoints[0] = MidTablePoints[0];
            RawPoints[1] = MidTablePoints[1];
            RawPoints[2] = MidTablePoints[2];
            RawPoints[3] = MidTablePoints[3];

            RawPoints[4] = MidToRawEq1(4, 0);
            RawPoints[5] = MidToRawEq1(5, 1);
            RawPoints[6] = MidToRawEq1(6, 2);
            RawPoints[7] = MidToRawEq1(7, 3);

            RawPoints[8] = MidToRawEq2(8, 4,4);
            RawPoints[9] = MidToRawEq2(9, 5, 5);
            RawPoints[10] = MidToRawEq2(10, 6, 6);
            RawPoints[11] = MidToRawEq2(11, 7, 7);

            RawPoints[12] = MidToRawEq3(12, 8, 4, 0);
            RawPoints[13] = MidToRawEq3(13, 9, 5, 1);
            RawPoints[14] = MidToRawEq3(14, 10, 6, 2);
            RawPoints[15] = MidToRawEq3(15, 11, 7, 3);
        }

        Vector3 ProcessedToMidEq1(int A, int B)
        {
            return ProcessedPoints[A] / 3 + MidTablePoints[B];
        }

        Vector3 ProcessedToMidEq2(int A, int B, int midA)
        {
            return (ProcessedPoints[A] + ProcessedPoints[B]) / 3 + MidTablePoints[midA];
        }

        Vector3 ProcessedToMidEq3(int a, int midA, int midB, int midC)
        {
            return ProcessedPoints[a] + ProcessedPoints[midA] + ProcessedPoints[midB] + ProcessedPoints[midC];
        }

        Vector3 MidToRawEq1(int A, int B)
        {
            return MidTablePoints[A] / 3 + RawPoints[B];
        }

        Vector3 MidToRawEq2(int A, int B, int midA)
        {
            return (MidTablePoints[A] + MidTablePoints[B]) / 3 + RawPoints[midA];
        }

        Vector3 MidToRawEq3(int a, int midA, int midB, int midC)
        {
            return MidTablePoints[a] + MidTablePoints[midA] + MidTablePoints[midB] + MidTablePoints[midC];
        }

        #endregion

        #region Processing
        public void GenerateProcessedPoints()
        {
            MidTablePoints = new Vector3[16];
            ProcessedPoints = new Vector3[16];

            MidTablePoints[0] = RawPoints[0];
            MidTablePoints[1] = RawToMidEq1(1, 0);
            MidTablePoints[2] = RawToMidEq2(2, 1, 1);
            MidTablePoints[3] = RawToMidEq3(3, 2, 1, 0);

            MidTablePoints[4] = RawPoints[4];
            MidTablePoints[5] = RawToMidEq1(5, 4);
            MidTablePoints[6] = RawToMidEq2(6, 5, 5);
            MidTablePoints[7] = RawToMidEq3(7, 6, 5, 4);

            MidTablePoints[8] = RawPoints[8];
            MidTablePoints[9] = RawToMidEq1(9, 8);
            MidTablePoints[10] = RawToMidEq2(10, 9, 9);
            MidTablePoints[11] = RawToMidEq3(11, 10, 9, 8);

            MidTablePoints[12] = RawPoints[12];
            MidTablePoints[13] = RawToMidEq1(13, 12);
            MidTablePoints[14] = RawToMidEq2(14, 13, 13);
            MidTablePoints[15] = RawToMidEq3(15, 14, 13, 12);

            ProcessedPoints[0] = MidTablePoints[0];
            ProcessedPoints[1] = MidTablePoints[1];
            ProcessedPoints[2] = MidTablePoints[2];
            ProcessedPoints[3] = MidTablePoints[3];

            ProcessedPoints[4] = MidToProcessedEq1(4, 0);
            ProcessedPoints[5] = MidToProcessedEq1(5, 1);
            ProcessedPoints[6] = MidToProcessedEq1(6, 2);
            ProcessedPoints[7] = MidToProcessedEq1(7, 3);

            ProcessedPoints[8] = MidToProcessedEq2(8, 4, 4);
            ProcessedPoints[9] = MidToProcessedEq2(9, 5, 5);
            ProcessedPoints[10] = MidToProcessedEq2(10, 6, 6);
            ProcessedPoints[11] = MidToProcessedEq2(11, 7, 7);

            ProcessedPoints[12] = MidToProcessedEq3(12, 8, 4, 0);
            ProcessedPoints[13] = MidToProcessedEq3(13, 9, 5, 1);
            ProcessedPoints[14] = MidToProcessedEq3(14, 10, 6, 2);
            ProcessedPoints[15] = MidToProcessedEq3(15, 11, 7, 3);
        }

        Vector3 RawToMidEq1(int A, int B)
        {
            return (RawPoints[A] - RawPoints[B]) * 3;
        }

        Vector3 RawToMidEq2(int A, int B, int midA)
        {
            return (RawPoints[A] - RawPoints[B]) * 3 - MidTablePoints[midA];
        }

        Vector3 RawToMidEq3(int a, int midA, int midB, int midC)
        {
            return RawPoints[a] - MidTablePoints[midA] - MidTablePoints[midB] - MidTablePoints[midC];
        }

        Vector3 MidToProcessedEq1(int A, int B)
        {
            return (MidTablePoints[A] - MidTablePoints[B]) * 3;
        }

        Vector3 MidToProcessedEq2(int A, int B, int EndA)
        {
            return (MidTablePoints[A] - MidTablePoints[B]) * 3 - ProcessedPoints[EndA];
        }

        Vector3 MidToProcessedEq3(int a, int midA, int midB, int midC)
        {
            return MidTablePoints[a] - ProcessedPoints[midA] - ProcessedPoints[midB] - ProcessedPoints[midC];
        }
        #endregion

        #region Spline Coefficients
        public Vector3 GetPoint(float t)
        {
            float u = 1f - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 point = (uuu) * RawPoints[0];            
            point += (3f * uu * t) * RawPoints[1];          
            point += (3f * u * tt) * RawPoints[2];           
            point += (ttt) * RawPoints[3];                   

            return point;
        }

        public float[] CalcCoefficients(int samples = 200)
        {
            var ts = new List<float>();
            for (int i = 0; i < samples; i++)
                ts.Add(i / (float)(samples - 1));

            var pts = new List<Vector3>();
            foreach (var t in ts)
                pts.Add(GetPoint(t));

            var dists = new List<float> { 0.0f };
            float total = 0.0f;

            for (int i = 1; i < samples; i++)
            {
                float dx = pts[i].X - pts[i - 1].X;
                float dy = pts[i].Y - pts[i - 1].Y;
                float dz = pts[i].Z - pts[i - 1].Z;
                total += MathF.Sqrt(dx * dx + dy * dy + dz * dz);
                dists.Add(total);
            }

            // Convert to meters
            var lengths = new List<float>();
            foreach (var d in dists)
                lengths.Add(d * 0.01f);

            // Compute sums for cubic regression
            int n = lengths.Count;
            float sx = 0, sx2 = 0, sx3 = 0, sx4 = 0, sx5 = 0, sx6 = 0;
            float sy = 0, sxy = 0, sx2y = 0, sx3y = 0;

            for (int i = 0; i < n; i++)
            {
                float x = lengths[i];
                float y = ts[i];
                float x2 = x * x;
                float x3 = x2 * x;

                sx += x;
                sx2 += x2;
                sx3 += x3;
                sx4 += x2 * x2;
                sx5 += x3 * x2;
                sx6 += x3 * x3;

                sy += y;
                sxy += x * y;
                sx2y += x2 * y;
                sx3y += x3 * y;
            }

            // Normal equations for cubic regression
            float[,] A =
            {
            { sx6, sx5, sx4, sx3 },
            { sx5, sx4, sx3, sx2 },
            { sx4, sx3, sx2, sx },
            { sx3, sx2, sx, n }
        };
            float[] B = { sx3y, sx2y, sxy, sy };

            // Gaussian elimination
            for (int i = 0; i < 4; i++)
            {
                float pivot = A[i, i];
                for (int j = i; j < 4; j++)
                    A[i, j] /= pivot;
                B[i] /= pivot;

                for (int k = i + 1; k < 4; k++)
                {
                    float factor = A[k, i];
                    for (int j = i; j < 4; j++)
                        A[k, j] -= factor * A[i, j];
                    B[k] -= factor * B[i];
                }
            }

            // Back substitution
            float[] u = new float[4];
            for (int i = 3; i >= 0; i--)
            {
                float sum = 0;
                for (int j = i + 1; j < 4; j++)
                    sum += A[i, j] * u[j];
                u[i] = B[i] - sum;
            }

            return u;
        }
        #endregion
    }
}
