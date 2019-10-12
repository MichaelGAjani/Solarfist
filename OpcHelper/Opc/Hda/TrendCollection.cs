namespace Jund.OpcHelper.Opc.Hda
{
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class TrendCollection : ICloneable, IList, ICollection, IEnumerable
    {
        private ArrayList m_trends;

        public TrendCollection()
        {
            this.m_trends = new ArrayList();
        }

        public TrendCollection(TrendCollection items)
        {
            this.m_trends = new ArrayList();
            if (items != null)
            {
                foreach (Trend trend in items)
                {
                    this.Add(trend);
                }
            }
        }

        public int Add(Trend value)
        {
            return this.Add(value);
        }

        public int Add(object value)
        {
            if (!typeof(Trend).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add Trend objects into the collection.");
            }
            return this.m_trends.Add(value);
        }

        public void Clear()
        {
            this.m_trends.Clear();
        }

        public virtual object Clone()
        {
            TrendCollection trends = (TrendCollection) base.MemberwiseClone();
            trends.m_trends = new ArrayList();
            foreach (Trend trend in this.m_trends)
            {
                trends.m_trends.Add(trend.Clone());
            }
            return trends;
        }

        public bool Contains(Trend value)
        {
            return this.Contains(value);
        }

        public bool Contains(object value)
        {
            return this.m_trends.Contains(value);
        }

        public void CopyTo(Array array, int index)
        {
            if (this.m_trends != null)
            {
                this.m_trends.CopyTo(array, index);
            }
        }

        public void CopyTo(Trend[] array, int index)
        {
            this.CopyTo((Array) array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_trends.GetEnumerator();
        }

        public int IndexOf(Trend value)
        {
            return this.IndexOf(value);
        }

        public int IndexOf(object value)
        {
            return this.m_trends.IndexOf(value);
        }

        public void Insert(int index, Trend value)
        {
            this.Insert(index, value);
        }

        public void Insert(int index, object value)
        {
            if (!typeof(Trend).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add Trend objects into the collection.");
            }
            this.m_trends.Insert(index, value);
        }

        public void Remove(Trend value)
        {
            this.Remove(value);
        }

        public void Remove(object value)
        {
            this.m_trends.Remove(value);
        }

        public void RemoveAt(int index)
        {
            this.m_trends.RemoveAt(index);
        }

        public int Count
        {
            get
            {
                if (this.m_trends == null)
                {
                    return 0;
                }
                return this.m_trends.Count;
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

        public Trend this[int index]
        {
            get
            {
                return (Trend) this.m_trends[index];
            }
        }

        public Trend this[string name]
        {
            get
            {
                foreach (Trend trend in this.m_trends)
                {
                    if (trend.Name == name)
                    {
                        return trend;
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
                return this.m_trends[index];
            }
            set
            {
                if (!typeof(Trend).IsInstanceOfType(value))
                {
                    throw new ArgumentException("May only add Trend objects into the collection.");
                }
                this.m_trends[index] = value;
            }
        }
    }
}

