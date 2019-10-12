namespace Jund.OpcHelper.OpcCom.Ae
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct ResultIDs
    {
        public const int S_ALREADYACKED = 0x40200;
        public const int S_INVALIDBUFFERTIME = 0x40201;
        public const int S_INVALIDMAXSIZE = 0x40202;
        public const int S_INVALIDKEEPALIVETIME = 0x40203;
        public const int E_INVALIDBRANCHNAME = -1073479165;
        public const int E_INVALIDTIME = -1073479164;
        public const int E_BUSY = -1073479163;
        public const int E_NOINFO = -1073479162;
    }
}

