namespace Jund.OpcHelper.Opc.Hda
{
    using System;

    [Flags]
    public enum Quality
    {
        Calculated = 0x80000,
        Conversion = 0x800000,
        DataLost = 0x400000,
        ExtraData = 0x10000,
        Interpolated = 0x20000,
        NoBound = 0x100000,
        NoData = 0x200000,
        Partial = 0x1000000,
        Raw = 0x40000
    }
}

