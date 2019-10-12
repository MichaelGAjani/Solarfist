namespace Jund.OpcHelper.OpcRcw.Cmd
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct ActionDefinition
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szDescription;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szEventName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szInArguments;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szOutArguments;
        public int dwReserved;
    }
}

