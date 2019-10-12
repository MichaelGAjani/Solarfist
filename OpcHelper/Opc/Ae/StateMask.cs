namespace Jund.OpcHelper.Opc.Ae
{
    using System;

    [Flags]
    public enum StateMask
    {
        Active = 4,
        All = 0xffff,
        BufferTime = 8,
        ClientHandle = 2,
        KeepAlive = 0x20,
        MaxSize = 0x10,
        Name = 1
    }
}

