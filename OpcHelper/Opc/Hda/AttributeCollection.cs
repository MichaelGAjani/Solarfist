namespace Jund.OpcHelper.Opc.Hda
{
    using Opc;
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class AttributeCollection : ICloneable, ICollection, IEnumerable
    {
        private Opc.Hda.Attribute[] m_attributes;

        public AttributeCollection()
        {
            this.m_attributes = new Opc.Hda.Attribute[0];
        }

        public AttributeCollection(ICollection collection)
        {
            this.m_attributes = new Opc.Hda.Attribute[0];
            this.Init(collection);
        }

        public void Clear()
        {
            this.m_attributes = new Opc.Hda.Attribute[0];
        }

        public virtual object Clone()
        {
            return new AttributeCollection(this);
        }

        public void CopyTo(Array array, int index)
        {
            if (this.m_attributes != null)
            {
                this.m_attributes.CopyTo(array, index);
            }
        }

        public void CopyTo(Opc.Hda.Attribute[] array, int index)
        {
            this.CopyTo((Array) array, index);
        }

        public Opc.Hda.Attribute Find(int id)
        {
            foreach (Opc.Hda.Attribute attribute in this.m_attributes)
            {
                if (attribute.ID == id)
                {
                    return attribute;
                }
            }
            return null;
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_attributes.GetEnumerator();
        }

        public void Init(ICollection collection)
        {
            this.Clear();
            if (collection != null)
            {
                ArrayList list = new ArrayList(collection.Count);
                foreach (object obj2 in collection)
                {
                    if (obj2.GetType() == typeof(Opc.Hda.Attribute))
                    {
                        list.Add(Opc.Convert.Clone(obj2));
                    }
                }
                this.m_attributes = (Opc.Hda.Attribute[]) list.ToArray(typeof(Opc.Hda.Attribute));
            }
        }

        public int Count
        {
            get
            {
                if (this.m_attributes == null)
                {
                    return 0;
                }
                return this.m_attributes.Length;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public Opc.Hda.Attribute this[int index]
        {
            get
            {
                return this.m_attributes[index];
            }
            set
            {
                this.m_attributes[index] = value;
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

