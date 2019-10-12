namespace Jund.OpcHelper.OpcRcw.Dx
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class QualityStatusName
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUALITY_BAD = "bad";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUALITY_BAD_COMM_FAILURE = "badCommFailure";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUALITY_BAD_CONFIG_ERROR = "badConfigurationError";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUALITY_BAD_DEVICE_FAILURE = "badDeviceFailure";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUALITY_BAD_LAST_KNOWN_VALUE = "badLastKnownValue";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUALITY_BAD_NOT_CONNECTED = "badNotConnected";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUALITY_BAD_OUT_OF_SERVICE = "badOutOfService";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUALITY_BAD_SENSOR_FAILURE = "badSensorFailure";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUALITY_GOOD = "good";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUALITY_GOOD_LOCAL_OVERRIDE = "goodLocalOverride";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUALITY_UNCERTAIN = "uncertain";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUALITY_UNCERTAIN_EU_EXCEEDED = "uncertainEUExceeded";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUALITY_UNCERTAIN_LAST_USABLE_VALUE = "uncertainLastUsableValue";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUALITY_UNCERTAIN_SENSOR_NOT_ACCURATE = "uncertainSensorNotAccurate";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUALITY_UNCERTAIN_SUB_NORMAL = "uncertainSubNormal";
    }
}

