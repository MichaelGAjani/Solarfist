namespace Jund.OpcHelper.OpcRcw.Cmd
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8)]
    public struct ArgumentDefinition
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szName;
        public short vtValueType;
        public short wReserved;
        public int bOptional;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szDescription;
        [MarshalAs(UnmanagedType.Struct)]
        public object vDefaultValue;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szUnitType;
        public int dwReserved;
        [MarshalAs(UnmanagedType.Struct)]
        public object vLowLimit;
        [MarshalAs(UnmanagedType.Struct)]
        public object vHighLimit;
    }
}

