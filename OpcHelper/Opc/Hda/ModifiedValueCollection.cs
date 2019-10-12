namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;
    using System.Reflection;

    [Serializable]
    public class ModifiedValueCollection : ItemValueCollection
    {
        public ModifiedValueCollection()
        {
        }

        public ModifiedValueCollection(Item item) : base(item)
        {
        }

        public ModifiedValueCollection(ItemValueCollection item) : base(item)
        {
        }

        public ModifiedValueCollection(ItemIdentifier item) : base(item)
        {
        }

        public ModifiedValue this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = value;
            }
        }
    }
}

