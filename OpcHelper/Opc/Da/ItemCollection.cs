namespace Jund.OpcHelper.Opc.Da
{
    using Opc;
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class ItemCollection : ICloneable, IList, ICollection, IEnumerable
    {
        private ArrayList m_items;

        public ItemCollection()
        {
            this.m_items = new ArrayList();
        }

        public ItemCollection(ItemCollection items)
        {
            this.m_items = new ArrayList();
            if (items != null)
            {
                foreach (Item item in items)
                {
                    this.Add(item);
                }
            }
        }

        public int Add(Item value)
        {
            return this.Add(value);
        }

        public int Add(object value)
        {
            if (!typeof(Item).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add Item objects into the collection.");
            }
            return this.m_items.Add(value);
        }

        public void Clear()
        {
            this.m_items.Clear();
        }

        public virtual object Clone()
        {
            ItemCollection items = (ItemCollection) base.MemberwiseClone();
            items.m_items = new ArrayList();
            foreach (Item item in this.m_items)
            {
                items.m_items.Add(item.Clone());
            }
            return items;
        }

        public bool Contains(Item value)
        {
            return this.Contains(value);
        }

        public bool Contains(object value)
        {
            return this.m_items.Contains(value);
        }

        public void CopyTo(Array array, int index)
        {
            if (this.m_items != null)
            {
                this.m_items.CopyTo(array, index);
            }
        }

        public void CopyTo(Item[] array, int index)
        {
            this.CopyTo((Array) array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_items.GetEnumerator();
        }

        public int IndexOf(Item value)
        {
            return this.IndexOf(value);
        }

        public int IndexOf(object value)
        {
            return this.m_items.IndexOf(value);
        }

        public void Insert(int index, Item value)
        {
            this.Insert(index, value);
        }

        public void Insert(int index, object value)
        {
            if (!typeof(Item).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add Item objects into the collection.");
            }
            this.m_items.Insert(index, value);
        }

        public void Remove(Item value)
        {
            this.Remove(value);
        }

        public void Remove(object value)
        {
            this.m_items.Remove(value);
        }

        public void RemoveAt(int index)
        {
            this.m_items.RemoveAt(index);
        }

        public int Count
        {
            get
            {
                if (this.m_items == null)
                {
                    return 0;
                }
                return this.m_items.Count;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public Item this[int index]
        {
            get
            {
                return (Item) this.m_items[index];
            }
            set
            {
                this.m_items[index] = value;
            }
        }

        public Item this[ItemIdentifier itemID]
        {
            get
            {
                foreach (Item item in this.m_items)
                {
                    if (itemID.Key == item.Key)
                    {
                        return item;
                    }
                }
                return null;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this.m_items[index];
            }
            set
            {
                if (!typeof(Item).IsInstanceOfType(value))
                {
                    throw new ArgumentException("May only add Item objects into the collection.");
                }
                this.m_items[index] = value;
            }
        }
    }
}

