using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.ThreeD;
using Aspose.ThreeD.Animation;
using Aspose.ThreeD.Entities;
using Aspose.ThreeD.Formats;
using System.Numerics;

namespace SSXMultiTool.FileHandlers.ModelsExport
{
    internal class AsposeHandler
    {


        public void SaveTrickyModel(string Output, TrickyMPFModelHandler Handler)
        {
            Scene scene = new Scene();
            scene.AssetInfo.UpVector = Axis.ZAxis;
            Node MainNode = new Node("Mesh");
            Mesh mesh = new Mesh();

            List<MeshMaterialData> MaterialsData = new List<MeshMaterialData>();
            for (int i = 0; i < Handler.ModelList.Count; i++)
            {
                var model = Handler.ModelList[i];
                for (int a = 0; a < model.MeshGroups.Count; a++)
                {
                    var MeshGroups = model.MeshGroups[a];
                    for (int b = 0; b < MeshGroups.meshGroupSubs.Count; b++)
                    {
                        var MeshSubGroup = MeshGroups.meshGroupSubs[b];
                        for (int c = 0; c < MeshSubGroup.MeshGroupHeaders.Count; c++)
                        {
                            var MeshGroupHeader = MeshSubGroup.MeshGroupHeaders[c];
                            MeshMaterialData matData = new MeshMaterialData();
                            matData.IndexVertices = new List<int>();
                            matData.IndexNormals = new List<int>();
                            matData.IndexUVS = new List<int>();
                            matData.vertices = new List<Vector3>();
                            matData.normals = new List<Vector3>();
                            matData.uvs = new List<Vector4>();
                            for (int d = 0; d < MeshGroupHeader.staticMesh.Count; d++)
                            {
                                for (int e = 0; e < MeshGroupHeader.staticMesh[d].faces.Count; e++)
                                {
                                    var Face = MeshGroupHeader.staticMesh[d].faces[e];
                                    //Vertices
                                    if (!matData.vertices.Contains(Face.V1))
                                    {
                                        matData.vertices.Add(Face.V1);
                                    }
                                    matData.IndexVertices.Add(matData.vertices.IndexOf(Face.V1));

                                    if (!matData.vertices.Contains(Face.V2))
                                    {
                                        matData.vertices.Add(Face.V2);
                                    }
                                    matData.IndexVertices.Add(matData.vertices.IndexOf(Face.V2));

                                    if (!matData.vertices.Contains(Face.V3))
                                    {
                                        matData.vertices.Add(Face.V3);
                                    }
                                    matData.IndexVertices.Add(matData.vertices.IndexOf(Face.V3));

                                    //UVs
                                    if (!matData.uvs.Contains(Face.UV1))
                                    {
                                        matData.uvs.Add(Face.UV1);
                                    }
                                    matData.IndexUVS.Add(matData.uvs.IndexOf(Face.UV1));

                                    if (!matData.uvs.Contains(Face.UV2))
                                    {
                                        matData.uvs.Add(Face.UV2);
                                    }
                                    matData.IndexUVS.Add(matData.uvs.IndexOf(Face.UV2));

                                    if (!matData.uvs.Contains(Face.UV3))
                                    {
                                        matData.uvs.Add(Face.UV3);
                                    }
                                    matData.IndexUVS.Add(matData.uvs.IndexOf(Face.UV3));

                                    //Normals
                                    if (!matData.normals.Contains(Face.Normal1))
                                    {
                                        matData.normals.Add(Face.Normal1);
                                    }
                                    matData.IndexNormals.Add(matData.normals.IndexOf(Face.Normal1));

                                    if (!matData.normals.Contains(Face.Normal2))
                                    {
                                        matData.normals.Add(Face.Normal2);
                                    }
                                    matData.IndexNormals.Add(matData.normals.IndexOf(Face.Normal2));

                                    if (!matData.normals.Contains(Face.Normal3))
                                    {
                                        matData.normals.Add(Face.Normal3);
                                    }
                                    matData.IndexNormals.Add(matData.normals.IndexOf(Face.Normal3));
                                }
                            }
                            MaterialsData.Add(matData);
                        }
                    }
                }
            }


            scene.Save(Output, FileFormat.FBX7700Binary);
        }



    }


    struct MeshMaterialData
    {


        public List<int> IndexVertices;
        public List<int> IndexNormals;
        public List<int> IndexUVS;
        public List<Vector3> vertices;
        public List<Vector3> normals;
        public List<Vector4> uvs;
    }
}
