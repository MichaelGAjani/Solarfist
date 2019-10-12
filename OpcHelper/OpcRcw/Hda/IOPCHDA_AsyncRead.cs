namespace Jund.OpcHelper.OpcRcw.Hda
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType((short) 1), Guid("1F1217B5-DEE0-11D2-A5E5-000086339399"), ComConversionLoss]
    public interface IOPCHDA_AsyncRead
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ReadRaw([In] int dwTransactionID, [In, Out] ref OPCHDA_TIME htStartTime, [In, Out] ref OPCHDA_TIME htEndTime, [In] int dwNumValues, [In] int bBounds, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=5)] int[] phServer, out int pdwCancelID, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void AdviseRaw([In] int dwTransactionID, [In, Out] ref OPCHDA_TIME htStartTime, [In] OPCHDA_FILETIME ftUpdateInterval, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] int[] phServer, out int pdwCancelID, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ReadProcessed([In] int dwTransactionID, [In, Out] ref OPCHDA_TIME htStartTime, [In, Out] ref OPCHDA_TIME htEndTime, [In] OPCHDA_FILETIME ftResampleInterval, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] int[] phServer, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] int[] haAggregate, out int pdwCancelID, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void AdviseProcessed([In] int dwTransactionID, [In, Out] ref OPCHDA_TIME htStartTime, [In] OPCHDA_FILETIME ftResampleInterval, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] int[] phServer, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] int[] haAggregate, [In] int dwNumIntervals, out int pdwCancelID, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ReadAtTime([In] int dwTransactionID, [In] int dwNumTimeStamps, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=1)] OPCHDA_FILETIME[] ftTimeStamps, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] int[] phServer, out int pdwCancelID, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ReadModified([In] int dwTransactionID, [In, Out] ref OPCHDA_TIME htStartTime, [In, Out] ref OPCHDA_TIME htEndTime, [In] int dwNumValues, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] int[] phServer, out int pdwCancelID, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ReadAttribute([In] int dwTransactionID, [In, Out] ref OPCHDA_TIME htStartTime, [In, Out] ref OPCHDA_TIME htEndTime, [In] int hServer, [In] int dwNumAttributes, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] int[] dwAttributeIDs, out int pdwCancelID, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Cancel([In] int dwCancelID);
    }
}

