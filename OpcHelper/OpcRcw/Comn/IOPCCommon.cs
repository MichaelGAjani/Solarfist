namespace Jund.OpcHelper.OpcRcw.Comn
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, ComConversionLoss, Guid("F31DFDE2-07B6-11D2-B2D8-0060083BA1FB"), InterfaceType((short) 1)]
    public interface IOPCCommon
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void SetLocaleID([In] int dwLcid);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetLocaleID(out int pdwLcid);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void QueryAvailableLocaleIDs(out int pdwCount, out IntPtr pdwLcid);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetErrorString([In] int dwError, [MarshalAs(UnmanagedType.LPWStr)] out string ppString);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void SetClientName([In, MarshalAs(UnmanagedType.LPWStr)] string szName);
    }
}

