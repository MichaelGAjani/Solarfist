namespace Jund.CommunicationHelper
{
    using System;
    using System.Runtime.CompilerServices;

    public class ProgressEventArgs
    {
        public int BytesLoaded { get; set; }

        public int BytesTotal { get; set; }

        public int ProgressValue
        {
            get
            {
                if ((this.BytesTotal != 0) && (this.BytesLoaded < this.BytesTotal))
                {
                    return Convert.ToInt32((double) ((this.BytesLoaded * 100.0) / ((double) this.BytesTotal)));
                }
                return 100;
            }
        }

        public Guid RequestId { get; set; }
    }
}

