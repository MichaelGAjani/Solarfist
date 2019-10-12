namespace Jund.OpcHelper.OpcRcw.Dx
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct ItemIdentifier
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szItemPath;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szItemName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szVersion;
        public int dwReserved;
    }
}

