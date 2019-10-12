namespace Jund.OpcHelper.Opc.Ae
{
    using Opc;
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.Serialization;

    [Serializable]
    public class Subscription : ISubscription, IDisposable, ISerializable, ICloneable
    {
        private StringCollection m_areas;
        private AttributeDictionary m_attributes;
        private CategoryCollection m_categories;
        private SubscriptionFilters m_filters;
        private string m_name;
        private Opc.Ae.Server m_server;
        private StringCollection m_sources;
        private SubscriptionState m_state;
        private ISubscription m_subscription;

        public event EventChangedEventHandler EventChanged
        {
            add
            {
                this.m_subscription.EventChanged += value;
            }
            remove
            {
                this.m_subscription.EventChanged -= value;
            }
        }

        protected Subscription(SerializationInfo info, StreamingContext context)
        {
            this.m_server = null;
            this.m_subscription = null;
            this.m_state = new SubscriptionState();
            this.m_name = null;
            this.m_filters = new SubscriptionFilters();
            this.m_categories = new CategoryCollection();
            this.m_areas = new StringCollection();
            this.m_sources = new StringCollection();
            this.m_attributes = new AttributeDictionary();
            this.m_state = (SubscriptionState) info.GetValue("ST", typeof(SubscriptionState));
            this.m_filters = (SubscriptionFilters) info.GetValue("FT", typeof(SubscriptionFilters));
            this.m_attributes = (AttributeDictionary) info.GetValue("AT", typeof(AttributeDictionary));
            this.m_name = this.m_state.Name;
            this.m_categories = new CategoryCollection(this.m_filters.Categories.ToArray());
            this.m_areas = new StringCollection(this.m_filters.Areas.ToArray());
            this.m_sources = new StringCollection(this.m_filters.Sources.ToArray());
        }

        public Subscription(Opc.Ae.Server server, ISubscription subscription, SubscriptionState state)
        {
            this.m_server = null;
            this.m_subscription = null;
            this.m_state = new SubscriptionState();
            this.m_name = null;
            this.m_filters = new SubscriptionFilters();
            this.m_categories = new CategoryCollection();
            this.m_areas = new StringCollection();
            this.m_sources = new StringCollection();
            this.m_attributes = new AttributeDictionary();
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }
            if (subscription == null)
            {
                throw new ArgumentNullException("subscription");
            }
            this.m_server = server;
            this.m_subscription = subscription;
            this.m_state = (SubscriptionState) state.Clone();
            this.m_name = state.Name;
        }

        public void CancelRefresh()
        {
            if (this.m_subscription == null)
            {
                throw new NotConnectedException();
            }
            this.m_subscription.CancelRefresh();
        }

        public virtual object Clone()
        {
            return (Subscription) base.MemberwiseClone();
        }

        public void Dispose()
        {
            if (this.m_subscription != null)
            {
                this.m_server.SubscriptionDisposed(this);
                this.m_subscription.Dispose();
            }
        }

        public Opc.Ae.AttributeDictionary GetAttributes()
        {
            Opc.Ae.AttributeDictionary dictionary = new Opc.Ae.AttributeDictionary();
            IDictionaryEnumerator enumerator = this.m_attributes.GetEnumerator();
            while (enumerator.MoveNext())
            {
                int key = (int) enumerator.Key;
                dictionary.Add(key, ((AttributeCollection) enumerator.Value).ToArray());
            }
            return dictionary;
        }

        public SubscriptionFilters GetFilters()
        {
            if (this.m_subscription == null)
            {
                throw new NotConnectedException();
            }
            this.m_filters = this.m_subscription.GetFilters();
            this.m_categories = new CategoryCollection(this.m_filters.Categories.ToArray());
            this.m_areas = new StringCollection(this.m_filters.Areas.ToArray());
            this.m_sources = new StringCollection(this.m_filters.Sources.ToArray());
            return (SubscriptionFilters) this.m_filters.Clone();
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ST", this.m_state);
            info.AddValue("FT", this.m_filters);
            info.AddValue("AT", this.m_attributes);
        }

        public int[] GetReturnedAttributes(int eventCategory)
        {
            if (this.m_subscription == null)
            {
                throw new NotConnectedException();
            }
            int[] returnedAttributes = this.m_subscription.GetReturnedAttributes(eventCategory);
            this.m_attributes.Update(eventCategory, (int[]) Opc.Convert.Clone(returnedAttributes));
            return returnedAttributes;
        }

        public SubscriptionState GetState()
        {
            if (this.m_subscription == null)
            {
                throw new NotConnectedException();
            }
            this.m_state = this.m_subscription.GetState();
            this.m_state.Name = this.m_name;
            return (SubscriptionState) this.m_state.Clone();
        }

        public SubscriptionState ModifyState(int masks, SubscriptionState state)
        {
            if (this.m_subscription == null)
            {
                throw new NotConnectedException();
            }
            this.m_state = this.m_subscription.ModifyState(masks, state);
            if ((masks & 1) != 0)
            {
                this.m_state.Name = this.m_name = state.Name;
            }
            else
            {
                this.m_state.Name = this.m_name;
            }
            return (SubscriptionState) this.m_state.Clone();
        }

        public void Refresh()
        {
            if (this.m_subscription == null)
            {
                throw new NotConnectedException();
            }
            this.m_subscription.Refresh();
        }

        public void SelectReturnedAttributes(int eventCategory, int[] attributeIDs)
        {
            if (this.m_subscription == null)
            {
                throw new NotConnectedException();
            }
            this.m_subscription.SelectReturnedAttributes(eventCategory, attributeIDs);
            this.m_attributes.Update(eventCategory, (int[]) Opc.Convert.Clone(attributeIDs));
        }

        public void SetFilters(SubscriptionFilters filters)
        {
            if (this.m_subscription == null)
            {
                throw new NotConnectedException();
            }
            this.m_subscription.SetFilters(filters);
            this.GetFilters();
        }

        public bool Active
        {
            get
            {
                return this.m_state.Active;
            }
        }

        public StringCollection Areas
        {
            get
            {
                return this.m_areas;
            }
        }

        public AttributeDictionary Attributes
        {
            get
            {
                return this.m_attributes;
            }
        }

        public int BufferTime
        {
            get
            {
                return this.m_state.BufferTime;
            }
        }

        public CategoryCollection Categories
        {
            get
            {
                return this.m_categories;
            }
        }

        public object ClientHandle
        {
            get
            {
                return this.m_state.ClientHandle;
            }
        }

        public int EventTypes
        {
            get
            {
                return this.m_filters.EventTypes;
            }
        }

        internal SubscriptionFilters Filters
        {
            get
            {
                return this.m_filters;
            }
        }

        public int HighSeverity
        {
            get
            {
                return this.m_filters.HighSeverity;
            }
        }

        public int KeepAlive
        {
            get
            {
                return this.m_state.KeepAlive;
            }
        }

        public int LowSeverity
        {
            get
            {
                return this.m_filters.LowSeverity;
            }
        }

        public int MaxSize
        {
            get
            {
                return this.m_state.MaxSize;
            }
        }

        public string Name
        {
            get
            {
                return this.m_state.Name;
            }
        }

        public Opc.Ae.Server Server
        {
            get
            {
                return this.m_server;
            }
        }

        public StringCollection Sources
        {
            get
            {
                return this.m_sources;
            }
        }

        internal SubscriptionState State
        {
            get
            {
                return this.m_state;
            }
        }

        [Serializable]
        public class AttributeCollection : ReadOnlyCollection
        {
            internal AttributeCollection() : base(new int[0])
            {
            }

            internal AttributeCollection(int[] attributeIDs) : base(attributeIDs)
            {
            }

            protected AttributeCollection(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }

            public int[] ToArray()
            {
                return (int[]) Opc.Convert.Clone(this.Array);
            }

            public int this[int index]
            {
                get
                {
                    return (int) this.Array.GetValue(index);
                }
            }
        }

        [Serializable]
        public class AttributeDictionary : ReadOnlyDictionary
        {
            internal AttributeDictionary() : base(null)
            {
            }

            internal AttributeDictionary(Hashtable dictionary) : base(dictionary)
            {
            }

            protected AttributeDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }

            internal void Update(int categoryID, int[] attributeIDs)
            {
                this.Dictionary[categoryID] = new Subscription.AttributeCollection(attributeIDs);
            }

            public Subscription.AttributeCollection this[int categoryID]
            {
                get
                {
                    return (Subscription.AttributeCollection) base[categoryID];
                }
            }
        }

        public class CategoryCollection : ReadOnlyCollection
        {
            internal CategoryCollection() : base(new int[0])
            {
            }

            internal CategoryCollection(int[] categoryIDs) : base(categoryIDs)
            {
            }

            public int[] ToArray()
            {
                return (int[]) Opc.Convert.Clone(this.Array);
            }

            public int this[int index]
            {
                get
                {
                    return (int) this.Array.GetValue(index);
                }
            }
        }

        private class Names
        {
            internal const string ATTRIBUTES = "AT";
            internal const string FILTERS = "FT";
            internal const string STATE = "ST";
        }

        public class StringCollection : ReadOnlyCollection
        {
            internal StringCollection() : base(new string[0])
            {
            }

            internal StringCollection(string[] strings) : base(strings)
            {
            }

            public string[] ToArray()
            {
                return (string[]) Opc.Convert.Clone(this.Array);
            }

            public string this[int index]
            {
                get
                {
                    return (string) this.Array.GetValue(index);
                }
            }
        }
    }
}

