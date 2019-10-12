namespace Jund.OpcHelper.OpcCom.Da.Wrapper
{
    using Opc;
    using Opc.Da;
    using OpcCom;
    using OpcCom.Da;
    using OpcRcw.Da;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    [CLSCompliant(false)]
    public class Group : ConnectionPointContainer, IDisposable, IOPCItemMgt, IOPCSyncIO2, IOPCSyncIO, IOPCAsyncIO3, IOPCAsyncIO2, IOPCGroupStateMgt2, IOPCGroupStateMgt, IOPCItemDeadbandMgt, IOPCItemSamplingMgt
    {
        private const int LOCALE_SYSTEM_DEFAULT = 0x800;
        private int m_clientHandle = 0;
        private DataChangedEventHandler m_dataChanged = null;
        private Hashtable m_items = new Hashtable();
        private int m_lcid = 0x800;
        private string m_name = null;
        private int m_nextHandle = 0x3e8;
        private Hashtable m_requests = new Hashtable();
        private OpcCom.Da.Wrapper.Server m_server = null;
        private int m_serverHandle = 0;
        private ISubscription m_subscription = null;
        private int m_timebias = 0;

        public Group(OpcCom.Da.Wrapper.Server server, string name, int handle, int lcid, int timebias, ISubscription subscription)
        {
            base.RegisterInterface(typeof(IOPCDataCallback).GUID);
            this.m_server = server;
            this.m_name = name;
            this.m_serverHandle = handle;
            this.m_lcid = lcid;
            this.m_timebias = timebias;
            this.m_subscription = subscription;
        }

        private void AddItems(Item[] items)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if (items == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                ItemResult[] resultArray = this.m_subscription.AddItems(items);
                if ((resultArray == null) || (resultArray.Length != items.Length))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                for (int i = 0; i < resultArray.Length; i++)
                {
                    if (resultArray[i].ResultID.Succeeded())
                    {
                        this.m_items[++this.m_nextHandle] = resultArray[i];
                    }
                }
            }
        }

        public void AddItems(int dwCount, OPCITEMDEF[] pItemArray, out IntPtr ppAddResults, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if ((dwCount == 0) || (pItemArray == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    Item[] items = new Item[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new Item();
                        items[i].ItemName = pItemArray[i].szItemID;
                        items[i].ItemPath = pItemArray[i].szAccessPath;
                        items[i].ClientHandle = pItemArray[i].hClient;
                        items[i].ServerHandle = null;
                        items[i].Active = pItemArray[i].bActive != 0;
                        items[i].ActiveSpecified = true;
                        items[i].ReqType = OpcCom.Interop.GetType((VarEnum) pItemArray[i].vtRequestedDataType);
                    }
                    ItemResult[] results = this.m_subscription.AddItems(items);
                    if ((results == null) || (results.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    PropertyID[] propertyIDs = new PropertyID[] { Property.DATATYPE, Property.ACCESSRIGHTS };
                    ItemPropertyCollection[] propertysArray = this.m_server.IServer.GetProperties(items, propertyIDs, true);
                    ppAddResults = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(OPCITEMRESULT)) * results.Length);
                    IntPtr ptr = ppAddResults;
                    for (int j = 0; j < results.Length; j++)
                    {
                        OPCITEMRESULT structure = new OPCITEMRESULT {
                            hServer = 0,
                            dwBlobSize = 0,
                            pBlob = IntPtr.Zero,
                            vtCanonicalDataType = 0,
                            dwAccessRights = 0,
                            wReserved = 0
                        };
                        if (results[j].ResultID.Succeeded())
                        {
                            structure.hServer = ++this.m_nextHandle;
                            structure.vtCanonicalDataType = (short) OpcCom.Da.Interop.MarshalPropertyValue(Property.DATATYPE, propertysArray[j][0].Value);
                            structure.dwAccessRights = (int) OpcCom.Da.Interop.MarshalPropertyValue(Property.ACCESSRIGHTS, propertysArray[j][1].Value);
                            this.m_items[this.m_nextHandle] = results[j];
                        }
                        Marshal.StructureToPtr(structure, ptr, false);
                        ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(OPCITEMRESULT)));
                    }
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(results);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        private int AssignHandle()
        {
            return ++this.m_nextHandle;
        }

        public void Cancel2(int dwCancelID)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if (!base.IsConnected(typeof(IOPCDataCallback).GUID))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147220992);
                }
                try
                {
                    IDictionaryEnumerator enumerator = this.m_requests.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        Opc.Da.Request key = (Opc.Da.Request) enumerator.Key;
                        if (key.Handle.Equals(dwCancelID))
                        {
                            this.m_requests.Remove(key);
                            goto Label_009A;
                        }
                    }
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            Label_009A:;
            }
        }

        public void ClearItemDeadband(int dwCount, int[] phServer, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if ((dwCount == 0) || (phServer == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    ArrayList list = new ArrayList();
                    Item[] items = new Item[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        Item item = (Item) this.m_items[phServer[i]];
                        items[i] = new Item(item);
                        if (item != null)
                        {
                            if (item.DeadbandSpecified)
                            {
                                items[i].Deadband = 0f;
                                items[i].DeadbandSpecified = false;
                            }
                            else
                            {
                                list.Add(i);
                            }
                        }
                    }
                    ItemResult[] results = this.m_subscription.ModifyItems(0x80, items);
                    if ((results == null) || (results.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    foreach (int num2 in list)
                    {
                        if (results[num2].ResultID.Succeeded())
                        {
                            results[num2].ResultID = new ResultID(-1073478656L);
                        }
                    }
                    for (int j = 0; j < dwCount; j++)
                    {
                        if (results[j].ResultID.Succeeded())
                        {
                            this.m_items[phServer[j]] = results[j];
                        }
                    }
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(results);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void ClearItemSamplingRate(int dwCount, int[] phServer, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if ((dwCount == 0) || (phServer == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    Item[] items = new Item[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new Item((ItemIdentifier) this.m_items[phServer[i]]);
                        items[i].SamplingRate = 0;
                        items[i].SamplingRateSpecified = false;
                    }
                    ItemResult[] results = this.m_subscription.ModifyItems(0x100, items);
                    if ((results == null) || (results.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    for (int j = 0; j < dwCount; j++)
                    {
                        if (results[j].ResultID.Succeeded())
                        {
                            this.m_items[phServer[j]] = results[j];
                        }
                    }
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(results);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void CloneGroup(string szName, ref Guid riid, out object ppUnk)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                Group group = null;
                try
                {
                    SubscriptionState state = this.m_subscription.GetState();
                    state.Name = szName;
                    state.Active = false;
                    group = this.m_server.CreateGroup(ref state, this.m_lcid, this.m_timebias);
                    Item[] items = new Item[this.m_items.Count];
                    int num = 0;
                    foreach (Item item in this.m_items.Values)
                    {
                        items[num++] = item;
                    }
                    group.AddItems(items);
                    ppUnk = group;
                }
                catch (Exception exception)
                {
                    if (group != null)
                    {
                        try
                        {
                            this.m_server.RemoveGroup(group.ServerHandle, 0);
                        }
                        catch
                        {
                        }
                    }
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void CreateEnumerator(ref Guid riid, out object ppUnk)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if (riid != typeof(IEnumOPCItemAttributes).GUID)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    int[] numArray = new int[this.m_items.Count];
                    Item[] itemIDs = new Item[this.m_items.Count];
                    int index = 0;
                    IDictionaryEnumerator enumerator = this.m_items.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        numArray[index] = (int) enumerator.Key;
                        itemIDs[index] = (Item) enumerator.Value;
                        index++;
                    }
                    PropertyID[] propertyIDs = new PropertyID[] { Property.ACCESSRIGHTS, Property.DATATYPE, Property.EUTYPE, Property.EUINFO, Property.HIGHEU, Property.LOWEU };
                    ItemPropertyCollection[] propertysArray = this.m_server.IServer.GetProperties(itemIDs, propertyIDs, true);
                    EnumOPCItemAttributes.ItemAttributes[] items = new EnumOPCItemAttributes.ItemAttributes[this.m_items.Count];
                    for (int i = 0; i < itemIDs.Length; i++)
                    {
                        items[i] = new EnumOPCItemAttributes.ItemAttributes();
                        items[i].ItemID = itemIDs[i].ItemName;
                        items[i].AccessPath = itemIDs[i].ItemPath;
                        items[i].ClientHandle = (int) itemIDs[i].ClientHandle;
                        items[i].ServerHandle = numArray[i];
                        items[i].Active = itemIDs[i].Active;
                        items[i].RequestedDataType = itemIDs[i].ReqType;
                        items[i].AccessRights = (accessRights) propertysArray[i][0].Value;
                        items[i].CanonicalDataType = (System.Type) propertysArray[i][1].Value;
                        items[i].EuType = (euType) propertysArray[i][2].Value;
                        items[i].EuInfo = (string[]) propertysArray[i][3].Value;
                        if (items[i].EuType == euType.analog)
                        {
                            items[i].MaxValue = (double) propertysArray[i][4].Value;
                            items[i].MinValue = (double) propertysArray[i][5].Value;
                        }
                    }
                    ppUnk = new EnumOPCItemAttributes(items);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void Dispose()
        {
            lock (this)
            {
                if (this.m_subscription != null)
                {
                    this.m_subscription.DataChanged -= this.m_dataChanged;
                    this.m_server.IServer.CancelSubscription(this.m_subscription);
                    this.m_subscription = null;
                }
            }
        }

        public void GetEnable(out int pbEnable)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if (!base.IsConnected(typeof(IOPCDataCallback).GUID))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147220992);
                }
                try
                {
                    pbEnable = this.m_subscription.GetEnabled() ? 1 : 0;
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void GetItemBufferEnable(int dwCount, int[] phServer, out IntPtr ppbEnable, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if ((dwCount == 0) || (phServer == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    int[] source = new int[dwCount];
                    int[] numArray2 = new int[dwCount];
                    for (int i = 0; i < dwCount; i++)
                    {
                        ItemResult result = (ItemResult) this.m_items[phServer[i]];
                        numArray2[i] = -1073479679;
                        if ((result != null) && result.ResultID.Succeeded())
                        {
                            source[i] = (result.EnableBuffering && result.EnableBufferingSpecified) ? 1 : 0;
                            numArray2[i] = 0;
                        }
                    }
                    ppbEnable = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * dwCount);
                    Marshal.Copy(source, 0, ppbEnable, dwCount);
                    ppErrors = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * dwCount);
                    Marshal.Copy(numArray2, 0, ppErrors, dwCount);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void GetItemDeadband(int dwCount, int[] phServer, out IntPtr ppPercentDeadband, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if ((dwCount == 0) || (phServer == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    float[] source = new float[dwCount];
                    int[] numArray2 = new int[dwCount];
                    for (int i = 0; i < dwCount; i++)
                    {
                        ItemResult result = (ItemResult) this.m_items[phServer[i]];
                        numArray2[i] = -1073479679;
                        if ((result != null) && result.ResultID.Succeeded())
                        {
                            if (result.DeadbandSpecified)
                            {
                                source[i] = result.Deadband;
                                numArray2[i] = 0;
                            }
                            else
                            {
                                numArray2[i] = -1073478656;
                            }
                        }
                    }
                    ppPercentDeadband = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(float)) * dwCount);
                    Marshal.Copy(source, 0, ppPercentDeadband, dwCount);
                    ppErrors = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * dwCount);
                    Marshal.Copy(numArray2, 0, ppErrors, dwCount);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void GetItemSamplingRate(int dwCount, int[] phServer, out IntPtr ppdwSamplingRate, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if ((dwCount == 0) || (phServer == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    int[] source = new int[dwCount];
                    int[] numArray2 = new int[dwCount];
                    for (int i = 0; i < dwCount; i++)
                    {
                        ItemResult result = (ItemResult) this.m_items[phServer[i]];
                        numArray2[i] = -1073479679;
                        if ((result != null) && result.ResultID.Succeeded())
                        {
                            if (result.SamplingRateSpecified)
                            {
                                source[i] = result.SamplingRate;
                                numArray2[i] = 0;
                            }
                            else
                            {
                                numArray2[i] = -1073478651;
                            }
                        }
                    }
                    ppdwSamplingRate = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * dwCount);
                    Marshal.Copy(source, 0, ppdwSamplingRate, dwCount);
                    ppErrors = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * dwCount);
                    Marshal.Copy(numArray2, 0, ppErrors, dwCount);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void GetKeepAlive(out int pdwKeepAliveTime)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                try
                {
                    SubscriptionState state = this.m_subscription.GetState();
                    if (state == null)
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    pdwKeepAliveTime = state.KeepAlive;
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void GetState(out int pUpdateRate, out int pActive, out string ppName, out int pTimeBias, out float pPercentDeadband, out int pLCID, out int phClientGroup, out int phServerGroup)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                try
                {
                    SubscriptionState state = this.m_subscription.GetState();
                    if (state == null)
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    pUpdateRate = state.UpdateRate;
                    pActive = state.Active ? 1 : 0;
                    ppName = state.Name;
                    pTimeBias = this.m_timebias;
                    pPercentDeadband = state.Deadband;
                    pLCID = this.m_lcid;
                    phClientGroup = this.m_clientHandle = (int) state.ClientHandle;
                    phServerGroup = this.m_serverHandle;
                    this.m_name = state.Name;
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        private void InvokeCallback(object requestHandle, ItemValueResult[] results, bool dataChanged)
        {
            try
            {
                object callback = null;
                int dwTransid = 0;
                int hGroup = 0;
                int hrMastererror = 0;
                int hrMasterquality = 0;
                int[] phClientItems = null;
                object[] pvValues = null;
                short[] pwQualities = null;
                OpcRcw.Da.FILETIME[] pftTimeStamps = null;
                int[] pErrors = null;
                lock (this)
                {
                    bool flag = false;
                    IDictionaryEnumerator enumerator = this.m_requests.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        Opc.Da.Request key = (Opc.Da.Request) enumerator.Key;
                        if (key.Handle.Equals(requestHandle))
                        {
                            dwTransid = (int) enumerator.Value;
                            this.m_requests.Remove(key);
                            flag = true;
                            break;
                        }
                    }
                    if (!dataChanged && !flag)
                    {
                        return;
                    }
                    callback = base.GetCallback(typeof(IOPCDataCallback).GUID);
                    if (callback == null)
                    {
                        return;
                    }
                    hGroup = this.m_clientHandle;
                    if (results != null)
                    {
                        phClientItems = new int[results.Length];
                        pvValues = new object[results.Length];
                        pwQualities = new short[results.Length];
                        pftTimeStamps = new OpcRcw.Da.FILETIME[results.Length];
                        pErrors = new int[results.Length];
                        for (int i = 0; i < results.Length; i++)
                        {
                            phClientItems[i] = (int) results[i].ClientHandle;
                            pvValues[i] = results[i].Value;
                            pwQualities[i] = results[i].QualitySpecified ? results[i].Quality.GetCode() : ((short) 0);
                            pftTimeStamps[i] = OpcCom.Da.Interop.Convert(OpcCom.Interop.GetFILETIME(results[i].Timestamp));
                            pErrors[i] = OpcCom.Interop.GetResultID(results[i].ResultID);
                            if (results[i].Quality.QualityBits != qualityBits.good)
                            {
                                hrMasterquality = 1;
                            }
                            if (results[i].ResultID != ResultID.S_OK)
                            {
                                hrMastererror = 1;
                            }
                        }
                    }
                }
                if (dataChanged)
                {
                    ((IOPCDataCallback) callback).OnDataChange(dwTransid, hGroup, hrMasterquality, hrMastererror, phClientItems.Length, phClientItems, pvValues, pwQualities, pftTimeStamps, pErrors);
                }
                else
                {
                    ((IOPCDataCallback) callback).OnReadComplete(dwTransid, hGroup, hrMasterquality, hrMastererror, phClientItems.Length, phClientItems, pvValues, pwQualities, pftTimeStamps, pErrors);
                }
            }
            catch (Exception exception)
            {
                string message = exception.Message;
            }
        }

        public override void OnAdvise(Guid riid)
        {
            lock (this)
            {
                this.m_dataChanged = new DataChangedEventHandler(this.OnDataChanged);
                this.m_subscription.DataChanged += this.m_dataChanged;
            }
        }

        private void OnDataChanged(object subscriptionHandle, object requestHandle, ItemValueResult[] results)
        {
            this.InvokeCallback(requestHandle, results, true);
        }

        private void OnReadComplete(object requestHandle, ItemValueResult[] results)
        {
            this.InvokeCallback(requestHandle, results, false);
        }

        public override void OnUnadvise(Guid riid)
        {
            lock (this)
            {
                if (this.m_dataChanged != null)
                {
                    this.m_subscription.DataChanged -= this.m_dataChanged;
                    this.m_dataChanged = null;
                }
            }
        }

        private void OnWriteComplete(object clientHandle, IdentifiedResult[] results)
        {
            try
            {
                object callback = null;
                int dwTransid = -1;
                int hGroup = -1;
                int hrMastererr = 0;
                int[] pClienthandles = null;
                int[] pErrors = null;
                lock (this)
                {
                    bool flag = false;
                    IDictionaryEnumerator enumerator = this.m_requests.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        Opc.Da.Request key = (Opc.Da.Request) enumerator.Key;
                        if (key.Handle.Equals(clientHandle))
                        {
                            dwTransid = (int) enumerator.Value;
                            this.m_requests.Remove(key);
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        return;
                    }
                    callback = base.GetCallback(typeof(IOPCDataCallback).GUID);
                    if (callback == null)
                    {
                        return;
                    }
                    hGroup = this.m_clientHandle;
                    if (results != null)
                    {
                        pClienthandles = new int[results.Length];
                        pErrors = new int[results.Length];
                        for (int i = 0; i < results.Length; i++)
                        {
                            pClienthandles[i] = (int) results[i].ClientHandle;
                            pErrors[i] = OpcCom.Interop.GetResultID(results[i].ResultID);
                            if (results[i].ResultID != ResultID.S_OK)
                            {
                                hrMastererr = 1;
                            }
                        }
                    }
                }
                ((IOPCDataCallback) callback).OnWriteComplete(dwTransid, hGroup, hrMastererr, pClienthandles.Length, pClienthandles, pErrors);
            }
            catch (Exception exception)
            {
                string message = exception.Message;
            }
        }

        public void Read(OPCDATASOURCE dwSource, int dwCount, int[] phServer, out IntPtr ppItemValues, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if ((dwCount == 0) || (phServer == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    Item[] items = new Item[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new Item((ItemIdentifier) this.m_items[phServer[i]]);
                        items[i].MaxAge = (dwSource == OPCDATASOURCE.OPC_DS_DEVICE) ? 0 : 0x7fffffff;
                        items[i].MaxAgeSpecified = true;
                    }
                    ItemValueResult[] input = this.m_subscription.Read(items);
                    if ((input == null) || (input.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    ppItemValues = OpcCom.Da.Interop.GetItemStates(input);
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(input);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void Read(int dwCount, int[] phServer, int dwTransactionID, out int pdwCancelID, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if ((dwCount == 0) || (phServer == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                if (!base.IsConnected(typeof(IOPCDataCallback).GUID))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147220992);
                }
                try
                {
                    Item[] items = new Item[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new Item((ItemIdentifier) this.m_items[phServer[i]]);
                        items[i].MaxAge = 0;
                        items[i].MaxAgeSpecified = true;
                    }
                    pdwCancelID = this.AssignHandle();
                    IRequest request = null;
                    IdentifiedResult[] results = this.m_subscription.Read(items, (int) pdwCancelID, new ReadCompleteEventHandler(this.OnReadComplete), out request);
                    if ((results == null) || (results.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    if (request != null)
                    {
                        this.m_requests[request] = dwTransactionID;
                    }
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(results);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void ReadMaxAge(int dwCount, int[] phServer, int[] pdwMaxAge, int dwTransactionID, out int pdwCancelID, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if (((dwCount == 0) || (phServer == null)) || (pdwMaxAge == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                if (!base.IsConnected(typeof(IOPCDataCallback).GUID))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147220992);
                }
                try
                {
                    Item[] items = new Item[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new Item((ItemIdentifier) this.m_items[phServer[i]]);
                        items[i].MaxAge = (pdwMaxAge[i] < 0) ? 0x7fffffff : pdwMaxAge[i];
                        items[i].MaxAgeSpecified = true;
                    }
                    pdwCancelID = this.AssignHandle();
                    IRequest request = null;
                    IdentifiedResult[] results = this.m_subscription.Read(items, (int) pdwCancelID, new ReadCompleteEventHandler(this.OnReadComplete), out request);
                    if ((results == null) || (results.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    if (request != null)
                    {
                        this.m_requests[request] = dwTransactionID;
                    }
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(results);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void ReadMaxAge(int dwCount, int[] phServer, int[] pdwMaxAge, out IntPtr ppvValues, out IntPtr ppwQualities, out IntPtr ppftTimeStamps, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if (((dwCount == 0) || (phServer == null)) || (pdwMaxAge == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    Item[] items = new Item[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new Item((ItemIdentifier) this.m_items[phServer[i]]);
                        items[i].MaxAge = (pdwMaxAge[i] < 0) ? 0x7fffffff : pdwMaxAge[i];
                        items[i].MaxAgeSpecified = true;
                    }
                    ItemValueResult[] results = this.m_subscription.Read(items);
                    if ((results == null) || (results.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    object[] values = new object[results.Length];
                    short[] input = new short[results.Length];
                    DateTime[] datetimes = new DateTime[results.Length];
                    for (int j = 0; j < results.Length; j++)
                    {
                        values[j] = results[j].Value;
                        input[j] = results[j].QualitySpecified ? results[j].Quality.GetCode() : ((short) 0);
                        datetimes[j] = results[j].TimestampSpecified ? results[j].Timestamp : DateTime.MinValue;
                    }
                    ppvValues = OpcCom.Interop.GetVARIANTs(values, false);
                    ppwQualities = OpcCom.Interop.GetInt16s(input);
                    ppftTimeStamps = OpcCom.Interop.GetFILETIMEs(datetimes);
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(results);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void Refresh2(OPCDATASOURCE dwSource, int dwTransactionID, out int pdwCancelID)
        {
            lock (this)
            {
                if (!base.IsConnected(typeof(IOPCDataCallback).GUID))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147220992);
                }
                int dwMaxAge = (dwSource == OPCDATASOURCE.OPC_DS_DEVICE) ? 0 : 0x7fffffff;
                this.RefreshMaxAge(dwMaxAge, dwTransactionID, out pdwCancelID);
            }
        }

        public void RefreshMaxAge(int dwMaxAge, int dwTransactionID, out int pdwCancelID)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if (!base.IsConnected(typeof(IOPCDataCallback).GUID))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147220992);
                }
                try
                {
                    pdwCancelID = this.AssignHandle();
                    IRequest request = null;
                    this.m_subscription.Refresh((int) pdwCancelID, out request);
                    if (request != null)
                    {
                        this.m_requests[request] = dwTransactionID;
                    }
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void RemoveItems(int dwCount, int[] phServer, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                int[] numArray1 = (int[]) new ArrayList(this.m_items.Keys).ToArray(typeof(int));
                if ((dwCount == 0) || (phServer == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    ItemIdentifier[] items = new ItemIdentifier[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new ItemIdentifier((ItemIdentifier) this.m_items[phServer[i]]);
                    }
                    IdentifiedResult[] results = this.m_subscription.RemoveItems(items);
                    if ((results == null) || (results.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    for (int j = 0; j < dwCount; j++)
                    {
                        if (results[j].ResultID.Succeeded())
                        {
                            this.m_items.Remove(phServer[j]);
                        }
                    }
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(results);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void SetActiveState(int dwCount, int[] phServer, int bActive, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if ((dwCount == 0) || (phServer == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    Item[] items = new Item[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new Item((ItemIdentifier) this.m_items[phServer[i]]);
                        items[i].Active = bActive != 0;
                        items[i].ActiveSpecified = true;
                    }
                    ItemResult[] results = this.m_subscription.ModifyItems(8, items);
                    if ((results == null) || (results.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    for (int j = 0; j < dwCount; j++)
                    {
                        if (results[j].ResultID.Succeeded())
                        {
                            this.m_items[phServer[j]] = results[j];
                        }
                    }
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(results);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void SetClientHandles(int dwCount, int[] phServer, int[] phClient, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if (((dwCount == 0) || (phServer == null)) || (phClient == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    Item[] items = new Item[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new Item((ItemIdentifier) this.m_items[phServer[i]]);
                        items[i].ClientHandle = phClient[i];
                    }
                    ItemResult[] results = this.m_subscription.ModifyItems(2, items);
                    if ((results == null) || (results.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    for (int j = 0; j < dwCount; j++)
                    {
                        if (results[j].ResultID.Succeeded())
                        {
                            this.m_items[phServer[j]] = results[j];
                        }
                    }
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(results);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void SetDatatypes(int dwCount, int[] phServer, short[] pRequestedDatatypes, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if (((dwCount == 0) || (phServer == null)) || (pRequestedDatatypes == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    Item[] items = new Item[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new Item((ItemIdentifier) this.m_items[phServer[i]]);
                        items[i].ReqType = OpcCom.Interop.GetType((VarEnum) pRequestedDatatypes[i]);
                    }
                    ItemResult[] results = this.m_subscription.ModifyItems(0x40, items);
                    if ((results == null) || (results.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    for (int j = 0; j < dwCount; j++)
                    {
                        if (results[j].ResultID.Succeeded())
                        {
                            this.m_items[phServer[j]] = results[j];
                        }
                    }
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(results);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void SetEnable(int bEnable)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if (!base.IsConnected(typeof(IOPCDataCallback).GUID))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147220992);
                }
                try
                {
                    this.m_subscription.SetEnabled(bEnable != 0);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void SetItemBufferEnable(int dwCount, int[] phServer, int[] pbEnable, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if (((dwCount == 0) || (phServer == null)) || (pbEnable == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    Item[] items = new Item[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new Item((ItemIdentifier) this.m_items[phServer[i]]);
                        items[i].EnableBuffering = pbEnable[i] != 0;
                        items[i].EnableBufferingSpecified = pbEnable[i] != 0;
                    }
                    ItemResult[] results = this.m_subscription.ModifyItems(0x200, items);
                    if ((results == null) || (results.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    for (int j = 0; j < dwCount; j++)
                    {
                        if (results[j].ResultID.Succeeded())
                        {
                            this.m_items[phServer[j]] = results[j];
                        }
                    }
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(results);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void SetItemDeadband(int dwCount, int[] phServer, float[] pPercentDeadband, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if (((dwCount == 0) || (phServer == null)) || (pPercentDeadband == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    Item[] items = new Item[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new Item((ItemIdentifier) this.m_items[phServer[i]]);
                        items[i].Deadband = pPercentDeadband[i];
                        items[i].DeadbandSpecified = true;
                    }
                    ItemResult[] results = this.m_subscription.ModifyItems(0x80, items);
                    if ((results == null) || (results.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    for (int j = 0; j < dwCount; j++)
                    {
                        if (results[j].ResultID.Succeeded())
                        {
                            this.m_items[phServer[j]] = results[j];
                        }
                    }
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(results);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void SetItemSamplingRate(int dwCount, int[] phServer, int[] pdwRequestedSamplingRate, out IntPtr ppdwRevisedSamplingRate, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if (((dwCount == 0) || (phServer == null)) || (pdwRequestedSamplingRate == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    Item[] items = new Item[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new Item((ItemIdentifier) this.m_items[phServer[i]]);
                        items[i].SamplingRate = pdwRequestedSamplingRate[i];
                        items[i].SamplingRateSpecified = true;
                    }
                    ItemResult[] results = this.m_subscription.ModifyItems(0x100, items);
                    if ((results == null) || (results.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    int[] source = new int[dwCount];
                    for (int j = 0; j < dwCount; j++)
                    {
                        if (results[j].ResultID.Succeeded())
                        {
                            this.m_items[phServer[j]] = results[j];
                            source[j] = results[j].SamplingRate;
                        }
                    }
                    ppdwRevisedSamplingRate = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * dwCount);
                    Marshal.Copy(source, 0, ppdwRevisedSamplingRate, dwCount);
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(results);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void SetKeepAlive(int dwKeepAliveTime, out int pdwRevisedKeepAliveTime)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                try
                {
                    SubscriptionState state = new SubscriptionState();
                    if (state == null)
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    state.KeepAlive = dwKeepAliveTime;
                    state = this.m_subscription.ModifyState(0x20, state);
                    pdwRevisedKeepAliveTime = state.KeepAlive;
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void SetName(string szName)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                try
                {
                    SubscriptionState state = this.m_subscription.GetState();
                    if (state == null)
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    int errorCode = this.m_server.SetGroupName(state.Name, szName);
                    if (errorCode != 0)
                    {
                        throw new ExternalException("SetName", errorCode);
                    }
                    this.m_name = state.Name = szName;
                    this.m_subscription.ModifyState(1, state);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void SetState(IntPtr pRequestedUpdateRate, out int pRevisedUpdateRate, IntPtr pActive, IntPtr pTimeBias, IntPtr pPercentDeadband, IntPtr pLCID, IntPtr phClientGroup)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                try
                {
                    SubscriptionState state = new SubscriptionState();
                    if (state == null)
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    int masks = 0;
                    if (pRequestedUpdateRate != IntPtr.Zero)
                    {
                        state.UpdateRate = Marshal.ReadInt32(pRequestedUpdateRate);
                        masks |= 0x10;
                    }
                    if (pActive != IntPtr.Zero)
                    {
                        state.Active = Marshal.ReadInt32(pActive) != 0;
                        masks |= 8;
                    }
                    if (pTimeBias != IntPtr.Zero)
                    {
                        this.m_timebias = Marshal.ReadInt32(pTimeBias);
                    }
                    if (pPercentDeadband != IntPtr.Zero)
                    {
                        float[] destination = new float[1];
                        Marshal.Copy(pPercentDeadband, destination, 0, 1);
                        state.Deadband = destination[0];
                        masks |= 0x80;
                    }
                    if (pLCID != IntPtr.Zero)
                    {
                        this.m_lcid = Marshal.ReadInt32(pLCID);
                        state.Locale = OpcCom.Interop.GetLocale(this.m_lcid);
                        masks |= 4;
                    }
                    if (phClientGroup != IntPtr.Zero)
                    {
                        state.ClientHandle = this.m_clientHandle = Marshal.ReadInt32(phClientGroup);
                        masks |= 2;
                    }
                    state = this.m_subscription.ModifyState(masks, state);
                    pRevisedUpdateRate = state.UpdateRate;
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void ValidateItems(int dwCount, OPCITEMDEF[] pItemArray, int bBlobUpdate, out IntPtr ppValidationResults, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if ((dwCount == 0) || (pItemArray == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    Item[] items = new Item[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new Item();
                        items[i].ItemName = pItemArray[i].szItemID;
                        items[i].ItemPath = pItemArray[i].szAccessPath;
                        items[i].ClientHandle = pItemArray[i].hClient;
                        items[i].ServerHandle = null;
                        items[i].Active = false;
                        items[i].ActiveSpecified = true;
                        items[i].ReqType = OpcCom.Interop.GetType((VarEnum) pItemArray[i].vtRequestedDataType);
                    }
                    ItemResult[] resultArray = this.m_subscription.AddItems(items);
                    if ((resultArray == null) || (resultArray.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    this.m_subscription.RemoveItems(resultArray);
                    PropertyID[] propertyIDs = new PropertyID[] { Property.DATATYPE, Property.ACCESSRIGHTS };
                    ItemPropertyCollection[] propertysArray = this.m_server.IServer.GetProperties(items, propertyIDs, true);
                    ppValidationResults = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(OPCITEMRESULT)) * resultArray.Length);
                    IntPtr ptr = ppValidationResults;
                    for (int j = 0; j < resultArray.Length; j++)
                    {
                        OPCITEMRESULT structure = new OPCITEMRESULT {
                            hServer = 0,
                            dwBlobSize = 0,
                            pBlob = IntPtr.Zero,
                            vtCanonicalDataType = 0,
                            dwAccessRights = 0,
                            wReserved = 0
                        };
                        if (resultArray[j].ResultID.Succeeded())
                        {
                            structure.vtCanonicalDataType = (short) OpcCom.Da.Interop.MarshalPropertyValue(Property.DATATYPE, propertysArray[j][0].Value);
                            structure.dwAccessRights = (int) OpcCom.Da.Interop.MarshalPropertyValue(Property.ACCESSRIGHTS, propertysArray[j][1].Value);
                        }
                        Marshal.StructureToPtr(structure, ptr, false);
                        ptr = (IntPtr) (ptr.ToInt32() + Marshal.SizeOf(typeof(OPCITEMRESULT)));
                    }
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(resultArray);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void Write(int dwCount, int[] phServer, object[] pItemValues, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if (((dwCount == 0) || (phServer == null)) || (pItemValues == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    ItemValue[] items = new ItemValue[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new ItemValue((ItemIdentifier) this.m_items[phServer[i]]);
                        items[i].Value = pItemValues[i];
                        items[i].Quality = Quality.Bad;
                        items[i].QualitySpecified = false;
                        items[i].Timestamp = DateTime.MinValue;
                        items[i].TimestampSpecified = false;
                    }
                    IdentifiedResult[] results = this.m_subscription.Write(items);
                    if ((results == null) || (results.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(results);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void Write(int dwCount, int[] phServer, object[] pItemValues, int dwTransactionID, out int pdwCancelID, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if (((dwCount == 0) || (phServer == null)) || (pItemValues == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                if (!base.IsConnected(typeof(IOPCDataCallback).GUID))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147220992);
                }
                try
                {
                    ItemValue[] items = new ItemValue[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new ItemValue((ItemIdentifier) this.m_items[phServer[i]]);
                        items[i].Value = pItemValues[i];
                        items[i].Quality = Quality.Bad;
                        items[i].QualitySpecified = false;
                        items[i].Timestamp = DateTime.MinValue;
                        items[i].TimestampSpecified = false;
                    }
                    pdwCancelID = this.AssignHandle();
                    IRequest request = null;
                    IdentifiedResult[] results = this.m_subscription.Write(items, (int) pdwCancelID, new WriteCompleteEventHandler(this.OnWriteComplete), out request);
                    if ((results == null) || (results.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    if (request != null)
                    {
                        this.m_requests[request] = dwTransactionID;
                    }
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(results);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void WriteVQT(int dwCount, int[] phServer, OPCITEMVQT[] pItemVQT, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if (((dwCount == 0) || (phServer == null)) || (pItemVQT == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                try
                {
                    ItemValue[] items = new ItemValue[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new ItemValue((ItemIdentifier) this.m_items[phServer[i]]);
                        items[i].Value = pItemVQT[i].vDataValue;
                        items[i].Quality = new Quality(pItemVQT[i].wQuality);
                        items[i].QualitySpecified = pItemVQT[i].bQualitySpecified != 0;
                        items[i].Timestamp = OpcCom.Interop.GetFILETIME(OpcCom.Da.Interop.Convert(pItemVQT[i].ftTimeStamp));
                        items[i].TimestampSpecified = pItemVQT[i].bTimeStampSpecified != 0;
                    }
                    IdentifiedResult[] results = this.m_subscription.Write(items);
                    if ((results == null) || (results.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(results);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public void WriteVQT(int dwCount, int[] phServer, OPCITEMVQT[] pItemVQT, int dwTransactionID, out int pdwCancelID, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (this.m_subscription == null)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                }
                if (((dwCount == 0) || (phServer == null)) || (pItemVQT == null))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147024809);
                }
                if (!base.IsConnected(typeof(IOPCDataCallback).GUID))
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(-2147220992);
                }
                try
                {
                    ItemValue[] items = new ItemValue[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new ItemValue((ItemIdentifier) this.m_items[phServer[i]]);
                        items[i].Value = pItemVQT[i].vDataValue;
                        items[i].Quality = new Quality(pItemVQT[i].wQuality);
                        items[i].QualitySpecified = pItemVQT[i].bQualitySpecified != 0;
                        items[i].Timestamp = OpcCom.Interop.GetFILETIME(OpcCom.Da.Interop.Convert(pItemVQT[i].ftTimeStamp));
                        items[i].TimestampSpecified = pItemVQT[i].bTimeStampSpecified != 0;
                    }
                    pdwCancelID = this.AssignHandle();
                    IRequest request = null;
                    IdentifiedResult[] results = this.m_subscription.Write(items, (int) pdwCancelID, new WriteCompleteEventHandler(this.OnWriteComplete), out request);
                    if ((results == null) || (results.Length != items.Length))
                    {
                        throw OpcCom.Da.Wrapper.Server.CreateException(-2147467259);
                    }
                    if (request != null)
                    {
                        this.m_requests[request] = dwTransactionID;
                    }
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(results);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Da.Wrapper.Server.CreateException(exception);
                }
            }
        }

        public string Name
        {
            get
            {
                lock (this)
                {
                    return this.m_name;
                }
            }
        }

        public int ServerHandle
        {
            get
            {
                lock (this)
                {
                    return this.m_serverHandle;
                }
            }
        }
    }
}

