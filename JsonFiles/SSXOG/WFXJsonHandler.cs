//using SSXMultiTool.FileHandlers.LevelFiles.OGPS2;
//using SSXMultiTool.JsonFiles.Tricky;
//using SSXMultiTool.Utilities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SSXMultiTool.JsonFiles.SSXOG
//{
//    public class WFXJsonHandler
//    {
//        public List<EffectSlotJson> EffectSlots = new List<EffectSlotJson>();
//        public List<EffectHeaderStruct> EffectHeaders = new List<EffectHeaderStruct>();

//        public static Effect EffectToJSON(WFXHandler.Effect TempEffect)
//        {
//            var NewEffect = new WFXJsonHandler.Effect();
//            NewEffect.MainType = TempEffect.MainType;

//            if (NewEffect.MainType == 0)
//            {
//                var NewSubType = new Type0();
//                NewSubType.SubType = TempEffect.type0.Value.SubType;
//                if (NewSubType.SubType == 0)
//                {
//                    var NewSubSubType = new Type0Sub0();
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub0.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub0.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub0.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.type0Sub0.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type0.Value.type0Sub0.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type0.Value.type0Sub0.Value.U6;
//                    NewSubType.type0Sub0 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 2)
//                {
//                    NewSubType.type0Sub2 = TempEffect.type0.Value.type0Sub2;
//                }
//                else if (NewSubType.SubType == 5)
//                {
//                    NewSubType.type0Sub5 = TempEffect.type0.Value.type0Sub5;
//                }
//                else if (NewSubType.SubType == 6)
//                {
//                    var NewSubSubType = new Type0Sub6();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub6.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub6.Value.U1;
//                    NewSubType.type0Sub6 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 7)
//                {
//                    var NewSubSubType = new Type0Sub7();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub7.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub7.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub7.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub7.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.type0Sub7.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type0.Value.type0Sub7.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type0.Value.type0Sub7.Value.U6;
//                    NewSubType.type0Sub7 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 10)
//                {
//                    var NewSubSubType = new Type0Sub10();
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub10.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub10.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub10.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.type0Sub10.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type0.Value.type0Sub10.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type0.Value.type0Sub10.Value.U6;
//                    NewSubType.type0Sub10 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 11)
//                {
//                    var NewSubSubType = new Type0Sub11();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub11.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub11.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub11.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub11.Value.U3;
//                    NewSubType.type0Sub11 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 12)
//                {
//                    var NewSubSubType = new FenceFlex();
//                    NewSubSubType.U0 = TempEffect.type0.Value.Fence.Value.U0;
//                    NewSubSubType.FlexAmmount = TempEffect.type0.Value.Fence.Value.FlexAmmount;
//                    NewSubType.Fence = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 13)
//                {
//                    var NewSubSubType = new Type0Sub13();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub13.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub13.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub13.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub13.Value.U3;
//                    NewSubType.type0Sub13 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 14)
//                {
//                    var NewSubSubType = new Type0Sub14();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub14.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub14.Value.U1;
//                    NewSubType.type0Sub14 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 15)
//                {
//                    var NewSubSubType = new Type0Sub15();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub15.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub15.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub15.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub15.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.type0Sub15.Value.U4;
//                    NewSubType.type0Sub15 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 17)
//                {
//                    var NewSubSubType = new CrowdBox();
//                    NewSubSubType.U0 = TempEffect.type0.Value.CrowdEffect.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.CrowdEffect.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.CrowdEffect.Value.U2;
//                    NewSubType.CrowdEffect = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 18)
//                {
//                    var NewSubSubType = new Type0Sub18();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub18.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub18.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub18.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub18.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.type0Sub18.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type0.Value.type0Sub18.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type0.Value.type0Sub18.Value.U6;
//                    NewSubType.type0Sub18 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 20)
//                {
//                    var NewSubSubType = new Type0Sub20();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub20.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub20.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub20.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub20.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.type0Sub20.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type0.Value.type0Sub20.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type0.Value.type0Sub20.Value.U6;
//                    NewSubSubType.U7 = TempEffect.type0.Value.type0Sub20.Value.U7;
//                    NewSubSubType.U8 = TempEffect.type0.Value.type0Sub20.Value.U8;
//                    NewSubSubType.U9 = TempEffect.type0.Value.type0Sub20.Value.U9;
//                    NewSubType.type0Sub20 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 23)
//                {
//                    //DONE
//                }
//                else if (NewSubType.SubType == 24)
//                {
//                    var NewSubSubType = new Type0Sub24();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub24.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub24.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub24.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub24.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.type0Sub24.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type0.Value.type0Sub24.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type0.Value.type0Sub24.Value.U6;
//                    NewSubSubType.U7 = TempEffect.type0.Value.type0Sub24.Value.U7;
//                    NewSubSubType.U8 = TempEffect.type0.Value.type0Sub24.Value.U8;
//                    NewSubSubType.U9 = TempEffect.type0.Value.type0Sub24.Value.U9;
//                    NewSubSubType.U10 = TempEffect.type0.Value.type0Sub24.Value.U10;
//                    NewSubSubType.U11 = TempEffect.type0.Value.type0Sub24.Value.U11;
//                    NewSubSubType.U12 = TempEffect.type0.Value.type0Sub24.Value.U12;
//                    NewSubSubType.U13 = TempEffect.type0.Value.type0Sub24.Value.U13;
//                    NewSubSubType.U14 = TempEffect.type0.Value.type0Sub24.Value.U14;
//                    NewSubSubType.U15 = TempEffect.type0.Value.type0Sub24.Value.U15;
//                    NewSubSubType.U16 = TempEffect.type0.Value.type0Sub24.Value.U16;
//                    NewSubSubType.U17 = TempEffect.type0.Value.type0Sub24.Value.U17;
//                    NewSubSubType.U18 = TempEffect.type0.Value.type0Sub24.Value.U18;
//                    NewSubType.type0Sub24 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 256)
//                {
//                    var NewSubSubType = new Type0Sub256();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub256.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub256.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub256.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub256.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.type0Sub256.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type0.Value.type0Sub256.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type0.Value.type0Sub256.Value.U6;
//                    NewSubSubType.U7 = TempEffect.type0.Value.type0Sub256.Value.U7;
//                    NewSubType.type0Sub256 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 257)
//                {
//                    var NewSubSubType = new Type0Sub257();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub257.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub257.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub257.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub257.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.type0Sub257.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type0.Value.type0Sub257.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type0.Value.type0Sub257.Value.U6;
//                    NewSubSubType.U7 = TempEffect.type0.Value.type0Sub257.Value.U7;
//                    NewSubType.type0Sub257 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 258)
//                {
//                    var NewSubSubType = new Type0Sub258();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub258.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub258.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub258.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub258.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.type0Sub258.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type0.Value.type0Sub258.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type0.Value.type0Sub258.Value.U6;
//                    NewSubSubType.U7 = TempEffect.type0.Value.type0Sub258.Value.U7;
//                    NewSubSubType.U8 = TempEffect.type0.Value.type0Sub258.Value.U8;
//                    NewSubSubType.U9 = TempEffect.type0.Value.type0Sub258.Value.U9;
//                    NewSubSubType.U10 = TempEffect.type0.Value.type0Sub258.Value.U10;
//                    NewSubSubType.U11 = TempEffect.type0.Value.type0Sub258.Value.U11;
//                    NewSubType.type0Sub258 = NewSubSubType;
//                }
//                else
//                {
//                    MessageBox.Show("ERROR Converting to Json Format SSF " + NewEffect.MainType + ", " + NewEffect.type0.Value.SubType);
//                }

//                NewEffect.type0 = NewSubType;
//            }
//            else if (NewEffect.MainType == 2)
//            {
//                var NewSubType = new Type2();
//                NewSubType.SubType = TempEffect.type2.Value.SubType;

//                if (NewSubType.SubType == 0)
//                {
//                    var NewSubSubType = new Type2Sub0();
//                    NewSubSubType.U0 = TempEffect.type2.Value.type2Sub0.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type2.Value.type2Sub0.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type2.Value.type2Sub0.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type2.Value.type2Sub0.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type2.Value.type2Sub0.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type2.Value.type2Sub0.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type2.Value.type2Sub0.Value.U6;
//                    NewSubSubType.U7 = TempEffect.type2.Value.type2Sub0.Value.U7;
//                    NewSubSubType.U8 = TempEffect.type2.Value.type2Sub0.Value.U8;
//                    NewSubSubType.U9 = TempEffect.type2.Value.type2Sub0.Value.U9;
//                    NewSubSubType.U10 = TempEffect.type2.Value.type2Sub0.Value.U10;
//                    NewSubSubType.U11 = TempEffect.type2.Value.type2Sub0.Value.U11;
//                    NewSubSubType.U12 = TempEffect.type2.Value.type2Sub0.Value.U12;
//                    NewSubSubType.U13 = TempEffect.type2.Value.type2Sub0.Value.U13;
//                    NewSubSubType.U14 = TempEffect.type2.Value.type2Sub0.Value.U14;
//                    NewSubSubType.U15 = TempEffect.type2.Value.type2Sub0.Value.U15;
//                    NewSubSubType.U16 = TempEffect.type2.Value.type2Sub0.Value.U16;
//                    NewSubSubType.U17 = TempEffect.type2.Value.type2Sub0.Value.U17;
//                    NewSubSubType.U18 = TempEffect.type2.Value.type2Sub0.Value.U18;
//                    NewSubSubType.U19 = TempEffect.type2.Value.type2Sub0.Value.U19;
//                    NewSubSubType.U20 = TempEffect.type2.Value.type2Sub0.Value.U20;
//                    NewSubSubType.U21 = TempEffect.type2.Value.type2Sub0.Value.U21;
//                    NewSubSubType.U22 = TempEffect.type2.Value.type2Sub0.Value.U22;
//                    NewSubSubType.U23 = TempEffect.type2.Value.type2Sub0.Value.U23;
//                    NewSubSubType.U24 = TempEffect.type2.Value.type2Sub0.Value.U24;
//                    NewSubSubType.U25 = TempEffect.type2.Value.type2Sub0.Value.U25;
//                    NewSubSubType.U26 = TempEffect.type2.Value.type2Sub0.Value.U26;
//                    NewSubSubType.U27 = TempEffect.type2.Value.type2Sub0.Value.U27;
//                    NewSubSubType.U28 = TempEffect.type2.Value.type2Sub0.Value.U28;
//                    NewSubSubType.U29 = TempEffect.type2.Value.type2Sub0.Value.U29;
//                    NewSubSubType.U30 = TempEffect.type2.Value.type2Sub0.Value.U30;
//                    NewSubSubType.U31 = TempEffect.type2.Value.type2Sub0.Value.U31;
//                    NewSubSubType.U32 = TempEffect.type2.Value.type2Sub0.Value.U32;
//                    NewSubSubType.U33 = TempEffect.type2.Value.type2Sub0.Value.U33;
//                    NewSubSubType.U34 = TempEffect.type2.Value.type2Sub0.Value.U34;
//                    NewSubSubType.U35 = TempEffect.type2.Value.type2Sub0.Value.U35;
//                    NewSubSubType.U36 = TempEffect.type2.Value.type2Sub0.Value.U36;
//                    NewSubSubType.U37 = TempEffect.type2.Value.type2Sub0.Value.U37;
//                    NewSubSubType.U38 = TempEffect.type2.Value.type2Sub0.Value.U38;
//                    NewSubSubType.U39 = TempEffect.type2.Value.type2Sub0.Value.U39;
//                    NewSubSubType.U40 = TempEffect.type2.Value.type2Sub0.Value.U40;
//                    NewSubSubType.U41 = TempEffect.type2.Value.type2Sub0.Value.U41;
//                    NewSubSubType.U42 = TempEffect.type2.Value.type2Sub0.Value.U42;
//                    NewSubSubType.U43 = TempEffect.type2.Value.type2Sub0.Value.U43;
//                    NewSubSubType.U44 = TempEffect.type2.Value.type2Sub0.Value.U44;
//                    NewSubSubType.U45 = TempEffect.type2.Value.type2Sub0.Value.U45;
//                    NewSubSubType.U46 = TempEffect.type2.Value.type2Sub0.Value.U46;
//                    NewSubSubType.U47 = TempEffect.type2.Value.type2Sub0.Value.U47;
//                    NewSubSubType.U48 = TempEffect.type2.Value.type2Sub0.Value.U48;
//                    NewSubSubType.U49 = TempEffect.type2.Value.type2Sub0.Value.U49;
//                    NewSubSubType.U50 = TempEffect.type2.Value.type2Sub0.Value.U50;

//                    NewSubType.type2Sub0 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 1)
//                {
//                    var NewSubSubType = new SplinePathAnimation();
//                    NewSubSubType.SplineIndex = TempEffect.type2.Value.SplineAnimation.Value.SplineIndex;
//                    NewSubSubType.U1 = TempEffect.type2.Value.SplineAnimation.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type2.Value.SplineAnimation.Value.U2;
//                    NewSubSubType.InstanceCount = TempEffect.type2.Value.SplineAnimation.Value.InstanceCount;
//                    NewSubSubType.AnimationSpeed = TempEffect.type2.Value.SplineAnimation.Value.AnimationSpeed;
//                    NewSubSubType.U5 = TempEffect.type2.Value.SplineAnimation.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type2.Value.SplineAnimation.Value.U6;
//                    NewSubSubType.U7 = TempEffect.type2.Value.SplineAnimation.Value.U7;
//                    NewSubSubType.R = TempEffect.type2.Value.SplineAnimation.Value.R;
//                    NewSubSubType.G = TempEffect.type2.Value.SplineAnimation.Value.G;
//                    NewSubSubType.B = TempEffect.type2.Value.SplineAnimation.Value.B;

//                    NewSubType.SplineAnimation = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 2)
//                {
//                    var NewSubSubType = new Type2Sub2();
//                    NewSubSubType.U0 = TempEffect.type2.Value.type2Sub2.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type2.Value.type2Sub2.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type2.Value.type2Sub2.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type2.Value.type2Sub2.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type2.Value.type2Sub2.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type2.Value.type2Sub2.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type2.Value.type2Sub2.Value.U6;
//                    NewSubSubType.U7 = TempEffect.type2.Value.type2Sub2.Value.U7;
//                    NewSubSubType.U8 = TempEffect.type2.Value.type2Sub2.Value.U8;
//                    NewSubSubType.U9 = TempEffect.type2.Value.type2Sub2.Value.U9;
//                    NewSubSubType.U10 = TempEffect.type2.Value.type2Sub2.Value.U10;
//                    NewSubSubType.U11 = TempEffect.type2.Value.type2Sub2.Value.U11;
//                    NewSubSubType.U12 = TempEffect.type2.Value.type2Sub2.Value.U12;
//                    NewSubSubType.U13 = TempEffect.type2.Value.type2Sub2.Value.U13;
//                    NewSubSubType.U14 = TempEffect.type2.Value.type2Sub2.Value.U14;
//                    NewSubSubType.U15 = TempEffect.type2.Value.type2Sub2.Value.U15;
//                    NewSubSubType.U16 = TempEffect.type2.Value.type2Sub2.Value.U16;
//                    NewSubSubType.U17 = TempEffect.type2.Value.type2Sub2.Value.U17;
//                    NewSubSubType.U18 = TempEffect.type2.Value.type2Sub2.Value.U18;
//                    NewSubSubType.U19 = TempEffect.type2.Value.type2Sub2.Value.U19;
//                    NewSubSubType.U20 = TempEffect.type2.Value.type2Sub2.Value.U20;
//                    NewSubSubType.U21 = TempEffect.type2.Value.type2Sub2.Value.U21;
//                    NewSubSubType.U22 = TempEffect.type2.Value.type2Sub2.Value.U22;
//                    NewSubSubType.U23 = TempEffect.type2.Value.type2Sub2.Value.U23;
//                    NewSubSubType.U24 = TempEffect.type2.Value.type2Sub2.Value.U24;
//                    NewSubSubType.U25 = TempEffect.type2.Value.type2Sub2.Value.U25;
//                    NewSubSubType.U26 = TempEffect.type2.Value.type2Sub2.Value.U26;
//                    NewSubSubType.U27 = TempEffect.type2.Value.type2Sub2.Value.U27;
//                    NewSubSubType.U28 = TempEffect.type2.Value.type2Sub2.Value.U28;
//                    NewSubSubType.U29 = TempEffect.type2.Value.type2Sub2.Value.U29;
//                    NewSubSubType.U30 = TempEffect.type2.Value.type2Sub2.Value.U30;
//                    NewSubSubType.U31 = TempEffect.type2.Value.type2Sub2.Value.U31;
//                    NewSubSubType.U32 = TempEffect.type2.Value.type2Sub2.Value.U32;
//                    NewSubSubType.U33 = TempEffect.type2.Value.type2Sub2.Value.U33;
//                    NewSubSubType.U34 = TempEffect.type2.Value.type2Sub2.Value.U34;
//                    NewSubSubType.U35 = TempEffect.type2.Value.type2Sub2.Value.U35;
//                    NewSubSubType.U36 = TempEffect.type2.Value.type2Sub2.Value.U36;
//                    NewSubSubType.U37 = TempEffect.type2.Value.type2Sub2.Value.U37;
//                    NewSubSubType.U38 = TempEffect.type2.Value.type2Sub2.Value.U38;
//                    NewSubSubType.U39 = TempEffect.type2.Value.type2Sub2.Value.U39;
//                    NewSubSubType.U40 = TempEffect.type2.Value.type2Sub2.Value.U40;
//                    NewSubSubType.U41 = TempEffect.type2.Value.type2Sub2.Value.U41;
//                    NewSubSubType.U42 = TempEffect.type2.Value.type2Sub2.Value.U42;
//                    NewSubSubType.U43 = TempEffect.type2.Value.type2Sub2.Value.U43;
//                    NewSubSubType.U44 = TempEffect.type2.Value.type2Sub2.Value.U44;
//                    NewSubSubType.U45 = TempEffect.type2.Value.type2Sub2.Value.U45;
//                    NewSubSubType.U46 = TempEffect.type2.Value.type2Sub2.Value.U46;
//                    NewSubSubType.U47 = TempEffect.type2.Value.type2Sub2.Value.U47;
//                    NewSubSubType.U48 = TempEffect.type2.Value.type2Sub2.Value.U48;
//                    NewSubSubType.U49 = TempEffect.type2.Value.type2Sub2.Value.U49;
//                    NewSubSubType.U50 = TempEffect.type2.Value.type2Sub2.Value.U50;

//                    NewSubType.type2Sub2 = NewSubSubType;
//                }
//                else
//                {
//                    MessageBox.Show("ERROR Converting to Json Format SSF " + NewEffect.MainType + ", " + NewEffect.type2.Value.SubType);
//                }

//                NewEffect.type2 = NewSubType;
//            }
//            else if (NewEffect.MainType == 3)
//            {
//                var NewSubType = new Type3();

//                NewSubType.U0 = TempEffect.type3.Value.U0;
//                NewSubType.U1 = TempEffect.type3.Value.U1;

//                NewEffect.type3 = NewSubType;
//            }
//            else if (NewEffect.MainType == 4)
//            {
//                NewEffect.WaitTime = TempEffect.WaitTime;
//            }
//            else if (NewEffect.MainType == 5)
//            {
//                var NewSubType = new Type5();

//                NewSubType.U0 = TempEffect.type5.Value.U0;
//                NewSubType.U1 = TempEffect.type5.Value.U1;
//                NewSubType.U2 = TempEffect.type5.Value.U2;

//                NewEffect.type5 = NewSubType;
//            }
//            else if (NewEffect.MainType == 7)
//            {
//                var NewInstance = new InstanceEffect();

//                NewInstance.InstanceIndex = TempEffect.Instance.Value.InstanceIndex;
//                NewInstance.EffectIndex = TempEffect.Instance.Value.EffectIndex;

//                NewEffect.Instance = NewInstance;
//            }
//            else if (NewEffect.MainType == 8)
//            {
//                NewEffect.SoundPlay = TempEffect.SoundPlay;
//            }
//            else if (NewEffect.MainType == 9)
//            {
//                var NewSubType = new Type9();

//                NewSubType.U0 = TempEffect.type9.Value.U0;
//                NewSubType.U1 = TempEffect.type9.Value.U1;

//                NewEffect.type9 = NewSubType;
//            }
//            else if (NewEffect.MainType == 13)
//            {
//                NewEffect.type13 = TempEffect.type13;
//            }
//            else if (NewEffect.MainType == 14)
//            {
//                NewEffect.MultiplierScore = TempEffect.MultiplierScore;
//            }
//            else if (NewEffect.MainType == 17)
//            {
//                NewEffect.type17 = TempEffect.type17;
//            }
//            else if (NewEffect.MainType == 18)
//            {
//                NewEffect.type18 = TempEffect.type18;
//            }
//            else if (NewEffect.MainType == 21)
//            {
//                NewEffect.FunctionRunIndex = TempEffect.FunctionRunIndex;
//            }
//            else if (NewEffect.MainType == 24)
//            {
//                NewEffect.TeleportInstanceIndex = TempEffect.TeleportInstanceIndex;
//            }
//            else if (NewEffect.MainType == 25)
//            {
//                var NewSpline = new SplineEffect();

//                NewSpline.SplineIndex = TempEffect.Spline.Value.SplineIndex;
//                NewSpline.Effect = TempEffect.Spline.Value.Effect;

//                NewEffect.Spline = NewSpline;
//            }
//            else
//            {
//                MessageBox.Show("ERROR Converting to Json Format SSF " + NewEffect.MainType);
//            }

//            return NewEffect;
//        }

//        public static WFXHandler.Effect JSONToEffect(Effect TempEffect)
//        {
//            var NewEffect = new WFXHandler.Effect();
//            NewEffect.MainType = TempEffect.MainType;

//            if (NewEffect.MainType == 0)
//            {
//                var NewSubType = new WFXHandler.Type0();
//                NewSubType.SubType = TempEffect.type0.Value.SubType;
//                if (NewSubType.SubType == 0)
//                {
//                    var NewSubSubType = new WFXHandler.Type0Sub0();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub0.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub0.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub0.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub0.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.type0Sub0.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type0.Value.type0Sub0.Value.U5;
//                    NewSubType.type0Sub0 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 2)
//                {
//                    NewSubType.Debounce = TempEffect.type0.Value.Debounce.Value;
//                }
//                else if (NewSubType.SubType == 5)
//                {
//                    NewSubType.DeadNodeMode = TempEffect.type0.Value.DeadNodeMode.Value;
//                }
//                else if (NewSubType.SubType == 6)
//                {
//                    var NewSubSubType = new SSFHandler.CounterEffect();
//                    NewSubSubType.Count = TempEffect.type0.Value.Counter.Value.Count;
//                    NewSubSubType.U1 = TempEffect.type0.Value.Counter.Value.U1;
//                    NewSubType.Counter = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 7)
//                {
//                    var NewSubSubType = new SSFHandler.BoostEffect();
//                    NewSubSubType.Mode = TempEffect.type0.Value.Boost.Value.Mode;
//                    NewSubSubType.U1 = TempEffect.type0.Value.Boost.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.Boost.Value.U2;
//                    NewSubSubType.BoostAmount = TempEffect.type0.Value.Boost.Value.BoostAmount;
//                    NewSubSubType.BoostDir = JsonUtil.ArrayToVector3(TempEffect.type0.Value.Boost.Value.BoostDir);
//                    NewSubType.Boost = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 10)
//                {
//                    var NewSubSubType = new SSFHandler.UVScrolling();
//                    NewSubSubType.U0 = TempEffect.type0.Value.UVScroll.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.UVScroll.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.UVScroll.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.UVScroll.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.UVScroll.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type0.Value.UVScroll.Value.U5;
//                    NewSubType.UVScroll = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 11)
//                {
//                    var NewSubSubType = new SSFHandler.TextureFlipEffect();
//                    NewSubSubType.U0 = TempEffect.type0.Value.TextureFlip.Value.U0;
//                    NewSubSubType.Direction = TempEffect.type0.Value.TextureFlip.Value.Direction;
//                    NewSubSubType.Speed = TempEffect.type0.Value.TextureFlip.Value.Speed;
//                    NewSubSubType.Length = TempEffect.type0.Value.TextureFlip.Value.Length;
//                    NewSubSubType.U4 = TempEffect.type0.Value.TextureFlip.Value.U4;
//                    NewSubType.TextureFlip = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 12)
//                {
//                    var NewSubSubType = new SSFHandler.FenceFlex();
//                    NewSubSubType.U0 = TempEffect.type0.Value.Fence.Value.U0;
//                    NewSubSubType.FlexAmmount = TempEffect.type0.Value.Fence.Value.FlexAmmount;
//                    NewSubType.Fence = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 13)
//                {
//                    var NewSubSubType = new SSFHandler.Type0Sub13();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub13.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub13.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub13.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub13.Value.U3;
//                    NewSubType.type0Sub13 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 14)
//                {
//                    var NewSubSubType = new SSFHandler.Type0Sub14();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub14.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub14.Value.U1;
//                    NewSubType.type0Sub14 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 15)
//                {
//                    var NewSubSubType = new SSFHandler.Type0Sub15();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub15.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub15.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub15.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub15.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.type0Sub15.Value.U4;
//                    NewSubType.type0Sub15 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 17)
//                {
//                    var NewSubSubType = new SSFHandler.CrowdBox();
//                    NewSubSubType.U0 = TempEffect.type0.Value.CrowdEffect.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.CrowdEffect.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.CrowdEffect.Value.U2;
//                    NewSubType.CrowdEffect = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 18)
//                {
//                    var NewSubSubType = new SSFHandler.Type0Sub18();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub18.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub18.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub18.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub18.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.type0Sub18.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type0.Value.type0Sub18.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type0.Value.type0Sub18.Value.U6;
//                    NewSubType.type0Sub18 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 20)
//                {
//                    var NewSubSubType = new SSFHandler.Type0Sub20();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub20.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub20.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub20.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub20.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.type0Sub20.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type0.Value.type0Sub20.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type0.Value.type0Sub20.Value.U6;
//                    NewSubSubType.U7 = TempEffect.type0.Value.type0Sub20.Value.U7;
//                    NewSubSubType.U8 = TempEffect.type0.Value.type0Sub20.Value.U8;
//                    NewSubSubType.U9 = TempEffect.type0.Value.type0Sub20.Value.U9;
//                    NewSubType.type0Sub20 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 23)
//                {
//                    //DONE
//                }
//                else if (NewSubType.SubType == 24)
//                {
//                    var NewSubSubType = new SSFHandler.Type0Sub24();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub24.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub24.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub24.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub24.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.type0Sub24.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type0.Value.type0Sub24.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type0.Value.type0Sub24.Value.U6;
//                    NewSubSubType.U7 = TempEffect.type0.Value.type0Sub24.Value.U7;
//                    NewSubSubType.U8 = TempEffect.type0.Value.type0Sub24.Value.U8;
//                    NewSubSubType.U9 = TempEffect.type0.Value.type0Sub24.Value.U9;
//                    NewSubSubType.U10 = TempEffect.type0.Value.type0Sub24.Value.U10;
//                    NewSubSubType.U11 = TempEffect.type0.Value.type0Sub24.Value.U11;
//                    NewSubSubType.U12 = TempEffect.type0.Value.type0Sub24.Value.U12;
//                    NewSubSubType.U13 = TempEffect.type0.Value.type0Sub24.Value.U13;
//                    NewSubSubType.U14 = TempEffect.type0.Value.type0Sub24.Value.U14;
//                    NewSubSubType.U15 = TempEffect.type0.Value.type0Sub24.Value.U15;
//                    NewSubSubType.U16 = TempEffect.type0.Value.type0Sub24.Value.U16;
//                    NewSubSubType.U17 = TempEffect.type0.Value.type0Sub24.Value.U17;
//                    NewSubSubType.U18 = TempEffect.type0.Value.type0Sub24.Value.U18;
//                    NewSubType.type0Sub24 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 256)
//                {
//                    var NewSubSubType = new SSFHandler.Type0Sub256();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub256.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub256.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub256.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub256.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.type0Sub256.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type0.Value.type0Sub256.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type0.Value.type0Sub256.Value.U6;
//                    NewSubSubType.U7 = TempEffect.type0.Value.type0Sub256.Value.U7;
//                    NewSubType.type0Sub256 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 257)
//                {
//                    var NewSubSubType = new SSFHandler.Type0Sub257();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub257.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub257.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub257.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub257.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.type0Sub257.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type0.Value.type0Sub257.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type0.Value.type0Sub257.Value.U6;
//                    NewSubSubType.U7 = TempEffect.type0.Value.type0Sub257.Value.U7;
//                    NewSubType.type0Sub257 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 258)
//                {
//                    var NewSubSubType = new SSFHandler.Type0Sub258();
//                    NewSubSubType.U0 = TempEffect.type0.Value.type0Sub258.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type0.Value.type0Sub258.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type0.Value.type0Sub258.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type0.Value.type0Sub258.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type0.Value.type0Sub258.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type0.Value.type0Sub258.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type0.Value.type0Sub258.Value.U6;
//                    NewSubSubType.U7 = TempEffect.type0.Value.type0Sub258.Value.U7;
//                    NewSubSubType.U8 = TempEffect.type0.Value.type0Sub258.Value.U8;
//                    NewSubSubType.U9 = TempEffect.type0.Value.type0Sub258.Value.U9;
//                    NewSubSubType.U10 = TempEffect.type0.Value.type0Sub258.Value.U10;
//                    NewSubSubType.U11 = TempEffect.type0.Value.type0Sub258.Value.U11;
//                    NewSubType.type0Sub258 = NewSubSubType;
//                }
//                else
//                {
//                    MessageBox.Show("ERROR Converting to Json Format SSF " + NewEffect.MainType + ", " + NewEffect.type0.Value.SubType);
//                }

//                NewEffect.type0 = NewSubType;
//            }
//            else if (NewEffect.MainType == 2)
//            {
//                var NewSubType = new SSFHandler.Type2();
//                NewSubType.SubType = TempEffect.type2.Value.SubType;

//                if (NewSubType.SubType == 0)
//                {
//                    var NewSubSubType = new SSFHandler.Type2Sub0();
//                    NewSubSubType.U0 = TempEffect.type2.Value.type2Sub0.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type2.Value.type2Sub0.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type2.Value.type2Sub0.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type2.Value.type2Sub0.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type2.Value.type2Sub0.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type2.Value.type2Sub0.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type2.Value.type2Sub0.Value.U6;
//                    NewSubSubType.U7 = TempEffect.type2.Value.type2Sub0.Value.U7;
//                    NewSubSubType.U8 = TempEffect.type2.Value.type2Sub0.Value.U8;
//                    NewSubSubType.U9 = TempEffect.type2.Value.type2Sub0.Value.U9;
//                    NewSubSubType.U10 = TempEffect.type2.Value.type2Sub0.Value.U10;
//                    NewSubSubType.U11 = TempEffect.type2.Value.type2Sub0.Value.U11;
//                    NewSubSubType.U12 = TempEffect.type2.Value.type2Sub0.Value.U12;
//                    NewSubSubType.U13 = TempEffect.type2.Value.type2Sub0.Value.U13;
//                    NewSubSubType.U14 = TempEffect.type2.Value.type2Sub0.Value.U14;
//                    NewSubSubType.U15 = TempEffect.type2.Value.type2Sub0.Value.U15;
//                    NewSubSubType.U16 = TempEffect.type2.Value.type2Sub0.Value.U16;
//                    NewSubSubType.U17 = TempEffect.type2.Value.type2Sub0.Value.U17;
//                    NewSubSubType.U18 = TempEffect.type2.Value.type2Sub0.Value.U18;
//                    NewSubSubType.U19 = TempEffect.type2.Value.type2Sub0.Value.U19;
//                    NewSubSubType.U20 = TempEffect.type2.Value.type2Sub0.Value.U20;
//                    NewSubSubType.U21 = TempEffect.type2.Value.type2Sub0.Value.U21;
//                    NewSubSubType.U22 = TempEffect.type2.Value.type2Sub0.Value.U22;
//                    NewSubSubType.U23 = TempEffect.type2.Value.type2Sub0.Value.U23;
//                    NewSubSubType.U24 = TempEffect.type2.Value.type2Sub0.Value.U24;
//                    NewSubSubType.U25 = TempEffect.type2.Value.type2Sub0.Value.U25;
//                    NewSubSubType.U26 = TempEffect.type2.Value.type2Sub0.Value.U26;
//                    NewSubSubType.U27 = TempEffect.type2.Value.type2Sub0.Value.U27;
//                    NewSubSubType.U28 = TempEffect.type2.Value.type2Sub0.Value.U28;
//                    NewSubSubType.U29 = TempEffect.type2.Value.type2Sub0.Value.U29;
//                    NewSubSubType.U30 = TempEffect.type2.Value.type2Sub0.Value.U30;
//                    NewSubSubType.U31 = TempEffect.type2.Value.type2Sub0.Value.U31;
//                    NewSubSubType.U32 = TempEffect.type2.Value.type2Sub0.Value.U32;
//                    NewSubSubType.U33 = TempEffect.type2.Value.type2Sub0.Value.U33;
//                    NewSubSubType.U34 = TempEffect.type2.Value.type2Sub0.Value.U34;
//                    NewSubSubType.U35 = TempEffect.type2.Value.type2Sub0.Value.U35;
//                    NewSubSubType.U36 = TempEffect.type2.Value.type2Sub0.Value.U36;
//                    NewSubSubType.U37 = TempEffect.type2.Value.type2Sub0.Value.U37;
//                    NewSubSubType.U38 = TempEffect.type2.Value.type2Sub0.Value.U38;
//                    NewSubSubType.U39 = TempEffect.type2.Value.type2Sub0.Value.U39;
//                    NewSubSubType.U40 = TempEffect.type2.Value.type2Sub0.Value.U40;
//                    NewSubSubType.U41 = TempEffect.type2.Value.type2Sub0.Value.U41;
//                    NewSubSubType.U42 = TempEffect.type2.Value.type2Sub0.Value.U42;
//                    NewSubSubType.U43 = TempEffect.type2.Value.type2Sub0.Value.U43;
//                    NewSubSubType.U44 = TempEffect.type2.Value.type2Sub0.Value.U44;
//                    NewSubSubType.U45 = TempEffect.type2.Value.type2Sub0.Value.U45;
//                    NewSubSubType.U46 = TempEffect.type2.Value.type2Sub0.Value.U46;
//                    NewSubSubType.U47 = TempEffect.type2.Value.type2Sub0.Value.U47;
//                    NewSubSubType.U48 = TempEffect.type2.Value.type2Sub0.Value.U48;
//                    NewSubSubType.U49 = TempEffect.type2.Value.type2Sub0.Value.U49;
//                    NewSubSubType.U50 = TempEffect.type2.Value.type2Sub0.Value.U50;

//                    NewSubType.type2Sub0 = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 1)
//                {
//                    var NewSubSubType = new SSFHandler.SplinePathAnimation();
//                    NewSubSubType.SplineIndex = TempEffect.type2.Value.SplineAnimation.Value.SplineIndex;
//                    NewSubSubType.U1 = TempEffect.type2.Value.SplineAnimation.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type2.Value.SplineAnimation.Value.U2;
//                    NewSubSubType.InstanceCount = TempEffect.type2.Value.SplineAnimation.Value.InstanceCount;
//                    NewSubSubType.AnimationSpeed = TempEffect.type2.Value.SplineAnimation.Value.AnimationSpeed;
//                    NewSubSubType.U5 = TempEffect.type2.Value.SplineAnimation.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type2.Value.SplineAnimation.Value.U6;
//                    NewSubSubType.U7 = TempEffect.type2.Value.SplineAnimation.Value.U7;
//                    NewSubSubType.R = TempEffect.type2.Value.SplineAnimation.Value.R;
//                    NewSubSubType.G = TempEffect.type2.Value.SplineAnimation.Value.G;
//                    NewSubSubType.B = TempEffect.type2.Value.SplineAnimation.Value.B;

//                    NewSubType.SplineAnimation = NewSubSubType;
//                }
//                else if (NewSubType.SubType == 2)
//                {
//                    var NewSubSubType = new SSFHandler.Type2Sub2();
//                    NewSubSubType.U0 = TempEffect.type2.Value.type2Sub2.Value.U0;
//                    NewSubSubType.U1 = TempEffect.type2.Value.type2Sub2.Value.U1;
//                    NewSubSubType.U2 = TempEffect.type2.Value.type2Sub2.Value.U2;
//                    NewSubSubType.U3 = TempEffect.type2.Value.type2Sub2.Value.U3;
//                    NewSubSubType.U4 = TempEffect.type2.Value.type2Sub2.Value.U4;
//                    NewSubSubType.U5 = TempEffect.type2.Value.type2Sub2.Value.U5;
//                    NewSubSubType.U6 = TempEffect.type2.Value.type2Sub2.Value.U6;
//                    NewSubSubType.U7 = TempEffect.type2.Value.type2Sub2.Value.U7;
//                    NewSubSubType.U8 = TempEffect.type2.Value.type2Sub2.Value.U8;
//                    NewSubSubType.U9 = TempEffect.type2.Value.type2Sub2.Value.U9;
//                    NewSubSubType.U10 = TempEffect.type2.Value.type2Sub2.Value.U10;
//                    NewSubSubType.U11 = TempEffect.type2.Value.type2Sub2.Value.U11;
//                    NewSubSubType.U12 = TempEffect.type2.Value.type2Sub2.Value.U12;
//                    NewSubSubType.U13 = TempEffect.type2.Value.type2Sub2.Value.U13;
//                    NewSubSubType.U14 = TempEffect.type2.Value.type2Sub2.Value.U14;
//                    NewSubSubType.U15 = TempEffect.type2.Value.type2Sub2.Value.U15;
//                    NewSubSubType.U16 = TempEffect.type2.Value.type2Sub2.Value.U16;
//                    NewSubSubType.U17 = TempEffect.type2.Value.type2Sub2.Value.U17;
//                    NewSubSubType.U18 = TempEffect.type2.Value.type2Sub2.Value.U18;
//                    NewSubSubType.U19 = TempEffect.type2.Value.type2Sub2.Value.U19;
//                    NewSubSubType.U20 = TempEffect.type2.Value.type2Sub2.Value.U20;
//                    NewSubSubType.U21 = TempEffect.type2.Value.type2Sub2.Value.U21;
//                    NewSubSubType.U22 = TempEffect.type2.Value.type2Sub2.Value.U22;
//                    NewSubSubType.U23 = TempEffect.type2.Value.type2Sub2.Value.U23;
//                    NewSubSubType.U24 = TempEffect.type2.Value.type2Sub2.Value.U24;
//                    NewSubSubType.U25 = TempEffect.type2.Value.type2Sub2.Value.U25;
//                    NewSubSubType.U26 = TempEffect.type2.Value.type2Sub2.Value.U26;
//                    NewSubSubType.U27 = TempEffect.type2.Value.type2Sub2.Value.U27;
//                    NewSubSubType.U28 = TempEffect.type2.Value.type2Sub2.Value.U28;
//                    NewSubSubType.U29 = TempEffect.type2.Value.type2Sub2.Value.U29;
//                    NewSubSubType.U30 = TempEffect.type2.Value.type2Sub2.Value.U30;
//                    NewSubSubType.U31 = TempEffect.type2.Value.type2Sub2.Value.U31;
//                    NewSubSubType.U32 = TempEffect.type2.Value.type2Sub2.Value.U32;
//                    NewSubSubType.U33 = TempEffect.type2.Value.type2Sub2.Value.U33;
//                    NewSubSubType.U34 = TempEffect.type2.Value.type2Sub2.Value.U34;
//                    NewSubSubType.U35 = TempEffect.type2.Value.type2Sub2.Value.U35;
//                    NewSubSubType.U36 = TempEffect.type2.Value.type2Sub2.Value.U36;
//                    NewSubSubType.U37 = TempEffect.type2.Value.type2Sub2.Value.U37;
//                    NewSubSubType.U38 = TempEffect.type2.Value.type2Sub2.Value.U38;
//                    NewSubSubType.U39 = TempEffect.type2.Value.type2Sub2.Value.U39;
//                    NewSubSubType.U40 = TempEffect.type2.Value.type2Sub2.Value.U40;
//                    NewSubSubType.U41 = TempEffect.type2.Value.type2Sub2.Value.U41;
//                    NewSubSubType.U42 = TempEffect.type2.Value.type2Sub2.Value.U42;
//                    NewSubSubType.U43 = TempEffect.type2.Value.type2Sub2.Value.U43;
//                    NewSubSubType.U44 = TempEffect.type2.Value.type2Sub2.Value.U44;
//                    NewSubSubType.U45 = TempEffect.type2.Value.type2Sub2.Value.U45;
//                    NewSubSubType.U46 = TempEffect.type2.Value.type2Sub2.Value.U46;
//                    NewSubSubType.U47 = TempEffect.type2.Value.type2Sub2.Value.U47;
//                    NewSubSubType.U48 = TempEffect.type2.Value.type2Sub2.Value.U48;
//                    NewSubSubType.U49 = TempEffect.type2.Value.type2Sub2.Value.U49;
//                    NewSubSubType.U50 = TempEffect.type2.Value.type2Sub2.Value.U50;

//                    NewSubType.type2Sub2 = NewSubSubType;
//                }
//                else
//                {
//                    MessageBox.Show("ERROR Converting to Json Format SSF " + NewEffect.MainType + ", " + NewEffect.type2.Value.SubType);
//                }

//                NewEffect.type2 = NewSubType;
//            }
//            else if (NewEffect.MainType == 3)
//            {
//                var NewSubType = new SSFHandler.Type3();

//                NewSubType.U0 = TempEffect.type3.Value.U0;
//                NewSubType.U1 = TempEffect.type3.Value.U1;

//                NewEffect.type3 = NewSubType;
//            }
//            else if (NewEffect.MainType == 4)
//            {
//                NewEffect.WaitTime = TempEffect.WaitTime.Value;
//            }
//            else if (NewEffect.MainType == 5)
//            {
//                var NewSubType = new SSFHandler.Type5();

//                NewSubType.U0 = TempEffect.type5.Value.U0;
//                NewSubType.U1 = TempEffect.type5.Value.U1;
//                NewSubType.U2 = TempEffect.type5.Value.U2;

//                NewEffect.type5 = NewSubType;
//            }
//            else if (NewEffect.MainType == 7)
//            {
//                var NewInstance = new SSFHandler.InstanceEffect();

//                NewInstance.InstanceIndex = TempEffect.Instance.Value.InstanceIndex;
//                NewInstance.EffectIndex = TempEffect.Instance.Value.EffectIndex;

//                NewEffect.Instance = NewInstance;
//            }
//            else if (NewEffect.MainType == 8)
//            {
//                NewEffect.SoundPlay = TempEffect.SoundPlay.Value;
//            }
//            else if (NewEffect.MainType == 9)
//            {
//                var NewSubType = new SSFHandler.Type9();

//                NewSubType.U0 = TempEffect.type9.Value.U0;
//                NewSubType.U1 = TempEffect.type9.Value.U1;

//                NewEffect.type9 = NewSubType;
//            }
//            else if (NewEffect.MainType == 13)
//            {
//                NewEffect.type13 = TempEffect.type13.Value;
//            }
//            else if (NewEffect.MainType == 14)
//            {
//                NewEffect.MultiplierScore = TempEffect.MultiplierScore.Value;
//            }
//            else if (NewEffect.MainType == 17)
//            {
//                NewEffect.type17 = TempEffect.type17.Value;
//            }
//            else if (NewEffect.MainType == 18)
//            {
//                NewEffect.type18 = TempEffect.type18.Value;
//            }
//            else if (NewEffect.MainType == 21)
//            {
//                NewEffect.FunctionRunIndex = TempEffect.FunctionRunIndex.Value;
//            }
//            else if (NewEffect.MainType == 24)
//            {
//                NewEffect.TeleportInstanceIndex = TempEffect.TeleportInstanceIndex.Value;
//            }
//            else if (NewEffect.MainType == 25)
//            {
//                var NewSpline = new SSFHandler.SplineEffect();

//                NewSpline.SplineIndex = TempEffect.Spline.Value.SplineIndex;
//                NewSpline.Effect = TempEffect.Spline.Value.Effect;

//                NewEffect.Spline = NewSpline;
//            }
//            else
//            {
//                MessageBox.Show("ERROR Converting to Json Format SSF " + NewEffect.MainType);
//            }

//            return NewEffect;
//        }

//        public struct EffectSlotJson
//        {
//            public string EffectSlotName;

//            public int PersistantEffectSlot;
//            public int CollisionEffectSlot;
//            public int Slot3;
//            public int Slot4;
//            public int EffectTriggerSlot;
//            public int Slot6;
//            public int Slot7;
//        }

//        public struct PhysicsHeader
//        {
//            public string PhysicsName;
//            public List<PhysicsData> PhysicsDatas;
//        }

//        public struct PhysicsData
//        {
//            public int U2;

//            public float UFloat0;
//            public float UFloat1;
//            public float UFloat2;
//            public float UFloat3;
//            public float UFloat4;
//            public float UFloat5;
//            public float UFloat6;
//            public float UFloat7;
//            public float UFloat8;
//            public float UFloat9;
//            public float UFloat10;
//            public float UFloat11;
//            public float UFloat12;
//            public float UFloat13;
//            public float UFloat14;
//            public float UFloat15;
//            public float UFloat16;
//            public float UFloat17;
//            public float UFloat18;
//            public float UFloat19;
//            public float UFloat20;
//            public float UFloat21;
//            public float UFloat22;
//            public float UFloat23;

//            public List<UPhysicsStruct> uPhysicsStruct0;

//            public byte[] UByteData;
//        }

//        public struct UPhysicsStruct
//        {
//            public float U0;
//            public float U1;
//            public int U2;
//        }

//        public struct EffectHeaderStruct
//        {
//            public string EffectName;
//            public List<Effect> Effects;
//        }

//        public struct Effect
//        {
//            //16
//            public int MainType;
//            public int Size;

//            public Type0? type0;
//            public Type1? type1;
//            public Type2? type2;
//            public float type3;
//            public Type4? type4;
//            public Type6? type6;
//            public int type7;
//            public Type8? type8;
//            public int type9;
//            public int type12;
//            public float type13;
//            public float type16;
//            public float type17;
//        }

//        public struct Type0
//        {
//            public int SubType;

//            public Type0Sub0? type0Sub0;
//            public float type0Sub2;
//            public int type0Sub5;
//            public Type0Sub6? type0Sub6;
//            public Type0Sub7? type0Sub7;
//            public Type0Sub10? type0Sub10;
//            public Type0Sub11? type0Sub11;
//            public Type0Sub12? type0Sub12;
//            public Type0Sub13? type0Sub13;
//            public Type0Sub14? type0Sub14;
//            public Type0Sub15? type0Sub15;
//            public Type0Sub16? type0Sub16;
//            public float type0Sub17;
//            public Type0Sub18? type0Sub18;
//            public Type0Sub19? type0Sub19;
//            public Type0Sub20? type0Sub20;
//            public Type0Sub21? type0Sub21;
//            public Type0Sub22? type0Sub22;
//            public Type0Sub256? type0Sub256;
//            public Type0Sub257? type0Sub257;
//            public Type0Sub258? type0Sub258;
//            public Type0Sub259? type0Sub259;
//        }

//        public struct Type0Sub0
//        {
//            public float U1;
//            public float U2;
//            public float U3;
//            public float U4;
//            public int U5;
//            public int U6;
//        }

//        public struct Type0Sub6
//        {
//            public int U0;
//            public float U1;

//        }

//        public struct Type0Sub7
//        {
//            public int U0;
//            public int U1;
//            public int U2;
//            public int U3;
//            public int U4;
//            public int U5;
//            public int U6;
//        }

//        public struct Type0Sub10
//        {
//            public float U1;
//            public float U2;
//            public float U3;
//            public float U4;
//            public int U5;
//            public int U6;
//        }

//        public struct Type0Sub11
//        {
//            public int U0;
//            public int U1;
//            public int U2;
//            public int U3;
//        }

//        public struct Type0Sub12
//        {
//            public int U0;
//            public float U1;
//        }

//        public struct Type0Sub13
//        {
//            public int U0;
//            public int U1;
//            public int U2;
//            public int U3;
//        }

//        public struct Type0Sub14
//        {
//            public int U0;
//            public float U1;
//        }

//        public struct Type0Sub15
//        {
//            public float U0;
//            public float U1;
//            public float U2;
//            public float U3;
//            public float U4;
//        }

//        public struct Type0Sub16
//        {
//            public int U0;
//            public int U1;
//            public int U2;
//            public int U3;
//            public int U4;
//            public int U5;
//            public int U6;
//            public int U7;
//            public int U8;
//            public int U9;
//        }

//        public struct Type0Sub18
//        {
//            public int U0;
//            public int U1;
//            public int U2;
//            public int U3;
//            public int U4;
//            public int U5;
//        }

//        public struct Type0Sub19
//        {
//            public int U0;
//            public int U1;
//            public int U2;
//            public int U3;
//            public int U4;
//            public int U5;
//            public int U6;
//            public int U7;
//        }

//        public struct Type0Sub20
//        {
//            public int U0;
//            public int U1;
//            public int U2;
//            public int U3;
//            public int U4;
//            public int U5;
//            public int U6;
//            public int U7;
//            public int U8;
//        }

//        public struct Type0Sub21
//        {
//            public int U0;
//            public int U1;
//            public float U2;
//        }

//        public struct Type0Sub22
//        {
//            public int U1;
//            public int U2;
//            public int U3;
//            public int U4;
//            public int U5;
//            public int U6;
//            public int U7;
//            public int U8;
//            public int U9;
//            public int U10;
//            public int U11;
//            public int U12;
//            public int U13;
//            public int U14;
//            public int U15;
//            public int U16;
//            public int U17;
//            public int U18;
//            public int U19;
//            public int U20;
//            public int U21;
//            public int U22;
//            public int U23;
//            public int U24;
//            public int U25;
//            public int U26;
//            public int U27;
//            public int U28;
//            public int U29;
//            public int U30;
//            public int U31;
//            public int U32;
//            public int U33;
//            public int U34;
//            public int U35;
//            public int U36;
//            public int U37;
//            public int U38;
//            public int U39;
//            public int U40;
//            public int U41;
//            public int U42;
//            public int U43;
//            public int U44;
//            public int U45;
//            public int U46;
//            public int U47;
//            public int U48;
//            public int U49;
//            public int U50;
//        }

//        public struct Type0Sub256
//        {
//            public int U0;
//            public int U1;
//            public int U2;
//            public int U3;
//            public int U4;
//            public int U5;
//            public int U6;
//        }

//        public struct Type0Sub257
//        {
//            public int U0;
//            public int U1;
//            public int U2;
//            public int U3;
//            public int U4;
//            public int U5;
//            public int U6;
//        }

//        public struct Type0Sub258
//        {
//            public int U0;
//            public int U1;
//            public int U2;
//            public int U3;
//            public int U4;
//            public int U5;
//            public int U6;
//            public int U7;
//            public int U8;
//            public int U9;
//            public int U10;
//        }

//        public struct Type0Sub259
//        {
//            public int U0;
//            public int U1;
//            public int U2;
//            public int U3;
//            public int U4;
//            public int U5;
//            public int U6;
//            public int U7;
//            public int U8;
//        }

//        public struct Type1
//        {
//            public int SubType;
//            public Type1Sub0? type1Sub0;
//            public Type1Sub1? type1Sub1;
//            public Type1Sub2? type1Sub2;
//        }

//        public struct Type1Sub0
//        {
//            public int U1;
//            public int U2;
//            public int U3;
//            public int U4;
//            public int U5;
//            public int U6;
//            public int U7;
//            public int U8;
//            public int U9;
//            public int U10;
//            public int U11;
//            public int U12;
//            public int U13;
//            public int U14;
//            public int U15;
//            public int U16;
//            public int U17;
//            public int U18;
//            public int U19;
//            public int U20;
//            public int U21;
//            public int U22;
//            public int U23;
//            public int U24;
//            public int U25;
//            public int U26;
//            public int U27;
//            public int U28;
//            public int U29;
//            public int U30;
//            public int U31;
//            public int U32;
//            public int U33;
//            public int U34;
//            public int U35;
//            public int U36;
//            public int U37;
//            public int U38;
//            public int U39;
//            public int U40;
//            public int U41;
//            public int U42;
//            public int U43;
//            public int U44;
//            public int U45;
//            public int U46;
//            public int U47;
//            public int U48;
//            public int U49;
//            public int U50;
//            public int U51;
//        }

//        public struct Type1Sub1
//        {
//            public int U0;
//            public int U1;
//            public int U2;
//            public int U3;
//            public int U4;
//            public int U5;
//            public int U6;
//            public int U7;
//            public int U8;
//            public int U9;
//            public int U10;
//        }

//        public struct Type1Sub2
//        {
//            public int U0;
//            public int U1;
//            public int U2;
//            public int U3;
//            public int U4;
//            public int U5;
//            public int U6;
//            public int U7;
//            public int U8;
//            public int U9;
//            public int U10;
//            public int U11;
//        }

//        public struct Type2
//        {
//            public int U0;
//            public float U1;
//        }

//        public struct Type4
//        {
//            public int U0;
//            public int U1;
//            public int U2;
//        }

//        public struct Type6
//        {
//            public int U0;
//            public int U1;
//            public int U2;
//        }

//        public struct Type8
//        {
//            public int U0;
//            public float U1;
//        }

//#endregion
//    }
//}
