namespace Jund.OpcHelper.OpcRcw.Da
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType((short) 1), Guid("8E368666-D72E-4F78-87ED-647611C61C9F")]
    public interface IOPCGroupStateMgt2 : IOPCGroupStateMgt
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetState(out int pUpdateRate, out int pActive, [MarshalAs(UnmanagedType.LPWStr)] out string ppName, out int pTimeBias, out float pPercentDeadband, out int pLCID, out int phClientGroup, out int phServerGroup);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void SetState([In] IntPtr pRequestedUpdateRate, out int pRevisedUpdateRate, [In] IntPtr pActive, [In] IntPtr pTimeBias, [In] IntPtr pPercentDeadband, [In] IntPtr pLCID, [In] IntPtr phClientGroup);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void SetName([In, MarshalAs(UnmanagedType.LPWStr)] string szName);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void CloneGroup([In, MarshalAs(UnmanagedType.LPWStr)] string szName, [In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void SetKeepAlive([In] int dwKeepAliveTime, out int pdwRevisedKeepAliveTime);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetKeepAlive(out int pdwKeepAliveTime);
    }
}

