namespace Jund.OpcHelper.OpcCom.Da.Wrapper
{
    using OpcRcw.Comn;
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;

    [CLSCompliant(false)]
    public class ConnectionPoint : IConnectionPoint
    {
        private object m_callback = null;
        private ConnectionPointContainer m_container = null;
        private int m_cookie = 0;
        private Guid m_interface = Guid.Empty;

        public ConnectionPoint(Guid iid, ConnectionPointContainer container)
        {
            this.m_interface = iid;
            this.m_container = container;
        }

        public void Advise(object pUnkSink, out int pdwCookie)
        {
            ConnectionPoint point;
            Monitor.Enter(point = this);
            try
            {
                if (pUnkSink == null)
                {
                    throw new ExternalException("E_POINTER", -2147467261);
                }
                pdwCookie = 0;
                if (this.m_callback != null)
                {
                    throw new ExternalException("CONNECT_E_ADVISELIMIT", -2147220991);
                }
                this.m_callback = pUnkSink;
                pdwCookie = ++this.m_cookie;
                this.m_container.OnAdvise(this.m_interface);
            }
            catch (Exception exception)
            {
                throw Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(point);
            }
        }

        public void EnumConnections(out IEnumConnections ppenum)
        {
            throw new ExternalException("E_NOTIMPL", -2147467263);
        }

        public void GetConnectionInterface(out Guid pIID)
        {
            ConnectionPoint point;
            Monitor.Enter(point = this);
            try
            {
                pIID = this.m_interface;
            }
            catch (Exception exception)
            {
                throw Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(point);
            }
        }

        public void GetConnectionPointContainer(out IConnectionPointContainer ppCPC)
        {
            ConnectionPoint point;
            Monitor.Enter(point = this);
            try
            {
                ppCPC = this.m_container;
            }
            catch (Exception exception)
            {
                throw Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(point);
            }
        }

        public void Unadvise(int dwCookie)
        {
            ConnectionPoint point;
            Monitor.Enter(point = this);
            try
            {
                if ((this.m_cookie != dwCookie) || (this.m_callback == null))
                {
                    throw new ExternalException("CONNECT_E_NOCONNECTION", -2147220992);
                }
                this.m_callback = null;
                this.m_container.OnUnadvise(this.m_interface);
            }
            catch (Exception exception)
            {
                throw Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(point);
            }
        }

        public object Callback
        {
            get
            {
                return this.m_callback;
            }
        }

        public bool IsConnected
        {
            get
            {
                return (this.m_callback != null);
            }
        }
    }
}

