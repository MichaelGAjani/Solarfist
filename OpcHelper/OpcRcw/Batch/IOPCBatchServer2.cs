namespace Jund.OpcHelper.OpcRcw.Batch
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType((short) 1), Guid("895A78CF-B0C5-11D4-A0B7-000102A980B1")]
    public interface IOPCBatchServer2
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void CreateFilteredEnumerator([In] ref Guid riid, [In] ref tagOPCBATCHSUMMARYFILTER pFilter, [In, MarshalAs(UnmanagedType.LPWStr)] string szModel, [MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
    }
}

