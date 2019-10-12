namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class ItemValueCollection : Item, IResult, IActualTime, ICloneable, IList, ICollection, IEnumerable
    {
        private string m_diagnosticInfo;
        private DateTime m_endTime;
        private Opc.ResultID m_resultID;
        private DateTime m_startTime;
        private ArrayList m_values;

        public ItemValueCollection()
        {
            this.m_startTime = DateTime.MinValue;
            this.m_endTime = DateTime.MinValue;
            this.m_values = new ArrayList();
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
        }

        public ItemValueCollection(Item item) : base(item)
        {
            this.m_startTime = DateTime.MinValue;
            this.m_endTime = DateTime.MinValue;
            this.m_values = new ArrayList();
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
        }

        public ItemValueCollection(ItemValueCollection item) : base((Item) item)
        {
            this.m_startTime = DateTime.MinValue;
            this.m_endTime = DateTime.MinValue;
            this.m_values = new ArrayList();
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            this.m_values = new ArrayList(item.m_values.Count);
            foreach (ItemValue value2 in item.m_values)
            {
                if (value2 != null)
                {
                    this.m_values.Add(value2.Clone());
                }
            }
        }

        public ItemValueCollection(ItemIdentifier item) : base(item)
        {
            this.m_startTime = DateTime.MinValue;
            this.m_endTime = DateTime.MinValue;
            this.m_values = new ArrayList();
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
        }

        public int Add(ItemValue value)
        {
            return this.Add(value);
        }

        public int Add(object value)
        {
            if (!typeof(ItemValue).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add ItemValue objects into the collection.");
            }
            return this.m_values.Add(value);
        }

        public void AddRange(ItemValueCollection collection)
        {
            if (collection != null)
            {
                foreach (ItemValue value2 in collection)
                {
                    if (value2 != null)
                    {
                        this.m_values.Add(value2.Clone());
                    }
                }
            }
        }

        public void Clear()
        {
            this.m_values.Clear();
        }

        public override object Clone()
        {
            ItemValueCollection values = (ItemValueCollection) base.Clone();
            values.m_values = new ArrayList(this.m_values.Count);
            foreach (ItemValue value2 in this.m_values)
            {
                values.m_values.Add(value2.Clone());
            }
            return values;
        }

        public bool Contains(ItemValue value)
        {
            return this.Contains(value);
        }

        public bool Contains(object value)
        {
            return this.m_values.Contains(value);
        }

        public void CopyTo(Array array, int index)
        {
            if (this.m_values != null)
            {
                this.m_values.CopyTo(array, index);
            }
        }

        public void CopyTo(ItemValue[] array, int index)
        {
            this.CopyTo((Array) array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_values.GetEnumerator();
        }

        public int IndexOf(ItemValue value)
        {
            return this.IndexOf(value);
        }

        public int IndexOf(object value)
        {
            return this.m_values.IndexOf(value);
        }

        public void Insert(int index, ItemValue value)
        {
            this.Insert(index, value);
        }

        public void Insert(int index, object value)
        {
            if (!typeof(ItemValue).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add ItemValue objects into the collection.");
            }
            this.m_values.Insert(index, value);
        }

        public void Remove(ItemValue value)
        {
            this.Remove(value);
        }

        public void Remove(object value)
        {
            this.m_values.Remove(value);
        }

        public void RemoveAt(int index)
        {
            this.m_values.RemoveAt(index);
        }

        public int Count
        {
            get
            {
                if (this.m_values == null)
                {
                    return 0;
                }
                return this.m_values.Count;
            }
        }

        public string DiagnosticInfo
        {
            get
            {
                return this.m_diagnosticInfo;
            }
            set
            {
                this.m_diagnosticInfo = value;
            }
        }

        public DateTime EndTime
        {
            get
            {
                return this.m_endTime;
            }
            set
            {
                this.m_endTime = value;
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

        public ItemValue this[int index]
        {
            get
            {
                return (ItemValue) this.m_values[index];
            }
            set
            {
                this.m_values[index] = value;
            }
        }

        public Opc.ResultID ResultID
        {
            get
            {
                return this.m_resultID;
            }
            set
            {
                this.m_resultID = value;
            }
        }

        public DateTime StartTime
        {
            get
            {
                return this.m_startTime;
            }
            set
            {
                this.m_startTime = value;
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
                return this.m_values[index];
            }
            set
            {
                if (!typeof(ItemValue).IsInstanceOfType(value))
                {
                    throw new ArgumentException("May only add ItemValue objects into the collection.");
                }
                this.m_values[index] = value;
            }
        }
    }
}

