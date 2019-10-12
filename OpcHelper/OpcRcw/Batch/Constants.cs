namespace Jund.OpcHelper.OpcRcw.Batch
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class Constants
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_CATEGORY_DESCRIPTION_BATCH10 = "OPC Batch Server Version 1.0";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_CATEGORY_DESCRIPTION_BATCH20 = "OPC Batch Server Version 2.0";
    }
}

