namespace Jund.OpcHelper.OpcRcw.Dx
{
    using System;

    public enum DXConnectionState
    {
        DEACTIVATED = 3,
        INITIALIZING = 1,
        OPERATIONAL = 2,
        SOURCE_SERVER_NOT_CONNECTED = 4,
        SUBSCRIPTION_FAILED = 5,
        TARGET_ITEM_NOT_FOUND = 6
    }
}

