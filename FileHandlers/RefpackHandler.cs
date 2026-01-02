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
        public static int GetDecompressSize(byte[] bytes)
        {
            byte[] Signature = new byte[2];
            int DecompressSize;
            Stream stream = new MemoryStream(bytes);

            if (bytes.Length == 0)
            {
                return 0;
            }

            stream.Read(Signature, 0, 2);

            if (Signature[1] != 0xFB)
            {
                stream.Dispose();
                stream.Close();
                return 0;
            }

            if (Signature[0] == 0x01 && Signature[1] == 0x00)
            {
                stream.Position += 3;
            }

            DecompressSize = StreamUtil.ReadUInt24(stream, true);

            return DecompressSize;
        }

        public static byte[] Decompress(byte[] Matrix, bool bypass = false, int BypassSize = 0)
        {
            byte[] Signature = new byte[2];
            int DecompressSize;
            int CompressSize;

            Stream stream = new MemoryStream(Matrix);

            int first;
            int second;
            int third;
            int fourth;

            byte[] Output;
            int pos = 0;

            int proc_len; //Process Length
            int ref_run; //Refrence Run

            if (Matrix.Length == 0)
            {
                return null;
            }

            if (!bypass)
            {
                stream.Read(Signature, 0, 2);

                if (Signature[1] != 0xFB || Signature[0] != 0x10)
                {
                    stream.Dispose();
                    stream.Close();
                    return Matrix;
                }


                //NOT EVEN USED SO??
                if (Signature[0] == 0x01 && Signature[1] == 0x00)
                {
                    stream.Position += 3;
                }

                DecompressSize = StreamUtil.ReadInt24(stream, true);
            }
            else
            {
                DecompressSize = BypassSize;
            }

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

                    int TempPos = pos - ((first & 0x60) << 3) - second - 1;
                    ref_run = ((first >> 2) & 0x07) + 3;
                    for (int i = 0; i < ref_run; i++)
                    {
                        Output[pos] = Output[TempPos+i];
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

                    int TempPos = pos - ((second & 0x3f) << 8) - third - 1;
                    ref_run = (first & 0x3f) + 4;

                    for (int i = 0; i < ref_run; i++)
                    {
                        Output[pos] = Output[TempPos+i];
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

                    int TempPos = pos - ((first & 0x10) << 12) - (second << 8) - third - 1;
                    ref_run = ((first & 0x0c) << 6) + fourth + 5;

                    for (int i = 0; i < ref_run; i++)
                    {
                        Output[pos] = Output[TempPos+i];
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

        public static bool DecompressStream(Stream stream, Stream Output)
        {
            byte[] Signature = new byte[2];

            int first;
            int second;
            int third;
            int fourth;

            int proc_len; //Process Length
            int ref_run; //Refrence Run

            stream.Read(Signature, 0, 2);

            if (Signature[1] != 0xFB || Signature[0] != 0x10)
            {
                return false;
            }

            //NOT EVEN USED SO??
            if (Signature[0] == 0x01 && Signature[1] == 0x00)
            {
                stream.Position += 3;
            }

            int DecompressSize = StreamUtil.ReadInt24(stream, true);

            while (true)
            {
                first = stream.ReadByte();

                if ((first & 0x80) == 0) //Best Guess
                {
                    second = stream.ReadByte();

                    proc_len = first & 0x03;

                    StreamUtil.WriteBytes(Output, StreamUtil.ReadBytes(stream, proc_len));

                    long TempPos = Output.Position - ((first & 0x60) << 3) - second - 1;
                    ref_run = ((first >> 2) & 0x07) + 3;

                    long Temp = Output.Position;
                    Output.Position = TempPos;
                    byte[] array = StreamUtil.ReadBytes(Output, ref_run);
                    Output.Position = Temp;
                    StreamUtil.WriteBytes(Output, array);
                }
                else if ((first & 0x40) == 0)
                {
                    second = stream.ReadByte();
                    third = stream.ReadByte();

                    proc_len = second >> 6;
                    StreamUtil.WriteBytes(Output, StreamUtil.ReadBytes(stream, proc_len));

                    long TempPos = Output.Position - ((second & 0x3f) << 8) - third - 1;
                    ref_run = (first & 0x3f) + 4;

                    long Temp = Output.Position;
                    Output.Position = TempPos;
                    byte[] array = StreamUtil.ReadBytes(Output, ref_run);
                    Output.Position = Temp;
                    StreamUtil.WriteBytes(Output, array);
                }
                else if ((first & 0x20) == 0)
                {
                    second = stream.ReadByte();
                    third = stream.ReadByte();
                    fourth = stream.ReadByte();

                    proc_len = first & 0x03;

                    StreamUtil.WriteBytes(Output, StreamUtil.ReadBytes(stream, proc_len));

                    long TempPos = Output.Position - ((first & 0x10) << 12) - (second << 8) - third - 1;
                    ref_run = ((first & 0x0c) << 6) + fourth + 5;

                    long Temp = Output.Position;
                    Output.Position = TempPos;
                    byte[] array = StreamUtil.ReadBytes(Output, ref_run);
                    Output.Position = Temp;
                    StreamUtil.WriteBytes(Output, array);
                }
                else
                {
                    proc_len = (first & 0x1f) * 4 + 4;

                    if (proc_len <= 0x70)
                    {
                        // no stop flag

                        StreamUtil.WriteBytes(Output, StreamUtil.ReadBytes(stream, proc_len));

                    }
                    else
                    {
                        // has a stop flag
                        proc_len = first & 0x3;

                        StreamUtil.WriteBytes(Output, StreamUtil.ReadBytes(stream, proc_len));

                        break;
                    }
                }

            }
            return true;
        }

        //Taken from https://github.com/gibbed/Gibbed.RefPack
        public static bool Compress(byte[] input, out byte[] output, CompressionLevel? level = null)
        {
            if(level==null)
            {
                level = CompressionLevel.Max;
            }


             byte[] Signature = new byte[2];
             int DecompressSize;
             int CompressSize;


            if (input.LongLength >= 0xFFFFFFFF)
            {
                throw new InvalidOperationException("input data is too large");
            }

            var endIsValid = false;
            var compressedChunks = new List<byte[]>();
            var compressedIndex = 0;
            var compressedLength = 0;
            output = null;

            if (input.Length < 16)
            {
                return false;
            }

            var blockTrackingQueue = new Queue<KeyValuePair<int, int>>();
            var blockPretrackingQueue = new Queue<KeyValuePair<int, int>>();

            // So lists aren't being freed and allocated so much
            var unusedLists = new Queue<List<int>>();
            var latestBlocks = new Dictionary<int, List<int>>();
            var lastBlockStored = 0;

            while (compressedIndex < input.Length)
            {
                while (compressedIndex > lastBlockStored + level.BlockInterval && input.Length - compressedIndex > 16)
                {
                    if (blockPretrackingQueue.Count >= level.PrequeueLength)
                    {
                        var tmppair = blockPretrackingQueue.Dequeue();
                        blockTrackingQueue.Enqueue(tmppair);

                        List<int> valueList;

                        if (latestBlocks.TryGetValue(tmppair.Key, out valueList) == false)
                        {
                            valueList = unusedLists.Count > 0 ? unusedLists.Dequeue() : new List<int>();
                            latestBlocks[tmppair.Key] = valueList;
                        }

                        if (valueList.Count >= level.SameValToTrack)
                        {
                            var earliestIndex = 0;
                            var earliestValue = valueList[0];

                            for (int loop = 1; loop < valueList.Count; loop++)
                            {
                                if (valueList[loop] < earliestValue)
                                {
                                    earliestIndex = loop;
                                    earliestValue = valueList[loop];
                                }
                            }

                            valueList[earliestIndex] = tmppair.Value;
                        }
                        else
                        {
                            valueList.Add(tmppair.Value);
                        }

                        if (blockTrackingQueue.Count > level.QueueLength)
                        {
                            var tmppair2 = blockTrackingQueue.Dequeue();
                            valueList = latestBlocks[tmppair2.Key];

                            for (int loop = 0; loop < valueList.Count; loop++)
                            {
                                if (valueList[loop] == tmppair2.Value)
                                {
                                    valueList.RemoveAt(loop);
                                    break;
                                }
                            }

                            if (valueList.Count == 0)
                            {
                                latestBlocks.Remove(tmppair2.Key);
                                unusedLists.Enqueue(valueList);
                            }
                        }
                    }

                    var newBlock = new KeyValuePair<int, int>(BitConverter.ToInt32(input, lastBlockStored),
                                                              lastBlockStored);
                    lastBlockStored += level.BlockInterval;
                    blockPretrackingQueue.Enqueue(newBlock);
                }

                if (input.Length - compressedIndex < 4)
                {
                    // Just copy the rest
                    var chunk = new byte[input.Length - compressedIndex + 1];
                    chunk[0] = (byte)(0xFC | (input.Length - compressedIndex));
                    Array.Copy(input, compressedIndex, chunk, 1, input.Length - compressedIndex);

                    compressedChunks.Add(chunk);
                    compressedIndex += chunk.Length - 1;
                    compressedLength += chunk.Length;

                    // int toRead = 0;
                    // int toCopy2 = 0;
                    // int copyOffset = 0;

                    endIsValid = true;
                    continue;
                }

                // Search ahead the next 3 bytes for the "best" sequence to copy
                var sequenceStart = 0;
                var sequenceLength = 0;
                var sequenceIndex = 0;
                var isSequence = false;

                if (FindSequence(input,
                                 compressedIndex,
                                 ref sequenceStart,
                                 ref sequenceLength,
                                 ref sequenceIndex,
                                 latestBlocks,
                                 level))
                {
                    isSequence = true;
                }
                else
                {
                    // Find the next sequence
                    for (int loop = compressedIndex + 4;
                         isSequence == false && loop + 3 < input.Length;
                         loop += 4)
                    {
                        if (FindSequence(input,
                                         loop,
                                         ref sequenceStart,
                                         ref sequenceLength,
                                         ref sequenceIndex,
                                         latestBlocks,
                                         level))
                        {
                            sequenceIndex += loop - compressedIndex;
                            isSequence = true;
                        }
                    }

                    if (sequenceIndex == int.MaxValue)
                    {
                        sequenceIndex = input.Length - compressedIndex;
                    }

                    // Copy all the data skipped over
                    while (sequenceIndex >= 4)
                    {
                        int toCopy = (sequenceIndex & ~3);
                        if (toCopy > 112)
                        {
                            toCopy = 112;
                        }

                        var chunk = new byte[toCopy + 1];
                        chunk[0] = (byte)(0xE0 | ((toCopy >> 2) - 1));
                        Array.Copy(input, compressedIndex, chunk, 1, toCopy);
                        compressedChunks.Add(chunk);
                        compressedIndex += toCopy;
                        compressedLength += chunk.Length;
                        sequenceIndex -= toCopy;

                        // int toRead = 0;
                        // int toCopy2 = 0;
                        // int copyOffset = 0;
                    }
                }

                if (isSequence)
                {
                    /*
                     * 00-7F  0oocccpp oooooooo
                     *   Read 0-3
                     *   Copy 3-10
                     *   Offset 0-1023
                     *   
                     * 80-BF  10cccccc ppoooooo oooooooo
                     *   Read 0-3
                     *   Copy 4-67
                     *   Offset 0-16383
                     *   
                     * C0-DF  110cccpp oooooooo oooooooo cccccccc
                     *   Read 0-3
                     *   Copy 5-1028
                     *   Offset 0-131071
                     *   
                     * E0-FC  111ppppp
                     *   Read 4-128 (Multiples of 4)
                     *   
                     * FD-FF  111111pp
                     *   Read 0-3
                     */
                    if (FindRunLength(input, sequenceStart, compressedIndex + sequenceIndex) < sequenceLength)
                    {
                        break;
                    }

                    while (sequenceLength > 0)
                    {
                        int thisLength = sequenceLength;
                        if (thisLength > 1028)
                        {
                            thisLength = 1028;
                        }

                        sequenceLength -= thisLength;
                        int offset = compressedIndex - sequenceStart + sequenceIndex - 1;

                        byte[] chunk;
                        if (thisLength > 67 || offset > 16383)
                        {
                            chunk = new byte[sequenceIndex + 4];
                            chunk[0] =
                                (byte)
                                (0xC0 | sequenceIndex | (((thisLength - 5) >> 6) & 0x0C) | ((offset >> 12) & 0x10));
                            chunk[1] = (byte)((offset >> 8) & 0xFF);
                            chunk[2] = (byte)(offset & 0xFF);
                            chunk[3] = (byte)((thisLength - 5) & 0xFF);
                        }
                        else if (thisLength > 10 || offset > 1023)
                        {
                            chunk = new byte[sequenceIndex + 3];
                            chunk[0] = (byte)(0x80 | ((thisLength - 4) & 0x3F));
                            chunk[1] = (byte)(((sequenceIndex << 6) & 0xC0) | ((offset >> 8) & 0x3F));
                            chunk[2] = (byte)(offset & 0xFF);
                        }
                        else
                        {
                            chunk = new byte[sequenceIndex + 2];
                            chunk[0] =
                                (byte)
                                ((sequenceIndex & 0x3) | (((thisLength - 3) << 2) & 0x1C) | ((offset >> 3) & 0x60));
                            chunk[1] = (byte)(offset & 0xFF);
                        }

                        if (sequenceIndex > 0)
                        {
                            Array.Copy(input, compressedIndex, chunk, chunk.Length - sequenceIndex, sequenceIndex);
                        }

                        compressedChunks.Add(chunk);
                        compressedIndex += thisLength + sequenceIndex;
                        compressedLength += chunk.Length;

                        // int toRead = 0;
                        // int toCopy = 0;
                        // int copyOffset = 0;

                        sequenceStart += thisLength;
                        sequenceIndex = 0;
                    }
                }
            }

            if (/*compressedLength + 6 < input.Length*/true)
            {
                int chunkPosition;

                if (input.Length > 0xFFFFFF)
                {
                    output = new byte[compressedLength + 5 + (endIsValid ? 0 : 1)];
                    output[0] = 0x10 | 0x80; // 0x80 = length is 4 bytes
                    output[1] = 0xFB;
                    output[2] = (byte)(input.Length >> 24);
                    output[3] = (byte)(input.Length >> 16);
                    output[4] = (byte)(input.Length >> 8);
                    output[5] = (byte)(input.Length);
                    chunkPosition = 6;
                }
                else
                {
                    output = new byte[compressedLength + 5 + (endIsValid ? 0 : 1)];
                    output[0] = 0x10;
                    output[1] = 0xFB;
                    output[2] = (byte)(input.Length >> 16);
                    output[3] = (byte)(input.Length >> 8);
                    output[4] = (byte)(input.Length);
                    chunkPosition = 5;
                }

                foreach (byte[] t in compressedChunks)
                {
                    Array.Copy(t, 0, output, chunkPosition, t.Length);
                    chunkPosition += t.Length;
                }

                if (!endIsValid)
                {
                    output[output.Length - 1] = 0xFC;
                }

                return true;
            }

            return false;
        }

        private static bool FindSequence(byte[] data,int offset,ref int bestStart,ref int bestLength,ref int bestIndex,Dictionary<int, List<int>> blockTracking,CompressionLevel level)
        {
            int start;
            int end = -level.BruteForceLength;

            if (offset < level.BruteForceLength)
            {
                end = -offset;
            }

            if (offset > 4)
            {
                start = -3;
            }
            else
            {
                start = offset - 3;
            }

            bool foundRun = false;
            if (bestLength < 3)
            {
                bestLength = 3;
                bestIndex = int.MaxValue;
            }

            var search = new byte[data.Length - offset > 4 ? 4 : data.Length - offset];

            for (int loop = 0; loop < search.Length; loop++)
            {
                search[loop] = data[offset + loop];
            }

            while (start >= end && bestLength < 1028)
            {
                byte currentByte = data[start + offset];

                for (int loop = 0; loop < search.Length; loop++)
                {
                    if (currentByte != search[loop] || start >= loop || start - loop < -131072)
                    {
                        continue;
                    }

                    int len = FindRunLength(data, offset + start, offset + loop);

                    if ((len > bestLength || len == bestLength && loop < bestIndex) &&
                        (len >= 5 ||
                         len >= 4 && start - loop > -16384 ||
                         len >= 3 && start - loop > -1024))
                    {
                        foundRun = true;
                        bestStart = offset + start;
                        bestLength = len;
                        bestIndex = loop;
                    }
                }

                start--;
            }

            if (blockTracking.Count > 0 && data.Length - offset > 16 && bestLength < 1028)
            {
                for (int loop = 0; loop < 4; loop++)
                {
                    var thisPosition = offset + 3 - loop;
                    var adjust = loop > 3 ? loop - 3 : 0;
                    var value = BitConverter.ToInt32(data, thisPosition);
                    List<int> positions;

                    if (blockTracking.TryGetValue(value, out positions))
                    {
                        foreach (var trypos in positions)
                        {
                            int localadjust = adjust;

                            if (trypos + 131072 < offset + 8)
                            {
                                continue;
                            }

                            int length = FindRunLength(data, trypos + localadjust, thisPosition + localadjust);

                            if (length >= 5 && length > bestLength)
                            {
                                foundRun = true;
                                bestStart = trypos + localadjust;
                                bestLength = length;
                                if (loop < 3)
                                {
                                    bestIndex = 3 - loop;
                                }
                                else
                                {
                                    bestIndex = 0;
                                }
                            }

                            if (bestLength > 1028)
                            {
                                break;
                            }
                        }
                    }

                    if (bestLength > 1028)
                    {
                        break;
                    }
                }
            }

            return foundRun;
        }

        private static int FindRunLength(byte[] data, int source, int destination)
        {
            int endSource = source + 1;
            int endDestination = destination + 1;

            while (endDestination < data.Length && data[endSource] == data[endDestination] &&
                   endDestination - destination < 1028)
            {
                endSource++;
                endDestination++;
            }

            return endDestination - destination;
        }
    }

    public class CompressionLevel
    {
        public static readonly CompressionLevel Max = new CompressionLevel(1, 1, 10, 64);

        public readonly int BlockInterval;
        public readonly int SearchLength;
        public readonly int PrequeueLength;
        public readonly int QueueLength;
        public readonly int SameValToTrack;
        public readonly int BruteForceLength;

        public CompressionLevel(int blockInterval,
                                int searchLength,
                                int prequeueLength,
                                int queueLength,
                                int sameValToTrack,
                                int bruteForceLength)
        {
            this.BlockInterval = blockInterval;
            this.SearchLength = searchLength;
            this.PrequeueLength = prequeueLength;
            this.QueueLength = queueLength;
            this.SameValToTrack = sameValToTrack;
            this.BruteForceLength = bruteForceLength;
        }

        public CompressionLevel(int blockInterval, int searchLength, int sameValToTrack, int bruteForceLength)
        {
            this.BlockInterval = blockInterval;
            this.SearchLength = searchLength;
            this.PrequeueLength = this.SearchLength / this.BlockInterval;
            this.QueueLength = 131000 / this.BlockInterval - this.PrequeueLength;
            this.SameValToTrack = sameValToTrack;
            this.BruteForceLength = bruteForceLength;
        }
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
