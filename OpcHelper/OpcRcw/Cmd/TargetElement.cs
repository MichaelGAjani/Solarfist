namespace Jund.OpcHelper.OpcRcw.Cmd
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), ComConversionLoss]
    public struct TargetElement
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szLabel;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szTargetID;
        public int bIsTarget;
        public int bHasChildren;
        public int dwNoOfNamespaceUris;
        [ComConversionLoss]
        public IntPtr pszNamespaceUris;
    }
}

