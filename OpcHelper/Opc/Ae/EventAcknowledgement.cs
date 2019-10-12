namespace Jund.OpcHelper.Opc.Ae
{
    using System;

    [Serializable]
    public class EventAcknowledgement : ICloneable
    {
        private DateTime m_activeTime;
        private string m_conditionName;
        private int m_cookie;
        private string m_sourceName;

        public EventAcknowledgement()
        {
            this.m_sourceName = null;
            this.m_conditionName = null;
            this.m_activeTime = DateTime.MinValue;
            this.m_cookie = 0;
        }

        public EventAcknowledgement(EventNotification notification)
        {
            this.m_sourceName = null;
            this.m_conditionName = null;
            this.m_activeTime = DateTime.MinValue;
            this.m_cookie = 0;
            this.m_sourceName = notification.SourceID;
            this.m_conditionName = notification.ConditionName;
            this.m_activeTime = notification.ActiveTime;
            this.m_cookie = notification.Cookie;
        }

        public virtual object Clone()
        {
            return base.MemberwiseClone();
        }

        public DateTime ActiveTime
        {
            get
            {
                return this.m_activeTime;
            }
            set
            {
                this.m_activeTime = value;
            }
        }

        public string ConditionName
        {
            get
            {
                return this.m_conditionName;
            }
            set
            {
                this.m_conditionName = value;
            }
        }

        public int Cookie
        {
            get
            {
                return this.m_cookie;
            }
            set
            {
                this.m_cookie = value;
            }
        }

        public string SourceName
        {
            get
            {
                return this.m_sourceName;
            }
            set
            {
                this.m_sourceName = value;
            }
        }
    }
}

