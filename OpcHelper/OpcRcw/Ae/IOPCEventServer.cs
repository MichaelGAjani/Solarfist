namespace Jund.OpcHelper.OpcRcw.Ae
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType((short) 1), Guid("65168851-5783-11D1-84A0-00608CB8A7E9"), ComConversionLoss]
    public interface IOPCEventServer
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetStatus([ComAliasName("OpcAeRcw.OPCEVENTSERVERSTATUS")] out IntPtr ppEventServerStatus);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void CreateEventSubscription([In] int bActive, [In] int dwBufferTime, [In] int dwMaxSize, [In] int hClientSubscription, [In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppUnk, out int pdwRevisedBufferTime, out int pdwRevisedMaxSize);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void QueryAvailableFilters(out int pdwFilterMask);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void QueryEventCategories([In] int dwEventType, out int pdwCount, out IntPtr ppdwEventCategories, out IntPtr ppszEventCategoryDescs);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void QueryConditionNames([In] int dwEventCategory, out int pdwCount, out IntPtr ppszConditionNames);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void QuerySubConditionNames([In, MarshalAs(UnmanagedType.LPWStr)] string szConditionName, out int pdwCount, out IntPtr ppszSubConditionNames);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void QuerySourceConditions([In, MarshalAs(UnmanagedType.LPWStr)] string szSource, out int pdwCount, out IntPtr ppszConditionNames);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void QueryEventAttributes([In] int dwEventCategory, out int pdwCount, out IntPtr ppdwAttrIDs, out IntPtr ppszAttrDescs, out IntPtr ppvtAttrTypes);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void TranslateToItemIDs([In, MarshalAs(UnmanagedType.LPWStr)] string szSource, [In] int dwEventCategory, [In, MarshalAs(UnmanagedType.LPWStr)] string szConditionName, [In, MarshalAs(UnmanagedType.LPWStr)] string szSubconditionName, [In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] int[] pdwAssocAttrIDs, out IntPtr ppszAttrItemIDs, out IntPtr ppszNodeNames, out IntPtr ppCLSIDs);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetConditionState([In, MarshalAs(UnmanagedType.LPWStr)] string szSource, [In, MarshalAs(UnmanagedType.LPWStr)] string szConditionName, [In] int dwNumEventAttrs, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] int[] pdwAttributeIDs, [ComAliasName("OpcAeRcw.OPCCONDITIONSTATE")] out IntPtr ppConditionState);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void EnableConditionByArea([In] int dwNumAreas, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)] string[] pszAreas);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void EnableConditionBySource([In] int dwNumSources, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)] string[] pszSources);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void DisableConditionByArea([In] int dwNumAreas, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)] string[] pszAreas);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void DisableConditionBySource([In] int dwNumSources, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)] string[] pszSources);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void AckCondition([In] int dwCount, [In, MarshalAs(UnmanagedType.LPWStr)] string szAcknowledgerID, [In, MarshalAs(UnmanagedType.LPWStr)] string szComment, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)] string[] pszSource, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)] string[] pszConditionName, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)] OpcRcw.Ae.FILETIME[] pftActiveTime, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] pdwCookie, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void CreateAreaBrowser([In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
    }
}

