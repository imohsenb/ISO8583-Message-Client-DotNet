using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using imohsenb.iso8583.builders;
using imohsenb.iso8583.entities;
using imohsenb.iso8583.interfaces;
using imohsenb.iso8583.utils;

namespace imohsenb.iso8583.clients
{
    public class DefaultISOClient : IISOClient
    {
        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        private string _host;
        private int _port;
        private static Socket _client;
        private static string response = String.Empty;
        private static int _length = 0;
        private int _timeout = 60000;
        private int _connectionTimeout = 10000;

        public DefaultISOClient()
        {
        }

        private void Init()
        {
            response = String.Empty;
            sendDone.Reset();
            receiveDone.Reset();
            connectDone.Reset();
        }

        public void Connect()
        {
            Init();

            IPAddress ipAddress = IPAddress.Parse(_host);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, _port);

            _client = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            _client.ReceiveTimeout = _timeout;
            _client.SendTimeout = _timeout;

            IAsyncResult result = _client.BeginConnect(remoteEP,
                new AsyncCallback(ConnectCallback), _client);

            connectDone.WaitOne(_connectionTimeout, true);

            if (!_client.Connected)
            { 
                _client.Close();
                throw new IOException("Failed to connect server.");
            }
        }

        public void Disconnect()
        {
            _client.Shutdown(SocketShutdown.Both);
            _client.Close();
        }

        public bool IsClosed()
        {
            return !_client.IsBound;
        }

        public bool IsConnected()
        {
            return _client.Connected;
        }

        public ISOMessage SendMessageSync(ISOMessage isoMessage)
        {
            if(_client == null || !_client.Connected)
                Connect();

            if (_client.Connected)
            {
                Send(isoMessage);
                sendDone.WaitOne();

                Receive(_client);
                receiveDone.WaitOne();


                byte[] rData = Encoding.ASCII.GetBytes(response);

                if (_client.Connected)
                    Disconnect();

                if(response.Length == 0)
                {
                    throw new IOException("Remote server did not respond!");
                }

                return ISOMessageBuilder.Unpacker().SetMessage(response).Build();
            }
            else
            {
                throw new IOException("Client not connected!");
            }
        }

        public void SetEventListener(ISOClientEventListener isoClientEventListener)
        {}

        public void SetBlocking(bool blocking)
        {}

        public void SetLength(int bytes)
        {}

        public void SetReadTimeout(int timeout)
        {
            _timeout = timeout;
        }

        public void SetConnectionTimeout(int timeout)
        {
            _connectionTimeout = timeout;
        }

        public void SetSocketAddress(string host, int port)
        {
            _host = host;
            _port = port;
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());                    
            }
        }

        private static void Receive(Socket client)
        {
            try
            {
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket   
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    string rData = StringUtil.FromByteArray(state.buffer);

                    Console.WriteLine(rData);
                    state.sb.Append(rData);

                    if (bytesRead < StateObject.BufferSize)
                    {
                        response = state.sb.ToString();
                        receiveDone.Set();
                    }
                    else
                    {
                        // Get the rest of the data.  
                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                            new AsyncCallback(ReceiveCallback), state);
                    }
                    
                }
                else
                {
                    // All the data has arrived; put it in response.  
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    // Signal that all bytes have been received.  
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                receiveDone.Set();
            }
        }

        private static void Send(ISOMessage msg)
        {
            byte[] byteData = InitBuffer(msg);
            // Begin sending the data to the remote device.  
            _client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), _client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static byte[] InitBuffer(ISOMessage isoMessage)
        {
            int len = isoMessage.GetBody().Length + isoMessage.GetHeader().Length;

            ByteArray buffer = new ByteArray();

            if (_length > 0)
            {
//                    byte[] mlen = ByteBuffer.allocate(4).putInt(len).array();
//                    buffer.put(Arrays.copyOfRange(mlen, 2, 4));
            }

            buffer.append(isoMessage.GetHeader())
                .append(isoMessage.GetBody());

            return buffer.array();
        }

        public void Dispose()
        {                
            _client.Dispose();
        }


        public class StateObject
        {
            // Client socket.  
            public Socket workSocket = null;
            // Size of receive buffer.  
            public const int BufferSize = 256;
            // Receive buffer.  
            public byte[] buffer = new byte[BufferSize];
            // Received data string.  
            public StringBuilder sb = new StringBuilder();
        }
    }
}