using SSXMultiTool.JsonFiles.SSX3;
using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SSXMultiTool.FileHandlers.LevelFiles.SSX3PS2.SSBData
{
    internal class WorldAIP
    {
        public int MagicWords;
        public int NumAIPaths;
        public int NumTrackPaths;
        public int NumU0;
        public int NumU1;

        public List<AIPath> aiPaths = new List<AIPath>();
        public List<TrackPath> trackPaths = new List<TrackPath>();
        public List<U0Struct> u0Structs = new List<U0Struct>();
        public List<U1Struct> u1Structs = new List<U1Struct>();


        public void LoadData(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream();
            StreamUtil.WriteBytes(stream, bytes);
            stream.Position = 0;

            MagicWords = StreamUtil.ReadInt32(stream);
            NumAIPaths = StreamUtil.ReadInt32(stream);

            aiPaths = new List<AIPath>();

            for (int i = 0; i < NumAIPaths; i++)
            {
                var TempAIPath = new AIPath();

                TempAIPath.U0 = StreamUtil.ReadUInt32(stream);
                TempAIPath.U1 = StreamUtil.ReadUInt32(stream);
                TempAIPath.U2 = StreamUtil.ReadUInt32(stream);
                TempAIPath.U3 = StreamUtil.ReadUInt32(stream);
                TempAIPath.U4 = StreamUtil.ReadUInt32(stream);
                TempAIPath.U5 = StreamUtil.ReadUInt32(stream);
                TempAIPath.U6 = StreamUtil.ReadUInt32(stream);
                TempAIPath.NumPoints = StreamUtil.ReadUInt32(stream);
                TempAIPath.NumEvents = StreamUtil.ReadUInt32(stream);

                TempAIPath.PathPos = StreamUtil.ReadVector3(stream);
                TempAIPath.BBoxMin = StreamUtil.ReadVector3(stream);
                TempAIPath.BBoxMax = StreamUtil.ReadVector3(stream);

                TempAIPath.VectorPoints = new List<Vector4>();

                for (global::System.Int32 j = 0; j < TempAIPath.NumPoints; j++)
                {
                    TempAIPath.VectorPoints.Add(StreamUtil.ReadVector4(stream));
                }

                TempAIPath.PathEvents = new List<PathEvent>();
                for (int a = 0; a < TempAIPath.NumEvents; a++)
                {
                    var NewUStrcut = new PathEvent();
                    NewUStrcut.EventType = StreamUtil.ReadUInt32(stream);
                    NewUStrcut.EventValue = StreamUtil.ReadUInt32(stream);
                    NewUStrcut.EventStart = StreamUtil.ReadFloat(stream);
                    NewUStrcut.EventEnd = StreamUtil.ReadFloat(stream);
                    TempAIPath.PathEvents.Add(NewUStrcut);
                }

                aiPaths.Add(TempAIPath);
            }

            NumTrackPaths = StreamUtil.ReadUInt32(stream);

            trackPaths = new List<TrackPath>();
            for (int i = 0; i < NumTrackPaths; i++)
            {
                var NewPath = new TrackPath();
                NewPath.Type = StreamUtil.ReadUInt32(stream);
                NewPath.U0 = StreamUtil.ReadUInt32(stream);
                NewPath.U1 = StreamUtil.ReadUInt32(stream);
                NewPath.U2 = StreamUtil.ReadFloat(stream);

                NewPath.NumPoints = StreamUtil.ReadUInt32(stream);
                NewPath.NumEvents = StreamUtil.ReadUInt32(stream);

                NewPath.PathPos = StreamUtil.ReadVector3(stream);
                NewPath.BBoxMin = StreamUtil.ReadVector3(stream);
                NewPath.BBoxMax = StreamUtil.ReadVector3(stream);

                NewPath.VectorPoints = new List<Vector4>();
                for (int a = 0; a < NewPath.NumPoints; a++)
                {
                    NewPath.VectorPoints.Add(StreamUtil.ReadVector4(stream));
                }

                NewPath.PathEvents = new List<PathEvent>();
                for (int a = 0; a < NewPath.NumEvents; a++)
                {
                    var NewUStrcut = new PathEvent();
                    NewUStrcut.EventType = StreamUtil.ReadUInt32(stream);
                    NewUStrcut.EventValue = StreamUtil.ReadUInt32(stream);
                    NewUStrcut.EventStart = StreamUtil.ReadFloat(stream);
                    NewUStrcut.EventEnd = StreamUtil.ReadFloat(stream);
                    NewPath.PathEvents.Add(NewUStrcut);
                }

                trackPaths.Add(NewPath);
            }

            NumU0 = StreamUtil.ReadUInt32(stream);
            u0Structs = new List<U0Struct>();

            for (int i = 0; i < NumU0; i++)
            {
                U0Struct TempU0Struct = new U0Struct();

                TempU0Struct.U0 = StreamUtil.ReadUInt32(stream);
                TempU0Struct.U1 = StreamUtil.ReadUInt32(stream);

                u0Structs.Add(TempU0Struct);
            }

            NumU1 = StreamUtil.ReadUInt32(stream);
            u1Structs = new List<U1Struct>();

            for (int i = 0; i < NumU1; i++)
            {
                U1Struct TempU1Struct = new U1Struct();

                TempU1Struct.U0 = StreamUtil.ReadUInt32(stream);
                TempU1Struct.U1 = StreamUtil.ReadUInt32(stream);
                TempU1Struct.U2 = StreamUtil.ReadVector3(stream);
                TempU1Struct.U3 = StreamUtil.ReadVector3(stream);
                TempU1Struct.U4 = StreamUtil.ReadUInt32(stream);
                TempU1Struct.U5 = StreamUtil.ReadUInt32(stream);

                u1Structs.Add(TempU1Struct);
            }
        }

        public void ToJson(string path)
        {
            AIPJsonHandler aipJsonHandler = new AIPJsonHandler();

            aipJsonHandler.aiPaths = new List<AIPJsonHandler.AIPath>();

            for (int i = 0; i < aiPaths.Count; i++)
            {
                var newAIPath = new AIPJsonHandler.AIPath();

                newAIPath.Name = "AI Path " +i.ToString();

                newAIPath.U0 = aiPaths[i].U0;
                newAIPath.U1 = aiPaths[i].U1;
                newAIPath.U2 = aiPaths[i].U2;
                newAIPath.U3 = aiPaths[i].U3;
                newAIPath.U4 = aiPaths[i].U4;
                newAIPath.U5 = aiPaths[i].U5;
                newAIPath.U6 = aiPaths[i].U6;

                newAIPath.PathPos = JsonUtil.Vector3ToArray(aiPaths[i].PathPos);
                newAIPath.PathPoints = new float[aiPaths[i].VectorPoints.Count, 3];

                for (int a = 0; a < aiPaths[i].VectorPoints.Count; a++)
                {
                    newAIPath.PathPoints[a, 0] = aiPaths[i].VectorPoints[a].X * aiPaths[i].VectorPoints[a].W;
                    newAIPath.PathPoints[a, 1] = aiPaths[i].VectorPoints[a].Y * aiPaths[i].VectorPoints[a].W;
                    newAIPath.PathPoints[a, 2] = aiPaths[i].VectorPoints[a].Z * aiPaths[i].VectorPoints[a].W;
                }

                newAIPath.PathEvents = new List<AIPJsonHandler.PathEvent>();
                for (int a = 0; a < aiPaths[i].PathEvents.Count; a++)
                {
                    var NewStruct = new AIPJsonHandler.PathEvent();
                    NewStruct.EventType = aiPaths[i].PathEvents[a].EventType;
                    NewStruct.EventValue = aiPaths[i].PathEvents[a].EventValue;
                    NewStruct.EventStart = aiPaths[i].PathEvents[a].EventStart;
                    NewStruct.EventEnd = aiPaths[i].PathEvents[a].EventEnd;
                    newAIPath.PathEvents.Add(NewStruct);
                }

                aipJsonHandler.aiPaths.Add(newAIPath);
            }

            aipJsonHandler.trackPaths = new List<AIPJsonHandler.TrackPath>();

            for (int i = 0; i < trackPaths.Count; i++)
            {
                var newAIPath = new AIPJsonHandler.TrackPath();

                newAIPath.Name = "Track Path " + i.ToString();

                newAIPath.Type = trackPaths[i].Type;
                newAIPath.U0 = trackPaths[i].U0;
                newAIPath.U1 = trackPaths[i].U1;
                newAIPath.U2 = trackPaths[i].U2;

                newAIPath.PathPos = JsonUtil.Vector3ToArray(trackPaths[i].PathPos);
                newAIPath.PathPoints = new float[trackPaths[i].VectorPoints.Count, 3];

                for (int a = 0; a < trackPaths[i].VectorPoints.Count; a++)
                {
                    newAIPath.PathPoints[a, 0] = trackPaths[i].VectorPoints[a].X * trackPaths[i].VectorPoints[a].W;
                    newAIPath.PathPoints[a, 1] = trackPaths[i].VectorPoints[a].Y * trackPaths[i].VectorPoints[a].W;
                    newAIPath.PathPoints[a, 2] = trackPaths[i].VectorPoints[a].Z * trackPaths[i].VectorPoints[a].W;
                }

                newAIPath.PathEvents = new List<AIPJsonHandler.PathEvent>();
                for (int a = 0; a < trackPaths[i].PathEvents.Count; a++)
                {
                    var NewStruct = new AIPJsonHandler.PathEvent();
                    NewStruct.EventType = trackPaths[i].PathEvents[a].EventType;
                    NewStruct.EventValue = trackPaths[i].PathEvents[a].EventValue;
                    NewStruct.EventStart = trackPaths[i].PathEvents[a].EventStart;
                    NewStruct.EventEnd = trackPaths[i].PathEvents[a].EventEnd;
                    newAIPath.PathEvents.Add(NewStruct);
                }

                aipJsonHandler.trackPaths.Add(newAIPath);
            }

            aipJsonHandler.u0Structs = new List<AIPJsonHandler.U0Struct>();

            for (int i = 0; i < u0Structs.Count; i++)
            {
                var NewU0struct = new AIPJsonHandler.U0Struct();

                NewU0struct.U0 = u0Structs[i].U0;
                NewU0struct.U1 = u0Structs[i].U1;

                aipJsonHandler.u0Structs.Add(NewU0struct);
            }

            aipJsonHandler.u1Structs = new List<AIPJsonHandler.U1Struct>();

            for (int i = 0; i < u1Structs.Count; i++)
            {
                var NewU1struct = new AIPJsonHandler.U1Struct();

                NewU1struct.U0 = u1Structs[i].U0;
                NewU1struct.U1 = u1Structs[i].U1;
                NewU1struct.U2 = JsonUtil.Vector3ToArray(u1Structs[i].U2);
                NewU1struct.U3 = JsonUtil.Vector3ToArray(u1Structs[i].U3);
                NewU1struct.U0 = u1Structs[i].U4;
                NewU1struct.U1 = u1Structs[i].U5;

                aipJsonHandler.u1Structs.Add(NewU1struct);
            }

            aipJsonHandler.CreateJson(path);
        }

        public struct AIPath
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;

            public int NumPoints;
            public int NumEvents;

            public Vector3 PathPos;
            public Vector3 BBoxMin;
            public Vector3 BBoxMax;

            public List<Vector4> VectorPoints;
            public List<PathEvent> PathEvents;
        }

        public struct TrackPath
        {
            public int Type;
            public int U0;
            public int U1;
            public float U2;

            public int NumPoints;
            public int NumEvents;

            public Vector3 PathPos;
            public Vector3 BBoxMin;
            public Vector3 BBoxMax;

            public List<Vector4> VectorPoints;
            public List<PathEvent> PathEvents;
        }

        public struct PathEvent
        {
            public int EventType;
            public int EventValue;
            public float EventStart;
            public float EventEnd;
        }

        public struct U0Struct
        {
            public int U0; 
            public int U1;
        }

        public struct U1Struct
        {
            public int U0;
            public int U1;
            public Vector3 U2;
            public Vector3 U3;
            public int U4;
            public int U5;
        }
    }
}
