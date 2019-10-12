namespace Jund.OpcHelper.OpcRcw.Dx
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), ComConversionLoss]
    public struct DXConnection
    {
        public uint dwMask;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szItemPath;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szItemName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szVersion;
        public int dwBrowsePathCount;
        [ComConversionLoss]
        public IntPtr pszBrowsePaths;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szDescription;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szKeyword;
        public int bDefaultSourceItemConnected;
        public int bDefaultTargetItemConnected;
        public int bDefaultOverridden;
        [MarshalAs(UnmanagedType.Struct)]
        public object vDefaultOverrideValue;
        [MarshalAs(UnmanagedType.Struct)]
        public object vSubstituteValue;
        public int bEnableSubstituteValue;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szTargetItemPath;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szTargetItemName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szSourceServerName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szSourceItemPath;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szSourceItemName;
        public int dwSourceItemQueueSize;
        public int dwUpdateRate;
        public float fltDeadBand;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szVendorData;
    }
}

