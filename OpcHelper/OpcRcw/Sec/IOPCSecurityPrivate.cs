namespace Jund.OpcHelper.OpcRcw.Sec
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType((short) 1), Guid("7AA83A02-6C77-11D3-84F9-00008630A38B")]
    public interface IOPCSecurityPrivate
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void IsAvailablePriv(out int pbAvailable);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Logon([In, MarshalAs(UnmanagedType.LPWStr)] string szUserID, [In, MarshalAs(UnmanagedType.LPWStr)] string szPassword);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Logoff();
    }
}

