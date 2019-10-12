namespace Jund.OpcHelper.OpcRcw.Cmd
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class Constants
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_CATEGORY_DESCRIPTION_CMD10 = "OPC Command Execution Servers Version 1.0";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPCCMD_NAMESPACE_V10 = "http://opcfoundation.org/webservices/OPCCMD/10";
    }
}

