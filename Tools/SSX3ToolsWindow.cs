using SSXMultiTool.FileHandlers;
using SSXMultiTool.FileHandlers.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSXMultiTool
{
    public partial class SSX3ToolsWindow : Form
    {
        public SSX3ToolsWindow()
        {
            InitializeComponent();
        }

        #region CharDB File
        CHARDBLHandler charHandler = new CHARDBLHandler();
        bool DisableUpdate;
        private void charLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Character DBL|CHARDB.DBL|DataBase List (*.DBL)|*.DBL|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            //openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                charBox1.Items.Clear();
                charHandler.LoadCharFile(openFileDialog.FileName);
                for (int i = 0; i < charHandler.charDBs.Count; i++)
                {
                    charBox1.Items.Add(charHandler.charDBs[i].LongName);
                }
            }
        }

        private void CharApply(object sender, EventArgs e)
        {
            if (!DisableUpdate)
            {
                CharDB temp = new CharDB
                {
                    LongName = chartextBox1.Text,
                    FirstName = chartextBox2.Text,
                    NickName = chartextBox3.Text,
                    BloodType = chartextBox4.Text,
                    Height = chartextBox5.Text,
                    Nationality = chartextBox6.Text,
                    Unkown1 = (int)charnumericUpDown1.Value,
                    Stance = (int)charnumericUpDown2.Value,
                    ModelSize = (int)charnumericUpDown3.Value,
                    Gender = (int)charnumericUpDown4.Value,
                    Age = (int)charnumericUpDown5.Value,
                    Position = (int)charnumericUpDown6.Value
                };
                charHandler.charDBs[charBox1.SelectedIndex] = temp;
                int temp1 = charBox1.SelectedIndex;
                charBox1.Items[temp1] = temp.LongName;
            }
        }

        private void charBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (charBox1.SelectedIndex != -1)
            {
                DisableUpdate = true;
                CharDB temp = charHandler.charDBs[charBox1.SelectedIndex];
                chartextBox1.Text = temp.LongName;
                chartextBox2.Text = temp.FirstName;
                chartextBox3.Text = temp.NickName;
                chartextBox4.Text = temp.BloodType;
                chartextBox5.Text = temp.Height;
                chartextBox6.Text = temp.Nationality;
                charnumericUpDown1.Value = temp.Unkown1;
                charnumericUpDown2.Value = temp.Stance;
                charnumericUpDown3.Value = temp.ModelSize;
                charnumericUpDown4.Value = temp.Gender;
                charnumericUpDown5.Value = temp.Age;
                charnumericUpDown6.Value = temp.Position;
                DisableUpdate = false;
            }
        }

        private void charSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog
            {
                Filter = "Character DBL|CHARDB.DBL|DataBase List (*.DBL)|*.DBL|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                charHandler.SaveCharFile(openFileDialog.FileName);
            }
        }

        private void charSave_Click(object sender, EventArgs e)
        {
            charHandler.SaveCharFile();
        }
        #endregion

        #region BoltPS2 Items
        BoltPS2Handler boltPS2 = new BoltPS2Handler();
        bool loaded = false;
        private void BoltLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "BOLTPS2 File (*.dat)|*.dat|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                boltPS2 = new BoltPS2Handler();
                boltPS2.load(openFileDialog.FileName);
                loaded = true;
                BoltCharacter.SelectedIndex = 0;
                GenerateTreeview();
            }
        }

        List<bool> Parented = new List<bool>();
        int pos = 0;
        void GenerateTreeview()
        {
            BoltPS2TreeView.Nodes.Clear();

            var temp = boltPS2.characters[BoltCharacter.SelectedIndex];
            Parented = new List<bool>();

            for (int i = 0; i < temp.entries.Count; i++)
            {
                Parented.Add(false);
            }
            bool test = false;
            int Testing = 0;
            while (!test)
            {
                for (int i = 0; i < temp.entries.Count; i++)
                {
                    Testing++;
                    if (!Parented[i])
                    {
                        pos = i;
                        //Check Parent Node
                        if (temp.entries[i].ParentID == -1)
                        {
                            Parented[i] = true;
                            BoltPS2TreeView.Nodes.Add(temp.entries[i].ItemID.ToString(), temp.entries[i].ItemID.ToString() + " - " + temp.entries[i].itemName);
                        }
                        else
                        {
                            for (int a = 0; a < BoltPS2TreeView.Nodes.Count; a++)
                            {
                                if (BoltPS2TreeView.Nodes[a].Name == temp.entries[i].ParentID.ToString())
                                {
                                    Parented[i] = true;
                                    BoltPS2TreeView.Nodes[a].Nodes.Add(temp.entries[i].ItemID.ToString(), temp.entries[i].ItemID.ToString() + " - " + temp.entries[i].itemName);
                                    break;
                                }
                                else
                                {
                                    var temp1 = BoltPS2TreeView.Nodes[a];
                                    CheckChildNode(temp1, temp.entries[i]);
                                }
                            }
                        }
                    }
                }


                //Check if list has been ordered
                test = true;
                for (int i = 0; i < Parented.Count; i++)
                {
                    if (!Parented[i])
                    {
                        test = false;
                        break;
                    }
                }
            }
        }

        void CheckChildNode(TreeNode Parent, ItemEntries item)
        {
            for (int i = 0; i < Parent.Nodes.Count; i++)
            {
                if (Parent.Nodes[i].Name == item.ParentID.ToString())
                {
                    Parented[pos] = true;
                    Parent.Nodes[i].Nodes.Add(item.ItemID.ToString(), item.ItemID.ToString() + " - " + item.itemName);
                    //return Parent;
                }
                else
                {
                    CheckChildNode(Parent.Nodes[i], item);
                }
            }
            //return Parent;
        }

        private void BoltSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog
            {
                Filter = "BIG File (*.dat)|*.dat|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                boltPS2.Save(openFileDialog.FileName);
            }
        }

        private void BoltDupe_Click(object sender, EventArgs e)
        {
            if (BoltPS2TreeView.SelectedNode != null)
            {
                int CharIndex = BoltCharacter.SelectedIndex;
                int Index = -1;

                for (int i = 0; i < boltPS2.characters[CharIndex].entries.Count; i++)
                {
                    if (BoltPS2TreeView.SelectedNode.Name == boltPS2.characters[CharIndex].entries[i].ItemID.ToString())
                    {
                        Index = i;
                        break;
                    }
                }

                var temp1 = boltPS2.characters[CharIndex];
                var temp = temp1.entries[Index];

                temp1.entries.Add(temp);

                boltPS2.characters[CharIndex] = temp1;
            }
        }

        private void BoltApply_Click(object sender, EventArgs e)
        {
            if (BoltPS2TreeView.SelectedNode != null && loaded)
            {
                int Index1 = BoltCharacter.SelectedIndex;
                int Index = 0;

                for (int i = 0; i < boltPS2.characters[Index1].entries.Count; i++)
                {
                    if (BoltPS2TreeView.SelectedNode.Name == boltPS2.characters[Index1].entries[i].ItemID.ToString())
                    {
                        Index = i;
                        break;
                    }
                }
                var Char = boltPS2.characters[Index1];
                var tempEntry = Char.entries[Index];

                tempEntry.unkownInt1 = (int)BoltUnkownOne.Value;
                tempEntry.Unlock = (int)BoltUnlock.Value;
                tempEntry.unkownInt2 = (int)BoltUnkownTwo.Value;
                tempEntry.ItemID = (int)BoltUnkownThree.Value;
                tempEntry.ParentID = (int)BoltUnkownFour.Value;
                tempEntry.category = (int)BoltCat.Value;
                tempEntry.buyable = (int)BoltBuy.Value;
                tempEntry.menuOrder = (int)BoltMenuOrder.Value;
                tempEntry.unkownInt5 = (int)BoltUnkown7.Value;
                tempEntry.weight = (int)BoltFillBar.Value;
                tempEntry.cost = (int)BoltCost.Value;
                tempEntry.unkownInt8 = (int)BoltUnkown8.Value;

                tempEntry.SpecialID = (int)BoltSpecialOne.Value;
                tempEntry.SpecialID2 = (int)BoltSpecialTwo.Value;
                tempEntry.SpecialID3 = (int)BoltSpecialThree.Value;

                tempEntry.itemName = BoltName.Text;
                tempEntry.ModelID = BoltModelID.Text;
                tempEntry.ModelID2 = BoltModelIDTwo.Text;
                tempEntry.ModelID3 = BoltModelIDThree.Text;
                tempEntry.ModelID4 = BoltModelIDFour.Text;
                tempEntry.ModelPath = BoltModelPath.Text;
                tempEntry.TexturePath = BoltTexturePath.Text;
                tempEntry.SmallIcon = BoltIconPath.Text;

                tempEntry.unkownInt6 = (int)BoltUnkown9.Value;


                Char.entries[Index] = tempEntry;
                boltPS2.characters[Index1] = Char;

                GenerateTreeview();
            }
        }

        private void BoltCharacter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BoltCharacter.SelectedIndex != -1 && loaded)
            {
                GenerateTreeview();
            }
        }

        private void BoltPS2TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (BoltPS2TreeView.SelectedNode != null && loaded)
            {
                int Index1 = BoltCharacter.SelectedIndex;
                int Index = 0;

                for (int i = 0; i < boltPS2.characters[Index1].entries.Count; i++)
                {
                    if (BoltPS2TreeView.SelectedNode.Name == boltPS2.characters[Index1].entries[i].ItemID.ToString())
                    {
                        Index = i;
                        break;
                    }
                }


                BoltUnkownOne.Value = boltPS2.characters[Index1].entries[Index].unkownInt1;
                BoltUnlock.Value = boltPS2.characters[Index1].entries[Index].Unlock;
                BoltUnkownTwo.Value = boltPS2.characters[Index1].entries[Index].unkownInt2;
                BoltUnkownThree.Value = boltPS2.characters[Index1].entries[Index].ItemID;
                BoltUnkownFour.Value = boltPS2.characters[Index1].entries[Index].ParentID;
                BoltCat.Value = boltPS2.characters[Index1].entries[Index].category;
                BoltBuy.Value = boltPS2.characters[Index1].entries[Index].buyable;
                BoltMenuOrder.Value = boltPS2.characters[Index1].entries[Index].menuOrder;
                BoltUnkown7.Value = boltPS2.characters[Index1].entries[Index].unkownInt5;
                BoltFillBar.Value = boltPS2.characters[Index1].entries[Index].weight;
                BoltCost.Value = boltPS2.characters[Index1].entries[Index].cost;
                BoltUnkown8.Value = boltPS2.characters[Index1].entries[Index].unkownInt8;

                BoltSpecialOne.Value = boltPS2.characters[Index1].entries[Index].SpecialID;
                BoltSpecialTwo.Value = boltPS2.characters[Index1].entries[Index].SpecialID2;
                BoltSpecialThree.Value = boltPS2.characters[Index1].entries[Index].SpecialID3;

                BoltName.Text = boltPS2.characters[Index1].entries[Index].itemName;
                BoltModelID.Text = boltPS2.characters[Index1].entries[Index].ModelID;
                BoltModelIDTwo.Text = boltPS2.characters[Index1].entries[Index].ModelID2;
                BoltModelIDThree.Text = boltPS2.characters[Index1].entries[Index].ModelID3;
                BoltModelIDFour.Text = boltPS2.characters[Index1].entries[Index].ModelID4;
                BoltModelPath.Text = boltPS2.characters[Index1].entries[Index].ModelPath;
                BoltTexturePath.Text = boltPS2.characters[Index1].entries[Index].TexturePath;
                BoltIconPath.Text = boltPS2.characters[Index1].entries[Index].SmallIcon;

                BoltUnkown9.Value = boltPS2.characters[Index1].entries[Index].unkownInt6;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < boltPS2.unkown2.Count; i++)
            {
                var temp = boltPS2.unkown2[i];
                temp.UnkownInt4 = 0;
                boltPS2.unkown2[i] = temp;
            }
        }

        #endregion

        #region MusicINF File
        MusicINFHandler musicINFHandler = new MusicINFHandler();

        private void MusicLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Music Config |MUSIC.INF|Config File (*.INF)|*.INF|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                musiclistBox1.Items.Clear();
                musicINFHandler.musPath = openFileDialog.FileName;
                musicINFHandler.LoadMusFile(openFileDialog.FileName);
                for (int i = 0; i < musicINFHandler.musFileSongs.Count; i++)
                {
                    musiclistBox1.Items.Add(musicINFHandler.musFileSongs[i].ID);
                }
            }
        }
        bool musHold;
        private void musiclistBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            musHold = true;
            if (musiclistBox1.SelectedIndex != -1)
            {
                MusicID.Text = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].ID;
                MusicTitleBox.Text = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].Title;
                MusicArtistBox.Text = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].Artist;
                MusicAlbumBox.Text = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].Album;
                MusicPathDataBox.Text = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].PathData;
                MusicDataBox.Text = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].MusicData;
                MusicLoopData.Text = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].loopData;
                MusINFnumericUpDown1.Value = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].BeatsPerMeasure;
                MusINFnumericUpDown2.Value = (decimal)musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].BPM;
                MusINFnumericUpDown3.Value = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].SEDValue;
                MusINFnumericUpDown4.Value = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].Lowpass;
                MusINFnumericUpDown5.Value = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].Preview;
                MusINFnumericUpDown6.Value = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].MeasuresPerBar;
                MusINFnumericUpDown7.Value = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].PhrasesPerBank;
                MusINFnumericUpDown8.Value = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].BeatsPerMeasure;
                MusINFnumericUpDown9.Value = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].BeatsPerPhrase;
                MusINFnumericUpDown10.Value = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].AsyncLevel;
                MusINFnumericUpDown11.Value = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].SongBig;
                MusINFnumericUpDown12.Value = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].DuckToLoops;
                MusnumericUpDown0.Value = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].AADTOFE;
                MuscheckBox1.Checked = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].Category0;
                MuscheckBox2.Checked = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].Category1;
                MuscheckBox3.Checked = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].Category2;
                MuscheckBox4.Checked = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].Category3;
                MuscheckBox5.Checked = musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex].Category4;
            }
            musHold = false;
        }

        private void MusUpdate(object sender, EventArgs e)
        {
            if (musiclistBox1.SelectedIndex != -1 && !musHold)
            {
                MusFileSong temp = new MusFileSong();
                temp.ID = MusicID.Text;
                temp.Title = MusicTitleBox.Text;
                temp.Artist = MusicArtistBox.Text;
                temp.Album = MusicAlbumBox.Text;
                temp.PathData = MusicPathDataBox.Text;
                temp.MusicData = MusicDataBox.Text;
                temp.loopData = MusicLoopData.Text;
                temp.BeatsPerMeasure = (int)MusINFnumericUpDown1.Value;
                temp.BPM = (int)MusINFnumericUpDown2.Value;
                temp.SEDValue = (int)MusINFnumericUpDown3.Value;
                temp.Lowpass = (int)MusINFnumericUpDown4.Value;
                temp.Preview = (int)MusINFnumericUpDown5.Value;
                temp.MeasuresPerBar = (int)MusINFnumericUpDown6.Value;
                temp.PhrasesPerBank = (int)MusINFnumericUpDown7.Value;
                temp.BeatsPerMeasure = (int)MusINFnumericUpDown8.Value;
                temp.BeatsPerPhrase = (int)MusINFnumericUpDown9.Value;
                temp.AsyncLevel = (int)MusINFnumericUpDown10.Value;
                temp.SongBig = (int)MusINFnumericUpDown11.Value;
                temp.DuckToLoops = (int)MusINFnumericUpDown12.Value;
                temp.AADTOFE = (int)MusnumericUpDown0.Value;
                temp.Category0 = MuscheckBox1.Checked;
                temp.Category1 = MuscheckBox2.Checked;
                temp.Category2 = MuscheckBox3.Checked;
                temp.Category3 = MuscheckBox4.Checked;
                temp.Category4 = MuscheckBox5.Checked;

                musicINFHandler.musFileSongs[musiclistBox1.SelectedIndex] = temp;

            }
        }

        private void MusSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog
            {
                Filter = "Config File (*.INF)|*.INF|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                musicINFHandler.SaveMusFile(openFileDialog.FileName);
            }
        }

        private void MusAdd_Click(object sender, EventArgs e)
        {
            MusFileSong song = new MusFileSong();
            song.ID = "[Null]";
            musicINFHandler.musFileSongs.Add(song);
            musiclistBox1.Items.Clear();
            for (int i = 0; i < musicINFHandler.musFileSongs.Count; i++)
            {
                musiclistBox1.Items.Add(musicINFHandler.musFileSongs[i].ID);
            }
        }

        private void MusRemove_Click(object sender, EventArgs e)
        {
            if (musiclistBox1.SelectedIndex != -1)
            {
                musicINFHandler.musFileSongs.RemoveAt(musiclistBox1.SelectedIndex);
                musiclistBox1.Items.Clear();
                for (int i = 0; i < musicINFHandler.musFileSongs.Count; i++)
                {
                    musiclistBox1.Items.Add(musicINFHandler.musFileSongs[i].ID);
                }
            }
        }
        #endregion

        SSX3MPFModelHandler modelHandler = new SSX3MPFModelHandler();
        SSX3ModelCombiner ssx3ModelCombiner = new SSX3ModelCombiner();

        private void MPFLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Model File (*.mpf)|*.mpf|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                modelHandler = new SSX3MPFModelHandler();
                ssx3ModelCombiner = new SSX3ModelCombiner();
                modelHandler.load(openFileDialog.FileName);
                ssx3ModelCombiner.AddFile(modelHandler);

                MPFWarningLabel.Text = ssx3ModelCombiner.CheckBones(0);


                MpfModelList.Items.Clear();
                for (int i = 0; i < modelHandler.ModelList.Count; i++)
                {
                    MpfModelList.Items.Add(modelHandler.ModelList[i].ModelName);
                }
            }
        }

        private void MPFSaveDecompressed_Click(object sender, EventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog
            {
                Filter = "Model File (*.mpf)|*.mpf|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                modelHandler.Save(openFileDialog.FileName, false);
            }
        }

        private void MpfExport_Click(object sender, EventArgs e)
        {
            if(MpfModelList.SelectedIndex!=-1)
            {
                SaveFileDialog openFileDialog = new SaveFileDialog
                {
                    Filter = "Model File (*.glb)|*.glb|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ssx3ModelCombiner.MeshReassigned(MpfModelList.SelectedIndex);

                    glftHandler.SaveSSX3Glft(openFileDialog.FileName, ssx3ModelCombiner);
                }
            }
        }

        private void MpfBoneLoad_Click(object sender, EventArgs e)
        {
            if (ssx3ModelCombiner.modelHandlers != null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Model File (*.mpf)|*.mpf|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    modelHandler = new SSX3MPFModelHandler();
                    modelHandler.load(openFileDialog.FileName);
                    ssx3ModelCombiner.AddBones(modelHandler);

                    MPFWarningLabel.Text = ssx3ModelCombiner.CheckBones(0);
                }
            }
        }

        private void MpfSaveCompressed_Click(object sender, EventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog
            {
                Filter = "Model File (*.mpf)|*.mpf|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                modelHandler.Save(openFileDialog.FileName, true);
            }
        }
    }
}
