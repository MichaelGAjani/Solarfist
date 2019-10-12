namespace Jund.OpcHelper.Opc.Ae
{
    using System;

    [Flags]
    public enum FilterType
    {
        All = 0xffff,
        Area = 8,
        Category = 2,
        Event = 1,
        Severity = 4,
        Source = 0x10
    }
}

