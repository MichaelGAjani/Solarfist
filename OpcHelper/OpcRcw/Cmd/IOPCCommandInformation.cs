namespace Jund.OpcHelper.OpcRcw.Cmd
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType((short) 1), ComConversionLoss, Guid("3104B525-2016-442D-9696-1275DE978778")]
    public interface IOPCCommandInformation
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void QueryCapabilities(out double pdblMaxStorageTime, out int pbSupportsEventFilter);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void QueryComands(out int pdwCount, out IntPtr ppNamespaces);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void BrowseCommandTargets([In, MarshalAs(UnmanagedType.LPWStr)] string szTargetID, [In, MarshalAs(UnmanagedType.LPWStr)] string szNamespaceUri, [In] BrowseFilter eBrowseFilter, out int pdwCount, out IntPtr ppTargets);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetCommandDescription([In, MarshalAs(UnmanagedType.LPWStr)] string szCommandName, [In, MarshalAs(UnmanagedType.LPWStr)] string szNamespaceUri, out CommandDescription pDescription);
    }
}

