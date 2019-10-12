namespace Jund.CommunicationHelper
{
    using System;
    using System.Net;

    public class ConnectedEventArgs : ClientEventArgs
    {
        private bool allowed;

        public ConnectedEventArgs(long clientId, IPEndPoint clientEP) : base(clientId, clientEP)
        {
            this.allowed = true;
        }

        public bool Allowed
        {
            get => 
                this.allowed;
            set
            {
                this.allowed = value;
            }
        }
    }
}

