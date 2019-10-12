namespace Jund.OpcHelper.OpcRcw.Dx
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct SourceServer
    {
        public uint dwMask;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szItemPath;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szItemName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szVersion;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szDescription;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szServerType;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szServerURL;
        public int bDefaultSourceServerConnected;
        public int dwReserved;
    }
}

