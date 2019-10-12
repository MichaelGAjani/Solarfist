namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class BrowseFilterCollection : ItemIdentifier, ICollection, IEnumerable
    {
        private BrowseFilter[] m_filters;

        public BrowseFilterCollection()
        {
            this.m_filters = new BrowseFilter[0];
        }

        public BrowseFilterCollection(ICollection collection)
        {
            this.m_filters = new BrowseFilter[0];
            this.Init(collection);
        }

        public void Clear()
        {
            this.m_filters = new BrowseFilter[0];
        }

        public override object Clone()
        {
            return new BrowseFilterCollection(this);
        }

        public void CopyTo(Array array, int index)
        {
            if (this.m_filters != null)
            {
                this.m_filters.CopyTo(array, index);
            }
        }

        public void CopyTo(BrowseFilter[] array, int index)
        {
            this.CopyTo((Array) array, index);
        }

        public BrowseFilter Find(int id)
        {
            foreach (BrowseFilter filter in this.m_filters)
            {
                if (filter.AttributeID == id)
                {
                    return filter;
                }
            }
            return null;
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_filters.GetEnumerator();
        }

        public void Init(ICollection collection)
        {
            this.Clear();
            if (collection != null)
            {
                ArrayList list = new ArrayList(collection.Count);
                foreach (object obj2 in collection)
                {
                    if (obj2.GetType() == typeof(BrowseFilter))
                    {
                        list.Add(Opc.Convert.Clone(obj2));
                    }
                }
                this.m_filters = (BrowseFilter[]) list.ToArray(typeof(BrowseFilter));
            }
        }

        public int Count
        {
            get
            {
                if (this.m_filters == null)
                {
                    return 0;
                }
                return this.m_filters.Length;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public BrowseFilter this[int index]
        {
            get
            {
                return this.m_filters[index];
            }
            set
            {
                this.m_filters[index] = value;
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

