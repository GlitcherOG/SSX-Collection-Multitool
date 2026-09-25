using SSXLibrary.FileHandlers;
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
                BoltPS2Handler = new BoltPS2Handler();
                BoltPS2Handler.load(openFileDialog.FileName);
                loaded = true;
                BoltCharacter.SelectedIndex = 0;
                //BoltCharacter2.SelectedIndex = 0;
                GenerateTreeview();
                GenerateListBoxes();
            }
        }

        //private void BoltSave_Click(object sender, EventArgs e)
        //{
        //    SaveFileDialog openFileDialog = new SaveFileDialog
        //    {
        //        Filter = "BIG File (*.dat)|*.dat|All files (*.*)|*.*",
        //        FilterIndex = 1,
        //        RestoreDirectory = false
        //    };
        //    if (openFileDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        boltPS2.Save(openFileDialog.FileName);
        //    }
        //}

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
                                    /*if(!*/
                                    CheckChildNode(temp1, temp.entries[i]);
                                    /*)*/
                                    //{
                                    //    BoltPS2TreeView.Nodes.Add(temp.entries[i].ItemID.ToString(), temp.entries[i].ItemID.ToString() + " - " + temp.entries[i].itemName);
                                    //}
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

        bool CheckChildNode(TreeNode Parent, ItemEntries item)
        {
            for (int i = 0; i < Parent.Nodes.Count; i++)
            {
                if (Parent.Nodes[i].Name == item.ParentID.ToString())
                {
                    Parented[pos] = true;
                    Parent.Nodes[i].Nodes.Add(item.ItemID.ToString(), item.ItemID.ToString() + " - " + item.itemName);
                    return true;
                }
                else
                {
                    bool test = CheckChildNode(Parent.Nodes[i], item);
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

        //NEED To add check if item id changes
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

            EquipLinkList.Items.Clear();

            for (int i = 0; i < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks.Count; i++)
            {
                for (int j = 0; j < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Count; j++)
                {
                    if (BoltPS2Handler.characters[BoltCharacter.SelectedIndex].equipLinks[i].MainItemID == BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[j].ItemID)
                    {
                        EquipLinkList.Items.Add(BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[j].ItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[j].itemName);
                        break;
                    }
                }

            }

            DefaultOutfitItem.Items.Clear();

            for (int i = 0; i < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Count; i++)
            {
                DefaultOutfitItem.Items.Add(BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].ItemID + " - " + BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].itemName);
            }
        }

        private void EquipLinkList_SelectedIndexChanged(object sender, EventArgs e)
        {

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
            if(DefaultOutfitList.SelectedIndex != -1 && DefaultOutfitItem.SelectedIndex !=-1)
            {
                int ItemID = int.Parse(DefaultOutfitItem.GetItemText(DefaultOutfitItem.SelectedIndex).Split(" ")[0]);

                var Item = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].defaultOutfits[DefaultOutfitList.SelectedIndex];

                Item.ItemID = ItemID;

                for (int i = 0; i < BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries.Count; i++)
                {
                    if (BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].ItemID== ItemID)
                    {
                        Item.CategoryID = BoltPS2Handler.characters[BoltCharacter.SelectedIndex].entries[i].CharacterID;
                        break;
                    }
                }

                BoltPS2Handler.characters[BoltCharacter.SelectedIndex].defaultOutfits[DefaultOutfitList.SelectedIndex] = Item;

                DefaultOutfitList.Items[DefaultOutfitList.SelectedIndex] = DefaultOutfitItem.Text;
            }
        }

        //private void BoltDupe_Click(object sender, EventArgs e)
        //{
        //    if (BoltPS2TreeView.SelectedNode != null)
        //    {
        //        int CharIndex = BoltCharacter.SelectedIndex;
        //        int Index = -1;

        //        for (int i = 0; i < boltPS2.characters[CharIndex].entries.Count; i++)
        //        {
        //            if (BoltPS2TreeView.SelectedNode.Name == boltPS2.characters[CharIndex].entries[i].ItemID.ToString())
        //            {
        //                Index = i;
        //                break;
        //            }
        //        }

        //        var temp1 = boltPS2.characters[CharIndex];
        //        var temp = temp1.entries[Index];

        //        temp1.entries.Add(temp);

        //        boltPS2.characters[CharIndex] = temp1;
        //    }
        //}

        //private void BoltApply_Click(object sender, EventArgs e)
        //{
        //    if (BoltPS2TreeView.SelectedNode != null && loaded)
        //    {
        //        int Index1 = BoltCharacter.SelectedIndex;
        //        int Index = 0;

        //        for (int i = 0; i < boltPS2.characters[Index1].entries.Count; i++)
        //        {
        //            if (BoltPS2TreeView.SelectedNode.Name == boltPS2.characters[Index1].entries[i].ItemID.ToString())
        //            {
        //                Index = i;
        //                break;
        //            }
        //        }
        //        var Char = boltPS2.characters[Index1];
        //        var tempEntry = Char.entries[Index];

        //        tempEntry.unkownInt1 = (int)BoltUnkownOne.Value;
        //        tempEntry.UnlockCondition = (int)BoltUnlock.Value;
        //        tempEntry.TextureType = (int)BoltUnkownTwo.Value;
        //        tempEntry.ItemID = (int)BoltUnkownThree.Value;
        //        tempEntry.ParentID = (int)BoltUnkownFour.Value;
        //        tempEntry.category = (int)BoltCat.Value;
        //        tempEntry.buyable = (int)BoltBuy.Value;
        //        tempEntry.menuOrder = (int)BoltMenuOrder.Value;
        //        tempEntry.unkownInt5 = (int)BoltUnkown7.Value;
        //        tempEntry.weight = (int)BoltFillBar.Value;
        //        tempEntry.Cost = (int)BoltCost.Value;
        //        tempEntry.FileID = (int)BoltFileID.Value;

        //        tempEntry.SpecialID = (int)BoltSpecialOne.Value;
        //        tempEntry.SpecialID2 = (int)BoltSpecialTwo.Value;
        //        tempEntry.SpecialID3 = (int)BoltSpecialThree.Value;

        //        tempEntry.itemName = BoltName.Text;
        //        tempEntry.ModelID = BoltModelID.Text;
        //        tempEntry.ModelID2 = BoltModelIDTwo.Text;
        //        tempEntry.ModelID3 = BoltModelIDThree.Text;
        //        tempEntry.ModelID4 = BoltModelIDFour.Text;
        //        tempEntry.ModelPath = BoltModelPath.Text;
        //        tempEntry.TexturePath = BoltTexturePath.Text;
        //        tempEntry.SmallIcon = BoltIconPath.Text;

        //        tempEntry.unkownInt6 = (int)BoltUnkown9.Value;

        //        Char.entries[Index] = tempEntry;
        //        boltPS2.characters[Index1] = Char;

        //        GenerateTreeview();
        //    }
        //}

        //private void BoltPS2TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    if (BoltPS2TreeView.SelectedNode != null && loaded)
        //    {
        //        int Index1 = BoltCharacter.SelectedIndex;
        //        int Index = 0;

        //        for (int i = 0; i < boltPS2.characters[Index1].entries.Count; i++)
        //        {
        //            if (BoltPS2TreeView.SelectedNode.Name == boltPS2.characters[Index1].entries[i].ItemID.ToString())
        //            {
        //                Index = i;
        //                break;
        //            }
        //        }

        //        BoltUnkownOne.Value = boltPS2.characters[Index1].entries[Index].unkownInt1;
        //        BoltUnlock.Value = boltPS2.characters[Index1].entries[Index].UnlockCondition;
        //        BoltUnkownTwo.Value = boltPS2.characters[Index1].entries[Index].TextureType;
        //        BoltUnkownThree.Value = boltPS2.characters[Index1].entries[Index].ItemID;
        //        BoltUnkownFour.Value = boltPS2.characters[Index1].entries[Index].ParentID;
        //        BoltCat.Value = boltPS2.characters[Index1].entries[Index].category;
        //        BoltBuy.Value = boltPS2.characters[Index1].entries[Index].buyable;
        //        BoltMenuOrder.Value = boltPS2.characters[Index1].entries[Index].menuOrder;
        //        BoltUnkown7.Value = boltPS2.characters[Index1].entries[Index].unkownInt5;
        //        BoltFillBar.Value = boltPS2.characters[Index1].entries[Index].weight;
        //        BoltCost.Value = boltPS2.characters[Index1].entries[Index].Cost;
        //        BoltFileID.Value = boltPS2.characters[Index1].entries[Index].FileID;

        //        BoltSpecialOne.Value = boltPS2.characters[Index1].entries[Index].SpecialID;
        //        BoltSpecialTwo.Value = boltPS2.characters[Index1].entries[Index].SpecialID2;
        //        BoltSpecialThree.Value = boltPS2.characters[Index1].entries[Index].SpecialID3;

        //        BoltName.Text = boltPS2.characters[Index1].entries[Index].itemName;
        //        BoltModelID.Text = boltPS2.characters[Index1].entries[Index].ModelID;
        //        BoltModelIDTwo.Text = boltPS2.characters[Index1].entries[Index].ModelID2;
        //        BoltModelIDThree.Text = boltPS2.characters[Index1].entries[Index].ModelID3;
        //        BoltModelIDFour.Text = boltPS2.characters[Index1].entries[Index].ModelID4;
        //        BoltModelPath.Text = boltPS2.characters[Index1].entries[Index].ModelPath;
        //        BoltTexturePath.Text = boltPS2.characters[Index1].entries[Index].TexturePath;
        //        BoltIconPath.Text = boltPS2.characters[Index1].entries[Index].SmallIcon;

        //        BoltUnkown9.Value = boltPS2.characters[Index1].entries[Index].unkownInt6;
        //    }
        //}

        ////private void button1_Click(object sender, EventArgs e)
        ////{
        ////    for (int i = 0; i < boltPS2.unkown2.Count; i++)
        ////    {
        ////        var temp = boltPS2.unkown2[i];
        ////        temp.UnkownInt4 = 0;
        ////        boltPS2.unkown2[i] = temp;
        ////    }
        ////}

        //private void UnknownlistBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int Index1 = BoltCharacter2.SelectedIndex;
        //    if (UnknownlistBox1.SelectedIndex != -1)
        //    {
        //        Bolt1Unkown1.Value = boltPS2.characters[Index1].equipLinks[UnknownlistBox1.SelectedIndex].MainItemEquip;
        //        Bolt1Unkown2.Value = boltPS2.characters[Index1].equipLinks[UnknownlistBox1.SelectedIndex].MainItemID;
        //        Bolt1Unkown3.Value = boltPS2.characters[Index1].equipLinks[UnknownlistBox1.SelectedIndex].UnkownInt2;
        //        Bolt1Unkown4.Value = boltPS2.characters[Index1].equipLinks[UnknownlistBox1.SelectedIndex].IfEquipBool;
        //        Bolt1Unkown5.Value = boltPS2.characters[Index1].equipLinks[UnknownlistBox1.SelectedIndex].IfEquipID;
        //        Bolt1Unkown6.Value = boltPS2.characters[Index1].equipLinks[UnknownlistBox1.SelectedIndex].UnkownInt5;
        //        Bolt1Unkown7.Value = boltPS2.characters[Index1].equipLinks[UnknownlistBox1.SelectedIndex].UnkownInt6;
        //        Bolt1Unkown8.Value = boltPS2.characters[Index1].equipLinks[UnknownlistBox1.SelectedIndex].SecondaryItemEquip;
        //        Bolt1Unkown9.Value = boltPS2.characters[Index1].equipLinks[UnknownlistBox1.SelectedIndex].SecondaryItemID;
        //    }
        //}

        //private void BoltCharacter2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (BoltCharacter2.SelectedIndex != -1)
        //    {
        //        UnknownlistBox1.Items.Clear();
        //        for (int i = 0; i < boltPS2.characters[BoltCharacter2.SelectedIndex].equipLinks.Count; i++)
        //        {
        //            UnknownlistBox1.Items.Add(i.ToString());
        //        }
        //    }
        //}

        //#endregion
    }
}
