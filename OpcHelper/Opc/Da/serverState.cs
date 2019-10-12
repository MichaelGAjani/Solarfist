namespace Jund.OpcHelper.Opc.Da
{
    using System;

    public enum serverState
    {
        unknown,
        running,
        failed,
        noConfig,
        suspended,
        test,
        commFault
    }
}

