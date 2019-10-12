namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using Opc.Da;
    using System;

    [Serializable]
    public class ItemValue : ICloneable
    {
        private Opc.Hda.Quality m_historianQuality = Opc.Hda.Quality.NoData;
        private Opc.Da.Quality m_quality = Opc.Da.Quality.Bad;
        private DateTime m_timestamp = DateTime.MinValue;
        private object m_value = null;

        public object Clone()
        {
            Opc.Hda.ItemValue value2 = (Opc.Hda.ItemValue) base.MemberwiseClone();
            value2.Value = Opc.Convert.Clone(this.Value);
            return value2;
        }

        public Opc.Hda.Quality HistorianQuality
        {
            get
            {
                return this.m_historianQuality;
            }
            set
            {
                this.m_historianQuality = value;
            }
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

