namespace Jund.OpcHelper.OpcRcw.Cmd
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("3104B526-2016-442D-9696-1275DE978778"), ComConversionLoss, InterfaceType((short) 1)]
    public interface IOPCCommandExecution
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void SyncInvoke([In, MarshalAs(UnmanagedType.LPWStr)] string szCommandName, [In, MarshalAs(UnmanagedType.LPWStr)] string szNamespaceUri, [In, MarshalAs(UnmanagedType.LPWStr)] string szTargetID, [In] int dwNoOfArguments, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=3)] Argument[] pArguments, [In] int dwNoOfFilters, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=5)] string[] szFilters, out int pdwNoOfEvents, out IntPtr ppEvents);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void AsyncInvoke([In, MarshalAs(UnmanagedType.LPWStr)] string szCommandName, [In, MarshalAs(UnmanagedType.LPWStr)] string szNamespaceUri, [In, MarshalAs(UnmanagedType.LPWStr)] string szTargetID, [In] int dwNoOfArguments, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=3)] Argument[] pArguments, [In] int dwNoOfFilters, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=5)] string[] pszFilters, [In, MarshalAs(UnmanagedType.Interface)] IOPCComandCallback ipCallback, [In] int dwUpdateFrequency, [In] int dwKeepAliveTime, [MarshalAs(UnmanagedType.LPWStr)] out string pszInvokeUUID, out int pdwRevisedUpdateFrequency);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Connect([In, MarshalAs(UnmanagedType.LPWStr)] string szInvokeUUID, [In, MarshalAs(UnmanagedType.Interface)] IOPCComandCallback ipCallback, [In] int dwUpdateFrequency, [In] int dwKeepAliveTime, out int pdwRevisedUpdateFrequency);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Disconnect([In, MarshalAs(UnmanagedType.LPWStr)] string szInvokeUUID);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void QueryState([In, MarshalAs(UnmanagedType.LPWStr)] string szInvokeUUID, [In] int dwWaitTime, out int pdwNoOfEvents, out IntPtr ppEvents, out int pdwNoOfPermittedControls, out IntPtr ppszPermittedControls, out int pbNoStateChange);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Control([In, MarshalAs(UnmanagedType.LPWStr)] string szInvokeUUID, [In, MarshalAs(UnmanagedType.LPWStr)] string szControl);
    }
}

