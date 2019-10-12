namespace Jund.OpcHelper.OpcRcw.Hda
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct OPCHDA_FILETIME
    {
        public int dwLowDateTime;
        public int dwHighDateTime;
    }
}

