namespace Jund.OpcHelper.OpcRcw.Cmd
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct StateTransition
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szTransitionID;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szStartState;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szEndState;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szTriggerEvent;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szAction;
        public int dwReserved;
    }
}

