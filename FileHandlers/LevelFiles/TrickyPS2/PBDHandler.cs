using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using SSXMultiTool.Utilities;

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

        public byte[] MeshData;

        public void LoadPBD(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                MagicBytes = StreamUtil.ReadBytes(stream, 4);
                NumPlayerStarts = StreamUtil.ReadUInt32(stream); //NA
                NumPatches = StreamUtil.ReadUInt32(stream); //Done
                NumInstances = StreamUtil.ReadUInt32(stream); //Done
                NumParticleInstances = StreamUtil.ReadUInt32(stream); //Done
                NumMaterials = StreamUtil.ReadUInt32(stream); //Done
                NumMaterialBlocks = StreamUtil.ReadUInt32(stream); //Done
                NumLights = StreamUtil.ReadUInt32(stream); //Done
                NumSplines = StreamUtil.ReadUInt32(stream); //Done
                NumSplineSegments = StreamUtil.ReadUInt32(stream); //Done
                NumTextureFlipbooks = StreamUtil.ReadUInt32(stream); //Done
                NumModels = StreamUtil.ReadUInt32(stream); //Done
                NumParticleModel = StreamUtil.ReadUInt32(stream); //Done
                NumTextures = StreamUtil.ReadUInt32(stream); //Done
                NumCameras = StreamUtil.ReadUInt32(stream); //Used in SSXFE MAP
                LightMapSize = StreamUtil.ReadUInt32(stream); //Always blank?

                PlayerStartOffset = StreamUtil.ReadUInt32(stream); //NA
                PatchOffset = StreamUtil.ReadUInt32(stream); //Done
                InstanceOffset = StreamUtil.ReadUInt32(stream); //Done
                ParticleInstancesOffset = StreamUtil.ReadUInt32(stream); //Done
                MaterialOffset = StreamUtil.ReadUInt32(stream); //Done
                MaterialBlocksOffset = StreamUtil.ReadUInt32(stream); //Done
                LightsOffset = StreamUtil.ReadUInt32(stream); //Done 
                SplineOffset = StreamUtil.ReadUInt32(stream); //Done
                SplineSegmentOffset = StreamUtil.ReadUInt32(stream); //Done
                TextureFlipbookOffset = StreamUtil.ReadUInt32(stream); //Done
                ModelPointerOffset = StreamUtil.ReadUInt32(stream); //Done
                ModelsOffset = StreamUtil.ReadUInt32(stream); //Done
                ParticleModelPointerOffset = StreamUtil.ReadUInt32(stream); //Done
                ParticleModelsOffset = StreamUtil.ReadUInt32(stream); //Sort of Loading
                CameraPointerOffset = StreamUtil.ReadUInt32(stream); //Done
                CamerasOffset = StreamUtil.ReadUInt32(stream); //sort of loading
                HashOffset = StreamUtil.ReadUInt32(stream); //sort of loading
                MeshDataOffset = StreamUtil.ReadUInt32(stream); //Loading

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

                    patch.PatchStyle = StreamUtil.ReadUInt32(stream);
                    patch.Unknown2 = StreamUtil.ReadInt16(stream);
                    patch.PatchVisablity = StreamUtil.ReadInt16(stream);
                    patch.TextureAssigment = StreamUtil.ReadInt16(stream);

                    patch.LightmapID = StreamUtil.ReadInt16(stream);

                    //Always the same
                    patch.Unknown4 = StreamUtil.ReadUInt32(stream); //Negitive one
                    patch.Unknown5 = StreamUtil.ReadUInt32(stream);
                    patch.Unknown6 = StreamUtil.ReadUInt32(stream);


                    Patches.Add(patch);
                }

                stream.Position = InstanceOffset;
                Instances = new List<Instance>();
                for (int i = 0; i < NumInstances; i++)
                {
                    var TempInstance = new Instance();
                    TempInstance.matrix4X4 = StreamUtil.ReadMatrix4x4(stream);
                    TempInstance.Unknown5 = StreamUtil.ReadVector4(stream);
                    TempInstance.Unknown6 = StreamUtil.ReadVector4(stream);
                    TempInstance.Unknown7 = StreamUtil.ReadVector4(stream);
                    TempInstance.Unknown8 = StreamUtil.ReadVector4(stream);
                    TempInstance.Unknown9 = StreamUtil.ReadVector4(stream);
                    TempInstance.Unknown10 = StreamUtil.ReadVector4(stream);
                    TempInstance.Unknown11 = StreamUtil.ReadVector4(stream);
                    TempInstance.RGBA = StreamUtil.ReadVector4(stream);
                    TempInstance.ModelID = StreamUtil.ReadUInt32(stream);
                    TempInstance.PrevInstance = StreamUtil.ReadUInt32(stream);
                    TempInstance.NextInstance = StreamUtil.ReadUInt32(stream);

                    TempInstance.LowestXYZ = StreamUtil.ReadVector3(stream);
                    TempInstance.HighestXYZ = StreamUtil.ReadVector3(stream);

                    TempInstance.UnknownInt26 = StreamUtil.ReadUInt32(stream);
                    TempInstance.UnknownInt27 = StreamUtil.ReadUInt32(stream);
                    TempInstance.UnknownInt28 = StreamUtil.ReadUInt32(stream);
                    TempInstance.ModelID2 = StreamUtil.ReadUInt32(stream);
                    TempInstance.UnknownInt30 = StreamUtil.ReadUInt32(stream);
                    TempInstance.UnknownInt31 = StreamUtil.ReadUInt32(stream);
                    TempInstance.UnknownInt32 = StreamUtil.ReadUInt32(stream);
                    Instances.Add(TempInstance);
                }

                stream.Position = ParticleInstancesOffset;
                particleInstances = new List<ParticleInstance>();
                for (int i = 0; i < NumParticleInstances; i++)
                {
                    ParticleInstance TempParticle = new ParticleInstance();
                    TempParticle.matrix4X4 = StreamUtil.ReadMatrix4x4(stream);
                    TempParticle.UnknownInt1 = StreamUtil.ReadUInt32(stream);
                    TempParticle.LowestXYZ = StreamUtil.ReadVector3(stream);
                    TempParticle.HighestXYZ = StreamUtil.ReadVector3(stream);
                    TempParticle.UnknownInt8 = StreamUtil.ReadUInt32(stream);
                    TempParticle.UnknownInt9 = StreamUtil.ReadUInt32(stream);
                    TempParticle.UnknownInt10 = StreamUtil.ReadUInt32(stream);
                    TempParticle.UnknownInt11 = StreamUtil.ReadUInt32(stream);
                    TempParticle.UnknownInt12 = StreamUtil.ReadUInt32(stream);
                    particleInstances.Add(TempParticle);
                }

                stream.Position = MaterialOffset;
                materials = new List<TrickyMaterial>();
                for (int i = 0; i < NumMaterials; i++)
                {
                    var TempMaterial = new TrickyMaterial();
                    TempMaterial.TextureID = StreamUtil.ReadInt16(stream);
                    TempMaterial.UnknownInt2 = StreamUtil.ReadInt16(stream);
                    TempMaterial.UnknownInt3 = StreamUtil.ReadUInt32(stream);

                    TempMaterial.UnknownFloat1 = StreamUtil.ReadFloat(stream);
                    TempMaterial.UnknownFloat2 = StreamUtil.ReadFloat(stream);
                    TempMaterial.UnknownFloat3 = StreamUtil.ReadFloat(stream);
                    TempMaterial.UnknownFloat4 = StreamUtil.ReadFloat(stream);

                    TempMaterial.UnknownInt8 = StreamUtil.ReadUInt32(stream);

                    TempMaterial.UnknownFloat5 = StreamUtil.ReadFloat(stream);
                    TempMaterial.UnknownFloat6 = StreamUtil.ReadFloat(stream);
                    TempMaterial.UnknownFloat7 = StreamUtil.ReadFloat(stream);
                    TempMaterial.UnknownFloat8 = StreamUtil.ReadFloat(stream);

                    TempMaterial.UnknownInt13 = StreamUtil.ReadUInt32(stream);
                    TempMaterial.UnknownInt14 = StreamUtil.ReadUInt32(stream);
                    TempMaterial.UnknownInt15 = StreamUtil.ReadUInt32(stream);
                    TempMaterial.UnknownInt16 = StreamUtil.ReadUInt32(stream);
                    TempMaterial.UnknownInt17 = StreamUtil.ReadUInt32(stream);
                    TempMaterial.UnknownInt18 = StreamUtil.ReadUInt32(stream);

                    TempMaterial.TextureFlipbookID = StreamUtil.ReadInt16(stream);
                    TempMaterial.UnknownInt20 = StreamUtil.ReadInt16(stream);

                    materials.Add(TempMaterial);
                }

                stream.Position = MaterialBlocksOffset;
                materialBlocks = new List<MaterialBlock>();
                for (int i = 0; i < NumMaterialBlocks; i++)
                {
                    var TempMaterialBlock = new MaterialBlock();
                    TempMaterialBlock.BlockCount = StreamUtil.ReadUInt32(stream);
                    TempMaterialBlock.ints = new List<int>();
                    for (int a = 0; a < TempMaterialBlock.BlockCount; a++)
                    {
                        TempMaterialBlock.ints.Add(StreamUtil.ReadUInt32(stream));
                    }
                    materialBlocks.Add(TempMaterialBlock);
                }

                stream.Position = LightsOffset;
                lights = new List<Light>();
                for (int i = 0; i < NumLights; i++)
                {
                    var TempLights = new Light();
                    TempLights.Type = StreamUtil.ReadUInt32(stream);
                    TempLights.spriteRes = StreamUtil.ReadUInt32(stream);
                    TempLights.UnknownFloat1 = StreamUtil.ReadFloat(stream);
                    TempLights.UnknownInt1 = StreamUtil.ReadUInt32(stream);
                    TempLights.Colour = StreamUtil.ReadVector3(stream);
                    TempLights.Direction = StreamUtil.ReadVector3(stream);
                    TempLights.Postion = StreamUtil.ReadVector3(stream);
                    TempLights.LowestXYZ = StreamUtil.ReadVector3(stream);
                    TempLights.HighestXYZ = StreamUtil.ReadVector3(stream);
                    TempLights.UnknownFloat2 = StreamUtil.ReadUInt32(stream);
                    TempLights.UnknownInt2 = StreamUtil.ReadUInt32(stream);
                    TempLights.UnknownFloat3 = StreamUtil.ReadUInt32(stream);
                    TempLights.UnknownInt3 = StreamUtil.ReadUInt32(stream);
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
                    spline.Unknown1 = StreamUtil.ReadUInt32(stream);
                    spline.SplineSegmentCount = StreamUtil.ReadUInt32(stream);
                    spline.SplineSegmentPosition = StreamUtil.ReadUInt32(stream);
                    spline.Unknown2 = StreamUtil.ReadUInt32(stream);
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

                    splinesSegment.PreviousSegment = StreamUtil.ReadUInt32(stream);
                    splinesSegment.NextSegment = StreamUtil.ReadUInt32(stream);
                    splinesSegment.SplineParent = StreamUtil.ReadUInt32(stream);

                    splinesSegment.LowestXYZ = StreamUtil.ReadVector3(stream);
                    splinesSegment.HighestXYZ = StreamUtil.ReadVector3(stream);

                    splinesSegment.SegmentDisatnce = StreamUtil.ReadFloat(stream);
                    splinesSegment.PreviousSegmentsDistance = StreamUtil.ReadFloat(stream);
                    splinesSegment.Unknown32 = StreamUtil.ReadUInt32(stream);
                    splinesSegments.Add(splinesSegment);
                }

                //Texture Flips
                textureFlipbooks = new List<TextureFlipbook>();
                stream.Position = TextureFlipbookOffset;
                for (int i = 0; i < NumTextureFlipbooks; i++)
                {
                    var TempTextureFlip = new TextureFlipbook();
                    TempTextureFlip.ImageCount = StreamUtil.ReadUInt32(stream);
                    TempTextureFlip.ImagePositions = new List<int>();
                    for (int a = 0; a < TempTextureFlip.ImageCount; a++)
                    {
                        TempTextureFlip.ImagePositions.Add(StreamUtil.ReadUInt32(stream));
                    }
                    textureFlipbooks.Add(TempTextureFlip);
                }

                //ModelPointers
                stream.Position = ModelPointerOffset;
                PrefabPointers = new List<int>();
                for (int i = 0; i < NumModels; i++)
                {
                    PrefabPointers.Add(StreamUtil.ReadUInt32(stream));
                }

                //ModelHeaders
                stream.Position = ModelsOffset;
                PrefabData = new List<Prefabs>();
                for (int i = 0; i < PrefabPointers.Count; i++)
                {
                    stream.Position = ModelsOffset + PrefabPointers[i];
                    var TempHeader = new Prefabs();
                    TempHeader.TotalLength = StreamUtil.ReadUInt32(stream);
                    TempHeader.ObjectCount = StreamUtil.ReadUInt32(stream);
                    TempHeader.ObjectOffset = StreamUtil.ReadUInt32(stream);
                    TempHeader.MaterialBlockID = StreamUtil.ReadUInt32(stream);
                    TempHeader.Unknown3 = StreamUtil.ReadUInt32(stream); //Does change sometimes
                    TempHeader.AnimTime = StreamUtil.ReadFloat(stream);
                    TempHeader.Scale = StreamUtil.ReadVector3(stream);
                    TempHeader.TotalMeshCount = StreamUtil.ReadUInt32(stream);
                    TempHeader.VertexCount = StreamUtil.ReadUInt32(stream);
                    TempHeader.TriStripCount = StreamUtil.ReadUInt32(stream);
                    TempHeader.Unknown4 = StreamUtil.ReadUInt32(stream); //Who even knows

                    TempHeader.NonTriCount = StreamUtil.ReadUInt32(stream);

                    TempHeader.PrefabObjects = new List<ObjectHeader>();
                    for (int a = 0; a < TempHeader.ObjectCount; a++)
                    {
                        var TempPrefab = new ObjectHeader();
                        long StartPos = stream.Position;
                        TempPrefab.ParentID = StreamUtil.ReadUInt32(stream);
                        TempPrefab.ObjectHighOffset = StreamUtil.ReadUInt32(stream);
                        TempPrefab.ObjectMediumOffset = StreamUtil.ReadUInt32(stream);
                        TempPrefab.ObjectLowOffset = StreamUtil.ReadUInt32(stream);
                        TempPrefab.AnimOffset = StreamUtil.ReadUInt32(stream);
                        TempPrefab.Matrix4x4Offset = StreamUtil.ReadUInt32(stream);

                        long tempPos = stream.Position;
                        if (TempPrefab.Matrix4x4Offset != -1)
                        {
                            stream.Position = StartPos + TempPrefab.Matrix4x4Offset;
                            TempPrefab.matrix4X4 = StreamUtil.ReadMatrix4x4(stream);
                        }

                        if (TempPrefab.ObjectHighOffset != 0 && TempPrefab.ObjectHighOffset != -1)
                        {
                            stream.Position = StartPos + TempPrefab.ObjectHighOffset;

                            ObjectData meshHeader = new ObjectData();
                            meshHeader.TotalEntryLength = StreamUtil.ReadUInt32(stream);
                            meshHeader.LowestXYZ = StreamUtil.ReadVector3(stream);
                            meshHeader.HighestXYZ = StreamUtil.ReadVector3(stream);

                            meshHeader.Flags = StreamUtil.ReadUInt32(stream);
                            meshHeader.MeshCount = StreamUtil.ReadUInt32(stream);
                            meshHeader.FaceCount = StreamUtil.ReadUInt32(stream);

                            meshHeader.TotalOffsetsLength = StreamUtil.ReadUInt32(stream);

                            meshHeader.MeshOffsetPositions = new List<int>();
                            for (int c = 0; c < meshHeader.MeshCount; c++)
                            {
                                meshHeader.MeshOffsetPositions.Add(StreamUtil.ReadUInt32(stream));
                            }

                            meshHeader.MeshOffsets = new List<MeshOffsets>();
                            for (int b = 0; b < meshHeader.MeshCount; b++)
                            {
                                var context = new MeshOffsets();
                                context.EntryLength = StreamUtil.ReadUInt32(stream);
                                context.MaterialBlockPos = StreamUtil.ReadUInt32(stream);
                                context.MeshDataLength = StreamUtil.ReadUInt32(stream);
                                context.StartPos = StreamUtil.ReadUInt32(stream);
                                context.Length1 = StreamUtil.ReadUInt32(stream);
                                context.Length2 = StreamUtil.ReadUInt32(stream);
                                context.Length3 = StreamUtil.ReadUInt32(stream);
                                meshHeader.MeshOffsets.Add(context);
                            }
                            TempPrefab.objectData = meshHeader;
                        }

                        if (TempPrefab.AnimOffset != 0)
                        {
                            stream.Position = StartPos + TempPrefab.AnimOffset;

                            //Load Stuff
                            var TempAnimation = new ObjectAnimation();

                            //Possibly Starting Animation Postion/Rotation
                            TempAnimation.U1 = StreamUtil.ReadFloat(stream);
                            TempAnimation.U2 = StreamUtil.ReadFloat(stream);
                            TempAnimation.U3 = StreamUtil.ReadFloat(stream);
                            TempAnimation.U4 = StreamUtil.ReadFloat(stream);
                            TempAnimation.U5 = StreamUtil.ReadFloat(stream);
                            TempAnimation.U6 = StreamUtil.ReadFloat(stream);

                            TempAnimation.AnimationAction = StreamUtil.ReadUInt32(stream);

                            //Animation Type
                            //1 - X Position
                            //2 - Y Position
                            //4 - Z Position
                            //8 - X Rotate
                            //16 - Y Rotate
                            //32 - Z Rotate

                            TempAnimation.OffsetCount = StreamUtil.ReadUInt32(stream);
                            TempAnimation.OffsetStart = StreamUtil.ReadUInt32(stream);

                            TempAnimation.Offsets = new List<int>();

                            for (int b = 0; b < TempAnimation.OffsetCount; b++)
                            {
                                TempAnimation.Offsets.Add(StreamUtil.ReadUInt32(stream));
                            }

                            TempAnimation.animationEntries = new List<AnimationEntry>();
                            for (int b = 0; b < TempAnimation.Offsets.Count; b++)
                            {
                                stream.Position = StartPos + TempPrefab.AnimOffset + TempAnimation.Offsets[b];
                                var TempAnimationEntry = new AnimationEntry();

                                TempAnimationEntry.MathCount = StreamUtil.ReadUInt32(stream);
                                TempAnimationEntry.MathOffset = StreamUtil.ReadUInt32(stream);

                                TempAnimationEntry.animationMaths = new List<AnimationMath>();

                                for (int c = 0; c < TempAnimationEntry.MathCount; c++)
                                {
                                    AnimationMath TempMath = new AnimationMath();
                                    TempMath.Value1 = StreamUtil.ReadFloat(stream);
                                    TempMath.Value2 = StreamUtil.ReadFloat(stream);
                                    TempMath.Value3 = StreamUtil.ReadFloat(stream);
                                    TempMath.Value4 = StreamUtil.ReadFloat(stream);
                                    TempMath.Value5 = StreamUtil.ReadFloat(stream);
                                    TempMath.Value6 = StreamUtil.ReadFloat(stream);
                                    TempAnimationEntry.animationMaths.Add(TempMath);
                                }

                                TempAnimation.animationEntries.Add(TempAnimationEntry);
                            }

                            TempPrefab.objectAnimation = TempAnimation;
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
                    ParticleModelPointers.Add(StreamUtil.ReadUInt32(stream));
                }

                //Particle Models
                particleModels = new List<ParticleModel>();
                for (int i = 0; i < NumParticleModel; i++)
                {
                    stream.Position = ParticleModelsOffset + ParticleModelPointers[i];
                    ParticleModel TempParticleModel = new ParticleModel();
                    TempParticleModel.TotalLength = StreamUtil.ReadUInt32(stream);
                    TempParticleModel.bytes = StreamUtil.ReadBytes(stream, TempParticleModel.TotalLength - 4);
                    particleModels.Add(TempParticleModel);
                }

                //Camera Pointers
                stream.Position = CameraPointerOffset;
                CameraPointers = new List<int>();
                for (int i = 0; i < NumCameras; i++)
                {
                    CameraPointers.Add(StreamUtil.ReadUInt32(stream));
                }

                //Camera Data
                Cameras = new List<Camera>();
                for (int i = 0; i < NumCameras; i++)
                {
                    stream.Position = CamerasOffset + CameraPointers[i];
                    Camera TempCamera = new Camera();
                    TempCamera.TotalLength = StreamUtil.ReadUInt32(stream);
                    TempCamera.bytes = StreamUtil.ReadBytes(stream, TempCamera.TotalLength - 4);

                    Cameras.Add(TempCamera);
                }

                stream.Position = HashOffset;
                hashData = new HashData();
                hashData.TotalLength = StreamUtil.ReadUInt32(stream);
                hashData.bytes = StreamUtil.ReadBytes(stream, hashData.TotalLength - 4);

                //New Model Reading Method
                //Make a way to combine models
                int MeshID = 0;
                stream.Position = MeshDataOffset;
                for (int i = 0; i < PrefabData.Count; i++)
                {
                    var TempPrefabData = PrefabData[i];
                    if (TempPrefabData.Scale.X == 0)
                    {
                        TempPrefabData.Scale.X = 1;
                    }
                    if (TempPrefabData.Scale.Y == 0)
                    {
                        TempPrefabData.Scale.Y = 1;
                    }
                    if (TempPrefabData.Scale.Z == 0)
                    {
                        TempPrefabData.Scale.Z = 1;
                    }
                    for (int a = 0; a < TempPrefabData.PrefabObjects.Count; a++)
                    {
                        var TempObjects = TempPrefabData.PrefabObjects[a];
                        if (TempObjects.objectData.MeshOffsets != null && !TempObjects.objectData.MeshOffsets.Equals(new List<MeshOffsets>()))
                        {
                            for (int b = 0; b < TempObjects.objectData.MeshOffsets.Count; b++)
                            {
                                var TempMeshOffset = TempObjects.objectData.MeshOffsets[b];
                                var TempMeshData = new Mesh();
                                TempMeshData.meshChunk = new List<MeshChunk>();
                                stream.Position = TempObjects.objectData.MeshOffsets[b].StartPos + MeshDataOffset;
                                while (true)
                                {
                                    var temp = ReadMesh(stream, TempPrefabData.Scale);
                                    TempMeshData.meshChunk.Add(temp);
                                    stream.Position += 31;
                                    if (StreamUtil.ReadUInt8(stream) == 0x6C)
                                    {
                                        stream.Position -= 32;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                TempMeshData=objHandler.GenerateFaces(TempMeshData);
                                TempMeshOffset.FullMesh = TempMeshData;
                                TempMeshOffset.MeshID = MeshID;
                                TempMeshOffset.MeshPath = MeshID.ToString() + ".obj";
                                TempObjects.objectData.MeshOffsets[b] = TempMeshOffset;
                                MeshID++;
                            }
                        }
                        TempPrefabData.PrefabObjects[a] = TempObjects;
                    }
                    PrefabData[i] = TempPrefabData;
                }

                stream.Position = MeshDataOffset;
                MeshData=  StreamUtil.ReadBytes(stream, (int)(stream.Length - MeshDataOffset));
            }
        }

        public MeshChunk ReadMesh(Stream stream, Vector3 Scale)
        {
            var ModelData = new MeshChunk();

            if (stream.Position >= stream.Length)
            {
                return new MeshChunk();
            }

            stream.Position += 48;

            ModelData.StripCount = StreamUtil.ReadUInt32(stream);
            stream.Position += 4;
            ModelData.VertexCount = StreamUtil.ReadUInt32(stream);
            stream.Position += 4;

            stream.Position += 16;

            //Load Strip Count
            List<int> TempStrips = new List<int>();
            for (int a = 0; a < ModelData.StripCount; a++)
            {
                TempStrips.Add(StreamUtil.ReadInt16(stream) / 3);

            }
            StreamUtil.AlignBy16(stream);

            stream.Position += 16;
            ModelData.Tristrip = TempStrips;

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

            ModelData.TextureCords = UVs;

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
            ModelData.normals = Normals;

            List<Vector3> vertices = new List<Vector3>();
            stream.Position += 16;
            //Load Vertex
            for (int a = 0; a < ModelData.VertexCount; a++)
            {
                Vector3 vertex = new Vector3();
                vertex.X = ((float)StreamUtil.ReadInt16(stream) / 32768f)*Scale.X;
                vertex.Y = ((float)StreamUtil.ReadInt16(stream) / 32768f)*Scale.Y;
                vertex.Z = ((float)StreamUtil.ReadInt16(stream) / 32768f)*Scale.Z;
                vertices.Add(vertex);
            }
            StreamUtil.AlignBy16(stream);
            ModelData.vertices = vertices;
            stream.Position += 16;

            return ModelData;
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
                StreamUtil.WriteInt16(stream, TempPatch.Unknown2); //41
                StreamUtil.WriteInt16(stream, TempPatch.PatchVisablity);
                StreamUtil.WriteInt16(stream, TempPatch.TextureAssigment);
                StreamUtil.WriteInt16(stream, TempPatch.LightmapID);
                StreamUtil.WriteInt32(stream, TempPatch.Unknown4);
                StreamUtil.WriteInt32(stream, TempPatch.Unknown5);
                StreamUtil.WriteInt32(stream, TempPatch.Unknown6);
            }

            //Instances
            InstanceOffset = (int)stream.Position;
            for (int i = 0; i < Instances.Count; i++)
            {
                var TempInstance = Instances[i];
                StreamUtil.WriteMatrix4x4(stream, TempInstance.matrix4X4);
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
                StreamUtil.WriteInt32(stream, TempInstance.ModelID2); //SSF Index?
                StreamUtil.WriteInt32(stream, TempInstance.UnknownInt30);
                StreamUtil.WriteInt32(stream, TempInstance.UnknownInt31);
                StreamUtil.WriteInt32(stream, TempInstance.UnknownInt32);
            }

            //Particle Instances
            ParticleInstancesOffset = (int)stream.Position;
            for (int i = 0; i < particleInstances.Count; i++)
            {
                var TempParticle = particleInstances[i];
                StreamUtil.WriteMatrix4x4(stream, TempParticle.matrix4X4);
                StreamUtil.WriteInt32(stream, TempParticle.UnknownInt1);
                StreamUtil.WriteVector3(stream, TempParticle.LowestXYZ);
                StreamUtil.WriteVector3(stream, TempParticle.HighestXYZ);
                StreamUtil.WriteInt32(stream, TempParticle.UnknownInt8);
                StreamUtil.WriteInt32(stream, TempParticle.UnknownInt9);
                StreamUtil.WriteInt32(stream, TempParticle.UnknownInt10);
                StreamUtil.WriteInt32(stream, TempParticle.UnknownInt11);
                StreamUtil.WriteInt32(stream, TempParticle.UnknownInt12);
            }

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

            //Set Space Aside for model pointers
            ModelPointerOffset = (int)stream.Position;
            PrefabPointers = new List<int>();
            stream.Position += 4 * PrefabData.Count;

            StreamUtil.AlignBy16(stream);

            //FIX Saving
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
                    TempObject.Matrix4x4Offset = -1;
                    if (!TempObject.matrix4X4.Equals(new Matrix4x4()) && TempObject.matrix4X4 != null)
                    {
                        StreamUtil.AlignBy16(stream);
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
                    if(!TempObject.objectData.Equals(null) && !TempObject.objectData.Equals(new ObjectData()))
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

                        //Go back and Write Total Context Offset
                        StreamUtil.WriteInt32(stream, 44);

                        //Go back and Write Mesh Offset
                        int StartMeshOffsets = (int)stream.Position;
                        stream.Position += 4 * TempObject.objectData.MeshOffsets.Count;

                        List<int> MeshOffsets = new List<int>();

                        for (int b = 0; b < TempObject.objectData.MeshOffsets.Count; b++)
                        {
                            MeshOffsets.Add((int)stream.Position);
                            StreamUtil.WriteInt32(stream, 4 * 7);
                            StreamUtil.WriteInt32(stream, TempObject.objectData.MeshOffsets[b].MaterialBlockPos);
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

                //Write AnimData
                for (int a = 0; a < TempPrefab.PrefabObjects.Count; a++)
                {
                    var TempObject = TempPrefab.PrefabObjects[a];
                    TempObject.AnimOffset = 0;
                    if (TempObject.objectAnimation.animationEntries != null)
                    {
                        TempObject.AnimOffset = (int)stream.Position;

                        var TempAnim = TempObject.objectAnimation;

                        StreamUtil.WriteFloat32(stream, TempAnim.U1);
                        StreamUtil.WriteFloat32(stream, TempAnim.U2);
                        StreamUtil.WriteFloat32(stream, TempAnim.U3);
                        StreamUtil.WriteFloat32(stream, TempAnim.U4);
                        StreamUtil.WriteFloat32(stream, TempAnim.U5);
                        StreamUtil.WriteFloat32(stream, TempAnim.U6);

                        StreamUtil.WriteInt32(stream, TempAnim.AnimationAction);

                        StreamUtil.WriteInt32(stream, TempAnim.animationEntries.Count);
                        StreamUtil.WriteInt32(stream, (int)(stream.Position - TempObject.AnimOffset + 4));

                        //Leave Space For Offsets

                        List<int> Offsets = new List<int>();
                        int OffsetPos = (int)stream.Position;
                        stream.Position += 4 * TempAnim.animationEntries.Count;

                        for (int b = 0; b < TempAnim.animationEntries.Count; b++)
                        {
                            Offsets.Add((int)(stream.Position - TempObject.AnimOffset));
                            StreamUtil.WriteInt32(stream, TempAnim.animationEntries[b].animationMaths.Count);
                            StreamUtil.WriteInt32(stream, 8);

                            for (int c = 0; c < TempAnim.animationEntries[b].animationMaths.Count; c++)
                            {
                                StreamUtil.WriteFloat32(stream, TempAnim.animationEntries[b].animationMaths[c].Value1);
                                StreamUtil.WriteFloat32(stream, TempAnim.animationEntries[b].animationMaths[c].Value2);
                                StreamUtil.WriteFloat32(stream, TempAnim.animationEntries[b].animationMaths[c].Value3);
                                StreamUtil.WriteFloat32(stream, TempAnim.animationEntries[b].animationMaths[c].Value4);
                                StreamUtil.WriteFloat32(stream, TempAnim.animationEntries[b].animationMaths[c].Value5);
                                StreamUtil.WriteFloat32(stream, TempAnim.animationEntries[b].animationMaths[c].Value6);
                            }
                        }


                        //Write Offsets
                        int EndAnimPos = (int)stream.Position;
                        stream.Position = OffsetPos;
                        for (int b = 0; b < Offsets.Count; b++)
                        {
                            StreamUtil.WriteInt32(stream, Offsets[b]);
                        }
                        stream.Position = EndAnimPos;

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
                    if (TempPrefab.PrefabObjects[a].ObjectHighOffset != 0)
                    {
                        StreamUtil.WriteInt32(stream, TempPrefab.PrefabObjects[a].ObjectHighOffset - StartPosObject);
                        StreamUtil.WriteInt32(stream, TempPrefab.PrefabObjects[a].ObjectMediumOffset - StartPosObject);
                        StreamUtil.WriteInt32(stream, TempPrefab.PrefabObjects[a].ObjectLowOffset - StartPosObject);
                    }
                    else
                    {
                        StreamUtil.WriteInt32(stream, TempPrefab.PrefabObjects[a].ObjectHighOffset);
                        StreamUtil.WriteInt32(stream, TempPrefab.PrefabObjects[a].ObjectMediumOffset);
                        StreamUtil.WriteInt32(stream, TempPrefab.PrefabObjects[a].ObjectLowOffset);
                    }
                    if (TempPrefab.PrefabObjects[a].AnimOffset != 0)
                    {
                        StreamUtil.WriteInt32(stream, TempPrefab.PrefabObjects[a].AnimOffset - StartPosObject);
                    }
                    else
                    {
                        StreamUtil.WriteInt32(stream, TempPrefab.PrefabObjects[a].AnimOffset);
                    }
                    if (TempPrefab.PrefabObjects[a].Matrix4x4Offset != -1)
                    {
                        StreamUtil.WriteInt32(stream, TempPrefab.PrefabObjects[a].Matrix4x4Offset - StartPosObject);
                    }
                    else
                    {
                        StreamUtil.WriteInt32(stream, TempPrefab.PrefabObjects[a].Matrix4x4Offset);
                    }
                }
                stream.Position = TempPos;


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

            //Make Space For Particle Pointer
            ParticleModelPointerOffset = (int)stream.Position;
            stream.Position += 4 * particleModels.Count;
            ParticleModelPointers = new List<int>();

            //Write Particle Data
            ParticleModelsOffset = (int)stream.Position;
            for (int i = 0; i < particleModels.Count; i++)
            {
                ParticleModelPointers.Add((int)stream.Position - ParticleModelsOffset);
                StreamUtil.WriteInt32(stream, 4 + particleModels[i].bytes.Length);
                StreamUtil.WriteBytes(stream, particleModels[i].bytes);
            }


            //Write ParticlePointer
            TempPosition = (int)stream.Position;
            stream.Position = ParticleModelPointerOffset;
            for (int i = 0; i < ParticleModelPointers.Count; i++)
            {
                StreamUtil.WriteInt32(stream, ParticleModelPointers[i]);
            }
            stream.Position = TempPosition;

            //Make space for Camera Pointer
            CameraPointerOffset = (int)stream.Position;
            stream.Position += 4 * Cameras.Count;
            CameraPointers = new List<int>();

            //Write Camera
            CamerasOffset = (int)stream.Position;
            for (int i = 0; i < Cameras.Count; i++)
            {
                CameraPointers.Add((int)stream.Position- CamerasOffset);
                StreamUtil.WriteInt32(stream,4 + Cameras[i].bytes.Length);
                StreamUtil.WriteBytes(stream, Cameras[i].bytes);
            }

            //Write Camera Pointer
            TempPosition = (int)stream.Position;
            stream.Position = CameraPointerOffset;
            for (int i = 0; i < CameraPointers.Count; i++)
            {
                StreamUtil.WriteInt32(stream, CameraPointers[i]);
            }
            stream.Position = TempPosition;


            //Write Hash Data
            HashOffset = (int)stream.Position;

            StreamUtil.WriteInt32(stream, hashData.bytes.Length + 4);
            StreamUtil.WriteBytes(stream, hashData.bytes);
            StreamUtil.AlignBy16(stream);


            //Write MeshData
            MeshDataOffset = (int)stream.Position;
            StreamUtil.WriteBytes(stream, MeshData);


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

        public void ExportModels(string path)
        {
            //glstHandler.SavePDBModelglTF(path, this);
            for (int a = 0; a < PrefabData.Count; a++)
            {
                for (int ax = 0; ax < PrefabData[a].PrefabObjects.Count; ax++)
                {
                    if (PrefabData[a].PrefabObjects[ax].objectData.MeshOffsets != null)
                    {
                        for (int i = 0; i < PrefabData[a].PrefabObjects[ax].objectData.MeshOffsets.Count; i++)
                        {
                            string outputString = "";
                            string output = "# Exported From SSX Using SSX Multitool Modder by GlitcherOG \n";

                            List<Vector3> vertices = new List<Vector3>();
                            List<Vector3> Normals = new List<Vector3>();
                            List<Vector2> UV = new List<Vector2>();
                            outputString += "o Mesh" + i.ToString() + "\n";
                            var Data = PrefabData[a].PrefabObjects[ax].objectData.MeshOffsets[i].FullMesh;
                            for (int b = 0; b < Data.meshFaces.Count; b++)
                            {
                                var Face = Data.meshFaces[b];

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
                                output += "v " + vertices[z].X + " " + vertices[z].Y + " " + vertices[z].Z + "\n";
                            }
                            for (int z = 0; z < UV.Count; z++)
                            {
                                output += "vt " + UV[z].X + " " + -UV[z].Y + "\n";
                            }
                            for (int z = 0; z < Normals.Count; z++)
                            {
                                output += "vn " + Normals[z].X + " " + Normals[z].Y + " " + Normals[z].Z + "\n";
                            }
                            output += outputString;
                            File.WriteAllText(path + "/" + PrefabData[a].PrefabObjects[ax].objectData.MeshOffsets[i].MeshID.ToString() + ".obj", output);
                        }
                    }
                }

            }
        }

        public void ImportMeshes(string FolderPath)
        {
            MemoryStream memoryStream = new MemoryStream();

            for (int ia = 0; ia < PrefabData.Count; ia++)
            {
                var TempPrefab = PrefabData[ia];
                bool Set = false;
                Vector3 LowestXYZ = new Vector3(0,0,0);
                Vector3 HighestXYZ = new Vector3(0, 0, 0);

                List<Vector3> verticeList = new List<Vector3>();

                for (int az = 0; az < TempPrefab.PrefabObjects.Count; az++)
                {
                    var TempObject = TempPrefab.PrefabObjects[az];
                    if (TempObject.objectData.MeshOffsets != null)
                    {
                        for (int bz = 0; bz < TempObject.objectData.MeshOffsets.Count; bz++)
                        {
                            var TempMeshOffset = TempObject.objectData.MeshOffsets[bz];
                            if (File.Exists(FolderPath + "\\" + TempMeshOffset.MeshPath))
                            {
                                ModelObject modelObject = objHandler.LoadFile(FolderPath + "\\" + TempMeshOffset.MeshPath);
                                TempMeshOffset.FullMesh = objHandler.GenerateTristripDataOneNew(modelObject.Meshs[0]);
                            }

                            if (!TempMeshOffset.FullMesh.Equals(null))
                            {
                                if (TempMeshOffset.FullMesh.meshChunk!=null)
                                {
                                    for (int i = 0; i < TempMeshOffset.FullMesh.meshChunk.Count; i++)
                                    {
                                        for (int a = 0; a < TempMeshOffset.FullMesh.meshChunk[i].vertices.Count; a++)
                                        {
                                            verticeList.Add(TempMeshOffset.FullMesh.meshChunk[i].vertices[a]);
                                            if (!Set)
                                            {
                                                LowestXYZ = TempMeshOffset.FullMesh.meshChunk[i].vertices[a];
                                                HighestXYZ = TempMeshOffset.FullMesh.meshChunk[i].vertices[a];
                                                Set = true;
                                            }
                                            else
                                            {
                                                LowestXYZ = MathUtil.Lowest(LowestXYZ, TempMeshOffset.FullMesh.meshChunk[i].vertices[a]);
                                                HighestXYZ = MathUtil.Highest(HighestXYZ, TempMeshOffset.FullMesh.meshChunk[i].vertices[a]);
                                            }
                                        }
                                    }
                                }
                            }


                            TempObject.objectData.MeshOffsets[bz] = TempMeshOffset;
                        }
                    }

                    TempPrefab.PrefabObjects[az] = TempObject;
                }

                TempPrefab.Scale = new Vector3(1, 1, 1);
                if (LowestXYZ.X < 0)
                {
                    LowestXYZ.X = -LowestXYZ.X;
                }
                if (LowestXYZ.Y < 0)
                {
                    LowestXYZ.Y = -LowestXYZ.Y;
                }
                if (LowestXYZ.Z < 0)
                {
                    LowestXYZ.Z = -LowestXYZ.Z;
                }
                HighestXYZ = MathUtil.Highest(HighestXYZ, LowestXYZ);


                //Calculate Scale
                if (HighestXYZ.X * 32768f > 32768f)
                {
                    while (true)
                    {
                        if ((HighestXYZ.X * 32768f) / TempPrefab.Scale.X > 32768f)
                        {
                            TempPrefab.Scale.X += 0.1f;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (HighestXYZ.Y * 32768f > 32768f)
                {
                    while (true)
                    {
                        if ((HighestXYZ.Y * 32768f) / TempPrefab.Scale.Y > 32768f)
                        {
                            TempPrefab.Scale.Y += 0.1f;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (HighestXYZ.Z * 32768f > 32768f)
                {
                    while (true)
                    {
                        if ((HighestXYZ.Z * 32768f) / TempPrefab.Scale.Z > 32768f)
                        {
                            TempPrefab.Scale.Z += 0.1f;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                //Write Mesh
                for (int az = 0; az < TempPrefab.PrefabObjects.Count; az++)
                {
                    var TempObject = TempPrefab.PrefabObjects[az];
                    if (TempObject.objectData.MeshOffsets != null)
                    {
                        for (int bz = 0; bz < TempObject.objectData.MeshOffsets.Count; bz++)
                        {
                            var TempMeshOffset = TempObject.objectData.MeshOffsets[bz];
                            TempMeshOffset.StartPos = 0;
                            TempMeshOffset.Length1 = 0;
                            TempMeshOffset.Length2 = 0;
                            TempMeshOffset.Length3 = 0;
                            TempMeshOffset.MeshDataLength = 0;
                            if (!TempMeshOffset.FullMesh.Equals(null))
                            {
                                var MeshData = TempMeshOffset.FullMesh;


                                TempMeshOffset.StartPos = (int)memoryStream.Position;
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
                                    StreamUtil.WriteUInt8(memoryStream, TempMeshChunk.vertices.Count);

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

                                    StreamUtil.WriteUInt8(memoryStream, TempMeshChunk.Tristrip.Count);

                                    TempBytes = new byte[1] { 0x66 };
                                    StreamUtil.WriteBytes(memoryStream, TempBytes);

                                    //Tristrip Chunk Generation

                                    for (int i = 0; i < TempMeshChunk.Tristrip.Count; i++)
                                    {
                                        StreamUtil.WriteInt16(memoryStream, TempMeshChunk.Tristrip[i] * 3);
                                    }
                                    StreamUtil.AlignBy16(memoryStream);

                                    //Line 6
                                    StreamUtil.WriteInt24(memoryStream, 1);

                                    StreamUtil.WriteUInt8(memoryStream, 48);

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
                                        StreamUtil.WriteUInt8(memoryStream, 13 + TempMeshChunk.Tristrip.Count);
                                    }
                                    else
                                    {
                                        StreamUtil.WriteUInt8(memoryStream, 2 + TempMeshChunk.Tristrip.Count);
                                    }
                                    StreamUtil.WriteUInt8(memoryStream, 128);
                                    StreamUtil.WriteUInt8(memoryStream, TempMeshChunk.TextureCords.Count);
                                    StreamUtil.WriteUInt8(memoryStream, 117);

                                    //UVCord Save
                                    for (int i = 0; i < TempMeshChunk.TextureCords.Count; i++)
                                    {
                                        StreamUtil.WriteInt16(memoryStream, (int)((TempMeshChunk.TextureCords[i].X) * 4096f));
                                        StreamUtil.WriteInt16(memoryStream, (int)((TempMeshChunk.TextureCords[i].Y) * 4096f));
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
                                    TempBytes = new byte[12] { 0, 0, 0, 0, 0x03, 0x01, 0, 0x01, 0, 0, 0, 0 };
                                    StreamUtil.WriteBytes(memoryStream, TempBytes);
                                    if (FirstChunk)
                                    {
                                        StreamUtil.WriteUInt8(memoryStream, 14 + TempMeshChunk.Tristrip.Count);
                                    }
                                    else
                                    {
                                        StreamUtil.WriteUInt8(memoryStream, 3 + TempMeshChunk.Tristrip.Count);
                                    }
                                    StreamUtil.WriteUInt8(memoryStream, 128);
                                    StreamUtil.WriteUInt8(memoryStream, TempMeshChunk.normals.Count);
                                    StreamUtil.WriteUInt8(memoryStream, 121);
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
                                        StreamUtil.WriteUInt8(memoryStream, 15 + TempMeshChunk.Tristrip.Count);
                                    }
                                    else
                                    {
                                        StreamUtil.WriteUInt8(memoryStream, 4 + TempMeshChunk.Tristrip.Count);
                                    }
                                    StreamUtil.WriteUInt8(memoryStream, 128);
                                    StreamUtil.WriteUInt8(memoryStream, TempMeshChunk.vertices.Count);
                                    StreamUtil.WriteUInt8(memoryStream, 121);

                                    //Vertices Generation
                                    for (int i = 0; i < TempMeshChunk.vertices.Count; i++)
                                    {
                                        StreamUtil.WriteInt16(memoryStream, (int)((TempMeshChunk.vertices[i].X * 32768f) / TempPrefab.Scale.X));
                                        StreamUtil.WriteInt16(memoryStream, (int)((TempMeshChunk.vertices[i].Y * 32768f) / TempPrefab.Scale.Y));
                                        StreamUtil.WriteInt16(memoryStream, (int)((TempMeshChunk.vertices[i].Z * 32768f) / TempPrefab.Scale.Z));
                                    }
                                    StreamUtil.AlignBy16(memoryStream);

                                    #endregion

                                    //Total Model Size
                                    StreamUtil.WriteInt24(memoryStream, 1);

                                    StreamUtil.WriteUInt8(memoryStream, 48);

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

                                long TotalByteLenght = (memoryStream.Position - ModelStart);
                                OldPos = memoryStream.Position;
                                for (int i = 0; i < TotalModelLengthPos.Count; i++)
                                {
                                    memoryStream.Position = TotalModelLengthPos[i];
                                    StreamUtil.WriteInt32(memoryStream, (int)TotalByteLenght);
                                }
                                for (int i = 0; i < TotalModelLengthPos1.Count; i++)
                                {
                                    memoryStream.Position = TotalModelLengthPos1[i];
                                    if(i==0)
                                    {
                                        StreamUtil.WriteInt32(memoryStream, (int)TotalByteLenght + (1) * 16);
                                    }
                                    else
                                    {
                                        StreamUtil.WriteInt32(memoryStream, (int)TotalByteLenght + (2) * 16);
                                    }
                                }
                                memoryStream.Position = OldPos;

                                //Mesh Endbytes
                                TempMeshOffset.Length1 = (int)(memoryStream.Position - TempMeshOffset.StartPos);
                                TempBytes = new byte[16] { 0x01, 0, 0, 0x05, 0, 0, 0, 0x30, 0, 0, 0, 0, 0, 0, 0, 0 };
                                StreamUtil.WriteBytes(memoryStream, TempBytes);
                                TempMeshOffset.Length2 = (int)(memoryStream.Position - TempMeshOffset.StartPos);
                                TempBytes = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xEF, 0xBE, 0xAD, 0xDE };
                                StreamUtil.WriteBytes(memoryStream, TempBytes);
                                TempMeshOffset.Length3 = (int)(memoryStream.Position - TempMeshOffset.StartPos);
                                StreamUtil.WriteBytes(memoryStream, TempBytes);
                                TempMeshOffset.MeshDataLength = (int)(memoryStream.Position - TempMeshOffset.StartPos);

                            }

                            TempObject.objectData.MeshOffsets[bz] = TempMeshOffset;
                        }
                    }

                    TempPrefab.PrefabObjects[az] = TempObject;
                }

                PrefabData[ia] = TempPrefab;
            }
            memoryStream.Position = 0;
            MeshData = StreamUtil.ReadBytes(memoryStream, (int)memoryStream.Length);
        }

        public void RegenerateLowestAndHighest()
        {
            //Patches
            for (int i = 0; i < Patches.Count; i++)
            {
                var TempPatch = Patches[i];


                Patches[i] = TempPatch;
            }

            //Instances
            for (int i = 0; i < Instances.Count; i++)
            {
                var TempInstance = Instances[i];

                var TempPrefabData = PrefabData[TempInstance.ModelID];
                Vector3 LowestXYZ = new Vector3(0, 0, 0);
                Vector3 HighestXYZ = new Vector3(0, 0, 0);
                for (int a = 0; a < TempPrefabData.PrefabObjects.Count; a++)
                {
                    if (TempPrefabData.PrefabObjects[a].objectData.MeshOffsets != null)
                    {
                        LowestXYZ = Vector3.Transform(TempPrefabData.PrefabObjects[a].objectData.MeshOffsets[0].FullMesh.meshChunk[0].vertices[0], TempInstance.matrix4X4);
                        HighestXYZ = Vector3.Transform(TempPrefabData.PrefabObjects[a].objectData.MeshOffsets[0].FullMesh.meshChunk[0].vertices[0], TempInstance.matrix4X4);
                    }
                }
                for (int a = 0; a < TempPrefabData.PrefabObjects.Count; a++)
                {
                    if (TempPrefabData.PrefabObjects[a].objectData.MeshOffsets != null)
                    {
                        for (int b = 0; b < TempPrefabData.PrefabObjects[a].objectData.MeshOffsets.Count; b++)
                        {
                            for (int c = 0; c < TempPrefabData.PrefabObjects[a].objectData.MeshOffsets[b].FullMesh.meshChunk.Count; c++)
                            {
                                for (int d = 0; d < TempPrefabData.PrefabObjects[a].objectData.MeshOffsets[b].FullMesh.meshChunk[c].vertices.Count; d++)
                                {
                                    var Vertice = Vector3.Transform(TempPrefabData.PrefabObjects[a].objectData.MeshOffsets[b].FullMesh.meshChunk[c].vertices[d], TempInstance.matrix4X4);

                                    LowestXYZ = MathUtil.Lowest(LowestXYZ, Vertice);
                                    HighestXYZ = MathUtil.Highest(HighestXYZ, Vertice);
                                }
                            }
                        }
                    }
                }

                TempInstance.HighestXYZ = HighestXYZ;
                TempInstance.LowestXYZ = LowestXYZ;

                Instances[i] = TempInstance;
            }
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
        public ObjectAnimation objectAnimation;
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
    }

    public struct MeshOffsets
    {
        public string MeshPath;
        public int MeshID;

        public int EntryLength;
        public int MaterialBlockPos;
        public int MeshDataLength;
        public int StartPos;
        public int Length1;
        public int Length2;
        public int Length3;

        public Mesh FullMesh;
    }

    public struct ObjectAnimation
    {
        //Start AnimPos/Rot?
        public float U1;
        public float U2;
        public float U3;
        public float U4;
        public float U5;
        public float U6;

        public int AnimationAction; //Stored as Bit Flags
                                    //1 - X Position
                                    //2 - Y Position
                                    //4 - Z Position
                                    //8 - X Rotate
                                    //16 - Y Rotate
                                    //32 - Z Rotate

        public int OffsetCount;
        public int OffsetStart;
        public List<int> Offsets;
        public List<AnimationEntry> animationEntries; //Probably Frames
    }

    public struct AnimationEntry
    {
        public int MathCount;
        public int MathOffset;

        public List<AnimationMath> animationMaths;
    }

    public struct AnimationMath
    {
        public float Value1;
        public float Value2;
        public float Value3;
        public float Value4;
        public float Value5;
        public float Value6;
    }

    public struct ParticleModel
    {
        public int TotalLength;
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
        public Matrix4x4 matrix4X4;
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
        public Matrix4x4 matrix4X4;
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

        public int CameraLength;
        public Vector3 Translation;
        public Vector3 Rotation;
        public int Type;
        public int FocalLength;
        public float AspectRatio;
        public Vector2 Aperture;
        public Vector2 ClipPlane;
        public Vector3 IntrestPoint;
        public Vector3 UpVector;

    }

    public struct HashData
    {
        public int TotalLength;
        public byte[] bytes;
    }

    public struct Patch
    {
        public Vector4 LightMapPoint;

        public Vector4 UVPoint1;
        public Vector4 UVPoint2;
        public Vector4 UVPoint3;
        public Vector4 UVPoint4;

        //public List<Vector4> Points;

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

        public int Unknown2;
        public int PatchVisablity;
        public int TextureAssigment; // Texture Assigment 
        public int LightmapID;
        public int Unknown4; //Negative one
        public int Unknown5; //Same
        public int Unknown6; //Same
    }
}
