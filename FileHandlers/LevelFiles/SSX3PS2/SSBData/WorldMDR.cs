using SSXMultiTool.JsonFiles.SSX3;
using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2.SSBData
{
    public class WorldMDR
    {
        public int TrackID;
        public int RID;

        public int U1Count;
        public int U1Offset;
        public int U3;
        public int U4;

        public float U6;
        public float U7;
        public float U8;
        public float U9;

        public int ModelDataOffset;
        public int U11Count;

        public List<int> U12 = new List<int>();

        public List<ModelObject> ModelObjects = new List<ModelObject>();

        public void LoadData(Stream stream)
        {
            var Temp = WorldCommon.ObjectIDLoad(stream);

            TrackID = Temp.TrackID;
            RID = Temp.RID;

            U1Count = StreamUtil.ReadUInt32(stream);
            U1Offset = StreamUtil.ReadUInt32(stream);
            U3 = StreamUtil.ReadUInt32(stream);
            U4 = StreamUtil.ReadUInt32(stream);

            U6 = StreamUtil.ReadFloat(stream);
            U7 = StreamUtil.ReadFloat(stream);
            U8 = StreamUtil.ReadFloat(stream);
            U9 = StreamUtil.ReadFloat(stream);

            ModelDataOffset = StreamUtil.ReadUInt32(stream);
            U11Count = StreamUtil.ReadUInt32(stream);

            U12 = new List<int>();

            for (int i = 0; i < U11Count; i++)
            {
                U12.Add(StreamUtil.ReadUInt32(stream));
            }

            ModelObjects = new List<ModelObject>();

            for (int i = 0; i < U1Count; i++)
            {
                ModelObject TempS1 = new ModelObject();

                TempS1.ParentID = StreamUtil.ReadUInt32(stream);
                TempS1.U1Offset = StreamUtil.ReadUInt32(stream);
                TempS1.U2Offset = StreamUtil.ReadUInt32(stream);
                TempS1.MatrixOffset = StreamUtil.ReadUInt32(stream);

                long TempPos = stream.Position;

                if (TempS1.U1Offset > 0)
                {
                    stream.Position = TempS1.U1Offset;

                    var TempS2 = new UnknownS2();
                    TempS2.BboxLow = StreamUtil.ReadVector3(stream);
                    TempS2.BboxHigh = StreamUtil.ReadVector3(stream);
                    TempS2.U0 = StreamUtil.ReadUInt32(stream);
                    TempS2.ArrayCount = StreamUtil.ReadUInt32(stream);
                    TempS2.ArrayOffset = StreamUtil.ReadUInt32(stream);

                    TempS2.ModelHeaderOffset = new List<ModelDataHeaderStruct>();
                    stream.Position = TempS2.ArrayOffset;
                    for (int a = 0; a < TempS2.ArrayCount; a++)
                    {
                        ModelDataHeaderStruct TempS5 = new ModelDataHeaderStruct();

                        TempS5.HeaderOffset = StreamUtil.ReadUInt32(stream);

                        long TempPos1 = stream.Position;

                        stream.Position = TempS5.HeaderOffset;

                        TempS5.U0 = StreamUtil.ReadInt16(stream);
                        TempS5.U1 = StreamUtil.ReadInt16(stream);
                        TempS5.ModelDataOffset = StreamUtil.ReadInt24(stream);
                        TempS5.U4 = StreamUtil.ReadInt8(stream);

                        TempS5.ModelOffsetHeaders = new List<ModelData>();

                        stream.Position = TempS5.ModelDataOffset + ModelDataOffset;

                        while (true)
                        {
                            ModelData TempS7 = new ModelData();

                            TempS7.LineCount = StreamUtil.ReadInt24(stream);
                            TempS7.U1 = StreamUtil.ReadUInt8(stream);
                            TempS7.ModelOffset = StreamUtil.ReadInt24(stream);
                            TempS7.U2 = StreamUtil.ReadUInt8(stream);
                            if (TempS7.U2 != 0)
                            {
                                Console.WriteLine(TempS7.U2 + " Detected");
                                return;
                            }
                            TempS5.ModelOffsetHeaders.Add(TempS7);

                            StreamUtil.AlignBy16(stream);

                            if (TempS7.U1 == 96)
                            {
                                break;
                            }
                        }
                        stream.Position = TempPos1;
                        TempS2.ModelHeaderOffset.Add(TempS5);
                    }

                    TempS1.unknownS2 = TempS2;
                }

                if (TempS1.U2Offset > 0)
                {
                    stream.Position = TempS1.U2Offset;
                    UnknownS3 TempS3 = new UnknownS3();
                    TempS3.U0 = StreamUtil.ReadVector3(stream);
                    TempS3.U1 = StreamUtil.ReadVector3(stream);

                    TempS3.U2 = StreamUtil.ReadUInt32(stream);
                    TempS3.U3 = StreamUtil.ReadUInt32(stream);
                    TempS3.U4 = StreamUtil.ReadUInt32(stream);
                    TempS3.U5 = StreamUtil.ReadUInt32(stream);

                    TempS3.U6 = StreamUtil.ReadUInt32(stream);

                    TempS1.unknownS3 = TempS3;
                }

                if (TempS1.MatrixOffset > 0)
                {
                    stream.Position = TempS1.MatrixOffset;

                    TempS1.matrix4X4 = StreamUtil.ReadMatrix4x4(stream);
                }

                stream.Position = TempPos;

                ModelObjects.Add(TempS1);
            }


            for (int i = 0; i < ModelObjects.Count; i++)
            {
                var S1 = ModelObjects[i];

                if (S1.U1Offset > 0)
                {
                    for (int a = 0; a < S1.unknownS2.ModelHeaderOffset.Count; a++)
                    {
                        var ModelHeader = S1.unknownS2.ModelHeaderOffset[a];
                        bool HeaderStart = false;
                        bool Normal = false;
                        for (int b = 0; b < ModelHeader.ModelOffsetHeaders.Count - 1; b++)
                        {
                            var ModelOffsetHeader = ModelHeader.ModelOffsetHeaders[b];

                            if (ModelOffsetHeader.ModelOffset > 0)
                            {
                                stream.Position = ModelOffsetHeader.ModelOffset + ModelDataOffset;

                                if (HeaderStart == false)
                                {
                                    ModelHeader.U00 = StreamUtil.ReadInt32(stream);
                                    ModelHeader.U01 = StreamUtil.ReadInt32(stream);
                                    ModelHeader.U02 = StreamUtil.ReadInt32(stream);
                                    ModelHeader.U03 = StreamUtil.ReadInt32(stream);

                                    ModelHeader.U04 = StreamUtil.ReadVector4(stream);
                                    HeaderStart = true;
                                }

                                if (!Normal)
                                {
                                    stream.Position += 32;
                                    ModelVandUVData TempModelData = new ModelVandUVData();

                                    TempModelData.TristripCount = StreamUtil.ReadUInt32(stream);

                                    stream.Position += 4;

                                    TempModelData.VerticesCount = StreamUtil.ReadUInt32(stream);

                                    stream.Position += 4 + 16;

                                    TempModelData.Tristrip = new List<int>();

                                    for (int c = 0; c < TempModelData.TristripCount; c++)
                                    {
                                        TempModelData.Tristrip.Add(StreamUtil.ReadUInt16(stream));
                                    }
                                    StreamUtil.AlignBy16(stream);
                                    stream.Position += 16;
                                    TempModelData.UV = new List<Vector2>();

                                    for (int c = 0; c < TempModelData.VerticesCount; c++)
                                    {
                                        Vector2 vector3 = new Vector2();

                                        vector3.X = StreamUtil.ReadInt16(stream) / 32767f;
                                        vector3.Y = StreamUtil.ReadInt16(stream) / 32767f;

                                        TempModelData.UV.Add(vector3);
                                    }
                                    StreamUtil.AlignBy16(stream);

                                    stream.Position += 16;

                                    TempModelData.Vertices = new List<Vector3>();

                                    for (int c = 0; c < TempModelData.VerticesCount; c++)
                                    {
                                        Vector3 vector3 = new Vector3();

                                        vector3.X = StreamUtil.ReadInt16(stream) / 16f;
                                        vector3.Y = StreamUtil.ReadInt16(stream) / 16f;
                                        vector3.Z = StreamUtil.ReadInt16(stream) / 16f;

                                        TempModelData.Vertices.Add(vector3);
                                    }
                                    StreamUtil.AlignBy16(stream);

                                    ModelOffsetHeader.modelVandUVData = TempModelData;
                                }
                                else
                                {
                                    ModelNormalData TempModelData = new ModelNormalData();

                                    stream.Position += 14;

                                    TempModelData.NormalCount = StreamUtil.ReadUInt8(stream);

                                    stream.Position += 1;

                                    TempModelData.Normals = new List<Vector3>();

                                    for (int c = 0; c < TempModelData.NormalCount; c++)
                                    {
                                        Vector3 vector3 = new Vector3();

                                        vector3.X = StreamUtil.ReadInt16(stream) / 32767f;
                                        vector3.Y = StreamUtil.ReadInt16(stream) / 32767f;
                                        vector3.Z = StreamUtil.ReadInt16(stream) / 32767f;

                                        TempModelData.Normals.Add(vector3);
                                    }
                                    StreamUtil.AlignBy16(stream);

                                    ModelOffsetHeader.modelNormalData = TempModelData;
                                }

                                Normal = !Normal;
                            }

                            ModelHeader.ModelOffsetHeaders[b] = ModelOffsetHeader;
                        }

                        S1.unknownS2.ModelHeaderOffset[a] = ModelHeader;
                    }
                }

                ModelObjects[i] = S1;
            }
            ConvertToModelFaces();
        }

        public void ConvertToModelFaces()
        {
            for (int i = 0; i < ModelObjects.Count; i++)
            {
                var S1 = ModelObjects[i];
                S1.modelFaces = new List<ModelFace>();

                if (S1.U1Offset > 0)
                {
                    for (int a = 0; a < S1.unknownS2.ModelHeaderOffset.Count; a++)
                    {
                        var S5 = S1.unknownS2.ModelHeaderOffset[a];
                        Rotation = false;
                        for (int b = 0; b < S5.ModelOffsetHeaders.Count / 2; b++)
                        {
                            int VerticesUVID = b * 2;
                            int NormalID = b * 2 + 1;

                            var VerticesUV = S5.ModelOffsetHeaders[VerticesUVID];
                            var Normal = S5.ModelOffsetHeaders[NormalID];

                            S1.modelFaces.AddRange(GenerateFaces(VerticesUV.modelVandUVData, Normal.modelNormalData));

                        }

                        S1.unknownS2.ModelHeaderOffset[a] = S5;
                    }
                }

                ModelObjects[i] = S1;
            }


        }
        bool Rotation = false;
        public List<ModelFace> GenerateFaces(ModelVandUVData ModelData, ModelNormalData modelNormalData)
        {
            //Increment Strips
            List<int> strip2 = new List<int>();
            strip2.Add(0);
            foreach (var item in ModelData.Tristrip)
            {
                strip2.Add(strip2[strip2.Count - 1] + item);
            }

            //Make Faces
            List<ModelFace> faces = new List<ModelFace>();
            int localIndex = 0;
            bool Rotation = false;
            for (int b = 0; b < ModelData.Vertices.Count; b++)
            {
                if (InsideSplits(b, strip2))
                {
                    Rotation = false;
                    localIndex = 1;
                    continue;
                }
                if (localIndex < 2)
                {
                    localIndex++;
                    continue;
                }

                faces.Add(CreateFaces(b, ModelData, modelNormalData, Rotation));
                Rotation = !Rotation;
                localIndex++;
            }

            return faces;
        }
        public bool InsideSplits(int Number, List<int> splits)
        {
            foreach (var item in splits)
            {
                if (item == Number)
                {
                    return true;
                }
            }
            return false;
        }
        public ModelFace CreateFaces(int Index, ModelVandUVData ModelData, ModelNormalData modelNormalData, bool roatation)
        {
            ModelFace face = new ModelFace();
            int Index1 = 0;
            int Index2 = 0;
            int Index3 = 0;
            //Fixes the Rotation For Exporting
            //Swap When Exporting to other formats
            //1-Clockwise
            //0-Counter Clocwise
            if (roatation)
            {
                Index1 = Index;
                Index2 = Index - 1;
                Index3 = Index - 2;
            }
            if (!roatation)
            {
                Index1 = Index - 2;
                Index2 = Index - 1;
                Index3 = Index;
            }
            face.V1 = ModelData.Vertices[Index1];
            face.V2 = ModelData.Vertices[Index2];
            face.V3 = ModelData.Vertices[Index3];

            //face.V1Pos = Index1;
            //face.V2Pos = Index2;
            //face.V3Pos = Index3;

            face.UV1 = ModelData.UV[Index1];
            face.UV2 = ModelData.UV[Index2];
            face.UV3 = ModelData.UV[Index3];

            //face.UV1Pos = Index1;
            //face.UV2Pos = Index2;
            //face.UV3Pos = Index3;

            face.Normal1 = modelNormalData.Normals[Index1];
            face.Normal2 = modelNormalData.Normals[Index2];
            face.Normal3 = modelNormalData.Normals[Index3];

            //face.Normal1Pos = Index1;
            //face.Normal2Pos = Index2;
            //face.Normal3Pos = Index3;



            return face;
        }

        //public void SaveModelGLB(string Path)
        //{
        //    var scene = new SharpGLTF.Scenes.SceneBuilder();

        //    var material1 = new MaterialBuilder("TempMat")
        //    .WithChannelParam(KnownChannel.BaseColor, KnownProperty.RGBA, new Vector4(1, 1, 1, 1));

        //    List<MeshBuilder<VertexPositionNormal, VertexTexture1>> MeshList = new List<MeshBuilder<VertexPositionNormal, VertexTexture1>>();

        //    for (int i = 0; i < ModelObjects.Count; i++)
        //    {
        //        var s1 = ModelObjects[i];
        //        var mesh = new MeshBuilder<VertexPositionNormal, VertexTexture1>(i.ToString());

        //        if (s1.modelFaces == null)
        //        {
        //            return;
        //        }

        //        for (int a = 0; a < s1.modelFaces.Count; a++)
        //        {
        //            var Face = s1.modelFaces[a];
        //            VertexPositionNormal TempPos1 = new VertexPositionNormal();
        //            TempPos1.Position = Face.V1;
        //            TempPos1.Normal = Face.Normal1;

        //            VertexPositionNormal TempPos2 = new VertexPositionNormal();
        //            TempPos2.Position = Face.V2;
        //            TempPos2.Normal = Face.Normal2;

        //            VertexPositionNormal TempPos3 = new VertexPositionNormal();
        //            TempPos3.Position = Face.V3;
        //            TempPos3.Normal = Face.Normal3;

        //            VertexTexture1 TempTexture1 = new VertexTexture1();
        //            TempTexture1.TexCoord = Face.UV1;

        //            VertexTexture1 TempTexture2 = new VertexTexture1();
        //            TempTexture2.TexCoord = Face.UV2;

        //            VertexTexture1 TempTexture3 = new VertexTexture1();
        //            TempTexture3.TexCoord = Face.UV3;

        //            mesh.UsePrimitive(material1).AddTriangle((TempPos1, TempTexture1), (TempPos2, TempTexture2), (TempPos3, TempTexture3));
        //        }

        //        if (ModelObjects[i].ParentID == -1)
        //        {
        //            if (ModelObjects[i].MatrixOffset > 0)
        //            {
        //                scene.AddRigidMesh(mesh, ModelObjects[i].matrix4X4);
        //            }
        //            else
        //            {
        //                scene.AddRigidMesh(mesh, Matrix4x4.CreateFromQuaternion(new Quaternion(0, 0, 0, 0)));
        //            }
        //        }
        //        else
        //        {
        //            if (ModelObjects[i].MatrixOffset > 0)
        //            {
        //                MeshList[ModelObjects[i].ParentID].AddMesh(mesh, ModelObjects[i].matrix4X4);
        //            }
        //            else
        //            {
        //                MeshList[ModelObjects[i].ParentID].AddMesh(mesh, Matrix4x4.CreateFromQuaternion(new Quaternion(0, 0, 0, 0)));
        //            }
        //        }
        //        MeshList.Add(mesh);
        //    }

        //    var model = scene.ToGltf2();
        //    //model.ApplyBasisTransform(matrix4X4);
        //    model.SaveGLB(Path);
        //}

        public MDRJsonHandler.MainModelHeader SaveModel(string Path)
        {
            MDRJsonHandler.MainModelHeader JsonModelObject = new MDRJsonHandler.MainModelHeader();

            //Convert to JSON
            JsonModelObject.TrackID = TrackID;
            JsonModelObject.RID = RID;

            JsonModelObject.U3 = U3;
            JsonModelObject.U4 = U4;

            JsonModelObject.U6 = U6;
            JsonModelObject.U7 = U7;
            JsonModelObject.U8 = U8;
            JsonModelObject.U9 = U9;

            JsonModelObject.U12 = U12;

            JsonModelObject.ModelObjects = new List<MDRJsonHandler.ModelObject>();

            for (int i = 0; i < ModelObjects.Count; i++)
            {
                var modelObject = ModelObjects[i];
                var NewObject = new MDRJsonHandler.ModelObject();

                NewObject.ParentID = modelObject.ParentID;

                Vector3 Scale;
                Quaternion Rotation;
                Vector3 Location;

                Matrix4x4.Decompose(modelObject.matrix4X4, out Scale, out Rotation, out Location);
                NewObject.Position = JsonUtil.Vector3ToArray(Location);
                NewObject.Rotation = JsonUtil.QuaternionToArray(Rotation);
                NewObject.Scale = JsonUtil.Vector3ToArray(Scale);

                NewObject.unknownS2 = new MDRJsonHandler.UnknownS2();

                NewObject.unknownS2.BboxLow = JsonUtil.Vector3ToArray(modelObject.unknownS2.BboxLow);
                NewObject.unknownS2.BboxHigh = JsonUtil.Vector3ToArray(modelObject.unknownS2.BboxHigh);

                NewObject.unknownS2.U0 = modelObject.unknownS2.U0;

                if (modelObject.modelFaces != null)
                {
                    if (modelObject.modelFaces.Count != 0)
                    {
                        NewObject.ModelPath = RID + "-" + i + ".obj";
                    }
                }

                NewObject.unknownS2.ModelHeaderOffset = new List<MDRJsonHandler.ModelDataHeaderStruct>();

                if (modelObject.unknownS2.ModelHeaderOffset != null)
                {
                    for (global::System.Int32 j = 0; j < modelObject.unknownS2.ModelHeaderOffset.Count; j++)
                    {
                        var TempModelOffset = new MDRJsonHandler.ModelDataHeaderStruct();

                        TempModelOffset.U0 = modelObject.unknownS2.ModelHeaderOffset[j].U0;
                        TempModelOffset.U1 = modelObject.unknownS2.ModelHeaderOffset[j].U1;
                        TempModelOffset.U4 = modelObject.unknownS2.ModelHeaderOffset[j].U4;

                        TempModelOffset.U00 = modelObject.unknownS2.ModelHeaderOffset[j].U00;
                        TempModelOffset.U01 = modelObject.unknownS2.ModelHeaderOffset[j].U01;
                        TempModelOffset.U02 = modelObject.unknownS2.ModelHeaderOffset[j].U02;
                        TempModelOffset.U03 = modelObject.unknownS2.ModelHeaderOffset[j].U03;

                        TempModelOffset.U04 = JsonUtil.Vector4ToArray(modelObject.unknownS2.ModelHeaderOffset[j].U04);

                        NewObject.unknownS2.ModelHeaderOffset.Add(TempModelOffset);
                    }
                }

                NewObject.unknownS3 = new MDRJsonHandler.UnknownS3();

                NewObject.unknownS3.U0 = JsonUtil.Vector3ToArray(modelObject.unknownS3.U0);
                NewObject.unknownS3.U1 = JsonUtil.Vector3ToArray(modelObject.unknownS3.U1);

                NewObject.unknownS3.U2 = modelObject.unknownS3.U2;
                NewObject.unknownS3.U3 = modelObject.unknownS3.U3;
                NewObject.unknownS3.U4 = modelObject.unknownS3.U4;
                NewObject.unknownS3.U5 = modelObject.unknownS3.U5;
                NewObject.unknownS3.U6 = modelObject.unknownS3.U6;

                JsonModelObject.ModelObjects.Add(NewObject);
            }

            //Save OBJs
            //glstHandler.SavePDBModelglTF(path, this);
            for (int a = 0; a < ModelObjects.Count; a++)
            {
                string outputString = "";
                string output = "# Exported From SSX Using SSX Multitool Modder by GlitcherOG \n";

                List<Vector3> vertices = new List<Vector3>();
                List<Vector3> Normals = new List<Vector3>();
                List<Vector2> UV = new List<Vector2>();
                outputString += "o Mesh" + a.ToString() + "\n";
                var Data = ModelObjects[a];

                if (Data.modelFaces != null)
                {

                    for (int b = 0; b < Data.modelFaces.Count; b++)
                    {
                        var Face = Data.modelFaces[b];

                        //Vertices
                        if (!vertices.Contains(Face.V1))
                        {
                            vertices.Add(Face.V1);
                        }
                        int VPos1 = vertices.IndexOf(Face.V1) + 1;

                        if (!vertices.Contains(Face.V2))
                        {
                            vertices.Add(Face.V2);
                        }
                        int VPos2 = vertices.IndexOf(Face.V2) + 1;

                        if (!vertices.Contains(Face.V3))
                        {
                            vertices.Add(Face.V3);
                        }
                        int VPos3 = vertices.IndexOf(Face.V3) + 1;

                        //UVs
                        if (!UV.Contains(Face.UV1))
                        {
                            UV.Add(Face.UV1);
                        }
                        int UPos1 = UV.IndexOf(Face.UV1) + 1;

                        if (!UV.Contains(Face.UV2))
                        {
                            UV.Add(Face.UV2);
                        }
                        int UPos2 = UV.IndexOf(Face.UV2) + 1;

                        if (!UV.Contains(Face.UV3))
                        {
                            UV.Add(Face.UV3);
                        }
                        int UPos3 = UV.IndexOf(Face.UV3) + 1;

                        //Normals
                        if (!Normals.Contains(Face.Normal1))
                        {
                            Normals.Add(Face.Normal1);
                        }
                        int NPos1 = Normals.IndexOf(Face.Normal1) + 1;

                        if (!Normals.Contains(Face.Normal2))
                        {
                            Normals.Add(Face.Normal2);
                        }
                        int NPos2 = Normals.IndexOf(Face.Normal2) + 1;

                        if (!Normals.Contains(Face.Normal3))
                        {
                            Normals.Add(Face.Normal3);
                        }
                        int NPos3 = Normals.IndexOf(Face.Normal3) + 1;

                        outputString += "f " + VPos1.ToString() + "/" + UPos1.ToString() + "/" + NPos1.ToString() + " " + VPos2.ToString() + "/" + UPos2.ToString() + "/" + NPos2.ToString() + " " + VPos3.ToString() + "/" + UPos3.ToString() + "/" + NPos3.ToString() + "\n";
                    }

                    for (int z = 0; z < vertices.Count; z++)
                    {
                        output += "v " + vertices[z].X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + vertices[z].Y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + vertices[z].Z.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\n";
                    }
                    for (int z = 0; z < UV.Count; z++)
                    {
                        output += "vt " + UV[z].X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + (1 - UV[z].Y).ToString(CultureInfo.InvariantCulture.NumberFormat) + "\n";
                    }
                    for (int z = 0; z < Normals.Count; z++)
                    {
                        output += "vn " + Normals[z].X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Normals[z].Y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Normals[z].Z.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\n";
                    }
                    output += outputString;

                    if (Data.modelFaces.Count != 0)
                    {
                        File.WriteAllText(Path + "/" + RID + "-" + a + ".obj", output);
                    }
                }
                //    if (ModelObjects[a].unknownS2.ModelHeaderOffset[ax].model != null)
                //    {
                //        for (int i = 0; i < ModelObjects[a].PrefabObjects[ax].objectData.MeshOffsets.Count; i++)
                //        {
                //            string outputString = "";
                //            string output = "# Exported From SSX Using SSX Multitool Modder by GlitcherOG \n";

                //            List<Vector3> vertices = new List<Vector3>();
                //            List<Vector3> Normals = new List<Vector3>();
                //            List<Vector2> UV = new List<Vector2>();
                //            outputString += "o Mesh" + i.ToString() + "\n";
                //            var Data = ModelObjects[a].PrefabObjects[ax].objectData.MeshOffsets[i].FullMesh;
                //            for (int b = 0; b < Data.meshFaces.Count; b++)
                //            {
                //                var Face = Data.meshFaces[b];

                //                //Vertices
                //                if (!vertices.Contains(Face.V1))
                //                {
                //                    vertices.Add(Face.V1);
                //                }
                //                int VPos1 = vertices.IndexOf(Face.V1) + 1;

                //                if (!vertices.Contains(Face.V2))
                //                {
                //                    vertices.Add(Face.V2);
                //                }
                //                int VPos2 = vertices.IndexOf(Face.V2) + 1;

                //                if (!vertices.Contains(Face.V3))
                //                {
                //                    vertices.Add(Face.V3);
                //                }
                //                int VPos3 = vertices.IndexOf(Face.V3) + 1;

                //                //UVs
                //                if (!UV.Contains(Face.UV1))
                //                {
                //                    UV.Add(Face.UV1);
                //                }
                //                int UPos1 = UV.IndexOf(Face.UV1) + 1;

                //                if (!UV.Contains(Face.UV2))
                //                {
                //                    UV.Add(Face.UV2);
                //                }
                //                int UPos2 = UV.IndexOf(Face.UV2) + 1;

                //                if (!UV.Contains(Face.UV3))
                //                {
                //                    UV.Add(Face.UV3);
                //                }
                //                int UPos3 = UV.IndexOf(Face.UV3) + 1;

                //                //Normals
                //                if (!Normals.Contains(Face.Normal1))
                //                {
                //                    Normals.Add(Face.Normal1);
                //                }
                //                int NPos1 = Normals.IndexOf(Face.Normal1) + 1;

                //                if (!Normals.Contains(Face.Normal2))
                //                {
                //                    Normals.Add(Face.Normal2);
                //                }
                //                int NPos2 = Normals.IndexOf(Face.Normal2) + 1;

                //                if (!Normals.Contains(Face.Normal3))
                //                {
                //                    Normals.Add(Face.Normal3);
                //                }
                //                int NPos3 = Normals.IndexOf(Face.Normal3) + 1;

                //                outputString += "f " + VPos1.ToString() + "/" + UPos1.ToString() + "/" + NPos1.ToString() + " " + VPos2.ToString() + "/" + UPos2.ToString() + "/" + NPos2.ToString() + " " + VPos3.ToString() + "/" + UPos3.ToString() + "/" + NPos3.ToString() + "\n";
                //            }

                //            for (int z = 0; z < vertices.Count; z++)
                //            {
                //                output += "v " + vertices[z].X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + vertices[z].Y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + vertices[z].Z.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\n";
                //            }
                //            for (int z = 0; z < UV.Count; z++)
                //            {
                //                output += "vt " + UV[z].X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + (1 - UV[z].Y).ToString(CultureInfo.InvariantCulture.NumberFormat) + "\n";
                //            }
                //            for (int z = 0; z < Normals.Count; z++)
                //            {
                //                output += "vn " + Normals[z].X.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Normals[z].Y.ToString(CultureInfo.InvariantCulture.NumberFormat) + " " + Normals[z].Z.ToString(CultureInfo.InvariantCulture.NumberFormat) + "\n";
                //            }
                //            output += outputString;
                //            File.AppendAllText(path + "/" + PrefabData[a].PrefabObjects[ax].objectData.MeshOffsets[i].MeshID.ToString() + ".obj", output);
                //        }
                //    }

            }


            //Return Model

            return JsonModelObject;
        }

        public struct ModelObject
        {
            public int ParentID;
            public int U1Offset;
            public int U2Offset;
            public int MatrixOffset;

            public Matrix4x4 matrix4X4;
            public UnknownS2 unknownS2;
            public UnknownS3 unknownS3;

            public List<ModelFace> modelFaces;
        }

        public struct UnknownS2
        {
            public Vector3 BboxLow;
            public Vector3 BboxHigh;
            public int U0;
            public int ArrayCount;
            public int ArrayOffset;

            public List<ModelDataHeaderStruct> ModelHeaderOffset;
        }

        public struct UnknownS3
        {
            public Vector3 U0;
            public Vector3 U1; //?

            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
        }

        public struct ModelDataHeaderStruct
        {
            public int HeaderOffset;

            public int U0;
            public int U1;
            public int ModelDataOffset;
            public int U4;

            //Main Header Object
            public int U00;
            public int U01;
            public int U02;
            public int U03;

            public Vector4 U04;

            public string ModelPath;

            public List<ModelData> ModelOffsetHeaders;
        }

        public struct ModelData
        {
            public int LineCount;
            public int U1;
            public int ModelOffset;
            public int U2;

            //Standard Models
            public ModelVandUVData modelVandUVData;
            public ModelNormalData modelNormalData;

            //Non-Standard
        }

        public struct ModelVandUVData
        {
            public int TristripCount;
            public int VerticesCount;

            public List<int> Tristrip;
            public List<Vector3> Vertices;
            public List<Vector2> UV;
        }

        public struct ModelNormalData
        {
            public int NormalCount;

            public List<Vector3> Normals;
        }

        public struct ModelFace
        {
            public Vector3 V1;
            public Vector3 V2;
            public Vector3 V3;

            public Vector2 UV1;
            public Vector2 UV2;
            public Vector2 UV3;

            public Vector3 Normal1;
            public Vector3 Normal2;
            public Vector3 Normal3;
        }
    }
}
