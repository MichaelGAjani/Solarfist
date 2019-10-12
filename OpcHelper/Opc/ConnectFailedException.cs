namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ConnectFailedException : ResultIDException
    {
        private const string Default = "Could not connect to server.";

        public ConnectFailedException() : base(ResultID.E_ACCESS_DENIED, "Could not connect to server.")
        {
        }

        public ConnectFailedException(Exception e) : base(ResultID.E_NETWORK_ERROR, "Could not connect to server.", e)
        {
        }

        public ConnectFailedException(string message) : base(ResultID.E_NETWORK_ERROR, "Could not connect to server.\r\n" + message)
        {
        }

        protected ConnectFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ConnectFailedException(string message, Exception innerException) : base(ResultID.E_NETWORK_ERROR, "Could not connect to server.\r\n" + message, innerException)
        {
        }
    }
}

