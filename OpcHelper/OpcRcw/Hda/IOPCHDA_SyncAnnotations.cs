namespace Jund.OpcHelper.OpcRcw.Hda
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, ComConversionLoss, InterfaceType((short) 1), Guid("1F1217B4-DEE0-11D2-A5E5-000086339399")]
    public interface IOPCHDA_SyncAnnotations
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void QueryCapabilities(out OPCHDA_ANNOTATIONCAPABILITIES pCapabilities);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Read([In, Out] ref OPCHDA_TIME htStartTime, [In, Out] ref OPCHDA_TIME htEndTime, [In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] int[] phServer, out IntPtr ppAnnotationValues, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Insert([In] int dwNumItems, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] phServer, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)] OPCHDA_FILETIME[] ftTimeStamps, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)] OPCHDA_ANNOTATION[] pAnnotationValues, out IntPtr ppErrors);
    }
}

