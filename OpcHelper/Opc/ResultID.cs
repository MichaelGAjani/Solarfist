﻿namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Xml;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct ResultID : ISerializable
    {
        private XmlQualifiedName m_name;
        private int m_code;
        public static readonly ResultID S_OK;
        public static readonly ResultID S_FALSE;
        public static readonly ResultID E_FAIL;
        public static readonly ResultID E_INVALIDARG;
        public static readonly ResultID E_TIMEDOUT;
        public static readonly ResultID E_OUTOFMEMORY;
        public static readonly ResultID E_NETWORK_ERROR;
        public static readonly ResultID E_ACCESS_DENIED;
        public static readonly ResultID E_NOTSUPPORTED;
        private ResultID(SerializationInfo info, StreamingContext context)
        {
            string name = (string) info.GetValue("NA", typeof(string));
            string ns = (string) info.GetValue("NS", typeof(string));
            this.m_name = new XmlQualifiedName(name, ns);
            this.m_code = (int) info.GetValue("CO", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (this.m_name != null)
            {
                info.AddValue("NA", this.m_name.Name);
                info.AddValue("NS", this.m_name.Namespace);
            }
            info.AddValue("CO", this.m_code);
        }

        public XmlQualifiedName Name
        {
            get
            {
                return this.m_name;
            }
        }
        public int Code
        {
            get
            {
                return this.m_code;
            }
        }
        public static bool operator ==(ResultID a, ResultID b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ResultID a, ResultID b)
        {
            return !a.Equals(b);
        }

        public bool Succeeded()
        {
            if (this.Code != -1)
            {
                return (this.Code >= 0);
            }
            return ((this.Name != null) && this.Name.Name.StartsWith("S_"));
        }

        public bool Failed()
        {
            if (this.Code != -1)
            {
                return (this.Code < 0);
            }
            return ((this.Name != null) && this.Name.Name.StartsWith("E_"));
        }

        public ResultID(XmlQualifiedName name)
        {
            this.m_name = name;
            this.m_code = -1;
        }

        public ResultID(long code)
        {
            this.m_name = null;
            if (code > 0x7fffffffL)
            {
                code = -(0x100000000L - code);
            }
            this.m_code = (int) code;
        }

        public ResultID(string name, string ns)
        {
            this.m_name = new XmlQualifiedName(name, ns);
            this.m_code = -1;
        }

        public ResultID(ResultID resultID, long code)
        {
            this.m_name = resultID.Name;
            if (code > 0x7fffffffL)
            {
                code = -(0x100000000L - code);
            }
            this.m_code = (int) code;
        }

        public override bool Equals(object target)
        {
            if ((target != null) && (target.GetType() == typeof(ResultID)))
            {
                ResultID tid = (ResultID) target;
                if ((tid.Code != -1) && (this.Code != -1))
                {
                    return ((tid.Code == this.Code) && (tid.Name == this.Name));
                }
                if ((tid.Name != null) && (this.Name != null))
                {
                    return (tid.Name == this.Name);
                }
            }
            return false;
        }

        public override string ToString()
        {
            if (this.Name != null)
            {
                return this.Name.Name;
            }
            return string.Format("0x{0,0:X}", this.Code);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        static ResultID()
        {
            S_OK = new ResultID("S_OK", "http://opcfoundation.org/DataAccess/");
            S_FALSE = new ResultID("S_FALSE", "http://opcfoundation.org/DataAccess/");
            E_FAIL = new ResultID("E_FAIL", "http://opcfoundation.org/DataAccess/");
            E_INVALIDARG = new ResultID("E_INVALIDARG", "http://opcfoundation.org/DataAccess/");
            E_TIMEDOUT = new ResultID("E_TIMEDOUT", "http://opcfoundation.org/DataAccess/");
            E_OUTOFMEMORY = new ResultID("E_OUTOFMEMORY", "http://opcfoundation.org/DataAccess/");
            E_NETWORK_ERROR = new ResultID("E_NETWORK_ERROR", "http://opcfoundation.org/DataAccess/");
            E_ACCESS_DENIED = new ResultID("E_ACCESS_DENIED", "http://opcfoundation.org/DataAccess/");
            E_NOTSUPPORTED = new ResultID("E_NOTSUPPORTED", "http://opcfoundation.org/DataAccess/");
        }
        public class Ae
        {
            public static readonly ResultID E_BUSY = new ResultID("E_BUSY", "http://opcfoundation.org/AlarmAndEvents/");
            public static readonly ResultID E_INVALIDBRANCHNAME = new ResultID("E_INVALIDBRANCHNAME", "http://opcfoundation.org/AlarmAndEvents/");
            public static readonly ResultID E_INVALIDTIME = new ResultID("E_INVALIDTIME", "http://opcfoundation.org/AlarmAndEvents/");
            public static readonly ResultID E_NOINFO = new ResultID("E_NOINFO", "http://opcfoundation.org/AlarmAndEvents/");
            public static readonly ResultID S_ALREADYACKED = new ResultID("S_ALREADYACKED", "http://opcfoundation.org/AlarmAndEvents/");
            public static readonly ResultID S_INVALIDBUFFERTIME = new ResultID("S_INVALIDBUFFERTIME", "http://opcfoundation.org/AlarmAndEvents/");
            public static readonly ResultID S_INVALIDKEEPALIVETIME = new ResultID("S_INVALIDKEEPALIVETIME", "http://opcfoundation.org/AlarmAndEvents/");
            public static readonly ResultID S_INVALIDMAXSIZE = new ResultID("S_INVALIDMAXSIZE", "http://opcfoundation.org/AlarmAndEvents/");
        }

        public class Cpx
        {
            public static readonly ResultID E_FILTER_DUPLICATE = new ResultID("E_FILTER_DUPLICATE", "http://opcfoundation.org/ComplexData/");
            public static readonly ResultID E_FILTER_ERROR = new ResultID("E_FILTER_ERROR", "http://opcfoundation.org/ComplexData/");
            public static readonly ResultID E_FILTER_INVALID = new ResultID("E_FILTER_INVALID", "http://opcfoundation.org/ComplexData/");
            public static readonly ResultID E_TYPE_CHANGED = new ResultID("E_TYPE_CHANGED", "http://opcfoundation.org/ComplexData/");
            public static readonly ResultID S_FILTER_NO_DATA = new ResultID("S_FILTER_NO_DATA", "http://opcfoundation.org/ComplexData/");
        }

        public class Da
        {
            public static readonly ResultID E_BADTYPE = new ResultID("E_BADTYPE", "http://opcfoundation.org/DataAccess/");
            public static readonly ResultID E_INVALID_FILTER = new ResultID("E_INVALID_FILTER", "http://opcfoundation.org/DataAccess/");
            public static readonly ResultID E_INVALID_ITEM_NAME = new ResultID("E_INVALID_ITEM_NAME", "http://opcfoundation.org/DataAccess/");
            public static readonly ResultID E_INVALID_ITEM_PATH = new ResultID("E_INVALID_ITEM_PATH", "http://opcfoundation.org/DataAccess/");
            public static readonly ResultID E_INVALID_PID = new ResultID("E_INVALID_PID", "http://opcfoundation.org/DataAccess/");
            public static readonly ResultID E_INVALIDCONTINUATIONPOINT = new ResultID("E_INVALIDCONTINUATIONPOINT", "http://opcfoundation.org/DataAccess/");
            public static readonly ResultID E_INVALIDHANDLE = new ResultID("E_INVALIDHANDLE", "http://opcfoundation.org/DataAccess/");
            public static readonly ResultID E_NO_ITEM_BUFFERING = new ResultID("E_NO_ITEM_BUFFERING", "http://opcfoundation.org/DataAccess/");
            public static readonly ResultID E_NO_ITEM_DEADBAND = new ResultID("E_NO_ITEM_DEADBAND", "http://opcfoundation.org/DataAccess/");
            public static readonly ResultID E_NO_ITEM_SAMPLING = new ResultID("E_NO_ITEM_SAMPLING", "http://opcfoundation.org/DataAccess/");
            public static readonly ResultID E_NO_WRITEQT = new ResultID("E_NO_WRITEQT", "http://opcfoundation.org/DataAccess/");
            public static readonly ResultID E_RANGE = new ResultID("E_RANGE", "http://opcfoundation.org/DataAccess/");
            public static readonly ResultID E_READONLY = new ResultID("E_READONLY", "http://opcfoundation.org/DataAccess/");
            public static readonly ResultID E_UNKNOWN_ITEM_NAME = new ResultID("E_UNKNOWN_ITEM_NAME", "http://opcfoundation.org/DataAccess/");
            public static readonly ResultID E_UNKNOWN_ITEM_PATH = new ResultID("E_UNKNOWN_ITEM_PATH", "http://opcfoundation.org/DataAccess/");
            public static readonly ResultID E_WRITEONLY = new ResultID("E_WRITEONLY", "http://opcfoundation.org/DataAccess/");
            public static readonly ResultID S_CLAMP = new ResultID("S_CLAMP", "http://opcfoundation.org/DataAccess/");
            public static readonly ResultID S_DATAQUEUEOVERFLOW = new ResultID("S_DATAQUEUEOVERFLOW", "http://opcfoundation.org/DataAccess/");
            public static readonly ResultID S_UNSUPPORTEDRATE = new ResultID("S_UNSUPPORTEDRATE", "http://opcfoundation.org/DataAccess/");
        }

        public class Dx
        {
            public static readonly ResultID E_CONNECTIONS_EXIST = new ResultID("E_CONNECTIONS_EXIST", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_DUPLICATE_NAME = new ResultID("E_DUPLICATE_NAME", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_INVALID_BROWSE_PATH = new ResultID("E_INVALID_BROWSE_PATH", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_INVALID_CONFIG_FILE = new ResultID("E_INVALID_CONFIG_FILE", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_INVALID_DEADBAND = new ResultID("E_INVALID_DEADBAND", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_INVALID_ITEM_NAME = new ResultID("E_INVALID_ITEM_NAME", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_INVALID_ITEM_PATH = new ResultID("E_INVALID_ITEM_PATH", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_INVALID_NAME = new ResultID("E_INVALID_NAME", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_INVALID_QUEUE_SIZE = new ResultID("E_INVALID_QUEUE_SIZE", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_INVALID_SERVER_TYPE = new ResultID("E_INVALID_SERVER_TYPE", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_INVALID_SERVER_URL = new ResultID("E_INVALID_SERVER_URL", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_INVALID_SOURCE_ITEM = new ResultID("E_INVALID_SOURCE_ITEM", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_INVALID_TARGET_ITEM = new ResultID("E_INVALID_TARGET_ITEM", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_NOITEMLIST = new ResultID("E_NOITEMLIST", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_OVERRIDE_BADTYPE = new ResultID("E_OVERRIDE_BADTYPE", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_OVERRIDE_RANGE = new ResultID("E_OVERRIDE_RANGE", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_PERSIST_FAILED = new ResultID("E_PERSIST_FAILED", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_PERSISTING = new ResultID("E_PERSISTING", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_SERVER_STATE = new ResultID("E_SERVER_STATE", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_SOURCE_ITEM_BAD_QUALITY = new ResultID("E_SOURCE_ITEM_BAD_QUALITY", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_SOURCE_ITEM_BADRIGHTS = new ResultID("E_SOURCE_ITEM_BADRIGHTS", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_SOURCE_ITEM_BADTYPE = new ResultID("E_SOURCE_ITEM_BADTYPE", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_SOURCE_ITEM_RANGE = new ResultID("E_SOURCE_ITEM_RANGE", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_SOURCE_SERVER_FAULT = new ResultID("E_SOURCE_SERVER_FAULT", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_SOURCE_SERVER_NO_ACCESSS = new ResultID("E_SOURCE_SERVER_NO_ACCESSS", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_SOURCE_SERVER_NOT_CONNECTED = new ResultID("E_SOURCE_SERVER_NOT_CONNECTED", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_SOURCE_SERVER_TIMEOUT = new ResultID("E_SOURCE_SERVER_TIMEOUT", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_SUBSCRIPTION_FAULT = new ResultID("E_SUBSCRIPTION_FAULT", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_SUBSTITUTE_BADTYPE = new ResultID("E_SUBSTITUTE_BADTYPE", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_SUBSTITUTE_RANGE = new ResultID("E_SUBSTITUTE_RANGE", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_TARGET_ALREADY_CONNECTED = new ResultID("E_TARGET_ALREADY_CONNECTED", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_TARGET_FAULT = new ResultID("E_TARGET_FAULT", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_TARGET_ITEM_BADTYPE = new ResultID("E_TARGET_ITEM_BADTYPE", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_TARGET_ITEM_DISCONNECTED = new ResultID("E_TARGET_ITEM_DISCONNECTED", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_TARGET_ITEM_RANGE = new ResultID("E_TARGET_ITEM_RANGE", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_TARGET_NO_ACCESSS = new ResultID("E_TARGET_NO_ACCESSS", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_TARGET_NO_WRITES_ATTEMPTED = new ResultID("E_TARGET_NO_WRITES_ATTEMPTED", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_TOO_MANY_CONNECTIONS = new ResultID("E_TOO_MANY_CONNECTIONS", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_UNKNOWN_ITEM_NAME = new ResultID("E_UNKNOWN_ITEM_NAME", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_UNKNOWN_ITEM_PATH = new ResultID("E_UNKNOWN_ITEM_PATH", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_UNKNOWN_SERVER_NAME = new ResultID("E_UNKNOWN_SERVER_NAME", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_UNKNOWN_SOURCE_ITEM = new ResultID("E_UNKNOWN_SOURCE_ITEM", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_UNKNOWN_TARGET_ITEM = new ResultID("E_UNKNOWN_TARGET_ITEM", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_UNSUPPORTED_SERVER_TYPE = new ResultID("E_UNSUPPORTED_SERVER_TYPE", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID E_VERSION_MISMATCH = new ResultID("E_VERSION_MISMATCH", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID S_CLAMP = new ResultID("S_CLAMP", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID S_TARGET_OVERRIDEN = new ResultID("S_TARGET_OVERRIDEN", "http://opcfoundation.org/DataExchange/");
            public static readonly ResultID S_TARGET_SUBSTITUTED = new ResultID("S_TARGET_SUBSTITUTED", "http://opcfoundation.org/DataExchange/");
        }

        public class Hda
        {
            public static readonly ResultID E_DATAEXISTS = new ResultID("E_DATAEXISTS", "http://opcfoundation.org/HistoricalDataAccess/");
            public static readonly ResultID E_INVALIDAGGREGATE = new ResultID("E_INVALIDAGGREGATE", "http://opcfoundation.org/HistoricalDataAccess/");
            public static readonly ResultID E_INVALIDATTRID = new ResultID("E_INVALIDATTRID", "http://opcfoundation.org/HistoricalDataAccess/");
            public static readonly ResultID E_INVALIDDATATYPE = new ResultID("E_INVALIDDATATYPE", "http://opcfoundation.org/HistoricalDataAccess/");
            public static readonly ResultID E_MAXEXCEEDED = new ResultID("E_MAXEXCEEDED", "http://opcfoundation.org/HistoricalDataAccess/");
            public static readonly ResultID E_NODATAEXISTS = new ResultID("E_NODATAEXISTS", "http://opcfoundation.org/HistoricalDataAccess/");
            public static readonly ResultID E_NOT_AVAIL = new ResultID("E_NOT_AVAIL", "http://opcfoundation.org/HistoricalDataAccess/");
            public static readonly ResultID E_UNKNOWNATTRID = new ResultID("E_UNKNOWNATTRID", "http://opcfoundation.org/HistoricalDataAccess/");
            public static readonly ResultID S_CURRENTVALUE = new ResultID("S_CURRENTVALUE", "http://opcfoundation.org/HistoricalDataAccess/");
            public static readonly ResultID S_EXTRADATA = new ResultID("S_EXTRADATA", "http://opcfoundation.org/HistoricalDataAccess/");
            public static readonly ResultID S_INSERTED = new ResultID("S_INSERTED", "http://opcfoundation.org/HistoricalDataAccess/");
            public static readonly ResultID S_MOREDATA = new ResultID("S_MOREDATA", "http://opcfoundation.org/HistoricalDataAccess/");
            public static readonly ResultID S_NODATA = new ResultID("S_NODATA", "http://opcfoundation.org/HistoricalDataAccess/");
            public static readonly ResultID S_REPLACED = new ResultID("S_REPLACED", "http://opcfoundation.org/HistoricalDataAccess/");
            public static readonly ResultID W_NOFILTER = new ResultID("W_NOFILTER", "http://opcfoundation.org/HistoricalDataAccess/");
        }

        private class Names
        {
            internal const string CODE = "CO";
            internal const string NAME = "NA";
            internal const string NAMESPACE = "NS";
        }
    }
}

