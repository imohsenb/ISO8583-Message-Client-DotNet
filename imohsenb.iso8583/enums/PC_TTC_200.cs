using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imohsenb.iso8583.enums
{
    public sealed class PC_TTC_200
    {
        public static PC_TTC_200 Purchase = new PC_TTC_200("00");
        public static PC_TTC_200 Withdrawal = new PC_TTC_200("01");
        public static PC_TTC_200 Void = new PC_TTC_200("02");
        public static PC_TTC_200 Refund_Return = new PC_TTC_200("20");
        public static PC_TTC_200 Payment_Deposit_Refresh = new PC_TTC_200("21");
        public static PC_TTC_200 AccountTransfer = new PC_TTC_200("40");
        public static PC_TTC_200 PurchaseAdvise = new PC_TTC_200("00");
        public static PC_TTC_200 Refund_Return_advise = new PC_TTC_200("20");
        private readonly string _code;

        private PC_TTC_200(string code)
        {
            this._code = code;
        }

        public string GetCode()
        {
            return _code;
        }
    }
}
