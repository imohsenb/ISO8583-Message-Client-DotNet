using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imohsenb.iso8583.enums
{
    public sealed class PC_ATC
    {
        public static PC_ATC Default = new PC_ATC("00");
        public static PC_ATC SavingAccount = new PC_ATC("10");
        public static PC_ATC CheckingAccount = new PC_ATC("20");
        public static PC_ATC CreditCardAccount = new PC_ATC("30");

        private readonly string code;

        private PC_ATC(string code)
        {
            this.code = code;
        }
        public string GetCode()
        {
            return code;
        }
    }
}
