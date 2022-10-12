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
        public int ParticleModelCount;
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
        public int ModelDataOffset;
        public int Unknown34;
        public int Unknown35;

        public List<Patch> Patches;
        public List<Spline> splines;
        public List<SplinesSegments> splinesSegments;
        public List<TextureFlipbook> textureFlipbooks;
        public List<Instance> Instances;
        public List<ParticleInstance> particleInstances;
        public List<TrickyMaterial> materials;
        public List<MaterialBlock> materialBlocks;
        public List<Light> lights;
        public List<int> ModelPointers;
        public List<Model> modelHeaders;
        public List<int> ParticleModelPointers;
        public List<ParticleModel> particleModels;
        public List<int> CameraPointers;
        public List<Camera> Cameras = new List<Camera>();

        public List<MeshData> models;

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
                ParticleModelCount = StreamUtil.ReadInt32(stream); //Done
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
                ModelsOffset = StreamUtil.ReadInt32(stream); //Sort of Loading
                ParticleModelPointerOffset = StreamUtil.ReadInt32(stream); //Done
                ParticleModelsOffset = StreamUtil.ReadInt32(stream); //Sort of Loading
                CameraPointerOffset = StreamUtil.ReadInt32(stream); //Done
                CamerasOffset = StreamUtil.ReadInt32(stream); //Unknown
                HashOffset = StreamUtil.ReadInt32(stream);
                ModelDataOffset = StreamUtil.ReadInt32(stream); //Loading
                Unknown34 = StreamUtil.ReadInt32(stream); //Possibly Just Blank
                Unknown35 = StreamUtil.ReadInt32(stream); //Possibly Just Blank

                //Patch Loading
                stream.Position = PatchOffset;
                Patches = new List<Patch>();
                for (int i = 0; i < NumPatches; i++)
                {
                    Patch patch = LoadPatch(stream);
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
                    TempLights.UnknownInt1 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt2 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt3 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt4 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt5 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt6 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt7 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt8 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt9 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt10 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt11 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt12 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt13 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt14 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt15 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt16 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt17 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt18 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt19 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt20 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt21 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt22 = StreamUtil.ReadInt32(stream);
                    TempLights.UnknownInt23 = StreamUtil.ReadInt32(stream);
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
                ModelPointers = new List<int>();
                for (int i = 0; i < NumModels; i++)
                {
                    ModelPointers.Add(StreamUtil.ReadInt32(stream));
                }

                //ModelHeaders
                int test = 0;
                stream.Position = ModelsOffset;
                modelHeaders = new List<Model>();
                for (int i = 0; i < ModelPointers.Count; i++)
                {
                    stream.Position = ModelsOffset + ModelPointers[i];
                    var TempHeader = new Model();
                    TempHeader.TotalLength = StreamUtil.ReadInt32(stream);
                    TempHeader.Unknown0 = StreamUtil.ReadInt32(stream);
                    TempHeader.Unknown1 = StreamUtil.ReadInt32(stream);
                    TempHeader.Unknown2 = StreamUtil.ReadInt32(stream);
                    TempHeader.Unknown3 = StreamUtil.ReadInt32(stream);
                    TempHeader.Unknown4 = StreamUtil.ReadFloat(stream);
                    TempHeader.scale = StreamUtil.ReadVector3(stream);
                    TempHeader.ModelDataCount = StreamUtil.ReadInt32(stream);
                    TempHeader.Unknown9 = StreamUtil.ReadInt32(stream);
                    TempHeader.TriStripCount = StreamUtil.ReadInt32(stream);
                    TempHeader.VertexCount = StreamUtil.ReadInt32(stream);
                    TempHeader.Unknown12 = StreamUtil.ReadInt32(stream); //Non-Triangle
                    TempHeader.Unknown13 = StreamUtil.ReadInt32(stream);
                    TempHeader.Unknown14 = StreamUtil.ReadInt32(stream);
                    TempHeader.Unknown15 = StreamUtil.ReadInt32(stream);
                    TempHeader.Unknown16 = StreamUtil.ReadInt32(stream);
                    TempHeader.Unknown17 = StreamUtil.ReadInt32(stream);
                    TempHeader.Unknown18 = StreamUtil.ReadInt32(stream);

                    TempHeader.UnknownLength = StreamUtil.ReadInt32(stream);

                    TempHeader.LowestXYZ = StreamUtil.ReadVector3(stream);
                    TempHeader.HighestXYZ = StreamUtil.ReadVector3(stream);

                    //Unknown (Blank?)

                    //EntryCount
                    //Face Count
                    //Unknown

                    //By EntryCount
                    //Unknown

                    //By EntryCount
                    //Entry Length
                    //MeshID only out of positions starting from 0
                    //LengthFac
                    //StartPos
                    //Lenght-3*16
                    //Lenght-2*16
                    //Lenght-1*16

                    int TempInt = TempHeader.UnknownLength;
                    if (TempHeader.UnknownLength - 24 > 0)
                    {
                        TempInt = TempHeader.UnknownLength - 24;
                    }

                    TempHeader.bytes = StreamUtil.ReadBytes(stream, TempInt);
                    modelHeaders.Add(TempHeader);

                    test += TempHeader.ModelDataCount;

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
                for (int i = 0; i < ParticleModelCount; i++)
                {
                    stream.Position = ParticleModelsOffset + ParticleModelPointers[i];
                    ParticleModel TempParticleModel = new ParticleModel();
                    TempParticleModel.TotalLength = StreamUtil.ReadInt32(stream);
                    TempParticleModel.Unknown0 = StreamUtil.ReadInt32(stream);
                    TempParticleModel.Unknown1 = StreamUtil.ReadInt32(stream);
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

                //ModelData
                stream.Position = ModelDataOffset;
                models = new List<MeshData>();
                MeshData model = new MeshData();
                model.staticMeshes = new List<StaticMesh>();
                int count = 0;

                while (true)
                {
                    var temp = ReadMesh(stream);
                    if (temp.Equals(new StaticMesh()))
                    {
                        break;
                    }
                    count++;
                    model.staticMeshes.Add(GenerateFaces(temp));
                    stream.Position += 31;
                    if (StreamUtil.ReadByte(stream) == 0x6C)
                    {
                        stream.Position -= 32;
                    }
                    else
                    {
                        stream.Position += 48;
                        models.Add(model);
                        model = new MeshData();
                        model.staticMeshes = new List<StaticMesh>();
                    }
                }
            }
        }
        public void Save(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                MagicBytes = StreamUtil.ReadBytes(stream, 4);
                NumPlayerStarts = StreamUtil.ReadInt32(stream);

                StreamUtil.WriteInt32(stream, Patches.Count);
                StreamUtil.WriteInt32(stream, Instances.Count);
                StreamUtil.WriteInt32(stream, particleInstances.Count);

                NumMaterials = StreamUtil.ReadInt32(stream);
                NumMaterialBlocks = StreamUtil.ReadInt32(stream);
                NumLights = StreamUtil.ReadInt32(stream);

                StreamUtil.WriteInt32(stream, splines.Count);
                StreamUtil.WriteInt32(stream, splinesSegments.Count);
                StreamUtil.WriteInt32(stream, textureFlipbooks.Count);

                NumModels = StreamUtil.ReadInt32(stream);
                ParticleModelCount = StreamUtil.ReadInt32(stream);

                StreamUtil.WriteInt32(stream, NumTextures);

                NumCameras = StreamUtil.ReadInt32(stream); //Used in SSXFE MAP

                StreamUtil.WriteInt32(stream, 0); //Lightmap size 

                PlayerStartOffset = StreamUtil.ReadInt32(stream);
                PatchOffset = StreamUtil.ReadInt32(stream); //Done Need to make custom write later
                InstanceOffset = StreamUtil.ReadInt32(stream); //Done Need to make custom write later
                ParticleInstancesOffset = StreamUtil.ReadInt32(stream); //Done Need to make custom write later
                MaterialOffset = StreamUtil.ReadInt32(stream);
                MaterialBlocksOffset = StreamUtil.ReadInt32(stream);
                LightsOffset = StreamUtil.ReadInt32(stream);
                SplineOffset = StreamUtil.ReadInt32(stream); //Done Need to make custom write later
                SplineSegmentOffset = StreamUtil.ReadInt32(stream); //Done Need to make custom write later
                TextureFlipbookOffset = StreamUtil.ReadInt32(stream); //Done Need to make custom write later
                ModelPointerOffset = StreamUtil.ReadInt32(stream);
                ModelsOffset = StreamUtil.ReadInt32(stream);

                ParticleModelPointerOffset = StreamUtil.ReadInt32(stream);
                ParticleModelsOffset = StreamUtil.ReadInt32(stream);
                CameraPointerOffset = StreamUtil.ReadInt32(stream);
                CamerasOffset = StreamUtil.ReadInt32(stream);
                HashOffset = StreamUtil.ReadInt32(stream);
                ModelDataOffset = StreamUtil.ReadInt32(stream);
                Unknown34 = StreamUtil.ReadInt32(stream);
                Unknown35 = StreamUtil.ReadInt32(stream);

                //Patches
                stream.Position = PatchOffset;
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

                //Instances
                stream.Position = InstanceOffset;
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
            }
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

        public Patch LoadPatch(Stream stream)
        {
            Patch face = new Patch();

            face.LightMapPoint = StreamUtil.ReadVector4(stream);

            face.UVPoint1 = StreamUtil.ReadVector4(stream);
            face.UVPoint2 = StreamUtil.ReadVector4(stream);
            face.UVPoint3 = StreamUtil.ReadVector4(stream);
            face.UVPoint4 = StreamUtil.ReadVector4(stream);

            face.R4C4 = StreamUtil.ReadVector4(stream);
            face.R4C3 = StreamUtil.ReadVector4(stream);
            face.R4C2 = StreamUtil.ReadVector4(stream);
            face.R4C1 = StreamUtil.ReadVector4(stream);
            face.R3C4 = StreamUtil.ReadVector4(stream);
            face.R3C3 = StreamUtil.ReadVector4(stream);
            face.R3C2 = StreamUtil.ReadVector4(stream);
            face.R3C1 = StreamUtil.ReadVector4(stream);
            face.R2C4 = StreamUtil.ReadVector4(stream);
            face.R2C3 = StreamUtil.ReadVector4(stream);
            face.R2C2 = StreamUtil.ReadVector4(stream);
            face.R2C1 = StreamUtil.ReadVector4(stream);
            face.R1C4 = StreamUtil.ReadVector4(stream);
            face.R1C3 = StreamUtil.ReadVector4(stream);
            face.R1C2 = StreamUtil.ReadVector4(stream);
            face.R1C1 = StreamUtil.ReadVector4(stream);

            face.LowestXYZ = StreamUtil.ReadVector3(stream);
            face.HighestXYZ = StreamUtil.ReadVector3(stream);

            face.Point1 = StreamUtil.ReadVector4(stream);
            face.Point2 = StreamUtil.ReadVector4(stream);
            face.Point3 = StreamUtil.ReadVector4(stream);
            face.Point4 = StreamUtil.ReadVector4(stream);

            face.PatchStyle = StreamUtil.ReadInt32(stream);
            face.Unknown2 = StreamUtil.ReadInt32(stream); //Material/Lighting
            face.TextureAssigment = StreamUtil.ReadInt16(stream);

            face.LightmapID = StreamUtil.ReadInt16(stream);

            //Always the same
            face.Unknown4 = StreamUtil.ReadInt32(stream); //Negitive one
            face.Unknown5 = StreamUtil.ReadInt32(stream);
            face.Unknown6 = StreamUtil.ReadInt32(stream);

            return face;
        }


        public void ExportModels(string path)
        {
            //glstHandler.SavePDBModelglTF(path, this);

            int Pos1 = 0;
            for (int a = 0; a < modelHeaders.Count; a++)
            {
                int VStartPoint = 0;
                int NStartPoint = 0;
                int UStartPoint = 0;
                int StartPos = Pos1;
                string output = "# Exported From SSX Using SSX Multitool Modder by GlitcherOG \n";
                for (int ax = Pos1; ax < StartPos + modelHeaders[a].ModelDataCount; ax++)
                {
                    output += "o Mesh" + ax.ToString() + "\n";
                    string outputString = "";
                    List<Vector3> vertices = new List<Vector3>();
                    List<Vector3> Normals = new List<Vector3>();
                    List<Vector2> UV = new List<Vector2>();
                    Vector3 Scale = modelHeaders[a].scale;
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
                    for (int i = 0; i < models[ax].staticMeshes.Count; i++)
                    {
                        var Data = models[ax].staticMeshes[i];
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
                    Pos1++;
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
                            StreamUtil.WriteInt16(memoryStream, (int)((TempMeshChunk.vertices[i].X * 32768f )/ modelHeaders[a].scale.X));
                            StreamUtil.WriteInt16(memoryStream, (int)((TempMeshChunk.vertices[i].Y * 32768f )/ modelHeaders[a].scale.Y));
                            StreamUtil.WriteInt16(memoryStream, (int)((TempMeshChunk.vertices[i].Z * 32768f )/ modelHeaders[a].scale.Z));
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

    public struct Model
    {
        public int TotalLength;
        public int Unknown0;
        public int Unknown1;
        public int Unknown2;
        public int Unknown3; //ID
        public float Unknown4;
        public Vector3 scale;
        public int ModelDataCount;
        public int Unknown9;
        public int TriStripCount; //Tristrip Count
        public int VertexCount; //Vertex Count
        public int Unknown12;
        public int Unknown13;
        public int Unknown14;
        public int Unknown15;
        public int Unknown16;
        public int Unknown17;
        public int Unknown18;

        public int UnknownLength;

        public Vector3 LowestXYZ;
        public Vector3 HighestXYZ;

        public byte[] bytes;
    }

    public struct ParticleModel
    {
        public int TotalLength;
        public int Unknown0;
        public int Unknown1;
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
        public int UnknownInt1;
        public int UnknownInt2;
        public int UnknownInt3;
        public int UnknownInt4;
        public int UnknownInt5;
        public int UnknownInt6;
        public int UnknownInt7;
        public int UnknownInt8;
        public int UnknownInt9;
        public int UnknownInt10;
        public int UnknownInt11;
        public int UnknownInt12;
        public int UnknownInt13;
        public int UnknownInt14;
        public int UnknownInt15;
        public int UnknownInt16;
        public int UnknownInt17;
        public int UnknownInt18;
        public int UnknownInt19;
        public int UnknownInt20;
        public int UnknownInt21;
        public int UnknownInt22;
        public int UnknownInt23;
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
