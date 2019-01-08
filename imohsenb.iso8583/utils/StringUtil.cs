using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace imohsenb.iso8583.utils
{
    public class StringUtil
    {
        private static char[] hexArray = "0123456789ABCDEF".ToCharArray();

        public static string FromByteArray(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < data.Length; j++)
            {
                int v = data[j] & 0xFF;

                sb.Append(v.ToString("X2"));
            }
            return sb.ToString();
        }

        public static string AsciiFromByteArray(byte[] data)
        {
            return HexToAscii(FromByteArray(data));
        }

        public static string AsciiToHex(string asciiStr)
        {
            char[] chars = asciiStr.ToCharArray();
            StringBuilder hex = new StringBuilder();
            foreach (char ch in chars)
            {
                hex.Append(IntToHexString(ch));
            }

            return hex.ToString();
        }

        public static string HexToAscii(string data)
        {
            StringBuilder output = new StringBuilder();
            char[] chars = data.ToCharArray();
            for (int i = 0; i < chars.Length; i+=2)
            {
                output.Append(Convert.ToChar(Convert.ToInt32(chars[i] + "" + chars[i + 1], 16)));
            }

            return output.ToString();
        }

        public static byte[] AsciiToHex(byte[] data)
        {

            char[] hexChars = new char[data.Length*2];
            for (int j = 0; j < data.Length; j++)
            {
                int v = data[j] & 0xFF;
                hexChars[j*2] = hexArray[v >> 4];
                hexChars[j*2 + 1] = hexArray[v & 0x0F];
            }

            byte[] res = new byte[hexChars.Length];
            for (int i = 0; i < hexChars.Length; i++)
            {
                res[i] = (byte) hexChars[i];
            }

            return res;
        }

        public static byte[] HexStringToByteArray(string hexString)
        {
            if (hexString == null)
                return new byte[0];

            int len = hexString.Length;
            if (len % 2 != 0)
            {
                hexString = "0" + hexString;
                len++;
            }

            byte[] data = new byte[hexString.Length / 2];
            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            return data;
        }


        public static string IntToHexString(int value)
        {
            return value.ToString("X2");
        }

        public static byte[] AsciiToByteArray(byte[] bytes)
        {
            return HexStringToByteArray(HexToAscii(FromByteArray(bytes)));
        }

        public static string ToHexstring(string str)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                sb.Append(ToHexstring(str[i]));
            }
            return sb.ToString();
        }

        /**
         * convert into Hexadecimal notation of Unicode.<br>
         * example)a?\u0061
         * @param ch
         * @return
         */

        public static string ToHexstring(char ch)
        {
            string hex = IntToHexString((int) ch);
            while (hex.Length < 4)
            {
                hex = "0" + hex;
            }
            hex = "\\u" + hex;
            return hex;
        }

        public static string BitArrayToHexString(BitArray bitArray)
        {
            StringBuilder buffer = new StringBuilder();
            string bStr = ToBitString(bitArray);

            for (int c = 0; c < bitArray.Count; c = c + 4)
            {
                int _decimal = (int)Convert.ToInt64(bStr.Substring(c, 4),2);
                string hexStr = (_decimal).ToString("X1");
                buffer.Append(hexStr);
            }
            return buffer.ToString();
        }

        public static string ToBitString(BitArray bits)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < bits.Count; i++)
            {
                char c = bits[i] ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
        }

        public static BitArray BitArrayFromHexString(string value)
        {
            BitArray ba = new BitArray(value.Length/2*8);
            int offset = 0;
            for (int i = 0; i < value.Length; i = i + 1)
            {
                string item = value.Substring(i, 1);
                byte bitem = (byte) Convert.ToByte(item, 16);
                if ((bitem & Convert.ToInt32("1000",2)) > 0)
                    ba.Set(offset, true);
                if ((bitem & Convert.ToInt32("0100", 2)) > 0)
                    ba.Set(offset + 1, true);
                if ((bitem & Convert.ToInt32("0010", 2)) > 0)
                    ba.Set(offset + 2, true);
                if ((bitem & Convert.ToInt32("0001", 2)) > 0)
                    ba.Set(offset + 3, true);
                offset += 4;
            }
            return ba;
        }
    }
}
