namespace Jund.OpcHelper.OpcRcw.Hda
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("1F1217B8-DEE0-11D2-A5E5-000086339399"), InterfaceType((short) 1), ComConversionLoss]
    public interface IOPCHDA_Playback
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ReadRawWithUpdate([In] int dwTransactionID, [In, Out] ref OPCHDA_TIME htStartTime, [In, Out] ref OPCHDA_TIME htEndTime, [In] int dwNumValues, [In] OPCHDA_FILETIME ftUpdateDuration, [In] OPCHDA_FILETIME ftUpdateInterval, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=6)] int[] phServer, out int pdwCancelID, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ReadProcessedWithUpdate([In] int dwTransactionID, [In, Out] ref OPCHDA_TIME htStartTime, [In, Out] ref OPCHDA_TIME htEndTime, [In] OPCHDA_FILETIME ftResampleInterval, [In] int dwNumIntervals, [In] OPCHDA_FILETIME ftUpdateInterval, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=6)] int[] phServer, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=6)] int[] haAggregate, out int pdwCancelID, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Cancel([In] int dwCancelID);
    }
}

