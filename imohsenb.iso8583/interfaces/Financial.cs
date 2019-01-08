using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imohsenb.iso8583.interfaces
{
    public interface Financial<T>
    {
        DataElement<T> setAmount(decimal amount);
    }
}
