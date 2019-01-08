using imohsenb.iso8583.enums;

namespace imohsenb.iso8583.interfaces
{
    public interface MessagePacker<T>
    {
        DataElement<T> MTI(MESSAGE_FUNCTION mFunction, MESSAGE_ORIGIN mOrigin);
        MessagePacker<T> SetLeftPadding(char character);
        MessagePacker<T> SetRightPadding(char character);
    }
}
