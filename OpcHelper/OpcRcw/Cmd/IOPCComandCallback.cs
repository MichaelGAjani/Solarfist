namespace Jund.OpcHelper.OpcRcw.Cmd
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport, Guid("3104B527-2016-442D-9696-1275DE978778"), InterfaceType((short) 1)]
    public interface IOPCComandCallback
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
        void OnStateChange([In] int dwNoOfEvents, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)] StateChangeEvent[] pEvents, [In] int dwNoOfPermittedControls, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=2)] string[] pszPermittedControls, [In] int bNoStateChange);
    }
}

