namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.Serialization;

    [Serializable]
    public class WriteableCollection : IList, ICollection, IEnumerable, ICloneable, ISerializable
    {
        protected const string INVALID_TYPE = "A value with type '{0}' cannot be added to the collection.";
        protected const string INVALID_VALUE = "The value '{0}' cannot be added to the collection.";
        private ArrayList m_array;
        private System.Type m_elementType;

        protected WriteableCollection(ICollection array, System.Type elementType)
        {
            this.m_array = null;
            this.m_elementType = null;
            if (array != null)
            {
                this.m_array = new ArrayList(array);
            }
            else
            {
                this.m_array = new ArrayList();
            }
            this.m_elementType = typeof(object);
            if (elementType != null)
            {
                foreach (object obj2 in this.m_array)
                {
                    this.ValidateElement(obj2);
                }
                this.m_elementType = elementType;
            }
        }

        protected WriteableCollection(SerializationInfo info, StreamingContext context)
        {
            this.m_array = null;
            this.m_elementType = null;
            this.m_elementType = (System.Type) info.GetValue("ET", typeof(System.Type));
            int capacity = (int) info.GetValue("CT", typeof(int));
            this.m_array = new ArrayList(capacity);
            for (int i = 0; i < capacity; i++)
            {
                this.m_array.Add(info.GetValue("EL" + i.ToString(), typeof(object)));
            }
        }

        public virtual int Add(object value)
        {
            this.ValidateElement(value);
            return this.m_array.Add(value);
        }

        public virtual void AddRange(ICollection collection)
        {
            if (collection != null)
            {
                foreach (object obj2 in collection)
                {
                    this.ValidateElement(obj2);
                }
                this.m_array.AddRange(collection);
            }
        }

        public virtual void Clear()
        {
            this.m_array.Clear();
        }

        public virtual object Clone()
        {
            WriteableCollection writeables = (WriteableCollection) base.MemberwiseClone();
            writeables.m_array = new ArrayList();
            for (int i = 0; i < this.m_array.Count; i++)
            {
                writeables.Add(Opc.Convert.Clone(this.m_array[i]));
            }
            return writeables;
        }

        public virtual bool Contains(object value)
        {
            return this.m_array.Contains(value);
        }

        public virtual void CopyTo(System.Array array, int index)
        {
            if (this.m_array != null)
            {
                this.m_array.CopyTo(array, index);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_array.GetEnumerator();
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ET", this.m_elementType);
            info.AddValue("CT", this.m_array.Count);
            for (int i = 0; i < this.m_array.Count; i++)
            {
                info.AddValue("EL" + i.ToString(), this.m_array[i]);
            }
        }

        public virtual int IndexOf(object value)
        {
            return this.m_array.IndexOf(value);
        }

        public virtual void Insert(int index, object value)
        {
            this.ValidateElement(value);
            this.m_array.Insert(index, value);
        }

        public virtual void Remove(object value)
        {
            this.m_array.Remove(value);
        }

        public virtual void RemoveAt(int index)
        {
            this.m_array.RemoveAt(index);
        }

        public virtual System.Array ToArray()
        {
            return this.m_array.ToArray(this.m_elementType);
        }

        protected virtual void ValidateElement(object element)
        {
            if (element == null)
            {
                throw new ArgumentException(string.Format("The value '{0}' cannot be added to the collection.", element));
            }
            if (!this.m_elementType.IsInstanceOfType(element))
            {
                throw new ArgumentException(string.Format("A value with type '{0}' cannot be added to the collection.", element.GetType()));
            }
        }

        protected virtual ArrayList Array
        {
            get
            {
                return this.m_array;
            }
            set
            {
                this.m_array = value;
                if (this.m_array == null)
                {
                    this.m_array = new ArrayList();
                }
            }
        }

        public virtual int Count
        {
            get
            {
                return this.m_array.Count;
            }
        }

        protected virtual System.Type ElementType
        {
            get
            {
                return this.m_elementType;
            }
            set
            {
                foreach (object obj2 in this.m_array)
                {
                    this.ValidateElement(obj2);
                }
                this.m_elementType = value;
            }
        }

        public virtual bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public virtual bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public virtual bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public virtual object this[int index]
        {
            get
            {
                return this.m_array[index];
            }
            set
            {
                this.m_array[index] = value;
            }
        }

        public virtual object SyncRoot
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
                return this[index];
            }
            set
            {
                this[index] = value;
            }
        }

        private class Names
        {
            internal const string COUNT = "CT";
            internal const string ELEMENT = "EL";
            internal const string ELEMENT_TYPE = "ET";
        }
    }
}

