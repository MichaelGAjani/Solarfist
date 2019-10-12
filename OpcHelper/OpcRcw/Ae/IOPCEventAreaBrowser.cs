namespace Jund.OpcHelper.OpcRcw.Ae
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType((short) 1), Guid("65168857-5783-11D1-84A0-00608CB8A7E9")]
    public interface IOPCEventAreaBrowser
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ChangeBrowsePosition([In, ComAliasName("OpcAeRcw.OPCAEBROWSEDIRECTION")] OPCAEBROWSEDIRECTION dwBrowseDirection, [In, MarshalAs(UnmanagedType.LPWStr)] string szString);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void BrowseOPCAreas([In, ComAliasName("OpcAeRcw.OPCAEBROWSETYPE")] OPCAEBROWSETYPE dwBrowseFilterType, [In, MarshalAs(UnmanagedType.LPWStr)] string szFilterCriteria, [MarshalAs(UnmanagedType.Interface)] out IEnumString ppIEnumString);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetQualifiedAreaName([In, MarshalAs(UnmanagedType.LPWStr)] string szAreaName, [MarshalAs(UnmanagedType.LPWStr)] out string pszQualifiedAreaName);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetQualifiedSourceName([In, MarshalAs(UnmanagedType.LPWStr)] string szSourceName, [MarshalAs(UnmanagedType.LPWStr)] out string pszQualifiedSourceName);
    }
}

