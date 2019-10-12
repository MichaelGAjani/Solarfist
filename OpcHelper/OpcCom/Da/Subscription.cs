namespace Jund.OpcHelper.OpcCom.Da
{
    using Opc;
    using Opc.Da;
    using OpcCom;
    using OpcRcw.Da;
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Subscription : ISubscription, IDisposable
    {
        private Callback m_callback = null;
        protected ConnectionPoint m_connection = null;
        protected int m_counter = 0;
        protected int m_filters = 9;
        protected object m_group = null;
        protected object m_handle = null;
        private ItemTable m_items = new ItemTable();
        protected string m_name = null;

        public event DataChangedEventHandler DataChanged
        {
            add
            {
                lock (this)
                {
                    this.m_callback.DataChanged += value;
                    this.Advise();
                }
            }
            remove
            {
                lock (this)
                {
                    this.m_callback.DataChanged -= value;
                    this.Unadvise();
                }
            }
        }

        internal Subscription(object group, SubscriptionState state, int filters)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }
            this.m_group = group;
            this.m_name = state.Name;
            this.m_handle = state.ClientHandle;
            this.m_filters = filters;
            this.m_callback = new Callback(state.ClientHandle, this.m_filters, this.m_items);
        }

        public ItemResult[] AddItems(Item[] items)
        {
            ItemResult[] resultArray2;
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (items.Length == 0)
            {
                return new ItemResult[0];
            }
            lock (this)
            {
                ItemTable table;
                if (this.m_group == null)
                {
                    throw new NotConnectedException();
                }
                int length = items.Length;
                OPCITEMDEF[] oPCITEMDEFs = OpcCom.Da.Interop.GetOPCITEMDEFs(items);
                for (int i = 0; i < length; i++)
                {
                    oPCITEMDEFs[i].hClient = ++this.m_counter;
                }
                IntPtr zero = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCItemMgt) this.m_group).AddItems(length, oPCITEMDEFs, out zero, out ppErrors);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCItemMgt.AddItems", exception);
                }
                int[] numArray = OpcCom.Da.Interop.GetItemResults(ref zero, length, true);
                int[] numArray2 = OpcCom.Interop.GetInt32s(ref ppErrors, length, true);
                ItemResult[] resultArray = new ItemResult[length];
                for (int j = 0; j < length; j++)
                {
                    resultArray[j] = new ItemResult(items[j]);
                    resultArray[j].ServerHandle = numArray[j];
                    resultArray[j].ClientHandle = oPCITEMDEFs[j].hClient;
                    if (!resultArray[j].ActiveSpecified)
                    {
                        resultArray[j].Active = true;
                        resultArray[j].ActiveSpecified = true;
                    }
                    resultArray[j].ResultID = OpcCom.Interop.GetResultID(numArray2[j]);
                    resultArray[j].DiagnosticInfo = null;
                    if (resultArray[j].ResultID.Succeeded())
                    {
                        resultArray[j].ClientHandle = items[j].ClientHandle;
                        lock ((table = this.m_items))
                        {
                            this.m_items[oPCITEMDEFs[j].hClient] = new ItemIdentifier(resultArray[j]);
                        }
                        resultArray[j].ClientHandle = oPCITEMDEFs[j].hClient;
                    }
                }
                this.UpdateDeadbands(resultArray);
                this.UpdateSamplingRates(resultArray);
                this.SetEnableBuffering(resultArray);
                lock ((table = this.m_items))
                {
                    resultArray2 = (ItemResult[]) this.m_items.ApplyFilters(this.m_filters, resultArray);
                }
            }
            return resultArray2;
        }

        private void Advise()
        {
            if (this.m_connection == null)
            {
                this.m_connection = new ConnectionPoint(this.m_group, typeof(IOPCDataCallback).GUID);
                this.m_connection.Advise(this.m_callback);
            }
        }

        protected virtual IdentifiedResult[] BeginRead(ItemIdentifier[] itemIDs, Item[] items, int requestID, out int cancelID)
        {
            IdentifiedResult[] resultArray2;
            try
            {
                int[] phServer = new int[itemIDs.Length];
                int[] pdwMaxAge = new int[itemIDs.Length];
                for (int i = 0; i < itemIDs.Length; i++)
                {
                    phServer[i] = (int) itemIDs[i].ServerHandle;
                    pdwMaxAge[i] = items[i].MaxAgeSpecified ? items[i].MaxAge : 0;
                }
                IntPtr zero = IntPtr.Zero;
                ((IOPCAsyncIO3) this.m_group).ReadMaxAge(itemIDs.Length, phServer, pdwMaxAge, requestID, out cancelID, out zero);
                int[] numArray3 = OpcCom.Interop.GetInt32s(ref zero, itemIDs.Length, true);
                IdentifiedResult[] resultArray = new IdentifiedResult[itemIDs.Length];
                for (int j = 0; j < itemIDs.Length; j++)
                {
                    resultArray[j] = new IdentifiedResult(itemIDs[j]);
                    resultArray[j].ResultID = OpcCom.Interop.GetResultID(numArray3[j]);
                    resultArray[j].DiagnosticInfo = null;
                    if (numArray3[j] == -1073479674)
                    {
                        resultArray[j].ResultID = new ResultID(ResultID.Da.E_WRITEONLY, -1073479674L);
                    }
                }
                resultArray2 = resultArray;
            }
            catch (Exception exception)
            {
                throw OpcCom.Interop.CreateException("IOPCAsyncIO3.ReadMaxAge", exception);
            }
            return resultArray2;
        }

        protected virtual IdentifiedResult[] BeginWrite(ItemIdentifier[] itemIDs, ItemValue[] items, int requestID, out int cancelID)
        {
            IdentifiedResult[] resultArray2;
            try
            {
                int[] phServer = new int[itemIDs.Length];
                for (int i = 0; i < itemIDs.Length; i++)
                {
                    phServer[i] = (int) itemIDs[i].ServerHandle;
                }
                OPCITEMVQT[] oPCITEMVQTs = OpcCom.Da.Interop.GetOPCITEMVQTs(items);
                IntPtr zero = IntPtr.Zero;
                ((IOPCAsyncIO3) this.m_group).WriteVQT(itemIDs.Length, phServer, oPCITEMVQTs, requestID, out cancelID, out zero);
                int[] numArray2 = OpcCom.Interop.GetInt32s(ref zero, itemIDs.Length, true);
                IdentifiedResult[] resultArray = new IdentifiedResult[itemIDs.Length];
                for (int j = 0; j < itemIDs.Length; j++)
                {
                    resultArray[j] = new IdentifiedResult(itemIDs[j]);
                    resultArray[j].ResultID = OpcCom.Interop.GetResultID(numArray2[j]);
                    resultArray[j].DiagnosticInfo = null;
                    if (numArray2[j] == -1073479674)
                    {
                        resultArray[j].ResultID = new ResultID(ResultID.Da.E_READONLY, -1073479674L);
                    }
                }
                resultArray2 = resultArray;
            }
            catch (Exception exception)
            {
                throw OpcCom.Interop.CreateException("IOPCAsyncIO3.WriteVQT", exception);
            }
            return resultArray2;
        }

        public void Cancel(IRequest request, CancelCompleteEventHandler callback)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            lock (this)
            {
                lock (request)
                {
                    if (this.m_callback.CancelRequest((OpcCom.Da.Request) request))
                    {
                        ((OpcCom.Da.Request) request).Callback = callback;
                        try
                        {
                            ((IOPCAsyncIO2) this.m_group).Cancel2(((OpcCom.Da.Request) request).CancelID);
                        }
                        catch (Exception exception)
                        {
                            throw OpcCom.Interop.CreateException("IOPCAsyncIO2.Cancel2", exception);
                        }
                    }
                }
            }
        }

        private void ClearDeadbands(ItemResult[] items)
        {
            if ((items != null) && (items.Length != 0))
            {
                try
                {
                    int[] phServer = new int[items.Length];
                    for (int i = 0; i < items.Length; i++)
                    {
                        phServer[i] = System.Convert.ToInt32(items[i].ServerHandle);
                    }
                    IntPtr zero = IntPtr.Zero;
                    ((IOPCItemDeadbandMgt) this.m_group).ClearItemDeadband(phServer.Length, phServer, out zero);
                    int[] numArray2 = OpcCom.Interop.GetInt32s(ref zero, phServer.Length, true);
                    for (int j = 0; j < numArray2.Length; j++)
                    {
                        if (OpcCom.Interop.GetResultID(numArray2[j]).Failed())
                        {
                            items[j].Deadband = 0f;
                            items[j].DeadbandSpecified = false;
                        }
                    }
                }
                catch
                {
                    for (int k = 0; k < items.Length; k++)
                    {
                        items[k].Deadband = 0f;
                        items[k].DeadbandSpecified = false;
                    }
                }
            }
        }

        private void ClearSamplingRates(ItemResult[] items)
        {
            if ((items != null) && (items.Length != 0))
            {
                try
                {
                    int[] phServer = new int[items.Length];
                    for (int i = 0; i < items.Length; i++)
                    {
                        phServer[i] = System.Convert.ToInt32(items[i].ServerHandle);
                    }
                    IntPtr zero = IntPtr.Zero;
                    ((IOPCItemSamplingMgt) this.m_group).ClearItemSamplingRate(phServer.Length, phServer, out zero);
                    int[] numArray2 = OpcCom.Interop.GetInt32s(ref zero, phServer.Length, true);
                    for (int j = 0; j < numArray2.Length; j++)
                    {
                        if (OpcCom.Interop.GetResultID(numArray2[j]).Failed())
                        {
                            items[j].SamplingRate = 0;
                            items[j].SamplingRateSpecified = false;
                        }
                    }
                }
                catch
                {
                    for (int k = 0; k < items.Length; k++)
                    {
                        items[k].SamplingRate = 0;
                        items[k].SamplingRateSpecified = false;
                    }
                }
            }
        }

        public void Dispose()
        {
            lock (this)
            {
                if (this.m_group != null)
                {
                    if (this.m_connection != null)
                    {
                        this.m_connection.Dispose();
                        this.m_connection = null;
                    }
                    OpcCom.Interop.ReleaseServer(this.m_group);
                    this.m_group = null;
                }
            }
        }

        public virtual bool GetEnabled()
        {
            bool flag;
            lock (this)
            {
                if (this.m_group == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    int pbEnable = 0;
                    ((IOPCAsyncIO3) this.m_group).GetEnable(out pbEnable);
                    flag = pbEnable != 0;
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCAsyncIO3.GetEnable", exception);
                }
            }
            return flag;
        }

        public int GetResultFilters()
        {
            lock (this)
            {
                return this.m_filters;
            }
        }

        public SubscriptionState GetState()
        {
            lock (this)
            {
                SubscriptionState state = new SubscriptionState {
                    ClientHandle = this.m_handle
                };
                try
                {
                    string ppName = null;
                    int pActive = 0;
                    int pUpdateRate = 0;
                    float pPercentDeadband = 0f;
                    int pTimeBias = 0;
                    int pLCID = 0;
                    int phClientGroup = 0;
                    int phServerGroup = 0;
                    ((IOPCGroupStateMgt) this.m_group).GetState(out pUpdateRate, out pActive, out ppName, out pTimeBias, out pPercentDeadband, out pLCID, out phClientGroup, out phServerGroup);
                    state.Name = ppName;
                    state.ServerHandle = phServerGroup;
                    state.Active = pActive != 0;
                    state.UpdateRate = pUpdateRate;
                    state.Deadband = pPercentDeadband;
                    state.Locale = OpcCom.Interop.GetLocale(pLCID);
                    this.m_name = state.Name;
                    try
                    {
                        int pdwKeepAliveTime = 0;
                        ((IOPCGroupStateMgt2) this.m_group).GetKeepAlive(out pdwKeepAliveTime);
                        state.KeepAlive = pdwKeepAliveTime;
                    }
                    catch
                    {
                        state.KeepAlive = 0;
                    }
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCGroupStateMgt.GetState", exception);
                }
                return state;
            }
        }

        public ItemResult[] ModifyItems(int masks, Item[] items)
        {
            ItemResult[] resultArray2;
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (items.Length == 0)
            {
                return new ItemResult[0];
            }
            lock (this)
            {
                ItemTable table;
                if (this.m_group == null)
                {
                    throw new NotConnectedException();
                }
                ItemResult[] resultArray = null;
                lock ((table = this.m_items))
                {
                    resultArray = this.m_items.CreateItems(items);
                }
                if ((masks & 0x40) != 0)
                {
                    this.SetReqTypes(resultArray);
                }
                if ((masks & 8) != 0)
                {
                    this.UpdateActive(resultArray);
                }
                if ((masks & 0x80) != 0)
                {
                    this.UpdateDeadbands(resultArray);
                }
                if ((masks & 0x100) != 0)
                {
                    this.UpdateSamplingRates(resultArray);
                }
                if ((masks & 0x200) != 0)
                {
                    this.SetEnableBuffering(resultArray);
                }
                lock ((table = this.m_items))
                {
                    resultArray2 = (ItemResult[]) this.m_items.ApplyFilters(this.m_filters, resultArray);
                }
            }
            return resultArray2;
        }

        public SubscriptionState ModifyState(int masks, SubscriptionState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }
            lock (this)
            {
                if (((masks & 1) != 0) && (state.Name != this.m_name))
                {
                    try
                    {
                        ((IOPCGroupStateMgt) this.m_group).SetName(state.Name);
                        this.m_name = state.Name;
                    }
                    catch (Exception exception)
                    {
                        throw OpcCom.Interop.CreateException("IOPCGroupStateMgt.SetName", exception);
                    }
                }
                if ((masks & 2) != 0)
                {
                    this.m_handle = state.ClientHandle;
                    this.m_callback.SetFilters(this.m_handle, this.m_filters);
                }
                int num = state.Active ? 1 : 0;
                int num2 = ((masks & 4) != 0) ? OpcCom.Interop.GetLocale(state.Locale) : 0;
                GCHandle handle = GCHandle.Alloc(num, GCHandleType.Pinned);
                GCHandle handle2 = GCHandle.Alloc(num2, GCHandleType.Pinned);
                GCHandle handle3 = GCHandle.Alloc(state.UpdateRate, GCHandleType.Pinned);
                GCHandle handle4 = GCHandle.Alloc(state.Deadband, GCHandleType.Pinned);
                int pRevisedUpdateRate = 0;
                try
                {
                    ((IOPCGroupStateMgt) this.m_group).SetState(((masks & 0x10) != 0) ? handle3.AddrOfPinnedObject() : IntPtr.Zero, out pRevisedUpdateRate, ((masks & 8) != 0) ? handle.AddrOfPinnedObject() : IntPtr.Zero, IntPtr.Zero, ((masks & 0x80) != 0) ? handle4.AddrOfPinnedObject() : IntPtr.Zero, ((masks & 4) != 0) ? handle2.AddrOfPinnedObject() : IntPtr.Zero, IntPtr.Zero);
                }
                catch (Exception exception2)
                {
                    throw OpcCom.Interop.CreateException("IOPCGroupStateMgt.SetState", exception2);
                }
                finally
                {
                    if (handle.IsAllocated)
                    {
                        handle.Free();
                    }
                    if (handle2.IsAllocated)
                    {
                        handle2.Free();
                    }
                    if (handle3.IsAllocated)
                    {
                        handle3.Free();
                    }
                    if (handle4.IsAllocated)
                    {
                        handle4.Free();
                    }
                }
                if ((masks & 0x20) != 0)
                {
                    int pdwRevisedKeepAliveTime = 0;
                    try
                    {
                        ((IOPCGroupStateMgt2) this.m_group).SetKeepAlive(state.KeepAlive, out pdwRevisedKeepAliveTime);
                    }
                    catch
                    {
                        state.KeepAlive = 0;
                    }
                }
                return this.GetState();
            }
        }

        public ItemValueResult[] Read(Item[] items)
        {
            ItemValueResult[] resultArray2;
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (items.Length == 0)
            {
                return new ItemValueResult[0];
            }
            lock (this)
            {
                ItemTable table;
                if (this.m_group == null)
                {
                    throw new NotConnectedException();
                }
                ItemIdentifier[] itemIDs = null;
                lock ((table = this.m_items))
                {
                    itemIDs = this.m_items.GetItemIDs(items);
                }
                ItemValueResult[] results = this.Read(itemIDs, items);
                lock ((table = this.m_items))
                {
                    resultArray2 = (ItemValueResult[]) this.m_items.ApplyFilters(this.m_filters, results);
                }
            }
            return resultArray2;
        }

        protected virtual ItemValueResult[] Read(ItemIdentifier[] itemIDs, Item[] items)
        {
            ItemValueResult[] resultArray2;
            try
            {
                int[] phServer = new int[itemIDs.Length];
                int[] pdwMaxAge = new int[itemIDs.Length];
                for (int i = 0; i < itemIDs.Length; i++)
                {
                    phServer[i] = (int) itemIDs[i].ServerHandle;
                    pdwMaxAge[i] = items[i].MaxAgeSpecified ? items[i].MaxAge : 0;
                }
                IntPtr zero = IntPtr.Zero;
                IntPtr ppwQualities = IntPtr.Zero;
                IntPtr ppftTimeStamps = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCSyncIO2) this.m_group).ReadMaxAge(itemIDs.Length, phServer, pdwMaxAge, out zero, out ppwQualities, out ppftTimeStamps, out ppErrors);
                object[] objArray = OpcCom.Interop.GetVARIANTs(ref zero, itemIDs.Length, true);
                short[] numArray3 = OpcCom.Interop.GetInt16s(ref ppwQualities, itemIDs.Length, true);
                DateTime[] timeArray = OpcCom.Interop.GetFILETIMEs(ref ppftTimeStamps, itemIDs.Length, true);
                int[] numArray4 = OpcCom.Interop.GetInt32s(ref ppErrors, itemIDs.Length, true);
                ItemValueResult[] resultArray = new ItemValueResult[itemIDs.Length];
                for (int j = 0; j < itemIDs.Length; j++)
                {
                    resultArray[j] = new ItemValueResult(itemIDs[j]);
                    resultArray[j].Value = objArray[j];
                    resultArray[j].Quality = new Quality(numArray3[j]);
                    resultArray[j].QualitySpecified = objArray[j] != null;
                    resultArray[j].Timestamp = timeArray[j];
                    resultArray[j].TimestampSpecified = objArray[j] != null;
                    resultArray[j].ResultID = OpcCom.Interop.GetResultID(numArray4[j]);
                    resultArray[j].DiagnosticInfo = null;
                    if (numArray4[j] == -1073479674)
                    {
                        resultArray[j].ResultID = new ResultID(ResultID.Da.E_WRITEONLY, -1073479674L);
                    }
                }
                resultArray2 = resultArray;
            }
            catch (Exception exception)
            {
                throw OpcCom.Interop.CreateException("IOPCSyncIO2.ReadMaxAge", exception);
            }
            return resultArray2;
        }

        public IdentifiedResult[] Read(Item[] items, object requestHandle, ReadCompleteEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            request = null;
            if (items.Length == 0)
            {
                return new IdentifiedResult[0];
            }
            lock (this)
            {
                ItemTable table;
                if (this.m_group == null)
                {
                    throw new NotConnectedException();
                }
                if (this.m_connection == null)
                {
                    this.Advise();
                }
                ItemIdentifier[] itemIDs = null;
                lock ((table = this.m_items))
                {
                    itemIDs = this.m_items.GetItemIDs(items);
                }
                OpcCom.Da.Request request2 = new OpcCom.Da.Request(this, requestHandle, this.m_filters, this.m_counter++, callback);
                this.m_callback.BeginRequest(request2);
                request = request2;
                IdentifiedResult[] results = null;
                int cancelID = 0;
                try
                {
                    results = this.BeginRead(itemIDs, items, request2.RequestID, out cancelID);
                }
                catch (Exception exception)
                {
                    this.m_callback.EndRequest(request2);
                    throw exception;
                }
                lock ((table = this.m_items))
                {
                    this.m_items.ApplyFilters(this.m_filters | 4, results);
                }
                lock (request)
                {
                    if (request2.BeginRead(cancelID, results))
                    {
                        this.m_callback.EndRequest(request2);
                        request = null;
                    }
                }
                return results;
            }
        }

        public virtual void Refresh()
        {
            lock (this)
            {
                if (this.m_group == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    int pdwCancelID = 0;
                    ((IOPCAsyncIO3) this.m_group).RefreshMaxAge(0x7fffffff, ++this.m_counter, out pdwCancelID);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCAsyncIO3.RefreshMaxAge", exception);
                }
            }
        }

        public virtual void Refresh(object requestHandle, out IRequest request)
        {
            lock (this)
            {
                if (this.m_group == null)
                {
                    throw new NotConnectedException();
                }
                if (this.m_connection == null)
                {
                    this.Advise();
                }
                OpcCom.Da.Request request2 = new OpcCom.Da.Request(this, requestHandle, this.m_filters, this.m_counter++, null);
                int pdwCancelID = 0;
                try
                {
                    ((IOPCAsyncIO3) this.m_group).RefreshMaxAge(0, request2.RequestID, out pdwCancelID);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCAsyncIO3.RefreshMaxAge", exception);
                }
                request = request2;
                lock (request)
                {
                    request2.BeginRefresh(pdwCancelID);
                }
            }
        }

        public IdentifiedResult[] RemoveItems(ItemIdentifier[] items)
        {
            IdentifiedResult[] resultArray2;
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (items.Length == 0)
            {
                return new IdentifiedResult[0];
            }
            lock (this)
            {
                ItemTable table;
                if (this.m_group == null)
                {
                    throw new NotConnectedException();
                }
                ItemIdentifier[] itemIDs = null;
                lock ((table = this.m_items))
                {
                    itemIDs = this.m_items.GetItemIDs(items);
                }
                int[] phServer = new int[itemIDs.Length];
                for (int i = 0; i < itemIDs.Length; i++)
                {
                    phServer[i] = (int) itemIDs[i].ServerHandle;
                }
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCItemMgt) this.m_group).RemoveItems(itemIDs.Length, phServer, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCItemMgt.RemoveItems", exception);
                }
                int[] numArray2 = OpcCom.Interop.GetInt32s(ref zero, itemIDs.Length, true);
                IdentifiedResult[] results = new IdentifiedResult[itemIDs.Length];
                for (int j = 0; j < itemIDs.Length; j++)
                {
                    results[j] = new IdentifiedResult(itemIDs[j]);
                    results[j].ResultID = OpcCom.Interop.GetResultID(numArray2[j]);
                    results[j].DiagnosticInfo = null;
                    if (results[j].ResultID.Succeeded())
                    {
                        lock ((table = this.m_items))
                        {
                            this.m_items[results[j].ClientHandle] = null;
                        }
                    }
                }
                lock ((table = this.m_items))
                {
                    resultArray2 = (IdentifiedResult[]) this.m_items.ApplyFilters(this.m_filters, results);
                }
            }
            return resultArray2;
        }

        private void SetActive(ItemResult[] items, bool active)
        {
            if ((items != null) && (items.Length != 0))
            {
                try
                {
                    int[] phServer = new int[items.Length];
                    for (int i = 0; i < items.Length; i++)
                    {
                        phServer[i] = System.Convert.ToInt32(items[i].ServerHandle);
                    }
                    IntPtr zero = IntPtr.Zero;
                    ((IOPCItemMgt) this.m_group).SetActiveState(items.Length, phServer, active ? 1 : 0, out zero);
                    int[] numArray2 = OpcCom.Interop.GetInt32s(ref zero, phServer.Length, true);
                    for (int j = 0; j < numArray2.Length; j++)
                    {
                        if (OpcCom.Interop.GetResultID(numArray2[j]).Failed())
                        {
                            items[j].Active = false;
                            items[j].ActiveSpecified = true;
                        }
                    }
                }
                catch
                {
                    for (int k = 0; k < items.Length; k++)
                    {
                        items[k].Active = false;
                        items[k].ActiveSpecified = true;
                    }
                }
            }
        }

        private void SetDeadbands(ItemResult[] items)
        {
            if ((items != null) && (items.Length != 0))
            {
                try
                {
                    int[] phServer = new int[items.Length];
                    float[] pPercentDeadband = new float[items.Length];
                    for (int i = 0; i < items.Length; i++)
                    {
                        phServer[i] = System.Convert.ToInt32(items[i].ServerHandle);
                        pPercentDeadband[i] = items[i].Deadband;
                    }
                    IntPtr zero = IntPtr.Zero;
                    ((IOPCItemDeadbandMgt) this.m_group).SetItemDeadband(phServer.Length, phServer, pPercentDeadband, out zero);
                    int[] numArray3 = OpcCom.Interop.GetInt32s(ref zero, phServer.Length, true);
                    for (int j = 0; j < numArray3.Length; j++)
                    {
                        if (OpcCom.Interop.GetResultID(numArray3[j]).Failed())
                        {
                            items[j].Deadband = 0f;
                            items[j].DeadbandSpecified = false;
                        }
                    }
                }
                catch
                {
                    for (int k = 0; k < items.Length; k++)
                    {
                        items[k].Deadband = 0f;
                        items[k].DeadbandSpecified = false;
                    }
                }
            }
        }

        private void SetEnableBuffering(ItemResult[] items)
        {
            if ((items != null) && (items.Length != 0))
            {
                ArrayList list = new ArrayList();
                foreach (ItemResult result in items)
                {
                    if (result.ResultID.Succeeded())
                    {
                        list.Add(result);
                    }
                }
                if (list.Count != 0)
                {
                    try
                    {
                        int[] phServer = new int[list.Count];
                        int[] pbEnable = new int[list.Count];
                        for (int i = 0; i < list.Count; i++)
                        {
                            ItemResult result2 = (ItemResult) list[i];
                            phServer[i] = System.Convert.ToInt32(result2.ServerHandle);
                            pbEnable[i] = (result2.EnableBufferingSpecified && result2.EnableBuffering) ? 1 : 0;
                        }
                        IntPtr zero = IntPtr.Zero;
                        ((IOPCItemSamplingMgt) this.m_group).SetItemBufferEnable(phServer.Length, phServer, pbEnable, out zero);
                        int[] numArray3 = OpcCom.Interop.GetInt32s(ref zero, phServer.Length, true);
                        for (int j = 0; j < numArray3.Length; j++)
                        {
                            ItemResult result3 = (ItemResult) list[j];
                            if (OpcCom.Interop.GetResultID(numArray3[j]).Failed())
                            {
                                result3.EnableBuffering = false;
                                result3.EnableBufferingSpecified = true;
                            }
                        }
                    }
                    catch
                    {
                        foreach (ItemResult result4 in list)
                        {
                            result4.EnableBuffering = false;
                            result4.EnableBufferingSpecified = true;
                        }
                    }
                }
            }
        }

        public virtual void SetEnabled(bool enabled)
        {
            lock (this)
            {
                if (this.m_group == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    ((IOPCAsyncIO3) this.m_group).SetEnable(enabled ? 1 : 0);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCAsyncIO3.SetEnable", exception);
                }
            }
        }

        private void SetReqTypes(ItemResult[] items)
        {
            if ((items != null) && (items.Length != 0))
            {
                ArrayList list = new ArrayList();
                foreach (ItemResult result in items)
                {
                    if (result.ResultID.Succeeded() && (result.ReqType != null))
                    {
                        list.Add(result);
                    }
                }
                if (list.Count != 0)
                {
                    try
                    {
                        int[] phServer = new int[list.Count];
                        short[] pRequestedDatatypes = new short[list.Count];
                        for (int i = 0; i < list.Count; i++)
                        {
                            ItemResult result2 = (ItemResult) list[i];
                            phServer[i] = System.Convert.ToInt32(result2.ServerHandle);
                            pRequestedDatatypes[i] = (short) OpcCom.Interop.GetType(result2.ReqType);
                        }
                        IntPtr zero = IntPtr.Zero;
                        ((IOPCItemMgt) this.m_group).SetDatatypes(list.Count, phServer, pRequestedDatatypes, out zero);
                        int[] numArray3 = OpcCom.Interop.GetInt32s(ref zero, phServer.Length, true);
                        for (int j = 0; j < numArray3.Length; j++)
                        {
                            if (OpcCom.Interop.GetResultID(numArray3[j]).Failed())
                            {
                                ItemResult result3 = (ItemResult) list[j];
                                result3.ResultID = ResultID.Da.E_BADTYPE;
                                result3.DiagnosticInfo = null;
                            }
                        }
                    }
                    catch
                    {
                        for (int k = 0; k < list.Count; k++)
                        {
                            ItemResult result4 = (ItemResult) list[k];
                            result4.ResultID = ResultID.Da.E_BADTYPE;
                            result4.DiagnosticInfo = null;
                        }
                    }
                }
            }
        }

        public void SetResultFilters(int filters)
        {
            lock (this)
            {
                this.m_filters = filters;
                this.m_callback.SetFilters(this.m_handle, this.m_filters);
            }
        }

        private void SetSamplingRates(ItemResult[] items)
        {
            if ((items != null) && (items.Length != 0))
            {
                try
                {
                    int[] phServer = new int[items.Length];
                    int[] pdwRequestedSamplingRate = new int[items.Length];
                    for (int i = 0; i < items.Length; i++)
                    {
                        phServer[i] = System.Convert.ToInt32(items[i].ServerHandle);
                        pdwRequestedSamplingRate[i] = items[i].SamplingRate;
                    }
                    IntPtr zero = IntPtr.Zero;
                    IntPtr ppErrors = IntPtr.Zero;
                    ((IOPCItemSamplingMgt) this.m_group).SetItemSamplingRate(phServer.Length, phServer, pdwRequestedSamplingRate, out zero, out ppErrors);
                    int[] numArray3 = OpcCom.Interop.GetInt32s(ref zero, phServer.Length, true);
                    int[] numArray4 = OpcCom.Interop.GetInt32s(ref ppErrors, phServer.Length, true);
                    for (int j = 0; j < numArray4.Length; j++)
                    {
                        if (items[j].SamplingRate != numArray3[j])
                        {
                            items[j].SamplingRate = numArray3[j];
                            items[j].SamplingRateSpecified = true;
                        }
                        else if (OpcCom.Interop.GetResultID(numArray4[j]).Failed())
                        {
                            items[j].SamplingRate = 0;
                            items[j].SamplingRateSpecified = false;
                        }
                    }
                }
                catch
                {
                    for (int k = 0; k < items.Length; k++)
                    {
                        items[k].SamplingRate = 0;
                        items[k].SamplingRateSpecified = false;
                    }
                }
            }
        }

        private void Unadvise()
        {
            if ((this.m_connection != null) && (this.m_connection.Unadvise() == 0))
            {
                this.m_connection.Dispose();
                this.m_connection = null;
            }
        }

        private void UpdateActive(ItemResult[] items)
        {
            if ((items != null) && (items.Length != 0))
            {
                ArrayList list = new ArrayList();
                ArrayList list2 = new ArrayList();
                foreach (ItemResult result in items)
                {
                    if (result.ResultID.Succeeded() && result.ActiveSpecified)
                    {
                        if (result.Active)
                        {
                            list.Add(result);
                        }
                        else
                        {
                            list2.Add(result);
                        }
                    }
                }
                this.SetActive((ItemResult[]) list.ToArray(typeof(ItemResult)), true);
                this.SetActive((ItemResult[]) list2.ToArray(typeof(ItemResult)), false);
            }
        }

        private void UpdateDeadbands(ItemResult[] items)
        {
            if ((items != null) && (items.Length != 0))
            {
                ArrayList list = new ArrayList();
                ArrayList list2 = new ArrayList();
                foreach (ItemResult result in items)
                {
                    if (result.ResultID.Succeeded())
                    {
                        if (result.DeadbandSpecified)
                        {
                            list.Add(result);
                        }
                        else
                        {
                            list2.Add(result);
                        }
                    }
                }
                this.SetDeadbands((ItemResult[]) list.ToArray(typeof(ItemResult)));
                this.ClearDeadbands((ItemResult[]) list2.ToArray(typeof(ItemResult)));
            }
        }

        private void UpdateSamplingRates(ItemResult[] items)
        {
            if ((items != null) && (items.Length != 0))
            {
                ArrayList list = new ArrayList();
                ArrayList list2 = new ArrayList();
                foreach (ItemResult result in items)
                {
                    if (result.ResultID.Succeeded())
                    {
                        if (result.SamplingRateSpecified)
                        {
                            list.Add(result);
                        }
                        else
                        {
                            list2.Add(result);
                        }
                    }
                }
                this.SetSamplingRates((ItemResult[]) list.ToArray(typeof(ItemResult)));
                this.ClearSamplingRates((ItemResult[]) list2.ToArray(typeof(ItemResult)));
            }
        }

        public IdentifiedResult[] Write(ItemValue[] items)
        {
            IdentifiedResult[] resultArray2;
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (items.Length == 0)
            {
                return new IdentifiedResult[0];
            }
            lock (this)
            {
                ItemTable table;
                if (this.m_group == null)
                {
                    throw new NotConnectedException();
                }
                ItemIdentifier[] itemIDs = null;
                lock ((table = this.m_items))
                {
                    itemIDs = this.m_items.GetItemIDs(items);
                }
                IdentifiedResult[] results = this.Write(itemIDs, items);
                lock ((table = this.m_items))
                {
                    resultArray2 = (IdentifiedResult[]) this.m_items.ApplyFilters(this.m_filters, results);
                }
            }
            return resultArray2;
        }

        protected virtual IdentifiedResult[] Write(ItemIdentifier[] itemIDs, ItemValue[] items)
        {
            IdentifiedResult[] resultArray2;
            try
            {
                int[] phServer = new int[itemIDs.Length];
                for (int i = 0; i < itemIDs.Length; i++)
                {
                    phServer[i] = (int) itemIDs[i].ServerHandle;
                }
                OPCITEMVQT[] oPCITEMVQTs = OpcCom.Da.Interop.GetOPCITEMVQTs(items);
                IntPtr zero = IntPtr.Zero;
                ((IOPCSyncIO2) this.m_group).WriteVQT(itemIDs.Length, phServer, oPCITEMVQTs, out zero);
                int[] numArray2 = OpcCom.Interop.GetInt32s(ref zero, itemIDs.Length, true);
                IdentifiedResult[] resultArray = new IdentifiedResult[itemIDs.Length];
                for (int j = 0; j < itemIDs.Length; j++)
                {
                    resultArray[j] = new IdentifiedResult(itemIDs[j]);
                    resultArray[j].ResultID = OpcCom.Interop.GetResultID(numArray2[j]);
                    resultArray[j].DiagnosticInfo = null;
                    if (numArray2[j] == -1073479674)
                    {
                        resultArray[j].ResultID = new ResultID(ResultID.Da.E_READONLY, -1073479674L);
                    }
                }
                resultArray2 = resultArray;
            }
            catch (Exception exception)
            {
                throw OpcCom.Interop.CreateException("IOPCSyncIO2.WriteVQT", exception);
            }
            return resultArray2;
        }

        public IdentifiedResult[] Write(ItemValue[] items, object requestHandle, WriteCompleteEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            request = null;
            if (items.Length == 0)
            {
                return new IdentifiedResult[0];
            }
            lock (this)
            {
                ItemTable table;
                if (this.m_group == null)
                {
                    throw new NotConnectedException();
                }
                if (this.m_connection == null)
                {
                    this.Advise();
                }
                ItemIdentifier[] itemIDs = null;
                lock ((table = this.m_items))
                {
                    itemIDs = this.m_items.GetItemIDs(items);
                }
                OpcCom.Da.Request request2 = new OpcCom.Da.Request(this, requestHandle, this.m_filters, this.m_counter++, callback);
                this.m_callback.BeginRequest(request2);
                request = request2;
                IdentifiedResult[] results = null;
                int cancelID = 0;
                try
                {
                    results = this.BeginWrite(itemIDs, items, request2.RequestID, out cancelID);
                }
                catch (Exception exception)
                {
                    this.m_callback.EndRequest(request2);
                    throw exception;
                }
                lock ((table = this.m_items))
                {
                    this.m_items.ApplyFilters(this.m_filters | 4, results);
                }
                lock (request)
                {
                    if (request2.BeginWrite(cancelID, results))
                    {
                        this.m_callback.EndRequest(request2);
                        request = null;
                    }
                }
                return results;
            }
        }

        private class Callback : IOPCDataCallback
        {
            private int m_filters = 9;
            private object m_handle = null;
            private OpcCom.Da.Subscription.ItemTable m_items = null;
            private Hashtable m_requests = new Hashtable();

            public event DataChangedEventHandler DataChanged
            {
                add
                {
                    lock (this)
                    {
                        this.m_dataChanged = (DataChangedEventHandler) Delegate.Combine(this.m_dataChanged, value);
                    }
                }
                remove
                {
                    lock (this)
                    {
                        this.m_dataChanged = (DataChangedEventHandler) Delegate.Remove(this.m_dataChanged, value);
                    }
                }
            }

            private event DataChangedEventHandler m_dataChanged;

            public Callback(object handle, int filters, OpcCom.Da.Subscription.ItemTable items)
            {
                this.m_handle = handle;
                this.m_filters = filters;
                this.m_items = items;
            }

            public void BeginRequest(OpcCom.Da.Request request)
            {
                lock (this)
                {
                    this.m_requests[request.RequestID] = request;
                }
            }

            public bool CancelRequest(OpcCom.Da.Request request)
            {
                lock (this)
                {
                    return this.m_requests.ContainsKey(request.RequestID);
                }
            }

            public void EndRequest(OpcCom.Da.Request request)
            {
                lock (this)
                {
                    this.m_requests.Remove(request.RequestID);
                }
            }

            public void OnCancelComplete(int dwTransid, int hGroup)
            {
                try
                {
                    OpcCom.Da.Request request = null;
                    lock (this)
                    {
                        request = (OpcCom.Da.Request) this.m_requests[dwTransid];
                        if (request == null)
                        {
                            return;
                        }
                        this.m_requests.Remove(dwTransid);
                    }
                    lock (request)
                    {
                        request.EndRequest();
                    }
                }
                catch (Exception exception)
                {
                    string stackTrace = exception.StackTrace;
                }
            }

            public void OnDataChange(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
            {
                try
                {
                    OpcCom.Da.Request request = null;
                    lock (this)
                    {
                        if (dwTransid != 0)
                        {
                            request = (OpcCom.Da.Request) this.m_requests[dwTransid];
                            if (request != null)
                            {
                                this.m_requests.Remove(dwTransid);
                            }
                        }
                        if (this.m_dataChanged != null)
                        {
                            ItemValueResult[] results = this.UnmarshalValues(dwCount, phClientItems, pvValues, pwQualities, pftTimeStamps, pErrors);
                            lock (this.m_items)
                            {
                                this.m_items.ApplyFilters(this.m_filters | 4, results);
                            }
                            this.m_dataChanged(this.m_handle, (request != null) ? request.Handle : null, results);
                        }
                    }
                }
                catch (Exception exception)
                {
                    string stackTrace = exception.StackTrace;
                }
            }

            public void OnReadComplete(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
            {
                try
                {
                    OpcCom.Da.Request request = null;
                    ItemValueResult[] results = null;
                    lock (this)
                    {
                        request = (OpcCom.Da.Request) this.m_requests[dwTransid];
                        if (request == null)
                        {
                            return;
                        }
                        this.m_requests.Remove(dwTransid);
                        results = this.UnmarshalValues(dwCount, phClientItems, pvValues, pwQualities, pftTimeStamps, pErrors);
                    }
                    lock (request)
                    {
                        request.EndRequest(results);
                    }
                }
                catch (Exception exception)
                {
                    string stackTrace = exception.StackTrace;
                }
            }

            public void OnWriteComplete(int dwTransid, int hGroup, int hrMastererror, int dwCount, int[] phClientItems, int[] pErrors)
            {
                try
                {
                    OpcCom.Da.Request request = null;
                    IdentifiedResult[] callbackResults = null;
                    lock (this)
                    {
                        request = (OpcCom.Da.Request) this.m_requests[dwTransid];
                        if (request == null)
                        {
                            return;
                        }
                        this.m_requests.Remove(dwTransid);
                        callbackResults = new IdentifiedResult[dwCount];
                        for (int i = 0; i < callbackResults.Length; i++)
                        {
                            ItemIdentifier item = this.m_items[phClientItems[i]];
                            callbackResults[i] = new IdentifiedResult(item);
                            callbackResults[i].ClientHandle = phClientItems[i];
                            callbackResults[i].ResultID = OpcCom.Interop.GetResultID(pErrors[i]);
                            callbackResults[i].DiagnosticInfo = null;
                            if (pErrors[i] == -1073479674)
                            {
                                callbackResults[i].ResultID = new ResultID(ResultID.Da.E_READONLY, -1073479674L);
                            }
                        }
                    }
                    lock (request)
                    {
                        request.EndRequest(callbackResults);
                    }
                }
                catch (Exception exception)
                {
                    string stackTrace = exception.StackTrace;
                }
            }

            public void SetFilters(object handle, int filters)
            {
                lock (this)
                {
                    this.m_handle = handle;
                    this.m_filters = filters;
                }
            }

            private ItemValueResult[] UnmarshalValues(int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
            {
                ItemValueResult[] resultArray = new ItemValueResult[dwCount];
                for (int i = 0; i < resultArray.Length; i++)
                {
                    ItemIdentifier item = this.m_items[phClientItems[i]];
                    resultArray[i] = new ItemValueResult(item);
                    resultArray[i].ClientHandle = phClientItems[i];
                    resultArray[i].Value = pvValues[i];
                    resultArray[i].Quality = new Quality(pwQualities[i]);
                    resultArray[i].QualitySpecified = true;
                    resultArray[i].Timestamp = OpcCom.Interop.GetFILETIME(OpcCom.Da.Interop.Convert(pftTimeStamps[i]));
                    resultArray[i].TimestampSpecified = resultArray[i].Timestamp != DateTime.MinValue;
                    resultArray[i].ResultID = OpcCom.Interop.GetResultID(pErrors[i]);
                    resultArray[i].DiagnosticInfo = null;
                    if (pErrors[i] == -1073479674)
                    {
                        resultArray[i].ResultID = new ResultID(ResultID.Da.E_WRITEONLY, -1073479674L);
                    }
                }
                return resultArray;
            }
        }

        private class ItemTable
        {
            private Hashtable m_items = new Hashtable();

            public ItemIdentifier[] ApplyFilters(int filters, ItemIdentifier[] results)
            {
                if (results == null)
                {
                    return null;
                }
                foreach (ItemIdentifier identifier in results)
                {
                    ItemIdentifier identifier2 = this[identifier.ClientHandle];
                    if (identifier2 != null)
                    {
                        identifier.ItemName = ((filters & 1) != 0) ? identifier2.ItemName : null;
                        identifier.ItemPath = ((filters & 2) != 0) ? identifier2.ItemPath : null;
                        identifier.ServerHandle = identifier.ClientHandle;
                        identifier.ClientHandle = ((filters & 4) != 0) ? identifier2.ClientHandle : null;
                    }
                    if (((filters & 8) == 0) && (identifier.GetType() == typeof(ItemValueResult)))
                    {
                        ((ItemValueResult) identifier).Timestamp = DateTime.MinValue;
                        ((ItemValueResult) identifier).TimestampSpecified = false;
                    }
                }
                return results;
            }

            public ItemResult[] CreateItems(Item[] items)
            {
                if (items == null)
                {
                    return null;
                }
                ItemResult[] resultArray = new ItemResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    resultArray[i] = new ItemResult(items[i]);
                    ItemIdentifier identifier = this[items[i].ServerHandle];
                    if (identifier != null)
                    {
                        resultArray[i].ItemName = identifier.ItemName;
                        resultArray[i].ItemPath = identifier.ItemName;
                        resultArray[i].ServerHandle = identifier.ServerHandle;
                        identifier.ClientHandle = items[i].ClientHandle;
                    }
                    if (resultArray[i].ServerHandle == null)
                    {
                        resultArray[i].ResultID = ResultID.Da.E_INVALIDHANDLE;
                        resultArray[i].DiagnosticInfo = null;
                    }
                    else
                    {
                        resultArray[i].ClientHandle = items[i].ServerHandle;
                    }
                }
                return resultArray;
            }

            private int GetInvalidHandle()
            {
                int num = 0;
                foreach (ItemIdentifier identifier in this.m_items.Values)
                {
                    if (((identifier.ServerHandle != null) && (identifier.ServerHandle.GetType() == typeof(int))) && (num < ((int) identifier.ServerHandle)))
                    {
                        num = ((int) identifier.ServerHandle) + 1;
                    }
                }
                return num;
            }

            public ItemIdentifier[] GetItemIDs(ItemIdentifier[] items)
            {
                int invalidHandle = this.GetInvalidHandle();
                ItemIdentifier[] identifierArray = new ItemIdentifier[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    ItemIdentifier identifier = this[items[i].ServerHandle];
                    if (identifier != null)
                    {
                        identifierArray[i] = (ItemIdentifier) identifier.Clone();
                    }
                    else
                    {
                        identifierArray[i] = new ItemIdentifier();
                        identifierArray[i].ServerHandle = invalidHandle;
                    }
                    identifierArray[i].ClientHandle = items[i].ServerHandle;
                }
                return identifierArray;
            }

            public ItemIdentifier this[object handle]
            {
                get
                {
                    if (handle != null)
                    {
                        return (ItemIdentifier) this.m_items[handle];
                    }
                    return null;
                }
                set
                {
                    if (handle != null)
                    {
                        if (value == null)
                        {
                            this.m_items.Remove(handle);
                        }
                        else
                        {
                            this.m_items[handle] = value;
                        }
                    }
                }
            }
        }
    }
}

