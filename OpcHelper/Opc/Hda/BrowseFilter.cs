namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;

    [Serializable]
    public class BrowseFilter : ICloneable
    {
        private int m_attributeID = 0;
        private object m_filterValue = null;
        private Opc.Hda.Operator m_operator = Opc.Hda.Operator.Equal;

        public virtual object Clone()
        {
            BrowseFilter filter = (BrowseFilter) base.MemberwiseClone();
            filter.FilterValue = Opc.Convert.Clone(this.FilterValue);
            return filter;
        }

        public int AttributeID
        {
            get
            {
                return this.m_attributeID;
            }
            set
            {
                this.m_attributeID = value;
            }
        }

        public object FilterValue
        {
            get
            {
                return this.m_filterValue;
            }
            set
            {
                this.m_filterValue = value;
            }
        }

        public Opc.Hda.Operator Operator
        {
            get
            {
                return this.m_operator;
            }
            set
            {
                this.m_operator = value;
            }
        }
    }
}

