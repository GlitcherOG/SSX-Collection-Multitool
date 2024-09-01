using SSXMultiTool.FileHandlers;
using SSXMultiTool.FileHandlers.Models.SSX3;
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
using SSXMultiTool.FileHandlers.SSX3;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using NAudio.Wave;

namespace SSXMultiTool
{
    public partial class SSX3ToolsWindow : Form
    {
        public SSX3ToolsWindow()
        {
            InitializeComponent();
            charcomboBox1.SelectedIndex = 0;
            MorphScaleNumber.Value = (decimal)SSX3PS2MPF.MorphScale;
        }

        #region CharDB File
        CHARDBLHandler charHandler = new CHARDBLHandler();
        bool DisableUpdate;
        private void charLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Character DBL|CHARDB.DBL|Cheat Character DBL|CHCHARDB.DBL|DataBase List (*.DBL)|*.DBL|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            //openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                charBox1.Items.Clear();
                charHandler.LoadCharFile(openFileDialog.FileName, charcomboBox1.SelectedIndex);
                for (int i = 0; i < charHandler.charDBs.Count; i++)
                {
                    charBox1.Items.Add(charHandler.charDBs[i].LongName);
                }
            }
        }

        private void CharApply(object sender, EventArgs e)
        {
            if (!DisableUpdate && charBox1.SelectedIndex != -1)
            {
                CharDB temp = new CharDB
                {
                    FirstNameEnglish = chartextBox0.Text,
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
                chartextBox0.Text = temp.FirstNameEnglish;
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
                charHandler.SaveCharFile(openFileDialog.FileName, charcomboBox1.SelectedIndex);
            }
        }

        private void charSave_Click(object sender, EventArgs e)
        {
            charHandler.SaveCharFile(null, charcomboBox1.SelectedIndex);
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
                tempEntry.Cost = (int)BoltCost.Value;
                tempEntry.FileID = (int)BoltFileID.Value;

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
                BoltCost.Value = boltPS2.characters[Index1].entries[Index].Cost;
                BoltFileID.Value = boltPS2.characters[Index1].entries[Index].FileID;

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

        SSX3PS2MPF modelHandler = new SSX3PS2MPF();
        SSX3PS2ModelCombiner ssx3ModelCombiner = new SSX3PS2ModelCombiner();

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
                if (MpfHeaderChecker.DetectFileType(openFileDialog.FileName) != 2)
                {
                    MessageBox.Show(MpfHeaderChecker.TypeErrorMessage(MpfHeaderChecker.DetectFileType(openFileDialog.FileName)));
                    return;
                }

                modelHandler = new SSX3PS2MPF();
                ssx3ModelCombiner = new SSX3PS2ModelCombiner();
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
                MessageBox.Show("Warning Unable to be loaded in game");
                ssx3ModelCombiner.SaveMPF(openFileDialog.FileName, false);
            }
        }

        private void MpfExport_Click(object sender, EventArgs e)
        {
            if (MpfModelList.SelectedIndex != -1)
            {
                if (ssx3ModelCombiner.CheckBones(MpfModelList.SelectedIndex) != "")
                {
                    MessageBox.Show(ssx3ModelCombiner.CheckBones(MpfModelList.SelectedIndex));
                    return;
                }
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
                    if (MpfHeaderChecker.DetectFileType(openFileDialog.FileName) != 2)
                    {
                        MessageBox.Show(MpfHeaderChecker.TypeErrorMessage(MpfHeaderChecker.DetectFileType(openFileDialog.FileName)));
                        return;
                    }


                    modelHandler = new SSX3PS2MPF();
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
                ssx3ModelCombiner.SaveMPF(openFileDialog.FileName, true);
            }
        }

        private void MpfImport_Click(object sender, EventArgs e)
        {
            if (MpfModelList.SelectedIndex != -1)
            {
                if (ssx3ModelCombiner.CheckBones(MpfModelList.SelectedIndex) != "")
                {
                    MessageBox.Show(ssx3ModelCombiner.CheckBones(MpfModelList.SelectedIndex));
                    return;
                }
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "gltf File (*.glb)|*.glb|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SSX3PS2ModelCombiner TempCombiner = null;

                    try
                    {
                        TempCombiner = glftHandler.LoadSSX3Glft(openFileDialog.FileName);
                    }
                    catch
                    {
                        MessageBox.Show("Failed to Load File");
                    }

                    try
                    {
                        ssx3ModelCombiner.NormalAverage = ImportAverageNormal.Checked;
                        ssx3ModelCombiner.UpdateBones = BoneUpdateCheck.Checked;
                        ssx3ModelCombiner.StartRegenMesh(TempCombiner, MpfModelList.SelectedIndex);
                        UpdateData();
                    }
                    catch
                    {
                        MessageBox.Show("Failed to Convert File");
                    }
                }
            }
        }

        private void MpfModelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        void UpdateData()
        {
            if (MpfModelList.SelectedIndex != -1)
            {
                var TempModel = ssx3ModelCombiner.modelHandlers.ModelList[MpfModelList.SelectedIndex];

                MpfModelName.Text = TempModel.ModelName;
                FileID.Text = TempModel.FileID.ToString();
                BoneCount.Text = TempModel.BoneCount.ToString();
                MaterialCount.Text = TempModel.MaterialCount.ToString();
                IkCount.Text = "0";
                ShapeKeyCount.Text = TempModel.MorphKeyCount.ToString();
                MpfWeights.Text = TempModel.WeightCount.ToString();

                TristripCountLabel.Text = ssx3ModelCombiner.TristripCount(TempModel).ToString();
                VerticeCount.Text = ssx3ModelCombiner.VerticeCount(TempModel).ToString();
                MeshChunks.Text = ssx3ModelCombiner.ChunkCount(TempModel).ToString();
                MaterialGroupCount.Text = TempModel.MaterialGroupList.Count.ToString();
                WeightGroupCount.Text = ssx3ModelCombiner.WeigthRefCount(TempModel).ToString();
                MorphGroupCount.Text = ssx3ModelCombiner.MorphGroupCount(TempModel).ToString();

                MaterialList.Items.Clear();
                for (int i = 0; i < TempModel.MaterialList.Count; i++)
                {
                    MaterialList.Items.Add(TempModel.MaterialList[i].MainTexture);
                }
            }
            else
            {
                FileID.Text = "0";
                BoneCount.Text = "0";
                MaterialCount.Text = "0";
                IkCount.Text = "0";
                ShapeKeyCount.Text = "0";
                MpfWeights.Text = "0";

                TristripCountLabel.Text = "0";
                VerticeCount.Text = "0";
                MeshChunks.Text = "0";
                MaterialGroupCount.Text = "0";
                WeightGroupCount.Text = "0";
                MorphGroupCount.Text = "0";
                MaterialList.Items.Clear();
            }
        }

        private void BiglessStore_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "BOLTPS2 File (*.dat)|*.dat|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var TempboltPS2 = new BoltPS2Handler();
                TempboltPS2.load(openFileDialog.FileName);
                for (int i = 0; i < TempboltPS2.characters.Count; i++)
                {
                    var TempCharacter = TempboltPS2.characters[i];
                    for (int a = 0; a < TempCharacter.entries.Count; a++)
                    {
                        var TempEntries = TempCharacter.entries[a];

                        if (TempEntries.ModelPath != null)
                        {
                            if (TempEntries.ModelPath.ToLower().Contains(".big|"))
                            {
                                TempEntries.ModelPath = TempEntries.ModelPath.ToLower().Replace(".big|", "/");
                            }
                        }

                        if (TempEntries.TexturePath != null)
                        {
                            if (TempEntries.TexturePath.ToLower().Contains(".big|"))
                            {
                                TempEntries.TexturePath = TempEntries.TexturePath.ToLower().Replace(".big|", "/");
                            }
                        }
                        TempCharacter.entries[a] = TempEntries;
                    }
                    TempboltPS2.characters[i] = TempCharacter;
                }

                TempboltPS2.Save(openFileDialog.FileName);
            }
        }

        private void ModelListLabel_Click(object sender, EventArgs e)
        {
            MPFSaveDecompressed.Visible = true;
        }

        private void MaterialList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MaterialList.SelectedIndex != -1 && !DisableUpdate)
            {
                DisableUpdate = true;
                var TempModel = ssx3ModelCombiner.modelHandlers.ModelList[MpfModelList.SelectedIndex].MaterialList[MaterialList.SelectedIndex];
                MatMainTexture.Text = TempModel.MainTexture;
                MatTextureFlag1.Text = TempModel.Texture1;
                MatTextureFlag2.Text = TempModel.Texture2;
                MatTextureFlag3.Text = TempModel.Texture3;
                MatTextureFlag4.Text = TempModel.Texture4;

                MatFlagFactor.Value = (decimal)TempModel.FactorFloat;
                MatUnknown1.Value = (decimal)TempModel.Unused1Float;
                MatUnknown2.Value = (decimal)TempModel.Unused2Float;

                DisableUpdate = false;
            }
        }

        private void MatUpdate(object sender, EventArgs e)
        {
            if (!DisableUpdate && MaterialList.SelectedIndex != -1)
            {
                DisableUpdate = true;
                var TempModel = ssx3ModelCombiner.modelHandlers.ModelList[MpfModelList.SelectedIndex];
                var TempMaterial = TempModel.MaterialList[MaterialList.SelectedIndex];

                TempMaterial.MainTexture = MatMainTexture.Text;
                TempMaterial.Texture1 = MatTextureFlag1.Text;
                TempMaterial.Texture2 = MatTextureFlag2.Text;
                TempMaterial.Texture3 = MatTextureFlag3.Text;
                TempMaterial.Texture4 = MatTextureFlag4.Text;

                TempMaterial.FactorFloat = (float)MatFlagFactor.Value;
                TempMaterial.Unused1Float = (float)MatUnknown1.Value;
                TempMaterial.Unused2Float = (float)MatUnknown2.Value;

                MaterialList.Items[MaterialList.SelectedIndex] = TempMaterial.MainTexture;

                TempModel.MaterialList[MaterialList.SelectedIndex] = TempMaterial;
                ssx3ModelCombiner.modelHandlers.ModelList[MpfModelList.SelectedIndex] = TempModel;

                DisableUpdate = false;
            }
        }

        private void LoadLUI_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "LUI File (*.LUI)|*.LUI|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LUIHandler lUIHandler = new LUIHandler();
                lUIHandler.LoadLUIFile(openFileDialog.FileName);
                lUIHandler.SaveDecompressed(openFileDialog.FileName + "1");
            }
        }

        #region HDR
        DATAudio datAudio = new DATAudio();
        HDRHandler hdrHandler = new HDRHandler();
        public List<string> Files = new List<string>();
        bool HDRLoaded = false;
        private void hdrLoadFolder_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog openFileDialog1 = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "Select Wav Folder",
            };
            if (openFileDialog1.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string[] NewFiles = Directory.GetFiles(openFileDialog1.FileName, "*.wav", SearchOption.TopDirectoryOnly);

                Files = NewFiles.ToList();

                hdrFileList.Items.Clear();
                for (int i = 0; i < Files.Count; i++)
                {
                    hdrFileList.Items.Add(Path.GetFileName(Files[i]));
                }
            }
        }

        private void hdrRemoveFile_Click(object sender, EventArgs e)
        {
            if (hdrFileList.SelectedIndex != -1)
            {
                Files.RemoveAt(hdrFileList.SelectedIndex);

                hdrFileList.Items.Clear();
                for (int i = 0; i < Files.Count; i++)
                {
                    hdrFileList.Items.Add(Path.GetFileName(Files[i]));
                }
            }
        }

        private void hdrUp_Click(object sender, EventArgs e)
        {
            if (hdrFileList.SelectedIndex != -1 && hdrFileList.SelectedIndex != 0)
            {
                int Pos = hdrFileList.SelectedIndex;
                var Tempstring = Files[Pos];
                Files.RemoveAt(Pos);
                Files.Insert(Pos - 1, Tempstring);
                hdrFileList.Items.Clear();

                for (int i = 0; i < Files.Count; i++)
                {
                    hdrFileList.Items.Add(Path.GetFileName(Files[i]));
                }

                hdrFileList.SelectedIndex = Pos - 1;
            }
        }

        private void hdrDown_Click(object sender, EventArgs e)
        {
            if (hdrFileList.SelectedIndex != -1 && hdrFileList.SelectedIndex != Files.Count - 1)
            {
                int Pos = hdrFileList.SelectedIndex;
                var Tempstring = Files[Pos];
                Files.RemoveAt(Pos);
                Files.Insert(Pos + 1, Tempstring);
                hdrFileList.Items.Clear();

                for (int i = 0; i < Files.Count; i++)
                {
                    hdrFileList.Items.Add(Path.GetFileName(Files[i]));
                }

                hdrFileList.SelectedIndex = Pos + 1;
            }
        }

        private void hdrAddFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Wav Audio File (*.wav)|*.wav|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Files.Add(openFileDialog.FileName);

                hdrFileList.Items.Clear();
                for (int i = 0; i < Files.Count; i++)
                {
                    hdrFileList.Items.Add(Path.GetFileName(Files[i]));
                }
            }
        }

        private void hdrFileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hdrFileList.SelectedIndex != -1)
            {
                using (var reader = new WaveFileReader(Files[hdrFileList.SelectedIndex]))
                {
                    HDRTime.Text = reader.TotalTime.ToString(@"hh\:mm\:ss\.ff");
                    HDRSample.Text = reader.WaveFormat.SampleRate.ToString();
                    HDRFileSize.Text = reader.Length.ToString();
                    HDRTotalSamples.Text = (reader.TotalTime.TotalSeconds * reader.WaveFormat.SampleRate).ToString();
                }
            }
        }

        WaveFileReader waveFile = null;
        WaveOut waveOut = null;
        private void hdrPlay_Click(object sender, EventArgs e)
        {
            if (hdrFileList.SelectedIndex != -1)
            {
                if (waveOut != null)
                {
                    waveOut.Stop();
                    waveFile.Close();
                }
                waveFile = new WaveFileReader(Files[hdrFileList.SelectedIndex]);
                waveOut = new WaveOut();
                waveOut.Init(waveFile);
                waveOut.Play();
            }
        }
        private bool CheckSX()
        {
            string StringPath = Application.StartupPath;
            if (Directory.GetFiles(StringPath, "sx_2002.exe", SearchOption.TopDirectoryOnly).Length == 1)
            {
                return true;
            }
            MessageBox.Show("Missing sx_2002.exe");
            return false;
        }
        private void hdrExtract_Click(object sender, EventArgs e)
        {
            if (CheckSX())
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "PS2 Audio File (*.dat)|*.dat|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    CommonOpenFileDialog openFileDialog1 = new CommonOpenFileDialog
                    {
                        IsFolderPicker = true,
                        Title = "Select Extract Folder",
                    };
                    if (openFileDialog1.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        datAudio = new DATAudio();

                        datAudio.ExtractGuess(openFileDialog.FileName, openFileDialog1.FileName);

                        MessageBox.Show("Audio Extracted");

                        string[] NewFiles = Directory.GetFiles(openFileDialog1.FileName, "*.wav", SearchOption.TopDirectoryOnly);

                        Files = NewFiles.ToList();

                        hdrFileList.Items.Clear();
                        for (int i = 0; i < Files.Count; i++)
                        {
                            hdrFileList.Items.Add(Path.GetFileName(Files[i]));
                        }
                    }
                }

            }
        }

        private void hdrBuildDAT_Click(object sender, EventArgs e)
        {
            if (CheckSX() && HDRLoaded)
            {
                SaveFileDialog openFileDialog1 = new SaveFileDialog
                {
                    Filter = "PS2 Audio File (*.dat)|*.dat|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (hdrHandler.fileHeaders.Count != Files.ToArray().Length)
                    {
                        MessageBox.Show("Incorrect Wav Count " + hdrHandler.fileHeaders.Count + "/" + Files.ToArray().Length);
                        return;
                    }
                    hdrHandler = datAudio.GenerateDATAndHDR3(Files.ToArray(), openFileDialog1.FileName, hdrHandler);
                    MessageBox.Show("File Generated");

                    hdrList2.Items.Clear();
                    for (int i = 0; i < hdrHandler.fileHeaders.Count; i++)
                    {
                        hdrList2.Items.Add("Entry " + i + " - Offset " + hdrHandler.fileHeaders[i].OffsetInt);
                    }
                }
            }
            else
            {
                if (!HDRLoaded)
                {
                    MessageBox.Show("Rebuilding Requires HDR File Loaded");
                }
            }
        }

        private void hdrList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hdrList2.SelectedIndex != -1 && !DisableUpdate)
            {
                DisableUpdate = true;
                hdrEntryU1.Value = hdrHandler.fileHeaders[hdrList2.SelectedIndex].Unknown;
                hdrEntryU2.Value = hdrHandler.fileHeaders[hdrList2.SelectedIndex].Unknown2;
                hdrEntryU3.Value = hdrHandler.fileHeaders[hdrList2.SelectedIndex].EventID;
                DisableUpdate = false;
            }
        }

        private void HdrLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PS2 Audio Header File (*.hdr)|*.hdr|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                hdrHandler.Load(openFileDialog.FileName);
                hdrBuildDAT.Enabled = true;
                HDRLoaded = true;

                hdrList2.Items.Clear();

                for (int i = 0; i < hdrHandler.fileHeaders.Count; i++)
                {
                    hdrList2.Items.Add("Entry " + i + " - Offset " + hdrHandler.fileHeaders[i].OffsetInt);
                }

                DisableUpdate = true;
                hdrU1.Value = hdrHandler.U1;
                hdrU2.Value = hdrHandler.U2;
                hdrAligmentMode.Value = hdrHandler.AligmentSize;
                hdrEntryType.Value = hdrHandler.EntryTypes;
                hdrUpdateActiveButtons();
                DisableUpdate = false;
            }
        }

        public void hdrUpdateActiveButtons()
        {
            hdrEntryU1.Enabled = false;
            hdrEntryU2.Enabled = false;
            hdrEntryU3.Enabled = false;

            if (hdrHandler.EntryTypes == 1 || hdrHandler.EntryTypes == 2)
            {
                hdrEntryU1.Enabled = true;
            }
            if (hdrHandler.EntryTypes == 3)
            {
                hdrEntryU2.Enabled = true;
                hdrEntryU3.Enabled = true;
            }
            if (hdrHandler.EntryTypes == 4)
            {
                hdrEntryU1.Enabled = true;
                hdrEntryU2.Enabled = true;
                hdrEntryU3.Enabled = true;
            }
        }

        private void hdrSave_Click(object sender, EventArgs e)
        {
            if (HDRLoaded)
            {
                SaveFileDialog openFileDialog1 = new SaveFileDialog
                {
                    Filter = "PS2 Audio File (*.hdr)|*.hdr|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    hdrHandler.Save(openFileDialog1.FileName);
                }
            }
        }

        private void hdrEntryU1_ValueChanged(object sender, EventArgs e)
        {
            if (hdrList2.SelectedIndex != -1 && !DisableUpdate)
            {
                DisableUpdate = true;
                var Temp = hdrHandler.fileHeaders[hdrList2.SelectedIndex];
                Temp.Unknown = (int)hdrEntryU1.Value;
                Temp.Unknown2 = (int)hdrEntryU2.Value;
                Temp.EventID = (int)hdrEntryU3.Value;
                hdrHandler.fileHeaders[hdrList2.SelectedIndex] = Temp;
                DisableUpdate = false;
            }
        }

        private void hdrU1_ValueChanged(object sender, EventArgs e)
        {
            if (hdrList2.SelectedIndex != -1 && !DisableUpdate)
            {
                DisableUpdate = true;
                hdrHandler.U1 = (int)hdrU1.Value;
                hdrHandler.U2 = (int)hdrU2.Value;
                hdrHandler.AligmentSize = (int)hdrAligmentMode.Value;
                hdrHandler.EntryTypes = (int)hdrEntryType.Value;
                hdrUpdateActiveButtons();
                DisableUpdate = false;
            }
        }

        private void SSX3ToolsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveFile.Close();
            }
        }


        #endregion

        private void MorphScaleNumber_ValueChanged(object sender, EventArgs e)
        {
            SSX3PS2MPF.MorphScale = (float)MorphScaleNumber.Value;
        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void MpfModelName_TextChanged(object sender, EventArgs e)
        {
            if (MpfModelList.SelectedIndex != -1)
            {
                var TempModel = ssx3ModelCombiner.modelHandlers.ModelList[MpfModelList.SelectedIndex];

                TempModel.ModelName = MpfModelName.Text;

                ssx3ModelCombiner.modelHandlers.ModelList[MpfModelList.SelectedIndex] = TempModel;

                MpfModelList.Items[MpfModelList.SelectedIndex] = TempModel.ModelName;
            }
        }

        private void label59_Click(object sender, EventArgs e)
        {

        }
    }
}
