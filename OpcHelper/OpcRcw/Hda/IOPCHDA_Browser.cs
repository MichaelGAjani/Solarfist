namespace Jund.OpcHelper.OpcRcw.Hda
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType((short) 1), Guid("1F1217B1-DEE0-11D2-A5E5-000086339399")]
    public interface IOPCHDA_Browser
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetEnum([In] OPCHDA_BROWSETYPE dwBrowseType, [MarshalAs(UnmanagedType.Interface)] out IEnumString ppIEnumString);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ChangeBrowsePosition([In] OPCHDA_BROWSEDIRECTION dwBrowseDirection, [In, MarshalAs(UnmanagedType.LPWStr)] string szString);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetItemID([In, MarshalAs(UnmanagedType.LPWStr)] string szNode, [MarshalAs(UnmanagedType.LPWStr)] out string pszItemID);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetBranchPosition([MarshalAs(UnmanagedType.LPWStr)] out string pszBranchPos);
    }
}

