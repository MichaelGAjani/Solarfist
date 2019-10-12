namespace Jund.CommunicationHelper
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class Response
    {
        private Hashtable data;
        private string message = string.Empty;
        private Guid requestId;
        private DateTime time = DateTime.Now;

        public Response(Guid requestId)
        {
            this.requestId = requestId;
        }

        internal void SetData(Hashtable data)
        {
            this.data = data;
        }

        public Hashtable Data
        {
            get
            {
                if (this.data == null)
                {
                    this.data = new Hashtable();
                }
                return this.data;
            }
        }

        public string Message
        {
            get => 
                (this.message ?? string.Empty);
            set
            {
                this.message = value ?? string.Empty;
            }
        }

        public Guid RequestId =>
            this.requestId;

        public int Status { get; set; }

        public DateTime Time =>
            this.time;
    }
}

