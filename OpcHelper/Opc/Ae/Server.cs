namespace Jund.OpcHelper.Opc.Ae
{
    using Opc;
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    [Serializable]
    public class Server : Opc.Server, Opc.Ae.IServer, Opc.IServer, IDisposable, ISerializable
    {
        private bool m_disposing;
        private int m_filters;
        private SubscriptionCollection m_subscriptions;

        public Server(Factory factory, URL url) : base(factory, url)
        {
            this.m_filters = 0;
            this.m_disposing = false;
            this.m_subscriptions = new SubscriptionCollection();
        }

        protected Server(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.m_filters = 0;
            this.m_disposing = false;
            this.m_subscriptions = new SubscriptionCollection();
            int num = (int) info.GetValue("CT", typeof(int));
            this.m_subscriptions = new SubscriptionCollection();
            for (int i = 0; i < num; i++)
            {
                Subscription subscription = (Subscription) info.GetValue("SU" + i.ToString(), typeof(Subscription));
                this.m_subscriptions.Add(subscription);
            }
        }

        public ResultID[] AcknowledgeCondition(string acknowledgerID, string comment, EventAcknowledgement[] conditions)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Ae.IServer) base.m_server).AcknowledgeCondition(acknowledgerID, comment, conditions);
        }

        public BrowseElement[] Browse(string areaID, BrowseType browseType, string browseFilter)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Ae.IServer) base.m_server).Browse(areaID, browseType, browseFilter);
        }

        public BrowseElement[] Browse(string areaID, BrowseType browseType, string browseFilter, int maxElements, out IBrowsePosition position)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Ae.IServer) base.m_server).Browse(areaID, browseType, browseFilter, maxElements, out position);
        }

        public BrowseElement[] BrowseNext(int maxElements, ref IBrowsePosition position)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Ae.IServer) base.m_server).BrowseNext(maxElements, ref position);
        }

        public override void Connect(URL url, ConnectData connectData)
        {
            base.Connect(url, connectData);
            if (this.m_subscriptions.Count != 0)
            {
                SubscriptionCollection subscriptions = new SubscriptionCollection();
                foreach (Subscription subscription in this.m_subscriptions)
                {
                    try
                    {
                        subscriptions.Add(this.EstablishSubscription(subscription));
                    }
                    catch
                    {
                    }
                }
                this.m_subscriptions = subscriptions;
            }
        }

        public ISubscription CreateSubscription(SubscriptionState state)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            ISubscription subscription = ((Opc.Ae.IServer) base.m_server).CreateSubscription(state);
            if (subscription != null)
            {
                Subscription subscription2 = new Subscription(this, subscription, state);
                this.m_subscriptions.Add(subscription2);
                return subscription2;
            }
            return null;
        }

        public ResultID[] DisableConditionByArea(string[] areas)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Ae.IServer) base.m_server).DisableConditionByArea(areas);
        }

        public ResultID[] DisableConditionBySource(string[] sources)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Ae.IServer) base.m_server).DisableConditionBySource(sources);
        }

        public override void Disconnect()
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            this.m_disposing = true;
            foreach (Subscription subscription in this.m_subscriptions)
            {
                subscription.Dispose();
            }
            this.m_disposing = false;
            base.Disconnect();
        }

        public ResultID[] EnableConditionByArea(string[] areas)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Ae.IServer) base.m_server).EnableConditionByArea(areas);
        }

        public ResultID[] EnableConditionBySource(string[] sources)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Ae.IServer) base.m_server).EnableConditionBySource(sources);
        }

        private Subscription EstablishSubscription(Subscription template)
        {
            ISubscription subscription = null;
            try
            {
                subscription = ((Opc.Ae.IServer) base.m_server).CreateSubscription(template.State);
                if (subscription == null)
                {
                    return null;
                }
                Subscription subscription2 = new Subscription(this, subscription, template.State);
                subscription2.SetFilters(template.Filters);
                IDictionaryEnumerator enumerator = template.Attributes.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    subscription2.SelectReturnedAttributes((int) enumerator.Key, ((Subscription.AttributeCollection) enumerator.Value).ToArray());
                }
                return subscription2;
            }
            catch
            {
                if (subscription != null)
                {
                    subscription.Dispose();
                    subscription = null;
                }
            }
            return null;
        }

        public Condition GetConditionState(string sourceName, string conditionName, int[] attributeIDs)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Ae.IServer) base.m_server).GetConditionState(sourceName, conditionName, attributeIDs);
        }

        public EnabledStateResult[] GetEnableStateByArea(string[] areas)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Ae.IServer) base.m_server).GetEnableStateByArea(areas);
        }

        public EnabledStateResult[] GetEnableStateBySource(string[] sources)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Ae.IServer) base.m_server).GetEnableStateBySource(sources);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("CT", this.m_subscriptions.Count);
            for (int i = 0; i < this.m_subscriptions.Count; i++)
            {
                info.AddValue("SU" + i.ToString(), this.m_subscriptions[i]);
            }
        }

        public ServerStatus GetStatus()
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            ServerStatus status = ((Opc.Ae.IServer) base.m_server).GetStatus();
            if (status.StatusInfo == null)
            {
                status.StatusInfo = base.GetString("serverState." + status.ServerState.ToString());
            }
            return status;
        }

        public int QueryAvailableFilters()
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            this.m_filters = ((Opc.Ae.IServer) base.m_server).QueryAvailableFilters();
            return this.m_filters;
        }

        public string[] QueryConditionNames(int eventCategory)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Ae.IServer) base.m_server).QueryConditionNames(eventCategory);
        }

        public string[] QueryConditionNames(string sourceName)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Ae.IServer) base.m_server).QueryConditionNames(sourceName);
        }

        public Opc.Ae.Attribute[] QueryEventAttributes(int eventCategory)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Ae.IServer) base.m_server).QueryEventAttributes(eventCategory);
        }

        public Category[] QueryEventCategories(int eventType)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Ae.IServer) base.m_server).QueryEventCategories(eventType);
        }

        public string[] QuerySubConditionNames(string conditionName)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Ae.IServer) base.m_server).QuerySubConditionNames(conditionName);
        }

        internal void SubscriptionDisposed(Subscription subscription)
        {
            if (!this.m_disposing)
            {
                this.m_subscriptions.Remove(subscription);
            }
        }

        public ItemUrl[] TranslateToItemIDs(string sourceName, int eventCategory, string conditionName, string subConditionName, int[] attributeIDs)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Ae.IServer) base.m_server).TranslateToItemIDs(sourceName, eventCategory, conditionName, subConditionName, attributeIDs);
        }

        public int AvailableFilters
        {
            get
            {
                return this.m_filters;
            }
        }

        public SubscriptionCollection Subscriptions
        {
            get
            {
                return this.m_subscriptions;
            }
        }

        private class Names
        {
            internal const string COUNT = "CT";
            internal const string SUBSCRIPTION = "SU";
        }

        public class SubscriptionCollection : ReadOnlyCollection
        {
            internal SubscriptionCollection() : base(new Subscription[0])
            {
            }

            internal void Add(Subscription subscription)
            {
                Subscription[] array = new Subscription[this.Count + 1];
                this.Array.CopyTo(array, 0);
                array[this.Count] = subscription;
                this.Array = array;
            }

            internal void Remove(Subscription subscription)
            {
                Subscription[] subscriptionArray = new Subscription[this.Count - 1];
                int num = 0;
                for (int i = 0; i < this.Array.Length; i++)
                {
                    Subscription subscription2 = (Subscription) this.Array.GetValue(i);
                    if (subscription != subscription2)
                    {
                        subscriptionArray[num++] = subscription2;
                    }
                }
                this.Array = subscriptionArray;
            }

            public Subscription[] ToArray()
            {
                return (Subscription[]) this.Array;
            }

            public Subscription this[int index]
            {
                get
                {
                    return (Subscription) this.Array.GetValue(index);
                }
            }
        }
    }
}

