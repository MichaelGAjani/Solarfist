namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class AccessDeniedException : ResultIDException
    {
        private const string Default = "The server refused the connection.";

        public AccessDeniedException() : base(ResultID.E_ACCESS_DENIED, "The server refused the connection.")
        {
        }

        public AccessDeniedException(Exception e) : base(ResultID.E_ACCESS_DENIED, "The server refused the connection.", e)
        {
        }

        public AccessDeniedException(string message) : base(ResultID.E_ACCESS_DENIED, "The server refused the connection.\r\n" + message)
        {
        }

        protected AccessDeniedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public AccessDeniedException(string message, Exception innerException) : base(ResultID.E_NETWORK_ERROR, "The server refused the connection.\r\n" + message, innerException)
        {
        }
    }
}

