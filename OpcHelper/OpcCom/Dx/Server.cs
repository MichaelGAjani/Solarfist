namespace Jund.OpcHelper.OpcCom.Dx
{
    using Opc;
    using Opc.Da;
    using Opc.Dx;
    using OpcCom;
    using OpcCom.Da;
    using OpcRcw.Dx;
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    public class Server : OpcCom.Da.Server, Opc.Dx.IServer, Opc.Da.IServer, Opc.IServer, IDisposable
    {
        internal Server(URL url, object server) : base(url, server)
        {
        }

        public GeneralResponse AddDXConnections(Opc.Dx.DXConnection[] connections)
        {
            GeneralResponse generalResponse;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    DXGeneralResponse response;
                    OpcRcw.Dx.DXConnection[] dXConnections = OpcCom.Dx.Interop.GetDXConnections(connections);
                    if (dXConnections == null)
                    {
                        dXConnections = new OpcRcw.Dx.DXConnection[0];
                    }
                    ((IOPCConfiguration) base.m_server).AddDXConnections(dXConnections.Length, dXConnections, out response);
                    generalResponse = OpcCom.Dx.Interop.GetGeneralResponse(response, true);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.AddDXConnections", exception);
                }
            }
            return generalResponse;
        }

        public GeneralResponse AddSourceServers(Opc.Dx.SourceServer[] servers)
        {
            GeneralResponse generalResponse;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    DXGeneralResponse response;
                    OpcRcw.Dx.SourceServer[] sourceServers = OpcCom.Dx.Interop.GetSourceServers(servers);
                    ((IOPCConfiguration) base.m_server).AddServers(sourceServers.Length, sourceServers, out response);
                    generalResponse = OpcCom.Dx.Interop.GetGeneralResponse(response, true);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.AddServers", exception);
                }
            }
            return generalResponse;
        }

        public GeneralResponse CopyDefaultSourceServerAttributes(bool configToStatus, Opc.Dx.ItemIdentifier[] servers)
        {
            GeneralResponse generalResponse;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    DXGeneralResponse response;
                    OpcRcw.Dx.ItemIdentifier[] itemIdentifiers = OpcCom.Dx.Interop.GetItemIdentifiers(servers);
                    ((IOPCConfiguration) base.m_server).CopyDefaultServerAttributes(configToStatus ? 1 : 0, itemIdentifiers.Length, itemIdentifiers, out response);
                    generalResponse = OpcCom.Dx.Interop.GetGeneralResponse(response, true);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.CopyDefaultServerAttributes", exception);
                }
            }
            return generalResponse;
        }

        public GeneralResponse CopyDXConnectionDefaultAttributes(bool configToStatus, string browsePath, Opc.Dx.DXConnection[] connectionMasks, bool recursive, out ResultID[] errors)
        {
            GeneralResponse generalResponse;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    DXGeneralResponse response;
                    OpcRcw.Dx.DXConnection[] dXConnections = OpcCom.Dx.Interop.GetDXConnections(connectionMasks);
                    if (dXConnections == null)
                    {
                        dXConnections = new OpcRcw.Dx.DXConnection[0];
                    }
                    IntPtr zero = IntPtr.Zero;
                    ((IOPCConfiguration) base.m_server).CopyDXConnectionDefaultAttributes(configToStatus ? 1 : 0, (browsePath != null) ? browsePath : "", dXConnections.Length, dXConnections, recursive ? 1 : 0, out zero, out response);
                    errors = OpcCom.Dx.Interop.GetResultIDs(ref zero, dXConnections.Length, true);
                    generalResponse = OpcCom.Dx.Interop.GetGeneralResponse(response, true);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.CopyDXConnectionDefaultAttributes", exception);
                }
            }
            return generalResponse;
        }

        public GeneralResponse DeleteDXConnections(string browsePath, Opc.Dx.DXConnection[] connectionMasks, bool recursive, out ResultID[] errors)
        {
            GeneralResponse generalResponse;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    DXGeneralResponse response;
                    OpcRcw.Dx.DXConnection[] dXConnections = OpcCom.Dx.Interop.GetDXConnections(connectionMasks);
                    if (dXConnections == null)
                    {
                        dXConnections = new OpcRcw.Dx.DXConnection[0];
                    }
                    IntPtr zero = IntPtr.Zero;
                    ((IOPCConfiguration) base.m_server).DeleteDXConnections((browsePath != null) ? browsePath : "", dXConnections.Length, dXConnections, recursive ? 1 : 0, out zero, out response);
                    errors = OpcCom.Dx.Interop.GetResultIDs(ref zero, dXConnections.Length, true);
                    generalResponse = OpcCom.Dx.Interop.GetGeneralResponse(response, true);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.DeleteDXConnections", exception);
                }
            }
            return generalResponse;
        }

        public GeneralResponse DeleteSourceServers(Opc.Dx.ItemIdentifier[] servers)
        {
            GeneralResponse generalResponse;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    DXGeneralResponse response;
                    OpcRcw.Dx.ItemIdentifier[] itemIdentifiers = OpcCom.Dx.Interop.GetItemIdentifiers(servers);
                    ((IOPCConfiguration) base.m_server).DeleteServers(itemIdentifiers.Length, itemIdentifiers, out response);
                    generalResponse = OpcCom.Dx.Interop.GetGeneralResponse(response, true);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.DeleteServers", exception);
                }
            }
            return generalResponse;
        }

        public Opc.Dx.SourceServer[] GetSourceServers()
        {
            Opc.Dx.SourceServer[] serverArray;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    int pdwCount = 0;
                    IntPtr zero = IntPtr.Zero;
                    ((IOPCConfiguration) base.m_server).GetServers(out pdwCount, out zero);
                    serverArray = OpcCom.Dx.Interop.GetSourceServers(ref zero, pdwCount, true);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.GetServers", exception);
                }
            }
            return serverArray;
        }

        public GeneralResponse ModifyDXConnections(Opc.Dx.DXConnection[] connections)
        {
            GeneralResponse generalResponse;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    DXGeneralResponse response;
                    OpcRcw.Dx.DXConnection[] dXConnections = OpcCom.Dx.Interop.GetDXConnections(connections);
                    if (dXConnections == null)
                    {
                        dXConnections = new OpcRcw.Dx.DXConnection[0];
                    }
                    ((IOPCConfiguration) base.m_server).ModifyDXConnections(dXConnections.Length, dXConnections, out response);
                    generalResponse = OpcCom.Dx.Interop.GetGeneralResponse(response, true);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.ModifyDXConnections", exception);
                }
            }
            return generalResponse;
        }

        public GeneralResponse ModifySourceServers(Opc.Dx.SourceServer[] servers)
        {
            GeneralResponse generalResponse;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    DXGeneralResponse response;
                    OpcRcw.Dx.SourceServer[] sourceServers = OpcCom.Dx.Interop.GetSourceServers(servers);
                    ((IOPCConfiguration) base.m_server).ModifyServers(sourceServers.Length, sourceServers, out response);
                    generalResponse = OpcCom.Dx.Interop.GetGeneralResponse(response, true);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.ModifyServers", exception);
                }
            }
            return generalResponse;
        }

        public Opc.Dx.DXConnection[] QueryDXConnections(string browsePath, Opc.Dx.DXConnection[] connectionMasks, bool recursive, out ResultID[] errors)
        {
            Opc.Dx.DXConnection[] connectionArray2;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    OpcRcw.Dx.DXConnection[] dXConnections = OpcCom.Dx.Interop.GetDXConnections(connectionMasks);
                    if (dXConnections == null)
                    {
                        dXConnections = new OpcRcw.Dx.DXConnection[0];
                    }
                    int pdwCount = 0;
                    IntPtr zero = IntPtr.Zero;
                    IntPtr ppConnections = IntPtr.Zero;
                    ((IOPCConfiguration) base.m_server).QueryDXConnections((browsePath != null) ? browsePath : "", dXConnections.Length, dXConnections, recursive ? 1 : 0, out zero, out pdwCount, out ppConnections);
                    errors = OpcCom.Dx.Interop.GetResultIDs(ref zero, dXConnections.Length, true);
                    connectionArray2 = OpcCom.Dx.Interop.GetDXConnections(ref ppConnections, pdwCount, true);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.QueryDXConnections", exception);
                }
            }
            return connectionArray2;
        }

        public string ResetConfiguration(string configurationVersion)
        {
            string str2;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    string pszConfigurationVersion = null;
                    ((IOPCConfiguration) base.m_server).ResetConfiguration(configurationVersion, out pszConfigurationVersion);
                    str2 = pszConfigurationVersion;
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.ResetConfiguration", exception);
                }
            }
            return str2;
        }

        public GeneralResponse UpdateDXConnections(string browsePath, Opc.Dx.DXConnection[] connectionMasks, bool recursive, Opc.Dx.DXConnection connectionDefinition, out ResultID[] errors)
        {
            GeneralResponse generalResponse;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    DXGeneralResponse response;
                    OpcRcw.Dx.DXConnection[] dXConnections = OpcCom.Dx.Interop.GetDXConnections(connectionMasks);
                    if (dXConnections == null)
                    {
                        dXConnections = new OpcRcw.Dx.DXConnection[0];
                    }
                    OpcRcw.Dx.DXConnection dXConnection = OpcCom.Dx.Interop.GetDXConnection(connectionDefinition);
                    IntPtr zero = IntPtr.Zero;
                    ((IOPCConfiguration) base.m_server).UpdateDXConnections((browsePath != null) ? browsePath : "", dXConnections.Length, dXConnections, recursive ? 1 : 0, ref dXConnection, out zero, out response);
                    errors = OpcCom.Dx.Interop.GetResultIDs(ref zero, dXConnections.Length, true);
                    generalResponse = OpcCom.Dx.Interop.GetGeneralResponse(response, true);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCConfiguration.UpdateDXConnections", exception);
                }
            }
            return generalResponse;
        }
    }
}

