namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.Serialization;

    [Serializable]
    public class ReadOnlyDictionary : IDictionary, ICollection, IEnumerable, ISerializable
    {
        private Hashtable m_dictionary;
        private const string READ_ONLY_DICTIONARY = "Cannot change the contents of a read-only dictionary";

        protected ReadOnlyDictionary(Hashtable dictionary)
        {
            this.m_dictionary = new Hashtable();
            this.Dictionary = dictionary;
        }

        protected ReadOnlyDictionary(SerializationInfo info, StreamingContext context)
        {
            this.m_dictionary = new Hashtable();
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

        public void Add(object key, object value)
        {
            throw new InvalidOperationException("Cannot change the contents of a read-only dictionary");
        }

        public virtual void Clear()
        {
            throw new InvalidOperationException("Cannot change the contents of a read-only dictionary");
        }

        public virtual object Clone()
        {
            ReadOnlyDictionary dictionary = (ReadOnlyDictionary) base.MemberwiseClone();
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
            throw new InvalidOperationException("Cannot change the contents of a read-only dictionary");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public virtual int Count
        {
            get
            {
                return this.m_dictionary.Count;
            }
        }

        protected virtual Hashtable Dictionary
        {
            get
            {
                return this.m_dictionary;
            }
            set
            {
                this.m_dictionary = value;
                if (this.m_dictionary == null)
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
                return true;
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
                throw new InvalidOperationException("Cannot change the contents of a read-only dictionary");
            }
        }

        public virtual ICollection Keys
        {
            get
            {
                return this.m_dictionary.Keys;
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

        private class Names
        {
            internal const string COUNT = "CT";
            internal const string KEY = "KY";
            internal const string VALUE = "VA";
        }
    }
}

