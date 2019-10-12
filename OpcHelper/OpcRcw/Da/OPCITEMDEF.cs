namespace Jund.OpcHelper.OpcRcw.Da
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), ComConversionLoss]
    public struct OPCITEMDEF
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szAccessPath;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szItemID;
        public int bActive;
        public int hClient;
        public int dwBlobSize;
        [ComConversionLoss]
        public IntPtr pBlob;
        public short vtRequestedDataType;
        public short wReserved;
    }
}

