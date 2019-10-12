namespace Jund.OpcHelper.Opc
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.Serialization;

    [Serializable]
    public class ReadOnlyCollection : ICollection, IEnumerable, ICloneable, ISerializable
    {
        private System.Array m_array;

        protected ReadOnlyCollection(System.Array array)
        {
            this.m_array = null;
            this.Array = array;
        }

        protected ReadOnlyCollection(SerializationInfo info, StreamingContext context)
        {
            this.m_array = null;
            this.m_array = (System.Array) info.GetValue("AR", typeof(System.Array));
        }

        public virtual object Clone()
        {
            ReadOnlyCollection onlys = (ReadOnlyCollection) base.MemberwiseClone();
            ArrayList list = new ArrayList(this.m_array.Length);
            System.Type baseType = null;
            for (int i = 0; i < this.m_array.Length; i++)
            {
                object o = this.m_array.GetValue(i);
                if (baseType == null)
                {
                    baseType = o.GetType();
                }
                else if (baseType != typeof(object))
                {
                    while (!baseType.IsInstanceOfType(o))
                    {
                        baseType = baseType.BaseType;
                    }
                }
                list.Add(Opc.Convert.Clone(o));
            }
            onlys.Array = list.ToArray(baseType);
            return onlys;
        }

        public virtual void CopyTo(System.Array array, int index)
        {
            if (this.m_array != null)
            {
                this.m_array.CopyTo(array, index);
            }
        }

        public virtual IEnumerator GetEnumerator()
        {
            return this.m_array.GetEnumerator();
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("AR", this.m_array);
        }

        public virtual System.Array ToArray()
        {
            return (System.Array) Opc.Convert.Clone(this.m_array);
        }

        protected virtual System.Array Array
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
                    this.m_array = new object[0];
                }
            }
        }

        public virtual int Count
        {
            get
            {
                return this.m_array.Length;
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
                return this.m_array.GetValue(index);
            }
        }

        public virtual object SyncRoot
        {
            get
            {
                return this;
            }
        }

        private class Names
        {
            internal const string ARRAY = "AR";
        }
    }
}

