using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers
{
    public class SMPHandler
    {
        byte[] magic; // 'smp!' 4
        int unk; // shape start addr
        int unk2;
        int unk3;
        int unk4; // table count
        int unk5;
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
