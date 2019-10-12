namespace Jund.OpcHelper.OpcRcw.Dx
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class ServerTypeName
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_TYPE_COM_DA10 = "COM-DA1.0";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_TYPE_COM_DA204 = "COM-DA2.04";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_TYPE_COM_DA205 = "COM-DA2.05";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_TYPE_COM_DA30 = "COM-DA3.0";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_TYPE_XML_DA10 = "XML-DA1.0";
    }
}

