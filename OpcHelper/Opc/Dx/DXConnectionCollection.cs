namespace Jund.OpcHelper.Opc.Dx
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.Serialization;

    [Serializable]
    public class DXConnectionCollection : ICloneable, IList, ICollection, IEnumerable, ISerializable
    {
        private ArrayList m_connections;

        internal DXConnectionCollection()
        {
            this.m_connections = new ArrayList();
        }

        internal DXConnectionCollection(ICollection connections)
        {
            this.m_connections = new ArrayList();
            if (connections != null)
            {
                foreach (DXConnection connection in connections)
                {
                    this.m_connections.Add(connection);
                }
            }
        }

        protected DXConnectionCollection(SerializationInfo info, StreamingContext context)
        {
            this.m_connections = new ArrayList();
            DXConnection[] connectionArray = (DXConnection[]) info.GetValue("Connections", typeof(DXConnection[]));
            if (connectionArray != null)
            {
                foreach (DXConnection connection in connectionArray)
                {
                    this.m_connections.Add(connection);
                }
            }
        }

        public int Add(DXConnection value)
        {
            return this.Add(value);
        }

        public int Add(object value)
        {
            this.Insert(this.m_connections.Count, value);
            return (this.m_connections.Count - 1);
        }

        public void Clear()
        {
            this.m_connections.Clear();
        }

        public virtual object Clone()
        {
            DXConnectionCollection connections = (DXConnectionCollection) base.MemberwiseClone();
            connections.m_connections = new ArrayList();
            foreach (DXConnection connection in this.m_connections)
            {
                connections.m_connections.Add(connection.Clone());
            }
            return connections;
        }

        public bool Contains(DXConnection value)
        {
            return this.Contains(value);
        }

        public bool Contains(object value)
        {
            foreach (ItemIdentifier identifier in this.m_connections)
            {
                if (identifier.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(Array array, int index)
        {
            if (this.m_connections != null)
            {
                this.m_connections.CopyTo(array, index);
            }
        }

        public void CopyTo(DXConnection[] array, int index)
        {
            this.CopyTo((Array) array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_connections.GetEnumerator();
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            DXConnection[] connectionArray = null;
            if (this.m_connections.Count > 0)
            {
                connectionArray = new DXConnection[this.m_connections.Count];
                for (int i = 0; i < connectionArray.Length; i++)
                {
                    connectionArray[i] = (DXConnection) this.m_connections[i];
                }
            }
            info.AddValue("Connections", connectionArray);
        }

        public int IndexOf(DXConnection value)
        {
            return this.IndexOf(value);
        }

        public int IndexOf(object value)
        {
            return this.m_connections.IndexOf(value);
        }

        public void Insert(int index, DXConnection value)
        {
            this.Insert(index, value);
        }

        public void Insert(int index, object value)
        {
            if (!typeof(DXConnection).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add DXConnection objects into the collection.");
            }
            this.m_connections.Insert(index, (DXConnection) value);
        }

        public void Remove(DXConnection value)
        {
            this.Remove(value);
        }

        public void Remove(object value)
        {
            if (!typeof(ItemIdentifier).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only delete Opc.Dx.ItemIdentifier obejcts from the collection.");
            }
            foreach (ItemIdentifier identifier in this.m_connections)
            {
                if (identifier.Equals(value))
                {
                    this.m_connections.Remove(identifier);
                    break;
                }
            }
        }

        public void RemoveAt(int index)
        {
            if ((index < 0) || (index >= this.m_connections.Count))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            this.Remove(this.m_connections[index]);
        }

        public DXConnection[] ToArray()
        {
            return (DXConnection[]) this.m_connections.ToArray(typeof(DXConnection));
        }

        public int Count
        {
            get
            {
                if (this.m_connections == null)
                {
                    return 0;
                }
                return this.m_connections.Count;
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

        public DXConnection this[int index]
        {
            get
            {
                return (DXConnection) this.m_connections[index];
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
                return this.m_connections[index];
            }
            set
            {
                this.Insert(index, value);
            }
        }

        private class Names
        {
            internal const string CONNECTIONS = "Connections";
        }
    }
}

