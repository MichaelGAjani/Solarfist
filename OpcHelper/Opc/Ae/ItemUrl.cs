namespace Jund.OpcHelper.Opc.Ae
{
    using Opc;
    using System;

    [Serializable]
    public class ItemUrl : ItemIdentifier
    {
        private URL m_url;

        public ItemUrl()
        {
            this.m_url = new URL();
        }

        public ItemUrl(ItemUrl item) : base(item)
        {
            this.m_url = new URL();
            if (item != null)
            {
                this.Url = item.Url;
            }
        }

        public ItemUrl(ItemIdentifier item) : base(item)
        {
            this.m_url = new URL();
        }

        public ItemUrl(ItemIdentifier item, URL url) : base(item)
        {
            this.m_url = new URL();
            this.Url = url;
        }

        public override object Clone()
        {
            return new ItemUrl(this);
        }

        public URL Url
        {
            get
            {
                return this.m_url;
            }
            set
            {
                this.m_url = value;
            }
        }
    }
}

