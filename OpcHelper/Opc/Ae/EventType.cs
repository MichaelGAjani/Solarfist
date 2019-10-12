namespace Jund.OpcHelper.Opc.Ae
{
    using System;

    [Flags]
    public enum EventType
    {
        All = 0xffff,
        Condition = 4,
        Simple = 1,
        Tracking = 2
    }
}

