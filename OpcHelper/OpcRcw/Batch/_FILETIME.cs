namespace Jund.OpcHelper.OpcRcw.Batch
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct _FILETIME
    {
        public int dwLowDateTime;
        public int dwHighDateTime;
    }
}

