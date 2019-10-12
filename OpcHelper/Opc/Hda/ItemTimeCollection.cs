namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class ItemTimeCollection : ItemIdentifier, ICloneable, IList, ICollection, IEnumerable
    {
        private ArrayList m_times;

        public ItemTimeCollection()
        {
            this.m_times = new ArrayList();
        }

        public ItemTimeCollection(ItemTimeCollection item) : base(item)
        {
            this.m_times = new ArrayList();
            this.m_times = new ArrayList(item.m_times.Count);
            foreach (DateTime time in item.m_times)
            {
                this.m_times.Add(time);
            }
        }

        public ItemTimeCollection(ItemIdentifier item) : base(item)
        {
            this.m_times = new ArrayList();
        }

        public int Add(DateTime value)
        {
            return this.Add(value);
        }

        public int Add(object value)
        {
            if (!typeof(DateTime).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add DateTime objects into the collection.");
            }
            return this.m_times.Add(value);
        }

        public void Clear()
        {
            this.m_times.Clear();
        }

        public override object Clone()
        {
            ItemTimeCollection times = (ItemTimeCollection) base.Clone();
            times.m_times = new ArrayList(this.m_times.Count);
            foreach (DateTime time in this.m_times)
            {
                times.m_times.Add(time);
            }
            return times;
        }

        public bool Contains(DateTime value)
        {
            return this.Contains(value);
        }

        public bool Contains(object value)
        {
            return this.m_times.Contains(value);
        }

        public void CopyTo(Array array, int index)
        {
            if (this.m_times != null)
            {
                this.m_times.CopyTo(array, index);
            }
        }

        public void CopyTo(DateTime[] array, int index)
        {
            this.CopyTo((Array) array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_times.GetEnumerator();
        }

        public int IndexOf(DateTime value)
        {
            return this.IndexOf(value);
        }

        public int IndexOf(object value)
        {
            return this.m_times.IndexOf(value);
        }

        public void Insert(int index, DateTime value)
        {
            this.Insert(index, value);
        }

        public void Insert(int index, object value)
        {
            if (!typeof(DateTime).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add DateTime objects into the collection.");
            }
            this.m_times.Insert(index, value);
        }

        public void Remove(DateTime value)
        {
            this.Remove(value);
        }

        public void Remove(object value)
        {
            this.m_times.Remove(value);
        }

        public void RemoveAt(int index)
        {
            this.m_times.RemoveAt(index);
        }

        public int Count
        {
            get
            {
                if (this.m_times == null)
                {
                    return 0;
                }
                return this.m_times.Count;
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

        public DateTime this[int index]
        {
            get
            {
                return (DateTime) this.m_times[index];
            }
            set
            {
                this.m_times[index] = value;
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
                return this.m_times[index];
            }
            set
            {
                if (!typeof(DateTime).IsInstanceOfType(value))
                {
                    throw new ArgumentException("May only add DateTime objects into the collection.");
                }
                this.m_times[index] = value;
            }
        }
    }
}

