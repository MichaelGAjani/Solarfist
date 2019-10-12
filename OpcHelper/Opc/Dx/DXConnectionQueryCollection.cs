namespace Jund.OpcHelper.Opc.Dx
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class DXConnectionQueryCollection : ICloneable, IList, ICollection, IEnumerable
    {
        private ArrayList m_queries = new ArrayList();

        internal DXConnectionQueryCollection()
        {
        }

        public int Add(DXConnectionQuery value)
        {
            return this.Add(value);
        }

        public int Add(object value)
        {
            this.Insert(this.m_queries.Count, value);
            return (this.m_queries.Count - 1);
        }

        public void Clear()
        {
            this.m_queries.Clear();
        }

        public virtual object Clone()
        {
            DXConnectionQueryCollection querys = (DXConnectionQueryCollection) base.MemberwiseClone();
            querys.m_queries = new ArrayList();
            foreach (DXConnectionQuery query in this.m_queries)
            {
                querys.m_queries.Add(query.Clone());
            }
            return querys;
        }

        public bool Contains(DXConnectionQuery value)
        {
            return this.Contains(value);
        }

        public bool Contains(object value)
        {
            foreach (ItemIdentifier identifier in this.m_queries)
            {
                if (identifier.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(Array array, int index)
        {
            if (this.m_queries != null)
            {
                this.m_queries.CopyTo(array, index);
            }
        }

        public void CopyTo(DXConnectionQuery[] array, int index)
        {
            this.CopyTo((Array) array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_queries.GetEnumerator();
        }

        public int IndexOf(DXConnectionQuery value)
        {
            return this.IndexOf(value);
        }

        public int IndexOf(object value)
        {
            return this.m_queries.IndexOf(value);
        }

        internal void Initialize(ICollection queries)
        {
            this.m_queries.Clear();
            if (queries != null)
            {
                foreach (DXConnectionQuery query in queries)
                {
                    this.m_queries.Add(query);
                }
            }
        }

        public void Insert(int index, DXConnectionQuery value)
        {
            this.Insert(index, value);
        }

        public void Insert(int index, object value)
        {
            if (!typeof(DXConnectionQuery).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add DXConnectionQuery objects into the collection.");
            }
            this.m_queries.Insert(index, value);
        }

        public void Remove(DXConnectionQuery value)
        {
            this.Remove(value);
        }

        public void Remove(object value)
        {
            if (!typeof(DXConnectionQuery).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only delete DXConnectionQuery obejcts from the collection.");
            }
            this.m_queries.Remove(value);
        }

        public void RemoveAt(int index)
        {
            if ((index < 0) || (index >= this.m_queries.Count))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            this.Remove(this.m_queries[index]);
        }

        public int Count
        {
            get
            {
                if (this.m_queries == null)
                {
                    return 0;
                }
                return this.m_queries.Count;
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

        public DXConnectionQuery this[int index]
        {
            get
            {
                return (DXConnectionQuery) this.m_queries[index];
            }
        }

        public DXConnectionQuery this[string name]
        {
            get
            {
                foreach (DXConnectionQuery query in this.m_queries)
                {
                    if (query.Name == name)
                    {
                        return query;
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
                return this.m_queries[index];
            }
            set
            {
                this.Insert(index, value);
            }
        }
    }
}

