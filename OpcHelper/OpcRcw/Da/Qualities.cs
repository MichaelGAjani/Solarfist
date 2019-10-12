namespace Jund.OpcHelper.OpcRcw.Da
{
    using System;

    public abstract class Qualities
    {
        public const short OPC_LIMIT_CONST = 3;
        public const short OPC_LIMIT_HIGH = 2;
        public const short OPC_LIMIT_LOW = 1;
        public const short OPC_LIMIT_MASK = 3;
        public const short OPC_LIMIT_OK = 0;
        public const short OPC_QUALITY_BAD = 0;
        public const short OPC_QUALITY_COMM_FAILURE = 0x18;
        public const short OPC_QUALITY_CONFIG_ERROR = 4;
        public const short OPC_QUALITY_DEVICE_FAILURE = 12;
        public const short OPC_QUALITY_EGU_EXCEEDED = 0x54;
        public const short OPC_QUALITY_GOOD = 0xc0;
        public const short OPC_QUALITY_LAST_KNOWN = 20;
        public const short OPC_QUALITY_LAST_USABLE = 0x44;
        public const short OPC_QUALITY_LOCAL_OVERRIDE = 0xd8;
        public const short OPC_QUALITY_MASK = 0xc0;
        public const short OPC_QUALITY_NOT_CONNECTED = 8;
        public const short OPC_QUALITY_OUT_OF_SERVICE = 0x1c;
        public const short OPC_QUALITY_SENSOR_CAL = 80;
        public const short OPC_QUALITY_SENSOR_FAILURE = 0x10;
        public const short OPC_QUALITY_SUB_NORMAL = 0x58;
        public const short OPC_QUALITY_UNCERTAIN = 0x40;
        public const short OPC_QUALITY_WAITING_FOR_INITIAL_DATA = 0x20;
        public const short OPC_STATUS_MASK = 0xfc;
    }
}

