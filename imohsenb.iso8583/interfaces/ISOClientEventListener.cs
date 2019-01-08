using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imohsenb.iso8583.interfaces
{
    public interface ISOClientEventListener
    {
        void Connecting();
        void Connected();
        void ConnectionFailed();
        void ConnectionClosed();
        void Disconnected();
        void BeforeSendingMessage();
        void AfterSendingMessage();
        void OnReceiveData();
        void BeforeReceiveResponse();
        void AfterReceiveResponse();
    }
}
