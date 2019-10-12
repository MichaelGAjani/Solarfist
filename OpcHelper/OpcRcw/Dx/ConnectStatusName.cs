namespace Jund.OpcHelper.OpcRcw.Dx
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class ConnectStatusName
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECT_STATUS_CONNECTED = "connected";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECT_STATUS_CONNECTING = "connecting";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECT_STATUS_DISCONNECTED = "disconnected";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECT_STATUS_FAILED = "failed";
    }
}

