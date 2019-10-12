namespace Jund.OpcHelper.Opc.Ae
{
    using System;

    public enum ServerState
    {
        Unknown,
        Running,
        Failed,
        NoConfig,
        Suspended,
        Test,
        CommFault
    }
}

