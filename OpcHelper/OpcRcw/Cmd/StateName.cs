namespace Jund.OpcHelper.OpcRcw.Cmd
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class StateName
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPCCMD_STATE_NAME_ABNORMAL_FAILURE = "AbnormalFailure";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPCCMD_STATE_NAME_COMPLETE = "Complete";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPCCMD_STATE_NAME_EXECUTING = "Executing";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPCCMD_STATE_NAME_HALTED = "Halted";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPCCMD_STATE_NAME_IDLE = "Idle";
    }
}

