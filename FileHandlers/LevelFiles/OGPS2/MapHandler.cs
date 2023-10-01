using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.LevelFiles.OGPS2
{
    internal class MapHandler
    {
        public List<LinkerItem> Models = new List<LinkerItem>();
        public List<LinkerItem> Splines = new List<LinkerItem>();
        int LinePos = 12;

        public void Load(string path)
        {
            string[] Lines = File.ReadAllLines(path);
            Models = ReadLinkerItems(Lines);
            LinePos += 3;
            Splines = ReadLinkerItems(Lines, true);
        }

        List<LinkerItem> ReadLinkerItems(string[] Lines, bool Read2 = false)
        {
            var TempList = new List<LinkerItem>();
            while (true)
            {
                if (Lines[LinePos] == ";")
                {
                    break;
                }
                var LinkerItem = new LinkerItem();
                LinkerItem.Name = Lines[LinePos].TrimEnd(' ');
                if (!Read2)
                {
                    LinePos++;
                    LinkerItem.ID = int.Parse(Lines[LinePos].Substring(6, 5).TrimEnd(' '));
                    LinkerItem.Index = int.Parse(Lines[LinePos].Substring(21, 5).TrimEnd(' '));
                }
                else
                {
                    LinePos++;
                    LinkerItem.ID = int.Parse(Lines[LinePos].Substring(4).TrimEnd(' '));
                }
                TempList.Add(LinkerItem);
                LinePos++;
                if (LinePos >= Lines.Length)
                {
                    break;
                }
            }
            return TempList;
        }

        public struct LinkerItem
        {
            public string Name;
            public int ID;
            public int Index;
        }
    }
}
