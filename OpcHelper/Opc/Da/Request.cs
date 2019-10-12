namespace Jund.OpcHelper.Opc.Da
{
    using Opc;
    using System;

    [Serializable]
    public class Request : IRequest
    {
        private object m_handle = null;
        private ISubscription m_subscription = null;

        public Request(ISubscription subscription, object handle)
        {
            this.m_subscription = subscription;
            this.m_handle = handle;
        }

        public void Cancel(CancelCompleteEventHandler callback)
        {
            this.m_subscription.Cancel(this, callback);
        }

        public object Handle
        {
            get
            {
                return this.m_handle;
            }
        }

        public ISubscription Subscription
        {
            get
            {
                return this.m_subscription;
            }
        }
    }
}

