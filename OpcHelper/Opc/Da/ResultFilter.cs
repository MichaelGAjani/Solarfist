namespace Jund.OpcHelper.Opc.Da
{
    using System;

    [Flags]
    public enum ResultFilter
    {
        All = 0x3f,
        ClientHandle = 4,
        DiagnosticInfo = 0x20,
        ErrorText = 0x10,
        ItemName = 1,
        ItemPath = 2,
        ItemTime = 8,
        Minimal = 9
    }
}

