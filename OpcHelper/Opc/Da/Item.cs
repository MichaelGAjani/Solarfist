namespace Jund.OpcHelper.Opc.Da
{
    using Opc;
    using System;

    [Serializable]
    public class Item : ItemIdentifier
    {
        private bool m_active;
        private bool m_activeSpecified;
        private float m_deadband;
        private bool m_deadbandSpecified;
        private bool m_enableBuffering;
        private bool m_enableBufferingSpecified;
        private int m_maxAge;
        private bool m_maxAgeSpecified;
        private System.Type m_reqType;
        private int m_samplingRate;
        private bool m_samplingRateSpecified;

        public Item()
        {
            this.m_reqType = null;
            this.m_maxAge = 0;
            this.m_maxAgeSpecified = false;
            this.m_active = true;
            this.m_activeSpecified = false;
            this.m_deadband = 0f;
            this.m_deadbandSpecified = false;
            this.m_samplingRate = 0;
            this.m_samplingRateSpecified = false;
            this.m_enableBuffering = false;
            this.m_enableBufferingSpecified = false;
        }

        public Item(Item item) : base(item)
        {
            this.m_reqType = null;
            this.m_maxAge = 0;
            this.m_maxAgeSpecified = false;
            this.m_active = true;
            this.m_activeSpecified = false;
            this.m_deadband = 0f;
            this.m_deadbandSpecified = false;
            this.m_samplingRate = 0;
            this.m_samplingRateSpecified = false;
            this.m_enableBuffering = false;
            this.m_enableBufferingSpecified = false;
            if (item != null)
            {
                this.ReqType = item.ReqType;
                this.MaxAge = item.MaxAge;
                this.MaxAgeSpecified = item.MaxAgeSpecified;
                this.Active = item.Active;
                this.ActiveSpecified = item.ActiveSpecified;
                this.Deadband = item.Deadband;
                this.DeadbandSpecified = item.DeadbandSpecified;
                this.SamplingRate = item.SamplingRate;
                this.SamplingRateSpecified = item.SamplingRateSpecified;
                this.EnableBuffering = item.EnableBuffering;
                this.EnableBufferingSpecified = item.EnableBufferingSpecified;
            }
        }

        public Item(ItemIdentifier item)
        {
            this.m_reqType = null;
            this.m_maxAge = 0;
            this.m_maxAgeSpecified = false;
            this.m_active = true;
            this.m_activeSpecified = false;
            this.m_deadband = 0f;
            this.m_deadbandSpecified = false;
            this.m_samplingRate = 0;
            this.m_samplingRateSpecified = false;
            this.m_enableBuffering = false;
            this.m_enableBufferingSpecified = false;
            if (item != null)
            {
                base.ItemName = item.ItemName;
                base.ItemPath = item.ItemPath;
                base.ClientHandle = item.ClientHandle;
                base.ServerHandle = item.ServerHandle;
            }
        }

        public bool Active
        {
            get
            {
                return this.m_active;
            }
            set
            {
                this.m_active = value;
            }
        }

        public bool ActiveSpecified
        {
            get
            {
                return this.m_activeSpecified;
            }
            set
            {
                this.m_activeSpecified = value;
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

        public bool EnableBuffering
        {
            get
            {
                return this.m_enableBuffering;
            }
            set
            {
                this.m_enableBuffering = value;
            }
        }

        public bool EnableBufferingSpecified
        {
            get
            {
                return this.m_enableBufferingSpecified;
            }
            set
            {
                this.m_enableBufferingSpecified = value;
            }
        }

        public int MaxAge
        {
            get
            {
                return this.m_maxAge;
            }
            set
            {
                this.m_maxAge = value;
            }
        }

        public bool MaxAgeSpecified
        {
            get
            {
                return this.m_maxAgeSpecified;
            }
            set
            {
                this.m_maxAgeSpecified = value;
            }
        }

        public System.Type ReqType
        {
            get
            {
                return this.m_reqType;
            }
            set
            {
                this.m_reqType = value;
            }
        }

        public int SamplingRate
        {
            get
            {
                return this.m_samplingRate;
            }
            set
            {
                this.m_samplingRate = value;
            }
        }

        public bool SamplingRateSpecified
        {
            get
            {
                return this.m_samplingRateSpecified;
            }
            set
            {
                this.m_samplingRateSpecified = value;
            }
        }
    }
}

