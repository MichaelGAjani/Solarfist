namespace Jund.OpcHelper.OpcRcw.Dx
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("C130D281-F4AA-4779-8846-C2C4CB444F2A"), InterfaceType((short) 1), ComConversionLoss]
    public interface IOPCConfiguration
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetServers(out int pdwCount, out IntPtr ppServers);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void AddServers([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)] SourceServer[] pServers, out DXGeneralResponse pResponse);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ModifyServers([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)] SourceServer[] pServers, out DXGeneralResponse pResponse);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void DeleteServers([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)] ItemIdentifier[] pServers, out DXGeneralResponse pResponse);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void CopyDefaultServerAttributes([In] int bConfigToStatus, [In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=1)] ItemIdentifier[] pServers, out DXGeneralResponse pResponse);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void QueryDXConnections([In, MarshalAs(UnmanagedType.LPWStr)] string szBrowsePath, [In] int dwNoOfMasks, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=1)] DXConnection[] pDXConnectionMasks, [In] int bRecursive, out IntPtr ppErrors, out int pdwCount, out IntPtr ppConnections);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void AddDXConnections([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)] DXConnection[] pConnections, out DXGeneralResponse pResponse);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void UpdateDXConnections([In, MarshalAs(UnmanagedType.LPWStr)] string szBrowsePath, [In] int dwNoOfMasks, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=1)] DXConnection[] pDXConnectionMasks, [In] int bRecursive, [In] ref DXConnection pDXConnectionDefinition, out IntPtr ppErrors, out DXGeneralResponse pResponse);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ModifyDXConnections([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)] DXConnection[] pDXConnectionDefinitions, out DXGeneralResponse pResponse);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void DeleteDXConnections([In, MarshalAs(UnmanagedType.LPWStr)] string szBrowsePath, [In] int dwNoOfMasks, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=1)] DXConnection[] pDXConnectionMasks, [In] int bRecursive, out IntPtr ppErrors, out DXGeneralResponse pResponse);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void CopyDXConnectionDefaultAttributes([In] int bConfigToStatus, [In, MarshalAs(UnmanagedType.LPWStr)] string szBrowsePath, [In] int dwNoOfMasks, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=2)] DXConnection[] pDXConnectionMasks, [In] int bRecursive, out IntPtr ppErrors, out DXGeneralResponse pResponse);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void ResetConfiguration([In, MarshalAs(UnmanagedType.LPWStr)] string szConfigurationVersion, [MarshalAs(UnmanagedType.LPWStr)] out string pszConfigurationVersion);
    }
}

