namespace Jund.OpcHelper.Opc.Ae
{
    using Opc;
    using Opc.Da;
    using System;

    [Serializable]
    public class EventNotification : ICloneable
    {
        private bool m_ackRequired = false;
        private DateTime m_activeTime = DateTime.MinValue;
        private string m_actorID = null;
        private AttributeCollection m_attributes = new AttributeCollection();
        private int m_changeMask = 0;
        private object m_clientHandle = null;
        private string m_conditionName = null;
        private int m_cookie = 0;
        private int m_eventCategory = 0;
        private Opc.Ae.EventType m_eventType = Opc.Ae.EventType.Condition;
        private string m_message = null;
        private int m_newState = 0;
        private Opc.Da.Quality m_quality = Opc.Da.Quality.Bad;
        private int m_severity = 1;
        private string m_sourceID = null;
        private string m_subConditionName = null;
        private DateTime m_time = DateTime.MinValue;

        public virtual object Clone()
        {
            EventNotification notification = (EventNotification) base.MemberwiseClone();
            notification.m_attributes = (AttributeCollection) this.m_attributes.Clone();
            return notification;
        }

        public void SetAttributes(object[] attributes)
        {
            if (attributes == null)
            {
                this.m_attributes = new AttributeCollection();
            }
            else
            {
                this.m_attributes = new AttributeCollection(attributes);
            }
        }

        public bool AckRequired
        {
            get
            {
                return this.m_ackRequired;
            }
            set
            {
                this.m_ackRequired = value;
            }
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

        public string ActorID
        {
            get
            {
                return this.m_actorID;
            }
            set
            {
                this.m_actorID = value;
            }
        }

        public AttributeCollection Attributes
        {
            get
            {
                return this.m_attributes;
            }
        }

        public int ChangeMask
        {
            get
            {
                return this.m_changeMask;
            }
            set
            {
                this.m_changeMask = value;
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

        public int EventCategory
        {
            get
            {
                return this.m_eventCategory;
            }
            set
            {
                this.m_eventCategory = value;
            }
        }

        public Opc.Ae.EventType EventType
        {
            get
            {
                return this.m_eventType;
            }
            set
            {
                this.m_eventType = value;
            }
        }

        public string Message
        {
            get
            {
                return this.m_message;
            }
            set
            {
                this.m_message = value;
            }
        }

        public int NewState
        {
            get
            {
                return this.m_newState;
            }
            set
            {
                this.m_newState = value;
            }
        }

        public Opc.Da.Quality Quality
        {
            get
            {
                return this.m_quality;
            }
            set
            {
                this.m_quality = value;
            }
        }

        public int Severity
        {
            get
            {
                return this.m_severity;
            }
            set
            {
                this.m_severity = value;
            }
        }

        public string SourceID
        {
            get
            {
                return this.m_sourceID;
            }
            set
            {
                this.m_sourceID = value;
            }
        }

        public string SubConditionName
        {
            get
            {
                return this.m_subConditionName;
            }
            set
            {
                this.m_subConditionName = value;
            }
        }

        public DateTime Time
        {
            get
            {
                return this.m_time;
            }
            set
            {
                this.m_time = value;
            }
        }

        [Serializable]
        public class AttributeCollection : ReadOnlyCollection
        {
            internal AttributeCollection() : base(new object[0])
            {
            }

            internal AttributeCollection(object[] attributes) : base(attributes)
            {
            }
        }
    }
}

