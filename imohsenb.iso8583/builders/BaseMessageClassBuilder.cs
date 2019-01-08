using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using imohsenb.iso8583.entities;
using imohsenb.iso8583.enums;
using imohsenb.iso8583.exceptions;
using imohsenb.iso8583.interfaces;
using imohsenb.iso8583.utils;

namespace imohsenb.iso8583.builders
{
    public class BaseMessageClassBuilder<T> : DataElement<T>,  MessagePacker<T>
    {

        #region Variables
        private string version;
        private string messageClass = "0";
        private string messageFunction = "0";
        private string messageOrigin = "0";
        private string processCode;
        private string header;
        private char paddingCharacter = 'F';
        private bool leftPadding = false;
        private SortedDictionary<int, byte[]> dataElements = new SortedDictionary<int, byte[]>();
        #endregion

        public BaseMessageClassBuilder(string version, string messageClass)
        {
            this.version = version;
            this.messageClass = messageClass;
        }

        #region BuildBuffer
        public ISOMessage Build()
        {

            ISOMessage finalMessage = new ISOMessage();
            finalMessage.SetMessage(BuildBuffer(true), this.header != null);

            return finalMessage;
        }
        #endregion

        private byte[] BuildBuffer(bool generateBitmap)
        {
            BitArray primaryBitmap = new BitArray(64);
            ByteArray dataBuffer = new ByteArray();

            foreach (KeyValuePair<int, byte[]> element in dataElements)
            {
                if (generateBitmap)
                    primaryBitmap.Set(element.Key - 1, !primaryBitmap.Get(element.Key - 1));
                dataBuffer.append(element.Value);
            }

            if (generateBitmap)
                dataBuffer.prepend(StringUtil.HexStringToByteArray(StringUtil.BitArrayToHexString(primaryBitmap)));

            dataBuffer.prepend(
                StringUtil.HexStringToByteArray((version + messageClass + messageFunction + messageOrigin)));

            if (header != null && generateBitmap)
                dataBuffer.prepend(StringUtil.HexStringToByteArray(header));

            return dataBuffer.array();

        }

        public DataElement<T> SetHeader(string header)
        {
            this.header = header;
            return this;
        }

        public DataElement<T> SetField(int no, byte[] value)
        {
            SetField(FIELDS.ValueOf(no), value);
            return this;
        }

        public DataElement<T> SetField(FIELDS field, byte[] value)
        {

            byte[] fValue = value;

            if (value == null)
                throw new ISOException(field + " is Null");
            //Length check and padding
            if (field.IsFixed())
            {
                if (field.GetLength()%2 != 0)
                {
                    if (field.getType().Equals("n"))
                    {
//                        byte[] _fixed = new byte[(int) Math.Ceiling(field.GetLength()/2f)*2];
//
//                        for (int i = 0; i<fValue.Length; i++) {
//                            _fixed[i] = (byte) ((fValue[i] & 0x0F) << 4);
//                            if (i + 1 < value.Length)
//                                _fixed[i] += Convert.ToByte((fValue[i + 1] & 0xF0) >> 4);
//                        }
//
//                        _fixed[fValue.Length - 1] = (byte) (_fixed[fValue.Length - 1] + 0x0F);
//                        fValue = _fixed;
                    }
                }
                else if (field.GetLength() - (fValue.Length*2) > 0 && field.getType().Equals("n"))
                {

                    ByteArray valueBuffer = new ByteArray();
                    valueBuffer.append(fValue);
                    valueBuffer.prepend(
                        Encoding.ASCII.GetBytes(new char[(field.GetLength() - (fValue.Length*2))/2])
                    );
                    fValue = valueBuffer.array();
                    valueBuffer.clear();
                }

                if (fValue.Length > field.GetLength())
                {
                    fValue = Arrays.CopyOfRange(fValue, fValue.Length - field.GetLength(), fValue.Length);
                }

            }
            else
            {

                int dLen = fValue.Length;
                switch (field.getType())
                {
                    case "z":
                        if (dLen > field.GetLength())
                            fValue = Arrays.CopyOfRange(fValue, fValue.Length - field.GetLength(), fValue.Length);

                        dLen = fValue.Length * 2;

                        break;
                }

                ByteArray valueBuffer = new ByteArray();
                valueBuffer.append(fValue);

                switch (field.GetFormat())
                {
                    case "LL":
                        if (2 - dLen.ToString().Length <= 0)
                        {
                            valueBuffer.prepend(StringUtil.HexStringToByteArray(dLen + ""));
                        }
                        else
                        {
                            var eLen = (new String('0', (2 - dLen.ToString().Length) + dLen));
                            valueBuffer.prepend(
                                StringUtil.HexStringToByteArray(
                                    eLen.Substring(eLen.Length - 2, 2)
                                ));

                        }
                        break;
                    case "LLL":
                    {
                        var eLen = '0' + new String('0', (4 - dLen.ToString().Length)) + dLen;

                        valueBuffer.prepend(
                            StringUtil.HexStringToByteArray(
                                eLen.Substring(eLen.Length - 4, 4)
                            ));

                    }
                        break;
                }

                fValue = valueBuffer.array();


                valueBuffer.clear();
                valueBuffer = null;
            }

            AddOrUpdateElement(field, fValue);

            return this;
        }

        public DataElement<T> SetField(int no, string value)
        {
            SetField(FIELDS.ValueOf(no), value);
            return this;
        }

        public DataElement<T> SetField(FIELDS field, string value)
        {
            if(value != null)
                switch (field.getType())
                {
                    case "n":
                        SetField(field, StringUtil.HexStringToByteArray(value));
                        break;
                    default:
                        SetField(field, Encoding.Default.GetBytes(value));
                        break;
                }

            return this;
        }

        private void AddOrUpdateElement(FIELDS field, byte[] fValue)
        {
            if (dataElements.ContainsKey(field.GetNo()))
            {
                dataElements[field.GetNo()] = fValue;
            }
            else
            {
                dataElements.Add(field.GetNo(), fValue);
            }
        }

        #region MAC-Methods
        public DataElement<T> GenerateMac(ISOMacGenerator generator)
        {

            if (generator != null)
            {
                byte[] mac = generator.Generate(BuildBuffer(true));
                if (mac != null)
                    SetField(FIELDS.F64_MAC, mac);
                else
                    throw new ISOException("MAC is null");
            }

            return this;
        }

        public DataElement<T> GenerateMac(Func<byte[], byte[]> generate)
        {
            byte[] mac = generate(BuildBuffer(true));

            if (mac != null)
                SetField(FIELDS.F64_MAC, mac);
            else
                throw new ISOException("MAC is null");

            return this;
        }
        #endregion

        public DataElement<T> MTI(MESSAGE_FUNCTION mFunction, MESSAGE_ORIGIN mOrigin)
        {
            this.messageFunction = mFunction.GetCode();
            this.messageOrigin = mOrigin.GetCode();
            return this;
        }


        public MessagePacker<T> SetLeftPadding(char character)
        {
            this.leftPadding = true;
            this.paddingCharacter = character;
            return this;
        }


        public MessagePacker<T> SetRightPadding(char character)
        {
            this.leftPadding = false;
            this.paddingCharacter = character;
            return this;
        }

        #region ProcessCodeMethods
        public DataElement<T> ProcessCode(string code)
        {
            this.processCode = code;
            this.SetField(FIELDS.F3_ProcessCode, this.processCode);
            return this;
        }

        public DataElement<T> ProcessCode(PC_TTC_100 ttc)
        {
            this.processCode = ttc.GetCode() + PC_ATC.Default.GetCode() + PC_ATC.Default.GetCode();
            this.SetField(FIELDS.F3_ProcessCode, this.processCode);
            return this;
        }

        public DataElement<T> ProcessCode(PC_TTC_100 ttc, PC_ATC atcFrom, PC_ATC atcTo)
        {
            this.processCode = ttc.GetCode() + atcFrom.GetCode() + atcTo.GetCode();
            this.SetField(FIELDS.F3_ProcessCode, this.processCode);
            return this;
        }

        public DataElement<T> ProcessCode(PC_TTC_200 ttc)
        {
            this.processCode = ttc.GetCode() + PC_ATC.Default.GetCode() + PC_ATC.Default.GetCode();
            this.SetField(FIELDS.F3_ProcessCode, this.processCode);
            return this;
        }

        public DataElement<T> ProcessCode(PC_TTC_200 ttc, PC_ATC atcFrom, PC_ATC atcTo)
        {
            this.processCode = ttc.GetCode() + atcFrom.GetCode() + atcTo.GetCode();
            this.SetField(FIELDS.F3_ProcessCode, this.processCode);
            return this;
        }
        #endregion

        #region Accessors
        public string GetMessageClass()
        {
            return messageClass;
        }

        public string GetProcessCode()
        {
            return processCode;
        }
        #endregion
    }
}
