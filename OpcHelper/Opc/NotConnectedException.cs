namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class NotConnectedException : ApplicationException
    {
        private const string Default = "The remote server is not currently connected.";

        public NotConnectedException() : base("The remote server is not currently connected.")
        {
        }

        public NotConnectedException(Exception e) : base("The remote server is not currently connected.", e)
        {
        }

        public NotConnectedException(string message) : base("The remote server is not currently connected.\r\n" + message)
        {
        }

        protected NotConnectedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NotConnectedException(string message, Exception innerException) : base("The remote server is not currently connected.\r\n" + message, innerException)
        {
        }
    }
}

