namespace Jund.OpcHelper.Opc.Ae
{
    using System;

    [Flags]
    public enum ChangeMask
    {
        AcknowledgeState = 2,
        ActiveState = 1,
        Attribute = 0x80,
        EnableState = 4,
        Message = 0x40,
        Quality = 8,
        Severity = 0x10,
        SubCondition = 0x20
    }
}

