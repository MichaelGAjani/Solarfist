namespace Jund.OpcHelper.OpcRcw.Ae
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct OPCEVENTSERVERSTATUS
    {
        public OpcRcw.Ae.FILETIME ftStartTime;
        public OpcRcw.Ae.FILETIME ftCurrentTime;
        public OpcRcw.Ae.FILETIME ftLastUpdateTime;
        public OPCEVENTSERVERSTATE dwServerState;
        public short wMajorVersion;
        public short wMinorVersion;
        public short wBuildNumber;
        public short wReserved;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szVendorInfo;
    }
}

