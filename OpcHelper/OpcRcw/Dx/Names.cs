namespace Jund.OpcHelper.OpcRcw.Dx
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class Names
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string ACTUAL_UPDATE_RATE = "ActualUpdateRate";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONFIGURATION_VERSION = "ConfigurationVersion";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECT_FAIL_COUNT = "ConnectFailCount";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECTION_BROWSE_PATHS = "BrowsePath";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECTION_COUNT = "DXConnectionCount";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECTION_DESCRIPTION = "Description";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECTION_KEYWORD = "Keyword";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECTION_NAME = "Name";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECTION_SOURCE_SERVER_NAME = "SourceServerName";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECTION_STATE = "DXConnectionState";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECTION_STATUS = "Status";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECTION_STATUS_TYPE = "DXConnectionStatus";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECTION_TYPE = "DXConnection";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECTION_VERSION = "Version";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string CONNECTIONS_ROOT = "DXConnectionsRoot";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string DATABASE_ROOT = "DX";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string DEADBAND = "Deadband";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string DEFAULT_OVERRIDDEN = "DefaultOverridden";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string DEFAULT_OVERRIDE_VALUE = "DefaultOverrideValue";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string DEFAULT_SOURCE_ITEM_CONNECTED = "DefaultSourceItemConnected";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string DEFAULT_SOURCE_SERVER_CONNECTED = "DefaultSourceServerConnected";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string DEFAULT_TARGET_ITEM_CONNECTED = "DefaultTargetItemConnected";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string DIRTY_FLAG = "DirtyFlag";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string ENABLE_SUBSTITUTE_VALUE = "EnableSubstituteValue";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string ERROR = "OPCError";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string ERROR_ID = "ID";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string ERROR_TEXT = "Text";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string ITEM_NAME = "ItemName";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string ITEM_PATH = "ItemPath";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string LAST_CONNECT_FAIL_TIMESTAMP = "LastConnectFailTimestamp";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string LAST_CONNECT_TIMESTAMP = "LastConnectTimestamp";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string LAST_DATA_CHANGE_TIMESTAMP = "LastDataChangeTimestamp";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string LIMIT_BITS = "LimitBits";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string MAX_CONNECTIONS = "MaxDXConnections";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string MAX_QUEUE_SIZE = "MaxQueueSize";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string NAMESPACE_V10 = "http://opcfoundation.org/webservices/OPCDX/10";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OPC_CATEGORY_DESCRIPTION_DX10 = "OPC Data Exchange Servers Version 1.0";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OVERRIDDEN = "Overridden";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string OVERRIDE_VALUE = "OverrideValue";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string PING_TIME = "PingTime";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUALITY = "DXQuality";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUALITY_STATUS = "Quality";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUEUE_FLUSH_COUNT = "QueueFlushCount";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string QUEUE_HIGH_WATER_MARK = "QueueHighWaterMark";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SEPARATOR = "/";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_CONNECT_STATUS = "ConnectStatus";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_ERROR_DIAGNOSTIC = "ErrorDiagnostic";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_ERROR_ID = "ErrorID";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_STATE = "ServerState";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_STATUS = "ServerStatus";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_STATUS_TYPE = "DXServerStatus";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_TYPE = "ServerType";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SERVER_URL = "ServerURL";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_ERROR_DIAGNOSTIC = "SourceErrorDiagnostic";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_ERROR_ID = "SourceErrorID";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_ITEM_CONNECTED = "SourceItemConnected";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_ITEM_NAME = "SourceItemName";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_ITEM_PATH = "SourceItemPath";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_ITEM_QUEUE_SIZE = "QueueSize";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_QUALITY = "SourceQuality";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_SERVER_CONNECTED = "SourceServerConnected";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_SERVER_DESCRIPTION = "Description";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_SERVER_ERROR_DIAGNOSTIC = "ErrorDiagnostic";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_SERVER_ERROR_ID = "ErrorID";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_SERVER_NAME = "Name";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_SERVER_STATUS = "Status";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_SERVER_STATUS_TYPE = "DXSourceServerStatus";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_SERVER_TYPE = "SourceServer";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_SERVER_TYPES = "SourceServerTypes";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_SERVER_URL_SCHEME_OPCDA = "opcda";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_SERVER_URL_SCHEME_XMLDA = "http";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_SERVER_VERSION = "Version";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_SERVERS_ROOT = "SourceServers";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_TIMESTAMP = "SourceTimestamp";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SOURCE_VALUE = "SourceValue";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string SUBSTITUTE_VALUE = "SubstituteValue";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string TARGET_ITEM_CONNECTED = "TargetItemConnected";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string TARGET_ITEM_NAME = "TargetItemName";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string TARGET_ITEM_PATH = "TargetItemPath";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string UPDATE_RATE = "UpdateRate";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string VENDOR_BITS = "VendorBits";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string VENDOR_DATA = "VendorData";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string VERSION = "Version";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string WRITE_ERROR_DIAGNOSTIC = "WriteErrorDiagnostic";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string WRITE_ERROR_ID = "WriteErrorID";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string WRITE_QUALITY = "WriteQuality";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string WRITE_TIMESTAMP = "WriteTimestamp";
        [MarshalAs(UnmanagedType.LPWStr)]
        public const string WRITE_VALUE = "WriteValue";
    }
}

