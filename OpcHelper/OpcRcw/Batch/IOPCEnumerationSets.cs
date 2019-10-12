namespace Jund.OpcHelper.OpcRcw.Batch
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, ComConversionLoss, Guid("A8080DA3-E23E-11D2-AFA7-00C04F539421"), InterfaceType((short) 1)]
    public interface IOPCEnumerationSets
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void QueryEnumerationSets(out int pdwCount, out IntPtr ppdwEnumSetId, out IntPtr ppszEnumSetName);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void QueryEnumeration([In] int dwEnumSetId, [In] int dwEnumValue, [MarshalAs(UnmanagedType.LPWStr)] out string pszEnumName);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void QueryEnumerationList([In] int dwEnumSetId, out int pdwCount, out IntPtr ppdwEnumValue, out IntPtr ppszEnumName);
    }
}

