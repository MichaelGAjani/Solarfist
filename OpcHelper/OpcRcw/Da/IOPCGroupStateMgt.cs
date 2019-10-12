namespace Jund.OpcHelper.OpcRcw.Da
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType((short) 1), Guid("39C13A50-011E-11D0-9675-0020AFD8ADB3")]
    public interface IOPCGroupStateMgt
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void GetState(out int pUpdateRate, out int pActive, [MarshalAs(UnmanagedType.LPWStr)] out string ppName, out int pTimeBias, out float pPercentDeadband, out int pLCID, out int phClientGroup, out int phServerGroup);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void SetState([In] IntPtr pRequestedUpdateRate, out int pRevisedUpdateRate, [In] IntPtr pActive, [In] IntPtr pTimeBias, [In] IntPtr pPercentDeadband, [In] IntPtr pLCID, [In] IntPtr phClientGroup);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void SetName([In, MarshalAs(UnmanagedType.LPWStr)] string szName);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void CloneGroup([In, MarshalAs(UnmanagedType.LPWStr)] string szName, [In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
    }
}

