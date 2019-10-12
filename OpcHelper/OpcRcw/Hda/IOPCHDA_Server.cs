namespace Jund.OpcHelper.OpcRcw.Hda
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType((short) 1), Guid("1F1217B0-DEE0-11D2-A5E5-000086339399"), ComConversionLoss]
    public interface IOPCHDA_Server
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetItemAttributes(out int pdwCount, out IntPtr ppdwAttrID, out IntPtr ppszAttrName, out IntPtr ppszAttrDesc, out IntPtr ppvtAttrDataType);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetAggregates(out int pdwCount, out IntPtr ppdwAggrID, out IntPtr ppszAggrName, out IntPtr ppszAggrDesc);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetHistorianStatus(out OPCHDA_SERVERSTATUS pwStatus, out IntPtr pftCurrentTime, out IntPtr pftStartTime, out short pwMajorVersion, out short pwMinorVersion, out short pwBuildNumber, out int pdwMaxReturnValues, [MarshalAs(UnmanagedType.LPWStr)] out string ppszStatusString, [MarshalAs(UnmanagedType.LPWStr)] out string ppszVendorInfo);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetItemHandles([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)] string[] pszItemID, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] phClient, out IntPtr pphServer, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ReleaseItemHandles([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] phServer, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ValidateItemIDs([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)] string[] pszItemID, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void CreateBrowse([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] pdwAttrID, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] OPCHDA_OPERATORCODES[] pOperator, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)] object[] vFilter, [MarshalAs(UnmanagedType.Interface)] out IOPCHDA_Browser pphBrowser, out IntPtr ppErrors);
    }
}

