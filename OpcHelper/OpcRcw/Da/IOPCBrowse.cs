namespace Jund.OpcHelper.OpcRcw.Da
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("39227004-A18F-4B57-8B0A-5235670F4468"), InterfaceType((short) 1), ComConversionLoss]
    public interface IOPCBrowse
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetProperties([In] int dwItemCount, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)] string[] pszItemIDs, [In] int bReturnPropertyValues, [In] int dwPropertyCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] int[] pdwPropertyIDs, out IntPtr ppItemProperties);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Browse([In, MarshalAs(UnmanagedType.LPWStr)] string szItemID, [In, Out] ref IntPtr pszContinuationPoint, [In] int dwMaxElementsReturned, [In] OPCBROWSEFILTER dwBrowseFilter, [In, MarshalAs(UnmanagedType.LPWStr)] string szElementNameFilter, [In, MarshalAs(UnmanagedType.LPWStr)] string szVendorFilter, [In] int bReturnAllProperties, [In] int bReturnPropertyValues, [In] int dwPropertyCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=8)] int[] pdwPropertyIDs, out int pbMoreElements, out int pdwCount, out IntPtr ppBrowseElements);
    }
}

