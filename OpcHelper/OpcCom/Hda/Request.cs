namespace Jund.OpcHelper.OpcCom.Hda
{
    using Opc;
    using Opc.Hda;
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    internal class Request : IRequest, IActualTime
    {
        private Delegate m_callback = null;
        private int m_cancelID = 0;
        private DateTime m_endTime = DateTime.MinValue;
        private Hashtable m_items = null;
        private object m_requestHandle = null;
        private int m_requestID = 0;
        private ArrayList m_results = null;
        private DateTime m_startTime = DateTime.MinValue;

        public event CancelCompleteEventHandler CancelComplete
        {
            add
            {
                lock (this)
                {
                    this.m_cancelComplete = (CancelCompleteEventHandler) Delegate.Combine(this.m_cancelComplete, value);
                }
            }
            remove
            {
                lock (this)
                {
                    this.m_cancelComplete = (CancelCompleteEventHandler) Delegate.Remove(this.m_cancelComplete, value);
                }
            }
        }

        private event CancelCompleteEventHandler m_cancelComplete;

        public Request(object requestHandle, Delegate callback, int requestID)
        {
            this.m_requestHandle = requestHandle;
            this.m_callback = callback;
            this.m_requestID = requestID;
        }

        public bool InvokeCallback(object results)
        {
            lock (this)
            {
                if (this.m_items == null)
                {
                    if (this.m_results == null)
                    {
                        this.m_results = new ArrayList();
                    }
                    this.m_results.Add(results);
                    return false;
                }
                if (typeof(DataUpdateEventHandler).IsInstanceOfType(this.m_callback))
                {
                    return this.InvokeCallback((DataUpdateEventHandler) this.m_callback, results);
                }
                if (typeof(ReadValuesEventHandler).IsInstanceOfType(this.m_callback))
                {
                    return this.InvokeCallback((ReadValuesEventHandler) this.m_callback, results);
                }
                if (typeof(ReadAttributesEventHandler).IsInstanceOfType(this.m_callback))
                {
                    return this.InvokeCallback((ReadAttributesEventHandler) this.m_callback, results);
                }
                if (typeof(ReadAnnotationsEventHandler).IsInstanceOfType(this.m_callback))
                {
                    return this.InvokeCallback((ReadAnnotationsEventHandler) this.m_callback, results);
                }
                if (typeof(UpdateCompleteEventHandler).IsInstanceOfType(this.m_callback))
                {
                    return this.InvokeCallback((UpdateCompleteEventHandler) this.m_callback, results);
                }
                return true;
            }
        }

        private bool InvokeCallback(DataUpdateEventHandler callback, object results)
        {
            if (typeof(ItemValueCollection[]).IsInstanceOfType(results))
            {
                ItemValueCollection[] valuesArray = (ItemValueCollection[]) results;
                this.UpdateResults(valuesArray);
                try
                {
                    callback(this, valuesArray);
                }
                catch
                {
                }
            }
            return false;
        }

        private bool InvokeCallback(ReadAnnotationsEventHandler callback, object results)
        {
            if (!typeof(AnnotationValueCollection[]).IsInstanceOfType(results))
            {
                return false;
            }
            AnnotationValueCollection[] valuesArray = (AnnotationValueCollection[]) results;
            this.UpdateResults(valuesArray);
            try
            {
                callback(this, valuesArray);
            }
            catch
            {
            }
            return true;
        }

        private bool InvokeCallback(ReadAttributesEventHandler callback, object results)
        {
            if (!typeof(ItemAttributeCollection).IsInstanceOfType(results))
            {
                return false;
            }
            ItemAttributeCollection attributes = (ItemAttributeCollection) results;
            this.UpdateResults(new ItemAttributeCollection[] { attributes });
            try
            {
                callback(this, attributes);
            }
            catch
            {
            }
            return true;
        }

        private bool InvokeCallback(ReadValuesEventHandler callback, object results)
        {
            if (!typeof(ItemValueCollection[]).IsInstanceOfType(results))
            {
                return false;
            }
            ItemValueCollection[] valuesArray = (ItemValueCollection[]) results;
            this.UpdateResults(valuesArray);
            try
            {
                callback(this, valuesArray);
            }
            catch
            {
            }
            foreach (ItemValueCollection values in valuesArray)
            {
                if (values.ResultID == ResultID.Hda.S_MOREDATA)
                {
                    return false;
                }
            }
            return true;
        }

        private bool InvokeCallback(UpdateCompleteEventHandler callback, object results)
        {
            if (!typeof(ResultCollection[]).IsInstanceOfType(results))
            {
                return false;
            }
            ResultCollection[] resultsArray = (ResultCollection[]) results;
            this.UpdateResults(resultsArray);
            try
            {
                callback(this, resultsArray);
            }
            catch
            {
            }
            return true;
        }

        public void OnCancelComplete()
        {
            lock (this)
            {
                if (this.m_cancelComplete != null)
                {
                    this.m_cancelComplete(this);
                }
            }
        }

        public bool Update(int cancelID, ItemIdentifier[] results)
        {
            lock (this)
            {
                this.m_cancelID = cancelID;
                this.m_items = new Hashtable();
                foreach (ItemIdentifier identifier in results)
                {
                    if (!typeof(IResult).IsInstanceOfType(identifier) || ((IResult) identifier).ResultID.Succeeded())
                    {
                        this.m_items[identifier.ServerHandle] = new ItemIdentifier(identifier);
                    }
                }
                if (this.m_items.Count == 0)
                {
                    return true;
                }
                bool flag = false;
                if (this.m_results != null)
                {
                    foreach (object obj2 in this.m_results)
                    {
                        flag = this.InvokeCallback(obj2);
                    }
                }
                return flag;
            }
        }

        private void UpdateResults(ItemIdentifier[] results)
        {
            foreach (ItemIdentifier identifier in results)
            {
                if (typeof(IActualTime).IsInstanceOfType(identifier))
                {
                    ((IActualTime) identifier).StartTime = this.StartTime;
                    ((IActualTime) identifier).EndTime = this.EndTime;
                }
                ItemIdentifier identifier2 = (ItemIdentifier) this.m_items[identifier.ServerHandle];
                if (identifier2 != null)
                {
                    identifier.ItemName = identifier2.ItemName;
                    identifier.ItemPath = identifier2.ItemPath;
                    identifier.ServerHandle = identifier2.ServerHandle;
                    identifier.ClientHandle = identifier2.ClientHandle;
                }
            }
        }

        public int CancelID
        {
            get
            {
                return this.m_cancelID;
            }
        }

        public DateTime EndTime
        {
            get
            {
                return this.m_endTime;
            }
            set
            {
                this.m_endTime = value;
            }
        }

        public object Handle
        {
            get
            {
                return this.m_requestHandle;
            }
        }

        public int RequestID
        {
            get
            {
                return this.m_requestID;
            }
        }

        public DateTime StartTime
        {
            get
            {
                return this.m_startTime;
            }
            set
            {
                this.m_startTime = value;
            }
        }
    }
}

