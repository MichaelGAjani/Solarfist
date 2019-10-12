namespace Jund.OpcHelper.OpcCom.Hda
{
    using Opc.Hda;
    using OpcCom;
    using OpcRcw.Hda;
    using System;
    using System.Collections;

    internal class DataCallback : IOPCHDA_DataCallback
    {
        private CallbackExceptionEventHandler m_callbackException = null;
        private int m_nextID = 0;
        private Hashtable m_requests = new Hashtable();

        public event CallbackExceptionEventHandler CallbackException
        {
            add
            {
                lock (this)
                {
                    this.m_callbackException = (CallbackExceptionEventHandler) Delegate.Combine(this.m_callbackException, value);
                }
            }
            remove
            {
                lock (this)
                {
                    this.m_callbackException = (CallbackExceptionEventHandler) Delegate.Remove(this.m_callbackException, value);
                }
            }
        }

        public bool CancelRequest(Request request, CancelCompleteEventHandler callback)
        {
            lock (this)
            {
                if (!this.m_requests.Contains(request.RequestID))
                {
                    return false;
                }
                if (callback != null)
                {
                    request.CancelComplete += callback;
                }
                else
                {
                    this.m_requests.Remove(request.RequestID);
                }
                return true;
            }
        }

        public Request CreateRequest(object requestHandle, Delegate callback)
        {
            lock (this)
            {
                Request request = new Request(requestHandle, callback, ++this.m_nextID);
                this.m_requests[request.RequestID] = request;
                return request;
            }
        }

        private void HandleException(int requestID, Exception exception)
        {
            lock (this)
            {
                Request request = (Request) this.m_requests[requestID];
                if ((request != null) && (this.m_callbackException != null))
                {
                    this.m_callbackException(request, exception);
                }
            }
        }

        public void OnCancelComplete(int dwCancelID)
        {
            try
            {
                lock (this)
                {
                    Request request = (Request) this.m_requests[dwCancelID];
                    if (request != null)
                    {
                        request.OnCancelComplete();
                        this.m_requests.Remove(request.RequestID);
                    }
                }
            }
            catch (Exception exception)
            {
                this.HandleException(dwCancelID, exception);
            }
        }

        public void OnDataChange(int dwTransactionID, int hrStatus, int dwNumItems, OPCHDA_ITEM[] pItemValues, int[] phrErrors)
        {
            try
            {
                lock (this)
                {
                    Request request = (Request) this.m_requests[dwTransactionID];
                    if (request != null)
                    {
                        ItemValueCollection[] results = new ItemValueCollection[pItemValues.Length];
                        for (int i = 0; i < pItemValues.Length; i++)
                        {
                            results[i] = OpcCom.Hda.Interop.GetItemValueCollection(pItemValues[i], false);
                            results[i].ServerHandle = results[i].ClientHandle;
                            results[i].ClientHandle = null;
                            results[i].ResultID = OpcCom.Interop.GetResultID(phrErrors[i]);
                        }
                        if (request.InvokeCallback(results))
                        {
                            this.m_requests.Remove(request.RequestID);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.HandleException(dwTransactionID, exception);
            }
        }

        public void OnInsertAnnotations(int dwTransactionID, int hrStatus, int dwCount, int[] phClients, int[] phrErrors)
        {
            try
            {
                lock (this)
                {
                    Request request = (Request) this.m_requests[dwTransactionID];
                    if (request != null)
                    {
                        ArrayList list = new ArrayList();
                        if (dwCount > 0)
                        {
                            int num = phClients[0];
                            ResultCollection results = new ResultCollection();
                            for (int i = 0; i < dwCount; i++)
                            {
                                if (phClients[i] != num)
                                {
                                    results.ServerHandle = num;
                                    list.Add(results);
                                    num = phClients[i];
                                    results = new ResultCollection();
                                }
                                Result result = new Result(OpcCom.Interop.GetResultID(phrErrors[i]));
                                results.Add(result);
                            }
                            results.ServerHandle = num;
                            list.Add(results);
                        }
                        if (request.InvokeCallback((ResultCollection[]) list.ToArray(typeof(ResultCollection))))
                        {
                            this.m_requests.Remove(request.RequestID);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.HandleException(dwTransactionID, exception);
            }
        }

        public void OnPlayback(int dwTransactionID, int hrStatus, int dwNumItems, IntPtr ppItemValues, int[] phrErrors)
        {
            try
            {
                lock (this)
                {
                    Request request = (Request) this.m_requests[dwTransactionID];
                    if (request != null)
                    {
                        ItemValueCollection[] results = new ItemValueCollection[dwNumItems];
                        int[] numArray = OpcCom.Interop.GetInt32s(ref ppItemValues, dwNumItems, false);
                        for (int i = 0; i < dwNumItems; i++)
                        {
                            IntPtr pInput = (IntPtr) numArray[i];
                            ItemValueCollection[] valuesArray2 = OpcCom.Hda.Interop.GetItemValueCollections(ref pInput, 1, false);
                            if ((valuesArray2 != null) && (valuesArray2.Length == 1))
                            {
                                results[i] = valuesArray2[0];
                                results[i].ServerHandle = results[i].ClientHandle;
                                results[i].ClientHandle = null;
                                results[i].ResultID = OpcCom.Interop.GetResultID(phrErrors[i]);
                            }
                        }
                        if (request.InvokeCallback(results))
                        {
                            this.m_requests.Remove(request.RequestID);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.HandleException(dwTransactionID, exception);
            }
        }

        public void OnReadAnnotations(int dwTransactionID, int hrStatus, int dwNumItems, OPCHDA_ANNOTATION[] pAnnotationValues, int[] phrErrors)
        {
            try
            {
                lock (this)
                {
                    Request request = (Request) this.m_requests[dwTransactionID];
                    if (request != null)
                    {
                        AnnotationValueCollection[] results = new AnnotationValueCollection[pAnnotationValues.Length];
                        for (int i = 0; i < pAnnotationValues.Length; i++)
                        {
                            results[i] = OpcCom.Hda.Interop.GetAnnotationValueCollection(pAnnotationValues[i], false);
                            results[i].ServerHandle = pAnnotationValues[i].hClient;
                            results[i].ResultID = OpcCom.Interop.GetResultID(phrErrors[i]);
                        }
                        if (request.InvokeCallback(results))
                        {
                            this.m_requests.Remove(request.RequestID);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.HandleException(dwTransactionID, exception);
            }
        }

        public void OnReadAttributeComplete(int dwTransactionID, int hrStatus, int hClient, int dwNumItems, OPCHDA_ATTRIBUTE[] pAttributeValues, int[] phrErrors)
        {
            try
            {
                lock (this)
                {
                    Request request = (Request) this.m_requests[dwTransactionID];
                    if (request != null)
                    {
                        ItemAttributeCollection results = new ItemAttributeCollection {
                            ServerHandle = hClient
                        };
                        AttributeValueCollection[] valuesArray = new AttributeValueCollection[pAttributeValues.Length];
                        for (int i = 0; i < pAttributeValues.Length; i++)
                        {
                            valuesArray[i] = OpcCom.Hda.Interop.GetAttributeValueCollection(pAttributeValues[i], false);
                            valuesArray[i].ResultID = OpcCom.Interop.GetResultID(phrErrors[i]);
                            results.Add(valuesArray[i]);
                        }
                        if (request.InvokeCallback(results))
                        {
                            this.m_requests.Remove(request.RequestID);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.HandleException(dwTransactionID, exception);
            }
        }

        public void OnReadComplete(int dwTransactionID, int hrStatus, int dwNumItems, OPCHDA_ITEM[] pItemValues, int[] phrErrors)
        {
            try
            {
                lock (this)
                {
                    Request request = (Request) this.m_requests[dwTransactionID];
                    if (request != null)
                    {
                        ItemValueCollection[] results = new ItemValueCollection[pItemValues.Length];
                        for (int i = 0; i < pItemValues.Length; i++)
                        {
                            results[i] = OpcCom.Hda.Interop.GetItemValueCollection(pItemValues[i], false);
                            results[i].ServerHandle = pItemValues[i].hClient;
                            results[i].ResultID = OpcCom.Interop.GetResultID(phrErrors[i]);
                        }
                        if (request.InvokeCallback(results))
                        {
                            this.m_requests.Remove(request.RequestID);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.HandleException(dwTransactionID, exception);
            }
        }

        public void OnReadModifiedComplete(int dwTransactionID, int hrStatus, int dwNumItems, OPCHDA_MODIFIEDITEM[] pItemValues, int[] phrErrors)
        {
            try
            {
                lock (this)
                {
                    Request request = (Request) this.m_requests[dwTransactionID];
                    if (request != null)
                    {
                        ModifiedValueCollection[] results = new ModifiedValueCollection[pItemValues.Length];
                        for (int i = 0; i < pItemValues.Length; i++)
                        {
                            results[i] = OpcCom.Hda.Interop.GetModifiedValueCollection(pItemValues[i], false);
                            results[i].ServerHandle = pItemValues[i].hClient;
                            results[i].ResultID = OpcCom.Interop.GetResultID(phrErrors[i]);
                        }
                        if (request.InvokeCallback(results))
                        {
                            this.m_requests.Remove(request.RequestID);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.HandleException(dwTransactionID, exception);
            }
        }

        public void OnUpdateComplete(int dwTransactionID, int hrStatus, int dwCount, int[] phClients, int[] phrErrors)
        {
            try
            {
                lock (this)
                {
                    Request request = (Request) this.m_requests[dwTransactionID];
                    if (request != null)
                    {
                        ArrayList list = new ArrayList();
                        if (dwCount > 0)
                        {
                            int num = phClients[0];
                            ResultCollection results = new ResultCollection();
                            for (int i = 0; i < dwCount; i++)
                            {
                                if (phClients[i] != num)
                                {
                                    results.ServerHandle = num;
                                    list.Add(results);
                                    num = phClients[i];
                                    results = new ResultCollection();
                                }
                                Result result = new Result(OpcCom.Interop.GetResultID(phrErrors[i]));
                                results.Add(result);
                            }
                            results.ServerHandle = num;
                            list.Add(results);
                        }
                        if (request.InvokeCallback((ResultCollection[]) list.ToArray(typeof(ResultCollection))))
                        {
                            this.m_requests.Remove(request.RequestID);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.HandleException(dwTransactionID, exception);
            }
        }
    }
}

