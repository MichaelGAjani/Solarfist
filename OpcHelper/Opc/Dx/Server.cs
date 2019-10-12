namespace Jund.OpcHelper.Opc.Dx
{
    using Opc;
    using Opc.Da;
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    [Serializable]
    public class Server : Opc.Da.Server, Opc.Dx.IServer, Opc.Da.IServer, Opc.IServer, IDisposable, ISerializable
    {
        private DXConnectionQueryCollection m_connectionQueries;
        private SourceServerCollection m_sourceServers;
        private string m_version;

        public Server(Factory factory, URL url) : base(factory, url)
        {
            this.m_version = null;
            this.m_sourceServers = new SourceServerCollection();
            this.m_connectionQueries = new DXConnectionQueryCollection();
        }

        protected Server(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.m_version = null;
            this.m_sourceServers = new SourceServerCollection();
            this.m_connectionQueries = new DXConnectionQueryCollection();
            DXConnectionQuery[] queryArray = (DXConnectionQuery[]) info.GetValue("Queries", typeof(DXConnectionQuery[]));
            if (queryArray != null)
            {
                foreach (DXConnectionQuery query in queryArray)
                {
                    this.m_connectionQueries.Add(query);
                }
            }
        }

        public DXConnection AddDXConnection(DXConnection connection)
        {
            GeneralResponse response = this.AddDXConnections(new DXConnection[] { connection });
            if ((response == null) || (response.Count != 1))
            {
                throw new InvalidResponseException();
            }
            if (response[0].ResultID.Failed())
            {
                throw new ResultIDException(response[0].ResultID);
            }
            return new DXConnection(connection) { ItemName = response[0].ItemName, ItemPath = response[0].ItemPath, Version = response[0].Version };
        }

        public GeneralResponse AddDXConnections(DXConnection[] connections)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            GeneralResponse response = ((Opc.Dx.IServer) base.m_server).AddDXConnections(connections);
            if (response != null)
            {
                this.m_version = response.Version;
            }
            return response;
        }

        public SourceServer AddSourceServer(SourceServer server)
        {
            GeneralResponse response = this.AddSourceServers(new SourceServer[] { server });
            if ((response == null) || (response.Count != 1))
            {
                throw new InvalidResponseException();
            }
            if (response[0].ResultID.Failed())
            {
                throw new ResultIDException(response[0].ResultID);
            }
            return new SourceServer(server) { ItemName = response[0].ItemName, ItemPath = response[0].ItemPath, Version = response[0].Version };
        }

        public GeneralResponse AddSourceServers(SourceServer[] servers)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            GeneralResponse response = ((Opc.Dx.IServer) base.m_server).AddSourceServers(servers);
            if (response != null)
            {
                this.GetSourceServers();
                this.m_version = response.Version;
            }
            return response;
        }

        public GeneralResponse CopyDefaultSourceServerAttributes(bool configToStatus, Opc.Dx.ItemIdentifier[] servers)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            GeneralResponse response = ((Opc.Dx.IServer) base.m_server).CopyDefaultSourceServerAttributes(configToStatus, servers);
            if (response != null)
            {
                if (!configToStatus)
                {
                    this.GetSourceServers();
                }
                this.m_version = response.Version;
            }
            return response;
        }

        public GeneralResponse CopyDXConnectionDefaultAttributes(bool configToStatus, string browsePath, DXConnection[] connectionMasks, bool recursive, out ResultID[] errors)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            GeneralResponse response = ((Opc.Dx.IServer) base.m_server).CopyDXConnectionDefaultAttributes(configToStatus, browsePath, connectionMasks, recursive, out errors);
            if (response != null)
            {
                this.m_version = response.Version;
            }
            return response;
        }

        public void DeleteDXConnections(DXConnection connection)
        {
            ResultID[] errors = null;
            GeneralResponse response = this.DeleteDXConnections(null, new DXConnection[] { connection }, true, out errors);
            if (((errors != null) && (errors.Length > 0)) && errors[0].Failed())
            {
                throw new ResultIDException(errors[0]);
            }
            if ((response == null) || (response.Count != 1))
            {
                throw new InvalidResponseException();
            }
            if (response[0].ResultID.Failed())
            {
                throw new ResultIDException(response[0].ResultID);
            }
        }

        public GeneralResponse DeleteDXConnections(string browsePath, DXConnection[] connectionMasks, bool recursive, out ResultID[] errors)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            GeneralResponse response = ((Opc.Dx.IServer) base.m_server).DeleteDXConnections(browsePath, connectionMasks, recursive, out errors);
            if (response != null)
            {
                this.m_version = response.Version;
            }
            return response;
        }

        public void DeleteSourceServer(SourceServer server)
        {
            GeneralResponse response = this.DeleteSourceServers(new Opc.Dx.ItemIdentifier[] { server });
            if ((response == null) || (response.Count != 1))
            {
                throw new InvalidResponseException();
            }
            if (response[0].ResultID.Failed())
            {
                throw new ResultIDException(response[0].ResultID);
            }
        }

        public GeneralResponse DeleteSourceServers(Opc.Dx.ItemIdentifier[] servers)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            GeneralResponse response = ((Opc.Dx.IServer) base.m_server).DeleteSourceServers(servers);
            if (response != null)
            {
                this.GetSourceServers();
                this.m_version = response.Version;
            }
            return response;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            DXConnectionQuery[] queryArray = null;
            if (this.m_connectionQueries.Count > 0)
            {
                queryArray = new DXConnectionQuery[this.m_connectionQueries.Count];
                for (int i = 0; i < queryArray.Length; i++)
                {
                    queryArray[i] = this.m_connectionQueries[i];
                }
            }
            info.AddValue("Queries", queryArray);
        }

        public SourceServer[] GetSourceServers()
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            SourceServer[] sourceServers = ((Opc.Dx.IServer) base.m_server).GetSourceServers();
            this.m_sourceServers.Initialize(sourceServers);
            return sourceServers;
        }

        public DXConnection ModifyDXConnection(DXConnection connection)
        {
            GeneralResponse response = this.ModifyDXConnections(new DXConnection[] { connection });
            if ((response == null) || (response.Count != 1))
            {
                throw new InvalidResponseException();
            }
            if (response[0].ResultID.Failed())
            {
                throw new ResultIDException(response[0].ResultID);
            }
            return new DXConnection(connection) { ItemName = response[0].ItemName, ItemPath = response[0].ItemPath, Version = response[0].Version };
        }

        public GeneralResponse ModifyDXConnections(DXConnection[] connections)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            GeneralResponse response = ((Opc.Dx.IServer) base.m_server).ModifyDXConnections(connections);
            if (response != null)
            {
                this.m_version = response.Version;
            }
            return response;
        }

        public SourceServer ModifySourceServer(SourceServer server)
        {
            GeneralResponse response = this.ModifySourceServers(new SourceServer[] { server });
            if ((response == null) || (response.Count != 1))
            {
                throw new InvalidResponseException();
            }
            if (response[0].ResultID.Failed())
            {
                throw new ResultIDException(response[0].ResultID);
            }
            return new SourceServer(server) { ItemName = response[0].ItemName, ItemPath = response[0].ItemPath, Version = response[0].Version };
        }

        public GeneralResponse ModifySourceServers(SourceServer[] servers)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            GeneralResponse response = ((Opc.Dx.IServer) base.m_server).ModifySourceServers(servers);
            if (response != null)
            {
                this.GetSourceServers();
                this.m_version = response.Version;
            }
            return response;
        }

        public DXConnection[] QueryDXConnections(string browsePath, DXConnection[] connectionMasks, bool recursive, out ResultID[] errors)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Dx.IServer) base.m_server).QueryDXConnections(browsePath, connectionMasks, recursive, out errors);
        }

        public string ResetConfiguration(string configurationVersion)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            this.m_version = ((Opc.Dx.IServer) base.m_server).ResetConfiguration((configurationVersion == null) ? this.m_version : configurationVersion);
            return this.m_version;
        }

        public GeneralResponse UpdateDXConnections(string browsePath, DXConnection[] connectionMasks, bool recursive, DXConnection connectionDefinition, out ResultID[] errors)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            GeneralResponse response = ((Opc.Dx.IServer) base.m_server).UpdateDXConnections(browsePath, connectionMasks, recursive, connectionDefinition, out errors);
            if (response != null)
            {
                this.m_version = response.Version;
            }
            return response;
        }

        public DXConnectionQueryCollection Queries
        {
            get
            {
                return this.m_connectionQueries;
            }
        }

        public SourceServerCollection SourceServers
        {
            get
            {
                return this.m_sourceServers;
            }
        }

        public string Version
        {
            get
            {
                return this.m_version;
            }
        }

        private class Names
        {
            internal const string QUERIES = "Queries";
        }
    }
}

