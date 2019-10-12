namespace Jund.CommunicationHelper
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class Request
    {
        private Guid id = Guid.NewGuid();
        private Hashtable parameters;
        private string source = string.Empty;
        private DateTime time = DateTime.Now;

        public int Command { get; set; }

        public Guid Id =>
            this.id;

        public Hashtable Parameters
        {
            get
            {
                if (this.parameters == null)
                {
                    this.parameters = new Hashtable();
                }
                return this.parameters;
            }
        }

        public string Source
        {
            get => 
                (this.source ?? string.Empty);
            set
            {
                this.source = value ?? string.Empty;
            }
        }

        public DateTime Time =>
            this.time;
    }
}

