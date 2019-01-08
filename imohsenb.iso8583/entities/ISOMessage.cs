using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using imohsenb.iso8583.enums;
using imohsenb.iso8583.exceptions;
using imohsenb.iso8583.interfaces;
using imohsenb.iso8583.parsers;
using imohsenb.iso8583.utils;

namespace imohsenb.iso8583.entities
{
    /// <summary>
    /// ISO Message object holding iso data and provide useful method to use data
    /// </summary>
    public class ISOMessage
    {

        #region Variables
        private SortedDictionary<int, byte[]> dataElements = new SortedDictionary<int, byte[]>();
        private bool isNil = true;
        private string message;
        private string mti;
        private byte[] msg;
        private byte[] header;
        private byte[] body;
        private byte[] primaryBitmap;
        private int msgClass;
        private int msgFunction;
        private int msgOrigin;
        private int len = 0;
        #endregion

        public static ISOMessage NullObject()
        {
            return new ISOMessage();
        }

        /// <summary>
        /// Check message is Nil
        /// </summary>
        /// <returns>true if message is Nil</returns>
        public bool IsNil()
        {
            return isNil;
        }

        /// <summary>
        /// Get message header
        /// </summary>
        /// <returns>message header in byte array</returns>
        public byte[] GetHeader()
        {
            return header;
        }

        /// <summary>
        /// Get message body
        /// </summary>
        /// <returns>Message body in byte array</returns>
        public byte[] GetBody()
        {
            return body;
        }

        /// <summary>
        /// Length of message
        /// </summary>
        /// <returns></returns>
        public int Length()
        {
            return len;
        }

        /// <summary>
        /// Get special field from message with field number
        /// </summary>
        /// <param name="fieldNo">field number</param>
        /// <returns>field data</returns>
        /// <exception cref="ISOException">throws ISOException if field does not exists</exception>
        public byte[] GetField(int fieldNo)
        {
            if (!dataElements.ContainsKey(fieldNo))
                throw new ISOException("Field No " + fieldNo + " does not exists");
            return dataElements[fieldNo];
        }

        /// <summary>
        /// Get special field from message with field enum
        /// </summary>
        /// <param name="field">field enum</param>
        /// <returns></returns>
        /// <exception cref="ISOException">throws ISOException if field does not exists</exception>
        public byte[] GetField(FIELDS field)
        {
            if (!dataElements.ContainsKey(field.GetNo()))
                throw new ISOException("Field No " + field.GetNo() + " does not exists");
            return dataElements[field.GetNo()];
        }

        /// <summary>
        /// Get string representation of field no
        /// </summary>
        /// <param name="fieldNo">Field number</param>
        /// <returns>Field value in string</returns>
        public string GetStringField(int fieldNo)
        {
            return GetStringField(FIELDS.ValueOf(fieldNo));
        }

        /// <summary>
        /// Get string representation of field enum
        /// </summary>
        /// <param name="field">Field enum</param>
        /// <returns>Field value in string</returns>
        public string GetStringField(FIELDS field)
        {
            return GetStringField(field, false);
        }

        /// <summary>
        /// Get string representation of field no
        /// </summary>
        /// <param name="fieldNo">Field no</param>
        /// <param name="asciiFix">Fix ascii characters</param>
        /// <returns>Field value in string</returns>
        public string GetStringField(int fieldNo, bool asciiFix)
        {
            return GetStringField(FIELDS.ValueOf(fieldNo), asciiFix);
        }

        /// <summary>
        /// Get string representation of field enum
        /// </summary>
        /// <param name="field">Field enum</param>
        /// <param name="asciiFix">Fix ascii characters</param>
        /// <returns>Field value in string</returns>
        public string GetStringField(FIELDS field, bool asciiFix)
        {
            string temp = StringUtil.FromByteArray(GetField(field.GetNo()));
            if (asciiFix && !field.getType().Equals("n"))
                return StringUtil.HexToAscii(temp);
            return temp;
        }

        /// <summary>
        /// Set and parse message to ISOMessage 
        /// </summary>
        /// <param name="message">message for parsing</param>
        /// <param name="headerAvailable">it needs to consider ISO message header or not</param>
        /// <returns></returns>
        public ISOMessage SetMessage(byte[] message, bool headerAvailable)
        {
            isNil = false;

            this.msg = message;
            len = this.msg.Length/2;

            int headerOffset = 0;

            //Consider 5 byte for header
            if (headerAvailable)
                headerOffset = 5;

            this.header = Arrays.CopyOfRange(this.msg, 0, headerOffset);
            this.body = Arrays.CopyOfRange(this.msg, headerOffset, this.msg.Length);
            this.primaryBitmap = Arrays.CopyOfRange(body, 2, 10);

            //parse message header
            parseHeader();
            //parse message body
            parseBody();

            return this;
        }

        /// <summary>
        /// Set and parse iso message to ISOMessage
        /// </summary>
        /// <param name="message">Iso message in byte array</param>
        /// <returns></returns>
        public ISOMessage SetMessage(byte[] message)
        {
            return this.SetMessage(message, true);
        }

        /// <summary>
        /// Parse header
        /// </summary>
        private void parseHeader()
        {
            if (body.Length > 2)
            {
                mti = StringUtil.FromByteArray(Arrays.CopyOfRange(body, 0, 2));
                msgClass = Convert.ToInt32(mti.Substring(1, 1));
                msgFunction = Convert.ToInt32(mti.Substring(2, 1));
                msgOrigin = Convert.ToInt32(mti.Substring(3, 1));
            }
        }
        
        /// <summary>
        /// Parse body
        /// </summary>
        private void parseBody()
        {
            BitArray pb = StringUtil.BitArrayFromHexString(StringUtil.FromByteArray(primaryBitmap));
            
            MessageParser parser = new MessageParser(body);

            for (int i = 1;i < pb.Count; i++)
            {
                int o = i + 1;
                if (!pb.Get(i))
                    continue;

                FIELDS field = FIELDS.ValueOf(o);

                byte[] fieldData = parser.parse(field);

                AddElement(field, fieldData);
            }
        }

        /// <summary>
        /// Add new field to data element stack
        /// </summary>
        /// <param name="field">field enum</param>
        /// <param name="body">body of message</param>
        /// <param name="offset">field offset</param>
        /// <param name="len">field data len</param>
        /// <exception cref="ISOException"></exception>
        private void AddElement(FIELDS field, byte[] body, int offset, int len)
        {
            if (body.Length >= offset + len)
            {
                AddElement(field, Arrays.CopyOfRange(body, offset, offset + len));
            }
            else
            {
                throw new ISOException("ArrayIndexOutOfBoundsException for field " + field + " len: " + body.Length +
                                       "/" + (offset + len));
            }
        }

        /// <summary>
        /// Add new field to data element stack
        /// </summary>
        /// <param name="field">field enum</param>
        /// <param name="data">field data</param>
        private void AddElement(FIELDS field, byte[] data)
        {
            dataElements.Add(field.GetNo(), data);
        }

        /// <summary>
        /// Check if fields exists by field enum
        /// </summary>
        /// <param name="field">Field enum</param>
        /// <returns>Returns true if field already exists</returns>
        public bool FieldExits(FIELDS field)
        {
            return FieldExits(field.GetNo());
        }

        /// <summary>
        /// Check if fields exists
        /// </summary>
        /// <param name="no">Field number</param>
        /// <returns>Returns true if field already exists</returns>
        public bool FieldExits(int no)
        {
            return dataElements.ContainsKey(no);
        }

        /// <summary>
        /// Get message MTI code
        /// </summary>
        /// <returns></returns>
        public string GetMti()
        {
            return mti;
        }

        /// <summary>
        /// Get Message Class
        /// </summary>
        /// <returns></returns>
        public int GetMsgClass()
        {
            return msgClass;
        }

        /// <summary>
        /// Get message function code
        /// </summary>
        /// <returns></returns>
        public int GetMsgFunction()
        {
            return msgFunction;
        }

        /// <summary>
        /// Get message origin
        /// </summary>
        /// <returns></returns>
        public int GetMsgOrigin()
        {
            return msgOrigin;
        }

        /// <summary>
        /// Validate Message Mac.
        /// Of course it needs a special MAC calculation algorithm that designed by
        /// your system.
        /// </summary>
        /// <param name="isoMacGenerator"></param>
        /// <returns></returns>
        public bool ValidateMac(ISOMacGenerator isoMacGenerator)
        {
            if (!FieldExits(FIELDS.F64_MAC) || GetField(FIELDS.F64_MAC).Length == 0)
            {
                return false;
            }
            byte[] mBody = new byte[GetBody().Length - 8];
            GetBody().CopyTo(mBody, GetBody().Length - 8);
            byte[] oMac = Arrays.CopyOf(GetField(FIELDS.F64_MAC), 8);
            byte[] vMac = isoMacGenerator.Generate(mBody);

            return oMac.SequenceEqual(vMac);
        }

        /// <summary>
        /// Convert message to String
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (message == null)
                message = StringUtil.FromByteArray(msg);
            return message;
        }

        /// <summary>
        /// Convert fields to string
        /// </summary>
        /// <returns></returns>
        public string FieldsToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("\r\n");
            foreach (KeyValuePair<int, byte[]> item in dataElements)
            {
                stringBuilder
                    .Append(FIELDS.ValueOf(item.Key))
                    .Append(" : ")
                    .Append(StringUtil.FromByteArray(item.Value))
                    .Append("\r\n");
            }

            stringBuilder.Append("\r\n");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Clear all content
        /// </summary>
        public void Clear()
        {
            message = null;
            header = null;
            body = null;
            primaryBitmap = null;
        }
    }
}