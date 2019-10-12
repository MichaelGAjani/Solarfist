namespace Jund.OpcHelper.OpcCom
{
    using OpcRcw.Comn;
    using System;

    public class ConnectionPoint : IDisposable
    {
        private int m_cookie = 0;
        private int m_refs = 0;
        private IConnectionPoint m_server = null;

        public ConnectionPoint(object server, Guid iid)
        {
            ((IConnectionPointContainer) server).FindConnectionPoint(ref iid, out this.m_server);
        }

        public int Advise(object callback)
        {
            if (this.m_refs++ == 0)
            {
                this.m_server.Advise(callback, out this.m_cookie);
            }
            return this.m_refs;
        }

        public void Dispose()
        {
            if (this.m_server != null)
            {
                while (this.Unadvise() > 0)
                {
                }
                Interop.ReleaseServer(this.m_server);
                this.m_server = null;
            }
        }

        public int Unadvise()
        {
            if (--this.m_refs == 0)
            {
                this.m_server.Unadvise(this.m_cookie);
            }
            return this.m_refs;
        }
    }
}

