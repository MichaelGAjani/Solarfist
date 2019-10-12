namespace Jund.OpcHelper.OpcRcw.Comn
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("B196B284-BAB4-101A-B69C-00AA00341D07"), InterfaceType((short) 1)]
    public interface IConnectionPointContainer
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void EnumConnectionPoints([MarshalAs(UnmanagedType.Interface)] out IEnumConnectionPoints ppenum);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void FindConnectionPoint([In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IConnectionPoint ppCP);
    }
}

