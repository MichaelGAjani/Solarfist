namespace Jund.OpcHelper.Opc.Da
{
    using Opc;
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class ItemPropertyCollection : ArrayList, IResult
    {
        private string m_diagnosticInfo;
        private string m_itemName;
        private string m_itemPath;
        private Opc.ResultID m_resultID;

        public ItemPropertyCollection()
        {
            this.m_itemName = null;
            this.m_itemPath = null;
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
        }

        public ItemPropertyCollection(ItemIdentifier itemID)
        {
            this.m_itemName = null;
            this.m_itemPath = null;
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            if (itemID != null)
            {
                this.m_itemName = itemID.ItemName;
                this.m_itemPath = itemID.ItemPath;
            }
        }

        public ItemPropertyCollection(ItemIdentifier itemID, Opc.ResultID resultID)
        {
            this.m_itemName = null;
            this.m_itemPath = null;
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            if (itemID != null)
            {
                this.m_itemName = itemID.ItemName;
                this.m_itemPath = itemID.ItemPath;
            }
            this.ResultID = resultID;
        }

        public int Add(ItemProperty value)
        {
            return this.Add(value);
        }

        public bool Contains(ItemProperty value)
        {
            return this.Contains(value);
        }

        public void CopyTo(ItemProperty[] array, int index)
        {
            this.CopyTo(array, index);
        }

        public int IndexOf(ItemProperty value)
        {
            return this.IndexOf(value);
        }

        public void Insert(int index, ItemProperty value)
        {
            this.Insert(index, value);
        }

        public void Remove(ItemProperty value)
        {
            this.Remove(value);
        }

        public string DiagnosticInfo
        {
            get
            {
                return this.m_diagnosticInfo;
            }
            set
            {
                this.m_diagnosticInfo = value;
            }
        }

        public ItemProperty this[int index]
        {
            get
            {
                return (ItemProperty) base[index];
            }
            set
            {
                base[index] = value;
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

        public Opc.ResultID ResultID
        {
            get
            {
                return this.m_resultID;
            }
            set
            {
                this.m_resultID = value;
            }
        }
    }
}

