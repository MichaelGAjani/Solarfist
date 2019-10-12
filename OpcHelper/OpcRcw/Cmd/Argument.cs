namespace Jund.OpcHelper.OpcRcw.Cmd
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8)]
    public struct Argument
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szName;
        [MarshalAs(UnmanagedType.Struct)]
        public object vValue;
    }
}

