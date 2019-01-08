using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imohsenb.iso8583.interfaces
{
    public interface ISOMacGeneratorAsync
    {
       Task<byte[]> Generate(byte[] data);
    }
}
