namespace Jund.OpcHelper.OpcRcw.Hda
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType((short) 1), Guid("1F1217B7-DEE0-11D2-A5E5-000086339399"), ComConversionLoss]
    public interface IOPCHDA_AsyncAnnotations
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void QueryCapabilities(out OPCHDA_ANNOTATIONCAPABILITIES pCapabilities);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Read([In] int dwTransactionID, [In, Out] ref OPCHDA_TIME htStartTime, [In, Out] ref OPCHDA_TIME htEndTime, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] int[] phServer, out int pdwCancelID, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Insert([In] int dwTransactionID, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] int[] phServer, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=1)] OPCHDA_FILETIME[] ftTimeStamps, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=1)] OPCHDA_ANNOTATION[] pAnnotationValues, out int pdwCancelID, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Cancel([In] int dwCancelID);
    }
}

