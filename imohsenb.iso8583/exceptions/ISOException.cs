using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imohsenb.iso8583.exceptions
{
    public class ISOException : Exception
    {
        public ISOException(string message) : base(message)
        {}

        public ISOException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
