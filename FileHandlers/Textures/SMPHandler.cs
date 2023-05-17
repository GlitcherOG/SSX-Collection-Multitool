using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers.Textures
{
    public class SMPHandler
    {
        byte[] magic; // 'smp!' 4
        int HeaderSize; // shape start addr
        int unk2; //Shps Size
        int unk3; //Appears to be some kind of mode
        int unk4; //total table count
        int unk5; //
        int unk6;
        int unk7;
        int unk8;
        int unk9;
        int unk10;
        int unk11;
        int unk12;
        int unk13;
        int unk14;

        List<SmpTabEntry> test = new List<SmpTabEntry>();

    }

    struct SmpTabEntry
    {
        string ShapeName;
        int unk; // could be shape count?
    };
}
