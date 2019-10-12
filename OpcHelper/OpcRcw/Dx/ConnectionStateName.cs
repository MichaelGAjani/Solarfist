namespace Jund.OpcHelper.OpcRcw.Dx
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class ConnectionStateName
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECTION_STATE_DEACTIVATED = "deactivated";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECTION_STATE_INITIALIZING = "initializing";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECTION_STATE_OPERATIONAL = "operational";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECTION_STATE_SOURCE_SERVER_NOT_CONNECTED = "sourceServerNotConnected";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECTION_STATE_SUBSCRIPTION_FAILED = "subscriptionFailed";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECTION_STATE_TARGET_ITEM_NOT_FOUND = "targetItemNotFound";
    }
}

