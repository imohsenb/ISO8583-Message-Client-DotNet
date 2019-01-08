using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imohsenb.iso8583.enums
{
    public sealed class VERSION
    {
        public static VERSION V1987 = new VERSION("0");
        public static VERSION V1993 = new VERSION("1");
        public static VERSION V2003 = new VERSION("2");
        private readonly string _code;

        private VERSION(string versionCode)
        {
            this._code = versionCode;
        }

        public string GetCode()
        {
            return _code;
        }
    }
}
