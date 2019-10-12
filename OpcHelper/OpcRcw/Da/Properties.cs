namespace Jund.OpcHelper.OpcRcw.Da
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class Properties
    {
        public const int OPC_PROPERTY_ACCESS_RIGHTS = 5;
        public const int OPC_PROPERTY_ALARM_AREA_LIST = 0x12e;
        public const int OPC_PROPERTY_ALARM_QUICK_HELP = 0x12d;
        public const int OPC_PROPERTY_CHANGE_RATE_LIMIT = 0x137;
        public const int OPC_PROPERTY_CLOSE_LABEL = 0x6a;
        public const int OPC_PROPERTY_CONDITION_LOGIC = 0x130;
        public const int OPC_PROPERTY_CONDITION_STATUS = 300;
        public const int OPC_PROPERTY_CONSISTENCY_WINDOW = 0x25d;
        public const int OPC_PROPERTY_DATA_FILTER_VALUE = 0x261;
        public const int OPC_PROPERTY_DATATYPE = 1;
        public const int OPC_PROPERTY_DEADBAND = 0x132;
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_ACCESS_RIGHTS = "Item Access Rights";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_ALARM_AREA_LIST = "Alarm Area List";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_ALARM_QUICK_HELP = "Alarm Quick Help";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_CHANGE_RATE_LIMIT = "Rate of Change Limit";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_CLOSE_LABEL = "Contact Close Label";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_CONDITION_LOGIC = "Condition Logic";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_CONDITION_STATUS = "Condition Status";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_CONSISTENCY_WINDOW = "Consistency Window";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_DATA_FILTER_VALUE = "Data Filter Value";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_DATATYPE = "Item Canonical Data Type";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_DEADBAND = "Deadband";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_DESCRIPTION = "Item Description";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_DEVIATION_LIMIT = "Deviation Limit";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_DICTIONARY = "Dictionary";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_DICTIONARY_ID = "Dictionary ID";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_EU_INFO = "Item EU Info";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_EU_TYPE = "Item EU Type";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_EU_UNITS = "EU Units";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_HI_LIMIT = "Hi Limit";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_HIGH_EU = "High EU";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_HIGH_IR = "High Instrument Range";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_HIHI_LIMIT = "HiHi Limit";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_LIMIT_EXCEEDED = "Limit Exceeded";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_LO_LIMIT = "Lo Limit";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_LOLO_LIMIT = "LoLo Limit";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_LOW_EU = "Low EU";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_LOW_IR = "Low Instrument Range";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_OPEN_LABEL = "Contact Open Label";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_PRIMARY_ALARM_AREA = "Primary Alarm Area";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_QUALITY = "Item Quality";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_SCAN_RATE = "Server Scan Rate";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_SOUND_FILE = "Sound File";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_TIMESTAMP = "Item Timestamp";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_TIMEZONE = "Item Timezone";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_TYPE_DESCRIPTION = "Type Description";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_TYPE_ID = "Type ID";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_TYPE_SYSTEM_ID = "Type System ID";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_UNCONVERTED_ITEM_ID = "Unconverted Item ID";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_UNFILTERED_ITEM_ID = "Unfiltered Item ID";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_VALUE = "Item Value";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_PROPERTY_DESC_WRITE_BEHAVIOR = "Write Behavior";
        public const int OPC_PROPERTY_DESCRIPTION = 0x65;
        public const int OPC_PROPERTY_DEVIATION_LIMIT = 0x138;
        public const int OPC_PROPERTY_DICTIONARY = 0x25b;
        public const int OPC_PROPERTY_DICTIONARY_ID = 0x259;
        public const int OPC_PROPERTY_EU_INFO = 8;
        public const int OPC_PROPERTY_EU_TYPE = 7;
        public const int OPC_PROPERTY_EU_UNITS = 100;
        public const int OPC_PROPERTY_HI_LIMIT = 0x134;
        public const int OPC_PROPERTY_HIGH_EU = 0x66;
        public const int OPC_PROPERTY_HIGH_IR = 0x68;
        public const int OPC_PROPERTY_HIHI_LIMIT = 0x133;
        public const int OPC_PROPERTY_LIMIT_EXCEEDED = 0x131;
        public const int OPC_PROPERTY_LO_LIMIT = 0x135;
        public const int OPC_PROPERTY_LOLO_LIMIT = 310;
        public const int OPC_PROPERTY_LOW_EU = 0x67;
        public const int OPC_PROPERTY_LOW_IR = 0x69;
        public const int OPC_PROPERTY_OPEN_LABEL = 0x6b;
        public const int OPC_PROPERTY_PRIMARY_ALARM_AREA = 0x12f;
        public const int OPC_PROPERTY_QUALITY = 3;
        public const int OPC_PROPERTY_SCAN_RATE = 6;
        public const int OPC_PROPERTY_SOUND_FILE = 0x139;
        public const int OPC_PROPERTY_TIMESTAMP = 4;
        public const int OPC_PROPERTY_TIMEZONE = 0x6c;
        public const int OPC_PROPERTY_TYPE_DESCRIPTION = 0x25c;
        public const int OPC_PROPERTY_TYPE_ID = 0x25a;
        public const int OPC_PROPERTY_TYPE_SYSTEM_ID = 600;
        public const int OPC_PROPERTY_UNCONVERTED_ITEM_ID = 0x25f;
        public const int OPC_PROPERTY_UNFILTERED_ITEM_ID = 0x260;
        public const int OPC_PROPERTY_VALUE = 2;
        public const int OPC_PROPERTY_WRITE_BEHAVIOR = 0x25e;
    }
}

