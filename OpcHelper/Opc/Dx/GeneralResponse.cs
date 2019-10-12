namespace Jund.OpcHelper.Opc.Dx
{
    using Opc;
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class GeneralResponse : ICloneable, ICollection, IEnumerable
    {
        private Opc.Dx.IdentifiedResult[] m_results;
        private string m_version;

        public GeneralResponse()
        {
            this.m_version = null;
            this.m_results = new Opc.Dx.IdentifiedResult[0];
        }

        public GeneralResponse(string version, ICollection results)
        {
            this.m_version = null;
            this.m_results = new Opc.Dx.IdentifiedResult[0];
            this.Version = version;
            this.Init(results);
        }

        public void Clear()
        {
            this.m_results = new Opc.Dx.IdentifiedResult[0];
        }

        public virtual object Clone()
        {
            return new IdentifiedResultCollection(this);
        }

        public void CopyTo(Array array, int index)
        {
            if (this.m_results != null)
            {
                this.m_results.CopyTo(array, index);
            }
        }

        public void CopyTo(Opc.Dx.IdentifiedResult[] array, int index)
        {
            this.CopyTo((Array) array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_results.GetEnumerator();
        }

        private void Init(ICollection collection)
        {
            this.Clear();
            if (collection != null)
            {
                ArrayList list = new ArrayList(collection.Count);
                foreach (object obj2 in collection)
                {
                    if (typeof(Opc.Dx.IdentifiedResult).IsInstanceOfType(obj2))
                    {
                        list.Add(((Opc.Dx.IdentifiedResult) obj2).Clone());
                    }
                }
                this.m_results = (Opc.Dx.IdentifiedResult[]) list.ToArray(typeof(Opc.Dx.IdentifiedResult));
            }
        }

        public int Count
        {
            get
            {
                if (this.m_results == null)
                {
                    return 0;
                }
                return this.m_results.Length;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public Opc.Dx.IdentifiedResult this[int index]
        {
            get
            {
                return this.m_results[index];
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

        public string Version
        {
            get
            {
                return this.m_version;
            }
            set
            {
                this.m_version = value;
            }
        }
    }
}

