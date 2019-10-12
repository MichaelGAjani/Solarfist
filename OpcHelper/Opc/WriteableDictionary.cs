namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.Serialization;

    [Serializable]
    public class WriteableDictionary : IDictionary, ICollection, IEnumerable, ISerializable
    {
        protected const string INVALID_TYPE = "A {1} with type '{0}' cannot be added to the dictionary.";
        protected const string INVALID_VALUE = "The {1} '{0}' cannot be added to the dictionary.";
        private Hashtable m_dictionary;
        private System.Type m_keyType;
        private System.Type m_valueType;

        protected WriteableDictionary(SerializationInfo info, StreamingContext context)
        {
            this.m_dictionary = new Hashtable();
            this.m_keyType = null;
            this.m_valueType = null;
            this.m_keyType = (System.Type) info.GetValue("KT", typeof(System.Type));
            this.m_valueType = (System.Type) info.GetValue("VT", typeof(System.Type));
            int num = (int) info.GetValue("CT", typeof(int));
            this.m_dictionary = new Hashtable();
            for (int i = 0; i < num; i++)
            {
                object obj2 = info.GetValue("KY" + i.ToString(), typeof(object));
                object obj3 = info.GetValue("VA" + i.ToString(), typeof(object));
                if (obj2 != null)
                {
                    this.m_dictionary[obj2] = obj3;
                }
            }
        }

        protected WriteableDictionary(IDictionary dictionary, System.Type keyType, System.Type valueType)
        {
            this.m_dictionary = new Hashtable();
            this.m_keyType = null;
            this.m_valueType = null;
            this.m_keyType = (keyType == null) ? typeof(object) : keyType;
            this.m_valueType = (valueType == null) ? typeof(object) : valueType;
            this.Dictionary = dictionary;
        }

        public virtual void Add(object key, object value)
        {
            this.ValidateKey(key, this.m_keyType);
            this.ValidateValue(value, this.m_valueType);
            this.m_dictionary.Add(key, value);
        }

        public virtual void Clear()
        {
            this.m_dictionary.Clear();
        }

        public virtual object Clone()
        {
            WriteableDictionary dictionary = (WriteableDictionary) base.MemberwiseClone();
            Hashtable hashtable = new Hashtable();
            IDictionaryEnumerator enumerator = this.m_dictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                hashtable.Add(Opc.Convert.Clone(enumerator.Key), Opc.Convert.Clone(enumerator.Value));
            }
            dictionary.m_dictionary = hashtable;
            return dictionary;
        }

        public virtual bool Contains(object key)
        {
            return this.m_dictionary.Contains(key);
        }

        public virtual void CopyTo(Array array, int index)
        {
            if (this.m_dictionary != null)
            {
                this.m_dictionary.CopyTo(array, index);
            }
        }

        public virtual IDictionaryEnumerator GetEnumerator()
        {
            return this.m_dictionary.GetEnumerator();
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("KT", this.m_keyType);
            info.AddValue("VT", this.m_valueType);
            info.AddValue("CT", this.m_dictionary.Count);
            int num = 0;
            IDictionaryEnumerator enumerator = this.m_dictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                info.AddValue("KY" + num.ToString(), enumerator.Key);
                info.AddValue("VA" + num.ToString(), enumerator.Value);
                num++;
            }
        }

        public virtual void Remove(object key)
        {
            this.m_dictionary.Remove(key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        protected virtual void ValidateKey(object element, System.Type type)
        {
            if (element == null)
            {
                throw new ArgumentException(string.Format("The {1} '{0}' cannot be added to the dictionary.", element, "key"));
            }
            if (!type.IsInstanceOfType(element))
            {
                throw new ArgumentException(string.Format("A {1} with type '{0}' cannot be added to the dictionary.", element.GetType(), "key"));
            }
        }

        protected virtual void ValidateValue(object element, System.Type type)
        {
            if ((element != null) && !type.IsInstanceOfType(element))
            {
                throw new ArgumentException(string.Format("A {1} with type '{0}' cannot be added to the dictionary.", element.GetType(), "value"));
            }
        }

        public virtual int Count
        {
            get
            {
                return this.m_dictionary.Count;
            }
        }

        protected virtual IDictionary Dictionary
        {
            get
            {
                return this.m_dictionary;
            }
            set
            {
                if (value != null)
                {
                    if (this.m_keyType != null)
                    {
                        foreach (object obj2 in value.Keys)
                        {
                            this.ValidateKey(obj2, this.m_keyType);
                        }
                    }
                    if (this.m_valueType != null)
                    {
                        foreach (object obj3 in value.Values)
                        {
                            this.ValidateValue(obj3, this.m_valueType);
                        }
                    }
                    this.m_dictionary = new Hashtable(value);
                }
                else
                {
                    this.m_dictionary = new Hashtable();
                }
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

        public virtual object this[object key]
        {
            get
            {
                return this.m_dictionary[key];
            }
            set
            {
                this.ValidateKey(key, this.m_keyType);
                this.ValidateValue(value, this.m_valueType);
                this.m_dictionary[key] = value;
            }
        }

        public virtual ICollection Keys
        {
            get
            {
                return this.m_dictionary.Keys;
            }
        }

        protected System.Type KeyType
        {
            get
            {
                return this.m_keyType;
            }
            set
            {
                foreach (object obj2 in this.m_dictionary.Keys)
                {
                    this.ValidateKey(obj2, value);
                }
                this.m_keyType = value;
            }
        }

        public virtual object SyncRoot
        {
            get
            {
                return this;
            }
        }

        public virtual ICollection Values
        {
            get
            {
                return this.m_dictionary.Values;
            }
        }

        protected System.Type ValueType
        {
            get
            {
                return this.m_valueType;
            }
            set
            {
                foreach (object obj2 in this.m_dictionary.Values)
                {
                    this.ValidateValue(obj2, value);
                }
                this.m_valueType = value;
            }
        }

        private class Names
        {
            internal const string COUNT = "CT";
            internal const string KEY = "KY";
            internal const string KEY_TYPE = "KT";
            internal const string VALUE = "VA";
            internal const string VALUE_VALUE = "VT";
        }
    }
}

