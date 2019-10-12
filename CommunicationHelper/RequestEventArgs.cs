namespace Jund.CommunicationHelper
{
    using System;
    using System.Collections;
    using System.Net;
    using System.Runtime.CompilerServices;

    public class RequestEventArgs : ClientEventArgs
    {
        private Jund.CommunicationHelper.Request request;
        private Hashtable responseData;
        private string responseMessage;

        public RequestEventArgs(long clientId, IPEndPoint clientEP, Jund.CommunicationHelper.Request request) : base(clientId, clientEP)
        {
            this.responseMessage = string.Empty;
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            this.request = request;
        }

        public bool CloseAfterResponse { get; set; }

        public Jund.CommunicationHelper.Request Request =>
            this.request;

        public Hashtable ResponseData
        {
            get
            {
                if (this.responseData == null)
                {
                    this.responseData = new Hashtable();
                }
                return this.responseData;
            }
        }

        public string ResponseMessage
        {
            get => 
                (this.responseMessage ?? string.Empty);
            set
            {
                this.responseMessage = value ?? string.Empty;
            }
        }

        public int ResponseStatus { get; set; }
    }
}

