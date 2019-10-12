namespace Jund.OpcHelper.OpcRcw.Comn
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("F31DFDE1-07B6-11D2-B2D8-0060083BA1FB"), InterfaceType((short) 1)]
    public interface IOPCShutdown
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ShutdownRequest([In, MarshalAs(UnmanagedType.LPWStr)] string szReason);
    }
}

