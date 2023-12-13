using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.FileHandlers
{
    public class GTFHandler
    {
        public int GTFVersion;
        public int FileLenght;  //excluding header (add len_header for full length)
        public int NumTextures; // 1
        public int TexIsNum; //0/1 (1 is normal/DXT5, dds2gtf.exe doesn't work with it) 
        public int Unknown0; //0
        public int HeaderLenght; //
        public int TextureLenght;
        public int TextureType;
        public int NumMipmaps;
        public int Unknown1;
        public int Remaps;
        public int TextureWidth;
        public int TextureHeight;
        public int TextureDepth;

        public void GTFToDDS(string path)
        {

        }

        public void DDSToGTF(string path)
        {

        }

//        with open(self.in_file_path, 'rb') as f:
//            gtf_version  = f.read(4)
//            len_file     = get_uint32(f) # excluding header (add len_header for full length)
//            num_textures = get_uint32(f) # 1
//            tex_is_nm    = get_uint16(f) # 0/1 (1 is normal/DXT5, dds2gtf.exe doesn't work with it) 
//            unk          = get_uint16(f) # 0
//            len_header   = get_uint32(f) # 128 also offset to texture buffer
//            len_texture  = get_uint32(f) # texture/pixel buffer (including mipmaps)
//            tex_type     = get_uint8(f) # 0x86 (134) DXT1 | 0x88 (136) DXT5
//            num_mipmaps  = get_uint8(f) # 10
//            unk          = f.read(2) # (2, 0) or (512)
//            unk_remaps   = f.read(4) # not sure what remaps refers to
//            tex_width  = get_uint16(f)
//            tex_height = get_uint16(f)
//            tex_depth  = get_uint16(f) # 1

//            dxt_type = ""
//            if tex_type == 0x86:
//                dxt_type =  'DXT1'
//            elif tex_type == 0x88:
//                dxt_type = 'DXT5'
//            else:
//                print("Unknown type")
//                sys.exit()

//            if num_textures > 1:
//                print("Input file contains more than 1 texture. Only one can be extracted.")
//            elif num_textures == 0:
//                print("Input file has 0 textures.")
//                sys.exit()

//            f.seek(len_header)
//            tex_buffer = f.read(len_texture)

//# New DDS buffer

//        dds_buffer = b'DDS\x20\x7c\x00\x00\x00\x07\x10\x0a\x00'
//        dds_buffer += s.pack('<I', tex_height)
//        dds_buffer += s.pack('<I', tex_width)
//        dds_buffer += b'\x00\x00'
//        if dxt_type == 'DXT1':
//            dds_buffer += b'\x02\x00'
//        else:
//            dds_buffer += b'\x04\x00'
//        dds_buffer += b'\x00\x00\x00\x00'
//        dds_buffer += s.pack('<I', num_mipmaps)
//        dds_buffer += b'\x00' * 44
//        dds_buffer += b'\x20\x00\x00\x00'
//        dds_buffer += b'\x04\x00\x00\x00'
//        dds_buffer += dxt_type.encode('utf-8')
//        dds_buffer += b'\x00' * 20
//        dds_buffer += b'\x08\x10\x40\x00'
//        dds_buffer += b'\x00' * 16

//        dds_buffer += tex_buffer

//        with open(self.out_file_path, 'wb') as f:
//            f.write(dds_buffer)

//        print(f"\nOutput path:\n{os.path.abspath(self.out_file_path)}\nFinished converting!")


//    def dds_to_gtf(self):
//        self.out_file_path += '.gtf'

//        with open(self.in_file_path, 'rb') as f:

//            f.seek(4)
//            len_header = get_uint32(f) + 4
//            f.seek(0xc)
//            tex_height = get_uint32(f)
//            tex_width  = get_uint32(f)
//            len_tex_linear = get_uint32(f) # byte size of first texture
//            f.seek(0x1c)
//            num_mipmaps = get_uint32(f)
//            f.seek(0x4c)
//            pixel_format0 = get_uint32(f) # size?  # DDSPixelFormat
//            pixel_format1 = get_uint32(f) # flags?
//            dxt_type = get_string(f, 4)

//            len_block = 16
//            if dxt_type == 'DXT1':
//                len_block = 8
//            elif dxt_type == 'DXT5':
//                len_block = 16
//            else:
//                print("Unknown DXT type. Trying DXT5.")

//            len_texture = 0 # including mipmaps
//            mip_w = tex_width
//            mip_h = tex_height
//            for i in range(num_mipmaps) : # idk if this is the right way but it seems to work
//                len_mip = (mip_w // 4) * (mip_h // 4) * len_block
//                len_texture += len_mip

//                mip_w = mip_w / 2
//                mip_h = mip_h / 2

//            len_texture = int (len_texture + len_block* 2)
//            len_end_buffer = self.len_in_file - len_header
            
//            if len_texture<len_end_buffer:
//                print("len_texture is less than len_end_buffer. Using len_end_buffer.")
//                len_texture = len_end_buffer

//            f.seek(len_header)

//            tex_buffer = f.read(len_texture)

//        end_padding = calc_padding(len_texture) + 32
//        tex_is_nm = b''
//        if dxt_type == 'DXT1':
//            tex_is_nm += b'\x00\x00\x00\x00'
//            tex_type = b'\x86'
//        else:
//            tex_is_nm += b'\x00\x01\x00\x00'
//            tex_type = b'\x88'

//        gtf_buffer = b'\x01\x05\x00\x00'
//        gtf_buffer += s.pack('>I', len_texture + end_padding)
//        gtf_buffer += b'\x00\x00\x00\x01' # num_textures
//        gtf_buffer += tex_is_nm
//        gtf_buffer += b'\x00\x00\x00\x80' # len_header
//        gtf_buffer += s.pack('>I', len_texture)
//        gtf_buffer += tex_type
//        gtf_buffer += s.pack('B', num_mipmaps)
//        gtf_buffer += b'\x02\x00\x00\x00\xaa\xe4'
//        gtf_buffer += s.pack('>H', tex_width)
//        gtf_buffer += s.pack('>H', tex_height)
//        gtf_buffer += b'\x00\x01' # tex_depth
//        gtf_buffer += b'\x00' * 0x5a # 0xa + 0x50

//        gtf_buffer += tex_buffer
//        gtf_buffer += b'\x00' * end_padding

//# print(len_texture + calc_padding(len_texture) + 16)

//        with open(self.out_file_path, 'wb') as f:
//            f.write(gtf_buffer)

//        print(f"\nOutput path:\n{os.path.abspath(self.out_file_path)}\nFinished converting!")
    }
}
