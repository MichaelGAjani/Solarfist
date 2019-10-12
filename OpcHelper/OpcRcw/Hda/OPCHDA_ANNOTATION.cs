namespace Jund.OpcHelper.OpcRcw.Hda
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), ComConversionLoss]
    public struct OPCHDA_ANNOTATION
    {
        public int hClient;
        public int dwNumValues;
        [ComConversionLoss]
        public IntPtr ftTimeStamps;
        [ComConversionLoss]
        public IntPtr szAnnotation;
        [ComConversionLoss]
        public IntPtr ftAnnotationTime;
        [ComConversionLoss]
        public IntPtr szUser;
    }
}

