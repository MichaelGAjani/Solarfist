namespace Jund.OpcHelper.Opc.Da
{
    using System;

    public class Property
    {
        public static readonly PropertyID ACCESSRIGHTS = new PropertyID("accessRights", 5, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID ALARM_AREA_LIST = new PropertyID("alarmAreaList", 0x12e, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID ALARM_QUICK_HELP = new PropertyID("alarmQuickHelp", 0x12d, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID CLOSELABEL = new PropertyID("closeLabel", 0x6a, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID CONDITION_LOGIC = new PropertyID("conditionLogic", 0x130, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID CONDITION_STATUS = new PropertyID("conditionStatus", 300, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID CONSISTENCY_WINDOW = new PropertyID("consistencyWindow", 0x25d, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID DATA_FILTER_VALUE = new PropertyID("dataFilterValue", 0x261, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID DATATYPE = new PropertyID("dataType", 1, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID DEADBAND = new PropertyID("deadband", 0x132, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID DESCRIPTION = new PropertyID("description", 0x65, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID DEVIATION_LIMIT = new PropertyID("deviationLimit", 0x138, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID DICTIONARY = new PropertyID("dictionary", 0x25b, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID DICTIONARY_ID = new PropertyID("dictionaryID", 0x259, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID ENGINEERINGUINTS = new PropertyID("engineeringUnits", 100, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID EUINFO = new PropertyID("euInfo", 8, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID EUTYPE = new PropertyID("euType", 7, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID HI_LIMIT = new PropertyID("hiLimit", 0x134, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID HIGHEU = new PropertyID("highEU", 0x66, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID HIGHIR = new PropertyID("highIR", 0x68, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID HIHI_LIMIT = new PropertyID("hihiLimit", 0x133, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID LIMIT_EXCEEDED = new PropertyID("limitExceeded", 0x131, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID LO_LIMIT = new PropertyID("loLimit", 0x135, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID LOLO_LIMIT = new PropertyID("loloLimit", 310, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID LOWEU = new PropertyID("lowEU", 0x67, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID LOWIR = new PropertyID("lowIR", 0x69, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID MAXIMUM_VALUE = new PropertyID("maximumValue", 110, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID MINIMUM_VALUE = new PropertyID("minimumValue", 0x6d, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID OPENLABEL = new PropertyID("openLabel", 0x6b, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID PRIMARY_ALARM_AREA = new PropertyID("primaryAlarmArea", 0x12f, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID QUALITY = new PropertyID("quality", 3, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID RATE_CHANGE_LIMIT = new PropertyID("rangeOfChangeLimit", 0x137, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID SCANRATE = new PropertyID("scanRate", 6, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID SOUNDFILE = new PropertyID("soundFile", 0x139, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID TIMESTAMP = new PropertyID("timestamp", 4, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID TIMEZONE = new PropertyID("timeZone", 0x6c, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID TYPE_DESCRIPTION = new PropertyID("typeDescription", 0x25c, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID TYPE_ID = new PropertyID("typeID", 0x25a, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID TYPE_SYSTEM_ID = new PropertyID("typeSystemID", 600, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID UNCONVERTED_ITEM_ID = new PropertyID("unconvertedItemID", 0x25f, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID UNFILTERED_ITEM_ID = new PropertyID("unfilteredItemID", 0x260, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID VALUE = new PropertyID("value", 2, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID VALUE_PRECISION = new PropertyID("valuePrecision", 0x6f, "http://opcfoundation.org/DataAccess/");
        public static readonly PropertyID WRITE_BEHAVIOR = new PropertyID("writeBehavior", 0x25e, "http://opcfoundation.org/DataAccess/");
    }
}

