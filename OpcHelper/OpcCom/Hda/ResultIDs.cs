namespace Jund.OpcHelper.OpcCom.Hda
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct ResultIDs
    {
        public const int E_MAXEXCEEDED = -1073475583;
        public const int S_NODATA = 0x40041002;
        public const int S_MOREDATA = 0x40041003;
        public const int E_INVALIDAGGREGATE = -1073475580;
        public const int S_CURRENTVALUE = 0x40041005;
        public const int S_EXTRADATA = 0x40041006;
        public const int W_NOFILTER = -2147217401;
        public const int E_UNKNOWNATTRID = -1073475576;
        public const int E_NOT_AVAIL = -1073475575;
        public const int E_INVALIDDATATYPE = -1073475574;
        public const int E_DATAEXISTS = -1073475573;
        public const int E_INVALIDATTRID = -1073475572;
        public const int E_NODATAEXISTS = -1073475571;
        public const int S_INSERTED = 0x4004100e;
        public const int S_REPLACED = 0x4004100f;
    }
}

