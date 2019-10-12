namespace Jund.OpcHelper.Opc.Cpx
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidDataToWriteException : ApplicationException
    {
        private const string Default = "The object cannot be written because it is not consistent with the schema.";

        public InvalidDataToWriteException() : base("The object cannot be written because it is not consistent with the schema.")
        {
        }

        public InvalidDataToWriteException(Exception e) : base("The object cannot be written because it is not consistent with the schema.", e)
        {
        }

        public InvalidDataToWriteException(string message) : base("The object cannot be written because it is not consistent with the schema.\r\n" + message)
        {
        }

        protected InvalidDataToWriteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidDataToWriteException(string message, Exception innerException) : base("The object cannot be written because it is not consistent with the schema.\r\n" + message, innerException)
        {
        }
    }
}

