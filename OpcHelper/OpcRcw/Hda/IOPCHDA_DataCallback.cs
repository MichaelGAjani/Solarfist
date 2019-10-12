namespace Jund.OpcHelper.OpcRcw.Hda
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("1F1217B9-DEE0-11D2-A5E5-000086339399"), ComConversionLoss, InterfaceType((short) 1)]
    public interface IOPCHDA_DataCallback
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void OnDataChange([In] int dwTransactionID, [In] int hrStatus, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=2)] OPCHDA_ITEM[] pItemValues, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] int[] phrErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void OnReadComplete([In] int dwTransactionID, [In] int hrStatus, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=2)] OPCHDA_ITEM[] pItemValues, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] int[] phrErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void OnReadModifiedComplete([In] int dwTransactionID, [In] int hrStatus, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=2)] OPCHDA_MODIFIEDITEM[] pItemValues, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] int[] phrErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void OnReadAttributeComplete([In] int dwTransactionID, [In] int hrStatus, [In] int hClient, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=3)] OPCHDA_ATTRIBUTE[] pAttributeValues, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] int[] phrErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void OnReadAnnotations([In] int dwTransactionID, [In] int hrStatus, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=2)] OPCHDA_ANNOTATION[] pAnnotationValues, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] int[] phrErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void OnInsertAnnotations([In] int dwTransactionID, [In] int hrStatus, [In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] int[] phClients, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] int[] phrErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void OnPlayback([In] int dwTransactionID, [In] int hrStatus, [In] int dwNumItems, [In] IntPtr ppItemValues, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] int[] phrErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void OnUpdateComplete([In] int dwTransactionID, [In] int hrStatus, [In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] int[] phClients, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] int[] phrErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void OnCancelComplete([In] int dwCancelID);
    }
}

