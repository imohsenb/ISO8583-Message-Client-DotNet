using System;
using imohsenb.iso8583.clients;
using imohsenb.iso8583.interfaces;

namespace imohsenb.iso8583.builders
{
    public partial class ISOClientBuilder
    {
        private static ClientBuilder clientBuilder;

        /// <summary>
        /// Create socket for connection to server.
        /// </summary>
        /// <param name="host">Host IP</param>
        /// <param name="port">Host PORT</param>
        /// <returns></returns>
        public static ClientBuilder CreateSocket(String host, int port, bool ssl = false, string sslName = "test")
        {
            clientBuilder = new ClientBuilder(host, port, ssl, sslName);
            return clientBuilder;
        }

        /**
         * ClientBuilder
         */
        public class ClientBuilder
        {
            private IISOClient client;

            /**
             * Create ISO Client after initializing
             * @param host socket Host
             * @param port socket ip
             */
            public ClientBuilder(String host, int port, bool ssl = false, string sslName = "test")
            {
                client = (ssl) ? (new SslIsoClient(sslName)) : (new DefaultISOClient()) as IISOClient;
                client.SetSocketAddress(host, port);
            }

            /**
         * Sending with NIO (false) or Blocking IO (true)
         * @param blocking:true
         * @return {@link ClientBuilder}
         */
            public ClientBuilder ConfigureBlocking(bool blocking)
            {
                client.SetBlocking(blocking);
                return this;
            }


            /// <summary>
            /// Build ISOClient for sending messages
            /// </summary>
            /// <returns></returns>
            public IISOClient Build()
            {
                return client;
            }

            /// <summary>
            /// Set Read timeout 
            /// </summary>
            /// <param name="timeout">Timeout in milisecond</param>
            /// <returns></returns>
            public ClientBuilder SetReadTimeout(int timeout)
            {
                client.SetReadTimeout(timeout);
                return this;
            }

            /// <summary>
            /// Set Connection timeout
            /// </summary>
            /// <param name="timeout">Timeout in milisecond</param>
            /// <returns></returns>
            public ClientBuilder SetConnectionTimeout(int timeout)
            {
                client.SetConnectionTimeout(timeout);
                return this;
            }

            /// <summary>
            /// Number of bytes for message length.
            /// default is 2 for message lenght.
            /// zero value prevent sending message length 
            /// </summary>
            /// <param name="bytes"></param>
            /// <returns></returns>
            public ClientBuilder Length(int bytes)
            {
                client.SetLength(bytes);
                return this;
            }

            /**
             * Set event listener for dispatch events
             * @param eventListener Implementation of {@link ISOClientEventListener}
             * @return {@link ClientBuilder}
             */
            public ClientBuilder SetEventListener(ISOClientEventListener eventListener)
            {
                if (eventListener != null)
                    client.SetEventListener(eventListener);
                return this;
            }
        }
    }
}
