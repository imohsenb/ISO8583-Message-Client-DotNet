using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imohsenb.iso8583.interfaces
{
    public interface ISOMacGenerator
    {
        byte[] Generate(byte[] data);
    }
}
