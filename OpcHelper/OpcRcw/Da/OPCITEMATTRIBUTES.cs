namespace Jund.OpcHelper.OpcRcw.Da
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), ComConversionLoss]
    public struct OPCITEMATTRIBUTES
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szAccessPath;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szItemID;
        public int bActive;
        public int hClient;
        public int hServer;
        public int dwAccessRights;
        public int dwBlobSize;
        [ComConversionLoss]
        public IntPtr pBlob;
        public short vtRequestedDataType;
        public short vtCanonicalDataType;
        public OPCEUTYPE dwEUType;
        [MarshalAs(UnmanagedType.Struct)]
        public object vEUInfo;
    }
}

