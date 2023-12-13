using SSXMultiTool.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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
        public int TexIsNum; //0/1 (1 is normal/DXT5, dds2gtf.exe doesn't work with it) (int 16)
        public int Unknown0; //0 (int 16)
        public int HeaderLenght; //128 also offset to texture buffer
        public int TextureLenght; //texture/pixel buffer (including mipmaps)
        public int TextureType; // 0x86 (134) DXT1 | 0x88 (136) DXT5 (8)
        public int NumMipmaps; //10 (8)
        public int Unknown1; // (2, 0) or (512) (2 bytes)
        public int Remaps; //not sure what remaps refers to
        public int TextureWidth; //(int 16)
        public int TextureHeight; //(int 16)
        public int TextureDepth; //1 (int 16)

        public void GTFToDDS(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                GTFVersion = StreamUtil.ReadUInt32(stream);
                FileLenght = StreamUtil.ReadUInt32(stream);
                NumTextures = StreamUtil.ReadUInt32(stream);
                TexIsNum = StreamUtil.ReadUInt16(stream);
                Unknown0 = StreamUtil.ReadUInt16(stream);
                HeaderLenght = StreamUtil.ReadUInt32(stream);
                TextureLenght = StreamUtil.ReadUInt32(stream);
                TextureType = StreamUtil.ReadUInt8(stream);
                NumMipmaps = StreamUtil.ReadUInt8(stream);
                Unknown1 = StreamUtil.ReadUInt16(stream);
                Remaps = StreamUtil.ReadUInt32(stream);
                TextureWidth = StreamUtil.ReadUInt16(stream);
                TextureHeight = StreamUtil.ReadUInt16(stream);
                TextureDepth = StreamUtil.ReadUInt16(stream);

                string DXT_Type = "";
                if(TextureType == 0x86)
                {
                    DXT_Type = "DXT1";
                }
                else if (TextureType == 0x88)
                {
                    DXT_Type = "DXT5";
                }

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


                byte[] Bytes = StreamUtil.ReadBytes(stream, TextureLenght);
            }
        }

        public void DDSToGTF(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {

            }
        }

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
