namespace Jund.OpcHelper.OpcRcw.Da
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType((short) 1), Guid("39C13A70-011E-11D0-9675-0020AFD8ADB3")]
    public interface IOPCDataCallback
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void OnDataChange([In] int dwTransid, [In] int hGroup, [In] int hrMasterquality, [In] int hrMastererror, [In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] int[] phClientItems, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=4)] object[] pvValues, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] short[] pwQualities, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=4)] OpcRcw.Da.FILETIME[] pftTimeStamps, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] int[] pErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void OnReadComplete([In] int dwTransid, [In] int hGroup, [In] int hrMasterquality, [In] int hrMastererror, [In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] int[] phClientItems, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=4)] object[] pvValues, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] short[] pwQualities, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=4)] OpcRcw.Da.FILETIME[] pftTimeStamps, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] int[] pErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void OnWriteComplete([In] int dwTransid, [In] int hGroup, [In] int hrMastererr, [In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] int[] pClienthandles, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] int[] pErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void OnCancelComplete([In] int dwTransid, [In] int hGroup);
    }
}

