namespace Jund.OpcHelper.OpcRcw.Da
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, ComConversionLoss, InterfaceType((short) 1), Guid("39C13A52-011E-11D0-9675-0020AFD8ADB3")]
    public interface IOPCSyncIO
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Read([In] OPCDATASOURCE dwSource, [In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] int[] phServer, out IntPtr ppItemValues, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Write([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] phServer, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)] object[] pItemValues, out IntPtr ppErrors);
    }
}

