using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SSXMultiTool.JsonFiles.Tricky
{
    public class SSFJsonHandler
    {
        public List<EffectSlotJson> EffectSlots = new List<EffectSlotJson>();
        public List<PhysicsHeader> PhysicsHeaders = new List<PhysicsHeader>();
        public List<Function> Functions = new List<Function>();
        public List<EffectHeaderStruct> EffectHeaders = new List<EffectHeaderStruct>();

        public void CreateJson(string path, bool Inline = false)
        {
            var TempFormating = Formatting.None;
            if (Inline)
            {
                TempFormating = Formatting.Indented;
            }

            var serializer = JsonConvert.SerializeObject(this, TempFormating, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText(path, serializer);
        }

        public static InstanceJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonConvert.DeserializeObject<InstanceJsonHandler>(stream);
                return container;
            }
            else
            {
                return new InstanceJsonHandler();
            }
        }

        public struct EffectSlotJson
        {
            public string EffectSlotName;

            public int PersistantEffectSlot;
            public int CollisionEffectSlot;
            public int Slot3;
            public int Slot4;
            public int EffectTriggerSlot;
            public int Slot6;
            public int Slot7;
        }

        public struct PhysicsHeader
        {
            public string PhysicsName;
            public List<PhysicsData> PhysicsDatas;
        }

        public struct PhysicsData
        {
            public float UFloat0;
            public float UFloat1;
            public float UFloat2;
            public float UFloat3;
            public float UFloat4;
            public float UFloat5;
            public float UFloat6;
            public float UFloat7;
            public float UFloat8;
            public float UFloat9;
            public float UFloat10;
            public float UFloat11;
            public float UFloat12;
            public float UFloat13;
            public float UFloat14;
            public float UFloat15;
            public float UFloat16;
            public float UFloat17;
            public float UFloat18;
            public float UFloat19;
            public float UFloat20;
            public float UFloat21;
            public float UFloat22;
            public float UFloat23;

            public List<UPhysicsStruct> uPhysicsStruct0;

            public byte[] UByteData;
            public byte[] UByteEndData;
        }

        public struct UPhysicsStruct
        {
            public float U0;
            public float U1;
            public int U2;
        }

        public struct EffectHeaderStruct
        {
            public string EffectName;
            public List<Effect> Effects;
        }

        public struct Function
        {
            public string FunctionName;
            public List<Effect> Effects;
        }

        #region Effects
        public struct Effect
        {
            public int MainType;

            public Type0? type0;
            public Type2? type2;
            public Type3? type3;
            public float WaitTime;
            public Type5? type5;
            public InstanceEffect? Instance;
            public int SoundPlay;
            public Type9? type9;

            public float type13;
            public float MultiplierScore;
            public float type17;
            public float type18;
            public int FunctionRunIndex;
            public int TeleportInstanceIndex;
            public SplineEffect? Spline;
        }

        #region Type0
        public struct Type0
        {
            public int SubType;

            public Type0Sub0? type0Sub0; ///Roller?
            public int type0Sub2;  //Debounce?
            public int DeadNodeMode; //5
            public CounterEffect? Counter; //6
            public Type0Sub7? type0Sub7; //Boost
            public UVScrolling? UVScroll; //10
            public TextureFlipEffect? TextureFlip; //11
            public FenceFlex? Fence; //12
            public Type0Sub13? type0Sub13; //Flag
            public Type0Sub14? type0Sub14; //Cracked
            public Type0Sub15? type0Sub15; //LapBoost
            public CrowdBox? CrowdEffect; //17
            public Type0Sub18? type0Sub18; //ZBoost
            public Type0Sub20? type0Sub20; //cMeshAnim
            public Type0Sub24? type0Sub24; //TubeEndBoost
            public Type0Sub256? type0Sub256; //AnimObject
            public Type0Sub257? type0Sub257; //AnimDelta
            public Type0Sub258? type0Sub258; //AnimCombo
        }

        public struct Type0Sub0
        {
            public float U0;
            public float U1;
            public float U2;
            public float U3;
            public float U4;
            public float U5;
        }

        public struct CounterEffect
        {
            public int Count;
            public float U1;
        }

        public struct Type0Sub7
        {
            public int U0;
            public int U1;
            public float U2;
            public float U3;
            public float U4;
            public int U5;
            public float U6;
        }

        public struct UVScrolling
        {
            public int U0;
            public float U1;
            public float U2;
            public float U3;
            public float U4;
            public int U5;
        }

        public struct TextureFlipEffect
        {
            public int U0;
            public int Direction;
            public float Speed;
            public float Length;
            public int U4;
        }

        public struct FenceFlex
        {
            public int U0;
            public float FlexAmmount;
        }

        public struct Type0Sub13
        {
            public int U0;
            public float U1;
            public float U2;
            public int U3;
        }

        public struct Type0Sub14
        {
            public float U0;
            public float U1;
        }

        public struct Type0Sub15
        {
            public float U0;
            public float U1;
            public int U2;
            public int U3;
            public float U4;
        }

        public struct CrowdBox
        {
            public int U0;
            public int U1;
            public int U2;
        }

        public struct Type0Sub18
        {
            public float U0;
            public float U1;
            public int U2;
            public int U3;
            public float U4;
            public float U5;
            public float U6;
        }

        public struct Type0Sub20
        {
            public int U0;
            public float U1;
            public float U2;
            public int U3;
            public int U4;
            public int U5;
            public float U6;
            public float U7;
            public float U8;
            public float U9;
        }

        public struct Type0Sub24
        {
            public int U0;
            public int U1;
            public float U2;
            public float U3;
            public int U4;
            public int U5;
            public int U6;
            public float U7;
            public float U8;
            public float U9;
            public float U10;
            public float U11;
            public float U12;
            public float U13;
            public float U14;
            public float U15;
            public float U16;
            public float U17;
            public float U18;
        }

        public struct Type0Sub256
        {
            public int U0;
            public float U1;
            public float U2;
            public float U3;
            public int U4;
            public float U5;
            public int U6;
            public int U7;
        }

        public struct Type0Sub257
        {
            public int U0;
            public int U1;
            public float U2;
            public float U3;
            public int U4;
            public float U5;
            public int U6;
            public int U7;
        }

        public struct Type0Sub258
        {
            public int U0;
            public int U1;
            public float U2;
            public float U3;
            public int U4;
            public float U5;
            public int U6;
            public int U7;
            public float U8;
            public float U9;
            public float U10;
            public int U11;
        }
        #endregion

        #region Type2
        public struct Type2
        {
            public int SubType;

            public Type2Sub0? type2Sub0; //Emitter
            public Type2Sub1? type2Sub1; //SplinePath
            public Type2Sub2? type2Sub2; //CollideEmitter
        }

        public struct Type2Sub0
        {
            public int U0;
            public int U1;
            public float U2;
            public float U3;
            public float U4;
            public float U5;
            public float U6;
            public float U7;
            public float U8;
            public int U9;
            public float U10;
            public float U11;
            public float U12;
            public float U13;
            public float U14;
            public float U15;
            public float U16;
            public float U17;
            public float U18;
            public float U19;
            public float U20;
            public float U21;
            public float U22;
            public float U23;
            public float U24;
            public float U25;
            public float U26;
            public float U27;
            public float U28;
            public float U29;
            public float U30;
            public float U31;
            public float U32;
            public float U33;
            public float U34;
            public float U35;
            public float U36;
            public float U37;
            public float U38;
            public float U39;
            public float U40;
            public float U41;
            public float U42;
            public float U43;
            public float U44;
            public float U45;
            public float U46;
            public float U47;
            public float U48;
            public int U49;
            public int U50;
        }

        public struct Type2Sub1
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public float U4;
            public float U5;
            public int U6;
            public int U7;
            public int U8;
            public int U9;
            public int U10;
        }

        public struct Type2Sub2
        {
            public int U0;
            public int U1;
            public int U2;
            public int U3;
            public int U4;
            public int U5;
            public int U6;
            public int U7;
            public int U8;
            public int U9;
            public int U10;
            public int U11;
            public int U12;
            public int U13;
            public int U14;
            public int U15;
            public int U16;
            public int U17;
            public int U18;
            public int U19;
            public int U20;
            public int U21;
            public int U22;
            public int U23;
            public int U24;
            public int U25;
            public int U26;
            public int U27;
            public int U28;
            public int U29;
            public int U30;
            public int U31;
            public int U32;
            public int U33;
            public int U34;
            public int U35;
            public int U36;
            public int U37;
            public int U38;
            public int U39;
            public int U40;
            public int U41;
            public int U42;
            public int U43;
            public int U44;
            public int U45;
            public int U46;
            public int U47;
            public int U48;
            public int U49;
            public int U50;
        }
        #endregion


        public struct Type3
        {
            public int U0;
            public int U1;
        }

        public struct Type5
        {
            public int U0;
            public float U1;
            public int U2;
        }

        public struct InstanceEffect
        {
            public int InstanceIndex;
            public int EffectIndex;
        }

        public struct Type9
        {
            public int U0;
            public float U1;
        }

        public struct SplineEffect
        {
            public int SplineIndex;
            public int Effect;

            //0 - Toggle Off
            //1 - Toggle On
        }
        #endregion
    }
}
