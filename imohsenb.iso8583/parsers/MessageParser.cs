using System;
using System.Collections;
using imohsenb.iso8583.enums;
using imohsenb.iso8583.utils;

namespace imohsenb.iso8583.parsers
{
    public class MessageParser
    {
        private byte[] message;
        int offset = 10;
        private int fieldLength = 0;
        private string fieldType = "unknown";
        private string fieldFormat = "LL";

        public MessageParser(byte[] message)
        {
            this.message = message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public byte[] parse(FIELDS field)
        {
            fieldLength = field.GetLength();
            fieldType = field.getType();
            fieldFormat = field.GetFormat();
            
            if (field.IsFixed())
                return fixedFieldParser();
            else
                return varFieldParser();
        }

        /// <summary>
        /// Fixed length field parser
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private byte[] fixedFieldParser()
        {
            switch (fieldType)
            {
                case "n":
                    return numericFixedFieldParser();
                default:
                    return alphaNumericFixedFieldParser();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private byte[] alphaNumericFixedFieldParser()
        {
            this.offset += fieldLength;
            return Arrays.CopyOfRange(this.message, this.offset - fieldLength, this.offset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private byte[] numericFixedFieldParser()
        {
            var len = fieldLength;
            len = (int) Math.Ceiling((decimal)len/2);
            this.offset += len;
            return Arrays.CopyOfRange(this.message, this.offset - len, this.offset);
        }
        
        /// <summary>
        /// Variable length field parser
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private byte[] varFieldParser()
        {
            switch (fieldFormat)
            {
                case "LL":
                    return varFieldParser(1);
                case "LLL":
                    return varFieldParser(2);
            }
            return new byte[0];
        }

        /// <summary>
        /// Variable field parser
        /// </summary>
        /// <param name="formatLength"></param>
        /// <returns></returns>
        private byte[] varFieldParser(int formatLength)
        {
            int len = Convert.ToInt32(
                StringUtil.FromByteArray(Arrays.CopyOfRange(message, offset, offset + formatLength)));

            if (fieldType.Equals("z"))
                len /= 2;
            
            offset = offset + formatLength;

            offset += len;

            return Arrays.CopyOfRange(message, offset - len, offset);
        }
    }
}