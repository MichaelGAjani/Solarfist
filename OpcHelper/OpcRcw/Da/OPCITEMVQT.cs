namespace Jund.OpcHelper.OpcRcw.Da
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct OPCITEMVQT
    {
        [MarshalAs(UnmanagedType.Struct)]
        public object vDataValue;
        public int bQualitySpecified;
        public short wQuality;
        public short wReserved;
        public int bTimeStampSpecified;
        public int dwReserved;
        public OpcRcw.Da.FILETIME ftTimeStamp;
    }
}

