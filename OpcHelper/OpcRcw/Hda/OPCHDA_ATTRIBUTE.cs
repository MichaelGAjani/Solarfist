namespace Jund.OpcHelper.OpcRcw.Hda
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), ComConversionLoss]
    public struct OPCHDA_ATTRIBUTE
    {
        public int hClient;
        public int dwNumValues;
        public int dwAttributeID;
        [ComConversionLoss]
        public IntPtr ftTimeStamps;
        [ComConversionLoss]
        public IntPtr vAttributeValues;
    }
}

