namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class AlreadyConnectedException : ApplicationException
    {
        private const string Default = "The remote server is already connected.";

        public AlreadyConnectedException() : base("The remote server is already connected.")
        {
        }

        public AlreadyConnectedException(Exception e) : base("The remote server is already connected.", e)
        {
        }

        public AlreadyConnectedException(string message) : base("The remote server is already connected.\r\n" + message)
        {
        }

        protected AlreadyConnectedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public AlreadyConnectedException(string message, Exception innerException) : base("The remote server is already connected.\r\n" + message, innerException)
        {
        }
    }
}

