namespace Jund.OpcHelper.Opc.Ae
{
    using System;

    [Serializable]
    public class BrowseElement
    {
        private string m_name = null;
        private BrowseType m_nodeType = BrowseType.Area;

        public virtual object Clone()
        {
            return base.MemberwiseClone();
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

        public BrowseType NodeType
        {
            get
            {
                return this.m_nodeType;
            }
            set
            {
                this.m_nodeType = value;
            }
        }

        public string QualifiedName
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
    }
}

