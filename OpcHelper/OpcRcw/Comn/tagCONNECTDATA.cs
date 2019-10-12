namespace Jund.OpcHelper.OpcRcw.Comn
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct tagCONNECTDATA
    {
        [MarshalAs(UnmanagedType.IUnknown)]
        public object pUnk;
        public int dwCookie;
    }
}

