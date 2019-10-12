namespace Jund.OpcHelper.OpcCom
{
    using Opc;
    using OpcCom.Ae;
    using OpcCom.Da;
    using OpcCom.Da20;
    using OpcCom.Dx;
    using OpcCom.Hda;
    using OpcRcw.Ae;
    using OpcRcw.Da;
    using OpcRcw.Dx;
    using OpcRcw.Hda;
    using System;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Text;

    [Serializable]
    public class Factory : Opc.Factory
    {
        public Factory() : base(null, false)
        {
        }

        public Factory(bool useRemoting) : base(null, useRemoting)
        {
        }

        protected Factory(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public static object Connect(URL url, ConnectData connectData)
        {
            Guid guid;
            NetworkCredential credential;
            object obj2;
            string path = url.Path;
            string g = null;
            int index = url.Path.IndexOf('/');
            if (index >= 0)
            {
                path = url.Path.Substring(0, index);
                g = url.Path.Substring(index + 1);
            }
            if (g == null)
            {
                guid = new ServerEnumerator().CLSIDFromProgID(path, url.HostName, connectData);
                if (!(guid == Guid.Empty))
                {
                    goto Label_007F;
                }
                try
                {
                    guid = new Guid(path);
                    goto Label_007F;
                }
                catch
                {
                    throw new ConnectFailedException(path);
                }
            }
            try
            {
                guid = new Guid(g);
            }
            catch
            {
                throw new ConnectFailedException(g);
            }
        Label_007F:
            credential = (connectData != null) ? connectData.GetCredential(null, null) : null;
            if ((connectData == null) || (connectData.LicenseKey == null))
            {
                try
                {
                    return OpcCom.Interop.CreateInstance(guid, url.HostName, credential);
                }
                catch (Exception exception)
                {
                    throw new ConnectFailedException(exception);
                }
            }
            try
            {
                obj2 = OpcCom.Interop.CreateInstanceWithLicenseKey(guid, url.HostName, credential, connectData.LicenseKey);
            }
            catch (Exception exception2)
            {
                throw new ConnectFailedException(exception2);
            }
            return obj2;
        }

        public override IServer CreateInstance(URL url, ConnectData connectData)
        {
            object o = Connect(url, connectData);
            if (o == null)
            {
                return null;
            }
            OpcCom.Server server = null;
            System.Type type = null;
            try
            {
                if (url.Scheme == "opcda")
                {
                    if (!typeof(IOPCServer).IsInstanceOfType(o))
                    {
                        type = typeof(IOPCServer);
                        throw new NotSupportedException();
                    }
                    if (!typeof(IOPCBrowse).IsInstanceOfType(o) || !typeof(IOPCItemIO).IsInstanceOfType(o))
                    {
                        if (!typeof(IOPCItemProperties).IsInstanceOfType(o))
                        {
                            type = typeof(IOPCItemProperties);
                            throw new NotSupportedException();
                        }
                        server = new OpcCom.Da20.Server(url, o);
                    }
                    else
                    {
                        server = new OpcCom.Da.Server(url, o);
                    }
                }
                else if (url.Scheme == "opcae")
                {
                    if (!typeof(IOPCEventServer).IsInstanceOfType(o))
                    {
                        type = typeof(IOPCEventServer);
                        throw new NotSupportedException();
                    }
                    server = new OpcCom.Ae.Server(url, o);
                }
                else if (url.Scheme == "opchda")
                {
                    if (!typeof(IOPCHDA_Server).IsInstanceOfType(o))
                    {
                        type = typeof(IOPCHDA_Server);
                        throw new NotSupportedException();
                    }
                    server = new OpcCom.Hda.Server(url, o);
                }
                else
                {
                    if (!(url.Scheme == "opcdx"))
                    {
                        throw new NotSupportedException(string.Format("The URL scheme '{0}' is not supported.", url.Scheme));
                    }
                    if (!typeof(IOPCConfiguration).IsInstanceOfType(o))
                    {
                        type = typeof(IOPCConfiguration);
                        throw new NotSupportedException();
                    }
                    server = new OpcCom.Dx.Server(url, o);
                }
            }
            catch (NotSupportedException exception)
            {
                OpcCom.Interop.ReleaseServer(server);
                server = null;
                if (type != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendFormat("The COM server does not support the interface ", new object[0]);
                    builder.AppendFormat("'{0}'.", type.FullName);
                    builder.Append("\r\n\r\nThis problem could be caused by:\r\n");
                    builder.Append("- incorrectly installed proxy/stubs.\r\n");
                    builder.Append("- problems with the DCOM security settings.\r\n");
                    builder.Append("- a personal firewall (sometimes activated by default).\r\n");
                    throw new NotSupportedException(builder.ToString());
                }
                throw exception;
            }
            catch (Exception exception2)
            {
                OpcCom.Interop.ReleaseServer(server);
                server = null;
                throw exception2;
            }
            if (server != null)
            {
                server.Initialize(url, connectData);
            }
            return server;
        }
    }
}

