using System;

namespace imohsenb.iso8583.utils
{
    public class Arrays
    {
        public static byte[] CopyOfRange(byte[] source, int from, int to)
        {
            
            int length = to - from;

            if (from > source.Length)
                return new byte[length];

            if (length + from > source.Length)
                length = source.Length - from;
            byte[] dest = new byte[length];
            Array.Copy(source, from, dest, 0, length);
            return dest;
        }

        public static byte[] CopyOf(byte[] source, int len)
        {
            byte[] dest = new byte[len];
            Array.Copy(source, 0, dest, 0, len);
            return dest;
        }
    }
}
