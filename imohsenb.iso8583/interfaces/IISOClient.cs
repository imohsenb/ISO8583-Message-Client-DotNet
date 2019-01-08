using imohsenb.iso8583.entities;
using System;

namespace imohsenb.iso8583.interfaces
{
    /// <summary>
    /// Iso client for interacting with server.
    /// </summary>
    public interface IISOClient : IDisposable
    {
        /// <summary>
        /// Connect to server
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnect from server.S
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Send Message to server synchronously.
        /// Connection established automatically If client does not connected to server.
        /// </summary>
        /// <param name="isoMessage"></param>
        /// <exception cref="IOException">Throws Exception if server did not respond or in connection failure</exception>
        /// <returns></returns>
        ISOMessage SendMessageSync(ISOMessage isoMessage);

        /// <summary>
        /// Check client is already connected to server
        /// </summary>
        /// <returns></returns>
        bool IsConnected();
        void SetSocketAddress(string host, int port);

        /// <summary>
        /// Check client connection was closed
        /// </summary>
        /// <returns></returns>
        bool IsClosed();

        /// <summary>
        /// Set event listener
        /// </summary>
        /// <param name="isoClientEventListener"></param>
        void SetEventListener(ISOClientEventListener isoClientEventListener);
        void SetBlocking(bool blocking);
        void SetReadTimeout(int timeout);
        void SetConnectionTimeout(int timeout);
        void SetLength(int bytes);
    }
}
