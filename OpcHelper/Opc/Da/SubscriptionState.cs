namespace Jund.OpcHelper.Opc.Da
{
    using System;

    [Serializable]
    public class SubscriptionState : ICloneable
    {
        private bool m_active = true;
        private object m_clientHandle = null;
        private float m_deadband = 0f;
        private int m_keepAlive = 0;
        private string m_locale = null;
        private string m_name = null;
        private object m_serverHandle = null;
        private int m_updateRate = 0;

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

        public float Deadband
        {
            get
            {
                return this.m_deadband;
            }
            set
            {
                this.m_deadband = value;
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

        public string Locale
        {
            get
            {
                return this.m_locale;
            }
            set
            {
                this.m_locale = value;
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

        public object ServerHandle
        {
            get
            {
                return this.m_serverHandle;
            }
            set
            {
                this.m_serverHandle = value;
            }
        }

        public int UpdateRate
        {
            get
            {
                return this.m_updateRate;
            }
            set
            {
                this.m_updateRate = value;
            }
        }
    }
}

