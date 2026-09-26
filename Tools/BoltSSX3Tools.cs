using SSXLibrary.FileHandlers;
using SSXLibrary.FileHandlers.Models.SSX3;
using SSXLibrary.FileHandlers.Models.Tricky;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
                int ItemID = int.Parse(DefaultOutfitItem.GetItemText(DefaultOutfitItem.SelectedIndex).Split(" ")[0]);

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
                BoltCat.Value = BoltPS2Handler.characters[Index1].entries[Index].category;
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

        public void UpdateTreeBasic(object sender, EventArgs e)
        {
            int Index1 = BoltCharacter.SelectedIndex;
            int Index = int.Parse(BoltPS2TreeView.SelectedNode.Name);

            GeneralApply();

            var Char = BoltPS2Handler.characters[Index1];
            var tempEntry = Char.entries[Index];

            BoltPS2TreeView.SelectedNode.Text = tempEntry.ItemID + " - " + tempEntry.itemName;
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

                //if (tempEntry.ParentID != -1)
                //{
                //    var Node = BoltPS2TreeView.SelectedNode;

                //    Node.Remove();

                //    var ParentIDIndex = BoltPS2Handler.GetItemIndex(Index1, tempEntry.ParentID);

                //    var NodeParentList = BoltPS2TreeView.Nodes.Find(ParentIDIndex.ToString(), true);

                //    var NodeParent = NodeParentList[0];

                //    bool NodeIDFound=false;
                //    for (int i = 0; i < NodeParent.Nodes.Count; i++)
                //    {
                //        int NodeID = int.Parse(NodeParent.Nodes[i].Name);

                //        if(NodeID< Index)
                //        {
                //            NodeIDFound = true;
                //            NodeParent.Nodes.Insert(i,Node);
                //        }
                //    }

                //    if (!NodeIDFound)
                //    {
                //        NodeParent.Nodes.Add(Node);
                //    }

                //    BoltPS2TreeView.SelectedNode = Node;
                //}
                //else
                //{
                //    if (BoltPS2TreeView.SelectedNode.Parent != null)
                //    {
                //        var Node = BoltPS2TreeView.SelectedNode;

                //        Node.Remove();

                //        bool NodeIDFound = false;
                //        for (int i = 0; i < BoltPS2TreeView.Nodes.Count; i++)
                //        {
                //            int NodeID = int.Parse(BoltPS2TreeView.Nodes[i].Name);

                //            if (NodeID < Index)
                //            {
                //                NodeIDFound = true;
                //                BoltPS2TreeView.Nodes.Insert(i, Node);
                //            }
                //        }

                //        if (!NodeIDFound)
                //        {
                //            BoltPS2TreeView.Nodes.Add(Node);
                //        }

                //        BoltPS2TreeView.SelectedNode = Node;
                //    }
                //}


                //Update All Item Details

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
                tempEntry.TexturePath = BoltTexturePath.Text;
                tempEntry.SmallIcon = BoltIconPath.Text;

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
    }
}
