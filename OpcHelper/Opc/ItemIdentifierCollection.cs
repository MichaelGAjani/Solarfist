namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class ItemIdentifierCollection : ICloneable, ICollection, IEnumerable
    {
        private ItemIdentifier[] m_itemIDs;

        public ItemIdentifierCollection()
        {
            this.m_itemIDs = new ItemIdentifier[0];
        }

        public ItemIdentifierCollection(ICollection collection)
        {
            this.m_itemIDs = new ItemIdentifier[0];
            this.Init(collection);
        }

        public void Clear()
        {
            this.m_itemIDs = new ItemIdentifier[0];
        }

        public virtual object Clone()
        {
            return new ItemIdentifierCollection(this);
        }

        public void CopyTo(Array array, int index)
        {
            if (this.m_itemIDs != null)
            {
                this.m_itemIDs.CopyTo(array, index);
            }
        }

        public void CopyTo(ItemIdentifier[] array, int index)
        {
            this.CopyTo((Array) array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_itemIDs.GetEnumerator();
        }

        public void Init(ICollection collection)
        {
            this.Clear();
            if (collection != null)
            {
                ArrayList list = new ArrayList(collection.Count);
                foreach (object obj2 in collection)
                {
                    if (typeof(ItemIdentifier).IsInstanceOfType(obj2))
                    {
                        list.Add(((ItemIdentifier) obj2).Clone());
                    }
                }
                this.m_itemIDs = (ItemIdentifier[]) list.ToArray(typeof(ItemIdentifier));
            }
        }

        public int Count
        {
            get
            {
                if (this.m_itemIDs == null)
                {
                    return 0;
                }
                return this.m_itemIDs.Length;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public ItemIdentifier this[int index]
        {
            get
            {
                return this.m_itemIDs[index];
            }
            set
            {
                this.m_itemIDs[index] = value;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }
    }
}

