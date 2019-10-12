namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;

    [Serializable]
    public class Item : ItemIdentifier
    {
        private int m_aggregateID;

        public Item()
        {
            this.m_aggregateID = 0;
        }

        public Item(Item item) : base(item)
        {
            this.m_aggregateID = 0;
            if (item != null)
            {
                this.AggregateID = item.AggregateID;
            }
        }

        public Item(ItemIdentifier item) : base(item)
        {
            this.m_aggregateID = 0;
        }

        public int AggregateID
        {
            get
            {
                return this.m_aggregateID;
            }
            set
            {
                this.m_aggregateID = value;
            }
        }
    }
}

