namespace Jund.CommunicationHelper
{
    using System;
    using System.Net;

    public class ClientEventArgs : EventArgs
    {
        private IPEndPoint client;
        private long clientId;

        public ClientEventArgs(long clientId, IPEndPoint clientEP)
        {
            if (clientId <= 0L)
            {
                throw new ArgumentOutOfRangeException("clientId");
            }
            if (clientEP == null)
            {
                throw new ArgumentNullException("clientEP");
            }
            this.clientId = clientId;
            this.client = clientEP;
        }

        public IPEndPoint ClientAddress =>
            this.client;

        public long ClientId =>
            this.clientId;
    }
}

