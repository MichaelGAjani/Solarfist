namespace Jund.OpcHelper.OpcRcw.Ae
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), ComConversionLoss]
    public struct ONEVENTSTRUCT
    {
        public short wChangeMask;
        public short wNewState;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szSource;
        public OpcRcw.Ae.FILETIME ftTime;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szMessage;
        public int dwEventType;
        public int dwEventCategory;
        public int dwSeverity;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szConditionName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szSubconditionName;
        public short wQuality;
        public short wReserved;
        public int bAckRequired;
        public OpcRcw.Ae.FILETIME ftActiveTime;
        public int dwCookie;
        public int dwNumEventAttrs;
        [ComConversionLoss]
        public IntPtr pEventAttributes;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szActorID;
    }
}

