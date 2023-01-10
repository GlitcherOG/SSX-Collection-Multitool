using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Models
{
    public class TristripGenerator
    {

        public static List<IndiceTristrip> GenerateTristripMethod1(List<IndiceFace> indiceFaces)
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
                                    tristrip.Indices.Add(TempFace.Id2);
                                }
                                indiceFaces[i] = TempFace;
                                EndedEarly = true;
                                break;
                            }
                        }
                        if (i >= indiceFaces.Count - 1)
                        {
                            EndedEarly = false;
                        }
                    }

                    if (!EndedEarly || tristrip.Indices.Count >= 20)
                    {
                        tristripList.Add(tristrip);
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

        public static List<IndiceTristrip> GenerateTristripMethod2(List<IndiceFace> indiceFaces)
        {
            List<IndiceTristrip> tristripList = new List<IndiceTristrip>();

            //Order the faces based on a prority of how many neabours they have

            //Using Order Generate first tristrip

            //Rerun prority

            //Loop Generating tristrip till all are taken

            return tristripList;
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

        //static int SharesEdge2(bool CounterClockwise, TrickyMPFModelHandler.Face TempFace, TristripStruct tristrip, bool LooseCheck)
        //{
        //    int Index = tristrip.vertices.Count - 1;
        //    int Index2 = 0;
        //    int Index3 = 0;

        //    tristrip.vertices.Add(new Vector3(0, 0, 0));
        //    tristrip.normals.Add(new Vector3(0, 0, 0));
        //    tristrip.TextureCords.Add(new Vector4(0, 0, 0, 0));
        //    tristrip.Weights.Add(0);

        //    tristrip.vertices.Add(tristrip.vertices[tristrip.vertices.Count - 2]);
        //    tristrip.normals.Add(tristrip.normals[tristrip.normals.Count - 2]);
        //    tristrip.TextureCords.Add(tristrip.TextureCords[tristrip.TextureCords.Count - 2]);
        //    tristrip.Weights.Add(tristrip.Weights[tristrip.Weights.Count - 2]);

        //    if (CounterClockwise)
        //    {
        //        Index2 = Index;
        //        Index3 = Index - 1;
        //    }
        //    else
        //    {
        //        Index2 = Index - 1;
        //        Index3 = Index;
        //    }

        //    if (CounterClockwise)
        //    {
        //        tristrip.vertices[Index - 1] = TempFace.V1;
        //        tristrip.normals[Index - 1] = TempFace.Normal1;
        //        tristrip.TextureCords[Index - 1] = TempFace.UV1;
        //        tristrip.Weights[Index - 1] = TempFace.Weight1Pos;
        //    }
        //    else
        //    {
        //        tristrip.vertices[Index - 1] = TempFace.V2;
        //        tristrip.normals[Index - 1] = TempFace.Normal2;
        //        tristrip.TextureCords[Index - 1] = TempFace.UV2;
        //        tristrip.Weights[Index - 1] = TempFace.Weight2Pos;
        //    }

        //    if (VerticeEqual(TempFace.V1, tristrip.vertices[Index3]) && VerticeEqual(TempFace.V2, tristrip.vertices[Index2]))
        //    {
        //        if (TempFace.Weight1Pos == tristrip.Weights[Index3] && TempFace.Weight2Pos == tristrip.Weights[Index2])
        //        {
        //            if (!LooseCheck)
        //            {
        //                if (NormalsEqual(TempFace.Normal1, tristrip.normals[Index3]) && NormalsEqual(TempFace.Normal2, tristrip.normals[Index2]))
        //                {
        //                    if (UVEqual(TempFace.UV1, tristrip.TextureCords[Index3]) && UVEqual(TempFace.UV2, tristrip.TextureCords[Index2]))
        //                    {
        //                        tristrip.vertices.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                        tristrip.normals.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                        tristrip.TextureCords.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                        tristrip.Weights.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                        return 3;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                tristrip.vertices.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                tristrip.normals.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                tristrip.TextureCords.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                tristrip.Weights.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                return 3;
        //            }
        //        }
        //    }

        //    if (CounterClockwise)
        //    {
        //        tristrip.vertices[Index - 1] = TempFace.V2;
        //        tristrip.normals[Index - 1] = TempFace.Normal2;
        //        tristrip.TextureCords[Index - 1] = TempFace.UV2;
        //        tristrip.Weights[Index - 1] = TempFace.Weight2Pos;
        //    }
        //    else
        //    {
        //        tristrip.vertices[Index - 1] = TempFace.V3;
        //        tristrip.normals[Index - 1] = TempFace.Normal3;
        //        tristrip.TextureCords[Index - 1] = TempFace.UV3;
        //        tristrip.Weights[Index - 1] = TempFace.Weight3Pos;
        //    }

        //    if (VerticeEqual(TempFace.V2, tristrip.vertices[Index3]) && VerticeEqual(TempFace.V3, tristrip.vertices[Index2]))
        //    {
        //        if (TempFace.Weight2Pos == tristrip.Weights[Index3] && TempFace.Weight3Pos == tristrip.Weights[Index2])
        //        {
        //            if (!LooseCheck)
        //            {
        //                if (NormalsEqual(TempFace.Normal2, tristrip.normals[Index3]) && NormalsEqual(TempFace.Normal3, tristrip.normals[Index2]))
        //                {
        //                    if (UVEqual(TempFace.UV2, tristrip.TextureCords[Index3]) && UVEqual(TempFace.UV3, tristrip.TextureCords[Index2]))
        //                    {
        //                        tristrip.vertices.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                        tristrip.normals.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                        tristrip.TextureCords.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                        tristrip.Weights.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                        return 1;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                tristrip.vertices.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                tristrip.normals.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                tristrip.TextureCords.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                tristrip.Weights.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                return 1;
        //            }
        //        }
        //    }

        //    if (CounterClockwise)
        //    {
        //        tristrip.vertices[Index - 1] = TempFace.V3;
        //        tristrip.normals[Index - 1] = TempFace.Normal3;
        //        tristrip.TextureCords[Index - 1] = TempFace.UV3;
        //        tristrip.Weights[Index - 1] = TempFace.Weight3Pos;
        //    }
        //    else
        //    {
        //        tristrip.vertices[Index - 1] = TempFace.V1;
        //        tristrip.normals[Index - 1] = TempFace.Normal1;
        //        tristrip.TextureCords[Index - 1] = TempFace.UV1;
        //        tristrip.Weights[Index - 1] = TempFace.Weight1Pos;
        //    }

        //    if (VerticeEqual(TempFace.V3, tristrip.vertices[Index3]) && VerticeEqual(TempFace.V1, tristrip.vertices[Index2]))
        //    {
        //        if (TempFace.Weight3Pos == tristrip.Weights[Index3] && TempFace.Weight1Pos == tristrip.Weights[Index2])
        //        {
        //            if (!LooseCheck)
        //            {
        //                if (NormalsEqual(TempFace.Normal3, tristrip.normals[Index3]) && NormalsEqual(TempFace.Normal1, tristrip.normals[Index2]))
        //                {
        //                    if (UVEqual(TempFace.UV3, tristrip.TextureCords[Index3]) && UVEqual(TempFace.UV1, tristrip.TextureCords[Index2]))
        //                    {
        //                        tristrip.vertices.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                        tristrip.normals.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                        tristrip.TextureCords.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                        tristrip.Weights.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                        return 2;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                tristrip.vertices.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                tristrip.normals.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                tristrip.TextureCords.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                tristrip.Weights.RemoveRange(tristrip.vertices.Count - 3, 2);
        //                return 2;
        //            }
        //        }
        //    }

        //    tristrip.vertices.RemoveRange(tristrip.vertices.Count - 3, 2);
        //    tristrip.normals.RemoveRange(tristrip.vertices.Count - 3, 2);
        //    tristrip.TextureCords.RemoveRange(tristrip.vertices.Count - 3, 2);
        //    tristrip.Weights.RemoveRange(tristrip.vertices.Count - 3, 2);

        //    return -1;
        //}



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
