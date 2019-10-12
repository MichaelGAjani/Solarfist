namespace Jund.OpcHelper.OpcRcw.Ae
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class Constants
    {
        public const int ALL_EVENTS = 7;
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CATEGORY_DESCRIPTION_AE10 = "OPC Alarm & Event Server Version 1.0";
        public const int CHANGE_ACK_STATE = 2;
        public const int CHANGE_ACTIVE_STATE = 1;
        public const int CHANGE_ATTRIBUTE = 0x80;
        public const int CHANGE_ENABLE_STATE = 4;
        public const int CHANGE_MESSAGE = 0x40;
        public const int CHANGE_QUALITY = 8;
        public const int CHANGE_SEVERITY = 0x10;
        public const int CHANGE_SUBCONDITION = 0x20;
        public const int CONDITION_ACKED = 4;
        public const int CONDITION_ACTIVE = 2;
        public const int CONDITION_ENABLED = 1;
        public const int CONDITION_EVENT = 4;
        public const int FILTER_BY_AREA = 8;
        public const int FILTER_BY_CATEGORY = 2;
        public const int FILTER_BY_EVENT = 1;
        public const int FILTER_BY_SEVERITY = 4;
        public const int FILTER_BY_SOURCE = 0x10;
        public const int SIMPLE_EVENT = 1;
        public const int TRACKING_EVENT = 2;
    }
}

