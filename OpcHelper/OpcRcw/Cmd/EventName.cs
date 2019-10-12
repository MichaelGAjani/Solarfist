namespace Jund.OpcHelper.OpcRcw.Cmd
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class EventName
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPCCMD_EVENT_NAME_ABORTED = "Aborted";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPCCMD_EVENT_NAME_CANCELLED = "Cancelled";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPCCMD_EVENT_NAME_FINISHED = "Finished";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPCCMD_EVENT_NAME_HALTED = "Halted";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPCCMD_EVENT_NAME_INVOKE = "Invoke";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPCCMD_EVENT_NAME_RESET = "Reset";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPCCMD_EVENT_NAME_RESUMED = "Resumed";
    }
}

