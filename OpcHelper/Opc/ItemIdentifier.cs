namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Text;

    [Serializable]
    public class ItemIdentifier : ICloneable
    {
        private object m_clientHandle;
        private string m_itemName;
        private string m_itemPath;
        private object m_serverHandle;

        public ItemIdentifier()
        {
            this.m_itemName = null;
            this.m_itemPath = null;
            this.m_clientHandle = null;
            this.m_serverHandle = null;
        }

        public ItemIdentifier(ItemIdentifier itemID)
        {
            this.m_itemName = null;
            this.m_itemPath = null;
            this.m_clientHandle = null;
            this.m_serverHandle = null;
            if (itemID != null)
            {
                this.ItemPath = itemID.ItemPath;
                this.ItemName = itemID.ItemName;
                this.ClientHandle = itemID.ClientHandle;
                this.ServerHandle = itemID.ServerHandle;
            }
        }

        public ItemIdentifier(string itemName)
        {
            this.m_itemName = null;
            this.m_itemPath = null;
            this.m_clientHandle = null;
            this.m_serverHandle = null;
            this.ItemPath = null;
            this.ItemName = itemName;
        }

        public ItemIdentifier(string itemPath, string itemName)
        {
            this.m_itemName = null;
            this.m_itemPath = null;
            this.m_clientHandle = null;
            this.m_serverHandle = null;
            this.ItemPath = itemPath;
            this.ItemName = itemName;
        }

        public virtual object Clone()
        {
            return base.MemberwiseClone();
        }

        public object ClientHandle
        {
            get
            {
                return this.m_clientHandle;
            }
            set
            {
                this.m_clientHandle = value;
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

        public string Key
        {
            get
            {
                return new StringBuilder(0x40).Append((this.ItemName == null) ? "null" : this.ItemName).Append("\r\n").Append((this.ItemPath == null) ? "null" : this.ItemPath).ToString();
            }
        }

        public object ServerHandle
        {
            get
            {
                return this.m_serverHandle;
            }
            set
            {
                this.m_serverHandle = value;
            }
        }
    }
}

