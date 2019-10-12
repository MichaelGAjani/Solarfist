namespace Jund.OpcHelper.OpcRcw.Da
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class Constants
    {
        public const int OPC_BROWSE_HASCHILDREN = 1;
        public const int OPC_BROWSE_ISITEM = 2;
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_CATEGORY_DESCRIPTION_DA10 = "OPC Data Access Servers Version 1.0";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_CATEGORY_DESCRIPTION_DA20 = "OPC Data Access Servers Version 2.0";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_CATEGORY_DESCRIPTION_DA30 = "OPC Data Access Servers Version 3.0";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_CATEGORY_DESCRIPTION_XMLDA10 = "OPC XML Data Access Servers Version 1.0";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_CONSISTENCY_WINDOW_NOT_CONSISTENT = "Not Consistent";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_CONSISTENCY_WINDOW_UNKNOWN = "Unknown";
        public const int OPC_READABLE = 1;
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_TYPE_SYSTEM_OPCBINARY = "OPCBinary";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_TYPE_SYSTEM_XMLSCHEMA = "XMLSchema";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_WRITE_BEHAVIOR_ALL_OR_NOTHING = "All or Nothing";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_WRITE_BEHAVIOR_BEST_EFFORT = "Best Effort";
        public const int OPC_WRITEABLE = 2;
    }
}

