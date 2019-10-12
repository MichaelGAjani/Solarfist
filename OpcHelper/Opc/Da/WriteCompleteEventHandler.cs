namespace Jund.OpcHelper.Opc.Da
{
    using Opc;
    using System;
    using System.Runtime.CompilerServices;

    public delegate void WriteCompleteEventHandler(object requestHandle, IdentifiedResult[] results);
}

