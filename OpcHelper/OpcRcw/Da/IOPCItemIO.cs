namespace Jund.OpcHelper.OpcRcw.Da
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, ComConversionLoss, Guid("85C0B427-2893-4CBC-BD78-E5FC5146F08F"), InterfaceType((short) 1)]
    public interface IOPCItemIO
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Read([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)] string[] pszItemIDs, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] int[] pdwMaxAge, out IntPtr ppvValues, out IntPtr ppwQualities, out IntPtr ppftTimeStamps, out IntPtr ppErrors);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void WriteVQT([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)] string[] pszItemIDs, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)] OPCITEMVQT[] pItemVQT, out IntPtr ppErrors);
    }
}

