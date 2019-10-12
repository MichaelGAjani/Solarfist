namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;

    [Serializable]
    public class AttributeValue : ICloneable
    {
        private DateTime m_timestamp = DateTime.MinValue;
        private object m_value = null;

        public virtual object Clone()
        {
            AttributeValue value2 = (AttributeValue) base.MemberwiseClone();
            value2.m_value = Opc.Convert.Clone(this.m_value);
            return value2;
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

