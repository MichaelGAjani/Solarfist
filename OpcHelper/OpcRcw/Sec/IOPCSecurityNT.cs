namespace Jund.OpcHelper.OpcRcw.Sec
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType((short) 1), Guid("7AA83A01-6C77-11D3-84F9-00008630A38B")]
    public interface IOPCSecurityNT
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void IsAvailableNT(out int pbAvailable);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void QueryMinImpersonationLevel(out uint pdwMinImpLevel);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ChangeUser();
    }
}

