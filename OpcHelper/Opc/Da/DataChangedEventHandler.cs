namespace Jund.OpcHelper.Opc.Da
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void DataChangedEventHandler(object subscriptionHandle, object requestHandle, ItemValueResult[] values);
}

