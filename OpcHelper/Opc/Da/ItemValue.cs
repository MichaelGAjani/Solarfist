namespace Jund.OpcHelper.Opc.Da
{
    using Opc;
    using System;

    [Serializable]
    public class ItemValue : ItemIdentifier
    {
        private Opc.Da.Quality m_quality;
        private bool m_qualitySpecified;
        private DateTime m_timestamp;
        private bool m_timestampSpecified;
        private object m_value;

        public ItemValue()
        {
            this.m_value = null;
            this.m_quality = Opc.Da.Quality.Bad;
            this.m_qualitySpecified = false;
            this.m_timestamp = DateTime.MinValue;
            this.m_timestampSpecified = false;
        }

        public ItemValue(ItemValue item) : base(item)
        {
            this.m_value = null;
            this.m_quality = Opc.Da.Quality.Bad;
            this.m_qualitySpecified = false;
            this.m_timestamp = DateTime.MinValue;
            this.m_timestampSpecified = false;
            if (item != null)
            {
                this.Value = Opc.Convert.Clone(item.Value);
                this.Quality = item.Quality;
                this.QualitySpecified = item.QualitySpecified;
                this.Timestamp = item.Timestamp;
                this.TimestampSpecified = item.TimestampSpecified;
            }
        }

        public ItemValue(ItemIdentifier item)
        {
            this.m_value = null;
            this.m_quality = Opc.Da.Quality.Bad;
            this.m_qualitySpecified = false;
            this.m_timestamp = DateTime.MinValue;
            this.m_timestampSpecified = false;
            if (item != null)
            {
                base.ItemName = item.ItemName;
                base.ItemPath = item.ItemPath;
                base.ClientHandle = item.ClientHandle;
                base.ServerHandle = item.ServerHandle;
            }
        }

        public ItemValue(string itemName) : base(itemName)
        {
            this.m_value = null;
            this.m_quality = Opc.Da.Quality.Bad;
            this.m_qualitySpecified = false;
            this.m_timestamp = DateTime.MinValue;
            this.m_timestampSpecified = false;
        }

        public override object Clone()
        {
            ItemValue value2 = (ItemValue) base.MemberwiseClone();
            value2.Value = Opc.Convert.Clone(this.Value);
            return value2;
        }

        public Opc.Da.Quality Quality
        {
            get
            {
                return this.m_quality;
            }
            set
            {
                this.m_quality = value;
            }
        }

        public bool QualitySpecified
        {
            get
            {
                return this.m_qualitySpecified;
            }
            set
            {
                this.m_qualitySpecified = value;
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return this.m_timestamp;
            }
            set
            {
                this.m_timestamp = value;
            }
        }

        public bool TimestampSpecified
        {
            get
            {
                return this.m_timestampSpecified;
            }
            set
            {
                this.m_timestampSpecified = value;
            }
        }

        public object Value
        {
            get
            {
                return this.m_value;
            }
            set
            {
                this.m_value = value;
            }
        }
    }
}

