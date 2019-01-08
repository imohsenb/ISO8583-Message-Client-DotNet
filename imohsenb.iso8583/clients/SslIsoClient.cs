using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using imohsenb.iso8583.builders;
using imohsenb.iso8583.entities;
using imohsenb.iso8583.interfaces;
using imohsenb.iso8583.utils;

namespace imohsenb.iso8583.clients
{
    public class SslIsoClient : IISOClient
    {
        #region Variables

        private static Hashtable certificateErrors = new Hashtable();

        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        private string _host;
        private int _port;
        private static TcpClient _client;
        private static string response = String.Empty;
        private static int _length = 0;
        private int _timeout = 60000;
        private int _connectionTimeout = 10000;
        private static SslStream sslStream;
        private string sslName = "test";

        #endregion

        public SslIsoClient(string sslName = "test")
        {
            this.sslName = sslName;
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

            _client = new TcpClient(_host, _port);
            
            _client.ReceiveTimeout = _timeout;
            _client.SendTimeout = _timeout;

            if (!_client.Connected)
            { 
                _client.Close();
                throw new IOException("Failed to connect server.");
            }

            Console.WriteLine("Connected.");

            sslStream = new SslStream(
                _client.GetStream(),
                false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate),
                null
                );
            // The server name must match the name on the server certificate.
            try
            {
                sslStream.AuthenticateAsClient(sslName);
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                }
                Console.WriteLine("Authentication failed - closing the connection.");
                _client.Close();
                throw new IOException(e.Message);
            }
        }

        public void Disconnect()
        {
            _client.Close();
        }

        public bool IsClosed()
        {
            return !_client.Connected;
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

                Receive();
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

        private static void Receive()
        {            
            try
            {
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = sslStream;

                // Begin receiving the data from the remote device.  
                sslStream.BeginRead(state.buffer, 0, StateObject.BufferSize, 
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
                SslStream client = state.workSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndRead(ar);

                Console.WriteLine($"Read : {bytesRead}");

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
                        client.BeginRead(state.buffer, 0, StateObject.BufferSize,
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
            try
            {
                sslStream.BeginWrite(byteData, 0, byteData.Length, new AsyncCallback(SendCallback), sslStream);
                sslStream.Flush();
                Console.WriteLine("Message was sent.");
            }catch(Exception e)
            {
                Console.WriteLine($"Error was occured in Sending : {e.Message} ");
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                SslStream client = (SslStream)ar.AsyncState;

                // Complete sending the data to the remote device.  
                client.EndWrite(ar);
                Console.WriteLine("Sent to server.");

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

            buffer.append(isoMessage.GetHeader())
                .append(isoMessage.GetBody());

            return buffer.array();
        }

        public void Dispose()
        {
            _client = null;
        }

        public class StateObject
        {
            // Client socket.  
            public SslStream workSocket = null;
            // Size of receive buffer.  
            public const int BufferSize = 256;
            // Receive buffer.  
            public byte[] buffer = new byte[BufferSize];
            // Received data string.  
            public StringBuilder sb = new StringBuilder();
        }

        // The following method is invoked by the RemoteCertificateValidationDelegate.
        public static bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {


            if (
                sslPolicyErrors == SslPolicyErrors.None 
                //|| sslPolicyErrors == (SslPolicyErrors.RemoteCertificateNameMismatch)
                //|| sslPolicyErrors == (SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateChainErrors)
                )
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);
            
            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }
    }
}