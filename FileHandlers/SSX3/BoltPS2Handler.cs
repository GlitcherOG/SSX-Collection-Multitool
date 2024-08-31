using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers
{
    public class BoltPS2Handler
    {
        int unknown; //Store items?
        int ItemEntryAmmount; //Possibly Textures?
        public List<Character> characters = new List<Character>();
        int ammount2;
        public List<Unkown2> unkown2 = new List<Unkown2>();
        int ammount3;
        public List<Unkown3> unkown3 = new List<Unkown3>();
        int ammount4;
        public List<Unkown3> unkown4 = new List<Unkown3>();
        int StringListLength; // Matches The Used Portion of the List
        public List<string> StringList = new List<string>();
        public List<int> StringPos = new List<int>();
        public void load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                characters = new List<Character>();

                unknown = StreamUtil.ReadUInt32(stream);

                //4829
                ItemEntryAmmount = StreamUtil.ReadUInt32(stream);

                for (int i = 0; i < ItemEntryAmmount; i++)
                {
                    //Read 56 bytes
                    ItemEntries temp1 = new ItemEntries();
                    temp1.CharacterID = stream.ReadByte(); //1
                    temp1.unkownInt1 = stream.ReadByte(); //2
                    temp1.Unlock = stream.ReadByte(); //3
                    temp1.unkownInt2 = stream.ReadByte(); //4
                    temp1.ItemID = StreamUtil.ReadInt16(stream);
                    temp1.ParentID = StreamUtil.ReadInt16(stream);

                    temp1.category = stream.ReadByte(); //9
                    temp1.buyable = stream.ReadByte(); //10
                    temp1.menuOrder = stream.ReadByte(); //11

                    temp1.unkownInt5 = stream.ReadByte(); //12
                    
                    temp1.weight = StreamUtil.ReadInt16(stream); //13-14
                    temp1.Cost = StreamUtil.ReadInt16(stream); //15-16
                    temp1.FileID = stream.ReadByte(); //17
                    temp1.SpecialID = stream.ReadByte(); //18
                    temp1.SpecialID2 = stream.ReadByte(); //19
                    temp1.SpecialID3 = stream.ReadByte(); //20

                    temp1.nameOffset = StreamUtil.ReadUInt32(stream);
                    temp1.ModelIDOffset = StreamUtil.ReadUInt32(stream);
                    temp1.ModelID2Offset = StreamUtil.ReadUInt32(stream);
                    temp1.ModelID3Offset = StreamUtil.ReadUInt32(stream);
                    temp1.ModelID4Offset = StreamUtil.ReadUInt32(stream);
                    temp1.ModelPathOffset = StreamUtil.ReadUInt32(stream);
                    temp1.TexturePathOffset = StreamUtil.ReadUInt32(stream);
                    temp1.SmallIconOffset = StreamUtil.ReadUInt32(stream);

                    temp1.unkownInt6 = StreamUtil.ReadUInt32(stream);
                    
                    if(characters.Count-1 != temp1.CharacterID)
                    {
                        Character tempSlot = new Character();
                        tempSlot.entries = new List<ItemEntries>();
                        tempSlot.entries.Add(temp1);
                        characters.Add(tempSlot);
                    }
                    else
                    {
                        var tempSlot = characters[temp1.CharacterID];
                        tempSlot.entries.Add(temp1);
                        characters[temp1.CharacterID] = tempSlot;
                    }
                }

                //7327
                ammount2 = StreamUtil.ReadUInt32(stream);

                for (int i = 0; i < ammount2; i++)
                {
                    //Read 12 bytes
                    Unkown2 temp2 = new Unkown2();
                    temp2.CharacterID = stream.ReadByte();
                    temp2.BoolInt = stream.ReadByte(); //Effects what can be equiped with what
                    temp2.UnkownInt = StreamUtil.ReadInt16(stream); //Effects model loading (Possible Item ID)
                    temp2.UnkownInt2 = stream.ReadByte(); // No idea

                    //All seem to be relyant on the next
                    temp2.UnkownInt3 = stream.ReadByte(); //No idea
                    temp2.UnkownInt4 = StreamUtil.ReadInt16(stream);
                    temp2.UnkownInt5 = stream.ReadByte();
                    temp2.BoolInt2 = stream.ReadByte();
                    temp2.UnkownInt7 = StreamUtil.ReadInt16(stream);
                    unkown2.Add(temp2);
                }

                //152
                ammount3 = StreamUtil.ReadUInt32(stream);

                for (int i = 0; i < ammount3; i++)
                {
                    //Read 8 bytes
                    Unkown3 temp3 = new Unkown3();
                    temp3.UnkownInt = StreamUtil.ReadUInt32(stream);
                    temp3.UnkownInt2 = StreamUtil.ReadInt16(stream);
                    temp3.UnkownInt3 = StreamUtil.ReadInt16(stream);
                    unkown3.Add(temp3);
                }

                //88
                ammount4 = StreamUtil.ReadUInt32(stream);

                for (int i = 0; i < ammount4; i++)
                {
                    //Read 8 Bytes
                    Unkown3 temp4 = new Unkown3();
                    temp4.UnkownInt = StreamUtil.ReadUInt32(stream);
                    temp4.UnkownInt2 = StreamUtil.ReadInt16(stream);
                    temp4.UnkownInt3 = StreamUtil.ReadInt16(stream);
                    unkown4.Add(temp4);
                }

                //181173
                StringListLength = StreamUtil.ReadUInt32(stream);
                long startpos = stream.Position;

                while (stream.Position < stream.Length - 1)
                {
                    int pos = (int)(stream.Position - startpos+1);
                    string tempstring = StreamUtil.ReadNullEndString(stream);
                    StringList.Add(tempstring);
                    StringPos.Add(pos);
                    stream.Position++;
                }

                //Grab strings based on offsets
                for (int i = 0; i < characters.Count; i++)
                {
                    var tempChar = characters[i];
                    for (int a = 0; a < tempChar.entries.Count; a++)
                    {
                        var temp5 = tempChar.entries[a];
                        if (StringPos.IndexOf(temp5.nameOffset) != -1)
                        {
                            temp5.itemName = StringList[StringPos.IndexOf(temp5.nameOffset)];
                        }
                        if (StringPos.IndexOf(temp5.ModelIDOffset) != -1)
                        {
                            temp5.ModelID = StringList[StringPos.IndexOf(temp5.ModelIDOffset)];
                        }
                        if (StringPos.IndexOf(temp5.ModelID2Offset) != -1)
                        {
                            temp5.ModelID2 = StringList[StringPos.IndexOf(temp5.ModelID2Offset)];
                        }
                        if (StringPos.IndexOf(temp5.ModelID3Offset) != -1)
                        {
                            temp5.ModelID3 = StringList[StringPos.IndexOf(temp5.ModelID3Offset)];
                        }
                        if (StringPos.IndexOf(temp5.ModelID4Offset) != -1)
                        {
                            temp5.ModelID4 = StringList[StringPos.IndexOf(temp5.ModelID4Offset)];
                        }
                        if (StringPos.IndexOf(temp5.ModelPathOffset) != -1)
                        {
                            temp5.ModelPath = StringList[StringPos.IndexOf(temp5.ModelPathOffset)];
                        }
                        if (StringPos.IndexOf(temp5.TexturePathOffset) != -1)
                        {
                            temp5.TexturePath = StringList[StringPos.IndexOf(temp5.TexturePathOffset)];
                        }
                        if (StringPos.IndexOf(temp5.SmallIconOffset) != -1)
                        {
                            temp5.SmallIcon = StringList[StringPos.IndexOf(temp5.SmallIconOffset)];
                        }
                        tempChar.entries[a] = temp5;
                    }
                    //tempChar.entries.Sort((s1, s2) => s1.ItemID.CompareTo(s2.ItemID));
                    characters[i] = tempChar;
                }

                stream.Dispose();
                stream.Close();
            }
        }

        //public void Test()
        //{
        //    for (int i = 0; i < unkown1.Count; i++)
        //    {
        //        var temp = unkown1[i];

        //        //temp.unkown1[27] = 0x01;

        //        unkown1[i] = temp;
        //    }
        //}

        public void Save(string path)
        {
            Stream stream = new MemoryStream();
            Stream streamString = new MemoryStream();

            StreamUtil.WriteInt32(stream, unknown);
            int ListCount = 0;

            for (int i = 0; i < characters.Count; i++)
            {
                for (int a = 0; a < characters[i].entries.Count; a++)
                {
                    ListCount++;
                }
            }

            StreamUtil.WriteInt32(stream, ListCount);

            StringList = new List<string>();
            StringPos = new List<int>();

            for (int i = 0; i < characters.Count; i++)
            {
                var TempCharEntry = characters[i];
                for (int a = 0; a < TempCharEntry.entries.Count; a++)
                {
                    var TempEntry = TempCharEntry.entries[a];
                    stream.WriteByte((byte)TempEntry.CharacterID);
                    stream.WriteByte((byte)TempEntry.unkownInt1);
                    stream.WriteByte((byte)TempEntry.Unlock);
                    stream.WriteByte((byte)TempEntry.unkownInt2);

                    StreamUtil.WriteInt16(stream, TempEntry.ItemID);
                    StreamUtil.WriteInt16(stream, TempEntry.ParentID);

                    stream.WriteByte((byte)TempEntry.category);
                    stream.WriteByte((byte)TempEntry.buyable);
                    stream.WriteByte((byte)TempEntry.menuOrder);

                    stream.WriteByte((byte)TempEntry.unkownInt5);

                    StreamUtil.WriteInt16(stream, TempEntry.weight);
                    StreamUtil.WriteInt16(stream, TempEntry.Cost);

                    stream.WriteByte((byte)TempEntry.FileID);
                    stream.WriteByte((byte)TempEntry.SpecialID);
                    stream.WriteByte((byte)TempEntry.SpecialID2);
                    stream.WriteByte((byte)TempEntry.SpecialID3);

                    WriteStringList(stream, streamString, TempEntry.itemName);
                    WriteStringList(stream, streamString, TempEntry.ModelID);
                    WriteStringList(stream, streamString, TempEntry.ModelID2);
                    WriteStringList(stream, streamString, TempEntry.ModelID3);
                    WriteStringList(stream, streamString, TempEntry.ModelID4);
                    WriteStringList(stream, streamString, TempEntry.ModelPath);
                    WriteStringList(stream, streamString, TempEntry.TexturePath);
                    WriteStringList(stream, streamString, TempEntry.SmallIcon);

                    StreamUtil.WriteInt32(stream, TempEntry.unkownInt6);
                }
            }

            StreamUtil.WriteInt32(stream, unkown2.Count);

            for (int i = 0; i < unkown2.Count; i++)
            {
                var TempEntry = unkown2[i];

                stream.WriteByte((byte)TempEntry.CharacterID);
                stream.WriteByte((byte)TempEntry.BoolInt);
                StreamUtil.WriteInt16(stream, TempEntry.UnkownInt);
                stream.WriteByte((byte)TempEntry.UnkownInt2);
                stream.WriteByte((byte)TempEntry.UnkownInt3);
                StreamUtil.WriteInt16(stream, TempEntry.UnkownInt4);
                stream.WriteByte((byte)TempEntry.UnkownInt5);
                stream.WriteByte((byte)TempEntry.BoolInt2);
                StreamUtil.WriteInt16(stream, TempEntry.UnkownInt7);
            }

            StreamUtil.WriteInt32(stream, unkown3.Count);

            for (int i = 0; i < unkown3.Count; i++)
            {
                var TempEntry = unkown3[i];

                StreamUtil.WriteInt32(stream, TempEntry.UnkownInt);
                StreamUtil.WriteInt16(stream, TempEntry.UnkownInt2);
                StreamUtil.WriteInt16(stream, TempEntry.UnkownInt3);
            }

            StreamUtil.WriteInt32(stream, unkown4.Count);

            for (int i = 0; i < unkown4.Count; i++)
            {
                var TempEntry = unkown4[i];

                StreamUtil.WriteInt32(stream, TempEntry.UnkownInt);
                StreamUtil.WriteInt16(stream, TempEntry.UnkownInt2);
                StreamUtil.WriteInt16(stream, TempEntry.UnkownInt3);
            }

            StreamUtil.WriteInt32(stream, (int)streamString.Position);

            streamString.Position = 0;
            byte[] bytes = new byte[streamString.Length];
            streamString.Read(bytes, 0, bytes.Length);
            StreamUtil.WriteBytes(stream, bytes);

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

        public void WriteStringList(Stream stream, Stream stream1, string StringTest)
        {
            int offset = 0;
            if (StringTest != "" && StringTest != null)
            {
                if (StringList.IndexOf(StringTest) != -1)
                {
                    offset = StringPos[StringList.IndexOf(StringTest)];
                }
                else
                {
                    offset = (int)stream1.Position + 1;
                    StringPos.Add(offset);
                    StringList.Add(StringTest);
                    StreamUtil.WriteNullString(stream1, StringTest);
                }
            }
            StreamUtil.WriteInt32(stream, offset);
        }

    }

    public struct Character
    {
        public List<ItemEntries> entries;
    }

    public struct ItemEntries
    {
        public int CharacterID;
        public int unkownInt1;
        public int Unlock;
        public int unkownInt2;
        public int ItemID;
        public int ParentID; //Effects Equip Postion??
        public int category;
        public int buyable;
        public int menuOrder;
        public int unkownInt5;
        public int weight;
        public int Cost;
        public int FileID;

        public int SpecialID;
        public int SpecialID2;
        public int SpecialID3;

        public int nameOffset;
        public string itemName;
        public int ModelIDOffset;
        public string ModelID;
        public int ModelID2Offset;
        public string ModelID2;
        public int ModelID3Offset;
        public string ModelID3;
        public int ModelID4Offset;
        public string ModelID4;
        public int ModelPathOffset;
        public string ModelPath;
        public int TexturePathOffset;
        public string TexturePath;
        public int SmallIconOffset;
        public string SmallIcon;

        public int unkownInt6; //Effects Equip??
    }

    public struct Unkown2
    {
        public int CharacterID;
        public int BoolInt;
        public int UnkownInt;
        public int UnkownInt2;
        public int UnkownInt3;
        public int UnkownInt4;
        public int UnkownInt5;
        public int UnkownInt6;
        public int BoolInt2;
        public int UnkownInt7;
    }

    public struct Unkown3
    {
        public int UnkownInt;
        public int UnkownInt2;
        public int UnkownInt3;
    }
}
