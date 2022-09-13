using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SSXMultiTool.Utilities;

namespace SSXMultiTool.FileHandlers
{
    internal class RefpackHandler
    {
        byte[] Signature = new byte[2];
        public int DecompressSize;
        public int CompressSize;

        public byte[] Decompress(byte[] Matrix)
        {
            Stream stream = new MemoryStream(Matrix);

            int first;
            int second;
            int third;
            int fourth;

            byte[] Output;
            int pos = 0;

            int proc_len;
            int ref_run;
            byte[] ref_ptr;

            if(Matrix.Length==0)
            {
                return null;
            }

            stream.Read(Signature, 0, 2);

            if (Signature[1]!=0xFB)
            {
                stream.Dispose();
                stream.Close();
                return Matrix;
            }

            if (Signature[0] == 0x01 && Signature[1] == 0x00)
            {
                stream.Position+=3;
            }

            CompressSize = (int)stream.Length;
            DecompressSize = StreamUtil.ReadInt24Big(stream);

            Output = new byte[DecompressSize];

            while(true)
            {
                first = stream.ReadByte();

                if ((first & 0x80)==0) //Best Guess
                {
                    second = stream.ReadByte();

                    proc_len = first & 0x03;

                    for (int i = 0; i < proc_len; i++)
                    {
                        Output[pos] = (byte)stream.ReadByte();
                        pos++;
                    }

                    ref_ptr = Output;
                    int TempPos;
                    TempPos = pos - ((first & 0x60) << 3) - second - 1;

                    ref_run = ((first >> 2) & 0x07) + 3;
                    for (int i = 0; i < ref_run; i++)
                    {
                        Output[pos] = ref_ptr[TempPos+i];
                        pos++;
                    }

                }
                else if ((first & 0x40)==0)
                {
                    second = stream.ReadByte();
                    third = stream.ReadByte();

                    proc_len = second >> 6;
                    for (int i = 0; i < proc_len; i++)
                    {
                        Output[pos] = (byte)stream.ReadByte();
                        pos++;
                    }

                    ref_ptr = Output;
                    int TempPos;
                    TempPos = pos - ((second & 0x3f) << 8) - third - 1;
                    ref_run = (first & 0x3f) + 4;

                    for (int i = 0; i < ref_run; i++)
                    {
                        Output[pos] = ref_ptr[TempPos+i];
                        pos++;
                    }

                }
                else if((first & 0x20)==0)
                {
                    second = stream.ReadByte();
                    third = stream.ReadByte();
                    fourth = stream.ReadByte();

                    proc_len = first & 0x03;

                    for (int i = 0; i < proc_len; i++)
                    {
                        Output[pos] = (byte)stream.ReadByte();
                        pos++;
                    }

                    ref_ptr = Output;
                    int TempPos;
                    TempPos = pos - ((first & 0x10) << 12) - (second << 8) - third - 1;
                    ref_run = ((first & 0x0c) << 6) + fourth + 5;

                    for (int i = 0; i < ref_run; i++)
                    {
                        Output[pos] = ref_ptr[TempPos+i];
                        pos++;
                    }
                }
                else
                {
                    proc_len = (first & 0x1f) * 4 + 4;

                    if (proc_len <= 0x70)
                    {
                        // no stop flag

                        for (int i = 0; i < proc_len; i++)
                        {
                            Output[pos] = (byte)stream.ReadByte();
                            pos++;
                        }

                    }
                    else
                    {
                        // has a stop flag
                        proc_len = first & 0x3;

                        for (int i = 0; i < proc_len; i++)
                        {
                            Output[pos] = (byte)stream.ReadByte();
                            pos++;
                        }

                        break;
                    }
                }

            }

            stream.Dispose();
            stream.Close();
            return Output;
        }

        //public byte[] Compress(byte[] Matrix)
        //{
        //    //Not working so return Matrix
        //    return Matrix;



        //    //Int32
        //    int len;
        //    int tlen;
        //    int tcost;
        //    int run;
        //    int toffset;
        //    int boffset;
        //    int blen;
        //    int bcost;
        //    int mlen;
        //    //int 8 Pointer
        //    int tptr;
        //    int cptr;
        //    int rptr;

        //    int to = 0; //Was Output buffer pointer now pos

        //    int countliterals = 0;
        //    int countshort = 0;
        //    int countlong = 0;
        //    int countvlong = 0;
        //    long hash;
        //    long hoffset;
        //    long minhoffset;
        //    int i;

        //    //Int 32
        //    int link;
        //    int hashtbl;
        //    int hashptr;

        //    len = Matrix.Length;

        //    byte[] output = new byte[len*2+8192];

        //    for (i = 0; i < 4; i++, to++)
        //    {
        //        output[to] = (byte)(Matrix.Length >> (i * 8) & 255);
        //    }

        //    run = 0;

        //    //Not entirely sure what this does
        //    //cptr = rptr = static_cast<u_int8 *>(input.buffer);

        //    return null;
        //}
    }
}

//void RefPack::compress(const CompressorInput &input, DecompressorInput* output)
//{
//#if 0
//    u_int32 quick=0;          // seems to prevent a long compression if set true.  Probably affects compression ratio too.
//#endif

//    int32 len;
//    u_int32 tlen;
//    u_int32 tcost;
//    u_int32 run;
//    u_int32 toffset;
//    u_int32 boffset;
//    u_int32 blen;
//    u_int32 bcost;
//    u_int32 mlen;
//    const u_int8* tptr;
//    const u_int8* cptr;
//    const u_int8* rptr;
//    u_int8* to;

//    int countliterals = 0;
//    int countshort = 0;
//    int countlong = 0;
//    int countvlong = 0;
//    long hash;
//    long hoffset;
//    long minhoffset;
//    int i;
//    int32* link;
//    int32* hashtbl;
//    int32* hashptr;

//    len = input.lengthInBytes;

//    output->buffer = NEW int8[input.lengthInBytes * 2 + 8192];   // same wild guess Frank Barchard makes
//    to = static_cast<u_int8*>(output->buffer);

//    // write size into the stream 
//    for (i = 0; i < 4; i++, to++)
//        *to = static_cast<int8>(input.lengthInBytes >> (i * 8) & 255);

//    run = 0;
//    cptr = rptr = static_cast<u_int8*>(input.buffer);

//    hashtbl = NEW int32[65536];
//    link = NEW int32[131072];

//    hashptr = hashtbl;
//    for (i = 0; i < 65536L / 16; ++i)
//    {
//        *(hashptr + 0) = *(hashptr + 1) = *(hashptr + 2) = *(hashptr + 3) =
//        *(hashptr + 4) = *(hashptr + 5) = *(hashptr + 6) = *(hashptr + 7) =
//        *(hashptr + 8) = *(hashptr + 9) = *(hashptr + 10) = *(hashptr + 11) =
//        *(hashptr + 12) = hashptr[13] = hashptr[14] = hashptr[15] = -1L;
//        hashptr += 16;
//    }

//    while (len > 0)
//    {
//        boffset = 0;
//        blen = bcost = 2;
//        mlen = min(len, 1028);
//        tptr = cptr - 1;
//        hash = HASH(cptr);
//        hoffset = hashtbl[hash];
//        minhoffset = max(cptr - static_cast<u_int8*>(input.buffer) - 131071, 0);


//        if (hoffset >= minhoffset)
//        {
//            do
//            {
//                tptr = static_cast<u_int8*>(input.buffer) + hoffset;
//                if (cptr[blen] == tptr[blen])
//                {
//                    tlen = matchlen(cptr, tptr, mlen);
//                    if (tlen > blen)
//                    {
//                        toffset = (cptr - 1) - tptr;
//                        if (toffset < 1024 && tlen <= 10)       /* two byte long form */
//                            tcost = 2;
//                        else if (toffset < 16384 && tlen <= 67) /* three byte long form */
//                            tcost = 3;
//                        else                                /* four byte very long form */
//                            tcost = 4;

//                        if (tlen - tcost + 4 > blen - bcost + 4)
//                        {
//                            blen = tlen;
//                            bcost = tcost;
//                            boffset = toffset;
//                            if (blen >= 1028) break;
//                        }
//                    }
//                }
//            } while ((hoffset = link[hoffset & 131071]) >= minhoffset);
//        }
//        if (bcost >= blen)
//        {
//            hoffset = (cptr - static_cast<u_int8*>(input.buffer));
//            link[hoffset & 131071] = hashtbl[hash];
//            hashtbl[hash] = hoffset;

//            ++run;
//            ++cptr;
//            --len;
//        }
//        else
//        {
//            while (run > 3)                   /* literal block of data */
//            {
//                tlen = min((u_int32)112, run & ~3);
//                run -= tlen;
//                *to++ = (unsigned char) (0xe0 + (tlen >> 2) - 1);
//                memcpy(to, rptr, tlen);
//                rptr += tlen;
//                to += tlen;
//                ++countliterals;
//            }
//            if (bcost == 2)                   /* two byte long form */
//            {
//                *to++ = (unsigned char) (((boffset >> 8) << 5) + ((blen - 3) << 2) + run);
//                *to++ = (unsigned char) boffset;
//                ++countshort;
//            }
//            else if (bcost == 3)              /* three byte long form */
//            {
//                *to++ = (unsigned char) (0x80 + (blen - 4));
//                *to++ = (unsigned char) ((run << 6) + (boffset >> 8));
//                *to++ = (unsigned char) boffset;
//                ++countlong;
//            }
//            else                            /* four byte very long form */
//            {
//                *to++ = (unsigned char) (0xc0 + ((boffset >> 16) << 4) + (((blen - 5) >> 8) << 2) + run);
//                *to++ = (unsigned char) (boffset >> 8);
//                *to++ = (unsigned char) (boffset);
//                *to++ = (unsigned char) (blen - 5);
//                ++countvlong;
//            }
//            if (run)
//            {
//                memcpy(to, rptr, run);
//                to += run;
//                run = 0;
//            }
//#if 0
//            if (quick)
//            {
//                hoffset = (cptr-static_cast<u_int8 *>(input.buffer));
//                link[hoffset&131071] = hashtbl[hash];
//                hashtbl[hash] = hoffset;
//                cptr += blen;
//            }
//            else
//#endif
//            {
//                for (i = 0; i < (int)blen; ++i)
//                {
//                    hash = HASH(cptr);
//                    hoffset = (cptr - static_cast<u_int8*>(input.buffer));
//                    link[hoffset & 131071] = hashtbl[hash];
//                    hashtbl[hash] = hoffset;
//                    ++cptr;
//                }
//            }

//            rptr = cptr;
//            len -= blen;
//        }
//    }
//    while (run > 3)                       /* no match at end, use literal */
//    {
//        tlen = min((u_int32)112, run & ~3);
//        run -= tlen;
//        *to++ = (unsigned char) (0xe0 + (tlen >> 2) - 1);
//        memcpy(to, rptr, tlen);
//        rptr += tlen;
//        to += tlen;
//    }

//    *to++ = (unsigned char) (0xfc + run); /* end of stream command + 0..3 literal */
//    if (run)
//    {
//        memcpy(to, rptr, run);
//        to += run;
//    }

//    delete[] link;
//    delete[] hashtbl;

//    output->lengthInBytes = (to - static_cast<u_int8*>(output->buffer));
//}
