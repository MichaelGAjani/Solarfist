namespace Jund.OpcHelper.OpcRcw.Ae
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), ComConversionLoss]
    public struct OPCCONDITIONSTATE
    {
        public short wState;
        public short wReserved1;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szActiveSubCondition;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szASCDefinition;
        public int dwASCSeverity;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szASCDescription;
        public short wQuality;
        public short wReserved2;
        public OpcRcw.Ae.FILETIME ftLastAckTime;
        public OpcRcw.Ae.FILETIME ftSubCondLastActive;
        public OpcRcw.Ae.FILETIME ftCondLastActive;
        public OpcRcw.Ae.FILETIME ftCondLastInactive;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szAcknowledgerID;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szComment;
        public int dwNumSCs;
        [ComConversionLoss]
        public IntPtr pszSCNames;
        [ComConversionLoss]
        public IntPtr pszSCDefinitions;
        [ComConversionLoss]
        public IntPtr pdwSCSeverities;
        [ComConversionLoss]
        public IntPtr pszSCDescriptions;
        public int dwNumEventAttrs;
        [ComConversionLoss]
        public IntPtr pEventAttributes;
        [ComConversionLoss]
        public IntPtr pErrors;
    }
}

