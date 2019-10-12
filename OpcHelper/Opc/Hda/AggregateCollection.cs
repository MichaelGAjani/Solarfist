namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class AggregateCollection : ICloneable, ICollection, IEnumerable
    {
        private Aggregate[] m_aggregates;

        public AggregateCollection()
        {
            this.m_aggregates = new Aggregate[0];
        }

        public AggregateCollection(ICollection collection)
        {
            this.m_aggregates = new Aggregate[0];
            this.Init(collection);
        }

        public void Clear()
        {
            this.m_aggregates = new Aggregate[0];
        }

        public virtual object Clone()
        {
            return new AggregateCollection(this);
        }

        public void CopyTo(Array array, int index)
        {
            if (this.m_aggregates != null)
            {
                this.m_aggregates.CopyTo(array, index);
            }
        }

        public void CopyTo(Aggregate[] array, int index)
        {
            this.CopyTo((Array) array, index);
        }

        public Aggregate Find(int id)
        {
            foreach (Aggregate aggregate in this.m_aggregates)
            {
                if (aggregate.ID == id)
                {
                    return aggregate;
                }
            }
            return null;
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_aggregates.GetEnumerator();
        }

        public void Init(ICollection collection)
        {
            this.Clear();
            if (collection != null)
            {
                ArrayList list = new ArrayList(collection.Count);
                foreach (object obj2 in collection)
                {
                    if (obj2.GetType() == typeof(Aggregate))
                    {
                        list.Add(Opc.Convert.Clone(obj2));
                    }
                }
                this.m_aggregates = (Aggregate[]) list.ToArray(typeof(Aggregate));
            }
        }

        public int Count
        {
            get
            {
                if (this.m_aggregates == null)
                {
                    return 0;
                }
                return this.m_aggregates.Length;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public Aggregate this[int index]
        {
            get
            {
                return this.m_aggregates[index];
            }
            set
            {
                this.m_aggregates[index] = value;
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

