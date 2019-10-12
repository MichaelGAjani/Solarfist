namespace Jund.OpcHelper.OpcRcw.Batch
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct tagOPCBATCHSUMMARYFILTER
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szID;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szDescription;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szOPCItemID;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szMasterRecipeID;
        public float fMinBatchSize;
        public float fMaxBatchSize;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szEU;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szExecutionState;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szExecutionMode;
        public _FILETIME ftMinStartTime;
        public _FILETIME ftMaxStartTime;
        public _FILETIME ftMinEndTime;
        public _FILETIME ftMaxEndTime;
    }
}

