﻿namespace Jund.OpcHelper.OpcCom
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("50E8496C-FA60-46a4-AF72-512494C664C6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCWrappedServer
    {
        void Load([MarshalAs(UnmanagedType.LPStruct)] Guid clsid);
        void Unload();
    }
}

