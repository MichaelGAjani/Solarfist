namespace Jund.OpcHelper.OpcRcw.Cmd
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4), ComConversionLoss]
    public struct NamespaceDefinition
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szUri;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szDescription;
        public int dwNoOfCommandNames;
        [ComConversionLoss]
        public IntPtr pszCommandNames;
    }
}

