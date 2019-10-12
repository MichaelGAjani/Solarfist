namespace Jund.OpcHelper.Opc.Dx
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class SourceServerCollection : ICollection, IEnumerable, ICloneable
    {
        private ArrayList m_servers = new ArrayList();

        internal SourceServerCollection()
        {
        }

        public virtual object Clone()
        {
            SourceServerCollection servers = (SourceServerCollection) base.MemberwiseClone();
            servers.m_servers = new ArrayList();
            foreach (SourceServer server in this.m_servers)
            {
                servers.m_servers.Add(server.Clone());
            }
            return servers;
        }

        public void CopyTo(Array array, int index)
        {
            if (this.m_servers != null)
            {
                this.m_servers.CopyTo(array, index);
            }
        }

        public void CopyTo(SourceServer[] array, int index)
        {
            this.CopyTo((Array) array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_servers.GetEnumerator();
        }

        internal void Initialize(ICollection sourceServers)
        {
            this.m_servers.Clear();
            if (sourceServers != null)
            {
                foreach (SourceServer server in sourceServers)
                {
                    this.m_servers.Add(server);
                }
            }
        }

        public int Count
        {
            get
            {
                if (this.m_servers == null)
                {
                    return 0;
                }
                return this.m_servers.Count;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public SourceServer this[int index]
        {
            get
            {
                return (SourceServer) this.m_servers[index];
            }
        }

        public SourceServer this[string name]
        {
            get
            {
                foreach (SourceServer server in this.m_servers)
                {
                    if (server.Name == name)
                    {
                        return server;
                    }
                }
                return null;
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

