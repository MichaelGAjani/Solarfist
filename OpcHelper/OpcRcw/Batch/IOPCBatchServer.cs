namespace Jund.OpcHelper.OpcRcw.Batch
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("8BB4ED50-B314-11D3-B3EA-00C04F8ECEAA"), InterfaceType((short) 1)]
    public interface IOPCBatchServer
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetDelimiter([MarshalAs(UnmanagedType.LPWStr)] out string pszDelimiter);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void CreateEnumerator([In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
    }
}

