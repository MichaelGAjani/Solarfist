namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class AttributeValueCollection : IResult, ICloneable, IList, ICollection, IEnumerable
    {
        private int m_attributeID;
        private string m_diagnosticInfo;
        private Opc.ResultID m_resultID;
        private ArrayList m_values;

        public AttributeValueCollection()
        {
            this.m_attributeID = 0;
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            this.m_values = new ArrayList();
        }

        public AttributeValueCollection(Opc.Hda.Attribute attribute)
        {
            this.m_attributeID = 0;
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            this.m_values = new ArrayList();
            this.m_attributeID = attribute.ID;
        }

        public AttributeValueCollection(AttributeValueCollection collection)
        {
            this.m_attributeID = 0;
            this.m_resultID = Opc.ResultID.S_OK;
            this.m_diagnosticInfo = null;
            this.m_values = new ArrayList();
            this.m_values = new ArrayList(collection.m_values.Count);
            foreach (AttributeValue value2 in collection.m_values)
            {
                this.m_values.Add(value2.Clone());
            }
        }

        public int Add(AttributeValue value)
        {
            return this.Add(value);
        }

        public int Add(object value)
        {
            if (!typeof(AttributeValue).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add AttributeValue objects into the collection.");
            }
            return this.m_values.Add(value);
        }

        public void Clear()
        {
            this.m_values.Clear();
        }

        public virtual object Clone()
        {
            AttributeValueCollection values = (AttributeValueCollection) base.MemberwiseClone();
            values.m_values = new ArrayList(this.m_values.Count);
            foreach (AttributeValue value2 in this.m_values)
            {
                values.m_values.Add(value2.Clone());
            }
            return values;
        }

        public bool Contains(AttributeValue value)
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

        public void CopyTo(AttributeValue[] array, int index)
        {
            this.CopyTo((Array) array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_values.GetEnumerator();
        }

        public int IndexOf(AttributeValue value)
        {
            return this.IndexOf(value);
        }

        public int IndexOf(object value)
        {
            return this.m_values.IndexOf(value);
        }

        public void Insert(int index, AttributeValue value)
        {
            this.Insert(index, value);
        }

        public void Insert(int index, object value)
        {
            if (!typeof(AttributeValue).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add AttributeValue objects into the collection.");
            }
            this.m_values.Insert(index, value);
        }

        public void Remove(AttributeValue value)
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

        public int AttributeID
        {
            get
            {
                return this.m_attributeID;
            }
            set
            {
                this.m_attributeID = value;
            }
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

        public AttributeValue this[int index]
        {
            get
            {
                return (AttributeValue) this.m_values[index];
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
                if (!typeof(AttributeValue).IsInstanceOfType(value))
                {
                    throw new ArgumentException("May only add AttributeValue objects into the collection.");
                }
                this.m_values[index] = value;
            }
        }
    }
}

