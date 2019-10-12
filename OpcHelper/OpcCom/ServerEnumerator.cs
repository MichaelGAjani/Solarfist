namespace Jund.OpcHelper.OpcCom
{
    using Opc;
    using Opc.Ae;
    using Opc.Da;
    using Opc.Dx;
    using Opc.Hda;
    using OpcRcw.Comn;
    using System;
    using System.Collections;
    using System.Net;

    public class ServerEnumerator : IDiscovery, IDisposable
    {
        private static readonly Guid CLSID = new Guid("13486D51-4821-11D2-A494-3CB306C10000");
        private string m_host = null;
        private IOPCServerList2 m_server = null;
        private const string ProgID = "OPC.ServerList.1";

        public Guid CLSIDFromProgID(string progID, string host, ConnectData connectData)
        {
            lock (this)
            {
                Guid empty;
                NetworkCredential credential = (connectData != null) ? connectData.GetCredential(null, null) : null;
                this.m_server = (IOPCServerList2) Interop.CreateInstance(CLSID, host, credential);
                this.m_host = host;
                try
                {
                    this.m_server.CLSIDFromProgID(progID, out empty);
                }
                catch
                {
                    empty = Guid.Empty;
                }
                finally
                {
                    Interop.ReleaseServer(this.m_server);
                    this.m_server = null;
                }
                return empty;
            }
        }

        private URL CreateUrl(Specification specification, Guid clsid)
        {
            URL url = new URL {
                HostName = this.m_host,
                Port = 0,
                Path = null
            };
            if (specification == Specification.COM_DA_30)
            {
                url.Scheme = "opcda";
            }
            else if (specification == Specification.COM_DA_20)
            {
                url.Scheme = "opcda";
            }
            else if (specification == Specification.COM_DA_10)
            {
                url.Scheme = "opcda";
            }
            else if (specification == Specification.COM_DX_10)
            {
                url.Scheme = "opcdx";
            }
            else if (specification == Specification.COM_AE_10)
            {
                url.Scheme = "opcae";
            }
            else if (specification == Specification.COM_HDA_10)
            {
                url.Scheme = "opchda";
            }
            else if (specification == Specification.COM_BATCH_10)
            {
                url.Scheme = "opcbatch";
            }
            else if (specification == Specification.COM_BATCH_20)
            {
                url.Scheme = "opcbatch";
            }
            try
            {
                string ppszProgID = null;
                string ppszVerIndProgID = null;
                string ppszUserType = null;
                this.m_server.GetClassDetails(ref clsid, out ppszProgID, out ppszUserType, out ppszVerIndProgID);
                if (ppszVerIndProgID != null)
                {
                    url.Path = string.Format("{0}/{1}", ppszVerIndProgID, "{" + clsid.ToString() + "}");
                    return url;
                }
                if (ppszProgID != null)
                {
                    url.Path = string.Format("{0}/{1}", ppszProgID, "{" + clsid.ToString() + "}");
                }
                return url;
            }
            catch (Exception)
            {
            }
            finally
            {
                if (url.Path == null)
                {
                    url.Path = string.Format("{0}", "{" + clsid.ToString() + "}");
                }
            }
            return url;
        }

        public void Dispose()
        {
        }

        public string[] EnumerateHosts()
        {
            return Interop.EnumComputers();
        }

        public Opc.Server[] GetAvailableServers(Specification specification)
        {
            return this.GetAvailableServers(specification, null, null);
        }

        public Opc.Server[] GetAvailableServers(Specification specification, string host, ConnectData connectData)
        {
            Opc.Server[] serverArray;
            lock (this)
            {
                NetworkCredential credential = (connectData != null) ? connectData.GetCredential(null, null) : null;
                this.m_server = (IOPCServerList2) Interop.CreateInstance(CLSID, host, credential);
                this.m_host = host;
                try
                {
                    ArrayList list = new ArrayList();
                    Guid guid = new Guid(specification.ID);
                    IOPCEnumGUID ppenumClsid = null;
                    Guid[] rgcatidImpl = new Guid[] { guid };
                    this.m_server.EnumClassesOfCategories(1, rgcatidImpl, 0, null, out ppenumClsid);
                    Guid[] guidArray = this.ReadClasses(ppenumClsid);
                    Interop.ReleaseServer(ppenumClsid);
                    ppenumClsid = null;
                    foreach (Guid guid2 in guidArray)
                    {
                        OpcCom.Factory factory = new OpcCom.Factory();
                        try
                        {
                            URL url = this.CreateUrl(specification, guid2);
                            Opc.Server server = null;
                            if (specification == Specification.COM_DA_30)
                            {
                                server = new Opc.Da.Server(factory, url);
                            }
                            else if (specification == Specification.COM_DA_20)
                            {
                                server = new Opc.Da.Server(factory, url);
                            }
                            else if (specification == Specification.COM_AE_10)
                            {
                                server = new Opc.Ae.Server(factory, url);
                            }
                            else if (specification == Specification.COM_HDA_10)
                            {
                                server = new Opc.Hda.Server(factory, url);
                            }
                            else if (specification == Specification.COM_DX_10)
                            {
                                server = new Opc.Dx.Server(factory, url);
                            }
                            list.Add(server);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    serverArray = (Opc.Server[]) list.ToArray(typeof(Opc.Server));
                }
                finally
                {
                    Interop.ReleaseServer(this.m_server);
                    this.m_server = null;
                }
            }
            return serverArray;
        }

        private Guid[] ReadClasses(IOPCEnumGUID enumerator)
        {
            ArrayList list = new ArrayList();
            int pceltFetched = 0;
            Guid[] rgelt = new Guid[10];
            do
            {
                try
                {
                    enumerator.Next(rgelt.Length, rgelt, out pceltFetched);
                    for (int i = 0; i < pceltFetched; i++)
                    {
                        list.Add(rgelt[i]);
                    }
                }
                catch
                {
                    break;
                }
            }
            while (pceltFetched > 0);
            return (Guid[]) list.ToArray(typeof(Guid));
        }
    }
}

