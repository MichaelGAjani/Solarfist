namespace Jund.OpcHelper.OpcRcw.Hda
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class Constants
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_CATEGORY_DESCRIPTION_HDA10 = "OPC History Data Access Servers Version 1.0";
        public const int OPCHDA_ARCHIVING = 5;
        public const int OPCHDA_CALCULATED = 0x80000;
        public const int OPCHDA_CONVERSION = 0x800000;
        public const int OPCHDA_DATA_TYPE = 1;
        public const int OPCHDA_DATALOST = 0x400000;
        public const int OPCHDA_DERIVE_EQUATION = 6;
        public const int OPCHDA_DESCRIPTION = 2;
        public const int OPCHDA_ENG_UNITS = 3;
        public const int OPCHDA_EXCEPTION_DEV = 0x10;
        public const int OPCHDA_EXCEPTION_DEV_TYPE = 0x11;
        public const int OPCHDA_EXTRADATA = 0x10000;
        public const int OPCHDA_HIGH_ENTRY_LIMIT = 0x12;
        public const int OPCHDA_INTERPOLATED = 0x20000;
        public const int OPCHDA_ITEMID = 13;
        public const int OPCHDA_LOW_ENTRY_LIMIT = 0x13;
        public const int OPCHDA_MAX_TIME_INT = 14;
        public const int OPCHDA_MIN_TIME_INT = 15;
        public const int OPCHDA_NOBOUND = 0x100000;
        public const int OPCHDA_NODATA = 0x200000;
        public const int OPCHDA_NODE_NAME = 7;
        public const int OPCHDA_NORMAL_MAXIMUM = 11;
        public const int OPCHDA_NORMAL_MINIMUM = 12;
        public const int OPCHDA_PROCESS_NAME = 8;
        public const int OPCHDA_RAW = 0x40000;
        public const int OPCHDA_SOURCE_NAME = 9;
        public const int OPCHDA_SOURCE_TYPE = 10;
        public const int OPCHDA_STEPPED = 4;
    }
}

