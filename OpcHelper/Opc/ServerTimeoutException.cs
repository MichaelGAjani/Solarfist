namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Runtime.Serialization;

    public class ServerTimeoutException : ResultIDException
    {
        private const string Default = "The server did not respond within the specified timeout period.";

        public ServerTimeoutException() : base(ResultID.E_TIMEDOUT, "The server did not respond within the specified timeout period.")
        {
        }

        public ServerTimeoutException(Exception e) : base(ResultID.E_TIMEDOUT, "The server did not respond within the specified timeout period.", e)
        {
        }

        public ServerTimeoutException(string message) : base(ResultID.E_TIMEDOUT, "The server did not respond within the specified timeout period.\r\n" + message)
        {
        }

        protected ServerTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ServerTimeoutException(string message, Exception innerException) : base(ResultID.E_TIMEDOUT, "The server did not respond within the specified timeout period.\r\n" + message, innerException)
        {
        }
    }
}

