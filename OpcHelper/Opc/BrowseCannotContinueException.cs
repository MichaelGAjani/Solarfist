namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class BrowseCannotContinueException : ApplicationException
    {
        private const string Default = "The browse operation cannot continue.";

        public BrowseCannotContinueException() : base("The browse operation cannot continue.")
        {
        }

        public BrowseCannotContinueException(string message) : base("The browse operation cannot continue.\r\n" + message)
        {
        }

        protected BrowseCannotContinueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public BrowseCannotContinueException(string message, Exception innerException) : base("The browse operation cannot continue.\r\n" + message, innerException)
        {
        }
    }
}

