namespace Jund.OpcHelper.OpcRcw.Cmd
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct EventDefinition
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szDescription;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szDataTypeDefinition;
        public int dwReserved;
    }
}

