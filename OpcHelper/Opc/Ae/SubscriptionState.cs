namespace Jund.OpcHelper.Opc.Ae
{
    using System;

    [Serializable]
    public class SubscriptionState : ICloneable
    {
        private bool m_active = true;
        private int m_bufferTime = 0;
        private object m_clientHandle = null;
        private int m_keepAlive = 0;
        private int m_maxSize = 0;
        private string m_name = null;

        public virtual object Clone()
        {
            return base.MemberwiseClone();
        }

        public bool Active
        {
            get
            {
                return this.m_active;
            }
            set
            {
                this.m_active = value;
            }
        }

        public int BufferTime
        {
            get
            {
                return this.m_bufferTime;
            }
            set
            {
                this.m_bufferTime = value;
            }
        }

        public object ClientHandle
        {
            get
            {
                return this.m_clientHandle;
            }
            set
            {
                this.m_clientHandle = value;
            }
        }

        public int KeepAlive
        {
            get
            {
                return this.m_keepAlive;
            }
            set
            {
                this.m_keepAlive = value;
            }
        }

        public int MaxSize
        {
            get
            {
                return this.m_maxSize;
            }
            set
            {
                this.m_maxSize = value;
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
    }
}

