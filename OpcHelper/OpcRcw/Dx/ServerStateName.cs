namespace Jund.OpcHelper.OpcRcw.Dx
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class ServerStateName
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_STATE_COMM_FAULT = "commFault";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_STATE_FAILED = "failed";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_STATE_NOCONFIG = "noConfig";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_STATE_RUNNING = "running";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_STATE_SUSPENDED = "suspended";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_STATE_TEST = "test";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_STATE_UNKNOWN = "unknown";
    }
}

