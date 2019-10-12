namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class ItemAttributeCollection : ItemIdentifier, IResult, IActualTime, IList, ICollection, IEnumerable
    {
        private ArrayList m_attributes;
        private string m_diagnosticInfo;
        private DateTime m_endTime;
        private Opc.ResultID m_resultID;
        private DateTime m_startTime;

        public ItemAttributeCollection()
        {
            this.m_startTime = DateTime.MinValue;
            this.m_endTime = DateTime.MinValue;
            this.m_attributes = new ArrayList();
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
        }

        public ItemAttributeCollection(ItemAttributeCollection item) : base(item)
        {
            this.m_startTime = DateTime.MinValue;
            this.m_endTime = DateTime.MinValue;
            this.m_attributes = new ArrayList();
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            this.m_attributes = new ArrayList(item.m_attributes.Count);
            foreach (AttributeValueCollection values in item.m_attributes)
            {
                if (values != null)
                {
                    this.m_attributes.Add(values.Clone());
                }
            }
        }

        public ItemAttributeCollection(ItemIdentifier item) : base(item)
        {
            this.m_startTime = DateTime.MinValue;
            this.m_endTime = DateTime.MinValue;
            this.m_attributes = new ArrayList();
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
        }

        public int Add(AttributeValueCollection value)
        {
            return this.Add(value);
        }

        public int Add(object value)
        {
            if (!typeof(AttributeValueCollection).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add AttributeValueCollection objects into the collection.");
            }
            return this.m_attributes.Add(value);
        }

        public void Clear()
        {
            this.m_attributes.Clear();
        }

        public override object Clone()
        {
            ItemAttributeCollection attributes = (ItemAttributeCollection) base.Clone();
            attributes.m_attributes = new ArrayList(this.m_attributes.Count);
            foreach (AttributeValueCollection values in this.m_attributes)
            {
                attributes.m_attributes.Add(values.Clone());
            }
            return attributes;
        }

        public bool Contains(AttributeValueCollection value)
        {
            return this.Contains(value);
        }

        public bool Contains(object value)
        {
            return this.m_attributes.Contains(value);
        }

        public void CopyTo(Array array, int index)
        {
            if (this.m_attributes != null)
            {
                this.m_attributes.CopyTo(array, index);
            }
        }

        public void CopyTo(AttributeValueCollection[] array, int index)
        {
            this.CopyTo((Array) array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_attributes.GetEnumerator();
        }

        public int IndexOf(AttributeValueCollection value)
        {
            return this.IndexOf(value);
        }

        public int IndexOf(object value)
        {
            return this.m_attributes.IndexOf(value);
        }

        public void Insert(int index, AttributeValueCollection value)
        {
            this.Insert(index, value);
        }

        public void Insert(int index, object value)
        {
            if (!typeof(AttributeValueCollection).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add AttributeValueCollection objects into the collection.");
            }
            this.m_attributes.Insert(index, value);
        }

        public void Remove(AttributeValueCollection value)
        {
            this.Remove(value);
        }

        public void Remove(object value)
        {
            this.m_attributes.Remove(value);
        }

        public void RemoveAt(int index)
        {
            this.m_attributes.RemoveAt(index);
        }

        public int Count
        {
            get
            {
                if (this.m_attributes == null)
                {
                    return 0;
                }
                return this.m_attributes.Count;
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

        public AttributeValueCollection this[int index]
        {
            get
            {
                return (AttributeValueCollection) this.m_attributes[index];
            }
            set
            {
                this.m_attributes[index] = value;
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
                return this.m_attributes[index];
            }
            set
            {
                if (!typeof(AttributeValueCollection).IsInstanceOfType(value))
                {
                    throw new ArgumentException("May only add AttributeValueCollection objects into the collection.");
                }
                this.m_attributes[index] = value;
            }
        }
    }
}

