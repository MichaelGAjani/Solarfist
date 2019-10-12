namespace Jund.OpcHelper.Opc.Ae
{
    using Opc;
    using Opc.Da;
    using System;
    using System.Reflection;

    [Serializable]
    public class Condition : ICloneable
    {
        private string m_acknowledgerID = null;
        private SubCondition m_activeSubcondition = new SubCondition();
        private AttributeValueCollection m_attributes = new AttributeValueCollection();
        private string m_comment = null;
        private DateTime m_condLastActive = DateTime.MinValue;
        private DateTime m_condLastInactive = DateTime.MinValue;
        private DateTime m_lastAckTime = DateTime.MinValue;
        private Opc.Da.Quality m_quality = Opc.Da.Quality.Bad;
        private int m_state = 0;
        private SubConditionCollection m_subconditions = new SubConditionCollection();
        private DateTime m_subCondLastActive = DateTime.MinValue;

        public virtual object Clone()
        {
            Condition condition = (Condition) base.MemberwiseClone();
            condition.m_activeSubcondition = (SubCondition) this.m_activeSubcondition.Clone();
            condition.m_subconditions = (SubConditionCollection) this.m_subconditions.Clone();
            condition.m_attributes = (AttributeValueCollection) this.m_attributes.Clone();
            return condition;
        }

        public string AcknowledgerID
        {
            get
            {
                return this.m_acknowledgerID;
            }
            set
            {
                this.m_acknowledgerID = value;
            }
        }

        public SubCondition ActiveSubCondition
        {
            get
            {
                return this.m_activeSubcondition;
            }
            set
            {
                this.m_activeSubcondition = value;
            }
        }

        public AttributeValueCollection Attributes
        {
            get
            {
                return this.m_attributes;
            }
        }

        public string Comment
        {
            get
            {
                return this.m_comment;
            }
            set
            {
                this.m_comment = value;
            }
        }

        public DateTime CondLastActive
        {
            get
            {
                return this.m_condLastActive;
            }
            set
            {
                this.m_condLastActive = value;
            }
        }

        public DateTime CondLastInactive
        {
            get
            {
                return this.m_condLastInactive;
            }
            set
            {
                this.m_condLastInactive = value;
            }
        }

        public DateTime LastAckTime
        {
            get
            {
                return this.m_lastAckTime;
            }
            set
            {
                this.m_lastAckTime = value;
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

        public int State
        {
            get
            {
                return this.m_state;
            }
            set
            {
                this.m_state = value;
            }
        }

        public SubConditionCollection SubConditions
        {
            get
            {
                return this.m_subconditions;
            }
        }

        public DateTime SubCondLastActive
        {
            get
            {
                return this.m_subCondLastActive;
            }
            set
            {
                this.m_subCondLastActive = value;
            }
        }

        public class AttributeValueCollection : WriteableCollection
        {
            internal AttributeValueCollection() : base(null, typeof(AttributeValue))
            {
            }

            public AttributeValue[] ToArray()
            {
                return (AttributeValue[]) this.Array.ToArray();
            }

            public AttributeValue this[int index]
            {
                get
                {
                    return (AttributeValue) this.Array[index];
                }
            }
        }

        public class SubConditionCollection : WriteableCollection
        {
            internal SubConditionCollection() : base(null, typeof(SubCondition))
            {
            }

            public SubCondition[] ToArray()
            {
                return (SubCondition[]) this.Array.ToArray();
            }

            public SubCondition this[int index]
            {
                get
                {
                    return (SubCondition) this.Array[index];
                }
            }
        }
    }
}

