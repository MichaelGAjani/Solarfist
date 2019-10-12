namespace Jund.OpcHelper.OpcRcw.Ae
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("6516885F-5783-11D1-84A0-00608CB8A7E9"), InterfaceType((short) 1)]
    public interface IOPCEventSink
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void OnEvent([In] int hClientSubscription, [In] int bRefresh, [In] int bLastRefresh, [In] int dwCount, [In, ComAliasName("OpcAeRcw.ONEVENTSTRUCT"), MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=3)] ONEVENTSTRUCT[] pEvents);
    }
}

