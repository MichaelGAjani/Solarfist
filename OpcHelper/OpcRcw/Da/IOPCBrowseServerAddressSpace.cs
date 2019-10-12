namespace Jund.OpcHelper.OpcRcw.Da
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("39C13A4F-011E-11D0-9675-0020AFD8ADB3"), InterfaceType((short) 1)]
    public interface IOPCBrowseServerAddressSpace
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void QueryOrganization(out OPCNAMESPACETYPE pNameSpaceType);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ChangeBrowsePosition([In] OPCBROWSEDIRECTION dwBrowseDirection, [In, MarshalAs(UnmanagedType.LPWStr)] string szString);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void BrowseOPCItemIDs([In] OPCBROWSETYPE dwBrowseFilterType, [In, MarshalAs(UnmanagedType.LPWStr)] string szFilterCriteria, [In] short vtDataTypeFilter, [In] int dwAccessRightsFilter, [MarshalAs(UnmanagedType.Interface)] out IEnumString ppIEnumString);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetItemID([In, MarshalAs(UnmanagedType.LPWStr)] string szItemDataID, [MarshalAs(UnmanagedType.LPWStr)] out string szItemID);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void BrowseAccessPaths([In, MarshalAs(UnmanagedType.LPWStr)] string szItemID, [MarshalAs(UnmanagedType.Interface)] out IEnumString ppIEnumString);
    }
}

