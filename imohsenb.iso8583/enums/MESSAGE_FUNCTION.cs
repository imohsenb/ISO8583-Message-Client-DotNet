using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imohsenb.iso8583.enums
{
    public sealed class MESSAGE_FUNCTION
    {
        public static MESSAGE_FUNCTION Request = new MESSAGE_FUNCTION("0");
        public static MESSAGE_FUNCTION Advice = new MESSAGE_FUNCTION("2");
        private readonly string _code;

        public MESSAGE_FUNCTION(string code)
        {
            this._code = code;
        }

        public string GetCode()
        {
            return _code;
        }
    }
}
