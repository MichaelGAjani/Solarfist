namespace Jund.OpcHelper.OpcRcw.Da
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct OPCSERVERSTATUS
    {
        public OpcRcw.Da.FILETIME ftStartTime;
        public OpcRcw.Da.FILETIME ftCurrentTime;
        public OpcRcw.Da.FILETIME ftLastUpdateTime;
        public OPCSERVERSTATE dwServerState;
        public int dwGroupCount;
        public int dwBandWidth;
        public short wMajorVersion;
        public short wMinorVersion;
        public short wBuildNumber;
        public short wReserved;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szVendorInfo;
    }
}

