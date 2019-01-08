using System;
using imohsenb.iso8583.entities;
using imohsenb.iso8583.enums;
using imohsenb.iso8583.interfaces;
using imohsenb.iso8583.utils;

namespace imohsenb.iso8583.builders
{
    public static class ISOMessageBuilder
    {
        public static MessageClass Packer(VERSION version)
        {
            return new Builder(version.GetCode());
        }

        private class Builder : MessageClass
        {

            private readonly string version;

            public Builder(string version)
            {
                this.version = version;
            }

            public MessagePacker<GeneralMessageClassBuilder> Authorization()
            {
                return new GeneralMessageClassBuilder(version, "1");
            }

            public MessagePacker<GeneralMessageClassBuilder> Financial()
            {
                return new GeneralMessageClassBuilder(version, "2");
            }

            public MessagePacker<GeneralMessageClassBuilder> FileAction()
            {
                return new GeneralMessageClassBuilder(version, "3");
            }

            public MessagePacker<GeneralMessageClassBuilder> Reversal()
            {
                return new GeneralMessageClassBuilder(version, "4");
            }

            public MessagePacker<GeneralMessageClassBuilder> Reconciliation()
            {
                return new GeneralMessageClassBuilder(version, "5");
            }

            public MessagePacker<GeneralMessageClassBuilder> Administrative()
            {
                return new GeneralMessageClassBuilder(version, "6");
            }

            public MessagePacker<GeneralMessageClassBuilder> FeeCollection()
            {
                return new GeneralMessageClassBuilder(version, "7");
            }

            public MessagePacker<GeneralMessageClassBuilder> NetworkManagement()
            {
                return new GeneralMessageClassBuilder(version, "8");
            }

        }


        public static UnpackMessage Unpacker()
        {
            return new UnpackBuilder();
        }

        public class UnpackBuilder : UnpackMessage, UnpackMethods
        {

            private byte[] message;


            public UnpackMethods SetMessage(byte[] message)
            {
                this.message = message;
                return this;
            }


            public UnpackMethods SetMessage(string message)
            {
                SetMessage(StringUtil.HexStringToByteArray(message));
                return this;
            }


            public ISOMessage Build()
            {
                ISOMessage finalMessage = new ISOMessage();
                finalMessage.SetMessage(message);
                return finalMessage;
            }
        }

    }
}
