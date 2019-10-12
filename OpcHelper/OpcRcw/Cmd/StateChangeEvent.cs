namespace Jund.OpcHelper.OpcRcw.Cmd
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), ComConversionLoss]
    public struct StateChangeEvent
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szEventName;
        public int dwReserved;
        public _FILETIME ftEventTime;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szEventData;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szOldState;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szNewState;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szStateData;
        public int dwNoOfInArguments;
        [ComConversionLoss]
        public IntPtr pInArguments;
        public int dwNoOfOutArguments;
        [ComConversionLoss]
        public IntPtr pOutArguments;
    }
}

