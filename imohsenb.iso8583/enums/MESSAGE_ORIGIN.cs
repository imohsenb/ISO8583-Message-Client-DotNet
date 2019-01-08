using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imohsenb.iso8583.enums
{
    public sealed class MESSAGE_ORIGIN
    {
        public static MESSAGE_ORIGIN Acquirer = new MESSAGE_ORIGIN("0");
        private readonly string _code;

        private MESSAGE_ORIGIN(string code)
        {
            this._code = code;
        }

        public string GetCode()
        {
            return _code;
        }
    }
}
