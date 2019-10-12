namespace Jund.OpcHelper.OpcRcw.Da
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct OPCBROWSEELEMENT
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szItemID;
        public int dwFlagValue;
        public int dwReserved;
        public OPCITEMPROPERTIES ItemProperties;
    }
}

