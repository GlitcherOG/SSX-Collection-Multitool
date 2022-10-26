using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSX_Modder.FileHandlers
{
    class MusicINFHandler
    {
        public string musPath;
        public string musFileHeader = "";
        public List<MusFileSong> musFileSongs = new List<MusFileSong>();

        public void LoadMusFile(string path)
        {
            musFileSongs.Clear();
            string wholeFile = File.ReadAllText(path);
            string[] splitPoint = new string[1] { "#### " };
            string[] LineSplit = wholeFile.Split(splitPoint, StringSplitOptions.None);
            musFileHeader = LineSplit[0];
            for (int i = 1; i < LineSplit.Length; i++)
            {
                MusFileSong fileSong = new MusFileSong
                {
                    Comment = "",
                    ID = "",
                    AADTOFE = 0,
                    Title = "",
                    Artist = "",
                    Album = "",
                    PathData = "",
                    MusicData = "",
                    loopData = "",
                    BPM = -1,
                    BeatsPerMeasure = -1,
                    MeasuresPerBar = -1,
                    PhrasesPerBank = -1,
                    BeatsPerPhrase = -1,
                    PhraseAligin = -1,
                    DelayCount = -1,
                    DelayTime = -1,
                    DelayFeedback = -1,
                    DelayLevel = -1,
                    PathLevel = -1,
                    AsyncLevel = -1,
                    Category0 = false,
                    Category1 = false,
                    Category2 = false,
                    Category3 = false,
                    Category4 = false,
                    DuckToLoops = -1,
                    SEDValue = -1,
                    Lowpass = -1,
                    Preview = -1,
                    SongBig = 1,
                };
                splitPoint = new string[1] { "\r\n" };
                string[] lines = LineSplit[i].Split(splitPoint, StringSplitOptions.RemoveEmptyEntries);
                fileSong.Comment = lines[0];
                for (int a = 0; a < lines.Length; a++)
                {
                    if (lines[a].Contains("[") && fileSong.ID == "")
                    {
                        fileSong.ID = lines[a];
                    }
                    if (lines[a].Contains("="))
                    {
                        string[] var = GetVar(lines[a]);
                        if (var[0] == "ADDTOFE")
                        {
                            fileSong.AADTOFE = Int32.Parse(var[1]);
                        }
                        if (var[0] == "TITLE")
                        {
                            fileSong.Title = var[1].Trim('"');
                        }
                        if (var[0] == "ARTIST")
                        {
                            fileSong.Artist = var[1].Trim('"');
                        }
                        if (var[0] == "ALBUM")
                        {
                            fileSong.Album = var[1].Trim('"');
                        }
                        if (var[0] == "PATHDATA")
                        {
                            fileSong.PathData = var[1].Trim('"');
                        }
                        if (var[0] == "MUSDATA")
                        {
                            fileSong.MusicData = var[1].Trim('"');
                        }
                        if (var[0] == "LOOPDATA")
                        {
                            fileSong.loopData = var[1].Trim('"');
                        }
                        if (var[0] == "BPM")
                        {
                            fileSong.BPM = float.Parse(var[1]);
                        }
                        if (var[0] == "BeatsPerMeasure")
                        {
                            fileSong.BeatsPerMeasure = Int32.Parse(var[1]);
                        }
                        if (var[0] == "MeasuresPerBar")
                        {
                            fileSong.MeasuresPerBar = Int32.Parse(var[1]);
                        }
                        if (var[0] == "PhrasesPerBank")
                        {
                            fileSong.PhrasesPerBank = Int32.Parse(var[1]);
                        }
                        if (var[0] == "BeatsPerPhrase")
                        {
                            fileSong.BeatsPerPhrase = Int32.Parse(var[1]);
                        }
                        if (var[0] == "PhraseAlign")
                        {
                            fileSong.PhraseAligin = Int32.Parse(var[1]);
                        }
                        if (var[0] == "DelayCount")
                        {
                            fileSong.DelayCount = Int32.Parse(var[1]);
                        }
                        if (var[0] == "DelayTime")
                        {
                            fileSong.DelayTime = Int32.Parse(var[1]);
                        }
                        if (var[0] == "DelayFeedback")
                        {
                            fileSong.DelayFeedback = Int32.Parse(var[1]);
                        }
                        if (var[0] == "DelayLevel")
                        {
                            fileSong.DelayLevel = Int32.Parse(var[1]);
                        }
                        if (var[0].ToLower() == "asynclevel")
                        {
                            fileSong.AsyncLevel = Int32.Parse(var[1]);
                        }
                        if (var[0].ToLower() == "pathlevel")
                        {
                            fileSong.PathLevel = Int32.Parse(var[1]);
                        }
                        if (var[0] == "DUCKTOLOOPS")
                        {
                            fileSong.DuckToLoops = Int32.Parse(var[1]);
                        }
                        if (var[0] == "SEDVALUE")
                        {
                            fileSong.SEDValue = Int32.Parse(var[1]);
                        }
                        if (var[0] == "LOWPASS")
                        {
                            fileSong.Lowpass = Int32.Parse(var[1]);
                        }
                        if (var[0] == "PREVIEW")
                        {
                            fileSong.Preview = Int32.Parse(var[1]);
                        }
                        if (var[0] == "SONGBIG")
                        {
                            fileSong.SongBig = Int32.Parse(var[1]);
                        }
                        if (var[0] == "CATEGORY")
                        {
                            if (var[1] == "0")
                            {
                                fileSong.Category0 = true;
                            }
                            if (var[1] == "1")
                            {
                                fileSong.Category1 = true;
                            }
                            if (var[1] == "2")
                            {
                                fileSong.Category2 = true;
                            }
                            if (var[1] == "3")
                            {
                                fileSong.Category3 = true;
                            }
                            if (var[1] == "4")
                            {
                                fileSong.Category4 = true;
                            }
                        }
                    }
                }
                musFileSongs.Add(fileSong);
            }
        }

        public void SaveMusFile(string path = null)
        {
            string newFile = musFileHeader;
            for (int i = 0; i < musFileSongs.Count; i++)
            {
                newFile += "#### " + musFileSongs[i].Comment + Environment.NewLine;
                newFile += musFileSongs[i].ID + Environment.NewLine;
                newFile += "    ADDTOFE = " + musFileSongs[i].AADTOFE.ToString() + Environment.NewLine;
                newFile += "    TITLE = \"" + musFileSongs[i].Title + "\"" + Environment.NewLine;
                newFile += "    ARTIST = \"" + musFileSongs[i].Artist + "\"" + Environment.NewLine;
                newFile += "    ALBUM = \"" + musFileSongs[i].Album + "\"" + Environment.NewLine;
                newFile += "    PATHDATA = \"" + musFileSongs[i].PathData + "\"" + Environment.NewLine;
                newFile += "    MUSDATA = \"" + musFileSongs[i].MusicData + "\"" + Environment.NewLine;
                if (musFileSongs[i].loopData != "")
                {
                    newFile += "    LOOPDATA = \"" + musFileSongs[i].loopData + "\"" + Environment.NewLine;
                }
                if (musFileSongs[i].BeatsPerMeasure != -1)
                {
                    newFile += "    BeatsPerMeasure = " + musFileSongs[i].BeatsPerMeasure + Environment.NewLine;
                }
                if (musFileSongs[i].MeasuresPerBar != -1)
                {
                    newFile += "    MeasuresPerBar = " + musFileSongs[i].MeasuresPerBar + Environment.NewLine;
                }
                if (musFileSongs[i].PhrasesPerBank != -1)
                {
                    newFile += "    PhrasesPerBank = " + musFileSongs[i].PhrasesPerBank + Environment.NewLine;
                }
                if (musFileSongs[i].BeatsPerPhrase != -1)
                {
                    newFile += "    BeatsPerPhrase = " + musFileSongs[i].BeatsPerPhrase + Environment.NewLine;
                }
                if (musFileSongs[i].PhraseAligin != -1)
                {
                    newFile += "    PhraseAlign = " + musFileSongs[i].PhraseAligin + Environment.NewLine;
                }
                if (musFileSongs[i].DelayCount != -1)
                {
                    newFile += "    DelayCount = " + musFileSongs[i].DelayCount + Environment.NewLine;
                }
                if (musFileSongs[i].DelayTime != -1)
                {
                    newFile += "    DelayTime = " + musFileSongs[i].DelayTime + Environment.NewLine;
                }
                if (musFileSongs[i].DelayFeedback != -1)
                {
                    newFile += "    DelayFeedback = " + musFileSongs[i].DelayFeedback + Environment.NewLine;
                }
                if (musFileSongs[i].DelayLevel != -1)
                {
                    newFile += "    DelayLevel = " + musFileSongs[i].DelayLevel + Environment.NewLine;
                }
                if (musFileSongs[i].PathLevel != -1)
                {
                    newFile += "    Pathlevel = " + musFileSongs[i].PathLevel + Environment.NewLine;
                }
                if (musFileSongs[i].AsyncLevel != -1)
                {
                    newFile += "    Asynclevel = " + musFileSongs[i].AsyncLevel + Environment.NewLine;
                }
                if (musFileSongs[i].BPM != -1)
                {
                    newFile += "    BPM = " + musFileSongs[i].BPM + Environment.NewLine;
                }
                if (musFileSongs[i].Category0)
                {
                    newFile += "    CATEGORY = 0" + Environment.NewLine;
                }
                if (musFileSongs[i].Category1)
                {
                    newFile += "    CATEGORY = 1" + Environment.NewLine;
                }
                if (musFileSongs[i].Category2)
                {
                    newFile += "    CATEGORY = 2" + Environment.NewLine;
                }
                if (musFileSongs[i].Category3)
                {
                    newFile += "    CATEGORY = 3" + Environment.NewLine;
                }
                if (musFileSongs[i].Category4)
                {
                    newFile += "    CATEGORY = 4" + Environment.NewLine;
                }
                if (musFileSongs[i].DuckToLoops != -1)
                {
                    newFile += "    DUCKTOLOOPS = " + musFileSongs[i].DuckToLoops + Environment.NewLine;
                }
                newFile += "    SEDVALUE = " + musFileSongs[i].SEDValue + Environment.NewLine;
                if (musFileSongs[i].Lowpass != -1)
                {
                    newFile += "    LOWPASS = " + musFileSongs[i].Lowpass + Environment.NewLine;
                }
                if (musFileSongs[i].Preview != -1)
                {
                    newFile += "    PREVIEW = " + musFileSongs[i].Preview + Environment.NewLine;
                }
                newFile += "    SONGBIG = " + musFileSongs[i].SongBig + Environment.NewLine;
                newFile += Environment.NewLine + Environment.NewLine;
            }
            newFile += Environment.NewLine;
            File.WriteAllText(path, newFile);
        }
        string[] GetVar(string varable)
        {
            string[] splitPoint = new string[1] { "=" };
            string[] split = varable.Split(splitPoint, StringSplitOptions.None);
            for (int i = 0; i < split.Length; i++)
            {
                split[i] = split[i].Trim(' ');
            }
            return split;
        }
    }

    public struct MusFileSong
    {
        public string Comment;
        public string ID;
        public int AADTOFE;
        public string Title;
        public string Artist;
        public string Album;
        public string PathData;
        public string MusicData;
        public string loopData;
        public float BPM;
        public int BeatsPerMeasure;
        public int MeasuresPerBar;
        public int PhrasesPerBank;
        public int BeatsPerPhrase;
        public int PhraseAligin;
        public int DelayCount;
        public int DelayTime;
        public int DelayFeedback;
        public int DelayLevel;
        public int PathLevel;
        public int AsyncLevel;
        public bool Category0;
        public bool Category1;
        public bool Category2;
        public bool Category3;
        public bool Category4;
        public int DuckToLoops;
        public int SEDValue;
        public int Lowpass;
        public int Preview;
        public int SongBig;
    }
}
