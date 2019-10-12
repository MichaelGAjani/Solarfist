namespace Jund.OpcHelper.Opc.Da
{
    using Opc;
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    [Serializable]
    public class Server : Opc.Server, Opc.Da.IServer, Opc.IServer, IDisposable
    {
        private int m_filters;
        private SubscriptionCollection m_subscriptions;

        public Server(Factory factory, URL url) : base(factory, url)
        {
            this.m_subscriptions = new SubscriptionCollection();
            this.m_filters = 9;
        }

        protected Server(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.m_subscriptions = new SubscriptionCollection();
            this.m_filters = 9;
            this.m_filters = (int) info.GetValue("Filters", typeof(int));
            Subscription[] subscriptionArray = (Subscription[]) info.GetValue("Subscription", typeof(Subscription[]));
            if (subscriptionArray != null)
            {
                foreach (Subscription subscription in subscriptionArray)
                {
                    this.m_subscriptions.Add(subscription);
                }
            }
        }

        public BrowseElement[] Browse(ItemIdentifier itemID, BrowseFilters filters, out BrowsePosition position)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Da.IServer) base.m_server).Browse(itemID, filters, out position);
        }

        public BrowseElement[] BrowseNext(ref BrowsePosition position)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Da.IServer) base.m_server).BrowseNext(ref position);
        }

        public virtual void CancelSubscription(ISubscription subscription)
        {
            if (subscription == null)
            {
                throw new ArgumentNullException("subscription");
            }
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            if (!typeof(Subscription).IsInstanceOfType(subscription))
            {
                throw new ArgumentException("Incorrect object type.", "subscription");
            }
            if (!this.Equals(((Subscription) subscription).Server))
            {
                throw new ArgumentException("Unknown subscription.", "subscription");
            }
            SubscriptionCollection subscriptions = new SubscriptionCollection();
            foreach (Subscription subscription2 in this.m_subscriptions)
            {
                if (!subscription.Equals(subscription2))
                {
                    subscriptions.Add(subscription2);
                }
            }
            if (subscriptions.Count == this.m_subscriptions.Count)
            {
                throw new ArgumentException("Subscription not found.", "subscription");
            }
            this.m_subscriptions = subscriptions;
            ((Opc.Da.IServer) base.m_server).CancelSubscription(((Subscription) subscription).m_subscription);
        }

        public override object Clone()
        {
            Opc.Da.Server server = (Opc.Da.Server) base.Clone();
            if (server.m_subscriptions != null)
            {
                SubscriptionCollection subscriptions = new SubscriptionCollection();
                foreach (Subscription subscription in server.m_subscriptions)
                {
                    subscriptions.Add(subscription.Clone());
                }
                server.m_subscriptions = subscriptions;
            }
            return server;
        }

        public override void Connect(URL url, ConnectData connectData)
        {
            base.Connect(url, connectData);
            if (this.m_subscriptions != null)
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

        protected virtual Subscription CreateSubscription(ISubscription subscription)
        {
            return new Subscription(this, subscription);
        }

        public virtual ISubscription CreateSubscription(SubscriptionState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            ISubscription subscription = ((Opc.Da.IServer) base.m_server).CreateSubscription(state);
            subscription.SetResultFilters(this.m_filters);
            SubscriptionCollection subscriptions = new SubscriptionCollection();
            if (this.m_subscriptions != null)
            {
                foreach (Subscription subscription2 in this.m_subscriptions)
                {
                    subscriptions.Add(subscription2);
                }
            }
            subscriptions.Add(this.CreateSubscription(subscription));
            this.m_subscriptions = subscriptions;
            return this.m_subscriptions[this.m_subscriptions.Count - 1];
        }

        public override void Disconnect()
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            if (this.m_subscriptions != null)
            {
                foreach (Subscription subscription in this.m_subscriptions)
                {
                    subscription.Dispose();
                }
                this.m_subscriptions = null;
            }
            base.Disconnect();
        }

        private Subscription EstablishSubscription(Subscription template)
        {
            Subscription subscription = new Subscription(this, ((Opc.Da.IServer) base.m_server).CreateSubscription(template.State));
            subscription.SetResultFilters(template.Filters);
            try
            {
                subscription.AddItems(template.Items);
            }
            catch
            {
                subscription.Dispose();
                subscription = null;
            }
            return subscription;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Filters", this.m_filters);
            Subscription[] subscriptionArray = null;
            if (this.m_subscriptions.Count > 0)
            {
                subscriptionArray = new Subscription[this.m_subscriptions.Count];
                for (int i = 0; i < subscriptionArray.Length; i++)
                {
                    subscriptionArray[i] = this.m_subscriptions[i];
                }
            }
            info.AddValue("Subscription", subscriptionArray);
        }

        public ItemPropertyCollection[] GetProperties(ItemIdentifier[] itemIDs, PropertyID[] propertyIDs, bool returnValues)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Da.IServer) base.m_server).GetProperties(itemIDs, propertyIDs, returnValues);
        }

        public int GetResultFilters()
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            this.m_filters = ((Opc.Da.IServer) base.m_server).GetResultFilters();
            return this.m_filters;
        }

        public ServerStatus GetStatus()
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            ServerStatus status = ((Opc.Da.IServer) base.m_server).GetStatus();
            if (status.StatusInfo == null)
            {
                status.StatusInfo = base.GetString("serverState." + status.ServerState.ToString());
            }
            return status;
        }

        public ItemValueResult[] Read(Item[] items)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Da.IServer) base.m_server).Read(items);
        }

        public void SetResultFilters(int filters)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            ((Opc.Da.IServer) base.m_server).SetResultFilters(filters);
            this.m_filters = filters;
        }

        public IdentifiedResult[] Write(ItemValue[] items)
        {
            if (base.m_server == null)
            {
                throw new NotConnectedException();
            }
            return ((Opc.Da.IServer) base.m_server).Write(items);
        }

        public int Filters
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
            internal const string FILTERS = "Filters";
            internal const string SUBSCRIPTIONS = "Subscription";
        }
    }
}

