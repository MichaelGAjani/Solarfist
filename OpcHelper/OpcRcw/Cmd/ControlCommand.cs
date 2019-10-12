namespace Jund.OpcHelper.OpcRcw.Cmd
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class ControlCommand
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPCCMD_CONTROL_CANCEL = "Cancel";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPCCMD_CONTROL_RESUME = "Resume";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPCCMD_CONTROL_SUSPEND = "Suspend";
    }
}

