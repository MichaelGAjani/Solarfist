namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class IdentifiedResultCollection : ICloneable, ICollection, IEnumerable
    {
        private IdentifiedResult[] m_results;

        public IdentifiedResultCollection()
        {
            this.m_results = new IdentifiedResult[0];
        }

        public IdentifiedResultCollection(ICollection collection)
        {
            this.m_results = new IdentifiedResult[0];
            this.Init(collection);
        }

        public void Clear()
        {
            this.m_results = new IdentifiedResult[0];
        }

        public virtual object Clone()
        {
            return new IdentifiedResultCollection(this);
        }

        public void CopyTo(Array array, int index)
        {
            if (this.m_results != null)
            {
                this.m_results.CopyTo(array, index);
            }
        }

        public void CopyTo(IdentifiedResult[] array, int index)
        {
            this.CopyTo((Array) array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_results.GetEnumerator();
        }

        public void Init(ICollection collection)
        {
            this.Clear();
            if (collection != null)
            {
                ArrayList list = new ArrayList(collection.Count);
                foreach (object obj2 in collection)
                {
                    if (typeof(IdentifiedResult).IsInstanceOfType(obj2))
                    {
                        list.Add(((IdentifiedResult) obj2).Clone());
                    }
                }
                this.m_results = (IdentifiedResult[]) list.ToArray(typeof(IdentifiedResult));
            }
        }

        public int Count
        {
            get
            {
                if (this.m_results == null)
                {
                    return 0;
                }
                return this.m_results.Length;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public IdentifiedResult this[int index]
        {
            get
            {
                return this.m_results[index];
            }
            set
            {
                this.m_results[index] = value;
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

