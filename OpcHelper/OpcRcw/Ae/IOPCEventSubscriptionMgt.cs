namespace Jund.OpcHelper.OpcRcw.Ae
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType((short) 1), Guid("65168855-5783-11D1-84A0-00608CB8A7E9"), ComConversionLoss]
    public interface IOPCEventSubscriptionMgt
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void SetFilter([In] int dwEventType, [In] int dwNumCategories, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] int[] pdwEventCategories, [In] int dwLowSeverity, [In] int dwHighSeverity, [In] int dwNumAreas, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=5)] string[] pszAreaList, [In] int dwNumSources, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=7)] string[] pszSourceList);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetFilter(out int pdwEventType, out int pdwNumCategories, out IntPtr ppdwEventCategories, out int pdwLowSeverity, out int pdwHighSeverity, out int pdwNumAreas, out IntPtr ppszAreaList, out int pdwNumSources, out IntPtr ppszSourceList);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void SelectReturnedAttributes([In] int dwEventCategory, [In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] int[] dwAttributeIDs);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetReturnedAttributes([In] int dwEventCategory, out int pdwCount, out IntPtr ppdwAttributeIDs);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Refresh([In] int dwConnection);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void CancelRefresh([In] int dwConnection);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetState(out int pbActive, out int pdwBufferTime, out int pdwMaxSize, out int phClientSubscription);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void SetState([In] IntPtr pbActive, [In] IntPtr pdwBufferTime, [In] IntPtr pdwMaxSize, [In] int hClientSubscription, out int pdwRevisedBufferTime, out int pdwRevisedMaxSize);
    }
}

