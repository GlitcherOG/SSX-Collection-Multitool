using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.LevelFiles.TrickyPS2
{
    public class MapHandler
    {
        public List<LinkerItem> Models = new List<LinkerItem>();
        public List<LinkerItem> particelModels = new List<LinkerItem>();
        public List<LinkerItem> Patchs = new List<LinkerItem>();
        public List<LinkerItem> InternalInstances = new List<LinkerItem>();
        public List<LinkerItem> PlayerStarts = new List<LinkerItem>();
        public List<LinkerItem> ParticleInstances = new List<LinkerItem>();
        public List<LinkerItem> Splines = new List<LinkerItem>();
        public List<LinkerItem> Lights = new List<LinkerItem>();
        public List<LinkerItem> Materials = new List<LinkerItem>();
        public List<LinkerItem> ContextBlocks = new List<LinkerItem>();
        public List<LinkerItem> Cameras = new List<LinkerItem>();
        public List<LinkerItem> Textures = new List<LinkerItem>();
        public List<LinkerItem> Lightmaps = new List<LinkerItem>();

        int LinePos = 23;

        public void Load(string path)
        {
            string[] Lines = File.ReadAllLines(path);

            LinePos = 23;

            Models = ReadLinkerItems(Lines);

            LinePos += 9;
            particelModels = ReadLinkerItems(Lines);

            LinePos += 9;
            Patchs = ReadLinkerItems(Lines);

            LinePos += 9;
            InternalInstances = ReadLinkerItems(Lines);

            LinePos += 9;
            PlayerStarts = ReadLinkerItems(Lines);

            LinePos += 9;
            ParticleInstances = ReadLinkerItems(Lines);

            LinePos += 9;
            Splines = ReadLinkerItems(Lines);

            LinePos += 9;
            Lights = ReadLinkerItems(Lines);

            LinePos += 9;
            Materials = ReadLinkerItems(Lines);

            LinePos += 9;
            ContextBlocks = ReadLinkerItems(Lines);

            LinePos += 9;
            Cameras = ReadLinkerItems(Lines);

            LinePos += 8;
            Textures = ReadLinkerItems(Lines);

            LinePos += 8;
            Lightmaps = ReadLinkerItems(Lines);
        }

        public void Save(string path)
        {
            //Header
            List<string> FileLines = new List<string>();
            FileLines.Add("");
            FileLines.Add("SSX Level Extractor V" + Program.Version + " By Archy/GlitcherOG");
            FileLines.Add("N/A");
            FileLines.Add("N/A");
            FileLines.Add("N/A");
            FileLines.Add("N/A");
            FileLines.Add("Compiled (" + DateTime.UtcNow.ToLongDateString() + " :: " + DateTime.UtcNow.ToLongTimeString() + ")");
            FileLines.Add("");
            FileLines.Add("##");
            FileLines.Add("");
            FileLines.Add("##"); //Some File Drive Thing
            FileLines.Add("");
            FileLines.Add("##");
            FileLines.Add("");
            FileLines.Add("");

            //MAP FILE START

            FileLines = GenerateBlock(FileLines, "MAP FILE");
            FileLines.Add("");

            //Models
            FileLines = GenerateBlock(FileLines, "MODELS BEGIN", true);
            for (int i = 0; i < Models.Count; i++)
            {
                FileLines.Add(GenerateLinkerItem(Models[i]));
            }
            FileLines.Add("");
            FileLines = GenerateBlock(FileLines, "MODELS END");
            FileLines.Add("");

            //Particle Models
            FileLines = GenerateBlock(FileLines, "PARTICLE MODELS BEGIN", true);
            for (int i = 0; i < particelModels.Count; i++)
            {
                FileLines.Add(GenerateLinkerItem(particelModels[i]));
            }
            FileLines.Add("");
            FileLines = GenerateBlock(FileLines, "PARTICLE MODELS END");
            FileLines.Add("");

            //Patches
            FileLines = GenerateBlock(FileLines, "PATCHES BEGIN", true);
            for (int i = 0; i < Patchs.Count; i++)
            {
                FileLines.Add(GenerateLinkerItem(Patchs[i]));
            }
            FileLines.Add("");
            FileLines = GenerateBlock(FileLines, "PATCHES END");
            FileLines.Add("");

            //Internal Instances
            FileLines = GenerateBlock(FileLines, "INTERNAL INSTANCES BEGIN", true);
            for (int i = 0; i < InternalInstances.Count; i++)
            {
                FileLines.Add(GenerateLinkerItem(InternalInstances[i]));
            }
            FileLines.Add("");
            FileLines = GenerateBlock(FileLines, "INTERNAL INSTANCES END");
            FileLines.Add("");

            //Player Starts
            FileLines = GenerateBlock(FileLines, "PLAYER STARTS BEGIN", true);
            for (int i = 0; i < PlayerStarts.Count; i++)
            {
                FileLines.Add(GenerateLinkerItem(PlayerStarts[i]));
            }
            FileLines.Add("");
            FileLines = GenerateBlock(FileLines, "PLAYER STARTS END");
            FileLines.Add("");

            //Particle Instances
            FileLines = GenerateBlock(FileLines, "PARTICLE INSTANCES BEGIN", true);
            for (int i = 0; i < ParticleInstances.Count; i++)
            {
                FileLines.Add(GenerateLinkerItem(ParticleInstances[i]));
            }
            FileLines.Add("");
            FileLines = GenerateBlock(FileLines, "PARTICLE INSTANCES END");
            FileLines.Add("");

            //Splines
            FileLines = GenerateBlock(FileLines, "SPLINES BEGIN", true);
            for (int i = 0; i < Splines.Count; i++)
            {
                FileLines.Add(GenerateLinkerItem(Splines[i]));
            }
            FileLines.Add("");
            FileLines = GenerateBlock(FileLines, "SPLINES END");
            FileLines.Add("");

            //Lights
            FileLines = GenerateBlock(FileLines, "LIGHTS BEGIN", true);
            for (int i = 0; i < Lights.Count; i++)
            {
                FileLines.Add(GenerateLinkerItem(Lights[i]));
            }
            FileLines.Add("");
            FileLines = GenerateBlock(FileLines, "LIGHTS END");
            FileLines.Add("");

            //Materials
            FileLines = GenerateBlock(FileLines, "MATERIALS BEGIN", true);
            for (int i = 0; i < Materials.Count; i++)
            {
                FileLines.Add(GenerateLinkerItem(Materials[i]));
            }
            FileLines.Add("");
            FileLines = GenerateBlock(FileLines, "MATERIALS END");
            FileLines.Add("");

            //Context Blocks
            FileLines = GenerateBlock(FileLines, "CONTEXT BLOCKS BEGIN", true);
            for (int i = 0; i < ContextBlocks.Count; i++)
            {
                FileLines.Add(GenerateLinkerItem(ContextBlocks[i]));
            }
            FileLines.Add("");
            FileLines = GenerateBlock(FileLines, "CONTEXT BLOCKS END");
            FileLines.Add("");

            //Cameras
            FileLines = GenerateBlock(FileLines, "CAMERAS BEGIN", true);
            for (int i = 0; i < Cameras.Count; i++)
            {
                FileLines.Add(GenerateLinkerItem(Cameras[i]));
            }
            FileLines.Add("");
            FileLines = GenerateBlock(FileLines, "CAMERAS END");
            FileLines.Add("");

            //Textures
            FileLines = GenerateBlock(FileLines, "TEXTURES BEGIN");
            for (int i = 0; i < Textures.Count; i++)
            {
                FileLines.Add(GenerateLinkerItem(Textures[i]));
            }
            FileLines.Add("");
            FileLines = GenerateBlock(FileLines, "TEXTURES END");
            FileLines.Add("");

            //Lightmaps
            FileLines = GenerateBlock(FileLines, "LIGHTMAPS BEGIN");
            for (int i = 0; i < Lightmaps.Count; i++)
            {
                FileLines.Add(GenerateLinkerItem(Lightmaps[i]));
            }
            FileLines.Add("");
            FileLines = GenerateBlock(FileLines, "LIGHTMAPS END");

            string[] lines = FileLines.ToArray();
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllLines(path, lines);
        }

        List<string> GenerateBlock(List<string> List, string BlockText, bool TableAdd = false)
        {
            List.Add("###");
            List.Add("### " + BlockText);
            List.Add("###");
            if (TableAdd)
            {
                List.Add("### Name                                                                          UID       Ref       HashValue ");
            }
            return List;
        }

        string GenerateLinkerItem(LinkerItem item)
        {
            string TempName = GenerateLongString(item.Name, 82);
            string TempUID = GenerateLongString(item.UID.ToString(), 10);
            string TempRef = GenerateLongString(item.Ref.ToString(), 10);
            string TempHashValue = GenerateLongString(item.Hashvalue.ToString(), 10);

            string TempString = TempName + TempUID + TempRef + TempHashValue;
            return TempString;
        }

        string GenerateLongString(string name, int Length)
        {
            char[] TempCharArray = new char[Length];
            for (int i = 0; i < TempCharArray.Length; i++)
            {
                TempCharArray[i] = ' ';
            }

            char[] TempName = name.ToCharArray();

            for (int i = 0; i < TempName.Length; i++)
            {
                TempCharArray[i] = TempName[i];
            }

            return new string(TempCharArray);
        }

        List<LinkerItem> ReadLinkerItems(string[] Lines)
        {
            var TempList = new List<LinkerItem>();
            while (true)
            {
                if (Lines[LinePos] == "")
                {
                    break;
                }
                var LinkerItem = new LinkerItem();
                LinkerItem.Name = Lines[LinePos].Substring(0, 82).TrimEnd(' ');
                LinkerItem.UID = int.Parse(Lines[LinePos].Substring(82, 10).TrimEnd(' '));
                LinkerItem.Ref = int.Parse(Lines[LinePos].Substring(92, 10).TrimEnd(' '));
                LinkerItem.Hashvalue = Lines[LinePos].Substring(102, 10).TrimEnd(' ');
                TempList.Add(LinkerItem);
                LinePos++;
            }
            return TempList;
        }
    }

    public struct LinkerItem
    {
        public string Name;
        public int UID;
        public int Ref;
        public string Hashvalue;
    }
}
