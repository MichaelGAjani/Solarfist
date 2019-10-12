namespace Jund.OpcHelper.OpcCom.Da
{
    using Opc;
    using Opc.Da;
    using System;

    [Serializable]
    public class Request : Opc.Da.Request
    {
        internal Delegate Callback;
        internal int CancelID;
        internal int Filters;
        internal ItemIdentifier[] InitialResults;
        internal int RequestID;

        public Request(ISubscription subscription, object clientHandle, int filters, int requestID, Delegate callback) : base(subscription, clientHandle)
        {
            this.RequestID = 0;
            this.CancelID = 0;
            this.Callback = null;
            this.Filters = 0;
            this.InitialResults = null;
            this.Filters = filters;
            this.RequestID = requestID;
            this.Callback = callback;
            this.CancelID = 0;
            this.InitialResults = null;
        }

        public bool BeginRead(int cancelID, IdentifiedResult[] results)
        {
            this.CancelID = cancelID;
            ItemValueResult[] initialResults = null;
            if ((this.InitialResults != null) && (this.InitialResults.GetType() == typeof(ItemValueResult[])))
            {
                initialResults = (ItemValueResult[]) this.InitialResults;
                this.InitialResults = results;
                this.EndRequest(initialResults);
                return true;
            }
            foreach (IdentifiedResult result in results)
            {
                if (result.ResultID.Succeeded())
                {
                    this.InitialResults = results;
                    return false;
                }
            }
            return true;
        }

        public bool BeginRefresh(int cancelID)
        {
            this.CancelID = cancelID;
            return false;
        }

        public bool BeginWrite(int cancelID, IdentifiedResult[] results)
        {
            this.CancelID = cancelID;
            if ((this.InitialResults != null) && (this.InitialResults.GetType() == typeof(IdentifiedResult[])))
            {
                IdentifiedResult[] initialResults = (IdentifiedResult[]) this.InitialResults;
                this.InitialResults = results;
                this.EndRequest(initialResults);
                return true;
            }
            foreach (IdentifiedResult result in results)
            {
                if (result.ResultID.Succeeded())
                {
                    this.InitialResults = results;
                    return false;
                }
            }
            for (int i = 0; i < results.Length; i++)
            {
                if ((this.Filters & 1) == 0)
                {
                    results[i].ItemName = null;
                }
                if ((this.Filters & 2) == 0)
                {
                    results[i].ItemPath = null;
                }
                if ((this.Filters & 4) == 0)
                {
                    results[i].ClientHandle = null;
                }
            }
            ((WriteCompleteEventHandler) this.Callback)(base.Handle, results);
            return true;
        }

        public void EndRequest()
        {
            if (typeof(CancelCompleteEventHandler).IsInstanceOfType(this.Callback))
            {
                ((CancelCompleteEventHandler) this.Callback)(base.Handle);
            }
        }

        public void EndRequest(ItemValueResult[] results)
        {
            if (this.InitialResults == null)
            {
                this.InitialResults = results;
            }
            else if (typeof(CancelCompleteEventHandler).IsInstanceOfType(this.Callback))
            {
                ((CancelCompleteEventHandler) this.Callback)(base.Handle);
            }
            else
            {
                for (int i = 0; i < results.Length; i++)
                {
                    if ((this.Filters & 1) == 0)
                    {
                        results[i].ItemName = null;
                    }
                    if ((this.Filters & 2) == 0)
                    {
                        results[i].ItemPath = null;
                    }
                    if ((this.Filters & 4) == 0)
                    {
                        results[i].ClientHandle = null;
                    }
                    if ((this.Filters & 8) == 0)
                    {
                        results[i].Timestamp = DateTime.MinValue;
                        results[i].TimestampSpecified = false;
                    }
                }
                if (typeof(ReadCompleteEventHandler).IsInstanceOfType(this.Callback))
                {
                    ((ReadCompleteEventHandler) this.Callback)(base.Handle, results);
                }
            }
        }

        public void EndRequest(IdentifiedResult[] callbackResults)
        {
            if (this.InitialResults == null)
            {
                this.InitialResults = callbackResults;
            }
            else if ((this.Callback != null) && (this.Callback.GetType() == typeof(CancelCompleteEventHandler)))
            {
                ((CancelCompleteEventHandler) this.Callback)(base.Handle);
            }
            else
            {
                IdentifiedResult[] initialResults = (IdentifiedResult[]) this.InitialResults;
                int index = 0;
                for (int i = 0; i < initialResults.Length; i++)
                {
                    while (index < callbackResults.Length)
                    {
                        if (callbackResults[i].ClientHandle.Equals(initialResults[index].ServerHandle))
                        {
                            callbackResults[i].ServerHandle = initialResults[index].ServerHandle;
                            callbackResults[i].ClientHandle = initialResults[index].ClientHandle;
                            initialResults[index++] = callbackResults[i];
                            break;
                        }
                        index++;
                    }
                }
                for (int j = 0; j < initialResults.Length; j++)
                {
                    if ((this.Filters & 1) == 0)
                    {
                        initialResults[j].ItemName = null;
                    }
                    if ((this.Filters & 2) == 0)
                    {
                        initialResults[j].ItemPath = null;
                    }
                    if ((this.Filters & 4) == 0)
                    {
                        initialResults[j].ClientHandle = null;
                    }
                }
                if ((this.Callback != null) && (this.Callback.GetType() == typeof(WriteCompleteEventHandler)))
                {
                    ((WriteCompleteEventHandler) this.Callback)(base.Handle, initialResults);
                }
            }
        }
    }
}

