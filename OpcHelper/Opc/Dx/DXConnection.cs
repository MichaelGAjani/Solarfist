namespace Jund.OpcHelper.Opc.Dx
{
    using System;

    [Serializable]
    public class DXConnection : ItemIdentifier
    {
        private BrowsePathCollection m_browsePaths;
        private float m_deadband;
        private bool m_deadbandSpecified;
        private bool m_defaultOverridden;
        private bool m_defaultOverriddenSpecified;
        private object m_defaultOverrideValue;
        private bool m_defaultSourceItemConnected;
        private bool m_defaultSourceItemConnectedSpecified;
        private bool m_defaultTargetItemConnected;
        private bool m_defaultTargetItemConnectedSpecified;
        private string m_description;
        private bool m_enableSubstituteValue;
        private bool m_enableSubstituteValueSpecified;
        private string m_keyword;
        private string m_name;
        private string m_sourceItemName;
        private string m_sourceItemPath;
        private int m_sourceItemQueueSize;
        private bool m_sourceItemQueueSizeSpecified;
        private string m_sourceServerName;
        private object m_substituteValue;
        private string m_targetItemName;
        private string m_targetItemPath;
        private int m_updateRate;
        private bool m_updateRateSpecified;
        private string m_vendorData;

        public DXConnection()
        {
            this.m_name = null;
            this.m_browsePaths = new BrowsePathCollection();
            this.m_description = null;
            this.m_keyword = null;
            this.m_defaultSourceItemConnected = false;
            this.m_defaultSourceItemConnectedSpecified = false;
            this.m_defaultTargetItemConnected = false;
            this.m_defaultTargetItemConnectedSpecified = false;
            this.m_defaultOverridden = false;
            this.m_defaultOverriddenSpecified = false;
            this.m_defaultOverrideValue = null;
            this.m_enableSubstituteValue = false;
            this.m_enableSubstituteValueSpecified = false;
            this.m_substituteValue = null;
            this.m_targetItemName = null;
            this.m_targetItemPath = null;
            this.m_sourceServerName = null;
            this.m_sourceItemName = null;
            this.m_sourceItemPath = null;
            this.m_sourceItemQueueSize = 1;
            this.m_sourceItemQueueSizeSpecified = false;
            this.m_updateRate = 0;
            this.m_updateRateSpecified = false;
            this.m_deadband = 0f;
            this.m_deadbandSpecified = false;
            this.m_vendorData = null;
        }

        public DXConnection(DXConnection connection) : base((ItemIdentifier) connection)
        {
            this.m_name = null;
            this.m_browsePaths = new BrowsePathCollection();
            this.m_description = null;
            this.m_keyword = null;
            this.m_defaultSourceItemConnected = false;
            this.m_defaultSourceItemConnectedSpecified = false;
            this.m_defaultTargetItemConnected = false;
            this.m_defaultTargetItemConnectedSpecified = false;
            this.m_defaultOverridden = false;
            this.m_defaultOverriddenSpecified = false;
            this.m_defaultOverrideValue = null;
            this.m_enableSubstituteValue = false;
            this.m_enableSubstituteValueSpecified = false;
            this.m_substituteValue = null;
            this.m_targetItemName = null;
            this.m_targetItemPath = null;
            this.m_sourceServerName = null;
            this.m_sourceItemName = null;
            this.m_sourceItemPath = null;
            this.m_sourceItemQueueSize = 1;
            this.m_sourceItemQueueSizeSpecified = false;
            this.m_updateRate = 0;
            this.m_updateRateSpecified = false;
            this.m_deadband = 0f;
            this.m_deadbandSpecified = false;
            this.m_vendorData = null;
            if (connection != null)
            {
                this.BrowsePaths.AddRange(connection.BrowsePaths);
                this.Name = connection.Name;
                this.Description = connection.Description;
                this.Keyword = connection.Keyword;
                this.DefaultSourceItemConnected = connection.DefaultSourceItemConnected;
                this.DefaultSourceItemConnectedSpecified = connection.DefaultSourceItemConnectedSpecified;
                this.DefaultTargetItemConnected = connection.DefaultTargetItemConnected;
                this.DefaultTargetItemConnectedSpecified = connection.DefaultTargetItemConnectedSpecified;
                this.DefaultOverridden = connection.DefaultOverridden;
                this.DefaultOverriddenSpecified = connection.DefaultOverriddenSpecified;
                this.DefaultOverrideValue = connection.DefaultOverrideValue;
                this.EnableSubstituteValue = connection.EnableSubstituteValue;
                this.EnableSubstituteValueSpecified = connection.EnableSubstituteValueSpecified;
                this.SubstituteValue = connection.SubstituteValue;
                this.TargetItemName = connection.TargetItemName;
                this.TargetItemPath = connection.TargetItemPath;
                this.SourceServerName = connection.SourceServerName;
                this.SourceItemName = connection.SourceItemName;
                this.SourceItemPath = connection.SourceItemPath;
                this.SourceItemQueueSize = connection.SourceItemQueueSize;
                this.SourceItemQueueSizeSpecified = connection.SourceItemQueueSizeSpecified;
                this.UpdateRate = connection.UpdateRate;
                this.UpdateRateSpecified = connection.UpdateRateSpecified;
                this.Deadband = connection.Deadband;
                this.DeadbandSpecified = connection.DeadbandSpecified;
                this.VendorData = connection.VendorData;
            }
        }

        public DXConnection(ItemIdentifier item) : base(item)
        {
            this.m_name = null;
            this.m_browsePaths = new BrowsePathCollection();
            this.m_description = null;
            this.m_keyword = null;
            this.m_defaultSourceItemConnected = false;
            this.m_defaultSourceItemConnectedSpecified = false;
            this.m_defaultTargetItemConnected = false;
            this.m_defaultTargetItemConnectedSpecified = false;
            this.m_defaultOverridden = false;
            this.m_defaultOverriddenSpecified = false;
            this.m_defaultOverrideValue = null;
            this.m_enableSubstituteValue = false;
            this.m_enableSubstituteValueSpecified = false;
            this.m_substituteValue = null;
            this.m_targetItemName = null;
            this.m_targetItemPath = null;
            this.m_sourceServerName = null;
            this.m_sourceItemName = null;
            this.m_sourceItemPath = null;
            this.m_sourceItemQueueSize = 1;
            this.m_sourceItemQueueSizeSpecified = false;
            this.m_updateRate = 0;
            this.m_updateRateSpecified = false;
            this.m_deadband = 0f;
            this.m_deadbandSpecified = false;
            this.m_vendorData = null;
        }

        public override object Clone()
        {
            return new DXConnection(this);
        }

        public BrowsePathCollection BrowsePaths
        {
            get
            {
                return this.m_browsePaths;
            }
        }

        public float Deadband
        {
            get
            {
                return this.m_deadband;
            }
            set
            {
                this.m_deadband = value;
            }
        }

        public bool DeadbandSpecified
        {
            get
            {
                return this.m_deadbandSpecified;
            }
            set
            {
                this.m_deadbandSpecified = value;
            }
        }

        public bool DefaultOverridden
        {
            get
            {
                return this.m_defaultOverridden;
            }
            set
            {
                this.m_defaultOverridden = value;
            }
        }

        public bool DefaultOverriddenSpecified
        {
            get
            {
                return this.m_defaultOverriddenSpecified;
            }
            set
            {
                this.m_defaultOverriddenSpecified = value;
            }
        }

        public object DefaultOverrideValue
        {
            get
            {
                return this.m_defaultOverrideValue;
            }
            set
            {
                this.m_defaultOverrideValue = value;
            }
        }

        public bool DefaultSourceItemConnected
        {
            get
            {
                return this.m_defaultSourceItemConnected;
            }
            set
            {
                this.m_defaultSourceItemConnected = value;
            }
        }

        public bool DefaultSourceItemConnectedSpecified
        {
            get
            {
                return this.m_defaultSourceItemConnectedSpecified;
            }
            set
            {
                this.m_defaultSourceItemConnectedSpecified = value;
            }
        }

        public bool DefaultTargetItemConnected
        {
            get
            {
                return this.m_defaultTargetItemConnected;
            }
            set
            {
                this.m_defaultTargetItemConnected = value;
            }
        }

        public bool DefaultTargetItemConnectedSpecified
        {
            get
            {
                return this.m_defaultTargetItemConnectedSpecified;
            }
            set
            {
                this.m_defaultTargetItemConnectedSpecified = value;
            }
        }

        public string Description
        {
            get
            {
                return this.m_description;
            }
            set
            {
                this.m_description = value;
            }
        }

        public bool EnableSubstituteValue
        {
            get
            {
                return this.m_enableSubstituteValue;
            }
            set
            {
                this.m_enableSubstituteValue = value;
            }
        }

        public bool EnableSubstituteValueSpecified
        {
            get
            {
                return this.m_enableSubstituteValueSpecified;
            }
            set
            {
                this.m_enableSubstituteValueSpecified = value;
            }
        }

        public string Keyword
        {
            get
            {
                return this.m_keyword;
            }
            set
            {
                this.m_keyword = value;
            }
        }

        public string Name
        {
            get
            {
                return this.m_name;
            }
            set
            {
                this.m_name = value;
            }
        }

        public string SourceItemName
        {
            get
            {
                return this.m_sourceItemName;
            }
            set
            {
                this.m_sourceItemName = value;
            }
        }

        public string SourceItemPath
        {
            get
            {
                return this.m_sourceItemPath;
            }
            set
            {
                this.m_sourceItemPath = value;
            }
        }

        public int SourceItemQueueSize
        {
            get
            {
                return this.m_sourceItemQueueSize;
            }
            set
            {
                this.m_sourceItemQueueSize = value;
            }
        }

        public bool SourceItemQueueSizeSpecified
        {
            get
            {
                return this.m_sourceItemQueueSizeSpecified;
            }
            set
            {
                this.m_sourceItemQueueSizeSpecified = value;
            }
        }

        public string SourceServerName
        {
            get
            {
                return this.m_sourceServerName;
            }
            set
            {
                this.m_sourceServerName = value;
            }
        }

        public object SubstituteValue
        {
            get
            {
                return this.m_substituteValue;
            }
            set
            {
                this.m_substituteValue = value;
            }
        }

        public string TargetItemName
        {
            get
            {
                return this.m_targetItemName;
            }
            set
            {
                this.m_targetItemName = value;
            }
        }

        public string TargetItemPath
        {
            get
            {
                return this.m_targetItemPath;
            }
            set
            {
                this.m_targetItemPath = value;
            }
        }

        public int UpdateRate
        {
            get
            {
                return this.m_updateRate;
            }
            set
            {
                this.m_updateRate = value;
            }
        }

        public bool UpdateRateSpecified
        {
            get
            {
                return this.m_updateRateSpecified;
            }
            set
            {
                this.m_updateRateSpecified = value;
            }
        }

        public string VendorData
        {
            get
            {
                return this.m_vendorData;
            }
            set
            {
                this.m_vendorData = value;
            }
        }
    }
}

