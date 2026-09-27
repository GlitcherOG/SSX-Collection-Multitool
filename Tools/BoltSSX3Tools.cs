using Microsoft.WindowsAPICodePack.Dialogs;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SSX_Library.EATextureLibrary;
using SSXLibrary.FileHandlers;
using SSXLibrary.FileHandlers.Models.SSX3;
using System.CodeDom;
using System.IO;

namespace SSXMultiTool.Tools
{
    public partial class BoltSSX3Tools : Form
    {
        public BoltSSX3Tools()
        {
            InitializeComponent();
            BoltCharacter.SelectedIndex = 0;
        }
        BoltPS2Handler BoltPS2Handler = new BoltPS2Handler();
        public string DataPath;
        bool loaded = false;
        bool Wait = false;

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "BOLTPS2 File (*.dat)|*.dat|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Wait = true;
                BoltPS2Handler = new BoltPS2Handler();
                BoltPS2Handler.load(openFileDialog.FileName);
                loaded = true;
                BoltCharacter.SelectedIndex = 0;
                //BoltCharacter2.SelectedIndex = 0;
                GenerateTreeview();
                GenerateListBoxes();
                Wait = false;
            }
        }

        private void BoltSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog
            {
                Filter = "BOLTPS2 File (*.dat)|*.dat|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = false
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                BoltPS2Handler.Save(openFileDialog.FileName);
            }
        }

        List<bool> Parented = new List<bool>();
        int pos = 0;
        void GenerateTreeview()
        {
            BoltPS2TreeView.Nodes.Clear();

            var temp = BoltPS2Handler.characters[BoltCharacter.SelectedIndex];
            Parented = new List<bool>();

            for (int i = 0; i < temp.entries.Count; i++)
            {
                Parented.Add(false);
            }
            bool test = false;
            int Testing = 0;
            while (!test)
            {
                Testing++;
                for (int i = 0; i < temp.entries.Count; i++)
                {
                    if (!Parented[i])
                    {
                        pos = i;
                        //Check Parent Node
                        if (temp.entries[i].ParentID == -1)
                        {
                            Parented[i] = true;
                            BoltPS2TreeView.Nodes.Add(i.ToString(), temp.entries[i].ItemID.ToString() + " - " + temp.entries[i].itemName);
                        }
                        else
                        {
                            for (int a = 0; a < BoltPS2TreeView.Nodes.Count; a++)
                            {
                                if (temp.entries[int.Parse(BoltPS2TreeView.Nodes[a].Name)].ItemID == temp.entries[i].ParentID)
                                {
                                    Parented[i] = true;
                                    BoltPS2TreeView.Nodes[a].Nodes.Add(i.ToString(), temp.entries[i].ItemID.ToString() + " - " + temp.entries[i].itemName);
                                    break;
                                }
                                else
                                {
                                    var temp1 = BoltPS2TreeView.Nodes[a];
                                    CheckChildNode(temp1, temp.entries[i], i);
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

                if(Testing==100)
                {
                    throw new Exception("Bad Bolt File, Unable to generate Tree");
                }
            }
        }

        bool CheckChildNode(TreeNode Parent, ItemEntries item, int id)
        {
            var temp = BoltPS2Handler.characters[BoltCharacter.SelectedIndex];

            for (int i = 0; i < Parent.Nodes.Count; i++)
            {
                if (temp.entries[int.Parse(Parent.Nodes[i].Name)].ItemID == item.ParentID)
                {
                    Parented[pos] = true;
                    Parent.Nodes[i].Nodes.Add(id.ToString(), item.ItemID.ToString() + " - " + item.itemName);
                    return true;
                }
                else
                {
                    bool test = CheckChildNode(Parent.Nodes[i], item, id);
                    if (test)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void BoltCharacter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BoltCharacter.SelectedIndex != -1 && loaded)
            {
                GenerateTreeview();
                GenerateListBoxes();
            }
        }

        private void GenerateListBoxes()
        {
            DefaultOutfitList.Items.Clear();

            for (int i = 0; i < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].defaultOutfits.Count; i++)
            {
                for (int j = 0; j < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Count; j++)
                {
                    if (BoltPS2Handler.characters[BoltCharacter.SelectedIndex].defaultOutfits[i].ItemID == BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[j].ItemID)
                    {
                        DefaultOutfitList.Items.Add(BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[j].ItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[j].itemName);
                        break;
                    }
                }

            }

            DefaultOutfitItem.Items.Clear();

            for (int i = 0; i < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Count; i++)
            {
                DefaultOutfitItem.Items.Add(BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].ItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].itemName);
            }

            EquipLinkIf.Items.Clear();
            EquipLinkIf.Items.Add("None");
            for (int i = 0; i < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Count; i++)
            {
                EquipLinkIf.Items.Add(BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].ItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].itemName);
            }

            EquipLinkSet.Items.Clear();

            for (int i = 0; i < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Count; i++)
            {
                EquipLinkSet.Items.Add(BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].ItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].itemName);
            }

            HandComboList.Items.Clear();
            HandComboList.Items.Add("None");
            for (int i = 0; i < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Count; i++)
            {
                HandComboList.Items.Add(BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].ItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].itemName);
            }
        }

        public List<int> EquipList = new List<int>();
        private void GenerateEquipLink()
        {
            EquipLinkIf.SelectedIndex = 0;
            EquipLinkSet.SelectedIndex = -1;
            EquipLinkIfBool.Checked = false;
            EquipLinkSetBool.Checked = false;
            EquipLinksEquip.Checked = false;

            EquipLinkList.Items.Clear();
            EquipList.Clear();

            int Index = int.Parse(BoltPS2TreeView.SelectedNode.Name);

            for (int i = 0; i < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks.Count; i++)
            {
                //ID
                if (BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks[i].MainItemID == BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[Index].ItemID)
                {
                    for (int j = 0; j < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Count; j++)
                    {
                        if (BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks[i].SecondaryItemID == BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[j].ItemID)
                        {
                            EquipLinkList.Items.Add(BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks[i].SecondaryItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[j].itemName);
                            EquipList.Add(i);
                            break;
                        }
                    }
                }
            }
        }

        private void EquipLinkList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Wait)
            {
                Wait = true;
                if (EquipLinkList.SelectedIndex != -1)
                {
                    int ItemIndex = int.Parse(BoltPS2TreeView.SelectedNode.Name);
                    int EquipIndex = EquipList[EquipLinkList.SelectedIndex];

                    EquipLinksEquip.Checked = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks[EquipIndex].MainItemEquip == 1;

                    EquipLinkIf.SelectedIndex = BoltPS2Handler.GetItemIndex(BoltCharacter.SelectedIndex, BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks[EquipIndex].IfEquipID) + 1;
                    EquipLinkIfBool.Checked = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks[EquipIndex].IfEquipBool == 1;

                    EquipLinkSet.SelectedIndex = BoltPS2Handler.GetItemIndex(BoltCharacter.SelectedIndex, BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks[EquipIndex].SecondaryItemID);
                    EquipLinkSetBool.Checked = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks[EquipIndex].SecondaryItemEquip == 1;
                }
                else
                {
                    EquipLinkIf.SelectedIndex = 0;
                    EquipLinkSet.SelectedIndex = -1;
                    EquipLinkIfBool.Checked = false;
                    EquipLinkSetBool.Checked = false;
                    EquipLinksEquip.Checked = false;
                }
                Wait = false;
            }
        }

        private void EquipLinksUpdated(object sender, EventArgs e)
        {
            if (EquipLinkList.SelectedIndex != -1 && !Wait)
            {
                Wait = true;
                int ItemIndex = int.Parse(BoltPS2TreeView.SelectedNode.Name);
                int EquipIndex = EquipList[EquipLinkList.SelectedIndex];

                var EquipLink = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks[EquipIndex];

                EquipLink.MainItemEquip = EquipLinksEquip.Checked ? 1 : 0;

                if (EquipLinkIf.SelectedIndex > 0)
                {
                    EquipLink.IfEquipID = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[EquipLinkIf.SelectedIndex - 1].ItemID;
                }
                else
                {
                    EquipLink.IfEquipID = -1;
                }
                EquipLink.IfEquipBool = EquipLinkIfBool.Checked ? 1 : 0;

                if (EquipLinkSet.SelectedIndex != -1)
                {
                    EquipLink.SecondaryItemID = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[EquipLinkSet.SelectedIndex].ItemID;

                    EquipLinkList.Items[EquipLinkList.SelectedIndex] = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[EquipLinkSet.SelectedIndex].ItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[EquipLinkSet.SelectedIndex].itemName;
                }
                EquipLink.SecondaryItemEquip = EquipLinkSetBool.Checked ? 1 : 0;

                BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks[EquipIndex] = EquipLink;

                Wait = false;
            }
        }

        private void DefaultOutfitList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loaded == true && DefaultOutfitList.SelectedIndex != -1)
            {
                DefaultOutfitItem.SelectedIndex = DefaultOutfitItem.Items.IndexOf(DefaultOutfitList.Text);
            }
            else if (loaded == true && DefaultOutfitList.SelectedIndex == -1)
            {
                DefaultOutfitItem.SelectedIndex = -1;
            }
        }

        private void DefaultOutfitItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DefaultOutfitList.SelectedIndex != -1 && DefaultOutfitItem.SelectedIndex != -1)
            {
                int ItemID = int.Parse(DefaultOutfitItem.GetItemText(DefaultOutfitItem.Items[DefaultOutfitItem.SelectedIndex]).Split(" ")[0]);

                var Item = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].defaultOutfits[DefaultOutfitList.SelectedIndex];

                Item.ItemID = ItemID;

                for (int i = 0; i < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Count; i++)
                {
                    if (BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].ItemID == ItemID)
                    {
                        Item.CategoryID = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].CharacterID;
                        break;
                    }
                }

                BoltPS2Handler.characters[BoltCharacter.SelectedIndex].defaultOutfits[DefaultOutfitList.SelectedIndex] = Item;

                DefaultOutfitList.Items[DefaultOutfitList.SelectedIndex] = DefaultOutfitItem.Text;
            }
        }

        private void BoltPS2TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (BoltPS2TreeView.SelectedNode != null && loaded && !Wait)
            {
                Wait = true;
                int Index1 = BoltCharacter.SelectedIndex;
                int Index = int.Parse(BoltPS2TreeView.SelectedNode.Name);

                BoltUnkownOne.Value = BoltPS2Handler.characters[Index1].entries[Index].unkownInt1;
                BoltUnlock.Value = BoltPS2Handler.characters[Index1].entries[Index].UnlockCondition;
                BoltUnkownTwo.Value = BoltPS2Handler.characters[Index1].entries[Index].TextureType;
                BoltUnkownThree.Value = BoltPS2Handler.characters[Index1].entries[Index].ItemID;
                BoltUnkownFour.Value = BoltPS2Handler.characters[Index1].entries[Index].ParentID;
                try 
                {
                    BoltCat.Value = BoltPS2Handler.characters[Index1].entries[Index].category;
                }
                catch
                {
                    BoltCat.Value = -1;
                }
                BoltBuy.Checked = BoltPS2Handler.characters[Index1].entries[Index].buyable;
                BoltMenuOrder.Value = BoltPS2Handler.characters[Index1].entries[Index].menuOrder;
                BoltUnkown7.Value = BoltPS2Handler.characters[Index1].entries[Index].unkownInt5;
                BoltFillBar.Value = BoltPS2Handler.characters[Index1].entries[Index].weight;
                BoltCost.Value = BoltPS2Handler.characters[Index1].entries[Index].Cost;
                BoltFileID.Value = BoltPS2Handler.characters[Index1].entries[Index].FileID;

                BoltSpecialOne.Value = BoltPS2Handler.characters[Index1].entries[Index].SpecialID;
                BoltSpecialTwo.Value = BoltPS2Handler.characters[Index1].entries[Index].SpecialID2;
                BoltSpecialThree.Value = BoltPS2Handler.characters[Index1].entries[Index].SpecialID3;

                BoltName.Text = BoltPS2Handler.characters[Index1].entries[Index].itemName;
                BoltModelID.Text = BoltPS2Handler.characters[Index1].entries[Index].ModelID;
                BoltModelIDTwo.Text = BoltPS2Handler.characters[Index1].entries[Index].ModelID2;
                BoltModelIDThree.Text = BoltPS2Handler.characters[Index1].entries[Index].ModelID3;
                BoltModelIDFour.Text = BoltPS2Handler.characters[Index1].entries[Index].ModelID4;
                BoltModelPath.Text = BoltPS2Handler.characters[Index1].entries[Index].ModelPath;
                BoltTexturePath.Text = BoltPS2Handler.characters[Index1].entries[Index].TexturePath;
                BoltIconPath.Text = BoltPS2Handler.characters[Index1].entries[Index].SmallIcon;

                BoltUnkown9.Value = BoltPS2Handler.characters[Index1].entries[Index].unkownInt6;

                GenerateEquipLink();
                //LoadImages();

                HandComboList.SelectedIndex = 0;

                //Hand Data
                for (int i = 0; i < BoltPS2Handler.characters[Index1].handMatch.Count; i++)
                {
                    if (BoltPS2Handler.characters[Index1].handMatch[i].LeftHand == BoltPS2Handler.characters[Index1].entries[Index].ItemID)
                    {
                        HandComboList.SelectedIndex = BoltPS2Handler.GetItemIndex(Index1, BoltPS2Handler.characters[Index1].handMatch[i].RightHand) + 1;
                    }
                }

                Wait = false;
            }
        }

        private void AutoDataGetButton_Click(object sender, EventArgs e)
        {
            if (BoltPS2TreeView.SelectedNode != null && loaded)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "MPF File (*.mpf)|*.mpf|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = false
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SSX3PS2MPF trickyPS2MPF = new SSX3PS2MPF();

                    trickyPS2MPF.load(openFileDialog.FileName);

                    if (trickyPS2MPF.ModelList.Count > 1)
                    {
                        BoltModelID.Text = trickyPS2MPF.ModelList[0].ModelName;
                        BoltModelIDTwo.Text = trickyPS2MPF.ModelList[1].ModelName;
                        BoltModelIDThree.Text = trickyPS2MPF.ModelList[2].ModelName;
                        BoltModelIDFour.Text = trickyPS2MPF.ModelList[3].ModelName;
                        BoltFileID.Value = trickyPS2MPF.ModelList[0].FileID;
                        BoltFillBar.Value = trickyPS2MPF.ModelList[0].TriangleCount;
                    }
                    else
                    {
                        BoltModelID.Text = trickyPS2MPF.ModelList[0].ModelName;
                        BoltFileID.Value = trickyPS2MPF.ModelList[0].FileID;
                        BoltFillBar.Value = trickyPS2MPF.ModelList[0].TriangleCount;
                        BoltModelIDTwo.Text = "";
                        BoltModelIDThree.Text = "";
                        BoltModelIDFour.Text = "";
                    }
                }
            }
        }

        public void UpdateListBoxOneEntry(int CharID, int IndexID)
        {
            DefaultOutfitItem.Items[IndexID] = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[IndexID].ItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[IndexID].itemName;
            EquipLinkIf.Items[IndexID+1] = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[IndexID].ItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[IndexID].itemName;
            EquipLinkSet.Items[IndexID] = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[IndexID].ItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[IndexID].itemName;
            HandComboList.Items[IndexID+1] = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[IndexID].ItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[IndexID].itemName;

            //DefaultOutfitItem.Items.Clear();

            //for (int i = 0; i < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Count; i++)
            //{
            //    DefaultOutfitItem.Items.Add(BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].ItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].itemName);
            //}

            //EquipLinkIf.Items.Clear();
            //EquipLinkIf.Items.Add("None");
            //for (int i = 0; i < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Count; i++)
            //{
            //    EquipLinkIf.Items.Add(BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].ItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].itemName);
            //}

            //EquipLinkSet.Items.Clear();

            //for (int i = 0; i < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Count; i++)
            //{
            //    EquipLinkSet.Items.Add(BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].ItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].itemName);
            //}

            //HandComboList.Items.Clear();
            //HandComboList.Items.Add("None");
            //for (int i = 0; i < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Count; i++)
            //{
            //    HandComboList.Items.Add(BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].ItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].itemName);
            //}
        }

        public void UpdateTreeBasic(object sender, EventArgs e)
        {
            int Index1 = BoltCharacter.SelectedIndex;
            int Index = int.Parse(BoltPS2TreeView.SelectedNode.Name);

            GeneralApply();

            var Char = BoltPS2Handler.characters[Index1];
            var tempEntry = Char.entries[Index];

            BoltPS2TreeView.SelectedNode.Text = tempEntry.ItemID + " - " + tempEntry.itemName;
            UpdateListBoxOneEntry(Index1, Index);
        }

        public void UpdateTreeFull(object sender, EventArgs e)
        {
            if (!Wait)
            {
                Wait = true;
                //Get Current ID
                int Index1 = BoltCharacter.SelectedIndex;
                int Index = int.Parse(BoltPS2TreeView.SelectedNode.Name);

                var Char = BoltPS2Handler.characters[Index1];
                var tempEntry = Char.entries[Index];
                var OldEntryID = Char.entries[Index].ItemID;
                var NewEntryID = (int)BoltUnkownThree.Value;

                var OldParentID = Char.entries[Index].ParentID;
                var NewParentID = (int)BoltUnkownFour.Value;

                ItemIDLabel.Text = "Item ID";
                ParentItemIDLabel.Text = "Parent Item ID";

                if (OldEntryID != NewEntryID)
                {
                    //Check if New ID Conflicts
                    //If So Dont Update and Update Label to say invalid
                    bool ValidID = true;
                    for (int i = 0; i < Char.entries.Count; i++)
                    {
                        if (NewEntryID == Char.entries[i].ItemID)
                        {
                            ValidID = false;
                            break;
                        }
                    }

                    if (!ValidID)
                    {
                        ItemIDLabel.Text = "Item ID - Invalid";
                        Wait = false;
                        return;
                    }
                    //Update Data
                    tempEntry.ItemID = (int)BoltUnkownThree.Value;
                    UpdateListBoxOneEntry(Index1, Index);
                }
                else if (OldParentID != NewParentID || (OldParentID != -1 && NewParentID == -1))
                {
                    if (NewParentID != -1)
                    {
                        //Check if New ID Conflicts
                        //If So Dont Update and Update Label to say invalid
                        bool ValidID = false;
                        for (int i = 0; i < Char.entries.Count; i++)
                        {
                            if (NewParentID == Char.entries[i].ItemID)
                            {
                                ValidID = BoltPS2Handler.ItemParentIDValid(Index1, OldEntryID, NewParentID);
                                break;
                            }
                        }

                        if (!ValidID)
                        {
                            ParentItemIDLabel.Text = "Parent Item ID - Invalid";
                            Wait = false;
                            return;
                        }
                    }

                    tempEntry.ParentID = (int)BoltUnkownFour.Value;
                }
                else
                {
                    Wait = false;
                    return;
                }

                Char.entries[Index] = tempEntry;

                //Update Text
                BoltPS2TreeView.SelectedNode.Text = tempEntry.ItemID + " - " + Char.entries[Index].itemName;

                if (OldParentID != NewParentID)
                {
                    bool StartOfItem = false;
                    bool Found = false;

                    var TempEntry = Char.entries[Index];
                    Char.entries.RemoveAt(Index);

                    for (int i = 0; i < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Count; i++)
                    {
                        if (StartOfItem)
                        {
                            if (BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].ParentID != NewParentID)
                            {
                                Found = true;
                                BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Insert(i, TempEntry);
                                break;
                            }
                        }
                        else
                        {
                            if (BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].ParentID == NewParentID)
                            {
                                StartOfItem = true;
                            }
                        }
                    }

                    if (!Found)
                    {
                        BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Add(TempEntry);
                    }
                }

                //Update All Item Details
                for (int i = 0; i < Char.entries.Count; i++)
                {
                    var TempEntry = Char.entries[i];

                    if(tempEntry.category==OldEntryID)
                    {
                        TempEntry.category = NewEntryID;
                    }

                    if (tempEntry.ParentID == OldEntryID)
                    {
                        TempEntry.ParentID = NewEntryID;
                    }

                    Char.entries[i] = TempEntry;
                }

                //Update All Default Items
                for (int i = 0; i < Char.defaultOutfits.Count; i++)
                {
                    var Equip = Char.defaultOutfits[i];
                    if (Equip.ItemID == OldEntryID)
                    {
                        Equip.ItemID = NewEntryID;
                    }
                    Char.defaultOutfits[i] = Equip;
                }

                //Update All EquipLinks
                for (int i = 0; i < Char.equipLinks.Count; i++)
                {
                    var Equip = Char.equipLinks[i];
                    if (Equip.MainItemID == OldEntryID)
                    {
                        Equip.MainItemID = NewEntryID;
                    }
                    if (Equip.SecondaryItemEquip == OldEntryID)
                    {
                        Equip.SecondaryItemEquip = NewEntryID;
                    }
                    if (Equip.IfEquipID == OldEntryID)
                    {
                        Equip.IfEquipID = NewEntryID;
                    }
                    Char.equipLinks[i] = Equip;
                }

                BoltPS2Handler.characters[Index1] = Char;

                if (OldParentID != NewParentID)
                {
                    string Key = BoltPS2TreeView.SelectedNode.Name;
                    GenerateTreeview();
                    var List = BoltPS2TreeView.Nodes.Find(Key, true);
                    BoltPS2TreeView.SelectedNode = List[0];
                }
                GenerateListBoxes();
                Wait = false;
            }
        }

        public void GeneralApplyButton(object sender, EventArgs e)
        {
            GeneralApply();
        }

        public void GeneralApply()
        {
            if (!Wait)
            {
                Wait = true;
                int Index1 = BoltCharacter.SelectedIndex;
                int Index = int.Parse(BoltPS2TreeView.SelectedNode.Name);

                var Char = BoltPS2Handler.characters[Index1];
                var tempEntry = Char.entries[Index];

                tempEntry.unkownInt1 = (int)BoltUnkownOne.Value;
                tempEntry.UnlockCondition = (int)BoltUnlock.Value;
                tempEntry.TextureType = (int)BoltUnkownTwo.Value;
                tempEntry.category = (int)BoltCat.Value;

                for (int i = 0; i < Char.defaultOutfits.Count; i++)
                {
                    var Equip = Char.defaultOutfits[i];
                    if (Equip.ItemID == tempEntry.ItemID)
                    {
                        Equip.CategoryID = tempEntry.category;
                    }
                    Char.defaultOutfits[i] = Equip;
                }

                tempEntry.buyable = BoltBuy.Checked;
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
                if (tempEntry.SmallIcon != BoltIconPath.Text || tempEntry.TexturePath != BoltTexturePath.Text)
                {
                    tempEntry.TexturePath = BoltTexturePath.Text;
                    tempEntry.SmallIcon = BoltIconPath.Text;
                    //LoadImages();
                }

                tempEntry.unkownInt6 = (int)BoltUnkown9.Value;

                Char.entries[Index] = tempEntry;
                BoltPS2Handler.characters[Index1] = Char;
                Wait = false;
            }
        }

        private void HandComboList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Wait && HandComboList.SelectedIndex != -1)
            {
                int Index1 = BoltCharacter.SelectedIndex;
                int Index = int.Parse(BoltPS2TreeView.SelectedNode.Name);

                bool Found = false;

                for (int i = 0; i < BoltPS2Handler.characters[Index1].handMatch.Count; i++)
                {
                    if (BoltPS2Handler.characters[Index1].handMatch[i].LeftHand == BoltPS2Handler.characters[Index1].entries[Index].ItemID)
                    {
                        Found = true;
                        if (HandComboList.SelectedIndex == 0)
                        {
                            BoltPS2Handler.characters[Index1].handMatch.RemoveAt(i);
                            return;
                        }

                        var Item = BoltPS2Handler.characters[Index1].handMatch[i];

                        Item.RightHand = BoltPS2Handler.characters[Index1].entries[HandComboList.SelectedIndex - 1].ItemID;

                        BoltPS2Handler.characters[BoltCharacter.SelectedIndex].handMatch[DefaultOutfitList.SelectedIndex] = Item;

                    }
                }

                if (!Found)
                {
                    var Char = BoltPS2Handler.characters[Index1];

                    var Hand = new HandMatch();

                    Hand.CharID = Index1;
                    Hand.LeftHand = Char.entries[Index].ItemID;
                    Hand.RightHand = Char.entries[HandComboList.SelectedIndex - 1].ItemID;

                    Char.handMatch.Add(Hand);

                    BoltPS2Handler.characters[Index1] = Char;
                }
            }
        }

        private void DefaultOutfitRemove_Click(object sender, EventArgs e)
        {
            if (DefaultOutfitList.SelectedIndex != -1)
            {
                int Index = DefaultOutfitList.SelectedIndex;
                DefaultOutfitList.Items.RemoveAt(Index);
                BoltPS2Handler.characters[BoltCharacter.SelectedIndex].defaultOutfits.RemoveAt(Index);
            }
        }

        private void DefaultOutfitAdd_Click(object sender, EventArgs e)
        {
            var NewOutfit = new DefaultOutfit();

            NewOutfit.CharID = BoltCharacter.SelectedIndex;
            NewOutfit.ItemID = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[0].ItemID;
            NewOutfit.CategoryID = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[0].category;

            BoltPS2Handler.characters[BoltCharacter.SelectedIndex].defaultOutfits.Add(NewOutfit);

            DefaultOutfitList.Items.Add(BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[0].ItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[0].itemName);
        }

        private void CharDatatoolStripButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog openFileDialog1 = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "Select Extract Folder",
            };
            if (openFileDialog1.ShowDialog() == CommonFileDialogResult.Ok)
            {
                DataPath = openFileDialog1.FileName;
            }
            else
            {
                DataPath = "";
            }
        }

        //private void LoadImages()
        //{
        //    if(DataPath!="")
        //    {
        //        int Index1 = BoltCharacter.SelectedIndex;
        //        int Index = int.Parse(BoltPS2TreeView.SelectedNode.Name);

        //        if (BoltPS2Handler.characters[Index1].entries[Index].SmallIcon != null && BoltPS2Handler.characters[Index1].entries[Index].SmallIcon != "")
        //        {
        //            if (File.Exists(Path.Combine(DataPath, BoltPS2Handler.characters[Index1].entries[Index].SmallIcon)))
        //            {
        //                try
        //                {
        //                    OldShapeHandler oldShapeHandler = new OldShapeHandler();

        //                    oldShapeHandler.LoadShape(Path.Combine(DataPath, BoltPS2Handler.characters[Index1].entries[Index].SmallIcon));

        //                    IconImage.Image = ToDrawingImage(oldShapeHandler.ShapeImages[0].Image);
        //                }
        //                catch
        //                {
        //                    IconImage.Image = null;
        //                }

        //            }
        //            else
        //            {
        //                IconImage.Image = null;
        //            }
        //        }
        //    }
        //}
        public static System.Drawing.Image ToDrawingImage(SixLabors.ImageSharp.Image imageSharpImage)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Save ImageSharp image into the stream as a PNG
                imageSharpImage.Save(memoryStream, new PngEncoder());
                memoryStream.Position = 0;

                // Load it back as a System.Drawing.Image
                return System.Drawing.Image.FromStream(memoryStream);
            }
        }

        private void EquipLinkAdd_Click(object sender, EventArgs e)
        {
            var NewEquip = new EquipLink();
            int Index = int.Parse(BoltPS2TreeView.SelectedNode.Name);
            NewEquip.CharacterID = BoltCharacter.SelectedIndex;
            NewEquip.MainItemID = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[Index].ItemID;
            NewEquip.MainItemEquip = 0;

            NewEquip.UnkownInt2 = 0;
            NewEquip.IfEquipBool = 0;
            NewEquip.IfEquipID = -1;

            NewEquip.UnkownInt5 = 0;
            NewEquip.UnkownInt6 = 0;

            NewEquip.SecondaryItemID = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[0].ItemID;
            NewEquip.SecondaryItemEquip = 0;

            //Set it at the end of this items equip link list
            bool StartOfItem = false;
            bool Found = false;

            for (int i = 0; i < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks.Count; i++)
            {
                if(StartOfItem)
                {
                    if (BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks[i].MainItemID != NewEquip.MainItemID)
                    {
                        Found = true;
                        BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks.Insert(i,NewEquip);
                        break;
                    }
                }
                else
                {
                    if (BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks[i].MainItemID == NewEquip.MainItemID)
                    {
                        StartOfItem = true;
                    }
                }
            }

            if(!Found)
            {
                BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks.Add(NewEquip);
            }

            EquipList.Add(BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks.IndexOf(NewEquip));

            EquipLinkList.Items.Add(BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[0].ItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[0].itemName);
        }

        private void EquipLinkRemove_Click(object sender, EventArgs e)
        {
            if (EquipLinkList.SelectedIndex != -1)
            {
                int Index = EquipLinkList.SelectedIndex;
                int ID = EquipList[Index];
                BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks.RemoveAt(ID);
                GenerateEquipLink();
            }
        }

        private void TreeViewRemove_Click(object sender, EventArgs e)
        {
            if (BoltPS2TreeView.SelectedNode != null)
            {
                Wait = true;
                //Get IDs
                int Index1 = BoltCharacter.SelectedIndex;
                int Index = int.Parse(BoltPS2TreeView.SelectedNode.Name);
                int OldItemID = BoltPS2Handler.characters[Index1].entries[Index].ItemID;
                //Remove from Entries List
                BoltPS2Handler.characters[Index1].entries.RemoveAt(Index);

                //Go through all details and remove from category, parent, equip links, default outfit, nis link
                //Parent
                //Category
                for (int i = 0; i < BoltPS2Handler.characters[Index1].entries.Count; i++)
                {
                    var Entry = BoltPS2Handler.characters[Index1].entries[i];

                    if (Entry.ParentID== OldItemID)
                    {
                        Entry.ParentID = -1;
                    }
                    if (Entry.category == OldItemID)
                    {
                        Entry.category = -1;
                    }

                    BoltPS2Handler.characters[Index1].entries[i] = Entry;
                }

                //EquipLinks
                for (int i = 0; i < BoltPS2Handler.characters[Index1].equipLinks.Count; i++)
                {
                    if (BoltPS2Handler.characters[Index1].equipLinks[i].IfEquipID == OldItemID)
                    {
                        var EquipEntry = BoltPS2Handler.characters[Index1].equipLinks[i];

                        EquipEntry.IfEquipID = -1;

                        BoltPS2Handler.characters[Index1].equipLinks[i] = EquipEntry;
                    }

                    if(BoltPS2Handler.characters[Index1].equipLinks[i].SecondaryItemEquip == OldItemID || BoltPS2Handler.characters[Index1].equipLinks[i].MainItemID == OldItemID)
                    {
                        BoltPS2Handler.characters[Index1].equipLinks.RemoveAt(i);
                        i--;
                    }
                }

                //DefaultOutfit
                for (int i = 0; i < BoltPS2Handler.characters[Index1].defaultOutfits.Count; i++)
                {
                    if (BoltPS2Handler.characters[Index1].defaultOutfits[i].ItemID == OldItemID)
                    {
                        BoltPS2Handler.characters[Index1].defaultOutfits.RemoveAt(i);
                        i--;
                    }
                }

                //NisLink
                for (int i = 0; i < BoltPS2Handler.characters[Index1].handMatch.Count; i++)
                {
                    if (BoltPS2Handler.characters[Index1].handMatch[i].LeftHand == OldItemID || BoltPS2Handler.characters[Index1].handMatch[i].RightHand == OldItemID)
                    {
                        BoltPS2Handler.characters[Index1].handMatch.RemoveAt(i);
                        i--;
                    }
                }

                //Update TreeView
                //Update ListBoxes
                GenerateTreeview();
                GenerateListBoxes();
                Wait = false;
            }

        }

        private void TreeViewAdd_Click(object sender, EventArgs e)
        {
            ItemEntries itemEntries = new ItemEntries();

            itemEntries.CharacterID = BoltCharacter.SelectedIndex;
            itemEntries.unkownInt1 = -1;
            itemEntries.UnlockCondition = 0;
            itemEntries.TextureType = -1;

            //Do a check and take the next free id
            int Highest = -1;

            itemEntries.ItemID = -1;
            for (int i = 0; i < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Count; i++)
            {
                if(Highest <= BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].ItemID)
                {
                    Highest = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].ItemID + 1;
                }
            }
            itemEntries.ItemID = Highest;


            itemEntries.ParentID = -1;
            itemEntries.category = -1;
            itemEntries.buyable = false;
            itemEntries.menuOrder = 0;
            itemEntries.unkownInt5 = 117;
            itemEntries.weight = 0;
            itemEntries.Cost = 0;
            itemEntries.FileID = -1;

            itemEntries.SpecialID = -1;
            itemEntries.SpecialID2 = -1;
            itemEntries.SpecialID3 = -1;

            itemEntries.itemName = "New Item";
            itemEntries.ModelID = "";
            itemEntries.ModelID2 = "";
            itemEntries.ModelID3 = "";
            itemEntries.ModelID4 = "";
            itemEntries.ModelPath = "";
            itemEntries.TexturePath = "";
            itemEntries.SmallIcon = "";

            itemEntries.unkownInt6 = 0;

            BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Add(itemEntries);

            GenerateTreeview();
            GenerateListBoxes();
        }
    }
}
