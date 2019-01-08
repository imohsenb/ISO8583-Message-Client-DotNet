using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imohsenb.iso8583.utils
{
    public static class TLVParser
    {
        public static SortedDictionary<string, byte[]> Parse(byte[] message, int tagLength, int lengthOfDataLen)
        {
            SortedDictionary<string, byte[]> parts = new SortedDictionary<string, byte[]>();

            int offset = 0;

            while (offset < message.Length)
            {
                string tag = Encoding.ASCII.GetString(Arrays.CopyOfRange(message, offset, offset + tagLength));

                int len = Int32.Parse(Encoding.ASCII.GetString(Arrays.CopyOfRange(message, offset + tagLength, offset + tagLength + lengthOfDataLen)));

                if (message.Length >= offset + tagLength + lengthOfDataLen + len)
                    parts.Add(tag, Arrays.CopyOfRange(message, offset + tagLength + lengthOfDataLen, offset + tagLength + lengthOfDataLen + len));
                offset += len + tagLength + lengthOfDataLen;
            }

            return parts;
        }
    }
}
