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
        void LoadMUS(string path)
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

                //typedef struct {
                //int32_t num_samples;
                //int32_t sample_rate;
                //int32_t channels;
                //int32_t platform;
                //int32_t version;
                //int32_t bps;
                //int32_t codec1;
                //int32_t codec2;

                //int32_t loop_start;
                //int32_t loop_end;

                //uint32_t flag_value;

                //off_t offsets[EA_MAX_CHANNELS];
                //off_t coefs[EA_MAX_CHANNELS];
                //off_t loops[EA_MAX_CHANNELS];

                //int big_endian;
                //int loop_flag;
                //int codec_config;
                //int use_pcm_blocks;

                //size_t stream_size;
                //}
                //ea_header;




                //header_id = read_32bitBE(offset + 0x00, sf);
                //if ((header_id & 0xFFFF0000) == EA_BLOCKID_LOC_HEADER)
                //{
                //    ea.codec_config |= (header_id & 0xFFFF) << 16;
                //}


                //SCHl
                //u32 HeaderSize
                //u32 PlatformID - PT0500 for ps2

                //All These Values Use a Patch System where the first u8 is what the data is the next u8 is how many bytes and the rest is the data
                //u8 06 PriorityID
                //u8 0B BankChannels
                //u8 FD Info Section Start (No Patch)
                //u8 80 Version
                //u8 85 SampleCount
                //u8 82 ChannelCount
                //u8 84 SampleRate
                //u8 A0 Codex2 Defines
                //u8 8C Unknown (Always seems to be 4?)
                //u8 FF Padding Start then 0-padded so it's 32b aligned
                //More Here but probably unneeded 
                //https://github.com/vgmstream/vgmstream/blob/master/src/meta/ea_schl.c#L1594
                //EA_CODEC2_VAG For PS2

                //SCCl
                //u32 HeaderSize
                //u32 BlockCount
            }
        }

    }
}

//https://github.com/vgmstream/vgmstream/blob/master/src/meta/ea_schl.c
//https://github.com/vgmstream/vgmstream/blob/master/src/meta/ea_schl_fixed.c
//https://github.com/vgmstream/vgmstream/blob/master/src/meta/ea_schl_streamfile.h
