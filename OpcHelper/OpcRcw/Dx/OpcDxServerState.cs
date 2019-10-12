namespace Jund.OpcHelper.OpcRcw.Dx
{
    using System;

    public enum OpcDxServerState
    {
        COMM_FAULT = 6,
        FAILED = 2,
        NOCONFIG = 3,
        RUNNING = 1,
        SUSPENDED = 4,
        TEST = 5,
        UNKNOWN = 7
    }
}

