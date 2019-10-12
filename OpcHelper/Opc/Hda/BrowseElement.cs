namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;

    public class BrowseElement : ItemIdentifier
    {
        private AttributeValueCollection m_attributes = new AttributeValueCollection();
        private bool m_hasChildren = false;
        private bool m_isItem = false;
        private string m_name = null;

        public override object Clone()
        {
            BrowseElement element = (BrowseElement) base.MemberwiseClone();
            element.Attributes = (AttributeValueCollection) this.m_attributes.Clone();
            return element;
        }

        public AttributeValueCollection Attributes
        {
            get
            {
                return this.m_attributes;
            }
            set
            {
                this.m_attributes = value;
            }
        }

        public bool HasChildren
        {
            get
            {
                return this.m_hasChildren;
            }
            set
            {
                this.m_hasChildren = value;
            }
        }

        public bool IsItem
        {
            get
            {
                return this.m_isItem;
            }
            set
            {
                this.m_isItem = value;
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
    }
}

