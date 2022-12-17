using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using SSXMultiTool.Utilities;
using SSXMultiTool.FileHandlers;
using SSXMultiTool.FileHandlers.Models;

namespace SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2
{
    public class PBDHandler
    {
        public byte[] MagicBytes;
        public int NumPlayerStarts;
        public int NumPatches;
        public int NumInstances;
        public int NumParticleInstances;
        public int NumMaterials;
        public int NumMaterialBlocks;
        public int NumLights;
        public int NumSplines;
        public int NumSplineSegments;
        public int NumTextureFlipbooks;
        public int NumModels;
        public int NumParticleModel;
        public int NumTextures;
        public int NumCameras;
        public int LightMapSize;

        public int PlayerStartOffset;
        public int PatchOffset;
        public int InstanceOffset;
        public int ParticleInstancesOffset;
        public int MaterialOffset;
        public int MaterialBlocksOffset;
        public int LightsOffset;
        public int SplineOffset;
        public int SplineSegmentOffset;
        public int TextureFlipbookOffset;
        public int ModelPointerOffset;
        public int ModelsOffset;
        public int ParticleModelPointerOffset;
        public int ParticleModelsOffset;
        public int CameraPointerOffset;
        public int CamerasOffset;
        public int HashOffset;
        public int MeshDataOffset;

        public List<Patch> Patches;
        public List<Spline> splines;
        public List<SplinesSegments> splinesSegments;
        public List<TextureFlipbook> textureFlipbooks;
        public List<Instance> Instances;
        public List<ParticleInstance> particleInstances;
        public List<TrickyMaterial> materials;
        public List<MaterialBlock> materialBlocks;
        public List<Light> lights;
        public List<int> PrefabPointers;
        public List<Prefabs> PrefabData;
        public List<int> ParticleModelPointers;
        public List<ParticleModel> particleModels;
        public List<int> CameraPointers;
        public List<Camera> Cameras = new List<Camera>();
        public HashData hashData = new HashData();

        public void LoadPBD(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                MagicBytes = StreamUtil.ReadBytes(stream, 4);
                NumPlayerStarts = StreamUtil.ReadInt32(stream); //NA
                NumPatches = StreamUtil.ReadInt32(stream); //Done
                NumInstances = StreamUtil.ReadInt32(stream); //Done
                NumParticleInstances = StreamUtil.ReadInt32(stream); //Done
                NumMaterials = StreamUtil.ReadInt32(stream); //Done
                NumMaterialBlocks = StreamUtil.ReadInt32(stream); //Done
                NumLights = StreamUtil.ReadInt32(stream); //Done
                NumSplines = StreamUtil.ReadInt32(stream); //Done
                NumSplineSegments = StreamUtil.ReadInt32(stream); //Done
                NumTextureFlipbooks = StreamUtil.ReadInt32(stream); //Done
                NumModels = StreamUtil.ReadInt32(stream); //Done
                NumParticleModel = StreamUtil.ReadInt32(stream); //Done
                NumTextures = StreamUtil.ReadInt32(stream); //Done
                NumCameras = StreamUtil.ReadInt32(stream); //Used in SSXFE MAP
                LightMapSize = StreamUtil.ReadInt32(stream); //Always blank?

                PlayerStartOffset = StreamUtil.ReadInt32(stream); //NA
                PatchOffset = StreamUtil.ReadInt32(stream); //Done
                InstanceOffset = StreamUtil.ReadInt32(stream); //Done
                ParticleInstancesOffset = StreamUtil.ReadInt32(stream); //Done
                MaterialOffset = StreamUtil.ReadInt32(stream); //Done
                MaterialBlocksOffset = StreamUtil.ReadInt32(stream); //Done
                LightsOffset = StreamUtil.ReadInt32(stream); //Done 
                SplineOffset = StreamUtil.ReadInt32(stream); //Done
                SplineSegmentOffset = StreamUtil.ReadInt32(stream); //Done
                TextureFlipbookOffset = StreamUtil.ReadInt32(stream); //Done
                ModelPointerOffset = StreamUtil.ReadInt32(stream); //Done
                ModelsOffset = StreamUtil.ReadInt32(stream); //Done
                ParticleModelPointerOffset = StreamUtil.ReadInt32(stream); //Done
                ParticleModelsOffset = StreamUtil.ReadInt32(stream); //Sort of Loading
                CameraPointerOffset = StreamUtil.ReadInt32(stream); //Done
                CamerasOffset = StreamUtil.ReadInt32(stream); //sort of loading
                HashOffset = StreamUtil.ReadInt32(stream); //sort of loading
                MeshDataOffset = StreamUtil.ReadInt32(stream); //Loading

                //Patch Loading
                stream.Position = PatchOffset;
                Patches = new List<Patch>();
                for (int i = 0; i < NumPatches; i++)
                {
                    Patch patch = new Patch();

                    patch.LightMapPoint = StreamUtil.ReadVector4(stream);

                    patch.UVPoint1 = StreamUtil.ReadVector4(stream);
                    patch.UVPoint2 = StreamUtil.ReadVector4(stream);
                    patch.UVPoint3 = StreamUtil.ReadVector4(stream);
                    patch.UVPoint4 = StreamUtil.ReadVector4(stream);

                    patch.R4C4 = StreamUtil.ReadVector4(stream);
                    patch.R4C3 = StreamUtil.ReadVector4(stream);
                    patch.R4C2 = StreamUtil.ReadVector4(stream);
                    patch.R4C1 = StreamUtil.ReadVector4(stream);
                    patch.R3C4 = StreamUtil.ReadVector4(stream);
                    patch.R3C3 = StreamUtil.ReadVector4(stream);
                    patch.R3C2 = StreamUtil.ReadVector4(stream);
                    patch.R3C1 = StreamUtil.ReadVector4(stream);
                    patch.R2C4 = StreamUtil.ReadVector4(stream);
                    patch.R2C3 = StreamUtil.ReadVector4(stream);
                    patch.R2C2 = StreamUtil.ReadVector4(stream);
                    patch.R2C1 = StreamUtil.ReadVector4(stream);
                    patch.R1C4 = StreamUtil.ReadVector4(stream);
                    patch.R1C3 = StreamUtil.ReadVector4(stream);
                    patch.R1C2 = StreamUtil.ReadVector4(stream);
                    patch.R1C1 = StreamUtil.ReadVector4(stream);

                    patch.LowestXYZ = StreamUtil.ReadVector3(stream);
                    patch.HighestXYZ = StreamUtil.ReadVector3(stream);

                    patch.Point1 = StreamUtil.ReadVector4(stream);
                    patch.Point2 = StreamUtil.ReadVector4(stream);
                    patch.Point3 = StreamUtil.ReadVector4(stream);
                    patch.Point4 = StreamUtil.ReadVector4(stream);

                    patch.PatchStyle = StreamUtil.ReadInt32(stream);
                    patch.Unknown2 = StreamUtil.ReadInt32(stream); //Material/Lighting
                    patch.TextureAssigment = StreamUtil.ReadInt16(stream);

                    patch.LightmapID = StreamUtil.ReadInt16(stream);

                    //Always the same
                    patch.Unknown4 = StreamUtil.ReadInt32(stream); //Negitive one
                    patch.Unknown5 = StreamUtil.ReadInt32(stream);
                    patch.Unknown6 = StreamUtil.ReadInt32(stream);

                    Patches.Add(patch);
                }

                stream.Position = InstanceOffset;
                Instances = new List<Instance>();
                for (int i = 0; i < NumInstances; i++)
                {
                    var TempInstance = new Instance();
                    TempInstance.MatrixCol1 = StreamUtil.ReadVector4(stream);
                    TempInstance.MatrixCol2 = StreamUtil.ReadVector4(stream);
                    TempInstance.MatrixCol3 = StreamUtil.ReadVector4(stream);
                    TempInstance.InstancePosition = StreamUtil.ReadVector4(stream);
                    TempInstance.Unknown5 = StreamUtil.ReadVector4(stream);
                    TempInstance.Unknown6 = StreamUtil.ReadVector4(stream);
                    TempInstance.Unknown7 = StreamUtil.ReadVector4(stream);
                    TempInstance.Unknown8 = StreamUtil.ReadVector4(stream);
                    TempInstance.Unknown9 = StreamUtil.ReadVector4(stream);
                    TempInstance.Unknown10 = StreamUtil.ReadVector4(stream);
                    TempInstance.Unknown11 = StreamUtil.ReadVector4(stream);
                    TempInstance.RGBA = StreamUtil.ReadVector4(stream);
                    TempInstance.ModelID = StreamUtil.ReadInt32(stream);
                    TempInstance.PrevInstance = StreamUtil.ReadInt32(stream);
                    TempInstance.NextInstance = StreamUtil.ReadInt32(stream);

                    TempInstance.LowestXYZ = StreamUtil.ReadVector3(stream);
                    TempInstance.HighestXYZ = StreamUtil.ReadVector3(stream);

                    TempInstance.UnknownInt26 = StreamUtil.ReadInt32(stream);
                    TempInstance.UnknownInt27 = StreamUtil.ReadInt32(stream);
                    TempInstance.UnknownInt28 = StreamUtil.ReadInt32(stream);
                    TempInstance.ModelID2 = StreamUtil.ReadInt32(stream);
                    TempInstance.UnknownInt30 = StreamUtil.ReadInt32(stream);
                    TempInstance.UnknownInt31 = StreamUtil.ReadInt32(stream);
                    TempInstance.UnknownInt32 = StreamUtil.ReadInt32(stream);
                    Instances.Add(TempInstance);
                }

                stream.Position = ParticleInstancesOffset;
                particleInstances = new List<ParticleInstance>();
                for (int i = 0; i < NumParticleInstances; i++)
                {
                    ParticleInstance TempParticle = new ParticleInstance();
                    TempParticle.MatrixCol1 = StreamUtil.ReadVector4(stream);
                    TempParticle.MatrixCol2 = StreamUtil.ReadVector4(stream);
                    TempParticle.MatrixCol3 = StreamUtil.ReadVector4(stream);
                    TempParticle.ParticleInstancePosition = StreamUtil.ReadVector4(stream);
                    TempParticle.UnknownInt1 = StreamUtil.ReadInt32(stream);
                    TempParticle.LowestXYZ = StreamUtil.ReadVector3(stream);
                    TempParticle.HighestXYZ = StreamUtil.ReadVector3(stream);
                    TempParticle.UnknownInt8 = StreamUtil.ReadInt32(stream);
                    TempParticle.UnknownInt9 = StreamUtil.ReadInt32(stream);
                    TempParticle.UnknownInt10 = StreamUtil.ReadInt32(stream);
                    TempParticle.UnknownInt11 = StreamUtil.ReadInt32(stream);
                    TempParticle.UnknownInt12 = StreamUtil.ReadInt32(stream);
                    particleInstances.Add(TempParticle);
                }

                stream.Position = MaterialOffset;
                materials = new List<TrickyMaterial>();
                for (int i = 0; i < NumMaterials; i++)
                {
                    var TempMaterial = new TrickyMaterial();
                    TempMaterial.TextureID = StreamUtil.ReadInt16(stream);
                    TempMaterial.UnknownInt2 = StreamUtil.ReadInt16(stream);
                    TempMaterial.UnknownInt3 = StreamUtil.ReadInt32(stream);

                    TempMaterial.UnknownFloat1 = StreamUtil.ReadFloat(stream);
                    TempMaterial.UnknownFloat2 = StreamUtil.ReadFloat(stream);
                    TempMaterial.UnknownFloat3 = StreamUtil.ReadFloat(stream);
                    TempMaterial.UnknownFloat4 = StreamUtil.ReadFloat(stream);

                    TempMaterial.UnknownInt8 = StreamUtil.ReadInt32(stream);

                    TempMaterial.UnknownFloat5 = StreamUtil.ReadFloat(stream);
                    TempMaterial.UnknownFloat6 = StreamUtil.ReadFloat(stream);
                    TempMaterial.UnknownFloat7 = StreamUtil.ReadFloat(stream);
                    TempMaterial.UnknownFloat8 = StreamUtil.ReadFloat(stream);

                    TempMaterial.UnknownInt13 = StreamUtil.ReadInt32(stream);
                    TempMaterial.UnknownInt14 = StreamUtil.ReadInt32(stream);
                    TempMaterial.UnknownInt15 = StreamUtil.ReadInt32(stream);
                    TempMaterial.UnknownInt16 = StreamUtil.ReadInt32(stream);
                    TempMaterial.UnknownInt17 = StreamUtil.ReadInt32(stream);
                    TempMaterial.UnknownInt18 = StreamUtil.ReadInt32(stream);

                    TempMaterial.TextureFlipbookID = StreamUtil.ReadInt16(stream);
                    TempMaterial.UnknownInt20 = StreamUtil.ReadInt16(stream);

                    materials.Add(TempMaterial);
                }

                stream.Position = MaterialBlocksOffset;
                materialBlocks = new List<MaterialBlock>();
                for (int i = 0; i < NumMaterialBlocks; i++)
                {
                    var TempMaterialBlock = new MaterialBlock();
                    TempMaterialBlock.BlockCount = StreamUtil.ReadInt32(stream);
                    TempMaterialBlock.ints = new List<int>();
                    for (int a = 0; a < TempMaterialBlock.BlockCount; a++)
                    {
                        TempMaterialBlock.ints.Add(StreamUtil.ReadInt32(stream));
                    }
                    materialBlocks.Add(TempMaterialBlock);
                }

                stream.Position = LightsOffset;
                lights = new List<Light>();
                for (int i = 0; i < NumLights; i++)
                {
                    var TempLights = new Light();
                    TempLights.Type = StreamUtil.ReadInt32(stream);
                    TempLights.spriteRes = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownFloat1 = StreamUtil.ReadFloat(stream);
                    TempLights.UnknownInt1 = StreamUtil.ReadInt32(stream);
                    TempLights.Colour = StreamUtil.ReadVector3(stream);
                    TempLights.Direction = StreamUtil.ReadVector3(stream);
                    TempLights.Postion = StreamUtil.ReadVector3(stream);
                    TempLights.LowestXYZ = StreamUtil.ReadVector3(stream);
                    TempLights.HighestXYZ = StreamUtil.ReadVector3(stream);
                    TempLights.UnknownFloat2 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt2 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownFloat3 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt3 = StreamUtil.ReadInt32(stream);
                    lights.Add(TempLights);
                }

                //Spline Data
                stream.Position = SplineOffset;
                splines = new List<Spline>();
                for (int i = 0; i < NumSplines; i++)
                {
                    Spline spline = new Spline();
                    spline.LowestXYZ = StreamUtil.ReadVector3(stream);
                    spline.HighestXYZ = StreamUtil.ReadVector3(stream);
                    spline.Unknown1 = StreamUtil.ReadInt32(stream);
                    spline.SplineSegmentCount = StreamUtil.ReadInt32(stream);
                    spline.SplineSegmentPosition = StreamUtil.ReadInt32(stream);
                    spline.Unknown2 = StreamUtil.ReadInt32(stream);
                    splines.Add(spline);
                }

                //Spline Segments
                stream.Position = SplineSegmentOffset;
                splinesSegments = new List<SplinesSegments>();
                for (int i = 0; i < NumSplineSegments; i++)
                {
                    SplinesSegments splinesSegment = new SplinesSegments();

                    splinesSegment.Point4 = StreamUtil.ReadVector4(stream);
                    splinesSegment.Point3 = StreamUtil.ReadVector4(stream);
                    splinesSegment.Point2 = StreamUtil.ReadVector4(stream);
                    splinesSegment.ControlPoint = StreamUtil.ReadVector4(stream);

                    splinesSegment.ScalingPoint = StreamUtil.ReadVector4(stream);

                    splinesSegment.PreviousSegment = StreamUtil.ReadInt32(stream);
                    splinesSegment.NextSegment = StreamUtil.ReadInt32(stream);
                    splinesSegment.SplineParent = StreamUtil.ReadInt32(stream);

                    splinesSegment.LowestXYZ = StreamUtil.ReadVector3(stream);
                    splinesSegment.HighestXYZ = StreamUtil.ReadVector3(stream);

                    splinesSegment.SegmentDisatnce = StreamUtil.ReadFloat(stream);
                    splinesSegment.PreviousSegmentsDistance = StreamUtil.ReadFloat(stream);
                    splinesSegment.Unknown32 = StreamUtil.ReadInt32(stream);
                    splinesSegments.Add(splinesSegment);
                }

                //Texture Flips
                textureFlipbooks = new List<TextureFlipbook>();
                stream.Position = TextureFlipbookOffset;
                for (int i = 0; i < NumTextureFlipbooks; i++)
                {
                    var TempTextureFlip = new TextureFlipbook();
                    TempTextureFlip.ImageCount = StreamUtil.ReadInt32(stream);
                    TempTextureFlip.ImagePositions = new List<int>();
                    for (int a = 0; a < TempTextureFlip.ImageCount; a++)
                    {
                        TempTextureFlip.ImagePositions.Add(StreamUtil.ReadInt32(stream));
                    }
                    textureFlipbooks.Add(TempTextureFlip);
                }

                //ModelPointers
                stream.Position = ModelPointerOffset;
                PrefabPointers = new List<int>();
                for (int i = 0; i < NumModels; i++)
                {
                    PrefabPointers.Add(StreamUtil.ReadInt32(stream));
                }

                //ModelHeaders
                stream.Position = ModelsOffset;
                PrefabData = new List<Prefabs>();
                for (int i = 0; i < PrefabPointers.Count; i++)
                {
                    stream.Position = ModelsOffset + PrefabPointers[i];
                    var TempHeader = new Prefabs();
                    TempHeader.TotalLength = StreamUtil.ReadInt32(stream);
                    TempHeader.ObjectCount = StreamUtil.ReadInt32(stream);
                    TempHeader.ObjectOffset = StreamUtil.ReadInt32(stream); //Size From Start? Idk it doesnt seem to change
                    TempHeader.MaterialBlockID = StreamUtil.ReadInt32(stream);
                    TempHeader.Unknown3 = StreamUtil.ReadInt32(stream); //Doesnt Change as well
                    TempHeader.AnimTime = StreamUtil.ReadFloat(stream);
                    TempHeader.Scale = StreamUtil.ReadVector3(stream);
                    TempHeader.TotalMeshCount = StreamUtil.ReadInt32(stream);
                    TempHeader.VertexCount = StreamUtil.ReadInt32(stream);
                    TempHeader.TriStripCount = StreamUtil.ReadInt32(stream);
                    TempHeader.Unknown4 = StreamUtil.ReadInt32(stream); //Who even knows
                    TempHeader.NonTriCount = StreamUtil.ReadInt32(stream);

                    TempHeader.PrefabObjects = new List<ObjectHeader>();
                    for (int a = 0; a < TempHeader.ObjectCount; a++)
                    {
                        var TempPrefab = new ObjectHeader();
                        long StartPos = stream.Position;
                        TempPrefab.ParentID = StreamUtil.ReadInt32(stream);
                        TempPrefab.ObjectHighOffset = StreamUtil.ReadInt32(stream);
                        TempPrefab.ObjectMediumOffset = StreamUtil.ReadInt32(stream);
                        TempPrefab.ObjectLowOffset = StreamUtil.ReadInt32(stream);
                        TempPrefab.AnimOffset = StreamUtil.ReadInt32(stream);
                        TempPrefab.Matrix4x4Offset = StreamUtil.ReadInt32(stream);

                        long tempPos = stream.Position;
                        if (TempPrefab.Matrix4x4Offset != 0 && TempPrefab.Matrix4x4Offset != -1)
                        {
                            stream.Position = StartPos + TempPrefab.Matrix4x4Offset;
                            TempPrefab.matrix4X4 = StreamUtil.ReadMatrix4x4(stream);
                        }

                        if (TempPrefab.ObjectHighOffset != 0 && TempPrefab.ObjectHighOffset != -1)
                        {
                            stream.Position = StartPos + TempPrefab.ObjectHighOffset;

                            ObjectData meshHeader = new ObjectData();
                            meshHeader.TotalEntryLength = StreamUtil.ReadInt32(stream);
                            meshHeader.LowestXYZ = StreamUtil.ReadVector3(stream);
                            meshHeader.HighestXYZ = StreamUtil.ReadVector3(stream);

                            meshHeader.Flags = StreamUtil.ReadInt32(stream);
                            meshHeader.MeshCount = StreamUtil.ReadInt32(stream);
                            meshHeader.FaceCount = StreamUtil.ReadInt32(stream);

                            meshHeader.TotalOffsetsLength = StreamUtil.ReadInt32(stream); //Totatl Context Lengths

                            meshHeader.MeshOffsetPositions = new List<int>();
                            for (int c = 0; c < meshHeader.MeshCount; c++)
                            {
                                meshHeader.MeshOffsetPositions.Add(StreamUtil.ReadInt32(stream));
                            }

                            meshHeader.MeshOffsets = new List<MeshOffsets>();
                            for (int b = 0; b < meshHeader.MeshCount; b++)
                            {
                                var context = new MeshOffsets();
                                context.EntryLength = StreamUtil.ReadInt32(stream);
                                context.MeshID = StreamUtil.ReadInt32(stream);
                                context.MeshDataLength = StreamUtil.ReadInt32(stream);
                                context.StartPos = StreamUtil.ReadInt32(stream);
                                context.Length1 = StreamUtil.ReadInt32(stream);
                                context.Length2 = StreamUtil.ReadInt32(stream);
                                context.Length3 = StreamUtil.ReadInt32(stream);
                                meshHeader.MeshOffsets.Add(context);
                            }
                            TempPrefab.objectData = meshHeader;
                        }
                        stream.Position = tempPos;
                        TempHeader.PrefabObjects.Add(TempPrefab);
                    }
                    PrefabData.Add(TempHeader);
                }

                //Particle Model Pointers
                stream.Position = ParticleModelPointerOffset;
                ParticleModelPointers = new List<int>();
                for (int i = 0; i < NumParticleInstances; i++)
                {
                    ParticleModelPointers.Add(StreamUtil.ReadInt32(stream));
                }

                //Particle Models
                particleModels = new List<ParticleModel>();
                for (int i = 0; i < NumParticleModel; i++)
                {
                    stream.Position = ParticleModelsOffset + ParticleModelPointers[i];
                    ParticleModel TempParticleModel = new ParticleModel();
                    TempParticleModel.TotalLength = StreamUtil.ReadInt32(stream);
                    TempParticleModel.ObjectCount = StreamUtil.ReadInt32(stream);
                    TempParticleModel.ObjectOffset = StreamUtil.ReadInt32(stream);
                    TempParticleModel.Unknown2 = StreamUtil.ReadInt32(stream);
                    TempParticleModel.Unknown3 = StreamUtil.ReadInt32(stream);
                    TempParticleModel.Unknown4 = StreamUtil.ReadInt32(stream);
                    TempParticleModel.Unknown5 = StreamUtil.ReadInt32(stream);
                    TempParticleModel.Unknown6 = StreamUtil.ReadInt32(stream);
                    TempParticleModel.Unknown7 = StreamUtil.ReadInt32(stream);
                    TempParticleModel.Unknown8 = StreamUtil.ReadInt32(stream);
                    TempParticleModel.Unknown9 = StreamUtil.ReadInt32(stream);
                    TempParticleModel.Unknown10 = StreamUtil.ReadInt32(stream);
                    TempParticleModel.UnknownLenght = StreamUtil.ReadInt32(stream);
                    TempParticleModel.bytes = StreamUtil.ReadBytes(stream, TempParticleModel.UnknownLenght - 4);
                    particleModels.Add(TempParticleModel);
                }

                //Camera Pointers
                stream.Position = CameraPointerOffset;
                CameraPointers = new List<int>();
                for (int i = 0; i < NumCameras; i++)
                {
                    CameraPointers.Add(StreamUtil.ReadInt32(stream));
                }

                //Camera Data
                Cameras = new List<Camera>();
                for (int i = 0; i < NumCameras; i++)
                {
                    stream.Position = CamerasOffset + CameraPointers[i];
                    Camera TempCamera = new Camera();
                    TempCamera.TotalLength = StreamUtil.ReadInt32(stream);
                    TempCamera.bytes = StreamUtil.ReadBytes(stream, TempCamera.TotalLength - 4);
                    Cameras.Add(TempCamera);
                }

                stream.Position = HashOffset;
                hashData = new HashData();
                hashData.TotalLength = StreamUtil.ReadInt32(stream);
                hashData.bytes = StreamUtil.ReadBytes(stream, hashData.TotalLength - 4);

                //New Model Reading Method
                //Make a way to combine models
                int MeshID = 0;
                stream.Position = MeshDataOffset;
                models = new List<MeshData>();
                for (int i = 0; i < PrefabData.Count; i++)
                {
                    var TempPrefabData = PrefabData[i];
                    for (int a = 0; a < TempPrefabData.PrefabObjects.Count; a++)
                    {
                        var TempObjects = TempPrefabData.PrefabObjects[a];
                        if (TempObjects.objectData.MeshOffsets != null)
                        {
                            TempObjects.objectData.meshes = new();
                            TempObjects.objectData.ModelIDs = new();
                            for (int b = 0; b < TempObjects.objectData.MeshOffsets.Count; b++)
                            {
                                var TempMeshData = new MeshData();
                                TempMeshData.staticMeshes = new List<StaticMesh>();
                                stream.Position = TempObjects.objectData.MeshOffsets[b].StartPos + MeshDataOffset;
                                while (true)
                                {
                                    var temp = ReadMesh(stream);
                                    TempMeshData.staticMeshes.Add(GenerateFaces(temp));
                                    stream.Position += 31;
                                    if (StreamUtil.ReadByte(stream) == 0x6C)
                                    {
                                        stream.Position -= 32;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                TempObjects.objectData.ModelIDs.Add(MeshID);
                                MeshID++;
                                TempObjects.objectData.meshes.Add(TempMeshData);
                            }
                        }
                        TempPrefabData.PrefabObjects[a] = TempObjects;
                    }
                    PrefabData[i] = TempPrefabData;
                }
            }
        }
        public void Save(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                //Skip writing header info
                stream.Position += 4 * 34;

                StreamUtil.AlignBy16(stream);

                //Patches
                stream.Position = PatchOffset;
                //PatchOffset = (int)stream.Position;
                for (int i = 0; i < Patches.Count; i++)
                {
                    var TempPatch = Patches[i];
                    StreamUtil.WriteVector4(stream, TempPatch.LightMapPoint);

                    StreamUtil.WriteVector4(stream, TempPatch.UVPoint1);
                    StreamUtil.WriteVector4(stream, TempPatch.UVPoint2);
                    StreamUtil.WriteVector4(stream, TempPatch.UVPoint3);
                    StreamUtil.WriteVector4(stream, TempPatch.UVPoint4);

                    StreamUtil.WriteVector4(stream, TempPatch.R4C4);
                    StreamUtil.WriteVector4(stream, TempPatch.R4C3);
                    StreamUtil.WriteVector4(stream, TempPatch.R4C2);
                    StreamUtil.WriteVector4(stream, TempPatch.R4C1);
                    StreamUtil.WriteVector4(stream, TempPatch.R3C4);
                    StreamUtil.WriteVector4(stream, TempPatch.R3C3);
                    StreamUtil.WriteVector4(stream, TempPatch.R3C2);
                    StreamUtil.WriteVector4(stream, TempPatch.R3C1);
                    StreamUtil.WriteVector4(stream, TempPatch.R2C4);
                    StreamUtil.WriteVector4(stream, TempPatch.R2C3);
                    StreamUtil.WriteVector4(stream, TempPatch.R2C2);
                    StreamUtil.WriteVector4(stream, TempPatch.R2C1);
                    StreamUtil.WriteVector4(stream, TempPatch.R1C4);
                    StreamUtil.WriteVector4(stream, TempPatch.R1C3);
                    StreamUtil.WriteVector4(stream, TempPatch.R1C2);
                    StreamUtil.WriteVector4(stream, TempPatch.R1C1);

                    StreamUtil.WriteVector3(stream, TempPatch.LowestXYZ);
                    StreamUtil.WriteVector3(stream, TempPatch.HighestXYZ);

                    StreamUtil.WriteVector4(stream, TempPatch.Point1);
                    StreamUtil.WriteVector4(stream, TempPatch.Point2);
                    StreamUtil.WriteVector4(stream, TempPatch.Point3);
                    StreamUtil.WriteVector4(stream, TempPatch.Point4);

                    StreamUtil.WriteInt32(stream, TempPatch.PatchStyle);
                    StreamUtil.WriteInt32(stream, TempPatch.Unknown2);
                    StreamUtil.WriteInt16(stream, TempPatch.TextureAssigment);
                    StreamUtil.WriteInt16(stream, TempPatch.LightmapID);
                    StreamUtil.WriteInt32(stream, TempPatch.Unknown4);
                    StreamUtil.WriteInt32(stream, TempPatch.Unknown5);
                    StreamUtil.WriteInt32(stream, TempPatch.Unknown6);
                }

                StreamUtil.AlignBy16(stream);

                //Instances
                stream.Position = InstanceOffset;
                //InstanceOffset = (int)stream.Position;
                for (int i = 0; i < Instances.Count; i++)
                {
                    var TempInstance = Instances[i];
                    StreamUtil.WriteVector4(stream, TempInstance.MatrixCol1);
                    StreamUtil.WriteVector4(stream, TempInstance.MatrixCol2);
                    StreamUtil.WriteVector4(stream, TempInstance.MatrixCol3);
                    StreamUtil.WriteVector4(stream, TempInstance.InstancePosition);
                    StreamUtil.WriteVector4(stream, TempInstance.Unknown5);
                    StreamUtil.WriteVector4(stream, TempInstance.Unknown6);
                    StreamUtil.WriteVector4(stream, TempInstance.Unknown7);
                    StreamUtil.WriteVector4(stream, TempInstance.Unknown8);
                    StreamUtil.WriteVector4(stream, TempInstance.Unknown9);
                    StreamUtil.WriteVector4(stream, TempInstance.Unknown10);
                    StreamUtil.WriteVector4(stream, TempInstance.Unknown11);
                    StreamUtil.WriteVector4(stream, TempInstance.RGBA);

                    StreamUtil.WriteInt32(stream, TempInstance.ModelID);
                    StreamUtil.WriteInt32(stream, TempInstance.PrevInstance);
                    StreamUtil.WriteInt32(stream, TempInstance.NextInstance);

                    StreamUtil.WriteVector3(stream, TempInstance.LowestXYZ);
                    StreamUtil.WriteVector3(stream, TempInstance.HighestXYZ);

                    StreamUtil.WriteInt32(stream, TempInstance.UnknownInt26);
                    StreamUtil.WriteInt32(stream, TempInstance.UnknownInt27);
                    StreamUtil.WriteInt32(stream, TempInstance.UnknownInt28);
                    StreamUtil.WriteInt32(stream, TempInstance.ModelID2);
                    StreamUtil.WriteInt32(stream, TempInstance.UnknownInt30);
                    StreamUtil.WriteInt32(stream, TempInstance.UnknownInt31);
                    StreamUtil.WriteInt32(stream, TempInstance.UnknownInt32);
                }

                ////Particle Instances
                //stream.Position = ParticleInstancesOffset;
                //for (int i = 0; i < particleInstances.Count; i++)
                //{
                //    var TempParticle = particleInstances[i];
                //    SaveVertices(stream, TempParticle.MatrixCol1, true);
                //    SaveVertices(stream, TempParticle.MatrixCol2, true);
                //    SaveVertices(stream, TempParticle.MatrixCol3, true);
                //    SaveVertices(stream, TempParticle.ParticleInstancePosition, true);
                //    StreamUtil.WriteInt32(stream, TempParticle.UnknownInt1);
                //    SaveVertices(stream, TempParticle.LowestXYZ, false);
                //    SaveVertices(stream, TempParticle.HighestXYZ, false);
                //    StreamUtil.WriteInt32(stream, TempParticle.UnknownInt8);
                //    StreamUtil.WriteInt32(stream, TempParticle.UnknownInt9);
                //    StreamUtil.WriteInt32(stream, TempParticle.UnknownInt10);
                //    StreamUtil.WriteInt32(stream, TempParticle.UnknownInt11);
                //    StreamUtil.WriteInt32(stream, TempParticle.UnknownInt12);
                //}

                stream.Position = MaterialOffset;
                for (int i = 0; i < materials.Count; i++)
                {
                    var TempMaterial = materials[i];
                    StreamUtil.WriteInt16(stream, TempMaterial.TextureID);
                    StreamUtil.WriteInt16(stream, TempMaterial.UnknownInt2);
                    StreamUtil.WriteInt32(stream, TempMaterial.UnknownInt3);

                    StreamUtil.WriteFloat32(stream, TempMaterial.UnknownFloat1);
                    StreamUtil.WriteFloat32(stream, TempMaterial.UnknownFloat2);
                    StreamUtil.WriteFloat32(stream, TempMaterial.UnknownFloat3);
                    StreamUtil.WriteFloat32(stream, TempMaterial.UnknownFloat4);

                    StreamUtil.WriteInt32(stream, TempMaterial.UnknownInt8);

                    StreamUtil.WriteFloat32(stream, TempMaterial.UnknownFloat5);
                    StreamUtil.WriteFloat32(stream, TempMaterial.UnknownFloat6);
                    StreamUtil.WriteFloat32(stream, TempMaterial.UnknownFloat7);
                    StreamUtil.WriteFloat32(stream, TempMaterial.UnknownFloat8);

                    StreamUtil.WriteInt32(stream, TempMaterial.UnknownInt13);
                    StreamUtil.WriteInt32(stream, TempMaterial.UnknownInt14);
                    StreamUtil.WriteInt32(stream, TempMaterial.UnknownInt15);
                    StreamUtil.WriteInt32(stream, TempMaterial.UnknownInt16);
                    StreamUtil.WriteInt32(stream, TempMaterial.UnknownInt17);
                    StreamUtil.WriteInt32(stream, TempMaterial.UnknownInt18);

                    StreamUtil.WriteInt16(stream, TempMaterial.TextureFlipbookID);
                    StreamUtil.WriteInt16(stream, TempMaterial.UnknownInt20);
                }

                //StreamUtil.AlignBy16(stream);

                ////Texture Flipbooks
                //stream.Position = TextureFlipbookOffset;
                //for (int i = 0; i < textureFlipbooks.Count; i++)
                //{
                //    StreamUtil.WriteInt32(stream, textureFlipbooks[i].ImagePositions.Count);
                //    for (int a = 0; a < textureFlipbooks[i].ImagePositions.Count; a++)
                //    {
                //        StreamUtil.WriteInt32(stream, textureFlipbooks[i].ImagePositions[a]);
                //    }
                //}


                ////Spline
                stream.Position = SplineOffset;
                for (int i = 0; i < splines.Count; i++)
                {
                    var spline = splines[i];
                    StreamUtil.WriteVector3(stream, spline.LowestXYZ);
                    StreamUtil.WriteVector3(stream, spline.HighestXYZ);
                    StreamUtil.WriteInt32(stream, spline.Unknown1);
                    StreamUtil.WriteInt32(stream, spline.SplineSegmentCount);
                    StreamUtil.WriteInt32(stream, spline.SplineSegmentPosition);
                    StreamUtil.WriteInt32(stream, spline.Unknown2);
                }

                //Spline Segments
                stream.Position = SplineSegmentOffset;
                for (int i = 0; i < splinesSegments.Count; i++)
                {
                    var SplineSegment = splinesSegments[i];
                    StreamUtil.WriteVector4(stream, SplineSegment.Point4);
                    StreamUtil.WriteVector4(stream, SplineSegment.Point3);
                    StreamUtil.WriteVector4(stream, SplineSegment.Point2);
                    StreamUtil.WriteVector4(stream, SplineSegment.ControlPoint);

                    StreamUtil.WriteVector4(stream, SplineSegment.ScalingPoint);

                    StreamUtil.WriteInt32(stream, SplineSegment.PreviousSegment);
                    StreamUtil.WriteInt32(stream, SplineSegment.NextSegment);
                    StreamUtil.WriteInt32(stream, SplineSegment.SplineParent);

                    StreamUtil.WriteVector3(stream, SplineSegment.LowestXYZ);
                    StreamUtil.WriteVector3(stream, SplineSegment.HighestXYZ);

                    StreamUtil.WriteFloat32(stream, SplineSegment.SegmentDisatnce);
                    StreamUtil.WriteFloat32(stream, SplineSegment.PreviousSegmentsDistance);
                    StreamUtil.WriteInt32(stream, SplineSegment.Unknown32);
                }


                
                //Go back and write Positions and shit
                stream.Position = 0;

                StreamUtil.WriteBytes(stream, new byte[4] { 0x00, 0x15, 0x1B, 0x01 });
                StreamUtil.WriteInt32(stream, 0);
                StreamUtil.WriteInt32(stream, Patches.Count);
                StreamUtil.WriteInt32(stream, Instances.Count);
                StreamUtil.WriteInt32(stream, particleInstances.Count);
                StreamUtil.WriteInt32(stream, materials.Count);
                StreamUtil.WriteInt32(stream, materialBlocks.Count);
                StreamUtil.WriteInt32(stream, lights.Count);
                StreamUtil.WriteInt32(stream, splines.Count);
                StreamUtil.WriteInt32(stream, splinesSegments.Count);
                StreamUtil.WriteInt32(stream, textureFlipbooks.Count);
                StreamUtil.WriteInt32(stream, PrefabData.Count);
                StreamUtil.WriteInt32(stream, particleModels.Count);
                StreamUtil.WriteInt32(stream, NumTextures);
                StreamUtil.WriteInt32(stream, Cameras.Count);
                StreamUtil.WriteInt32(stream, 0); //Lightmap size 

                StreamUtil.WriteInt32(stream, 0);
                StreamUtil.WriteInt32(stream, PatchOffset);
                StreamUtil.WriteInt32(stream, InstanceOffset);
                StreamUtil.WriteInt32(stream, ParticleInstancesOffset);
                StreamUtil.WriteInt32(stream, MaterialOffset);
                StreamUtil.WriteInt32(stream, MaterialBlocksOffset);
                StreamUtil.WriteInt32(stream, LightsOffset);
                StreamUtil.WriteInt32(stream, SplineOffset);
                StreamUtil.WriteInt32(stream, SplineSegmentOffset);
                StreamUtil.WriteInt32(stream, TextureFlipbookOffset);
                StreamUtil.WriteInt32(stream, ModelPointerOffset);
                StreamUtil.WriteInt32(stream, ModelsOffset);
                StreamUtil.WriteInt32(stream, ParticleModelPointerOffset);
                StreamUtil.WriteInt32(stream, ParticleModelsOffset);
                StreamUtil.WriteInt32(stream, CameraPointerOffset);
                StreamUtil.WriteInt32(stream, CamerasOffset);
                StreamUtil.WriteInt32(stream, HashOffset);
                StreamUtil.WriteInt32(stream, MeshDataOffset);
            }
        }

        public void SaveNew(string path)
        {
            MemoryStream stream = new MemoryStream();
            //Skip writing header info
            stream.Position += 4 * 34;


            StreamUtil.AlignBy16(stream);

            //Patches
            PatchOffset = (int)stream.Position;
            for (int i = 0; i < Patches.Count; i++)
            {
                var TempPatch = Patches[i];
                StreamUtil.WriteVector4(stream, TempPatch.LightMapPoint);

                StreamUtil.WriteVector4(stream, TempPatch.UVPoint1);
                StreamUtil.WriteVector4(stream, TempPatch.UVPoint2);
                StreamUtil.WriteVector4(stream, TempPatch.UVPoint3);
                StreamUtil.WriteVector4(stream, TempPatch.UVPoint4);

                StreamUtil.WriteVector4(stream, TempPatch.R4C4);
                StreamUtil.WriteVector4(stream, TempPatch.R4C3);
                StreamUtil.WriteVector4(stream, TempPatch.R4C2);
                StreamUtil.WriteVector4(stream, TempPatch.R4C1);
                StreamUtil.WriteVector4(stream, TempPatch.R3C4);
                StreamUtil.WriteVector4(stream, TempPatch.R3C3);
                StreamUtil.WriteVector4(stream, TempPatch.R3C2);
                StreamUtil.WriteVector4(stream, TempPatch.R3C1);
                StreamUtil.WriteVector4(stream, TempPatch.R2C4);
                StreamUtil.WriteVector4(stream, TempPatch.R2C3);
                StreamUtil.WriteVector4(stream, TempPatch.R2C2);
                StreamUtil.WriteVector4(stream, TempPatch.R2C1);
                StreamUtil.WriteVector4(stream, TempPatch.R1C4);
                StreamUtil.WriteVector4(stream, TempPatch.R1C3);
                StreamUtil.WriteVector4(stream, TempPatch.R1C2);
                StreamUtil.WriteVector4(stream, TempPatch.R1C1);

                StreamUtil.WriteVector3(stream, TempPatch.LowestXYZ);
                StreamUtil.WriteVector3(stream, TempPatch.HighestXYZ);

                StreamUtil.WriteVector4(stream, TempPatch.Point1);
                StreamUtil.WriteVector4(stream, TempPatch.Point2);
                StreamUtil.WriteVector4(stream, TempPatch.Point3);
                StreamUtil.WriteVector4(stream, TempPatch.Point4);

                StreamUtil.WriteInt32(stream, TempPatch.PatchStyle);
                StreamUtil.WriteInt32(stream, TempPatch.Unknown2);
                StreamUtil.WriteInt16(stream, TempPatch.TextureAssigment);
                StreamUtil.WriteInt16(stream, TempPatch.LightmapID);
                StreamUtil.WriteInt32(stream, TempPatch.Unknown4);
                StreamUtil.WriteInt32(stream, TempPatch.Unknown5);
                StreamUtil.WriteInt32(stream, TempPatch.Unknown6);
            }

            StreamUtil.AlignBy16(stream);

            //Instances
            InstanceOffset = (int)stream.Position;
            for (int i = 0; i < Instances.Count; i++)
            {
                var TempInstance = Instances[i];
                StreamUtil.WriteVector4(stream, TempInstance.MatrixCol1);
                StreamUtil.WriteVector4(stream, TempInstance.MatrixCol2);
                StreamUtil.WriteVector4(stream, TempInstance.MatrixCol3);
                StreamUtil.WriteVector4(stream, TempInstance.InstancePosition);
                StreamUtil.WriteVector4(stream, TempInstance.Unknown5);
                StreamUtil.WriteVector4(stream, TempInstance.Unknown6);
                StreamUtil.WriteVector4(stream, TempInstance.Unknown7);
                StreamUtil.WriteVector4(stream, TempInstance.Unknown8);
                StreamUtil.WriteVector4(stream, TempInstance.Unknown9);
                StreamUtil.WriteVector4(stream, TempInstance.Unknown10);
                StreamUtil.WriteVector4(stream, TempInstance.Unknown11);
                StreamUtil.WriteVector4(stream, TempInstance.RGBA);

                StreamUtil.WriteInt32(stream, TempInstance.ModelID);
                StreamUtil.WriteInt32(stream, TempInstance.PrevInstance);
                StreamUtil.WriteInt32(stream, TempInstance.NextInstance);

                StreamUtil.WriteVector3(stream, TempInstance.LowestXYZ);
                StreamUtil.WriteVector3(stream, TempInstance.HighestXYZ);

                StreamUtil.WriteInt32(stream, TempInstance.UnknownInt26);
                StreamUtil.WriteInt32(stream, TempInstance.UnknownInt27);
                StreamUtil.WriteInt32(stream, TempInstance.UnknownInt28);
                StreamUtil.WriteInt32(stream, TempInstance.ModelID2);
                StreamUtil.WriteInt32(stream, TempInstance.UnknownInt30);
                StreamUtil.WriteInt32(stream, TempInstance.UnknownInt31);
                StreamUtil.WriteInt32(stream, TempInstance.UnknownInt32);
            }

            StreamUtil.AlignBy16(stream);

            //Particle Instances
            ParticleInstancesOffset = (int)stream.Position;
            for (int i = 0; i < particleInstances.Count; i++)
            {
                var TempParticle = particleInstances[i];
                StreamUtil.WriteVector4(stream, TempParticle.MatrixCol1);
                StreamUtil.WriteVector4(stream, TempParticle.MatrixCol2);
                StreamUtil.WriteVector4(stream, TempParticle.MatrixCol3);
                StreamUtil.WriteVector4(stream, TempParticle.ParticleInstancePosition);
                StreamUtil.WriteInt32(stream, TempParticle.UnknownInt1);
                StreamUtil.WriteVector3(stream, TempParticle.LowestXYZ);
                StreamUtil.WriteVector3(stream, TempParticle.HighestXYZ);
                StreamUtil.WriteInt32(stream, TempParticle.UnknownInt8);
                StreamUtil.WriteInt32(stream, TempParticle.UnknownInt9);
                StreamUtil.WriteInt32(stream, TempParticle.UnknownInt10);
                StreamUtil.WriteInt32(stream, TempParticle.UnknownInt11);
                StreamUtil.WriteInt32(stream, TempParticle.UnknownInt12);
            }

            StreamUtil.AlignBy16(stream);

            MaterialOffset = (int)stream.Position;
            for (int i = 0; i < materials.Count; i++)
            {
                var TempMaterial = materials[i];
                StreamUtil.WriteInt16(stream, TempMaterial.TextureID);
                StreamUtil.WriteInt16(stream, TempMaterial.UnknownInt2);
                StreamUtil.WriteInt32(stream, TempMaterial.UnknownInt3);

                StreamUtil.WriteFloat32(stream, TempMaterial.UnknownFloat1);
                StreamUtil.WriteFloat32(stream, TempMaterial.UnknownFloat2);
                StreamUtil.WriteFloat32(stream, TempMaterial.UnknownFloat3);
                StreamUtil.WriteFloat32(stream, TempMaterial.UnknownFloat4);

                StreamUtil.WriteInt32(stream, TempMaterial.UnknownInt8);

                StreamUtil.WriteFloat32(stream, TempMaterial.UnknownFloat5);
                StreamUtil.WriteFloat32(stream, TempMaterial.UnknownFloat6);
                StreamUtil.WriteFloat32(stream, TempMaterial.UnknownFloat7);
                StreamUtil.WriteFloat32(stream, TempMaterial.UnknownFloat8);

                StreamUtil.WriteInt32(stream, TempMaterial.UnknownInt13);
                StreamUtil.WriteInt32(stream, TempMaterial.UnknownInt14);
                StreamUtil.WriteInt32(stream, TempMaterial.UnknownInt15);
                StreamUtil.WriteInt32(stream, TempMaterial.UnknownInt16);
                StreamUtil.WriteInt32(stream, TempMaterial.UnknownInt17);
                StreamUtil.WriteInt32(stream, TempMaterial.UnknownInt18);

                StreamUtil.WriteInt16(stream, TempMaterial.TextureFlipbookID);
                StreamUtil.WriteInt16(stream, TempMaterial.UnknownInt20);
            }
            StreamUtil.AlignBy16(stream);

            MaterialBlocksOffset = (int)stream.Position;
            for (int i = 0; i < materialBlocks.Count; i++)
            {
                var TempMaterialBlock = materialBlocks[i];
                StreamUtil.WriteInt32(stream,TempMaterialBlock.ints.Count);
                for (int a = 0; a < TempMaterialBlock.ints.Count; a++)
                {
                    StreamUtil.WriteInt32(stream, TempMaterialBlock.ints[a]);
                }  
            }
            StreamUtil.AlignBy16(stream);

            LightsOffset = (int)stream.Position;
            for (int i = 0; i < lights.Count; i++)
            {
                var TempLights = lights[i];
                StreamUtil.WriteInt32(stream, TempLights.Type);
                StreamUtil.WriteInt32(stream, TempLights.spriteRes);
                StreamUtil.WriteFloat32(stream, TempLights.UnknownFloat1);
                StreamUtil.WriteInt32(stream, TempLights.UnknownInt1);
                StreamUtil.WriteVector3(stream, TempLights.Colour);
                StreamUtil.WriteVector3(stream, TempLights.Direction);
                StreamUtil.WriteVector3(stream, TempLights.Postion);
                StreamUtil.WriteVector3(stream, TempLights.LowestXYZ);
                StreamUtil.WriteVector3(stream, TempLights.HighestXYZ);
                StreamUtil.WriteFloat32(stream, TempLights.UnknownFloat2);
                StreamUtil.WriteInt32(stream, TempLights.UnknownInt2);
                StreamUtil.WriteFloat32(stream, TempLights.UnknownFloat3);
                StreamUtil.WriteInt32(stream, TempLights.UnknownInt3);
            }
            StreamUtil.AlignBy16(stream);

            ////Spline
            SplineOffset = (int)stream.Position;
            for (int i = 0; i < splines.Count; i++)
            {
                var spline = splines[i];
                StreamUtil.WriteVector3(stream, spline.LowestXYZ);
                StreamUtil.WriteVector3(stream, spline.HighestXYZ);
                StreamUtil.WriteInt32(stream, spline.Unknown1);
                StreamUtil.WriteInt32(stream, spline.SplineSegmentCount);
                StreamUtil.WriteInt32(stream, spline.SplineSegmentPosition);
                StreamUtil.WriteInt32(stream, spline.Unknown2);
            }
            StreamUtil.AlignBy16(stream);
            //Spline Segments
            SplineSegmentOffset = (int)stream.Position;
            for (int i = 0; i < splinesSegments.Count; i++)
            {
                var SplineSegment = splinesSegments[i];
                StreamUtil.WriteVector4(stream, SplineSegment.Point4);
                StreamUtil.WriteVector4(stream, SplineSegment.Point3);
                StreamUtil.WriteVector4(stream, SplineSegment.Point2);
                StreamUtil.WriteVector4(stream, SplineSegment.ControlPoint);

                StreamUtil.WriteVector4(stream, SplineSegment.ScalingPoint);

                StreamUtil.WriteInt32(stream, SplineSegment.PreviousSegment);
                StreamUtil.WriteInt32(stream, SplineSegment.NextSegment);
                StreamUtil.WriteInt32(stream, SplineSegment.SplineParent);

                StreamUtil.WriteVector3(stream, SplineSegment.LowestXYZ);
                StreamUtil.WriteVector3(stream, SplineSegment.HighestXYZ);

                StreamUtil.WriteFloat32(stream, SplineSegment.SegmentDisatnce);
                StreamUtil.WriteFloat32(stream, SplineSegment.PreviousSegmentsDistance);
                StreamUtil.WriteInt32(stream, SplineSegment.Unknown32);
            }
            StreamUtil.AlignBy16(stream);


            //Texture Flipbooks
            TextureFlipbookOffset = (int)stream.Position;
            for (int i = 0; i < textureFlipbooks.Count; i++)
            {
                StreamUtil.WriteInt32(stream, textureFlipbooks[i].ImagePositions.Count);
                for (int a = 0; a < textureFlipbooks[i].ImagePositions.Count; a++)
                {
                    StreamUtil.WriteInt32(stream, textureFlipbooks[i].ImagePositions[a]);
                }
            }
            StreamUtil.AlignBy16(stream);

            //Set Space Aside for model pointers
            ModelPointerOffset = (int)stream.Position;
            PrefabPointers = new List<int>();
            stream.Position = 4 * PrefabData.Count;

            StreamUtil.AlignBy16(stream);

            int Size;
            int TempPos;

            ModelsOffset = (int)stream.Position;
            for (int i = 0; i < PrefabData.Count; i++)
            {
                var TempPrefab = PrefabData[i];

                int StartPos = (int)stream.Position;
                PrefabPointers.Add((int)stream.Position - ModelsOffset);
                stream.Position += 4;
                StreamUtil.WriteInt32(stream, TempPrefab.PrefabObjects.Count);
                StreamUtil.WriteInt32(stream, 56);
                StreamUtil.WriteInt32(stream, TempPrefab.MaterialBlockID);
                StreamUtil.WriteInt32(stream, TempPrefab.Unknown3);
                StreamUtil.WriteFloat32(stream, TempPrefab.AnimTime);
                StreamUtil.WriteVector3(stream, TempPrefab.Scale);
                StreamUtil.WriteInt32(stream, TempPrefab.TotalMeshCount);
                StreamUtil.WriteInt32(stream, TempPrefab.VertexCount);
                StreamUtil.WriteInt32(stream, TempPrefab.TriStripCount);
                StreamUtil.WriteInt32(stream, TempPrefab.Unknown4);
                StreamUtil.WriteInt32(stream, TempPrefab.NonTriCount);

                //Skip Over Objects and Come back to them
                int TempObjectPos = (int)stream.Position;
                stream.Position += 4 * 6 * TempPrefab.PrefabObjects.Count;

                //Write Matrix4x4
                for (int a = 0; a < TempPrefab.PrefabObjects.Count; a++)
                {
                    var TempObject = TempPrefab.PrefabObjects[a];
                    TempObject.Matrix4x4Offset = 0;
                    if (TempObject.matrix4X4.Equals(new Matrix4x4()) && TempObject.matrix4X4 != null)
                    {
                        TempObject.Matrix4x4Offset = (int)stream.Position;
                        StreamUtil.WriteMatrix4x4(stream, TempObject.matrix4X4);
                    }
                    TempPrefab.PrefabObjects[a] = TempObject;
                }
                //Write Object Data
                for (int a = 0; a < TempPrefab.PrefabObjects.Count; a++)
                {
                    var TempObject = TempPrefab.PrefabObjects[a];
                    TempObject.ObjectHighOffset = 0;
                    TempObject.ObjectMediumOffset = 0;
                    TempObject.ObjectLowOffset = 0;
                    if(TempObject.objectData!=null%%TempObject.objectData != new ObjectData())
                    {
                        TempObject.ObjectHighOffset = (int)stream.Position;
                        TempObject.ObjectMediumOffset = (int)stream.Position;
                        TempObject.ObjectLowOffset = (int)stream.Position;
                        int StartObjectPrefab = (int)stream.Position;
                        stream.Position += 4;
                        StreamUtil.WriteVector3(stream, TempObject.objectData.LowestXYZ);
                        StreamUtil.WriteVector3(stream, TempObject.objectData.HighestXYZ);

                        StreamUtil.WriteInt32(stream, TempObject.objectData.Flags);
                        StreamUtil.WriteInt32(stream, TempObject.objectData.MeshOffsets.Count);
                        StreamUtil.WriteInt32(stream, TempObject.objectData.FaceCount);

                        //Go back and Write Total Context Length
                        stream.Position += 4* TempObject.objectData.MeshOffsets.Count + (4 * 7)* TempObject.objectData.MeshOffsets.Count;

                        //Go back and Write Mesh Offset
                        int StartMeshOffsets = (int)stream.Position;
                        stream.Position += 4 * TempObject.objectData.MeshOffsets.Count;

                        List<int> MeshOffsets = new List<int>();

                        for (int b = 0; b < TempObject.objectData.MeshOffsets.Count; b++)
                        {
                            MeshOffsets.Add((int)stream.Position);
                            StreamUtil.WriteInt32(stream, 4 * 7);
                            StreamUtil.WriteInt32(stream, b);
                            StreamUtil.WriteInt32(stream, TempObject.objectData.MeshOffsets[b].MeshDataLength);
                            StreamUtil.WriteInt32(stream, TempObject.objectData.MeshOffsets[b].StartPos);
                            StreamUtil.WriteInt32(stream, TempObject.objectData.MeshOffsets[b].Length1);
                            StreamUtil.WriteInt32(stream, TempObject.objectData.MeshOffsets[b].Length2);
                            StreamUtil.WriteInt32(stream, TempObject.objectData.MeshOffsets[b].Length3);
                        }

                        TempPos = (int)stream.Position;
                        stream.Position = StartMeshOffsets;
                        for (int b = 0; b < MeshOffsets.Count; b++)
                        {
                            StreamUtil.WriteInt32(stream, MeshOffsets[b] - (int)stream.Position);
                        }
                        stream.Position = TempPos;





                        TempPos = (int)stream.Position;
                        Size = (int)stream.Position - StartObjectPrefab;
                        stream.Position = StartObjectPrefab;
                        StreamUtil.WriteInt32(stream, Size);
                        stream.Position = TempPos;
                    }

                    TempPrefab.PrefabObjects[a] = TempObject;
                }


                //Go back and write ObjectOffsets
                TempPos = (int)stream.Position;
                stream.Position = TempObjectPos;
                for (int a = 0; a < TempPrefab.PrefabObjects.Count; a++)
                {
                    int StartPosObject = (int)stream.Position;
                    StreamUtil.WriteInt32(stream, TempPrefab.PrefabObjects[a].ParentID);
                    StreamUtil.WriteInt32(stream, TempPrefab.PrefabObjects[a].ObjectHighOffset- StartPosObject);
                    StreamUtil.WriteInt32(stream, TempPrefab.PrefabObjects[a].ObjectMediumOffset- StartPosObject);
                    StreamUtil.WriteInt32(stream, TempPrefab.PrefabObjects[a].ObjectLowOffset- StartPosObject);
                    StreamUtil.WriteInt32(stream, TempPrefab.PrefabObjects[a].AnimOffset);
                    StreamUtil.WriteInt32(stream, TempPrefab.PrefabObjects[a].Matrix4x4Offset-StartPosObject);
                }


                StreamUtil.AlignBy16(stream);
                TempPos = (int)stream.Position;
                Size = (int)stream.Position - StartPos;
                stream.Position = StartPos;
                StreamUtil.WriteInt32(stream, Size);
                stream.Position = TempPos;

            }

            int TempPosition = (int)stream.Position;
            stream.Position = ModelPointerOffset;
            for (int i = 0; i < PrefabPointers.Count; i++)
            {
                StreamUtil.WriteInt32(stream, PrefabPointers[i]);
            }
            stream.Position = TempPosition;
            StreamUtil.AlignBy16(stream);


            //Go back and write Positions and shit
            stream.Position = 0;
            StreamUtil.WriteBytes(stream, new byte[4] { 0x00, 0x15, 0x1B, 0x01 });
            StreamUtil.WriteInt32(stream, 0);
            StreamUtil.WriteInt32(stream, Patches.Count);
            StreamUtil.WriteInt32(stream, Instances.Count);
            StreamUtil.WriteInt32(stream, particleInstances.Count);
            StreamUtil.WriteInt32(stream, materials.Count);
            StreamUtil.WriteInt32(stream, materialBlocks.Count);
            StreamUtil.WriteInt32(stream, lights.Count);
            StreamUtil.WriteInt32(stream, splines.Count);
            StreamUtil.WriteInt32(stream, splinesSegments.Count);
            StreamUtil.WriteInt32(stream, textureFlipbooks.Count);
            StreamUtil.WriteInt32(stream, PrefabData.Count);
            StreamUtil.WriteInt32(stream, particleModels.Count);
            StreamUtil.WriteInt32(stream, NumTextures);
            StreamUtil.WriteInt32(stream, Cameras.Count);
            StreamUtil.WriteInt32(stream, 0); //Lightmap size 

            StreamUtil.WriteInt32(stream, 0);
            StreamUtil.WriteInt32(stream, PatchOffset);
            StreamUtil.WriteInt32(stream, InstanceOffset);
            StreamUtil.WriteInt32(stream, ParticleInstancesOffset);
            StreamUtil.WriteInt32(stream, MaterialOffset);
            StreamUtil.WriteInt32(stream, MaterialBlocksOffset);
            StreamUtil.WriteInt32(stream, LightsOffset);
            StreamUtil.WriteInt32(stream, SplineOffset);
            StreamUtil.WriteInt32(stream, SplineSegmentOffset);
            StreamUtil.WriteInt32(stream, TextureFlipbookOffset);
            StreamUtil.WriteInt32(stream, ModelPointerOffset);
            StreamUtil.WriteInt32(stream, ModelsOffset);
            StreamUtil.WriteInt32(stream, ParticleModelPointerOffset);
            StreamUtil.WriteInt32(stream, ParticleModelsOffset);
            StreamUtil.WriteInt32(stream, CameraPointerOffset);
            StreamUtil.WriteInt32(stream, CamerasOffset);
            StreamUtil.WriteInt32(stream, HashOffset);
            StreamUtil.WriteInt32(stream, MeshDataOffset);


            if (File.Exists(path))
            {
                File.Delete(path);
            }
            var file = File.Create(path);
            stream.Position = 0;
            stream.CopyTo(file);
            stream.Dispose();
            file.Close();
        }
        #region Standard Mesh Stuff
        public StaticMesh ReadMesh(Stream stream)
        {
            var ModelData = new StaticMesh();

            if (stream.Position >= stream.Length)
            {
                return new StaticMesh();
            }

            stream.Position += 48;

            ModelData.StripCount = StreamUtil.ReadInt32(stream);
            ModelData.Unknown1 = StreamUtil.ReadInt32(stream);
            ModelData.VertexCount = StreamUtil.ReadInt32(stream);
            ModelData.Unknown3 = StreamUtil.ReadInt32(stream);

            stream.Position += 16;

            //Load Strip Count
            List<int> TempStrips = new List<int>();
            for (int a = 0; a < ModelData.StripCount; a++)
            {
                TempStrips.Add(StreamUtil.ReadInt16(stream)/3);
               
            }
            StreamUtil.AlignBy16(stream);

            stream.Position += 16;
            ModelData.Strips = TempStrips;

            List<Vector2> UVs = new List<Vector2>();
            //Read UV Texture Points
            stream.Position += 48;
            for (int a = 0; a < ModelData.VertexCount; a++)
            {
                Vector2 uv = new Vector2();
                uv.X = StreamUtil.ReadInt16(stream) / 4096f;
                uv.Y = StreamUtil.ReadInt16(stream) / 4096f;
                UVs.Add(uv);
            }
            StreamUtil.AlignBy16(stream);
            stream.Position += 16;

            //Everything Above is Correct

            ModelData.uv = UVs;

            List<Vector3> Normals = new List<Vector3>();
            //Read Normals
            stream.Position += 32;
            for (int a = 0; a < ModelData.VertexCount; a++)
            {
                Vector3 normal = new Vector3();
                normal.X = StreamUtil.ReadInt16(stream) / 32768f;
                normal.Y = StreamUtil.ReadInt16(stream) / 32768f;
                normal.Z = StreamUtil.ReadInt16(stream) / 32768f;
                Normals.Add(normal);
            }
            StreamUtil.AlignBy16(stream);
            ModelData.uvNormals = Normals;

            List<Vector3> vertices = new List<Vector3>();
            stream.Position += 16;
            //Load Vertex
            for (int a = 0; a < ModelData.VertexCount; a++)
            {
                Vector3 vertex = new Vector3();
                vertex.X = (float)StreamUtil.ReadInt16(stream) / 32768f;
                vertex.Y = (float)StreamUtil.ReadInt16(stream) / 32768f;
                vertex.Z = (float)StreamUtil.ReadInt16(stream) / 32768f;
                vertices.Add(vertex);
            }
            StreamUtil.AlignBy16(stream);
            ModelData.vertices = vertices;
            stream.Position += 16;

            return ModelData;
        }

        public StaticMesh GenerateFaces(StaticMesh models)
        {
            var ModelData = models;
            //Increment Strips
            List<int> strip2 = new List<int>();
            strip2.Add(0);
            foreach (var item in ModelData.Strips)
            {
                strip2.Add(strip2[strip2.Count - 1] + item);
            }
            ModelData.Strips = strip2;

            //Make Faces
            ModelData.faces = new List<Face>();
            int localIndex = 0;
            int Rotation = 0;
            for (int b = 0; b < ModelData.vertices.Count; b++)
            {
                if (InsideSplits(b, ModelData.Strips))
                {
                    Rotation = 0;
                    localIndex = 1;
                    continue;
                }
                if (localIndex < 2)
                {
                    localIndex++;
                    continue;
                }

                ModelData.faces.Add(CreateFaces(b, ModelData, Rotation));
                Rotation++;
                if (Rotation == 2)
                {
                    Rotation = 0;
                }
                localIndex++;
            }

            return ModelData;
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
        public Face CreateFaces(int Index, StaticMesh ModelData, int roatation)
        {
            Face face = new Face();
            int Index1 = 0;
            int Index2 = 0;
            int Index3 = 0;
            //Fixes the Rotation For Exporting
            //Swap When Exporting to other formats
            //1-Clockwise
            //0-Counter Clocwise
            if (roatation == 1)
            {
                Index1 = Index;
                Index2 = Index - 1;
                Index3 = Index - 2;
            }
            if (roatation == 0)
            {
                Index1 = Index;
                Index2 = Index - 2;
                Index3 = Index - 1;
            }
            face.V1 = ModelData.vertices[Index1];
            face.V2 = ModelData.vertices[Index2];
            face.V3 = ModelData.vertices[Index3];

            face.V1Pos = Index1;
            face.V2Pos = Index2;
            face.V3Pos = Index3;

            if (ModelData.uv.Count != 0)
            {
                face.UV1 = ModelData.uv[Index1];
                face.UV2 = ModelData.uv[Index2];
                face.UV3 = ModelData.uv[Index3];

                face.UV1Pos = Index1;
                face.UV2Pos = Index2;
                face.UV3Pos = Index3;

                face.Normal1 = ModelData.uvNormals[Index1];
                face.Normal2 = ModelData.uvNormals[Index2];
                face.Normal3 = ModelData.uvNormals[Index3];

                face.Normal1Pos = Index1;
                face.Normal2Pos = Index2;
                face.Normal3Pos = Index3;
            }

            return face;
        }
        #endregion

        public void ExportModelsNew(string path)
        {
            //glstHandler.SavePDBModelglTF(path, this);

            for (int a = 0; a < PrefabData.Count; a++)
            {
                int VStartPoint = 0;
                int NStartPoint = 0;
                int UStartPoint = 0;
                string output = "# Exported From SSX Using SSX Multitool Modder by GlitcherOG \n";
                for (int ax = 0; ax < PrefabData[a].PrefabObjects.Count; ax++)
                {
                    output += "o Mesh" + ax.ToString() + "\n";
                    string outputString = "";
                    List<Vector3> vertices = new List<Vector3>();
                    List<Vector3> Normals = new List<Vector3>();
                    List<Vector2> UV = new List<Vector2>();
                    Vector3 Scale = PrefabData[a].Scale;
                    if (Scale.X == 0)
                    {
                        Scale.X = 1;
                    }
                    if (Scale.Y == 0)
                    {
                        Scale.Y = 1;
                    }
                    if (Scale.Z == 0)
                    {
                        Scale.Z = 1;
                    }
                    if (PrefabData[a].PrefabObjects[ax].objectData.meshes != null)
                    {
                        for (int i = 0; i < PrefabData[a].PrefabObjects[ax].objectData.meshes.Count; i++)
                        {
                            for (int ab = 0; ab < PrefabData[a].PrefabObjects[ax].objectData.meshes[i].staticMeshes.Count; ab++)
                            {
                                var Data = PrefabData[a].PrefabObjects[ax].objectData.meshes[i].staticMeshes[ab];
                                for (int b = 0; b < Data.faces.Count; b++)
                                {
                                    var Face = Data.faces[b];

                                    //Vertices
                                    if (!vertices.Contains(Face.V1))
                                    {
                                        vertices.Add(Face.V1);
                                    }
                                    int VPos1 = vertices.IndexOf(Face.V1) + 1 + VStartPoint;

                                    if (!vertices.Contains(Face.V2))
                                    {
                                        vertices.Add(Face.V2);
                                    }
                                    int VPos2 = vertices.IndexOf(Face.V2) + 1 + VStartPoint;

                                    if (!vertices.Contains(Face.V3))
                                    {
                                        vertices.Add(Face.V3);
                                    }
                                    int VPos3 = vertices.IndexOf(Face.V3) + 1 + VStartPoint;

                                    //UVs
                                    if (!UV.Contains(Face.UV1))
                                    {
                                        UV.Add(Face.UV1);
                                    }
                                    int UPos1 = UV.IndexOf(Face.UV1) + 1 + UStartPoint;

                                    if (!UV.Contains(Face.UV2))
                                    {
                                        UV.Add(Face.UV2);
                                    }
                                    int UPos2 = UV.IndexOf(Face.UV2) + 1 + UStartPoint;

                                    if (!UV.Contains(Face.UV3))
                                    {
                                        UV.Add(Face.UV3);
                                    }
                                    int UPos3 = UV.IndexOf(Face.UV3) + 1 + UStartPoint;

                                    //Normals
                                    if (!Normals.Contains(Face.Normal1))
                                    {
                                        Normals.Add(Face.Normal1);
                                    }
                                    int NPos1 = Normals.IndexOf(Face.Normal1) + 1 + NStartPoint;

                                    if (!Normals.Contains(Face.Normal2))
                                    {
                                        Normals.Add(Face.Normal2);
                                    }
                                    int NPos2 = Normals.IndexOf(Face.Normal2) + 1 + NStartPoint;

                                    if (!Normals.Contains(Face.Normal3))
                                    {
                                        Normals.Add(Face.Normal3);
                                    }
                                    int NPos3 = Normals.IndexOf(Face.Normal3) + 1 + NStartPoint;

                                    outputString += "f " + VPos1.ToString() + "/" + UPos1.ToString() + "/" + NPos1.ToString() + " " + VPos2.ToString() + "/" + UPos2.ToString() + "/" + NPos2.ToString() + " " + VPos3.ToString() + "/" + UPos3.ToString() + "/" + NPos3.ToString() + "\n";
                                }
                            }
                        }
                    }

                    for (int i = 0; i < vertices.Count; i++)
                    {
                        output += "v " + vertices[i].X * Scale.X + " " + vertices[i].Y * Scale.Y + " " + vertices[i].Z * Scale.Z + "\n";
                    }
                    for (int i = 0; i < UV.Count; i++)
                    {
                        output += "vt " + UV[i].X + " " + -UV[i].Y + "\n";
                    }
                    for (int i = 0; i < Normals.Count; i++)
                    {
                        output += "vn " + Normals[i].X + " " + Normals[i].Y + " " + Normals[i].Z + "\n";
                    }
                    VStartPoint += vertices.Count;
                    NStartPoint += Normals.Count;
                    UStartPoint += UV.Count;
                    output += outputString;
                }
                File.WriteAllText(path + "/" + a.ToString() + ".obj", output);

            }
        }

        public void ImportModels(string path, objHandler objHandler)
        {
            MemoryStream memoryStream = new MemoryStream();

            for (int a = 0; a < objHandler.modelObjects.Count; a++)
            {
                for (int b = 0; b < objHandler.modelObjects[a].Mesh.Count; b++)
                {
                    var MeshData = objHandler.modelObjects[a].Mesh[b];
                    long ModelStart = 0;
                    long OldPos = 0;
                    long NewPos = 0;
                    List<long> TotalModelLengthPos = new List<long>();
                    List<long> TotalModelLengthPos1 = new List<long>();
                    byte[] TempBytes = new byte[1];

                    //Tristrip Split


                    ModelStart = memoryStream.Position;
                    bool FirstChunk = true;
                    //Start Mesh Write
                    for (int c = 0; c < MeshData.meshChunk.Count; c++)
                    {
                        var TempMeshChunk = MeshData.meshChunk[c];

                        #region Tristrip Data
                        //Line 1
                        OldPos = memoryStream.Position;
                        memoryStream.Position += 3;
                        TempBytes = new byte[13] { 0x10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                        StreamUtil.WriteBytes(memoryStream, TempBytes);

                        //Line2
                        TempBytes = new byte[12] { 0, 0, 0, 0, 0x01, 0x01, 0, 0x01, 0, 0, 0, 0 };
                        StreamUtil.WriteBytes(memoryStream, TempBytes);

                        //Check if first chunk
                        if (FirstChunk)
                        {
                            TempBytes = new byte[1] { 0x0B };
                            StreamUtil.WriteBytes(memoryStream, TempBytes);
                        }
                        else
                        {
                            TempBytes = new byte[1] { 0 };
                            StreamUtil.WriteBytes(memoryStream, TempBytes);
                        }

                        TempBytes = new byte[3] { 0x80, 0x02, 0x6C };
                        StreamUtil.WriteBytes(memoryStream, TempBytes);

                        //Line 3

                        //Write Vertice count
                        StreamUtil.WriteInt8(memoryStream, TempMeshChunk.vertices.Count);

                        TempBytes = new byte[15] { 0x80, 0, 0, 0, 0x40, 0x2E, 0x30, 0x12, 0x04, 0, 0, 0, 0, 0, 0 };
                        StreamUtil.WriteBytes(memoryStream, TempBytes);

                        //Line 4

                        //Write Tristrip Count
                        StreamUtil.WriteInt32(memoryStream, TempMeshChunk.Tristrip.Count);

                        StreamUtil.WriteInt32(memoryStream, 0);

                        //Write Vertice count
                        StreamUtil.WriteInt32(memoryStream, TempMeshChunk.vertices.Count);

                        StreamUtil.WriteInt32(memoryStream, 0);

                        //Line 5

                        TempBytes = new byte[12] { 0, 0, 0, 0, 0x01, 0x01, 0, 0x01, 0, 0, 0, 0 };
                        StreamUtil.WriteBytes(memoryStream, TempBytes);

                        //Check if first chunk
                        if (FirstChunk)
                        {
                            TempBytes = new byte[1] { 0x0D };
                            StreamUtil.WriteBytes(memoryStream, TempBytes);
                        }
                        else
                        {
                            TempBytes = new byte[1] { 0x02 };
                            StreamUtil.WriteBytes(memoryStream, TempBytes);
                        }

                        TempBytes = new byte[1] { 0x80 };
                        StreamUtil.WriteBytes(memoryStream, TempBytes);

                        StreamUtil.WriteInt8(memoryStream, TempMeshChunk.Tristrip.Count);

                        TempBytes = new byte[1] { 0x66 };
                        StreamUtil.WriteBytes(memoryStream, TempBytes);

                        //Tristrip Chunk Generation

                        for (int i = 0; i < TempMeshChunk.Tristrip.Count; i++)
                        {
                            StreamUtil.WriteInt16(memoryStream, TempMeshChunk.Tristrip[i] * 6);
                        }
                        StreamUtil.AlignBy16(memoryStream);

                        //Line 6
                        StreamUtil.WriteInt24(memoryStream, 1);

                        StreamUtil.WriteInt8(memoryStream, 48);

                        TotalModelLengthPos.Add(memoryStream.Position);

                        StreamUtil.AlignBy16(memoryStream);

                        ///Generate HeaderSize
                        NewPos = memoryStream.Position;
                        memoryStream.Position = OldPos;

                        int Size = ((int)(NewPos - OldPos) / 16) - 2;
                        StreamUtil.WriteInt24(memoryStream, Size);

                        memoryStream.Position = NewPos;
                        #endregion

                        OldPos = memoryStream.Position;
                        memoryStream.Position += 3;
                        TempBytes = new byte[13] { 0x10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                        StreamUtil.WriteBytes(memoryStream, TempBytes);

                        #region UVCords

                        //Line 2
                        TempBytes = new byte[16] { 0, 0x10, 0, 0, 0, 0x10, 0, 0, 0, 0, 0, 0x20, 0x50, 0x50, 0x50, 0x50 };
                        StreamUtil.WriteBytes(memoryStream, TempBytes);

                        //Line 3
                        TempBytes = new byte[12] { 0, 0, 0, 0, 0x03, 0x01, 0, 0x01, 0, 0, 0, 0 };
                        StreamUtil.WriteBytes(memoryStream, TempBytes);
                        if (FirstChunk)
                        {
                            StreamUtil.WriteInt8(memoryStream, 13+ TempMeshChunk.Tristrip.Count);
                        }
                        else
                        {
                            StreamUtil.WriteInt8(memoryStream, 2 + TempMeshChunk.Tristrip.Count);
                        }
                        StreamUtil.WriteInt8(memoryStream, 128);
                        StreamUtil.WriteInt8(memoryStream, TempMeshChunk.TextureCords.Count);
                        StreamUtil.WriteInt8(memoryStream, 117);

                        //UVCord Save
                        for (int i = 0; i < TempMeshChunk.TextureCords.Count; i++)
                        {
                            StreamUtil.WriteInt16(memoryStream, (int)((TempMeshChunk.TextureCords[i].X) * 4096f));
                            StreamUtil.WriteInt16(memoryStream, -(int)((TempMeshChunk.TextureCords[i].Y) * 4096f));
                        }
                        StreamUtil.AlignBy16(memoryStream);

                        #endregion

                        #region Normals
                        //Line1
                        TempBytes = new byte[16] { 0, 0, 0, 0x05, 0, 0, 0, 0x30, 0, 0, 0, 0, 0, 0, 0, 0 };
                        StreamUtil.WriteBytes(memoryStream, TempBytes);
                        //Line2
                        TempBytes = new byte[16] { 0, 0, 0, 0, 0, 0x80, 0, 0, 0, 0, 0, 0x20, 0x40, 0x40, 0x40, 0x40 };
                        StreamUtil.WriteBytes(memoryStream, TempBytes);
                        //Line3
                        TempBytes = new byte[12] { 0, 0, 0,0, 0x03, 0x01, 0, 0x01, 0, 0, 0, 0 };
                        StreamUtil.WriteBytes(memoryStream, TempBytes);
                        if (FirstChunk)
                        {
                            StreamUtil.WriteInt8(memoryStream, 14 + TempMeshChunk.Tristrip.Count);
                        }
                        else
                        {
                            StreamUtil.WriteInt8(memoryStream, 3 + TempMeshChunk.Tristrip.Count);
                        }
                        StreamUtil.WriteInt8(memoryStream, 128);
                        StreamUtil.WriteInt8(memoryStream, TempMeshChunk.normals.Count);
                        StreamUtil.WriteInt8(memoryStream, 121);
                        //Normals Generation
                        for (int i = 0; i < TempMeshChunk.normals.Count; i++)
                        {
                            StreamUtil.WriteInt16(memoryStream, (int)((TempMeshChunk.normals[i].X) * 32768f));
                            StreamUtil.WriteInt16(memoryStream, (int)((TempMeshChunk.normals[i].Y) * 32768f));
                            StreamUtil.WriteInt16(memoryStream, (int)((TempMeshChunk.normals[i].Z) * 32768f));
                        }
                        StreamUtil.AlignBy16(memoryStream);
                        #endregion

                        #region Vertices
                        //Line 1
                        TempBytes = new byte[12] { 0, 0, 0, 0, 0x03, 0x01, 0, 0x01, 0, 0, 0, 0 };
                        StreamUtil.WriteBytes(memoryStream, TempBytes);
                        if (FirstChunk)
                        {
                            StreamUtil.WriteInt8(memoryStream, 15 + TempMeshChunk.Tristrip.Count);
                        }
                        else
                        {
                            StreamUtil.WriteInt8(memoryStream, 4 + TempMeshChunk.Tristrip.Count);
                        }
                        StreamUtil.WriteInt8(memoryStream, 128);
                        StreamUtil.WriteInt8(memoryStream, TempMeshChunk.vertices.Count);
                        StreamUtil.WriteInt8(memoryStream, 121);

                        //Vertices Generation
                        for (int i = 0; i < TempMeshChunk.vertices.Count; i++)
                        {
                            StreamUtil.WriteInt16(memoryStream, (int)((TempMeshChunk.vertices[i].X * 32768f )/ PrefabData[a].Scale.X));
                            StreamUtil.WriteInt16(memoryStream, (int)((TempMeshChunk.vertices[i].Y * 32768f )/ PrefabData[a].Scale.Y));
                            StreamUtil.WriteInt16(memoryStream, (int)((TempMeshChunk.vertices[i].Z * 32768f )/ PrefabData[a].Scale.Z));
                        }
                        StreamUtil.AlignBy16(memoryStream);

                        #endregion

                        //Total Model Size
                        StreamUtil.WriteInt24(memoryStream, 1);

                        StreamUtil.WriteInt8(memoryStream, 48);

                        TotalModelLengthPos1.Add(memoryStream.Position);

                        StreamUtil.AlignBy16(memoryStream);

                        //Generate LineSize
                        NewPos = memoryStream.Position;
                        memoryStream.Position = OldPos;

                        Size = ((int)(NewPos - OldPos) / 16) - 2;
                        StreamUtil.WriteInt24(memoryStream, Size);

                        memoryStream.Position = NewPos;


                        FirstChunk = false;
                    }

                    TempBytes = new byte[16] { 0x01, 0, 0, 0x60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    StreamUtil.WriteBytes(memoryStream, TempBytes);

                    TempBytes = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0x01, 0x01, 0, 0x01 };
                    StreamUtil.WriteBytes(memoryStream, TempBytes);

                    //Generate TotalModelSize Parts

                    long TotalByteLenght = (memoryStream.Position-ModelStart);
                    OldPos = memoryStream.Position;
                    for (int i = 0; i < TotalModelLengthPos.Count; i++)
                    {
                        memoryStream.Position = TotalModelLengthPos[i];
                        StreamUtil.WriteInt32(memoryStream, (int)TotalByteLenght);
                    }
                    for (int i = 0; i < TotalModelLengthPos1.Count; i++)
                    {
                        memoryStream.Position = TotalModelLengthPos1[i];
                        StreamUtil.WriteInt32(memoryStream, (int)TotalByteLenght+(1+i)*16);
                    }
                    memoryStream.Position = OldPos;

                    //Mesh Endbytes
                    TempBytes = new byte[16] { 0x01, 0, 0, 0x05, 0, 0, 0, 0x30, 0, 0, 0, 0, 0, 0, 0, 0 };
                    StreamUtil.WriteBytes(memoryStream, TempBytes);
                    TempBytes = new byte[16] { 0,0,0,0,0,0,0,0,0,0,0,0,0xEF,0xBE,0xAD,0xDE };
                    StreamUtil.WriteBytes(memoryStream, TempBytes);
                    StreamUtil.WriteBytes(memoryStream, TempBytes);
                }
            }
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            var file = File.Create(path);
            memoryStream.Position = 0;
            memoryStream.CopyTo(file);
            memoryStream.Dispose();
            file.Close();
        }

    }

    public struct Prefabs
    {
        public int TotalLength;
        public int ObjectCount;
        public int ObjectOffset;
        public int MaterialBlockID;
        public int Unknown3;
        public float AnimTime;
        public Vector3 Scale;
        public int TotalMeshCount;
        public int VertexCount;
        public int TriStripCount;
        public int Unknown4;
        public int NonTriCount;
        public List<ObjectHeader> PrefabObjects;
    }

    public struct ObjectHeader
    {
        public int ParentID;
        public int ObjectHighOffset;
        public int ObjectMediumOffset;
        public int ObjectLowOffset;
        public int AnimOffset;
        public int Matrix4x4Offset;

        public Matrix4x4 matrix4X4;
        public ObjectData objectData;
    }

    public struct ObjectData
    {
        public int TotalEntryLength;
        public Vector3 LowestXYZ;
        public Vector3 HighestXYZ;

        public int Flags;
        public int MeshCount;
        public int FaceCount;

        public int TotalOffsetsLength;
        public List<int> MeshOffsetPositions;

        public List<MeshOffsets> MeshOffsets;
        public List<MeshData> meshes;
        public List<int> ModelIDs;
    }

    public struct MeshOffsets
    {
        public int EntryLength;
        public int MeshID;
        public int MeshDataLength;
        public int StartPos;
        public int Length1;
        public int Length2;
        public int Length3;
    }

    public struct ParticleModel
    {
        public int TotalLength;
        public int ObjectCount;
        public int ObjectOffset;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public int Unknown5;
        public int Unknown6;
        public int Unknown7;
        public int Unknown8;
        public int Unknown9;
        public int Unknown10;
        public int UnknownLenght;
        public byte[] bytes;
    }

    public struct TextureFlipbook
    {
        public int ImageCount;
        public List<int> ImagePositions;
    }

    public struct Spline
    {
        public Vector3 LowestXYZ;
        public Vector3 HighestXYZ;
        public int Unknown1;
        public int SplineSegmentCount;
        public int SplineSegmentPosition;
        public int Unknown2;
    }

    public struct SplinesSegments
    {
        public Vector4 Point4;
        public Vector4 Point3;
        public Vector4 Point2;
        public Vector4 ControlPoint;
        public Vector4 ScalingPoint; //Not really sure about that
        public int PreviousSegment;
        public int NextSegment;
        public int SplineParent;
        public Vector3 LowestXYZ;
        public Vector3 HighestXYZ;
        public float SegmentDisatnce;
        public float PreviousSegmentsDistance;
        public int Unknown32;
    }

    public struct Instance
    {
        public Vector4 MatrixCol1;
        public Vector4 MatrixCol2;
        public Vector4 MatrixCol3;
        public Vector4 InstancePosition;
        public Vector4 Unknown5;
        public Vector4 Unknown6;
        public Vector4 Unknown7;
        public Vector4 Unknown8;
        public Vector4 Unknown9;
        public Vector4 Unknown10;
        public Vector4 Unknown11;
        public Vector4 RGBA;
        public int ModelID;
        public int PrevInstance;
        public int NextInstance;

        public Vector3 LowestXYZ;
        public Vector3 HighestXYZ;

        public int UnknownInt26;
        public int UnknownInt27;
        public int UnknownInt28;
        public int ModelID2;
        public int UnknownInt30;
        public int UnknownInt31;
        public int UnknownInt32;

        public int LTGState;
    }

    public struct ParticleInstance
    {
        public Vector4 MatrixCol1;
        public Vector4 MatrixCol2;
        public Vector4 MatrixCol3;
        public Vector4 ParticleInstancePosition;
        public int UnknownInt1;
        public Vector3 LowestXYZ;
        public Vector3 HighestXYZ;
        public int UnknownInt8;
        public int UnknownInt9;
        public int UnknownInt10;
        public int UnknownInt11;
        public int UnknownInt12;
    }

    public struct TrickyMaterial
    {
        public int TextureID;
        public int UnknownInt2;
        public int UnknownInt3;

        public float UnknownFloat1;
        public float UnknownFloat2;
        public float UnknownFloat3;
        public float UnknownFloat4;

        public int UnknownInt8;

        public float UnknownFloat5;
        public float UnknownFloat6;
        public float UnknownFloat7;
        public float UnknownFloat8;

        public int UnknownInt13;
        public int UnknownInt14;
        public int UnknownInt15;
        public int UnknownInt16;
        public int UnknownInt17;
        public int UnknownInt18;

        public int TextureFlipbookID;
        public int UnknownInt20; 

    }

    public struct Light
    {
        public int Type;
        public int spriteRes;
        public float UnknownFloat1;
        public int UnknownInt1;
        public Vector3 Colour;
        public Vector3 Direction;
        public Vector3 Postion;
        public Vector3 LowestXYZ;
        public Vector3 HighestXYZ;
        public float UnknownFloat2;
        public int UnknownInt2;
        public float UnknownFloat3;
        public int UnknownInt3;
    }

    public struct MaterialBlock
    {
        public int BlockCount;
        public List<int> ints;
    }

    public struct Camera
    {
        public int TotalLength;
        public byte[] bytes;
    }

    public struct HashData
    {
        public int TotalLength;
        public byte[] bytes;
    }

    public struct MeshData
    {
        public List<StaticMesh> staticMeshes;
    }

    public struct StaticMesh
    {
        public int ChunkID;

        public int StripCount;
        public int Unknown1;
        public int VertexCount;
        public int Unknown3;
        public List<int> Strips;

        public List<Vector2> uv;
        public List<Vector3> vertices;
        public List<Face> faces;
        public List<Vector3> uvNormals;
    }


    public struct Face
    {
        public Vector3 V1;
        public Vector3 V2;
        public Vector3 V3;

        public int V1Pos;
        public int V2Pos;
        public int V3Pos;

        public Vector2 UV1;
        public Vector2 UV2;
        public Vector2 UV3;

        public int UV1Pos;
        public int UV2Pos;
        public int UV3Pos;

        public Vector3 Normal1;
        public Vector3 Normal2;
        public Vector3 Normal3;

        public int Normal1Pos;
        public int Normal2Pos;
        public int Normal3Pos;
    }

    public struct Patch
    {
        public Vector4 LightMapPoint;

        public Vector4 UVPoint1;
        public Vector4 UVPoint2;
        public Vector4 UVPoint3;
        public Vector4 UVPoint4;

        public Vector4 R4C4;
        public Vector4 R4C3;
        public Vector4 R4C2;
        public Vector4 R4C1;
        public Vector4 R3C4;
        public Vector4 R3C3;
        public Vector4 R3C2;
        public Vector4 R3C1;
        public Vector4 R2C4;
        public Vector4 R2C3;
        public Vector4 R2C2;
        public Vector4 R2C1;
        public Vector4 R1C4;
        public Vector4 R1C3;
        public Vector4 R1C2;
        public Vector4 R1C1;

        public Vector3 LowestXYZ;
        public Vector3 HighestXYZ;

        public Vector4 Point1;
        public Vector4 Point2;
        public Vector4 Point3;
        public Vector4 Point4;

        //0 - Reset
        //1 - Standard Snow
        //2 - Standard Off Track?
        //3 - Powered Snow
        //4 - Slow Powered Snow
        //5 - Ice Standard
        //6 - Bounce/Unskiiable //
        //7 - Ice/Water No Trail
        //8 - Glidy(Lots Of snow particels) //
        //9 - Rock 
        //10 - Wall
        //11 - No Trail, Ice Crunch Sound Effect//
        //12 - No Sound, No Trail, Small particle Wake//
        //13 - Off Track Metal (Slidly Slow, Metal Grinding sounds, Sparks)
        //14 - Speed, Grinding Sound//
        //15 - Standard?//
        //16 - Standard Sand
        //17 - ?//
        //18 - Show Off Ramp/Metal
        public int PatchStyle; //Type

        public int Unknown2; // Some Kind of material Assignment Or Lighting
        public int TextureAssigment; // Texture Assigment 
        public int LightmapID;
        public int Unknown4; //Negative one
        public int Unknown5; //Same
        public int Unknown6; //Same
    }
}
