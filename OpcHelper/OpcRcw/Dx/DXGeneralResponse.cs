namespace Jund.OpcHelper.OpcRcw.Dx
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), ComConversionLoss]
    public struct DXGeneralResponse
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szConfigurationVersion;
        public int dwCount;
        [ComConversionLoss]
        public IntPtr pIdentifiedResults;
        public int dwReserved;
    }
}

