using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imohsenb.iso8583.enums
{
    public sealed class PC_TTC_100
    {
        public static PC_TTC_100 Authorization = new PC_TTC_100("00");
        public static PC_TTC_100 AuthorizationVoid = new PC_TTC_100("02");
        public static PC_TTC_100 Refund_Return = new PC_TTC_100("20");
        public static PC_TTC_100 Refund_Return_void = new PC_TTC_100("22");
        public static PC_TTC_100 BalanceInquiry = new PC_TTC_100("30");

        private readonly string _code;

        private PC_TTC_100(string code)
        {
            this._code = code;
        }

        public string GetCode()
        {
            return _code;
        }
    }
}
