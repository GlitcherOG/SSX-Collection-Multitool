using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTriStrip;

namespace SSXMultiTool.FileHandlers.Models
{
    public class TristripGenerator
    {
        public static List<IndiceFace> NeighbourPriority(List<IndiceFace> indiceFaces)
        {
            for (int i = 0; i < indiceFaces.Count; i++)
            {
                var TempFace = indiceFaces[i];
                TempFace.Neighbours = 0;
                for (int a = 0; a < indiceFaces.Count; a++)
                {
                    if (!indiceFaces[a].Tristripped)
                    {
                        int PointShare = 0;

                        if (TempFace.Id1 == indiceFaces[a].Id1 || TempFace.Id1 == indiceFaces[a].Id2 || TempFace.Id1 == indiceFaces[a].Id3)
                        {
                            PointShare++;
                        }

                        if (TempFace.Id2 == indiceFaces[a].Id1 || TempFace.Id2 == indiceFaces[a].Id2 || TempFace.Id2 == indiceFaces[a].Id3)
                        {
                            PointShare++;
                        }

                        if (TempFace.Id3 == indiceFaces[a].Id1 || TempFace.Id3 == indiceFaces[a].Id2 || TempFace.Id3 == indiceFaces[a].Id3)
                        {
                            PointShare++;
                        }

                        if (PointShare >= 2)
                        {
                            TempFace.Neighbours++;
                        }
                    }
                }

                indiceFaces[i] = TempFace;
            }

            indiceFaces = indiceFaces.OrderBy(x => x.Neighbours).ToList();

            return indiceFaces;
        }


        public static List<IndiceTristrip> GenerateTristripBasic(List<IndiceFace> indiceFaces)
        {
            List<IndiceTristrip> tristripList = new List<IndiceTristrip>();
            IndiceTristrip tristrip = new IndiceTristrip();
            tristrip.Indices = new List<int>();

            bool rotation = false;

            while (true)
            {
                if (tristrip.Indices.Count != 0)
                {
                    //Tristrip data ensuring that all the material id is taken into account
                    bool EndedEarly = false;
                    for (int i = 0; i < indiceFaces.Count; i++)
                    {
                        var TempFace = indiceFaces[i];
                        if (TempFace.MaterialID == tristrip.MaterialID && !TempFace.Tristripped)
                        {
                            int Edge = SharesEdge(rotation, TempFace, tristrip);
                            if (Edge != -1)
                            {
                                TempFace.Tristripped = true;
                                rotation = !rotation;
                                if (Edge == 1)
                                {
                                    tristrip.Indices.Add(TempFace.Id1);
                                }
                                else if (Edge == 2)
                                {
                                    tristrip.Indices.Add(TempFace.Id2);
                                }
                                else if (Edge == 3)
                                {
                                    tristrip.Indices.Add(TempFace.Id3);
                                }
                                indiceFaces[i] = TempFace;
                                EndedEarly = true;
                                indiceFaces = NeighbourPriority(indiceFaces);
                                break;
                            }
                        }
                        if (i >= indiceFaces.Count - 1)
                        {
                            EndedEarly = false;
                        }
                    }

                    if (!EndedEarly)
                    {
                        tristripList.Add(tristrip);
                        indiceFaces = NeighbourPriority(indiceFaces);
                        tristrip = new IndiceTristrip();
                        tristrip.Indices = new List<int>();
                    }
                }
                else
                {
                    bool FullRunTest = false;
                    for (int i = 0; i < indiceFaces.Count; i++)
                    {
                        var TempFace = indiceFaces[i];
                        if (!TempFace.Tristripped)
                        {
                            FullRunTest = true;
                            rotation = false;
                            TempFace.Tristripped = true;
                            tristrip.Indices.Add(TempFace.Id1);
                            tristrip.Indices.Add(TempFace.Id2);
                            tristrip.Indices.Add(TempFace.Id3);

                            tristrip.MaterialID = TempFace.MaterialID;
                            indiceFaces[i] = TempFace;
                            indiceFaces = NeighbourPriority(indiceFaces);
                            break;
                        }
                    }

                    if (!FullRunTest)
                    {
                        break;
                    }
                }
            }

            return tristripList;
        }

        public static List<IndiceTristrip> GenerateTristripRotationTesting(List<IndiceFace> indiceFaces)
        {
            List<IndiceTristrip> tristripList = new List<IndiceTristrip>();
            IndiceTristrip tristrip = new IndiceTristrip();
            tristrip.Indices = new List<int>();

            bool rotation = false;

            int StartRotation = 0;
            bool RotationTestDone = false;
            int BestRotation = 0;
            int MaxRotation = 0;
            List<int> TempFaces = new List<int>();
            while (true)
            {
                if (tristrip.Indices.Count != 0)
                {
                    //Tristrip data ensuring that all the material id is taken into account
                    bool EndedEarly = false;
                    for (int i = 0; i < indiceFaces.Count; i++)
                    {
                        var TempFace = indiceFaces[i];
                        if (TempFace.MaterialID == tristrip.MaterialID && !TempFace.Tristripped)
                        {
                            int Edge = SharesEdge(rotation, TempFace, tristrip);
                            if (Edge != -1)
                            {
                                TempFace.Tristripped = true;
                                rotation = !rotation;
                                TempFaces.Add(i);
                                if (Edge == 1)
                                {
                                    tristrip.Indices.Add(TempFace.Id1);
                                }
                                else if (Edge == 2)
                                {
                                    tristrip.Indices.Add(TempFace.Id2);
                                }
                                else if (Edge == 3)
                                {
                                    tristrip.Indices.Add(TempFace.Id3);
                                }
                                indiceFaces[i] = TempFace;
                                EndedEarly = true;
                                indiceFaces = NeighbourPriority(indiceFaces);
                                break;
                            }
                        }
                        if (i >= indiceFaces.Count - 1)
                        {
                            EndedEarly = false;
                        }
                    }

                    if (!EndedEarly)
                    {
                        if (!RotationTestDone)
                        {
                            if (MaxRotation < tristrip.Indices.Count)
                            {
                                MaxRotation = tristrip.Indices.Count;
                                BestRotation = StartRotation;
                            }

                            for (int i = 0; i < indiceFaces.Count; i++)
                            {
                                var TempFace = indiceFaces[i];
                                if (TempFaces.Contains(i))
                                {
                                    TempFace.Tristripped = false;
                                }
                                indiceFaces[i] = TempFace;
                            }
                            tristrip = new IndiceTristrip();
                            tristrip.Indices = new List<int>();
                            TempFaces = new List<int>();
                        }
                        else
                        {
                            tristripList.Add(tristrip);
                            StartRotation = 0;
                            MaxRotation = 0;
                            BestRotation = 0;
                            indiceFaces = NeighbourPriority(indiceFaces);
                            TempFaces = new List<int>();
                            tristrip = new IndiceTristrip();
                            tristrip.Indices = new List<int>();
                        }
                    }
                }
                else
                {
                    bool FullRunTest = false;
                    for (int i = 0; i < indiceFaces.Count; i++)
                    {
                        var TempFace = indiceFaces[i];
                        if (!TempFace.Tristripped)
                        {
                            StartRotation++;
                            FullRunTest = true;
                            rotation = false;
                            TempFace.Tristripped = true;

                            if (StartRotation == 4)
                            {
                                StartRotation = BestRotation;
                                RotationTestDone = true;
                            }

                            if (StartRotation == 1)
                            {
                                tristrip.Indices.Add(TempFace.Id1);
                                tristrip.Indices.Add(TempFace.Id2);
                                tristrip.Indices.Add(TempFace.Id3);
                            }
                            else if (StartRotation == 2)
                            {
                                tristrip.Indices.Add(TempFace.Id2);
                                tristrip.Indices.Add(TempFace.Id3);
                                tristrip.Indices.Add(TempFace.Id1);
                            }
                            else if (StartRotation == 3)
                            {
                                tristrip.Indices.Add(TempFace.Id3);
                                tristrip.Indices.Add(TempFace.Id1);
                                tristrip.Indices.Add(TempFace.Id2);
                            }
                            TempFaces.Add(i);
                            tristrip.MaterialID = TempFace.MaterialID;
                            indiceFaces[i] = TempFace;
                            indiceFaces = NeighbourPriority(indiceFaces);
                            break;
                        }
                    }

                    if (!FullRunTest)
                    {
                        break;
                    }
                }
            }

            return tristripList;
        }

        public static List<IndiceTristrip> GenerateTristripStiching(List<IndiceFace> indiceFaces)
        {
            List<IndiceTristrip> tristripList = new List<IndiceTristrip>();
            IndiceTristrip tristrip = new IndiceTristrip();
            tristrip.Indices = new List<int>();

            bool rotation = false;
            bool FailedTest = false;
            while (true)
            {
                if (tristrip.Indices.Count != 0)
                {
                    //Tristrip data ensuring that all the material id is taken into account
                    bool EndedEarly = false;
                    for (int i = 0; i < indiceFaces.Count; i++)
                    {
                        var TempFace = indiceFaces[i];
                        if (TempFace.MaterialID == tristrip.MaterialID && !TempFace.Tristripped)
                        {
                            int Edge = SharesEdge(rotation, TempFace, tristrip);
                            if (Edge != -1)
                            {
                                TempFace.Tristripped = true;
                                rotation = !rotation;
                                if (Edge == 1)
                                {
                                    tristrip.Indices.Add(TempFace.Id1);
                                }
                                else if (Edge == 2)
                                {
                                    tristrip.Indices.Add(TempFace.Id2);
                                }
                                else if (Edge == 3)
                                {
                                    tristrip.Indices.Add(TempFace.Id3);
                                }
                                indiceFaces[i] = TempFace;
                                EndedEarly = true;
                                indiceFaces = NeighbourPriority(indiceFaces);
                                break;
                            }
                        }
                        if (i >= indiceFaces.Count - 1)
                        {
                            EndedEarly = false;
                        }
                    }

                    if (!EndedEarly)
                    {
                        if (!FailedTest)
                        {
                            for (int i = 0; i < indiceFaces.Count; i++)
                            {
                                var TempFace = indiceFaces[i];
                                if (TempFace.MaterialID == tristrip.MaterialID && !TempFace.Tristripped)
                                {
                                    int Edge = SharesEdge2(rotation, TempFace, tristrip);
                                    if (Edge != -1)
                                    {
                                        tristrip.Indices.Add(0);
                                        tristrip.Indices.Add(tristrip.Indices[tristrip.Indices.Count - 2]);
                                        int Index = tristrip.Indices.Count - 1;
                                        TempFace.Tristripped = true;
                                        if (Edge == 1)
                                        {
                                            if (rotation)
                                            {
                                                tristrip.Indices[Index - 1] = TempFace.Id2;
                                            }
                                            else
                                            {
                                                tristrip.Indices[Index - 1] = TempFace.Id3;
                                            }
                                            tristrip.Indices.Add(TempFace.Id1);
                                        }
                                        else if (Edge == 2)
                                        {
                                            if (rotation)
                                            {
                                                tristrip.Indices[Index - 1] = TempFace.Id3;
                                            }
                                            else
                                            {
                                                tristrip.Indices[Index - 1] = TempFace.Id1;
                                            }
                                            tristrip.Indices.Add(TempFace.Id2);
                                        }
                                        else if (Edge == 3)
                                        {
                                            if (rotation)
                                            {
                                                tristrip.Indices[Index - 1] = TempFace.Id1;
                                            }
                                            else
                                            {
                                                tristrip.Indices[Index - 1] = TempFace.Id2;
                                            }
                                            tristrip.Indices.Add(TempFace.Id3);
                                        }
                                        rotation = !rotation;
                                        indiceFaces[i] = TempFace;
                                        break;
                                    }
                                }
                                if (i >= indiceFaces.Count - 1)
                                {
                                    FailedTest = true;
                                }
                            }
                        }
                        else
                        {
                            tristripList.Add(tristrip);
                            FailedTest = false;
                            indiceFaces = NeighbourPriority(indiceFaces);
                            tristrip = new IndiceTristrip();
                            tristrip.Indices = new List<int>();
                        }
                    }
                }
                else
                {
                    bool FullRunTest = false;
                    for (int i = 0; i < indiceFaces.Count; i++)
                    {
                        var TempFace = indiceFaces[i];
                        if (!TempFace.Tristripped)
                        {
                            FullRunTest = true;
                            rotation = false;
                            TempFace.Tristripped = true;
                            tristrip.Indices.Add(TempFace.Id1);
                            tristrip.Indices.Add(TempFace.Id2);
                            tristrip.Indices.Add(TempFace.Id3);

                            tristrip.MaterialID = TempFace.MaterialID;
                            indiceFaces[i] = TempFace;
                            indiceFaces = NeighbourPriority(indiceFaces);
                            break;
                        }
                    }

                    if (!FullRunTest)
                    {
                        break;
                    }
                }
            }

            return tristripList;
        }

        public static List<IndiceTristrip> GenerateTristripRotationTestingN(List<IndiceFace> indiceFaces)
        {
            List<IndiceTristrip> tristripList = new List<IndiceTristrip>();
            IndiceTristrip tristrip = new IndiceTristrip();
            tristrip.Indices = new List<int>();

            bool rotation = false;
            bool FailedTest = false;

            int StartRotation = 0;
            bool RotationTestDone = false;
            int BestRotation = 0;
            int MaxRotation = 0;
            List<int> TempFaces = new List<int>();


            while (true)
            {
                if (tristrip.Indices.Count != 0)
                {
                    //Tristrip data ensuring that all the material id is taken into account
                    bool EndedEarly = false;
                    for (int i = 0; i < indiceFaces.Count; i++)
                    {
                        var TempFace = indiceFaces[i];
                        if (TempFace.MaterialID == tristrip.MaterialID && !TempFace.Tristripped)
                        {
                            int Edge = SharesEdge(rotation, TempFace, tristrip);
                            if (Edge != -1)
                            {
                                TempFace.Tristripped = true;
                                rotation = !rotation;
                                if (Edge == 1)
                                {
                                    tristrip.Indices.Add(TempFace.Id1);
                                }
                                else if (Edge == 2)
                                {
                                    tristrip.Indices.Add(TempFace.Id2);
                                }
                                else if (Edge == 3)
                                {
                                    tristrip.Indices.Add(TempFace.Id3);
                                }
                                TempFaces.Add(i);
                                indiceFaces[i] = TempFace;
                                EndedEarly = true;
                                indiceFaces = NeighbourPriority(indiceFaces);
                                break;
                            }
                        }
                        if (i >= indiceFaces.Count - 1)
                        {
                            EndedEarly = false;
                        }
                    }

                    if (!EndedEarly)
                    {
                        if (!FailedTest)
                        {
                            for (int i = 0; i < indiceFaces.Count; i++)
                            {
                                var TempFace = indiceFaces[i];
                                if (TempFace.MaterialID == tristrip.MaterialID && !TempFace.Tristripped)
                                {
                                    int Edge = SharesEdge2(rotation, TempFace, tristrip);
                                    if (Edge != -1)
                                    {
                                        tristrip.Indices.Add(0);
                                        tristrip.Indices.Add(tristrip.Indices[tristrip.Indices.Count - 2]);
                                        int Index = tristrip.Indices.Count - 1;
                                        TempFace.Tristripped = true;
                                        if (Edge == 1)
                                        {
                                            if (rotation)
                                            {
                                                tristrip.Indices[Index - 1] = TempFace.Id2;
                                            }
                                            else
                                            {
                                                tristrip.Indices[Index - 1] = TempFace.Id3;
                                            }
                                            tristrip.Indices.Add(TempFace.Id1);
                                        }
                                        else if (Edge == 2)
                                        {
                                            if (rotation)
                                            {
                                                tristrip.Indices[Index - 1] = TempFace.Id3;
                                            }
                                            else
                                            {
                                                tristrip.Indices[Index - 1] = TempFace.Id1;
                                            }
                                            tristrip.Indices.Add(TempFace.Id2);
                                        }
                                        else if (Edge == 3)
                                        {
                                            if (rotation)
                                            {
                                                tristrip.Indices[Index - 1] = TempFace.Id1;
                                            }
                                            else
                                            {
                                                tristrip.Indices[Index - 1] = TempFace.Id2;
                                            }
                                            tristrip.Indices.Add(TempFace.Id3);
                                        }
                                        TempFaces.Add(i);
                                        rotation = !rotation;
                                        indiceFaces[i] = TempFace;
                                        indiceFaces = NeighbourPriority(indiceFaces);
                                        break;
                                    }
                                }
                                if (i >= indiceFaces.Count - 1)
                                {
                                    FailedTest = true;
                                }
                            }
                        }
                        else
                        {
                            if (!RotationTestDone)
                            {
                                if (MaxRotation < tristrip.Indices.Count)
                                {
                                    MaxRotation = tristrip.Indices.Count;
                                    BestRotation = StartRotation;
                                }

                                for (int i = 0; i < indiceFaces.Count; i++)
                                {
                                    var TempFace = indiceFaces[i];
                                    if (TempFaces.Contains(i))
                                    {
                                        TempFace.Tristripped = false;
                                    }
                                    indiceFaces[i] = TempFace;
                                }
                                tristrip = new IndiceTristrip();
                                tristrip.Indices = new List<int>();
                                TempFaces = new List<int>();
                            }
                            else
                            {
                                tristripList.Add(tristrip);
                                FailedTest = false;
                                StartRotation = 0;
                                MaxRotation = 0;
                                BestRotation = 0;
                                indiceFaces = NeighbourPriority(indiceFaces);
                                TempFaces = new List<int>();
                                tristrip = new IndiceTristrip();
                                tristrip.Indices = new List<int>();
                            }
                        }
                    }
                }
                else
                {
                    bool FullRunTest = false;
                    for (int i = 0; i < indiceFaces.Count; i++)
                    {
                        var TempFace = indiceFaces[i];
                        if (!TempFace.Tristripped)
                        {
                            StartRotation++;
                            FullRunTest = true;
                            rotation = false;
                            TempFace.Tristripped = true;

                            if (StartRotation == 4)
                            {
                                StartRotation = BestRotation;
                                RotationTestDone = true;
                            }

                            if (StartRotation == 1)
                            {
                                tristrip.Indices.Add(TempFace.Id1);
                                tristrip.Indices.Add(TempFace.Id2);
                                tristrip.Indices.Add(TempFace.Id3);
                            }
                            else if (StartRotation == 2)
                            {
                                tristrip.Indices.Add(TempFace.Id2);
                                tristrip.Indices.Add(TempFace.Id3);
                                tristrip.Indices.Add(TempFace.Id1);
                            }
                            else if (StartRotation == 3)
                            {
                                tristrip.Indices.Add(TempFace.Id3);
                                tristrip.Indices.Add(TempFace.Id1);
                                tristrip.Indices.Add(TempFace.Id2);
                            }
                            TempFaces.Add(i);
                            tristrip.MaterialID = TempFace.MaterialID;
                            indiceFaces[i] = TempFace;
                            indiceFaces = NeighbourPriority(indiceFaces);
                            break;
                        }
                    }

                    if (!FullRunTest)
                    {
                        break;
                    }
                }
            }

            return tristripList;
        }

        public static List<IndiceTristrip> GenerateTristripNivda(List<IndiceFace> indiceFaces)
        {
            List<IndiceTristrip> tristripList = new List<IndiceTristrip>();

            ushort[] Index = new ushort[indiceFaces.Count * 3];

            for (int i = 0; i < indiceFaces.Count; i++)
            {
                Index[i * 3] = (ushort)indiceFaces[i].Id1;
                Index[i * 3 + 1] = (ushort)indiceFaces[i].Id2;
                Index[i * 3 + 2] = (ushort)indiceFaces[i].Id3;
            }

            var TempPrimativeGroup = ToTriangleStrips(Index, false);

            for (int i = 0; i < TempPrimativeGroup.Length; i++)
            {
                var TempIndiceTristrip = new IndiceTristrip();
                TempIndiceTristrip.Indices = new List<int>();

                for (int a = 0; a < TempPrimativeGroup[i].Indices.Length; a++)
                {
                    TempIndiceTristrip.Indices.Add(TempPrimativeGroup[i].Indices[a]);
                }
                tristripList.Add(TempIndiceTristrip);
            }

            return tristripList;
        }

        public static TriStrip.PrimitiveGroup[] ToTriangleStrips(ushort[] indexBuffer, bool validateStrips)
        {
            var triStrip = new TriStrip(); // create new class instance

            triStrip.DisableRestart(); // we want separate strips, so restart is not needed
            triStrip.SetCacheSize(24); // GeForce1/2 vertex cache size is 16
            triStrip.SetListsOnly(false); // we want separate strips, not optimized list
            triStrip.SetMinStripSize(0); // minimum triangle count in a strip is 0
            triStrip.SetStitchStrips(false); // don't stitch strips into one huge strip

            if (triStrip.GenerateStrips(indexBuffer, out var result, validateStrips))
            {
                return result; // if strips were generated and validated correctly, return
            }

            return null; // if something went wrong, return null (or throw instead)
        }


        static int SharesEdge(bool CounterClockwise, IndiceFace TempFace, IndiceTristrip tristrip)
        {
            int Index = tristrip.Indices.Count - 1;
            int Index2 = 0;
            int Index3 = 0;
            if (CounterClockwise)
            {
                Index2 = Index;
                Index3 = Index - 1;
            }
            else
            {
                Index2 = Index - 1;
                Index3 = Index;
            }

            if (TempFace.Id1 == tristrip.Indices[Index3] && TempFace.Id2 == tristrip.Indices[Index2])
            {
                return 3;
            }

            if (TempFace.Id2 == tristrip.Indices[Index3] && TempFace.Id3 == tristrip.Indices[Index2])
            {
                return 1;
            }

            if (TempFace.Id3 == tristrip.Indices[Index3] && TempFace.Id1 == tristrip.Indices[Index2])
            {
                return 2;
            }
            return -1;
        }

        static int SharesEdge2(bool CounterClockwise, IndiceFace TempFace, IndiceTristrip tristrip)
        {
            tristrip.Indices.Add(0);
            tristrip.Indices.Add(tristrip.Indices[tristrip.Indices.Count - 2]);

            int Index = tristrip.Indices.Count - 1;
            int Index2 = 0;
            int Index3 = 0;

            if (CounterClockwise)
            {
                Index2 = Index;
                Index3 = Index - 1;
            }
            else
            {
                Index2 = Index - 1;
                Index3 = Index;
            }

            if (CounterClockwise)
            {
                tristrip.Indices[Index - 1] = TempFace.Id1;
            }
            else
            {
                tristrip.Indices[Index - 1] = TempFace.Id2;
            }

            if (TempFace.Id1 == tristrip.Indices[Index3] && TempFace.Id2 == tristrip.Indices[Index2])
            {
                tristrip.Indices.RemoveRange(tristrip.Indices.Count - 3, 2);
                return 3;
            }

            if (CounterClockwise)
            {
                tristrip.Indices[Index - 1] = TempFace.Id2;
            }
            else
            {
                tristrip.Indices[Index - 1] = TempFace.Id3;
            }

            if (TempFace.Id2 == tristrip.Indices[Index3] && TempFace.Id3 == tristrip.Indices[Index2])
            {
                tristrip.Indices.RemoveRange(tristrip.Indices.Count - 3, 2);
                return 1;
            }

            if (CounterClockwise)
            {
                tristrip.Indices[Index - 1] = TempFace.Id3;
            }
            else
            {
                tristrip.Indices[Index - 1] = TempFace.Id1;
            }

            if (TempFace.Id3 == tristrip.Indices[Index3] && TempFace.Id1 == tristrip.Indices[Index2])
            {
                tristrip.Indices.RemoveRange(tristrip.Indices.Count - 3, 2);
                return 2;
            }
            tristrip.Indices.RemoveRange(tristrip.Indices.Count - 3, 2);
            return -1;
        }

        public struct IndiceFace
        {
            public bool Tristripped;
            public int MaterialID;
            public int Neighbours;

            public int Id1;
            public int Id2;
            public int Id3;

            public int NeighbourId12;
            public int NeighbourId23;
            public int NeighbourId31;
        }

        public struct IndiceTristrip
        {
            public int MaterialID;
            public List<int> Indices;
        }
    }
}
