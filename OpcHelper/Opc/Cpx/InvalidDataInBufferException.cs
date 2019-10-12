namespace Jund.OpcHelper.Opc.Cpx
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidDataInBufferException : ApplicationException
    {
        private const string Default = "The data in the buffer cannot be read because it is not consistent with the schema.";

        public InvalidDataInBufferException() : base("The data in the buffer cannot be read because it is not consistent with the schema.")
        {
        }

        public InvalidDataInBufferException(Exception e) : base("The data in the buffer cannot be read because it is not consistent with the schema.", e)
        {
        }

        public InvalidDataInBufferException(string message) : base("The data in the buffer cannot be read because it is not consistent with the schema.\r\n" + message)
        {
        }

        protected InvalidDataInBufferException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidDataInBufferException(string message, Exception innerException) : base("The data in the buffer cannot be read because it is not consistent with the schema.\r\n" + message, innerException)
        {
        }
    }
}

