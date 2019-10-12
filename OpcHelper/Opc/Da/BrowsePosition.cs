namespace Jund.OpcHelper.Opc.Da
{
    using Opc;
    using System;

    [Serializable]
    public class BrowsePosition : IBrowsePosition, IDisposable, ICloneable
    {
        private BrowseFilters m_filters = null;
        private ItemIdentifier m_itemID = null;

        public BrowsePosition(ItemIdentifier itemID, BrowseFilters filters)
        {
            if (filters == null)
            {
                throw new ArgumentNullException("filters");
            }
            this.m_itemID = (itemID != null) ? ((ItemIdentifier) itemID.Clone()) : null;
            this.m_filters = (BrowseFilters) filters.Clone();
        }

        public virtual object Clone()
        {
            return (BrowsePosition) base.MemberwiseClone();
        }

        public virtual void Dispose()
        {
        }

        public BrowseFilters Filters
        {
            get
            {
                return (BrowseFilters) this.m_filters.Clone();
            }
        }

        public ItemIdentifier ItemID
        {
            get
            {
                return this.m_itemID;
            }
        }

        public int MaxElementsReturned
        {
            get
            {
                return this.m_filters.MaxElementsReturned;
            }
            set
            {
                this.m_filters.MaxElementsReturned = value;
            }
        }
    }
}

