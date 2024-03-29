﻿namespace Jund.OpcHelper.OpcRcw.Comn
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType((short) 1), Guid("55C382C8-21C7-4E88-96C1-BECFB1E3F483")]
    public interface IOPCEnumGUID
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Next([In] int celt, [Out, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct)] Guid[] rgelt, out int pceltFetched);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Skip([In] int celt);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Reset();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void Clone([MarshalAs(UnmanagedType.Interface)] out IOPCEnumGUID ppenum);
    }
}

