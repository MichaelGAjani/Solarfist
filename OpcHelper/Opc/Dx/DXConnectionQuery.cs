namespace Jund.OpcHelper.Opc.Dx
{
    using Opc;
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    public class DXConnectionQuery
    {
        private string m_browsePath;
        private DXConnectionCollection m_masks;
        private string m_name;
        private bool m_recursive;

        public DXConnectionQuery()
        {
            this.m_name = null;
            this.m_browsePath = null;
            this.m_masks = new DXConnectionCollection();
            this.m_recursive = false;
        }

        public DXConnectionQuery(DXConnectionQuery query)
        {
            this.m_name = null;
            this.m_browsePath = null;
            this.m_masks = new DXConnectionCollection();
            this.m_recursive = false;
            if (query != null)
            {
                this.Name = query.Name;
                this.BrowsePath = query.BrowsePath;
                this.Recursive = query.Recursive;
                this.m_masks = new DXConnectionCollection(query.Masks);
            }
        }

        public virtual object Clone()
        {
            return new DXConnectionQuery(this);
        }

        public GeneralResponse CopyDefaultAttributes(Opc.Dx.Server server, bool configToStatus, out ResultID[] errors)
        {
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }
            return server.CopyDXConnectionDefaultAttributes(configToStatus, this.BrowsePath, this.Masks.ToArray(), this.Recursive, out errors);
        }

        public GeneralResponse Delete(Opc.Dx.Server server, out ResultID[] errors)
        {
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }
            return server.DeleteDXConnections(this.BrowsePath, this.Masks.ToArray(), this.Recursive, out errors);
        }

        public DXConnection[] Query(Opc.Dx.Server server, out ResultID[] errors)
        {
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }
            return server.QueryDXConnections(this.BrowsePath, this.Masks.ToArray(), this.Recursive, out errors);
        }

        public GeneralResponse Update(Opc.Dx.Server server, DXConnection connectionDefinition, out ResultID[] errors)
        {
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }
            return server.UpdateDXConnections(this.BrowsePath, this.Masks.ToArray(), this.Recursive, connectionDefinition, out errors);
        }

        public string BrowsePath
        {
            get
            {
                return this.m_browsePath;
            }
            set
            {
                this.m_browsePath = value;
            }
        }

        public DXConnectionCollection Masks
        {
            get
            {
                return this.m_masks;
            }
        }

        public string Name
        {
            get
            {
                return this.m_name;
            }
            set
            {
                this.m_name = value;
            }
        }

        public bool Recursive
        {
            get
            {
                return this.m_recursive;
            }
            set
            {
                this.m_recursive = value;
            }
        }
    }
}

