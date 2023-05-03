using NAudio.Wave.Compression;
using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ExplorerBar;

namespace SSXMultiTool.FileHandlers.Audio
{
    public class EAAudioHandler
    {
        SCHlHeader schlHeader = new SCHlHeader();
        SCClHeader scclHeader = new SCClHeader();
        List<SCDlHeader> scdlHeaders = new List<SCDlHeader>();

        public void Load(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                //#define EA_BLOCKID_HEADER           0x5343486C /* "SCHl" */
                //#define EA_BLOCKID_COUNT            0x5343436C /* "SCCl" */
                //#define EA_BLOCKID_DATA             0x5343446C /* "SCDl" */
                //#define EA_BLOCKID_END              0x5343456C /* "SCEl" */
                /* Stream is divided into blocks/chunks: SCHl=audio header, SCCl=count of SCDl, SCDl=data xN, SCLl=loop end, SCEl=end.
                   Video uses picture blocks (MVhd/MV0K/etc) and sometimes multiaudio blocks (SHxx/SCxx/SDxx/SExx where xx=language).
                   The number/size is affected by: block rate setting, sample rate, channels, CPU location (SPU/main/DSP/others), etc */

                schlHeader.HeaderMagic = StreamUtil.ReadString(stream, 4);
                schlHeader.HeaderSize = StreamUtil.ReadUInt32(stream);
                schlHeader.PlatformID = StreamUtil.ReadUInt32(stream)>>16;

                while (stream.Position < schlHeader.HeaderSize)
                {
                    //More Here but probably unneeded 
                    //https://github.com/vgmstream/vgmstream/blob/master/src/meta/ea_schl.c#L1594
                    int DataType = StreamUtil.ReadUInt8(stream);
                    switch (DataType)
                    {
                        case 0x06:
                            schlHeader.PriorityID = PatchRead(stream);
                            break;
                        case 0x0B:
                            schlHeader.BankChannels = PatchRead(stream);
                            break;
                        case 0xFD:
                            //Info Header Start
                            break;
                        case 0x80:
                            schlHeader.Version = PatchRead(stream);
                            break;
                        case 0x85:
                            schlHeader.SampleCount = PatchRead(stream);
                            break;
                        case 0x82:
                            schlHeader.ChannelCount = PatchRead(stream);
                            break;
                        case 0x84:
                            schlHeader.SampleRate = PatchRead(stream);
                            break;
                        case 0xA0:
                            schlHeader.Codex2Def = PatchRead(stream);
                            break;
                        case 0x8C:
                            schlHeader.Unknown = PatchRead(stream);
                            break;
                        case 0xFF:
                            StreamUtil.AlignBy(stream, 4);
                            break;
                        default:
                            int Temp = PatchRead(stream);
                            MessageBox.Show($"Error Unkwon Patch Type {DataType} With Data {Temp}");
                            break;
                    }
                }

                if(schlHeader.SampleRate==0)
                {
                    switch (schlHeader.PlatformID)
                    {
                        case 0x05: //PS2
                            schlHeader.SampleRate = 22050;
                            break;
                        default:
                            break;
                    }
                }

                scclHeader.HeaderMagic = StreamUtil.ReadString(stream, 4);
                scclHeader.HeaderSize = StreamUtil.ReadUInt32(stream);
                scclHeader.BlockCount = StreamUtil.ReadUInt32(stream);

                for (int i = 0; i < scclHeader.BlockCount; i++)
                {
                    var NewSCDl = new SCDlHeader();
                    NewSCDl.HeaderMagic = StreamUtil.ReadString(stream, 4);
                    NewSCDl.HeaderSize = StreamUtil.ReadUInt32(stream);
                    NewSCDl.AudioData = StreamUtil.ReadBytes(stream, NewSCDl.HeaderSize - 8);
                    scdlHeaders.Add(NewSCDl);
                }

            }
        }

        public void DecodeAudio()
        {

        }

        public int PatchRead(Stream stream)
        {
            int ByteSize = StreamUtil.ReadInt8(stream);
            byte[] TempValue = StreamUtil.ReadBytes(stream, ByteSize, true);
            byte[] Value = new byte[4];
            for (int i = 0; i < TempValue.Length; i++)
            {
                Value[i] = TempValue[i];
            }
            return BitConverter.ToInt32(Value, 0);

        }

        public struct SCHlHeader
        {
            public string HeaderMagic;
            public int HeaderSize;
            public int PlatformID;

            public int PriorityID;
            public int BankChannels;
            public int Version;
            public int SampleCount;
            public int ChannelCount;
            public int SampleRate;
            public int Codex2Def;
            public int Unknown;
        }

        public struct SCClHeader
        {
            public string HeaderMagic;
            public int HeaderSize;
            public int BlockCount;
        }

        public struct SCDlHeader
        {
            public string HeaderMagic;
            public int HeaderSize;

            public byte[] AudioData;
        }
    }
}

//https://github.com/vgmstream/vgmstream/blob/master/src/meta/ea_schl.c
//https://github.com/vgmstream/vgmstream/blob/master/src/meta/ea_schl_fixed.c
//https://github.com/vgmstream/vgmstream/blob/master/src/meta/ea_schl_streamfile.h
