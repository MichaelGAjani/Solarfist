namespace Jund.OpcHelper.Opc.Da
{
    using Opc;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    [Serializable]
    public class Subscription : ISubscription, IDisposable, ISerializable, ICloneable
    {
        private bool m_enabled;
        private int m_filters;
        private Item[] m_items;
        internal Opc.Da.Server m_server;
        private SubscriptionState m_state;
        internal ISubscription m_subscription;

        public event DataChangedEventHandler DataChanged
        {
            add
            {
                this.m_subscription.DataChanged += value;
            }
            remove
            {
                this.m_subscription.DataChanged -= value;
            }
        }

        public Subscription(Opc.Da.Server server, ISubscription subscription)
        {
            this.m_server = null;
            this.m_subscription = null;
            this.m_state = new SubscriptionState();
            this.m_items = null;
            this.m_enabled = true;
            this.m_filters = 9;
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
            this.GetResultFilters();
            this.GetState();
        }

        protected Subscription(SerializationInfo info, StreamingContext context)
        {
            this.m_server = null;
            this.m_subscription = null;
            this.m_state = new SubscriptionState();
            this.m_items = null;
            this.m_enabled = true;
            this.m_filters = 9;
            this.m_state = (SubscriptionState) info.GetValue("State", typeof(SubscriptionState));
            this.m_filters = (int) info.GetValue("Filters", typeof(int));
            this.m_items = (Item[]) info.GetValue("Items", typeof(Item[]));
        }

        public virtual ItemResult[] AddItems(Item[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (items.Length == 0)
            {
                return new ItemResult[0];
            }
            ItemResult[] resultArray = this.m_subscription.AddItems(items);
            if ((resultArray == null) || (resultArray.Length == 0))
            {
                throw new InvalidResponseException();
            }
            ArrayList list = new ArrayList();
            if (this.m_items != null)
            {
                list.AddRange(this.m_items);
            }
            for (int i = 0; i < resultArray.Length; i++)
            {
                if (!resultArray[i].ResultID.Failed())
                {
                    Item item = new Item(resultArray[i]) {
                        ItemName = items[i].ItemName,
                        ItemPath = items[i].ItemPath,
                        ClientHandle = items[i].ClientHandle
                    };
                    list.Add(item);
                }
            }
            this.m_items = (Item[]) list.ToArray(typeof(Item));
            this.GetState();
            return resultArray;
        }

        public void Cancel(IRequest request, CancelCompleteEventHandler callback)
        {
            this.m_subscription.Cancel(request, callback);
        }

        public virtual object Clone()
        {
            Subscription subscription = (Subscription) base.MemberwiseClone();
            subscription.m_server = null;
            subscription.m_subscription = null;
            subscription.m_state = (SubscriptionState) this.m_state.Clone();
            subscription.m_state.ServerHandle = null;
            subscription.m_state.Active = false;
            if (subscription.m_items != null)
            {
                ArrayList list = new ArrayList();
                foreach (Item item in subscription.m_items)
                {
                    list.Add(item.Clone());
                }
                subscription.m_items = (Item[]) list.ToArray(typeof(Item));
            }
            return subscription;
        }

        public void Dispose()
        {
            if (this.m_subscription != null)
            {
                this.m_subscription.Dispose();
                this.m_server = null;
                this.m_subscription = null;
                this.m_items = null;
            }
        }

        public bool GetEnabled()
        {
            this.m_enabled = this.m_subscription.GetEnabled();
            return this.m_enabled;
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("State", this.m_state);
            info.AddValue("Filters", this.m_filters);
            info.AddValue("Items", this.m_items);
        }

        public int GetResultFilters()
        {
            this.m_filters = this.m_subscription.GetResultFilters();
            return this.m_filters;
        }

        public SubscriptionState GetState()
        {
            this.m_state = this.m_subscription.GetState();
            return this.m_state;
        }

        public virtual ItemResult[] ModifyItems(int masks, Item[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (items.Length == 0)
            {
                return new ItemResult[0];
            }
            ItemResult[] resultArray = this.m_subscription.ModifyItems(masks, items);
            if ((resultArray == null) || (resultArray.Length == 0))
            {
                throw new InvalidResponseException();
            }
            for (int i = 0; i < resultArray.Length; i++)
            {
                if (!resultArray[i].ResultID.Failed())
                {
                    for (int j = 0; j < this.m_items.Length; j++)
                    {
                        if (this.m_items[j].ServerHandle.Equals(items[i].ServerHandle))
                        {
                            this.m_items[j] = new Item(resultArray[i]) { ItemName = this.m_items[j].ItemName, ItemPath = this.m_items[j].ItemPath, ClientHandle = this.m_items[j].ClientHandle };
                            break;
                        }
                    }
                }
            }
            this.GetState();
            return resultArray;
        }

        public SubscriptionState ModifyState(int masks, SubscriptionState state)
        {
            this.m_state = this.m_subscription.ModifyState(masks, state);
            return this.m_state;
        }

        public ItemValueResult[] Read(Item[] items)
        {
            return this.m_subscription.Read(items);
        }

        public IdentifiedResult[] Read(Item[] items, object requestHandle, ReadCompleteEventHandler callback, out IRequest request)
        {
            return this.m_subscription.Read(items, requestHandle, callback, out request);
        }

        public void Refresh()
        {
            this.m_subscription.Refresh();
        }

        public void Refresh(object requestHandle, out IRequest request)
        {
            this.m_subscription.Refresh(requestHandle, out request);
        }

        public virtual IdentifiedResult[] RemoveItems(ItemIdentifier[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (items.Length == 0)
            {
                return new IdentifiedResult[0];
            }
            IdentifiedResult[] resultArray = this.m_subscription.RemoveItems(items);
            if ((resultArray == null) || (resultArray.Length == 0))
            {
                throw new InvalidResponseException();
            }
            ArrayList list = new ArrayList();
            foreach (Item item in this.m_items)
            {
                bool flag = false;
                for (int i = 0; i < resultArray.Length; i++)
                {
                    if (item.ServerHandle.Equals(items[i].ServerHandle))
                    {
                        flag = resultArray[i].ResultID.Succeeded();
                        break;
                    }
                }
                if (!flag)
                {
                    list.Add(item);
                }
            }
            this.m_items = (Item[]) list.ToArray(typeof(Item));
            this.GetState();
            return resultArray;
        }

        public void SetEnabled(bool enabled)
        {
            this.m_subscription.SetEnabled(enabled);
            this.m_enabled = enabled;
        }

        public void SetResultFilters(int filters)
        {
            this.m_subscription.SetResultFilters(filters);
            this.m_filters = filters;
        }

        public IdentifiedResult[] Write(ItemValue[] items)
        {
            return this.m_subscription.Write(items);
        }

        public IdentifiedResult[] Write(ItemValue[] items, object requestHandle, WriteCompleteEventHandler callback, out IRequest request)
        {
            return this.m_subscription.Write(items, requestHandle, callback, out request);
        }

        public bool Active
        {
            get
            {
                return this.m_state.Active;
            }
        }

        public object ClientHandle
        {
            get
            {
                return this.m_state.ClientHandle;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.m_enabled;
            }
        }

        public int Filters
        {
            get
            {
                return this.m_filters;
            }
        }

        public Item[] Items
        {
            get
            {
                if (this.m_items == null)
                {
                    return new Item[0];
                }
                Item[] itemArray = new Item[this.m_items.Length];
                for (int i = 0; i < this.m_items.Length; i++)
                {
                    itemArray[i] = (Item) this.m_items[i].Clone();
                }
                return itemArray;
            }
        }

        public string Locale
        {
            get
            {
                return this.m_state.Locale;
            }
        }

        public string Name
        {
            get
            {
                return this.m_state.Name;
            }
        }

        public Opc.Da.Server Server
        {
            get
            {
                return this.m_server;
            }
        }

        public object ServerHandle
        {
            get
            {
                return this.m_state.ServerHandle;
            }
        }

        public SubscriptionState State
        {
            get
            {
                return (SubscriptionState) this.m_state.Clone();
            }
        }

        private class Names
        {
            internal const string FILTERS = "Filters";
            internal const string ITEMS = "Items";
            internal const string STATE = "State";
        }
    }
}

