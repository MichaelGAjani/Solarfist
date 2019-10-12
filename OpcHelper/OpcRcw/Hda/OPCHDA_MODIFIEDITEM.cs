namespace Jund.OpcHelper.OpcRcw.Hda
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), ComConversionLoss]
    public struct OPCHDA_MODIFIEDITEM
    {
        public int hClient;
        public int dwCount;
        [ComConversionLoss]
        public IntPtr pftTimeStamps;
        [ComConversionLoss]
        public IntPtr pdwQualities;
        [ComConversionLoss]
        public IntPtr pvDataValues;
        [ComConversionLoss]
        public IntPtr pftModificationTime;
        [ComConversionLoss]
        public IntPtr pEditType;
        [ComConversionLoss]
        public IntPtr szUser;
    }
}

