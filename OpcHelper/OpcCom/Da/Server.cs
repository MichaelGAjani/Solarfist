namespace Jund.OpcHelper.OpcCom.Da
{
    using Opc;
    using Opc.Da;
    using OpcCom;
    using OpcRcw.Da;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Server : OpcCom.Server, Opc.Da.IServer, Opc.IServer, IDisposable
    {
        protected int m_filters;
        private Hashtable m_subscriptions;

        internal Server()
        {
            this.m_filters = 9;
            this.m_subscriptions = new Hashtable();
        }

        internal Server(URL url, object server)
        {
            this.m_filters = 9;
            this.m_subscriptions = new Hashtable();
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            base.m_url = (URL) url.Clone();
            base.m_server = server;
        }

        public virtual BrowseElement[] Browse(ItemIdentifier itemID, BrowseFilters filters, out Opc.Da.BrowsePosition position)
        {
            if (filters == null)
            {
                throw new ArgumentNullException("filters");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                position = null;
                int pdwCount = 0;
                int pbMoreElements = 0;
                IntPtr zero = IntPtr.Zero;
                IntPtr ppBrowseElements = IntPtr.Zero;
                try
                {
                    ((IOPCBrowse) base.m_server).Browse(((itemID != null) && (itemID.ItemName != null)) ? itemID.ItemName : "", ref zero, filters.MaxElementsReturned, OpcCom.Da.Interop.GetBrowseFilter(filters.BrowseFilter), (filters.ElementNameFilter != null) ? filters.ElementNameFilter : "", (filters.VendorFilter != null) ? filters.VendorFilter : "", filters.ReturnAllProperties ? 1 : 0, filters.ReturnPropertyValues ? 1 : 0, (filters.PropertyIDs != null) ? filters.PropertyIDs.Length : 0, OpcCom.Da.Interop.GetPropertyIDs(filters.PropertyIDs), out pbMoreElements, out pdwCount, out ppBrowseElements);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCBrowse.Browse", exception);
                }
                BrowseElement[] elements = OpcCom.Da.Interop.GetBrowseElements(ref ppBrowseElements, pdwCount, true);
                string continuationPoint = Marshal.PtrToStringUni(zero);
                Marshal.FreeCoTaskMem(zero);
                if ((pbMoreElements != 0) || ((continuationPoint != null) && (continuationPoint != "")))
                {
                    position = new OpcCom.Da.BrowsePosition(itemID, filters, continuationPoint);
                }
                this.ProcessResults(elements, filters.PropertyIDs);
                return elements;
            }
        }

        public virtual BrowseElement[] BrowseNext(ref Opc.Da.BrowsePosition position)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if ((position == null) || (position.GetType() != typeof(OpcCom.Da.BrowsePosition)))
                {
                    throw new BrowseCannotContinueException();
                }
                OpcCom.Da.BrowsePosition position2 = (OpcCom.Da.BrowsePosition) position;
                if (((position2 == null) || (position2.ContinuationPoint == null)) || (position2.ContinuationPoint == ""))
                {
                    throw new BrowseCannotContinueException();
                }
                int pdwCount = 0;
                int pbMoreElements = 0;
                ItemIdentifier itemID = ((OpcCom.Da.BrowsePosition) position).ItemID;
                BrowseFilters filters = ((OpcCom.Da.BrowsePosition) position).Filters;
                IntPtr pszContinuationPoint = Marshal.StringToCoTaskMemUni(position2.ContinuationPoint);
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCBrowse) base.m_server).Browse(((itemID != null) && (itemID.ItemName != null)) ? itemID.ItemName : "", ref pszContinuationPoint, filters.MaxElementsReturned, OpcCom.Da.Interop.GetBrowseFilter(filters.BrowseFilter), (filters.ElementNameFilter != null) ? filters.ElementNameFilter : "", (filters.VendorFilter != null) ? filters.VendorFilter : "", filters.ReturnAllProperties ? 1 : 0, filters.ReturnPropertyValues ? 1 : 0, (filters.PropertyIDs != null) ? filters.PropertyIDs.Length : 0, OpcCom.Da.Interop.GetPropertyIDs(filters.PropertyIDs), out pbMoreElements, out pdwCount, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCBrowse.BrowseNext", exception);
                }
                BrowseElement[] elements = OpcCom.Da.Interop.GetBrowseElements(ref zero, pdwCount, true);
                position2.ContinuationPoint = Marshal.PtrToStringUni(pszContinuationPoint);
                Marshal.FreeCoTaskMem(pszContinuationPoint);
                if ((pbMoreElements == 0) && ((position2.ContinuationPoint == null) || (position2.ContinuationPoint == "")))
                {
                    position = null;
                }
                this.ProcessResults(elements, filters.PropertyIDs);
                return elements;
            }
        }

        public void CancelSubscription(ISubscription subscription)
        {
            if (subscription == null)
            {
                throw new ArgumentNullException("subscription");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                if (!typeof(OpcCom.Da.Subscription).IsInstanceOfType(subscription))
                {
                    throw new ArgumentException("Incorrect object type.", "subscription");
                }
                SubscriptionState state = subscription.GetState();
                if (!this.m_subscriptions.ContainsKey(state.ServerHandle))
                {
                    throw new ArgumentException("Handle not found.", "subscription");
                }
                this.m_subscriptions.Remove(state.ServerHandle);
                subscription.Dispose();
                try
                {
                    ((IOPCServer) base.m_server).RemoveGroup((int) state.ServerHandle, 0);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCServer.RemoveGroup", exception);
                }
            }
        }

        protected object ChangeType(object source, System.Type type, string locale)
        {
            object obj3;
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);
            }
            catch
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            }
            try
            {
                object obj2 = Opc.Convert.ChangeType(source, type);
                if ((typeof(float) == type) && float.IsInfinity(System.Convert.ToSingle(obj2)))
                {
                    throw new OverflowException();
                }
                obj3 = obj2;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }
            return obj3;
        }

        public ISubscription CreateSubscription(SubscriptionState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                SubscriptionState state2 = (SubscriptionState) state.Clone();
                Guid gUID = typeof(IOPCItemMgt).GUID;
                object ppUnk = null;
                int phServerGroup = 0;
                int pRevisedUpdateRate = 0;
                GCHandle handle = GCHandle.Alloc(state2.Deadband, GCHandleType.Pinned);
                try
                {
                    ((IOPCServer) base.m_server).AddGroup((state2.Name != null) ? state2.Name : "", state2.Active ? 1 : 0, state2.UpdateRate, 0, IntPtr.Zero, handle.AddrOfPinnedObject(), OpcCom.Interop.GetLocale(state2.Locale), out phServerGroup, out pRevisedUpdateRate, ref gUID, out ppUnk);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCServer.AddGroup", exception);
                }
                finally
                {
                    if (handle.IsAllocated)
                    {
                        handle.Free();
                    }
                }
                try
                {
                    int pdwRevisedKeepAliveTime = 0;
                    ((IOPCGroupStateMgt2) ppUnk).SetKeepAlive(state2.KeepAlive, out pdwRevisedKeepAliveTime);
                    state2.KeepAlive = pdwRevisedKeepAliveTime;
                }
                catch
                {
                    state2.KeepAlive = 0;
                }
                state2.ServerHandle = phServerGroup;
                if (pRevisedUpdateRate > state2.UpdateRate)
                {
                    state2.UpdateRate = pRevisedUpdateRate;
                }
                OpcCom.Da.Subscription subscription = this.CreateSubscription(ppUnk, state2, this.m_filters);
                this.m_subscriptions[phServerGroup] = subscription;
                return subscription;
            }
        }

        protected virtual OpcCom.Da.Subscription CreateSubscription(object group, SubscriptionState state, int filters)
        {
            return new OpcCom.Da.Subscription(group, state, filters);
        }

        public override void Dispose()
        {
            lock (this)
            {
                if (base.m_server != null)
                {
                    foreach (OpcCom.Da.Subscription subscription in this.m_subscriptions.Values)
                    {
                        subscription.Dispose();
                        try
                        {
                            SubscriptionState state = subscription.GetState();
                            ((IOPCServer) base.m_server).RemoveGroup((int) state.ServerHandle, 0);
                        }
                        catch
                        {
                        }
                    }
                    this.m_subscriptions.Clear();
                    OpcCom.Interop.ReleaseServer(base.m_server);
                    base.m_server = null;
                }
            }
        }

        public override string GetErrorText(string locale, ResultID resultID)
        {
            string str2;
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                try
                {
                    string ppString = null;
                    ((IOPCServer) base.m_server).GetErrorString(resultID.Code, OpcCom.Interop.GetLocale(locale), out ppString);
                    str2 = ppString;
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCServer.GetErrorString", exception);
                }
            }
            return str2;
        }

        public virtual ItemPropertyCollection[] GetProperties(ItemIdentifier[] itemIDs, PropertyID[] propertyIDs, bool returnValues)
        {
            if (itemIDs == null)
            {
                throw new ArgumentNullException("itemIDs");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                string[] pszItemIDs = new string[itemIDs.Length];
                for (int i = 0; i < itemIDs.Length; i++)
                {
                    pszItemIDs[i] = itemIDs[i].ItemName;
                }
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCBrowse) base.m_server).GetProperties(itemIDs.Length, pszItemIDs, returnValues ? 1 : 0, (propertyIDs != null) ? propertyIDs.Length : 0, OpcCom.Da.Interop.GetPropertyIDs(propertyIDs), out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCBrowse.GetProperties", exception);
                }
                ItemPropertyCollection[] propertysArray = OpcCom.Da.Interop.GetItemPropertyCollections(ref zero, itemIDs.Length, true);
                if ((propertyIDs != null) && (propertyIDs.Length > 0))
                {
                    foreach (ItemPropertyCollection propertys in propertysArray)
                    {
                        for (int j = 0; j < propertys.Count; j++)
                        {
                            propertys[j].ID = propertyIDs[j];
                        }
                    }
                }
                return propertysArray;
            }
        }

        public int GetResultFilters()
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                return this.m_filters;
            }
        }

        public ServerStatus GetStatus()
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCServer) base.m_server).GetStatus(out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCServer.GetStatus", exception);
                }
                return OpcCom.Da.Interop.GetServerStatus(ref zero, true);
            }
        }

        private void ProcessResults(BrowseElement[] elements, PropertyID[] propertyIDs)
        {
            if (elements != null)
            {
                foreach (BrowseElement element in elements)
                {
                    if (element.Properties != null)
                    {
                        foreach (ItemProperty property in element.Properties)
                        {
                            if (propertyIDs != null)
                            {
                                foreach (PropertyID yid in propertyIDs)
                                {
                                    if (property.ID.Code == yid.Code)
                                    {
                                        property.ID = yid;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public virtual ItemValueResult[] Read(Item[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                int length = items.Length;
                if (length == 0)
                {
                    throw new ArgumentOutOfRangeException("items.Length", "0");
                }
                string[] pszItemIDs = new string[length];
                int[] pdwMaxAge = new int[length];
                for (int i = 0; i < length; i++)
                {
                    pszItemIDs[i] = items[i].ItemName;
                    pdwMaxAge[i] = items[i].MaxAgeSpecified ? items[i].MaxAge : 0;
                }
                IntPtr zero = IntPtr.Zero;
                IntPtr ppwQualities = IntPtr.Zero;
                IntPtr ppftTimeStamps = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCItemIO) base.m_server).Read(length, pszItemIDs, pdwMaxAge, out zero, out ppwQualities, out ppftTimeStamps, out ppErrors);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCItemIO.Read", exception);
                }
                object[] objArray = OpcCom.Interop.GetVARIANTs(ref zero, length, true);
                short[] numArray2 = OpcCom.Interop.GetInt16s(ref ppwQualities, length, true);
                DateTime[] timeArray = OpcCom.Interop.GetFILETIMEs(ref ppftTimeStamps, length, true);
                int[] numArray3 = OpcCom.Interop.GetInt32s(ref ppErrors, length, true);
                string locale = this.GetLocale();
                ItemValueResult[] resultArray = new ItemValueResult[length];
                for (int j = 0; j < resultArray.Length; j++)
                {
                    resultArray[j] = new ItemValueResult(items[j]);
                    resultArray[j].ServerHandle = null;
                    resultArray[j].Value = objArray[j];
                    resultArray[j].Quality = new Quality(numArray2[j]);
                    resultArray[j].QualitySpecified = true;
                    resultArray[j].Timestamp = timeArray[j];
                    resultArray[j].TimestampSpecified = timeArray[j] != DateTime.MinValue;
                    resultArray[j].ResultID = OpcCom.Interop.GetResultID(numArray3[j]);
                    resultArray[j].DiagnosticInfo = null;
                    if (numArray3[j] == -1073479674)
                    {
                        resultArray[j].ResultID = new ResultID(ResultID.Da.E_WRITEONLY, -1073479674L);
                    }
                    if ((resultArray[j].Value != null) && (items[j].ReqType != null))
                    {
                        try
                        {
                            resultArray[j].Value = this.ChangeType(objArray[j], items[j].ReqType, locale);
                        }
                        catch (Exception exception2)
                        {
                            resultArray[j].Value = null;
                            resultArray[j].Quality = Quality.Bad;
                            resultArray[j].QualitySpecified = true;
                            resultArray[j].Timestamp = DateTime.MinValue;
                            resultArray[j].TimestampSpecified = false;
                            if (exception2.GetType() == typeof(OverflowException))
                            {
                                resultArray[j].ResultID = OpcCom.Interop.GetResultID(-1073479669);
                            }
                            else
                            {
                                resultArray[j].ResultID = OpcCom.Interop.GetResultID(-1073479676);
                            }
                        }
                    }
                    if ((this.m_filters & 1) == 0)
                    {
                        resultArray[j].ItemName = null;
                    }
                    if ((this.m_filters & 2) == 0)
                    {
                        resultArray[j].ItemPath = null;
                    }
                    if ((this.m_filters & 4) == 0)
                    {
                        resultArray[j].ClientHandle = null;
                    }
                    if ((this.m_filters & 8) == 0)
                    {
                        resultArray[j].Timestamp = DateTime.MinValue;
                        resultArray[j].TimestampSpecified = false;
                    }
                }
                return resultArray;
            }
        }

        public void SetResultFilters(int filters)
        {
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                this.m_filters = filters;
            }
        }

        public virtual IdentifiedResult[] Write(ItemValue[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            lock (this)
            {
                if (base.m_server == null)
                {
                    throw new NotConnectedException();
                }
                int length = items.Length;
                if (length == 0)
                {
                    throw new ArgumentOutOfRangeException("items.Length", "0");
                }
                string[] pszItemIDs = new string[length];
                for (int i = 0; i < length; i++)
                {
                    pszItemIDs[i] = items[i].ItemName;
                }
                OPCITEMVQT[] oPCITEMVQTs = OpcCom.Da.Interop.GetOPCITEMVQTs(items);
                IntPtr zero = IntPtr.Zero;
                try
                {
                    ((IOPCItemIO) base.m_server).WriteVQT(length, pszItemIDs, oPCITEMVQTs, out zero);
                }
                catch (Exception exception)
                {
                    throw OpcCom.Interop.CreateException("IOPCItemIO.Read", exception);
                }
                int[] numArray = OpcCom.Interop.GetInt32s(ref zero, length, true);
                IdentifiedResult[] resultArray = new IdentifiedResult[length];
                for (int j = 0; j < length; j++)
                {
                    resultArray[j] = new IdentifiedResult(items[j]);
                    resultArray[j].ServerHandle = null;
                    resultArray[j].ResultID = OpcCom.Interop.GetResultID(numArray[j]);
                    resultArray[j].DiagnosticInfo = null;
                    if (numArray[j] == -1073479674)
                    {
                        resultArray[j].ResultID = new ResultID(ResultID.Da.E_READONLY, -1073479674L);
                    }
                    if ((this.m_filters & 1) == 0)
                    {
                        resultArray[j].ItemName = null;
                    }
                    if ((this.m_filters & 2) == 0)
                    {
                        resultArray[j].ItemPath = null;
                    }
                    if ((this.m_filters & 4) == 0)
                    {
                        resultArray[j].ClientHandle = null;
                    }
                }
                return resultArray;
            }
        }
    }
}

