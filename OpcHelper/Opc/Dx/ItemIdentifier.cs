namespace Jund.OpcHelper.Opc.Dx
{
    using Opc;
    using System;

    [Serializable]
    public class ItemIdentifier : Opc.ItemIdentifier
    {
        private string m_version;

        public ItemIdentifier()
        {
            this.m_version = null;
        }

        public ItemIdentifier(Opc.Dx.ItemIdentifier item) : base(item)
        {
            this.m_version = null;
            if (item != null)
            {
                this.m_version = item.m_version;
            }
        }

        public ItemIdentifier(Opc.ItemIdentifier item) : base(item)
        {
            this.m_version = null;
        }

        public ItemIdentifier(string itemName) : base(itemName)
        {
            this.m_version = null;
        }

        public ItemIdentifier(string itemPath, string itemName) : base(itemPath, itemName)
        {
            this.m_version = null;
        }

        public override bool Equals(object target)
        {
            if (!typeof(Opc.Dx.ItemIdentifier).IsInstanceOfType(target))
            {
                return false;
            }
            Opc.Dx.ItemIdentifier identifier = (Opc.Dx.ItemIdentifier) target;
            return (((identifier.ItemName == base.ItemName) && (identifier.ItemPath == base.ItemPath)) && (identifier.Version == this.Version));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public string Version
        {
            get
            {
                return this.m_version;
            }
            set
            {
                this.m_version = value;
            }
        }
    }
}

