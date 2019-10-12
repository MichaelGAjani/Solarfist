namespace Jund.OpcHelper.OpcRcw.Hda
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType((short) 1), ComConversionLoss, Guid("1F1217B2-DEE0-11D2-A5E5-000086339399")]
    public interface IOPCHDA_SyncRead
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ReadRaw([In, Out] ref OPCHDA_TIME htStartTime, [In, Out] ref OPCHDA_TIME htEndTime, [In] int dwNumValues, [In] int bBounds, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] int[] phServer, out IntPtr ppItemValues, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ReadProcessed([In, Out] ref OPCHDA_TIME htStartTime, [In, Out] ref OPCHDA_TIME htEndTime, [In] OPCHDA_FILETIME ftResampleInterval, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] int[] phServer, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] int[] haAggregate, out IntPtr ppItemValues, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ReadAtTime([In] int dwNumTimeStamps, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)] OPCHDA_FILETIME[] ftTimeStamps, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] int[] phServer, out IntPtr ppItemValues, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ReadModified([In, Out] ref OPCHDA_TIME htStartTime, [In, Out] ref OPCHDA_TIME htEndTime, [In] int dwNumValues, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] int[] phServer, out IntPtr ppItemValues, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ReadAttribute([In, Out] ref OPCHDA_TIME htStartTime, [In, Out] ref OPCHDA_TIME htEndTime, [In] int hServer, [In] int dwNumAttributes, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] int[] pdwAttributeIDs, out IntPtr ppAttributeValues, out IntPtr ppErrors);
    }
}

