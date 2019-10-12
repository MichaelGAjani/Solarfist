namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class ResultCollection : ItemIdentifier, ICloneable, IList, ICollection, IEnumerable
    {
        private ArrayList m_results;

        public ResultCollection()
        {
            this.m_results = new ArrayList();
        }

        public ResultCollection(ResultCollection item) : base(item)
        {
            this.m_results = new ArrayList();
            this.m_results = new ArrayList(item.m_results.Count);
            foreach (Result result in item.m_results)
            {
                this.m_results.Add(result.Clone());
            }
        }

        public ResultCollection(ItemIdentifier item) : base(item)
        {
            this.m_results = new ArrayList();
        }

        public int Add(Result value)
        {
            return this.Add(value);
        }

        public int Add(object value)
        {
            if (!typeof(Result).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add Result objects into the collection.");
            }
            return this.m_results.Add(value);
        }

        public void Clear()
        {
            this.m_results.Clear();
        }

        public override object Clone()
        {
            ResultCollection results = (ResultCollection) base.Clone();
            results.m_results = new ArrayList(this.m_results.Count);
            foreach (ResultCollection results2 in this.m_results)
            {
                results.m_results.Add(results2.Clone());
            }
            return results;
        }

        public bool Contains(Result value)
        {
            return this.Contains(value);
        }

        public bool Contains(object value)
        {
            return this.m_results.Contains(value);
        }

        public void CopyTo(Array array, int index)
        {
            if (this.m_results != null)
            {
                this.m_results.CopyTo(array, index);
            }
        }

        public void CopyTo(Result[] array, int index)
        {
            this.CopyTo((Array) array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_results.GetEnumerator();
        }

        public int IndexOf(Result value)
        {
            return this.IndexOf(value);
        }

        public int IndexOf(object value)
        {
            return this.m_results.IndexOf(value);
        }

        public void Insert(int index, Result value)
        {
            this.Insert(index, value);
        }

        public void Insert(int index, object value)
        {
            if (!typeof(Result).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add Result objects into the collection.");
            }
            this.m_results.Insert(index, value);
        }

        public void Remove(Result value)
        {
            this.Remove(value);
        }

        public void Remove(object value)
        {
            this.m_results.Remove(value);
        }

        public void RemoveAt(int index)
        {
            this.m_results.RemoveAt(index);
        }

        public int Count
        {
            get
            {
                if (this.m_results == null)
                {
                    return 0;
                }
                return this.m_results.Count;
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

        public Result this[int index]
        {
            get
            {
                return (Result) this.m_results[index];
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

        object IList.this[int index]
        {
            get
            {
                return this.m_results[index];
            }
            set
            {
                if (!typeof(Result).IsInstanceOfType(value))
                {
                    throw new ArgumentException("May only add Result objects into the collection.");
                }
                this.m_results[index] = value;
            }
        }
    }
}

