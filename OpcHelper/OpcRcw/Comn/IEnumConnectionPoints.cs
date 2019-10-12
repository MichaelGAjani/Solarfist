﻿namespace Jund.OpcHelper.OpcRcw.Comn
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("B196B285-BAB4-101A-B69C-00AA00341D07"), InterfaceType((short) 1)]
    public interface IEnumConnectionPoints
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void RemoteNext([In] int cConnections, [Out, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Interface)] IConnectionPoint[] ppCP, out int pcFetched);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Skip([In] int cConnections);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Reset();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Clone([MarshalAs(UnmanagedType.Interface)] out IEnumConnectionPoints ppenum);
    }
}

