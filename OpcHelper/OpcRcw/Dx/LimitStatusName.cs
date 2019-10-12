namespace Jund.OpcHelper.OpcRcw.Dx
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class LimitStatusName
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string LIMIT_CONSTANT = "constant";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string LIMIT_HIGH = "high";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string LIMIT_LOW = "low";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string LIMIT_NONE = "none";
    }
}

