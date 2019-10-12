namespace Jund.OpcHelper.Opc.Da
{
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class SubscriptionCollection : ICloneable, IList, ICollection, IEnumerable
    {
        private ArrayList m_subscriptions;

        public SubscriptionCollection()
        {
            this.m_subscriptions = new ArrayList();
        }

        public SubscriptionCollection(SubscriptionCollection subscriptions)
        {
            this.m_subscriptions = new ArrayList();
            if (subscriptions != null)
            {
                foreach (Subscription subscription in subscriptions)
                {
                    this.Add(subscription);
                }
            }
        }

        //public int Add(Subscription value)
        //{
        //    return this.Add(value);
        //}

        public int Add(object value)
        {
            if (!typeof(Subscription).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add Subscription objects into the collection.");
            }
            return this.m_subscriptions.Add(value);
        }

        public void Clear()
        {
            this.m_subscriptions.Clear();
        }

        public virtual object Clone()
        {
            SubscriptionCollection subscriptions = (SubscriptionCollection) base.MemberwiseClone();
            subscriptions.m_subscriptions = new ArrayList();
            foreach (Subscription subscription in this.m_subscriptions)
            {
                subscriptions.m_subscriptions.Add(subscription.Clone());
            }
            return subscriptions;
        }

        public bool Contains(Subscription value)
        {
            return this.Contains(value);
        }

        public bool Contains(object value)
        {
            return this.m_subscriptions.Contains(value);
        }

        public void CopyTo(Array array, int index)
        {
            if (this.m_subscriptions != null)
            {
                this.m_subscriptions.CopyTo(array, index);
            }
        }

        public void CopyTo(Subscription[] array, int index)
        {
            this.CopyTo((Array) array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_subscriptions.GetEnumerator();
        }

        public int IndexOf(Subscription value)
        {
            return this.IndexOf(value);
        }

        public int IndexOf(object value)
        {
            return this.m_subscriptions.IndexOf(value);
        }

        public void Insert(int index, Subscription value)
        {
            this.Insert(index, value);
        }

        public void Insert(int index, object value)
        {
            if (!typeof(Subscription).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add Subscription objects into the collection.");
            }
            this.m_subscriptions.Insert(index, value);
        }

        public void Remove(Subscription value)
        {
            this.Remove(value);
        }

        public void Remove(object value)
        {
            this.m_subscriptions.Remove(value);
        }

        public void RemoveAt(int index)
        {
            this.m_subscriptions.RemoveAt(index);
        }

        public int Count
        {
            get
            {
                if (this.m_subscriptions == null)
                {
                    return 0;
                }
                return this.m_subscriptions.Count;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public Subscription this[int index]
        {
            get
            {
                return (Subscription) this.m_subscriptions[index];
            }
            set
            {
                this.m_subscriptions[index] = value;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this.m_subscriptions[index];
            }
            set
            {
                if (!typeof(Subscription).IsInstanceOfType(value))
                {
                    throw new ArgumentException("May only add Subscription objects into the collection.");
                }
                this.m_subscriptions[index] = value;
            }
        }
    }
}

