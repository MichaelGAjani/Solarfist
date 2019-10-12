namespace Jund.OpcHelper.OpcRcw.Da
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), ComConversionLoss]
    public struct OPCITEMPROPERTIES
    {
        public int hrErrorID;
        public int dwNumProperties;
        [ComConversionLoss]
        public IntPtr pItemProperties;
        public int dwReserved;
    }
}

