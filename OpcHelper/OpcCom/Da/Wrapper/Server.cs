namespace Jund.OpcHelper.OpcCom.Da.Wrapper
{
    using Opc;
    using Opc.Da;
    using OpcCom;
    using OpcCom.Da;
    using OpcRcw.Comn;
    using OpcRcw.Da;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;
    using System.Threading;

    [CLSCompliant(false)]
    public class Server : ConnectionPointContainer, IOPCCommon, IOPCServer, IOPCBrowseServerAddressSpace, IOPCItemProperties, IOPCBrowse, IOPCItemIO, IOPCWrappedServer
    {
        private Stack m_browseStack = new Stack();
        private Hashtable m_continuationPoints = new Hashtable();
        private Hashtable m_groups = new Hashtable();
        private int m_lcid = 0x800;
        private int m_nextHandle = 1;
        private Opc.Da.IServer m_server = null;

        protected Server()
        {
            base.RegisterInterface(typeof(IOPCShutdown).GUID);
        }

        public void AddGroup(string szName, int bActive, int dwRequestedUpdateRate, int hClientGroup, IntPtr pTimeBias, IntPtr pPercentDeadband, int dwLCID, out int phServerGroup, out int pRevisedUpdateRate, ref Guid riid, out object ppUnk)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                SubscriptionState state = new SubscriptionState {
                    Name = szName,
                    ServerHandle = null,
                    ClientHandle = hClientGroup,
                    Active = bActive != 0,
                    Deadband = 0f,
                    KeepAlive = 0,
                    Locale = OpcCom.Interop.GetLocale(dwLCID),
                    UpdateRate = dwRequestedUpdateRate
                };
                if (pPercentDeadband != IntPtr.Zero)
                {
                    float[] destination = new float[1];
                    Marshal.Copy(pPercentDeadband, destination, 0, 1);
                    state.Deadband = destination[0];
                }
                DateTime now = DateTime.Now;
                int timebias = (int) -TimeZone.CurrentTimeZone.GetUtcOffset(now).TotalMinutes;
                if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(now))
                {
                    timebias += 60;
                }
                if (pTimeBias != IntPtr.Zero)
                {
                    timebias = Marshal.ReadInt32(pTimeBias);
                }
                Group group = this.CreateGroup(ref state, dwLCID, timebias);
                phServerGroup = group.ServerHandle;
                pRevisedUpdateRate = state.UpdateRate;
                ppUnk = group;
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        private void Browse(ItemIdentifier itemID, OPCBROWSETYPE dwBrowseFilterType, string szFilterCriteria, short vtDataTypeFilter, int dwAccessRightsFilter, ArrayList hits)
        {
            BrowseFilters filters = new BrowseFilters {
                MaxElementsReturned = 0,
                BrowseFilter = browseFilter.all,
                ElementNameFilter = (dwBrowseFilterType != OPCBROWSETYPE.OPC_FLAT) ? szFilterCriteria : "",
                VendorFilter = null,
                ReturnAllProperties = false
            };
            filters.PropertyIDs = new PropertyID[] { Property.DATATYPE, Property.ACCESSRIGHTS };
            filters.ReturnPropertyValues = true;
            BrowseElement[] elementArray = null;
            try
            {
                Opc.Da.BrowsePosition position = null;
                elementArray = this.m_server.Browse(itemID, filters, out position);
                if (position != null)
                {
                    position.Dispose();
                    position = null;
                }
            }
            catch
            {
                throw new ExternalException("BrowseOPCItemIDs", -2147467259);
            }
            foreach (BrowseElement element in elementArray)
            {
                if (dwBrowseFilterType == OPCBROWSETYPE.OPC_FLAT)
                {
                    if (element.HasChildren)
                    {
                        this.Browse(new ItemIdentifier(element.ItemName), dwBrowseFilterType, szFilterCriteria, vtDataTypeFilter, dwAccessRightsFilter, hits);
                    }
                }
                else
                {
                    if (dwBrowseFilterType == OPCBROWSETYPE.OPC_BRANCH)
                    {
                        if (element.HasChildren)
                        {
                            goto Label_00F9;
                        }
                        continue;
                    }
                    if ((dwBrowseFilterType == OPCBROWSETYPE.OPC_LEAF) && element.HasChildren)
                    {
                        continue;
                    }
                }
            Label_00F9:
                if (element.IsItem)
                {
                    if (vtDataTypeFilter != 0)
                    {
                        short type = (short) OpcCom.Interop.GetType((System.Type) element.Properties[0].Value);
                        if (type != vtDataTypeFilter)
                        {
                            continue;
                        }
                    }
                    if (dwAccessRightsFilter != 0)
                    {
                        accessRights rights = (accessRights) element.Properties[1].Value;
                        if (((dwAccessRightsFilter == 1) && (rights == accessRights.writable)) || ((dwAccessRightsFilter == 2) && (rights == accessRights.readable)))
                        {
                            continue;
                        }
                    }
                }
                if (dwBrowseFilterType != OPCBROWSETYPE.OPC_FLAT)
                {
                    hits.Add(element.Name);
                }
                else if (element.IsItem && ((szFilterCriteria.Length == 0) || Opc.Convert.Match(element.ItemName, szFilterCriteria, true)))
                {
                    hits.Add(element.ItemName);
                }
            }
        }

        public void Browse(string szItemID, ref IntPtr pszContinuationPoint, int dwMaxElementsReturned, OPCBROWSEFILTER dwBrowseFilter, string szElementNameFilter, string szVendorFilter, int bReturnAllProperties, int bReturnPropertyValues, int dwPropertyCount, int[] pdwPropertyIDs, out int pbMoreElements, out int pdwCount, out IntPtr ppBrowseElements)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                ItemIdentifier itemID = new ItemIdentifier(szItemID);
                BrowseFilters filters = new BrowseFilters {
                    MaxElementsReturned = dwMaxElementsReturned,
                    BrowseFilter = OpcCom.Da.Interop.GetBrowseFilter(dwBrowseFilter),
                    ElementNameFilter = szElementNameFilter,
                    VendorFilter = szVendorFilter,
                    ReturnAllProperties = bReturnAllProperties != 0,
                    ReturnPropertyValues = bReturnPropertyValues != 0,
                    PropertyIDs = OpcCom.Da.Interop.GetPropertyIDs(pdwPropertyIDs)
                };
                Opc.Da.BrowsePosition position = null;
                BrowseElement[] input = null;
                string key = null;
                if (pszContinuationPoint != IntPtr.Zero)
                {
                    key = Marshal.PtrToStringUni(pszContinuationPoint);
                }
                if ((key == null) || (key.Length == 0))
                {
                    input = this.m_server.Browse(itemID, filters, out position);
                }
                else
                {
                    ContinuationPoint point = (ContinuationPoint) this.m_continuationPoints[key];
                    if (point != null)
                    {
                        position = point.Position;
                        this.m_continuationPoints.Remove(key);
                    }
                    if (position == null)
                    {
                        throw new ExternalException("E_INVALIDCONTINUATIONPOINT", -1073478653);
                    }
                    Marshal.FreeCoTaskMem(pszContinuationPoint);
                    pszContinuationPoint = IntPtr.Zero;
                    position.MaxElementsReturned = dwMaxElementsReturned;
                    input = this.m_server.BrowseNext(ref position);
                }
                this.CleanupContinuationPoints();
                if (position != null)
                {
                    key = Guid.NewGuid().ToString();
                    this.m_continuationPoints[key] = new ContinuationPoint(position);
                    pszContinuationPoint = Marshal.StringToCoTaskMemUni(key);
                }
                if (pszContinuationPoint == IntPtr.Zero)
                {
                    pszContinuationPoint = Marshal.StringToCoTaskMemUni(string.Empty);
                }
                pbMoreElements = 0;
                pdwCount = 0;
                ppBrowseElements = IntPtr.Zero;
                if (input != null)
                {
                    pdwCount = input.Length;
                    ppBrowseElements = OpcCom.Da.Interop.GetBrowseElements(input, dwPropertyCount > 0);
                }
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        public void BrowseAccessPaths(string szItemID, out OpcRcw.Da.IEnumString ppIEnumString)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                throw new ExternalException("BrowseAccessPaths", -2147467263);
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        public void BrowseOPCItemIDs(OPCBROWSETYPE dwBrowseFilterType, string szFilterCriteria, short vtDataTypeFilter, int dwAccessRightsFilter, out OpcRcw.Da.IEnumString ppIEnumString)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                ItemIdentifier itemID = null;
                if (this.m_browseStack.Count > 0)
                {
                    itemID = (ItemIdentifier) this.m_browseStack.Peek();
                }
                ArrayList hits = new ArrayList();
                this.Browse(itemID, dwBrowseFilterType, szFilterCriteria, vtDataTypeFilter, dwAccessRightsFilter, hits);
                ppIEnumString = new OpcCom.Da.Wrapper.EnumString(hits);
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        private void BuildBrowseStack(ItemIdentifier itemID)
        {
            this.m_browseStack.Clear();
            this.BuildBrowseStack(null, itemID);
        }

        private bool BuildBrowseStack(ItemIdentifier itemID, ItemIdentifier targetID)
        {
            BrowseFilters filters = new BrowseFilters {
                MaxElementsReturned = 0,
                BrowseFilter = browseFilter.all,
                ElementNameFilter = null,
                VendorFilter = null,
                ReturnAllProperties = false,
                PropertyIDs = null,
                ReturnPropertyValues = false
            };
            BrowseElement[] elementArray = null;
            Opc.Da.BrowsePosition position = null;
            try
            {
                elementArray = this.m_server.Browse(itemID, filters, out position);
            }
            catch (Exception)
            {
                this.m_browseStack.Clear();
                return false;
            }
            if (position != null)
            {
                position.Dispose();
                position = null;
            }
            if ((elementArray == null) || (elementArray.Length == 0))
            {
                this.m_browseStack.Clear();
                return false;
            }
            foreach (BrowseElement element in elementArray)
            {
                if (element.ItemName == targetID.ItemName)
                {
                    return true;
                }
                if (targetID.ItemName.StartsWith(element.ItemName))
                {
                    ItemIdentifier identifier = new ItemIdentifier(targetID.ItemName);
                    this.m_browseStack.Push(identifier);
                    return this.BuildBrowseStack(identifier, targetID);
                }
            }
            return false;
        }

        public void ChangeBrowsePosition(OPCBROWSEDIRECTION dwBrowseDirection, string szString)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                BrowseElement element;
                BrowseFilters filters = new BrowseFilters {
                    MaxElementsReturned = 0,
                    BrowseFilter = browseFilter.all,
                    ElementNameFilter = null,
                    VendorFilter = null,
                    ReturnAllProperties = false,
                    PropertyIDs = null,
                    ReturnPropertyValues = false
                };
                ItemIdentifier itemID = null;
                Opc.Da.BrowsePosition position = null;
                switch (dwBrowseDirection)
                {
                    case OPCBROWSEDIRECTION.OPC_BROWSE_UP:
                        if (this.m_browseStack.Count == 0)
                        {
                            throw CreateException(-2147467259);
                        }
                        goto Label_0146;

                    case OPCBROWSEDIRECTION.OPC_BROWSE_DOWN:
                        if ((szString == null) || (szString.Length == 0))
                        {
                            throw CreateException(-2147024809);
                        }
                        goto Label_00F4;

                    case OPCBROWSEDIRECTION.OPC_BROWSE_TO:
                        if ((szString != null) && (szString.Length != 0))
                        {
                            break;
                        }
                        this.m_browseStack.Clear();
                        goto Label_0179;

                    default:
                        goto Label_0179;
                }
                itemID = new ItemIdentifier(szString);
                BrowseElement[] elementArray = null;
                try
                {
                    elementArray = this.m_server.Browse(itemID, filters, out position);
                }
                catch (Exception)
                {
                    throw CreateException(-2147024809);
                }
                if ((elementArray == null) || (elementArray.Length == 0))
                {
                    throw CreateException(-2147024809);
                }
                this.m_browseStack.Clear();
                this.m_browseStack.Push(null);
                this.m_browseStack.Push(itemID);
                goto Label_0179;
            Label_00F4:
                element = this.FindChild(szString);
                if ((element == null) || !element.HasChildren)
                {
                    throw CreateException(-2147024809);
                }
                this.m_browseStack.Push(new ItemIdentifier(element.ItemName));
                goto Label_0179;
            Label_0146:
                itemID = (ItemIdentifier) this.m_browseStack.Pop();
                if ((this.m_browseStack.Count > 0) && (this.m_browseStack.Peek() == null))
                {
                    this.BuildBrowseStack(itemID);
                }
            Label_0179:
                if (position != null)
                {
                    position.Dispose();
                    position = null;
                }
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        private void CleanupContinuationPoints()
        {
            ArrayList list = new ArrayList();
            foreach (DictionaryEntry entry in this.m_continuationPoints)
            {
                try
                {
                    ContinuationPoint point = entry.Value as ContinuationPoint;
                    if ((DateTime.UtcNow.Ticks - point.Timestamp.Ticks) > 0x165a0bc00L)
                    {
                        list.Add(entry.Key);
                    }
                }
                catch
                {
                    list.Add(entry.Key);
                }
            }
            foreach (string str in list)
            {
                ContinuationPoint point2 = (ContinuationPoint) this.m_continuationPoints[str];
                this.m_continuationPoints.Remove(str);
                point2.Position.Dispose();
            }
        }

        public static Exception CreateException(Exception e)
        {
            if (typeof(ExternalException).IsInstanceOfType(e))
            {
                return e;
            }
            if (typeof(ResultIDException).IsInstanceOfType(e))
            {
                return new ExternalException(e.Message, OpcCom.Interop.GetResultID(((ResultIDException) e).Result));
            }
            return new ExternalException(e.Message, -2147467259);
        }

        public static Exception CreateException(int code)
        {
            return new ExternalException(string.Format("0x{0:X8}", code), code);
        }

        internal Group CreateGroup(ref SubscriptionState state, int lcid, int timebias)
        {
            lock (this)
            {
                ISubscription subscription = this.m_server.CreateSubscription(state);
                state = subscription.GetState();
                if (state == null)
                {
                    throw CreateException(-2147467259);
                }
                if (this.m_groups.Contains(state.Name))
                {
                    this.m_server.CancelSubscription(subscription);
                    throw new ExternalException("E_DUPLICATENAME", -1073479668);
                }
                Group group = new Group(this, state.Name, ++this.m_nextHandle, lcid, timebias, subscription);
                this.m_groups[state.Name] = group;
                return group;
            }
        }

        public void CreateGroupEnumerator(OPCENUMSCOPE dwScope, ref Guid riid, out object ppUnk)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                switch (dwScope)
                {
                    case OPCENUMSCOPE.OPC_ENUM_PUBLIC_CONNECTIONS:
                    case OPCENUMSCOPE.OPC_ENUM_PUBLIC:
                        if (riid != typeof(OpcRcw.Comn.IEnumString).GUID)
                        {
                            if (riid != typeof(IEnumUnknown).GUID)
                            {
                                throw new ExternalException("E_NOINTERFACE", -2147467262);
                            }
                            ppUnk = new EnumUnknown(null);
                        }
                        else
                        {
                            ppUnk = new OpcCom.Da.Wrapper.EnumString(null);
                        }
                        return;
                }
                if (riid == typeof(IEnumUnknown).GUID)
                {
                    ppUnk = new EnumUnknown(this.m_groups);
                }
                else
                {
                    if (riid != typeof(OpcRcw.Comn.IEnumString).GUID)
                    {
                        throw new ExternalException("E_NOINTERFACE", -2147467262);
                    }
                    ArrayList strings = new ArrayList(this.m_groups.Count);
                    foreach (Group group in this.m_groups.Values)
                    {
                        strings.Add(group.Name);
                    }
                    ppUnk = new OpcCom.Da.Wrapper.EnumString(strings);
                }
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        private BrowseElement FindChild(string name)
        {
            ItemIdentifier itemID = null;
            if (this.m_browseStack.Count > 0)
            {
                itemID = (ItemIdentifier) this.m_browseStack.Peek();
            }
            BrowseElement[] elementArray = null;
            try
            {
                BrowseFilters filters = new BrowseFilters {
                    MaxElementsReturned = 0,
                    BrowseFilter = browseFilter.all,
                    ElementNameFilter = name,
                    VendorFilter = null,
                    ReturnAllProperties = false,
                    PropertyIDs = null,
                    ReturnPropertyValues = false
                };
                Opc.Da.BrowsePosition position = null;
                elementArray = this.m_server.Browse(itemID, filters, out position);
                if (position != null)
                {
                    position.Dispose();
                    position = null;
                }
            }
            catch (Exception)
            {
                return null;
            }
            if ((elementArray != null) && (elementArray.Length > 0))
            {
                return elementArray[0];
            }
            return null;
        }

        public void GetErrorString(int dwError, int dwLocale, out string ppString)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                ppString = this.m_server.GetErrorText(OpcCom.Interop.GetLocale(dwLocale), OpcCom.Interop.GetResultID(dwError));
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        public void GetGroupByName(string szName, ref Guid riid, out object ppUnk)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                foreach (Group group in this.m_groups.Values)
                {
                    if (group.Name == szName)
                    {
                        ppUnk = group;
                        return;
                    }
                }
                throw new ExternalException("E_INVALIDARG", -2147024809);
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        public void GetItemID(string szItemDataID, out string szItemID)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                if ((szItemDataID == null) || (szItemDataID.Length == 0))
                {
                    if (this.m_browseStack.Count == 0)
                    {
                        szItemID = "";
                    }
                    else
                    {
                        szItemID = ((ItemIdentifier) this.m_browseStack.Peek()).ItemName;
                    }
                }
                else if (this.IsItem(szItemDataID))
                {
                    szItemID = szItemDataID;
                }
                else
                {
                    BrowseElement element = this.FindChild(szItemDataID);
                    if (element == null)
                    {
                        throw CreateException(-2147024809);
                    }
                    szItemID = element.ItemName;
                }
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        public void GetItemProperties(string szItemID, int dwCount, int[] pdwPropertyIDs, out IntPtr ppvData, out IntPtr ppErrors)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                if ((dwCount == 0) || (pdwPropertyIDs == null))
                {
                    throw CreateException(-2147024809);
                }
                if ((szItemID == null) || (szItemID.Length == 0))
                {
                    throw CreateException(-1073479672);
                }
                ItemIdentifier[] itemIDs = new ItemIdentifier[] { new ItemIdentifier(szItemID) };
                PropertyID[] propertyIDs = new PropertyID[pdwPropertyIDs.Length];
                for (int i = 0; i < propertyIDs.Length; i++)
                {
                    propertyIDs[i] = OpcCom.Da.Interop.GetPropertyID(pdwPropertyIDs[i]);
                }
                ItemPropertyCollection[] propertysArray = this.m_server.GetProperties(itemIDs, propertyIDs, true);
                if ((propertysArray == null) || (propertysArray.Length != 1))
                {
                    throw CreateException(-2147467259);
                }
                if (propertysArray[0].ResultID.Failed())
                {
                    throw new ResultIDException(propertysArray[0].ResultID);
                }
                object[] values = new object[propertysArray[0].Count];
                for (int j = 0; j < propertysArray[0].Count; j++)
                {
                    ItemProperty property = propertysArray[0][j];
                    if (property.ResultID.Succeeded())
                    {
                        values[j] = OpcCom.Da.Interop.MarshalPropertyValue(property.ID, property.Value);
                    }
                }
                ppvData = OpcCom.Interop.GetVARIANTs(values, false);
                ppErrors = OpcCom.Da.Interop.GetHRESULTs((IResult[]) propertysArray[0].ToArray(typeof(IResult)));
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        public void GetLocaleID(out int pdwLcid)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                pdwLcid = this.m_lcid;
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        public void GetProperties(int dwItemCount, string[] pszItemIDs, int bReturnPropertyValues, int dwPropertyCount, int[] pdwPropertyIDs, out IntPtr ppItemProperties)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                if ((dwItemCount == 0) || (pszItemIDs == null))
                {
                    throw new ExternalException("E_INVALIDARG", -2147024809);
                }
                ppItemProperties = IntPtr.Zero;
                ItemIdentifier[] itemIDs = new ItemIdentifier[dwItemCount];
                for (int i = 0; i < dwItemCount; i++)
                {
                    itemIDs[i] = new ItemIdentifier(pszItemIDs[i]);
                }
                PropertyID[] propertyIDs = null;
                if ((dwPropertyCount > 0) && (pdwPropertyIDs != null))
                {
                    propertyIDs = OpcCom.Da.Interop.GetPropertyIDs(pdwPropertyIDs);
                }
                ItemPropertyCollection[] input = this.m_server.GetProperties(itemIDs, propertyIDs, bReturnPropertyValues != 0);
                if (input != null)
                {
                    ppItemProperties = OpcCom.Da.Interop.GetItemPropertyCollections(input);
                }
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        public void GetStatus(out IntPtr ppServerStatus)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                OPCSERVERSTATUS serverStatus = OpcCom.Da.Interop.GetServerStatus(this.m_server.GetStatus(), this.m_groups.Count);
                ppServerStatus = Marshal.AllocCoTaskMem(Marshal.SizeOf(serverStatus.GetType()));
                Marshal.StructureToPtr(serverStatus, ppServerStatus, false);
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        private bool IsItem(string name)
        {
            ItemIdentifier identifier = new ItemIdentifier(name);
            try
            {
                PropertyID[] propertyIDs = new PropertyID[] { Property.DATATYPE };
                ItemPropertyCollection[] propertysArray = this.m_server.GetProperties(new ItemIdentifier[] { identifier }, propertyIDs, false);
                if ((propertysArray == null) || (propertysArray.Length != 1))
                {
                    return false;
                }
                if (propertysArray[0].ResultID.Failed() || propertysArray[0][0].ResultID.Failed())
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual void Load(Guid clsid)
        {
        }

        public void LookupItemIDs(string szItemID, int dwCount, int[] pdwPropertyIDs, out IntPtr ppszNewItemIDs, out IntPtr ppErrors)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                if (((szItemID == null) || (szItemID.Length == 0)) || ((dwCount == 0) || (pdwPropertyIDs == null)))
                {
                    throw CreateException(-2147024809);
                }
                ItemIdentifier[] itemIDs = new ItemIdentifier[] { new ItemIdentifier(szItemID) };
                PropertyID[] propertyIDs = new PropertyID[pdwPropertyIDs.Length];
                for (int i = 0; i < propertyIDs.Length; i++)
                {
                    propertyIDs[i] = OpcCom.Da.Interop.GetPropertyID(pdwPropertyIDs[i]);
                }
                ItemPropertyCollection[] propertysArray = this.m_server.GetProperties(itemIDs, propertyIDs, false);
                if ((propertysArray == null) || (propertysArray.Length != 1))
                {
                    throw CreateException(-2147467259);
                }
                if (propertysArray[0].ResultID.Failed())
                {
                    throw new ResultIDException(propertysArray[0].ResultID);
                }
                string[] values = new string[propertysArray[0].Count];
                for (int j = 0; j < propertysArray[0].Count; j++)
                {
                    ItemProperty property = propertysArray[0][j];
                    if (property.ID.Code <= Property.EUINFO.Code)
                    {
                        property.ResultID = ResultID.Da.E_INVALID_PID;
                    }
                    if (property.ResultID.Succeeded())
                    {
                        values[j] = property.ItemName;
                    }
                }
                ppszNewItemIDs = OpcCom.Interop.GetUnicodeStrings(values);
                ppErrors = OpcCom.Da.Interop.GetHRESULTs((IResult[]) propertysArray[0].ToArray(typeof(IResult)));
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        void IOPCCommon.GetErrorString(int dwError, out string ppString)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                ppString = this.m_server.GetErrorText(this.m_server.GetLocale(), OpcCom.Interop.GetResultID(dwError));
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        public void QueryAvailableLocaleIDs(out int pdwCount, out IntPtr pdwLcid)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                pdwCount = 0;
                pdwLcid = IntPtr.Zero;
                string[] supportedLocales = this.m_server.GetSupportedLocales();
                if ((supportedLocales != null) && (supportedLocales.Length > 0))
                {
                    pdwLcid = Marshal.AllocCoTaskMem(supportedLocales.Length * Marshal.SizeOf(typeof(int)));
                    int[] source = new int[supportedLocales.Length];
                    for (int i = 0; i < supportedLocales.Length; i++)
                    {
                        source[i] = OpcCom.Interop.GetLocale(supportedLocales[i]);
                    }
                    Marshal.Copy(source, 0, pdwLcid, supportedLocales.Length);
                    pdwCount = supportedLocales.Length;
                }
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        public void QueryAvailableProperties(string szItemID, out int pdwCount, out IntPtr ppPropertyIDs, out IntPtr ppDescriptions, out IntPtr ppvtDataTypes)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                if ((szItemID == null) || (szItemID.Length == 0))
                {
                    throw new ExternalException("QueryAvailableProperties", -2147024809);
                }
                ItemIdentifier[] itemIDs = new ItemIdentifier[] { new ItemIdentifier(szItemID) };
                ItemPropertyCollection[] propertysArray = this.m_server.GetProperties(itemIDs, null, false);
                if ((propertysArray == null) || (propertysArray.Length != 1))
                {
                    throw new ExternalException("LookupItemIDs", -2147467259);
                }
                if (propertysArray[0].ResultID.Failed())
                {
                    throw new ResultIDException(propertysArray[0].ResultID);
                }
                int[] input = new int[propertysArray[0].Count];
                string[] values = new string[propertysArray[0].Count];
                short[] numArray2 = new short[propertysArray[0].Count];
                for (int i = 0; i < propertysArray[0].Count; i++)
                {
                    ItemProperty property = propertysArray[0][i];
                    if (property.ResultID.Succeeded())
                    {
                        input[i] = property.ID.Code;
                        PropertyDescription description = PropertyDescription.Find(property.ID);
                        if (description != null)
                        {
                            values[i] = description.Name;
                            numArray2[i] = (short) OpcCom.Interop.GetType(description.Type);
                        }
                    }
                }
                pdwCount = input.Length;
                ppPropertyIDs = OpcCom.Interop.GetInt32s(input);
                ppDescriptions = OpcCom.Interop.GetUnicodeStrings(values);
                ppvtDataTypes = OpcCom.Interop.GetInt16s(numArray2);
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        public void QueryOrganization(out OPCNAMESPACETYPE pNameSpaceType)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                pNameSpaceType = OPCNAMESPACETYPE.OPC_NS_HIERARCHIAL;
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        public void Read(int dwCount, string[] pszItemIDs, int[] pdwMaxAge, out IntPtr ppvValues, out IntPtr ppwQualities, out IntPtr ppftTimeStamps, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (((dwCount == 0) || (pszItemIDs == null)) || (pdwMaxAge == null))
                {
                    throw CreateException(-2147024809);
                }
                try
                {
                    Item[] items = new Item[dwCount];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i] = new Item(new ItemIdentifier(pszItemIDs[i]));
                        items[i].MaxAge = (pdwMaxAge[i] < 0) ? 0x7fffffff : pdwMaxAge[i];
                        items[i].MaxAgeSpecified = true;
                    }
                    ItemValueResult[] results = this.m_server.Read(items);
                    if ((results == null) || (results.Length != items.Length))
                    {
                        throw CreateException(-2147467259);
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
                    throw CreateException(exception);
                }
            }
        }

        public void RemoveGroup(int hServerGroup, int bForce)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                foreach (Group group in this.m_groups.Values)
                {
                    if (group.ServerHandle == hServerGroup)
                    {
                        this.m_groups.Remove(group.Name);
                        group.Dispose();
                        return;
                    }
                }
                throw new ExternalException("E_FAIL", -2147467259);
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        public void SetClientName(string szName)
        {
        }

        public int SetGroupName(string oldName, string newName)
        {
            lock (this)
            {
                Group group = (Group) this.m_groups[oldName];
                if (((newName == null) || (newName.Length == 0)) || (group == null))
                {
                    return -2147024809;
                }
                if (this.m_groups.Contains(newName))
                {
                    return -1073479668;
                }
                this.m_groups.Remove(oldName);
                this.m_groups[newName] = group;
                return 0;
            }
        }

        public void SetLocaleID(int dwLcid)
        {
            OpcCom.Da.Wrapper.Server server;
            Monitor.Enter(server = this);
            try
            {
                this.m_server.SetLocale(OpcCom.Interop.GetLocale(dwLcid));
                this.m_lcid = dwLcid;
            }
            catch (Exception exception)
            {
                throw CreateException(exception);
            }
            finally
            {
                Monitor.Exit(server);
            }
        }

        public virtual void Unload()
        {
        }

        public void WriteVQT(int dwCount, string[] pszItemIDs, OPCITEMVQT[] pItemVQT, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (((dwCount == 0) || (pszItemIDs == null)) || (pItemVQT == null))
                {
                    throw CreateException(-2147024809);
                }
                try
                {
                    ItemValue[] values = new ItemValue[dwCount];
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = new ItemValue(new ItemIdentifier(pszItemIDs[i]));
                        values[i].Value = pItemVQT[i].vDataValue;
                        values[i].Quality = new Quality(pItemVQT[i].wQuality);
                        values[i].QualitySpecified = pItemVQT[i].bQualitySpecified != 0;
                        values[i].Timestamp = OpcCom.Interop.GetFILETIME(OpcCom.Da.Interop.Convert(pItemVQT[i].ftTimeStamp));
                        values[i].TimestampSpecified = pItemVQT[i].bTimeStampSpecified != 0;
                    }
                    IdentifiedResult[] results = this.m_server.Write(values);
                    if ((results == null) || (results.Length != values.Length))
                    {
                        throw CreateException(-2147467259);
                    }
                    ppErrors = OpcCom.Da.Interop.GetHRESULTs(results);
                }
                catch (Exception exception)
                {
                    throw CreateException(exception);
                }
            }
        }

        public Opc.Da.IServer IServer
        {
            get
            {
                return this.m_server;
            }
            set
            {
                this.m_server = value;
            }
        }

        private class ContinuationPoint
        {
            public Opc.Da.BrowsePosition Position;
            public DateTime Timestamp = DateTime.UtcNow;

            public ContinuationPoint(Opc.Da.BrowsePosition position)
            {
                this.Position = position;
            }
        }
    }
}

