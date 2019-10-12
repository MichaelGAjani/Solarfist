namespace Jund.OpcHelper.OpcCom.Da.Wrapper
{
    using OpcRcw.Comn;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;
    using System.Threading;

    [CLSCompliant(false)]
    public class ConnectionPointContainer : IConnectionPointContainer
    {
        private Hashtable m_connectionPoints = new Hashtable();

        protected ConnectionPointContainer()
        {
        }

        public void EnumConnectionPoints(out IEnumConnectionPoints ppenum)
        {
            ConnectionPointContainer container;
            Monitor.Enter(container = this);
            try
            {
                ppenum = new OpcCom.Da.Wrapper.EnumConnectionPoints(this.m_connectionPoints.Values);
            }
            catch (Exception exception)
            {
                throw Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(container);
            }
        }

        public void FindConnectionPoint(ref Guid riid, out IConnectionPoint ppCP)
        {
            ConnectionPointContainer container;
            Monitor.Enter(container = this);
            try
            {
                ppCP = null;
                ConnectionPoint point = (ConnectionPoint) this.m_connectionPoints[(Guid) riid];
                if (point == null)
                {
                    throw new ExternalException("CONNECT_E_NOCONNECTION", -2147220992);
                }
                ppCP = point;
            }
            catch (Exception exception)
            {
                throw Server.CreateException(exception);
            }
            finally
            {
                Monitor.Exit(container);
            }
        }

        protected object GetCallback(Guid iid)
        {
            ConnectionPoint point = (ConnectionPoint) this.m_connectionPoints[iid];
            if (point != null)
            {
                return point.Callback;
            }
            return null;
        }

        protected bool IsConnected(Guid iid)
        {
            ConnectionPoint point = (ConnectionPoint) this.m_connectionPoints[iid];
            return ((point != null) && point.IsConnected);
        }

        public virtual void OnAdvise(Guid riid)
        {
        }

        public virtual void OnUnadvise(Guid riid)
        {
        }

        protected void RegisterInterface(Guid iid)
        {
            this.m_connectionPoints[iid] = new ConnectionPoint(iid, this);
        }

        protected void UnregisterInterface(Guid iid)
        {
            this.m_connectionPoints.Remove(iid);
        }
    }
}

