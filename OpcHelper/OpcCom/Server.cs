namespace Jund.OpcHelper.OpcCom
{
    using Opc;
    using OpcRcw.Comn;
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class Server : IServer, IDisposable
    {
        private Callback m_callback;
        private ConnectionPoint m_connection;
        protected object m_server;
        protected URL m_url;

        public event ServerShutdownEventHandler ServerShutdown
        {
            add
            {
                OpcCom.Server server;
                Monitor.Enter(server = this);
                try
                {
                    this.Advise();
                    this.m_callback.ServerShutdown += value;
                }
                catch
                {
                }
                finally
                {
                    Monitor.Exit(server);
                }
            }
            remove
            {
                lock (this)
                {
                    this.m_callback.ServerShutdown -= value;
                    this.Unadvise();
                }
            }
        }

        internal Server()
        {
            this.m_server = null;
            this.m_url = null;
            this.m_connection = null;
            this.m_callback = null;
            this.m_url = null;
            this.m_server = null;
            this.m_callback = new Callback(this);
        }

        internal Server(URL url, object server)
        {
            this.m_server = null;
            this.m_url = null;
            this.m_connection = null;
            this.m_callback = null;
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            this.m_url = (URL) url.Clone();
            this.m_server = server;
            this.m_callback = new Callback(this);
        }

        private void Advise()
        {
            if (this.m_connection == null)
            {
                this.m_connection = new ConnectionPoint(this.m_server, typeof(IOPCShutdown).GUID);
                this.m_connection.Advise(this.m_callback);
            }
        }

        public virtual void Dispose()
        {
            lock (this)
            {
                if (this.m_connection != null)
                {
                    this.m_connection.Dispose();
                    this.m_connection = null;
                }
                Interop.ReleaseServer(this.m_server);
                this.m_server = null;
            }
        }

        public virtual string GetErrorText(string locale, ResultID resultID)
        {
            string str3;
            lock (this)
            {
                if (this.m_server == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    string str = this.GetLocale();
                    if (str != locale)
                    {
                        this.SetLocale(locale);
                    }
                    string ppString = null;
                    ((IOPCCommon) this.m_server).GetErrorString(resultID.Code, out ppString);
                    if (str != locale)
                    {
                        this.SetLocale(str);
                    }
                    str3 = ppString;
                }
                catch (Exception exception)
                {
                    throw Interop.CreateException("IOPCServer.GetErrorString", exception);
                }
            }
            return str3;
        }

        public virtual string GetLocale()
        {
            string locale;
            lock (this)
            {
                if (this.m_server == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    int pdwLcid = 0;
                    ((IOPCCommon) this.m_server).GetLocaleID(out pdwLcid);
                    locale = Interop.GetLocale(pdwLcid);
                }
                catch (Exception exception)
                {
                    throw Interop.CreateException("IOPCCommon.GetLocaleID", exception);
                }
            }
            return locale;
        }

        public virtual string[] GetSupportedLocales()
        {
            string[] strArray;
            lock (this)
            {
                if (this.m_server == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    int pdwCount = 0;
                    IntPtr zero = IntPtr.Zero;
                    ((IOPCCommon) this.m_server).QueryAvailableLocaleIDs(out pdwCount, out zero);
                    int[] numArray = Interop.GetInt32s(ref zero, pdwCount, true);
                    if (numArray != null)
                    {
                        ArrayList list = new ArrayList();
                        foreach (int num2 in numArray)
                        {
                            try
                            {
                                list.Add(Interop.GetLocale(num2));
                            }
                            catch
                            {
                            }
                        }
                        return (string[]) list.ToArray(typeof(string));
                    }
                    strArray = null;
                }
                catch (Exception exception)
                {
                    throw Interop.CreateException("IOPCCommon.QueryAvailableLocaleIDs", exception);
                }
            }
            return strArray;
        }

        public virtual void Initialize(URL url, ConnectData connectData)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            lock (this)
            {
                if ((this.m_url == null) || !this.m_url.Equals(url))
                {
                    if (this.m_server != null)
                    {
                        this.Uninitialize();
                    }
                    this.m_server = (IOPCCommon) OpcCom.Factory.Connect(url, connectData);
                }
                this.m_url = (URL) url.Clone();
            }
        }

        public virtual string SetLocale(string locale)
        {
            lock (this)
            {
                if (this.m_server == null)
                {
                    throw new NotConnectedException();
                }
                int dwLcid = Interop.GetLocale(locale);
                try
                {
                    ((IOPCCommon) this.m_server).SetLocaleID(dwLcid);
                }
                catch (Exception exception)
                {
                    if (dwLcid != 0)
                    {
                        throw Interop.CreateException("IOPCCommon.SetLocaleID", exception);
                    }
                    try
                    {
                        ((IOPCCommon) this.m_server).SetLocaleID(0x800);
                    }
                    catch
                    {
                    }
                }
                return this.GetLocale();
            }
        }

        private void Unadvise()
        {
            if ((this.m_connection != null) && (this.m_connection.Unadvise() == 0))
            {
                this.m_connection.Dispose();
                this.m_connection = null;
            }
        }

        public virtual void Uninitialize()
        {
            lock (this)
            {
                this.Dispose();
            }
        }

        private class Callback : IOPCShutdown
        {
            private OpcCom.Server m_server = null;

            private event ServerShutdownEventHandler m_serverShutdown;

            public event ServerShutdownEventHandler ServerShutdown
            {
                add
                {
                    lock (this)
                    {
                        this.m_serverShutdown = (ServerShutdownEventHandler) Delegate.Combine(this.m_serverShutdown, value);
                    }
                }
                remove
                {
                    lock (this)
                    {
                        this.m_serverShutdown = (ServerShutdownEventHandler) Delegate.Remove(this.m_serverShutdown, value);
                    }
                }
            }

            public Callback(OpcCom.Server server)
            {
                this.m_server = server;
            }

            public void ShutdownRequest(string reason)
            {
                try
                {
                    lock (this)
                    {
                        if (this.m_serverShutdown != null)
                        {
                            this.m_serverShutdown(reason);
                        }
                    }
                }
                catch (Exception exception)
                {
                    string stackTrace = exception.StackTrace;
                }
            }
        }
    }
}

