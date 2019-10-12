namespace Jund.OpcHelper.Opc.Da
{
    using System;

    [Flags]
    public enum StateMask
    {
        Active = 8,
        All = 0xffff,
        ClientHandle = 2,
        Deadband = 0x80,
        EnableBuffering = 0x200,
        KeepAlive = 0x20,
        Locale = 4,
        Name = 1,
        ReqType = 0x40,
        SamplingRate = 0x100,
        UpdateRate = 0x10
    }
}

