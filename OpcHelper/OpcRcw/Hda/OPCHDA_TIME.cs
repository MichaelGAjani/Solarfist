namespace Jund.OpcHelper.OpcRcw.Hda
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct OPCHDA_TIME
    {
        public int bString;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szTime;
        public OPCHDA_FILETIME ftTime;
    }
}

