using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imohsenb.iso8583.interfaces
{
    public interface UnpackMessage
    {

        UnpackMethods SetMessage(byte[] message);
        UnpackMethods SetMessage(string message);

    }

}
