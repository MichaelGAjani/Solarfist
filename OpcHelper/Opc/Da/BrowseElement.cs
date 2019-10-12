namespace Jund.OpcHelper.Opc.Da
{
    using Opc;
    using System;

    [Serializable]
    public class BrowseElement : ICloneable
    {
        private bool m_hasChildren = false;
        private bool m_isItem = false;
        private string m_itemName = null;
        private string m_itemPath = null;
        private string m_name = null;
        private ItemProperty[] m_properties = new ItemProperty[0];

        public virtual object Clone()
        {
            BrowseElement element = (BrowseElement) base.MemberwiseClone();
            element.m_properties = (ItemProperty[]) Opc.Convert.Clone(this.m_properties);
            return element;
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

        public string ItemName
        {
            get
            {
                return this.m_itemName;
            }
            set
            {
                this.m_itemName = value;
            }
        }

        public string ItemPath
        {
            get
            {
                return this.m_itemPath;
            }
            set
            {
                this.m_itemPath = value;
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

        public ItemProperty[] Properties
        {
            get
            {
                return this.m_properties;
            }
            set
            {
                this.m_properties = value;
            }
        }
    }
}

