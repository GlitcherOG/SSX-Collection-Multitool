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
    }
}
