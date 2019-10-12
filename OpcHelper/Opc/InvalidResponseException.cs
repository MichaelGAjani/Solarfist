namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidResponseException : ApplicationException
    {
        private const string Default = "The response from the server was invalid or incomplete.";

        public InvalidResponseException() : base("The response from the server was invalid or incomplete.")
        {
        }

        public InvalidResponseException(Exception e) : base("The response from the server was invalid or incomplete.", e)
        {
        }

        public InvalidResponseException(string message) : base("The response from the server was invalid or incomplete.\r\n" + message)
        {
        }

        protected InvalidResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidResponseException(string message, Exception innerException) : base("The response from the server was invalid or incomplete.\r\n" + message, innerException)
        {
        }
    }
}

