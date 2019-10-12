namespace Jund.OpcHelper.OpcRcw.Hda
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), ComConversionLoss]
    public struct OPCHDA_ITEM
    {
        public int hClient;
        public int haAggregate;
        public int dwCount;
        [ComConversionLoss]
        public IntPtr pftTimeStamps;
        [ComConversionLoss]
        public IntPtr pdwQualities;
        [ComConversionLoss]
        public IntPtr pvDataValues;
    }
}

