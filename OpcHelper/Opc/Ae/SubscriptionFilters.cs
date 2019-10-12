namespace Jund.OpcHelper.Opc.Ae
{
    using Opc;
    using System;
    using System.Reflection;
    using System.Runtime.Serialization;

    [Serializable]
    public class SubscriptionFilters : ICloneable, ISerializable
    {
        private StringCollection m_areas;
        private CategoryCollection m_categories;
        private int m_eventTypes;
        private int m_highSeverity;
        private int m_lowSeverity;
        private StringCollection m_sources;

        public SubscriptionFilters()
        {
            this.m_eventTypes = 0xffff;
            this.m_categories = new CategoryCollection();
            this.m_highSeverity = 0x3e8;
            this.m_lowSeverity = 1;
            this.m_areas = new StringCollection();
            this.m_sources = new StringCollection();
        }

        protected SubscriptionFilters(SerializationInfo info, StreamingContext context)
        {
            this.m_eventTypes = 0xffff;
            this.m_categories = new CategoryCollection();
            this.m_highSeverity = 0x3e8;
            this.m_lowSeverity = 1;
            this.m_areas = new StringCollection();
            this.m_sources = new StringCollection();
            this.m_eventTypes = (int) info.GetValue("ET", typeof(int));
            this.m_categories = (CategoryCollection) info.GetValue("CT", typeof(CategoryCollection));
            this.m_highSeverity = (int) info.GetValue("HS", typeof(int));
            this.m_lowSeverity = (int) info.GetValue("LS", typeof(int));
            this.m_areas = (StringCollection) info.GetValue("AR", typeof(StringCollection));
            this.m_sources = (StringCollection) info.GetValue("SR", typeof(StringCollection));
        }

        public virtual object Clone()
        {
            SubscriptionFilters filters = (SubscriptionFilters) base.MemberwiseClone();
            filters.m_categories = (CategoryCollection) this.m_categories.Clone();
            filters.m_areas = (StringCollection) this.m_areas.Clone();
            filters.m_sources = (StringCollection) this.m_sources.Clone();
            return filters;
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ET", this.m_eventTypes);
            info.AddValue("CT", this.m_categories);
            info.AddValue("HS", this.m_highSeverity);
            info.AddValue("LS", this.m_lowSeverity);
            info.AddValue("AR", this.m_areas);
            info.AddValue("SR", this.m_sources);
        }

        public StringCollection Areas
        {
            get
            {
                return this.m_areas;
            }
        }

        public CategoryCollection Categories
        {
            get
            {
                return this.m_categories;
            }
        }

        public int EventTypes
        {
            get
            {
                return this.m_eventTypes;
            }
            set
            {
                this.m_eventTypes = value;
            }
        }

        public int HighSeverity
        {
            get
            {
                return this.m_highSeverity;
            }
            set
            {
                this.m_highSeverity = value;
            }
        }

        public int LowSeverity
        {
            get
            {
                return this.m_lowSeverity;
            }
            set
            {
                this.m_lowSeverity = value;
            }
        }

        public StringCollection Sources
        {
            get
            {
                return this.m_sources;
            }
        }

        [Serializable]
        public class CategoryCollection : WriteableCollection
        {
            internal CategoryCollection() : base(null, typeof(int))
            {
            }

            protected CategoryCollection(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }

            public int[] ToArray()
            {
                return (int[]) this.Array.ToArray(typeof(int));
            }

            public int this[int index]
            {
                get
                {
                    return (int) this.Array[index];
                }
            }
        }

        private class Names
        {
            internal const string AREAS = "AR";
            internal const string CATEGORIES = "CT";
            internal const string EVENT_TYPES = "ET";
            internal const string HIGH_SEVERITY = "HS";
            internal const string LOW_SEVERITY = "LS";
            internal const string SOURCES = "SR";
        }

        [Serializable]
        public class StringCollection : WriteableCollection
        {
            internal StringCollection() : base(null, typeof(string))
            {
            }

            protected StringCollection(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }

            public string[] ToArray()
            {
                return (string[]) this.Array.ToArray(typeof(string));
            }

            public string this[int index]
            {
                get
                {
                    return (string) this.Array[index];
                }
            }
        }
    }
}

