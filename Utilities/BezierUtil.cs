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
        public Vector3 RawR1C1;
        public Vector3 RawR1C2;
        public Vector3 RawR1C3;
        public Vector3 RawR1C4;
        public Vector3 RawR2C1;
        public Vector3 RawR2C2;
        public Vector3 RawR2C3;
        public Vector3 RawR2C4;
        public Vector3 RawR3C1;
        public Vector3 RawR3C2;
        public Vector3 RawR3C3;
        public Vector3 RawR3C4;
        public Vector3 RawR4C1;
        public Vector3 RawR4C2;
        public Vector3 RawR4C3;
        public Vector3 RawR4C4;

        public Vector3 ProcessedR1C1;
        public Vector3 ProcessedR1C2;
        public Vector3 ProcessedR1C3;
        public Vector3 ProcessedR1C4;
        public Vector3 ProcessedR2C1;
        public Vector3 ProcessedR2C2;
        public Vector3 ProcessedR2C3;
        public Vector3 ProcessedR2C4;
        public Vector3 ProcessedR3C1;
        public Vector3 ProcessedR3C2;
        public Vector3 ProcessedR3C3;
        public Vector3 ProcessedR3C4;
        public Vector3 ProcessedR4C1;
        public Vector3 ProcessedR4C2;
        public Vector3 ProcessedR4C3;
        public Vector3 ProcessedR4C4;

        public void GenerateRawPatch()
        {
            //Row 1
            RawR1C1 = ProcessedR1C1;
            RawR1C2 = RawR1C1 + ProcessedR1C2 / 3;
            RawR1C3 = RawR1C2 + (ProcessedR1C2 + ProcessedR1C3) / 3;
            RawR1C4 = RawR1C1 + ProcessedR1C2 + ProcessedR1C3 + ProcessedR1C4;

            //Row2
            RawR2C1 = RawR1C1 + ProcessedR2C1 / 3;
            RawR2C2 = RawR1C1 + (ProcessedR1C2 + ProcessedR2C1 + ProcessedR2C2 / 3) / 3;
            RawR2C3 = RawR2C2 + (ProcessedR1C2 + ProcessedR1C3 + (ProcessedR2C2 + ProcessedR2C3) / 3) / 3;
            RawR2C4 = RawR1C4 + (ProcessedR2C1 + ProcessedR2C2 + ProcessedR2C3 + ProcessedR2C4) / 3;

            //Row 3
            RawR3C1 = RawR2C1 + (ProcessedR2C1 + ProcessedR3C1) / 3;
            RawR3C2 = RawR2C2 + (ProcessedR2C1 + ProcessedR3C1 + (ProcessedR2C2 + ProcessedR3C2) / 3) / 3;

            //DUE TO THE MATH CALUCLATION TO WORK THIS OUT BEING UNKNOWN THIS ONE ISNT DONE
            //RawR3C3 = 

            RawR3C4 = (ProcessedR2C1 + ProcessedR2C2 + ProcessedR2C3 + ProcessedR2C4 + ProcessedR3C1 + ProcessedR3C2 + ProcessedR3C3 + ProcessedR3C4) / 3 + RawR2C4;

            ////Row 4
            RawR4C1 = RawR1C1 + ProcessedR2C1 + ProcessedR3C1 + ProcessedR4C1;
            RawR4C2 = RawR4C1 + (ProcessedR1C2 + ProcessedR2C2 + ProcessedR3C2 + ProcessedR4C2) / 3;
            RawR4C3 = RawR4C2 + (ProcessedR4C2 + ProcessedR4C3 + ProcessedR3C2 + ProcessedR3C3 + ProcessedR2C2 + ProcessedR2C3 + ProcessedR1C2 + ProcessedR1C3) / 3;
            RawR4C4 = RawR1C1 + ProcessedR1C2 + ProcessedR1C3 + ProcessedR1C4 + ProcessedR2C1 + ProcessedR2C2 + ProcessedR2C3 + ProcessedR2C4 + ProcessedR3C1 + ProcessedR3C2 + ProcessedR3C3 + ProcessedR3C4 + ProcessedR4C1 + ProcessedR4C2 + ProcessedR4C3 + ProcessedR4C4;
        }

        public void GenerateProcessedPatch()
        {
            ProcessedR1C1 = RawR1C1;
            ProcessedR1C2 = (RawR1C2 - ProcessedR1C1) * 3;
            ProcessedR1C3 = (RawR1C3 - RawR1C2) * 3 - ProcessedR1C2;
            ProcessedR1C4 = RawR1C4 - (ProcessedR1C1 + ProcessedR1C2 + ProcessedR1C3);

            ProcessedR2C1 = (RawR2C1 - ProcessedR1C1) * 3;
            ProcessedR2C2 = RawR2C2 * 9 - ProcessedR1C1 * 9 - ProcessedR1C2 * 3 - ProcessedR2C1 * 3;
            ProcessedR2C3 = RawR2C3 * 9 - RawR2C2 * 9 - ProcessedR1C2 * 3 - ProcessedR1C3 * 3 - ProcessedR2C2;
            ProcessedR2C4 = RawR2C4 * 3 - RawR1C4 * 3 - ProcessedR2C1 - ProcessedR2C2 - ProcessedR2C3;

            ProcessedR3C1 = (RawR3C1 - RawR2C1) * 3 - ProcessedR2C1;
            ProcessedR3C2 = RawR3C2 * 9 - RawR2C2 * 9 - ProcessedR2C1 * 3 - ProcessedR3C1 * 3 - ProcessedR2C2;

            //ProcessedR3C3 = 0;

            ProcessedR3C4 = RawR3C4 * 3 - RawR2C4 * 3 - ProcessedR2C1 - ProcessedR2C2 - ProcessedR2C3 - ProcessedR2C4 - ProcessedR3C1 - ProcessedR3C2 - ProcessedR3C3;

            ProcessedR4C1 = RawR4C1 - (ProcessedR1C1 + ProcessedR2C1 + ProcessedR3C1);
            ProcessedR4C2 = RawR4C2 * 3 - RawR4C1 * 3 - ProcessedR1C2 - ProcessedR2C2 - ProcessedR3C2;
            ProcessedR4C3 = RawR4C3 * 3 - RawR4C2 * 3 - ProcessedR1C2 - ProcessedR2C2 - ProcessedR3C2 - ProcessedR4C2 - ProcessedR1C3 - ProcessedR2C3 - ProcessedR3C3;
            ProcessedR4C4 = RawR4C4 - (ProcessedR1C1 + ProcessedR1C2 + ProcessedR1C3 + ProcessedR1C4 + ProcessedR2C1 + ProcessedR2C2 + ProcessedR2C3 + ProcessedR2C4 + ProcessedR3C1 + ProcessedR3C2 + ProcessedR3C3 + ProcessedR3C4 + ProcessedR4C1 + ProcessedR4C2 + ProcessedR4C3);
        }

        public void GenerateRawCurve()
        {
            RawR1C1 = ProcessedR1C1;
            RawR1C2 = RawR1C1 + ProcessedR1C2 / 3;
            RawR1C3 = RawR1C2 + (ProcessedR1C2 + ProcessedR1C3) / 3;
            RawR1C4 = RawR1C1 + ProcessedR1C2 + ProcessedR1C3 + ProcessedR1C4;
        }

        public void GenerateProcessedCurve()
        {

        }
    }
}
