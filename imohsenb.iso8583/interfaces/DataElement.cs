using imohsenb.iso8583.entities;
using imohsenb.iso8583.enums;
using System;

namespace imohsenb.iso8583.interfaces
{
    public interface DataElement<T> : ProcessCode<T>
    {
        ISOMessage Build();

        DataElement<T> GenerateMac(ISOMacGenerator generator);
        DataElement<T> GenerateMac(Func<byte[], byte[]> generate);

        DataElement<T> SetField(int no, string value);
        DataElement<T> SetField(FIELDS field, string value);
        DataElement<T> SetField(int no, byte[] value);
        DataElement<T> SetField(FIELDS field, byte[] value);
        DataElement<T> SetHeader(string header);


        string GetMessageClass();
        string GetProcessCode();
    }
}
